using Godot;
using System;
using Array = Godot.Collections.Array;

public class TileSetUpdater : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		string name = "DevRoom";
		TileMap orgMap = ResourceLoader.Load<PackedScene>($"res://DefaultFloors/{name}.tscn").Instance<TileMap>();
		TileMap retMap = orgMap;
		retMap.Clear();
		retMap.Name = name;
		retMap.TileSet = ResourceLoader.Load<TileSet>("res://default_tileset.tres");
		Array tiles = orgMap.GetUsedCells();
		foreach (Vector2 v in tiles)
		{
			retMap.SetCell((int)v.x, (int)v.y, 9);
		}
		PackedScene overwrite = new PackedScene();
		overwrite.Pack(retMap);
		ResourceSaver.Save($"res://DefaultFloors/{name + "1"}.tscn", overwrite);
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
