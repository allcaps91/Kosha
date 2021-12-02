using ComBase;
using ComBase.Controls;
using ComDbB;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : SupDrst
    /// File Name       : frmSupDrstChengGuViewNew.cs
    /// Description     : 의약품 재고 현황
    /// Author          : 이정현
    /// Create Date     : 2017-11-30
    /// <history> 
    /// 의약품 재고 현황
    /// </history>
    /// <seealso>
    /// PSMH\drug\drinfo\FrmChengGuViewNew.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\drinfo\drinfo.vbp
    /// </vbp>
    /// </summary>
    public partial class frmSupDrstChengGuViewNew : Form
    {
        public frmSupDrstChengGuViewNew()
        {
            InitializeComponent();
        }

        private void frmSupDrstChengGuViewNew_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            dtpFDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(-1);
            dtpTDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(-1);
            
            cboGu.Text = "";
            cboGu.Items.Clear();
            cboGu.Items.Add("00.전체");
            cboGu.Items.Add("01.주사약");
            cboGu.Items.Add("02.수액제");
            cboGu.Items.Add("03.경구약");
            cboGu.Items.Add("04.외용제");
            cboGu.Items.Add("05.생제약");
            cboGu.SelectedIndex = 0;

            ssList_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 0;

            GetJego();
        }

        private void btnJego_Click(object sender, EventArgs e)
        {
            GetJego();
        }

        private void GetJego()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssList_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     JEPCODE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_JEP ";
                SQL = SQL + ComNum.VBLF + "     Where GUBUN = '1' ";
                SQL = SQL + ComNum.VBLF + "         AND DELDATE IS NULL";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssList_Sheet1.RowCount = dt.Rows.Count;
                    ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["JEPCODE"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void cboGu_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (VB.Left(cboGu.Text, 2))
            {
                case "00":
                    cboBun.Items.Clear();
                    cboBun.Items.Add("00.전체");
                    break;
                case "01":
                    cboBun.Items.Clear();
                    cboBun.Items.Add("00.전체");
                    cboBun.Items.Add("01.일반주사약(1)");
                    cboBun.Items.Add("02.일반주사약(2)");
                    cboBun.Items.Add("03.소모적은약");
                    cboBun.Items.Add("04.마취과약");
                    cboBun.Items.Add("05.응급약");
                    cboBun.Items.Add("06.항생주사약");
                    cboBun.Items.Add("07.항암제");
                    cboBun.Items.Add("08.고가 주사약");
                    cboBun.Items.Add("09.냉장약-일반약");
                    cboBun.Items.Add("10.냉장약-항암제");
                    cboBun.Items.Add("11.냉장약-생제약");
                    break;
                case "02":
                    cboBun.Items.Clear();
                    cboBun.Items.Add("00.전체");
                    cboBun.Items.Add("21.일반수액제");
                    cboBun.Items.Add("22.고가수액제");
                    cboBun.Items.Add("23.영양수액제");
                    cboBun.Items.Add("24.관류용수액제");
                    break;
                case "03":
                    cboBun.Items.Clear();
                    cboBun.Items.Add("00.전체");
                    cboBun.Items.Add("31.일반정제");
                    cboBun.Items.Add("34.극약");
                    cboBun.Items.Add("36.항암정제");
                    cboBun.Items.Add("37.산제");
                    cboBun.Items.Add("38.시럽제");
                    cboBun.Items.Add("99.감염내과");
                    break;
                case "04":
                    cboBun.Items.Clear();
                    cboBun.Items.Add("00.전체");
                    cboBun.Items.Add("41.일반외용제");
                    cboBun.Items.Add("42.제제약");
                    cboBun.Items.Add("43.패취제");
                    cboBun.Items.Add("44.스프레이제");
                    cboBun.Items.Add("45.가글제");
                    cboBun.Items.Add("46.부인과질정");
                    cboBun.Items.Add("47.항문좌제");
                    cboBun.Items.Add("48.기타외용제");
                    cboBun.Items.Add("49.안약");
                    cboBun.Items.Add("50.고가외용제");
                    break;
                case "05":
                    cboBun.Items.Clear();
                    cboBun.Items.Add("00.**");
                    cboBun.Items.Add("51.주사약");
                    cboBun.Items.Add("52.외용약");
                    cboBun.Items.Add("53.경구약");
                    break;
                case "06":
                    cboBun.Items.Clear();
                    cboBun.Items.Add("00.전체");
                    cboBun.Items.Add("61.갬브로");
                    cboBun.Items.Add("62.박스터");
                    cboBun.Items.Add("63.보령제약");
                    cboBun.Items.Add("64.FMC");
                    cboBun.Items.Add("65.기타");
                    break;
                case "07":
                    cboBun.Items.Clear();
                    cboBun.Items.Add("00.전체");
                    cboBun.Items.Add("71.X-ray조영제");
                    cboBun.Items.Add("72.MRI조영제");
                    cboBun.Items.Add("73.기타");
                    break;
                case "08":
                    cboBun.Items.Clear();
                    cboBun.Items.Add("00.전체");
                    cboBun.Items.Add("81.향정주사약");
                    cboBun.Items.Add("82.향정경구약");
                    cboBun.Items.Add("83.향정외용약");
                    break;
                case "09":
                    cboBun.Items.Clear();
                    cboBun.Items.Add("00.전체");
                    cboBun.Items.Add("91.주사마약");
                    cboBun.Items.Add("92.경구마약");
                    cboBun.Items.Add("93.외용마약");
                    break;
                case "10":
                    cboBun.Items.Clear();
                    cboBun.Items.Add("00.전체");
                    cboBun.Items.Add("10.주사제");
                    cboBun.Items.Add("20.외용제");
                    break;
                case "11":
                    cboBun.Items.Clear();
                    cboBun.Items.Add("00.전체");
                    cboBun.Items.Add("A1.원료약품");
                    cboBun.Items.Add("A2.경구제제약");
                    cboBun.Items.Add("A3.외용제제약");
                    break;
            }

            cboBun.SelectedIndex = 0;
        }

        private string READ_LTD_NAME(PsmhDb pDbCon, string strCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            if (strCode.Trim() == "") { return rtnVal; }

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     Name";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "AIS_LTD";
                SQL = SQL + ComNum.VBLF + "     WHERE LTDCode = '" + strCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["Name"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

                return rtnVal;
            }
        }

        private string Read_BunName(PsmhDb pDbCon, string strCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            if (strCode.Trim() == "") { return rtnVal; }

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     BunName";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_BUN";
                SQL = SQL + ComNum.VBLF + "     WHERE GbBun = '2' ";
                SQL = SQL + ComNum.VBLF + "         AND BUN = '" + strCode + "' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY Bun ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["BunName"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

                return rtnVal;
            }
        }

        private void txtJepCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtJepCode.Text = txtJepCode.Text.ToUpper();

                if (txtJepCode.Text.Trim() == "") { return; }

                GetData();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            ssView_Sheet1.RowCount = 0;

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            double dblGENSOMO = 0;
            double dblDAYSOMO = 0;
            double dblOSOMO = 0;
            double dblISOMO = 0;
            double dblNEWJEGO = 0;
            double dblChsomo = 0;
            double dblChJogo = 0;

            string strJep = "";
            string strGu = VB.Left(cboGu.Text, 2);
            string strBun = VB.Left(cboBun.Text, 2);
            string strFDate = dtpFDate.Value.ToString("yyyy-MM-dd");
            string strFDate2 = dtpFDate.Value.ToString("yyyy-MM-01");
            string strMM = dtpFDate.Value.Month.ToString("00");
            string strFDate3 = "";

            if (strMM == "01")
            {
                strFDate3 = dtpFDate.Value.AddYears(-1).ToString("yyyy-12-01");
            }
            else
            {
                strFDate3 = dtpFDate.Value.AddMonths(-1).ToString("yyyy-MM-01");
            }

            string strTDate = dtpTDate.Value.ToString("yyyy-MM-dd");
            string strTDate3 = dtpTDate.Value.ToString("yyyy-MM-") + DateTime.DaysInMonth(dtpTDate.Value.Year, dtpTDate.Value.Month).ToString("00");
            string strYYMM = dtpFDate.Value.ToString("yyyyMM");

            string strNewGu = "";
            string strNewBun = "";
            string strGelCode = "";

            txtJepCode.Text = txtJepCode.Text.ToUpper();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";

                if (chk1.Checked == true)
                {
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.GUIPDRUG, A.JGELCODE, A.SUB, A.JEPCODE, A.COVQTY COVUNIT, A.JEPNAME, A.CHENGGU, A.CHENGBUN, A.JEPNAMEK, A.JEPNAME, A.JEPCODE";

                    #region //이름 
                    SQL = SQL + ComNum.VBLF + ",    (";
                    SQL = SQL + ComNum.VBLF + "SELECT NAME";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "AIS_LTD ";
                    SQL = SQL + ComNum.VBLF + " WHERE LTDCODE = A.GUIPDRUG";
                    SQL = SQL + ComNum.VBLF + " ) AS GUIP_NM";

                    SQL = SQL + ComNum.VBLF + ",    (";
                    SQL = SQL + ComNum.VBLF + "SELECT NAME";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "AIS_LTD ";
                    SQL = SQL + ComNum.VBLF + " WHERE LTDCODE = A.JGELCODE";
                    SQL = SQL + ComNum.VBLF + " ) AS GELNAME";


                    SQL = SQL + ComNum.VBLF + ",    (";
                    SQL = SQL + ComNum.VBLF + "SELECT BUNNAME";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "DRUG_BUN ";
                    SQL = SQL + ComNum.VBLF + " WHERE GBBUN  = '2'";
                    SQL = SQL + ComNum.VBLF + "   AND BUN    = A.SUB";
                    SQL = SQL + ComNum.VBLF + "   AND ROWNUM = 1";
                    SQL = SQL + ComNum.VBLF + " ) AS BUNNM";
                    #endregion


                    #region //전월 소모량(출고)
                    SQL = SQL + ComNum.VBLF + ",    (";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(IpchGbn, '6', Qty, '7', Qty, 0)) AS GenSomo ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_IPCH ";
                    SQL = SQL + ComNum.VBLF + "     WHERE InDate >= TO_DATE('" + strFDate3 + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND InDate <= TO_DATE('" + strTDate3 + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND JepCode = A.JEPCODE ";
                    SQL = SQL + ComNum.VBLF + ") AS GENSOMO";
                    #endregion

                    #region 소모량
                    SQL = SQL + ComNum.VBLF + ",    (";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(IpchGbn, '6', Qty, '7', Qty, 0)) AS DaySomo ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_IPCH ";
                    SQL = SQL + ComNum.VBLF + "     WHERE InDate >= TO_DATE('" + strFDate2 + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND InDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND JepCode = A.JEPCODE ";
                    SQL = SQL + ComNum.VBLF + ") AS DAYSOMO";
                    #endregion

                    #region 조회 소모량(외래, 입원)
                    SQL = SQL + ComNum.VBLF + ",    (";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(IpchGbn, '6', Qty, 0)) AS OSomo ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_IPCH ";
                    SQL = SQL + ComNum.VBLF + "     WHERE InDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND InDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND JepCode = A.JEPCODE ";
                    SQL = SQL + ComNum.VBLF + "         AND IpchGbn IN ('6', '7') ";
                    SQL = SQL + ComNum.VBLF + ") AS OSOMO";

                    SQL = SQL + ComNum.VBLF + ",    (";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(IpchGbn, '7', Qty, 0)) AS ISomo ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_IPCH ";
                    SQL = SQL + ComNum.VBLF + "     WHERE InDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND InDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND JepCode = A.JEPCODE ";
                    SQL = SQL + ComNum.VBLF + "         AND IpchGbn IN ('6', '7') ";
                    SQL = SQL + ComNum.VBLF + ") AS ISOMO";
                    #endregion


                    #region  현 재고임
                    SQL = SQL + ComNum.VBLF + ",    (";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     (IWOLQTY1 + IPGOQTY - CHULQTY) AS NEWJEGO";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "DRUG_SUBUL ";
                    SQL = SQL + ComNum.VBLF + " WHERE YYMM    = '" + strYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND JEPCODE = A.JEPCODE ";
                    SQL = SQL + ComNum.VBLF + "   AND ROWNUM  = 1 ";
                    SQL = SQL + ComNum.VBLF + ") AS NEWJEGO";
                    #endregion

                    #region 청구수량?
                    SQL = SQL + ComNum.VBLF + ",    (";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     QTY";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "DRUG_CHENGU ";
                    SQL = SQL + ComNum.VBLF + " WHERE BDATE = TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND JEPCODE = A.JEPCODE ";
                    SQL = SQL + ComNum.VBLF + "   AND ROWNUM  = 1 ";
                    SQL = SQL + ComNum.VBLF + ") AS CHENGU_QTY";
                    #endregion

                    #region 입고수량?
                    SQL = SQL + ComNum.VBLF + ",    (";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     QTY AS IPQTY";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "DRUG_IPCH ";
                    SQL = SQL + ComNum.VBLF + " WHERE INDATE = TO_DATE('" + Convert.ToDateTime(strTDate).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND JEPCODE = A.JEPCODE ";
                    SQL = SQL + ComNum.VBLF + "   AND ROWNUM  = 1 ";
                    SQL = SQL + ComNum.VBLF + ") AS IPCH_QTY";
                    #endregion

                    #region 창고수량?
                    SQL = SQL + ComNum.VBLF + ",    (";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     (IWOLQTY1 + IPGOQTY + CHAQTY - CHULQTY) AS ChangJEGO";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "DRUG_SUBUL ";
                    SQL = SQL + ComNum.VBLF + " WHERE YYMM = '" + strTDate.Replace("-", "").Left(6) + "'";
                    SQL = SQL + ComNum.VBLF + "   AND JEPCODE = A.JEPCODE ";
                    SQL = SQL + ComNum.VBLF + "   AND BuseCode = '00' ";
                    SQL = SQL + ComNum.VBLF + "   AND ROWNUM  = 1 ";
                    SQL = SQL + ComNum.VBLF + ") AS CHANGJEGO";
                    #endregion


                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_JEP A, " + ComNum.DB_ERP + "DRUG_CHENGU B  ";
                    SQL = SQL + ComNum.VBLF + "     WHERE (A.CHENGGU IS NOT NULL OR CHENGGU <> '') ";
                    SQL = SQL + ComNum.VBLF + "         AND (A.CHENGBUN IS NOT NULL OR CHENGBUN <> '') ";
                    SQL = SQL + ComNum.VBLF + "         AND (A.DELDATE IS NULL OR A.DELDATE = '') ";
                    SQL = SQL + ComNum.VBLF + "         AND A.CHENGGU NOT IN ('00') ";

                    if (txtJepCode.Text.Trim() != "")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.JEPCODE = '" + txtJepCode.Text + "'";
                    }
                    
                    SQL = SQL + ComNum.VBLF + "         AND A.JEPCODE = B.JEPCODE(+) ";
                    SQL = SQL + ComNum.VBLF + "         AND B.GELCODE(+) = '" + strGelCode + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.JEPCODE IN ('STRIBIL','INTEL100','PREZ','PREZ600','NOV100','REYAT200','REYAT150','STOCRIN','ISENT','KALET','COMBIVIR','KIVEXA','TRUVA','TRIMEQ','VALCY450')";
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.JGELCODE, A.GUIPDRUG, A.JEPNAME, A.SUB, A.JEPCODE, A.COVQTY, A.CHENGGU, A.CHENGBUN, A.JEPNAMEK, A.JEPNAME, A.JEPCODE";
                }
                else if (chk2.Checked == true)
                {
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.GUIPDRUG, A.JGELCODE, A.SUB, A.JEPCODE, A.COVQTY COVUNIT, A.JEPNAME, A.CHENGGU, A.CHENGBUN, A.JEPNAMEK, A.JEPNAME, A.JEPCODE   ";

                    #region //이름 
                    SQL = SQL + ComNum.VBLF + ",    (";
                    SQL = SQL + ComNum.VBLF + "SELECT NAME";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "AIS_LTD ";
                    SQL = SQL + ComNum.VBLF + " WHERE LTDCODE = A.GUIPDRUG";
                    SQL = SQL + ComNum.VBLF + " ) AS GUIP_NM";

                    SQL = SQL + ComNum.VBLF + ",    (";
                    SQL = SQL + ComNum.VBLF + "SELECT NAME";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "AIS_LTD ";
                    SQL = SQL + ComNum.VBLF + " WHERE LTDCODE = A.JGELCODE";
                    SQL = SQL + ComNum.VBLF + " ) AS GELNAME";


                    SQL = SQL + ComNum.VBLF + ",    (";
                    SQL = SQL + ComNum.VBLF + "SELECT BUNNAME";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "DRUG_BUN ";
                    SQL = SQL + ComNum.VBLF + " WHERE GBBUN  = '2'";
                    SQL = SQL + ComNum.VBLF + "   AND BUN    = A.SUB";
                    SQL = SQL + ComNum.VBLF + "   AND ROWNUM = 1";
                    SQL = SQL + ComNum.VBLF + " ) AS BUNNM";
                    #endregion

                    #region //전월 소모량(출고)
                    SQL = SQL + ComNum.VBLF + ",    (";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(IpchGbn, '6', Qty, '7', Qty, 0)) AS GenSomo ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_IPCH ";
                    SQL = SQL + ComNum.VBLF + "     WHERE InDate >= TO_DATE('" + strFDate3 + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND InDate <= TO_DATE('" + strTDate3 + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND JepCode = A.JEPCODE ";
                    SQL = SQL + ComNum.VBLF + ") AS GENSOMO";
                    #endregion

                    #region 소모량
                    SQL = SQL + ComNum.VBLF + ",    (";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(IpchGbn, '6', Qty, '7', Qty, 0)) AS DaySomo ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_IPCH ";
                    SQL = SQL + ComNum.VBLF + "     WHERE InDate >= TO_DATE('" + strFDate2 + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND InDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND JepCode = A.JEPCODE ";
                    SQL = SQL + ComNum.VBLF + ") AS DAYSOMO";
                    #endregion

                    #region 조회 소모량(외래, 입원)
                    SQL = SQL + ComNum.VBLF + ",    (";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(IpchGbn, '6', Qty, 0)) AS OSomo ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_IPCH ";
                    SQL = SQL + ComNum.VBLF + "     WHERE InDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND InDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND JepCode = A.JEPCODE ";
                    SQL = SQL + ComNum.VBLF + "         AND IpchGbn IN ('6', '7') ";
                    SQL = SQL + ComNum.VBLF + ") AS OSOMO";

                    SQL = SQL + ComNum.VBLF + ",    (";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(IpchGbn, '7', Qty, 0)) AS ISomo ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_IPCH ";
                    SQL = SQL + ComNum.VBLF + "     WHERE InDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND InDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND JepCode = '" + strJep + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND IpchGbn IN ('6', '7') ";
                    SQL = SQL + ComNum.VBLF + ") AS ISOMO";
                    #endregion


                    #region  현 재고임
                    SQL = SQL + ComNum.VBLF + ",    (";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     (IWOLQTY1 + IPGOQTY - CHULQTY) AS NEWJEGO";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "DRUG_SUBUL ";
                    SQL = SQL + ComNum.VBLF + " WHERE YYMM  = '" + strYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND JEPCODE = A.JEPCODE ";
                    SQL = SQL + ComNum.VBLF + "   AND ROWNUM  = 1 ";
                    SQL = SQL + ComNum.VBLF + ") AS NEWJEGO";
                    #endregion

                    #region 청구수량?
                    SQL = SQL + ComNum.VBLF + ",    (";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     QTY";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "DRUG_CHENGU ";
                    SQL = SQL + ComNum.VBLF + " WHERE BDATE = TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND JEPCODE = A.JEPCODE ";
                    SQL = SQL + ComNum.VBLF + "   AND ROWNUM  = 1 ";
                    SQL = SQL + ComNum.VBLF + ") AS CHENGU_QTY";
                    #endregion

                    #region 입고수량?
                    SQL = SQL + ComNum.VBLF + ",    (";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     QTY AS IPQTY";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "DRUG_IPCH ";
                    SQL = SQL + ComNum.VBLF + " WHERE INDATE = TO_DATE('" + Convert.ToDateTime(strTDate).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND JEPCODE = A.JEPCODE ";
                    SQL = SQL + ComNum.VBLF + "   AND ROWNUM  = 1 ";
                    SQL = SQL + ComNum.VBLF + ") AS IPCH_QTY";
                    #endregion

                    #region 창고수량?
                    SQL = SQL + ComNum.VBLF + ",    (";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     (IWOLQTY1 + IPGOQTY + CHAQTY - CHULQTY) AS ChangJEGO";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "DRUG_SUBUL ";
                    SQL = SQL + ComNum.VBLF + " WHERE YYMM = '" + strTDate.Replace("-", "").Left(6) + "'";
                    SQL = SQL + ComNum.VBLF + "   AND JEPCODE = A.JEPCODE ";
                    SQL = SQL + ComNum.VBLF + "   AND BuseCode = '00' ";
                    SQL = SQL + ComNum.VBLF + "   AND ROWNUM  = 1 ";
                    SQL = SQL + ComNum.VBLF + ") AS CHANGJEGO";
                    #endregion

                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_JEP A, " + ComNum.DB_ERP + "DRUG_CHENGU B, " + ComNum.DB_ERP + "DRUG_LAJEP C  ";
                    SQL = SQL + ComNum.VBLF + "     WHERE (A.CHENGGU IS NOT NULL OR CHENGGU <> '') ";
                    SQL = SQL + ComNum.VBLF + "         AND (A.CHENGBUN IS NOT NULL OR CHENGBUN <> '') ";
                    SQL = SQL + ComNum.VBLF + "         AND (A.DELDATE IS NULL OR A.DELDATE = '') ";
                    SQL = SQL + ComNum.VBLF + "         AND A.CHENGGU NOT IN ('00') ";

                    if (txtJepCode.Text.Trim() != "")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.JEPCODE = '" + txtJepCode.Text + "'";
                    }

                    SQL = SQL + ComNum.VBLF + "         AND A.JEPCODE = B.JEPCODE(+) ";
                    SQL = SQL + ComNum.VBLF + "         AND A.JEPCODE = C.JEPCODE";
                    SQL = SQL + ComNum.VBLF + "         AND (WARDCODE NOT LIKE '%ER%' OR WARDCODE IS NULL)";
                    SQL = SQL + ComNum.VBLF + "         AND B.GELCODE(+) = '" + strGelCode + "' ";

                    if (strGu != "00")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.CHENGGU = '" + strGu + "' ";
                    }

                    if (strBun != "00")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.CHENGBUN = '" + strBun + "' ";
                    }

                    SQL = SQL + ComNum.VBLF + "GROUP BY A.JGELCODE, A.GUIPDRUG, A.JEPNAME, A.SUB, A.JEPCODE, A.COVQTY, A.CHENGGU, A.CHENGBUN, A.JEPNAMEK, A.JEPNAME, A.JEPCODE";
                }
                else
                {
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.GUIPDRUG, A.JGELCODE, A.SUB, A.JEPCODE, A.COVQTY COVUNIT, A.JEPNAME, A.CHENGGU, A.CHENGBUN, A.JEPNAMEK, A.JEPNAME, A.JEPCODE   ";

                    #region //이름 
                    SQL = SQL + ComNum.VBLF + ",    (";
                    SQL = SQL + ComNum.VBLF + "SELECT NAME";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "AIS_LTD ";
                    SQL = SQL + ComNum.VBLF + " WHERE LTDCODE = A.GUIPDRUG";
                    SQL = SQL + ComNum.VBLF + " ) AS GUIP_NM";

                    SQL = SQL + ComNum.VBLF + ",    (";
                    SQL = SQL + ComNum.VBLF + "SELECT NAME";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "AIS_LTD ";
                    SQL = SQL + ComNum.VBLF + " WHERE LTDCODE = A.JGELCODE";
                    SQL = SQL + ComNum.VBLF + " ) AS GELNAME";

                    SQL = SQL + ComNum.VBLF + ",    (";
                    SQL = SQL + ComNum.VBLF + "SELECT BUNNAME";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "DRUG_BUN ";
                    SQL = SQL + ComNum.VBLF + " WHERE GBBUN  = '2'";
                    SQL = SQL + ComNum.VBLF + "   AND BUN    = A.SUB";
                    SQL = SQL + ComNum.VBLF + "   AND ROWNUM = 1";
                    SQL = SQL + ComNum.VBLF + " ) AS BUNNM";
                    #endregion

                    #region //전월 소모량(출고)
                    SQL = SQL + ComNum.VBLF + ",    (";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(IpchGbn, '6', Qty, '7', Qty, 0)) AS GenSomo ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_IPCH ";
                    SQL = SQL + ComNum.VBLF + "     WHERE InDate >= TO_DATE('" + strFDate3 + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND InDate <= TO_DATE('" + strTDate3 + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND JepCode = A.JEPCODE ";
                    SQL = SQL + ComNum.VBLF + ") AS GENSOMO";
                    #endregion

                    #region 소모량
                    SQL = SQL + ComNum.VBLF + ",    (";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(IpchGbn, '6', Qty, '7', Qty, 0)) AS DaySomo ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_IPCH ";
                    SQL = SQL + ComNum.VBLF + "     WHERE InDate >= TO_DATE('" + strFDate2 + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND InDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND JepCode = A.JEPCODE ";
                    SQL = SQL + ComNum.VBLF + ") AS DAYSOMO";
                    #endregion


                    #region 조회 소모량(외래, 입원)
                    SQL = SQL + ComNum.VBLF + ",    (";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(IpchGbn, '6', Qty, 0)) AS OSomo ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_IPCH ";
                    SQL = SQL + ComNum.VBLF + "     WHERE InDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND InDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND JepCode = A.JEPCODE ";
                    SQL = SQL + ComNum.VBLF + "         AND IpchGbn IN ('6', '7') ";
                    SQL = SQL + ComNum.VBLF + ") AS OSOMO";

                    SQL = SQL + ComNum.VBLF + ",    (";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(IpchGbn, '7', Qty, 0)) AS ISomo ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_IPCH ";
                    SQL = SQL + ComNum.VBLF + "     WHERE InDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND InDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND JepCode = A.JEPCODE ";
                    SQL = SQL + ComNum.VBLF + "         AND IpchGbn IN ('6', '7') ";
                    SQL = SQL + ComNum.VBLF + ") AS ISOMO";
                    #endregion


                    #region  현 재고임
                    SQL = SQL + ComNum.VBLF + ",    (";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     (IWOLQTY1 + IPGOQTY - CHULQTY) AS NEWJEGO";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "DRUG_SUBUL ";
                    SQL = SQL + ComNum.VBLF + " WHERE YYMM  = '" + strYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND JEPCODE = A.JEPCODE ";
                    SQL = SQL + ComNum.VBLF + "   AND ROWNUM  = 1 ";
                    SQL = SQL + ComNum.VBLF + ") AS NEWJEGO";
                    #endregion

                    #region 청구수량?
                    SQL = SQL + ComNum.VBLF + ",    (";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     QTY";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "DRUG_CHENGU ";
                    SQL = SQL + ComNum.VBLF + " WHERE BDATE = TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND JEPCODE = A.JEPCODE ";
                    SQL = SQL + ComNum.VBLF + "   AND ROWNUM  = 1 ";
                    SQL = SQL + ComNum.VBLF + ") AS CHENGU_QTY";
                    #endregion

                    #region 입고수량?
                    SQL = SQL + ComNum.VBLF + ",    (";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     QTY AS IPQTY";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "DRUG_IPCH ";
                    SQL = SQL + ComNum.VBLF + " WHERE INDATE = TO_DATE('" + Convert.ToDateTime(strTDate).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND JEPCODE = A.JEPCODE ";
                    SQL = SQL + ComNum.VBLF + "   AND ROWNUM  = 1 ";
                    SQL = SQL + ComNum.VBLF + ") AS IPCH_QTY";
                    #endregion

                    #region 창고수량?
                    SQL = SQL + ComNum.VBLF + ",    (";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     (IWOLQTY1 + IPGOQTY + CHAQTY - CHULQTY) AS ChangJEGO";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "DRUG_SUBUL ";
                    SQL = SQL + ComNum.VBLF + " WHERE YYMM = '" + strTDate.Replace("-", "").Left(6) + "'";
                    SQL = SQL + ComNum.VBLF + "   AND JEPCODE = A.JEPCODE ";
                    SQL = SQL + ComNum.VBLF + "   AND BuseCode = '00' ";
                    SQL = SQL + ComNum.VBLF + "   AND ROWNUM  = 1 ";
                    SQL = SQL + ComNum.VBLF + ") AS CHANGJEGO";
                    #endregion

                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_JEP A, " + ComNum.DB_ERP + "DRUG_CHENGU B  ";
                    SQL = SQL + ComNum.VBLF + "     WHERE (A.CHENGGU IS NOT NULL OR CHENGGU <> '') ";
                    SQL = SQL + ComNum.VBLF + "         AND (A.CHENGBUN IS NOT NULL OR CHENGBUN <> '') ";
                    SQL = SQL + ComNum.VBLF + "         AND (A.DELDATE IS NULL OR A.DELDATE = '') ";
                    SQL = SQL + ComNum.VBLF + "         AND A.CHENGGU NOT IN ('00') ";

                    if (txtJepCode.Text.Trim() != "")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.JEPCODE = '" + txtJepCode.Text + "'";
                    }

                    SQL = SQL + ComNum.VBLF + "         AND A.JEPCODE = B.JEPCODE(+) ";
                    SQL = SQL + ComNum.VBLF + "         AND B.GELCODE(+) = '" + strGelCode + "' ";

                    if (strGu != "00")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.CHENGGU = '" + strGu + "' ";
                    }

                    if (strBun != "00")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.CHENGBUN = '" + strBun + "' ";
                    }

                    SQL = SQL + ComNum.VBLF + "GROUP BY A.JGELCODE, A.GUIPDRUG, A.JEPNAME, A.SUB, A.JEPCODE, A.COVQTY, A.CHENGGU, A.CHENGBUN, A.JEPNAMEK, A.JEPNAME, A.JEPCODE";
                }

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strJep = dt.Rows[i]["JEPCODE"].ToString().Trim();
                        strNewGu = dt.Rows[i]["CHENGGU"].ToString().Trim();
                        strNewBun = dt.Rows[i]["CHENGBUN"].ToString().Trim();

                        dblGENSOMO = VB.Val(dt.Rows[i]["GENSOMO"].ToString().Trim());
                        dblDAYSOMO = VB.Val(dt.Rows[i]["DAYSOMO"].ToString().Trim());

                        dblOSOMO = VB.Val(dt.Rows[i]["OSOMO"].ToString().Trim());
                        dblISOMO = VB.Val(dt.Rows[i]["ISOMO"].ToString().Trim());
                        dblNEWJEGO = VB.Val(dt.Rows[i]["NEWJEGO"].ToString().Trim());


                        //ssView_Sheet1.Cells[i, 0].Text = READ_LTD_NAME(clsDB.DbCon, dt.Rows[i]["GUIPDRUG"].ToString().Trim());
                        //ssView_Sheet1.Cells[i, 1].Text = READ_LTD_NAME(clsDB.DbCon, dt.Rows[i]["JGELCODE"].ToString().Trim());
                        //ssView_Sheet1.Cells[i, 2].Text = Read_BunName(clsDB.DbCon, dt.Rows[i]["SUB"].ToString().Trim());

                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["GUIP_NM"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["GELNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["BUNNM"].ToString().Trim();

                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["JEPCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["COVUNIT"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["JEPNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dblGENSOMO.ToString("###,##0");
                        ssView_Sheet1.Cells[i, 7].Text = dblDAYSOMO.ToString("###,##0");
                        ssView_Sheet1.Cells[i, 8].Text = dblOSOMO.ToString("###,##0");
                        ssView_Sheet1.Cells[i, 9].Text = dblISOMO.ToString("###,##0");
                        ssView_Sheet1.Cells[i, 10].Text = (dblOSOMO + dblISOMO).ToString("###,##0");
                        ssView_Sheet1.Cells[i, 11].Text = dblNEWJEGO.ToString("###,##0");


                        ssView_Sheet1.Cells[i, 12].Text = dt.Rows[i]["CHANGJEGO"].To<long>(0).ToString("###,##0");

                        ssView_Sheet1.Cells[i, 13].Text = dt.Rows[i]["CHENGU_QTY"].To<long>(0).ToString("###,###");
                        ssView_Sheet1.Cells[i, 14].Text = dt.Rows[i]["IPCH_QTY"].To<long>(0).ToString("###,###");

                        ssView_Sheet1.Cells[i, 15].Text = strNewGu;
                        ssView_Sheet1.Cells[i, 16].Text = strNewBun;
                        ssView_Sheet1.Cells[i, 18].Text = (dblNEWJEGO + dt.Rows[i]["CHANGJEGO"].To<long>(0)).ToString("###,##0");


                        dblChsomo = dblGENSOMO;
                        dblChJogo = dblNEWJEGO;

                        if ((dblChsomo * 0.3) >= dblChJogo && dblChsomo > 0)
                        {
                            ssView_Sheet1.Cells[i, 0, i, ssView_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(255, 200, 255);
                        }

                        dblGENSOMO = 0;
                        dblDAYSOMO = 0;
                        dblOSOMO = 0;
                        dblISOMO = 0;
                        dblNEWJEGO = 0;
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssList_Sheet1.Cells[e.Row, e.Column].Text.Trim() == "") { return; }

            txtJepCode.Text = ssList_Sheet1.Cells[e.Row, e.Column].Text.Trim();

            GetData();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            dtpFDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(-1);
            dtpTDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(-1);

            ssView_Sheet1.RowCount = 0;
            ssList_Sheet1.RowCount = 0;
            txtJepCode.Text = "";

            cboBun.SelectedIndex = 0;

            GetJego();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            string strFont1 = "";
            string strHead1 = "";
            string strFont2 = "";
            string strHead2 = "";

            strFont1 = "/fn\"맑은 고딕\" /fz\"20\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"맑은 고딕\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";

            strHead1 = "/c/f1" + "의약품 재고 현황" + "/f1/n";
            strHead2 = "/l/f2" + "출력일자 : " + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-") + "/f2/n";

            ssView_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssView_Sheet1.PrintInfo.ZoomFactor = 0.85f;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Top = 20;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssView_Sheet1.PrintInfo.Margin.Header = 10;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView_Sheet1.PrintInfo.UseSmartPrint = false;
            ssView_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssView_Sheet1.PrintInfo.Preview = false;
            ssView.PrintSheet(0);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
