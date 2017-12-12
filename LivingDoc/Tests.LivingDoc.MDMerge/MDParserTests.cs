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
    public class MDParserTests
    {
        [Test]
        public void MDParser_ParseMarkdown_TextWoHeader()
        {
            Assert.Catch<FormatException>(() =>
                MDParser.ParseMarkdown(new string[] { "No header" })
            );
        }

        [Test]
        public void MDParser_ParseMarkdown_NotFirstHeader()
        {
            Assert.Catch<FormatException>(() =>
                MDParser.ParseMarkdown(new string[] { "## Header" })
            );
        }

        [Test]
        public void MDParser_ParseMarkdown_ParseHeader()
        {
            Assert.AreEqual("Header 1", MDParser.ParseMarkdown(new string[] { "# Header 1 #" }).Header);
        }

        [Test]
        public void MDParser_ParseMarkdown_Parse1paragraph()
        {
            string[] text = new string[]
            {
                "",
                "# Header",
                "",
                "Paragraph text",
                ""
            };

            CollectionAssert.AreEqual(new string[] { "Paragraph text" }, MDParser.ParseMarkdown(text).Text.ToArray());
        }

        [Test]
        public void MDParser_ParseParagraph_ComplexTest()
        {
            string[] text = new string[]
            {
                "# Header 1",
                "",
                "## Header 1.1",
                "",
                "Text 1.1",
                "",
                "## Header 1.2"
            };

            int iLine = 0;

            MDParagraph paragraph = MDParser.ParseParagraph(text, ref iLine, 1);
            Assert.AreEqual("Header 1", paragraph.Header);
            Assert.AreEqual(0, paragraph.Text.Count);
            Assert.AreEqual(1, paragraph.Level);
            Assert.AreEqual(2, paragraph.SubParagraphs.Count);

            MDParagraph p11 = paragraph.SubParagraphs[0];
            Assert.AreEqual("Header 1.1", p11.Header);
            Assert.AreEqual(1, p11.Text.Count);
            Assert.AreEqual("Text 1.1", p11.Text[0]);
            Assert.AreEqual(2, p11.Level);
            Assert.AreEqual(0, p11.SubParagraphs.Count);

            MDParagraph p12 = paragraph.SubParagraphs[1];
            Assert.AreEqual("Header 1.2", p12.Header);
            Assert.AreEqual(0, p12.Text.Count);
            Assert.AreEqual(2, p12.Level);
            Assert.AreEqual(0, p12.SubParagraphs.Count);
        }
    }
}
