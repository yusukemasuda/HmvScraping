namespace HmvScraping.Console
{
    using System.IO;

    public interface IApplication
    {
        void Run(Options options);

        void Run(string outputDirectory, params string[] keywords);

        void Run(DirectoryInfo outputDirectory, params string[] keywords);
    }
}
