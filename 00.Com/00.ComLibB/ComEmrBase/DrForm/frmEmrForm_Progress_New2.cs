using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace ComEmrBase
{
    public partial class frmEmrForm_Progress_New2 : Form, EmrChartForm
    {
        //string mstrPROGNO = "";
        string mSYSMPGB = "";
        /// <summary>
        /// //외부에서 저장을 요청한경우
        /// </summary>
        bool mSaveFlag = false;
        /// <summary>
        /// //초진차트 작성 두번 보이는 것 안보이게
        /// </summary>
        bool mIsFirtQuery = false; 
        bool IsLoadForm = true;

        string mstrEmrNo = "0"; //Progress 
        string mstrFormNo = "963";
        string mstrUpdateNo = "2";

        frmEmrBaseSympOld fEmrMacro = null;
        frmEmrBaseEmrChartOld fEmrChart = null;
        FormEmrMessage mEmrCallForm = null;

        EmrPatient AcpEmr = null;

        public frmEmrForm_Progress_New2()
        {
            InitializeComponent();
        }

        public frmEmrForm_Progress_New2(EmrPatient pAcpEmr, FormEmrMessage pEmrCallForm)
        {
            InitializeComponent();
            AcpEmr = pAcpEmr;
            mEmrCallForm = pEmrCallForm;
        }

        public frmEmrForm_Progress_New2(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm)
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
        /// 작성중인게 있는지 확인 하는 함수
        /// </summary>
        /// <returns></returns>
        public string CheckChartChangeData()
        {
            string rtnVal = string.Empty;

            if(txtProgress.Visible == true)
            {
                if(txtProgress.TextLength > 0)
                {
                    rtnVal = "현재 경과기록지를 작성중입니다. ";
                }
            }
            else
            {
                if (ssSOAP_Sheet1.Cells[0, 1].Text.Trim().Length > 0 || ssSOAP_Sheet1.Cells[1, 1].Text.Trim().Length > 0 ||
                    ssSOAP_Sheet1.Cells[2, 1].Text.Trim().Length > 0 || ssSOAP_Sheet1.Cells[3, 1].Text.Trim().Length > 0 )
                {
                    if(chkSOAP0.Checked)
                    {
                        rtnVal = "현재 SOAP 형식을 작성중입니다. ";

                    }
                    else if(chkSOAP1.Checked)
                    {
                        rtnVal = "현재 수술 후 환자상태 형식을 작성중입니다. ";
                    }
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 사용자 정보가 바뀐 경우 : 사용자별 환경 설정을 다시 한다
        /// </summary>
        /// 
        public void SetUserInfo()
        {
            ClearForm();
            SetUserOption();
        }

        /// <summary>
        /// 옵션보기 설정 변경시 상용구 재로드
        /// </summary>
        public void ChangeBoilerplate()
        {
            GetMeCroTitle();
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

            GetPatRmk();
            GetMibi();

            //ChkFrDate();

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

            //tabEmr.SelectedTab = tabEmrWrite;

            //strEmrOption = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "AFTERPROGSAVE");
            //if (VB.Val(strEmrOption) == 1)
            //{
            //    tabEmr.SelectedTab = tabEmrWrite;
            //}
            //else
            //{
            //    tabEmr.SelectedTab = tabEmrView;
            //}

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
                SQL = SQL + ComNum.VBLF + "    NVL(MAX(M1.EMRNO), 0) AS EMRNO  ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMRXMLMST M1 ";
                SQL = SQL + ComNum.VBLF + "WHERE M1.PTNO = '" + AcpEmr.ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "    AND M1.MEDFRDATE = '" + AcpEmr.medFrDate.Replace("-", "") + "' ";
                SQL = SQL + ComNum.VBLF + "    AND M1.MEDDEPTCD = '" + AcpEmr.medDeptCd + "' ";
                SQL = SQL + ComNum.VBLF + "    AND M1.INOUTCLS = 'O' ";
                SQL = SQL + ComNum.VBLF + "    AND M1.FORMNO = 963 ";
                SQL = SQL + ComNum.VBLF + "    AND M1.USEID = '" + clsType.User.IdNumber + "' ";
                SQL = SQL + ComNum.VBLF + "UNION ALL";
                SQL = SQL + ComNum.VBLF + "SELECT  ";
                SQL = SQL + ComNum.VBLF + "    NVL(MAX(EMRNO), 0) AS EMRNO  ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST ";
                SQL = SQL + ComNum.VBLF + "WHERE PTNO = '" + AcpEmr.ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "    AND MEDFRDATE = '" + AcpEmr.medFrDate.Replace("-", "") + "' ";
                SQL = SQL + ComNum.VBLF + "    AND MEDDEPTCD = '" + AcpEmr.medDeptCd + "' ";
                SQL = SQL + ComNum.VBLF + "    AND INOUTCLS = 'O' ";
                SQL = SQL + ComNum.VBLF + "    AND FORMNO = 963 ";
                SQL = SQL + ComNum.VBLF + "    AND CHARTUSEID = '" + clsType.User.IdNumber + "' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY EMRNO DESC ";

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
                SQL = SQL + ComNum.VBLF + "EMRNO,   ITEMVALUE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTROW ";
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO = (  ";
                SQL = SQL + ComNum.VBLF + "            SELECT  ";
                SQL = SQL + ComNum.VBLF + "                MAX(M1.EMRNO)  ";
                SQL = SQL + ComNum.VBLF + "            FROM " + ComNum.DB_EMR + "AEMRCHARTMST M1 ";
                SQL = SQL + ComNum.VBLF + "            WHERE M1.PTNO = '" + AcpEmr.ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "                AND M1.MEDFRDATE < '" + AcpEmr.medFrDate.Replace("-", "") + "' ";
                SQL = SQL + ComNum.VBLF + "                AND M1.MEDDEPTCD = '" + AcpEmr.medDeptCd + "' ";
                SQL = SQL + ComNum.VBLF + "                AND M1.INOUTCLS = 'O' ";
                SQL = SQL + ComNum.VBLF + "                AND M1.FORMNO = 963 ";
                SQL = SQL + ComNum.VBLF + "                AND M1.CHARTDATE = (SELECT MAX(M1.CHARTDATE)  ";
                SQL = SQL + ComNum.VBLF + "                                        FROM " + ComNum.DB_EMR + "AEMRCHARTMST M1 ";
                SQL = SQL + ComNum.VBLF + "                                        WHERE M1.PTNO = '" + AcpEmr.ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "                                            AND M1.MEDFRDATE < '" + AcpEmr.medFrDate.Replace("-", "") + "' ";
                SQL = SQL + ComNum.VBLF + "                                            AND M1.MEDDEPTCD = '" + AcpEmr.medDeptCd + "' ";
                SQL = SQL + ComNum.VBLF + "                                            AND M1.INOUTCLS = 'O' ";
                SQL = SQL + ComNum.VBLF + "                                            AND M1.FORMNO = 963) ";
                SQL = SQL + ComNum.VBLF + "                ) ";
                SQL = SQL + ComNum.VBLF + "  AND EMRNOHIS > 0";
                SQL = SQL + ComNum.VBLF + "  AND ITEMCD IN ('I0000000981')";

                SQL = SQL + ComNum.VBLF + "  UNION ALL";
                SQL = SQL + ComNum.VBLF + " SELECT ";
                SQL = SQL + ComNum.VBLF + " EMRNO, EXTRACTVALUE(chartxml, '//ta1') as  ITEMVALUE ";
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
                SQL = SQL + ComNum.VBLF + "ORDER BY EMRNO DESC ";

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
                //strCHARTA = MakeContentValue(dt.Rows[0]["ITEMVALUE"].ToString().Trim());
                strCHARTA = dt.Rows[0]["ITEMVALUE"].ToString().Trim();

                dt.Dispose();
                dt = null;

                txtProgress.Text = strCHARTA.Replace("\r\n", "\n").Replace("\n", "\r\n");

                //tabEmr.SelectedTab = tabEmrWrite;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void ClearColtrol()
        {
            try
            {
                //mEDIT = "";
                //mEmrImageNo = "0";

                chkSOAP0.Checked = false;
                chkSOAP1.Checked = false;

                mbtnDelete.Visible = true;
                mbtnSave.Visible = true;
                //mbtnSaveImag.Visible = true;
                btnMibi.Visible = false;

                dtpChartDate.Enabled = true;
                txtChartTime.Enabled = true;
                txtEmrNo.Enabled = false;

                txtEmrNo.Text = "";
                txtProgress.Text = "";
                txtProgress.Tag = null;

                btnSearchRmk.BackColor = Color.White;
                imgRmk.Visible = false;
                imgRmk.Tag = null;
                toolRmk.Tag = null;
                
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
        public double SetSaveData()
        {
            mSaveFlag = true;

            string strEMRNO = "0";

            if (SaveData() == true)
            {
                strEMRNO = mstrEmrNo;
                ClearForm();
            }

            return VB.Val(strEMRNO);
        }

        private void ClearEnd()
        {
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");

            dtpChartDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(VB.Left(strCurDateTime, 8), "D"));
            txtChartTime.Text = ComFunc.FormatStrToDate(VB.Right(strCurDateTime, 6), "M");
            mstrEmrNo = "0";
            //mEmrImageNo = "0";

            ClearColtrol();
        }
        #endregion //Public Function

        #region //Private Function

        /// <summary>
        /// 기록지 신규 작성을 위해 클리어
        /// </summary>
        public void pClearForm()
        {
            //모든 컨트롤을 초기화 한다.
            mstrEmrNo = "0";
            ClearColtrol();
            txtProgress.Clear();
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
     
        private void ClearForm()
        {
            ClearNew();
        }

        private void ClearNew()
        {
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");

            dtpChartDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(VB.Left(strCurDateTime, 8), "D"));
            txtChartTime.Text = ComFunc.FormatStrToDate(VB.Right(strCurDateTime, 6), "M");
            mstrEmrNo = "0";
            txtEmrNo.Text = "0";
            //mEmrImageNo = "0";

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
                        //mstrUserChoJinFormName = dt.Rows[i]["FORMNAME"].ToString().Trim();
                        //mstrUserChoJinForm = dt.Rows[i]["FORMNO"].ToString().Trim();
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
        
        private void SetUserOption()
        {
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
            
        }

        #endregion //Private Function

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
            if (strPRINTFLAG == "N")
            {
                frmEmrPrintOption frmEmrPrintOptionX = new frmEmrPrintOption();
                frmEmrPrintOptionX.ShowDialog();
            }

            if (clsFormPrint.mstrPRINTFLAG == "-1")
            {
                return rtnVal;
            }

            //if (clsEmrQuery.SaveEmrXmlPrnYnForm(mstrEmrNo, "0") == false)
            //{
            //    return rtnVal;
            //}

            //rtnVal = clsFormPrint.PrintFormLong(clsDB.DbCon, mstrFormNo, mstrUpdateNo, AcpEmr, mstrEmrNo, panChart, "C");
            return rtnVal;
        }

        public int ToImageFormMsg(string strPRINTFLAG)
        {
            int rtnVal = 0;
            //rtnVal = clsFormPrint.PrintToTifFileLong(clsDB.DbCon, mstrFormNo, mstrUpdateNo, AcpEmr, mstrEmrNo, panChart, "C");
            return rtnVal;
        }


        #endregion

        private void frmEmrBaseProgressOcs_Load(object sender, EventArgs e)
        {
            ssUSERFORM.Top = 36;
            ssUSERFORM.Left = 7;

            ssGRPMACRO.Top = 36;
            ssGRPMACRO.Left = 290;

            txtProgress.Font = new Font("굴림체", 11, FontStyle.Regular);

            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");

            dtpChartDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(VB.Left(strCurDateTime, 8), "D"));
            txtChartTime.Text = ComFunc.FormatStrToDate(VB.Right(strCurDateTime, 6), "M");

            //mstrPROGNO = "963";
            //mstrPROGNAME = "Progress Note";
            //mstrPROGIMGNO = "1232";
            //mstrPROGIMGNAME = "Progress Image";

            ClearForm();
            #region 옵션 처리
            string strEmrOption =  clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "EMRKORENG");
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


            strEmrOption = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "EMRKORENGVISIBLE");
            if (VB.Val(strEmrOption) == 1 || string.IsNullOrEmpty(strEmrOption))
            {
                lblKorE.Visible = true;
            }
            else
            {
                lblKorE.Visible = false;
            }

            strEmrOption = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "SPECIALBTNVISIBLE");
            if (VB.Val(strEmrOption) == 1 || string.IsNullOrEmpty(strEmrOption))
            {
                panSpecial.Visible = true;
            }
            else
            {
                panSpecial.Visible = false;
            }
            #endregion

            //MakeWardChart(); //이거 필요없음
            SetUserOption();

            GetMeCroTitle();

            IsLoadForm = false;

            //환자의 이전 프로그래스 내역 불러오기 
            if (AcpEmr != null && mEmrCallForm != null && ((Form) mEmrCallForm).Name == "frmEmrBaseProgressOcsNew")
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

        }

       
        void GetMeCroTitle()
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            ssGRPFORM_Sheet1.RowCount = 0;

            switch (clsEmrPublic.gstrMcrAllFlag)
            {
                //전체
                case "1":
                    mSYSMPGB = "ALL";
                    break;
                //과별
                case "2":
                    mSYSMPGB = clsType.User.DeptCode;
                    break;
                //유저
                case "3":
                    mSYSMPGB = clsType.User.DrCode;
                    break;
            }

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT SYSMPGB, SYSMPINDEX, SYSMPKEY, SYSMPNAME";
                SQL += ComNum.VBLF + " FROM ADMIN.EMRSYSMP";
                SQL += ComNum.VBLF + " WHERE SYSMPGB = '" + mSYSMPGB + "'";
                SQL += ComNum.VBLF + "   AND SYSMPRMK IS NOT NULL";
                SQL += ComNum.VBLF + "ORDER BY SYSMPNAME";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, " EMRSYSMP 조회중 문제가 발생했습니다");
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

                ssGRPFORM_Sheet1.RowCount = dt.Rows.Count;
                ssGRPFORM_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssGRPFORM_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SYSMPNAME"].ToString().Trim();
                    ssGRPFORM_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SYSMPINDEX"].ToString().Trim();
                    ssGRPFORM_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SYSMPGB"].ToString().Trim();
                    ssGRPFORM_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SYSMPKEY"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void frmEmrBaseProgressOcs_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {

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
            }
            catch
            {

            }
        }

        private void btnSerach_Click(object sender, EventArgs e)
        {
            //SetPatInfoImg();
            //GetChartHis();
        }

        private void btnUserChoReg_Click(object sender, EventArgs e)
        {
            using (frmEmrBaseUserChoRegOld frm = new frmEmrBaseUserChoRegOld())
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(this);
            }
            GetUserChoFormNew();
        }

        private void btnSearchRmk_Click(object sender, EventArgs e)
        {
            if (AcpEmr == null)
                return;

            using (frmEmrBaseSingularRemark frm = new frmEmrBaseSingularRemark(AcpEmr.ptNo))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog();
            }
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

            fEmrMacro = new frmEmrBaseSympOld(clsType.User.IdNumber, clsType.User.DeptCode, clsType.User.IdNumber, "I0000000981", "ProgressNote");
            fEmrMacro.rEventMakeText += new frmEmrBaseSympOld.EventMakeText(frmEmrBaseSympOld_EventMakeText);
            fEmrMacro.FormClosed += FEmrMacro_FormClosed;
            //fEmrMacro.rEventClosed += new frmEmrBaseSympOld.EventClosed(frmEmrBaseSympOld_EventClosed);
            fEmrMacro.Owner = this;
            fEmrMacro.Show();
        }

        private void FEmrMacro_FormClosed(object sender, FormClosedEventArgs e)
        {
            fEmrMacro.Dispose();
            fEmrMacro = null;
            GetMeCroTitle();
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
                txtProgress.SelectionStart = selstart + intMacro + 1;
            }

            GetMeCroTitle();
        }

        private void mbtnNew_Click(object sender, EventArgs e)
        {
            if (mEmrCallForm != null)
            {
                mEmrCallForm.MsgClear();
            }
            mstrEmrNo = "0";
            txtEmrNo.Text = "";
            ClearNew();
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
                txtProgress.Visible = false;
                ssSOAP_Sheet1.Cells[0, 0].Text = "S)";
                ssSOAP_Sheet1.Cells[1, 0].Text = "O)";
                ssSOAP_Sheet1.Cells[2, 0].Text = "A)";
                ssSOAP_Sheet1.Cells[3, 0].Text = "P)";
                ssSOAP_Sheet1.Columns[0].Width = 30;
                ssSOAP_Sheet1.Columns[1].Width = 520;
                //ssSOAP_Sheet1.Columns[1].Width = 450;
                ssSOAP.Visible = true;
                //ssSOAP.Top = 117;
                //ssSOAP.Left = 0;
                //ssSOAP.Width = panProg.Width;
                //ssSOAP.Dock = DockStyle.Left;
                panProg.Height = 500;
                //ssSOAP.BringToFront();
                ssSOAP.Focus();
                ssSOAP_Sheet1.SetActiveCell(0, 1);
            }
            else
            {
                //txtProgress.Visible = true;
                chkSOAP1.Checked = true;
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
                txtProgress.Visible = false;
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
                txtProgress.Visible = true;
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

            //ChkFrDate();

            if (SaveData() == true)
            {
                string strEmrNo = "0";
                strEmrNo = mstrEmrNo;
                mstrEmrNo = "0";
                txtEmrNo.Text = "";
                ClearForm();

                //SetProgressOne(strEmrNo);

                if(mEmrCallForm != null && ((Form)mEmrCallForm).Name == "frmEmrBaseProgressOcsNew")
                {
                    string strEmrOption = "0";
                    strEmrOption = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "AFTERPROGSAVE");
                    if (VB.Val(strEmrOption) == 1)
                    {
                        SetProgressOne(strEmrNo);
                    }
                    else
                    {
                        ((frmEmrBaseProgressOcsNew)mEmrCallForm).SetChartView();
                    }
                }

                //SetPatInfoImg();
                //GetChartHis();

                if (mEmrCallForm != null)
                {
                    mEmrCallForm.MsgSave("0");
                }
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


            //string strURL = "";
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

                //if (clsEmrQueryOld.EmrSaveUserOption(clsDB.DbCon, "OOORD", "DTPSDATE", dtpSDate.Value.ToString("yyyy-MM-dd")) == false)
                //{

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

                SQL = "SELECT A.FORMNO, A.EMRNO, F.FORMNAME,  A.FORMNO, A.MEDFRDATE,";
                SQL = SQL + ComNum.VBLF + "    A.CHARTDATE, A.CHARTTIME, A.USEID AS USEID, EXTRACTVALUE(B.CHARTXML, '//ta1') as ITEMVALUE";
                SQL = SQL + ComNum.VBLF + "    FROM ADMIN.EMRXMLMST A";
                SQL = SQL + ComNum.VBLF + "      INNER JOIN ADMIN.EMRXML B";
                SQL = SQL + ComNum.VBLF + "         ON A.EMRNO = B.EMRNO";
                SQL = SQL + ComNum.VBLF + "      INNER JOIN ADMIN.AEMRFORM F";
                SQL = SQL + ComNum.VBLF + "         ON A.FORMNO   = F.FORMNO";
                SQL = SQL + ComNum.VBLF + "        AND F.UPDATENO > 0";
                SQL = SQL + ComNum.VBLF + "        AND F.OLDGB = '1'";
                SQL = SQL + ComNum.VBLF + "    WHERE A.EMRNO = " + strEmrNo;
                SQL = SQL + ComNum.VBLF + "UNION ALL";
                SQL = SQL + ComNum.VBLF + "SELECT A.FORMNO, A.EMRNO, F.FORMNAME,  A.FORMNO, A.MEDFRDATE,";
                SQL = SQL + ComNum.VBLF + "    A.CHARTDATE, A.CHARTTIME, A.CHARTUSEID AS USEID, (B.ITEMVALUE || B.ITEMVALUE1) AS ITEMVALUE";
                SQL = SQL + ComNum.VBLF + "    FROM ADMIN.AEMRCHARTMST A";
                SQL = SQL + ComNum.VBLF + "      INNER JOIN ADMIN.AEMRCHARTROW B";
                SQL = SQL + ComNum.VBLF + "         ON A.EMRNO = B.EMRNO";
                SQL = SQL + ComNum.VBLF + "        AND A.EMRNOHIS = B.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "        AND B.ITEMCD IN ('I0000000981')";
                SQL = SQL + ComNum.VBLF + "      INNER JOIN ADMIN.AEMRFORM F";
                SQL = SQL + ComNum.VBLF + "         ON A.FORMNO   = F.FORMNO";
                SQL = SQL + ComNum.VBLF + "        AND A.UPDATENO = F.UPDATENO ";
                SQL = SQL + ComNum.VBLF + "    WHERE A.EMRNO = " + strEmrNo;
                SQL = SQL + ComNum.VBLF + "ORDER BY EMRNO DESC";

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
                strUSERFORMNO = dt.Rows[0]["FORMNO"].ToString().Trim();
                strFormName = dt.Rows[0]["FORMNAME"].ToString().Trim();
                strChartDate = dt.Rows[0]["CHARTDATE"].ToString().Trim();
                strChartTime = dt.Rows[0]["CHARTTIME"].ToString().Trim();
                strUseId = dt.Rows[0]["USEID"].ToString().Trim();
                strMedFrDate = dt.Rows[0]["MEDFRDATE"].ToString().Trim();
                strRmk = dt.Rows[0]["ITEMVALUE"].ToString().Trim();

                dt.Dispose();
                dt = null;


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
                        //mbtnSaveImag.Visible = true;
                    }
                }
                else
                {
                    mbtnDelete.Visible = true;
                    mbtnSave.Visible = true;
                    //mbtnSaveImag.Visible = true;
                }
                //tabEmr.SelectedTab = tabEmrWrite;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                //ComFunc.MsgBoxEx(this, ex.Message);
                //clsDB.SaveSqlErrLog(ex.Message, "", clsDB.DbCon); //에러로그 저장
                return;
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
                        if (ComFunc.MsgBoxQEx(this, "외래진료일과 챠트 작성일이 다릅니다. 계속 진행하시겠습니까?", "PSMH", MessageBoxDefaultButton.Button2) == DialogResult.No)
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

            string strTemp = string.Empty;

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

            if (string.IsNullOrWhiteSpace(txtProgress.Text.Trim()))
            {
                ComFunc.MsgBoxEx(this, "내용을 입력해주세요.");
                return false;
            }

            double dblEmrNo = VB.Val(txtEmrNo.Text);
            //string strChartUseId = "";

            //int i = 0;
            //DataTable dt = null;
            string SQL = string.Empty;    //Query문
            string SqlErr = string.Empty; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수


            string strProgress = ComFunc.QuotConv(txtProgress.Text.Trim());

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

            double dblEmrNoNew = 0;
            clsEmrQuery.SaveNewProgressEx(clsDB.DbCon, this, AcpEmr, VB.Val(txtEmrNo.Text), txtProgress.Text.Trim().Replace("'", "`"), ref dblEmrNoNew, strChartDate, strChartTime);

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
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


                clsDB.setCommitTran(clsDB.DbCon);

                mstrEmrNo = dblEmrNoNew.ToString();

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

            if (ComFunc.MsgBoxQEx(this, "기존내용을 삭제하시겠습니까?", "PSMH", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }
            double dblEmrNo = VB.Val(txtEmrNo.Text);

            if (DeleteDate(dblEmrNo) == true)
            {
                ClearForm();
                if (mEmrCallForm != null)
                {
                    mEmrCallForm.MsgDelete();
                }
            }
        }

        private bool DeleteDate(double dblEmrNo)
        {
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            strCurDateTime = strCurDateTime.Replace("-", "").Replace(":", "");
            string strCurDate = VB.Left(strCurDateTime, 8);
            string strCurTime = VB.Right(strCurDateTime, 6);
            string SqlErr = string.Empty;

            DataTable dt = null;
            string SQL = "";    //Query문

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                string strChartUseId = "";

                SQL = "SELECT ";
                SQL = SQL + ComNum.VBLF + "                A.EMRNO, A.USEID";
                SQL = SQL + ComNum.VBLF + "    FROM ADMIN.EMRXML A";
                SQL = SQL + ComNum.VBLF + "    WHERE A.EMRNO = " + dblEmrNo;
                SQL = SQL + ComNum.VBLF + "UNION ALL ";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "                A.EMRNO, A.CHARTUSEID";
                SQL = SQL + ComNum.VBLF + "    FROM ADMIN.AEMRCHARTMST A";
                SQL = SQL + ComNum.VBLF + "    WHERE A.EMRNO = " + dblEmrNo;
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return true;
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

                #region //과거기록 백업
                double dblEmrHisNo = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "EMRXMLHISNO"));
                SqlErr = clsEmrQuery.SaveChartMastHis(clsDB.DbCon, dblEmrNo.ToString(), dblEmrHisNo, strCurDate, strCurTime, "C", "", strChartUseId);
                if (SqlErr != "OK")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                #endregion

                #region //과거기록 백업 : Old EMR Backup
                if (DeleteDateOldChart(dblEmrNo, dblEmrHisNo) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    Cursor.Current = Cursors.Default;
                }
                #endregion

                clsDB.setCommitTran(clsDB.DbCon);
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                Log.Error(ex.ToString());
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, "경과기록지 삭제도중 오류발생", clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private bool DeleteDateOldChart(double dblEmrNo , double dblEmrHisNo)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                strCurDateTime = strCurDateTime.Replace("-", "").Replace(":", "");

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
                    return true;
                }
                dt.Dispose();
                dt = null;

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
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE FROM ADMIN.EMRXML";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + dblEmrNo;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE FROM ADMIN.EMRXMLMST";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + dblEmrNo;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
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

                //txtProgress.Text = txtProgress.Text + " " + dt.Rows[0]["SYSMPRMK"].ToString().Trim();

                int selstart = txtProgress.SelectionStart == 0 ? txtProgress.TextLength : txtProgress.SelectionStart;
                txtProgress.Text = txtProgress.Text.Insert(selstart, " " + dt.Rows[0]["SYSMPRMK"].ToString().Trim());
                txtProgress.Focus();
                txtProgress.SelectionStart = selstart + 2;
                txtProgress.SelectionLength = 0;

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
            //19-08-12 TF팀 요구사항으로 추가함.
            else if (e.KeyCode == Keys.F5)
            {
                ((TextBox)sender).Clear();
            }
            else if (e.KeyCode == Keys.F12)
            {
                if (txtProgress.Text.IndexOf(" ") == -1)
                    return;

                string sText = txtProgress.Text.Substring(txtProgress.Text.TrimEnd().LastIndexOf(" ")).Trim();

                txtProgress.Text = txtProgress.Text.Substring(0, txtProgress.Text.LastIndexOf(" "));

                GetMeCro(txtProgress, sText);
            }
        }

        private void GetMeCro(TextBox txtProgress, string SYSMPNAME)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT SYSMPNAME, SYSMPRMK";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.EMRSYSMP";
                SQL = SQL + ComNum.VBLF + " WHERE SYSMPNAME = '" + SYSMPNAME.Replace("'", "`") + "'";
                SQL = SQL + ComNum.VBLF + "   AND SYSMPGB = '" + clsType.User.DrCode + "'";

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
                    int selstart = txtProgress.TextLength;
                    txtProgress.Text = txtProgress.Text.Insert(selstart, " " + dt.Rows[0]["SYSMPRMK"].ToString().Trim() + " ");
                    txtProgress.Focus();
                    txtProgress.SelectionStart = txtProgress.TextLength + 2;
                    txtProgress.SelectionLength = 0;                                                                
                }
                //else
                //{
                //    clsEmrFunc.DspControlF12(clsDB.DbCon, this, panChart, dt, ssMacroWord, txtProgress);
                //}

                //txtProgress.Text = txtProgress.Text + " " + dt.Rows[0]["SYSMPRMK"].ToString().Trim();


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

        private void BtnSpecial_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            int selstart = txtProgress.SelectionStart == 0 ? txtProgress.TextLength : txtProgress.SelectionStart;
            txtProgress.Text = txtProgress.Text.Insert(selstart, " " + btn.Text.Replace("\r\n", "").Trim());
            txtProgress.Focus();
            txtProgress.SelectionStart = selstart + 2;
            txtProgress.SelectionLength = 0;
        }

        private void BtnSideBarWrite_Click(object sender, EventArgs e)
        {
            if(btnSideBarWrite.Text == "◁")
            {
                ssGRPFORM.Visible = false;
                panLeft.Width = 30;
                btnSideBarWrite.Text = "▷";
            }
            else
            {
                ssGRPFORM.Visible = true;
                panLeft.Width = 70;
                btnSideBarWrite.Text = "◁";
            }
        }

        private void ssGRPFORM_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssGRPFORM_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssGRPFORM, e.Column);
                return;
            }

            ssGRPFORM_Sheet1.Cells[0, 0, ssGRPFORM_Sheet1.RowCount - 1, ssGRPFORM_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssGRPFORM_Sheet1.Cells[e.Row, 0, e.Row, ssGRPFORM_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            GetMeCro(ssGRPFORM_Sheet1.Cells[e.Row, 1].Text.Trim());
        }


        private void ssGRPFORM_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            //if (ssGRPFORM_Sheet1.RowCount == 0) return;

            //if (e.ColumnHeader == true)
            //{
            //    clsSpread.gSpdSortRow(ssGRPFORM, e.Column);
            //    return;
            //}

            //ssGRPFORM_Sheet1.Cells[0, 0, ssGRPFORM_Sheet1.RowCount - 1, ssGRPFORM_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            //ssGRPFORM_Sheet1.Cells[e.Row, 0, e.Row, ssGRPFORM_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            //GetMeCro(ssGRPFORM_Sheet1.Cells[e.Row, 1].Text.Trim());

            //rEventMakeText(intOption, txtProgress.Text.Trim());
        }
    }
}
