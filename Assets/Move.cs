using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{

    //public string mMove;
    //public Vector2 mTargetLoc;
    public float mScore;
    //public Piece mPiece;
    public string mPieceName;
    //_______________________________________


    public int x; // x & y as TargetLocation
    public int y;
    public bool success;
    public bool threaten;
    public bool hide; //??
    public bool highThreat;
    public bool attacked;
    public bool attacked2;
    public bool moveAgain;


    public int removeX;
    public int removeY;
    public int removeX2;
    public int removeY2;

    public int currentX; // x & y as CurrentLocation
    public int currentY;
    public int player; //Player

    public string attackedPiece;
    public string attackedPiece2;

}
