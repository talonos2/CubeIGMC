using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player1 : NetworkBehaviour
{
    GameObject home;
    GameObject guest;

    GameGrid gameGridHome;
    GameGrid gameGridGuest;
    GameGrid myGameGrid;

    public bool justExitedMenu;
    private float timeHeldBothRotatesAtOnce;


    bool started = false;

	// Use this for initialization
	void Start ()
    {
        //Time.timeScale = 0;
        home = GameObject.Find("GameGridHome");
        guest = GameObject.Find("GameGridGuest");
        gameGridHome = home.GetComponent<GameGrid>();
        gameGridGuest = guest.GetComponent<GameGrid>();
        Sharedgamedata.issingleplayer = false;

        if (Sharedgamedata.logger == false)
        {
            //gameGridHome.isPlayerOne = true;
            myGameGrid = gameGridHome;
            Sharedgamedata.logger = true;
        }
        else
        {
            myGameGrid = gameGridGuest;
        }





    }
	
	// Update is called once per frame
	void ProxyUpdate ()
    {

/*
        if (isLocalPlayer)
        {
            //            Debug.Log(gameGridHome);
                        Debug.Log("I do one");
            gameGridHome.proxyUpdate();
            //            Debug.Log("local");
        }
        else
        {
            if (!started)
            {
                Debug.Log("timestarts");
                Time.timeScale = 1;
                started = true;
            }
            Debug.Log("I do the other");
            gameGridGuest.proxyUpdate();
//            Debug.Log("not local");

        }
*/



    }

    void Update()
    {



        if (Time.timeScale == 0)
        {
            justExitedMenu = true;
            return;
        }

        if (Input.GetButton("Rotate1_P1") && Input.GetButton("Rotate2_P1") || (Input.GetButton("Rotate1_P2") && Input.GetButton("Rotate2_P2")))
        {
            float oldTimeHeld = timeHeldBothRotatesAtOnce;
            timeHeldBothRotatesAtOnce += Time.deltaTime;
            if ((int)timeHeldBothRotatesAtOnce != (int)oldTimeHeld)
            {
                myGameGrid.ominousTick.Play();
            }
            if (timeHeldBothRotatesAtOnce > 5)
            {
                myGameGrid.Reboot();
                timeHeldBothRotatesAtOnce = 0;
            }
        }
        else
        {
            timeHeldBothRotatesAtOnce = 0;
        }


        myGameGrid.HandleUpMovement();
        myGameGrid.HandleDownMovement();
        myGameGrid.HandleLeftMovement();
        myGameGrid.HandleRightMovement();

        if (((Input.GetButtonDown("Place_P1") || Input.GetButtonDown("Place_P2")) && justExitedMenu == false))
        {
            //Fallback: If there's somebohw someting directly underneath you, do not place. Should never happen in practice.
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (myGameGrid.currentPiece.HasBlockAt(x, y) && myGameGrid.IsInInvalidArea(myGameGrid.currentPiecePosition.x + x - 1, myGameGrid.currentPiecePosition.y + y - 1))
                    {
                        return;
                    }
                }
            }

            myGameGrid.DropPiece();
        }

        if (Input.GetButtonDown("Rotate1_P1") || (Input.GetButtonDown("Rotate1_P2")))
        {
            bool[,] surroundings = new bool[3, 3];
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    surroundings[x, y] = myGameGrid.IsObstructedAt(myGameGrid.currentPiecePosition.x + x - 1, myGameGrid.currentPiecePosition.y + y - 1);
                }
            }
            if (myGameGrid.currentPiece.rotateCCW(surroundings))
            {
                myGameGrid.prevPieceRotation = myGameGrid.currentPieceRotation;
                myGameGrid.currentPieceRotation = (myGameGrid.currentPieceRotation + 5) % 4;
                myGameGrid.timeSinceLastRot = 0f;
                myGameGrid.currentPiece.PlaySlideSound();
            }
            else
            {
                //Make some sort of sound.
            }
        }

        if (Input.GetButtonDown("Rotate2_P1") || (Input.GetButtonDown("Rotate2_P2")))
        {
            bool[,] surroundings = new bool[3, 3];
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    surroundings[x, y] = myGameGrid.IsObstructedAt(myGameGrid.currentPiecePosition.x + x - 1, myGameGrid.currentPiecePosition.y + y - 1);
                }
            }
            if (myGameGrid.currentPiece.RotateCW(surroundings))
            {
                myGameGrid.prevPieceRotation = myGameGrid.currentPieceRotation;
                myGameGrid.currentPieceRotation = (myGameGrid.currentPieceRotation + 3) % 4;
                myGameGrid.timeSinceLastRot = 0f;
                myGameGrid.currentPiece.PlaySlideSound();
            }
            else
            {
                //Make some sort of sound.
            }
        }

        myGameGrid.UpdateCurrentPieceTransform();

        for (int x = 0; x < GameGrid.numCells.x; x++)
        {
            for (int y = 0; y < GameGrid.numCells.y; y++)
            {
                if (myGameGrid.grid[x, y] != null)
                {
                    Debug.DrawLine(new Vector3(this.transform.position.x + x - GameGrid.numCells.x / 2, 0, this.transform.position.y + y - GameGrid.numCells.y / 2), new Vector3(this.transform.position.x + x - GameGrid.numCells.x / 2, 4, this.transform.position.y + y - GameGrid.numCells.y / 2));
                }
            }
        }


    }




    public void HandleUpMovement()
    {
        float speed = myGameGrid.player.GetMovementSpeed();
        if ((Input.GetAxis("Vertical_P1") > 0 || Input.GetAxis("Vertical_P2") > 0 && !myGameGrid.isUpBeingHeld))
        {
            myGameGrid.isUpBeingHeld = true;
            myGameGrid.timeSinceLastMoveUpEvent = speed * -myGameGrid.buttonMashDebounceInput;
            myGameGrid.justPressedUp = true;
        }
        if (myGameGrid.isUpBeingHeld)
        {
            if (myGameGrid.timeSinceLastMoveUpEvent > speed || myGameGrid.justPressedUp)
            {
                myGameGrid.justPressedUp = false;
                myGameGrid.TryGoUp();

                myGameGrid.timeSinceLastMoveUpEvent %= speed;
            }
            myGameGrid.timeSinceLastMoveUpEvent += Time.deltaTime;
        }
        if (!(Input.GetAxis("Vertical_P1") > 0 || Input.GetAxis("Vertical_P2") > 0))
        {
            myGameGrid.isUpBeingHeld = false;
        }
    }


    public void HandleDownMovement()
    {
        float speed = myGameGrid.player.GetMovementSpeed();
        if ((Input.GetAxis("Vertical_P1") < 0 || Input.GetAxis("Vertical_P2") < 0 ) && !myGameGrid.isDownBeingHeld)
        {
            myGameGrid.isDownBeingHeld = true;
            myGameGrid.timeSinceLastMoveDownEvent = speed * -myGameGrid.buttonMashDebounceInput;
            myGameGrid.justPressedDown = true;
        }
        if (myGameGrid.isDownBeingHeld)
        {
            if (myGameGrid.timeSinceLastMoveDownEvent > speed || myGameGrid.justPressedDown)
            {
                myGameGrid.justPressedDown = false;
                myGameGrid.TryGoDown();

                myGameGrid.timeSinceLastMoveDownEvent %= speed;
            }
            myGameGrid.timeSinceLastMoveDownEvent += Time.deltaTime;
        }
        if (!(Input.GetAxis("Vertical_P1") < 0 || Input.GetAxis("Vertical_P2") < 0))
        {
            myGameGrid.isDownBeingHeld = false;
        }
    }

    public void HandleLeftMovement()
    {
        float speed = myGameGrid.player.GetMovementSpeed();
        if ((Input.GetAxis("Horizontal_P1") < 0 || Input.GetAxis("Horizontal_P2") < 0) && !myGameGrid.isLeftBeingHeld)
        {
            myGameGrid.isLeftBeingHeld = true;
            myGameGrid.timeSinceLastMoveLeftEvent = speed * -myGameGrid.buttonMashDebounceInput;
            myGameGrid.justPressedLeft = true;
        }
        if (myGameGrid.isLeftBeingHeld)
        {
            if (myGameGrid.timeSinceLastMoveLeftEvent > speed || myGameGrid.justPressedLeft)
            {
                myGameGrid.justPressedLeft = false;
                myGameGrid.TryGoLeft();

                myGameGrid.timeSinceLastMoveLeftEvent %= speed;
            }
            myGameGrid.timeSinceLastMoveLeftEvent += Time.deltaTime;
        }
        if (!(Input.GetAxis("Horizontal_P1") < 0 || Input.GetAxis("Horizontal_P2") < 0))
        {
            myGameGrid.isLeftBeingHeld = false;
        }
    }


    public void HandleRightMovement()
    {
        float speed = myGameGrid.player.GetMovementSpeed();
        if ((Input.GetAxis("Horizontal_P1") > 0 || Input.GetAxis("Horizontal_P2") > 0) && !myGameGrid.isRightBeingHeld)
        {
            myGameGrid.isRightBeingHeld = true;
            myGameGrid.timeSinceLastMoveRightEvent = speed * -myGameGrid.buttonMashDebounceInput;
            myGameGrid.justPressedRight = true;
        }
        if (myGameGrid.isRightBeingHeld)
        {
            if (myGameGrid.timeSinceLastMoveRightEvent > speed || myGameGrid.justPressedRight)
            {
                myGameGrid.justPressedRight = false;
                myGameGrid.TryGoRight();

                myGameGrid.timeSinceLastMoveRightEvent %= speed;
            }
            myGameGrid.timeSinceLastMoveRightEvent += Time.deltaTime;
        }
        if (!(Input.GetAxis("Horizontal_P1") > 0 || Input.GetAxis("Horizontal_P2") > 0))
        {
            myGameGrid.isRightBeingHeld = false;
        }
    }


}
