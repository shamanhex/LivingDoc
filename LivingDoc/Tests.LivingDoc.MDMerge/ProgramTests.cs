using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LivingDoc.MDMerge;
using NUnit.Framework;     
using System.IO;

namespace Tests.LivingDoc.MDMerge
{
    [TestFixture]
    class ProgramTests
    {
        [Test]
        public void Program_Main_SimpleMerge()
        {
            string workingFolder = TestContext.CurrentContext.WorkDirectory;

            File.WriteAllLines(Path.Combine(workingFolder,"Part1.md"), new string[] {
                "# Header 1",
                "",
                "Text 1",
                "",
                "## Header 1.1", 
                "",
                "Text 1.1"
            });

            File.WriteAllLines(Path.Combine(workingFolder, "Part2.md"), new string[] {
                "# Header 1",
                "",               
                "## Header 1.2",
                "",
                "Text 1.2"
            });             

            Program.Main(new string[] { Path.Combine(workingFolder, "Result.md"),
                                        Path.Combine(workingFolder, "Part1.md"),
                                        Path.Combine(workingFolder, "Part2.md") });

            string[] result = File.ReadAllLines(Path.Combine(workingFolder, "Result.md"));

            CollectionAssert.AreEqual(new string[] {
                "# Header 1",
                "",
                "Text 1",
                "",
                "## Header 1.1",
                "",
                "Text 1.1",
                "",
                "## Header 1.2",
                "",
                "Text 1.2",
                ""
            }, result);      
        }
    }
}
