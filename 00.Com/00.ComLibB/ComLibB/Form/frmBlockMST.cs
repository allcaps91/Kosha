using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmBlockMST : Form
    {
        public frmBlockMST()
        {
            InitializeComponent();
        }

        private void frmBlockMST_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            dtpFDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddMonths(-1);
            dtpTDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            lblFDate.Text = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddMonths(-1).ToString("yyyy-MM-dd");
            lblTDate.Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            ScreenClear();

            cboQTY.Items.Add("1.회");
            cboQTY.Items.Add("2.회");
            cboQTY.Items.Add("3.회");
            cboQTY.Items.Add("4.회");
            cboQTY.Items.Add("5.회");
            cboQTY.Items.Add("6.회");
            cboQTY.SelectedIndex = 0;

        }

        private void ScreenClear()
        {
            txtPano.Text = "";
            txtSName.Text = "";
            txtAgeSex.Text = "";
            txtTel.Text = "";
            txtHTel.Text = "";
            dtpSDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            cboQTY.Items.Clear();
            chkOK.Checked = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            string strROWID = "";
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            SQL = "SELECT ROWID FROM "+ ComNum.DB_PMPA +"BAS_BLOCKMST ";
            SQL = SQL + " WHERE PANO = '" + txtPano.Text + "' ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }

            strROWID = dt.Rows[0]["ROWID"].ToString().Trim();

            dt.Dispose();
            dt = null;

            if (SaveData(strROWID) == true)
            {
                ScreenClear();
                cmdViewClick();
            }
        }

        private bool SaveData(string strROWID)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;
            
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
            {
                return rtnVal; //권한 확인
            }

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (strROWID != "")
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "UPDATE";
                    SQL = SQL + ComNum.VBLF + " " + ComNum.DB_PMPA + "BAS_BLOCKMST  SET ";
                    SQL = SQL + ComNum.VBLF + "  SDATE = TO_DATE('" + dtpSDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ,";
                    SQL = SQL + ComNum.VBLF + "  QTY = '" + VB.Left(cboQTY.Text, 1) + "' , ";

                    if (chkOK.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "   OKDATE = SYSDATE ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "   OKDATE = '' ";
                    }

                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return rtnVal;
                    }
                }
                else
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "BAS_BLOCKMST (PANO, SDATE, QTY , OKDATE) VALUES ( ";
                    SQL = SQL + ComNum.VBLF + "  '" + txtPano.Text + "', TO_DATE(  '" + dtpSDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ,  '" + VB.Left(cboQTY.Text, 1) + "', ";

                    if (chkOK.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "   SYSDATE  ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "   ''  ";
                    }

                    SQL = SQL + ComNum.VBLF + " )  ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return rtnVal;
                    }
                }
                //clsDB.setRollbackTran(clsDB.DbCon);     //test
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            if (DeleteData() == true)
            {
                ScreenClear();
                cmdViewClick();
            }

        }

        private bool DeleteData()
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;
            bool rtnVal = false;
            
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
            {
                return rtnVal; //권한 확인
            }

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "DELETE  FROM "+ ComNum.DB_PMPA +"BAS_BLOCKMST";
                SQL = SQL + ComNum.VBLF + "   WHERE PANO = '" + txtPano.Text + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                //clsDB.setRollbackTran(clsDB.DbCon);     //test
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제 하였습니다.");
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            cmdViewClick();
        }

        private void cmdViewClick()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            string strJumin = "";

            ssIndex_Sheet1.RowCount = 0;
            
            if (VB.IsDate(dtpFDate.Value) == false)
            {
                return;
            }

            if (VB.IsDate(dtpTDate.Value) == false)
            {
                return;
            }

            try
            {
                //'블록 마스트 읽기
                SQL = " SELECT TO_CHAR(A.SDATE,'YY/MM/DD') SDATE , A.QTY,  TO_CHAR(A.OKDATE,'YY/MM/DD') OKDATE , ";
                SQL = SQL + ComNum.VBLF + "  A.PANO, A.ROWID,  B.SNAME, B.SEX, B.JUMIN1, B.JUMIN2, b.JUMIN3, B.TEL, B.HPHONE  ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_BLOCKMST A  , " + ComNum.DB_PMPA + "BAS_PATIENT B ";
                SQL = SQL + ComNum.VBLF + " WHERE A.SDATE >=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND A.SDATE <=TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND A.PANO = B.PANO (+)";
                SQL = SQL + ComNum.VBLF + " ORDER BY PANO ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                //DataTable의 data를 Spread에 DataSource로 매핑
                ssIndex_Sheet1.RowCount = dt.Rows.Count;
                ssIndex_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssIndex_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssIndex_Sheet1.Cells[i, 1].Text = dt.Rows[i]["sName"].ToString().Trim();

                    if (dt.Rows[i]["Jumin3"].ToString().Trim() != "")
                    {
                        strJumin = dt.Rows[i]["Jumin1"].ToString().Trim() + clsAES.DeAES(dt.Rows[i]["Jumin3"].ToString().Trim());
                    }
                    else
                    {
                        strJumin = dt.Rows[i]["Jumin1"].ToString().Trim() + dt.Rows[i]["Jumin2"].ToString().Trim();
                    }

                    ssIndex_Sheet1.Cells[i, 2].Text = ComFunc.AgeCalc(clsDB.DbCon,  strJumin) + "/" + dt.Rows[i]["Sex"].ToString().Trim();
                    ssIndex_Sheet1.Cells[i, 3].Text = dt.Rows[i]["sDate"].ToString().Trim();
                    ssIndex_Sheet1.Cells[i, 4].Text = dt.Rows[i]["HPHONE"].ToString().Trim() + " / " + dt.Rows[i]["Tel"].ToString().Trim();
                    ssIndex_Sheet1.Cells[i, 5].Text = dt.Rows[i]["OKDATE"].ToString().Trim();
                    ssIndex_Sheet1.Cells[i, 6].Text = dt.Rows[i]["QTY"].ToString().Trim();
                    //ssIndex_Sheet1.Cells[i, 7].Text = dt.Rows[]["ROWID"].ToString().Trim);
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnPrint1_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string strHead1 = "";
            string strHead2 = "";
            string strFont1 = "";
            string strFont2 = "";

            strFont1 = "/fn\"굴림체\" /fz\"18\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strHead1 = "/c/f1" + " 신경차단술 명단" + "/f1/n/n";

            strFont2 = "/fn\"굴림체\" /fz\"11\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strHead2 = "/l/f1" + "◆관리일: " + dtpFDate.Value.ToString("yyyy-MM-dd") + "부터 " + dtpTDate.Value.ToString("yyyy-MM-dd") + "까지" + " /f1/n/n";

            ssIndex_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssIndex_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssIndex_Sheet1.PrintInfo.Margin.Top = 10;
            ssIndex_Sheet1.PrintInfo.Margin.Bottom = 10;
            ssIndex_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssIndex_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssIndex_Sheet1.PrintInfo.ShowBorder = true;
            ssIndex_Sheet1.PrintInfo.ShowColor = true;
            ssIndex_Sheet1.PrintInfo.ShowGrid = false;
            ssIndex_Sheet1.PrintInfo.ShowShadows = true;
            ssIndex_Sheet1.PrintInfo.UseMax = false;
            ssIndex_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssIndex_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssIndex.PrintSheet(0);
        }

        private void btnView2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            ssSunab_Sheet1.RowCount = 0;

            if (txtPano.Text == "")
            {
                return;
            }

            try
            {
                SQL = "SELECT A.BDATE, A.SUCODE, B.SUNAMEK, SUM(QTY * NAL ) NAL   ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_SLIP A, " + ComNum.DB_PMPA + "BAS_SUN B";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE >=TO_DATE('" + dtpSDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND A.SUNEXT = B.SUNEXT (+) ";
                SQL = SQL + ComNum.VBLF + "   AND B.GBNS ='Y' ";
                SQL = SQL + ComNum.VBLF + " GROUP BY A.BDATE, A.SUCODE, B.SUNAMEK ";
                SQL = SQL + ComNum.VBLF + " HAVING SUM(QTY * NAL) != 0 ";
                SQL = SQL + " ORDER BY BDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                //DataTable의 data를 Spread에 DataSource로 매핑
                ssSunab_Sheet1.Rows.Count = dt.Rows.Count;
                ssSunab_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssSunab_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDate"].ToString().Trim();
                    ssSunab_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                    ssSunab_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();
                    ssSunab_Sheet1.Cells[i, 3].Text = dt.Rows[i]["NAL"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void ssIndex_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssIndex_Sheet1.RowCount == 0) { return; }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssIndex, e.Column);
                return;
            }

            ssIndex_Sheet1.Cells[0, 0, ssIndex_Sheet1.RowCount - 1, ssIndex_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssIndex_Sheet1.Cells[e.Row, 0, e.Row, ssIndex_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            if (e.Row == 0 || e.Column == 0)
                return;

            txtPano.Text = ssIndex_Sheet1.Cells[e.Row, 0].Text.ToString().Trim();

            GetData();
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)

        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            GetData();
        }

        private void GetData()
        {
            string strJumin = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssSunab_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = " SELECT PANO , SNAME, TEL, HPHONE, SEX , JUMIN1, JUMIN2, JUMIN3  ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + txtPano.Text + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 등록번호의 환자가 없습니다.");
                    return;
                }
                txtSName.Text = dt.Rows[0]["sName"].ToString().Trim();

                if (dt.Rows[0]["Jumin3"].ToString().Trim() != "")
                {
                    strJumin = dt.Rows[0]["Jumin1"].ToString().Trim() + clsAES.DeAES(dt.Rows[0]["Jumin3"].ToString().Trim());
                }
                else
                {
                    strJumin = dt.Rows[0]["Jumin1"].ToString().Trim() + dt.Rows[0]["Jumin2"].ToString().Trim();
                }

                txtAgeSex.Text = ComFunc.AgeCalc(clsDB.DbCon, strJumin) + "/" + dt.Rows[0]["Sex"].ToString().Trim();

                txtTel.Text = dt.Rows[0]["Tel"].ToString().Trim();
                txtHTel.Text = dt.Rows[0]["HPHONE"].ToString().Trim();

                dt.Dispose();
                dt = null;

                //블록마스트 읽기
                SQL = "";
                SQL = " SELECT SDATE , QTY,  TO_CHAR(OKDATE,'YYYY-MM-DD') OKDATE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_BLOCKMST ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + txtPano.Text + "'  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                dtpSDate.Value = Convert.ToDateTime(dt.Rows[0]["sDate"].ToString().Trim());
                lblFDate.Text = Convert.ToDateTime(dt.Rows[0]["sDate"].ToString().Trim()).ToString();
                if (cboQTY.Text != (dt.Rows[0]["QTY"].ToString().Trim() + ".회") && dt.Rows[0]["QTY"].ToString().Trim() != "")
                {
                    cboQTY.SelectedIndex = Convert.ToInt32(dt.Rows[0]["QTY"].ToString().Trim()) - 1;
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }
    }
}
