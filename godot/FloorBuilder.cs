using Godot;
using System;
using System.Collections.Generic;

public class FloorBuilder : Spatial
{
	[Export] public Vector2 GridSize = new Vector2(20, 20); // Floor grid size
	[Export] public float RoomSpacing = 4f;  // Spacing between rooms
	private Random random = new Random();

	// List of room scene paths
	private PackedScene[] roomScenes = {
		// Put the name of the pre made floor map in here when made
		
		GD.Load<PackedScene>("res://Rooms/RoomA.tscn"),
		GD.Load<PackedScene>("res://Rooms/RoomB.tscn"),
		GD.Load<PackedScene>("res://Rooms/RoomC.tscn")
		
	};

	private bool[,] floorGrid;  // 2D array to track grid occupancy

	public override void _Ready()
	{
		//doesnt work for now so its commented out
		//floorGrid = new bool[(int)GridSize.x, (int)GridSize.y];
		//GenerateFloor();
	}

	private void GenerateFloor()
	{
		// Start from a random position on the grid
		Vector2 startPosition = new Vector2(
			random.Next((int)GridSize.x),
			random.Next((int)GridSize.y)
		);

		PlaceRoom(startPosition);

		// Repeat room placement based on grid size and available cells
		for (int i = 0; i < GridSize.x * GridSize.y; i++)
		{
			Vector2 nextPosition = FindNextAvailablePosition();
			if (nextPosition != Vector2.Zero)  // Ensure position is available
			{
				PlaceRoom(nextPosition);
			}
		}
	}

	private void PlaceRoom(Vector2 gridPosition)
	{
		PackedScene roomScene = roomScenes[random.Next(roomScenes.Length)];
		Spatial roomInstance = roomScene.Instance<Spatial>();

		// Set the room's position in the world based on the grid position
		Vector3 worldPosition = new Vector3(
			gridPosition.x * RoomSpacing,
			0,
			gridPosition.y * RoomSpacing
		);
		roomInstance.Translation = worldPosition;

		// Mark grid as occupied and add room to scene tree
		AddChild(roomInstance);
		floorGrid[(int)gridPosition.x, (int)gridPosition.y] = true;

		GD.Print($"Placed room at {gridPosition}");
	}

	private Vector2 FindNextAvailablePosition()
	{
		List<Vector2> availablePositions = new List<Vector2>();

		// Check for empty adjacent cells to already placed rooms
		for (int x = 0; x < GridSize.x; x++)
		{
			for (int y = 0; y < GridSize.y; y++)
			{
				if (!floorGrid[x, y] && HasAdjacentRoom(new Vector2(x, y)))
				{
					availablePositions.Add(new Vector2(x, y));
				}
			}
		}

		// Randomly choose an available position, if any
		if (availablePositions.Count > 0)
		{
			return availablePositions[random.Next(availablePositions.Count)];
		}
		return Vector2.Zero;  // No available positions found
	}

	private bool HasAdjacentRoom(Vector2 gridPosition)
	{
		Vector2[] directions = { Vector2.Up, Vector2.Down, Vector2.Left, Vector2.Right };

		foreach (Vector2 direction in directions)
		{
			Vector2 adjacentPosition = gridPosition + direction;

			if (IsWithinGrid(adjacentPosition) && floorGrid[(int)adjacentPosition.x, (int)adjacentPosition.y])
			{
				return true;
			}
		}
		return false;
	}

	private bool IsWithinGrid(Vector2 position)
	{
		return position.x >= 0 && position.y >= 0 &&
			   position.x < GridSize.x && position.y < GridSize.y;
	}
}
