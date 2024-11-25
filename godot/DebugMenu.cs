using Godot;
using System;

public class DebugMenu : Control
{




	private void _on_Generate_Settings_pressed()
	{
		int rInt;
		float rFlt;
		string rText = GetNode<TextEdit>("FlrGenSettingsTitle/RoomCountInt").Text;
		if(!Int32.TryParse(rText, out rInt))
		{
			GD.PrintErr("Type Error: Did Not Input An Int");
			return;
		}
		rText = GetNode<TextEdit>("FlrGenSettingsTitle/RoomSpaceFloat").Text;
		if(!float.TryParse(rText, out rFlt))
		{
			GD.PrintErr("Type Error: Did not Input A Float");
			return;
		}
		ConfigFile rFile = new ConfigFile();
		rFile.Load("res://Config/debug.cfg");
		rFile.SetValue("RANDOM_SETTINGS", "ROOM_CNT", rInt);
		rFile.SetValue("RANDOM_SETTINGS", "ROOM_SPACE", rFlt);
		rFile.Save("res://Config/debug.cfg");
	}
	private void _on_Room_Select_pressed()
	{
		GetNode<Control>("RoomSelector").Show();
		GetNode<Control>("Title Subtext").Hide();
		GetNode<Control>("Floor Generator").Hide();
		GetNode<Control>("Room Select").Hide();

	}
	private void _on_Floor_Generator_pressed()
	{
		GetNode<Control>("FlrGenSettingsTitle").Show();
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
	private void _on_FlrGenBack_pressed()
	{
		GetNode<Control>("FlrGenSettingsTitle").Hide();
		GetNode<Control>("Title Subtext").Show();
		GetNode<Control>("Floor Generator").Show();
		GetNode<Control>("Room Select").Show();
	}

	public override void _Ready()
	{
		GetNode<Control>("FlrGenSettingsTitle").Hide();
		GetNode<Control>("RoomSelector").Hide();
		GetNode<Control>("Title Subtext").Show();
		GetNode<Control>("Floor Generator").Show();
		GetNode<Control>("Room Select").Show();

	}

}
