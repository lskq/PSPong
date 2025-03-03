using System.Windows.Input;

namespace PSPong;

internal class Controller
{
    public Controller()
    {
        Model = new();
        View = new(Model);
    }

    public Model Model { get; init; }
    public View View { get; init; }

    public void Play()
    {
        Model.Ball1.SetRandomVelocity();

        do
        {
            int[] input = PlayerMovement();
            Model.Step(input[0], input[1]);
            View.Step();
        } while (Model.Winner() == null && !Keyboard.IsKeyDown(Key.Escape));

        View.ShowEndMessage();
        Thread.Sleep(100);

        do { } while (!Keyboard.IsKeyDown(Key.Escape));

        View.TearDown();
    }

    static public int[] PlayerMovement()
    {
        int player1YDelta = 0;
        int player2YDelta = 0;

        if (Keyboard.IsKeyDown(Key.Q))
            player1YDelta -= 1;

        if (Keyboard.IsKeyDown(Key.A))
            player1YDelta += 1;

        if (Keyboard.IsKeyDown(Key.Up))
            player2YDelta -= 1;

        if (Keyboard.IsKeyDown(Key.Down))
            player2YDelta += 1;

        return [player1YDelta, player2YDelta,];
    }

}