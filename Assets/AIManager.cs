using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    // Start is called before the first frame update

public static float Minimax(
            BoardDraught board,
            int player,
            int maxDepth,
            int currentDepth,

            ref Move bestMove)
    {
        if (board.IsGameOver() || currentDepth == maxDepth)
        {
            board.SetCurrentMove(bestMove);
            return board.Evaluate(player);//board.Evaluate(player);
        }

        bestMove = null;
        float bestScore = Mathf.Infinity;
        if (board.GetCurrentPlayer() == player)
            bestScore = Mathf.NegativeInfinity;
         List<Move> allMoves = new List<Move>();
        int nextPlayer = 0;
        
        if (player == 2)
        {
            allMoves = board.GetMoves(player);
            nextPlayer = 1;
        }
        else if (player == 1)
        {
            allMoves = board.GetMoves(player);
            nextPlayer = 2;
        }
        BoardDraught bTest = board;
        Move currentMove;
        if (currentDepth == 0)
        {
           float maxScore = 0;
        }
        foreach (Move m in allMoves)//board.GetMoves())
        {
            bTest = board.MakeMove(m);
            float currentScore;
            //Evaluate Moves
            currentMove = m;// null;//= m;//m; ??
            if (m.attacked)
            {
                if (nextPlayer == 2)
                    nextPlayer = 1;
                else
                    nextPlayer = 2;
            } 
            currentScore = Minimax(bTest, nextPlayer, maxDepth, currentDepth + 1, ref currentMove);

            board.SetCurrentMove(m);
            float newScore = board.Evaluate(player);
            //Evaluierung aktueller Move
            
            //currentScore += newScore;
            if (board.GetCurrentPlayer() == player)
            {
                currentScore += newScore;

                if (currentScore > bestScore)
                {
                    bestScore = currentScore;
                    bestMove = m;
                    m.mScore = bestScore;
                }
            }
            else
            {
                currentScore -= newScore;

                if (currentScore < bestScore)
                {
                    bestScore = currentScore;
                    bestMove = m;
                    m.mScore = bestScore;
                }
            }
            bTest.StepBack();
        }
        List<Move> bestMoves = new List<Move>();
        if (currentDepth == 0)
        {
            foreach (Move m in allMoves)
            {
                if (m.mScore == bestScore)
                {
                    bestMoves.Add(m);
                }
            }
            System.Random rnd = new System.Random();

            int index = rnd.Next(bestMoves.Count);
            bestMove = bestMoves.ToArray()[index];
        }
        //board.GetMoves())


        //   bestMove.mScore = bestScore;
        return bestScore;
    }
}



/*  for (int i = 0; i < pieceManager.getBPieces().ToArray().Length; i++)
            {
                List<Piece> list = pieceManager.getBPieces();
                pieceManager.getBPieces()[i].getPossibleActions();
                bool checkDead = list[i].isDead();
                if (!pieceManager.getBPieces()[i].isDead())
                    allMoves.AddRange(pieceManager.getBPieces()[i].moves); //Searching for all Possible Moves
            }
        }
        else if (player == 1)
        {
            for (int i = 0; i < pieceManager.getBPieces().ToArray().Length; i++)
            {
                nextPlayer = 2;

                List<Piece> list = pieceManager.getWPieces();
                pieceManager.getWPieces()[i].getPossibleActions();
                bool checkDead = list[i].isDead();
                if (!pieceManager.getWPieces()[i].isDead())
                    allMoves.AddRange(pieceManager.getWPieces()[i].moves); //Searching for all Possible Moves
            }
        }
*/