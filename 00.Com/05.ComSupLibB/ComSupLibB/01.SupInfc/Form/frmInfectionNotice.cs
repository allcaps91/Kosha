using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ComSupLibB
{
    /// <summary>
    /// \Etc\infect_msg\infect_msg.vbp
    /// Frm감염공지마스터.frm
    /// </summary>

    public partial class frmInfectionNotice : Form
    {
        string FstrROWID = string.Empty;
        string FstrFileName = string.Empty;
        string FstrWRTNO = string.Empty;
        string FstrDate = string.Empty;

        public frmInfectionNotice()
        {
            InitializeComponent();
        }

        private void frmInfectionNotice_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            panel9.Visible = clsType.User.Sabun == "04349";

            dtpDate.Value = DateTime.Now.AddDays(-200);
            dtpDate2.Value = DateTime.Now;

            ss1_Sheet1.Columns[5, 9].Visible = false;

            cboSize.Items.Clear();
            for (int i = 9; i < 31; i++)
            {
                cboSize.Items.Add(i);
            }

            cboSize.Text = "10";

            txtRemark5.Text = string.Empty;

            SetTitle();

            Screen_Clear();
        }

        void SetTitle()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty;
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL.AppendLine("SELECT strWRTNO,SDATE,EDATE,TITLE,REMARK,REMARK2,ENTSABUN,FILENAME,FILESIZE,FILETYPE,Bold,ForeColor,View1,GbPrint,ROWID ");
                SQL.AppendLine(" FROM KOSMOS_OCS.OCS_INFECT_MSG");
                SQL.AppendLine(" WHERE GUBUN ='9'");
                SQL.AppendLine("   AND TITLE ='감염공지 타이틀'");

                SqlErr = clsDB.GetDataTable(ref dt, SQL.ToString().Trim(), clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
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

                txtRemark5.SelectedRtf = dt.Rows[0]["Remark2"].ToString().Trim();

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            GetSearchData();
        }

        void GetSearchData()
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT strWRTNO, TO_CHAR(SDATE,'YYYY-MM-DD') SDATE ,TO_CHAR(EDATE,'YYYY-MM-DD') EDATE,TITLE,REMARK,ENTSABUN,FILENAME,FILESIZE,FILETYPE,Bold,ForeColor,";
                SQL += ComNum.VBLF + "  TITLE_INFO,TITLE_INFO_TERM,ROWID ";
                SQL += ComNum.VBLF + " FROM KOSMOS_OCS.OCS_INFECT_MSG";
                SQL += ComNum.VBLF + "  WHERE SDATE >=TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND SDATE <=TO_DATE('" + dtpDate2.Text + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND GUBUN  ='0' ";
                SQL += ComNum.VBLF + " ORDER BY SDATE DESC,EDATE DESC  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
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

                ss1_Sheet1.SetRowHeight(-1, 22);
                ss1_Sheet1.RowCount = dt.Rows.Count;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SDate"].ToString().Trim();


                    if (string.IsNullOrEmpty(dt.Rows[i]["TITLE_INFO_TERM"].ToString().Trim()) == false)
                    {
                        if (DateTime.Compare(Convert.ToDateTime(dt.Rows[i]["TITLE_INFO_TERM"].ToString().Trim()), Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"))) >= 0)
                        {
                            //ss1_Sheet1.Cells[i, 10].Text = dt.Rows[i]["TITLE_INFO"].ToString().Trim();
                            ss1_Sheet1.Cells[i, 1].Text = "'" + dt.Rows[i]["TITLE_INFO"].ToString().Trim() + "' " + dt.Rows[i]["TITLE"].ToString().Trim();
                        }
                        else
                        {
                            ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Title"].ToString().Trim();
                        }
                    }
                    else
                    {
                        ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Title"].ToString().Trim();

                    }


                    ss1_Sheet1.Cells[i, 1].Font = new Font("맑은 고딕", 8.5f, dt.Rows[i]["Bold"].ToString().Trim() == "1" ? FontStyle.Bold : FontStyle.Regular);
                    ss1_Sheet1.Cells[i, 1].ForeColor = string.IsNullOrEmpty(dt.Rows[i]["ForeColor"].ToString().Trim()) ? Color.FromArgb(0, 0, 0) : Color.FromArgb(Convert.ToInt32(dt.Rows[i]["ForeColor"].ToString().Trim()));

                    ss1_Sheet1.Cells[i, 2].Text = One_Data_Read_Select_CNT(dt.Rows[i]["strWRTNO"].ToString().Trim()).ToString();
                    ss1_Sheet1.Cells[i, 3].Text = string.IsNullOrEmpty(dt.Rows[i]["FileName"].ToString().Trim()) == false ? "▦" : string.Empty;

                    ss1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["EDate"].ToString().Trim();

                    ss1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["FileName"].ToString().Trim();

                    ss1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["FileType"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["strWRTNO"].ToString().Trim();

                    ss1_Sheet1.Cells[i, 9].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                    ss1_Sheet1.Rows[i].Height = ss1_Sheet1.Rows[i].GetPreferredHeight() + 5;
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        //One_Data_Read_Select_CNT

        long One_Data_Read_Select_CNT(string ArgstrWRTNO)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            long rtnVal = 0;

            try
            {
                SQL = " SELECT TITLE,READDATE,ENTSABUN,decode(DOWNLOAD,'1','다운',DOWNLOAD) download ";
                SQL += ComNum.VBLF + " FROM KOSMOS_OCS.OCS_INFECT_MSG_LIST ";
                SQL += ComNum.VBLF + "  WHERE strWRTNO ='" + ArgstrWRTNO + "' ";
                SQL += ComNum.VBLF + " ORDER BY SDATE,READDATE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
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


                rtnVal = dt.Rows.Count;

                dt.Dispose();
                dt = null;

                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnTool0_Click(object sender, EventArgs e)
        {
            SelectWord();

            switch (VB.Right(((Button)sender).Name, 1))
            {
                case "0":
                    txtRemark2.SelectionFont = new Font(txtRemark2.Font, txtRemark2.SelectionFont.Italic ? FontStyle.Regular : FontStyle.Italic);
                    break;
                case "1":
                    txtRemark2.SelectionFont = new Font(txtRemark2.Font, txtRemark2.SelectionFont.Bold ? FontStyle.Regular : FontStyle.Bold);
                    break;
                case "2":
                    txtRemark2.SelectionFont = new Font(txtRemark2.Font, txtRemark2.SelectionFont.Strikeout ? FontStyle.Regular : FontStyle.Strikeout);
                    break;
                case "3":
                    txtRemark2.SelectionFont = new Font(txtRemark2.Font, txtRemark2.SelectionFont.Underline ? FontStyle.Regular : FontStyle.Underline);
                    break;
            }

            txtRemark2.Focus();
        }

        private void lblColor0_Click(object sender, EventArgs e)
        {
            SelectWord();

            txtRemark2.SelectionColor = ((Label)sender).BackColor;
            txtRemark2.Focus();
        }

        void SelectWord()
        {
            if (txtRemark2.SelectionLength > 0) return;

            //TODO : Span 함수 ??
            //TxtRemark2.Span " ,;:.?!", False, True
            //intWordStart = TxtRemark2.SelStart

            //TxtRemark2.Span " ,;:.?!", True, True
            //intWordStop = TxtRemark2.SelStart + TxtRemark2.SelLength

            //TxtRemark2.SelStart = intWordStart
            //TxtRemark2.SelLength = intWordStop - intWordStart
        }

        private void cboSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectWord();

            txtRemark2.SelectionFont = new Font(txtRemark2.SelectionFont.FontFamily, Convert.ToSingle(cboSize.Text), txtRemark2.SelectionFont.Style);
            txtRemark2.Focus();
        }

        void Screen_Clear()
        {
            FstrROWID = string.Empty;
            FstrFileName = string.Empty;
            FstrWRTNO = string.Empty;

            txtTitle.Text = string.Empty;
            txtInfo.Text = string.Empty;
            dtpBDate.Checked = false;

            //txtRemark.Text = string.Empty;
            txtRemark2.Text = string.Empty;

            txtFileName.Text = string.Empty;

            rdoView0.Checked = true;
            chkPrint.Checked = false;

            btnDelete.Enabled = false;
            btnDelete2.Enabled = false;
            btnSave.Enabled = false;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            Screen_Clear();

            btnSave.Enabled = true;

            txtTitle.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            Save_Data();

            GetSearchData();

            Screen_Clear();
        }

        bool Save_Data()
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            string strSDate = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).ToShortDateString();
            string strTitle = txtTitle.Text.Trim().Replace("'", "`");
            string strRemark = txtRemark2.Rtf;

            long nWRTNO = 0;
            string strWRTNO = string.Empty;

            string strBold = chkBold.Checked ? "1" : "0";
            int intFColor = txtFColor.ForeColor.ToArgb();

            string strView = "0";

            if (rdoView1.Checked)
            {
                strView = "1";
            }
            else if (rdoView2.Checked)
            {
                strView = "2";
            }


            string strPrint = chkPrint.Checked ? "N" : "Y";

            string strTinfo = txtInfo.Text.Trim();
            string strTBDate = dtpBDate.Checked ? dtpBDate.Text.Trim() : string.Empty;

            string strTime = chkTime.Checked ? "1" : string.Empty;


            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (string.IsNullOrEmpty(FstrROWID))
                {
                    nWRTNO = READ_KMS_EDMS_NO(); //등록할 신규 문서번호를 부여함

                    strWRTNO = VB.Left(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), 4) + nWRTNO.ToString("000000");

                    SQL = " INSERT INTO KOSMOS_OCS.OCS_INFECT_MSG (";
                    SQL = SQL + "\r" + " STRWRTNO, SDATE, EDATE, TITLE,";
                    SQL = SQL + "\r" + " REMARK2, ENTSABUN, Bold, ForeColor,";
                    SQL = SQL + "\r" + " GUBUN, View1, GbPrint, TITLE_INFO, ";
                    SQL = SQL + "\r" + " TITLE_INFO_TERM, TIME_TERM ) VALUES ( ";
                    SQL = SQL + "\r" + " '" + strWRTNO + "',TO_DATE('" + strSDate + "','YYYY-MM-DD'),TO_DATE('2999-12-31','YYYY-MM-DD'), '" + strTitle + "',";
                    SQL = SQL + "\r" + " :Remark2, " + clsType.User.Sabun + ",'" + strBold + "','" + intFColor + "',";
                    SQL = SQL + "\r" + " '0','" + strView + "','" + strPrint + "','" + strTinfo + "',TO_DATE('" + strTBDate + "','YYYY-MM-DD'),'" + strTime + "'  )  ";

                }
                else
                {
                    //'기존번호
                    strWRTNO = FstrWRTNO;

                    SQL = " UPDATE KOSMOS_OCS.OCS_INFECT_MSG SET ";
                    SQL += ComNum.VBLF + " Title ='" + strTitle + "', ";

                    SQL += ComNum.VBLF + " Bold ='" + strBold + "', ";
                    SQL += ComNum.VBLF + " ForeColor ='" + intFColor + "', ";

                    SQL += ComNum.VBLF + " View1 ='" + strView + "', ";
                    SQL += ComNum.VBLF + " GbPrint ='" + strPrint + "', ";

                    SQL += ComNum.VBLF + " TITLE_INFO ='" + strTinfo + "', ";
                    SQL += ComNum.VBLF + " TITLE_INFO_TERM =TO_DATE('" + strTBDate + "','YYYY-MM-DD'),  ";
                    SQL += ComNum.VBLF + " TIME_TERM = '" + strTime + "', ";
                    SQL += ComNum.VBLF + " Remark2 = :Remark2";
                    SQL += ComNum.VBLF + " WHERE ROWID ='" + FstrROWID + "' ";
                }

                SqlErr = clsDB.ExecuteLongQuery(SQL, strRemark, ref intRowAffected, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = " SELECT ROWID FROM KOSMOS_OCS.OCS_INFECT_MSG WHERE strWRTNO = '" + strWRTNO + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    FstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                //첨부파일
                menuSave_AppendFile(strWRTNO);


                //최대 5M으로 제한
                if (Encoding.Default.GetByteCount(txtRemark2.SelectedRtf) <= 4999999) //5000000
                {
                    SQL = " UPDATE KOSMOS_OCS.OCS_INFECT_MSG SET REMARK2 = :REMARK2 ";
                    SQL += ComNum.VBLF + " WHERE strWRTNO = '" + strWRTNO + "'";

                    SqlErr = clsDB.ExecuteLongQuery(SQL, strRemark, ref intRowAffected, clsDB.DbCon);
                    if (string.IsNullOrEmpty(SqlErr) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }
                else
                {
                    ComFunc.MsgBox("저장할 내용이 너무커서 저장이 불가능 합니다." + ComNum.VBLF + "내용을 축소 조정후 다시 저장을 하십시오");
                }


                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        long READ_KMS_EDMS_NO()
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            long rtnVal = -1;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "SELECT KOSMOS_PMPA.SEQ_EDMSNO.NEXTVAL NextNo FROM DUAL";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
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

                rtnVal = Convert.ToInt64(dt.Rows[0]["NextNo"].ToString());

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        bool menuSave_AppendFile(string strWRTNO)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;
            bool rtnVal = false;

            string strLocalDir = txtFileName.Text.Trim();
            string strLocalFile = string.Empty;
            string strLocalType = string.Empty;
            string strServerName = "/data/EDMS_DATA/INFECT/F_" + strWRTNO;

            long nFileLen = 0;

            //첨부파일이 없으면
            if (string.IsNullOrEmpty(strLocalDir) && string.IsNullOrEmpty(FstrFileName))
            {
                return rtnVal;
            }

            //첨부파일 이름이 동일하면 전송 안함
            if (FstrFileName == strLocalDir)
            {
                return rtnVal;
            }

            Cursor.Current = Cursors.WaitCursor;
            //clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                Ftpedt ftp = new Ftpedt();

                //파일이름만 분리
                if (string.IsNullOrEmpty(strLocalDir) == false)
                {
                    strLocalFile = strLocalDir.Substring(strLocalDir.LastIndexOf("\\") + 1);
                    strLocalType = strLocalFile.Substring(strLocalFile.LastIndexOf(".") + 1);
                    nFileLen = new System.IO.FileInfo(strLocalDir).Length;
                    if (nFileLen != 0)
                    {
                        nFileLen = nFileLen / 1024;
                    }
                }

                if (FstrFileName != txtFileName.Text.Trim())
                {
                    //첨부파일을 삭제함
                    if (string.IsNullOrEmpty(txtFileName.Text.Trim()))
                    {
                        if (string.IsNullOrEmpty(FstrFileName.Trim()) == false)
                        {
                            if (ftp.FtpDeleteFile("192.168.100.31", "oracle", ftp.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle"), "F_" + strWRTNO, "/data/EDMS_DATA/INFECT") == true)
                            {
                                SQL = "UPDATE KOSMOS_OCS.OCS_INFECT_MSG Set FileName = '',";
                                SQL += ComNum.VBLF + " FileSize = 0, FileType = '' ";
                                SQL += ComNum.VBLF + " WHERE ROWID='" + FstrROWID + "' ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (string.IsNullOrEmpty(SqlErr) == false)
                                {
                                    ftp = null;
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }
                            }
                        }
                    }
                    else
                    {
                        //FtpedtX.FtpUploadBatch(@"C:\PSMHEXE\exenet\edtFTPnet.dll",  "edtFTPnet.dll", "/psnfs/psmhexe/exenet"); //파일업로드
                        if (ftp.FtpUpload("192.168.100.31", "oracle", ftp.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle"), txtFileName.Text, "F_" + strWRTNO, "/data/EDMS_DATA/INFECT") == true)
                        {
                            SQL = "UPDATE KOSMOS_OCS.OCS_INFECT_MSG Set FileName = '" + strLocalFile + "',";
                            SQL += ComNum.VBLF + " FileSize = " + nFileLen + ",";
                            SQL += ComNum.VBLF + " FileType = '" + strLocalType + "' ";
                            SQL += ComNum.VBLF + " WHERE ROWID = '" + FstrROWID + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (string.IsNullOrEmpty(SqlErr) == false)
                            {
                                ftp = null;
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                    }
                }

                //clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                ftp = null;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        string READ_FTP(string strIP, string strUSER)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            string rtnVal = string.Empty;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL += ComNum.VBLF + "SELECT USERPASS FROM KOSMOS_PMPA.BAS_ACCOUNT_SERVER ";
                SQL += ComNum.VBLF + " WHERE IP='" + strIP + "' ";
                SQL += ComNum.VBLF + "   AND USERID='" + strUSER + "' ";
                SQL += ComNum.VBLF + "  ORDER BY SDATE DESC ";
                SQL += ComNum.VBLF + " ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
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

                rtnVal = clsAES.DeAES(dt.Rows[0]["USERPASS"].ToString().Trim());

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            if (string.IsNullOrEmpty(FstrROWID))
                return;

            Update_Data();
            GetSearchData();
            Screen_Clear();
        }

        bool Update_Data()
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "UPDATE KOSMOS_OCS.OCS_INFECT_MSG Set EDate=TRUNC(SYSDATE) ";
                SQL += ComNum.VBLF + "WHERE ROWID='" + FstrROWID + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        bool Delete_Data()
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = " DELETE FROM KOSMOS_OCS.OCS_INFECT_MSG ";
                SQL += ComNum.VBLF + "WHERE ROWID='" + FstrROWID + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            if (string.IsNullOrEmpty(txtTitle.Text.Trim()))
            {
                return;
            }

            ss_Prt_Sheet1.Cells[3, 1].Text = "제목 : " + txtTitle.Text;
            ss_Prt_Sheet1.Cells[3, 3].Text = "등록일자 : " + FstrDate;

            ss_Prt_Sheet1.Cells[4, 1].Text = "첨부 : " + txtFileName.Text;
            ss_Prt_Sheet1.Cells[7, 1].Text = txtRemark2.Text;

            ss_Prt_Sheet1.PrintInfo.Margin.Left = 120;
            ss_Prt_Sheet1.PrintInfo.Margin.Top = 70;
            ss_Prt_Sheet1.PrintInfo.ShowRowHeader = PrintHeader.Hide;
            ss_Prt_Sheet1.PrintInfo.ShowColumnHeader = PrintHeader.Hide;
            ss_Prt_Sheet1.PrintInfo.ShowBorder = false;
            ss_Prt_Sheet1.PrintInfo.ShowColor = true;
            ss_Prt_Sheet1.PrintInfo.ShowGrid = false;
            ss_Prt_Sheet1.PrintInfo.ShowShadows = false;
            ss_Prt_Sheet1.PrintInfo.UseMax = false;
            ss_Prt_Sheet1.PrintInfo.PrintType = PrintType.All;
            ss_Prt_Sheet1.PrintInfo.Preview = false;
            ss_Prt.PrintSheet(0);
        }

        private void btnPrintImage_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            txtRemark3.Clear();

            txtRemark3.SelectedRtf = txtRemark5.Rtf;
            txtRemark3.SelectedText = "------------------------------------------------------------------------------------------------------------------\r";
            txtRemark3.SelectedText = "제목 : " + txtTitle.Text.Trim() + ComNum.VBLF;
            txtRemark3.SelectedText = "등록일자 : " + FstrDate + ComNum.VBLF;
            txtRemark3.SelectedText = "------------------------------------------------------------------------------------------------------------------\r";
            txtRemark3.SelectedRtf = txtRemark2.Rtf;

            clsRichTextBoxPrint cRchPrint = new clsRichTextBoxPrint(txtRemark3);
            cRchPrint.PrintRTF();
            cRchPrint = null;
        }

        private void btnDelete2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
                return; //권한 확인

            if (string.IsNullOrEmpty(FstrROWID))
                return;

            if (ComFunc.MsgBoxQ("정말로 선택한 자료를 삭제처리하시겠습니까??") == DialogResult.No)
                return;


            Delete_Data();
            GetSearchData();
            Screen_Clear();
        }

        private void btnFColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog ColorDg = new ColorDialog())
            {
                if (ColorDg.ShowDialog() == DialogResult.Cancel)
                    return;

                txtFColor.ForeColor = ColorDg.Color;
            }
        }

        private void btnSearchFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog OpenFile = new OpenFileDialog())
            {
                OpenFile.Title = "열기";
                OpenFile.Filter = "모든 파일 (*.*)|*.*|텍스트 문서(*.TXT)|*.TXT|HTML 문서(*.HTM)|*.HTM";
                OpenFile.ReadOnlyChecked = true;
                OpenFile.ShowDialog();

                txtFileName.Text = OpenFile.FileName;
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            //첨부팡링 ㅣ없으면
            if (string.IsNullOrEmpty(FstrFileName))
                return;

            string strFileName = string.Empty;

            using (SaveFileDialog SaveFile = new SaveFileDialog())
            {
                SaveFile.Title = "다른 이름으로 저장";
                SaveFile.Filter = "모든 파일 (*.*)|*.*|텍스트 문서(*.TXT)|*.TXT|HTML 문서(*.HTM)|*.HTM";
                SaveFile.FileName = FstrFileName;
                SaveFile.ShowDialog();

                strFileName = SaveFile.FileName;
            }

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

            Ftpedt ftp = new Ftpedt();
            if (ftp.FtpDownload("192.168.100.31", "oracle", ftp.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle"), strFileName, "F_" + FstrWRTNO, "/data/EDMS_DATA/INFECT") == false)
            {
                ComFunc.MsgBox(strFileName + "다운로드 실패");
            } 
            else
            {
                ComFunc.MsgBox(strFileName + "다운로드 완료");
            }
            ftp = null;

            return;
        }

        private void ss1_CellDoubleClick(object sender, CellClickEventArgs e)
        {

            FstrROWID = string.Empty;

            FstrDate = ss1_Sheet1.Cells[e.Row, 0].Text.Trim();
            FstrFileName = ss1_Sheet1.Cells[e.Row, 5].Text.Trim();
            FstrWRTNO = ss1_Sheet1.Cells[e.Row, 7].Text.Trim();
            FstrROWID = ss1_Sheet1.Cells[e.Row, 9].Text.Trim();

            btnDelete.Enabled = true;
            btnDelete2.Enabled = true;
            btnSave.Enabled = true;

            txtFileName.Text = FstrFileName;

            One_Data_Select(FstrROWID);

            One_Data_Read_Select(FstrWRTNO);
        }

        private void ss2_CellDoubleClick(object sender, CellClickEventArgs e)
        {

        }


        void One_Data_Select(string strRowid)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            txtTitle.Clear();
            txtRemark2.Clear();
            chkBold.Checked = false;
            chkTime.Checked = false;

            txtInfo.Clear();
            dtpBDate.Checked = false;

            rdoView0.Checked = true;
            chkPrint.Checked = false;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT strWRTNO,SDATE,EDATE,TITLE,REMARK,REMARK2,ENTSABUN,FILENAME,FILESIZE,FILETYPE,";
                SQL += ComNum.VBLF + " Bold,ForeColor,View1,GbPrint,TITLE_INFO, TITLE_INFO_TERM,ROWID, TIME_TERM ";
                SQL += ComNum.VBLF + " FROM KOSMOS_OCS.OCS_INFECT_MSG";
                SQL += ComNum.VBLF + "  WHERE ROWID ='" + strRowid + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
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

                txtTitle.Text = dt.Rows[0]["Title"].ToString().Trim();

                lblPTitle.Text = txtTitle.Text;


                if (dt.Rows[0]["Bold"].ToString().Trim() == "1")
                {
                    txtFColor.Font = new Font("맑은 고딕", 9.75f, FontStyle.Bold);
                    txtFColor.Font = new Font("맑은 고딕", 9.75f, FontStyle.Bold);
                    chkBold.Checked = true;
                }
                else
                {
                    txtFColor.Font = new Font("맑은 고딕", 9.75f, FontStyle.Regular);
                    txtFColor.Font = new Font("맑은 고딕", 9.75f, FontStyle.Regular);
                }




                txtTitle.ForeColor = string.IsNullOrEmpty(dt.Rows[0]["ForeColor"].ToString().Trim()) ? Color.FromArgb(-2147483640) : Color.FromArgb(Convert.ToInt32(dt.Rows[0]["ForeColor"].ToString().Trim()));
                txtFColor.ForeColor = string.IsNullOrEmpty(dt.Rows[0]["ForeColor"].ToString().Trim()) ? Color.FromArgb(-2147483640) : Color.FromArgb(Convert.ToInt32(dt.Rows[0]["ForeColor"].ToString().Trim()));

                chkTime.Checked = dt.Rows[0]["TIME_TERM"].ToString().Trim().Equals("1");


                txtRemark2.SelectedRtf = dt.Rows[0]["Remark2"].ToString().Trim().Replace("`", "'");

                switch (dt.Rows[0]["View1"].ToString().Trim())
                {
                    case "0":
                        rdoView0.Checked = true;
                        break;
                    case "1":
                        rdoView1.Checked = true;
                        break;
                    case "2":
                        rdoView2.Checked = true;
                        break;
                }


                chkPrint.Checked = dt.Rows[0]["GbPrint"].ToString().Trim().Equals("N");

                txtInfo.Text = dt.Rows[0]["title_info"].ToString().Trim();
                dtpBDate.Value = string.IsNullOrEmpty(dt.Rows[0]["TITLE_INFO_TERM"].ToString().Trim()) == false ? Convert.ToDateTime(dt.Rows[0]["TITLE_INFO_TERM"].ToString().Trim()) : DateTime.Now;
                dtpBDate.Checked = string.IsNullOrEmpty(dt.Rows[0]["TITLE_INFO_TERM"].ToString().Trim()) == false;


                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        void One_Data_Read_Select(string strWRTNO)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            ss2_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {

                SQL = " SELECT TITLE,TO_CHAR(READDATE,'YYYY-MM-DD') READDATE, ENTSABUN,decode(DOWNLOAD,'1','다운',DOWNLOAD) download ";
                SQL += ComNum.VBLF + " FROM KOSMOS_OCS.OCS_INFECT_MSG_LIST ";
                SQL += ComNum.VBLF + "  WHERE strWRTNO ='" + strWRTNO + "' ";
                SQL += ComNum.VBLF + " ORDER BY SDATE,READDATE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
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


                ss2_Sheet1.RowCount = dt.Rows.Count;
                ss2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ss2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Title"].ToString().Trim();
                    ss2_Sheet1.Cells[i, 1].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["ENTSABUN"].ToString().Trim());
                    ss2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ReadDate"].ToString().Trim();
                    ss2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DOWNLOAD"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnPrint2_Click(object sender, EventArgs e)
        {
            if (ss1_Sheet1.NonEmptyRowCount == 0) return;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            Set_Print();
        }

        void Set_Print()
        {
            string strTitle = "감염공지 확인명단";
            string strHeader = string.Empty;
            string strFooter = string.Empty;
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            btnPrint.Enabled = false;


            strHeader = CS.setSpdPrint_String(strTitle, new Font("바탕체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("공지사항: " + lblPTitle.Text, new Font("바탕체", 11.25f), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("등록일자: " + ComQuery.CurrentDateTime(clsDB.DbCon, "S"), new Font("바탕체", 11.25f), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 200, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, true, false);

            CS.setSpdPrint(ss1, PrePrint, setMargin, setOption, strHeader, strFooter);
            CS = null;

            btnPrint.Enabled = true;
        }

        private void btnSave3_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            Save_Data2();

            GetSearchData();

            Screen_Clear();
        }

        bool Save_Data2()
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            string strSDate = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).ToShortDateString();
            string strTitle = txtTitle.Text.Trim().Replace("'", "`");
            string strRemark = txtRemark.Text.Trim().Replace("'", "`");

            long nWRTNO = 0;
            string strWRTNO = string.Empty;

            string strBold = chkBold.Checked ? "1" : "0";
            int intFColor = txtFColor.ForeColor.ToArgb();

            string strPrint = chkPrint.Checked ? "N" : "Y";

            string strTinfo = txtInfo.Text.Trim();
            string strTBDate = dtpBDate.Text.Trim();

            string strTime = chkTime.Checked ? "1" : string.Empty;


            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (string.IsNullOrEmpty(FstrROWID))
                {
                    nWRTNO = READ_KMS_EDMS_NO(); //등록할 신규 문서번호를 부여함

                    strWRTNO = VB.Left(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), 4) + nWRTNO.ToString("000000");

                    SQL = " INSERT INTO KOSMOS_OCS.OCS_INFECT_MSG (STRWRTNO, SDATE,EDATE,TITLE,REMARK,ENTSABUN,Bold,ForeColor,GUBUN ) VALUES ( ";
                    SQL = SQL + " '" + strWRTNO + "',TO_DATE('" + strSDate + "','YYYY-MM-DD'), TO_DATE('2999-12-31','YYYY-MM-DD'), ";
                    SQL = SQL + " '" + strTitle + "','" + strRemark + "'," + clsType.User.Sabun + ",'" + strBold + "','" + intFColor + "','0' )  ";

                }
                else
                {
                    //'기존번호
                    strWRTNO = FstrWRTNO;

                    SQL = " UPDATE KOSMOS_OCS.OCS_INFECT_MSG SET ";
                    SQL += ComNum.VBLF + " Title ='" + strTitle + "', ";

                    SQL += ComNum.VBLF + " Bold ='" + strBold + "', ";
                    SQL += ComNum.VBLF + " ForeColor ='" + intFColor + "', ";

                    SQL += ComNum.VBLF + " Remark = '" + strRemark + "'";
                    SQL += ComNum.VBLF + " WHERE ROWID ='" + FstrROWID + "' ";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = " SELECT ROWID FROM KOSMOS_OCS.OCS_INFECT_MSG WHERE strWRTNO = '" + strWRTNO + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    FstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                //첨부파일
                menuSave_AppendFile(strWRTNO);

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

    }
}
