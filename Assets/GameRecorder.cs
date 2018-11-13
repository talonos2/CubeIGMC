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

    public static readonly int UP_PRESS = 0;
    public static readonly int UP_RELEASE = 1;
    public static readonly int DOWN_PRESS = 2;
    public static readonly int DOWN_RELEASE = 3;
    public static readonly int LEFT_PRESS = 4;
    public static readonly int LEFT_RELEASE = 5;
    public static readonly int RIGHT_PRESS = 6;
    public static readonly int RIGHT_RELEASE = 7;

    public static readonly int CCW_ROTATE = 8;
    public static readonly int CW_ROTATE = 9;
    public static readonly int DROP = 10;

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
        string path = "Assets/AIs/"+dateString+".AI";
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(json);
        writer.Close();
    }

    /*
    static void WriteString()
    {
 

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("Test");
        writer.Close();

        //Re-import the file to update the reference in the editor
        AssetDatabase.ImportAsset(path);
        TextAsset asset = Resources.Load("test");

        //Print the text from the file
        Debug.Log(asset.text);
    } */
}