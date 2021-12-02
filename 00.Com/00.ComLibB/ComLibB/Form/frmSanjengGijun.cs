using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmSanjengGijun.cs
    /// Description     : 수가코드별 산정기준 등록
    /// Author          : 유진호
    /// Create Date     : 2018-06-05
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\basic\busuga\Busuga01.frm(FrmSanjengGijun) >> frmSanjengGijun.cs 폼이름 재정의" />
    public partial class frmSanjengGijun : Form
    {
        string GstrSanCode = "";
        
        int nDayQty = 0;
        int nIpWonQty = 0;
        string strSuCode = "";
        string strBCheck = "";
        string strRemark = "";
        string strCheckMsg = "";
        string strYesNo = "";
        string strFlag = "";
        string strDisplay = "";
        string strItemCD = "";
        string strFrResult = "";
        string strToResult = "";
        string[] strGupDept;
        string[] strBiGupDept;

        public frmSanjengGijun()
        {
            InitializeComponent();
        }

        public frmSanjengGijun(string strSanCode)
        {
            InitializeComponent();
            GstrSanCode = strSanCode;
        }

        private void frmSanjengGijun_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            strGupDept = new string[5];
            strBiGupDept = new string[5];

            SCREEN_CLEAR();

            cboDisplay.Items.Add("A.All OCS");
            cboDisplay.Items.Add("I.병동OCS");
            cboDisplay.Items.Add("O.외래OCS");


            txtSuCode.Text = "";
            ss1_Sheet1.RowCount = 0;
            ss1_Sheet1.RowCount = 12;

            if (GstrSanCode != "")
            {
                txtSuCode.Text = GstrSanCode;
            }
        }

        private void SCREEN_CLEAR()
        {
            txtSuName.Text = "";
            txtDayQty.Text = "";
            txtIpWonQty.Text = "";
            chkBcheck.Checked = false;
            chkUpDate.Checked = false;

            Control[] penel = null;
            Control obj = null;
            for (int i = 0; i < 5; i++)
            {
                penel = this.Controls.Find("txtGupDept_" + i.ToString(), true);
                if (penel != null)
                {
                    if (penel.Length > 0)
                    {
                        obj = (TextBox)penel[0];
                        obj.Text = "";
                    }
                }
                penel = this.Controls.Find("txtBiGupDept_" + i.ToString(), true);
                if (penel != null)
                {
                    if (penel.Length > 0)
                    {
                        obj = (TextBox)penel[0];
                        obj.Text = "";
                    }
                }
            }
            cboDisplay.Text = "";
            txtRemark.Text = "";
            txtCheckMsg.Text = "";
            txtITemCD.Text = "";
            txtFrom.Text = "";
            txtTo.Text = "";
            lblUnit.Text = "";
            chkBcheck.Checked = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool rtVal = false;
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            int I = 0;
            int j = 0;




            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            if (VB.Trim(txtSuCode.Text) == "")
            {
                ComFunc.MsgBox("등록할 코드 가공란입니다.", "확인");
                return;
            }

            if (ComFunc.MsgBoxQ("등록하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }


            strFlag = "OK";
            strSuCode = VB.Trim(txtSuCode.Text);
            nDayQty = (int)VB.Val(txtDayQty.Text);
            nIpWonQty = (int)VB.Val(txtIpWonQty.Text);
            if (chkBcheck.Checked == true)
            {
                strBCheck = "Y";
            }
            else
            {
                strBCheck = "N";
            }

            strGupDept[0] = txtGupDept_0.Text;
            strGupDept[1] = txtGupDept_1.Text;
            strGupDept[2] = txtGupDept_2.Text;
            strGupDept[3] = txtGupDept_3.Text;
            strGupDept[4] = txtGupDept_4.Text;
            strBiGupDept[0] = txtBiGupDept_0.Text;
            strBiGupDept[1] = txtBiGupDept_1.Text;
            strBiGupDept[2] = txtBiGupDept_2.Text;
            strBiGupDept[3] = txtBiGupDept_3.Text;
            strBiGupDept[4] = txtBiGupDept_4.Text;

            strRemark = txtRemark.Text;
            strCheckMsg = txtCheckMsg.Text;
            strDisplay = VB.Left(cboDisplay.Text, 1);

            strItemCD = txtITemCD.Text;
            strFrResult = txtFrom.Text;
            strToResult = txtTo.Text;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (chkUpDate.Checked == true)
                {
                    if (UPDATE_JSIM_GIJUN() == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
                else
                {
                    if (INSERT_JSIM_GIJUN() == false)
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
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }

            REMARK_PROCSESS();
            SCREEN_CLEAR();
            txtSuCode.Text = "";
        }

        private bool UPDATE_JSIM_GIJUN()
        {
            bool rtVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            SQL = "UPDATE JSIM_GIJUN SET";
            SQL = SQL + ComNum.VBLF + " DAYQTY     = '" + nDayQty + "',";
            SQL = SQL + ComNum.VBLF + " IPWONQTY   = '" + nIpWonQty + "',";
            SQL = SQL + ComNum.VBLF + " BCHECK     = '" + strBCheck + "',";
            SQL = SQL + ComNum.VBLF + " GUPDEPT1   = '" + strGupDept[0] + "',";
            SQL = SQL + ComNum.VBLF + " GUPDEPT2   = '" + strGupDept[1] + "',";
            SQL = SQL + ComNum.VBLF + " GUPDEPT3   = '" + strGupDept[2] + "',";
            SQL = SQL + ComNum.VBLF + " GUPDEPT4   = '" + strGupDept[3] + "',";
            SQL = SQL + ComNum.VBLF + " GUPDEPT5   = '" + strGupDept[4] + "',";
            SQL = SQL + ComNum.VBLF + " BIGUPDEPT1 = '" + strBiGupDept[0] + "',";
            SQL = SQL + ComNum.VBLF + " BIGUPDEPT2 = '" + strBiGupDept[1] + "',";
            SQL = SQL + ComNum.VBLF + " BIGUPDEPT3 = '" + strBiGupDept[2] + "',";
            SQL = SQL + ComNum.VBLF + " BIGUPDEPT4 = '" + strBiGupDept[3] + "',";
            SQL = SQL + ComNum.VBLF + " BIGUPDEPT5 = '" + strBiGupDept[4] + "',";
            SQL = SQL + ComNum.VBLF + " CHECKMSG   = '" + strCheckMsg + "',";
            SQL = SQL + ComNum.VBLF + " DISPLAY    = '" + strDisplay + "',";
            SQL = SQL + ComNum.VBLF + " ITEMCD     = '" + strItemCD + "',";
            SQL = SQL + ComNum.VBLF + " FRRESULT   = '" + strFrResult + "',";
            SQL = SQL + ComNum.VBLF + " TORESULT   = '" + strToResult + "'  ";
            SQL = SQL + ComNum.VBLF + " WHERE SUCODE = '" + strSuCode + "'";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                return rtVal;
            }

            rtVal = true;
            return rtVal;
        }

        private bool INSERT_JSIM_GIJUN()
        {
            bool rtVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            SQL = " INSERT INTO JSIM_GIJUN (SUCODE, DAYQTY, IPWONQTY, BCHECK, ";
            SQL = SQL + ComNum.VBLF + "     GUPDEPT1, GUPDEPT2, GUPDEPT3, GUPDEPT4, GUPDEPT5,";
            SQL = SQL + ComNum.VBLF + "     BIGUPDEPT1, BIGUPDEPT2, BIGUPDEPT3, BIGUPDEPT4, BIGUPDEPT5, ";
            SQL = SQL + ComNum.VBLF + "     CHECKMSG, REMARK, DISPLAY,";
            SQL = SQL + ComNum.VBLF + "     ITEMCD, FRRESULT, TORESULT)";
            SQL = SQL + ComNum.VBLF + " VALUES('" + strSuCode + "', '" + nDayQty + "', '" + nIpWonQty + "', '" + strBCheck + "', ";
            SQL = SQL + ComNum.VBLF + "        '" + strGupDept[0] + "', '" + strGupDept[1] + "', '" + strGupDept[2] + "', ";
            SQL = SQL + ComNum.VBLF + "        '" + strGupDept[3] + "', '" + strGupDept[4] + "',";
            SQL = SQL + ComNum.VBLF + "        '" + strBiGupDept[0] + "', '" + strBiGupDept[1] + "', '" + strBiGupDept[2] + "', ";
            SQL = SQL + ComNum.VBLF + "        '" + strBiGupDept[3] + "', '" + strBiGupDept[4] + "',";
            SQL = SQL + ComNum.VBLF + "        '" + strCheckMsg + "', '1', '" + strDisplay + "', ";
            SQL = SQL + ComNum.VBLF + "        '" + strItemCD + "', '" + strFrResult + "','" + strToResult + "')";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                return rtVal;
            }

            rtVal = true;
            return rtVal;
        }

        private bool REMARK_PROCSESS()
        {
            bool rtVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            SQL = "UPDATE JSIM_GIJUN SET";
            SQL = SQL + ComNum.VBLF + " REMARK     = '" + strRemark + "',";            
            SQL = SQL + ComNum.VBLF + " WHERE SUCODE = '" + strSuCode + "'";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                return rtVal;
            }

            rtVal = true;
            return rtVal;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
            txtSuCode.Focus();
            txtSuCode.Text = "";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            bool rtVal = false;
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (ComFunc.MsgBoxQ("삭제하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                strSuCode = txtSuCode.Text;

                SQL = " DELETE JSIM_GIJUN WHERE SUCODE = '" + strSuCode + "'";

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
                ComFunc.MsgBox("삭제 완료 하였습니다.");
                Cursor.Current = Cursors.Default;                
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }

            txtSuCode.Text = "";
            txtSuCode.Focus();
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

                SQL = "SELECT A.SUCODE, A.DAYQTY, A.IPWONQTY, A.BCHECK, B.SUNAMEK,";
                SQL = SQL + ComNum.VBLF + " A.GUPDEPT1 ||','|| A.GUPDEPT2 ||','|| A.GUPDEPT3 ||','|| A.GUPDEPT4 ||','|| A.GUPDEPT5  GUPDEPT,";
                SQL = SQL + ComNum.VBLF + " A.BIGUPDEPT1 ||','|| A.BIGUPDEPT2 ||','|| A.BIGUPDEPT3 ||','|| A.BIGUPDEPT4 ||','|| A.BIGUPDEPT5  BIGUPDEPT,";
                SQL = SQL + ComNum.VBLF + " A.CHECKMSG, A.DISPLAY";
                SQL = SQL + ComNum.VBLF + " FROM JSIM_GIJUN A, BAS_SUN B";
                SQL = SQL + ComNum.VBLF + " WHERE A.SUCODE = B.SUNEXT";
                SQL = SQL + ComNum.VBLF + "  ORDER BY A.SUCODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                
                if (dt.Rows.Count > 0)
                {
                    ss1_Sheet1.RowCount = dt.Rows.Count;
                    ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {                        
                        ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DAYQTY"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["IPWONQTY"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BCHECK"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DISPLAY"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["GUPDEPT"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BIGUPDEPT"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CHECKMSG"].ToString().Trim();
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ss1_Sheet1.RowCount = 0;
            ss1_Sheet1.RowCount = 12;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";
            string SysDate = "";

            SysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            Cursor.Current = Cursors.WaitCursor;

            strFont1 = "/fn\"굴림체\" /fz\"16\" /fb0 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"10\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead1 = "/n/n/f1/C 수가코드별 산정기준집" + "/n/n/n/n";
            strHead2 = "/l/f2" + "인쇄일자 : " + SysDate;

            //ssView1_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;	//가로
            ss1_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;   //세로

            ss1_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ss1_Sheet1.PrintInfo.Margin.Left = 35;
            ss1_Sheet1.PrintInfo.Margin.Right = 0;
            ss1_Sheet1.PrintInfo.Margin.Top = 35;
            ss1_Sheet1.PrintInfo.Margin.Bottom = 30;
            ss1_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ss1_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ss1_Sheet1.PrintInfo.ShowBorder = true;
            ss1_Sheet1.PrintInfo.ShowColor = false;
            ss1_Sheet1.PrintInfo.ShowGrid = true;
            ss1_Sheet1.PrintInfo.ShowShadows = false;
            ss1_Sheet1.PrintInfo.UseMax = false;
            ss1_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ss1.PrintSheet(0);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ss1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (ss1_Sheet1.Cells[e.Row, e.Column].Text == "") return;

            try
            {                                
                SQL = "SELECT SUNAMEK, UNIT FROM  BAS_SUN ";
                SQL = SQL + ComNum.VBLF + " WHERE SUNEXT ='" + ss1_Sheet1.Cells[e.Row, e.Column].Text + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                
                if (dt.Rows.Count > 0)
                {                    
                    txtSuName.Text = dt.Rows[0]["SUNAMEK"].ToString().Trim();
                    lblUnit.Text = dt.Rows[0]["UNIT"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            READ_JSIM_GIJUN(ss1_Sheet1.Cells[e.Row, e.Column].Text);
            READ_JSIM_SIMSAMSG(ss1_Sheet1.Cells[e.Row, e.Column].Text);
        }

        private void READ_JSIM_GIJUN(string strSuCode)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "SELECT A.SUCODE, A.DAYQTY, A.IPWONQTY, A.BCHECK, "; //' B.SUNAMEK,";
                SQL = SQL + ComNum.VBLF + " A.GUPDEPT1, A.GUPDEPT2, A.GUPDEPT3 , A.GUPDEPT4 , A.GUPDEPT5 ,";
                SQL = SQL + ComNum.VBLF + " A.BIGUPDEPT1, A.BIGUPDEPT2, A.BIGUPDEPT3, A.BIGUPDEPT4, A.BIGUPDEPT5, ";
                SQL = SQL + ComNum.VBLF + " A.CHECKMSG, A.REMARK, A.DISPLAY, A.ITEMCD, A.FRRESULT, A.TORESULT  ";
                SQL = SQL + ComNum.VBLF + " FROM JSIM_GIJUN A "; //', BAS_SUN B"            ;
                SQL = SQL + ComNum.VBLF + "  WHERE A.SUCODE = '" + strSuCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                
                if (dt.Rows.Count > 0)
                {                    
                    ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DEPTKORNAME"].ToString().Trim();

                    chkUpDate.Checked = true;
                    txtSuCode.Text = strSuCode;
                    txtDayQty.Text = dt.Rows[i]["DAYQTY"].ToString().Trim();
                    txtIpWonQty.Text = dt.Rows[i]["IPWONQTY"].ToString().Trim();
                    if (dt.Rows[i]["BCHECK"].ToString().Trim() == "Y") chkBcheck.Checked = true;


                    txtGupDept_0.Text = dt.Rows[i]["GUPDEPT1"].ToString().Trim();
                    txtGupDept_1.Text = dt.Rows[i]["GUPDEPT2"].ToString().Trim();
                    txtGupDept_2.Text = dt.Rows[i]["GUPDEPT3"].ToString().Trim();
                    txtGupDept_3.Text = dt.Rows[i]["GUPDEPT4"].ToString().Trim();
                    txtGupDept_4.Text = dt.Rows[i]["GUPDEPT5"].ToString().Trim();


                    txtBiGupDept_0.Text = dt.Rows[i]["BIGUPDEPT1"].ToString().Trim();
                    txtBiGupDept_1.Text = dt.Rows[i]["BIGUPDEPT2"].ToString().Trim();
                    txtBiGupDept_2.Text = dt.Rows[i]["BIGUPDEPT3"].ToString().Trim();
                    txtBiGupDept_3.Text = dt.Rows[i]["BIGUPDEPT4"].ToString().Trim();
                    txtBiGupDept_4.Text = dt.Rows[i]["BIGUPDEPT5"].ToString().Trim();
                    
                    txtCheckMsg.Text = dt.Rows[i]["CHECKMSG"].ToString().Trim();
                    
                    txtITemCD.Text = dt.Rows[i]["ITEMCD"].ToString().Trim();
                    txtFrom.Text = dt.Rows[i]["FRRESULT"].ToString().Trim();
                    txtTo.Text = dt.Rows[i]["TORESULT"].ToString().Trim();
                    txtRemark.Text = dt.Rows[i]["REMARK"].ToString().Trim();

                    switch (dt.Rows[i]["DISPLAY"].ToString().Trim())
                    {
                        case "A":
                            cboDisplay.Text = "A.ALL OCS";
                            break;
                        case "I":
                            cboDisplay.Text = "I.병동 OCS";
                            break;
                        case "O":
                            cboDisplay.Text = "O.외래 OCS";
                            break;
                    }                
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void READ_JSIM_SIMSAMSG(string strSuCode)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "SELECT REMARK, TO_CHAR(SDATE,'YYYY-MM-DD') SDATE,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(DDATE,'YYYY-MM-DD') DDATE";
                SQL = SQL + ComNum.VBLF + " FROM JSIM_SIMSAMSG ";
                SQL = SQL + ComNum.VBLF + "  WHERE SUCODE = '" + strSuCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    btnOcsMsg.ForeColor = Color.FromArgb(255, 0, 0);
                }
                else
                {
                    btnOcsMsg.ForeColor = Color.FromArgb(0, 0, 0);
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void txtSuCode_Enter(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
        }

        private void txtSuCode_KeyDown(object sender, KeyEventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (e.KeyCode != Keys.Enter) return;

            if (txtSuCode.Text == "") return;

            try
            {
                SQL = "SELECT SUNAMEK, UNIT FROM  BAS_SUN ";
                SQL = SQL + ComNum.VBLF + " WHERE SUNEXT ='" + txtSuCode.Text + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    txtSuName.Text = dt.Rows[0]["SUNAMEK"].ToString().Trim();
                    lblUnit.Text = dt.Rows[0]["UNIT"].ToString().Trim();
                }
                else
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("입력하려는 코드는 수가에 등록되지 않은 코드입니다.");
                    return;
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            READ_JSIM_GIJUN(txtSuName.Text);
            READ_JSIM_SIMSAMSG(txtSuName.Text);
        }
    }
}
