using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using ComNurLibB;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : MedOprNr
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-02-14
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= d:\psmh\mtsEmr\CADEX\FrmCommonKnowViewInfect.frmm" >> frmCommonKnowViewInfect.cs 폼이름 재정의" />

    public partial class frmCommonKnowViewInfect : Form
    {
        clsIpdNr CIN = new clsIpdNr();
        string strDTP = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

        string mstrWRTNO = "";

        public frmCommonKnowViewInfect(string strWRTNO)
        {
            InitializeComponent();
            mstrWRTNO = strWRTNO;
        }



        private void frmCommonKnowViewInfect_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            //int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strROWID = "";


            Cursor.Current = Cursors.WaitCursor;
            
            try
            {
                SQL = "";
                SQL = " SELECT ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_INFECT_MSG ";
                SQL = SQL + ComNum.VBLF + " WHERE STRWRTNO = '" + mstrWRTNO + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                }

                if (strROWID == "")
                {
                    this.Close();
                }

                dt.Dispose();
                dt = null;

                OCS_Infect_Msg_show(strROWID);

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        private void btnDownLoad_Click(object sender, EventArgs e)
        {
            //string strFile = "";
            string strLocal = "";
            string strServerNameT = "192.168.100.31";
            string strUserNameT = "oracle";
            //string strPasswordT = "pcnfs1";
            //string strHostDir = "/pcnfs/nurCKnow/";
            string strFileName = string.Empty;

            string strWRTNo = "";

            Ftpedt FtpedtX = new Ftpedt();

            //if (FtpedtX.FtpConnetBatch(strServerNameT, strUserNameT, strPasswordT) == false)
            //{
            //    ComFunc.MsgBox("FTP Server Connect ERROR !!!", "오류");
            //    return;
            //}

            SaveFileDialog SaveFile = new SaveFileDialog();
            SaveFile.Title = "저장";
            SaveFile.Filter = "모든 파일 (*.*)|*.*|텍스트 문서(*.TXT)|*.TXT|HTML 문서(*.HTM)|*.HTM";
            SaveFile.FileName = TxtFileName.Text.Trim();

            strFileName = SaveFile.FileName;
            SaveFile.ShowDialog();

            if (string.IsNullOrEmpty(strFileName))
                return; 

            if (System.IO.File.Exists(strFileName))
            {
                if (ComFunc.MsgBoxQ(strFileName + " 파일이 존재합니다." + ComNum.VBLF + "파일을 삭제하고 다운로드를 하시겠습니까?") == DialogResult.No)
                {
                    return;
                }

                System.IO.File.Delete(strFileName);
            }


            strLocal = SaveFile.FileName;
            strWRTNo = TxtstrWRTNO.Text.Trim();

            if (strLocal == "")
            {
                ComFunc.MsgBox("저장 취소", "취소");
                FtpedtX.FtpDisConnetBatch();
                FtpedtX = null;
                return;
            }

            //if (FtpedtX.FtpDownload(strServerNameT, strUserNameT, strPasswordT, strLocal, strHostDir + strFile, strHostDir) == false)
            if (FtpedtX.FtpDownload(strServerNameT, strUserNameT, FtpedtX.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle"), strLocal, "F_" + strWRTNo, "/data/EDMS_DATA/INFECT") == false)
            {
                ComFunc.MsgBox("다운로드 실패", "종료");
                FtpedtX.FtpDisConnetBatch();
                FtpedtX = null;
                return;
            }

            //FtpedtX.FtpDisConnetBatch();
            FtpedtX = null;

            ComFunc.MsgBox("다운로드 완료");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OCS_Infect_Msg_show(string ArgROWID)
        {
            //int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            //string strROWID = "";

            Cursor.Current = Cursors.WaitCursor;

            TxtTitle.Text = "";
            TxtFileName.Text = "";
            TxtFileName.Visible = false;
            TxtstrWRTNO.Text = "";

            try
            {
                SQL = "";
                SQL = " SELECT Title,Remark,Remark2, strWRTNO,FileName,Bold,ForeColor, VIEW1, GBPRINT";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_MED + "OCS_INFECT_MSG";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + ArgROWID + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    //'감염관리 공지사항 인쇄 권한 시행되면 주석 제거할 예정
                    if (dt.Rows[0]["GBPRINT"].ToString().Trim() == "Y")
                    {
                        //mnuPrt
                        //btnPrint
                        btnPrint.Enabled = true;
                    }
                    else
                    {
                        btnPrint.Enabled = false;
                    }

                    lblTitle.Visible = true;
                    TxtTitle.Text = dt.Rows[0]["Title"].ToString().Trim();
                    TxtTitle.Visible = true;

                    TxtFileName.Text = dt.Rows[0]["FileName"].ToString().Trim();

                    if (TxtFileName.Text != "")
                    {
                        TxtFileName.Visible = true;
                    }
                    else
                    {
                        TxtFileName.Visible = false;
                    }

                    if (TxtFileName.Text != "")
                    {
                        btnDownLoad.Visible = true;
                    }
                    else
                    {
                        btnDownLoad.Visible = false;
                    }

                    TxtstrWRTNO.Text = dt.Rows[0]["strWRTNO"].ToString().Trim();

                    if (dt.Rows[0]["REMARK"].ToString().Trim() == "")
                    {
                        TxtInfo3.SelectedRtf = dt.Rows[0]["Remark2"].ToString().Trim();
                    }
                    else
                    {
                        TxtInfo3.Text = dt.Rows[0]["Remark"].ToString().Trim();
                    }

                    if (dt.Rows[0]["Bold"].ToString().Trim() == "1")
                    {

                    }
                    else
                    {

                    }
                    TxtTitle.ForeColor = System.Drawing.Color.Black;

                    if (dt.Rows[0]["ForeColor"].ToString().Trim() != "")
                    {
                        //TxtTitle.ForeColor = ColorTranslator.FromWin32(int.Parse(dt.Rows[0]["ForeColor"].ToString().Trim(), System.Globalization.NumberStyles.AllowHexSpecifier));
                        TxtTitle.ForeColor = ColorTranslator.FromWin32(int.Parse(dt.Rows[0]["ForeColor"].ToString().Trim()));
                    }
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            string strName = "";

            //int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }


            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread SP = new clsSpread();

            //string strTitle = "";
            //string strTitle1 = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT * FROM KOSMOS_ADM.INSA_MST";
                SQL = SQL + ComNum.VBLF + " WHERE JIKJONG = '41'";
                SQL = SQL + ComNum.VBLF + "      AND JIK IN (SELECT CODE FROM KOSMOS_ADM.INSA_CODE";
                SQL = SQL + ComNum.VBLF + "WHERE GUBUN = '2'";
                SQL = SQL + ComNum.VBLF + "    AND (NAME LIKE '%책임%'";
                SQL = SQL + ComNum.VBLF + "       OR NAME LIKE '%과장%'";
                SQL = SQL + ComNum.VBLF + "       OR NAME LIKE '%수%'";
                SQL = SQL + ComNum.VBLF + "       OR NAME LIKE '%부장%'))";
                SQL = SQL + ComNum.VBLF + "    AND TOIDAY IS NULL";
                SQL = SQL + ComNum.VBLF + "    AND SABUN = '" + VB.Val(clsType.User.Sabun).ToString("00000") + "'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ComFunc.MsgBox("인쇄권한이 없습니다.", "확인");
                    dt.Dispose();
                    dt = null;
                    return;
                }

                strName = VB.InputBox("해당 병동의 책임자 이름을 입력하세요.", "책임자명");

                if (strName == "")
                {
                    MessageBox.Show("이름이 입력되지 않았습니다");
                }

                ssPrt_Sheet1.Cells[4, 3].Text = CIN.gsWard;
                ssPrt_Sheet1.Cells[4, 5].Text = strName;

                ssPrt_Sheet1.Cells[5, 3].Text = TxtTitle.Text;

                ssPrt_Sheet1.Cells[10, 2].Text = TxtInfo3.Text;

                ssPrt_Sheet1.Cells[10, 4].Text = strDTP;

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 35, 35, 20, 20);
                setOption = new clsSpread.SpdPrint_Option(FarPoint.Win.Spread.PrintOrientation.Portrait, FarPoint.Win.Spread.PrintType.All, 0, 0, true, true, true, true, true, false, false);

                SP.setSpdPrint(ssPrt, PrePrint, setMargin, setOption, strHeader, strFooter);

                MessageBox.Show("출력 되었습니다", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        private void btnPrintImg_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
        }
    }
}
