using UnityEngine;

internal class LocalPVPMover : Mover
{
    private Combatant player;
    private AudioSource ominousTick;
    private bool isUpBeingHeld;
    private float timeSinceLastMoveUpEvent;
    private bool justPressedUp;
    private bool returnJustUpped;
    private bool returnJustDowned;
    private bool returnJustLefted;
    private bool returnJustRighted;
    private bool returnJustRebooted;
    private bool returnJustDropped;
    private bool returnJustCCWed;
    private bool returnJustCWed;
    private bool isDownBeingHeld;
    private float timeSinceLastMoveDownEvent;
    private bool justPressedDown;
    private bool justPressedRight;
    private bool isLeftBeingHeld;
    private bool isRightBeingHeld;
    private float timeSinceLastMoveLeftEvent;
    private float timeSinceLastMoveRightEvent;
    private bool justPressedLeft;
    const float buttonMashDebounceInput = .2f;

    private float timeHeldBothRotatesAtOnce;
    private string vert;
    private string horiz;
    private string drop;
    private string rot1;
    private string rot2;

    public LocalPVPMover(Combatant player, AudioSource ominousTick, bool oneOrTwo)
    {
        this.player = player;
        this.ominousTick = ominousTick;
        if (oneOrTwo)
        {
            vert = "Vertical_P1";
            horiz = "Horizontal_P1";
            drop = "Place_P1";
            rot1 = "Rotate1_P1";
            rot2 = "Rotate2_P1";
        }
        else
        {
            vert = "Vertical_P2";
            horiz = "Horizontal_P2";
            drop = "Place_P2";
            rot1 = "Rotate1_P2";
            rot2 = "Rotate2_P2";
        }
    }

    internal override bool GetInput(MoverCommand command)
    {
        switch (command)
        {
            case MoverCommand.DROP:
                return returnJustDropped;
            case MoverCommand.CW:
                return returnJustCWed;
            case MoverCommand.CCW:
                return returnJustCCWed;
            case MoverCommand.LEFT:
                return returnJustLefted;
            case MoverCommand.RIGHT:
                return returnJustRighted;
            case MoverCommand.UP:
                return returnJustUpped;
            case MoverCommand.DOWN:
                return returnJustDowned;
            case MoverCommand.REBOOT:
                return returnJustRebooted;
        }
        return false;
    }

    internal override void Tick(bool justExitedMenu)
    {
        returnJustUpped = false;
        returnJustDowned = false;
        returnJustLefted = false;
        returnJustRighted = false;
        returnJustRebooted = false;
        returnJustDropped = false;
        returnJustCCWed = false;
        returnJustCWed = false;

        returnJustUpped = HandleMovement(vert,ref isUpBeingHeld, ref timeSinceLastMoveUpEvent, ref justPressedUp, true);
        returnJustDowned = HandleMovement(vert, ref isDownBeingHeld, ref timeSinceLastMoveDownEvent, ref justPressedDown, false);
        returnJustLefted = HandleMovement(horiz, ref isLeftBeingHeld, ref timeSinceLastMoveLeftEvent, ref justPressedLeft, false);
        returnJustRighted = HandleMovement(horiz, ref isRightBeingHeld, ref timeSinceLastMoveRightEvent, ref justPressedRight, true);

        //Handle Reboot
        if ((Input.GetButton(rot1) && Input.GetButton(rot2)))
        {
            float oldTimeHeld = timeHeldBothRotatesAtOnce;
            timeHeldBothRotatesAtOnce += Time.deltaTime;
            if ((int)timeHeldBothRotatesAtOnce != (int)oldTimeHeld)
            {
                ominousTick.Play();
            }
            if (timeHeldBothRotatesAtOnce > 5)
            {
                returnJustRebooted = true;
                timeHeldBothRotatesAtOnce = 0;
            }
        }
        else
        {
            timeHeldBothRotatesAtOnce = 0;
        }


        //Handle Drop
        returnJustDropped = ((Input.GetButtonDown(drop)) && !justExitedMenu);

        //Handle rotations:
        returnJustCCWed = (Input.GetButtonDown(rot1));
        returnJustCWed = (Input.GetButtonDown(rot2));
    }


    private bool HandleMovement(string name, ref bool isBeingHeld, ref float timeSinceLastEvent, ref bool justPressed, bool positive)
    {
        bool toReturn = false;

        float speed = player.GetMovementSpeed();

        if ((positive ? (Input.GetAxis(name) > 0) : (Input.GetAxis(name) < 0)) && !isBeingHeld)
        {
            justPressed = true;
            isBeingHeld = true;
            timeSinceLastEvent = speed * -buttonMashDebounceInput;
        }

        if (isBeingHeld)
        {
            if (timeSinceLastEvent > speed || justPressed) //Or justpressed here lets you move immediately upon pressing the button.
            {
                justPressed = false;
                toReturn = true;
                timeSinceLastEvent %= speed;
            }


            timeSinceLastEvent += Time.deltaTime;

        }
        if (!(positive ? (Input.GetAxis(name) > 0) : (Input.GetAxis(name) < 0)))
        {
            isBeingHeld = false;
        }

        return toReturn;
    }
}