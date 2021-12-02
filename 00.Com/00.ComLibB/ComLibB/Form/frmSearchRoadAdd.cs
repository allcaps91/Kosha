using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmSearchRoadAdd : Form
    {
        ComboBox ComboCap = null;
        TextBox TextDongNm = null;
        string[] strData = new string[5001];
        string FstrFlag = "";
        private string GstrValue = "";
        private string GstrMsgList = "";

        public delegate void SetGstrValue(string GstrValue);
        public event SetGstrValue rSetGstrValue;

        public delegate void SetGstrMsgList(string GstrMsgList);
        public event SetGstrMsgList rSetGstrMsgList;

        public delegate void EventClose();
        public event EventClose rEventClose;


        public frmSearchRoadAdd()
        {
            InitializeComponent();
        }

        public frmSearchRoadAdd(string strFlag)
        {
            InitializeComponent();
            FstrFlag = strFlag;

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            //rEventClose();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            GstrValue = "";
            rSetGstrValue(GstrValue);
        }

        private void cboCap_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ss1_Sheet1.RowCount = 0;
                TextDongNm.ImeMode = ImeMode.Hangul;
                TextDongNm.SelectAll();
                TextDongNm.Focus();
                SendKeys.Send("{Tab}");
            }
            
        }

        private void txtDongNm_MouseDown(object sender, MouseEventArgs e)
        {
            ss1_Sheet1.RowCount = 0;
            TextDongNm.ImeMode = ImeMode.Hangul;
            TextDongNm.Select();
        }

        private void txtDongNm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (VB.Len(TextDongNm.Text) >= 2)
                {
                    FstrFlag = "OK";
                    TextDongNm.Text = VB.UCase(TextDongNm.Text).Trim();

                    Juso_Display();

                    FstrFlag = "";
                    ss1.Focus();
                }
            }
        }

        private void Juso_Display()
        {
            string strShow = "";
            int i = 0;

            string strHeadJuso = "";
            string strRoadJuso = "";
            string strJibunJuso = "";
            string strZipName = "";
            string strWord = "";
            string strBun = "";
            string strBun2 = "";

            

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            if (FstrFlag == "")
                return;

            if (TextDongNm.Text.Trim() == "")
            {
                ComFunc.MsgBox("검색명이 없습니다.");
                return;
            }

            if (VB.I(TextDongNm.Text, "-") > 1)
            {
                strBun2 = VB.Pstr(TextDongNm.Text, "-", 2);
                strWord = VB.Pstr(VB.Pstr(TextDongNm.Text, "-", 1), " ", 1);
                strBun = VB.Pstr(VB.Pstr(TextDongNm.Text, "-", 1), " ", 2);
            }
            else if (VB.I(TextDongNm.Text, " ") > 1)
            {
                strWord = VB.Pstr(TextDongNm.Text, " ", 1);
                strBun = VB.Pstr(TextDongNm.Text, " ", 2);
            }
            else
            {
                strWord = TextDongNm.Text.Trim();
                strBun = "";
            }


            Control[] controls = ComFunc.GetAllControls(this);

            foreach (Control ctl in controls)
            {
                if (ctl is RadioButton)
                {
                    if (VB.Left(((RadioButton)ctl).Name, 10) == "rdoZipName")
                    {
                        if (((RadioButton)ctl).Checked == true)
                        {
                            strZipName = ((RadioButton)ctl).Text;
                            break;
                        }
                    }
                }
            }

            for (i = 0; i < 5001; i++)
                strData[i] = "";

            ss1_Sheet1.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT /*+ index(bas_zips_road INDEX_ZIPSROAD9)*/ZIPNAME1 || ' ' || ZIPNAME2 || ' ' || ZIPNAME3 AS HEADJUSO, ";
                SQL = SQL + ComNum.VBLF + "        ROADNAME, BUN1, BUN2, DONGNAME, RINAME, DONGNAME2, SAN,    ";
                SQL = SQL + ComNum.VBLF + "        JIBUN1, JIBUN2, ROADCODE, BUILDNAME, BUILDNO, ZIPCODE, MAILJIYEK      ";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.Bas_ZIPS_ROAD                                  ";
                SQL = SQL + ComNum.VBLF + "  Where ZIPNAME1 ='" + strZipName + "'                             ";

                if (ComboCap.Text.Trim() != "전체")
                {
                    SQL = SQL + ComNum.VBLF + "    AND ZIPNAME2 ='" + ComboCap.Text.Trim() + "'                    ";
                }

                if (VB.Right(strWord, 1) == "리")
                {
                    SQL = SQL + ComNum.VBLF + "    AND RINAME LIKE '" + strWord + "%'                         ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND (ROADNAME LIKE '%" + strWord + "%'                     ";
                    SQL = SQL + ComNum.VBLF + "     OR DONGNAME  LIKE '%" + strWord + "%'                     ";
                    SQL = SQL + ComNum.VBLF + "     OR DONGNAME2 LIKE '%" + strWord + "%'                     ";
                    SQL = SQL + ComNum.VBLF + "     OR BUILDNAME LIKE '%" + strWord + "%')                    ";
                }

                if (strBun != "")
                {
                    if (VB.Right(strWord, 1) == "로" || VB.Right(strWord, 1) == "길")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND  BUN1  LIKE '%" + strBun + "%'                    ";
                    }
                    else if (VB.Right(strWord, 1) == "동" || VB.Right(strWord, 1) == "읍" ||
                            VB.Right(strWord, 1) == "면" || VB.Right(strWord, 1) == "리")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND  JIBUN1  LIKE '%" + strBun + "%'                    ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "    AND ( BUN1  LIKE '%" + strBun + "%'                    ";
                        SQL = SQL + ComNum.VBLF + "     OR JIBUN1  LIKE '%" + strBun + "%'    )               ";
                    }
                }

                if (strBun2 != "")
                {
                    if (VB.Right(strWord, 1) == "로" || VB.Right(strWord, 1) == "길")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND  BUN2  LIKE '%" + strBun2 + "%'                    ";
                    }
                    else if (VB.Right(strWord, 1) == "동" || VB.Right(strWord, 1) == "읍" ||
                            VB.Right(strWord, 1) == "면" || VB.Right(strWord, 1) == "리")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND  JIBUN2  LIKE '%" + strBun2 + "%'                    ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "    AND ( BUN2  LIKE '%" + strBun2 + "%'                    ";
                        SQL = SQL + ComNum.VBLF + "     OR JIBUN2  LIKE '%" + strBun2 + "%'    )               ";
                    }
                }

                if (tabControl1.SelectedIndex == 0)
                    SQL = SQL + ComNum.VBLF + "  ORDER BY HEADJUSO, ROADNAME, BUN1, BUN2                      ";
                else if (tabControl1.SelectedIndex == 1)
                    SQL = SQL + ComNum.VBLF + "  ORDER BY HEADJUSO, DONGNAME, DONGNAME2, RINAME, JIBUN1, JIBUN2 ";

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                if (dt.Rows.Count > 5000)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("자료건수가 5000개가 넘었습니다. 명칭을 자세히 기입하여 다시 조회해주세요.");
                    return;
                }

                ss1_Sheet1.RowCount = dt.Rows.Count;
                ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strHeadJuso = dt.Rows[i]["HEADJUSO"].ToString().Trim();
                    strRoadJuso = dt.Rows[i]["ROADNAME"].ToString().Trim();
                    strRoadJuso = strRoadJuso + " " + dt.Rows[i]["BUN1"].ToString().Trim();

                    if (VB.Val(dt.Rows[i]["BUN2"].ToString().Trim()) > 0)
                        strRoadJuso = strRoadJuso + " " + dt.Rows[i]["BUN2"].ToString().Trim();
                    if (dt.Rows[i]["BUILDNAME"].ToString().Trim() != "")
                        strRoadJuso = strRoadJuso + " " + dt.Rows[i]["BUILDNAME"].ToString().Trim();

                    strJibunJuso = "";

                    if (dt.Rows[i]["RINAME"].ToString().Trim() != "")
                    {
                        strJibunJuso = strJibunJuso + " " + dt.Rows[i]["RINAME"].ToString().Trim();
                    }
                    else
                    {
                        if (dt.Rows[i]["DONGNAME"].ToString().Trim() == "")
                            strJibunJuso = strJibunJuso + " " + dt.Rows[i]["DONGNAME2"].ToString().Trim();
                        else
                            strJibunJuso = strJibunJuso + " " + dt.Rows[i]["DONGNAME"].ToString().Trim();
                    }

                    if (VB.Val(dt.Rows[i]["SAN"].ToString().Trim()) > 0)
                        strJibunJuso = strJibunJuso + "산";

                    strJibunJuso = strJibunJuso + dt.Rows[i]["JIBUN1"].ToString().Trim();

                    if (VB.Val(dt.Rows[i]["JIBUN2"].ToString().Trim()) > 0)
                        strJibunJuso = strJibunJuso + "-" + dt.Rows[i]["JIBUN2"].ToString().Trim();

                    ss1_Sheet1.Cells[i, 0].Text = strHeadJuso + " " + strRoadJuso;
                    ss1_Sheet1.Cells[i, 1].Text = strHeadJuso + " " + strJibunJuso;
                    ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ZIPCODE"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ROADCODE"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["BUILDNO"].ToString().Trim();

                    strShow = "";
                    strShow = dt.Rows[i]["ZIPCODE"].ToString().Trim() + "|";                //1 우편번호
                    strShow = strShow + strHeadJuso + " " + strRoadJuso + "|";              //2 도로명주소
                    strShow = strShow + dt.Rows[i]["MailJiYek"].ToString().Trim() + "|";    //3 지역코드
                    strShow = strShow + dt.Rows[i]["ROADCODE"].ToString().Trim() + "|";     //4 도로명코드
                    strShow = strShow + dt.Rows[i]["BUILDNO"].ToString().Trim();            //5 건물번호

                    strData[i + 1] = strShow;
                }
                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            ss1.Focus();

        }

        private void frmSearchRoadAdd_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            //ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            GstrMsgList = "";   //명칭까지포함여부

            
            txtDongNm0.Text = "";
            txtDongNm1.Text = "";
            cboCap0.Items.Clear();
            cboCap1.Items.Clear();

            cboCap0.Items.Add("전체");
            cboCap1.Items.Add("전체");

            TextDongNm = txtDongNm0;
            ComboCap = cboCap0;

            try
            {
                SQL = "";
                SQL = " SELECT ZIPNAME2 From KOSMOS_PMPA.BAS_ZIPS_ROAD Where ZIPNAME1 ='경상북도' ";
                SQL = SQL + ComNum.VBLF + " Group By ZIPNAME2 ";

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
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboCap0.Items.Add(dt.Rows[i]["ZIPNAME2"].ToString().Trim());
                    cboCap1.Items.Add(dt.Rows[i]["ZIPNAME2"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            cboCap0.SelectedIndex = 0;
            cboCap1.SelectedIndex = 0;

            FstrFlag = "";
            txtDongNm0.Select();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            FstrFlag = "";

            if (tabControl1.SelectedIndex == 0)
            {
                ComboCap = cboCap0;
                TextDongNm = txtDongNm0;
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                ComboCap = cboCap1;
                TextDongNm = txtDongNm1;
            }

            Juso_Display();
        }

        private void rdoZipName_Click(object sender, EventArgs e)
        {
            string strCap1 = "";
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            if (((RadioButton)sender).Checked == true)
            {
                strCap1 = ((RadioButton)sender).Text;
            }

          

            ComboCap.Items.Clear();
            ComboCap.Items.Add("전체");
            ComboCap.SelectedIndex = 0;

            TextDongNm.Text = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT ZIPNAME2 From KOSMOS_PMPA.BAS_ZIPS_ROAD Where ZIPNAME1 ='" + strCap1 + "' ";
                SQL = SQL + ComNum.VBLF + " Group By ZIPNAME2 ";

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
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ComboCap.Items.Add(dt.Rows[i]["ZIPNAME2"].ToString().Trim());
                }
                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void ss1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ss1_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ss1, e.Column);
                return;
            }

            ss1_Sheet1.Cells[0, 0, ss1_Sheet1.RowCount - 1, ss1_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ss1_Sheet1.Cells[e.Row, 0, e.Row, ss1_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            GstrValue = strData[e.Row + 1];

            rSetGstrValue(GstrValue);
            this.Close();
        }

        private void ss1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (ss1_Sheet1.RowCount == 0)
                {
                    return;
                }

                ss1_Sheet1.Cells[0, 0, ss1_Sheet1.RowCount - 1, ss1_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
                ss1_Sheet1.Cells[ss1_Sheet1.ActiveRowIndex, 0, ss1_Sheet1.ActiveRowIndex, ss1_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

                GstrValue = strData[ss1_Sheet1.ActiveRowIndex + 1];

                rSetGstrValue(GstrValue);
                this.Close();
            }
        }

    }
}
