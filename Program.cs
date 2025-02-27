namespace PSPong;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        Model model = new Model();
        model.SetUp();

        View view = new View(model);
        view.SetUp();

        string? winner = null;
        do
        {
            model.Step(0, 0);
            view.Step();
            winner = model.GetWinner();
        } while (winner == null);
    }
}