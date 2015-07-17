using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

	// a static field accessible by code anywhere
	static public bool goalMet = false;

	void OnTriggerEnter( Collider other ) {
		// when the trigger is hit by something
		// check to see if it's a Projectile
		if (other.gameObject.tag == "Projectile") {
			// if so, set goalMet to true
			Goal.goalMet = true;
			// also set the alpha of the color to higher opacity
			Color c = GetComponent<Renderer>().material.color;
			c.a = 1;
			GetComponent<Renderer>().material.color = c;
		}
	}
}
