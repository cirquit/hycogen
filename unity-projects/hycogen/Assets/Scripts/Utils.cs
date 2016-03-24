using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Utils {

	/*
	 * creates a mantle around 'y' with size 'puffer'
	 * checks is x is in that mantle
	 * 
	 * if not specified, the puffer is 0.0001f (most of the time this is enough to bypass physics)
	 **/
	public static bool IsInBounds(float x, float y, float mantle =  0.0001f)
	{
		if (x <= y + mantle && x >= y - mantle)
			return true;
		else
			return false;
	}

	/*
	 * creates a GameObject to call a LineRenderer to display a Line for an amount of time
	 * after the time has run out, the object destroys itself
     *
     * @TODO use a prefab
	 **/
	public static void DrawLine(List<Vector3> vs , Color c, float time = 0.02f)
	{
		GameObject    obj         = new GameObject ();
		LineRenderer rend         = obj.AddComponent<LineRenderer>();
		Material     lineMaterial = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));

        obj.name = "Path";
		rend.SetWidth (0.02f, 0.02f);
		rend.SetColors (c, c);
		rend.SetPositions(vs.ToArray());
		rend.material = lineMaterial;
		UnityEngine.Object.Destroy (obj, time);
	}
}
