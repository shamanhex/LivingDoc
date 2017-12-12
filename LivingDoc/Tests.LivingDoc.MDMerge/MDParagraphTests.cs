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
    class MDParagraphTests
    {
        [Test]
        public void MDParagraph_GetLines_retLines()
        {
            MDParagraph p = new MDParagraph(2, "Header");
            p.Text.Add("Line 1");
            p.Text.Add("Line 2");
            
            string[] lines = p.GetLines();
                                                 
            CollectionAssert.AreEqual(new string[] {
                "## Header",
                "",
                "Line 1",
                "Line 2",
                ""
            }, lines);
        }
    }
}
