namespace PSPong;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        Controller controller = new();
        controller.Play();
    }
}