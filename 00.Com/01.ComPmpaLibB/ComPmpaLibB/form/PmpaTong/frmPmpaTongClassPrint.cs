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
    /// File Name       : frmPmpaTongClassPrint.cs
    /// Description     : 미수종류별 청구 및 입금통계
    /// Author          : 박창욱
    /// Create Date     : 2017-09-20
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\MISUG304.FRM(FrmClassTongPrint.frm) >> frmPmpaTongClassPrint.cs 폼이름 재정의" />	
    public partial class frmPmpaTongClassPrint : Form
    {
        clsSpread CS = new clsSpread();
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsPmpaMisu cpm = new clsPmpaMisu();
        double[] nAmt = new double[15];
        double[] nTot = new double[15];

        public frmPmpaTongClassPrint()
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
            int nRead = 0;
            int nRow = 0;
            string strFYYMM = "";
            string strTYYMM = "";

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;

            for (i = 0; i < 15; i++)
            {
                nTot[i] = 0;
                nAmt[i] = 0;
            }

            strFYYMM = VB.Left(cboFYYMM.Text, 4) + VB.Mid(cboFYYMM.Text, 7, 2);
            strTYYMM = VB.Left(cboTYYMM.Text, 4) + VB.Mid(cboTYYMM.Text, 7, 2);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Class,";
                SQL = SQL + ComNum.VBLF + "   CASE WHEN CLASS = '11' THEN '01' ";          //보훈청
                SQL = SQL + ComNum.VBLF + "        WHEN CLASS = '08' THEN '02' ";          //계약처
                SQL = SQL + ComNum.VBLF + "        WHEN CLASS IN('13','12') THEN '03' ";   //심신장애, 시각장애
                SQL = SQL + ComNum.VBLF + "        WHEN CLASS  = '09' THEN '04' ";         //혈액
                SQL = SQL + ComNum.VBLF + "        WHEN CLASS  = '16' THEN '05' ";         //혈액
                SQL = SQL + ComNum.VBLF + "        Else '99' END SORT, ";                  //나머지 항목들
                SQL = SQL + ComNum.VBLF + "        SUM(IwolAmt) cIwolAmt,";
                SQL = SQL + ComNum.VBLF + "        SUM(DECODE(IpdOpd,'I',MisuAmt,0)) cIMAmt,";
                SQL = SQL + ComNum.VBLF + "        SUM(DECODE(IpdOpd,'O',MisuAmt,0)) cOMAmt,";
                SQL = SQL + ComNum.VBLF + "        SUM(IpgumAmt+EtcAmt) cIpgumAmt,";
                SQL = SQL + ComNum.VBLF + "        SUM(SakAmt) cSakAmt,";
                SQL = SQL + ComNum.VBLF + "        SUM(BanAmt) cBanAmt,";
                SQL = SQL + ComNum.VBLF + "        SUM(JanAmt) cJanAmt,";
                SQL = SQL + ComNum.VBLF + "        SUM(TotMAmt) cTotMAmt,SUM(TotSAmt) cTotSAmt";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GELTOT";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND YYMM >= '" + strFYYMM + "'";
                SQL = SQL + ComNum.VBLF + "    AND YYMM <= '" + strTYYMM + "'";
                SQL = SQL + ComNum.VBLF + "    AND Class >= '08'";
                SQL = SQL + ComNum.VBLF + "    AND Class <  '20'";
                SQL = SQL + ComNum.VBLF + "    AND Class <> '10'"; //계약처제외
                SQL = SQL + ComNum.VBLF + "  GROUP BY Class";
                SQL = SQL + ComNum.VBLF + "  ORDER BY SORT, Class";

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
                    ComFunc.MsgBox("해당월의 DATA가 없습니다.");
                    return;
                }

                nRow = 0;
                nRead = dt.Rows.Count;

                for (i = 0; i < nRead; i++)
                {
                    nAmt[1] = VB.Val(dt.Rows[i]["cIwolAmt"].ToString().Trim());     //이월
                    nAmt[2] = VB.Val(dt.Rows[i]["cOMAmt"].ToString().Trim());       //외래청구
                    nAmt[3] = VB.Val(dt.Rows[i]["cIMAmt"].ToString().Trim());       //입원청구
                    nAmt[4] = VB.Val(dt.Rows[i]["cOMAmt"].ToString().Trim()) + VB.Val(dt.Rows[i]["cIMAmt"].ToString().Trim());
                    nAmt[5] = VB.Val(dt.Rows[i]["cBanAmt"].ToString().Trim());      //반송
                    nAmt[6] = VB.Val(dt.Rows[i]["cIpgumAmt"].ToString().Trim());    //입금
                    nAmt[7] = VB.Val(dt.Rows[i]["cSakAmt"].ToString().Trim());      //삭감
                    nAmt[8] = 0;                                                    //삭감율
                    nAmt[9] = VB.Val(dt.Rows[i]["cIpgumAmt"].ToString().Trim()) + VB.Val(dt.Rows[i]["cSakAmt"].ToString().Trim());
                    nAmt[10] = VB.Val(dt.Rows[i]["cJanAmt"].ToString().Trim());     //현미수액
                    nAmt[11] = VB.Val(dt.Rows[i]["cTotMAmt"].ToString().Trim());    //삭감율계산용 총진료비
                    nAmt[12] = VB.Val(dt.Rows[i]["cTotSAmt"].ToString().Trim());    //삭감율계산용 총삭감액
                    if (nAmt[12] != 0 && nAmt[11] != 0)
                    {
                        nAmt[8] = nAmt[12] / nAmt[11] * 100;    //삭감율
                    }

                    nRow += 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    //TODO : clsPmpaPb.GstrMisuClass[i]에 값이 없으므로 실상황 테스트 필요
                    ssView_Sheet1.Cells[nRow - 1, 0].Text = cpm.READ_MisuClass(dt.Rows[i]["Class"].ToString().Trim());
                    for (k = 1; k < 11; k++)
                    {
                        if (k != 8)
                        {
                            ssView_Sheet1.Cells[nRow - 1, k].Text = nAmt[k].ToString("###,###,###,##0 ");
                        }
                        if (k == 8)
                        {
                            ssView_Sheet1.Cells[nRow - 1, k].Text = nAmt[k].ToString("##0.00") + "%";
                        }
                        nTot[k] += nAmt[k];
                    }
                    nTot[11] += nAmt[11];
                    nTot[12] += nAmt[12];
                }
                dt.Dispose();
                dt = null;

                nRow += 1;
                ssView_Sheet1.RowCount = nRow;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                nTot[8] = 0;
                if (nTot[11] != 0 && nTot[12] != 0)
                {
                    nTot[8] = nTot[12] / nTot[11] * 100;    //삭감율
                }
                ssView_Sheet1.Cells[nRow - 1, 0].Text = "** 전체합계 **";

                for (k = 1; k < 11; k++)
                {
                    if (k != 8)
                    {
                        ssView_Sheet1.Cells[nRow - 1, k].Text = nTot[k].ToString("###,###,###,##0 ");
                    }
                    if (k == 8)
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strPrintName = clsPrint.gGetDefaultPrinter();//기본프린터 이름을 가져온다

            if (strPrintName != "")
            {
                strTitle = cboFYYMM.Text + "~" + cboTYYMM.Text + "청구 및 미수 내역 총괄표";

                strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += CS.setSpdPrint_String("미수종류 : 미수종류별", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += CS.setSpdPrint_String("출력시간 : " + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter, strPrintName);
            }
        }

        private void frmPmpaTongClassPrint_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboFYYMM, 20, "", "1");
            clsVbfunc.SetCboDate(clsDB.DbCon, cboTYYMM, 20, "", "1");

            cboFYYMM.SelectedIndex = 0;
            cboTYYMM.SelectedIndex = 0;
        }
    }
}
