using Godot;
using System;

public class ClickInteract : Area
{
	public override void _Ready()
	{
		Connect("input_event", this, nameof(OnInteractableClicked));
	}

	private void OnInteractableClicked(Node camera, InputEvent inputEvent, Vector3 clickPosition, Vector3 normal, int shapeIdx)
	{
		if (inputEvent is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == (int)ButtonList.Left)
		{
			QueueFree(); // Remove the object when clicked
		}
	}
}
