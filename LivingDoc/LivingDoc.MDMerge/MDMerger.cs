using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivingDoc.MDMerge
{
    public class MDMerger
    {
        public static MDParagraph Merge(MDStruct mdStruct, params MDParagraph[] paragraphs)
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

            merged.Text = MergeParagraphsText(paragraphs, mdStruct == null ? MDMergeStrategy.Union : mdStruct.MergeStrategy);

            UnionSubparagraphs(merged.SubParagraphs, paragraphs, mdStruct == null ? null : mdStruct.Substruct);

            return merged;
        }

        private static void UnionSubparagraphs(List<MDParagraph> subParagraphs, MDParagraph[] paragraphs, List<MDStruct> substruct)
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

            List<string> sortedHeaderList = totalHeaderList;

            if (substruct != null)
            {
                sortedHeaderList = new List<string>();

                foreach (MDStruct mdStruct in substruct)
                {
                    string[] matchHeaders = totalHeaderList.Where(header => mdStruct.IsMatchHeader(header)).ToArray();

                    if (matchHeaders.Length == 0)
                        continue;

                    sortedHeaderList.AddRange(matchHeaders);
                    totalHeaderList.RemoveAll(x => matchHeaders.Contains(x));
                }

                sortedHeaderList.AddRange(totalHeaderList);
            }

            foreach (string header in sortedHeaderList)
            {
                List<MDParagraph> allSubParagraphs = new List<MDParagraph>();
                foreach (MDParagraph p in paragraphs)
                {
                    allSubParagraphs.AddRange(p.SubParagraphs.Where(sp => string.Compare(sp.Header, header, ignoreCase: true) == 0));
                }
                subParagraphs.Add(Merge(null, allSubParagraphs.ToArray()));
            }
        }

        private static List<string> MergeParagraphsText(MDParagraph[] paragraphs, MDMergeStrategy strategy)
        {
            switch (strategy)
            {
                case MDMergeStrategy.Union:
                    {
                        return UnionParagraphsText(paragraphs);
                    }
                case MDMergeStrategy.First:
                    {
                        if (paragraphs.Length == 0)
                            return new List<string>();
                        IEnumerable<MDParagraph> nonEmptyPragraphs = paragraphs.Where(p => p.Text != null && p.Text.Count > 0);
                        if (nonEmptyPragraphs.Count() > 0)
                        {
                            return nonEmptyPragraphs.First().Text;
                        }
                        return new List<string>();
                    }
                case MDMergeStrategy.SingleOnly:
                    {
                        if (paragraphs.Length == 0)
                            return new List<string>();
                        IEnumerable<MDParagraph> nonEmptyPragraphs = paragraphs.Where(p => p.Text != null && p.Text.Count > 0);
                        if (nonEmptyPragraphs.Count() > 1)
                        {
                            throw new InvalidOperationException("Ошибка: Найдено более одного непутого абзаца для объединения. Стратегия: SingleOnly");
                        }
                        return nonEmptyPragraphs.First().Text;
                    }
                default: throw new NotImplementedException(string.Format("Стратегия {0} не реализована.", strategy));
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
