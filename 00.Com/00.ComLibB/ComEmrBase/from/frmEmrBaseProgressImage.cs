using ComBase;
using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrBaseProgressImage : Form, EmrChartForm
    {
        string mEmrImageNo = "0"; //Image
        string mstrEmrNo = "0"; //Progress 
        string mstrFormNo = "1232";
        string mstrUpdateNo = "1";
        FormEmrMessage mEmrCallForm = null;
        EmrPatient AcpEmr = null;

        string EmrUrlMain = "http://192.168.100.33:8090/Emr/MtsEmrSite.mts";
        string gEmrUrl = "http://192.168.100.33:8090/Emr";
        string EmrUrlImage = "http://192.168.100.33:8090/Emr/progressImageEditor.mts?formNo=1232";
        string EmrUrlPatSend = "http://192.168.100.33:8090/Emr/emrxmlInfo.mts?";

        string mstrUserChoJinForm = string.Empty;
        string mstrUserChoJinFormName = string.Empty;

        #region //EmrChartForm
        public double SaveDataMsg(string strFlag)
        {
            double dblEmrNo = 0;
            return dblEmrNo;
        }

        public bool DelDataMsg()
        {
            bool rtnVal = false;
            //rtnVal = pDelData();
            return rtnVal;
        }

        public void ClearFormMsg()
        {
            mstrEmrNo = "0";
            ClearForm();
        }

        public void SetUserFormMsg(double dblMACRONO)
        {
            //pSetUserForm(dblMACRONO);
        }

        public bool SaveUserFormMsg(double dblMACRONO)
        {
            bool rtnVal = false;
            //rtnVal = pSaveUserForm(dblMACRONO);
            return rtnVal;
        }

        public void SetChartHisMsg(string strEmrNo, string strOldGb)
        {

        }

        public int PrintFormMsg(string strPRINTFLAG)
        {
            int rtnVal = 0;
            //if (strPRINTFLAG == "N")
            //{
            //    frmEmrPrintOption frmEmrPrintOptionX = new frmEmrPrintOption();
            //    frmEmrPrintOptionX.ShowDialog();
            //}

            //if (clsFormPrint.mstrPRINTFLAG == "-1")
            //{
            //    return rtnVal;
            //}

            return rtnVal;
        }

        public int ToImageFormMsg(string strPRINTFLAG)
        {
            int rtnVal = 0;
            //rtnVal = clsFormPrint.PrintToTifFileLong(clsDB.DbCon, mstrFormNo, mstrUpdateNo, AcpEmr, mstrEmrNo, panChart, "C");
            return rtnVal;
        }


        #endregion

        public frmEmrBaseProgressImage()
        {
            InitializeComponent();
        }

        public frmEmrBaseProgressImage(EmrPatient pAcpEmr, FormEmrMessage pEmrCallForm)
        {
            InitializeComponent();
            AcpEmr = pAcpEmr;
            mEmrCallForm = pEmrCallForm;
        }

        public frmEmrBaseProgressImage(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm)
        {
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            AcpEmr = po;
            mstrEmrNo = strEmrNo;
            mEmrCallForm = pEmrCallForm;
            InitializeComponent();
        }

        #region //Public Function
        /// <summary>
        /// 사용자 정보가 바뀐 경우 : 사용자별 환경 설정을 다시 한다
        /// </summary>
        public void SetUserInfo()
        {
            ClearForm();
            SetUserOption();
        }

        /// <summary>
        /// 환자정보가 바뀔 경우 : 환자 정보를 갱신한다
        /// </summary>
        /// <param name="AcpEmr"></param>
        public void SetPatInfo(EmrPatient pAcpEmr)
        {
            if (AcpEmr != null && pAcpEmr.ptNo == AcpEmr.ptNo)
                return;

            AcpEmr = pAcpEmr;
            ClearForm();

            //if (AcpEmr.inOutCls == "I")
            //{
            //    optInOut2.Checked = true;
            //}
            //else
            //{
            //    optInOut1.Checked = true;
            //}

            webImage.Navigate(EmrUrlMain);
            DateTime dtp = DateTime.Now;

            while (webImage.ReadyState != WebBrowserReadyState.Complete)
            {
                if ((DateTime.Now - dtp).TotalMilliseconds >= 60000)
                {
                    Log.Debug("webImage, {} ", "1분경과 : " + "webImage, {}", EmrUrlMain);
                    break;
                }
                Application.DoEvents();
            }
            Application.DoEvents();

            SetPatInfoImg();
        }


        public void ClearPatInfo()
        {
            AcpEmr = null;
            ClearColtrol();
        }

        private void ClearColtrol()
        {
            try
            {
                mEmrImageNo = "0";

                btnSaveImag.Visible = true;

                dtpChartDate.Enabled = true;
                txtChartTime.Enabled = true;

                //webEMR.Navigate(EmrUrlMain);
                //Application.DoEvents();

                //webImage.Navigate(EmrUrlMain);
                //Application.DoEvents();

            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                ComFunc.MsgBoxEx(this, ex.Message);
            }

        }


        private void ClearEnd()
        {
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");

            dtpChartDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(VB.Left(strCurDateTime, 8), "D"));
            txtChartTime.Text = ComFunc.FormatStrToDate(VB.Right(strCurDateTime, 6), "M");
            mEmrImageNo = "0";

            ClearColtrol();
        }
        #endregion //Public Function

        #region //Private Function

        private void WebLogin()
        {

            //clsType.User.Passhash256 = "5857b0bd2c33cea36a79dba3e1f17e225e0dad5976b53a7f4b790e9694bcd028";

            string strURL = "";
            string strUseId = clsType.User.IdNumber;
            string strPw = clsType.User.Passhash256;

            //webImage.Navigate("http://192.168.100.33:8090/Emr/emrxmlInfo.mts?ptNo=&acpNo=&inOutCls=&medFrDate=&medFrTime=&medEndDate=&medEndTime=&medDeptCd=&medDeptName=&medDrCd=&gubun=3&formNo=");
            //while (webImage.IsBusy == true)
            //{
            //    Application.DoEvents();
            //}

            //webEMR.Navigate("http://192.168.100.33:8090/Emr/emrxmlInfo.mts?ptNo=&acpNo=&inOutCls=&medFrDate=&medFrTime=&medEndDate=&medEndTime=&medDeptCd=&medDeptName=&medDrCd=&gubun=3&formNo=");
            //while (webImage.IsBusy == true)
            //{
            //    Application.DoEvents();
            //}

            try
            {
                //---------------------------------------------
                // 2019-01-22 테스트로 변경함
                //---------------------------------------------
                //for (int intWeb = 0; intWeb < 40000; intWeb++)
                //{
                //    Application.DoEvents();
                //}
                //ComFunc.Delay(1000);
                //---------------------------------------------


                ////http://192.168.100.33:8090/Emr/doLogin.mts?useId=15200&password=5857b0bd2c33cea36a79dba3e1f17e225e0dad5976b53a7f4b790e9694bcd028&loginType=vb
                //webImage.Navigate("http://192.168.100.33:8090/Emr/doLogin.mts?useId=15200&password=5857b0bd2c33cea36a79dba3e1f17e225e0dad5976b53a7f4b790e9694bcd028&loginType=vb");

                strURL = gEmrUrl + "/doLogin.mts?useId=" + strUseId + "&password=" + strPw + "&loginType=vb";
                webImage.Navigate(strURL);
                DateTime dtp = DateTime.Now;

                while (webImage.ReadyState != WebBrowserReadyState.Complete)
                {
                    if ((DateTime.Now - dtp).TotalMilliseconds >= 60000)
                    {
                        Log.Debug("webImage, {}", "1분경과 : " + strURL);
                        break;
                    }
                    Application.DoEvents();
                }

                //---------------------------------------------
                // 2019-01-22 테스트로 변경함
                //---------------------------------------------
                //for (int intWeb = 0; intWeb < 40000; intWeb++)
                //{
                //    Application.DoEvents();
                //}
                //ComFunc.Delay(1000);
                //---------------------------------------------
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                ComFunc.MsgBoxEx(this, ex.Message);
                //clsDB.SaveSqlErrLog(ex.Message, "", clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private void SetPatInfoImg()
        {
            if (AcpEmr == null)
            {
                Log.Warn("AcpEmr is null");
                return;
            }

            string strURL = "";

            string strMedEndDate = "";
            string strMedEndTime = "";

            try
            {
                if (AcpEmr.inOutCls == "I")
                {
                    strMedEndDate = AcpEmr.medEndDate;
                    strMedEndTime = AcpEmr.medEndTime;
                }


                //http://192.168.100.33:8090/Emr/emrxmlInfo.mts?ptNo=03983614&acpNo=0&inOutCls=O&medFrDate=20170803&medFrTime=114500&medEndDate=&medEndTime=&medDeptCd=MN&medDeptName=&medDrCd=0503&gubun=3&formNo=
                //http://192.168.100.33:8090/Emr/emrxmlInfo.mts?ptNo=03983614&acpNo=0&inOutCls=O&medFrDate=20170803&medFrTime=114500&medEndDate=&medEndTime=&medDeptCd=MN&medDeptName=&medDrCd=0503&gubun=3&formNo=
                strURL = EmrUrlPatSend +
                       "ptNo=" + AcpEmr.ptNo +
                       "&acpNo=" + AcpEmr.acpNo +
                       "&inOutCls=" + AcpEmr.inOutCls +
                       "&medFrDate=" + AcpEmr.medFrDate +
                       "&medFrTime=" + AcpEmr.medFrTime +
                       "&medEndDate=" + strMedEndDate +
                       "&medEndTime=" + strMedEndTime +
                       "&medDeptCd=" + AcpEmr.medDeptCd +
                       "&medDeptName=" + "" +
                       "&medDrCd=" + AcpEmr.medDrCd +
                       "&gubun=" + "3" +
                       "&formNo=";
                //strURL = "http://192.168.100.33:8090/Emr/emrxmlInfo.mts?ptNo=02487371&acpNo=0&inOutCls=O&medFrDate=20180226&medFrTime=141800&medEndDate=&medEndTime=&medDeptCd=GS&medDeptName=&medDrCd=2121&gubun=3&formNo=";
                webImage.Navigate(strURL); //한장씩 볼 경우
                DateTime dtp = DateTime.Now;

                while (webImage.ReadyState != WebBrowserReadyState.Complete)
                {
                    if ((DateTime.Now - dtp).TotalMilliseconds >= 60000)
                    {
                        Log.Debug("webImage, {}", "1분경과 : " + EmrUrlMain);
                        break;
                    }
                    Application.DoEvents();
                }

                //---------------------------------------------
                // 2019-01-22 테스트로 변경함
                //---------------------------------------------
                //for (int intWeb = 0; intWeb < 40000; intWeb++)
                //{
                //    Application.DoEvents();
                //}
                ComFunc.Delay(1000);
                //---------------------------------------------
                webImage.Navigate(EmrUrlImage);
                dtp = DateTime.Now;

                while (webImage.ReadyState != WebBrowserReadyState.Complete)
                {
                    if ((DateTime.Now - dtp).TotalMilliseconds >= 60000)
                    {
                        Log.Debug("webImage, {}", "1분경과 : " + strURL);
                        break;
                    }
                    Application.DoEvents();
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                //ComFunc.MsgBoxEx(this, ex.Message);
                //clsDB.SaveSqlErrLog(ex.Message, "", clsDB.DbCon); //에러로그 저장
                return;
            }
        }


        private void ReadFirstDate(ref string strFirstDATE, string arg = "")
        {

            if (AcpEmr == null)
                return;

            if (AcpEmr.inOutCls == "I")
                return;
            if (AcpEmr.medDeptCd == "HD")
                return;

            string strDeptCode = "";

            if (arg != "")
            {
                strDeptCode = "'" + VB.Replace(arg, "^", "','");
                if (VB.Right(arg, 1) == "^")
                {
                    strDeptCode = VB.Mid(strDeptCode, 1, VB.Len(strDeptCode) - 2);
                }
                else
                {
                    strDeptCode = VB.Mid(strDeptCode, 1, VB.Len(strDeptCode)) + "'";
                }
            }

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = " SELECT TO_CHAR(MIN(DDATE),'YYYY-MM-DD') MINDATE FROM";
            SQL = SQL + ComNum.VBLF + " (SELECT MIN(INDATE) DDATE";
            SQL = SQL + ComNum.VBLF + " From KOSMOS_PMPA.IPD_NEW_MASTER";
            SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + AcpEmr.ptNo + " '";
            if (arg == "")
            {
                switch (AcpEmr.medDeptCd)
                {
                    case "MC":
                    case "MD":
                    case "ME":
                    case "MG":
                    case "MI":
                    case "MN":
                    case "MP":
                    case "MR":
                        SQL = SQL + ComNum.VBLF + "   AND DEPTCODE IN ('MD', '" + AcpEmr.medDeptCd + "' ) ";
                        break;
                    default:
                        SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + AcpEmr.medDeptCd + "' ";
                        break;
                }
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE IN (" + strDeptCode + ") ";
            }
            SQL = SQL + ComNum.VBLF + " Union All";
            SQL = SQL + ComNum.VBLF + " SELECT MIN(ACTDATE) DDATE FROM KOSMOS_PMPA.OPD_MASTER";
            SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + AcpEmr.ptNo + "' ";
            if (arg == "")
            {
                switch (AcpEmr.medDeptCd)
                {
                    case "MC":
                    case "MD":
                    case "ME":
                    case "MG":
                    case "MI":
                    case "MN":
                    case "MP":
                    case "MR":
                        SQL = SQL + ComNum.VBLF + "   AND DEPTCODE IN ('MD', '" + AcpEmr.medDeptCd + "' )) ";
                        break;
                    default:
                        SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + AcpEmr.medDeptCd + "') ";
                        break;
                }
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE IN (" + strDeptCode + ") )";
            }

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count > 0)
            {

            }
            strFirstDATE = dt.Rows[0]["MINDATE"].ToString().Trim();
            dt.Dispose();
            dt = null;

        }


        private void ClearForm()
        {
            ClearNew();
        }

        private void ClearNew()
        {
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");

            dtpChartDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(VB.Left(strCurDateTime, 8), "D"));
            txtChartTime.Text = ComFunc.FormatStrToDate(VB.Right(strCurDateTime, 6), "M");
            mEmrImageNo = "0";

            ClearColtrol();

            if (AcpEmr != null)
            {
                if (AcpEmr.medFrDate != "")
                {
                    if (AcpEmr.inOutCls == "O")
                    {
                        dtpChartDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(AcpEmr.medFrDate, "D"));
                    }
                    else
                    {
                        dtpChartDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(VB.Left(strCurDateTime, 8), "D"));
                    }
                }
                else
                {
                    dtpChartDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(VB.Left(strCurDateTime, 8), "D"));
                }

                //SetPatInfoImg();
                //GetChartHis();
            }
        }


        private void SetUserOption()
        {
            WebLogin();
        }

        #endregion //Private Function

        private void FrmEmrBaseProgressImage_Load(object sender, EventArgs e)
        {
            webImage.Dock = DockStyle.Fill;
            webImage.BringToFront();

            DateTime dtp;
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");

            dtpChartDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(VB.Left(strCurDateTime, 8), "D"));
            txtChartTime.Text = ComFunc.FormatStrToDate(VB.Right(strCurDateTime, 6), "M");

            ClearForm();

            SetUserOption();

            webImage.Navigate(EmrUrlMain);
            dtp = DateTime.Now;
            while (webImage.ReadyState != WebBrowserReadyState.Complete)
            {
                if ((DateTime.Now - dtp).TotalMilliseconds >= 60000)
                {
                    Log.Debug("webImage, {}", "1분경과 : " + EmrUrlMain);
                    break;
                }
                Application.DoEvents();
            }
            Application.DoEvents();

            if (AcpEmr != null)
            {
                SetPatInfoImg();
            }
        }

        private void btnSaveImag_Click(object sender, EventArgs e)
        {
            if (clsType.User.DrCode == "")
            {
                ComFunc.MsgBoxEx(this, "의사만 작성이 가능합니다.");
                return;
            }
            if (ComFunc.CheckTime(txtChartTime.Text.Trim()) == false)
            {
                ComFunc.MsgBoxEx(this, "작성시간 오류입니다.");
                return;
            }

            if (SaveDataImageWeb() == false)
            {
                return;
            }
            //for (int intWeb = 0; intWeb < 400000; intWeb++)
            //{
            //    Application.DoEvents();
            //    Application.DoEvents();
            //    Application.DoEvents();
            //    Application.DoEvents();
            //    Application.DoEvents();
            //    Application.DoEvents();
            //}

            //---------------------------------------------
            // 2019-01-22 테스트로 변경함
            //---------------------------------------------

            //ComFunc.Delay(1000);

            //---------------------------------------------
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            string strCurDate = VB.Left(strCurDateTime, 8);
            string strCurTime = VB.Right(strCurDateTime, 6);
            //for (int intWeb = 0; intWeb < 400000; intWeb++)
            //{
            //    Application.DoEvents();
            //    Application.DoEvents();
            //    Application.DoEvents();
            //    Application.DoEvents();
            //    Application.DoEvents();
            //    Application.DoEvents();
            //}

            //---------------------------------------------
            // 2019-01-22 테스트로 변경함
            //---------------------------------------------

            //ComFunc.Delay(1000);

            //---------------------------------------------
            if (VB.Val(mEmrImageNo) == 0)
            {
                SaveDataImage(VB.Val(mEmrImageNo));
            }

            mEmrImageNo = "";
        }

        private bool SaveDataImageWeb()
        {
            try
            {
                string strChartTime = txtChartTime.Text.Replace(":", "");
                if (strChartTime.Length < 6)
                {
                    strChartTime = strChartTime + "00";
                }
                webImage.Document.GetElementById("chartDate").SetAttribute("value", dtpChartDate.Value.ToString("yyyyMMdd"));
                webImage.Document.GetElementById("chartTime").SetAttribute("value", strChartTime);

                string strURL = "javascript:doSave()";
                webImage.Navigate(strURL);
                DateTime dtp = DateTime.Now;

                ComFunc.Delay(1000);

                //---------------------------------------------

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool SaveDataImage(double pEmrNo)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strEmrNo = "";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                string strCurDate = VB.Left(strCurDateTime, 8);
                string strCurTime = VB.Right(strCurDateTime, 6);

                SQL = " SELECT EMRNO FROM KOSMOS_EMR.EMRXMLIMAGES";
                SQL = SQL + ComNum.VBLF + " WHERE WRITEDATE = '" + strCurDate + "'";
                SQL = SQL + ComNum.VBLF + " AND USEID = '" + clsType.User.IdNumber + "'";
                SQL = SQL + ComNum.VBLF + " AND PTNO = '" + AcpEmr.ptNo + "'";
                SQL = SQL + ComNum.VBLF + " ORDER BY WRITEDATE DESC, WRITETIME DESC";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                strEmrNo = dt.Rows[0]["EMRNO"].ToString().Trim();
                dt.Dispose();
                dt = null;

                SQL = " INSERT INTO KOSMOS_EMR.EMRXMLMST";
                SQL = SQL + ComNum.VBLF + " SELECT EMRNO, PTNO, '1', FORMNO, USEID,";
                SQL = SQL + ComNum.VBLF + " CHARTDATE, CHARTTIME, INOUTCLS, MEDFRDATE,";
                SQL = SQL + ComNum.VBLF + " MedFrTime , MedEndDate, MedEndTime, MedDeptCd, MedDrCd, writeDate, writeTime";
                SQL = SQL + ComNum.VBLF + "  From KOSMOS_EMR.EMRXML";
                SQL = SQL + ComNum.VBLF + " Where EMRNO = " + strEmrNo;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                //ComFunc.MsgBoxEx(this, "저장하였습니다.");
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                Log.Error(ex.ToString());
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }
    }
}
