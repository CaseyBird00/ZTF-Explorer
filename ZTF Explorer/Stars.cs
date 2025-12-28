using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTF_Explorer
{
    public class Star
    {
        public double ObjID { get; set; }
        public float Ra { get; set; }
        public float Decl { get; set; }
        public Boolean Processed { get; set; }
        public Boolean AAVSO { get; set; }

        public Star(double objID, float ra, float decl, bool processed, bool aAVSO)
        {
            ObjID = objID;
            this.Ra = ra;
            this.Decl = decl;
            Processed = processed;
            AAVSO = aAVSO;
        }
    }

    public class LightCurve
    {
        public double ObjID;
        public int Filterid;
        public double Hmjd;
        public double Mag;
        public double Magerr;

        public LightCurve(double objID, int filterid, double hmjd, double mag, double magerr)
        {
            ObjID = objID;
            Filterid = filterid;
            Hmjd = hmjd;
            Mag = mag;
            Magerr = magerr;
            
        }
    }
}
