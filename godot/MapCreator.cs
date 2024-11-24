using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;


public class MapCreator : TileMap
{
	private List<Vector2> tilePositions = new List<Vector2>();

	public override void _Ready()
	{
		
	}


	//a method to hold different starting rooms shapes
	private void GenerateStartingRoom()
	{
		Random random = new Random();
		 
		List<Vector2> roomTiles = new List<Vector2>();


		var StartingRoom = new Godot.Collections.Dictionary {
			{0, new Godot.Collections.Dictionary{ 
				{"tilePositions", new List<Vector2>{
					new Vector2(0, 0), new Vector2(1,0), new Vector2(2,0),
					new Vector2(0, 1), new Vector2(1,1), new Vector2(2,1),
					new Vector2(0, 2), new Vector2(1,2), new Vector2(2,2),
									   new Vector2(1,2)}
					},
				{"extendPoints", new List<Vector2>{new Vector2(1,-1)}

				}

				}


			},
			{1, new Godot.Collections.Dictionary{
				{"tilePositions", new List<Vector2>{
					new Vector2(0,0), new Vector2(0,1), new Vector2(0,2)
				}
			},
				{"extendPoints", new List<Vector2>{new Vector2(0,-1)}

				}
			}
			},
			{2, new Godot.Collections.Dictionary{
				{"tilePositions", new List<Vector2>{
					new Vector2(2,0), new Vector2(0,1), new Vector2(1,1), new Vector2(2,1), new Vector2(2,2), new Vector2(2,3)

				}
			},
				{"extendPoints", new List<Vector2>{new Vector2(-1,1), new Vector2(2,-1)}

				}
			}
			}
			


		};
		
		 //get positions from dictionary'
		 if StartingRoom.ContainsKey(int roomSelction = random.Next(0, 3)) {
			roomTiles = StartingRoom[int.Parse(roomSelction.ToString())]["tilePositions"] as List<Vector2>;
		}
		else
		{
			GD.Print("Error in generating starting room");
		}
		
		



	}

	private void GenerateBigRoom()
	{
		Random random = new Random();
		 
		List<Vector2> roomTiles = new List<Vector2>();


		var BigRoom = new Godot.Collections.Dictionary {
			{0, new Godot.Collections.Dictionary{ 
				{"tilePositions", new List<Vector2>{
					new Vector2(2, 0), new Vector2(3,0), new Vector2(4,0),
					new Vector2(0, 1), new Vector2(1,1), new Vector2(2,1), new Vector2(3,1), new Vector2(4,1),
					new Vector2(0, 2), new Vector2(1,2), new Vector2(2,2), new Vector2(3,2), new Vector2(4,2),
					new Vector2(0, 3), new Vector2(1,3), new Vector2(2,3), new Vector2(3,3), new Vector2(4,3),
					new Vector2(0, 4), new Vector2(1,4), new Vector2(2,4), new Vector2(3,4), new Vector2(4,4)
					}
					},
				{"extendPoints", new List<Vector2>{new Vector2(2,5), new Vector2(5,2)}

				}

				}


			},
			{1, new Godot.Collections.Dictionary{
				{"tilePositions", new List<Vector2>{
					new Vector2(0,0), new Vector2(0,1), new Vector2(0,2),
					new Vector2(1,0), new Vector2(1,1), new Vector2(1,2),
					new Vector2(2,0), new Vector2(2,1), new Vector2(2,2)
				}
			},
				{"extendPoints", new List<Vector2>{new Vector2(1,3)}

				}
			}
			},
			{2, new Godot.Collections.Dictionary{
				{"tilePositions", new List<Vector2>{
					new Vector2(0,0), new Vector2(0,1), new Vector2(0,2),
					new Vector2(1,0), new Vector2(1,1), new Vector2(1,2),
					new Vector2(2,0), new Vector2(2,1), new Vector2(2,2),
					new Vector2(3,0), new Vector2(3,1), new Vector2(3,2)
				}
			},
				{"extendPoints", new List<Vector2>{new Vector2(-1,3), new Vector2(3,1)}

				}
			}
			}
			


		};
		
		 //get positions from dictionary'
		 if BigRoom.ContainsKey(int roomSelction = random.Next(0, 3)) {
			roomTiles = StartingRoom[int.Parse(roomSelction.ToString())]["tilePositions"] as List<Vector2>;
		}
		else
		{
			GD.Print("Error in generating Big rooms");
		}

	}
	private void GenerateFinalRoom()
	{
		Random random = new Random();
		 
		List<Vector2> roomTiles = new List<Vector2>();


		var FinalRoom = new Godot.Collections.Dictionary {
			{0, new Godot.Collections.Dictionary{ 
				{"tilePositions", new List<Vector2>{
					new Vector2(0, 0),  new Vector2(1,0), new Vector2(2, 0), new Vector2(3,0), new Vector2(4,0),
					new Vector2(0, 1), new Vector2(1,1), new Vector2(2,1), new Vector2(3,1), new Vector2(4,1), new Vector2(5,1),
					new Vector2(0, 2), new Vector2(1,2), new Vector2(2,2), new Vector2(3,2), new Vector2(4,2), new Vector2(5,2), new Vector2(6,2),
					new Vector2(0, 3), new Vector2(1,3), new Vector2(2,3), new Vector2(3,3), new Vector2(4,3), new Vector2(5,3), 
					new Vector2(0, 4), new Vector2(1,4), new Vector2(2,4), new Vector2(3,4), new Vector2(4,4)
					}
					},
				{"extendPoints", new List<Vector2>{new Vector2(2,-1), new Vector2(-1,-1), new Vector2(1,5)}

				}
				


			},
			{1, new Godot.Collections.Dictionary{
				{"tilePositions", new List<Vector2>{
														new Vector2(2,0),
					new Vector2(1,1), new Vector2(2,1), new Vector2(3,1), new Vector2(4,1), new Vector2(5,1), new Vector2(6,1), new Vector2(7,1),
					new Vector2(1,2), new Vector2(2,2), new Vector2(3,2), new Vector2(4,2), new Vector2(5,2), new Vector2(6,2), new Vector2(7,2),
					new Vector2(0,3), new Vector2(1,3), new Vector2(2,3), new Vector2(3,3), new Vector2(4,3), new Vector2(5,3), new Vector2(6,3), new Vector2(7,3),
					new Vector2(1,4), new Vector2(2,4), new Vector2(3,4), new Vector2(4,4), new Vector2(5,4), new Vector2(6,4), new Vector2(7,4),
					                                                                                          new Vector2(6,5)

				}
			},
				{"extendPoints", new List<Vector2>{new Vector2(-1,2), new Vector2(2,-1)}

				}
			}
			}
			
			


		};
		
		 //get positions from dictionary'
		 if BigRoom.ContainsKey(int roomSelction = random.Next(0, 2)) {
			roomTiles = StartingRoom[int.Parse(roomSelction.ToString())]["tilePositions"] as List<Vector2>;
		}
		else
		{
			GD.Print("Error in generating Final room");
		}

	}			
}
}
