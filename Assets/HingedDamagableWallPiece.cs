using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingedDamagableWallPiece : DamagableDisplay
{
    private Quaternion initialRotation;
    private Vector3 initialPosition;

    public override void Damage()
    {
        this.GetComponent<Rigidbody>().isKinematic = false;
        JointLimits l = this.GetComponent<HingeJoint>().limits;
        float newMax = UnityEngine.Random.Range(0, 120) + 30;
        l.max = newMax;
        this.GetComponent<HingeJoint>().limits = l;
        this.transform.localEulerAngles = new Vector3(-85, 90, 0);
        this.isDamaged = true;
    }

    public override void Fix()
    {
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.transform.position = initialPosition;
        this.transform.rotation = initialRotation;
        this.isDamaged = false;
    }

    // Use this for initialization
    void Start ()
    {
        this.initialRotation = this.transform.rotation;
        this.initialPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
