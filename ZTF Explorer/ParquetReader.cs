using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ParquetSharp;

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

                var Column0 = rowGroupReader.Column(0).LogicalReader<long?>();
                var Column2 = rowGroupReader.Column(2).LogicalReader<short?>();
                var Column4 = rowGroupReader.Column(4).LogicalReader<float?>();
                var Column5 = rowGroupReader.Column(5).LogicalReader<float?>();

                var Objid = Column0.ReadAll(groupNumRows);
                var Fieldid = Column2.ReadAll(groupNumRows);
                var ObjRA = Column4.ReadAll(groupNumRows);
                var ObjDec = Column5.ReadAll(groupNumRows);

                for (int i = 0; i < groupNumRows; ++i)
                {
                    var objRA = ObjRA[i] ?? 0f;
                    var objDec = ObjDec[i] ?? 0f;

                    /*
                    Console.WriteLine(Objid[i]);
                    Console.WriteLine(Fieldid[i]);
                    Console.WriteLine(ObjRA[i]);
                    Console.WriteLine(ObjDec[i]);
                    */
                    Star star = new(Convert.ToDouble(Objid[i]), Convert.ToInt32(Fieldid[i]), objRA, objDec, false, false);
                    LightCurveRead(i,rowGroupReader);
                }
            }
        }

        public void LightCurveRead(int Row,  RowGroupReader rowGroupReader)
        {
            var groupNumRows = checked((int)rowGroupReader.MetaData.NumRows);

            var Column1 = rowGroupReader.Column(1).LogicalReader<long?>();
            var Column7 = rowGroupReader.Column(7).LogicalReader<double?[]>();
            var Column8 = rowGroupReader.Column(8).LogicalReader<float?[]>();
            var Column9 = rowGroupReader.Column(9).LogicalReader<float?[]>();

            var Filterid = Column1.ReadAll(groupNumRows);
            var Hmjd = Column7.ReadAll(groupNumRows);
            var Mag = Column8.ReadAll(groupNumRows);
            var Magerr = Column9.ReadAll(groupNumRows);

            Console.WriteLine(Hmjd[Row][0]);
        }
    }
}


