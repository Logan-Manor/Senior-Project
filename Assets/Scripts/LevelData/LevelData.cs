
using System;
using System.Collections.Generic;


[System.Serializable]
public class Level
{
    public string LevelName;
    public string ImagePath;
    public bool IsUnlocked;
    public float BronzeTime;
    public float SilverTime;
    public float GoldTime;
    public float BestTime;
    public string Difficulty;
}

[System.Serializable]
public class LevelList
{
    public List<Level> levels; // Initialized in constructor or inline
}

