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
    /// File Name       : frmPmpaTongEtcMisu.cs
    /// Description     : 기타미수 월별청구 및 입금통계
    /// Author          : 박창욱
    /// Create Date     : 2017-10-10
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\Frm기타미수통계표1.FRM(Frm기타미수통계표1.frm) >> frmPmpaTongEtcMisu.cs 폼이름 재정의" />	
    public partial class frmPmpaTongEtcMisu : Form
    {
        clsSpread CS = new clsSpread();
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;

        double[] nAmt = new double[15];
        double[] nTot = new double[15];

        public frmPmpaTongEtcMisu()
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
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int k = 0;
            int nRow = 0;
            int nRead = 0;
            string strFYYMM = "";
            string strTYYMM = "";

            strFYYMM = VB.Left(cboFYYMM.Text, 4) + VB.Mid(cboFYYMM.Text, 7, 2);
            strTYYMM = VB.Left(cboTYYMM.Text, 4) + VB.Mid(cboTYYMM.Text, 7, 2);

            ssView_Sheet1.RowCount = 0;
            for (i = 0; i < 15; i++)
            {
                nTot[i] = 0;
                nAmt[i] = 0;
            }

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT YYMM, SUM(IwolAmt) cIwolAmt, SUM(DECODE(IpdOpd,'I',MisuAmt,0)) cIMAmt,";
                SQL = SQL + ComNum.VBLF + "        SUM(DECODE(IpdOpd,'O',MisuAmt,0)) cOMAmt, SUM(IpgumAmt+EtcAmt) cIpgumAmt, SUM(SakAmt) cSakAmt,";
                SQL = SQL + ComNum.VBLF + "        SUM(BanAmt) cBanAmt, SUM(JanAmt) cJanAmt, SUM(TotMAmt) cTotMAmt,";
                SQL = SQL + ComNum.VBLF + "        SUM(TotSAmt) cTotSAmt";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GELTOT";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND YYMM >= '" + strFYYMM + "'";
                SQL = SQL + ComNum.VBLF + "    AND YYMM <= '" + strTYYMM + "'";
                SQL = SQL + ComNum.VBLF + "    AND Class >= '08'";
                SQL = SQL + ComNum.VBLF + "    AND Class <  '20'";
                SQL = SQL + ComNum.VBLF + "    AND Class <> '10'"; //계약처제외
                SQL = SQL + ComNum.VBLF + "  GROUP BY YYMM";
                SQL = SQL + ComNum.VBLF + "  ORDER BY YYMM";

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRow = 0;
                nRead = dt.Rows.Count;

                for (i = 0; i < nRead; i++)
                {
                    nAmt[1] = VB.Val(dt.Rows[i]["cIwolAmt"].ToString().Trim());     //이월
                    nAmt[2] = VB.Val(dt.Rows[i]["cOMAmt"].ToString().Trim());     //외래청구
                    nTot[2] += nAmt[2];
                    nAmt[3] = VB.Val(dt.Rows[i]["cIMAmt"].ToString().Trim());     //입원청구
                    nTot[3] += nAmt[3];
                    nAmt[4] = VB.Val(dt.Rows[i]["cOMAmt"].ToString().Trim()) + VB.Val(dt.Rows[i]["cIMAmt"].ToString().Trim());
                    nTot[4] += nAmt[4];
                    nAmt[6] = VB.Val(dt.Rows[i]["cIpgumAmt"].ToString().Trim());  //입금
                    nTot[6] += nAmt[6];
                    nAmt[7] = VB.Val(dt.Rows[i]["cSakAmt"].ToString().Trim());   //삭감
                    nAmt[8] = 0;                               //삭감율
                    nAmt[9] = VB.Val(dt.Rows[i]["cBanAmt"].ToString().Trim());    //반송
                    nAmt[10] = VB.Val(dt.Rows[i]["cIpgumAmt"].ToString().Trim()) + VB.Val(dt.Rows[i]["cSakAmt"].ToString().Trim());
                    nTot[10] += nAmt[10];
                    nAmt[11] = VB.Val(dt.Rows[i]["cJanAmt"].ToString().Trim());   //현미수액
                    nAmt[12] = VB.Val(dt.Rows[i]["cTotMAmt"].ToString().Trim());  //삭감율계산용 총진료비
                    nAmt[13] = VB.Val(dt.Rows[i]["cTotSAmt"].ToString().Trim());  //삭감율계산용 총삭감액
                    if (nAmt[13] != 0 && nAmt[12] != 0)
                    {
                        nAmt[8] = nAmt[13] / nAmt[12] * 100;    //삭감율
                    }

                    nRow += 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    ssView_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["YYMM"].ToString().Trim();

                    for (k = 1; k < 12; k++)
                    {
                        if (k != 8)
                        {
                            ssView_Sheet1.Cells[nRow - 1, k].Text = nAmt[k].ToString("###,###,###,##0 ");
                        }
                        if (k == 8)
                        {
                            ssView_Sheet1.Cells[nRow - 1, k].Text = nAmt[k].ToString("##0.00") + "%";
                        }
                        if (k == 5)
                        {
                            ssView_Sheet1.Cells[nRow - 1, k].Text = "";
                        }
                    }

                    nTot[12] += nAmt[12];
                    nTot[13] += nAmt[13];
                }
                dt.Dispose();
                dt = null;

                nRow += 1;
                ssView_Sheet1.RowCount = nRow;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                nTot[8] = 0;
                if (nTot[12] != 0 && nTot[13] != 0)
                {
                    nTot[8] = nTot[13] / nTot[12] * 100;    //삭감율
                }

                ssView_Sheet1.Cells[nRow - 1, 0].Text = "** 전체합계 **";
                for (k = 1; k < 12; k++)
                {
                    if (k != 8)
                    {
                        ssView_Sheet1.Cells[nRow - 1, k].Text = nTot[k].ToString("###,###,###,##0 ");
                    }
                    if (k == 5)
                    {
                        ssView_Sheet1.Cells[nRow - 1, k].Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
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

            strTitle = "(기타 미수) 청구 및 미수 내역 총괄표";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("미수종류 : 계약처, 혈액원, 보훈청, 심신장애, 노인장기요양", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("작업년월 : " + cboFYYMM.Text + "~" + cboTYYMM.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 30, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void frmPmpaTongEtcMisu_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboFYYMM, 20, "", "1");
            clsVbfunc.SetCboDate(clsDB.DbCon, cboTYYMM, 20, "", "1");
            cboFYYMM.SelectedIndex = 1;
            cboTYYMM.SelectedIndex = 1;
        }
    }
}
