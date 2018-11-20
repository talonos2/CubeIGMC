using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldChanger : MonoBehaviour
{

    private Renderer shieldRenderer;
    public Combatant shipToMonitor;
    private float recentShieldDamage = 0; //This is as a proportion of max shield amount
    private Material shieldMaterial;

    const float fullShieldFresnelIntensity = 20;
    const float fullShieldFresnelWidth = 1;
    const float fullShieldDistortion = 3;

    const float fresnelIntensityMultiplierOnDamage = 1;
    const float fresnelWidthMultiplierOnDamage = 1;
    const float distortionMultiplierOnDamage = 3;

    const float proportionOfShieldThatCountsAsMaxDamage = .2f;

    // Use this for initialization
    void Start () {
        shieldRenderer = this.gameObject.GetComponent<Renderer>();
        //Copy the material over
        shieldRenderer.material = new Material(shieldRenderer.material);
        shieldMaterial = shieldRenderer.material;
	}
	
	// Update is called once per frame
	void Update ()
    {
        float percentUp = Mathf.Sqrt(shipToMonitor.getShieldPercent());
        float damageMultiplier = (Mathf.Min(proportionOfShieldThatCountsAsMaxDamage, recentShieldDamage) * (1/ proportionOfShieldThatCountsAsMaxDamage));

        float fresnelInensity = percentUp * fullShieldFresnelIntensity * (1+damageMultiplier* fresnelIntensityMultiplierOnDamage);
        float fresnelWidth = percentUp * fullShieldFresnelWidth * (1 + damageMultiplier* fresnelWidthMultiplierOnDamage);
        float distortion = percentUp * fullShieldDistortion * (1 + damageMultiplier* distortionMultiplierOnDamage);

        shieldMaterial.SetFloat("_Fresnel", fresnelInensity+.01f);
        shieldMaterial.SetFloat("_FresnelWidth", fresnelWidth);
        shieldMaterial.SetFloat("_Distort", distortion);


    }


}
