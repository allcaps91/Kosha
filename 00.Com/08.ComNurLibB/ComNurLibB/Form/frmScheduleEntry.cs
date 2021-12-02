using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    /// <summary>   
    /// File Name       : frmScheduleEntry.cs
    /// Description     : 병동 Schedule 등록
    /// Author          : 유진호
    /// Create Date     : 2018-02-02
    /// <history>       
    /// D:\포항성모병원 VB Source(2017.11.20)\nurse\nropd\FrmScheduleEntry
    /// </history>
    /// </summary>
    public partial class frmScheduleEntry : Form, MainFormMessage
    {
        #region MainFormMessage InterFace

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

        ComFunc CF = new ComFunc();

        int nDateH1 = 0;                //'월의 처음 값
        int nDateH2 = 0;                //'월의 마지막 값
        int nCurrRow = 0;
        int nCurrCol = 0;
        string str_sabun = "";          //'insa변수사번
        string str_YYMM = "";           //'insa변수YYMM
        string[] str_BunpYo;            //'insa변수번표
        //string str_ChulTime = "";       //'insa변수출근시각
        //string str_ChulGbn = "";        //'insa변수출근구분
        //int n_BunpYo = 0;
        //int n_ChulTime = 0;
        //int n_DD = 0;
        //string str_CTime = "";
        string[] strChuCode;
        int FnRow = 0;
        int FnCOL = 0;
        string str_jik = "";            //'insa변수직책

        string FstrCheck = "";
        string GstrWardCode = "";
        string mstrMacro = "";

        public frmScheduleEntry()
        {
            InitializeComponent();
        }

        public frmScheduleEntry(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
        }

        public frmScheduleEntry(MainFormMessage pform, string strWardCode)
        {
            InitializeComponent();
            this.mCallForm = pform;
            GstrWardCode = strWardCode;
        }

        public frmScheduleEntry(string strWardCode)
        {
            InitializeComponent();

            GstrWardCode = strWardCode;
        }

        private void frmScheduleEntry_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등


            if (GstrWardCode == "")
            {
                GstrWardCode = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "");
            }
            if (clsType.User.Sabun == "40024")
            {
                GstrWardCode = "OPD";
            }
            

            if (GstrWardCode == "4H")
            {
                GstrWardCode = "HU";
            }

            if (GstrWardCode == "OP")
            {
                GstrWardCode = "OR";
            }

            clsSpread.gSpreadEnter_NextCol(ssView1);

            InputMap map = ssView1.GetInputMap(InputMapMode.WhenAncestorOfFocused, OperationMode.Normal);
            map.Put(new Keystroke(Keys.F2, Keys.None), "None");
            map.Put(new Keystroke(Keys.F3, Keys.None), "None");
            map.Put(new Keystroke(Keys.F4, Keys.None), "None");

            if (GstrWardCode == "OPD")
            {
                ssView5.Visible = false;
                chk_Sort.Visible = false;
            }

            ComFunc.ReadSysDate(clsDB.DbCon);

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            int nYY = 0;
            int nMM = 0;
            string strYYMM = "";
            //string strData = "";

            str_BunpYo = new string[32];
            strChuCode = new string[51];

            for (i = 0; i < strChuCode.Length; i++)
            {
                strChuCode[i] = "";
            }

            try
            {

                //SQL = "";
                //SQL = SQL + ComNum.VBLF + " SELECT NURCODE FROM KOSMOS_ADM.INSA_GUNTAECODE ";
                //SQL = SQL + ComNum.VBLF + " WHERE NURCODE IS NOT NULL ";
                //SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL  ";
                //SQL = SQL + ComNum.VBLF + " ORDER BY NURCODE ";

                SQL = "";
                SQL = " SELECT NURCODE";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "INSA_GUNTAECODE A, " + ComNum.DB_PMPA + "NUR_CODE B";
                SQL = SQL + ComNum.VBLF + "  WHERE a.NURCODE Is Not Null";
                SQL = SQL + ComNum.VBLF + "    AND A.DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "    AND B.DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "    AND A.NURCODE = B.CODE";
                SQL = SQL + ComNum.VBLF + "    AND GUBUN = '4'";
                SQL = SQL + ComNum.VBLF + "    ORDER BY B.PRINTRANKING";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    if (GstrWardCode != "OPD")
                    {
                        //ssView3_Sheet1.ColumnCount = dt.Rows.Count;
                        ssView3_Sheet1.ColumnCount = 39;
                    }

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i < 39)
                        {
                            ssView3_Sheet1.Cells[1, i].Text = dt.Rows[i]["NURCODE"].ToString().Trim();
                            ssView3_Sheet1.Cells[1, i].Text = dt.Rows[i]["NURCODE"].ToString().Trim();
                        }
                        else
                        {
                            ssView3_Sheet1.Cells[1, 36].Text = dt.Rows[i]["NURCODE"].ToString().Trim();
                            ssView3_Sheet1.Cells[1, 36].Text = dt.Rows[i]["NURCODE"].ToString().Trim();
                        }

                        strChuCode[i + 1] = dt.Rows[i]["NURCODE"].ToString().Trim();
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            for (int idx = 0; idx < ssView1_Sheet1.ColumnCount; idx++)
            {
                clsSpread.gSpreadHeaderLineBoder(ssView1, 0, idx, 0, idx, Color.Black, 1, false, false, true, false);
            }

            if (GstrWardCode == "OPD")
            {
                ssView1_Sheet1.Rows[1].Visible = false;
                ssView1_Sheet1.Rows[2].Visible = false;
                ssView1_Sheet1.Rows[3].Visible = false;
                ssView1_Sheet1.Rows[4].Visible = false;
                ssView1_Sheet1.Columns[35].Visible = false;
                ssView1_Sheet1.Columns[36].Visible = false;
                ssView1_Sheet1.Columns[37].Visible = false;
                ssView1_Sheet1.Columns[38].Visible = false;
                ssView1_Sheet1.Columns[39].Visible = false;
            }
            ssView1_Sheet1.Columns[40].Visible = false;
            ssView1_Sheet1.Columns[41].Visible = false;
            ssView1_Sheet1.Columns[42].Visible = false;
            ssView1_Sheet1.Columns[43].Visible = false;
            ssView1_Sheet1.Columns[45+1].Visible = false;

            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnHelp.Visible = false;
            panHelpList.Visible = false;
            btnHelp1.Visible = false;
            panHelp1List.Visible = false;
            txtHelpName.Text = "";


            if (GstrWardCode == "OPD")
            {
                //SSopd.Visible = True
                //SS2.Visible = False
            }
            else
            {
                //SS2.Visible = True
                //SSopd.Visible = False
            }

            Screen_Clear();

            nYY = Convert.ToInt32(VB.Val(VB.Left(clsPublic.GstrSysDate, 4)));
            nMM = Convert.ToInt32(VB.Val(VB.Mid(clsPublic.GstrSysDate, 6, 2)) + 1);


            if (nMM == 13)
            {
                nYY = nYY + 1;
                nMM = 1;
            }

            strYYMM = VB.Format(nYY, "###0") + ComFunc.LPAD(nMM.ToString(), 2, "0");
            DateTime dtTemp;
            cboYYMM.Items.Clear();
            for (i = 0; i < 12; i++)
            {
                cboYYMM.Items.Add(VB.Left(strYYMM, 4) + "년" + VB.Right(strYYMM, 2) + "월");
                dtTemp = Convert.ToDateTime(VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01").AddDays(-1);
                strYYMM = dtTemp.Year.ToString() + ComFunc.LPAD(dtTemp.Month.ToString(), 2, "0");
            }
            cboYYMM.SelectedIndex = 0;

            ComboDept_Load();

            cbo3A.Items.Add("OPEN");
            cbo3A.Items.Add("CLOSE");
            cbo3A.Items.Add("등록창");
            cbo3A.SelectedIndex = 0;

            ReadHelp1List();

            if (cboDept.Text == "3A")
                cbo3A.Visible = true;
        }

        void ReadHelp1List()
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            //int nYY = 0;
            //int nMM = 0;
            //string strYYMM = "";
            string strData = "";

            try
            {
                ssList2_Sheet1.RowCount = 0;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Buse,Sabun,KorName,Jik ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_ADM.INSA_MST ";
                SQL = SQL + ComNum.VBLF + " WHERE Buse IN (";
                SQL = SQL + ComNum.VBLF + " SELECT MATCH_CODE FROM KOSMOS_PMPA.NUR_CODE";
                SQL = SQL + ComNum.VBLF + "  WHERE GUBUN = '2'";
                SQL = SQL + ComNum.VBLF + "  AND GBUSE = 'Y')";
                if (GstrWardCode == "OPD")
                {
                    SQL = SQL + ComNum.VBLF + "  AND (ToiDay is NULL OR TOIDAY >= TO_DATE('2015-01-01','YYYY-MM-DD')) ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND (ToiDay is NULL OR TOIDAY >= TO_DATE('2015-01-01','YYYY-MM-DD')) ";
                }
                if (txtHelpName.Text.Trim() != "")
                {
                    SQL = SQL + ComNum.VBLF + "  AND KORNAME LIKE '%" + txtHelpName.Text.Trim() + "%'";
                }
                SQL = SQL + ComNum.VBLF + "ORDER BY KorName,Jik,Sabun ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strData = VB.Left(dt.Rows[i]["KorName"].ToString().Trim() + VB.Space(10), 10);
                        strData = strData + VB.Left(dt.Rows[i]["SABUN"].ToString().Trim() + VB.Space(8), 8);
                        strData = strData + VB.Left(Read_JikName(dt.Rows[i]["Jik"].ToString().Trim()) + VB.Space(15), 15);
                        strData = strData + VB.Left(getDeptName(dt.Rows[i]["BUSE"].ToString().Trim()) + VB.Space(10), 10);

                        ssList2_Sheet1.RowCount = ssList2_Sheet1.RowCount + 1;
                        ssList2_Sheet1.Cells[i, 0].Text = strData;
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        string Check_Nurse_Buse(string argSabun)
        {
            //2016-08-09 계장 김현욱
            //부서이동으로 인하여 간호부가 아닐 경우 INSA_CHULTIME에
            //데이터를 핸들링되지 않도록 하기 위함.

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strBuse = "";
            string rtnVar = "";


            try
            {
                SQL = "";
                SQL = " SELECT BUSE";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.INSA_MST";
                SQL = SQL + ComNum.VBLF + " WHERE SABUN = '" + argSabun + "'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }

                if (dt.Rows.Count > 0)
                {
                    strBuse = dt.Rows[0]["BUSE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (strBuse == "")
                {
                    rtnVar = "OK";
                    return rtnVar;
                }

                SQL = "";
                SQL = " SELECT CODE";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_CODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '2'";
                SQL = SQL + ComNum.VBLF + "   AND MATCH_CODE = '" + strBuse + "'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVar = "OK";
                }
                else
                {
                    rtnVar = "NO";
                }

                dt.Dispose();
                dt = null;

                return rtnVar;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVar;
            }
        }

        /// <summary>
        /// Jik을 읽어 cJikName 반환
        /// </summary>
        /// <param name="argJik">Jik</param>
        /// <returns></returns>
        private string Read_JikName(string argJik)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT Name cJikName FROM NUR_CODE ";
                SQL += ComNum.VBLF + "WHERE Gubun = '1' ";
                SQL += ComNum.VBLF + "  AND Code = '" + String.Format("{0:00}", argJik) + "' ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return rtnVal;
                }

                if (dt.Rows[0]["cJikName"].ToString().Trim() != "")
                {
                    rtnVal = dt.Rows[0]["cJikName"].ToString().Trim();
                }
                else
                {
                    rtnVal = "";
                }
                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            catch
            {
                dt.Dispose();
                dt = null;
                rtnVal = "";
                return rtnVal;
            }
        }

        private void ComboDept_Load()
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                cboDept.Items.Clear();

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Code,Name FROM KOSMOS_PMPA.NUR_CODE ";

                if (GstrWardCode == "GAN" || clsPublic.GstrJobGrade == "EDPS" || clsType.User.Sabun == "04349" || clsType.User.Sabun == "08985" || 
                    clsType.User.Sabun == "04636" || clsType.User.Sabun == "16092" || clsType.User.Sabun == "23767" || 
                    clsType.User.Sabun == "36433" || clsType.User.Sabun == "20095" || clsType.User.Sabun == "42646" || 
                    clsType.User.Sabun == "42092" || clsType.User.Sabun == "35472" || clsType.User.Sabun == "08822" || 
                    clsType.User.Sabun == "38287" || clsType.User.Sabun == "14472" || clsType.User.Sabun == "48345")
                {
                    SQL = SQL + ComNum.VBLF + " WHERE GBUSE = 'Y' ";
                    SQL = SQL + ComNum.VBLF + " AND Gubun = '2' ";
                    SQL = SQL + ComNum.VBLF + " ORDER By PrintRanking ";
                }
                else if (GstrWardCode != "GAN" && (GstrWardCode == "OR" || GstrWardCode == "AN"))
                {
                    SQL = SQL + ComNum.VBLF + " WHERE Code IN ('AN','OR','ANGO')";
                    SQL = SQL + ComNum.VBLF + "   AND NAME <> '수술실'  ";
                    SQL = SQL + ComNum.VBLF + " AND Gubun = '2' ";
                    SQL = SQL + ComNum.VBLF + " ORDER By PrintRanking ";
                }
                else if (GstrWardCode != "GAN" && (GstrWardCode == "7W"))
                {
                    SQL = SQL + ComNum.VBLF + " WHERE Code IN ('7W','HD')";
                    SQL = SQL + ComNum.VBLF + " AND Gubun = '2' ";
                    SQL = SQL + ComNum.VBLF + " ORDER By PrintRanking ";
                }
                else if (GstrWardCode != "GAN" && (GstrWardCode == "SI"))     //'심전도실
                {
                    SQL = SQL + ComNum.VBLF + " WHERE Code IN ('OPD','DOCT')";
                    SQL = SQL + ComNum.VBLF + " AND Gubun = '2' ";
                    SQL = SQL + ComNum.VBLF + " ORDER By PrintRanking DESC ";
                }
                else if (GstrWardCode != "GAN" && GstrWardCode == "3C")      //'3C경우
                {
                    SQL = SQL + ComNum.VBLF + " WHERE Code IN ('3C','NR','DR')";
                    SQL = SQL + ComNum.VBLF + " AND Gubun = '2' ";
                    SQL = SQL + ComNum.VBLF + " ORDER By PrintRanking ";
                }
                else if(GstrWardCode == "OPD")
                {
                    SQL = SQL + ComNum.VBLF + " WHERE Code IN ('OPD','DT','ANGO')";
                    SQL = SQL + ComNum.VBLF + " AND Gubun = '2' ";
                    SQL = SQL + ComNum.VBLF + " ORDER By PrintRanking ";
                    
                }
                else if (GstrWardCode != "GAN")
                {
                    SQL = SQL + ComNum.VBLF + " WHERE Code = '" + GstrWardCode + "' ";
                    SQL = SQL + ComNum.VBLF + " AND Gubun = '2' ";
                    SQL = SQL + ComNum.VBLF + " ORDER By PrintRanking ";
                }


                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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
                    ComFunc.MsgBox("부서가 설정되어 있지 않습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboDept.Items.Add(dt.Rows[i]["Name"].ToString().Trim());
                    }

                    cboDept.SelectedIndex = 0;
                }

                dt.Dispose();
                dt = null;

                if (GstrWardCode == "ER")
                {
                    cboDept.Items.Clear();
                    cboDept.Items.Add("ER전체");
                    cboDept.Items.Add("ER간호사/조무사");
                    cboDept.Items.Add("ER구조사");
                    cboDept.SelectedIndex = 0;
                }

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            if (cboDept.Items.Count == 1)
                cboDept.Enabled = false;
        }

        private void Screen_Clear()
        {
            ssView1_Sheet1.RowCount = 50;
            ssView1_Sheet1.Cells[0, 4, 0, ssView1_Sheet1.ColumnCount - 1].Text = "";
            ssView1_Sheet1.Cells[0, 4, 0, ssView1_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(192, 192, 192);

            ssView1_Sheet1.Cells[1, 4, 4, ssView1_Sheet1.ColumnCount - 1].Text = "";
            ssView1_Sheet1.Cells[1, 4, 4, ssView1_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 255);

            ssView1_Sheet1.Cells[6, 1, ssView1_Sheet1.RowCount - 1, 3].Text = "";
            ssView1_Sheet1.Cells[6, 1, ssView1_Sheet1.RowCount - 1, 3].BackColor = Color.FromArgb(192, 192, 192);

            ssView1_Sheet1.Cells[6, 4, ssView1_Sheet1.RowCount - 1, ssView1_Sheet1.ColumnCount - 1].Text = "";
            ssView1_Sheet1.Cells[6, 4, ssView1_Sheet1.RowCount - 1, ssView1_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 255);

            ssView5_Sheet1.Cells[0, 4, ssView5_Sheet1.RowCount - 1, ssView5_Sheet1.ColumnCount - 1].Text = "";
            ssView5_Sheet1.Cells[0, 4, ssView5_Sheet1.RowCount - 1, ssView5_Sheet1.ColumnCount - 1].BackColor = Color.White;

            ssView5_Sheet1.Cells[0, 1, ssView5_Sheet1.RowCount - 1, 3].BackColor = Color.FromArgb(192, 192, 192);
            ssView5_Sheet1.Cells[3, 3, 3, ssView5_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(192, 192, 192);

            ssView5_Sheet1.Cells[7, 3, 7, ssView5_Sheet1.ColumnCount - 1].Text = "";
            ssView5_Sheet1.Cells[7, 3, 7, ssView5_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(192, 192, 192);

            ssView5_Sheet1.Cells[11, 3, 11, ssView5_Sheet1.ColumnCount - 1].Text = "";
            ssView5_Sheet1.Cells[11, 3, 11, ssView5_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(192, 192, 192);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인
            btnSearchClick();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
                return;  //권한 확인
            if (btnSaveClick() == true)
            {
                //'/ 등록이후의 작업
                Screen_Clear();
                btnCancel.Enabled = true;
                btnExit.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bool boolFlag = false;

            if (cboDept.Enabled == true)
            {
                boolFlag = true;
            }

            Screen_Clear();
            cboYYMM.Enabled = true;
            if ((GstrWardCode == "GAN" || clsType.User.Sabun == "04349") && cboDept.Enabled == false)
            {
                cboDept.Enabled = true;
            }

            if ((GstrWardCode == "AN" || GstrWardCode == "OR") && cboDept.Enabled == false)
            {
                cboDept.Enabled = true;
            }

            if (GstrWardCode == "7W" && cboDept.Enabled == false)
            {
                cboDept.Enabled = true;
            }

            if (boolFlag == true)
            {
                cboDept.Enabled = true;
            }

            btnSearch.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnExit.Enabled = true;

            panHelp1List.Visible = false;
            txtHelpName.Text = "";
            btnHelp1.Visible = false;
            panHelpList.Visible = false;
            btnHelp.Visible = false;

            lblMsg.Text = "";
            txtSabun.Text = "";
            panSS1.Enabled = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            printSheet();

            if (ComFunc.LeftH(cboDept.Text, 2) == "3A")
            {
                cboDept.Text = "3A";
            }

            if (GstrWardCode != "OPD")
            {
                btnSearchClick();
            }
        }

        void printSheet()
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                return; //권한 확인

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            //string strHead2 = "";
            string strHead3 = "";
            string SysDate = "";
            Cursor.Current = Cursors.WaitCursor;

            SysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            ssPrint_Sheet1.Columns[0].Visible = false;
            ssPrint_Sheet1.Columns[1].Visible = false;
            ssPrint_Sheet1.Columns[44+1].Visible = false;

            ssPrint_Sheet1.RowCount = ssView1_Sheet1.NonEmptyRowCount;

            for (int i = 0; i < ssPrint_Sheet1.RowCount; i++)
            {
                for (int j = 0; j < ssPrint_Sheet1.ColumnCount; j++)
                {
                    ssPrint_Sheet1.Cells[i, j].Text = ssView1_Sheet1.Cells[i, j].Text;
                    ssPrint_Sheet1.Cells[i, j].BackColor = Color.White;

                    if (GstrWardCode != "OPD")
                    {
                        switch (ssPrint_Sheet1.Cells[i, j].Text.Trim())
                        {
                            case "D1":
                                ssPrint_Sheet1.Cells[i, j].Text = "D";
                                break;
                            case "E1":
                                ssPrint_Sheet1.Cells[i, j].Text = "E";
                                break;
                            case "N1":
                                ssPrint_Sheet1.Cells[i, j].Text = "N";
                                break;
                            case "SH":
                                ssPrint_Sheet1.Cells[i, j].Text = "S";
                                break;
                            case "D/L1":
                                ssPrint_Sheet1.Cells[i, j].Text = "D/L";
                                break;
                            case "E/L1":
                                ssPrint_Sheet1.Cells[i, j].Text = "E/L";
                                break;
                            case "N/L1":
                                ssPrint_Sheet1.Cells[i, j].Text = "N/L";
                                break;
                            case "S1":
                                ssPrint_Sheet1.Cells[i, j].Text = "S";
                                break;
                            case "SP":
                                ssPrint_Sheet1.Cells[i, j].Text = "D";
                                break;
                            case "SP1":
                                ssPrint_Sheet1.Cells[i, j].Text = "H/D";
                                break;
                            case "SHD":
                                ssPrint_Sheet1.Cells[i, j].Text = "E";
                                break;
                            case "SCSR":
                                ssPrint_Sheet1.Cells[i, j].Text = "E";
                                break;
                            default:
                                break;
                        }
                    }
                }
            }


            string strDateH2;
            strDateH2 = CF.READ_LASTDAY(clsDB.DbCon, VB.Left(cboYYMM.Text, 4) + "-" + VB.Mid(cboYYMM.Text, 6, 2) + "-01");

            nDateH2 = Convert.ToInt32(VB.Val(VB.Right(strDateH2, 2)));

            int nCol = 5;
            if (nDateH2 == 28)
            {
                ssPrint_Sheet1.Columns[nCol + 28 - 2].Visible = true;
                ssPrint_Sheet1.Columns[nCol + 29 - 2].Visible = false;
                ssPrint_Sheet1.Columns[nCol + 30 - 2].Visible = false;
                ssPrint_Sheet1.Columns[nCol + 31 - 2].Visible = false;
            }
            else if (nDateH2 == 29)
            {
                ssPrint_Sheet1.Columns[nCol + 28 - 2].Visible = true;
                ssPrint_Sheet1.Columns[nCol + 29 - 2].Visible = true;
                ssPrint_Sheet1.Columns[nCol + 30 - 2].Visible = false;
                ssPrint_Sheet1.Columns[nCol + 31 - 2].Visible = false;
            }
            else if (nDateH2 == 30)
            {
                ssPrint_Sheet1.Columns[nCol + 28 - 2].Visible = true;
                ssPrint_Sheet1.Columns[nCol + 29 - 2].Visible = true;
                ssPrint_Sheet1.Columns[nCol + 30 - 2].Visible = true;
                ssPrint_Sheet1.Columns[nCol + 31 - 2].Visible = false;
            }
            else if (nDateH2 == 31)
            {
                ssPrint_Sheet1.Columns[nCol + 28 - 2].Visible = true;
                ssPrint_Sheet1.Columns[nCol + 29 - 2].Visible = true;
                ssPrint_Sheet1.Columns[nCol + 30 - 2].Visible = true;
                ssPrint_Sheet1.Columns[nCol + 31 - 2].Visible = true;
            }

            ssPrint_Sheet1.ColumnHeader.Cells[0, 0, 0, ssPrint_Sheet1.ColumnCount - 2].Border = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.Black));

            ssPrint_Sheet1.SetRowHeight(-1, 19);

            ssPrint_Sheet1.PrintInfo.Preview = false;

            //ssPrint_Sheet1.PrintInfo.Preview = true;

            

            strFont1 = "/fn\"굴림체\" /fz\"16\" /fb0 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"10\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead1 = "/n/n/f1/C " + cboDept.Text + " 번표 " + "/n";
            //strHead2 = "/l/f2" + "작업년월 : " + VB.Left(cboYYMM.Text, 4) + "년 " + VB.Mid(cboYYMM.Text, 6, 2) + "월" + VB.Space(10) + "인쇄일자 : " + SysDate;

            string strLine1 = VB.Space(120) + "┌────────┐ " + "\n";
            string strLine2 = VB.Space(120) + "│병동 책임자 확인│" + "\n";
            string strLine3 = VB.Space(120) + "├────────┤" + "\n";
            string strLine4 = VB.Space(120) + "│　　　　　　　　│" + "\n";
            string strLine5 = VB.Space(120) + "│　　　　　　　　│" + "\n";
            string strLine6 = VB.Left("작업년월 : " + VB.Left(cboYYMM.Text, 4) + "년 " + VB.Mid(cboYYMM.Text, 6, 2) + "월" + VB.Space(10) + "인쇄일자 : " + SysDate + VB.Space(110), 110) + "└────────┘" + "\n";

            strHead3 = strLine1 + strLine2 + strLine3 + strLine4 + strLine5 + strLine6;

            ssPrint_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            //ssPrint_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead3 + strHead2;
            ssPrint_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead3 ;
            ssPrint_Sheet1.PrintInfo.Margin.Left = 0;
            ssPrint_Sheet1.PrintInfo.Margin.Right = 0;
            ssPrint_Sheet1.PrintInfo.Margin.Top = 35;
            ssPrint_Sheet1.PrintInfo.Margin.Bottom = 30;
            ssPrint_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssPrint_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssPrint_Sheet1.PrintInfo.ShowBorder = true;
            ssPrint_Sheet1.PrintInfo.ShowColor = false;
            ssPrint_Sheet1.PrintInfo.ShowGrid = true;
            ssPrint_Sheet1.PrintInfo.ShowShadows = false;
            ssPrint_Sheet1.PrintInfo.UseMax = false;
            ssPrint_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssPrint_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssPrint.PrintSheet(0);

            Cursor.Current = Cursors.Default;
        }

        void Total_Read()
        {
            int i = 0;
            int j = 0;
            int k = 0;
            int nDTot = 0;      //주간근무일 합계
            int nETot = 0;      //저녁근무일 합계
            int nNTot = 0;      //야간근무일 합계
            //2020-10-27 nOFFTot는 스케쥴상 OFF, nHUGATot는 결근계상 휴가
            double nOFFTot = 0;    //휴무, 휴가등 합계    
            double nHUGATot = 0;    //휴가등 합계

            string SSData = "";

            for (k = 4; k < ssView1_Sheet1.ColumnCount; k++)
            {
                nDTot = 0;
                nETot = 0;
                nNTot = 0;
                nOFFTot = 0;
                nHUGATot = 0;

                for (i = 6; i < ssView1_Sheet1.RowCount; i++)
                {
                    SSData = ssView1_Sheet1.Cells[i, k].Text.Trim();

                    #region 사용안함
                    //switch (SSData.Trim())
                    //{
                    //    case "D":
                    //    case "S":
                    //    case "D/E":
                    //    case "D/L":
                    //    case "H/D1":
                    //    case "H/D2":
                    //    case "교육":
                    //    case "출장":
                    //    case "D1":
                    //    case "D/L1":
                    //        nDTot = nDTot + 1;
                    //        break;
                    //    case "E":
                    //    case "C":
                    //    case "E/N":
                    //    case "L/E":
                    //    case "E1":
                    //    case "E/L1":
                    //        nETot = nETot + 1;
                    //        break;
                    //    case "N":
                    //    case "N/L":
                    //    case "N1":
                    //    case "N/L1":
                    //        nNTot = nNTot + 1;
                    //        break;
                    //    case "OFF":
                    //    case "휴무":
                    //    case "휴가":
                    //        nOFFTot = nOFFTot + 1;
                    //        break;
                    //} 
                    #endregion

                    switch (SSData.Trim())
                    {
                        case "D":
                        case "S":
                        case "D/E":
                        case "D/L":
                        case "H/D1":
                        case "H/D2":
                        case "교육":
                        case "출장":
                        case "D1":
                        case "D/L1":
                            nDTot = nDTot + 1;
                            break;
                        case "E":
                        case "C":
                        case "E/N":
                        case "L/E":
                        case "E1":
                        case "E/L1":
                            nETot = nETot + 1;
                            break;
                        case "N":
                        case "N/L":
                        case "N1":
                        case "N/L1":
                            nNTot = nNTot + 1;
                            break;
                        case "OFF":
                            nOFFTot = nOFFTot + 1;
                            break;
                        case "휴가":
                            nHUGATot = nHUGATot + 1;
                            break;
                    }

                    if (SSData.Trim() == "H/S" || SSData.Trim() == "H/S1" || SSData.Trim() == "H/S2" || SSData.Trim() == "H/D" || SSData.Trim() == "H/D2" || SSData.Trim() == "H/E1"
                         || SSData.Trim() == "S/8" || SSData.Trim() == "H/S" || SSData.Trim() == "MOFF")
                    {
                        nOFFTot = nOFFTot + 0.5;
                    }
                    if (SSData.Trim() == "MOFF" || SSData.Trim() == "반휴")
                    {
                        nHUGATot = nHUGATot + 0.5;
                    }
                }

                for (j = 1; j <= 4; j++)
                {
                    switch (j)
                    {
                        case 1:
                            ssView1_Sheet1.Cells[j, k].Text = nDTot.ToString();
                            break;
                        case 2:
                            ssView1_Sheet1.Cells[j, k].Text = nETot.ToString();
                            break;
                        case 3:
                            ssView1_Sheet1.Cells[j, k].Text = nNTot.ToString();
                            break;
                        case 4:
                            ssView1_Sheet1.Cells[j, k].Text = nOFFTot.ToString();
                            break;
                    }
                }
            }


            for (i = 7; i < ssView1_Sheet1.RowCount; i++)
            {
                nDTot = 0;
                nETot = 0;
                nNTot = 0;
                nOFFTot = 0;
                nHUGATot = 0;

                for (k = 4; k < 35; k++)
                {
                    SSData = ssView1_Sheet1.Cells[i, k].Text.Trim();

                    switch (SSData.Trim())
                    {
                        case "D":
                        case "S":
                        case "D/E":
                        case "D/L":
                        case "H/D1":
                        case "H/D2":
                        case "교육":
                        case "출장":
                        case "D1":
                        case "D/L1":
                            nDTot = nDTot + 1;
                            break;
                        case "E":
                        case "C":
                        case "E/N":
                        case "L/E":
                        case "E1":
                        case "E/L1":
                            nETot = nETot + 1;
                            break;
                        case "N":
                        case "N/L":
                        case "N1":
                        case "N/L1":
                            nNTot = nNTot + 1;
                            break;
                        case "OFF":
                            nOFFTot = nOFFTot + 1;
                            break;
                        case "휴가":
                            nHUGATot = nHUGATot + 1;
                            break;
                    }

                    if (SSData.Trim() == "H/S" || SSData.Trim() == "H/S1" || SSData.Trim() == "H/S2" || SSData.Trim() == "H/D" || SSData.Trim() == "H/D2" || SSData.Trim() == "H/E1"
                         || SSData.Trim() == "S/8" || SSData.Trim() == "H/S" || SSData.Trim() == "MOFF")
                    {
                        nOFFTot = nOFFTot + 0.5;
                    }
                    if (SSData.Trim() == "MOFF" || SSData.Trim() == "반휴")
                    {
                        nHUGATot = nHUGATot + 0.5;
                    }
                }

                ssView1_Sheet1.Cells[i, 35].Text = nDTot.ToString();
                ssView1_Sheet1.Cells[i, 36].Text = nETot.ToString();
                ssView1_Sheet1.Cells[i, 37].Text = nNTot.ToString();
                ssView1_Sheet1.Cells[i, 38].Text = nOFFTot.ToString();
                ssView1_Sheet1.Cells[i, 44].Text = nHUGATot.ToString();
            }



            //60병동 청구를 위해 스케쥴 통계

            int nDTot_NUR = 0;            //주간근무일 합계      간호사
            int nETot_NUR = 0;            //저녁근무일 합계
            int nNTot_NUR = 0;            //야간근무일 합계
            int nOFFTot_NUR = 0;            //휴무,휴가등 합계

            int nDTot_NUJ = 0;            //주간근무일 합계     조무사
            int nETot_NUJ = 0;            //저녁근무일 합계
            int nNTot_NUJ = 0;            //야간근무일 합계
            int nOFFTot_NUJ = 0;            //휴무,휴가등 합계

            int nDTot_NUH = 0;            //주간근무일 합계    간병인
            int nETot_NUH = 0;            //저녁근무일 합계
            int nNTot_NUH = 0;            //야간근무일 합계
            int nOFFTot_NUH = 0;            //휴무,휴가등 합계
            string SSTEMP = "";

            for (k = 4; k < 35; k++)
            {
                nDTot_NUR = 0;
                nETot_NUR = 0;
                nNTot_NUR = 0;
                nOFFTot_NUR = 0;
                nDTot_NUJ = 0;
                nETot_NUJ = 0;
                nNTot_NUJ = 0;
                nOFFTot_NUJ = 0;
                nDTot_NUH = 0;
                nETot_NUH = 0;
                nNTot_NUH = 0;
                nOFFTot_NUH = 0;

                for (i = 6; i < ssView1_Sheet1.RowCount; i++)
                {
                    SSData = ssView1_Sheet1.Cells[i, 34].Text.Trim();
                    SSTEMP = ssView1_Sheet1.Cells[i, 3].Text.Trim();

                    if (SSTEMP.IndexOf("간호사") > -1)
                    {
                        if (SSData == "D" || SSData == "S" || SSData == "D/E" || SSData == "D/L" || SSData == "H/D1" || SSData == "H/D2" || SSData == "교육" || SSData == "출장" || SSData == "D1" || SSData == "D/L1")
                        {
                            nDTot_NUR++;
                        }
                        else if (SSData == "E" || SSData == "C" || SSData == "E/N" || SSData == "L/E" || SSData == "E1" || SSData == "E/L1")
                        {
                            nETot_NUR++;
                        }
                        else if (SSData == "N" || SSData == "N/L" || SSData == "N1" || SSData == "N/L1")
                        {
                            nNTot_NUR++;
                        }
                        else if (SSData == "OFF" || SSData == "휴무" || SSData == "휴가")
                        {
                            nOFFTot_NUR++;
                        }
                    }
                    else if (SSTEMP.IndexOf("조무사") > -1)
                    {
                        if (SSData == "D" || SSData == "S" || SSData == "D/E" || SSData == "D/L" || SSData == "H/D1" || SSData == "H/D2" || SSData == "교육" || SSData == "출장" || SSData == "D1" || SSData == "D/L1")
                        {
                            nDTot_NUJ++;
                        }
                        else if (SSData == "E" || SSData == "C" || SSData == "E/N" || SSData == "L/E" || SSData == "E1" || SSData == "E/L1")
                        {
                            nETot_NUJ++;
                        }
                        else if (SSData == "N" || SSData == "N/L" || SSData == "N1" || SSData == "N/L1")
                        {
                            nNTot_NUJ++;
                        }
                        else if (SSData == "OFF" || SSData == "휴무" || SSData == "휴가")
                        {
                            nOFFTot_NUJ++;
                        }
                    }
                    else if (SSTEMP == "")
                    {
                        if (SSData == "D" || SSData == "S" || SSData == "D/E" || SSData == "D/L" || SSData == "H/D1" || SSData == "H/D2" || SSData == "교육" || SSData == "출장" || SSData == "D1" || SSData == "D/L1")
                        {
                            nDTot_NUH++;
                        }
                        else if (SSData == "E" || SSData == "C" || SSData == "E/N" || SSData == "L/E" || SSData == "E1" || SSData == "E/L1")
                        {
                            nETot_NUH++;
                        }
                        else if (SSData == "N" || SSData == "N/L" || SSData == "N1" || SSData == "N/L1")
                        {
                            nNTot_NUH++;
                        }
                        else if (SSData == "OFF" || SSData == "휴무" || SSData == "휴가")
                        {
                            nOFFTot_NUH++;
                        }
                    }
                }

                for (j = 0; j < 13; j++)
                {
                    switch (j)
                    {
                        case 0:
                            ssView5_Sheet1.Cells[j, k].Text = nDTot_NUR.ToString();
                            break;
                        case 1:
                            ssView5_Sheet1.Cells[j, k].Text = nETot_NUR.ToString();
                            break;
                        case 2:
                            ssView5_Sheet1.Cells[j, k].Text = nNTot_NUR.ToString();
                            break;
                        case 3:
                            ssView5_Sheet1.Cells[j, k].Text = nOFFTot_NUR.ToString();
                            break;
                        case 4:
                            ssView5_Sheet1.Cells[j, k].Text = nDTot_NUJ.ToString();
                            break;
                        case 5:
                            ssView5_Sheet1.Cells[j, k].Text = nETot_NUJ.ToString();
                            break;
                        case 6:
                            ssView5_Sheet1.Cells[j, k].Text = nNTot_NUJ.ToString();
                            break;
                        case 7:
                            ssView5_Sheet1.Cells[j, k].Text = nOFFTot_NUJ.ToString();
                            break;
                        case 8:
                            ssView5_Sheet1.Cells[j, k].Text = nDTot_NUH.ToString();
                            break;
                        case 9:
                            ssView5_Sheet1.Cells[j, k].Text = nETot_NUH.ToString();
                            break;
                        case 10:
                            ssView5_Sheet1.Cells[j, k].Text = nNTot_NUH.ToString();
                            break;
                        case 11:
                            ssView5_Sheet1.Cells[j, k].Text = nOFFTot_NUH.ToString();
                            break;
                        case 12:
                            ssView5_Sheet1.Cells[j, k].Text = (nDTot_NUR + nETot_NUR + nNTot_NUR + nDTot_NUJ + nETot_NUJ + nNTot_NUJ + nDTot_NUH + nETot_NUH + nNTot_NUH).ToString();
                            break;
                    }
                }
            }
        }

        private void btnAutoInput_Click(object sender, EventArgs e)
        {
            //FrmAutoInput
            frmAutoInput frm = new frmAutoInput();
            frm.ShowDialog();
        }

        private void btnSearchClick()
        {

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            int i = 0;
            int j = 0;
            //int K = 0;
            string strYY = "";         //'콤보 박스 YY
            string strMM = "";         //'콤보 박스 MM
            string strDate = "";       //'날짜 비교
            string strDateH1 = "";
            string strDateH2 = "";
            string strDay = "";        //'월의 처음 날짜
            string strHDay = "";       //'월의 처음 날짜 (휴일 Check)
            string strOFFDay = "";     //'월의 처음 날짜 (AN,OR 휴일 OFF Check)
            string strDay1 = "";       //'월의 처음 요일
            int nCol = 0;
            int nCol2 = 0;
            int nCol3 = 0;
            int nCol4 = 0;
            //int nRow = 0;               //'SS1.MaxRows 비교용
            int nTEMP = 0;              //'Data Display-Column 변수
            string strBuse = "";
            int nHDay = 0;              //'휴일인 날짜
            string strNewday = "";
            //int nRowCount = 0;
            string str3A = "";          //'3A병동구분용(O,C)

            int nCnt = 0;               //'해당월 카운트
            int nCnt2 = 0;              //'지난월 카운트

            string strBYYMM = "";
            string strSTATUS = "";

            ssOpdMiss_Sheet1.Visible = false;

            cboYYMM.Enabled = false;
            //SSFrame4.Enabled = False

            if (VB.Trim(cboDept.Text) == "외래" || VB.Trim(cboDept.Text) == "DOCTOR" || VB.Trim(cboDept.Text) == "OR" || VB.Trim(cboDept.Text) == "마취과")
            {
                //SSopd.Visible = True
                //SS2.Visible = False
            }
            else
            {
                //SS2.Visible = True
                //SSopd.Visible = False
            }

            if (GstrWardCode == "GAN" || clsType.User.Sabun == "04349")
                cboDept.Enabled = false;
            if (GstrWardCode == "AN" || GstrWardCode == "OR")
                cboDept.Enabled = false;
            if (GstrWardCode == "7W")
                cboDept.Enabled = false;

            btnSearch.Enabled = false;
            Screen_Clear();
            panSS1.Enabled = true;
            btnHelp.Visible = true;
            btnHelp1.Visible = true;


            strNewday = VB.Left(clsPublic.GstrSysDate, 4) + VB.Mid(clsPublic.GstrSysDate, 6, 2);
            strYY = VB.Left(cboYYMM.Text, 4);
            strMM = VB.Mid(cboYYMM.Text, 6, 2);
            strDay = strYY + "-" + strMM + "-01";
            strHDay = strYY + "-" + strMM + "-01";
            strOFFDay = strYY + "-" + strMM + "-01";
            strDate = strYY + "-" + strMM + "-01";
            strDateH1 = strDate;
            strDate = strYY + "-" + strMM + "-01";

            strDateH2 = CF.READ_LASTDAY(clsDB.DbCon, strDate);

            nDateH1 = Convert.ToInt32(VB.Val(VB.Right(strDateH1, 2)));
            nDateH2 = Convert.ToInt32(VB.Val(VB.Right(strDateH2, 2)));


            nCol = 5;
            if (nDateH2 == 28)
            {
                ssView1_Sheet1.Columns[nCol + 28 - 2].Visible = true;
                ssView1_Sheet1.Columns[nCol + 29 - 2].Visible = false;
                ssView1_Sheet1.Columns[nCol + 30 - 2].Visible = false;
                ssView1_Sheet1.Columns[nCol + 31 - 2].Visible = false;
            }
            else if (nDateH2 == 29)
            {
                ssView1_Sheet1.Columns[nCol + 28 - 2].Visible = true;
                ssView1_Sheet1.Columns[nCol + 29 - 2].Visible = true;
                ssView1_Sheet1.Columns[nCol + 30 - 2].Visible = false;
                ssView1_Sheet1.Columns[nCol + 31 - 2].Visible = false;
            }
            else if (nDateH2 == 30)
            {
                ssView1_Sheet1.Columns[nCol + 28 - 2].Visible = true;
                ssView1_Sheet1.Columns[nCol + 29 - 2].Visible = true;
                ssView1_Sheet1.Columns[nCol + 30 - 2].Visible = true;
                ssView1_Sheet1.Columns[nCol + 31 - 2].Visible = false;
            }
            else if (nDateH2 == 31)
            {
                ssView1_Sheet1.Columns[nCol + 28 - 2].Visible = true;
                ssView1_Sheet1.Columns[nCol + 29 - 2].Visible = true;
                ssView1_Sheet1.Columns[nCol + 30 - 2].Visible = true;
                ssView1_Sheet1.Columns[nCol + 31 - 2].Visible = true;
            }
            if (GstrWardCode == "ER")
            {
                strBuse = "ER";
            }
            else
            {
                strBuse = clsOpdNr.READ_BUSECODE(clsDB.DbCon, VB.Trim(cboDept.Text));
            }
            DateTime dtTemp = Convert.ToDateTime(VB.Trim(VB.Left(cboYYMM.Text, 4) + "-" + VB.Mid(cboYYMM.Text, 6, 2) + "-01")).AddDays(-1);
            strBYYMM = dtTemp.ToString("yyyyMM");

            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT COUNT(*) CNT";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_SCHEDULE1 a, KOSMOS_PMPA.NUR_CODE b ";
                SQL = SQL + ComNum.VBLF + " WHERE a.WardCode = '" + strBuse + "' ";
                SQL = SQL + ComNum.VBLF + "  AND a.YYMM = '" + strBYYMM + "' ";  //'전월
                SQL = SQL + ComNum.VBLF + "  AND a.JikCode = b.Code ";
                SQL = SQL + ComNum.VBLF + "  AND b.Gubun = '1' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    btnSearch.Enabled = true;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nCnt2 = Convert.ToInt32(VB.Val(dt.Rows[0]["CNT"].ToString().Trim()));
                }

                dt.Dispose();
                dt = null;


                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT COUNT(*) CNT";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_SCHEDULE1 ";
                SQL = SQL + ComNum.VBLF + " WHERE YYMM = '" + VB.Trim(VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 6, 2)) + "' ";  //'당월
                SQL = SQL + ComNum.VBLF + "  AND WardCode = '" + strBuse + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnCancel.Enabled = true;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    btnSearch.Enabled = true;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nCnt = Convert.ToInt32(VB.Val(dt.Rows[0]["CNT"].ToString().Trim()));
                }

                dt.Dispose();
                dt = null;

                if (nCnt2 > 0)
                {
                    if (nCnt == 0)
                    {
                        strSTATUS = "NEW";
                    }
                    
                    else if (((double)nCnt / nCnt2) * 100 < 20 || strBuse == "DT")  //2019-01-30 치과 예외 처리  
                    {
                        strSTATUS = "CALC";
                    }
                    else
                    {
                        strSTATUS = "UPDATE";
                    }
                }
                else
                {
                    if (nCnt == 0)
                    {
                        strSTATUS = "NEW";
                    }
                    else
                    {
                        strSTATUS = "UPDATE";
                    }
                }


                //'월의 요일 Check
                nCol2 = 5;
                if (GstrWardCode == "OPD")
                {
                    SQL = "Select TO_CHAR(TO_DATE('" + strDay + "', 'YYYY-MM-DD'),'DY') cWeek FROM DUAL ";
                }
                else
                {
                    SQL = "Select TO_CHAR(TO_DATE('" + strDay + "', 'YYYY-MM-DD'),'d') cWeek FROM DUAL ";
                }
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    btnSearch.Enabled = true;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strDay1 = dt.Rows[0]["cWeek"].ToString().Trim();

                    if (GstrWardCode == "OPD")
                    {
                        switch (strDay1)
                        {
                            case "SUN":
                            case "일":
                                strDay1 = "1";
                                break;
                            case "MON":
                            case "월":
                                strDay1 = "2";
                                break;
                            case "TUE":
                            case "화":
                                strDay1 = "3";
                                break;
                            case "WED":
                            case "수":
                                strDay1 = "4";
                                break;
                            case "THU":
                            case "목":
                                strDay1 = "5";
                                break;
                            case "FRI":
                            case "금":
                                strDay1 = "6";
                                break;
                            case "SAT":
                            case "토":
                                strDay1 = "7";
                                break;
                        }
                    }
                }

                dt.Dispose();
                dt = null;


                for (i = nDateH1; i <= nDateH2; i++)
                {
                    switch (strDay1)
                    {
                        case "1":
                            ssView1_Sheet1.Cells[0, nCol2 - 1].Text = "일";
                            break;
                        case "2":
                            ssView1_Sheet1.Cells[0, nCol2 - 1].Text = "월";
                            break;
                        case "3":
                            ssView1_Sheet1.Cells[0, nCol2 - 1].Text = "화";
                            break;
                        case "4":
                            ssView1_Sheet1.Cells[0, nCol2 - 1].Text = "수";
                            break;
                        case "5":
                            ssView1_Sheet1.Cells[0, nCol2 - 1].Text = "목";
                            break;
                        case "6":
                            ssView1_Sheet1.Cells[0, nCol2 - 1].Text = "금";
                            break;
                        case "7":
                            ssView1_Sheet1.Cells[0, nCol2 - 1].Text = "토";
                            break;
                    }
                    nCol2 = nCol2 + 1;
                    strDay1 = (VB.Val(strDay1) + 1).ToString().Trim();
                    if (VB.Val(strDay1) > 7)
                        strDay1 = "1";
                }

                //if (VB.Val(VB.Trim(VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2))) >= 200309)
                if (VB.Val(VB.Trim(VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 6, 2))) >= 200309) //2018-12-24
                {
                    if (cboDept.Text == "3A")
                    {
                        if (cbo3A.SelectedIndex == 0)
                        {
                            str3A = "O";
                        }
                        else if (cbo3A.SelectedIndex == 1)
                        {
                            str3A = "C";
                        }
                    }
                }


                //'해당 부서에 해당하는 직원을 DisPlay
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT YYMM,WardCode,Sabun,SName,JikCode,Schedule,DTot,";
                SQL = SQL + ComNum.VBLF + " ETot,NTot,OffTot,Gubun,ROWID, RANK ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_SCHEDULE1 A";
                SQL = SQL + ComNum.VBLF + " WHERE YYMM = '" + VB.Trim(VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 6, 2)) + "' ";     //'당월
                SQL = SQL + ComNum.VBLF + "  AND WardCode = '" + strBuse + "' ";

                if (txtSabun.Text != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND ( SABUN = '" + txtSabun.Text + "' or SABUN IN ( SELECT SABUN FROM KOSMOS_ADM.INSA_MST WHERE KORNAME like '%" + txtSabun.Text + "%')  ) ";
                }

                if (VB.Val(VB.Trim(VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 6, 2))) <= 201002)
                {
                    if (VB.Val(VB.Trim(VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 6, 2))) >= 200309)
                    {
                        if (cboDept.Text == "3A" && cbo3A.Text != "")
                            SQL = SQL + ComNum.VBLF + " AND Gubun = '" + str3A + "' ";
                    }
                }

                //'2017-10-30 해당월에 퇴사하거나 재직중인 사람만 조회되도록 변경함.
                SQL = SQL + ComNum.VBLF + "  AND EXISTS ( SELECT * FROM KOSMOS_ADM.INSA_MST B";
                SQL = SQL + ComNum.VBLF + "                WHERE A.SABUN = B.SABUN";
                SQL = SQL + ComNum.VBLF + "                  AND (B.TOIDAY IS NULL OR";
                SQL = SQL + ComNum.VBLF + "                       B.TOIDAY >= TO_DATE('" + VB.Trim(VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 6, 2) + "01") + "','YYYY-MM-DD'))";
                if (GstrWardCode == "ER")
                {
                    switch (cboDept.Text.Trim())
                    {
                        case "ER간호사/조무사":
                            SQL = SQL + ComNum.VBLF + "     AND B.BUSE = '033109' ";
                            break;
                        case "ER구조사":
                            SQL = SQL + ComNum.VBLF + "     AND B.BUSE = '100490' ";
                            break;
                        default:
                            SQL = SQL + ComNum.VBLF + "     AND B.BUSE IN ('033109','100490') ";
                            break;
                    }

                }
                SQL = SQL + ComNum.VBLF + "              )";


                if (VB.Trim(cboDept.Text) == "DOCTOR")
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY A.RANK ASC, A.JikCode, A.SName, A.Sabun ";
                }
                else
                {
                    if (chk_Sort.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + " ORDER BY  A.JikCode, A.Sabun, A.SName ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " ORDER BY A.RANK ASC, A.JikCode, A.Sabun, A.SName ";
                    }
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    btnSearch.Enabled = true;
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    if (cboDept.Text == "DOCTOR")
                    {
                        //GoSub
                        BAS_DOCTOR_READ();
                    }
                    else
                    {
                        if (GstrWardCode == "ER")
                        {
                            strBuse = "ER";
                        }
                        else
                        {
                            strBuse = ComboDept_Code();
                            strBuse = clsOpdNr.READ_BUSECODE(clsDB.DbCon, VB.Trim(cboDept.Text));
                        }

                        

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT a.YYMM,a.WardCode,a.Sabun,a.SName,a.JikCode,b.Printranking,a.Gubun ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_SCHEDULE1 a, KOSMOS_PMPA.NUR_CODE b ";
                        SQL = SQL + ComNum.VBLF + " WHERE a.WardCode = '" + strBuse + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND a.YYMM = '" + strBYYMM + "' ";  //'전월
                        SQL = SQL + ComNum.VBLF + "  AND a.JikCode = b.Code ";
                        SQL = SQL + ComNum.VBLF + "  AND b.Gubun = '1' ";

                        if (GstrWardCode != "OPD")
                        {
                            //2017-10-30 해당월에 퇴사하거나 재직중인 사람만 조회되도록 변경함.
                            SQL = SQL + ComNum.VBLF + "  AND EXISTS ( SELECT * FROM KOSMOS_ADM.INSA_MST B";
                            SQL = SQL + ComNum.VBLF + "                WHERE A.SABUN = B.SABUN";
                            SQL = SQL + ComNum.VBLF + "                  AND (B.TOIDAY IS NULL OR";
                            SQL = SQL + ComNum.VBLF + "                       B.TOIDAY >= TO_DATE('" + (VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 6, 2) + "01").Trim() + "','YYYY-MM-DD'))";
                            //최초 조회 시 응급실 구분 상관없이 ER 전체 조회
                            //if (GstrWardCode == "ER")
                            //{
                            //    switch (cboDept.Text.Trim())
                            //    {
                            //        case "ER간호사/조무사":
                            //            SQL = SQL + ComNum.VBLF + "     AND B.BUSE = '033109' ";
                            //            break;
                            //        case "ER구조사":
                            //            SQL = SQL + ComNum.VBLF + "     AND B.BUSE = '100490' ";
                            //            break;
                            //        default:
                            //            SQL = SQL + ComNum.VBLF + "     AND B.BUSE IN ('033109','100490') ";
                            //            break;
                            //    }

                            //}
                            SQL = SQL + ComNum.VBLF + "              )";
                        }




                        if (txtSabun.Text != "")
                        {
                            SQL = SQL + ComNum.VBLF + " AND ( A.SABUN = '" + txtSabun.Text +
                                                      "' or A.SABUN IN ( SELECT SABUN FROM KOSMOS_ADM.INSA_MST WHERE KORNAME like '%" + txtSabun.Text + "%')  ) ";
                        }

                        if (VB.Val(VB.Trim(VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 6, 2))) <= 201002)
                        {
                            if (VB.Val(VB.Trim(VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 6, 2))) >= 200309)
                            {
                                if (cboDept.Text == "3A" && cbo3A.Text != "")
                                    SQL = SQL + ComNum.VBLF + " AND a.Gubun = '" + str3A + "' ";
                            }
                        }

                        if (GstrWardCode == "OPD")
                        {
                            SQL = SQL + ComNum.VBLF + " ORDER BY  JikCode, Sabun ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + " ORDER BY PrintRanking, JikCode, Sabun ";
                        }

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            btnSearch.Enabled = true;
                            return;
                        }

                        if (dt.Rows.Count == 0)
                        {
                            dt.Dispose();
                            dt = null;
                            btnSearch.Enabled = true;
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            ssView1_Sheet1.RowCount = dt.Rows.Count + 7;

                            for (i = 0; i < dt.Rows.Count; i++)
                            {
                                //if (ssView1_Sheet1.RowCount <= i + 7)
                                //{
                                //    ssView1_Sheet1.RowCount = i + 7;
                                //}

                                if (GstrWardCode == "ER")
                                {
                                    ssView1_Sheet1.Cells[i + 7, 0].Text = "ER";
                                }
                                else
                                {
                                    ssView1_Sheet1.Cells[i + 7, 0].Text = cboDept.Text.Trim();
                                }
                                ssView1_Sheet1.Cells[i + 7, 1].Text = dt.Rows[i]["SABUN"].ToString().Trim();
                                ssView1_Sheet1.Cells[i + 7, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                                ssView1_Sheet1.Cells[i + 7, 3].Text = Read_JikName(dt.Rows[i]["JIKCODE"].ToString().Trim());

                                ssView1_Sheet1.Cells[i + 7, 44+1].Text = (i + 1).ToString();
                            }
                        }
                        dt.Dispose();
                        dt = null;

                        //'마취과 또는 OR인 경우에는 휴일에 OFF DisPlay
                        nCol4 = 4;
                        if (VB.Trim(cboDept.Text) == "마취과" || VB.Trim(cboDept.Text) == "OR" || VB.Trim(cboDept.Text) == "외래")
                        {
                            for (i = nDateH1; i <= nDateH2; i++)
                            {
                                SQL = " Select TO_CHAR(JobDate,'YYYY-MM-DD') JOBDATE  ";
                                SQL = SQL + ComNum.VBLF + "  FROM BAS_JOB ";
                                SQL = SQL + ComNum.VBLF + " WHERE HolyDay = '*' ";
                                SQL = SQL + ComNum.VBLF + "   AND JOBDATE <>TO_DATE('2014-06-04','YYYY-MM-DD') ";
                                SQL = SQL + ComNum.VBLF + "   AND JobDate = TO_DATE('" + strOFFDay + "','YYYY-MM-DD') ";

                                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    btnSearch.Enabled = true;
                                    return;
                                }

                                if (dt.Rows.Count > 0)
                                {
                                    for (j = 0; j < dt.Rows.Count; j++)
                                    {
                                        nHDay = Convert.ToInt32(VB.Val(VB.Right(dt.Rows[j]["JobDate"].ToString().Trim(), 2)));
                                        ssView1_Sheet1.Cells[6, nCol4 + nHDay - 1, ssView1_Sheet1.RowCount - 1, nCol4 + nHDay - 1].Text = "OFF";
                                    }
                                }

                                strOFFDay = Convert.ToDateTime(strOFFDay).AddDays(1).ToShortDateString();
                            }
                        }
                    }
                }
                else if (strSTATUS == "UPDATE")
                {
                    ssView1_Sheet1.RowCount = dt.Rows.Count + 7;
                    ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView1_Sheet1.Cells[i + 7, 0].Text = clsOpdNr.Read_BuseName(clsDB.DbCon, dt.Rows[i]["WardCode"].ToString().Trim());
                        ssView1_Sheet1.Cells[i + 7, 1].Text = dt.Rows[i]["SABUN"].ToString().Trim();
                        ssView1_Sheet1.Cells[i + 7, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView1_Sheet1.Cells[i + 7, 3].Text = Read_JikName(dt.Rows[i]["JIKCODE"].ToString().Trim());
                        ssView1_Sheet1.Cells[i + 7, 4].Text = dt.Rows[i]["SCHEDULE"].ToString();

                        nCol = 4;
                        nTEMP = 0;
                        for (j = nDateH1; j <= nDateH2; j++)
                        {
                            ssView1_Sheet1.Cells[i + 7, nCol].Text = clsNurse.MidH(dt.Rows[i]["SCHEDULE"].ToString(), nTEMP, 4).Trim();

                            nCol = nCol + 1;
                            nTEMP = nTEMP + 4;
                        }

                        if (nDateH2 == 28)
                        {
                            nCol = nCol + 3;
                        }
                        else if (nDateH2 == 29)
                        {
                            nCol = nCol + 2;
                        }
                        else if (nDateH2 == 30)
                        {
                            nCol = nCol + 1;
                        }

                        ssView1_Sheet1.Cells[i + 7, nCol + 4].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssView1_Sheet1.Cells[i + 7, nCol + 5].Text = clsOpdNr.Read_BuseName(clsDB.DbCon, dt.Rows[i]["WardCode"].ToString().Trim());
                        ssView1_Sheet1.Cells[i + 7, nCol + 6].Text = Read_JikName(dt.Rows[i]["JIKCODE"].ToString().Trim());
                        ssView1_Sheet1.Cells[i + 7, nCol + 7].Text = dt.Rows[i]["SCHEDULE"].ToString();

                        ssView1_Sheet1.Cells[i + 7, 44+1].Text = (i + 1).ToString();
                        ssView1_Sheet1.Cells[i + 7, 45+1].Text = (i + 1).ToString();

                        //if (GstrWardCode == "OPD")
                        //{
                        //    ssView1_Sheet1.Cells[i + 7, 44].Text = dt.Rows[i]["RANK"].ToString().Trim();
                        //    ssView1_Sheet1.Cells[i + 7, 45].Text = dt.Rows[i]["RANK"].ToString().Trim();
                        //}
                        //else
                        //{
                        //    if (txtSabun.Text != "")
                        //    {
                        //        ssView1_Sheet1.Cells[i + 7, 44].Text = dt.Rows[i]["RANK"].ToString().Trim();
                        //    }
                        //    else
                        //    {
                        //        ssView1_Sheet1.Cells[i + 7, 45].Text = dt.Rows[i]["RANK"].ToString().Trim();
                        //    }
                        //}
                    }
                }

                if (strSTATUS == "CALC")
                {
                    if (GstrWardCode == "ER")
                    {
                        strBuse = "ER";
                    }
                    else
                    {
                        strBuse = ComboDept_Code();
                        strBuse = clsOpdNr.READ_BUSECODE(clsDB.DbCon, VB.Trim(cboDept.Text));
                    }
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT a.YYMM,a.WardCode,a.Sabun,a.SName,a.JikCode,b.Printranking,a.Gubun, '1' GUBUN2, '' SCHEDULE, A.ROWID,a.rank";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_SCHEDULE1 a, KOSMOS_PMPA.NUR_CODE b";
                    SQL = SQL + ComNum.VBLF + "  WHERE a.WardCode = '" + strBuse + "'";
                    SQL = SQL + ComNum.VBLF + "   AND a.YYMM = '" + strBYYMM + "'";
                    if(strBYYMM == "201911")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND A.SABUN NOT IN ('48560') ";
                    }
                    SQL = SQL + ComNum.VBLF + "   AND a.JikCode = b.Code";
                    SQL = SQL + ComNum.VBLF + "   AND b.Gubun = '1'";
                    if (txtSabun.Text != "")
                    {
                        SQL = SQL + ComNum.VBLF + " AND ( a.SABUN = '" + txtSabun.Text + "' or a.Sname like '%" + txtSabun.Text + "%' ) ";
                    }
                    SQL = SQL + ComNum.VBLF + "   AND NOT EXISTS";
                    SQL = SQL + ComNum.VBLF + "   (SELECT * FROM KOSMOS_PMPA.NUR_SCHEDULE1 SUB";
                    SQL = SQL + ComNum.VBLF + "   WHERE A.WARDCODE = SUB.WARDCODE";
                    SQL = SQL + ComNum.VBLF + "   AND A.SABUN = SUB.SABUN";
                    SQL = SQL + ComNum.VBLF + "   AND A.YYMM = '" + strBYYMM + "'";
                    SQL = SQL + ComNum.VBLF + "   AND SUB.YYMM = '" + VB.Trim(VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 6, 2)) + "')";
                    SQL = SQL + ComNum.VBLF + " Union All";
                    SQL = SQL + ComNum.VBLF + " SELECT a.YYMM,a.WardCode,a.Sabun,a.SName,a.JikCode,b.Printranking,a.Gubun, '2' GUBUN2, SCHEDULE, A.ROWID,a.rank";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_SCHEDULE1 a, KOSMOS_PMPA.NUR_CODE b";
                    SQL = SQL + ComNum.VBLF + "  WHERE a.WardCode = '" + strBuse + "'";
                    SQL = SQL + ComNum.VBLF + "   AND a.YYMM = '" + VB.Trim(VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 6, 2)) + "'";
                    SQL = SQL + ComNum.VBLF + "   AND a.JikCode = b.Code";
                    SQL = SQL + ComNum.VBLF + "   AND b.Gubun = '1'";
                    if (txtSabun.Text != "")
                    {
                        SQL = SQL + ComNum.VBLF + " AND ( a.SABUN = '" + txtSabun.Text + "' or a.Sname like '%" + txtSabun.Text + "%' ) ";
                    }

                    if (GstrWardCode == "OPD")
                    {
                        SQL = SQL + ComNum.VBLF + " ORDER BY  JikCode, Sabun ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " ORDER BY  RANK, JikCode, Sabun ";
                    }

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        btnSearch.Enabled = true;
                        return;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        btnSearch.Enabled = true;
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        ssView1_Sheet1.RowCount = dt.Rows.Count + 7;
                        ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            if (GstrWardCode == "ER")
                            {
                                ssView1_Sheet1.Cells[i + 7, 0].Text = "ER";
                            }
                            else
                            {
                                ssView1_Sheet1.Cells[i + 7, 0].Text = VB.Trim(cboDept.Text);
                            }
                            ssView1_Sheet1.Cells[i + 7, 1].Text = dt.Rows[i]["SABUN"].ToString().Trim();
                            ssView1_Sheet1.Cells[i + 7, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                            ssView1_Sheet1.Cells[i + 7, 3].Text = Read_JikName(dt.Rows[i]["JIKCODE"].ToString().Trim());

                            if (dt.Rows[i]["Gubun2"].ToString().Trim() == "2")
                            {
                                ssView1_Sheet1.Cells[i + 7, 4].Text = dt.Rows[i]["SCHEDULE"].ToString();

                                nCol = 4;
                                nTEMP = 0;
                                for (j = nDateH1; j <= nDateH2; j++)
                                {
                                    ssView1_Sheet1.Cells[i + 7, nCol].Text = clsNurse.MidH(dt.Rows[i]["SCHEDULE"].ToString(), nTEMP, 4);
                                    nCol = nCol + 1;
                                    nTEMP = nTEMP + 4;
                                }

                                if (nDateH2 == 28)
                                {
                                    nCol = nCol + 3;
                                }
                                else if (nDateH2 == 29)
                                {
                                    nCol = nCol + 2;
                                }
                                else if (nDateH2 == 30)
                                {
                                    nCol = nCol + 1;
                                }

                                ssView1_Sheet1.Cells[i + 7, nCol + 4].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                                ssView1_Sheet1.Cells[i + 7, nCol + 5].Text = clsOpdNr.Read_BuseName(clsDB.DbCon, dt.Rows[i]["WardCode"].ToString().Trim());
                                ssView1_Sheet1.Cells[i + 7, nCol + 6].Text = Read_JikName(dt.Rows[i]["JIKCODE"].ToString().Trim());
                                ssView1_Sheet1.Cells[i + 7, nCol + 7].Text = dt.Rows[i]["SCHEDULE"].ToString();

                            }

                            ssView1_Sheet1.Cells[i + 7, 44+1].Text = dt.Rows[i]["RANK"].ToString().Trim();
                        }

                        //'마취과 또는 OR인 경우에는 휴일에 OFF DisPlay
                        nCol4 = 4;
                        if (VB.Trim(cboDept.Text) == "마취과" || VB.Trim(cboDept.Text) == "OR" || VB.Trim(cboDept.Text) == "외래")
                        {
                            //2019-02-07 위에서 연산 한번 한 다음에 또 할경우 변수 초기화
                            strOFFDay = strYY + "-" + strMM + "-01";

                            for (i = nDateH1; i <= nDateH2; i++)
                            {
                                SQL = " Select TO_CHAR(JobDate,'YYYY-MM-DD') JOBDATE  ";
                                SQL = SQL + ComNum.VBLF + "  FROM BAS_JOB ";
                                SQL = SQL + ComNum.VBLF + " WHERE HolyDay = '*' ";
                                SQL = SQL + ComNum.VBLF + "   AND JOBDATE <>TO_DATE('2014-06-04','YYYY-MM-DD') ";
                                SQL = SQL + ComNum.VBLF + "   AND JobDate = TO_DATE('" + strOFFDay + "','YYYY-MM-DD') ";

                                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    btnSearch.Enabled = true;
                                    return;
                                }

                                if (dt.Rows.Count > 0)
                                {
                                    for (j = 0; j < dt.Rows.Count; j++)
                                    {
                                        nHDay = Convert.ToInt32(VB.Val(VB.Right(dt.Rows[j]["JobDate"].ToString().Trim(), 2)));
                                        ssView1_Sheet1.Cells[6, nCol4 + nHDay - 1, ssView1_Sheet1.RowCount - 1, nCol4 + nHDay - 1].Text = "OFF";
                                    }
                                }

                                strOFFDay = Convert.ToDateTime(strOFFDay).AddDays(1).ToShortDateString();
                            }
                        }
                    }
                }

                if (GstrWardCode != "OPD")
                {
                    Total_Read();
                }

                //'휴일인 경우에 Color DisPlay
                nCol3 = 4;
                SQL = "";
                SQL = SQL + ComNum.VBLF + "Select TO_CHAR(JobDate,'YYYY-MM-DD') JOBDATE  ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_JOB ";
                SQL = SQL + ComNum.VBLF + " WHERE HolyDay = '*' ";
                SQL = SQL + ComNum.VBLF + "   AND JobDate >= TO_DATE('" + strDateH1 + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND JobDate <= TO_DATE('" + strDateH2 + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND JOBDATE <> TO_DATE('2014-06-04','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    btnSearch.Enabled = true;
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    btnSearch.Enabled = true;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (j = 0; j < dt.Rows.Count; j++)
                    {
                        nHDay = Convert.ToInt32(VB.Val(VB.Right(dt.Rows[j]["JobDate"].ToString().Trim(), 2)));
                        ssView1_Sheet1.Cells[0, nCol3 + nHDay - 1, 4, nCol3 + nHDay - 1].BackColor = Color.Red;
                        ssView1_Sheet1.Cells[6, nCol3 + nHDay - 1, ssView1_Sheet1.RowCount - 1, nCol3 + nHDay - 1].BackColor = Color.Red;

                        if (GstrWardCode != "OPD")
                        {
                            ssView5_Sheet1.Cells[0, nCol3 + nHDay - 1, ssView5_Sheet1.RowCount - 1, nCol3 + nHDay - 1].BackColor = Color.Red;
                        }
                    }
                }



                if (clsType.User.Sabun == "08985" || clsType.User.Sabun == "04349" || clsType.User.Sabun == "06022" || clsType.User.Sabun == "13386" || clsType.User.Sabun == "23758" || clsType.User.Sabun == "23767" || clsType.User.Sabun == "42646" || clsType.User.Sabun == "21403")
                {
                    btnSave.Enabled = true;
                }
                else if (string.Compare(VB.Left(VB.Trim(cboYYMM.Text), 4) + VB.Mid(VB.Trim(cboYYMM.Text), 6, 2), strNewday) >= 0 && (GstrWardCode == "ER" || GstrWardCode == "OPD"))
                {
                    btnSave.Enabled = true;
                }
                else if (string.Compare(VB.Left(VB.Trim(cboYYMM.Text), 4) + VB.Mid(VB.Trim(cboYYMM.Text), 6, 2), strNewday) > 0)
                {
                    btnSave.Enabled = true;
                }
                else
                {
                    btnSave.Enabled = false;
                }

                btnCancel.Enabled = true;


                //'경리과 수녀님 등록 권한 제외
                if (clsType.User.Sabun == "30761")
                    btnSave.Enabled = false;

                if (VB.Trim(cboDept.Text) == "OR" || VB.Trim(cboDept.Text) == "수술실" || VB.Trim(cboDept.Text) == "마취과")
                {
                    btnSave.Enabled = true;
                }

                if (strBuse == "OPD")
                {

                
                    CheckOpdMiss(strDay, strDateH2);
                }

                //TEST
                //btnSave.Enabled = true;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }


        private void CheckOpdMiss(string strFrDate, string strToDate)
        {

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strData = "";
            string strSabun = "";

            int i = 0;

            ssOpdMiss_Sheet1.Visible = true;

            strSabun = "";
            ssOpdMiss_Sheet1.Rows.Count = 0;

            for (i = 7; i <= ssView1_Sheet1.RowCount - 1; i++)
            {

                strSabun += "'" + ssView1_Sheet1.Cells[i, 1].Text.Trim() + "',";
            }
            if (strSabun != "")
            {
                strSabun = strSabun.Substring(0, strSabun.Length - 1);
            }
            
            SQL = " SELECT SABUN, KORNAME, JIK, BUSE FROM KOSMOS_ADM.INSA_MST  ";
            SQL += ComNum.VBLF + " WHERE KUNDAY <= TO_DATE('" + strToDate + "', 'YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "   AND (TOIDAY IS NULL OR TOIDAY >= TO_DATE('" + strFrDate + "', 'YYYY-MM-DD')) ";
            SQL += ComNum.VBLF + "   AND BUSE = '033110' ";
            SQL += ComNum.VBLF + "   AND SABUN NOT IN (" + strSabun + ") ";
            SQL += ComNum.VBLF + " ORDER BY KUNDAY ";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }
            if (dt.Rows.Count > 0)
            {
                ssOpdMiss_Sheet1.Rows.Count = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strData = VB.Left(dt.Rows[i]["KorName"].ToString().Trim() + VB.Space(10), 10);
                    strData = strData + VB.Left(dt.Rows[i]["SABUN"].ToString().Trim() + VB.Space(8), 8);
                    strData = strData + VB.Left(Read_JikName(dt.Rows[i]["Jik"].ToString().Trim()) + VB.Space(15), 15);
                    strData = strData + VB.Left(getDeptName(dt.Rows[i]["BUSE"].ToString().Trim()) + VB.Space(10), 10);

                    ssOpdMiss_Sheet1.RowCount = ssOpdMiss_Sheet1.RowCount + 1;
                    ssOpdMiss_Sheet1.Cells[i, 0].Text = strData;
                }
            }

            dt.Dispose();
            dt = null;

        }

        private string ComboDept_Code()
        {
            string rtnVal = "";

            switch (cboDept.Text.Trim())
            {
                case "간호부":
                    rtnVal = "033101";
                    break;
                case "OR":
                    rtnVal = "033102";
                    break;
                case "마취과":
                    rtnVal = "033103";
                    break;
                case "NR":
                    rtnVal = "033104";
                    break;
                case "DR":
                    rtnVal = "033105";
                    break;
                case "SICU":
                    rtnVal = "033106";
                    break;
                case "공급실":
                    rtnVal = "033107";
                    break;
                case "HD":
                    rtnVal = "033108";
                    break;
                case "ER":
                    rtnVal = "033109";
                    break;
                case "외래":
                    rtnVal = "033110";
                    break;
                case "정신과":
                    rtnVal = "033111";
                    break;
                case "3A":
                    rtnVal = "033112";
                    break;
                case "3B":
                    rtnVal = "033113";
                    break;
                case "3W":
                    rtnVal = "033127";
                    break;
                case "4A":
                    rtnVal = "033114";
                    break;
                case "5W":
                    rtnVal = "033116";
                    break;
                case "6W":
                    rtnVal = "033117";
                    break;
                case "7W":
                    rtnVal = "033118";
                    break;
                case "8W":
                    rtnVal = "033119";
                    break;
                case "MICU":
                    rtnVal = "033106";
                    break;
                case "32":
                    rtnVal = "101743";
                    break;
                case "33":
                    rtnVal = "101744";
                    break;
                case "34":
                    rtnVal = "101745";
                    break;
                case "40":
                    rtnVal = "101752";
                    break;
                case "50":
                    rtnVal = "101753";
                    break;
                case "53":
                    rtnVal = "101746";
                    break;
                case "55":
                    rtnVal = "101747";
                    break;
                case "60":
                    rtnVal = "101754";
                    break;
                case "63":
                    rtnVal = "101748";
                    break;
                case "65":
                    rtnVal = "101749";
                    break;
                case "70":
                    rtnVal = "101755";
                    break;
                case "73":
                    rtnVal = "101750";
                    break;
                case "75":
                    rtnVal = "101751";
                    break;
                case "80":
                    rtnVal = "101756";
                    break;
                case "83":
                    rtnVal = "101776";
                    break;
                case "85":
                    rtnVal = "101781";
                    break;
                case "수술실":
                    rtnVal = "033102";
                    break;
            }

            return rtnVal;
        }


        private void BAS_DOCTOR_READ()
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            int nHDay = 0;              //'휴일인 날짜
            int nCol3 = 0;

            string strYY = VB.Left(cboYYMM.Text, 4);
            string strMM = VB.Mid(cboYYMM.Text, 6, 2);
            string strDate = strYY + "-" + strMM + "-01";
            string strDateH1 = strDate;
            string strDateH2 = CF.READ_LASTDAY(clsDB.DbCon, strDate);

            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT A.SABUN, A.KORNAME, B.NAME BJIKNAME, A.JIK ,B.GUBUN ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_ADM.INSA_MST A, KOSMOS_ADM.INSA_CODE B ";
                SQL = SQL + ComNum.VBLF + " WHERE A.JIK = B.CODE";
                SQL = SQL + ComNum.VBLF + "   AND A.TOIDAY IS NULL ";
                SQL = SQL + ComNum.VBLF + "   AND b.GuBun = '2' ";
                SQL = SQL + ComNum.VBLF + "   AND A.JIK BETWEEN '21' AND '26' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY A.JIK, A.KORNAME, A.SABUN ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView1_Sheet1.RowCount = dt.Rows.Count + 7;
                    ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView1_Sheet1.Cells[i + 7, 0].Text = dt.Rows[i]["DEPTKORNAME"].ToString().Trim();
                        ssView1_Sheet1.Cells[i + 7, 1].Text = dt.Rows[i]["Sabun"].ToString().Trim();
                        ssView1_Sheet1.Cells[i + 7, 2].Text = dt.Rows[i]["KorName"].ToString().Trim();
                        ssView1_Sheet1.Cells[i + 7, 3].Text = Read_JikName(dt.Rows[i]["Jik"].ToString().Trim());
                    }
                }
                dt.Dispose();
                dt = null;


                //'휴일인 경우에 Color DisPlay
                nCol3 = 4;
                SQL = SQL + ComNum.VBLF + "Select TO_CHAR(JobDate,'YYYY-MM-DD') JOBDATE  ";
                SQL = SQL + ComNum.VBLF + "  FROM BAS_JOB ";
                SQL = SQL + ComNum.VBLF + " WHERE HolyDay = '*' ";
                SQL = SQL + ComNum.VBLF + "   AND JobDate >= TO_DATE('" + strDateH1 + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND JobDate <= TO_DATE('" + strDateH2 + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND JOBDATE <> TO_DATE('2014-06-04','YYYY-MM-DD')";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nHDay = Convert.ToInt32(VB.Val(VB.Right(dt.Rows[i]["JobDate"].ToString().Trim(), 2)));
                        ssView1_Sheet1.Cells[0, nCol3 + nHDay - 1, 4, nCol3 + nHDay - 1].BackColor = Color.Red;
                        ssView1_Sheet1.Cells[6, nCol3 + nHDay - 1, ssView1_Sheet1.RowCount - 1, nCol3 + nHDay - 1].BackColor = Color.Red;
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private bool btnSaveClick()
        {
            bool rtVal = false;
            int i = 0;
            int j = 0;
            int k = 0;
            string SQL = "";
            //string SqlErr = "";
            //int intRowAffected = 0;

            string strYY = "";
            string strMM = "";
            string strDept = "";                //'부서
            string strSABUN = "";               //'사번
            string strName = "";                //'성명
            string strJik = "";                 //'직책
            string strSCHEDULE = "";            //'근무내역
            string strTotSchedule = "";         //'Tot 근무내역
            //string nDTot = "";                  //'주간근무일 합계
            //string nETot = "";                  //'저녁근무일 합계
            //string nNTot = "";                  //'야간근무일 합계
            //string nOFFTot = "";                //'휴무 합계
            string strROWID = "";
            string strOldDept = "";             //'OLD 부서
            string strOldJik = "";              //'OLD 직책
            string strOldSchedule = "";         //'OLD 스케줄
            int nRank = 0;                      //'순서
            int nRank_OLD = 0;                  //'순서 OLD

            string strWeek = "";                //'익일의 근무 형태
            string strNew = "";                 //'근무한 날짜
            string strCompt = "";               //'근무 현태가 변경되면 'Y' 표시
            string[] str_BunpYo_1 = new string[31];   //'insa 번표1

            bool boolFlag = false;

            if (cboDept.Enabled == true)
            {
                boolFlag = true;
            }

            cboYYMM.Enabled = false;
            cboDept.Enabled = false;
            btnSearch.Enabled = false;
            btnSave.Enabled = false;

            panHelpList.Visible = false;
            btnHelp.Visible = false;
            panHelp1List.Visible = false;
            txtHelpName.Text = "";
            btnHelp1.Visible = false;
            lblMsg.Text = "";
            panSS1.Enabled = false;
            //'jjy(2005-04-11) nr dr 사용함 4월부터
            //'If Trim(ComboDept.Text) = "NR" Or Trim(ComboDept.Text) = "DR" Then   MsgBox "병동을 확인하세요", , "확인": Exit Sub
            str_YYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 6, 2);


            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i <= 31; i++)
                {
                    str_BunpYo[i] = "";
                }

                for (i = 6; i < ssView1_Sheet1.Rows.Count; i++)
                {
                    //str_ChulTime = "";
                    //str_ChulGbn = "";
                    //str_CTime = "";
                    strSCHEDULE = "";
                    strTotSchedule = "";

                    strDept = ssView1_Sheet1.Cells[i, 0].Text;
                    strSABUN = ssView1_Sheet1.Cells[i, 1].Text;
                    str_sabun = strSABUN;
                    strName = ssView1_Sheet1.Cells[i, 2].Text;
                    strJik = ssView1_Sheet1.Cells[i, 3].Text;


                    for (j = 4; j < 35; j++)
                    {
                        strSCHEDULE = clsNurse.MidH(VB.Trim(ssView1_Sheet1.Cells[i, j].Text) + VB.Space(4), 0, 4);
                        strTotSchedule = strTotSchedule + strSCHEDULE;
                    }

                    strROWID = ssView1_Sheet1.Cells[i, 39].Text;
                    strOldDept = ssView1_Sheet1.Cells[i, 40].Text;
                    strOldJik = ssView1_Sheet1.Cells[i, 41].Text;
                    strOldSchedule = ssView1_Sheet1.Cells[i, 42].Text;
                    nRank = Convert.ToInt32(VB.Val(ssView1_Sheet1.Cells[i, 44+1].Text));
                    nRank_OLD = Convert.ToInt32(VB.Val(ssView1_Sheet1.Cells[i, 45+1].Text));


                    if (strDept != strOldDept && strROWID == "" && strSABUN != "")
                    {
                        INSERT_Rtn(strName, strDept, strSABUN, strJik, strTotSchedule, nRank);
                    }
                    else if (strDept != strOldDept && strROWID != "")
                    {
                        DEPT_INSERT_RTN(strName, strDept, strSABUN, strJik, strTotSchedule, nRank);
                    }
                    else if (strDept == strOldDept)
                    {
                        if (strTotSchedule != strOldSchedule || nRank != nRank_OLD)
                            SCHEDULE_UPDATE_RTN(strName, strDept, strSABUN, strJik, strTotSchedule, nRank);
                        if (strJik != strOldJik)
                            JIK_UPDATE_RTN(strName, strDept, strSABUN, strJik, strTotSchedule, nRank);
                    }

                    //'INSA_CHULTIME테이블의 DATA 작업
                    for (k = 5; k <= nDateH2 + 4; k++)
                    {
                        strSCHEDULE = clsNurse.MidH(VB.Trim(ssView1_Sheet1.Cells[i, k - 1].Text) + VB.Space(4), 0, 4);
                        str_BunpYo[k - 4] = VB.Trim(BunpYo_Data(strSCHEDULE));
                        if (str_BunpYo[k - 4] == "")
                        {
                            ComFunc.MsgBox("(" + (k - 4) + "일 사번은 " + strSABUN + ") Schedule을 다시 확인 하세요.!!");
                        }
                    }

                    Insa_GunTae_Data();
                }


                //'/마취과 , 수술실에 당직을 이전 시킴
                if (VB.Trim(cboDept.Text) == "마취과" || VB.Trim(cboDept.Text) == "OR")
                {
                    strYY = VB.Left(cboYYMM.Text, 4);
                    strMM = VB.Mid(cboYYMM.Text, 6, 2);
                    for (i = 0; i < ssView1_Sheet1.RowCount; i++)
                    {
                        strWeek = "";
                        strCompt = "";

                        strSABUN = ssView1_Sheet1.Cells[i, 1].Text;
                        strCompt = ssView1_Sheet1.Cells[i, 43].Text;

                        if (strCompt == "Y")
                        {
                            for (j = nDateH1; j < nDateH2; j++)
                            {
                                strNew = "";
                                strWeek = ssView1_Sheet1.Cells[i, 4 + j].Text;
                                strNew = strYY + "-" + strMM + "-" + VB.Format(j, "00");
                                if (VB.Trim(cboDept.Text) == "OR")
                                {
                                    if (strWeek == "C")
                                    {
                                        INSERT_RTNC(strNew);
                                    }
                                    else
                                    {
                                        DELETE_RTNC(strNew);
                                    }

                                    if (strWeek == "C1")
                                    {
                                        INSERT_RTN1(strNew);
                                    }
                                    else
                                    {
                                        DELETE_RTN1(strNew);
                                    }

                                    if (strWeek == "C2")
                                    {
                                        INSERT_RTN2(strNew);
                                    }
                                    else
                                    {
                                        DELETE_RTN2(strNew);
                                    }
                                }
                                else
                                {
                                    if (strWeek == "C")
                                    {
                                        AINSERT_RTN(strNew);
                                    }
                                    else
                                    {
                                        ADELETE_RTN(strNew);
                                    }
                                }
                            }
                        }
                    }
                }


                for (i = 7; i < ssView1_Sheet1.RowCount; i++)
                {
                    TOT_SUM(VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 6, 2), VB.Trim(ssView1_Sheet1.Cells[i, 1].Text));
                }

                if (boolFlag == true)
                {
                    cboDept.Enabled = true;
                }

                cboYYMM.Enabled = true;

                //clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        private bool INSERT_Rtn(string strName, string strDept, string strSabun, string strJik, string strTotSchedule, int nRank)
        {
            bool rtVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "INSERT INTO KOSMOS_PMPA.NUR_SCHEDULE1 (YYMM,WardCode,Sabun,SName,JikCode,Schedule, Rank) VALUES ";
            SQL = SQL + ComNum.VBLF + " ('" + VB.Trim(VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 6, 2)) + "','" + clsOpdNr.READ_BUSECODE(clsDB.DbCon, strDept) + "', ";
            SQL = SQL + ComNum.VBLF + " '" + strSabun + "','" + strName + "','" + Read_JikCode(strJik) + "', ";
            SQL = SQL + ComNum.VBLF + " '" + strTotSchedule + "' , '" + nRank + "' ) ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                Cursor.Current = Cursors.Default;
                return rtVal;
            }

            rtVal = true;
            return rtVal;
        }

        private bool DEPT_INSERT_RTN(string strName, string strDept, string strSabun, string strJik, string strTotSchedule, int nRank)
        {
            bool rtVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT Sabun FROM KOSMOS_PMPA.NUR_SCHEDULE1 ";
            SQL = SQL + ComNum.VBLF + "  WHERE YYMM = '" + VB.Trim(VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 6, 2)) + "' ";
            SQL = SQL + ComNum.VBLF + " AND WardCode = '" + clsOpdNr.READ_BUSECODE(clsDB.DbCon, strDept) + "' ";
            SQL = SQL + ComNum.VBLF + " AND Sabun = '" + strSabun + "' ";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO KOSMOS_PMPA.NUR_SCHEDULE1 (YYMM,WardCode,Sabun,SName,JikCode,Schedule, RANK) VALUES ";
                SQL = SQL + ComNum.VBLF + "('" + VB.Trim(VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 6, 2)) + "','" + clsOpdNr.READ_BUSECODE(clsDB.DbCon, strDept) + "', ";
                SQL = SQL + ComNum.VBLF + "'" + clsType.User.Sabun + "','" + strName + "','" + Read_JikCode(strJik) + "','" + strTotSchedule + "' ,'" + nRank + "'  ) ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }
            }
            else
            {
                ComFunc.MsgBox("이미 해당 부서에 등록되어 있습니다.");
                dt.Dispose();
                dt = null;
            }

            rtVal = true;
            return rtVal;
        }

        private bool JIK_UPDATE_RTN(string strName, string strDept, string strSabun, string strJik, string strTotSchedule, int nRank)
        {
            bool rtVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "UPDATE KOSMOS_PMPA.NUR_SCHEDULE1 SET ";
            SQL = SQL + ComNum.VBLF + " JikCode='" + Read_JikCode(strJik) + "', ";
            SQL = SQL + ComNum.VBLF + "  RANK = '" + nRank + "' ";
            SQL = SQL + ComNum.VBLF + " WHERE Sabun = '" + strSabun + "' ";
            SQL = SQL + ComNum.VBLF + "  AND YYMM = '" + VB.Trim(VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 6, 2)) + "' ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                Cursor.Current = Cursors.Default;
                return rtVal;
            }

            rtVal = true;
            return rtVal;
        }

        private bool SCHEDULE_UPDATE_RTN(string strName, string strDept, string strSabun, string strJik, string strTotSchedule, int nRank)
        {
            bool rtVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "UPDATE KOSMOS_PMPA.NUR_SCHEDULE1 SET ";
            SQL = SQL + ComNum.VBLF + " Schedule='" + strTotSchedule + "', ";
            SQL = SQL + ComNum.VBLF + "  RANK = '" + nRank + "' ";
            SQL = SQL + ComNum.VBLF + " WHERE Sabun = '" + strSabun + "' ";
            SQL = SQL + ComNum.VBLF + "  AND YYMM = '" + VB.Trim(VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 6, 2)) + "' ";
            SQL = SQL + ComNum.VBLF + "  AND WardCode = '" + clsOpdNr.READ_BUSECODE(clsDB.DbCon, strDept) + "' ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                Cursor.Current = Cursors.Default;
                return rtVal;
            }

            rtVal = true;
            return rtVal;
        }

        //'/마취과, 수술실의 당직 Rtn
        private bool INSERT_RTNC(string strNew)
        {
            bool rtVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(ActDate,'YYYY-MM-DD') ACTDATE ";
            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_SCHEDULE2 ";
            SQL = SQL + ComNum.VBLF + " WHERE ActDate = TO_DATE('" + strNew + "','YYYY-MM-DD')";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO KOSMOS_PMPA.NUR_SCHEDULE2 (ActDate,OP1) VALUES ";
                SQL = SQL + ComNum.VBLF + "(TO_DATE('" + strNew + "','YYYY-MM-DD') ,'" + clsType.User.Sabun + "') ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }
            }
            else
            {
                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "UPDATE KOSMOS_PMPA.NUR_SCHEDULE2 SET OP1 = '" + clsType.User.Sabun + "' ";
                SQL = SQL + ComNum.VBLF + " WHERE ActDate = TO_DATE('" + strNew + "','YYYY-MM-DD')";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }
            }

            rtVal = true;
            return rtVal;
        }

        private bool DELETE_RTNC(string strNew)
        {
            bool rtVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "UPDATE KOSMOS_PMPA.NUR_SCHEDULE2 SET OP1 = '' ";
            SQL = SQL + ComNum.VBLF + " WHERE OP1 = '" + clsType.User.Sabun + "' ";
            SQL = SQL + ComNum.VBLF + " AND ActDate = TO_DATE('" + strNew + "','YYYY-MM-DD')";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                Cursor.Current = Cursors.Default;
                return rtVal;
            }

            rtVal = true;
            return rtVal;
        }

        private bool INSERT_RTN1(string strNew)
        {
            bool rtVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ActDate ";
            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_SCHEDULE2 ";
            SQL = SQL + ComNum.VBLF + " WHERE ActDate = TO_DATE('" + strNew + "','YYYY-MM-DD') ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO KOSMOS_PMPA.NUR_SCHEDULE2 (ActDate,OP2) VALUES ";
                SQL = SQL + ComNum.VBLF + "(TO_DATE('" + strNew + "','YYYY-MM-DD'),'" + clsType.User.Sabun + "') ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }
            }
            else
            {
                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "UPDATE KOSMOS_PMPA.NUR_SCHEDULE2 SET OP2 = '" + clsType.User.Sabun + "' ";
                SQL = SQL + ComNum.VBLF + " WHERE ActDate = TO_DATE('" + strNew + "','YYYY-MM-DD')";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }
            }

            rtVal = true;
            return rtVal;
        }

        private bool DELETE_RTN1(string strNew)
        {
            bool rtVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "UPDATE KOSMOS_PMPA.NUR_SCHEDULE2 SET OP2 = '' ";
            SQL = SQL + ComNum.VBLF + " WHERE OP2 = '" + clsType.User.Sabun + "' ";
            SQL = SQL + ComNum.VBLF + " AND ActDate = TO_DATE('" + strNew + "','YYYY-MM-DD')";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                Cursor.Current = Cursors.Default;
                return rtVal;
            }

            rtVal = true;
            return rtVal;
        }

        private bool INSERT_RTN2(string strNew)
        {
            bool rtVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ActDate ";
            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_SCHEDULE2 ";
            SQL = SQL + ComNum.VBLF + " WHERE ActDate = TO_DATE('" + strNew + "','YYYY-MM-DD') ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO KOSMOS_PMPA.NUR_SCHEDULE2 (ActDate,OP3) VALUES ";
                SQL = SQL + ComNum.VBLF + "(TO_DATE('" + strNew + "','YYYY-MM-DD'),'" + clsType.User.Sabun + "') ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }
            }
            else
            {
                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "UPDATE KOSMOS_PMPA.NUR_SCHEDULE2 SET OP3 = '" + clsType.User.Sabun + "' ";
                SQL = SQL + ComNum.VBLF + " WHERE ActDate = TO_DATE('" + strNew + "','YYYY-MM-DD')";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }
            }

            rtVal = true;
            return rtVal;
        }

        private bool DELETE_RTN2(string strNew)
        {
            bool rtVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "UPDATE KOSMOS_PMPA.NUR_SCHEDULE2 SET OP3 = '' ";
            SQL = SQL + ComNum.VBLF + " WHERE OP3 = '" + clsType.User.Sabun + "' ";
            SQL = SQL + ComNum.VBLF + " AND ActDate = TO_DATE('" + strNew + "','YYYY-MM-DD')";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                Cursor.Current = Cursors.Default;
                return rtVal;
            }

            rtVal = true;
            return rtVal;
        }

        private bool AINSERT_RTN(string strNew)
        {
            bool rtVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ActDate ";
            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_SCHEDULE2 ";
            SQL = SQL + ComNum.VBLF + " WHERE ActDate = TO_DATE('" + strNew + "','YYYY-MM-DD') ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO KOSMOS_PMPA.NUR_SCHEDULE2 (ActDate,AN) VALUES ";
                SQL = SQL + ComNum.VBLF + "(TO_DATE('" + strNew + "','YYYY-MM-DD'),'" + clsType.User.Sabun + "') ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }
            }
            else
            {
                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "UPDATE KOSMOS_PMPA.NUR_SCHEDULE2 SET AN = '" + clsType.User.Sabun + "' ";
                SQL = SQL + ComNum.VBLF + " WHERE ActDate = TO_DATE('" + strNew + "','YYYY-MM-DD')";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }
            }

            rtVal = true;
            return rtVal;
        }

        private bool ADELETE_RTN(string strNew)
        {
            bool rtVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "UPDATE KOSMOS_PMPA.NUR_SCHEDULE2 SET AN = '' ";
            SQL = SQL + ComNum.VBLF + " WHERE AN = '" + clsType.User.Sabun + "' ";
            SQL = SQL + ComNum.VBLF + " AND ActDate = TO_DATE('" + strNew + "','YYYY-MM-DD')";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                Cursor.Current = Cursors.Default;
                return rtVal;
            }

            rtVal = true;
            return rtVal;
        }


        private string CHECK_GUNTAE(string argSABUN, string argDATE, string argGUNTAE)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strReturn = "";

            string strGuntae = "";

            switch (argGUNTAE)
            {
                case "휴가":
                    strGuntae = "A";
                    break;
                case "경조":
                    strGuntae = "C";
                    break;
                case "교육":
                    strGuntae = "D";
                    break;
                case "출장":
                    strGuntae = "E";
                    break;
                case "학회":
                    strGuntae = "K";
                    break;
                case "반휴":
                    strGuntae = "P";
                    break;
                default:
                    strReturn = "OK";
                    return "OK";
            }
        
            strReturn = "NO";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT SUM(DECODE(SUBSTR(SUBGBN, 2, 1), '+', 1, -1)) CNT ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.REPORT_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE JOBDATE = TO_DATE('" + argDATE + "', 'YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND SETSABUN = '" + argSABUN + "' ";
                SQL = SQL + ComNum.VBLF + "   AND SUBGBN LIKE '" + strGuntae + "%' ";
                SQL = SQL + ComNum.VBLF + "   AND CANDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "  HAVING SUM(DECODE(SUBSTR(SUBGBN, 2, 1), '+', 1, -1)) > 0 ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return strReturn;
                }
                if (dt.Rows.Count > 0)
                {
                    strReturn = "OK";
                }
                dt.Dispose();
                dt = null;

                return strReturn;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
            return strReturn;

        }


        private string Read_JikCode(string argValue)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Code cJikCode FROM NUR_CODE ";
                SQL = SQL + ComNum.VBLF + "WHERE Gubun = '1' ";
                SQL = SQL + ComNum.VBLF + "  AND Name = '" + argValue + "' ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["cJikCode"].ToString().Trim();
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
            return rtnVal;
        }

        private string BunpYo_Data(string ArgData)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (string.Compare(VB.Left(ArgData, 1), "가") >= 0)
                {
                    switch (ArgData)
                    {
                        case "휴가":
                            rtnVal = "TAAA";
                            break;
                        case "월차":
                            rtnVal = "TBBB";
                            break;
                        case "특일":
                            rtnVal = "TCCC";
                            break;
                        case "경조":
                            rtnVal = "TCCC";
                            break;
                        case "교육":
                            rtnVal = "TDDD";
                            break;
                        case "출장":
                            rtnVal = "TEEE";
                            break;
                        case "병가":
                            rtnVal = "TFFF";
                            break;
                        case "분휴":
                            rtnVal = "TGGG";
                            break;
                        case "피정":
                            rtnVal = "THHH";
                            break;
                        case "훈련":
                            rtnVal = "TIII";
                            break;
                        case "생휴":
                            rtnVal = "TJJJ";
                            break;
                        case "학회":
                            rtnVal = "TKKK";
                            break;
                        case "결근":
                            rtnVal = "TLLL";
                            break;
                        case "지각":
                            rtnVal = "TMMM";
                            break;
                        case "조퇴":
                            rtnVal = "TNNN";
                            break;
                        case "반휴":
                            rtnVal = "TPPP";
                            break;
                        case "휴직":
                            rtnVal = "TRRR";
                            break;
                        case "파견":
                            rtnVal = "TUUU";
                            break;
                        case "난휴":
                            rtnVal = "TVVV";
                            break;
                        case "가휴":
                            rtnVal = "TWWW";
                            break;
                        case "무휴":
                            rtnVal = "TYYY";
                            break;
                        case "산재":
                            rtnVal = "TZZZ";
                            break;
                        case "무반":
                            rtnVal = "TQQQ";
                            break;
                    }
                }
                else
                {
                    if (ArgData == "    ")
                    {
                        rtnVal = "D083";
                    }
                    else
                    {

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT CODE From KOSMOS_ADM.INSA_GUNTAECODE ";
                        SQL = SQL + ComNum.VBLF + " WHERE NURCODE = '" + ArgData + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL ";

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return rtnVal;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            rtnVal = dt.Rows[0]["CODE"].ToString().Trim();
                        }
                        else
                        {
                            rtnVal = "";
                        }

                        dt.Dispose();
                        dt = null;
                    }
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        private bool Insa_GunTae_Data()
        {
            bool rtVal = false;
            int i = 0;
            //int j = 0;
            //int k = 0;
            DataTable dt = null;
            //DataTable dt2 = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string[] strBunpYo = new string[32];
            string[] strChulTime = new string[32];
            string[] strChulGbn = new string[32];
            //string strBuData = "";
            //string strBuData1 = "";
            //string strRowId11 = "";
            int nDay = 0;
            string nDD = "";

            string strJikCode = "";
            string strBuseCode = "";
            //string nREAD_1 = "";
            string strKorname = "";
            string strBuseName = "";

            if (GstrWardCode != "OPD")
            {
                if (Check_Nurse_Buse(str_sabun) == "NO")
                {
                    return false;
                }
            }


            Cursor.Current = Cursors.WaitCursor;


            ComFunc.ReadSysDate(clsDB.DbCon);

            nDay = Convert.ToInt32(VB.Val(VB.Right(clsPublic.GstrSysDate, 2)));
            nDD = Convert.ToString(DateTime.DaysInMonth(Convert.ToInt32(VB.Left(cboYYMM.Text, 4)), Convert.ToInt32(VB.Mid(cboYYMM.Text, 6, 2))));

            SQL = "SELECT A.BUSE ABUSE, A.JIK AJIK, A.KORNAME AKORNAME, B.NAME BNAME";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_ERP + "INSA_MST A, " + ComNum.DB_PMPA + "BAS_BUSE B";
            SQL = SQL + ComNum.VBLF + "  WHERE A.BUSE = B.BUCODE";
            SQL = SQL + ComNum.VBLF + "  AND A.SABUN = '" + str_sabun.PadLeft(5, '0') + "'";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return rtVal;
            }

            if (dt.Rows.Count > 0)
            {
                strJikCode = dt.Rows[0]["AJIK"].ToString().Trim();
                strBuseCode = dt.Rows[0]["ABUSE"].ToString().Trim();
                strKorname = dt.Rows[0]["AKORNAME"].ToString().Trim();
                strBuseName = dt.Rows[0]["BNAME"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;

            if (str_sabun != "")
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT SABUN, YYMM, ";
                for (i = 1; i <= Convert.ToInt32(nDD); i++)
                {
                    SQL = SQL + ComNum.VBLF + "BUNPYO" + i + ", CHULTIME" + i + ", CHULGBN" + i + ", ";
                }
                SQL = SQL + ComNum.VBLF + " RowId FROM " + ComNum.DB_ERP + "INSA_CHULTIME ";
                SQL = SQL + ComNum.VBLF + " WHERE SaBUN = '" + str_sabun + "' ";
                SQL = SQL + ComNum.VBLF + " AND YYMM = '" + str_YYMM + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count == 0)
                {
                    //'INSA_GUNTAE테이블에 INSERT함.
                    SQL = "INSERT INTO KOSMOS_ADM.INSA_CHULTIME(SABUN, YYMM, BUSE_CODE, BUSE_NAME, JIK_CODE, JIK_NAME, ENT_DATE, CHUL_KORNAME,";
                    for (i = 1; i <= Convert.ToInt32(nDD) - 1; i++)
                    {
                        SQL = SQL + ComNum.VBLF + " BUNPYO" + i + ", ";
                        if (i == Convert.ToInt32(nDD) - 1)
                            SQL = SQL + ComNum.VBLF + " BUNPYO" + (i + 1) + ") values ";
                    }
                    SQL = SQL + ComNum.VBLF + " ('" + str_sabun + "', '" + str_YYMM + "', '" + strBuseCode + "', '" + strBuseName + "', '" + strJikCode + "', '" + str_jik + "', TO_DATE('" + clsPublic.GstrSysDate + "', 'YYYY-MM-DD'), '" + strKorname + "',";
                    for (i = 1; i <= Convert.ToInt32(nDD) - 1; i++)
                    {
                        SQL = SQL + ComNum.VBLF + " '" + str_BunpYo[i] + "', ";
                        if (i == Convert.ToInt32(nDD) - 1)
                            SQL = SQL + ComNum.VBLF + " '" + str_BunpYo[i + 1] + "') ";
                    }
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return rtVal;
                    }
                }
                else
                {
                    for (i = 1; i <= Convert.ToInt32(nDD); i++)
                    {
                        strBunpYo[i] = dt.Rows[0]["BUNPYO" + nDD].ToString().Trim();
                        if (str_BunpYo[i] != strBunpYo[i])
                            strBunpYo[i] = str_BunpYo[i];
                    }

                    SQL = " UPDATE KOSMOS_ADM.INSA_CHULTIME SET ";
                    for (i = 1; i <= Convert.ToInt32(nDD) - 1; i++)
                    {
                        SQL = SQL + ComNum.VBLF + " BUNPYO" + i + " = '" + strBunpYo[i] + "',";
                        if (i == Convert.ToInt32(nDD) - 1)
                            SQL = SQL + ComNum.VBLF + " BUNPYO" + (i + 1) + " = '" + strBunpYo[i + 1] + "'";
                    }
                    SQL = SQL + ComNum.VBLF + " WHERE SABUN  = '" + str_sabun + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND YYMM  = '" + str_YYMM + "' ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return rtVal;
                    }
                }
            }

            Cursor.Current = Cursors.Default;
            rtVal = true;
            return rtVal;

        }

        private bool TOT_SUM(string argYYMM, string argSabun)
        {
            bool rtVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            //'간호부 토탈 DAY, EVENING, NIGHT, OFF 일수 등록                
            int nSum_Day = 0;
            int nSum_Eve = 0;
            int nSum_Nit = 0;
            int nSum_OFF = 0;

            //int nReadTOT = 0;
            int nTEMP = 0;

            int nSDATE = 0;
            int nEDATE = 0;
            string strSCHEDULE = "";
            string strCode = "";
            int q = 0;

            nSDATE = 1;
            nEDATE = DateTime.DaysInMonth(Convert.ToInt32(VB.Val(VB.Left(argYYMM, 4))), Convert.ToInt32(VB.Right(argYYMM, 2)));

            Cursor.Current = Cursors.WaitCursor;
            //clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = " SELECT SCHEDULE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_SCHEDULE1 ";
                SQL = SQL + ComNum.VBLF + " WHERE YYMM = '" + argYYMM + "' ";
                SQL = SQL + ComNum.VBLF + "      AND SABUN = '" + clsType.User.Sabun + "' ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count > 0)
                {
                    //strSCHEDULE = dt.Rows[0]["SCHEDULE"].ToString().Trim();
                    strSCHEDULE = dt.Rows[0]["SCHEDULE"].ToString();
                }
                else
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                nSum_Day = 0;
                nSum_Eve = 0;
                nSum_Nit = 0;
                nSum_OFF = 0;
                nTEMP = 0;

                for (q = nSDATE - 1; q < nEDATE - 1; q++)
                {
                    strCode = VB.Trim(clsNurse.MidH(strSCHEDULE, nTEMP, 4));
                    switch (strCode)
                    {
                        case "MD":
                        case "D":
                        case "C1":
                        case "S":
                        case "S/2":
                        case "S10":
                        case "S11":
                        case "":
                            nSum_Day = nSum_Day + 1;
                            break;
                        case "S12":
                        case "S13":
                        case "S/1":
                        case "E":
                        case "ME":
                            nSum_Eve = nSum_Eve + 1;
                            break;
                        case "N":
                        case "MN":
                            nSum_Nit = nSum_Nit + 1;
                            break;
                        case "OFF":
                            nSum_OFF = nSum_OFF + 1;
                            break;
                        case "OFFA":
                            nSum_OFF = Convert.ToInt32(Convert.ToDouble(nSum_OFF) + 0.5);
                            nSum_Eve = Convert.ToInt32(Convert.ToDouble(nSum_Eve) + 0.5);
                            break;
                        case "OFFP":
                            nSum_OFF = Convert.ToInt32(Convert.ToDouble(nSum_OFF) + 0.5);
                            nSum_Day = Convert.ToInt32(Convert.ToDouble(nSum_Day) + 0.5);
                            break;
                        case "H/D1":
                            nSum_Day = Convert.ToInt32(Convert.ToDouble(nSum_Day) + 0.5);
                            break;
                        case "H/D2":
                            nSum_Eve = Convert.ToInt32(Convert.ToDouble(nSum_Eve) + 0.5);
                            break;
                        case "D/E":          //'간호부 DAY,EVENING      D/E
                            nSum_Day = nSum_Day + 1;
                            nSum_Eve = nSum_Eve + 1;
                            break;
                        case "E/N":          //'간호부 EVENING,NIGHT        E/N
                            nSum_Eve = nSum_Eve + 1;
                            nSum_Nit = nSum_Nit + 1;
                            break;
                        case "D/L":          //'간호부 LONG DAY     D/L
                            nSum_Day = nSum_Day + 1;
                            nSum_Eve = Convert.ToInt32(Convert.ToDouble(nSum_Eve) + 0.5);
                            break;
                        case "N/L":          //'간호부 LONG NIGHT       N/L
                            nSum_Nit = nSum_Nit + 1;
                            nSum_Eve = Convert.ToInt32(Convert.ToDouble(nSum_Eve) + 0.5);
                            break;
                    }
                    nTEMP = nTEMP + 4;
                }



                SQL = "";
                SQL = SQL + ComNum.VBLF + "UPDATE KOSMOS_PMPA.NUR_SCHEDULE1 SET ";
                SQL = SQL + ComNum.VBLF + " DTOT = " + nSum_Day + ",";
                SQL = SQL + ComNum.VBLF + " ETOT = " + nSum_Eve + ",";
                SQL = SQL + ComNum.VBLF + " NTOT = " + nSum_Nit + ",";
                SQL = SQL + ComNum.VBLF + " OFFTOT = " + nSum_OFF;
                SQL = SQL + ComNum.VBLF + " WHERE YYMM = '" + argYYMM + "' ";
                SQL = SQL + ComNum.VBLF + "       AND SABUN = '" + argSabun + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                //clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        private void btnHelp1List_Click(object sender, EventArgs e)
        {
            panHelp1List.Visible = false;
            txtHelpName.Text = "";
        }

        private void btnHelpList_Click(object sender, EventArgs e)
        {
            panHelpList.Visible = false;
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strData = "";

            panHelpList.Visible = true;

            ssList1_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT Code,Name,Jik ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_CODE ";
                SQL = SQL + ComNum.VBLF + "WHERE Gubun = '8' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY PrintRanking, Jik ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssList1_Sheet1.RowCount = dt.Rows.Count;
                ssList1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strData = VB.Left(dt.Rows[i]["CODE"].ToString().Trim() + VB.Space(8), 8);
                    strData += VB.Left(dt.Rows[i]["NAME"].ToString().Trim() + VB.Space(10), 10);
                    strData += Read_JikName(VB.Left(dt.Rows[i]["JIK"].ToString().Trim() + VB.Space(2), 2));
                    ssList1_Sheet1.Cells[i, 0].Text = strData;
                }
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private void btnHelpList_Click_1(object sender, EventArgs e)
        {
            panHelpList.Visible = false;
        }

        private void btnHelp1_Click(object sender, EventArgs e)
        {
            panHelp1List.Visible = true;
        }

        private void btnHelp1List_Click_1(object sender, EventArgs e)
        {
            txtHelpName.Text = "";
            panHelp1List.Visible = false;
        }

        private void ssView1_KeyDown(object sender, KeyEventArgs e)
        {
            //int i, j = 0;
            //int nCol2 = 0;
            //int nCol3 = 0;
            //int nHDay = 0;                    //휴일인 날짜
            string strYY = "";                //콤보 박스 YY
            string strMM = "";                //콤보 박스 MM
            string strDate = "";              //날짜 비교
            string strDateH1 = "";
            string strDateH2 = "";
            string strDay = "";               //월의 처음 날짜
            string strHDay = "";              //월의 처음 날짜 (휴일 check)
            //string strDay1 = "";              //월의 처음 요일
            string strNewday = "";            //시스템의 현재 YYMM
            //string strDept = "";              //부서
            //string strSABUN = "";             //사번
            //string strName = "";              //성명
            //string strJik = "";               //직책
            //string strSCHEDULE = "";          //근무내역
            //string strTotSchedule = "";       //Tot 근무내역
            string strROWID = "";
            //string strOldDept = "";           //OLD 부서
            //string strOldJik = "";            //OLD 직책
            //string strOldSchedule = "";       //OLD 스케줄

            string strSysDate = "";

            ComFunc cf = new ComFunc();

            ssView1_Sheet1.Rows[nCurrRow].Locked = false;

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            strNewday = VB.Left(strSysDate, 4) + VB.Mid(strSysDate, 6, 2);
            strYY = VB.Left(cboYYMM.Text, 4);
            strMM = VB.Mid(cboYYMM.Text, 6, 2);
            strDay = strYY + "-" + strMM + "-01";
            strHDay = strYY + "-" + strMM + "-01";
            strDate = strYY + "-" + strMM + "-01";
            if (VB.IsDate(strDate) == true)
            {
                strDateH1 = Convert.ToDateTime(strDate).ToString("yyyy-MM-dd");
                strDate = Convert.ToDateTime(strDate).ToString("yyyy-MM-dd");
            }
            strROWID = "";
            strROWID = ssView1_Sheet1.Cells[nCurrRow, 39].Text.Trim();
            strDateH2 = cf.READ_LASTDAY(clsDB.DbCon, strDate);

            if (nCurrRow < 6)
            {
                return;
            }

            if (e.KeyCode == Keys.F11 && e.Shift == false)
            {
                if (GstrWardCode == "GAN" || clsType.User.Sabun == "04349" || clsType.User.Sabun == "08985" || clsType.User.Sabun == "22394" || clsType.User.Sabun == "42646")
                {
                    DELETE_RTN(strROWID);
                }
                else
                {
                    if (string.Compare(VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 6, 2), strNewday) > 0)
                    {
                        DELETE_RTN(strROWID);
                    }
                }

                ssView1_Sheet1.Rows[nCurrRow].Remove();
            }


            if (nCurrCol >= nDateH1 + 3 && nCurrCol <= nDateH2 + 3)
            {
                if (e.Shift == false)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.F2:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[1];
                            moveNextCol();
                            break;
                        case Keys.F3:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[2];
                            moveNextCol();
                            break;
                        case Keys.F4:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[3];
                            moveNextCol();
                            //SendKeys.Send("{Enter}");
                            break;
                        case Keys.F5:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[4];
                            moveNextCol();
                            break;
                        case Keys.F6:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[5];
                            moveNextCol();
                            break;
                        case Keys.F7:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[6];
                            moveNextCol();
                            break;
                        case Keys.F8:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[7];
                            moveNextCol();
                            break;
                        case Keys.F9:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[8];
                            moveNextCol();
                            break;
                    }

                }
                else
                {
                    switch (e.KeyValue)
                    {
                        case 49:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[9];
                            moveNextCol();
                            break;
                        case 50:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[10];
                            moveNextCol();
                            break;
                        case 51:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[11];
                            moveNextCol();
                            break;
                        case 52:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[12];
                            moveNextCol();
                            break;
                        case 53:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[13];
                            moveNextCol();
                            break;
                        case 54:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[14];
                            moveNextCol();
                            break;
                        case 55:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[15];
                            moveNextCol();
                            break;
                        case 56:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[16];
                            moveNextCol();
                            break;
                        case 57:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[17];
                            moveNextCol();
                            break;

                        case 81:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[18];
                            moveNextCol();
                            break;
                        case 87:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[19];
                            moveNextCol();
                            break;
                        case 69:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[20];
                            moveNextCol();
                            break;
                        case 82:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[21];
                            moveNextCol();
                            break;
                        case 84:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[22];
                            moveNextCol();
                            break;
                        case 85:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[24];
                            moveNextCol();
                            break;
                        case 73:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[25];
                            moveNextCol();
                            break;

                        case 89:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[23];
                            moveNextCol();
                            break;

                        case 65:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[26];
                            moveNextCol();
                            break;
                        case 66:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[27];
                            moveNextCol();
                            break;
                        case 67:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[28];
                            moveNextCol();
                            break;
                        case 68:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[29];
                            moveNextCol();
                            break;
                        //case 69:
                        //    ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[30];
                        //    moveNextCol();
                        //    break;
                        case 70:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[31];
                            moveNextCol();
                            break;
                        case 71:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[32];
                            moveNextCol();
                            break;
                        case 72:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[33];
                            moveNextCol();
                            break; //add
                        case 74:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[34];
                            moveNextCol();
                            break;
                        case 75:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[35];
                            moveNextCol();
                            break;
                        case 76:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[36];
                            moveNextCol();
                            break;
                        case 77:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[37];
                            moveNextCol();
                            break;
                        case 78:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[38];
                            moveNextCol();
                            break;
                        case 79:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = strChuCode[39];
                            moveNextCol();
                            break;

                        case 113:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = "교육";
                            moveNextCol();
                            break;
                        case 114:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = "훈련";
                            moveNextCol();
                            break;
                        case 115:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = "휴가";
                            moveNextCol();
                            break;
                        case 116:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = "조퇴";
                            moveNextCol();
                            break;
                        case 117:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = "생휴";
                            moveNextCol();
                            break;
                        case 118:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = "결근";
                            moveNextCol();
                            break;
                        case 119:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = "병가";
                            moveNextCol();
                            break;
                        case 120:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = "경조";
                            moveNextCol();
                            break;
                        case 121:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = "분휴";
                            moveNextCol();
                            break;
                        case 122:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = "월차";
                            moveNextCol();
                            break;
                        case 123:
                            ssView1_Sheet1.Cells[nCurrRow, nCurrCol].Text = "반휴";
                            moveNextCol();
                            break;
                    }

                }
            }

            if (GstrWardCode != "OPD")
            {
                Total_Read();
            }


        }

        void DELETE_RTN(string strRowid)
        {
            string SQL = "";    //Query문
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " SELECT Sabun FROM " + ComNum.DB_PMPA + "NUR_SCHEDULE1 ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strRowid + "'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    SQL = "";
                    SQL = "DELETE " + ComNum.DB_PMPA + "NUR_SCHEDULE1 ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strRowid + "'";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        dt.Dispose();
                        dt = null;
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                }

                dt.Dispose();
                dt = null;

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void ssList1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int k = 0;
            int nRow = 0;
            int nCol3 = 0;
            int nHDay = 0;
            //string strOK = "";
            string strSabun1 = "";
            string strSabun2 = "";
            string strName = "";
            string strJik = "";
            string strYY = "";
            string strMM = "";
            string strHDay = "";

            ComFunc cf = new ComFunc();

            strYY = VB.Left(cboYYMM.Text, 4);
            strMM = VB.Mid(cboYYMM.Text, 6, 2);
            strHDay = strYY + "-" + strMM + "-01";

            strSabun1 = VB.Left(ssList1_Sheet1.Cells[e.Row, e.Column].Text, 8).Trim();
            strName = VB.Mid(ssList1_Sheet1.Cells[e.Row, e.Column].Text, 9, 10).Trim();
            strJik = (VB.Mid(ssList1_Sheet1.Cells[e.Row, e.Column].Text, 19, 15) + VB.Space(15)).Trim();

            for (i = 6; i < ssView1_Sheet1.RowCount; i++)
            {
                strSabun2 = ssView1_Sheet1.Cells[i, 1].Text.Trim();

                if (strSabun2 == "")
                {
                    nRow = i;
                }
                else if (strSabun1 == strSabun2)
                {
                    ComFunc.MsgBox("이미 해당 사번이 등록됨");
                    return;
                }
            }

            ssView1_Sheet1.RowCount++;

            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 0].Text = cboDept.Text.Trim();
            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 1].Text = strSabun1;
            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 2].Text = strName;
            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 3].Text = strJik;

            panHelpList.Visible = false;

            Cursor.Current = Cursors.WaitCursor;


            //휴일인 경우에 Color Display
            nCol3 = 4;
            try
            {
                for (i = nDateH1; i <= nDateH2; i++)
                {
                    SQL = "";
                    SQL = "Select JobDate  ";
                    SQL = SQL + ComNum.VBLF + "  FROM BAS_JOB ";
                    SQL = SQL + ComNum.VBLF + " WHERE HolyDay = '*' ";
                    SQL = SQL + ComNum.VBLF + "   AND JobDate = TO_DATE('" + strHDay + "' ,'YYYY-MM-DD')";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        for (k = 0; k < dt.Rows.Count; k++)
                        {
                            nHDay = Convert.ToInt32(VB.Val(VB.Left(dt.Rows[k]["JOBDATE"].ToString().Trim(), 2)));

                            ssView1_Sheet1.Cells[0, nCol3 + nHDay - 1, 4, nCol3 + nHDay - 1].BackColor = Color.Red;
                            ssView1_Sheet1.Cells[6, nCol3 + nHDay - 1, ssView1_Sheet1.RowCount - 1, nCol3 + nHDay - 1].BackColor = Color.Red;
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    strHDay = cf.DATE_ADD(clsDB.DbCon, strHDay, 1);
                }

                ssView1.Focus();

                cf = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private void ssList2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int k = 0;
            int nRow = 0;
            int nCol3 = 0;
            int nHDay = 0;
            //string strOK = "";
            string strSabun1 = "";
            string strSabun2 = "";
            string strName = "";
            string strJik = "";
            string strYY = "";
            string strMM = "";
            string strHDay = "";

            ComFunc cf = new ComFunc();

            strYY = VB.Left(cboYYMM.Text, 4);
            strMM = VB.Mid(cboYYMM.Text, 6, 2);
            strHDay = strYY + "-" + strMM + "-01";

            strName = VB.Left(ssList2_Sheet1.Cells[e.Row, e.Column].Text, 10).Trim();
            strSabun1 = VB.Mid(ssList2_Sheet1.Cells[e.Row, e.Column].Text, 11, 8).Trim();
            strJik = (VB.Mid(ssList2_Sheet1.Cells[e.Row, e.Column].Text, 19, 15) + VB.Space(15)).Trim();

            for (i = 6; i < ssView1_Sheet1.RowCount; i++)
            {
                strSabun2 = ssView1_Sheet1.Cells[i, 1].Text.Trim();

                if (strSabun2 == "")
                {
                    nRow = i;
                }
                else if (strSabun1 == strSabun2)
                {
                    ComFunc.MsgBox("이미 해당 사번이 등록됨");
                    return;
                }
            }

            ssView1_Sheet1.RowCount++;

            btnSave.Enabled = true;

            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 0].Text = cboDept.Text.Trim();
            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 1].Text = strSabun1;
            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 2].Text = strName;
            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 3].Text = strJik;

            panHelp1List.Visible = false;
            txtHelpName.Text = "";

            Cursor.Current = Cursors.WaitCursor;


            //휴일인 경우에 Color Display
            nCol3 = 4;
            try
            {
                for (i = nDateH1; i <= nDateH2; i++)
                {
                    SQL = "";
                    SQL = "Select TO_CHAR(JobDate,'YYYY-MM-DD') JOBDATE  ";
                    SQL = SQL + ComNum.VBLF + "  FROM BAS_JOB ";
                    SQL = SQL + ComNum.VBLF + " WHERE HolyDay = '*' ";
                    SQL = SQL + ComNum.VBLF + "   AND JobDate = TO_DATE('" + strHDay + "' ,'YYYY-MM-DD')";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        for (k = 0; k < dt.Rows.Count; k++)
                        {
                            nHDay = Convert.ToInt32(VB.Val(VB.Right(dt.Rows[k]["JOBDATE"].ToString().Trim(), 2)));

                            ssView1_Sheet1.Cells[0, nCol3 + nHDay - 1, 4, nCol3 + nHDay - 1].BackColor = Color.Red;
                            ssView1_Sheet1.Cells[6, nCol3 + nHDay - 1, ssView1_Sheet1.RowCount - 1, nCol3 + nHDay - 1].BackColor = Color.Red;
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    strHDay = cf.DATE_ADD(clsDB.DbCon, strHDay, 1);
                }

                ssView1.Focus();

                cf = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private void ssView1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right && GstrWardCode == "OPD")
            {
                ssView1_Sheet1.Cells[e.Row, e.Column].Text = mstrMacro;
            }

            nCurrRow = e.Row;
            nCurrCol = e.Column;
            panHelpList.Visible = false;
            panHelp1List.Visible = false;
            txtHelpName.Text = "";
        }

        private void ssView1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string[] strData = null;

            FarPoint.Win.Spread.CellType.TextCellType txtCell = null;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //부서에 대한 ComboBox
                if (e.Column == 0 && e.Row > 5)
                {
                    if (ssView1_Sheet1.Cells[e.Row, 0].CellType.ToString() == "ComboBoxCellType")
                    {
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    SQL = "";
                    SQL = "SELECT Name FROM " + ComNum.DB_PMPA + "NUR_CODE ";
                    SQL = SQL + ComNum.VBLF + " WHERE Gubun = '2' ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY PrintRanking ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strData = new string[dt.Rows.Count];

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            strData[i] = dt.Rows[i]["NAME"].ToString().Trim();
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    clsSpread.gSpreadComboDataSetEx1(ssView1, e.Row, 0, e.Row, 0, strData, false);
                }

                if ((e.Column >= 4 && e.Column <= (nDateH2 + 4)) && e.Row > 5)
                {
                    txtCell = new FarPoint.Win.Spread.CellType.TextCellType();
                    txtCell.CharacterCasing = CharacterCasing.Upper;
                    txtCell.Multiline = false;
                    txtCell.MaxLength = 10;
                    ssView1_Sheet1.Cells[e.Row, e.Column].CellType = txtCell;
                }

                txtCell = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private void ssView1_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            if (FstrCheck == "OK")
            {
                return;
            }

            //int i = 0;
            //int j = 0;
            //int k = 0;
            //int nNewData = 0;
            //int nSSData = 0;
            //string SSData = "";
            //string strYData = "";

            nCurrRow = e.NewRow;
            nCurrCol = e.NewColumn;

            ssView1_Sheet1.Rows[e.Row].Locked = false;

            if (cboDept.Text.Trim() == "마취과" || cboDept.Text.Trim() == "OR")
            {
                if (nCurrRow >= 6 && nCurrCol >= 4)
                {
                    ssView1_Sheet1.Cells[nCurrRow, 43].Text = "Y";
                }
            }

            if (e.Column != e.NewColumn || e.Row != e.NewRow)
            {
                ssView1_Sheet1.Cells[e.Row, 0].CellType = new FarPoint.Win.Spread.CellType.TextCellType();
                ssView1_Sheet1.Cells[e.Row, 3].CellType = new FarPoint.Win.Spread.CellType.TextCellType();
            }

            if (GstrWardCode != "OPD")
            {
                FstrCheck = "OK";

                if ((e.Column != e.NewColumn || e.Row != e.NewRow) && Check_Schedule(ssView1_Sheet1.Cells[e.Row, e.Column].Text, e.Column, e.Row) == "NO")
                {
                    ComFunc.MsgBox("[N-OFF-D] 근무는 입력 불가능합니다." + ComNum.VBLF + "변경하시거나 간호부에 문의하시기 바랍니다.");
                    ssView1_Sheet1.Cells[e.Row, e.Column].Text = "";
                }

                FstrCheck = "";
            }

            if (GstrWardCode != "OPD")
            {
                FstrCheck = "OK";

                if ((e.Column != e.NewColumn || e.Row != e.NewRow) && Check_Schedule2(ssView1_Sheet1.Cells[e.Row, e.Column].Text, e.Column, e.Row) == "NO")
                {
                    ComFunc.MsgBox("[D/L1-E/L1-N/L1] 근무는 입력 불가능합니다." + ComNum.VBLF + "변경하시거나 간호부에 문의하시기 바랍니다.");
                    ssView1_Sheet1.Cells[e.Row, e.Column].Text = "";
                }

                FstrCheck = "";
            }

        }

        private string Check_Schedule(string argText, int argCol, int argRow)
        {
            string str1 = "";
            string str2 = "";
            string str3 = "";
            string rtnVar = "";

            if (clsType.User.Sabun == "42646")
            {
                rtnVar = "OK";
                return rtnVar;
            }

            switch (argText.Trim())
            {
                case "N":
                case "N1":
                    str1 = argText.Trim();
                    str2 = ssView1_Sheet1.Cells[argRow, argCol + 1].Text.Trim();
                    str3 = ssView1_Sheet1.Cells[argRow, argCol + 2].Text.Trim();
                    break;
                case "OFF":
                    str1 = ssView1_Sheet1.Cells[argRow, argCol - 1].Text.Trim();
                    str2 = argText.Trim();
                    str3 = ssView1_Sheet1.Cells[argRow, argCol + 1].Text.Trim();
                    break;
                case "D":
                case "D1":
                    str1 = ssView1_Sheet1.Cells[argRow, argCol - 2].Text.Trim();
                    str2 = ssView1_Sheet1.Cells[argRow, argCol - 1].Text.Trim();
                    str3 = argText.Trim();
                    break;
                default:
                    break;
            }

            if ((str1 == "N" && str2 == "OFF" && str3 == "D") || (str1 == "N1" && str2 == "OFF" && str3 == "D1"))
            {
                rtnVar = "NO";
            }
            else
            {
                rtnVar = "OK";
            }

            return rtnVar;
        }

        private string Check_Schedule2(string argText, int argCol, int argRow)
        {
            string str1 = "";
            string str2 = "";
            string str3 = "";
            string rtnVar = "";

            if (clsType.User.Sabun == "42646")
            {
                rtnVar = "OK";
                return rtnVar;
            }

            switch (argText.Trim())
            {
                case "D/L1":
                    str1 = argText.Trim();
                    str2 = ssView1_Sheet1.Cells[argRow, argCol + 1].Text.Trim();
                    str3 = ssView1_Sheet1.Cells[argRow, argCol + 2].Text.Trim();
                    break;
                case "E/L1":
                    str1 = ssView1_Sheet1.Cells[argRow, argCol - 1].Text.Trim();
                    str2 = argText.Trim();
                    str3 = ssView1_Sheet1.Cells[argRow, argCol + 1].Text.Trim();
                    break;
                case "N/L1":
                    str1 = ssView1_Sheet1.Cells[argRow, argCol - 2].Text.Trim();
                    str2 = ssView1_Sheet1.Cells[argRow, argCol - 1].Text.Trim();
                    str3 = argText.Trim();
                    break;
                default:
                    break;
            }

            if (str1 == "D/L1" && str2 == "E/L1" && str3 == "N/L1")
            {
                rtnVar = "NO";
            }
            else
            {
                rtnVar = "OK";
            }

            return rtnVar;
        }


        private void ssView3_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (GstrWardCode == "OPD")
            {
                if (e.Row == 1 || e.Row == 3)
                {
                    mstrMacro = ssView3_Sheet1.Cells[e.Row, e.Column].Text.Trim();
                }
            }

            ssView3_Sheet1.Cells[FnRow, FnCOL].ForeColor = Color.Black;
            ssView3_Sheet1.Cells[FnRow, FnCOL].BackColor = Color.White;

            ssView3_Sheet1.Cells[e.Row, e.Column].ForeColor = Color.Black;
            ssView3_Sheet1.Cells[e.Row, e.Column].BackColor = Color.White;
        }

        private void ssView3_TextTipFetch(object sender, FarPoint.Win.Spread.TextTipFetchEventArgs e)
        {
            if (e.Row == 1 || e.Row == 3)
            {
                DataTable dt = null;
                string SQL = "";    //Query문
                string SqlErr = ""; //에러문 받는 변수

                string strText = "";

                try
                {
                    SQL = "";
                    SQL = " SELECT STIME,ETIME,REMARK FROM KOSMOS_ADM.INSA_GUNTAECODE ";
                    SQL = SQL + ComNum.VBLF + " WHERE TRIM(NURCODE) = '" + ssView3_Sheet1.Cells[e.Row, e.Column].Text.Trim() + "' ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        return;
                    }

                    strText = "출근시간 : " + dt.Rows[0]["STIME"].ToString().Trim() + " 퇴근시간 : " + dt.Rows[0]["ETIME"].ToString().Trim() + "    " + dt.Rows[0]["REMARK"].ToString().Trim();

                    dt.Dispose();
                    dt = null;

                    FnRow = e.Row;
                    FnCOL = e.Column;

                    e.TipText = strText;
                    //e.TipWidth = 500;
                    FarPoint.Win.Spread.TipAppearance ta = new FarPoint.Win.Spread.TipAppearance(Color.Ivory, Color.Black, new Font("맑은 고딕", 11));
                    ssView3.TextTipAppearance = ta;
                    ssView3.TextTipDelay = 0;
                    e.ShowTip = true;
                }
                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
            }
        }

        void moveNextCol()
        {
            if (ssView1_Sheet1.ActiveColumnIndex == ssView1_Sheet1.ColumnCount - 1)
            {
                return;
            }

            int nOldRow = 0;
            int nOldCol = 0;

            nOldRow = ssView1_Sheet1.ActiveRowIndex;
            nOldCol = ssView1_Sheet1.ActiveColumnIndex;

            ssView1_Sheet1.SetActiveCell(ssView1_Sheet1.ActiveRowIndex, ssView1_Sheet1.ActiveColumnIndex + 1);

            nCurrRow = ssView1_Sheet1.ActiveRowIndex;
            nCurrCol = ssView1_Sheet1.ActiveColumnIndex;

            ssView1_Sheet1.Rows[nCurrRow].Locked = true;


            if (GstrWardCode != "OPD")
            {
                FstrCheck = "OK";

                if (nOldCol != nCurrCol && Check_Schedule(ssView1_Sheet1.Cells[nOldRow, nOldCol].Text, nOldCol, nOldRow) == "NO")
                {
                    ComFunc.MsgBox("[N-OFF-D] 근무는 입력 불가능합니다." + ComNum.VBLF + "변경하시거나 간호부에 문의하시기 바랍니다.");
                    ssView1_Sheet1.Cells[nOldRow, nOldCol].Text = "";
                }

                FstrCheck = "";
            }
        }


        string getDeptName(string strDeptCode)
        {
            string rtnVar = "";

            switch (strDeptCode)
            {
                case "033101":
                    rtnVar = "간호부";
                    break;
                case "033102":
                    rtnVar = "OR";
                    break;
                case "033103":
                    rtnVar = "마취과";
                    break;
                case "033104":
                    rtnVar = "NR";
                    break;
                case "033105":
                    rtnVar = "DR";
                    break;
                case "033106":
                    rtnVar = "SICU";
                    break;
                case "033107":
                    rtnVar = "공급실";
                    break;
                case "033108":
                    rtnVar = "HD";
                    break;
                case "033109":
                    rtnVar = "ER";
                    break;
                //case "033102":      rtnVar = "수술실"      ;     break;
                case "033110":
                    rtnVar = "OPD";
                    break;
                case "033111":
                    rtnVar = "정신과";
                    break;
                case "033112":
                    rtnVar = "3A";
                    break;
                case "033113":
                    rtnVar = "3B";
                    break;
                case "033114":
                    rtnVar = "4A";
                    break;
                case "033116":
                    rtnVar = "5W";
                    break;
                case "033117":
                    rtnVar = "6W";
                    break;
                case "033118":
                    rtnVar = "7W";
                    break;
                case "033119":
                    rtnVar = "8W";
                    break;
                //case "033106":      rtnVar = "MICU"       ;     break;
                case "033121":
                    rtnVar = "HU";
                    break;
                case "033126":
                    rtnVar = "6A";
                    break;
                case "033127":
                    rtnVar = "3W";
                    break;
                case "033125":
                    rtnVar = "4W";
                    break;
                case "044501":
                    rtnVar = "종합건진";
                    break;
                case "101743":
                    rtnVar = "32";
                    break;
                case "101744":
                    rtnVar = "33";
                    break;
                case "101752":
                    rtnVar = "40";
                    break;
                case "101753":
                    rtnVar = "50";
                    break;
                case "101746":
                    rtnVar = "53";
                    break;
                case "101747":
                    rtnVar = "55";
                    break;
                case "101754":
                    rtnVar = "60";
                    break;
                case "101748":
                    rtnVar = "63";
                    break;
                case "101749":
                    rtnVar = "65";
                    break;
                case "101755":
                    rtnVar = "70";
                    break;
                case "101750":
                    rtnVar = "73";
                    break;
                case "101751":
                    rtnVar = "75";
                    break;
                case "101756":
                    rtnVar = "80";
                    break;
                case "101776":
                    rtnVar = "83";
                    break;
                case "101781":
                    rtnVar = "85";
                    break;
            }

            return rtnVar;
        }

        private void cboDept_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.Focus();
            }
        }

        private void frmScheduleEntry_Activated(object sender, EventArgs e)
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

        private void frmScheduleEntry_FormClosed(object sender, FormClosedEventArgs e)
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

        private void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clsType.User.Sabun == "42646")
            {
                if (cboDept.Text == "간호부")
                {
                    chk_Sort.Checked = false;
                }
                else
                {
                    chk_Sort.Checked = true;
                }
            }
            
        }

        private void btnHelpSearch_Click(object sender, EventArgs e)
        {
            ReadHelp1List();
        }

        private void txtHelpName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnHelpSearch.PerformClick();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i = 0;
            int j = 0;
            string strSabun = "";
            string strDate = "";
            string strYYMM = "";

            strYYMM = VB.Left(cboYYMM.Text, 4) + "-" +  VB.Mid(cboYYMM.Text, 6, 2);

            for (i = 0; i <= ssView1_Sheet1.RowCount - 1; i++)
            {
                strSabun = ssView1_Sheet1.Cells[i, 1].Text.Trim();
                if (strSabun != "")
                {
                    for (j = 4; j <= 34; j++)
                    {
                        strDate = strYYMM + "-" + (j - 3).ToString("00");
                        if (CHECK_GUNTAE(strSabun, strDate, ssView1_Sheet1.Cells[i, j].Text.Trim()) == "NO")
                        {
                            ssView1_Sheet1.Cells[i, j].BackColor = Color.Blue;
                            ssView1_Sheet1.Cells[i, j].ForeColor = Color.Yellow;
                        }
                    }
                }
            }
        }
    }
}
