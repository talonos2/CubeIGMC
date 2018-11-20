using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableTile : DamagableDisplay
{
    public Material crackedMaterial;
    private Material originalMaterial;
    private bool willSetFire;
    private new Renderer renderer;
    public GameObject firesPrefab;
    private GameObject fires;
    public float fireChance = .02f;

    public override void Damage()
    {
        this.renderer.material = crackedMaterial;
        if (willSetFire)
        {
            fires.gameObject.SetActive(true);
        }
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
        willSetFire = UnityEngine.Random.value < fireChance;
        if (willSetFire)
        {
            fires = GameObject.Instantiate(firesPrefab);
            fires.transform.parent = this.transform;
            fires.transform.localPosition = new Vector3(0, 0, 0);
            fires.gameObject.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
