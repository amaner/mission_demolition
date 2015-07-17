using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {

	// the only instance of the following camera
	// called a "singleton"
	static public FollowCam S;

	// fields set in the Unity Inspector pane
	public float easing = 0.05f;
	public Vector2 minXY;  // will default in Unity to [0,0]
	public bool __________; // the separator, as in Slingshot.cs

	// fields set dynamically
	public GameObject poi; // the point of interest
	// it will be filled in the Slingshot code
	// since it will be the projectile
	public float camZ; // the desired Z position of the camera

	void Awake() { // Awake runs before Start
		S = this;  // S will be the camera to which this is attached
		camZ = this.transform.position.z; // camZ is this object's z coord
	}

	void FixedUpdate() { // Update runs every frame
		Vector3 destination;
		// if there is no poi, return to P[0,0,0]
		if (poi == null) {
			destination = Vector3.zero;
		} else {
			// get the position of the poi
			destination = poi.transform.position;
			// if poi is a projectile, check to see if it's at rest
			if (poi.tag == "Projectile") {
				// if it is sleeping (that is, not moving)
				if (poi.GetComponent<Rigidbody>().IsSleeping ()) {
					// return to default view
					poi = null;
					// in the next update
					return;
				}
			}
		}
		// limit the X & Y to minimum values
		destination.x = Mathf.Max (minXY.x, destination.x);
		destination.y = Mathf.Max (minXY.y, destination.y);
		// interpolate from the current position toward destination
		destination = Vector3.Lerp (transform.position, destination, easing);
		// retain a destination.z of camZ
		destination.z = camZ;
		// set the camera to the destination
		transform.position = destination;
		// set the orthographic size of the camera to keep Ground in view
		this.GetComponent<Camera>().orthographicSize = destination.y + 10;
	}
}
