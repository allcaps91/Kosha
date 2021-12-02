using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComMirLibB.MirEnt
{
    public class clsComMirEnt
    {
        public interface ComMirEntInterface
        {

            void GetSpread(FarPoint.Win.Spread.FpSpread spdCopy, int nCopyRow);  //청구내역 복사 기능에 사용하는 인터페이스


           
        }

        public class TABLE_SUGA_SET   //                          '수가 Tables
        {
            public string Sucode = ""; 
            public string Sunext = "";
            public string Bun = "";
            public string SugbA = "";
            public string SugbB = "";
            public string SugbE = "";
            public string SugbJ = "";
            public string SugbS = "";
            public string SunameK = "";
            public long Price = 0;
            public string BCODE = "";
            public string EDIJONG = "";
            public double SUHAM = 0;
            public string OLDBCODE = "";
            public long OldPrice = 0;
            public string OLDJONG = "";
            public double OLDGESU = 0;
            public string SugbAA = "";
            public string SugbAC = "";

        }
        // public TABLE_SUGA_SET TSU;

    }
}
