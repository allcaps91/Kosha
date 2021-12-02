using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

/// <summary>
/// Description : 상병조회
/// Author : 김형범
/// Create Date : 2017.06.
/// </summary>
/// <history>
/// VB열서어 FileName이맞고VBName이틀린폼으로 하는지 FileName이틀리고VBName이맞는폼으로 할지 확인필요 ( VBName이 맞는 form으로 했음)
/// DB 조회중 ORA-29275: 부분 다중 바이트 문자 에러발생
/// FileOpen 및 고속print
/// </history>
namespace ComLibB
{
    /// <summary> 상병조회 </summary>
    public partial class frmILLHelp : Form
    {

        string GstrRetValue = "";
        string GstrHelpCode = "";

        public delegate void EventExit();
        public event EventExit rEventExit;

        public delegate void SendText(string strText);
        public event SendText rSendText;

        /// <summary> 상병조회 </summary>
        public frmILLHelp()
        {
            InitializeComponent();
        }

        private void frmILLHelp_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            cboViewClass.Items.Clear();
            cboViewClass.Items.Add("1.국제4단분류");
            cboViewClass.Items.Add("2.상해외인코드");
            cboViewClass.Items.Add("3.국제수술코드");
            cboViewClass.Items.Add("4.User Define");
            cboViewClass.Items.Add("5.기록실수술코드");
            cboViewClass.SelectedIndex = 0;

            txtSearch.Text = "";
            optPart1.Checked = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //TODO: DB 조회중 ORA-29275: 부분 다중 바이트 문자 에러발생
        private void btnView_Click(object sender, EventArgs e)
        {
            int i = 0;
            string strSearch = "";
            string SQL = string.Empty;      // "" 과의 차이점은 무엇?
            string SqlErr = string.Empty;
            DataTable dt = null;

            ssSangbyeong_Sheet1.RowCount = 0;

            strSearch = txtSearch.Text.Trim();

            if (strSearch == "")
            {
                if (ComFunc.MsgBoxQ("전체 사병이 조회 됩니다. 시간이많이 걸립니다. 그래도 조회 하시겠습니까?", "확인") == DialogResult.No)
                {
                    return;
                }
            }
            
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT IllCode, IllClass, IllNameK, IllNameE, DispHeader";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_ILLS";
                SQL = SQL + ComNum.VBLF + " WHERE IllClass = '" + VB.Left(cboViewClass.Text, 1) + "' ";

                if (optSort0.Checked == true)
                {
                    if (txtSearch.Text != "")
                    {
                        SQL = SQL + ComNum.VBLF + "   AND ILLCODE LIKE '%" + strSearch + "%' ";
                    }
                    SQL = SQL + ComNum.VBLF + " ORDER BY IllCode ";
                }
                else if (optSort1.Checked == true)
                {
                    if (txtSearch.Text != "")
                    {
                        SQL = SQL + ComNum.VBLF + "   AND ILLNameK LIKE '%" + strSearch + "%' ";
                    }
                    SQL = SQL + ComNum.VBLF + " ORDER BY IllNameK,IllCode ";
                }
                else
                {
                    if (txtSearch.Text != "")
                    {
                        SQL = SQL + ComNum.VBLF + " AND ILLNameK LIKE '%" + strSearch + "%' ";
                    }
                    SQL = SQL + ComNum.VBLF + "ORDER BY IllNameE,IllCode ";
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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssSangbyeong_Sheet1.RowCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssSangbyeong_Sheet1.Cells[i, 0].Text = dt.Rows[i]["IllCode"].ToString().Trim();
                    ssSangbyeong_Sheet1.Cells[i, 1].Text = dt.Rows[i]["IllClass"].ToString().Trim();
                    ssSangbyeong_Sheet1.Cells[i, 2].Text = dt.Rows[i]["IllNameK"].ToString().Trim();
                    ssSangbyeong_Sheet1.Cells[i, 3].Text = dt.Rows[i]["IllNameE"].ToString().Trim();
                    ssSangbyeong_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DispHeader"].ToString().Trim();
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
            }
        }

        //TODO:FileOpen 및 고속print
        private void btnPrint2_Click(object sender, EventArgs e)
        {
            int i = 0;
            int k = 0;
            int intFile = 0;
            int intPage = 0;
            int intRow = 0;
            int intPart = 0;
            string strCode = "";
            string[,] strString = new string[5, 40];

            Cursor.Current = Cursors.WaitCursor;

            //intFile = FreeFile()
            //ChDir("C:\")
            //Open "c:\illcode1.lst" For Output As #10

            CmdPrint3Head(ref intFile, ref intPart, ref strString);

            ssSangbyeong_Sheet1.ColumnCount = 1;
            ssSangbyeong_Sheet1.RowCount = 1;
            strCode = ssSangbyeong.Text;
            intPart = 1;
            intRow = 1;

            for (k = 0; k < 40; k++)
            {
                strString[1, k] = "";
                strString[2, k] = "";
                strString[3, k] = "";
                strString[4, k] = "";
            }

            for (i = 1; i < ssSangbyeong_Sheet1.RowCount; i++)
            {
                if (VB.Mid(strCode, 2, 2) != VB.Mid(ssSangbyeong_Sheet1.Cells[i, 0].Text.Trim(), 2, 2))
                {
                    strCode = ssSangbyeong_Sheet1.Cells[i, 0].Text.Trim();
                    if (!(intRow == 1 || intRow == 39))
                    {
                        strString[intPart * 2 - 1, intRow] = "";
                        strString[intPart * 2, intRow] = "";        //String$(25, "-")
                        intRow = intRow + 1;
                        if (intPart == 1 && intRow >= 39)
                        {
                            intRow = 1;
                            intPart = 2;
                        }
                        if (intPart == 2 && intRow >= 39)
                        {
                            intRow = 1;
                            intPart = 1;
                            PrtWrite(ref intFile, ref intPage, ref strString);
                        }
                    }
                }

                strString[intPart + 2 - 1, intRow] = ssSangbyeong_Sheet1.Cells[i, 0].Text.Trim();
                strString[intPart * 2, intRow] = ssSangbyeong_Sheet1.Cells[i, 2].Text.Trim();
                intRow = intRow + 1;
                if (intPart == 1 && intRow >= 39)
                {
                    intRow = 1;
                    intPart = 2;
                }
                if (intPart == 2 && intRow >= 39)
                {
                    intRow = 1;
                    intPart = 1;
                    PrtWrite(ref intRow, ref intPart, ref strString);
                }
            }
            //마지막 라인 인쇄
            PrtWrite(ref intFile, ref intPage, ref strString);

            //Close #10

            if (optPart1.Checked == true)
            {
                //Close #11
            }

            //Open "c:\illcode.lod" For Output As #nFile
            //Print #nFile, "open 192.9.200.1"
            //Print #nFile, "user oracle7 oracle7"
            //Print #nFile, "prompt"
            //Print #nFile, "cd ../pmpa/lst"
            //Print #nFile, "put c:\illcode1.lst"
            if (optPart1.Checked == true)
            {
                //Print #nFile, "put c:\illcode2.lst";
            }
            //Print #nFile, "bye"
            //Close #nFile

            //Shell "ftp -n -i -s:c:/illcode.lod", vbNormalNoFocus

            Cursor.Current = Cursors.Default;

            ComFunc.MsgBoxQ("Unix로 Login후 /users/pmpa/lst/illcode1.lst를 인쇄하세요 양면일경우는 illcode1.lst 인쇄후.다시 illcode2.lst 인쇄하세요", "확인");
        }

        //TODO:고속print
        private void PrtWrite(ref int intFile, ref int intPage, ref string[,] strString)
        {
            int k = 0;

            if (optPart1.Checked == true)
            {
                if (intPage % 2 == 1)
                {
                    intFile = 10;
                }
                else
                {
                    intFile = 11;
                }
            }
            else
            {
                intFile = 10;
            }

            for (k = 1; k < 40; k++)
            {
                //Print #nFile, LeftH(strString(1, K) & Space$(7), 7);        //상병코드
                //Print #nFile, LeftH(strString(2, K) & Space$(35), 35);      //한글명칭
                //Print #nFile, " ";
                //Print #nFile, LeftH(strString(3, K) & Space$(7), 7);         //상병코드
                //Print #nFile, LeftH(strString(4, K) & Space$(35), 35)        //한글명칭

            }

            for (k = 0; k < 40; k++)
            {

                strString[1, k] = "";
                strString[2, k] = "";
                strString[3, k] = "";
                strString[4, k] = "";
            }


            CmdPrint3Head(ref intFile, ref intPage, ref strString);
            return;
        }

        //TODO:고속print
        private void CmdPrint3Head(ref int intFile, ref int intPage, ref string[,] strString) ////Head Print
        {

            if (intPage != 0)
            {
                //Print #nFile, Chr$(12);
            }
            intPage = intPage + 1;

            if (optPart1.Checked == true)
            {
                if (intPage % 2 == 1)
                {
                    intFile = 10;
                }
                else
                {
                    intFile = 11;
                }
            }
            else
            {
                intFile = 10;
            }

            //Print #nFile, "인쇄일자 : "; GstrSysDate; " "; GstrSysTime; Space$(43);
            //Print #nFile, "Page : "; Format$(nPage, "0000")
            //Print #nFile, String$(82, "=")
            //Print #nFile, "상병코드   상  병  명  칭";
            //'Print #nFile, "#041";

            //Print #nFile, "상병코드   상  병  명  칭"
            //Print #nFile, String$(82, "=")

            return;
        }

        private void ssSangbyeong_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row == 0 || e.Column == 0)
            {
                return;
            }

            GstrRetValue = ssSangbyeong_Sheet1.Cells[e.Row, 0].Text;
            GstrHelpCode = ssSangbyeong_Sheet1.Cells[e.Row, 2].Text;

            this.Close();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnView.Focus();
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";
            string strCurDateTime = "";

            strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");

            Cursor.Current = Cursors.WaitCursor;

            strFont1 = "/fn\"굴림체\" /fz\"15\"";
            strHead1 = "/c/n" + "상   병   코   드 LIST" + "/n";
            strFont2 = "/fn\"굴림체\" /fz\"11\"";
            strHead2 = "/l/f2" + "출력일자 : " + ComFunc.FormatStrToDateEx(VB.Left(strCurDateTime, 8), "D", "-") + " " + ComFunc.FormatStrToDateEx(VB.Right(strCurDateTime, 6), "T", ":") + VB.Space(125) + "PAGE:" + "/p";

            ssSangbyeong_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2;
            ssSangbyeong_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssSangbyeong_Sheet1.PrintInfo.Margin.Top = 30;
            ssSangbyeong_Sheet1.PrintInfo.Margin.Bottom = 10;
            ssSangbyeong_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssSangbyeong_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssSangbyeong_Sheet1.PrintInfo.ShowBorder = true;
            ssSangbyeong_Sheet1.PrintInfo.ShowColor = false;
            ssSangbyeong_Sheet1.PrintInfo.ShowGrid = true;
            ssSangbyeong_Sheet1.PrintInfo.ShowShadows = true;
            ssSangbyeong_Sheet1.PrintInfo.UseMax = true;
            ssSangbyeong_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssSangbyeong.PrintSheet(0);

            Cursor.Current = Cursors.Default;
        }
    }
}
