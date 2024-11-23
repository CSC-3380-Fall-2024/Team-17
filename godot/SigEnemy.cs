using Godot;
using System;

public class SigEnemy : Node2D

{
	public void OnAttack()
	{
		GD.Print("Enemy Attack");
	}
	
	public void OnDmgTaken()
	{
		GD.Print("Enemy Taken Damage");
	}
	
	public void OnDmgDealt()
	{
		GD.Print("Enemy Hits Player");
	}
	
	
	
}
