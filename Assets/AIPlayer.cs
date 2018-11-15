using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
internal class AIPlayer
{
    public int seed;
    public List<InputEvent> events = new List<InputEvent>();
    private bool justDropped;
    private bool justCWed;
    private bool justCCWed;
    private bool isPressingUp;
    private bool isPressingDown;
    private bool isPressingLeft;
    private bool isPressingRight;
    private float firstTimeStamp;

    public const int UP = 0;
    public const int DOWN = 2;
    public const int LEFT = 4;
    public const int RIGHT = 6;

    public const int CCW_ROTATE = 8;
    public const int CW_ROTATE = 9;
    public const int DROP = 10;



    public AIPlayer()
    {

    }

    public void TickAI()
    {
        if (firstTimeStamp==0)
        {
            this.firstTimeStamp = Time.realtimeSinceStartup;
        }
        float timeElapsed = Time.realtimeSinceStartup - this.firstTimeStamp;

        isPressingUp = false;
        isPressingDown = false;
        isPressingLeft = false;
        isPressingRight = false;
        justDropped = false;
        justCWed = false;
        justCCWed = false;
        if (events.Count == 0)
        {
            return;
        }
        while (events[0].time < timeElapsed)
        {
            InputEvent ie = events[0];
            Debug.Log("At " + ie.time + ", got" + ie.eventType);
            events.RemoveAt(0);
            switch (ie.eventType)
            {
                case UP:
                    isPressingUp = true;
                    break;
                case DOWN:
                    isPressingDown = true;
                    break;
                case LEFT:
                    isPressingLeft = true;
                    break;
                case RIGHT:
                    isPressingRight = true;
                    break;
                case DROP:
                    justDropped = true;
                    break;
                case CCW_ROTATE:
                    justCCWed = true;
                    break;
                case CW_ROTATE:
                    justCWed = true;
                    break;
                default:
                    Debug.LogError("Bad integer passed to AIPlayer.TickAI!");
                    break;
            }
            if (events.Count==0)
            {
                return;
            }
        }
    }

    public bool GetButtonDown(string v)
    {
        switch (v)
        {
            case "Place":
                return justDropped;
            case "Rotate2":
                return justCWed;
            case "Rotate1":
                return justCCWed;
            case "LEFT":
                return isPressingLeft;
            case "RIGHT":
                return isPressingRight;
            case "UP":
                return isPressingUp;
            case "DOWN":
                return isPressingDown;

            default:
                Debug.LogError("Bad string passed to AIPlayer.GetButtonDown: " + v);
                return false;
        }
    }

    public bool getButtonPressed(string v)
    {
        switch (v)
        {
            case "Left":
                return isPressingLeft;
            case "Right":
                return isPressingRight;
            case "Up":
                return isPressingUp;
            case "Down":
                return isPressingDown;
            default:
                Debug.LogError("Bad string passed to AIPlayer.GetButtonPressed: " + v);
                return false;
        }
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
}