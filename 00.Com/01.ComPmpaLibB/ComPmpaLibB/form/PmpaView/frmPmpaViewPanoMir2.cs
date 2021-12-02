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
    /// File Name       : frmPmpaViewPanoMir2.cs
    /// Description     : 등록번호별 청구차액 조회
    /// Author          : 박창욱
    /// Create Date     : 2017-09-27
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\misubs\misubs76.frm(FrmPanoMirView2.frm) >> frmPmpaViewPanoMir2.cs 폼이름 재정의" />
    public partial class frmPmpaViewPanoMir2 : Form
    {
        clsSpread CS = new clsSpread();
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsPmpaFunc cpf = new clsPmpaFunc();

        string GstrRetValue = "";

        public frmPmpaViewPanoMir2()
        {
            InitializeComponent();
        }

        public frmPmpaViewPanoMir2(string strRetValue)
        {
            InitializeComponent();
            GstrRetValue = strRetValue;
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
            string strGbn = "";
            bool PrePrint = true;

            strTitle = "등록번호별 청구차액 조회";

            if (rdoIO0.Checked == true)
            {
                strGbn = strGbn + "[구분]: 입원";
            }
            else
            {
                strGbn = strGbn + "[구분]: 외래";
            }

            if (rdoInsu0.Checked == true)
            {
                strGbn = strGbn + " [보험종류]: 건강보험,의료급여";
            }
            else if (rdoInsu1.Checked == true)
            {
                strGbn = strGbn + " [보험종류]: 산재";
            }
            else if (rdoInsu2.Checked == true)
            {
                strGbn = strGbn + " [보험종류]: 자보";
            }
            else
            {
                strGbn = strGbn + " [보험종류]: 전체";
            }

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String(strGbn, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

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

            int nRead = 0;

            if (txtPano.Text.Length != 8)
            {
                ComFunc.MsgBox("등록번호를 입력하세요");
                txtPano.Focus();
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //개인별 청구 차액 을 표시
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT YYMM, PANO ,BI, SNAME, IPDOPD, SUBI,TOTAMT,JOHAP, JEPJAMT";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND PANO ='" + txtPano.Text + "'";
                if (rdoIO0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND IpdOpd='I' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " AND IpdOpd='O' ";
                }

                if (rdoInsu0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND SUBI IN ('1','2')"; //보험보호
                }
                else if (rdoInsu1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND SUBI IN ('3')";     //산재
                }
                else if (rdoInsu2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND SUBI IN ('4')";     //자보
                }
                SQL = SQL + ComNum.VBLF + " UNION ALL        ";
                SQL = SQL + ComNum.VBLF + "SELECT YYMM, PANO ,BI, SNAME, '', SUBI,BUILdtAMT TOTAMT ,BUILDJAMT JOHAP, JEPJAMT";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MIR_IPDID ";
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND PANO ='" + txtPano.Text + "'";
                SQL = SQL + ComNum.VBLF + "   AND YYMM>='200112'";
                SQL = SQL + ComNum.VBLF + "   AND Flag='1' "; //청구Build한 내역
                if (rdoInsu0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND SUBI IN ('1','2')"; //보험보호
                }
                else if (rdoInsu1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND SUBI IN ('3')";     //산재
                }
                else if (rdoInsu2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND SUBI IN ('4')";     //자보
                }
                SQL = SQL + ComNum.VBLF + " ORDER BY YYMM DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRead = dt.Rows.Count;
                ssView_Sheet1.RowCount = nRead;

                for (i = 0; i < nRead; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["YYMM"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["BI"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    switch (dt.Rows[i]["IPDOPD"].ToString().Trim())
                    {
                        case "O":
                            ssView_Sheet1.Cells[i, 4].Text = "외래";
                            break;
                        case "I":
                            ssView_Sheet1.Cells[i, 4].Text = "퇴원";
                            break;
                        default:
                            ssView_Sheet1.Cells[i, 4].Text = "중간";
                            break;
                    }
                    ssView_Sheet1.Cells[i, 5].Text = VB.Val(dt.Rows[i]["TOTAMT"].ToString().Trim()).ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[i, 6].Text = VB.Val(dt.Rows[i]["JOHAP"].ToString().Trim()).ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[i, 7].Text = VB.Val(dt.Rows[i]["JEPJAMT"].ToString().Trim()).ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[i, 8].Text = (VB.Val(dt.Rows[i]["JEPJAMT"].ToString().Trim()) -
                                                      VB.Val(dt.Rows[i]["JOHAP"].ToString().Trim())).ToString("###,###,###,##0");
                }
                ssView.Enabled = true;
                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        private void frmPmpaViewPanoMir2_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            txtPano.Text = "";
            lblSName.Text = "";

            if (GstrRetValue != "")
            {
                if (VB.Right(GstrRetValue, 1) == "O")
                {
                    rdoIO1.Checked = true;
                }
                else
                {
                    rdoIO0.Checked = true;
                }
                txtPano.Text = VB.Left(GstrRetValue, 8);
                switch (VB.Mid(GstrRetValue, 9, 2))
                {
                    case "31":
                        rdoInsu1.Checked = true;
                        break;
                    case "52":
                        rdoInsu2.Checked = true;
                        break;
                    default:
                        rdoInsu0.Checked = true;
                        break;
                }

                #region 김해수 2018-09-12 퇴원,중간청구액 차액점검 및변경 자동조회

                if (txtPano.Text != "" && GstrRetValue != "")
                {
                    txtPano.Text = VB.Val(txtPano.Text).ToString("00000000");

                    if (cpf.Get_BasPatient(clsDB.DbCon, txtPano.Text).Rows.Count == 0)
                    {
                        ComFunc.MsgBox("등록되지 않은 등록번호입니다.");
                        txtPano.Focus();
                        return;
                    }
                    lblSName.Text = cpf.Get_BasPatient(clsDB.DbCon, txtPano.Text).Rows[0]["SName"].ToString().Trim();

                    Search_Data();
                    GstrRetValue = "";
                }
                #endregion
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            GstrRetValue = "";
            this.Close();
        }

        private void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssView, e.Column);
                return;
            }

            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssView_Sheet1.Cells[e.Row, 0, e.Row, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            GstrRetValue = VB.Left(ssView_Sheet1.Cells[e.Row, 8].ToString().Trim() + VB.Space(10), 10);     //EDI 접수일
            GstrRetValue += VB.Val(ssView_Sheet1.Cells[e.Row, 11].ToString().Trim()).ToString("0000000000");    //WRTNO
            GstrRetValue += VB.Val(ssView_Sheet1.Cells[e.Row, 4].ToString().Trim()).ToString("0000000000");     //조합부담

            this.Close();
        }

        private void txtPano_Enter(object sender, EventArgs e)
        {
            lblSName.Text = "";
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;
            ssView.Enabled = false;
            if (txtPano.Text != "" && GstrRetValue != "")
            {
                txtPano.Text = VB.Val(txtPano.Text).ToString("00000000");

                if (cpf.Get_BasPatient(clsDB.DbCon, txtPano.Text).Rows.Count == 0)
                {
                    ComFunc.MsgBox("등록되지 않은 등록번호입니다.");
                    txtPano.Focus();
                    return;
                }
                lblSName.Text = cpf.Get_BasPatient(clsDB.DbCon, txtPano.Text).Rows[0]["SName"].ToString().Trim();

                Search_Data();
                GstrRetValue = "";
            }
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            if (txtPano.Text.Trim() == "")
            {
                return;
            }

            txtPano.Text = VB.Val(txtPano.Text).ToString("00000000");

            if (cpf.Get_BasPatient(clsDB.DbCon, txtPano.Text).Rows.Count == 0)
            {
                ComFunc.MsgBox("등록되지 않은 등록번호입니다.");
                txtPano.Focus();
                return;
            }
            lblSName.Text = cpf.Get_BasPatient(clsDB.DbCon, txtPano.Text).Rows[0]["SName"].ToString().Trim();
            btnSearch.Focus();
        }
    }
}
