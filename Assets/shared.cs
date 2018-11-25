using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class shared : NetworkBehaviour {

	// Use this for initialization
	void Start ()
    {
	    if (Sharedgamedata.issingleplayer==true)
        {
            gameObject.SetActive(false);
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
