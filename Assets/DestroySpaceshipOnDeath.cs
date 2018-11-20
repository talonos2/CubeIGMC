using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySpaceshipOnDeath : DeathEffect
{

    bool isExploding;
    float timeSinceDied;

    public List<Rigidbody> stuffToBlowUp;
    public List<Renderer> stuffToHide;

    public override void FinalityExplosion()
    {
        isExploding = true;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isExploding)
        {
            foreach (Rigidbody rb in stuffToBlowUp)
            {
                Vector3 waggle = new Vector3(UnityEngine.Random.value - .5f, UnityEngine.Random.value - .5f, UnityEngine.Random.value - .5f);
                rb.isKinematic = false;
                rb.AddExplosionForce(1, this.transform.position + waggle, 3, 0);
            }
            foreach (Renderer r in stuffToHide)
            {
                r.enabled = false;
            }
        }
	}
}
