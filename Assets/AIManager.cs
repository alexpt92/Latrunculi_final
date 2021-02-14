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
            return 0;

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
        Move currentMove;
        if (currentDepth == 0)
        {
            float maxScore = 0;
        }
        float score = 0;
        foreach (Move m in allMoves)
        {
            board.MakeMove(m);
            float currentScore = 0;
            currentMove = m;
            if (m.attacked)
            {
                if (nextPlayer == 2)
                    nextPlayer = 1;
                else
                    nextPlayer = 2;
            }
            currentScore = Minimax(board, nextPlayer, maxDepth, currentDepth + 1, ref currentMove);

            float newScore = board.Evaluate(player, currentDepth);
            //Evaluierung aktueller Move
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
            board.StepBack();
            if (m.attacked)
            {
                if (nextPlayer == 2)
                    nextPlayer = 1;
                else
                    nextPlayer = 2;
            }
        }
        List<Move> bestMoves = new List<Move>();
        score += bestScore;

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
        return score;
    }


    public static float Minimax_old(
                BoardDraught board,
                int player,
                int maxDepth,
                int currentDepth,

                ref Move bestMove)
    {
        if (board.IsGameOver() || currentDepth == maxDepth)
        {
            return board.EvaluateOld2(player);
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
        Move currentMove;


        foreach (Move m in allMoves)
        {
            board.MakeMove(m);
            float currentScore = 0;
            currentMove = m;
            if (m.attacked)
            {
                if (nextPlayer == 2)
                    nextPlayer = 1;
                else
                    nextPlayer = 2;
            }
            currentScore = Minimax_old(board, nextPlayer, maxDepth, currentDepth + 1, ref currentMove);

            
            if (board.GetCurrentPlayer() == player)
            {
                if (currentScore > bestScore)
                {
                    bestScore = currentScore;
                    bestMove = m;
                    m.mScore = bestScore;
                }
            }
            else
            {
                if (currentScore < bestScore)
                {
                    bestScore = currentScore;
                    bestMove = m;
                    m.mScore = bestScore;
                }
            }
            board.StepBack();
            if (m.attacked)
            {
                if (nextPlayer == 2)
                    nextPlayer = 1;
                else
                    nextPlayer = 2;
            }
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

        return bestScore;
    }
}