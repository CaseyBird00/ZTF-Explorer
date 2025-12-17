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
            CompareMagnitudes(star);
        }

        public static void CompareMagnitudes(Star star)
        {
            if (lightCurveQ != null)
            {
                var starLCs = lightCurveQ.Where(lc => lc.ObjID == star.ObjID).ToList();

                // for (int i = 1; i < lightCurveQ.Count; i++)
                //{
                //Console.WriteLine($"Processing ObjID {lightCurveQ[i].ObjID} at Hmjd {lightCurveQ[i].Hmjd}");

                double threshold = .7;
                int MinOccurances = 20;

                var sortedMags = starLCs.Select(lc => lc.Mag).OrderBy(m => m).ToList();
                double medianMag = sortedMags[sortedMags.Count / 2];

                int deviationCount = starLCs.Count(lc => Math.Abs(lc.Mag - medianMag) >= threshold);

                if (deviationCount >= MinOccurances)
                    {
                    Console.WriteLine($"Variation detected for star {star.ObjID}");

                    Queue.VariableStarsQ.Add(star);
                    return;
                    }
               // }
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
