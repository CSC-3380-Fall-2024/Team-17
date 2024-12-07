using Godot;
using System;

public class SigContrScript : Node2D
{
	private SigPlayer _player;
	private SigEnemy _enemy;


	public override void _Ready()
	{

		_player = GetNode<SigPlayer>("SigPlayer");
		_enemy = GetNode<SigEnemy>("SigEnemy");

		// Connect Signals Here
		_player.Connect(nameof(SigPlayer.DmgDealt), _enemy, nameof(SigEnemy.OnDmgTaken));
		
	}
	
	private void _on_Attack_pressed()
	{
		_player.PerformDmgDealt();
	}
	
	private void _on_Skill_pressed()
	{
		_player.PerformSkill();
	}

private void _on_Defend_pressed()
	{
		_player.PerformDefend();
	}

private void _on_Item_pressed()
	{
		_player.PerformItem();
	}

private void _on_Switch_pressed()
	{
		_player.PerformSwitch();
	}
	
	private void _on_Run_pressed()
	{
		_player.PerformRun();
		// Change the scene to World.tscn
		GetTree().ChangeScene("res://World.tscn");
	}
	
}
