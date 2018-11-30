using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class GameRecorder
{
    public int seed;
    public List<InputEvent> events = new List<InputEvent>();

    private readonly float firstTimeStamp;

    public const int UP = 0;
    public const int DOWN = 2;
    public const int LEFT = 4;
    public const int RIGHT = 6;

    public const int CCW_ROTATE = 8;
    public const int CW_ROTATE = 9;
    public const int DROP = 10;
    public const int REBOOT = 11;

    public GameRecorder(int seed)
    {
        this.seed = seed;
        this.firstTimeStamp = Time.realtimeSinceStartup;
    }

    public void RegisterEvent (int eventType)
    {
        this.events.Add(new InputEvent(eventType, Time.realtimeSinceStartup - firstTimeStamp));
    }

    [Serializable]
    public class InputEvent
    {
        public float time;
        public int eventType;

        public InputEvent(int eventType, float time)
        {
            this.eventType = eventType;
            this.time = time;
        }
 
    }

    public void PrintOut()
    {
        string json = JsonUtility.ToJson(this);
        string dateString = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute;
        string path = "Assets/AIs/"+dateString+".AI.json";
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(json);
        writer.Close();
    }

}