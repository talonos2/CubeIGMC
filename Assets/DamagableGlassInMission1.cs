using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableGlassInMission1 : DamagableDisplay {

    public Material postShatter;
    public GameObject explosionPrefab;
    private GameObject explosion;
    private Material originalMat;
    private new Renderer renderer;

    public override void Damage()
    {
        this.isDamaged = true;
        this.renderer.material = postShatter;
        this.explosion.SetActive(true);
    }

    public override void Fix()
    {
        throw new System.NotImplementedException();
    }

    // Use this for initialization
    void Start () {
        this.renderer = this.GetComponent<Renderer>();
        this.originalMat = renderer.material;
        this.explosion = GameObject.Instantiate(explosionPrefab);
        this.explosion.transform.position = this.transform.position;
        this.explosion.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
