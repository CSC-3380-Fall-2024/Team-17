using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

public class Battle : Control
{
    public class CombatChar
    {
        public enum SkillsList {Ice1,Fire1,Elec1,Heal1,Stab1}
        private int attack;
        private int def;
        private int mag;
        public int speed;
        private int luck;
        private int health;
        private List<object> skills;

        protected CombatChar(int attack, int def, int mag, int speed, int luck, int health)
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

        public int MagAttack(int magSkill)
        {
            return (mag*magSkill)/2;
        }

        public int PhysAttack(int physSkill)
        {
            return (attack*physSkill)/2;
        }

        public int DamageCalc(int damage)
        {
            return damage/def;
        }

        public void AddSkills(SkillsList skill)
        {
           switch(skill)
           {
            case SkillsList.Ice1:
            skills.Add(SkillsList.Ice1);
            break;
            case SkillsList.Fire1:
            skills.Add(SkillsList.Fire1);
            break;
            case SkillsList.Elec1:
            skills.Add(SkillsList.Elec1);
            break;
            case SkillsList.Heal1:
            skills.Add(SkillsList.Heal1);
            break;
            case SkillsList.Stab1:
            skills.Add(SkillsList.Stab1);
            break;
           }
        }
        public void Damage(int damageCalc)
        {
            health -= damageCalc;
        }
        public int GetSpeed()
        {
            return speed;
        }

    }
    public static void TurnOrder(ref List<CombatChar> allChars)
    {
        allChars.Sort((x,y) => x.GetSpeed().CompareTo(y.GetSpeed()));

    }
    public CombatChar activeChar;
    
    public override void _Ready()
    {
        
        
    }

}
