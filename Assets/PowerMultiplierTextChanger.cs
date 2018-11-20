using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerMultiplierTextChanger : MonoBehaviour
{
    public Sprite[] sprites;
    public SpriteRenderer toChange;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void ChangeText(int newMulter)
    {
        toChange.sprite = sprites[newMulter];
    }
}
