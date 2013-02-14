using System.Collections;

public class TestScript{

	
    public bool TestSomething()
    {
        return true;
    }

    public bool MissileLoads()
    {
        Missile m = new Missile();

        if (m == null)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    public float TestAngleFunction()
    {
        UnityEngine.Vector3 p1 = new UnityEngine.Vector3(0, 1, 0);
        UnityEngine.Vector3 p2 = new UnityEngine.Vector3(1, 1, 0);
        UnityEngine.Vector3 vel1 = p2 - p1;
               
        UnityEngine.Vector3 p3 = new UnityEngine.Vector3(1, 2, 0);
        UnityEngine.Vector3 vel2 = p3 - p2;

        return UnityEngine.Vector3.Angle(vel1, vel2);

    }
}
