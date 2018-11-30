using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableWallPiece : DamagableDisplay
{
    float timeSinceDamaged = 0;
    private Material originalMat;
    private Material personalMatToMakeTranslucent;
    private new Renderer renderer;
    private bool animate;
    private bool hasSwitchedMaterial;

    private GameObject explosion;
    public GameObject explosionPrefab;

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
        this.animate = true;
        this.explosion.SetActive(true);
    }

    public override void Fix()
    {
        throw new System.NotImplementedException();
    }

    // Use this for initialization
    void Start()
    {
        this.renderer = this.GetComponent<Renderer>();
        this.originalMat = renderer.material;
        this.explosion = GameObject.Instantiate(explosionPrefab);
        this.explosion.transform.position = this.transform.position;
        this.explosion.SetActive(false);

    }

    void FixedUpdate()
    {
        if (isDamaged && animate)
        {
            timeSinceDamaged += Time.deltaTime;
            if (timeSinceDamaged > 3.5)
            {
                this.renderer.enabled = false;
                this.GetComponent<Rigidbody>().isKinematic = true;
                this.animate = false;
                this.renderer.material = this.originalMat;
            }
            if (timeSinceDamaged > 2.5 && !hasSwitchedMaterial)
            {
                this.personalMatToMakeTranslucent = new Material(this.originalMat);
                StandardShaderUtils.ChangeRenderMode(personalMatToMakeTranslucent, StandardShaderUtils.BlendMode.Transparent);
                this.renderer.material = personalMatToMakeTranslucent;
                hasSwitchedMaterial = true;
            }
            if (timeSinceDamaged > 2.5)
            {
                this.personalMatToMakeTranslucent.color = new Color(1, 1, 1, (3.5f - timeSinceDamaged) / 1f);
            }
        }
    }
}
