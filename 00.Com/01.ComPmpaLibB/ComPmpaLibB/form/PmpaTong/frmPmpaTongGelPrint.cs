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
    /// File Name       : frmPmpaTongGelPrint.cs
    /// Description     : 보험회사별 청구 및 입금통계
    /// Author          : 박창욱
    /// Create Date     : 2017-09-18
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\MISUT203.FRM(FrmGelTongPrint.frm) >> frmPmpaTongGelPrint.cs 폼이름 재정의" />	
    public partial class frmPmpaTongGelPrint : Form
    {
        clsPmpaFunc cpf = new clsPmpaFunc();
        clsSpread CS = new clsSpread();
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        ComFunc cf = new ComFunc();

        double[] nAmt = new double[12];
        double[] nTot = new double[12];

        public frmPmpaTongGelPrint()
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
            int nRead = 0;
            int nRow = 0;
            string strFYYMM = "";
            string strTYYMM = "";
            string strMiaName = "";

            string FDATE = "";
            string TDATE = "";

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;

            for (i = 0; i < 12; i++)
            {
                nTot[i] = 0;
                nAmt[i] = 0;
            }

            strFYYMM = VB.Left(cboFYYMM.Text, 4) + VB.Mid(cboFYYMM.Text, 7, 2);
            strTYYMM = VB.Left(cboTYYMM.Text, 4) + VB.Mid(cboTYYMM.Text, 7, 2);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT GelCode, SUM(IwolAmt) cIwolAmt, SUM(MisuAmt) cMisuAmt,";
                SQL = SQL + ComNum.VBLF + "        SUM(IpgumAmt) cIpgumAmt, SUM(SakAmt) cSakAmt, SUM(BanAmt+EtcAmt) cEtcAmt,";
                SQL = SQL + ComNum.VBLF + "        SUM(JanAmt) cJanAmt, SUM(TotMAmt) cTotMAmt, SUM(TotSAmt) cTotSAmt";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GELTOT";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND YYMM >= '" + strFYYMM + "'";
                SQL = SQL + ComNum.VBLF + "    AND YYMM <= '" + strTYYMM + "'";
                if (rdoJob0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND Class = '07'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND Class = '20'";
                }
                if (rdoIO1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND IPDOPD ='I'";
                }
                if (rdoIO2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND IPDOPD ='O'";
                }
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
                nRead = dt.Rows.Count;

                for (i = 0; i < nRead; i++)
                {

                    FDATE = strFYYMM + "01";
                    FDATE = VB.Left(strFYYMM, 4) + "-" + VB.Right(strFYYMM, 2) +"-01";
                    TDATE = cf.READ_LASTDAY(clsDB.DbCon, strTYYMM + "01");

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT B.IpdOpd, B.Class, B.TongGbn, Sum(A.Amt) Amt , B.GelCode";
                    SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.MISU_SLIP A, KOSMOS_PMPA.MISU_IDMST B";
                    SQL = SQL + ComNum.VBLF + "WHERE A.BDATE >= TO_DATE('" + FDATE + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "AND A.BDATE <= TO_DATE('" + TDATE + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "AND A.GUBUN >='11' AND A.GUBUN <='20'";
                    SQL = SQL + ComNum.VBLF + "AND A.AMT <> 0";
                    SQL = SQL + ComNum.VBLF + "AND A.WRTNO = B.WRTNO(+)";
                    SQL = SQL + ComNum.VBLF + "AND B.CLASS = '07'";
                    SQL = SQL + ComNum.VBLF + "AND B.TONGGBN = '5'";
                    SQL = SQL + ComNum.VBLF + "AND B.GELCODE = '" + dt.Rows[i]["GelCode"].ToString().Trim() + "'";
                    SQL = SQL + ComNum.VBLF + "GROUP BY B.IPDOPD,B.Class,B.TongGbn,B.GELCODE";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    nAmt[1] = VB.Val(dt.Rows[i]["cIwolAmt"].ToString().Trim());     //이월
                    nAmt[2] = VB.Val(dt.Rows[i]["cMisuAmt"].ToString().Trim());     //청구
                    nAmt[3] = VB.Val(dt.Rows[i]["cIpgumAmt"].ToString().Trim());    //입금
                    nAmt[4] = VB.Val(dt.Rows[i]["cSakAmt"].ToString().Trim());      //삭감
                    nAmt[5] = nAmt[3] + nAmt[4];                                    //입금계
                    nAmt[6] = 0;                                                    //삭감율
                    nAmt[7] = VB.Val(dt.Rows[i]["cJanAmt"].ToString().Trim());      //현미수액
                    if (dt1.Rows.Count > 0)
                    {
                        nAmt[8] = VB.Val(dt1.Rows[0]["Amt"].ToString().Trim());            //이의신청액
                    }
                    else
                    {
                        nAmt[8] = 0;                                                //이의신청액
                    }
                    nAmt[9] = VB.Val(dt.Rows[i]["cEtcAmt"].ToString().Trim());      //기타입금액
                    nAmt[10] = VB.Val(dt.Rows[i]["cTotMAmt"].ToString().Trim());    //삭감율계산용 총진료비
                    nAmt[11] = VB.Val(dt.Rows[i]["cTotSAmt"].ToString().Trim());    //삭감율계산용 총삭감액
                    if (nAmt[10] != 0 && nAmt[11] != 0)
                    {
                        nAmt[6] = nAmt[11] / nAmt[10] * 100;                        //삭감율
                    }

                    dt1.Dispose();
                    dt1 = null;

                    nRow += 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    if (cpf.GET_BAS_MIA(clsDB.DbCon, dt.Rows[i]["GelCode"].ToString().Trim()) == "")
                    {
                        strMiaName = "-< " + dt.Rows[i]["GelCode"].ToString().Trim() + " ->";
                    }
                    else
                    {
                        strMiaName = cpf.GET_BAS_MIA(clsDB.DbCon, dt.Rows[i]["GelCode"].ToString().Trim());
                    }

                    ssView_Sheet1.Cells[nRow - 1, 0].Text = strMiaName.Trim();
                    for (k = 1; k < 10; k++)
                    {
                        if (k != 6)
                        {
                            ssView_Sheet1.Cells[nRow - 1, k].Text = nAmt[k].ToString("###,###,###,##0 ");
                        }
                        if (k == 6)
                        {
                            ssView_Sheet1.Cells[nRow - 1, k].Text = nAmt[k].ToString("##0.00 ") + "%";
                        }
                        nTot[k] += nAmt[k];
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            string strIO = "";
            bool PrePrint = true;



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
                strIO = " (외래)";
            }
            strTitle = strIO + "청구 및 미수 내역 총괄표";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            

            if (rdoJob0.Checked == true)
            {   
                strHeader += CS.setSpdPrint_String("작업기준 : 발생월 기준", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }
            else if (rdoJob1.Checked == true)
            {
                strHeader += CS.setSpdPrint_String("청구년월 : 진료 연월을 기준", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }
            strHeader += CS.setSpdPrint_String("출력시간 : " + VB.Now().ToString() + "\n담당자:" + clsType.User.JobName + "( 인 )",
                         new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strHeader += "\n\n\n\n";
            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 100, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);  

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void frmPmpaTongGelPrint_Load(object sender, EventArgs e)
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

        private void rdoJob_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoJob0.Checked == true)
            {
                ssView_Sheet1.ColumnHeader.Cells[0, 2].Text = "당월청구";
            }
            else if (rdoJob1.Checked == true)
            {
                ssView_Sheet1.ColumnHeader.Cells[0, 2].Text = "전월진료분청구";
            }
        }

        private void ssView_PrintHeaderFooterArea(object sender, PrintHeaderFooterAreaEventArgs e)
        {
            if (Chk1.Checked == true)
            {
                Pen cPen = new Pen(Color.Black, 1);
                cPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                cPen.Width = 1;
                StringFormat drawFormat = new StringFormat();
                drawFormat.Alignment = StringAlignment.Far;
                Font font = new Font("맑은 고딕", 10, FontStyle.Regular);

                int strX = 722;
                int strY = 94;

                if (e.IsHeader == true)
                {
                    #region 헤더
                    //e.Graphics.DrawString(" 테스트 ", new Font("맑은 고딕", 15, FontStyle.Bold), Brushes.Black, 575, 20, drawFormat);
                    #endregion

                    #region 칸 그리기
                    e.Graphics.DrawRectangle(cPen, 700, 90, 380, 90);  

                    e.Graphics.DrawLine(cPen, 725, 115, 1080, 115);
                    //결재
                    e.Graphics.DrawLine(cPen, 725, 90, 725, 180);
                    //담당
                    e.Graphics.DrawLine(cPen, 795, 90, 795, 180);
                    //계장
                    e.Graphics.DrawLine(cPen, 865, 90, 865, 180);
                    //과장
                    e.Graphics.DrawLine(cPen, 935, 90, 935, 180);
                    //행정처장
                    e.Graphics.DrawLine(cPen, 1005, 90, 1005, 180);
                    //전결처리
                    e.Graphics.DrawLine(cPen, 1005, 115, 935, 180);
                    e.Graphics.DrawLine(cPen, 1080, 115, 1005, 180);

                    #endregion


                    #region 칸안에 글
                    e.Graphics.DrawString("결", font, Brushes.Black, strX, 102, drawFormat);
                    e.Graphics.DrawString("재", font, Brushes.Black, strX, 102 + 47, drawFormat);
                    e.Graphics.DrawString("담    당", font, Brushes.Black, strX + 64, strY, drawFormat);
                    e.Graphics.DrawString("원무행정", font, Brushes.Black, strX + 137, strY, drawFormat);
                    e.Graphics.DrawString("팀    장", font, Brushes.Black, strX + 204, strY, drawFormat);
                    e.Graphics.DrawString("행정처장", font, Brushes.Black, strX + 278, strY, drawFormat);
                    e.Graphics.DrawString("병 원 장", font, Brushes.Black, strX + 348, strY, drawFormat);

                    //e.Graphics.DrawString(" 전 결 ", font, Brushes.Black, strX + 198, strY + 42, drawFormat);
                    #endregion

                    #region 업무일지 / 작업일
                    drawFormat.Alignment = StringAlignment.Near;
                    //e.Graphics.DrawString("작성자   : " + clsType.User.UserName, font, Brushes.Black, 30, 85, drawFormat);
                    //e.Graphics.DrawString("작업일자 : ", font, Brushes.Black, 30, 105, drawFormat);
                    //e.Graphics.DrawString("출력시간 : " + Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).ToString("yyyy-MM-dd HH:mm"), font, Brushes.Black, 30, 125, drawFormat);
                    #endregion
                }
            }
        }
    }
}
