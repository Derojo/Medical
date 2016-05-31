using UnityEngine;
using System.Collections;

public class buildAvatar : MonoBehaviour {

	public MeshFilter HairstyleMesh;
	public MeshRenderer RootHairstyle;
	public MeshRenderer RootHead;
	public SkinnedMeshRenderer RootShirt;
	public SkinnedMeshRenderer RootTrouser;

	public Mesh[] hairstyles;
	public Mesh[] shirts;
	public Mesh[] trousers;

    public bool isPlayer;
    // Use this for initialization
    private string avatarString = "";
	void Start () {
        Match currentMatch = MatchManager.I.GetMatch(MatchManager.I.currentMatchID);
        string opponentId = MatchManager.I.GetOppenentId(currentMatch);
        if (opponentId != "")
        {
            PlayerManager.I.GetPlayerInformationById(opponentId);
        }
        if (isPlayer)
        {
            avatarString = PlayerManager.I.player.avatar;
        } else
        {
            if (PlayerManager.I.currentOpponentInfo != null)
            {
                avatarString = PlayerManager.I.currentOpponentInfo["avatar"].ToString();
            }
            else {
                avatarString = "0_1_1_0_1_0_0_0";
            }
           
        }
		string[] avatar = avatarString.Split (new string[] {"_"}, System.StringSplitOptions.None);


		// build hairstyle
		HairstyleMesh.sharedMesh = hairstyles[int.Parse(avatar[1])];
		string hairstylename = "Hairstyle_"+ avatar[1] + "_" + avatar[2];
		RootHairstyle.sharedMaterial = (Material)Resources.Load ("Materials/Avatar/Hairstyles/" + hairstylename, typeof(Material));
		// build head
		string headName = "skin_"+avatar[0] + "_"+avatar[3];

		RootHead.sharedMaterial = (Material)Resources.Load("Materials/Avatar/Heads/"+ headName, typeof(Material));

		RootShirt.sharedMesh = shirts[int.Parse(avatar[4])];
		string shirtname = "Shirt_"+avatar[0] +"_"+ avatar[4]  + "_" + avatar[5];
		RootShirt.sharedMaterial = (Material)Resources.Load ("Materials/Avatar/Shirts/" + shirtname, typeof(Material));

		RootTrouser.sharedMesh = trousers[int.Parse(avatar[6])];
		string trousername = "Trouser_"+ avatar[6] + "_" + avatar[7];

		RootTrouser.sharedMaterial = (Material)Resources.Load ("Materials/Avatar/Trousers/" + trousername, typeof(Material));

		

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
