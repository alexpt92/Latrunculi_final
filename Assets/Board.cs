using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;



public enum CellState
{
    None,
    Friendly,
    Enemy,
    Free,
    OutOfBounds
}


[Serializable]
public class Board : MonoBehaviour
{
    List<Move> allMoves = new List<Move>();

    public GameObject mCellPrefab;
    public int sizeX;
    public int sizeY;
    [HideInInspector]
    public CellDraughtV[,] mAllCells;// = new Cell[sizeX, sizeY];
    public string[,] simpleAllCells;
    protected int player;
    //private bool gameOver;//= false;
    protected Vector3Int mMovement;
    Color currentColor = Color.clear;

   /* private void Update()
    {
        Debug.Log("Attack: " + pointAttacked + " Hide: " + pointHide + " Threat: " + pointThreat);
    }*/
    /* public Board(Cell[,] copyBoard, int nextPlayer, int rows, int cols)
     {
         mAllCells = copyBoard;
         player = nextPlayer;
         this.sizeX = rows;
         this.sizeY = cols;
 }*/

    /*public Board MakeMove(Move m)
    {
        //int nextPlayer;
        //Copy Board and make move#
        Move move = (Move)m;
        //NextPlayer();
        int nextPlayer;
        if (player == 1)
            nextPlayer = 2;
        else
            nextPlayer = 1;
        Cell[,] copy = new Cell[sizeX, sizeY];
        Cell[] copy2 = new Cell[sizeX];
        //Array.Copy(mAllCells, 0, copy, 0, mAllCells.Length);
        //copy = (Cell[,])mAllCells.Clone() as Cell[,];
        //Cell[] tmpcopy;
        //Extensions.Clone<Cell>(CustomArray.GetRow(mAllCells, 1));
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                copy[i, j] = Instantiate(mAllCells[i, j]);// copy2[j];
            }
            
        }
        //copy[m.x, m.y].mCurrentPiece = m.mPiece;
        copy[m.currentX, m.currentY].mCurrentPiece = null;
        if (m.attacked)
        {
            copy[m.removeX, m.removeY].mCurrentPiece = null;
        }
        if (m.attacked2)
        {
            copy[m.removeX2, m.removeY2].mCurrentPiece = null;
        }
        //Board b = new BoardDraught(copy, nextPlayer, sizeX, sizeY);
        // return b;
        return null;
        //Piece[,] copy = new Cell[sizeX, sizeY]; 
        //Array.Copy(board, 0, copy, 0, board.Length);
        // Board newBoard = new Board();
        //   Array.Clear(newBoard.mAll)
        //   Array.Copy(mAllCells, 0, newBoard.mAllCells, 0, mAllCells.Length);
        string tmpName = simpleAllCells[m.currentX, m.currentY];
        simpleAllCells[m.currentX, m.currentY] = "empty";
        simpleAllCells[m.x, m.y] = tmpName;
        //mAllCells[m.currentX, m.currentY].mCurrentPiece.MakeMove(mAllCells[m.x, m.y]);
        //TODO:!
        //Array.Copy(mAllCells, 0, copy, 0, mAllCells.Length);
        //  newBoard.mAllCells = copy;
        // copy[move.y, move.x] = move; 
        //Board b = new Board(copy, nextPlayer); 
        //return b;
        return this;// newBoard;
    }*/

    public virtual string IsGameOver()
    {
        bool gameOver = false;
        int whitePiecesLeft = 0;
        int blackPiecesLeft = 0;
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                if (simpleAllCells[i, j] != null && simpleAllCells[i, j].Contains("B"))
                {
                    blackPiecesLeft++;
                }
                if (simpleAllCells[i, j] != null && simpleAllCells[i, j].Contains("W"))
                {
                    whitePiecesLeft++;
                }
            }
        }
        if (whitePiecesLeft < 2 || GetMoves(1).Count == 0)
        {
            gameOver = true;
            return "Player 1";
        }
        else if (blackPiecesLeft < 2 || GetMoves(2).Count == 0)
            return "Player 2";
        else
            return "";
       // return gameOver;
    }

    internal void resizeBoard()
    {


        float height = (Screen.height / 4) * 3;
        float width = Screen.width;
        float midHeight = height / 2;
        float midWidth = width / 2;
        float sizePieceY;
        if (sizeY < sizeX)
        {
            sizePieceY = height / sizeX;

        }
        else
        {
            sizePieceY = height / sizeY;
        }
        float offsetY = (Screen.height - height) / 2;
        float offsetX = (Screen.width - (sizeX * sizePieceY)) / 2;
        float sizePieceX = sizePieceY;

        for (int y = 0; y < this.sizeY; y++)
        {
            for (int x = 0; x < this.sizeX; x++)
            {

                GameObject currentCell = mAllCells[x, y].gameObject;


                //Position
                RectTransform rectTransform = currentCell.GetComponent<RectTransform>();

                rectTransform.sizeDelta = new Vector2(sizePieceX, sizePieceX);
                rectTransform.anchoredPosition = new Vector2((x * sizePieceX) + offsetX, (y * sizePieceX) + offsetY);

                //Setup
                currentCell.name = "CellX" + x + "Y" + y;
                mAllCells[x, y] = currentCell.GetComponent<CellDraughtV>();
                simpleAllCells[x, y] = "empty";

            }
        }
    }

    public virtual float Evaluate(int player)
    {
        Color color = Color.white;
        if (player == 1)
            color = Color.black;
        return Evaluate(color);

        //return Mathf.NegativeInfinity;
    }

    public virtual void DestroyElements()
    {
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                //Destroy(mAllCells[i, j].gameObject);
                Destroy(mAllCells[i, j]);// copy2[j];
            }


        }
    }

    public virtual float Evaluate(Color color)
    {
        float eval = 1f;
        float pointSimple = 1f;
        float pointSuccess = 5f;
        float pointAttacked = 5f;
        float pointThreat = 3f;
        float pointHide = 2f;
        int rows = sizeX;
        int cols = sizeY;
        int i;
        int j;

        for (i = 0; i < rows; i++)
        {
            for (j = 0; j < cols; j++)
            {
                Piece p = mAllCells[i, j].mCurrentPiece;
                if (p == null)
                    continue;
                if (p.mColor != color)
                    continue;
                Move[] moves = p.getPossibleActions().ToArray();
                foreach (Move m in moves)
                {
                    if (m.attacked)
                        eval += pointAttacked;
                    if (m.attacked2)
                        eval += pointAttacked;
                    if (m.threaten)
                        eval += pointThreat;
                    if (m.hide)
                        eval += pointHide;
                   // if (m.success)
                     //   eval += pointSuccess;
                    if (eval == 1f)
                        eval += pointSimple;

                }
            }
        }
        return eval;
    }

    public virtual float Evaluate()
    {
        Color color = Color.white;
        if (player == 2)
        {
            color = Color.black;
        }
        return Evaluate(color);
        //  return Mathf.NegativeInfinity;
    }


    public virtual int GetCurrentPlayer()
    {
        return player;
    }



    public virtual Move[] GetMoves(Piece piece)
    {

        List<Move> moves = new List<Move>();
        int[] moveX = new int[] { -sizeX, sizeX };
        //int moveY = -1;

        if (player == 2)
        {

        }
        //return piece.getPossibleActions().ToArray();  

        return new Move[0];
    }


    public virtual List<Move> GetMoves(int player)
    {
        allMoves = new List<Move>();
        mMovement = new Vector3Int(sizeX, sizeY, 1);
        string color;
        if (player == 1)
        {
            currentColor = Color.white;
            color = "W";
        }
        else
        {
            color = "B";
            currentColor = Color.black;
        }

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                if (simpleAllCells[i, j] != null && simpleAllCells[i, j].Contains(color))
                {
                    Vector2Int currentPos = new Vector2Int(i, j);
                    CheckPathing(currentPos);

                }
            }
        }

        return allMoves;
    }

    private void CheckPathing(Vector2Int currentPos)
    {

        CreateCellPath(1, 0, mMovement.x, currentPos);
        CreateCellPath(-1, 0, mMovement.x, currentPos);

        //Vertical
        CreateCellPath(0, 1, mMovement.y, currentPos);
        CreateCellPath(0, -1, mMovement.y, currentPos);
        // throw new NotImplementedException();
    }


    public void CreateCellPath(int xDirection, int yDirection, int movement, Vector2Int currentPos)
    {
        int currentX = currentPos.x;
        int currentY = currentPos.y;
        Move m = new Move();// this.gameObject.AddComponent<MoveDraught>();// = new MoveDraught();

        for (int i = 1; i <= movement; i++)
        {
            currentX += xDirection;
            currentY += yDirection;

            CellState cellState = CellState.None;
            cellState = ValidateCell(currentX, currentY);

            //If enemy, break;
            if (cellState == CellState.Enemy)
            {
                break;
            }

            //get CellState
            if (cellState != CellState.Free)
                break;

            //ADD to Highlighted List
            // mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);
            Vector2Int targetPos = new Vector2Int(currentX, currentY);
          //  FindMoves_new(currentPos, targetPos);
            FindMoves(currentPos, targetPos);


        }
    }


    public virtual void FindMoves(Vector2Int currentPos, Vector2Int targetPos)
    {
        int currentX = targetPos.x;
        int currentY = targetPos.y;

        int targetX = targetPos.x + 1;
        int targetY = targetPos.y;

        int allyX = targetPos.x + 2;
        int allyY = targetPos.y;

        List<Vector2Int> attackedCells = new List<Vector2Int>();

        int counter = allMoves.Count;
        Move m = new Move();
        m.mPieceName = simpleAllCells[currentPos.x, currentPos.y];
        m.attacked = false;
        m.attacked2 = false;
        m.attacked3 = false;

        m.threaten = false;
        m.hide = false;
        m.removeX = -1;
        m.removeY = -1;
        m.removeX2 = -1;
        m.removeY2 = -1;
        m.currentX = currentPos.x;
        m.currentY = currentPos.y;
        m.x = targetPos.x;
        m.y = targetPos.y;

        if (currentColor == Color.black)
            m.player = 2;
        else
            m.player = 1;

        if (ValidateCell(targetX, targetY) == CellState.Enemy
            && ValidateCell(allyX, allyY) == CellState.Friendly)
        {//check for attack
            attackedCells.Add(new Vector2Int(targetX, targetY));
            if (m.attacked2 == true && m.attacked == true)
            {
                m.attacked3 = true;
                m.removeX3 = targetX;
                m.removeY3 = targetY;
            }
            else if (m.attacked && !m.attacked2)
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

        if (ValidateCell(targetX, targetY) == CellState.Enemy
            && ValidateCell(allyX, allyY) != CellState.Friendly && ValidateCell(allyX, allyY) != CellState.OutOfBounds)
        {//check for threat
            m.threaten = true;
            for (int i = currentX; i >= 0; i--)
            {
                if (ValidateCell(i, targetPos.y) == CellState.Friendly || ValidateCell(i, targetPos.y) == CellState.OutOfBounds)
                    break;
                if (ValidateCell(i, targetPos.y) == CellState.Enemy)
                {
                    m.highAlert = true;
                }
            }
            for (int i = currentX; i < sizeX; i++)
            {
                if (ValidateCell(i, targetPos.y) == CellState.Friendly || ValidateCell(i, targetPos.y) == CellState.OutOfBounds)
                    break;
                if (ValidateCell(i, targetPos.y) == CellState.Enemy)
                {
                    m.highAlert = true;
                }
            }
            for (int j = currentY; j >= 0; j--)
            {
                if (ValidateCell(targetPos.x, j) == CellState.Friendly || ValidateCell(targetPos.x, j) == CellState.OutOfBounds)
                    break;
                if (ValidateCell(targetPos.x, j) == CellState.Enemy)
                {
                    m.highAlert = true;
                }
            }
            for (int j = currentY; j < sizeY; j++)
            {
                if (ValidateCell(targetPos.x, j) == CellState.Friendly || ValidateCell(targetPos.x, j) == CellState.OutOfBounds)
                    break;
                if (ValidateCell(targetPos.x, j) == CellState.Enemy)
                {
                    m.highAlert = true;
                }
            }
        }

        int ally2X = targetPos.x - 1;
        int ally2Y = targetPos.y;

        int ally3X = targetPos.x - 1;
        int ally3Y = targetPos.y + 1;

        allyX = targetPos.x;
        allyY = targetPos.y + 1;

        if (ValidateCell(allyX, allyY) == CellState.Friendly && ValidateCell(ally2X, ally2Y) == CellState.Friendly)
        {
            m.prepSquareHide = true;
            if (ValidateCell(ally3X, ally3Y) == CellState.Friendly)
            {
                m.prepSquareHide = false;
                m.squareHide = true;
            }
        }

        if (ValidateCell(allyX, allyY) == CellState.Friendly && ValidateCell(ally3X, ally3Y) == CellState.Friendly)
        {
            m.prepSquareHide = true;
            if (ValidateCell(ally2X, ally2Y) == CellState.Friendly)
            {
                m.prepSquareHide = false;
                m.squareHide = true;
            }
        }

        if (ValidateCell(ally2X, ally2Y) == CellState.Friendly && ValidateCell(ally3X, ally3Y) == CellState.Friendly)
        {
            m.prepSquareHide = true;
            if (ValidateCell(allyX, allyY) == CellState.Friendly)
            {
                m.prepSquareHide = false;
                m.squareHide = true;
            }
        }

        ally2X = targetPos.x + 1;
        ally2Y = targetPos.y;

        ally3X = targetPos.x + 1;
        ally3Y = targetPos.y + 1;

        //  allyX = targetPos.x;
        //  allyY = targetPos.y + 1;

        if (ValidateCell(allyX, allyY) == CellState.Friendly && ValidateCell(ally2X, ally2Y) == CellState.Friendly)
        {
            m.prepSquareHide = true;
            if (ValidateCell(ally3X, ally3Y) == CellState.Friendly)
            {
                m.prepSquareHide = false;
                m.squareHide = true;
            }
        }
        if (ValidateCell(ally2X, ally2Y) == CellState.Friendly && ValidateCell(ally3X, ally3Y) == CellState.Friendly)
        {
            m.prepSquareHide = true;
            if (ValidateCell(allyX, allyY) == CellState.Friendly)
            {
                m.prepSquareHide = false;
                m.squareHide = true;
            }
        }
        if (ValidateCell(allyX, allyY) == CellState.Friendly && ValidateCell(ally3X, ally3Y) == CellState.Friendly)
        {
            m.prepSquareHide = true;
            if (ValidateCell(ally2X, ally2Y) == CellState.Friendly)
            {
                m.prepSquareHide = false;
                m.squareHide = true;
            }
        }

        ally2X = targetPos.x - 1;
        ally2Y = targetPos.y;

        ally3X = targetPos.x - 1;
        ally3Y = targetPos.y - 1;

        allyX = targetPos.x;
        allyY = targetPos.y - 1;

        if (ValidateCell(allyX, allyY) == CellState.Friendly && ValidateCell(ally2X, ally2Y) == CellState.Friendly)
        {
            m.prepSquareHide = true;
            if (ValidateCell(ally3X, ally3Y) == CellState.Friendly)
            {
                m.prepSquareHide = false;
                m.squareHide = true;
            }
        }
        if (ValidateCell(ally2X, ally2Y) == CellState.Friendly && ValidateCell(ally3X, ally3Y) == CellState.Friendly)
        {
            m.prepSquareHide = true;
            if (ValidateCell(allyX, allyY) == CellState.Friendly)
            {
                m.prepSquareHide = false;
                m.squareHide = true;
            }
        }
        if (ValidateCell(allyX, allyY) == CellState.Friendly && ValidateCell(ally3X, ally3Y) == CellState.Friendly)
        {
            m.prepSquareHide = true;
            if (ValidateCell(ally2X, ally2Y) == CellState.Friendly)
            {
                m.prepSquareHide = false;
                m.squareHide = true;
            }
        }

        ally2X = targetPos.x + 1;
        ally2Y = targetPos.y;

        ally3X = targetPos.x + 1;
        ally3Y = targetPos.y - 1;

        allyX = targetPos.x;
        allyY = targetPos.y - 1;

        if (ValidateCell(allyX, allyY) == CellState.Friendly && ValidateCell(ally2X, ally2Y) == CellState.Friendly)
        {
            m.prepSquareHide = true;
            if (ValidateCell(ally3X, ally3Y) == CellState.Friendly)
            {
                m.prepSquareHide = false;
                m.squareHide = true;
            }
        }
        if (ValidateCell(ally2X, ally2Y) == CellState.Friendly && ValidateCell(ally3X, ally3Y) == CellState.Friendly)
        {
            m.prepSquareHide = true;
            if (ValidateCell(allyX, allyY) == CellState.Friendly)
            {
                m.prepSquareHide = false;
                m.squareHide = true;
            }
        }
        if (ValidateCell(allyX, allyY) == CellState.Friendly && ValidateCell(ally3X, ally3Y) == CellState.Friendly)
        {
            m.prepSquareHide = true;
            if (ValidateCell(ally2X, ally2Y) == CellState.Friendly)
            {
                m.prepSquareHide = false;
                m.squareHide = true;
            }
        }

        targetX = targetPos.x - 1;
        targetY = targetPos.y;

        allyX = targetPos.x - 2;
        allyY = targetPos.y;


        if (ValidateCell(targetX, targetY) == CellState.Enemy
            && ValidateCell(allyX, allyY) == CellState.Friendly)
        {//check for attack
            attackedCells.Add(new Vector2Int(targetX, targetY));
            if (m.attacked2 == true && m.attacked == true)
            {
                m.attacked3 = true;
                m.removeX3 = targetX;
                m.removeY3 = targetY;
            }
            else if (m.attacked && !m.attacked2)
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

        if (ValidateCell(targetX, targetY) == CellState.Enemy
            && ValidateCell(allyX, allyY) != CellState.Friendly && ValidateCell(allyX, allyY) != CellState.OutOfBounds)
        {//check for threat
            m.threaten = true;
            for (int i = currentX; i >= 0; i--)
            {
                if (ValidateCell(i, targetPos.y) == CellState.Friendly || ValidateCell(i, targetPos.y) == CellState.OutOfBounds)
                    break;
                if (ValidateCell(i, targetPos.y) == CellState.Enemy)
                {
                    m.highAlert = true;
                }
            }
            for (int i = currentX; i < sizeX; i++)
            {
                if (ValidateCell(i, targetPos.y) == CellState.Friendly || ValidateCell(i, targetPos.y) == CellState.OutOfBounds)
                    break;
                if (ValidateCell(i, targetPos.y) == CellState.Enemy)
                {
                    m.highAlert = true;
                }
            }
            for (int j = currentY; j >= 0; j--)
            {
                if (ValidateCell(targetPos.x, j) == CellState.Friendly || ValidateCell(targetPos.x, j) == CellState.OutOfBounds)
                    break;
                if (ValidateCell(targetPos.x, j) == CellState.Enemy)
                {
                    m.highAlert = true;
                }
            }
            for (int j = currentY; j < sizeY; j++)
            {
                if (ValidateCell(targetPos.x, j) == CellState.Friendly || ValidateCell(targetPos.x, j) == CellState.OutOfBounds)
                    break;
                if (ValidateCell(targetPos.x, j) == CellState.Enemy)
                {
                    m.highAlert = true;
                }
            }
        }

        targetX = targetPos.x;
        targetY = targetPos.y + 1;

        allyX = targetPos.x;
        allyY = targetPos.y + 2;

        if (ValidateCell(targetX, targetY) == CellState.Enemy
            && ValidateCell(allyX, allyY) == CellState.Friendly)
        {//check for attack
            attackedCells.Add(new Vector2Int(targetX, targetY));
            if (m.attacked2 == true && m.attacked == true)
            {
                m.attacked3 = true;
                m.removeX3 = targetX;
                m.removeY3 = targetY;
            }
            else if (m.attacked && !m.attacked2)
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

        if (ValidateCell(targetX, targetY) == CellState.Enemy
            && ValidateCell(allyX, allyY) != CellState.Friendly && ValidateCell(allyX, allyY) != CellState.OutOfBounds)
        {//check for threat
            m.threaten = true;
        }

        targetX = targetPos.x;
        targetY = targetPos.y - 1;

        allyX = targetPos.x;
        allyY = targetPos.y - 2;

        if (ValidateCell(targetX, targetY) == CellState.Enemy
            && ValidateCell(allyX, allyY) == CellState.Friendly)
        {//check for attack
            attackedCells.Add(new Vector2Int(targetX, targetY));
            if (m.attacked2 == true && m.attacked == true)
            {
                m.attacked3 = true;
                m.removeX3 = targetX;
                m.removeY3 = targetY;
            }
            else if (m.attacked && !m.attacked2)
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

        if (ValidateCell(targetX, targetY) == CellState.Enemy
            && ValidateCell(allyX, allyY) != CellState.Friendly && 
            ValidateCell(allyX, allyY) != CellState.OutOfBounds)
        {//check for threat
            m.threaten = true;
            for (int i = currentX; i >= 0; i--)
            {
                if (ValidateCell(i, targetPos.y) == CellState.Friendly || ValidateCell(i, targetPos.y) == CellState.OutOfBounds)
                    break;
                if (ValidateCell(i, targetPos.y) == CellState.Enemy)
                {
                    m.highAlert = true;
                }
            }
            for (int i = currentX; i < sizeX; i++)
            {
                if (ValidateCell(i, targetPos.y) == CellState.Friendly || ValidateCell(i, targetPos.y) == CellState.OutOfBounds)
                    break;
                if (ValidateCell(i, targetPos.y) == CellState.Enemy)
                {
                    m.highAlert = true;
                }
            }
            for (int j = currentY; j >= 0; j--)
            {
                if (ValidateCell(targetPos.x, j) == CellState.Friendly || ValidateCell(targetPos.x, j) == CellState.OutOfBounds)
                    break;
                if (ValidateCell(targetPos.x, j) == CellState.Enemy)
                {
                    m.highAlert = true;
                }
            }
            for (int j = currentY; j < sizeY; j++)
            {
                if (ValidateCell(targetPos.x, j) == CellState.Friendly || ValidateCell(targetPos.x, j) == CellState.OutOfBounds)
                    break;
                if (ValidateCell(targetPos.x, j) == CellState.Enemy)
                {
                    m.highAlert = true;
                }
            }


        }

        if (ValidateCell(targetX, targetY) != CellState.Enemy
            && ValidateCell(allyX, allyY) == CellState.Friendly)
        {
            m.hide = true;
        }



         targetX = targetPos.x - 1;
         targetY = targetPos.y - 1;

         allyX = targetPos.x - 1;
         allyY = targetPos.y - 2;

        if (ValidateCell(targetX, targetY) == CellState.Enemy
    && ValidateCell(allyX, allyY) == CellState.Friendly)
        {
            m.highThreat = true;
        }

        targetX = targetPos.x - 1;
        targetY = targetPos.y - 1;

        allyX = targetPos.x - 2;
        allyY = targetPos.y - 1;

        if (ValidateCell(targetX, targetY) == CellState.Enemy
&& ValidateCell(allyX, allyY) == CellState.Friendly)
        {
            m.highThreat = true;
        }
        //----------------
        targetX = targetPos.x + 1;
        targetY = targetPos.y - 1;

        allyX = targetPos.x + 1;
        allyY = targetPos.y - 2;
        if (ValidateCell(targetX, targetY) == CellState.Enemy
&& ValidateCell(allyX, allyY) == CellState.Friendly)
        {
            m.highThreat = true;
        }
        targetX = targetPos.x + 1;
        targetY = targetPos.y - 1;

        allyX = targetPos.x + 2;
        allyY = targetPos.y - 1;
        if (ValidateCell(targetX, targetY) == CellState.Enemy
&& ValidateCell(allyX, allyY) == CellState.Friendly)
        {
            m.highThreat = true;
        }
        //--------
        targetX = targetPos.x + 1;
        targetY = targetPos.y + 1;

        allyX = targetPos.x + 1;
        allyY = targetPos.y + 2;

        if (ValidateCell(targetX, targetY) == CellState.Enemy
&& ValidateCell(allyX, allyY) == CellState.Friendly)
        {
            m.highThreat = true;
        }
        targetX = targetPos.x + 1;
        targetY = targetPos.y + 1;

        allyX = targetPos.x + 2;
        allyY = targetPos.y + 1;

        if (ValidateCell(targetX, targetY) == CellState.Enemy
&& ValidateCell(allyX, allyY) == CellState.Friendly)
        {
            m.highThreat = true;
        }
        //---------
        targetX = targetPos.x - 1;
        targetY = targetPos.y + 1;

        allyX = targetPos.x - 2;
        allyY = targetPos.y + 1;

        if (ValidateCell(targetX, targetY) == CellState.Enemy
&& ValidateCell(allyX, allyY) == CellState.Friendly)
        {
            m.highThreat = true;
        }

        targetX = targetPos.x - 1;
        targetY = targetPos.y + 1;

        allyX = targetPos.x - 1;
        allyY = targetPos.y + 2;

        if (ValidateCell(targetX, targetY) == CellState.Enemy
&& ValidateCell(allyX, allyY) == CellState.Friendly)
        {
            m.highThreat = true;
        }

            allMoves.Add(m);
    }

    public virtual void FindMoves_new(Vector2Int currentPos, Vector2Int targetPos)
    {
        int currentX = targetPos.x;
        int currentY = targetPos.y;

        int targetX = targetPos.x + 1;
        int targetY = targetPos.y;

        int allyX = targetPos.x + 2;
        int allyY = targetPos.y;

        List<Vector2Int> attackedCells = new List<Vector2Int>();
        int counter = allMoves.Count;
        Move m = new Move();
        m.removeX = -1;
        m.removeY = -1;
        m.removeX2 = -1;
        m.removeY2 = -1;
        m.currentX = currentPos.x;
        m.currentY = currentPos.y;
        m.x = targetPos.x;
        m.y = targetPos.y;

        for (int i = targetPos.y; i < sizeY; i++)
        {
            for (int j = targetPos.x; j < sizeX; j++)
            {
                if (ValidateCell(j,i) == CellState.Enemy)
                {
                    if (j == targetPos.x+1 && i==targetPos.y)
                    {
                        m.highAlertRight = true;
                        m.threatRight = true;
                    }
                    if (j == targetPos.x && i == targetPos.y + 1)
                    {
                        m.threatUp = true;
                        m.highAlertUp = true;
                    }

                }
                if (ValidateCell(j, i) == CellState.Friendly)
                {
                    if (j == targetPos.x + 1 && i == targetPos.y)
                    {
                        m.highHideRight = true;
                    }
                    if (j == targetPos.x && i == targetPos.y + 1) 
                    { 
                        m.highHideUp = true;
                    }
                    if (j == targetPos.x && i == targetPos.y + 2 && m.highThreatUp)
                    {
                        if (m.attacked)
                        {
                            m.attacked2 = true;
                            m.removeX2 = j;
                            m.removeY2 = i;
                        }
                        else if (m.attacked2)
                        {
                            m.attacked3 = true;
                            m.removeX3 = j;
                            m.removeY3 = i;
                        }
                        else
                        {
                            m.attacked = true;
                            m.removeX = j;
                            m.removeY = i;
                        }
                    }
                    if (j == targetPos.x + 2 && i == targetPos.y && m.highThreatRight)
                    {
                        if (m.attacked)
                        {
                            m.attacked2 = true;
                            m.removeX2 = j;
                            m.removeY2 = i;
                        }
                        else if (m.attacked2)
                        {
                            m.attacked3 = true;
                            m.removeX3 = j;
                            m.removeY3 = i;
                        }
                        else
                        {
                            m.attacked = true;
                            m.removeX = j;
                            m.removeY = i;
                        }
                    }


                }
                if (ValidateCell(j, i) == CellState.OutOfBounds)
                {
                    break;
                }
                if (ValidateCell(j, i) == CellState.Free)
                {
                    
                }
            }
        }

        for (int i = targetPos.y; i >= 0; i--)
        {
            for (int j = targetPos.x; j >= 0; j--)
            {
                if (ValidateCell(j, i) == CellState.Enemy)
                {
                    if (j == targetPos.x - 1 && i == targetPos.y)
                    {
                        m.highAlertLeft = true;
                    }
                    if (j == targetPos.x && i == targetPos.y - 1)
                    {
                        m.highAlertDown = true;
                    }
                    if ( j == targetPos.x - 1 && i < targetPos.y - 1 && m.highAlertRight)
                    {
                        m.danger = true;
                    }
                    if (i == targetPos.y - 1 && i < targetPos.x - 1 && m.highAlertRight)
                    {
                        m.danger = true;
                    }
                    if (j == targetPos.x - 1 && i < targetPos.y - 1 && m.alertRight)
                    {
                        m.dangerLow = true;
                    }
                    if (i == targetPos.y - 1 && i < targetPos.x - 1 && m.alertRight)
                    {
                        m.dangerLow = true;
                    }
                }
                if (ValidateCell(j, i) == CellState.Friendly)
                {
                    if (j == targetPos.x - 1 && i == targetPos.y)
                    {
                        m.highHideRight = true;
                    }
                    if (j == targetPos.x && i == targetPos.y - 1)
                    {
                        m.highHideUp = true;
                    }
                    if (j == targetPos.x && i == targetPos.y - 2 && m.highThreatDown)
                    {
                        if (m.attacked)
                        {
                            m.attacked2 = true;
                            m.removeX2 = j;
                            m.removeY2 = i;
                        }
                        else if (m.attacked2)
                        {
                            m.attacked3 = true;
                            m.removeX3 = j;
                            m.removeY3 = i;
                        }
                        else
                        {
                            m.attacked = true;
                            m.removeX = j;
                            m.removeY = i;
                        }
                    }
                    if (j == targetPos.x - 2 && i == targetPos.y && m.highThreatLeft)
                    {
                        if (m.attacked)
                        {
                            m.attacked2 = true;
                            m.removeX2 = j;
                            m.removeY2 = i;
                        }
                        else if (m.attacked2)
                        {
                            m.attacked3 = true;
                            m.removeX3 = j;
                            m.removeY3 = i;
                        }
                        else
                        {
                            m.attacked = true;
                            m.removeX = j;
                            m.removeY = i;
                        }
                    }



                }
                if (ValidateCell(j, i) == CellState.OutOfBounds)
                {
                    break;
                }
                if (ValidateCell(j, i) == CellState.Free)
                {

                }

            }
            if (ValidateCell(sizeX, i) == CellState.OutOfBounds)
            {
                break;
            }
        }

        for (int i = targetPos.y + 1; i < sizeY; i++)
        {
            for (int j = targetPos.x - 1; j > 0; j--)
            {
                if (ValidateCell(j, i) == CellState.Enemy)
                {
                    if (j == targetPos.x - 1 && i == targetPos.y + 1 && (m.highAlertRight || m.highAlertDown))
                    {
                        m.danger = true;
                    }
                    if (j == targetPos.x - 1 && i > targetPos.y + 1 )
                    {
                        m.danger = true;
                    }
                }
                if (ValidateCell(j, i) == CellState.Friendly)
                {
                    if (j == targetPos.x - 1 && i == targetPos.y + 1 && m.highHideLeft && m.highHideUp == false)
                    {
                        m.prepSquareHideUpMiss = true;
                        //m.highHideRight = true;
                    }
                    if (j == targetPos.x - 1 && i == targetPos.y + 1 && m.highHideLeft && m.highHideUp)
                    {
                        m.squareHide = true;
                    }


                        if (j == targetPos.x - 1 && i > targetPos.y + 1)
                    {
                        
                    }


                }
                    if (ValidateCell(j, i) == CellState.OutOfBounds)
                {
                    break;
                }
                if (ValidateCell(j, i) == CellState.Free)
                {

                }
            }

        }

        for (int i = targetPos.y - 1; i > 0; i--)
        {
            for (int j = targetPos.x + 1; j < sizeX; j++)
            {
                if (ValidateCell(j, i) == CellState.Enemy)
                {
                    if (i == targetPos.y -1 && j == targetPos.x+1 && m.highAlertLeft)
                    {
                        m.danger = true;
                    }
                    if (i == targetPos.y - 1 && j == targetPos.x + 1 && m.highAlertUp)
                    {
                        m.danger = true;
                    }
                    if (i == targetPos.y - 1 && j == targetPos.x + 1 && !m.highAlertUp && !m.highAlertLeft && !m.danger)
                    {
                        m.dangerLow = true; 
                    }


                }
                if (ValidateCell(j, i) == CellState.Friendly)
                {
                    if (j == targetPos.x + 1 && i == targetPos.y - 1 && m.highHideRight && m.highHideDown == false)
                    {
                        m.prepSquareHideDownMiss = true;
                        //m.highHideRight = true;
                    }
                    if (j == targetPos.x - 1 && i == targetPos.y + 1 && m.highHideRight && m.highHideDown)
                    {
                        m.squareHide = true;
                    }

                    if (j == targetPos.x - 1 && i > targetPos.y + 1)
                    {

                    }
                }
                if (ValidateCell(j, i) == CellState.OutOfBounds)
                {
                    break;
                }
                if (ValidateCell(j, i) == CellState.Free)
                {

                }
            }
        }
        allMoves.Add(m);
    }
    public virtual void FindAggressiveMoves(Vector2Int currentPos, Vector2Int targetPos)
    {
        int currentX = targetPos.x;
        int currentY = targetPos.y;

        int targetX = targetPos.x + 1;
        int targetY = targetPos.y;

        int allyX = targetPos.x + 2;
        int allyY = targetPos.y;

        List<Vector2Int> attackedCells = new List<Vector2Int>();

        int counter = allMoves.Count;
        Move m = new Move();
        m.mPieceName = simpleAllCells[currentPos.x, currentPos.y];
        m.attacked = false;
        m.attacked2 = false;
        m.threaten = false;
        m.hide = false;
        m.removeX = -1;
        m.removeY = -1;
        m.removeX2 = -1;
        m.removeY2 = -1;
        m.currentX = currentPos.x;
        m.currentY = currentPos.y;
        m.x = targetPos.x;
        m.y = targetPos.y;



        if (currentColor == Color.black)
            m.player = 2;
        else
            m.player = 1;


        if (ValidateCell(targetX, targetY) == CellState.Enemy
        && ValidateCell(allyX, allyY) == CellState.Friendly)
        {//check for attack
            attackedCells.Add(new Vector2Int(targetX, targetY));
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

        if (ValidateCell(targetX, targetY) == CellState.Enemy
            && ValidateCell(allyX, allyY) != CellState.Friendly)
        {//check for threat
            m.threaten = true;
        }

        targetX = targetPos.x - 1;
        targetY = targetPos.y;

        allyX = targetPos.x - 2;
        allyY = targetPos.y;

        if (ValidateCell(targetX, targetY) == CellState.Enemy
&& ValidateCell(allyX, allyY) == CellState.Friendly)
        {//check for attack
            attackedCells.Add(new Vector2Int(targetX, targetY));
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

        if (ValidateCell(targetX, targetY) == CellState.Enemy
    && ValidateCell(allyX, allyY) != CellState.Friendly)
        {//check for threat
            m.threaten = true;
        }

        targetX = targetPos.x;
        targetY = targetPos.y + 1;

        allyX = targetPos.x;
        allyY = targetPos.y + 2;

        if (ValidateCell(targetX, targetY) == CellState.Enemy
    && ValidateCell(allyX, allyY) == CellState.Friendly)
        {//check for attack
            attackedCells.Add(new Vector2Int(targetX, targetY));
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

        if (ValidateCell(targetX, targetY) == CellState.Enemy
&& ValidateCell(allyX, allyY) != CellState.Friendly)
        {//check for threat
            m.threaten = true;
        }

        targetX = targetPos.x;
        targetY = targetPos.y - 1;

        allyX = targetPos.x;
        allyY = targetPos.y - 2;

        if (ValidateCell(targetX, targetY) == CellState.Enemy
    && ValidateCell(allyX, allyY) == CellState.Friendly)
        {//check for attack
            attackedCells.Add(new Vector2Int(targetX, targetY));
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

        if (ValidateCell(targetX, targetY) == CellState.Enemy
&& ValidateCell(allyX, allyY) != CellState.Friendly)
        {//check for threat
            m.threaten = true;
        }

        if (ValidateCell(targetX, targetY) != CellState.Enemy
&& ValidateCell(allyX, allyY) == CellState.Friendly)
        {
            m.hide = true;
        }
        allMoves.Add(m);
    }


    public virtual CellState ValidateCell(int targetX, int targetY)
    {
        if (targetX < 0 || targetX > (sizeX - 1))
            return CellState.OutOfBounds;

        if (targetY < 0 || targetY > (sizeY - 1))
            return CellState.OutOfBounds;

        if (simpleAllCells[targetX, targetY] != null)
        {
            if (player == 1)
            {
                if (simpleAllCells[targetX, targetY].Contains("W"))
                {

                    return CellState.Friendly;
                }
                else if (simpleAllCells[targetX, targetY].Contains("B"))
                {
                    return CellState.Enemy;
                }
            }
            if (player == 2)
                if (simpleAllCells[targetX, targetY].Contains("B"))
                {

                    return CellState.Friendly;
                }
                else if (simpleAllCells[targetX, targetY].Contains("W"))
                {
                    return CellState.Enemy;
                }
        }
        if (simpleAllCells[targetX, targetY] == null)
        {
            return CellState.Free;
        }
        return CellState.None;
    }

    public void Create(int newSizeX, int newSizeY)
    {
        this.sizeX = newSizeX;
        this.sizeY = newSizeY;
        //UnityEngine.Object.Instantiate(mCellPrefab, new Vector3(y * 3 - sizeX / 2, x * 3 - sizeY / 2), Quaternion.identity);
        //gameOver = false;
        mAllCells = new CellDraughtV[this.sizeX, this.sizeY];
        simpleAllCells = new string[this.sizeX, this.sizeY];
        mMovement = new Vector3Int(sizeX, sizeY, 1);
        //  this.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 125));
        player = 1;



        float height = (Screen.height / 4) * 3;
        float width = Screen.width;
        float midHeight = height / 2;
        float midWidth = width / 2;
        float sizePieceY;
        if (sizeY < sizeX)
        {
            sizePieceY = height / sizeX;

        }
        else
        {
            sizePieceY = height / sizeY;
        }
        float offsetY = (Screen.height - height) / 2;
        float offsetX = (Screen.width - (sizeX*sizePieceY))/2;
        //float ratio = width / height;
        //float sizePieceX = width / sizeX;
        float sizePieceX = sizePieceY;//*ratio;

        //float offset = (width - height) / 2;


        for (int y = 0; y < this.sizeY; y++)
        {
            for (int x = 0; x < this.sizeX; x++)
            {

                // mCellPrefab.transform.
                GameObject newCell = Instantiate(mCellPrefab, transform);//Instantiate(mCellPrefab, new Vector3(x * newX, y*newY), Quaternion.identity);
                                                                         //Instantiate(mCellPrefab, transform);


                //Position


                float ii = Screen.width / (sizeX);
                float jj = Screen.height / (sizeY);
                RectTransform rectTransform = newCell.GetComponent<RectTransform>();
                float cameraSize = Camera.main.orthographicSize;

                rectTransform.sizeDelta = new Vector2(sizePieceX, sizePieceX);
                rectTransform.anchoredPosition = new Vector2((x * sizePieceX) + offsetX , (y * sizePieceX) + offsetY);

                //Setup
                newCell.name = "CellX" + x + "Y" + y;
                mAllCells[x, y] = newCell.GetComponent<CellDraughtV>();
                mAllCells[x, y].Setup(new Vector2Int((int)(x), (int)y), this);
                simpleAllCells[x, y] = "empty";

            }
        }
    }

    public CellState ValidateCell(int targetX, int targetY, Piece checkingPiece)
    {

        //Bounds check
        if (targetX < 0 || targetX > (sizeX - 1))
            return CellState.OutOfBounds;

        if (targetY < 0 || targetY > (sizeY - 1))
            return CellState.OutOfBounds;

        // Get cell
        CellDraughtV targetCell = mAllCells[targetX, targetY];

        // If the cell has a piece
        if (targetCell.mCurrentPiece != null)
        {
            //if friendly
            if (checkingPiece.mColor == targetCell.mCurrentPiece.mColor)
                return CellState.Friendly;

            if (checkingPiece.mColor != targetCell.mCurrentPiece.mColor)
                return CellState.Enemy;
        }

        //if (mAllCells[targetX, targetY].mCurrentPiece != null && mAllCells[targetX, targetY].mCurrentPiece.name == checkingPiece.name)
        //  return CellState.Free;
        if (targetCell.mCurrentPiece == null)
            return CellState.Free;

        return CellState.None;


    }

    public virtual void NextPlayer()
    {
        if (player == 1)
        {
            player = 2;
        }
        else if (player == 2)
        {
            player = 1;
        }
    }


    /* public void copyBoard ()
     {
         Board newItem = new Board();
         GameObject referenceObject;
         referenceObject = GameObject.FindGameObjectWithTag("BoardCanvas");
         CopySpecialComponents(referenceObject.gameObject, newItem.gameObject);
     }*/

    /*  private void CopySpecialComponents(GameObject _sourceGO, GameObject _targetGO)
      {
          foreach (var component in _sourceGO.GetComponents<Component>())
          {
              var componentType = component.GetType();
              if (componentType != typeof(Transform) &&
                  componentType != typeof(MeshFilter) &&
                  componentType != typeof(MeshRenderer)
                  )
              {
                  Debug.Log("Found a component of type " + component.GetType());
                  UnityEditorInternal.ComponentUtility.CopyComponent(component);
                  UnityEditorInternal.ComponentUtility.PasteComponentAsNew(_targetGO);
                  Debug.Log("Copied " + component.GetType() + " from " + _sourceGO.name + " to " + _targetGO.name);
              }
          }
      }*/

    public string[,] getDraughtAsStrings()
    {
        string[,] draught = new string[sizeX, sizeY];

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                if (mAllCells[i, j].mCurrentPiece != null)
                    draught[i, j] = mAllCells[i, j].mCurrentPiece.name;

                else
                    draught[i, j] = null;

            }


        }
        return draught;
    }


    public BoardDraught getDraught()
    {
        CellDraught[,] copy = new CellDraught[sizeX, sizeY];
        CellDraught[] copy2 = new CellDraught[sizeX];

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                //  copy[i, j].mBoardPosition = mAllCells[i, j].mBoardPosition;
                if (mAllCells[i, j].mCurrentPiece != null)
                    copy[i, j].Setup(mAllCells[i, j].mBoardPosition, mAllCells[i, j].mCurrentPiece.name);

                // copy[i, j].mCurrentPiece = mAllCells[i, j].mCurrentPiece.name;
                else
                    copy[i, j].Setup(mAllCells[i, j].mBoardPosition, null);

            }


        }
        //BoardDraught b = new BoardDraught(copy, player, sizeX, sizeY);
        //return b;
        return null;
    }

    /* public Board DeepCopy ()
     {
        /Board b = new Board();
         b.mAllCells = this.mAllCells.Clone() as Cell[,];
      //   b.mAllCells = this.mAllCells;//Array.Copy(mAllCells, b.mAllCells, mAllCells.Length);
         b.player = this.player;
         b.sizeX = this.sizeX;
         b.sizeY = this.sizeY;
         return b;
     }*/


}