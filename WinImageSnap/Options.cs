using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using clipr;

namespace WinImageSnap
{
    [ApplicationInfo(Description = "The options for WinImageSnap.")]
    public class Options
    {
        [NamedArgument('c', "cameraNames", Action = ParseAction.Store,
            Description = "The device names to use for snapping, in preferred order.")]
        public string CameraNames { get; set; }

        [NamedArgument('b', "branch", Action = ParseAction.Store,
            Description = "The branch name (used for the watermark).")]
        public string Branch { get; set; }

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

        [NamedArgument('l', "list", Action = ParseAction.StoreTrue,
            Description = "List all detected cameras.")]
        public bool List { get; set; }
    }
}
