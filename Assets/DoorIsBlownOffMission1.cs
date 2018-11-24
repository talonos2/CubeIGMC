using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorIsBlownOffMission1 : DeathEffect
{

    public override void FinalityExplosion()
    {
        Rigidbody rb = this.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForceAtPosition(new Vector3(1, 0, 0), this.gameObject.transform.position + new Vector3(0, 1, 0), ForceMode.Impulse);

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
