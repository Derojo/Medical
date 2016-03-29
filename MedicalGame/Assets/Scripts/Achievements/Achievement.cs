using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class Achievement
{
    [SerializeField]
    private string name;


    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    [SerializeField]
    private string description;


    public string Description
    {
        get { return description; }
        set { description = value; }
    }
    [SerializeField]
    private bool unlocked;

    public bool Unlocked
    {
        get { return unlocked; }
        set { unlocked = value; }
    }
    [SerializeField]
    private int points;

    public int Points
    {
        get { return points; }
        set { points = value; }
    }
    [SerializeField]
    private Sprite sprite;

    public Sprite ASprite
    {
        get { return sprite; }
        set { sprite = value; }
    }

    private GameObject achievementRef;


    public Achievement(string name, string description, int points, Sprite a_sprite)
    {
        this.name = name;
        this.description = description;
        this.unlocked = false;
        this.points = points;
        this.sprite = a_sprite;
        LoadAchievement();

    }

    public bool EarnAchievement()
    {
        if(!unlocked)
        {

            SaveAchievement(true);
            return true;
        }
        return false;
    }//end bool earnachievement


    //saving function
    public void SaveAchievement(bool value)
    {
        unlocked = value; //Sets the value

        //Gets the amount of points
        int tmpPoints = PlayerPrefs.GetInt("Points");

        //Stores the amount of points 
        PlayerPrefs.SetInt("Points", tmpPoints += points);

        //Stores the achievment's status
        PlayerPrefs.SetInt(name, value ? 1 : 0);

        //Saves the achievment
        PlayerPrefs.Save();
    }

    
    public void LoadAchievement()
    {
        unlocked = PlayerPrefs.GetInt(name) == 1 ? true : false;

    }

}
