using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivingDoc.MDMerge
{
    public class MDParagraph
    {
        public int Level { get; set; }

        public string Header { get; set; }

        public List<string> Text { get; set; }

        public List<MDParagraph> SubParagraphs { get; set; }

        public MDParagraph(int level)
        {
            Level = level;
            Header = string.Empty;
            Text = new List<string>();
            SubParagraphs = new List<MDParagraph>();
        }

        public MDParagraph(int level, string header)
        {
            Level = level;
            Header = header;
            Text = new List<string>();
            SubParagraphs = new List<MDParagraph>();
        }
                                                                
        public string[] GetLines()
        {
            string headerLine = "#".PadLeft(this.Level, '#') + " " + this.Header;

            List<string> lines = new List<string>();

            lines.Add(headerLine);
            lines.Add("");
            lines.AddRange(this.Text);
            lines.Add("");

            foreach (MDParagraph p in this.SubParagraphs)
            {
                lines.AddRange(p.GetLines());
            }

            return lines.ToArray();
        }
    }
}
