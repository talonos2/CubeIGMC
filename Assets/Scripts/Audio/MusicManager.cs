using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The music is brought with you from scene to scene. This class, a parent of the music, provides a singleton access point 
/// to all the music. This means that music can fade in and out between scenes, etc.
/// </summary>
public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public List<AudioSource> music = new List<AudioSource>();
    private float[] fadeLengths;
    private float[] fadeStartTimes;
    private float[] fadeVolumes;
    private float[] fadeStartVolumes;

    const int SKEAKY_SHIP = 0;
    const int BLASTING_THROUGH_WALLS = 1;
    const int CUTSCENE_1 = 2;
    const int TRACTOR_BEAM = 3;
    const int TITLE_SCREEN = 4;

    // Use this for initialization
    void Start()
    {
        fadeLengths = new float[music.Count];
        fadeStartTimes = new float[music.Count];
        fadeVolumes = new float[music.Count];
        fadeStartVolumes = new float[music.Count];
    }

    /// <summary>
    /// Sets up the Music Manager's singleton design pattern - only one instance of
    /// the manager is allowed to exist and is referenced by the variable "instance"
    /// </summary>
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        for (int x = 0; x < music.Count; x++)
        {
            if (fadeLengths[x] > 0)
            {
                fadeLengths[x] -= Time.deltaTime;
                music[x].volume = Mathf.Lerp(fadeStartVolumes[x], fadeVolumes[x], 1 - (fadeLengths[x] / fadeStartTimes[x]));
            }
        }
    }

    public void StopAllMusic()
    {
        for (int x = 0; x < music.Count; x++)
        {
            music[x].Stop();
            fadeLengths[x] = 0;
        }
    }

    public void FadeMusic(int num, float time, float volume)
    {
        fadeLengths[num] = time;
        fadeVolumes[num] = volume;
        fadeStartTimes[num] = time;
        fadeStartVolumes[num] = music[num].volume;
    }

    public void FadeOutc(int num, float time)
    {
        FadeMusic(num, time, 0);
    }
}
