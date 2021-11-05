using System;
using System.IO;
using System.Runtime.Serialization.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OmahaMTG.Challenge.Challenges;

namespace CodeCampTest
{
    [TestClass]
    public class ExecutorTests
    {
        // Not real tests here (yet) - just a place to provide entry points in to the code.
        [TestMethod]
        public void FactoryProducesValidCodeCamp()
        {
            var codeCamp = CodeCampFactory.CreateRandomCodeCamp(400, 40, 8, 5);
            Assert.IsNotNull(codeCamp);
            Assert.IsNotNull(codeCamp.Attendees);
            Assert.IsNotNull(codeCamp.Sessions);
            Assert.IsNotNull(codeCamp.Speakers);
            Assert.IsNotNull(codeCamp.Rooms);
            Assert.IsNotNull(codeCamp.TimeSlots);
        }

        [TestMethod]
        public void ExecuteSample()
        {
            var impl = new CodeCampChallenge();
            var executor = new CodeCampExecutor();
            var results = executor.RunChallenge(impl);
            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void GenerateJson()
        {
            var codeCamp = CodeCampFactory.CreateRandomCodeCamp(100, 10, 2, 5);
            using (var fs = new FileStream("./codecamp2.txt", FileMode.OpenOrCreate))
            {
                var ser = new DataContractJsonSerializer(typeof (CodeCamp));
                ser.WriteObject(fs, codeCamp);
                fs.Flush();
            }
            Assert.IsTrue(true);
        }
    }
}
