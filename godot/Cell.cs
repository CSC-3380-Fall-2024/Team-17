using Godot;
using System;

public class Cell : Area
{
	private MeshInstance topFace;
	private MeshInstance northFace;
	private MeshInstance eastFace;
	private MeshInstance southFace;
	private MeshInstance westFace;
	private MeshInstance bottomFace;
	
	public override void _Ready()
	{
		topFace = GetNode<MeshInstance>("TopFace");
		northFace = GetNode<MeshInstance>("NorthFace");
		eastFace = GetNode<MeshInstance>("EastFace");
		southFace = GetNode<MeshInstance>("SouthFace");
		westFace = GetNode<MeshInstance>("WestFace");
		bottomFace = GetNode<MeshInstance>("BottomFace");
	}

	public void UpdateFaces(Godot.Collections.Dictionary<Vector2, Cell> cellList)
	{
		Vector2 myGridPosition = new Vector2(Translation.x / Globals.GRID_SIZE, Translation.z / 2);

		if (cellList.ContainsKey(myGridPosition + Vector2.Right))
		{
			eastFace.QueueFree();
		}
		if (cellList.ContainsKey(myGridPosition + Vector2.Left))
		{
			westFace.QueueFree();
		}
		if (cellList.ContainsKey(myGridPosition + Vector2.Down))
		{
			southFace.QueueFree();
		}
		if (cellList.ContainsKey(myGridPosition + Vector2.Up))
		{
			northFace.QueueFree();
		}
	}
}
