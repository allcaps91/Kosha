using ComBase; //기본 클래스
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmMsymEntry : Form, MainFormMessage
    {
        /// <summary>
        /// Class Name      : ComLibB
        /// File Name       : 
        /// Description     : 
        /// Author          : 김효성
        /// Create Date     : 2017-01 -
        /// Update History  : 
        /// </summary>
        /// <history>  
        /// btnCodePrint_Click,btnExellPrint \ 유닉스 서버 연결 X 소스 및 버튼은 남겨두고 버튼 숨김
        /// 
        /// </history>
        /// <seealso cref= D:\psmh\basic\bucode\bucode.vbp\FrmMsymEntry" >> frmMsymEntry.cs 폼이름 재정의" />

        /// <summary>
        /// 이벤트를 전달할 경우
        /// <para name = "GstrSILLCodeD"></para>
        /// <para name = "GstrSILLCode"></para>
        /// <para name = "GstrSILLNameK"></para>
        /// <para name = "GstrSILLNameE"></para>
        /// <para name = "GnLogOutCNT"></para> 
        /// </summary>
        string fstrROWID = "";
        string GbSabun = "";
        string GSILLCodeD = "";
        string GSILLNameK = "";
        string GSILLNameE = "";
        int GintOutCNT = 0;

        /// <summary>
        /// 선택 텍스트 박스
        /// </summary>
        private TextBox lastDateText = null;
        #region //MainFormMessage
        public MainFormMessage mCallForm = null;
        public void MsgActivedForm(Form frm)
        {

        }
        public void MsgUnloadForm(Form frm)
        {

        }
        public void MsgFormClear()
        {

        }
        public void MsgSendPara(string strPara)
        {

        }
        #endregion

        public frmMsymEntry()
        {
            InitializeComponent();
        }

        public frmMsymEntry(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
        }

        public frmMsymEntry(string strSabunm, string strSILLCodeD, string strSILLNameK, string strSILLNameE, int intOutCNT)
        {
            InitializeComponent();

            GbSabun = strSabunm;
            GSILLCodeD = strSILLCodeD;
            GSILLNameK = strSILLNameK;
            GSILLNameE = strSILLNameE;
            GintOutCNT = intOutCNT;
        }

        private void frmMsymEntry_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "N", "Y"); //폼 기본값 세팅 등

            Font = new System.Drawing.Font("굴림", 9);

            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            SCREEN_CLEAR();

            cboClass.Items.Clear();
            cboClass.Items.Add("A.상병(대)분류");
            cboClass.Items.Add("B.상병(중)분류");
            cboClass.Items.Add("C.상병(소)분류");
            cboClass.Items.Add("1.KCD8차");
            cboClass.Items.Add("2.신생물 형태분류");
            cboClass.Items.Add("3.국제수술코드");
            cboClass.Items.Add("4.User Define");
            cboClass.Items.Add("5.기록실수술코드");
            cboClass.SelectedIndex = 3;

            cboViewClass.Items.Clear();
            cboViewClass.Items.Add("A.상병(대)분류");
            cboViewClass.Items.Add("B.상병(중)분류");
            cboViewClass.Items.Add("C.상병(소)분류");
            cboViewClass.Items.Add("1.상병코드(KCD8차적용)");
            cboViewClass.Items.Add("2.신생물 형태분류");
            cboViewClass.Items.Add("3.국제수술코드");
            cboViewClass.Items.Add("4.User Define");
            cboViewClass.Items.Add("5.기록실수술코드");
            cboViewClass.Items.Add("6.KCD6차만");
            cboViewClass.Items.Add("7.KCD7차만");
            cboViewClass.Items.Add("D.KCD8차만");
            cboViewClass.Items.Add("8.불완전상병");
            cboViewClass.Items.Add("9.삭제코드");
            cboViewClass.SelectedIndex = 10;

            cboSex.Items.Clear();
            cboSex.Items.Add(" ");
            cboSex.Items.Add("M");
            cboSex.Items.Add("F");
            cboSex.SelectedIndex = 0;

            btnSave.Enabled = true;

            //if (Convert.ToString(GbSabun) == "4349")
            //{
            //    ssView_Sheet1.Columns[5].Visible = false;
            //}
        }

        private void READ_BAS_ILLS_KCD6(string ArgGbn)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                SQL = "SELECT IllCode, ILLCODED, IllClass, IllNameK,";
                SQL = SQL + ComNum.VBLF + " IllNameE, DispHeader, SEX, NOUSE, ";
                SQL = SQL + ComNum.VBLF + " ILLUPCODE, TO_CHAR(DDATE, 'YYYY-MM-DD') DDATE, TO_CHAR(NoUseDate, 'YYYY-MM-DD') NoUseDate ,REPCODE";
                //SQL = SQL + ComNum.VBLF + " FROM BAS_ILLS ";
                //if (VB.Left(cboViewClass.Text, 1) == "D")
                //{
                //    SQL = SQL + ComNum.VBLF + " FROM TEMP_BAS_ILLS ";
                //}
                //else
                //{
                    SQL = SQL + ComNum.VBLF + " FROM BAS_ILLS ";
                //}
         
                switch (VB.Left(cboViewClass.Text, 1))
                {

                    case "6":
                        SQL = SQL + ComNum.VBLF + "WHERE IllClass='1' ";
                        SQL = SQL + ComNum.VBLF + "  AND KCD6 ='*' ";
                        break;
                    case "7":
                        SQL = SQL + ComNum.VBLF + "WHERE IllClass='1' ";
                        SQL = SQL + ComNum.VBLF + "  AND KCD7 ='*' ";
                        break;
                    case "D":
                        SQL = SQL + ComNum.VBLF + "WHERE IllClass='1' ";
                        SQL = SQL + ComNum.VBLF + "  AND KCD8 ='*' ";
                        break;
                    case "8":
                        SQL = SQL + ComNum.VBLF + "WHERE IllClass='1' ";
                        SQL = SQL + ComNum.VBLF + "  AND NOUSEDATE IS NOT NULL ";
                        break;
                    case "9":
                        SQL = SQL + ComNum.VBLF + "WHERE DDATE IS NOT NULL ";
                        break;
                    default:
                        SQL = SQL + ComNum.VBLF + "WHERE IllClass='" + VB.Left(cboViewClass.Text, 1) + "' ";
                        break;
                }
                

                if (chkV252View.Checked) SQL = SQL + ComNum.VBLF + " AND GBV252 = '*' ";
                if (chkV352View.Checked) SQL = SQL + ComNum.VBLF + " AND GBV352 = '*' ";

                if (chkER2.Checked) SQL = SQL + ComNum.VBLF + " AND GBER ='*' ";

                if (chkGbnVcode1.Checked) SQL = SQL + ComNum.VBLF + " AND GBVCODE1 ='*' ";


                if (chkGbnVcode2.Checked) SQL = SQL + ComNum.VBLF + " AND GBVCODE2 ='*' ";


                if (chkGbnVcode3.Checked) SQL = SQL + ComNum.VBLF + " AND GBVCODE3 ='*' ";

                if (ArgGbn != "ALL") SQL = SQL + ComNum.VBLF + " AND ILLCODE LIKE '" + ArgGbn + "%'  ";

                if (rdoSortCode.Checked == true)
                    SQL = SQL + ComNum.VBLF + "ORDER BY IllCodeD,illcode ";

                else if (rdoSortKR.Checked == true)
                    SQL = SQL + ComNum.VBLF + "ORDER BY IllNameK,IllCodeD,illcode ";

                else
                    SQL = SQL + ComNum.VBLF + "ORDER BY IllNameE,IllCodeD,illcode ";

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

                ssView_Sheet1.Rows.Count = 0;
                ssView_Sheet1.Rows.Count = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["illcodeD"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["illclass"].ToString().Trim();
                    if (dt.Rows[i]["Nouse"].ToString().Trim() != "")
                        ssView_Sheet1.Cells[i, 2].Text = "*" + dt.Rows[i]["ILLNAMEK"].ToString().Trim();
                    else
                        ssView_Sheet1.Cells[i, 2].Text = " " + dt.Rows[i]["ILLNAMEK"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = " " + dt.Rows[i]["IllnameE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = " " + dt.Rows[i]["DispHeader"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["illcode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["NoUseDate"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["REPCODE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                btnPrint.Enabled = true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

        }

        private string READ_BAS_ILL_Name(string ArgILLCODE, string ArgClass = "", string ArgGb = "")
        {
            string rtnVal = "";
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                rtnVal = "";
                SQL = " SELECT ILLNAMEK, ILLNAMEE FROM " + ComNum.VBLF + "BAS_ILLS ";
                SQL = SQL + ComNum.VBLF + " WHERE ILLCODE = '" + ArgILLCODE.Trim() + "' ";
                if (ArgClass != "")
                    SQL = SQL + ComNum.VBLF + "   AND ILLCLASS = '" + ArgClass + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    if (ArgGb == "2")
                    {
                        rtnVal = dt.Rows[0]["IllnameE"].ToString().Trim();
                        return rtnVal;
                    }
                    else
                    {
                        rtnVal = dt.Rows[0]["ILLNAMEK"].ToString().Trim();
                        return rtnVal;
                    }
                }
                else
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return rtnVal;
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private void SCREEN_CLEAR()
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            txtCode.Text = "";
            cboClass.SelectedIndex = cboClass.Items.Count > 3 ? 3 : -1;
            txtNameSpacing.Text = "0";
            txtNameK.Text = "";
            txtNameE.Text = "";
            txtDispHeader.Text = "";
            txtILLUP.Text = "";
            txtILLUPName.Text = "";
            TxtSDate.Text = "";
            TxtDDate.Text = "";
            txtILLCODED.Text = "";
            fstrROWID = "";
            TxtNoUseDate.Text = "";
            txtRepCode.Text = "";
            chkV252.Checked = false;
            chkGBVCode.Checked = false;
            chkGBVCode1.Checked = false;
            chkGBVCode2.Checked = false;
            chkGBVCode3.Checked = false;


            btnSave.Enabled = false;
            btnDelete.Enabled = false;
            btnCancel.Enabled = false;
        }

        private void Screen_Display(string ArgClass)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            txtCode.Text = txtCode.Text.Trim().ToUpper();

            if (txtCode.Text == "")
            {
                SCREEN_CLEAR();
                return;
            }
            if (ArgClass == "6" || ArgClass == "7" || ArgClass == "8")
                ArgClass = "1";

            try
            {
                //자료를 READ
                SQL = "SELECT ROWID,IllCode,IllClass,IllNameK,IllNameE,";
                SQL = SQL + ComNum.VBLF + "NameSpacing,DispHeader, ILLUPCODE ,SEX, NOUSE, INFECT, GUBUN , GBINFECT,REPCODE, ";
                SQL = SQL + ComNum.VBLF + " ILLCODED, KCD8 , TO_CHAR(SDATE, 'YYYY-MM-DD') SDATE, TO_CHAR(DDATE, 'YYYY-MM-DD') DDATE,  TO_CHAR(NoUseDate, 'YYYY-MM-DD') NoUseDate, GBV252 , GBVCODE, IPDETC, GBER, GBV352, GBVCODE1, GBVCODE2 , GBVCODE3";
                SQL = SQL + ComNum.VBLF + "  FROM BAS_ILLS ";
                SQL = SQL + ComNum.VBLF + " WHERE IllCode = '" + txtCode.Text.Replace("'", "`") + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ILLCLASS = '" + ArgClass + "' ";

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

                    ComFunc.ReadSysDate(clsDB.DbCon);
                    SCREEN_CLEAR();

                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnDelete.Enabled = true;
                    return;
                }
                fstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();

                switch (dt.Rows[0]["IllClass"].ToString().Trim())
                {
                    case "A":
                        cboClass.Text = "A.상병(대)분류";
                        break;
                    case "B":
                        cboClass.Text = "B.상병(중)분류";
                        break;
                    case "C":
                        cboClass.Text = "C.상병(소)분류";
                        break;
                    case "1":
                        cboClass.Text = "1.KCD8차계정";
                        break;
                    case "2":
                        cboClass.Text = "2.신생물 형태분류";
                        break;
                    case "3":
                        cboClass.Text = "3.국제수술코드";
                        break;
                    case "4":
                        cboClass.Text = "4.User Define";
                        break;
                    case "5":
                        cboClass.Text = "5.기록실수술코드";
                        break;
                    default:
                        cboClass.SelectedIndex = -1;
                        break;
                }

                txtNameSpacing.Text = dt.Rows[0]["NameSpacing"].ToString().Trim();
                txtNameK.Text = dt.Rows[0]["IllNameK"].ToString().Trim();
                txtNameE.Text = dt.Rows[0]["IllNameE"].ToString().Trim();
                txtDispHeader.Text = dt.Rows[0]["DispHeader"].ToString().Trim();
                txtILLUP.Text = dt.Rows[0]["ILLUPCODE"].ToString().Trim();
                txtILLUPName.Text = READ_BAS_ILL_Name(txtILLUP.Text);
                txtILLCODED.Text = dt.Rows[0]["ILLCODED"].ToString().Trim();
                txtGbInfect.Text = dt.Rows[0]["GBINFECT"].ToString().Trim();
                txtRepCode.Text = dt.Rows[0]["REPCODE"].ToString().Trim();
                if (dt.Rows[0]["SDATE"].ToString().Trim() != "")
                {
                    TxtSDate.Text = dt.Rows[0]["SDATE"].ToString().Trim();
                }
                else
                {
                    TxtSDate.Text = "";
                }

                if (dt.Rows[0]["DDATE"].ToString().Trim() != "")
                {
                    TxtDDate.Text = dt.Rows[0]["DDATE"].ToString().Trim();
                }
                else
                {
                    TxtDDate.Text = "";
                }

                if (dt.Rows[0]["NoUseDate"].ToString().Trim() != "")
                {
                    TxtNoUseDate.Text = dt.Rows[0]["NoUseDate"].ToString().Trim();
                }
                else
                {
                    TxtNoUseDate.Text = "";
                }

                cboSex.Text = dt.Rows[0]["SEX"].ToString().Trim();

                chkNoUse.Checked = dt.Rows[0]["NOUSE"].ToString().Trim().Equals("N");
                chkInFect.Checked = dt.Rows[0]["INFECT"].ToString().Trim().Equals("Y");
                chkGubun.Checked = dt.Rows[0]["GUBUN"].ToString().Trim().Equals("1");
                //chkKCD6.Checked = dt.Rows[0]["KCD6"].ToString().Trim().Equals("*");
                //기존 KCD6체크박스를 KCD8체크박스로 사용함(2020-12-30)
                chkKCD6.Checked = dt.Rows[0]["KCD8"].ToString().Trim().Equals("*");
                chkV252.Checked = dt.Rows[0]["GBV252"].ToString().Trim().Equals("*");
                chkV352.Checked       = dt.Rows[0]["GBV352"].ToString().Trim().Equals("*");//        '2018-10-24
                chkGBVCode.Checked = dt.Rows[0]["GBVCODE"].ToString().Trim().Equals("*");
                chkGBVCode1.Checked = dt.Rows[0]["GBVCODE1"].ToString().Trim().Equals("*"); 
                chkGBVCode2.Checked   = dt.Rows[0]["GBVCODE2"].ToString().Trim().Equals("*");
                chkGBVCode3.Checked   = dt.Rows[0]["GBVCODE3"].ToString().Trim().Equals("*");
                chkjusangbung.Checked = dt.Rows[0]["IPDETC"].ToString().Trim().Equals("Y");

                chkER.Checked = dt.Rows[0]["GBER"].ToString().Trim().Equals("*");

                Cursor.Current = Cursors.Default;

                dt.Dispose();
                dt = null;

                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                btnDelete.Enabled = true;

                cboClass.Focus();
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnSaveCancel_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
            txtCode.Focus();
        }

        private bool Delete()
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            clsDB.setBeginTran(clsDB.DbCon);


            try
            {
                if (ComFunc.MsgBoxQ("정말로 삭제를 하시겠습니까?", "삭제여부", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
                    return false;

                SQL = "DELETE BAS_ILLS WHERE IllCode='" + txtCode.Text + "' ";
                SQL = SQL + ComNum.VBLF + " AND ILLCLASS  = '" + VB.Left(cboClass.Text, 1) + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제하였습니다.");
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
                return; //rtnVal;

            if (Delete() == true)
            {
                SCREEN_CLEAR();
                txtCode.Focus();
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                return; //rtnVal;

            Cursor.Current = Cursors.WaitCursor;

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            cboViewClass.Items.Add("7.불완전상병");
            cboViewClass.Items.Add("8.삭제코드");

            //자료를 인쇄
            strFont1 = "/fn\"굴림체\" /fz\"15";
            strFont2 = "/fn\"굴림체\" /fz\"11";
            strHead1 = "/n" + "/l/f1/c" + "상   병   코   드   집" + "/n";
            switch (VB.Left(cboViewClass.Text, 1))
            {
                case "7":
                    strHead1 = "/n" + "/l/f1/c" + "불   완   전   상   병   코   드" + "/n";
                    break;
                case "8":
                    strHead1 = "/n" + "/l/f1/c" + "삭   제   상   병   코   드" + "/n";
                    break;
                default:
                    strHead1 = "/n" + "/l/f1/c" + "상   병   코   드   집" + "/n";
                    break;
            }
            strHead2 = "/l/f2" + "출력일자 : " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + VB.Space(125) + "PAGE:" + "/p";
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Left = 5;
            ssView_Sheet1.PrintInfo.Margin.Right = 0;
            ssView_Sheet1.PrintInfo.Margin.Top = 30;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 110;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = true;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);

            Cursor.Current = Cursors.Default;
        }

        private void btnCodePrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                return; //rtnVal;

            return;

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            Cursor.Current = Cursors.WaitCursor;

            int i = 0;
            int j = 0;
            int k = 0;
            int nFile = 0;
            int nPage = 0;
            int nLine = 0;
            int nRow = 0;
            int nPart = 0;
            string strCode = "";
            string[,] strString = new string[5, 40];

            System.IO.StreamReader file = new System.IO.StreamReader(@"c:\illcode1.xls");


            //TODO: 프린트 관련

            if (nPage != 0)
            {

            }
            nPage = nPage + 1;


            if (rdoPart1.Checked == true)
            {
                if ((nPage % 2) == 1)
                {
                    nFile = 10;
                }
                else
                {
                    nFile = 11;
                }
            }
            else
            {
                nFile = 10;
            }

            strFont1 = "/fn\"굴림체\" /fz\"15";
            strFont2 = "/fn\"굴림체\" /fz\"11";
            strHead1 = "/n" + "/l/f1/c" + "인쇄일자" + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-") + "  " + nPage.ToString();
            strHead2 = "/l/f2" + "상병코드   상  병  명  칭 : " + "/p";
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Left = 5;
            ssView_Sheet1.PrintInfo.Margin.Right = 0;
            ssView_Sheet1.PrintInfo.Margin.Top = 30;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 110;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = true;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);






            if (rdoPart1.Checked == false)
            {
                #region
                CmdPrint3_Head(ref nPage, ref nFile);
                #endregion

                strCode = ssView_Sheet1.Cells[0, 0].Text;
                nPart = 1;
                nRow = 1;

                for (k = 0; k < 40; k++)
                {
                    strString[1, k] = "";
                    strString[2, k] = "";
                    strString[3, k] = "";
                    strString[4, k] = "";
                }

                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    if (VB.Mid(strCode, 2, 2) != VB.Mid(ssView_Sheet1.Cells[i, 0].Text, 2, 2))
                    {
                        strCode = ssView_Sheet1.Cells[i, 0].Text;
                        if (nRow != 1 && nRow != 39)
                        {
                            strString[nPart * 2 - 1, nRow] = "";
                            strString[nPart * 2, nRow] = VB.String(25, "-");
                            nRow += 1;
                            if (nPart == 1 && nRow >= 39)
                            {
                                nRow = 1;
                                nPart = 2;
                            }
                            if (nPart == 2 && nRow >= 39)
                            {
                                nRow = 1;
                                nPart = 1;
                                #region
                                PRT_WRITE(ref nPage, ref nFile, ref strString);
                                #endregion
                            }
                        }
                    }

                    strString[nPart * 2 - 1, nRow] = ssView_Sheet1.Cells[i, 0].Text.ToString().Trim();
                    strString[nPart * 2, nRow] = ssView_Sheet1.Cells[i, 2].Text.ToString().Trim();
                    PRT_WRITE(ref nPage, ref nFile, ref strString);
                }
                //마지막 라인 인쇄
                #region
                PRT_WRITE(ref nPage, ref nFile, ref strString);
                #endregion

            }

            Cursor.Current = Cursors.Default;
        }

        private void CmdPrint3_Head(ref int nPage, ref int nFile)  //TODO
        {
            //TODO: 프린트 관련

            if (nPage != 0)
            {

            }
            nPage = nPage + 1;


            if (rdoPart1.Checked == true)
            {
                if ((nPage % 2) == 1)
                {
                    nFile = 10;
                }
                else
                {
                    nFile = 11;
                }
            }
            else
            {
                nFile = 10;
            }


            //Print #nFile, "인쇄일자 : "; GstrSysDate; " "; GstrSysTime; Space$(43);
            //Print #nFile, "Page : "; Format$(nPage, "0000")
            //Print #nFile, String$(82, "=")
            //Print #nFile, "상병코드   상  병  명  칭";
            //'Print #nFile, "#041";


            //Print #nFile, "상병코드   상  병  명  칭"
            //Print #nFile, String$(82, "=")

            //Return
        }

        private void PRT_WRITE(ref int nPage, ref int nFile, ref string[,] strString) //TODO PRT_WRITE
        {
            int k = 0;

            if (rdoPart1.Checked == true)
            {
                if ((nPage % 2) == 1)
                    nFile = 10;
                else
                    nFile = 11;
            }
            else
            {
                nFile = 10;
            }
            //TODO 
            //for (k = 0; k < 39; k++)
            //{

            //    Print #nFile, LeftH(strString(1, K) & Space$(7), 7);        '상병코드
            //    Print #nFile, LeftH(strString(2, K) & Space$(35), 35);      '한글명칭
            //    Print #nFile, " ";
            //    Print #nFile, LeftH(strString(3, K) & Space$(7), 7);         '상병코드
            //    Print #nFile, LeftH(strString(4, K) & Space$(35), 35)        '한글명칭
            //}

            for (k = 0; k < 40; k++)
            {
                strString[1, k] = "";
                strString[2, k] = "";
                strString[3, k] = "";
                strString[4, k] = "";
            }

            CmdPrint3_Head(ref nPage, ref nFile);
        }

        private bool Save()
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                return false;//권한 확인

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);


            try
            {
                if (txtCode.Text.Trim() == "")
                {
                    ComFunc.MsgBox("상병코드가 공란입니다.", "오류");
                    return false;
                }
                if (cboClass.Text.Trim() == "")
                {
                    ComFunc.MsgBox("상병분류가 공란입니다.", "오류");
                    return false;
                }
                if (txtNameK.Text.Trim() == "" && txtNameE.Text.Trim() == "")
                {
                    ComFunc.MsgBox("한글명,영문명 모두 공란입니다.", "오류");
                    return false;
                }

                txtNameK.Text = clsVbfunc.QuotationChange(txtNameK.Text.Trim());
                txtNameE.Text = clsVbfunc.QuotationChange(txtNameE.Text.Trim());
                txtDispHeader.Text = clsVbfunc.QuotationChange(txtDispHeader.Text.Trim());

                if (fstrROWID == "")    //신규등록
                {
                    SQL = "INSERT INTO BAS_ILLS (";
                    SQL = SQL + ComNum.VBLF + " IllCode, IllClass, IllNameK, IllNameE, NameSpacing,";
                    SQL = SQL + ComNum.VBLF + " DispHeader, ILLUPCODE, NOUSE,INFECT, GUBUN, ";
                    SQL = SQL + ComNum.VBLF + " KCD8, SDATE, DDATE, ILLCODED  , NoUseDate,repcode,";
                    SQL = SQL + ComNum.VBLF + " GBV252, GBV352, GBVCODE, IPDETC, GBER,  GBVCODE1, GBVCODE2,GBVCODE3) VALUES (";
                    SQL = SQL + ComNum.VBLF + " '" + txtCode.Text + "','" + VB.Left(cboClass.Text, 1) + "','";
                    SQL = SQL + ComNum.VBLF + txtNameK.Text.Trim() + "','" + txtNameE.Text.Trim() + "','" + VB.Val(txtNameSpacing.Text).ToString("0") + "',";
                    SQL = SQL + ComNum.VBLF + " '" + txtDispHeader.Text + "', '" + txtILLUP.Text + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + (chkNoUse.Checked ? "N" : "") + "' , ";
                    SQL = SQL + ComNum.VBLF + " '" + (chkInFect.Checked ? "Y" : "") + "' , ";
                    SQL = SQL + ComNum.VBLF + " '" + (chkGubun.Checked ? "1" : "") + "',  ";
                    SQL = SQL + ComNum.VBLF + " '" + (chkKCD6.Checked ? "*" : "") + "', ";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + TxtSDate.Text + "','YYYY-MM-DD') , ";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + TxtDDate.Text + "','YYYY-MM-DD') , ";
                    SQL = SQL + ComNum.VBLF + " '" + txtILLCODED.Text + "' , ";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + TxtNoUseDate.Text + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + " '" + txtRepCode.Text.ToUpper() + "' , ";
                    SQL = SQL + ComNum.VBLF + " '" + (chkV252.Checked ? "*" : "") + "' , ";
                    SQL = SQL + ComNum.VBLF + " '" + (chkV352.Checked ? "*" : "") + "' , ";
                    SQL = SQL + ComNum.VBLF + " '" + (chkGBVCode.Checked ? "*" : "") + "' , ";
                    //'SQL = SQL + " '" + IIf(Chkjusangbung.Value = 1, " * ", "") + "' , "
                    SQL = SQL + ComNum.VBLF + " '" + (chkjusangbung.Checked ? "Y" : "N") + "' , ";//     '2018-10-25
                    SQL = SQL + ComNum.VBLF + " '" + (chkER.Checked ? "*" : "") + "','" + (chkGBVCode1.Checked ? "*" : "") + "',";
                    SQL = SQL + ComNum.VBLF + " '" + (chkGBVCode2.Checked ? "*" : "") + "','" + (chkGBVCode3.Checked ? "*" : "") + "'";//  " '2018-12-21
                    SQL = SQL + ComNum.VBLF + " ) ";
                }
                else
                {
                    SQL = "UPDATE BAS_ILLS SET IllClass='" + VB.Left(cboClass.Text, 1) + "',";
                    SQL = SQL + ComNum.VBLF + "IllNameK='" + txtNameK.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "IllNameE='" + txtNameE.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "DispHeader='" + txtDispHeader.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "NameSpacing='" + VB.Format(txtNameSpacing.Text.Trim(), "0") + "',";
                    SQL = SQL + ComNum.VBLF + "  ILLUPCODE = '" + txtILLUP.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "  NOUSE = '" + (chkNoUse.Checked ? "N" : "") + "' , ";
                    SQL = SQL + ComNum.VBLF + "  INFECT = '" + (chkInFect.Checked ? "Y" : "") + "' , ";
                    SQL = SQL + ComNum.VBLF + "  GUBUN ='" + (chkGubun.Checked ? "1" : "") + "',  ";
                    SQL = SQL + ComNum.VBLF + "  KCD8 ='" + (chkKCD6.Checked ? "*" : "") + "' , ";
                    SQL = SQL + ComNum.VBLF + "  SDATE = TO_DATE('" + TxtSDate.Text + "','YYYY-MM-DD') , ";
                    SQL = SQL + ComNum.VBLF + "  DDATE = TO_DATE('" + TxtDDate.Text + "','YYYY-MM-DD') , ";
                    SQL = SQL + ComNum.VBLF + "  ILLCODED  = '" + txtILLCODED.Text + "', ";
                    SQL = SQL + ComNum.VBLF + "  repcode  = '" + txtRepCode.Text.ToUpper() + "', ";
                    SQL = SQL + ComNum.VBLF + "  NoUseDate = TO_DATE('" + TxtNoUseDate.Text + "','YYYY-MM-DD') , ";
                    SQL = SQL + ComNum.VBLF + "  GBV252 = '" + (chkV252.Checked ? "*" : "") + "'  ,";
                    SQL = SQL + ComNum.VBLF + " GBV352 = '" + (chkV352.Checked ? "*" : "") + "'  ,";//        '2018-10-24
                    SQL = SQL + ComNum.VBLF + " GBVCODE = '" + (chkGBVCode.Checked ? "*" : "") + "' , ";
                    //'SQL = SQL + " IPDETC = '" + IIf(Chkjusangbung = 1, "Y", "") + "' , "
                    SQL = SQL + ComNum.VBLF + " IPDETC = '" + (chkjusangbung.Checked ? "Y" : "N") + "' , ";//   '2018-10-25
                    SQL = SQL + ComNum.VBLF + " GBER = '" + (chkER.Checked ? "*" : "") + "',  ";
                    SQL = SQL + ComNum.VBLF + " GBVCODE1 = '" + (chkGBVCode1.Checked ? "*" : "") + "',  ";//      '2018-12-21
                    SQL = SQL + ComNum.VBLF + " GBVCODE2 = '" + (chkGBVCode2.Checked ? "*" : "") + "',  ";//      '2018-12-21
                    SQL = SQL + ComNum.VBLF + " GBVCODE3 = '" + (chkGBVCode3.Checked ? "*" : "") + "'  ";//       '2018-12-21

                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + fstrROWID + "' ";
                }
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Save() == true)
            {
                SCREEN_CLEAR();
                txtCode.Focus();
            }

        }

        private void btnCenterSearch_Click(object sender, EventArgs e)
        {
            READ_BAS_ILLS_KCD6("ALL");
        }

        private void btnExellPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                return;//권한 확인


            Cursor.Current = Cursors.WaitCursor;

            using (SaveFileDialog saveFile = new SaveFileDialog())
            {
                saveFile.Filter = "엑셀파일(*.xlsx)|*.xlsx";
                saveFile.ShowDialog(this);
                ssView_Sheet1.Protect = false;
                ssView.SaveExcel(saveFile.FileName, FarPoint.Excel.ExcelSaveFlags.UseOOXMLFormat | FarPoint.Excel.ExcelSaveFlags.SaveCustomRowHeaders);
                ssView_Sheet1.Protect = true;
            }
            

            Cursor.Current = Cursors.Default;
        }

        private void botomSearch()
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //    return;//권한 확인


            txtCode1.Text = txtCode1.Text.ToUpper().Trim();
            string strCode = txtCode1.Text.Trim();

            for (int i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                if (strCode == ssView_Sheet1.Cells[i, 0].Text)
                {
                    ssView.ShowRow(0, i, FarPoint.Win.Spread.VerticalPosition.Center);
                    ssView_Sheet1.SetActiveCell(i, 0);
                    break;
                }
            }

        }

        private void btnbotomSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return;//권한 확인

            botomSearch();
        }

        private void btnSreach_Click(object sender, EventArgs e)
        {
            string strText = ((Button)sender).Text;

            READ_BAS_ILLS_KCD6(strText.Trim());
        }

        private void rdoSortSearch_Click(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {

            }
        }
        //TODO : VB폼에선 메뉴스트립에선 없는 메뉴이며, 비지블은 false값임
        #region KCD6 일괄입력(사용안함)

        //private void btnKCD_Click(object sender, EventArgs e)
        //{
        //    if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
        //        return;//권한 확인

        //    if (GbSabun != "4349")
        //    {
        //        ComFunc.MsgBox("권한이 없습니다.", "확인");
        //        return;
        //    }
        //    if (ComFunc.MsgBoxQ("KCD6차 원내 상병에 적용 하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
        //        return;

        //    int i = 0;
        //    int j = 0;
        //    int nREAD = 0;
        //    int nREAD2 = 0;
        //    string strIllCode = "";
        //    string strIllCodeNew = "";
        //    string strILLNameK = "";
        //    string strILLNameE = "";
        //    string strGbNoUse = "";
        //    string strGbSex = "";
        //    string strGbInfect = "";
        //    string strInfect = "";
        //    string strROWID = "";
        //    string strADD = "";
        //    string strOK = "";
        //    string SQL = "";
        //    string SqlErr = ""; //에러문 받는 변수
        //    int intRowAffected = 0; //변경된 Row 받는 변수
        //    DataTable dt = null;
        //    DataTable dt1 = null;
        //    DataTable dt2 = null;

        //    clsDB.setBeginTran(clsDB.DbCon);


        //    try
        //    {
        //        SQL = " SELECT ILLCODE, ILLNAMEK , ILLNAMEE, GBNOUSE, GBSEX, GBINFECT";
        //        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_ILLS_KCD6_OK ";
        //        SQL = SQL + ComNum.VBLF + " ORDER BY ILLCODE ";

        //        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
        //        if (SqlErr != "")
        //        {
        //            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
        //            ComFunc.MsgBox("조회중 문제가 발생했습니다");
        //            return;
        //        }

        //        nREAD = dt.Rows.Count;

        //        strOK = "OK";

        //        for (i = 0; i < nREAD; i++)
        //        {
        //            strIllCode = (dt.Rows[0]["illcode"].ToString().Trim());
        //            strILLNameK = VB.Replace(dt.Rows[0]["ILLNAMEK"].ToString().Trim(), "'", "`");
        //            strILLNameE = VB.Replace(dt.Rows[0]["IllnameE"].ToString().Trim(), "'", "`");
        //            strGbNoUse = (dt.Rows[0]["gbnouse"].ToString().Trim());
        //            strGbSex = dt.Rows[0]["gbsex"].ToString().Trim();

        //            if (strGbSex == "Y")
        //                strGbSex = "M";
        //            else if (strGbSex == "X")
        //                strGbSex = "F";
        //            else
        //                strGbSex = "";

        //            if (strGbInfect != "")
        //                strGbInfect = "Y";
        //            else
        //                strGbInfect = "";

        //            strADD = "";

        //            if (strIllCode == "A000")
        //                i = i;

        //            SQL = " SELECT ROWID, KCD8  FROM " + ComNum.DB_PMPA + "BAS_ILLS";
        //            SQL = SQL + ComNum.VBLF + " WHERE ILLCODED = '" + strIllCode + "' ";
        //            SQL = SQL + ComNum.VBLF + "   AND ILLCLASS ='1'";
        //            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
        //            if (SqlErr != "")
        //            {
        //                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
        //                ComFunc.MsgBox("조회중 문제가 발생했습니다");
        //                return;
        //            }

        //            nREAD2 = dt1.Rows.Count;

        //            if (nREAD2 == 0)
        //            {
        //                SQL = "INSERT INTO " + ComNum.DB_PMPA + "BAS_ILLS ( ILLCODE, ILLCLASS , ILLNAMEK, ILLNAMEE, NAMESPACING, DISPHEADER, RETURNVAL, NOUSE, SEX, INFECT, GUBUN, ILLUPCODE, ILLCODED, GBINFECT,KCD8) ";
        //                SQL = SQL + ComNum.VBLF + "  VALUES ( ";
        //                SQL = SQL + ComNum.VBLF + " '" + strIllCode + "' , '1','" + strILLNameK + "', '" + strILLNameE + "', ";
        //                SQL = SQL + ComNum.VBLF + "  0 , '', 1, '" + strGbNoUse + "', '" + strGbSex + "' , '" + strInfect + "' ,";
        //                SQL = SQL + ComNum.VBLF + " '', '', '" + strIllCode + "' ,  '" + strGbInfect + "', '*'  ";
        //                SQL = SQL + ComNum.VBLF + " ) ";
        //                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
        //                if (SqlErr != "")
        //                {
        //                    clsDB.setRollbackTran(clsDB.DbCon);
        //                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
        //                    ComFunc.MsgBox(SqlErr);
        //                    Cursor.Current = Cursors.Default;
        //                    return;
        //                }
        //            }
        //            else if (nREAD2 == 1)
        //            {
        //                strROWID = dt1.Rows[0]["ROWID"].ToString().Trim();
        //                if (dt1.Rows[0]["KCD6"].ToString().Trim() == "*")
        //                {
        //                    strIllCodeNew = strIllCode + VB.Format(dt2.Rows[0]["000"].ToString().Trim());

        //                    SQL = "SELECT ILLCODE FROM ADMIN.BAS_ILLS ";
        //                    SQL = SQL + ComNum.VBLF + " WHERE ILLCODE = '" + strIllCodeNew + "' ";
        //                    SQL = SQL + ComNum.VBLF + " AND   ILLCLASS = '1' ";
        //                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
        //                    if (SqlErr != "")
        //                    {
        //                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
        //                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
        //                        return;
        //                    }

        //                    if (dt2.Rows.Count == 0)
        //                    {
        //                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "BAS_ILLS ( ILLCODE, ILLCLASS , ILLNAMEK, ILLNAMEE, NAMESPACING, DISPHEADER, RETURNVAL, NOUSE, SEX, INFECT, GUBUN, ILLUPCODE, ILLCODED, GBINFECT,KCD6) ";
        //                        SQL = SQL + ComNum.VBLF + "  VALUES ( ";
        //                        SQL = SQL + ComNum.VBLF + " '" + strIllCodeNew + "' , '1','" + strILLNameK + "', '" + strILLNameE + "', ";
        //                        SQL = SQL + ComNum.VBLF + "  0 , '', 1, '" + strGbNoUse + "', '" + strGbSex + "' , '" + strInfect + "' ,";
        //                        SQL = SQL + ComNum.VBLF + " '', '', '" + strIllCode + "' ,  '" + strGbInfect + "' ,'*' ";
        //                        SQL = SQL + ComNum.VBLF + " ) ";

        //                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
        //                        if (SqlErr != "")
        //                        {
        //                            clsDB.setRollbackTran(clsDB.DbCon);
        //                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
        //                            ComFunc.MsgBox(SqlErr);
        //                            Cursor.Current = Cursors.Default;
        //                            return;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        strOK = "NO";
        //                        ComFunc.MsgBox("오류", "확인");
        //                    }
        //                }
        //                else
        //                {
        //                    SQL = "UPDATE " + ComNum.VBLF + "BAS_ILLS SET ";
        //                    SQL = SQL + ComNum.VBLF + "  ILLNAMEK = '" + strILLNameK + "' ,  ";
        //                    SQL = SQL + ComNum.VBLF + "  ILLNAMEE = '" + strILLNameE + "',";
        //                    SQL = SQL + ComNum.VBLF + "  NOUSE = '" + strGbNoUse + "' ,";
        //                    SQL = SQL + ComNum.VBLF + "  SEX = '" + strGbSex + "' ,";
        //                    SQL = SQL + ComNum.VBLF + "  INFECT = '" + strInfect + "' ,";
        //                    SQL = SQL + ComNum.VBLF + "  GBINFECT = '" + strGbInfect + "',";
        //                    SQL = SQL + ComNum.VBLF + "  KCD6= '*' ";
        //                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";
        //                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
        //                    if (SqlErr != "")
        //                    {
        //                        clsDB.setRollbackTran(clsDB.DbCon);
        //                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
        //                        ComFunc.MsgBox(SqlErr);
        //                        Cursor.Current = Cursors.Default;
        //                        return;
        //                    }
        //                }
        //                if (nREAD2 > 1)
        //                {
        //                    strIllCodeNew = strIllCode + VB.Format(dt2.Rows[0]["000"].ToString().Trim());
        //                    SQL = "SELECT ILLCODE FROM ADMIN.BAS_ILLS ";
        //                    SQL = SQL + ComNum.VBLF + " WHERE ILLCODE = '" + strIllCodeNew + "' ";
        //                    SQL = SQL + ComNum.VBLF + " AND ILLCLASS = '1' ";

        //                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
        //                    if (SqlErr != "")
        //                    {
        //                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
        //                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
        //                        return;
        //                    }

        //                    if (dt2.Rows.Count == 0)
        //                    {
        //                        SQL = "INSERT INTO " + ComNum.VBLF + "BAS_ILLS ( ILLCODE, ILLCLASS , ILLNAMEK, ILLNAMEE, NAMESPACING, DISPHEADER, RETURNVAL, NOUSE, SEX, INFECT, GUBUN, ILLUPCODE, ILLCODED, GBINFECT,KCD6) ";
        //                        SQL = SQL + ComNum.VBLF + "  VALUES ( ";
        //                        SQL = SQL + ComNum.VBLF + " '" + strIllCodeNew + "' , '1','" + strILLNameK + "', '" + strILLNameE + "', ";
        //                        SQL = SQL + ComNum.VBLF + "  0 , '', 1, '" + strGbNoUse + "', '" + strGbSex + "' , '" + strInfect + "' ,";
        //                        SQL = SQL + ComNum.VBLF + " '', '', '" + strIllCode + "' ,  '" + strGbInfect + "' ,'*' ";
        //                        SQL = SQL + ComNum.VBLF + " ) ";
        //                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
        //                        if (SqlErr != "")
        //                        {
        //                            clsDB.setRollbackTran(clsDB.DbCon);
        //                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
        //                            ComFunc.MsgBox(SqlErr);
        //                            Cursor.Current = Cursors.Default;
        //                            return;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        ComFunc.MsgBox("오류 ", "확인");
        //                        strOK = "NO";
        //                    }
        //                    dt2.Dispose();
        //                    dt2 = null;
        //                }
        //                dt1.Dispose();
        //                dt1 = null;
        //                if (strOK == "NO")
        //                {
        //                    break;
        //                }
        //            }
        //            dt.Dispose();
        //            dt = null;

        //            if (strOK == "OK")
        //            {
        //                Cursor.Current = Cursors.Default;
        //                clsDB.setCommitTran(clsDB.DbCon);
        //                ComFunc.MsgBox("작업 완료.");
        //            }
        //            else
        //            {
        //                Cursor.Current = Cursors.Default;
        //                clsDB.setRollbackTran(clsDB.DbCon);
        //                ComFunc.MsgBox("작업 오류.");
        //            }
        //        }
        //        clsDB.setCommitTran(clsDB.DbCon);
        //        ComFunc.MsgBox("저장하였습니다.");
        //        Cursor.Current = Cursors.Default;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsDB.setRollbackTran(clsDB.DbCon);
        //        ComFunc.MsgBox(ex.Message);
        //        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
        //        Cursor.Current = Cursors.Default;
        //        return;
        //    }
        //} 
        #endregion

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
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

            txtCode.Text = ssView_Sheet1.Cells[e.Row, 5].Text.Trim();
            Screen_Display(VB.Left(cboClass.Text, 1));
        }

        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Screen_Display(VB.Left(cboClass.Text, 1));
            }
        }

        private void txtCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (VB.Len(txtCode.Text) < 3)
            {
                GSILLCodeD = txtCode.Text;

                if (GSILLCodeD != "")
                {
                    txtCode.Text = GSILLCodeD;
                    txtILLCODED.Text = GSILLCodeD;
                    txtNameE.Text = GSILLNameK;
                    txtNameK.Text = GSILLNameE;
                }
                txtCode.Focus();
            }
        }

        private void txtCode1_KeyDown(object sender, KeyEventArgs e)
        {
            GintOutCNT = 0;

            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            txtCode1.Text = txtCode1.Text.Trim().ToUpper();
            if (txtCode1.Text == "")
                return;

            botomSearch();

            SendKeys.Send("{Tab}");
        }

        private void txtILLUP_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtILLUPName.Text = READ_BAS_ILL_Name(txtILLUPName.Text);
                
            }

            return;
        }

        private void txtNameE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtNameK_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtDispHeader_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtNameSpacing_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mnu금지약어관리_Click(object sender, EventArgs e)
        {
      
        }

        private void TxtDate_DoubleClick(object sender, EventArgs e)
        {
            TextBox txtDate = (sender as TextBox);

            lastDateText = txtDate;
            dtpCalendar.SetBounds(txtDate.Location.X, txtDate.Location.Y + 50, dtpCalendar.Width, dtpCalendar.Height);
            dtpCalendar.Visible = true;
        }


        private void dtpCalendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            if (lastDateText == null)
                return;

            lastDateText.Text = dtpCalendar.SelectionStart.ToShortDateString();
            dtpCalendar.Visible = false;
        }

        private void btnSearchDel_Click(object sender, EventArgs e)
        {
            if (sender.Equals(btnSearchDel))
            {
                cboViewClass.SelectedIndex = 11;
            }
            else
            {
                cboViewClass.SelectedIndex = 10;
            }

            READ_BAS_ILLS_KCD6("");
        }

        private void frmMsymEntry_Activated(object sender, EventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgActivedForm(this);
            }
        }

        private void frmMsymEntry_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }
    }
}