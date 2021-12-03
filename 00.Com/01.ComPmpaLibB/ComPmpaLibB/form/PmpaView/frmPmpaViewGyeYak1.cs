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
    /// File Name       : frmPmpaViewGyeYak1.cs
    /// Description     : 계약처 미수내역 인쇄
    /// Author          : 박창욱
    /// Create Date     : 2017-10-18
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\IPD\ilrepd\ILREPD07.FRM(FrmGyeYak1.frm) >> frmPmpaViewGyeYak1.cs 폼이름 재정의" />	
    public partial class frmPmpaViewGyeYak1 : Form
    {
        public frmPmpaViewGyeYak1()
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

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = cboYM.Text + " 계약처별 미수금 내역";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력자:" + clsType.User.JobName, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
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

            int nReadCnt = 0;
            double nMisuAmt = 0;
            double nTotal = 0;
            double nTotChong = 0;
            double nTotJohap = 0;
            double nTotBonin = 0;
            double nGum1 = 0;
            double nGum2 = 0;
            double nGum3 = 0;
            double nGum4 = 0;
            string strDate1 = "";
            string StrDate2 = "";
            string strPANO = "";
            string strDeptCode = "";
            string strActDate = "";
            string strMsg = "";
            string strSname = "";
            string strGelCode = "";

            clsPmpaFunc cpf = new clsPmpaFunc();

            strDate1 = VB.Left(cboYM.Text, 4) + "-" + VB.Mid(cboYM.Text, 7, 2) + "-01";
            StrDate2 = clsVbfunc.LastDay(Convert.ToInt32(VB.Left(cboYM.Text, 4)), Convert.ToInt32(VB.Mid(cboYM.Text, 7, 2)));

            nTotal = 0;
            nTotChong = 0;
            nTotJohap = 0;
            nTotBonin = 0;

            ssView_Sheet1.RowCount = 0;

            try
            {
                //입원
                #region View_IPD_Select

                //입원환자 계약처 Select
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT a.Pano, a.Bi, a.Amt,";
                SQL = SQL + ComNum.VBLF + "        c.Sname, b.DeptCode, b.Ilsu,";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(b.InDate,'YY-MM-DD') InIlja, TO_CHAR(b.OutDate,'YY-MM-DD') OutIlja, b.Amt50,";
                SQL = SQL + ComNum.VBLF + "        b.Amt53, b.Amt55, b.Amt56,";
                SQL = SQL + ComNum.VBLF + "        a.GelCode";
                //SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "WORK_IPD_CASH a, " + ComNum.DB_PMPA + "WORK_IPD_TRANS b, " + ComNum.DB_PMPA + "BAS_PATIENT c";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH a, " + ComNum.DB_PMPA + "IPD_TRANS b, " + ComNum.DB_PMPA + "BAS_PATIENT c";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND a.ActDate >= TO_DATE('" + strDate1 + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND a.ActDate <= TO_DATE('" + StrDate2 + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND a.SuNext = 'Y96J'";
                SQL = SQL + ComNum.VBLF + "    AND a.Amt <> 0";
                SQL = SQL + ComNum.VBLF + "    AND a.TRSNO=b.TRSNO(+)";
                SQL = SQL + ComNum.VBLF + "    AND a.Pano=c.Pano(+)";
                SQL = SQL + ComNum.VBLF + "  ORDER BY b.GelCode,a.Pano,b.OutDate";
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

                nReadCnt = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nMisuAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    if (nMisuAmt != 0)
                    {
                        ssView_Sheet1.RowCount += 1;
                        strPANO = dt.Rows[i]["Pano"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = strPANO;
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["Sname"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = cpf.GET_BAS_MIA(clsDB.DbCon, dt.Rows[i]["Gelcode"].ToString().Trim());
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["InIlJa"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["OutIlJa"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = VB.Val(dt.Rows[i]["Ilsu"].ToString().Trim()).ToString("### ");
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = "입원";
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, dt.Rows[i]["DeptCode"].ToString().Trim());
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = nMisuAmt.ToString("###,###,##0 ");
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = VB.Val(dt.Rows[i]["Amt50"].ToString().Trim()).ToString("###,###,##0 ");
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = VB.Val(dt.Rows[i]["Amt53"].ToString().Trim()).ToString("###,###,##0 ");
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = VB.Val(dt.Rows[i]["Amt55"].ToString().Trim()).ToString("###,###,##0 ");
                        nTotal += nMisuAmt;
                        nTotChong += VB.Val(dt.Rows[i]["Amt50"].ToString().Trim());
                        nTotJohap += VB.Val(dt.Rows[i]["Amt53"].ToString().Trim());
                        nTotBonin += VB.Val(dt.Rows[i]["Amt55"].ToString().Trim());
                    }
                }
                dt.Dispose();
                dt = null;

                #endregion

                //외래
                #region View_OPD_Select

                //외래환자 계약처 Select
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(ActDate,'YYYY-MM-DD') ActDate, Part, SeqNo,";
                SQL = SQL + ComNum.VBLF + "        Pano, Bi, DeptCode,";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(BDate,'YY-MM-DD') InIlja, Amt1+Amt2 MAmt";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND ActDate >= TO_DATE('" + strDate1 + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND ActDate <= TO_DATE('" + StrDate2 + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND SuNext   = 'Y96J'";
                SQL = SQL + ComNum.VBLF + "  ORDER BY Pano,ActDate";

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

                nReadCnt = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strActDate = dt.Rows[i]["ActDate"].ToString().Trim();
                    strPANO = dt.Rows[i]["Pano"].ToString().Trim();
                    strDeptCode = dt.Rows[i]["DeptCode"].ToString().Trim();

                    //해당 계약처의 환자인지 Check
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT Sname,GelCode";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "    AND ActDate = TO_DATE('" + strActDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "    AND Pano    = '" + strPANO + "'";
                    SQL = SQL + ComNum.VBLF + "    AND DeptCode= '" + strDeptCode + "'";
                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt1.Rows.Count == 0)
                    {
                        strMsg = "등록번호 : " + strPANO + ComNum.VBLF;
                        strMsg += "회계일자 : " + strActDate + ComNum.VBLF;
                        strMsg += "진 료 과 : " + strDeptCode + ComNum.VBLF;
                        strMsg += "미수금액 : " + dt.Rows[i]["MAmt"].ToString().Trim();
                        ComFunc.MsgBox(strMsg);
                        return;
                    }
                    else
                    {
                        strSname = dt1.Rows[0]["SNAME"].ToString().Trim();
                        strGelCode = dt1.Rows[0]["GELCODE"].ToString().Trim();
                    }

                    dt1.Dispose();
                    dt1 = null;

                    //1명의 총진료비, 조합부담, 본인부담, 미수액 Select
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT SUM(DECODE(SuNext,'Y96J    ',Amt1+Amt2,0)) Gum1,"; //미수
                    SQL = SQL + ComNum.VBLF + "        SUM(DECODE(BUN,98,Amt1+Amt2,0)) Gum2,";        //조합
                    SQL = SQL + ComNum.VBLF + "        SUM(DECODE(BUN,99,Amt1+Amt2,0)) Gum3 ";        //본인
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP";
                    SQL = SQL + ComNum.VBLF + "  WHERE ActDate = TO_DATE('" + strActDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "    AND Pano = '" + strPANO + "'";
                    SQL = SQL + ComNum.VBLF + "    AND DeptCode = '" + strDeptCode + "'";
                    SQL = SQL + ComNum.VBLF + "    AND Part = '" + dt.Rows[i]["Part"].ToString().Trim() + "'";
                    SQL = SQL + ComNum.VBLF + "    AND SeqNO = " + dt.Rows[i]["SeqNo"].ToString().Trim() + "";
                    SQL = SQL + ComNum.VBLF + "  GROUP BY ActDate,Pano,DeptCode";
                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    nGum1 = 0;
                    nGum2 = 0;
                    nGum3 = 0;
                    nGum4 = 0;

                    if (dt1.Rows.Count == 1)
                    {
                        nGum1 = VB.Val(dt1.Rows[0]["Gum1"].ToString().Trim());  //후불
                        nGum2 = VB.Val(dt1.Rows[0]["Gum2"].ToString().Trim());  //조합부담
                        nGum3 = VB.Val(dt1.Rows[0]["Gum3"].ToString().Trim());  //본인부담
                        nGum4 = nGum1 + nGum2 + nGum3;                          //총진료비
                    }

                    dt1.Dispose();
                    dt1 = null;

                    if (nGum1 != 0)
                    {
                        ssView_Sheet1.RowCount += 1;
                        strPANO = dt.Rows[i]["Pano"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = strPANO;
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = strSname.Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = cpf.GET_BAS_MIA(clsDB.DbCon, strGelCode.Trim());
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["InIlja"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = "";
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = "";
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = "외래";
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, dt.Rows[i]["DeptCode"].ToString().Trim());
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = nGum1.ToString("###,###,##0 ");   //미수액
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = nGum4.ToString("###,###,##0 ");   //총진료비
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = nGum2.ToString("###,###,##0 ");  //조합부담
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = nGum3.ToString("###,###,##0 ");  //본인부담
                        nTotal += nGum1;
                        nTotChong += nGum4;
                        nTotJohap += nGum2;
                        nTotBonin += nGum3;
                    }
                }
                dt.Dispose();
                dt = null;

                #endregion

                ssView_Sheet1.RowCount += 1;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "* 합계 *";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = nTotal.ToString("###,###,##0 ");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = nTotChong.ToString("###,###,##0 ");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = nTotJohap.ToString("###,###,##0 ");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = nTotBonin.ToString("###,###,##0 ");

                btnPrint.Focus();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmPmpaViewGyeYak1_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboYM, 12, "", "1");
            cboYM.SelectedIndex = 1;
        }
    }
}
