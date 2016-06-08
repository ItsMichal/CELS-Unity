using UnityEngine;
using System.Collections;

public class CellNav : MonoBehaviour {
	//Cell feilds
	public GameObject goal;
	public Color color;
	public bool watching = false;
	Vector3[] array = new Vector3[3];
	static NavMeshAgent agent;
	public Factions fac;
	public int food = 10;
	Vector3 last;
	public float sub = 10;
	public float dist = 0;
	public ArrayList enemy;

	//CODE--------------------------------------------------------------------------------------------------

	// Update is called once per frame.
	void Update () {

		transform.FindChild("Cylinder").transform.localScale = new Vector3(.3f * food,.3f *food,.3f *food);
		GetComponent<CapsuleCollider>().radius = transform.FindChild("Cylinder").transform.localScale.x/2;

		transform.FindChild("Cylinder").GetComponent<Renderer>().material.color = color;


		//subtracts food for every time the $dist reaches the value $sub.
		if(food > 0)
		dist += (int)Vector3.Distance(transform.position,last) * .0001f; 



		if(sub < dist){
			food -= 1;
			dist = 0;
		}


		//when the cell has a goal it will set the cell as the target.
		if(goal != null){
			agent = GetComponent<NavMeshAgent>();
			agent.destination = goal.transform.position;
		}else{

			transform.position +=new Vector3(Random.Range(-0.1f, 0.1f), 0, Random.Range(-0.1f, 0.1f));
		}

		transform.position += stearAway();

		//if the user clicks on the cell it will flip the $watching to control the Main Camera. 
		if(watching){
			//Debug.Log(""+ fac.foodbound);
			Camera.main.orthographicSize = transform.FindChild("Cylinder").transform.localScale.x + 5;
			Vector3 buf = this.transform.position;
			//the position of the camera in the Y is dynamic for cells growing larger. 
			buf.y = transform.FindChild("Cylinder").transform.localScale.y + 7f;
			Camera.main.transform.position = buf;

		}



		//The if statement that allow replication.
		if(food >= fac.foodbound){
			food /= 2;
			fac.addMember(this.transform.position,food/2);
			transform.FindChild("Cylinder").transform.localScale = new Vector3(.5f ,.5f,.5f);
		}



	}

	// Allows for the $watching to be flip when clicked.
	void OnMouseDown(){
		watching = !watching;
	}

	//When collide with a object the program will see what the object it touched then apply the correct way to handle it.
	void OnTriggerEnter(Collider other){

		//IF statment for food tag - Destroys the food then increases it internal consumption. 
		if(other.gameObject.tag == "Food"){

			if(other == goal)
				goal = null;

			Destroy(other);
			Destroy(other.gameObject);
			other = null;
			food++;

			transform.FindChild("Cylinder").transform.localScale = new Vector3(.3f *food,.3f *food,.3f *food);
			GetComponent<CapsuleCollider>().radius = transform.FindChild("Cylinder").transform.localScale.x/2; 
		

		}

	}

	//setter for Color deals with faction.
	public void setColor(Color col){
		color = col;

	}

	//setter for faction to commuicate with the other cells or local variables.  
	public void setfac(Factions fac){
		this.fac = fac;
	}

	//when the object is done being created Start() will be called, in this we set the $last 
   //Helps the cell with loosing food for everytime it moves a set distence. 
	public void start(){
		last = this.transform.position;
	}

	//setter for the $goal object 
	public void setTarget(GameObject target){
		
		goal = target;
		
	}


	public Vector3 stearAway(){
		if(enemy.Count > 0){
		Vector3 buff = new Vector3(0,0,0);

		foreach(GameObject en in enemy){

			Vector3 di = (this.transform.position) - (en.transform.position);
			di.Normalize();
			di /= Vector3.Distance(en.transform.position, this.transform.position);
			buff += di;
		}

			if(enemy.Count > 0)
			buff /= (float)enemy.Count;

		buff *= -15;



			return buff;
		}

		return new Vector3(0,0,0);
	}

	//----------------------------------------------------TODO-----------------------------------------------

	//+colision Detection with applingforce(spelled wrong maybe)
	//+ 

}
