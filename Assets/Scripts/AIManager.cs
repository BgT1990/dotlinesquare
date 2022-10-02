using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles Ai players
public class AIManager : MonoBehaviour
{

    public int CurrentPlayer;
    public int MyPlayerNumber;
    public bool isThisAiEasy = false;
    public bool isThisAiMedium = false;
    public bool isThisAiHard = false;
    public List<GameObject> UnclaimedSquares3Lines;
    public List<GameObject> UnclaimedSquares2Lines;
    public List<GameObject> UnclaimedSquares1Line;
    public List<GameObject> UnclaimedSquaresNoLines;
    public List<GameObject> LinesAroundChosenSquare;
    public List<GameObject> LinesArounddangerSquare;
    public List<GameObject> LinesToRemove;
    public GameObject ChosenSquare;
    public GameObject ChosenLine;
    public int ChosenLineX, ChosenLineY;
    public GameObject board;
    public bool running = false;
    public bool TurnTaken = false;
    public int TurnCounter;
    private int LocalTurnCounter;
    public bool DangerSkip = false;

    private void Start()
    {
        if (MyPlayerNumber == 1)
        {
            if (board.GetComponent<BoardManager>().PersistGameObject.GetComponent<PersistObject>().PlayerSlot1 == "easyai")
            {
                isThisAiEasy = true;
                isThisAiMedium = false;
                isThisAiHard = false;
            }
            else if (board.GetComponent<BoardManager>().PersistGameObject.GetComponent<PersistObject>().PlayerSlot1 == "medai")
            {
                isThisAiEasy = false;
                isThisAiMedium = true;
                isThisAiHard = false;
            }
            else if (board.GetComponent<BoardManager>().PersistGameObject.GetComponent<PersistObject>().PlayerSlot1 == "hardai")
            {
                isThisAiEasy = false;
                isThisAiMedium = false;
                isThisAiHard = true;
            }
        }
        else if (MyPlayerNumber == 2)
        {
            if (board.GetComponent<BoardManager>().PersistGameObject.GetComponent<PersistObject>().PlayerSlot2 == "easyai")
            {
                isThisAiEasy = true;
                isThisAiMedium = false;
                isThisAiHard = false;
            }
            else if (board.GetComponent<BoardManager>().PersistGameObject.GetComponent<PersistObject>().PlayerSlot2 == "medai")
            {
                isThisAiEasy = false;
                isThisAiMedium = true;
                isThisAiHard = false;
            }
            else if (board.GetComponent<BoardManager>().PersistGameObject.GetComponent<PersistObject>().PlayerSlot2 == "hardai")
            {
                isThisAiEasy = false;
                isThisAiMedium = false;
                isThisAiHard = true;
            }
        }
        else if (MyPlayerNumber == 3)
        {
            if (board.GetComponent<BoardManager>().PersistGameObject.GetComponent<PersistObject>().PlayerSlot3 == "easyai")
            {
                isThisAiEasy = true;
                isThisAiMedium = false;
                isThisAiHard = false;
            }
            else if (board.GetComponent<BoardManager>().PersistGameObject.GetComponent<PersistObject>().PlayerSlot3 == "medai")
            {
                isThisAiEasy = false;
                isThisAiMedium = true;
                isThisAiHard = false;
            }
            else if (board.GetComponent<BoardManager>().PersistGameObject.GetComponent<PersistObject>().PlayerSlot3 == "hardai")
            {
                isThisAiEasy = false;
                isThisAiMedium = false;
                isThisAiHard = true;
            }
        }
        else if (MyPlayerNumber == 4)
        {
            if (board.GetComponent<BoardManager>().PersistGameObject.GetComponent<PersistObject>().PlayerSlot4 == "easyai")
            {
                isThisAiEasy = true;
                isThisAiMedium = false;
                isThisAiHard = false;
            }
            else if (board.GetComponent<BoardManager>().PersistGameObject.GetComponent<PersistObject>().PlayerSlot4 == "medai")
            {
                isThisAiEasy = false;
                isThisAiMedium = true;
                isThisAiHard = false;
            }
            else if (board.GetComponent<BoardManager>().PersistGameObject.GetComponent<PersistObject>().PlayerSlot4 == "hardai")
            {
                isThisAiEasy = false;
                isThisAiMedium = false;
                isThisAiHard = true;
            }
        }
    }

    void Update()
    {
        CurrentPlayer = board.GetComponent<BoardManager>().currentPlayer;
        TurnCounter = board.GetComponent<BoardManager>().TurnCounter;


        if (CurrentPlayer == MyPlayerNumber && running == false && TurnTaken == false && board.GetComponent<BoardManager>().Endgame == false)
        {
            //board.GetComponent<BoardManager>().IsAiTurn = true;
            SquareCheck();
            //SquareChoose(); //moved into the end of squarecheck
            //LineChoose(); // moved into the end of squarechoose
            
            //Delay(); //moved into the end of linechoose

            //TakeTurn(); //Moved to inside Delay();
        }
        else if(running == false || CurrentPlayer != MyPlayerNumber)
        {
            //not really a great time do do things...
        }
    }

    private void SquareCheck()
    {
        UnclaimedSquares3Lines.Clear();
        UnclaimedSquares2Lines.Clear();
        UnclaimedSquares1Line.Clear();
        UnclaimedSquaresNoLines.Clear();

        foreach (GameObject square in board.GetComponent<BoardManager>().Squares)
        {
            if(square.GetComponent<Square>().adjacentLinesTaken == 0)
            {
                UnclaimedSquaresNoLines.Add(square);
            }
            else if (square.GetComponent<Square>().adjacentLinesTaken == 1)
            {
                UnclaimedSquares1Line.Add(square);
            }
            else if (square.GetComponent<Square>().adjacentLinesTaken == 2)
            {
                UnclaimedSquares2Lines.Add(square);
            }
            else if (square.GetComponent<Square>().adjacentLinesTaken == 3)
            {
                UnclaimedSquares3Lines.Add(square);
            }
            else
            {
                //we dont care about these
            }
        }
        SquareChoose();
    }

    private void SquareChoose()
    {
        ChosenSquare = null;
        if (isThisAiEasy == true)//board.GetComponent<BoardManager>().IsAiHard == false) //Standard level ai, has a chance to miss a claimable square and a chance to give them away
        {
            if (UnclaimedSquares3Lines.Count != 0 && Random.value <= 0.90f)
            {
                ChosenSquare = UnclaimedSquares3Lines[Random.Range(0, UnclaimedSquares3Lines.Count)];
            }
            else if ((UnclaimedSquares1Line.Count != 0 || UnclaimedSquaresNoLines.Count != 0) && Random.value <= 0.98f)
            {
                if (UnclaimedSquares1Line.Count != 0 && Random.value <= 0.5f)
                {
                    ChosenSquare = UnclaimedSquares1Line[Random.Range(0, UnclaimedSquares1Line.Count)];
                }
                else if (UnclaimedSquaresNoLines.Count != 0)
                {
                    ChosenSquare = UnclaimedSquaresNoLines[Random.Range(0, UnclaimedSquaresNoLines.Count)];
                }
            }
            else if (UnclaimedSquares2Lines.Count != 0 && Random.value <= 0.7f)
            {
                ChosenSquare = UnclaimedSquares2Lines[Random.Range(0, UnclaimedSquares2Lines.Count)];
            }
        }
        else if (isThisAiMedium == true) //this one never misses a claimable square, but has a chance to give one away
        {
            if (UnclaimedSquares3Lines.Count != 0)  //First looks for squares that are 1 line of capture
            {
                ChosenSquare = UnclaimedSquares3Lines[Random.Range(0, UnclaimedSquares3Lines.Count)];
            }
            else if (UnclaimedSquaresNoLines.Count != 0) //next looks for completely free squares
            {
                ChosenSquare = UnclaimedSquaresNoLines[Random.Range(0, UnclaimedSquaresNoLines.Count)];
            }
            else if (UnclaimedSquares1Line.Count != 0) //then looks for squares with 1 line already
            {
                ChosenSquare = UnclaimedSquares1Line[Random.Range(0, UnclaimedSquares1Line.Count)];
            }
            else if (UnclaimedSquares2Lines.Count != 0) //Lastly looks for squares that are 1 line off 3 lines, as to not give away squares
            {
                ChosenSquare = UnclaimedSquares2Lines[Random.Range(0, UnclaimedSquares2Lines.Count)];
            }
        }
        else if (isThisAiHard == true) //this one never misses a claimable square or gives one away until there are no other options
        {
            LinesArounddangerSquare.Clear();

            if (UnclaimedSquares3Lines.Count != 0)  //First looks for squares that are 1 line off capture
            {
                ChosenSquare = UnclaimedSquares3Lines[Random.Range(0, UnclaimedSquares3Lines.Count)];
                DangerSkip = true;
            }
            else
            {
                if (UnclaimedSquares2Lines.Count != 0) //Gathering the lines in dangerous 2 lines taken squares
                {
                    foreach (GameObject dangersquare in UnclaimedSquares2Lines)
                    {
                        int dangerSquareX = dangersquare.GetComponent<Square>().xPos;
                         int dangerSquareY = dangersquare.GetComponent<Square>().yPos;

                        //Horizontal lines
                        foreach (GameObject line in board.GetComponent<BoardManager>().Lines)
                        {
                            if ((line.GetComponent<Line>().xPos == dangerSquareX) && (line.GetComponent<Line>().isClicked == false) && (line.GetComponent<Line>().isHorizontal == true))
                            {
                                if (line.GetComponent<Line>().yPos == dangerSquareY)
                                {
                                    LinesArounddangerSquare.Add(line);
                                }
                                else if (line.GetComponent<Line>().yPos == dangerSquareY + 1)
                                {
                                    LinesArounddangerSquare.Add(line);
                                }
                            }
                        }

                        //Vertical lines
                        foreach (GameObject line in board.GetComponent<BoardManager>().Lines)
                        {
                            if ((line.GetComponent<Line>().yPos == dangerSquareY) && (line.GetComponent<Line>().isClicked == false) && (line.GetComponent<Line>().isHorizontal == false))
                            {
                                if (line.GetComponent<Line>().xPos == dangerSquareX)
                                {
                                    LinesArounddangerSquare.Add(line);
                                }
                                else if (line.GetComponent<Line>().xPos == dangerSquareX + 1)
                                {
                                    LinesArounddangerSquare.Add(line);
                                }
                            }
                        }
                    }
                }

                if (UnclaimedSquaresNoLines.Count != 0) //next looks for completely free squares
                {
                    ChosenSquare = UnclaimedSquaresNoLines[Random.Range(0, UnclaimedSquaresNoLines.Count)];
                }
                else if (UnclaimedSquares1Line.Count != 0) //then looks for squares with 1 line already
                {
                    ChosenSquare = UnclaimedSquares1Line[Random.Range(0, UnclaimedSquares1Line.Count)];
                }
                else if (UnclaimedSquares2Lines.Count != 0) //Lastly looks for squares that are 1 line off 3 lines, as to not give away squares
                {
                    ChosenSquare = UnclaimedSquares2Lines[Random.Range(0, UnclaimedSquares2Lines.Count)];
                }
            }
        }

        //Impossible Ai that will know how many squares each claimable square will give away and choose the one least profitable for the next player
        LineChoose();
    }

    private void LineChoose()
    {
        ChosenLineX = -1;
        ChosenLineY = -1;
        ChosenLine = null;
        LinesAroundChosenSquare.Clear();
        int ChosenSquareX = -1;
        int ChosenSquareY = -1;
        if (ChosenSquare != null)
        {
            if (isThisAiHard == false)
            {
                ChosenSquareX = ChosenSquare.GetComponent<Square>().xPos;
                ChosenSquareY = ChosenSquare.GetComponent<Square>().yPos;

                //Horizontal lines
                foreach (GameObject line in board.GetComponent<BoardManager>().Lines)
                {
                    if ((line.GetComponent<Line>().xPos == ChosenSquareX) && (line.GetComponent<Line>().isClicked == false) && (line.GetComponent<Line>().isHorizontal == true))
                    {
                        if (line.GetComponent<Line>().yPos == ChosenSquareY)
                        {
                            LinesAroundChosenSquare.Add(line);
                        }
                        else if (line.GetComponent<Line>().yPos == ChosenSquareY + 1)
                        {
                            LinesAroundChosenSquare.Add(line);
                        }
                    }
                }

                //Vertical lines
                foreach (GameObject line in board.GetComponent<BoardManager>().Lines)
                {
                    if ((line.GetComponent<Line>().yPos == ChosenSquareY) && (line.GetComponent<Line>().isClicked == false) && (line.GetComponent<Line>().isHorizontal == false))
                    {
                        if (line.GetComponent<Line>().xPos == ChosenSquareX)
                        {
                            LinesAroundChosenSquare.Add(line);
                        }
                        else if (line.GetComponent<Line>().xPos == ChosenSquareX + 1)
                        {
                            LinesAroundChosenSquare.Add(line);
                        }
                    }
                }

                if (LinesAroundChosenSquare.Count != 0)
                {
                    ChosenLine = LinesAroundChosenSquare[Random.Range(0, LinesAroundChosenSquare.Count)];
                    ChosenLineX = ChosenLine.GetComponent<Line>().xPos;
                    ChosenLineY = ChosenLine.GetComponent<Line>().yPos;
                }
            }
            else if (isThisAiHard == true)
            {
                ChosenSquareX = ChosenSquare.GetComponent<Square>().xPos;
                ChosenSquareY = ChosenSquare.GetComponent<Square>().yPos;
                LinesToRemove.Clear();


                //Horizontal lines
                foreach (GameObject line in board.GetComponent<BoardManager>().Lines)
                {
                    if ((line.GetComponent<Line>().xPos == ChosenSquareX) && (line.GetComponent<Line>().isClicked == false) && (line.GetComponent<Line>().isHorizontal == true))
                    {
                        if (line.GetComponent<Line>().yPos == ChosenSquareY)
                        {
                            LinesAroundChosenSquare.Add(line);
                        }
                        else if (line.GetComponent<Line>().yPos == ChosenSquareY + 1)
                        {
                            LinesAroundChosenSquare.Add(line);
                        }
                    }
                }

                //Vertical lines
                foreach (GameObject line in board.GetComponent<BoardManager>().Lines)
                {
                    if ((line.GetComponent<Line>().yPos == ChosenSquareY) && (line.GetComponent<Line>().isClicked == false) && (line.GetComponent<Line>().isHorizontal == false))
                    {
                        if (line.GetComponent<Line>().xPos == ChosenSquareX)
                        {
                            LinesAroundChosenSquare.Add(line);
                        }
                        else if (line.GetComponent<Line>().xPos == ChosenSquareX + 1)
                        {
                            LinesAroundChosenSquare.Add(line);
                        }
                    }
                }

                if (LinesAroundChosenSquare.Count != 0)
                {
                    if (DangerSkip == true)
                    {
                        ChosenLine = LinesAroundChosenSquare[Random.Range(0, LinesAroundChosenSquare.Count)];
                    }
                    else if (DangerSkip == false)
                    {
                        if (LinesArounddangerSquare.Count != 0)
                        {
                            foreach (GameObject potentialline in LinesAroundChosenSquare)
                            {
                                foreach (GameObject dangerline in LinesArounddangerSquare)
                                {
                                    if (potentialline == dangerline)
                                    {
                                        LinesToRemove.Add(potentialline);
                                    }
                                }
                            }
                            foreach (GameObject badLine in LinesToRemove)
                            {
                                LinesAroundChosenSquare.Remove(badLine); //this needs some work
                            }
                        }
                        if (LinesAroundChosenSquare.Count != 0)
                        {
                            ChosenLine = LinesAroundChosenSquare[Random.Range(0, LinesAroundChosenSquare.Count)];
                        }
                        else if (LinesAroundChosenSquare.Count == 0)
                        {
                            ChosenLine = LinesArounddangerSquare[Random.Range(0, LinesArounddangerSquare.Count)];
                        }
                    }
                    ChosenLineX = ChosenLine.GetComponent<Line>().xPos;
                    ChosenLineY = ChosenLine.GetComponent<Line>().yPos;
                }
            }
        }
        Delay();
    }

    private void TurnCheck()
    {
        if(TurnCounter < LocalTurnCounter)
        {
            TurnTaken = true;
        }
        else if (TurnCounter >= LocalTurnCounter)
        {
            TurnTaken = false;
        }
    }

    private void TakeTurn()
    {
        if (ChosenLine != null)
        {
            if (ChosenLine.GetComponent<Line>().isClicked == true)
            {
                ChosenLine = null;

            }
            else
            {
                board.GetComponent<BoardManager>().MouseTarget = ChosenLine;
                //ChosenLine.GetComponent<Line>().isClicked = true;
                //ChosenLine.GetComponent<MeshRenderer>().material = board.GetComponent<BoardManager>().selectedMat;
                //board.GetComponent<BoardManager>().IsAiTurn = false;
                //board.GetComponent<BoardManager>().currentPlayer += 1;

                LocalTurnCounter += 1;
                //board.GetComponent<BoardManager>().IsAiTurn = false;
            }

        }
        ChosenSquare = null;
        ChosenLine = null;
        DangerSkip = false;
        LinesAroundChosenSquare.Clear();
        UnclaimedSquares3Lines.Clear();
        UnclaimedSquares2Lines.Clear();
        UnclaimedSquares1Line.Clear();
        UnclaimedSquaresNoLines.Clear();

    }

    private void Delay()
    {
        if (isThisAiEasy == true)//board.GetComponent<BoardManager>().IsAiHard == false)
        {
            StopAllCoroutines();
            StartCoroutine(Waiter());
            IEnumerator Waiter()
            {
                running = true;
                yield return new WaitForSecondsRealtime(Random.Range(0.2f, 1f));
                TakeTurn();
                running = false;
            }
        }
        else if (isThisAiMedium == true)
        {
            StopAllCoroutines();
            StartCoroutine(Waiter());
            IEnumerator Waiter()
            {
                running = true;
                yield return new WaitForSecondsRealtime(Random.Range(0.1f, 0.7f));
                TakeTurn();
                running = false;
            }
        }
        else if (isThisAiHard == true)
        {
            TakeTurn();
            //Hard Ai Player does not wait...
        }
    }

}
