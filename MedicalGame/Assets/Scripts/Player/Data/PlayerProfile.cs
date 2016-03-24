using UnityEngine;


[System.Serializable]
public class PlayerProfile{
	// Match ID
	[SerializeField] public string name;
	// Player ID
	[SerializeField] public int age;
	// Opponent ID
	[SerializeField] public string color;
	// Match status
	[SerializeField] public string hobby;
	// Match current turn
	[SerializeField] public string film;


	public PlayerProfile( string _name, int _age, string _color, string _hobby, string _film) {
		name = _name;
		age = _age;
		color = _color;
		hobby = _hobby;
		film = _film;
	}
}