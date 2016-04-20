using UnityEngine;

[System.Serializable]
public class Turn{
	// Match ID
	[SerializeField] public int t_ID;
	// Player ID
	[SerializeField] public string  p_ID;
	// Opponent ID
	[SerializeField] public int q_ID;
	// Match status
	[SerializeField] public int t_st;

	public Turn( int _t_ID = 0, string _p_ID = "", int _q_ID = 0, int _t_st = 0) {
		t_ID = _t_ID;
		p_ID = _p_ID;
		q_ID = _q_ID;
		t_st = _t_st;
	}
}