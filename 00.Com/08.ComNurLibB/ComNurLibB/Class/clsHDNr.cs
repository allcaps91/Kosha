using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComDbB;
using ComBase;
using System.Data;

namespace ComNurLibB
{
    public class clsHDNr
    {
        public class HDQI2009_Result
        {
            public string Hb;
            public string Hct;
            public string WBC;
            public string Plt;
            public string TPro;
            public string Alb;
            public string SGOT;
            public string SGPT;
            public string ALP;
            public string Glucose;
            public string BUN;
            public string Cr;
            public string Uricacid;
            public string Na;
            public string K;
            public string P;
            public string TotalCa;
            public string TChol;
            public string HDLChol;
            public string LDLChol;
            public string TG;
            public string UIBC;
            public string TIBC;
            public string FE;
            public string Ferritin;
            public string PTH;
            public string HbA1C;
            public string ChestPA;
            public string HBsAg;
            public string HBsAb;
            public string HCVAb;
            public string EKG;
            public string VDRL;
            public string HIVAb;
        }
        public static HDQI2009_Result HDResult = new HDQI2009_Result();



        public static void READ_TSET(PsmhDb pDbCon, string strPANO, ref string strBDATE, ref string strNEXTDATE)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, TO_CHAR(BDATE + 180,'YYYY-MM-DD') NEXTDATE, PTNO, B.SNAME ";
                SQL = SQL + ComNum.VBLF + " FROM( ";
                SQL = SQL + ComNum.VBLF + " SELECT MAX(BDATE) BDATE, PTNO ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_HDPE_TSET MST ";
                SQL = SQL + ComNum.VBLF + " WHERE EXISTS ";
                SQL = SQL + ComNum.VBLF + " (SELECT * FROM KOSMOS_PMPA.NUR_HDPE_MST SUB ";
                SQL = SQL + ComNum.VBLF + "  WHERE MST.PTNO = SUB.PANO ";
                SQL = SQL + ComNum.VBLF + "    AND(SUB.ENDDATE IS NULL OR SUB.ENDDATE = '')) ";
                SQL = SQL + ComNum.VBLF + "  GROUP BY PTNO) A, KOSMOS_PMPA.BAS_PATIENT B ";                
                SQL = SQL + ComNum.VBLF + " WHERE A.PTNO = B.PANO ";
                SQL = SQL + ComNum.VBLF + "   AND A.PTNO = '" + strPANO + "' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY BDATE ASC ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                
                if (dt.Rows.Count > 0)
                {
                    strBDATE = dt.Rows[0]["BDATE"].ToString().Trim();
                    strNEXTDATE = dt.Rows[0]["NEXTDATE"].ToString().Trim();                    
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }
    }
}
