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
    /// File Name       : frmPmpaTongGyeTongPrint2.cs
    /// Description     : 계약처별 청구 및 입금통계
    /// Author          : 박창욱
    /// Create Date     : 2017-10-18
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\MISUR203.FRM(FrmGelTongPrint.frm) >> frmPmpaTongGyeTongPrint2.cs 폼이름 재정의" />	
    public partial class frmPmpaTongGyeTongPrint2 : Form
    {
        double[] nAmt = new double[12];
        double[] nTot = new double[12];

        public frmPmpaTongGyeTongPrint2()
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
            int nRow = 0;
            int nREAD = 0;
            string strYYMM = "";
            string strMiaName = "";

            clsPmpaFunc cpf = new clsPmpaFunc();

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;

            for (i = 0; i < 12; i++)
            {
                nAmt[i] = 0;
                nTot[i] = 0;
            }

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT GelCode, SUM(IwolAmt) cIwolAmt, SUM(MisuAmt) cMisuAmt,";
                SQL = SQL + ComNum.VBLF + "        SUM(IpgumAmt+EtcAmt) cIpgumAmt, SUM(SakAmt) cSakAmt, SUM(BanAmt+EtcAmt) cEtcAmt,";
                SQL = SQL + ComNum.VBLF + "        SUM(JanAmt) cJanAmt, SUM(TotMAmt) cTotMAmt, SUM(TotSAmt) cTotSAmt";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GELTOT";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND YYMM = '" + strYYMM + "'";
                SQL = SQL + ComNum.VBLF + "    AND Class = '08'"; //계약처
                SQL = SQL + ComNum.VBLF + "  GROUP BY GelCode";
                SQL = SQL + ComNum.VBLF + "  ORDER BY GelCode";

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
                nREAD = dt.Rows.Count;

                for (i = 0; i < nREAD; i++)
                {
                    nAmt[1] = VB.Val(dt.Rows[i]["cIwolAmt"].ToString().Trim());     //이월
                    nAmt[2] = VB.Val(dt.Rows[i]["cMisuAmt"].ToString().Trim());     //청구
                    nAmt[3] = VB.Val(dt.Rows[i]["cIpgumAmt"].ToString().Trim());    //입금
                    nAmt[4] = VB.Val(dt.Rows[i]["cSakAmt"].ToString().Trim());      //삭감
                    nAmt[5] = nAmt[3] + nAmt[4];                                    //입금계
                    nAmt[6] = 0;                                                    //삭감율
                    nAmt[7] = VB.Val(dt.Rows[i]["cJanAmt"].ToString().Trim());      //현미수액
                    nAmt[8] = 0;                                                    //이의신청액
                    nAmt[9] = VB.Val(dt.Rows[i]["cEtcAmt"].ToString().Trim());      //기타입금액
                    nAmt[10] = VB.Val(dt.Rows[i]["cTotMAmt"].ToString().Trim());     //삭감율계산용 총진료비
                    nAmt[11] = VB.Val(dt.Rows[i]["cTotSAmt"].ToString().Trim());     //삭감율계산용 총삭감액
                    if (nAmt[10] != 0 && nAmt[11] != 0)
                    {
                        nAmt[6] = nAmt[11] / nAmt[10] * 100;      //삭감율
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

                    ssView_Sheet1.Cells[nRow - 1, 0].Text = strMiaName;

                    for (k = 1; k < 10; k++)
                    {
                        if (k != 6)
                        {
                            ssView_Sheet1.Cells[nRow - 1, k].Text = nAmt[k].ToString("###,###,###,##0 ");
                        }
                        if (k == 6)
                        {
                            ssView_Sheet1.Cells[nRow - 1, k].Text = nAmt[k].ToString("##0.00") + "%";
                        }
                        nTot[k] += nAmt[k];
                    }
                    nTot[10] += nAmt[10];
                    nTot[11] += nTot[11];
                }
                dt.Dispose();
                dt = null;

                nRow += 1;
                ssView_Sheet1.RowCount = nRow;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                nTot[6] = 0;
                if (nTot[10] != 0 && nTot[11] != 0)
                {
                    nTot[6] = nTot[11] / nTot[10] * 100;  //삭감율
                }
                ssView_Sheet1.Cells[nRow - 1, 0].Text = "** 전체합계 **";
                for (k = 1; k < 10; k++)
                {
                    if (k != 6)
                    {
                        ssView_Sheet1.Cells[nRow - 1, k].Text = nTot[k].ToString("###,###,###,##0 ");
                    }
                    if (k == 6)
                    {
                        ssView_Sheet1.Cells[nRow - 1, k].Text = nTot[k].ToString("##0.00") + "%";
                    }
                }
            }
            catch (Exception ex)
            {
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
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = cboYYMM.Text + " 청구 및 미수 내역 총괄표";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("작업기준 : 계약처 제출기준", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력시간 : " + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력자 : " + clsType.User.JobName, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 100, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false, 0.9f);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void frmPmpaTongGyeTongPrint2_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM, 12, "", "1");
            cboYYMM.SelectedIndex = 1;
        }
    }
}
