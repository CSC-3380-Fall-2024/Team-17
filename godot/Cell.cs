using Godot;
using System;
using System.Collections.Generic;

public class Cell : Area
{
	private MeshInstance topFace;
	private MeshInstance northFace;
	private MeshInstance eastFace;
	private MeshInstance southFace;
	private MeshInstance westFace;
	private MeshInstance bottomFace;
	private bool hasSpawned = false;

	[Export] public float spawnProbabilityA = 0.2f;
	[Export] public float spawnProbabilityB = 0.2f; // % Chance of Spawn
	[Export] public PackedScene interactableA;
	[Export] public PackedScene interactableB;
	[Export] public Godot.Collections.Array<Vector3> spawnPoints = new Godot.Collections.Array<Vector3>(); // List of spawn points

	private static Random random = new Random();
	private HashSet<Vector3> usedSpawnPoints = new HashSet<Vector3>();  // Track used spawn points

	public override void _Ready()
	{
		topFace = GetNode<MeshInstance>("TopFace");
		northFace = GetNode<MeshInstance>("NorthFace");
		eastFace = GetNode<MeshInstance>("EastFace");
		southFace = GetNode<MeshInstance>("SouthFace");
		westFace = GetNode<MeshInstance>("WestFace");
		bottomFace = GetNode<MeshInstance>("BottomFace");

		SpawnInteractable();
	}

	private void SpawnInteractable()
	{
		GD.Print("SpawnInteractable called");

		if (hasSpawned)
		{
			GD.Print("SpawnInteractable skipped because it already ran for this cell");
			return;  // Skip spawning if already done
		}

		hasSpawned = true; // Mark this cell as having spawned an interactable

		// Spawn Interactable A
		if (random.NextDouble() < spawnProbabilityA && interactableA != null)
		{
			Vector3 spawnPoint = GetRandomSpawnPoint();  // Get the random spawn point for A
			if (spawnPoint != Vector3.Zero)  // Make sure it's a valid point
			{
				GD.Print($"Spawning Interactable A at {spawnPoint}");
				SpawnInteractable(interactableA, spawnPoint);
			}
		}

		// Spawn Interactable B
		if (random.NextDouble() < spawnProbabilityB && interactableB != null)
		{
			Vector3 spawnPoint = GetRandomSpawnPoint();  // Get the random spawn point for B
			if (spawnPoint != Vector3.Zero)  // Make sure it's a valid point
			{
				GD.Print($"Spawning Interactable B at {spawnPoint}");
				SpawnInteractable(interactableB, spawnPoint);
			}
		}
	}

	private void SpawnInteractable(PackedScene interactable, Vector3 position)
	{
		if (usedSpawnPoints.Contains(position))
		{
			GD.Print($"Spawn skipped at used point: {position}");
			return;  // Skip spawning if position has already been used
		}

		Spatial interactableInstance = (Spatial)interactable.Instance();
		interactableInstance.GlobalTransform = GlobalTransform.Translated(position);
		AddChild(interactableInstance);

		usedSpawnPoints.Add(position);  // Add the position to used spawn points
		GD.Print($"Spawned {interactableInstance.Name} at {position}");
	}

	private Vector3 GetRandomSpawnPoint()
	{
		if (spawnPoints.Count > 0)
		{
			// Pick a random index and check if it's already used
			Vector3 spawnPoint = spawnPoints[random.Next(spawnPoints.Count)];
			GD.Print($"Selected Spawn Point: {spawnPoint}");

			// If the spawn point has already been used, try again
			if (usedSpawnPoints.Contains(spawnPoint))
			{
				GD.Print($"Spawn point {spawnPoint} already used. Choosing a different point.");
				return Vector3.Zero;  // Return a "zero" point to indicate failure
			}

			return spawnPoint;
		}

		GD.Print("Default Spawn Point Used");
		return Translation + new Vector3(0, 1, 0);
	}

	public void UpdateFaces(Godot.Collections.Array<Vector2> cellList)
	{
		// Get grid position
		Vector2 myGridPosition = new Vector2(Translation.x / Globals.GRID_SIZE, Translation.z / 2);

		// Hide specific faces based on neighbors
		if (cellList.Contains(myGridPosition + Vector2.Right))
		{
			eastFace.QueueFree();
		}
		if (cellList.Contains(myGridPosition + Vector2.Left))
		{
			westFace.QueueFree();
		}
		if (cellList.Contains(myGridPosition + Vector2.Down))
		{
			southFace.QueueFree();
		}
		if (cellList.Contains(myGridPosition + Vector2.Up))
		{
			northFace.QueueFree();
		}
	}
}
