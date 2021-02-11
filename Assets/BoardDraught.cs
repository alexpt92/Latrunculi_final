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


    float dangerPoints = -10f;
    float dangerLowPoints = -5f;
    float squarePoints = 30f; 
    float prepSquareHidePoints = 7f;
    float attackPoints = 50f;
    float pointHighHide = 5f;
    private List<String> mWPieces = null;
    private List<String> mBPieces = null;

    float eval = 1f;
    float pointSimple = 1f;
    float pointSuccess = 250f;
    float pointAttacked = 100f;
    float pointThreat = 10f;
    float pointHide = 20f;
    float pointHighThreat = 60f;
    float pointSquareHide = 40f;
    float pointHighAlert = -100f;
    float pointCorner = 80f;
    float pointPrepSquad = 20f;
    float pointOOBAndCorner = 20f;
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

    public BoardDraught(string[,] copyBoard, int nextPlayer, int rows, int cols, float newAttackPoints, float newHidePoints, float newThreatPoints, float newHighThreatPoints, float newSquadPoints, float newCornerPoints, float newHighAlertPoints, float newPrepSquadPoints, List<Piece> newMWPieces, List<Piece> newMBPieces)
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
        pointPrepSquad = newPrepSquadPoints;
        Piece[] test = newMWPieces.ToArray();
        GameObject test2 =  test[1].gameObject;
        mWPieces = new List<string>();
        mBPieces = new List<string>();
        for (int i = 0; i < newMWPieces.ToArray().Length-1; i++)
        {
            mWPieces.Add(newMWPieces.ToArray()[i].gameObject.name);
           // mWPieces[i] = newMWPieces.ToArray()[i].gameObject.name;

        }
        for (int i = 0; i < newMBPieces.ToArray().Length-1; i++)
        {
            mBPieces.Add(newMBPieces.ToArray()[i].gameObject.name);

           // mBPieces[i] = newMBPieces.ToArray()[i].gameObject.name;

        }
     //   mWPieces = newMWPieces;
       // mBPieces = newbMPieces;
    /*  pointThreat = 50;
pointAttacked = 80;
pointHide = 70;
pointHighThreat = 60;
pointSquareHide = 0;
pointCorner = 0;
pointHighAlert = 0;
*/

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

    public override void FindMoves_new(Vector2Int currentPos, Vector2Int targetPos)
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
                if (ValidateCell(j, i) == CellState.Enemy)
                {
                    if (j == targetPos.x + 1 && i == targetPos.y)
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
                        attackedCells.Add(new Vector2Int(targetX, targetY));
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
                        attackedCells.Add(new Vector2Int(targetX, targetY));

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
                    if (j == targetPos.x - 1 && i < targetPos.y - 1 && m.highAlertRight)
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
                        attackedCells.Add(new Vector2Int(targetX, targetY));

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
                        attackedCells.Add(new Vector2Int(targetX, targetY));

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
                    if (j == targetPos.x - 1 && i > targetPos.y + 1)
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
                    if (i == targetPos.y - 1 && j == targetPos.x + 1 && m.highAlertLeft)
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
        m.attacked3 = false;
        m.threaten = false;
        m.hide = false;
        m.removeX = -1;
        m.removeY = -1;
        m.removeX2 = -1;
        m.removeY2 = -1;
        m.removeX3 = -1;
        m.removeY3 = -1;
        m.currentX = currentPos.x;
        m.currentY = currentPos.y;
        m.x = targetPos.x;
        m.y = targetPos.y;

        if (currentColor == Color.black)
        {
            m.player = 2;
            if (mBPieces.ToArray().Length < mWPieces.ToArray().Length)
            {
                m.winning = false;
            }
            else if (mBPieces.ToArray().Length == mWPieces.ToArray().Length)
            {
                m.even = true;
            }
            else
            {
                m.winning = true;
            }
        }
        else
        {
            m.player = 1;
            if (mWPieces.ToArray().Length < mBPieces.ToArray().Length)
            {
                m.winning = false;
            }
            else
            {
                m.winning = true;
            }
        }
        //ANALYZE CURRENTPOS

        if ((currentPos.x == 0 && currentPos.y == 0) || (currentPos.x == 0 && currentPos.y == sizeY - 1)
|| (currentPos.x == sizeX - 1 && currentPos.y == 0) || (currentPos.x == sizeX - 1 && currentPos.y == sizeY - 1))
        {
            m.isInCorner = true;
        }

        if ((ValidateCell(currentPos.x + 1, currentPos.y) == CellState.Friendly && ValidateCell(currentPos.x, currentPos.y + 1) == CellState.Friendly && ValidateCell(currentPos.x+1, currentPos.y+1) == CellState.Free) 
            || (ValidateCell(currentPos.x + 1, currentPos.y) == CellState.Friendly && ValidateCell(currentPos.x + 1, currentPos.y + 1) == CellState.Friendly && ValidateCell(currentPos.x, currentPos.y + 1) == CellState.Free)
            || (ValidateCell(currentPos.x, currentPos.y + 1) == CellState.Friendly && ValidateCell(currentPos.x + 1, currentPos.y + 1) == CellState.Friendly && ValidateCell(currentPos.x + 1, currentPos.y) == CellState.Free)
            || (ValidateCell(currentPos.x - 1, currentPos.y + 1) == CellState.Friendly && ValidateCell(currentPos.x, currentPos.y + 1) == CellState.Friendly && ValidateCell(currentPos.x - 1, currentPos.y) == CellState.Free)
            || (ValidateCell(currentPos.x, currentPos.y + 1) == CellState.Friendly && ValidateCell(currentPos.x - 1, currentPos.y) == CellState.Friendly && ValidateCell(currentPos.x - 1, currentPos.y + 1) == CellState.Free)
            || (ValidateCell(currentPos.x - 1, currentPos.y + 1) == CellState.Friendly && ValidateCell(currentPos.x - 1, currentPos.y) == CellState.Friendly && ValidateCell(currentPos.x, currentPos.y + 1) == CellState.Free)
            || (ValidateCell(currentPos.x - 1, currentPos.y) == CellState.Friendly && ValidateCell(currentPos.x, currentPos.y - 1) == CellState.Friendly && ValidateCell(currentPos.x - 1, currentPos.y - 1) == CellState.Free)
            || (ValidateCell(currentPos.x - 1, currentPos.y - 1) == CellState.Friendly && ValidateCell(currentPos.x, currentPos.y - 1) == CellState.Friendly && ValidateCell(currentPos.x - 1, currentPos.y) == CellState.Free)
            || (ValidateCell(currentPos.x - 1, currentPos.y - 1) == CellState.Friendly && ValidateCell(currentPos.x - 1, currentPos.y) == CellState.Friendly && ValidateCell(currentPos.x, currentPos.y - 1) == CellState.Free)
            || (ValidateCell(currentPos.x, currentPos.y - 1) == CellState.Friendly && ValidateCell(currentPos.x + 1, currentPos.y) == CellState.Friendly && ValidateCell(currentPos.x + 1, currentPos.y - 1) == CellState.Free)
            || (ValidateCell(currentPos.x, currentPos.y - 1) == CellState.Friendly && ValidateCell(currentPos.x + 1, currentPos.y - 1) == CellState.Friendly && ValidateCell(currentPos.x + 1, currentPos.y) == CellState.Free)
            || (ValidateCell(currentPos.x + 1, currentPos.y - 1) == CellState.Friendly && ValidateCell(currentPos.x + 1, currentPos.y) == CellState.Friendly && ValidateCell(currentPos.x, currentPos.y - 1) == CellState.Free))
        {
            m.isPrepSquad = true;
        }

        if ((currentPos.x == 1 && currentPos.y == 0 && ValidateCell(currentPos.x - 1, currentPos.y) == CellState.Friendly) 
            || (currentPos.x == 0 && currentPos.y == 1 && ValidateCell(currentPos.x, currentPos.y - 1) == CellState.Friendly)
            || (currentPos.x == 1 && currentPos.y == sizeY-1 && ValidateCell(currentPos.x - 1, currentPos.y) == CellState.Friendly)
            || (currentPos.x == 0 && currentPos.y == sizeY - 2 && ValidateCell(currentPos.x, currentPos.y + 1) == CellState.Friendly)
            || (currentPos.x == sizeX - 2 && currentPos.y == 0 && ValidateCell(currentPos.x - 1, currentPos.y) == CellState.Friendly)
            || (currentPos.x == sizeX-1 && currentPos.y == 1 && ValidateCell(currentPos.x , currentPos.y + 1) == CellState.Friendly)
            || (currentPos.x == sizeX - 2 && currentPos.y == sizeY - 1 && ValidateCell(currentPos.x + 1, currentPos.y) == CellState.Friendly)
            || (currentPos.x == sizeX - 1 && currentPos.y == sizeY - 2 && ValidateCell(currentPos.x, currentPos.y + 1) == CellState.Friendly))
        {
            m.isOOBAndFriendlyCorner = true;
        }

        if ((ValidateCell(currentPos.x+1,currentPos.y) == CellState.Friendly && ValidateCell(currentPos.x, currentPos.y+1) == CellState.Friendly
            && ValidateCell(currentPos.x+1, currentPos.y+1) == CellState.Friendly)
            || (ValidateCell(currentPos.x - 1, currentPos.y) == CellState.Friendly && ValidateCell(currentPos.x, currentPos.y + 1) == CellState.Friendly
            && ValidateCell(currentPos.x - 1, currentPos.y + 1) == CellState.Friendly)
            || (ValidateCell(currentPos.x - 1, currentPos.y) == CellState.Friendly && ValidateCell(currentPos.x-1, currentPos.y - 1) == CellState.Friendly
            && ValidateCell(currentPos.x , currentPos.y - 1) == CellState.Friendly)
            || (ValidateCell(currentPos.x + 1, currentPos.y) == CellState.Friendly && ValidateCell(currentPos.x, currentPos.y - 1) == CellState.Friendly
            && ValidateCell(currentPos.x + 1, currentPos.y - 1) == CellState.Friendly))
        {
            m.isSquareHide = true;
            m.isPrepSquad = false;
        }


        for (int i = 0; i < sizeX; i++)
        {
            if ((ValidateCell(currentPos.x, currentPos.y + 1) == CellState.Enemy && ValidateCell(i, currentPos.y) == CellState.Enemy)|| 
                (ValidateCell(currentPos.x, currentPos.y - 1) == CellState.Enemy && ValidateCell(i, currentPos.y) == CellState.Enemy))
            {
                m.danger = true;
            }
            if (ValidateCell(i, currentPos.y) == CellState.OutOfBounds || ValidateCell(i, currentPos.y) == CellState.Friendly)
                break;
        }
        for (int i = 0; i >= 0; i--)
        {
            if ((ValidateCell(currentPos.x, currentPos.y + 1) == CellState.Enemy && ValidateCell(i, currentPos.y) == CellState.Enemy) ||
                (ValidateCell(currentPos.x, currentPos.y - 1) == CellState.Enemy && ValidateCell(i, currentPos.y) == CellState.Enemy))
            {
                m.danger = true;
            }
            if (ValidateCell(i, currentPos.y) == CellState.OutOfBounds || ValidateCell(i, currentPos.y) == CellState.Friendly)
                break;
        }

        for (int j = 0; j < sizeY; j++)
        {
            if ((ValidateCell(currentPos.x + 1, currentPos.y) == CellState.Enemy && ValidateCell(currentPos.x, j) == CellState.Enemy) ||
                (ValidateCell(currentPos.x - 1, currentPos.y) == CellState.Enemy && ValidateCell(currentPos.x, j) == CellState.Enemy))
            {
                m.danger = true;
            }
            if (ValidateCell(currentPos.x, j) == CellState.OutOfBounds || ValidateCell(currentPos.x, j) == CellState.Friendly)
                break;
        }
        for (int j = 0; j >= 0; j++)
        {
            if ((ValidateCell(currentPos.x + 1, currentPos.y) == CellState.Enemy && ValidateCell(currentPos.x, j) == CellState.Enemy) ||
                (ValidateCell(currentPos.x - 1, currentPos.y) == CellState.Enemy && ValidateCell(currentPos.x, j) == CellState.Enemy))
            {
                m.danger = true;
            }
            if (ValidateCell(currentPos.x, j) == CellState.OutOfBounds || ValidateCell(currentPos.x, j) == CellState.Friendly)
                break;
        }

        //ANALYZE NEW POSITION


        if ((ValidateCell(currentPos.x + 1, currentPos.y) == CellState.Friendly && ValidateCell(currentPos.x, currentPos.y + 1) == CellState.Friendly && ValidateCell(currentPos.x + 1, currentPos.y + 1) == CellState.Free)
    || (ValidateCell(currentPos.x + 1, currentPos.y) == CellState.Friendly && ValidateCell(currentPos.x + 1, currentPos.y + 1) == CellState.Friendly && ValidateCell(currentPos.x, currentPos.y + 1) == CellState.Free)
    || (ValidateCell(currentPos.x, currentPos.y + 1) == CellState.Friendly && ValidateCell(currentPos.x + 1, currentPos.y + 1) == CellState.Friendly && ValidateCell(currentPos.x + 1, currentPos.y) == CellState.Free)
    || (ValidateCell(currentPos.x - 1, currentPos.y + 1) == CellState.Friendly && ValidateCell(currentPos.x, currentPos.y + 1) == CellState.Friendly && ValidateCell(currentPos.x - 1, currentPos.y) == CellState.Free)
    || (ValidateCell(currentPos.x, currentPos.y + 1) == CellState.Friendly && ValidateCell(currentPos.x - 1, currentPos.y) == CellState.Friendly && ValidateCell(currentPos.x - 1, currentPos.y + 1) == CellState.Free)
    || (ValidateCell(currentPos.x - 1, currentPos.y + 1) == CellState.Friendly && ValidateCell(currentPos.x - 1, currentPos.y) == CellState.Friendly && ValidateCell(currentPos.x, currentPos.y + 1) == CellState.Free)
    || (ValidateCell(currentPos.x - 1, currentPos.y) == CellState.Friendly && ValidateCell(currentPos.x, currentPos.y - 1) == CellState.Friendly && ValidateCell(currentPos.x - 1, currentPos.y - 1) == CellState.Free)
    || (ValidateCell(currentPos.x - 1, currentPos.y - 1) == CellState.Friendly && ValidateCell(currentPos.x, currentPos.y - 1) == CellState.Friendly && ValidateCell(currentPos.x - 1, currentPos.y) == CellState.Free)
    || (ValidateCell(currentPos.x - 1, currentPos.y - 1) == CellState.Friendly && ValidateCell(currentPos.x - 1, currentPos.y) == CellState.Friendly && ValidateCell(currentPos.x, currentPos.y - 1) == CellState.Free)
    || (ValidateCell(currentPos.x, currentPos.y - 1) == CellState.Friendly && ValidateCell(currentPos.x + 1, currentPos.y) == CellState.Friendly && ValidateCell(currentPos.x + 1, currentPos.y - 1) == CellState.Free)
    || (ValidateCell(currentPos.x, currentPos.y - 1) == CellState.Friendly && ValidateCell(currentPos.x + 1, currentPos.y - 1) == CellState.Friendly && ValidateCell(currentPos.x + 1, currentPos.y) == CellState.Free)
    || (ValidateCell(currentPos.x + 1, currentPos.y - 1) == CellState.Friendly && ValidateCell(currentPos.x + 1, currentPos.y) == CellState.Friendly && ValidateCell(currentPos.x, currentPos.y - 1) == CellState.Free))
        {
            m.prepSquad = true;
        }

        if ((ValidateCell(targetPos.x + 1, targetPos.y) == CellState.Friendly && ValidateCell(targetPos.x, targetPos.y + 1) == CellState.Friendly
    && ValidateCell(targetPos.x + 1, targetPos.y + 1) == CellState.Friendly)
    || (ValidateCell(targetPos.x - 1, targetPos.y) == CellState.Friendly && ValidateCell(targetPos.x, targetPos.y + 1) == CellState.Friendly
    && ValidateCell(targetPos.x - 1, targetPos.y + 1) == CellState.Friendly)
    || (ValidateCell(targetPos.x - 1, targetPos.y) == CellState.Friendly && ValidateCell(targetPos.x - 1, targetPos.y - 1) == CellState.Friendly
    && ValidateCell(targetPos.x, targetPos.y - 1) == CellState.Friendly)
    || (ValidateCell(targetPos.x + 1, targetPos.y) == CellState.Friendly && ValidateCell(targetPos.x, targetPos.y - 1) == CellState.Friendly
    && ValidateCell(targetPos.x + 1, targetPos.y - 1) == CellState.Friendly))
        {
            m.squareHide = true;
            m.prepSquad = false;
        }

        if ((targetPos.x == 1 && targetPos.y == 0 && ValidateCell(targetPos.x - 1, targetPos.y) == CellState.Friendly)
    || (targetPos.x == 0 && targetPos.y == 1 && ValidateCell(targetPos.x, targetPos.y - 1) == CellState.Friendly)
    || (targetPos.x == 1 && targetPos.y == sizeY - 1 && ValidateCell(targetPos.x - 1, targetPos.y) == CellState.Friendly)
    || (targetPos.x == 0 && targetPos.y == sizeY - 2 && ValidateCell(targetPos.x, targetPos.y + 1) == CellState.Friendly)
    || (targetPos.x == sizeX - 2 && targetPos.y == 0 && ValidateCell(targetPos.x - 1, targetPos.y) == CellState.Friendly)
    || (targetPos.x == sizeX - 1 && targetPos.y == 1 && ValidateCell(targetPos.x, targetPos.y + 1) == CellState.Friendly)
    || (targetPos.x == sizeX - 2 && targetPos.y == sizeY - 1 && ValidateCell(targetPos.x + 1, targetPos.y) == CellState.Friendly)
    || (targetPos.x == sizeX - 1 && targetPos.y == sizeY - 2 && ValidateCell(targetPos.x, targetPos.y + 1) == CellState.Friendly))
        {
            m.OOBAndFriendlyCorner = true;
        }

        if (ValidateCell(targetPos.x, targetPos.y) == CellState.Free && ((targetPos.x == 0 && targetPos.y == 0) || (targetPos.x == 0 && targetPos.y == sizeY - 1)
   || (targetPos.x == sizeX - 1 && targetPos.y == 0) || (targetPos.x == sizeX - 1 && targetPos.y == sizeY - 1)))
        {
            m.corner = true;
        }

        if (ValidateCell(targetX, targetY) == CellState.Enemy
            && ValidateCell(allyX, allyY) == CellState.Friendly)
        {
            attackedCells.Add(new Vector2Int (targetX, targetY));
            if (m.attacked2 == true && m.attacked == true)
            {
                m.attacked3 = true;
                m.removeX3 = targetX;
                m.removeY3 = targetY;
                m.attackedPiece3 = simpleAllCells[targetX, targetY];

            }
            else if (m.attacked && !m.attacked2)
            {
                m.attacked2 = true;
                m.removeX2 = targetX;
                m.removeY2 = targetY;
                m.attackedPiece2 = simpleAllCells[targetX, targetY];

            }
            else
            {
                m.attacked = true;
                m.removeX = targetX;
                m.removeY = targetY;
                m.attackedPiece = simpleAllCells[targetX, targetY];
            }
        }

        if (ValidateCell(targetX, targetY) == CellState.Enemy
            && ValidateCell(allyX, allyY) != CellState.Friendly
            && ValidateCell(allyX, allyY) != CellState.OutOfBounds)
        {
            m.threaten = true;
            for (int i = currentX; i >= 0; i--)
            {
                if (ValidateCell(i, targetPos.y) == CellState.Friendly || ValidateCell(i, targetPos.y) == CellState.OutOfBounds)
                    break;
                if (ValidateCell(i, targetPos.y) == CellState.Enemy)
                {
                    m.highAlert = true;
                }
                if (ValidateCell(i, targetPos.y + 1) == CellState.Enemy && ValidateCell(targetPos.x, targetPos.y - 1) == CellState.Enemy)
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
                if (ValidateCell(i, targetPos.y - 1) == CellState.Enemy && ValidateCell(targetPos.x, targetPos.y + 1) == CellState.Enemy)
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
                if (ValidateCell(targetPos.x - 1, j) == CellState.Enemy && ValidateCell(targetPos.x + 1, targetPos.y) == CellState.Enemy)
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
                if (ValidateCell(targetPos.x - 1, j) == CellState.Enemy && ValidateCell(targetPos.x + 1, targetPos.y) == CellState.Enemy)
                {
                    m.highAlert = true;
                }
            }
        }

        targetX = targetPos.x - 1;
        targetY = targetPos.y;

        allyX = targetPos.x - 2;
        allyY = targetPos.y;

        if (ValidateCell(targetX, targetY) == CellState.Enemy
&& ValidateCell(allyX, allyY) == CellState.Friendly)
        {
            attackedCells.Add(new Vector2Int(targetX, targetY));
            if (m.attacked2 == true && m.attacked == true)
            {
                m.attacked3 = true;
                m.removeX3 = targetX;
                m.removeY3 = targetY;
                m.attackedPiece3 = simpleAllCells[targetX, targetY];

            }
            else if (m.attacked && !m.attacked2)
            {
                m.attacked2 = true;
                m.removeX2 = targetX;
                m.removeY2 = targetY;
                m.attackedPiece2 = simpleAllCells[targetX, targetY];

            }
            else
            {
                m.attacked = true;
                m.removeX = targetX;
                m.removeY = targetY;
                m.attackedPiece = simpleAllCells[targetX, targetY];

            }

        }

        if (ValidateCell(targetX, targetY) == CellState.Enemy
    && ValidateCell(allyX, allyY) != CellState.Friendly
    && ValidateCell(allyX, allyY) != CellState.OutOfBounds)
        {
            m.threaten = true;
            for (int i = currentX; i >= 0; i--)
            {
                if (ValidateCell(i, targetPos.y) == CellState.Friendly || ValidateCell(i, targetPos.y) == CellState.OutOfBounds)
                    break;
                if (ValidateCell(i, targetPos.y) == CellState.Enemy)
                {
                    m.highAlert = true;
                }
                if (ValidateCell(i, targetPos.y + 1) == CellState.Enemy && ValidateCell(targetPos.x, targetPos.y - 1) == CellState.Enemy)
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
                if (ValidateCell(i, targetPos.y - 1) == CellState.Enemy && ValidateCell(targetPos.x, targetPos.y + 1) == CellState.Enemy)
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
                if (ValidateCell(targetPos.x - 1, j) == CellState.Enemy && ValidateCell(targetPos.x + 1, targetPos.y) == CellState.Enemy)
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
                if (ValidateCell(targetPos.x - 1, j) == CellState.Enemy && ValidateCell(targetPos.x + 1, targetPos.y) == CellState.Enemy)
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
        {
            attackedCells.Add(new Vector2Int(targetX, targetY));
            if (m.attacked2 == true && m.attacked == true)
            {
                m.attacked3 = true;
                m.removeX3 = targetX;
                m.removeY3 = targetY;
                m.attackedPiece3 = simpleAllCells[targetX, targetY];

            }
            else if (m.attacked && !m.attacked2)
            {
                m.attacked2 = true;
                m.removeX2 = targetX;
                m.removeY2 = targetY;
                m.attackedPiece2 = simpleAllCells[targetX, targetY];

            }
            else
            {
                m.attacked = true;
                m.removeX = targetX;
                m.removeY = targetY;
                m.attackedPiece = simpleAllCells[targetX, targetY];

            }
        }

        if (ValidateCell(targetX, targetY) == CellState.Enemy
&& ValidateCell(allyX, allyY) != CellState.Friendly
&& ValidateCell(allyX, allyY) != CellState.OutOfBounds)
        {
            m.threaten = true;
            for (int i = currentX; i >= 0; i--)
            {
                if (ValidateCell(i, targetPos.y) == CellState.Friendly || ValidateCell(i, targetPos.y) == CellState.OutOfBounds)
                    break;
                if (ValidateCell(i, targetPos.y) == CellState.Enemy)
                {
                    m.highAlert = true;
                }
                if (ValidateCell(i, targetPos.y + 1) == CellState.Enemy && ValidateCell(targetPos.x, targetPos.y - 1) == CellState.Enemy)
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
                if (ValidateCell(i, targetPos.y - 1) == CellState.Enemy && ValidateCell(targetPos.x, targetPos.y + 1) == CellState.Enemy)
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
                if (ValidateCell(targetPos.x - 1, j) == CellState.Enemy && ValidateCell(targetPos.x + 1, targetPos.y) == CellState.Enemy)
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
                if (ValidateCell(targetPos.x - 1, j) == CellState.Enemy && ValidateCell(targetPos.x + 1, targetPos.y) == CellState.Enemy)
                {
                    m.highAlert = true;
                }
            }
        }

        targetX = targetPos.x;
        targetY = targetPos.y - 1;

        allyX = targetPos.x;
        allyY = targetPos.y - 2;

        if (ValidateCell(targetX, targetY) == CellState.Enemy
    && ValidateCell(allyX, allyY) == CellState.Friendly)
        {
            attackedCells.Add(new Vector2Int (targetX, targetY));
            if (m.attacked2 == true && m.attacked == true)
            {
                m.attacked3 = true;
                m.removeX3 = targetX;
                m.removeY3 = targetY;
                m.attackedPiece3 = simpleAllCells[targetX, targetY];

            }
            else if (m.attacked && !m.attacked2)
            {
                m.attacked2 = true;
                m.removeX2 = targetX;
                m.removeY2 = targetY;
                m.attackedPiece2 = simpleAllCells[targetX, targetY];

            }
            else
            {
                m.attacked = true;
                m.removeX = targetX;
                m.removeY = targetY;
                m.attackedPiece = simpleAllCells[targetX, targetY];

            }
        }

        if (ValidateCell(targetX, targetY) == CellState.Enemy
&& ValidateCell(allyX, allyY) != CellState.Friendly
&& ValidateCell(allyX, allyY) != CellState.OutOfBounds)
        {
            m.threaten = true;
            for (int i = currentX; i >= 0; i--)
            {
                if (ValidateCell(i, targetPos.y) == CellState.Friendly || ValidateCell(i, targetPos.y) == CellState.OutOfBounds)
                    break;
                if (ValidateCell(i, targetPos.y) == CellState.Enemy)
                {
                    m.highAlert = true;
                }
                if (ValidateCell(i, targetPos.y + 1) == CellState.Enemy && ValidateCell(targetPos.x, targetPos.y - 1) == CellState.Enemy)
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
                if (ValidateCell(i, targetPos.y - 1) == CellState.Enemy && ValidateCell(targetPos.x, targetPos.y + 1) == CellState.Enemy)
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
                if (ValidateCell(targetPos.x - 1, j) == CellState.Enemy && ValidateCell(targetPos.x + 1, targetPos.y) == CellState.Enemy)
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
                if (ValidateCell(targetPos.x-1, j) == CellState.Enemy && ValidateCell(targetPos.x + 1, targetPos.y) == CellState.Enemy)
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
            if (player == 1)
           // if (m.attackedPiece.Contains("W"))
            for (int i = 0; i<mWPieces.ToArray().Length; i++)
            {
                if (m.attackedPiece == mWPieces.ToArray()[i])
                {
                        mWPieces.Remove(mWPieces.ToArray()[i]);
                }
            }
            if (player == 2)
           // if (m.attackedPiece.Contains("B"))
                for (int i = 0; i < mBPieces.ToArray().Length; i++)
                {
                    if (m.attackedPiece == mBPieces.ToArray()[i])
                    {
                        mBPieces.Remove(mBPieces.ToArray()[i]);
                    }
                }
        }
        if (m.attacked2)
        {
            m.attackedPiece2 = simpleAllCells[m.removeX2, m.removeY2];
            simpleAllCells[m.removeX2, m.removeY2] = null;
            if (player == 1)

               // if (m.attackedPiece2.Contains("W"))
                for (int i = 0; i < mWPieces.ToArray().Length; i++)
                {
                    if (m.attackedPiece2 == mWPieces.ToArray()[i])
                    {
                        mWPieces.Remove(mWPieces.ToArray()[i]);
                    }
                }
            if (player == 2)

                //if (m.attackedPiece2.Contains("B"))
                for (int i = 0; i < mBPieces.ToArray().Length; i++)
                {
                    if (m.attackedPiece2 == mBPieces.ToArray()[i])
                    {
                        mBPieces.Remove(mBPieces.ToArray()[i]);
                    }
                }
        }
        if (m.attacked3)
        {
            m.attackedPiece3 = simpleAllCells[m.removeX3, m.removeY3];
            simpleAllCells[m.removeX3, m.removeY3] = null;
            if (player == 1)

              //  if (m.attackedPiece3.Contains("W"))
                for (int i = 0; i < mWPieces.ToArray().Length; i++)
                {
                    if (m.attackedPiece3 == mWPieces.ToArray()[i])
                    {
                        mWPieces.Remove(mWPieces.ToArray()[i]);
                    }
                }
            if (player == 2)

               // if (m.attackedPiece3.Contains("B"))
                for (int i = 0; i < mBPieces.ToArray().Length; i++)
                {
                    if (m.attackedPiece3 == mBPieces.ToArray()[i])
                    {
                        mBPieces.Remove(mBPieces.ToArray()[i]);
                    }
                }
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
            if (lastMove.attackedPiece.Contains("W"))
            {
                mWPieces.Add(lastMove.attackedPiece);
            }
            if (lastMove.attackedPiece.Contains("B"))
            {
                mBPieces.Add(lastMove.attackedPiece);
            }
        }
        if (lastMove.attackedPiece2 != null)
        {
            simpleAllCells[lastMove.removeX2, lastMove.removeY2] = lastMove.attackedPiece2;
            if (lastMove.attackedPiece2.Contains("W"))
            {
                mWPieces.Add(lastMove.attackedPiece2);
            }
            if (lastMove.attackedPiece2.Contains("B"))
            {
                mBPieces.Add(lastMove.attackedPiece2);
            }
        }
        if (lastMove.attackedPiece3 != null)
        {
            simpleAllCells[lastMove.removeX3, lastMove.removeY3] = lastMove.attackedPiece3;
            if (lastMove.attackedPiece3.Contains("W"))
                { 
                mWPieces.Add(lastMove.attackedPiece3);
                }
            if (lastMove.attackedPiece3.Contains("B"))
                {
                 mBPieces.Add(lastMove.attackedPiece3);
                }
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
        int iwas = GetMoves(player).Count;
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

    public float Evaluate(int player, int depth)
    {
        string color = "W";
        if (player == 1)
            color ="B";
       // return Evaluate_new(color);
        return Evaluate(color, depth);

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

    public float Evaluate_new(string color)
    {
        float eval = 1f;


        int rows = sizeX;
        int cols = sizeY;

        if (currentMove.danger)
            eval += dangerPoints;
        if (currentMove.dangerLow)
            eval += dangerLowPoints;
        if (currentMove.squareHide)
            eval += squarePoints; 
        if (currentMove.prepSquareHide)
            eval += prepSquareHidePoints;
        if (currentMove.attacked)
            eval += attackPoints;
        if (currentMove.attacked2)
            eval += attackPoints;
        if (currentMove.highHide)
            eval += pointHighHide;
        if (currentMove.attacked3)
            eval += attackPoints;

       /* if (currentMove.attacked)
            eval += pointAttacked;
        if (currentMove.attacked2)
            eval += pointAttacked;
        if (currentMove.threaten)
            eval += pointThreat;
        if (currentMove.hide)
            eval += pointHide;
        if (currentMove.highThreat)
            eval += pointHighThreat;*/
        if (IsGameOver())
            eval += pointSuccess;
        currentMove.mScore += eval;


        //    }
        //  }
        //  return 1;
        return eval;
    }

    public float Evaluate(string color, int depth)
    {
      float eval = 1f;
        float depthWeight = 1;
        if (depth>0)
            depthWeight = 1 / depth;

    int rows = sizeX;
    int cols = sizeY;
        float dangerMultiplier = 1f;
        float attackWeight = 1f;
        float aggroWeight = 1f;
        float defenseWeight = 1f;
        if (currentMove.even)
        {
            aggroWeight = 2;
        }
        if (currentMove.winning)
        {
            aggroWeight = 3;
        }
        if (!currentMove.winning && !currentMove.even)
            defenseWeight = 2;
        if (currentMove.danger)
        {
            dangerMultiplier = 2f;
        }
        if (currentMove.isInCorner)
        {
            eval += -20;
        }
        if (currentMove.isPrepSquad)
        {
            eval += -10;
        }
        if (currentMove.isInCorner && currentMove.attacked && !currentMove.highAlert)
        {
            attackWeight = 3f;
        }

            if (currentMove.isOOBAndFriendlyCorner)
        {
            eval += -10;
        }
        else if ((currentMove.isOOBAndFriendlyCorner || currentMove.isSquareHide) && currentMove.threaten && !currentMove.attacked)
        {
            attackWeight = 2f;
        }
        else if ((currentMove.isOOBAndFriendlyCorner || currentMove.isSquareHide) && currentMove.threaten && !currentMove.attacked)
        {
            attackWeight = 1.25f;
        }
        else if (currentMove.isOOBAndFriendlyCorner && currentMove.attacked && !currentMove.highAlert)
        {
            attackWeight = 3f;
        }
        else if (currentMove.isOOBAndFriendlyCorner && currentMove.attacked && currentMove.highAlert)
        {
            attackWeight = 3f;
        }
            else if (currentMove.attacked)
        {
            attackWeight = 2f;
        }

        //attackWeight = 1;
        // dangerMultiplier = 1;
        /*  pointThreat = 50;
          pointAttacked = 80;
          pointHide = 70;
          pointHighThreat = 60;
          pointSquareHide = 0;
          pointCorner = 0;
          pointHighAlert = 0;
        */

       /* pointSuccess = 250f; //Defensiv
        pointAttacked = 100f;
        pointThreat = 10f;
        pointHide = 20f;
        pointHighThreat = 60f;
        pointSquareHide = 40f;
        pointHighAlert = -100f;
        pointCorner = 100f;
        pointPrepSquad = 20f;*/

       /*  pointSuccess = 250f;
         pointAttacked = 50f;
         pointThreat = 50f;
         pointHide = 50f; //BOTH
         pointHighThreat = 50f;
         pointSquareHide = 40f;
         pointHighAlert = -50f;
         pointCorner = 50f;
         pointPrepSquad = 50f;*/

   /*      pointSuccess = 250f;
 pointAttacked = 100f;
 pointThreat = 70f;
 pointHide = 10f; //Aggro
 pointHighThreat = 80f;
 pointSquareHide = 20f;
 pointHighAlert = -100f;
 pointCorner = 10f;
 pointPrepSquad = 10f;*/

        if (currentMove.attacked)
            eval += pointAttacked * attackWeight * aggroWeight;
        if (currentMove.attacked2)
            eval += pointAttacked * attackWeight * aggroWeight; 
        if (currentMove.attacked2)
            eval += pointAttacked * attackWeight * aggroWeight;// * dangerMultiplier;
        if (currentMove.threaten)
            eval += pointThreat * attackWeight * aggroWeight;// * dangerMultiplier;
        if (currentMove.hide)
            eval += pointHide * dangerMultiplier * defenseWeight;
        if (currentMove.squareHide)
            eval += pointSquareHide * dangerMultiplier * defenseWeight;
        if (currentMove.prepSquad)
            eval += pointPrepSquad * dangerMultiplier * defenseWeight;
        if (currentMove.corner)
            eval += pointCorner * dangerMultiplier * defenseWeight;
        if (currentMove.OOBAndFriendlyCorner)
            eval += pointOOBAndCorner * dangerMultiplier * defenseWeight;
            if (currentMove.highAlert)
            eval += pointHighAlert;
        //        if (eval == 1f)
        //           eval += pointSimple;
        //if (currentMove.success)
        //  eval += pointSuccess;
        if (currentMove.highThreat)
                        eval += pointHighThreat;
                    if (IsGameOver())
                        eval += pointSuccess;
        eval *= depthWeight;
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
