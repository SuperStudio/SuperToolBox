using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperToolBox.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperToolBox.Test.SysInfo
{

    [TestClass]
    public class CPUParserTest
    {

        [TestMethod]
        [DataRow("12th Gen Intel(R) Core(TM) i5-12600KF", "i5-12600KF")]
        [DataRow("Intel(R) Core(TM) i5-10400 CPU @ 2.90GHz", "i5-10400")]
        [DataRow("AMD Athlon(tm) 64 X2 Dual Core Processor 4600+", "AMD Athlon(tm) 64 X2 Dual Core Processor 4600+")]
        public void TestCPU(string input, string output)
        {
            string value = BaseDeviceInfoView.ParseCPUID(input);
            Console.WriteLine(value);
            Assert.AreEqual(output, value); 
        }
    }
}
