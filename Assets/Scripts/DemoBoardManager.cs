using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoBoardManager : MonoBehaviour
{
    public static DemoBoardManager Instance { set; get; }
    private int boardSize;
    public int noOfPlayers;
    public int currentPlayer = 1;
    public int noOfAi;
    public int P1Score = 0;
    public int P2Score = 0;
    public int P3Score = 0;
    public int P4Score = 0;
    public int P1Wins = 0;
    public int P2Wins = 0;
    public int P3Wins = 0;
    public int P4Wins = 0;
    public bool Endgame = false;
    public bool ResetCheck = false;
    public int Winner = 0;
    public GameObject HorizontalLine;
    public GameObject VerticalLine;
    public GameObject Square;
    private const float SquareSpace = 1.5f;
    private const float LineSpace = 0.75f;
    public GameObject MouseTarget;
    public GameObject GameCamera;
    //public GameObject PersistGameObject;
    public bool IsAiTurn;
    public GameObject AIPlayer1;
    public GameObject AIPlayer2;
    public GameObject AIPlayer3;
    public GameObject AIPlayer4;
    public bool IsAiHard;
    public bool isP1Ai;
    public bool isP2Ai;
    public bool isP3Ai;
    public bool isP4Ai;
    public bool isP1empty;
    public bool isP2empty;
    public bool isP3empty;
    public bool isP4empty;
    public bool IsEmptyTurn;
    public List<string> PlayerSlot;

    public bool GamePaused;

    private GameObject Square1 = null;
    private GameObject Square2 = null;

    public List<GameObject> Lines;
    public List<GameObject> Squares;

    public Material defaultMat;
    public Material selectedMat;
    public Material highlightMat;
    public List<Material> PlayerColours;

    public int TurnCounter;

    //Not needed for DEMO
    //Stuff for online
    private Client client;
    private bool amIHost;
    public bool isMyTurn = false;
    public bool PlayingOnline = false;
    public int ReceivedX = -1;
    public int ReceivedY = -1;
    public bool ReceivedHorizontal = false;
    public int ReceivedTurnCount;
    private bool SwitchFirstPlayer = true;

    public GameObject DemoBoard;
    public Vector3 MainLocation;
    public Vector3 BoardSpawnLocation;
    public Vector3 BoardDefaultLocation;
    public Quaternion BoardDefaultRotation;

    void Start()
    {
        Instance = this;
        //PersistGameObject = GameObject.Find("PersistGameObject");
        //noOfPlayers = PersistGameObject.GetComponent<PersistObject>().noOfPlayers;
        DemoSetPlayers();
        //boardSize = PersistGameObject.GetComponent<PersistObject>().boardSize;
        DemoBoardSizeGen();
        //noOfAi = PersistGameObject.GetComponent<PersistObject>().noOfAI;
        client = FindObjectOfType<Client>();
        TurnCounter = 0;
        ReceivedTurnCount = 0;
        //AiEnable();
        MainLocation = Camera.main.GetComponent<Transform>().position;
        BoardDefaultLocation = DemoBoard.GetComponent<Transform>().position;
        BoardDefaultRotation = DemoBoard.GetComponent<Transform>().rotation;
        AiEnableNew();
        SpawnHorizontals();
        SpawnVerticals();
        SpawnSquares();
        SetCamera();
        JauntyBoardOffset();

        //if (GameObject.Find("Client") != null)
        if (client != null)
        {
            PlayingOnline = true;
            //amIHost = GameObject.Find("Client").GetComponent<Client>().isHost;
            amIHost = client.GetComponent<Client>().isHost;
        }

    }

    void Update()
    {
        if (GamePaused == false || (GamePaused == true && PlayingOnline == true))
        {
            AiTurnCheck();
            EmptyTurnCheck();

            if (PlayingOnline)
            {
                OnlineTurnCheck();
            }

            if (IsAiTurn == false && !IsEmptyTurn && (!PlayingOnline | (PlayingOnline && isMyTurn)))
            {
                SelectLine();

                if (Input.GetMouseButtonDown(0))
                {
                    if (MouseTarget != null)
                    {
                        if (!MouseTarget.GetComponent<Line>().isClicked)
                        {
                            MouseTarget.GetComponent<Line>().isClicked = true;
                            MouseTarget.GetComponent<MeshRenderer>().material = selectedMat;
                            SelectedLineCheck();
                            SquareCheck();
                        }
                    }
                    //check there is a target and if the target is available
                }
            }
            else if (IsAiTurn == true && MouseTarget != null && MouseTarget.GetComponent<Line>().isClicked == false && !Endgame)
            {
                MouseTarget.GetComponent<Line>().isClicked = true;
                MouseTarget.GetComponent<MeshRenderer>().material = selectedMat;
                SelectedLineCheck();
                SquareCheck();
                //IsAiTurn = false;  
            }
            else if (IsEmptyTurn)
            {
                currentPlayer += 1;
            }
            else if (PlayingOnline && !isMyTurn)
            {
                if (ReceivedX != -1)
                {
                    SelectLine();
                    if (MouseTarget != null)
                    {
                        MouseTarget.GetComponent<Line>().isClicked = true;
                        MouseTarget.GetComponent<MeshRenderer>().material = selectedMat;
                        SelectedLineCheck();
                        SquareCheck();
                        ReceivedX = -1;
                        ReceivedY = -1;
                    }
                }

            }
            if (currentPlayer > 4)
            {
                currentPlayer = 1;
            }

            Reset();
        }
    }

    private void DemoSetPlayers()
    {
        noOfPlayers = Random.Range(2, 5);
        noOfAi = 0;

        while (noOfAi < noOfPlayers)
        {
            int R = Random.Range(0, 4);
            if (PlayerSlot[R] == "" || PlayerSlot[R] == null)
            {
                PlayerSlot[R] = "easyai";
                noOfAi += 1;
            }
        }
        for (int P = 0; P < 4; P++)
        {
            if (PlayerSlot[P] == "" || PlayerSlot[P] == null)
            {
                PlayerSlot[P] = "empty";
            }
        }
        EmptySlotHandler();
    }

    private void DemoBoardSizeGen()
    {
        boardSize = Random.Range(2, 25);
    }

    private Vector3 GetBoardPlacement()
    {
        float f;
        f = ((boardSize * SquareSpace) / 2) - LineSpace;
         //maincamera is inactive, store this on start;
        BoardSpawnLocation = MainLocation + new Vector3(-1*f, 1 * f, (boardSize * 1.5f));
        //Vector3 JauntyOffset = new Vector3(2 * f, 0, -boardSize);
        return BoardSpawnLocation;// + JauntyOffset;
    }

    private void JauntyBoardOffset()
    {
        //references
        //size 2 board wants  (7, -4, 0)offset
        //size 3 board wants  (6.5,-3,  0)offset
        //size 4 board wants  ( 7, -3,  1)offset
        //size 5 board wants  (5.5,-2.5,1)offset
        //size 6 board wants  (,,)          offset
        //size 7 board wants  (,,)          offset
        //size 8 board wants  (,,)           offset
        //size 9 board wants  ( 4, -1,  2)  offset
        //size 10 board wants (,,)           offset
        //size 11 board wants (,,)           offset
        //size 12 board wants (,,)           offset
        //size 13 board wants (,,)           offset
        //size 14 board wants ( 1,  0,  3)  offset
        //size 15 board wants ( 0,  0,  3)  offset
        //size 16 board wants (,,)          offset
        //size 17 board wants (,,)          offset
        //size 18 board wants ( 1,  1,  4)   offset
        //size 19 board wants (,,)          offset
        //size 20 board wants (,,)          offset
        //size 21 board wants (,,)          offset
        //size 22 board wants (,,)           offset
        //size 23 board wants (-3, 2.5, 5)   offset
        //size 24 board wants (,,)           offset

        Vector3 LocationOffset = new Vector3(((float)boardSize * -0.45f) + 8f, ((float)boardSize * 0.275f) - 4f, (float)boardSize /4f);
        Quaternion RotationOffset = Quaternion.Euler(new Vector3(25, 40, -6));
        DemoBoard.GetComponent<Transform>().SetPositionAndRotation(BoardDefaultLocation+LocationOffset, BoardDefaultRotation*RotationOffset);
    }
    private void SetCamera()
    {
        float f = ((boardSize * SquareSpace) / 2) - LineSpace;
        GameCamera.GetComponent<Transform>().position = GetBoardPlacement() + new Vector3(f, -1 * f, -1 * (boardSize * 1.5f));// + DemoBoard.GetComponent<Transform>().position; just parented it to the board instead
    }

    private void SpawnHorizontals()
    {
        for (int n = 0; n < boardSize + 1; n++)
        {
            for (int i = 0; i < boardSize; i++)
            {
                GameObject go = Instantiate(HorizontalLine, new Vector3((0 + i) * SquareSpace, LineSpace - (n * SquareSpace), 0) + GetBoardPlacement(), Quaternion.Euler(0, 0, 90), this.transform);
                go.GetComponent<Line>().xPos = i;
                go.GetComponent<Line>().yPos = n;
                go.GetComponent<Line>().isHorizontal = true;
                Lines.Add(go);
            }
        }
    }

    private void SpawnVerticals()
    {
        for (int n = 0; n < boardSize; n++)
        {
            for (int i = 0; i < boardSize + 1; i++)
            {
                GameObject go = Instantiate(VerticalLine, new Vector3(-LineSpace + (i * SquareSpace), n * -SquareSpace, 0) + GetBoardPlacement(), Quaternion.identity, this.transform);
                go.GetComponent<Line>().xPos = i;
                go.GetComponent<Line>().yPos = n;
                Lines.Add(go);
            }
        }
    }

    private void SpawnSquares()
    {
        for (int n = 0; n < boardSize; n++)
        {
            for (int i = 0; i < boardSize; i++)
            {
                GameObject go = Instantiate(Square, new Vector3(i * SquareSpace, n * -SquareSpace, 0) + GetBoardPlacement(), Quaternion.Euler(0, 180, 0), this.transform);
                go.GetComponent<Square>().xPos = i;
                go.GetComponent<Square>().yPos = n;
                Squares.Add(go);
            }
        }
    }

    private void SelectLine()
    {
        if (IsAiTurn == false && (!PlayingOnline | (PlayingOnline && isMyTurn)))
        {
            if (!Camera.main)
                return;

            if (MouseTarget != null)
            {
                if (!MouseTarget.GetComponent<Line>().isClicked)
                {
                    MouseTarget.GetComponent<MeshRenderer>().material = defaultMat;
                    MouseTarget = null;
                }
            }

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 90.0f, LayerMask.GetMask("Line")))
            {
                MouseTarget = hit.transform.gameObject;

                if (!MouseTarget.GetComponent<Line>().isClicked)
                {
                    MouseTarget.GetComponent<MeshRenderer>().material = highlightMat;
                }
            }
            //else
            //{
            //    if(MouseTarget != null && !MouseTarget.GetComponent<Line>().isClicked)
            //    {
            //        MouseTarget.GetComponent<MeshRenderer>().material = defaultMat;
            //        MouseTarget = null;
            //    }
            //    else
            //    MouseTarget = null;
            //}
        }
        if (IsAiTurn == false && (PlayingOnline && !isMyTurn))
        {
            if (MouseTarget != null)
            {
                if (!MouseTarget.GetComponent<Line>().isClicked)
                {
                    MouseTarget.GetComponent<Line>().isClicked = true;
                    MouseTarget = null;
                }
            }
            foreach (GameObject line in Lines)
            {
                if (line.GetComponent<Line>().isHorizontal == ReceivedHorizontal)
                {
                    if ((line.GetComponent<Line>().xPos == ReceivedX) && (line.GetComponent<Line>().yPos == ReceivedY))
                    {
                        MouseTarget = line;
                    }
                }
            }

        }
    }

    private void SelectedLineCheck()
    {
        //Check which squares this line affects then flag them for further checks

        int x = MouseTarget.GetComponent<Line>().xPos;
        int y = MouseTarget.GetComponent<Line>().yPos;
        bool Horizontal = MouseTarget.GetComponent<Line>().isHorizontal;

        if (Horizontal)
        {
            //if horizontal line, the squares to check are above or below, and will be between 0 and boardsize on Y and between 0 and boardsize -1 on X, there will be a maximum of 2
            //check the square with the same X and Y and the square with same X and Y-1
            if (y == 0)
            {
                foreach (GameObject go in Squares)
                {
                    if (go.GetComponent<Square>().xPos == x && go.GetComponent<Square>().yPos == y)
                    {
                        Square1 = go;
                        break;
                    }
                }
            }
            else if (y == boardSize)
            {
                foreach (GameObject go in Squares)
                {
                    if (go.GetComponent<Square>().xPos == x && go.GetComponent<Square>().yPos + 1 == y)
                    {
                        Square1 = go;
                        break;
                    }
                }
            }
            else
            {
                foreach (GameObject go in Squares)
                {
                    if (go.GetComponent<Square>().xPos == x && go.GetComponent<Square>().yPos == y)
                    {
                        Square1 = go;
                    }
                }
                foreach (GameObject go in Squares)
                {
                    if (go.GetComponent<Square>().xPos == x && go.GetComponent<Square>().yPos + 1 == y)
                    {
                        Square2 = go;
                        break;
                    }
                }
            }
        }
        else
        {
            //if vertical line, the squares to check are above or below, and will be between 0 and boardsize -1 on Y and between 0 and boardsize on X, there will be a maximum of 2
            //check the square with the same X and Y and the square with X-1 and same Y
            if (x == 0)
            {
                foreach (GameObject go in Squares)
                {
                    if (go.GetComponent<Square>().xPos == x && go.GetComponent<Square>().yPos == y)
                    {
                        Square1 = go;
                        break;
                    }
                }
            }
            else if (x == boardSize)
            {
                foreach (GameObject go in Squares)
                {
                    if (go.GetComponent<Square>().xPos + 1 == x && go.GetComponent<Square>().yPos == y)
                    {
                        Square1 = go;
                        break;
                    }
                }
            }
            else
            {
                foreach (GameObject go in Squares)
                {
                    if (go.GetComponent<Square>().xPos == x && go.GetComponent<Square>().yPos == y)
                    {
                        Square1 = go;
                    }
                }
                foreach (GameObject go in Squares)
                {
                    if (go.GetComponent<Square>().xPos + 1 == x && go.GetComponent<Square>().yPos == y)
                    {
                        Square2 = go;
                        break;
                    }
                }
            }
        }
    }

    //Not needed for DEMO
    private void SendMove()
    {
        if (client != null && (PlayingOnline && isMyTurn) && MouseTarget != null)// && TurnCount < TurnCounter)
        {
            int x = MouseTarget.GetComponent<Line>().xPos;
            int y = MouseTarget.GetComponent<Line>().yPos;
            bool Horizontal = MouseTarget.GetComponent<Line>().isHorizontal;

            //Sending the pre-checked line co-ords online
            string msg = "CMOV|";
            msg += x.ToString() + "|";
            msg += y.ToString() + "|";
            msg += Horizontal.GetHashCode().ToString() + "|";
            msg += TurnCounter.ToString();

            client.Send(msg);
            //TurnCount = TurnCounter;
            //TurnCount++;
            msg = "";
        }
    }

    private void SquareCheck()
    {
        //Check squares flagged by SelectedLineCheck() for 4 toggled lines surrounding them, if a square is toggled current player gets another go and +1 point
        //if all the squares are taken, end the game
        bool extraGo = false;
        if (Square1 != null)
        {
            Square1.GetComponent<Square>().adjacentLinesTaken += 1;

            if (Square1.GetComponent<Square>().adjacentLinesTaken == 4)
            {
                Square1.GetComponent<Square>().ownedSquare = true;
                Square1.GetComponent<Square>().whoOwns = currentPlayer;
                Square1.GetComponent<MeshRenderer>().material = PlayerColours[currentPlayer - 1];
                ScoreTally();
                extraGo = true;
                TurnCounter += 1;
                //if (Square2 == null)
                //{
                //    currentPlayer -= 1;
                //}
            }

            Square1 = null;
        }

        if (Square2 != null)
        {
            Square2.GetComponent<Square>().adjacentLinesTaken += 1;
            if (Square2.GetComponent<Square>().adjacentLinesTaken == 4)
            {
                Square2.GetComponent<Square>().ownedSquare = true;
                Square2.GetComponent<Square>().whoOwns = currentPlayer;
                Square2.GetComponent<MeshRenderer>().material = PlayerColours[currentPlayer - 1];
                ScoreTally();
                //currentPlayer -= 1;
                extraGo = true;
                TurnCounter += 1;
            }

            Square2 = null;

        }

        if (!extraGo)
        {
            currentPlayer += 1;
            TurnCounter += 1;
            if (currentPlayer > 4)
            {
                currentPlayer = 1;
            }
        }
        SendMove();
        MouseTarget = null;
    }

    private void ScoreTally()
    {
        int CurrentP1Count = 0;
        int CurrentP2Count = 0;
        int CurrentP3Count = 0;
        int CurrentP4Count = 0;
        int squaresLeft = 0;

        foreach (GameObject go in Squares)
        {
            if (go.GetComponent<Square>().whoOwns == 1)
            {
                CurrentP1Count += 1;
                if (CurrentP1Count > P1Score)
                {
                    P1Score = CurrentP1Count;
                }
            }
            else if (go.GetComponent<Square>().whoOwns == 2)
            {
                CurrentP2Count += 1;
                if (CurrentP2Count > P2Score)
                {
                    P2Score = CurrentP2Count;
                }
            }
            else if (go.GetComponent<Square>().whoOwns == 3)
            {
                CurrentP3Count += 1;
                if (CurrentP3Count > P3Score)
                {
                    P3Score = CurrentP3Count;
                }
            }
            else if (go.GetComponent<Square>().whoOwns == 4)
            {
                CurrentP4Count += 1;
                if (CurrentP4Count > P4Score)
                {
                    P4Score = CurrentP4Count;
                }
            }
            else if (go.GetComponent<Square>().whoOwns == 0)
            {
                squaresLeft += 1;
            }
        }

        if (squaresLeft == 0)
        {
            //game is over
            if (Endgame)
            {
                return;
            }
            else
            {
                EndGame();
            }
        }

    }

    private void EndGame()
    {
        //reset all values
        //declare winner/scores
        //ask if play again or go to menu
        Endgame = true;
        Debug.Log("Game Over");
        Debug.Log("P1: " + P1Score + " red");
        Debug.Log("P2: " + P2Score + " green");
        Debug.Log("P3: " + P3Score + " yellow");
        Debug.Log("P4: " + P4Score + " blue");

        if (P1Score > P2Score && P1Score > P3Score && P1Score > P4Score)
        {
            Debug.Log("Player 1, red Wins!");
            Winner = 1;
            P1Wins += 1;
        }
        else if (P2Score > P1Score && P2Score > P3Score && P2Score > P4Score)
        {
            Debug.Log("Player 2, green Wins!");
            Winner = 2;
            P2Wins += 1;
        }
        else if (P3Score > P1Score && P3Score > P2Score && P3Score > P4Score)
        {
            Debug.Log("Player 3, yellow Wins!");
            Winner = 3;
            P3Wins += 1;
        }
        else if (P4Score > P1Score && P4Score > P2Score && P4Score > P3Score)
        {
            Debug.Log("Player 4, blue Wins!");
            Winner = 4;
            P4Wins += 1;
        }
        else
        {
            Debug.Log("Tie game!");
            Winner = 0;
        }
        DemoWaitBeforeReset();
    }

    private void DemoWaitBeforeReset()
    {
        StopAllCoroutines();
        StartCoroutine(Waiter2());
        IEnumerator Waiter2()
        {
            yield return new WaitForSecondsRealtime(5f);
            DeleteBoard();
            ResetCheck = true;
            Reset();
        }
    }

    private void DeleteBoard()
    {
        foreach(GameObject go in Lines)
        {
            Destroy(go);
        }
        foreach (GameObject go in Squares)
        {
            Destroy(go);
        }
        Squares.Clear();
        Lines.Clear();
    }

    private void Reset()
    {
        if (ResetCheck == false)
        {
            return;
        }
        else
        {
            MouseTarget = null;
            foreach (GameObject go in Lines)
            {
                go.GetComponent<Line>().isClicked = false;
                go.GetComponent<MeshRenderer>().material = defaultMat;
            }

            foreach (GameObject go in Squares)
            {
                go.GetComponent<Square>().adjacentLinesTaken = 0;
                go.GetComponent<Square>().ownedSquare = false;
                go.GetComponent<Square>().whoOwns = 0;
                go.GetComponent<MeshRenderer>().material = defaultMat;
            }
            P1Score = 0;
            P2Score = 0;
            P3Score = 0;
            P4Score = 0;
            currentPlayer = 1;
            if (PlayingOnline && SwitchFirstPlayer)
            {
                currentPlayer += 1;
                SwitchFirstPlayer = false;
                if (currentPlayer > 2) { currentPlayer = 1; }
            }
            else if (PlayingOnline && !SwitchFirstPlayer)
            {
                SwitchFirstPlayer = true;
            }
            for (int P = 0; P < 4; P++)
            {
                PlayerSlot[P] = "";
            }
            DemoSetPlayers();
            DemoBoardSizeGen();
            AiEnableNew();
            DemoBoard.GetComponent<Transform>().SetPositionAndRotation(BoardDefaultLocation, BoardDefaultRotation);
            SpawnHorizontals();
            SpawnVerticals();
            SpawnSquares();
            SetCamera();
            JauntyBoardOffset();
            Winner = 0;
            TurnCounter = 0;
            Endgame = false;
            ResetCheck = false;
        }
    }

    //Not needed for DEMO
    private void AiEnable()
    {
        if (noOfAi == 4)
        {
            AIPlayer1.SetActive(true);
            AIPlayer2.SetActive(true);
            AIPlayer3.SetActive(true);
            AIPlayer4.SetActive(true);
            isP1Ai = true;
            isP2Ai = true;
            isP3Ai = true;
            isP4Ai = true;

        }
        else if (noOfAi == 3)
        {
            if (noOfPlayers == 4)
            {
                AIPlayer1.SetActive(false);
                AIPlayer2.SetActive(true);
                AIPlayer3.SetActive(true);
                AIPlayer4.SetActive(true);
                isP1Ai = false;
                isP2Ai = true;
                isP3Ai = true;
                isP4Ai = true;
            }
            else if (noOfPlayers == 3)
            {
                AIPlayer1.SetActive(true);
                AIPlayer2.SetActive(true);
                AIPlayer3.SetActive(true);
                AIPlayer4.SetActive(false);
                isP1Ai = true;
                isP2Ai = true;
                isP3Ai = true;
                isP4Ai = false;
            }
        }
        else if (noOfAi == 2)
        {
            if (noOfPlayers == 4)
            {
                AIPlayer1.SetActive(false);
                AIPlayer2.SetActive(false);
                AIPlayer3.SetActive(true);
                AIPlayer4.SetActive(true);
                isP1Ai = false;
                isP2Ai = false;
                isP3Ai = true;
                isP4Ai = true;
            }
            else if (noOfPlayers == 3)
            {
                AIPlayer1.SetActive(false);
                AIPlayer2.SetActive(true);
                AIPlayer3.SetActive(true);
                AIPlayer4.SetActive(false);
                isP1Ai = false;
                isP2Ai = true;
                isP3Ai = true;
                isP4Ai = false;
            }
            else if (noOfPlayers == 2)
            {
                AIPlayer1.SetActive(true);
                AIPlayer2.SetActive(true);
                AIPlayer3.SetActive(false);
                AIPlayer4.SetActive(false);
                isP1Ai = true;
                isP2Ai = true;
                isP3Ai = false;
                isP4Ai = false;
            }

        }
        else if (noOfAi == 1)
        {
            if (noOfPlayers == 4)
            {
                AIPlayer1.SetActive(false);
                AIPlayer2.SetActive(false);
                AIPlayer3.SetActive(false);
                AIPlayer4.SetActive(true);
                isP1Ai = false;
                isP2Ai = false;
                isP3Ai = false;
                isP4Ai = true;
            }
            else if (noOfPlayers == 3)
            {
                AIPlayer1.SetActive(false);
                AIPlayer2.SetActive(false);
                AIPlayer3.SetActive(true);
                AIPlayer4.SetActive(false);
                isP1Ai = false;
                isP2Ai = false;
                isP3Ai = true;
                isP4Ai = false;
            }
            else if (noOfPlayers == 2)
            {
                AIPlayer1.SetActive(false);
                AIPlayer2.SetActive(true);
                AIPlayer3.SetActive(false);
                AIPlayer4.SetActive(false);
                isP1Ai = false;
                isP2Ai = true;
                isP3Ai = false;
                isP4Ai = false;
            }
            else if (noOfPlayers == 1)
            {
                AIPlayer1.SetActive(true);
                AIPlayer2.SetActive(false);
                AIPlayer3.SetActive(false);
                AIPlayer4.SetActive(false);
                isP1Ai = true;
                isP2Ai = false;
                isP3Ai = false;
                isP4Ai = false;
            }
        }
        else if (noOfAi == 0)
        {
            AIPlayer1.SetActive(false);
            AIPlayer2.SetActive(false);
            AIPlayer3.SetActive(false);
            AIPlayer4.SetActive(false);
            isP1Ai = false;
            isP2Ai = false;
            isP3Ai = false;
            isP4Ai = false;
        }
    }

    private void AiEnableNew()
    {
        //string P1Slot = PersistGameObject.GetComponent<PersistObject>().PlayerSlot1;
        //string P2Slot = PersistGameObject.GetComponent<PersistObject>().PlayerSlot2;
        //string P3Slot = PersistGameObject.GetComponent<PersistObject>().PlayerSlot3;
        //string P4Slot = PersistGameObject.GetComponent<PersistObject>().PlayerSlot4;

        if (PlayerSlot[0] == "easyai" || PlayerSlot[0] == "medai" || PlayerSlot[0] == "hardai")
        {
            AIPlayer1.SetActive(true);
            isP1Ai = true;
        }
        else
        {
            AIPlayer1.SetActive(false);
            isP1Ai = false;
        }
        if (PlayerSlot[1] == "easyai" || PlayerSlot[1] == "medai" || PlayerSlot[1] == "hardai")
        {
            AIPlayer2.SetActive(true);
            isP2Ai = true;
        }
        else
        {
            AIPlayer2.SetActive(false);
            isP2Ai = false;
        }
        if (PlayerSlot[2] == "easyai" || PlayerSlot[2] == "medai" || PlayerSlot[2] == "hardai")
        {
            AIPlayer3.SetActive(true);
            isP3Ai = true;
        }
        else
        {
            AIPlayer3.SetActive(false);
            isP3Ai = false;
        }
        if (PlayerSlot[3] == "easyai" || PlayerSlot[3] == "medai" || PlayerSlot[3] == "hardai")
        {
            AIPlayer4.SetActive(true);
            isP4Ai = true;
        }
        else
        {
            AIPlayer4.SetActive(false);
            isP4Ai = false;
        }
    }

    private void EmptySlotHandler()
    {
        //string P1Slot = PersistGameObject.GetComponent<PersistObject>().PlayerSlot1;
        //string P2Slot = PersistGameObject.GetComponent<PersistObject>().PlayerSlot2;
        //string P3Slot = PersistGameObject.GetComponent<PersistObject>().PlayerSlot3;
        //string P4Slot = PersistGameObject.GetComponent<PersistObject>().PlayerSlot4;

        if (PlayerSlot[0] == "empty")
        {
            isP1empty = true;
        }
        else
        {
            isP1empty = false;
        }
        if (PlayerSlot[1] == "empty")
        {
            isP2empty = true;
        }
        else
        {
            isP2empty = false;
        }
        if (PlayerSlot[2] == "empty")
        {
            isP3empty = true;
        }
        else
        {
            isP3empty = false;
        }
        if (PlayerSlot[3] == "empty")
        {
            isP4empty = true;
        }
        else
        {
            isP4empty = false;
        }
    }

    private void EmptyTurnCheck()
    {
        if (currentPlayer == 1)
        {
            if (isP1empty == true)
            {
                IsEmptyTurn = true;
                //Debug.Log("Empty Turn");
            }
            else
            {
                IsEmptyTurn = false;
            }
        }
        else if (currentPlayer == 2)
        {
            if (isP2empty == true)
            {
                IsEmptyTurn = true;
                //Debug.Log("Empty Turn");
            }
            else
            {
                IsEmptyTurn = false;
            }
        }
        else if (currentPlayer == 3)
        {
            if (isP3empty == true)
            {
                IsEmptyTurn = true;
                //Debug.Log("Empty Turn");
            }
            else
            {
                IsEmptyTurn = false;
            }
        }
        else if (currentPlayer == 4)
        {
            if (isP4empty == true)
            {
                IsEmptyTurn = true;
                //Debug.Log("Empty Turn");
            }
            else
            {
                IsEmptyTurn = false;
            }
        }
    }

    private void AiTurnCheck()
    {
        if (currentPlayer == 1)
        {
            if (isP1Ai == true)
            {
                IsAiTurn = true;
            }
            else
            {
                IsAiTurn = false;
            }
        }
        else if (currentPlayer == 2)
        {
            if (isP2Ai == true)
            {
                IsAiTurn = true;
            }
            else
            {
                IsAiTurn = false;
            }
        }
        else if (currentPlayer == 3)
        {
            if (isP3Ai == true)
            {
                IsAiTurn = true;
            }
            else
            {
                IsAiTurn = false;
            }
        }
        else if (currentPlayer == 4)
        {
            if (isP4Ai == true)
            {
                IsAiTurn = true;
            }
            else
            {
                IsAiTurn = false;
            }
        }
    }

    //Not needed for DEMO
    private void OnlineTurnCheck()
    {
        if ((currentPlayer - 1) == (!amIHost).GetHashCode())
        {
            isMyTurn = true;
        }
        else
        {
            isMyTurn = false;
        }
    }
}
