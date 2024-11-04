
using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class World : Spatial
{
	[Export] 
	public PackedScene Map;
	
	private List<Cell> cells = new List<Cell>();

	
	public override void _Ready()
	{
		var environment = GetTree().Root.World.FallbackEnvironment as Godot.Environment;
		if (environment != null)
		environment.BackgroundColor = new Color(0, 0, 0); 
		environment.AmbientLightColor = new Color(0.263f, 0.176f, 0.427f); 
		environment.DofBlurFarEnabled = true;
		environment.DofBlurNearEnabled = true;
		
		GenerateMap("DevRoom"); 
	}
	 
	private void GenerateMap(string i)//need to specify the tilemap but only if it is related to the Map node
	{
		if (Map == null) return;
		
		var mapInstance = Map.Instance(); 
		AddChild(mapInstance);
		var mapScript = mapInstance as Map;
		var tileMap = mapScript.GetTileMap(i); 
		Array<Vector2> usedTiles = new Array<Vector2>();
		foreach (var vector in tileMap.GetUsedCells())
		{
			usedTiles.Add((Vector2)vector);
		}

		mapInstance.QueueFree();
	
		foreach (Vector2 tile in usedTiles)
		{
			var cell = (Cell)GD.Load<PackedScene>("res://Cell.tscn").Instance();
			AddChild(cell);
			cell.Translation = new Vector3(tile.x * Globals.GRID_SIZE, 0, tile.y * Globals.GRID_SIZE);
			cells.Add(cell);
			GD.Print($"Cell created at: {cell.Translation}");
		}


		foreach (var cell in cells)
		{
			cell.UpdateFaces(usedTiles);
		}
	}
}
