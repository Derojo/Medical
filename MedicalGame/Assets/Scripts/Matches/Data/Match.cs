using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Match{
	// Match ID
	[SerializeField] public string m_ID;
	// Player ID
	[SerializeField] public float p_ID;
	// Opponent ID
	[SerializeField] public float o_ID;
	// Match status
	[SerializeField] public string m_status;
	// Match current turn
	[SerializeField] public int m_ct;
	// Match current player
	[SerializeField] public float m_cp;
	// Match turns
	[SerializeField] public List<Turn> m_trns;



	public Match( string _m_ID, float _p_ID, float _o_ID, string _m_status, int _m_ct, float _m_cp, Turn _turn = null) {
		m_ID = _m_ID;
		p_ID = _p_ID;
		o_ID = _o_ID;
		m_status = _m_status;
		m_ct = _m_ct;
		m_cp = _m_cp;
		if (_turn != null) {
			m_trns.Add (_turn);
		}
	}

	public void AddTurn(Turn _turn) {
		if (m_trns == null) {
			m_trns = new List<Turn> ();
		}
		m_trns.Add (_turn);
	}
}