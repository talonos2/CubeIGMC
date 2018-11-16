using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerLight : DamagableDisplay {

    private Material mat;
    public Light theLight;
    public ParticleSystem particles;
    public ParticleSystem brokenParticles;
    private float oldOn = 0;
    public Texture damagedMaterial;
    public Texture actuallyDontEmit;


    // Use this for initialization
    void Start () {
        mat = new Material(this.GetComponent<Renderer>().material);
        this.GetComponent<Renderer>().material = mat;

    }

    public void SetHowMuchOn(float on)
    {
        on = Mathf.Min(Mathf.Max(on, 0), 1);
        if (on==oldOn)
        {
            return;
        }

        this.mat.SetColor("_EmissionColor", new Color(.24f * on, .24f * on, .12f * on));
        if (on > .01f)
        {
            if (!isDamaged)
            {
                brokenParticles.gameObject.SetActive(false);
                particles.gameObject.SetActive(true);
                ParticleSystem.EmissionModule em = particles.emission;
                em.rateOverTime = 10f * on;
                theLight.gameObject.SetActive(true);
                theLight.intensity = on * 10f;
            }
            else
            {
                brokenParticles.gameObject.SetActive(true);
                particles.gameObject.SetActive(false);
                theLight.gameObject.SetActive(false);
            }
        }
        else
        {
            theLight.gameObject.SetActive(false);
            particles.gameObject.SetActive(false);
            brokenParticles.gameObject.SetActive(false);
        }

        oldOn = on;
    }

    public override void Damage()
    {
        this.mat.SetTexture("_MainTex", this.damagedMaterial);
        this.mat.SetTexture("_EmissionMap", this.actuallyDontEmit);
        this.isDamaged = true;

        float realOldOn = oldOn;
        oldOn = -1;

        SetHowMuchOn(realOldOn);
    }

    public override void Fix()
    {
        throw new System.NotImplementedException();
    }
}
