using UnityEngine;
using System.Collections;

public class Factions : MonoBehaviour {

	private string name;
	private Color FacColor;
	private GameObject boids;


	ArrayList targets = new ArrayList();
	ArrayList members = new ArrayList();


	public Factions(string name,Color FacColor, Vector3 headQuartersloc,GameObject boids){

		this.name = name;
		this.FacColor = FacColor;
		this.boids = boids;

		for(int i = 0; i < 1; i++){
			addMember(headQuartersloc);
		
		}

	}

	public void Update (){

		Debug.Log("Fractions-Member-Update");
		foreach(GameObject cel in members){
			
			CellNav cell = cel.GetComponent<CellNav>() as CellNav;
			if(cell.goal == null && targets.Count >= 1){

				GameObject f = targets[0] as GameObject;
				float dist = Vector3.Distance(cel.transform.position,f.transform.position);
				foreach(GameObject t in targets)
				{
					if(Vector3.Distance(cel.transform.position,(t as GameObject).transform.position) < dist){

						f = t as GameObject;
						dist = Vector3.Distance(cel.transform.position,(t as GameObject).transform.position);
					}

				}
				
				cell.setTarget(f);
				targets.Remove(f);
			}
			
		}
	}

	public void setTarget(ArrayList objects){

		targets = objects;
	}

	public void addMember(Vector3 loc){
		GameObject buf = Instantiate(boids,loc,Quaternion.identity) as GameObject;

		(buf.GetComponent<CellNav>() as CellNav).setColor(FacColor);
		members.Add(buf);

	}


}
