using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Achievement 
{
    private string name;

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    private string description;

    public string Description
    {
        get { return description; }
        set { description = value; }
    }

    private bool unlocked;

    public bool Unlocked
    {
        get { return unlocked; }
        set { unlocked = value; }
    }

    private int points;

    public int Points
    {
        get { return points; }
        set { points = value; }
    }

    private int spriteIndex;

    public int SpriteIndex
    {
        get { return spriteIndex; }
        set { spriteIndex = value; }
    }

    private GameObject achievementRef;


    public Achievement(string name, string description, int points, int SpriteIndex, GameObject achievementRef)
    {
        this.name = name;
        this.description = description;
        this.unlocked = false;
        this.points = points;
        this.spriteIndex = spriteIndex;
        this.achievementRef = achievementRef;

    }

    public bool EarnAchievement()
    {
        if(!unlocked)
        {
            //achievementRef.GetComponent<Image>().sprite = AchievementManager.Instance.unlockedSprite;
            unlocked = true;
            return true;
        }
        return false;
    }//end bool earnachievement
}
