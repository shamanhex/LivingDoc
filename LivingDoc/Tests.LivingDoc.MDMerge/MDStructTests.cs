using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LivingDoc.MDMerge;
using NUnit.Framework;

namespace Tests.LivingDoc.MDMerge
{
    [TestFixture]
    public class MDStructTests
    {
        [Test]
        public void MDStruct_LoadFromXml_FileWithEmptyParagraphHeade_Exception()
        {
            XDocument xDoc = XDocument.Parse(@"
<paragraph>
</paragraph>");

            Assert.Catch<FormatException>(() => MDStruct.LoadFromXml(xDoc.Root));
        }

        [Test]
        public void MDStruct_LoadFromXml_SingleParagraph_Ok()
        {
            XDocument xDoc = XDocument.Parse(@"
<paragraph header='Спецификация'>
</paragraph>");

            MDStruct mdStruct = MDStruct.LoadFromXml(xDoc.Root);

            Assert.AreEqual("Спецификация", mdStruct.HeaderPattern);
            Assert.AreEqual(MDMergeStrategy.Union, mdStruct.MergeStrategy);
            Assert.AreEqual(0, mdStruct.Substruct.Count);
        }

        [Test]
        public void MDStruct_LoadFromXml_StrategyIsEmpty_Ok()
        {
            XDocument xDoc = XDocument.Parse(@"
<paragraph header='Спецификация' strategy=''>
</paragraph>");

            MDStruct mdStruct = MDStruct.LoadFromXml(xDoc.Root);

            Assert.AreEqual("Спецификация", mdStruct.HeaderPattern);
            Assert.AreEqual(MDMergeStrategy.Union, mdStruct.MergeStrategy);
            Assert.AreEqual(0, mdStruct.Substruct.Count);
        }

        [Test]
        public void MDStruct_LoadFromXml_StructWithSubstructures_Ok()
        {
            XDocument xDoc = XDocument.Parse(@"
<paragraph header='Спецификация'>
    <paragraph header='Общая информация' strategy='Union'/>
    <paragraph header='Требования к программному обеспечению' strategy='First' />
    <paragraph header='Требования к аппаратному обеспечению' strategy='SingleOnly' />
</paragraph>");

            MDStruct mdStruct = MDStruct.LoadFromXml(xDoc.Root);

            Assert.AreEqual("Спецификация", mdStruct.HeaderPattern);
            Assert.AreEqual(MDMergeStrategy.Union, mdStruct.MergeStrategy);
            Assert.AreEqual(3, mdStruct.Substruct.Count);

            MDStruct mdGeneralInfo = mdStruct.Substruct[0];
            Assert.AreEqual("Общая информация", mdGeneralInfo.HeaderPattern);
            Assert.AreEqual(MDMergeStrategy.Union, mdGeneralInfo.MergeStrategy);
            Assert.AreEqual(0, mdGeneralInfo.Substruct.Count);

            MDStruct mdSoftwareReq = mdStruct.Substruct[1];
            Assert.AreEqual("Требования к программному обеспечению", mdSoftwareReq.HeaderPattern);
            Assert.AreEqual(MDMergeStrategy.First, mdSoftwareReq.MergeStrategy);
            Assert.AreEqual(0, mdSoftwareReq.Substruct.Count);

            MDStruct mdHardwareReq = mdStruct.Substruct[2];
            Assert.AreEqual("Требования к аппаратному обеспечению", mdHardwareReq.HeaderPattern);
            Assert.AreEqual(MDMergeStrategy.SingleOnly, mdHardwareReq.MergeStrategy);
            Assert.AreEqual(0, mdHardwareReq.Substruct.Count);
        }
        
        [TestCase(true, "header [\\d]+", "Header 1")]
        public void MDStruct_IsMatch_Check(bool isMatch, string pattern, string header)
        {
            MDStruct mdStruct = new MDStruct();
            mdStruct.HeaderPattern = pattern;

            Assert.AreEqual(isMatch, mdStruct.IsMatchHeader(header));
        }      

    }
}

