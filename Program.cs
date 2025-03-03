namespace PSPong;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        Model model = new();
        View view = new(model);
        Controller controller = new(model, view);

        controller.Play();
    }
}