using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Pdoxcl2Sharp;

namespace EUIVParser.App
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Expected path to a .eu4 file.");
                return;
            }
            
            //dotnet publish -c Release --no-self-contained -r linux-x64
            //dotnet publish -c Release -r linux-x64

            var sw = new Stopwatch();
            sw.Start();
            Console.WriteLine("\nStart parsing\n");

            using var stream = OpenSaveGame(args[0]);
            var stats = ParadoxParser.Parse(stream, new Parser());

            Console.WriteLine($"\nParsing finished after {sw.Elapsed.TotalSeconds:N2}s");
            Console.WriteLine("\nStart exporting to json\n");
            sw.Restart();

            string json = JsonConvert.SerializeObject(stats, Formatting.Indented);

            string dir = Path.GetDirectoryName(args[0]);
            string fileName = Path.GetFileNameWithoutExtension(args[0]) + ".json";

            File.WriteAllText(Path.Combine(dir, fileName), json);

            Console.WriteLine($"\nParsing finished after {sw.Elapsed.TotalSeconds:N2}s");
        }

        private static Stream OpenSaveGame(string path)
        {
            const string uncompressedIdentifier = "EU4txt";
            const string gameStateIdentifier = "gamestate";
            const int bufferSize = 128;

            var stream = File.OpenRead(path);
            var streamReader = new StreamReader(stream, Encoding.UTF8, true, bufferSize);
            string firstLine = streamReader.ReadLine();

            stream.Position = 0;
            if (firstLine == uncompressedIdentifier)
            {
                return stream;
            }

            var archive = new ZipArchive(stream, ZipArchiveMode.Read);
            var entry = archive.Entries.FirstOrDefault(p => p.Name == gameStateIdentifier);
            if (entry == null)
                throw new InvalidOperationException("Invalid save game, 'gamestate' not found.");

            return entry.Open();
        }
    }
}