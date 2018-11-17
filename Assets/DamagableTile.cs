using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableTile : DamagableDisplay
{
    public Material crackedMaterial;
    private Material originalMaterial;
    private new Renderer renderer;

    public override void Damage()
    {
        this.renderer.material = crackedMaterial;
        this.isDamaged = true;
    }

    public override void Fix()
    {
        throw new System.NotImplementedException();
    }

    // Use this for initialization
    void Start () {
        this.renderer = this.GetComponent<Renderer>();
        this.originalMaterial = renderer.material;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
