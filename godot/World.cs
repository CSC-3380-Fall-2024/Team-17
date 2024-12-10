
using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public class World : Spatial
{
	[Export] 
	public PackedScene Map;
	
	private List<Cell> cells = new List<Cell>();
	
	private int this_floor = 0;

	protected enum Floor {
		BELOW,
		HERE,
		ABOVE
	}

	public override void _Ready()
	{
		var environment = GetTree().Root.World.FallbackEnvironment as Godot.Environment;
		if (environment != 	null)
		environment.BackgroundColor = new Color(0, 0, 0); 
		environment.AmbientLightColor = new Color(0.263f, 0.176f, 0.427f); 
		environment.DofBlurFarEnabled = true;
		environment.DofBlurNearEnabled = true;
		
		ConfigFile dbgFile = new ConfigFile();
		dbgFile.Load("res://Config/debug.cfg");
		string mapName = dbgFile.GetValue("FLOOR_SETTINGS", "MAP_NAME") as string;
		GD.Print(mapName);
		if(mapName.Equals("random"))
		{
			GenerateMap();
		}
		else 
		{
			GenerateMap("res://DefaultFloors/" + mapName); 
		}
	}
	
	private void GenerateMap()
	{
		var mapInstance = GetNode<Node2D>("Map"); 
		TileMap tileMap = mapInstance.GetChild(0) as TileMap;
		Array<Vector2> usedTiles = new Array<Vector2>();
		foreach (var vector in tileMap.GetUsedCells())
		{
			usedTiles.Add((Vector2)vector);
		}
	
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
	
	private void GenerateMap(string i)//need to specify the tilemap but only if it is related to the Map node
	{
		// if (Map == null) return;
		
		// var mapInstance = Map.Instance(); 
		// AddChild(mapInstance);
		// var mapScript = mapInstance as Map;
		// var tileMap = mapScript.GetTileMap(i);
		var mapScene = ResourceLoader.Load<PackedScene>(i).Instance();
		TileMap tileMap = mapScene as TileMap;
		Array<Vector2> usedTiles = new Array<Vector2>();
		foreach (var vector in tileMap.GetUsedCells())
		{
			usedTiles.Add((Vector2)vector);
		}
	
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
