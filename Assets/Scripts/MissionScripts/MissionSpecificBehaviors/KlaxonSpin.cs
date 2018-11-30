using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KlaxonSpin : DamagableDisplay
{
    private bool on;
    public GameObject light1;
    public GameObject light2;

    public override void Damage()
    {
        isDamaged = true;
    }

    public override void Fix()
    {
        throw new System.NotImplementedException();
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (on)
        {
            this.gameObject.transform.localRotation = Quaternion.Euler(this.gameObject.transform.localRotation.eulerAngles + new Vector3(0, 180 / (6.468f / 6f) * Time.deltaTime, 0));
        }
    }

    public void TurnOn()
    {
        light1.SetActive(true);
        light2.SetActive(true);
        on = true;
    }
}
