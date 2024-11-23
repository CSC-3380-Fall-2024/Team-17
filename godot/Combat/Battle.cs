using Godot;
using System;


public class Battle : Control
{
	// Creates a signal when the textbox is closed
	[Signal]
	public delegate void TextboxClosed();
	
	public override void _Ready()
	{
		GetNode<Control>("Textbox").Hide();
		GetNode<Control>("ActionsPanel").Hide();	
		
		display_text("A wild enemy appears!");
		
		Connect(nameof(TextboxClosed), this, nameof(OnTextboxClosed));
		
	}
	
	public override void _Input(InputEvent @event)
	{
		// Checks that the left mouse button is pressed
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == (int)ButtonList.Left && mouseEvent.Pressed)
		{
			GetNode<Control>("Textbox").Hide();
			EmitSignal(nameof(TextboxClosed));
		}
	}
	
	private void display_text(string text)
	{
		GetNode<Control>("Textbox").Show();
		GetNode<Label>("Textbox/Label").Text = text;
	}
	
	private void OnTextboxClosed()
	{
		// Show the ActionsPanel when the textbox is closed
		GetNode<Control>("ActionsPanel").Show();
		GD.Print("Textbox closed!"); 
	}
}

