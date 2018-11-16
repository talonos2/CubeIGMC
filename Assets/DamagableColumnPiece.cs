using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableColumnPiece : DamagableDisplay
{
    float timeSinceDamaged = 0;
    public override void Damage()
    {
        this.GetComponent<Rigidbody>().isKinematic = false;
        this.GetComponent<BoxCollider>().enabled = true;
        this.GetComponent<Rigidbody>().velocity = new Vector3(
            UnityEngine.Random.Range(-2f, 2f),
            UnityEngine.Random.Range(3, 10),
            UnityEngine.Random.Range(-2f, 2f));
        this.GetComponent<Rigidbody>().AddTorque(
            UnityEngine.Random.Range(-20f, 20f),
            UnityEngine.Random.Range(-20f, 20f),
            UnityEngine.Random.Range(-20f, 20f));
        this.isDamaged = true;
    }

    public override void Fix()
    {
        throw new System.NotImplementedException();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	void FixedUpdate ()
    {
		if (isDamaged)
        {
            timeSinceDamaged += Time.deltaTime;
            if (timeSinceDamaged > 5)
            {
                this.GetComponent<Renderer>().enabled = false;
                this.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
	}
}
