using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTF_Explorer
{
    public class StarProcessing
    {
        public void CalculateLightcurve(LightCurve lightCurve)
        {

            if (lightCurve.Mag > 20.0)
                Console.WriteLine("Light curve is " + " " + lightCurve.Mag);

        }

    }
}
