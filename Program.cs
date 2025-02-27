namespace PSPong;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        Model model = new Model();
        View view = new View(model);
        view.SetUp();

        Controller controller = new Controller(model, view);
        controller.Play();
    }
}