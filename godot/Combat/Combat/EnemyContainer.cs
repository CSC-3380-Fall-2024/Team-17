using Godot;
using System;

public class EnemyContainer : VBoxContainer
{
	private ProgressBar _pBar;
	private Label _hpText;
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
		
	}

	public override void _Ready()
	{
		GetNode<Battle>("..").Connect(nameof(Battle.EHealthUpdate), this, nameof(DMGTaken));
		_pBar = GetNode<ProgressBar>("./ProgressBar");
		_hpText = GetNode<Label>("./ProgressBar/Label");
		ConfigFile cFile = new ConfigFile();
		cFile.Load("res://config/debug.cfg");
		int maxHealth = (int)cFile.GetValue("ENEMY","HEALTH");
		BarUpdate(maxHealth);
	}


}
