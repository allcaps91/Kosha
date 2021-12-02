using ComLibB;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaTongClassYear.cs
    /// Description     : 미수종류별 연간통계 조회
    /// Author          : 박창욱
    /// Create Date     : 2017-09-20
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\MISUM304.FRM(FrmClassYear.frm) >> frmPmpaTongClassYear.cs 폼이름 재정의" />	
    public partial class frmPmpaTongClassYear : Form
    {
        clsSpread CS = new clsSpread();
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;

        double[,] nTotAmt = new double[14, 7];

        public frmPmpaTongClassYear()
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
            string strYear = "";

            strYear = VB.Left(cboYear.Text, 4);

            Cursor.Current = Cursors.WaitCursor;

            for (i = 0; i < 14; i++)
            {
                for (k = 0; k < 7; k++)
                {
                    nTotAmt[i, k] = 0;
                }
            }

            btnSearch.Enabled = false;
            btnPrint.Enabled = false;

            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";

            for (i = 1; i < 13; i++)
            {
                ssView_Sheet1.Cells[i - 1, 0].Text = strYear + "." + i.ToString("00") + "월";
            }
            ssView_Sheet1.Cells[12, 0].Text = "합 계";

            try
            {
                //월별로 금액을 누적
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT YYMM,SUM(IwolAmt) cIwolAmt, SUM(MisuAmt) cMisuAmt, SUM(IpgumAmt) cIpgumAmt,";
                SQL = SQL + ComNum.VBLF + "        SUM(SakAmt) cSakAmt,  SUM(BanAmt+EtcAmt) cEtcAmt, SUM(JanAmt) cJanAmt";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GELTOT";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND YYMM >= '" + strYear + "01'";
                SQL = SQL + ComNum.VBLF + "    AND YYMM <= '" + strYear + "12'";
                if (rdoBi0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND Class = '01'";
                }
                else if (rdoBi1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND Class = '02'";
                }
                else if (rdoBi2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND Class = '03'";
                }
                else if (rdoBi3.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND Class = '04'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND Class < '05'";
                }
                SQL = SQL + ComNum.VBLF + "  GROUP BY YYMM";

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

                nRead = dt.Rows.Count;

                for (i = 0; i < nRead; i++)
                {
                    k = (int)VB.Val(ComFunc.RightH(dt.Rows[i]["YYMM"].ToString().Trim(), 2));

                    nTotAmt[k, 1] += VB.Val(dt.Rows[i]["cIwolAmt"].ToString().Trim());
                    nTotAmt[k, 2] += VB.Val(dt.Rows[i]["cMisuAmt"].ToString().Trim());
                    nTotAmt[k, 3] += VB.Val(dt.Rows[i]["cIpgumAmt"].ToString().Trim());
                    nTotAmt[k, 4] += VB.Val(dt.Rows[i]["cSakAmt"].ToString().Trim());
                    nTotAmt[k, 5] += VB.Val(dt.Rows[i]["cEtcAmt"].ToString().Trim());
                    nTotAmt[k, 6] += VB.Val(dt.Rows[i]["cJanAmt"].ToString().Trim());

                    nTotAmt[13, 1] += VB.Val(dt.Rows[i]["cIwolAmt"].ToString().Trim());
                    nTotAmt[13, 2] += VB.Val(dt.Rows[i]["cMisuAmt"].ToString().Trim());
                    nTotAmt[13, 3] += VB.Val(dt.Rows[i]["cIpgumAmt"].ToString().Trim());
                    nTotAmt[13, 4] += VB.Val(dt.Rows[i]["cSakAmt"].ToString().Trim());
                    nTotAmt[13, 5] += VB.Val(dt.Rows[i]["cEtcAmt"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;

                //합계를 기준으로 잔액을 계산
                nTotAmt[13, 1] = nTotAmt[1, 1];
                nTotAmt[13, 6] = nTotAmt[13, 1] + nTotAmt[13, 2] - nTotAmt[13, 3];
                nTotAmt[13, 6] = nTotAmt[13, 6] - nTotAmt[13, 4] - nTotAmt[13, 5];

                //저장한 내용을 Display
                for (i = 1; i < 14; i++)
                {
                    for (k = 1; k < 7; k++)
                    {
                        ssView_Sheet1.Cells[i - 1, k].Text = nTotAmt[i, k].ToString("###,###,###,##0 ");
                    }
                }

                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            string strMisu = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strPrintName = clsPrint.gGetDefaultPrinter();//기본프린터 이름을 가져온다


            if (strPrintName != "")
            {
                strTitle = "미수종류별 연간통계";

                strMisu = "미수구분 : ";
                if (rdoBi0.Checked == true)
                {
                    strMisu += "공단";
                }
                else if (rdoBi1.Checked == true)
                {
                    strMisu += "직장";
                }
                else if (rdoBi2.Checked == true)
                {
                    strMisu += "지역";
                }
                else if (rdoBi3.Checked == true)
                {
                    strMisu += "보호";
                }
                else
                {
                    strMisu += "전체";
                }

                strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += CS.setSpdPrint_String("출력시간 : " + VB.Now().ToString() + VB.Space(2) + strMisu, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += CS.setSpdPrint_String("출력자 : " + clsType.User.JobName + " 인 " + VB.Space(15), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter, strPrintName);
            }
        }

        private void frmPmpaTongClassYear_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            int nYY = 0;
            string strSysDate = "";

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon,"D"), "D");

            nYY = (int)VB.Val(VB.Left(strSysDate, 4));

            cboYear.Items.Clear();
            for (i = 1; i < 6; i++)
            {
                cboYear.Items.Add(nYY.ToString("0000") + " 년도");
                nYY -= 1;
            }
            cboYear.SelectedIndex = 0;
        }
    }
}
