using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalExplosionHingePiece : DeathEffect
{
    float timeSinceDeath = 0;
    private HingeJoint hj;
    float timeUntilIFallOff = 0;
    private bool isExploding;

    public override void FinalityExplosion()
    {
        isExploding = true;
    }

    // Use this for initialization
    void Start ()
    {
        hj = this.GetComponent<HingeJoint>();
        timeUntilIFallOff = UnityEngine.Random.value * UnityEngine.Random.value * 3;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (isExploding)
        {
            timeSinceDeath += Time.deltaTime;
            if (timeSinceDeath>timeUntilIFallOff)
            {
                GameObject.Destroy(hj);
                this.enabled = false;
            }
        }
	}
}
