using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public class FloorBuilder : Spatial
{
	[Export]
	public PackedScene Map; // The main map scene to generate the level

	[Export]
	public Array<PackedScene> Rooms; // Array of available room scenes

	[Export]
	public Array<PackedScene> Hallways; // Array of available hallway scenes

	[Export]
	public int MaxRooms = 5; // Maximum number of rooms allowed to spawn

	private int roomsSpawned = 0; // Tracks the number of rooms spawned
	private List<Cell> cells = new List<Cell>(); // List to store generated cells
	private TileMap tileMap; // The tile map to place cells on
	private Random random = new Random(); // Random number generator for placement

	// Called when the node is added to the scene tree
	public override void _Ready()
	{
		// Seed the random number generator for map generation
		GD.Randomize();

		// Set the environment background and lighting
		var environment = GetTree().Root.World.FallbackEnvironment as Godot.Environment;
		if (environment != null)
		{
			environment.BackgroundColor = new Color(0, 0, 0);
			environment.AmbientLightColor = new Color(0.263f, 0.176f, 0.427f);
			environment.DofBlurFarEnabled = true;
			environment.DofBlurNearEnabled = true;
		}

		// Start generating the map with a specified map creator
		GenerateMap("MapCreator");
	}

	// Generates the initial map by placing the first room and hallways
	private void GenerateMap(string i)
	{
		if (Map == null) return;

		// Instance the map and attach it to the scene
		var mapInstance = Map.Instance();
		AddChild(mapInstance);
		var mapScript = mapInstance as Map;

		// Get the tile map from the map instance
		tileMap = mapScript.GetTileMap(i);
		if (tileMap == null)
		{
			GD.PrintErr("TileMap not found in Map node.");
			mapInstance.QueueFree();
			return;
		}

		Array<Vector2> usedTiles = new Array<Vector2>();
		GenerateInitialRoom(); // Generate the first room

		// Add used cells from tileMap to usedTiles array for later reference
		foreach (var vector in tileMap.GetUsedCells())
		{
			usedTiles.Add((Vector2)vector);
		}

		// Create cells at each tile position in usedTiles and add to the scene
		foreach (Vector2 tile in usedTiles)
		{
			var cell = (Cell)GD.Load<PackedScene>("res://Cell.tscn").Instance();
			AddChild(cell);
			cell.Translation = new Vector3(tile.x * Globals.GRID_SIZE, 0, tile.y * Globals.GRID_SIZE);
			cells.Add(cell);
		}

		// Update faces for each cell based on usedTiles for collision/visuals
		foreach (var cell in cells)
		{
			cell.UpdateFaces(usedTiles);
		}

		// Clean up the initial map instance
		mapInstance.QueueFree();
	}

	// Generates the initial room and places the first hallway leading from it
	private void GenerateInitialRoom()
	{
		HashSet<Vector2> occupiedPositions = new HashSet<Vector2>();

		// Place the initial room if below maximum room count
		if (roomsSpawned < MaxRooms)
		{
			var roomScene = Rooms[(int)GD.RandRange(0, Rooms.Count)];
			var roomInstance = roomScene.Instance();

			// Get the TileMap for the room instance
			var roomTileMap = roomInstance as TileMap ?? roomInstance.GetNode<TileMap>("TileMap");

			if (roomTileMap == null)
			{
				GD.PrintErr($"TileMap not found in {roomScene.ResourcePath}");
				roomInstance.QueueFree();
				return;
			}

			// Random position for the initial room
			Vector2 roomPosition = new Vector2((int)GD.RandRange(0, 10), (int)GD.RandRange(0, 10));
			foreach (Vector2 roomCell in roomTileMap.GetUsedCells())
			{
				var globalPosition = roomPosition + roomCell;
				tileMap.SetCellv(globalPosition, roomTileMap.GetCellv(roomCell));
				occupiedPositions.Add(globalPosition);
			}
			roomInstance.QueueFree();
			roomsSpawned++;

			// Start the first hallway from this initial room
			PlaceHallwayAndNewRoom(roomPosition, roomTileMap, Hallways, Rooms, occupiedPositions);
		}
	}

	// Places a hallway connected to the starting room and a new room at the end of the hallway
	private void PlaceHallwayAndNewRoom(Vector2 startingRoomPosition, TileMap roomTileMap, Array<PackedScene> hallwayScenes, Array<PackedScene> roomScenes, HashSet<Vector2> occupiedPositions)
	{
		if (roomsSpawned >= MaxRooms) return;

		// Define potential exit points for hallways around the room
		var edgePositions = new Vector2[]
		{
			startingRoomPosition + new Vector2(roomTileMap.GetUsedRect().Size.x + 1, 0), // Right
			startingRoomPosition + new Vector2(-1, 0),                                  // Left
			startingRoomPosition + new Vector2(0, roomTileMap.GetUsedRect().Size.y + 1), // Bottom
			startingRoomPosition + new Vector2(0, -1)                                    // Top
		};

		foreach (Vector2 edgePosition in edgePositions)
		{
			if (!occupiedPositions.Contains(edgePosition))
			{
				// Randomly select a hallway and instance it
				var hallwayScene = hallwayScenes[(int)GD.RandRange(0, hallwayScenes.Count)];
				var hallwayInstance = hallwayScene.Instance() as Node2D;

				var hallwayTileMap = hallwayInstance.GetNode<TileMap>("TileMap");

				if (hallwayTileMap == null)
				{
					GD.PrintErr($"TileMap not found in {hallwayScene.ResourcePath}");
					hallwayInstance.QueueFree();
					continue;
				}

				// Apply a random rotation (0, 90, 180, 270 degrees) to align hallway
				int rotationDegrees = ((int)GD.RandRange(0, 4)) * 90;
				hallwayInstance.RotationDegrees = rotationDegrees;

				// Calculate hallway cell positions based on rotation
				foreach (Vector2 hallwayCell in hallwayTileMap.GetUsedCells())
				{
					Vector2 rotatedPosition = RotateVector(hallwayCell, rotationDegrees);
					var globalPosition = edgePosition + rotatedPosition;
					if (!occupiedPositions.Contains(globalPosition))
					{
						tileMap.SetCellv(globalPosition, hallwayTileMap.GetCellv(hallwayCell));
						occupiedPositions.Add(globalPosition);
					}
				}
				AddChild(hallwayInstance);

				// Place a new room at the end of the rotated hallway
				PlaceNewRoomAtHallwayEnd(edgePosition + RotateVector(hallwayTileMap.GetUsedRect().Size, rotationDegrees), roomScenes, occupiedPositions);
				break; // Exit after placing one hallway to connect to one new room
			}
		}
	}

	// Helper function to rotate a Vector2 by specified degrees (90, 180, 270)
	private Vector2 RotateVector(Vector2 vector, int degrees)
	{
		switch (degrees)
		{
			case 90:
				return new Vector2(-vector.y, vector.x);
			case 180:
				return new Vector2(-vector.x, -vector.y);
			case 270:
				return new Vector2(vector.y, -vector.x);
			default:
				return vector; // 0 degrees, no rotation
		}
	}

	// Places a new room at the end of a hallway, connecting it to the layout
	private void PlaceNewRoomAtHallwayEnd(Vector2 position, Array<PackedScene> roomScenes, HashSet<Vector2> occupiedPositions)
	{
		if (roomsSpawned >= MaxRooms) return;

		// Select a random room from available room scenes
		var roomScene = roomScenes[(int)GD.RandRange(0, roomScenes.Count)];
		var roomInstance = roomScene.Instance();

		var roomTileMap = roomInstance as TileMap ?? roomInstance.GetNode<TileMap>("TileMap");

		if (roomTileMap == null)
		{
			GD.PrintErr($"TileMap not found in {roomScene.ResourcePath}");
			roomInstance.QueueFree();
			return;
		}

		// Place room cells at the specified position if they’re not occupied
		foreach (Vector2 roomCell in roomTileMap.GetUsedCells())
		{
			var globalPosition = position + roomCell;
			if (!occupiedPositions.Contains(globalPosition))
			{
				tileMap.SetCellv(globalPosition, roomTileMap.GetCellv(roomCell));
				occupiedPositions.Add(globalPosition);
			}
		}
		roomInstance.QueueFree();
		roomsSpawned++;

		// Extend the layout by placing additional hallways from the new room
		PlaceHallwayAndNewRoom(position, roomTileMap, Hallways, Rooms, occupiedPositions);
	}
}
