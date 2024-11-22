using Godot;
using System;
using System.Linq;

public class Cell : Area
{
	private MeshInstance topFace;
	private MeshInstance northFace;
	private MeshInstance eastFace;
	private MeshInstance southFace;
	private MeshInstance westFace;
	private MeshInstance bottomFace;
	
	[Export] public float spawnProbabilityA = 0.2f;
	[Export] public float spawnProbabilityB = 0.2f;  // % of Spawn
 	[Export] public PackedScene interactableA;
	[Export] public PackedScene interactableB;
	
	
	 private static Random random = new Random();

	
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
		// Spawn Interactable A
		if (random.NextDouble() < spawnProbabilityA && interactableA != null)
		{
			SpawnInteractable(interactableA, new Vector3(0, 1, 0)); // Adjust position as needed
		}

		// Spawn Interactable B
		if (random.NextDouble() < spawnProbabilityB && interactableB != null)
		{
			SpawnInteractable(interactableB, new Vector3(1, 1, 0)); // Adjust position as needed
		}
	}

	private void SpawnInteractable(PackedScene interactable, Vector3 position)
	{
		Spatial interactableInstance = (Spatial)interactable.Instance();
		AddChild(interactableInstance);
		interactableInstance.Translation = position;
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
