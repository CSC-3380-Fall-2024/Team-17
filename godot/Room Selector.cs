using Godot;
using System;
using Godot.Collections;
using System.Collections.Generic;

public class Room_Selector : MenuButton
{
    Directory dir;

    public List<string> convDirToStr(string location)
    {
        List<string> tileList = new List<String>();
        if (!dir.DirExists(location))
        {
            GD.PrintErr("Error, directory does not exist");
            return tileList;
        }
        dir.Open(location);
        dir.ListDirBegin();
        string fileItr = dir.GetNext();
        while (fileItr != "")
        {
            tileList.Add(fileItr);
            fileItr = dir.GetNext();
        }
        return tileList;

    }
        public override void _Ready()
    {
        Room_Selector rSelect = new Room_Selector();
        List<string> rList = convDirToStr("DefaultFloors");
        
        
    }
}
