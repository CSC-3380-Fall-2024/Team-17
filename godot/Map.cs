using Godot;
using System;

public class Map : Node2D
{
	static void GetTileMap()
	{
		return Godot.find_node("MapCreator")
	}
}
