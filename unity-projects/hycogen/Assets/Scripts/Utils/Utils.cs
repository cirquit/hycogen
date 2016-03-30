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
    public static void DrawLine(List<Vector3> vs, float time = 0.02f, Color? c = null)
	{
		GameObject   obj          = new GameObject ();
		LineRenderer rend         = obj.AddComponent<LineRenderer>();
		Material     lineMaterial = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        Color        col          = c ?? Color.black;

        obj.name = "Path";
        obj.tag  = "AgentPath";
		rend.SetWidth (0.04f, 0.04f);
		rend.SetColors (col, col);
        rend.SetVertexCount(vs.Count);
        rend.SetPositions(vs.ToArray());
		rend.material = lineMaterial;
		UnityEngine.Object.Destroy (obj, time);
	}


    public static void DrawPath(Path p, Vector3 curPos, float time = 0.02f, Color? c = null)
    {
        Color col = c ?? Color.black;
        DrawLine(p.CreateAbsolutePathWithStart(curPos), time, col);
    }



    /*
     * Fisher-Yates Shuffle (stable)
     **/
    public static void Shuffle<T>(T[] arr)
    {
        int length = arr.Length;

        for (int i = 0; i < length; i++)
        {
           int j = Mathf.FloorToInt(Random.Range(0.0f, 1.0f) * (length - i));
           T k = arr[j];
           arr[j] = arr[i];
           arr[i] = k;
        }
    }
}
