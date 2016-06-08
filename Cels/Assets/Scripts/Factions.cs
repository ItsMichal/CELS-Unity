using UnityEngine;
using System.Collections;
//TODO: comment stuff pls
public class Factions : MonoBehaviour {

	private string name;
	private Color FacColor;
	private GameObject boids;
	public int foodbound = 20;
	public ArrayList enemy;
	public SimControl control;
	ArrayList targets = new ArrayList();
	ArrayList members = new ArrayList();


	public Factions(string name,Color FacColor, Vector3 headQuartersloc,GameObject boids,SimControl s){

		this.name = name;
		this.FacColor = FacColor;
		this.boids = boids;
		this.control = s;
		for(int i = 0; i < 1; i++){
			addMember(headQuartersloc,10);
		
		}

	}

	public void Update (){

		 enemy = control.mem();
		foreach(GameObject cel in members){
			
			CellNav cell = cel.GetComponent<CellNav>() as CellNav;
			if(cell.goal == null && targets.Count >= 1){

				GameObject f = targets[0] as GameObject;
				float dist = Vector3.Distance(cel.transform.position,f.transform.position);
				foreach(GameObject t in targets)
				{
					if(t != null && Vector3.Distance(cel.transform.position,(t as GameObject).transform.position) < dist){

						f = t as GameObject;
						dist = Vector3.Distance(cel.transform.position,(t as GameObject).transform.position);
					}

				}
				
				cell.setTarget(f);
				targets.Remove(f);

			}

			//passing the enemys to the cell
			ArrayList buff = new ArrayList();
			foreach(GameObject tar in enemy){

				if(Vector3.Distance(tar.transform.position,cell.transform.position) < 20)
					buff.Add(tar);

			}

			cell.enemy = buff;
				}

	}

	public void setTarget(ArrayList objects){

		targets = objects;
	}
	public void setEnemys(ArrayList objects){

		enemy = objects;

	}

	public void addMember(Vector3 loc, int food){
		GameObject buf = Instantiate(boids,loc,Quaternion.identity) as GameObject;

		(buf.GetComponent<CellNav>() as CellNav).setColor(FacColor);
		(buf.GetComponent<CellNav>() as CellNav).setfac(this);
		(buf.GetComponent<CellNav>() as CellNav).food = food | 10;
		members.Add(buf);
		control.addcell(buf);
	}


}

//----------------------------------------------------TODO-----------------------------------------------

//+Traits control
//+better values for food procesing
//+
