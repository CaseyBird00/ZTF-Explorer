using Microsoft.Data.SqlClient;
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

        public static int score = 0;
        public static void StartProcess(Star star)
        {
            SortLightCurves(star.ObjID);
            CountPoints(star);
            AmplitudeCalc(star);
            flagVariable(star);

        }

        public static void CountPoints(Star star)
        {
            if (lightCurveQ != null)
            {
                if (lightCurveQ.Count >= 20)
                {
                    return;
                }
                else if (lightCurveQ.Count > 20 || lightCurveQ.Count < 30)
                {
                    score += 10;
                }
                else if (lightCurveQ.Count > 30 || lightCurveQ.Count < 40)
                {
                    score += 15;
                }
                else if(lightCurveQ.Count > 40)
                {
                    score += 20;
                }
            }
            
        }

        public static void AmplitudeCalc(Star star) {

            double Amplitude;
            double maxMag = lightCurveQ.Max(lc => lc.Mag);
            double minMag = lightCurveQ.Min(lc => lc.Mag);
            Amplitude = maxMag - minMag;

            if(Amplitude < 0.25)
            {
                return;
            }else if(Amplitude > 0.25 || Amplitude < 0.49)
            {
                score += 10;
            }
            else if(Amplitude > 0.5 || Amplitude < 0.79)
            {
                score += 18;
            }
            else if(Amplitude > 0.79)
            {
                score += 25;
            }

        }

        public static void flagVariable(Star star)
        {
            if(score >= 25)
            {
                Queue.VariableStarsQ.Add(star);
                Console.WriteLine($"Star {star.ObjID} flagged as variable with score {score}");
            }
            score = 0;
            lightCurveQ.Clear();
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
