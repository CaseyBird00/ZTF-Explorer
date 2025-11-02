using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTF_Explorer
{
    public class Star
    {
        public string ObjID;
        public int FieldID;
        public float ra;
        public float decl;
        public Boolean Checked;
        public Boolean AAVSO;
    }

    public class LightCurve
    {
        public string ObjID;
        public float hmjd;
        public float mag;
        public float magerr;
        public float filterid;
    }
}
