using Godot;
using System;

public class Map : Node2D
{
	public TileMap GetTileMap(string i)
	{
		return GetNode<TileMap>(i);
		//returns tilemap of name i in the the Map family, ie MapCreator and DevRoom
	}
}
