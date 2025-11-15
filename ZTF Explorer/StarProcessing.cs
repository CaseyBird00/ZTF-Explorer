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
            SortLightCurves(star.ObjID);
            CompareMagnitudes();
        }

        public static void CompareMagnitudes()
        {
            if (lightCurveQ != null)
            {
                for (int i = 1; i < lightCurveQ.Count; i++)
                {
                    var prev = lightCurveQ[i - 1];
                    var curr = lightCurveQ[i];

                    var diff = Math.Abs(curr.Mag - prev.Mag);

                    if (diff > curr.Magerr)
                    {
                        Console.WriteLine($"Significant change detected for ObjID {curr.ObjID} at Hmjd {curr.Hmjd}: Mag changed from {prev.Mag} to {curr.Mag} (Diff: {diff})");
                        //Console.ReadLine();
                    }
                }
            }
            
        }

        public static void SortLightCurves(double objid)
        {
            lightCurveQ.Clear();
            for (int i = 0; i < Queue.LightCurveQ.Count; i++)
            {
                if (Queue.LightCurveQ[i].ObjID == objid)
                {

                    lightCurveQ.Add(Queue.LightCurveQ[i]);

                    Console.WriteLine(Queue.LightCurveQ[i].Hmjd);
                }
            }

            lightCurveQ = lightCurveQ.OrderBy(lc => lc.Hmjd).ToList();
            foreach(var i in lightCurveQ)
            {
                Console.WriteLine($"Sorted: {i.Hmjd}");
            }
        }

    }
}
