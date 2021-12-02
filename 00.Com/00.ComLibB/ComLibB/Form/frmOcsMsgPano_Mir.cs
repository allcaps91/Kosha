using ComBase; //기본 클래스
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmOcsMsgPano_Mir.cs
    /// Description     : 심사과 환자메세지등록(외래환자)-청구 참고사항 폼
    /// Author          : 안정수
    /// Create Date     : 2017-06-10
    /// Update History  : GnJobSabun을 받아들이는 생성자 추가
    /// <history>       
    /// D:\타병원\PSMHH\basic\busuga\BuSuga67.frm(FrmOcsMsgPano_MIR) => frmOcsMsgPano_Mir.cs 으로 변경함
    /// ORA_29275 : 부분 다중 바이트 문자 오류 발생, LenB, RightB, 구현 필요
    /// 컨버전하기 애매한 부분 주석처리
    /// READ_BAS_ClinicDeptNameK 구현필요(vbFunc.bas)
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\basic\busuga\BuSuga67.frm(FrmOcsMsgPano_MIR)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\basic\busuga\busuga.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    public partial class frmOcsMsgPano_Mir : Form
    {
        string FGstrRowid = "";
        string FstrPano = "";
        string FstrSName = "";
        string mJobSabun = "";

        private frmSpecialText frmSpecialTextX = null;

        long FnWrtno = 0;

        ComFunc cfun = new ComFunc();

        public frmOcsMsgPano_Mir()
        {
            InitializeComponent();
        }

        public frmOcsMsgPano_Mir(string strJobSabun)
        {
            InitializeComponent();
            mJobSabun = strJobSabun;
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmOcsMsgPano_Mir_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            SCREEN_CLEAR();

            ComboDept_SET(cboDept);

            optSort0.Checked = true;
            optView1.Checked = true;

            //groupBox3.Visible = false;

        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
        }

        void SCREEN_CLEAR()
        {
            txtInfo.Text = "";
            txtPano.Text = "";
            lblName.Text = "";
            FGstrRowid = "";

            btnSave.Enabled = false;
            btnDelete.Enabled = false;
        }

        void ComboDept_SET(ComboBox ArgCombo, string ArgAll = "", string ArgTYPE = "1")
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int i = 0;

            if(ArgAll == "")
            {
                ArgAll = "1";
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  DEPTCODE, DEPTNAMEK";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT";
            SQL += ComNum.VBLF + "ORDER BY PRINTRANKING ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            ArgCombo.Items.Clear();

            //2019-06-25 김해수 작성자 목록클리어 및 작성자 심사팀 담당자
            cboSabun.Items.Clear();
            cboSabun.Items.Add("*****.전체");
            cboSabun.Items.Add("15273.정희정");
            cboSabun.Items.Add("13635.이민주");
            cboSabun.Items.Add("19399.김준수");
            cboSabun.Items.Add("00468.심경순");
            cboSabun.Items.Add("22699.김연서");
            cboSabun.Items.Add("21181.이향숙");
            cboSabun.Items.Add("02749.김순옥");
            cboSabun.Items.Add("38320.현미정");
            cboSabun.Items.Add("37074.김성열");
            cboSabun.Items.Add("27176.이은주");
            cboSabun.Items.Add("46000.정지애");
            cboSabun.Items.Add("#####.목록외");
            cboSabun.SelectedIndex = 0;

            if (ArgAll == "1")
            {
                switch (ArgTYPE)
                {
                    case "1":
                        ArgCombo.Items.Add("**.전체");
                        break;
                    case "2":
                        ArgCombo.Items.Add("**");
                        break;
                    case "3":
                        ArgCombo.Items.Add("전체");
                        break;
                }
            }

            for(i = 0; i < dt.Rows.Count; i++)
            {
                switch (ArgTYPE)
                {
                    case "1":
                        ArgCombo.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim() + "." + dt.Rows[i]["DeptNameK"].ToString().Trim());
                        break;
                    case "2":
                        ArgCombo.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim());
                        break;
                    case "3":
                        ArgCombo.Items.Add(dt.Rows[i]["DeptNamek"].ToString().Trim());
                        break;
                }
            }

            ArgCombo.SelectedIndex = 0;

            dt.Dispose();
            dt = null;
        }

        void btnFont_Click(object sender, EventArgs e)
        {
            DialogResult dr = this.fontDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                txtInfo.Font = fontDialog1.Font;
                //txtInfo.SelectionFont = new System.Drawing.Font("맑은 고딕", 14);
                txtInfo.SelectionColor = fontDialog1.Color;
            }

            txtInfo.Focus();
        }

        void btnSText_Click(object sender, EventArgs e)
        {
            frmSpecialTextX = new frmSpecialText();
            frmSpecialTextX.rSendText += new frmSpecialText.SendText(GetText);
            frmSpecialTextX.rEventExit += new frmSpecialText.EventExit(frmSpecialTextX_rEventExit);
            frmSpecialTextX.Show();
        }

        void GetText(string str)
        {
            txtInfo.Text += str;
        }

        void frmSpecialTextX_rEventExit()
        {            
            frmSpecialTextX.Dispose();
            frmSpecialTextX = null;           
        }

        void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            GetData();
        }

        void GetData()
        {
            ssList_Sheet1.RowCount = 0;

            if (optView1.Checked == true && txtPano.Text.Trim() == "")
            {
                ComFunc.MsgBox("등록번호를 입력해야합니다.");
                return;
            }

            int i = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;


            SQL = "SELECT A.PANO  APANO , A.SNAME,   B.PANO BPANO, TO_CHAR(B.SDATE,'YYYY-MM-DD') SDATE, B.ENTSABUN";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT A, " + ComNum.DB_PMPA + "BAS_OCSMEMO_MIR B";
            if (optView0.Checked == true)
            {
                SQL += ComNum.VBLF + "WHERE A.PANO = B.PANO ";
            }
            if (optView1.Checked == true)
            {
                SQL += ComNum.VBLF + "WHERE A.PANO = B.PANO(+) "; // 재원자
            }
            if (txtPano.Text.Trim() != "")
            {
                SQL += ComNum.VBLF + "  AND A.PANO = '" + txtPano.Text + "'";
            }

            //2019-06-24 김해수 김준수 주임 요청으로 삭제일 있는 데이터 조회 안함
            SQL += ComNum.VBLF + "   AND B.DDATE IS NULL";

            switch (VB.Left(cboSabun.Text, 5))
            {
                case "*****": //전체조회
                    break;
                case "#####": //심사팀 직원외 조회
                    SQL += ComNum.VBLF + "   AND (B.ENTSABUN NOT IN ('15273','13635','19399','00468','22699','21181','02749','38320','37074','27176','46000') OR B.ENTSABUN IS NULL)";
                    break;
                default:      //작성자사번에따라 조회
                    SQL += ComNum.VBLF + "   AND B.ENTSABUN = '" + VB.Left(cboSabun.Text, 5) + "'";
                    break;
            }

            if (optSort0.Checked == true)
            {
                SQL += ComNum.VBLF + "GROUP BY A.PANO, A.SNAME,   B.PANO,B.SDATE,B.ENTSABUN";
            }
            if (optSort1.Checked == true)
            {
                SQL += ComNum.VBLF + "GROUP BY A.SNAME, A.PANO,   B.PANO,B.SDATE,B.ENTSABUN";
            }

            //오류 발생 쿼리부분, DBView로 볼때는 데이터 나오지만 데이터를 뿌릴때 오류발생
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            ssMsg_Sheet1.RowCount = 0;
            ssList_Sheet1.RowCount = 0;
            ssList_Sheet1.RowCount = dt.Rows.Count;
            ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (i = 0; i < ssList_Sheet1.RowCount; i++)
            {
                ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["APANO"].ToString().Trim();
                ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();

                ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SDATE"].ToString().Trim(); //등록일
                ssList_Sheet1.Cells[i, 4].Text = cfun.Read_SabunName(clsDB.DbCon, dt.Rows[i]["ENTSABUN"].ToString().Trim()); //작성자

                if (dt.Rows[i]["BPANO"].ToString().Trim() != "")
                {
                    ssList_Sheet1.Rows[i].BackColor = Color.FromArgb(255, 233, 233);
                }
            }

            dt.Dispose();
            dt = null;
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인
            DelData();
        }

        void DelData()
        {
            if(FGstrRowid == "")
            {
                ComFunc.MsgBox("전산오류입니다.");
                return;
            }

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "UPDATE";
                SQL += ComNum.VBLF + ComNum.DB_PMPA + "BAS_OCSMEMO_MIR";
                SQL += ComNum.VBLF + "SET";
                SQL += ComNum.VBLF + "DDATE = TRUNC(SYSDATE)";
                SQL += ComNum.VBLF + "WHERE ROWID = '" + FGstrRowid + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제하였습니다.");
                Cursor.Current = Cursors.Default;

                SCREEN_CLEAR();

                //GetData();
            }

            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        void btnReg_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();

            btnSave.Enabled = true;
            txtInfo.Focus();
        }
         
        void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            //if (string.IsNullOrEmpty(txtPano.Text) || string.IsNullOrEmpty(lblName.Text))
            //{
            //    ComFunc.MsgBox("등록번호를 확인해주세요.");
            //    txtPano.Focus();
            //    return;
            //}

            if(SaveData() == true)
            {
                SCREEN_CLEAR();

                btnReg.Enabled = false;

                ComFunc.MsgBox("저장완료");
            }
        }

        bool SaveData()
        {
            string strData = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            

            strData = VB.Replace(txtInfo.Text, "'", "`");

            try
            {
                if (FGstrRowid == "")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  MAX(WRTNO) MWRTNO";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_OCSMEMO_MIR";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return false;
                    }

                    FnWrtno = Convert.ToInt16(dt.Rows[0]["MWRTNO"].ToString().Trim()) + 1;
                    SqlErr = "";
                    dt.Dispose();
                    dt = null;

                    if(FstrPano == "") { FstrPano = txtPano.Text; }
                    if(FstrSName == "") { FstrSName = lblName.Text; }

                    SQL = "";
                    SQL += ComNum.VBLF + "INSERT INTO  ";
                    SQL += ComNum.VBLF + ComNum.DB_PMPA + "BAS_OCSMEMO_MIR";
                    SQL += ComNum.VBLF + "(  PANO, SNAME, MEMO, SDATE, DDATE, WRTNO, ENTSABUN, DeptCode ) ";
                    SQL += ComNum.VBLF + "VALUES ( ";
                    SQL += ComNum.VBLF + " '" + FstrPano + "', ";
                    SQL += ComNum.VBLF + " '" + FstrSName + "', ";
                    SQL += ComNum.VBLF + " :MEMO, ";
                    SQL += ComNum.VBLF + " trunc(sysdate),";
                    SQL += ComNum.VBLF + " '' , ";
                    SQL += ComNum.VBLF + " '" + FnWrtno + "', ";
                    SQL += ComNum.VBLF + " '" + clsType.User.Sabun + "' ,";
                    SQL += ComNum.VBLF + "  '" + VB.Left(cboDept.SelectedItem.ToString(), 2) + "'";
                    SQL += ComNum.VBLF + ")";

                    SqlErr = clsDB.ExecuteLongQuery(SQL, txtInfo.Rtf , ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }


                    //SQL = "UPDATE " + ComNum.DB_PMPA + "BAS_OCSMEMO_MIR ";
                    //SQL = SQL + ComNum.VBLF + "  MEMO = :MEMO ";
                    //SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + FstrPano + "' ";
                    //SQL = SQL + ComNum.VBLF + "   AND WRTNO = '" + FnWrtno + "' ";

                    //PsmhDb pDbCon = clsDB.DbCon;
                    //OracleCommand cmd = new OracleCommand(SQL, pDbCon.Con);

                    //cmd.Parameters.Add("MEMO", txtInfo.Rtf);
                    //cmd.ExecuteNonQuery();



                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT ";
                    SQL += ComNum.VBLF + "  WRTNO, ROWID ";
                    SQL += ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_OCSMEMO_MIR";
                    SQL += ComNum.VBLF + " WHERE PANO = '" + FstrPano + "' ";
                    SQL += ComNum.VBLF + "   AND WRTNO = '" + FnWrtno + "' ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return false;
                    }

                    FnWrtno = Convert.ToInt16(dt.Rows[0]["WRTNO"].ToString().Trim()) + 1;

                    FGstrRowid = dt.Rows[0]["ROWID"].ToString().Trim();

                    dt.Dispose();
                    dt = null;
                }
                else
                {

                    SQL = "";
                    SQL += ComNum.VBLF + " UPDATE ";
                    SQL += ComNum.VBLF + ComNum.DB_PMPA + "BAS_OCSMEMO_MIR";
                    SQL += ComNum.VBLF + " SET ";
                    SQL += ComNum.VBLF + "MEMO = :MEMO, ";
                    SQL += ComNum.VBLF + " ENTSABUN = '" + clsType.User.Sabun + "',";
                    SQL += ComNum.VBLF + " DeptCode = '" + VB.Left(cboDept.SelectedItem.ToString(), 2) + "'  ";
                    SQL += ComNum.VBLF + " WHERE ROWID = '" + FGstrRowid + "' ";


                    SqlErr = clsDB.ExecuteLongQuery(SQL, txtInfo.Rtf ,ref intRowAffected, clsDB.DbCon);  
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }


                    //SQL = "UPDATE " + ComNum.DB_PMPA + "BAS_OCSMEMO_MIR";
                    //SQL = SQL + ComNum.VBLF + "  MEMO = :MEMO ";
                    //SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FGstrRowid + "' ";

                    //PsmhDb pDbCon = clsDB.DbCon;
                    //OracleCommand cmd = new OracleCommand(SQL, pDbCon.Con);

                    //cmd.Parameters.Add("MEMO", txtInfo.Rtf);
                    //cmd.ExecuteNonQuery();                    
                }


                SQL = "";
                SQL = "SELECT MEMO ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_OCSMEMO_MIR ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FGstrRowid + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                if (dt.Rows.Count > 0)
                {                    
                    txtInfo.Rtf = VB.Replace(dt.Rows[0]["MEMO"].ToString(), "`", "'");
                }
                
                dt.Dispose();
                dt = null;


                clsDB.setCommitTran(clsDB.DbCon);                
                Cursor.Current = Cursors.Default;
                return true;                
            }
            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (e.Row < 0 || e.Column < 0) return;

            //마우스 우클릭
            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            if (sender == this.ssList)
            {
                if (e.ColumnHeader == true)
                {
                    clsSpread.gSpdSortRow(ssList, e.Column); //sort 정렬 기능 
                    return;
                }
                if (e.RowHeader == true)
                {
                    return;
                }
            }

            FstrPano = ssList_Sheet1.Cells[e.Row, 0].Text;
            FstrSName = ssList_Sheet1.Cells[e.Row, 1].Text;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  PANO, SNAME, TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, TO_CHAR(DDATE,'YYYY-MM-DD') DDATE,";
            SQL += ComNum.VBLF + "  WRTNO, ROWID, ENTSABUN";
            SQL += ComNum.VBLF + "FROM " +ComNum.DB_PMPA + "BAS_OCSMEMO_MIR";
            SQL += ComNum.VBLF + "WHERE PANO = '" + FstrPano + "'";
            SQL += ComNum.VBLF + "ORDER BY SDATE DESC";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            ssMsg_Sheet1.RowCount = dt.Rows.Count;

            for(i = 0; i < ssMsg_Sheet1.RowCount; i++)
            {
                ssMsg_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SDATE"].ToString().Trim();
                ssMsg_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DDATE"].ToString().Trim();
                ssMsg_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();
                ssMsg_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                ssMsg_Sheet1.Cells[i, 4].Text = cfun.Read_SabunName(clsDB.DbCon, dt.Rows[i]["ENTSABUN"].ToString().Trim());
                ssMsg_Sheet1.Cells[i, 5].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                ssMsg_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ROWID"].ToString().Trim();
            }

            if(dt.Rows.Count == 0)
            {
                SCREEN_CLEAR();

                btnSave.Enabled = true;
            }

            dt.Dispose();
            dt = null;
            btnReg.Enabled = true;

            
        }

        void ssMsg_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if(e.Row == -1)
            {
                return;
            }

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            
            DataTable dt = null;

            FGstrRowid = ssMsg_Sheet1.Cells[e.Row, 6].Text;

            try
            {
                if (e.RowHeader == true)
                {

                    clsDB.setBeginTran(clsDB.DbCon);

                    if (FGstrRowid == "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }


                    if (MessageBox.Show("완전삭제 하시겠습니까", "작업선택", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                    else
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        

                        SQL = "";
                        SQL += ComNum.VBLF + "DELETE";
                        SQL += ComNum.VBLF + ComNum.DB_PMPA + "BAS_OCSMEMO_MIR";
                        SQL += ComNum.VBLF + "WHERE ROWID = '" + FGstrRowid + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        clsDB.setCommitTran(clsDB.DbCon);
                        ComFunc.MsgBox("삭제하였습니다.");
                        Cursor.Current = Cursors.Default;
                    }
                    
                }

                SQL = "";
                SQL += ComNum.VBLF + "SELECT";
                SQL += ComNum.VBLF + "  MEMO , DEPTCODE";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_OCSMEMO_MIR";
                SQL += ComNum.VBLF + "WHERE ROWID = '" + FGstrRowid + "'";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                txtInfo.Rtf =dt.Rows[0]["MEMO"].ToString().Trim().Replace("`", "'");                
                cboDept.Text = dt.Rows[0]["DeptCode"].ToString().Trim() + "." + clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, dt.Rows[0]["DeptCode"].ToString().Trim());
                btnSave.Enabled = true;
                btnDelete.Enabled = true;

                dt.Dispose();
                dt = null;
            }

            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        void txtPano_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar != 13)
            {
                return;
            }

            GetData();
        }

        void txtPano_Leave(object sender, EventArgs e)
        {
            if(txtPano.Text == "")
            {
                lblName.Text = "";
            }

            txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  PANO , SNAME";
            SQL += ComNum.VBLF + "FROM " +ComNum.DB_PMPA + "BAS_PATIENT";
            SQL += ComNum.VBLF + "WHERE PANO = '" + txtPano.Text.Trim() + "'";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                lblName.Text = dt.Rows[0]["SNAME"].ToString().Trim();
            }
            else
            {
                txtPano.Text = "";
                lblName.Text = "";
            }

            dt.Dispose();
            dt = null;
            
        }

        void ssMsg_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            
        }

        void ssMsg_TextChanged(object sender, EventArgs e)
        {
            
        }

        void ssMsg_Enter(object sender, EventArgs e)
        {
            
        }
    }
}

