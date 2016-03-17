using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Matches {

	public List<Match> matches;

	public Matches() {
		if (matches == null) {
			matches = new List<Match> ();
		}
	}
}