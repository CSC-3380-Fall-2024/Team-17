using Godot;
using System;

public class DebugMenu : Control
{



	private void _on_Floor_Generator_pressed()
	{
		ConfigFile rFile = new ConfigFile();
		rFile.Load("res://Config/debug.cfg");
		rFile.SetValue("FLOOR_SETTINGS","MAP_NAME", "random");
		rFile.Save("res://Config/debug.cfg");
		GD.Print("Not Yet Implemented");


	}
	private void _on_Room_Select_pressed()
	{
		GetNode<Control>("RoomSelector").Show();
		GetNode<Control>("Title Subtext").Hide();
		GetNode<Control>("Floor Generator").Hide();
		GetNode<Control>("Room Select").Hide();

	}
	private void _on_Back_pressed()
	{
		GetNode<Control>("RoomSelector").Hide();
		GetNode<Control>("Title Subtext").Show();
		GetNode<Control>("Floor Generator").Show();
		GetNode<Control>("Room Select").Show();
	}
	private void _on_Aspect_Ratio_pressed()
	{
		GetNode<Control>("RoomSelector").Hide();
		GetNode<Control>("Title Subtext").Hide();
		GetNode<Control>("Floor Generator").Hide();
		GetNode<Control>("Room Select").Hide();
		GetNode<Control>("Aspect Ratio2").Show();
		GetNode<Control>("Aspect Ratio3").Show();
	}
	
	private void _on_Aspect_Ratio2_pressed()
	{
		GetNode<Control>("RoomSelector").Hide();
		GetNode<Control>("Title Subtext").Hide();
		GetNode<Control>("Floor Generator").Hide();
		GetNode<Control>("Room Select").Hide();
	}
	public override void _Ready()
	{
		GetNode<Control>("RoomSelector").Hide();
		GetNode<Control>("Title Subtext").Show();
		GetNode<Control>("Floor Generator").Show();
		GetNode<Control>("Room Select").Show();
		GetNode<Control>("Aspect Ratio2").Hide();
		GetNode<Control>("Aspect Ratio3").Hide();

	}

}

