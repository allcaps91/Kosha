using System;
using System.Data;
using System.Windows.Forms;
using ComLibB;
using FarPoint.Win.Spread;
using System.Drawing;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComSupLibB
{
    /// <summary>
    /// Class Name      : MedOprNr
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-04-12
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= D:\psmh\nurse\nrinfo\Frm선택식등록" >> frmSelectSeve.cs 폼이름 재정의" />

    public partial class frmSelectSeve : Form
    {
        string GstrWardCode = "";
        string GstrWardCodes = "";

        public frmSelectSeve()
        {
            InitializeComponent();
        }

        public frmSelectSeve(string GstrWardCodeX, string GstrWardCodesX)
        {
            GstrWardCode = GstrWardCodeX;
            GstrWardCodes = GstrWardCodesX;
            InitializeComponent();
        }

        private void frmSelectSeve_Load(object sender, EventArgs e)
        {
            btnSave.Enabled = false;

            SS1_Clear();

            if (GstrWardCode == "")
            {
                GstrWardCode = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE");
            }

            ComboWard_SET();

            SS1_Sheet1.Columns[2].Visible = false;

            SS2_Sheet1.Columns[6].Visible = false;
            SS2_Sheet1.Columns[9].Visible = false;
            SS2_Sheet1.Columns[12].Visible = false;
            SS2_Sheet1.Columns[13].Visible = false;

            TxtDate.Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            Search();
            SMenu_Set_Title();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;


            strTitle = VB.Pstr(ComboWard.Text, " ", 1) + " 선택식 환자 명단";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("작업일자 : " + TxtDate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(SS2, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnPrint2_Click(object sender, EventArgs e)
        {
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;


            strTitle = TxtTitle.Text;

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("인쇄일자 : " + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Save() == false)
            {
                return;
            }
            Search();
        }

        private bool Save()
        {
            string strROWID = "";
            string strChange = "";
            string strRemark = "";
            string strPano = "";
            string[] strMenu = new string[6];

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;
            int j = 0;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            //선택식은 12시 이전까지만 입력가능

            if (clsType.User.Sabun != "04444" && clsType.User.Sabun != "04349" && clsType.User.Sabun != "24737" && clsType.User.Sabun != "27050")
            {
                MessageBox.Show("선택식은 영양팀에서만 입력가능합니다.(영양팀에 문의하세요)"
                    , "확인");
                return rtnVal;
            }

            try
            {
                for (i = 1; i <= SS2_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
                {
                    for (j = 1; j <= 5; j++)
                    {
                        strMenu[j] = "N";
                    }

                    strMenu[4] = "";

                    strPano = SS2_Sheet1.Cells[i - 1, 2].Text.Trim();

                    if (Convert.ToBoolean(SS2_Sheet1.Cells[i - 1, 5].Value) == true)
                    {
                        strMenu[1] = "Y";
                    }

                    strMenu[4] = SS2_Sheet1.Cells[i - 1, 6].Text.Trim();

                    if (Convert.ToBoolean(SS2_Sheet1.Cells[i - 1, 7].Value) == true)
                    {
                        strMenu[2] = "Y";
                    }
                    if (Convert.ToBoolean(SS2_Sheet1.Cells[i - 1, 8].Value) == true)
                    {
                        strMenu[3] = "Y";
                    }
                    if (Convert.ToBoolean(SS2_Sheet1.Cells[i - 1, 9].Value) == true)
                    {
                        strMenu[5] = "Y";
                    }

                    strRemark = SS2_Sheet1.Cells[i - 1, 10].Text.Trim();
                    strChange = SS2_Sheet1.Cells[i - 1, 12].Text.Trim();
                    strROWID = SS2_Sheet1.Cells[i - 1, 13].Text.Trim();

                    if (strChange == "Y")
                    {
                        if (strROWID != "")
                        {
                            SQL = "";
                            SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_SORDER_HIS ";
                            SQL = SQL + ComNum.VBLF + " SELECT * FROM " + ComNum.DB_PMPA + "DIET_SORDER ";
                            SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }

                            SQL = "";
                            SQL = " UPDATE " + ComNum.DB_PMPA + "DIET_SORDER SET ";
                            SQL = SQL + ComNum.VBLF + " MDate =TO_DATE('" + (TxtDate.Text).Trim() + "','YYYY-MM-DD') , ";
                            SQL = SQL + ComNum.VBLF + " Menu1 ='" + strMenu[1] + "', ";
                            SQL = SQL + ComNum.VBLF + " Menu2 ='" + strMenu[2] + "', ";
                            SQL = SQL + ComNum.VBLF + " Menu3 ='" + strMenu[3] + "', ";
                            SQL = SQL + ComNum.VBLF + " Menu4 ='" + strMenu[4] + "', ";
                            SQL = SQL + ComNum.VBLF + " Menu5 ='" + strMenu[5] + "', ";
                            SQL = SQL + ComNum.VBLF + " Remark ='" + strRemark + "', ";
                            SQL = SQL + ComNum.VBLF + " EntDate =TO_DATE('" + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-") + "','YYYY-MM-DD HH24:MI'), ";
                            SQL = SQL + ComNum.VBLF + " EntSabun = " + clsType.User.Sabun + " ";
                            SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + strROWID + "' ";

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
                            SQL = "INSERT INTO KOSMOS_PMPA.DIET_SORDER (MDate,Pano,Menu1,Menu2,Menu3,Menu4,Menu5,Remark, ";
                            SQL = SQL + ComNum.VBLF + " EntDate, EntSabun ) VALUES ";
                            SQL = SQL + ComNum.VBLF + " ( TO_DATE('" + (TxtDate.Text).Trim() + "','YYYY-MM-DD'),'" + strPano + "', ";
                            SQL = SQL + ComNum.VBLF + " '" + strMenu[1] + "','" + strMenu[2] + "','" + strMenu[3] + "', ";
                            SQL = SQL + ComNum.VBLF + " '" + strMenu[4] + "','" + strMenu[5] + "','" + strRemark + "', ";
                            SQL = SQL + ComNum.VBLF + "   TO_DATE('" + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-") + "','YYYY-MM-DD HH24:MI'), ";
                            SQL = SQL + ComNum.VBLF + " " + clsType.User.Sabun + " ) ";

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
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void Search()
        {
            SMenu_Set();
            SMenu_Set_Title();

            if (GstrWardCode == "HD")
            {
                READ_OPD();
            }
            else if (GstrWardCode == "A1" || GstrWardCode == "A2" || GstrWardCode == "A3" || GstrWardCode == "A4")
            {
                READ_DIET_ADD();
            }
            else
            {
                READ_IPD();
            }

            Use_Possible();
        }

        private void btnSave1_Click(object sender, EventArgs e)
        {
            if (Save1() == false)
            {
                return;
            }
            SS1_Clear();
        }

        private bool Save1()
        {
            string[] strMenu = new string[8];
            string strROWID = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;
            int j = 0;
            int k = 0;

            Cursor.Current = Cursors.WaitCursor;

            for (i = 1; i <= 7; i++)
            {
                strMenu[i] = "";
            }

            for (i = 0; i <= 9; i++)
            {
                if (i != 4 && i != 6)
                {
                    k = k + 1;
                    for (j = 3; j <= SS1_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); j++)
                    {
                        if (SS1_Sheet1.Cells[j - 1, i - 1].Text != "")
                        {
                            strMenu[k] = strMenu[k] + SS1_Sheet1.Cells[j - 1, i - 1].Text.Trim() + "{}";
                        }
                    }
                }
            }

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //일자세팅 확인

                SQL = "";
                SQL = " SELECT ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_SMENU ";
                SQL = SQL + ComNum.VBLF + " WHERE MDate =TO_DATE('" + (TxtDate.Text).Trim() + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strROWID = dt.Rows[0]["ROWID"].ToString().Trim();

                    if (strROWID != "")
                    {
                        SQL = "";
                        SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_SMENU_HIS ";
                        SQL = SQL + ComNum.VBLF + " SELECT * FROM " + ComNum.DB_PMPA + "DIET_SMENU ";
                        SQL = SQL + ComNum.VBLF + "  WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }


                        SQL = " UPDATE " + ComNum.DB_PMPA + "DIET_SMENU SET ";
                        SQL = SQL + ComNum.VBLF + " Menu11 ='" + strMenu[1] + "', ";
                        SQL = SQL + ComNum.VBLF + " Menu12 ='" + strMenu[2] + "', ";
                        SQL = SQL + ComNum.VBLF + " Menu13 ='" + strMenu[3] + "', ";
                        SQL = SQL + ComNum.VBLF + " Menu21 ='" + strMenu[4] + "', ";
                        SQL = SQL + ComNum.VBLF + " Menu22 ='" + strMenu[5] + "', ";
                        SQL = SQL + ComNum.VBLF + " Menu31 ='" + strMenu[6] + "', ";
                        SQL = SQL + ComNum.VBLF + " Menu32 ='" + strMenu[7] + "', ";
                        SQL = SQL + ComNum.VBLF + " EntDate =TO_DATE('" + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-") + "','YYYY-MM-DD HH24:MI'), ";
                        SQL = SQL + ComNum.VBLF + " EntSabun = " + clsType.User.Sabun + " ";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + strROWID + "' ";

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
                }
                else
                {
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "DIET_SMENU (MDate,Menu11,Menu12,Menu13,Menu21,Menu22,Menu31,Menu32, ";
                    SQL = SQL + ComNum.VBLF + " EntDate, EntSabun ) VALUES ";
                    SQL = SQL + ComNum.VBLF + " ( TO_DATE('" + (TxtDate.Text).Trim() + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + " '" + strMenu[1] + "','" + strMenu[2] + "','" + strMenu[3] + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strMenu[4] + "','" + strMenu[5] + "','" + strMenu[6] + "','" + strMenu[7] + "', ";
                    SQL = SQL + ComNum.VBLF + "   TO_DATE('" + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-") + "','YYYY-MM-DD HH24:MI'), ";
                    SQL = SQL + ComNum.VBLF + " " + clsType.User.Sabun + " ) ";
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

        }

        private void SS1_Clear()
        {
            int i = 0;
            int j = 0;

            for (i = 3; i <= SS1_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
            {
                for (j = 1; j <= 9; j++)
                {
                    SS1_Sheet1.Cells[i - 1, j - 1].Text = "";
                }
            }
        }

        private void SMenu_Set()
        {
            int i = 0;
            int j = 0;
            int k = 0;
            //int nCnt = 0;
            string[] strMenu = new string[8];
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            for (i = 1; i <= 7; i++)
            {
                strMenu[i] = "";
            }

            SS1_Clear();
            SS2_Sheet1.Cells[0, 0, SS2_Sheet1.RowCount - 1, SS2_Sheet1.ColumnCount - 1].Text = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //식단조회
                SQL = "";
                SQL = " SELECT Menu11,Menu12,Menu13,Menu21,Menu22,Menu31,Menu32  ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_SMENU ";
                SQL = SQL + ComNum.VBLF + " WHERE MDate =TO_DATE('" + (TxtDate.Text).Trim() + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strMenu[1] = dt.Rows[0]["Menu11"].ToString().Trim();
                    strMenu[2] = dt.Rows[0]["Menu12"].ToString().Trim();
                    strMenu[3] = dt.Rows[0]["Menu13"].ToString().Trim();
                    strMenu[4] = dt.Rows[0]["Menu21"].ToString().Trim();
                    strMenu[5] = dt.Rows[0]["Menu22"].ToString().Trim();
                    strMenu[6] = dt.Rows[0]["Menu31"].ToString().Trim();
                    strMenu[7] = dt.Rows[0]["Menu32"].ToString().Trim();

                    for (i = 1; i <= 9; i++)
                    {
                        if (i != 4 && i != 7)
                        {
                            k = k + 1;
                            for (j = 1; j < VB.I(strMenu[k], "{}"); j++)
                            {
                                SS1_Sheet1.Cells[(j + 2) - 1, i - 1].Text = VB.Pstr(strMenu[k], "{}", j);
                            }
                        }
                    }
                }
                else
                {
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void SMenu_Set_Title()
        {
            ComFunc CF = new ComFunc();

            TxtTitle.Text = CF.DATE_ADD(clsDB.DbCon, TxtDate.Text, 1) + "일  선택식 메뉴";
            SMenu_Set();
        }

        private void Use_Possible()
        {
            if (clsType.User.Sabun == "04349" || clsType.User.Sabun == "04444")
            {
                btnSave.Enabled = true;
                btnSave1.Enabled = true;
            }
            else
            {
                if (string.Compare(TxtDate.Text, ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")) < 0)
                {
                    btnSave.Enabled = false;
                }
                else
                {
                    btnSave.Enabled = true;
                }
                btnSave1.Enabled = false;
            }
        }

        private void READ_OPD()
        {

            string strOK = "";
            string strNewData = "";
            string strOldData = "";
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            DataTable dtTemp = null;
            string SqlErr = "";
            int nRow = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT Pano,SName,DeptCode,'HD' as WardCode,'0' as RoomCode,Age,Sex   ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE  ACTDATE = TO_DATE('" + (TxtDate.Text).Trim() + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND (JIN ='6' OR DEPTCODE='HD' ) ";


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
                        if (ChkOK.Checked == true)
                        {
                            strOK = "";
                        }
                        else
                        {
                            strOK = "OK";
                        }
                        //선택식등록 확인
                        SQL = "";
                        SQL = " SELECT TO_CHAR(MDate,'YYYY-MM-DD') MDate,Pano,Menu1,Menu2,Menu3,Menu4,Menu5,Remark,ROWID  ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.DIET_SORDER ";
                        SQL = SQL + ComNum.VBLF + "   WHERE Pano='" + dt.Rows[i]["Pano"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND MDate=TO_DATE('" + (TxtDate.Text).Trim() + "','YYYY-MM-DD') ";

                        SqlErr = clsDB.GetDataTable(ref dtTemp, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dtTemp.Rows.Count > 0)
                        {
                            strOK = "OK";
                        }
                        if (strOK == "OK")
                        {
                            nRow += 1;
                            SS2_Sheet1.RowCount = nRow;
                            strNewData = dt.Rows[i]["WardCode"].ToString().Trim();

                            if (strNewData != strOldData)
                            {
                                SS2_Sheet1.Cells[nRow - 1, 0].Text = strNewData;
                            }

                            SS2_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                            SS2_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["Pano"].ToString().Trim();
                            SS2_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["SName"].ToString().Trim();
                            SS2_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["Sex"].ToString().Trim();

                            if (dtTemp.Rows.Count > 0)
                            {
                                if (dtTemp.Rows[0]["Menu1"].ToString().Trim() == "Y")
                                {
                                    SS2_Sheet1.Cells[nRow - 1, 5].Value = true;     //'아침선택
                                }
                                SS2_Sheet1.Cells[nRow - 1, 6].Text = dtTemp.Rows[0]["Menu4"].ToString().Trim();  //'기호식품(아침)
                                if (dtTemp.Rows[0]["Menu2"].ToString().Trim() == "Y")
                                {
                                    SS2_Sheet1.Cells[nRow - 1, 7].Value = true;     //'점심선택
                                }
                                if (dtTemp.Rows[0]["Menu3"].ToString().Trim() == "Y")
                                {
                                    SS2_Sheet1.Cells[nRow - 1, 8].Value = true;     //'저녁선택
                                }
                                if (dtTemp.Rows[0]["Menu5"].ToString().Trim() == "Y")
                                {
                                    SS2_Sheet1.Cells[nRow - 1, 9].Value = true;    //'잡곡밥
                                }
                                SS2_Sheet1.Cells[nRow - 1, 10].Text = dtTemp.Rows[0]["ReMark"].ToString().Trim();
                                SS2_Sheet1.Cells[nRow - 1, 12].Text = dtTemp.Rows[0]["ROWID"].ToString().Trim();
                            }

                            strOldData = dt.Rows[i]["WardCode"].ToString().Trim();
                        }

                        dtTemp.Dispose();
                        dtTemp = null;
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void READ_DIET_ADD()
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            DataTable dtTemp = null;
            string SqlErr = "";
            //string strPano = "";
            string strOK = "";
            string strNewData = "";
            string strOldData = "";
            int nRow = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT a.Pano,b.SName, '00' as DeptCode,a.WardCode,a.RoomCode, 0 as Age,b.Sex    ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "DIET_NEWORDER A, " + ComNum.DB_PMPA + "BAS_PATIENT B";
                SQL = SQL + ComNum.VBLF + "   WHERE A.ACTDATE =TO_DATE('" + (TxtDate.Text).Trim() + "','YYYY-MM-DD')";
                if (GstrWardCode == "A1")//'이침추가분"
                {
                    SQL = SQL + ComNum.VBLF + "    AND A.ENTDATE >= TO_DATE('" + (TxtDate.Text).Trim() + " 05:01" + "' ,'YYYY-MM-DD HH24:MI') ";
                    SQL = SQL + ComNum.VBLF + "    AND A.ENTDATE <= TO_DATE('" + (TxtDate.Text).Trim() + " 09:00" + "','YYYY-MM-DD HH24:MI')";
                    SQL = SQL + ComNum.VBLF + "    AND A.DIETDAY='1'";

                }
                else if (GstrWardCode == "A2")// '점심추가분
                {
                    SQL = SQL + ComNum.VBLF + "    AND A.ENTDATE >= TO_DATE('" + (TxtDate.Text).Trim() + " 11:01" + "','YYYY-MM-DD HH24:MI')";
                    SQL = SQL + ComNum.VBLF + "    AND A.ENTDATE <= TO_DATE('" + (TxtDate.Text).Trim() + " 12:30" + "','YYYY-MM-DD HH24:MI')";
                    SQL = SQL + ComNum.VBLF + "    AND A.DIETDAY='2'";
                }
                else if (GstrWardCode == "A3")// '저녁추가분
                {
                    SQL = SQL + ComNum.VBLF + "    AND A.ENTDATE >= TO_DATE('" + (TxtDate.Text).Trim() + " 16:01" + "','YYYY-MM-DD HH24:MI')";
                    SQL = SQL + ComNum.VBLF + "    AND A.ENTDATE <= TO_DATE('" + (TxtDate.Text).Trim() + " 17:30" + "','YYYY-MM-DD HH24:MI')";
                    SQL = SQL + ComNum.VBLF + "    AND A.DIETDAY='3'";

                }
                else if (GstrWardCode == "A4")// ' 아침 변경분

                {
                    SQL = SQL + ComNum.VBLF + "    AND A.ENTDATE >= TO_DATE('" + (TxtDate.Text).Trim() + " 00:01" + "','YYYY-MM-DD HH24:MI')";
                    SQL = SQL + ComNum.VBLF + "    AND A.ENTDATE <= TO_DATE('" + (TxtDate.Text).Trim() + " 05:00" + "','YYYY-MM-DD HH24:MI')";
                    SQL = SQL + ComNum.VBLF + "    AND A.DIETDAY='1'";
                }
                SQL = SQL + ComNum.VBLF + " AND A.PANO = B.PANO ";
                SQL = SQL + ComNum.VBLF + " ORDER BY A.ROOMCODE   ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    SS2_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (ChkOK.Checked == true)
                        {
                            strOK = "";
                        }
                        else
                        {
                            strOK = "OK";
                        }

                        //선택식등록

                        SQL = "";
                        SQL = " SELECT TO_CHAR(MDate,'YYYY-MM-DD') MDate,Pano,Menu1,Menu2,Menu3,Menu4,Menu5,Remark,ROWID  ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.DIET_SORDER ";
                        SQL = SQL + ComNum.VBLF + "   WHERE Pano='" + dt.Rows[i]["Pano"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND MDate=TO_DATE('" + (TxtDate.Text).Trim() + "','YYYY-MM-DD') ";

                        SqlErr = clsDB.GetDataTable(ref dtTemp, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dtTemp.Rows.Count > 0)
                        {
                            strOK = "OK";
                        }
                        if (strOK == "OK")
                        {
                            nRow = nRow + 1;

                            SS2_Sheet1.RowCount = nRow;

                            strNewData = dt.Rows[i]["WardCode"].ToString().Trim();
                            if (strNewData != strOldData)
                            {
                                SS2_Sheet1.Cells[nRow - 1, 0].Text = strNewData;
                            }

                            SS2_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                            SS2_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["Pano"].ToString().Trim();
                            SS2_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["SName"].ToString().Trim();
                            SS2_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["Sex"].ToString().Trim();

                            if (dtTemp.Rows.Count > 0)
                            {
                                if (dtTemp.Rows[0]["Menu1"].ToString().Trim() == "Y")
                                {
                                    SS2_Sheet1.Cells[nRow - 1, 5].Value = true;     //'아침선택
                                }
                                SS2_Sheet1.Cells[nRow - 1, 6].Text = dtTemp.Rows[0]["Menu4"].ToString().Trim();  //'기호식품(아침)
                                if (dtTemp.Rows[0]["Menu2"].ToString().Trim() == "Y")
                                {
                                    SS2_Sheet1.Cells[nRow - 1, 7].Value = true;     //'점심선택
                                }
                                if (dtTemp.Rows[0]["Menu3"].ToString().Trim() == "Y")
                                {
                                    SS2_Sheet1.Cells[nRow - 1, 8].Value = true;     //'저녁선택
                                }
                                if (dtTemp.Rows[0]["Menu5"].ToString().Trim() == "Y")
                                {
                                    SS2_Sheet1.Cells[nRow - 1, 9].Value = true;    //'잡곡밥
                                }
                                SS2_Sheet1.Cells[nRow - 1, 10].Text = dtTemp.Rows[0]["ReMark"].ToString().Trim();
                                SS2_Sheet1.Cells[nRow - 1, 12].Text = dtTemp.Rows[0]["ROWID"].ToString().Trim();
                            }

                            strOldData = dt.Rows[i]["WardCode"].ToString().Trim();
                        }

                        dtTemp.Dispose();
                        dtTemp = null;
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void READ_IPD()
        {
            DataTable RsReadIpd = null;
            DataTable RsTemp = null;
            DataTable RsTemp2 = null;
            DataTable RsTemp3 = null;
            DataTable RsTemp4 = null;
            int i = 0;
            int j = 0;
            int nRow = 0;
            //int nRead = 0;
            int nREAD2 = 0;
            //int nRead3 = 0;
            //string strPano = "";
            string strNewData = "";
            string strOldData = "";
            string strOK = "";
            string strOK1 = "";
            string[] strDay = new string[4];
            //string strDietDay = "";
            int[] nCnt = new int[11];
            string SqlErr = "";
            string SQL = "";

            Cursor.Current = Cursors.WaitCursor;

            for (i = 1; i <= 10; i++)
            {
                nCnt[i] = 0;
            }

            for (i = 1; i <= 3; i++)
            {
                strDay[i] = "";
            }

            TxtTitle.Text = "";

            try
            {
                SQL = "";
                SQL = " SELECT Pano,SName,DeptCode,WardCode,RoomCode,Age,Sex  ";
                if (string.Compare(TxtDate.Text, ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")) < 0)
                {
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_BM ";
                    SQL = SQL + ComNum.VBLF + " WHERE JOBDATE = TO_DATE('" + (TxtDate.Text).Trim() + "','YYYY-MM-DD') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                    SQL = SQL + ComNum.VBLF + " WHERE  ACTDATE IS NULL ";
                    if (GstrWardCodes == "TOP" || clsType.User.Sabun == "04349" || clsType.User.Sabun == "20433" || clsType.User.Sabun == "20442" || clsType.User.Sabun == "20193" || clsType.User.Sabun == "04444")
                    {
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " AND GBSTS NOT IN ('6','7','9') ";// '퇴원계산서인쇄, 퇴원수납완료,취소 제외
                    }
                }
                if (ComboWard.Text != "*.전체")
                {
                    if (GstrWardCode == "IU")
                    {
                        if (GstrWardCodes == "SICU" || VB.Left(ComboWard.Text, 2) == "외과")
                        {
                            SQL = SQL + ComNum.VBLF + "  AND  WardCode = 'IU' AND RoomCode=233 ";
                        }
                        else if (GstrWardCodes == "MICU" || VB.Left(ComboWard.Text, 2) == "내과")
                        {
                            SQL = SQL + ComNum.VBLF + "  AND  WardCode = 'IU' AND RoomCode=234 ";
                        }
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "  AND  WardCode = '" + VB.Right(ComboWard.Text, 2) + "' ";
                    }
                }

                SQL = SQL + ComNum.VBLF + "     AND PANO <'90000000' ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY ROOMCODE ";

                SqlErr = clsDB.GetDataTable(ref RsReadIpd, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (RsReadIpd.Rows.Count > 0)
                {
                    SS2_Sheet1.RowCount = RsReadIpd.Rows.Count;

                    for (i = 0; i < RsReadIpd.Rows.Count; i++)
                    {
                        strOK = "";
                        if (ChkOK.Checked == true)
                        {
                            strOK = "";
                        }
                        else
                        {
                            strOK = "OK";
                        }
                        //선택식등록 확인
                        SQL = "";
                        SQL = " SELECT TO_CHAR(MDate,'YYYY-MM-DD') MDate,Pano,Menu1,Menu2,Menu3,Menu4,Menu5,Remark,";
                        SQL = SQL + ComNum.VBLF + " TO_CHAR(EntDate,'HH24:MI') EntDate,ROWID  ";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_SORDER ";
                        SQL = SQL + ComNum.VBLF + "   WHERE Pano='" + RsReadIpd.Rows[i]["Pano"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND MDate=TO_DATE('" + (TxtDate.Text).Trim() + "','YYYY-MM-DD') ";

                        SqlErr = clsDB.GetDataTable(ref RsTemp, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (RsTemp.Rows.Count > 0)
                        {
                            nREAD2 = RsTemp.Rows.Count;

                            if (RsTemp.Rows[0]["Menu1"].ToString().Trim() == "Y")
                            {
                                strOK = "OK";
                            }
                            if (RsTemp.Rows[0]["Menu2"].ToString().Trim() == "Y")
                            {
                                strOK = "OK";
                            }
                            if (RsTemp.Rows[0]["Menu3"].ToString().Trim() == "Y")
                            {
                                strOK = "OK";
                            }
                            if (RsTemp.Rows[0]["Menu4"].ToString().Trim() != "")
                            {
                                strOK = "OK";
                            }
                            if (RsTemp.Rows[0]["Menu5"].ToString().Trim() == "Y")
                            {
                                strOK = "OK";
                            }
                        }

                        if (string.Compare((TxtDate.Text).Trim(), ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")) <= 0)
                        {
                            //밥,산모밥 선택
                            strOK1 = "";

                            SQL = "";
                            SQL = " SELECT DietDay ";
                            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "DIET_NEWORDER ";
                            SQL = SQL + ComNum.VBLF + "  WHERE ACTDATE = TO_DATE('" + (TxtDate.Text).Trim() + "' ,'YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "    AND PANO ='" + RsReadIpd.Rows[i]["Pano"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "     AND Bun IN ('02','03') ";//   '저염치료식, 일반치료식
                            SQL = SQL + ComNum.VBLF + "   Order BY DietDay ";

                            SqlErr = clsDB.GetDataTable(ref RsTemp3, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            if (RsTemp3.Rows.Count != 3)
                            {
                                for (j = 1; j <= 3; j++)
                                {
                                    SQL = "";
                                    SQL = " SELECT ROWID ";
                                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "DIET_NEWORDER ";
                                    SQL = SQL + ComNum.VBLF + "  WHERE ACTDATE = TO_DATE('" + (TxtDate.Text).Trim() + "' ,'YYYY-MM-DD') ";
                                    SQL = SQL + ComNum.VBLF + "    AND PANO ='" + RsReadIpd.Rows[i]["Pano"].ToString().Trim() + "' ";
                                    SQL = SQL + ComNum.VBLF + "    AND DietCode IN ('10','20') ";
                                    SQL = SQL + ComNum.VBLF + "    AND DietDay= '" + j.ToString() + "' ";
                                    SQL = SQL + ComNum.VBLF + "   ORDER BY Bun ";

                                    SqlErr = clsDB.GetDataTable(ref RsTemp2, SQL, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        return;
                                    }

                                    if (RsTemp2.Rows.Count > 0)
                                    {
                                        SQL = "";
                                        SQL = " SELECT ROWID ";
                                        SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.DIET_NEWORDER ";
                                        SQL = SQL + ComNum.VBLF + "  WHERE ACTDATE = TO_DATE('" + (TxtDate.Text).Trim() + "' ,'YYYY-MM-DD') ";
                                        SQL = SQL + ComNum.VBLF + "    AND PANO ='" + RsReadIpd.Rows[i]["Pano"].ToString().Trim() + "' ";
                                        SQL = SQL + ComNum.VBLF + "    AND Bun IN ('02','03') ";
                                        SQL = SQL + ComNum.VBLF + "    AND DietDay= '" + j.ToString() + "' ";
                                        SQL = SQL + ComNum.VBLF + "   ORDER BY Bun ";

                                        SqlErr = clsDB.GetDataTable(ref RsTemp4, SQL, clsDB.DbCon);

                                        if (SqlErr != "")
                                        {
                                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                            return;
                                        }

                                        if (RsTemp4.Rows.Count == 0)
                                        {
                                            strOK1 = "OK";
                                        }
                                        RsTemp4.Dispose();
                                        RsTemp4 = null;
                                    }
                                    RsTemp2.Dispose();
                                    RsTemp2 = null;
                                }
                            }
                            else
                            {
                                //3개면 무조건 제외
                                strOK1 = "";
                            }

                            if (RsTemp3.Rows.Count > 3)
                            {
                                strOK1 = "";
                                SQL = "";
                                SQL = " SELECT DietDay ";
                                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "DIET_NEWORDER ";
                                SQL = SQL + ComNum.VBLF + "  WHERE ACTDATE = TO_DATE('" + (TxtDate.Text).Trim() + "' ,'YYYY-MM-DD') ";
                                SQL = SQL + ComNum.VBLF + "    AND PANO ='" + RsReadIpd.Rows[i]["Pano"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + "     AND Bun IN ('02','03') ";//   '저염치료식, 일반치료식;
                                SQL = SQL + ComNum.VBLF + "   Group BY DietDay ";

                                SqlErr = clsDB.GetDataTable(ref RsTemp4, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    return;
                                }
                                if (RsTemp4.Rows.Count == 3)
                                {
                                    strOK1 = "";
                                    RsTemp4.Dispose();
                                    RsTemp4 = null;
                                }
                                else
                                {
                                    RsTemp4.Dispose();
                                    RsTemp4 = null;

                                    for (j = 1; j <= 3; j++)
                                    {
                                        SQL = "";
                                        SQL = " SELECT ROWID ";
                                        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "DIET_NEWORDER ";
                                        SQL = SQL + ComNum.VBLF + "  WHERE ACTDATE = TO_DATE('" + (TxtDate.Text).Trim() + "' ,'YYYY-MM-DD') ";
                                        SQL = SQL + ComNum.VBLF + "    AND PANO ='" + RsReadIpd.Rows[i]["Pano"].ToString().Trim() + "' ";
                                        SQL = SQL + ComNum.VBLF + "    AND DietCode IN ('10','20') ";
                                        SQL = SQL + ComNum.VBLF + "    AND DietDay= '" + j.ToString() + "' ";
                                        SQL = SQL + ComNum.VBLF + "   ORDER BY Bun ";

                                        SqlErr = clsDB.GetDataTable(ref RsTemp4, SQL, clsDB.DbCon);

                                        if (SqlErr != "")
                                        {
                                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                            return;
                                        }
                                        if (RsTemp4.Rows.Count > 0)
                                        {
                                            strOK1 = "OK";
                                        }
                                        RsTemp4.Dispose();
                                        RsTemp4 = null;
                                    }
                                }
                            }
                            RsTemp3.Dispose();
                            RsTemp3 = null;
                        }
                        else
                        {
                            strOK1 = "";
                            SQL = "";
                            SQL = " SELECT ROWID ";
                            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.DIET_NEWORDER ";
                            SQL = SQL + ComNum.VBLF + "  WHERE ACTDATE = TO_DATE('" + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-") + "' ,'YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "    AND PANO ='" + RsReadIpd.Rows[i]["Pano"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "    AND DietCode IN ('10','20','22') ";
                            SQL = SQL + ComNum.VBLF + "    AND DietDay= '3' ";
                            SQL = SQL + ComNum.VBLF + "   ORDER BY Bun ";

                            SqlErr = clsDB.GetDataTable(ref RsTemp2, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }
                            if (RsTemp2.Rows.Count > 0)
                            {
                                strOK1 = "OK";
                            }
                            RsTemp2.Dispose();
                            RsTemp2 = null;
                        }

                        //전체리스트
                        if (OptGu1.Checked == true)
                        {
                            strOK1 = "OK";
                        }

                        if (strOK == "OK" && strOK1 == "OK")
                        {
                            nRow = nRow + 1;
                            SS2_Sheet1.RowCount = nRow;

                            strNewData = RsReadIpd.Rows[i]["WardCode"].ToString().Trim();

                            if (strNewData != strOldData)
                            {
                                SS2_Sheet1.Cells[nRow - 1, 0].Text = strNewData;

                                if (nRow != 1)
                                {
                                    TxtSInwon.Text = TxtSInwon.Text + "【 " + strOldData + " 】" + " 아침 : " + nCnt[1] + " , 점심 : " + nCnt[2] + " ,저녁 : " + nCnt[3] + "【 합계】" + nCnt[1] + nCnt[2] + nCnt[3] + ComNum.VBLF;
                                }

                                for (j = 1; j <= 5; j++)
                                {
                                    nCnt[j] = 0;
                                }
                            }
                            SS2_Sheet1.Cells[nRow - 1, 1].Text = RsReadIpd.Rows[i]["RoomCode"].ToString().Trim();
                            SS2_Sheet1.Cells[nRow - 1, 2].Text = RsReadIpd.Rows[i]["Pano"].ToString().Trim();
                            SS2_Sheet1.Cells[nRow - 1, 3].Text = RsReadIpd.Rows[i]["SName"].ToString().Trim();
                            SS2_Sheet1.Cells[nRow - 1, 4].Text = RsReadIpd.Rows[i]["Age"].ToString().Trim() + "/" + RsReadIpd.Rows[i]["Sex"].ToString().Trim();

                            if (nREAD2 > 0)
                            {
                                if (RsTemp.Rows[0]["Menu1"].ToString().Trim() == "Y")
                                {
                                    SS2_Sheet1.Cells[nRow - 1, 5].Value = true;
                                    nCnt[1] = nCnt[1] + 1;
                                    nCnt[6] = nCnt[6] + 1;  //'아침선택
                                }

                                SS2_Sheet1.Cells[nRow - 1, 6].Text = RsTemp.Rows[0]["Menu4"].ToString().Trim();

                                if (RsTemp.Rows[0]["Menu4"].ToString().Trim() != "")
                                {
                                    nCnt[4] = nCnt[4] + 1;                                         //'기호식품(아침)
                                }

                                if (RsTemp.Rows[0]["Menu2"].ToString().Trim() == "Y")
                                {
                                    SS2_Sheet1.Cells[nRow - 1, 7].Value = true;
                                    nCnt[2] = nCnt[2] + 1;
                                    nCnt[7] = nCnt[7] + 1;//  '점심선택
                                }

                                if (RsTemp.Rows[0]["Menu3"].ToString().Trim() == "Y")
                                {
                                    SS2_Sheet1.Cells[nRow - 1, 8].Value = true;
                                    nCnt[3] = nCnt[3] + 1;
                                    nCnt[8] = nCnt[8] + 1;  //'저녁선택
                                }

                                if (RsTemp.Rows[0]["Menu5"].ToString().Trim() == "Y")
                                {
                                    SS2_Sheet1.Cells[nRow - 1, 9].Value = true;
                                    nCnt[5] = nCnt[5] + 1;                        //'잡곡밥
                                }

                                SS2_Sheet1.Cells[nRow - 1, 10].Text = RsTemp.Rows[0]["ReMark"].ToString().Trim();

                                if (strOK == "OK")
                                {
                                    SS2_Sheet1.Cells[nRow - 1, 11].Text = RsTemp.Rows[0]["EntDate"].ToString().Trim();
                                }
                                if (string.Compare(SS2_Sheet1.Cells[nRow - 1, 11].Text, "12:00") > 0)
                                {
                                    SS2_Sheet1.Cells[nRow - 1, 11].BackColor = Color.FromArgb(255, 0, 0);// RGB(255, 0, 0)
                                }
                            }

                            strOldData = RsReadIpd.Rows[i]["WardCode"].ToString().Trim();
                        }

                        RsTemp.Dispose();
                        RsTemp = null;

                    }

                    if (VB.Left(ComboWard.Text, 1) == "*")
                    {
                        TxtSInwon.Text = TxtSInwon.Text + "【 " + strNewData + " 】" + " 아침 : " + nCnt[1] + " , 점심 : " + nCnt[2] + " ,저녁 : " + nCnt[3] + "【 합계】" + nCnt[1] + nCnt[2] + nCnt[3] + ComNum.VBLF;
                    }

                    RsReadIpd.Dispose();
                    RsReadIpd = null;

                    if (VB.Left(ComboWard.Text, 1) != "*")
                    {
                        TxtCnt.Text = " 아침 : " + nCnt[1] + " , 점심 : " + nCnt[2] + " ,저녁 : " + nCnt[3] + " [기호식 : " + nCnt[4] + ", 잡곡밥 : " + nCnt[5] + "]";
                    }
                    else
                    {
                        TxtCnt.Text = " 아침 : " + nCnt[6] + " , 점심 : " + nCnt[7] + " ,저녁 : " + nCnt[8];
                    }


                    if (VB.Left(ComboWard.Text, 1) == "*")
                    {
                        panInwon.Visible = true;
                    }
                }


                RsReadIpd.Dispose();
                RsReadIpd = null;

            }
            catch (Exception ex)
            {
                if (RsReadIpd != null)
                {
                    RsReadIpd.Dispose();
                    RsReadIpd = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void TxtSInwon_DoubleClick(object sender, EventArgs e)
        {
            panInwon.Visible = false;
        }

        private void ComboWard_SET()
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strWardCode = "";

            Cursor.Current = Cursors.WaitCursor;

            if (GstrWardCodes == "TOP" || clsType.User.Sabun == "04349" || clsType.User.Sabun == "20433" || clsType.User.Sabun == "20442" || clsType.User.Sabun == "20193" || clsType.User.Sabun == "04444")
            {
                ComboWard.Enabled = true;
                strWardCode = "";
            }
            else
            {
                if (GstrWardCodes == "HD")
                {
                    strWardCode = "HD";
                }
                else
                {
                    if (GstrWardCodes == "MICU" || GstrWardCodes == "SICU")
                    {
                        strWardCode = "IU";
                    }
                    else
                    {
                        strWardCode = GstrWardCode;
                    }

                }
            }

            //콤보 set
            ComboWard.Items.Clear();
            if (GstrWardCodes == "TOP" || clsType.User.Sabun == "04349" || clsType.User.Sabun == "20433" || clsType.User.Sabun == "20442" || clsType.User.Sabun == "20193" || clsType.User.Sabun == "04444")
            {
                ComboWard.Items.Add("*전체");
            }

            try
            {
                if (strWardCode == "HD")
                {
                    ComboWard.Items.Add(VB.Left("외래(HD)" + VB.Space(20), 20) + "HD");
                }
                else
                {
                    SQL = "";
                    SQL = " SELECT WARDCODE, WARDNAME FROM BAS_WARD ";
                    SQL = SQL + ComNum.VBLF + " Where WardCode NOT IN ('IQ','2W', '3B','3C','ND','NR') ";

                    if (strWardCode != "")
                    {
                        SQL = SQL + ComNum.VBLF + " AND WARDCODE = '" + strWardCode + "'";
                    }

                    SQL = SQL + ComNum.VBLF + " Order By WardNAME ";

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
                            if (dt.Rows[i]["WardCODE"].ToString().Trim() == "IU")
                            {
                                if (GstrWardCodes == "TOP" || clsType.User.Sabun == "04349" || clsType.User.Sabun == "20433" || clsType.User.Sabun == "20442" || clsType.User.Sabun == "20193" || clsType.User.Sabun == "04444")
                                {
                                    ComboWard.Items.Add(VB.Left("내과" + dt.Rows[i]["WARDNAME"].ToString().Trim() + VB.Space(20), 20) + "IU");
                                    ComboWard.Items.Add(VB.Left("외과" + dt.Rows[i]["WARDNAME"].ToString().Trim() + VB.Space(20), 20) + "IU");
                                }
                                else
                                {
                                    if (GstrWardCodes == "MICU")
                                    {
                                        ComboWard.Items.Add(VB.Left("내과" + dt.Rows[i]["WARDNAME"].ToString().Trim() + VB.Space(20), 20) + "IU");
                                    }
                                    else if (GstrWardCodes == "SICU")
                                    {
                                        ComboWard.Items.Add(VB.Left("외과" + dt.Rows[i]["WARDNAME"].ToString().Trim() + VB.Space(20), 20) + "IU");
                                    }
                                }
                            }
                            else
                            {
                                ComboWard.Items.Add(VB.Left(dt.Rows[i]["WARDNAME"].ToString().Trim() + VB.Space(20), 20) + dt.Rows[i]["WARDCODE"].ToString().Trim());
                            }
                        }
                    }

                }
                
                if (GstrWardCodes == "TOP" || clsType.User.Sabun == "04349" || clsType.User.Sabun == "20433" || clsType.User.Sabun == "20442" ||
                       clsType.User.Sabun == "20193" || clsType.User.Sabun == "04444")
                {
                    ComboWard.SelectedIndex = 1;
                }
                else
                {
                    ComboWard.SelectedIndex = 0;
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        private void panInwon_DoubleClick(object sender, EventArgs e)
        {
            panInwon.Visible = false;
        }

        private void SS2_Change(object sender, ChangeEventArgs e)
        {
            SS2_Sheet1.Cells[e.Row, 12].Text = "Y";
        }

        private void TxtDate_ValueChanged(object sender, EventArgs e)
        {
            Use_Possible();
            SMenu_Set_Title();
        }
    }
}
