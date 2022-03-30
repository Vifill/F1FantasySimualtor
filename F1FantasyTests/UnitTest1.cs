using F1FantasySim;
using F1FantasySim.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace F1FantasyTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            List<ApiModel> players;
            using (StreamReader r = new StreamReader("ApiModelInput.json"))
            {
                players = JsonConvert.DeserializeObject<Players>(r.ReadToEnd()).players;
            }




            //Simulator sim = new Simulator()
        }
    }
}