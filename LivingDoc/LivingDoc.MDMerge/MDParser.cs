using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LivingDoc.MDMerge
{
    public static class MDParser
    {
        public static Regex HeaderTmpl = new Regex(@"\s*(?<Marker>[#]+)\s*(?<HeaderText>.*)\s*", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);

        public static MDParagraph ParseMarkdown(string[] text)
        {

            int iLine = 0;

            //skip empty lines
            SkipEmptyLines(text, ref iLine);

            //parse empty lines
            MDParagraph markdown = ParseParagraph(text, ref iLine, 1);

            return markdown;
        }

        public static MDParagraph ParseParagraph(string[] text, ref int iLine, int level)
        {
            if (iLine >= text.Count())
                return null;

            MDParagraph paragraph = new MDParagraph(level);

            //parse header
            Match headerMatch = HeaderTmpl.Match(text[iLine]);
            if (!headerMatch.Success)
            {
                throw new FormatException(string.Format("Строка {0}: Ожидался заголовок.", iLine + 1));
            }
            string headerMark = headerMatch.Groups["Marker"].Value;
            if (headerMark.Length < level)
            {
                return null;
            }
            if (headerMark.Length != level)
            {
                throw new FormatException(string.Format("Строка {0}. Ожидался заголовок уровня: {1}", iLine + 1, level));
            }
            paragraph.Header = headerMatch.Groups["HeaderText"].Value.Trim().TrimEnd('#').TrimEnd();
            iLine++;

            SkipEmptyLines(text, ref iLine);

            //add all non header lines to paragraph text
            paragraph.Text.AddRange(ReadParagraphText(text, ref iLine));

            //parse subparagraphs
            while (true)
            {
                MDParagraph p = ParseParagraph(text, ref iLine, level + 1);

                if (p == null)
                {
                    break;
                }

                paragraph.SubParagraphs.Add(p);
            }

            return paragraph;
        }

        public static string[] ReadParagraphText(string[] text, ref int iLine)
        {

            List<string> paragraphText = new List<string>();

            //read to first header
            while (true)
            {
                if (iLine >= text.Count())
                {
                    break;
                }
                string line = text[iLine];
                if (!HeaderTmpl.IsMatch(line))
                {
                    paragraphText.Add(line);
                }
                else
                {
                    break;
                }
                iLine++;
            }

            //remove empty lines
            for (int i = paragraphText.Count - 1; i > 0; i--)
            {
                if (string.IsNullOrWhiteSpace(paragraphText[i]))
                {
                    paragraphText.RemoveAt(i);
                }
                else
                {
                    break;
                }
            }

            return paragraphText.ToArray();
        }

        private static void SkipEmptyLines(string[] text, ref int iLine)
        {
            for (; iLine < text.Length; iLine++)
            {
                if (!string.IsNullOrWhiteSpace(text[iLine]))
                    break;
            }
        }
    }
}
