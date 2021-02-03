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
    public bool threaten;
    public bool hide; //??
    public bool highHide;

    public bool highHideRight;
    public bool highHideLeft;
    public bool highHideUp;
    public bool highHideDown;
    public bool squareHide;
    public bool prepSquareHide;
    public bool prepSquareHideUpMiss;
    public bool prepSquareHideDownMiss;

    public bool dangerLow;
    public bool danger;


    public bool highThreat;
    public bool attacked;
    public bool attacked2;
    public bool attacked3;

    public bool moveAgain;
    public bool highAlert;

    public bool threatRight;
    public bool threatLeft;
    public bool threatUp;
    public bool threatDown;

    public bool highThreatUp;
    public bool highThreatDown;
    public bool highThreatRight;
    public bool highThreatLeft;

    public bool highAlertRight;
    public bool highAlertLeft;
    public bool highAlertUp;
    public bool highAlertDown;

    public bool alertRight;
    public bool alertLeft;
    public bool alertUp;
    public bool alertDown;
    public bool corner;
    public bool isInCorner;
    public bool isSquareHide;
    public bool isPrepSquad;
    public bool isOOBAndFriendlyCorner;
    public bool OOBAndFriendlyCorner;
    public bool prepSquad;

    public int removeX;
    public int removeY;
    public int removeX2;
    public int removeY2; 
    public int removeX3;
    public int removeY3;

    public int currentX; // x & y as CurrentLocation
    public int currentY;
    public int player; //Player

    public string attackedPiece;
    public string attackedPiece2;

}
