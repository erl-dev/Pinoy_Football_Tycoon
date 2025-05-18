using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public string playerName;
    public int age;
    public string position;
    public string nationality;
    public float marketValue;
    public int rating;

    [System.NonSerialized]
    public string marketValueInMillionsPHP; 
}

[System.Serializable]
public class TeamData
{
    public string teamName;
    public string homeVenue; 
    public int foundedYear;
    public float squadValue;
    public int pflTrophies;
    public int copaPaulinoTrophies;
    public List<PlayerData> players;
}