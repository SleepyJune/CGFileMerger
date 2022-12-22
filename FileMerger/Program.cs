﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileMerger
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Directory.GetCurrentDirectory();
            string[] files = System.IO.Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories);

            string outputPath = Directory.GetCurrentDirectory() + "\\output.cs";

            File.Create(outputPath).Close();

            foreach (var file in files)
            {
                if (file.Contains("obj\\") || file.Contains("bin\\"))
                {
                    continue;
                }

                string[] lines = File.ReadAllLines(file);
                int namespaceIndex = 0;

                for (int i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];

                    if (line.Contains("namespace") && !Path.GetFileName(file).Contains("Program.cs"))
                    {
                        namespaceIndex = i+2;
                        break;
                    }
                }

                var linesToAppend = lines.Skip(namespaceIndex).Take(lines.Length - namespaceIndex - 1);

                File.AppendAllLines(outputPath, linesToAppend);
                File.AppendAllLines(outputPath, new List<string> { "" });
            }

            File.AppendAllLines(outputPath, new List<string> { "}" });
        }
    }
}
