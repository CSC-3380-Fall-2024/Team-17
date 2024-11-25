using Godot;
using System;
using Godot.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Linq;


public class RoomSelector : MenuButton
{
	Directory dir;
	ConfigFile tscnPath;
	public List<string> convDirToStr(string location)
	{
		List<string> tileList = new List<String>();
		dir = new Directory();
		dir.Open(location);
		if (!dir.DirExists(location))
		{
			GD.PrintErr("Error, directory does not exist");
			return tileList;
		}
		dir.ListDirBegin(true);
		string fileItr = dir.GetNext();
		while (fileItr != "")
		{
			tileList.Add(fileItr);
			fileItr = dir.GetNext();
		}
		dir.ListDirEnd();
		return tileList;

	}
	private void IPressed(int x)
	{
		string id = GetNode<MenuButton>(".").GetPopup().GetItemText(x);
		tscnPath = new ConfigFile();
		tscnPath.Load("res://Config/debug.cfg");
		tscnPath.SetValue("FLOOR_SETTINGS","MAP_NAME", id);
		tscnPath.Save("res://Config/debug.cfg");
		//GetTree().ChangeScene("res://World.tscn"); //greyed out because breaks if implemented
	}
	public override void _Ready()
	{
		List<string> rList = convDirToStr("res://DefaultFloors");
		foreach (string i in rList)
		{
			GetNode<MenuButton>(".").GetPopup().AddItem(i);
		}
		GetNode<MenuButton>(".").GetPopup().Connect("id_pressed", GetNode<MenuButton>("."),nameof(IPressed));
		
	}
}

