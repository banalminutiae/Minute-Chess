using System;

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
	}

	public class Board
	{
		Piece[] _state = new Piece[64];

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
	}
}