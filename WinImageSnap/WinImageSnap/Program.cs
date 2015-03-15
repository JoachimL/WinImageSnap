using System.Collections.Generic;
using clipr;

namespace WinImageSnap
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = CliParser.Parse<Options>(args);
            new Snapper(new Configuration()
            {
                MaxSleepTime = options.MaxWaitTime,
                RepositoryName = options.RepositoryName,
                OutputFolder = options.OutputFolder
            }).Snap();
        }
    }

    [ApplicationInfo(Description = "The options for WinImageSnap.")]
    public class Options
    {
        [NamedArgument('r', "repositoryName", Action = ParseAction.Store,
            Description = "The name of the repository.")]
        public string RepositoryName { get; set; }

        [NamedArgument('w', "maxWaitTime", Action = ParseAction.Store,
            Description = "The maximum time to wait for a snap.")]
        public int MaxWaitTime { get; set; }

        [NamedArgument('o', "outputFolder", Action = ParseAction.Store,
            Description = "The output folder (Where snaps are stored).")]
        public string OutputFolder { get; set; }


    }
}
