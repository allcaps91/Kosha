using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComDbB;
using ComBase;

namespace ComSupLibB.SupLbEx
{
    /// <summary>
    /// Class Name : ComSupLibB.SupLbEx
    /// File Name : clsComSupLbExSendSQL.cs
    /// Title or Description : 기관 자동신고 관련 SQL 모음
    /// Author : 안정수
    /// Create Date : 2018-06-13
    /// Update History : 
    /// </summary>
    public class clsComSupLbExSendSQL
    {
        /// <summary>
        /// 병원체 자동신고 폼에서 사용함, EXAM_AUTOSEND_LOG
        /// </summary>
        public class cLbExSend
        {
            public string dplct_at                          = ""; 
            public string rspns_mssage_ty                   = ""; 
            public string strCert                           = "";
            public string User                              = "";

            public string strdgnss_de                       = ""; // SS1.Col = 2
            public string strreqest_de                      = ""; // SS1.Col = 3
            public string strpatnt_regist_no                = ""; // SS1.Col = 4
            public string strpatnt_nm                       = ""; // SS1.Col = 5

            public string strpatnt_sexdstn_cd               = ""; // SS1.Col = 6
            public string strpatnt_lifyea_md                = ""; // SS1.Col = 7
            public string strreqestinstt_charger_nm         = ""; // SS1.Col = 9
            public string strSpecNo                         = ""; // SS1.Col = 11
            public string strResult                         = ""; // SS1.Col = 13
            public string strinspctinstt_charger_nm         = ""; // SS1.Col = 17
            public string strpthgogan_cd                    = ""; // SS1.Col = 20

            public string strspm_ty_list                    = ""; // SS1.Col = 21
            public string strinspct_mth_ty_list             = ""; // SS1.Col = 22
            public string strspm_ty_etc                     = ""; // SS1.Col = 23

            public string strinspct_mth_ty_etc              = ""; // SS1.Col = 24
            public string strMasterCode                     = ""; // SS1.Col = 25
            public string strSubCode                        = ""; // SS1.Col = 26
            public string strOrderNo                        = ""; // SS1.Col = 27
            public string strBDate                          = ""; // SS1.Col = 28
            public string strResultDate                     = ""; // SS1.Col = 29
            public string strResultSabun                    = ""; // SS1.Col = 30           
        }
        public string ins_EXAM_AUTOSEND_LOG(PsmhDb pDbCon, cLbExSend argCls, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = "";

            SQL = "";
            SQL += ComNum.VBLF + "INSERT INTO KOSMOS_OCS.EXAM_AUTOSEND_LOG";
            SQL += ComNum.VBLF + "(";
            SQL += ComNum.VBLF + "  JDATE,RDATE,Pano,sName,Sex,";
            SQL += ComNum.VBLF + "  BIRTH,DRNAME,SpecNo,MASTERCODE,SUBCODE,";
            SQL += ComNum.VBLF + "  Result,RESULTDATE,RESULTSABUN,ACODE,EXAMTYPE,";
            SQL += ComNum.VBLF + "  EXAMWAY,EXAMTYPEETC,EXAMWAYETC,ORDERNO,";
            SQL += ComNum.VBLF + "  BDATE,SENDDATE,SENDSABUN";
            SQL += ComNum.VBLF + ")";
            SQL += ComNum.VBLF + "VALUES (";
            SQL += ComNum.VBLF + "   TO_DATE('" + argCls.strdgnss_de + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "  ,TO_DATE('" + argCls.strreqest_de + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "  ,'" + argCls.strpatnt_regist_no + "'";
            SQL += ComNum.VBLF + "  ,'" + argCls.strpatnt_nm + "'";
            SQL += ComNum.VBLF + "  ,'" + argCls.strpatnt_sexdstn_cd + "'";
            SQL += ComNum.VBLF + "  ,'" + argCls.strpatnt_lifyea_md.Replace("-", "") + "'";
            SQL += ComNum.VBLF + "  ,'" + argCls.strreqestinstt_charger_nm + "'";
            SQL += ComNum.VBLF + "  ,'" + argCls.strSpecNo + "'";
            SQL += ComNum.VBLF + "  ,'" + argCls.strMasterCode + "'";
            SQL += ComNum.VBLF + "  ,'" + argCls.strSubCode + "'";
            SQL += ComNum.VBLF + "  ,'" + argCls.strResult + "'";
            SQL += ComNum.VBLF + "  ,TO_DATE('" + argCls.strResultDate + "','YYYY-MM-DD HH24:MI')";
            SQL += ComNum.VBLF + "  , " + argCls.strResultSabun + "";
            SQL += ComNum.VBLF + "  ,'" + argCls.strpthgogan_cd + "'";
            SQL += ComNum.VBLF + "  ,'" + argCls.strspm_ty_list + "'";
            SQL += ComNum.VBLF + "  ,'" + argCls.strinspct_mth_ty_list + "'";
            SQL += ComNum.VBLF + "  ,'" + argCls.strspm_ty_etc + "'";
            SQL += ComNum.VBLF + "  ,'" + argCls.strinspct_mth_ty_etc + "'";
            SQL += ComNum.VBLF + "  ," + argCls.strOrderNo + "";
            SQL += ComNum.VBLF + "  ,TO_DATE('" + argCls.strBDate + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "  ,SYSDATE";
            SQL += ComNum.VBLF + "  ," + argCls.User + " ";
            SQL += ComNum.VBLF + ")";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }

            return SqlErr;
        }
    }
}
