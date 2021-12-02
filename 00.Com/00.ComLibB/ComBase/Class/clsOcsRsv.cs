using ComDbB; //DB연결
using System;
using System.Data;

namespace ComBase
{
    public class clsOcsRsv
    {
        #region //Opd_FM_Resv(Opd_FM_Resv.bas)
        public static void Varient_Clear()
        {
            clsPublic.GnChoInWon_A = 0;    //'2013-11-27
            clsPublic.GnJaeInWon_A = 0;    //'2013-11-27
            clsPublic.GnChoInWon_P = 0;    //'2013-11-27
            clsPublic.GnJaeInWon_P = 0;    //'2013-11-27
        }

        public static void READ_FM_CHOJAE_INWON_Time(PsmhDb pDbCon, string ArgSDate, string ArgDept, string ArgDrCode, string ArgTime)
        {
            string strSql = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                clsPublic.GnRInWon_Cho = 0;
                clsPublic.GnRInWon_Jae = 0;

                strSql = " SELECT RINWON1, RINWON2 ";
                strSql = strSql + ComNum.VBLF + "  FROM KOSMOS_PMPA.ETC_FM_SCH ";
                strSql = strSql + ComNum.VBLF + " WHERE SDate <=TO_DATE('" + ArgSDate + "','YYYY-MM-DD') ";
                strSql = strSql + ComNum.VBLF + "   AND RTime ='" + ArgTime + "' ";
                strSql = strSql + ComNum.VBLF + "   AND DeptCode ='" + ArgDept + "' ";
                strSql = strSql + ComNum.VBLF + "   AND DrCode ='" + ArgDrCode + "' ";
                strSql = strSql + ComNum.VBLF + " ORDER By SDate DESC ";
                SqlErr = clsDB.GetDataTable(ref dt, strSql, pDbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, strSql, pDbCon); //에러로그 저장
                    return;
                }
                if(dt.Rows.Count > 0)
                {
                    clsPublic.GnRInWon_Cho = (int)VB.Val(dt.Rows[0]["RINWON1"].ToString().Trim());     //'오전 초진 인원
                    clsPublic.GnRInWon_Jae = (int)VB.Val(dt.Rows[0]["RINWON2"].ToString().Trim());     //'오전 재진 인원
                }
                dt.Dispose();
                dt = null;

            }
            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, strSql, pDbCon); //에러로그 저장
            }
        }

        /// <summary>
        /// Author : 박창욱
        /// Create Date : 2018.02.20
        /// </summary>
        public static void READ_FM_CHOJAE_INWON_Time_Yoil(PsmhDb pDbCon, string ArgSDate, string ArgDept, string ArgDrCode, string ArgTime, string ArgYoil)
        {
            string strSql = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                clsPublic.GnRInWon_Cho = 0;
                clsPublic.GnRInWon_Jae = 0;

                strSql = " SELECT RINWON1, RINWON2 ";
                strSql = strSql + ComNum.VBLF + "  FROM KOSMOS_PMPA.ETC_FM_SCH ";
                strSql = strSql + ComNum.VBLF + " WHERE SDate <=TO_DATE('" + ArgSDate + "','YYYY-MM-DD') ";
                strSql = strSql + ComNum.VBLF + "   AND RTime ='" + ArgTime + "' ";
                strSql = strSql + ComNum.VBLF + "   AND DeptCode ='" + ArgDept + "' ";
                strSql = strSql + ComNum.VBLF + "   AND DrCode ='" + ArgDrCode + "' ";
                strSql = strSql + ComNum.VBLF + "   AND Yoil ='" + ArgYoil + "' ";
                strSql = strSql + ComNum.VBLF + " ORDER By SDate DESC ";
                SqlErr = clsDB.GetDataTable(ref dt, strSql, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, strSql, pDbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    clsPublic.GnRInWon_Cho = (int)VB.Val(dt.Rows[0]["RINWON1"].ToString().Trim());     //'오전 초진 인원
                    clsPublic.GnRInWon_Jae = (int)VB.Val(dt.Rows[0]["RINWON2"].ToString().Trim());     //'오전 재진 인원
                }
                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, strSql, pDbCon); //에러로그 저장
            }
        }

        #endregion //Opd_FM_Resv(Opd_FM_Resv.bas)
    }
}
