using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LivingDoc.MDMerge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MergeParams options;
            try
            {
                options = ParseArgs(args);
            }
            catch (ArgumentException argEx)
            {
                Console.WriteLine("Ошибка: ", argEx.Message);
                return;
            }
            
            Console.WriteLine("Read markdown files:");

            List<MDParagraph> markdowns = new List<MDParagraph>();

            foreach (string mdPath in options.InputPaths)
            {                    
                string fileName = Path.GetFileName(mdPath);

                Console.WriteLine("{0}: Read lines...", fileName);

                string[] mdLines = File.ReadAllLines(mdPath);

                Console.WriteLine("{0}: Parse markdown...", fileName);

                MDParagraph markdown = MDParser.ParseMarkdown(mdLines);     
                markdowns.Add(markdown);
            }

            MDStruct mdStruct = null;
            if (!string.IsNullOrEmpty(options.StructPath))
            {
                Console.WriteLine("Load struct");

                using (Stream fMdStruct = File.Open(options.StructPath, FileMode.Open))
                {
                    XDocument mdStructXml = XDocument.Load(fMdStruct);
                    mdStruct = MDStruct.LoadFromXml(mdStructXml.Root);
                }          
            }
            
            Console.WriteLine("Merge files...");

            MDParagraph merged = MDMerger.Merge(mdStruct, markdowns.ToArray());

            Console.WriteLine("Write merged Markdown to {0}", Path.GetFileName(options.ResultPath));

            File.WriteAllLines(options.ResultPath, merged.GetLines());

        }

        public static MergeParams ParseArgs(string[] args)
        {
            if (args.Length < 3)
            {
                throw new ArgumentException("Ошибка кол-ва аргументов. Формат вызова: [-s struct.md.xml] result.md input1.md input2.md ... ");
            }

            MergeParams options = new MergeParams();
                                         
            string firstParam = args[0].ToLower();
            if (firstParam == "-s" || firstParam == "--s" || firstParam == "-struct" || firstParam == "--struct")
            {
                if (args.Length < 5)
                {
                    throw new ArgumentException("Ошибка кол-ва аргументов. Формат вызова: [-s struct.md.xml] result.md input1.md input2.md ... ");
                }

                options.StructPath = args[1];
                options.ResultPath = args[2];
                options.InputPaths = args.Skip(3).ToArray();
            }
            else
            {                                  
                options.ResultPath = args[0];
                options.InputPaths = args.Skip(1).ToArray();
            }   

            return options;
        }
    }
}

