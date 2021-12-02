using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComLibB
{
    /// <summary> 기관조합, 거래처 </summary>
    public partial class frmBasMia : Form
    {
        string strClass = string.Empty;

        /// <summary> 기관조합, 거래처 </summary>
        public frmBasMia()
        {
            InitializeComponent();
        }

        void frmBasMia_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            MiaClass();
            DetailClass();
            DetailClass1();
            GbCity();
            Screen_Clear();
            ssView_Sheet1.Columns[5].Visible = false;   //ROWID
        }

        void Display()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            int i = 0;
            int nCount1 = 0;
            string strCombo1 = string.Empty;
            string strJuso = string.Empty;

            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SELECT MiaCode, MiaClass, MiaName, MiaTel, MiaDetail,");
            sb.AppendLine("       MiaMisu, MiaJuso, TO_CHAR(DelDate, 'yyyy-MM-dd') DelDate,GbCity ");
            sb.AppendLine("  FROM BAS_MIA");
            sb.AppendLine(" WHERE MiaCode = '" + txtMiaCode.Text + "' ");

            SQL = sb.ToString();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }
            else
            {
                btnSave.Enabled = true;
            }

            txtDelDate.Text = dt.Rows[0]["DELDATE"].ToString().Trim();  //삭제일자
            txtMiaName.Text = dt.Rows[0]["MIANAME"].ToString().Trim();  //기관조합 (거래처)명
            txtTelName.Text = dt.Rows[0]["MIATEL"].ToString().Trim();   //조합 전화번호
            strCombo1 = dt.Rows[0]["MIACLASS"].ToString().Trim();       //조합구분

            nCount1 = cboMiaClass.Items.Count;
            for (i = 0; i < nCount1; i++)
            {
                strClass = VB.Left(cboMiaClass.Items[i].ToString(), 2);
                if (strCombo1.Equals(strClass))
                {
                    cboMiaClass.SelectedIndex = i;
                    break;
                }
            }

            strCombo1 = dt.Rows[0]["MIADETAIL"].ToString().Trim();  //상세분류
            nCount1 = cboDetailClass.Items.Count;
            for (i = 0; i < nCount1; i++)
            {
                strClass = VB.Left(cboDetailClass.Items[i].ToString(), 2);
                if (strCombo1.Equals(strClass))
                {
                    cboDetailClass.SelectedIndex = i;
                    break;
                }
            }

            strCombo1 = dt.Rows[0]["GBCITY"].ToString().Trim();  //시구지역구분
            nCount1 = cboGbCity.Items.Count;
            for (i = 0; i < nCount1; i++)
            {
                strClass = VB.Left(cboGbCity.Items[i].ToString(), 2);
                if (strCombo1.Equals(strClass))
                {
                    cboGbCity.SelectedIndex = i;
                    break;
                }
            }

            txtMailCode.Text = VB.Left(dt.Rows[0]["MIAJUSO"].ToString().Trim(), 3); //우편번호
            txtMailCode.Text += "-" + VB.Mid(dt.Rows[0]["MIAJUSO"].ToString().Trim(), 4, 3);

            txtMiaJuso.Text = VB.Mid(dt.Rows[0]["MIAJUSO"].ToString().Trim(), 7, 13);
            txtMiaJuso1.Text = VB.Mid(dt.Rows[0]["MIAJUSO"].ToString().Trim(), 21, 40);

            dt.Dispose();
            dt = null;

            Cursor.Current = Cursors.Default;
        }

        void MiaClass()
        {
            cboMiaClass.Items.Clear();
            cboMiaClass.Items.Add("11.공단");
            cboMiaClass.Items.Add("12.공단");
            cboMiaClass.Items.Add("13.지역");
            cboMiaClass.Items.Add("20.보호");
            cboMiaClass.Items.Add("90.거래처");
        }

        void DetailClass()
        {
            cboDetailClass.Items.Clear();
            cboDetailClass.Items.Add("01.각지구구조합");
            cboDetailClass.Items.Add("02.대기업조합");
            cboDetailClass.Items.Add("11.초등학교");
            cboDetailClass.Items.Add("12.중학교");
            cboDetailClass.Items.Add("13.고등학교");
            cboDetailClass.Items.Add("14.전문대, 대학");
            cboDetailClass.Items.Add("15.특수학교");
            cboDetailClass.Items.Add("21.군.공무원");
            cboDetailClass.Items.Add("31.경찰.소방");
            cboDetailClass.Items.Add("32.시,군,구청");
            cboDetailClass.Items.Add("33.법무공무원");
            cboDetailClass.Items.Add("34.교육공무원");
            cboDetailClass.Items.Add("35.전화우체국");
            cboDetailClass.Items.Add("36.철도,원호,세무");
            cboDetailClass.Items.Add("41.기타");
            cboDetailClass.Items.Add("71.서울");
            cboDetailClass.Items.Add("72.부산");
            cboDetailClass.Items.Add("73.대구");
            cboDetailClass.Items.Add("74.인천");
            cboDetailClass.Items.Add("75.광주");
            cboDetailClass.Items.Add("76.대전");
            cboDetailClass.Items.Add("77.울산");
            cboDetailClass.Items.Add("81.경기");
            cboDetailClass.Items.Add("82.강원");
            cboDetailClass.Items.Add("83.충북");
            cboDetailClass.Items.Add("84.충남");
            cboDetailClass.Items.Add("85.경북");
        }

        void DetailClass1()
        {
            int n = 0;
            int i = 0;

            cboDetailClass1.Items.Clear();

            n = cboDetailClass.Items.Count;
            for (i = 0; i < n; i++)
            {
                cboDetailClass1.Items.Add(cboDetailClass.Items[i].ToString());
            }
        }

        void GbCity()
        {
            cboGbCity.Items.Clear();
            cboGbCity.Items.Add("1.시,구지역");
            cboGbCity.Items.Add("0.기타");
        }

        void Screen_Clear()
        {
            txtMiaCode.Text = "";
            txtDelDate.Text = "";
            cboMiaClass.Text = "";
            cboDetailClass.Text = "";
            txtMiaName.Text = "";
            txtTelName.Text = "";
            cboGbCity.Text = "";
            txtMailCode.Text = "";
            txtMiaJuso.Text = "";
            cboDetailClass.Text = "";
            txtMiaJuso.Text = "";
            txtMiaJuso1.Text = "";

            ssView_Sheet1.RowCount = 0;

            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnDelete.Enabled = false;
            btnSearch.Enabled = false;
            btnPrint.Enabled = false;
            btnShow.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Screen_Clear();
            txtMiaCode.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            if (ComFunc.MsgBoxQ("정말 삭제하시겠습니까?") == DialogResult.No)
            {
                return;
            }

            string SQL = string.Empty;
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("DELETE BAS_MIA");
            sb.AppendLine(" WHERE Miacode = '" + txtMiaCode.Text.Trim() + "'");

            SQL = sb.ToString();

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Screen_Clear();
                txtMiaCode.Focus();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void btnMailHelp_Click(object sender, EventArgs e)
        {
            frmMail frmMailX = new frmMail();
            frmMailX.SendEvent += (value) =>
            {
                txtMailCode.Text = VB.Left(value, 3) + "-" + VB.Mid(value, 4, 3);
                txtMiaJuso.Text = VB.Mid(value, 9, 50);
            };
            frmMailX.ShowDialog();
            frmMailX.Dispose();
            frmMailX = null;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strMiaJuso = string.Empty;
            string strDelDate = string.Empty;

            string SQL = string.Empty;
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SELECT MiaCode");
            sb.AppendLine("  FROM BAS_MIA");
            sb.AppendLine(" WHERE MiaCode = '" + txtMiaCode.Text.Trim() + "'");

            SQL = sb.ToString();

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                strDelDate = txtDelDate.Text.Trim();
                strMiaJuso = VB.Left(txtMailCode.Text, 3) + VB.Right(txtMailCode.Text, 3);
                strMiaJuso += VB.Left(txtMiaJuso1.Text + VB.Space(20), 20);
                strMiaJuso += VB.Left(txtMiaJuso2.Text + VB.Space(40), 40);

                sb.Clear();
                if (dt.Rows.Count == 0)
                {
                    sb.AppendLine("INSERT INTO BAS_MIA");
                    sb.AppendLine(" (");
                    sb.AppendLine("   MiaCode, DelDate, MiaClass, MiaDetail, MiaName,");
                    sb.AppendLine("   MiaTel, GbCity, MiaJuso");
                    sb.AppendLine(" )");
                    sb.AppendLine("VALUES (");
                    sb.AppendLine("   '" + txtMiaCode.Text.Trim() + "',");                          //조합코드
                    sb.AppendLine("    TO_DATE('" + strDelDate.Trim() + "','YYYY-MM-DD'),");        //삭제일자
                    sb.AppendLine("   '" + VB.Left(cboMiaClass.Text.Trim(), 2) + "',");             //조합구분
                    sb.AppendLine("   '" + VB.Left(cboDetailClass.Text.Trim(), 2) + "',");          //상세분류
                    sb.AppendLine("   '" + txtMiaName.Text.Trim() + "',");                          //조합명
                    sb.AppendLine("   '" + txtTelName.Text.Trim() + "',");                          //전화번호
                    sb.AppendLine("   '" + VB.Left(cboGbCity.Text.Trim() + VB.Space(1), 1) + "',"); //시.군지역 구분
                    sb.AppendLine("   '" + VB.RTrim(strMiaJuso) + "'");                             //우편번호및 주소
                    sb.AppendLine(" )");
                }
                else
                {
                    sb.AppendLine("UPDATE BAS_MIA SET");
                    sb.AppendLine("   MiaCode = '" + txtMiaCode.Text.Trim() + "',");                         //조합코드
                    sb.AppendLine("   DelDate = TO_DATE('" + txtDelDate.Text.Trim() + "','YYYY-MM-DD'),");   //삭제일자
                    sb.AppendLine("   MiaClass = '" + VB.Left(cboMiaClass.Text.Trim(), 2) + "',");           //조합구분
                    sb.AppendLine("   MiaDetail = '" + VB.Left(cboDetailClass.Text.Trim(), 2) + "',");       //상세분류
                    sb.AppendLine("   MiaName = '" + txtMiaName.Text.Trim() + "',");                         //조합명
                    sb.AppendLine("   MiaTel = '" + txtTelName.Text.Trim() + "',");                          //전화번호
                    sb.AppendLine("   GbCity = '" + VB.Left(cboGbCity.Text.Trim(), 1) + "',");               //시.군지역 구분
                    sb.AppendLine("   MiaJuso = '" + VB.RTrim(strMiaJuso) + "' ");                           //우편번호 주소
                    sb.AppendLine("   WHERE Miacode = '" + txtMiaCode.Text.Trim() + "'");
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    dt.Dispose();
                    dt = null;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                dt.Dispose();
                dt = null;

                clsDB.setCommitTran(clsDB.DbCon);
                Screen_Clear();
                txtMiaCode.Focus();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "조 합 기 호  코 드 집";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력일자 : " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            ssView_Sheet1.RowCount = 0;
            Screen_Clear();
            txtMiaCode.Focus();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            int i = 0;
            string strClass1 = string.Empty;
            string strChoice = string.Empty;
            StringBuilder sb = null;
            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            if (string.IsNullOrWhiteSpace(cboDetailClass1.Text))
            {
                ComFunc.MsgBox("상세분류를 선택하세요.");
                cboDetailClass1.Focus();
                return;
            }

            btnPrint.Enabled = true;
            btnShow.Enabled = true;
            btnSearch.Enabled = false;

            Cursor.Current = Cursors.WaitCursor;

            if (rdoReferance0.Checked)
            {
                strChoice = "11";
            }
            else if (rdoReferance1.Checked)
            {
                strChoice = "12";
            }
            else if (rdoReferance2.Checked)
            {
                strChoice = "13";
            }
            else if (rdoReferance3.Checked)
            {
                strChoice = "20";
            }
            else
            {
                strChoice = "90";
            }

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 20;

            strClass1 = VB.Left(cboDetailClass1.Text, 2);

            sb = new StringBuilder();
            sb.AppendLine("SELECT MiaCode,MiaName,MiaDetail,MiaTel,MiaJuso,MiaClass,ROWID");
            sb.AppendLine("  FROM BAS_MIA");
            sb.AppendLine(" WHERE MiaClass = '" + strChoice + "' ");
            sb.AppendLine("   AND MiaDetail = '" + strClass1 + "' ");
            if (rdoReferanceA0.Checked)
            {
                sb.AppendLine(" ORDER BY MiaCode");
            }
            else
            {
                sb.AppendLine(" ORDER BY MiaName");
            }

            SQL = sb.ToString();

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count == 0)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("자료가 없습니다.");
                dt.Dispose();
                dt = null;
                return;
            }

            ssView_Sheet1.RowCount = dt.Rows.Count;
            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["MIACODE"].ToString().Trim();
                ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["MIANAME"].ToString().Trim();
                ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["MIADETAIL"].ToString().Trim();
                ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["MIATEL"].ToString().Trim();
                ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["MIAJUSO"].ToString().Trim();
                ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;

            Cursor.Current = Cursors.Default;
            btnSearch.Enabled = true;
        }

        private void cboDetailClass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                txtMiaName.Focus();
            }
        }

        private void cboDetailClass1_Click(object sender, EventArgs e)
        {
            btnSearch.Enabled = true;
        }

        private void cboGbCity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                txtMailCode.Focus();
            }
        }

        private void cboMiaClass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                cboDetailClass.Focus();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssView_EditModeOff(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            if (ssView_Sheet1.ActiveColumnIndex < 1 || ssView_Sheet1.ActiveColumnIndex > 3)
            {
                return;
            }

            string strData = string.Empty;
            string strROWID = string.Empty;

            string SQL = string.Empty;
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            StringBuilder sb = new StringBuilder();

            strData = ssView_Sheet1.ActiveCell.Text.Trim();
            strROWID = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 5].Text.Trim();

            sb.AppendLine("UPDATE BAS_MIA SET");
            switch (ssView_Sheet1.ActiveColumnIndex)
            {
                case 1:
                    sb.AppendLine(" MiaName = '" + strData + "' ");
                    break;
                case 2:
                    sb.AppendLine(" MiaDeltail = '" + strData + "' ");
                    break;
                case 3:
                    sb.AppendLine(" MiaTel = '" + strData + "' ");
                    break;
            }
            sb.AppendLine(" WHERE ROWID = '" + strROWID + "' ");

            SQL = sb.ToString();

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void txtDelDate_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            frmCalendar2 frmCalendar2X = new frmCalendar2();
            clsPublic.GstrCalDate = "";
            frmCalendar2X.ShowDialog();
            frmCalendar2X.Dispose();
            frmCalendar2X = null;

            txtDelDate.Text = clsPublic.GstrCalDate;
        }

        private void txtDelDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                cboMiaClass.Focus();
            }
        }

        private void txtMailCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                txtMiaJuso.Focus();
            }
        }

        private void txtMiaCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                txtMiaCode.Text = txtMiaCode.Text.Trim().ToUpper();

                if (string.IsNullOrWhiteSpace(txtMiaCode.Text))
                {
                    txtMiaCode.Focus();
                    return;
                }

                Display();
                txtDelDate.Focus();
            }
        }

        private void txtMiaCode_Leave(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnDelete.Enabled = true;
        }

        private void txtMiaJuso_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                btnSave.Focus();
            }
        }

        private void txtMiaName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                txtTelName.Focus();
            }
        }

        private void txtTelName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                cboGbCity.Focus();
            }
        }
    }
}
