using Godot;
using System;
using System.Collections.Genric

public class World : Spatial
{
	[Export] // Makes it so we can see in Editor
	public PackedScene Map;
	
	private List<Cell> cells = new List<Cell>();
	
	public override void _Ready()
	{
		 var environment = GetTree().Root.World.FallbackEnvironment;
		environment.BackgroundMode = Environment.BgColor;
		environment.BackgroundColor = new Color(0, 0, 0); // Black color
		environment.AmbientLightColor = new Color(0.263f, 0.176f, 0.427f); // Equivalent to Color("432d6d")
		environment.DofBlurFarEnabled = true;
		environment.DofBlurNearEnabled = true;
		
		GenerateMap(); //Call Map Generation
	}
	 
	private void GenerateMap()
	{
		if (!(Map is PackedScene)) return;
		
		var map = (PackedScene)Map.Instance();
		var tileMap = map.GetNode<TileMap>("TileMap");
		
		var used Tiles = tileMap.GetUsedCells();
		map.QueueFree();
		
		foreach (var tile in usedTiles)
		{
			var cell = (Cell)GD.Load<PackedScene>("res://Cell.tscn").Instance();
				AddChild(cell);
				cell.Translation = new Vector3(tile.x * Globals.GRID_SIZE, 0, tile.y * Globals.GRID_SIZE);
				cells.Add(cell);
		}
		
		foreach (var cell in cells)
		{
			cell.UpdateFaces(usedTiles);
		}
}
