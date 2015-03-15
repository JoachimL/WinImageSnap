using clipr;

namespace WinImageSnap
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = CliParser.Parse<Options>(args);
            new Snapper(new Configuration
            {
                MaxSleepTime = options.MaxWaitTime,
                RepositoryName = options.RepositoryName,
                OutputFolder = options.OutputFolder,
                Verbose = options.Verbose,
                CameraNames = options.CameraNames
            }).Snap();
        }
    }

    [ApplicationInfo(Description = "The options for WinImageSnap.")]
    public class Options
    {
        [NamedArgument('c', "cameraNames", Action = ParseAction.Store,
            Description = "The device names to use for snapping, in preferred order.")]
        public string CameraNames { get; set; }

        [NamedArgument('r', "repositoryName", Action = ParseAction.Store,
            Description = "The name of the repository.")]
        public string RepositoryName { get; set; }

        [NamedArgument('w', "maxWaitTime", Action = ParseAction.Store,
            Description = "The maximum time to wait for a snap.")]
        public int MaxWaitTime { get; set; }

        [NamedArgument('o', "outputFolder", Action = ParseAction.Store,
            Description = "The output folder (where snaps are stored). Defaults to c:\\temp")]
        public string OutputFolder { get; set; }

        [NamedArgument('v', "verbose", Action = ParseAction.StoreTrue,
            Description = "Add for verbose output.")]
        public bool Verbose { get; set; }


    }
}
