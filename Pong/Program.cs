namespace Pong
{
    using System;
    using System.Threading;

    class Program
    {
        const int Width = 60;
        const int Height = 22;

        static int ballX, ballY;
        static int ballVelX, ballVelY;

        static int playerY = Height / 2;
        static int cpuY = Height / 2;
        static int paddleSize = 4;

        static int playerScore = 0;
        static int cpuScore = 0;

        static void Main()
        {
            Console.CursorVisible = false;
            Console.Clear();

            ShowStartScreen();
            ResetBall();

            while (true)
            {
                SafeDraw();
                SafeInput();
                SafeLogic();
                Thread.Sleep(45);
            }
        }

        // ────────────────────────────────────────────────
        // Start Screen
        // ────────────────────────────────────────────────
        static void ShowStartScreen()
        {
            Console.Clear();
            Console.WriteLine("=========== PONG ==========");
            Console.WriteLine("Controls: Arrow Up / Arrow Down");
            Console.WriteLine("Press ENTER to start...");
            Console.ReadLine();
        }

        // ────────────────────────────────────────────────
        // Draw (Flicker-free)
        // ────────────────────────────────────────────────
        static void SafeDraw()
        {
            try
            {
                Console.SetCursorPosition(0, 0);

                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        if (x == 0 || x == Width - 1)
                        {
                            Console.Write("|");
                        }
                        else if (x == ballX && y == ballY)
                        {
                            Console.Write("O");
                        }
                        else if (IsPlayerPaddle(x, y))
                        {
                            Console.Write("#");
                        }
                        else if (IsCpuPaddle(x, y))
                        {
                            Console.Write("#");
                        }
                        else
                        {
                            Console.Write(" ");
                        }
                    }
                    Console.WriteLine();
                }

                Console.WriteLine($"Player: {playerScore}   CPU: {cpuScore}");
            }
            catch
            {
                // Prevent crash if the console is resized
            }
        }

        static bool IsPlayerPaddle(int x, int y)
        {
            return x == 2 && y >= playerY - paddleSize && y <= playerY + paddleSize;
        }

        static bool IsCpuPaddle(int x, int y)
        {
            return x == Width - 3 && y >= cpuY - paddleSize && y <= cpuY + paddleSize;
        }

        // ────────────────────────────────────────────────
        // Input (Crash-proof)
        // ────────────────────────────────────────────────
        static void SafeInput()
        {
            while (Console.KeyAvailable)
            {
                ConsoleKey key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.UpArrow)
                    playerY--;
                if (key == ConsoleKey.DownArrow)
                    playerY++;

                // Clamp paddle so it never goes out of bounds
                playerY = Math.Max(paddleSize + 1, Math.Min(Height - paddleSize - 2, playerY));
            }
        }

        // ────────────────────────────────────────────────
        // Game Logic (Foolproof)
        // ────────────────────────────────────────────────
        static void SafeLogic()
        {
            ballX += ballVelX;
            ballY += ballVelY;

            // Bounce top/bottom safely
            if (ballY <= 1 || ballY >= Height - 2)
                ballVelY = -ballVelY;

            // CPU AI (safe)
            if (ballY > cpuY && cpuY < Height - paddleSize - 2)
                cpuY++;
            else if (ballY < cpuY && cpuY > paddleSize + 1)
                cpuY--;

            // Player paddle collision
            if (ballX == 3 && ballY >= playerY - paddleSize && ballY <= playerY + paddleSize)
                ballVelX = 1;

            // CPU paddle collision
            if (ballX == Width - 4 && ballY >= cpuY - paddleSize && ballY <= cpuY + paddleSize)
                ballVelX = -1;

            // Score: CPU scores
            if (ballX <= 0)
            {
                cpuScore++;
                ResetBall();
            }

            // Score: Player scores
            if (ballX >= Width - 1)
            {
                playerScore++;
                ResetBall();
            }
        }

        // ────────────────────────────────────────────────
        // Safe Ball Reset
        // ────────────────────────────────────────────────
        static void ResetBall()
        {
            ballX = Width / 2;
            ballY = Height / 2;

            Random r = new Random();

            ballVelX = r.Next(0, 2) == 0 ? -1 : 1;
            ballVelY = r.Next(0, 2) == 0 ? -1 : 1;
        }
    }
}
