using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

public class Battle : Control
{
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
        protected List<Skill> allSkills;

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
        protected int health;
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

        public int MagAttack(Skill magSkill)
        {
            return (mag*magSkill.GetDamage())/2;
        }

        public int PhysAttack(Skill physSkill)
        {
            return (attack*physSkill.GetDamage())/2;
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
        public int Damage(int damageCalc)
        {
            health -= damageCalc;
            return damageCalc;
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

        public void DoNothing(){}//enemy does nothing

        public void EnemyChoiceSel()
        {
            Random rand = new Random();
            int randNum = rand.Next(11);
            if (randNum >= 8)
            {
                if (skillList.Count() > 0)
                {
                    randNum = rand.Next(skillList.Count()+1);
                    if (skillList.ElementAt<Skill>(randNum).mag)
                    {
                        MagAttack(skillList.ElementAt<Skill>(randNum));
                    }
                    else
                    {
                        PhysAttack(skillList.ElementAt<Skill>(randNum));
                    }
                    return;
                }
            }
            else if (randNum >=3)
            {
                BaseAttack();
                return;
            }
            else
            {
                DoNothing();
            }
        }
    }

    public static void TurnOrder(ref List<CombatChar> allChars)
    {
        allChars.Sort((x,y) => x.GetSpeed().CompareTo(y.GetSpeed()));

    }
    
    public override void _Ready()
    {
        //get enemy from tscn
        //get allies from tscn
        List<CombatChar> allChars; //append all characters here
        //List<CombatChar> turnOrder = allChars;
        //TurnOrder(turnOrder);

    }

}
