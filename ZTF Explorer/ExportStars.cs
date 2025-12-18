using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ZTF_Explorer
{
    public static class ExportStars
    {
        public static void ExportAndClearCandidates()
        {
            if (Queue.Candidates.Count == 0)
                return;

            var sb = new StringBuilder();

            // Header
            sb.AppendLine("ObjID,RA,DECL,Status");

            foreach (var star in Queue.Candidates)
            {
                sb.AppendLine(
    $"{star.ObjID},{star.Ra},{star.Decl},Pending"
);
            }

            string filePath = Path.Combine(
            AppContext.BaseDirectory,
            "Stars.csv"
        );

            File.WriteAllText(filePath, sb.ToString());

            // 🔥 Remove everything AFTER successful write
            Queue.Candidates.Clear();
        }
    }
}

