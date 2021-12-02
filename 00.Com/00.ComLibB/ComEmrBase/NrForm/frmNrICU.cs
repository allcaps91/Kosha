using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase;

namespace ComEmrBase
{
    public partial class frmNrICU : Form, EmrChartForm
    {

        #region // 폼에 사용하는 변수를 코딩하는 부분
        private const string mDirection = "H";   //기록지 작성방향(H: 옆으로, V:아래로)
        private const int mintTCol = 3;  //해드 칼럼수
        private const int mintTRow = 4;  //해드 로수
        private const int mintBRow = 3;  //밑줄
        private const int mintColW_I = 120;  //밑줄
        private const int mintColW_V = 70;  //밑줄
        public string mstrFormNameGb = "ICU관리";
        public string mstrFormNameWard = "혈역학적";
        private string mJOBGB = "IVT";
        //IIN : 의료관련감염예방 ssInfect_Sheet1   2017-11-10 이준우 추가
        //IVT : 혈액학적 ssVital_Sheet1
        //INU : 신경순환계  ssNuro
        //IRR : 호흡기계 ssRR_Sheet1
        //IBS : 투석기록 ssBst_Sheet1
        //IAC : 활동기록 ssAct_Sheet1
        //IIO : 섭취배셜 ssIO_Sheet1
        private string mstrDayTime = "00:00/07:00";
        private string mstrEveTime = "07:01/16:00";
        private string mstrNightTime = "16:01/24:00";
        

        FarPoint.Win.Spread.FpSpread mSpd;
        FarPoint.Win.Spread.SheetView mSpdView;

        private frmNrIcuTimeSet frmNrIcuTimeSetX = null;

        ContextMenu PopupMenu = null;
        int mPopRow = 0;
        int mPopCol = 0;

        private frmEmrChartNew frmEmrChartX;
        private frmOcrPrint frmOcrPrintX;


        #endregion

        #region // 기록지 관련 변수(기록지번호, 명칭 등 수정요함)
        public FormEmrMessage mEmrCallForm;

        public string mstrFormNo = "1761";
        public string mstrUpdateNo = "0";
        public string mstrFormText = "";
        public EmrPatient p = null;
        public string mstrEmrNo = "0";
        public string mstrMode = "W";
        private mtsPanel15.TransparentPanel panEditLock = null;

        #endregion

        #region //EmrChartForm
        public void SetChartHisMsg(string strEmrNo, string strOldGb)
        {
            return;
        }

        public double SaveDataMsg(string strFlag)
        {
            double dblEmrNo = pSaveData(strFlag);
            return dblEmrNo;
        }

        public bool DelDataMsg()
        {
            return pDelData();
        }

        public void ClearFormMsg()
        {
            mstrEmrNo = "0";
            pClearForm();
        }
        public void SetUserFormMsg(double dblMACRONO)
        {
            //TODO
            //pSetUserForm(dblMACRONO);
        }

        public bool SaveUserFormMsg(double dblMACRONO)
        {
            bool rtnVal = false;
            return rtnVal;
        }

        public int PrintFormMsg(string strPRINTFLAG)
        {
            int rtnVal = 0;
            if (strPRINTFLAG == "N")
            {
                frmEmrPrintOption frmEmrPrintOptionX = new frmEmrPrintOption();
                frmEmrPrintOptionX.ShowDialog();
            }

            if (clsFormPrint.mstrPRINTFLAG == "-1")
            {
                return rtnVal;
            }

            if (clsEmrQuery.SaveEmrXmlPrnYnForm(clsDB.DbCon, mstrEmrNo, "0") == false)
            {
                return rtnVal;
            }

            rtnVal = clsFormPrint.PrintFormLong(clsDB.DbCon, mstrFormNo, mstrUpdateNo, p, mstrEmrNo, panChart, "C");
            return rtnVal;
        }

        public int ToImageFormMsg(string strPRINTFLAG)
        {
            int rtnVal = clsFormPrint.PrintToTifFileLong(clsDB.DbCon, mstrFormNo, mstrUpdateNo, p, mstrEmrNo, panChart, "C");
            return rtnVal;
        }
        #endregion

        #region //외부에서 이벤트 받아서 처리 클리어, 저장, 삭제, 프린터

        /// <summary>
        /// 환자 받아서 기록지를 초기화 한다.
        /// </summary>
        public void gPatientinfoRecive(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode)
        {
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            p = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;

            //폼을 클리어하고 기록지 작성 정보등을 갱신한다.
            pClearForm();
            //기록지 정보를 세팅한다.
            pSetEmrInfo();
        }

        /// <summary>
        /// 폼이 로드할때 초기 세팅을 한다
        /// </summary>
        public void pInitForm()
        {
            pClearForm();
            pSetEmrInfo();
        }

        /// <summary>
        /// 폼별 특수한 초기화세팅이 필요할 경우 코딩.
        /// </summary>
        public void pInitFormSpc()
        {

        }

        public void pSetEmrInfo()
        {

        }

        /// <summary>
        /// 저장
        /// </summary>
        /// <returns></returns>
        public double pSaveData(string strFlag)
        {
            double dblEmrNo = 0;

            SetSpread();

            if (SaveTimeSet(mSpdView, mJOBGB) == false)
            {
                ComFunc.MsgBoxEx(this, "시간 저장중 에러가 발생했습니다.");
                return dblEmrNo;
            }

            int i = 0;
            int j = 0;

            for (i = 3; i < mSpdView.Columns.Count; i++)
            {
                bool bolIsItem = false;

                string[] arryEMRNO = null;
                string[] arryITEMCD = null;
                string[] arryITEMNO = null;
                string[] arryITEMINDEX = null;
                string[] arryITEMTYPE = null;
                string[] arryITEMVALUE = null;
                string[] arryITEMVALUE1 = null;
                string[] arryDSPSEQ = null;

                string strVital = "";

                if (mSpdView.Cells[mSpdView.RowCount - 1, i].Text == "Y")
                {
                    //if (VB.Val(mSpdView.Cells[mSpdView.Rows.Count - 4, i].Text.Trim()) > 0)
                    //{
                    //    continue;
                    //}
                    for (j = 4; j < mSpdView.RowCount - 4; j++)
                    {
                        if (mSpdView.Cells[j, i].Text.Trim() != "")
                        {
                            strVital = mSpdView.Cells[j, i].Text.Trim();
                        }
                    }
                    if (strVital == "")
                    {
                        continue;
                    }
                }

                if (mSpdView.Cells[mSpdView.RowCount - 1, i].Text == "Y")
                {
                    string strEmrNo = Convert.ToString(VB.Val(mSpdView.Cells[mSpdView.Rows.Count - 4, i].Text.Trim()));
                    string strChartDate = dtpFrDate.Value.ToString("yyyyMMdd");// mSpdView.Cells[0, i].Text.Trim().Replace("-", "");
                    string strChartTime = mSpdView.Cells[2, i].Text.Trim().Replace(":", "") + "00";
                    string strCHARTUSEID = "";
                    string strCOMPUSEID = "";
                    string strSAVEGB = "0";
                    string strFORMGB = "0";

                    if (VB.Val(strEmrNo) > 0)
                    {
                        strCHARTUSEID = mSpdView.Cells[mSpdView.Rows.Count - 3, i].Text.Trim();
                        strCOMPUSEID = mSpdView.Cells[mSpdView.Rows.Count - 3, i].Text.Trim();
                    }
                    else
                    {
                        strCHARTUSEID = clsType.User.IdNumber;
                        strCOMPUSEID = clsType.User.IdNumber;
                    }

                    for (j = 4; j < mSpdView.RowCount - 4; j++)
                    {
                        string strITEMCD = mSpdView.Cells[j, 0].Text.Trim();
                        string[] strItem = VB.Split(mSpdView.Cells[j, 0].Text.Trim(), "_");
                        string strITEMNO = "";
                        string strITEMINDEX = "-1";
                        if (strItem.Length > 0)
                        {
                            strITEMNO = strItem[0].Trim();
                        }
                        if (strItem.Length > 1)
                        {
                            strITEMINDEX = strItem[1].Trim();
                        }
                        string strDSPSEQ = "0";
                        string strITEMTYPE = "TEXT";
                        string strITEMVALUE = mSpdView.Cells[j, i].Text.Trim();
                        string strITEMVALUE1 = "";

                        if (arryEMRNO == null)
                        {
                            arryEMRNO = new string[0];
                            arryITEMCD = new string[0];
                            arryITEMNO = new string[0];
                            arryITEMINDEX = new string[0];
                            arryITEMTYPE = new string[0];
                            arryITEMVALUE = new string[0];
                            arryITEMVALUE1 = new string[0];
                            arryDSPSEQ = new string[0];
                        }

                        Array.Resize<string>(ref arryEMRNO, arryEMRNO.Length + 1);
                        Array.Resize<string>(ref arryITEMCD, arryITEMCD.Length + 1);
                        Array.Resize<string>(ref arryITEMNO, arryITEMNO.Length + 1);
                        Array.Resize<string>(ref arryITEMINDEX, arryITEMINDEX.Length + 1);
                        Array.Resize<string>(ref arryITEMTYPE, arryITEMTYPE.Length + 1);
                        Array.Resize<string>(ref arryITEMVALUE, arryITEMVALUE.Length + 1);
                        Array.Resize<string>(ref arryITEMVALUE1, arryITEMVALUE1.Length + 1);
                        Array.Resize<string>(ref arryDSPSEQ, arryDSPSEQ.Length + 1);

                        arryEMRNO[arryEMRNO.Length - 1] = "0";
                        arryITEMCD[arryEMRNO.Length - 1] = strITEMCD;
                        arryITEMNO[arryEMRNO.Length - 1] = strITEMNO;
                        arryITEMINDEX[arryEMRNO.Length - 1] = strITEMINDEX;
                        arryITEMTYPE[arryEMRNO.Length - 1] = strITEMTYPE;
                        arryITEMVALUE[arryEMRNO.Length - 1] = strITEMVALUE;
                        arryITEMVALUE1[arryEMRNO.Length - 1] = strITEMVALUE1;
                        arryDSPSEQ[arryEMRNO.Length - 1] = strDSPSEQ;
                    }

                    string strSAVECERT = "1";
                    if (clsEmrQuery.SaveFlowChart(clsDB.DbCon, p, mstrFormNo, mstrUpdateNo, strEmrNo, strChartDate, strChartTime, 
                        strCHARTUSEID, strCOMPUSEID, strSAVEGB, strSAVECERT, strFORMGB,
                        arryITEMCD, arryITEMNO, arryITEMINDEX, arryITEMTYPE, arryITEMVALUE, arryDSPSEQ, arryITEMVALUE1
                        ) == 0)
                    {
                        //에러임
                    }
                    else
                    {
                        //의료관련감염예방 저장시 MED_OCS.EMR_INFECT 테이블에 새로 발생된 EMRNO를 UPDATE 해줌.
                        //if (mJOBGB == "IIN")
                        //{
                        //    Update_InfectEmrno(p.acpNo, p.ptNo, arryITEMCD, strChartDate,strChartTime);
                        //}
                    }

                }
            }

            return dblEmrNo;
        }

        private void Update_InfectEmrno(string gAcpno, string gPtno, string[] gItemcd, string gDate, string gTime)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string StrEmrno = "0";
            string StrItemcd = "";

            StrItemcd = gItemcd[0];   //대표ITEMCD값만 읽어옴.
            StrEmrno = clsEmrQueryEtc.Read_EmrNo(clsDB.DbCon, p.acpNo, StrItemcd, gDate, gTime);

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                SQL = "";
                SQL = "UPDATE MED_OCS.EMR_INFECT SET ";
                SQL = SQL + ComNum.VBLF + " EMRNO = '"+ StrEmrno +"' ";
                SQL = SQL + ComNum.VBLF + " WHERE ACPNO = '" + p.acpNo + "' ";
                SQL = SQL + ComNum.VBLF + " AND PTNO = '" + p.ptNo + "' ";
                SQL = SQL + ComNum.VBLF + " AND CHARTDATE = '" + gDate + "' ";
                SQL = SQL + ComNum.VBLF + " AND CHARTTIME = '" + gTime + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

        }

        private bool SaveTimeSet(FarPoint.Win.Spread.SheetView SpdView, string strJOBGB)
        {
            bool rtnVal = false;

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon,"A");

                SQL = "";
                SQL = "DELETE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALTIME";
                SQL = SQL + ComNum.VBLF + "WHERE FORMNO = " + mstrFormNo;
                SQL = SQL + ComNum.VBLF + "    AND ACPNO = " + p.acpNo;
                SQL = SQL + ComNum.VBLF + "    AND JOBGB = '" + strJOBGB + "'";
                SQL = SQL + ComNum.VBLF + "    AND CHARTDATE = '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                for (i = 3; i < SpdView.Columns.Count; i++)
                {
                    SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRBVITALTIME ";
                    SQL = SQL + ComNum.VBLF + "(FORMNO, ACPNO, JOBGB , CHARTDATE, TIMEVALUE, SUBGB, WRITEDATE, WRITETIME, WRITEUSEID)";
                    SQL = SQL + ComNum.VBLF + "VALUES (";
                    SQL = SQL + ComNum.VBLF + "" + mstrFormNo + ",";
                    SQL = SQL + ComNum.VBLF + "" + p.acpNo + ",";
                    SQL = SQL + ComNum.VBLF + "'" + strJOBGB + "',";
                    SQL = SQL + ComNum.VBLF + "'" + dtpFrDate.Value.ToString("yyyyMMdd") + "',";
                    SQL = SQL + ComNum.VBLF + "'" + SpdView.Cells[2, i].Text.Trim().Replace(":", "") + "',";
                    if (SpdView.Cells[3, i].Text.Trim() == "합계")
                    {
                        SQL = SQL + ComNum.VBLF + "'1',";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "'0',";
                    }
                    SQL = SQL + ComNum.VBLF + "'" + VB.Left(strCurDateTime, 8) + "',";
                    SQL = SQL + ComNum.VBLF + "'" + VB.Right(strCurDateTime, 6) + "',";
                    SQL = SQL + ComNum.VBLF + "'" + clsType.User.IdNumber + "'";
                    SQL = SQL + ComNum.VBLF + ")";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        #endregion

        #region // 기록지 클리어, 저장, 삭제, 프린터

        /// <summary>
        /// 화면 정리
        /// </summary>
        public void pClearForm()
        {
            ////모든 컨트롤을 초기화 한다.

            pClearFormExcept();
        }

        /// <summary>
        /// 폼별로 EMR 작성 내역을 화면에 보여준다.
        /// </summary>
        private void pLoadEmrChartInfo()
        {
            LoadData(ssVital_Sheet1, "IVT");
            LoadData(ssNuro_Sheet1, "INU");
            LoadData(ssRR_Sheet1, "IRR");
            LoadData(ssBst_Sheet1, "IBS");
            LoadData(ssAct_Sheet1, "IAC");
            LoadData(ssIO_Sheet1, "IIO");
            //2017-11-10 이준우
            LoadData(ssInfect_Sheet1, "IIN");
            //==================
        }

        /// <summary>
        /// 데이타를 불러와 세팅한다
        /// </summary>
        /// <param name="SpdView"></param>
        /// <param name="strJOBGB"></param>
        private void LoadData(FarPoint.Win.Spread.SheetView SpdView, string strJOBGB)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            SQL = SQL + ComNum.VBLF + " SELECT A.EMRNO, A.CHARTDATE, A.CHARTTIME, A.CHARTUSEID, A.PRNTYN, ";
            SQL = SQL + ComNum.VBLF + "                B. ITEMCD, B.ITEMNO, B.ITEMINDEX, B.ITEMTYPE, B.ITEMVALUE,";
            SQL = SQL + ComNum.VBLF + "                U.USENAME";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B";
            SQL = SQL + ComNum.VBLF + "    ON  B.EMRNO = A.EMRNO";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BC ";
            SQL = SQL + ComNum.VBLF + "    ON BC.BASCD = B.ITEMCD ";
            SQL = SQL + ComNum.VBLF + "   AND BC.BSNSCLS = 'ICU관리' ";
            switch (strJOBGB)
            {
                case "IVT":
                    SQL = SQL + ComNum.VBLF + "    AND BC.UNITCLS = '혈역학적'";
                    break;
                case "INU":
                    SQL = SQL + ComNum.VBLF + "    AND BC.UNITCLS = '신경순환계'";
                    break;
                case "IRR":
                    SQL = SQL + ComNum.VBLF + "    AND BC.UNITCLS = '호흡기계'";
                    break;
                case "IBS":
                    SQL = SQL + ComNum.VBLF + "    AND BC.UNITCLS = '투석기록'";
                    break;
                case "IAC":
                    SQL = SQL + ComNum.VBLF + "    AND BC.UNITCLS = '활동기록'";
                    break;
                case "IIO":
                    SQL = SQL + ComNum.VBLF + "    AND BC.UNITCLS = '섭취배설'";
                    break;
                //2017-11-10 이준우 추가
                case "IIN":
                    SQL = SQL + ComNum.VBLF + "    AND BC.UNITCLS = '의료관련감염예방'";
                    break;
                //=====================
            }
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN  " + ComNum.DB_EMR + "AVIEWEMRUSER U";
            SQL = SQL + ComNum.VBLF + "    ON  U.USEID = A.CHARTUSEID";
            if (p.inOutCls == "I")
            {
                SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = '" + p.acpNo + "'";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "WHERE A.PTNO = '" + p.ptNo + "'";
            }
            SQL = SQL + ComNum.VBLF + "  AND A.FORMNO = " + mstrFormNo;
            SQL = SQL + ComNum.VBLF + "  AND A.UPDATENO = " + mstrUpdateNo;
            SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE = '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";
            SQL = SQL + ComNum.VBLF + "  AND A.CHARTUSEID <> '합계'";
            SQL = SQL + ComNum.VBLF + "ORDER BY A.CHARTDATE ASC , A.CHARTTIME ASC , A.EMRNO, BC.BASVAL";

            

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            int i = 0;
            int j = 0;
            int k = 0;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                for (j = 3; j < SpdView.Columns.Count; j++)
                {
                    if (SpdView.Cells[3, j].Text.Trim() == "합계")
                    {
                        continue;
                    }

                    if (SpdView.Cells[2, j].Text.Trim() == ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M"))
                    {
                        SpdView.Cells[0, j].Text = ComFunc.FormatStrToDate(dt.Rows[i]["CHARTDATE"].ToString().Trim(), "D");
                        SpdView.Cells[1, j].Text = clsEmrFunc.DutyGet(ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M"));
                        SpdView.Cells[2, j].Text = ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M");
                        if (dt.Rows[i]["CHARTUSEID"].ToString().Trim() == "합계")
                        {
                            SpdView.Cells[3, j].Text = dt.Rows[i]["CHARTUSEID"].ToString().Trim();
                        }
                        else
                        {
                            SpdView.Cells[3, j].Text = dt.Rows[i]["USENAME"].ToString().Trim();
                        }
                        SpdView.Cells[SpdView.Rows.Count - 4, j].Text = dt.Rows[i]["EMRNO"].ToString().Trim();
                        SpdView.Cells[SpdView.Rows.Count - 3, j].Text = dt.Rows[i]["CHARTUSEID"].ToString().Trim();
                        SpdView.Cells[SpdView.Rows.Count - 2, j].Text = dt.Rows[i]["PRNTYN"].ToString().Trim();
                        if (dt.Rows[i]["CHARTUSEID"].ToString().Trim() == clsType.User.IdNumber)
                        {
                            SpdView.Cells[4, j, SpdView.Rows.Count - 5, j].Locked = false;
                        }
                        else
                        {
                            SpdView.Cells[4, j, SpdView.Rows.Count - 5, j].Locked = false;
                        }

                        for (k = 4; k < SpdView.RowCount - 4; k++)
                        {
                            if (dt.Rows[i]["ITEMCD"].ToString().Trim() == SpdView.Cells[k, 0].Text.Trim())
                            {
                                SpdView.Cells[k, j].Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                                if (strJOBGB == "IIO")
                                {
                                    if ((dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000030622") || (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000030623"))
                                    {
                                        SpdView.Cells[k, j].BackColor = Color.LightBlue;
                                    }
                                }
                            }
                        }
                    }
                    if (i >= dt.Rows.Count) break;
                }
                if (i >= dt.Rows.Count) break;
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

            if (strJOBGB == "IIO")
            {
                LoadEmrChartInfoTot(SpdView, strJOBGB);
            }
        }

        /// <summary>
        /// 저장된 데이타를 조회한다
        /// </summary>
        /// <param name="SpdView"></param>
        /// <param name="strJOBGB"></param>
        private void LoadEmrChartInfoTot(FarPoint.Win.Spread.SheetView SpdView, string strJOBGB)
        {

            if (strJOBGB != "IIO") return;

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            SQL = SQL + ComNum.VBLF + " SELECT A.EMRNO, A.CHARTDATE, A.CHARTTIME, A.CHARTUSEID, A.PRNTYN, ";
            SQL = SQL + ComNum.VBLF + "                B. ITEMCD, B.ITEMNO, B.ITEMINDEX, B.ITEMTYPE, B.ITEMVALUE,";
            SQL = SQL + ComNum.VBLF + "                U.USENAME";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B";
            SQL = SQL + ComNum.VBLF + "    ON  B.EMRNO = A.EMRNO";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BC ";
            SQL = SQL + ComNum.VBLF + "    ON BC.BASCD = B.ITEMCD ";
            SQL = SQL + ComNum.VBLF + "   AND BC.BSNSCLS = 'ICU관리' ";
            switch (strJOBGB)
            {
                case "IVT":
                    SQL = SQL + ComNum.VBLF + "    AND BC.UNITCLS = '혈역학적'";
                    break;
                case "INU":
                    SQL = SQL + ComNum.VBLF + "    AND BC.UNITCLS = '신경순환계'";
                    break;
                case "IRR":
                    SQL = SQL + ComNum.VBLF + "    AND BC.UNITCLS = '호흡기계'";
                    break;
                case "IBS":
                    SQL = SQL + ComNum.VBLF + "    AND BC.UNITCLS = '투석기록'";
                    break;
                case "IAC":
                    SQL = SQL + ComNum.VBLF + "    AND BC.UNITCLS = '활동기록'";
                    break;
                case "IIO":
                    SQL = SQL + ComNum.VBLF + "    AND BC.UNITCLS = '섭취배설'";
                    break;
                //2017-11-10 이준우
                case "IIN":
                    SQL = SQL + ComNum.VBLF + "    AND BC.UNITCLS = '의료관련감염예방'";
                    break;
                //=================
            }
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN  " + ComNum.DB_EMR + "AVIEWEMRUSER U";
            SQL = SQL + ComNum.VBLF + "    ON  U.USEID = A.CHARTUSEID";
            if (p.inOutCls == "I")
            {
                SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = '" + p.acpNo + "'";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "WHERE A.PTNO = '" + p.ptNo + "'";
            }
            SQL = SQL + ComNum.VBLF + "  AND A.FORMNO = " + mstrFormNo;
            SQL = SQL + ComNum.VBLF + "  AND A.UPDATENO = " + mstrUpdateNo;
            SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE = '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";
            SQL = SQL + ComNum.VBLF + "  AND A.CHARTUSEID = '합계'";
            SQL = SQL + ComNum.VBLF + "ORDER BY A.CHARTDATE ASC , A.CHARTTIME ASC , A.EMRNO, BC.BASVAL";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            int i = 0;
            int j = 0;
            int k = 0;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                for (j = 3; j < SpdView.Columns.Count; j++)
                {
                    if (SpdView.Cells[3, j].Text.Trim() != "합계")
                    {
                        continue;
                    }

                    if (SpdView.Cells[2, j].Text.Trim() == ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M"))
                    {
                        SpdView.Cells[0, j].Text = ComFunc.FormatStrToDate(dt.Rows[i]["CHARTDATE"].ToString().Trim(), "D");
                        SpdView.Cells[1, j].Text = clsEmrFunc.DutyGet(ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M"));
                        SpdView.Cells[2, j].Text = ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M");
                        if (dt.Rows[i]["CHARTUSEID"].ToString().Trim() == "합계")
                        {
                            SpdView.Cells[3, j].Text = dt.Rows[i]["CHARTUSEID"].ToString().Trim();
                        }
                        else
                        {
                            SpdView.Cells[3, j].Text = dt.Rows[i]["USENAME"].ToString().Trim();
                        }


                        SpdView.Cells[SpdView.Rows.Count - 4, j].Text = dt.Rows[i]["EMRNO"].ToString().Trim();
                        SpdView.Cells[SpdView.Rows.Count - 3, j].Text = dt.Rows[i]["CHARTUSEID"].ToString().Trim();
                        SpdView.Cells[SpdView.Rows.Count - 2, j].Text = dt.Rows[i]["PRNTYN"].ToString().Trim();
                        if (dt.Rows[i]["CHARTUSEID"].ToString().Trim() == "합계")
                        {
                            SpdView.Cells[4, j, SpdView.Rows.Count - 5, j].Locked = true;
                        }
                        else
                        {
                            if (dt.Rows[i]["CHARTUSEID"].ToString().Trim() == clsType.User.IdNumber)
                            {
                                SpdView.Cells[4, j, SpdView.Rows.Count - 5, j].Locked = false;
                            }
                            else
                            {
                                SpdView.Cells[4, j, SpdView.Rows.Count - 5, j].Locked = true;
                            }
                        }

                        for (k = 4; k < SpdView.RowCount - 4; k++)
                        {
                            if (dt.Rows[i]["ITEMCD"].ToString().Trim() == SpdView.Cells[k, 0].Text.Trim())
                            {
                                SpdView.Cells[k, j].Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                                //i = i + 1;
                                //if (i >= dt.Rows.Count) break;
                            }
                        }
                    }
                    if (i >= dt.Rows.Count) break;
                }
                if (i >= dt.Rows.Count) break;
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// 저장
        /// </summary>
        /// <returns></returns>
        private double pSaveEmrData(string strFlag)
        {
            double dblEmrNo = 0;

            return dblEmrNo;
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <returns></returns>
        public bool pDelData(string strUseId = "")
        {
            if (VB.Val(mstrEmrNo) == 0)
            {
                return false;
            }

            if (strUseId != "합계")
            {
                if (VB.Val(mstrEmrNo) != 0)
                {
                    //if (clsEmrQuery.IsChangeAuthCopy(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber) == false) return false;
                }
                strUseId = clsType.User.IdNumber;
            }

            if (clsXML.gDeleteEmrXmlNotAuth(clsDB.DbCon, mstrEmrNo, strUseId) == true)
            {
                mstrEmrNo = "0";
                SetSpread();
                SetDefaultData(mSpdView, mJOBGB);
                mEmrCallForm.MsgDelete();
            }
            return true;
        }

        /// <summary>
        /// 기록지를 출력한다.
        /// </summary>
        public void pPrintForm()
        {

        }
        
        #endregion


        #region //공통 모듈 = 작업구분
        private void SetDefault()
        {
            //IIN : 의료관련감염예방 ssInfect_Sheet1   2017-11-10 이준우 추가
            //IVT : 혈액학적 ssVital_Sheet1
            //INU : 신경순환계  ssNuro
            //IRR : 호흡기계 ssRR_Sheet1
            //IBS : 투석기록 ssBst_Sheet1
            //IAC : 활동기록 ssAct_Sheet1
            //IIO : 섭취배셜 ssIO_Sheet1
            panIoOrder.Height = 30;
            ssIoMix_Sheet1.RowCount = 0;
            ssIoForm_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 0;
            panTot.Visible = false;

            SetDefaultData(ssVital_Sheet1, "IVT");
            SetDefaultData(ssNuro_Sheet1, "INU");
            SetDefaultData(ssRR_Sheet1, "IRR");
            SetDefaultData(ssBst_Sheet1, "IBS");
            SetDefaultData(ssAct_Sheet1, "IAC");
            SetDefaultData(ssIO_Sheet1, "IIO");
            //2017-11-10 이준우--------------------
            SetDefaultData(ssInfect_Sheet1, "IIN");
            //=====================================
        }

        private void VisibleSetTrue()
        {
            int i = 0;

            for (i = 3; i < ssVital_Sheet1.Columns.Count; i++)
            {
                ssVital_Sheet1.Columns[i].Visible = true;
            }
            for (i = 3; i < ssNuro_Sheet1.Columns.Count; i++)
            {
                ssNuro_Sheet1.Columns[i].Visible = true;
            }
            for (i = 3; i < ssRR_Sheet1.Columns.Count; i++)
            {
                ssRR_Sheet1.Columns[i].Visible = true;
            }
            for (i = 3; i < ssBst_Sheet1.Columns.Count; i++)
            {
                ssBst_Sheet1.Columns[i].Visible = true;
            }
            for (i = 3; i < ssAct_Sheet1.Columns.Count; i++)
            {
                ssAct_Sheet1.Columns[i].Visible = true;
            }
            for (i = 3; i < ssIO_Sheet1.Columns.Count; i++)
            {
                ssIO_Sheet1.Columns[i].Visible = true;
            }
            //2017-11-10 이준우
            for (i = 3; i < ssInfect_Sheet1.Columns.Count; i++)
            {
                ssInfect_Sheet1.Columns[i].Visible = true;
            }
            //==================
        }

        private string strUNITCLS1()
        {
            string rtnVal = "";

            switch (mJOBGB)
            {
                case "IVT":
                    rtnVal = rtnVal + ComNum.VBLF + "    AND BC.UNITCLS = '혈역학적'";
                    break;
                case "INU":
                    rtnVal = rtnVal + ComNum.VBLF + "    AND BC.UNITCLS = '신경순환계'";
                    break;
                case "IRR":
                    rtnVal = rtnVal + ComNum.VBLF + "    AND BC.UNITCLS = '호흡기계'";
                    break;
                case "IBS":
                    rtnVal = rtnVal + ComNum.VBLF + "    AND BC.UNITCLS = '투석기록'";
                    break;
                case "IAC":
                    rtnVal = rtnVal + ComNum.VBLF + "    AND BC.UNITCLS = '활동기록'";
                    break;
                case "IIO":
                    rtnVal = rtnVal + ComNum.VBLF + "    AND BC.UNITCLS = '섭취배설'";
                    break;
                //2017-11-10 이준우
                case "IIN":
                    rtnVal = rtnVal + ComNum.VBLF + "    AND BC.UNITCLS = '의료관련감염예방'";
                    break;
                //=================
            }

            return rtnVal;
        }

        private void tabChart_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabChart.SelectedTab == tabNuro)
            {
                mJOBGB = "INU";
            }
            else if (tabChart.SelectedTab == tabRR)
            {
                mJOBGB = "IRR";
            }
            else if (tabChart.SelectedTab == tabBst)
            {
                mJOBGB = "IBS";
            }
            else if (tabChart.SelectedTab == tabAct)
            {
                mJOBGB = "IAC";
            }
            else if (tabChart.SelectedTab == tabIO)
            {
                mJOBGB = "IIO";
            }
            //2017-11-10 이준우
            else if (tabChart.SelectedTab == tabInfection)
            {
                mJOBGB = "IIN";
            }
            //=================
            else //tabVital
            {
                mJOBGB = "IVT";
            }

            SetSpread();
        }

        private void SetFormNameGb()
        {
            if (mJOBGB == "INU")
            {
                mstrFormNameGb = "신경순환계";
            }
            else if (mJOBGB == "IRR")
            {
                mstrFormNameGb = "호흡기계";
            }
            else if (mJOBGB == "IBS")
            {
                mstrFormNameGb = "투석기록";
            }
            else if (mJOBGB == "IAC")
            {
                mstrFormNameGb = "활동기록";
            }
            else if (mJOBGB == "IIO")
            {
                mstrFormNameGb = "섭취배설";
            }
            //2017-11-10 이준우
            else if (mJOBGB == "IIN")
            {
                mstrFormNameGb = "의료관련감염예방";
            }
            //================
            else //tabVital
            {
                mstrFormNameGb = "혈역학적";
            }
        }

        private void LoadJobData()
        {
            if (mJOBGB == "INU")
            {
                SetDefaultData(ssNuro_Sheet1, "INU");
                LoadData(ssNuro_Sheet1, mJOBGB);
            }
            else if (mJOBGB == "IRR")
            {
                SetDefaultData(ssRR_Sheet1, "IRR");
                LoadData(ssRR_Sheet1, mJOBGB);
            }
            else if (mJOBGB == "IBS")
            {
                SetDefaultData(ssBst_Sheet1, "IBS");
                LoadData(ssBst_Sheet1, mJOBGB);
            }
            else if (mJOBGB == "IAC")
            {
                SetDefaultData(ssAct_Sheet1, "IAC");
                LoadData(ssAct_Sheet1, mJOBGB);
            }
            else if (mJOBGB == "IIO")
            {
                SetDefaultData(ssIO_Sheet1, "IIO");
                LoadData(ssIO_Sheet1, mJOBGB);
            }
            //2017-11-10 이준우
            else if (mJOBGB == "IIN")
            {
                SetDefaultData(ssInfect_Sheet1, "IIN");
                LoadData(ssInfect_Sheet1, mJOBGB);
            }
            //=================
            else //tabVital
            {
                SetDefaultData(ssVital_Sheet1, "IVT");
                LoadData(ssVital_Sheet1, mJOBGB);
            }
        }

        private void SetSpread()
        {
            if (mJOBGB == "INU")
            {
                mSpd = ssNuro;
                mSpdView = ssNuro_Sheet1;
            }
            else if (mJOBGB == "IRR")
            {
                mSpd = ssRR;
                mSpdView = ssRR_Sheet1;
            }
            else if (mJOBGB == "IBS")
            {
                mSpd = ssBst;
                mSpdView = ssBst_Sheet1;
            }
            else if (mJOBGB == "IAC")
            {
                mSpd = ssAct;
                mSpdView = ssAct_Sheet1;
            }
            else if (mJOBGB == "IIO")
            {
                mSpd = ssIO;
                mSpdView = ssIO_Sheet1;
            }
            //2017-11-10 이준우
            else if (mJOBGB == "IIN")
            {
                mSpd = ssInfect;
                mSpdView = ssInfect_Sheet1;
            }
            //==================
            else //tabVital
            {
                mSpd = ssVital;
                mSpdView = ssVital_Sheet1;
            }
        }

        private void VisibleSet(string strTime)
        {
            VisibleSetDt(ssVital_Sheet1, VB.Split(strTime, "/")[0], VB.Split(strTime, "/")[1]);
            VisibleSetDt(ssNuro_Sheet1, VB.Split(strTime, "/")[0], VB.Split(strTime, "/")[1]);
            VisibleSetDt(ssRR_Sheet1, VB.Split(strTime, "/")[0], VB.Split(strTime, "/")[1]);
            VisibleSetDt(ssBst_Sheet1, VB.Split(strTime, "/")[0], VB.Split(strTime, "/")[1]);
            VisibleSetDt(ssAct_Sheet1, VB.Split(strTime, "/")[0], VB.Split(strTime, "/")[1]);
            VisibleSetDt(ssIO_Sheet1, VB.Split(strTime, "/")[0], VB.Split(strTime, "/")[1]);
            //2017-11-10 이준우
            VisibleSetDt(ssInfect_Sheet1, VB.Split(strTime, "/")[0], VB.Split(strTime, "/")[1]);
            //================
        }

        #endregion

        private void pClearFormExcept()
        {
            SetDefault();

            SetGunPrio();
        }

        private void SetGunPrio()
        {
            //동국대 경주병원만
            lblGun.Text = "";
            lblPrio.Text = "";

            return;

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT * FROM ICU_Priority ";
            SQL = SQL + ComNum.VBLF + "    WHERE PATIENT_NO = '" + p.ptNo + "'  ";
            SQL = SQL + ComNum.VBLF + "      AND ORDER_DATE = TO_DATE('" + dtpFrDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {

                    if (VB.Val(dt.Rows[i]["P1"].ToString().Trim()) != 0)
                    {
                        lblPrio.Text = "1";
                    }
                    else if (VB.Val(dt.Rows[i]["P2"].ToString().Trim()) != 0)
                    {
                        lblPrio.Text = "2";
                    }
                    else if (VB.Val(dt.Rows[i]["P3"].ToString().Trim()) != 0)
                    {
                        lblPrio.Text = "3";
                    }
                    else if (VB.Val(dt.Rows[i]["P4a"].ToString().Trim()) != 0)
                    {
                        lblPrio.Text = "4a";
                    }
                    else if (VB.Val(dt.Rows[i]["P4b"].ToString().Trim()) != 0)
                    {
                        lblPrio.Text = "4b";
                    }
                }
            }
            dt.Dispose();
            dt = null;

            SQL = " SELECT   BUN FROM MED_OCS.NR_BUN_TONG  ";
            SQL = SQL + ComNum.VBLF + " WHERE    PATIENT_NO    = '" + p.ptNo + "'  ";
            SQL = SQL + ComNum.VBLF + " AND  ACT_DATE    = (";
            SQL = SQL + ComNum.VBLF + "                    SELECT   MAX(ACT_DATE) FROM MED_OCS.NR_BUN_TONG  ";
            SQL = SQL + ComNum.VBLF + "                    WHERE  ACT_DATE    >= TO_DATE('" + ComFunc.FormatStrToDate(p.medFrDate, "D") + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "                    AND  ACT_DATE    < TO_DATE('" + dtpFrDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "                    AND    PATIENT_NO    = '" + p.ptNo + "' )  ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dt.Rows.Count > 0)
            {
                lblGun.Text = dt.Rows[0]["BUN"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;
        }

        private void SetDefaultData(FarPoint.Win.Spread.SheetView SpdView, string strJOBGB)
        {
            //일자별 등록된 것이 있는지 파악해서 있으면 세팅을 하고
            //없으면 기본을 가지고 세팅을 한다.
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SetTopRow(SpdView);

            string strBASEXNAME = "";
            int intS = 0;

            Cursor.Current = Cursors.WaitCursor;
            SQL = "SELECT B.BASCD, B.BASNAME, B.BASEXNAME, B.DISSEQNO, A.CHARTDATE ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET A";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "    ON B.BASCD = A.ITEMCD";
            SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = 'ICU관리'";
            switch (strJOBGB)
            {
                case "IVT":
                    SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '혈역학적'";
                    break;
                case "INU":
                    SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '신경순환계'";
                    break;
                case "IRR":
                    SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '호흡기계'";
                    break;
                case "IBS":
                    SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '투석기록'";
                    break;
                case "IAC":
                    SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '활동기록'";
                    break;
                case "IIO":
                    SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '섭취배설'";
                    break;
                case "IIN":
                    SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '의료관련감염예방'";
                    break;
            }
            SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND A.JOBGB = '" + strJOBGB + "'";
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = (SELECT MAX(CHARTDATE) AS CHARTDATE ";
            SQL = SQL + ComNum.VBLF + "                                        FROM " + ComNum.DB_EMR + "AEMRBVITALSET ";
            SQL = SQL + ComNum.VBLF + "                                        WHERE ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "                                            AND JOBGB = '" + strJOBGB + "'";
            SQL = SQL + ComNum.VBLF + "                                            AND CHARTDATE <= '" + dtpFrDate.Value.ToString("yyyyMMdd") + "')";
            SQL = SQL + ComNum.VBLF + "ORDER BY B.DISSEQNO, B.BASNAME";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    SpdView.RowCount = SpdView.RowCount + 1;
                    clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);
                    clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_L, "", true);
                    clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 2, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);

                    SpdView.Cells[SpdView.RowCount - 1, 0].Text = dt.Rows[i]["BASCD"].ToString().Trim();
                    if (strBASEXNAME != dt.Rows[i]["BASEXNAME"].ToString().Trim())
                    {
                        SpdView.Cells[SpdView.RowCount - 1, 1].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                        if (i != 0)
                        {
                            SpdView.AddSpanCell(intS, 1, SpdView.RowCount - 1 - intS, 1);
                        }
                        intS = SpdView.RowCount - 1;
                    }
                    strBASEXNAME = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                    SpdView.Cells[SpdView.RowCount - 1, 2].Text = dt.Rows[i]["BASNAME"].ToString().Trim();
                    if (strJOBGB == "IIO")
                    {
                        if ((dt.Rows[i]["BASCD"].ToString().Trim() == "I0000030622") || (dt.Rows[i]["BASCD"].ToString().Trim() == "I0000030623"))
                        {
                            SpdView.Cells[SpdView.RowCount - 1, 2].BackColor = Color.LightBlue;
                            SpdView.Cells[SpdView.RowCount - 1, 2].BackColor = Color.LightBlue;
                        }
                    }
                    //SpdView.SetColumnWidth(2, Convert.ToInt32(SpdView.GetPreferredColumnWidth(i)) + 4);
                    SpdView.SetRowHeight(SpdView.RowCount - 1, ComNum.SPDROWHT);
                }
                dt.Dispose();
                dt = null;
                SpdView.AddSpanCell(intS, 1, SpdView.RowCount - intS, 1);

                SetButtonRow(SpdView);
                SetTimeSet(SpdView, strJOBGB);
                return;
            }
            else
            {
                dt.Dispose();
                dt = null;
                SetButtonRow(SpdView);
                SetTimeSet(SpdView, strJOBGB);
            }
            
            Cursor.Current = Cursors.Default;

        }

        private void SetTimeSet(FarPoint.Win.Spread.SheetView SpdView, string strJOBGB)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int j = 0;

            SQL = "";
            SQL = "SELECT TIMEVALUE, SUBGB ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALTIME";
            SQL = SQL + ComNum.VBLF + "WHERE ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND JOBGB = '" + strJOBGB + "'";
            SQL = SQL + ComNum.VBLF + "    AND CHARTDATE = '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";
            SQL = SQL + ComNum.VBLF + "ORDER BY TO_NUMBER(TIMEVALUE)";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    SpdView.Columns.Count = SpdView.Columns.Count + 1;
                    SpdView.SetColumnWidth(SpdView.Columns.Count - 1, mintColW_V);

                    clsSpread.SetTypeAndValue(SpdView, 0, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    SpdView.Cells[0, SpdView.Columns.Count - 1].BackColor = Color.LightBlue;
                    clsSpread.SetTypeAndValue(SpdView, 1, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    SpdView.Cells[1, SpdView.Columns.Count - 1].BackColor = Color.LightBlue;
                    clsSpread.SetTypeAndValue(SpdView, 2, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, ComFunc.FormatStrToDate(dt.Rows[i]["TIMEVALUE"].ToString().Trim(), "M"), false);
                    SpdView.Cells[2, SpdView.Columns.Count - 1].BackColor = Color.LightBlue;
                    clsSpread.SetTypeAndValue(SpdView, 3, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    SpdView.Cells[3, SpdView.Columns.Count - 1].BackColor = Color.LightBlue;
                    if (dt.Rows[i]["SUBGB"].ToString().Trim() == "1")
                    {
                        SpdView.Cells[3, SpdView.Columns.Count - 1].Text = "합계";
                    }

                    for (j = 4; j < SpdView.RowCount - 4; j++)
                    {
                        clsSpread.SetTypeAndValue(SpdView, j, SpdView.Columns.Count - 1, "TypeText", false, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);
                    }
                    clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 4, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 3, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 2, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                }
                dt.Dispose();
                dt = null;
                return;
            }
            dt.Dispose();
            dt = null;

            for (i = 0; i < 25; i++)
            {
                SpdView.Columns.Count = SpdView.Columns.Count + 1;
                SpdView.SetColumnWidth(SpdView.Columns.Count - 1, mintColW_V);

                clsSpread.SetTypeAndValue(SpdView, 0, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                SpdView.Cells[0, SpdView.Columns.Count - 1].BackColor = Color.LightBlue;
                clsSpread.SetTypeAndValue(SpdView, 1, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                SpdView.Cells[1, SpdView.Columns.Count - 1].BackColor = Color.LightBlue;
                clsSpread.SetTypeAndValue(SpdView, 2, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, ComFunc.SetAutoZero(i.ToString().Trim(), 2) + ":00", false);
                SpdView.Cells[2, SpdView.Columns.Count - 1].BackColor = Color.LightBlue;
                clsSpread.SetTypeAndValue(SpdView, 3, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                SpdView.Cells[3, SpdView.Columns.Count - 1].BackColor = Color.LightBlue;

                for (j = 4; j < SpdView.RowCount - 4; j++)
                {
                    clsSpread.SetTypeAndValue(SpdView, j, SpdView.Columns.Count - 1, "TypeText", false, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);
                }
                clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 4, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 3, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 2, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, SpdView.Columns.Count - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            }
        }

        private void InitSpdSet(FarPoint.Win.Spread.SheetView SpdView)
        {
            SpdView.RowCount = 0;
            SpdView.ColumnCount = 0;

            SpdView.RowCount = mintTRow;
            SpdView.ColumnCount = mintTCol;
            SpdView.SetRowHeight(-1, ComNum.SPDROWHT);
            SpdView.SetColumnWidth(-1, mintColW_I);
            SpdView.SetColumnWidth(1, 60);
            SpdView.SetColumnWidth(2, 160);
            SpdView.Columns[0].Visible = false;
            SpdView.Rows[0].Visible = false;
            SpdView.Rows[1].Visible = false;
        }

        private void SetTopRow(FarPoint.Win.Spread.SheetView SpdView)
        {
            InitSpdSet(SpdView);

            clsSpread.SetTypeAndValue(SpdView, 0, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(SpdView, 1, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(SpdView, 2, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(SpdView, 3, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);

            clsSpread.SetTypeAndValue(SpdView, 0, 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "작성일자", false);
            SpdView.Cells[0, 1, 0, 2].BackColor = Color.LightBlue;
            SpdView.AddSpanCell(0, 1, 1, 2);
            clsSpread.SetTypeAndValue(SpdView, 1, 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "Duty", false);
            SpdView.Cells[1, 1, 1, 2].BackColor = Color.LightBlue;
            SpdView.AddSpanCell(1, 1, 1, 2);
            clsSpread.SetTypeAndValue(SpdView, 2, 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "작성시간", false);
            SpdView.Cells[2, 1, 2, 2].BackColor = Color.LightBlue;
            SpdView.AddSpanCell(2, 1, 1, 2);
            clsSpread.SetTypeAndValue(SpdView, 3, 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "작성자", false);
            SpdView.Cells[3, 1, 3, 2].BackColor = Color.LightBlue;
            SpdView.AddSpanCell(3, 1, 1, 2);
        }

        private void SetButtonRow(FarPoint.Win.Spread.SheetView SpdView)
        {
            SpdView.RowCount = SpdView.RowCount + 1;
            clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "EMRNO", false);
            clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);

            SpdView.RowCount = SpdView.RowCount + 1;
            clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "USEID", false);
            clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);

            SpdView.RowCount = SpdView.RowCount + 1;
            clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "PRNTYN", false);
            clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            SpdView.RowCount = SpdView.RowCount + 1;
            clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "CHANG", false);
            clsSpread.SetTypeAndValue(SpdView, SpdView.RowCount - 1, 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            SpdView.Rows[SpdView.RowCount - 4].Visible = false;
            SpdView.Rows[SpdView.RowCount - 3].Visible = false;
            SpdView.Rows[SpdView.RowCount - 2].Visible = false;
            SpdView.Rows[SpdView.RowCount - 1].Visible = false;
        }

        public frmNrICU()
        {
            InitializeComponent();
        }

        public frmNrICU(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm)
        {
            InitializeComponent();

            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            p = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            mEmrCallForm = pEmrCallForm;
        }

        public frmNrICU(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm, Panel panParent)
        {
            InitializeComponent();

            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            p = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            mEmrCallForm = pEmrCallForm;

            if (panParent.Parent.Parent.Parent.Parent.Parent.Name == "frmDrOcsMain")
            {
                panIoOrder.Visible = false;
                mbtnSaveAll.Visible = false;
                mbtnDeleteAll.Visible = false;
                txtTime.Visible = false;
                mbtnInsert.Visible = false;
                mbtnInsertTime.Visible = false;
                mbtnUpdate.Visible = false;
                mbtnPrint.Visible = false;
            }
        }

        private void frmNrICU_Load(object sender, EventArgs e)
        {
            ssIoForm_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 0;
            SetDutyTime();
            mSpd = ssVital;
            mSpdView = ssVital_Sheet1;

            SetPatInfo();

            if (VB.Val(mstrEmrNo) != 0)
            {
                clsEmrFunc.SetMedFrEndDate(clsDB.DbCon, mstrEmrNo, p, dtpFrDate, null);
            }
            else
            {
                dtpFrDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon,"D"), "D"));
            }
            // 2017-04-08 유진호 ICU -> ICU, MIC, SIC
            if (clsType.User.DeptCode == "ICU" || clsType.User.DeptCode == "MIC" || clsType.User.DeptCode == "SIC")
            {
                mbtnPainList.Visible = true;
            }
        }

        private void SetPatInfo()
        {
            lblGun.Text = "";
            lblPrio.Text = "";
            lblHD.Text = "";
            lblICUD.Text = "";
            ssDise_Sheet1.RowCount = 0;

            string strCurDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon,"D"), "D");

            if (p == null) return;

            lblHD.Text = (VB.DateDiff("d", Convert.ToDateTime(ComFunc.FormatStrToDate(p.medFrDate, "D")), Convert.ToDateTime(strCurDate)) + 1).ToString();
            //TODO
            //GetDataICUD(strCurDate);
            //GetDataDiag(p.inOutCls, p.medFrDate, p.medEndDate, p.medDeptCd);

        }

        private void SetPatInfoDay()
        {
            lblGun.Text = "";
        }

        private void GetDataICUD(string strCurDate)
        {
            //동국대 경주병원만 일단 사용
            lblICUD.Text = "";
            return;

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            
            string strIcuDate = "";

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(MAX(C_INDATE),'YYYY-MM-DD')  AS C_INDATE ";
            SQL = SQL + ComNum.VBLF + "FROM MED_OCS.IPD_ITLIST_ICU ";
            SQL = SQL + ComNum.VBLF + "WHERE  C_PATNUM = '" + p.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "    AND  C_INDATE >= TO_DATE('" + ComFunc.FormatStrToDate(p.medFrDate, "D") + "','YYYY-MM-DD')";
            SQL = SQL + ComNum.VBLF + "    AND  C_INDATE <= TO_DATE('" + strCurDate + "','YYYY-MM-DD')";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }

            strIcuDate = dt.Rows[0]["C_INDATE"].ToString().Trim();
            dt.Dispose();
            dt = null;

            if (strIcuDate == "") return;

            lblICUD.Text = (VB.DateDiff("d", Convert.ToDateTime(strIcuDate), Convert.ToDateTime(strCurDate)) + 1).ToString();
        }

        private void GetDataDiag(string strInOutCls, string strMedFrDate, string strMedEndDate, string strDxDrDept)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strDxTable = "";

            ssDise_Sheet1.RowCount = 0;

            strDxTable = Convert.ToString(VB.IIf(strInOutCls == "O", "OPD_DX", "IPD_DX"));

            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + " MAX(ORDER_DATE) AS ORDER_DATE, ";
            SQL = SQL + ComNum.VBLF + " ECODE, ENAME,  MAIN_CODE, RO_CODE  ";  //TOOTH_POSITION, ECODE_KCD6,
            SQL = SQL + ComNum.VBLF + " FROM " + strDxTable + "";
            SQL = SQL + ComNum.VBLF + " WHERE PATIENT_NO = '" + p.ptNo + "'";

            if (strDxTable == "IPD_DX")
            {
                SQL = SQL + ComNum.VBLF + " AND ORDER_DATE >= " + ComFunc.ConvOraToDate(ComFunc.FormatStrToDate(strMedFrDate, "D"), "D");
                if (strMedEndDate != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND ORDER_DATE <= " + ComFunc.ConvOraToDate(ComFunc.FormatStrToDate(strMedEndDate, "D"), "D");
                }
            }
            else
            {
                SQL = SQL + ComNum.VBLF + " AND ORDER_DATE = " + ComFunc.ConvOraToDate(ComFunc.FormatStrToDate(strMedFrDate, "D"), "D");
            }

            switch (strDxDrDept)
            {
                case "RF":
                case "PU":
                case "HO":
                case "KH":
                case "GI":
                case "ED":
                case "CV":
                case "CO":
                    SQL = SQL + ComNum.VBLF + " AND CLINICAL_DEPT IN ('IM','" + strDxDrDept + "')";
                    break;
                default:
                    SQL = SQL + ComNum.VBLF + " AND CLINICAL_DEPT = '" + strDxDrDept + "'";
                    break;
            }
            SQL = SQL + ComNum.VBLF + " And ECODE_KCD6 Is Not Null ";
            SQL = SQL + ComNum.VBLF + " GROUP BY ECODE, ENAME,  MAIN_CODE, RO_CODE  ";
            SQL = SQL + ComNum.VBLF + " ORDER BY  MAIN_CODE Desc  ";  //CLINICAL_DEPT, 
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }
            ssDise_Sheet1.RowCount = dt.Rows.Count;
            ssDise_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssDise_Sheet1.Cells[i, 1].Text = ComFunc.FormatStrToDateTime(dt.Rows[i]["ORDER_DATE"].ToString().Trim(), "D");
                ssDise_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ECODE"].ToString().Trim();
                ssDise_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ENAME"].ToString().Trim();
                ssDise_Sheet1.Cells[i, 4].Text = "Y";
                ssDise_Sheet1.Cells[i, 6].Text = dt.Rows[i]["MAIN_CODE"].ToString().Trim();
                ssDise_Sheet1.Cells[i, 7].Text = dt.Rows[i]["RO_CODE"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;
        }

        private void SetDutyTime()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "SELECT B.BASCD, B.BASNAME, B.BASEXNAME, B.DISSEQNO ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "WHERE B.BSNSCLS = 'ICU관리'";
            SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = 'DUTYTIME'";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["BASCD"].ToString().Trim() == "DAY")
                    {
                        mstrDayTime = dt.Rows[i]["BASNAME"].ToString().Trim();
                    }
                    else if (dt.Rows[i]["BASCD"].ToString().Trim() == "EVE")
                    {
                        mstrEveTime = dt.Rows[i]["BASNAME"].ToString().Trim();
                    }
                    else if (dt.Rows[i]["BASCD"].ToString().Trim() == "NIGHT")
                    {
                        mstrNightTime = dt.Rows[i]["BASNAME"].ToString().Trim();
                    }
                }
            }
            dt.Dispose();
            dt = null;
        }

        private void dtpFrDate_ValueChanged(object sender, EventArgs e)
        {
            pClearFormExcept();
            pLoadEmrChartInfo();
            dtpOrderDate.Value = dtpFrDate.Value;
            GetOrderData();
        }

        private void GetOrderData()
        {
            ssView_Sheet1.RowCount = 0;

            return;

            //TODO ___


            if (p == null) return;
            if (VB.Val(p.acpNo) == 0) return;

            
            string strODate = "";
            string strQueryDept = "";

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            strODate = dtpOrderDate.Value.ToString("yyyy-MM-dd");
            strQueryDept = p.medDeptCd;
            //ORDER_KEY : PK
            SQL = " SELECT DISTINCT  ";
            SQL = SQL + ComNum.VBLF + "    A.ORDER_KEY,  A.INPUT_STATUS, ";
            SQL = SQL + ComNum.VBLF + "    TO_CHAR(A.ORDER_DATE,'YYYY-MM-DD') AS ORDER_DATE,  A.CLINICAL_DEPT, A.ITEM_CODE, A.ITEM_NAME, A.QTY_OF_ORDER, A.DOSAGE_OF_ORDER, A.DOSAGE_CODE, A.FREQUENCY_OF_ORDER, A.VALUE_OF_FREQUENCY,  A.DURATION_OF_ORDER, A.SEQ_NO ";
            SQL = SQL + ComNum.VBLF + "    , B.CLASS_OF_ORDER, B.S_CODE, B.DELETE_FLAG, B.SHAPE, B.ATC,  B.CHK_OUT  ";
            SQL = SQL + ComNum.VBLF + "    , C.S_DRUGCODE  , C.S_SUGBB , C.S_SUGBBO , C.S_SUGBS ,C.S_SUGBT , C.S_DELDATE, C.S_SUDATE ,C.I_DRUG100  DRUG100,  C.OCS_REMARK    ";
            SQL = SQL + ComNum.VBLF + "    , D.CHKFLAG  ";
            SQL = SQL + ComNum.VBLF + "    , U.DR_NAME  ";
            SQL = SQL + ComNum.VBLF + "    , (SELECT MAX(BC.BASCD) FROM " + ComNum.DB_EMR + "AEMRBASCD BC ";
            SQL = SQL + ComNum.VBLF + "                WHERE BC.REMARK2 = TRIM(A.ITEM_CODE)";
            SQL = SQL + ComNum.VBLF + "                    AND BC.BSNSCLS = 'ICU관리'";
            SQL = SQL + ComNum.VBLF + "                    AND BC.UNITCLS = '섭취배설') AS BASCD";
            SQL = SQL + ComNum.VBLF + " FROM IPD_ORDER_DMC A ";
            SQL = SQL + ComNum.VBLF + " LEFT OUTER JOIN OPDIPD_MEDICINES_DMC B ";
            SQL = SQL + ComNum.VBLF + "    ON A.ITEM_CODE = B.ITEM_CODE ";
            SQL = SQL + ComNum.VBLF + "    AND B.CLASS_OF_ORDER NOT IN ('7','88','99')";
            SQL = SQL + ComNum.VBLF + " LEFT OUTER JOIN    MED_PMPA.BAS_SUMAST_DMC C ";
            SQL = SQL + ComNum.VBLF + "    ON B.S_CODE    = C.S_CODE ";
            SQL = SQL + ComNum.VBLF + " LEFT OUTER JOIN OPDIPD_PASSWORD_DMC U ";
            SQL = SQL + ComNum.VBLF + "    ON A.DR_CODE = U.DR_CODE ";
            SQL = SQL + ComNum.VBLF + " LEFT OUTER JOIN   OPDIPD_OCS_SLIP_DMC D  ";
            SQL = SQL + ComNum.VBLF + "    ON A.ITEM_CODE = D.ITEM_CODE ";
            SQL = SQL + ComNum.VBLF + " WHERE A.PATIENT_NO = '" + p.ptNo + "'   ";
           
            SQL = SQL + ComNum.VBLF + "   AND A.ORDER_DATE >= TO_DATE('" + ComFunc.FormatStrToDate(p.medFrDate, "D") + "', 'yyyy-MM-dd')  ";
            SQL = SQL + ComNum.VBLF + "   AND A.ORDER_DATE <= TO_DATE('" + strODate + "', 'yyyy-MM-dd')  ";
            SQL = SQL + ComNum.VBLF + "   AND ( (A.DOSAGE_OF_ORDER = '" + strODate + "') OR (A.ORDER_DATE + A.DURATION_OF_ORDER) > TO_DATE('" + strODate + "', 'yyyy-MM-dd')  ";
            SQL = SQL + ComNum.VBLF + "       ) ";
            SQL = SQL + ComNum.VBLF + "   AND A.GROUP_OF_ORDER IN ('3', '5')";
            
            ////////// 영록이 붙침 - 2016-03-02
            SQL = SQL + ComNum.VBLF + " UNION      ";
            SQL = SQL + ComNum.VBLF + " SELECT DISTINCT  ";
            SQL = SQL + ComNum.VBLF + "    A.ORDER_KEY,  A.INPUT_STATUS, ";
            SQL = SQL + ComNum.VBLF + "    TO_CHAR(A.ORDER_DATE,'YYYY-MM-DD') AS ORDER_DATE,  A.CLINICAL_DEPT, A.ITEM_CODE, A.ITEM_NAME, A.QTY_OF_ORDER, A.DOSAGE_OF_ORDER, A.DOSAGE_CODE, A.FREQUENCY_OF_ORDER, A.VALUE_OF_FREQUENCY,  A.DURATION_OF_ORDER, A.SEQ_NO ";
            SQL = SQL + ComNum.VBLF + "    , B.CLASS_OF_ORDER, B.S_CODE, B.DELETE_FLAG, B.SHAPE, B.ATC,  B.CHK_OUT  ";
            SQL = SQL + ComNum.VBLF + "    , C.S_DRUGCODE  , C.S_SUGBB , C.S_SUGBBO , C.S_SUGBS ,C.S_SUGBT , C.S_DELDATE, C.S_SUDATE ,C.I_DRUG100  DRUG100,  C.OCS_REMARK    ";
            SQL = SQL + ComNum.VBLF + "    , D.CHKFLAG  ";
            SQL = SQL + ComNum.VBLF + "    , U.DR_NAME  ";
            SQL = SQL + ComNum.VBLF + "    , (SELECT MAX(BC.BASCD) FROM " + ComNum.DB_EMR + "AEMRBASCD BC ";
            SQL = SQL + ComNum.VBLF + "                WHERE BC.REMARK2 = TRIM(A.ITEM_CODE)";
            SQL = SQL + ComNum.VBLF + "                    AND BC.BSNSCLS = 'ICU관리'";
            SQL = SQL + ComNum.VBLF + "                    AND BC.UNITCLS = '섭취배설') AS BASCD";
            SQL = SQL + ComNum.VBLF + " FROM OPD_ORDER_DMC A ";
            SQL = SQL + ComNum.VBLF + " LEFT OUTER JOIN OPDIPD_MEDICINES_DMC B ";
            SQL = SQL + ComNum.VBLF + "    ON A.ITEM_CODE = B.ITEM_CODE ";
            SQL = SQL + ComNum.VBLF + "    AND B.CLASS_OF_ORDER NOT IN ('7','88','99')";
            SQL = SQL + ComNum.VBLF + " LEFT OUTER JOIN    MED_PMPA.BAS_SUMAST_DMC C ";
            SQL = SQL + ComNum.VBLF + "    ON B.S_CODE    = C.S_CODE ";
            SQL = SQL + ComNum.VBLF + " LEFT OUTER JOIN OPDIPD_PASSWORD_DMC U ";
            SQL = SQL + ComNum.VBLF + "    ON A.DR_CODE = U.DR_CODE ";
            SQL = SQL + ComNum.VBLF + " LEFT OUTER JOIN   OPDIPD_OCS_SLIP_DMC D  ";
            SQL = SQL + ComNum.VBLF + "    ON A.ITEM_CODE = D.ITEM_CODE ";
            SQL = SQL + ComNum.VBLF + " WHERE A.PATIENT_NO = '" + p.ptNo + "'   ";
            SQL = SQL + ComNum.VBLF + "   AND A.ORDER_DATE = TO_DATE('" + strODate + "', 'yyyy-MM-dd')  ";
            SQL = SQL + ComNum.VBLF + "   AND ( (A.DOSAGE_OF_ORDER = '" + strODate + "') OR (A.ORDER_DATE + A.DURATION_OF_ORDER) > TO_DATE('" + strODate + "', 'yyyy-MM-dd')  ";
            SQL = SQL + ComNum.VBLF + "       ) ";
            SQL = SQL + ComNum.VBLF + "   AND A.GROUP_OF_ORDER IN ('3', '5')";
            SQL = SQL + ComNum.VBLF + "   AND A.CLINICAL_DEPT = 'EM'";
            ///////////////////////////////////////////////////
            SQL = SQL + ComNum.VBLF + "ORDER BY ORDER_DATE, DOSAGE_CODE, CLASS_OF_ORDER, DOSAGE_OF_ORDER, SEQ_NO ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }
            ssView_Sheet1.RowCount = dt.Rows.Count;
            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            //MovePreorderedItemsToSpread
            string strMix = "";
            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ORDER_DATE"].ToString().Trim();
                ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ITEM_NAME"].ToString().Trim();

                if ((dt.Rows[i]["INPUT_STATUS"].ToString().Trim() == "2") || (dt.Rows[i]["INPUT_STATUS"].ToString().Trim() == "3")) // DC
                {
                    ssView_Sheet1.Cells[i, 2].Text = "[D/C] " + ssView_Sheet1.Cells[i, 2].Text.Trim();
                }
                else
                {
                    if (dt.Rows[i]["INPUT_STATUS"].ToString().Trim() == "1") // ADD
                    {
                        ssView_Sheet1.Cells[i, 2].Text = "추] " + ssView_Sheet1.Cells[i, 2].Text.Trim();
                    }
                }

                if (VB.Val(dt.Rows[i]["QTY_OF_ORDER"].ToString().Trim()) < 0)
                {
                    ssView_Sheet1.Cells[i, 3].Text = "0";
                }
                else
                {
                    ssView_Sheet1.Cells[i, 3].Text = VB.Val(dt.Rows[i]["QTY_OF_ORDER"].ToString().Trim()).ToString();
                }
                ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DOSAGE_OF_ORDER"].ToString().Trim();
                ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["FREQUENCY_OF_ORDER"].ToString().Trim();

                ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["VALUE_OF_FREQUENCY"].ToString().Trim();
                ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["DURATION_OF_ORDER"].ToString().Trim();
                ssView_Sheet1.Cells[i, 8].Text = "";
                ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["CLINICAL_DEPT"].ToString().Trim();
                ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["DR_NAME"].ToString().Trim();
                ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["ORDER_KEY"].ToString().Trim();
                ssView_Sheet1.Cells[i, 12].Text = dt.Rows[i]["CLASS_OF_ORDER"].ToString().Trim();
                ssView_Sheet1.Cells[i, 13].Text = dt.Rows[i]["BASCD"].ToString().Trim();
                ssView_Sheet1.Cells[i, 14].Text = dt.Rows[i]["ITEM_CODE"].ToString().Trim();
                ssView_Sheet1.Cells[i, 15].Text = dt.Rows[i]["ITEM_NAME"].ToString().Trim();

                if (dt.Rows[i]["BASCD"].ToString().Trim() != "")
                {
                    ssView_Sheet1.Cells[i, 0, i, ssView_Sheet1.ColumnCount - 1].BackColor = Color.Yellow;
                }
                if ((dt.Rows[i]["CLASS_OF_ORDER"].ToString().Trim() == "11") || (dt.Rows[i]["CLASS_OF_ORDER"].ToString().Trim() == "21") || (dt.Rows[i]["CLASS_OF_ORDER"].ToString().Trim() == "22") || (dt.Rows[i]["CLASS_OF_ORDER"].ToString().Trim() == "23"))
                {
                    ssView_Sheet1.Cells[i, 2].ForeColor = Color.Blue;
                    
                    if ((dt.Rows[i]["CLASS_OF_ORDER"].ToString().Trim() == "21") || (dt.Rows[i]["CLASS_OF_ORDER"].ToString().Trim() == "22") || (dt.Rows[i]["CLASS_OF_ORDER"].ToString().Trim() == "23"))
                    {
                        if (strMix != "")
                        {
                            if (strMix == dt.Rows[i]["DOSAGE_CODE"].ToString().Trim())
                            {
                                ssView_Sheet1.Cells[i, 2].Text = "┃ " + ssView_Sheet1.Cells[i, 2].Text.Trim();
                            }
                            else
                            {
                                ssView_Sheet1.Cells[i - 1, 2].Text = ssView_Sheet1.Cells[i - 1, 2].Text.Trim().Replace("┃", "┗");
                                strMix = "";
                                if ((VB.Left(dt.Rows[i]["DOSAGE_CODE"].ToString(), 5) == "FLIM-") || (VB.Left(dt.Rows[i]["DOSAGE_CODE"].ToString(), 4) == "IMX-"))
                                {
                                    strMix = dt.Rows[i]["DOSAGE_CODE"].ToString().Trim();
                                    ssView_Sheet1.Cells[i, 2].Text = "┏" + ssView_Sheet1.Cells[i, 2].Text.Trim();
                                }
                            }
                        }
                        else
                        {
                            if ((VB.Left(dt.Rows[i]["DOSAGE_CODE"].ToString(), 5) == "FLIM-") || (VB.Left(dt.Rows[i]["DOSAGE_CODE"].ToString(), 4) == "IMX-"))
                            {
                                strMix = dt.Rows[i]["DOSAGE_CODE"].ToString().Trim();
                                ssView_Sheet1.Cells[i, 2].Text = "┏" + ssView_Sheet1.Cells[i, 2].Text.Trim();
                            }
                        }
                    }
                    else
                    {
                        if (strMix != "") ssView_Sheet1.Cells[i - 1, 2].Text = ssView_Sheet1.Cells[i - 1, 2].Text.Trim().Replace("┃", "┗");
                        strMix = "";
                    }
                }
                else
                {
                    if (strMix != "") ssView_Sheet1.Cells[i - 1, 2].Text = ssView_Sheet1.Cells[i - 1, 2].Text.Trim().Replace("┃", "┗");
                    strMix = "";
                }

                if ((dt.Rows[i]["INPUT_STATUS"].ToString().Trim() == "2") || (dt.Rows[i]["INPUT_STATUS"].ToString().Trim() == "3")) // DC
                {
                    ssView_Sheet1.Cells[i, 2].ForeColor = Color.Red;
                }
            }
            if (strMix != "")
            {
                ssView_Sheet1.Cells[i - 1, 2].Text = "┗ " + ssView_Sheet1.Cells[i - 1, 2].Text.Trim();
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        private void mbtnBefore_Click(object sender, EventArgs e)
        {
            dtpFrDate.Value = dtpFrDate.Value.AddDays(-1);
        }

        private void mbtnNext_Click(object sender, EventArgs e)
        {
            dtpFrDate.Value = dtpFrDate.Value.AddDays(+1);
        }

        private void mbtnUpdate_Click(object sender, EventArgs e)
        {

            SetFormNameGb();

            mstrFormNo = "646";
            mstrUpdateNo = "0";
            frmVitalSet frmVitalSetX = new frmVitalSet(p, mstrFormNameGb, mstrFormNo, mstrUpdateNo);
            frmVitalSetX.ShowDialog();

            LoadJobData();

            ColVisible();
        }

        private void ssVital_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;

            if (ssVital_Sheet1.Columns.Count <= 3) return;

            if (e.Column < 3) return;

            for (i = 3; i < ssVital_Sheet1.Columns.Count; i++)
            {
                ssVital_Sheet1.Cells[0, i, ssVital_Sheet1.Rows.Count - 1, i].Font = new Font("굴림", 9, FontStyle.Regular);
            }
            ssVital_Sheet1.Cells[0, e.Column, ssVital_Sheet1.Rows.Count - 1, e.Column].Font = new Font("굴림", 9, FontStyle.Bold);
            if (e.Button == MouseButtons.Right)
            {
                ViewItemValue(e.Row, e.Column);
                return;
            }
        }

        private void ssNuro_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;

            if (ssNuro_Sheet1.Columns.Count <= 3) return;

            if (e.Column < 3) return;

            for (i = 3; i < ssNuro_Sheet1.Columns.Count; i++)
            {
                ssNuro_Sheet1.Cells[0, i, ssNuro_Sheet1.Rows.Count - 1, i].Font = new Font("굴림", 9, FontStyle.Regular);
            }
            ssNuro_Sheet1.Cells[0, e.Column, ssNuro_Sheet1.Rows.Count - 1, e.Column].Font = new Font("굴림", 9, FontStyle.Bold);
            if (e.Button == MouseButtons.Right)
            {
                ViewItemValue(e.Row, e.Column);
                return;
            }
        }

        private void ssRR_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;

            if (ssRR_Sheet1.Columns.Count <= 3) return;

            if (e.Column < 3) return;

            for (i = 3; i < ssRR_Sheet1.Columns.Count; i++)
            {
                ssRR_Sheet1.Cells[0, i, ssRR_Sheet1.Rows.Count - 1, i].Font = new Font("굴림", 9, FontStyle.Regular);
            }
            ssRR_Sheet1.Cells[0, e.Column, ssRR_Sheet1.Rows.Count - 1, e.Column].Font = new Font("굴림", 9, FontStyle.Bold);
            if (e.Button == MouseButtons.Right)
            {
                ViewItemValue(e.Row, e.Column);
                return;
            }
        }

        private void ssBst_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;

            if (ssBst_Sheet1.Columns.Count <= 3) return;

            if (e.Column < 3) return;

            for (i = 3; i < ssBst_Sheet1.Columns.Count; i++)
            {
                ssBst_Sheet1.Cells[0, i, ssBst_Sheet1.Rows.Count - 1, i].Font = new Font("굴림", 9, FontStyle.Regular);
            }
            ssBst_Sheet1.Cells[0, e.Column, ssBst_Sheet1.Rows.Count - 1, e.Column].Font = new Font("굴림", 9, FontStyle.Bold);

            if (e.Button == MouseButtons.Right)
            {
                ViewItemValue(e.Row, e.Column);
                return;
            }
        }

        private void ssAct_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;

            if (ssAct_Sheet1.Columns.Count <= 3) return;

            if (e.Column < 3) return;

            for (i = 3; i < ssAct_Sheet1.Columns.Count; i++)
            {
                ssAct_Sheet1.Cells[0, i, ssAct_Sheet1.Rows.Count - 1, i].Font = new Font("굴림", 9, FontStyle.Regular);
            }
            ssAct_Sheet1.Cells[0, e.Column, ssAct_Sheet1.Rows.Count - 1, e.Column].Font = new Font("굴림", 9, FontStyle.Bold);

            if (e.Button == MouseButtons.Right)
            {
                ViewItemValue(e.Row, e.Column);
                return;
            }
        }

        private void ssIO_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;

            if (ssIO_Sheet1.Columns.Count <= 3) return;

            if (e.Column == 2)
            {
                string strITEMCD = ssIO_Sheet1.Cells[e.Row, 0].Text.Trim();
                ViewToolTip(strITEMCD, e.Row, e.Column);
                GetMixInfo(strITEMCD);
                return;
            }

            if (e.Column < 3)
            {
                return;
            }

            for (i = 3; i < ssIO_Sheet1.Columns.Count; i++)
            {
                ssIO_Sheet1.Cells[0, i, ssIO_Sheet1.Rows.Count - 1, i].Font = new Font("굴림", 9, FontStyle.Regular);
            }
            ssIO_Sheet1.Cells[0, e.Column, ssIO_Sheet1.Rows.Count - 1, e.Column].Font = new Font("굴림", 9, FontStyle.Bold);
            if (e.Button == MouseButtons.Right)
            {
                ViewItemValue(e.Row, e.Column);
                return;
            }
        }

        private void ViewToolTip(string strITEMCD, int Row, int Col)
        {
            string strToolTip = "";

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssIO_Sheet1.Cells[Row, 2].Note = null;
            
            SQL = "";
            SQL = "SELECT A.ITEMCD, A.ORDERCD, A.ORDERNM, A.ORDER_KEY, A.ITEMNAME, B.BASNAME FROM " + ComNum.DB_EMR + "AEMRIOMIX A";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "    ON B.BASCD = A.ITEMCD ";
            SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = 'ICU관리' ";
            SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '섭취배설' ";
            SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";
            SQL = SQL + ComNum.VBLF + "    AND A.ITEMCD = '" + strITEMCD + "'";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    strToolTip = strToolTip + dt.Rows[i]["ITEMNAME"].ToString().Trim() + ComNum.VBLF;
                    strToolTip = strToolTip + " ┗" + dt.Rows[i]["ORDERNM"].ToString().Trim() + ComNum.VBLF;
                }
                else
                {
                    if (i == dt.Rows.Count - 1)
                    {
                        strToolTip = strToolTip + " ┗" + dt.Rows[i]["ORDERNM"].ToString().Trim();
                    }
                    else
                    {
                        strToolTip = strToolTip + " ┗" + dt.Rows[i]["ORDERNM"].ToString().Trim() + ComNum.VBLF;
                    }
                }
            }
            dt.Dispose();
            dt = null;

            ssIO_Sheet1.Cells[Row, 2].Note = strToolTip;
            ssIO_Sheet1.Cells[Row, 2].NoteIndicatorSize = new Size(9, 9);
            ssIO_Sheet1.Cells[Row, 2].NoteIndicatorPosition = FarPoint.Win.Spread.NoteIndicatorPosition.TopRight;
            ssIO_Sheet1.Cells[Row, 2].NoteStyle = FarPoint.Win.Spread.NoteStyle.PopupNote;
        }

        private void ssVital_EditChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column < 3) return;
            if (e.Row < 4) return;
            if (e.Row > ssVital_Sheet1.RowCount - 4) return;

            ssVital_Sheet1.Cells[ssVital_Sheet1.RowCount - 1, e.Column].Text = "Y";
        }

        private void ssNuro_EditChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column < 3) return;
            if (e.Row < 4) return;
            if (e.Row > ssNuro_Sheet1.RowCount - 4) return;

            ssNuro_Sheet1.Cells[ssNuro_Sheet1.RowCount - 1, e.Column].Text = "Y";
        }

        private void ssRR_EditChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column < 3) return;
            if (e.Row < 4) return;
            if (e.Row > ssRR_Sheet1.RowCount - 4) return;

            ssRR_Sheet1.Cells[ssRR_Sheet1.RowCount - 1, e.Column].Text = "Y";
        }

        private void ssBst_EditChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column < 3) return;
            if (e.Row < 4) return;
            if (e.Row > ssBst_Sheet1.RowCount - 4) return;

            ssBst_Sheet1.Cells[ssBst_Sheet1.RowCount - 1, e.Column].Text = "Y";
        }

        private void ssAct_EditChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column < 3) return;
            if (e.Row < 4) return;
            if (e.Row > ssAct_Sheet1.RowCount - 4) return;

            ssAct_Sheet1.Cells[ssAct_Sheet1.RowCount - 1, e.Column].Text = "Y";
        }

        private void ssIO_EditChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column < 3) return;
            if (e.Row < 4) return;
            if (e.Row > ssIO_Sheet1.RowCount - 4) return;

            ssIO_Sheet1.Cells[ssIO_Sheet1.RowCount - 1, e.Column].Text = "Y";

            //합계를 구한다.
            SumIntakeOutPutRow(e.Column);
        }

        private void ViewItemValue(int Row, int Col)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            PopupMenu = null;

            PopupMenu = new ContextMenu();
            mSpd.ContextMenu = null;
            mPopRow = -1;
            mPopCol = -1;

            mPopRow = Row;
            mPopCol = Col;

            if (Row < 4)
            {
                if (mSpd.Name == "")
                {

                }
                string strCHARTUSEID = ssIO_Sheet1.Cells[3, mPopCol].Text.Trim();
                if (strCHARTUSEID == "합계")
                {
                    PopupMenu = new ContextMenu();
                    PopupMenu.MenuItems.Add("합계 삭제", new System.EventHandler(SubMenuIo_Click));
                    mSpd.ContextMenu = PopupMenu; // 입력
                }
                else
                {
                    PopupMenu = new ContextMenu();
                    PopupMenu.MenuItems.Add("합계 추가", new System.EventHandler(SubMenuIo_Click));
                    mSpd.ContextMenu = PopupMenu; // 입력
                }
            }

            mSpdView.SetActiveCell(Row, Col);

            string strITEMCD = mSpdView.Cells[Row, 0].Text.Trim();

            Cursor.Current = Cursors.WaitCursor;
            SQL = " SELECT ITEMVALUE  ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRITEMVALUE ";
            SQL = SQL + ComNum.VBLF + "WHERE FORMNO = " + VB.Val(mstrFormNo);
            SQL = SQL + ComNum.VBLF + "    AND ITEMCD = '" + strITEMCD + "' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY DSPSEQ ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            PopupMenu.Name = "ICU기록";
            for (i = 0; i < dt.Rows.Count; i++)
            {
                PopupMenu.MenuItems.Add(dt.Rows[i]["ITEMVALUE"].ToString().Trim(), new System.EventHandler(mnuItemValue_Click));
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
            mSpd.ContextMenu = PopupMenu;

        }

        private void mnuItemValue_Click(object sender, EventArgs e) // 이벤트 헨들러
        {
            string strPopMenuName = ((MenuItem)sender).Text.Trim();

            mSpd.ContextMenu = null;

            if (mPopRow == -1) return;
            if (mSpdView == null) return;

            if (VB.InStr(strPopMenuName, "]") > 0)
            {
                if (mSpdView.Cells[mPopRow, mPopCol].Text.Trim() != "")
                {
                    mSpdView.Cells[mPopRow, mPopCol].Text = mSpdView.Cells[mPopRow, mPopCol].Text.Trim() + "," + (VB.Split(strPopMenuName, "]")[0]).Trim();
                }
                else
                {
                    mSpdView.Cells[mPopRow, mPopCol].Text = (VB.Split(strPopMenuName, "]")[0]).Trim();
                }
            }
            else
            {
                mSpdView.Cells[mPopRow, mPopCol].Text = strPopMenuName;
            }

            if (strPopMenuName != "")
            {
                mSpdView.Cells[mSpdView.RowCount - 1, mPopCol].Text = "Y";
            }
        }

        private void SubMenuIo_Click(object sender, EventArgs e) // 이벤트 헨들러
        {
            string strJob = ((MenuItem)sender).Text.Trim();
            string strTime = mSpdView.Cells[2, mPopCol].Text.Trim();
            mSpd.ContextMenu = null;
            if (strJob == "합계 추가")
            {
                TotAdd(strTime);
            }
            else if (strJob == "합계 삭제")
            {
                TotDel(strTime);
            }
        }

        private void TotDel(string strTime)
        {
            int intSelCol = mPopCol;

            //EMRNO가 있는지 확인
            string strEmrNo = Convert.ToString(VB.Val(ssIO_Sheet1.Cells[ssIO_Sheet1.Rows.Count - 4, intSelCol].Text.Trim()));

            if (ssIO_Sheet1.Cells[3, intSelCol].Text.Trim() == "합계")
            {
                DeleteAll(mSpdView, "합계");
            }
            mstrEmrNo = "0";
        }

        private void TotAdd(string strTime)
        {
            int i = 0;
            int j = 0;
            int intSelCol = 0;
            int intTime = 0;
            intTime = Convert.ToInt32(VB.Val(strTime.Replace(":", "")));

            if (intTime <= 0) return;

            if (mPopCol == ssIO_Sheet1.ColumnCount - 1)
            {
                ssIO_Sheet1.ColumnCount = ssIO_Sheet1.ColumnCount + 1;
            }
            else
            {
                ssIO_Sheet1.AddColumns(mPopCol + 1, 1);
            }
            intSelCol = mPopCol + 1;

            ssIO_Sheet1.Columns[intSelCol].Width = mintColW_V;

            clsSpread.SetTypeAndValue(ssIO_Sheet1, 0, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            ssIO_Sheet1.Cells[0, intSelCol].BackColor = Color.LightBlue;
            clsSpread.SetTypeAndValue(ssIO_Sheet1, 1, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            ssIO_Sheet1.Cells[1, intSelCol].BackColor = Color.LightBlue;
            clsSpread.SetTypeAndValue(ssIO_Sheet1, 2, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            ssIO_Sheet1.Cells[2, intSelCol].BackColor = Color.LightBlue;
            ssIO_Sheet1.Cells[2, intSelCol].Text = strTime;
            clsSpread.SetTypeAndValue(ssIO_Sheet1, 3, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            ssIO_Sheet1.Cells[3, intSelCol].BackColor = Color.LightBlue;
            for (j = 4; j < ssIO_Sheet1.RowCount - 4; j++)
            {
                clsSpread.SetTypeAndValue(ssIO_Sheet1, j, intSelCol, "TypeText", false, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);
                if ((ssIO_Sheet1.Cells[j, 0].Text.Trim() == "I0000030622") || (ssIO_Sheet1.Cells[j, 0].Text.Trim() == "I0000030623"))
                {
                    ssIO_Sheet1.Cells[j, 0, j, ssIO_Sheet1.ColumnCount - 1].BackColor = Color.LightYellow;
                }
            }
            clsSpread.SetTypeAndValue(ssIO_Sheet1, ssIO_Sheet1.RowCount - 4, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(ssIO_Sheet1, ssIO_Sheet1.RowCount - 3, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(ssIO_Sheet1, ssIO_Sheet1.RowCount - 2, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(ssIO_Sheet1, ssIO_Sheet1.RowCount - 1, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);

            ssIO_Sheet1.Cells[3, intSelCol].Text = "합계";
            ssIO_Sheet1.Cells[ssIO_Sheet1.Rows.Count - 3, intSelCol].Text = "합계";

            if (SaveTimeSet(ssIO_Sheet1, mJOBGB) == false)
            {
                ComFunc.MsgBoxEx(this, "시간 저장중 에러가 발생했습니다.");
            }

            pSaveDataTot(intSelCol);
            SetDefaultData(ssIO_Sheet1, "IIO");
            LoadData(ssIO_Sheet1, "IIO");
        }

        public double pSaveDataTot(int intSelCol)
        {
            string strFlag = "0";

            double dblEmrNo = 0;

            int i = intSelCol;
            int j = 0;


            bool bolIsItem = false;

            string[] arryEMRNO = null;
            string[] arryITEMCD = null;
            string[] arryITEMNO = null;
            string[] arryITEMINDEX = null;
            string[] arryITEMTYPE = null;
            string[] arryITEMVALUE = null;
            string[] arryITEMVALUE1 = null;
            string[] arryDSPSEQ = null;

            string strEmrNo = Convert.ToString(VB.Val(ssIO_Sheet1.Cells[ssIO_Sheet1.Rows.Count - 4, i].Text.Trim()));
            string strChartDate = dtpFrDate.Value.ToString("yyyyMMdd");// ssIO_Sheet1.Cells[0, i].Text.Trim().Replace("-", "");
            string strChartTime = ssIO_Sheet1.Cells[2, i].Text.Trim().Replace(":", "") + "00";
            string strCHARTUSEID = "합계";
            string strCOMPUSEID = "합계";
            string strSAVEGB = "0";
            string strFORMGB = "0";
            
            SumIntakeOutPutRowTot(intSelCol);

            for (j = 4; j < ssIO_Sheet1.RowCount - 4; j++)
            {
                string strITEMCD = ssIO_Sheet1.Cells[j, 0].Text.Trim();
                string[] strItem = VB.Split(ssIO_Sheet1.Cells[j, 0].Text.Trim(), "_");
                string strITEMNO = "";
                string strITEMINDEX = "-1";
                if (strItem.Length > 0)
                {
                    strITEMNO = strItem[0].Trim();
                }
                if (strItem.Length > 1)
                {
                    strITEMINDEX = strItem[1].Trim();
                }
                string strDSPSEQ = "0";
                string strITEMTYPE = "TEXT";
                string strITEMVALUE = ssIO_Sheet1.Cells[j, i].Text.Trim();
                string strITEMVALUE1 = "";

                if (arryEMRNO == null)
                {
                    arryEMRNO = new string[0];
                    arryITEMCD = new string[0];
                    arryITEMNO = new string[0];
                    arryITEMINDEX = new string[0];
                    arryITEMTYPE = new string[0];
                    arryITEMVALUE = new string[0];
                    arryITEMVALUE1 = new string[0];
                    arryDSPSEQ = new string[0];
                }

                Array.Resize<string>(ref arryEMRNO, arryEMRNO.Length + 1);
                Array.Resize<string>(ref arryITEMCD, arryITEMCD.Length + 1);
                Array.Resize<string>(ref arryITEMNO, arryITEMNO.Length + 1);
                Array.Resize<string>(ref arryITEMINDEX, arryITEMINDEX.Length + 1);
                Array.Resize<string>(ref arryITEMTYPE, arryITEMTYPE.Length + 1);
                Array.Resize<string>(ref arryITEMVALUE, arryITEMVALUE.Length + 1);
                Array.Resize<string>(ref arryITEMVALUE1, arryITEMVALUE1.Length + 1);
                Array.Resize<string>(ref arryDSPSEQ, arryDSPSEQ.Length + 1);

                arryEMRNO[arryEMRNO.Length - 1] = "0";
                arryITEMCD[arryEMRNO.Length - 1] = strITEMCD;
                arryITEMNO[arryEMRNO.Length - 1] = strITEMNO;
                arryITEMINDEX[arryEMRNO.Length - 1] = strITEMINDEX;
                arryITEMTYPE[arryEMRNO.Length - 1] = strITEMTYPE;
                arryITEMVALUE[arryEMRNO.Length - 1] = strITEMVALUE;
                arryITEMVALUE1[arryEMRNO.Length - 1] = strITEMVALUE1;
                arryDSPSEQ[arryEMRNO.Length - 1] = strDSPSEQ;
            }

            string strSAVECERT = "1";

            if (clsEmrQuery.SaveFlowChart(clsDB.DbCon, p, mstrFormNo, mstrUpdateNo, strEmrNo, strChartDate, strChartTime, 
                strCHARTUSEID, strCOMPUSEID, strSAVEGB, strSAVECERT, strFORMGB,
                arryITEMCD, arryITEMNO, arryITEMINDEX, arryITEMTYPE, arryITEMVALUE, arryDSPSEQ, arryITEMVALUE1
                ) == 0)
            {
                //에러임
            }

            return dblEmrNo;
        }

        private void SumIntakeOutPutRow(int Col)
        {
            int j = 0;
            int intIntakeRow = -1;
            int intOutPutRow = -1;
            int intIntakeSum = 0;
            int intOutPutSum = 0;

            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            //Intake
            //OutPut
            for (j = 4; j < ssIO_Sheet1.RowCount - 4; j++)
            {
                string strITEMCD = ssIO_Sheet1.Cells[j, 0].Text.Trim();

                if ((strITEMCD == "I0000030622") || (strITEMCD == "I0000030623"))
                {
                    if (strITEMCD == "I0000030622")
                    {
                        intIntakeRow = j;
                    }
                    else if (strITEMCD == "I0000030623")
                    {
                        intOutPutRow = j;
                    }
                }
                else
                {
                    SQL = "";
                    SQL = SQL + "SELECT VFLAG1";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD ";
                    SQL = SQL + ComNum.VBLF + "WHERE BSNSCLS = 'ICU관리'";
                    SQL = SQL + ComNum.VBLF + "    AND UNITCLS = '섭취배설'";
                    SQL = SQL + ComNum.VBLF + "    AND BASCD = '" + strITEMCD + "'";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["VFLAG1"].ToString().Trim() == "01.섭취")
                        {
                            intIntakeSum = intIntakeSum + Convert.ToInt32(VB.Val(ssIO_Sheet1.Cells[j, Col].Text.Trim()));
                        }
                        else if (dt.Rows[0]["VFLAG1"].ToString().Trim() == "11.배설")
                        {
                            intOutPutSum = intOutPutSum + Convert.ToInt32(VB.Val(ssIO_Sheet1.Cells[j, Col].Text.Trim()));
                        }
                    }
                    dt.Dispose();
                    dt = null;
                }
            }

            if (intIntakeRow >= 0)
            {
                ssIO_Sheet1.Cells[intIntakeRow, Col].Text = intIntakeSum.ToString();
            }

            if (intOutPutRow >= 0)
            {
                ssIO_Sheet1.Cells[intOutPutRow, Col].Text = intOutPutSum.ToString();
            }

        }

        private void SumIntakeOutPutRowTot(int TotCol)
        {
            int Col = 0;
            int Row = 0;
            int intSum = 0;

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            for (Row = 4; Row < ssIO_Sheet1.RowCount - 4; Row++)
            {
                string strITEMCD = ssIO_Sheet1.Cells[Row, 0].Text.Trim();
                intSum = 0;

                SQL = "";
                SQL = SQL + "SELECT VFLAG1";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD ";
                SQL = SQL + ComNum.VBLF + "WHERE BSNSCLS = 'ICU관리'";
                SQL = SQL + ComNum.VBLF + "    AND UNITCLS = '섭취배설'";
                SQL = SQL + ComNum.VBLF + "    AND BASCD = '" + strITEMCD + "'";
                SQL = SQL + ComNum.VBLF + "    AND VFLAG1 IN ('01.섭취', '02.섭취', '11.배설', '12.배설', '09.섭취', '19.배설')";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (Col = TotCol - 1; Col >= 3; Col--)
                    {
                        if (ssIO_Sheet1.Cells[3, Col].Text.Trim() == "합계")
                        {
                            break;
                        }
                        else
                        {
                            intSum = intSum + Convert.ToInt32(VB.Val(ssIO_Sheet1.Cells[Row, Col].Text.Trim()));
                        }
                    }
                    ssIO_Sheet1.Cells[Row, TotCol].Text = intSum.ToString();
                }
                dt.Dispose();
                dt = null;
            }

        }

        private void mbtnInsert_Click(object sender, EventArgs e)
        {
            SaveTime(txtTime.Text.Trim());
            ColVisible();
        }

        private void SaveTime(string strTime)
        {
            int i = 0;
            int j = 0;
            int intSelCol = 0;
            int intTime = 0;
            intTime = Convert.ToInt32(VB.Val(strTime.Replace(":", "")));

            SetSpread();

            if (intTime <= 0) return;
            if (intTime > 2400) return;

            if (intTime < Convert.ToInt32(VB.Val(mSpdView.Cells[2, 3].Text.Replace(":", ""))))
            {
                intSelCol = 3;
                mSpdView.AddColumns(intSelCol, 1);
            }
            else
            {
                for (i = 3; i < mSpdView.Columns.Count; i++)
                {
                    if (intTime > Convert.ToInt32(VB.Val(mSpdView.Cells[2, i].Text.Replace(":", ""))))
                    {
                        if (i + 1 == mSpdView.Columns.Count)
                        {
                            break;
                        }
                        else
                        {
                            if (intTime < Convert.ToInt32(VB.Val(mSpdView.Cells[2, i + 1].Text.Replace(":", ""))))
                            {
                                intSelCol = i;
                                break;
                            }
                        }
                    }
                }
                if (intSelCol == 0)
                {
                    mSpdView.Columns.Count = mSpdView.Columns.Count + 1;
                    intSelCol = mSpdView.Columns.Count - 1;
                }
                else
                {
                    intSelCol = intSelCol + 1;
                    mSpdView.AddColumns(intSelCol, 1);
                }
            }

            mSpdView.Columns[intSelCol].Width = mintColW_V;

            clsSpread.SetTypeAndValue(mSpdView, 0, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            mSpdView.Cells[0, intSelCol].BackColor = Color.LightBlue;
            clsSpread.SetTypeAndValue(mSpdView, 1, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            mSpdView.Cells[1, intSelCol].BackColor = Color.LightBlue;
            clsSpread.SetTypeAndValue(mSpdView, 2, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            mSpdView.Cells[2, intSelCol].BackColor = Color.LightBlue;
            mSpdView.Cells[2, intSelCol].Text = strTime.Trim();
            clsSpread.SetTypeAndValue(mSpdView, 3, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            mSpdView.Cells[3, intSelCol].BackColor = Color.LightBlue;
            for (j = 4; j < mSpdView.RowCount - 4; j++)
            {
                clsSpread.SetTypeAndValue(mSpdView, j, intSelCol, "TypeText", false, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);
            }
            clsSpread.SetTypeAndValue(mSpdView, mSpdView.RowCount - 4, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(mSpdView, mSpdView.RowCount - 3, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(mSpdView, mSpdView.RowCount - 2, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(mSpdView, mSpdView.RowCount - 1, intSelCol, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);

            if (SaveTimeSet(mSpdView, mJOBGB) == false)
            {
                ComFunc.MsgBoxEx(this, "시간 저장중 에러가 발생했습니다.");
            }
        }
        private void mbtnSaveAll_Click(object sender, EventArgs e)
        {
            pSaveData("1"); //인증저장
            SetSpread();
            SetDefaultData(mSpdView, mJOBGB);
            LoadData(mSpdView, mJOBGB);
        }

        private void mbtnDeleteAll_Click(object sender, EventArgs e)
        {
            int intSelCol = 0;
            string StrItemcd = "";
            string StrDate = "";
            string StrTime = "";
            string StrEmrno = "0";

            SetSpread();

            if (mSpdView.Columns.Count <= 3) return;

            if (mSpdView.ActiveColumnIndex < 3) return;

            //2017-11-16 이준우 의료관련감염예방 삭제시는 별도루틴으로 한다.
            if (tabChart.SelectedTab != tabInfection)
            {
                intSelCol = mSpdView.ActiveColumnIndex;
                string strTime = mSpdView.Cells[2, intSelCol].Text.Trim();

                if (VB.Right(strTime, 2) == "00")
                {
                    DeleteOne(mSpdView);
                }
                else
                {
                    DeleteAll(mSpdView);
                }
            }
            else
            {
                //2017-11-16
                //의료관련감염예방은 선택한 해당항목(ITEMCD)의 MED_OCS.EMR_INFECT 및 MHEMR.AEMRCHARTROW의 해당항목자료만 삭제 하며, 
                //다른항목(ITEMCD)이 없을경우에만 MHEMR.AEMRCHARTMST 를 삭제한다!!
                if (mSpdView.ActiveRowIndex < 3) return;

                //선택한 항목에 입력된 자료가 있는지 판단.
                //자료가 있으면 해당 항목(ITEMCD)를 모두 삭제한다.
                if (mSpdView.Cells[mSpdView.ActiveRowIndex, mSpdView.ActiveColumnIndex].Text.Trim() != "")  
                {
                    StrItemcd = mSpdView.Cells[mSpdView.ActiveRowIndex, 0].Text.Trim();
                    StrDate = dtpFrDate.Value.ToString("yyyyMMdd");
                    StrTime = mSpdView.Cells[2, mSpdView.ActiveColumnIndex].Text.Trim().Replace(":", "") + "00";

                    StrEmrno = clsEmrQueryEtc.Read_EmrNo(clsDB.DbCon, p.acpNo, StrItemcd, StrDate, StrTime);

                    Delete_Infect(StrEmrno, p.acpNo, p.ptNo, StrItemcd, StrDate, StrTime);

                }

            }
            
            ColVisible();
        }

        private void Delete_Infect(string gEmrno, string gAcpno, string gPtno, string gItemcd, string gDate, string gTime)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            //MHEMR.AEMRCHARTROW 테이블의 해당 EMRNO의 해당항목(ITEMCD)의 ITEMVALUE 값을 NULL 로 UPDATE
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                
                SQL = "UPDATE " + ComNum.DB_EMR + "AEMRCHARTROW SET ";
                SQL = SQL + ComNum.VBLF + " ITEMVALUE = '' ";
                SQL = SQL + ComNum.VBLF + " WHERE EMRNO = '" + gEmrno + "' ";
                SQL = SQL + ComNum.VBLF + " AND ITEMCD = '" + gItemcd + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            //MED_OCS.EMR_INFECT 테이블의 해당 EMRNO 이면서, 해당항목(ITEMCD) 이외의 항목(ITEMCD)이 저장되어져 있지 않으면
            //MHEMR.AEMRCHARTMST 테이블 및 MHEMR.AEMRCHARTROW 테이블의 해당 EMRNO 삭제처리.
            SQL = " ";
            SQL = SQL + ComNum.VBLF + " SELECT * FROM MED_OCS.EMR_INFECT ";
            SQL = SQL + ComNum.VBLF + " WHERE EMRNO = '" + gEmrno + "' ";
            SQL = SQL + ComNum.VBLF + " AND ACPNO = '" + gAcpno + "' ";
            SQL = SQL + ComNum.VBLF + " AND PTNO = '" + gPtno + "' ";
            SQL = SQL + ComNum.VBLF + " AND CHARTDATE = '" + gDate + "' ";
            SQL = SQL + ComNum.VBLF + " AND CHARTTIME = '" + gTime + "' ";
            SQL = SQL + ComNum.VBLF + " AND ITEMCD <> '" + gItemcd + "' ";
            
            Cursor.Current = Cursors.WaitCursor;

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                //해당 MST, ROW 모두 삭제시킴
                clsDB.setBeginTran(clsDB.DbCon);
                try
                {
                    SQL = "DELETE FROM " + ComNum.DB_EMR + "AEMRCHARTMST ";
                    SQL = SQL + ComNum.VBLF + " WHERE EMRNO = '" + gEmrno + "' ";
                    SQL = SQL + ComNum.VBLF + " AND ACPNO = '" + gAcpno + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return ;
                    }

                    SQL = "DELETE FROM " + ComNum.DB_EMR + "AEMRCHARTROW ";
                    SQL = SQL + ComNum.VBLF + " WHERE EMRNO = '" + gEmrno + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return ;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, ex.Message);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }
            else
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }

            //MED_OCS.EMR_INFECT 테이블의 해당 EMRNO, ITEMCD 내역삭제
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                SQL = "";
                SQL = "INSERT INTO MED_OCS.EMR_INFECT_LOG ";
                SQL = SQL + ComNum.VBLF + " (M_DATE, M_SABUN, IP_DATE, IP_SABUN,  ";
                SQL = SQL + ComNum.VBLF + " ACPNO, EMRNO, PTNO, CHARTDATE,CHARTTIME, ";
                SQL = SQL + ComNum.VBLF + " ITEMCD, SEQ, SUB_SEQ, ACT_GUBUN) ";
                SQL = SQL + ComNum.VBLF + " SELECT SYSDATE, '" + clsType.User.IdNumber + "' , ";
                SQL = SQL + ComNum.VBLF + " IP_DATE, IP_SABUN, ";
                SQL = SQL + ComNum.VBLF + " ACPNO, EMRNO, PTNO, CHARTDATE,CHARTTIME, ";
                SQL = SQL + ComNum.VBLF + " ITEMCD, SEQ, SUB_SEQ, ACT_GUBUN ";
                SQL = SQL + ComNum.VBLF + " FROM MED_OCS.EMR_INFECT ";
                SQL = SQL + ComNum.VBLF + " WHERE EMRNO = '" + gEmrno + "' ";
                SQL = SQL + ComNum.VBLF + " AND ACPNO = '" + gAcpno + "' ";
                SQL = SQL + ComNum.VBLF + " AND PTNO = '" + gPtno + "' ";
                SQL = SQL + ComNum.VBLF + " AND CHARTDATE = '" + gDate + "' ";
                SQL = SQL + ComNum.VBLF + " AND CHARTTIME = '" + gTime + "' ";
                SQL = SQL + ComNum.VBLF + " AND ITEMCD = '" + gItemcd + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return ;
                }

                SQL = "DELETE FROM MED_OCS.EMR_INFECT ";
                SQL = SQL + ComNum.VBLF + " WHERE EMRNO = '" + gEmrno + "' ";
                SQL = SQL + ComNum.VBLF + " AND ACPNO = '" + gAcpno + "' ";
                SQL = SQL + ComNum.VBLF + " AND PTNO = '" + gPtno + "' ";
                SQL = SQL + ComNum.VBLF + " AND CHARTDATE = '" + gDate + "' ";
                SQL = SQL + ComNum.VBLF + " AND CHARTTIME = '" + gTime + "' ";
                SQL = SQL + ComNum.VBLF + " AND ITEMCD = '" + gItemcd + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return ;
                }

                clsDB.setCommitTran(clsDB.DbCon);

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return ;
            }

            //다시 보여줌
            SetDefaultData(mSpdView, mJOBGB);
            LoadData(mSpdView, mJOBGB);
            
        }

        private void DeleteOne(FarPoint.Win.Spread.SheetView SpdView)
        {
            int intSelCol = 0;

            if (SpdView.Columns.Count <= 3) return;

            if (SpdView.ActiveColumnIndex < 3) return;
            intSelCol = SpdView.ActiveColumnIndex;

            //EMRNO가 있는지 확인
            string strEmrNo = Convert.ToString(VB.Val(SpdView.Cells[SpdView.Rows.Count - 4, intSelCol].Text.Trim()));

            if (VB.Val(strEmrNo) == 0)
            {
                for (int k = 4; k < SpdView.RowCount - 4; k++)
                {
                    SpdView.Cells[k, intSelCol].Text = "";
                }
                return;
            }
            mstrEmrNo = strEmrNo;
            if (pDelData() == true)
            {
                SetDefaultData(SpdView, mJOBGB);
                LoadData(SpdView, mJOBGB);
            }
            mstrEmrNo = "0";
        }

        private void DeleteAll(FarPoint.Win.Spread.SheetView SpdView, string strUseId = "")
        {
            int intSelCol = 0;

            if (SpdView.Columns.Count <= 3) return;

            if (SpdView.ActiveColumnIndex < 3) return;
            intSelCol = SpdView.ActiveColumnIndex;
            //EMRNO가 있는지 확인
            string strEmrNo = Convert.ToString(VB.Val(SpdView.Cells[SpdView.Rows.Count - 4, intSelCol].Text.Trim()));

            if (VB.Val(strEmrNo) == 0)
            {
                SpdView.Columns[intSelCol].Remove();
                if (SaveTimeSet(SpdView, mJOBGB) == false)
                {
                    ComFunc.MsgBoxEx(this, "시간 저장중 에러가 발생했습니다.");
                }

                SetDefaultData(SpdView, mJOBGB);
                LoadData(SpdView, mJOBGB);
                return;
            }
            mstrEmrNo = strEmrNo;
            if (pDelData(strUseId) == true)
            {
                SpdView.Columns[intSelCol].Remove();
                if (SaveTimeSet(SpdView, mJOBGB) == false)
                {
                    ComFunc.MsgBoxEx(this, "시간 저장중 에러가 발생했습니다.");
                }
                SetDefaultData(SpdView, mJOBGB);
                LoadData(SpdView, mJOBGB);
            }
            mstrEmrNo = "0";
        }

        private void mbtnUp_Click(object sender, EventArgs e)
        {
            panWrite.Visible = false;
        }

        private void mbtnDown_Click(object sender, EventArgs e)
        {
            panWrite.Visible = true;
        }

        private void optAll_CheckedChanged(object sender, EventArgs e)
        {
            if (optAll.Checked == true)
            {
                ColVisible();
            }
        }

        private void optDay_CheckedChanged(object sender, EventArgs e)
        {
            if (optDay.Checked == true)
            {
                ColVisible();
            }
        }

        private void optEve_CheckedChanged(object sender, EventArgs e)
        {
            if (optEve.Checked == true)
            {
                ColVisible();
            }
        }

        private void optNight_CheckedChanged(object sender, EventArgs e)
        {
            if (optNight.Checked == true)
            {
                ColVisible();
            }
        }
        private void ColVisible()
        {
            if (optAll.Checked == true)
            {
                VisibleSetTrue();
            }
            else if (optDay.Checked == true)
            {
                VisibleSet(mstrDayTime);
            }
            else if (optEve.Checked == true)
            {
                VisibleSet(mstrEveTime);
            }
            else if (optNight.Checked == true)
            {
                VisibleSet(mstrNightTime);
            }
        }

        private void VisibleSetDt(FarPoint.Win.Spread.SheetView SpdView, string STime, string ETime)
        {
            int i = 0;

            for (i = 3; i < SpdView.Columns.Count; i++)
            {
                SpdView.Columns[i].Visible = true;
            }

            for (i = 3; i < SpdView.Columns.Count; i++)
            {
                if (VB.Val(SpdView.Cells[2, i].Text.Replace(":", "")) < VB.Val(STime.Replace(":", "")))
                {
                    SpdView.Columns[i].Visible = false;
                }
                else if (VB.Val(SpdView.Cells[2, i].Text.Replace(":", "")) > VB.Val(ETime.Replace(":", "")))
                {
                    SpdView.Columns[i].Visible = false;
                }
            }
        }

        private void mbtnInsertTime_Click(object sender, EventArgs e)
        {
            if (frmNrIcuTimeSetX != null)
            {
                frmNrIcuTimeSetX.Dispose();
                frmNrIcuTimeSetX = null;
            }

            frmNrIcuTimeSetX = new frmNrIcuTimeSet();
            frmNrIcuTimeSetX.rSetTime += new frmNrIcuTimeSet.SetTime(frmNrIcuTimeSet_SetTime);
            frmNrIcuTimeSetX.rEventClosed += new frmNrIcuTimeSet.EventClosed(frmNrIcuTimeSet_EventClosed);
            frmNrIcuTimeSetX.TopMost = true;
            frmNrIcuTimeSetX.ShowDialog(this);
        }

        private void frmNrIcuTimeSet_EventClosed()
        {
            frmNrIcuTimeSetX.Dispose();
            frmNrIcuTimeSetX = null;
        }

        private void frmNrIcuTimeSet_SetTime(string strTime)
        {
            frmNrIcuTimeSetX.Dispose();
            frmNrIcuTimeSetX = null;

            string[] arryTime = VB.Split(strTime, "/");

            int i = 0;
            int j = 0;

            SetSpread();

            for (i = 0; i < arryTime.Length; i++)
            {
                string strTimeCheck = arryTime[i];
                bool blnFind = false;

                for (j = 3; j < mSpdView.Columns.Count; j++)
                {
                    if (mSpdView.Cells[2, j].Text.Trim() == strTimeCheck.Trim())
                    {
                        blnFind = true;
                        break;
                    }
                }
                if (blnFind == false)
                {
                    SaveTime(arryTime[i].Trim());
                }
            }

            ColVisible();
        }

        private void mbtnSaveItem_Click(object sender, EventArgs e)
        {
            if (SaveIoItemEx() == true)
            {
                SetDefaultData(mSpdView, mJOBGB);
                LoadData(mSpdView, mJOBGB);
            }
        }

        private bool SaveIoItemEx()
        {
            bool rtnVal = false;

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (ssIoForm_Sheet1.RowCount == 0) return false;

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                string strCurDataTime = ComQuery.CurrentDateTime(clsDB.DbCon,"A");
                string strWDate = VB.Left(strCurDataTime, 8);
                string strWTime = VB.Right(strCurDataTime, 6);
                string strChratDate = dtpFrDate.Value.ToString("yyyyMMdd");

                SQL = "";
                SQL = "SELECT ITEMCD  FROM " + ComNum.DB_EMR + "AEMRBVITALSET ";
                SQL = SQL + ComNum.VBLF + "WHERE ACPNO = " + p.acpNo;
                SQL = SQL + ComNum.VBLF + "    AND JOBGB = '" + mJOBGB + "'";
                SQL = SQL + ComNum.VBLF + "    AND CHARTDATE = '" + strChratDate + "'";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRBVITALSET ";
                    //SQL = SQL + ComNum.VBLF + "(ACPNO, PTNO, CHARTDATE, JOBGB, ITEMCD, WRITEDATE, WRITETIME, WRITEUSEID)";
                    SQL = SQL + ComNum.VBLF + "SELECT ACPNO, PTNO,  '" + strChratDate + "', JOBGB, ITEMCD, '" + strWDate + "','" + strWTime + "' ,'" + clsType.User.IdNumber + "'";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET ";
                    SQL = SQL + ComNum.VBLF + "WHERE ACPNO = " + p.acpNo;
                    SQL = SQL + ComNum.VBLF + "    AND JOBGB = '" + mJOBGB + "'";
                    SQL = SQL + ComNum.VBLF + "    AND CHARTDATE = ( SELECT MAX(CHARTDATE)  FROM " + ComNum.DB_EMR + "AEMRBVITALSET ";
                    SQL = SQL + ComNum.VBLF + "                                WHERE ACPNO = " + p.acpNo;
                    SQL = SQL + ComNum.VBLF + "                                    AND JOBGB = '" + mJOBGB + "')";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }
                dt.Dispose();
                dt = null;

                for (i = 0; i < ssIoForm_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(ssIoForm_Sheet1.Cells[i, 0].Value) == false) continue;

                    SQL = "";
                    SQL = "SELECT ITEMCD  FROM " + ComNum.DB_EMR + "AEMRBVITALSET ";
                    SQL = SQL + ComNum.VBLF + "WHERE ACPNO = " + p.acpNo;
                    SQL = SQL + ComNum.VBLF + "    AND JOBGB = '" + mJOBGB + "'";
                    SQL = SQL + ComNum.VBLF + "    AND CHARTDATE = '" + strChratDate + "'";
                    SQL = SQL + ComNum.VBLF + "    AND ITEMCD = '" + ssIoForm_Sheet1.Cells[i, 1].Text.Trim() + "'";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return false;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRBVITALSET ";
                        SQL = SQL + ComNum.VBLF + "(ACPNO, PTNO, CHARTDATE, JOBGB, ITEMCD, WRITEDATE, WRITETIME, WRITEUSEID)";
                        SQL = SQL + ComNum.VBLF + "VALUES ('" + p.acpNo + "','" + p.ptNo + "','" + dtpFrDate.Value.ToString("yyyyMMdd") + "',";
                        SQL = SQL + ComNum.VBLF + "'" + mJOBGB + "','" + ssIoForm_Sheet1.Cells[i, 1].Text.Trim() + "','" + strWDate + "','" + strWTime + "' ,'" + clsType.User.IdNumber + "')";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                    }
                    dt.Dispose();
                    dt = null;
                }
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                for (i = 0; i < ssIoForm_Sheet1.RowCount; i++)
                {
                    ssIoForm_Sheet1.Cells[i, 0].Value = false;
                }
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void mbtnUdIoDown_Click(object sender, EventArgs e)
        {
            panIoOrder.Height = 30;
        }

        private void mbtnUdIoUp_Click(object sender, EventArgs e)
        {
            panIoOrder.Height = 450;
        }

        private void mbtnSearchIo_Click(object sender, EventArgs e)
        {
            SearchIoItem("ALL");
        }

        private void SearchIoItem(string strFlag)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            ssIoForm_Sheet1.RowCount = 0;

            SQL = "";
            SQL = "SELECT BASVAL, BASCD, BASNAME, BASEXNAME FROM " + ComNum.DB_EMR + "AEMRBASCD";
            SQL = SQL + ComNum.VBLF + "WHERE BSNSCLS = 'ICU관리'";
            SQL = SQL + ComNum.VBLF + "    AND UNITCLS = '섭취배설'";
            if (strFlag == "ITEM")
            {
                SQL = SQL + ComNum.VBLF + "    AND UPPER(BASNAME) LIKE '" + txtItemName.Text.Trim().ToUpper() + "%'";
            }
            SQL = SQL + ComNum.VBLF + "    AND REMARK2 IS NOT NULL";
            if (optSearchIo02.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "    AND REMARK2 = 'DIET'";
            }
            else if (optSearchIo03.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "    AND REMARK2 = '혈액'";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "    AND REMARK2 <> 'DIET'";
            }
            SQL = SQL + ComNum.VBLF + "    AND USECLS = '0'";
            SQL = SQL + ComNum.VBLF + "ORDER BY BASNAME";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }
            ssIoForm_Sheet1.RowCount = dt.Rows.Count;
            ssIoForm_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssIoForm_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BASCD"].ToString().Trim();
                ssIoForm_Sheet1.Cells[i, 2].Text = dt.Rows[i]["BASNAME"].ToString().Trim();
                ssIoForm_Sheet1.Cells[i, 3].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        private void txtItemName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            if (txtItemName.Text.Trim() == "") return;

            SearchIoItem("ITEM");
        }

        private void ssIoForm_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssIoForm_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssIoForm, e.Column);
                return;
            }
        }

        private void mbtnSaveMix_Click(object sender, EventArgs e)
        {
            if (SaveIoItem() == true)
            {
            }
        }

        private bool SaveIoItem()
        {
            bool rtnVal = false;
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (ssIO_Sheet1.RowCount == 0) return rtnVal;
            if (ssView_Sheet1.RowCount == 0) return rtnVal;

            string strITEMCD = ssIO_Sheet1.Cells[ssIO_Sheet1.ActiveRowIndex, 0].Text.Trim();
            string strITEMNM = "";

            if (strITEMCD == "") return rtnVal;

            SQL = "";
            SQL = "SELECT BASCD, BASNAME  FROM " + ComNum.DB_EMR + "AEMRBASCD ";
            SQL = SQL + ComNum.VBLF + "WHERE BSNSCLS = 'ICU관리' ";
            SQL = SQL + ComNum.VBLF + "    AND UNITCLS = '섭취배설' ";
            SQL = SQL + ComNum.VBLF + "    AND BASCD = '" + strITEMCD + "' ";
            SQL = SQL + ComNum.VBLF + "    AND REMARK2 <> 'DIET' ";
            SQL = SQL + ComNum.VBLF + "    AND REMARK2 IS NOT NULL ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBoxEx(this, "Mix할 수액을 선택해 주십시요.");
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            strITEMNM = dt.Rows[0]["BASNAME"].ToString().Trim();
            dt.Dispose();
            dt = null;

            if (ComFunc.MsgBoxQ(strITEMNM + "에 선택한 처방을 Mix하시겠습니까?", "ICU관리", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                string strCurDataTime = ComQuery.CurrentDateTime(clsDB.DbCon,"A");
                string strWDate = VB.Left(strCurDataTime, 8);
                string strWTime = VB.Right(strCurDataTime, 6);

                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    if (ssView_Sheet1.Cells[i, 14].Text.Trim() == "") continue;
                    if (ssView_Sheet1.Cells[i, 1].BackColor == Color.Yellow) continue;

                    if (Convert.ToBoolean(ssView_Sheet1.Cells[i, 0].Value) == false) continue;

                    string strOrdCd = ssView_Sheet1.Cells[i, 14].Text.Trim();
                    string strOrdNm = ssView_Sheet1.Cells[i, 15].Text.Trim();

                    SQL = "";
                    SQL = "SELECT ITEMCD  FROM " + ComNum.DB_EMR + "AEMRIOMIX ";
                    SQL = SQL + ComNum.VBLF + "WHERE ACPNO = " + p.acpNo;
                    SQL = SQL + ComNum.VBLF + "    AND CHARTDATE = '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";
                    SQL = SQL + ComNum.VBLF + "    AND ITEMCD = '" + strITEMCD + "'";
                    SQL = SQL + ComNum.VBLF + "    AND ORDER_KEY = " + ssView_Sheet1.Cells[i, 11].Text.Trim();
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return false;
                    }
                    
                    if (dt.Rows.Count == 0)
                    {
                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRIOMIX ";
                        SQL = SQL + ComNum.VBLF + "(ACPNO, PTNO, CHARTDATE, ITEMCD, ORDERCD, ORDERNM, ORDER_KEY, WRITEDATE, WRITETIME, WRITEUSEID, ITEMNAME)";
                        SQL = SQL + ComNum.VBLF + "VALUES (" + p.acpNo + ",'" + p.ptNo + "','" + dtpFrDate.Value.ToString("yyyyMMdd") + "',";
                        SQL = SQL + ComNum.VBLF + "'" + strITEMCD + "','" + strOrdCd.Trim() + "','" + strOrdNm.Trim() + "', " + ssView_Sheet1.Cells[i, 11].Text.Trim() + ",'" + strWDate + "','" + strWTime + "' ,'" + clsType.User.IdNumber + "', '" + strITEMNM + "')";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                    }
                    dt.Dispose();
                    dt = null;
                }
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Value = false;
                }

                ViewToolTip(strITEMCD, ssIO_Sheet1.ActiveRowIndex, 2);
                GetMixInfo(strITEMCD);
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void GetMixInfo(string strITEMCD)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssIoMix_Sheet1.RowCount = 0;

            SQL = "";
            SQL = "SELECT A.ITEMCD, A.ORDERCD, A.ORDERNM, A.ORDER_KEY, A.ITEMNAME, B.BASNAME ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRIOMIX A";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "    ON B.BASCD = A.ITEMCD ";
            SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = 'ICU관리' ";
            SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '섭취배설' ";
            SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";
            SQL = SQL + ComNum.VBLF + "    AND A.ITEMCD = '" + strITEMCD + "'";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }

            string strITEMCD1 = "";

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (strITEMCD1 != dt.Rows[i]["ITEMCD"].ToString().Trim())
                {
                    ssIoMix_Sheet1.RowCount = ssIoMix_Sheet1.RowCount + 1;
                    ssIoMix_Sheet1.Cells[ssIoMix_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["ITEMNAME"].ToString().Trim();
                    ssIoMix_Sheet1.Cells[ssIoMix_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["ITEMCD"].ToString().Trim();
                }
                strITEMCD1 = dt.Rows[i]["ITEMCD"].ToString().Trim();
                ssIoMix_Sheet1.RowCount = ssIoMix_Sheet1.RowCount + 1;
                ssIoMix_Sheet1.Cells[ssIoMix_Sheet1.RowCount - 1, 1].Text = " ┗" + dt.Rows[i]["ORDERNM"].ToString().Trim().Replace("┗", "");
                ssIoMix_Sheet1.Cells[ssIoMix_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["ITEMCD"].ToString().Trim();
                ssIoMix_Sheet1.Cells[ssIoMix_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["ORDER_KEY"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;
        }

        private void mbtnDeleteItem_Click(object sender, EventArgs e)
        {
            if (DeleteIoItem() == true)
            {

            }
        }

        private bool DeleteIoItem()
        {
            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            bool rtnVal = false;
            string strITEMCD = "";

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                for (i = 0; i < ssIoMix_Sheet1.RowCount; i++)
                {
                    if (i != 0)
                    {
                        if (Convert.ToBoolean(ssIoMix_Sheet1.Cells[i, 0].Value) == false) continue;

                        strITEMCD = ssIoMix_Sheet1.Cells[i, 2].Text.Trim();
                        string strORDER_KEY = ssIoMix_Sheet1.Cells[i, 3].Text.Trim();

                        SQL = "";
                        SQL = "DELETE  FROM " + ComNum.DB_EMR + "AEMRIOMIX ";
                        SQL = SQL + ComNum.VBLF + "WHERE ACPNO = " + p.acpNo;
                        SQL = SQL + ComNum.VBLF + "    AND CHARTDATE = '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";
                        SQL = SQL + ComNum.VBLF + "    AND ITEMCD = '" + strITEMCD + "'";
                        SQL = SQL + ComNum.VBLF + "    AND ORDER_KEY = " + strORDER_KEY;
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                GetMixInfo(strITEMCD);

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }

        }

        private void mbtnUpTop_Click(object sender, EventArgs e)
        {
            panTop.Height = 30;
        }

        private void mbtnDownTop_Click(object sender, EventArgs e)
        {
            panTop.Height = 60;
        }

        private void mbtnSaveName_Click(object sender, EventArgs e)
        {
            if (SaveIoItemName() == true)
            {

            }
        }

        private bool SaveIoItemName()
        {
            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            bool rtnVal = false;
            string strITEMCD = "";
            string strITEMNM = "";

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                for (i = 0; i < ssIoMix_Sheet1.RowCount; i++)
                {
                    strITEMCD = ssIoMix_Sheet1.Cells[i, 2].Text.Trim();

                    if (i == 0)
                    {
                        strITEMNM = ssIoMix_Sheet1.Cells[i, 1].Text.Trim();
                        if (Convert.ToBoolean(ssIoMix_Sheet1.Cells[i, 0].Value) == true)
                        {
                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_EMR + "AEMRIOMIX SET ";
                            SQL = SQL + ComNum.VBLF + "    ITEMNAME = '" + strITEMNM + "'";
                            SQL = SQL + ComNum.VBLF + "WHERE ACPNO = " + p.acpNo;
                            SQL = SQL + ComNum.VBLF + "    AND CHARTDATE = '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";
                            SQL = SQL + ComNum.VBLF + "    AND ITEMCD = '" + strITEMCD + "'";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBoxEx(this, SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (Convert.ToBoolean(ssIoMix_Sheet1.Cells[i, 0].Value) == false) continue;

                        strITEMCD = ssIoMix_Sheet1.Cells[i, 2].Text.Trim();
                        string strORDERNM = ssIoMix_Sheet1.Cells[i, 1].Text.Trim();
                        string strORDER_KEY = ssIoMix_Sheet1.Cells[i, 3].Text.Trim();

                        SQL = "";
                        SQL = "UPDATE " + ComNum.DB_EMR + "AEMRIOMIX SET ";
                        SQL = SQL + ComNum.VBLF + "    ORDERNM = '" + strORDERNM.Replace("┗", "") + "'";
                        SQL = SQL + ComNum.VBLF + "WHERE ACPNO = " + p.acpNo;
                        SQL = SQL + ComNum.VBLF + "    AND CHARTDATE = '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";
                        SQL = SQL + ComNum.VBLF + "    AND ITEMCD = '" + strITEMCD + "'";
                        SQL = SQL + ComNum.VBLF + "    AND ORDER_KEY = " + strORDER_KEY;
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                GetMixInfo(strITEMCD);

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }

        }

        private void mbtnTotal_Click(object sender, EventArgs e)
        {
            if (panTot.Visible == true)
            {
                panTot.Visible = false;
            }
            else
            {
                SetIoTotDefault();

                LoadIoTot("Day", "070000", "145900");
                LoadIoTot("Eve", "150000", "225900");
                LoadIoTot("Night", "230000", "065900");
                LoadIoTot("Tot", "070000", "065900");

                panTot.Visible = true;
            }
        }

        private void LoadIoTot(string strDuty, string strSTime, string strETime)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int i = 0;
            int j = 0;
            int k = 0;

            ssIoTot_Sheet1.Columns.Count = ssIoTot_Sheet1.Columns.Count + 1;
            j = ssIoTot_Sheet1.Columns.Count - 1;
            ssIoTot_Sheet1.Columns[j].Width = 40;

            clsSpread.SetTypeAndValue(ssIoTot_Sheet1, 0, j, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(ssIoTot_Sheet1, 1, j, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(ssIoTot_Sheet1, 2, j, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(ssIoTot_Sheet1, 3, j, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            ssIoTot_Sheet1.Cells[0, j, 3, j].Font = new Font("굴림", 10, FontStyle.Bold);
            ssIoTot_Sheet1.Cells[0, j, 3, j].BackColor = Color.LightBlue;
            ssIoTot_Sheet1.Cells[2, j].Text = strDuty;
            ssIoTot_Sheet1.AddSpanCell(2, j, 2, 1);

            Cursor.Current = Cursors.WaitCursor;
            SQL = " SELECT  ";
            SQL = SQL + ComNum.VBLF + "    B.ITEMCD,  ";
            SQL = SQL + ComNum.VBLF + "    SUM(DECODE(B.ITEMVALUE, NULL, 0, B.ITEMVALUE)) AS ITEMVALUE , MAX(BC.BASNAME) AS ITEMNAME";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A  ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRBVITALTIME TM ";
            SQL = SQL + ComNum.VBLF + "    ON  A.ACPNO = TM.ACPNO ";
            SQL = SQL + ComNum.VBLF + "    AND TM.JOBGB = 'IIO' ";
            SQL = SQL + ComNum.VBLF + "    AND (A.CHARTDATE || A.CHARTTIME )= (TM.CHARTDATE || TM.TIMEVALUE || '00') ";
            if ((strDuty == "Tot") || (strDuty == "Night"))
            {
                SQL = SQL + ComNum.VBLF + "  AND (TM.CHARTDATE || TM.TIMEVALUE || '00')>= '" + dtpFrDate.Value.ToString("yyyyMMdd") + strSTime + "' ";
                SQL = SQL + ComNum.VBLF + "  AND (TM.CHARTDATE || TM.TIMEVALUE || '00') <= '" + (VB.DateAdd("d", 1, dtpFrDate.Value.ToString("yyyy-MM-dd"))).ToString("yyyyMMdd") + strETime + "' ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "  AND (TM.CHARTDATE || TM.TIMEVALUE || '00') >= '" + dtpFrDate.Value.ToString("yyyyMMdd") + strSTime + "' ";
                SQL = SQL + ComNum.VBLF + "  AND (TM.CHARTDATE || TM.TIMEVALUE || '00') <= '" + dtpFrDate.Value.ToString("yyyyMMdd") + strETime + "' ";
            }
            SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B ";
            SQL = SQL + ComNum.VBLF + "    ON  B.EMRNO = A.EMRNO ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BC  ";
            SQL = SQL + ComNum.VBLF + "   ON BC.BASCD = B.ITEMCD  ";
            SQL = SQL + ComNum.VBLF + "   AND BC.BSNSCLS = 'ICU관리'  ";
            SQL = SQL + ComNum.VBLF + "   AND BC.UNITCLS = '섭취배설' ";
            SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "  AND A.FORMNO = " + mstrFormNo;
            SQL = SQL + ComNum.VBLF + "  AND A.UPDATENO = " + mstrUpdateNo;
            SQL = SQL + ComNum.VBLF + "  AND A.CHARTUSEID <> '합계' ";

            if ((strDuty == "Tot") || (strDuty == "Night"))
            {
                SQL = SQL + ComNum.VBLF + "  AND (A.CHARTDATE || A.CHARTTIME || '00') >= '" + dtpFrDate.Value.ToString("yyyyMMdd") + strSTime + "' ";
                SQL = SQL + ComNum.VBLF + "  AND (A.CHARTDATE || A.CHARTTIME || '00') <= '" + (VB.DateAdd("d", 1, dtpFrDate.Value.ToString("yyyy-MM-dd"))).ToString("yyyyMMdd") + strETime + "' ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "  AND (A.CHARTDATE || A.CHARTTIME || '00') >= '" + dtpFrDate.Value.ToString("yyyyMMdd") + strSTime + "' ";
                SQL = SQL + ComNum.VBLF + "  AND (A.CHARTDATE || A.CHARTTIME || '00') <= '" + dtpFrDate.Value.ToString("yyyyMMdd") + strETime + "' ";
            }
            
            SQL = SQL + ComNum.VBLF + "GROUP BY  B.ITEMCD ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (ssIoTot_Sheet1.Cells[3, j].Text.Trim() == "합계")
                {
                    continue;
                }
                
                for (k = 4; k < ssIoTot_Sheet1.RowCount - 4; k++)
                {
                    if (dt.Rows[i]["ITEMCD"].ToString().Trim() == ssIoTot_Sheet1.Cells[k, 0].Text.Trim())
                    {
                        if ((dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000030622") || (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000030623"))
                        {
                            ssIoTot_Sheet1.Cells[k, j].BackColor = Color.LightBlue;
                        }
                        if (VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim()) == 0)
                        {
                            ssIoTot_Sheet1.Cells[k, j].Text = "";
                        }
                        else
                        {
                            ssIoTot_Sheet1.Cells[k, j].Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                        }
                    }
                }
                if (i >= dt.Rows.Count) break;
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        private void SetIoTotDefault()
        {
            //일자별 등록된 것이 있는지 파악해서 있으면 세팅을 하고
            //없으면 기본을 가지고 세팅을 한다.
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SetTopRow(ssIoTot_Sheet1);

            string strBASEXNAME = "";
            int intS = 0;

            Cursor.Current = Cursors.WaitCursor;
            SQL = "SELECT B.BASCD, B.BASNAME, B.BASEXNAME, B.DISSEQNO, A.CHARTDATE ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET A";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "    ON B.BASCD = A.ITEMCD";
            SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = 'ICU관리'";
            SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '섭취배설'";
            SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND A.JOBGB = 'IIO'";
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = (SELECT MAX(CHARTDATE) AS CHARTDATE ";
            SQL = SQL + ComNum.VBLF + "                                        FROM " + ComNum.DB_EMR + "AEMRBVITALSET ";
            SQL = SQL + ComNum.VBLF + "                                        WHERE ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "                                            AND JOBGB = 'IIO'";
            SQL = SQL + ComNum.VBLF + "                                            AND CHARTDATE <= '" + dtpFrDate.Value.ToString("yyyyMMdd") + "')";
            SQL = SQL + ComNum.VBLF + "ORDER BY B.DISSEQNO, B.BASNAME";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssIoTot_Sheet1.RowCount = ssIoTot_Sheet1.RowCount + 1;
                    clsSpread.SetTypeAndValue(ssIoTot_Sheet1, ssIoTot_Sheet1.RowCount - 1, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);
                    clsSpread.SetTypeAndValue(ssIoTot_Sheet1, ssIoTot_Sheet1.RowCount - 1, 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_L, "", true);
                    clsSpread.SetTypeAndValue(ssIoTot_Sheet1, ssIoTot_Sheet1.RowCount - 1, 2, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);

                    ssIoTot_Sheet1.Cells[ssIoTot_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["BASCD"].ToString().Trim();
                    if (strBASEXNAME != dt.Rows[i]["BASEXNAME"].ToString().Trim())
                    {
                        ssIoTot_Sheet1.Cells[ssIoTot_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                        if (i != 0)
                        {
                            ssIoTot_Sheet1.AddSpanCell(intS, 1, ssIoTot_Sheet1.RowCount - 1 - intS, 1);
                        }
                        intS = ssIoTot_Sheet1.RowCount - 1;
                    }
                    strBASEXNAME = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                    ssIoTot_Sheet1.Cells[ssIoTot_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["BASNAME"].ToString().Trim();
                    if ((dt.Rows[i]["BASCD"].ToString().Trim() == "I0000030622") || (dt.Rows[i]["BASCD"].ToString().Trim() == "I0000030623"))
                    {
                        ssIoTot_Sheet1.Cells[ssIoTot_Sheet1.RowCount - 1, 1].BackColor = Color.LightBlue;
                        ssIoTot_Sheet1.Cells[ssIoTot_Sheet1.RowCount - 1, 2].BackColor = Color.LightBlue;
                    }
                    ssIoTot_Sheet1.SetRowHeight(ssIoTot_Sheet1.RowCount - 1, ComNum.SPDROWHT);
                }
                dt.Dispose();
                dt = null;
                ssIoTot_Sheet1.AddSpanCell(intS, 1, ssIoTot_Sheet1.RowCount - intS, 1);

                SetButtonRow(ssIoTot_Sheet1);
                //SetTimeSet(ssIoTot_Sheet1, "IIO");
                return;
            }
            else
            {
                dt.Dispose();
                dt = null;
                SetButtonRow(ssIoTot_Sheet1);
                //SetTimeSet(ssIoTot_Sheet1, "IIO");
            }
        }

        private void mbtnSearchBST_Click(object sender, EventArgs e)
        {
            //TODO return;
            return;
            //if (p == null) return;
            //frmEmrBstView frmEmrBstViewX = new frmEmrBstView(p.acpNo, "646");
            //frmEmrBstViewX.TopMost = true;
            //frmEmrBstViewX.ShowDialog(this);
        }

        private void mbtnChart_236_Click(object sender, EventArgs e)
        {
            LoadChart("236", "0");
        }

        private void mbtnChart_238_Click(object sender, EventArgs e)
        {
            LoadChart("238", "0");
        }

        private void LoadChart(string strFORMNO, string strUPDATENO)
        {
            string strFORMTYPE = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "SELECT MAX(UPDATENO) AS UPDATENO FROM " + ComNum.DB_EMR + "AEMRFORM ";
            SQL = SQL + ComNum.VBLF + "      WHERE FORMNO = " + VB.Val(strFORMNO);
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }
            strUPDATENO = dt.Rows[0]["UPDATENO"].ToString().Trim();
            dt.Dispose();
            dt = null;

            if (VB.Val(p.acpNo) == 0) return;
            EmrForm fWrite = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, strFORMNO, strUPDATENO);
            if (clsEmrFunc.isInOutFix(fWrite, p.inOutCls) == false)
            {
                return;
            }

            strFORMTYPE = fWrite.FmFORMTYPE.ToString();

            if (strFORMTYPE == "2")
            {
                if (frmOcrPrintX != null)
                {
                    frmOcrPrintX.Dispose();
                    frmOcrPrintX = null;
                }

                frmOcrPrintX = new frmOcrPrint(strFORMNO, strUPDATENO, p.acpNo, "W");
                frmOcrPrintX.ShowDialog();
            }
            else
            {
                if (frmEmrChartX != null)
                {
                    frmEmrChartX.Dispose();
                    frmEmrChartX = null;
                }

                string strEMRNO = "0";
                //당일 차팅 내역이 있는지 조회를 한다.
                SQL = "SELECT MAX(EMRNO) AS EMRNO FROM " + ComNum.DB_EMR + "AEMRCHARTMST ";
                SQL = SQL + ComNum.VBLF + "WHERE ACPNO = " + VB.Val(p.acpNo);
                SQL = SQL + ComNum.VBLF + "      AND CHARTDATE = '" + p.medFrDate + "'";
                SQL = SQL + ComNum.VBLF + "      AND FORMNO = " + VB.Val(strFORMNO);
                SQL = SQL + ComNum.VBLF + "      AND UPDATENO = " + VB.Val(strUPDATENO);
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strEMRNO = (VB.Val(dt.Rows[0]["EMRNO"].ToString().Trim())).ToString();
                }
                dt.Dispose();
                dt = null;
                frmEmrChartX = new frmEmrChartNew(strFORMNO, strUPDATENO, p, strEMRNO, "W"); //, true);
                //frmEmrChartX.rEventClosed += new frmEmrChartNew.EventClosed(frmEmrChart_EventClosed);
                frmEmrChartX.ShowDialog();
            }
        }

        private void frmEmrChart_EventClosed()
        {
            frmEmrChartX.Dispose();
            frmEmrChartX = null;
        }

        private void mbtnLine01_Click(object sender, EventArgs e)
        {
            //TOTO return;
            return;
            //frmLineSheetMng frmLineSheetMngX = new frmLineSheetMng(p.acpNo, "01", dtpFrDate.Value.ToString("yyyyMMdd"), "Line Sheet");
            //frmLineSheetMngX.ShowDialog();
        }

        private void mbtnLine02_Click(object sender, EventArgs e)
        {
            //TOTO return;
            return;
            //frmLineSheetMng frmLineSheetMngX = new frmLineSheetMng(p.acpNo, "02", dtpFrDate.Value.ToString("yyyyMMdd"), "배액관");
            //frmLineSheetMngX.ShowDialog();
        }

        private void mbtnSearchAll_Click(object sender, EventArgs e)
        {
            GetVitalGraph();
        }

        private void GetVitalGraph()
        {

            bool blnData = false;
            int intRow = 0;
            int i = 0;
            int intSeries = 0;
            int intSeriesE = -1;

            chartVital.Series.Clear();
            chartVital.Titles.Clear();
            chartVital.ChartAreas.Clear();

            chartVital.ChartAreas.Add("Default");
            chartVital.Titles.Add("Vital");
            chartVital.Titles[0].Font = new Font("굴림", 16F, FontStyle.Bold);

            if ((chkSBP.Checked == false) && (chkPR.Checked == false) && (chkBT.Checked == false))
            {
                return;
            }

            try
            {
                for (intRow = 4; intRow < ssVital_Sheet1.RowCount - 4; intRow++)
                {
                    if (chkSBP.Checked == true)
                    {
                        if (ssVital_Sheet1.Cells[intRow, 0].Text.Trim() == "I0000002018")
                        {
                            for (i = 3; i < ssVital_Sheet1.ColumnCount; i++)
                            {
                                if (VB.Val(ssVital_Sheet1.Cells[intRow, i].Text.Trim()) > 0)
                                {
                                    blnData = true;
                                }
                            }
                        }

                        if (ssVital_Sheet1.Cells[intRow, 0].Text.Trim() == "I0000001765")
                        {
                            for (i = 3; i < ssVital_Sheet1.ColumnCount; i++)
                            {
                                if (VB.Val(ssVital_Sheet1.Cells[intRow, i].Text.Trim()) > 0)
                                {
                                    blnData = true;
                                }
                            }
                        }
                    }
                    if (chkPR.Checked == true)
                    {
                        if (ssVital_Sheet1.Cells[intRow, 0].Text.Trim() == "I0000014815")
                        {
                            for (i = 3; i < ssVital_Sheet1.ColumnCount; i++)
                            {
                                if (VB.Val(ssVital_Sheet1.Cells[intRow, i].Text.Trim()) > 0)
                                {
                                    blnData = true;
                                }
                            }
                        }
                    }
                    if (chkBT.Checked == true)
                    {
                        if (ssVital_Sheet1.Cells[intRow, 0].Text.Trim() == "I0000001811")
                        {
                            for (i = 3; i < ssVital_Sheet1.ColumnCount; i++)
                            {
                                if (VB.Val(ssVital_Sheet1.Cells[intRow, i].Text.Trim()) > 0)
                                {
                                    blnData = true;
                                }
                            }
                        }
                    }
                }

                if (blnData == false) return;

                chartVital.ChartAreas["Default"].Position = new System.Windows.Forms.DataVisualization.Charting.ElementPosition(0, 5, 85, 90);
                chartVital.ChartAreas["Default"].InnerPlotPosition = new System.Windows.Forms.DataVisualization.Charting.ElementPosition(13, 5, 90, 90);
                
                if (chkSBP.Checked == true)
                {
                    chartVital.Series.Add("SBP");
                    chartVital.Series[intSeries].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                    chartVital.Series[intSeries].MarkerImage = clsEmrType.EmrSvrInfo.EmrClient + "\\Icon\\SBP.png";
                    chartVital.Series[intSeries].IsValueShownAsLabel = false;


                    for (intRow = 4; intRow < ssVital_Sheet1.RowCount - 4; intRow++)
                    {
                        if (ssVital_Sheet1.Cells[intRow, 0].Text.Trim() == "I0000002018")
                        {
                            blnData = true;

                            for (i = 3; i < ssVital_Sheet1.ColumnCount; i++)
                            {
                                if (VB.Val(ssVital_Sheet1.Cells[intRow, i].Text.Trim()) == 0)
                                {
                                    chartVital.Series[intSeries].Points.AddY(double.NaN); //Chart1.Series[0].Points[14].IsEmpty = true;
                                    chartVital.Series[intSeries].Points[i - 3].IsEmpty = true;
                                }
                                else
                                {
                                    chartVital.Series[intSeries].Points.AddY(VB.Val(ssVital_Sheet1.Cells[intRow, i].Text.Trim()));
                                }
                            }
                            intSeriesE = 0;
                        }
                    }

                    intSeries = intSeries + 1;

                    chartVital.Series.Add("DBP");
                    chartVital.Series[intSeries].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                    chartVital.Series[intSeries].MarkerImage = clsEmrType.EmrSvrInfo.EmrClient + "\\Icon\\DBP.png";
                    chartVital.Series[intSeries].IsValueShownAsLabel = false;


                    for (intRow = 4; intRow < ssVital_Sheet1.RowCount - 4; intRow++)
                    {
                        if (ssVital_Sheet1.Cells[intRow, 0].Text.Trim() == "I0000001765")
                        {
                            blnData = true;
                            for (i = 3; i < ssVital_Sheet1.ColumnCount; i++)
                            {
                                if (VB.Val(ssVital_Sheet1.Cells[intRow, i].Text.Trim()) == 0)
                                {
                                    chartVital.Series[intSeries].Points.AddY(double.NaN); //Chart1.Series[0].Points[14].IsEmpty = true;
                                    chartVital.Series[intSeries].Points[i - 3].IsEmpty = true;
                                }
                                else
                                {
                                    chartVital.Series[intSeries].Points.AddY(VB.Val(ssVital_Sheet1.Cells[intRow, i].Text.Trim()));
                                }
                            }
                            if (intSeriesE == -1)
                            {
                                intSeriesE = 0;
                            }
                        }
                    }
                    intSeries = intSeries + 1;
                }

                if (chkPR.Checked == true)
                {
                    chartVital.Series.Add("맥박");
                    chartVital.Series[intSeries].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    chartVital.Series[intSeries].BorderWidth = 2;
                    chartVital.Series[intSeries].Color = Color.IndianRed;
                    chartVital.Series[intSeries].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                    chartVital.Series[intSeries].MarkerColor = Color.IndianRed;
                    chartVital.Series[intSeries].MarkerSize = 6;


                    for (intRow = 4; intRow < ssVital_Sheet1.RowCount - 4; intRow++)
                    {
                        if (ssVital_Sheet1.Cells[intRow, 0].Text.Trim() == "I0000014815")
                        {
                            blnData = true;
                            for (i = 3; i < ssVital_Sheet1.ColumnCount; i++)
                            {
                                if (VB.Val(ssVital_Sheet1.Cells[intRow, i].Text.Trim()) == 0)
                                {
                                    chartVital.Series[intSeries].Points.AddY(double.NaN); //Chart1.Series[0].Points[14].IsEmpty = true;
                                    chartVital.Series[intSeries].Points[i - 3].IsEmpty = true;
                                }
                                else
                                {
                                    chartVital.Series[intSeries].Points.AddY(VB.Val(ssVital_Sheet1.Cells[intRow, i].Text.Trim()));
                                }
                            }
                            if (intSeriesE == -1)
                            {
                                intSeriesE = 0;
                            }
                        }
                    }
                    intSeries = intSeries + 1;
                }

                if (chkBT.Checked == true)
                {
                    chartVital.Series.Add("체온");
                    chartVital.Series[intSeries].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    chartVital.Series[intSeries].BorderWidth = 2;
                    chartVital.Series[intSeries].Color = Color.Blue;
                    chartVital.Series[intSeries].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                    chartVital.Series[intSeries].MarkerColor = Color.Blue;
                    chartVital.Series[intSeries].MarkerSize = 6;

                    for (intRow = 4; intRow < ssVital_Sheet1.RowCount - 4; intRow++)
                    {
                        if (ssVital_Sheet1.Cells[intRow, 0].Text.Trim() == "I0000001811")
                        {
                            blnData = true;
                            for (i = 3; i < ssVital_Sheet1.ColumnCount; i++)
                            {
                                if (VB.Val(ssVital_Sheet1.Cells[intRow, i].Text.Trim()) == 0)
                                {
                                    chartVital.Series[intSeries].Points.AddY(double.NaN); //Chart1.Series[0].Points[14].IsEmpty = true;
                                    chartVital.Series[intSeries].Points[i - 3].IsEmpty = true;
                                }
                                else
                                {
                                    chartVital.Series[intSeries].Points.AddY(VB.Val(ssVital_Sheet1.Cells[intRow, i].Text.Trim()));
                                }
                            }
                            if (intSeriesE == -1)
                            {
                                intSeriesE = 0;
                            }
                        }
                    }
                    intSeries = intSeries + 1;
                }

                chartVital.Series.Add("주의선");
                chartVital.Series[intSeries].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chartVital.Series[intSeries].BorderWidth = 2;
                chartVital.Series[intSeries].Color = Color.Orange;
                chartVital.Series[intSeries].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.None;
                
                blnData = true;
                for (i = 3; i < ssVital_Sheet1.ColumnCount; i++)
                {
                    chartVital.Series[intSeries].Points.AddY(100);
                }

                if (intSeriesE == -1)
                {
                    return;
                }

                //if (blnData == false) return;

                int intX = 0;
                for (i = 3; i < ssVital_Sheet1.ColumnCount; i++)
                {
                    chartVital.Series[intSeriesE].Points[intX].AxisLabel = ssVital_Sheet1.Cells[2, i].Text.Trim();
                    intX = intX + 1;
                }

                chartVital.ChartAreas["Default"].AxisX.Interval = 1;
                chartVital.ChartAreas["Default"].AxisY.Interval = 10;
                chartVital.ChartAreas["Default"].AxisY.IntervalOffset = 10;
                chartVital.ChartAreas["Default"].AxisY.Minimum = 30;
                chartVital.ChartAreas["Default"].AxisY.Maximum = 250;
                
                chartVital.Series["주의선"].ChartArea = "Default";
                
                if (chkSBP.Checked == true && chkBT.Checked == true && chkPR.Checked == true)
                {
                    CreateYAxis(chartVital, chartVital.ChartAreas["Default"], chartVital.Series["맥박"], 2, 2);
                    CreateYAxis(chartVital, chartVital.ChartAreas["Default"], chartVital.Series["체온"], 5, 2);
                }
                else if (chkSBP.Checked == false && chkBT.Checked == true && chkPR.Checked == true)
                {
                    CreateYAxis(chartVital, chartVital.ChartAreas["Default"], chartVital.Series["체온"], 5, 2);
                    chartVital.Series["맥박"].ChartArea = "Default";
                    chartVital.ChartAreas["Default"].AxisY.LineColor = Color.IndianRed;
                    chartVital.ChartAreas["Default"].AxisY.InterlacedColor = Color.IndianRed;
                }
                else if (chkSBP.Checked == false && chkBT.Checked == false && chkPR.Checked == true)
                {
                    chartVital.Series["맥박"].ChartArea = "Default";
                    chartVital.ChartAreas["Default"].AxisY.LineColor = Color.IndianRed;
                    chartVital.ChartAreas["Default"].AxisY.InterlacedColor = Color.IndianRed;
                }
                else if (chkSBP.Checked == false && chkBT.Checked == true && chkPR.Checked == false)
                {
                    chartVital.Series["체온"].ChartArea = "Default";
                    chartVital.ChartAreas["Default"].AxisY.LineColor = Color.Blue;
                    chartVital.ChartAreas["Default"].AxisY.InterlacedColor = Color.Blue;
                    chartVital.ChartAreas["Default"].AxisY.Interval = 0.5;
                    chartVital.ChartAreas["Default"].AxisY.IntervalOffset = 0.5;
                    chartVital.ChartAreas["Default"].AxisY.Minimum = 34.0;
                    chartVital.ChartAreas["Default"].AxisY.Maximum = 45.0;
                }
                else if (chkSBP.Checked == true && chkBT.Checked == false && chkPR.Checked == true)
                {
                    CreateYAxis(chartVital, chartVital.ChartAreas["Default"], chartVital.Series["맥박"], 2, 2);
                }
                else if (chkSBP.Checked == true && chkBT.Checked == true && chkPR.Checked == false)
                {
                    CreateYAxis(chartVital, chartVital.ChartAreas["Default"], chartVital.Series["체온"], 5, 2);
 
                }

                chartVital.ChartAreas["Default"].AxisY.LineWidth = 2;

                if (chartVital.ChartAreas.Count > 0)
                {
                    for (int k = 1; k < chartVital.ChartAreas.Count; k++)
                    {
                        if (VB.Split(chartVital.ChartAreas[k].Name,"_")[1] == "맥박")
                        {
                            chartVital.ChartAreas[k].AxisY.Interval = 10;
                            //chartVital.ChartAreas[k].AxisY.IntervalOffset = 10;
                            chartVital.ChartAreas[k].AxisY.Minimum = 30;
                            chartVital.ChartAreas[k].AxisY.Maximum = 250;
                        }
                        else if (VB.Split(chartVital.ChartAreas[k].Name, "_")[1] == "체온")
                        {
                            chartVital.ChartAreas[k].AxisY.Interval = 0.5;
                            //chartVital.ChartAreas[k].AxisY.IntervalOffset = 0.5;
                            chartVital.ChartAreas[k].AxisY.Minimum = 34.0;
                            chartVital.ChartAreas[k].AxisY.Maximum = 45.0;
                        }
                    }
                }

                //=========

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
            }

        }

        public void CreateYAxis(System.Windows.Forms.DataVisualization.Charting.Chart chart, System.Windows.Forms.DataVisualization.Charting.ChartArea area,
            System.Windows.Forms.DataVisualization.Charting.Series series, float axisOffset, float labelsSize)
        {
            // Create new chart area for original series
            System.Windows.Forms.DataVisualization.Charting.ChartArea areaSeries = chart.ChartAreas.Add("ChartArea_" + series.Name);
            areaSeries.BackColor = Color.Transparent;
            areaSeries.BorderColor = Color.Transparent;
            areaSeries.Position.FromRectangleF(area.Position.ToRectangleF());
            areaSeries.InnerPlotPosition.FromRectangleF(area.InnerPlotPosition.ToRectangleF());
            areaSeries.AxisX.MajorGrid.Enabled = false;
            areaSeries.AxisX.MajorTickMark.Enabled = false;
            areaSeries.AxisX.LabelStyle.Enabled = false;
            areaSeries.AxisY.MajorGrid.Enabled = false;
            areaSeries.AxisY.MajorTickMark.Enabled = false;
            areaSeries.AxisY.LabelStyle.Enabled = false;
            areaSeries.AxisY.IsStartedFromZero = area.AxisY.IsStartedFromZero;


            series.ChartArea = areaSeries.Name;

            if (series.Name == "체온")
            {

                // Create new chart area for axis
                System.Windows.Forms.DataVisualization.Charting.ChartArea areaAxis = chart.ChartAreas.Add("AxisY-" + series.ChartArea);
                areaAxis.BackColor = Color.Transparent;
                areaAxis.BorderColor = Color.Transparent;
                areaAxis.Position.FromRectangleF(chart.ChartAreas[series.ChartArea].Position.ToRectangleF());
                areaAxis.InnerPlotPosition.FromRectangleF(chart.ChartAreas[series.ChartArea].InnerPlotPosition.ToRectangleF());

                // Create a copy of specified series
                System.Windows.Forms.DataVisualization.Charting.Series seriesCopy = chart.Series.Add(series.Name + "_Copy");
                seriesCopy.ChartType = series.ChartType;
                foreach (System.Windows.Forms.DataVisualization.Charting.DataPoint point in series.Points)
                {
                    seriesCopy.Points.AddXY(point.XValue, point.YValues[0]);
                }

                // Hide copied series
                seriesCopy.IsVisibleInLegend = false;
                seriesCopy.Color = Color.Transparent;
                seriesCopy.BorderColor = Color.Transparent;
                seriesCopy.ChartArea = areaAxis.Name;

                // Disable drid lines & tickmarks
                areaAxis.AxisX.LineWidth = 0;
                areaAxis.AxisX.MajorGrid.Enabled = false;
                areaAxis.AxisX.MajorTickMark.Enabled = false;
                areaAxis.AxisX.LabelStyle.Enabled = false;
                areaAxis.AxisY.MajorGrid.Enabled = false;
                areaAxis.AxisY.IsStartedFromZero = area.AxisY.IsStartedFromZero;
                areaAxis.AxisY.LabelStyle.Font = area.AxisY.LabelStyle.Font;

                if (series.Name == "체온")
                {
                    areaAxis.AxisY.LineColor = Color.Blue;
                    areaAxis.AxisY.InterlacedColor = Color.Blue;
                }
                else if (series.Name == "맥박")
                {
                    areaAxis.AxisY.LineColor = Color.IndianRed;
                    areaAxis.AxisY.InterlacedColor = Color.IndianRed;
                }
                areaAxis.AxisY.LineWidth = 2;

                // Adjust area position
                areaAxis.Position.X = axisOffset;
                areaAxis.InnerPlotPosition.X = labelsSize;
            }

        }

        private void tabICU_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabICU.SelectedTab == tabIcuView)
            {
                GetVitalGraph();
            }
        }

        private void mbtnSearchOrd_Click(object sender, EventArgs e)
        {
            GetOrderData();
        }

        private void mbtnPainList_Click(object sender, EventArgs e)
        {
            //TOTO return;
            return;
            //frmPainList frm = new frmPainList(p,"통증");
            //frm.TopMost = true;
            //frm.ShowDialog();
        }

        private void mbtnPrint_Click(object sender, EventArgs e)
        {
            FarPoint.Win.Spread.FpSpread ssList = null;
            switch(tabChart.SelectedIndex)
            {
                case 0:
                    ssList = ssVital;
                    break;
                case 1:
                    ssList = ssNuro;
                    break;
                case 2:
                    ssList = ssRR;
                    break;
                case 3:
                    ssList = ssBst;
                    break;
                case 4:
                    ssList = ssAct;
                    break;
                case 5:
                    ssList = ssIO;
                    break;
                //2017-11-10 이준우
                case 6:
                    ssList = ssInfect;
                    break;
                //================
            }

            mbtnPrint.Enabled = false;

            clsSpreadPrint.PrintEmrSpread(clsDB.DbCon, mstrFormNo, mstrUpdateNo, p, true, dtpFrDate.Value.ToString("yyyyMMdd"), dtpFrDate.Value.ToString("yyyyMMdd"),
                                         ssList, "P", 30, 20, 20, 20, false, FarPoint.Win.Spread.PrintOrientation.Landscape, "COL", -1, ssList.ActiveSheet.RowCount - 3, mintTRow, "A");

            mbtnPrint.Enabled = true;
        }

        private void ssInfect_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;

            if (ssInfect_Sheet1.Columns.Count <= 3) return;

            if (e.Column < 3) return;

            for (i = 3; i < ssInfect_Sheet1.Columns.Count; i++)
            {
                ssInfect_Sheet1.Cells[0, i, ssInfect_Sheet1.Rows.Count - 1, i].Font = new Font("굴림", 9, FontStyle.Regular);
            }
            ssInfect_Sheet1.Cells[0, e.Column, ssInfect_Sheet1.Rows.Count - 1, e.Column].Font = new Font("굴림", 9, FontStyle.Bold);
            if (e.Button == MouseButtons.Right)
            {
                ViewItemValue(e.Row, e.Column);
                return;
            }

        }

        private void ssInfect_EditChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column < 3) return;
            if (e.Row < 4) return;
            if (e.Row > ssInfect_Sheet1.RowCount - 4) return;

            ssInfect_Sheet1.Cells[ssInfect_Sheet1.RowCount - 1, e.Column].Text = "Y";
        }

        private void ssInfect_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            //TOTO return;
            return;

            //string StrItemcd = "";
            //string StrDate = "";
            //string StrTime = "";
            //string StrEmrno = "0";

            //string SQL = "";
            //DataTable dt = null;

            //if (e.Column < 3) return;
            //if (e.Row < 4) return;
            //if (e.Row > ssInfect_Sheet1.RowCount - 4) return;

            //StrItemcd = ssInfect_Sheet1.Cells[e.Row, 0].Text.Trim();
            //StrDate = dtpFrDate.Value.ToString("yyyyMMdd");
            //StrTime = ssInfect_Sheet1.Cells[2, e.Column].Text.Trim().Replace(":", "") + "00";

            //StrEmrno = clsEmrQueryEtc.Read_EmrNo(clsDB.DbCon, p.acpNo,StrItemcd, StrDate, StrTime);

            ////체크리스트 FORM 열기
            //clsEmrFunc.gstrInfectSaveYn = "";

            ////유치도뇨관 삽입(I0000031965)의 경우 SUB_SEQ가 더 들어가서 다른 FORM 에서 띠워준다!
            //if (StrItemcd == "I0000031965")
            //{
            //    frmInfect1 frmInfect1X = new frmInfect1(StrEmrno, p.acpNo, StrItemcd, p.ptNo, p.ptName, StrDate, StrTime, p.ward);
            //    frmInfect1X.ShowDialog();

            //    //체크리스트 화면에서 저장버튼을 누른경우만 저장해준다!
            //    if (clsEmrFunc.gstrInfectSaveYn == "Y")
            //    {
            //        //저장한 부분의 항목을 읽어와서 해당칸에 시행수/항목수로 표기해준 후 자동 저장까지 시킨다.
            //        string Str_ActCnt = "0";
            //        string Str_TotCnt = "0";

            //        SQL = " ";
            //        SQL = SQL + ComNum.VBLF + "SELECT SUM(ACTCNT) AS ACTCNT, SUM(TOTCNT) AS TOTCNT ";
            //        SQL = SQL + ComNum.VBLF + "FROM ( ";
            //        SQL = SQL + ComNum.VBLF + "       SELECT SUM(DECODE(ACT_GUBUN,'1',1,0)) AS ACTCNT, COUNT(*) AS TOTCNT ";
            //        SQL = SQL + ComNum.VBLF + "       FROM MED_OCS.EMR_INFECT ";
            //        SQL = SQL + ComNum.VBLF + "       WHERE SEQ NOT IN(SELECT SEQ FROM MED_OCS.EMR_INFECT_SUBLIST ";
            //        SQL = SQL + ComNum.VBLF + "                        WHERE ITEMCD = '" + StrItemcd + "' ";
            //        SQL = SQL + ComNum.VBLF + "                        AND USE_YN = 'Y' ";
            //        SQL = SQL + ComNum.VBLF + "                        GROUP BY SEQ) ";
            //        SQL = SQL + ComNum.VBLF + "       AND ACPNO = '" + p.acpNo + "' ";
            //        if (StrEmrno != "0")
            //        {
            //            SQL = SQL + ComNum.VBLF + "       AND EMRNO = '" + StrEmrno + "' ";
            //        }
            //        SQL = SQL + ComNum.VBLF + "       AND PTNO = '" + p.ptNo + "' ";
            //        SQL = SQL + ComNum.VBLF + "       AND CHARTDATE = '" + StrDate + "' ";
            //        SQL = SQL + ComNum.VBLF + "       AND CHARTTIME = '" + StrTime + "' ";
            //        SQL = SQL + ComNum.VBLF + "       AND ITEMCD = '" + StrItemcd + "' ";
            //        SQL = SQL + ComNum.VBLF + "       UNION ALL ";
            //        SQL = SQL + ComNum.VBLF + "       SELECT SUM(ACTCNT) AS ACTCNT, COUNT(*) AS TOTCNT ";
            //        SQL = SQL + ComNum.VBLF + "       FROM ( ";
            //        SQL = SQL + ComNum.VBLF + "             SELECT SEQ, SUM(ACTCNT) AS ACTCNT ";
            //        SQL = SQL + ComNum.VBLF + "             FROM ( ";
            //        SQL = SQL + ComNum.VBLF + "                   SELECT SEQ, DECODE(ACT_GUBUN,'1',1,0) AS ACTCNT FROM MED_OCS.EMR_INFECT ";
            //        SQL = SQL + ComNum.VBLF + "                   WHERE SEQ IN (SELECT SEQ FROM MED_OCS.EMR_INFECT_SUBLIST ";
            //        SQL = SQL + ComNum.VBLF + "                                 WHERE ITEMCD = '" + StrItemcd + "' ";
            //        SQL = SQL + ComNum.VBLF + "                                 AND USE_YN = 'Y' ";
            //        SQL = SQL + ComNum.VBLF + "                                 GROUP BY SEQ) ";
            //        SQL = SQL + ComNum.VBLF + "                   AND ACPNO = '" + p.acpNo + "' ";
            //        if (StrEmrno != "0")
            //        {
            //            SQL = SQL + ComNum.VBLF + "                   AND EMRNO = '" + StrEmrno + "' ";
            //        }
            //        SQL = SQL + ComNum.VBLF + "                   AND PTNO = '" + p.ptNo + "' ";
            //        SQL = SQL + ComNum.VBLF + "                   AND CHARTDATE = '" + StrDate + "' ";
            //        SQL = SQL + ComNum.VBLF + "                   AND CHARTTIME = '" + StrTime + "' ";
            //        SQL = SQL + ComNum.VBLF + "                   AND ITEMCD = '" + StrItemcd + "' ";
            //        SQL = SQL + ComNum.VBLF + "                   GROUP BY SEQ, DECODE(ACT_GUBUN,'1',1,0) )  ";
            //        SQL = SQL + ComNum.VBLF + "              GROUP BY SEQ) ";
            //        SQL = SQL + ComNum.VBLF + "      )";
                   
            //        Cursor.Current = Cursors.WaitCursor;

            //        dt = clsDB.GetDataTable(SQL);

            //        if (dt == null)
            //        {
            //            MessageBox.Show(new Form() { TopMost = true }, "조회중 문제가 발생했습니다");
            //            Cursor.Current = Cursors.Default;
            //            return;
            //        }

            //        if (dt.Rows.Count == 0)
            //        {
            //            dt.Dispose();
            //            dt = null;
            //            Cursor.Current = Cursors.Default;
            //        }
            //        else
            //        {
            //            Str_ActCnt = dt.Rows[0]["ACTCNT"].ToString().Trim();
            //            Str_TotCnt = dt.Rows[0]["TOTCNT"].ToString().Trim();

            //            dt.Dispose();
            //            dt = null;
            //            Cursor.Current = Cursors.Default;
            //        }

            //        ssInfect_Sheet1.Cells[e.Row, e.Column].Text = Str_ActCnt + "/" + Str_TotCnt;

            //        ssInfect_Sheet1.Cells[ssInfect_Sheet1.RowCount - 1, e.Column].Text = "Y";

            //        //저장루틴---------------------------
            //        pSaveData("1"); //인증저장
            //        SetSpread();
            //        SetDefaultData(mSpdView, mJOBGB);
            //        LoadData(mSpdView, mJOBGB);

            //        clsEmrFunc.gstrInfectSaveYn = "";

            //    }

            //}
            //else
            //{
            //    frmInfect frmInfectX = new frmInfect(StrEmrno, p.acpNo, StrItemcd, p.ptNo, p.ptName, StrDate, StrTime, p.ward);
            //    frmInfectX.ShowDialog();

            //    //체크리스트 화면에서 저장버튼을 누른경우만 저장해준다!
            //    if (clsEmrFunc.gstrInfectSaveYn == "Y")
            //    {
            //        //저장한 부분의 항목을 읽어와서 해당칸에 시행수/항목수로 표기해준 후 자동 저장까지 시킨다.
            //        string Str_ActCnt = "0";
            //        string Str_TotCnt = "0";

            //        SQL = " ";
            //        SQL = SQL + ComNum.VBLF + " SELECT SUM(DECODE(ACT_GUBUN,'1',1,0)) AS ACTCNT, COUNT(*) AS TOTCNT ";
            //        SQL = SQL + ComNum.VBLF + " FROM MED_OCS.EMR_INFECT ";
            //        SQL = SQL + ComNum.VBLF + " WHERE ACPNO = '" + p.acpNo + "' ";
            //        if (StrEmrno != "0")
            //        {
            //            SQL = SQL + ComNum.VBLF + " AND EMRNO = '" + StrEmrno + "' ";
            //        }
            //        SQL = SQL + ComNum.VBLF + " AND PTNO = '" + p.ptNo + "' ";
            //        SQL = SQL + ComNum.VBLF + " AND CHARTDATE = '" + StrDate + "' ";
            //        SQL = SQL + ComNum.VBLF + " AND CHARTTIME = '" + StrTime + "' ";
            //        SQL = SQL + ComNum.VBLF + " AND ITEMCD = '" + StrItemcd + "' ";

            //        Cursor.Current = Cursors.WaitCursor;

            //        dt = clsDB.GetDataTable(SQL);

            //        if (dt == null)
            //        {
            //            MessageBox.Show(new Form() { TopMost = true }, "조회중 문제가 발생했습니다");
            //            Cursor.Current = Cursors.Default;
            //            return;
            //        }

            //        if (dt.Rows.Count == 0)
            //        {
            //            dt.Dispose();
            //            dt = null;
            //            Cursor.Current = Cursors.Default;
            //        }
            //        else
            //        {
            //            Str_ActCnt = dt.Rows[0]["ACTCNT"].ToString().Trim();
            //            Str_TotCnt = dt.Rows[0]["TOTCNT"].ToString().Trim();

            //            dt.Dispose();
            //            dt = null;
            //            Cursor.Current = Cursors.Default;
            //        }

            //        ssInfect_Sheet1.Cells[e.Row, e.Column].Text = Str_ActCnt + "/" + Str_TotCnt;

            //        ssInfect_Sheet1.Cells[ssInfect_Sheet1.RowCount - 1, e.Column].Text = "Y";

            //        //저장루틴---------------------------
            //        pSaveData("1"); //인증저장
            //        SetSpread();
            //        SetDefaultData(mSpdView, mJOBGB);
            //        LoadData(mSpdView, mJOBGB);

            //        clsEmrFunc.gstrInfectSaveYn = "";

            //    }

            //}

        }

    }
}
