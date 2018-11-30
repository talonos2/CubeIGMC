using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeConversionManager : MonoBehaviour
{

    private List<CubeConversionRequest> toConvert = new List<CubeConversionRequest>();
    private float timeStamp;
    public InvisibleDelayedChargeGiver chargeGiverPrefab;
    public Transform endPoint;
    public float tweenTime = 1.5f;
    public float particleToShipTime = .6f;
    public float infusionParticleSpawnTime = 1;
    public Combatant player;
    public PowerupEffect powerupEffectPrefab;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        float oldTimeStamp = timeStamp;
        timeStamp += Time.deltaTime * 8;
        if ((int)oldTimeStamp != (int)timeStamp&&toConvert.Count>0) //IE: Every 1/8th second...
        {
            CubeConversionRequest request = toConvert[UnityEngine.Random.Range(0, toConvert.Count)];

            //float spinnerFloaterOffset = UnityEngine.Random.Range(.6f, 1.6f);
            float spinnerFloaterOffset = UnityEngine.Random.Range(.1f, .3f); //Very little swirl

            GameObject go = new GameObject();
            go.transform.SetParent(this.transform);
            go.transform.position = request.gameCube.transform.position + new Vector3(spinnerFloaterOffset, 0, 0);
            SpinnerFloater spinnerFloater = go.AddComponent<SpinnerFloater>();

            InvisibleDelayedChargeGiver chargeGiver = GameObject.Instantiate<InvisibleDelayedChargeGiver>(chargeGiverPrefab);
            chargeGiver.target = player;
            chargeGiver.delay = tweenTime+particleToShipTime;
            chargeGiver.type = request.type;
            chargeGiver.SetAmountForOneCube(request.type);
            spinnerFloater.chargeToGive = chargeGiver.amount;
            
            chargeGiver = GameObject.Instantiate<InvisibleDelayedChargeGiver>(chargeGiverPrefab);
            chargeGiver.target = player;
            chargeGiver.delay = infusionParticleSpawnTime;
            chargeGiver.type = PowerupType.ENERGY;
            chargeGiver.SetAmountAsEnergyCostForOneCube(request.type);
            spinnerFloater.energyToDrain = chargeGiver.amount*-1;

            spinnerFloater.cubeIOwn = request.gameCube;
            spinnerFloater.startPosition = go.transform.position;
            spinnerFloater.startRotation = spinnerFloater.cubeIOwn.transform.rotation;
            Transform target = player.getNextAttackCubeHolderPosition();
            spinnerFloater.endPosition = target.position;
            spinnerFloater.endRotation = target.rotation;
            spinnerFloater.offset = spinnerFloaterOffset;
            spinnerFloater.tweenTime = tweenTime-.1f;
            spinnerFloater.toDrainEnergyFrom = player;
            spinnerFloater.infusionParticleSpawnTime = infusionParticleSpawnTime;
            spinnerFloater.powerupEffect = powerupEffectPrefab;
            spinnerFloater.type = request.type;
            request.gameCube.transform.parent = spinnerFloater.transform;

            toConvert.Remove(request);
        }
	}

    internal void QueueCube(GameCube gameCube, PowerupType type)
    {
        switch (type)
        {
            case PowerupType.ATTACK:
                gameCube.coloredSection.material = gameCube.attackMaterial;
                break;
            case PowerupType.SHIELDS:
                gameCube.coloredSection.material = gameCube.shieldMaterial;
                break;
            default:
                Debug.LogError("Bad Cube type passed to the cube conversion manager: " + type + ", destroying Cube instead of enqueueing it.");
                GameObject.Destroy(gameCube.gameObject);
                return;
        }
        toConvert.Add(new CubeConversionRequest(gameCube, type));
    }
}

internal class CubeConversionRequest
{
    public PowerupType type;
    public GameCube gameCube;

    public CubeConversionRequest(GameCube gameCube, PowerupType type)
    {
        this.gameCube = gameCube;
        this.type = type;
    }
}