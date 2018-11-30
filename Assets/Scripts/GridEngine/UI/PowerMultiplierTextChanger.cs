using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerMultiplierTextChanger : MonoBehaviour
{
    public Sprite[] sprites;
    public SpriteRenderer toChange;
    private int oldMulter;
    public ComboParticleHolder holder;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void ChangeText(int newMulter)
    {
        if (oldMulter != newMulter)
        {
            GameObject particles = GameObject.Instantiate(holder.comboParticles[newMulter].gameObject);
            particles.transform.parent = this.toChange.transform;
            particles.transform.localPosition = new Vector3(0, 0, 0);
            particles.transform.localScale = new Vector3(1.4285f, 1.4285f, 1.4285f);
            oldMulter = newMulter;
        }
        toChange.sprite = sprites[newMulter];
    }
}
