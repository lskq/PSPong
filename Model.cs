namespace PSPong;

class Model
{
    public class Game()
    {
        static private int height = Console.WindowHeight - 1;
        static private int width = Console.WindowWidth - 1;

        static private int playerLength = 3;
        static private int winningScore = 3;
        static private int startPosY = (height / 2) + 1;
        static private int[] player1StartPos = [1, startPosY];
        static private int[] player2StartPos = [width, startPosY];
        static private int[] ballStartPos = [width / 2, height / 2];

        private Player player1;
        private Player player2;
        private Ball ball;

        public void SetUp()
        {
            double[] ballStartVelocity = [0.0, 1.0];

            player1 = new(player1StartPos, playerLength, "Player 1");
            player2 = new(player2StartPos, playerLength, "Player 2");
            ball = new(ballStartPos, ballStartVelocity);
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
                position[1] += y;
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
}
