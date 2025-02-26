namespace PSPong;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        Model game = new Model();

        game.SetUp();

        string? winner = null;

        do
        {
            game.Play(0, 0);
            winner = game.GetWinner();
        } while (winner == null);
    }
}