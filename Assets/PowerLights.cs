using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerLights : MonoBehaviour
{
    public PowerLight[] lights;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SetAllLights(float howMuchOn)
    {
        for (int i = 0; i < lights.Length; i++)
        {
            float on = howMuchOn * lights.Length - i;
            lights[i].SetHowMuchOn(on);
        }
    }

    internal Vector3 GetTargetAtPercent(float v)
    {
        int lightNum = Mathf.FloorToInt(v * lights.Length);
        if (lightNum >= lights.Length) { lightNum = lights.Length - 1; }
        return lights[lightNum].theLight.transform.position;
    }
}
