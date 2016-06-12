using UnityEngine;
using System.Collections;

public class Flock {
    //TODO: Optimize Simulation/GameObject baseobj, make it so only 1 import is needed
    private Simulation tsa;
	public ArrayList h;
	public ArrayList killer;
    public GameObject baseobj;
	int high = 0;
	// An ArrayList for all the boids
	Color g = new Color(0, 0, 0);
    /**
	 * <p>
	 * Constructor that will link the Simulation object to the flock 
	 * along with a gobal color.  
	 * @param tsa
	 * @param colors
	 */
    public Flock(GameObject sat, Color colors, Simulation sa)
    {
        tsa = sa;
        baseobj = sat;
        
        h = new ArrayList();

        killer = new ArrayList();
        // Initialize the ArrayList
        g = colors;
    }

    /**
	 * @return amount of food objects
	 */
    public int getAmBac()
    {
        return h.Count;
    }

    /**
	 * 
	 * @return amount of Cells.
	 */
    public int getAmKiller()
    {
        return killer.Count;
    }




    /**
	 * <p>
	 * This method will run every Object. It will tell
	 * each one to process it step for that play. 
	 */
    public void run()
    {

        foreach (Food b in h)
        {
            b.run(h);
            b.lifetime++;

        }

        foreach (Cell k in killer)
        {
            k.run(h);

        }

    }


    /**
	 * <p>
	 * Mutator adding a new food object to the simulation.  
	 * @param b
	 */
    public void addBoid(Food b)
    {
        h.Add(b);
        b.colord = g;
    }

    /**
	 * <p>
	 * Mutator that will add more Hostile cells to the simulation.
	 * @param b
	 */
    public void addbea(Cell b)
    {
        killer.Add(b);
    }

    /**
	 * <p>
	 * 
	 * See if any objects are in the edible range of an object.
	 * If an object is then it will remove that object and reward the 
	 * other object points.
	 */
    public void eat()
    {
        for (int f = 0; f < killer.Count; f++)
        {
            Cell j = (Cell) killer[f];
            
            for (int i = 0; i < killer.Count; i++)
            {
                Cell otherhc = (Cell) killer[i];
                if (j.faction.isEnemyFromFID(otherhc.faction.getFid()))
                {
                    if (j.am > otherhc.am)
                    {
                        float k = Vector3.Distance(j.location, otherhc.location);
                        if (k <= j.am)
                        {
                            j.am += otherhc.am;
                            if (j.am > j.faction.big.am + 8)
                            {
                                j.faction.big = j;
                            }
                            Cell xt = (Cell) killer[i];
                            xt.dead = true;
                            xt.faction.members.Remove(xt);
                            killer.Remove(i);
                        }
                    }
                }
            }

            for (int s = 0; s < h.Count; s++)
            {
                float k = Vector3.Distance(j.location, ((Food) h[s]).location);
                if (k <= j.am - 3)
                {
                    h.Remove(s);
                    j.am += 2;
                    if (j.am > j.faction.big.am + 8)
                    {
                        j.faction.big = j;
                    }
                }

            }

        }

    }

    /**
	 * <p>
	 * This method will calculate if a cell can reproduce or not based 
	 * on its food level. Some objects have different ristrictions then others
	 * objects.
	 */
    public void reproduce()
    {
        baseobj.SetActive(true);
        for (int k = 0; k < killer.Count; k++)
        {
            Cell j = (Cell) killer[(k)];
            // welcome to the space jam
            if (j.am > j.getRe())
            {
                // System.out.println((j.am - (j.am%10))/10);
                
                addbea(new Cell(baseobj, j.location.x, j.location.y, j.faction, j.am / 2, j, tsa));
                j.am = j.am / 2;
            }
            if (j.am == 0)
            {
                j.dead = true;
                j.faction.members.Remove(j);
                killer.Remove(j);
            }
        }
        for (int y = 0; y < 3; y++)
        {
            int select =  Mathf.RoundToInt(Random.Range(0,h.Count - 1));
            if (h.Count != 0)
            {   
                //TODO: Check this out
                if ((int)Random.Range(0, ((Food) h[select]).ratio) == 1 && this.h.Count < 300)
                    addBoid(new Food(baseobj, ((Food) h[select]).location.x,((Food) h[(select)]).location.y,
                            ((Food) h[select]).ratio + (int) Random.Range(-5, 5)));
            }
        }

    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
