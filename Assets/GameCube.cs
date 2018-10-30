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

    public MeshRenderer coloredSection;

    Combatant owner;
    CubeType type;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    internal void Initialize(Combatant owner)
    {
        this.owner = owner;
        this.type = owner.GetRandomCubeType();
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

}
