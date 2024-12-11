using Godot;
using System;
using System.Dynamic;

public class TurnIndicator4 : Panel
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
			GetNode<TurnIndicator4>(".").Show();
			GD.Print("Ally3 Option Select");
			// EmitSignal(nameof(UpdateNext),false);
			GetNode<Battle>("../..").EmitSignal(nameof(Battle.UpdateSkills), "ALLY3");
		}
		else if(check)
		{
			turn = false;
			GetNode<TurnIndicator4>(".").Hide();
			//EmitSignal(nameof(UpdateNext),true);
			GetNode<Battle>("../..").EmitSignal(nameof(Battle.FinishedSel));
		}
		else{
			turn = true;
			GetNode<TurnIndicator4>(".").Hide();
			//EmitSignal(nameof(UpdateNext),false);
		}
	}
	public override void _Ready()
	{
		GetNode<Control>("../../PartyPanel3/TurnIndicator3").Connect(nameof(TurnIndicator3.UpdateNext),GetNode<TurnIndicator4>("."),nameof(TurnUpdate));
		GetNode<Battle>("../..").Connect(nameof(Battle.HealthUpdate4), this, nameof(DMGTaken));
		GetNode<TurnIndicator4>(".").Hide();
		turn = false;
		_pBar = GetNode<ProgressBar>("../PlayerData/ProgressBar4");
		_hpText = GetNode<Label>("../PlayerData/ProgressBar4/Label");
		_name = GetNode<Label>("../NameCharacter4");
		ConfigFile cFile = new ConfigFile();
		cFile.Load("res://config/debug.cfg");
		_name.Text = (string)cFile.GetValue("ALLY3","NAME");
		int maxHealth = (int)cFile.GetValue("ALLY3","HEALTH");
		BarUpdate(maxHealth);



	}

}
