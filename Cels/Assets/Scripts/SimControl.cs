﻿using UnityEngine;
using System.Collections;

public class SimControl: MonoBehaviour {

	// Use this for initialization

	public int numFac = 4;
	public GameObject heaquarter;
	public GameObject food;
	public GameObject cells;
	public float spawnRate;


	//factions
	ArrayList fac = new ArrayList();
	//food target
	ArrayList foodList = new ArrayList();


	void Start () {
	


		// generate starting factions
		for(int i = 0; i < numFac; i++){


			fac.Add ( new Factions(i+"",new Color(Random.Range(0,5f),Random.Range(0,5f),Random.Range(0,5f)),new Vector3(0,0,0),cells));


		}


		InvokeRepeating("GenerateFood", 0, spawnRate);

	}
	
	// Update is called once per frame
	void Update () {


		for(int i = 0; i < foodList.Count; i++){
			GameObject food = foodList[i] as GameObject;
				if(food == null){
				Debug.Log(food);
				foodList.Remove(food);
			}
		}


		foreach(Factions facs in fac){
			facs.setTarget(foodList);
			facs.Update();
		}

	}

	void GenerateFood(){

		Vector3 foodpos = new Vector3(Random.Range(-29,29),0,Random.Range(-29,29));
		foodList.Add(Instantiate(food, foodpos, Quaternion.identity)as GameObject);
	}
}