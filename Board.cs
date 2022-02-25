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

		private void AddLegalMoves(List<Move> moves, int squareIndex)
		{
			switch( _state[squareIndex]) // piece of current square on board
			{
				case Piece.BlackPawn:
				{
					AppendBlackPawnMove(moves, squareIndex);
					break;
				}
				case Piece.BlackBishop:
				{
					AppendBlackBishopMove(moves, squareIndex);
					break;
				}
				case Piece.WhitePawn:
				{
					AppendWhitePawnMove(moves, squareIndex);
					break;
				}

			}
		}
		private void AppendBlackPawnMove(List<Move> moves, int startPosition)
		{
			int legalMoveIndex = startPosition - 8;
			if (legalMoveIndex >= 0 && _state[legalMoveIndex] == Piece.None)
			{
				moves.Add(new Move((byte)startPosition, (byte)(legalMoveIndex), Piece.None));
			}
		}

		private void AppendBlackBishopMove(List<Move> moves, int startPosition)
		{
			
		}

		private void AppendWhitePawnMove(List<Move> moves, int startPosition)
		{
			int legalMoveIndex = startPosition + 8;
			if (legalMoveIndex < 64 && _state[legalMoveIndex] == Piece.None)
			{
				moves.Add(new Move((byte)startPosition, (byte)(legalMoveIndex), Piece.None));
			}
		}
	}
}