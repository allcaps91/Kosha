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

namespace ComLibB
{
    public partial class frmSugaChange : Form
    {
        private frmSearchSugaSQL frmSearchSugaSQLX = null;

        string GstrHelpCode = "";

        public frmSugaChange()
        {
            InitializeComponent();
        }

        private void frmSugaChange_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ss1_Sheet1.Columns[26].Visible = true;
            ss1_Sheet1.Columns[27].Visible = true;
            ss1_Sheet1.Columns[28].Visible = true;
            ss1_Sheet1.Columns[29].Visible = true;

            ComFunc.ReadSysDate(clsDB.DbCon);
            dtpSudate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
            SCREEN_CLEAR();
        }

        private void SCREEN_CLEAR()
        {
            //Call SS_Clear(Me.ss1)
            grbJob.Enabled = true;
            btnView.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnExit.Enabled = true;
            btnPrint.Enabled = false;
            ss1.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
            btnView.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveData();
        }

        private bool saveData()
        {
            bool rtVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            int i = 0;
            string strChange = "";

            string strSuCode = "";
            string strSuNext = "";
            string strGbn = "";
            int nBAmt = 0;
            int nTAmt = 0;
            int nIAmt = 0;
            string strSuDate = "";
            int nOldBAmt = 0;
            int nOldTAmt = 0;
            int nOldIAmt = 0;
            string strSuDate3 = "";
            int nBAmt3 = 0;
            int nTAmt3 = 0;
            int nIAmt3 = 0;
            string strSuDate4 = "";
            int nBAmt4 = 0;
            int nTAmt4 = 0;
            int nIAmt4 = 0;
            string strSuDate5 = "";
            int nBAmt5 = 0;
            int nTAmt5 = 0;
            int nIAmt5 = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return rtVal; ; //권한 확인

                for (i = 0; i < ss1_Sheet1.RowCount; i++)
                {
                    strChange = ss1_Sheet1.Cells[i, 26].Text;
                    if (strChange != "Y") continue;

                    strSuCode = ss1_Sheet1.Cells[i, 0].Text;
                    strSuNext = ss1_Sheet1.Cells[i, 1].Text;
                    strGbn = ss1_Sheet1.Cells[i, 2].Text;
                    nBAmt = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[i, 4].Text));
                    nTAmt = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[i, 5].Text));
                    nIAmt = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[i, 6].Text));
                    strSuDate = ss1_Sheet1.Cells[i, 8].Text;
                    nOldBAmt = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[i, 9].Text));
                    nOldTAmt = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[i, 10].Text));
                    nOldIAmt = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[i, 11].Text));
                    strSuDate3 = ss1_Sheet1.Cells[i, 12].Text;
                    nBAmt3 = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[i, 13].Text));
                    nTAmt3 = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[i, 14].Text));
                    nIAmt3 = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[i, 15].Text));
                    strSuDate4 = ss1_Sheet1.Cells[i, 16].Text;
                    nBAmt4 = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[i, 17].Text));
                    nTAmt4 = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[i, 18].Text));
                    nIAmt4 = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[i, 19].Text));
                    strSuDate5 = ss1_Sheet1.Cells[i, 20].Text;
                    nBAmt5 = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[i, 21].Text));
                    nTAmt5 = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[i, 22].Text));
                    nIAmt5 = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[i, 23].Text));

                    if (strGbn == "T")
                    {
                        SQL = "UPDATE BAS_SUT SET BAMT=" + nBAmt + ",TAMT=" + nTAmt + ",IAMT=" + nIAmt + ",";
                        SQL = SQL + ComNum.VBLF + "SUDATE=TO_DATE('" + strSuDate + "','YYYY-MM-DD'),OLDBAMT=" + nOldBAmt + ",";
                        SQL = SQL + ComNum.VBLF + "OLDTAMT=" + nOldTAmt + ",OLDIAMT=" + nOldIAmt + ",";
                        SQL = SQL + ComNum.VBLF + "SUDATE3=TO_DATE('" + strSuDate3 + "','YYYY-MM-DD'),BAMT3=" + nBAmt3 + ",";
                        SQL = SQL + ComNum.VBLF + "TAMT3=" + nTAmt3 + ",IAMT3=" + nIAmt3 + ",";
                        SQL = SQL + ComNum.VBLF + "SUDATE4=TO_DATE('" + strSuDate4 + "','YYYY-MM-DD'),BAMT4=" + nBAmt4 + ",";
                        SQL = SQL + ComNum.VBLF + "TAMT4=" + nTAmt4 + ",IAMT4=" + nIAmt4 + ",";
                        SQL = SQL + ComNum.VBLF + "SUDATE5=TO_DATE('" + strSuDate5 + "','YYYY-MM-DD'),BAMT5=" + nBAmt5 + ",";
                        SQL = SQL + ComNum.VBLF + "TAMT5=" + nTAmt5 + ",IAMT5=" + nIAmt5 + " ";
                        SQL = SQL + ComNum.VBLF + "WHERE SUCODE = '" + strSuCode + "' ";
                    }
                    else
                    {
                        SQL = "UPDATE BAS_SUH SET BAMT=" + nBAmt + ",TAMT=" + nTAmt + ",IAMT=" + nIAmt + ",";
                        SQL = SQL + ComNum.VBLF + "SUDATE=TO_DATE('" + strSuDate + "','YYYY-MM-DD'),OLDBAMT=" + nOldBAmt + ",";
                        SQL = SQL + ComNum.VBLF + "OLDTAMT=" + nOldTAmt + ",OLDIAMT=" + nOldIAmt + ",";
                        SQL = SQL + ComNum.VBLF + "SUDATE3=TO_DATE('" + strSuDate3 + "','YYYY-MM-DD'),BAMT3=" + nBAmt3 + ",";
                        SQL = SQL + ComNum.VBLF + "TAMT3=" + nTAmt3 + ",IAMT3=" + nIAmt3 + ",";
                        SQL = SQL + ComNum.VBLF + "SUDATE4=TO_DATE('" + strSuDate4 + "','YYYY-MM-DD'),BAMT4=" + nBAmt4 + ",";
                        SQL = SQL + ComNum.VBLF + "TAMT4=" + nTAmt4 + ",IAMT4=" + nIAmt4 + ",";
                        SQL = SQL + ComNum.VBLF + "SUDATE5=TO_DATE('" + strSuDate5 + "','YYYY-MM-DD'),BAMT5=" + nBAmt5 + ",";
                        SQL = SQL + ComNum.VBLF + "TAMT5=" + nTAmt5 + ",IAMT5=" + nIAmt5 + " ";
                        SQL = SQL + ComNum.VBLF + "WHERE SUCODE = '" + strSuCode + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND SUNEXT = '" + strSuNext + "' ";
                    }

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("수가코드 등록중 오류가 발생함");
                        Cursor.Current = Cursors.Default;
                        return rtVal;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;

                SCREEN_CLEAR();
                btnView.Focus();

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

        private void btnPrint_Click(object sender, EventArgs e)
        {

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                return; //권한 확인

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";
            string SysDate = "";
            string SysTime = "";

            ss1_Sheet1.Columns[18].Visible = false;
            ss1_Sheet1.Columns[19].Visible = false;
            ss1_Sheet1.Columns[20].Visible = false;
            ss1_Sheet1.Columns[21].Visible = false;
            ss1_Sheet1.Columns[22].Visible = false;
            ss1_Sheet1.Columns[23].Visible = false;
            ss1_Sheet1.Columns[24].Visible = false;
            ss1_Sheet1.Columns[25].Visible = false;

            SysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            SysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            Cursor.Current = Cursors.WaitCursor;

            strFont1 = "/fn\"굴림체\" /fz\"16\" /fb0 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"10\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead1 = "/n/n/f1/C " + dtpSudate.Text + "일 적용 수가변경 내역" + "/n/n/n/n";
            strHead2 = "/l/f2" + "인쇄일자 : " + SysDate + " " + SysTime;

            ss1_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ss1_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ss1_Sheet1.PrintInfo.Margin.Left = 35;
            ss1_Sheet1.PrintInfo.Margin.Right = 0;
            ss1_Sheet1.PrintInfo.Margin.Top = 35;
            ss1_Sheet1.PrintInfo.Margin.Bottom = 30;
            ss1_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ss1_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ss1_Sheet1.PrintInfo.ShowBorder = true;
            ss1_Sheet1.PrintInfo.ShowColor = false;
            ss1_Sheet1.PrintInfo.ShowGrid = true;
            ss1_Sheet1.PrintInfo.ShowShadows = false;
            ss1_Sheet1.PrintInfo.UseMax = false;
            ss1_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ss1.PrintSheet(0);


            Application.DoEvents();
            Application.DoEvents();

            ss1_Sheet1.Columns[18].Visible = true;
            ss1_Sheet1.Columns[19].Visible = true;
            ss1_Sheet1.Columns[20].Visible = true;
            ss1_Sheet1.Columns[21].Visible = true;
            ss1_Sheet1.Columns[22].Visible = true;
            ss1_Sheet1.Columns[23].Visible = true;
            ss1_Sheet1.Columns[24].Visible = true;
            ss1_Sheet1.Columns[25].Visible = true;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                ss1_Sheet1.RowCount = 0;

                if (optJob_2.Checked == false)
                {
                    btnPrint.Enabled = false;

                    frmSearchSugaSQLX = new frmSearchSugaSQL();
                    frmSearchSugaSQLX.rSetSuGaCodeSQL += frmSearchSugaSQLX_rSetSuGaCodeSQL;
                    frmSearchSugaSQLX.rEventClosed += frmSearchSugaSQLX_rEventClosed;
                    frmSearchSugaSQLX.ShowDialog();

                    SQL = GstrHelpCode;
                    if (SQL == "") return;
                }
                else
                {
                    SQL = "     SELECT NU,BUN,SUCODE,GBN,SUNEXT,SUGBSS,SUGBBI,SUQTY,SUGBA,SUGBB,SUGBC,SUGBD,SUGBE,";
                    SQL = SQL + ComNum.VBLF + "      SUGBF,SUGBG,SUGBH,SUGBI,SUGBJ,SUGBK,SUGBL,SUGBM,SUGBN,SUGBO,";
                    SQL = SQL + ComNum.VBLF + "      SUGBN,DAYMAX,TOTMAX,IAMT,TAMT,BAMT,";
                    SQL = SQL + ComNum.VBLF + "      TO_CHAR(SUDATE,'YYYY-MM-DD')  SUDATE,OLDIAMT,OLDTAMT,OLDBAMT,";
                    SQL = SQL + ComNum.VBLF + "      TO_CHAR(SUDATE3,'YYYY-MM-DD') SUDATE3,IAMT3,TAMT3,BAMT3,";
                    SQL = SQL + ComNum.VBLF + "      TO_CHAR(SUDATE4,'YYYY-MM-DD') SUDATE4,IAMT4,TAMT4,BAMT4,";
                    SQL = SQL + ComNum.VBLF + "      TO_CHAR(SUDATE5,'YYYY-MM-DD') SUDATE5,IAMT5,TAMT5,BAMT5,";
                    SQL = SQL + ComNum.VBLF + "      SUNAMEK,SUNAMEE,SUNAMEG,UNIT,DAICODE,HCODE,BCODE,";
                    SQL = SQL + ComNum.VBLF + "      SUHAM,EDIJONG,TO_CHAR(EDIDATE,'YYYY-MM-DD') EDIDATE,";
                    SQL = SQL + ComNum.VBLF + "      OLDBCODE,OLDGESU,OLDJONG,WONCODE,WONAMT ";
                    SQL = SQL + ComNum.VBLF + " FROM VIEW_SUGA_CODE ";
                    SQL = SQL + ComNum.VBLF + "WHERE BUN >= '01' AND BUN <= '84' ";
                    SQL = SQL + ComNum.VBLF + "  AND SUDATE = TO_DATE('" + dtpSudate.Text + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND DELDATE IS NULL ";
                    //'SORT순서(누적,분류,수가코드,품명코드순);
                    SQL = SQL + ComNum.VBLF + "ORDER BY NU,BUN,SUCODE,GBN DESC,SUNEXT ";
                }

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                }

                if (dt.Rows.Count > 0)
                {
                    ss1.Enabled = true;

                    ss1_Sheet1.RowCount = dt.Rows.Count;
                    ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SuCode"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Gbn"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Bun"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["BAmt"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["TAmt"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["IAmt"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["SuNameK"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["SuDate"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 9].Text = dt.Rows[i]["OldBAmt"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 10].Text = dt.Rows[i]["OldTAmt"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 11].Text = dt.Rows[i]["OldIAmt"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 12].Text = dt.Rows[i]["SuDate3"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 13].Text = dt.Rows[i]["BAmt3"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 14].Text = dt.Rows[i]["TAmt3"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 15].Text = dt.Rows[i]["IAmt3"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 16].Text = dt.Rows[i]["SuDate4"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 17].Text = dt.Rows[i]["BAmt4"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 18].Text = dt.Rows[i]["TAmt4"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 19].Text = dt.Rows[i]["IAmt4"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 20].Text = dt.Rows[i]["SuDate5"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 21].Text = dt.Rows[i]["BAmt5"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 22].Text = dt.Rows[i]["TAmt5"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 23].Text = dt.Rows[i]["IAmt5"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 24].Text = dt.Rows[i]["SugbE"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 25].Text = dt.Rows[i]["SugbF"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 26].Text = "";
                        ss1_Sheet1.Cells[i, 27].Text = dt.Rows[i]["BAmt"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 28].Text = dt.Rows[i]["TAmt"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 29].Text = dt.Rows[i]["IAmt"].ToString().Trim();
                    }
                }


                btnView.Enabled = false;
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                btnExit.Enabled = false;

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

        private void frmSearchSugaSQLX_rEventClosed()
        {
            //닫는 이벤트 내용
            if (frmSearchSugaSQLX != null)
            {
                frmSearchSugaSQLX.Dispose();
                frmSearchSugaSQLX = null;
            }
        }

        private void frmSearchSugaSQLX_rSetSuGaCodeSQL(string argSQL)
        {
            if (argSQL != "")
            {
                GstrHelpCode = argSQL;
            }
        }

        private void ss1_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            string strData = "";
            string strBun = "";
            string strNu = "";
            int nBAmt = 0;
            int nTAmt = 0;
            int nIAmt = 0;
            int nOldBAmt = 0;
            string strSuDate = "";
            string strGbE = "";
            string strGbF = "";

            ss1_Sheet1.Cells[e.Row, 26].Text = "Y";
            if ((e.Column == 4 || e.Column == 5 || e.Column == 6) && (chkJob_0.Checked == true))
            {
                // SS1_Suga_Move    '현재수가를 변경수가1로 Move
                //'변경수가1이 현재일보다 크거나 같으면 Move 안함
                strSuDate = ss1_Sheet1.Cells[e.Row, 8].Text;
                if (Convert.ToDateTime(strSuDate) >= dtpSudate.Value) return;


                //'보험수가가 변경되지 않았으면 이동 안함
                nBAmt = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[e.Row, 4].Text));
                nOldBAmt = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[e.Row, 27].Text));
                if (nBAmt == nOldBAmt) return;


                //'수가4를 수가5로 Move
                strSuDate = ss1_Sheet1.Cells[e.Row, 16].Text;
                nBAmt = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[e.Row, 17].Text));
                nTAmt = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[e.Row, 18].Text));
                nIAmt = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[e.Row, 19].Text));
                if (strSuDate != "" || nBAmt != 0 || nTAmt != 0 || nIAmt != 0)
                {
                    ss1_Sheet1.Cells[e.Row, 20].Text = strSuDate;
                    ss1_Sheet1.Cells[e.Row, 21].Text = Convert.ToString(nBAmt);
                    ss1_Sheet1.Cells[e.Row, 22].Text = Convert.ToString(nTAmt);
                    ss1_Sheet1.Cells[e.Row, 23].Text = Convert.ToString(nIAmt);
                }

                //'수가3를 수가4로 Move
                strSuDate = ss1_Sheet1.Cells[e.Row, 12].Text;
                nBAmt = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[e.Row, 13].Text));
                nTAmt = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[e.Row, 14].Text));
                nIAmt = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[e.Row, 15].Text));
                if (strSuDate != "" || nBAmt != 0 || nTAmt != 0 || nIAmt != 0)
                {
                    ss1_Sheet1.Cells[e.Row, 16].Text = strSuDate;
                    ss1_Sheet1.Cells[e.Row, 17].Text = Convert.ToString(nBAmt);
                    ss1_Sheet1.Cells[e.Row, 18].Text = Convert.ToString(nTAmt);
                    ss1_Sheet1.Cells[e.Row, 19].Text = Convert.ToString(nIAmt);
                }

                //'수가2를 수가3로 Move
                strSuDate = ss1_Sheet1.Cells[e.Row, 8].Text;
                nBAmt = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[e.Row, 9].Text));
                nTAmt = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[e.Row, 10].Text));
                nIAmt = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[e.Row, 11].Text));
                if (strSuDate != "" || nBAmt != 0 || nTAmt != 0 || nIAmt != 0)
                {
                    ss1_Sheet1.Cells[e.Row, 12].Text = strSuDate;
                    ss1_Sheet1.Cells[e.Row, 13].Text = Convert.ToString(nBAmt);
                    ss1_Sheet1.Cells[e.Row, 14].Text = Convert.ToString(nTAmt);
                    ss1_Sheet1.Cells[e.Row, 15].Text = Convert.ToString(nIAmt);
                }

                //'최초의 금액을 수가2로 Move
                strSuDate = ss1_Sheet1.Cells[e.Row, 8].Text;
                nBAmt = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[e.Row, 27].Text));
                nTAmt = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[e.Row, 28].Text));
                nIAmt = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[e.Row, 29].Text));
                if (strSuDate != "" || nBAmt != 0 || nTAmt != 0 || nIAmt != 0)
                {
                    ss1_Sheet1.Cells[e.Row, 8].Text = strSuDate;
                    ss1_Sheet1.Cells[e.Row, 9].Text = Convert.ToString(nBAmt);
                    ss1_Sheet1.Cells[e.Row, 10].Text = Convert.ToString(nTAmt);
                    ss1_Sheet1.Cells[e.Row, 11].Text = Convert.ToString(nIAmt);
                }
            }

            if (e.Column == 4 && chkJob_1.Checked == true)
            {
                //SS1_Suga_Gesan: '보험수가를 기준으로 자보,일반수가를 계산
                strBun = ss1_Sheet1.Cells[e.Row, 3].Text;
                nBAmt = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[e.Row, 4].Text));
                strGbE = ss1_Sheet1.Cells[e.Row, 24].Text;
                strGbF = ss1_Sheet1.Cells[e.Row, 25].Text;
                
                nIAmt = Gesan_IlbanAmt(nBAmt, strBun, strGbE, strGbF);
                ss1_Sheet1.Cells[e.Row, 5].Text = Convert.ToString(nBAmt);
                ss1_Sheet1.Cells[e.Row, 6].Text = Convert.ToString(nTAmt);                
            }
        }

        private int Gesan_IlbanAmt(int ArgBAmt, string argBun, string ArgSugbE, string ArgSugbF, string ArgSugbJ = "")
        {
            int nIAmt = 0;

            //'진찰료,입원료는 보험가,일반가 동일하게 처리함
            if (Convert.ToInt32(argBun) <= Convert.ToInt32("10"))
            {
                return ArgBAmt;
            }


            //'비급여수가(식대(74)-종합건진(84)는 보험가,일반가 동일하게 처리함
            if (Convert.ToInt32(argBun) >= Convert.ToInt32("74"))
            {
                return ArgBAmt;
            }


            //'내복약,외용약품의 일반가 계산
            if ((argBun == "11" || argBun == "12") && ArgSugbE == "0")
            {
                nIAmt = Gesan_IlbanAmt_YAK(ArgBAmt, ArgSugbF);
            }
            //'주사약 일반가 계산
            else if (argBun == "20" && ArgSugbE == "0")
            {
                nIAmt = Gesan_IlbanAmt_JUSA(ArgBAmt, ArgSugbF);
            }
            //'기타 일반수가를 계산
            else
            {
                nIAmt = Gesan_IlbanAmt_ETC(ArgBAmt, ArgSugbE, ArgSugbJ);
            }

            return nIAmt;
        }

        private int Gesan_IlbanAmt_YAK(int ArgBAmt, string ArgSugbF)   //'내복약,외용약품의 일반가 계산
        {
            int nIAmt = 0;

            //'비급여수가 100,000원이상은 보험가,일반가 동일하게 처리함
            if (ArgSugbF == "1" && ArgBAmt >= 100000)
            {
                nIAmt = ArgBAmt;
                return nIAmt;
            }

            if (ArgBAmt < 11)
            {
                nIAmt = ArgBAmt * 5;
            }
            else if (ArgBAmt < 51)
            {
                nIAmt = ArgBAmt * 4;
            }
            else if (ArgBAmt < 101)
            {
                nIAmt = Convert.ToInt32(ArgBAmt * 3.5);
            }
            else if (ArgBAmt < 500)
            {
                nIAmt = ArgBAmt * 3;
            }
            else if (ArgBAmt < 1001)
            {
                nIAmt = ArgBAmt * 2;
            }
            else
            {
                nIAmt = Convert.ToInt32(ArgBAmt * 1.5);
            }
            return nIAmt;
        }



        private int Gesan_IlbanAmt_JUSA(int ArgBAmt, string ArgSugbF)   //'주사약제 일반수가 계산
        {
            int nIAmt = 0;

            //'비급여수가 100,000원이상은 보험가,일반가 동일하게 처리함
            if (ArgSugbF == "1" && ArgBAmt >= 100000)
            {
                nIAmt = ArgBAmt;
                return nIAmt;
            }

            if (ArgBAmt < 501)
            {
                nIAmt = ArgBAmt * 5;
            }
            else if (ArgBAmt < 1001)
            {
                nIAmt = ArgBAmt * 4;
            }
            else if (ArgBAmt < 3001)
            {
                nIAmt = ArgBAmt * 3;
            }
            else if (ArgBAmt < 5001)
            {
                nIAmt = Convert.ToInt32(ArgBAmt * 2.5);
            }
            else if (ArgBAmt < 10001)
            {
                nIAmt = ArgBAmt * 2;
            }
            else
            {
                nIAmt = Convert.ToInt32(ArgBAmt * 1.5);
            }
            return nIAmt;
        }



        private int Gesan_IlbanAmt_ETC(int ArgBAmt, string ArgSugbE, string ArgSugbJ)    //'기타수가 일반가 계산
        {
            int nIAmt = 0;

            //'행위료이면 보험수가 * 보험병원가산율 * 2
            if (ArgSugbE == "1") nIAmt = Convert.ToInt32((ArgBAmt * 1.25) * 2);
            //'재료대이면 보험수가의 2배
            if (ArgSugbE != "1") nIAmt = ArgBAmt * 2;
            //'10원보다 크면 10원미만 절사
            //'외부의뢰검사 는 절사 않함
            if (ArgSugbJ != "9" && ArgSugbJ != "8")
            {
                if (nIAmt > 10)
                {
                    nIAmt = nIAmt / 10;
                    nIAmt = nIAmt * 10;
                }
            }
            return nIAmt;
        }

    }
}
