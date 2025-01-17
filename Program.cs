﻿namespace Tetris
{
	class Program
	{
		static void Main(string[] args)
		{
			int mapHeight = 20;
			int mapWidth = 10;
			int namberOfFigure = 6;
			string textGameOver = string.Empty;
			string textScore = string.Empty;
			int score = 0;
			bool gameOver = false;
			while (true)
			{
				Console.Clear();
				Interface.WriteGameOver(textGameOver, textScore, mapWidth, mapHeight);

				ConsoleKeyInfo keyToStart = Console.ReadKey();
				if (keyToStart.Key == ConsoleKey.Escape) break;
				else if (keyToStart.Key == ConsoleKey.Enter)
				{
					Console.Clear();

					Borders borders = new Borders(mapWidth, mapHeight);
					borders.Draw();

					TetrisFigure nextFigure = new TetrisFigure(TetrisFigure.RandomFigure(namberOfFigure), TetrisFigure.RandomRotate());
					Interface interfac = new Interface(mapWidth, nextFigure);
					interfac.Draw();

					InstalledFigure installedFigure = new InstalledFigure(mapWidth, mapHeight);

					gameOver = false;
					while (!gameOver)
					{

						installedFigure.CheckLine();
						interfac.ReWriteScore(installedFigure.score);

						TetrisFigure tetrisFigure = new TetrisFigure(nextFigure.select, nextFigure.rotate);
						nextFigure = new TetrisFigure(TetrisFigure.RandomFigure(namberOfFigure), TetrisFigure.RandomRotate());

						interfac.RewriteFigure(nextFigure);
						tetrisFigure.Offset(4, 0);
						tetrisFigure.Draw();

						Thread.Sleep(100);
						bool firstPass = true;
						while (true)
						{
							int timeStep = 0;
							if (interfac.score > 3000) timeStep = 200;
							else if (interfac.score > 5000) timeStep = 100;
							else timeStep = 300;

							while (timeStep > 0)
							{
								int timeToSleep = 5;
								if (Console.KeyAvailable)
								{
									ConsoleKeyInfo key = Console.ReadKey();
									if (key.Key == ConsoleKey.UpArrow)
									{
										TetrisFigure tetrisFigureWhithRotate = tetrisFigure.DegreeRotation90();
										if (!borders.IsHit(tetrisFigureWhithRotate) && !installedFigure.IsHit(tetrisFigureWhithRotate))
											tetrisFigure = tetrisFigure.RewriteOn(tetrisFigureWhithRotate);
									}
									else if (key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.LeftArrow)
									{
										TetrisFigure FigureMove = tetrisFigure.HorizontalMove(key.Key);
										if (!borders.IsHit(FigureMove) && !installedFigure.IsHit(FigureMove))
											tetrisFigure = tetrisFigure.RewriteOn(FigureMove);
									}
									else if (!firstPass && key.Key == ConsoleKey.DownArrow) timeStep = 0;
								}
								timeStep -= timeToSleep;
								Thread.Sleep(timeToSleep);
							}
							TetrisFigure tetrisFigureMove = tetrisFigure.VerticalMove();
							if (!borders.IsHitDownBorder(tetrisFigureMove) && !installedFigure.IsHit(tetrisFigureMove))
								tetrisFigure = tetrisFigure.RewriteOn(tetrisFigureMove);
							else
							{
								if (borders.IsHitLineGameOver(tetrisFigure)) gameOver = true;
								else installedFigure.Add(tetrisFigure);
								break;
							}
							firstPass = false;
						}
						score = interfac.score;
					}
				}
				if (gameOver)
				{
					textGameOver = "GAME OVER";
					textScore = "Ваш результат равен " + score;
				}
			}

		}
	}
}