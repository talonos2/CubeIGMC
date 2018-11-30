using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
internal class AIPlayer : Mover
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
    private bool justRebooted;

    public const int UP = 0;
    public const int DOWN = 2;
    public const int LEFT = 4;
    public const int RIGHT = 6;

    public const int CCW_ROTATE = 8;
    public const int CW_ROTATE = 9;
    public const int DROP = 10;
    public const int REBOOT = 11;

    private Queue<int> commands = new Queue<int>();

    public AIPlayer()
    {

    }

    private float timeElapsed = 0;

    private void BufferFurtherCommands()
    { 
        if (!MissionManager.freezeAI)
        {
            timeElapsed += Time.deltaTime;
        }
        if (events.Count == 0)
        {
            return;
        }
        while (events[0].time < timeElapsed)
        {
            InputEvent ie = events[0];
            commands.Enqueue(ie.eventType);
            events.RemoveAt(0);
            if (events.Count == 0)
            {
                return;
            }
        }
    }

    private void PlayNextCommand()
    {
        isPressingUp = false;
        isPressingDown = false;
        isPressingLeft = false;
        isPressingRight = false;
        justDropped = false;
        justCWed = false;
        justCCWed = false;
        if (commands.Count > 0)
        {
            int command = commands.Dequeue();
            switch (command)
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
                case REBOOT:
                    justRebooted = true;
                    break;
                default:
                    Debug.LogError("Bad integer passed to AIPlayer.TickAI!");
                    break;
            }
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

    public void MakeRobotic(float speed)
    {
            float timeSoFar = 0;
            foreach (InputEvent ie in events)
            {
                timeSoFar += speed;
                ie.time = timeSoFar;
        }
    }

    public void MakeLoop()
    {
        float timeToPlay = events[events.Count - 1].time;
        for (int x = 0; x < 4; x++)
        {
            int threeGuessesHowICausedAnInfiniteLoopHere = events.Count;
            for (int y = 0; y < threeGuessesHowICausedAnInfiniteLoopHere; y++)
            {
                InputEvent newIE = new InputEvent(events[y].eventType, events[y].time+timeToPlay);
                events.Add(newIE);
            }
            timeToPlay *= 2;
        }
    }

    internal override bool GetInput(MoverCommand command)
    {
        switch (command)
        {
            case MoverCommand.DROP:
                return justDropped;
            case MoverCommand.CW:
                return justCWed;
            case MoverCommand.CCW:
                return justCCWed;
            case MoverCommand.LEFT:
                return isPressingLeft;
            case MoverCommand.RIGHT:
                return isPressingRight;
            case MoverCommand.UP:
                return isPressingUp;
            case MoverCommand.DOWN:
                return isPressingDown;
            case MoverCommand.REBOOT:
                return justRebooted;
        }
        return false;
    }

    internal override void Tick(bool unused)
    {
        PlayNextCommand();
        BufferFurtherCommands();
    }
}