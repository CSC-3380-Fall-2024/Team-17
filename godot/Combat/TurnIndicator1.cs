using Godot;
using System;
using System.Dynamic;
using System.Security.Cryptography.X509Certificates;

public class TurnIndicator1 : Panel
{
	[Signal]
	public delegate void UpdateNext();
	[Signal]
	public delegate void DamageTaken();
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
		if (!check && !turn)
		{
			turn = true;
			GetNode<TurnIndicator1>(".").Show();
			EmitSignal(nameof(UpdateNext),false);
			GetNode<Battle>("../..").EmitSignal(nameof(Battle.UpdateSkills), "PLAYER");
		}
		else{
			turn = false;
			GetNode<TurnIndicator1>(".").Hide();
			EmitSignal(nameof(UpdateNext),true);
		}
	}
	public override void _Ready()
	{
		GetNode<Control>("../..").Connect(nameof(Battle.NextCharGoes),GetNode<TurnIndicator1>("."),nameof(TurnUpdate));
		GetNode<Battle>("../..").Connect(nameof(Battle.HealthUpdate1), this, nameof(DMGTaken));
		GetNode<TurnIndicator1>(".").Show();
		turn = true;
		_pBar = GetNode<ProgressBar>("../PlayerData/ProgressBar1");
		_hpText = GetNode<Label>("../PlayerData/ProgressBar1/Label");
		_name = GetNode<Label>("../NameCharacter1");
		ConfigFile cFile = new ConfigFile();
		cFile.Load("res://config/debug.cfg");
		_name.Text = (string)cFile.GetValue("PLAYER","NAME");
		int maxHealth = (int)cFile.GetValue("PLAYER","HEALTH");
		BarUpdate(maxHealth);



	}

}
