using UnityEngine;
using System.Collections;

public class CheckResourceAttr : PropertyAttribute {
	public const string skillPath = "skill/";
	public const string effectPath = "effect/";
	public const string hudPath = "hud/";
	public const string heroPath = "hero/";

	public string path;
	public CheckResourceAttr(string path=""){
		this.path = path;
	}

}
