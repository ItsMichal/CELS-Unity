using UnityEngine;
using System.Collections;

public class CellNav : MonoBehaviour {

	public GameObject goal;

	public Color color;

	public bool watching = false;

	Vector3[] array = new Vector3[3];

	static NavMeshAgent agent;

	public Factions fac;

	public float food = 0;

	Vector3 last;

	//CODE--------------------------------------------------------------------------------------------------
	public void setfac(Factions fac){
		this.fac = fac;
	}

	public void start(){
		last = this.transform.position;
	}

	public void setTarget(GameObject target){

		goal = target;

	}
	// Update is called once per frame
	void Update () {

		transform.FindChild("Cylinder").transform.localScale = new Vector3(.5f * food,.5f * food,.5f * food);
		GetComponent<CapsuleCollider>().radius = transform.FindChild("Cylinder").transform.localScale.x/2;

		transform.FindChild("Cylinder").GetComponent<Renderer>().material.color = color;

		food -= Vector3.Distance(transform.position,last) * 0.0001f; 



		if(goal != null){
			agent = GetComponent<NavMeshAgent>();
			agent.destination = goal.transform.position;


		}

		if(watching){
			Debug.Log(""+ fac.foodbound);
			Camera.main.orthographicSize = 5;
			Vector3 buf = this.transform.position;
			buf.y = 7f;
			Camera.main.transform.position = buf;

		}




		if(food >= fac.foodbound){

			food = fac.foodbound/2;
			fac.addMember(this.transform.position);
			transform.FindChild("Cylinder").transform.localScale = new Vector3(.5f ,.5f,.5f);
		}



	}

	void OnMouseDown(){

		watching = !watching;
		
		
	}

	void OnTriggerEnter(Collider other){

		if(other.gameObject.tag == "Food"){

			if(other == goal)
				goal = null;

			Destroy(other);
			Destroy(other.gameObject);
			other = null;
			food++;
			transform.FindChild("Cylinder").transform.localScale = new Vector3(.5f * food,.5f * food,.5f * food);
			GetComponent<CapsuleCollider>().radius = transform.FindChild("Cylinder").transform.localScale.x/2; 
		

		}

	}

	public void setColor(Color col){
		color = col;

	}

}
