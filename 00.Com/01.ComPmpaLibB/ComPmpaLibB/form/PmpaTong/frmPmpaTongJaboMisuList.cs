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
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-09-14
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= d:\psmh\misu\misugye.vbp\Frm자보미수통계표1.FRM" >> frmPmpaTongJaboMisuList.cs 폼이름 재정의" />

    public partial class frmPmpaTongJaboMisuList : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaFunc CPF = new clsPmpaFunc();
        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        double[] nAmt = new double[13];
        double[] nTot = new double[13];

        public frmPmpaTongJaboMisuList()
        {
            InitializeComponent();
        }

        private void frmPmpaTongJaboMisuList_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboFDate, 20, "", "1");
            clsVbfunc.SetCboDate(clsDB.DbCon, cboTDate, 20, "", "1");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i = 0;
            int j = 0;
            DataTable dt = null;
            DataTable dtFc2 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int nRow = 0;
            string strFYYMM = "";
            string strTYYMM = "";
            string strFDate = "";
            string strTDate = "";
            string strMianame = "";


            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;

            for (i = 0; i <= 12; i++)
            {
                nTot[i] = 0;
            }

            strFYYMM = VB.Left(cboFDate.Text, 4) + VB.Mid(cboFDate.Text, 7, 2);
            strTYYMM = VB.Left(cboTDate.Text, 4) + VB.Mid(cboTDate.Text, 7, 2);

            strFDate = VB.Left(cboTDate.Text, 4) + "-" + VB.Mid(cboFDate.Text, 7, 2) + "-01";
            strTDate = CF.READ_LASTDAY(clsDB.DbCon, strFDate);
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT YYMM,SUM(IwolAmt) cIwolAmt,                                         ";
                SQL = SQL + ComNum.VBLF + "        SUM(MisuAmt) cMisuAmt,SUM(IpgumAmt) cIpgumAmt,                         ";
                SQL = SQL + ComNum.VBLF + "        SUM(SakAmt) cSakAmt,SUM(BanAmt+EtcAmt) cEtcAmt,SUM(JanAmt) cJanAmt,    ";
                SQL = SQL + ComNum.VBLF + "        SUM(TotMAmt) cTotMAmt,SUM(TotSAmt) cTotSAmt                            ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GELTOT                                      ";
                SQL = SQL + ComNum.VBLF + "  WHERE YYMM >= '" + strFYYMM + "'                                             ";
                SQL = SQL + ComNum.VBLF + "    AND YYMM <= '" + strTYYMM + "'                                             ";

                if (rdojob0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND Class = '07'                                                       ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND Class = '20'                                                       ";
                }

                if (rdoIO1.Checked == true)
                    SQL = SQL + ComNum.VBLF + " AND IPDOPD ='I'   ";

                if (rdoIO2.Checked == true)
                    SQL = SQL + ComNum.VBLF + " AND IPDOPD ='O'   ";

                SQL = SQL + ComNum.VBLF + "  GROUP BY YYMM                                                             ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY YYMM                                                             ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("해당월의 자료가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                nRow = 0;

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nAmt[1] = VB.Val(dt.Rows[i]["CIWOLAMT"].ToString().Trim()); //'이월
                    nAmt[2] = VB.Val(dt.Rows[i]["CMISUAMT"].ToString().Trim());   //'청구
                    nTot[2] = nTot[2] + VB.Val(dt.Rows[i]["CMISUAMT"].ToString().Trim());//'청구
                    nAmt[3] = VB.Val(dt.Rows[i]["CIPGUMAMT"].ToString().Trim()); //'입금
                    nAmt[6] = VB.Val(dt.Rows[i]["CIPGUMAMT"].ToString().Trim()); //'입금계
                    nTot[6] = nTot[6] + VB.Val(dt.Rows[i]["CIPGUMAMT"].ToString().Trim());

                    strFDate = VB.Left(dt.Rows[i]["YYMM"].ToString().Trim(), 4) + "-" + VB.Mid(dt.Rows[i]["YYMM"].ToString().Trim(), 5, 2) + "-01";
                    strTDate = CF.READ_LASTDAY(clsDB.DbCon, strFDate);

                    // '해당월 검토액 READ

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT SUM(AMT) AMT ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_SLIP";
                    SQL = SQL + ComNum.VBLF + " WHERE BDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND BDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";

                    if (rdojob0.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "    AND Class = '07'                                                       ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "    AND Class = '20'                                                       ";
                    }

                    if (rdoIO1.Checked == true)
                        SQL = SQL + ComNum.VBLF + " AND IPDOPD ='I'   ";

                    if (rdoIO2.Checked == true)
                        SQL = SQL + ComNum.VBLF + " AND IPDOPD ='O'   ";

                    SQL = SQL + ComNum.VBLF + "    AND GUBUN ='01' "; //'1차검토액

                    SqlErr = clsDB.GetDataTable(ref dtFc2, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        btnSearch.Enabled = true;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    nAmt[4] = VB.Val(dtFc2.Rows[0]["AMT"].ToString().Trim());

                    dtFc2.Dispose();
                    dtFc2 = null;

                    nAmt[5] = VB.Val(dt.Rows[i]["cSakAmt"].ToString().Trim());//  '삭감
                    nAmt[8] = VB.Val(dt.Rows[i]["cJanAmt"].ToString().Trim());//  '현미수액
                    nAmt[10] = VB.Val(dt.Rows[i]["cEtcAmt"].ToString().Trim());// '기타입금액
                    nAmt[11] = VB.Val(dt.Rows[i]["cTotMAmt"].ToString().Trim());// '삭감율계산용 총진료비
                    nAmt[12] = VB.Val(dt.Rows[i]["cTotSAmt"].ToString().Trim());// '삭감율계산용 총삭감액

                    if (nAmt[11] != 0 && nAmt[12] != 0)
                    {
                        nAmt[7] = (nAmt[12] / nAmt[11] * 100); //'삭감율
                    }

                    nRow = nRow + 1;

                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    ssView_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["YYMM"].ToString().Trim();

                    for (j = 1; j <= 10; j++)
                    {
                        if (j != 7)
                        {
                            ssView_Sheet1.Cells[nRow - 1, j].Text = nAmt[j].ToString("###,###,###,##0 ");
                        }
                        if (j == 7)
                        {
                            ssView_Sheet1.Cells[nRow - 1, j].Text = nAmt[j].ToString("##0.00") + "%";
                        }
                    }
                }

                nRow = nRow + 1;
                ssView_Sheet1.RowCount = nRow;
                ssView_Sheet1.Cells[nRow - 1, 0].Text = "** 전체합계 **";

                for (j = 1; j <= 10; j++)
                {
                    if (j != 7)
                    {
                        ssView_Sheet1.Cells[nRow - 1, j].Text = nTot[j].ToString("###,###,###,##0 ");
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;

            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }     //권한확인

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            strTitle = "(자동차보험) 청구 및 미수 내역 총괄표";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            if (rdoIO0.Checked == true)
            {
                strHeader += CS.setSpdPrint_String("  (전체)", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }

            if (rdoIO1.Checked == true)
            {
                strHeader += CS.setSpdPrint_String("  (입원)", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }

            if (rdoIO2.Checked == true)
            {
                strHeader += CS.setSpdPrint_String("  (외래)", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }


            if (rdojob0.Checked == true)
            {
                strHeader += CS.setSpdPrint_String("작업기준 : 발생월 기준", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }
            if (rdojob1.Checked == true)
            {
                strHeader += CS.setSpdPrint_String("청구년월 : 진료 연월을 기준", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }
            strHeader += CS.setSpdPrint_String("작업년월 : " + cboFDate.Text + " ~ " + cboTDate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 30, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rdojob_CheckedChanged(object sender, EventArgs e)
        {
            if (rdojob0.Checked == true)
            {
                ssView_Sheet1.ColumnHeader.Cells[0, 2].Text = "당월청구";
            }
            else

            {
                ssView_Sheet1.ColumnHeader.Cells[0, 2].Text = "전월진료분청구";
            }
        }
    }
}
