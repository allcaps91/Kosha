using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스
using ComDbB; //DB연결

namespace ComEmrBase
{
    public class clsOcsForm
    {

        /// <summary>
        /// OCS FORM을 저장한다.
        /// </summary>
        /// <param name="ChartForm"></param>
        /// <param name="isSpcPanel"></param>
        /// <param name="pControl"></param>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="strOcsFormSeq"></param>
        /// <param name="strChartDate"></param>
        /// <param name="strChartTime"></param>
        /// <param name="strPTNO"></param>
        /// <param name="strINOUTCLS"></param>
        /// <param name="strMEDFRDATE"></param>
        /// <param name="strMEDDEPTCD"></param>
        /// <param name="strMEDENDDATE"></param>
        /// <param name="strACPNO"></param>
        /// <returns></returns>
        public static double SaveOcsFormMst(PsmhDb pDbCon, Control ChartForm, bool isSpcPanel, Control pControl,
                                string strFormNo, string strUpdateNo, string strOcsFormSeq, string strChartDate, string strChartTime, string strUseId,
                                string strPTNO, string strINOUTCLS = "", string strMEDFRDATE = "", string strMEDDEPTCD = "", string strMEDENDDATE = "", string strACPNO = "")
        {
            double rtnVal = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            double dblOcsFormHisNew = 0;
            double dblOcsFormSeqNew = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(pDbCon);
            

            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(pDbCon, "A");
                string strCurDate = VB.Left(strCurDateTime, 8);
                string strCurTime = VB.Right(strCurDateTime, 6);

                dblOcsFormSeqNew = ComQuery.GetSequencesNo(pDbCon, "" + ComNum.DB_EMR + "GETSEQ_OCSFORMMST_SEQ");

                if (VB.Val(strOcsFormSeq) > 0)
                {
                    //OCSFORMMST
                    dblOcsFormHisNew = ComQuery.GetSequencesNo(pDbCon, "" + ComNum.DB_EMR + "GETSEQ_OCSFORMMSTHIS_SEQ");

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "OCSFORMMSTHIS ";
                    SQL = SQL + ComNum.VBLF + "    (SAVENOHIS, SAVENO, FORMNO, UPDATENO, CHARTDATE, CHARTTIME, CHARTUSEID, ";
                    SQL = SQL + ComNum.VBLF + "    ACPNO, PTNO, INOUTCLS, MEDFRDATE, MEDDEPTCD, MEDENDDATE,";
                    SQL = SQL + ComNum.VBLF + "    WRITEDATE, WRITETIME, ";
                    SQL = SQL + ComNum.VBLF + "    DCSAVENO, DCUSEID, DCDATE, DCTIME )";
                    SQL = SQL + ComNum.VBLF + "SELECT ";
                    SQL = SQL + ComNum.VBLF + dblOcsFormHisNew + " AS SAVENOHIS, ";
                    SQL = SQL + ComNum.VBLF + "    SAVENO, FORMNO, UPDATENO, CHARTDATE, CHARTTIME, CHARTUSEID, ";
                    SQL = SQL + ComNum.VBLF + "    ACPNO, PTNO, INOUTCLS, MEDFRDATE, MEDDEPTCD, MEDENDDATE,";
                    SQL = SQL + ComNum.VBLF + "    WRITEDATE, WRITETIME, ";
                    SQL = SQL + ComNum.VBLF + "'" + dblOcsFormSeqNew + "' AS DCSAVENO, ";
                    SQL = SQL + ComNum.VBLF + "'" + strUseId + "' AS DCUSEID, ";
                    SQL = SQL + ComNum.VBLF + "'" + strCurDate + "' AS DCDATE, ";
                    SQL = SQL + ComNum.VBLF + "'" + strCurTime + "' AS DCTIME ";
                    SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_EMR + "OCSFORMMST";
                    SQL = SQL + ComNum.VBLF + "WHERE SAVENO = " + strOcsFormSeq;
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "DELETE FROM  " + ComNum.DB_EMR + "OCSFORMMST";
                    SQL = SQL + ComNum.VBLF + "WHERE SAVENO = " + strOcsFormSeq;
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    //OCSFORMROW
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "OCSFORMROWHIS ";
                    SQL = SQL + ComNum.VBLF + "    ( SAVENO, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1 )";
                    SQL = SQL + ComNum.VBLF + "SELECT ";
                    SQL = SQL + ComNum.VBLF + "    SAVENO, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1";
                    SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_EMR + "OCSFORMROW";
                    SQL = SQL + ComNum.VBLF + "WHERE SAVENO = " + strOcsFormSeq;
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "DELETE FROM  " + ComNum.DB_EMR + "OCSFORMROW";
                    SQL = SQL + ComNum.VBLF + "WHERE SAVENO = " + strOcsFormSeq;
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                }

                //저장 MAST
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "OCSFORMMST ";
                SQL = SQL + ComNum.VBLF + "    (SAVENO, FORMNO, UPDATENO, CHARTDATE, CHARTTIME, CHARTUSEID, ";
                SQL = SQL + ComNum.VBLF + "     ACPNO, PTNO, INOUTCLS, MEDFRDATE, MEDDEPTCD, MEDENDDATE,";
                SQL = SQL + ComNum.VBLF + "    WRITEDATE, WRITETIME )";
                SQL = SQL + ComNum.VBLF + "VALUES (";
                SQL = SQL + ComNum.VBLF + dblOcsFormSeqNew.ToString() + ",";    //SAVENO
                SQL = SQL + ComNum.VBLF + strFormNo + ","; //FORMNO
                SQL = SQL + ComNum.VBLF + strUpdateNo + ",";   //UPDATENO
                SQL = SQL + ComNum.VBLF + "'" + strChartDate + "',";   //CHARTDATE
                SQL = SQL + ComNum.VBLF + "'" + strChartTime + "',";   //CHARTTIME
                SQL = SQL + ComNum.VBLF + "'" + strUseId + "',";   //CHARTUSEID
                SQL = SQL + ComNum.VBLF + VB.Val(strACPNO) + ",";   //ACPNO
                SQL = SQL + ComNum.VBLF + "'" + strPTNO + "',";   //PTNO
                SQL = SQL + ComNum.VBLF + "'" + strINOUTCLS + "',";   //INOUTCLS
                SQL = SQL + ComNum.VBLF + "'" + strMEDFRDATE + "',";   //MEDFRDATE
                SQL = SQL + ComNum.VBLF + "'" + strMEDDEPTCD + "',";   //MEDDEPTCD
                SQL = SQL + ComNum.VBLF + "'" + strMEDENDDATE + "',";   //MEDENDDATE
                SQL = SQL + ComNum.VBLF + "'" + strCurDate + "',";   //WRITEDATE
                SQL = SQL + ComNum.VBLF + "'" + strCurTime + "') ";   //WRITETIME

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //저장 OCSFORMROW
                if (SaveDataOCSFORMROW(ChartForm, false, null, dblOcsFormSeqNew) == false)
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("OCSFORMROW저장 중 에러가 발생했습니다.");
                    return rtnVal;
                }

                rtnVal = dblOcsFormSeqNew;

                clsDB.setCommitTran(pDbCon);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// OCSFORMROW에 data를 저장한다.
        /// </summary>
        /// <param name="ChartForm"></param>
        /// <param name="isSpcPanel"></param>
        /// <param name="pControl"></param>
        /// <param name="dbSaveNoNew"></param>
        /// <returns></returns>
        public static bool SaveDataOCSFORMROW(Control ChartForm, bool isSpcPanel, Control pControl, double dbSaveNoNew, clsTrans TRS = null)
        {
            bool rtnVal = false;
            string SQL = "";

            string strITEMCD = "";
            string strITEMNO = "";
            string strITEMINDEX = "";
            string strITEMTYPE = "";
            string strITEMVALUE = "";
            string strITEMVALUE1 = "";
            string strDSPSEQ = "0";

            bool bolIsItem = false;

            string[] arrySAVENO = null;
            string[] arryITEMCD = null;
            string[] arryITEMNO = null;
            string[] arryITEMINDEX = null;
            string[] arryITEMTYPE = null;
            string[] arryITEMVALUE = null;
            string[] arryITEMVALUE1 = null;
            string[] arryDSPSEQ = null;

            try
            {
                if (isSpcPanel == true)
                {
                    if (pControl == null)
                    {
                        ComFunc.MsgBox("선택된 컨테이너가 존재하지 않습니다.");
                        return rtnVal;
                    }
                }

                Control[] controls = null;

                if (isSpcPanel == true)
                {
                    controls = ComFunc.GetAllControls(pControl);
                }
                else
                {
                    controls = ComFunc.GetAllControls(ChartForm);
                }

                foreach (Control objControl in controls)
                {
                    if (ComFunc.IsVisible(objControl, isSpcPanel, pControl) == true) //숨긴패널 안에 컨트롤은 저장하지 않는다.
                    {
                        strITEMCD = "";
                        strITEMNO = "";
                        strITEMINDEX = "-1";
                        strITEMTYPE = "";
                        strITEMVALUE = "";
                        strITEMVALUE1 = "";
                        strDSPSEQ = "0";

                        bolIsItem = false;

                        strITEMINDEX = clsXML.IsArryCon(objControl);
                        if (strITEMINDEX == "") strITEMINDEX = "-1";

                        strITEMCD = objControl.Name;
                        strITEMNO = strITEMCD;

                        if ((objControl is FarPoint.Win.Spread.FpSpread) == false)
                        {
                            if (VB.InStr(strITEMCD, "_") > 0)
                            {
                                string[] strParams = VB.Split(strITEMCD.Trim(), "_", -1);
                                strITEMNO = strParams[0];
                            }
                        }

                        if (strITEMNO != "txtMedFrDate" && strITEMNO != "txtMedFrTime" && strITEMNO != "")
                        {
                            //DateTimePicker(DTPicker)
                            if (objControl is DateTimePicker)
                            {
                                strITEMTYPE = "DATE";
                                bolIsItem = true;
                                if (isSpcPanel == true)
                                {
                                    if (clsXML.IsParent(objControl, pControl) == true)
                                    {
                                        strITEMVALUE = VB.Format(((DateTimePicker)objControl).Value, "yyyy-MM-dd");
                                    }
                                }
                                else
                                {
                                    strITEMVALUE = VB.Format(((DateTimePicker)objControl).Value, "yyyy-MM-dd");
                                }
                            }
                            //TextBox
                            if (objControl is TextBox)
                            {
                                bolIsItem = true;
                                strITEMTYPE = "TEXT";

                                if (isSpcPanel == true)
                                {
                                    if (clsXML.IsParent(objControl, pControl) == true)
                                    {
                                        string strITEMVALUETOT = objControl.Text.Trim().Replace("'", "`");
                                        int intLenTot = (int)ComFunc.GetWordByByte(strITEMVALUETOT);
                                        int intLen = 3999;
                                        if (intLenTot > 3999)
                                        {
                                            string strTmp0 = ComFunc.GetMidStr(strITEMVALUETOT, 0, 3999);
                                            if (VB.Right(strTmp0, 1) == "\r" || VB.Right(strTmp0, 1) == "?")
                                            {
                                                intLen = 3998;
                                            }
                                            strITEMVALUE = ComFunc.GetMidStr(strITEMVALUETOT, 0, intLen);
                                            strITEMVALUE1 = ComFunc.GetMidStr(strITEMVALUETOT, intLen, intLenTot - intLen);
                                        }
                                        else
                                        {
                                            strITEMVALUE = strITEMVALUETOT;
                                        }
                                    }
                                }
                                else
                                {
                                    string strITEMVALUETOT = objControl.Text.Trim().Replace("'", "`");
                                    int intLenTot = (int)ComFunc.GetWordByByte(strITEMVALUETOT);
                                    int intLen = 3999;
                                    if (intLenTot > 3999)
                                    {
                                        string strTmp0 = ComFunc.GetMidStr(strITEMVALUETOT, 0, 3999);
                                        if (VB.Right(strTmp0, 1) == "\r" || VB.Right(strTmp0, 1) == "?")
                                        {
                                            intLen = 3998;
                                        }
                                        strITEMVALUE = ComFunc.GetMidStr(strITEMVALUETOT, 0, intLen);
                                        strITEMVALUE1 = ComFunc.GetMidStr(strITEMVALUETOT, intLen, intLenTot - intLen);
                                    }
                                    else
                                    {
                                        strITEMVALUE = strITEMVALUETOT;
                                    }
                                }
                            }
                            //ComboBox
                            if (objControl is ComboBox)
                            {
                                bolIsItem = true;
                                strITEMTYPE = "COMBO";
                                if (isSpcPanel == true)
                                {
                                    if (clsXML.IsParent(objControl, pControl) == true)
                                    {
                                        strITEMVALUE = objControl.Text.Trim().Replace("'", "`");
                                    }
                                }
                                else
                                {
                                    strITEMVALUE = objControl.Text.Trim().Replace("'", "`");
                                }
                            }
                            //CheckBox
                            if (objControl is CheckBox)
                            {
                                bolIsItem = true;
                                strITEMTYPE = "CHECK";
                                if (isSpcPanel == true)
                                {
                                    if (clsXML.IsParent(objControl, pControl) == true)
                                    {
                                        strITEMVALUE = (((CheckBox)objControl).Checked == true ? "1" : "0");
                                    }
                                }
                                else
                                {
                                    strITEMVALUE = (((CheckBox)objControl).Checked == true ? "1" : "0");
                                }
                            }
                            //RadioButton(OptionButton)
                            if (objControl is RadioButton)
                            {
                                bolIsItem = true;
                                strITEMTYPE = "RADIO";
                                if (isSpcPanel == true)
                                {
                                    if (clsXML.IsParent(objControl, pControl) == true)
                                    {
                                        strITEMVALUE = (((RadioButton)objControl).Checked == true ? "1" : "0");
                                    }
                                }
                                else
                                {
                                    strITEMVALUE = (((RadioButton)objControl).Checked == true ? "1" : "0");
                                }
                            }

                            if (objControl is PictureBox)
                            {
                                bolIsItem = true;
                                strITEMTYPE = "IMAGE";
                                strITEMVALUE = "";
                                if (isSpcPanel == true)
                                {
                                    if (clsXML.IsParent(objControl, pControl) == true)
                                    {
                                        if (((PictureBox)objControl).Tag != null)
                                        {
                                            strITEMVALUE = ((PictureBox)objControl).Tag.ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    if (((PictureBox)objControl).Tag != null)
                                    {
                                        strITEMVALUE = ((PictureBox)objControl).Tag.ToString();
                                    }
                                }
                            }
                        }

                        if (bolIsItem == true)
                        {

                            if (arrySAVENO == null)
                            {
                                arrySAVENO = new string[0];
                                arryITEMCD = new string[0];
                                arryITEMNO = new string[0];
                                arryITEMINDEX = new string[0];
                                arryITEMTYPE = new string[0];
                                arryITEMVALUE = new string[0];
                                arryITEMVALUE1 = new string[0];
                                arryDSPSEQ = new string[0];
                            }

                            Array.Resize<string>(ref arrySAVENO, arrySAVENO.Length + 1);
                            Array.Resize<string>(ref arryITEMCD, arryITEMCD.Length + 1);
                            Array.Resize<string>(ref arryITEMNO, arryITEMNO.Length + 1);
                            Array.Resize<string>(ref arryITEMINDEX, arryITEMINDEX.Length + 1);
                            Array.Resize<string>(ref arryITEMTYPE, arryITEMTYPE.Length + 1);
                            Array.Resize<string>(ref arryITEMVALUE, arryITEMVALUE.Length + 1);
                            Array.Resize<string>(ref arryITEMVALUE1, arryITEMVALUE1.Length + 1);
                            Array.Resize<string>(ref arryDSPSEQ, arryDSPSEQ.Length + 1);

                            arrySAVENO[arrySAVENO.Length - 1] = dbSaveNoNew.ToString();
                            arryITEMCD[arrySAVENO.Length - 1] = strITEMCD;
                            arryITEMNO[arrySAVENO.Length - 1] = strITEMNO;
                            arryITEMINDEX[arrySAVENO.Length - 1] = strITEMINDEX;
                            arryITEMTYPE[arrySAVENO.Length - 1] = strITEMTYPE;
                            arryITEMVALUE[arrySAVENO.Length - 1] = strITEMVALUE;
                            arryITEMVALUE1[arrySAVENO.Length - 1] = strITEMVALUE1;
                            arryDSPSEQ[arrySAVENO.Length - 1] = strDSPSEQ;
                        }
                    }
                }

                if (arrySAVENO == null) return rtnVal;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "OCSFORMROW ";
                SQL = SQL + ComNum.VBLF + "    (SAVENO       ,ITEMCD       ,ITEMNO      ,ITEMINDEX    ,ITEMTYPE   , ITEMVALUE, DSPSEQ, ITEMVALUE1 )";
                SQL = SQL + ComNum.VBLF + "VALUES (";
                SQL = SQL + ComNum.VBLF + dbSaveNoNew.ToString() + ",";    //SAVENO
                SQL = SQL + ComNum.VBLF + " ?,";   //ITEMCD
                SQL = SQL + ComNum.VBLF + " ?,"; //ITEMNO
                SQL = SQL + ComNum.VBLF + " ?,"; //ITEMINDEX
                SQL = SQL + ComNum.VBLF + " ?,";   //ITEMTYPE
                SQL = SQL + ComNum.VBLF + " ?,";   //ITEMVALUE
                SQL = SQL + ComNum.VBLF + " ?,";   //DSPSEQ
                SQL = SQL + ComNum.VBLF + " ?";   //ITEMVALUE
                SQL = SQL + ComNum.VBLF + ")";

                //clsDB.ExecuteChartRow(SQL, dbSaveNoNew, arryITEMCD, arryITEMNO, arryITEMINDEX, arryITEMTYPE, arryITEMVALUE, arryDSPSEQ, arryITEMVALUE1);

                rtnVal = true;
                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="strEmrNo"></param>
        /// <param name="strUseId"></param>
        /// <returns></returns>
        public static bool DeleteOcsForm(PsmhDb pDbCon, string strOcsFormSeq, string strUseId)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(pDbCon);
            
            try
            {
                SQL = "SELECT ";
                SQL = SQL + ComNum.VBLF + "                A.SAVENO, A.CHARTUSEID";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "OCSFORMMST A";
                SQL = SQL + ComNum.VBLF + "    WHERE A.SAVENO = " + strOcsFormSeq;

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("삭제할 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                if (ComFunc.MsgBoxQ("기존내용을 삭제하시겠습니까?", "EMR 삭제", MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    clsDB.setRollbackTran(pDbCon);
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                
                string strCurDateTime = ComQuery.CurrentDateTime(pDbCon, "A");

                //OCSFORMMST

                double dblOcsFormHisNew = ComQuery.GetSequencesNo(pDbCon, "" + ComNum.DB_EMR + "GETSEQ_OCSFORMMSTHIS_SEQ");

                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "OCSFORMMSTHIS ";
                SQL = SQL + ComNum.VBLF + "    (SAVENOHIS, SAVENO, FORMNO, UPDATENO, CHARTDATE, CHARTTIME, ";
                SQL = SQL + ComNum.VBLF + "    ACPNO, PTNO, INOUTCLS, MEDFRDATE, MEDDEPTCD, MEDENDDATE,";
                SQL = SQL + ComNum.VBLF + "    WRITEDATE, WRITETIME, ";
                SQL = SQL + ComNum.VBLF + "    DCSAVENO, DCUSEID, DCDATE, DCTIME )";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + dblOcsFormHisNew + " AS SAVENOHIS, ";
                SQL = SQL + ComNum.VBLF + "    SAVENO, FORMNO, UPDATENO, CHARTDATE, CHARTTIME,";
                SQL = SQL + ComNum.VBLF + "    ACPNO, PTNO, INOUTCLS, MEDFRDATE, MEDDEPTCD, MEDENDDATE,";
                SQL = SQL + ComNum.VBLF + "    WRITEDATE, WRITETIME, ";
                SQL = SQL + ComNum.VBLF + "    SAVENO AS DCSAVENO, '" + strUseId + "', '" + strCurDateTime.Substring(0, 8) + "', '" + strCurDateTime.Substring(8, 6) + "' ";
                SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_EMR + "OCSFORMMST";
                SQL = SQL + ComNum.VBLF + "WHERE SAVENO = " + strOcsFormSeq;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                SQL = "";
                SQL = SQL + ComNum.VBLF + "DELETE FROM  " + ComNum.DB_EMR + "OCSFORMMST";
                SQL = SQL + ComNum.VBLF + "WHERE SAVENO = " + strOcsFormSeq;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                //OCSFORMROW
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "OCSFORMROWHIS ";
                SQL = SQL + ComNum.VBLF + "    ( SAVENO, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1 )";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "    SAVENO, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1";
                SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_EMR + "OCSFORMROW";
                SQL = SQL + ComNum.VBLF + "WHERE SAVENO = " + strOcsFormSeq;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                SQL = "";
                SQL = SQL + ComNum.VBLF + "DELETE FROM  " + ComNum.DB_EMR + "OCSFORMROW";
                SQL = SQL + ComNum.VBLF + "WHERE SAVENO = " + strOcsFormSeq;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(pDbCon);
                ComFunc.MsgBox("삭제하였습니다.");
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// OCSFORMROW 의 DATA를 폼에 뿌린다.
        /// </summary>
        /// <param name="frmXmlForm"></param>
        /// <param name="strOcsFormSeq"></param>
        /// <param name="blnErrOption"></param>
        /// <param name="OldYn"></param>
        /// <param name="dtp"></param>
        /// <param name="cb"></param>
        public static void LoadDataOcsFormRow(PsmhDb pDbCon, Control frmXmlForm, string strOcsFormSeq, bool blnErrOption, bool OldYn, DateTimePicker dtp, ComboBox cb, FarPoint.Win.Spread.FpSpread SpdWrite = null, string mDirection = "")
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT A.SAVENO, A.CHARTDATE, A.CHARTTIME, A.FORMNO, A.UPDATENO, ";
                SQL = SQL + ComNum.VBLF + "        R.ITEMCD, R.ITEMINDEX, R.ITEMTYPE, R.ITEMVALUE, R.ITEMVALUE1";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "OCSFORMMST A";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "OCSFORMROW R";
                SQL = SQL + ComNum.VBLF + "        ON R.SAVENO = A.SAVENO";
                SQL = SQL + ComNum.VBLF + "    WHERE A.SAVENO = " + VB.Val(strOcsFormSeq);

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                if (OldYn == true)
                {
                    dtp.Value = Convert.ToDateTime(ComFunc.FormatStrToDate((dt.Rows[0]["CHARTDATE"].ToString()).Trim(), "D"));
                    cb.Text = ComFunc.FormatStrToDate((dt.Rows[0]["CHARTTIME"].ToString()).Trim(), "M");
                    //dtp.Enabled = false;
                }

                int i = 0;
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    string strITEMCD = "";
                    //string strITEMINDEX = "";
                    string strITEMVALUE = "";
                    string strITEMVALUE1 = "";
                    string strITEMTYPE = "";

                    strITEMCD = dt.Rows[i]["ITEMCD"].ToString().Trim();

                    strITEMTYPE = dt.Rows[i]["ITEMTYPE"].ToString().Trim();
                    strITEMVALUE = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                    strITEMVALUE1 = dt.Rows[i]["ITEMVALUE1"].ToString().Trim();

                    string strFormNo = dt.Rows[i]["FORMNO"].ToString().Trim();
                    string strUpdateNo = dt.Rows[i]["UPDATENO"].ToString().Trim();
                    string strEmrNoOld = strOcsFormSeq;

                    Control[] tx = null;
                    Control obj = null;

                    if (strITEMTYPE == "DATE")
                    {
                        if (strITEMCD.Trim() != "dtMedFrDate")
                        {
                            tx = frmXmlForm.Controls.Find(strITEMCD, true);
                            if (tx.Length > 0)
                            {
                                obj = (DateTimePicker)tx[0];
                                if (strITEMVALUE != "")
                                {
                                    ((DateTimePicker)obj).Value = Convert.ToDateTime(strITEMVALUE);
                                }
                            }
                        }
                    }
                    else if (strITEMTYPE == "TEXT")
                    {
                        tx = frmXmlForm.Controls.Find(strITEMCD, true);
                        if (tx.Length > 0)
                        {
                            obj = (TextBox)tx[0];
                            //string strText = strITEMVALUE.Trim(); (strITEMVALUE.Replace("\n", "\r\n")).Replace("]]", "");
                            //string strText1 = strITEMVALUE1.Trim(); (strITEMVALUE1.Replace("\n", "\r\n")).Replace("]]", "");
                            string strText = strITEMVALUE.Trim(); strITEMVALUE.Replace("]]", "");
                            string strText1 = strITEMVALUE1.Trim(); strITEMVALUE1.Replace("]]", "");
                            if (((TextBox)obj).Multiline == false)
                            {
                                obj.Text = strText.Replace("\r\n", " ") + strText1.Replace("\r\n", " ");
                            }
                            else
                            {
                                obj.Text = strText + strText1;
                            }
                        }
                    }
                    else if (strITEMTYPE == "COMBO")
                    {
                        if (strITEMCD.Trim() != "txtMedFrTime")
                        {
                            tx = frmXmlForm.Controls.Find(strITEMCD, true);
                            if (tx.Length > 0)
                            {
                                obj = (ComboBox)tx[0];
                                obj.Text = VB.Replace(strITEMVALUE, "", "", 1, -1);
                            }
                        }
                    }
                    else if (strITEMTYPE == "CHECK")
                    {
                        tx = frmXmlForm.Controls.Find(strITEMCD, true);
                        if (tx.Length > 0)
                        {
                            obj = (CheckBox)tx[0];
                            ((CheckBox)obj).Checked = Convert.ToBoolean(VB.Val(strITEMVALUE));
                        }
                    }
                    else if (strITEMTYPE == "RADIO")
                    {
                        tx = frmXmlForm.Controls.Find(strITEMCD, true);
                        if (tx.Length > 0)
                        {
                            obj = (RadioButton)tx[0];
                            ((RadioButton)obj).Checked = Convert.ToBoolean(VB.Val(strITEMVALUE));
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                //SetSignImage(frmXmlForm, strOcsFormSeq);
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }













    }
}
