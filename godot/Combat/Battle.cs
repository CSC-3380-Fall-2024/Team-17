using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using static SigEnemy;
using static SigPlayer;
using static SigContrScript;
using System.Runtime.Serialization;
public class Battle : Control
{
	// Creates a signal when the textbox is closed
	[Signal]
	public delegate void TextboxClosed();
	[Signal]
	public delegate void Atk(); //does an attack
	[Signal]
	public delegate void SkillAtk(); //does a skill attack
	[Signal]
	public delegate void OptionSelected();//player has chosen their option
	// [Signal]
	// public delegate void Defend();
	// [Signal]
	// public delegate void Item();
	// [Signal]
	// public delegate void Switch();
	// [Signal]
	// public delegate void Run();
	[Signal]
	public delegate void DmgTaken();
	[Signal]
	public delegate void DmgDealt();
	[Signal]
	public delegate void FinishedSel();//this will kick off the combat
	[Signal]
	public delegate void Missed();
	[Signal]
	public delegate void UpdateSkills();//takes the name of a combatChar as an arg and the updates the skil list for that char
	[Signal]
	public delegate void NextCharGoes();
	[Signal]
	public delegate void EnemyChoiceSel();
	[Signal]
	public delegate void HealthUpdate1();
	[Signal]
	public delegate void HealthUpdate2();
	[Signal]
	public delegate void HealthUpdate3();
	[Signal]
	public delegate void HealthUpdate4();
	[Signal]
	public delegate void EHealthUpdate();
	public Queue<string> turnQueue = new Queue<string>();
	public List<CombatChar> turnOrder = new List<CombatChar>();
	public List<CombatChar> partyMem = new List<CombatChar>();
	public bool startSig;
	private CombatChar enemy;
	static public SkillList sList = new SkillList();
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
		
	}
	public static void TurnOrder(ref List<CombatChar> allChars)
	{
		allChars.Sort((x,y) => y.GetSpeed().CompareTo(x.GetSpeed()));

	}
	
	public override void _Ready()
	{
		GetNode<Control>("Textbox").Hide();
		GetNode<Control>("ActionsPanel").Hide();
		Connect(nameof(OptionSelected), this, nameof(OnSkillAtk));
		display_text("A wild enemy appears!");
		Connect(nameof(FinishedSel),this, nameof(ReadySig));
		Connect(nameof(TextboxClosed), this, nameof(OnTextboxClosed));
		CombatPlayer player = new CombatPlayer("PLAYER","debug");
		CombatChar ally1 = new CombatChar("ALLY1", "debug");
		CombatChar ally2 = new CombatChar("ALLY2", "debug");
		CombatChar ally3 = new CombatChar("ALLY3", "debug");
		CombatChar ally4 = new CombatChar("ALLY4", "debug");
		enemy = new CombatEnemy("ENEMY", "debug");	
		turnOrder = new List<CombatChar> { player, ally1, ally2, ally3, enemy};
		//TurnOrder(ref turnOrder);
		GD.Print($"the turn order is {turnOrder[0].name},{turnOrder[1].name},{turnOrder[2].name},{turnOrder[3].name},{turnOrder[4].name}");
		partyMem = new List<CombatChar> {player, ally1, ally2, ally3};
		startSig = false;
		//EmitSignal(nameof(Atk),player.DamageCalc(player.BaseAttack(),enemy),enemy);
		GD.Print("Combat Start!");
		EmitSignal(nameof(UpdateSkills),"PLAYER");

	}
	 public override void _Process(float delta)
 	{
		List<string> strList = sList.GetAllSkills();
		if(turnQueue.Count.Equals(4) && startSig)
			{
				foreach(string i in turnQueue) { GD.Print(i); }
				List<string> eChList = enemy.GetSkills();
				eChList.Add("attack");
				Random rand = new Random();
				int r = rand.Next(eChList.Count);
				turnQueue.Enqueue("e"+eChList[r]);
				GD.Print(turnQueue.Count);
				while(turnQueue.Count != 0 && startSig)
				{
					int j = 0;
					CombatChar y = turnOrder[j];
					string i = turnQueue.Peek();
					int x = rand.Next(4);
					switch (i)
					{
						case "attack":
						EmitSignal(nameof(Atk),y.DamageCalc(y.BaseAttack(),enemy),enemy);
						GD.Print(y.name + " is attacking enemy for " + y.BaseAttack());
						break;
						case "eattack":
						EmitSignal(nameof(Atk) ,enemy.DamageCalc(enemy.BaseAttack(),partyMem[x]),partyMem[x]);
						GD.Print(enemy.name + " is attacking " + partyMem[x].name + " for " + enemy.BaseAttack());
						break;
						case "ice1":
						EmitSignal(nameof(SkillAtk),sList.GetSkill(i),y,enemy);
						GD.Print(y.name + " activates spell " + i +" for "+ y.SkillAttack(sList.GetSkill(i)));
						break;
						case "fire1":
						EmitSignal(nameof(SkillAtk),sList.GetSkill(i),y,enemy);
						GD.Print(y.name + " activates spell " + i +" for "+ y.SkillAttack(sList.GetSkill(i)));
						break;
						case "elec1":
						EmitSignal(nameof(SkillAtk),sList.GetSkill(i),y,enemy);
						GD.Print(y.name + " activates spell " + i +" for "+ y.SkillAttack(sList.GetSkill(i)));
						break;
						case "stab1":
						EmitSignal(nameof(SkillAtk),sList.GetSkill(i),y,enemy);
						GD.Print(y.name + " activates spell " + i +" for "+ y.SkillAttack(sList.GetSkill(i)));
						break;
						case "eice1":
						EmitSignal(nameof(SkillAtk), sList.GetSkill(i.Substring(1)), enemy, partyMem[x]);
						GD.Print(enemy.name + " activates spell " + i.Substring(1) +" at "+ turnOrder[x].name + " for " +enemy.SkillAttack(sList.GetSkill(i.Substring(1))));
						break; 
						case "efire1":
						EmitSignal(nameof(SkillAtk), sList.GetSkill(i.Substring(1)), enemy, partyMem[x]);
						GD.Print(enemy.name + " activates spell " + i.Substring(1) +" at "+ turnOrder[x].name + " for " +enemy.SkillAttack(sList.GetSkill(i.Substring(1))));
						break;
						case "eelec1":
						EmitSignal(nameof(SkillAtk), sList.GetSkill(i.Substring(1)), enemy, partyMem[x]);
						GD.Print(enemy.name + " activates spell " + i.Substring(1) +" at "+ turnOrder[x].name + " for " +enemy.SkillAttack(sList.GetSkill(i.Substring(1))));
						break;
						case "eStab1":
						EmitSignal(nameof(SkillAtk), sList.GetSkill(i.Substring(1)), enemy, partyMem[x]);
						GD.Print(enemy.name + " activates spell " + i.Substring(1) +" at "+ turnOrder[x].name + " for " +enemy.SkillAttack(sList.GetSkill(i.Substring(1))));
						break;
					}
					j++;
					turnQueue.Dequeue();
				}
				startSig = false;
				GetNode<Control>("Textbox").Hide();
				EmitSignal(nameof(NextCharGoes),false);
				EmitSignal(nameof(UpdateSkills),"PLAYER");
				EmitSignal(nameof(HealthUpdate1), turnOrder[0].health);
				EmitSignal(nameof(HealthUpdate2), turnOrder[1].health);
				EmitSignal(nameof(HealthUpdate3), turnOrder[2].health);
				EmitSignal(nameof(HealthUpdate4), turnOrder[3].health);
				EmitSignal(nameof(EHealthUpdate), enemy.health);
				turnQueue.Clear();
				GD.Print("New Turn!");
			}
			if(turnQueue.Count == 0)
			{
				if((partyMem[0] != null && partyMem[0].health < 0) || (enemy != null && enemy.health < 0))
				{
					GD.Print("Combat Ended");
					GetTree().ChangeScene("res://World.tscn");
				}
			}
		}
		
		

	private void _on_Attack_pressed()
	{
		turnQueue.Enqueue("attack");
		GD.Print("Attacked");
		EmitSignal(nameof(NextCharGoes),true);
	}
	private void _on_Skill_pressed()
	{
		GetNode<PopupMenu>("SkillMenuPopup").Popup_();
	}	
	private void OnSkillAtk(string skill)
	{
		turnQueue.Enqueue(skill);
	}
	private void SigEnemyChoiceSel(string choice)
	{
		turnQueue.Enqueue(choice);
	}
	private void ReadySig()
	{
		display_text("Combat Is About to Start");
		startSig = true;
	}
}
