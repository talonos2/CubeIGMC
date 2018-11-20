using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFX : DeathEffect
{
    private ParticleSystem s;

    private bool isExploding;
    private ParticleSystem.NoiseModule nm;
    private Rigidbody rb;
    private ConstantForce cf;

    float timeSinceExplosion;

    public override void FinalityExplosion()
    {
        this.isExploding = true;
        this.rb.isKinematic = false;
        this.nm.enabled = true;
        cf.force = new Vector3(0, UnityEngine.Random.value + 1.5f, 0);
    }

    // Use this for initialization
    void Start () {
        s = this.gameObject.GetComponent<ParticleSystem>();
        nm = s.noise;
        rb = gameObject.GetComponent<Rigidbody>();
        cf = gameObject.GetComponent<ConstantForce>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isExploding)
        {
            timeSinceExplosion += Time.deltaTime;
            nm.strength = timeSinceExplosion * .3f;
            if (timeSinceExplosion >= 3f)
            {
                this.enabled = false;
            }
        }
	}
}
