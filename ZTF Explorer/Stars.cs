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
        public int FieldID { get; set; }
        public float Ra { get; set; }
        public float Decl { get; set; }
        public Boolean Processed { get; set; }
        public Boolean AAVSO { get; set; }

        public Star(double objID, int fieldID, float ra, float decl, bool processed, bool aAVSO)
        {
            ObjID = objID;
            FieldID = fieldID;
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
        public float Hmjd;
        public float Mag;
        public float Magerr;

        public LightCurve(double objID, float filterid, float hmjd, float mag, float magerr)
        {
            ObjID = objID;
            Hmjd = hmjd;
            Mag = mag;
            Magerr = magerr;
            Filterid = filterid;
        }
    }
}
