using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalExplosionTilePiece : DeathEffect
{
    private DamagableTileDestroyer previousDestruction;
    private bool isBlowingUp;
    private bool willSpawnExplosion;
    float timeUntilBlowUp;
    public float spawnExplosionChance = .1f;

    private Collider col;
    private Rigidbody rb;

    private GameObject explosion;
    public GameObject explosionPrefab;

    public override void FinalityExplosion()
    {
        isBlowingUp = true;
    }

    // Use this for initialization
    void Start () {
        previousDestruction = this.gameObject.GetComponent<DamagableTileDestroyer>();
        willSpawnExplosion = UnityEngine.Random.value < spawnExplosionChance;
        if (willSpawnExplosion&&!previousDestruction.willGetDestroyed)
        {
            this.explosion = GameObject.Instantiate(explosionPrefab);
            this.explosion.transform.position = this.transform.position;
            this.explosion.SetActive(false);
        }
        rb = previousDestruction.toDestroy.GetComponent<Rigidbody>();
        col = previousDestruction.toDestroy.GetComponent<Collider>();
        timeUntilBlowUp = (1f - Mathf.Sqrt(UnityEngine.Random.value)) * 3;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (isBlowingUp)
        {
            if (previousDestruction.willGetDestroyed)
            {
                this.enabled = false;
                return;
            }
            this.timeUntilBlowUp -= Time.deltaTime;
            if (timeUntilBlowUp < 0)
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
                if (this.willSpawnExplosion)
                {
                    this.explosion.SetActive(true);
                }
                this.enabled = false;
            }
        }
	}
}
