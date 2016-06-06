using UnityEngine;
using System.Collections;

public class CellNav : MonoBehaviour {

	public GameObject goal;

	public Color color;

	public bool watching = false;

	Vector3[] array = new Vector3[3];



	static NavMeshAgent agent;

	public Factions fac;

	public int food = 0;

	public CellNav(){




	}

	public void setfac(Factions fac){
		this.fac = fac;
	}

	public void setTarget(GameObject target){

		goal = target;

	}
	// Update is called once per frame
	void Update () {
	
		transform.FindChild("Cylinder").GetComponent<Renderer>().material.color = color;



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

			food = 0;
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
