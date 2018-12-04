using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A singleton class to hold information that must be passed between scenes. You might ask why
/// this is a gameobject instead of a static class. Didn't MissionManager cause us enough pain?
/// 
/// ...well, yes. But having this be a monobehavior makes testing certain scenes easier. For
/// instance, you can put "3" in the mission field in-editor and launch the "GameScreen"
/// scene to test mission 3. If this were a static class, you'd have had to edit the code,
/// test, then edit it back to avoid bugs.
/// 
/// The trick (and what killed us with te MissionManager during IGMC) is that we *CANNOT* store
/// references to things in this class if those things don't persist between scenes! Also, we
/// *CANNOT* have any *CHILDREN* of this GameObject store such data. As long as we follow this
/// rule, this class is helpful instead of dangerous. Be ye therefore warned.
/// 
/// Oh, and comment the member variables of this class a little more carefully than normal,
/// so we know how and where they should be assigned. Type "///" right before any variable
/// and it'll auto-create a litte summary tag, and the stuff you put in there will appear
/// as a tooltip when you see the object in the inspector.
/// </summary>
public class CrossScenePlayerData : MonoBehaviour
{
    public static CrossScenePlayerData instance;

    /// <summary>
    /// This was something in the mission manager, but I'm not sure this is used anymore. We might axe it soon.
    /// </summary>
    public static bool isEnteringAsHost;

    /// <summary>
    /// The MissionManager reads this when you enter the "GameScreen" scene to know what mission to load.
    /// </summary>
    public int missionNumToLoad;

    /// <summary>
    /// The file path to player 1's character sheet. Will probably be replaced with the character sheet itself.
    /// </summary>
    public string player1CharacterSheetPath;

    /// <summary>
    /// The file path to player 2's character sheet. Will probably be replaced with the character sheet itself.
    /// </summary>
    public string player2CharacterSheetPath;

    /// <summary>
    /// The ip of the host to connect to.
    /// </summary>
    public string ipSlug;

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

    void Start () {/*NOOP*/}
	void Update () {/*NOOP*/}
}
