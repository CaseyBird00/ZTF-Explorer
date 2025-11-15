using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTF_Explorer
{
    public class StarProcessing
    {


        static List<LightCurve> lightCurveQ = new List<LightCurve>();
        
        public void Start()
        {

        }
        public static void StartProcess(Star star)
        {
            SortLightgCurves(star.ObjID);
        }

        public static void SortLightgCurves(double objid)
        {
            for(int i = 0; i < Queue.LightCurveQ.Count; i++)
            {
                if (Queue.LightCurveQ[i].ObjID == objid)
                {
                    lightCurveQ.Add(Queue.LightCurveQ[i]);
                    Console.WriteLine(Queue.LightCurveQ[i]);
                }
            }
        }

    }
}
