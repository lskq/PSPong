using System.Windows.Input;

namespace PSPong;

class View(Model model)
{
    static char playerIcon = '#';
    static char ballIcon = '@';
    static char empty = ' ';

    static int rate = 60;

    int height = Model.Height;
    int width = Model.Width;

    Model.Player[] players = model.Players();
    Model.Ball ball = model.Ball1;

    public void Step()
    {
        foreach (Model.Player player in players)
        {
            int x = player.X;

            int oldY = player.OldY;
            int currentY = player.Y;

            int tail = player.Tail;

            if (currentY > oldY)
            {
                Console.SetCursorPosition(x, oldY);
                Console.Write(empty);
                Console.SetCursorPosition(x, currentY + tail);
                Console.Write(playerIcon);
            }
            else if (currentY < oldY)
            {
                Console.SetCursorPosition(x, oldY + tail);
                Console.Write(empty);
                Console.SetCursorPosition(x, currentY);
                Console.Write(playerIcon);
            }
        }

        int oldBallX = (int)ball.OldX;
        int oldBallY = (int)ball.OldY;

        int currentBallX = (int)ball.X;
        int currentBallY = (int)ball.Y;

        if (oldBallX != currentBallX || oldBallY != currentBallY)
        {
            Console.SetCursorPosition(oldBallX, oldBallY);
            Console.Write(empty);
            Console.SetCursorPosition(currentBallX, currentBallY);
            Console.Write(ballIcon);
        }

        UpdateScore();

        Thread.Sleep(rate);
    }

    public void ShowEndMessage()
    {
        string? winner = model.Winner();
        string endMessage = "";

        if (winner != null)
        {
            endMessage = $"{winner} wins!";
        }
        else
        {
            endMessage = "Game aborted. Press esc again to quit.";
        }

        Console.SetCursorPosition((width / 2) - endMessage.Length / 2, height / 2);
        Console.WriteLine(endMessage);

    }

    public void UpdateScore()
    {
        int x = width / 4;
        Console.SetCursorPosition(x, 0);
        Console.Write(players[0].Score);

        x = width * 3 / 4;
        Console.SetCursorPosition(x, 0);
        Console.Write(players[1].Score);
    }
    public void SetUp()
    {
        Console.Clear();
        Console.CursorVisible = false;

        foreach (Model.Player player in players)
        {
            int y = player.Y;
            int length = player.Length;

            for (int i = y; i < y + length; i++)
            {
                Console.SetCursorPosition(player.X, i);
                Console.Write(playerIcon);
            }
        }

        Console.SetCursorPosition((int)ball.X, (int)ball.Y);
        Console.Write(ballIcon);

        UpdateScore();
    }

    public void TearDown()
    {
        Console.Clear();
        Console.CursorVisible = true;
    }
}