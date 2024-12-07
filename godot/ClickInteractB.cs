using Godot;
using System;

public class ClickInteractB : Area
{
	// The path to the scene you want to switch to
	[Export] public string SceneToLoad = "res://Combat/Battle.tscn";

	public override void _Ready()
	{
		// Ensure collision signals are connected (if needed)
		Connect("body_entered", this, nameof(OnBodyEntered));
	}

	private void OnBodyEntered(Node body)
	{
		// Check if the body that entered is the player
		if (body is Player)
		{
			GD.Print("Player collided with InteractableB!");
			OnPlayerCollision(); // Call the interaction logic
		}
	}

	public void OnPlayerCollision()
	{
		GD.Print("InteractableB was triggered by the player!");
		QueueFree(); // Remove the object or perform other actions
	}
}
