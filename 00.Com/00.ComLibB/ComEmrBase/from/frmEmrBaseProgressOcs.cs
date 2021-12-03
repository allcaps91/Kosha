using ComBase;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace ComEmrBase
{
    public partial class frmEmrBaseProgressOcs : Form
    {
        //string mInOutGb = "";
        string mstrPROGNO = "";
        //string mstrPROGNAME = "";
        //string mstrPROGIMGNO = "";
        //string mstrPROGIMGNAME = "";
        string mstrUserChoJinForm = "";
        string mstrUserChoJinFormName = "";
        string mstrUserDept = "";
        //string mOption = "";
        string mSYSMPGB = "";
        //string mEDIT = "";  //fstrEDIT
        //string mClick = ""; //FstrClick
        bool mSaveFlag = false; //외부에서 저장을 요청한경우
        bool mIsFirtQuery = false; //초진차트 작성 두번 보이는 것 안보이게
        bool IsLoadForm = true;

        string mEmrNo = "0"; //Progress 
        string mEmrImageNo = "0"; //Image

        //Form CallForm = null;
        frmEmrBaseSympOld fEmrMacro = null;
        frmEmrBaseEmrChartOld fEmrChart = null;

        EmrPatient AcpEmr = null;

        string EmrUrlMain = "http://192.168.100.33:8090/Emr/MtsEmrSite.mts";
        string gEmrUrl = "http://192.168.100.33:8090/Emr";
        string EmrUrlImage = "http://192.168.100.33:8090/Emr/progressImageEditor.mts?formNo=1232";
        string EmrUrlPatSend = "http://192.168.100.33:8090/Emr/emrxmlInfo.mts?";

        public frmEmrBaseProgressOcs()
        {
            InitializeComponent();
        }

        public frmEmrBaseProgressOcs(EmrPatient pAcpEmr)
        {
            InitializeComponent();
            AcpEmr = pAcpEmr;
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
            AcpEmr = pAcpEmr;
            mIsFirtQuery = true;
            ClearForm();
            ChkFrDate();

            //if (AcpEmr.inOutCls == "I")
            //{
            //    optInOut2.Checked = true;
            //}
            //else
            //{
            //    optInOut1.Checked = true;
            //}
            webEMR.Navigate(EmrUrlMain);
            DateTime dtp = DateTime.Now;

            while (webEMR.ReadyState != WebBrowserReadyState.Complete)
            {
                if ((DateTime.Now - dtp).TotalMilliseconds >= 60000)
                {
                    Log.Debug("webEMR, {} ", "1분경과 : " + "webEMR, {}", EmrUrlMain);
                    break;
                }
                Application.DoEvents();
            }
            Application.DoEvents();

            webImage.Navigate(EmrUrlMain);
            dtp = DateTime.Now;

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

            GetPatRmk();
            GetMibi();
            SetPatInfoImg();
            GetChartHis();

            string strEmrOption = "";
            strEmrOption = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "EMRKORENG");
            if (VB.Val(strEmrOption) == 1)
            {
                ComFunc.SetIMEMODE(this, "E");
                lblKorE.Text = "영어";
            }
            else
            {
                ComFunc.SetIMEMODE(this, "K");
                lblKorE.Text = "한글";
            }

            strEmrOption = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "AFTERPROGSAVE");
            if (VB.Val(strEmrOption) == 1)
            {
                tabEmr.SelectedTab = tabEmrWrite;
            }
            else
            {
                tabEmr.SelectedTab = tabEmrView;
            }

            //환자의 이전 프로그래스 내역 불러오기
            if (AcpEmr != null)
            {
                if (AcpEmr.inOutCls == "O" && AcpEmr.medDeptCd != "ER")
                {
                    strEmrOption = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "PROGGETHIS");
                    if (VB.Val(strEmrOption) == 1)
                    {
                        GetSetProgHis();
                    }
                }
            }

            clsApi.FlushMemoryEx();
        }

        private void GetSetProgHis()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strEMRNO = "0";
            string strCHARTA = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT  ";
                SQL = SQL + ComNum.VBLF + "    MAX(M1.EMRNO) AS EMRNO  ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMRXMLMST M1 ";
                SQL = SQL + ComNum.VBLF + "WHERE M1.PTNO = '" + AcpEmr.ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "    AND M1.MEDFRDATE = '" + AcpEmr.medFrDate.Replace("-", "") + "' ";
                SQL = SQL + ComNum.VBLF + "    AND M1.MEDDEPTCD = '" + AcpEmr.medDeptCd + "' ";
                SQL = SQL + ComNum.VBLF + "    AND M1.INOUTCLS = 'O' ";
                SQL = SQL + ComNum.VBLF + "    AND M1.FORMNO = 963 ";
                SQL = SQL + ComNum.VBLF + "    AND M1.USEID = '" + clsType.User.IdNumber + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strEMRNO = dt.Rows[0]["EMRNO"].ToString().Trim();

                    dt.Dispose();
                    dt = null;
                    if (VB.Val(strEMRNO) > 0)
                    {
                        Cursor.Current = Cursors.Default;
                        SetProgressOne(strEMRNO);
                        return;
                    }
                }
                else
                {
                    dt.Dispose();
                    dt = null;
                }

                SQL = "";
                SQL = " SELECT ";
                SQL = SQL + ComNum.VBLF + "    CHARTXML AS CHARTA ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMRXML ";
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO = (  ";
                SQL = SQL + ComNum.VBLF + "            SELECT  ";
                SQL = SQL + ComNum.VBLF + "                MAX(M1.EMRNO)  ";
                SQL = SQL + ComNum.VBLF + "            FROM " + ComNum.DB_EMR + "EMRXMLMST M1 ";
                SQL = SQL + ComNum.VBLF + "            WHERE M1.PTNO = '" + AcpEmr.ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "                AND M1.MEDFRDATE < '" + AcpEmr.medFrDate.Replace("-", "") + "' ";
                SQL = SQL + ComNum.VBLF + "                AND M1.MEDDEPTCD = '" + AcpEmr.medDeptCd + "' ";
                SQL = SQL + ComNum.VBLF + "                AND M1.INOUTCLS = 'O' ";
                SQL = SQL + ComNum.VBLF + "                AND M1.FORMNO = 963 ";
                SQL = SQL + ComNum.VBLF + "                AND M1.CHARTDATE = (SELECT MAX(M1.CHARTDATE)  ";
                SQL = SQL + ComNum.VBLF + "                                        FROM " + ComNum.DB_EMR + "EMRXMLMST M1 ";
                SQL = SQL + ComNum.VBLF + "                                        WHERE M1.PTNO = '" + AcpEmr.ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "                                            AND M1.MEDFRDATE < '" + AcpEmr.medFrDate.Replace("-", "") + "' ";
                SQL = SQL + ComNum.VBLF + "                                            AND M1.MEDDEPTCD = '" + AcpEmr.medDeptCd + "' ";
                SQL = SQL + ComNum.VBLF + "                                            AND M1.INOUTCLS = 'O' ";
                SQL = SQL + ComNum.VBLF + "                                            AND M1.FORMNO = 963) ";
                SQL = SQL + ComNum.VBLF + "                ) ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

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
                strCHARTA = MakeContentValue(dt.Rows[0]["CHARTA"].ToString().Trim());
                dt.Dispose();
                dt = null;

                txtProgress.Text = strCHARTA.Replace("\r\n", "\n").Replace("\n", "\r\n");

                tabEmr.SelectedTab = tabEmrWrite;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        public void ClearPatInfo()
        {
            AcpEmr = null;
            ClearColtrol();

            if (fEmrMacro != null)
            {
                fEmrMacro.Dispose();
                fEmrMacro = null;
            }
            if (fEmrChart != null)
            {
                fEmrChart.Dispose();
                fEmrChart = null;
            }
        }

        private void ClearColtrol()
        {
            try
            {
                //mEDIT = "";
                mEmrImageNo = "0";

                chkSOAP0.Checked = false;
                chkSOAP1.Checked = false;

                mbtnDelete.Visible = true;
                mbtnSave.Visible = true;
                mbtnSaveImag.Visible = true;
                btnMibi.Visible = false;

                dtpChartDate.Enabled = true;
                txtChartTime.Enabled = true;
                txtEmrNo.Enabled = false;

                txtEmrNo.Text = "";
                txtProgress.Text = "";
                txtProgress.Tag = null;

                panProg.Height = 150;

                btnSearchRmk.BackColor = Color.White;
                imgRmk.Visible = false;
                imgRmk.Tag = null;
                toolRmk.Tag = null;

                //webEMR.Navigate(EmrUrlMain);
                //Application.DoEvents();

                //webImage.Navigate(EmrUrlMain);
                //Application.DoEvents();

                //저장후 SOAP 클리어
                string strEmrOption = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "PROGSOAPCLEAR");
                if (VB.Val(strEmrOption) == 1)
                {
                    ssSOAP_Sheet1.Cells[0, 1].Text = "";
                    ssSOAP_Sheet1.Cells[1, 1].Text = "";
                    ssSOAP_Sheet1.Cells[2, 1].Text = "";
                    ssSOAP_Sheet1.Cells[3, 1].Text = "";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                ComFunc.MsgBoxEx(this, ex.Message);
            }

        }

        /// <summary>
        /// 기록지를 저장한다.
        /// </summary>
        public void SetSaveData()
        {
            mSaveFlag = true;

            if (SaveData() == true)
            {

            }
        }

        public bool CheckAndSaveEmr()
        {
            bool rtnVal = true;

            bool isChange = false;

            if (fEmrMacro != null)
            {
                fEmrMacro.Dispose();
                fEmrMacro = null;
            }

            //수정 권한이 없으면 빠져나간다
            if (mbtnSave.Visible == false)
            {
                ClearEnd();
                return rtnVal;
            }

            if (VB.Val(txtEmrNo.Text) != 0)
            {
                if (txtProgress.Tag == null)
                {
                    if (txtProgress.Text.Trim() != "")
                    {
                        isChange = true;
                    }
                }
                else
                {
                    if (txtProgress.Text.Trim() != txtProgress.Tag.ToString().Trim())
                    {
                        isChange = true;
                    }
                }
            }
            else
            {
                if (txtProgress.Text.Trim() != "")
                {
                    isChange = true;
                }
                if (chkSOAP0.Checked == true || chkSOAP1.Checked == true)
                {
                    isChange = true;
                }
            }

            if (isChange == false)
            {
                ClearEnd();
                return true;
            }

            if (ComFunc.MsgBoxQ("EMR 차트 변경 내역이 있습니다." + ComNum.VBLF + "저장하시겠습니까?", "PSMH", MessageBoxDefaultButton.Button1) == DialogResult.No)
            {
                ClearEnd();
                return true;
            }

            mSaveFlag = true;

            rtnVal = SaveData();

            if (rtnVal == true)
            {
                ClearEnd();
            }

            return rtnVal;
        }

        private void ClearEnd()
        {
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");

            dtpChartDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(VB.Left(strCurDateTime, 8), "D"));
            txtChartTime.Text = ComFunc.FormatStrToDate(VB.Right(strCurDateTime, 6), "M");
            mEmrNo = "0";
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
                ////http://192.168.100.33:8090/Emr/doLogin.mts?useId=15200&password=5857b0bd2c33cea36a79dba3e1f17e225e0dad5976b53a7f4b790e9694bcd028&loginType=vb
                ////webEMR.Navigate("http://192.168.100.33:8090/Emr/doLogin.mts?useId=15200&password=5857b0bd2c33cea36a79dba3e1f17e225e0dad5976b53a7f4b790e9694bcd028&loginType=vb");
                strURL = gEmrUrl + "/doLogin.mts?useId=" + strUseId + "&password=" + strPw + "&loginType=vb";
                webEMR.Navigate(strURL);
                DateTime dtp = DateTime.Now;

                while (webEMR.ReadyState != WebBrowserReadyState.Complete)
                {
                    if ((DateTime.Now - dtp).TotalMilliseconds >= 60000)
                    {
                        Log.Debug("webEMR, {}", "1분경과 : " + strURL);
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


                ////http://192.168.100.33:8090/Emr/doLogin.mts?useId=15200&password=5857b0bd2c33cea36a79dba3e1f17e225e0dad5976b53a7f4b790e9694bcd028&loginType=vb
                //webImage.Navigate("http://192.168.100.33:8090/Emr/doLogin.mts?useId=15200&password=5857b0bd2c33cea36a79dba3e1f17e225e0dad5976b53a7f4b790e9694bcd028&loginType=vb");

                strURL = gEmrUrl + "/doLogin.mts?useId=" + strUseId + "&password=" + strPw + "&loginType=vb";
                webImage.Navigate(strURL);
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

        private void GetPatRmk()
        {
            imgRmk.Visible = false;
            imgRmk.Tag = null;
            toolRmk.Tag = null;
            btnSearchRmk.BackColor = Color.White;

            if (AcpEmr == null)
            {
                return;
            }

            string strBad = GetPatInfoBad(AcpEmr.ptNo);
            string strGood = GetPatInfoGood(AcpEmr.ptNo);

            imgRmk.Tag = strBad;
            toolRmk.Tag = strBad;

            if (strBad != "" || strGood != "")
            {
                btnSearchRmk.BackColor = Color.Yellow;
            }

            if (imgRmk.Tag.ToString() != "")
            {
                imgRmk.Visible = true;
            }
        }

        private void GetMibi()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            btnMibi.Visible = false;

            try
            {
                SQL = " SELECT A.PTNO, B.SNAME AS PTNAME, ";
                SQL = SQL + ComNum.VBLF + "        A.MEDFRDATE, A.MEDENDDATE, A.MIBIGRP, A.MIBICD, A.MIBIRMK ";
                SQL = SQL + ComNum.VBLF + "    FROM ADMIN.EMRMIBI A, ADMIN.BAS_PATIENT B ";
                SQL = SQL + ComNum.VBLF + "    WHERE A.MEDDEPTCD = '" + clsType.User.DeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "    AND A.MEDDRCD = '" + clsType.User.IdNumber + "' ";
                SQL = SQL + ComNum.VBLF + "    AND A.MIBICLS = 1";
                SQL = SQL + ComNum.VBLF + "    AND A.MIBIFNDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "    AND A.PTNO = B.PANO(+) ";
                SQL = SQL + ComNum.VBLF + "    ORDER BY B.SNAME, A.MEDFRDATE, A.MIBIGRP, MIBICD ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

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
                dt.Dispose();
                dt = null;
                btnMibi.Visible = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private string GetPatInfoBad(string strPtNo)
        {
            string rtnVal = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = SQL + ComNum.VBLF + " SELECT REMARK ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.SGLRMK ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPtNo + "'";
                SQL = SQL + ComNum.VBLF + " AND GUBUN = '1'";
                SQL = SQL + ComNum.VBLF + " ORDER BY INPDATE DESC";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                rtnVal = (VB.Left(dt.Rows[0]["REMARK"].ToString().Trim(), 20)).Trim();
                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private string GetPatInfoGood(string strPtNo)
        {
            string rtnVal = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = SQL + ComNum.VBLF + " SELECT REMARK ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.SGLRMK ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPtNo + "'";
                SQL = SQL + ComNum.VBLF + " AND GUBUN = '0'";
                SQL = SQL + ComNum.VBLF + " ORDER BY INPDATE DESC";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                rtnVal = (VB.Left(dt.Rows[0]["REMARK"].ToString().Trim(), 20)).Trim();
                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
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

                //---------------------------------------------
                // 2019-01-22 테스트로 변경함
                //---------------------------------------------
                //for (int intWeb = 0; intWeb < 40000; intWeb++)
                //{
                //    Application.DoEvents();
                //}
                //ComFunc.Delay(1000);
                //---------------------------------------------

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
                //webEMR.Navigate(strURL); //한장씩 볼 경우
                //while (webEMR.IsBusy == true)
                //{
                //    Application.DoEvents();
                //}
                //for (int intWeb = 0; intWeb < 40000; intWeb++)
                //{
                //    Application.DoEvents();
                //}

            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                //ComFunc.MsgBoxEx(this, ex.Message);
                //clsDB.SaveSqlErrLog(ex.Message, "", clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private void GetChartHis()
        {
            if (AcpEmr == null)
            {
                Log.Warn(" AcpEmr is Null");
                return;
            }


            //의사가 아닐경우 연속보기 사용안함
            if (clsType.User.DrCode == "")
            {
                return;
            }


            string strURL = "";
            //string strEmrNo = "74761596";
            //webEMR.Navigate(gEmrUrl + "/emrView.mts?emrNo=" + strEmrNo); //한장씩 볼 경우

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                string strCurDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
                int intRowCnt = 0;

                if (mIsFirtQuery == true)
                {
                    //2018.09.11 shlee. 아래 조건 추가(외래환자일 경우에만 Check)
                    if (AcpEmr.inOutCls == "O")
                    {
                        if (AcpEmr.medDeptCd == "ER")
                        {
                            return;
                        }

                        SQL = " SELECT PANO";
                        SQL = SQL + ComNum.VBLF + "  FROM ADMIN.OPD_MASTER";
                        SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + AcpEmr.ptNo + "'";
                        SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + AcpEmr.medDeptCd + "'";
                        SQL = SQL + ComNum.VBLF + "   AND BDATE < TO_DATE('" + AcpEmr.medFrDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "   AND ROWNUM = 1";

                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        intRowCnt = dt.Rows.Count;
                        dt.Dispose();
                        dt = null;

                        if (intRowCnt == 0)
                        {
                            intRowCnt = 0;
                            SQL = " SELECT A.EMRNO";
                            SQL = SQL + ComNum.VBLF + " FROM ADMIN.EMRXMLMST A, ADMIN.EMRGRPFORM B, ADMIN.EMRFORM C";
                            SQL = SQL + ComNum.VBLF + " WHERE C.GRPFORMNO = b.GRPFORMNO";
                            SQL = SQL + ComNum.VBLF + "   AND A.FORMNO = C.FORMNO";
                            SQL = SQL + ComNum.VBLF + "   AND B.GRPFORMNO IN (27,2)";
                            SQL = SQL + ComNum.VBLF + "   AND PTNO = '" + AcpEmr.ptNo + "'";
                            SQL = SQL + ComNum.VBLF + "   AND A.CHARTDATE = '" + AcpEmr.medFrDate + "'";
                            SQL = SQL + ComNum.VBLF + "   AND A.MEDDEPTCD = '" + AcpEmr.medDeptCd + "' ";
                            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }
                            intRowCnt = dt.Rows.Count;
                            dt.Dispose();
                            dt = null;

                            if (intRowCnt == 0)
                            {
                                ComFunc.MsgBoxEx(this, "해당 진료과에 처음 진료받는 환자입니다. 초진기록지를 작성하여 주시기 바랍니다.");
                            }
                        }
                    }
                }

                mIsFirtQuery = false;

                #region //경과기록지 보기

                string pPtPtNo = AcpEmr.ptNo;
                string pInOutCls = "";
                string pMedDeptCd = "";
                string pMedFrDate = "";
                string pMedEndDate = "";
                string pSort = "";
                string pGUBUN = "";

                if (optInOut1.Checked == true)
                {
                    pInOutCls = "1";
                }
                else if (optInOut2.Checked == true)
                {
                    pInOutCls = "2";
                }
                else if (optInOut3.Checked == true)
                {
                    pInOutCls = "0";
                }

                if (chkUserOpt.Checked == false)
                {
                    pMedDeptCd = clsType.User.DeptCode;
                    pGUBUN = "1";
                }
                else
                {
                    pMedDeptCd = mstrUserDept.Replace("'", "").Replace(",", "^");
                    pGUBUN = "2";
                }
                string strFirstDATE = "";
                ReadFirstDate(ref strFirstDATE);

                string strDTPSDate = "";
                string strDTPEDATE = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

                strDTPSDate = dtpSDate.Value.ToString("yyyyMMdd");

                pMedFrDate = dtpSDate.Value.ToString("yyyyMMdd");
                pMedEndDate = dtpEDate.Value.ToString("yyyyMMdd");

                if (clsEmrQueryOld.READ_FM_LIMIT(AcpEmr.ptNo, AcpEmr.medDrCd, "") == true)
                {
                    pMedDeptCd = VB.Replace(pMedDeptCd, "FM^", "");
                }
                if (clsType.User.IdNumber == "19094")
                {
                    if (pMedDeptCd == "MR" || pMedDeptCd == "RA")
                    {
                        pMedDeptCd = "MR^RA^";
                    }
                    else
                    {
                        switch (pMedDeptCd)
                        {
                            case "MC":
                            case "MD":
                            case "ME":
                            case "MG":
                            case "MI":
                            case "MN":
                            case "MP":
                            case "MR":
                                pMedDeptCd = "MD^MG^MC^MP^ME^MN^MR^MI^";
                                break;
                            default:

                                break;
                        }
                    }
                }
                else
                {
                    switch (pMedDeptCd)
                    {
                        case "MC":
                        case "MD":
                        case "ME":
                        case "MG":
                        case "MI":
                        case "MN":
                        case "MP":
                        case "MR":
                            pMedDeptCd = "MD^MG^MC^MP^ME^MN^MR^MI^";
                            break;
                        default:

                            break;
                    }
                }


                if (opSortAs.Checked == true)
                {
                    pSort = "1";
                }
                else
                {
                    pSort = "0";
                }

                DateTime dtp = DateTime.Now;
                //http://192.168.100.33:8090/Emr/grpFormNoByProgress.mts?grpFormNo=2&ptNo=03983614&inOutCls=0&medDeptCd=MD^MN^OS^PC^DM&startDate=20150820&endDate=20180522&sort=0&gubun=2
                //http://192.168.100.33:8090/Emr/grpFormNoByProgress.mts?grpFormNo=2&ptNo=03983614&inOutCls=0&medDeptCd=DM^MD^MN^OS^PC&startDate=20160501&endDate=20160501&sort=0&gubun=2
                strURL = gEmrUrl + "/grpFormNoByProgress.mts?grpFormNo=2&ptNo=" + pPtPtNo +
                        "&inOutCls=" + pInOutCls +
                        "&medDeptCd=" + pMedDeptCd +
                        "&startDate=" + pMedFrDate +
                        "&endDate=" + pMedEndDate +
                        "&sort=" + pSort +
                        "&gubun=" + pGUBUN;

                //strURL = "http://192.168.100.33:8090/Emr/grpFormNoByProgress.mts?grpFormNo=2&ptNo=02487371&inOutCls=0&medDeptCd=MD^MN^OS^PC^DM&startDate=20090302&endDate=20180524&sort=0&gubun=2";
                webEMR.Navigate(strURL);
                dtp = DateTime.Now;

                while (webEMR.ReadyState != WebBrowserReadyState.Complete)
                {
                    if ((DateTime.Now - dtp).TotalMilliseconds >= 60000)
                    {
                        Log.Debug("webEMR, {} ", "1분경과 : " + pPtPtNo + ":" + pMedDeptCd + " : URL = " + strURL);
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
                #endregion //경과기록지 보기

                if (clsEmrQueryOld.EmrSaveUserOption(clsDB.DbCon, "OOORD", "DTPSDATE", dtpSDate.Value.ToString("yyyy-MM-dd")) == false)
                {

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
            SQL = SQL + ComNum.VBLF + " From ADMIN.IPD_NEW_MASTER";
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
            SQL = SQL + ComNum.VBLF + " SELECT MIN(ACTDATE) DDATE FROM ADMIN.OPD_MASTER";
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

        private void InitForm()
        {
            lblChoChart.Text = "";
            ssUSERFORM_Sheet1.RowCount = 0;
            ssUserDept_Sheet1.RowCount = 0;
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
            mEmrNo = "0";
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

        private void GetUserChoFormNew()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssUSERFORM_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "  SELECT B.GRPFORMNO, B.GRPFORMNAME, A.FORMNO, A.FORMNAME1 FORMNAME, DECODE(A.INOUTCLS,'1','외래','2','입원','공통') AS FORMGB, C.DISPSEQ ";
                SQL = SQL + ComNum.VBLF + "    FROM ADMIN.EMRFORM A INNER JOIN ADMIN.EMRGRPFORM B";
                SQL = SQL + ComNum.VBLF + "        ON A.GRPFORMNO = B.GRPFORMNO";
                SQL = SQL + ComNum.VBLF + "        INNER JOIN ADMIN.EMRUSERFORMCHO C";
                SQL = SQL + ComNum.VBLF + "        ON A.FORMNO = C.FORMNO";
                SQL = SQL + ComNum.VBLF + "    WHERE (B.USECHECK IS NULL ";
                SQL = SQL + ComNum.VBLF + "        OR B.USECHECK = '0')";
                SQL = SQL + ComNum.VBLF + "    AND C.USEID = '" + clsType.User.IdNumber + "'";
                SQL = SQL + ComNum.VBLF + "    ORDER BY C.DISPSEQ, A.FORMNO";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

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

                ssUSERFORM_Sheet1.RowCount = dt.Rows.Count;
                ssUSERFORM_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        mstrUserChoJinFormName = dt.Rows[i]["FORMNAME"].ToString().Trim();
                        mstrUserChoJinForm = dt.Rows[i]["FORMNO"].ToString().Trim();
                    }
                    ssUSERFORM_Sheet1.Cells[i, 0].Text = dt.Rows[i]["GRPFORMNAME"].ToString().Trim();
                    ssUSERFORM_Sheet1.Cells[i, 1].Text = dt.Rows[i]["FORMNAME"].ToString().Trim();
                    ssUSERFORM_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DISPSEQ"].ToString().Trim();
                    ssUSERFORM_Sheet1.Cells[i, 3].Text = dt.Rows[i]["FORMNO"].ToString().Trim();
                    ssUSERFORM_Sheet1.Cells[i, 4].Text = dt.Rows[i]["GRPFORMNO"].ToString().Trim();
                    ssUSERFORM_Sheet1.Cells[i, 6].Text = dt.Rows[i]["FORMGB"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void GetUserDept()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssUserDept_Sheet1.RowCount = 0;
            try
            {
                SQL = "";
                SQL = "SELECT ";
                SQL = SQL + ComNum.VBLF + "     A.MEDDEPTCD, A.DEPTKORNAME, B.USEID ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.VIEWBMEDDEPT A ";
                SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN ADMIN.EMRUSERDEPT B";
                SQL = SQL + ComNum.VBLF + "    ON A.MEDDEPTCD = B.MEDDEPTCD ";
                SQL = SQL + ComNum.VBLF + "     AND B.USEID = '" + clsType.User.IdNumber + "'";
                SQL = SQL + ComNum.VBLF + " ORDER BY A.DEPTKORNAME, A.PRTGRD ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

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

                ssUserDept_Sheet1.RowCount = dt.Rows.Count;
                ssUserDept_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["USEID"].ToString().Trim() != "")
                    {
                        ssUserDept_Sheet1.Cells[i, 0].Value = true;
                    }
                    ssUserDept_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DEPTKORNAME"].ToString().Trim();
                    ssUserDept_Sheet1.Cells[i, 2].Text = dt.Rows[i]["MEDDEPTCD"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

                MakeUserDept();
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void MakeUserDept()
        {
            int i = 0;
            mstrUserDept = "";
            for (i = 0; i < ssUserDept_Sheet1.RowCount; i++)
            {
                if (Convert.ToBoolean(ssUserDept_Sheet1.Cells[i, 0].Value) == true)
                {
                    mstrUserDept = mstrUserDept + ",'" + ssUserDept_Sheet1.Cells[i, 2].Text.Trim() + "'";
                }
            }
            if (mstrUserDept.Length > 0)
            {
                mstrUserDept = VB.Right(mstrUserDept, mstrUserDept.Length - 1);
            }
        }

        private void SetUserOption()
        {
            GetUserChoFormNew();
            GetUserDept();

            lblChoChart.Text = mstrUserChoJinFormName;

            WebLogin();

            string strOptMcro = "";
            strOptMcro = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "OPTMCRO");
            if (strOptMcro == "1")
            {
                optDept.Checked = true;
            }
            else if (strOptMcro == "2")
            {
                optAll.Checked = true;
            }
            else
            {
                optUse.Checked = true;
            }

            string strOptKind = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "OPTKIND");
            if (strOptKind == "1")
            {
                optInOut1.Checked = true;
            }
            else if (strOptKind == "2")
            {
                optInOut2.Checked = true;
            }
            else
            {
                optInOut3.Checked = true;
            }

            string strOptSort = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "OPTSORT");
            if (strOptSort == "1")
            {
                opSortAs.Checked = true;
            }
            else
            {
                opSortDs.Checked = true;
            }

            string strDTPSDate = "";
            string strDTPEDATE = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            strDTPSDate = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "DTPSDATE");
            if (strDTPSDate == "")
            {
                strDTPSDate = (VB.DateAdd("m", -3, strDTPEDATE)).ToString("yyyy-MM-dd");
            }

            dtpSDate.Value = Convert.ToDateTime(strDTPSDate);
            dtpEDate.Value = Convert.ToDateTime(strDTPEDATE);

        }

        #endregion //Private Function

        private void frmEmrBaseProgressOcs_Load(object sender, EventArgs e)
        {
            webEMR.Dock = DockStyle.Fill;
            webImage.Dock = DockStyle.Fill;
            webEMR.BringToFront();
            webImage.BringToFront();

            //webEMR.ObjectForScripting = this;
            //webImage.ObjectForScripting = this;

            webEMR.ScriptErrorsSuppressed = true;
            webImage.ScriptErrorsSuppressed = true;

            ssUserDept.Top = 36;
            ssUserDept.Left = 7;

            ssUSERFORM.Top = 36;
            ssUSERFORM.Left = 7;

            ssGRPMACRO.Top = 36;
            ssGRPMACRO.Left = 290;

            txtProgress.Font = new Font("굴림체", 11, FontStyle.Regular);

            DateTime dtp;
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");

            dtpChartDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(VB.Left(strCurDateTime, 8), "D"));
            txtChartTime.Text = ComFunc.FormatStrToDate(VB.Right(strCurDateTime, 6), "M");

            string strDTPSDate = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "DTPSDATE");
            dtpEDate.Value = dtpChartDate.Value;
            if(strDTPSDate!= "")
            {
                dtpSDate.Value = Convert.ToDateTime(strDTPSDate);
            }
            //dtpSDate.Value = VB.DateAdd("m", -1, dtpEDate.Value.ToString());

            mstrPROGNO = "963";
            //mstrPROGNAME = "Progress Note";
            //mstrPROGIMGNO = "1232";
            //mstrPROGIMGNAME = "Progress Image";

            switch (clsType.User.DeptCode)
            {
                case "MG":
                case "MC":
                case "MP":
                case "ME":
                case "MN":
                case "MI":
                case "MD":
                    chkUserOpt.Checked = false;
                    break;
                default:
                    chkUserOpt.Checked = true;
                    break;
            }

            chkUserOpt.Checked = true;

            ClearForm();
            string strEmrOption = "";
            strEmrOption = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "EMRKORENG");
            if (VB.Val(strEmrOption) == 1)
            {
                ComFunc.SetIMEMODE(this, "E");
                lblKorE.Text = "영어";
            }
            else
            {
                ComFunc.SetIMEMODE(this, "K");
                lblKorE.Text = "한글";
            }
            
            //MakeWardChart(); //이거 필요없음
            SetUserOption();

            if (clsType.User.IdNumber == "19094")
            {
                MakeUserDept_19094();
            }

            webEMR.Navigate(EmrUrlMain);
            dtp = DateTime.Now;
            while (webEMR.ReadyState != WebBrowserReadyState.Complete)
            {
                if ((DateTime.Now - dtp).TotalMilliseconds >= 60000)
                {
                    Log.Debug("webEMR, {}", "1분경과 : " + EmrUrlMain);
                    break;
                }
                Application.DoEvents();
            }
            Application.DoEvents();

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
                //if (AcpEmr.inOutCls == "I")
                //{
                //    optInOut2.Checked = true;
                //}
                //else
                //{
                //    optInOut1.Checked = true;
                //}

                SetPatInfoImg();
                GetChartHis();
                tabEmr.SelectedTab = tabEmrWrite;
            }

            IsLoadForm = false;

            strEmrOption = "";
            //【처방화면 사이즈 조절】
            strEmrOption = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "ORDEMRRESIZE");
            if (VB.Val(strEmrOption) == 1)
            {
                txtProgress.Dock = DockStyle.Fill;
            }

            strEmrOption = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "AFTERPROGSAVE");
            if (VB.Val(strEmrOption) == 1)
            {
                tabEmr.SelectedTab = tabEmrWrite;
            }
            else
            {
                tabEmr.SelectedTab = tabEmrView;
            }

        }

        /// <summary>
        /// 입원일자 다른지 체크
        /// </summary>
        void ChkFrDate()
        {
            //외래면 체크안함.
            if (AcpEmr == null || AcpEmr.inOutCls == "O")
                return;

            OracleDataReader reader = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            try
            {
                SQL = " SELECT TO_CHAR(MAX(INDATE), 'YYYYMMDD') INDATE";
                SQL += ComNum.VBLF + " FROM ADMIN.IPD_NEW_MASTER";
                SQL += ComNum.VBLF + " WHERE PANO = '" + AcpEmr.ptNo + "'";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "입원일자 점검중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (reader.HasRows && reader.Read())
                {
                    if (reader.GetValue(0).ToString().Trim() != AcpEmr.medFrDate)
                    {
                        AcpEmr.medFrDate = reader.GetValue(0).ToString().Trim();
                    }
                }

                reader.Dispose();
                reader = null;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void MakeUserDept_19094()
        {
            int i = 0;

            for (i = 0; i < ssUserDept_Sheet1.RowCount; i++)
            {
                ssUserDept_Sheet1.Cells[i, 0].Value = false;
                if (ssUserDept_Sheet1.Cells[i, 2].Text.Trim() == "RA" || ssUserDept_Sheet1.Cells[i, 2].Text.Trim() == "MR")
                {
                    ssUserDept_Sheet1.Cells[i, 0].Value = true;
                }
            }

            mstrUserDept = "";
            for (i = 0; i < ssUserDept_Sheet1.RowCount; i++)
            {
                if (Convert.ToBoolean(ssUserDept_Sheet1.Cells[i, 0].Value) == true)
                {
                    mstrUserDept = mstrUserDept + ",'" + ssUserDept_Sheet1.Cells[i, 2].Text.Trim() + "'";
                }
            }
            if (mstrUserDept.Length > 0)
            {
                mstrUserDept = VB.Right(mstrUserDept, mstrUserDept.Length - 1);
            }
        }

        private void frmEmrBaseProgressOcs_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                webImage.Navigate("http://192.168.100.33:8090/Emr/doLogOut.mts");   //로그아웃
                webEMR.Navigate("http://192.168.100.33:8090/Emr/doLogOut.mts");   //로그아웃

                if (fEmrMacro != null)
                {
                    fEmrMacro.Dispose();
                    fEmrMacro = null;
                }
                if (fEmrChart != null)
                {
                    fEmrChart.Dispose();
                    fEmrChart = null;
                }

                this.SuspendLayout();

                this.webImage.DocumentCompleted -= new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webImage_DocumentCompleted);
                this.webEMR.DocumentCompleted -= new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webEMR_DocumentCompleted);

                this.Controls.Remove(this.webImage);
                this.Controls.Remove(this.webEMR);
                this.ResumeLayout(false);

                webImage.Dispose();
                webImage = null;

                webEMR.Dispose();
                webEMR = null;
            }
            catch
            {

            }
        }

        private void btnSerach_Click(object sender, EventArgs e)
        {
            //SetPatInfoImg();
            GetChartHis();
        }

        private void btnUserChoReg_Click(object sender, EventArgs e)
        {
            frmEmrBaseUserChoRegOld frm = new frmEmrBaseUserChoRegOld();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
            GetUserChoFormNew();
        }

        private void btnSearchRmk_Click(object sender, EventArgs e)
        {
            if (AcpEmr == null)
                return;

            frmEmrBaseSingularRemark frm = new frmEmrBaseSingularRemark(AcpEmr.ptNo);
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }

        private void btnSetUserDept_Click(object sender, EventArgs e)
        {
            ssUserDept.Visible = true;
            ssUserDept.BringToFront();
            ssUserDept.Focus();
        }

        private void ssUserDept_Leave(object sender, EventArgs e)
        {
            SaveUserDept();
            GetUserDept();
            ssUserDept.Visible = false;
        }

        private bool SaveUserDept()
        {
            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE FROM ADMIN.EMRUSERDEPT";
                SQL = SQL + ComNum.VBLF + " WHERE USEID = '" + clsType.User.IdNumber + "'";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");

                for (i = 0; i < ssUserDept_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(ssUserDept_Sheet1.Cells[i, 0].Value) == true)
                    {
                        string strDeptCode = ssUserDept_Sheet1.Cells[i, 2].Text.Trim();

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " INSERT INTO ADMIN.EMRUSERDEPT ";
                        SQL = SQL + ComNum.VBLF + "      (USEID, MEDDEPTCD, DISPSEQ, WRITEDATE, WRITETIME)";
                        SQL = SQL + ComNum.VBLF + " VALUES (";
                        SQL = SQL + ComNum.VBLF + "  '" + clsType.User.IdNumber + "',";
                        SQL = SQL + ComNum.VBLF + "  '" + strDeptCode + "',";
                        SQL = SQL + ComNum.VBLF + "  1,";
                        SQL = SQL + ComNum.VBLF + " '" + VB.Left(strCurDateTime, 8) + "',";
                        SQL = SQL + ComNum.VBLF + " '" + VB.Right(strCurDateTime, 6) + "')";

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

        private void mbtnSheet_Click(object sender, EventArgs e)
        {
            GetUserChoFormNew();

            if (mstrUserChoJinForm == "")
            {
                ComFunc.MsgBoxEx(this, "사용자별 초진 기록지가 등록되어 있지 않습니다." + ComNum.DB_PMPA + "초진기록지를 등록해 주십시오.");
                frmEmrBaseUserChoRegOld frm = new frmEmrBaseUserChoRegOld();
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog();
                GetUserChoFormNew();
                lblChoChart.Text = mstrUserChoJinFormName;
                return;
            }
            lblChoChart.Text = mstrUserChoJinFormName;
            ssUSERFORM.Visible = true;
            ssUSERFORM.BringToFront();
        }

        private void ssUSERFORM_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssUSERFORM_Sheet1.RowCount <= 0)
                return;
            if (e.ColumnHeader == true)
                return;

            ssUSERFORM.Visible = false;

            string strFormNm = ssUSERFORM_Sheet1.Cells[e.Row, 1].Text.Trim();
            string strForm = ssUSERFORM_Sheet1.Cells[e.Row, 3].Text.Trim();

            //if (AcpEmr.inOutCls == "O" && ssUSERFORM_Sheet1.Cells[e.Row, 6].Text.Trim() == "입원")
            //{
            //    ComFunc.MsgBoxEx(this, "외래에서는 사용 불가합니다.");
            //    return;
            //}

            LoadFixedSheet("NEW", strForm, strFormNm, clsType.User.IdNumber, "", "", "");
        }

        private void ssUSERFORM_Leave(object sender, EventArgs e)
        {
            ssUSERFORM.Visible = false;
        }

        private void mbtnMacro_Click(object sender, EventArgs e)
        {
            if (fEmrMacro != null)
            {
                fEmrMacro.Dispose();
                fEmrMacro = null;
            }

            panProg.Height = 500;
            Application.DoEvents();

            fEmrMacro = new frmEmrBaseSympOld(clsType.User.IdNumber, clsType.User.DeptCode, clsType.User.IdNumber, "ta1", "ProgressNote");
            fEmrMacro.rEventMakeText += new frmEmrBaseSympOld.EventMakeText(frmEmrBaseSympOld_EventMakeText);
            fEmrMacro.FormClosed += FEmrMacro_FormClosed;
            fEmrMacro.Owner = this;
            fEmrMacro.Show();
        }

        private void FEmrMacro_FormClosed(object sender, FormClosedEventArgs e)
        {
            fEmrMacro.Dispose();
            fEmrMacro = null;
        }

        private void frmEmrBaseSympOld_EventMakeText(int intOption, string strMacro)
        {
            if (intOption == 0)
            {
                txtProgress.Text = "";
                txtProgress.Text = strMacro;
            }
            else
            {
                //txtProgress.Text = txtProgress.Text + " " + strMacro;
                int selstart = txtProgress.SelectionStart;
                int intMacro = strMacro.Length;
                txtProgress.Text = txtProgress.Text.Insert(selstart, " " + strMacro);
                txtProgress.Focus();
                txtProgress.SelectionStart = selstart + intMacro;
            }
        }


        private void mbtnNew_Click(object sender, EventArgs e)
        {
            ClearNew();
        }

        private void webImage_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void webEMR_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                // WebBrowser browser = sender as WebBrowser;

                string[] arryArUrl = null;
                string strEmrNo = "";
                string strRmk = "";
                string strEmrImgNo = "";
                string strFormNo = "";
                string strUSERFORMNO = "";
                string strFormName = "";
                //string strIMGPATH = "";
                string strChartDate = "";
                string strMedFrDate = "";
                string strChartTime = "";
                string strUseId = "";

                arryArUrl = VB.Split(e.Url.ToString(), "&");
                string strUrl = e.Url.ToString();

                if (arryArUrl.Length <= 6)
                {
                    return;
                }

                int i = 0;
                DataTable dt = null;
                string SQL = "";    //Query문
                string SqlErr = ""; //에러문 받는 변수

                for (i = 0; i < arryArUrl.Length; i++)
                {
                    if (VB.Left(arryArUrl[i], 4) == "GOVB")
                    {
                        strEmrNo = VB.Mid(arryArUrl[i], 6, arryArUrl[i].Length - 5);
                    }
                }

                if (VB.Val(strEmrNo) == 0)
                {
                    Log.Warn("strEmrNo 없음");
                    return;
                }


                SQL = "SELECT A.FORMNO, A.EMRNO, B.FORMNAME1 FORMNAME,  B.USERFORMNO, A.MEDFRDATE,";
                SQL = SQL + ComNum.VBLF + "    A.CHARTDATE, A.CHARTTIME, A.USEID";
                SQL = SQL + ComNum.VBLF + "    FROM ADMIN.EMRXML A INNER JOIN ADMIN.EMRFORM B";
                SQL = SQL + ComNum.VBLF + "    ON A.FORMNO = B.FORMNO AND A.UPDATENO = B.UPDATENO";
                SQL = SQL + ComNum.VBLF + "    AND A.EMRNO = " + strEmrNo;

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

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

                strEmrNo = dt.Rows[0]["EMRNO"].ToString().Trim();
                strFormNo = dt.Rows[0]["FORMNO"].ToString().Trim();
                strUSERFORMNO = dt.Rows[0]["USERFORMNO"].ToString().Trim();
                strFormName = dt.Rows[0]["FORMNAME"].ToString().Trim();
                //strIMGPATH = "";
                strChartDate = dt.Rows[0]["CHARTDATE"].ToString().Trim();
                strChartTime = dt.Rows[0]["CHARTTIME"].ToString().Trim();
                strUseId = dt.Rows[0]["USEID"].ToString().Trim();
                strMedFrDate = dt.Rows[0]["MEDFRDATE"].ToString().Trim();

                dt.Dispose();
                dt = null;

                if (strUSERFORMNO == "1")
                {
                    strRmk = GetProgText(strEmrNo);
                }

                chkSOAP0.Checked = false;
                chkSOAP1.Checked = false;

                switch (strUSERFORMNO)
                {
                    case "0":   //정형화서식(초진,입원 등)
                        LoadFixedSheet("OLD", strFormNo, strFormName, strUseId, strEmrNo, strChartDate, strChartTime);
                        break;
                    case "1":   //프로그래스
                        dtpChartDate.Text = ComFunc.FormatStrToDate(strChartDate, "D");
                        txtChartTime.Text = ComFunc.FormatStrToDate(strChartTime, "M");
                        dtpChartDate.Enabled = false;
                        txtChartTime.Enabled = false;
                        txtProgress.Text = strRmk.Replace("\r\n", "\n").Replace("\n", "\r\n");
                        txtProgress.Tag = txtProgress.Text;

                        txtEmrNo.Text = strEmrNo;

                        if (clsType.User.IdNumber != strUseId)
                        {
                            txtEmrNo.Enabled = true;
                            mbtnSave.Visible = false;
                            mbtnDelete.Visible = false;
                            mbtnSaveImag.Visible = false;
                        }
                        else
                        {
                            txtEmrNo.Enabled = true;
                            mbtnSave.Visible = true;
                            mbtnDelete.Visible = true;
                            mbtnSaveImag.Visible = true;
                        }

                        if (AcpEmr.inOutCls == "O")
                        {
                            if (AcpEmr.medFrDate != strMedFrDate)
                            {
                                mbtnDelete.Visible = false;
                                mbtnSave.Visible = false;
                                mbtnSaveImag.Visible = false;
                            }
                            else
                            {
                                mbtnDelete.Visible = true;
                                mbtnSave.Visible = true;
                                mbtnSaveImag.Visible = true;
                            }
                        }
                        else
                        {
                            mbtnDelete.Visible = true;
                            mbtnSave.Visible = true;
                            mbtnSaveImag.Visible = true;
                        }
                        tabEmr.SelectedTab = tabEmrWrite;
                        break;
                    case "2":   //이미지
                        for (i = 0; i < arryArUrl.Length; i++)
                        {
                            if (VB.Left(arryArUrl[i], 5) == "IMGNO")
                            {
                                strEmrImgNo = VB.Mid(arryArUrl[i], 7, arryArUrl[i].Length - 6);
                            }
                        }

                        if (VB.Val(strEmrImgNo) == 0)
                        {
                            return;
                        }

                        txtEmrNo.Text = strEmrNo;
                        mEmrImageNo = strEmrImgNo;

                        dtpChartDate.Text = ComFunc.FormatStrToDate(strChartDate, "D");
                        txtChartTime.Text = ComFunc.FormatStrToDate(strChartTime, "M");
                        dtpChartDate.Enabled = false;
                        txtChartTime.Enabled = false;
                        txtProgress.Text = strRmk;
                        txtEmrNo.Text = strEmrImgNo;
                        if (AcpEmr.inOutCls == "O")
                        {
                            if (AcpEmr.medFrDate != strMedFrDate)
                            {
                                mbtnDelete.Visible = false;
                                mbtnSave.Visible = false;
                                mbtnSaveImag.Visible = false;
                            }
                            else
                            {
                                mbtnDelete.Visible = true;
                                mbtnSave.Visible = true;
                                mbtnSaveImag.Visible = true;
                            }
                        }
                        else
                        {
                            mbtnDelete.Visible = true;
                            mbtnSave.Visible = true;
                            mbtnSaveImag.Visible = true;
                        }
                        tabEmr.SelectedTab = tabEmrWrite;
                        webImage.Navigate(EmrUrlImage + "1232" + "&emrImageNo=" + strEmrImgNo);
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
                        //ComFunc.Delay(1000);
                        //---------------------------------------------
                        tabEmr.SelectedTab = tabEmrWrite;

                        break;
                    default:

                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                StackTrace stackTrace = new StackTrace();
                foreach (StackFrame frame in stackTrace.GetFrames())
                {
                    Log.Debug(frame.GetFileName() + ",line:" + frame.GetFileLineNumber() + " , " + frame.GetMethod().Name);
                }
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
            }

        }

        private void LoadFixedSheet(string strNew, string strFormNo, string strFormName, string strUseId, string strEmrNo, string strChartDate, string strChartTime)
        {
            if (fEmrChart != null)
            {
                fEmrChart.Dispose();
                fEmrChart = null;
            }
            //(Form pCallForm, EmrPatient pAcpEmr, string pOld, string pChartDate, string pChartTime, string pFormName, string pEmrNo = "")
            fEmrChart = new frmEmrBaseEmrChartOld(this, AcpEmr, strNew, strChartDate, strChartTime, strFormNo, strFormName, strEmrNo);
            fEmrChart.rSaveOrDelete += new frmEmrBaseEmrChartOld.SaveOrDelete(frmEmrBaseEmrChartOld_SaveOrDelete);
            fEmrChart.rEventClosed += new frmEmrBaseEmrChartOld.EventClosed(frmEmrBaseEmrChartOld_EventClosed);
            //fEmrChart.ShowDialog(this);
            fEmrChart.Owner = this;
            fEmrChart.Show();

        }

        private void frmEmrBaseEmrChartOld_SaveOrDelete()
        {
            fEmrChart.Dispose();
            fEmrChart = null;
            GetChartHis();
        }

        private void frmEmrBaseEmrChartOld_EventClosed()
        {
            fEmrChart.Dispose();
            fEmrChart = null;
        }

        private string GetProgText(string strEmrNo)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "SELECT B.USERFORMNO, CHARTXML AS CHARTA";
                SQL = SQL + ComNum.VBLF + "    FROM ADMIN.EMRXML A INNER JOIN ADMIN.EMRFORM B";
                SQL = SQL + ComNum.VBLF + "    ON A.FORMNO = B.FORMNO AND A.UPDATENO = B.UPDATENO";
                SQL = SQL + ComNum.VBLF + "    AND A.EMRNO = " + strEmrNo;

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (dt.Rows[0]["USERFORMNO"].ToString().Trim() == "0" || dt.Rows[0]["USERFORMNO"].ToString().Trim() == "1")
                {
                    rtnVal = MakeContentValue(dt.Rows[0]["CHARTA"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장

                return rtnVal;
            }
        }

        private string MakeContentValue(string strCHARTA)
        {
            string rtnVal = "";

            XmlDocument Doc = new XmlDocument();

            try
            {
                Doc.LoadXml(strCHARTA);

                XmlNodeList nodeList = null;

                nodeList = Doc.SelectNodes("chart");

                foreach (XmlNode node in nodeList)
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        rtnVal = rtnVal + (childNode.InnerText.ToString() + "").ToString();
                        rtnVal = rtnVal + ComNum.VBLF;
                    }
                }

            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }

            return rtnVal;
        }

        private void txtProgress_Enter(object sender, EventArgs e)
        {
            panProg.Height = 500;
            if (lblKorE.Text == "영어")
            {
                ComFunc.SetIMEMODE(this, "E");
                lblKorE.Text = "영어";
            }
            else
            {
                ComFunc.SetIMEMODE(this, "K");
                lblKorE.Text = "한글";
            }
        }

        private void btnBig_Click(object sender, EventArgs e)
        {
            panProg.Height = 500;
        }

        private void btnSmall_Click(object sender, EventArgs e)
        {
            panProg.Height = 150;
        }

        private void chkSOAP0_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSOAP0.Checked == true)
            {
                chkSOAP1.Checked = false;
                ssSOAP_Sheet1.Cells[0, 0].Text = "S)";
                ssSOAP_Sheet1.Cells[1, 0].Text = "O)";
                ssSOAP_Sheet1.Cells[2, 0].Text = "A)";
                ssSOAP_Sheet1.Cells[3, 0].Text = "P)";
                ssSOAP_Sheet1.Columns[0].Width = 30;
                ssSOAP_Sheet1.Columns[1].Width = 520;
                ssSOAP.Visible = true;
                ssSOAP.Top = 117;
                ssSOAP.Left = 0;
                ssSOAP.Width = panProg.Width;
                panProg.Height = 500;
                ssSOAP.BringToFront();
                ssSOAP.Focus();
                ssSOAP_Sheet1.SetActiveCell(0, 1);
            }
            else
            {
                if (chkSOAP1.Checked == false)
                {
                    ssSOAP.Visible = false;
                    panProg.Height = 150;
                }
            }
        }

        private void chkSOAP1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSOAP1.Checked == true)
            {
                chkSOAP0.Checked = false;
                ssSOAP_Sheet1.Cells[0, 0].Text = "수술/시술명 및 수술/시술후 환자상태)";
                ssSOAP_Sheet1.Cells[1, 0].Text = "수술/시술 후 주요검사결과)";
                ssSOAP_Sheet1.Cells[2, 0].Text = "수술/시술후 발생가능한 문제점)";
                ssSOAP_Sheet1.Cells[3, 0].Text = "관찰 및 치료계획)";
                ssSOAP_Sheet1.Columns[0].Width = 90;
                ssSOAP_Sheet1.Columns[1].Width = 460;
                ssSOAP.Visible = true;
                ssSOAP.Top = 117;
                ssSOAP.Left = 0;
                ssSOAP.Width = panProg.Width;
                panProg.Height = 500;
                ssSOAP.BringToFront();
                ssSOAP.Focus();
                ssSOAP_Sheet1.SetActiveCell(0, 1);
            }
            else
            {
                if (chkSOAP0.Checked == false)
                {
                    ssSOAP.Visible = false;
                    panProg.Height = 150;
                }
            }
        }

        private void imgRmk_MouseHover(object sender, EventArgs e)
        {
            if (toolRmk.Tag == null)
                return;
            if (toolRmk.Tag.ToString() != "")
                return;

            toolRmk.SetToolTip(imgRmk, toolRmk.Tag.ToString());
        }

        private void mbtnSave_Click(object sender, EventArgs e)
        {
            mSaveFlag = false;

            if (VB.Val(txtEmrNo.Text) > 0)
            {
                if (ComFunc.MsgBoxQ("기존내용을 변경하시겠습니까?", "PSMH", MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }

            ChkFrDate();

            if (SaveData() == true)
            {
                string strEmrNo = "0";
                strEmrNo = mEmrNo;

                ClearForm();
                string strEmrOption = "0";
                strEmrOption = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "AFTERPROGSAVE");
                if (VB.Val(strEmrOption) == 1)
                {
                    SetProgressOne(strEmrNo);
                }
                else
                {
                    tabEmr.SelectedTab = tabEmrView;
                }

                //SetPatInfoImg();
                GetChartHis();


            }
        }

        private void SetProgressOne(string strEmrNo)
        {
            if (AcpEmr == null)
            {
                return;
            }

            try
            {
                DataTable dt = null;
                string SQL = "";    //Query문
                string SqlErr = ""; //에러문 받는 변수

                string strRmk = "";
                string strFormNo = "";
                string strUSERFORMNO = "";
                string strFormName = "";
                string strChartDate = "";
                string strMedFrDate = "";
                string strChartTime = "";
                string strUseId = "";

                SQL = "SELECT A.FORMNO, A.EMRNO, B.FORMNAME1 FORMNAME,  B.USERFORMNO, A.MEDFRDATE,";
                SQL = SQL + ComNum.VBLF + "    A.CHARTDATE, A.CHARTTIME, A.USEID";
                SQL = SQL + ComNum.VBLF + "    FROM ADMIN.EMRXML A INNER JOIN ADMIN.EMRFORM B";
                SQL = SQL + ComNum.VBLF + "    ON A.FORMNO = B.FORMNO AND A.UPDATENO = B.UPDATENO";
                SQL = SQL + ComNum.VBLF + "    AND A.EMRNO = " + strEmrNo;

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

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

                strEmrNo = dt.Rows[0]["EMRNO"].ToString().Trim();
                strFormNo = dt.Rows[0]["FORMNO"].ToString().Trim();
                strUSERFORMNO = dt.Rows[0]["USERFORMNO"].ToString().Trim();
                strFormName = dt.Rows[0]["FORMNAME"].ToString().Trim();
                strChartDate = dt.Rows[0]["CHARTDATE"].ToString().Trim();
                strChartTime = dt.Rows[0]["CHARTTIME"].ToString().Trim();
                strUseId = dt.Rows[0]["USEID"].ToString().Trim();
                strMedFrDate = dt.Rows[0]["MEDFRDATE"].ToString().Trim();

                dt.Dispose();
                dt = null;

                if (strUSERFORMNO == "1")
                {
                    strRmk = GetProgText(strEmrNo);
                }

                chkSOAP0.Checked = false;
                chkSOAP1.Checked = false;

                dtpChartDate.Text = ComFunc.FormatStrToDate(strChartDate, "D");
                txtChartTime.Text = ComFunc.FormatStrToDate(strChartTime, "M");
                dtpChartDate.Enabled = false;
                txtChartTime.Enabled = false;
                txtProgress.Text = strRmk.Replace("\r\n", "\n").Replace("\n", "\r\n");
                txtProgress.Tag = txtProgress.Text;

                txtEmrNo.Text = strEmrNo;

                if (clsType.User.IdNumber != strUseId)
                {
                    txtEmrNo.Enabled = true;
                    mbtnSave.Visible = false;
                    mbtnDelete.Visible = false;
                    mbtnSaveImag.Visible = false;
                }
                else
                {
                    txtEmrNo.Enabled = true;
                    mbtnSave.Visible = true;
                    mbtnDelete.Visible = true;
                    mbtnSaveImag.Visible = true;
                }

                if (AcpEmr.inOutCls == "O")
                {
                    if (AcpEmr.medFrDate != strMedFrDate)
                    {
                        mbtnDelete.Visible = false;
                        mbtnSave.Visible = false;
                        mbtnSaveImag.Visible = false;
                    }
                    else
                    {
                        mbtnDelete.Visible = true;
                        mbtnSave.Visible = true;
                        mbtnSaveImag.Visible = true;
                    }
                }
                else
                {
                    mbtnDelete.Visible = true;
                    mbtnSave.Visible = true;
                    mbtnSaveImag.Visible = true;
                }
                tabEmr.SelectedTab = tabEmrWrite;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                //ComFunc.MsgBoxEx(this, ex.Message);
                //clsDB.SaveSqlErrLog(ex.Message, "", clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private void mbtnSaveImag_Click(object sender, EventArgs e)
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

                while (webImage.ReadyState != WebBrowserReadyState.Complete)
                {
                    if ((DateTime.Now - dtp).TotalMilliseconds >= 2000)
                    {
                        break;
                    }
                    Application.DoEvents();
                    //Application.DoEvents();
                    //Application.DoEvents();
                    //Application.DoEvents();
                    //Application.DoEvents();
                    //Application.DoEvents();
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

                SQL = " SELECT EMRNO FROM ADMIN.EMRXMLIMAGES";
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

                SQL = " INSERT INTO ADMIN.EMRXMLMST";
                SQL = SQL + ComNum.VBLF + " SELECT EMRNO, PTNO, '1', FORMNO, USEID,";
                SQL = SQL + ComNum.VBLF + " CHARTDATE, CHARTTIME, INOUTCLS, MEDFRDATE,";
                SQL = SQL + ComNum.VBLF + " MedFrTime , MedEndDate, MedEndTime, MedDeptCd, MedDrCd, writeDate, writeTime";
                SQL = SQL + ComNum.VBLF + "  From ADMIN.EMRXML";
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

        private bool SaveData()
        {
            if (clsType.User.DrCode == "")
            {
                ComFunc.MsgBoxEx(this, "의사만 작성이 가능합니다.");
                return false;
            }

            if (clsType.User.IdNumber.Trim() == "")
            {
                ComFunc.MsgBoxEx(this, "정상적인 사용이 아닙니다.");
                return false;
            }

            if (ComFunc.CheckTime(txtChartTime.Text.Trim()) == false)
            {
                ComFunc.MsgBoxEx(this, "작성시간 오류입니다.");
                return false;
            }

            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            strCurDateTime = strCurDateTime.Replace("-", "").Replace(":", "");
            string strCurDate = VB.Left(strCurDateTime, 8);
            string strCurTime = VB.Right(strCurDateTime, 6);

            if (VB.Val(txtEmrNo.Text) == 0)
            {
                if (AcpEmr.inOutCls == "O")
                {
                    if (AcpEmr.medFrDate != dtpChartDate.Value.ToString("yyyyMMdd"))
                    {
                        if (ComFunc.MsgBoxQ("외래진료일과 챠트 작성일이 다릅니다. 계속 진행하시겠습니까?", "PSMH", MessageBoxDefaultButton.Button2) == DialogResult.No)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if (AcpEmr.medEndDate != "")
                    {
                        if ((VB.Val(AcpEmr.medFrDate) > VB.Val(dtpChartDate.Value.ToString("yyyyMMdd"))) || (VB.Val(AcpEmr.medEndDate) < VB.Val(dtpChartDate.Value.ToString("yyyyMMdd"))))
                        {
                            ComFunc.MsgBoxEx(this, "작성일자 오류입니다. 입원기간내 작성요망");
                            return false;
                        }
                    }
                    else
                    {
                        if ((VB.Val(AcpEmr.medFrDate) > VB.Val(dtpChartDate.Value.ToString("yyyyMMdd"))) || (VB.Val(strCurDate) < VB.Val(dtpChartDate.Value.ToString("yyyyMMdd"))))
                        {
                            ComFunc.MsgBoxEx(this, "작성일자 오류입니다. 입원기간내 작성요망");
                            return false;
                        }
                    }
                }
            }

            if (fEmrMacro != null)
            {
                fEmrMacro.Dispose();
                fEmrMacro = null;
            }

            string strTemp = "";

            if (chkSOAP0.Checked == true)
            {
                strTemp = strTemp + "S)" + ComNum.VBLF + ssSOAP_Sheet1.Cells[0, 1].Text.Trim() + ComNum.VBLF + ComNum.VBLF;
                strTemp = strTemp + "O)" + ComNum.VBLF + ssSOAP_Sheet1.Cells[1, 1].Text.Trim() + ComNum.VBLF + ComNum.VBLF;
                strTemp = strTemp + "A)" + ComNum.VBLF + ssSOAP_Sheet1.Cells[2, 1].Text.Trim() + ComNum.VBLF + ComNum.VBLF;
                strTemp = strTemp + "P)" + ComNum.VBLF + ssSOAP_Sheet1.Cells[3, 1].Text.Trim() + ComNum.VBLF + ComNum.VBLF;
                txtProgress.Text = strTemp;
                ssSOAP.Visible = false;
            }
            else if (chkSOAP1.Checked == true)
            {
                strTemp = strTemp + "수술/시술명 및 수술/시술후 환자상태)" + ComNum.VBLF + ssSOAP_Sheet1.Cells[0, 1].Text.Trim() + ComNum.VBLF + ComNum.VBLF;
                strTemp = strTemp + "수술/시술 후 주요검사결과)" + ComNum.VBLF + ssSOAP_Sheet1.Cells[1, 1].Text.Trim() + ComNum.VBLF + ComNum.VBLF;
                strTemp = strTemp + "수술/시술후 발생가능한 문제점)" + ComNum.VBLF + ssSOAP_Sheet1.Cells[2, 1].Text.Trim() + ComNum.VBLF + ComNum.VBLF;
                strTemp = strTemp + "관찰 및 치료계획)" + ComNum.VBLF + ssSOAP_Sheet1.Cells[3, 1].Text.Trim() + ComNum.VBLF + ComNum.VBLF;
                txtProgress.Text = strTemp;
                ssSOAP.Visible = false;
            }

            double dblEmrNo = VB.Val(txtEmrNo.Text);
            string strChartUseId = "";

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strXML = "";
            string strXmlHead = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + "\r\n" + "<chart>" + "\r\n" + "<ta1 type=\"textArea\" label=\"Progress\"><![CDATA[";
            string strXmlTail = "]]></ta1>" + "\r\n" + "</chart>";

            string strProgress = ComFunc.QuotConv(txtProgress.Text.Trim());

            strXML = strXmlHead + strProgress + strXmlTail;

            string strCONTENTS = "(SELECT CONTENTS FROM ADMIN.EMRFORM WHERE FORMNO = " + VB.Val(mstrPROGNO) + ")";

            string strUPDATENO = "1";

            string strChartDate = dtpChartDate.Value.ToString("yyyyMMdd");
            string strChartTime = txtChartTime.Text.Replace(":", "");
            string strInOutCls = AcpEmr.inOutCls;
            string strMedFrDate = AcpEmr.medFrDate;
            string strMedFrTime = AcpEmr.medFrTime;
            string strMedEndDate = AcpEmr.medEndDate;
            string strMedEndTime = AcpEmr.medEndTime;
            string strMedDeptCd = AcpEmr.medDeptCd;
            string strMedDrCd = AcpEmr.medDrCd;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (dblEmrNo > 0)
                {
                    //이전내역을 가지고 와서 임시로 가지고 있는다.
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT  ";
                    SQL = SQL + ComNum.VBLF + "      EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                    SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD";
                    SQL = SQL + ComNum.VBLF + " FROM ADMIN.EMRXML";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + dblEmrNo;
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
                        ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                    strInOutCls = dt.Rows[i]["INOUTCLS"].ToString().Trim();
                    strMedFrDate = dt.Rows[i]["MEDFRDATE"].ToString().Trim();
                    strMedFrTime = dt.Rows[i]["MEDFRTIME"].ToString().Trim();
                    strMedEndDate = dt.Rows[i]["MEDENDDATE"].ToString().Trim();
                    strMedEndTime = dt.Rows[i]["MEDENDTIME"].ToString().Trim();
                    strMedDeptCd = dt.Rows[i]["MEDDEPTCD"].ToString().Trim();
                    strMedDrCd = dt.Rows[i]["MEDDRCD"].ToString().Trim();
                    strChartUseId = dt.Rows[0]["USEID"].ToString().Trim();
                    dt.Dispose();
                    dt = null;

                    #region //DeleteData

                    if (clsType.User.IdNumber != strChartUseId)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, "기존 작성자와 다릅니다." + ComNum.VBLF + "삭제할 수 없습니다.");
                        Cursor.Current = Cursors.Default;
                        return false;
                    }

                    double dblEmrHisNo = ComQuery.GetSequencesNo(clsDB.DbCon, "ADMIN.EMRXMLHISNO");

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " INSERT INTO ADMIN.EMRXMLHISTORY";
                    SQL = SQL + ComNum.VBLF + "      (HISTORYNO, EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                    SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,HISTORYWRITEDATE,HISTORYWRITETIME,DELUSEID,CERTNO)";
                    SQL = SQL + ComNum.VBLF + " SELECT  " + dblEmrHisNo + ",";
                    SQL = SQL + ComNum.VBLF + "      EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                    SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,";
                    SQL = SQL + ComNum.VBLF + "      '" + strCurDate + "',";
                    SQL = SQL + ComNum.VBLF + "      '" + strCurTime + "', '" + clsType.User.IdNumber + "',CERTNO";
                    SQL = SQL + ComNum.VBLF + " FROM ADMIN.EMRXML";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + dblEmrNo;
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " DELETE FROM ADMIN.EMRXML";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + dblEmrNo;
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " DELETE FROM ADMIN.EMRXMLMST";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + dblEmrNo;
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }

                    #endregion
                }

                double dblEmrNoNew = ComQuery.GetSequencesNo(clsDB.DbCon, "ADMIN.GetEmrXmlNo");
                if (strChartTime.Length < 6)
                {
                    strChartTime = strChartTime + "00";
                }

                #region //SaveData

                SQL = "";
                SQL = SQL + ComNum.VBLF + " INSERT INTO ADMIN.EMRXML";
                SQL = SQL + ComNum.VBLF + "      (EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD, MIBICHECK, ";
                SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,UPDATENO) ";
                SQL = SQL + ComNum.VBLF + "      VALUES (";
                SQL = SQL + ComNum.VBLF + "      " + dblEmrNoNew + ",";
                SQL = SQL + ComNum.VBLF + "      " + mstrPROGNO + ",";
                SQL = SQL + ComNum.VBLF + "      '" + clsType.User.IdNumber + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strChartDate + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strChartTime + "',";
                SQL = SQL + ComNum.VBLF + "      '" + "0" + "',";
                SQL = SQL + ComNum.VBLF + "      '" + AcpEmr.ptNo + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strInOutCls + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strMedFrDate + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strMedFrTime + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strMedEndDate + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strMedEndTime + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strMedDeptCd + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strMedDrCd + "',";
                SQL = SQL + ComNum.VBLF + "      '" + "0" + "',";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Left(strCurDateTime, 8) + "',";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Right(strCurDateTime, 6) + "',";
                SQL = SQL + ComNum.VBLF + "      :1,";
                SQL = SQL + ComNum.VBLF + "      '" + strUPDATENO + "')";
                SqlErr = clsDB.ExecuteXmlQuery(SQL, strXML, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                SQL = " INSERT INTO ADMIN.EMRXMLMST";
                SQL = SQL + ComNum.VBLF + " SELECT EMRNO, PTNO, '1', FORMNO, USEID,";
                SQL = SQL + ComNum.VBLF + " CHARTDATE, CHARTTIME, INOUTCLS, MEDFRDATE,";
                SQL = SQL + ComNum.VBLF + " MedFrTime , MedEndDate, MedEndTime, MedDeptCd, MedDrCd, writeDate, writeTime";
                SQL = SQL + ComNum.VBLF + "  From ADMIN.EMRXML";
                SQL = SQL + ComNum.VBLF + " Where EMRNO = " + dblEmrNoNew;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + "    UPDATE ADMIN.EMRMIBI SET WRITEDATE = '" + VB.Left(strCurDateTime, 8) + "', WRITETIME = '" + VB.Right(strCurDateTime, 6) + "'";
                SQL = SQL + ComNum.VBLF + "  WHERE PTNO = '" + AcpEmr.ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND MEDFRDATE = '" + strMedFrDate + "' ";
                SQL = SQL + ComNum.VBLF + "  AND MEDDRCD = '" + clsType.User.IdNumber + "' ";
                SQL = SQL + ComNum.VBLF + "  AND MIBICLS = '1' ";
                SQL = SQL + ComNum.VBLF + "  AND MIBIGRP = 'D' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                #endregion //SaveData


                clsDB.setCommitTran(clsDB.DbCon);

                mEmrNo = dblEmrNoNew.ToString();

                if (mSaveFlag == false)
                {
                    ComFunc.MsgBoxEx(this, "저장하였습니다.");
                }
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

        private void mbtnDelete_Click(object sender, EventArgs e)
        {

            if (VB.Val(txtEmrNo.Text) == 0)
            {
                return;
            }

            if (ComFunc.MsgBoxQ("기존내용을 삭제하시겠습니까?", "PSMH", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }
            double dblEmrNo = VB.Val(txtEmrNo.Text);

            if (DeleteDate(dblEmrNo) == true)
            {
                ClearForm();
                //SetPatInfoImg();
                GetChartHis();
            }
        }

        private bool DeleteDate(double dblEmrNo)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strChartUseId = "";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "SELECT ";
                SQL = SQL + ComNum.VBLF + "                A.EMRNO, A.USEID";
                SQL = SQL + ComNum.VBLF + "    FROM ADMIN.EMRXML A";
                SQL = SQL + ComNum.VBLF + "    WHERE A.EMRNO = " + dblEmrNo;

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
                    ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                strChartUseId = dt.Rows[0]["USEID"].ToString().Trim();
                dt.Dispose();
                dt = null;

                if (clsType.User.IdNumber != strChartUseId)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, "기존 작성자와 다릅니다." + ComNum.VBLF + "삭제할 수 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                strCurDateTime = strCurDateTime.Replace("-", "").Replace(":", "");

                double dblEmrHisNo = ComQuery.GetSequencesNo(clsDB.DbCon, "ADMIN.EMRXMLHISNO");

                SQL = "";
                SQL = SQL + ComNum.VBLF + " INSERT INTO ADMIN.EMRXMLHISTORY";
                SQL = SQL + ComNum.VBLF + "      (HISTORYNO, EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,HISTORYWRITEDATE,HISTORYWRITETIME,DELUSEID,CERTNO)";
                SQL = SQL + ComNum.VBLF + " SELECT  " + dblEmrHisNo + ",";
                SQL = SQL + ComNum.VBLF + "      EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Left(strCurDateTime, 8) + "',";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Right(strCurDateTime, 6) + "', '" + clsType.User.IdNumber + "',CERTNO";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.EMRXML";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + dblEmrNo;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE FROM ADMIN.EMRXML";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + dblEmrNo;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE FROM ADMIN.EMRXMLMST";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + dblEmrNo;
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
                ComFunc.MsgBoxEx(this, "저장하였습니다.");
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

        private void btnHelp_Click(object sender, EventArgs e)
        {
            GetSysmpList();
        }

        private void GetSysmpList()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssGRPMACRO_Sheet1.RowCount = 0;
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT SYSMPGB, SYSMPINDEX, SYSMPKEY, SYSMPNAME";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.EMRSYSMP";
                SQL = SQL + ComNum.VBLF + "          WHERE SYSMPGB = '" + mSYSMPGB + "'";
                SQL = SQL + ComNum.VBLF + "          AND SYSMPRMK IS NOT NULL";
                SQL = SQL + ComNum.VBLF + "          AND SYSMPNAME LIKE '%" + txtSysmp.Text.Trim() + "%'";
                SQL = SQL + ComNum.VBLF + "      ORDER BY SYSMPNAME";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

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

                if (dt.Rows.Count == 1)
                {
                    string strSYSMPINDEX = dt.Rows[0]["SYSMPINDEX"].ToString().Trim();
                    dt.Dispose();
                    dt = null;

                    GetMeCro(strSYSMPINDEX);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssGRPMACRO_Sheet1.RowCount = dt.Rows.Count;
                ssGRPMACRO_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssGRPMACRO_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SYSMPNAME"].ToString().Trim();
                    ssGRPMACRO_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SYSMPINDEX"].ToString().Trim();
                    ssGRPMACRO_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SYSMPNAME"].ToString().Trim();
                    ssGRPMACRO_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SYSMPKEY"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

                ssGRPMACRO.Visible = true;
                ssGRPMACRO.BringToFront();
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void GetMeCro(string strSYSMPINDEX)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT SYSMPRMK";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.EMRSYSMP";
                SQL = SQL + ComNum.VBLF + "      WHERE SYSMPINDEX = " + VB.Val(strSYSMPINDEX);

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

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

                if (chkAdd.Checked == true)
                {
                    txtProgress.Text = txtProgress.Text + " " + dt.Rows[0]["SYSMPRMK"].ToString().Trim();
                }
                else
                {
                    txtProgress.Text = dt.Rows[0]["SYSMPRMK"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void ssGRPMACRO_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssGRPMACRO_Sheet1.RowCount == 0)
                return;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssGRPMACRO, e.Column);
                return;
            }
            string strSYSMPINDEX = ssGRPMACRO_Sheet1.Cells[e.Row, 1].Text.Trim();

            GetMeCro(strSYSMPINDEX);
        }

        private void ssGRPMACRO_Leave(object sender, EventArgs e)
        {
            ssGRPMACRO.Visible = false;
        }

        private void optInOut3_CheckedChanged(object sender, EventArgs e)
        {
            if (clsEmrQueryOld.EmrSaveUserOption(clsDB.DbCon, "OOORD", "OPTKIND", "0") == false)
            {

            }
        }

        private void optInOut1_CheckedChanged(object sender, EventArgs e)
        {
            if (clsEmrQueryOld.EmrSaveUserOption(clsDB.DbCon, "OOORD", "OPTKIND", "1") == false)
            {

            }
        }

        private void optInOut2_CheckedChanged(object sender, EventArgs e)
        {
            if (clsEmrQueryOld.EmrSaveUserOption(clsDB.DbCon, "OOORD", "OPTKIND", "2") == false)
            {

            }
        }

        private void optAll_CheckedChanged(object sender, EventArgs e)
        {
            //mOption = "H";
            mSYSMPGB = "ALL";
            if (clsEmrQueryOld.EmrSaveUserOption(clsDB.DbCon, "OOORD", "OPTMCRO", "2") == false)
            {

            }
        }

        private void optDept_CheckedChanged(object sender, EventArgs e)
        {
            //mOption = "D";
            mSYSMPGB = clsType.User.DeptCode;
            if (clsEmrQueryOld.EmrSaveUserOption(clsDB.DbCon, "OOORD", "OPTMCRO", "1") == false)
            {

            }
        }

        private void optUse_CheckedChanged(object sender, EventArgs e)
        {
            //mOption = "U";
            mSYSMPGB = clsType.User.IdNumber;
            if (clsEmrQueryOld.EmrSaveUserOption(clsDB.DbCon, "OOORD", "OPTMCRO", "0") == false)
            {

            }
        }

        private void opSortAs_CheckedChanged(object sender, EventArgs e)
        {
            if (clsEmrQueryOld.EmrSaveUserOption(clsDB.DbCon, "OOORD", "OPTSORT", "1") == false)
            {

            }
        }

        private void opSortDs_CheckedChanged(object sender, EventArgs e)
        {
            if (clsEmrQueryOld.EmrSaveUserOption(clsDB.DbCon, "OOORD", "OPTSORT", "0") == false)
            {

            }
        }

        private void txtSysmp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return)
                return;

            GetSysmpList();
        }

        private void txtProgress_KeyDown(object sender, KeyEventArgs e)
        {
            string strText = "";
            if (e.KeyCode == Keys.F2)
            {
                strText = ((TextBox)sender).SelectedText.Trim();
                strText = EngHanConv.Eng2Kor(strText);
                ((TextBox)sender).SelectedText = strText;
            }
            else if (e.KeyCode == Keys.F3)
            {
                strText = ((TextBox)sender).SelectedText.Trim();
                strText = EngHanConv.Kor2Eng(strText);
                ((TextBox)sender).SelectedText = strText;
            }
        }

        private void lblKorE_Click(object sender, EventArgs e)
        {
            if (lblKorE.Text == "영어")
            {
                ComFunc.SetIMEMODE(this, "K");
                lblKorE.Text = "한글";
            }
            else
            {
                ComFunc.SetIMEMODE(this, "E");
                lblKorE.Text = "영어";
            }
        }

        private void txtProgress_ImeModeChanged(object sender, EventArgs e)
        {
            if (txtProgress.ImeMode == ImeMode.Hangul)
            {
                lblKorE.Text = "한글";
            }
            else
            {
                lblKorE.Text = "영어";
            }
        }

        private void frmEmrBaseProgressOcs_Resize(object sender, EventArgs e)
        {
            if (IsLoadForm == true)
                return;

            try
            {

            }
            catch
            {

            }
        }

        private void dtpSDate_ValueChanged(object sender, EventArgs e)
        {
            //if (clsEmrQueryOld.EmrSaveUserOption(clsDB.DbCon, "OOORD", "DTPSDATE", dtpSDate.Value.ToString("yyyy-MM-dd")) == false)
            //{

            //}
        }
    }
}
