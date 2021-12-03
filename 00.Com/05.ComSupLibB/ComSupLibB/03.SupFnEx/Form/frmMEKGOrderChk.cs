using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using Oracle.DataAccess.Client;
using DevComponents.DotNetBar;
using ComLibB;

namespace ComSupLibB.SupFnEx
{
    /// <summary>
    /// Class Name      : ComSupLibB
    /// File Name       : frmMekgOrderChk.cs
    /// Description     : EKG 액팅 폼
    /// Author          : 김욱동
    /// Create Date     : 2020-11-11
    /// Update History  :    
    /// <history>       
    /// </history>
    /// <seealso>
    ///
    /// </seealso>
    /// <vbp>
    /// default 		: 
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    public partial class frmMEKGOrderChk : Form , MainFormMessage
    {        
        string mJobSabun = "";
        //string gTab = "0";
        string strPano = "";
        string strGbu = "";
        string strWard = "";
        string gsWard = "";

        frmEmrViewer viewMain = null;

        #region MainFormMessage InterFace

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

        //void eFormClosed(object sender, FormClosedEventArgs e)
        //{
        //    if (this.mCallForm == null)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        this.mCallForm.MsgUnloadForm(this);
        //    }
        //}

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
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }

        #endregion

        public frmMEKGOrderChk()
        {
            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(eFormClosed);
        }


        public frmMEKGOrderChk(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            this.FormClosed += new FormClosedEventHandler(eFormClosed);
        }

        public frmMEKGOrderChk(string strJobSabun)
        {
            InitializeComponent();
            mJobSabun = strJobSabun;
            SetEvent();
            this.FormClosed += new FormClosedEventHandler(eFormClosed);
        }

        void SetEvent()
        {
            this.tab1.Click += new EventHandler(eTabEvent);
            this.tab3.Click += new EventHandler(eTabEvent);
            this.tab4.Click += new EventHandler(eTabEvent);
        }

        void eTabEvent(object sender, EventArgs e)
        {
            SuperTabItem ss = (SuperTabItem)sender;


            if (sender == this.tab1)
            {
                GetData(strPano,"0");
            }
            else if (sender == this.tab3)
            {
                GetData(strPano, "1");
            }
            else if (sender == this.tab4)
            {
                
                GetData(strPano, "2");
                
            }
            

        }


        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void frmMEKGOrderChk_Load(object sender, EventArgs e)
        {

            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회

            //dtpExamDate.Value = DateTime.Now.AddDays(-1);
           
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
            gsWard = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "");
            Screen_Clear();
            WardList();
            GetPanoListData();

        }

        void Screen_Clear()
        {
            for(int i = 0; i < ssPano_Sheet1.RowCount; i++)
            {
                for(int j = 0; j < ssPano_Sheet1.ColumnCount; j++)
                {
                    ssPano_Sheet1.Cells[i, j].Text = "";
                }
            }
        }

        private void WardList()
        {
            #region ComboWard_SET()
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            ////cboWard.Items.Clear();
            ////cboWard.Items.Add("전체");

            ////int sIndex = -1;
            ////int sCount = 0;

            //try
            //{
            //    SQL = " SELECT NAME WARDCODE, MATCH_CODE";
            //    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_CODE";
            //    SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '2'";
            //    SQL = SQL + ComNum.VBLF + "   AND SUBUSE = '1'";
            //    SQL = SQL + ComNum.VBLF + "   AND MATCH_CODE = '" + clsType.User.BuseCode + "'";
            //    SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING";

            //    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            //    if (SqlErr.Length > 0)
            //    {
            //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
            //        ComFunc.MsgBoxEx(this, SqlErr);
            //        return;
            //    }

            //    if (dt.Rows.Count > 0 )
            //    {
            //        strWard = dt.Rows[0]["WARDCODE"].ToString().Trim();
            //        if (strWard == "ER")
            //        {
            //            strGbu = "O";
            //            groupBox5.Visible = true;
            //        }
            //        else
            //        {
            //            strGbu = "I";
            //        }

            //    }
            //    else
            //    {
            //        strGbu = "O";
            //    }

            //    dt.Dispose();
            //cboWard.Items.Clear();
            //cboWard.Items.Add("전체");

            //int sIndex = -1;
            //int sCount = 0;

            try
            {
                SQL = " SELECT NAME WARDCODE, MATCH_CODE";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_CODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '2'";
                SQL = SQL + ComNum.VBLF + "   AND SUBUSE = '1'";
                SQL = SQL + ComNum.VBLF + "   AND CODE = '" + gsWard + "'";
                SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strWard = dt.Rows[0]["WARDCODE"].ToString().Trim();
                    if (strWard == "ER")
                    {
                        strGbu = "O";
                        groupBox5.Visible = true;
                    }
                    else
                    {
                        strGbu = "I";
                    }

                }
                else
                {
                    strGbu = "O";
                    groupBox7.Visible = true;
                }

                dt.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, "EKG WardList()", clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
            #endregion
        }
        private void ss2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
                return;
            if (ss2_Sheet1.RowCount == 0)
                return;

            clsPublic.GnLogOutCNT = 0;

            string strROWID = "";
            string strResul = "";
            string strImageGbn = "";

            strResul = ss2_Sheet1.Cells[e.Row, 7].Text.Trim();
            strROWID = ss2_Sheet1.Cells[e.Row, 10].Text.Trim();
            strImageGbn = ss2_Sheet1.Cells[e.Row, 10].Text.Trim();

            if (strResul != "" && e.Column == 7)
            {
                ECGFILE_DBToFile(strROWID, ss2_Sheet1.Cells[e.Row, 11].Text.Trim(), "1");
            }
            if (strResul != "" && e.Column == 8)
            {
                #region //Call EMR New
              
                    viewMain = new frmEmrViewer(ss2_Sheet1.Cells[e.Row, 11].Text);
                    viewMain.Show();
                    viewMain.SetEkg(strROWID);
                #endregion //Call EMR New
                //ComFunc.MsgBox("EMR 이동하는 기능은 현재 점검 중 입니다.");
                //return;

            }
        }

        private void ss3_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
                return;
            if (ss3_Sheet1.RowCount == 0)
                return;

            clsPublic.GnLogOutCNT = 0;

            string strROWID = "";
            string strResul = "";
        
            strResul = ss3_Sheet1.Cells[e.Row, 2].Text.Trim();
            strROWID = ss3_Sheet1.Cells[e.Row, 3].Text.Trim();

            if (strResul != "" && e.Column == 2)
            {
                ECGFILE_DBToFileW(strROWID, ss3_Sheet1.Cells[e.Row, 1].Text.Trim(), "1");
            }
        }

        private void ECGFILE_DBToFile(string strROWID, string strPtNo, string strViewerExe)
        {
            bool bolWin7 = true;
            bool bolWin10 = true;

            string strECGFile = "";

            string strFileName = "";
            string strRemotePath = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            DataTable dt = null;

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            if (clsVbfunc.GetFile("C:\\Program Files\\NKC\\ECG Viewer3\\FileViewer.exe") == "")
            {
                bolWin7 = false;
            }
            else
            {
                strECGFile = @"C:\Program Files\NKC\ECG Viewer3\FileViewer.exe";
            }

            if (clsVbfunc.GetFile("C:\\Program Files (x86)\\NKC\\ECG Viewer3\\FileViewer.exe") == "")
            {
                bolWin10 = false;
            }
            else
            {
                strECGFile = @"C:\Program Files (x86)\NKC\ECG Viewer3\FileViewer.exe";
            }

            if (bolWin7 == false && bolWin10 == false)
            {
                ComFunc.MsgBox("ECG(EKG) Viewer가 설치되지 안았습니다.");
                return;
            }

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인


                //'2014-11-18 FTP저장체크
                SQL = " SELECT ROWID,TO_CHAR(RDATE,'YYYYMMDD') AS RDATE, FILEPATH FROM KOSMOS_OCS.ETC_JUPMST ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + strROWID + "' ";
                SQL = SQL + ComNum.VBLF + "  AND GbFTP ='Y' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                strFileName = @"C:\CMC\ECG_" + strPtNo + ".ecg";

                if (dt.Rows.Count > 0)
                {

                    strRemotePath = "/data/ocs_etc/" + dt.Rows[0]["RDATE"].ToString().Trim() + "/";


                    //'2014-11-18 서버에서 PC로 파일을 다운로드함
                    using (Ftpedt FtpedtX = new Ftpedt())
                    {
                        FtpedtX.FtpDownload("192.168.100.31", "oracle", FtpedtX.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle"), strFileName, dt.Rows[0]["FILEPATH"].ToString().Trim(), strRemotePath);
                    }

                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    dt.Dispose();
                    dt = null;

                    SQL = "";
                    SQL = "SELECT IMAGE ";
                    SQL = SQL + ComNum.VBLF + "FROM KOSMOS_OCS.ETC_JUPMST ";
                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        byte[] b = (byte[])(dt.Rows[0]["IMAGE"]);

                        using (MemoryStream stream = new MemoryStream(b))
                        {
                            using (Bitmap image = new Bitmap(stream))
                            {
                                image.Save(strFileName);
                            }
                        }

                    }

                    dt.Dispose();
                    dt = null;
                }

                //'ecg viwer 실행
                if (clsVbfunc.GetFile(strFileName) != "")
                {
                    if (strViewerExe == "1")
                    {
                        //프로그램이 사용중인지 체크
                        bool isRunning = false;

                        System.Diagnostics.Process[] ProcessEx = System.Diagnostics.Process.GetProcessesByName("FileViewer");
                        if (ProcessEx.Length > 0)
                        {
                            System.Diagnostics.Process[] Pro1 = System.Diagnostics.Process.GetProcessesByName("FileViewer");
                            System.Diagnostics.Process CurPro = System.Diagnostics.Process.GetCurrentProcess();
                            foreach (System.Diagnostics.Process Proc in Pro1)
                            {
                                if (Proc.Id != CurPro.Id)
                                {
                                    isRunning = true;
                                    Proc.Kill();
                                    ComFunc.Delay(500);
                                    isRunning = false;
                                    break;
                                }
                            }
                        }

                        if (isRunning == false)
                        {
                            ComFunc.Delay(500);
                            System.Diagnostics.Process program = System.Diagnostics.Process.Start(strECGFile, strFileName + " ");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }
        }


        private void ECGFILE_DBToFileW(string strROWID, string strPtNo, string strViewerExe)
        {
            bool bolWin7 = true;
            bool bolWin10 = true;

            string strECGFile = "";

            string strFileName = "";
            string strRemotePath = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            DataTable dt = null;

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            if (clsVbfunc.GetFile("C:\\Program Files\\NKC\\ECG Viewer3\\FileViewer.exe") == "")
            {
                bolWin7 = false;
            }
            else
            {
                strECGFile = @"C:\Program Files\NKC\ECG Viewer3\FileViewer.exe";
            }

            if (clsVbfunc.GetFile("C:\\Program Files (x86)\\NKC\\ECG Viewer3\\FileViewer.exe") == "")
            {
                bolWin10 = false;
            }
            else
            {
                strECGFile = @"C:\Program Files (x86)\NKC\ECG Viewer3\FileViewer.exe";
            }

            if (bolWin7 == false && bolWin10 == false)
            {
                ComFunc.MsgBox("ECG(EKG) Viewer가 설치되지 안았습니다.");
                return;
            }

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인


                //'2014-11-18 FTP저장체크
                SQL = " SELECT ROWID,TO_CHAR(EXDATE,'YYYYMMDD') AS RDATE, FILENAME AS FILEPATH FROM KOSMOS_OCS.ETC_JUPMST_WORK ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + strROWID + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                strFileName = @"C:\CMC\ECG_" + strPtNo + ".ecg";

                if (dt.Rows.Count > 0)
                {

                    strRemotePath = "/data/ocs_etc/" + dt.Rows[0]["RDATE"].ToString().Trim() + "/";


                    //'2014-11-18 서버에서 PC로 파일을 다운로드함
                    using (Ftpedt FtpedtX = new Ftpedt())
                    {
                        FtpedtX.FtpDownload("192.168.100.31", "oracle", FtpedtX.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle"), strFileName, dt.Rows[0]["FILEPATH"].ToString().Trim(), strRemotePath);
                    }

                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    dt.Dispose();
                    dt = null;

                }

                //'ecg viwer 실행
                if (clsVbfunc.GetFile(strFileName) != "")
                {
                    if (strViewerExe == "1")
                    {
                        //프로그램이 사용중인지 체크
                        bool isRunning = false;

                        System.Diagnostics.Process[] ProcessEx = System.Diagnostics.Process.GetProcessesByName("FileViewer");
                        if (ProcessEx.Length > 0)
                        {
                            System.Diagnostics.Process[] Pro1 = System.Diagnostics.Process.GetProcessesByName("FileViewer");
                            System.Diagnostics.Process CurPro = System.Diagnostics.Process.GetCurrentProcess();
                            foreach (System.Diagnostics.Process Proc in Pro1)
                            {
                                if (Proc.Id != CurPro.Id)
                                {
                                    isRunning = true;
                                    Proc.Kill();
                                    ComFunc.Delay(500);
                                    isRunning = false;
                                    break;
                                }
                            }
                        }

                        if (isRunning == false)
                        {
                            ComFunc.Delay(500);
                            System.Diagnostics.Process program = System.Diagnostics.Process.Start(strECGFile, strFileName + " ");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }
        }


        private void ETC_FILE_DBToFile(string strROWID, string strPtNo, string strViewerExe)
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //    return; //권한 확인

            if (clsVbfunc.GetFile("C:\\WINDOWS\\SYSTEM32\\SHIMGVW.DLL") == "")
            {
                ComFunc.MsgBox("WINDOWS IMAGE Viewer가 설치되지 않았습니다.");
                return;
            }

            string strFileName = "";
            string strRemotePath = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strAudioFile = "";

            DataTable dt = null;

            try
            {

                //'2014-11-18 FTP저장체크

                SQL = " SELECT ROWID,TO_CHAR(EXDATE,'YYYYMMDD') AS EXDATE, FILENAME       ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.ETC_JUPMST_WORK";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + strROWID + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                strFileName = @"C:\CMC\ETC.jpg";

                if (dt.Rows.Count > 0)
                {
                        strRemotePath = "/data/ocs_etc/" + dt.Rows[0]["EXDATE"].ToString().Trim() + "/";

                    //    '2014-11-18 서버에서 PC로 파일을 다운로드함
                    using (Ftpedt FtpedtX = new Ftpedt())
                    {
                        FtpedtX.FtpDownload("192.168.100.31", "oracle", FtpedtX.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle"), strFileName, dt.Rows[0]["FILENAME"].ToString().Trim(), strRemotePath);
                    }

                    dt.Dispose();
                    dt = null;
                }
                else
                {
  
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    dt.Dispose();
                    dt = null;

                }

                //이미지 실행
                if (strViewerExe == "1")
                {
                    if (clsVbfunc.GetFile("%ProgramFiles%\\Windows Photo Gallery\\PhotoViewer.dll") != "")
                    {
                        strAudioFile = "rundll32.exe %ProgramFiles%\\Windows Photo Gallery\\PhotoViewer.dll, ImageView_Fullscreen " + strFileName;
                    }
                    else
                    {
                        strAudioFile = "rundll32.exe shimgvw.dll, ImageView_Fullscreen " + strFileName;

                    }
                    VB.Shell(strAudioFile, "MaximizedFocus");
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }

        }

        void btnView_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            GetPanoListData();
        }

        void GetPanoListData()
        {
            int i = 0;
            //int nREAD = 0;

            ssPano_Sheet1.RowCount = 0;
            ss2_Sheet1.RowCount = 0;
            ss3_Sheet1.RowCount = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            
            try
            {
                if (strGbu == "I")
                {
                    if (rdo2C.Checked == true)
                    {
                        SQL = "SELECT A.PTNO, A.SNAME ,COUNT(A.PTNO) AS CNT ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.ETC_JUPMST A, KOSMOS_PMPA.IPD_NEW_MASTER B";
                        SQL = SQL + ComNum.VBLF + "     WHERE A.GBJOB NOT IN ('9') ";
                        SQL = SQL + ComNum.VBLF + "     AND A.PTNO = B.PANO";
                        SQL = SQL + ComNum.VBLF + "     AND (B.OUTDATE IS NULL OR B.OUTDATE = TRUNC(SYSDATE)) ";
                        SQL = SQL + ComNum.VBLF + "     AND B.WARDCODE =  '" + strWard + "' ";
                        SQL = SQL + ComNum.VBLF + "     AND A.ORDERCODE IN ('E6541', '01030110')";
                        SQL = SQL + ComNum.VBLF + "     AND A.DEPTCODE NOT IN ('TO', 'HR')";
                        SQL = SQL + ComNum.VBLF + "     AND A.GBIO = 'I'";
                        SQL = SQL + ComNum.VBLF + "     AND (A.RDATE >=  TO_DATE('" + dtpExamDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')  " +
                                                    "      AND A.RDATE <=  TO_DATE('" + dtpExamEDate.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI'))";
                        if (string.IsNullOrEmpty(txtPano.Text.Trim()) == false)
                        {
                            SQL = SQL + ComNum.VBLF + "     AND A.PTNO = '" + txtPano.Text.Trim() + "' ";
                        }
                        if (string.IsNullOrEmpty(txtSname.Text.Trim()) == false)
                        {
                            SQL = SQL + ComNum.VBLF + "     AND A.SNAME= '" + txtSname.Text.Trim() + "' ";
                        }

                        SQL = SQL + ComNum.VBLF + " GROUP BY A.PTNO, A.SNAME ";
                        SQL = SQL + ComNum.VBLF + " HAVING COUNT(A.PTNO) > 1 ";
                        SQL = SQL + ComNum.VBLF + " ORDER BY A.SNAME DESC";
                    }
                    else
                    {
                        SQL = "SELECT A.PTNO, A.SNAME  ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.ETC_JUPMST A, KOSMOS_PMPA.IPD_NEW_MASTER B";
                        SQL = SQL + ComNum.VBLF + "     WHERE A.GBJOB NOT IN ('9') ";
                        SQL = SQL + ComNum.VBLF + "     AND A.PTNO = B.PANO";
                        SQL = SQL + ComNum.VBLF + "     AND (B.OUTDATE IS NULL OR B.OUTDATE = TRUNC(SYSDATE)) ";
                        SQL = SQL + ComNum.VBLF + "     AND B.WARDCODE =  '" + strWard + "' ";
                        SQL = SQL + ComNum.VBLF + "     AND A.ORDERCODE IN ('E6541', '01030110')";
                        SQL = SQL + ComNum.VBLF + "     AND A.DEPTCODE NOT IN ('TO', 'HR')";
                        SQL = SQL + ComNum.VBLF + "     AND A.GBIO = 'I'";
                        SQL = SQL + ComNum.VBLF + "     AND (A.RDATE >=  TO_DATE('" + dtpExamDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')  " +
                                                    "      AND A.RDATE <=  TO_DATE('" + dtpExamEDate.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI'))";
                        if (string.IsNullOrEmpty(txtPano.Text.Trim()) == false)
                        {
                            SQL = SQL + ComNum.VBLF + "     AND A.PTNO = '" + txtPano.Text.Trim() + "' ";
                        }
                        if (string.IsNullOrEmpty(txtSname.Text.Trim()) == false)
                        {
                            SQL = SQL + ComNum.VBLF + "     AND A.SNAME= '" + txtSname.Text.Trim() + "' ";
                        }
                        SQL = SQL + ComNum.VBLF + " GROUP BY A.PTNO,A.SNAME";
                        SQL = SQL + ComNum.VBLF + " ORDER BY A.SNAME DESC";
                    }
                }
                else if (strGbu == "O")
                {
                    if (strWard == "ER")
                    {
                        if (rdo2C.Checked == true)
                        {
                            SQL = "SELECT A.PTNO, A.SNAME ,COUNT(A.PTNO) AS CNT ";
                            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.ETC_JUPMST A, KOSMOS_PMPA.OPD_MASTER B";
                            SQL = SQL + ComNum.VBLF + "     WHERE A.PTNO = B.PANO";
                            SQL = SQL + ComNum.VBLF + "     AND A.DEPTCODE = B.DEPTCODE"; 
                            SQL = SQL + ComNum.VBLF + "     AND (B.ACTDATE >=  TO_DATE('" + dtpExamDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')  " +
                                                     "      AND B.ACTDATE <=  TO_DATE('" + dtpExamEDate.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI'))";
                            if(rdoErF.Checked == true)
                            {
                                SQL = SQL + ComNum.VBLF + "     AND B.ER_NUM IN ('47','48','49','58','59')";
                            }
                            else if(rdoErR.Checked == true)
                            {
                                SQL = SQL + ComNum.VBLF + "     AND B.ER_NUM IN ('71','72','73','74','61','62','63','64','65','66','67'," +
                                                                  " '68','69','70','43','44','45','46','03')";
                            }
                            else if (rdoErT.Checked == true)
                            {
                                SQL = SQL + ComNum.VBLF + "     AND B.ER_NUM IN ('52','53','54','55','56','57','A0','A1','A2','A3','A4','A5','A6','A7','A8')";
                            }
                            else if (rdoErW.Checked == true)
                            {
                                SQL = SQL + ComNum.VBLF + "     AND B.ER_NUM IN ('81','82','83','84','85','86','75','76','77','78')";
                            }
                            SQL = SQL + ComNum.VBLF + "     AND A.GBJOB NOT IN ('9') ";
                            SQL = SQL + ComNum.VBLF + "     AND A.ORDERCODE IN ('E6541', '01030110')";
                            SQL = SQL + ComNum.VBLF + "     AND A.DEPTCODE NOT IN ('TO', 'HR')";
                            SQL = SQL + ComNum.VBLF + "     AND A.GBIO = 'O'";
                            SQL = SQL + ComNum.VBLF + "     AND A.DEPTCODE = 'ER'";
                            SQL = SQL + ComNum.VBLF + "     AND (A.RDATE >=  TO_DATE('" + dtpExamDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')  " +
                                                        "      AND A.RDATE <=  TO_DATE('" + dtpExamEDate.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI'))";
                            if (string.IsNullOrEmpty(txtPano.Text.Trim()) == false)
                            {
                                SQL = SQL + ComNum.VBLF + "     AND A.PTNO = '" + txtPano.Text.Trim() + "' ";
                            }
                            if (string.IsNullOrEmpty(txtSname.Text.Trim()) == false)
                            {
                                SQL = SQL + ComNum.VBLF + "     AND A.SNAME= '" + txtSname.Text.Trim() + "' ";
                            }

                            SQL = SQL + ComNum.VBLF + " GROUP BY A.PTNO, A.SNAME ";
                            SQL = SQL + ComNum.VBLF + " HAVING COUNT(A.PTNO) > 1 ";
                            SQL = SQL + ComNum.VBLF + " ORDER BY A.SNAME DESC";
                        }
                        else
                        {
                            SQL = "SELECT A.PTNO, A.SNAME  ";
                            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.ETC_JUPMST A, KOSMOS_PMPA.OPD_MASTER B";
                            SQL = SQL + ComNum.VBLF + "     WHERE A.PTNO = B.PANO";
                            SQL = SQL + ComNum.VBLF + "     AND A.DEPTCODE = B.DEPTCODE";
                            SQL = SQL + ComNum.VBLF + "     AND (B.ACTDATE >=  TO_DATE('" + dtpExamDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')  " +
                                                     "      AND B.ACTDATE <=  TO_DATE('" + dtpExamEDate.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI'))";
                            if (rdoErF.Checked == true)
                            {
                                SQL = SQL + ComNum.VBLF + "     AND B.ER_NUM IN ('47','48','49','58','59')";
                            }
                            else if (rdoErR.Checked == true)
                            {
                                SQL = SQL + ComNum.VBLF + "     AND B.ER_NUM IN ('71','72','73','74','61','62','63','64','65','66','67'," +
                                                                  " '68','69','70','43','44','45','46','03')";
                            }
                            else if (rdoErT.Checked == true)
                            {
                                SQL = SQL + ComNum.VBLF + "     AND B.ER_NUM IN ('52','53','54','55','56','57','A0','A1','A2','A3','A4','A5','A6','A7','A8')";
                            }
                            else if (rdoErW.Checked == true)
                            {
                                SQL = SQL + ComNum.VBLF + "     AND B.ER_NUM IN ('81','82','83','84','85','86','75','76','77','78')";
                            }
                            SQL = SQL + ComNum.VBLF + "     AND A.GBJOB NOT IN ('9') ";
                            SQL = SQL + ComNum.VBLF + "     AND A.ORDERCODE IN ('E6541', '01030110')";
                            SQL = SQL + ComNum.VBLF + "     AND A.DEPTCODE NOT IN ('TO', 'HR')";
                            SQL = SQL + ComNum.VBLF + "     AND A.GBIO = 'O'";
                            SQL = SQL + ComNum.VBLF + "     AND A.DEPTCODE = 'ER'";
                            SQL = SQL + ComNum.VBLF + "     AND (A.RDATE >=  TO_DATE('" + dtpExamDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')  " +
                                                        "      AND A.RDATE <=  TO_DATE('" + dtpExamEDate.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI'))";
                            if (string.IsNullOrEmpty(txtPano.Text.Trim()) == false)
                            {
                                SQL = SQL + ComNum.VBLF + "     AND A.PTNO = '" + txtPano.Text.Trim() + "' ";
                            }
                            if (string.IsNullOrEmpty(txtSname.Text.Trim()) == false)
                            {
                                SQL = SQL + ComNum.VBLF + "     AND A.SNAME= '" + txtSname.Text.Trim() + "' ";
                            }
                            SQL = SQL + ComNum.VBLF + " GROUP BY A.PTNO,A.SNAME";
                            SQL = SQL + ComNum.VBLF + " ORDER BY A.SNAME DESC";
                        }
                    }
                    else
                    {
                        if (rdo2C.Checked == true)
                        {
                            SQL = "SELECT PTNO, SNAME ,COUNT(PTNO) AS CNT ";
                            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.ETC_JUPMST ";
                            SQL = SQL + ComNum.VBLF + "     WHERE GBJOB NOT IN ('9') ";
                            SQL = SQL + ComNum.VBLF + "     AND ORDERCODE IN ('E6541', '01030110')";
                            SQL = SQL + ComNum.VBLF + "     AND DEPTCODE NOT IN ('TO', 'HR')";

                            if(groupBox7.Visible == true && rdoIB.Checked == true)
                            {
                                SQL = SQL + ComNum.VBLF + "     AND GBIO = 'I'";
                            }
                            else if (groupBox7.Visible == true && rdoOB.Checked == true)
                            {
                                SQL = SQL + ComNum.VBLF + "     AND GBIO = 'O'";
                            }

                            SQL = SQL + ComNum.VBLF + "     AND (RDATE >=  TO_DATE('" + dtpExamDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')  " +
                                                        "      AND RDATE <=  TO_DATE('" + dtpExamEDate.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI'))";
                            if (string.IsNullOrEmpty(txtPano.Text.Trim()) == false)
                            {
                                SQL = SQL + ComNum.VBLF + "     AND PTNO = '" + txtPano.Text.Trim() + "' ";
                            }
                            if (string.IsNullOrEmpty(txtSname.Text.Trim()) == false)
                            {
                                SQL = SQL + ComNum.VBLF + "     AND A.SNAME= '" + txtSname.Text.Trim() + "' ";
                            }

                            SQL = SQL + ComNum.VBLF + " GROUP BY PTNO, SNAME ";
                            SQL = SQL + ComNum.VBLF + " HAVING COUNT(PTNO) > 1 ";
                            SQL = SQL + ComNum.VBLF + " ORDER BY SNAME DESC";
                        }
                        else
                        {
                            SQL = "SELECT PTNO, SNAME  ";
                            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.ETC_JUPMST ";
                            SQL = SQL + ComNum.VBLF + "     WHERE GBJOB NOT IN ('9') ";
                            SQL = SQL + ComNum.VBLF + "     AND ORDERCODE IN ('E6541', '01030110')";
                            SQL = SQL + ComNum.VBLF + "     AND DEPTCODE NOT IN ('TO', 'HR')";

                            if (groupBox7.Visible == true && rdoIB.Checked == true)
                            {
                                SQL = SQL + ComNum.VBLF + "     AND GBIO = 'I'";
                            }
                            else if (groupBox7.Visible == true && rdoOB.Checked == true)
                            {
                                SQL = SQL + ComNum.VBLF + "     AND GBIO = 'O'";
                            }

                            SQL = SQL + ComNum.VBLF + "     AND (RDATE >=  TO_DATE('" + dtpExamDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')  " +
                                                        "      AND RDATE <=  TO_DATE('" + dtpExamEDate.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI'))";
                            if (string.IsNullOrEmpty(txtPano.Text.Trim()) == false)
                            {
                                SQL = SQL + ComNum.VBLF + "     AND PTNO = '" + txtPano.Text.Trim() + "' ";
                            }
                            if (string.IsNullOrEmpty(txtSname.Text.Trim()) == false)
                            {
                                SQL = SQL + ComNum.VBLF + "     AND A.SNAME= '" + txtSname.Text.Trim() + "' ";
                            }
                            SQL = SQL + ComNum.VBLF + " GROUP BY PTNO,SNAME";
                            SQL = SQL + ComNum.VBLF + " ORDER BY SNAME DESC";
                        }
                    }
       
                }


                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssPano_Sheet1.RowCount = dt.Rows.Count;

                for(i = 0; i < ssPano_Sheet1.RowCount; i++)
                {
                    ssPano_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PTNO"].ToString().Trim() + "";
                    ssPano_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim() + "";
                }

                dt.Dispose();
                dt = null;
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }


        }

    

        void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            SaveData();

        }

        void SaveData()
        {
            string strWrtno = "";
            string strFilename = "";
            string strFTP = "";
            string strRowid = "";
            string SQL = "";
            string strPtnol = "";
            string strWrtnol = "";
            string strFilepathl = "";
            long strOrdernol = 0;
            DataTable dt = null;

            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            for(int i = 0; i < ss3.ActiveSheet.RowCount; i++)
            {
                if(ss3.ActiveSheet.Rows[i].BackColor == Color.LightPink)
                {
                    strWrtno = ss3_Sheet1.Cells[i, 5].Text.Trim();
                    strFilename = ss3_Sheet1.Cells[i, 2].Text.Trim();
                }
            }

            for (int i = 0; i < ss2.ActiveSheet.RowCount; i++)
            {
                if (ss2.ActiveSheet.Rows[i].BackColor == Color.LightPink)
                {
                    strRowid = ss2_Sheet1.Cells[i, 10].Text.Trim();
                    strFTP = ss2_Sheet1.Cells[i, 7].Text.Trim();
                }
            }

            if (string.IsNullOrEmpty(strFTP) == true)
            {
                if (string.IsNullOrEmpty(strRowid) == false && string.IsNullOrEmpty(strWrtno) == false)
                {
                    try
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + "UPDATE KOSMOS_OCS.ETC_JUPMST ";
                        SQL += ComNum.VBLF + "  SET WRTNO = '" + strWrtno + "' ";
                        SQL += ComNum.VBLF + "  ,    FILEPATH = '" + strFilename + "' ";
                        SQL += ComNum.VBLF + "  ,    GBFTP = 'Y' ";
                        SQL += ComNum.VBLF + "  ,    GBJOB ='3' ";
                        SQL += ComNum.VBLF + "WHERE ROWID = '" + strRowid + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("업데이트중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        SQL = "";
                        SQL += ComNum.VBLF + "UPDATE KOSMOS_OCS.ETC_JUPMST_WORK ";
                        SQL += ComNum.VBLF + "  SET UPPS = '" + clsType.User.IdNumber + "' ";
                        SQL += ComNum.VBLF + "  ,   UPDT = SYSDATE ";
                        SQL += ComNum.VBLF + "  ,   GBCHK = '1' ";
                        SQL += ComNum.VBLF + "  ,   GBEMR = 'Y' ";

                        SQL += ComNum.VBLF + "WHERE WRTNO = '" + strWrtno + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("업데이트중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }


                    }
                    catch (Exception ex)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                    }

                    ComFunc.MsgBox("저장완료 하였습니다.");
                    GetPanoListData();

                    #region ETC_JUPMST 를 읽어온다
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT PTNO, WRTNO, FILEPATH, ORDERNO";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "ETC_JUPMST";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "  AND ROWID = '" + strRowid + "'";
                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr + "액팅 로그 저장오류가 발생되었습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strPtnol = dt.Rows[0]["PTNO"].ToString().Trim();
                        strWrtnol = dt.Rows[0]["WRTNO"].ToString().Trim();
                        strFilepathl = dt.Rows[0]["FILEPATH"].ToString().Trim();
                        strOrdernol = (long)VB.Val(dt.Rows[0]["ORDERNO"].ToString().Trim());
                    }

                    dt.Dispose();
                    dt = null;
                    #endregion

                    #region WOKR LOG INSERT 쿼리

                    SQL = "";
                    SQL += ComNum.VBLF + " INSERT INTO KOSMOS_OCS.ETC_JUPMST_WORK_HIS  ";
                    SQL += ComNum.VBLF + "        (JOBSABUN, JOBDATE, PTNO, WRTNO, FILEPATH, ORDERNO) ";                            
                    SQL += ComNum.VBLF + " VALUES";
                    SQL += ComNum.VBLF + "      ( '" + clsType.User.IdNumber + "', SYSDATE, '" + strPtnol + "', '" + strWrtnol + "'";
                    SQL += ComNum.VBLF + "      , '" + strFilepathl + "', '" + strOrdernol + "' )";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        //clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr + "액팅 로그 저장오류가 발생되었습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);//에러로그 저장                        
                    }

                    #endregion
                }
                else
                {
                    ComFunc.MsgBox("선택하신 이미지파일 또는 환자정보가 없습니다. 선택 하신후 저장을 눌러주세요.");
                }
            }
            else
            {
                ComFunc.MsgBox("선택하신 검사는 이미 이미지파일이 있습니다. 삭제하신 후 진행하여 주세요.");
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            BackData();
        }

        void BackData()
        {
            //string strWrtno = "";
            string strFilename = "";
            string strRowid = "";
            string SQL = "";
            DataTable dt = null;
            string strPtnol = "";
            string strWrtnol = "";
            string strFilepathl = "";
            long strOrdernol = 0;
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;


            for (int i = 0; i < ss2.ActiveSheet.RowCount; i++)
            {
                if (ss2.ActiveSheet.Rows[i].BackColor == Color.LightPink)
                {
                    strRowid = ss2_Sheet1.Cells[i, 10].Text.Trim();
                    if(string.IsNullOrEmpty(ss2_Sheet1.Cells[i, 9].Text.Trim()) == false)
                    {
                        strFilename = ss2_Sheet1.Cells[i, 9].Text.Trim();
                    }
                    
                }
            }


            if (string.IsNullOrEmpty(strRowid) == false && string.IsNullOrEmpty(strFilename) == false)
            {

                try
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "UPDATE KOSMOS_OCS.ETC_JUPMST ";
                    SQL += ComNum.VBLF + "  SET WRTNO = '' ";
                    SQL += ComNum.VBLF + "  ,    FILEPATH = '' ";
                    SQL += ComNum.VBLF + "  ,    GBFTP = '' ";
                    SQL += ComNum.VBLF + "WHERE ROWID = '" + strRowid + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("업데이트중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    SQL = "";
                    SQL += ComNum.VBLF + "UPDATE KOSMOS_OCS.ETC_JUPMST_WORK ";
                    SQL += ComNum.VBLF + "  SET UPPS = '" + clsType.User.IdNumber + "' ";
                    SQL += ComNum.VBLF + "  ,   UPDT = SYSDATE ";
                    SQL += ComNum.VBLF + "  ,   GBCHK = '' ";
                    SQL += ComNum.VBLF + "  ,   GBEMR = 'N' ";
                    SQL += ComNum.VBLF + "  ,   EMRDATE = '' ";
                    SQL += ComNum.VBLF + "WHERE FILENAME = '" + strFilename + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("업데이트중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox("삭제완료 하였습니다.");
                GetPanoListData();
                #region ETC_JUPMST 를 읽어온다
                SQL = "";
                SQL += ComNum.VBLF + "SELECT PTNO, WRTNO, FILEPATH, ORDERNO";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "ETC_JUPMST";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "  AND ROWID = '" + strRowid + "'";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr + "액팅 로그 저장오류가 발생되었습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                }

                if (dt.Rows.Count > 0)
                {
                    strPtnol = dt.Rows[0]["PTNO"].ToString().Trim();
                    strWrtnol = dt.Rows[0]["WRTNO"].ToString().Trim();
                    strFilepathl = dt.Rows[0]["FILEPATH"].ToString().Trim();
                    strOrdernol = (long)VB.Val(dt.Rows[0]["ORDERNO"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;
                #endregion

                #region WOKR LOG INSERT 쿼리

                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO KOSMOS_OCS.ETC_JUPMST_WORK_HIS  ";
                SQL += ComNum.VBLF + "        (JOBSABUN, JOBDATE, PTNO, WRTNO, FILEPATH, ORDERNO) ";
                SQL += ComNum.VBLF + " VALUES";
                SQL += ComNum.VBLF + "      ( '" + clsType.User.IdNumber + "', SYSDATE, '" + strPtnol + "', '" + strWrtnol + "'";
                SQL += ComNum.VBLF + "      , '" + strFilepathl + "', '" + strOrdernol + "' )";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    //clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr + "액팅 로그 저장오류가 발생되었습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);//에러로그 저장                        
                }

                #endregion
            }
            else
            {
                ComFunc.MsgBox("선택된 행은 삭제할 이미지 파일이 없습니다.");
            }

        }

        void ssPano_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            strPano = ssPano_Sheet1.Cells[e.Row, 0].Text.Trim();
            GetData(strPano);
        }

        void GetData(string strPano , string GbJob = "")
        {
            string SQL = "";
            int i = 0;
            string SqlErr = ""; //에러문 받는 변수
            //int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;
            Cursor.Current = Cursors.WaitCursor;

            //ss2.DeleteRows ss2.DataRowCnt + 1, 1           

            try
            {
              
                SQL = "SELECT TO_CHAR(B.BDATE,'YYYY-MM-DD') BDATE ,  B.RDATE, B.SEX, B.AGE, B.DEPTCODE, B.GBIO, C.DRNAME, B.FILEPATH, A.ORDERNAME, B.ROWID, B.PTNO, B.GBJOB,B.GBPORT";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.ETC_JUPMST  B, KOSMOS_OCS.OCS_ORDERCODE A, KOSMOS_PMPA.BAS_DOCTOR C";
                //SQL = SQL + ComNum.VBLF + "     AND ((I.JDATE = TO_DATE('1900-01-01', 'YYYY-MM-DD') AND I.GBSTS <> '7') OR (I.JDATE = TO_DATE('" + dtpExamDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD') AND I.GBSTS = '7')";
                //SQL = SQL + ComNum.VBLF + "       OR I.OUTDATE = TO_DATE('" + dtpExamDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD'))";
                SQL = SQL + ComNum.VBLF + "     WHERE B.PTNO = '" + strPano + "'";
                SQL = SQL + ComNum.VBLF + "     AND B.DRCODE = C.DRCODE(+) ";
                SQL = SQL + ComNum.VBLF + "     AND B.ORDERCODE= A.ORDERCODE(+)";

                if(GbJob == "1")
                {
                    SQL = SQL + ComNum.VBLF + "     AND B.GBJOB NOT IN ('3','9') ";
                }
                else if (GbJob == "2")
                {
                    SQL = SQL + ComNum.VBLF + "     AND B.GBFTP IS NOT NULL ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "     AND B.GBJOB NOT IN ('9') ";
                }
                ///////////
                if (strGbu == "I")
                {
                    SQL = SQL + ComNum.VBLF + "     AND B.GBIO = 'I'";
                }
                else
                {
                    if (groupBox7.Visible == true && rdoIB.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     AND GBIO = 'I'";
                    }
                    else if (groupBox7.Visible == true && rdoOB.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     AND GBIO = 'O'";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     AND B.GBIO = 'O'";
                    }
                    
                }
                //////////////
                if(strWard == "ER")
                {
                    SQL = SQL + ComNum.VBLF + "     AND B.DEPTCODE = 'ER'";
                }
                SQL = SQL + ComNum.VBLF + "     AND B.ORDERCODE IN ('E6541', '01030110')";
                SQL = SQL + ComNum.VBLF + "     AND B.DEPTCODE NOT IN ('TO', 'HR')";
                SQL = SQL + ComNum.VBLF + "     AND (B.RDATE >=  TO_DATE('" + dtpExamDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')  " +
                                         "      AND B.RDATE <=  TO_DATE('" + dtpExamEDate.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI'))";
                SQL = SQL + ComNum.VBLF + " ORDER BY B.RDATE DESC";



                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ss2_Sheet1.RowCount = 0;
                ss2_Sheet1.RowCount = dt.Rows.Count;


                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ss2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["RDATE"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SEX"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["AGE"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 4].Text = dt.Rows[i]["GBIO"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DRNAME"].ToString().Trim();

                        if (string.IsNullOrEmpty(dt.Rows[i]["FILEPATH"].ToString().Trim()) == false)
                        {
                            ss2_Sheet1.Cells[i, 7].Text = "▦";
                            ss2.ActiveSheet.Rows[i].BackColor = Color.LightBlue;
                        }

                        ss2_Sheet1.Cells[i, 8].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 9].Text = dt.Rows[i]["FILEPATH"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 10].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 11].Text = dt.Rows[i]["PTNO"].ToString().Trim();

                        if (dt.Rows[i]["GBJOB"].ToString().Trim() != "3")
                        {
                            if(dt.Rows[i]["GBPORT"].ToString().Trim() != "M")
                            {
                                ss2_Sheet1.Cells[i, 12].Locked = true;
                                ss2_Sheet1.Cells[i, 13].Text = "미접수";
                            }
                            else
                            {
                                ss2_Sheet1.Cells[i, 12].Locked = false;
                                ss2_Sheet1.Cells[i, 13].Text = "-";
                            }
                        }
                        else if (ss2_Sheet1.Cells[i, 7].Text == "▦")
                        {
                            ss2_Sheet1.Cells[i, 13].Text = "검사완료";
                        }
                        else
                        {
                            ss2_Sheet1.Cells[i, 13].Text = "접수";
                        }

                    }
                }

                dt.Dispose();
                dt = null;
            }



            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }


            try
            {

                SQL = "SELECT EXDATE , PTNO, FILENAME, WRTNO, GBCHK, EXDATE, UPPS, UPDT, ROWID";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.ETC_JUPMST_WORK   ";
                //SQL = SQL + ComNum.VBLF + "     AND ((I.JDATE = TO_DATE('1900-01-01', 'YYYY-MM-DD') AND I.GBSTS <> '7') OR (I.JDATE = TO_DATE('" + dtpExamDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD') AND I.GBSTS = '7')";
                //SQL = SQL + ComNum.VBLF + "       OR I.OUTDATE = TO_DATE('" + dtpExamDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD'))";
                SQL = SQL + ComNum.VBLF + "     WHERE PTNO = '" + strPano + "'";
                SQL = SQL + ComNum.VBLF + "     AND (EXDATE >=  TO_DATE('" + dtpExamDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')  " +
                                         "      AND EXDATE <=  TO_DATE('" + dtpExamEDate.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI'))";
                if (GbJob == "1")
                {
                    SQL = SQL + ComNum.VBLF + "     AND GBCHK IS NULL ";
                }
                else if (GbJob == "2")
                {
                    SQL = SQL + ComNum.VBLF + "     AND GBCHK IS NOT NULL ";
                }
                SQL = SQL + ComNum.VBLF + " ORDER BY WRTNO DESC";



                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ss3_Sheet1.RowCount = 0;
                ss3_Sheet1.RowCount = dt.Rows.Count;

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ss3_Sheet1.Cells[i, 0].Text = dt.Rows[i]["EXDATE"].ToString().Trim();
                        ss3_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                        ss3_Sheet1.Cells[i, 2].Text = dt.Rows[i]["FILENAME"].ToString().Trim();
                        ss3_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ss3_Sheet1.Cells[i, 5].Text = dt.Rows[i]["WRTNO"].ToString().Trim();

                        if (dt.Rows[i]["GBCHK"].ToString().Trim() == "1")
                        {
                            ss3.ActiveSheet.Rows[i].BackColor = Color.LightBlue;
                            ss3_Sheet1.Cells[i, 4].Locked = true;
                            ss3_Sheet1.Cells[i, 7].Text = dt.Rows[i]["UPPS"].ToString().Trim();
                            ss3_Sheet1.Cells[i, 8].Text = dt.Rows[i]["UPDT"].ToString().Trim();

                        }

                        if (dt.Rows[i]["GBCHK"].ToString().Trim() == "1" && string.IsNullOrEmpty(ss3_Sheet1.Cells[i, 7].Text) == false)
                        {
                            ss3_Sheet1.Cells[i, 6].Text = "수동";
                        }
                        else if (dt.Rows[i]["GBCHK"].ToString().Trim() == "1" && string.IsNullOrEmpty(ss3_Sheet1.Cells[i, 7].Text) == true)
                        {
                            ss3_Sheet1.Cells[i, 6].Text = "자동";
                        }

                    }
                }

                dt.Dispose();
                dt = null;
            }



            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        void ss2_CellButtonClick(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            for (int i = 0; i < ss2.ActiveSheet.RowCount; i++)
            {
                if(i != e.Row)
                {
                    ss2.ActiveSheet.Cells[i, 12].Text = "False";
                    ss2.ActiveSheet.Rows[i].BackColor = Color.White;
                }
                
            }

            if (e.Column == 12)
            {

                if (ss2.ActiveSheet.Cells[e.Row, 12].Text == "True")
                {
                    ss2.ActiveSheet.Rows[e.Row].BackColor = Color.LightPink;

                }
                else
                {
                    ss2.ActiveSheet.Rows[e.Row].BackColor = Color.White;

                }

            }

        }
        void ss3_CellButtonClick(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            
            for(int i = 0; i < ss3.ActiveSheet.RowCount; i++ )
            {
                if (i != e.Row)
                {
                    ss3.ActiveSheet.Cells[i, 4].Text = "False";
                    ss3.ActiveSheet.Rows[i].BackColor = Color.White;
                }
            }

            if (e.Column == 4)
            {
              
                if (ss3.ActiveSheet.Cells[e.Row, 4].Text == "True")
                {
                    ss3.ActiveSheet.Rows[e.Row].BackColor = Color.LightPink;

                }
                else
                {
                    ss3.ActiveSheet.Rows[e.Row].BackColor = Color.White;
                }

            }
        }

     

    }
}
