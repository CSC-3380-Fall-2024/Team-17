using Godot;
using System;

public class DebugMenu : Control
{
	// Declare the OptionButton for resolutions
	private OptionButton ResolutionOptions;

	private void AddItems()
	{
		// Add resolution options
		ResolutionOptions.AddItem("1024x546"); 
		ResolutionOptions.AddItem("1280x720");  
		ResolutionOptions.AddItem("1600x900");  
		ResolutionOptions.AddItem("1920x1080"); 
	}

	private void OnOptionButtonItemSelected(int index)
	{
		// Change window size based on the selected resolution
		switch (index)
		{
			case 0:
				OS.WindowSize = new Vector2(1024, 546);
				break;
			case 1:
				OS.WindowSize = new Vector2(1280, 720);
				break;
			case 2:
				OS.WindowSize = new Vector2(1600, 900);
				break;
			case 3:
				OS.WindowSize = new Vector2(1920, 1080);
				break;
		}
		OS.CenterWindow(); // Center the window after resizing
	}

	private void _on_Floor_Generator_pressed()
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
		// Set the initial visibility of nodes
		GetNode<Control>("RoomSelector").Hide();
		GetNode<Control>("Title Subtext").Show();
		GetNode<Control>("Floor Generator").Show();
		GetNode<Control>("Room Select").Show();

		ResolutionOptions = GetNode<OptionButton>("ResolutionOptions");

		

		// Add resolution items to the OptionButton
		AddItems();

		// Connect the item_selected signal to the OnOptionButtonItemSelected method
		ResolutionOptions.Connect("item_selected", this, nameof(OnOptionButtonItemSelected));
	}
}
