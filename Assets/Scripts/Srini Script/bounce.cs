// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class bounce : MonoBehaviour {
	void Update ()
	{
		transform.Rotate (10,0,0*Time.deltaTime); //rotates 50 degrees per second around z axis
	}
}