using ComBase;
using mtsImgList;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmScanImageViewNew : Form, EmrChartForm
    {
        private Form mCallForm = null;
        private string mstrEmrNo = "0";       //EMRNO
        private string mstrACPNO = "0";       //접수번호
        private string mstrFORMCODE = string.Empty;     //스캔서식코드

        private string mSelectImagePath = "";
        private int mintSelectImg = -1;

        private int mintPrint = 0;

        #region 차트복사용 변수
        /// <summary>
        /// 차트복사용 환자번호
        /// </summary>
        private string mstrPtno = string.Empty;
        /// <summary>
        /// 차트복사용 신청일자
        /// </summary>
        private string mstrPrtDate = string.Empty;
        /// <summary>
        /// 차트복사용 신청자 아이디
        /// </summary>
        private string mstrUseId = string.Empty;
        /// <summary>
        /// 002 = 전송용
        /// </summary>
        //private string mstrPrtCode = "002";
        /// <summary>
        /// 의료정보팀 여부
        /// </summary>
        private bool mMedicalTeam = false;
        #endregion

        //private string mstrViewPath = @"C:\PSMHEXE\temp\mhdrerm\";
        public string mstrViewPathInit = @"C:\PSMHEXE\ScanTmp\Formname"; // clsType.gSvrInfo.strClient + "\\ScanTmp\\" + clsCommon.gstrEXENAME.Replace(".EXE", "");
        public string mstrViewPath = string.Empty;

        
        #region //EmrChartForm
        public double SaveDataMsg(string strFlag)
        {
            double dblEmrNo = 0;
            //isReciveOrderSave = true;
            //dblEmrNo = pSaveData(strFlag);
            //isReciveOrderSave = false;
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
            //pClearForm();
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

            //if (clsEmrQuery.ScanImageYn(mstrEmrNo, "0") == false)
            //{
            //    return rtnVal;
            //}

            if(strPRINTFLAG == "0")
            {
                 Cursor.Current = Cursors.WaitCursor;
                for(int i = 0; i < mtsThumbNail.Controls.Count; i++)
                {
                    mtsThumbNail.ItemX(i + 1).Selected = true;
                }
                ScanPrint();
                for (int i = 0; i < mtsThumbNail.Controls.Count; i++)
                {
                    File.Delete(mtsThumbNail.ItemX(i + 1).ImagePath);
                }

                rtnVal = mintPrint;
                Cursor.Current = Cursors.Default;
            }

            return rtnVal;
        }

        public int ToImageFormMsg(string strPRINTFLAG)
        {
            int rtnVal = 0;
            //rtnVal = clsFormPrint.PrintToTifFileLong(mstrFormNo, mstrUpdateNo, p, mstrEmrNo, panChart, "C");
            return rtnVal;
        }
        #endregion

        public frmScanImageViewNew()
        {
            InitializeComponent();
        }

        public frmScanImageViewNew(Form pCallForm, string strEmrNo)
        {
            InitializeComponent();
            mCallForm = pCallForm;
            mstrEmrNo = strEmrNo;
        }

        /// <summary>
        /// 19-07-29 생성
        /// 차트복사 전용 생성자
        /// </summary>
        /// <param name="pCallForm">부모 폼</param>
        /// <param name="strPtno">환자번호</param>
        /// <param name="strPrtDate">신청일자</param>
        /// <param name="strUseId">신청자아이디</param>
        /// <param name="bMedicalTeam">의료정보팀인지 여부 </param>
        public frmScanImageViewNew(Form pCallForm, string strPtno, string strPrtDate, string strUseId, bool bMedicalTeam)
        {
            InitializeComponent();
            mCallForm   = pCallForm;
            mstrPtno    = strPtno;
            mstrPrtDate = strPrtDate;
            mstrUseId   = strUseId;
            mMedicalTeam = bMedicalTeam;
        }

        public frmScanImageViewNew(Form pCallForm, string strACPNO, string strFORMCODE, string strEmrNo)
        {
            InitializeComponent();
            mCallForm = pCallForm;
            mstrACPNO = strACPNO;
            mstrFORMCODE = strFORMCODE;
            mstrEmrNo = strEmrNo;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmScanImageViewNew_Load(object sender, EventArgs e)
        {
            string strCurDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            lblCur.Text = "";
            lblTot.Text = "";

            mstrViewPath = mstrViewPathInit + "\\" + strCurDate;

            if (Directory.Exists(mstrViewPathInit) == true)
            {
                string[] subdirectoryEntries = Directory.GetDirectories(mstrViewPathInit);
                foreach (string subdirectory in subdirectoryEntries)
                {
                    ComFunc.DeleteFoldAll(subdirectory);
                    Directory.Delete(subdirectory);
                }
            }

            if (Directory.Exists(mstrViewPath) == false)
            {
                Directory.CreateDirectory(mstrViewPath);
            }

            if (mCallForm != null && mCallForm.Name == "frmEmrViewMain")
            {
                //panTitle.Visible = false;
                btnExit.Visible = false;
            }
            
            if (clsType.User.AuAPRINTIN == "1")
            {
                btnPrint.Visible = true;
                btnPrintB.Visible = true;
                panMenuSmall.Visible = true;
            }
            else
            {
                btnPrint.Visible = false;
                btnPrintB.Visible = false;
                panMenuSmall.Visible = false;
            }

            //gInitUserCombo(cboUser, "0");
            //InitCodeCombo(cboCode);

            mtsThumbNail.Clear();
            mtsImgMainB.ClearImage();
            Application.DoEvents();

            if(mMedicalTeam == false)
            {
                LoadScanChartPSMH();
            }
            else
            {
                panCopy.Visible = false;
                LoadScanCopyList();
            }
        }

        /// <summary>
        /// 유저 콤보박스 설정
        /// </summary>
        /// <param name="cbo"></param>
        /// <param name="strAuth"></param>
        /// <returns></returns>
        bool gInitUserCombo(ComboBox cbo, string strAuth = "1")
        {
            bool rtnVal = false;
            StringBuilder SQL = new StringBuilder();
            DataTable dt = null;

            cbo.Items.Clear();

            try
            {

                SQL.AppendLine(" SELECT USERID, NAME");
                SQL.AppendLine(" FROM ADMIN.EMR_USERT ");
                SQL.AppendLine(" WHERE (EDATE >= TO_CHAR(SYSDATE, 'YYYYMMDD') or EDATE = '' OR EDATE IS NULL )  ");
                SQL.AppendLine(" AND AUTH >= '" + strAuth + "'  ");
                SQL.AppendLine(" ORDER BY NAME ");

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString(), clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL.ToString(), clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return rtnVal;
                }

                if(dt.Rows.Count > 0)
                {
                    for(int i = 0; i < dt.Rows.Count; i++)
                    {
                        cbo.Items.Add(dt.Rows[i]["NAME"].ToString().Trim() + "." + VB.Space(20) + "," + dt.Rows[i]["UserID"].ToString().Trim());
                    }
                }


                dt.Dispose();

                cbo.SelectedIndex = 0;
                rtnVal = true;
            }
            catch(Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL.ToString(), clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
            return rtnVal;
        }

        /// <summary>
        /// 콤보박스 설정
        /// </summary>
        /// <param name="cbo"></param>
        private void InitCodeCombo(ComboBox cbo)
        {
            StringBuilder SQL = new StringBuilder();
            DataTable dt = null;

            cbo.Items.Clear();

            try
            {

                SQL.AppendLine(" SELECT CODE, NAME, ACTIVE, AUTH");
                SQL.AppendLine(" FROM ADMIN.EMR_PRINTCODET");
                SQL.AppendLine(" WHERE ACTIVE = '1' ");

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString(), clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL.ToString(), clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cbo.Items.Add(dt.Rows[i]["NAME"].ToString().Trim() + "." + VB.Space(20) + "," + dt.Rows[i]["CODE"].ToString().Trim());
                    }
                }

                dt.Dispose();
                cbo.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL.ToString(), clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
            return;
        }

        private void frmScanImageView_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                mtsThumbNail.Clear();
                mtsImgMainB.ClearImage();
            }
            catch
            {
            }

        }

        #region LoadScanChartPSMH
        /// <summary>
        /// TREATNO로 환자 스캔 정보 가져오는 함수
        /// </summary>
        private void LoadScanChartPSMH()
        {
            if (VB.Val(mstrACPNO) == 0)
            {
                return;
            }

            spScan_Sheet1.RowCount = 0;

            int i = 0;
            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                SQL = "";
                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "   T.PATID, C.TREATNO, C.PAGENO, C.PAGE, P.PATHID,  ";
                SQL = SQL + ComNum.VBLF + "    CASE  ";
                SQL = SQL + ComNum.VBLF + "        WHEN P.EXTENSION = '' OR P.EXTENSION IS NULL THEN 'tif'  ";
                SQL = SQL + ComNum.VBLF + "        ELSE P.EXTENSION  ";
                SQL = SQL + ComNum.VBLF + "    END AS EXTENSION,  ";
                SQL = SQL + ComNum.VBLF + "    C.SECURITY, P.FILESIZE, P.CDATE, F.NAME,  ";
                SQL = SQL + ComNum.VBLF + "    C.FORMCODE, C.UNREADY, C.CDNO, F.NAME ,T.CLASS , ";
                SQL = SQL + ComNum.VBLF + "    (SELECT C1.NAME  ";
                SQL = SQL + ComNum.VBLF + "        FROM ADMIN.EMR_CLINICT C1  ";
                SQL = SQL + ComNum.VBLF + "        WHERE C1.CLINCODE = T.CLINCODE) AS LOCATIONNM,  ";
                SQL = SQL + ComNum.VBLF + "    T.INDATE, P.LOCATION, ";
                SQL = SQL + ComNum.VBLF + "    S.IPADDRESS, S.FTPUSER, S.FTPPASSWD, S.LOCALPATH, ";
                //SQL = SQL + ComNum.VBLF + "    ( REPLACE(S.LOCALPATH, '\', '/') || '/' || REPLACE(P.LOCATION, '\', '/') ) AS SVRFILEPATH  ";
                SQL = SQL + ComNum.VBLF + "    ( S.LOCALPATH || '/' || P.LOCATION ) AS SVRFILEPATH  ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.EMR_PAGET P  ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.EMR_CHARTPAGET C ";
                SQL = SQL + ComNum.VBLF + "    ON P.PAGENO = C.PAGENO ";
                SQL = SQL + ComNum.VBLF + "    AND C.TREATNO = " + VB.Val(mstrACPNO);
                SQL = SQL + ComNum.VBLF + "    AND C.PAGE > 0 ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.EMR_FORMT F ";
                SQL = SQL + ComNum.VBLF + "    ON C.FORMCODE = F.FORMCODE ";
                if (VB.Val(mstrFORMCODE) > 0)
                {
                    SQL = SQL + ComNum.VBLF + "    AND F.FORMCODE = '" + mstrFORMCODE + "' ";
                }
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.EMR_TREATT T ";
                SQL = SQL + ComNum.VBLF + "    ON C.TREATNO = T.TREATNO ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.EMR_PATHT S ";
                SQL = SQL + ComNum.VBLF + "    ON P.PATHID = S.PATHID ";
                SQL = SQL + ComNum.VBLF + "ORDER BY F.ORDERBY, C.FORMCODE, C.PAGE ";
                //SQL = SQL + ComNum.VBLF + "ORDER BY F.ORDERBY, C.FORMCODE ASC , C.PAGE DESC ";

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
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    return;
                }

                spScan_Sheet1.RowCount = dt.Rows.Count;
                spScan_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sPTNO)].Text = dt.Rows[i]["PATID"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sACPNO)].Text = dt.Rows[i]["TREATNO"].ToString().Trim();
                    //spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sCLSNO)].Text = dt.Rows[i]["CLSNO"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sEMRNO)].Text = "0"; // dt.Rows[i]["EMRNO"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSCANNO)].Text = dt.Rows[i]["PAGENO"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSEQNO)].Text = "0";    // dt.Rows[i]["SEQNO"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sFORMNO)].Text = "";    // dt.Rows[i]["FORMNO"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sFORMNOCHG)].Text = "";     // dt.Rows[i]["FORMNO"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sFORMNAME)].Text = dt.Rows[i]["NAME"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sIMGPATH)].Text = dt.Rows[i]["SVRFILEPATH"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sIMGNAME)].Text = dt.Rows[i]["PAGENO"].ToString().Trim() + "." + dt.Rows[i]["EXTENSION"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sIMGSVR)].Text = dt.Rows[i]["PATHID"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSEVERPATH)].Text = dt.Rows[i]["SVRFILEPATH"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sIMGEXTENSION)].Text = dt.Rows[i]["EXTENSION"].ToString().Trim();

                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSVRIP)].Text = dt.Rows[i]["IPADDRESS"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSVRID)].Text = dt.Rows[i]["FTPUSER"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSVRPW)].Text = dt.Rows[i]["FTPPASSWD"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSVRHOME)].Text = dt.Rows[i]["LOCALPATH"].ToString().Trim();

                }
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                if (spScan_Sheet1.RowCount > 0)
                {
                    DspDownChart();
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }
        #endregion

        #region 신청일자에 해당 환자에 대해 신청한 목록 뿌려주는 함수.
        /// <summary>
        /// 신청일자에 해당 환자에 대해 신청한 목록 뿌려주는 함수.
        /// </summary>
        private void LoadScanCopyList()
        {
            if (mstrPrtDate.Length == 0 || mstrPtno.Length == 0 || mstrUseId.Length == 0)
            {
                return;
            }

            spScan_Sheet1.RowCount = 0;
            DataTable dt = null;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                string SQL = string.Empty;
                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    C.TREATNO, C.PAGENO, C.PAGE, P.PATHID,  ";
                SQL = SQL + ComNum.VBLF + "    CASE  ";
                SQL = SQL + ComNum.VBLF + "        WHEN P.EXTENSION = '' OR P.EXTENSION IS NULL THEN 'tif'  ";
                SQL = SQL + ComNum.VBLF + "        ELSE P.EXTENSION  ";
                SQL = SQL + ComNum.VBLF + "    END AS EXTENSION,  ";
                SQL = SQL + ComNum.VBLF + "    C.SECURITY, P.FILESIZE, P.CDATE, F.NAME,  ";
                SQL = SQL + ComNum.VBLF + "    C.FORMCODE, C.UNREADY, C.CDNO, F.NAME ,T.CLASS , ";
                SQL = SQL + ComNum.VBLF + "    (SELECT C1.NAME  ";
                SQL = SQL + ComNum.VBLF + "        FROM ADMIN.EMR_CLINICT C1  ";
                SQL = SQL + ComNum.VBLF + "        WHERE C1.CLINCODE = T.CLINCODE) AS LOCATIONNM,  ";
                SQL = SQL + ComNum.VBLF + "    T.INDATE, P.LOCATION, ";
                SQL = SQL + ComNum.VBLF + "    S.IPADDRESS, S.FTPUSER, S.FTPPASSWD, S.LOCALPATH, ";
                SQL = SQL + ComNum.VBLF + "    ( S.LOCALPATH || '/' || P.LOCATION ) AS SVRFILEPATH  ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.EMR_PAGET P  ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.EMR_CHARTPAGET C ";
                SQL = SQL + ComNum.VBLF + "    ON P.PAGENO = C.PAGENO ";
                SQL = SQL + ComNum.VBLF + "    AND C.PAGE > 0 ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.EMR_FORMT F ";
                SQL = SQL + ComNum.VBLF + "    ON C.FORMCODE = F.FORMCODE ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.EMR_TREATT T ";
                SQL = SQL + ComNum.VBLF + "    ON C.TREATNO = T.TREATNO ";
                SQL = SQL + ComNum.VBLF + "    AND T.PATID = '" + mstrPtno + "'";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.EMR_PATHT S ";
                SQL = SQL + ComNum.VBLF + "    ON P.PATHID = S.PATHID ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.EMR_PRINTNEEDT D";
                SQL = SQL + ComNum.VBLF + "    ON  D.CDATE = '" + mstrPrtDate  + "'";
                SQL = SQL + ComNum.VBLF + "    AND D.CUSERID = '" + mstrUseId  + "'";
                SQL = SQL + ComNum.VBLF + "    AND D.PAGENO = C.PAGENO";
                SQL = SQL + ComNum.VBLF + "ORDER BY F.ORDERBY, C.FORMCODE, C.PAGE ";
                //SQL = SQL + ComNum.VBLF + "ORDER BY F.ORDERBY, C.FORMCODE ASC , C.PAGE DESC ";

                string SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    return;
                }

                spScan_Sheet1.RowCount = dt.Rows.Count;
                spScan_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sACPNO)].Text = dt.Rows[i]["TREATNO"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSCANNO)].Text = dt.Rows[i]["PAGENO"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sFORMNAME)].Text = dt.Rows[i]["NAME"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sIMGPATH)].Text = dt.Rows[i]["SVRFILEPATH"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sIMGNAME)].Text = dt.Rows[i]["PAGENO"].ToString().Trim() + "." + dt.Rows[i]["EXTENSION"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sIMGSVR)].Text = dt.Rows[i]["PATHID"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSEVERPATH)].Text = dt.Rows[i]["SVRFILEPATH"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sIMGEXTENSION)].Text = dt.Rows[i]["EXTENSION"].ToString().Trim();

                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSVRIP)].Text = dt.Rows[i]["IPADDRESS"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSVRID)].Text = dt.Rows[i]["FTPUSER"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSVRPW)].Text = dt.Rows[i]["FTPPASSWD"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSVRHOME)].Text = dt.Rows[i]["LOCALPATH"].ToString().Trim();

                }
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                if (spScan_Sheet1.RowCount > 0)
                {
                    DspDownChart();
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }
        #endregion

        private void LoadScanChart()
        {
            //if (VB.Val(mstrACPNO) == 0)
            //{
            //    return;
            //}

            spScan_Sheet1.RowCount = 0;

            int i = 0;
            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                
                Cursor.Current = Cursors.WaitCursor;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT E.PTNO, E.ACPNO, E.EMRNO, E.CHARTDATE, E.FORMNO, B.FORMNAME,   ";
                SQL = SQL + ComNum.VBLF + "    S.SCANNO, S.SEQNO, S.IMGSVR, S.IMGPATH, S.IMGNAME, S.IMGEXTENSION, ";
                SQL = SQL + ComNum.VBLF + "    Z.BASNAME AS SVRIP, Z.REMARK1 AS SVRID, Z.REMARK2 AS SVRPW, ";
                SQL = SQL + ComNum.VBLF + "    Z.BASEXNAME AS SVRPATH, Z.VFLAG1 AS SVRHOME ";
                SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_EMR + "AEMRCHARTMST E  ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRSCAN S  ";
                SQL = SQL + ComNum.VBLF + "    ON E.EMRNO = S.EMRNO ";
                SQL = SQL + ComNum.VBLF + "    AND S.DELYN = '0'";
                SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRFORM B ";
                SQL = SQL + ComNum.VBLF + "      ON E.FORMNO = B.FORMNO ";
                SQL = SQL + ComNum.VBLF + "      AND E.UPDATENO = (SELECT MAX(B1.UPDATENO) FROM " + ComNum.DB_EMR + "AEMRFORM B1 ";
                SQL = SQL + ComNum.VBLF + "                            WHERE B1.FORMNO = B.FORMNO) ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRBASCD Z ";
                SQL = SQL + ComNum.VBLF + "    ON Z.BASCD = S.IMGSVR ";
                SQL = SQL + ComNum.VBLF + "    AND Z.BSNSCLS = '스캔관리' ";
                SQL = SQL + ComNum.VBLF + "    AND Z.UNITCLS = '스캔PATH' ";
                SQL = SQL + ComNum.VBLF + "WHERE E.EMRNO = " + mstrEmrNo.ToString();
                SQL = SQL + ComNum.VBLF + "ORDER BY S.SEQNO ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default ;
                    return ;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                spScan_Sheet1.RowCount = dt.Rows.Count;
                spScan_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sPTNO)].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sACPNO)].Text = dt.Rows[i]["ACPNO"].ToString().Trim();
                    //spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sCLSNO)].Text = dt.Rows[i]["CLSNO"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sEMRNO)].Text = dt.Rows[i]["EMRNO"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSCANNO)].Text = dt.Rows[i]["SCANNO"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSEQNO)].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sFORMNO)].Text = dt.Rows[i]["FORMNO"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sFORMNOCHG)].Text = dt.Rows[i]["FORMNO"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sFORMNAME)].Text = dt.Rows[i]["FORMNAME"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sIMGPATH)].Text = dt.Rows[i]["IMGPATH"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sIMGNAME)].Text = dt.Rows[i]["IMGNAME"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sIMGSVR)].Text = dt.Rows[i]["IMGSVR"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSEVERPATH)].Text = dt.Rows[i]["SVRPATH"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sIMGEXTENSION)].Text = dt.Rows[i]["IMGEXTENSION"].ToString().Trim();

                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSVRIP)].Text = dt.Rows[i]["SVRIP"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSVRID)].Text = dt.Rows[i]["SVRID"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSVRPW)].Text = dt.Rows[i]["SVRPW"].ToString().Trim();
                    spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSVRHOME)].Text = dt.Rows[i]["SVRHOME"].ToString().Trim();

                }
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                if (spScan_Sheet1.RowCount > 0)
                {
                    DspDownChart();
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void DspDownChart()
        {
            int i;
            

            if (Directory.Exists(mstrViewPath) == false)
            {
                Directory.CreateDirectory(mstrViewPath);
            }

            if (clsScan.DeleteFoldAll(mstrViewPath) == false)
            {
                ComFunc.MsgBoxEx(this, "작업 폴드의 파일 삭제중 에러가 발생했습니다.");
                return;
            }

            if (spScan_Sheet1.RowCount == 0) return;

            Cursor.Current = Cursors.WaitCursor;

            Ftpedt FtpedtX = new Ftpedt();
            bool ftpCon = false;

            try
            {
                lblTot.Text = string.Empty;
                lblCur.Text = string.Empty;
                lblTot.Text = spScan_Sheet1.RowCount.ToString();
                Application.DoEvents();

                string strServerAddress = string.Empty;
                string strUserName = string.Empty;
                string strPassword = string.Empty;

                string strFileNm   = string.Empty;
                string strSvrPath  = string.Empty;
                string strFORMNAME = string.Empty;

                for (i = 0; i < spScan_Sheet1.RowCount; i++)
                {
                    strServerAddress = spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSVRIP)].Text.Trim();
                    strUserName = spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSVRID)].Text.Trim();
                    strPassword = spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSVRPW)].Text.Trim();
                    break;
                }

                FtpedtX.FtpConBatchEx = FtpedtX.FtpConnetBatchEx(strServerAddress, strUserName, strPassword);
                if (FtpedtX.FtpConBatchEx == null)
                {
                    return;
                }

                //string strSvrIp = "";
                for (i = 0; i < spScan_Sheet1.RowCount; i++)
                {
                    //if (strSvrIp != spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSVRIP)].Text.Trim())
                    //{
                    //    strServerAddress = spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSVRIP)].Text.Trim();
                    //    intServerPort = (int)VB.Val(clsType.SvrInfo.strPort);
                    //    strUserName = spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSVRID)].Text.Trim();
                    //    strPassword = spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSVRPW)].Text.Trim();
                    //}
                    //strSvrIp = spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSVRIP)].Text.Trim();

                    strServerAddress = spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSVRIP)].Text.Trim();
                    strUserName = spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSVRID)].Text.Trim();
                    strPassword = spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSVRPW)].Text.Trim();
                    strFileNm = spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sIMGNAME)].Text.Trim();
                    strFORMNAME = spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sFORMNAME)].Text.Trim();
                    strSvrPath = spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSEVERPATH)].Text.Trim();
                    strSvrPath = strSvrPath.Replace("\\", "/");
                    bool blnDown = FtpedtX.FtpDownloadBatchEx(FtpedtX.FtpConBatchEx,mstrViewPath + "\\" + strFileNm, strFileNm, strSvrPath); //파일다운로드
                    if (blnDown == false)
                    {
                        return;
                    }

                    FileInfo file1 = new FileInfo(mstrViewPath + "\\" + strFileNm);
                    if (file1.Exists == true)
                    {
                        mtsThumbNail.AddThumbnail(mstrViewPath + "\\" + strFileNm, strFORMNAME);
                        mtsThumbNail.Controls[mtsThumbNail.Controls.Count - 1].Tag = spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sACPNO)].Text.Trim();
                        string strExt = file1.Extension;
                        string strCytFile = mstrViewPath + "\\" + strFileNm.Replace(file1.Extension, ".env");
                        clsCyper.Encrypt(mstrViewPath + "\\" + strFileNm, strCytFile);

                        if(mMedicalTeam == false)
                        {
                            File.Delete(mstrViewPath + "\\" + strFileNm);
                        }

                        lblCur.Text = (i + 1).ToString();
                        Application.DoEvents();
                    }

                }

                FtpedtX.FtpDisConnetBatchEx(FtpedtX.FtpConBatchEx);
                FtpedtX = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (ftpCon == true)
                {
                    FtpedtX.FtpDisConnetBatchEx(FtpedtX.FtpConBatchEx);
                }
                FtpedtX = null;
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }
        
        #region //이미지 핸들링
        private void mbtn0Left_Click(object sender, EventArgs e)
        {
            int i;
            for (i = 0; i < mtsThumbNail.Controls.Count; i++)
            {
                if (mtsThumbNail.ItemX(i).Selected == true)
                {
                    mtsImgMainB.ImagePath = mtsThumbNail.ItemX(i).ImagePath;
                    mtsImgMainB.DisplayImage();
                    mtsImgMainB.RotateImage(90, (short)1);
                }
            }
        }

        private void mbtn0Right_Click(object sender, EventArgs e)
        {
            int i;
            for (i = 0; i < mtsThumbNail.Controls.Count; i++)
            {
                if (mtsThumbNail.ItemX(i + 1).Selected == true)
                {
                    mtsImgMainB.ImagePath = mtsThumbNail.ItemX(i + 1).ImagePath;
                    mtsImgMainB.DisplayImage();
                    mtsImgMainB.RotateImage(270, (short)1);
                }
            }
        }

        private void mbtn0Reverse_Click(object sender, EventArgs e)
        {
            int i;
            for (i = 0; i < mtsThumbNail.Controls.Count; i++)
            {
                if (mtsThumbNail.ItemX(i + 1).Selected == true)
                {
                    mtsImgMainB.ImagePath = mtsThumbNail.ItemX(i + 1).ImagePath;
                    mtsImgMainB.DisplayImage();
                    mtsImgMainB.RotateImage(180, (short)1);
                }
            }
        }

        private void mbtn0FlipX_Click(object sender, EventArgs e)
        {
            int i;
            for (i = 0; i < mtsThumbNail.Controls.Count; i++)
            {
                if (mtsThumbNail.ItemX(i + 1).Selected == true)
                {
                    mtsImgMainB.ImagePath = mtsThumbNail.ItemX(i + 1).ImagePath;
                    mtsImgMainB.DisplayImage();
                    mtsImgMainB.FlipImage("X", (short)1);
                }
            }
        }

        private void mbtn0FlipY_Click(object sender, EventArgs e)
        {
            int i;
            for (i = 0; i < mtsThumbNail.Controls.Count; i++)
            {
                if (mtsThumbNail.ItemX(i + 1).Selected == true)
                {
                    mtsImgMainB.ImagePath = mtsThumbNail.ItemX(i + 1).ImagePath;
                    mtsImgMainB.DisplayImage();
                    mtsImgMainB.FlipImage("Y", (short)1);
                }
            }
        }

        private void mbtn1Left_Click(object sender, EventArgs e)
        {
            mtsImgMainB.RotateImage(270, 0);
        }

        private void mbtn1Right_Click(object sender, EventArgs e)
        {
            mtsImgMainB.RotateImage(90, 0);
        }

        private void mbtn1Reverse_Click(object sender, EventArgs e)
        {
            mtsImgMainB.RotateImage(180, 0);
        }

        private void mbtn1FlipX_Click(object sender, EventArgs e)
        {
            mtsImgMainB.FlipImage("X", 0);
        }

        private void mbtn1FlipY_Click(object sender, EventArgs e)
        {
            mtsImgMainB.FlipImage("Y", 0);
        }

        private void mbtn1ZoomIn_Click(object sender, EventArgs e)
        {
            mtsImgMainB.ZoomInImage();
        }

        private void mbtn1ZoomOut_Click(object sender, EventArgs e)
        {
            mtsImgMainB.ZoomOutImage();
        }

        private void mtsThumbNail_ThumbnailClick(mtsImgList.ThumbImage Sender, MouseEventArgs e)
        {
            //mSelectImagePath = mtsThumbNail.SelectedThumbnail.ImagePath;
            //mtsImgMainB.ImagePath = mSelectImagePath;
            //mtsImgMainB.DisplayImage();
        }

        private void mtsThumbNail_ThumbnailDoubleClick(mtsImgList.ThumbImage Sender, MouseEventArgs e)
        {
            mintSelectImg = -1;
            mSelectImagePath = "";
            mtsImgMainB.ClearImage();

            mintSelectImg = mtsThumbNail.Controls.IndexOf(Sender);
            mSelectImagePath = mtsThumbNail.SelectedThumbnail.ImagePath;
            
            string strCytFile = mSelectImagePath.Replace("tif", "env");
            clsCyper.Decrypt(strCytFile, mSelectImagePath);

            mtsImgMainB.ImagePath = mSelectImagePath;
            mtsImgMainB.DisplayImage();

            File.Delete(mSelectImagePath);

            tabImage.SelectedTab = tabImage.TabPages[1];
        }

        private void mtsImgMainB_ImgDblClick(object sender, EventArgs e)
        {
            tabImage.SelectedTab = tabImage.TabPages[0];
        }
        
        private void mbtnSmall_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                for (int i = 0; i < spScan_Sheet1.RowCount; i++)
                {
                    string strFileNm = spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sIMGNAME)].Text.Trim();
                    string strFORMNAME = spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sFORMNAME)].Text.Trim();

                    string strCytFile = mstrViewPath + "\\" + strFileNm.Replace("tif", "env");
                    FileInfo file1 = new FileInfo(strCytFile);
                    if (file1.Exists == true)
                    {
                        clsCyper.Decrypt(strCytFile, mstrViewPath + "\\" + strFileNm);
                    }
                }

                tabImage.SelectedTab = tabImageList;
                mtsThumbNail.ChangeThumbnailSize(200);

                ComFunc.Delay(1000);

                for (int i = 0; i < spScan_Sheet1.RowCount; i++)
                {
                    string strFileNm = spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sIMGNAME)].Text.Trim();
                    string strFORMNAME = spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sFORMNAME)].Text.Trim();

                    FileInfo file1 = new FileInfo(mstrViewPath + "\\" + strFileNm);
                    if (file1.Exists == true)
                    {
                        File.Delete(mstrViewPath + "\\" + strFileNm);
                    }
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void mbtnBig_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                for (int i = 0; i < spScan_Sheet1.RowCount; i++)
                {
                    string strFileNm = spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sIMGNAME)].Text.Trim();
                    string strFORMNAME = spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sFORMNAME)].Text.Trim();

                    string strCytFile = mstrViewPath + "\\" + strFileNm.Replace("tif", "env");
                    FileInfo file1 = new FileInfo(strCytFile);
                    if (file1.Exists == true)
                    {
                        clsCyper.Decrypt(strCytFile, mstrViewPath + "\\" + strFileNm);
                    }
                }

                tabImage.SelectedTab = tabImageList;
                mtsThumbNail.ChangeThumbnailSize(800);

                ComFunc.Delay(1000);

                for (int i = 0; i < spScan_Sheet1.RowCount; i++)
                {
                    string strFileNm = spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sIMGNAME)].Text.Trim();
                    string strFORMNAME = spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sFORMNAME)].Text.Trim();

                    FileInfo file1 = new FileInfo(mstrViewPath + "\\" + strFileNm);
                    if (file1.Exists == true)
                    {
                        File.Delete(mstrViewPath + "\\" + strFileNm);
                    }
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }
        
        #endregion //이미지 핸들링

        #region //출력관련
        private void btnPrint_Click(object sender, EventArgs e)
        {
            frmEmrPrintOption frmEmrPrintOptionX = new frmEmrPrintOption();
            frmEmrPrintOptionX.ShowDialog();

            if (clsFormPrint.mstrPRINTFLAG == "-1")
            {
                return;
            }

            if (SaveEmrScanPrnYn() == false)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            ScanPrint();
            Cursor.Current = Cursors.Default;
        }

        private bool SaveEmrScanPrnYn()
        {
            int i = 0;
            int intSelectCnt = 0;

            if (spScan_Sheet1.RowCount <= 0)
            {
                ComFunc.MsgBoxEx(this, "출력할 이미지가 없습니다.");
                return false;
            }

            for (i = 0; i < mtsThumbNail.Controls.Count; i++)
            {
                if (mtsThumbNail.ItemX(i + 1).Selected == true)
                {
                    intSelectCnt += 1;
                }
            }

            if (intSelectCnt <= 0)
            {
                ComFunc.MsgBoxEx(this, "출력할 이미지를 선택해 주십시요.");
                return false;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                string strPrintDate = VB.Left(strCurDateTime, 8);
                string strPrintTime = VB.Right(strCurDateTime, 6);
                string strSCANNO = "0";

                for (i = 0; i < mtsThumbNail.Controls.Count; i++)
                {
                    if (mtsThumbNail.ItemX(i + 1).Selected == true)
                    {
                        strSCANNO = spScan_Sheet1.Cells[i, 0].Text.Trim();

                        if (clsEmrQuery.SaveEmrPrintHis(clsDB.DbCon, mstrEmrNo, clsFormPrint.mstrPRINTFLAG, "00",
                                                            strPrintDate, strPrintTime, clsType.User.IdNumber, "0", strSCANNO) == false)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void ScanPrint()
        {
            try
            {
                #region 프린터 관련 변수
                System.Drawing.Printing.PrintDocument ScanPrint = new System.Drawing.Printing.PrintDocument();
                System.Drawing.Printing.PrintController printController = new System.Drawing.Printing.StandardPrintController();
                ScanPrint.PrintController = printController;  //기본인쇄창 없애기
                System.Drawing.Printing.PageSettings ps = new System.Drawing.Printing.PageSettings();
                ps.Margins = new System.Drawing.Printing.Margins(10, 10, 10, 10);
                ScanPrint.DefaultPageSettings = ps;
                ScanPrint.PrintPage += pBox_PrintPageEx;
                #endregion

                int i = 0;
                for (i = 0; i < mtsThumbNail.Controls.Count; i++)
                {
                    if (i >= mtsThumbNail.Controls.Count - 1 && mtsThumbNail.ItemX(i + 1).Selected == false)
                    {
                        break;
                    }
                    if (mtsThumbNail.ItemX(i + 1).Selected == true)
                    {
                        string strImagePath = mtsThumbNail.ItemX(i + 1).ImagePath;
                        string strCytFile = mtsThumbNail.ItemX(i + 1).ImagePath.Replace("tif", "env");
                        clsCyper.Decrypt(strCytFile, strImagePath);

                        mintPrint = i + 1;
                        ScanPrint.Print();
                        string strPageNo = spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSCANNO)].Text;

                        //추후 주석 제거 필요.
                        PrtLogInsert(strPageNo, mMedicalTeam ? "002" : "003");

                        if (clsEmrQuery.SaveEmrXmlPrnYnForm(clsDB.DbCon, "0", strPageNo) == false)
                        {
                            return;
                        }
                    }
                }

                #region 프린터 변수 삭제
                ScanPrint.Dispose();
                ScanPrint.PrintPage -= pBox_PrintPageEx;
                #endregion
                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void pBox_PrintPageEx(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            #region 이미지 불러오기
            Image pImage = GetImage(mtsThumbNail.ItemX(mintPrint).ImagePath);
            #endregion

            if (pImage.Size.Width > pImage.Size.Height)
            {
                pImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                e.Graphics.DrawImage(pImage, 10, 10, 760, 1100); //'가로 이미지 크기
            }
            else
            {
                e.Graphics.DrawImage(pImage, 10, 10, 760, 1100); //'세로 이미지 크기
            }

            #region 이미지 컨트롤 삭제
            pImage.Dispose();
            #endregion
        }

        private void pBox_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            for (int i = 0; i < mtsThumbNail.Controls.Count; i++)
            {
                if (i >= mtsThumbNail.Controls.Count - 1 && mtsThumbNail.ItemX(i + 1).Selected == false)
                {
                    e.HasMorePages = false;
                }
                if (mtsThumbNail.ItemX(i + 1).Selected == true)
                {
                    if (i >= mtsThumbNail.Controls.Count - 1)
                    {
                        e.HasMorePages = false;
                    }
                    else
                    {
                        e.HasMorePages = true;
                    }

                    Image pImage = GetImage(mtsThumbNail.ItemX(i + 1).ImagePath);

                    if (pImage.Size.Width > pImage.Size.Height)
                    {
                        e.Graphics.DrawImage(pImage, 10, 10, 900, 600); //'가로 이미지 크기
                    }
                    else
                    {
                        e.Graphics.DrawImage(pImage, 10, 10, 600, 900); //'세로 이미지 크기
                    }

                    pImage.Dispose();
                    
                }

            }
        }

        private void btnPrintB_Click(object sender, EventArgs e)
        {
            if (spScan_Sheet1.RowCount <= 0)
            {
                ComFunc.MsgBoxEx(this, "출력할 이미지가 없습니다.");
                return;
            }

            if (mintSelectImg < 0)
            {
                ComFunc.MsgBoxEx(this, "출력할 이미지를 선택해 주십시요.");
                return;
            }

            frmEmrPrintOption frmEmrPrintOptionX = new frmEmrPrintOption();
            frmEmrPrintOptionX.ShowDialog();

            if (clsFormPrint.mstrPRINTFLAG == "-1")
            {
                return;
            }

            if (SaveEmrScanPrnYnOne() == false)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            ScanPrintOne();
            Cursor.Current = Cursors.Default;
        }

        private bool SaveEmrScanPrnYnOne()
        {
            if (spScan_Sheet1.RowCount <= 0)
            {
                ComFunc.MsgBoxEx(this, "출력할 이미지가 없습니다.");
                return false;
            }

            if (mintSelectImg < 0)
            {
                ComFunc.MsgBoxEx(this, "출력할 이미지를 선택해 주십시요.");
                return false;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                string strPrintDate = VB.Left(strCurDateTime, 8);
                string strPrintTime = VB.Right(strCurDateTime, 6);
                string strSCANNO = "0";

                strSCANNO = spScan_Sheet1.Cells[mintSelectImg, 0].Text.Trim();

                if (clsEmrQuery.SaveEmrPrintHis(clsDB.DbCon, mstrEmrNo, clsFormPrint.mstrPRINTFLAG, "00",
                                                    strPrintDate, strPrintTime, clsType.User.IdNumber, "0", strSCANNO) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void ScanPrintOne()
        {
            try
            {
                System.Drawing.Printing.PrintDocument ScanPrint = new System.Drawing.Printing.PrintDocument();
                System.Drawing.Printing.PrintController printController = new System.Drawing.Printing.StandardPrintController();
                ScanPrint.PrintController = printController;  //기본인쇄창 없애기

                System.Drawing.Printing.PageSettings ps = new System.Drawing.Printing.PageSettings();
                //ps.PrinterSettings.PrinterName = "Microsoft XPS Document Writer";
                ps.Margins = new System.Drawing.Printing.Margins(10, 10, 10, 10);
                ScanPrint.DefaultPageSettings = ps;
                ScanPrint.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(pBox_PrintPageOne);
                ScanPrint.Print();
                ScanPrint.Dispose();
                ScanPrint.PrintPage -= new System.Drawing.Printing.PrintPageEventHandler(pBox_PrintPageOne);

                //PrintPreviewDialog pd = new PrintPreviewDialog();
                //pd.Document = ScanPrint;
                //pd.ShowDialog();
                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void pBox_PrintPageOne(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //Bitmap image1 = (Bitmap)Image.FromFile(mtsThumbNail.ItemX(mintSelectImg + 1).ImagePath, true);

            string strImagePath = mSelectImagePath;
            string strCytFile = mSelectImagePath.Replace("tif", "env");
            clsCyper.Decrypt(strCytFile, strImagePath);
            Image pImage = GetImage(strImagePath);

            string strPageNo = spScan_Sheet1.Cells[mintSelectImg, Convert.ToInt32(clsScanPublic.ScanSp.sSCANNO)].Text;

            //추후 주석 제거 필요.
            PrtLogInsert(strPageNo, mMedicalTeam ? "002" : "003");

            if (clsEmrQuery.SaveEmrXmlPrnYnForm(clsDB.DbCon, "0", strPageNo) == false)
            {
                return;
            }

            if (pImage.Size.Width > pImage.Size.Height)
            {
                e.Graphics.DrawImage(pImage, 30, 30, 1100, 760); //'가로 이미지 크기
            }
            else
            {
                e.Graphics.DrawImage(pImage, 30, 30, 760, 1100); //'세로 이미지 크기
            }

            pImage.Dispose();

            e.HasMorePages = false;
        }

        /// <summary>
        /// Image 불러오기
        /// </summary>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        private Image GetImage(string strFileName)
        {
            Image rtnVal = null;

            byte[] buff = File.ReadAllBytes(strFileName);
            using (MemoryStream ms = new MemoryStream(buff))
            {
                rtnVal = Image.FromStream(ms);
            }

            File.Delete(strFileName);
            return rtnVal;
        }

        #endregion //출력관련

        #region 복사신청
        private void btnSaveCopy_Click(object sender, EventArgs e)
        {

            if (spScan_Sheet1.RowCount <= 0)
            {
                ComFunc.MsgBoxEx(this, "복사신청할 이미지가 없습니다.");
                return;
            }

            int intSelectCnt = 0;
            List<int> lstRow = new List<int>();
            string strSysDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            string strPtno = spScan_Sheet1.Cells[0, Convert.ToInt32(clsScanPublic.ScanSp.sPTNO)].Text;

            for (int i = 0; i < mtsThumbNail.Controls.Count; i++)
            {
                string strPageNo = spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSCANNO)].Text;
                if (mtsThumbNail.ItemX(i + 1).Selected == true)
                {
                    if (DuplicateScan(strSysDate, clsType.User.IdNumber, strPtno, strPageNo))
                    {
                        ComFunc.MsgBoxEx(this, string.Format("{0}번째 항목은 이미 신청한 항목입니다. {1}다시 확인해주세요.", (i + 1), ComNum.VBLF));
                        return;
                    }
                    intSelectCnt += 1;
                    lstRow.Add(i);
                }
            }

            if (intSelectCnt <= 0)
            {
                ComFunc.MsgBoxEx(this, "복사신청할 이미지를 선택해 주십시요.");
                return;
            }
          

            int intTotCopyCnt = 0;
            if (CopyInsert(lstRow, ref intTotCopyCnt))
            {
                ComFunc.MsgBoxEx(this, string.Format("총 {0}개의 항목에 대하여 복사 신청을 완료했습니다.", intTotCopyCnt));
            }
        }

        /// <summary>
        /// 당일날 해당환자 해당 스캔 항목 신청했는지 검사
        /// </summary>
        /// <param name="strDate"></param>
        /// <param name="strUseId"></param>
        /// <param name="strPtno"></param>
        /// <returns></returns>
        bool DuplicateScan(string strDate, string strUseId, string strPtno, string strPageNo)
        {
            bool rtnVal = false;
            string SQL = string.Empty;
            OracleDataReader reader = null;

            try
            {

                SQL = "SELECT COUNT(*) CNT";
                SQL += ComNum.VBLF + " FROM ADMIN.EMR_PRINTNEEDT N, ADMIN.EMR_CHARTPAGET C,";
                SQL += ComNum.VBLF + "      ADMIN.EMR_TREATT T";
                SQL += ComNum.VBLF + " WHERE N.CDATE = '" + strDate + "' ";
                SQL += ComNum.VBLF + " AND N.PAGENO  = '" + strPageNo + "' ";
                SQL += ComNum.VBLF + " AND N.CUSERID = '" + strUseId + "' ";
                SQL += ComNum.VBLF + " AND N.PRINTCODE = '002'";
                SQL += ComNum.VBLF + " AND T.PATID = '" + strPtno + "' ";
                SQL += ComNum.VBLF + " AND C.PAGENO = N.PAGENO ";
                SQL += ComNum.VBLF + " AND T.TREATNO = C.TREATNO ";

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    return rtnVal;
                }

                if(reader.HasRows && reader.Read() && VB.Val(reader.GetValue(0).ToString().Trim()) > 0)
                {
                    rtnVal = true;
                }

                reader.Dispose();
            }
            catch(Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            } 
            return rtnVal;
        }

        /// <summary>
        /// 복사신청 함수
        /// </summary>
        /// <param name="lstRow">선택된 Row번호를 가지고 있는 제네릭</param>
        /// <param name="InsertCnt">총 Insert한갯수를 넘겨받을 변수</param>
        /// <returns></returns>
        bool CopyInsert(List<int> lstRow, ref int InsertCnt)
        {
            bool rtnVal = false;
            string SQL = string.Empty;
            int RowAffected = 0;

            double dPrtCnt = string.IsNullOrEmpty(txtPrtCnt.Text) || VB.IsNumeric(txtPrtCnt.Text) == false ? 1 : VB.Val(txtPrtCnt.Text);

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for(int i = 0; i < lstRow.Count; i++)
                {
                    string strPageNo  = spScan_Sheet1.Cells[lstRow[i], Convert.ToInt32(clsScanPublic.ScanSp.sSCANNO)].Text;
                    string strTreatNo = spScan_Sheet1.Cells[lstRow[i], Convert.ToInt32(clsScanPublic.ScanSp.sACPNO)].Text;

                    SQL = "INSERT INTO ADMIN.EMR_PRINTNEEDT(";
                    SQL += ComNum.VBLF + "TREATNO, PAGENO, CUSERID, PRINTCODE,";
                    SQL += ComNum.VBLF + "CDATE, NEEDGUBUN, NEEDCNT";
                    SQL += ComNum.VBLF + ")";
                    SQL += ComNum.VBLF + "VALUES(";
                    SQL += ComNum.VBLF + "'" + strTreatNo + "',";  //TreatNo
                    SQL += ComNum.VBLF + "'" + strPageNo + "',";  //PAGENO
                    SQL += ComNum.VBLF + "'" + clsType.User.IdNumber + "',"; //로그인 사번
                    SQL += ComNum.VBLF + "'002',"; //전송용
                    SQL += ComNum.VBLF + "TO_CHAR(SYSDATE, 'YYYYMMDD'), ";//서버날짜
                    SQL += ComNum.VBLF + "'1', ";//NEEDGUBUN
                    SQL += ComNum.VBLF + dPrtCnt;//요청 갯수
                    SQL += ComNum.VBLF + ")";

                    string sqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                    if (sqlErr.Length > 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, sqlErr);
                        return rtnVal;
                    }

                    InsertCnt += RowAffected;
                }

                rtnVal = true;
                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// 출력로그 함수
        /// </summary>
        /// <param name="lstPageNo">페이지번호 가지고 있는 제네릭</param>
        /// <param name="InsertCnt">총 Insert한 갯수를 넘겨받을 변수</param>
        /// <returns></returns>
        bool PrtLogInsert(string strPageNo, string strPrtCode = "002")
        {
            bool rtnVal = false;
            string SQL = string.Empty;
            int RowAffected = 0;

            //string strDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "INSERT INTO ADMIN.EMR_PAGEPRINTLOGT(";
                SQL += ComNum.VBLF + "PAGENO, PRINTCODE, CUSERID, NEEDUSER, CDATE, CTIME";
                SQL += ComNum.VBLF + ")";
                SQL += ComNum.VBLF + "VALUES(";
                SQL += ComNum.VBLF + "'" + strPageNo  + "',";  //페이지번호
                SQL += ComNum.VBLF + "'" + strPrtCode +"',";   //프린트코드
                SQL += ComNum.VBLF + "'" + clsType.User.Sabun + "',"; //사용자
                SQL += ComNum.VBLF + "'" + clsType.User.Sabun + "',"; //출력자 사번
                SQL += ComNum.VBLF + "TO_CHAR(SYSDATE, 'YYYYMMDD'), ";//서버날짜
                SQL += ComNum.VBLF + "TO_CHAR(SYSDATE, 'hh24miss')  ";//서버시간
                SQL += ComNum.VBLF + ")";

                string sqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return rtnVal;
                }


                SQL = "UPDATE ADMIN.EMR_PRINTNEEDT";
                SQL += ComNum.VBLF + " SET PRINTED = 'Y'";
                SQL += ComNum.VBLF + " WHERE PAGENO = " + strPageNo;
                SQL += ComNum.VBLF + " AND CDATE ='" + mstrPrtDate + "' ";
                SQL += ComNum.VBLF + " AND CUSERID ='" + mstrUseId + "' ";
                SQL += ComNum.VBLF + " AND PRINTCODE ='" + strPrtCode + "' ";

                sqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return rtnVal;
                }

                rtnVal = true;
                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            return rtnVal;
        }

        #endregion

        private void btnLeft_Click(object sender, EventArgs e)
        {
            mSelectImagePath = "";
            mtsImgMainB.ClearImage();

            if (mintSelectImg <= 0)
                mintSelectImg = mtsThumbNail.Controls.Count;

            mintSelectImg -= 1;
            mtsThumbNail.Controls[mintSelectImg].Focus();
            mSelectImagePath = ((ThumbImage)mtsThumbNail.Controls[mintSelectImg]).ImagePath;

            string strCytFile = mSelectImagePath.Replace("tif", "env");
            clsCyper.Decrypt(strCytFile, mSelectImagePath);

            mtsImgMainB.ImagePath = mSelectImagePath;
            mtsImgMainB.DisplayImage();

            File.Delete(mSelectImagePath);

            tabImage.SelectedTab = tabImage.TabPages[1];
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            mSelectImagePath = "";
            mtsImgMainB.ClearImage();

            if (mintSelectImg >= mtsThumbNail.Controls.Count - 1)
                mintSelectImg = -1;


            mintSelectImg += 1;
            mtsThumbNail.Controls[mintSelectImg].Focus();
            mSelectImagePath = ((ThumbImage)mtsThumbNail.Controls[mintSelectImg]).ImagePath;

            string strCytFile = mSelectImagePath.Replace("tif", "env");
            clsCyper.Decrypt(strCytFile, mSelectImagePath);

            mtsImgMainB.ImagePath = mSelectImagePath;
            mtsImgMainB.DisplayImage();

            File.Delete(mSelectImagePath);

            tabImage.SelectedTab = tabImage.TabPages[1];
        }

        private void btnAllChk_Click(object sender, EventArgs e)
        {
            if (mtsThumbNail.Controls.Count == 0)
                return;

            for (int i = 0; i < mtsThumbNail.Controls.Count; i++)
            {
                mtsThumbNail.ItemX(i + 1).Selected = true;
            }
        }
    }
}
