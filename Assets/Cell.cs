using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//[System.Serializable]
public class Cell : MonoBehaviour
{

    public Image mOutlineImage;
    public Sprite n_Sprite;
    public Sprite o_Sprite;
    public CellState state;

    [HideInInspector]
    public Vector2Int mBoardPosition = Vector2Int.zero;
    [HideInInspector]
    public Board mBoard = null;
    [HideInInspector]
    public RectTransform mRectTransform = null;
    [HideInInspector]
    public Piece mCurrentPiece = null;


    public virtual void Setup(Vector2Int newBoardPosition, Board newBoard)
    {
        mBoardPosition = newBoardPosition;
        mBoard = newBoard;

        mRectTransform = GetComponent<RectTransform>();
    }


    public virtual void highlight ()
    {
        this.GetComponent<Image>().sprite = n_Sprite;
    }

    public virtual void removeHighlight()
    {
        this.GetComponent<Image>().sprite = o_Sprite;
    }

    public virtual Vector2 getBoardPosition()
    {
        for (int i = 0; i < mBoard.sizeX; i++)
        {
            for (int j = 0; j < mBoard.sizeY; j++)
            {
                if (mBoard.mAllCells[i, j] == this)
                {
                    return new Vector2(i, j);
                }
            }
        }
        return new Vector2(-1 , -1);
    }

    public virtual void RemovePiece()
    {
        if (mCurrentPiece != null)
        {
            Debug.Log(mCurrentPiece.name + " was killed on " + this.name);
            mBoard.simpleAllCells[(int)mBoardPosition.x, (int)mBoardPosition.y] = "empty";

            mCurrentPiece.Kill();
        }
    }

}
