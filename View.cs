using System.Windows.Input;

namespace PSPong;

class View(Model model)
{
    static char playerIcon = '#';
    static char ballIcon = '@';
    static char empty = ' ';

    static int stepRate = 50;

    int height = model.GetHeight();
    int width = model.GetWidth();

    Model.Player[] players = model.GetPlayers();
    Model.Ball ball = model.GetBall();

    public void Step()
    {
        foreach (Model.Player player in players)
        {
            int x = player.GetX();

            int oldY = player.GetOldY();
            int currentY = player.GetY();

            if (currentY > oldY)
            {
                Console.SetCursorPosition(x, oldY);
                Console.Write(empty);
                Console.SetCursorPosition(x, currentY + player.GetTail());
                Console.Write(playerIcon);
            }
            else if (currentY < oldY)
            {
                Console.SetCursorPosition(x, oldY + player.GetTail());
                Console.Write(empty);
                Console.SetCursorPosition(x, currentY);
                Console.Write(playerIcon);
            }
        }

        int oldBallX = ball.GetOldX();
        int oldBallY = ball.GetOldY();

        int currentBallX = ball.GetX();
        int currentBallY = ball.GetY();

        if (oldBallX != currentBallX || oldBallY != currentBallY)
        {
            Console.SetCursorPosition(oldBallX, oldBallY);
            Console.Write(empty);
            Console.SetCursorPosition(currentBallX, currentBallY);
            Console.Write(ballIcon);
        }

        UpdateScore();

        Thread.Sleep(stepRate);
    }

    public void ShowEndMessage()
    {
        string? winner = model.GetWinner();
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
        Console.Write(players[0].GetScore());

        x = width * 3 / 4;
        Console.SetCursorPosition(x, 0);
        Console.Write(players[1].GetScore());
    }
    public void SetUp()
    {
        Console.Clear();
        Console.CursorVisible = false;

        foreach (Model.Player player in players)
        {
            int y = player.GetY();
            int length = player.GetLength();

            for (int i = y; i < y + length; i++)
            {
                Console.SetCursorPosition(player.GetX(), i);
                Console.Write(playerIcon);
            }
        }

        Console.SetCursorPosition(ball.GetX(), ball.GetY());
        Console.Write(ballIcon);

        UpdateScore();
    }

    public void TearDown()
    {
        Console.Clear();
        Console.CursorVisible = true;
    }
}