﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;


public class PieceManager : MonoBehaviour
{
    public GameObject mPiecePrefab;
    public bool moveAgain = false;
    public bool isAIActive = true;


    private List<Piece> mWPieces = null;
    private List<Piece> mBPieces = null;
    private List<Piece> mAllPieces = null;
    private Board board;


   // private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

    public List<Piece> getBPieces()
    {
        return mBPieces;
    }

    public List<Piece> getWPieces()
    {
        return mWPieces;
    }

    public Board getBoard()
    {
        return board;
    }
    public string[,] getBoardDraught2()
    {

        return board.getDraughtAsStrings();
    }
    public BoardDraught getBoardDraught()
    {
        
        return board.getDraught();
    }

    public void setBoard(Board newBoard)
    {
        board = newBoard;
    }

    public void Setup(Board board2, string aiType)
    {
        this.board = board2;

        isAIActive = aiType != null ? true : false;

        //Create White Pieces
        mWPieces = CreatePieces(Color.white, new Color32(100, 100, 100, 255), board);        
        //Create Black Pieces
        mBPieces = CreatePieces(Color.black, new Color32(0, 0, 0, 255), board);


        PlacePieces(0, mWPieces, board);

        PlacePieces(board.sizeY - 1, mBPieces, board);

        //White starts
    }


    void Update()
    {

    }

    internal void resizePieces()
    {
        List<Piece> newPieces = new List<Piece>();


        float height = (Screen.height / 4) * 3;
        float width = Screen.width;
        float sizePieceY;
        if (board.sizeY < board.sizeX)
        {
            sizePieceY = height / board.sizeX;

        }
        else
        {
            sizePieceY = height / board.sizeY;
        }
        float offsetY = (Screen.height - height) / 2;
        float offsetX = (Screen.width - (board.sizeX * sizePieceY)) / 2;
        float sizePieceX = sizePieceY;
        for (int i = 0; i < getBPieces().ToArray().Length; i++)
        {
            //new Object
            GameObject newPieceObject = getBPieces().ToArray()[i].gameObject;

            RectTransform rectTransform = newPieceObject.GetComponent<RectTransform>();

            rectTransform.sizeDelta = new Vector2(sizePieceX, sizePieceX);
            Vector3 currentCellPos = getBPieces().ToArray()[i].getCurrentCell().GetComponent<RectTransform>().anchoredPosition;
            rectTransform.anchoredPosition = new Vector3(currentCellPos.x, currentCellPos.y, 0);

        }
        for (int i = 0; i < getWPieces().ToArray().Length; i++)
        {
            //new Object
            GameObject newPieceObject = getWPieces().ToArray()[i].gameObject;

            RectTransform rectTransform = newPieceObject.GetComponent<RectTransform>();

            rectTransform.sizeDelta = new Vector2(sizePieceX, sizePieceX);
            Vector3 currentCellPos = getWPieces().ToArray()[i].getCurrentCell().GetComponent<RectTransform>().anchoredPosition;
            rectTransform.anchoredPosition = new Vector3(currentCellPos.x, currentCellPos.y, 0);
        }
    }

    private List<Piece> CreatePieces(Color teamColor, Color32 spriteColor, Board board)
    {
        List<Piece> newPieces = new List<Piece>();


        float height = (Screen.height / 4) * 3;
        float width = Screen.width;
        float midHeight = height / 2;
        float midWidth = width / 2;
        float sizePieceY;
        if (board.sizeY < board.sizeX)
        {
            sizePieceY = height / board.sizeX;

        }
        else
        {
            sizePieceY = height / board.sizeY;
        }
        float offsetY = (Screen.height - height) / 2;
        float offsetX = (Screen.width - (board.sizeX * sizePieceY)) / 2;
        float sizePieceX = sizePieceY;

        for (int i = 0; i < board.sizeX; i++)
        {
            //new Object
            GameObject newPieceObject = Instantiate(mPiecePrefab);
            newPieceObject.transform.SetParent(this.transform);
            string currentColor;
            if (teamColor == Color.white)
                currentColor = "White";
            else
                currentColor = "Black";
            newPieceObject.name = "Piece" + currentColor + i;

            //Scale&Position
            newPieceObject.transform.localScale = new Vector3(1, 1, 1);
            newPieceObject.transform.localRotation = Quaternion.identity;
            RectTransform rectTransform = newPieceObject.GetComponent<RectTransform>();

            rectTransform.sizeDelta = new Vector2(sizePieceX, sizePieceX);

            rectTransform.anchoredPosition = new Vector2((i * sizePieceX) + offsetX, (i * sizePieceX) + offsetY);

            //Store piece
            Piece newPiece = (Piece)newPieceObject.AddComponent(typeof(SimplePiece));
            newPieces.Add(newPiece);

            //Setup Piece
            newPiece.Setup(teamColor, spriteColor, this, new Vector3Int(board.sizeX, board.sizeY, 1));
        }
        return newPieces;

    }
    
    private void PlacePieces(int initRow, List<Piece> pieces, Board board)
    {
        for (int i = 0; i<board.sizeX; i++)
        {
            pieces[i].Place(board.mAllCells[i, initRow]);
            board.simpleAllCells[i, initRow] = pieces[i].name;
        }
    }

    private void SetInteractive(List<Piece> allPieces, bool value)
    {
        foreach (Piece piece in allPieces)
            piece.enabled = value;
    }

 

    private void DisableAllPieces()
    {

        SetInteractive(mWPieces, false);
        SetInteractive(mBPieces, false);

    }

    private void EnableAllPieces()
    {
        foreach (Piece piece in mAllPieces)
            piece.enabled = true;
    }
    
    private bool GameOver(Color color)
    {

        board.simpleAllCells = board.getDraughtAsStrings();
        if (board.IsGameOver() == "Player 1" || board.IsGameOver() == "Player 2")
            return true;
        else 
            return false;
    }

    IEnumerator RemoveWinMessageAfterDelay()
    {
        yield return new WaitForSeconds(5);
        GameObject.FindGameObjectWithTag("WinTag").GetComponent<TMPro.TextMeshProUGUI>().text = "";

    }

    public void activateALL()
    {
        SetInteractive(mWPieces, true);
        SetInteractive(mBPieces, true);
    }

    public void SwitchSides(Color color)
    {
        Piece[] wPieces = mWPieces.ToArray();
        Piece[] bPieces = mBPieces.ToArray();
        int wDeathCounter = 0;
        int bDeathCounter = 0;
        GameObject referenceObject;
        GameManager referenceScript;
        referenceObject = GameObject.FindGameObjectWithTag("GameManager");
        referenceScript = referenceObject.GetComponent<GameManager>();
        if (GameOver(color))
        {
            if (color == Color.black)
            {       if (moveAgain)
                {
                    Debug.Log("White wins. No Moves left.");
                    GameObject.FindGameObjectWithTag("WinTag").GetComponent<TMPro.TextMeshProUGUI>().text = "MUSCHELN GEWINNEN!";
                    StartCoroutine(RemoveWinMessageAfterDelay());

                    if (referenceScript.firstAIActive.isOn && referenceScript.secondAIActive.isOn)
                        StartCoroutine(RemoveWinMessageAfterDelay());
                }

                else
                {
                    Debug.Log("Black wins. No Moves left");
                    GameObject.FindGameObjectWithTag("WinTag").GetComponent<TMPro.TextMeshProUGUI>().text = "STEINE GEWINNEN!";
                    StartCoroutine(RemoveWinMessageAfterDelay());

                    if (referenceScript.firstAIActive.isOn && referenceScript.secondAIActive.isOn)
                    StartCoroutine(RemoveWinMessageAfterDelay());

                }
            }
            else
            {
                if (moveAgain)
                {
                    Debug.Log("Black wins. No Moves left");
                    GameObject.FindGameObjectWithTag("WinTag").GetComponent<TMPro.TextMeshProUGUI>().text = "STEINE GEWINNEN!";
                    StartCoroutine(RemoveWinMessageAfterDelay());

                    if (referenceScript.firstAIActive.isOn && referenceScript.secondAIActive.isOn)
                        StartCoroutine(RemoveWinMessageAfterDelay());
                }
                else
                {
                    Debug.Log("White wins. No Moves left.");
                    GameObject.FindGameObjectWithTag("WinTag").GetComponent<TMPro.TextMeshProUGUI>().text = "MUSCHELN GEWINNEN!";
                    StartCoroutine(RemoveWinMessageAfterDelay());

                    if (referenceScript.firstAIActive.isOn && referenceScript.secondAIActive.isOn)
                        StartCoroutine(RemoveWinMessageAfterDelay());

                }
            }
            ResetPieces(false);
        }
        else {
            for (int i = 0; i < wPieces.Length; i++)
            {
                if (wPieces[i].isDead())
                {
                    wDeathCounter++;
                    if (wDeathCounter == wPieces.Length - 1)
                    {
                        Debug.Log("Black wins.");

                        ResetPieces(false);
                    }
                }
                if (bPieces[i].isDead())
                {
                    bDeathCounter++;
                    if (bDeathCounter == bPieces.Length - 1)
                    {
                        Debug.Log("White wins.");

                        ResetPieces(false);
                    }
                }
            }
        }

        bool isBlackTurn = color == Color.white ? true : false;
        if (isAIActive && isBlackTurn)
        {

            if (referenceScript.getCurrentPlayer() == 1 && (referenceScript.firstAIActive.isOn))// || referenceScript.secondAIActive.isOn))
            {
                DisableAllPieces();
            }
            else if (referenceScript.getCurrentPlayer() == 2)
            {

            }

        }
        else if (referenceScript.secondAIActive.isOn)
            DisableAllPieces();
        else
        {
            SetInteractive(mWPieces, !isBlackTurn);
            SetInteractive(mBPieces, isBlackTurn);
        }
        if (!moveAgain)
        {
            NextPlayer();
            getBoard().NextPlayer();
        }
        else if (moveAgain)
        {
            moveAgain = false;

            referenceScript.MoveAgain();
        }
        if (board.GetCurrentPlayer() == 2)
            GameObject.FindGameObjectWithTag("PlayerTag").GetComponent<TMPro.TextMeshProUGUI>().text = "Muscheln sind am Zug!";
        else
            GameObject.FindGameObjectWithTag("PlayerTag").GetComponent<TMPro.TextMeshProUGUI>().text = "Steine sind am Zug!";
     
    }

    private void NextPlayer()
    {
        GameObject referenceObject;
        GameManager referenceScript;
        referenceObject = GameObject.FindGameObjectWithTag("GameManager");
        referenceScript = referenceObject.GetComponent<GameManager>();
            
        referenceScript.nextPlayer();

    }

    public void ResetPieces(bool startAgain)
    {
        //Reset White
        foreach (Piece piece in mWPieces)

        piece.Reset();

        foreach(Piece piece in mBPieces)
        {
            piece.Reset();
        }
        moveAgain = false;
        if (startAgain)
        {
            SwitchSides(Color.black);
        }
    }

    public Cell GetCellByPos(Vector3 pos)
    {
        for (int i = 0; i < board.sizeX; i++)
        {
            for (int j = 0; j < board.sizeY; j++)
            {
                if (board.mAllCells[i,j].transform.localPosition.x == pos.x && board.mAllCells[i, j].transform.localPosition.x == pos.y)
                {
                    return board.mAllCells[i, j];
                }
            }
        }
        return new Cell();
    }

    public void removePiece (Piece piece)
    {
        if (piece.mColor == Color.white)
        {
            for (int i = 0; i < mWPieces.ToArray().Length; i++)
            {
                if (mWPieces.ToArray()[i].name == piece.name)
                {
                    mWPieces.Remove(piece);
                }
            }
        }
        if (piece.mColor == Color.black)
        {
            for (int i = 0; i < mBPieces.ToArray().Length; i++)
            {
                if (mBPieces.ToArray()[i].name == piece.name)
                {
                    mBPieces.Remove(piece);
                }
            }
        }
    }
}
