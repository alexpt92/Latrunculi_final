using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellDraught : Cell
{

    public string mCurrentPieceName;

    public void Setup(Vector2Int newBoardPosition, string pieceName)
    {
        mBoardPosition = newBoardPosition;
        mCurrentPieceName = pieceName;

    }

    public CellDraught ()
    {
        mBoard = null;
        mCurrentPiece = null;
        mBoardPosition = Vector2Int.zero; ;
    }

}
