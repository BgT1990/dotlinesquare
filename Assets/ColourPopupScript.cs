using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourPopupScript : MonoBehaviour
{
    public List<Material> Colours;
    public GameObject ColourTilePrefab;

    private void OnEnable()
    {
        foreach (Material Colour in Colours)
        {
            //Instantiate(ColourTilePrefab,)
        }
    }

}
