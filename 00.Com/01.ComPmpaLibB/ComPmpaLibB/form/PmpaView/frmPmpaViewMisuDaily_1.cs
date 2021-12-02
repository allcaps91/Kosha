using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewMisuDaily_1.cs
    /// Description     : 미수발생 입금내역 조회
    /// Author          : 박창욱
    /// Create Date     : 2017-09-21
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\MISUP205_1.FRM(FrmMisuDaily_1.frm) >> frmPmpaViewMisuDaily_1.cs 폼이름 재정의" />	
    public partial class frmPmpaViewMisuDaily_1 : Form
    {
        clsSpread CS = new clsSpread();
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsPmpaFunc cpf = new clsPmpaFunc();
        clsPmpaMisu cpm = new clsPmpaMisu();
        string GstrPano = "";

        public frmPmpaViewMisuDaily_1()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
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


            strTitle = dtpFDate.Value.ToString("yyyy-MM-dd") + "~" + dtpTDate.Value.ToString("yyyy-MM-dd") + "일까지 미수 변동내역";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("작 성 자 : " + clsType.User.JobMan, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력시간 : " + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int k = 0;
            int nRow = 0;
            int nREAD = 0;
            int nREAD2 = 0;
            double nIDno = 0;
            double nTotMisu = 0;
            double nTotIpgum = 0;
            string strMisudtl = "";
            string strADD = "";

            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;
            btnPrint.Enabled = false;
            btnExit.Enabled = false;

            ssView_Sheet1.RowCount = 0;
            nRow = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT To_Char(Bdate,'YYYY-MM-DD') Bdate, Pano, Gubun1,";
                SQL = SQL + ComNum.VBLF + "        Gubun2, Amt, Remark,";
                SQL = SQL + ComNum.VBLF + "        Idno, Misudtl, FLAG,";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(EntTime,'YYYY-MM-DD HH24:MI') EntTime";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                if (txtPano.Text.Trim() != "")
                {
                    SQL = SQL + ComNum.VBLF + "    AND Pano ='" + txtPano.Text + "' ";
                }
                SQL = SQL + ComNum.VBLF + "    AND BDate >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND BDate <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND GUBUN1 = '1'";
                SQL = SQL + ComNum.VBLF + "  ORDER BY Pano,Bdate,Gubun1";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    btnSearch.Enabled = true;
                    btnPrint.Enabled = true;
                    btnExit.Enabled = true;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    btnSearch.Enabled = true;
                    btnPrint.Enabled = true;
                    btnExit.Enabled = true;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nREAD = dt.Rows.Count;
                nTotMisu = 0;
                nTotIpgum = 0;

                for (i = 0; i < nREAD; i++)
                {
                    nRow += 1;
                    if (ssView_Sheet1.RowCount < nRow)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    ssView_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["Bdate"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = cpf.Get_BasPatient(clsDB.DbCon, dt.Rows[i]["Pano"].ToString().Trim()).Rows[0]["Sname"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = VB.Val(dt.Rows[i]["Amt"].ToString().Trim()).ToString("###,###,###,##0");
                    nTotMisu = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = cpm.READ_BuseName(dt.Rows[i]["Gubun2"].ToString().Trim());
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["Remark"].ToString().Trim();
                    strMisudtl = ComFunc.LeftH(dt.Rows[i]["Misudtl"].ToString().Trim() + VB.Space(30), 30);
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = ComFunc.LeftH(strMisudtl, 1);
                    ssView_Sheet1.Cells[nRow - 1, 9].Text = VB.Mid(strMisudtl, 2, 2);
                    ssView_Sheet1.Cells[nRow - 1, 10].Text = cpm.READ_PerMisuGye(VB.Mid(strMisudtl, 4, 2));
                    ssView_Sheet1.Cells[nRow - 1, 11].Text = VB.Val(ComFunc.MidH(strMisudtl, 6, 9)).ToString("###,###,###,###");
                    ssView_Sheet1.Cells[nRow - 1, 12].Text = ComFunc.MidH(strMisudtl, 15, 8).Trim();
                    ssView_Sheet1.Cells[nRow - 1, 13].Text = VB.Right(strMisudtl, 8).Trim();

                    strADD = dt.Rows[i]["FLAG"].ToString().Trim();
                    if (strADD == "*")
                    {
                        ssView_Sheet1.Cells[nRow - 1, 14].Text = "수납";
                    }

                    nIDno = VB.Val(dt.Rows[i]["Idno"].ToString().Trim());
                    ssView_Sheet1.Cells[nRow - 1, 15].Text = clsVbfunc.GetPassName(clsDB.DbCon, nIDno.ToString("#####0"));


                    //입금완료건이면 입금완료된 내역을 DISPLAY
                    if (strADD == "*")
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT To_Char(Bdate,'YYYY-MM-DD') Bdate, Pano, Gubun1, Gubun2,  Amt, ";
                        SQL = SQL + ComNum.VBLF + "       Remark,Idno,Misudtl,FLAG,                                      ";
                        SQL = SQL + ComNum.VBLF + "       TO_CHAR(EntTime,'YYYY-MM-DD HH24:MI') EntTime                  ";
                        SQL = SQL + ComNum.VBLF + "  From MISU_GAINSLIP ";
                        SQL = SQL + ComNum.VBLF + " Where Pano = '" + dt.Rows[i]["Pano"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND GUBUN1 <> '1' ";
                        SQL = SQL + ComNum.VBLF + "   AND BDATE>=TO_DATE('" + dt.Rows[i]["Bdate"].ToString().Trim() + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND Flag = '*' ";
                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            btnSearch.Enabled = true;
                            btnPrint.Enabled = true;
                            btnExit.Enabled = true;
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        nREAD2 = dt1.Rows.Count;
                        if (nREAD2 > 0)
                        {
                            for (k = 0; k < nREAD2; k++)
                            {
                                nTotIpgum = 0;

                                nRow += 1;
                                if (ssView_Sheet1.RowCount < nRow)
                                {
                                    ssView_Sheet1.RowCount = nRow;
                                }

                                ssView_Sheet1.Cells[nRow - 1, 0].Text = dt1.Rows[k]["Bdate"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 1].Text = dt1.Rows[k]["Pano"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 2].Text = cpf.Get_BasPatient(clsDB.DbCon, dt1.Rows[k]["Pano"].ToString().Trim()).Rows[0]["Sname"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 4].Text = VB.Val(dt1.Rows[k]["Amt"].ToString().Trim()).ToString("###,###,###,##0");
                                nTotIpgum += VB.Val(dt1.Rows[k]["Amt"].ToString().Trim());
                                nTotMisu -= nTotIpgum;
                                ssView_Sheet1.Cells[nRow - 1, 5].Text = nTotMisu.ToString("###,###,###,##0");
                                ssView_Sheet1.Cells[nRow - 1, 6].Text = cpm.READ_BuseName((dt1.Rows[k]["Gubun2"].ToString().Trim()));
                                ssView_Sheet1.Cells[nRow - 1, 7].Text = dt1.Rows[k]["Remark"].ToString().Trim();
                                strMisudtl = ComFunc.LeftH(dt1.Rows[k]["Misudtl"].ToString().Trim() + VB.Space(30), 30);
                                ssView_Sheet1.Cells[nRow - 1, 8].Text = ComFunc.LeftH(strMisudtl, 1);
                                ssView_Sheet1.Cells[nRow - 1, 9].Text = ComFunc.MidH(strMisudtl, 2, 2);
                                ssView_Sheet1.Cells[nRow - 1, 10].Text = cpm.READ_PerMisuGye(ComFunc.MidH(strMisudtl, 4, 2));
                                ssView_Sheet1.Cells[nRow - 1, 11].Text = VB.Val(ComFunc.MidH(strMisudtl, 6, 9)).ToString("###,###,###,###");
                                ssView_Sheet1.Cells[nRow - 1, 12].Text = ComFunc.MidH(strMisudtl, 15, 8).Trim();
                                ssView_Sheet1.Cells[nRow - 1, 13].Text = VB.Right(strMisudtl, 8).Trim();
                            }
                        }
                        dt1.Dispose();
                        dt1 = null;
                    }
                }
                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
                btnExit.Enabled = true;

            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
                btnExit.Enabled = true;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void frmPmpaViewMisuDaily_1_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //this.Close(); //폼 권한 조회
            //return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            dtpFDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).AddDays(-1);
            dtpTDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
            txtPano.Text = "";
        }

        private void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssView, e.Column);
                return;
            }

            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssView_Sheet1.Cells[e.Row, 0, e.Row, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            GstrPano = ssView_Sheet1.Cells[e.Row, 1].Text;

            //TODO : 폼 호출
            //FrmMisu.Show
        }

        private void txtPano_Leave(object sender, EventArgs e)
        {
            if (txtPano.Text == "")
            {
                return;
            }
            txtPano.Text = VB.Val(txtPano.Text).ToString("00000000");
        }
    }
}
