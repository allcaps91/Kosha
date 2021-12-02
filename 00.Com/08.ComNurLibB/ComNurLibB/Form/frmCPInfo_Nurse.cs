using ComBase;
using ComBase.Controls;
using ComLibB;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

namespace ComNurLibB
{
    public partial class frmCPInfo_Nurse : Form, MainFormMessage
    {
        #region //MainFormMessage
        public MainFormMessage mCallForm = null;
        public void MsgActivedForm(Form frm)
        {

        }
        public void MsgUnloadForm(Form frm)
        {

        }
        public void MsgFormClear()
        {

        }
        public void MsgSendPara(string strPara)
        {

        }
        #endregion

        #region 동의서 출력
        frmAgreePrint frmAgreePrintX = null;
        #endregion

        #region 환자정보

        /// <summary>
        /// INDATE
        /// </summary>
        string fstrINDATE = string.Empty;

        /// <summary>
        /// IPDNO
        /// </summary>
        string fstrIPDNO = string.Empty;

        /// <summary>
        /// 이름
        /// </summary>
        string fstrName = string.Empty;
        /// <summary>
        /// 등록번호
        /// </summary>
        string fstrPano = string.Empty;
        /// <summary>
        /// 나이
        /// </summary>
        string fstrAge = string.Empty;
        /// <summary>
        /// 호실
        /// </summary>
        string fstrRoom = string.Empty;

        /// <summary>
        /// 환자 과
        /// </summary>
        string fstrDeptCode = string.Empty;

        /// <summary>
        /// 주치의 의사코드
        /// </summary>
        string fstrDrCode = string.Empty;

        /// <summary>
        /// CPNO
        /// </summary>
        string fstrCpNo = string.Empty;
        #endregion

        frmCPPrintForm frmCPPrintFormX = null;


        public frmCPInfo_Nurse()
        {
            InitializeComponent();
            setEvent();
        }

        public frmCPInfo_Nurse(string strIPDNO)
        {
            InitializeComponent();
            fstrIPDNO = strIPDNO;
            setEvent();
        }

        public frmCPInfo_Nurse(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
            setEvent();
        }

        void eFormActivated(object sender, EventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgActivedForm(this);
            }
        }

        void eFormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmAgreePrintX != null)
            {
                frmAgreePrintX.Dispose();
                frmAgreePrintX = null;
            }

            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            //단독으로 사용 되는 폼이 아니라서 별도 권한이 없습니다. 
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            SCREEN_CLEAR();

            SET_HEADER_I(fstrIPDNO);
            SET_CP(fstrIPDNO);

            CP_LAYOUT();
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnExit.Click += new EventHandler(eBtnClick);

            this.ssSet.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(ssSet_CellClick);
            this.ssConsent.ButtonClicked += new FarPoint.Win.Spread.EditorNotifyEventHandler(ssConsent_ButtonClicked);
        }

        void SCREEN_CLEAR()
        {
            //^ㅁ^v~
        }


        private void SET_HEADER_I(string strIPDNO)
        {
            string str1 = "";
            string str2 = "";
            string strINDATE = "";
            string strPANO = "";
            string strILLCODE = "";
            string strILLNAME = "";
            string strWARD = "";
            string strROOM = "";
            string strSNAME = "";
            string strSEX = "";
            string strAGE = "";
            string strJUMIN = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;


            SQL = " SELECT IPDNO, PANO, SNAME, SEX, AGE, WARDCODE, ROOMCODE, TO_CHAR(INDATE,'YYYY-MM-DD') INDATE, DRCODE,";
            SQL += ComNum.VBLF + " (SELECT JUMIN1 || '-' || JUMIN2 FROM KOSMOS_PMPA.BAS_PATIENT WHERE PANO = A.PANO AND ROWNUM = 1) JUMIN ";
            SQL += ComNum.VBLF + " , KOSMOS_PMPA.READ_CP_PROGRESS_NUR(A.IPDNO) AS CP_PROGRESS_NUR";
            SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.IPD_NEW_MASTER A ";
            SQL += ComNum.VBLF + " WHERE IPDNO = '" + strIPDNO + "' ";
           
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
                ComFunc.MsgBoxEx(this, "환자정보가 없습니다.");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                strPANO = dt.Rows[0]["PANO"].ToString().Trim();
                strWARD = dt.Rows[0]["WARDCODE"].ToString().Trim();
                strROOM = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                strSNAME = dt.Rows[0]["SNAME"].ToString().Trim();
                strSEX = dt.Rows[0]["SEX"].ToString().Trim();
                strAGE = dt.Rows[0]["AGE"].ToString().Trim();
                strJUMIN = dt.Rows[0]["JUMIN"].ToString().Trim();
                strINDATE = dt.Rows[0]["INDATE"].ToString().Trim();

                lblCpDays.Text = dt.Rows[0]["CP_PROGRESS_NUR"].ToString().Trim();

                fstrINDATE = strINDATE;
                fstrRoom = strROOM;
                fstrName = strSNAME;
                fstrAge = strAGE;
                fstrPano = strPANO;
                fstrDrCode = dt.Rows[0]["DRCODE"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;

            SQL = " SELECT ILLCODE, ";
            SQL += ComNum.VBLF + " (SELECT ILLNAMEK FROM KOSMOS_PMPA.BAS_ILLS WHERE ILLCLASS = '1' AND ILLCODE = A.ILLCODE AND ROWNUM = 1) ILLNAMEK ";
            SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_IILLS A ";
            SQL += ComNum.VBLF + " WHERE IPDNO = " + strIPDNO;
            SQL += ComNum.VBLF + "   AND SEQNO = 1 ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count > 0)
            {
                strILLCODE = dt.Rows[0]["ILLCODE"].ToString().Trim();
                strILLNAME = dt.Rows[0]["ILLNAMEK"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;

            str1 = "주 상 병 : " + strILLCODE + "        " + strILLNAME + "                 병동/호실 :  " + strWARD + " / " + strROOM;
            str2 = "환자정보 : " + strPANO + "     " + strSNAME + "     " + strSEX + "   " + strAGE + "   " + strJUMIN;

            ss1_Sheet1.Cells[0, 1].Text = str1;
            ss1_Sheet1.Cells[1, 1].Text = str2;

        }

        string READ_FTP(string strIP, string strUser)
        {
            string strVal = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = "SELECT USERPASS FROM KOSMOS_PMPA.BAS_ACCOUNT_SERVER      ";
                SQL = SQL + ComNum.VBLF + "WHERE IP = '" + strIP + "'      ";
                SQL = SQL + ComNum.VBLF + "    AND USERID = '" + strUser + "'        ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SDATE DESC      ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = clsAES.DeAES(dt.Rows[0]["USERPASS"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            return strVal;
        }


        private void GET_IMAGE(string argFileName, string argFileName2, string argGBN)
        {
            try
            {
                PictureBox pic = argGBN == "OCSFILE" ? picOcs1 : picPat1;
                PictureBox pic2 = argGBN == "OCSFILE" ? picOcs2 : picPat2;

                pic2.Visible = false;

                string strServerFileName = argFileName;
                string strFileName = @"C:\PSMHEXE\exenet\" + strServerFileName;

                if (strServerFileName.Length == 0)
                {
                    strServerFileName = null;
                    strFileName = null;
                    return;
                }

                if (File.Exists(strFileName))
                {
                    File.Delete(strFileName);
                }

                using (Ftpedt ftp = new Ftpedt())
                {
                    #region 1번 이미지
                    if (ftp.FtpDownload("192.168.100.31", "oracle", READ_FTP("192.168.100.31", "oracle"), strFileName, strServerFileName, "/data/EDMS_DATA/QI") == false)
                    {
                        ComFunc.MsgBoxEx(this, "다운로드 실패");
                    }
                    else
                    {
                        if (pic.Image != null)
                        {
                            pic.Image.Dispose();
                            pic.Image = null;
                        }

                        Image image = null;
                        using (MemoryStream mem = new MemoryStream(File.ReadAllBytes(strFileName)))
                        {
                            image = Image.FromStream(mem);
                        }
                        pic.Image = image;
                        pic.Width = image.Width;
                        pic.Height = image.Height;

                        image.Tag = "";
                        image.Tag = strFileName;
                    }
                    #endregion

                    #region 2번 이미지
                    if (argFileName2.NotEmpty())
                    {
                        strFileName = @"C:\PSMHEXE\exenet\" + argFileName2;
                        if (ftp.FtpDownload("192.168.100.31", "oracle", READ_FTP("192.168.100.31", "oracle"), strFileName, argFileName2, "/data/EDMS_DATA/QI") == false)
                        {
                            ComFunc.MsgBoxEx(this, "다운로드 실패");
                        }
                        else
                        {
                            if (pic2.Image != null)
                            {
                                pic2.Image.Dispose();
                                pic2.Image = null;
                            }

                            Image image = null;
                            using (MemoryStream mem = new MemoryStream(File.ReadAllBytes(strFileName)))
                            {
                                image = Image.FromStream(mem);
                            }
                            pic2.Image = image;
                            pic2.Width = image.Width;
                            pic2.Height = image.Height;
                            pic2.Visible = true;

                            image.Tag = "";
                            image.Tag = strFileName;
                        }
                    }
                    #endregion
                }
            }
            catch
            {
                return;
            }
        }

        private void pBox_PrintPageEx1(object sender, PrintPageEventArgs e, string FILEPATH)
        {
            Image pImage = null;
            using (MemoryStream mem = new MemoryStream(File.ReadAllBytes(FILEPATH)))
            {
                pImage = Image.FromStream(mem);
            }

            if (pImage.Size.Width > pImage.Size.Height)
            {
                //pImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                e.Graphics.DrawImage(pImage, 10, 10, 1100, 760); //'가로 이미지 크기
            }
            else
            {
                e.Graphics.DrawImage(pImage, 10, 10, 760, 1100); //'세로 이미지 크기
            }

            #region 의뢰서작업  2021-326
            //등록번호, 이름, 나이, 호실 
            using (Font font = new Font("굴림체", 10, FontStyle.Bold))
            {
                using (StringFormat stringFormat = new StringFormat())
                {
                    e.Graphics.DrawString(string.Format("등록번호: {0} 이름:{1} 나이:{2} 호실:{3}", fstrPano, fstrName, fstrAge, fstrRoom), font, Brushes.Black, 10, 10, stringFormat);
                }
            }
            #endregion
        }

        private void ImagePrt(string argGbn)
        {
            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                string strPrintDate = VB.Left(strCurDateTime, 8);
                string strPrintTime = VB.Right(strCurDateTime, 6);


                using (PrintDocument ScanPrint = new PrintDocument())
                {
                    PrintController printController = new StandardPrintController();
                    ScanPrint.PrintController = printController;  //기본인쇄창 없애기
                    PageSettings ps = new PageSettings();
                    ps.Margins = new Margins(10, 10, 10, 10);
                    ScanPrint.DefaultPageSettings = ps;

                    TabPage tabPage = argGbn.Equals("OCSFILE") ? tabPage1 : tabPage2;
                    for (int i = 0; i < tabPage.Controls.Count; i++)
                    {
                        Image image = (tabPage.Controls[i] as PictureBox).Image;
                        if (image == null)
                            continue;

                        ScanPrint.PrintPage += (sender2, e2) => pBox_PrintPageEx1(sender2, e2, image.Tag.ToString());
                        ps.Landscape = image.Width > image.Height;
                        ScanPrint.Print();

                        ScanPrint.PrintPage -= (sender2, e2) => pBox_PrintPageEx1(sender2, e2, image.Tag.ToString());
                    }
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

        private void SET_CP(string strIPDNO)
        {

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strCPCODE = "";
            string strCPNAME = "";
            string strCPDROP = "";
            string strCPCANCER = "";
            string strOCSFile = "";
            string strPATFile = "";

            string strOCSFile1 = "";
            string strPATFile1 = "";

            string strSTARTDATE = "";
            string strDEPTCODE = "";
            string strSTARTSABUN = "";

            if (picOcs1.Image != null)
            {
                picOcs1.Image.Dispose();
                picOcs1.Image = null;
            }

            if (picOcs2.Image != null)
            {
                picOcs2.Image.Dispose();
                picOcs2.Image = null;
            }

            if (picPat1.Image != null)
            {
                picPat1.Image.Dispose();
                picPat1.Image = null;
            }

            if (picPat2.Image != null)
            {
                picPat2.Image.Dispose();
                picPat2.Image = null;
            }

            txtCPCODE.Text = "";
            txtCPNAME.Text = "";
            txtDEPTCODE.Text = "";
            txtDRNAME.Text = "";

            //CP코드 읽어오기
            SQL = " SELECT CPCODE,  ";
            SQL += ComNum.VBLF + " CPNO,  ";
            SQL += ComNum.VBLF + " (SELECT NAME ";
            SQL += ComNum.VBLF + "   FROM KOSMOS_OCS.OCS_CP_SUB         ";
            SQL += ComNum.VBLF + "  WHERE CPCODE = A.CPCODE             ";
            SQL += ComNum.VBLF + "    AND GUBUN  = '03'                 ";
            SQL += ComNum.VBLF + "    AND SDATE = (SELECT MAX(SDATE) FROM KOSMOS_OCS.OCS_CP_MAIN WHERE CPCODE = A.CPCODE)                ";
            SQL += ComNum.VBLF + "    AND TRIM(TO_CHAR(DSPSEQ,'00')) = A.DROPCD AND ROWNUM = 1) CPDROP, ";
            SQL += ComNum.VBLF + " (SELECT NAME ";
            SQL += ComNum.VBLF + "   FROM KOSMOS_OCS.OCS_CP_SUB         ";
            SQL += ComNum.VBLF + "  WHERE CPCODE = A.CPCODE             ";
            SQL += ComNum.VBLF + "    AND GUBUN  = '02'                 ";
            SQL += ComNum.VBLF + "    AND SDATE = (SELECT MAX(SDATE) FROM KOSMOS_OCS.OCS_CP_MAIN WHERE CPCODE = A.CPCODE)                ";
            SQL += ComNum.VBLF + "    AND TRIM(TO_CHAR(DSPSEQ,'00')) = A.CANCERCD AND ROWNUM = 1) CPCANCER ";
            SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_CP_RECORD A ";
            SQL += ComNum.VBLF + " WHERE IPDNO = " + strIPDNO;
            SQL += ComNum.VBLF + "   AND CPNO = (SELECT MAX(CPNO) FROM KOSMOS_OCS.OCS_CP_RECORD WHERE IPDNO = A.IPDNO AND DELDATE IS NULL) ";
            SQL += ComNum.VBLF + "   AND DELDATE IS NULL ";
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
                ComFunc.MsgBoxEx(this, "CP정보가 없습니다.");
                return;
            }
            if (dt.Rows.Count > 0)
            {
                strCPCODE = dt.Rows[0]["CPCODE"].ToString().Trim();
                strCPDROP = dt.Rows[0]["CPDROP"].ToString().Trim();
                strCPCANCER = dt.Rows[0]["CPCANCER"].ToString().Trim();
                fstrCpNo = dt.Rows[0]["CPNO"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;

            //CP이름 읽어오기
            SQL = " SELECT BASCD, BASNAME ";
            SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_BASCD ";
            SQL += ComNum.VBLF + "  WHERE GRPCDB = 'CP관리' ";
            SQL += ComNum.VBLF + "    AND GRPCD = 'CP코드관리' ";
            SQL += ComNum.VBLF + "    AND BASCD = '" + strCPCODE + "' ";
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
                ComFunc.MsgBoxEx(this, "CP정보가 없습니다.");
                return;
            }
            if (dt.Rows.Count > 0)
            {
                strCPNAME = dt.Rows[0]["BASNAME"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;

            //CP 교육파일 읽어오기
            SQL = " SELECT CPCODE, OCSEDUFILE, PATEDUFILE, OCSEDUFILE1, PATEDUFILE1, CPDAY                                  ";
            SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_CP_MAIN A                                                           ";
            SQL += ComNum.VBLF + " WHERE CPCODE = '" + strCPCODE + "'                                                       ";
            SQL += ComNum.VBLF + "   AND SDATE = (SELECT MAX(SDATE) FROM KOSMOS_OCS.OCS_CP_MAIN WHERE CPCODE = A.CPCODE)    ";

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
                ComFunc.MsgBoxEx(this, "CP정보가 없습니다.");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                strOCSFile = dt.Rows[0]["OCSEDUFILE"].ToString().Trim();
                strPATFile = dt.Rows[0]["PATEDUFILE"].ToString().Trim();

                strOCSFile1 = dt.Rows[0]["OCSEDUFILE1"].ToString().Trim();
                strPATFile1 = dt.Rows[0]["PATEDUFILE1"].ToString().Trim();

                ssSet_Sheet1.RowCount = 0;
                ssSet_Sheet1.RowCount = Convert.ToInt32(dt.Rows[0]["CPDAY"].ToString().Trim());
                for (int i = 0; i < ssSet_Sheet1.RowCount; i++)
                {
                    ssSet_Sheet1.Cells[i, 0].Text = (i + 1).ToString();
                }

                fn_Read_SetOrder(ssSetOrder, strCPCODE, "CP처방" + Convert.ToInt32(ssSet_Sheet1.Cells[0, 0].Text.Trim()).ToString("00"), 1);
            }
            dt.Dispose();
            dt = null;

            #region CP동의서 읽어오기

            ssConsent_Sheet1.RowCount = 0;

            SQL = "SELECT";
            SQL += ComNum.VBLF + "  A.GUBUN, A.CODE, A.NAME, B.CPDAY                                                                    ";
            SQL += ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_CP_SUB A                                                               ";
            SQL += ComNum.VBLF + "INNER JOIN " + ComNum.DB_MED + "OCS_CP_MAIN B                                                         ";
            SQL += ComNum.VBLF + "   ON B.CPCODE = A.CPCODE                                                                             ";
            SQL += ComNum.VBLF + "  AND B.SDATE  = A.SDATE                                                                              ";
            SQL += ComNum.VBLF + "WHERE A.CPCODE = '" + strCPCODE + "'                                                                  ";
            SQL += ComNum.VBLF + "  AND A.SDATE = (SELECT MAX(SDATE) FROM KOSMOS_OCS.OCS_CP_MAIN WHERE CPCODE = A.CPCODE)               ";
            SQL += ComNum.VBLF + "ORDER BY GUBUN                                                                                        ";

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

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                switch (dt.Rows[i]["GUBUN"].ToString().Trim())
                {
                    case "07": // 동의서
                        ssConsent_Sheet1.RowCount += 1;
                        ssConsent_Sheet1.Cells[ssConsent_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["CODE"].ToString().Trim();
                        ssConsent_Sheet1.Cells[ssConsent_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["NAME"].ToString().Trim();
                        break;
                }
            }

            #endregion

            SQL = " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') STARTDATE, DEPTCODE, STARTSABUN                            ";
            SQL += ComNum.VBLF + "   , CASE WHEN CANCERDATE IS NOT NULL THEN TO_CHAR(TO_DATE(CANCERDATE,'YYYY-MM-DD'),'YYYY-MM-DD')         ";
            SQL += ComNum.VBLF + "          WHEN DROPDATE IS NOT NULL THEN TO_CHAR(TO_DATE(DROPDATE,'YYYY-MM-DD'),'YYYY-MM-DD')             ";
            SQL += ComNum.VBLF + "      END CANCERDATE                                                                                      ";
            SQL += ComNum.VBLF + "   FROM KOSMOS_OCS.OCS_CP_RECORD A                                                                        ";
            SQL += ComNum.VBLF + "  WHERE IPDNO = " + strIPDNO;
            SQL += ComNum.VBLF + "    AND CPNO = (SELECT MAX(CPNO) FROM KOSMOS_OCS.OCS_CP_RECORD WHERE IPDNO = A.IPDNO)                     ";
            SQL += ComNum.VBLF + "   AND DELDATE IS NULL ";

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
                ComFunc.MsgBoxEx(this, "CP정보가 없습니다.");
                return;
            }
            if (dt.Rows.Count > 0)
            {
                strSTARTDATE = dt.Rows[0]["STARTDATE"].ToString().Trim();
                strDEPTCODE = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                strSTARTSABUN = dt.Rows[0]["STARTSABUN"].ToString().Trim();

                string CANCERDATE = dt.Rows[0]["CANCERDATE"].ToString().Trim();

                lblCpInfo.Text = string.Format("CP 등록일:  {0}      CP 중단일:  {1}", strSTARTDATE, CANCERDATE);
            }
            dt.Dispose();
            dt = null;

            txtCPCODE.Text = strCPCODE;
            txtCPNAME.Text = strCPNAME;
            txtDEPTCODE.Text = strDEPTCODE;
            using (ComFunc cf = new ComFunc())
            {
                txtDRNAME.Text = cf.Read_SabunName(clsDB.DbCon, strSTARTSABUN);
            }
            txtCPCANCER.Text = strCPCANCER;
            txtCPDROP.Text = strCPDROP;

            GET_IMAGE(strOCSFile, strOCSFile1, "OCSFILE");
            GET_IMAGE(strPATFile, strPATFile1, "PATFILE");

        }

        private void btnPrtOCS_Click(object sender, EventArgs e)
        {
            ImagePrt("OCSFILE");
        }

        private void btnPrtPAT_Click(object sender, EventArgs e)
        {
            ImagePrt("PATFILE");
        }

        private void ssSet_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssSet_Sheet1.RowCount == 0) return;

            fn_Read_SetOrder(ssSetOrder, txtCPCODE.Text,
                "CP처방" + Convert.ToInt32(ssSet_Sheet1.Cells[e.Row, 0].Text.Trim()).ToString("00"), 1);
        }

        void fn_Read_SetOrder(FarPoint.Win.Spread.FpSpread SpdNm, string strCpCode, string strSetName, int sGbOrder)
        {
            SpdNm.ActiveSheet.RowCount = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL += " SELECT A.ORDERCODE,                                                                                                    \r";
                SQL += "        CASE WHEN A.ORDERCODE  IN('S/O', 'V/S', 'V001', 'V002', 'V003', 'V005', 'V008', 'V009') THEN REMARK             \r";
                SQL += "             WHEN B.ORDERNAMES IS NOT NULL THEN B.ORDERNAME  || ' ' ||  B.ORDERNAMES                                   \r";
                SQL += "             WHEN B.DISPHEADER IS NOT NULL AND B.SLIPNO <> '0005' THEN B.DISPHEADER || ' ' ||  B.ORDERNAME              \r";
                SQL += "             ELSE B.ORDERNAME                                                                                           \r";
                SQL += "         END  ORDERNAME1                                                                                                \r";
                SQL += "      , CASE WHEN A.GBINFO IS NOT NULL THEN A.GBINFO                                                                    \r";
                SQL += "             WHEN A.BUN < '30' THEN KOSMOS_OCS.FC_OCS_ODOSAGE_NAME(A.DOSCODE)                                           \r";
                SQL += "             ELSE KOSMOS_OCS.FC_OCS_OSPECIMAN_NAME(A.DOSCODE, A.SLIPNO) END DOSNAME                                     \r";
                SQL += "      , A.CONTENTS, A.QTY, A.GBDIV, A.NAL, A.GBSELF, A.GBER, A.GBPORT                                                   \r";
                SQL += "      , A.GBGROUP, A.REMARK, A.SUCODE, B.GBINPUT, A.PRN_REMARK                                                          \r";
                SQL += "      , A.PRN_INS_GBN, A.PRN_INS_UNIT, A.PRN_INS_SDATE, A.PRN_INS_EDATE                                                 \r";
                SQL += "      , A.PRN_INS_MAX, A.PRN_DOSCODE, A.PRN_TERM, A.PRN_NOTIFY, A.PRN_UNIT                                              \r";
                SQL += "      , A.SUBUL_WARD, A.ROWID RID                                                                                       \r";
                SQL += "      , NVL(A.ILLCODES_KCD6, A.ILLCODES) ILLCODES                                                                       \r";
                SQL += "      , A.BOOWI1,  A.BOOWI2, A.BOOWI3, A.BOOWI4, A.GBINFO , A.SLIPNO                                                    \r";
                SQL += "      , B.SENDDEPT, B.DISPHEADER, B.GBBOTH, B.ORDERNAME, B.ORDERNAMES                                                   \r";
                SQL += "   FROM KOSMOS_OCS.OCS_OPRM_CP   A                                                                                      \r";
                SQL += "      , KOSMOS_OCS.OCS_ORDERCODE B                                                                                      \r";
                SQL += "    WHERE a.PRMname   = '" + strSetName + "'                                                                            \r";
                SQL += "      AND A.CPCODE = '" + strCpCode + "'";

                if (sGbOrder == 3)
                {
                    SQL += "    AND A.GBORDER = 'P'                                                 \r";    //검사처방
                }
                SQL += "    AND a.ORDERCODE = b.ORDERCODE(+)                                        \r";
                SQL += "  ORDER BY a.Seqno, a.Slipno                                                \r";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    SpdNm.ActiveSheet.RowCount = 0;
                    dt.Dispose();
                    dt = null;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    SpdNm.ActiveSheet.RowCount = 0;
                    dt.Dispose();
                    dt = null;
                    return;
                }

                SpdNm.ActiveSheet.RowCount = dt.Rows.Count;

                if (dt.Rows.Count > 0)
                {
                    clsDB.DataTableToSpdRow(dt, SpdNm, 0, true);
                }

                dt.Dispose();
                dt = null;

                SpdNm.ActiveSheet.RowCount = SpdNm.ActiveSheet.NonEmptyRowCount;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void ssConsent_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (ssConsent_Sheet1.RowCount == 0)
            {
                return;
            }


            if (ComFunc.MsgBoxQEx(this, "해당 동의서를 출력 하시겠습니까?") == DialogResult.No)
            {
                return;
            }

            if (frmAgreePrintX != null)
            {
                frmAgreePrintX.Dispose();
                frmAgreePrintX = null;
            }

            string strFormNo = GetFomrNo(ssConsent_Sheet1.Cells[e.Row, 0].Text.Trim());

            if (strFormNo.Length == 0)
            {
                ComFunc.MsgBoxEx(this, "등록되어 있는 동의서번호가 없습니다 다시 확인해주세요.");
                ssConsent_Sheet1.SetActiveCell(e.Row, 0);
                return;
            }

            string strActDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");

            frmAgreePrintX = new frmAgreePrint(fstrPano, "0", "I", fstrINDATE.Replace("-", "") , "120000", "", "120000", fstrDeptCode.Trim(), clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, fstrDeptCode), fstrDrCode, "1", strFormNo);
            frmAgreePrintX.rEventClose += frmAgreePrintX_rEventClose;
            frmAgreePrintX.StartPosition = FormStartPosition.CenterScreen;
            frmAgreePrintX.Show();
        }

        private void frmAgreePrintX_rEventClose()
        {
            frmAgreePrintX.Dispose();
            frmAgreePrintX = null;
        }

        string GetFomrNo(string strCode)
        {

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            string strVal = string.Empty;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "SELECT NFLAG1";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BASCD";
                SQL += ComNum.VBLF + "WHERE GRPCDB = 'CP관리'";
                SQL += ComNum.VBLF + "  AND GRPCD  = 'CP동의서'";
                SQL += ComNum.VBLF + "  AND BASCD  = '" + strCode + "'";


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return strVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return strVal;
                }

                strVal = dt.Rows[0]["NFLAG1"].ToString().Trim();

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                return strVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return strVal;
            }
        }


        private void CP_LAYOUT()
        {
            if (frmCPPrintFormX != null)
            {
                frmCPPrintFormX.Close();
                frmCPPrintFormX.Dispose();
                frmCPPrintFormX = null;
            }

            frmCPPrintFormX = new frmCPPrintForm(txtCPCODE.Text.Trim());
            frmCPPrintFormX.TopLevel = false;
            frmCPPrintFormX.Parent = tabPage5;
            frmCPPrintFormX.Dock = DockStyle.Fill;
            tabPage5.Controls.Add(frmCPPrintFormX);

            frmCPPrintFormX.Text = "";
            frmCPPrintFormX.ControlBox = false;
            frmCPPrintFormX.FormBorderStyle = FormBorderStyle.None;
            frmCPPrintFormX.Top = 0;
            frmCPPrintFormX.Left = 0;
            frmCPPrintFormX.Show();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComFunc.MsgBoxQEx(this, "정말 CP처방을 중단하시겠습니까?") == DialogResult.No)
                return;

            using (Form frm = new frmOcsCpCancer(fstrCpNo))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(this);
            }
        }

        private void frmCPInfo_Nurse_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmCPPrintFormX != null)
            {
                frmCPPrintFormX.Close();
                frmCPPrintFormX.Dispose();
                frmCPPrintFormX = null;
            }
        }
    }
}
