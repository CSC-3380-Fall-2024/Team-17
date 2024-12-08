using Godot;
using System;

public class ClickInteractB : Area
{
	// The path to the scene you want to switch to
	[Export] public string SceneToLoad = "res://Combat/Battle.tscn";

	public override void _Ready()
	{
		// Connect the input event to handle clicking
		Connect("input_event", this, nameof(OnInteractableClicked));
		
		// Connect the body_entered signal to handle collisions with other objects
		Connect("body_entered", this, nameof(OnBodyEntered));
	}

	private void OnInteractableClicked(Node camera, InputEvent inputEvent, Vector3 clickPosition, Vector3 normal, int shapeIdx)
	{
		if (inputEvent is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == (int)ButtonList.Left)
		{
			GD.Print("InteractableB clicked, removing it...");
			QueueFree(); // Remove the object when clicked
		}
	}

	private void OnBodyEntered(Node body)
	{
		// Check if the body that entered is the player
		if (body is Player) // Adjust the type to match your Player class
		{
			GD.Print("Player collided with InteractableB, switching to combat scene...");
			GetTree().ChangeScene(SceneToLoad); // Change to the combat scene
		}
	}
}
