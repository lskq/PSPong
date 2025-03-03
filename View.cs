using System.Windows.Input;

namespace PSPong;

internal class View
{
    public View(Model model)
    {
        Model = model;

        Players = Model.Players();
        Ball = Model.Ball1;

        SetUp();
    }

    public static char PlayerIcon { get; } = '#';
    public static char BallIcon { get; } = '@';
    public static char EmptyIcon { get; } = ' ';

    static int UpdateRate { get; } = 60;

    static int Height { get; } = Model.Height;
    static int Width { get; } = Model.Width;

    public Model Model;
    public Model.Player[] Players;
    public Model.Ball Ball;

    public void Step()
    {
        foreach (Model.Player player in Players)
        {
            int x = player.X;

            int oldY = player.OldY;
            int currentY = player.Y;

            int tail = player.Tail;

            if (currentY > oldY)
            {
                Console.SetCursorPosition(x, oldY);
                Console.Write(EmptyIcon);
                Console.SetCursorPosition(x, currentY + tail);
                Console.Write(PlayerIcon);
            }
            else if (currentY < oldY)
            {
                Console.SetCursorPosition(x, oldY + tail);
                Console.Write(EmptyIcon);
                Console.SetCursorPosition(x, currentY);
                Console.Write(PlayerIcon);
            }
        }

        int oldBallX = (int)Ball.OldX;
        int oldBallY = (int)Ball.OldY;

        int currentBallX = (int)Ball.X;
        int currentBallY = (int)Ball.Y;

        if (oldBallX != currentBallX || oldBallY != currentBallY)
        {
            Console.SetCursorPosition(oldBallX, oldBallY);
            Console.Write(EmptyIcon);
            Console.SetCursorPosition(currentBallX, currentBallY);
            Console.Write(BallIcon);
        }

        UpdateScore();

        Thread.Sleep(UpdateRate);
    }

    public void ShowEndMessage()
    {
        string? winner = Model.Winner();
        string endMessage = "";

        if (winner != null)
        {
            endMessage = $"{winner} wins!";
        }
        else
        {
            endMessage = "Game aborted. Press esc again to quit.";
        }

        Console.SetCursorPosition((Width / 2) - endMessage.Length / 2, Height / 2);
        Console.WriteLine(endMessage);

    }

    public void UpdateScore()
    {
        int x = Width / 4;
        Console.SetCursorPosition(x, 0);
        Console.Write(Players[0].Score);

        x = Width * 3 / 4;
        Console.SetCursorPosition(x, 0);
        Console.Write(Players[1].Score);
    }
    public void SetUp()
    {
        Console.Clear();
        Console.CursorVisible = false;

        foreach (Model.Player player in Players)
        {
            int y = player.Y;
            int length = player.Length;

            for (int i = y; i < y + length; i++)
            {
                Console.SetCursorPosition(player.X, i);
                Console.Write(PlayerIcon);
            }
        }

        Console.SetCursorPosition((int)Ball.X, (int)Ball.Y);
        Console.Write(BallIcon);

        UpdateScore();
    }

    static public void TearDown()
    {
        Console.Clear();
        Console.CursorVisible = true;
    }
}