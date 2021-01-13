using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardDraught : Board
{
    //public string[,] simpleAllCells;
    //public int sizeX;
    //public int sizeY;
    [HideInInspector]
    //public string[,] simpleAllCells;
    //protected int player;
    //private bool gameOver = false;
   private Vector3Int mMovement;
   private Color currentColor = Color.clear;
    List<Move> allMoves = new List<Move>();
    protected string movedPiece;
    protected int oldX;
    protected int oldY;
    protected int currentX;
    protected int currentY;
    protected Move lastMove = null;
    protected Move currentMove = null;


    float eval = 1f;
    float pointSimple = 1f;
    float pointSuccess = 250f;
    float pointAttacked = 100f;
    float pointThreat = 10f;
    float pointHide = 100f;
    float pointHighThreat = 75f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCurrentMove(Move newMove)
    {
        currentMove = newMove;
    }

    public BoardDraught(string[,] copyBoard, int nextPlayer, int rows, int cols, float newAttackPoints, float newHidePoints, float newThreatPoints, float newHighThreatPoints)
    {
        simpleAllCells = copyBoard; 
        player = nextPlayer;
        this.sizeX = rows;
        this.sizeY = cols;
        mMovement = new Vector3Int(sizeX, sizeY, 1);
        pointAttacked = newAttackPoints;
        pointHide = newHidePoints;
        pointThreat = newThreatPoints;
        pointHighThreat = newHighThreatPoints;
    }

    public void changeAttackPoints ()
    {

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

 

    public override void FindMoves(Vector2Int currentPos, Vector2Int targetPos)
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
        //GameObject referenceObject;

        //referenceObject = GameObject.FindGameObjectWithTag("BoardCanvas");

      // MoveDraught m = referenceObject.AddComponent<MoveDraught>();// = new MoveDraught();
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
        {
            attackedCells.Add(new Vector2Int (targetX, targetY));
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
            && ValidateCell(allyX, allyY) != CellState.Friendly
            && ValidateCell(allyX, allyY) != CellState.OutOfBounds)
        {
            m.threaten = true;
        }

        targetX = targetPos.x - 1;
        targetY = targetPos.y;

        allyX = targetPos.x - 2;
        allyY = targetPos.y;

        if (ValidateCell(targetX, targetY) == CellState.Enemy
&& ValidateCell(allyX, allyY) == CellState.Friendly)
        {
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
    && ValidateCell(allyX, allyY) != CellState.Friendly
    && ValidateCell(allyX, allyY) != CellState.OutOfBounds)
        {
            m.threaten = true;
        }

        targetX = targetPos.x;
        targetY = targetPos.y + 1;

        allyX = targetPos.x;
        allyY = targetPos.y + 2;

        if (ValidateCell(targetX, targetY) == CellState.Enemy
    && ValidateCell(allyX, allyY) == CellState.Friendly)
        {
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
&& ValidateCell(allyX, allyY) != CellState.Friendly
&& ValidateCell(allyX, allyY) != CellState.OutOfBounds)
        {
            m.threaten = true;
        }

        targetX = targetPos.x;
        targetY = targetPos.y - 1;

        allyX = targetPos.x;
        allyY = targetPos.y - 2;

        if (ValidateCell(targetX, targetY) == CellState.Enemy
    && ValidateCell(allyX, allyY) == CellState.Friendly)
        {
            attackedCells.Add(new Vector2Int (targetX, targetY));
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
&& ValidateCell(allyX, allyY) != CellState.Friendly
&& ValidateCell(allyX, allyY) != CellState.OutOfBounds)
        {
            m.threaten = true;
        }

        if (ValidateCell(targetX, targetY) != CellState.Enemy
&& ValidateCell(allyX, allyY) == CellState.Friendly)
        {
            m.hide = true;
        }

        allMoves.Add(m);
    }

    public override CellState ValidateCell(int targetX, int targetY)
    {
        if (targetX < 0 || targetX > (sizeX - 1))
            return CellState.OutOfBounds;

        if (targetY < 0 || targetY > (sizeY - 1))
            return CellState.OutOfBounds;

        if (simpleAllCells[targetX, targetY] != null)
        {
            if (player == 1) {
                if (simpleAllCells[targetX, targetY].Contains("W")) {

                    return CellState.Friendly; }
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


   


    public BoardDraught MakeMove(Move m)
    {

        //int nextPlayer;

        //Copy Board and make move#


     /*   int nextPlayer;
        if (player == 1)
            nextPlayer = 2;
        else
            nextPlayer = 1;*/
        // string[,] copy = new string[sizeX, sizeY];

        //   copy = mAllCells;

        /*for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                copy[i, j] = mAllCells[i, j];
            }


        }*/
        //mAllCells[m.x,m.y] = m.mPieceName;
        /*copy[m.x, m.y] = m.mPieceName;
        copy[m.currentX, m.currentY] = null;
        if (m.attacked)
        {
            copy[m.removeX, m.removeY] = null;
        }
        if (m.attacked2)
        {
            copy[m.removeX2, m.removeY2] = null;
        }*/
        lastMove = m;
      /*  movedPiece = m.mPieceName;
        oldX = m.currentX;
        oldY = m.currentY;
        currentX = m.x;
        currentY = m.y;*/
        simpleAllCells[m.x, m.y] = m.mPieceName;
        simpleAllCells[m.currentX, m.currentY] = null;
        if (m.attacked)
        {
            lastMove.attackedPiece = simpleAllCells[m.removeX, m.removeY];
            simpleAllCells[m.removeX, m.removeY] = null;
        }
        if (m.attacked2)
        {
            lastMove.attackedPiece2 = simpleAllCells[m.removeX2, m.removeY2];
            simpleAllCells[m.removeX2, m.removeY2] = null;
        }
        //BoardDraught b = new BoardDraught(copy, nextPlayer, sizeX, sizeY);
        currentMove = m;

        return this;
    }

    public void StepBack()
    {
        simpleAllCells[lastMove.x, lastMove.y] = null;
        simpleAllCells[lastMove.currentX, lastMove.currentY] = movedPiece;
        if (lastMove.attackedPiece != null)
        {
            simpleAllCells[lastMove.removeX, lastMove.removeY] = lastMove.attackedPiece;

        }
        if (lastMove.attackedPiece2 != null)
        {
            simpleAllCells[lastMove.removeX2, lastMove.removeY2] = lastMove.attackedPiece2;
            
        }

    }

    public bool IsGameOver()
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
        if (whitePiecesLeft < 2 || blackPiecesLeft < 2 || GetMoves(player).Count == 0)
        {
            gameOver = true;
        }
            return gameOver;
    }


    public override List<Move> GetMoves(int player)
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

    public override float Evaluate(int player)
    {
        string color = "W";
        if (player == 1)
            color ="B";
        return Evaluate(color);

        //return Mathf.NegativeInfinity;
    }

    /* public virtual float Evaluate(int player, Move m)
     {
         string color = "W";
         if (player == 1)
             color = "B";
         return Evaluate(color);

         //return Mathf.NegativeInfinity;
     }*/
    public void AdjustAttackPoints(float newAttackPoints)
    {
       // pointAttacked = attackSlider.value;
        //Debug.Log(pointAttacked);
    }
  

    public void AdjustThreatPoints(float newThreatPoints)
    {
        pointThreat = newThreatPoints;
    }

    public void AdjustSuccessPoints(float newWinPoints)
    {
        pointSuccess = newWinPoints;

    }

    public void AdjustHidePoints(float newHidePoints)
    {
        pointHide = newHidePoints;
    }

    public float Evaluate(string color)
    {
      float eval = 1f;


    int rows = sizeX;
        int cols = sizeY;

                    if (currentMove.attacked)
                        eval += pointAttacked;
                    if (currentMove.attacked2)
                        eval += pointAttacked;
                    if (currentMove.threaten)
                        eval += pointThreat;
                    if (currentMove.hide)
                        eval += pointHide;
            //        if (eval == 1f)
             //           eval += pointSimple;
                    //if (currentMove.success)
                      //  eval += pointSuccess;
                    if (currentMove.highThreat)
                        eval += pointHighThreat;
                    if (IsGameOver())
                        eval += pointSuccess;
                        currentMove.mScore += eval;


        //    }
        //  }
      //  return 1;
        return eval;
    }
    public float Evaluate_old(string color)
    {
        float eval = 1f;
        float pointSimple = 1f;
        float pointSuccess = 5f;
        float pointAttacked = 100f;
        float pointThreat = 50f;
        float pointHide = -2f;
        int rows = sizeX;
        int cols = sizeY;
        int i;
        int j;

        for (i = 0; i < rows; i++)
        {
            for (j = 0; j < cols; j++)
            {
                string p = simpleAllCells[i, j];
                if (p == null || !(p.Contains(color)))
                    continue;
                Move[] moves = GetMoves(player).ToArray();//p.getPossibleActions().ToArray();
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
                    if (eval == 1f)
                        eval += pointSimple;
                   // if (m.success)
                     //   eval += pointSuccess;
                    m.mScore += eval;
                }
            }
        }
        return eval;
    }


}
