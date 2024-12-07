using Godot;
using System;
using System.Dynamic;

public class TurnIndicator3 : Panel
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
		SetText(health, (int)_pBar.Value);
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
			GetNode<TurnIndicator3>(".").Show();
			GD.Print("Ally2 Option Select");
			EmitSignal(nameof(UpdateNext),false);
			GetNode<Battle>("../..").EmitSignal(nameof(Battle.UpdateSkills), "ALLY2");
		}
		else if(check)
		{
			turn = false;
			GetNode<TurnIndicator3>(".").Hide();
			EmitSignal(nameof(UpdateNext),true);
		}
		else{
			turn = true;
			GetNode<TurnIndicator3>(".").Hide();
			EmitSignal(nameof(UpdateNext),false);
		}
	}
	public override void _Ready()
	{
		GetNode<Control>("../../PartyPanel2/TurnIndicator2").Connect(nameof(TurnIndicator2.UpdateNext),GetNode<TurnIndicator3>("."),nameof(TurnUpdate));
		GetNode<Battle>("../..").Connect(nameof(Battle.HealthUpdate3), this, nameof(DMGTaken));
		GetNode<TurnIndicator3>(".").Hide();
		turn = false;
		_pBar = GetNode<ProgressBar>("../PlayerData/ProgressBar3");
		_hpText = GetNode<Label>("../PlayerData/ProgressBar3/Label");
		_name = GetNode<Label>("../NameCharacter3");
		ConfigFile cFile = new ConfigFile();
		cFile.Load("res://config/debug.cfg");
		_name.Text = (string)cFile.GetValue("ALLY2","NAME");
		int maxHealth = (int)cFile.GetValue("ALLY2","HEALTH");
		BarUpdate(maxHealth);



	}

}
