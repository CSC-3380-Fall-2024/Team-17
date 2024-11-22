using Godot;
using System;

public class ClickInteractB : Area
{
	// The path to the scene you want to switch to
	[Export] public string SceneToLoad = "res://Combat/Battle.tscn";
	
	public override void _Ready()
	{
		// Connect the input event to the handler
		Connect("input_event", this, nameof(OnInteractableClicked));
	}

	private void OnInteractableClicked(Node camera, InputEvent inputEvent, Vector3 clickPosition, Vector3 normal, int shapeIdx)
	{
		// Check if the left mouse button was clicked
		if (inputEvent is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == (int)ButtonList.Left)
		{
			// Change the scene when the object is clicked
			GetTree().ChangeScene(SceneToLoad);
		}
	}
}
