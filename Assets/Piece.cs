using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Piece : EventTrigger
{

    [HideInInspector]
    public Color mColor = Color.clear;
    public Sprite n_Sprite;
    public Sprite o_Sprite;

    protected CellDraughtV mOriginalCell = null;
    protected CellDraughtV mCurrentCell = null;
    protected PieceManager mPieceManager;
    protected CellDraughtV mTargetCell = null;
    protected List<Move> mMoves = new List<Move>();
    protected Vector3Int mMovement;
    protected List<CellDraughtV> mHighlightedCells = new List<CellDraughtV>();
    public List<Move> moves = new List<Move>();

    private RectTransform mRectTransform = null;





    public virtual void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager, Vector3Int movement)
    {
        mPieceManager = newPieceManager;
        mColor = newTeamColor;
        mRectTransform = GetComponent<RectTransform>();
        mMovement = movement;
    }

    public virtual Move[] GetMoveArray ()
    {
        return this.mPieceManager.getBoard().GetMoves(this);
    }

    public virtual void GetMovesMan(ref Piece[,] board)
    {
        CheckPathing(true);
    }


    public virtual List<Move> getPossibleActions()
    {
        if (isDead())
            return null;
        else
        {
            CheckPathing(true);
            ClearMoves();
            if (mHighlightedCells.Count > 0)
            {
                for (int i = 0; i < mHighlightedCells.ToArray().Length; i++)
                {
                    mTargetCell = mHighlightedCells.ToArray()[i];
                    FindMoves();
                }
            }
            return moves;
        }
    }


    public virtual void Place(CellDraughtV newCell)
    {


        //Cell
        gameObject.SetActive(true);

        mCurrentCell = newCell;
        mOriginalCell = newCell;
        mCurrentCell.mCurrentPiece = this;

        //Object
        transform.localPosition = mCurrentCell.transform.localPosition;
    }

    public virtual void PlaceByAI(CellDraughtV newCell)
    {
        Color color = mColor;
        if (this.name == null)
            Debug.Log("this.name is null");
        if (mCurrentCell.name == null)
            Debug.Log("mCurrentCell.name is null");
        if (newCell.name == null)
            Debug.Log("newCell.name is null");
        Debug.Log(this.name + " bewegt sich von " + mCurrentCell.name + " nach " + newCell.name);
        mTargetCell = newCell;
        Move();
    }


    public virtual void MakeMove(CellDraughtV newCell)
    {
        Color color = mColor;
        if (this.name == null)
            Debug.Log("this.name is null");
        if (mCurrentCell.name == null)
            Debug.Log("mCurrentCell.name is null");
        if (newCell.name == null)
            Debug.Log("newCell.name is null");
        Debug.Log("DRAUGHT: " + this.name + " bewegt sich von " + mCurrentCell.name + " nach " + newCell.name);
        mTargetCell = newCell;
        MakeMove();
    }


    public virtual void MakeMove()
    {
        if (mTargetCell.mCurrentPiece != null)
        {
            mTargetCell = null;
            return;
        }

        Color color = mColor;

        //check neighbors for Enemys & Allys

        Cell[] attackedCells = checkNeighbor();
        if (attackedCells != null)
        {
            for (int i = 0; i <= attackedCells.Length - 1; i++)
            {
                if (attackedCells[i] != null)
                {
                    attackedCells[i].RemovePiece();
                    mPieceManager.moveAgain = true;
                }
            }
        }

        if (attackedCells != null)
        {
            if (color == Color.white)
                color = Color.black;
            else
                color = Color.white;
        }

        mCurrentCell.mCurrentPiece = null;

        mCurrentCell = mTargetCell;
        mCurrentCell.mCurrentPiece = this;

        mTargetCell = null;
    }

    public virtual void Reset()
    {
        Kill();

        Place(mOriginalCell);

    }

    public virtual void KillDraught()
    {
        mCurrentCell.mCurrentPiece = null;

    }

    public virtual void Kill()
    {
        //Clear Cell
        mCurrentCell.mCurrentPiece = null;

        //Remove Piece
        gameObject.SetActive(false);

    }

    #region movement



    protected virtual void Move()
    {

        Color color = mColor;
        
        //check neighbors for Enemys & Allys

        Cell[] attackedCells = checkNeighbor();
        if (attackedCells != null) {
            for (int i = 0; i <= attackedCells.Length - 1; i++)
            {
                if (attackedCells[i] != null)
                {
                    attackedCells[i].RemovePiece();
                    mPieceManager.moveAgain = true;
                }
            }
        }
        if (attackedCells != null)
        {
            if (color == Color.white)
            {
                color = Color.black;
            }
            else
            {
                color = Color.white;
            }
        }
        Vector2 oldPosition = mCurrentCell.getBoardPosition();
        mPieceManager.getBoard().simpleAllCells[(int)oldPosition.x, (int)oldPosition.y] = "empty";
        mCurrentCell.mCurrentPiece = null;
        Vector2 targetPosition = mTargetCell.getBoardPosition();
        mPieceManager.getBoard().simpleAllCells[(int)oldPosition.x, (int)oldPosition.y] = this.name; // Moving on Boarddraught

        mCurrentCell = mTargetCell;
        mCurrentCell.mCurrentPiece = this;

        transform.position = mCurrentCell.transform.position;
        mTargetCell = null;

        mPieceManager.SwitchSides(color);

    }

    private Cell[] checkNeighbor()
    {
        int currentX = mTargetCell.mBoardPosition.x;
        int currentY = mTargetCell.mBoardPosition.y;

        int targetX = mTargetCell.mBoardPosition.x + 1;
        int targetY = mTargetCell.mBoardPosition.y;

        int allyX = mTargetCell.mBoardPosition.x + 2;
        int allyY = mTargetCell.mBoardPosition.y;

        List<Cell> attackedCells = new List<Cell>();


        int counter = 0;



            if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) == CellState.Enemy
                && mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) == CellState.Friendly)
            {
                counter++;
                attackedCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
            }

            targetX = mTargetCell.mBoardPosition.x - 1;
            targetY = mTargetCell.mBoardPosition.y;

            allyX = mTargetCell.mBoardPosition.x - 2;
            allyY = mTargetCell.mBoardPosition.y;

            if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) == CellState.Enemy
        && mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) == CellState.Friendly)
            {
                counter++;
                attackedCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
            }

            targetX = mTargetCell.mBoardPosition.x;
            targetY = mTargetCell.mBoardPosition.y + 1;

            allyX = mTargetCell.mBoardPosition.x;
            allyY = mTargetCell.mBoardPosition.y + 2;

            if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) == CellState.Enemy
        && mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) == CellState.Friendly)
            {
                counter++;
                attackedCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
            }

            targetX = mTargetCell.mBoardPosition.x;
            targetY = mTargetCell.mBoardPosition.y - 1;

            allyX = mTargetCell.mBoardPosition.x;
            allyY = mTargetCell.mBoardPosition.y - 2;

            if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) == CellState.Enemy
        && mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) == CellState.Friendly)
            {
                counter++;
                attackedCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
            }

        

        //CHECK FOR AI starts HERE

            if (attackedCells.ToArray().Length > 0)
                return attackedCells.ToArray();
            else
                return null;
        
    }

    public virtual void FindMoves()
    {
        int currentX = mTargetCell.mBoardPosition.x;
        int currentY = mTargetCell.mBoardPosition.y;

        int targetX = mTargetCell.mBoardPosition.x + 1;
        int targetY = mTargetCell.mBoardPosition.y;

        int allyX = mTargetCell.mBoardPosition.x + 2;
        int allyY = mTargetCell.mBoardPosition.y;

        List<Cell> attackedCells = new List<Cell>();

        int counter = moves.Count;
        Move m = new Move();
        m.attacked = false;
        m.attacked2 = false;
        m.threaten = false;
        m.hide = false;
        m.removeX = -1;
        m.removeY = -1;
        m.removeX2 = -1;
        m.removeY2 = -1;
        m.currentX = mCurrentCell.mBoardPosition.x;
        m.currentY = mCurrentCell.mBoardPosition.y;
        m.x = mTargetCell.mBoardPosition.x;
        m.y = mTargetCell.mBoardPosition.y;

        if (mColor == Color.black)
            m.player = 2;
        else
            m.player = 1;

        if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) == CellState.Enemy
            && mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) == CellState.Friendly)
        {
            attackedCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
            if (m.attacked == true)
            {
                m.attacked2 = true;
                m.removeX2 = targetX;
                m.removeY2 = targetY;
            }
            else
            {
                m.attacked = true;
                m.removeX = targetX;
                m.removeY = targetY;
            }
        }

        if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) == CellState.Enemy
            && mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) != CellState.Friendly)
        {
            m.threaten = true;
        }

        if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) != CellState.Enemy
&& mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) == CellState.Friendly)
        {
            m.hide = true;
        }

        targetX = mTargetCell.mBoardPosition.x - 1;
        targetY = mTargetCell.mBoardPosition.y;

        allyX = mTargetCell.mBoardPosition.x - 2;
        allyY = mTargetCell.mBoardPosition.y;

        if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) == CellState.Enemy
&& mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) == CellState.Friendly)
        {
            attackedCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
            if (m.attacked == true)
            {
                m.attacked2 = true;
                m.removeX2 = targetX;
                m.removeY2 = targetY;
            }
            else
            {
                m.attacked = true;
                m.removeX = targetX;
                m.removeY = targetY;
            }

        }

        if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) == CellState.Enemy
    && mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) != CellState.Friendly)
        {
            m.threaten = true;
        }

        if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) != CellState.Enemy
&& mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) == CellState.Friendly)
        {
            m.hide = true;
        }

        targetX = mTargetCell.mBoardPosition.x;
        targetY = mTargetCell.mBoardPosition.y + 1;

        allyX = mTargetCell.mBoardPosition.x;
        allyY = mTargetCell.mBoardPosition.y + 2;

        if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) == CellState.Enemy
    && mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) == CellState.Friendly)
        {
            attackedCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
            if (m.attacked == true)
            {
                m.attacked2 = true;
                m.removeX2 = targetX;
                m.removeY2 = targetY;
            }
            else
            {
                m.attacked = true;
                m.removeX = targetX;
                m.removeY = targetY;
            }
        }

        if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) == CellState.Enemy
&& mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) != CellState.Friendly)
        {
            m.threaten = true;
        }

        if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) != CellState.Enemy
&& mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) == CellState.Friendly)
        {
            m.hide = true;
        }

        targetX = mTargetCell.mBoardPosition.x;
        targetY = mTargetCell.mBoardPosition.y - 1;

        allyX = mTargetCell.mBoardPosition.x;
        allyY = mTargetCell.mBoardPosition.y - 2;

        if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) == CellState.Enemy
    && mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) == CellState.Friendly)
        {
            attackedCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
            if (m.attacked == true)
            {
                m.attacked2 = true;
                m.removeX2 = targetX;
                m.removeY2 = targetY;
            }
            else
            {
                m.attacked = true;
                m.removeX = targetX;
                m.removeY = targetY;
            }
        }

            if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) == CellState.Enemy
&& mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) != CellState.Friendly)
        {
            m.threaten = true;
        }

        if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) != CellState.Enemy
&& mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) == CellState.Friendly)
        {
            m.hide = true;
        }
        moves.Add(m);
    }


    public virtual void ClearMoves()
    {
        moves = new List<Move>();
    }

    public virtual void CreateCellPath(int xDirection, int yDirection, int movement, bool isAI)
    {
        int currentX = mCurrentCell.mBoardPosition.x;
        int currentY = mCurrentCell.mBoardPosition.y;

        for (int i = 1; i<= movement; i++)
        {
            currentX += xDirection;
            currentY += yDirection;

            CellState cellState = CellState.None;
            cellState = mCurrentCell.mBoard.ValidateCell(currentX, currentY, this);

            //If enemy, break;
            if (cellState == CellState.Enemy)
            {
                break;
            }

            //get CellState
            if (cellState != CellState.Free)
                break;

            //ADD to Highlighted List
            mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);
        }



    }

    public virtual void CheckPathing(bool isAI)
    {
        //Horizontal
        ClearCells();

        if (isAI)
        {
            ClearMoves();
        }
        CreateCellPath(1, 0, mMovement.x, isAI);
        CreateCellPath(-1, 0, mMovement.x, isAI);

        //Vertical
        CreateCellPath(0, 1, mMovement.y, isAI);
        CreateCellPath(0, -1, mMovement.y, isAI);
    }

    public virtual void CheckPath(Move m)
    {
        int currentX = this.mCurrentCell.mBoardPosition.x;
        int currentY = this.mCurrentCell.mBoardPosition.y;

        if (currentX < m.x)
        {
            for (int i = currentX; i <= m.x; i++)
            {
                mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[i, m.y]);

            }
        }
        else if (currentY < m.y)
        {
            for (int i = currentY; i <= m.y; i++)
            {
                mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[m.x, i]);

            }
        }
        else if (currentX > m.x)
        {
            for (int i = currentX; i >= m.x; i--)
            {
                mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[i, m.y]);

            }
        }
        else if (currentY > m.y)
        {
            for (int i = currentY; i >= m.y; i--)
            {
                mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[m.x, i]);

            }
        }



    }

    public void ShowCells()
    {
        foreach (CellDraughtV cell in mHighlightedCells)
            cell.highlight();

    }

    public virtual void ClearCells()
    {
        foreach (CellDraughtV cell in mHighlightedCells)
            cell.removeHighlight();

        mHighlightedCells.Clear();
    }
    #endregion


    #region Events
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);

        CheckPathing(false);

        ShowCells();
    }


    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        //follow cursor Position
        transform.position += (Vector3)eventData.delta;


        foreach (CellDraughtV cell in mHighlightedCells)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(cell.mRectTransform, Input.mousePosition))
            {
                // if mouse is in a valid cell -> get it
                mTargetCell = cell;
                break;
            }
            //if mouse is not in highlighted cell -> no valid move
            mTargetCell = null;
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        ClearCells();

        if (!mTargetCell)
        {
            transform.position = mCurrentCell.gameObject.transform.position;
            return;
        }
        Move();
    }
    #endregion

    public virtual bool isDead ()
    {
        if (mCurrentCell.mCurrentPiece == null || mCurrentCell.mCurrentPiece != null && mCurrentCell.mCurrentPiece.name != this.name)
            return true;
        else

            return false;
    }


    public virtual CellDraughtV getTargetCell()
    {
        return mTargetCell;
    }

    public virtual CellDraughtV getCurrentCell()
    {
        return mCurrentCell;
    }
}
