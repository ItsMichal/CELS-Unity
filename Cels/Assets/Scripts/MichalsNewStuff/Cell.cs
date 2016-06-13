using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour {
    private Simulation tsa;
	int re = 20;
    /*
	 * The bellow Vector3s act like the point class, they contain importent
	 * information about the Hostile object.
	 */

    public GameObject baseobj;

    public bool dead = false;
    int rod = 60;
    public Faction faction;
    public Vector3 location;
    Vector3 velocity;
    Vector3 acceleration;
    float r;
    float maxforce; // Maximum steering force
    float maxspeed; // Maximum speed
    Vector3 colord = new Vector3(0, 0, 0);
    // eating system
    bool eatingb = false;
    public int am = 20;
    Food targets;
    int x = 0;
    int y = 0;
    int index = 0;
    int oc = 0;
    int maxdist = 50;
    Cell mother = null;
    int height;
    int width;

    public static Cell Create(GameObject b, float x, float y, Faction faction, int ama, Cell copy, Simulation ast)
    {
        //Debug.Log("HERE");
        Object foods = Resources.Load("boid");
        //Debug.Log(foods);

        GameObject nx = Instantiate(foods) as GameObject;
        Cell u = nx.GetComponent<Cell>();
        u.cCell(b,x,y,faction,ama,copy,ast);
        return u;
    }

    public void cCell(GameObject b, float x, float y, Faction faction, int ama, Cell copy, Simulation ast)
    {
        tsa = ast;
        baseobj = b;
        width = (int) baseobj.GetComponent<Collider>().bounds.size.x;
        // Sets faction then joins it
        this.faction = faction;
        this.faction.addMember(this);
        this.am = ama;
        acceleration = new Vector3(0, 0);
        // This is a new Vector3 method not yet implemented in JS
        // velocity = Vector3.random2D();

        // Leaving the code temporarily this way so that this example runs
        // in JS
        
        

        location = new Vector3(x, y);
        r = 2.0f;
        maxspeed = 3.3f;
        maxforce = 1.03f;
        maxspeed = copy.maxspeed;
        this.re = copy.re;
        this.rod = copy.rod;
        this.maxdist = copy.maxdist;
        this.mother = copy;

    }
    public static Cell Create(GameObject b, float x, float y, Faction faction, float speed, int re, int rod, int dis, Simulation ast)
    {
        //Debug.Log("HERE");
        Object foods = Resources.Load("boid");
        //Debug.Log(foods);

        GameObject nx = Instantiate(foods) as GameObject;
        Cell u = nx.GetComponent<Cell>();
        u.cCell(b,x,y,faction,speed,re,rod,dis,ast);
        return u;
    }

    public void cCell(GameObject b, float x, float y, Faction faction, float speed, int re, int rod, int dis, Simulation ast)
    {
        baseobj = b;
        tsa = ast;
        this.rod = rod;
        // Sets faction then joins it
        this.faction = faction;
        this.faction.addMember(this);
        acceleration = new Vector3(0, 0);

        // This is a new Vector3 method not yet implemented in JS
        // velocity = Vector3.random2D();

        // Leaving the code temporarily this way so that this example runs
        // in JS
        //float angle =  Random.Range(0, 6.28318530717958647693f);
        //velocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));

        location = new Vector3(x, y);
        r = 2.0f;
        // add genetics for a cell
        maxspeed = speed;
        this.re = re;
        this.maxdist = dis;
            
    }
    public static Cell Create(GameObject b, float x, float y, Faction faction, int[] s, Simulation ast)
    {
        //Debug.Log("HERE");
        Object foods = Resources.Load("Boid");  
        Debug.Log(foods);

        GameObject nx = Instantiate(foods) as GameObject;
        Cell u = nx.GetComponent<Cell>();
        u.cCell(b,x,y,faction,s,ast);
        return u;
    }
    public void cCell(GameObject b, float x, float y, Faction faction, int[] s, Simulation ast)
    {
        tsa = ast;
        baseobj = b;
        // Sets faction then joins it
        this.faction = faction;
        this.faction.addMember(this);
        acceleration = new Vector3(0, 0);

        // This is a new Vector3 method not yet implemented in JS
        // velocity = Vector3.random2D();

        // Leaving the code temporarily this way so that this example runs
        // in JS
        //float angle = Random.Range(0, 6.28318530717958647693f);
        //velocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));

        location = new Vector3(x, y);
        r = 2.0f;
        // add genetics for a cell
        maxspeed = s[0];
        // System.out.println(faction.name + ": " + this.getspeed());
        this.re = s[1];
        this.rod = s[2];
        this.maxdist = s[3];

    }

    public void setgenetics(float speed, float force)
    {
        maxforce = force;
        maxspeed = speed;
    }

    public float getspeed()
    {
        return maxspeed;
    }

    public int getRe()
    {
        return this.re;
    }

    // run everything
    public void run(ArrayList foods)
    {
        flock(foods);

        update();

        borders();
        render();

    }

    public void applyForce(Vector3 force)
    {
        // We could add mass here if we want A = F / M
        acceleration += (force);
    }

    // We accumulate a new acceleration each time based on three rules
    public void flock(ArrayList foods)
    {
        Vector3 sep = separate(foods); // Separation
        Vector3 ali = align(foods); // Alignment
        Vector3 coh = cohesion(foods); // Cohesion
                                       // Arbitrarily weight these forces
        sep *= (1.5f);
        ali *= (1.0f);
        coh *= (1.0f);
        // Add the force vectors to acceleration
        if (eatingb == false)
        {
            applyForce(sep);
            applyForce(ali);
            applyForce(coh);
        }
    }

    // Method to update location
    public void update()
    {
        if (this.am > this.faction.big.am + 8)
        {
            this.faction.big = this;
        }
        // System.out.println(am);
        if ((Time.time * 10f) % 60 == 0 && am > 0 && (Time.time * 10f) != oc)
        {
            am--;
            oc = (int)(Time.time * 10f);

        }
        // Update velocity
        velocity += (acceleration);
        // Limit speed
        Vector3.ClampMagnitude(velocity, maxspeed);
        location += (velocity);
        // Reset accelertion to 0 each cycle
        acceleration *= 0;
    }

    // A method that calculates and applies a steering force towards a
    // target
    // STEER = DESIRED MINUS VELOCITY
    public Vector3 seek(Vector3 target)
    {
        Vector3 desired = target - location; // A vector
                                                         // pointing from
                                                         // the location
                                                         // to the target
                                                         // Scale to maximum speed
        desired.Normalize();
        desired *= (maxspeed);
        // System.out.println(this.faction.name + "'s Speed: " + maxspeed);

        // Above two lines of code below could be condensed with new Vector3
        // setMag() method
        // Not using this method until Processing.js catches up
        // desired.setMag(maxspeed);

        // Steering = Desired minus Velocity
        Vector3 steer = desired - velocity;
        Vector3.ClampMagnitude(steer, maxforce); //steer.limit(maxforce); // Limit to maximum steering force
        return steer;
    }


    /// <summary>
    /// updated to support unity
    /// </summary>
    public void render()
    {
        //lines between cells, deprecated for unity purposes
        //    this.tsa.pushMatrix();
        //    this.tsa.stroke(this.tsa.color(0, 25));
        //    if (this.tsa.winner != null && this.tsa.winner.fid == faction.fid)
        //    {
        //        if (mother != null && !mother.dead)
        //        {
        //            this.tsa.line(location.x, location.y, mother.location.x, mother.location.y);
        //            //this.tsa.lerp(location.y, mother.location.y, (float) ((Time.time * 10f)%1000f)/1000f);
        //            this.tsa.fill(this.tsa.color(255 - faction.RGB[0], 255 - faction.RGB[1], 255 - faction.RGB[2]));
        //            this.tsa.stroke(0);
        //            /*this.tsa.ellipse(this.tsa.lerp(location.x, mother.location.x, (float) ((Time.time * 10f)%2000f)/2000f),this.tsa.lerp(location.y, mother.location.y, (float) ((Time.time * 10f)%3000f)/3000f) ,am/4, am/4);
        //this.tsa.ellipse(this.tsa.lerp(location.x, mother.location.x, (float) ((Time.time * 10f)%2000f)/2000f),this.tsa.lerp(location.y, mother.location.y, (float) ((Time.time * 10f)%5000f)/5000f) ,am/4, am/4);
        //this.tsa.ellipse(this.tsa.lerp(location.x, mother.location.x, (float) ((Time.time * 10f)%2000f)/2000f),this.tsa.lerp(location.y, mother.location.y, (float) ((Time.time * 10f)%2000f)/2000f) ,am/4, am/4);*/
        //        }
        //        else {

        //            this.tsa.line(location.x, location.y, faction.members.get(0).location.x,
        //                    faction.members.get(0).location.y);
        //            this.tsa.fill(this.tsa.color(255 - faction.RGB[0], 255 - faction.RGB[1], 255 - faction.RGB[2]));
        //            this.tsa.stroke(0);
        //            /*this.tsa.ellipse(this.tsa.lerp(location.x, faction.members.get(0).location.x, (float) ((Time.time * 10f)%2000f)/2000f),this.tsa.lerp(location.y, faction.members.get(0).location.y, (float) ((Time.time * 10f)%3000f)/3000f) ,am/4, am/4);
        //this.tsa.ellipse(this.tsa.lerp(location.x, faction.members.get(0).location.x, (float) ((Time.time * 10f)%2000f)/2000f),this.tsa.lerp(location.y, faction.members.get(0).location.y, (float) ((Time.time * 10f)%5000f)/5000f) ,am/4, am/4);
        //this.tsa.ellipse(this.tsa.lerp(location.x, faction.members.get(0).location.x, (float) ((Time.time * 10f)%2000f)/2000f),this.tsa.lerp(location.y, faction.members.get(0).location.y, (float) ((Time.time * 10f)%2000f)/2000f) ,am/4, am/4);*/
        //        }
        //    }

        //    this.tsa.noStroke();
        //    this.tsa.popMatrix();
        // Get a copy of your forward vector
        Vector3 forward = transform.forward;
        // Zero out the y component of your forward vector to only get the direction in the X,Z plane
        forward.y = 0;
        float headingAngle = Quaternion.LookRotation(forward).eulerAngles.y;
        float theta = headingAngle + (Mathf.Deg2Rad*90);
        //this.tsa.pushMatrix();

        transform.Translate(new Vector3(location.x, location.y));
        transform.Rotate(new Vector3(theta,0));

        //deprecated until further notice
        //if (this.Equals(this.faction.big))
        //{

        //    //TODO: Specialize because biggest cell?
            

        //    //this.tsa.fill(this.tsa.color(100, 100));
        //    //this.tsa.ellipse(0, 0, am * 1.25f + 5, am * 1.5f + 7);
        //    //this.tsa.fill(this.tsa.color(255 - this.faction.RGB[0], 255 - this.faction.RGB[1], 255 - this.faction.RGB[2], 100));
        //    //this.tsa.ellipse(0, 0, am * 1.25f + 3, am * 1.5f + 5);


        //}
        //else {
        //   // this.tsa.fill(100);
            
        //    //this.tsa.ellipse(0, 0, am + 3, am * 1.25f + 3);
        //}
        //set size
        transform.localScale = new Vector3(am, am);
        //this.tsa.ellipse(0, 0, am + 3, am * 1.25f + 3);// ellipse(0, 0, 20 +
                                                       // (am) +
                                                       // 5, 20 + (am * 1.5f) + 5);
                                                       // this.tsa.textSize(25);
        //this.tsa.popMatrix();

        //DEPRECATED

        // Draw a triangle rotated in the direction of velocity

        // heading2D() above is now heading() but leaving old syntax until
        // Processing.js catches up
        //this.tsa.pushMatrix();
        //this.tsa.translate(location.x, location.y);
        //this.tsa.rotate(theta);
        // this.tsa.stroke(255);

        // System.out.println(this.faction.RGB);
        //this.tsa.fill(this.tsa.color(faction.RGB[0], faction.RGB[1],
        //        faction.RGB[2])); /* color(am, colord.y, am) */
        //this.tsa.noStroke(); 
        //// strokeWeight(1);

        //this.tsa.ellipse(0, 0, am, am * 1.25f);// ellipse(0, 0, 20 + (am * 2),
        //                                       // 20 +

        //// (am * 3));

        //this.tsa.popMatrix();
        //this.tsa.pushMatrix();
        //this.tsa.translate(location.x, location.y);
        //this.tsa.fill(this.tsa.color(0));// (255-faction.RGB[0],
        //                                 // 255-faction.RGB[1],
        //                                 // 255-faction.RGB[2]));

        //if (faction.big.equals(this))
        //{
        //    this.tsa.textSize((float)(am / 2));
        //    this.tsa.text(faction.name + " Leader", am, am);
        //}
        //else if (faction.members.get(0).equals(this))
        //{
        //    this.tsa.textSize((float)(am / 1.5));
        //    this.tsa.text(faction.name + " Elder", am, am);
        //}
        //else {
        //    this.tsa.textSize(am / 3);
        //    this.tsa.text(faction.name, am, am);
        //}
        //this.tsa.popMatrix();

    }

    // Wraparound
    public void borders()
    {
        /*
		 * if (location.x < 0) location.x = width; if (location.y < 0)
		 * location.y = height; if (location.x > width) location.x = 0; if
		 * (location.y > height) location.y = 0;
		 */
        if (location.x < 0)
        {
            velocity.x = -velocity.x;
            location.x = 1;
        }
        // location.x = width;
        if (location.y < 0)
        {
            velocity.y = -velocity.y;
            location.y = 1;
        }
        if (location.x > width)
        {
            velocity.x = -velocity.x;
            location.x = width - 1;
        }
        // location.x = 0;
        if (location.y > height)
        {
            velocity.y = -velocity.y;
            location.y = height - 1;
        }
    }

    // eating method

    // Separation
    // Method checks for nearby boids and steers away
    public Vector3 separate(ArrayList foods)
    {
        float desiredseparation = 0.0f;
        Vector3 steer = new Vector3(0, 0, 0);
        int count = 0;
        // For every boid in the system, check if it's too close
        /*
		 * for (Cell other : cells) { float d = Vector3.dist(location,
		 * other.location); // If the distance is greater than 0 and less than
		 * an arbitrary // amount (0 when you are yourself) if ((d > 0) && (d <
		 * desiredseparation)) { // Calculate vector pointing away from neighbor
		 * Vector3 diff = Vector3.sub(location, other.location);
		 * diff.normalize(); diff.div(d); // Weight by distance steer.add(diff);
		 * count++; // Keep track of how many } }
		 */

        foreach (Cell other in this.faction.members)
        {
            float d = Vector3.Distance(location, other.location);
            // If the distance is greater than 0 and less than an
            // arbitrary
            // amount (0 when you are yourself)
            if ((d > 0) && (d < desiredseparation))
            {
                // Calculate vector pointing away from neighbor
                Vector3 diff = location - other.location;

                diff.Normalize();
                diff = diff / d;
                //diff.div(d); // Weight by distance
                steer += diff;
                count++; // Keep track of how many
            }
        }

        // Average -- divide by how many
        if (count > 0)
        {
            steer /= ((float)count);
        }

        // As long as the vector is greater than 0
        if (steer.magnitude > 0)
        {
            // First two lines of code below could be condensed with new
            // Vector3 setMag() method
            // Not using this method until Processing.js catches up
            // steer.setMag(maxspeed);

            // Implement Reynolds: Steering = Desired - Velocity
            steer.Normalize();
            steer *= (maxspeed);
            steer -= (velocity);
            Vector3.ClampMagnitude(steer, (maxforce));
        }
        return steer;
    }

    // Alignment
    // For every nearby boid in the system, calculate the average velocity
    public Vector3 align(ArrayList foods)
    {
        float neighbordist = 50;
        Vector3 sum = new Vector3(0, 0);
        int count = 0;
        foreach (Food other in foods)
        {
          
            float d = Vector3.Distance(location, other.location);
            if ((d > 0) && (d < neighbordist))
            {
                sum += other.velocity;
                count++;
            }
        }
        foreach (int enid in this.faction.getEnemy())
        {
            if (enid != 0)
            {
                foreach (Cell other in ((Faction) this.tsa.factions[enid - 1]).members)
                {
                    float d = Vector3.Distance(location, other.location);
                    if ((d > 0) && (d < neighbordist) && this.am > other.am)
                    {
                        sum+=(other.velocity);
                        count++;
                    }
                }
            }
        }

        if (count > 0)
        {
            sum/=((float)count);
            // First two lines of code below could be condensed with new
            // Vector3 setMag() method
            // Not using this method until Processing.js catches up
            // sum.setMag(maxspeed);

            // Implement Reynolds: Steering = Desired - Velocity
            sum.Normalize();
            sum*=(maxspeed);
            Vector3 steer = (sum - velocity);
            Vector3.ClampMagnitude(steer, maxforce);
            return steer;
        }
        else {
            return new Vector3(0, 0);
        }
    }

    // Cohesion
    // For the average location (i.e. center) of all nearby boids, calculate
    // steering vector towards that location
    public Vector3 cohesion(ArrayList foods)
    {
        /*
		 * float neighbordist = 50; float enemydist = 50;
		 */
        Vector3 sum = new Vector3(0, 0); // Start with empty vector to
                                         // accumulate all locations
        int count = 0;

        foreach (Food other in foods)
        {
            float d = Vector3.Distance(location, other.location);
            //this.tsa.line(location.x, location.y, other.location.x, other.location.y);
            if ((d > 0) && (d < maxdist))
            {
                sum+=(other.location); // Add location
                count++;

            }
        }

        foreach (int enid in this.faction.getEnemy())
        {
            if (enid != 0)
            {
                foreach (Cell other in ((Faction) this.tsa.factions[enid - 1]).members)
                {
                    // System.out.println(this.faction.fid + ": " +
                    // factions[enid-1].fid);
                    float d = Vector3.Distance(location, other.location);
                    // line(location.x, location.y, other.location.x,
                    // other.location.y);
                    if ((d > 0) && (d < maxdist) && (other.am < this.am - 5))
                    {
                        sum+=(other.location);
                        // sum.add(other.location);// Add location
                        count++;
                    }
                }
            }
        }

        if (count > 0)
        {
            sum/=(count);
            //this.tsa.stroke(new Color(colord.x, colord.y, colord.z));

            return seek(sum); // Steer towards the location
        }
        else {
            return new Vector3(0, 0);
        }
    }

    // Use this for initialization
    void Start () {
        float angle = Random.Range(0, 6.28318530717958647693f);
        velocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
