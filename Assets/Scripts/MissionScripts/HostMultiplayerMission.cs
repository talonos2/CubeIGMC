using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostMultiplayerMission : NetworkedMission
{
    internal override EngineRoomGameType GameType()
    {
        return EngineRoomGameType.SERVER_PVP;
    }
}
