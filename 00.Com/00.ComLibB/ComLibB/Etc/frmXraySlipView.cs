using System;
using System.Data;
using System.Windows.Forms;
using ComBase;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : 개인별 진료 내역 조회
    /// Description     : frmXraySlipView
    /// Author          : 전상원
    /// Create Date     : 2018-04-20
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= " \xray\xumain\xumain.vbp(FrmSlipView_new) >> frmXraySlipView.cs 폼이름 재정의" />
    public partial class frmXraySlipView : Form
    {
        int[] nArrSeqno = new int[201];
        string[] strArrDate = new string[201];
        string[] strArrDept = new string[201];
        string[] strArrBi = new string[201];
        string strFlagChange = "";

        //PSMHVB\xray\xumain\FrmSlipView_new.FRM
        public frmXraySlipView()
        {
            InitializeComponent();
        }

        private void frmXraySlipView_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            lblID1.Text = "";
            lblID2.Text = "";
            lblID3.Text = "";
            lblID4.Text = "";
            lblID5.Text = "";

            lblGajeng.Text = "";

            txtPano.Text = "";
            ssView_Sheet1.RowCount = 0;
            strFlagChange = "";

            if (clsPublic.GstrPANO != "")
            {
                txtPano.Text = clsPublic.GstrPANO;
                strFlagChange = "";
                txtPano.TabIndex = 1;
                ssView.TabIndex = 0;
                ssView_Sheet1.Visible = true;
                Read_PAT_MAST();
                Read_OS_JINRYO();
                Read_Ipd_new_master();
            }

            ssView1_Sheet1.RowCount = 0;
            ssView2_Sheet1.RowCount = 0;
        }

        private void Read_OPD_SLIP(int ArgIndex)
        {
            int i = 0;

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssView1_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT  Sucode, SunameK, BaseAmt, Qty, Nal, GbSlip,";
                SQL = SQL + ComNum.VBLF + "        GbSpc, GbNgt, GbGisul, GbSelf, GbChild,";
                SQL = SQL + ComNum.VBLF + "        Amt1, Amt2, SeqNo, Part,OrderNo ";
                SQL = SQL + ComNum.VBLF + "  FROM  OPD_SLIP O,  BAS_SUN B";
                SQL = SQL + ComNum.VBLF + " WHERE  ActDate  = TO_DATE('" + VB.Trim(ssView_Sheet1.Cells[ArgIndex, 0].Text) + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND  Pano     = '" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + "   AND  Bi       = '" + strArrBi[ArgIndex] + "' ";
                SQL = SQL + ComNum.VBLF + "   AND  DeptCode = '" + VB.Trim(ssView_Sheet1.Cells[ArgIndex, 1].Text) + "' ";
                SQL = SQL + ComNum.VBLF + "   AND  SeqNo    = '" + VB.Trim(ssView_Sheet1.Cells[ArgIndex, 4].Text) + "' ";
                SQL = SQL + ComNum.VBLF + "   AND  O.Sunext = B.Sunext ";
                if (chkXray.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND  ((O.Bun    > '64' AND  O.Bun    < '74') OR ( O.BUN='78' OR  O.BUN='41' OR  O.BUN='47'  ) )    ";
                }
                SQL = SQL + ComNum.VBLF + " ORDER  BY  SeqNo, Part, O.Bun, O.Sucode, O.Sunext";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView1_Sheet1.RowCount = i + 1;
                        
                        ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["Sucode"].ToString().Trim();
                        ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["SunameK"].ToString().Trim();
                        ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 2].Text = VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("##,###,##0");
                        ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 3].Text = VB.Val(dt.Rows[i]["QTY"].ToString().Trim()).ToString("##0");
                        ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 4].Text = VB.Val(dt.Rows[i]["Nal"].ToString().Trim()).ToString("##0");
                        ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["GbSpc"].ToString().Trim();
                        ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["GbNgt"].ToString().Trim();
                        ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 7].Text = dt.Rows[i]["GbGisul"].ToString().Trim();
                        ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 8].Text = dt.Rows[i]["GbSelf"].ToString().Trim();
                        ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 9].Text = dt.Rows[i]["GbChild"].ToString().Trim();
                        ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 10].Text = VB.Val(dt.Rows[i]["Amt1"].ToString().Trim()).ToString("##,###,##0");
                        ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 11].Text = VB.Val(dt.Rows[i]["Amt2"].ToString().Trim()).ToString("####,##0");
                        ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 12].Text = VB.Val(dt.Rows[i]["SeqNo"].ToString().Trim()).ToString("###0");
                        ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 13].Text = dt.Rows[i]["Part"].ToString().Trim();

                        if (dt.Rows[i]["GbSlip"].ToString().Trim() == "G")
                        {
                            lblGajeng.Text = "(가정간호)";
                        }

                        ssView1_Sheet1.Cells[i, 14].Text = dt.Rows[i]["OrderNo"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void Read_OS_JINRYO()
        {
            int i = 0;
            string strBi = "";
            int nTemp = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(ActDate,'YYYY-MM-DD') Adate, ";
                SQL = SQL + ComNum.VBLF + "       DeptCode, DrName, Bi,Seqno ";
                SQL = SQL + ComNum.VBLF + "  FROM OPD_SLIP O, BAS_DOCTOR B ";
                SQL = SQL + ComNum.VBLF + " WHERE Pano     = '" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + "  AND O.DrCode = B.DrCode(+) ";
                if (chkXray.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND  ((O.Bun    > '64' AND  O.Bun    < '74') OR ( O.BUN='78' OR  O.BUN='41' OR  O.BUN='47'  ) )     ";
                }
                SQL = SQL + ComNum.VBLF + " GROUP BY ActDate, DeptCode, DrName, Bi,Seqno ";
                SQL = SQL + ComNum.VBLF + " ORDER BY ActDate Desc";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssView_Sheet1.RowCount = 0;

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows.Count > 200)
                    {
                        nTemp = 200;
                        ssView_Sheet1.RowCount = nTemp;
                    }
                    else
                    {
                        nTemp = dt.Rows.Count;
                        ssView_Sheet1.RowCount = nTemp;
                    }

                    for (i = 0; i < nTemp; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Adate"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DrName"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = VB.Val(dt.Rows[i]["SeqNo"].ToString().Trim()).ToString("0000");
                        strArrBi[i] = dt.Rows[i]["Bi"].ToString().Trim();

                        switch (dt.Rows[i]["Bi"].ToString().Trim())
                        {
                            case "11":
                            case "12":
                            case "13":
                            case "14":
                            case "15":
                                strBi = "보험";
                                break;
                            case "21":
                            case "22":
                            case "23":
                            case "24":
                            case "25":
                                strBi = "보호";
                                break;
                            case "31":
                            case "32":
                            case "33":
                            case "34":
                            case "35":
                                strBi = "산재";
                                break;
                            case "41":
                            case "42":
                            case "43":
                            case "44":
                            case "45":
                                strBi = "보험100%";
                                break;
                            case "52":
                                strBi = "자보";
                                break;
                            case "55":
                                strBi = "자보일반";
                                break;
                            default:
                                strBi = "일반";
                                break;
                        }

                        ssView_Sheet1.Cells[i, 3].Text = strBi;
                    }

                    ssView.Enabled = true;
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void Read_Ipd_new_master()
        {
            int i = 0;
            int nREAD = 0;

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //환자명단을 SELECT
                SQL = "";
                SQL = "SELECT  Pano, SName, DeptCode, DrCode, DeCode(GbSTS,'0','재원중','9','입원취소','7','퇴원','퇴원중') GbSTS, Bi,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(InDate,'YYYY-MM-DD') InDate, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(OutDate,'YYYY-MM-DD') OutDate ";
                SQL = SQL + ComNum.VBLF + " FROM IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE Pano='" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY InDate DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nREAD = dt.Rows.Count;

                ssView2_Sheet1.RowCount = 0;
                ssView2_Sheet1.RowCount = nREAD;

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["InDate"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["OutDate"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 6].Text = dt.Rows[i]["GbSTS"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void Read_PAT_MAST()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(StartDate, 'YYYY-MM-DD') Sdate,";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(LastDate, 'YYYY-MM-DD') Ldate, ";
                SQL = SQL + ComNum.VBLF + "       Sname, Sex, Jumin1, Jumin2,Jumin3 ";
                SQL = SQL + ComNum.VBLF + "  FROM BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE Pano = '" + txtPano.Text + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    lblID1.Text = dt.Rows[0]["Sname"].ToString().Trim();
                    lblID2.Text = dt.Rows[0]["Sex"].ToString().Trim();
                    lblID3.Text = dt.Rows[0]["Jumin1"].ToString().Trim() + " - ";

                    if (dt.Rows[0]["Jumin3"].ToString().Trim() != "")
                    {
                        lblID3.Text = lblID3.Text + clsAES.DeAES(dt.Rows[0]["Jumin3"].ToString().Trim());
                    }
                    else
                    {
                        lblID3.Text = lblID3.Text + dt.Rows[0]["Jumin2"].ToString().Trim();
                    }

                    lblID4.Text = dt.Rows[0]["Sdate"].ToString().Trim();
                    lblID5.Text = dt.Rows[0]["Ldate"].ToString().Trim();
                    lblGajeng.Text = "";
                }
                else
                {
                    ComFunc.MsgBox("데이터가 없습니다. 확인하세요!!!", "확인");
                    txtPano.Focus();
                    dt.Dispose();
                    dt = null;
                    return;
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPano.Text = VB.Val(txtPano.Text).ToString("00000000");

                if (VB.IsNumeric(txtPano.Text))
                {
                    Read_PAT_MAST();
                    Read_OS_JINRYO();
                    Read_Ipd_new_master();
                }
            }
        }

        private void txtPano_TextChanged(object sender, EventArgs e)
        {
            strFlagChange = "**";

            if (VB.Val(lblID1.Text) > VB.Val(""))
            {
                lblID1.Text = "";
                lblID2.Text = "";
                lblID3.Text = "";
                lblID4.Text = "";
                lblID5.Text = "";

                ssView_Sheet1.RowCount = 0;
                ssView.Enabled = false;
            }
        }

        private void txtPano_Enter(object sender, EventArgs e)
        {
            txtPano.SelectionStart = 0;
            txtPano.SelectionLength = VB.Len(txtPano.Text);
        }

        private void txtPano_Leave(object sender, EventArgs e)
        {
            txtPano.Text = VB.Val(txtPano.Text).ToString("00000000");

            if (VB.IsNumeric(txtPano.Text))
            {
                Read_PAT_MAST();
                Read_OS_JINRYO();
                Read_Ipd_new_master();
            }

            strFlagChange = "";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            clsPublic.GstrPANO = "";
            this.Close();
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if(ssView_Sheet1.RowCount != 0)
            {
                lblGajeng.Text = "";
                Read_OPD_SLIP(e.Row);
            }
            return;
        }
    }
}