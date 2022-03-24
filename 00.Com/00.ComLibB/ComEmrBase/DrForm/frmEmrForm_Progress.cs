using System;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrForm_Progress : Form
    {

        #region // 폼에 사용하는 변수를 코딩하는 부분
        //private frmEmrInitList frmEmrInitListEvent;
        #endregion

        #region // 기록지 관련 변수(기록지번호, 명칭 등 수정요함)
        public FormEmrMessage mEmrCallForm;

        public string mstrFormNo = "963";
        public string mstrUpdateNo = "0";
        public string mstrFormText = "";
        public EmrPatient p = null;
        public string mstrEmrNo = "0";
        public string mstrMode = "W";
        #endregion

        #region // 이전 서식 관련
        //string mInOutGb = "";
        //string mstrPROGNO = "";
        //string mstrPROGNAME = "";
        //string mstrPROGIMGNO = "";
        //string mstrPROGIMGNAME = "";
        //string mstrUserChoJinForm = "";
        //string mstrUserChoJinFormName = "";
        //string mstrUserDept = "";
        //string mOption = "";
        //string mSYSMPGB = "";
        //string mEDIT = "";  //fstrEDIT
        //string mClick = ""; //FstrClick
        //bool mSaveFlag = false; //외부에서 저장을 요청한경우
        //bool mIsFirtQuery = false; //초진차트 작성 두번 보이는 것 안보이게

        //string mEmrNo = "0"; //Progress 
        //string mEmrImageNo = "0"; //Image

        //Form CallForm = null;
        //frmEmrBaseSympOld fEmrMacro = null;
        //frmEmrBaseEmrChartOld fEmrChart = null;

        //EmrPatient AcpEmr = null;
        #endregion


        //#region //Public Function
        ///// <summary>
        ///// 사용자 정보가 바뀐 경우 : 사용자별 환경 설정을 다시 한다
        ///// </summary>
        //public void SetUserInfo()
        //{
        //    ClearForm();
        //    SetUserOption();
        //}

        ///// <summary>
        ///// 환자정보가 바뀔 경우 : 환자 정보를 갱신한다
        ///// </summary>
        ///// <param name="AcpEmr"></param>
        //public void SetPatInfo(EmrPatient pAcpEmr)
        //{
        //    AcpEmr = pAcpEmr;
        //    mIsFirtQuery = true;
        //    ClearForm();

        //    GetPatRmk();
        //    GetMibi();
        //    SetPatInfoImg();
        //    GetChartHis();

        //    string strEmrOption = "";
        //    strEmrOption = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "EMRKORENG");
        //    if (VB.Val(strEmrOption) == 1)
        //    {
        //        ComFunc.SetIMEMODE(this, "E");
        //        lblKorE.Text = "영어";
        //    }
        //    else
        //    {
        //        ComFunc.SetIMEMODE(this, "K");
        //        lblKorE.Text = "한글";
        //    }

        //    strEmrOption = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "AFTERPROGSAVE");
        //    if (VB.Val(strEmrOption) == 1)
        //    {
        //        tabEmr.SelectedTab = tabEmrWrite;
        //    }
        //    else
        //    {
        //        tabEmr.SelectedTab = tabEmrView;
        //    }

        //    //환자의 이전 프로그래스 내역 불러오기
        //    strEmrOption = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "PROGGETHIS");
        //    if (VB.Val(strEmrOption) == 1)
        //    {
        //        GetSetProgHis();
        //    }
        //}

        //private void GetSetProgHis()
        //{
        //    DataTable dt = null;
        //    string SQL = "";    //Query문
        //    string SqlErr = ""; //에러문 받는 변수
        //    string strEMRNO = "0";
        //    string strCHARTA = "";

        //    try
        //    {
        //        SQL = "";
        //        SQL = SQL + ComNum.VBLF + "SELECT  ";
        //        SQL = SQL + ComNum.VBLF + "    MAX(M1.EMRNO) AS EMRNO  ";
        //        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMRXMLMST M1 ";
        //        SQL = SQL + ComNum.VBLF + "WHERE M1.PTNO = '" + AcpEmr.ptNo + "' ";
        //        SQL = SQL + ComNum.VBLF + "    AND M1.MEDFRDATE = '" + AcpEmr.medFrDate.Replace("-", "") + "' ";
        //        SQL = SQL + ComNum.VBLF + "    AND M1.MEDDEPTCD = '" + AcpEmr.medDeptCd + "' ";
        //        SQL = SQL + ComNum.VBLF + "    AND M1.INOUTCLS = 'O' ";
        //        SQL = SQL + ComNum.VBLF + "    AND M1.FORMNO = 963 ";
        //        SQL = SQL + ComNum.VBLF + "    AND M1.USEID = '" + clsType.User.IdNumber + "' ";

        //        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

        //        if (SqlErr != "")
        //        {
        //            ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
        //            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
        //            Cursor.Current = Cursors.Default;
        //            return;
        //        }
        //        if (dt.Rows.Count > 0)
        //        {
        //            strEMRNO = dt.Rows[0]["EMRNO"].ToString().Trim();

        //            dt.Dispose();
        //            dt = null;
        //            if (VB.Val(strEMRNO) > 0)
        //            {
        //                Cursor.Current = Cursors.Default;
        //                SetProgressOne(strEMRNO);
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            dt.Dispose();
        //            dt = null;
        //        }

        //        SQL = "";
        //        SQL = " SELECT ";
        //        SQL = SQL + ComNum.VBLF + "    CHARTXML AS CHARTA ";
        //        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMRXML ";
        //        SQL = SQL + ComNum.VBLF + "WHERE EMRNO = (  ";
        //        SQL = SQL + ComNum.VBLF + "            SELECT  ";
        //        SQL = SQL + ComNum.VBLF + "                MAX(M1.EMRNO)  ";
        //        SQL = SQL + ComNum.VBLF + "            FROM " + ComNum.DB_EMR + "EMRXMLMST M1 ";
        //        SQL = SQL + ComNum.VBLF + "            WHERE M1.PTNO = '" + AcpEmr.ptNo + "' ";
        //        SQL = SQL + ComNum.VBLF + "                AND M1.MEDFRDATE < '" + AcpEmr.medFrDate.Replace("-", "") + "' ";
        //        SQL = SQL + ComNum.VBLF + "                AND M1.MEDDEPTCD = '" + AcpEmr.medDeptCd + "' ";
        //        SQL = SQL + ComNum.VBLF + "                AND M1.INOUTCLS = 'O' ";
        //        SQL = SQL + ComNum.VBLF + "                AND M1.FORMNO = 963 ";
        //        SQL = SQL + ComNum.VBLF + "                AND M1.CHARTDATE = (SELECT MAX(M1.CHARTDATE)  ";
        //        SQL = SQL + ComNum.VBLF + "                                        FROM " + ComNum.DB_EMR + "EMRXMLMST M1 ";
        //        SQL = SQL + ComNum.VBLF + "                                        WHERE M1.PTNO = '" + AcpEmr.ptNo + "' ";
        //        SQL = SQL + ComNum.VBLF + "                                            AND M1.MEDFRDATE < '" + AcpEmr.medFrDate.Replace("-", "") + "' ";
        //        SQL = SQL + ComNum.VBLF + "                                            AND M1.MEDDEPTCD = '" + AcpEmr.medDeptCd + "' ";
        //        SQL = SQL + ComNum.VBLF + "                                            AND M1.INOUTCLS = 'O' ";
        //        SQL = SQL + ComNum.VBLF + "                                            AND M1.FORMNO = 963) ";
        //        SQL = SQL + ComNum.VBLF + "                ) ";

        //        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

        //        if (SqlErr != "")
        //        {
        //            ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
        //            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
        //            Cursor.Current = Cursors.Default;
        //            return;
        //        }
        //        if (dt.Rows.Count == 0)
        //        {
        //            dt.Dispose();
        //            dt = null;
        //            Cursor.Current = Cursors.Default;
        //            return;
        //        }
        //        strCHARTA = MakeContentValue(dt.Rows[0]["CHARTA"].ToString().Trim());
        //        dt.Dispose();
        //        dt = null;

        //        txtProgress.Text = strCHARTA.Replace("\r\n", "\n").Replace("\n", "\r\n");

        //        tabEmr.SelectedTab = tabEmrWrite;
        //    }
        //    catch (Exception ex)
        //    {
        //        ComFunc.MsgBoxEx(this, ex.Message);
        //        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
        //    }
        //}

        //public void ClearPatInfo()
        //{
        //    AcpEmr = null;
        //    ClearColtrol();

        //    if (fEmrMacro != null)
        //    {
        //        fEmrMacro.Dispose();
        //        fEmrMacro = null;
        //    }
        //    if (fEmrChart != null)
        //    {
        //        fEmrChart.Dispose();
        //        fEmrChart = null;
        //    }
        //}

        //private void ClearColtrol()
        //{
        //    mEDIT = "";
        //    mEmrImageNo = "0";

        //    chkSOAP0.Checked = false;
        //    chkSOAP1.Checked = false;

        //    mbtnDelete.Visible = true;
        //    mbtnSave.Visible = true;
        //    mbtnSaveImag.Visible = true;
        //    btnMibi.Visible = false;

        //    dtpChartDate.Enabled = true;
        //    txtChartTime.Enabled = true;
        //    txtEmrNo.Enabled = false;

        //    txtEmrNo.Text = "";
        //    txtProgress.Text = "";
        //    txtProgress.Tag = null;

        //    panProg.Height = 150;

        //    btnSearchRmk.BackColor = Color.White;
        //    imgRmk.Visible = false;
        //    imgRmk.Tag = null;
        //    toolRmk.Tag = null;

        //    webEMR.Navigate(EmrUrlMain);
        //    Application.DoEvents();

        //    webImage.Navigate(EmrUrlMain);
        //    Application.DoEvents();

        //    //저장후 SOAP 클리어
        //    string strEmrOption = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "PROGSOAPCLEAR");
        //    if (VB.Val(strEmrOption) == 1)
        //    {
        //        ssSOAP_Sheet1.Cells[0, 1].Text = "";
        //        ssSOAP_Sheet1.Cells[1, 1].Text = "";
        //        ssSOAP_Sheet1.Cells[2, 1].Text = "";
        //        ssSOAP_Sheet1.Cells[3, 1].Text = "";
        //    }
        //}

        ///// <summary>
        ///// 기록지를 저장한다.
        ///// </summary>
        //public void SetSaveData()
        //{
        //    mSaveFlag = true;

        //    if (SaveData() == true)
        //    {

        //    }
        //}

        //public bool CheckAndSaveEmr()
        //{
        //    bool rtnVal = true;

        //    bool isChange = false;

        //    if (fEmrMacro != null)
        //    {
        //        fEmrMacro.Dispose();
        //        fEmrMacro = null;
        //    }

        //    if (VB.Val(txtEmrNo.Text) != 0)
        //    {
        //        if (txtProgress.Tag == null)
        //        {
        //            if (txtProgress.Text.Trim() != "")
        //            {
        //                isChange = true;
        //            }
        //        }
        //        else
        //        {
        //            if (txtProgress.Text.Trim() != txtProgress.Tag.ToString().Trim())
        //            {
        //                isChange = true;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (txtProgress.Text.Trim() != "")
        //        {
        //            isChange = true;
        //        }
        //        if (chkSOAP0.Checked == true || chkSOAP1.Checked == true)
        //        {
        //            isChange = true;
        //        }
        //    }

        //    if (isChange == false)
        //    {
        //        return true;
        //    }

        //    if (ComFunc.MsgBoxQ("EMR 차트 변경 내역이 있습니다." + ComNum.VBLF + "저장하시겠습니까?", "알림", MessageBoxDefaultButton.Button1) == DialogResult.No)
        //    {
        //        return true;
        //    }

        //    mSaveFlag = true;

        //    rtnVal = SaveData();

        //    if (rtnVal == true)
        //    {
        //        ClearForm();
        //    }

        //    return rtnVal;
        //}

        //#endregion //Public Function

        //#region //Private Function

        //private void WebLogin()
        //{
        //    //SetPatInfoImg();

        //    //clsType.User.Passhash256 = "5857b0bd2c33cea36a79dba3e1f17e225e0dad5976b53a7f4b790e9694bcd028";

        //    string strURL = "";
        //    string strUseId = clsType.User.IdNumber;
        //    string strPw = clsType.User.Passhash256;

        //    //webImage.Navigate("http://192.168.100.33:8090/Emr/emrxmlInfo.mts?ptNo=&acpNo=&inOutCls=&medFrDate=&medFrTime=&medEndDate=&medEndTime=&medDeptCd=&medDeptName=&medDrCd=&gubun=3&formNo=");
        //    //while (webImage.IsBusy == true)
        //    //{
        //    //    Application.DoEvents();
        //    //}

        //    //webEMR.Navigate("http://192.168.100.33:8090/Emr/emrxmlInfo.mts?ptNo=&acpNo=&inOutCls=&medFrDate=&medFrTime=&medEndDate=&medEndTime=&medDeptCd=&medDeptName=&medDrCd=&gubun=3&formNo=");
        //    //while (webImage.IsBusy == true)
        //    //{
        //    //    Application.DoEvents();
        //    //}


        //    http://192.168.100.33:8090/Emr/doLogin.mts?useId=15200&password=5857b0bd2c33cea36a79dba3e1f17e225e0dad5976b53a7f4b790e9694bcd028&loginType=vb
        //    webEMR.Navigate("http://192.168.100.33:8090/Emr/doLogin.mts?useId=15200&password=5857b0bd2c33cea36a79dba3e1f17e225e0dad5976b53a7f4b790e9694bcd028&loginType=vb");

        //    strURL = gEmrUrl + "/doLogin.mts?useId=" + strUseId + "&password=" + strPw + "&loginType=vb";
        //    webEMR.Navigate(strURL);
        //    while (webEMR.IsBusy == true)
        //    {
        //        Application.DoEvents();
        //    }
        //    for (int intWeb = 0; intWeb < 40000; intWeb++)
        //    {
        //        Application.DoEvents();
        //    }

        //    ////http://192.168.100.33:8090/Emr/doLogin.mts?useId=15200&password=5857b0bd2c33cea36a79dba3e1f17e225e0dad5976b53a7f4b790e9694bcd028&loginType=vb
        //    //webImage.Navigate("http://192.168.100.33:8090/Emr/doLogin.mts?useId=15200&password=5857b0bd2c33cea36a79dba3e1f17e225e0dad5976b53a7f4b790e9694bcd028&loginType=vb");

        //    strURL = gEmrUrl + "/doLogin.mts?useId=" + strUseId + "&password=" + strPw + "&loginType=vb";
        //    webImage.Navigate(strURL);
        //    while (webImage.IsBusy == true)
        //    {
        //        Application.DoEvents();
        //    }
        //    for (int intWeb = 0; intWeb < 40000; intWeb++)
        //    {
        //        Application.DoEvents();
        //    }
        //}

        //private void GetPatRmk()
        //{
        //    imgRmk.Visible = false;
        //    imgRmk.Tag = null;
        //    toolRmk.Tag = null;
        //    btnSearchRmk.BackColor = Color.White;

        //    if (AcpEmr == null)
        //    {
        //        return;
        //    }

        //    string strBad = GetPatInfoBad(AcpEmr.ptNo);
        //    string strGood = GetPatInfoGood(AcpEmr.ptNo);

        //    imgRmk.Tag = strBad;
        //    toolRmk.Tag = strBad;

        //    if (strBad != "" || imgRmk.Tag.ToString() != "")
        //    {
        //        btnSearchRmk.BackColor = Color.Yellow;
        //    }

        //    if (imgRmk.Tag.ToString() != "")
        //    {
        //        imgRmk.Visible = true;
        //    }
        //}

        //private void GetMibi()
        //{
        //    DataTable dt = null;
        //    string SQL = "";    //Query문
        //    string SqlErr = ""; //에러문 받는 변수
        //    btnMibi.Visible = false;

        //    try
        //    {
        //        SQL = " SELECT A.PTNO, B.SNAME AS PTNAME, ";
        //        SQL = SQL + ComNum.VBLF + "        A.MEDFRDATE, A.MEDENDDATE, A.MIBIGRP, A.MIBICD, A.MIBIRMK ";
        //        SQL = SQL + ComNum.VBLF + "    FROM ADMIN.EMRMIBI A, ADMIN.BAS_PATIENT B ";
        //        SQL = SQL + ComNum.VBLF + "    WHERE A.MEDDEPTCD = '" + clsType.User.DeptCode + "' ";
        //        SQL = SQL + ComNum.VBLF + "    AND A.MEDDRCD = '" + clsType.User.IdNumber + "' ";
        //        SQL = SQL + ComNum.VBLF + "    AND A.MIBICLS = 1";
        //        SQL = SQL + ComNum.VBLF + "    AND A.MIBIFNDATE IS NULL";
        //        SQL = SQL + ComNum.VBLF + "    AND A.PTNO = B.PANO(+) ";
        //        SQL = SQL + ComNum.VBLF + "    ORDER BY B.SNAME, A.MEDFRDATE, A.MIBIGRP, MIBICD ";

        //        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

        //        if (SqlErr != "")
        //        {
        //            ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
        //            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
        //            Cursor.Current = Cursors.Default;
        //            return;
        //        }
        //        if (dt.Rows.Count == 0)
        //        {
        //            dt.Dispose();
        //            dt = null;
        //            Cursor.Current = Cursors.Default;
        //            return;
        //        }
        //        dt.Dispose();
        //        dt = null;
        //        btnMibi.Visible = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        ComFunc.MsgBoxEx(this, ex.Message);
        //        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
        //        return;
        //    }
        //}

        //private string GetPatInfoBad(string strPtNo)
        //{
        //    string rtnVal = "";

        //    DataTable dt = null;
        //    string SQL = "";    //Query문
        //    string SqlErr = ""; //에러문 받는 변수

        //    try
        //    {
        //        SQL = SQL + ComNum.VBLF + " SELECT REMARK ";
        //        SQL = SQL + ComNum.VBLF + " FROM ADMIN.SGLRMK ";
        //        SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPtNo + "'";
        //        SQL = SQL + ComNum.VBLF + " AND GUBUN = '1'";
        //        SQL = SQL + ComNum.VBLF + " ORDER BY INPDATE DESC";

        //        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

        //        if (SqlErr != "")
        //        {
        //            ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
        //            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
        //            Cursor.Current = Cursors.Default;
        //            return rtnVal;
        //        }
        //        if (dt.Rows.Count == 0)
        //        {
        //            dt.Dispose();
        //            dt = null;
        //            Cursor.Current = Cursors.Default;
        //            return rtnVal;
        //        }
        //        rtnVal = (VB.Left(dt.Rows[0]["REMARK"].ToString().Trim(), 20)).Trim();
        //        dt.Dispose();
        //        dt = null;
        //        return rtnVal;
        //    }
        //    catch (Exception ex)
        //    {
        //        ComFunc.MsgBoxEx(this, ex.Message);
        //        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
        //        return rtnVal;
        //    }
        //}

        //private string GetPatInfoGood(string strPtNo)
        //{
        //    string rtnVal = "";

        //    DataTable dt = null;
        //    string SQL = "";    //Query문
        //    string SqlErr = ""; //에러문 받는 변수

        //    try
        //    {
        //        SQL = SQL + ComNum.VBLF + " SELECT REMARK ";
        //        SQL = SQL + ComNum.VBLF + " FROM ADMIN.SGLRMK ";
        //        SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPtNo + "'";
        //        SQL = SQL + ComNum.VBLF + " AND GUBUN = '0'";
        //        SQL = SQL + ComNum.VBLF + " ORDER BY INPDATE DESC";

        //        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

        //        if (SqlErr != "")
        //        {
        //            ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
        //            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
        //            Cursor.Current = Cursors.Default;
        //            return rtnVal;
        //        }
        //        if (dt.Rows.Count == 0)
        //        {
        //            dt.Dispose();
        //            dt = null;
        //            Cursor.Current = Cursors.Default;
        //            return rtnVal;
        //        }
        //        rtnVal = (VB.Left(dt.Rows[0]["REMARK"].ToString().Trim(), 20)).Trim();
        //        dt.Dispose();
        //        dt = null;
        //        return rtnVal;
        //    }
        //    catch (Exception ex)
        //    {
        //        ComFunc.MsgBoxEx(this, ex.Message);
        //        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
        //        return rtnVal;
        //    }
        //}

        //private void SetPatInfoImg()
        //{
        //    if (AcpEmr == null)
        //    {
        //        return;
        //    }

        //    string strURL = "";

        //    string strMedEndDate = "";
        //    string strMedEndTime = "";

        //    if (AcpEmr.inOutCls == "I")
        //    {
        //        strMedEndDate = AcpEmr.medEndDate;
        //        strMedEndTime = AcpEmr.medEndTime;
        //    }

        //    //http://192.168.100.33:8090/Emr/emrxmlInfo.mts?ptNo=03983614&acpNo=0&inOutCls=O&medFrDate=20170803&medFrTime=114500&medEndDate=&medEndTime=&medDeptCd=MN&medDeptName=&medDrCd=0503&gubun=3&formNo=
        //    //http://192.168.100.33:8090/Emr/emrxmlInfo.mts?ptNo=03983614&acpNo=0&inOutCls=O&medFrDate=20170803&medFrTime=114500&medEndDate=&medEndTime=&medDeptCd=MN&medDeptName=&medDrCd=0503&gubun=3&formNo=
        //    strURL = EmrUrlPatSend +
        //           "ptNo=" + AcpEmr.ptNo +
        //           "&acpNo=" + AcpEmr.acpNo +
        //           "&inOutCls=" + AcpEmr.inOutCls +
        //           "&medFrDate=" + AcpEmr.medFrDate +
        //           "&medFrTime=" + AcpEmr.medFrTime +
        //           "&medEndDate=" + strMedEndDate +
        //           "&medEndTime=" + strMedEndTime +
        //           "&medDeptCd=" + AcpEmr.medDeptCd +
        //           "&medDeptName=" + "" +
        //           "&medDrCd=" + AcpEmr.medDrCd +
        //           "&gubun=" + "3" +
        //           "&formNo=";
        //    //strURL = "http://192.168.100.33:8090/Emr/emrxmlInfo.mts?ptNo=02487371&acpNo=0&inOutCls=O&medFrDate=20180226&medFrTime=141800&medEndDate=&medEndTime=&medDeptCd=GS&medDeptName=&medDrCd=2121&gubun=3&formNo=";
        //    webImage.Navigate(strURL); //한장씩 볼 경우
        //    while (webImage.IsBusy == true)
        //    {
        //        Application.DoEvents();
        //    }
        //    for (int intWeb = 0; intWeb < 40000; intWeb++)
        //    {
        //        Application.DoEvents();
        //    }

        //    webImage.Navigate(EmrUrlImage);
        //    while (webImage.IsBusy == true)
        //    {
        //        Application.DoEvents();
        //    }
        //    for (int intWeb = 0; intWeb < 40000; intWeb++)
        //    {
        //        Application.DoEvents();
        //    }

        //    //http://192.168.100.33:8090/Emr/emrxmlInfo.mts?ptNo=03983614&acpNo=0&inOutCls=O&medFrDate=20170803&medFrTime=114500&medEndDate=&medEndTime=&medDeptCd=MN&medDeptName=&medDrCd=0503&gubun=3&formNo=
        //    //http://192.168.100.33:8090/Emr/emrxmlInfo.mts?ptNo=03983614&acpNo=0&inOutCls=O&medFrDate=20170803&medFrTime=114500&medEndDate=&medEndTime=&medDeptCd=MN&medDeptName=&medDrCd=0503&gubun=3&formNo=
        //    strURL = EmrUrlPatSend +
        //           "ptNo=" + AcpEmr.ptNo +
        //           "&acpNo=" + AcpEmr.acpNo +
        //           "&inOutCls=" + AcpEmr.inOutCls +
        //           "&medFrDate=" + AcpEmr.medFrDate +
        //           "&medFrTime=" + AcpEmr.medFrTime +
        //           "&medEndDate=" + strMedEndDate +
        //           "&medEndTime=" + strMedEndTime +
        //           "&medDeptCd=" + AcpEmr.medDeptCd +
        //           "&medDeptName=" + "" +
        //           "&medDrCd=" + AcpEmr.medDrCd +
        //           "&gubun=" + "3" +
        //           "&formNo=";

        //    //strURL = "http://192.168.100.33:8090/Emr/emrxmlInfo.mts?ptNo=02487371&acpNo=0&inOutCls=O&medFrDate=20180226&medFrTime=141800&medEndDate=&medEndTime=&medDeptCd=GS&medDeptName=&medDrCd=2121&gubun=3&formNo=";
        //    webEMR.Navigate(strURL); //한장씩 볼 경우
        //    while (webEMR.IsBusy == true)
        //    {
        //        Application.DoEvents();
        //    }
        //    for (int intWeb = 0; intWeb < 40000; intWeb++)
        //    {
        //        Application.DoEvents();
        //    }
        //}

        //private void GetChartHis()
        //{
        //    if (AcpEmr == null)
        //    {
        //        return;
        //    }

        //    //의사가 아닐경우 연속보기 사용안함
        //    if (clsType.User.DrCode == "")
        //    {
        //        return;
        //    }


        //    string strURL = "";
        //    //string strEmrNo = "74761596";
        //    //webEMR.Navigate(gEmrUrl + "/emrView.mts?emrNo=" + strEmrNo); //한장씩 볼 경우

        //    DataTable dt = null;
        //    string SQL = "";    //Query문
        //    string SqlErr = ""; //에러문 받는 변수

        //    string strCurDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
        //    int intRowCnt = 0;

        //    if (mIsFirtQuery == true)
        //    {
        //        SQL = " SELECT PANO";
        //        SQL = SQL + ComNum.VBLF + "  FROM ADMIN.OPD_MASTER";
        //        SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + AcpEmr.ptNo + "'";
        //        SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + AcpEmr.medDeptCd + "'";
        //        SQL = SQL + ComNum.VBLF + "   AND BDATE < TO_DATE('" + strCurDate + "','YYYY-MM-DD')";
        //        SQL = SQL + ComNum.VBLF + "   AND ROWNUM = 1";

        //        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

        //        if (SqlErr != "")
        //        {
        //            ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
        //            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
        //            return;
        //        }
        //        intRowCnt = dt.Rows.Count;
        //        dt.Dispose();
        //        dt = null;

        //        if (intRowCnt == 0)
        //        {
        //            intRowCnt = 0;
        //            SQL = " SELECT A.EMRNO";
        //            SQL = SQL + ComNum.VBLF + " FROM ADMIN.EMRXMLMST A, ADMIN.EMRGRPFORM B, ADMIN.EMRFORM C";
        //            SQL = SQL + ComNum.VBLF + " WHERE C.GRPFORMNO = b.GRPFORMNO";
        //            SQL = SQL + ComNum.VBLF + "   AND A.FORMNO = C.FORMNO";
        //            SQL = SQL + ComNum.VBLF + "   AND B.GRPFORMNO IN (27,2)";
        //            SQL = SQL + ComNum.VBLF + "   AND PTNO = '" + AcpEmr.ptNo + "'";
        //            SQL = SQL + ComNum.VBLF + "   AND A.CHARTDATE = '" + strCurDate + "'";
        //            SQL = SQL + ComNum.VBLF + "   AND A.MEDDEPTCD = '" + AcpEmr.medDeptCd + "' ";
        //            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

        //            if (SqlErr != "")
        //            {
        //                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
        //                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
        //                return;
        //            }
        //            intRowCnt = dt.Rows.Count;
        //            dt.Dispose();
        //            dt = null;

        //            if (intRowCnt == 0)
        //            {
        //                ComFunc.MsgBoxEx(this, "해당 진료과에 처음 진료받는 환자입니다. 초진기록지를 작성하여 주시기 바랍니다.");
        //            }
        //        }
        //    }

        //    mIsFirtQuery = false;

        //    #region //경과기록지 보기

        //    string pPtPtNo = AcpEmr.ptNo;
        //    string pInOutCls = "";
        //    string pMedDeptCd = "";
        //    string pMedFrDate = "";
        //    string pMedEndDate = "";
        //    string pSort = "";
        //    string pGUBUN = "";

        //    if (optInOut1.Checked == true)
        //    {
        //        pInOutCls = "1";
        //    }
        //    else if (optInOut2.Checked == true)
        //    {
        //        pInOutCls = "2";
        //    }
        //    else if (optInOut3.Checked == true)
        //    {
        //        pInOutCls = "0";
        //    }

        //    if (chkUserOpt.Checked == false)
        //    {
        //        pMedDeptCd = clsType.User.DeptCode;
        //        pGUBUN = "1";
        //    }
        //    else
        //    {
        //        pMedDeptCd = mstrUserDept.Replace("'", "").Replace(",", "^");
        //        pGUBUN = "2";
        //    }
        //    string strFirstDATE = "";
        //    ReadFirstDate(ref strFirstDATE);

        //    string strDTPSDate = "";
        //    string strDTPEDATE = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        //    strDTPSDate = dtpSDate.Value.ToString("yyyyMMdd");

        //    pMedFrDate = dtpSDate.Value.ToString("yyyyMMdd");
        //    pMedEndDate = dtpEDate.Value.ToString("yyyyMMdd");

        //    if (clsEmrQueryOld.READ_FM_LIMIT(AcpEmr.ptNo, AcpEmr.medDrCd, "") == true)
        //    {
        //        pMedDeptCd = VB.Replace(pMedDeptCd, "FM^", "");
        //    }

        //    switch (pMedDeptCd)
        //    {
        //        case "MC":
        //        case "MD":
        //        case "ME":
        //        case "MG":
        //        case "MI":
        //        case "MN":
        //        case "MP":
        //        case "MR":
        //            pMedDeptCd = "MD^MG^MC^MP^ME^MN^MR^MI^";
        //            break;
        //        default:

        //            break;
        //    }

        //    if (opSortAs.Checked == true)
        //    {
        //        pSort = "1";
        //    }
        //    else
        //    {
        //        pSort = "0";
        //    }

        //    //http://192.168.100.33:8090/Emr/grpFormNoByProgress.mts?grpFormNo=2&ptNo=03983614&inOutCls=0&medDeptCd=MD^MN^OS^PC^DM&startDate=20150820&endDate=20180522&sort=0&gubun=2
        //    //http://192.168.100.33:8090/Emr/grpFormNoByProgress.mts?grpFormNo=2&ptNo=03983614&inOutCls=0&medDeptCd=DM^MD^MN^OS^PC&startDate=20160501&endDate=20160501&sort=0&gubun=2
        //    strURL = gEmrUrl + "/grpFormNoByProgress.mts?grpFormNo=2&ptNo=" + pPtPtNo +
        //            "&inOutCls=" + pInOutCls +
        //            "&medDeptCd=" + pMedDeptCd +
        //            "&startDate=" + pMedFrDate +
        //            "&endDate=" + pMedEndDate +
        //            "&sort=" + pSort +
        //            "&gubun=" + pGUBUN;

        //    //strURL = "http://192.168.100.33:8090/Emr/grpFormNoByProgress.mts?grpFormNo=2&ptNo=02487371&inOutCls=0&medDeptCd=MD^MN^OS^PC^DM&startDate=20090302&endDate=20180524&sort=0&gubun=2";
        //    webEMR.Navigate(strURL);
        //    while (webEMR.IsBusy == true)
        //    {
        //        Application.DoEvents();
        //    }
        //    for (int intWeb = 0; intWeb < 40000; intWeb++)
        //    {
        //        Application.DoEvents();
        //    }
        //    #endregion //경과기록지 보기
        //}

        //private void ReadFirstDate(ref string strFirstDATE, string arg = "")
        //{

        //    if (AcpEmr == null) return;

        //    if (AcpEmr.inOutCls == "I") return;
        //    if (AcpEmr.medDeptCd == "HD") return;

        //    string strDeptCode = "";

        //    if (arg != "")
        //    {
        //        strDeptCode = "'" + VB.Replace(arg, "^", "','");
        //        if (VB.Right(arg, 1) == "^")
        //        {
        //            strDeptCode = VB.Mid(strDeptCode, 1, VB.Len(strDeptCode) - 2);
        //        }
        //        else
        //        {
        //            strDeptCode = VB.Mid(strDeptCode, 1, VB.Len(strDeptCode)) + "'";
        //        }
        //    }

        //    DataTable dt = null;
        //    string SQL = "";    //Query문
        //    string SqlErr = ""; //에러문 받는 변수

        //    SQL = " SELECT TO_CHAR(MIN(DDATE),'YYYY-MM-DD') MINDATE FROM";
        //    SQL = SQL + ComNum.VBLF + " (SELECT MIN(INDATE) DDATE";
        //    SQL = SQL + ComNum.VBLF + " From ADMIN.IPD_NEW_MASTER";
        //    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + AcpEmr.ptNo + " '";
        //    if (arg == "")
        //    {
        //        switch (AcpEmr.medDeptCd)
        //        {
        //            case "MC":
        //            case "MD":
        //            case "ME":
        //            case "MG":
        //            case "MI":
        //            case "MN":
        //            case "MP":
        //            case "MR":
        //                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE IN ('MD', '" + AcpEmr.medDeptCd + "' ) ";
        //                break;
        //            default:
        //                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + AcpEmr.medDeptCd + "' ";
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        SQL = SQL + ComNum.VBLF + "   AND DEPTCODE IN (" + strDeptCode + ") ";
        //    }
        //    SQL = SQL + ComNum.VBLF + " Union All";
        //    SQL = SQL + ComNum.VBLF + " SELECT MIN(ACTDATE) DDATE FROM ADMIN.OPD_MASTER";
        //    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + AcpEmr.ptNo + "' ";
        //    if (arg == "")
        //    {
        //        switch (AcpEmr.medDeptCd)
        //        {
        //            case "MC":
        //            case "MD":
        //            case "ME":
        //            case "MG":
        //            case "MI":
        //            case "MN":
        //            case "MP":
        //            case "MR":
        //                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE IN ('MD', '" + AcpEmr.medDeptCd + "' )) ";
        //                break;
        //            default:
        //                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + AcpEmr.medDeptCd + "') ";
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        SQL = SQL + ComNum.VBLF + "   AND DEPTCODE IN (" + strDeptCode + ") )";
        //    }

        //    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

        //    if (SqlErr != "")
        //    {
        //        ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
        //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
        //        return;
        //    }
        //    if (dt.Rows.Count > 0)
        //    {

        //    }
        //    strFirstDATE = dt.Rows[0]["MINDATE"].ToString().Trim();
        //    dt.Dispose();
        //    dt = null;

        //}

        //private void InitForm()
        //{
        //    lblChoChart.Text = "";
        //    ssUSERFORM_Sheet1.RowCount = 0;
        //    ssUserDept_Sheet1.RowCount = 0;
        //}

        //private void ClearForm()
        //{
        //    ClearNew();
        //}

        //private void ClearNew()
        //{
        //    string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");

        //    dtpChartDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(VB.Left(strCurDateTime, 8), "D"));
        //    txtChartTime.Text = ComFunc.FormatStrToDate(VB.Right(strCurDateTime, 6), "M");
        //    mEmrNo = "0";
        //    mEmrImageNo = "0";

        //    ClearColtrol();

        //    if (AcpEmr != null)
        //    {
        //        if (AcpEmr.medFrDate != "")
        //        {
        //            if (mInOutGb == "O")
        //            {
        //                dtpChartDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(AcpEmr.medFrDate, "D"));
        //            }
        //            else
        //            {
        //                dtpChartDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(VB.Left(strCurDateTime, 8), "D"));
        //            }
        //        }
        //        else
        //        {
        //            dtpChartDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(VB.Left(strCurDateTime, 8), "D"));
        //        }

        //        SetPatInfoImg();
        //        GetChartHis();
        //    }
        //}

        //private void GetUserChoFormNew()
        //{
        //    int i = 0;
        //    DataTable dt = null;
        //    string SQL = "";    //Query문
        //    string SqlErr = ""; //에러문 받는 변수

        //    ssUSERFORM_Sheet1.RowCount = 0;
        //    try
        //    {
        //        SQL = "";
        //        SQL = SQL + ComNum.VBLF + "  SELECT B.GRPFORMNO, B.GRPFORMNAME, A.FORMNO, A.FORMNAME1 FORMNAME,  C.DISPSEQ ";
        //        SQL = SQL + ComNum.VBLF + "    FROM ADMIN.EMRFORM A INNER JOIN ADMIN.EMRGRPFORM B";
        //        SQL = SQL + ComNum.VBLF + "        ON A.GRPFORMNO = B.GRPFORMNO";
        //        SQL = SQL + ComNum.VBLF + "        INNER JOIN ADMIN.EMRUSERFORMCHO C";
        //        SQL = SQL + ComNum.VBLF + "        ON A.FORMNO = C.FORMNO";
        //        SQL = SQL + ComNum.VBLF + "    WHERE (B.USECHECK IS NULL ";
        //        SQL = SQL + ComNum.VBLF + "        OR B.USECHECK = '0')";
        //        SQL = SQL + ComNum.VBLF + "    AND C.USEID = '" + clsType.User.IdNumber + "'";
        //        SQL = SQL + ComNum.VBLF + "    ORDER BY C.DISPSEQ, A.FORMNO";

        //        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

        //        if (SqlErr != "")
        //        {
        //            ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
        //            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
        //            Cursor.Current = Cursors.Default;
        //            return;
        //        }
        //        if (dt.Rows.Count == 0)
        //        {
        //            dt.Dispose();
        //            dt = null;
        //            Cursor.Current = Cursors.Default;
        //            return;
        //        }

        //        ssUSERFORM_Sheet1.RowCount = dt.Rows.Count;
        //        ssUSERFORM_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
        //        for (i = 0; i < dt.Rows.Count; i++)
        //        {
        //            if (i == 0)
        //            {
        //                mstrUserChoJinFormName = dt.Rows[i]["FORMNAME"].ToString().Trim();
        //                mstrUserChoJinForm = dt.Rows[i]["FORMNO"].ToString().Trim();
        //            }
        //            ssUSERFORM_Sheet1.Cells[i, 0].Text = dt.Rows[i]["GRPFORMNAME"].ToString().Trim();
        //            ssUSERFORM_Sheet1.Cells[i, 1].Text = dt.Rows[i]["FORMNAME"].ToString().Trim();
        //            ssUSERFORM_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DISPSEQ"].ToString().Trim();
        //            ssUSERFORM_Sheet1.Cells[i, 3].Text = dt.Rows[i]["FORMNO"].ToString().Trim();
        //            ssUSERFORM_Sheet1.Cells[i, 4].Text = dt.Rows[i]["GRPFORMNO"].ToString().Trim();
        //        }
        //        dt.Dispose();
        //        dt = null;
        //    }
        //    catch (Exception ex)
        //    {
        //        ComFunc.MsgBoxEx(this, ex.Message);
        //        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
        //    }
        //}

        //private void GetUserDept()
        //{
        //    int i = 0;
        //    DataTable dt = null;
        //    string SQL = "";    //Query문
        //    string SqlErr = ""; //에러문 받는 변수

        //    ssUserDept_Sheet1.RowCount = 0;
        //    try
        //    {
        //        SQL = "";
        //        SQL = "SELECT ";
        //        SQL = SQL + ComNum.VBLF + "     A.MEDDEPTCD, A.DEPTKORNAME, B.USEID ";
        //        SQL = SQL + ComNum.VBLF + "FROM ADMIN.VIEWBMEDDEPT A ";
        //        SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN ADMIN.EMRUSERDEPT B";
        //        SQL = SQL + ComNum.VBLF + "    ON A.MEDDEPTCD = B.MEDDEPTCD ";
        //        SQL = SQL + ComNum.VBLF + "     AND B.USEID = '" + clsType.User.IdNumber + "'";
        //        SQL = SQL + ComNum.VBLF + " ORDER BY A.DEPTKORNAME, A.PRTGRD ";
        //        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

        //        if (SqlErr != "")
        //        {
        //            ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
        //            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
        //            Cursor.Current = Cursors.Default;
        //            return;
        //        }
        //        if (dt.Rows.Count == 0)
        //        {
        //            dt.Dispose();
        //            dt = null;
        //            Cursor.Current = Cursors.Default;
        //            return;
        //        }

        //        ssUserDept_Sheet1.RowCount = dt.Rows.Count;
        //        ssUserDept_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
        //        for (i = 0; i < dt.Rows.Count; i++)
        //        {
        //            if (dt.Rows[i]["USEID"].ToString().Trim() != "")
        //            {
        //                ssUserDept_Sheet1.Cells[i, 0].Value = true;
        //            }
        //            ssUserDept_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DEPTKORNAME"].ToString().Trim();
        //            ssUserDept_Sheet1.Cells[i, 2].Text = dt.Rows[i]["MEDDEPTCD"].ToString().Trim();
        //        }
        //        dt.Dispose();
        //        dt = null;

        //        MakeUserDept();
        //    }
        //    catch (Exception ex)
        //    {
        //        ComFunc.MsgBoxEx(this, ex.Message);
        //        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
        //    }
        //}

        //private void MakeUserDept()
        //{
        //    int i = 0;

        //    for (i = 0; i < ssUserDept_Sheet1.RowCount; i++)
        //    {
        //        if (Convert.ToBoolean(ssUserDept_Sheet1.Cells[i, 0].Value) == true)
        //        {
        //            mstrUserDept = mstrUserDept + ",'" + ssUserDept_Sheet1.Cells[i, 2].Text.Trim() + "'";
        //        }
        //    }
        //    if (mstrUserDept.Length > 0)
        //    {
        //        mstrUserDept = VB.Right(mstrUserDept, mstrUserDept.Length - 1);
        //    }
        //}

        //private void SetUserOption()
        //{
        //    GetUserChoFormNew();
        //    GetUserDept();

        //    lblChoChart.Text = mstrUserChoJinFormName;

        //    WebLogin();

        //    string strOptMcro = "";
        //    strOptMcro = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "OPTMCRO");
        //    if (strOptMcro == "1")
        //    {
        //        optDept.Checked = true;
        //    }
        //    else if (strOptMcro == "2")
        //    {
        //        optAll.Checked = true;
        //    }
        //    else
        //    {
        //        optUse.Checked = true;
        //    }

        //    string strOptKind = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "OPTKIND");
        //    if (strOptKind == "1")
        //    {
        //        optInOut1.Checked = true;
        //    }
        //    else if (strOptKind == "2")
        //    {
        //        optInOut2.Checked = true;
        //    }
        //    else
        //    {
        //        optInOut3.Checked = true;
        //    }

        //    string strOptSort = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "OPTSORT");
        //    if (strOptSort == "1")
        //    {
        //        opSortAs.Checked = true;
        //    }
        //    else
        //    {
        //        opSortDs.Checked = true;
        //    }

        //    string strDTPSDate = "";
        //    string strDTPEDATE = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        //    strDTPSDate = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "DTPSDATE");
        //    if (strDTPSDate == "")
        //    {
        //        strDTPSDate = (VB.DateAdd("m", -3, strDTPEDATE)).ToString("yyyy-MM-dd");
        //    }

        //    dtpSDate.Value = Convert.ToDateTime(strDTPSDate);
        //    dtpEDate.Value = Convert.ToDateTime(strDTPEDATE);

        //}

        //#endregion //Private Function


        public frmEmrForm_Progress()
        {
            InitializeComponent();
        }

        private void frmEmrForm_Progress_Load(object sender, EventArgs e)
        {

        }
        
    }
}
