using System;

namespace Chess
{
	// helper functions for going between uci notation to code representation
	static class Notation
	{
		public static Piece AsciiToPiece(char ascii)
		{
			switch (ascii)
			{
				case 'P':
					return Piece.WhitePawn;
				case 'N':
					return Piece.WhiteKnight;
				case 'B':
					return Piece.WhiteBishop;
				case 'R':
					return Piece.WhiteRook;
				case 'Q':
					return Piece.WhiteQueen;
				case 'K':
					return Piece.WhiteKing;
				case 'p':
					return Piece.BlackPawn;
				case 'n':
					return Piece.BlackKnight;
				case 'b':
					return Piece.BlackBishop;
				case 'r':
					return Piece.BlackRook;
				case 'q':
					return Piece.BlackQueen;
				case 'k':
					return Piece.BlackKing;
				default:
					throw new ArgumentException($"Piece character {ascii} not supported.");
			}
		}

		public static byte ToSquareIndex(string squareNotation)
		{
			int file = squareNotation[0] - 97;
			int rank = squareNotation[0] - 49;

			int index = rank * 8 + file;
			if (index >= 0 && index <= 63)
			{
				return (byte)index;
			}
			else
			{
				throw new ArgumentException($"{squareNotation} does not map to valid index between 0 and 63 ");
			}
		}
	}
}
