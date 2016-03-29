using UnityEngine;


[System.Serializable]
public class Rank {

	// Rank name
	[SerializeField]public string name;
	// Required exp
	[SerializeField] public float reqXP;
	// Level scope
	[SerializeField, Tooltip("Between values dash seperated : 1/5")] public string levelScope;

	public Rank( string _name, float _reqXP, string _levelScope) {
		name = _name;
		reqXP = _reqXP;
		levelScope = _levelScope;
	}
}
