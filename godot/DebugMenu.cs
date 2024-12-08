using Godot;
using System;

public class DebugMenu : Control
{



	private void _on_Floor_Generater_pressed()
	{
		ConfigFile cfg = new ConfigFile();
		cfg.Load("res://Config/debug.cfg");
		cfg.SetValue("FLOOR_SETTINGS","MAP_NAME", "random");
		cfg.Save("res://Config/debug.cfg");
		GetTree().ChangeScene("res://World.tscn");
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
	public override void _Ready()
	{
		GetNode<Control>("RoomSelector").Hide();
		GetNode<Control>("Title Subtext").Show();
		GetNode<Control>("Floor Generator").Show();
		GetNode<Control>("Room Select").Show();

	}

}

