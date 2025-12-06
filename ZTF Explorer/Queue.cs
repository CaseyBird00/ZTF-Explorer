using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTF_Explorer
{
    public static class Queue
    {
        public static List<LightCurve> LightCurveQ = new List<LightCurve>();
        public static ConcurrentQueue<Star> StarsQ = new ConcurrentQueue<Star>();

    }
}   
