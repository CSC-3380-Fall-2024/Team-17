using Godot;
using System;

public class ClickInteractB : Area
{
	[Export] public string SceneToLoad = "res://Combat/Battle.tscn";

	public override void _Ready()
	{
		Connect("body_entered", this, nameof(OnBodyEntered));
	}

	private void OnBodyEntered(Node body)
	{

		if (body is KinematicBody) // Looking for player Kinematic Body
		{
			GD.Print("Collided with InteractableB!");
			if (!string.IsNullOrEmpty(SceneToLoad))
			{
				GetTree().ChangeScene(SceneToLoad);
			}
			else
			{
				GD.Print("Scene Incorrect");
			}
		}
	}
}
