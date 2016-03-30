using UnityEngine;
using System.Collections;

public class SpawnScript : MonoBehaviour {

    private PathGA pathGA = null;

	void Start ()
    {
		Vector3    pos = new Vector3 (2.0f, 1.5f, 0.0f);
		Quaternion rot = new Quaternion ();

        GameObject agent  = (GameObject) Instantiate(Resources.Load("AgentPrefab"), pos, rot);   

        Agent agentScript = (Agent) agent.GetComponent<Agent>();

        pathGA = new PathGA(
                -1   // wallCollision
              , -900 // riverCollision
              , -2   // agentCollision
              , 100  // targetCollision
              , -1   // agentPathCollision
              , 10    // popSize
              , 4    // subPathCount
              , 3.0f // subPathLength
              , 2    // generationCount
              , 0.2f // alpha (NS - how many should be new)
              , 0.2f // beta  (NS - how many new children)
              , 1    // mode  (CO)
              , 0.5f // gamma (MU - how many should be mutated)
              , 0.5f // delta (MU - how much should be mutated in the individual)
              , 0.5f // maxDeviation (MU)
             );

        agentScript.speed  = 1.0f;
        agentScript.pathGA = pathGA;
	}

    private void OnGUI()
    {
        int height = 20;
        int width  = 300;
        int lines  = Mathf.Min(20, pathGA.popSize);

        if (pathGA.population != null)
        {
            GUI.Box(new Rect(0, 0, width, height * (lines + 1)), "Path fitness");

            for (int i = 0; i < lines; i++)
            {
                string line = pathGA.population[i].ToViewString();
                GUI.TextField(new Rect(0, (i+1) * height, width, height), line);
            }
        }
    }
}

