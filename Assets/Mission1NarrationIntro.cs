using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Narrate;

public class Mission1NarrationIntro : MonoBehaviour
{
    public TimerNarrationTrigger[] narrations;

	// Use this for initialization
	void Start ()
    {
        narrations[0].gameObject.SetActive(true);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
