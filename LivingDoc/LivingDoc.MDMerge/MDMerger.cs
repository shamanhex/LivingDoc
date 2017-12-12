using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivingDoc.MDMerge
{
    public class MDMerger
    {
        public static MDParagraph Merge(string mdStruct, params MDParagraph[] paragraphs)
        {
            if (paragraphs == null || paragraphs.Length == 0)
            {
                throw new ArgumentNullException("paragraphs", "Список объединяемых текстов не может быть пустым.");
            }

            if (paragraphs.Length == 1)
            {
                return paragraphs[0];
            }

            int level = paragraphs[0].Level;
            string header = paragraphs[0].Header;
            foreach (MDParagraph p in paragraphs)
            {
                if (p.Level != level)
                {
                    throw new ArgumentException("Все параграфы должны иметь одинаковый уровень.");
                }
                if (p.Header != header)
                {
                    throw new ArgumentException("Все параграфы должны иметь одинаковый заголовок.");
                }
            }

            MDParagraph merged = new MDParagraph(level, header);

            merged.Text = UnionParagraphsText(paragraphs);

            UnionSubparagraphs(merged.SubParagraphs, paragraphs);

            return merged;
        }

        private static void UnionSubparagraphs(List<MDParagraph> subParagraphs, MDParagraph[] paragraphs)
        {
            List<string> totalHeaderList = new List<string>();

            //Collect all unique headers for all subparagraphs 
            foreach (MDParagraph p in paragraphs)
            {
                foreach (MDParagraph subParagraph in p.SubParagraphs)
                {
                    if (!totalHeaderList.Contains(subParagraph.Header))
                    {
                        totalHeaderList.Add(subParagraph.Header);
                    }
                }
            }

            foreach (string header in totalHeaderList)
            {
                List<MDParagraph> allSubParagraphs = new List<MDParagraph>();
                foreach (MDParagraph p in paragraphs)
                {
                    allSubParagraphs.AddRange(p.SubParagraphs.Where(sp => string.Compare(sp.Header, header, ignoreCase: true) == 0));
                }
                subParagraphs.Add(Merge(null, allSubParagraphs.ToArray()));
            }
        }

        private static List<string> UnionParagraphsText(MDParagraph[] paragraphs)
        {
            List<string> text = new List<string>();

            foreach (MDParagraph p in paragraphs)
            {
                if (p.Text != null && p.Text.Count > 0)
                {
                    text.AddRange(p.Text);
                    text.Add("");
                }
            }

            if (text.Count > 0)
            {
                text.RemoveAt(text.Count - 1);
            }

            return text;
        }
    }
}
