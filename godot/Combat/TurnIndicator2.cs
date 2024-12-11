using Godot;
using System;
using System.Dynamic;

public class TurnIndicator2 : Panel
{
	[Signal]
	public delegate void UpdateNext();
	private ProgressBar _pBar;
	private Label _hpText;
	private Label _name;
	protected bool turn;
	private void SetText(int health, int maxHealth)
	{
		_hpText.Text = $"{health}/{maxHealth}";
	}
	private void DMGTaken(int health)
	{
		ConfigFile cFile = new ConfigFile();
		cFile.Load("res://config/debug.cfg");
		int maxHealth = (int)cFile.GetValue("ALLY3","HEALTH");
		SetText(health, maxHealth);
		_pBar.Value = health;
	}
	private void BarUpdate(int health)
	{
		_pBar.Value = health;
		SetText(health,health);
		turn = true;
		
	}
	private void TurnUpdate(bool check)
	{
		if (check && turn)
		{
			turn = false;
			//EmitSignal(nameof(UpdateSkills),"ALLY1");
			GetNode<TurnIndicator2>(".").Show();
			GD.Print("Ally1 Option Select");
			EmitSignal(nameof(UpdateNext),false);
			GetNode<Battle>("../..").EmitSignal(nameof(Battle.UpdateSkills), "ALLY1");
		}
		else if(check)
		{
			turn = false;
			GetNode<TurnIndicator2>(".").Hide();
			EmitSignal(nameof(UpdateNext),true);
		}
		else{
			turn = true;
			GetNode<TurnIndicator2>(".").Hide();
			EmitSignal(nameof(UpdateNext),false);
		}
	}
	public override void _Ready()
	{
		GetNode<Control>("../../PartyPanel1/TurnIndicator1").Connect(nameof(TurnIndicator1.UpdateNext),GetNode<TurnIndicator2>("."),nameof(TurnUpdate));
		GetNode<Battle>("../..").Connect(nameof(Battle.HealthUpdate2), this, nameof(DMGTaken));
		GetNode<TurnIndicator2>(".").Hide();
		turn = false;
		_pBar = GetNode<ProgressBar>("../PlayerData/ProgressBar2");
		_hpText = GetNode<Label>("../PlayerData/ProgressBar2/Label");
		_name = GetNode<Label>("../NameCharacter2");
		ConfigFile cFile = new ConfigFile();
		cFile.Load("res://config/debug.cfg");
		_name.Text = (string)cFile.GetValue("ALLY1","NAME");
		int maxHealth = (int)cFile.GetValue("ALLY1","HEALTH");
		BarUpdate(maxHealth);



	}

}
