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
	[Signal]
	public delegate void DmgTaken();
	[Signal]
	public delegate void DmgDealt();

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
		GD.Print("<Not Yet Implemented>");
	}

	public void PerformItem()
	{
		EmitSignal(nameof(Item));
		GD.Print("Item Open");
	}

	public void PerformSwitch()
	{
		EmitSignal(nameof(Switch));
		GD.Print("<Not Yet Implemented>");
	}

	public void PerformRun()
	{
		EmitSignal(nameof(Run));
		GD.Print("RUN!!");
	}
	
		public void PerformDmgDealt()
	{
		EmitSignal(nameof(DmgDealt));
		GD.Print("Hiya!");
	}
	
		public void PerformDmgTaken()
	{
		EmitSignal(nameof(DmgTaken));
		GD.Print("OUCH");
	}
}
