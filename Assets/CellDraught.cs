using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellDraught : Cell
{
    //public PieceDraught mCurrentPiece = null;
   // public BoardDraught mBoard = null;
    public string mCurrentPieceName;
    //public Vector2Int mBoardPosition = Vector2Int.zero;

    // Start is called before the first frame update

    public void Setup(Vector2Int newBoardPosition, string pieceName)
    {
        mBoardPosition = newBoardPosition;
        mCurrentPieceName = pieceName;
        //mBoard = newBoard;

       // mRectTransform = GetComponent<RectTransform>();
    }

    public CellDraught ()
    {
        mBoard = null;
        mCurrentPiece = null;
        mBoardPosition = Vector2Int.zero; ;
    }

}
