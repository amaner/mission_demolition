using UnityEngine;
using System.Collections;

public class Slingshot : MonoBehaviour {

	// fields set in the Unity Inspector pane
	public GameObject prefabProjectile;
	public float velocityMult = 4f;
	public bool __________;
	// fields set dynamically (by code)
	public GameObject launchPoint;
	public Vector3 launchPos;
	public GameObject projectile;
	public bool aimingMode;

	// Awake runs before the game starts
	void Awake() {
		// find the launchpoint's transform, and store it in a local variable
		Transform launchPointTrans = transform.Find ("LaunchPoint");
		// make a reference to the launchpoint
		launchPoint = launchPointTrans.gameObject;
		// make it inactive by default
		launchPoint.SetActive (false);
		// set the launch position to be the launch point's position
		launchPos = launchPointTrans.position;
	}
	void OnMouseEnter() {
		// if moused over, make the launchpoint active
		launchPoint.SetActive (true);
	}
	// 
	void OnMouseExit() {
		// if moused off, make the launchpoint inactive
		launchPoint.SetActive (false);
	}
	// for when the user presses down the left mouse button
	void OnMouseDown() {
		// the player has pressed the mouse button while over Slingshot
		aimingMode = true;
		// instantiate a projectile
		projectile = Instantiate (prefabProjectile) as GameObject;
		// start it at the launchpoint
		projectile.transform.position = launchPos;
		// set it to isKinematic for now (so it won't fall down
		// while we're aiming
		projectile.GetComponent<Rigidbody>().isKinematic = true;
	}

	void Update() {
		// if the slingshot is not in aiming mode, don't run this code
		if (!aimingMode) return;
		// get the current mouse position in 2D screen coords
		Vector3 mousePos2D = Input.mousePosition;
		// convert to 3D world coordinates
		mousePos2D.z = -Camera.main.transform.position.z;
		Vector3 mousePos3D = Camera.main.ScreenToWorldPoint (mousePos2D);
		// find the delta from the launch pos to the mousepos3d
		Vector3 mouseDelta = mousePos3D - launchPos;
		// limit mouseDelta to the radius of the slingshot sphere collider
		float maxMagnitude = this.GetComponent<SphereCollider> ().radius;
		if (mouseDelta.magnitude > maxMagnitude) { // if it steps outside of this
			mouseDelta.Normalize ();    // set its magnitude to 1
			mouseDelta *= maxMagnitude; // and multiply by maxmag
		}
		// move the projectile to the new position
		Vector3 projPos = launchPos + mouseDelta;
		projectile.transform.position = projPos;
		// how to exit aiming mode
		if (Input.GetMouseButtonUp (0)) {
			// the mouse button has been released
			aimingMode = false;
			// the projectile should no longer be kinematic
			// it should be flying through the air
			projectile.GetComponent<Rigidbody>().isKinematic = false;
			projectile.GetComponent<Rigidbody>().velocity = -mouseDelta * velocityMult;

			// set the following camera's POI to this projectile
			FollowCam.S.poi = projectile;

			// open the projectile field for another possible projectile
			// this doesn't kill the projectile...
			projectile = null;
		}
	}
}
