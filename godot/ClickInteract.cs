using Godot;
using System;

public class ClickInteract : Area
{
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
			QueueFree(); // Remove the object when clicked
		}
	}

	private void OnBodyEntered(Node body)
	{
		// Check if the body that entered is the player or any specific object
		if (body is KinematicBody player)  // Adjust this based on the type of the object that collides with it
		{
			GD.Print("Player collided with interactable, removing...");
			QueueFree(); // Remove the object when the player runs into it
		}
	}
}
