using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour

    //This will be applied to the square prefab and check if the square has 4 claimed lines around it and who owns it
{

    public int xPos;
    public int yPos;
    public int adjacentLinesTaken = 0;
    public bool ownedSquare = false;
    public int whoOwns = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
