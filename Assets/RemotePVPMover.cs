

using System;
using System.Collections.Generic;
using UnityEngine;

internal class RemoteNetworkedPVPMover : Mover
{

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

    public RemoteNetworkedPVPMover(EngineRoomNetworkManager ernm, bool isServer)
    {
        ernm.AttachToListenerMover(this, isServer);
    }

    internal void HandleMove(int command)
    {
        commands.Enqueue(command);
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
                    Debug.LogError("Bad integer passed to Network Player!");
                    break;
            }
        }
    }

    internal override void Tick(bool unused)
    {
        PlayNextCommand();
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


}