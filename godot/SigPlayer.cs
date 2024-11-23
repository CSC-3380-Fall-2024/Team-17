using Godot;
using System;

public class SigPlayer : Node2D
	

{
	// Declare signals
	[Signal]
	public delegate void Attack();
	[Signal]
	public delegate void Skill();
	[Signal]
	public delegate void Defend();
	[Signal]
	public delegate void Item();
	[Signal]
	public delegate void Switch();
	[Signal]
	public delegate void Run();

	// Functions to emit signals
	public void PerformAttack()
	{
		EmitSignal(nameof(Attack));
		
		GD.Print("Attack!");
	}

	public void PerformSkill()
	{
		EmitSignal(nameof(Skill));
		GD.Print("BANKAI(Skill)");
	}

	public void PerformDefend()
	{
		EmitSignal(nameof(Defend));
		GD.Print("BLOCK");
	}

	public void PerformItem()
	{
		EmitSignal(nameof(Item));
		GD.Print("Item");
	}

	public void PerformSwitch()
	{
		EmitSignal(nameof(Switch));
		GD.Print("SWITCH");
	}

	public void PerformRun()
	{
		EmitSignal(nameof(Run));
		GD.Print("RUN!!");
	}
}
