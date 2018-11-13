using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCube : MonoBehaviour
{
    public Material energyMaterial;
    public Material attackMaterial;
    public Material shieldMaterial;
    public Material psiMaterial;
    public Material glowMaterial;

    public MeshRenderer coloredSection;
    public MeshRenderer lightOnTop;

    public CubeType type;

    public float fallSpeed = 3;

    private bool isFalling;
    private float timeSinceFallStarted;
    private float delayUntilFallStarts;
    private float height;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isFalling)
        {
            timeSinceFallStarted += Time.deltaTime;
            if (timeSinceFallStarted >= delayUntilFallStarts)
            {
                float timeFalling = timeSinceFallStarted - delayUntilFallStarts;
                this.transform.position = new Vector3(transform.position.x, height - (fallSpeed * timeFalling * timeFalling), transform.position.z);

                if (timeFalling > 1.5)
                {
                    GameObject.Destroy(this.gameObject);
                }
            }
        }
		
	}

    internal void Initialize(Combatant owner, SeededRandom dice)
    {
        this.type = owner.GetRandomCubeType(dice);
        switch(type)
        {
            case CubeType.ATTACK:
                this.coloredSection.material = attackMaterial;
                break;
            case CubeType.SHIELDS:
                this.coloredSection.material = shieldMaterial;
                break;
            case CubeType.ENERGY:
                this.coloredSection.material = energyMaterial;
                break;
            case CubeType.PSI:
                this.coloredSection.material = psiMaterial;
                break;
        }
        
    }

    internal void Sink(float v)
    {
        isFalling = true;
        delayUntilFallStarts = v;
        height = this.transform.position.y;
        lightOnTop.material = glowMaterial;
    }
}
