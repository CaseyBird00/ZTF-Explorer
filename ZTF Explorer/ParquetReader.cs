using ParquetSharp;
using Microsoft.Data.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ZTF_Explorer
{
    public class ParquetReader
    {
        string filePath = "C:\\Users\\Casey\\Documents\\GitHub\\ZTF-Explorer\\ZTF Explorer\\Stars.parquet";

        public void Parquetreader()
        {
            using var file = new ParquetFileReader(filePath);

            using var rowGroup = file.RowGroup(0);

            using var firstCol = rowGroup.Column(0);

            using var firstRowGroupReader = file.RowGroup(0);

            int rows = checked((int)rowGroup.MetaData.NumRows);


            Console.WriteLine(file.FileMetaData.NumRowGroups);

            long?[] objId = rowGroup.Column(0)
                        .LogicalReader<long?>()
                        .ReadAll(rows);
            sbyte?[] filterId = rowGroup.Column(1)
                        .LogicalReader<sbyte?>()
                        .ReadAll(rows);
            float?[] ra = rowGroup.Column(4)
                        .LogicalReader<float?>()
                        .ReadAll(rows);
            float?[] decl = rowGroup.Column(5)
                        .LogicalReader<float?>()
                        .ReadAll(rows);

            for (int i = 0; i < objId.Length; i++)
            {
                Star star = new Star(
                    objId[i]!.Value,
                    ra[i]!.Value,
                    decl[i]!.Value,
                    false,
                    false);
                Queue.StarsQ.Enqueue(star);
                Console.WriteLine($"Enqueued star with ObjID: {star.ObjID}");
            }

            LightCurveParquetreader();
        }

        public void LightCurveParquetreader()
        {
            using var file = new ParquetFileReader(filePath);
            using var rowGroup = file.RowGroup(0);
            int rows = checked((int)rowGroup.MetaData.NumRows);

            // scalar per star
            long?[] objId = rowGroup.Column(0).LogicalReader<long?>().ReadAll(rows);
            sbyte?[] filterId = rowGroup.Column(1).LogicalReader<sbyte?>().ReadAll(rows);

            // LIST per star (arrays of values)
            double?[][] hmjd = rowGroup.Column(7).LogicalReader<double?[]>().ReadAll(rows);
            float?[][] mag = rowGroup.Column(8).LogicalReader<float?[]>().ReadAll(rows);
            float?[][] magerr = rowGroup.Column(9).LogicalReader<float?[]>().ReadAll(rows);

            for (int i = 0; i < rows; i++)
            {
                if (!objId[i].HasValue || !filterId[i].HasValue)
                    continue;

                if (filterId[i]!.Value != (sbyte)2) // r-band only
                    continue;

                var hmjdSeries = hmjd[i];
                var magSeries = mag[i];
                var magerrSeries = magerr[i];

                // Defensive: skip if any list is missing
                if (hmjdSeries == null || magSeries == null || magerrSeries == null)
                    continue;

                // They SHOULD be same length; clamp to smallest to be safe
                int n = Math.Min(hmjdSeries.Length, Math.Min(magSeries.Length, magerrSeries.Length));

                for (int j = 0; j < n; j++)
                {
                    if (!hmjdSeries[j].HasValue || !magSeries[j].HasValue || !magerrSeries[j].HasValue)
                        continue;

                    Queue.LightCurveQ.Add(new LightCurve(
                        objID: objId[i]!.Value,
                        filterid: filterId[i]!.Value,
                        hmjd: hmjdSeries[j]!.Value,
                        mag: magSeries[j]!.Value,
                        magerr: magerrSeries[j]!.Value
                    ));
                    Console.WriteLine($"Added LightCurve for ObjID: {objId[i]!.Value} at Hmjd: {hmjdSeries[j]!.Value}");
                }
            }
            Program.Main(Array.Empty<string>());
            return;


        }
    }
}



