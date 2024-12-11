using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;


public class MapCreator : TileMap
{
	private List<Vector2> tilePositions = new List<Vector2>();	

	private Random rand = new Random();

	public override void _Ready()
	{
		Godot.Collections.Dictionary<int, TileMap> dict = new Godot.Collections.Dictionary<int, TileMap>();
		Directory dir = new Directory();
		dir.Open("res://Rooms/");
		dir.ListDirBegin(true);
		if (!dir.DirExists("res://Rooms/"))
		{
			GD.PrintErr("Error, directory does not exist");
		}
		String currentFile = dir.GetNext();
		// GD.Print("\"" + currentFile + "\"");
		int i = 0;
		while(!currentFile.Equals(""))
		{
			Node newMapInstance = ResourceLoader.Load<PackedScene>("res://Rooms/" + currentFile).Instance();
			TileMap newMap = newMapInstance.GetChild(0) as TileMap;
			newMapInstance.QueueFree();
			newMap.Visible = false;
			GD.Print(currentFile);
			GD.Print(newMap.GetUsedCells());
			dict.Add(i, newMap);
			
			i++;
			currentFile = dir.GetNext();
		};

		//the following code is temporary and probably won't be used later (this behavior should be relegated to a helper function)
		tilePositions.RemoveAll(allVectors);
		Godot.Collections.Array newCells = dict[rand.Next(dict.Keys.Count)].GetUsedCells();
		GD.Print(newCells);
		foreach(Vector2 v in newCells)
		{
			tilePositions.Add(v);
		}
		//end temp code
		this.Clear();
		foreach(Vector2 v in tilePositions)
		{
			this.SetCell((int)v.x, (int)v.y, 1 /* THIS IS TEMPORARY AND I NEED TO MAKE A DICT FROM Vector2 TO int IN ORDER TO GRAB TILE INDICES */);
		}
	}

	protected List<Vector2> V2Rotate90(List<Vector2> list)
	{
		List<Vector2> rlist = list;
		int XMax = 0;
		foreach (Vector2 v in rlist)
		{
			if (v.x > XMax) XMax = (int)v.x;
		}
		for (int i = 0; i < rlist.Count; i++)
		{
			int swap = (int) rlist[i].x;
			rlist[i] = new Vector2((-1) * rlist[i].y - XMax, rlist[i].x);
		}

		return rlist;
	}

	private bool allVectors(Vector2 v){
		return true;
	}


	// //a method to hold different starting rooms shapes
	// private void GenerateStartingRoom()
	// {
	// 	Random random = new Random();
		 
	// 	List<Vector2> roomTiles = new List<Vector2>();


	// 	var StartingRoom = new Godot.Collections.Dictionary {
	// 		{0, new Godot.Collections.Dictionary{ 
	// 			{"tilePositions", new List<Vector2>{
	// 				new Vector2(0, 0), new Vector2(1,0), new Vector2(2,0),
	// 				new Vector2(0, 1), new Vector2(1,1), new Vector2(2,1),
	// 				new Vector2(0, 2), new Vector2(1,2), new Vector2(2,2),
	// 								   new Vector2(1,2)}
	// 				},
	// 			{"extendPoints", new List<Vector2>{new Vector2(1,-1)}

	// 			}

	// 			}


	// 		},
	// 		{1, new Godot.Collections.Dictionary{
	// 			{"tilePositions", new List<Vector2>{
	// 				new Vector2(0,0), new Vector2(0,1), new Vector2(0,2)
	// 			}
	// 		},
	// 			{"extendPoints", new List<Vector2>{new Vector2(0,-1)}

	// 			}
	// 		}
	// 		},
	// 		{2, new Godot.Collections.Dictionary{
	// 			{"tilePositions", new List<Vector2>{
	// 				new Vector2(2,0), new Vector2(0,1), new Vector2(1,1), new Vector2(2,1), new Vector2(2,2), new Vector2(2,3)

	// 			}
	// 		},
	// 			{"extendPoints", new List<Vector2>{new Vector2(-1,1), new Vector2(2,-1)}

	// 			}
	// 		}
	// 		}
			


	// 	};
		
	// 	 //get positions from dictionary'
	// 	 if StartingRoom.ContainsKey(int roomSelction = random.Next(0, 3)) {
	// 		roomTiles = StartingRoom[int.Parse(roomSelction.ToString())]["tilePositions"] as List<Vector2>;
	// 	}
	// 	else
	// 	{
	// 		GD.Print("Error in generating starting room");
	// 	}
		
		



	// }

	// private void GenerateBigRoom()
	// {
	// 	Random random = new Random();
		 
	// 	List<Vector2> roomTiles = new List<Vector2>();


	// 	var BigRoom = new Godot.Collections.Dictionary {
	// 		{0, new Godot.Collections.Dictionary{ 
	// 			{"tilePositions", new List<Vector2>{
	// 				new Vector2(2, 0), new Vector2(3,0), new Vector2(4,0),
	// 				new Vector2(0, 1), new Vector2(1,1), new Vector2(2,1), new Vector2(3,1), new Vector2(4,1),
	// 				new Vector2(0, 2), new Vector2(1,2), new Vector2(2,2), new Vector2(3,2), new Vector2(4,2),
	// 				new Vector2(0, 3), new Vector2(1,3), new Vector2(2,3), new Vector2(3,3), new Vector2(4,3),
	// 				new Vector2(0, 4), new Vector2(1,4), new Vector2(2,4), new Vector2(3,4), new Vector2(4,4)
	// 				}
	// 				},
	// 			{"extendPoints", new List<Vector2>{new Vector2(2,5), new Vector2(5,2)}

	// 			}

	// 			}


	// 		},
	// 		{1, new Godot.Collections.Dictionary{
	// 			{"tilePositions", new List<Vector2>{
	// 				new Vector2(0,0), new Vector2(0,1), new Vector2(0,2),
	// 				new Vector2(1,0), new Vector2(1,1), new Vector2(1,2),
	// 				new Vector2(2,0), new Vector2(2,1), new Vector2(2,2)
	// 			}
	// 		},
	// 			{"extendPoints", new List<Vector2>{new Vector2(1,3)}

	// 			}
	// 		}
	// 		},
	// 		{2, new Godot.Collections.Dictionary{
	// 			{"tilePositions", new List<Vector2>{
	// 				new Vector2(0,0), new Vector2(0,1), new Vector2(0,2),
	// 				new Vector2(1,0), new Vector2(1,1), new Vector2(1,2),
	// 				new Vector2(2,0), new Vector2(2,1), new Vector2(2,2),
	// 				new Vector2(3,0), new Vector2(3,1), new Vector2(3,2)
	// 			}
	// 		},
	// 			{"extendPoints", new List<Vector2>{new Vector2(-1,3), new Vector2(3,1)}

	// 			}
	// 		}
	// 		}
			


	// 	};
		
	// 	 //get positions from dictionary'
	// 	 if BigRoom.ContainsKey(int roomSelction = random.Next(0, 3)) {
	// 		roomTiles = StartingRoom[int.Parse(roomSelction.ToString())]["tilePositions"] as List<Vector2>;
	// 	}
	// 	else
	// 	{
	// 		GD.Print("Error in generating Big rooms");
	// 	}

	// }
	// private void GenerateFinalRoom()
	// {
	// 	Random random = new Random();
		 
	// 	List<Vector2> roomTiles = new List<Vector2>();


	// 	var FinalRoom = new Godot.Collections.Dictionary {
	// 		{0, new Godot.Collections.Dictionary{ 
	// 			{"tilePositions", new List<Vector2>{
	// 				new Vector2(0, 0),  new Vector2(1,0), new Vector2(2, 0), new Vector2(3,0), new Vector2(4,0),
	// 				new Vector2(0, 1), new Vector2(1,1), new Vector2(2,1), new Vector2(3,1), new Vector2(4,1), new Vector2(5,1),
	// 				new Vector2(0, 2), new Vector2(1,2), new Vector2(2,2), new Vector2(3,2), new Vector2(4,2), new Vector2(5,2), new Vector2(6,2),
	// 				new Vector2(0, 3), new Vector2(1,3), new Vector2(2,3), new Vector2(3,3), new Vector2(4,3), new Vector2(5,3), 
	// 				new Vector2(0, 4), new Vector2(1,4), new Vector2(2,4), new Vector2(3,4), new Vector2(4,4)
	// 				}
	// 				},
	// 			{"extendPoints", new List<Vector2>{new Vector2(2,-1), new Vector2(-1,-1), new Vector2(1,5)}

	// 			}
				


	// 		},
	// 		{1, new Godot.Collections.Dictionary{
	// 			{"tilePositions", new List<Vector2>{
	// 													new Vector2(2,0),
	// 				new Vector2(1,1), new Vector2(2,1), new Vector2(3,1), new Vector2(4,1), new Vector2(5,1), new Vector2(6,1), new Vector2(7,1),
	// 				new Vector2(1,2), new Vector2(2,2), new Vector2(3,2), new Vector2(4,2), new Vector2(5,2), new Vector2(6,2), new Vector2(7,2),
	// 				new Vector2(0,3), new Vector2(1,3), new Vector2(2,3), new Vector2(3,3), new Vector2(4,3), new Vector2(5,3), new Vector2(6,3), new Vector2(7,3),
	// 				new Vector2(1,4), new Vector2(2,4), new Vector2(3,4), new Vector2(4,4), new Vector2(5,4), new Vector2(6,4), new Vector2(7,4),
	// 																										  new Vector2(6,5)

	// 			}
	// 		},
	// 			{"extendPoints", new List<Vector2>{new Vector2(-1,2), new Vector2(2,-1)}

	// 			}
	// 		}
	// 		}
			
			


	// 	};
		
	// 	 //get positions from dictionary'
	// 	 if BigRoom.ContainsKey(int roomSelction = random.Next(0, 2)) {
	// 		roomTiles = StartingRoom[int.Parse(roomSelction.ToString())]["tilePositions"] as List<Vector2>;
	// 	}
	// 	else
	// 	{
	// 		GD.Print("Error in generating Final room");
	// 	}
	

}

class Room
{
	TileMap Map;
	Vector2[] bounds = new Vector2[2];
	Vector2[] connectors = new Vector2[0];
	bool[] conReady = new bool[0];

	protected Room(TileMap map)
	{
		Map = map;
		bounds[0] = new Vector2(Int32.MaxValue, Int32.MaxValue);
		bounds[1] = new Vector2(Int32.MinValue, Int32.MinValue);
		foreach(Vector2 v in Map.GetUsedCells())
		{
			if(Map.GetCellv(v) == 14)
			{
				connectors.Append(v);
				conReady.Append(true);
				continue;
			}
			if (v.x < bounds[0].x) bounds[0].x = v.x;
			if (v.x > bounds[1].x) bounds[1].x = v.x;
			if (v.y < bounds[0].y) bounds[0].y = v.y;
			if (v.y > bounds[1].y) bounds[1].y = v.y;
		}
		sortConnectors(connectors);
	}
	protected TileMap GetMap()
	{
		return this.Map;
	}
	protected Vector2[] GetBounds()
	{
		return bounds;
	}
	protected Vector2[] GetConnectors()
	{
		return connectors;
	}
	protected bool[] GetConnectorReady()
	{
		return conReady;
	}
	protected enum Direction {
		NORTH,
		EAST,
		SOUTH,
		WEST
	}
	private void sortConnectors(Vector2[] arr)
	{
		if(arr.Count() != 4)
		{
			GD.Print("Incorrect array size!");
			return;
		}
		//insertion sort by ascending y value: north ends up first, south ends up last
		int n = 4;
		for(int i = 1; i < n; i++)
		{
			int key = (int)arr[i].y;
			for(int j = i - 1; j >= 0 && arr[j].y > key; j--) arr[j+1] = arr[j];
		}
		//make sure east comes before west
		if(arr[1].x < arr[2].x)
		{
			Vector2 tmp = arr[1];
			arr[1] = arr[2];
			arr[2] = tmp;
		}
		//swap west and south to match enum order
		Vector2 tmp2 = arr[3];
		arr[3] = arr[2];
		arr[2] = tmp2;
	} 
}
