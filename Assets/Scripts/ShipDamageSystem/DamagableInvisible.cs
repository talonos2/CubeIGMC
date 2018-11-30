using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableInvisible : DamagableDisplay
{

    public override void Damage()
    {
        this.isDamaged = true;
    }

    public override void Fix()
    {
        throw new System.NotImplementedException();
    }

    // Use this for initialization
    void Start ()
    {

	}
	
}
