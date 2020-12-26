using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

    public int sizeX;
    public int sizeY;
   // public GameObject wPrefab;
   // public GameObject bPrefab;
    public Board mBoard;
    public PieceManager mPieceManager;
    public AIManager mAIManager;
    public string mAIType;
    public int maxDepth;
    public int AIActive;
    public Slider attackSlider;
    public Slider threatSlider;
    public Slider hideSlider;
    private Task t;
    private bool moving = false;
    //public bool secondAI;
    public Toggle secondAIActive;
    public Toggle firstAIActive;

    /* private float pointAttacked = 100f;
     private float pointThreat = 50f;
     private float pointHide = -2f;*/
    private float pointAttacked = 100f;
    private float pointThreat = 50f;
    private float pointHide = 20f;
    private float pointHighThreat = 0f;

    public Dropdown DDX;
    public Dropdown DDY;

    public InputField InputX;
    public InputField InputY;
    public TextMeshPro winText;
    public TextMesh loseText;
    public Text WinText;

    private float pointAttacked2 = 80f;
    private float pointThreat2 = 50f;
    private float pointHide2 = 20f;
    private float pointHighThreat2 = 0f;
    public EscapeMenuActivator EscapeMenu;

    private bool gameRunning;// = false;
                             // private int maxRounds = 10;
                             //  private int currentRound = 1; ?
    private int maxPlayers;// = 2;
    protected int currentPlayer = 0;
    string winner;

    private bool moveAgain;
    Vector2 initialVectorBottomLeft;
    Vector2 initialVectorTopRight;

    Vector2 updatedVectorBottomLeft;
    Vector2 updatedVectorTopRight;


    // Start is called before the first frame update
    void Start()
    {
 
        Camera.main.gameObject.SetActive(true);



        //Values for RESIZE
        initialVectorBottomLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        initialVectorTopRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        //Create Board

        //  mBoard.Create();



        //Create Pieces
        //  mPieceManager.Setup(mBoard, "iwas");

        GameObject.FindGameObjectWithTag("StartButton").SetActive(true);



    }



    void Update()
    {
        updatedVectorBottomLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        updatedVectorTopRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        if ((initialVectorBottomLeft != updatedVectorBottomLeft) || (initialVectorTopRight != updatedVectorTopRight))
        {
            EscapeMenu.resizeUI();
            if (mBoard.mAllCells != null)
            {
                mBoard.resizeBoard();
                mPieceManager.resizePieces();
               // EscapeMenu.resizeUI();
            }
            //EscapeMenuActivator.resizeUI();
            initialVectorBottomLeft = updatedVectorBottomLeft;
            initialVectorTopRight = updatedVectorTopRight;
        }
    }

    public void StartGame()
    {
        GameObject.FindGameObjectWithTag("StartButton").SetActive(false);
        GameObject.FindGameObjectWithTag("DDX").SetActive(false);
        GameObject.FindGameObjectWithTag("DDY").SetActive(false);

        if (firstAIActive.isOn == false)
        {
            //secondAIActive. = false;

            secondAIActive.interactable = false;
        }
        if (secondAIActive.isActiveAndEnabled)
        {
            AIActive = 2;
        }
        else if (firstAIActive.isOn)
            AIActive = 1;
        else 
            AIActive = 0;

        secondAIActive.interactable = false;
        firstAIActive.interactable = false;
        switch (DDX.value)
        {
            case 0:
                sizeX = 6;
                break;
            case 1:
                sizeX = 7;
                break;
            case 2:
                sizeX = 8;
                break;
            case 3:
                sizeX = 9;
                break;
            case 4:
                sizeX = 10;
                break;
            case 5:
                sizeX = 11;
                break;
            case 6:
                sizeX = 12;
                break;
        }
        switch (DDY.value)
        {
            case 0:
                sizeY = 6;
                break;
            case 1:
                sizeY = 7;
                break;
            case 2:
                sizeY = 8;
                break;
            case 3:
                sizeY = 9;
                break;
            case 4:
                sizeY = 10;
                break;
            case 5:
                sizeY = 11;
                break;
            case 6:
                sizeY = 12;
                break;
        }

        /*  if (InputX.text == "")
              sizeX = 8;

          else
              sizeX = System.Int32.Parse(InputX.text);

          if (InputY.text == "")
              sizeY = 8;
          else
              sizeY = System.Int32.Parse(InputY.text);

          */
        mBoard.Create(sizeX, sizeY);



        //Create Pieces
        mPieceManager.Setup(mBoard, "iwas");
        if (firstAIActive.isOn == false && secondAIActive.isOn == false)
        {
            mPieceManager.isAIActive = false;
        }
        mPieceManager.SwitchSides(Color.black);
    }

    public float getPoints(string type)
    {
        if (type=="attack")
        {
            return pointAttacked;
        }
        else if (type =="threat")
        {
            return pointThreat;
        }
        else if (type =="hide")
        {
            return pointHide;
        }
        else if (type == "attack2")

            {
                return pointAttacked2;
            }
            else if (type == "threat2")
            {
                return pointThreat2;
            }
            else if (type == "hide2")
            {
                return pointHide2;
            }
        else
        return 0;
    }

    public void ChangeAttackPoints(float newPoints)
    {
        pointAttacked = newPoints;
    }
    public void ChangeHidePoints(float newPoints)
    {
        pointHide = newPoints;
    }
    public void ChangeThreatPoints(float newPoints)
    {
        pointThreat = newPoints;
    }
    public void ChangeAttackPoints2(float newPoints)
    {
        pointAttacked2 = newPoints;
    }
    public void ChangeHidePoints2(float newPoints)
    {
        pointHide2 = newPoints;
    }
    public void ChangeThreatPoints2(float newPoints)
    {
        pointThreat2 = newPoints;
    }

    public void MovePiece()
    {
        if (currentPlayer == 2 && firstAIActive.isOn)
        {
            List<Move> moves = new List<Move>();
            mPieceManager.setBoard(mBoard);
            string[,] boarddraught = mBoard.getDraughtAsStrings();
            BoardDraught b = new BoardDraught(boarddraught, currentPlayer, mBoard.sizeX, mBoard.sizeY, pointAttacked, pointHide, pointThreat, 80);
            float test;
            Move currentMove = new Move();

            test = AIManager.Minimax(b, currentPlayer, maxDepth, 0, ref currentMove);

            // StartCoroutine(ExecuteMoveAfterDelay(1));
            List<Piece> blist = mPieceManager.getBPieces();
            for (int i = 0; i < blist.ToArray().Length; i++)
            {
                if (blist.ToArray()[i].name == currentMove.mPieceName)
                {
                    blist.ToArray()[i].CheckPath(currentMove);
                    StartCoroutine(ExecuteMoveAfterDelay((float)1.5f, i, currentMove));
                    break;
                }
            }

        }
        else if (currentPlayer == 1 && secondAIActive.isOn)
        {
            List<Move> moves = new List<Move>();
            mPieceManager.setBoard(mBoard);
            string[,] boarddraught = mBoard.getDraughtAsStrings();
            BoardDraught b = new BoardDraught(boarddraught, currentPlayer, mBoard.sizeX, mBoard.sizeY, pointAttacked2, pointHide2, pointThreat2, pointHighThreat2);
            float test;
            Move currentMove = new Move();

            test = AIManager.Minimax(b, currentPlayer, maxDepth, 0, ref currentMove);

            List<Piece> wlist = mPieceManager.getWPieces();
            for (int i = 0; i < wlist.ToArray().Length; i++)
            {
                if (wlist.ToArray()[i].name == currentMove.mPieceName)
                {
                    wlist.ToArray()[i].CheckPath(currentMove);
                    StartCoroutine(ExecuteMoveAfterDelay((float)1.5f, i, currentMove));
                    break;
                }
            }
        }
    }

    /*IEnumerator Move ()
    {
        yield return new WaitForSeconds(5f);
        
        if (currentPlayer == 2)
        {
            List<Move> moves = new List<Move>();
            mPieceManager.setBoard(mBoard);
            string[,] boarddraught = mBoard.getDraughtAsStrings();
            BoardDraught b = new BoardDraught(boarddraught, currentPlayer, mBoard.sizeX, mBoard.sizeY, pointAttacked, pointHide, pointThreat, 0);
            float test;
            Move currentMove = new Move();

            test = AIManager.Minimax(b, currentPlayer, maxDepth, 0, ref currentMove);

            //  Debug.Log("Moves Again: " + m.mPiece.name + " CurrentCell: " + m.mPiece.getCurrentCell().name + " TargetCell: " + mBoard.mAllCells[m.x, m.y].name);//.mPiece.getTargetCell().name);
            // StartCoroutine(ExecuteMoveAfterDelay(1));
            List<Piece> blist = mPieceManager.getBPieces();
            for (int i = 0; i < blist.ToArray().Length; i++)
            {
                if (blist.ToArray()[i].name == currentMove.mPieceName)
                {
                    blist.ToArray()[i].CheckPath(currentMove);
                    blist.ToArray()[i].ShowCells();
                    //StartCoroutine(ExecuteHighlightAfterDelay(1, i, currentMove));

                    StartCoroutine(ExecuteMoveAfterDelay((float)1.5, i, currentMove));

                    break;
                }
            }
            yield return new WaitForSeconds(5f);

        }
        else if (currentPlayer == 1 && AIActive == 2)
        {
            List<Move> moves = new List<Move>();
            mPieceManager.setBoard(mBoard);
            string[,] boarddraught = mBoard.getDraughtAsStrings();
            BoardDraught b = new BoardDraught(boarddraught, currentPlayer, mBoard.sizeX, mBoard.sizeY, pointAttacked, pointHide, pointThreat, pointHighThreat2);
            float test;
            Move currentMove = new Move();

            test = AIManager.Minimax(b, currentPlayer, maxDepth, 0, ref currentMove);

            //  Debug.Log("Moves Again: " + m.mPiece.name + " CurrentCell: " + m.mPiece.getCurrentCell().name + " TargetCell: " + mBoard.mAllCells[m.x, m.y].name);//.mPiece.getTargetCell().name);
            // StartCoroutine(ExecuteMoveAfterDelay(1));
            List<Piece> wlist = mPieceManager.getWPieces();
            for (int i = 0; i < wlist.ToArray().Length; i++)
            {
                if (wlist.ToArray()[i].name == currentMove.mPieceName)
                {
                    wlist.ToArray()[i].CheckPath(currentMove);
                    wlist.ToArray()[i].ShowCells();
                    //StartCoroutine(ExecuteHighlightAfterDelay(1, i, currentMove));

                    StartCoroutine(ExecuteMoveAfterDelay((float)1.5, i, currentMove));

                    break;
                }
            }
            yield return new WaitForSeconds(5f);

        }
    }*/

    IEnumerator ExecuteHighlightAfterDelay(float time, int idx, Move currentMove)
    {
        yield return new WaitForSeconds(time);
        List<Piece> blist = mPieceManager.getBPieces();

        blist.ToArray()[idx].ShowCells();


    }


    public void MoveAgain ()
    {
        if (GameOver() != null)
        {
            if (GameOver() == "Player 1" && AIActive != 2)
            {
                Debug.Log(GameOver() + "wins.");
               // GameObject.FindGameObjectWithTag("WinTag").GetComponent<TMPro.TextMeshProUGUI>().text = "YOU WIN!";

            }
            else if (GameOver() == "Player 2" && AIActive != 2)
            {
              //  GameObject.FindGameObjectWithTag("WinTag").GetComponent<TMPro.TextMeshProUGUI>().text = "YOU LOSE!";

                Debug.Log(GameOver() + "wins.");
            }
        }
        if (currentPlayer == 2 && firstAIActive.isOn)
        {
           // StartCoroutine(Move());
             MovePiece();
    }
        else if (currentPlayer == 1 && secondAIActive.isOn)//AIActive == 2)
        {
            //StartCoroutine(Move());

            MovePiece();
        }
    }

    IEnumerator ExecuteMoveAfterDelay(float time, int idx, Move currentMove)
    {
        //Print the time of when the function is first called.
        //Debug.Log("Started Coroutine at timestamp : " + Time.time);
        while (moving)
        {
            yield return new WaitForSeconds(time);
        }
        moving = true;
        //yield on a new YieldInstruction that waits for 5 seconds.
        // MovePiece();
        if (currentPlayer == 1 & secondAIActive.isOn)
        {

            List<Piece> wlist = mPieceManager.getWPieces();
            wlist.ToArray()[idx].ShowCells();

            yield return new WaitForSeconds(time);

            wlist.ToArray()[idx].PlaceByAI(mBoard.mAllCells[currentMove.x, currentMove.y]);

            wlist.ToArray()[idx].ClearCells();
            moving = false;

        }
        else if (currentPlayer == 2 && firstAIActive.isOn)
        {
            List<Piece> blist = mPieceManager.getBPieces();
            blist.ToArray()[idx].ShowCells();

            yield return new WaitForSeconds(time);

            blist.ToArray()[idx].PlaceByAI(mBoard.mAllCells[currentMove.x, currentMove.y]);

            blist.ToArray()[idx].ClearCells();

            moving = false;
        }

    }
    public void nextPlayer()
    {
        if (GameOver() != null && AIActive ==2)
        {
            mPieceManager.ResetPieces(true);
        }

        else if (GameOver() != null && AIActive != 2)
        {
            Debug.Log(GameOver() + "wins.");
            
           /* if (GameOver() == "Player 1")
                GameObject.FindGameObjectWithTag("WinTag").GetComponent<TMPro.TextMeshProUGUI>().text = "YOU WIN!";
            else if (GameOver() == "Player 2")
                GameObject.FindGameObjectWithTag("WinTag").GetComponent<TMPro.TextMeshProUGUI>().text = "YOU LOSE!";
            //SceneManager.LoadScene(0);*/
        }
        else
        {
            if (currentPlayer == 0)
            {
                currentPlayer = 1;
                if (AIActive == 2)
                {
                    MovePiece();
                }
            }
            else if (currentPlayer == 1 && firstAIActive.isOn)
            {
                currentPlayer = 2;
                MovePiece();

            }else if (currentPlayer == 2 && secondAIActive.isOn)//AIActive == 2)
            {
                currentPlayer = 1;
                MovePiece();

            }
            else
                currentPlayer = 1;
        }
    }
    public int getCurrentPlayer ()
    {
        return currentPlayer;
    }

    public void MoveAgain(int currentPlayer)
    {
        if (currentPlayer == 2)
            this.currentPlayer = 1;
        else if (currentPlayer == 1)
            this.currentPlayer = 2;
    }

    public string GameOver()
    {

        if (/*mPieceManager.getBPieces().ToArray().Length < 2 &&*/ mBoard.IsGameOver() == "Player 1")
            return "Player 1";
        else if (/*mPieceManager.getWPieces().ToArray().Length < 2 &&*/ mBoard.IsGameOver() == "Player 2")
            return "Player 2";
        else if (mBoard.GetMoves(1).ToArray().Length == 0)
        {
            return "Player 2";
        }
        else if (mBoard.GetMoves(2).ToArray().Length == 0) 
         { 
                return "Player 1";
        }
        else
            return null;
        
    }

}
