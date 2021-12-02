using ComBase; //기본 클래스
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmComSupDrugList.cs
    /// Description     : 비상마약관리대장
    /// Author          : 이정현
    /// Create Date     : 2017-07-19
    /// Update History  : 약제팀, 중환자실, 호스피스병동, 혈관조영실, 내시경실, 응급의료센터, 주사실, 종검 통합
    /// <history> 
    /// 각 부서별 비상마약관리대장 통합
    /// </history>
    /// <seealso>
    /// PSMH\drug\drmayak\FrmDrugList.frm
    /// PSMH\drug\drmayak\FrmDrugList4H.frm
    /// PSMH\drug\drmayak\FrmDrugListAG.frm
    /// PSMH\drug\drmayak\FrmDrugListENDO.frm
    /// PSMH\drug\drmayak\FrmDrugListER.frm
    /// PSMH\drug\drmayak\FrmDrugListIU.frm
    /// PSMH\drug\drmayak\FrmDrugListJusa.frm
    /// PSMH\drug\drmayak\FrmDrugListTO.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\drmayak\ojumst.vbp
    /// </vbp>
    /// </summary>
    public partial class frmComSupDrugList : Form
    {
        private frmComSupDrugListMagam frmComSupDrugListMagamEvent = null;

        private string GstrWardCode = "";
        private int GintUnit = 0;

        private string GstrLabelPrint1 = "";
        private string GstrLabelPrint2 = "";
        private string GstrLabelPrint3 = "";
        private string GstrLabelPrint4 = "";
        private string GstrLabelPrint5 = "";

        private FarPoint.Win.Spread.FpSpread ssViewPrint = new FarPoint.Win.Spread.FpSpread();
        private FarPoint.Win.Spread.SheetView ssViewPrint_Sheet1 = new FarPoint.Win.Spread.SheetView();

        public frmComSupDrugList()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 부서/병동 코드로 생성자 입력
        /// 32/33/35(중환자실), 4H(호스피스), AG(혈관조영실), AN(약제팀), EN(내시경실), ER(응급의료센터), JS(주사실), TO(종합검진센터)
        /// </summary>
        /// <param name="strWardCode"></param>
        public frmComSupDrugList(string strWardCode)
        {
            InitializeComponent();

            GstrWardCode = strWardCode;
        }

        private void frmComSupDrugList_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            dtpDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            cboWardCode.Text = "";
            cboWardCode.Items.Clear();
            cboWardCode.Items.Add("마취과" + VB.Space(20) + "AN");
            cboWardCode.Items.Add("응급실" + VB.Space(20) + "ER");
            cboWardCode.Items.Add("주사실" + VB.Space(20) + "JS");
            cboWardCode.Items.Add("32병동(중환자실)" + VB.Space(20) + "32");
            cboWardCode.Items.Add("33병동(SICU)" + VB.Space(20) + "33");
            cboWardCode.Items.Add("35병동(MICU)" + VB.Space(20) + "35");
            cboWardCode.Items.Add("4H병동(호스피스병동)" + VB.Space(20) + "4H");
            cboWardCode.Items.Add("혈관조영실" + VB.Space(20) + "AG");
            cboWardCode.Items.Add("종합건진" + VB.Space(20) + "TO");
            cboWardCode.Items.Add("내시경실" + VB.Space(20) + "EN");

            if (GstrWardCode == "")
            {
                GstrWardCode = SetWardCode();
            }
            
            if (GstrWardCode.Trim() != "")
            {
                switch (GstrWardCode)
                {
                    case "32":  //중환자실(ICU) -- 과거
                    case "33":  //응급 중환자실(EICU)
                    case "35":  //중환자실(GICU)
                    case "4H":  //호스피스병동
                    case "AG":  //혈관조영실
                    case "AN":  //약제팀
                    case "EN":  //내시경실
                    case "ER":  //응급의료센터
                    case "JS":  //주사실
                    case "TO":  //종합검진
                    case "MST":
                        break;
                    default:
                        ComFunc.MsgBox("지정된 부서코드가 없습니다.");
                        this.Close();
                        break;
                }
            }
            else
            {
                ComFunc.MsgBox("지정된 부서코드가 없습니다.");
                this.Close();
            }

            if (clsType.User.Grade == "EDPS" || GstrWardCode == "MST")
            {
                label1.Visible = true;
                cboWardCode.Visible = true;
                GstrWardCode = "AN";
            }

            ssView_Sheet1.Columns[3].Label = GstrWardCode.Equals("EN") || GstrWardCode.Equals("TO") ? "반환량" : "입고량"; //19-05-30 의뢰서 관련 작업

            SetForm();
            FormClear();

            //2019-06-10 전산의뢰서 2019-664
            if (GstrWardCode == "AN")
            {
                ssMagam.Visible = true;
                //READ_MAGAM_YN();
                READ_MAGAM_NEW_YN();
            }
            else
            {
                ssMagam.Visible = false;
            }
        }

        private string SetWardCode()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            string strCurDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            string strIP = clsCompuInfo.gstrCOMIP;

            try
            {
                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "     P.IPADDRESS, P.VALUEV";
                SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_PMPA + "BAS_BASCD B ";
                SQL = SQL + ComNum.VBLF + "     LEFT OUTER JOIN  " + ComNum.DB_PMPA + "BAS_PCCONFIG P ";
                SQL = SQL + ComNum.VBLF + "         ON B.GRPCD = P.GUBUN  ";
                SQL = SQL + ComNum.VBLF + "             AND B.BASCD = P.CODE  ";
                SQL = SQL + ComNum.VBLF + "             AND P.IPADDRESS =  '" + strIP + "'";
                SQL = SQL + ComNum.VBLF + "     WHERE B.GRPCDB = '프로그램PC세팅'  ";
                SQL = SQL + ComNum.VBLF + "         AND B.GRPCD = '비상마약'  ";
                SQL = SQL + ComNum.VBLF + "         AND B.APLFRDATE <= '" + strCurDate + "'";
                SQL = SQL + ComNum.VBLF + "         AND B.APLENDDATE >= '" + strCurDate + "'";
                SQL = SQL + ComNum.VBLF + "         AND B.USECLS = '1'";
                SQL = SQL + ComNum.VBLF + "         AND B.VFLAG1 = '1'";
                SQL = SQL + ComNum.VBLF + "ORDER BY B.DISPSEQ ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    Cursor.Current = Cursors.WaitCursor;
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.WaitCursor;
                    return rtnVal;
                }

                rtnVal = dt.Rows[0]["VALUEV"].ToString().Trim();

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장

                return rtnVal;
            }
        }

        private void SetForm()
        {
            switch(GstrWardCode)
            {
                case "AN":  //약제팀
                    lblMayakList.Text = "비상마약류 관리대장_마취과";
                    panChasu.Visible = true;
                    panBar.Visible = true;
                    panSet.Visible = false;
                    panBQty.Visible = true;
                    panBuild.Visible = true;
                    panJegoSet.Visible = true;
                    lblDQty.Text = "당일재고량 :";
                    //ssView_Sheet1.Columns[16].Visible = false; //19-05-13 주석처리                    
                    ssView_Sheet1.Columns[14].Label = "마취과" + ComNum.VBLF + "확인";
                    ssView_Sheet1.Columns[14].Width = 70;
                    rdoGB0.Checked = true;
                    break;
                case "4H":  //호스피스병동
                    lblMayakList.Text = "비상마약류 관리대장_호스피스";
                    panChasu.Visible = true;
                    panBar.Visible = false;
                    panSet.Visible = false;
                    panBQty.Visible = true;
                    panBuild.Visible = true;
                    panJegoSet.Visible = false;
                    if (clsPublic.GnJobSabun == 4349 || clsPublic.GstrJobGrade == "EDPS") { panJegoSet.Visible = true; }
                    lblDQty.Text = "당일재고량 :";
                    ssView_Sheet1.Columns[16].Visible = true;
                    ssView_Sheet1.Columns[14].Label = "병동" + ComNum.VBLF + "확인";
                    ssView_Sheet1.Columns[14].Width = 70;
                    rdoGB0.Checked = true;
                    break;
                case "AG":  //혈관조영실
                    lblMayakList.Text = "비상마약류 관리대장_혈관조영실";
                    panChasu.Visible = true;
                    panBar.Visible = false;
                    panSet.Visible = false;
                    panBQty.Visible = true;
                    panBuild.Visible = true;
                    panJegoSet.Visible = false;
                    if (clsPublic.GnJobSabun == 4349 || clsPublic.GstrJobGrade == "EDPS") { panJegoSet.Visible = true; }
                    lblDQty.Text = "당일재고량 :";
                    ssView_Sheet1.Columns[16].Visible = false;
                    ssView_Sheet1.Columns[14].Label = "혈관조영실" + ComNum.VBLF + "확인";
                    ssView_Sheet1.Columns[14].Width = 70;
                    rdoGB0.Checked = true;
                    break;
                case "EN":  //내시경실
                    lblMayakList.Text = "비상마약류 관리대장_내시경실";
                    panChasu.Visible = false;
                    panBar.Visible = false;
                    panSet.Visible = true;
                    panBQty.Visible = false;
                    panBuild.Visible = true;
                    panJegoSet.Visible = false;
                    lblDQty.Text = "당일비치량 :";
                    ssView_Sheet1.Columns[16].Visible = false;
                    ssView_Sheet1.Columns[14].Label = "내시경실" + ComNum.VBLF + "확인";
                    ssView_Sheet1.Columns[14].Width = 70;
                    rdoGB1.Checked = true;
                    break;
                case "ER":  //응급의료센터
                    lblMayakList.Text = "비상마약류 관리대장_응급의료센터";
                    panChasu.Visible = true;
                    panBar.Visible = false;
                    panSet.Visible = false;
                    panBQty.Visible = true;
                    panBuild.Visible = true;
                    panJegoSet.Visible = false;
                    if (clsPublic.GnJobSabun == 4349 || clsPublic.GstrJobGrade == "EDPS") { panJegoSet.Visible = true; }
                    lblDQty.Text = "당일재고량 :";
                    ssView_Sheet1.Columns[16].Visible = false;
                    ssView_Sheet1.Columns[14].Label = "응급센타" + ComNum.VBLF + "확인";
                    ssView_Sheet1.Columns[14].Width = 70;
                    rdoGB0.Checked = true;
                    break;
                case "32":  //중환자실(ICU) -- 과거
                case "33":  //응급 중환자실(EICU)
                case "35":  //중환자실(GICU)
                    lblMayakList.Text = "비상마약류 관리대장_중환자실";
                    panChasu.Visible = true;
                    panBar.Visible = false;
                    panSet.Visible = false;
                    panBQty.Visible = true;
                    panBuild.Visible = true;
                    panJegoSet.Visible = false;
                    if (clsPublic.GnJobSabun == 4349 || clsPublic.GstrJobGrade == "EDPS") { panJegoSet.Visible = true; }
                    lblDQty.Text = "당일재고량 :";
                    ssView_Sheet1.Columns[16].Visible = false;
                    ssView_Sheet1.Columns[14].Label = "병동" + ComNum.VBLF + "확인";
                    ssView_Sheet1.Columns[14].Width = 70;
                    rdoGB0.Checked = true;
                    break;
                case "JS":  //주사실
                    lblMayakList.Text = "비상마약류 관리대장_주사실";
                    panChasu.Visible = true;
                    panBar.Visible = false;
                    panSet.Visible = false;
                    panBQty.Visible = true;
                    panBuild.Visible = true;
                    panJegoSet.Visible = false;
                    if (clsPublic.GnJobSabun == 4349 || clsPublic.GstrJobGrade == "EDPS") { panJegoSet.Visible = true; }
                    lblDQty.Text = "당일재고량 :";
                    ssView_Sheet1.Columns[16].Visible = false;
                    ssView_Sheet1.Columns[14].Label = "주사실" + ComNum.VBLF + "확인";
                    ssView_Sheet1.Columns[14].Width = 70;
                    rdoGB0.Checked = true;
                    break;
                case "TO":  //종합검진
                    lblMayakList.Text = "비상마약류 관리대장_종합건진";
                    panChasu.Visible = false;
                    panBar.Visible = false;
                    panSet.Visible = true;
                    panBQty.Visible = true;
                    panBuild.Visible = false;
                    panJegoSet.Visible = false;
                    lblDQty.Text = "당일재고량 :";
                    ssView_Sheet1.Columns[16].Visible = false;
                    ssView_Sheet1.Columns[14].Label = "종합건진" + ComNum.VBLF + "확인";
                    ssView_Sheet1.Columns[14].Width = 70;
                    rdoGB1.Checked = true;
                    break;
            }

            if (ssView_Sheet1.Columns.Count > 16)
            {
                ssView_Sheet1.Columns[16].Visible = true; //19-05-13 수정
            }
        }

        private void FormClear()
        {
            txtBQty.Text = "";
            txtDCode.Text = "";
            txtDName.Text = "";
            txtDQty.Text = "";
            txtUnit.Text = "";

            ssList_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 0;

            cboChasu.Items.Clear();
            cboChasu.Items.Add("");
            cboChasu.Items.Add("1차수");
            cboChasu.Items.Add("2차수");
            cboChasu.Items.Add("3차수");
            cboChasu.Items.Add("4차수");
            cboChasu.Items.Add("5차수");
            cboChasu.Items.Add("6차수");
            cboChasu.Items.Add("7차수");
            
            SetChasu("");
        }

        private void SetChasu(string strMagam)
        {
            string SQL = "";
            DataTable dt = null;
            int i = 0;
            string SqlErr = "";

            string strSelChasu = "";

            if (cboChasu.Text != "")
            {
                strSelChasu = cboChasu.Text;
            }

            cboChasu.Items.Clear();

            try
            {
                //해당 일자에 차수 SET
                SQL = "";
                SQL = "SELECT NO1 FROM " + ComNum.DB_MED + "OCS_DRUG ";
                SQL = SQL + ComNum.VBLF + "  WHERE BUILDDATE =TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND GBN2 IN ('2')  ";
                SQL = SQL + ComNum.VBLF + "    AND WARDCODE ='" + GstrWardCode + "'  ";  //ICU
                SQL = SQL + ComNum.VBLF + "GROUP BY NO1 ";
                SQL = SQL + ComNum.VBLF + "ORDER BY NO1 DESC";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboChasu.Items.Add(dt.Rows[i]["NO1"].ToString().Trim() + "차수");
                    }

                    cboChasu.SelectedIndex = 0;

                    if (strMagam == "")
                    {
                        ComFunc.ComboFind(cboChasu, "", 0, strSelChasu);
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

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            SetChasu("");
        }

        private void btnJegoSet_Click(object sender, EventArgs e)
        {
            if (clsPublic.GstrJobGrade != "EDPS") { return; }

            string strDate = "";

            switch(GstrWardCode)
            {
                case "AN":
                    strDate = "2012-12-23";
                    break;
                case "4H":
                    strDate = "2014-06-09";
                    break;
                case "AG":
                    strDate = "2014-06-01";
                    break;
                case "ER":
                    strDate = "2014-02-17";
                    break;
                case "32":
                case "33":
                case "35":
                    strDate = "2014-06-09";
                    break;
                case "JS":
                    strDate = "2014-06-01";
                    break;
            }

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                switch(GstrWardCode)
                {
                    case "AN":
                        #region AN
                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                        SQL = SQL + ComNum.VBLF + "     (BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD'),";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '" + GstrWardCode + "',";
                        SQL = SQL + ComNum.VBLF + "         'N-PTD-HA',";
                        SQL = SQL + ComNum.VBLF + "         '4',";
                        SQL = SQL + ComNum.VBLF + "         '4',";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                        SQL = SQL + ComNum.VBLF + "     (BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD'),";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '" + GstrWardCode + "',";
                        SQL = SQL + ComNum.VBLF + "         'N-FT-HA',";
                        SQL = SQL + ComNum.VBLF + "         '30',";
                        SQL = SQL + ComNum.VBLF + "         '30',";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                        SQL = SQL + ComNum.VBLF + "     (BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD'),";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '" + GstrWardCode + "',";
                        SQL = SQL + ComNum.VBLF + "         'N-FT10',";
                        SQL = SQL + ComNum.VBLF + "         '10',";
                        SQL = SQL + ComNum.VBLF + "         '10',";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                        SQL = SQL + ComNum.VBLF + "     (BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD'),";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '" + GstrWardCode + "',";
                        SQL = SQL + ComNum.VBLF + "         'N-ULVA1',";
                        SQL = SQL + ComNum.VBLF + "         '5',";
                        SQL = SQL + ComNum.VBLF + "         '5',";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                        SQL = SQL + ComNum.VBLF + "     (BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD'),";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '" + GstrWardCode + "',";
                        SQL = SQL + ComNum.VBLF + "         'N-MP-HA',";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                        SQL = SQL + ComNum.VBLF + "     (BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD'),";
                        SQL = SQL + ComNum.VBLF + "         '2',";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '" + GstrWardCode + "',";
                        SQL = SQL + ComNum.VBLF + "         'DZP10',";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                        SQL = SQL + ComNum.VBLF + "     (BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD'),";
                        SQL = SQL + ComNum.VBLF + "         '2',";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '" + GstrWardCode + "',";
                        SQL = SQL + ComNum.VBLF + "         'A-BASCA',";
                        SQL = SQL + ComNum.VBLF + "         '20',";
                        SQL = SQL + ComNum.VBLF + "         '20',";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                        SQL = SQL + ComNum.VBLF + "     (BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD'),";
                        SQL = SQL + ComNum.VBLF + "         '2',";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '" + GstrWardCode + "',";
                        SQL = SQL + ComNum.VBLF + "         'A-KETA5',";
                        SQL = SQL + ComNum.VBLF + "         '5',";
                        SQL = SQL + ComNum.VBLF + "         '5',";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                        SQL = SQL + ComNum.VBLF + "     (BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD'),";
                        SQL = SQL + ComNum.VBLF + "         '2',";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '" + GstrWardCode + "',";
                        SQL = SQL + ComNum.VBLF + "         'A-PEN250',";
                        SQL = SQL + ComNum.VBLF + "         '7',";
                        SQL = SQL + ComNum.VBLF + "         '7',";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                        SQL = SQL + ComNum.VBLF + "     (BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD'),";
                        SQL = SQL + ComNum.VBLF + "         '2',";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '" + GstrWardCode + "',";
                        SQL = SQL + ComNum.VBLF + "         'A-POF50',";
                        SQL = SQL + ComNum.VBLF + "         '10',";
                        SQL = SQL + ComNum.VBLF + "         '10',";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                        SQL = SQL + ComNum.VBLF + "     (BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD'),";
                        SQL = SQL + ComNum.VBLF + "         '2',";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '" + GstrWardCode + "',";
                        SQL = SQL + ComNum.VBLF + "         'A-POL2',";
                        SQL = SQL + ComNum.VBLF + "         '20',";
                        SQL = SQL + ComNum.VBLF + "         '20',";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                        SQL = SQL + ComNum.VBLF + "     (BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD'),";
                        SQL = SQL + ComNum.VBLF + "         '2',";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '" + GstrWardCode + "',";
                        SQL = SQL + ComNum.VBLF + "         'A-POL12A',";
                        SQL = SQL + ComNum.VBLF + "         '10',";
                        SQL = SQL + ComNum.VBLF + "         '10',";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        #endregion
                        break;
                    case "4H":
                        #region 4H
                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                        SQL = SQL + ComNum.VBLF + "     ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD') ,";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '32',";
                        SQL = SQL + ComNum.VBLF + "         'N-PTD25',";
                        SQL = SQL + ComNum.VBLF + "         '2',";
                        SQL = SQL + ComNum.VBLF + "         '2',";
                        SQL = SQL + ComNum.VBLF + "         '1' ,";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     )";
                            
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                        SQL = SQL + ComNum.VBLF + "     ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD') ,";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '32',";
                        SQL = SQL + ComNum.VBLF + "         'N-MP-HA',";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     )";
                        
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                        SQL = SQL + ComNum.VBLF + "     ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD') ,";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '32',";
                        SQL = SQL + ComNum.VBLF + "         'N-FT-HA',";
                        SQL = SQL + ComNum.VBLF + "         '2',";
                        SQL = SQL + ComNum.VBLF + "         '2',";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                        SQL = SQL + ComNum.VBLF + "     ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD') ,";
                        SQL = SQL + ComNum.VBLF + "         '2',";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '32',";
                        SQL = SQL + ComNum.VBLF + "         'A-BASCA',";
                        SQL = SQL + ComNum.VBLF + "         '13',";
                        SQL = SQL + ComNum.VBLF + "         '13',";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG  ";
                        SQL = SQL + ComNum.VBLF + "     ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD') ,";
                        SQL = SQL + ComNum.VBLF + "         '2',";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '32',";
                        SQL = SQL + ComNum.VBLF + "         'A-PEN250',";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                        SQL = SQL + ComNum.VBLF + "     ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD') ,";
                        SQL = SQL + ComNum.VBLF + "         '2',";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '32',";
                        SQL = SQL + ComNum.VBLF + "         'DZP10',";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                        SQL = SQL + ComNum.VBLF + "     ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD') ,";
                        SQL = SQL + ComNum.VBLF + "         '2',";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '32', ";
                        SQL = SQL + ComNum.VBLF + "         'A-POL12A', ";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         '1', ";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                        SQL = SQL + ComNum.VBLF + "     ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD') ,";
                        SQL = SQL + ComNum.VBLF + "         '2', ";
                        SQL = SQL + ComNum.VBLF + "         '3', ";
                        SQL = SQL + ComNum.VBLF + "         '32', ";
                        SQL = SQL + ComNum.VBLF + "         'LZPA', ";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '1', ";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                        SQL = SQL + ComNum.VBLF + "     ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     ( ";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD') , ";
                        SQL = SQL + ComNum.VBLF + "         '2', ";
                        SQL = SQL + ComNum.VBLF + "         '3', ";
                        SQL = SQL + ComNum.VBLF + "         '32', ";
                        SQL = SQL + ComNum.VBLF + "         'A-POF50', ";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '1', ";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        #endregion
                        break;
                    case "AG":
                        #region AG
                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                        SQL = SQL + ComNum.VBLF + "     ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD') ,";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '" + GstrWardCode + "',";
                        SQL = SQL + ComNum.VBLF + "         'N-PTD-HA',";
                        SQL = SQL + ComNum.VBLF + "         '5',";
                        SQL = SQL + ComNum.VBLF + "         '5',";
                        SQL = SQL + ComNum.VBLF + "         '1' ,";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                        SQL = SQL + ComNum.VBLF + "     ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD') ,";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '" + GstrWardCode + "',";
                        SQL = SQL + ComNum.VBLF + "         'N-PTD25',";
                        SQL = SQL + ComNum.VBLF + "         '5',";
                        SQL = SQL + ComNum.VBLF + "         '5',";
                        SQL = SQL + ComNum.VBLF + "         '1' ,";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                        SQL = SQL + ComNum.VBLF + "     ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD') ,";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '" + GstrWardCode + "',";
                        SQL = SQL + ComNum.VBLF + "         'N-MP-HA',";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                        SQL = SQL + ComNum.VBLF + "     ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD') ,";
                        SQL = SQL + ComNum.VBLF + "         '2',";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '" + GstrWardCode + "',";
                        SQL = SQL + ComNum.VBLF + "         'DZP10',";
                        SQL = SQL + ComNum.VBLF + "         '5',";
                        SQL = SQL + ComNum.VBLF + "         '5',";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG";
                        SQL = SQL + ComNum.VBLF + "     ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD') ,";
                        SQL = SQL + ComNum.VBLF + "         '2',";
                        SQL = SQL + ComNum.VBLF + "         '3',";
                        SQL = SQL + ComNum.VBLF + "         '" + GstrWardCode + "',";
                        SQL = SQL + ComNum.VBLF + "         'NALPA',";
                        SQL = SQL + ComNum.VBLF + "         '2',";
                        SQL = SQL + ComNum.VBLF + "         '2',";
                        SQL = SQL + ComNum.VBLF + "         '1',";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        #endregion
                        break;
                    case "ER":
                        #region ER
                        SQL = "";
                        SQL = " INSERT INTO KOSMOS_OCS.OCS_DRUG  ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + "  VALUES ( TO_DATE('" + strDate + "','YYYY-MM-DD') , '1', '3', 'ER', 'N-PTD-HA', '5','5','1' ,TO_DATE('" + strDate + "','YYYY-MM-DD'))";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = " INSERT INTO KOSMOS_OCS.OCS_DRUG  ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + "  VALUES ( TO_DATE('" + strDate + "','YYYY-MM-DD') , '1', '3', 'ER', 'N-PTD25', '10','10','1' ,TO_DATE('" + strDate + "','YYYY-MM-DD'))";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = " INSERT INTO KOSMOS_OCS.OCS_DRUG  ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + "  VALUES ( TO_DATE('" + strDate + "','YYYY-MM-DD') , '1', '3', 'ER', 'N-MP-HA', '2','2','1', TO_DATE('" + strDate + "','YYYY-MM-DD'))";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = " INSERT INTO KOSMOS_OCS.OCS_DRUG  ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + "  VALUES ( TO_DATE('" + strDate + "','YYYY-MM-DD') , '1', '3', 'ER', 'N-MP5', '3','3','1', TO_DATE('" + strDate + "','YYYY-MM-DD'))";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = " INSERT INTO KOSMOS_OCS.OCS_DRUG  ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + "  VALUES ( TO_DATE('" + strDate + "','YYYY-MM-DD') , '1', '3', 'ER', 'N-FT-HA', '3','3','1', TO_DATE('" + strDate + "','YYYY-MM-DD'))";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = " INSERT INTO KOSMOS_OCS.OCS_DRUG  ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + "  VALUES ( TO_DATE('" + strDate + "','YYYY-MM-DD') , '2', '3', 'ER', 'A-BASCA', '3','3','1', TO_DATE('" + strDate + "','YYYY-MM-DD'))";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = " INSERT INTO KOSMOS_OCS.OCS_DRUG  ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + "  VALUES ( TO_DATE('" + strDate + "','YYYY-MM-DD') , '2', '3', 'ER', 'A-PEN250', '3','3','1', TO_DATE('" + strDate + "','YYYY-MM-DD'))";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = " INSERT INTO KOSMOS_OCS.OCS_DRUG  ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + "  VALUES ( TO_DATE('" + strDate + "','YYYY-MM-DD') , '2', '3', 'ER', 'DZP10', '5','5','1', TO_DATE('" + strDate + "','YYYY-MM-DD'))";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = " INSERT INTO KOSMOS_OCS.OCS_DRUG  ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + "  VALUES ( TO_DATE('" + strDate + "','YYYY-MM-DD') , '2', '3', 'ER', 'A-POL12A', '2','2','1', TO_DATE('" + strDate + "','YYYY-MM-DD'))";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = " INSERT INTO KOSMOS_OCS.OCS_DRUG  ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + "  VALUES ( TO_DATE('" + strDate + "','YYYY-MM-DD') , '2', '3', 'ER', 'A-KETA5', '1','1','1', TO_DATE('" + strDate + "','YYYY-MM-DD'))";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = " INSERT INTO KOSMOS_OCS.OCS_DRUG  ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + "  VALUES ( TO_DATE('" + strDate + "','YYYY-MM-DD') , '2', '3', 'ER', 'LZPA', '3','3','1', TO_DATE('" + strDate + "','YYYY-MM-DD'))";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = " INSERT INTO KOSMOS_OCS.OCS_DRUG  ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + "  VALUES ( TO_DATE('" + strDate + "','YYYY-MM-DD') , '2', '3', 'ER', 'NALPA', '1','1','1', TO_DATE('" + strDate + "','YYYY-MM-DD'))";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = " INSERT INTO KOSMOS_OCS.OCS_DRUG  ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + "  VALUES ( TO_DATE('" + strDate + "','YYYY-MM-DD') , '2', '3', 'ER', 'A-ETOM', '3','3','1', TO_DATE('" + strDate + "','YYYY-MM-DD'))";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        #endregion
                        break;
                    case "32":
                    case "33":
                    case "35":
                        #region ICU
                        SQL = "";
                        SQL = " INSERT INTO KOSMOS_OCS.OCS_DRUG  ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + "  VALUES ( TO_DATE('" + strDate + "','YYYY-MM-DD') , '1', '3', '32', 'N-PTD25', '2','2','1' ,TO_DATE('" + strDate + "','YYYY-MM-DD'))";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = " INSERT INTO KOSMOS_OCS.OCS_DRUG  ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + "  VALUES ( TO_DATE('" + strDate + "','YYYY-MM-DD') , '1', '3', '32', 'N-MP-HA', '1','1','1', TO_DATE('" + strDate + "','YYYY-MM-DD'))";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = " INSERT INTO KOSMOS_OCS.OCS_DRUG  ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + "  VALUES ( TO_DATE('" + strDate + "','YYYY-MM-DD') , '1', '3', '32', 'N-FT-HA', '2','2','1', TO_DATE('" + strDate + "','YYYY-MM-DD'))";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = " INSERT INTO KOSMOS_OCS.OCS_DRUG  ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + "  VALUES ( TO_DATE('" + strDate + "','YYYY-MM-DD') , '2', '3', '32', 'A-BASCA', '13','13','1', TO_DATE('" + strDate + "','YYYY-MM-DD'))";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = " INSERT INTO KOSMOS_OCS.OCS_DRUG  ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + "  VALUES ( TO_DATE('" + strDate + "','YYYY-MM-DD') , '2', '3', '32', 'A-PEN250', '1','1','1', TO_DATE('" + strDate + "','YYYY-MM-DD'))";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = " INSERT INTO KOSMOS_OCS.OCS_DRUG  ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + "  VALUES ( TO_DATE('" + strDate + "','YYYY-MM-DD') , '2', '3', '32', 'DZP10', '3','3','1', TO_DATE('" + strDate + "','YYYY-MM-DD'))";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = " INSERT INTO KOSMOS_OCS.OCS_DRUG  ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + "  VALUES ( TO_DATE('" + strDate + "','YYYY-MM-DD') , '2', '3', '32', 'A-POL12A', '1','1','1', TO_DATE('" + strDate + "','YYYY-MM-DD'))";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = " INSERT INTO KOSMOS_OCS.OCS_DRUG  ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + "  VALUES ( TO_DATE('" + strDate + "','YYYY-MM-DD') , '2', '3', '32', 'LZPA', '3','3','1', TO_DATE('" + strDate + "','YYYY-MM-DD'))";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = " INSERT INTO KOSMOS_OCS.OCS_DRUG  ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + "  VALUES ( TO_DATE('" + strDate + "','YYYY-MM-DD') , '2', '3', '32', 'A-POF50', '3','3','1', TO_DATE('" + strDate + "','YYYY-MM-DD'))";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        #endregion
                        break;
                    case "JS":
                        #region JS
                        SQL = "";
                        SQL = " INSERT INTO KOSMOS_OCS.OCS_DRUG  ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + "  VALUES ( TO_DATE('" + strDate + "','YYYY-MM-DD') , '1', '3', 'JS', 'N-PTD-HA', '5','5','1' ,TO_DATE('" + strDate + "','YYYY-MM-DD'))";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = " INSERT INTO KOSMOS_OCS.OCS_DRUG  ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + "  VALUES ( TO_DATE('" + strDate + "','YYYY-MM-DD') , '1', '3', 'JS', 'N-PTD25', '5','5','1' ,TO_DATE('" + strDate + "','YYYY-MM-DD'))";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = " INSERT INTO KOSMOS_OCS.OCS_DRUG  ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + "  VALUES ( TO_DATE('" + strDate + "','YYYY-MM-DD') , '1', '3', 'JS', 'N-MP-HA', '3','3','1', TO_DATE('" + strDate + "','YYYY-MM-DD'))";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = " INSERT INTO KOSMOS_OCS.OCS_DRUG  ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + "  VALUES ( TO_DATE('" + strDate + "','YYYY-MM-DD') , '2', '3', 'JS', 'DZP10', '5','5','1', TO_DATE('" + strDate + "','YYYY-MM-DD'))";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL = " INSERT INTO KOSMOS_OCS.OCS_DRUG  ( BDATE, GBN, GBN2, WARDCODE, SUCODE, QTY, REALQTY, NAL, BUILDDATE ) ";
                        SQL = SQL + "  VALUES ( TO_DATE('" + strDate + "','YYYY-MM-DD') , '2', '3', 'JS', 'NALPA', '2','2','1', TO_DATE('" + strDate + "','YYYY-MM-DD'))";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        #endregion
                        break;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnBuild_Click(object sender, EventArgs e)
        {
            using (frmComSupDrugListMagamEvent = new frmComSupDrugListMagam(GstrWardCode))
            {
                frmComSupDrugListMagamEvent.rEventClosed += Frm_rEventClosed;
                frmComSupDrugListMagamEvent.StartPosition = FormStartPosition.CenterParent;
                frmComSupDrugListMagamEvent.ShowDialog();
            }

            //2019-06-10 전산의뢰서 2019-664
            if (GstrWardCode == "AN")
            { 
                ssMagam.Visible = true;
                //READ_MAGAM_YN();
                READ_MAGAM_NEW_YN();
            }
            else
            {
                ssMagam.Visible = false;
            }
        }

        private void Frm_rEventClosed()
        {
            frmComSupDrugListMagamEvent.Dispose();
            frmComSupDrugListMagamEvent = null;

            SetChasu("MAGAM");            
            btnSearch.PerformClick();
        }

        private void rdoGB_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                FormClear();

                btnSearch_Click(this, e);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            switch(GstrWardCode)
            {
                case "AN":
                    if (dtpDate.Value >= Convert.ToDateTime("2014-09-01"))
                    {
                        READ_Drug_New();
                    }
                    else
                    {
                        READ_Drug();
                    }
                    break;
                case "4H":
                case "AG":
                case "ER":
                case "32":
                case "33":
                case "35":
                    READ_Drug_New();
                    break;
                case "JS":
                    READ_Drug();
                    break;
                case "EN":
                    if (dtpDate.Value >= Convert.ToDateTime("2015-01-05"))
                    {
                        READ_Drug();
                    }
                    else
                    {
                        ComFunc.MsgBox("비상마약류 관리대장은 2015-01-05일부터 조회가 가능합니다.");
                        return;
                    }
                    break;
                case "TO":
                    if (dtpDate.Value >= Convert.ToDateTime("2014-12-10"))
                    {
                        READ_Drug();
                    }
                    else
                    {
                        ComFunc.MsgBox("비상마약류 관리대장은 2014-12-10일부터 조회가 가능합니다.");
                        return;
                    }
                    break;
            }
        }

        private void READ_Drug()
        {
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            int i = 0;
            string SqlErr = "";

            ssList_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.SUCODE, B.JEPNAME, B.COVUNIT, C.UNIT, C.HNAME, A.QTY, A.ROWID";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUG A , " + ComNum.DB_ERP + "DRUG_JEP B, " + ComNum.DB_MED + "OCS_DRUGINFO_NEW C";
                SQL = SQL + ComNum.VBLF + "  WHERE A.BUILDDATE =TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND A.GBN2 IN ('3')  "; //재고
                SQL = SQL + ComNum.VBLF + "    AND A.WARDCODE ='" + GstrWardCode + "' ";

                switch (GstrWardCode)
                {
                    case "AN":
                    case "JS":
                        SQL = SQL + ComNum.VBLF + "    AND A.SUCODE = B.JEPCODE ";
                        SQL = SQL + ComNum.VBLF + "    AND A.SUCODE = C.SUNEXT ";
                        break;
                    case "EN":
                    case "TO":
                        SQL = SQL + ComNum.VBLF + "    AND A.SUCODE = B.JEPCODE(+) ";
                        SQL = SQL + ComNum.VBLF + "    AND A.SUCODE = C.SUNEXT(+) ";
                        SQL = SQL + ComNum.VBLF + "    AND A.SUCODE NOT IN ('A-POL12', 'A-POL12G') ";
                        break;
                }

                //2020-10-06 추가 
                SQL = SQL + ComNum.VBLF + "    AND A.SUCODE NOT IN ('A-BASCA') ";

                if (rdoGB0.Checked == true) { SQL = SQL + ComNum.VBLF + "    AND A.GBN = '1' "; }
                if (rdoGB1.Checked == true) { SQL = SQL + ComNum.VBLF + "    AND A.GBN = '2' "; }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssList_Sheet1.RowCount = dt.Rows.Count;
                ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for(i = 0; i < dt.Rows.Count; i++)
                {
                    ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["QTY"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 2].Text = "0";
                    ssList_Sheet1.Cells[i, 2].ForeColor = Color.Red;
                    ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["HNAME"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["UNIT"].ToString().Trim();

                    SQL = "";
                    SQL = "SELECT SUM(QTY* NAL) AS QTY FROM " + ComNum.DB_MED + "OCS_DRUG ";
                    SQL = SQL + ComNum.VBLF + "  WHERE BUILDDATE = TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "   AND GBN2 IN ('2')  ";    //입고(소모)
                    SQL = SQL + ComNum.VBLF + "   AND WARDCODE ='" + GstrWardCode + "' ";     //주사실

                    if (dt.Rows[i]["SUCODE"].ToString().Trim() == "A-ANE12G")
                    {
                        SQL = SQL + ComNum.VBLF + "   AND SUCODE IN ('A-ANE12', 'A-ANE12G') ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "   AND SUCODE =  '" + dt.Rows[i]["SUCODE"].ToString().Trim() + "' ";
                    }

                    if (GstrWardCode != "AN" && GstrWardCode != "EN" && GstrWardCode != "TO")
                    {
                        SQL = SQL + ComNum.VBLF + "AND NO1 = '" + cboChasu.Text.Replace("차수", "") + "' ";
                    }

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        if (VB.Val(dt1.Rows[0]["QTY"].ToString().Trim()) > 0)
                        {
                            if (ssList_Sheet1.Cells[i, 0].Text == "A-ANE12")
                            {
                                ssList_Sheet1.Cells[i, 2].Text = "0";
                            }
                            else
                            {
                                ssList_Sheet1.Cells[i, 2].Text = dt1.Rows[0]["QTY"].ToString().Trim();
                            }
                            ssList_Sheet1.Cells[i, 2].ForeColor = Color.Red;
                        }
                    }

                    dt1.Dispose();
                    dt1 = null;

                    Application.DoEvents();
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

        private void READ_Drug_New()
        {
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            int i = 0;
            string SqlErr = "";

            ssList_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "SELECT MAX(A.JDATE) AS JDATE, A.SUCODE ";
                SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_MED + "OCS_DRUG_SET A ";
                SQL = SQL + ComNum.VBLF + "WHERE WARDCODE = '" + GstrWardCode + "' ";
                SQL = SQL + ComNum.VBLF + "     AND JDATE <= TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";

                if (rdoGB0.Checked == true) { SQL = SQL + ComNum.VBLF + "     AND GBN ='1'"; }
                if (rdoGB1.Checked == true) { SQL = SQL + ComNum.VBLF + "     AND GBN ='2'"; }

                SQL = SQL + ComNum.VBLF + "GROUP BY SUCODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                for(i = 0; i < dt.Rows.Count; i++)
                {
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.SUCODE, B.JEPNAME, B.COVUNIT, A.QTY, C.HNAME, C.UNIT   ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUG_SET A , " + ComNum.DB_ERP + "DRUG_JEP B, " + ComNum.DB_MED + "OCS_DRUGINFO_NEW  C";
                    SQL = SQL + ComNum.VBLF + "  WHERE A.JDATE = TO_DATE('" + Convert.ToDateTime(dt.Rows[i]["JDATE"].ToString().Trim()).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND A.WARDCODE = '" + GstrWardCode + "' ";                          //마취과
                    SQL = SQL + ComNum.VBLF + "    AND A.SUCODE = '" + dt.Rows[i]["SUCODE"].ToString().Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND A.SUCODE = B.JEPCODE ";
                    SQL = SQL + ComNum.VBLF + "    AND A.SUCODE = C.SUNEXT ";
                    SQL = SQL + ComNum.VBLF + "    AND A.QTY <> 0 ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        ssList_Sheet1.RowCount = ssList_Sheet1.RowCount + 1;
                        ssList_Sheet1.SetRowHeight(ssList_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 0].Text = dt1.Rows[0]["SUCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 1].Text = dt1.Rows[0]["QTY"].ToString().Trim();
                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 2].Text = "0";
                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 2].ForeColor = Color.Red;
                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 3].Text = dt1.Rows[0]["HNAME"].ToString().Trim();
                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 4].Text = dt1.Rows[0]["UNIT"].ToString().Trim();

                        dt1.Dispose();
                        dt1 = null;

                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     SUM(QTY* NAL ) AS QTY";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUG ";
                        SQL = SQL + ComNum.VBLF + "  WHERE BUILDDATE =TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "    AND GBN2 IN ( '2')  ";    //입고(소모)
                        SQL = SQL + ComNum.VBLF + "    AND WARDCODE = '" + GstrWardCode + "' ";     //마취과
                        SQL = SQL + ComNum.VBLF + "    AND SUCODE =  '" + dt.Rows[i]["SUCODE"].ToString().Trim() + "' ";
                        
                        switch(GstrWardCode)
                        {
                            case "AN":
                                if (cboChasu.Text.Replace("차수", "").Trim() != "")
                                {
                                    SQL = SQL + ComNum.VBLF + "    AND NO1 = '" + cboChasu.Text.Replace("차수", "").Trim() + "' ";
                                }
                                break;
                            case "4H":
                            case "AG":
                            case "ER":
                            case "32":
                            case "33":
                            case "35":
                                SQL = SQL + ComNum.VBLF + "    AND NO1 = '" + cboChasu.Text.Replace("차수", "").Trim() + "' ";
                                break;
                        }

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            if (VB.Val(dt1.Rows[0]["QTY"].ToString().Trim()) > 0)
                            {
                                ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 2].Text = dt1.Rows[0]["QTY"].ToString().Trim();
                                ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 2].ForeColor = Color.Red;
                            }
                        }
                    }

                    dt1.Dispose();
                    dt1 = null;

                    Application.DoEvents();
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            FormClear();
        }

        private void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader || e.ColumnHeader) { return; }

            string strDCode = "";
            string strDName = "";
            string strDQty = "0";
            string strBQty = "0";
            string strUnit = "";
            
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            ssList_Sheet1.Cells[0, 0, ssList_Sheet1.RowCount - 1, ssList_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssList_Sheet1.Cells[e.Row, 0, e.Row, ssList_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            try
            {
                strDCode = ssList_Sheet1.Cells[e.Row, 0].Text.Trim();
                strDQty = ssList_Sheet1.Cells[e.Row, 1].Text.Trim();
                strDName = ssList_Sheet1.Cells[e.Row, 3].Text.Trim();
                strUnit = ssList_Sheet1.Cells[e.Row, 4].Text.Trim();

                if (GstrWardCode == "AN")
                {
                    if (dtpDate.Value >= Convert.ToDateTime("2013-09-10"))
                    {
                        switch (strDCode)
                        {
                            case "N-PTD-HA": strBQty = "10A"; break;
                            case "N-FT-HA": strBQty = "70A"; break;
                            case "N-FT10": strBQty = "20A"; break;
                            case "N-ULVA1": strBQty = "20V"; break;
                            case "N-MP-HA": strBQty = "4A"; break;
                            case "DZP10": strBQty = "3A"; break;
                            case "A-BASCA": strBQty = "30A"; break;
                            case "A-KETA5": strBQty = "10A"; break;
                            case "A-PEN250": strBQty = "10V"; break;
                            case "A-POF50": strBQty = "20V"; break;
                            case "A-POL2": strBQty = "70A"; break;
                            case "A-POL12A": strBQty = "20A"; break;
                            default: strBQty = "0"; break;
                        }
                    }
                    else if (dtpDate.Value >= Convert.ToDateTime("2013-01-03"))
                    {
                        switch (strDCode)
                        {
                            case "N-PTD-HA": strBQty = "4A"; break;
                            case "N-FT-HA": strBQty = "50A"; break;
                            case "N-FT10": strBQty = "20A"; break;
                            case "N-ULVA1": strBQty = "20V"; break;
                            case "N-MP-HA": strBQty = "1A"; break;
                            case "DZP10": strBQty = "3A"; break;
                            case "A-BASCA": strBQty = "30A"; break;
                            case "A-KETA5": strBQty = "5A"; break;
                            case "A-PEN250": strBQty = "7V"; break;
                            case "A-POF50": strBQty = "20V"; break;
                            case "A-POL2": strBQty = "70A"; break;
                            case "A-POL12A": strBQty = "20A"; break;
                            default: strBQty = "0"; break;
                        }
                    }
                    else
                    {
                        switch (strDCode)
                        {
                            case "N-PTD-HA": strBQty = "4A"; break;
                            case "N-FT-HA": strBQty = "30A"; break;
                            case "N-FT10": strBQty = "10A"; break;
                            case "N-ULVA1": strBQty = "5V"; break;
                            case "N-MP-HA": strBQty = "1A"; break;
                            case "DZP10": strBQty = "3A"; break;
                            case "A-BASCA": strBQty = "20A"; break;
                            case "A-KETA5": strBQty = "5A"; break;
                            case "A-PEN250": strBQty = "7V"; break;
                            case "A-POF50": strBQty = "10V"; break;
                            case "A-POL2": strBQty = "30A"; break;
                            case "A-POL12A": strBQty = "10A"; break;
                            default: strBQty = "0"; break;
                        }
                    }
                }
                else if (GstrWardCode == "ER")
                {
                    if (dtpDate.Value >= Convert.ToDateTime("2014-05-06"))
                    {
                        switch(strDCode)
                        {
                            case "N-PTD-HA": strBQty = "10A"; break;
                            case "N-PTD25":  strBQty = "10A"; break;
                            case "N-MP-HA":  strBQty = "10A"; break;
                            case "N-FT-HA":  strBQty = "3A"; break;
                            case "A-BASCA":  strBQty = "15A"; break;
                            case "A-PEN250": strBQty = "3V"; break;
                            case "DZP10":    strBQty = "10A"; break;
                            case "A-POL12A":   strBQty = "2A"; break;
                            case "A-KETA5":  strBQty = "8A"; break;
                            case "LZPA":   strBQty = "10A"; break;
                            case "NALPA":   strBQty = "1A"; break;
                            case "A-ETOM":   strBQty = "3A"; break;
                            default: strBQty = "0"; break;
                        }
                    }
                    else if (dtpDate.Value >= Convert.ToDateTime("2014-05-01"))
                    {
                        switch (strDCode)
                        {
                            case "N-PTD-HA": strBQty = "10A"; break;
                            case "N-PTD25":  strBQty = "10A"; break;
                            case "N-MP-HA":  strBQty = "10A"; break;
                            case "N-FT-HA":  strBQty = "3A"; break;
                            case "A-BASCA":  strBQty = "15A"; break;
                            case "A-PEN250": strBQty = "3V"; break;
                            case "DZP10":    strBQty = "5A"; break;
                            case "A-POL12A":   strBQty = "2A"; break;
                            case "A-KETA5":  strBQty = "3A"; break;
                            case "LZPA":   strBQty = "3A"; break;
                            case "NALPA":   strBQty = "1A"; break;
                            case "A-ETOM":   strBQty = "3A"; break;
                            default: strBQty = "0"; break;
                        }
                    }
                    else if (dtpDate.Value >= Convert.ToDateTime("2014-02-07"))
                    {
                        switch (strDCode)
                        {
                            case "N-PTD-HA": strBQty = "10A"; break;
                            case "N-PTD25":  strBQty = "10A"; break;
                            case "N-MP-HA":  strBQty = "10A"; break;
                            case "N-FT-HA":  strBQty = "3A"; break;
                            case "A-BASCA":  strBQty = "3A"; break;
                            case "A-PEN250": strBQty = "3V"; break;
                            case "DZP10":    strBQty = "5A"; break;
                            case "A-POL12A":   strBQty = "2A"; break;
                            case "A-KETA5":  strBQty = "3A"; break;
                            case "LZPA":   strBQty = "3A"; break;
                            case "NALPA":   strBQty = "1A"; break;
                            case "A-ETOM":   strBQty = "3A"; break;
                            default: strBQty = "0"; break;
                        }
                    }
                }

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.SUCODE, A.BQTY, A.QTY";

                if (GstrWardCode == "EN" || GstrWardCode == "TO")
                {
                    SQL = SQL + ", B.UNITNEW1";
                }

                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUG_SET A";

                if (GstrWardCode == "EN" || GstrWardCode == "TO")
                {
                    SQL = SQL + ", " + ComNum.DB_PMPA + "BAS_SUN B";
                }

                SQL = SQL + ComNum.VBLF + "  WHERE A.SUCODE = '" + strDCode + "' ";
                SQL = SQL + ComNum.VBLF + "    AND A.WARDCODE = '" + GstrWardCode + "'  ";  //마취과
                SQL = SQL + ComNum.VBLF + "    AND A.JDATE <= TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                
                if (GstrWardCode == "EN" || GstrWardCode == "TO")
                {
                    SQL = SQL + ComNum.VBLF + "    AND A.SUCODE = B.SUNEXT(+)";
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY A.JDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strBQty = dt.Rows[0]["BQTY"].ToString().Trim();

                    if (GstrWardCode == "EN" || GstrWardCode == "TO")
                    {
                        GintUnit = Convert.ToInt32(VB.Val(dt.Rows[0]["UNITNEW1"].ToString().Trim()));
                    }
                }

                dt.Dispose();
                dt = null;

                txtDCode.Text = strDCode;
                txtDName.Text = strDName;
                txtDQty.Text = strDQty;

                if (GstrWardCode == "TO")
                {
                    txtBQty.Text = VB.Pstr(strBQty, "A", 1);
                }
                else if (GstrWardCode != "EN")
                {
                    txtBQty.Text = strBQty;
                }

                txtUnit.Text = strUnit;

                READ_OCS_DURG(strDCode);
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

        private void READ_OCS_DURG(string strCode)
        {
            string SQL = "";
            DataTable dt = null;
            int i = 0;

            string SqlErr = "";

            double dblQty = 0;
            double dblJQty = 0;

            double dblJanQty = 0;
            double dblJegoQty = 0;

            double dblChasu = 0;

            ssView_Sheet1.RowCount = 0;

            switch (GstrWardCode)
            {
                case "AN":
                case "4H": 
                case "ER":
                case "32":
                case "33":
                case "35":
                case "JS":
                case "AG":
                case "TO":
                    dblChasu = VB.Val(VB.Left(cboChasu.Text, 1));

                    //if (dblChasu == 0)
                    //{
                    //    ComFunc.MsgBox("조회 오류 입니다. 마감을 다시 작업하거나 전산정보과에 확인바랍니다.");
                    //    return;
                    //}
                    break;
            }


        
            try
            {
                switch (GstrWardCode)
                {
                    case "AN":
                    case "AG":
                    case "ER":
                    case "JS":
                        //이월(전일재고) --------------------------------------------------------------------------------------------------------------------------------
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     GBN2, A.SUCODE, B.JEPNAME, A.QTY, A.ROWID ";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUG A , " + ComNum.DB_ERP + "DRUG_JEP B";
                        SQL = SQL + ComNum.VBLF + "  WHERE A.BUILDDATE = TO_DATE('" + dtpDate.Value.AddDays(-1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "    AND A.GBN2 IN ('3')  "; //재고
                        SQL = SQL + ComNum.VBLF + "    AND A.WARDCODE = '" + GstrWardCode + "' "; //마취과
                        SQL = SQL + ComNum.VBLF + "    AND A.SUCODE = B.JEPCODE ";
                        SQL = SQL + ComNum.VBLF + "    AND A.SUCODE = '" + strCode + "' ";

                        if (rdoGB0.Checked == true) { SQL = SQL + ComNum.VBLF + "    AND A.GBN = '1'"; }
                        if (rdoGB1.Checked == true) { SQL = SQL + ComNum.VBLF + "    AND A.GBN = '2'"; }
                        break;
                    case "4H":
                    case "32":
                    case "33":
                    case "35":
                        //이월 (전일재고로 하지 않고  당일비치수량으로 한다
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     GBN, SUCODE, QTY ";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUG_SET";
                        SQL = SQL + ComNum.VBLF + "  WHERE JDATE <= TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "    AND WARDCODE = '" + GstrWardCode + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND SUCODE = '" + strCode + "' ";
                        SQL = SQL + ComNum.VBLF + "ORDER BY JDATE DESC";
                        break;
                    case "EN":
                    case "TO":
                        //당일재고 --------------------------------------------------------------------------------------------------------------------------------
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     GBN2, A.SUCODE, B.SUNAMEK, A.QTY, A.ROWID ";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUG A, " + ComNum.DB_PMPA + "BAS_SUN B";
                        SQL = SQL + ComNum.VBLF + "  WHERE A.BUILDDATE = TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "    AND A.GBN2 IN ('3')";        //재고
                        SQL = SQL + ComNum.VBLF + "    AND A.WARDCODE = '" + GstrWardCode + "' ";      //내시경
                        SQL = SQL + ComNum.VBLF + "    AND A.SUCODE = B.SUNEXT ";                        
                        SQL = SQL + ComNum.VBLF + "    AND A.SUCODE = '" + strCode + "' ";                                              

                        if (rdoGB0.Checked == true) { SQL = SQL + ComNum.VBLF + " AND A.GBN = '1'"; }
                        if (rdoGB1.Checked == true) { SQL = SQL + ComNum.VBLF + " AND A.GBN = '2'"; }
                        break;
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                    ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                    if (GstrWardCode != "EN" && GstrWardCode != "TO")
                    {
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "전일재고량"; 
                    }
                    else
                    {
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "당일비치량";
                    }

                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = dt.Rows[0]["QTY"].ToString().Trim();

                    dblJQty = dblJQty + VB.Val(dt.Rows[0]["QTY"].ToString().Trim());
                }
                else
                {
                    if (GstrWardCode != "EN" && GstrWardCode != "TO")
                    {
                        ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                        ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "전일재고량";
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = txtDQty.Text.Trim();

                        dblJQty = dblJQty + VB.Val(txtDQty.Text.Trim());
                    }
                    else
                    {
                        ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                        ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "당일비치량";
                    }
                }

                dt.Dispose();
                dt = null;

                dblJegoQty = dblJQty;

                //소모 --------------------------------------------------------------------------------------------------------------------------------
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     GBN2, TO_CHAR(A.BDATE,'YYYY-MM-DD') AS BDATE, A.IO, A.ROOMCODE, A.SUCODE, A.DEPTCODE, A.PTNO, A.SUCODER, ";
                SQL = SQL + ComNum.VBLF + "     A.SNAME, ";

                if (GstrWardCode != "EN")
                {
                    SQL = SQL + "B.JEPNAME, ";
                }
                else
                {
                    SQL = SQL + "B.SUNAMEK, ";
                }

                SQL = SQL + "(A.REALQTY * NAL) AS REALQTY, (A.QTY * NAL) AS QTY, (A.QTY * NAL) - (A.REALQTY * NAL) AS JQTY, A.ROWID, A.ORDERNO ";

                if (GstrWardCode != "EN")
                {
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUG A, " + ComNum.DB_ERP + "DRUG_JEP B";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUG A, " + ComNum.DB_PMPA + "BAS_SUN B";
                }

                SQL = SQL + ComNum.VBLF + "  WHERE A.BUILDDATE = TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND A.GBN2 IN ('2')  "; //소모
                SQL = SQL + ComNum.VBLF + "    AND A.WARDCODE = '" + GstrWardCode + "' "; //마취과

                if (GstrWardCode != "EN")
                {
                    SQL = SQL + ComNum.VBLF + "    AND A.SUCODE = B.JEPCODE ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND A.SUCODE = B.SUNEXT ";
                }

                if (GstrWardCode == "TO")
                {
                    SQL = SQL + "(+)";
                }

                if(GstrWardCode == "EN")
                {
                    if (strCode == "A-ANE12G")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND A.SUCODE IN ('A-ANE12G','A-ANE12')";
                    }
                    else if(strCode == "A-ANE12")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND A.SUCODE NOT IN ('A-ANE12') ";                        
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "    AND A.SUCODE = '" + strCode + "' ";
                    }
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND A.SUCODE = '" + strCode + "' ";
                }
                

                if (rdoGB0.Checked == true) { SQL = SQL + ComNum.VBLF + "    AND A.GBN = '1'"; }
                if (rdoGB1.Checked == true) { SQL = SQL + ComNum.VBLF + "    AND A.GBN = '2'"; }

                if (GstrWardCode != "EN" && (int)dblChasu > 0) 
                {
                    SQL = SQL + ComNum.VBLF + "    AND A.NO1 = '" + (int)dblChasu + "' ";
                }
                //if (GstrWardCode != "AN" && GstrWardCode != "EN")
                //{
                //    SQL = SQL + ComNum.VBLF + "    AND A.NO1 = '" + (int)dblChasu + "' ";
                //}

                //19-04-19 약제 팀장님 요청으로 이름순으로 변경
                //if (clsType.User.BuseCode == "044101")
                //{
                //    SQL = SQL + ComNum.VBLF + "ORDER BY A.SNAME";
                //}
                if (GstrWardCode == "EN")
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY SUNEXT, A.DEPTCODE, A.SNAME";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY A.SUCODER, ENTDATE ASC";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        if (strCode == "A-ANE12" && GstrWardCode == "EN") break;

                        ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                        ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = (i + 1).ToString();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = VB.Val(dt.Rows[i]["QTY"].ToString().Trim()) > 0 ?  "소모" : "반환" ;
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["BDATE"].ToString().Trim();

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();

                        if (GstrWardCode != "TO")
                        {
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["IO"].ToString().Trim() == "I" ? dt.Rows[i]["ROOMCODE"].ToString().Trim() : "외래";
                        }
                        else
                        {
                            if (dt.Rows[i]["IO"].ToString().Trim() == "I")
                            {
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                            }
                            else
                            {
                                if (dt.Rows[i]["DEPTCODE"].ToString().Trim() == "TO" || dt.Rows[i]["DEPTCODE"].ToString().Trim() == "HR")
                                {
                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = "종검";
                                }
                                else
                                {
                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = "외래";
                                }
                            }
                        }

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = dt.Rows[i]["PTNO"].ToString().Trim();

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = dt.Rows[i]["SUCODER"].ToString().Trim();

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = dt.Rows[i]["REALQTY"].ToString().Trim();

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = dt.Rows[i]["QTY"].ToString().Trim();

                        dblQty = dblQty + VB.Val(dt.Rows[i]["QTY"].ToString().Trim());

                        if (VB.Val(dt.Rows[i]["QTY"].ToString().Trim()) < 0)
                        {
                            dblJegoQty = dblJegoQty - VB.Fix((int)((VB.Val(dt.Rows[i]["QTY"].ToString().Trim()) * 10 - 9) / 10));
                        }
                        else
                        {
                            dblJegoQty = dblJegoQty - VB.Fix((int)((VB.Val(dt.Rows[i]["QTY"].ToString().Trim()) * 10 + 9) / 10));
                        }

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = dblJegoQty.ToString();

                        dblJanQty = VB.Val(dt.Rows[i]["JQTY"].ToString().Trim());

                        if (dblJanQty != 0)
                        {
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13].Text = dblJanQty.ToString();
                        }

                        //if (dt.Rows[i]["PTNO"].ToString().Trim() == "07713751")
                        //{
                        //    i = i;
                        //}

                        //19-05-14 모든부서 다 보이게 수정(약제팀 요청)
                        //if (GstrWardCode == "4H" || GstrWardCode == "AN")
                        //{
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 16].Text = dt.Rows[i]["ORDERNO"].ToString().Trim();
                        //}
                    }
                }

                dt.Dispose();
                dt = null;

                ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "소 모 합 계";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = dblQty.ToString();

                dblJQty = dblJQty - dblQty;

                switch(GstrWardCode)
                {
                    case "AN":
                    case "AG":
                    case "EN":
                    case "ER":
                    case "JS":
                    case "TO":
                        //입고 --------------------------------------------------------------------------------------------------------------------------------
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     TO_CHAR(MAX(ENTDATE),'HH24:MI') AS TIME, SUM(A.QTY) AS QTY";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUG A, " + ComNum.DB_ERP + "DRUG_JEP B";
                        SQL = SQL + ComNum.VBLF + "  WHERE TRUNC(A.BUILDDATE) = TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "    AND A.GBN2 IN ('1')  ";   //입고
                        SQL = SQL + ComNum.VBLF + "    AND A.WARDCODE = '" + GstrWardCode + "' ";    //마취과
                        SQL = SQL + ComNum.VBLF + "    AND A.SUCODE = B.JEPCODE ";
                        SQL = SQL + ComNum.VBLF + "    AND A.SUCODE = '" + strCode + "' ";

                        switch(GstrWardCode)
                        {
                            case "ER":
                            case "JS":
                                SQL = SQL + ComNum.VBLF + "    AND A.NO1 = '" + (int)dblChasu + "' ";
                                break;
                        }

                        if (rdoGB0.Checked == true) { SQL = SQL + ComNum.VBLF + "     AND A.GBN = '1'"; }
                        if (rdoGB1.Checked == true) { SQL = SQL + ComNum.VBLF + "     AND A.GBN = '2'"; }

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                            ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT + ComNum.SPDROWHT);

                            if (GstrWardCode != "EN" && GstrWardCode != "TO")
                            {
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "약제팀불출";
                            }
                            else
                            {
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "당일반환량";
                            }


                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dtpDate.Value.ToString("yyyy-MM-dd");

                            ///의뢰서 작업
                            switch(GstrWardCode)
                            {
                                case "EN":
                                case "TO":
                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = dblJegoQty.ToString();
                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = "0";
                                    break;
                                case "AN":
                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = dblQty.ToString();
                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = ssView_Sheet1.Cells[0, 11].Text;
                                    break;
                            }

                            dblJQty = dblJQty + VB.Val(dt.Rows[0]["QTY"].ToString().Trim());
                        }
                        else
                        {
                            ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                            ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT + ComNum.SPDROWHT);

                            if (GstrWardCode != "EN" && GstrWardCode != "TO")
                            {
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "약제팀불출";
                            }
                            else
                            {
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "당일반환량"; 
                            }

                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = "";
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = "";
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = "";
                        }

                        dt.Dispose();
                        dt = null;

                        //통계 오류 점검
                        //if (GstrWardCode == "AN" || GstrWardCode == "ER")
                        if (GstrWardCode == "ER")
                        {
                            if (dblJQty > VB.Val(txtDQty.Text))
                            {
                                ComFunc.MsgBox("비치수량: " + txtDQty.Text + ComNum.VBLF + "계산수량:" + dblJQty + ComNum.VBLF + " 해당 마감 오류 입니다.");
                            }
                        }
                        break;
                    case "4H":
                    case "32":
                    case "33":
                    case "35":
                        ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                        ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT + ComNum.SPDROWHT);

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "약제팀불출";
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dtpDate.Value.ToString("yyyy-MM-dd");
                        break;
                }
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            { 
                return;
            }

            string strFont1 = string.Empty;
            string strEntDate = string.Empty;
            string strHead1 = string.Empty;
            string strBuse = string.Empty;
            string strChk1 = string.Empty;
            string strChk2 = string.Empty;
            string strBuseName = string.Empty;
            int intPageCnt = 0;
            int intPage = 1;

            Replay:

            SetPrt();

            ssViewPrint_Sheet1.RowCount = 8;
            //ssViewPrint_Sheet1.Columns[16, 17].Visible = false;

            switch (GstrWardCode)
            {
                case "AN":
                    strBuse = "마취과";
                    strChk1 = "수술실 실장";
                    strChk2 = "수 간 호 사";
                    strBuseName = "마취과";
                    break;
                case "32":  //중환자실(ICU) -- 과거
                    strBuse = "중환자실";
                    strChk1 = "  실  장  ";
                    strChk2 = "수 간 호 사";
                    strBuseName = "병동";
                    break;
                case "33":  //응급 중환자실(EICU)
                    strBuse = "EICU";
                    strChk1 = "  실  장  ";
                    strChk2 = "수 간 호 사";
                    strBuseName = "병동";
                    break;
                case "35":  //중환자실(GICU)
                    strBuse = "GICU";
                    strChk1 = "  실  장  ";
                    strChk2 = "수 간 호 사";
                    strBuseName = "병동";
                    break;
                case "4H":  //호스피스병동
                    strBuse = "호스피스병동";
                    strChk1 = "  실  장  ";
                    strChk2 = "수 간 호 사";
                    strBuseName = "병동";
                    break;
                case "AG":  //혈관조영실
                    strBuse = "혈관조영실";
                    strChk1 = "영상의학과장";
                    strChk2 = " 간 호 사 ";
                    strBuseName = "혈관조영실";
                    break;
                case "EN":  //내시경실
                    strBuse = "내시경실";
                    strChk1 = " 진료과장 ";
                    strChk2 = "  수간호  ";
                    strBuseName = "내시경실";
                    break;
                case "ER":  //응급의료센터
                    strBuse = "응급의료센타";
                    strChk1 = "  실  장  ";
                    strChk2 = "수 간 호 사";
                    strBuseName = "응급센타";
                    break;
                case "JS":  //주사실
                    strBuse = "본관주사실";
                    strChk1 = " 간호과장 ";
                    strChk2 = " 간 호 사 ";
                    strBuseName = "주사실";
                    break;
                case "TO":  //종합검진
                    strBuse = "종검";
                    strChk1 = " 진료과장 ";
                    strChk2 = " 간호팀장 ";
                    strBuseName = "종합건진";
                    break;
            }



            strFont1 = "/fn\"굴림체\" /fz\"18\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strHead1 = "/c/f1" + "비상마약류 관리대장 (" + strBuse + ")" + "/f1/n";
            
            ssViewPrint_Sheet1.Cells[0, 0].Text = "일    자 : " + dtpDate.Value.ToString("yyyy-MM-dd");
            ssViewPrint_Sheet1.Cells[0, 0].Text += ComNum.VBLF + "약 품 명 : " + txtDName.Text;
            ssViewPrint_Sheet1.Cells[0, 0].Text += ComNum.VBLF + "구    분 : " + (rdoGB0.Checked == true ? "마약" : "향정신성의약품");
            ssViewPrint_Sheet1.Cells[0, 0].Text += ComNum.VBLF + "약품코드 : " + txtDCode.Text;
            ssViewPrint_Sheet1.Cells[0, 0].Text += ComNum.VBLF + "단    위 : " + txtUnit.Text;
            ssViewPrint_Sheet1.Cells[0, 0].Text += ComNum.VBLF + "비치수량 : " + txtBQty.Text;

            //if (GstrWardCode != "AN" && GstrWardCode != "EN")
            if (GstrWardCode != "EN")
            {
                strEntDate = GetChasu();

                ssViewPrint_Sheet1.Cells[0, 0].Text += ComNum.VBLF + "차    수 : " + cboChasu.Text + " " + strEntDate;
            }

            ssViewPrint_Sheet1.Cells[0, 0].Text += ComNum.VBLF + "확    인 : " + strBuseName  + "                  약제팀          ";

            ssViewPrint_Sheet1.Cells[0, 10].Text = "마약류관리보조자" + ComNum.VBLF + "정 (" + strChk1 + ")";
            ssViewPrint_Sheet1.Cells[0, 14].Text = "마약류관리보조자" + ComNum.VBLF + "부 (" + strChk2 + ")";

            #region 19-05-30 의뢰서 관련 작업.
            ssViewPrint_Sheet1.Columns[15].Width = 50f;
            ssViewPrint_Sheet1.Columns[16, 17].Width = 43f;
            ssViewPrint_Sheet1.Cells[ssViewPrint_Sheet1.RowCount - 1, 16].ColumnSpan = 2;
            ssViewPrint_Sheet1.Cells[ssViewPrint_Sheet1.RowCount - 1, 16].Text = "Order\r\nNo";
            #endregion

            //ssViewPrint_Sheet1.Cells.Get(7, 16).Value = strBuseName + "\r\n확인"; //19-05-30 주석처리(의뢰서 작업)

            FarPoint.Win.ComplexBorder complexBorder0 = 
                new FarPoint.Win.ComplexBorder(
                    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), 
                    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), 
                    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.Black), 
                    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.Black), 
                    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder complexBorder1 =
                new FarPoint.Win.ComplexBorder(
                    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black),
                    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black),
                    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black),
                    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.Black),
                    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder complexBorder2 =
                new FarPoint.Win.ComplexBorder(
                    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black),
                    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black),
                    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.Black),
                    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black),
                    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            FarPoint.Win.ComplexBorder complexBorder3 =
                new FarPoint.Win.ComplexBorder(
                    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black));

            for (int i = intPageCnt; i < ssView_Sheet1.RowCount; i++)
            {
                ssViewPrint_Sheet1.RowCount = ssViewPrint_Sheet1.RowCount + 1;
                ssViewPrint_Sheet1.SetRowHeight(ssViewPrint_Sheet1.RowCount - 1, (int)ssView_Sheet1.Rows[i].Height);

                #region 19-05-30 추가
                ssViewPrint_Sheet1.Cells[ssViewPrint_Sheet1.RowCount - 1, 16].ColumnSpan = 2;
                #endregion
                ssViewPrint_Sheet1.Cells[ssViewPrint_Sheet1.RowCount - 1, 8].ColumnSpan = 3;

                ssViewPrint_Sheet1.Cells[ssViewPrint_Sheet1.RowCount - 1, 0].Text = ssView_Sheet1.Cells[i, 0].Text;
                ssViewPrint_Sheet1.Cells[ssViewPrint_Sheet1.RowCount - 1, 1].Text = ssView_Sheet1.Cells[i, 1].Text;
                ssViewPrint_Sheet1.Cells[ssViewPrint_Sheet1.RowCount - 1, 2].Text = ssView_Sheet1.Cells[i, 2].Text;
                ssViewPrint_Sheet1.Cells[ssViewPrint_Sheet1.RowCount - 1, 3].Text = ssView_Sheet1.Cells[i, 3].Text;
                ssViewPrint_Sheet1.Cells[ssViewPrint_Sheet1.RowCount - 1, 4].Text = ssView_Sheet1.Cells[i, 4].Text;
                ssViewPrint_Sheet1.Cells[ssViewPrint_Sheet1.RowCount - 1, 5].Text = ssView_Sheet1.Cells[i, 5].Text;
                ssViewPrint_Sheet1.Cells[ssViewPrint_Sheet1.RowCount - 1, 6].Text = ssView_Sheet1.Cells[i, 6].Text;
                ssViewPrint_Sheet1.Cells[ssViewPrint_Sheet1.RowCount - 1, 7].Text = ssView_Sheet1.Cells[i, 7].Text;
                ssViewPrint_Sheet1.Cells[ssViewPrint_Sheet1.RowCount - 1, 8].Text = ssView_Sheet1.Cells[i, 8].Text;
                ssViewPrint_Sheet1.Cells[ssViewPrint_Sheet1.RowCount - 1, 11].Text = ssView_Sheet1.Cells[i, 9].Text;
                ssViewPrint_Sheet1.Cells[ssViewPrint_Sheet1.RowCount - 1, 12].Text = ssView_Sheet1.Cells[i, 10].Text;
                ssViewPrint_Sheet1.Cells[ssViewPrint_Sheet1.RowCount - 1, 13].Text = ssView_Sheet1.Cells[i, 11].Text;
                ssViewPrint_Sheet1.Cells[ssViewPrint_Sheet1.RowCount - 1, 15].Text = ssView_Sheet1.Cells[i, 13].Text;
                ssViewPrint_Sheet1.Cells[ssViewPrint_Sheet1.RowCount - 1, 16].Text = ssView_Sheet1.Cells[i, 14].Text;
                //ssViewPrint_Sheet1.Cells[ssViewPrint_Sheet1.RowCount - 1, 17].Text = ssView_Sheet1.Cells[i, 15].Text; //19-05-30 의뢰서 관련 주석처리
                ssViewPrint_Sheet1.Cells[ssViewPrint_Sheet1.RowCount - 1, 16].Text = ssView_Sheet1.Cells[i, 16].Text;

                intPageCnt++;

                if (intPageCnt == (31 * intPage))  //19-05-30 의뢰서관련 작업 수정
                {
                    intPage++;
                    break;
                }
            }
            ssViewPrint_Sheet1.Cells[7, 0, ssViewPrint_Sheet1.RowCount - 2, ssViewPrint_Sheet1.ColumnCount - 3].Border = complexBorder0;
            ssViewPrint_Sheet1.Cells[7, ssViewPrint_Sheet1.ColumnCount - 2, ssViewPrint_Sheet1.RowCount - 2, ssViewPrint_Sheet1.ColumnCount - 2].Border = complexBorder1;
            ssViewPrint_Sheet1.Cells[ssViewPrint_Sheet1.RowCount - 1, 0, ssViewPrint_Sheet1.RowCount - 1, ssViewPrint_Sheet1.ColumnCount - 1].Border = complexBorder2;
            ssViewPrint_Sheet1.Cells[ssViewPrint_Sheet1.RowCount - 1, ssViewPrint_Sheet1.ColumnCount - 2, ssViewPrint_Sheet1.RowCount - 1, ssViewPrint_Sheet1.ColumnCount - 2].Border = complexBorder3;

            ssViewPrint_Sheet1.Rows[0, 6].Height = 30F;
            ssViewPrint_Sheet1.Rows[7].Height = 44F;

            ssViewPrint_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssViewPrint_Sheet1.PrintInfo.ZoomFactor = 0.95f;
            ssViewPrint_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssViewPrint_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssViewPrint_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssViewPrint_Sheet1.PrintInfo.Header = strFont1 + strHead1;
            ssViewPrint_Sheet1.PrintInfo.Margin.Top = 100;
            ssViewPrint_Sheet1.PrintInfo.Margin.Header = 30;
            ssViewPrint_Sheet1.PrintInfo.Margin.Bottom = 30;
            ssViewPrint_Sheet1.PrintInfo.ShowColor = false;
            ssViewPrint_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssViewPrint_Sheet1.PrintInfo.ShowBorder = false;
            ssViewPrint_Sheet1.PrintInfo.ShowGrid = false;
            ssViewPrint_Sheet1.PrintInfo.ShowShadows = false;
            ssViewPrint_Sheet1.PrintInfo.UseMax = false;
            ssViewPrint_Sheet1.PrintInfo.UseSmartPrint = false;
            ssViewPrint_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssViewPrint_Sheet1.PrintInfo.Preview = false;
            ssViewPrint.PrintSheet(0);
            
            if (intPageCnt < ssView_Sheet1.RowCount)
            {
                goto Replay;
            }
        }

        private void SetPrt()
        {
            ssViewPrint = new FarPoint.Win.Spread.FpSpread();
            ssViewPrint_Sheet1 = new FarPoint.Win.Spread.SheetView();

            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color375636360789436660197", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text465636360789436840207", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Static562636360789436850208");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("Static598636360789436860208");
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle5 = new FarPoint.Win.Spread.NamedStyle("Color361636360791234713040", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle6 = new FarPoint.Win.Spread.NamedStyle("Text425636360791234723040", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle7 = new FarPoint.Win.Spread.NamedStyle("Static540636360791234753042");
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle8 = new FarPoint.Win.Spread.NamedStyle("Static612636360791234773043");
            FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle9 = new FarPoint.Win.Spread.NamedStyle("Static648636360791234793044");
            FarPoint.Win.Spread.CellType.TextCellType textCellType12 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance2 = new FarPoint.Win.Spread.TipAppearance();
            FarPoint.Win.Spread.CellType.TextCellType textCellType13 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType14 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType15 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType16 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType17 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType18 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType19 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType20 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType21 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType22 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType23 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType24 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType25 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType26 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType27 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType28 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType29 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder1 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType30 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder2 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType31 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder3 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType32 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder4 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType33 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder5 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType34 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder6 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType35 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder7 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType36 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder8 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType37 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder9 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType38 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder10 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType39 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder11 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType40 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder12 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType41 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder13 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType42 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder14 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType43 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder15 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType44 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder16 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType45 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder17 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType46 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder18 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType47 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder19 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType48 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder20 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType49 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder21 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType50 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder22 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType51 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder23 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType52 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder24 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType53 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder25 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType54 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder26 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType55 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder27 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType56 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder28 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType57 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder29 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType58 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder30 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType59 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder31 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType60 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder32 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType61 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder33 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType62 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder34 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType63 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder35 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType64 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder36 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType65 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder37 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType66 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder38 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType67 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder39 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType68 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder40 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType69 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder41 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType70 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder42 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType71 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder43 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType72 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder44 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType73 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder45 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType74 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder46 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType75 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder47 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType76 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder48 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType77 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder49 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType78 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder50 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType79 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder51 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType80 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder52 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType81 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder53 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType82 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder54 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType83 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder55 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType84 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder56 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType85 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder57 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType86 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder58 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType87 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder59 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType88 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder60 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType89 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder61 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType90 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder62 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType91 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder63 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType92 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder64 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType93 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder65 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType94 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder66 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType95 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder67 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType96 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder68 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType97 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder69 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType98 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder70 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType99 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder71 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType100 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder72 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType101 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder73 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType102 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder74 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType103 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder75 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType104 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder76 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType105 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder77 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType106 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder78 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType107 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder79 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType108 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder80 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType109 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder81 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType110 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder82 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType111 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder83 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType112 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder84 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType113 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder85 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType114 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder86 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType115 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder87 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType116 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder88 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType117 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder89 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType118 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder90 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType119 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder91 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType120 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder92 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType121 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder93 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType122 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder94 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType123 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder95 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType124 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder96 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType125 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder97 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType126 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder98 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType127 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder99 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType128 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder100 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType129 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder101 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType130 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder102 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType131 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder103 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType132 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder104 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType133 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder105 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType134 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder106 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType135 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder107 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType136 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder108 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType137 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder109 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType138 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder110 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType139 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder111 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType140 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder112 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType141 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder113 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType142 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder114 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType143 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder115 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType144 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder116 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType145 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder117 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType146 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder118 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType147 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder119 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType148 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder120 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType149 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder121 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType150 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder122 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType151 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder123 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType152 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder124 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType153 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder125 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType154 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder126 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType155 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder127 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType156 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder128 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType157 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder129 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType158 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder130 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType159 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder131 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType160 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder132 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType161 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder133 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType162 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder134 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType163 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder135 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType164 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder136 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType165 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder137 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType166 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder138 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType167 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder139 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType168 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder140 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType169 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder141 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType170 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder142 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType171 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder143 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType172 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder144 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType173 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder145 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder complexBorder146 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder complexBorder147 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder complexBorder148 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder complexBorder149 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder complexBorder150 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder complexBorder151 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder complexBorder152 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder complexBorder153 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder complexBorder154 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder complexBorder155 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder complexBorder156 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder complexBorder157 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder complexBorder158 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder complexBorder159 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder complexBorder160 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder complexBorder161 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder complexBorder162 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType174 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType175 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType176 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType177 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType178 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType179 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType180 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType181 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType182 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType183 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType184 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType185 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType186 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType187 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType188 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType189 = new FarPoint.Win.Spread.CellType.TextCellType();
            // 
            // ssViewPrint
            // 
            ssViewPrint.AccessibleDescription = "ssViewPrint, Sheet1, Row 0, Column 0, 일자:\r\n약품명 : 하나염산페치딘주사액 50mg/1ml/A\r\n구분\r\n약품코드\r\n단위\r" +
    "\n비치수량\r\n차수";
            ssViewPrint.Location = new System.Drawing.Point(100, 300);
            ssViewPrint.Name = "ssViewPrint";
            ssViewPrint.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            ssViewPrint_Sheet1});
            ssViewPrint.Size = new System.Drawing.Size(800, 200);
            ssViewPrint.TabIndex = 5;
            ssViewPrint.Visible = false;
            // 
            // ssViewPrint_Sheet1
            // 
            ssViewPrint_Sheet1.Reset();
            ssViewPrint_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            ssViewPrint_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            ssViewPrint_Sheet1.ColumnCount = 18;
            ssViewPrint_Sheet1.RowCount = 9;
            ssViewPrint_Sheet1.Cells.Get(0, 0).Border = complexBorder1;
            textCellType30.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(0, 0).CellType = textCellType30;
            ssViewPrint_Sheet1.Cells.Get(0, 0).ColumnSpan = 9;
            ssViewPrint_Sheet1.Cells.Get(0, 0).Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(0, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            ssViewPrint_Sheet1.Cells.Get(0, 0).RowSpan = 7;
            ssViewPrint_Sheet1.Cells.Get(0, 0).Value = "일자:\r\n약품명 : 하나염산페치딘주사액 50mg/1ml/A\r\n구분\r\n약품코드\r\n단위\r\n비치수량\r\n차수";
            ssViewPrint_Sheet1.Cells.Get(0, 1).Border = complexBorder2;
            textCellType31.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(0, 1).CellType = textCellType31;
            ssViewPrint_Sheet1.Cells.Get(0, 1).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(0, 2).Border = complexBorder3;
            textCellType32.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(0, 2).CellType = textCellType32;
            ssViewPrint_Sheet1.Cells.Get(0, 2).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(0, 3).Border = complexBorder4;
            textCellType33.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(0, 3).CellType = textCellType33;
            ssViewPrint_Sheet1.Cells.Get(0, 3).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(0, 4).Border = complexBorder5;
            textCellType34.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(0, 4).CellType = textCellType34;
            ssViewPrint_Sheet1.Cells.Get(0, 4).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(0, 5).Border = complexBorder6;
            textCellType35.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(0, 5).CellType = textCellType35;
            ssViewPrint_Sheet1.Cells.Get(0, 5).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(0, 6).Border = complexBorder7;
            textCellType36.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(0, 6).CellType = textCellType36;
            ssViewPrint_Sheet1.Cells.Get(0, 6).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(0, 7).Border = complexBorder8;
            textCellType37.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(0, 7).CellType = textCellType37;
            ssViewPrint_Sheet1.Cells.Get(0, 7).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(0, 8).Border = complexBorder9;
            textCellType38.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(0, 8).CellType = textCellType38;
            ssViewPrint_Sheet1.Cells.Get(0, 8).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(0, 9).Border = complexBorder10;
            textCellType39.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(0, 9).CellType = textCellType39;
            ssViewPrint_Sheet1.Cells.Get(0, 9).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(0, 9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(0, 9).RowSpan = 6;
            ssViewPrint_Sheet1.Cells.Get(0, 9).Value = "확\r\n\r\n\r\n\r\n인";
            ssViewPrint_Sheet1.Cells.Get(0, 9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(0, 10).Border = complexBorder11;
            textCellType40.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(0, 10).CellType = textCellType40;
            ssViewPrint_Sheet1.Cells.Get(0, 10).ColumnSpan = 4;
            ssViewPrint_Sheet1.Cells.Get(0, 10).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(0, 10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(0, 10).RowSpan = 2;
            ssViewPrint_Sheet1.Cells.Get(0, 10).Value = "마약류관리보조자\r\n정 (   실   장   )";
            ssViewPrint_Sheet1.Cells.Get(0, 10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(0, 11).Border = complexBorder12;
            textCellType41.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(0, 11).CellType = textCellType41;
            ssViewPrint_Sheet1.Cells.Get(0, 11).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(0, 11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(0, 11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(0, 12).Border = complexBorder13;
            textCellType42.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(0, 12).CellType = textCellType42;
            ssViewPrint_Sheet1.Cells.Get(0, 12).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(0, 12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(0, 12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(0, 13).Border = complexBorder14;
            textCellType43.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(0, 13).CellType = textCellType43;
            ssViewPrint_Sheet1.Cells.Get(0, 13).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(0, 13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(0, 13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(0, 14).Border = complexBorder15;
            textCellType44.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(0, 14).CellType = textCellType44;
            ssViewPrint_Sheet1.Cells.Get(0, 14).ColumnSpan = 5; //19-5-30 의뢰서 관련 수정 (늘림)
            ssViewPrint_Sheet1.Cells.Get(0, 14).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(0, 14).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(0, 14).RowSpan = 2;
            ssViewPrint_Sheet1.Cells.Get(0, 14).Value = "마약류관리보조자\r\n부 (  수  간  호  사  )";
            ssViewPrint_Sheet1.Cells.Get(0, 14).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(0, 17).Border = complexBorder144;  //19-05-30 의뢰서 작업 주석처리
            textCellType45.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(0, 15).CellType = textCellType45;
            ssViewPrint_Sheet1.Cells.Get(0, 15).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(0, 15).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(0, 15).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //ssViewPrint_Sheet1.Cells.Get(0, 16).Border = complexBorder17;
            textCellType46.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(0, 16).CellType = textCellType46;
            ssViewPrint_Sheet1.Cells.Get(0, 16).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(0, 16).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(0, 16).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(0, 17).Border = complexBorder18;
            textCellType47.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(0, 17).CellType = textCellType47;
            ssViewPrint_Sheet1.Cells.Get(0, 17).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(0, 17).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(0, 17).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(1, 0).Border = complexBorder19;
            textCellType48.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(1, 0).CellType = textCellType48;
            ssViewPrint_Sheet1.Cells.Get(1, 0).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(1, 1).Border = complexBorder20;
            textCellType49.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(1, 1).CellType = textCellType49;
            ssViewPrint_Sheet1.Cells.Get(1, 1).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(1, 2).Border = complexBorder21;
            textCellType50.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(1, 2).CellType = textCellType50;
            ssViewPrint_Sheet1.Cells.Get(1, 2).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(1, 3).Border = complexBorder22;
            textCellType51.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(1, 3).CellType = textCellType51;
            ssViewPrint_Sheet1.Cells.Get(1, 3).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(1, 4).Border = complexBorder23;
            textCellType52.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(1, 4).CellType = textCellType52;
            ssViewPrint_Sheet1.Cells.Get(1, 4).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(1, 5).Border = complexBorder24;
            textCellType53.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(1, 5).CellType = textCellType53;
            ssViewPrint_Sheet1.Cells.Get(1, 5).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(1, 6).Border = complexBorder25;
            textCellType54.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(1, 6).CellType = textCellType54;
            ssViewPrint_Sheet1.Cells.Get(1, 6).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(1, 7).Border = complexBorder26;
            textCellType55.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(1, 7).CellType = textCellType55;
            ssViewPrint_Sheet1.Cells.Get(1, 7).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(1, 8).Border = complexBorder27;
            textCellType56.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(1, 8).CellType = textCellType56;
            ssViewPrint_Sheet1.Cells.Get(1, 8).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(1, 9).Border = complexBorder28;
            textCellType57.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(1, 9).CellType = textCellType57;
            ssViewPrint_Sheet1.Cells.Get(1, 9).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(1, 9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(1, 9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(1, 10).Border = complexBorder29;
            textCellType58.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(1, 10).CellType = textCellType58;
            ssViewPrint_Sheet1.Cells.Get(1, 10).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(1, 10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(1, 10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(1, 11).Border = complexBorder30;
            textCellType59.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(1, 11).CellType = textCellType59;
            ssViewPrint_Sheet1.Cells.Get(1, 11).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(1, 11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(1, 11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(1, 12).Border = complexBorder31;
            textCellType60.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(1, 12).CellType = textCellType60;
            ssViewPrint_Sheet1.Cells.Get(1, 12).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(1, 12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(1, 12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(1, 13).Border = complexBorder32;
            textCellType61.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(1, 13).CellType = textCellType61;
            ssViewPrint_Sheet1.Cells.Get(1, 13).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(1, 13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(1, 13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(1, 14).Border = complexBorder33;
            textCellType62.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(1, 14).CellType = textCellType62;
            ssViewPrint_Sheet1.Cells.Get(1, 14).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(1, 14).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(1, 14).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(1, 15).Border = complexBorder34;
            textCellType63.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(1, 15).CellType = textCellType63;
            ssViewPrint_Sheet1.Cells.Get(1, 15).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(1, 15).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(1, 15).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(1, 16).Border = complexBorder35;
            textCellType64.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(1, 16).CellType = textCellType64;
            ssViewPrint_Sheet1.Cells.Get(1, 16).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(1, 16).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(1, 16).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(1, 17).Border = complexBorder36;
            textCellType65.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(1, 17).CellType = textCellType65;
            ssViewPrint_Sheet1.Cells.Get(1, 17).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(1, 17).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(1, 17).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(2, 0).Border = complexBorder37;
            textCellType66.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(2, 0).CellType = textCellType66;
            ssViewPrint_Sheet1.Cells.Get(2, 0).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(2, 1).Border = complexBorder38;
            textCellType67.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(2, 1).CellType = textCellType67;
            ssViewPrint_Sheet1.Cells.Get(2, 1).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(2, 2).Border = complexBorder39;
            textCellType68.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(2, 2).CellType = textCellType68;
            ssViewPrint_Sheet1.Cells.Get(2, 2).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(2, 3).Border = complexBorder40;
            textCellType69.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(2, 3).CellType = textCellType69;
            ssViewPrint_Sheet1.Cells.Get(2, 3).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(2, 4).Border = complexBorder41;
            textCellType70.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(2, 4).CellType = textCellType70;
            ssViewPrint_Sheet1.Cells.Get(2, 4).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(2, 5).Border = complexBorder42;
            textCellType71.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(2, 5).CellType = textCellType71;
            ssViewPrint_Sheet1.Cells.Get(2, 5).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(2, 6).Border = complexBorder43;
            textCellType72.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(2, 6).CellType = textCellType72;
            ssViewPrint_Sheet1.Cells.Get(2, 6).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(2, 7).Border = complexBorder44;
            textCellType73.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(2, 7).CellType = textCellType73;
            ssViewPrint_Sheet1.Cells.Get(2, 7).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(2, 8).Border = complexBorder45;
            textCellType74.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(2, 8).CellType = textCellType74;
            ssViewPrint_Sheet1.Cells.Get(2, 8).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(2, 9).Border = complexBorder46;
            textCellType75.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(2, 9).CellType = textCellType75;
            ssViewPrint_Sheet1.Cells.Get(2, 9).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(2, 9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(2, 9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(2, 10).Border = complexBorder47;
            textCellType76.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(2, 10).CellType = textCellType76;
            ssViewPrint_Sheet1.Cells.Get(2, 10).ColumnSpan = 4;
            ssViewPrint_Sheet1.Cells.Get(2, 10).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(2, 10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(2, 10).RowSpan = 4;
            ssViewPrint_Sheet1.Cells.Get(2, 10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(2, 11).Border = complexBorder48;
            textCellType77.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(2, 11).CellType = textCellType77;
            ssViewPrint_Sheet1.Cells.Get(2, 11).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(2, 11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(2, 11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(2, 12).Border = complexBorder49;
            textCellType78.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(2, 12).CellType = textCellType78;
            ssViewPrint_Sheet1.Cells.Get(2, 12).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(2, 12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(2, 12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(2, 13).Border = complexBorder50;
            textCellType79.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(2, 13).CellType = textCellType79;
            ssViewPrint_Sheet1.Cells.Get(2, 13).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(2, 13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(2, 13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(2, 14).Border = complexBorder51;
            textCellType80.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(2, 14).CellType = textCellType80;
            ssViewPrint_Sheet1.Cells.Get(2, 14).ColumnSpan = 4;
            ssViewPrint_Sheet1.Cells.Get(2, 14).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(2, 14).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(2, 14).RowSpan = 4;
            ssViewPrint_Sheet1.Cells.Get(2, 14).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(2, 15).Border = complexBorder52;
            textCellType81.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(2, 15).CellType = textCellType81;
            ssViewPrint_Sheet1.Cells.Get(2, 15).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(2, 15).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(2, 15).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(2, 16).Border = complexBorder53;
            textCellType82.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(2, 16).CellType = textCellType82;
            ssViewPrint_Sheet1.Cells.Get(2, 16).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(2, 16).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(2, 16).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(2, 17).Border = complexBorder54;
            textCellType83.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(2, 17).CellType = textCellType83;
            ssViewPrint_Sheet1.Cells.Get(2, 17).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(2, 17).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(2, 17).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(3, 0).Border = complexBorder55;
            textCellType84.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(3, 0).CellType = textCellType84;
            ssViewPrint_Sheet1.Cells.Get(3, 0).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(3, 1).Border = complexBorder56;
            textCellType85.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(3, 1).CellType = textCellType85;
            ssViewPrint_Sheet1.Cells.Get(3, 1).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(3, 2).Border = complexBorder57;
            textCellType86.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(3, 2).CellType = textCellType86;
            ssViewPrint_Sheet1.Cells.Get(3, 2).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(3, 3).Border = complexBorder58;
            textCellType87.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(3, 3).CellType = textCellType87;
            ssViewPrint_Sheet1.Cells.Get(3, 3).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(3, 4).Border = complexBorder59;
            textCellType88.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(3, 4).CellType = textCellType88;
            ssViewPrint_Sheet1.Cells.Get(3, 4).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(3, 5).Border = complexBorder60;
            textCellType89.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(3, 5).CellType = textCellType89;
            ssViewPrint_Sheet1.Cells.Get(3, 5).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(3, 6).Border = complexBorder61;
            textCellType90.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(3, 6).CellType = textCellType90;
            ssViewPrint_Sheet1.Cells.Get(3, 6).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(3, 7).Border = complexBorder62;
            textCellType91.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(3, 7).CellType = textCellType91;
            ssViewPrint_Sheet1.Cells.Get(3, 7).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(3, 8).Border = complexBorder63;
            textCellType92.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(3, 8).CellType = textCellType92;
            ssViewPrint_Sheet1.Cells.Get(3, 8).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(3, 9).Border = complexBorder64;
            textCellType93.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(3, 9).CellType = textCellType93;
            ssViewPrint_Sheet1.Cells.Get(3, 9).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(3, 9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(3, 9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(3, 10).Border = complexBorder65;
            textCellType94.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(3, 10).CellType = textCellType94;
            ssViewPrint_Sheet1.Cells.Get(3, 10).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(3, 10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(3, 10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(3, 11).Border = complexBorder66;
            textCellType95.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(3, 11).CellType = textCellType95;
            ssViewPrint_Sheet1.Cells.Get(3, 11).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(3, 11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(3, 11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(3, 12).Border = complexBorder67;
            textCellType96.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(3, 12).CellType = textCellType96;
            ssViewPrint_Sheet1.Cells.Get(3, 12).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(3, 12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(3, 12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(3, 13).Border = complexBorder68;
            textCellType97.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(3, 13).CellType = textCellType97;
            ssViewPrint_Sheet1.Cells.Get(3, 13).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(3, 13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(3, 13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(3, 14).Border = complexBorder69;
            textCellType98.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(3, 14).CellType = textCellType98;
            ssViewPrint_Sheet1.Cells.Get(3, 14).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(3, 14).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(3, 14).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(3, 15).Border = complexBorder70;
            textCellType99.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(3, 15).CellType = textCellType99;
            ssViewPrint_Sheet1.Cells.Get(3, 15).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(3, 15).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(3, 15).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(3, 16).Border = complexBorder71;
            textCellType100.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(3, 16).CellType = textCellType100;
            ssViewPrint_Sheet1.Cells.Get(3, 16).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(3, 16).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(3, 16).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(3, 17).Border = complexBorder72;
            textCellType101.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(3, 17).CellType = textCellType101;
            ssViewPrint_Sheet1.Cells.Get(3, 17).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(3, 17).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(3, 17).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(4, 0).Border = complexBorder73;
            textCellType102.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(4, 0).CellType = textCellType102;
            ssViewPrint_Sheet1.Cells.Get(4, 0).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(4, 1).Border = complexBorder74;
            textCellType103.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(4, 1).CellType = textCellType103;
            ssViewPrint_Sheet1.Cells.Get(4, 1).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(4, 2).Border = complexBorder75;
            textCellType104.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(4, 2).CellType = textCellType104;
            ssViewPrint_Sheet1.Cells.Get(4, 2).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(4, 3).Border = complexBorder76;
            textCellType105.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(4, 3).CellType = textCellType105;
            ssViewPrint_Sheet1.Cells.Get(4, 3).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(4, 4).Border = complexBorder77;
            textCellType106.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(4, 4).CellType = textCellType106;
            ssViewPrint_Sheet1.Cells.Get(4, 4).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(4, 5).Border = complexBorder78;
            textCellType107.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(4, 5).CellType = textCellType107;
            ssViewPrint_Sheet1.Cells.Get(4, 5).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(4, 6).Border = complexBorder79;
            textCellType108.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(4, 6).CellType = textCellType108;
            ssViewPrint_Sheet1.Cells.Get(4, 6).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(4, 7).Border = complexBorder80;
            textCellType109.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(4, 7).CellType = textCellType109;
            ssViewPrint_Sheet1.Cells.Get(4, 7).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(4, 8).Border = complexBorder81;
            textCellType110.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(4, 8).CellType = textCellType110;
            ssViewPrint_Sheet1.Cells.Get(4, 8).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(4, 9).Border = complexBorder82;
            textCellType111.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(4, 9).CellType = textCellType111;
            ssViewPrint_Sheet1.Cells.Get(4, 9).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(4, 9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(4, 9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(4, 10).Border = complexBorder83;
            textCellType112.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(4, 10).CellType = textCellType112;
            ssViewPrint_Sheet1.Cells.Get(4, 10).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(4, 10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(4, 10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(4, 11).Border = complexBorder84;
            textCellType113.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(4, 11).CellType = textCellType113;
            ssViewPrint_Sheet1.Cells.Get(4, 11).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(4, 11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(4, 11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(4, 12).Border = complexBorder85;
            textCellType114.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(4, 12).CellType = textCellType114;
            ssViewPrint_Sheet1.Cells.Get(4, 12).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(4, 12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(4, 12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(4, 13).Border = complexBorder86;
            textCellType115.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(4, 13).CellType = textCellType115;
            ssViewPrint_Sheet1.Cells.Get(4, 13).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(4, 13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(4, 13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(4, 14).Border = complexBorder87;
            textCellType116.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(4, 14).CellType = textCellType116;
            ssViewPrint_Sheet1.Cells.Get(4, 14).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(4, 14).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(4, 14).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(4, 15).Border = complexBorder88;
            textCellType117.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(4, 15).CellType = textCellType117;
            ssViewPrint_Sheet1.Cells.Get(4, 15).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(4, 15).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(4, 15).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(4, 16).Border = complexBorder89;
            textCellType118.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(4, 16).CellType = textCellType118;
            ssViewPrint_Sheet1.Cells.Get(4, 16).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(4, 16).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(4, 16).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(4, 17).Border = complexBorder90;
            textCellType119.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(4, 17).CellType = textCellType119;
            ssViewPrint_Sheet1.Cells.Get(4, 17).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(4, 17).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(4, 17).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(5, 0).Border = complexBorder91;
            textCellType120.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(5, 0).CellType = textCellType120;
            ssViewPrint_Sheet1.Cells.Get(5, 0).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(5, 1).Border = complexBorder92;
            textCellType121.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(5, 1).CellType = textCellType121;
            ssViewPrint_Sheet1.Cells.Get(5, 1).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(5, 2).Border = complexBorder93;
            textCellType122.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(5, 2).CellType = textCellType122;
            ssViewPrint_Sheet1.Cells.Get(5, 2).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(5, 3).Border = complexBorder94;
            textCellType123.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(5, 3).CellType = textCellType123;
            ssViewPrint_Sheet1.Cells.Get(5, 3).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(5, 4).Border = complexBorder95;
            textCellType124.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(5, 4).CellType = textCellType124;
            ssViewPrint_Sheet1.Cells.Get(5, 4).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(5, 5).Border = complexBorder96;
            textCellType125.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(5, 5).CellType = textCellType125;
            ssViewPrint_Sheet1.Cells.Get(5, 5).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(5, 6).Border = complexBorder97;
            textCellType126.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(5, 6).CellType = textCellType126;
            ssViewPrint_Sheet1.Cells.Get(5, 6).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(5, 7).Border = complexBorder98;
            textCellType127.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(5, 7).CellType = textCellType127;
            ssViewPrint_Sheet1.Cells.Get(5, 7).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(5, 8).Border = complexBorder99;
            textCellType128.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(5, 8).CellType = textCellType128;
            ssViewPrint_Sheet1.Cells.Get(5, 8).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(5, 9).Border = complexBorder100;
            textCellType129.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(5, 9).CellType = textCellType129;
            ssViewPrint_Sheet1.Cells.Get(5, 9).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(5, 9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(5, 9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(5, 10).Border = complexBorder101;
            textCellType130.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(5, 10).CellType = textCellType130;
            ssViewPrint_Sheet1.Cells.Get(5, 10).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(5, 10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(5, 10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(5, 11).Border = complexBorder102;
            textCellType131.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(5, 11).CellType = textCellType131;
            ssViewPrint_Sheet1.Cells.Get(5, 11).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(5, 11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(5, 11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(5, 12).Border = complexBorder103;
            textCellType132.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(5, 12).CellType = textCellType132;
            ssViewPrint_Sheet1.Cells.Get(5, 12).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(5, 12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(5, 12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(5, 13).Border = complexBorder104;
            textCellType133.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(5, 13).CellType = textCellType133;
            ssViewPrint_Sheet1.Cells.Get(5, 13).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(5, 13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(5, 13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(5, 14).Border = complexBorder105;
            textCellType134.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(5, 14).CellType = textCellType134;
            ssViewPrint_Sheet1.Cells.Get(5, 14).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(5, 14).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(5, 14).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(5, 15).Border = complexBorder106;
            textCellType135.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(5, 15).CellType = textCellType135;
            ssViewPrint_Sheet1.Cells.Get(5, 15).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(5, 15).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(5, 15).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(5, 16).Border = complexBorder107;
            textCellType136.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(5, 16).CellType = textCellType136;
            ssViewPrint_Sheet1.Cells.Get(5, 16).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(5, 16).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(5, 16).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(5, 17).Border = complexBorder108;
            textCellType137.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(5, 17).CellType = textCellType137;
            ssViewPrint_Sheet1.Cells.Get(5, 17).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(5, 17).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(5, 17).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(6, 0).Border = complexBorder109;
            textCellType138.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(6, 0).CellType = textCellType138;
            ssViewPrint_Sheet1.Cells.Get(6, 0).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(6, 1).Border = complexBorder110;
            textCellType139.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(6, 1).CellType = textCellType139;
            ssViewPrint_Sheet1.Cells.Get(6, 1).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(6, 2).Border = complexBorder111;
            textCellType140.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(6, 2).CellType = textCellType140;
            ssViewPrint_Sheet1.Cells.Get(6, 2).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(6, 3).Border = complexBorder112;
            textCellType141.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(6, 3).CellType = textCellType141;
            ssViewPrint_Sheet1.Cells.Get(6, 3).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(6, 4).Border = complexBorder113;
            textCellType142.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(6, 4).CellType = textCellType142;
            ssViewPrint_Sheet1.Cells.Get(6, 4).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(6, 5).Border = complexBorder114;
            textCellType143.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(6, 5).CellType = textCellType143;
            ssViewPrint_Sheet1.Cells.Get(6, 5).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(6, 6).Border = complexBorder115;
            textCellType144.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(6, 6).CellType = textCellType144;
            ssViewPrint_Sheet1.Cells.Get(6, 6).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(6, 7).Border = complexBorder116;
            textCellType145.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(6, 7).CellType = textCellType145;
            ssViewPrint_Sheet1.Cells.Get(6, 7).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(6, 8).Border = complexBorder117;
            textCellType146.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(6, 8).CellType = textCellType146;
            ssViewPrint_Sheet1.Cells.Get(6, 8).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(6, 9).Border = complexBorder118;
            textCellType147.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(6, 9).CellType = textCellType147;
            ssViewPrint_Sheet1.Cells.Get(6, 9).ColumnSpan = 9;
            ssViewPrint_Sheet1.Cells.Get(6, 9).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(6, 10).Border = complexBorder119;
            textCellType148.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(6, 10).CellType = textCellType148;
            ssViewPrint_Sheet1.Cells.Get(6, 10).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(6, 11).Border = complexBorder120;
            textCellType149.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(6, 11).CellType = textCellType149;
            ssViewPrint_Sheet1.Cells.Get(6, 11).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(6, 12).Border = complexBorder121;
            textCellType150.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(6, 12).CellType = textCellType150;
            ssViewPrint_Sheet1.Cells.Get(6, 12).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(6, 13).Border = complexBorder122;
            textCellType151.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(6, 13).CellType = textCellType151;
            ssViewPrint_Sheet1.Cells.Get(6, 13).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(6, 14).Border = complexBorder123;
            textCellType152.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(6, 14).CellType = textCellType152;
            ssViewPrint_Sheet1.Cells.Get(6, 14).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(6, 15).Border = complexBorder124;
            textCellType153.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(6, 15).CellType = textCellType153;
            ssViewPrint_Sheet1.Cells.Get(6, 15).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(6, 16).Border = complexBorder125;
            textCellType154.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(6, 16).CellType = textCellType154;
            ssViewPrint_Sheet1.Cells.Get(6, 16).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(6, 17).Border = complexBorder126;
            textCellType155.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(6, 17).CellType = textCellType155;
            ssViewPrint_Sheet1.Cells.Get(6, 17).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(7, 0).Border = complexBorder127;
            textCellType156.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(7, 0).CellType = textCellType156;
            ssViewPrint_Sheet1.Cells.Get(7, 0).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(7, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(7, 0).Value = "순번";
            ssViewPrint_Sheet1.Cells.Get(7, 1).Border = complexBorder128;
            textCellType157.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(7, 1).CellType = textCellType157;
            ssViewPrint_Sheet1.Cells.Get(7, 1).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(7, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(7, 1).Value = "구분";
            ssViewPrint_Sheet1.Cells.Get(7, 2).Border = complexBorder129;
            textCellType158.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(7, 2).CellType = textCellType158;
            ssViewPrint_Sheet1.Cells.Get(7, 2).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(7, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(7, 2).Value = "처방일자";
            ssViewPrint_Sheet1.Cells.Get(7, 3).Border = complexBorder130;
            textCellType159.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(7, 3).CellType = textCellType159;
            ssViewPrint_Sheet1.Cells.Get(7, 3).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(7, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(7, 3).Value = GstrWardCode.Equals("EN") || GstrWardCode.Equals("TO") ? "반환량" : "입고량"; //19-05-30 의뢰서 관련 작업
            ssViewPrint_Sheet1.Cells.Get(7, 4).Border = complexBorder131;
            textCellType160.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(7, 4).CellType = textCellType160;
            ssViewPrint_Sheet1.Cells.Get(7, 4).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(7, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(7, 4).Value = "진료과";
            ssViewPrint_Sheet1.Cells.Get(7, 5).Border = complexBorder132;
            textCellType161.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(7, 5).CellType = textCellType161;
            ssViewPrint_Sheet1.Cells.Get(7, 5).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(7, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(7, 5).Value = "병실";
            ssViewPrint_Sheet1.Cells.Get(7, 6).Border = complexBorder133;
            textCellType162.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(7, 6).CellType = textCellType162;
            ssViewPrint_Sheet1.Cells.Get(7, 6).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(7, 6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(7, 6).Value = "환자명";
            ssViewPrint_Sheet1.Cells.Get(7, 7).Border = complexBorder134;
            textCellType163.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(7, 7).CellType = textCellType163;
            ssViewPrint_Sheet1.Cells.Get(7, 7).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(7, 7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(7, 7).Value = "등록번호";
            ssViewPrint_Sheet1.Cells.Get(7, 8).Border = complexBorder135;
            textCellType164.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(7, 8).CellType = textCellType164;
            ssViewPrint_Sheet1.Cells.Get(7, 8).ColumnSpan = 3;
            ssViewPrint_Sheet1.Cells.Get(7, 8).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(7, 8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(7, 8).Value = "수가코드";
            ssViewPrint_Sheet1.Cells.Get(7, 9).Border = complexBorder136;
            textCellType165.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(7, 9).CellType = textCellType165;
            ssViewPrint_Sheet1.Cells.Get(7, 9).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(7, 9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(7, 10).Border = complexBorder137;
            textCellType166.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(7, 10).CellType = textCellType166;
            ssViewPrint_Sheet1.Cells.Get(7, 10).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(7, 10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(7, 11).Border = complexBorder138;
            textCellType167.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(7, 11).CellType = textCellType167;
            ssViewPrint_Sheet1.Cells.Get(7, 11).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(7, 11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(7, 11).Value = "처방량";
            ssViewPrint_Sheet1.Cells.Get(7, 12).Border = complexBorder139;
            textCellType168.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(7, 12).CellType = textCellType168;
            ssViewPrint_Sheet1.Cells.Get(7, 12).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(7, 12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(7, 12).Value = "불출량";
            ssViewPrint_Sheet1.Cells.Get(7, 13).Border = complexBorder140;
            textCellType169.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(7, 13).CellType = textCellType169;
            ssViewPrint_Sheet1.Cells.Get(7, 13).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(7, 13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(7, 13).Value = "재고량";
            ssViewPrint_Sheet1.Cells.Get(7, 14).Border = complexBorder141;
            textCellType170.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(7, 14).CellType = textCellType170;
            ssViewPrint_Sheet1.Cells.Get(7, 14).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(7, 15).Border = complexBorder142;
            textCellType171.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(7, 15).CellType = textCellType171;
            ssViewPrint_Sheet1.Cells.Get(7, 15).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(7, 15).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(7, 15).Value = "잔여량\r\n발생";
            ssViewPrint_Sheet1.Cells.Get(7, 16).Border = complexBorder143;
            textCellType172.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(7, 16).CellType = textCellType172;
            ssViewPrint_Sheet1.Cells.Get(7, 16).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(7, 16).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(7, 16).Value = "응급센타\r\n확인";            
            ssViewPrint_Sheet1.Cells.Get(7, 16).Border = complexBorder144;
            textCellType173.Multiline = true;
            ssViewPrint_Sheet1.Cells.Get(7, 17).CellType = textCellType173;
            ssViewPrint_Sheet1.Cells.Get(7, 17).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Cells.Get(7, 17).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Cells.Get(7, 17).Value = "약제팀\r\n확인";

            ssViewPrint_Sheet1.Cells.Get(8, 0).Border = complexBorder145;
            ssViewPrint_Sheet1.Cells.Get(8, 1).Border = complexBorder146;
            ssViewPrint_Sheet1.Cells.Get(8, 2).Border = complexBorder147;
            ssViewPrint_Sheet1.Cells.Get(8, 3).Border = complexBorder148;
            ssViewPrint_Sheet1.Cells.Get(8, 4).Border = complexBorder149;
            ssViewPrint_Sheet1.Cells.Get(8, 5).Border = complexBorder150;
            ssViewPrint_Sheet1.Cells.Get(8, 6).Border = complexBorder151;
            ssViewPrint_Sheet1.Cells.Get(8, 7).Border = complexBorder152;
            ssViewPrint_Sheet1.Cells.Get(8, 8).Border = complexBorder153;
            ssViewPrint_Sheet1.Cells.Get(8, 8).ColumnSpan = 3;
            ssViewPrint_Sheet1.Cells.Get(8, 9).Border = complexBorder154;
            ssViewPrint_Sheet1.Cells.Get(8, 10).Border = complexBorder155;
            ssViewPrint_Sheet1.Cells.Get(8, 11).Border = complexBorder156;
            ssViewPrint_Sheet1.Cells.Get(8, 12).Border = complexBorder157;
            ssViewPrint_Sheet1.Cells.Get(8, 13).Border = complexBorder158;
            ssViewPrint_Sheet1.Cells.Get(8, 14).Border = complexBorder159;
            ssViewPrint_Sheet1.Cells.Get(8, 15).Border = complexBorder160;
            ssViewPrint_Sheet1.Cells.Get(8, 16).Border = complexBorder161;
            ssViewPrint_Sheet1.Cells.Get(8, 17).Border = complexBorder162;
            ssViewPrint_Sheet1.ColumnHeader.Visible = false;
            ssViewPrint_Sheet1.Columns.Get(0).CellType = textCellType174;
            ssViewPrint_Sheet1.Columns.Get(0).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Columns.Get(0).Width = 30F;
            ssViewPrint_Sheet1.Columns.Get(1).CellType = textCellType175;
            ssViewPrint_Sheet1.Columns.Get(1).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Columns.Get(1).Width = 75F;
            ssViewPrint_Sheet1.Columns.Get(2).CellType = textCellType176;
            ssViewPrint_Sheet1.Columns.Get(2).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Columns.Get(2).Width = 75F;
            ssViewPrint_Sheet1.Columns.Get(3).CellType = textCellType177;
            ssViewPrint_Sheet1.Columns.Get(3).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Columns.Get(3).Width = 45F;
            ssViewPrint_Sheet1.Columns.Get(4).CellType = textCellType178;
            ssViewPrint_Sheet1.Columns.Get(4).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Columns.Get(4).Width = 40F;
            ssViewPrint_Sheet1.Columns.Get(5).CellType = textCellType179;
            ssViewPrint_Sheet1.Columns.Get(5).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Columns.Get(5).Width = 45F;
            ssViewPrint_Sheet1.Columns.Get(6).CellType = textCellType180;
            ssViewPrint_Sheet1.Columns.Get(6).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Columns.Get(6).Width = 70F;
            ssViewPrint_Sheet1.Columns.Get(7).CellType = textCellType181;
            ssViewPrint_Sheet1.Columns.Get(7).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Columns.Get(7).Width = 70F;
            ssViewPrint_Sheet1.Columns.Get(8).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Columns.Get(8).Width = 25F;
            ssViewPrint_Sheet1.Columns.Get(9).CellType = textCellType182;
            ssViewPrint_Sheet1.Columns.Get(9).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Columns.Get(9).Width = 30F;
            ssViewPrint_Sheet1.Columns.Get(10).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Columns.Get(10).Width = 25F;
            ssViewPrint_Sheet1.Columns.Get(11).CellType = textCellType183;
            ssViewPrint_Sheet1.Columns.Get(11).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Columns.Get(11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            ssViewPrint_Sheet1.Columns.Get(11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Columns.Get(11).Width = 45F;
            ssViewPrint_Sheet1.Columns.Get(12).CellType = textCellType184;
            ssViewPrint_Sheet1.Columns.Get(12).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Columns.Get(12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            ssViewPrint_Sheet1.Columns.Get(12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Columns.Get(12).Width = 45F;
            ssViewPrint_Sheet1.Columns.Get(13).CellType = textCellType185;
            ssViewPrint_Sheet1.Columns.Get(13).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Columns.Get(13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            ssViewPrint_Sheet1.Columns.Get(13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Columns.Get(13).Width = 45F;
            ssViewPrint_Sheet1.Columns.Get(14).CellType = textCellType186;
            ssViewPrint_Sheet1.Columns.Get(14).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Columns.Get(14).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Columns.Get(14).Width = 3F;
            ssViewPrint_Sheet1.Columns.Get(15).CellType = textCellType187;
            ssViewPrint_Sheet1.Columns.Get(15).Font = new System.Drawing.Font("맑은 고딕", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Columns.Get(15).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            ssViewPrint_Sheet1.Columns.Get(15).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Columns.Get(15).Width = 45F;
            ssViewPrint_Sheet1.Columns.Get(16).CellType = textCellType188;
            ssViewPrint_Sheet1.Columns.Get(16).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Columns.Get(16).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Columns.Get(16).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Columns.Get(16).Width = 65F;
            ssViewPrint_Sheet1.Columns.Get(17).CellType = textCellType189;
            ssViewPrint_Sheet1.Columns.Get(17).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            ssViewPrint_Sheet1.Columns.Get(17).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Columns.Get(17).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.Columns.Get(17).Width = 45F;
            ssViewPrint_Sheet1.RowHeader.Columns.Default.Resizable = false;
            ssViewPrint_Sheet1.RowHeader.Visible = false;
            ssViewPrint_Sheet1.Rows.Get(7).Height = 40F;
            ssViewPrint_Sheet1.Rows.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewPrint_Sheet1.Rows.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssViewPrint_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
        }

        private string GetChasu()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT ENTDATE FROM " + ComNum.DB_MED + "OCS_DRUG ";
                SQL = SQL + ComNum.VBLF + " WHERE BUILDDATE = TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";

                if (rdoGB0.Checked == true) { SQL = SQL + ComNum.VBLF + "    AND GBN IN ( '1')  "; }//'  '마약
                if (rdoGB1.Checked == true) { SQL = SQL + ComNum.VBLF + "    AND GBN IN ( '2')  "; }//'  향정

                SQL = SQL + ComNum.VBLF + "    AND GBN2 IN ( '1')  ";
                SQL = SQL + ComNum.VBLF + "    AND WARDCODE ='" + GstrWardCode + "' "; //ICU
                SQL = SQL + ComNum.VBLF + "    AND SUCODE =  '" + txtDCode.Text + "' ";
                SQL = SQL + ComNum.VBLF + "    AND NO1 = '" + cboChasu.Text.Replace("차수", "").Trim() + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["ENTDATE"].ToString().Trim();
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private void btnBar_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            //string strPrtName = "";
            //string strWrittenData = "";
            //string strData = "";
            string strAge = "";
            string strSex = "";
            string strPano = "";
            string strJQty = "";
            string strBDate = "";
            string strSuCode = "";
            string strUnitNew3 = "";
            string strUnitNew4 = "";
            string strSuName = "";
            string strSname = "";
            //Dim MyDocInfo As DOCINFO
            //Dim nPrint  As Integer
            //Dim lhPrinter As Long
            //Dim lReturn As Long
            //Dim lpcWritten As Long
            //Dim lDoc As Long
            int i = 0;

            string mstrPrintName = "혈액환자정보";
            string strPrintName = string.Empty;

            using (clsPrint CP = new clsPrint())
            {
                strPrintName = CP.getPrinter_Chk(mstrPrintName.ToUpper());
            }

            if (strPrintName == "")
            {
                ComFunc.MsgBox("프린터 설정 오류입니다. 전산정보팀에 연락바랍니다.");
                return;
            }

            //if (clsPrint.gGetPrinterFind("혈액환자정보") == false)
            //{
            //    ComFunc.MsgBox("지정된 프린터를 찾을수 없습니다.", "혈액환자정보");
            //    return;
            //}

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    strBDate = ssView_Sheet1.Cells[i, 2].Text.Trim();
                    strSname = ssView_Sheet1.Cells[i, 6].Text.Trim();
                    strPano = ssView_Sheet1.Cells[i, 7].Text.Trim();
                    strAge = clsVbfunc.READ_AGE_GESAN(clsDB.DbCon, strPano);
                    strSex = clsVbfunc.READ_SEX(clsDB.DbCon, strPano);
                    strSuCode = ssView_Sheet1.Cells[i, 8].Text.Trim();
                    strJQty = ssView_Sheet1.Cells[i, 13].Text.Trim();

                    if (strJQty != "")
                    {
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     UNITNEW4, UNITNEW3, SUNAMEK";
                        SQL = SQL + ComNum.VBLF + "From " + ComNum.DB_PMPA + "BAS_SUN";
                        SQL = SQL + ComNum.VBLF + "     WHERE SUNEXT = '" + strSuCode + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            strUnitNew3 = dt.Rows[0]["UNITNEW3"].ToString().Trim();
                            strUnitNew4 = dt.Rows[0]["UNITNEW4"].ToString().Trim();
                            strSuName = dt.Rows[0]["SUNAMEK"].ToString().Trim();
                        }

                        dt.Dispose();
                        dt = null;

                        GstrLabelPrint1 = strPano + " " + strSname + " " + strSex + "/" + strAge;
                        GstrLabelPrint2 = "코  드 : " + strSuCode;
                        GstrLabelPrint3 = strSuName;
                        GstrLabelPrint4 = "잔여량 : " + strJQty.Trim() + strUnitNew3.Trim() + ", " + (VB.Val(strJQty)) * VB.Val(strUnitNew4.Trim()) + "ml";
                        GstrLabelPrint5 = "날  짜 : " + strBDate;

                        using (PrintDocument pd = new PrintDocument())
                        {
                            PrintController pc = new StandardPrintController();
                            pd.PrintController = pc;
                            pd.PrinterSettings.PrinterName = "혈액환자정보";
                            pd.PrinterSettings.DefaultPageSettings.PaperSize = new PaperSize("BARCODESIZE", 50, 30);

                            pd.PrintPage += new PrintPageEventHandler(eBarBARPrint);
                            pd.Print();    //프린트
                        }

                    }
                }
                
                Cursor.Current = Cursors.Default;
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
                Cursor.Current = Cursors.Default;
            }
        }

        private void eBarBARPrint(object sender, PrintPageEventArgs ev)
        {
            ev.Graphics.DrawString(GstrLabelPrint1, new Font("맑은 고딕", 10f, FontStyle.Bold), Brushes.Black, 7, 5, new StringFormat());
            ev.Graphics.DrawString(GstrLabelPrint2, new Font("맑은 고딕", 10f, FontStyle.Bold), Brushes.Black, 7, 22, new StringFormat());
            ev.Graphics.DrawString(GstrLabelPrint3, new Font("맑은 고딕", 8f, FontStyle.Bold), Brushes.Black, 30, 39, new StringFormat());
            ev.Graphics.DrawString(GstrLabelPrint4, new Font("맑은 고딕", 9f, FontStyle.Bold), Brushes.Black, 7, 56, new StringFormat());
            ev.Graphics.DrawString(GstrLabelPrint5, new Font("맑은 고딕", 9f, FontStyle.Bold), Brushes.Black, 7, 73, new StringFormat());
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            if (SaveSet() == true)
            {
                FormClear();
                btnSearch.Click += new EventHandler(btnSearch_Click);
            }
        }

        private bool SaveSet()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;

            bool rtnVal = false;

            string strSuCode = "";
            string strGBn = "";
            double dblQty = 0;

            strSuCode = txtDCode.Text.Trim();

            if (GstrWardCode == "TO")
            {
                dblQty = VB.Val(VB.Pstr(txtBQty.Text, "A", 1));
            }
            else if (GstrWardCode == "EN")
            {
                dblQty = VB.Val(txtDQty.Text);
            }

            if (strSuCode == "") { return rtnVal; }
            if (rdoGB0.Checked == true) { strGBn = "1"; }
            if (rdoGB1.Checked == true) { strGBn = "2"; }

            clsDB.setBeginTran(clsDB.DbCon);
            
            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     ROWID";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUG_SET";
                SQL = SQL + ComNum.VBLF + "     WHERE SUCODE = '" + strSuCode + "' ";
                SQL = SQL + ComNum.VBLF + "         AND WARDCODE = '" + GstrWardCode + "' ";
                SQL = SQL + ComNum.VBLF + "         AND JDATE = TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    SQL = "";
                    SQL = "UPDATE " + ComNum.DB_MED + "OCS_DRUG_SET";
                    SQL = SQL + ComNum.VBLF + "     SET";
                    SQL = SQL + ComNum.VBLF + "         QTY = '" + dblQty + "',";
                    SQL = SQL + ComNum.VBLF + "         BQTY = '" + dblQty + "A" + "' ";
                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt.Rows[0]["ROWID"].ToString().Trim() + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }
                else
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_DRUG_SET";
                    SQL = SQL + ComNum.VBLF + "     (JDATE, GBN, WARDCODE, SUCODE, BQTY, QTY, ENTDATE, UNIT)";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "         '" + strGBn + "',";
                    SQL = SQL + ComNum.VBLF + "         '" + GstrWardCode + "',";
                    SQL = SQL + ComNum.VBLF + "         '" + strSuCode + "',";
                    SQL = SQL + ComNum.VBLF + "         '" + dblQty + "A" + "',";
                    SQL = SQL + ComNum.VBLF + "         '" + dblQty + "',";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE,";
                    SQL = SQL + ComNum.VBLF + "         'A'";
                    SQL = SQL + ComNum.VBLF + "     )";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }

                dt.Dispose();
                dt = null;

                //당일재고 정보 갱신
                SQL = "";
                SQL = "UPDATE " + ComNum.DB_MED + "OCS_DRUG";
                SQL = SQL + ComNum.VBLF + "     SET";
                SQL = SQL + ComNum.VBLF + "         QTY =  '" + dblQty + "',";
                SQL = SQL + ComNum.VBLF + "         REALQTY = '" + dblQty + "' ";
                SQL = SQL + ComNum.VBLF + "WHERE BUILDDATE = TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "     AND GBN2 IN ('3')"; //재고
                SQL = SQL + ComNum.VBLF + "     AND WARDCODE = '" + GstrWardCode + "'";
                SQL = SQL + ComNum.VBLF + "     AND SUCODE ='" + strSuCode + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private void cboWardCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboWardCode.Text.Trim() != "")
            {
                GstrWardCode = VB.Right(cboWardCode.Text, 10).Trim();

                ssView_Sheet1.Columns[3].Label = GstrWardCode.Equals("EN") || GstrWardCode.Equals("TO") ? "반환량" : "입고량"; //19-05-30 의뢰서 관련 작업

                SetForm();
                FormClear();
            }
        }

        private void READ_MAGAM_YN()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            ComFunc.ReadSysDate(clsDB.DbCon);
            DateTime curDate = Convert.ToDateTime(clsPublic.GstrSysDate);
            
            string strDate = curDate.ToString("yyyy-MM-dd");
            string strDateB = curDate.AddDays(-1).ToString("yyyy-MM-dd");

            bool bolHuil = clsVbfunc.ChkDateHuIl(clsDB.DbCon, strDate);
            bool bolHuilY = clsVbfunc.ChkDateHuIl(clsDB.DbCon, strDateB);
            string strYoil = clsVbfunc.GetYoIl(strDate);

            ssMagam.ActiveSheet.Cells[0, 0].Text = clsPublic.GstrSysDate + " 마감여부";
            ssMagam.ActiveSheet.Cells[0, 2].Text = "X";
            ssMagam.ActiveSheet.Cells[0, 4].Text = "X";
            ssMagam.ActiveSheet.Cells[0, 6].Text = "X";

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT NO1 FROM KOSMOS_OCS.OCS_DRUG ";
            if (bolHuil == false && (strYoil != "토요일" || strYoil != "일요일"))
            {
                if (strYoil == "월요일")
                {
                    SQL = SQL + ComNum.VBLF + "WHERE MDATE >= TO_DATE('" + strDateB + " 12:00:01', 'YYYY-MM-DD HH24:MI:SS') ";
                    SQL = SQL + ComNum.VBLF + "  AND MDATE <= TO_DATE('" + strDate + " 16:00:00', 'YYYY-MM-DD HH24:MI:SS') ";
                }
                else
                {
                    if (strYoil == "토요일")
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE MDATE >= TO_DATE('" + strDateB + " 16:00:01', 'YYYY-MM-DD HH24:MI:SS') ";
                        SQL = SQL + ComNum.VBLF + "  AND MDATE <= TO_DATE('" + strDate + " 12:00:00', 'YYYY-MM-DD HH24:MI:SS') ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE MDATE >= TO_DATE('" + strDateB + " 16:00:01', 'YYYY-MM-DD HH24:MI:SS') ";
                        SQL = SQL + ComNum.VBLF + "  AND MDATE <= TO_DATE('" + strDate + " 16:00:00', 'YYYY-MM-DD HH24:MI:SS') ";
                    }
                }
            }
            else if (strYoil == "일요일")
            {
                SQL = SQL + ComNum.VBLF + "WHERE MDATE >=TO_DATE('" + strDateB + " 12:00:01','YYYY-MM-DD HH24:MI:SS') ";
                SQL = SQL + ComNum.VBLF + "  AND MDATE <=TO_DATE('" + strDate + " 12:00:00','YYYY-MM-DD HH24:MI:SS') ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "WHERE MDATE >= TO_DATE('" + strDateB + " 16:00:01', 'YYYY-MM-DD HH24:MI:SS') ";
            }
            SQL = SQL + ComNum.VBLF + "   AND WARDCODE = 'AN'";
            SQL = SQL + ComNum.VBLF + " GROUP BY NO1";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["NO1"].ToString().Trim() == "1")
                    {
                        ssMagam.ActiveSheet.Cells[0, 2].Text = "O";
                    }
                    else if (dt.Rows[i]["NO1"].ToString().Trim() == "2")
                    {
                        ssMagam.ActiveSheet.Cells[0, 4].Text = "O";
                    }
                    else if (dt.Rows[i]["NO1"].ToString().Trim() == "3")
                    {
                        ssMagam.ActiveSheet.Cells[0, 6].Text = "O";
                    }
                }
            }

            dt.Dispose();
            dt = null;
        }

        private void READ_MAGAM_NEW_YN()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            ComFunc.ReadSysDate(clsDB.DbCon);
            DateTime curDate = Convert.ToDateTime(clsPublic.GstrSysDate);

            string strDate = curDate.ToString("yyyyMMdd");
            
            ssMagam.ActiveSheet.Cells[0, 0].Text = clsPublic.GstrSysDate + " 마감여부";
            ssMagam.ActiveSheet.Cells[0, 2].Text = "X";
            ssMagam.ActiveSheet.Cells[0, 4].Text = "X";
            ssMagam.ActiveSheet.Cells[0, 6].Text = "X";

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT WARDCODE, CHASU              ";             
            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_DRUG_MAGAMLOG ";
            SQL = SQL + ComNum.VBLF + " WHERE BDATE = '" + strDate + "'    ";
            
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["CHASU"].ToString().Trim() == "1")
                    {
                        ssMagam.ActiveSheet.Cells[0, 2].Text = "O";
                    }
                    else if (dt.Rows[i]["CHASU"].ToString().Trim() == "2")
                    {
                        ssMagam.ActiveSheet.Cells[0, 4].Text = "O";
                    }
                    else if (dt.Rows[i]["CHASU"].ToString().Trim() == "3")
                    {
                        ssMagam.ActiveSheet.Cells[0, 6].Text = "O";
                    }
                }
            }

            dt.Dispose();
            dt = null;
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            string strBDate = dtpDate.Value.ToShortDateString();
            using (frmComSupDrugListMagamLog f = new frmComSupDrugListMagamLog(strBDate))
            {
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog(this);
            }
        }
    }
}
