using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UnityEngine;




namespace RedSkyProjectTesting
{
    [TestFixture]
    public class MissileFunctionTesting
    {
        Missile m;
        
        [TestFixtureSetUp]
        public void Init()
        {
           m = new Missile();
        }

        [Test]
        public void Test_That_Missile_is_Loading()
        {
            Assert.IsNotNull(m = new Missile());
        }
        
        [Test]
        public void Test_That_Missile_Can_Calculate_Targets_Velocity_and_Speed_Based_On_Two_Recorded_Position_Coords()
        {
            //While missile is in a stationary position, feed in two recorded positions of the target and test
            //that the missile can calculate the velocity of its target correctly.
                        
            Vector3 targetVectorExpected = new Vector3(3, 0, 0);
            float targetVectMagnitudeExpected = 3f;
            float targetSpeedExpected = 180;
            float targetSpeedExpectedTolerence = 0.0001f;                    
            
            m.oldMissilePosition = new Vector3(0, 0, 0);
            m.newMissilePosition = new Vector3(0, 0, 0);

            m.oldTargetPosition = new Vector3(0, 1, 0);
            m.TargetPosition = new Vector3(3, 1, 0);

            m.timeSinceLastUpdate = 1f / 60f; //assume 1 frame which took 1/60th of a second

            m.CalculateTargetsVelocityandSpeed();

            Assert.AreEqual(targetVectorExpected, m.TargetVelocityVector);
            Assert.AreEqual(targetVectMagnitudeExpected, Vector3.Magnitude(m.TargetVelocityVector));
            Assert.AreEqual(targetSpeedExpected, m.TargetSpeedMetersPerSecond, targetSpeedExpectedTolerence);
        }

        [Test]
        public void Test_If_Target_Is_Travelling_a_Linear_Course()
        {

            //*----*----*----* Known linear path

            Vector3 pos1 = new Vector3(0, 1, 0);
            Vector3 pos2 = new Vector3(1, 1, 0);
            Vector3 pos3 = new Vector3(2, 1, 0);
            Vector3 pos4 = new Vector3(3, 1, 0);

            Vector3 vel1 = pos2 - pos1;
            Vector3 vel2 = pos3 - pos2;
            Vector3 vel3 = pos4 - pos3;
                        
            m.FlightPath.Add(vel1);
            m.FlightPath.Add(vel2);
            m.FlightPath.Add(vel3);
            
            Assert.IsTrue(m.IsTargetCourseLinear(), string.Format("{0} {1} {2} {3}" ,m.FlightPath[m.FlightPath.Count -1], m.FlightPath[m.FlightPath.Count -2], m.FlightPath[m.FlightPath.Count -3],  m.FlightPath.Count.ToString()));            
            
        }

        [Test]
        public void Test_If_Target_Is_Travelling_a_NON_Linear_Course()
        {
            //          *
            //          |
            //*----*----* Known non linear path

            Vector3 pos1 = new Vector3(0, 1, 0);
            Vector3 pos2 = new Vector3(1, 1, 0);
            Vector3 pos3 = new Vector3(2, 1, 0);
            Vector3 pos4 = new Vector3(3, 2, 0);

            Vector3 vel1 = pos2 - pos1;
            Vector3 vel2 = pos3 - pos2;
            Vector3 vel3 = pos4 - pos3;
            
            m.FlightPath.Add(vel1);
            m.FlightPath.Add(vel2);
            m.FlightPath.Add(vel3);
            
            Assert.IsFalse(m.IsTargetCourseLinear(), string.Format("{0} {1} {2} {3}", m.FlightPath[m.FlightPath.Count - 1], m.FlightPath[m.FlightPath.Count - 2], m.FlightPath[m.FlightPath.Count - 3], m.FlightPath.Count.ToString()));

        }

        
        [Test]
        public void Test_that_Missile_Can_Calculate_Fuel_Cost_Of_Taking_a_Route_To_Target()
        {
            //Based on a intercept path test if the missile can calculate the fuel it requires to travel that distance.
            Assert.Fail("Not yet implemented");
        }

        [Test]
        public void Test_That_Missile_Is_Recording_Its_FuelLoad()
        {
            // The missiles fuel must be updated constantly
            Assert.Fail("Not yet implemented");
        }

        [Test]
        public void Test_PredictIntercept()
        {
            Vector3 expected = new Vector3(35.53454f, 44.04605f, 19.93092f);
                          
            m.newMissilePosition = new Vector3(0, 0, 0); //   B

            m.TargetPosition = new Vector3(150, 200, -300); // A
						
            m.TargetVelocityVector = new Vector3(34, 42, 23); // Av

            m.MaxSpeed = 60f; 

            Vector3 intercept = m.CalculateInterceptVector();

			Console.WriteLine(intercept);

            Assert.AreEqual(expected.x, intercept.x, 0.001f);
            Assert.AreEqual(expected.y, intercept.y, 0.001f);
            Assert.AreEqual(expected.z, intercept.z, 0.001f);
            
        }

        [Test]
        public void Test_That_Missile_Is_Plotting_Course_Correctly()
        {
            //Test that the missile has the correct path and satisfies the fuel required demand before plotting its course.
            Assert.Fail("Not yet implemented");
        }

        [Test]
        public void Test_If_Missile_Is_In_Detonation_Range()
        {
            // Test the missile is in close enough proximity that it can afford to detonate

            m.newMissilePosition = new Vector3(0, 1, 0);

            m.TargetPosition = new Vector3(29, 1, 0);

            Assert.IsTrue(m.InDetonationRange(), string.Format("{0}", Vector3.Distance(m.newMissilePosition, m.TargetPosition)));
            
        }

        [Test]
        public void Test_If_Missile_Is_NOT_In_Detonation_Range()
        {
            // Test the missile is in close enough proximity that it can afford to detonate

            m.newMissilePosition = new Vector3(0, 1, 0);

            m.TargetPosition = new Vector3(129, 1, 0);

            Assert.IsFalse(m.InDetonationRange(), string.Format("{0}", Vector3.Distance(m.newMissilePosition, m.TargetPosition)));

        }

        [Test]
        public void Test_That_Low_Fuel_Function_is_Working_Correctly()
        {
           
            Assert.Fail("Not yet implemented");
        }


    }
}
