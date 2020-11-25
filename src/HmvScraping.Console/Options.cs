namespace HmvScraping.Console
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using CommandLine;

    public class Options
    {
        [Option('k', "keyword", Required = false, HelpText = "Keyword to search")]
        public string Keyword { get; set; }

        [Option('f', "file", Required = false, HelpText = "File includes keywords")]
        public string File { get; set; }

        [Option('d', "directory", Required = true, HelpText = "Directory to put output files")]
        public string Directory { get; set; }
    }
}
