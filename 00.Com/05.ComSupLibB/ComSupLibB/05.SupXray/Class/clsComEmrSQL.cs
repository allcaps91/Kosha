using ComDbB; //DB연결
using ComBase; //기본 클래스
using System.Data;

namespace ComSupLibB.Com
{
    /// <summary>
    /// Class Name      : ComSupLibB.Com 
    /// File Name       : clsComEmrSQL.cs
    /// Description     : 진료지원 EMR 임시 연계관련 class
    /// Author          : 윤조연
    /// Create Date     : 2017-07-20 
    /// Update History  : 
    /// </summary> 
    /// <history>  
    ///  
    /// </history> 
    public class clsComEmrSQL   
    { 
        clsQuery Query = new clsQuery();

        string SQL = "";
        string SqlErr = ""; //에러문 받는 변수

        #region  기본 쿼리 관련

        public DataTable sel_EMRXML(PsmhDb pDbCon, string argJob, string argPano,string argWhere)
        {
            DataTable dt = null;

            SQL = "";
            if (argJob =="00")
            {
                //SQL += " SELECT                                                                     \r\n";
                //SQL += "   CHARTTIME                                                                \r\n";
                //SQL += "   ,extractValue(chartxml, '//it10') AS it10                                \r\n";
                //SQL += "   ,extractValue(chartxml, '//it11') AS it11                                \r\n";
                //SQL += "   ,extractValue(chartxml, '//it02') AS it02                                \r\n";
                //SQL += "   ,extractValue(chartxml, '//it03') AS it03                                \r\n";
                //SQL += "  FROM " + ComNum.DB_EMR + "EMRXML                                          \r\n";
                //SQL += "   WHERE 1 = 1                                                              \r\n";
                //if (argJob == "00")
                //{
                //    SQL += "    AND Ptno = '" + argPano + "'                                        \r\n";
                //    SQL += "    AND FORMNO = 1562                                                   \r\n";
                //    SQL += "    AND extractValue(chartxml, '//it10') IS NOT NULL                    \r\n";
                //}
                //else
                //{
                //    SQL += "    AND Pano = '" + argPano + "'                                        \r\n";
                //}

                //#region 신규
                //SQL += " UNION ALL                                                                  \r\n";
                SQL += " SELECT                                                                     \r\n";
                SQL += "   CHARTTIME                                                                \r\n";
                SQL += "   ,(SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMCD = 'I0000000418') AS it10 \r\n";
                SQL += "   ,(SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMCD = 'I0000000002') AS it11 \r\n";
                SQL += "   ,(SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMCD = 'I0000002018') AS it02 \r\n";
                SQL += "   ,(SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMCD = 'I0000001765') AS it03 \r\n";
                SQL += "   ,(SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMCD = 'I0000014815') AS it04 \r\n";
                SQL += "  FROM " + ComNum.DB_EMR + "AEMRCHARTMST A                                  \r\n";
                SQL += "    LEFT OUTER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R                     \r\n";
                SQL += "      ON A.EMRNO = R.EMRNO                                                  \r\n";
                SQL += "     AND A.EMRNOHIS = R.EMRNOHIS                                            \r\n";
                if(argWhere == "1")
                {
                    SQL += "     AND R.ITEMCD IN('I0000002018','I0000001765','I0000014815')  \r\n";
                }
                else
                {
                    SQL += "     AND R.ITEMCD = 'I0000000418'                                           \r\n";
                }
                SQL += "   WHERE 1 = 1                                                              \r\n";
                if (argJob == "00")
                {
                    SQL += "    AND Ptno = '" + argPano + "'                                        \r\n";
                    SQL += "    AND FORMNO IN(1562, 3150, 2431)                                           \r\n";
                    SQL += "    AND R.ITEMVALUE IS NOT NULL                                         \r\n";
                }
                else
                {
                    SQL += "    AND Pano = '" + argPano + "'                                        \r\n";
                }
                #endregion
            }
            else if (argJob == "01")
            {
                SQL += " SELECT                                                                     \r\n";
                SQL += "   EMRNO                                                                    \r\n";
                SQL += "  FROM " + ComNum.DB_EMR + "EMRXML                                          \r\n";
                SQL += "   WHERE 1 = 1                                                              \r\n";
                if (argWhere != "")
                {
                    SQL += "  " + argWhere + "                                                     \r\n";
                }
                else
                {
                    SQL += "    AND Ptno = '" + argPano + "'                                        \r\n";
                }

                //#region 신규
                SQL += " UNION ALL                                                                  \r\n";
                SQL += " SELECT                                                                     \r\n";
                SQL += "   EMRNO                                                                    \r\n";
                SQL += "  FROM " + ComNum.DB_EMR + "AEMRCHARTMST                                    \r\n";
                SQL += "   WHERE 1 = 1                                                              \r\n";
                if (argWhere != "")
                {
                    SQL += "  " + argWhere + "                                                     \r\n";
                }
                else
                {
                    SQL += "    AND Ptno = '" + argPano + "'                                        \r\n";
                }
                //#endregion
            }


            if (argJob == "00")
            {
                SQL += "    ORDER BY R.INPDATE DESC ,R.INPTIME DESC                                             \r\n";
            }
            else
            {

            }


            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }


       // #endregion


        #region 트랜잭션 쿼리 + enum INSERT, UPDATE,DELETE .... 

        /// <summary>
        /// Text Emr 연계관련 class
        /// </summary>
        public class Emr_Info
        {
            public string sEmrNo = "";
            public double nEmrNo = 0;
            public string Sabun = "";
            public double EmrHisNo = 0;
            public string CurrentDate = "";
            public string CurrentTime = "";

        }

        /// <summary>
        /// 6분걷기 결과입력시 차트 연계관련 insert 쿼리
        /// </summary>
        /// <param name="argCls"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string ins_EMRXMLHISTORY(PsmhDb pDbCon, Emr_Info argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_EMR + "EMRXMLHISTORY                                     \r\n";
            SQL += "    (HISTORYNO, EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO                   \r\n";
            SQL += "    ,INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD           \r\n";
            SQL += "    ,WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,HISTORYWRITEDATE                \r\n";
            SQL += "    ,HISTORYWRITETIME,DELUSEID,CERTNO )                                             \r\n";            
            SQL += "   SELECT  " + argCls.EmrHisNo + "                                                  \r\n";
            SQL += "          ,EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO                        \r\n";
            SQL += "          ,INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD     \r\n";
            SQL += "          ,WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO                           \r\n";
            SQL += "          ,'" + argCls.CurrentDate + "'                                             \r\n";
            SQL += "          ,'" + argCls.CurrentTime + "'                                             \r\n";
            SQL += "          ,'" + argCls.Sabun + "'                                                   \r\n";
            SQL += "          ,CERTNO                                                                   \r\n";
            SQL += "     FROM " + ComNum.DB_EMR + "EMRXML                                               \r\n";
            SQL += "      WHERE 1 = 1                                                                   \r\n";
            SQL += "       AND EMRNO =" + argCls.nEmrNo + "                                             \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        /// <summary>
        /// EMRXML 삭제
        /// </summary>
        /// <param name="argCls"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string del_EMRXML(PsmhDb pDbCon, Emr_Info argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " DELETE FROM " + ComNum.DB_EMR + "EMRXML                                            \r\n";
            SQL += "      WHERE 1 = 1                                                                   \r\n";
            SQL += "       AND EMRNO =" + argCls.nEmrNo + "                                             \r\n";
            
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        /// <summary>
        /// EMRXMLMST 삭제
        /// </summary>
        /// <param name="argCls"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string del_EMRXMLMST(PsmhDb pDbCon, Emr_Info argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " DELETE FROM " + ComNum.DB_EMR + "EMRXMLMST                                         \r\n";
            SQL += "      WHERE 1 = 1                                                                   \r\n";
            SQL += "       AND EMRNO =" + argCls.nEmrNo + "                                             \r\n";
            
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }


        #endregion

    }
}
