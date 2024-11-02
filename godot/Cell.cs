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
	
	[Export] public float spawnProbability = 0.2f;  // % of Spawn
 	[Export] public PackedScene interactable;
	
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
		// Generate a random value between 0 and 1, and spawn the interactable if within probability
		if (random.NextDouble() < spawnProbability && interactable != null)
		{
			Spatial interactableInstance = (Spatial)interactable.Instance();
			AddChild(interactableInstance);
			interactableInstance.Translation = new Vector3(0, 1, 0); // Adjust position as needed
		}
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
