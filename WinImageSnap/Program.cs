using clipr;

namespace WinImageSnap
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = CliParser.Parse<Options>(args);
            if (options.List)
                new Snapper(options).ListCameras();
            else
                new Snapper(options).Snap();
        }
    }
}
