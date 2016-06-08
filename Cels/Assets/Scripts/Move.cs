using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

	public float speed;
	public float targetOrtho;
	public float zoomSpeed = 5;
	public float minOrtho = 1.0f;
	public float maxOrtho = 20.0f;
	public float smoothSpeed = 2.0f;


	Vector3 target = new Vector3(0f,7f,0f);


	void Start() {
		targetOrtho = Camera.main.orthographicSize;
	}
	// Update is called once per frame
	void Update () {

		Vector3 mo = Camera.main.ScreenToWorldPoint(Input.mousePosition);


		if (Input.GetMouseButtonDown(0)) {
		
					//Limts the camera from going out of the map.
					if(mo.x <= 34 && mo.x >= -35)
						if(mo.z <= 34 && mo.z >= -35)
							target = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		
		}
		target.y = 100f;
	


		//changes scale by increasing and decreasing the Z 
		float scroll = Input.GetAxis ("Mouse ScrollWheel");
		if (scroll != 0.0f) {
			targetOrtho -= scroll * zoomSpeed;
			targetOrtho = Mathf.Clamp (targetOrtho, minOrtho, maxOrtho);
		}


			transform.position = Vector3.MoveTowards (transform.position, target, (speed * targetOrtho/2) * Time.deltaTime);
		

		Camera.main.orthographicSize = Mathf.MoveTowards (Camera.main.orthographicSize, targetOrtho, smoothSpeed);
	}


}
