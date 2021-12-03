using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComNurLibB
{
    /// <summary>   
    /// File Name       : frmOpdNrSlipView.cs
    /// Description     : 외래 진료과별 특정코드 수납내역 확인
    /// Author          : 유진호
    /// Create Date     : 2017-01-25
    /// <history>       
    /// D:\포항성모병원 VB Source(2017.11.20)\emr\emrprt\FrmSlipView
    /// </history>
    /// </summary>
    public partial class frmOpdNrSlipView : Form
    {
        public frmOpdNrSlipView()
        {
            InitializeComponent();
        }

        private void frmOpdNrSlipView_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ssView2_Sheet1.Columns[5].Visible = false;
            
            set_Combo();

            ComFunc.ReadSysDate(clsDB.DbCon);
            dtpFDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
            dtpTDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
        }

        private void set_Combo()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                cboDEPT.Items.Clear();

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT DEPTCODE, DEPTNAMEK";
                SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_PMPA.BAS_CLINICDEPT";
                SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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
                        cboDEPT.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim() + "." + dt.Rows[i]["DEPTNAMEK"].ToString().Trim());                        
                    }
                }

                cboDEPT.SelectedIndex = 0;

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
        
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인            
            btnPrintClick();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인            
            btnSaveClick();            
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            btnSearchClick();
        }

        private void btnSearchClick()
        {
            if (chkIpwon.Checked == true)
            {
                VIEW_SLIP_IPD(VB.Left(cboDEPT.Text, 2), dtpFDate.Text, dtpTDate.Text);
            }
            else
            {
                VIEW_SLIP(VB.Left(cboDEPT.Text, 2), dtpFDate.Text, dtpTDate.Text);
            }            
        }

        private bool btnSaveClick()
        {
            bool rtVal = false;
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strDept = "";
            string strSuCode = "";

            strDept = VB.Left(cboDEPT.Text, 2);
            if (strDept == "")
            {
                ComFunc.MsgBox("진료과가 선택되지 않았습니다.");
                return rtVal;
            }
            
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "DELETE KOSMOS_PMPA.NUR_OPD_SLIPVIEWCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE DEPTCODE = '" + strDept + "' ";
                
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                for (i = 0; i < ssView1_Sheet1.RowCount; i++)
                {
                    strSuCode = ssView1_Sheet1.Cells[i, 0].Text;
                    if (strSuCode != "")
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "INSERT INTO KOSMOS_PMPA.NUR_OPD_SLIPVIEWCODE(DEPTCODE, SUCODE) VALUES (";
                        SQL = SQL + ComNum.VBLF + "'" + strDept + "','" + strSuCode + "')";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return rtVal;
                        }
                    }
                }
                
                clsDB.setCommitTran(clsDB.DbCon);                
                Cursor.Current = Cursors.Default;
                VIEW_CODE(strDept);
                
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        private void VIEW_SLIP(string ArgDept, string ArgDate, string ArgDate2)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                
                ssView2_Sheet1.RowCount = 0;

                SQL = "";
                if (ArgDept == "EN")
                {
                    SQL = SQL + ComNum.VBLF + " SELECT A.PANO,A.BDATE, B.SNAME, SUM(A.QTY*NAL) CNT, A.SUCODE, C.SUNAMEK";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " SELECT A.PANO, B.SNAME, SUM(A.QTY*NAL) CNT, A.SUCODE, C.SUNAMEK";
                }
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_SLIP A, KOSMOS_PMPA.BAS_PATIENT B, KOSMOS_PMPA.BAS_SUN C";
                SQL = SQL + ComNum.VBLF + " WHERE BDATE >= TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "     AND BDATE <= TO_DATE('" + ArgDate2 + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "     AND A.DEPTCODE = '" + ArgDept + "'";
                SQL = SQL + ComNum.VBLF + "     AND A.PANO = B.PANO";
                SQL = SQL + ComNum.VBLF + "     AND A.SUNEXT = C.SUNEXT";
                SQL = SQL + ComNum.VBLF + "     AND A.SUNEXT IN (" + READ_SLIPCODE(ArgDept) + ")";
                if (ArgDept == "EN")
                {
                    SQL = SQL + ComNum.VBLF + "  GROUP BY A.PANO,A.BDATE,B.SNAME, A.SUCODE, C.SUNAMEK";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  GROUP BY A.PANO,B.SNAME, A.SUCODE, C.SUNAMEK";
                }
                SQL = SQL + ComNum.VBLF + "  ORDER BY A.PANO ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
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
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void VIEW_SLIP_IPD(string ArgDept, string ArgDate, string ArgDate2)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                
                ssView2_Sheet1.RowCount = 0;

                SQL = "";
                if (ArgDept == "EN")
                {
                    SQL = SQL + ComNum.VBLF + " SELECT A.PANO, A.BDATE,B.SNAME, SUM(A.QTY*A.NAL) CNT, A.SUCODE, C.SUNAMEK, D.ORDERSITE";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " SELECT A.PANO, B.SNAME, SUM(A.QTY*A.NAL) CNT, A.SUCODE, C.SUNAMEK, D.ORDERSITE";
                }
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.IPD_NEW_SLIP A, KOSMOS_PMPA.BAS_PATIENT B, ";
                SQL = SQL + ComNum.VBLF + "       KOSMOS_PMPA.BAS_SUN C, KOSMOS_OCS.OCS_IORDER D ";
                SQL = SQL + ComNum.VBLF + " WHERE A.BDATE >= TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND A.BDATE <= TO_DATE('" + ArgDate2 + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND A.DEPTCODE = '" + ArgDept + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.PANO = B.PANO";
                SQL = SQL + ComNum.VBLF + "   AND A.SUNEXT = C.SUNEXT";
                SQL = SQL + ComNum.VBLF + "   AND A.SUNEXT IN (" + READ_SLIPCODE(ArgDept) + ")";
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

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
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

        private string READ_SLIPCODE(string arg)
        {
            string rtnVal = "";
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT SUCODE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_OPD_SLIPVIEWCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE DEPTCODE = '" + arg + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }                
                if (dt.Rows.Count > 0)
                {                 
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        rtnVal = rtnVal + "'" + dt.Rows[i]["SUCODE"].ToString().Trim() + "',";
                    }
                    rtnVal = VB.Mid(rtnVal, 1, VB.Len(rtnVal) - 1);
                }
                dt.Dispose();
                dt = null;

                if (rtnVal == "") rtnVal = "''";                
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

            return rtnVal;
        }

        private void cboDEPT_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (VB.Left(cboDEPT.Text, 2) == "EN")
            {
                ssView2_Sheet1.ColumnHeader.Cells[0, 7].Text = "참고사항";                
                ssView2_Sheet1.Columns[5].Visible = false;                
            }
            else
            {
                ssView2_Sheet1.ColumnHeader.Cells[0, 7].Text = "외래수납여부";
                ssView2_Sheet1.Columns[5].Visible = true;                
            }

            VIEW_CODE(VB.Left(cboDEPT.Text, 2));
            VIEW_SLIP(VB.Left(cboDEPT.Text, 2), VB.Trim(dtpFDate.Text), VB.Trim(dtpTDate.Text));
        }

        private void VIEW_CODE(string arg)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                ssView1_Sheet1.RowCount = 0;

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT A.SUCODE, B.SUNAMEK ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_OPD_SLIPVIEWCODE A, KOSMOS_PMPA.BAS_SUN B";
                SQL = SQL + ComNum.VBLF + " WHERE A.SUCODE = B.SUNEXT ";
                SQL = SQL + ComNum.VBLF + "   AND A.DEPTCODE = '" + arg + "' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY A.SUCODE ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }                
                if (dt.Rows.Count > 0)
                {
                    ssView1_Sheet1.RowCount = dt.Rows.Count;
                    ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();
                    }
                }

                ssView1_Sheet1.RowCount = ssView1_Sheet1.RowCount + 10;

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

        private void ssView1_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (e.Row < 0) return;

            try
            {
                ssView1_Sheet1.Cells[e.Row, 1].Text = "";

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT SUNAMEK ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_SUN ";
                SQL = SQL + ComNum.VBLF + " WHERE SUNEXT = '" + ssView1_Sheet1.Cells[e.Row, 0].Text + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }                
                if (dt.Rows.Count > 0)
                {
                    ssView1_Sheet1.Cells[e.Row, 1].Text = dt.Rows[0]["SUNAMEK"].ToString().Trim();
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

        private void ssView2_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            string strPANO = "";

            if (e.Row == 6)
            {
                strPANO = ssView2_Sheet1.Cells[e.Row, 0].Text;

                //EMR 뷰어                
                clsVbEmr.EXECUTE_TextEmrView(strPANO, clsType.User.Sabun);
            }
        }

        private void btnPrintClick()
        {
            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";
            string SysDate = "";

            SysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            Cursor.Current = Cursors.WaitCursor;

            strFont1 = "/fn\"굴림체\" /fz\"16\" /fb0 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"10\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead1 = "/n/n/f1/C 특정코드 내역" + "/n/n/n/n";
            strHead2 = "/l/f2" + "조회기간 : " + dtpFDate.Value.ToString("yyyy-MM-dd") + "~" + dtpTDate.Value.ToString("yyyy-MM-dd") + VB.Space(10) + "인쇄일자 : " + SysDate;

            ssView2_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView2_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssView2_Sheet1.PrintInfo.Margin.Left = 35;
            ssView2_Sheet1.PrintInfo.Margin.Right = 0;
            ssView2_Sheet1.PrintInfo.Margin.Top = 35;
            ssView2_Sheet1.PrintInfo.Margin.Bottom = 30;
            ssView2_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView2_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView2_Sheet1.PrintInfo.ShowBorder = true;
            ssView2_Sheet1.PrintInfo.ShowColor = false;
            ssView2_Sheet1.PrintInfo.ShowGrid = true;
            ssView2_Sheet1.PrintInfo.ShowShadows = false;
            ssView2_Sheet1.PrintInfo.UseMax = false;
            ssView2_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView2.PrintSheet(0);
        }
    }
}
