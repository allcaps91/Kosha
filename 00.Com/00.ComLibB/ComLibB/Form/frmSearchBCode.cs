using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary> 표준코드 찾기 </summary>
    public partial class frmSearchBCode : Form
    {
        string GstrHelpCode = string.Empty; //global
        string GstrPassProgramID = string.Empty; //global


        //폼이 Close될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        /// <summary> 표준코드 찾기 </summary>
        public frmSearchBCode()
        {
            InitializeComponent();
        }

        public frmSearchBCode(string rGstrHelpCode)
        {
            InitializeComponent();
            GstrHelpCode = rGstrHelpCode;
        }

        void frmSearchBCode_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            //ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            ssView1_Sheet1.RowCount = 0;
            ssView2_Sheet1.RowCount = 0;

            if (GstrHelpCode != "")
            {
                txtData.Text = GstrHelpCode;
                rdoJong1.Checked = true;
                btnView();
            }

            if (GstrPassProgramID.Trim() == "BVSUGA")
            {
                btnPrint.Visible = false;
            }
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            //rEventClosed();
            this.Close();
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            btnView();
        }

        void btnView()
        {
            int i = 0;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            txtData.Text = txtData.Text.Trim();

            if (txtData.Text == "")
            {
                ComFunc.MsgBox("찾으실 자료가 공란입니다.", "확인");
                txtData.Focus();
                return;
            }

            ssView1.Enabled = true;
            btnCancel.Enabled = true;
            btnPrint.Enabled = true;

            try
            {


                SQL = "";
                SQL = "     SELECT Code,Jong,Pname,Spec,Danwi1,Danwi2,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(JDate1,'YYYY-MM-DD') JDate1,Price1, JANG1, SA1, ";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(JDate2,'YYYY-MM-DD') JDate2,Price2, JANG2, SA2, ";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(JDate3,'YYYY-MM-DD') JDate3,Price3, JANG3, SA3, ";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(JDate4,'YYYY-MM-DD') JDate4,Price4, JANG4, SA4, ";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(JDate5,'YYYY-MM-DD') JDate5,Price5, JANG5, SA5, ";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(JDate6,'YYYY-MM-DD') JDate6,Price6, JANG6, SA6, KASAN, ";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(LDate1,'YYYY-MM-DD') LDate1,LPrice1,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(LDate2,'YYYY-MM-DD') LDate2,LPrice2,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(LDate3,'YYYY-MM-DD') LDate3,LPrice3,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(LDate4,'YYYY-MM-DD') LDate4,LPrice4,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(LDate5,'YYYY-MM-DD') LDate5,LPrice5,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(CDate1,'YYYY-MM-DD') CDate1,CPrice1,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(CDate2,'YYYY-MM-DD') CDate2,CPrice2,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(CDate3,'YYYY-MM-DD') CDate3,CPrice3,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(CDate4,'YYYY-MM-DD') CDate4,CPrice4,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(CDate5,'YYYY-MM-DD') CDate5,CPrice5,";
                SQL = SQL + ComNum.VBLF + "      COMPNY,EFFECT, SCORE, SUSUL, SUGAGBN  ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "EDI_SUGA ";

                if (rdoJong0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "WHERE PName LIKE '%" + txtData.Text.Trim() + "%' ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY PName ";
                }
                else if (rdoJong1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "WHERE Code LIKE '" + txtData.Text.Trim().ToUpper() + "%' ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY Code ";
                }
                else if (rdoJong2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "WHERE Compny LIKE '" + txtData.Text.Trim() + "%' ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY Compny,Code ";
                }
                else if (rdoJong3.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "WHERE Effect LIKE '" + txtData.Text.Trim() + "%' ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY Effect,Code ";
                }


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
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView1_Sheet1.RowCount = dt.Rows.Count;
                ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Code"].ToString().Trim();

                    if (dt.Rows[i]["Jong"].ToString().Trim() == "1" && dt.Rows[i]["Spec"].ToString().Trim() == "1")
                    { // 수가일경우
                        ssView1_Sheet1.ColumnHeader.Cells.Get(i, 0).ForeColor = Color.FromArgb(255, 0, 0);
                        ssView1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PName"].ToString().Trim();
                    }
                    else
                    {
                        ssView1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PName"].ToString().Trim() + " " + dt.Rows[i]["Spec"].ToString().Trim();
                    }

                    if (dt.Rows[i]["SUGAGBN"].ToString().Trim() == "2")
                    {
                        ssView1_Sheet1.Cells[i, 1].Text = "[비]" + ssView1_Sheet1.Cells[i, 1].Text;
                    }

                    ssView1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Jong"].ToString().Trim();
                    ssView1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Danwi1"].ToString().Trim() + dt.Rows[i]["Danwi2"].ToString().Trim();
                    ssView1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SCORE"].ToString().Trim();
                    ssView1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SUSUL"].ToString().Trim();
                    ssView1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["JDate1"].ToString().Trim();
                    ssView1_Sheet1.Cells[i, 7].Text = VB.Format(VB.Val(dt.Rows[i]["Price1"].ToString().Trim()), "###,###,##0");
                    ssView1_Sheet1.Cells[i, 8].Text = VB.Format(VB.Val(dt.Rows[i]["JANG1"].ToString().Trim()), "###,###,##0");

                    switch (dt.Rows[i]["SA1"].ToString().Trim())
                    {
                        case "1":
                            ssView1_Sheet1.Cells[i, 9].Text = "급여";
                            break;
                        case "2":
                            ssView1_Sheet1.Cells[i, 9].Text = "미생산";
                            break;
                        case "3":
                            ssView1_Sheet1.Cells[i, 9].Text = "본인부담";
                            break;
                        case "4":
                            ssView1_Sheet1.Cells[i, 9].Text = "삭제";
                            break;
                        case "5":
                            ssView1_Sheet1.Cells[i, 9].Text = "공상제외";
                            break;
                        case "6":
                            ssView1_Sheet1.Cells[i, 9].Text = "급여정지";
                            break;
                    }

                    ssView1_Sheet1.Cells[i, 10].Text = dt.Rows[i]["Compny"].ToString().Trim();
                    ssView1_Sheet1.Cells[i, 11].Text = dt.Rows[i]["Effect"].ToString().Trim();
                    ssView1_Sheet1.Cells[i, 12].Text = dt.Rows[i]["JDate2"].ToString().Trim();
                    ssView1_Sheet1.Cells[i, 13].Text = VB.Format(VB.Val(dt.Rows[i]["Price2"].ToString().Trim()), "###,###,##0");
                    ssView1_Sheet1.Cells[i, 14].Text = VB.Format(VB.Val(dt.Rows[i]["JANG2"].ToString().Trim()), "###,###,##0");

                    switch (dt.Rows[i]["SA2"].ToString().Trim())
                    {
                        case "1":
                            ssView1_Sheet1.Cells[i, 15].Text = "급여";
                            break;
                        case "2":
                            ssView1_Sheet1.Cells[i, 15].Text = "미생산";
                            break;
                        case "3":
                            ssView1_Sheet1.Cells[i, 15].Text = "본인부담";
                            break;
                        case "4":
                            ssView1_Sheet1.Cells[i, 15].Text = "삭제";
                            break;
                        case "5":
                            ssView1_Sheet1.Cells[i, 15].Text = "공상제외";
                            break;
                        case "6":
                            ssView1_Sheet1.Cells[i, 15].Text = "급여정지";
                            break;
                    }

                    ssView1_Sheet1.Cells[i, 16].Text = dt.Rows[i]["JDate3"].ToString().Trim();
                    ssView1_Sheet1.Cells[i, 17].Text = VB.Format(VB.Val(dt.Rows[i]["Price3"].ToString().Trim()), "###,###,##0");
                    ssView1_Sheet1.Cells[i, 18].Text = dt.Rows[i]["JDate4"].ToString().Trim();
                    ssView1_Sheet1.Cells[i, 19].Text = VB.Format(VB.Val(dt.Rows[i]["Price4"].ToString().Trim()), "###,###,##0");
                    ssView1_Sheet1.Cells[i, 20].Text = dt.Rows[i]["JDate5"].ToString().Trim();
                    ssView1_Sheet1.Cells[i, 21].Text = VB.Format(VB.Val(dt.Rows[i]["Price5"].ToString().Trim()), "###,###,##0");
                    ssView1_Sheet1.Cells[i, 22].Text = dt.Rows[i]["LDate1"].ToString().Trim();
                    ssView1_Sheet1.Cells[i, 23].Text = VB.Format(VB.Val(dt.Rows[i]["LPrice1"].ToString().Trim()), "###,###,##0");
                    ssView1_Sheet1.Cells[i, 24].Text = dt.Rows[i]["LDate2"].ToString().Trim();
                    ssView1_Sheet1.Cells[i, 25].Text = VB.Format(VB.Val(dt.Rows[i]["LPrice2"].ToString().Trim()), "###,###,##0");
                    ssView1_Sheet1.Cells[i, 26].Text = dt.Rows[i]["LDate3"].ToString().Trim();
                    ssView1_Sheet1.Cells[i, 27].Text = VB.Format(VB.Val(dt.Rows[i]["LPrice3"].ToString().Trim()), "###,###,##0");
                    ssView1_Sheet1.Cells[i, 28].Text = dt.Rows[i]["LDate4"].ToString().Trim();
                    ssView1_Sheet1.Cells[i, 29].Text = VB.Format(VB.Val(dt.Rows[i]["LPrice4"].ToString().Trim()), "###,###,##0");
                    ssView1_Sheet1.Cells[i, 30].Text = dt.Rows[i]["LDate5"].ToString().Trim();
                    ssView1_Sheet1.Cells[i, 31].Text = VB.Format(VB.Val(dt.Rows[i]["LPrice5"].ToString().Trim()), "###,###,##0");
                    ssView1_Sheet1.Cells[i, 32].Text = dt.Rows[i]["CDate1"].ToString().Trim();
                    ssView1_Sheet1.Cells[i, 33].Text = VB.Format(VB.Val(dt.Rows[i]["CPrice1"].ToString().Trim()), "###,###,##0");
                    ssView1_Sheet1.Cells[i, 34].Text = dt.Rows[i]["CDate2"].ToString().Trim();
                    ssView1_Sheet1.Cells[i, 35].Text = VB.Format(VB.Val(dt.Rows[i]["CPrice2"].ToString().Trim()), "###,###,##0");
                    ssView1_Sheet1.Cells[i, 36].Text = dt.Rows[i]["CDate3"].ToString().Trim();
                    ssView1_Sheet1.Cells[i, 37].Text = VB.Format(VB.Val(dt.Rows[i]["CPrice3"].ToString().Trim()), "###,###,##0");
                    ssView1_Sheet1.Cells[i, 38].Text = dt.Rows[i]["CDate4"].ToString().Trim();
                    ssView1_Sheet1.Cells[i, 39].Text = VB.Format(VB.Val(dt.Rows[i]["CPrice4"].ToString().Trim()), "###,###,##0");
                    ssView1_Sheet1.Cells[i, 40].Text = dt.Rows[i]["CDate5"].ToString().Trim();
                    ssView1_Sheet1.Cells[i, 41].Text = VB.Format(VB.Val(dt.Rows[i]["CPrice5"].ToString().Trim()), "###,###,##0");

                    if (dt.Rows[i]["SUGAGBN"].ToString().Trim() == "2")
                    {
                        ssView1_Sheet1.ColumnHeader.Cells.Get(0, 0).ForeColor = Color.Red;
                    }
                }

                dt.Dispose();
                dt = null;

                //'DRG 응급행위가산
                SQL = "";
                SQL = " SELECT  DDATE, CODE, GBN, DNAME, SNAME, DJUMSUS, DAMTS, DJUMSUM, DAMTM, DJUMSUU, DAMTU, DJUMSUL, DAMTL ";
                SQL = SQL + ComNum.VBLF + "  FROM DRG_CODE_ER";

                if (rdoJong1.Checked == true)
                {
                    SQL = SQL + " WHERE Code LIKE '" + txtData.Text.Trim().ToUpper() + "%' ";
                    //SQL = SQL + " ORDER BY gbn, Code ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " WHERE DName LIKE '%" + txtData.Text.Trim() + "%'  ";
                    //SQL = SQL + ComNum.VBLF + " ORDER BY gbn, dName";
                }

                SQL = SQL + ComNum.VBLF + " ORDER BY DDATE DESC";

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
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView2_Sheet1.RowCount = dt.Rows.Count;
                ssView2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if(dt.Rows[i]["DDATE"].ToString().Trim() != "")
                    {
                        ssView2_Sheet1.Cells[i, 0].Text = ComFunc.FormatStrToDateTime(dt.Rows[i]["DDATE"].ToString().Trim(), "D");
                    }

                    ssView2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Code"].ToString().Trim();
                    ssView2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DNAME"].ToString().Trim() + " " + dt.Rows[i]["sName"].ToString().Trim();
                    ssView2_Sheet1.Cells[i, 3].Text = "별표" + dt.Rows[i]["Gbn"].ToString().Trim();
                    ssView2_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DJUMSUS"].ToString().Trim();
                    ssView2_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DAMTS"].ToString().Trim();
                    ssView2_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DJUMSUM"].ToString().Trim();
                    ssView2_Sheet1.Cells[i, 7].Text = dt.Rows[i]["DAMTM"].ToString().Trim();
                    ssView2_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DJUMSUU"].ToString().Trim();
                    ssView2_Sheet1.Cells[i, 9].Text = dt.Rows[i]["DAMTU"].ToString().Trim();
                    ssView2_Sheet1.Cells[i, 10].Text = dt.Rows[i]["DJUMSUL"].ToString().Trim();
                    ssView2_Sheet1.Cells[i, 11].Text = dt.Rows[i]["DAMTL"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            txtData.Text = "";
            ssView1_Sheet1.RowCount = 0;
            ssView2_Sheet1.RowCount = 0;
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            string strFont1 = string.Empty;
            string strFont2 = string.Empty;
            string strHead1 = string.Empty;
            string strHead2 = string.Empty;

            ssView1_Sheet1.ColumnHeader.Columns[8].Visible = false;
            ssView1_Sheet1.ColumnHeader.Columns[9].Visible = false;
            ssView1_Sheet1.ColumnHeader.Columns[10].Visible = false;
            ssView1_Sheet1.ColumnHeader.Columns[11].Visible = false;
            ssView1_Sheet1.ColumnHeader.Columns[12].Visible = false;
            ssView1_Sheet1.ColumnHeader.Columns[13].Visible = false;
            ssView1_Sheet1.ColumnHeader.Columns[14].Visible = false;
            ssView1_Sheet1.ColumnHeader.Columns[15].Visible = false;

            Cursor.Current = Cursors.WaitCursor;

            strFont1 = "/fn\"굴림체\" /fz\"16\" /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"10\" /fs2";
            strHead1 = "/c/f1" + " 표준수가코드" + "/n";
            strHead2 = "/n" + "/l/f2" + "인쇄일자 : " + Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")) + " " + Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T", ":"));
            strHead2 += "/r" + "PAGE:" + "/p" + VB.Space(15);

            ssView1_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssView1_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView1_Sheet1.PrintInfo.Margin.Top = 50;
            ssView1_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssView1_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView1_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView1_Sheet1.PrintInfo.ShowBorder = true;
            ssView1_Sheet1.PrintInfo.ShowColor = true;
            ssView1_Sheet1.PrintInfo.ShowGrid = true;
            ssView1_Sheet1.PrintInfo.ShowShadows = false;
            ssView1_Sheet1.PrintInfo.UseMax = false;
            ssView1_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView1.PrintSheet(0);

            ssView1_Sheet1.ColumnHeader.Columns[8].Visible = true;
            ssView1_Sheet1.ColumnHeader.Columns[9].Visible = true;
            ssView1_Sheet1.ColumnHeader.Columns[10].Visible = true;
            ssView1_Sheet1.ColumnHeader.Columns[11].Visible = true;
            ssView1_Sheet1.ColumnHeader.Columns[12].Visible = true;
            ssView1_Sheet1.ColumnHeader.Columns[13].Visible = true;
            ssView1_Sheet1.ColumnHeader.Columns[14].Visible = true;
            ssView1_Sheet1.ColumnHeader.Columns[15].Visible = true;

            Cursor.Current = Cursors.Default;
        }

        private void txtData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

    }
}
