using UnityEngine;
using System.Collections;

public class globalFlock : MonoBehaviour {

	public globalFlock myFlock;
	public GameObject fishPrefab;
	public GameObject goalPrefab;

	static int numFish = 20;
	static int numGoal = 5;

	public GameObject[] allFish = new GameObject[numFish];
	public Vector3 goalPos;
	public GameObject[] allGoal = new GameObject[numGoal];
	//set the size of the bounding box to keep the fish within.
	//its actual side length will be twice the values given here
	public Vector3 swimLimits = new Vector3(5,5,5);


	//draw bounding box for limits of swim space
	//as well as the fish goal pos
	void OnDrawGizmosSelected() 
	{
        Gizmos.color = new Color(1, 0, 0, 0.5F);
        Gizmos.DrawCube(transform.position, new Vector3(swimLimits.x*2, swimLimits.y*2, swimLimits.z*2));
        Gizmos.color = new Color(0, 1, 0, 1);
        Gizmos.DrawSphere(goalPos, 0.1f);
    }

	// Use this for initialization
	void Start () 
	{
		myFlock = this;
		goalPos = this.transform.position;

		//fog definition begins here:
		RenderSettings.fogColor = Camera.main.backgroundColor;
		RenderSettings.fogDensity = 0.03F;
        RenderSettings.fog = true;
        //gound texture definition ends
		for(int i = 0; i < numFish; i++)
		{
			Vector3 pos = this.transform.position + new Vector3(Random.Range(-swimLimits.x,swimLimits.x),
				                      							Random.Range(-swimLimits.y,swimLimits.y),
				                      							Random.Range(-swimLimits.z,swimLimits.z));
			allFish[i] = (GameObject) Instantiate(fishPrefab, pos, Quaternion.identity);
			allFish[i].GetComponent<flock>().myManager = this;

		}
		for(int i = 0; i < numGoal; i++)
		{
			Vector3 pos = this.transform.position + new Vector3(Random.Range(-swimLimits.x,swimLimits.x),
				Random.Range(-swimLimits.y,swimLimits.y),
				Random.Range(-swimLimits.z,swimLimits.z));
			allGoal[i] = (GameObject) Instantiate(goalPrefab, pos, Quaternion.identity);
//			allGoal[i].GetComponent<flock>().myManager = this;
		}
		
	}
	
	// Update is called once per frame
	void Update () 
{
		foreach (GameObject fish in allFish) {
			for (int i=0; i<numGoal; i++) {
				if(Vector3.Distance(fish.transform.position, allGoal[i].transform.position) < 0.5f) //food is eaten, create new food
				{
					Destroy (allGoal[i]);
					Vector3 pos = this.transform.position + new Vector3(Random.Range(-swimLimits.x,swimLimits.x),
						Random.Range(-swimLimits.y,swimLimits.y),
						Random.Range(-swimLimits.z,swimLimits.z));
					allGoal[i] = (GameObject) Instantiate(goalPrefab, pos, Quaternion.identity);
				}
			}

		}

//		if (Input.GetMouseButtonDown(0))
//		{
//			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//			if (Physics.Raycast(ray))
//				Instantiate(goalPrefab, ray.GetPoint(10), transform.rotation);
//		}


		foreach (GameObject fish in allFish) {
			if(Vector3.Distance(fish.transform.position, goalPos) < 0.5f) //food is eaten, create new food
			{
				goalPos = this.transform.position + new Vector3(Random.Range(-swimLimits.x,swimLimits.x),
					Random.Range(-swimLimits.y,swimLimits.y),
					Random.Range(-swimLimits.z,swimLimits.z));
				goalPrefab.transform.position = goalPos;
			}
		}
			
	}
}
