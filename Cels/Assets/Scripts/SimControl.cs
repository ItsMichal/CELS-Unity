using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SimControl: MonoBehaviour {

	// Use this for initialization

	public int numFac = 4;
	public GameObject heaquarter;
	public GameObject food;
	public GameObject cells;
	public Text UiTime;
	public float spawnRate;
	public long lastsec;

	//factions
	ArrayList fac = new ArrayList();
	//food target
	ArrayList foodList = new ArrayList();

	Component time;

	void Start () {
		time = UiTime.GetComponent<Text>();
        //imported from old code
        //og colors list
        float[,] fcolors = { { 255, 0, 0 }, { 0, 255, 0 }, { 0, 0, 255 }, { 255, 255, 0 }, { 0, 255, 255 }, { 255, 0, 255 }, { 0, 0, 0 }, { 255, 255, 255 } };

        // generate starting factions
        for (int i = 0; i < numFac; i++){

            //Problem with colors generating white sometimes, on top
            //not sure if lighting problem or code problem
            Color fc = new Color(fcolors[i,0], fcolors[i,1], fcolors[i,2]);

			fac.Add ( new Factions(i+"",fc,new Vector3(0,0,0),cells));

			//startsim();
		}


		InvokeRepeating("GenerateFood", 0, spawnRate);

	}
	
	// Update is called once per frame
	void Update () {

		//Debug.Log((int)(Time.time * 1000) - lastsec + " " + lastsec);
		//set the UI Timer to the correct Time.

		(time as Text).text = "Time: "+((int)(Time.time * 1000))+"";

		/////////////////////////////////////
		for(int i = 0; i < foodList.Count; i++){
			GameObject food = foodList[i] as GameObject;
				if(food == null){

				foodList.Remove(food);
			}
		}


		foreach(Factions facs in fac){
			facs.setTarget(foodList);
			facs.Update();
		}

	}


	void startsim(){
		lastsec = (int)(Time.time * 1000);


	}

	void GenerateFood(){

		Vector3 foodpos = new Vector3(Random.Range(-29,29),0,Random.Range(-29,29));
		foodList.Add(Instantiate(food, foodpos, Quaternion.identity)as GameObject);
	}
}
