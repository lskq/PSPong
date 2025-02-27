namespace PSPong;

class Model
{
    static private int buffer = 1;
    static private int height = Console.WindowHeight - buffer - 1;
    static private int width = Console.WindowWidth - 1;

    static private int playerLength = 3;
    static private int winningScore = 3;
    static private int player1StartPosX = 1;
    static private int player2StartPosX = width;
    static private int playersStartPosY = (height / 2) - 1;
    static private int ballStartPosX = width / 2;
    static private int ballStartPosY = height / 2;

    private Player player1;
    private Player player2;
    private Ball ball;

    public void Step(int player1Delta, int player2Delta)
    {
        player1.Move(player1Delta);
        player2.Move(player2Delta);
        ball.Move();

        CheckCollision();
    }

    public void SetUp()
    {
        player1 = new(player1StartPosX, playersStartPosY, playerLength, "Player 1");
        player2 = new(player2StartPosX, playersStartPosY, playerLength, "Player 2");

        NewBall();
    }

    public void NewBall()
    {
        double[] ballStartVelocity = [1.0, 0.0];
        ball = new(ballStartPosX, ballStartPosY, ballStartVelocity);
    }

    public void CheckCollision()
    {
        bool bounce = false;
        int ballPosX = ball.GetNextX();
        int ballPosY = ball.GetNextY();

        if (ballPosY <= buffer)
        {
            //bounced off the top
            bounce = true;
        }

        if (ballPosY >= height)
        {
            //bounced off the bottom
            bounce = true;
        }

        int[] player1Pos = [player1.GetX(), player1.GetY()];
        if (ballPosX <= player1Pos[0])
        {
            if (ballPosY >= player1Pos[1] && ballPosY <= player1Pos[1] + player1.GetLength())
            {
                //bounced off player 1
                bounce = true;
            }
            else
            {
                //scored on player 1
                player2.IncrementScore();
                NewBall();
                return;
            }
        }

        int[] player2Pos = [player2.GetX(), player2.GetY()];
        if (ballPosX >= player2Pos[0])
        {
            if (ballPosY >= player2Pos[1] && ballPosY <= player2Pos[1] + player2.GetLength())
            {
                //bounced off player 2
                bounce = true;
            }
            else
            {
                //scored on player 2
                player1.IncrementScore();
                NewBall();
                return;
            }
        }

        if (bounce)
        {
            double[] oldVelocity = ball.GetVelocity();
            double[] newVelocity = [-1 * oldVelocity[0], -1 * oldVelocity[1]];

            ball.SetVelocity(newVelocity);
        }
    }

    public string? GetWinner()
    {
        if (player1.GetScore() >= winningScore)
        {
            return player1.GetName();
        }
        if (player2.GetScore() >= winningScore)
        {
            return player2.GetName();
        }

        return null;
    }

    public Player[] GetPlayers()
    {
        return [player1, player2];
    }

    public Ball GetBall()
    {
        return ball;
    }

    public int GetHeight()
    {
        return height;
    }

    public int GetWidth()
    {
        return width;
    }

    public class Player(int x, int y, int length, string name)
    {
        private int x = x;
        private int y = y;
        private int oldY = y;
        private int length = length;
        private string name = name;
        private int score = 0;

        public void Move(int yMove)
        {
            int newY = y + yMove;

            if (buffer < newY && newY < height)
            {
                oldY = y;
                y = newY;
            }
        }

        public void IncrementScore()
        {
            score++;
        }

        public int GetX()
        {
            return x;
        }

        public int GetY()
        {
            return y;
        }

        public int GetOldY()
        {
            return oldY;
        }

        public int GetLength()
        {
            return length;
        }

        public string GetName()
        {
            return name;
        }

        public int GetScore()
        {
            return score;
        }
    }

    public class Ball(int x, int y, double[] velocity)
    {
        private double x = x;
        private double y = y;
        private double oldX = x;
        private double oldY = y;
        private double[] velocity = velocity;

        public void Move()
        {
            oldX = x;
            oldY = y;
            x += velocity[0];
            y += velocity[1];
        }

        public void SetVelocity(double[] newVelocity)
        {
            velocity = newVelocity;
        }

        public int GetNextX()
        {
            return (int)(x + velocity[0]);
        }

        public int GetNextY()
        {
            return (int)(y + velocity[1]);
        }

        public int GetX()
        {
            return (int)x;
        }

        public int GetY()
        {
            return (int)y;
        }

        public int GetOldX()
        {
            return (int)oldX;
        }

        public int GetOldY()
        {
            return (int)oldY;
        }

        public double[] GetVelocity()
        {
            return velocity;
        }
    }
}