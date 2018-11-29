﻿using System;
using UnityEngine.Networking;

internal class LocalNetworkedPVPMover : SinglePlayerMover
{
    private EngineRoomNetworkManager engineRoomNetworkManager;
    private bool isServer;
    private GameGrid parentGrid;

    public LocalNetworkedPVPMover(Combatant player, UnityEngine.AudioSource ominousTick, EngineRoomNetworkManager engineRoomNetworkManager, GameGrid parentGrid, int seed, bool isServer) : base(player, ominousTick)
    {
        this.engineRoomNetworkManager = engineRoomNetworkManager;
        this.isServer = isServer;
        engineRoomNetworkManager.AttachToSenderMover(this, isServer);
        this.parentGrid = parentGrid;
    }

    internal override bool GetInput(MoverCommand command)
    {
        return base.GetInput(command);
    }

    internal override void Tick(bool justExitedMenu)
    {
        base.Tick(justExitedMenu);
        if (this.returnJustCCWed) { engineRoomNetworkManager.Send(AIPlayer.CCW_ROTATE); }
        if (this.returnJustCWed) { engineRoomNetworkManager.Send(AIPlayer.CW_ROTATE); }
        if (this.returnJustDowned) { engineRoomNetworkManager.Send(AIPlayer.DOWN); }
        if (this.returnJustDropped) { engineRoomNetworkManager.Send(AIPlayer.DROP); }
        if (this.returnJustLefted) { engineRoomNetworkManager.Send(AIPlayer.LEFT); }
        if (this.returnJustRebooted) { engineRoomNetworkManager.Send(AIPlayer.REBOOT); }
        if (this.returnJustRighted) { engineRoomNetworkManager.Send(AIPlayer.RIGHT); }
        if (this.returnJustUpped) { engineRoomNetworkManager.Send(AIPlayer.UP); }
    }

    internal void AcceptSeed(int value)
    {
        parentGrid.getSeedFromServer(value);
    }

    internal int GetParentSeed()
    {
        return parentGrid.seeded;
}
}