using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// /// <summary>
    /// Class Name : frmDrgBaseCode
    /// File Name : frmDrgBaseCode.cs
    /// Title or Description : DRG기초코드관리
    /// Author : 유진호
    /// Create Date : 2017-11-02
    /// Update Histroy :     
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso>
    /// VB\basic\busuga\DRGMAIN01.frm(FrmBaseCode)
    /// </seealso>    
    /// </summary>
    public partial class frmDrgBaseCode : Form
    {
        private string FstrDRGCode;
        private string GstrHelpCode;

        public delegate void SetHelpCode(string strHelpCode);
        public event SetHelpCode rSetHelpCode;

        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        public frmDrgBaseCode()
        {
            InitializeComponent();
        }

        private void frmDrgBaseCode_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            READ_DRG_CODE_NEW();

            cboGBN.Items.Clear();
            cboGBN.Items.Add("1.본인부담 20%");
            cboGBN.Items.Add("2.본인부담 14%(차상위 만성질환 18세미만)");
            cboGBN.Items.Add("3.본인부담 10%(희귀난치성질환(6세미만포함)");
            cboGBN.Items.Add("4.본인부담  5%(중증질환자)");
            cboGBN.Items.Add("5.본인부담  3%(차장위 만성질활 15세이하)");
            cboGBN.SelectedIndex = 0;

            cboNgt.Items.Clear();
            cboNgt.Items.Add("0.무가산");
            cboNgt.Items.Add("1.야간공휴가산");
            cboNgt.Items.Add("2.심야");
            cboNgt.SelectedIndex = 0;

            cboTrunc.Items.Clear();
            cboTrunc.Items.Add("1.절사");
            cboTrunc.Items.Add("2.무절사");
            cboTrunc.SelectedIndex = 0;

            cboGADD.Items.Clear();
            cboGADD.Items.Add("0.무가산");
            cboGADD.Items.Add("1.부인과 30%가산");
            cboGADD.Items.Add("2.재왕절개 50%가산");
            cboGADD.Items.Add("3.재왕절개 취약지 50%가산");
            cboGADD.SelectedIndex = 0;
        }

        private void SCREEN_CLEAR()
        {
            SS3_CLEAR();
            SS2_CLEAR();
            txtDrgCode.Text = "";
            txtDrgName.Text = "";
        }

        private void SS2_CLEAR()
        {
            int i = 0;
            for (i = 0; i <= 5; i++)
            {
                ss2_Sheet1.Cells[(i * 5) + 1, 1, (i * 5) + 3, ss2_Sheet1.ColumnCount - 1].Text = "";
            }
        }

        private void SS3_CLEAR()
        {
            ss3_Sheet1.Cells[0, 0, ss3_Sheet1.RowCount - 1, ss3_Sheet1.ColumnCount - 1].Text = "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strSuNext = "";
            string strROWID = "";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; ; //권한 확인

                for (i = 0; i < ss5_Sheet1.NonEmptyRowCount; i++)
                {
                    strSuNext = VB.Trim(ss5_Sheet1.Cells[i, 0].Text.ToUpper());
                    strROWID = VB.Trim(ss5_Sheet1.Cells[i, 2].Text);

                    if (strROWID != "")
                    {
                        if (strSuNext == "")    //'삭제
                        {
                            SQL = " DELETE ADMIN.DRG_MAP_SUGA WHERE ROWID = '" + strROWID + "' ";
                        }
                        else    //'갱신
                        {
                            SQL = " UPDATE ADMIN.DRG_MAP_SUGA SET ";
                            SQL = SQL + "  SUNEXT = '" + strSuNext + "'";
                            SQL = SQL + " WHERE ROWID = '" + strROWID + "' ";
                        }
                    }
                    else
                    {
                        if (strSuNext != "")    //'등록
                        {
                            SQL = " INSERT INTO ADMIN.DRG_MAP_SUGA ( DCODE, SUNEXT ,ENTDATE,GBN ) VALUES ( ";
                            SQL = SQL + "  '" + FstrDRGCode + "', '" + strSuNext + "', SYSDATE ,'A' ) ";
                        }
                    }

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }


                for (i = 0; i < ss6_Sheet1.NonEmptyRowCount; i++)
                {
                    strSuNext = VB.Trim(ss6_Sheet1.Cells[i, 0].Text.ToUpper());
                    strROWID = VB.Trim(ss6_Sheet1.Cells[i, 2].Text);

                    if (strROWID != "")
                    {
                        if (strSuNext == "")    //'삭제
                        {
                            SQL = " DELETE ADMIN.DRG_MAP_SUGA WHERE ROWID = '" + strROWID + "' ";
                        }
                        else    //'갱신
                        {
                            SQL = " UPDATE ADMIN.DRG_MAP_SUGA SET ";
                            SQL = SQL + "  SUNEXT = '" + strSuNext + "'";
                            SQL = SQL + " WHERE ROWID = '" + strROWID + "' ";
                        }
                    }
                    else
                    {
                        if (strSuNext != "")    //'등록
                        {
                            SQL = " INSERT INTO ADMIN.DRG_MAP_SUGA ( DCODE, SUNEXT ,ENTDATE,GBN ) VALUES ( ";
                            SQL = SQL + "  '" + FstrDRGCode + "', '" + strSuNext + "', SYSDATE ,'B' ) ";
                        }
                    }

                     SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;

                READ_DRG_MAP_SUGA();

                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }


        private void READ_DRG_MAP_SUGA()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SQL = "";
                SQL = SQL + ComNum.VBLF + "";

                SQL = " SELECT A.SUNEXT ,   B.SUNAMEK , A.ROWID  ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.DRG_MAP_SUGA A ";
                SQL = SQL + ComNum.VBLF + "  LEFT OUTER JOIN ADMIN.BAS_SUN B";
                SQL = SQL + ComNum.VBLF + "      ON A.SUNEXT = B.SUNEXT ";
                SQL = SQL + ComNum.VBLF + "  WHERE A.DCODE  = '" + FstrDRGCode + "' ";
                SQL = SQL + ComNum.VBLF + "     AND A.GBN ='A' ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY A.SUNEXT ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                ss5_Sheet1.RowCount = 0;
                ss5_Sheet1.RowCount = 100;

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i + 1 > ss5_Sheet1.RowCount) ss5_Sheet1.RowCount = ss5_Sheet1.RowCount + 10;

                        ss5_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                        ss5_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SuNameK"].ToString().Trim();
                        ss5_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = " SELECT A.SUNEXT ,   B.SUNAMEK , A.ROWID  ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.DRG_MAP_SUGA A ";
                SQL = SQL + ComNum.VBLF + "  LEFT OUTER JOIN ADMIN.BAS_SUN B";
                SQL = SQL + ComNum.VBLF + "      ON A.SUNEXT = B.SUNEXT ";
                SQL = SQL + ComNum.VBLF + "  WHERE A.DCODE  = '" + FstrDRGCode + "' ";
                SQL = SQL + ComNum.VBLF + "     AND A.GBN ='B' ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY A.SUNEXT ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                ss6_Sheet1.RowCount = 0;
                ss6_Sheet1.RowCount = 100;

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i + 1 > ss6_Sheet1.RowCount) ss6_Sheet1.RowCount = ss6_Sheet1.RowCount + 10;

                        ss6_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                        ss6_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SuNameK"].ToString().Trim();
                        ss6_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ROWID"].ToString().Trim();
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

        private void READ_DRG_CODE_NEW()
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SQL = "";
                SQL = " SELECT DCODE FROM DRG_CODE_NEW";
                SQL = SQL + ComNum.VBLF + " GROUP BY  DCODE ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY DCODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ss1_Sheet1.RowCount = dt.Rows.Count;
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["DCODE"].ToString().Trim();
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            rEventClosed();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            READ_DRG_CODE_NEW();
        }

        private void ss1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SCREEN_CLEAR();

                FstrDRGCode = ss1_Sheet1.Cells[e.Row, 0].Text;

                if (FstrDRGCode == "") return;

                txtDrgCode.Text = FstrDRGCode;


                SQL = "";
                SQL = "SELECT DDATE, DCODE, DNAME,  DJUMSUM, DGOBI, DILSU_AV, DILSU_MIN, DILSU_MAX, DJUMDANGA, ";
                SQL = SQL + ComNum.VBLF + " DHJUMSUM, DAGE_MIN, DAGE_MAX, DOJUMSUM ";
                SQL = SQL + ComNum.VBLF + " FROM DRG_CODE_NEW";
                SQL = SQL + ComNum.VBLF + " WHERE DCODE = '" + FstrDRGCode + "' ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY DDATE DESC";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    txtDrgName.Text = dt.Rows[0]["DNAME"].ToString().Trim();
                    ss3_Sheet1.RowCount = dt.Rows.Count;
                    ss3_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (VB.IsDate(dt.Rows[i]["DDATE"].ToString().Trim()) == true)
                        {
                            ss3_Sheet1.Cells[i, 0].Text = Convert.ToDateTime(dt.Rows[i]["DDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                        }                        
                        ss3_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DJUMSUM"].ToString().Trim();
                        ss3_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DGOBI"].ToString().Trim();
                        ss3_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DILSU_AV"].ToString().Trim();
                        ss3_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DILSU_MIN"].ToString().Trim();
                        ss3_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DILSU_MAX"].ToString().Trim();
                        ss3_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DJUMDANGA"].ToString().Trim();
                        ss3_Sheet1.Cells[i, 7].Text = dt.Rows[i]["DHJUMSUM"].ToString().Trim();
                        ss3_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DAGE_MIN"].ToString().Trim();
                        ss3_Sheet1.Cells[i, 9].Text = dt.Rows[i]["DAGE_MAX"].ToString().Trim();
                        ss3_Sheet1.Cells[i, 10].Text = dt.Rows[i]["DOJUMSUM"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                READ_DRG_MAP_SUGA();
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

        private void ss3_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

            int i = 0;
            int j = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            int[,] NDATA = new int[3, 50];
            string strSDate;

            int nTday = 0;
            int nBday = 0;
            int nRowB = 0;

            int nRow = 0;
            int nCol = 0;

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                if (e.Column < 0 || e.Row < 0) return;

                SS2_CLEAR();
                strSDate = ss3_Sheet1.Cells[e.Row, 0].Text;
                nTday = Convert.ToInt32(VB.Val(ss3_Sheet1.Cells[e.Row, 2].Text));
                nBday = Convert.ToInt32(VB.Val(ss3_Sheet1.Cells[e.Row, 3].Text));

                //for (i = 0; i < 50; i++)
                //{
                //    NDATA[1, i] = 0;
                //    NDATA[2, i] = 0;
                //    NDATA[3, i] = 0;
                //}
                
                SQL = " SELECT DCODE,DDATE, GBBON, ";
                SQL = SQL + " DAY1,DAY2,DAY3,DAY4,DAY5,DAY6,DAY7,DAY8,DAY9,DAY10,";
                SQL = SQL + " DAY11,DAY12,DAY13,DAY14,DAY15,DAY16,DAY17,DAY18,DAY19,DAY20,";
                SQL = SQL + " DAY21,DAY22,DAY23,DAY24,DAY25,DAY26,DAY27,DAY28,DAY29,DAY30";
                SQL = SQL + "   FROM ADMIN.DRG_DAY_COST_NEW ";
                SQL = SQL + "  WHERE DCODE = '" + FstrDRGCode + "'";
                SQL = SQL + "    AND DDATE = TO_DATE('" + strSDate + "','YYYY-MM-DD')";
                SQL = SQL + "    AND GBN = '" + VB.Left(cboGBN.Text, 1) + "' ";
                SQL = SQL + "    AND GBNGT = '" + VB.Left(cboNgt.Text, 1) + "' ";
                SQL = SQL + "    AND GBNTRUNC = '" + VB.Left(cboTrunc.Text, 1) + "' ";
                SQL = SQL + "    AND GBOGADD = '" + VB.Left(cboGADD.Text, 1) + "' ";
                SQL = SQL + "   ORDER BY DCODE, DDATE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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
                        nCol = 0;

                        switch (dt.Rows[i]["GbBon"].ToString().Trim())
                        {
                            case "1":
                                nRowB = 2;    //'총액
                                break;
                            case "2":
                                nRowB = 0;    //'보험자
                                break;
                            case "3":
                                nRowB = 1;    //'본인
                                break;
                        }

                        for (j = 1; j <= 30; j++)
                        {
                            //select case문을 if문으로 변경함.
                            if (j >= 1 && j <= 5) nRow = 1;
                            else if (j >= 6 && j <= 10) nRow = 6;
                            else if (j >= 11 && j <= 15) nRow = 11;
                            else if (j >= 16 && j <= 20) nRow = 16;
                            else if (j >= 21 && j <= 25) nRow = 21;
                            else if (j >= 26 && j <= 30) nRow = 26;

                            if (j % 5 == 0)
                            {
                                nCol = 5;
                            }
                            else
                            {
                                nCol = (j % 5);
                            }


                            ss2_Sheet1.Cells[nRow + nRowB, nCol].Text = VB.Format(VB.Val(dt.Rows[i]["Day" + VB.Trim(j.ToString())].ToString().Trim()), "###,###,###,###.####");
                        }                        
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

        private void ss5_EditModeOff(object sender, EventArgs e)
        {
            if (ss5_Sheet1.ActiveColumnIndex == -1) return;

            GetSuNameK(ss5_Sheet1.Cells[ss5_Sheet1.ActiveRowIndex, 0].Text.Trim().ToUpper(), ss5);
        }

        private void ss6_EditModeOff(object sender, EventArgs e)
        {
            if (ss6_Sheet1.ActiveColumnIndex == -1) return;

            GetSuNameK(ss6_Sheet1.Cells[ss6_Sheet1.ActiveRowIndex, 0].Text.Trim().ToUpper(), ss6);
        }

        string GetSuNameK(string strSuNext, FarPoint.Win.Spread.FpSpread spd)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return ""; //권한 확인

            if (string.IsNullOrEmpty(strSuNext)) return strSuNext;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string rtnVal = "";

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT SUNAMEK FROM ADMIN.BAS_SUN ";
                SQL = SQL + ComNum.VBLF + "   WHERE SUNEXT = '" + strSuNext + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    spd.ActiveSheet.Cells[spd.ActiveSheet.ActiveRowIndex, 1].Text = dt.Rows[0]["SuNameK"].ToString().Trim();
                }
                else
                {
                    spd.ActiveSheet.Cells[spd.ActiveSheet.ActiveRowIndex, 0].Text = "";
                    spd.ActiveSheet.Cells[spd.ActiveSheet.ActiveRowIndex, 1].Text = "";
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void txtDrgCode_DoubleClick(object sender, EventArgs e)
        {
            GstrHelpCode = txtDrgCode.Text;
            rSetHelpCode(GstrHelpCode);
            this.Close();
        }
    }
}
