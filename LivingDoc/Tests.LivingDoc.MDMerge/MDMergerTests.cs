using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LivingDoc.MDMerge;
using NUnit.Framework;

namespace Tests.LivingDoc.MDMerge
{
    [TestFixture]
    public class MDMergerTests
    {
        [Test]
        public void MDMerger_MergeParagraph_tryMergeEmptyParagraphsList()
        {
            Assert.Catch<ArgumentNullException>(() => MDMerger.Merge(null, null));
        }

        [Test]
        public void MDMerger_MergeParagraph_tryMergeParagraphsWithDiffLevel()
        {
            MDParagraph p1 = new MDParagraph(1, "Header 1");
            MDParagraph p2 = new MDParagraph(2, "Header 1");

            Assert.Catch<ArgumentException>(() => MDMerger.Merge(null, p1, p2), "Все переданные параграфы должны иметь одинаковый уровень.");
        }

        [Test]
        public void MDMerger_MergeParagraph_tryMergeParagraphsWithDiffHeader()
        {
            MDParagraph p1 = new MDParagraph(1, "Header 1");
            MDParagraph p2 = new MDParagraph(1, "Header 2");

            Assert.Catch<ArgumentException>(() => MDMerger.Merge(null, p1, p2), "Все переданные параграфы должны иметь одинаковый заголовок.");
        }

        [Test]
        public void MDMerger_MergeParagraph_unionParagraphText()
        {
            MDParagraph p1 = new MDParagraph(1, "Header 1");
            p1.Text.Add("Main text (first)");

            MDParagraph p2 = new MDParagraph(1, "Header 1");
            p2.Text.Add("Main text (second)");

            MDParagraph mdMerged = MDMerger.Merge(null, p1, p2);

            Assert.AreEqual(1, mdMerged.Level);
            Assert.AreEqual("Header 1", mdMerged.Header);
            CollectionAssert.AreEqual(new string[] { "Main text (first)", "", "Main text (second)" }, mdMerged.Text.ToArray());
        }

        [Test]
        public void MDMerger_MergeParagraph_unionSimilarSubParagraphs()
        {
            MDParagraph p1 = new MDParagraph(1, "Header 1");
            p1.SubParagraphs.Add(new MDParagraph(2, "Header 1.1"));

            MDParagraph p2 = new MDParagraph(1, "Header 1");
            p2.SubParagraphs.Add(new MDParagraph(2, "Header 1.1"));

            MDParagraph mdMerged = MDMerger.Merge(null, p1, p2);

            Assert.AreEqual(1, mdMerged.Level);
            Assert.AreEqual("Header 1", mdMerged.Header);
            Assert.AreEqual(1, mdMerged.SubParagraphs.Count);
            Assert.AreEqual("Header 1.1", mdMerged.SubParagraphs[0].Header);
        }
    }
}
