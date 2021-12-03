using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmSugaList
    /// File Name : frmSugaList.cs
    /// Title or Description : 수가코드 목록조회
    /// Author : 유진호
    /// Create Date : 2017-11-02
    /// Update Histroy :     
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso>
    /// VB\basic\busuga\BuSuga18.frm(FrmSugaList)
    /// </seealso>    
    /// </summary>
    public partial class frmSugaList : Form
    {
        private int FnOldRow;
        private string SQL = "";
        private string GstrHelpCode = "";

        private frmSearchSugaSQL frmSearchSugaSQLX = null;

        public delegate void SetHelpCode(string strHelpCode);
        public event SetHelpCode rSetHelpCode;

        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        public frmSugaList()
        {
            InitializeComponent();
        }

        private void frmSugaList_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            GstrHelpCode = "";
            FnOldRow = 0;
            txtSuNameK.Text = "";

            if (VB.Trim(clsPublic.GstrPassProgramID) == "BVSUGA")
            {
                btnPrint.Visible = false;
                ss1.AutoClipboard = false;
            }

            txtCDate.Text = ComFunc.FormatStrToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"),"D");
        }

        private void DISPLAY_SUGA()
        {

            int i = 0;
            //, j;
            //int nREAD = 0;
            int nRow = 0;

            double nPrice = 0;
            double nPrice1 = 0;
            double nLPrice1 = 0;

            //string strData = "";
            //string strSuCode = "";
            string strJDate1 = "";
            string strLDate1 = "";
            string strChkDate = "";

            DataTable dt = null;
            DataTable dt2 = null;
            DataTable dt3 = null;
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                if (SQL == "") return;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    ComFunc.MsgBox("자료가 1건도 없습니다.", "확인");
                    dt.Dispose();
                    dt = null;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ss1_Sheet1.RowCount = 0;
                    ss1_Sheet1.RowCount = dt.Rows.Count;
                    //ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    strChkDate = txtCDate.Text;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if ((chkGroup.Checked == true && dt.Rows[i]["Gbn"].ToString().Trim() == "H") || (dt.Rows[i]["Gbn"].ToString().Trim() == "T"))
                        {
                            nRow = nRow + 1;
                            if (nRow > ss1_Sheet1.RowCount) ss1_Sheet1.RowCount = ss1_Sheet1.RowCount + 10;

                            //2021-01-06 비급여고지조회-------------
                            SQL = " SELECT GBSELFHANG FROM ADMIN.BAS_SUN ";
                            SQL = SQL + " WHERE SUNEXT = '" + dt.Rows[i]["SuNext"].ToString().Trim() + "'";

                            SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }

                            if (dt2.Rows.Count > 0)
                            {
                                if (dt2.Rows[0]["GBSELFHANG"].ToString().Trim() != "")
                                {
                                    ss1_Sheet1.Cells[nRow - 1, 19].Text = "Y";
                                }
                            }

                            dt2.Dispose();
                            dt2 = null;
                            //------------------------------------------

                            ss1_Sheet1.Cells[nRow -1, 0].Text = dt.Rows[i]["SuCode"].ToString().Trim();
                            ss1_Sheet1.Cells[nRow -1, 1].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                            ss1_Sheet1.Cells[nRow -1, 2].Text = dt.Rows[i]["SugbA"].ToString().Trim();
                            ss1_Sheet1.Cells[nRow -1, 3].Text = VB.Val(dt.Rows[i]["BAmt"].ToString().Trim()).ToString("###,###,##0 ");
                            ss1_Sheet1.Cells[nRow -1, 4].Text = dt.Rows[i]["SuNameK"].ToString().Trim();
                            ss1_Sheet1.Cells[nRow -1, 5].Text = dt.Rows[i]["SuDate"].ToString().Trim();
                            ss1_Sheet1.Cells[nRow -1, 6].Text = dt.Rows[i]["SugbE"].ToString().Trim();
                            ss1_Sheet1.Cells[nRow -1, 7].Text = dt.Rows[i]["SugbF"].ToString().Trim();
                            ss1_Sheet1.Cells[nRow -1, 8].Text = dt.Rows[i]["SugbP"].ToString().Trim();
                            ss1_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["SugbS"].ToString().Trim();    //'2016-09-20
                            ss1_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["SugbU"].ToString().Trim();    //'2018-11-30
                            ss1_Sheet1.Cells[nRow -1, 11].Text = dt.Rows[i]["BCode"].ToString().Trim();
                            ss1_Sheet1.Cells[nRow -1, 16].Text = dt.Rows[i]["Bun"].ToString().Trim() + "." + Read_BunName(dt.Rows[i]["Bun"].ToString().Trim());
                            ss1_Sheet1.Cells[nRow -1, 17].Text = VB.Val(dt.Rows[i]["oldbamt"].ToString().Trim()).ToString("###,###,##0 ");
                            ss1_Sheet1.Cells[nRow - 1, 18].Text = dt.Rows[i]["SUHAM"].ToString().Trim();



                            SQL = " SELECT SUNEXT, RGB FROM ADMIN.MIR_COLOR_SET ";
                            SQL = SQL + "  WHERE SABUN = '" + clsType.User.Sabun + "' ";
                            SQL = SQL + "      AND SUNEXT = '" + dt.Rows[i]["SuNext"].ToString().Trim() + "' ";
                            SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }

                            if (dt2.Rows.Count > 0)
                            {
                                if (dt2.Rows[0]["RGB"].ToString().Trim() != "")
                                {                                    
                                    ss1_Sheet1.Rows[nRow - 1].BackColor = Color.FromArgb(Convert.ToInt32(VB.Val(dt2.Rows[0]["RGB"].ToString().Trim())));                                    
                                }
                            }

                            dt2.Dispose();
                            dt2 = null;


                            SQL = " SELECT PRICE1, TO_CHAR(JDATE1, 'YYYY-MM-DD') JDATE1,  PRICE2, TO_CHAR(JDATE2, 'YYYY-MM-DD') JDATE2,  PRICE3, TO_CHAR(JDATE3, 'YYYY-MM-DD') JDATE3, PRICE4, TO_CHAR(JDATE4, 'YYYY-MM-DD') JDATE4, PRICE5, TO_CHAR(JDATE5, 'YYYY-MM-DD') JDATE5, ";
                            SQL = SQL + "  LPRICE1, TO_CHAR(LDATE1, 'YYYY-MM-DD') LDATE1,  LPRICE2, TO_CHAR(LDATE2, 'YYYY-MM-DD') LDATE2,  LPRICE3, TO_CHAR(LDATE3, 'YYYY-MM-DD') LDATE3, LPRICE4, TO_CHAR(LDATE4, 'YYYY-MM-DD') LDATE4, LPRICE5, TO_CHAR(LDATE5, 'YYYY-MM-DD') LDATE5, ";
                            SQL = SQL + " CODE_OLD  ";
                            SQL = SQL + " FROM EDI_SUGA WHERE CODE = '" + dt.Rows[i]["BCODE"].ToString().Trim() + "'";
                            if (dt.Rows[i]["edijong"].ToString().Trim() == "8")
                            {
                                SQL = SQL + " AND  JONG = '8' ";
                            }
                            else
                            {
                                SQL = SQL + " AND  JONG <>'8' ";
                            }
                            SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }
                            
                            nPrice1 = 0;
                            nLPrice1 = 0;

                            if (dt2.Rows.Count > 0)
                            {
                                if (dt.Rows[i]["SugbJ"].ToString().Trim() == "9")   //'외부의뢰검사는 의원급수가
                                {
                                    if (string.Compare(strChkDate, dt2.Rows[0]["ldate1"].ToString().Trim()) >= 0)
                                    {
                                        nLPrice1 = VB.Val(dt2.Rows[0]["LPrice1"].ToString().Trim().Replace(",", ""));
                                        strLDate1 = dt2.Rows[0]["ldate1"].ToString().Trim();
                                    }
                                    else if (string.Compare(strChkDate, dt2.Rows[0]["ldate2"].ToString().Trim()) >= 0)
                                    {
                                        nLPrice1 = VB.Val(dt2.Rows[0]["LPrice2"].ToString().Trim().Replace(",", ""));
                                        strLDate1 = dt2.Rows[0]["ldate2"].ToString().Trim();
                                    }
                                    else if (string.Compare(strChkDate, dt2.Rows[0]["ldate3"].ToString().Trim()) >= 0)
                                    {
                                        nLPrice1 = VB.Val(dt2.Rows[0]["LPrice3"].ToString().Trim().Replace(",", ""));
                                        strLDate1 = dt2.Rows[0]["ldate3"].ToString().Trim();
                                    }
                                    else if (string.Compare(strChkDate, dt2.Rows[0]["ldate4"].ToString().Trim()) >= 0)
                                    {
                                        nLPrice1 = VB.Val(dt2.Rows[0]["LPrice4"].ToString().Trim().Replace(",", ""));
                                        strLDate1 = dt2.Rows[0]["ldate4"].ToString().Trim();
                                    }
                                    else
                                    {
                                        nLPrice1 = VB.Val(dt2.Rows[0]["LPrice5"].ToString().Trim().Replace(",", ""));
                                        strLDate1 = dt2.Rows[0]["ldate5"].ToString().Trim();
                                    }
                                    nPrice = nLPrice1;
                                }
                                else
                                {
                                    if (string.Compare(strChkDate, dt2.Rows[0]["JDate1"].ToString().Trim()) >= 0)
                                    {
                                        nPrice1 = VB.Val(dt2.Rows[0]["Price1"].ToString().Trim().Replace(",", ""));
                                        strJDate1 = dt2.Rows[0]["JDate1"].ToString().Trim();
                                    }
                                    else if (string.Compare(strChkDate, dt2.Rows[0]["JDate2"].ToString().Trim()) >= 0)
                                    {
                                        nPrice1 = VB.Val(dt2.Rows[0]["Price2"].ToString().Trim().Replace(",", ""));
                                        strJDate1 = dt2.Rows[0]["JDate2"].ToString().Trim();
                                    }
                                    else if (string.Compare(strChkDate, dt2.Rows[0]["JDate3"].ToString().Trim()) >= 0)
                                    {
                                        nPrice1 = VB.Val(dt2.Rows[0]["Price3"].ToString().Trim().Replace(",", ""));
                                        strJDate1 = dt2.Rows[0]["JDate3"].ToString().Trim();
                                    }
                                    else if (string.Compare(strChkDate, dt2.Rows[0]["JDate4"].ToString().Trim()) >= 0)
                                    {
                                        nPrice1 = VB.Val(dt2.Rows[0]["Price4"].ToString().Trim().Replace(",", ""));
                                        strJDate1 = dt2.Rows[0]["JDate4"].ToString().Trim();
                                    }
                                    else
                                    {
                                        nPrice1 = VB.Val(dt2.Rows[0]["Price5"].ToString().Trim().Replace(",", ""));
                                        strJDate1 = dt2.Rows[0]["JDate5"].ToString().Trim();
                                    }
                                    nPrice = nPrice1;
                                }
                                
                                ss1_Sheet1.Cells[nRow - 1, 12].Text = nPrice1.ToString("###,###,##0 ");
                                ss1_Sheet1.Cells[nRow - 1, 13].Text = strJDate1;
                                ss1_Sheet1.Cells[nRow - 1, 14].Text = nLPrice1.ToString("###,###,##0 ");
                                ss1_Sheet1.Cells[nRow - 1, 15].Text = dt2.Rows[0]["CODE_OLD"].ToString().Trim();

                                dt2.Dispose();
                                dt2 = null;


                                //'환산계수 적용
                                nPrice = nPrice * (VB.Val(dt.Rows[i]["SUHAM"].ToString().Trim()) != 0 ? VB.Val(dt.Rows[i]["SUHAM"].ToString().Trim()) : 1);
                                nPrice = Convert.ToInt32(nPrice + 0.5);

                                //'퇴장방지약적용                                    
                                nPrice = nPrice * (dt.Rows[i]["SugbM"].ToString().Trim() == "1" ? 1.1 : 1);
                                nPrice = nPrice * (dt.Rows[i]["SugbJ"].ToString().Trim() == "9" ? 1.1 : 1);

                                if (dt.Rows[i]["SugbJ"].ToString().Trim() == "9")
                                {
                                    nPrice = VB.Fix(Convert.ToInt32(nPrice));
                                }
                                else if (dt.Rows[i]["SugbM"].ToString().Trim() == "1")
                                {
                                    nPrice = VB.Fix(Convert.ToInt32(((nPrice / 10) + 0.5)) * 10);
                                }



                                if (nPrice != VB.Val(dt.Rows[i]["BAmt"].ToString().Trim()))
                                {
                                    ss1_Sheet1.Cells[nRow - 1, 11].ForeColor = Color.Red;
                                    ss1_Sheet1.Cells[nRow - 1, 12].ForeColor = Color.Red;
                                    ss1_Sheet1.Cells[nRow - 1, 13].ForeColor = Color.Red;
                                }

                                SQL = " SELECT BAMT FROM BAS_SUGA_AMT ";
                                SQL = SQL + "  WHERE SUCODE ='" + dt.Rows[i]["SuCode"].ToString().Trim() + "'  ";
                                SQL = SQL + "    AND SUNEXT ='" + dt.Rows[i]["SuNext"].ToString().Trim() + "'  ";
                                SQL = SQL + "    AND SUDATE =TO_DATE('" + dt.Rows[i]["SuDate"].ToString().Trim() + "','YYYY-MM-DD') ";

                                SqlErr = clsDB.GetDataTableEx(ref dt3, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    return;
                                }

                                if (dt3.Rows.Count > 0)
                                {
                                    if (VB.Val(dt3.Rows[0]["BAmt"].ToString().Trim()) == 0 && nPrice == 0 && strJDate1 == "2018-01-01")
                                    {
                                        ss1_Sheet1.Cells[nRow - 1, 11].ForeColor = Color.Red;
                                        ss1_Sheet1.Cells[nRow - 1, 12].ForeColor = Color.Red;
                                        ss1_Sheet1.Cells[nRow - 1, 13].ForeColor = Color.Red;                                        
                                    }                                    
                                }

                                dt3.Dispose();
                                dt3 = null;                                
                            }
                        }
                    } //for
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
            Hide();
            //rEventClosed();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            if (VB.Trim(txtSuNameK.Text) == "") return;
            
            SQL = "     SELECT NU,BUN,SUCODE,GBN,SUNEXT,SUGBSS,SUGBBI,SUQTY,SUGBA,SUGBB,SUGBC,SUGBD,SUGBE,";
            SQL = SQL + "      SUGBF,SUGBG,SUGBH,SUGBI,SUGBJ,SUGBK,SUGBL,SUGBM,SUGBN,SUGBO,";
            SQL = SQL + "      SUGBP, SUGBQ, SUGBR, SUGBS, SUGBT, SUGBU, ";
            SQL = SQL + "      SUGBN, SUGBV, DAYMAX,TOTMAX,IAMT,TAMT,BAMT,";
            SQL = SQL + "      TO_CHAR(SUDATE,'YYYY-MM-DD')  SUDATE,OLDIAMT,OLDTAMT,OLDBAMT,";
            SQL = SQL + "      TO_CHAR(SUDATE3,'YYYY-MM-DD') SUDATE3,IAMT3,TAMT3,BAMT3,";
            SQL = SQL + "      TO_CHAR(SUDATE4,'YYYY-MM-DD') SUDATE4,IAMT4,TAMT4,BAMT4,";
            SQL = SQL + "      TO_CHAR(SUDATE5,'YYYY-MM-DD') SUDATE5,IAMT5,TAMT5,BAMT5,";
            SQL = SQL + "      SUNAMEK,SUNAMEE,SUNAMEG,UNIT,DAICODE,HCODE,BCODE,";
            SQL = SQL + "      SUHAM,EDIJONG,TO_CHAR(EDIDATE,'YYYY-MM-DD') EDIDATE,";
            SQL = SQL + "      OLDBCODE,OLDGESU,OLDJONG,WONCODE,WONAMT,NURCODE, NROWID ";
            SQL = SQL + "  FROM VIEW_SUGA_CODE ";
            SQL = SQL + " WHERE SUNAMEK  LIKE '%" + txtSuNameK.Text + "%'  ";
            SQL = SQL + "   AND DELDATE IS NULL ";

            Cursor.Current = Cursors.WaitCursor;
            DISPLAY_SUGA();
            Cursor.Current = Cursors.Default;
        }

        private void btnSearchView_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //    return; //권한 확인

            ss1_Sheet1.Columns[1].Visible = chkGroup.Checked;

            if (FnOldRow != -1)
            {
                ss1_Sheet1.Rows[FnOldRow].BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
                FnOldRow = -1;
            }
            
            SQL = "";
            
            //닫는 이벤트 내용
            if (frmSearchSugaSQLX != null)
            {
                frmSearchSugaSQLX.Dispose();
                frmSearchSugaSQLX = null;
            }
            frmSearchSugaSQLX = new frmSearchSugaSQL();
            frmSearchSugaSQLX.StartPosition = FormStartPosition.CenterParent;
            frmSearchSugaSQLX.rSetSuGaCodeSQL += frmSearchSugaSQLX_rSetSuGaCodeSQL;
            frmSearchSugaSQLX.rEventClosed += frmSearchSugaSQLX_rEventClosed;
            frmSearchSugaSQLX.ShowDialog();
            
            if (SQL == "") return;

            Cursor.Current = Cursors.WaitCursor;
            DISPLAY_SUGA();
            Cursor.Current = Cursors.Default;
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
                SQL = argSQL;
            }
        }

        private void ss1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ss1_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true) 
            {
                return;
            }

            if (FnOldRow != -1)
            {
                ss1_Sheet1.Rows[FnOldRow].BackColor = Color.White;
            }

            ss1_Sheet1.Rows[e.Row].BackColor = Color.FromArgb(129, 252, 224);
            FnOldRow = e.Row;            
            GstrHelpCode = ss1_Sheet1.Cells[e.Row, 0].Text.Trim().ToUpper();
            rSetHelpCode(GstrHelpCode);

            this.Hide();
        }

        private void txtCDate_DoubleClick(object sender, EventArgs e)
        {
            Calendar_Date_Select(txtCDate);
        }

        private void Calendar_Date_Select(Control ArgText)
        {
            clsPublic.GstrCalDate = VB.Trim(ArgText.Text);
            if (VB.Len(VB.Trim(ArgText.Text)) != 10)
            {
                clsPublic.GstrCalDate = clsPublic.GstrSysDate;
            }

            frmCalendar frmCalendarX = new frmCalendar();
            frmCalendarX.ShowDialog();

            if (VB.Len(clsPublic.GstrCalDate) == 10)
            {
                ArgText.Text = clsPublic.GstrCalDate;
            }
        }

        private string Read_BunName(string argBun)     //'분류명칭
        {
            string ArgReturn = "";
            //int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return ArgReturn; //권한 확인

                SQL = "SELECT Name FROM BAS_BUN ";
                SQL = SQL + "WHERE Jong = '1' ";
                SQL = SQL + "  AND Code = '" + VB.Trim(argBun) + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return ArgReturn;
                }

                if (dt.Rows.Count > 0)
                {
                    ArgReturn = dt.Rows[0]["Name"].ToString().Trim();
                }
                else
                {
                    ArgReturn = "";
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

            return ArgReturn;
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
            string Systime = "";

            SysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            Systime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            ss1_Sheet1.Columns[9].Visible = true;
            ss1_Sheet1.Columns[10].Visible = true;
            ss1_Sheet1.Columns[11].Visible = true;

            Cursor.Current = Cursors.WaitCursor;

            strFont1 = "/fn\"굴림체\" /fz\"16\" /fb0 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"10\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead1 = "/n/n/f1/C 수 가 목 록" + "/n/n/n/n";
            strHead2 = "/l/f2" + "인쇄일자 : " + SysDate + " " + Systime;
            strHead2 = strHead2 + "/r/f2" + "PAGE : /p";

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
        }

        private void ss1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ss1_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ss1, e.Column);
                return;
            }
        }
    }
}
