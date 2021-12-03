using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewBSTList.cs
    /// Description     : 당뇨약사용내역
    /// Author          : 박창욱
    /// Create Date     : 2017-10-30
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 2010-10-30 박창욱 : FrmBSTList, FrmBSTList2 통합
    /// </history>
    /// <seealso cref= "\mtsEmr\FrmBSTList.frm(FrmBSTList.frm) >> frmPmpaViewBSTList.cs 폼이름 재정의" />
    /// <seealso cref= "\IPD\iusent\FrmBSTList_IPD.frm(FrmBSTList.frm) >> frmPmpaViewBSTList.cs 폼이름 재정의" />
    /// <seealso cref= "\mtsEmr\FrmBSTList2.frm(FrmBSTList2.frm) >> frmPmpaViewBSTList.cs 폼이름 재정의" />
    public partial class frmPmpaViewBSTList : Form
    {
        clsPmpaFunc cpf = new clsPmpaFunc();
        string GstrHelpCode = "";
        
        public delegate void SetKK(string strDate, string strKK, int nK);
        public static event SetKK rSetKK;

        //public delegate void SetDate(string strDate);
        //public event SetDate rSetDate;
        
        //public delegate void SetNK(double strNK);
        //public event SetNK rSetNK;

        //public delegate void EventClose();
        //public event EventClose rEventClose;

        public frmPmpaViewBSTList()
        {
            InitializeComponent();
            SetEvent();
        }

        private void SetEvent()
        {
            this.ssView.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(ssView_CellDoubleClick);
        }

        public frmPmpaViewBSTList(string strHelpCode)
        {
            InitializeComponent();
            SetEvent();
            GstrHelpCode = strHelpCode;
        }

        private void frmPmpaViewBSTList_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            string strSysDate = "";

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            txtPtno.Text = "";
            lblName.Text = "";

            dtpFDate.Value = Convert.ToDateTime(strSysDate).AddDays(-7);
            dtpTDate.Value = Convert.ToDateTime(strSysDate);

            if (GstrHelpCode != "")
            {
                txtPtno.Text = VB.Split(GstrHelpCode)[0];
                dtpFDate.Value = Convert.ToDateTime(VB.Split(GstrHelpCode)[1]);
                dtpTDate.Value = Convert.ToDateTime(VB.Split(GstrHelpCode)[2]);
                lblName.Text = cpf.Get_BasPatient(clsDB.DbCon, txtPtno.Text).Rows[0]["Sname"].ToString().Trim();
            }

            Search_Data();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            //rEventClose();
            this.Close();
            return;
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

            ssView_Sheet1.RowCount = 0;

            if (txtPtno.Text == "")
            {
                return;
            }

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT CHARTDATE, SUM(HHUM1) HHUM1, SUM(SC) SC";
                SQL = SQL + ComNum.VBLF + "  FROM (";
                SQL = SQL + ComNum.VBLF + "        SELECT CHARTDATE,";
                SQL = SQL + ComNum.VBLF + "        CASE";
                SQL = SQL + ComNum.VBLF + "         WHEN TRIM(UPPER(EXTRACTVALUE(CHARTXML,'//ta4'))) = 'H-HUMI' THEN TO_NUMBER(EXTRACTVALUE(CHARTXML,'//ta5'))";
                SQL = SQL + ComNum.VBLF + "         WHEN TRIM(UPPER(EXTRACTVALUE(CHARTXML,'//ta4'))) = 'H-HUM1' THEN TO_NUMBER(EXTRACTVALUE(CHARTXML,'//ta5'))";
                SQL = SQL + ComNum.VBLF + "        ELSE 0";
                SQL = SQL + ComNum.VBLF + "        END HHUM1,";
                SQL = SQL + ComNum.VBLF + "        CASE";
                SQL = SQL + ComNum.VBLF + "         WHEN TRIM(UPPER(EXTRACTVALUE(CHARTXML,'//ta4'))) IS NOT NULL THEN 1";
                SQL = SQL + ComNum.VBLF + "        ELSE 0";
                SQL = SQL + ComNum.VBLF + "        END SC";
                SQL = SQL + ComNum.VBLF + "        FROM " + ComNum.DB_EMR + "EMRXML";
                SQL = SQL + ComNum.VBLF + "        WHERE EMRNO IN (";
                SQL = SQL + ComNum.VBLF + "                        SELECT EMRNO FROM " + ComNum.DB_EMR + "EMRXMLMST";
                SQL = SQL + ComNum.VBLF + "                        WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "                        AND FORMNO = '1572'";
                SQL = SQL + ComNum.VBLF + "                        AND PTNO = '" + txtPtno.Text + "' ";
                SQL = SQL + ComNum.VBLF + "                        AND INOUTCLS = 'I' )";
                SQL = SQL + ComNum.VBLF + "        AND CHARTDATE >= '" + dtpFDate.Value.ToString("yyyy-MM-dd").Replace("-", "") + "'";
                SQL = SQL + ComNum.VBLF + "        AND CHARTDATE <= '" + dtpTDate.Value.ToString("yyyy-MM-dd").Replace("-", "") + "'";

                SQL = SQL + ComNum.VBLF + "        UNION ALL ";

                SQL = SQL + ComNum.VBLF + "        SELECT C.CHARTDATE,   ";
                SQL = SQL + ComNum.VBLF + "            (SELECT  ";
                SQL = SQL + ComNum.VBLF + "                CASE  ";
                SQL = SQL + ComNum.VBLF + "                    WHEN R.ITEMVALUE = 'H-HUMI' THEN  ";
                SQL = SQL + ComNum.VBLF + "                               TO_NUMBER((SELECT R1.ITEMVALUE FROM " + ComNum.DB_EMR + "AEMRCHARTROW R1 ";
                SQL = SQL + ComNum.VBLF + "                                         WHERE R1.EMRNO = R.EMRNO AND R1.ITEMCD = 'I0000035480') ) ";
                SQL = SQL + ComNum.VBLF + "                    ELSE 0 ";
                SQL = SQL + ComNum.VBLF + "                END  ";
                SQL = SQL + ComNum.VBLF + "              FROM " + ComNum.DB_EMR + "AEMRCHARTROW R ";
                SQL = SQL + ComNum.VBLF + "              WHERE R.EMRNO = C.EMRNO ";
                SQL = SQL + ComNum.VBLF + "                  AND R.ITEMCD = 'I0000004686'  ";
                SQL = SQL + ComNum.VBLF + "            ) AS HHUM1,   ";
                SQL = SQL + ComNum.VBLF + "            (SELECT  ";
                SQL = SQL + ComNum.VBLF + "                CASE  ";
                SQL = SQL + ComNum.VBLF + "                    WHEN R.ITEMVALUE IS NOT NULL THEN 1 ";
                SQL = SQL + ComNum.VBLF + "                    ELSE 0 ";
                SQL = SQL + ComNum.VBLF + "                END  ";
                SQL = SQL + ComNum.VBLF + "              FROM " + ComNum.DB_EMR + "AEMRCHARTROW R ";
                SQL = SQL + ComNum.VBLF + "              WHERE R.EMRNO = C.EMRNO ";
                SQL = SQL + ComNum.VBLF + "                  AND R.ITEMCD = 'I0000004686'  ";
                SQL = SQL + ComNum.VBLF + "            ) AS SC ";
                SQL = SQL + ComNum.VBLF + "        FROM " + ComNum.DB_EMR + "AEMRCHARTMST C ";
                SQL = SQL + ComNum.VBLF + "        WHERE C.PTNO = '" + txtPtno.Text + "' ";
                SQL = SQL + ComNum.VBLF + "            AND C.FORMNO = '1572' ";
                SQL = SQL + ComNum.VBLF + "            AND CHARTDATE >= '" + dtpFDate.Value.ToString("yyyy-MM-dd").Replace("-", "") + "'";
                SQL = SQL + ComNum.VBLF + "            AND CHARTDATE <= '" + dtpTDate.Value.ToString("yyyy-MM-dd").Replace("-", "") + "'";
                SQL = SQL + ComNum.VBLF + "        ) ";
                SQL = SQL + ComNum.VBLF + " GROUP BY CHARTDATE";
                SQL = SQL + ComNum.VBLF + "HAVING SUM(HHUM1) + SUM(SC) <> 0 ";
                SQL = SQL + ComNum.VBLF + " ORDER BY CHARTDATE";

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
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    //ssView_Sheet1.Cells[i, 0].Text = Convert.ToDateTime(dt.Rows[i]["CHARTDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                    ssView_Sheet1.Cells[i, 0].Text = VB.Left(dt.Rows[i]["CHARTDATE"].ToString().Trim(), 4);
                    ssView_Sheet1.Cells[i, 0].Text += "-" + VB.Mid(dt.Rows[i]["CHARTDATE"].ToString().Trim(), 5, 2);
                    ssView_Sheet1.Cells[i, 0].Text += "-" + VB.Mid(dt.Rows[i]["CHARTDATE"].ToString().Trim(), 7, 2);
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["HHUM1"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SC"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.RowHeader == true || e.ColumnHeader == true)
            {
                return;
            }
            
            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssView_Sheet1.Cells[e.Row, 0, e.Row, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            int nH = 0;
            int nK = 0;
            string strDate = "";

            strDate = ssView_Sheet1.Cells[e.Row, 0].Text.Trim();
            nH = (int)VB.Val(ssView_Sheet1.Cells[e.Row, 1].Text.Trim());
            nK = (int)VB.Val(ssView_Sheet1.Cells[e.Row, 2].Text.Trim());
            
            if (nK != 0)
            {
                rSetKK(strDate, "KK010", nK);
                //rSetDate(strDate);
                //rSetNK(nK);
            }
        }

        private void txtPtno_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            lblName.Text = "";
            txtPtno.Text = ComFunc.LPAD(txtPtno.Text, 8, "0");
            lblName.Text = cpf.Get_BasPatient(clsDB.DbCon, txtPtno.Text).Rows[0]["Sname"].ToString().Trim();
            btnSearch.Focus();
        }
    }
}
