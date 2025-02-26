namespace PSPong;

class Model
{
    static private int buffer = 1;
    static private int height = Console.WindowHeight - buffer - 1;
    static private int width = 10;//Console.WindowWidth - 1;

    static private int playerLength = 3;
    static private int winningScore = 3;
    static private int startPosY = (height / 2) - 1;
    static private int[] player1StartPos = [1, startPosY];
    static private int[] player2StartPos = [width, startPosY];
    static private int[] ballStartPos = [width / 2, height / 2];

    private Player player1;
    private Player player2;
    private Ball ball;

    public void Play(int player1Delta, int player2Delta)
    {
        player1.Move(player1Delta);
        player2.Move(player2Delta);
        ball.Move();

        CheckCollision();
    }

    public void SetUp()
    {
        player1 = new(player1StartPos, playerLength, "Player 1");
        player2 = new(player2StartPos, playerLength, "Player 2");

        NewBall();
    }

    public void NewBall()
    {
        double[] ballStartVelocity = [1.0, 0.0];
        ball = new(ballStartPos, ballStartVelocity);
    }

    public void CheckCollision()
    {
        bool bounce = false;
        int[] ballPos = ball.GetNextPosition();

        if (ballPos[1] <= buffer)
        {
            //bounced off the top
            bounce = true;
        }

        if (ballPos[1] >= height)
        {
            //bounced off the bottom
            bounce = true;
        }

        int[] player1Pos = player1.GetPosition();
        if (ballPos[0] <= player1Pos[0])
        {
            if (ballPos[1] >= player1Pos[1] && ballPos[1] <= player1Pos[1] + player1.GetLength())
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

        int[] player2Pos = player2.GetPosition();
        if (ballPos[0] >= player2Pos[0])
        {
            if (ballPos[1] >= player2Pos[1] && ballPos[1] <= player2Pos[1] + player2.GetLength())
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

    public Player GetPlayer1()
    {
        return player1;
    }

    public Player GetPlayer2()
    {
        return player2;
    }

    public Ball GetBall()
    {
        return ball;
    }

    static public int GetHeight()
    {
        return height;
    }

    static public int GetWidth()
    {
        return width;
    }

    public class Player(int[] position, int length, string name)
    {
        private int[] position = position;
        private int length = length;
        private string name = name;
        private int score = 0;

        public void Move(int y)
        {
            int newY = position[1] + y;

            if (buffer < newY && newY < height)
            {
                position[1] = newY;
            }
        }

        public void IncrementScore()
        {
            score++;
        }

        public int[] GetPosition()
        {
            return position;
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

    public class Ball(int[] position, double[] velocity)
    {
        private double[] position = [position[0], position[1]];
        private double[] velocity = velocity;

        public void Move()
        {
            position[0] += velocity[0];
            position[1] += velocity[1];
        }

        public void SetVelocity(double[] newVelocity)
        {
            velocity = newVelocity;
        }

        public int[] GetNextPosition()
        {
            return [(int)(position[0] + velocity[0]), (int)(position[1] + velocity[1])];
        }

        public int[] GetPosition()
        {
            return [(int)position[0], (int)position[1]];
        }

        public double[] GetVelocity()
        {
            return velocity;
        }
    }
}