using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmDoctPlan4.cs
    /// Description     : 의사별진료스케쥴
    /// Author          : 유진호
    /// Create Date     : 2018-02-06
    /// Update History  : 
    /// </summary>
    /// <history>      
    /// </history>
    /// <seealso cref= "\nurse\nropd\nropd17.frm(FrmDoctPlan4.frm) >> frmDoctPlan4.cs 폼이름 재정의" />	
    public partial class frmDoctPlan4 : Form, MainFormMessage
    {
        string mPara1 = "";
        private string GstrHelpCode = "";
        private string[] FstrHolyDay = new string[32];
        private string[] FstrYoil = new string[32];
        private string[] FstrYoilJin = new string[7];
        private string[] FstrYoilJin2 = new string[7];
        private string[] FstrYoilJin3 = new string[7];

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
        #endregion //MainFormMessage
        
        
        public frmDoctPlan4()
        {
            InitializeComponent();
        }

        public frmDoctPlan4(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
        }

        public frmDoctPlan4(MainFormMessage pform, string sPara1)
        {
            InitializeComponent();
            mCallForm = pform;
            mPara1 = sPara1;
        }

        private void frmDoctPlan4_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ComFunc.ReadSysDate(clsDB.DbCon);

            dateTimePicker1.Text = VB.Left(clsPublic.GstrSysDate, 7) + "-01";

            dateTimePicker1.CustomFormat = "yyyy-MM";

            int i = 0;
            int nYY = 0;
            int nMM = 0;

            chkInsa.Checked = false;

            if (clsType.User.Sabun == "16154")
            {
                chkInsa.Checked = true;
            }

            //년월 cbo Set
            nYY = (int)VB.Val(VB.Left(Convert.ToDateTime(clsPublic.GstrSysDate).AddMonths(-6).ToShortDateString(), 4));
            nMM = (int)VB.Val(VB.Mid(Convert.ToDateTime(clsPublic.GstrSysDate).AddMonths(-6).ToShortDateString(), 6, 2));
            cboYYMM.Items.Clear();
            for (i = 1; i < 13; i++)
            {
                cboYYMM.Items.Add(nYY.ToString("0000") + "-" + nMM.ToString("00"));
                nMM += 1;
                if (nMM == 13)
                {
                    nYY += 1;
                    nMM = 1;
                }
            }
            cboYYMM.SelectedIndex = 0;

            ssView1_Sheet1.Columns[95].Visible = false;
            ssView1_Sheet1.Columns[96].Visible = false;

            Screen_Clear();
            SetComboDept();
            //'폼 컬럼 set
            SET_SPREAD(ssView1_Sheet1);
        }

        private void frmDoctPlan4_Activated(object sender, EventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgActivedForm(this);
            }
        }

        private void frmDoctPlan4_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgUnloadForm(this);
            }
        }

        private void SetComboDept()
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (clsType.User.Sabun == "27603")
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT DEPTCODE, DEPTNAMEK FROM BAS_CLINICDEPT";
                    SQL = SQL + ComNum.VBLF + " WHERE DEPTCODE ='RD' ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        cboDept.Items.Clear();
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            cboDept.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim() + "." + dt.Rows[i]["DEPTNAMEK"].ToString().Trim());
                        }
                        cboDept.SelectedIndex = 0;
                    }
                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT DEPTCODE, DEPTNAMEK FROM BAS_CLINICDEPT";
                    SQL = SQL + ComNum.VBLF + " WHERE DeptCode <> 'RD' ";
                    if (GstrHelpCode != "")
                    {
                        if (VB.Len(GstrHelpCode) <= 2)
                        {
                            SQL = SQL + ComNum.VBLF + " AND DEPTCODE IN   ";
                            SQL = SQL + ComNum.VBLF + "        ( SELECT DrDept1 FROM BAS_DOCTOR WHERE DrCode IN (" + GstrHelpCode + "))";
                        }
                    }
                    SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        cboDept.Items.Clear();
                        cboDept.Items.Add("**.전체");
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            cboDept.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim() + "." + dt.Rows[i]["DEPTNAMEK"].ToString().Trim());
                        }
                        cboDept.SelectedIndex = 0;
                    }
                    dt.Dispose();
                    dt = null;
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
        }

        private void SET_SPREAD(FarPoint.Win.Spread.SheetView Spd)
        {
            int i = 0;
            int nCol = 0;

            Spd.RowCount = 0;

            Spd.AddColumnHeaderSpanCell(0, 0, 3, 1);
            Spd.ColumnHeader.Cells[0, 0].Value = "진료과";

            Spd.AddColumnHeaderSpanCell(0, 1, 3, 1);
            Spd.ColumnHeader.Cells[0, 1].Value = "의사";
            
            clsSpread.gSpreadHeaderLineBoder(ssView1, 0, 0, 2, 1, Color.Black, 1, false, false, true, false);

            for (i = 1; i <= 31; i++)
            {
                nCol = (i * 3) - 1;
                Spd.AddColumnHeaderSpanCell(0, nCol, 1, 3);
                Spd.AddColumnHeaderSpanCell(1, nCol, 1, 3);
                Spd.ColumnHeader.Cells[0, nCol].Value = i;
                Spd.ColumnHeader.Cells[1, nCol].Value = "";
                Spd.ColumnHeader.Cells[2, nCol].Value = "AM";
                Spd.ColumnHeader.Cells[2, nCol + 1].Value = "PM";
                Spd.ColumnHeader.Cells[2, nCol + 2].Value = "야";

                clsSpread.gSpreadHeaderLineBoder(ssView1, 0, nCol, 1, nCol, Color.Black, 1, false, false, true, false);
                clsSpread.gSpreadHeaderLineBoder(ssView1, 2, nCol, 2, nCol + 1, Color.FromArgb(190, 190, 190), 1, false, false, true, false);
                clsSpread.gSpreadHeaderLineBoder(ssView1, 2, nCol + 2, 2, nCol + 2, Color.Black, 1, false, false, true, false);
            }
        }


        private void Screen_Clear()
        {
            btnSearch.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;

            ssView1_Sheet1.RowCount = 0;
            ssView1_Sheet1.RowCount = 30;

            ssView1_Sheet1.Cells[0, 0, ssView1_Sheet1.RowCount - 1, ssView1_Sheet1.ColumnCount - 1].Text = "";
            ssView1_Sheet1.Cells[0, 0, ssView1_Sheet1.RowCount - 1, ssView1_Sheet1.ColumnCount - 1].BackColor = lbl_4.BackColor;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            btnSearchClick();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인
            if (btnSaveClick() == true)
            {
                Screen_Clear();
                cboYYMM.Focus();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Screen_Clear();
            cboYYMM.Focus();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                return; //권한 확인

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";
            string strFoot = "";
            string SysDate = "";

            SmartPrintRulesCollection prules = new SmartPrintRulesCollection();

            SysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            Cursor.Current = Cursors.WaitCursor;

            strFont1 = "/fn\"굴림체\" /fz\"16\" /fb0 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"10\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead1 = "/n/n/f1/C 의사별 진료 스케쥴" + "/n/n/n/n";
            //strHead2 = "/l/f2" + "진료년월 : " + cboYYMM.Text + VB.Space(20) + "인쇄일자 : " + SysDate;
            strHead2 = "/l/f2" + "진료년월 : " + dateTimePicker1.Value.ToString("yyyy-MM") + VB.Space(20) + "인쇄일자 : " + SysDate;
            strFoot = "1.진료  2.수술  3.특검  4.휴진  5.학회  6.휴가  7.출장  8.감염  9.OFF  D.교육";

            ssView1_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView1_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2 + ComNum.VBLF + strFoot;
            ssView1_Sheet1.PrintInfo.Margin.Left = 35;
            ssView1_Sheet1.PrintInfo.Margin.Right = 0;
            ssView1_Sheet1.PrintInfo.Margin.Top = 35;
            ssView1_Sheet1.PrintInfo.Margin.Bottom = 30;
            ssView1_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView1_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView1_Sheet1.PrintInfo.ShowBorder = true;
            ssView1_Sheet1.PrintInfo.ShowColor = false;
            ssView1_Sheet1.PrintInfo.ShowGrid = true;
            ssView1_Sheet1.PrintInfo.ShowShadows = false;
            ssView1_Sheet1.PrintInfo.UseMax = false;
            ssView1_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            //ssView1_Sheet1.PrintInfo.UseMax = true;
            //ssView1_Sheet1.PrintInfo.BestFitCols = true;
            //ssView1_Sheet1.PrintInfo.BestFitRows = true;

            //ssView1_Sheet1.PrintInfo.SmartPrintPagesTall = 1;
            //ssView1_Sheet1.PrintInfo.SmartPrintPagesWide = 1;

            //prules.Add(new BestFitColumnRule(ResetOption.None));
            //prules.Add(new LandscapeRule(ResetOption.None));
            //prules.Add(new ScaleRule(ResetOption.None, 1, (float)0.1, (float)0.1));

            //ssView1_Sheet1.PrintInfo.SmartPrintRules = prules;
            //ssView1_Sheet1.PrintInfo.UseSmartPrint = true;
            ssView1_Sheet1.PrintInfo.ZoomFactor = 0.5f;
            
            ssView1.PrintSheet(0);
        }

        private void btnPlan_Click(object sender, EventArgs e)
        {
            frmDoctPlan1 frmDoctPlan1X = new frmDoctPlan1();
            frmDoctPlan1X.StartPosition = FormStartPosition.CenterParent;
            frmDoctPlan1X.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearchClick()
        {
            int i = 0;
            int j = 0;
            //int K = 0;
            //int nREAD = 0;
            int nDay = 0;
            int nLastDay = 0;
            string strGbn = "";
            string strGbn2 = "";
            string strGbn3 = "";
            string strFDate = "";
            string strTDate = "";
            string strYoil = "";
            DayOfWeek yoil;
            string strDate = "";
            string strYYMM = "";
            string strDrCode = "";
            //string strSCHEDULE = "";
            
            DataTable dt = null;
            DataTable dt2 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                btnSearch.Enabled = false;

                //strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 6, 2);
                //strFDate = cboYYMM.Text + "-01";

                strYYMM = dateTimePicker1.Value.ToString("yyyyMM");
                strFDate = dateTimePicker1.Value.ToString("yyyy-MM-01");
                nLastDay = DateTime.DaysInMonth(Convert.ToDateTime(strFDate).Year, Convert.ToDateTime(strFDate).Month);
                strTDate = VB.Left(strFDate, 8) + ComFunc.LPAD(nLastDay.ToString(),2,"0");


                for (i = 0; i < FstrHolyDay.Length; i++) FstrHolyDay[i] = "";

                //'해당월에 의사별 스케쥴을 만들었는지 Check
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT COUNT(*) CNT FROM BAS_SCHEDULE ";
                SQL = SQL + ComNum.VBLF + " WHERE SchDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND SchDate<=TO_DATE('" + strTDate + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    if ((int)VB.Val(dt.Rows[0]["CNT"].ToString().Trim()) == 0)
                    {
                        if (ComFunc.MsgBoxQ("해당월의 스케쥴을 신규로 만드시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                        {
                            dt.Dispose();
                            dt = null;
                            return;
                        }
                    }
                }
                dt.Dispose();
                dt = null;


                //'일자별 휴일을 SET
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(JobDate,'DD') ILJA FROM BAS_JOB ";
                SQL = SQL + ComNum.VBLF + "WHERE JobDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND JobDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND HolyDay = '*' ";
                SQL = SQL + ComNum.VBLF + "  AND jobdate <> to_date('2014-06-04','yyyy-mm-dd') ";

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
                        nDay = (int)VB.Val(dt.Rows[i]["ILJA"].ToString().Trim());
                        FstrHolyDay[nDay] = "*";
                    }
                }

                

                //'일자별 요일을 SET
                for (i = 1; i <= nLastDay; i++)
                {
                    strDate = dateTimePicker1.Value.ToString("yyyy-MM") + "-" + VB.Format(i, "00");
                    yoil = Convert.ToDateTime(strDate).DayOfWeek;

                    switch (yoil)
                    {
                        case DayOfWeek.Monday:
                            FstrYoil[i] = "1";
                            strYoil = "월";
                            break;
                        case DayOfWeek.Tuesday:
                            FstrYoil[i] = "2";
                            strYoil = "화";
                            break;
                        case DayOfWeek.Wednesday:
                            FstrYoil[i] = "3";
                            strYoil = "수";
                            break;
                        case DayOfWeek.Thursday:
                            FstrYoil[i] = "4";
                            strYoil = "목";
                            break;
                        case DayOfWeek.Friday:
                            FstrYoil[i] = "5";
                            strYoil = "금";
                            break;
                        case DayOfWeek.Saturday:
                            FstrYoil[i] = "6";
                            strYoil = "토";
                            break;
                        case DayOfWeek.Sunday:
                            FstrYoil[i] = "7";
                            strYoil = "일";
                            break;
                    }

                    if (FstrHolyDay[i] != "*" && FstrYoil[i] == "7")
                    {
                        FstrHolyDay[i] = "#";
                    }
                    
                    ssView1_Sheet1.ColumnHeader.Cells[1, (i * 3) - 1].Value = strYoil;
                    
                    if (FstrHolyDay[i] == "*")
                    {
                        ssView1_Sheet1.ColumnHeader.Cells[1, (i * 3) - 1].BackColor = lbl_1.BackColor;
                    }
                    else if (FstrHolyDay[i] == "#")
                    {
                        ssView1_Sheet1.ColumnHeader.Cells[1, (i * 3) - 1].BackColor = lbl_2.BackColor;
                    }
                    else
                    {
                        ssView1_Sheet1.ColumnHeader.Cells[1, (i * 3) - 1].BackColor = lbl_4.BackColor;
                    }
                }


                //'진료과별 의사코드,성명을 READ
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT A.DrDept1, A.DrCode, A.DrName ";
                SQL = SQL + ComNum.VBLF + " FROM BAS_DOCTOR A, BAS_CLINICDEPT B ";
                SQL = SQL + ComNum.VBLF + "WHERE A.SCHEDULE IS  NULL ";
                if (chkInsa.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.DRCODE IN ( ";
                    SQL = SQL + ComNum.VBLF + "     SELECT B.DRCODE ";
                    SQL = SQL + ComNum.VBLF + "       FROM KOSMOS_ADM.INSA_MST A, KOSMOS_OCS.OCS_DOCTOR B ";
                    SQL = SQL + ComNum.VBLF + "      WHERE A.SABUN = B.SABUN ";
                    SQL = SQL + ComNum.VBLF + "        AND (TOIDAY > TO_DATE('" + strTDate + "','YYYY-MM-DD') OR TOIDAY IS NULL)";
                    SQL = SQL + ComNum.VBLF + "        AND KUNDAY <= TO_DATE('" + strTDate + "','YYYY-MM-DD')) ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.TOUR <> 'Y' ";
                }
                if (VB.Left(cboDept.Text, 2) != "**") SQL = SQL + ComNum.VBLF + "  AND DRDEPT1 = '" + VB.Left(cboDept.Text, 2) + "' ";
                SQL = SQL + ComNum.VBLF + "  AND A.DrDept1 NOT IN ('HD','HR','PT','TO','R6') ";
                SQL = SQL + ComNum.VBLF + "  AND A.DRDEPT1 = B.DEPTCODE(+)";
                if(clsType.User.IdNumber == "23767")
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.DRCODE not in ('0116') ";
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY B.PrintRanking, A.DrDept1, decode(a.drcode,'1102',1,'1104',2,'1114',3,'1113', 4, '1108',5 ,'1111', 6,'1120',7 ,'1119',8, '1107', 9, A.PrintRanking ) ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView1_Sheet1.RowCount = dt.Rows.Count;
                    ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strDrCode = dt.Rows[i]["DrCode"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["DrDept1"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DrName"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 95].Text = strDrCode;
                        if (VB.Right(strDrCode, 2) == "99")
                        {
                            ssView1_Sheet1.Rows[i].Visible = false;
                        }


                        //'진료여부를 READ
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(SchDate,'DD') ILJA,GbJin, GbJin2, GbJin3";
                        SQL = SQL + ComNum.VBLF + " FROM BAS_SCHEDULE ";
                        SQL = SQL + ComNum.VBLF + "WHERE DrCode = '" + strDrCode + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND SchDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "  AND SchDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";

                        SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt2.Rows.Count > 0)
                        {
                            for (j = 0; j < dt2.Rows.Count; j++)
                            {
                                nDay = (int)VB.Val(dt2.Rows[j]["ILJA"].ToString().Trim());
                                strGbn = dt2.Rows[j]["GbJin"].ToString().Trim();
                                strGbn2 = dt2.Rows[j]["GbJin2"].ToString().Trim();
                                strGbn3 = dt2.Rows[j]["GbJin3"].ToString().Trim();

                                ssView1_Sheet1.Cells[i, (nDay * 3) - 1].Text = strGbn;
                                clsSpread.gSpreadLineBoder(ssView1, i, (nDay * 3) - 1, i, (nDay * 3) - 1, Color.FromArgb(192, 192, 192), 1, false, false, true, false);
                                                                
                                if (strGbn == "1") ssView1_Sheet1.Cells[i, (nDay * 3) - 1].BackColor = lbl_1.BackColor;
                                else if (strGbn == "2") ssView1_Sheet1.Cells[i, (nDay * 3) - 1].BackColor = lbl_2.BackColor;
                                else if (strGbn == "3") ssView1_Sheet1.Cells[i, (nDay * 3) - 1].BackColor = lbl_3.BackColor;
                                else if (strGbn == "9") ssView1_Sheet1.Cells[i, (nDay * 3) - 1].BackColor = lbl_9.BackColor;
                                else if (strGbn == "A") ssView1_Sheet1.Cells[i, (nDay * 3) - 1].BackColor = lbl_10.BackColor;
                                else if (strGbn == "D") ssView1_Sheet1.Cells[i, (nDay * 3) - 1].BackColor = lbl_11.BackColor;
                                else if (strGbn == "E") ssView1_Sheet1.Cells[i, (nDay * 3) - 1].BackColor = lbl_12.BackColor;
                                else if (strGbn == "F") ssView1_Sheet1.Cells[i, (nDay * 3) - 1].BackColor = lbl_13.BackColor;
                                else if (string.Compare(strGbn, "3") > 0) ssView1_Sheet1.Cells[i, (nDay * 3) - 1].BackColor = lbl_4.BackColor;
                                else ssView1_Sheet1.Cells[i, (nDay * 3) - 1].BackColor = lbl_4.BackColor;


                                ssView1_Sheet1.Cells[i, (nDay * 3)].Text = strGbn2;
                                if (VB.Left(cboDept.Text, 2) != "PD")
                                {
                                    clsSpread.gSpreadLineBoder(ssView1, i, (nDay * 3), i, (nDay * 3), Color.Black, 1, false, false, true, false);
                                    clsSpread.gSpreadHeaderLineBoder(ssView1, i, (nDay * 3), i, (nDay * 3), Color.Black, 1, false, false, true, false);
                                }
                                else
                                {
                                    clsSpread.gSpreadLineBoder(ssView1, i, (nDay * 3), i, (nDay * 3), Color.FromArgb(192, 192, 192), 1, false, false, true, false);
                                    clsSpread.gSpreadHeaderLineBoder(ssView1, i, (nDay * 3), i, (nDay * 3), Color.FromArgb(192, 192, 192), 1, false, false, true, false);
                                }

                                if (strGbn2 == "1") ssView1_Sheet1.Cells[i, (nDay * 3)].BackColor = lbl_1.BackColor;
                                else if (strGbn2 == "2") ssView1_Sheet1.Cells[i, (nDay * 3)].BackColor = lbl_2.BackColor;
                                else if (strGbn2 == "3") ssView1_Sheet1.Cells[i, (nDay * 3)].BackColor = lbl_3.BackColor;
                                else if (strGbn2 == "9") ssView1_Sheet1.Cells[i, (nDay * 3)].BackColor = lbl_9.BackColor;
                                else if (strGbn2 == "A") ssView1_Sheet1.Cells[i, (nDay * 3)].BackColor = lbl_10.BackColor;
                                else if (strGbn2 == "D") ssView1_Sheet1.Cells[i, (nDay * 3)].BackColor = lbl_11.BackColor;
                                else if (strGbn2 == "E") ssView1_Sheet1.Cells[i, (nDay * 3)].BackColor = lbl_12.BackColor;
                                else if (strGbn2 == "F") ssView1_Sheet1.Cells[i, (nDay * 3)].BackColor = lbl_13.BackColor;
                                else if (string.Compare(strGbn2, "3") > 0) ssView1_Sheet1.Cells[i, (nDay * 3)].BackColor = lbl_4.BackColor;
                                else ssView1_Sheet1.Cells[i, (nDay * 3)].BackColor = lbl_4.BackColor;


                                ssView1_Sheet1.Cells[i, (nDay * 3) + 1].Text = strGbn3;
                                if (VB.Left(cboDept.Text, 2) != "PD")
                                {
                                    ssView1_Sheet1.Columns[(nDay * 3) + 1].Visible = false;
                                }
                                else
                                {
                                    ssView1_Sheet1.Columns[(nDay * 3) + 1].Visible = true;
                                    clsSpread.gSpreadLineBoder(ssView1, i, (nDay * 3) + 1, i, (nDay * 3) + 1, Color.Black, 1, false, false, true, false);
                                }

                                if (strGbn3 == "1") ssView1_Sheet1.Cells[i, (nDay * 3) + 1].BackColor = lbl_1.BackColor;
                                else ssView1_Sheet1.Cells[i, (nDay * 3) + 1].BackColor = lbl_4.BackColor;
                            }
                        }
                        else
                        {
                            ssView1_Sheet1.Cells[i, 96].Text = "Y";
                            //'의사별 요일별 스케쥴을 READ
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(SchDate,'DD') ILJA,GbJin, GbJin2, GbJin3 FROM BAS_SCHEDULE ";
                            SQL = SQL + ComNum.VBLF + "WHERE DrCode = '" + strDrCode + "' ";
                            SQL = SQL + ComNum.VBLF + "  AND SchDate >= TO_DATE('1990-01-01','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "  AND SchDate <= TO_DATE('1990-01-06','YYYY-MM-DD') ";

                            SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }

                            if (dt2.Rows.Count > 0)
                            {
                                for (j = 1; j <= 6; j++)
                                {
                                    FstrYoilJin[j] = " ";
                                    FstrYoilJin2[j] = " ";
                                }
                                for (j = 0; j < dt2.Rows.Count; j++)
                                {
                                    nDay = (int)VB.Val(dt2.Rows[j]["ILJA"].ToString().Trim());
                                    strGbn = dt2.Rows[j]["GbJin"].ToString().Trim();
                                    strGbn2 = dt2.Rows[j]["GbJin2"].ToString().Trim();
                                    strGbn3 = dt2.Rows[j]["GbJin3"].ToString().Trim();

                                    if (strGbn != " ") FstrYoilJin[nDay] = strGbn;
                                    if (strGbn2 != " ") FstrYoilJin2[nDay] = strGbn2;
                                    if (strGbn3 != " ") FstrYoilJin3[nDay] = strGbn3;
                                }

                                for (j = 1; j <= nLastDay; j++)
                                {
                                    if (ssView1_Sheet1.Cells[i, (j * 3) - 1].Text == "" && FstrYoil[j] != "7")
                                    {
                                        strGbn = FstrYoilJin[(int)VB.Val(FstrYoil[j])];
                                        ssView1_Sheet1.Cells[i, (j * 3) - 1].Text = strGbn;
                                        clsSpread.gSpreadLineBoder(ssView1, i, (j * 3) - 1, i, (j * 3) - 1, Color.FromArgb(192,192,192), 1, false, false, true, false);

                                        if (strGbn == "1") ssView1_Sheet1.Cells[i, (j * 3) - 1].BackColor = lbl_1.BackColor;
                                        else if (strGbn == "2") ssView1_Sheet1.Cells[i, (j * 3) - 1].BackColor = lbl_2.BackColor;
                                        else if (strGbn == "3") ssView1_Sheet1.Cells[i, (j * 3) - 1].BackColor = lbl_3.BackColor;
                                        else if (strGbn == "9") ssView1_Sheet1.Cells[i, (j * 3) - 1].BackColor = lbl_9.BackColor;
                                        else if (strGbn == "A") ssView1_Sheet1.Cells[i, (j * 3) - 1].BackColor = lbl_10.BackColor;
                                        else if (strGbn == "D") ssView1_Sheet1.Cells[i, (j * 3) - 1].BackColor = lbl_11.BackColor;
                                        else if (strGbn == "E") ssView1_Sheet1.Cells[i, (j * 3) - 1].BackColor = lbl_12.BackColor;
                                        else if (strGbn == "F") ssView1_Sheet1.Cells[i, (j * 3) - 1].BackColor = lbl_13.BackColor;
                                        else if (string.Compare(strGbn, "3") > 0) ssView1_Sheet1.Cells[i, (j * 3) - 1].BackColor = lbl_4.BackColor;
                                        else ssView1_Sheet1.Cells[i, (j * 3) - 1].BackColor = lbl_4.BackColor;
                                    }

                                    if (ssView1_Sheet1.Cells[i, (j * 3)].Text == "" && FstrYoil[j] != "7")
                                    {
                                        strGbn2 = FstrYoilJin2[(int)VB.Val(FstrYoil[j])];
                                        ssView1_Sheet1.Cells[i, (j * 3)].Text = strGbn2;

                                        if (strGbn2 == "1") ssView1_Sheet1.Cells[i, (j * 3)].BackColor = lbl_1.BackColor;
                                        else if (strGbn2 == "2") ssView1_Sheet1.Cells[i, (j * 3)].BackColor = lbl_2.BackColor;
                                        else if (strGbn2 == "3") ssView1_Sheet1.Cells[i, (j * 3)].BackColor = lbl_3.BackColor;
                                        else if (strGbn2 == "9") ssView1_Sheet1.Cells[i, (j * 3)].BackColor = lbl_9.BackColor;
                                        else if (strGbn2 == "A") ssView1_Sheet1.Cells[i, (j * 3)].BackColor = lbl_10.BackColor;
                                        else if (strGbn2 == "D") ssView1_Sheet1.Cells[i, (j * 3)].BackColor = lbl_11.BackColor;
                                        else if (strGbn2 == "E") ssView1_Sheet1.Cells[i, (j * 3)].BackColor = lbl_12.BackColor;
                                        else if (strGbn2 == "F") ssView1_Sheet1.Cells[i, (j * 3)].BackColor = lbl_13.BackColor;
                                        else if (string.Compare(strGbn2, "3") > 0) ssView1_Sheet1.Cells[i, (j * 3)].BackColor = lbl_4.BackColor;
                                        else ssView1_Sheet1.Cells[i, (j * 3)].BackColor = lbl_4.BackColor;
                                    }

                                    if (ssView1_Sheet1.Cells[i, (j * 3) + 1].Text == "" && FstrYoil[j] != "7")
                                    {
                                        strGbn3 = FstrYoilJin3[(int)VB.Val(FstrYoil[j])];
                                        ssView1_Sheet1.Cells[i, (j * 3) + 1].Text = strGbn3;

                                        if (strGbn3 == "1") ssView1_Sheet1.Cells[i, (j * 3) + 1].BackColor = lbl_1.BackColor;
                                        else ssView1_Sheet1.Cells[i, (j * 3) + 1].BackColor = lbl_4.BackColor;
                                    }
                                }
                            }
                        }
                        dt2.Dispose();
                        dt2 = null;
                    }
                }
                dt.Dispose();
                dt = null;


                //'토요일,휴일 SHEET에 DATA SET
                for (j = 1; j <= nLastDay; j++)
                {
                    if (FstrHolyDay[j] != " ")
                    {
                        if (FstrHolyDay[j] == "*")      //'휴일
                        {
                            ssView1_Sheet1.Cells[0, (j * 3) - 1, ssView1_Sheet1.RowCount - 1, (j * 3) + 1].Text = "4";
                            ssView1_Sheet1.Cells[0, (j * 3) - 1, ssView1_Sheet1.RowCount - 1, (j * 3) + 1].BackColor = lbl_4.BackColor;
                            ssView1_Sheet1.Cells[0, (j * 3) - 1, ssView1_Sheet1.RowCount - 1, (j * 3) + 1].Locked = true;
                        }
                        else if (FstrHolyDay[j] == "#")  //'토요일
                        {
                            ssView1_Sheet1.Cells[0, (j * 3) - 1, ssView1_Sheet1.RowCount - 1, (j * 3) + 1].BackColor = lbl_4.BackColor;
                        }
                    }

                    
                    //if (Convert.ToDateTime(cboYYMM.Text + "-" + VB.Format(j, "00")) < Convert.ToDateTime(clsPublic.GstrSysDate))
                    if (Convert.ToDateTime(dateTimePicker1.Value.ToString("yyyy-MM") + "-" + VB.Format(j, "00")) < Convert.ToDateTime(clsPublic.GstrSysDate))   //2018-11-19
                        {
                        if (clsType.User.IdNumber == "1625" || clsType.User.IdNumber == "23767" || clsType.User.IdNumber == "27603"|| clsType.User.IdNumber == "48345" || clsType.User.IdNumber == "27345")  //2019-11-04 박은지 감사 관련 임시 권한 부여
                        {

                        }
                        else
                        {
                            ssView1_Sheet1.Cells[0, (j * 3) - 1, ssView1_Sheet1.RowCount - 1, (j * 3) + 1].Locked = true;
                        }
                        
                    }
                }

                lblMsg.Text = "";
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
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

            //마취통증의학과 진료스케쥴 관리 임시권한 부여
            //정선희  48345
            //이윤정  27345
            //진단검의학과 팀장 01625
            if (clsType.User.IdNumber == "1625" ||  clsType.User.IdNumber == "23767" || clsType.User.IdNumber == "27603"||clsType.User.IdNumber == "48345" || clsType.User.IdNumber == "27345")       //2018-12-03 김복희팀장 무조건 활성화 //2019-11-04 박은지 감사 관련 임시 권한 부여
            {
                btnSave.Enabled = true;
            }
        }


        private bool btnSaveClick()
        {
            bool rtVal = false;
            string SQL = "";
            string SqlErr = "";
            //int intRowAffected = 0;

            int i = 0;
            int j = 0;
            int nLastDay = 0;
            string strFDate = "";
            //string strTDate = "";

            string strDrCode = "";
            //string strDate = "";
            //string strDayGbn = "";
            string strGbn = "";
            string strGbn2 = "";
            string strGbn3 = "";
            //string strYoil = "";
            //string strOldGbn = "";

            //string strOK = "";


            btnSave.Enabled = false;
            btnCancel.Enabled = false;

            //strFDate = cboYYMM.Text + "-01";

            strFDate = dateTimePicker1.Value.ToString("yyyy-MM");
            nLastDay = DateTime.DaysInMonth(Convert.ToDateTime(strFDate).Year, Convert.ToDateTime(strFDate).Month);
            
            SET_PC_DOCTOR(dateTimePicker1.Value.ToString("yyyy-MM"));



            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                for (i = 0; i < ssView1_Sheet1.RowCount; i++)
                {
                    strDrCode = ssView1_Sheet1.Cells[i, 95].Text;

                    if (ssView1_Sheet1.Cells[i, 96].Text == "Y")
                    {
                        //'자료를 UPDATE
                        for (j = 1; j <= nLastDay; j++)
                        {
                            strGbn = ssView1_Sheet1.Cells[i, (j * 3) - 1].Text;
                            strGbn2 = ssView1_Sheet1.Cells[i, (j * 3)].Text;
                            strGbn3 = ssView1_Sheet1.Cells[i, (j * 3) + 1].Text;


                            if (CmdOK_Rtn(strGbn, strGbn2, strGbn3, strDrCode, j) == false)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                Cursor.Current = Cursors.Default;
                                return rtVal;
                            }                            
                        }
                    }
                }
                
                clsDB.setCommitTran(clsDB.DbCon);
                //ComFunc.MsgBox("저장 하였습니다.");
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

        private bool CmdOK_Rtn(string strGbn, string strGbn2, string strGbn3, string strDrCode, int nDay)
        {
            bool rtVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            string strDate = "";
            string strDayGbn = "";

            //strDate = cboYYMM.Text + "-" + VB.Format(nDay, "00");
            strDate = dateTimePicker1.Value.ToString("yyyy-MM") + "-" + VB.Format(nDay, "00");

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ROWID FROM  BAS_SCHEDULE ";
            SQL = SQL + ComNum.VBLF + "WHERE DrCode = '" + strDrCode + "' ";
            SQL = SQL + ComNum.VBLF + "  AND SchDate = TO_DATE('" + strDate + "','YYYY-MM-DD') ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return rtVal;
            }

            strDayGbn = "1";
            if (FstrHolyDay[nDay] == "*") strDayGbn = "3";
            if (FstrHolyDay[nDay] == "#") strDayGbn = "2";

            if (dt.Rows.Count > 0)
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "UPDATE BAS_SCHEDULE SET GBDAY = '" + strDayGbn + "',  ";
                SQL = SQL + ComNum.VBLF + " GBJIN = '" + strGbn + "',";
                SQL = SQL + ComNum.VBLF + " GBJINEND = '',";
                SQL = SQL + ComNum.VBLF + " GBJIN2 = '" + strGbn2 + "', ";
                SQL = SQL + ComNum.VBLF + " GBJIN3 = '" + strGbn3 + "' ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + dt.Rows[0]["ROWID"].ToString().Trim() + "'";
            }
            else
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO BAS_SCHEDULE (DRCODE, SCHDATE, GBDAY, GBJIN, GBJINEND, GBJIN2, GBJIN3) ";
                SQL = SQL + ComNum.VBLF + " VALUES('" + strDrCode + "', TO_DATE('" + strDate + "','YYYY-MM-DD'),'" + strDayGbn + "',";
                SQL = SQL + ComNum.VBLF + " '" + strGbn + "',' ','" + strGbn2 + "','" + strGbn3 + "' ) ";
            }

            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                dt.Dispose();
                dt = null;
                return rtVal;
            }

            dt.Dispose();
            dt = null;
            rtVal = true;
            return rtVal;
        }

        private bool SET_PC_DOCTOR(string argYYMM)
        {
            bool rtVal = false;            
            DataTable dt = null;
            DataTable dt2 = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            //'마취과 스케쥴 자동입력
            //'의뢰서 작업 주임 김현욱 2013-07-17
            int i = 0;
            int j = 0;
            //int nREAD = 0;
            int nLastDay = 0;
            string strSDATE = "";
            string strEDATE = "";
            string strDate = "";
            string strDrCode = "";

            string strHuil = "";
            string strYoil = "";
            DayOfWeek yoil;

            //strSDATE = cboYYMM.Text + "-01";
            strSDATE = dateTimePicker1.Value.ToString("yyyy-MM") + "-01";
            nLastDay = DateTime.DaysInMonth(Convert.ToDateTime(strSDATE).Year, Convert.ToDateTime(strSDATE).Month);

            //strEDATE = cboYYMM.Text + VB.Format(nLastDay, "0#");
            strEDATE = dateTimePicker1.Value.ToString("yyyy-MM-") + VB.Format(nLastDay, "0#");
            strDate = VB.Left(strEDATE, 8);
            
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT * ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_SCHEDULE";
                SQL = SQL + ComNum.VBLF + " WHERE DRCODE IN ( ";
                SQL = SQL + ComNum.VBLF + " SELECT DRCODE ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_DOCTOR";
                SQL = SQL + ComNum.VBLF + " WHERE DEPTCODE = 'PC'";
                SQL = SQL + ComNum.VBLF + "    AND GBOUT = 'N') ";
                SQL = SQL + ComNum.VBLF + "    AND SCHDATE >= TO_DATE('" + strSDATE + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND SCHDATE <= TO_DATE('" + strEDATE + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count > 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    dt.Dispose();
                    dt = null;
                    return rtVal;
                }
                dt.Dispose();
                dt = null;


                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT DRCODE ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_DOCTOR";
                SQL = SQL + ComNum.VBLF + " WHERE DEPTCODE = 'PC'";
                SQL = SQL + ComNum.VBLF + "    AND GBOUT = 'N'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strDrCode = dt.Rows[i]["DRCODE"].ToString().Trim();
                        for (j = 1; j <= (int)VB.Val(VB.Right(strEDATE, 2)); j++)
                        {
                            strHuil = "";
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(JobDate,'DD') ILJA FROM BAS_JOB ";
                            SQL = SQL + ComNum.VBLF + "WHERE JobDate = TO_DATE('" + strDate + VB.Format(j, "00") + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "  AND HolyDay = '*' ";
                            SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return rtVal;
                            }

                            if (dt2.Rows.Count > 0)
                            {
                                strHuil = "OK";
                            }
                            dt2.Dispose();
                            dt2 = null;

                            yoil = Convert.ToDateTime(strDate + VB.Format(j, "00")).DayOfWeek;

                            switch (yoil)
                            {
                                case DayOfWeek.Monday:
                                    strYoil = "월요일";
                                    break;
                                case DayOfWeek.Tuesday:
                                    strYoil = "화요일";
                                    break;
                                case DayOfWeek.Wednesday:
                                    strYoil = "수요일";
                                    break;
                                case DayOfWeek.Thursday:
                                    strYoil = "목요일";
                                    break;
                                case DayOfWeek.Friday:
                                    strYoil = "금요일";
                                    break;
                                case DayOfWeek.Saturday:
                                    strYoil = "토요일";
                                    break;
                                case DayOfWeek.Sunday:
                                    strYoil = "일요일";
                                    break;
                            }
                            
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "INSERT INTO KOSMOS_PMPA.BAS_SCHEDULE(";
                            SQL = SQL + ComNum.VBLF + " DRCODE, SCHDATE, GBDAY, GBJIN, GBJIN2) VALUES (";
                            if (strHuil != "OK" && strYoil == "토요일")
                            {
                                SQL = SQL + ComNum.VBLF + "'" + strDrCode + "', TO_DATE('" + strDate + VB.Format(j, "00") + "','YYYY-MM-DD'), '2','1','4') ";
                            }
                            else if (strHuil != "OK" && strYoil != "토요일")
                            {
                                SQL = SQL + ComNum.VBLF + "'" + strDrCode + "', TO_DATE('" + strDate + VB.Format(j, "00") + "','YYYY-MM-DD'), '1','1','1') ";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "'" + strDrCode + "', TO_DATE('" + strDate + VB.Format(j, "00") + "','YYYY-MM-DD'), '3','4','4') ";
                            }
                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
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
                }

                
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

        private void btnSearch_Enter(object sender, EventArgs e)
        {
            lblMsg.Text = "스케쥴이 입력않된 일자는 요일별스케쥴을 기준하여 자동 작성됩니다.";
        }

        private void btnSearch_Leave(object sender, EventArgs e)
        {
            lblMsg.Text = "";
        }

        private void cboYYMM_Enter(object sender, EventArgs e)
        {
            lblMsg.Text = "스케쥴이 입력않된 일자는 요일별스케쥴을 기준하여 자동 작성됩니다.";
        }

        private void cboYYMM_Leave(object sender, EventArgs e)
        {
            lblMsg.Text = "";
        }

        private void ssView1_EditModeOff(object sender, EventArgs e)
        {
            string strData = "";

            if (ssView1_Sheet1.ActiveColumnIndex < 2) return;

            strData = ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, ssView1_Sheet1.ActiveColumnIndex].Text;

            if ((string.Compare(strData, "1") < 0 || string.Compare(strData, "9") > 0) && strData != "A" && strData != "D" && strData != "E" && strData != "F")
            {
                ComFunc.MsgBox("구분은 A,D,E,F, 1-9 까지만 가능함");
                strData = "";
            }

            if ((ssView1_Sheet1.ActiveColumnIndex + 1) % 3 != 2)
            {
                if (strData == "1") ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, ssView1_Sheet1.ActiveColumnIndex].BackColor = lbl_1.BackColor;
                else if (strData == "2") ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, ssView1_Sheet1.ActiveColumnIndex].BackColor = lbl_2.BackColor;
                else if (strData == "3") ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, ssView1_Sheet1.ActiveColumnIndex].BackColor = lbl_3.BackColor;
                else if (strData == "9") ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, ssView1_Sheet1.ActiveColumnIndex].BackColor = lbl_9.BackColor;
                else if (strData == "A") ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, ssView1_Sheet1.ActiveColumnIndex].BackColor = lbl_10.BackColor;
                else if (strData == "D") ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, ssView1_Sheet1.ActiveColumnIndex].BackColor = lbl_11.BackColor;
                else if (strData == "E") ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, ssView1_Sheet1.ActiveColumnIndex].BackColor = lbl_12.BackColor;
                else if (strData == "F") ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, ssView1_Sheet1.ActiveColumnIndex].BackColor = lbl_13.BackColor;
                else if (string.Compare(strData, "3") > 0) ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, ssView1_Sheet1.ActiveColumnIndex].BackColor = lbl_4.BackColor;
                else ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, ssView1_Sheet1.ActiveColumnIndex].BackColor = lbl_4.BackColor;
            }
            else
            {
                if (strData == "1") ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, ssView1_Sheet1.ActiveColumnIndex].BackColor = lbl_1.BackColor;
                else ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, ssView1_Sheet1.ActiveColumnIndex].BackColor = lbl_4.BackColor;
            }

            ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, ssView1_Sheet1.ActiveColumnIndex].Text = ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, ssView1_Sheet1.ActiveColumnIndex].Text.Trim();

            ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, 96].Text = "Y";            
        }

        private void dateTimePicker1_Enter(object sender, EventArgs e)
        {
            lblMsg.Text = "스케쥴이 입력않된 일자는 요일별스케쥴을 기준하여 자동 작성됩니다.";
        }

        private void dateTimePicker1_Leave(object sender, EventArgs e)
        {
            lblMsg.Text = "";
        }
    }
}
