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
	float spe;

	public Factions(string name,Color FacColor, Vector3 headQuartersloc,GameObject boids,SimControl s,float speed){

		this.name = name;
		this.FacColor = FacColor;
		this.boids = boids;
		this.control = s;
		spe = speed;
		for(int i = 0; i < 5; i++){
			addMember(headQuartersloc + new Vector3(Random.Range(-10,10),0, Random.Range(-20,20)),Random.Range(10,20));
		
		}

	}

	public void Update (){

		 enemy = control.mem();
	

		foreach(GameObject cel in members){
			
			CellNav cell = cel.GetComponent<CellNav>() as CellNav;


			//passing the enemys to the cell----------------------------------------------------
			ArrayList buff = new ArrayList();
			foreach(GameObject tar in enemy){

				if(Vector3.Distance(tar.transform.position,cell.transform.position) < 20 && !members.Contains(tar) )
					buff.Add(tar);
			}
			

			cell.enemy = buff;

			foreach(GameObject f in targets)
				buff.Add(f);

			cell.nextgoal(buff);
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
		(buf.GetComponent<CellNav>() as CellNav).spe = this.spe;
		members.Add(buf);
		control.addcell(buf);
	}


}

//----------------------------------------------------TODO-----------------------------------------------

//+Traits control
//+better values for food procesing
//+
