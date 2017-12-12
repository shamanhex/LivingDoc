using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivingDoc.MDMerge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string outputMarkdownPath = args[0];

            string[] mdPaths = args.Skip(1).ToArray();

            Console.WriteLine("Read markdown files:");

            List<MDParagraph> markdowns = new List<MDParagraph>();

            foreach (string mdPath in mdPaths)
            {                    
                string fileName = Path.GetFileName(mdPath);

                Console.WriteLine("{0}: Read lines...", fileName);

                string[] mdLines = File.ReadAllLines(mdPath);

                Console.WriteLine("{0}: Parse markdown...", fileName);

                MDParagraph markdown = MDParser.ParseMarkdown(mdLines);     
                markdowns.Add(markdown);
            }

            Console.WriteLine("Merge files...");

            MDParagraph merged = MDMerger.Merge(null, markdowns.ToArray());

            Console.WriteLine("Write merged Markdown to {0}", Path.GetFileName(outputMarkdownPath));

            File.WriteAllLines(outputMarkdownPath, merged.GetLines());

        }
    }
}
