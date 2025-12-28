using ParquetSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
            using var file = new ParquetFileReader(@"C:\Users\Casey\Documents\GitHub\ZTF-Explorer\ZTF Explorer\Stars.parquet");
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
                for (int i = 0; i != groupNumRows; ++i)
                {
                    Console.WriteLine(groupNumRows);
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
                    LightCurveRead(Convert.ToDouble(Objid[i]), i, rowGroupReader);
                    //Console.WriteLine("Stars queue " + Queue.StarsQ.Count);
                    Console.WriteLine($"Star ID: {Objid[i]}, RA: {ObjRA[i]}, DECL: {ObjDec[i]}");

                }
                Program.Main(Array.Empty<string>());

            }
        }

        public void LightCurveRead(double objid, int row, RowGroupReader rowGroupReader)
        {
            Console.WriteLine($"ENTER LightCurveRead row={row} objid={objid}");

            try
            {
                int n = checked((int)rowGroupReader.MetaData.NumRows);

                using var filterReader = rowGroupReader.Column(1).LogicalReader<sbyte?>();     // filterid
                using var hmjdReader = rowGroupReader.Column(7).LogicalReader<double?[]>();  // hmjd list
                using var magReader = rowGroupReader.Column(8).LogicalReader<float?[]>();   // mag list  (nullable elems!)
                using var merrReader = rowGroupReader.Column(9).LogicalReader<float?[]>();   // magerr list (nullable elems!)

                var filter = filterReader.ReadAll(n);
                var hmjd = hmjdReader.ReadAll(n);
                var mag = magReader.ReadAll(n);
                var merr = merrReader.ReadAll(n);

                Console.WriteLine($"AFTER ReadAll row={row} filter={filter[row]} " +
                                  $"hmjdLen={(hmjd[row]?.Length ?? -1)} magLen={(mag[row]?.Length ?? -1)} merrLen={(merr[row]?.Length ?? -1)}");

                if ((filter[row] ?? (sbyte)-1) != 2)
                {
                    Console.WriteLine($"SKIP: filter != 1 (row={row}, filter={filter[row]})");
                    return;
                }

                var h = hmjd[row];
                var m = mag[row];
                var e = merr[row];

                if (h == null || m == null || e == null)
                {
                    Console.WriteLine("SKIP: one of arrays is null");
                    return;
                }

                int len = Math.Min(h.Length, Math.Min(m.Length, e.Length));
                int added = 0;

                for (int i = 0; i < len; i++)
                {
                    if (!h[i].HasValue || !m[i].HasValue || !e[i].HasValue) continue;

                    Queue.LightCurveQ.Add(new LightCurve(
                        objid,
                        1,
                        h[i]!.Value,
                        m[i]!.Value,
                        e[i]!.Value
                    ));

                    added++;
                }

                Console.WriteLine($"DONE row={row} added={added} total={Queue.LightCurveQ.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR in LightCurveRead row={row} objid={objid}: {ex}");
            }
        }


    }
}

