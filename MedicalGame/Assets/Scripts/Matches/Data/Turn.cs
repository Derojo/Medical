using UnityEngine;

[System.Serializable]
public class Turn{
	// Match ID
	[SerializeField] public int t_ID;
	// Player ID
	[SerializeField] public string  p_ID;
	// Question ID
	[SerializeField] public int q_ID;
	// Category ID
	[SerializeField] public int c_ID;
	// Turn status ( (in)correct )
	[SerializeField] public int t_st;

	public Turn( int _t_ID = 0, string _p_ID = "", int _q_ID = 0, int _c_ID = 0, int _t_st = 0) {
		t_ID = _t_ID;
		p_ID = _p_ID;
		q_ID = _q_ID;
		c_ID = _c_ID;
		t_st = _t_st;
	}
}