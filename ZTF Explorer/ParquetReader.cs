using ParquetSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ZTF_Explorer
{
    public class ParquetReader
    {
        public void parquetreader()
        {
            using var file = new ParquetFileReader("C:\\Users\\Casey\\Documents\\GitHub\\ZTF-Explorer\\ZTF Explorer\\Stars.parquet");
            using var firstRowGroupReader = file.RowGroup(0);

            for (int rowGroup = 0; rowGroup < file.FileMetaData.NumRowGroups; ++rowGroup)
            {
                using var rowGroupReader = file.RowGroup(rowGroup);
                var groupNumRows = checked((int)rowGroupReader.MetaData.NumRows);


                //Read variables from parquet file
                var Column0 = rowGroupReader.Column(0).LogicalReader<long?>(); //read ObJID
                var Column2 = rowGroupReader.Column(2).LogicalReader<short?>(); //read FieldID
                var Column4 = rowGroupReader.Column(4).LogicalReader<float?>(); //read RA
                var Column5 = rowGroupReader.Column(5).LogicalReader<float?>();// read DEC

                //Assign variables
                var Objid = Column0.ReadAll(groupNumRows);
                var Fieldid = Column2.ReadAll(groupNumRows);
                var ObjRA = Column4.ReadAll(groupNumRows);
                var ObjDec = Column5.ReadAll(groupNumRows);

                //Loop that reads each row and creates a star object
                for (int i = 0; i != 200 /*groupNumRows*/; ++i)
                {
                    Console.WriteLine(i);
                    var objRA = ObjRA[i] ?? 0f;
                    var objDec = ObjDec[i] ?? 0f;

                    /*
                    Console.WriteLine(Objid[i]);
                    Console.WriteLine(Fieldid[i]);
                    Console.WriteLine(ObjRA[i]);
                    Console.WriteLine(ObjDec[i]);
                    */
                    Star star = new(Convert.ToDouble(Objid[i]), Convert.ToInt32(Fieldid[i]), objRA, objDec, false, false);
                    Console.WriteLine("OBJ RA: " + objRA);
                    Queue.StarsQ.Enqueue(star);
                    LightCurveRead(Convert.ToDouble(Objid[i]), i,rowGroupReader);
                    Console.WriteLine("Stars queue " + Queue.StarsQ.Count);

                }
                Program.Main(Array.Empty<string>());

            }
        }

        public void LightCurveRead(double Objid, int Row,  RowGroupReader rowGroupReader)
        {
            var groupNumRows = checked((int)rowGroupReader.MetaData.NumRows);

            var Column1 = rowGroupReader.Column(1).LogicalReader<sbyte?>(); //FilterID
            var Column7 = rowGroupReader.Column(7).LogicalReader<double?[]>(); //HMJD
            var Column8 = rowGroupReader.Column(8).LogicalReader<float?[]>(); //Mag
            var Column9 = rowGroupReader.Column(9).LogicalReader<float?[]>();//Magerr

            

                //Assign variables
                var Filterid = Column1.ReadAll(groupNumRows);
                var Hmjd = Column7.ReadAll(groupNumRows);
                var Mag = Column8.ReadAll(groupNumRows);
                var Magerr = Column9.ReadAll(groupNumRows);

                //Loop that reads each row and creates a light curve object
                for (int i = 0; i < Mag[Row].Length; i++)
                {
                //Only use lightcurves with filterid 1 (g-band)
                if (Convert.ToInt32(Filterid[Row]) == 1)
                 {
               // Console.WriteLine(Filterid[Row]);
                        LightCurve lightCurve = new(Objid, Convert.ToInt32(Filterid[Row]), Convert.ToDouble(Hmjd[Row][i]), Convert.ToDouble(Mag[Row][i]), Convert.ToDouble(Magerr[Row][i]));
                    Queue.LightCurveQ.Add(lightCurve);


                }
            }



            Console.WriteLine("Light Curve queue " + Queue.LightCurveQ.Count);
            //Console.WriteLine(lightCurve.ObjID + " " + lightCurve.Filterid + " " + lightCurve.Hmjd + " " + lightCurve.Mag + "  "+ lightCurve.Magerr);
        }
    }
}


