using Godot;
using System;

public class SpawnableCell : Spatial
{
	[Export] public float SpawnChance = 0.1f;  // 10% spawn chance
	private bool _isObjectSpawned = false;

	public override void _Ready()
	{
		// Attempt to spawn an object when this cell is created
		TrySpawnObject();
	}

	private void TrySpawnObject()
	{
		// Generate a random value between 0 and 1
		Random random = new Random();
		float randomValue = (float)random.NextDouble();

		// Check if the random value is within the spawn chance threshold
		if (randomValue < SpawnChance)
		{
			SpawnObject();
		}
	}

	private void SpawnObject()
	{
		// Load the interactable object scene
		PackedScene interactableScene = (PackedScene)ResourceLoader.Load("res://InteractableA.tscn");

		if (interactableScene != null)
		{
			// Instance the object and add it as a child of the cell
			Spatial interactableInstance = (Spatial)interactableScene.Instance();
			AddChild(interactableInstance);
		}
		else
		{
			GD.PrintErr("Failed to load interactable object scene.");
		}
	}
}
