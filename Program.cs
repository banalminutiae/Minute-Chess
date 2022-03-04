using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Chess
{
	class Program
	{
		static Board _board = null;
		
		static void Main(string[] args)
		{
			bool running = true;
			while (running)
			{
				string[] tokens = Console.ReadLine().Split();
				switch(tokens[0])
				{
					case "uci":
					{
						Console.WriteLine("uciok");
						break;
					}
					case "isready":
					{
						Console.WriteLine("readyok");
						break;
					}
					case "position":
						UciPosition(tokens);
						break;
					case "go":
					{
						string uciMove = UciFindBestMove(tokens);
						Console.WriteLine($"bestmove {uciMove}");
						break;
					}
					case "stop":
					{
						break;
					}
					case "quit":
					{
						running = false;
						break;
					}
					default:
					{
						break;
					}
				}
			}
		}

		private static void UciPosition(string[] tokens)
		{
			// FEN parsing
			if (tokens[1] == "startpos")
			{
				_board = new Board(Board.STARTING_POS_FEN);
			}
			else if (tokens[1] == "fen")
			{
				_board = new Board($"{tokens[2]} {tokens[3]} {tokens[4]} {tokens[5]} {tokens[6]} {tokens[7]}");
			}

			int firstMove = Array.IndexOf(tokens, "moves") + 1;
			
			if (firstMove == 0)
			{
				return;
			}

			for (int movesIndex = firstMove; movesIndex < tokens.Length; movesIndex++)
			{
				_board.Play(new Move(tokens[movesIndex]));
			}
		}

		private static String UciFindBestMove(string[] tokens)
		{
			List<Move> legalMoves = _board.GetLegalMoves();
			Move randomMove = GetRandomMove(legalMoves); // to be replaced with move evaluation
			return randomMove.ToString();
		}

		private static Move GetRandomMove(List<Move> moves)
		{
			var rand = new Random();
			int index = rand.Next(moves.Count);
			return moves[index];
		}
	}
}
