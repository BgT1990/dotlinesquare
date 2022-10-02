using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextureChanges : MonoBehaviour
{
    public int BGcount;
    public bool altColours = false;
    public GameObject Board;
    public GameObject BackgroundPlane;
    public List<Material> AltPlayerColours;
    public List<Material> PlayerColours;
    public List<Material> AltBackgrounds;

    private void Start()
    {
        BGcount = 11;
    }

    public void ChangeBackground()
    {
        BGcount++;
        if (BGcount > 11) { BGcount = 0; }
        BackgroundPlane.GetComponent<MeshRenderer>().material = AltBackgrounds[BGcount];

    }

    public void ChangePlayerColours()
    {

        if (!altColours)
        {
            Board.GetComponent<BoardManager>().PlayerColours[0] = AltPlayerColours[0];
            Board.GetComponent<BoardManager>().PlayerColours[1] = AltPlayerColours[1];
            Board.GetComponent<BoardManager>().PlayerColours[2] = AltPlayerColours[2];
            Board.GetComponent<BoardManager>().PlayerColours[3] = AltPlayerColours[3];
            foreach(GameObject Sqrs in Board.GetComponent<BoardManager>().Squares)
            {
                if (Sqrs.GetComponent<Square>().whoOwns == 0)
                {
                    Sqrs.GetComponent<MeshRenderer>().material = AltPlayerColours[4];
                }
                else if(Sqrs.GetComponent<Square>().whoOwns == 1)
                {
                    Sqrs.GetComponent<MeshRenderer>().material = AltPlayerColours[0];
                    
                }
                else if (Sqrs.GetComponent<Square>().whoOwns == 2)
                {
                    Sqrs.GetComponent<MeshRenderer>().material = AltPlayerColours[1];
                    
                }
                else if (Sqrs.GetComponent<Square>().whoOwns == 3)
                {
                    Sqrs.GetComponent<MeshRenderer>().material = AltPlayerColours[2];
                    
                }
                else if (Sqrs.GetComponent<Square>().whoOwns == 4)
                {
                    Sqrs.GetComponent<MeshRenderer>().material = AltPlayerColours[3];
                }
            }
        }
        else
        {
            Board.GetComponent<BoardManager>().PlayerColours[0] = PlayerColours[0];
            Board.GetComponent<BoardManager>().PlayerColours[1] = PlayerColours[1];
            Board.GetComponent<BoardManager>().PlayerColours[2] = PlayerColours[2];
            Board.GetComponent<BoardManager>().PlayerColours[3] = PlayerColours[3];
            foreach (GameObject Sqrs in Board.GetComponent<BoardManager>().Squares)
            {
                if (Sqrs.GetComponent<Square>().whoOwns == 0)
                {
                    Sqrs.GetComponent<MeshRenderer>().material = AltPlayerColours[4];
                }
                else if (Sqrs.GetComponent<Square>().whoOwns == 1)
                {
                    Sqrs.GetComponent<MeshRenderer>().material = PlayerColours[0];

                }
                else if (Sqrs.GetComponent<Square>().whoOwns == 2)
                {
                    Sqrs.GetComponent<MeshRenderer>().material = PlayerColours[1];

                }
                else if (Sqrs.GetComponent<Square>().whoOwns == 3)
                {
                    Sqrs.GetComponent<MeshRenderer>().material = PlayerColours[2];

                }
                else if (Sqrs.GetComponent<Square>().whoOwns == 4)
                {
                    Sqrs.GetComponent<MeshRenderer>().material = PlayerColours[3];
                }
            }
        }
        altColours = !altColours;
    }
}
