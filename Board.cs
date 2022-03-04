using System;
using System.Collections.Generic;

namespace Chess
{
	public enum Piece
	{
		None,
		WhitePawn,
		WhiteKnight,
		WhiteBishop,
		WhiteRook,
		WhiteQueen,
		WhiteKing,
		BlackPawn,
		BlackKnight,
		BlackBishop,
		BlackRook,
		BlackQueen,
		BlackKing,
	}

	public struct Move
	{
		public static Move BlackCastleShort = new Move("e8g8");
		public static Move BlackCastleLong = new Move("e8c8");
		public static Move WhiteCastleShort = new Move("e1g1");
		public static Move WhiteCastleLong = new Move("e1c1");

		public static Move BlackRookShortCastle = new Move("h8f8");
		public static Move BlackRookLongCastle = new Move("a8d8");
		public static Move WhiteRookShortCastle = new Move("h1f1");
		public static Move WhiteRookLongCastle = new Move("a1d1");

		public byte SrcIdx;
		public byte DestIdx;
		public Piece Promotion;

		public Move(byte SrcIdx, byte DestIdx, Piece Promotion)
		{
			this.SrcIdx = SrcIdx;
			this.DestIdx = DestIdx;
			this.Promotion = Promotion;
		}

		public Move(string uciMoveNotation)
		{
			string fromSquare = uciMoveNotation.Substring(0, 2);
			string toSquare = uciMoveNotation.Substring(2, 2);

			this.SrcIdx = Notation.ToSquareIndex(fromSquare);
			this.DestIdx = Notation.ToSquareIndex(toSquare);

			// e.g. e7e8q
			Promotion = (uciMoveNotation.Length == 5) ? Notation.AsciiToPiece(uciMoveNotation[4]) : Piece.None;
		}

		public override string ToString()// format into UCI
		{
			string result = Notation.ToSquareName(SrcIdx);
			result += Notation.ToSquareName(DestIdx);

			if (Promotion != Piece.None)
			{
				result += Notation.ToChar(Promotion);
			}
			return result;
		}
	}

	public class Board
	{
		// notation denoting initial state of game board
		public const string STARTING_POS_FEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

		Piece[] _state = new Piece[64];

		bool _blacksMove = false;

		public Board() { }

		public Board(string fen)
		{
			string[] fields = fen.Split();
			string[] positions = fields[0].Split('/');
			int rank = 7;
			for (int i = 0; i < positions.Length; i++)
			{
				int file = 0;
				for (int j = 0; j < positions[i].Length; j++)
				{
					if (char.IsNumber(positions[i][j])) // numbers represent empty space
					{
						int emptySquares = (int) char.GetNumericValue(positions[i][j]);
						file += emptySquares;
					}
					else
					{
						_state[rank * 8 + file] = Notation.AsciiToPiece(positions[i][j]);
						file++;
					}
				}
				rank--; // across, then down
			}
			_blacksMove = fields[1].Equals("w", StringComparison.CurrentCultureIgnoreCase) ? false : true;
		}

		public Piece this[int idx]
		{
			get => _state[idx];
			set => _state[idx] = value;
		}

		public Piece this[int rank, int file]
		{
			get =>_state[rank * 8 + file];
			set =>_state[rank * 8 + file] = value;
		}

		public void Play(Move move)
		{
			Piece movingPiece = _state[move.SrcIdx];
			if (move.Promotion != Piece.None)
			{
				movingPiece = move.Promotion;
			}

			_state[move.DestIdx] = movingPiece;
			_state[move.SrcIdx] = Piece.None;

			if (CheckCastle(movingPiece, move, out Move rookMove))
			{
				Play(rookMove);
			}
		}

		public bool CheckCastle(Piece movingPiece, Move move, out Move rookMove)
		{
			if (movingPiece == Piece.BlackKing && move.Equals(Move.BlackCastleShort))
			{
				rookMove = Move.BlackRookShortCastle;
				return true;
			}

			if (movingPiece == Piece.BlackKing && move.Equals(Move.BlackCastleLong))
			{
				rookMove = Move.BlackRookLongCastle;
				return true;
			}

			if (movingPiece == Piece.WhiteKing && move.Equals(Move.WhiteCastleShort))
			{
				rookMove = Move.WhiteRookShortCastle;
				return true;
			}

			if (movingPiece == Piece.WhiteKing && move.Equals(Move.WhiteCastleLong))
			{
				rookMove = Move.WhiteRookLongCastle;
				return true;
			}

			rookMove = default;
			return false;
		}

		public List<Move> GetLegalMoves()
		{
			List<Move> moves = new List<Move>();
			for (int squareIndex = 0;
				squareIndex < 64;
				squareIndex++)
			{
				if (_state[squareIndex] == Piece.None) continue; // check for piece
				if (_state[squareIndex] < Piece.BlackPawn ^ _blacksMove) continue; // check for piece colour

				AddLegalMoves(moves, squareIndex);
			}
			return moves;
		}

		private bool PieceIsBlack(Piece piece) => (int)piece >= 7;

		private bool PieceIsWhite(Piece piece) => (int)piece < 7 && (int)piece != 0;

		private void AddLegalMoves(List<Move> moves, int squareIndex)
		{
			switch( _state[squareIndex]) 
			{
				case Piece.BlackPawn:
				{
					AddBlackPawnMove(moves, squareIndex);
					AddBlackPawnAttack(moves, squareIndex);
					break;
				}
				/*
				case Piece.BlackKnight:
				{
					AddBlackKnightMove(moves, squareIndex);
					AddBlackKnightAttack(moves, squareIndex);
					break;
				}
				case Piece.BlackBishop:
				{
					AddBlackBishopMove(moves, squareIndex);
					AddBlackBishopAttack(moves, squareIndex);
					break;
				}
				case Piece.BlackRook:
				{
					AddBlackRookMove(moves, squareIndex);
					AddBlackRookAttack(moves, squareIndex);
					break;
				}
				case Piece.BlackQueen:
				{
					AddBlackQueenMove(moves, squareIndex);
					AddBlackQueenAttack(moves, squareIndex);
					break;
				}
				case Piece.BlackKing:
				{
					AddBlackKingMove(moves, squareIndex);
					AddBlackKingAttack(moves, squareIndex);
					break;
				}
				*/
				case Piece.WhitePawn:
				{
					AddWhitePawnMove(moves, squareIndex);
					AddWhitePawnAttack(moves, squareIndex);
					break;
				}
				/*
				case Piece.WhiteKnight:
				{
					AddWhiteKnightMove(moves, squareIndex);
					AddWhiteKnightAttack(moves, squareIndex);
					break;
				}
				case Piece.WhiteBishop:
				{
					AddWhiteBishopMove(moves, squareIndex);
					AddWhiteBishopAttack(moves, squareIndex);
					break;
				}
				case Piece.WhiteRook:
				{
					AddWhiteRookMove(moves, squareIndex);
					AddWhiteRookAttack(moves, squareIndex);
					break;
				}
				case Piece.WhiteQueen:
				{
					AddWhiteQueenMove(moves, squareIndex);
					AddWhiteQueenAttack(moves, squareIndex);
					break;
				}
				case Piece.WhiteKing:
				{
					AddWhiteKingMove(moves, squareIndex);
					AddWhiteKingAttack(moves, squareIndex);
					break;
				}
				*/
			}
		}
		private void AddBlackPawnMove(List<Move> moves, int startPosition)
		{
			int legalMoveIndex = startPosition - 8;
			if (legalMoveIndex >= 0 && _state[legalMoveIndex] == Piece.None)
			{
				moves.Add(new Move((byte)startPosition, (byte)legalMoveIndex, Piece.None));
			}
		}

		private void AddBlackPawnAttack(List<Move> moves, int startPosition)
		{
			int rank = startPosition / 8;
			int file = startPosition % 8;

			if (rank - 1 >= 0 && file - 1 <= 7) // black pawns attack 'down' left 
			{
				if (PieceIsWhite(_state[startPosition - 9])) // values based on 8c8 board where each square is marked with an int, 0 being a1
				{
					moves.Add(new Move((byte)startPosition, (byte)(startPosition - 9), Piece.None));
				}
			}
			if (rank + 1 >= 0 && file - 1 <= 7) // and 'down' right
			{
				if (PieceIsWhite(_state[startPosition - 7]))
				{
					moves.Add(new Move((byte)startPosition, (byte)(startPosition - 7), Piece.None));
				}
			}
		}

		private void AddBlackKnightMove(List<Move> moves, int startPosition)
		{

		}

		private void AddBlackKnightAttack(List<Move> moves, int startPosition)
		{

		}

		private void AddBlackBishopMove(List<Move> moves, int startPosition)
		{
			
		}

		private void AddBlackBishopAttack(List<Move> moves, int startPosition)
		{

		}

		private void AddBlackRookMove(List<Move> moves, int startPosition)
		{

		}

		private void AddBlackRookAttack(List<Move> moves, int startPosition)
		{

		}

		private void AddBlackQueenMove(List<Move> moves, int startPosition)
		{

		}

		private void AddBlackQueenAttack(List<Move> moves, int startPosition)
		{

		}

		private void AddBlackKingMove(List<Move> moves, int startPosition)
		{

		}

		private void AddBlackKingAttack(List<Move> moves, int startPosition)
		{

		}

		private void AddWhitePawnMove(List<Move> moves, int startPosition)
		{
			int legalMoveIndex = startPosition + 8;
			if (legalMoveIndex < 64 && _state[legalMoveIndex] == Piece.None)
			{
				moves.Add(new Move((byte)startPosition, (byte)(legalMoveIndex), Piece.None));
			}
		}

		private void AddWhitePawnAttack(List<Move> moves, int startPosition)
		{
			int rank = startPosition / 8;
			int file = startPosition % 8;

			if (rank - 1 >= 0 && file + 1 <= 7)
			{
				if (PieceIsBlack(_state[startPosition + 7]))
				{
					moves.Add(new Move((byte)startPosition, (byte)(startPosition + 7), Piece.None));
				}
			}
			if (rank + 1 >= 0 && file + 1 <= 7)
			{
				if (PieceIsBlack(_state[startPosition + 9]))
				{
					moves.Add(new Move((byte)startPosition, (byte)(startPosition + 9), Piece.None));
				}
			}
		}

		private void AddWhiteKnightMove(List<Move> moves, int startPosition)
		{

		}

		private void AddWhiteKnightAttack(List<Move> moves, int startPosition)
		{

		}

		private void AddWhiteBishopMove(List<Move> moves, int startPosition)
		{

		}

		private void AddWhiteBishopAttack(List<Move> moves, int startPosition)
		{

		}

		private void AddWhiteRookMove(List<Move> moves, int startPosition)
		{

		}

		private void AddWhiteRookAttack(List<Move> moves, int startPosition)
		{

		}

		private void AddWhiteQueenMove(List<Move> moves, int startPosition)
		{

		}

		private void AddWhiteQueenAttack(List<Move> moves, int startPosition)
		{

		}

		private void AddWhiteKingMove(List<Move> moves, int startPosition)
		{

		}

		private void AddWhiteKingAttack(List<Move> moves, int startPosition)
		{

		}
	}
}