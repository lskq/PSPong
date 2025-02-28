using System.Windows.Input;

namespace PSPong;

class Controller(Model model, View view)
{
    Model model = model;
    View view = view;

    public void Play()
    {
        do
        {
            int[] input = GetPlayerMovement();
            model.Step(input[0], input[1]);
            view.Step();
        } while (!model.HasWinner() && !Keyboard.IsKeyDown(Key.Escape));

        view.ShowEndMessage();
        Thread.Sleep(100);

        do { } while (!Keyboard.IsKeyDown(Key.Escape));

        view.TearDown();
    }

    public int[] GetPlayerMovement()
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