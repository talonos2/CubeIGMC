using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableTileDestroyer : DamagableDisplay
{
    public float percentDestroyChance = .5f;

    public GameObject toDestroy;

    internal bool willGetDestroyed;
    private bool animate;
    private float timeSinceDamaged;
    private Rigidbody rb;
    private BoxCollider col;
    private Renderer rend;

    public override void Damage()
    {
        if (willGetDestroyed)
        {
            rb.isKinematic = false;
            col.enabled = true;
            rb.velocity = new Vector3(
            UnityEngine.Random.Range(-1f, 1f),
            UnityEngine.Random.Range(-.1f, -.4f),
            UnityEngine.Random.Range(-1f, 1f));
            rb.AddTorque(
            UnityEngine.Random.Range(-40f, 40f),
            UnityEngine.Random.Range(-40f, 40f),
            UnityEngine.Random.Range(-40f, 40f));
        this.animate = true;
        }
        this.isDamaged = true;
    }

    public override void Fix()
    {
        throw new System.NotImplementedException();
    }

    // Use this for initialization
    void Start () {
        this.rb = toDestroy.GetComponent<Rigidbody>();
        this.col = toDestroy.GetComponent<BoxCollider>();
        this.rend = toDestroy.GetComponent<Renderer>();
        willGetDestroyed = UnityEngine.Random.value < percentDestroyChance;
		
	}

    void FixedUpdate()
    {
        if (isDamaged && animate)
        {
            timeSinceDamaged += Time.deltaTime;
            if (timeSinceDamaged > 2)
            {
                rend.enabled = false;
                rb.isKinematic = true;
                this.animate = false;
            }
        }
    }

}
