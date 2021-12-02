using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

using ComLibB;

namespace ComPmpaLibB
{
    public partial class frmPmpaDrgCode : Form
    {
        private string strDrgCode = "";

        public frmPmpaDrgCode()
        {
            InitializeComponent();
        }

        private void frmPmpaDrgCode_Load(object sender, EventArgs e)
        {
            //본인부담율
            cboGbn.Items.Clear();
            cboGbn.Items.Add("1.본인부담 20%");
            cboGbn.Items.Add("2.본인부담 14%(차상위 만성질환 18세미만)");
            cboGbn.Items.Add("3.본인부담 10%(희귀난치성질환 6세미만포함)");
            cboGbn.Items.Add("4.본인부담  5%(중증질환자)");
            cboGbn.SelectedIndex = 0;

            //야간가산구분
            cboNgt.Items.Clear();
            cboNgt.Items.Add("0.무가산");
            cboNgt.Items.Add("1.야간공휴가산");
            cboNgt.Items.Add("2.심야");
            cboNgt.SelectedIndex = 0;

            //절사구분
            cboTrunc.Items.Clear();
            cboTrunc.Items.Add("1.절사");
            cboTrunc.Items.Add("2.무절사");
            cboTrunc.SelectedIndex = 0;

            //산모가산구분
            cboOgAdd.Items.Clear();
            cboOgAdd.Items.Add("0.무가산");
            cboOgAdd.Items.Add("1.부인과 30%가산");
            cboOgAdd.Items.Add("2.재왕절개 50%가산");
            cboOgAdd.Items.Add("3.재왕절개 취약지 50%가산");
            cboOgAdd.SelectedIndex = 0;

            Screen_Clear(); 

            Read_Drg_Code_New();
        }

        private void Read_Drg_Code_New()
        {
            int i = 0;
            int intCnt = 0;
            //int intRow = 0;

            DataTable Dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                ComFunc.SetAllControlClear(SS1, true);

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT DCODE                                ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "DRG_CODE_NEW   ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                                ";
                SQL = SQL + ComNum.VBLF + "  GROUP BY DCODE                             ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY DCODE                             ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                intCnt = Dt.Rows.Count;

                for (i = 0; i < intCnt; i++)
                {
                    if (SS1_Sheet1.RowCount < i + 1)
                    {
                        SS1_Sheet1.RowCount = i + 1;
                    }
                    SS1_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["DCODE"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, clsDB.DbCon);
            }
        }

        private void Read_Drg_Map_Suga()
        {
            ComFunc.SetAllControlClear(SS5, true);
            ComFunc.SetAllControlClear(SS6, true);

            DataTable Dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int intCnt = 0;
            int i = 0;

            try
            {
                //관련수가
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT A.SUNEXT, B.SUNAMEK, A.ROWID         ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "DRG_MAP_SUGA A,";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN B      ";
                SQL = SQL + ComNum.VBLF + "  WHERE A.DCODE  = '" + strDrgCode + "'      ";
                SQL = SQL + ComNum.VBLF + "    AND A.SUNEXT = B.SUNEXT(+)               ";
                SQL = SQL + ComNum.VBLF + "    AND A.GBN ='A'                           ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY A.SUNEXT                          ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                intCnt = Dt.Rows.Count;

                for (i = 0; i < intCnt; i++)
                {
                    if(i + 1 > SS5_Sheet1.Rows.Count)
                    {
                        SS5_Sheet1.Rows.Count = SS5_Sheet1.Rows.Count + 10;
                    }

                    SS5_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["SuNext"].ToString().Trim();
                    SS5_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["SuNameK"].ToString().Trim();
                    SS5_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
                }
                
                Dt.Dispose();
                Dt = null;


                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT A.SUNEXT, B.SUNAMEK, A.ROWID         ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "DRG_MAP_SUGA A,";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN B      ";
                SQL = SQL + ComNum.VBLF + "  WHERE A.DCODE  = '" + strDrgCode + "'      ";
                SQL = SQL + ComNum.VBLF + "    AND A.SUNEXT = B.SUNEXT(+)               ";
                SQL = SQL + ComNum.VBLF + "    AND A.GBN ='B'                           ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY A.SUNEXT                          ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                intCnt = Dt.Rows.Count;

                for (i = 0; i < intCnt; i++)
                {
                    if (i + 1 > SS6_Sheet1.Rows.Count)
                    {
                        SS6_Sheet1.Rows.Count = SS6_Sheet1.Rows.Count + 10;
                    }

                    SS6_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["SuNext"].ToString().Trim();
                    SS6_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["SuNameK"].ToString().Trim();
                    SS6_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, clsDB.DbCon);
            }
        }

        private void Screen_Clear()
        {
            Spread_Clear_SS2();
            Spread_Clear_SS3();
            txtDrgCode.Text = "";
            txtDrgName.Text = "";
        }

        private void Spread_Clear_SS2()
        {
            int i = 0;
            int j = 0;

            for(i = 1; i < 24; i++)
            {
                if( i % 4 != 0)
                {
                    for (j = 1; j < 6; j++)
                    {
                        SS2_Sheet1.Cells[i, j].Text = "";
                    }
                }
            }
        }
        private void Spread_Clear_SS3()
        {
            ComFunc.SetAllControlClear(SS3, true);
        }

        private void 새로고침RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Read_Drg_Code_New();
        }

        private void 종료XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SS1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if(e.Column < 0 || e.Row < 0)
            {
                return;
            }

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int intCnt = 0;
            int i = 0;

            Screen_Clear();

            strDrgCode = SS1_Sheet1.Cells[e.Row, e.Column].Text;

            if (strDrgCode == "") { return; }

            txtDrgCode.Text = strDrgCode;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(DDATE,'YYYY-MM-DD') DDATE, DCODE, DNAME, DJUMSUM,    ";
                SQL = SQL + ComNum.VBLF + "        DGOBI, DILSU_AV, DILSU_MIN,                                  ";
                SQL = SQL + ComNum.VBLF + "        DILSU_MAX, DJUMDANGA,DHJUMSUM, DAGE_MIN, DAGE_MAX, DOJUMSUM  ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "DRG_CODE_NEW                           ";
                SQL = SQL + ComNum.VBLF + "  WHERE DCODE = '" + strDrgCode + "'                                 ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY DDATE DESC                                                ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                intCnt = Dt.Rows.Count;

                if( intCnt == 0)
                {
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Dt.Dispose();
                    Dt = null;
                    return;
                }

                txtDrgName.Text = Dt.Rows[0]["DNAME"].ToString().Trim();

                SS3_Sheet1.Rows.Count = intCnt;

                for(i=0; i < intCnt; i++)
                {
                    SS3_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["DDATE"].ToString().Trim();
                    SS3_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["DJUMSUM"].ToString().Trim();
                    SS3_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["DGOBI"].ToString().Trim();
                    SS3_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["DILSU_AV"].ToString().Trim();
                    SS3_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["DILSU_MIN"].ToString().Trim();
                    SS3_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["DILSU_MAX"].ToString().Trim();
                    SS3_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["DJUMDANGA"].ToString().Trim();
                    SS3_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["DHJUMSUM"].ToString().Trim();
                    SS3_Sheet1.Cells[i, 8].Text = Dt.Rows[i]["DAGE_MIN"].ToString().Trim();
                    SS3_Sheet1.Cells[i, 9].Text = Dt.Rows[i]["DAGE_MAX"].ToString().Trim();
                    SS3_Sheet1.Cells[i, 10].Text = Dt.Rows[i]["DOJUMSUM"].ToString().Trim();
                }
               
                Dt.Dispose();
                Dt = null;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }
            
            Read_Drg_Map_Suga();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int i = 0;

            string strSuNext = "";
            string strRowid = "";

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
                        
             
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < SS5_Sheet1.Rows.Count; i++)
                {
                    strSuNext = SS5_Sheet1.Cells[i, 0].Text;
                    strRowid = SS5_Sheet1.Cells[i, 2].Text;

                    if (strRowid != "")
                    {
                        if (strSuNext == "")    //삭제
                        {
                            SQL = " DELETE " + ComNum.DB_PMPA + "DRG_MAP_SUGA WHERE ROWID = '" + strRowid + "' ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }
                        }
                        else
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "DRG_MAP_SUGA SET   ";
                            SQL = SQL + ComNum.VBLF + "      SUNEXT = '" + strSuNext + "'               ";
                            SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strRowid + "'                ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (strSuNext != "")    //등록
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + " INSERT INTO KOSMOS_PMPA.DRG_MAP_SUGA                        ";
                            SQL = SQL + ComNum.VBLF + " (DCODE, SUNEXT ,ENTDATE,GBN ) VALUES (                      ";
                            SQL = SQL + ComNum.VBLF + "  '" + strDrgCode + "', '" + strSuNext + "', SYSDATE ,'A' )  ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }
                        }
                    }

                }

                for (i = 0; i < SS6_Sheet1.Rows.Count; i++)
                {

                    strSuNext = SS6_Sheet1.Cells[i, 0].Text;
                    strRowid = SS6_Sheet1.Cells[i, 2].Text;

                    if (strRowid != "")
                    {
                        if (strSuNext == "")  //삭제
                        {
                            SQL = " DELETE KOSMOS_PMPA.DRG_MAP_SUGA WHERE ROWID = '" + strRowid + "' ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }
                        }
                        else //갱신
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + " UPDATE KOSMOS_PMPA.DRG_MAP_SUGA SET ";
                            SQL = SQL + ComNum.VBLF + " SUNEXT = '" + strSuNext + "'        ";
                            SQL = SQL + ComNum.VBLF + "  WHERE ROWID = '" + strRowid + "'   ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (strSuNext != "") //등록
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + " INSERT INTO KOSMOS_PMPA.DRG_MAP_SUGA                        ";
                            SQL = SQL + ComNum.VBLF + " (DCODE, SUNEXT ,ENTDATE, GBN) VALUES (                      ";
                            SQL = SQL + ComNum.VBLF + " '" + strDrgCode + "', '" + strSuNext + "', SYSDATE, 'B' )   ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("DRG MAP SUGA 저장완료.");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                clsDB.setRollbackTran(clsDB.DbCon);
            }

            Read_Drg_Map_Suga();

        }

        private void SS3_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strSDate = "";

            int i = 0;
            int j = 0;
            int nRow = 0;
            int nCol = 0;
            int nRowB = 0;

            if (e.Row < 0 || e.Column < 0)
            {
                return;
            }

            Spread_Clear_SS2();

            strSDate = SS3_Sheet1.Cells[e.Row, 0].Text;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT DCODE,DDATE, GBBON,                                          ";
                SQL = SQL + ComNum.VBLF + "        DAY1,DAY2,DAY3,DAY4,DAY5,DAY6,DAY7,DAY8,DAY9,DAY10,          ";
                SQL = SQL + ComNum.VBLF + "        DAY11,DAY12,DAY13,DAY14,DAY15,DAY16,DAY17,DAY18,DAY19,DAY20, ";
                SQL = SQL + ComNum.VBLF + "        DAY21,DAY22,DAY23,DAY24,DAY25,DAY26,DAY27,DAY28,DAY29,DAY30  ";
                SQL = SQL + ComNum.VBLF + "   From " + ComNum.DB_PMPA + "DRG_DAY_COST_NEW                       ";
                SQL = SQL + ComNum.VBLF + "  WHERE DCODE = '" + strDrgCode + "'                                 ";
                SQL = SQL + ComNum.VBLF + "    AND DDATE = TO_DATE('" + strSDate + "','YYYY-MM-DD')             ";
                SQL = SQL + ComNum.VBLF + "    AND GBN = '" + VB.Left(cboGbn.Text, 1) + "'                      ";
                SQL = SQL + ComNum.VBLF + "    AND GBNGT = '" + VB.Left(cboNgt.Text, 1) + "'                    ";
                SQL = SQL + ComNum.VBLF + "    AND GBNTRUNC = '" + VB.Left(cboTrunc.Text, 1) + "'               ";
                SQL = SQL + ComNum.VBLF + "    AND GBOGADD = '" + VB.Left(cboOgAdd.Text, 1) + "'                ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY DCODE, DDATE                                              ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (Dt.Rows.Count == 0)
                {
                    ComFunc.MsgBox("해당하는 DATA가 없습니다'");
                    return;
                }

                for (i = 0; i < Dt.Rows.Count; i++)
                {
                    nCol = 1;

                    switch (Dt.Rows[i]["GbBon"].ToString().Trim())
                    {
                        case "1":
                            nRowB = 2;  //총액
                            break;
                        case "2":
                            nRowB = 0;  //보험자
                            break;
                        case "3":
                            nRowB = 1;  //본인
                            break;
                    }

                    for (j = 1; j <= 30; j++)
                    {
                        if (j >= 1 && j <= 5)
                        { nRow = 1; }
                        else if (j >= 6 && j <= 10)
                        { nRow = 5; }
                        else if (j >= 11 && j <= 15)
                        { nRow = 9; }
                        else if (j >= 16 && j <= 20)
                        { nRow = 13; }
                        else if (j >= 21 && j <= 25)
                        { nRow = 17; }
                        else if (j >= 26 && j <= 30)
                        { nRow = 21; }

                        if (j % 5 == 0)
                        {
                            nCol = 5;
                        }
                        else
                        {
                            nCol = j % 5;
                        }
                        SS2_Sheet1.Cells[nRow + nRowB, nCol].Text = VB.Format( VB.Val( Dt.Rows[i]["Day" + j.ToString()].ToString()), "#,###.###0");
                    }
                }

                Dt.Dispose();
                Dt = null;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }
       
        }

        private void SS5_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strSuNext = "";

            if(e.Column != 0){ return; }

            strSuNext = SS5_Sheet1.Cells[e.Row, 0].Text.Trim();
            
            if(strSuNext == "") { return; }

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT SUNAMEK FROM " + ComNum.DB_PMPA + "BAS_SUN   ";
                SQL = SQL + ComNum.VBLF + "  WHERE SUNEXT = '" + strSuNext + "'                 ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (Dt.Rows.Count > 0)
                {
                    SS5_Sheet1.Cells[e.Row, 1].Text = Dt.Rows[0]["SuNameK"].ToString().Trim();
                }
                else
                {
                    SS5_Sheet1.Cells[e.Row, 0].Text = "";
                    SS5_Sheet1.Cells[e.Row, 1].Text = "";
                }

                Dt.Dispose();
                Dt = null;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }
        }

        private void SS6_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strSuNext = "";

            if (e.Column != 0) { return; }

            strSuNext = SS6_Sheet1.Cells[e.Row, 0].Text.Trim();

            if (strSuNext == "") { return; }

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT SUNAMEK FROM " + ComNum.DB_PMPA + "BAS_SUN   ";
                SQL = SQL + ComNum.VBLF + "  WHERE SUNEXT = '" + strSuNext + "'                 ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (Dt.Rows.Count > 0)
                {
                    SS6_Sheet1.Cells[e.Row, 1].Text = Dt.Rows[0]["SuNameK"].ToString().Trim();
                }
                else
                {
                    SS6_Sheet1.Cells[e.Row, 0].Text = "";
                    SS6_Sheet1.Cells[e.Row, 1].Text = "";
                }

                Dt.Dispose();
                Dt = null;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }
        }

        private void txtDrgCode_DoubleClick(object sender, EventArgs e)
        {
            clsPublic.GstrHelpCode = txtDrgCode.Text;
            this.Close();
        }
    }
}
