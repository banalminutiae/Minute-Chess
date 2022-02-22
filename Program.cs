using System;
using System.Diagnostics;

namespace Chess
{
	class Program
	{
		static void Main(string[] args)
		{
			while(true)
			{
				string command = Console.ReadLine().Split()[0];
				switch (command)
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
					case "go":
					{

						break;
					}
					default:
					{
						break;
					}
				}
			}
		}
	}
}
