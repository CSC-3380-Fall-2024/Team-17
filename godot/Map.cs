using Godot;
using System;

public class Map : Node2D
{
	public TileMap GetTileMap()
	{
		return GetNode<TileMap>("MapCreator");
	}
}
