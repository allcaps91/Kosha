using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary> 외래 진료과별 특정코드 수납내역 확인 </summary>
    public partial class frmSearchSilp : Form
    {
        /// <summary> 외래 진료과별 특정코드 수납내역 확인 </summary>
        public frmSearchSilp()
        {
            InitializeComponent();
        }

        //TODO: FormInfo_Histort 모듈(registry.bas) 레지스트리
        void frmSearchSilp_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                //Call FormInfo_History(Me.Name, Me.Caption)  TODO: FormInfo_Histort 모듈(registry.bas) 레지스트리

                ssView2_Sheet1.Columns[5].Visible = true;
                cboDept.Items.Clear();

                SQL = "";
                SQL = "SELECT DEPTCODE, DEPTNAMEK";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT";
                SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING";

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

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboDept.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim() + "." + dt.Rows[i]["DEPTNAMEK"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;

                cboDept.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void ViewCode(string strArg)
        {

            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                ssView1_Sheet1.RowCount = 0;

                SQL = "";
                SQL = "SELECT A.SUCODE, B.SUNAMEK ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_OPD_SLIPVIEWCODE A, " + ComNum.DB_PMPA + "BAS_SUN B";
                SQL = SQL + ComNum.VBLF + " WHERE A.SUCODE = B.SUNEXT ";
                SQL = SQL + ComNum.VBLF + "   AND A.DEPTCODE = '" + strArg + "' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY A.SUCODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {

                    ssView1_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();
                    }

                    ssView1_Sheet1.RowCount = ssView1_Sheet1.RowCount + 10;
                    ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
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
                return;
            }
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            if (chkIpwon.Checked == true)
            {
                ViewSlipIpd(VB.Left(cboDept.Text, 2), dtpDate1.Value.ToString("yyyy-MM-dd").Trim(), dtpDate2.Value.ToString("yyyy-MM-dd").Trim());
            }
            else
            {
                ViewSlip(VB.Left(cboDept.Text, 2), dtpDate1.Value.ToString("yyyy-MM-dd").Trim(), dtpDate2.Value.ToString("yyyy-MM-dd").Trim());
            }
        }

        void ViewSlipIpd(string ArgDept, string ArgDate, string ArgDate2)
        {

            int i = 0;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                ssView2_Sheet1.RowCount = 0;

                if (ArgDept == "EN")
                {
                    SQL = "";
                    SQL = " SELECT A.PANO, A.BDATE,B.SNAME, SUM(A.QTY*A.NAL) CNT, A.SUCODE, C.SUNAMEK, D.ORDERSITE";
                }
                else
                {
                    SQL = "";
                    SQL = " SELECT A.PANO, B.SNAME, SUM(A.QTY*A.NAL) CNT, A.SUCODE, C.SUNAMEK, D.ORDERSITE";
                }

                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP A, " + ComNum.DB_PMPA + "BAS_PATIENT B, ";
                SQL = SQL + ComNum.VBLF + "       " + ComNum.DB_PMPA + "BAS_SUN C, " + ComNum.DB_MED + "OCS_IORDER D ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND A.SUNEXT IN (" + ReadSlipCode(ArgDept) + ")";
                SQL = SQL + ComNum.VBLF + "   AND A.BDATE >= TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND A.BDATE <= TO_DATE('" + ArgDate2 + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND A.DEPTCODE = '" + ArgDept + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.PANO = B.PANO";
                SQL = SQL + ComNum.VBLF + "   AND A.SUNEXT = C.SUNEXT";
                SQL = SQL + ComNum.VBLF + "   AND A.PANO = D.PTNO";
                SQL = SQL + ComNum.VBLF + "   AND A.BDATE = D.BDATE ";
                SQL = SQL + ComNum.VBLF + "   AND A.ORDERNO = D.ORDERNO ";
                SQL = SQL + ComNum.VBLF + "   AND D.ORDERSITE IN ('OPD','OPDX') ";

                if (ArgDept == "EN")
                {
                    SQL = SQL + ComNum.VBLF + " GROUP BY A.PANO,A.BDATE, B.SNAME, A.SUCODE, C.SUNAMEK, D.ORDERSITE";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " GROUP BY A.PANO, B.SNAME, A.SUCODE, C.SUNAMEK, D.ORDERSITE";
                }

                SQL = SQL + ComNum.VBLF + " ORDER BY A.PANO ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView2_Sheet1.RowCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    //SS2.Row = i + 1
                    ssView2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssView2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssView2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["CNT"].ToString().Trim();
                    ssView2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                    ssView2_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();
                    if (ArgDept == "EN")
                    {
                        ssView2_Sheet1.Cells[i, 5].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                    }

                    switch (dt.Rows[i]["ORDERSITE"].ToString().Trim())
                    {
                        case "OPDX":
                            ssView2_Sheet1.Cells[i, 7].Text = "수납";
                            break;
                        case "OPD":
                            ssView2_Sheet1.Cells[i, 7].Text = "";
                            break;
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        string ReadSlipCode(string strArg)
        {
            string strVal = "";
            int i = 0;
            string strTemp = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt2 = null;

            try
            {
                SQL = "";
                SQL = "SELECT SUCODE ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_OPD_SLIPVIEWCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE DEPTCODE = '" + strArg + "' ";

                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strVal;
                }
                if (dt2.Rows.Count > 0)
                {
                    for (i = 0; i < dt2.Rows.Count; i++)
                    {
                        strTemp = strTemp + "'" + dt2.Rows[i]["SUCODE"].ToString().Trim() + "',";
                    }

                    strTemp = VB.Mid(strTemp, 1, VB.Len(strTemp) - 1);
                }

                dt2.Dispose();
                dt2 = null;

                if (strTemp == "")
                {
                    strTemp = "''";
                }

                strVal = strTemp;
                return strVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return strVal;
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            int i = 0;

            string strDept = "";
            string strSuCode = "";
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);


            try
            {
                strDept = VB.Left(cboDept.Text, 2);

                if (strDept == "")
                {
                    ComFunc.MsgBox("진료과가 선택되지 않았습니다.", "확인");
                    return;
                }

                SQL = "";
                SQL = " DELETE " + ComNum.DB_PMPA + "NUR_OPD_SLIPVIEWCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE DEPTCODE = '" + strDept + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                for (i = 0; i < ssView1_Sheet1.RowCount; i++)
                {
                    strSuCode = ssView1_Sheet1.Cells[i, 0].Text.Trim();

                    if (strSuCode != "")
                    {

                        SQL = "";
                        SQL = " INSERT INTO " + ComNum.DB_PMPA + "NUR_OPD_SLIPVIEWCODE(DEPTCODE, SUCODE) VALUES (";
                        SQL = SQL + ComNum.VBLF + "'" + strDept + "','" + ssView1_Sheet1.Cells[i, 0].Text.Trim() + "')";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                ViewCode(strDept);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            string strFont1 = "";
            string strHead1 = "";

            strFont1 = "/fn\"굴림\" /fz\"18\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strHead1 = "/f1/c" + cboDept.Text.Trim() + " 특정코드 내역" + "/f1/n";

            ssView2_Sheet1.PrintInfo.Header = strFont1 + strHead1;
            ssView2_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView2_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView2_Sheet1.PrintInfo.Margin.Top = 50;
            ssView2_Sheet1.PrintInfo.Margin.Bottom = 0;
            ssView2_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView2_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView2_Sheet1.PrintInfo.ShowBorder = true;
            ssView2_Sheet1.PrintInfo.ShowColor = false;
            ssView2_Sheet1.PrintInfo.ShowGrid = true;
            ssView2_Sheet1.PrintInfo.ShowShadows = false;
            ssView2_Sheet1.PrintInfo.UseMax = false;
            ssView2_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView2.PrintSheet(0);
        }

        void ViewSlip(string ArgDept, string ArgDate, string ArgDate2)
        {

            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                ssView2_Sheet1.RowCount = 0;

                if (ArgDept == "EN")
                {
                    SQL = "";
                    SQL = "SELECT A.PANO,A.BDATE, B.SNAME, SUM(A.QTY*NAL) CNT, A.SUCODE, C.SUNAMEK";
                }
                else
                {
                    SQL = "";
                    SQL = "SELECT A.PANO, B.SNAME, SUM(A.QTY*NAL) CNT, A.SUCODE, C.SUNAMEK";
                }

                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_SLIP A, " + ComNum.DB_PMPA + "BAS_PATIENT B, " + ComNum.DB_PMPA + "BAS_SUN C";
                SQL = SQL + ComNum.VBLF + " WHERE BDATE >= TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "     AND BDATE <= TO_DATE('" + ArgDate2 + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "     AND A.DEPTCODE = '" + ArgDept + "'";
                SQL = SQL + ComNum.VBLF + "     AND A.PANO = B.PANO";
                SQL = SQL + ComNum.VBLF + "     AND A.SUNEXT = C.SUNEXT";
                SQL = SQL + ComNum.VBLF + "     AND A.SUNEXT IN (" + ReadSlipCode(ArgDept) + ")";

                if (ArgDept == "EN")
                {
                    SQL = SQL + ComNum.VBLF + "  GROUP BY A.PANO,A.BDATE,B.SNAME, A.SUCODE, C.SUNAMEK";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  GROUP BY A.PANO,B.SNAME, A.SUCODE, C.SUNAMEK";
                }

                SQL = SQL + ComNum.VBLF + "  ORDER BY A.PANO ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView2_Sheet1.RowCount = dt.Rows.Count;
                    ssView2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["CNT"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();

                        if (ArgDept == "EN")
                        {
                            ssView2_Sheet1.Cells[i, 5].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        }
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        void cboDept_Click(object sender, EventArgs e)
        {
            if (VB.Left(cboDept.Text, 2) == "EN")
            {
                ssView2_Sheet1.ColumnHeader.Cells[0, 7].Text = "참고사항";
                ssView2_Sheet1.Columns[5].Visible = true;
            }
            else
            {
                ssView2_Sheet1.ColumnHeader.Cells[0, 7].Text = "외래수납여부";
                ssView2_Sheet1.Columns[5].Visible = false;
            }

            ViewCode(VB.Left(cboDept.Text, 2));
            ViewSlip(VB.Left(cboDept.Text, 2), dtpDate1.Value.ToString("yyyy-MM-dd").Trim(), dtpDate2.Value.ToString("yyyy-MM-dd").Trim());
        }

        //TODO: Dim CallTextView As clsCallEmrView
        void ssView2_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            string strPano = "";

            switch (e.Column)
            {
                case 6:
                    strPano = ssView2_Sheet1.Cells[e.Row, 0].Text;

                    //Dim CallTextView As clsCallEmrView
                    //Set CallTextView = New clsCallEmrView
                    //Call CallTextView.EXECUTE_TextEmrView(strPano, GnJobSabun)
                    //Set CallTextView = Nothing
                    break;
            }
        }

        private void ssView1_EditModeOff(object sender, EventArgs e)
        {

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, 1].Text = "";

                SQL = "";
                SQL = "SELECT SUNAMEK ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_SUN ";
                SQL = SQL + ComNum.VBLF + " WHERE SUNEXT = '" + ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, 0].Text.Trim() + "' ";

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

                ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, 1].Text = dt.Rows[0]["SUNAMEK"].ToString().Trim();

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }
    }
}
