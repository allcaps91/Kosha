using ComBase;
using ComBase.Controls;
using MTSImageListView;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmScanImageViewNew3 : Form, EmrChartForm
    {
        private Form mCallForm = null;
        private string mstrEmrNo = "0";       //EMRNO
        private string mstrACPNO = "0";       //접수번호
        private string mstrFORMCODE = string.Empty;     //스캔서식코드

        //private string mSelectImagePath = string.Empty;
        //private int mintSelectImg = -1;

        /// <summary>
        /// 프린터 출력 장수
        /// </summary>
        private int mintPrint = 0;

        /// <summary>
        /// 선택한 프린터 이름
        /// </summary>
        private string SelectPrtName = string.Empty;

        /// <summary>
        /// 마지막 줌 배열
        /// </summary>
        private double LastZoom = 0;

        #region 연속보기용 컨트롤

        List<PictureBox> pictures = new List<PictureBox>();
        List<Label> labels = new List<Label>();
        List<Panel> panels = new List<Panel>();

        #endregion


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
        /// 차트복사용 과 구분용도.
        /// </summary>
        private string mstrDeptCode = string.Empty;
        /// <summary>
        /// 차트복사용 신청 구분용도
        /// </summary>
        private string mstrNeedGbn = string.Empty;
        /// <summary>
        /// 의료정보팀 여부
        /// </summary>
        private bool mMedicalTeam = false;
        /// <summary>
        /// 차트복사 미출력/출력 조회
        /// </summary>
        private bool mPrint = false;
        /// <summary>
        /// 검사변환 조회 여부
        /// </summary>
        private string mCvtSearch = string.Empty;

        /// <summary>
        /// 뷰어용 폼
        /// </summary>
        frmScanPageView fScanPageView = null;
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

            if (strPRINTFLAG == "0")
            {
                Cursor.Current = Cursors.WaitCursor;
                //for (short i = 0; i < ltkThumb.TotalThumbnail; i++)
                //{
                //    ltkThumb.set_Select((short)(i + 1), true);
                //}

                for(int i = 0; i < imageListView1.Items.Count; i++)
                {
                    imageListView1.Items[i].IsSelected = true;
                }

                PrintF();

                //for (short i = 0; i < ltkThumb.TotalThumbnail; i++)
                //{
                //    if(File.Exists(ltkThumb.get_FileName((short)(i + 1)))) 
                //    {
                //        File.Delete(ltkThumb.get_FileName((short)(i + 1)));
                //    }
                //}

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

        #region 이미지 ㅇ녀속보기
        /// <summary>
        /// 연속보기 클리어
        /// </summary>
        public void ClearItemEx()
        {
            List<PictureBox> temps1 = pictures.Where(r => r.IsDisposed == false).ToList();
            foreach (PictureBox pic in temps1)
            {
                if (pic.IsDisposed == false)
                {
                    pic.Image.Dispose();
                    pic.Dispose();
                }
            }

            List<Label> temps2 = labels.Where(r => r.IsDisposed == false).ToList();
            foreach (Label lbl in temps2)
            {
                if (lbl.IsDisposed == false)
                    lbl.Dispose();
            }

            List<Panel> temps3 = panels.Where(r => r.IsDisposed == false).ToList();
            foreach (Panel pnl in temps3)
            {
                if (pnl.IsDisposed == false)
                    pnl.Dispose();
            }

            imgContinuView.Controls.Clear();
            pictures.Clear();
            labels.Clear();
            panels.Clear();
        }

        public void imgContinuViewAdd(string ImgPath, string Title)
        {
            Panel pnl = new Panel();
            pnl.Margin = new Padding(5, 5, 5, 5);
            pnl.Dock = DockStyle.Top;
            pnl.BorderStyle = BorderStyle.FixedSingle;
            //pnl.Paint += (sender, e) => Pnl_Paint(sender, e, Title, ImgPath);

            //Graphics graphics = pnl.CreateGraphics();

            //using (Font font = new Font("굴림체", 10, FontStyle.Bold))
            //{
            //    graphics.DrawString(Title, font, Brushes.Blue, 5, 10);

            //    using (Image img = GetImage(ImgPath))
            //    {
            //        int mW = mCallForm.Name.Equals("frmEmrJobChartCopy") ? mCallForm.Width : Width;
            //        int mH = mCallForm.Name.Equals("frmEmrJobChartCopy") ? mCallForm.Height : Height;

            //        pnl.Height = img.Height + 40;
            //        graphics.DrawImage(img, 5, 30, mW, img.Height);
            //    }
            //}

            PictureBox pic = new PictureBox();
            pic.Image = GetImage(ImgPath);
            pic.Dock = DockStyle.Fill;
            pic.SizeMode = PictureBoxSizeMode.StretchImage;

            Label lbl = new Label();
            lbl.Text = Title;
            lbl.ForeColor = Color.Blue;
            lbl.TextAlign = ContentAlignment.MiddleLeft;
            lbl.Dock = DockStyle.Top;

            pnl.Height = pic.Image.Height + lbl.Height + 5;


            using (Font font = new Font("굴림체", 10, FontStyle.Bold))
            {
                lbl.Font = font;
            }

            pnl.Controls.Add(pic);
            pnl.Controls.Add(lbl);
            pnl.Name = "pnl_" + imgContinuView.Controls.Count;
            imgContinuView.Controls.Add(pnl);
            imgContinuView.Controls.SetChildIndex(pnl, 0);

            pictures.Add(pic);
            labels.Add(lbl);
            panels.Add(pnl);
        }

        private void Pnl_Paint(object sender, PaintEventArgs e, string Title, string ImgPath)
        {
            using(Font font = new Font("굴림체", 10, FontStyle.Bold))
            {
                e.Graphics.DrawString(Title, font, Brushes.Black, 5, 10);

                using (Image img = GetImage(ImgPath))
                {
                    int mW = mCallForm.Name.Equals("frmEmrJobChartCopy") ? mCallForm.Width : Width;
                    int mH = mCallForm.Name.Equals("frmEmrJobChartCopy") ? mCallForm.Height : Height;

                    (sender as Control).Height = img.Height + 40;
                    e.Graphics.DrawImage(img, 5, 30, mW, img.Height);
                }
            }
        }

        #endregion

        #region
        /// <summary>
        /// 환자 받아서 기록지를 초기화 한다.
        /// </summary>
        public void gPatientinfoRecive(string strACPNO, string strFORMCODE, string strEmrNo)
        {
            picBig1.Visible = false;

            mstrACPNO = strACPNO;
            mstrFORMCODE = strFORMCODE;
            mstrEmrNo = strEmrNo;

            clsEmrPublic.bScanContinuView = chkContinuView.Checked;

            if (chkContinuView.Checked)
            {
                imageListView1.Visible = false;
                imgContinuView.Visible = true;
            }
            else
            {
                imageListView1.Visible = true;
                imgContinuView.Visible = false;
            }

            imageListView1.ClearItem();
            ClearItemEx();

            if (mMedicalTeam == false)
            {
                LoadScanChartPSMH();
                //imageListView1.ImageListItemSize = MTSImageListView.ImageListItemSize.SIZE_4X4;

                if (kpImageViewer1.Visible && imageListView1.Items.Count > 0)
                {
                    LoadScanImage(1);
                    kpImageViewer1.FitToScreen();
                    //ltkPageView.BestFit();
                }
            }
            else
            {
                LoadScanCopyList();
            }
        }

  
        #endregion


        public frmScanImageViewNew3()
        {
            InitializeComponent();
        }

        //public frmScanImageViewNew2(Form pCallForm, string strEmrNo)
        //{
        //    InitializeComponent();
        //    mCallForm = pCallForm;
        //    mstrEmrNo = strEmrNo;
        //}

        /// <summary>
        /// 19-07-29 생성
        /// 19-10-31 수정
        /// 차트복사 전용 생성자
        /// </summary>
        /// <param name="pCallForm">부모 폼</param>
        /// <param name="strVal">환자 데이터</param>
        /// <param name="bMedicalTeam">의료정보팀인지 여부 </param>
        public frmScanImageViewNew3(Form pCallForm, string strVal, bool bMedicalTeam)
        {
            InitializeComponent();
            mCallForm = pCallForm;
            if (strVal.IndexOf("|") == -1)
                return;

            string[] Arr = strVal.Split('|');

            mstrPtno = Arr[0];
            mstrPrtDate = Arr[1];
            mstrUseId = Arr[2];
            mstrDeptCode = Arr[3];
            mstrNeedGbn = Arr[4];
            mPrint = Arr[5].ToUpper().Equals("TRUE");
            mCvtSearch = Arr[6];
            mMedicalTeam = bMedicalTeam;
        }

        public frmScanImageViewNew3(Form pCallForm, string strACPNO, string strFORMCODE, string strEmrNo)
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
            imageListView1.Size = imageListView1.Parent.Size;

            string strCurDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");

            mstrViewPath = mstrViewPathInit + "\\" + strCurDate;

            clsEmrPublic.bScanContinuView = chkContinuView.Checked;

            kpImageViewer1.DecryptExtension = ".env";
            kpImageViewer1.EncryptionKey = "mentorCyper";
            kpImageViewer1.AfterZoom += KpImageViewer1_AfterZoom;

            //if (Directory.Exists(mstrViewPathInit) == true)
            //{
            //    //string[] subdirectoryEntries = Directory.GetDirectories(mstrViewPathInit);
            //    //foreach (string subdirectory in subdirectoryEntries)
            //    //{
            //    //    ComFunc.DeleteFoldAll(subdirectory);
            //    //    Directory.Delete(subdirectory);
            //    //}
            //}

            if (Directory.Exists(mstrViewPath) == false)
            {
                Directory.CreateDirectory(mstrViewPath);
            }

            //심사팀 미사용
            btnCopy.Visible = !clsType.User.BuseCode.Equals("078201");

            //imgContinuView.ImageX_Click += new mtsImageListEx.ImageListV.Image_Click(imgContinuView_ImageX_Click);
            //imgContinuView.ThumbX_DoubleClick += new mtsImageListEx.ImageListV.Image_DoubleClick(imgContinuView_DoubleClick);
            //imgContinuView.ThumbX_MouseDown += new mtsImageListEx.ImageListV.Image_MouseDown(imgContinuView_MouseDown);
            //imgContinuView.ThumbX_MouseUp += new mtsImageListEx.ImageListV.Image_MouseUp(imgContinuView_MouseUp);

            if (mCallForm != null && mCallForm.Name == "frmEmrViewMain")
            {
                //panTitle.Visible = false;
                btnExit.Visible = false;
            }

            picBig1.Visible = false;

            toolButtonRemove.Visible = mMedicalTeam && clsType.User.AuAMANAGE.Equals("1") || clsType.User.JobGroup.Equals("JOB002002") || clsType.User.JobGroup.Equals("JOB002003");

            //차트복사 관리자(의료정보팀), 제증명창구(원무팀) 일경우 열어줌.
            if (clsType.User.AuAPRINTIN.Equals("1") || clsType.User.BuseCode.Equals("077402"))
            {
                btnPrint.Visible = true;
                toolButtonPrint.Visible = true;
            }
            else
            {
                btnPrint.Visible = false;
                toolButtonPrint.Visible = false;
            }

            btnPrintSet.Visible = clsType.User.AuAPRINTIN.Equals("1");
            //gInitUserCombo(cboUser, "0");
            //InitCodeCombo(cboCode);

            #region 초기화

            imageListView1.ClearItem();

            ToolEnable(false);
            #endregion

            Application.DoEvents();

            if (mMedicalTeam == false)
            {
                imageListView1.ImageListItemSize = MTSImageListView.ImageListItemSize.SIZE_4X4;
                LoadScanChartPSMH();
              
            }
            else
            {
                LoadScanCopyList();
                Application.DoEvents();
            }

        }

        private void KpImageViewer1_AfterZoom(object sender, KaiwaProjects.ImageViewerZoomEventArgs e)
        {
            LastZoom = e.Zoom;
        }


        /// <summary>
        /// 메뉴 활성화/비활성화
        /// </summary>
        /// <param name="bVal"></param>
        private void ToolEnable(bool bVal = true)
        {
            //toolButtonLeft.Enabled = bVal;
            //toolButtonRight.Enabled = bVal;
            //toolButtonFitAll.Enabled = bVal;
            //toolButtonFitWidth.Enabled = bVal;
            //toolButtonChartCopy.Enabled    = bVal;
            //toolButtonSelectAll.Enabled      = bVal;
            //toolButtonUnSelectAll.Enabled    = bVal;
            //toolButtonFitWidth.Enabled       = bVal;

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

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cbo.Items.Add(dt.Rows[i]["NAME"].ToString().Trim() + "." + VB.Space(20) + "," + dt.Rows[i]["UserID"].ToString().Trim());
                    }
                }


                dt.Dispose();

                cbo.SelectedIndex = 0;
                rtnVal = true;
            }
            catch (Exception ex)
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
                ClearItemEx();
                imageListView1.ClearItem();
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

            //toolStripcboPage.Items.Clear();
            spScan_Sheet1.RowCount = 0;

            int i = 0;
            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                SQL.AppendLine(" SELECT  ");
                SQL.AppendLine("   T.PATID, C.TREATNO, C.PAGENO, C.PAGE, P.PATHID,  ");
                SQL.AppendLine("    CASE  ");
                SQL.AppendLine("        WHEN P.EXTENSION = '' OR P.EXTENSION IS NULL THEN 'tif'  ");
                SQL.AppendLine("        ELSE P.EXTENSION  ");
                SQL.AppendLine("    END AS EXTENSION,  ");
                SQL.AppendLine("    C.SECURITY, P.FILESIZE, P.CDATE, F.NAME,  ");
                SQL.AppendLine("    C.FORMCODE, C.UNREADY, C.CDNO, F.NAME ,T.CLASS , ");
                SQL.AppendLine("    (SELECT C1.NAME  ");
                SQL.AppendLine("        FROM ADMIN.EMR_CLINICT C1  ");
                SQL.AppendLine("        WHERE C1.CLINCODE = T.CLINCODE) AS LOCATIONNM,  ");
                SQL.AppendLine("    T.INDATE, P.LOCATION, ");
                SQL.AppendLine("    S.IPADDRESS, S.FTPUSER, S.FTPPASSWD, S.LOCALPATH, ");
                //SQL = SQL + ComNum.VBLF + "    ( REPLACE(S.LOCALPATH, '\', '/') || '/' || REPLACE(P.LOCATION, '\', '/') ) AS SVRFILEPATH  ";
                SQL.AppendLine("    ( S.LOCALPATH || '/' || P.LOCATION ) AS SVRFILEPATH  ");
                SQL.AppendLine("FROM ADMIN.EMR_PAGET P  ");
                SQL.AppendLine("INNER JOIN ADMIN.EMR_CHARTPAGET C ");
                SQL.AppendLine("   ON P.PAGENO = C.PAGENO ");
                SQL.AppendLine("  AND C.TREATNO = " + VB.Val(mstrACPNO));
                SQL.AppendLine("  AND C.PAGE > 0 ");
                SQL.AppendLine("INNER JOIN ADMIN.EMR_FORMT F ");
                SQL.AppendLine("   ON C.FORMCODE = F.FORMCODE ");
                if (VB.Val(mstrFORMCODE) > 0)
                {
                    SQL.AppendLine("    AND F.FORMCODE = '" + mstrFORMCODE + "' ");
                }
                SQL.AppendLine("INNER JOIN ADMIN.EMR_TREATT T ");
                SQL.AppendLine("   ON C.TREATNO = T.TREATNO ");
                SQL.AppendLine("INNER JOIN ADMIN.EMR_PATHT S ");
                SQL.AppendLine("   ON P.PATHID = S.PATHID ");
                SQL.AppendLine("ORDER BY F.ORDERBY, C.FORMCODE, C.PAGE ");
                //SQL.AppendLine("ORDER BY F.ORDERBY, C.FORMCODE ASC , C.PAGE DESC ");

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
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

                toolStripcboPage.Items.Clear();

                for (i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    toolStripcboPage.Items.Add(i + 1);

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

            imageListView1.ClearItem();

            switch (mstrNeedGbn)
            {
                case "기타":
                    mstrNeedGbn = "001";
                    break;
                case "전송용":
                    mstrNeedGbn = "002";
                    break;
                case "보험심사용":
                    mstrNeedGbn = "003";
                    break;
            }

            //toolStripcboPage.Items.Clear();
            spScan_Sheet1.RowCount = 0;
            DataTable dt = null;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                StringBuilder SQL = new StringBuilder();
                SQL.AppendLine(" SELECT  ");
                SQL.AppendLine("    TREATNO, PAGENO, PAGE, PATHID,  ");
                SQL.AppendLine("    EXTENSION,  SECURITY, FILESIZE, CDATE, NAME,  ");
                SQL.AppendLine("    FORMCODE, UNREADY, CDNO, NAME , CLASS ,  ");
                SQL.AppendLine("    LOCATIONNM,  ");
                SQL.AppendLine("    INDATE, LOCATION,   ");
                SQL.AppendLine("    IPADDRESS, FTPUSER, FTPPASSWD, LOCALPATH,   ");
                SQL.AppendLine("    SVRFILEPATH  ");
                SQL.AppendLine(" FROM  ");
                SQL.AppendLine(" (  ");

                if (clsType.User.IdNumber.Equals("16109"))
                {
                    if (mCvtSearch.Equals("CVT") == false)
                    {
                        #region 현재 살아있는 스캔
                        SQL.AppendLine(" SELECT  ");
                        SQL.AppendLine("    C.TREATNO, C.PAGENO, C.PAGE, P.PATHID,  ");
                        SQL.AppendLine("    CASE  ");
                        SQL.AppendLine("        WHEN P.EXTENSION = '' OR P.EXTENSION IS NULL THEN 'tif'  ");
                        SQL.AppendLine("        ELSE P.EXTENSION  ");
                        SQL.AppendLine("    END AS EXTENSION,  ");
                        SQL.AppendLine("    C.SECURITY, P.FILESIZE, P.CDATE, F.NAME,  ");
                        SQL.AppendLine("    C.FORMCODE, C.UNREADY, C.CDNO, T.CLASS , ");
                        SQL.AppendLine("    (SELECT C1.NAME  ");
                        SQL.AppendLine("        FROM ADMIN.EMR_CLINICT C1  ");
                        SQL.AppendLine("        WHERE C1.CLINCODE = T.CLINCODE) AS LOCATIONNM,  ");
                        SQL.AppendLine("    T.INDATE, P.LOCATION, ");
                        SQL.AppendLine("    S.IPADDRESS, S.FTPUSER, S.FTPPASSWD, S.LOCALPATH, ");
                        SQL.AppendLine("    ( S.LOCALPATH || '/' || P.LOCATION ) AS SVRFILEPATH  ");
                        SQL.AppendLine("    , F.ORDERBY ");
                        SQL.AppendLine("FROM ADMIN.EMR_PAGET P  ");
                        SQL.AppendLine("INNER JOIN ADMIN.EMR_CHARTPAGET C ");
                        SQL.AppendLine("   ON P.PAGENO = C.PAGENO ");
                        SQL.AppendLine("  AND C.PAGE > 0 ");
                        SQL.AppendLine("INNER JOIN ADMIN.EMR_FORMT F ");
                        SQL.AppendLine("   ON C.FORMCODE = F.FORMCODE ");
                        SQL.AppendLine("INNER JOIN ADMIN.EMR_TREATT T ");
                        SQL.AppendLine("   ON C.TREATNO = T.TREATNO ");
                        SQL.AppendLine("  AND T.PATID = '" + mstrPtno + "'");
                        SQL.AppendLine("  AND T.CLINCODE = '" + mstrDeptCode + "'");
                        SQL.AppendLine("INNER JOIN ADMIN.EMR_PATHT S ");
                        SQL.AppendLine("   ON P.PATHID = S.PATHID ");
                        SQL.AppendLine("INNER JOIN ADMIN.EMR_PRINTNEEDT D");
                        SQL.AppendLine("   ON  D.CDATE = '" + mstrPrtDate + "'");
                        SQL.AppendLine("  AND D.CUSERID = '" + mstrUseId + "'");
                        SQL.AppendLine("  AND D.PAGENO = C.PAGENO");
                        SQL.AppendLine("  AND D.PRINTCODE = '" + mstrNeedGbn + "'");
                        SQL.AppendLine("  AND D.PRINTED " + (mPrint ? " = 'Y' " : "IS NULL"));
                        #endregion
                    }
                    else
                    {
                        #region 복사신청후 변환한 항목도 보이게
                        SQL.AppendLine(" SELECT  ");
                        SQL.AppendLine("    C2.TREATNO, C2.PAGENO, C2.PAGE, P2.PATHID,  ");
                        SQL.AppendLine("    CASE  ");
                        SQL.AppendLine("        WHEN P2.EXTENSION = '' OR P2.EXTENSION IS NULL THEN 'tif'  ");
                        SQL.AppendLine("        ELSE P2.EXTENSION  ");
                        SQL.AppendLine("    END AS EXTENSION,  ");
                        SQL.AppendLine("    '' AS SECURITY, P2.FILESIZE, P2.CDATE, F2.NAME,  ");
                        SQL.AppendLine("    C2.FORMCODE, '' AS UNREADY, '' AS CDNO, T2.CLASS , ");
                        SQL.AppendLine("    (SELECT C1.NAME  ");
                        SQL.AppendLine("        FROM ADMIN.EMR_CLINICT C1  ");
                        SQL.AppendLine("        WHERE C1.CLINCODE = T2.CLINCODE) AS LOCATIONNM,  ");
                        SQL.AppendLine("    T2.INDATE, P2.LOCATION, ");
                        SQL.AppendLine("    S2.IPADDRESS, S2.FTPUSER, S2.FTPPASSWD, S2.LOCALPATH, ");
                        SQL.AppendLine("    ( S2.LOCALPATH || '/' || P2.LOCATION ) AS SVRFILEPATH  ");
                        SQL.AppendLine("    , F2.ORDERBY ");
                        SQL.AppendLine("FROM ADMIN.EMR_PAGET P2  ");
                        SQL.AppendLine("INNER JOIN ADMIN.EMR_DELETEPAGET C2 ");
                        SQL.AppendLine("   ON P2.PAGENO = C2.PAGENO ");
                        SQL.AppendLine("  AND C2.PAGE > 0 ");
                        SQL.AppendLine("INNER JOIN ADMIN.EMR_FORMT F2 ");
                        SQL.AppendLine("   ON C2.FORMCODE = F2.FORMCODE ");
                        SQL.AppendLine("INNER JOIN ADMIN.EMR_TREATT T2 ");
                        SQL.AppendLine("   ON C2.TREATNO = T2.TREATNO ");
                        SQL.AppendLine("  AND T2.PATID = '" + mstrPtno + "'");
                        SQL.AppendLine("  AND T2.CLINCODE = '" + mstrDeptCode + "'");
                        SQL.AppendLine("INNER JOIN ADMIN.EMR_PATHT S2 ");
                        SQL.AppendLine("   ON P2.PATHID = S2.PATHID ");
                        SQL.AppendLine("INNER JOIN ADMIN.EMR_PRINTNEEDT_BACKUP D2");
                        SQL.AppendLine("   ON D2.CDATE = '" + mstrPrtDate + "'");
                        SQL.AppendLine("  AND D2.CUSERID = '" + mstrUseId + "'");
                        SQL.AppendLine("  AND D2.PAGENO = C2.PAGENO");
                        SQL.AppendLine("  AND D2.PRINTCODE = '" + mstrNeedGbn + "'");
                        SQL.AppendLine("  AND D2.PRINTED " + (mPrint ? " = 'Y' " : "IS NULL"));
                        #endregion
                    }
                }
                else
                {
                    //if (mCvtSearch.Equals("CVT") == false)
                    //{
                    #region 현재 살아있는 스캔
                    SQL.AppendLine(" SELECT  ");
                    SQL.AppendLine("    C.TREATNO, C.PAGENO, C.PAGE, P.PATHID,  ");
                    SQL.AppendLine("    CASE  ");
                    SQL.AppendLine("        WHEN P.EXTENSION = '' OR P.EXTENSION IS NULL THEN 'tif'  ");
                    SQL.AppendLine("        ELSE P.EXTENSION  ");
                    SQL.AppendLine("    END AS EXTENSION,  ");
                    SQL.AppendLine("    C.SECURITY, P.FILESIZE, P.CDATE, F.NAME,  ");
                    SQL.AppendLine("    C.FORMCODE, C.UNREADY, C.CDNO, T.CLASS , ");
                    SQL.AppendLine("    (SELECT C1.NAME  ");
                    SQL.AppendLine("        FROM ADMIN.EMR_CLINICT C1  ");
                    SQL.AppendLine("        WHERE C1.CLINCODE = T.CLINCODE) AS LOCATIONNM,  ");
                    SQL.AppendLine("    T.INDATE, P.LOCATION, ");
                    SQL.AppendLine("    S.IPADDRESS, S.FTPUSER, S.FTPPASSWD, S.LOCALPATH, ");
                    SQL.AppendLine("    ( S.LOCALPATH || '/' || P.LOCATION ) AS SVRFILEPATH  ");
                    SQL.AppendLine("    , F.ORDERBY ");
                    SQL.AppendLine("FROM ADMIN.EMR_PAGET P  ");
                    SQL.AppendLine("INNER JOIN ADMIN.EMR_CHARTPAGET C ");
                    SQL.AppendLine("   ON P.PAGENO = C.PAGENO ");
                    SQL.AppendLine("  AND C.PAGE > 0 ");
                    SQL.AppendLine("INNER JOIN ADMIN.EMR_FORMT F ");
                    SQL.AppendLine("   ON C.FORMCODE = F.FORMCODE ");
                    SQL.AppendLine("INNER JOIN ADMIN.EMR_TREATT T ");
                    SQL.AppendLine("   ON C.TREATNO = T.TREATNO ");
                    SQL.AppendLine("  AND T.PATID = '" + mstrPtno + "'");
                    SQL.AppendLine("  AND T.CLINCODE = '" + mstrDeptCode + "'");
                    SQL.AppendLine("INNER JOIN ADMIN.EMR_PATHT S ");
                    SQL.AppendLine("   ON P.PATHID = S.PATHID ");
                    SQL.AppendLine("INNER JOIN ADMIN.EMR_PRINTNEEDT D");
                    SQL.AppendLine("   ON  D.CDATE = '" + mstrPrtDate + "'");
                    SQL.AppendLine("  AND D.CUSERID = '" + mstrUseId + "'");
                    SQL.AppendLine("  AND D.PAGENO = C.PAGENO");
                    SQL.AppendLine("  AND D.PRINTCODE = '" + mstrNeedGbn + "'");
                    SQL.AppendLine("  AND D.PRINTED " + (mPrint ? " = 'Y' " : "IS NULL"));
                    #endregion
                    //}
                    //else
                    //{
                    #region 복사신청후 변환한 항목도 보이게
                    SQL.AppendLine(" UNION ALL  ");
                    SQL.AppendLine(" SELECT  ");
                    SQL.AppendLine("    C2.TREATNO, C2.PAGENO, C2.PAGE, P2.PATHID,  ");
                    SQL.AppendLine("    CASE  ");
                    SQL.AppendLine("        WHEN P2.EXTENSION = '' OR P2.EXTENSION IS NULL THEN 'tif'  ");
                    SQL.AppendLine("        ELSE P2.EXTENSION  ");
                    SQL.AppendLine("    END AS EXTENSION,  ");
                    SQL.AppendLine("    '' AS SECURITY, P2.FILESIZE, P2.CDATE, F2.NAME,  ");
                    SQL.AppendLine("    C2.FORMCODE, '' AS UNREADY, '' AS CDNO, T2.CLASS , ");
                    SQL.AppendLine("    (SELECT C1.NAME  ");
                    SQL.AppendLine("        FROM ADMIN.EMR_CLINICT C1  ");
                    SQL.AppendLine("        WHERE C1.CLINCODE = T2.CLINCODE) AS LOCATIONNM,  ");
                    SQL.AppendLine("    T2.INDATE, P2.LOCATION, ");
                    SQL.AppendLine("    S2.IPADDRESS, S2.FTPUSER, S2.FTPPASSWD, S2.LOCALPATH, ");
                    SQL.AppendLine("    ( S2.LOCALPATH || '/' || P2.LOCATION ) AS SVRFILEPATH  ");
                    SQL.AppendLine("    , F2.ORDERBY ");
                    SQL.AppendLine("FROM ADMIN.EMR_PAGET P2  ");
                    SQL.AppendLine("INNER JOIN ADMIN.EMR_DELETEPAGET C2 ");
                    SQL.AppendLine("   ON P2.PAGENO = C2.PAGENO ");
                    SQL.AppendLine("  AND C2.PAGE > 0 ");
                    SQL.AppendLine("INNER JOIN ADMIN.EMR_FORMT F2 ");
                    SQL.AppendLine("   ON C2.FORMCODE = F2.FORMCODE ");
                    SQL.AppendLine("INNER JOIN ADMIN.EMR_TREATT T2 ");
                    SQL.AppendLine("   ON C2.TREATNO = T2.TREATNO ");
                    SQL.AppendLine("  AND T2.PATID = '" + mstrPtno + "'");
                    SQL.AppendLine("  AND T2.CLINCODE = '" + mstrDeptCode + "'");
                    SQL.AppendLine("INNER JOIN ADMIN.EMR_PATHT S2 ");
                    SQL.AppendLine("   ON P2.PATHID = S2.PATHID ");
                    SQL.AppendLine("INNER JOIN ADMIN.EMR_PRINTNEEDT_BACKUP D2");
                    SQL.AppendLine("   ON D2.CDATE = '" + mstrPrtDate + "'");
                    SQL.AppendLine("  AND D2.CUSERID = '" + mstrUseId + "'");
                    SQL.AppendLine("  AND D2.PAGENO = C2.PAGENO");
                    SQL.AppendLine("  AND D2.PRINTCODE = '" + mstrNeedGbn + "'");
                    SQL.AppendLine("  AND D2.PRINTED " + (mPrint ? " = 'Y' " : "IS NULL"));
                    #endregion
                    //}
                }

                SQL.AppendLine(" )  ");

                //SQL.AppendLine("ORDER BY T.INDATE, F.ORDERBY, C.FORMCODE ASC , C.PAGE");
                SQL.AppendLine("ORDER BY INDATE, ORDERBY, FORMCODE ASC , PAGE");

                string SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
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
                    toolStripcboPage.Items.Add(i + 1);

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

        private void DspDownChart()
        {
            int i;


            if (Directory.Exists(mstrViewPath) == false)
            {
                Directory.CreateDirectory(mstrViewPath);
            }

            //if (clsScan.DeleteFoldAll(mstrViewPath) == false)
            //{
            //    ComFunc.MsgBoxEx(this, "작업 폴드의 파일 삭제중 에러가 발생했습니다.");
            //    return;
            //}

            if (spScan_Sheet1.RowCount == 0) return;

            toolStripcboPage.SelectedIndex = 0;
            imageListView1.ClearItem();

            Cursor.Current = Cursors.WaitCursor;

            Ftpedt FtpedtX = new Ftpedt();
            bool ftpCon = false;

            try
            {
                string strServerAddress = string.Empty;
                string strUserName = string.Empty;
                string strPassword = string.Empty;

                string strFileNm = string.Empty;
                string strSvrPath = string.Empty;
                string strFORMNAME = string.Empty;

                strServerAddress = spScan_Sheet1.Cells[0, Convert.ToInt32(clsScanPublic.ScanSp.sSVRIP)].Text.Trim();
                strUserName = spScan_Sheet1.Cells[0, Convert.ToInt32(clsScanPublic.ScanSp.sSVRID)].Text.Trim();
                strPassword = spScan_Sheet1.Cells[0, Convert.ToInt32(clsScanPublic.ScanSp.sSVRPW)].Text.Trim();

                FtpedtX.FtpConBatchEx = FtpedtX.FtpConnetBatchEx(strServerAddress, strUserName, strPassword);
                if (FtpedtX.FtpConBatchEx == null)
                {
                    FtpedtX.Dispose();
                    return;
                }

                imageListView1.ClearItem();
                imgContinuView.Visible = false;

                //string strSvrIp = "";
                //ltkThumb.Redraw = false;
                for (i = 0; i < spScan_Sheet1.RowCount; i++)
                {
                    strServerAddress = spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSVRIP)].Text.Trim();
                    strUserName = spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSVRID)].Text.Trim();
                    strPassword = spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSVRPW)].Text.Trim();
                    strFileNm = spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sIMGNAME)].Text.Trim();
                    strFORMNAME = spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sFORMNAME)].Text.Trim();
                    strSvrPath = spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSEVERPATH)].Text.Trim();
                    strSvrPath = strSvrPath.Replace("\\", "/");

                    bool blnDown = FtpedtX.FtpDownloadBatchEx(FtpedtX.FtpConBatchEx, mstrViewPath + "\\" + strFileNm, strFileNm, strSvrPath); //파일다운로드

                    if (blnDown)
                    {
                        FileInfo file1 = new FileInfo(mstrViewPath + "\\" + strFileNm);
                        if (file1.Exists == true)
                        {
                            //ImageListViewItem viewItem = new ImageListViewItem(file1.FullName);
                            //viewItem.Text = ;
                            //items.Add(viewItem);

                            imageListView1.AddItem(file1.FullName, "[" + (i + 1) + "] " + strFORMNAME);

                            //imageListView1.Items.Add(viewItem);

                            if (chkContinuView.Checked) 
                            {
                                clsApi.SuspendDrawing(imgContinuView);
                                imgContinuViewAdd(mstrViewPath + "\\" + strFileNm, strFORMNAME);
                                clsApi.ResumeDrawing(imgContinuView);
                            }
                        }
                    }
                }

                FtpedtX.FtpDisConnetBatchEx(FtpedtX.FtpConBatchEx);

                if (chkContinuView.Checked)
                {
                    imgContinuView.Visible = true;
                }

                //imageListView1.SetItemSize();
                imageListView1.ImageItemVisible();

                //if (chkContinuView.Checked == false && items.Count > 0)
                //{
                //    //imageListView1.TotalCount = 0;
                //    //imageListView1.TotalCount = items.Count;
                //    //imageListView1.Items.AddRange(items.ToArray());
                //    //imageListView1.mRenderer.ResumePaint();

                //}

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (ftpCon == true)
                {
                    FtpedtX.FtpDisConnetBatchEx(FtpedtX.FtpConBatchEx);
                }
                
                ComFunc.MsgBoxEx(this, "스캔을 읽어오는 도중 오류가 발생했습니다");
                Cursor.Current = Cursors.Default;
            }

            FtpedtX.Dispose();
            FtpedtX = null;
        }

        #region //출력관련
        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintF();
        }

        void PrintF()
        {
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
            if (spScan_Sheet1.RowCount <= 0)
            {
                ComFunc.MsgBoxEx(this, "출력할 이미지가 없습니다.");
                return false;
            }

            if (imageListView1.SelectedItems.Count <= 0)
            {
                ComFunc.MsgBoxEx(this, "출력할 이미지를 선택해 주십시요.");
                return false;
            }

            //차트복사가 아닐때만 창 뛰움 
            //차트복사는 기본적으로 0(외부 출력)세팅 되어있음.
            if (mCallForm != null && mCallForm.Name.Equals("frmEmrJobChartCopy") == false)
            {
                using (frmEmrPrintOption frmEmrPrintOptionX = new frmEmrPrintOption())
                {
                    frmEmrPrintOptionX.StartPosition = FormStartPosition.CenterScreen;
                    frmEmrPrintOptionX.ShowDialog(this);
                }

                if (clsFormPrint.mstrPRINTFLAG == "-1")
                {
                    return false;
                }
            }


            clsDB.setBeginTran(clsDB.DbCon);
            string SQL = string.Empty;
            int RowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                string strPrintDate = VB.Left(strCurDateTime, 8);
                string strPrintTime = VB.Right(strCurDateTime, 6);
                string strSCANNO = "0";
                string strPrtCode = clsFormPrint.mstrPRINTFLAG.Equals("2") ? "003" : "002";

                for (int i = 0; i < imageListView1.Items.Count; i++)
                {
                    if (imageListView1.Items[i].IsSelected == true)
                    {
                        strSCANNO = spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSCANNO)].Text;

                        SQL = "INSERT INTO ADMIN.EMR_PAGEPRINTLOGT(";
                        SQL += ComNum.VBLF + "PAGENO, PRINTCODE, CUSERID, NEEDUSER, CDATE, CTIME";
                        SQL += ComNum.VBLF + ")";
                        SQL += ComNum.VBLF + "VALUES(";
                        SQL += ComNum.VBLF + "'" + strSCANNO + "',";  //페이지번호
                        SQL += ComNum.VBLF + "'" + (mstrNeedGbn.Length > 0 ? mstrNeedGbn : strPrtCode) + "',";   //프린트코드
                        SQL += ComNum.VBLF + "'" + clsType.User.Sabun + "',"; //사용자
                        SQL += ComNum.VBLF + "'" + clsType.User.Sabun + "',"; //출력자 사번
                        SQL += ComNum.VBLF + "TO_CHAR(SYSDATE, 'YYYYMMDD'), ";//서버날짜
                        SQL += ComNum.VBLF + "TO_CHAR(SYSDATE, 'HH24MISS')  ";//서버시간
                        SQL += ComNum.VBLF + ")";

                        string sqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                        if (sqlErr.Length > 0)
                        {
                            //if (mCallForm != null && mCallForm.Name.Equals("frmEmrJobChartCopy") == false)
                            //{
                            clsDB.setRollbackTran(clsDB.DbCon);
                            //}
                            clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, sqlErr);
                            return false;
                        }

                        if (mstrPrtDate.Length > 0 && mstrUseId.Length > 0 && mstrNeedGbn.Length > 0)
                        {
                            SQL = "UPDATE ADMIN.EMR_PRINTNEEDT";
                            SQL += ComNum.VBLF + " SET PRINTED = 'Y'";
                            SQL += ComNum.VBLF + " WHERE PAGENO    = " + strSCANNO;
                            SQL += ComNum.VBLF + "   AND CDATE     = '" + mstrPrtDate + "' ";
                            SQL += ComNum.VBLF + "   AND CUSERID   = '" + mstrUseId + "' ";
                            SQL += ComNum.VBLF + "   AND PRINTCODE = '" + mstrNeedGbn + "' ";

                            sqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                            if (sqlErr.Length > 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                                ComFunc.MsgBoxEx(this, sqlErr);
                                return false;
                            }

                            SQL = "UPDATE ADMIN.EMR_PRINTNEEDT_BACKUP";
                            SQL += ComNum.VBLF + " SET PRINTED = 'Y'";
                            SQL += ComNum.VBLF + " WHERE PAGENO    = " + strSCANNO;
                            SQL += ComNum.VBLF + "   AND CDATE     = '" + mstrPrtDate + "' ";
                            SQL += ComNum.VBLF + "   AND CUSERID   = '" + mstrUseId + "' ";
                            SQL += ComNum.VBLF + "   AND PRINTCODE = '" + mstrNeedGbn + "' ";

                            sqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                            if (sqlErr.Length > 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                                ComFunc.MsgBoxEx(this, sqlErr);
                                return false;
                            }
                        }

                        //if (clsEmrQuery.SaveEmrPrintHis(clsDB.DbCon, mstrEmrNo, clsFormPrint.mstrPRINTFLAG, "00",
                        //                                    strPrintDate, strPrintTime, clsType.User.IdNumber, "0", strSCANNO) == false)
                        //{
                        //    clsDB.setRollbackTran(clsDB.DbCon);
                        //    Cursor.Current = Cursors.Default;
                        //    return false;
                        //}
                    }
                }

                //if (mCallForm != null && mCallForm.Name.Equals("frmEmrJobChartCopy") == false)
                //{
                clsDB.setCommitTran(clsDB.DbCon);
                //}
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                //if (mCallForm != null && mCallForm.Name.Equals("frmEmrJobChartCopy") == false)
                //{
                clsDB.setRollbackTran(clsDB.DbCon);
                //}
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// 출력 함수
        /// </summary>
        private void ScanPrint()
        {
            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                string strPrintDate = VB.Left(strCurDateTime, 8);
                string strPrintTime = VB.Right(strCurDateTime, 6);

                mintPrint = 0;

                using (PrintDocument ScanPrint = new PrintDocument())
                {
                    PrintController printController = new StandardPrintController();
                    ScanPrint.PrintController = printController;  //기본인쇄창 없애기
                    PageSettings ps = new PageSettings();
                    ps.Margins = new Margins(10, 10, 10, 10);
                    ScanPrint.DefaultPageSettings = ps;
                    if (SelectPrtName.NotEmpty())
                    {
                        ScanPrint.PrinterSettings.PrinterName = SelectPrtName;
                    }
                    ScanPrint.PrintPage += pBox_PrintPageEx;

                    int i = 0;
                    for (i = 0; i < imageListView1.Items.Count; i++)
                    {
                        if (imageListView1.Items[i].IsSelected == true)
                        {
                            ps.Landscape = imageListView1.Items[i].pictureBox1.Image.Width > imageListView1.Items[i].pictureBox1.Image.Height;

                            ScanPrint.DocumentName = imageListView1.Items[i].FileName;
                            ScanPrint.Print();
                        }
                    }

                    ScanPrint.PrintPage -= pBox_PrintPageEx;
                }

                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }


        private void pBox_PrintPageEx(object sender, PrintPageEventArgs e)
        {
            using (Image pImage = GetImage(((PrintDocument)sender).DocumentName))
            {
                if (pImage.Size.Width > pImage.Size.Height)
                {
                    //pImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    e.Graphics.DrawImage(pImage, 10, 10, 1100, 760); //'가로 이미지 크기
                }
                else
                {
                    e.Graphics.DrawImage(pImage, 10, 10, 760, 1100); //'세로 이미지 크기
                }
            }

            mintPrint += 1;
        }

        /// <summary>
        /// Image 불러오기
        /// </summary>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        private Image GetImage(string strFileName)
        {
            Image rtnVal = null;

            if (strFileName.IndexOf(".env") != -1)
            {
               rtnVal = clsCyper.DecryptImage(strFileName);
            }
            else
            {
                byte[] buff = File.ReadAllBytes(strFileName);
                using (MemoryStream ms = new MemoryStream(buff))
                {
                    rtnVal = Image.FromStream(ms);
                }
            }

            return rtnVal;
        }

        #endregion //출력관련

        #region 복사신청
        private void toolButtonChartCopy_Click(object sender, EventArgs e)
        {
            RecordCopy();
        }


        /// <summary>
        /// 차트복사
        /// </summary>
        void RecordCopy()
        {
            if (spScan_Sheet1.RowCount <= 0)
            {
                ComFunc.MsgBoxEx(this, "복사신청할 이미지가 없습니다.");
                return;
            }

            if (imageListView1.SelectedItems.Count <= 0)
            {
                ComFunc.MsgBoxEx(this, "복사신청할 이미지를 선택해주세요.");
                return;
            }

            List<int> lstRow = new List<int>();
            string strSysDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            string strPtno = spScan_Sheet1.Cells[0, Convert.ToInt32(clsScanPublic.ScanSp.sPTNO)].Text;

            for (short i = 0; i < imageListView1.Items.Count; i++)
            {
                string strPageNo = spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSCANNO)].Text;
                if (imageListView1.Items[i].IsSelected == true)
                {
                    if (DuplicateScan(strSysDate, clsType.User.IdNumber, strPtno, strPageNo))
                    {
                        ComFunc.MsgBoxEx(this, string.Format("{0}번째 항목은 이미 신청한 항목입니다. {1}다시 확인해주세요.", (i + 1), ComNum.VBLF));
                        return;
                    }
                    lstRow.Add(i);
                }
            }

            int intTotCopyCnt = 0;
            if (CopyInsert(lstRow, ref intTotCopyCnt))
            {
                if (intTotCopyCnt == 0)
                    return;

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
            StringBuilder SQL = new StringBuilder();
            OracleDataReader reader = null;

            try
            {

                SQL.AppendLine("SELECT 1 AS CNT");
                SQL.AppendLine("FROM DUAL");
                SQL.AppendLine("WHERE EXISTS");
                SQL.AppendLine("(");
                SQL.AppendLine("SELECT 1");
                SQL.AppendLine(" FROM ADMIN.EMR_PRINTNEEDT N, ADMIN.EMR_CHARTPAGET C,");
                SQL.AppendLine("      ADMIN.EMR_TREATT T");
                SQL.AppendLine(" WHERE N.CDATE = '" + strDate + "' ");
                SQL.AppendLine("   AND N.PAGENO  = '" + strPageNo + "' ");
                SQL.AppendLine("   AND N.CUSERID = '" + strUseId + "' ");
                SQL.AppendLine("   AND T.PATID = '" + strPtno + "' ");
                SQL.AppendLine("   AND C.PAGENO = N.PAGENO ");
                SQL.AppendLine("   AND T.TREATNO = C.TREATNO ");
                SQL.AppendLine(")");

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL.ToString().Trim(), clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    clsDB.SaveSqlErrLog(sqlErr, SQL.ToString().Trim(), clsDB.DbCon);
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    rtnVal = true;
                }

                reader.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL.ToString().Trim(), clsDB.DbCon);
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
            StringBuilder SQL = new StringBuilder();
            int RowAffected = 0;

            //string strPrtCnt = VB.InputBox("복사신청할 갯수를 입력해주세요.", "복사신청", "1");
            //if (string.IsNullOrEmpty(strPrtCnt))
            //{
            //    return rtnVal;
            //}

            string strPrtCnt = "1";

            double dPrtCnt = VB.Val(strPrtCnt);

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (int i = 0; i < lstRow.Count; i++)
                {
                    string strPageNo = spScan_Sheet1.Cells[lstRow[i], Convert.ToInt32(clsScanPublic.ScanSp.sSCANNO)].Text;
                    string strTreatNo = spScan_Sheet1.Cells[lstRow[i], Convert.ToInt32(clsScanPublic.ScanSp.sACPNO)].Text;

                    SQL.Clear();
                    SQL.AppendLine("INSERT INTO ADMIN.EMR_PRINTNEEDT(");
                    SQL.AppendLine("TREATNO, PAGENO, CUSERID, PRINTCODE,");
                    SQL.AppendLine("CDATE, NEEDGUBUN, NEEDCNT");
                    SQL.AppendLine(")");
                    SQL.AppendLine("VALUES(");
                    SQL.AppendLine("'" + strTreatNo + "',");  //TreatNo
                    SQL.AppendLine("'" + strPageNo + "',");  //PAGENO
                    SQL.AppendLine("'" + clsType.User.IdNumber + "',"); //로그인 사번
                    SQL.AppendLine("'002',"); //전송용
                    SQL.AppendLine("TO_CHAR(SYSDATE, 'YYYYMMDD'), ");//서버날짜
                    SQL.AppendLine("'1', ");//NEEDGUBUN
                    SQL.AppendLine(dPrtCnt.ToString());//요청 갯수
                    SQL.AppendLine(")");

                    string sqlErr = clsDB.ExecuteNonQuery(SQL.ToString().Trim(), ref RowAffected, clsDB.DbCon);
                    if (sqlErr.Length > 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(sqlErr, SQL.ToString().Trim(), clsDB.DbCon);
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
                clsDB.SaveSqlErrLog(ex.Message, SQL.ToString().Trim(), clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            return rtnVal;
        }

        #endregion

        #region 이미지 컨트롤
        private void FScanPageView_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (fScanPageView != null)
            {
                fScanPageView.Dispose();
                fScanPageView = null;
            }
        }


        /// <summary>
        /// 스캔 이미지 복호화 후 보여주는 함수
        /// </summary>
        /// <param name="index"></param>
        private void LoadScanImage(int index)
        {
            string strImagePath = imageListView1.Items[index - 1].FileName;
            //string strCytFile = strImagePath.Replace(".tif", ".env");

            //if (strImagePath.Length == 0 || File.Exists(strCytFile) == false)
            //    return;

            toolStripcboPage.Text = (index).ToString();

            imageListView1.Visible = false;
            kpImageViewer1.Visible = true;
            kpImageViewer1.ImagePath = strImagePath;
            //kpImageViewer1.SetSelectZoom();
            //kpImageViewer1.SetZoom();

            //kpImageViewer1.DisplayMode = displayMode;// KaiwaProjects.KpDisplayMode.Horizontal;
            //kpImageViewer1.ZoomMode = zoomMode;
            //kpImageViewer1.Zoom = LastZoom;
        }


        /// <summary>
        /// 처음
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolButtonGo_Click(object sender, EventArgs e)
        {
            int SelectIndex = 1;
            //처음
            if (sender.Equals(toolButtonGoFirst))
            {
                if (imageListView1.Items.Count == 0 || toolStripcboPage.Text.Equals("1"))
                    return;

            }
            /// 이전
            else if (sender.Equals(toolButtonGoBack))
            {
                if (imageListView1.Items.Count == 0 || toolStripcboPage.Text.Equals("1"))
                    return;

                SelectIndex = int.Parse(toolStripcboPage.Text) - 1;
            }
            /// 다음
            else if (sender.Equals(toolButtonGoForward))
            {
                if (imageListView1.Items.Count == 0 || VB.Val(toolStripcboPage.Text) == imageListView1.Items.Count)

                    return;

                SelectIndex = int.Parse(toolStripcboPage.Text) + 1;
            }
            /// 마지막
            else if (sender.Equals(toolButtonGoLast))
            {
                if (imageListView1.Items.Count == 0)
                    return;

                SelectIndex = imageListView1.Items.Count;
            }

            LoadScanImage(SelectIndex);
        }

        private void toolButtonPrint_Click(object sender, EventArgs e)
        {
            if (SaveEmrScanPrnYn() == false)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            ScanPrint();
            Cursor.Current = Cursors.Default;
        }

        private void toolButtonRemove_Click(object sender, EventArgs e)
        {
            if (imageListView1.SelectedItems.Count == 0)
            {
                ComFunc.MsgBoxEx(this, "삭제할 항목을 선택해주세요.");
                return;
            }

            if (ComFunc.MsgBoxQEx(this, "선택하신 항목을 차트복사 신청내역에서 삭제하시겠습니까?") == DialogResult.No)
                return;


            StringBuilder SQL = new StringBuilder();
            clsDB.setBeginTran(clsDB.DbCon);

            int TotRow = 0;
            try
            {
                for (short i = 0; i < imageListView1.Items.Count; i++)
                {
                    int RowAffected = 0;
                    if (imageListView1.Items[i].IsSelected)
                    {
                        SQL.Clear();
                        SQL.AppendLine("DELETE ADMIN.EMR_PRINTNEEDT N");
                        SQL.AppendLine(" WHERE N.CDATE = '" + mstrPrtDate + "'");
                        SQL.AppendLine("   AND N.CUSERID = '" + mstrUseId + "'");
                        SQL.AppendLine("   AND N.PAGENO  = " + spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSCANNO)].Text.Trim());
                        //SQL.AppendLine("   AND PRINTED IS NULL");

                        string sqlErr = clsDB.ExecuteNonQueryEx(SQL.ToString().Trim(), ref RowAffected, clsDB.DbCon);
                        if (sqlErr.Length > 0)
                        {
                            clsDB.SaveSqlErrLog(sqlErr, SQL.ToString().Trim(), clsDB.DbCon);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }

                        TotRow += RowAffected;

                        SQL.Clear();
                        SQL.AppendLine("DELETE ADMIN.EMR_PRINTNEEDT_BACKUP N");
                        SQL.AppendLine(" WHERE N.CDATE = '" + mstrPrtDate + "'");
                        SQL.AppendLine("   AND N.CUSERID = '" + mstrUseId + "'");
                        SQL.AppendLine("   AND N.PAGENO  = " + spScan_Sheet1.Cells[i, Convert.ToInt32(clsScanPublic.ScanSp.sSCANNO)].Text.Trim());
                        //SQL.AppendLine("   AND PRINTED IS NULL");
                        sqlErr = clsDB.ExecuteNonQueryEx(SQL.ToString().Trim(), ref RowAffected, clsDB.DbCon);
                        if (sqlErr.Length > 0)
                        {
                            clsDB.SaveSqlErrLog(sqlErr, SQL.ToString().Trim(), clsDB.DbCon);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }

                        TotRow += RowAffected;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, string.Format("총 {0}개의 신청내역을 삭제하였습니다.", TotRow));
                LoadScanCopyList();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
            }

            imageListView1.Refresh();
        }

        #endregion

        private void chkChartView_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            RecordCopy();
        }

        private void btnPrintSet_Click(object sender, EventArgs e)
        {
            using(PrintDialog dialog = new PrintDialog())
            {
                dialog.AllowSomePages = true;
                dialog.ShowHelp = true;
                dialog.ShowDialog(this);

                SelectPrtName = dialog.PrinterSettings.PrinterName;
            }
        }

        private void frmScanImageViewNew2_Resize(object sender, EventArgs e)
        {
            imageListView1.Width = Width;
            imageListView1.Height = Height - 65;
        }

        private void chkContinuView_CheckedChanged(object sender, EventArgs e)
        {
            clsEmrPublic.bScanContinuView = chkContinuView.Checked;
        }

        private void toolButtonRefreshThumb_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (sender as ToolStripMenuItem);
            string[] arrStr = menuItem.Text.Split('x');

            if (arrStr.Length == 2 && VB.Val(arrStr[0]) > 0 && VB.Val(arrStr[1]) > 0)
            {
                short nHThumbCnt = short.Parse(arrStr[0]);
                short nVThumbCnt = short.Parse(arrStr[1]);

                RefreshThumbCnt(nHThumbCnt, nVThumbCnt);
            }
        }

        /// <summary>
        /// 한 화면에 표시되는 Thumbnail의 갯수를 조절한다.
        /// </summary>
        private void RefreshThumbCnt(short nHThumbCnt, short nVThumbCnt)
        {
            if (nHThumbCnt == 0)
            {
                imageListView1.ImageListItemSize = MTSImageListView.ImageListItemSize.SIZE_4X4;
                //imageListView1.ThumbnailSize = new Size(95, 120);
            }
            else
            {
                string size = string.Format("SIZE_{0}X{1}", nHThumbCnt, nVThumbCnt);

                try
                {
                    imageListView1.ImageListItemSize = size.ToEnum<MTSImageListView.ImageListItemSize>();
                }
                catch { }
                //CalThumbSize(imageListView1, nHThumbCnt, nVThumbCnt);
            }
        }

       
        #region 연속보기 관련 컨트롤 이벤트

        private void imgContinuView_ImageX_Click(object sender, EventArgs e)
        {
            //string strImagePath = imgContinuView.pImageList[imgContinuView.SelectedIndex].ClientPath;

            //if (ltkPageView.Load(strImagePath, 1))
            //{
            //    MoveImgView();
            //    ltkPageView.Visible = true;
            //    ltkPageView.BringToFront();
            //    //File.Delete(strImagePath);
            //}
        }

        private void imgContinuView_DoubleClick(object sender, EventArgs e)
        {

        }

        private void imgContinuView_MouseDown(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Right)
            //{
            //    string strImagePath = imgContinuView.pImageList[imgContinuView.SelectedIndex].ClientPath;

            //    SetImaeViewEx(strImagePath, picBig1);
            //}
        }

        private void imgContinuView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && (picBig1.Visible || fScanPageView != null))
            {
                if (fScanPageView != null)
                {
                    fScanPageView.Dispose();
                    fScanPageView = null;
                }
                picBig1.Visible = false;
            }
        }

        private void SetImaeViewEx(string strImagePath, PictureBox picBig)
        {
            Image BackImage = GetImage(strImagePath);
            //using (var memoryStream = new MemoryStream(File.ReadAllBytes(strImagePath)))
            //{
            //    BackImage = new Bitmap(memoryStream);
            //}

            double horzRatio = (double)((double)(mCallForm.Name.Equals("frmEmrJobChartCopy") ? mCallForm.Width : Width) / (double)BackImage.Width);
            double vertRatio = (double)((double)(mCallForm.Name.Equals("frmEmrJobChartCopy") ? mCallForm.Height : Height) / (double)BackImage.Height);
            double ratio = (double)(horzRatio < vertRatio ? horzRatio : vertRatio);

            picBig.Width = (int)(BackImage.Width * ratio);
            picBig.Height = (int)(BackImage.Height * ratio) - 60;

            picBig.Left = (mCallForm.Name.Equals("frmEmrJobChartCopy") ? mCallForm.Width / 2 : Width / 2) - picBig.Width / 2;
            picBig.Top = (mCallForm.Name.Equals("frmEmrJobChartCopy") ? mCallForm.Height / 2 : Height / 2) - picBig.Height / 2;
    
            picBig.Image = BackImage;
            picBig.SizeMode = PictureBoxSizeMode.StretchImage;
            picBig.Visible = true;
            picBig.BringToFront();
        }

        #endregion

        private void toolButtonSelectAll_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < imageListView1.Items.Count; i++)
            {
                if (imageListView1.Items[i].IsSelected == false)
                {
                    imageListView1.Items[i].IsSelected = true;
                }
            }

            imageListView1.Focus();
        }

        private void toolButtonUnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < imageListView1.Items.Count; i++)
            {
                if (imageListView1.Items[i].IsSelected == true)
                {
                    imageListView1.Items[i].IsSelected = false;
                }
            }

            imageListView1.Focus();
        }

        private void imageListView1_ImageItemDoubleClickEvent(ImageItem item, EventArgs e)
        {
            imageListView1.Visible = false;
            kpImageViewer1.Visible = true;
            toolStripcboPage.Text = (item.Index + 1).ToString();
            kpImageViewer1.ImagePath = item.FileName;
        }

        private void imageListView1_MouseUp(ImageItem item, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && (picBig1.Visible || fScanPageView != null))
            {
                if (fScanPageView != null)
                {
                    fScanPageView.Dispose();
                    fScanPageView = null;
                }
                picBig1.Visible = false;
            }
        }

        private void imageListView1_MouseDown(ImageItem item, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {
                if (fScanPageView != null)
                {
                    fScanPageView.Dispose();
                    fScanPageView = null;
                }


                if (mCallForm.Name.Equals("frmEmrJobChartCopy"))
                {
                    fScanPageView = new frmScanPageView(mCallForm, item.FileName);
                    fScanPageView.FormClosed += FScanPageView_FormClosed;
                    fScanPageView.StartPosition = FormStartPosition.CenterParent;
                    fScanPageView.Show(this);
                }
                else
                {

                    SetImaeViewEx(item.FileName, picBig1);
                }
            }
        }

        private void kpImageViewer1_ImageViewerDoubleClick()
        {
            kpImageViewer1.Visible = false;
            imageListView1.Visible = true;
        }

        private void picBig1_MouseUp(object sender, MouseEventArgs e)
        {
            picBig1.Visible = false;
        }


    }
}
