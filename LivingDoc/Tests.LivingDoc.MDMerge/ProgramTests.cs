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

        [Test]
        public void Program_Main_MergeByStructure()
        {
            string workingFolder = TestContext.CurrentContext.WorkDirectory;

            File.WriteAllLines(Path.Combine(workingFolder, "struct.md.xml"), new string[] {
 "<paragraph header='Header'>",
 "   <paragraph header='Header 1' strategy='Union'/>",
 "   <paragraph header='Header 2' strategy='Union'/>",
 "   <paragraph header='Default header' strategy='First' />",     
 "</paragraph>"});

            File.WriteAllLines(Path.Combine(workingFolder, "Part1.md"), new string[] {
                "# Header",
                "",
                "Main text",
                "",
                "## Header 2",
                "",
                "Text 2"
            });

            File.WriteAllLines(Path.Combine(workingFolder, "Part2.md"), new string[] {
                "# Header",
                "",
                "## Header 1",
                "",
                "Text 1"
            });

            Program.Main(new string[] { "-s",
                                        Path.Combine(workingFolder, "struct.md.xml"),
                                        Path.Combine(workingFolder, "Result.md"),
                                        Path.Combine(workingFolder, "Part1.md"),
                                        Path.Combine(workingFolder, "Part2.md") });

            string[] result = File.ReadAllLines(Path.Combine(workingFolder, "Result.md"));

            CollectionAssert.AreEqual(new string[] {
                "# Header",
                "",
                "Main text",
                "",
                "## Header 1",
                "",
                "Text 1",
                "",
                "## Header 2",
                "",
                "Text 2",
                ""
            }, result);
        }

        [Test]
        public void Program_ParseArgs_TooFewArgs_Exception()
        {
            Assert.Catch<ArgumentException>(() => Program.ParseArgs(new string[] { "1", "2" }));
        }

        [Test]
        public void Program_ParseArgs_TooFewArgsWithStructParam_Exception()
        {
            Assert.Catch<ArgumentException>(() => Program.ParseArgs(new string[] { "-s", "2", "3", "4" }));
        }

        [Test]
        public void Program_ParseArgs_WoStructParam_OK()
        {
            MergeParams options = Program.ParseArgs(new string[] { "result.md", "text1.md", "text2.md", "text3.md" });

            Assert.IsNull(options.StructPath);
            Assert.AreEqual("result.md", options.ResultPath);
            CollectionAssert.AreEqual(new string[] { "text1.md", "text2.md", "text3.md" }, options.InputPaths);
        }

        [Test]
        public void Program_ParseArgs_WithStructParam_OK()
        {
            MergeParams options = Program.ParseArgs(new string[] { "--struct", "struct.md.xml", "result.md", "text1.md", "text2.md", "text3.md" });

            Assert.AreEqual("struct.md.xml", options.StructPath);
            Assert.AreEqual("result.md", options.ResultPath);
            CollectionAssert.AreEqual(new string[] { "text1.md", "text2.md", "text3.md" }, options.InputPaths);
        }
    }
}
