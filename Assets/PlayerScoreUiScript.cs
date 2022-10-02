using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreUiScript : MonoBehaviour
{
    public GameObject ThisObject;
    public Text Name;
    public Text PlayerType;
    public Text Score;
    public GameObject ColourPanel;
    public GameObject BoxBackgroundPanel;
    public string Nametext;
    public string Typetext;
    public string Scoretext;
    public Material PlayerColour;
    //public bool isMyTurn;
    public Color DefaultColour;
    public Color MyTurnColour;
    public int MyPlayerNumber; //1 2 3 or 4
    private GameObject Board;

    private void Start()
    {
       Board = GameObject.Find("Board");
    }

    void Update()
    {
        Score.text = Scoretext;

        if (Board.GetComponent<BoardManager>().currentPlayer == MyPlayerNumber)
        {
            BoxBackgroundPanel.GetComponent<Image>().color = MyTurnColour;
        }
        else
        {
            BoxBackgroundPanel.GetComponent<Image>().color = DefaultColour;
        }
    }

    //Called from HudElements.DisableUnusedPlayerDisplaysNew()
    public void FillInInfo()
    {
        Nametext = Camera.main.GetComponent<HudElements>().PlayerNames[MyPlayerNumber-1];
        Name.text = Nametext;

        PersistObject p = GameObject.Find("PersistGameObject").GetComponent<PersistObject>();
        Typetext = p.Slots[MyPlayerNumber-1];
        if (Typetext == "human") { Typetext = "Human"; }
        else if (Typetext == "easyai") { Typetext = "Easy Ai"; }
        else if (Typetext == "medai") { Typetext = "Normal Ai"; }
        else if (Typetext == "hardai") { Typetext = "Hard Ai"; }
        
        PlayerType.text = Typetext;

        PlayerColour = GameObject.Find("Board").GetComponent<BoardManager>().PlayerColours[MyPlayerNumber-1];
        ColourPanel.GetComponent<Image>().material = PlayerColour;

    }
}
