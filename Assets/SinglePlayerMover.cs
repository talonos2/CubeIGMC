using System;
using UnityEngine;

internal class SinglePlayerMover : Mover
{
    protected Combatant player;
    private AudioSource ominousTick;
    private bool isUpBeingHeld;
    private float timeSinceLastMoveUpEvent;
    protected bool justPressedUp;
    protected bool returnJustUpped;
    protected bool returnJustDowned;
    protected bool returnJustLefted;
    protected bool returnJustRighted;
    protected bool returnJustRebooted;
    protected bool returnJustDropped;
    protected bool returnJustCCWed;
    protected bool returnJustCWed;
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

    public SinglePlayerMover(Combatant player, AudioSource ominousTick)
    {
        this.player = player;
        this.ominousTick = ominousTick;
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

        returnJustUpped = HandleMovement("Vertical_P1", "Vertical_P2", ref isUpBeingHeld, ref timeSinceLastMoveUpEvent, ref justPressedUp, true);
        returnJustDowned = HandleMovement("Vertical_P1", "Vertical_P2", ref isDownBeingHeld, ref timeSinceLastMoveDownEvent, ref justPressedDown, false);
        returnJustLefted = HandleMovement("Horizontal_P1", "Horizontal_P2", ref isLeftBeingHeld, ref timeSinceLastMoveLeftEvent, ref justPressedLeft, false);
        returnJustRighted = HandleMovement("Horizontal_P1", "Horizontal_P2", ref isRightBeingHeld, ref timeSinceLastMoveRightEvent, ref justPressedRight, true);

        //Handle Reboot
        if ((Input.GetButton("Rotate1_P1") && Input.GetButton("Rotate2_P1")) || (Input.GetButton("Rotate1_P2") && Input.GetButton("Rotate2_P2")))
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
        returnJustDropped = ((Input.GetButtonDown("Place_P1") || Input.GetButtonDown("Place_P2"))&& !justExitedMenu);

        //Handle rotations:
        returnJustCCWed = (Input.GetButtonDown("Rotate1_P1") || Input.GetButtonDown("Rotate1_P2"));
        returnJustCWed = (Input.GetButtonDown("Rotate2_P1") || Input.GetButtonDown("Rotate2_P2"));
    }


    private bool HandleMovement(string name1, string name2, ref bool isBeingHeld, ref float timeSinceLastEvent, ref bool justPressed, bool positive)
    {
        bool toReturn = false;

        float speed = player.GetMovementSpeed();

        if ((positive ? (Input.GetAxis(name1) > 0 || Input.GetAxis(name2) > 0) : (Input.GetAxis(name1) < 0 || Input.GetAxis(name2) < 0)) && !isBeingHeld)
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
        if (!(positive ? (Input.GetAxis(name1) > 0 || Input.GetAxis(name2) > 0) : (Input.GetAxis(name1) < 0 || Input.GetAxis(name2) < 0)))
        {
            isBeingHeld = false;
        }

        return toReturn;
    }

}