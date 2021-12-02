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
    /// File Name       : frmPmpaViewHistory.cs
    /// Description     : 개인 미수 입금표
    /// Author          : 박창욱
    /// Create Date     : 2017-10-19
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\MISUP206.FRM(FrmIpgumPrint.frm) >> frmPmpaViewHistory.cs 폼이름 재정의" />	
    public partial class frmPmpaViewPerIpgumPrint : Form
    {
        public frmPmpaViewPerIpgumPrint()
        {
            InitializeComponent();
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

            int i = 0;
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            ssPrint_Sheet1.Cells[1, 0].Text = "작업기간: " + dtpDate1.Value.ToString("yyyy-MM-dd") + "일부터 " + dtpDate2.Value.ToString("yyyy-MM-dd") + "일까지";
            ssPrint_Sheet1.Cells[2, 0].Text = "출력일자:" + VB.Now().ToString();
            ssPrint_Sheet1.Cells[3, 0].Text = "입금확인: 경리과             (인)";

            for (i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                ssPrint_Sheet1.RowCount += 1;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 7].ColumnSpan = 4;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = ssView_Sheet1.Cells[i, 0].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = ssView_Sheet1.Cells[i, 1].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 2].Text = ssView_Sheet1.Cells[i, 2].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 3].Text = ssView_Sheet1.Cells[i, 3].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 4].Text = ssView_Sheet1.Cells[i, 4].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 5].Text = ssView_Sheet1.Cells[i, 5].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 6].Text = ssView_Sheet1.Cells[i, 6].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 7].Text = ssView_Sheet1.Cells[i, 7].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 11].Text = ssView_Sheet1.Cells[i, 8].Text;
            }
            ssPrint_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);


            strTitle = "개 인 미 수 입 금 표";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, false, false, false, false, 0.9f);

            CS.setSpdPrint(ssPrint, PrePrint, setMargin, setOption, strHeader, strFooter);
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
            int nCnt = 0;
            double nTotMirAmt = 0;
            double nTotIpgumAmt = 0;
            double nJanAmt = 0;
            double nMirAmt = 0;
            double nIpgumAmt = 0;
            string strPano = "";
            string strRemark = "";

            clsPmpaFunc cpf = new clsPmpaFunc();

            if (dtpDate2.Value < dtpDate1.Value)
            {
                ComFunc.MsgBox("종료일자가 시작일자보다 작음");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            btnPrint.Enabled = false;
            btnSearch.Enabled = false;

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 20;

            try
            {
                //PANO    CHAR(8)     병원번호
                //GUBUN1  CHAR(1)     발생,입금구분(1.발생 2.입금 3.반송 4.감액)
                //GUBUN2  CHAR(1)     부서(1.외래 2.응급실 3.입원 4.심사계 5.원무과)
                //BDATE   DATE    발생,입금일자
                //AMT     NUMBER(9)   미수액,입금액
                //REMARK  VARCHAR2(1000)  적요
                //MISUdtL     VARCHAR2(30)    미수상세내역(아래참조)
                //IDNO    NUMBER(6)   입력자 사번
                //FLAG    CHAR(1)     입금완료(' * '=완료,' '=미완료)
                //PART    CHAR(1)     작업조
                //ENTTIME     DATE    등록시각(YY-MM-DD HH24:MI)
                // 입원외래(1)+진료과(2)+미수구분(2)+총진료비(9)+
                // 입원일자 (8) + 퇴원일자(8)
                // 01.가퇴원미수  02.업무착오미수  03.탈원미수
                // 04.지불각서    05.응급미수      06.외래미수
                // 07.심사청구미수 10.기타


                // 해당기간의 입금내역을 Read
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT PANO, TO_CHAR(BDate,'YYYY-MM-DD') vBDate, GUBUN2,";
                SQL = SQL + ComNum.VBLF + "        SUM(Amt) vIAmt, REMARK,  MISUdtL";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND Bdate >= TO_DATE('" + dtpDate1.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + dtpDate2.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND GUBUN1 IN ('2' ,'4','3')";
                //2015-07-13
                if (VB.Left(cboGbn.Text, 2) != "**")
                {
                    SQL = SQL + ComNum.VBLF + "    AND SUBSTR(MISUdtL,4,2)='" + VB.Left(cboGbn.Text, 2) + "'";
                }
                if (chkEnd.Checked == false)
                {
                    SQL = SQL + ComNum.VBLF + "    AND FLAG NOT IN '*'";
                }
                if (txtPano.Text != "")
                {
                    SQL = SQL + ComNum.VBLF + "    AND    PANO ='" + txtPano.Text + "'";
                }
                SQL = SQL + ComNum.VBLF + "  GROUP BY  PANO, BDate, GUBUN2, REMARK, MISUdtL";

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
                    ComFunc.MsgBox("입금된 내역이 없습니다.");
                    return;
                }

                nREAD = dt.Rows.Count;

                nTotMirAmt = 0;
                nTotIpgumAmt = 0;

                if (nREAD > 19)
                {
                    ssView_Sheet1.RowCount = nREAD + 1;
                }

                for (i = 0; i < nREAD; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = (i + 1).ToString();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["vBdate"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = cpf.Get_BasPatient(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim()).Rows[0]["SNAME"].ToString().Trim();

                    if (strPano != dt.Rows[i]["PANO"].ToString().Trim())
                    {
                        nJanAmt = 0;
                        nMirAmt = 0;
                        strPano = dt.Rows[i]["PANO"].ToString().Trim();

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT SUM(AMT) TAmt";
                        SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP";
                        SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "    AND PANO ='" + dt.Rows[i]["PANO"].ToString().Trim() + "'";
                        SQL = SQL + ComNum.VBLF + "    AND GUBUN1 = '1'";
                        SQL = SQL + ComNum.VBLF + "    AND FLAG NOT IN '*'";
                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        nMirAmt = Convert.ToDouble(VB.Val(dt1.Rows[0]["TAmt"].ToString().Trim()).ToString("###,###,###,##0"));
                        ssView_Sheet1.Cells[i, 4].Text = nMirAmt.ToString("###,###,###,##0");
                        nTotMirAmt += nMirAmt;

                        dt1.Dispose();
                        dt1 = null;
                    }
                    else
                    {
                        nMirAmt = nJanAmt;
                    }

                    nIpgumAmt = VB.Val(dt.Rows[i]["vIAmt"].ToString().Trim());
                    nJanAmt = nMirAmt - nIpgumAmt;

                    ssView_Sheet1.Cells[i, 5].Text = nIpgumAmt.ToString("###,###,###,##0");
                    nTotIpgumAmt += nIpgumAmt;

                    ssView_Sheet1.Cells[i, 6].Text = nJanAmt.ToString("###,###,###,##0");

                    ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["Remark"].ToString().Trim();
                    strRemark = dt.Rows[i]["Remark"].ToString().Trim();

                    for (k = 1; k <= strRemark.Length; k++)
                    {
                        switch (VB.Mid(strRemark, k, 1))
                        {
                            case "/n":
                                nCnt += 1;
                                break;
                        }
                    }

                    if (nCnt == 0)
                    {
                        nCnt = 1;
                    }
                }
                dt.Dispose();
                dt = null;

                if (nREAD < 18)
                {
                    ssView_Sheet1.Cells[nREAD, 4].Text = "< 이   하";
                    ssView_Sheet1.Cells[nREAD, 5].Text = "공";
                    ssView_Sheet1.Cells[nREAD, 6].Text = "란 >  ";
                }

                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = "** 전체";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = " 합계 **";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = nTotMirAmt.ToString("###,###,###,##0");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = nTotIpgumAmt.ToString("###,###,###,##0");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = (nTotMirAmt - nTotIpgumAmt).ToString("###,###,###,##0");

                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmPmpaViewPerIpgumPrint_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            string strSysDate = "";

            clsPmpaMisu cpm = new clsPmpaMisu();

            ssView.Dock = DockStyle.Fill;

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            dtpDate1.Value = Convert.ToDateTime(VB.Left(strSysDate, 8) + "01");
            dtpDate2.Value = Convert.ToDateTime(strSysDate);

            txtPano.Text = "";

            cboGbn.Items.Clear();
            cboGbn.Items.Add("**.전체");
            for (i = 1; i < 16; i++)
            {
                cboGbn.Items.Add(i.ToString("00") + "." + cpm.READ_PerMisuGye(i.ToString("00")));
            }
            cboGbn.SelectedIndex = 0;

            btnPrint.Enabled = false;

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 20;
        }

        private void txtPano_Leave(object sender, EventArgs e)
        {
            txtPano.Text = VB.Val(txtPano.Text).ToString("00000000");
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.Focus();
            }
        }
    }
}
