using UnityEngine;
using System.Collections;

public class Utils {

	/*
	 * creates a mantle around 'y' with size 'puffer'
	 * checks is x is in that mantle
	 **/
	public static bool IsInBounds(float x, float y, float puffer){
		if (x <= y + puffer && x >= y - puffer)
			return true;
		else
			return false;
	}


	public static bool IsInBounds(float x, float y){
		return IsInBounds (x, y, 0.0001f);
	}
}
