namespace PSPong;

class Model
{
    static private int buffer = 1;
    static private int height = Console.WindowHeight - 1;
    static private int width = Console.WindowWidth - 1;

    static private int playerLength = 3;
    static private int winningScore = 3;

    static private int player1StartPosX = 1;
    static private int player2StartPosX = width - 1;
    static private int playersStartPosY = (height / 2) - 1;

    static private int ballStartPosX = width / 2;
    static private int ballStartPosY = height / 2;

    static private double baseXVelocity = width / 100.0;
    static private double baseYVelocity = height / 10.0;

    private Player player1 = new(player1StartPosX, playersStartPosY, playerLength, "Player 1");
    private Player player2 = new(player2StartPosX, playersStartPosY, playerLength, "Player 2");
    private Ball ball = new(ballStartPosX, ballStartPosY, baseXVelocity, baseYVelocity);

    public void Step(int player1Delta, int player2Delta)
    {
        player1.Move(player1Delta);
        player2.Move(player2Delta);
        MoveBall();
    }

    public void MoveBall()
    {
        int ballX = ball.GetX();

        if (ballX == 0)
        {
            //scored on player 1
            player2.IncrementScore();
            ball.Reset();
            return;
        }
        else if (ballX == width)
        {
            //scored on player2
            player1.IncrementScore();
            ball.Reset();
            return;
        }

        int ballNextX = ball.GetNextX();
        int ballNextY = ball.GetNextY();

        double ballVelocityX = ball.GetVelocityX();
        double ballVelocityY = ball.GetVelocityY();

        if (ball.HasBounce())
        {
            ball.Move();
            ball.SetBounce(false);
            return;
        }

        int player1X = player1.GetX();
        int player1Y = player1.GetY();
        int player2X = player2.GetX();
        int player2Y = player2.GetY();

        // Collision-checking conditional from hell
        if (ballNextY < buffer ||
            ballNextY > height ||
            (ballNextX <= player1X && player1Y <= ballNextY && ballNextY <= player1Y + player1.GetTail()) ||
            (ballNextX >= player2X && player2Y <= ballNextY && ballNextY <= player2Y + player2.GetTail())
        )
        {
            if (ballNextY < buffer)
            {
                //About to hit the top
                ball.SetY(buffer);
                ball.SetVelocityY(-1 * ballVelocityY);
            }
            else if (ballNextY > height)
            {
                //About to hit the bottom
                ball.SetY(height);
                ball.SetVelocityY(-1 * ballVelocityY);
            }

            if (ballNextX <= player1X && player1Y <= ballNextY && ballNextY <= player1Y + player1.GetTail())
            {
                //About to bounce off player 1
                ball.SetX(player1.GetX() + 1);
                ball.SetVelocityX(-1 * ballVelocityX);
            }
            else if (ballNextX >= player2X && player2Y <= ballNextY && ballNextY <= player2Y + player2.GetTail())
            {
                //about to bounce off player 2
                ball.SetX(player2.GetX() - 1);
                ball.SetVelocityX(-1 * ballVelocityX);
            }

            ball.SetBounce(true);
        }

        if (!ball.HasBounce())
        {
            if (ballNextX <= 0)
            {
                //about to score on player 1
                ball.SetX(0);
                ball.SetVelocity([0, 0]);
            }
            else if (ballNextX >= width)
            {
                //about to score on player 2
                ball.SetX(width);
                ball.SetVelocity([0, 0]);
            }
            else
            {
                ball.Move();
            }
        }
    }

    public bool HasWinner()
    {
        if (player1.GetScore() >= winningScore || player2.GetScore() >= winningScore)
        {
            return true;
        }
        return false;
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
        else
        {
            return null;
        }
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

            if (buffer <= newY && newY <= height - length + 1)
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

        public int GetTail()
        {
            return length - 1;
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

    public class Ball(int x, int y, double velocityX, double velocityY)
    {
        private double x = x;
        private double y = y;
        private double oldX = x;
        private double oldY = y;
        private double velocityX = velocityX;
        private double velocityY = velocityY;
        private bool bounced = false;

        public void Move()
        {
            RememberPosition();
            x += velocityX;
            y += velocityY;
        }

        public void Reset()
        {
            SetPosition(ballStartPosX, ballStartPosY);
            SetRandomVelocity();
        }

        public void SetRandomVelocity()
        {
            Random random = new();

            velocityX = baseXVelocity * (random.NextDouble() + 0.5) * (random.Next(2) == 0 ? -2 : 2);
            velocityY = baseYVelocity * (random.NextDouble() + 0.5) * (random.Next(2) == 0 ? -1 : 1);
        }

        public void SetBounce(bool hasBounced)
        {
            bounced = hasBounced;
        }

        public void RememberPosition()
        {
            oldX = x;
            oldY = y;
        }

        public void SetPosition(int newX, int newY)
        {
            RememberPosition();
            x = newX;
            y = newY;

        }

        public void SetX(int newX)
        {
            RememberPosition();
            x = newX;
        }

        public void SetY(int newY)
        {
            RememberPosition();
            y = newY;
        }

        public void SetVelocity(double[] newVelocity)
        {
            velocityX = newVelocity[0];
            velocityY = newVelocity[1];
        }

        public void SetVelocityX(double newVelocityX)
        {
            velocityX = newVelocityX;
        }

        public void SetVelocityY(double newVelocityY)
        {
            velocityY = newVelocityY;
        }

        public bool HasBounce()
        {
            return bounced;
        }

        public int GetNextX()
        {
            return (int)(x + velocityX);
        }

        public int GetNextY()
        {
            return (int)(y + velocityY);
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
            return [velocityX, velocityY];
        }

        public double GetVelocityX()
        {
            return velocityX;
        }

        public double GetVelocityY()
        {
            return velocityY;
        }
    }
}