using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerFloater : MonoBehaviour
{
    internal Vector3 startPosition;
    internal Vector3 endPosition;
    internal float tweenTime;
    internal Combatant toDrainEnergyFrom;
    internal int energyToDrain;
    internal int chargeToGive;
    public PowerupType type;

    public GameCube cubeIOwn;

    private float timeSpentSpinning = 0;
    internal float infusionParticleSpawnTime;
    private bool hasSpawnedParticles = false;
    public PowerupEffect powerupEffect;
    internal float offset;

    private static Camera mainCamera;
    internal Quaternion startRotation;
    internal Quaternion endRotation;

    // Use this for initialization
    void Start () {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        timeSpentSpinning += Time.deltaTime;
        float percentThrough = timeSpentSpinning / tweenTime;
        percentThrough = Mathf.Sqrt(percentThrough);
        if (percentThrough >= 1)
        {
            GameObject.Destroy(cubeIOwn.gameObject);
            percentThrough = 1;
        }
        Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, percentThrough);
        Quaternion newRotation = Quaternion.Slerp(startRotation, endRotation, percentThrough);
        this.transform.position = newPosition;
        cubeIOwn.transform.rotation = newRotation;
        this.transform.rotation = Quaternion.Euler(0,percentThrough*360,0);
        if (percentThrough >= 1)
        {
            Vector2 screenSpace = toDrainEnergyFrom.cameraToShake.GetComponent<Camera>().WorldToScreenPoint(cubeIOwn.transform.position);
            Vector3 worldSpace = mainCamera.ScreenToWorldPoint(new Vector3(screenSpace.x, screenSpace.y, 15));

            for (int x = 0; x < energyToDrain; x++)
            {
                PowerupEffect p = GameObject.Instantiate(powerupEffect);
                p.Initialize(worldSpace, toDrainEnergyFrom.pawn.transform.position, 0, type);
                p.duration = .5f;
                p.height = .1f;
            }

            cubeIOwn.transform.parent = this.transform.parent;
            GameObject.Destroy(this.gameObject);
        }

        if (timeSpentSpinning > infusionParticleSpawnTime && !hasSpawnedParticles)
        {
            toDrainEnergyFrom.StartNewParticleBarrage();
            for (int x = 0; x < energyToDrain; x++)
            {
                Vector3 from = toDrainEnergyFrom.GetTargetOfParticle(PowerupType.ENERGY, -1);
                Vector3 to = endPosition + new Vector3(-offset, 0, 0) ;
                PowerupEffect p = GameObject.Instantiate(powerupEffect);
                p.Initialize(from, to, 0, PowerupType.ENERGY);
                hasSpawnedParticles = true;
            }
        }


    }
}
