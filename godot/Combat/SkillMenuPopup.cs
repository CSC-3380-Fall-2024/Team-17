using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

public class SkillMenuPopup : PopupMenu
{
	private void Update(string charStats)
	{
		GetNode<PopupMenu>(".").Clear();
		ConfigFile sklFile = new ConfigFile();
		sklFile.Load("res://Config/debug.cfg");
		String[] sklList = sklFile.GetValue(charStats,"SKILLS") as String[];

		foreach (string i in sklList)
		{
			GetNode<PopupMenu>(".").AddItem(" "+i);
		}
	}
	public override void _Ready()
	{

		GetNode<Control>("..").Connect(nameof(Battle.UpdateSkills),GetNode<PopupMenu>("."),nameof(Update));
	}
	private void _on_SkillMenuPopup_id_pressed(int id)
	{
		string txt = GetNode<PopupMenu>(".").GetItemText(id).Trim();
		SkillList sList = new SkillList();
		Skill skl = sList.GetSkill(txt);
		GD.Print($"{txt}: {skl.mag}");
		GetNode<Battle>("..").EmitSignal(nameof(Battle.OptionSelected), txt);
		GetNode<Battle>("..").EmitSignal(nameof(Battle.NextCharGoes),true);
	}
}

