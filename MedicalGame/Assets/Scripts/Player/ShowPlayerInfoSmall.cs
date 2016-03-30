using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowPlayerInfoSmall : MonoBehaviour {

	public Image rankingImg;
	public Text rankingName;
	public Text playerName;
	public Text playerLevel;

	private Sprite rankingSprite;

	// Use this for initialization
	void Start () {
		// Set player name info
		playerName.text = PlayerManager.I.player.profile.name;
		// Set ranking name info
		rankingName.text = PlayerManager.I.player.playerRank;
		// Set player level info
		playerLevel.text = "Level "+PlayerManager.I.player.playerLvl.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
