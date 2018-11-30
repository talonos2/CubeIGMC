using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestMultiplayerMission : NetworkedMission
{
    internal override EngineRoomGameType GameType()
    {
        return EngineRoomGameType.CLIENT_PVP;
    }
}