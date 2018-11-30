using System;
using UnityEngine;

public abstract class Mission : MonoBehaviour
{
    internal abstract void Unblock();
    internal abstract AIParams GetAIParams();

    internal abstract EngineRoomGameType GameType();

    internal void Win(bool shop)
    {
        PointerHolder p = MissionManager.instance.pointers;
        p.levelFinishedImage.sprite = p.campaignVictory;
        p.nextMissionButton.gameObject.SetActive(true);
        if (shop)
        {
            p.shopButton.gameObject.SetActive(true);
        }
        p.replayMissionText.text = "Replay";
        p.youHaveDiedScreen.gameObject.SetActive(true);
    }

    internal void Lose()
    {
        PointerHolder p = MissionManager.instance.pointers;
        p.levelFinishedImage.sprite = p.campaignDefeat;
        p.nextMissionButton.gameObject.SetActive(false);
        p.shopButton.gameObject.SetActive(false);
        p.replayMissionText.text = "Retry";
        p.youHaveDiedScreen.gameObject.SetActive(true);
    }
}