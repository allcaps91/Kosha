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
    /// File Name       : frmPmpaTongGelPrint2.cs
    /// Description     : 보험회사별 청구 및 입금 통계
    /// Author          : 박창욱
    /// Create Date     : 2017-10-10
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\MISUS203.FRM(FrmGelTongPrint.frm) >> frmPmpaTongGelPrint2.cs 폼이름 재정의" />
    public partial class frmPmpaTongGelPrint2 : Form
    {
        clsPmpaFunc cpf = new clsPmpaFunc();
        clsSpread CS = new clsSpread();
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;

        double[] nAmt = new double[12];
        double[] nTot = new double[12];

        public frmPmpaTongGelPrint2()
        {
            InitializeComponent();
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
            int nRead = 0;
            int nRow = 0;
            string strFYYMM = "";
            string strTYYMM = "";
            string strMiaName = "";

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;

            for (i = 0; i < 12; i++)
            {
                nTot[i] = 0;
            }

            strFYYMM = VB.Left(cboFYYMM.Text, 4) + VB.Mid(cboFYYMM.Text, 7, 2);
            strTYYMM = VB.Left(cboTYYMM.Text, 4) + VB.Mid(cboTYYMM.Text, 7, 2);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT YYMM, GelCode, SUM(IwolAmt) cIwolAmt, SUM(SakAmt) cSakAmt,";
                SQL = SQL + ComNum.VBLF + "        SUM(MisuAmt) cMisuAmt, SUM(IpgumAmt+EtcAmt) cIpgumAmt,";
                SQL = SQL + ComNum.VBLF + "        SUM(BanAmt+EtcAmt) cEtcAmt, SUM(JanAmt) cJanAmt, SUM(TotMAmt) cTotMAmt,";
                SQL = SQL + ComNum.VBLF + "        SUM(TotSAmt) cTotSAmt";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GELTOT";
                SQL = SQL + ComNum.VBLF + "  WHERE YYMM >= '" + strFYYMM + "'";
                SQL = SQL + ComNum.VBLF + "    AND YYMM <= '" + strTYYMM + "'";
                if (rdoIO1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND IPDOPD ='I'";
                }
                if (rdoIO2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND IPDOPD ='O'";
                }
                SQL = SQL + ComNum.VBLF + "    AND Class = '05'";
                SQL = SQL + ComNum.VBLF + "  GROUP BY YYMM, GelCode";
                SQL = SQL + ComNum.VBLF + "  ORDER BY YYMM, GelCode";

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
                    nAmt[1] = VB.Val(dt.Rows[i]["cIwolAmt"].ToString().Trim()); //이월
                    nAmt[2] = VB.Val(dt.Rows[i]["cMisuAmt"].ToString().Trim()); //청구
                    nTot[2] += nAmt[2];
                    nAmt[3] = VB.Val(dt.Rows[i]["cIpgumAmt"].ToString().Trim());    //입금
                    nTot[3] += nAmt[3];
                    nAmt[4] = VB.Val(dt.Rows[i]["cSakAmt"].ToString().Trim());  //삭감
                    nTot[4] += nAmt[4];
                    nAmt[5] = nAmt[3] + nAmt[4];    //입금계
                    nTot[5] += nAmt[5];
                    nAmt[6] = 0;        //삭감율
                    nAmt[7] = VB.Val(dt.Rows[i]["cJanAmt"].ToString().Trim());  //현미수액
                    nAmt[8] = 0;        //이의 신청액
                    nAmt[9] = VB.Val(dt.Rows[i]["cEtcAmt"].ToString().Trim());  //기타입금액
                    nAmt[10] = VB.Val(dt.Rows[i]["cTotMAmt"].ToString().Trim());    //삭감율계산용 총진료비
                    nAmt[11] = VB.Val(dt.Rows[i]["cTotSAmt"].ToString().Trim());    //삭감율계산용 총삭감액
                    if (nAmt[10] != 0 && nAmt[11] != 0)
                    {
                        nAmt[6] = nAmt[11] / nAmt[10] * 100;    //삭감율
                    }

                    nRow += 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    if (cpf.GET_BAS_MIA(clsDB.DbCon, dt.Rows[i]["GelCode"].ToString().Trim()) == "")
                    {
                        strMiaName = "-< " + dt.Rows[i]["GelCode"].ToString().Trim() + " >-";
                    }
                    else
                    {
                        strMiaName = cpf.GET_BAS_MIA(clsDB.DbCon, dt.Rows[i]["GelCode"].ToString().Trim());
                    }

                    ssView_Sheet1.Cells[nRow - 1, 0].Text = VB.Left(dt.Rows[i]["YYMM"].ToString().Trim(), 4) + "/" +
                                                          VB.Mid(dt.Rows[i]["YYMM"].ToString().Trim(), 5, dt.Rows[i]["YYMM"].ToString().Trim().Length);
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = strMiaName.Trim();

                    for (k = 1; k < 10; k++)
                    {
                        if (k != 6)
                        {
                            ssView_Sheet1.Cells[nRow - 1, k + 1].Text = nAmt[k].ToString("###,###,###,##0 ");
                        }
                        if (k == 6)
                        {
                            ssView_Sheet1.Cells[nRow - 1, k + 1].Text = nAmt[k].ToString("##0.00") + "%";
                        }
                    }
                    nTot[10] += nAmt[10];
                    nTot[11] += nAmt[11];
                }
                dt.Dispose();
                dt = null;

                nRow += 1;
                ssView_Sheet1.RowCount = nRow;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                nTot[6] = 0;
                if (nTot[10] != 0 && nTot[11] != 0)
                {
                    nTot[6] = nTot[11] / nTot[10] * 100;    //삭감율
                }

                ssView_Sheet1.Cells[nRow - 1, 1].Text = "** 전체합계 **";
                for (k = 1; k < 10; k++)
                {
                    if (k != 6)
                    {
                        ssView_Sheet1.Cells[nRow - 1, k + 1].Text = nTot[k].ToString("###,###,###,##0 ");
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
            string strIO = "";
            bool PrePrint = false;

            if (rdoIO0.Checked == true)
            {
                strIO = " (전체)";
            }
            if (rdoIO1.Checked == true)
            {
                strIO = " (입원)";
            }
            if (rdoIO2.Checked == true)
            {
                strIO = " (통원)";
            }

            strTitle = "(산업재해보험) 청구 및 미수 내역 총괄표  " + strIO;

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("작업년월 : " + cboFYYMM.Text + " ~ " + cboTYYMM.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void frmPmpaTongGelPrint2_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboFYYMM, 36, "", "1");
            clsVbfunc.SetCboDate(clsDB.DbCon, cboTYYMM, 36, "", "1");
            cboFYYMM.SelectedIndex = 0;
            cboTYYMM.SelectedIndex = 0;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
