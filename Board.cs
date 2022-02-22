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

			// e7e8q
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
		}
	}
}