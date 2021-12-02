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
    /// File Name       : frmPmPaVIEWBansongPrint
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-09-27
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= D:\psmh\misu\misumir.vbp\MISUM306.FRM(FrmBansongPrint)" >> frmPmPaVIEWBansongPrint.cs 폼이름 재정의" />
    /// 
    public partial class frmPmPaVIEWBansongPrint : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaMisu CPM = new clsPmpaMisu();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmPmPaVIEWBansongPrint()
        {
            InitializeComponent();
        }

        private void frmPmPaVIEWBansongPrint_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            dtpFDate.Value = Convert.ToDateTime(CPM.DATE_ADD(clsDB.DbCon, strDTP, -2));
            dtpTDate.Value = Convert.ToDateTime(CPM.DATE_ADD(clsDB.DbCon, strDTP, -1));

            btnPrint.Enabled = false;
        }

        private void rdoIBi_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i = 0;
            int j = 0;
            int nRow = 0;
            string strYYMM = "";
            string strFDate = "";
            string strTDate = "";
            string strNewData = "";
            string strOldData = "";
            string strGubun = "";
            double nAmt = 0;
            double nQty = 0;
            double nTotIwolAmt = 0;
            double nTotMirAmt = 0;
            double nTotIpgumAmt = 0;
            double nTotJanAmt = 0;

            if (dtpTDate.Value < dtpFDate.Value)
            {
                ComFunc.MsgBox("종요일자가 시작일자 보다 작음", "확인");
                dtpFDate.Select();
                return;
            }

            strFDate = dtpFDate.Value.ToString("yyyy-MM-dd");
            strTDate = dtpTDate.Value.ToString("yyyy-MM-dd");

            nTotIwolAmt = 0;
            nTotMirAmt = 0;
            nTotIpgumAmt = 0;
            nTotJanAmt = 0;

            btnSearch.Enabled = false;
            btnPrint.Enabled = false;

            CmdOK_Slip_Display(ref strNewData, ref strOldData);

            btnSearch.Enabled = true;
            btnPrint.Enabled = true;
        }

        private void CmdOK_Slip_Display(ref string strNewData, ref string strOldData)  //' 미수 상세내역 Display
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(a.Bdate,'YYYY-MM-DD') Bdate,b.Class,b.GelCode,b.IpdOpd,a.Gubun,          ";
                SQL = SQL + ComNum.VBLF + "        b.MisuID,a.Qty,a.Amt,a.Remark,b.Bun,TO_CHAR(b.Bdate,'yyyy-mm-dd') MirDate  ";
                SQL = SQL + ComNum.VBLF + "   FROM MISU_SLIP a,MISU_IDMST b                                                   ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1=1";
                SQL = SQL + ComNum.VBLF + "    AND a.Bdate >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','yyyy-mm-dd')                        ";
                SQL = SQL + ComNum.VBLF + "    AND a.Bdate <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','yyyy-mm-dd')                       ";

                if (rdoIBi0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Class = '01'                                                         ";
                }
                else if (rdoIBi1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Class = '02'                                                         ";
                }
                else if (rdoIBi2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Class = '03'                                                         ";
                }
                else if (rdoIBi3.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Class = '04'                                                         ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Class < '05'                                                         ";
                }

                if (rdojob0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Gubun = '32'                                                         ";
                }
                else if (rdojob1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Gubun = '10'                                                         "; //'심사중
                }
                else if (rdojob2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Gubun IN ('32','10','09')                                            ";
                }
                else if (rdojob3.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Gubun = '10'                                                         ";
                    SQL = SQL + ComNum.VBLF + "    AND b.GbEnd = '1'                                                          ";
                }
                else if (rdojob4.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Gubun = '09'                                                         "; //'보류건
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Gubun = '09'                                                         ";
                    SQL = SQL + ComNum.VBLF + "    AND b.GbEnd = '1'                                                          ";
                }


                SQL = SQL + ComNum.VBLF + "    AND a.WRTNO = b.WRTNO(+)                                                       ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY a.Bdate,b.Class,b.GelCode,b.MisuID,a.Gubun                              ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;

                }

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strNewData = dt.Rows[i]["Bdate"].ToString().Trim();
                    strNewData = strNewData + dt.Rows[i]["Class"].ToString().Trim();
                    strNewData = strNewData + dt.Rows[i]["GelCode"].ToString().Trim();
                    strNewData = VB.Left(strNewData + VB.Space(20), 20);

                    if (VB.Left(strNewData, 12) != VB.Left(strOldData, 12))      //'일자,종류가 틀린경우
                    {
                        ssView_Sheet1.Cells[i, 0].Text = VB.Left(strNewData, 10);
                        ssView_Sheet1.Cells[i, 2].Text = CPM.READ_MisuClass(VB.Mid(strNewData, 11, 2));
                        ssView_Sheet1.Cells[i, 2].Text = CPM.READ_BAS_MIA(VB.Right(strNewData, 8));
                        strOldData = strNewData;
                    }
                    else if (strNewData != strOldData)                        //'조합이 틀린경우
                    {
                        ssView_Sheet1.Cells[i, 2].Text = CPM.READ_BAS_MIA(VB.Right(strNewData, 8));
                        strOldData = strNewData;
                    }
                    ssView_Sheet1.Cells[i, 3].Text = CPM.READ_MisuGye_TA(dt.Rows[i]["Gubun"].ToString().Trim());
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["MisuID"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5].Text = CPM.READ_MisuIpdOpd(dt.Rows[i]["IpdOpd"].ToString().Trim());
                    ssView_Sheet1.Cells[i, 6].Text = CPM.YYMM_SET(dt.Rows[i]["MirDate"].ToString().Trim());
                    ssView_Sheet1.Cells[i, 7].Text = CPM.READ_MisuBunya(dt.Rows[i]["Bun"].ToString().Trim());
                    ssView_Sheet1.Cells[i, 8].Text = VB.Val(dt.Rows[i]["Qty"].ToString().Trim()).ToString("###,###,##0 ");
                    ssView_Sheet1.Cells[i, 9].Text = VB.Val(dt.Rows[i]["Amt"].ToString().Trim()).ToString("###,###,###,##0 ");
                    ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["Remark"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
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
                return;
            }     //권한확인

            strTitle = "심사중 및 반송자 명단";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("작업기간 : " + dtpFDate.Value.ToString("yyyy-MM-dd") + " ~ " + dtpTDate.Value.ToString("yyyy-MM-dd"), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
