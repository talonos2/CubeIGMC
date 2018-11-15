using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerLight : MonoBehaviour {

    private Material mat;
    public Light theLight;
    public ParticleSystem particles;
    private float oldOn = 0;

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
            theLight.gameObject.SetActive(true);
            particles.gameObject.SetActive(true);
            theLight.intensity = on * 10f;
            ParticleSystem.EmissionModule em = particles.emission;
            em.rateOverTime = 10f*on;
        }
        else
        {
            theLight.gameObject.SetActive(false);
            particles.gameObject.SetActive(false);
        }

        oldOn = on;
    }
}
