using UnityEngine;
using System.Collections;

public class CellNav : MonoBehaviour {

	public GameObject goal;

	public Color color;

	public bool watching = false;

	Vector3[] array = new Vector3[3];

	private float FoodEat = 0;

	public CellNav(){




	}


	public void setTarget(GameObject target){
		Debug.Log("update");
		goal = target;

	}
	// Update is called once per frame
	void Update () {
	
		transform.FindChild("Cylinder").GetComponent<Renderer>().material.color = color;

		if(goal != null){
			NavMeshAgent agent = GetComponent<NavMeshAgent>();
			agent.destination = goal.transform.position;

			if(Vector3.Distance(goal.transform.position,this.transform.position) <= 5){
				//goal = null;
			}

		}

		if(watching){

			Camera.main.orthographicSize = 5;
			Vector3 buf = this.transform.position;
			buf.y = 7f;
			Camera.main.transform.position = buf;




		}



	}
	void OnMouseDown(){

		watching = !watching;
		
		
	}

	void OnTriggerEnter(Collider other){

		if(other.gameObject.tag == "Food"){
			Destroy(other.gameObject);
			goal = null;
			FoodEat += 0.1f;
			transform.FindChild("Cylinder").transform.localScale += new Vector3(.5f,0f,.5f);



		}

	}

	public void setColor(Color col){
		color = col;

	}

}
