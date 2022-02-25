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

		public static String ToSquareName(byte squareIndex)
		{
			int file = squareIndex % 8;
			int rank = squareIndex / 8;

			return $"{(char)('a' + file)}{rank + 1}"; // file [0-7] [a-h] and rank [0-7] [1-8]
		}

		public static byte ToSquareIndex(string squareNotation)
		{
			int file = squareNotation[0] - 97;
			int rank = squareNotation[1] - 49;

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

		public static char ToChar(Piece piece)
		{
			switch(piece)
			{
				case Piece.WhitePawn:
				{
					return 'P';
				}
				case Piece.WhiteKnight:
				{
					return 'N';
				}
				case Piece.WhiteBishop:
				{
					return 'B';
				}
				case Piece.WhiteRook:
				{
					return 'R';
				}
				case Piece.WhiteQueen:
				{
					return 'Q';
				}
				case Piece.WhiteKing:
				{
					return 'K';
				}
				case Piece.BlackPawn:
				{
					return 'p';
				}
				case Piece.BlackKnight:
				{
					return 'n';
				}
				case Piece.BlackBishop:
				{
					return 'b';
				}
				case Piece.BlackRook:
				{
					return 'r';
				}
				case Piece.BlackQueen:
				{
					return 'q';
				}
				case Piece.BlackKing:
				{
					return 'k';
				}
				default:
					return ' ';
			}
		}
	}
}
