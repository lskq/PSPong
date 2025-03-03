namespace PSPong;

internal class Model()
{
    public static int Buffer { get; } = 1;
    public static int Height { get; } = Console.WindowHeight - 1;
    public static int Width { get; } = Console.WindowWidth - 1;

    public static int PlayerLength { get; } = 3;

    public static int Player1StartPosX { get; } = 1;
    public static int Player2StartPosX { get; } = Width - 1;
    public static int PlayersStartPosY { get; } = (Height / 2) - 1;

    public static int BallStartPosX { get; } = Width / 2;
    public static int BallStartPosY { get; } = Height / 2;

    public static double BaseXSpeed { get; } = Width / 100.0;
    public static double BaseYSpeed { get; } = Height / 20.0;

    public int WinningScore { get; set; } = 3;

    public Player Player1 { get; init; } = new(Player1StartPosX, PlayersStartPosY, "Player 1");
    public Player Player2 { get; init; } = new(Player2StartPosX, PlayersStartPosY, "Player 2");
    public Ball Ball1 { get; init; } = new();

    public void Step(int player1Delta, int player2Delta)
    {
        Player1.Move(player1Delta);
        Player2.Move(player2Delta);
        MoveBall();
    }

    public void MoveBall()
    {
        int ballX = (int)Ball1.X;

        if (ballX == 0)
        {
            //scored on player 1
            Player2.Score += 1; ;
            Ball1.Reset();
            return;
        }
        else if (ballX == Width)
        {
            //scored on player2
            Player1.Score += 1; ;
            Ball1.Reset();
            return;
        }

        if (Ball1.Bounced)
        {
            Ball1.Move();
            Ball1.Bounced = false;
            return;
        }

        int player1X = Player1.X;
        int player1Y = Player1.Y;

        int player2X = Player2.X;
        int player2Y = Player2.Y;

        int ballNextX = (int)Ball1.NextX;
        int ballNextY = (int)Ball1.NextY;

        // Collision-checking conditional from hell
        if (ballNextY < Buffer)
        {
            //About to hit the top
            Ball1.MoveY(Buffer);
            Ball1.VelocityY *= -1;
            Ball1.Bounced = true;
        }
        else if (ballNextY > Height)
        {
            //About to hit the bottom
            Ball1.MoveY(Height);
            Ball1.VelocityY *= -1;
            Ball1.Bounced = true;
        }

        if (ballNextX <= player1X && player1Y <= ballNextY && ballNextY <= player1Y + Player1.Tail)
        {
            //About to bounce off player 1
            Ball1.MoveX(Player1.X + 1);
            Ball1.VelocityX *= -1;
            Ball1.Bounced = true;
            return;
        }
        else if (ballNextX >= player2X && player2Y <= ballNextY && ballNextY <= player2Y + Player2.Tail)
        {
            //about to bounce off player 2
            Ball1.MoveX(Player2.X - 1);
            Ball1.VelocityX *= -1;
            Ball1.Bounced = true;
            return;
        }

        if (ballNextX <= 0)
        {
            //about to score on player 1
            Ball1.MoveX(0);
            Ball1.ChangeVelocity([0, 0]);
        }
        else if (ballNextX >= Width)
        {
            //about to score on player 2
            Ball1.MoveX(Width);
            Ball1.ChangeVelocity([0, 0]);
        }
        else if (!Ball1.Bounced)
        {
            Ball1.Move();
        }
    }

    public string? Winner()
    {
        if (Player1.Score >= WinningScore)
        {
            return Player1.Name;
        }
        if (Player2.Score >= WinningScore)
        {
            return Player2.Name;
        }
        else
        {
            return null;
        }
    }

    public Player[] Players()
    {
        return [Player1, Player2];
    }

    public class Player(int x, int y, string name)
    {
        public int X { get; set; } = x;
        public int Y { get; set; } = y;
        public int OldY { get; set; } = y;
        public string Name { get; init; } = name;
        public int Length { get; init; } = PlayerLength;
        public int Score { get; set; } = 0;

        public int Tail => Length - 1;

        public void Move(int yMove)
        {
            int newY = Y + yMove;

            if (Buffer <= newY && newY <= Height - Length + 1)
            {
                OldY = Y;
                Y = newY;
            }
        }
    }

    public class Ball
    {
        public Ball()
        {
            Reset();
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double OldX { get; set; }
        public double OldY { get; set; }
        public double VelocityX { get; set; }
        public double VelocityY { get; set; }
        public bool Bounced { get; set; }

        public double NextX => X + VelocityX;
        public double NextY => Y + VelocityY;

        public void Move()
        {
            RememberPosition();
            X += VelocityX;
            Y += VelocityY;
        }

        public void Reset()
        {
            SetPosition(BallStartPosX, BallStartPosY);
            SetRandomVelocity();
            Bounced = false;
        }

        public void SetRandomVelocity()
        {
            Random random = new();

            VelocityX = BaseXSpeed * (random.NextDouble() + 1) * (random.Next(2) == 0 ? -1 : 1);
            VelocityY = BaseYSpeed * (random.NextDouble() + 1) * (random.Next(2) == 0 ? -1 : 1);
        }

        public void RememberPosition()
        {
            OldX = X;
            OldY = Y;
        }

        public void SetPosition(int newX, int newY)
        {
            RememberPosition();
            X = newX;
            Y = newY;

        }

        public void MoveX(int newX)
        {
            RememberPosition();
            X = newX;
        }

        public void MoveY(int newY)
        {
            RememberPosition();
            Y = newY;
        }

        public void ChangeVelocity(double[] newVelocity)
        {
            VelocityX = newVelocity[0];
            VelocityY = newVelocity[1];
        }
    }
}