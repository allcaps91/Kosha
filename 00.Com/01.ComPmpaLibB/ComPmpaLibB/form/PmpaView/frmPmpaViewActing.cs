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
    /// File Name       : frmPmpaViewActing.cs
    /// Description     : 간호ActingView
    /// Author          : 박창욱
    /// Create Date     : 2017-10-30
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 2017-10-30 박창욱 : FrmActingView 폼 통합
    /// </history>
    /// <seealso cref= "\IPD\ipdSim2\FrmActingView.frm(FrmActingView.frm) >> frmPmpaViewActing.cs 폼이름 재정의" />
    /// <seealso cref= "\IPD\iusent\FrmActingView.frm(FrmActingView.frm) >> frmPmpaViewActing.cs 폼이름 재정의" />
    public partial class frmPmpaViewActing : Form
    {
        string GstrPANO = "";

        public frmPmpaViewActing()
        {
            InitializeComponent();
        }

        public frmPmpaViewActing(string strPano)
        {
            InitializeComponent();

            GstrPANO = strPano;
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

            strTitle = "간호 Acting View";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("등록번호 : " + txtPano.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("작업일자 : " + dtpFDate.Value.ToString("yyyy-MM-dd") + " ~ " + dtpTDate.Value.ToString("yyyy-MM-dd"), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search_Data();
        }

        void Search_Data()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nRow = 0;
            double nRealQty = 0;
            double nQty = 0;
            double nNal = 0;
            double nActDiv = 0;
            double nDivQty = 0;
            string strSuCode = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT A.BDATE, A.SUCODE, A.CONTENTS,";
                SQL = SQL + ComNum.VBLF + "       A.BCONTENTS, A.REALQTY, A.QTY,";
                SQL = SQL + ComNum.VBLF + "       A.NAL, A.GBDIV, A.ACTDIV, ";
                SQL = SQL + ComNum.VBLF + "       A.DIVQTY , A.ACTSABUN, A.GBSTATUS,";
                SQL = SQL + ComNum.VBLF + "       A.ORDERNO,B.SUNAMEK, C.KORNAME,";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(A.ACTTIME, 'YYYY-MM-DD HH24:MI') ACTTIME";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_IORDER_ACT A, " + ComNum.DB_PMPA + "BAS_SUN B, " + ComNum.DB_ERP + "INSA_MST C ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND A.PTNO ='" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.BDATE >=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND A.BDATE <=TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND ( A.GBSTATUS NOT IN 'D+'  OR A.GBSTATUS IS NULL)";

                if (rdoBUN0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.BUN IN ('11','12','20','23')";
                }
                if (rdoBUN1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.BUN IN ('11')";
                }
                if (rdoBUN2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.BUN IN ('20','23')";
                }
                if (rdoBUN3.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.BUN IN ('12')";
                }

                SQL = SQL + ComNum.VBLF + "   AND A.SUCODE = B.SUNEXT ";
                SQL = SQL + ComNum.VBLF + "   AND A.ACTSABUN = C.SABUN ";

                if (rdoGB0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY A.BDATE,  A.ORDERNO, A.SUCODE ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY A.SUCODE , A.ORDERNO , A.BDATE ";
                }

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

                ssView_Sheet1.RowCount = 0;
                ssView_Sheet1.RowCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (chkSum.Checked == true)
                    {
                        if (i == 0)
                        {
                            strSuCode = dt.Rows[i]["SuCode"].ToString().Trim();
                        }

                        if (strSuCode != dt.Rows[i]["SuCode"].ToString().Trim())
                        {
                            nRow += 1;
                            if (ssView_Sheet1.RowCount < nRow)
                            {
                                ssView_Sheet1.RowCount = nRow;
                            }
                            ssView_Sheet1.Cells[nRow - 1, 2].Text = "** 소  계  **";
                            ssView_Sheet1.Cells[nRow - 1, 9].Text = nActDiv.ToString();
                            ssView_Sheet1.Cells[nRow - 1, 10].Text = nDivQty.ToString();
                            strSuCode = dt.Rows[i]["SuCode"].ToString().Trim();
                            nRealQty = 0;
                            nQty = 0;
                            nNal = 0;
                            nActDiv = 0;
                            nDivQty = 0;
                        }
                    }
                    nRow += 1;
                    if (ssView_Sheet1.RowCount < nRow)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }
                    ssView_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["BDate"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["Sucode"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["SunameK"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["Contents"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["BCONTENTS"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["realqty"].ToString().Trim();
                    nRealQty += VB.Val(dt.Rows[i]["realqty"].ToString().Trim());
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["Qty"].ToString().Trim();
                    nQty += VB.Val(dt.Rows[i]["Qty"].ToString().Trim());
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["Nal"].ToString().Trim();
                    nNal += VB.Val(dt.Rows[i]["Nal"].ToString().Trim());
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = dt.Rows[i]["GBDIV"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 9].Text = "1";
                    nActDiv += 1;
                    ssView_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["divqty"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 11].Text = dt.Rows[i]["korname"].ToString().Trim() + "  " + dt.Rows[i]["ACTTIME"].ToString().Trim();
                }

                if (chkSum.Checked == true)
                {
                    nRow += 1;
                    if (ssView_Sheet1.RowCount < nRow)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = "** 소  계 **";
                    ssView_Sheet1.Cells[nRow - 1, 9].Text = nActDiv.ToString();
                    ssView_Sheet1.Cells[nRow - 1, 10].Text = nDivQty.ToString();
                }

                dt.Dispose();
                dt = null;

                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmPmpaViewActing_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            if (VB.Left(clsPmpaType.TIT.InDate, 10) != "")
            {
                dtpFDate.Value = Convert.ToDateTime(VB.Left(clsPmpaType.TIT.InDate, 10));
            }
            else
            {
                dtpFDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
            }
            dtpTDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
            if (GstrPANO != "")
            {
                txtPano.Text = GstrPANO;
                Search_Data();
            }
            else
            {
                txtPano.Text = "";
            }
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }
            txtPano.Text = ComFunc.LPAD(txtPano.Text, 8, "0");
            dtpFDate.Focus();
        }

        private void txtPano_Leave(object sender, EventArgs e)
        {
            txtPano.Text = ComFunc.LPAD(txtPano.Text, 8, "0");
        }
    }
}
