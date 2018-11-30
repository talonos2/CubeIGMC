using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableLight : DamagableDisplay
{
    private float oldIntensity;
    private float timeSinceDamage;
    private float timeforFirstFlicker;
    private float timeForPause;
    private new Light light;

    public override void Damage()
    {
        isDamaged = true;
    }

    public override void Fix()
    {
        throw new System.NotImplementedException();
    }

    // Use this for initialization
    void Start () {
        this.light = this.GetComponent<Light>();
        this.oldIntensity = light.intensity;
        timeforFirstFlicker = UnityEngine.Random.Range(.1f, .3f);
        timeForPause = UnityEngine.Random.Range(.4f, .8f);
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (isDamaged)
        {
            timeSinceDamage += Time.deltaTime;
            if (timeSinceDamage > (timeforFirstFlicker*2+timeForPause))
            {
                float t = (timeSinceDamage - (timeforFirstFlicker * 2 + timeForPause)) / (timeforFirstFlicker * 2);
                light.intensity = Mathf.Lerp(oldIntensity, 0, t);
            }
            else if (timeSinceDamage > timeforFirstFlicker*2)
            {
                //noop
            }
            else if (timeSinceDamage > timeforFirstFlicker)
            {
                float t = (timeSinceDamage - timeforFirstFlicker) / (timeforFirstFlicker);
                light.intensity = Mathf.Lerp(0, oldIntensity, t);
            }
            else
            {
                float t = timeSinceDamage / timeforFirstFlicker;
                light.intensity = Mathf.Lerp(oldIntensity, 0, t);
            }
        }
	}
}
