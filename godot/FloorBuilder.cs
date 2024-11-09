using Godot;
using System;
using System.Collections.Generic;

public class FloorBuilder : Node
{
	[Export] private PackedScene cellScene;
	[Export] private PackedScene[] roomPrefabs;
	[Export] private int maxRooms = 5;
	[Export] private int gridWidth = 20;
	[Export] private int gridHeight = 20;
	[Export] private PackedScene playerScene;
	private Spatial player;

	private RandomNumberGenerator rng = new RandomNumberGenerator();
	private HashSet<Vector2> occupiedPositions = new HashSet<Vector2>();
	private List<Cell> cellInstances = new List<Cell>();
	private List<Vector2> cellPositions = new List<Vector2>();

	public override void _Ready()
	{
		rng.Randomize();
		ConfigureEnvironment();

		Vector2 startPosition = Vector2.Zero;
		GenerateDungeon(startPosition);
		
		SpawnPlayer(startPosition);
		var cellPositionsArray = new Godot.Collections.Array<Vector2>(cellPositions);
		foreach (var cell in cellInstances)
		{
			UpdateAllCells();
		}

		
	}

	void SpawnPlayer(Vector2 startPosition)
	{
		if (playerScene == null)
		{
			GD.PrintErr("Player scene is not assigned.");
			return;
		}

		player = playerScene.Instance() as Spatial;
		if (player == null)
		{
			GD.PrintErr("Failed to instantiate player. Ensure the root node of 'playerScene' is Spatial.");
			return;
		}

		player.Translation = new Vector3(startPosition.x * 64, 0, startPosition.y * 64);
		AddChild(player);
	}

	void ConfigureEnvironment()
	{
		var environment = GetTree().Root.World.FallbackEnvironment as Godot.Environment;
		if (environment != null)
		{
			environment.BackgroundColor = new Color(0, 0, 0);
			environment.AmbientLightColor = new Color(0.263f, 0.176f, 0.427f);
			environment.DofBlurFarEnabled = true;
			environment.DofBlurNearEnabled = true;
		}
	}

	Vector2 GetPlayerPosition()
	{
		var player = GetNode<Node2D>("res://Player.tscn");
		if (player != null)
		{
			float tileSize = 1.0f;
			return new Vector2(
				Mathf.Floor(player.Position.x / tileSize),
				Mathf.Floor(player.Position.y / tileSize)
			);
		}
		return new Vector2(gridWidth / 2, gridHeight / 2);
	}

	void GenerateDungeon(Vector2 startPosition)
	{
		Vector2 startRoomSize = PlaceRoom(startPosition);
		int roomsCreated = 1;
		Queue<Tuple<Vector2, Vector2>> expansionPoints = new Queue<Tuple<Vector2, Vector2>>();
		expansionPoints.Enqueue(new Tuple<Vector2, Vector2>(startPosition, startRoomSize));

		while (roomsCreated < maxRooms && expansionPoints.Count > 0)
		{
			var (currentPos, currentRoomSize) = expansionPoints.Dequeue();
			foreach (Vector2 direction in GetRandomDirections())
			{
				Vector2 roomExitPosition = GetExitPosition(currentPos, currentRoomSize, direction);
				Vector2 newRoomPosition = roomExitPosition + direction * 5;
				if (!IsWithinBounds(newRoomPosition) || !IsAreaFree(newRoomPosition, new Vector2(5, 5)))
					continue;
				if (roomsCreated < maxRooms && rng.Randf() < 1f)
				{
					Vector2 newRoomSize = PlaceRoom(newRoomPosition);
					roomsCreated++;
					expansionPoints.Enqueue(new Tuple<Vector2, Vector2>(newRoomPosition, newRoomSize));
					GD.Print($"Room added at {newRoomPosition}");
					DrawConnection(roomExitPosition, newRoomPosition);
				}
			}
		}
		var cellPositionsArray = new Godot.Collections.Array<Vector2>(cellPositions);
		foreach (var cell in cellInstances)
		{
			cell.UpdateFaces(cellPositionsArray);
		}
		GD.Print($"Rooms created: {roomsCreated}");
	}

	Vector2 PlaceRoom(Vector2 position)
	{
		PackedScene roomPrefab = roomPrefabs[rng.RandiRange(0, roomPrefabs.Length - 1)];
		TileMap roomTileMap = roomPrefab.Instance() as TileMap;

		if (roomTileMap == null)
		{
			GD.PrintErr("The room prefab is not a TileMap. Ensure all roomPrefabs are of type TileMap.");
			return Vector2.Zero;
		}

		AddChild(roomTileMap);
		Rect2 usedRect = roomTileMap.GetUsedRect();
		Vector2 roomSize = usedRect.Size;
		float cellSize = 1.0f;
		Vector2 worldPosition = position - (roomSize * cellSize * 1f);
		roomTileMap.Position = worldPosition;
		MarkAreaOccupied(position, roomSize);
		ConvertTileMapTo3D(roomTileMap, position);
		return roomSize;
	}

	void DrawConnection(Vector2 start, Vector2 end)
	{
		Vector2 currentPosition = start;
		Vector2 direction = (end - start).Normalized();
		while (currentPosition.DistanceTo(end) > 1.0f)
		{
			if (!occupiedPositions.Contains(currentPosition))
			{
				PlaceConnectionCell(currentPosition);
				occupiedPositions.Add(currentPosition);
			}
			currentPosition += direction;
		}
	}

	void PlaceConnectionCell(Vector2 position)
	{
		if (cellScene != null)
		{
			var cellInstance = cellScene.Instance() as Cell;
			if (cellInstance == null)
			{
				GD.PrintErr("The cellScene prefab is not a Cell. Ensure cellScene is of type Cell.");
				return;
			}
			cellInstance.Translation = new Vector3(position.x, 0, position.y);
			AddChild(cellInstance);
			cellInstances.Add(cellInstance);
			cellPositions.Add(position);
		}
	}

	void ConvertTileMapTo3D(TileMap tileMap, Vector2 gridPosition)
	{
		Vector2 mapSize = tileMap.GetUsedRect().Size;
		Vector3 offset = new Vector3(gridPosition.x, 0, gridPosition.y);
		float cellSize = 1.0f;

		for (int x = 0; x < mapSize.x; x++)
		{
			for (int y = 0; y < mapSize.y; y++)
			{
				Vector2 cellPos = new Vector2(x, y);
				int tileId = tileMap.GetCellv(cellPos);

				if (tileId != -1)
				{
					Vector3 position = offset + new Vector3(cellPos.x * cellSize, 0, cellPos.y * cellSize);
					if (cellScene != null)
					{
						var cellInstance = cellScene.Instance() as Cell;
						if (cellInstance == null)
						{
							GD.PrintErr("The cellScene prefab is not a Cell. Ensure cellScene is of type Cell.");
							continue;
						}
						cellInstance.Translation = position;
						AddChild(cellInstance);
						cellInstances.Add(cellInstance);
						cellPositions.Add(new Vector2(gridPosition.x + cellPos.x, gridPosition.y + cellPos.y));
					}
				}
			}
		}
		tileMap.QueueFree();
	}

	void UpdateAllCells()
	{
		var cellPositionsArray = new Godot.Collections.Array<Vector2>(cellPositions);
		foreach (var cell in cellInstances)
		{
			cell.UpdateFaces(cellPositionsArray);
		}
	}

	List<Vector2> GetRandomDirections()
	{
		List<Vector2> directions = new List<Vector2> { Vector2.Up, Vector2.Down, Vector2.Left, Vector2.Right };
		for (int i = 0; i < directions.Count; i++)
		{
			int j = rng.RandiRange(0, directions.Count - 1);
			var temp = directions[i];
			directions[i] = directions[j];
			directions[j] = temp;
		}
		return directions;
	}

	bool IsWithinBounds(Vector2 position)
	{
		return position.x >= 0 && position.x < gridWidth &&
			   position.y >= 0 && position.y < gridHeight;
	}

	bool IsAreaFree(Vector2 startPosition, Vector2 size)
	{
		for (int x = 0; x < size.x; x++)
		{
			for (int y = 0; y < size.y; y++)
			{
				Vector2 pos = startPosition + new Vector2(x, y);
				if (occupiedPositions.Contains(pos))
					return false;
			}
		}
		return true;
	}

	void MarkAreaOccupied(Vector2 startPosition, Vector2 size)
	{
		for (int x = 0; x < size.x; x++)
		{
			for (int y = 0; y < size.y; y++)
			{
				occupiedPositions.Add(startPosition + new Vector2(x, y));
			}
		}
	}

	Vector2 GetExitPosition(Vector2 roomPosition, Vector2 roomSize, Vector2 direction)
	{
		return roomPosition + (roomSize * 1f) * direction;
	}
}
