using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace RedSkyProjectTesting
{

    [TestFixture]
    public class TestFoo
    {
        
        [Test]
        public void TestFooScript()
        {
            TestScript t = new TestScript();
			Console.WriteLine("First Test");
            Assert.IsTrue(t.TestSomething());            
        }

        [Test]
        public void TestMissileLoad()
        {
            Missile m = new Missile();
            Assert.IsNotNull(m);
        }

        [Test]
        public void VelocityAngle()
        {
            TestScript t = new TestScript();
            Assert.AreEqual(90, t.TestAngleFunction(), string.Format("%.3f", t.TestAngleFunction()));
            
        }
    }
}
