using UnityEngine;
using System.Collections;
using System;

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class PrefabAttribute : Attribute {
	public readonly string Name;
	public readonly bool Persistent;
	public readonly string Parent;

	public PrefabAttribute(string name, bool persistent, string parent) {
		Name = name;
		Persistent = persistent;
		Parent = parent;
	}

	public PrefabAttribute(string name) {
		Name = name;
		Persistent = false;
	}
}