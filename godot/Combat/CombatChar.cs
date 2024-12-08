using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.CompilerServices;

public class Skill : Godot.Object
{
	private int  damage;
	private int hitrate;
	public bool mag;
	
	public Skill(int damage, int hitrate, bool mag)
	{
		this.damage = damage;
		this.hitrate = hitrate;
		this.mag = mag;
	}

	public int GetDamage()
	{
		return damage;
	}

	public int GetHitRate()
	{
		return hitrate;
	}
}
public class SkillList
{
	public static Skill ice1 = new Skill(30, 90, true);
	public static Skill fire1 = new Skill(30, 90, true);
	public static Skill elec1 = new Skill(30,90, true);
	public static Skill stab1 = new Skill(40, 80, false);
	public Dictionary<string, Skill> allSkills = new Dictionary<string, Skill>();

	public SkillList()
	{   
		allSkills.Add("ice1",ice1);
		allSkills.Add("fire1",fire1);
		allSkills.Add("elec1",elec1);
		allSkills.Add("stab1",stab1);
	}
	public List<string> GetAllSkills()
	{
		List<string> strList = new List<string>();
		foreach(var i in allSkills)
		{
			strList.Add(i.Key);
		}
		return strList;
	}
	public Skill GetSkill(string id)
	{
		if(!allSkills.ContainsKey(id))
		{
			GD.Print("Error: Skill does not exist");
		}
		Skill skill = allSkills[id];
		return skill;
	}
	public string SkillToStr(Skill skill)
	{
		string str = "";
		foreach (string i in allSkills.Keys)
		{
			if(Equals(allSkills[i],skill))
			{
				GD.Print("the string is "+i);
				return i;
			}
		}
		GD.Print("Could not find skill");
		return str;
	}

}
public class CombatChar: Control
{
	public SkillList totalSkills = new SkillList();
	private int attack;
	private int def;
	protected int mag;
	private int speed;
	private int luck;
	public int health;
	public string name;
	public Dictionary<string,Skill> skillList = new Dictionary<string, Skill>();
	/// <summary>
	/// The general class of combat characters
	/// </summary>
	/// <param name="attack">the attack stat</param> 
	/// <param name="def">the defense stat</param>
	/// <param name="mag">the magic stat</param>
	/// <param name="speed">the speed stat</param>
	/// <param name="luck">the luck stat</param>
	/// <param name="health">the health of the character</param>
	/// <param name="name">the name of the character</param>
	public CombatChar(int attack, int def, int mag, int speed, int luck, int health, string name)
		  {
			this.attack = attack;
			this.def = def;
			this.mag = mag;
			this.speed = speed;
			this.luck = luck;
			this.health = health;
			this.name = name;;
		}
	/// <summary>
	/// The constructor for the combat character class. This takes the name of the cfg file
	/// and the section of the cfg file
	/// </summary>
	/// <param name="cName">the name of the section of the specified config file</param>
	/// <param name="file">the name of the file res://debug/*.cfg</param>
	public CombatChar(string cName, string file)
	{
		ConfigFile cFile = new ConfigFile();
		cFile.Load($"res://Config/{file}.cfg");
		attack = (int)cFile.GetValue(cName,"ATTACK");
		def = (int)cFile.GetValue(cName,"DEFENSE");
		mag = (int)cFile.GetValue(cName,"MAGIC");
		speed = (int)cFile.GetValue(cName,"SPEED");
		luck = (int)cFile.GetValue(cName,"LUCK");
		health = (int)cFile.GetValue(cName,"HEALTH");
		name = (string)cFile.GetValue(cName,"NAME");
		String[] cSplList = (String[])cFile.GetValue(cName,"SKILLS");
		List<Skill> sList = new List<Skill>();
		foreach (string i in cSplList)
		{
			sList.Add(totalSkills.GetSkill(i));
		}
		for (int i = 0; i < sList.Count; i++)
		{
			skillList.Add(cSplList[i],sList[i]);
		}


	}
	public CombatChar(){}
		private bool WillHit(Skill skill)
		{
			Random rand = new Random();
			int r = rand.Next(101);
			if (r > skill.GetHitRate())
			{
				return false;
			}
			return true;
		}

		public int BaseAttack()
		{
			return attack*30/2;
		}

		public int SkillAttack(Skill skill)
		{
			if (skill.mag)
			{
				 return mag*skill.GetDamage()/2;
			}
			else
			{
				return attack*skill.GetDamage()/2;
			}
		}

		public int DamageCalc(int damage, CombatChar enemy)
		{
			GD.Print($"damage: {damage}");
			return damage/enemy.GetDef();
		}

		public void AddSkills(Dictionary<string,Skill> sList,string strSkl, string cName)
		{
			if (sList.ContainsKey(strSkl))
			{
				GD.PrintErr("Error, skill already in skill list");
				return;
			}
			else
			{
				sList.Add(strSkl, totalSkills.GetSkill(strSkl));
				ConfigFile cFile = new ConfigFile();
				cFile.Load("res://config/debug.cfg");
				String[] strList = (String[])cFile.GetValue(cName,"SKILLS");
				Array.Resize(ref strList, strList.Length+1);
				strList[strList.Length-1] = strSkl;
				GD.Print(strList);
				cFile.SetValue(cName,"SKILLS",strList);
				cFile.Save("res://config/debug.cfg");
				
			}
		}
		public void Damage(int damageCalc, CombatChar combatChar)
		{
			combatChar.health -= damageCalc;
			GD.Print(combatChar.name +" Health: "+combatChar.health);
		}
		public void SklDmg(Skill skill, CombatChar combatChar, CombatChar enemy)
		{
			int dmg = DamageCalc(combatChar.SkillAttack(skill), enemy);
			enemy.health -= dmg;
			GD.Print(combatChar.name +" Health: "+enemy.health);
		}
		public int GetSpeed()
		{
			return speed;
		}
		public int GetDef()
		{
			return def;
		}
		public List<string> GetSkills()
		{
			List<string> strList = new List<string>();
			foreach(var i in skillList)
			{
				strList.Add(i.Key);
			}
			return strList;
		}
	public override void _Ready()
	{
		GetNode<Control>("..").Connect(nameof(Battle.Atk),GetNode<CombatChar>("."),nameof(Damage));
		GetNode<Control>("..").Connect(nameof(Battle.SkillAtk),GetNode<CombatChar>("."),nameof(SklDmg));
		//GetNode<Control>("..").Connect(nameof(Battle.EnemyChoiceSel), GetNode<CombatChar>("."), nameof(CombatEnemy.EnemySel));
	}

}
	public class CombatPlayer : CombatChar
	{
		/// <summary>
		/// The CombatPlayer Class, no different methods to differentiate it from CombatChar but exists if we need to
		/// </summary>
		/// <param name="attack">the attack stat</param> 
		/// <param name="def">the defense stat</param>
		/// <param name="mag">the magic stat</param>
		/// <param name="speed">the speed stat</param>
		/// <param name="luck">the luck stat</param>
		/// <param name="health">the health of the character</param>
		/// <param name="name">the name of the character</param>
		public CombatPlayer(int attack, int def, int mag, int speed, int luck, int health, string name)
		: base(attack, def, mag, speed, luck, health, name){}
		public CombatPlayer(string cName, string file) : base(cName,file){}
	}

	public class CombatEnemy : CombatChar
	{
		[Signal]
		public delegate void SigEnemyChoiceSel();
		[Signal]
		public delegate void SigDoNothing();
		/// <summary>
		/// The combat enemy class, the main difference is that it holds the method to pick the enemies attack
		/// </summary>
		/// <param name="attack">the attack stat</param> 
		/// <param name="def">the defense stat</param>
		/// <param name="mag">the magic stat</param>
		/// <param name="speed">the speed stat</param>
		/// <param name="luck">the luck stat</param>
		/// <param name="health">the health of the character</param>
		/// <param name="name">the name of the character</param>
		public CombatEnemy(int attack, int def, int mag, int speed, int luck, int health, string name) 
		: base(attack, def, mag, speed, luck, health, name){}
		public CombatEnemy(string cName, string file) : base(cName,file){}

		public void EnemySel(ref string chcSel)
		{
			Random rand = new Random();
			int randNum = rand.Next(5);
			List<string> stringList = GetSkills();
			foreach (string i in stringList) { GD.Print(i); }
			if (randNum >= 3)
			{
				if (skillList.Count() > 0)
				{
					randNum = rand.Next(skillList.Count()+1);
					chcSel = $"e{stringList[randNum]}";
					return; //SkillAttack(stringList[randNum]) as int;
				}
				else
				{
					chcSel = "eattack";
					return;
				}
			}
			else
			{
				chcSel = "eattack";
				return;
			}
		}
}

