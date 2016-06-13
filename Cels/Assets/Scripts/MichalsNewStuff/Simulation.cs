using UnityEngine;
using System.Collections;

public class Simulation : MonoBehaviour {

    // VARIABLES
    //width
    public GameObject baseobj;
    int width;
    int height;

    bool runforever = false;
    // Is GUI Menu
    bool isGUIEnabled_Start = false;
    bool isGUIEnabled_Params = false;
    bool stop = true;

    // The flock is a handler of every living thing
    Flock flock;
   
    // Cool colors
    int sfgColor = 0;
    int sbgColor = 255;
    int fgColor = 0;
    int bgColor = 255;
    float lerpr = 0;

    // Whether thing is being reversed
    bool rev = false;

    // List of factions
    int numOfFacs = 8;
    public ArrayList factions;
    float[][] fcolors = { new float[]{ 255, 0, 0 }, new float[]{ 0, 255, 0 }, new float[]{ 0, 0, 255 }, new float[]{ 255, 255, 0 }, new float[]{ 0, 255, 255 },
            new float[]{ 255, 0, 255 }, new float[]{ 0, 0, 0 },new float[] { 255, 255, 255 } };
    // Fenemies sets up a team system
    int[][] fenemies = new int[8][]{ new int[]{ 2, 3, 4, 5, 6, 7, 8 }, new int[]{ 1, 3, 4, 5, 6, 7, 8 }, new int[]{ 1, 2, 4, 5, 6, 7, 8 },
            new int[]{ 1, 2, 3, 5, 6, 7, 8 }, new int[]{ 1, 2, 3, 4, 6, 7, 8 }, new int[]{ 1, 2, 3, 4, 5, 7, 8 }, new int[]{ 1, 2, 3, 4, 5, 6, 8 },
            new int[]{ 1, 2, 3, 4, 5, 6, 7 } };
    static int life = 20;
    int[][] facst;
    string[] fname = { "Alpha", "Bravo", "Charlie", "Delta", "Echo", "Foxtrot", "Golf", "Hotel" };
    Faction winningFac;
    Faction winner = null;
    ArrayList winners = new ArrayList();
    int winIndex = 0;
    // The time left in the simulation
    int clock = 0;
    int time;
    // STOP

    // STOP
    // time to stop sim
    static int stoptimer = 30;
    int stoptime = stoptimer;
    // STOP

    // STOP
    int gen = 1;
    int countdown = stoptimer;

    //TODO: Implement graph
    void Start () {
        facst = new int[][] {
            new int[] { (int)Random.Range(0, 5), (int)Random.Range(life, 40), (int)Random.Range(50, 60), (int)Random.Range(30, 60) },
           new int[] { (int)Random.Range(1, 3), (int)Random.Range(life, 40), (int)Random.Range(50, 60), (int)Random.Range(30, 60) },
            new int[] { (int)Random.Range(1, 3), (int)Random.Range(life, 40), (int)Random.Range(50, 60), (int)Random.Range(30, 60) },
            new int[] { (int)Random.Range(1, 3), (int)Random.Range(life, 40), (int)Random.Range(50, 60), (int)Random.Range(30, 60) },
            new int[] { (int)Random.Range(1, 3), (int)Random.Range(life, 40), (int)Random.Range(50, 60), (int)Random.Range(30, 60) },
            new int[] { (int)Random.Range(1, 3), (int)Random.Range(life, 40), (int)Random.Range(50, 60), (int)Random.Range(30, 60) },
            new int[] { (int)Random.Range(1, 3), (int)Random.Range(life, 40), (int)Random.Range(50, 60), (int)Random.Range(30, 60) },
            new int[] { (int)Random.Range(1, 3), (int)Random.Range(life, 40), (int)Random.Range(50, 60), (int)Random.Range(30, 60) } };
        height = (int)baseobj.GetComponent<Collider>().bounds.size.x;
        width = (int)baseobj.GetComponent<Collider>().bounds.size.y;
        factions = new ArrayList();
        //make array
        

        for (int i = 0; i < facst.Length; i++)
        {
            
            
            factions.Add(Faction.Create(i + 1, fcolors[i], fenemies[i], fname[i]));
        }

        // The initial point for the flock that is set to be random
        Color first = new Color(Random.Range(0,255), (255), Random.Range(0,255));

        // Instantiate a flock of
        flock = Flock.Create(baseobj, first, this);
        // Add an initial set of boids into the system
        for (int i = 0; i < 69; i++)
        {
            flock.addBoid(Food.Create(baseobj, width / 2, height / 2, 20));
        }
        Debug.Log(factions.Count);
        for (int k = 0; k < 4; k++)
        {
            flock.addbea(Cell.Create(baseobj, width / 4, height / 4, (Faction) factions[0], facst[0], this));
            flock.addbea(Cell.Create(baseobj, width - width / 4, height - height / 4, (Faction)factions[1], facst[1], this));
            flock.addbea(Cell.Create(baseobj, width / 4, height - height / 4, (Faction)factions[2], facst[2], this));
            flock.addbea(Cell.Create(baseobj, width - width / 4, height / 4, (Faction)factions[3], facst[3], this));
            flock.addbea(Cell.Create(baseobj, width / 2, height - height / 4, (Faction)factions[4], facst[4], this));
            flock.addbea(Cell.Create(baseobj, width / 2, height / 4, (Faction)factions[5], facst[5], this));
            flock.addbea(Cell.Create(baseobj, width - width / 4, height / 2, (Faction)factions[6], facst[6], this));
            flock.addbea(Cell.Create(baseobj, width / 4, height / 2, (Faction)factions[7], facst[7], this));

        }

        time = (int)(Time.time*10);

    }

    // Update is called once per frame
    void Update () {
        flock.run();
        // flock.removes();

        flock.eat();
        flock.reproduce();
    }
}
