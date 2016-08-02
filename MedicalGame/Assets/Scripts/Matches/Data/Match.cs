using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Match {
	// Match ID
	[SerializeField] public string m_ID;
	// Players
	[SerializeField] public List<string> u_ids;
	// Player ID
	[SerializeField] public string p_ID;
	// Opponent ID
	[SerializeField] public string o_ID;
	// Match status
	[SerializeField] public string m_status;
	// Match current turn
	[SerializeField] public int m_ct;
	// Match current player
	[SerializeField] public string m_cp;
	// Match current category
	[SerializeField] public int m_cc;
	// respond time
	[SerializeField] public string m_date;
	// Match turns
	[SerializeField] public List<Turn> m_trns;




	public Match( string _m_ID="", List<string> _u_ids = null, string _p_ID="", string _o_ID="", string _m_status="", int _m_ct=0, string _m_cp="", int _m_cc=0, string matchDate = "",List<Turn> _turn = null) {
		m_ID = _m_ID;
		p_ID = _p_ID;
		o_ID = _o_ID;
		m_status = _m_status;
		m_ct = _m_ct;
		m_cp = _m_cp;
		m_cc = _m_cc;
		m_date = matchDate;
		if (_turn != null) {
			m_trns = _turn;
		}
		if (_u_ids != null) {
			u_ids = _u_ids;
		}

	}

	public void AddTurn(Turn _turn) {
		if (m_trns == null) {
			m_trns = new List<Turn> ();
		}
		m_trns.Add (_turn);
	}

	public void AddPlayer(string p_id) {
		if (u_ids == null) {
			u_ids = new List<string> (2);
		}
		u_ids.Add (p_id);
	}
}