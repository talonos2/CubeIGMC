using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupEffect : MonoBehaviour
{
    public ParticleSystem particles;
    public Color colorOfParticle;
    public float duration;
    public float height;

    private float delay;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float timeAlive;
    private bool hasStarted = false;

	// Use this for initialization
	void Start ()
    {
        var main = particles.main;  //I don't know why this step is necessary, but it is. :/
        main.startColor = colorOfParticle;
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
            particles.Emit(1);
        }

        if (hasStarted)
        {
            float percentThrough = (timeAlive - delay) / duration;

            //Warning, math ahead.
            Debug.Log(percentThrough+", "+timeAlive+", "+hasStarted+", "+duration);
            this.transform.localScale = new Vector3(Mathf.Sin(Mathf.PI*percentThrough), Mathf.Sin(Mathf.PI * percentThrough), Mathf.Sin(Mathf.PI * percentThrough));
            this.transform.position = new Vector3(
                    startPosition.x + (endPosition.x - startPosition.x) * percentThrough,
                    startPosition.y + (endPosition.y - startPosition.y) * percentThrough + 4 * height * ((percentThrough - .5f) * (percentThrough - .5f) + .25f),
                    startPosition.z + (endPosition.z - startPosition.z) * percentThrough);

            if (percentThrough >= 2)
            {
                GameObject.Destroy(this.gameObject);
            }
        }
	}

    public void Initialize(Vector3 startPosition, Vector3 endPosition, float delay)
    {
        this.startPosition = startPosition;
        this.endPosition = endPosition;
        this.delay = delay;
    }

}
