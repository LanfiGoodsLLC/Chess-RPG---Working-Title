using UnityEngine;
using System.Collections.Generic;

public class Pawn : ChessPieces
{
    public override List<Vector2Int> GetAvailableMoves(ref ChessPieces[,] board, int tileCountX, int tileCountY)
    {
        //make list
        List<Vector2Int> r = new List<Vector2Int>();

        int direction = (team == 0) ? 1 : -1;
        //list of moves:

        //Move 1. one in front
        if (board[currentX, currentY + direction] == null)
            r.Add(new Vector2Int(currentX, currentY + direction));

        //Move 2. two in front
        if (board[currentX, currentY + direction] == null)
        {
            //white team
            if (team == 0 && currentY == 1 && board[currentX, currentY + (direction * 2)] == null)
                r.Add(new Vector2Int(currentX, currentY + (direction * 2)));

            //black team 
            if (team == 1 && currentY == 6 && board[currentX, currentY + (direction * 2)] == null)
                r.Add(new Vector2Int(currentX, currentY + (direction * 2)));
        }

        //Attack Move

        if (currentX != tileCountX - 1)
            if (board[currentX + 1, currentY + direction] != null && board[currentX + 1, currentY + direction].team != team)
                r.Add(new Vector2Int(currentX + 1, currentY + direction));

        if (currentX != 0)
            if (board[currentX - 1, currentY + direction] != null && board[currentX - 1, currentY + direction].team != team)
                r.Add(new Vector2Int(currentX - 1, currentY + direction));


        //return list
        return r;
    }

    public override SpecialMove GetSpecialMoves(ref ChessPieces[,] board, ref List<Vector2Int[]> moveList, ref List<Vector2Int> availableMoves)

    {

        int direction = (team == 0) ? 1 : -1;

        //queening
        if((team == 0 && currentY == 6) || (team == 1 && currentY == 1))
                return SpecialMove.Promotion;

        //En Passant 
        if(moveList.Count > 0)
        {
            Vector2Int[] lastMove = moveList[moveList.Count - 1];
            if(board[lastMove[1].x, lastMove[1].y].type == ChessPieceType.Pawn) // if last move was a pawn
            {
                if (Mathf.Abs(lastMove[0].y - lastMove[1].y) == 2) //if last move was +2 in either direction
                {
                    if(board[lastMove[1].x, lastMove[1].y].team != team) //if move was from other team
                    {
                        if(lastMove[1].y == currentY) //if both pawn are on same y
                        {
                            if(lastMove[1].x == currentX - 1) //landed left
                            {
                                availableMoves.Add(new Vector2Int(currentX - 1, currentY + direction));
                                return SpecialMove.Enpassant;
                            }
                            if (lastMove[1].x == currentX + 1) //landed right
                            {
                                availableMoves.Add(new Vector2Int(currentX + 1, currentY + direction));
                                return SpecialMove.Enpassant;
                            }
                        }
                    }
                }
            }
        }

        return SpecialMove.none;
    }
}
