using System;
using ComDbB;
using ComBase;
using System.Windows.Forms;
using System.Data;

namespace ComLibB
{ 
    public class clsITS 
    {  
        public static HiraDur.IHIRAClient DurClient = default(HiraDur.IHIRAClient);  
        public static HiraDur.IHIRAPrescription DurPrescription = default(HiraDur.IHIRAPrescription);
        public static HiraDur.IHIRAResultSet DurResultSet = default(HiraDur.IHIRAResultSet);

        //DUR2 -----------------------------------------------------------
        public static HiraDur.IHIRAClient DurClient3 = default(HiraDur.IHIRAClient);
        public static HiraDur.IHIRAResultSet DurResultSet3 = default(HiraDur.IHIRAResultSet);
        //----------------------------------------------------------------

        public static string szLog;        

        public static string DUR_CHECK_Mers_New(PsmhDb pDbCon, string strJumin, string sSName, string strBDate)
        {
            string m_strPrscAdmSym = "37100068";
            int intResult = 0;

            long nTotResultCnt;
            string strMsg = "";

            string rtnVal = "NO";

            DurPrescription.AdminType = "";                                 //' 처방조제구분
            DurPrescription.JuminNo = strJumin.Replace("-", "");            //' 수진자주민번호
            DurPrescription.PatNm = sSName;                                 //' 수진자성명
            DurPrescription.MprscIssueAdmin = m_strPrscAdmSym;              //' 처방기관기호
            DurPrescription.PrscAdminName = "포항성모병원";                 //' 처방기관명
            DurPrescription.PrscPresDt = strBDate.Replace("-", "");         //' 처방일자
            DurPrescription.AppIssueAdmin = m_strPrscAdmSym;                //' 청구소프트웨어 업체코드
            DurPrescription.MprscIssueAdmin = m_strPrscAdmSym;
            DurPrescription.AppIssueCode = "D09128112011202411037056720112";//'인증코드

            DurClient.AdminCode = m_strPrscAdmSym;
            DurPrescription.MprscIssueAdmin = m_strPrscAdmSym;

            intResult = DurClient.ParticularDiseaseCheck(DurPrescription, DurResultSet);

            if (intResult != 0)
            {
                if (clsOrdFunction.GstrGbJob == "ER")
                {
                    szLog = "DUR Mers 점검실패[" + intResult + "] : " + DurClient.LastErrorMsg;
                }
                rtnVal = "NO";
            }
            else
            {
                if (clsOrdFunction.GstrGbJob == "ER")
                {
                    MessageBox.Show("점검완료", "DUR점검", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                if (DurResultSet.Totalcnt > 0)
                {
                    nTotResultCnt = DurResultSet.Totalcnt;

                    DurResultSet.NextResult();
                    strMsg = "";
                    for (var i = 0; i < nTotResultCnt; i++)
                    {
                        if (i == 0)
                        {
                            strMsg = DurResultSet.Message;
                        }
                        else
                        {
                            strMsg += "\r\n" + DurResultSet.Message;
                        }
                    }

                    if (strMsg.Trim() != "")
                    {
                        fn_Send_ITS(strMsg.Trim());
                    }
                }
                rtnVal = "OK";
            }

            return rtnVal;
        }

        public static void fn_Send_ITS(string msgITS)   // ITS : International Traveler Information System)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0; //변경된 Row 받는 변수

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL += "   merge into KOSMOS_OCS.EXAM_INFECT_MASTER a                                   \r";
                SQL += "   using dual b                                                                 \r";
                SQL += "      on (a.PTNO = '" + clsOrdFunction.Pat.PtNo + "'                            \r";
                SQL += "     and  a.GUBUN = '06'                                                        \r";
                SQL += "     and  a.CODE = 'F99'                                                        \r";
                SQL += "     and  a.RDATE <= TRUNC(SYSDATE) - 30)                                       \r";
                SQL += "    when matched then                                                           \r";
                SQL += "  update set                                                                    \r";
                SQL += "         GUBUN = '06'                                                           \r";    //의미없는 update
                SQL += "    when not matched then                                                       \r";
                SQL += "  insert                                                                        \r";
                SQL += "        (RDATE, PANO, GUBUN, SPECNO, EXNAME, ODATE, OSABUN, CODE, INFO)         \r";
                SQL += "  values                                                                        \r";
                SQL += "        (to_date('" + clsOrdFunction.GstrBDate + "', 'yyyy-mm-dd')              \r";
                SQL += "       , '" + clsOrdFunction.Pat.PtNo + "'                                      \r";
                SQL += "       , '06'                                                                   \r";
                SQL += "       , ''                                                                     \r";
                SQL += "       , ''                                                                     \r";
                SQL += "       , ''                                                                     \r";
                SQL += "       , ''                                                                     \r";
                SQL += "       , 'F99'                                                                  \r";
                SQL += "       , '" + msgITS + "')                                                      \r";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr + " 신종 감염병 점검내역 저장중 오류 발생!!!");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message + " 신종 감염병 점검내역 저장중 오류 발생!!!");
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }
    }
}
