using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupEffect : MonoBehaviour
{
    public ParticleSystem particles;
    public float duration;
    public float height;

    private float delay;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float timeAlive;
    private bool hasStarted = false;
    private bool hasImpacted = false;
    private Combatant owner;
    private CubeType type;

	// Use this for initialization
	void Start ()
    {
        var main = particles.main;  //I don't know why this step is necessary, but it is. :/
        main.startLifetime = duration;
    }
	
	// Update is called once per frame
	void Update ()
    {
        timeAlive += Time.deltaTime;
        if (!hasStarted && timeAlive > delay)
        {
            hasStarted = true;
            particles.gameObject.SetActive(true);
            //particles.Emit(1);
        }

        if (hasStarted)
        {
            float percentThrough = (timeAlive - delay) / duration;

            //Warning, math ahead.
            this.transform.localScale = new Vector3(Mathf.Sin(Mathf.PI*percentThrough), Mathf.Sin(Mathf.PI * percentThrough), Mathf.Sin(Mathf.PI * percentThrough));
            this.transform.position = new Vector3(
                    startPosition.x + (endPosition.x - startPosition.x) * percentThrough,
                    startPosition.y + (endPosition.y - startPosition.y) * percentThrough + 4 * height * (-1*(percentThrough - .5f) * (percentThrough - .5f) + .25f),
                    startPosition.z + (endPosition.z - startPosition.z) * percentThrough);
            if (!hasImpacted && percentThrough >= 1)
            {
                OnImpact();
                hasImpacted = true;
            }
            if (percentThrough >= 2)
            {
                GameObject.Destroy(this.gameObject);
            }
        }
	}

    public void Initialize(Vector3 startPosition, Vector3 endPosition, float delay, CubeType type, Combatant owner)
    {
        this.startPosition = startPosition;
        this.endPosition = endPosition;
        this.delay = delay;
        this.owner = owner;
        this.type = type;

        Color color = Color.white;

        var main = particles.main;  //I don't know why this step is necessary, but it is. :/

        switch (type)
        {
            case CubeType.ATTACK:
                color = new Color(1,.5f,.5f);
                break;
            case CubeType.SHIELDS:
                color = Color.cyan;
                break;
            case CubeType.ENERGY:
                color = Color.yellow;
                break;
            case CubeType.PSI:
                color = Color.magenta;
                break;
        }

        main.startColor = color;
    }

    public void OnImpact()
    {

        switch (type)
        {
            case CubeType.ATTACK:
                owner.ChargeAttack(1);
                break;
            case CubeType.SHIELDS:
                owner.ChargeShields(1);
                break;
            case CubeType.ENERGY:
                owner.ChargeEnergy(1);
                break;
            case CubeType.PSI:
                Debug.Log("We actually haven't done this one yet.");
                break;
        }
    }

}
