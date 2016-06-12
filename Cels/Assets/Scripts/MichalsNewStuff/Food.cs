using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour {

    public int lifetime = 0;
    public Vector3 location;
    public Vector3 velocity;
    public Vector3 acceleration;
    public float r;
    public float maxforce; // Maximum steering force
    public float maxspeed; // Maximum speed
    public Color colord = new Color(0, 0, 0);
    public int ratio;
    public int height;
    public int width;
    public GameObject baseobj;

    public Food(GameObject tsa, float x, float y, int ratio)
    {
        baseobj = tsa;
        height = (int) baseobj.GetComponent<Collider>().bounds.size.x;
        width = (int) baseobj.GetComponent<Collider>().bounds.size.y;
        acceleration = new Vector3(0, 0);

        // This is a new Vector3 method not yet implemented in JS
        // velocity = Vector3.random2D();

        // Leaving the code temporarily this way so that this example runs
        // in JS
        float angle = Random.Range(0, 6.283185307179586476925286766559f);
        velocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));

        location = new Vector3(x, y);
        r = 2.0f;
        maxspeed = 0.8f;
        maxforce = 0.03f;
        this.ratio = ratio;
        c1 = new Color(255, 50, 50);
        c2 = new Color(50, 255, 50);
        c3 = new Color(50, 50, 255);
    }

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
        applyForce(sep);
        applyForce(ali);
        applyForce(coh);
    }

    public void update()
    {
        // Update velocity
        velocity += (acceleration);
        // Limit speed
        Vector3.ClampMagnitude(velocity, maxspeed); // velocity.limit(maxspeed);
        location += (velocity);
        // Reset accelertion to 0 each cycle
        acceleration *= (0);
    }
    public Vector3 seek(Vector3 target)
    {
        Vector3 desired = target - location; // A vector
                                                         // pointing from
                                                         // the location
                                                         // to the target
                                                         // Scale to maximum speed
        desired.Normalize();
        desired *= (maxspeed);

        // Above two lines of code below could be condensed with new Vector3
        // setMag() method
        // Not using this method until Processing.js catches up
        // desired.setMag(maxspeed);

        // Steering = Desired minus Velocity
        Vector3 steer = desired - velocity;
        Vector3.ClampMagnitude(steer, maxforce); // Limit to maximum steering force
        return steer;
    }
    private Color cColor = Color.black;
    private Color c1, c2, c3;
    public void render()
    {
        // Draw a triangle rotated in the direction of velocity
        // drawGridLines(100, false);
        float headingAngle = (float)Mathf.Atan2(velocity.y, velocity.x);
        float theta = headingAngle + (Mathf.Deg2Rad * (90));

        // heading2D() above is now heading() but leaving old syntax until
        // Processing.js catches up
        float xtime = 100 - lifetime / 100;


        fill(cColor);
        
        transform.Translate(location.x, location.y, 0);
        transform.Rotate(theta, 0,0);
        //if (this.tsa.n < 1)
        //{
        //    cColor = this.tsa.lerpColor(c1, c2, this.tsa.n);
        //}
        //else if (this.tsa.n < 2)
        //{
        //    cColor = this.tsa.lerpColor(c2, c3, this.tsa.n - 1);
        //}
        //else if (this.tsa.n < 3)
        //{
        //    cColor = this.tsa.lerpColor(c3, c1, this.tsa.n - 2);
        //}
        ////System.out.println("IMA KILL YOU" + this.tsa.n);
        //// this.tsa.fill(this.tsa.color(this.tsa.lerpColor(c1, c2, amt)));
        //this.tsa.pushMatrix();
        //this.tsa.stroke(0);
        //this.tsa.triangle(3, 0, 6, 8, 0, 8);

        //this.tsa.noStroke();
        //this.tsa.popMatrix();
        //this.tsa.popMatrix();

    }

    // Wraparound
    public void borders()
    {
        if (location.x < 0)
            location.x = width;
        if (location.y < 0)
            location.y = height;
        if (location.x > width)
            location.x = 0;
        if (location.y > height)
            location.y = 0;

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

    // Separation
    // Method checks for nearby boids and steers away
    public Vector3 separate(ArrayList foods)
    {
        float desiredseparation = 25.0f;
        Vector3 steer = new Vector3(0, 0, 0);
        int count = 0;
        // For every boid in the system, check if it's too close
        foreach (Food other in foods)
        {
            float d = Vector3.Distance(location, other.location);
            // If the distance is greater than 0 and less than an arbitrary
            // amount (0 when you are yourself)
            if ((d > 0) && (d < desiredseparation))
            {
                // Calculate vector pointing away from neighbor
                Vector3 diff = location - other.location;
                diff.Normalize();
                diff /= d; // Weight by distance
                steer += (diff);
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
                sum += (other.velocity);
                count++;
            }
        }
        if (count > 0)
        {
            sum /= ((float)count);
            // First two lines of code below could be condensed with new
            // Vector3 setMag() method
            // Not using this method until Processing.js catches up
            // sum.setMag(maxspeed);

            // Implement Reynolds: Steering = Desired - Velocity
            sum.Normalize();
            sum *= ((maxspeed));
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
        float neighbordist = 50;
        Vector3 sum = new Vector3(0, 0); // Start with empty vector to
                                         // accumulate all locations
        int count = 0;

        foreach (Food other in foods)
        {
            float d = Vector3.Distance(location, other.location);
            //this.tsa.line(location.x, location.y, other.location.x, other.location.y);
            if ((d > 0) && (d < neighbordist))
            {
                sum += (other.location); // Add location
                count++;
            }
        }
        if (count > 0)
        {
            sum /= (count);
            //this.tsa.stroke(this.tsa.color(colord.x, colord.y, colord.z));

            return seek(sum); // Steer towards the location
        }
        else {
            return new Vector3(0, 0);
        }
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void fill(Color c)
    {
        GameObject whateverGameObject = this.gameObject;
        Color whateverColor = c;

        MeshRenderer gameObjectRenderer = whateverGameObject.GetComponent<MeshRenderer>();

        Material newMaterial = new Material(Shader.Find("Standard"));

        newMaterial.color = whateverColor;
        gameObjectRenderer.material = newMaterial;
    }
}
