using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManagerDraught : PieceManager
{
    private BoardDraught board;

    // Start is called before the first frame update
    public PieceManagerDraught (PieceManager pieceManager)
    {
        this.board = pieceManager.getBoardDraught();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
