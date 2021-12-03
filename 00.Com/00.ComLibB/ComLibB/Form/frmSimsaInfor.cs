using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스
using System.Drawing.Printing;
using System.Drawing;
using FarPoint.Win.Spread;

namespace ComLibB
{
    public partial class frmSimsaInfor : Form
    {
        private frmSpecialText frmSpecialTextX = null;

        private string GstrHelpCode = "";
        string FGstrRowid;

        private string GstrLabelPrint1 = "";
        private string GstrLabelPrint2 = "";
        private string GstrLabelPrint3 = "";
        private string GstrLabelPrint4 = "";
        private string GstrLabelPrint5 = "";
        private string GstrLabelPrint6 = "";
        private string GstrLabelPrint7 = "";
        private string GstrLabelPrint8 = "";
        private string GstrLabelPrint9 = "";
        private string GstrLabelPrint10 = "";


        public frmSimsaInfor()
        {
            InitializeComponent();
        }

        public frmSimsaInfor(string GstrHelpCode)
        {
            InitializeComponent();
            this.GstrHelpCode = GstrHelpCode;
        }

        private void frmSimsaInfor_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            Set_Init();
            SCREEN_CLEAR();

            if(GstrHelpCode != "")
            {
                txtSuNext.Text = GstrHelpCode;
                GetData();
            }            
        }

        private void Set_Init()
        {
            listBun.Items.Clear();
            listBun.Items.Add("01.기본진료");
            listBun.Items.Add("02.검사료");
            listBun.Items.Add("03.영상진단,방서선치료");
            listBun.Items.Add("04.마취");
            listBun.Items.Add("05.이학요법");
            listBun.Items.Add("06.정신요법");
            listBun.Items.Add("07.수술");
            listBun.Items.Add("08.수술재료,");
            listBun.Items.Add("09.처치,처치재료");
            listBun.Items.Add("10.주사제");
            listBun.Items.Add("11.내복약");
            listBun.Items.Add("12.외용");
            listBun.Items.Add("13.기타");
            

            cboJong.Items.Clear();
            cboJong.Items.Add("01.기본진료");
            cboJong.Items.Add("02.검사료");
            cboJong.Items.Add("03.영상진단,방서선치료");
            cboJong.Items.Add("04.마취");
            cboJong.Items.Add("05.이학요법");
            cboJong.Items.Add("06.정신요법");
            cboJong.Items.Add("07.수술");
            cboJong.Items.Add("08.수술재료");
            cboJong.Items.Add("09.처치,처치재료");
            cboJong.Items.Add("10.주사제");
            cboJong.Items.Add("11.내복약");
            cboJong.Items.Add("12.외용");
            cboJong.Items.Add("13.기타");
        }

        private void SCREEN_CLEAR()
        {
            txtSuNext.Text = ""   ;
            txtHname.Text = ""    ;
            txtEname.Text = ""    ;
            txtBCode.Text = ""    ;
            txtBAmt.Text = ""     ;
            txtWon.Text = "";
            lblWonName.Text = "";
            txtDelDate.Text = "";
            
            ss1_Sheet1.RowCount = 0;
            
            txtInfo.Text = "";
            FGstrRowid = "";


            txtView.Text = "";


            btnSave.Enabled = false;
            btnDelete.Enabled = false;
            panMain.Enabled = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                
                SQL = "SELECT A.SUNEXT, B.SUNAMEK ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_SIMSAINFOR A, KOSMOS_PMPA.BAS_SUN B";
                SQL = SQL + ComNum.VBLF + " WHERE A.SUNEXT =B.SUNEXT ";
                
                if( OptView_1.Checked == true) SQL = SQL + ComNum.VBLF + " AND A.SUNEXT LIKE '" + "%" + txtView.Text + "%" + "' ";
                if( OptView_2.Checked == true) SQL = SQL + ComNum.VBLF + " AND A.SUNAMEK LIKE '" + "%" + txtView.Text + "%" + "' ";
                if( OptView_3.Checked == true) SQL = SQL + ComNum.VBLF + " AND A.SUNAMEE LIKE '" + "%" + txtView.Text + "%" + "' ";                
                if( OptView_0.Checked == true) SQL = SQL + ComNum.VBLF + " ORDER BY A.SUNEXT ";
                if (OptView_1.Checked == true) SQL = SQL + ComNum.VBLF + " ORDER BY B.SUNAMEK ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                list1.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        list1.Items.Add(VB.Left(dt.Rows[i]["SUNEXT"].ToString().Trim() + VB.Space(8), 8) + " " + dt.Rows[i]["SUNAMEK"].ToString().Trim());
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
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void list1_DoubleClick(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
            txtSuNext.Text = VB.Left(list1.Text, 8);
            GetData();

        }

        private void listBun_DoubleClick(object sender, EventArgs e)
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
                
                SQL = "SELECT A.SUNEXT, B.SUNAMEK ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_SIMSAINFOR A, KOSMOS_PMPA.BAS_SUN B";
                SQL = SQL + ComNum.VBLF + " WHERE A.SUNEXT =B.SUNEXT ";                
                SQL = SQL + ComNum.VBLF + "  AND JONG = '" + VB.Left(listBun.Text, 2) + "' ";                
                if(OptSort_0.Checked == true) SQL = SQL + ComNum.VBLF + " ORDER BY A.SUNEXT ";
                if(OptSort_1.Checked == true) SQL = SQL + ComNum.VBLF + " ORDER BY B.SUNAMEK ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                list1.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        list1.Items.Add(VB.Left(dt.Rows[i]["SUNEXT"].ToString().Trim() + VB.Space(8), 8) + " " + dt.Rows[i]["SUNAMEK"].ToString().Trim());
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
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
            list1.Items.Clear();
            btnSearch.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (clsPublic.GnJobSabun == 16412 || clsPublic.GnJobSabun == 19684) //'원무과 이희목, 박시철
            {
                ComFunc.MsgBox("등록권한이 없습니다.", "확인");
                return;
            }

            if (FGstrRowid == "")
            {
                ComFunc.MsgBox("전산오류입니다.", "확인");
                return;
            }
            
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; ; //권한 확인

                SQL = " DELETE KOSMOS_PMPA.BAS_SIMSAINFOR WHERE ROWID = '" + FGstrRowid + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;

                SCREEN_CLEAR();
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }   
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (clsPublic.GnJobSabun == 16412 || clsPublic.GnJobSabun == 19684) //'원무과 이희목, 박시철
            {
                ComFunc.MsgBox("등록권한이 없습니다.", "확인");
                return;
            }

            SCREEN_CLEAR();

            panMain.Enabled = true;
            txtSuNext.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {            
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (clsType.User.Sabun == "16412" || clsType.User.Sabun == "19684") //'원무과 이희목, 박시철
            {
                ComFunc.MsgBox("등록권한이 없습니다.", "확인");
                return;
            }

            if (cboJong.Text == "")
            {
                ComFunc.MsgBox("심사정보 종류를 선택해주세요.", "확인");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            string strLong = txtInfo.Rtf.Replace("'", "`");

            try
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; ; //권한 확인

                if (FGstrRowid == "")
                {
                    SQL = " INSERT INTO KOSMOS_PMPA.BAS_SIMSAINFOR ( SUNEXT, REMARK, JONG) ";
                    SQL = SQL + ComNum.VBLF + " VALUES (";
                    SQL = SQL + ComNum.VBLF + " '" + txtSuNext.Text + "', ";
                    SQL = SQL + ComNum.VBLF + ":REMARK, ";
                    SQL = SQL + ComNum.VBLF + "'" + VB.Left(cboJong.Text, 2) + "' ) ";

                    SqlErr = clsDB.ExecuteClobQuery(SQL, strLong, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    SQL = " SELECT ROWID FROM KOSMOS_PMPA.BAS_SIMSAINFOR WHERE SUNEXT = '" + txtSuNext.Text + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }
                    FGstrRowid = dt.Rows[0]["ROWID"].ToString().Trim();                        
                }
                else
                {
                    SQL = " UPDATE KOSMOS_PMPA.BAS_SIMSAINFOR SET REMARK = :remark";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FGstrRowid + "' ";
                    SqlErr = clsDB.ExecuteClobQuery(SQL, strLong, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
                
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }            
        }

        private void OptView_1_CheckedChanged(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
        }

        private void txtSuNext_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            if (txtSuNext.Text.Trim() == "") return;

            GetData();
         
        }

        void GetData()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            txtSuNext.Text = txtSuNext.Text.ToUpper();

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                //'수가READ;
                SQL = " SELECT A.SUNAMEK, A.SUNAMEE, A.WONCODE, A.BCODE, B.BAMT, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(B.DELDATE,'YYYY-MM-DD') DELDATE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_SUN A, KOSMOS_PMPA.BAS_SUT B ";
                SQL = SQL + ComNum.VBLF + "   WHERE A.SUNEXT = B.SUNEXT ";
                SQL = SQL + ComNum.VBLF + "   AND A.SUNEXT = '" + txtSuNext.Text + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    txtHname.Text = dt.Rows[0]["SUNAMEK"].ToString().Trim();
                    txtEname.Text = dt.Rows[0]["SUNAMEE"].ToString().Trim();
                    txtBCode.Text = dt.Rows[0]["BCODE"].ToString().Trim();
                    txtBAmt.Text = VB.Val(dt.Rows[0]["BAMT"].ToString().Trim()).ToString("###,###,###,##0");
                    txtWon.Text = dt.Rows[0]["WONCODE"].ToString().Trim();
                    lblWonName.Text = READ_WonName(txtWon.Text);
                    txtDelDate.Text = dt.Rows[0]["DELDATE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                //'구입신고 READ
                SQL = "     SELECT TO_CHAR(A.GDATE,'YYYY-MM-DD') GDATE,A.GUBUN,A.SGBN,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(A.SDATE,'YYYY-MM-DD') SDATE,";
                SQL = SQL + ComNum.VBLF + "      A.QTY,A.AMT,A.PRICE,A.PANO,B.NAME ";
                SQL = SQL + ComNum.VBLF + " FROM EDI_GUIP A,KOSMOS_ADM.AIS_LTD B ";
                SQL = SQL + ComNum.VBLF + "WHERE A.BCODE = '" + VB.Trim(txtBCode.Text) + "' ";
                SQL = SQL + ComNum.VBLF + "  AND A.GELCODE = B.LTDCODE(+) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY A.GDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ss1_Sheet1.RowCount = ss1_Sheet1.RowCount + 1;
                        ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["GDate"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Name"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 2].Text = VB.Val(dt.Rows[i]["Qty"].ToString().Trim()).ToString("###,###,###,##0");
                        ss1_Sheet1.Cells[i, 3].Text = VB.Val(dt.Rows[i]["Amt"].ToString().Trim()).ToString("###,###,###,##0");
                        ss1_Sheet1.Cells[i, 4].Text = VB.Val(dt.Rows[i]["Price"].ToString().Trim()).ToString("###,###,###,##0");
                        ss1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Gubun"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["SGbn"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["SDate"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                //'심사기준 READ
                SQL = " SELECT JONG, REMARK, ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_SIMSAINFOR ";
                SQL = SQL + ComNum.VBLF + " WHERE SUNEXT = '" + txtSuNext.Text + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    FGstrRowid = dt.Rows[0]["ROWID"].ToString().Trim();
                    txtInfo.Rtf = VB.Replace(dt.Rows[0]["REMARK"].ToString().Trim(), "`", "'");
                    cboJong.SelectedIndex = Convert.ToInt32(VB.Val(dt.Rows[0]["JONG"].ToString().Trim()) - 1);
                    btnDelete.Enabled = true;
                }

                dt.Dispose();
                dt = null;

                btnSave.Enabled = true;
                panMain.Enabled = true;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private string READ_WonName(string ArgCode)
        {
            string ArgReturn = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수d

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return ArgReturn; //권한 확인

                if (VB.Trim(ArgCode) == "")
                {
                    return ArgReturn;
                }

                SQL = "SELECT HANGNAME FROM KOSMOS_ADM.WON_HANG ";
                SQL = SQL + ComNum.VBLF + "WHERE HANG='" + VB.Trim(ArgCode) + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return ArgReturn;
                }

                if (dt.Rows.Count > 0)
                {
                    ArgReturn = dt.Rows[0]["HangName"].ToString().Trim();
                }
                else
                {
                    ArgReturn = "** ERROR **";
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
                ComFunc.MsgBox(ex.Message);
            }

            return ArgReturn;
        }

        private void btnSText_Click(object sender, EventArgs e)
        {
            if (frmSpecialTextX != null)
            {
                frmSpecialTextX.Dispose();
                frmSpecialTextX = null;
            }
            frmSpecialTextX = new frmSpecialText();
            frmSpecialTextX.rSendText += new frmSpecialText.SendText(GetText);
            frmSpecialTextX.rEventExit += new frmSpecialText.EventExit(frmSpecialTextX_rEventExit);
            frmSpecialTextX.Show();
        }

        private void frmSpecialTextX_rEventExit()
        {
            frmSpecialTextX.Dispose();
            frmSpecialTextX = null;
        }

        private void GetText(string strText) 
        {
            //txtInfo.Text += strText;
            //커서 도중 문자 넣기
            string insertText = strText;
            int selectionIndex = txtInfo.SelectionStart;
            txtInfo.Text = txtInfo.Text.Insert(selectionIndex, insertText);
            txtInfo.SelectionStart = selectionIndex + insertText.Length;
        }

        private void btnFont_Click(object sender, EventArgs e)
        {
            using (FontDialog fontDialog = new FontDialog())
            {
                fontDialog.ShowColor = true;
                if (fontDialog.ShowDialog() == DialogResult.Cancel)
                    return;

                txtInfo.SelectionFont = fontDialog.Font;
                txtInfo.SelectionColor = fontDialog.Color;
            }
            txtInfo.Focus();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //인쇄기능

            //GstrLabelPrint1 = txtHname.Text + "(" + txtSuNext.Text.Trim() + ")" + ComNum.VBLF;
            //GstrLabelPrint2 = "포항성모병원 보험심사팀 -> 출력일자 : " + DateTime.Now.ToString("yyyy-MM-dd") + "  보험관련" + ComNum.VBLF;
            //GstrLabelPrint2 += "==============================================================" + ComNum.VBLF;
            //GstrLabelPrint2 += "표준코드 : " + txtBCode.Text + VB.Space(20) + "보험약가 : " + txtBAmt.Text + "원" + ComNum.VBLF;

            //PrintDocument PD = new PrintDocument();
            //PD.PrintPage += new PrintPageEventHandler(Document_PrintPage);
            //PD.Print();


            //----------------------------------------------------------------------------------------------
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strSubTitle1 = "";
            string strSubTitle2 = "";
            string strSubTitle3 = "";

            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            ssList.ActiveSheet.Cells[0, 0].Text = txtInfo.Text;

            strTitle = txtHname.Text + "(" + txtSuNext.Text.Trim() + ")";
            strSubTitle1 = "포항성모병원 보험심사팀 -> 출력일자 : " + DateTime.Now.ToString("yyyy-MM-dd");
            strSubTitle2 = "===========================================================================================";
            strSubTitle3 = "표준코드 : " + txtBCode.Text.Trim() + VB.Space(20) + "보험약가 : " + txtBAmt.Text + "원";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle1, new Font("굴림체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle2, new Font("굴림체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle3, new Font("굴림체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += SPR.setSpdPrint_String("\r\n\r\n\r\n", new Font("굴림체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Left, false, true);
            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 100, 0, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, false, false, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);
            
        }

        //void Document_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        //{
        //    int itar = 0;

        //    e.Graphics.DrawString(GstrLabelPrint1, new Font("맑은 고딕", 15f, FontStyle.Bold), Brushes.Black, 7, 5, new StringFormat());
        //    e.Graphics.DrawString(GstrLabelPrint2, new Font("맑은 고딕", 10f, FontStyle.Bold), Brushes.Black, 7, 25, new StringFormat());
        //    e.Graphics.DrawString(this.txtInfo.Text, new Font("맑은 고딕", 10f, FontStyle.Bold), Brushes.Black, 7, 81, new StringFormat());

        //    foreach (char c in this.txtInfo.Text)
        //    {
        //        if (c != '\n')
        //        {
        //            this.txtInfo.Select(itar, 1);
        //            Point point = this.txtInfo.GetPositionFromCharIndex(itar);
        //            e.Graphics.DrawString(c.ToString(), this.txtInfo.SelectionFont, new SolidBrush(this.txtInfo.SelectionColor), point);
        //        }
        //        itar++;
        //    }
        //}
    }
}
