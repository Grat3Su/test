using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using STD;

public class Main
{
	public static Main me;

	gGUI currScene, nextScene;

	public Main()
	{
		me = this;

		currScene = createGameObject("Proc");
		nextScene = null;
	}

	public void reset(string name)
	{
		nextScene = createGameObject(name);

		GameObject.Destroy(currScene);
		currScene = nextScene;
	}
		
	gGUI createGameObject(string nameGUI)
	{
		GameObject go = new GameObject(nameGUI);

		var type = System.Type.GetType(nameGUI);
		gGUI scene = (gGUI)go.AddComponent(type);
		return scene;
	}
}
