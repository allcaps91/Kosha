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
    /// File Name       : frmPmpaViewMonthMisu.cs
    /// Description     : 월말현재 미수금 명단 조회
    /// Author          : 박창욱
    /// Create Date     : 2017-09-26
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\MUMAIN08.FRM(FrmMonthMisuView.frm) >> frmPmpaViewMonthMisu.cs 폼이름 재정의" />	
    public partial class frmPmpaViewMonthMisu : Form
    {
        clsPmpaMisu cpm = new clsPmpaMisu();
        clsSpread CS = new clsSpread();
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;

        //1.건당   2.전체합계
        double[,] nTotAmt = new double[3, 8];
        double GnWRTNO = 0;

        public frmPmpaViewMonthMisu()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int k = 0;
            int nREAD = 0;
            int nRow = 0;
            int nIlsu = 0;
            string strNewData = "";
            string strOldData = "";
            string strYYMM = "";
            string strGDate = "";
            double nSlipJan = 0;

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
            strGDate = clsVbfunc.LastDay((int)VB.Val(VB.Left(strYYMM, 4)), (int)VB.Val(VB.Right(strYYMM, 2)));

            btnSearch.Enabled = false;
            btnPrint.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;

            //누적용 배열 clear
            for (i = 0; i < 3; i++)
            {
                for (k = 0; k < 8; k++)
                {
                    nTotAmt[i, k] = 0;
                }
            }

            try
            {
                //발생일별 미수 상세내역 Display
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT a.WRTNO, a.Class, a.GelCode,";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(b.Bdate,'YYYY-MM-DD') Bdate, b.MisuID, a.IpdOpd,";
                SQL = SQL + ComNum.VBLF + "        b.Bun, b.JepsuNo, a.IwolAmt,";
                SQL = SQL + ComNum.VBLF + "        a.MisuAmt, a.IpgumAmt, a.SakAmt + a.SakAmt2 SakAmt,";
                SQL = SQL + ComNum.VBLF + "        a.BanAmt, a.EtcAmt, a.JanAmt,";
                SQL = SQL + ComNum.VBLF + "        B.QTY1";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_MONTHLY a, " + ComNum.DB_PMPA + "MISU_IDMST b";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND a.YYMM='" + strYYMM + "'";
                if (rdoJob0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.JanAmt <> 0";
                }
                if (VB.Left(cboIpdOpd.Text, 1) != "*")
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.IPDOPD = '" + VB.Left(cboIpdOpd.Text, 1) + "'";
                }
                if (VB.Left(cboClass.Text, 2) != "00")
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Class = '" + VB.Left(cboClass.Text, 2) + "'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Class < '05'";   //전체
                }
                SQL = SQL + ComNum.VBLF + "    AND a.WRTNO=b.WRTNO";
                if (VB.Left(cboTongGbn.Text, 1) == "1")
                {
                    SQL = SQL + ComNum.VBLF + "    AND b.TONGGBN NOT IN ( '2' )";
                }
                if (VB.Left(cboTongGbn.Text, 1) == "2")
                {
                    SQL = SQL + ComNum.VBLF + "    AND b.TONGGBN IN ('2')";
                }

                if (rdoSort0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY b.BDate, a.Class, a.GelCode, b.MisuID, a.IpdOpd, b.Bun ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY b.BDate, b.MisuID, a.Class, a.GelCode, a.IpdOpd, b.Bun ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    btnPrint.Enabled = true;
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    btnSearch.Enabled = true;
                    btnPrint.Enabled = true;
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                strOldData = VB.Space(18);

                nREAD = dt.Rows.Count;

                for (i = 0; i < nREAD; i++)
                {
                    nIlsu = (int)VB.DateDiff("D", Convert.ToDateTime(dt.Rows[i]["Bdate"].ToString().Trim()), Convert.ToDateTime(strGDate));
                    strNewData = dt.Rows[i]["Bdate"].ToString().Trim();
                    nRow += 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    if (strNewData != strOldData)
                    {
                        ssView_Sheet1.Cells[nRow - 1, 0].Text = strNewData;
                        strOldData = strNewData;
                    }

                    ssView_Sheet1.Cells[nRow - 1, 1].Text = cpm.READ_BAS_MIA(dt.Rows[i]["GelCode"].ToString().Trim());
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = cpm.READ_MisuClass(dt.Rows[i]["Class"].ToString().Trim());
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["MisuID"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = cpm.READ_MisuIpdOpd(dt.Rows[i]["IpdOpd"].ToString().Trim());
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = cpm.READ_MisuBunya(dt.Rows[i]["Bun"].ToString().Trim());

                    nTotAmt[1, 1] = VB.Val(dt.Rows[i]["IwolAmt"].ToString().Trim());
                    nTotAmt[1, 2] = VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim());
                    nTotAmt[1, 3] = VB.Val(dt.Rows[i]["IpgumAmt"].ToString().Trim());
                    nTotAmt[1, 4] = VB.Val(dt.Rows[i]["SakAmt"].ToString().Trim());
                    nTotAmt[1, 5] = VB.Val(dt.Rows[i]["BanAmt"].ToString().Trim());
                    nTotAmt[1, 6] = VB.Val(dt.Rows[i]["EtcAmt"].ToString().Trim());
                    nTotAmt[1, 7] = VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim());

                    nTotAmt[2, 1] += VB.Val(dt.Rows[i]["IwolAmt"].ToString().Trim());
                    nTotAmt[2, 2] += VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim());
                    nTotAmt[2, 3] += VB.Val(dt.Rows[i]["IpgumAmt"].ToString().Trim());
                    nTotAmt[2, 4] += VB.Val(dt.Rows[i]["SakAmt"].ToString().Trim());
                    nTotAmt[2, 5] += VB.Val(dt.Rows[i]["BANAmt"].ToString().Trim());
                    nTotAmt[2, 6] += VB.Val(dt.Rows[i]["EtcAmt"].ToString().Trim());
                    nTotAmt[2, 7] += VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim());

                    ssView_Sheet1.Cells[nRow - 1, 6].Text = nIlsu.ToString("##,##0");
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = VB.Val(dt.Rows[i]["Qty1"].ToString().Trim()).ToString("###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = nTotAmt[1, 1].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 9].Text = nTotAmt[1, 2].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 10].Text = nTotAmt[1, 3].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 11].Text = nTotAmt[1, 4].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 12].Text = nTotAmt[1, 5].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 13].Text = nTotAmt[1, 6].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 14].Text = nTotAmt[1, 7].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 15].Text = dt.Rows[i]["WRTNO"].ToString().Trim();


                    //MISU_SLIP의 월말현재 잔액을 계산
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT Gubun SlipGbn, SUM(Amt) SlipAmt";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_SLIP";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "    AND WRTNO=" + dt.Rows[i]["WRTNO"].ToString().Trim();
                    SQL = SQL + ComNum.VBLF + "    AND BDate<=TO_DATE('" + strGDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "    AND Gubun>='11'"; //01-10번은 메모계정
                    SQL = SQL + ComNum.VBLF + "  GROUP BY Gubun";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        btnSearch.Enabled = true;
                        btnPrint.Enabled = true;
                        Cursor.Current = Cursors.Default;
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    nSlipJan = 0;
                    for (k = 0; k < dt1.Rows.Count; k++)
                    {
                        if (VB.Val(dt1.Rows[k]["SlipGbn"].ToString().Trim()) >= 11 && VB.Val(dt1.Rows[k]["SlipGbn"].ToString().Trim()) <= 19)
                        {
                            nSlipJan += VB.Val(dt1.Rows[k]["SlipAmt"].ToString().Trim());   //청구
                        }
                        else if (VB.Val(dt1.Rows[k]["SlipGbn"].ToString().Trim()) >= 21 && VB.Val(dt1.Rows[k]["SlipGbn"].ToString().Trim()) <= 99)
                        {
                            nSlipJan -= VB.Val(dt1.Rows[k]["SlipAmt"].ToString().Trim());   //입금, 삭감
                        }
                    }
                    dt1.Dispose();
                    dt1 = null;

                    //Misu_Monthly와 Misu_SLIP의 잔액을 비교
                    if (nTotAmt[1, 7] != nSlipJan)
                    {
                        ssView_Sheet1.Cells[nRow - 1, 14].Text = "▶오류◀";
                    }

                }
                dt.Dispose();
                dt = null;

                //전체합계
                nRow += 1;
                ssView_Sheet1.RowCount = nRow;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                ssView_Sheet1.Cells[nRow - 1, 1].Text = "** 전체합계 **";
                ssView_Sheet1.Cells[nRow - 1, 8].Text = nTotAmt[2, 1].ToString("###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 9].Text = nTotAmt[2, 2].ToString("###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 10].Text = nTotAmt[2, 3].ToString("###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 11].Text = nTotAmt[2, 4].ToString("###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 12].Text = nTotAmt[2, 5].ToString("###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 13].Text = nTotAmt[2, 6].ToString("###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 14].Text = nTotAmt[2, 7].ToString("###,###,###,##0");


                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
                Cursor.Current = Cursors.Default;

                lblMsg.Text = "▶해당 줄을 더블클릭하면 상세내역 조회";


            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
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

            strTitle = "현재 미수자 명부(" + cboClass.Text + ":" + cboIpdOpd.Text + ":" + cboTongGbn.Text + ")";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력자 : " + clsType.User.JobName + " 인 " + VB.Space(8), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void frmPmpaViewMonthMisu_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ssView_Sheet1.Columns[15].Visible = false;

            clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM, 60, "", "1");
            cboYYMM.SelectedIndex = 0;
            lblMsg.Text = "";

            cboClass.Items.Clear();
            cboClass.Items.Add("00.보험전체");
            cboClass.Items.Add("01.공단");
            cboClass.Items.Add("02.직장");
            cboClass.Items.Add("03.지역");
            cboClass.Items.Add("04.의료급여");
            cboClass.Items.Add("05.산재");
            cboClass.Items.Add("07.자보");
            cboClass.SelectedIndex = 0;

            cboIpdOpd.Items.Clear();
            cboIpdOpd.Items.Add("*.전체");
            cboIpdOpd.Items.Add("I.입원");
            cboIpdOpd.Items.Add("O.외래");
            cboIpdOpd.SelectedIndex = 0;

            cboTongGbn.Items.Clear();
            cboTongGbn.Items.Add("*.전체");
            cboTongGbn.Items.Add("1.퇴원 + 기타");
            cboTongGbn.Items.Add("2.중간");
            cboTongGbn.SelectedIndex = 0;
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

            GnWRTNO = VB.Val(ssView_Sheet1.Cells[e.Row, 15].Text);

            frmPmpaViewPanoMisuDtl f = new frmPmpaViewPanoMisuDtl(GnWRTNO);

            f.Show();
        }
    }
}
