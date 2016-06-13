using UnityEngine;
using System.Collections;

public class Faction : ScriptableObject  {
    // Some Parameters
    public string nafme;
    public int fid;
    public float[] RGB = { 0, 0, 0 };
    public int[] enemyfid;
    public ArrayList members = new ArrayList();
    public Cell big = null;

    public static Faction Create()
    {
        Faction obj = ScriptableObject.CreateInstance<Faction>();
        obj.cFaction();
        return obj;
    }

    public void cFaction()
    {
        fid = -1;
        enemyfid = new int[1];
        nafme = "Cell Group #" + fid;

    }
    /**
	 * <p>
	 * Constructor that will create a new factions but give it an ID.  
	 * @param id
	 */

    public static Faction Create(int id)
    {
        Faction obj = ScriptableObject.CreateInstance<Faction>();
        obj.cFaction(id);
        return obj;
    }

    public void cFaction(int id)
    {
        if (id != 0)
        {
            fid = id;
        }
        else {
            fid = -1;
            Debug.Log("ID CANNOT BE ZERO, SET TO -1");
        }

        enemyfid = new int[1];
        nafme = "Cell Group #" + fid;

    }
    /**
	 * <p>
	 * Constructor that will give a faction ID and Color. 
	 * @param id
	 * @param color
	 */
    public static Faction Create(int id, float color)
    {
        Faction obj = ScriptableObject.CreateInstance<Faction>();
        obj.cFaction(id, color);
        return obj;
    }
    public void cFaction(int id, float color)
    {
        if (id != 0)
        {
            fid = id;
        }
        else {
            fid = -1;
            Debug.Log("ID CANNOT BE ZERO, SET TO -1");
        }
        enemyfid = new int[1];
        nafme = "Cell Group #" + fid;

    }

    /**
	 * <p>
	 * 
	 * This constructor will create a faction that will
	 * have a custom ID, Color, and enemy list.
	 * @param id
	 * @param color
	 * @param enems
	 */
    public static Faction Create(int id, float[] color, int[] enems)
    {
        Faction obj = ScriptableObject.CreateInstance<Faction>();
        obj.cFaction(id, color, enems);
        return obj;
    }
    public void cFaction(int id, float[] color, int[] enems)
    {
        if (id != 0)
        {
            fid = id;
        }
        else {
            fid = -1;
            Debug.Log("ID CANNOT BE ZERO, SET TO -1");
        }

        RGB = (float[]) color.Clone();

        bool temp = true;
        foreach (int i in enems)
            if (i == fid)
                temp = false;
        if (temp)
        {
            enemyfid = (int[]) enems.Clone();
        }
        else {
            enemyfid = new int[1];
            Debug.Log("YOU CAN NOT BE YOUR OWN ENEMY.");
        }
        nafme = "Cell Group #" + fid;
    }

    /**
	 * <p>
	 * Constructor that will allow the custumization of 
	 * ID,color,enemy, and name.
	 *  
	 * @param id
	 * @param color
	 * @param enems
	 * @param name
	 */
    public static Faction Create(int id, float[] color, int[] enems, string name)
    {
        Faction obj = ScriptableObject.CreateInstance<Faction>();
        obj.cFaction(id, color, enems, name);
        return obj;
    }

    public void cFaction(int id, float[] color, int[] enems, string name)
    {
        if (id != 0)
        {
            fid = id;
        }
        else {
            fid = -1;
            Debug.Log("ID CANNOT BE ZERO, SET TO -1");
        }

        RGB = (float[]) color.Clone();

        bool temp = true;
        foreach (int i in enems)
            if (i == fid)
                temp = false;
        if (temp)
        {
            enemyfid = (int[]) enems.Clone();
        }
        else {
            enemyfid = new int[1];
            Debug.Log("YOU CAN NOT BE YOUR OWN ENEMY.");
        }
        this.nafme = name;
    }

    public int advSpe()
    {
        int sum = 0;
        foreach (Cell k in members)
        {
            sum += (int) k.getspeed();
        }
        sum /= members.Count;
        return sum;
    }

    public int advRe()
    {
        int sum = 0;
        foreach (Cell k in members)
        {
            sum += k.getRe();
        }
        sum /= members.Count;
        return sum;
    }

    public void addMember(Cell x)
    {
        if (big == null || x.am > big.am)
        {
            big = x;
        }
        members.Add(x);
    }

    public void removeMember(Cell x)
    {
        if (members.IndexOf(x) > -1)
        {
            members.Remove(members.IndexOf(x));
        }
        else {
            Debug.Log("COULD NOT REMOVE. SORRY.");
        }

    }
    public bool isEnemyFromFID(int id)
    {
        foreach (int id2 in enemyfid)
        {
            if (id2 == id)
            {
                return true;
            }
        }
        return false;
    }
    public int getFid()
    {
        return fid;
    }
    public void setFID(int fid)
    {
        this.fid = fid;
    }
    public float[] getFactionColor()
    {
        return RGB;
    }
    public void setFactionColor(float R, float G, float B)
    {
        this.RGB[0] = R;
        this.RGB[1] = G;
        this.RGB[2] = B;
    }
    public int[] getEnemy()
    {
        return enemyfid;
    }
    public void setEnemy(int[] enemyfid)
    {
        this.enemyfid = enemyfid;
    }
    public int getamount()
    {
        return members.Count;
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
