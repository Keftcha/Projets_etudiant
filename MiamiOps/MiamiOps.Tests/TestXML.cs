using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MiamiOps.Tests
{
    [TestFixture]
    public class TestXML
    {
        [Test]
        public void SimpleTest()
        {
            using (FileStream fs = File.OpenRead(@"..\..\..\..\MiamiOps.Map\Map\tilemap.tmx"))
            using (StreamReader sr = new StreamReader(fs, true))
            {
                XElement xml = XElement.Load(sr);
                string level = xml.Descendants("layer")
                                   .Single(l => l.Attribute("name").Value == "collide")
                                   .Element("data").Value;
                Console.WriteLine();
            }
        }
        [Test]
        public void TestXMLName()
        {
            using (FileStream fs = File.OpenRead(@"..\..\..\..\MiamiOps.Map\Map\tilemap.tmx"))
            using (StreamReader sr = new StreamReader(fs, true))
            {
                XElement xml = XElement.Load(sr);
                
               // Console.WriteLine(xml.Descendants("map"));
            }
        }

    }
}
