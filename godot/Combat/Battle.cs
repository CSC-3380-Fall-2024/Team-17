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
	public delegate void Attack();
	private SigPlayer sigPlayer;
	
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
		GD.Print("Textbox closed!"); 
	}

	
	public class Skill
	{
		private int damage;
		private int hitrate;
		public bool mag;
		public Skill(int damage, int hitrate, bool mag)
		{
			damage = this.damage;
			hitrate = this.hitrate;
			mag = this.mag;
		}

		public int GetDamage()
		{
			return damage;
		}
	}
	public class SkillList
	{
		public Skill ice1 = new Skill(30, 90, true);
		public Skill fire1 = new Skill(30, 90, true);
		public Skill elec1 = new Skill(30,90, true);
		public Skill stab1 = new Skill(40, 80, false);
		public List<Skill> allSkills = new List<Skill>();

		public SkillList()
		{   
			allSkills.Add(ice1);
			allSkills.Add(fire1);
			allSkills.Add(elec1);
			allSkills.Add(stab1);
		}


	}
	public class CombatChar
	{
		public SkillList totalSkills = new SkillList();
		protected int attack;
		protected int def;
		protected int mag;
		protected int speed;
		protected int luck;
		public int health;
		protected List<Skill> skillList;

		public CombatChar(){}//empty constructor so that way subclasses can be passed

		public CombatChar(int attack, int def, int mag, int speed, int luck, int health)
		{
			attack = this.attack;
			def = this.def;
			mag = this.mag;
			speed = this.speed;
			luck = this.luck;
			health = this.health;

		}

		public int BaseAttack()
		{
			return attack/2;
		}

		public int SkillAttack(Skill skill)
		{
			if (skill.mag)
			{
				return (mag*skill.GetDamage())/2;
			}
			else
			{
				return (attack*skill.GetDamage())/2;
			}
		}

		public int DamageCalc(int damage)
		{
			return damage/def;
		}

		public void AddSkillList(ref List<Skill> sList, Skill skill)
		{
			if (sList.Contains(skill))
			{
				GD.PrintErr("Error, skill already in skill list");
				return;
			}
			else
			{
				sList.Add(skill);
				GD.Print($"Skill {skill} has been added");
			}
		}
		public void Damage(int damageCalc, CombatChar combatChar)
		{
			combatChar.health -= damageCalc;
			
		}
		public int GetSpeed()
		{
			return speed;
		}

	}
	public class CombatPlayer:CombatChar
	{
		
		public CombatPlayer(int attack, int def, int mag, int speed, int luck, int health)
		{
			attack = this.attack;
			def = this.def;
			mag = this.mag;
			speed = this.speed;
			luck = this.luck;
			health = this.health;
		}
	}

	public class CombatEnemy:CombatChar
	{
		public CombatEnemy(int attack, int def, int mag, int speed, int luck, int health)
		{
			attack = this.attack;
			def = this.def;
			mag = this.mag;
			speed = this.speed;
			luck = this.luck;
			health = this.health;
		}

		public int DoNothing(){return 0;}//enemy does nothing

		public int EnemyChoiceSel()
		{
			Random rand = new Random();
			int randNum = rand.Next(11);
			if (randNum >= 8)
			{
				if (skillList.Count() > 0)
				{
					randNum = rand.Next(skillList.Count()+1);
					return SkillAttack(skillList[randNum]);
				}
				else
				{
					return BaseAttack();
				}
			}
			else if (randNum >=3)
			{
				return BaseAttack();
			}
			else
			{
				return DoNothing();
			}
		}
	}

	public static void TurnOrder(ref List<CombatChar> allChars)
	{
		allChars.Sort((x,y) => x.GetSpeed().CompareTo(y.GetSpeed()));

	}
	public override void _Ready()
	{
		GetNode<Control>("Textbox").Hide();
		GetNode<Control>("ActionsPanel").Hide();	
		
		display_text("A wild enemy appears!");
		
		Connect(nameof(TextboxClosed), this, nameof(OnTextboxClosed));

		SigPlayer sPlayer = GetNode<SigPlayer>("res://SigPlayer");
		CombatPlayer player = new CombatPlayer(3, 5, 3, 5, 1, 100);
		CombatChar ally1 = new CombatChar(3, 3, 3, 3, 4, 100);
		CombatChar ally2 = new CombatChar(3, 3, 3, 3, 3, 100);
		CombatChar ally3 = new CombatChar(3, 3, 3, 3, 2, 100);
		CombatEnemy enemy = new CombatEnemy(3, 2, 3, 1, 1, 100);
		List<CombatChar> turnOrder = new List<CombatChar> { player, ally1, ally2, ally3, enemy };
		TurnOrder(ref turnOrder);
		GD.Print($"the turn order is {turnOrder[0]},{turnOrder[1]},{turnOrder[2]},{turnOrder[3]},{turnOrder[4]}");
		Button aButton = GetNode<Button>("Attack");
		Button skillButton = GetNode<Button>("Skill");
		while(enemy.health != 0 || player.health != 0)
		{
			Queue<Action> turnQueue = new Queue<Action>();
			while(turnQueue.Count() > 5)
			{
				int i = 0;
				CombatChar x = turnOrder[i];
				if (x is CombatEnemy)
				{
					CombatEnemy y = x as CombatEnemy;
					Action enemyAttck = delegate() {y.Damage(y.DamageCalc(y.EnemyChoiceSel()), player);};
					turnQueue.Enqueue(enemyAttck);
					i++;

				}
				else
				{
					if (aButton.Pressed)
					{
						GD.Print("button pressed");
						Action attck = delegate() {x.Damage(x.DamageCalc(x.BaseAttack()), enemy);};
						turnQueue.Enqueue(attck);
						i++;
					}
				}

			}
			GD.Print("Combat started");
			foreach (Action i in turnQueue)
			{
				i();
			}
			GD.Print("New Turn!");
		}

	}

}

