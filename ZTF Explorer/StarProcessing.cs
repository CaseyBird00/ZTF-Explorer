using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ZTF_Explorer
{
    public class StarProcessing
    {


        static List<LightCurve> lightCurveQ = new List<LightCurve>();
        
        public static void StartProcess(Star star)
        {
            SortLightCurves(star.ObjID);
            CompareMagnitudes(star.ObjID);
        }

        public static void CompareMagnitudes(double objID)
        {
            if (lightCurveQ != null)
            {
                var starLCs = lightCurveQ.Where(lc => lc.ObjID == objID).ToList();

                for (int i = 1; i < lightCurveQ.Count; i++)
                {
                    //Console.WriteLine($"Processing ObjID {lightCurveQ[i].ObjID} at Hmjd {lightCurveQ[i].Hmjd}");

                    double magPeak = starLCs.Max(lc => lc.Mag);
                    double magTrough = starLCs.Min(lc => lc.Mag);

                    if(magPeak - magTrough >= 1)
                    {
                        Console.WriteLine($"Variation detected for star {lightCurveQ[i].ObjID}");
                        return;
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

                    //Console.WriteLine(Queue.LightCurveQ[i].Hmjd);
                }
            }

            lightCurveQ = lightCurveQ.OrderBy(lc => lc.Hmjd).ToList();
            foreach(var i in lightCurveQ)
            {
                //Console.WriteLine($"Sorted: {i.Hmjd}");
            }
        }

    }
}
