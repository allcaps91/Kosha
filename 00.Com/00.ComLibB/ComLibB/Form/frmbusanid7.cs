using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmbusanid7 : Form
    {
        /// Class Name      : ComLibB.dll
        /// File Name       : frmbusanid7.cs
        /// Description     : 수가코드 조회 및 코드집 인쇄
        /// Author          : 김효성
        /// Create Date     : 2017-06-27
        /// Update History  : 
        /// </summary>
        /// <history>  
        /// VB\basic\busuga\Busuga16.frm => frmbusanid7.cs 으로 변경함
        /// </history>
        /// <seealso> 
        /// VB\basic\busuga\Busuga16.frm(Busuga16.frm)
        /// </seealso>
        /// <vbp>
        /// default : VB\basic\busuga\busuga.vbp
        /// </vbp>
        ///         
        DateTime mdtp;
        frmSearchSugaSQL frmSearchSugaSQLX;

        string GgstrHelpCode = "";
        string GstrSQL = "";
        int GFnOldRow = 0;
        

        public frmbusanid7 ()
        {
            InitializeComponent ();
        }

        public frmbusanid7 (string FstrSQL , int FnOldRow , string GstrHelpCode)
        {
            InitializeComponent ();
            GgstrHelpCode = GstrHelpCode;
            GstrSQL = FstrSQL;
            GFnOldRow = FnOldRow;
        }

        private void ScreenClear ()
        {
            ssView_Sheet1.Rows.Count = 50;
            if (GFnOldRow != 0) ssView_Sheet1.RowHeader.Rows [GFnOldRow].BackColor = Color.FromArgb (255,255,255);
            GFnOldRow = 0;

            btnPrintJob.Enabled = false;
            btnDOSPrint.Enabled = false;
            btnDePastPrint.Enabled = false;
        }

        private void frmbusanid7_Load (object sender , EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            string SQL = "";
            ScreenClear ();
            SQL = "";
            mdtp = Convert.ToDateTime(ComFunc.FormatStrToDate (ComQuery.CurrentDateTime (clsDB.DbCon, "A") , "A"));
        }

        private void btnPrintJob_Click (object sender , EventArgs e)
        {
            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            if (ComQuery.IsJobAuth(this , "P", clsDB.DbCon) == false) return;//권한 확인

            Cursor.Current = Cursors.WaitCursor;

            strFont1 = "/fn\"굴림체\" /fz\"16\" /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"10\" /fs2";
            strHead1 = "/l/f1" + VB.Space (40) + "수  가   코  드  집" + "/n";
            strHead2 = "/l/f2" + "인쇄일자 : " + mdtp.ToString ("yyyy-MM-dd HH:mm");
            strHead2 = strHead2 + VB.Space (130) + "PAGE : /p";

            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Top = 50;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 50;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;

            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = true;
            ssView_Sheet1.PrintInfo.ShowColor = true;
            ssView_Sheet1.PrintInfo.ShowColor = true;
            ssView_Sheet1.PrintInfo.ShowGrid = false;
            ssView_Sheet1.PrintInfo.PrintShapes = false;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet (0);

            Cursor.Current = Cursors.Default;
        }

        private void btnExit_Click (object sender , EventArgs e)
        {
            this.Close ();
        }

        private void btnPastPrintLineCount () //CmdPrint3_Line_Count
        {

        }

        private void btnPastPrint () //CmdPrint3_Head
        {

        }

        private void btnPastPrint_Click (object sender , EventArgs e)
        {

        }

        private void ssView_CellDoubleClick (object sender , FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strSuCode = "";
            string strSuNext = "";
            string strBCode = "";
            string strSugbL = "";

            if (GFnOldRow != 0) ssView_Sheet1.RowHeader.Rows [GFnOldRow].BackColor = Color.FromArgb (255 , 255 , 255);
            if (e.Column != 0 && e.Column != 1 && e.Column != 3 && e.Column != 8) return;

            ssView_Sheet1.RowHeader.Rows [e.Row].BackColor = Color.FromArgb (129 , 252 , 224);
            GFnOldRow = e.Row;

            strSuCode = ssView_Sheet1.Cells [e.Row , 0].Text.Trim ();
            strSuNext = ssView_Sheet1.Cells [e.Row , 1].Text.Trim ();
            strBCode = ssView_Sheet1.Cells [e.Row , 18].Text.Trim ();
            strSugbL = VB.Mid (ssView_Sheet1.Cells [e.Row , 4].Text.Trim () , 12 , 1);

            switch (e.Column)
            {
                case 0:
                    GgstrHelpCode = strSuCode;
                    frmSugaEntry frmSugaEntryX = new frmSugaEntry(GgstrHelpCode);
                    frmSugaEntryX.ShowDialog();                    
                    break;
                case 1:
                    if (strBCode == "JJJJJJ")
                    {
                        GgstrHelpCode = strSuNext;
                        frmJunCodeEntry frm = new frmJunCodeEntry (GgstrHelpCode);                        
                        frm.ShowDialog();                        
                    }
                    else if (!(strBCode == "000000" || strBCode == "999999" || strBCode == "AAAAAA" || strBCode == ""))
                    {
                        GgstrHelpCode = strSuNext;
                        frmSearchBCode frm = new frmSearchBCode (GgstrHelpCode);                        
                        frm.ShowDialog();                        
                    }
                    else
                    {
                        ComFunc.MsgBox("표준코드가 등록 안됨", "확인");
                    }
                    break;
                case 3:
                    if (!(strBCode == "000000" || strBCode == "999999" || strBCode == "AAAAAA" || strBCode == ""))
                    {
                        if (strSugbL == "4" || strSugbL == "8")
                        {
                            GgstrHelpCode = strBCode;
                            frmSearchGuip frm = new frmSearchGuip(GgstrHelpCode);
                            frm.ShowDialog();
                        }
                        else
                        {
                            ComFunc.MsgBox("구입신고 표준수가가 아닙니다", "확인");
                        }
                    }
                    else
                    {
                        ComFunc.MsgBox ("구입신고 표준수가가 아닙니다" , "확인");
                    }
                    break;
                case 8:
                    if (!(strBCode == "000000" || strBCode == "999999" || strBCode == "AAAAAA" || strBCode == ""))
                    {
                        if (strSugbL == "3")
                        {
                            GgstrHelpCode = strBCode;
                            frmYGuipView frm = new frmYGuipView(GgstrHelpCode, clsType.User.Sabun);
                            frm.ShowDialog();
                        }
                        else
                        {
                            ComFunc.MsgBox("약품 실구입가 신고 대상이 아닙니다", "확인");
                        }
                    }
                    else
                    {
                        ComFunc.MsgBox("약품 실구입가 신고 대상이 아닙니다", "확인");
                    }
                    break;
            }
            GgstrHelpCode = "";
        }

        private void Search ()
        {
            if (ComQuery.IsJobAuth(this , "R", clsDB.DbCon) == false) return;//권한 확인

            ScreenClear ();

            if (frmSearchSugaSQLX != null)
            {
                frmSearchSugaSQLX.Dispose ();
                frmSearchSugaSQLX = null;
            }

            frmSearchSugaSQLX = new frmSearchSugaSQL ();
            frmSearchSugaSQLX.rSetSuGaCodeSQL += Frm_rSetSuGaCodeSQL;
            frmSearchSugaSQLX.rEventClosed += Frm_rEventClosed;
            frmSearchSugaSQLX.ShowDialog ();

        }

        private void Frm_rEventClosed ()
        {
            frmSearchSugaSQLX.Dispose ();
            frmSearchSugaSQLX = null;
        }

        private void Frm_rSetSuGaCodeSQL (string SQL)
        {
            int i = 0;
            int j = 0;
            int nRow = 0;
            string strData = "";
            string strNewData = "";
            string strOldData = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            if (SQL == "")
            {
                ComFunc.MsgBox ("검색 조건이 지정 안 됨" , "취소");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);
                SQL = "";

                if (dt.Rows.Count == 0)
                {

                    dt.Dispose ();
                    dt = null;
                    ComFunc.MsgBox ("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssView.Enabled = true;

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight (-1 , ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (i > ssView_Sheet1.Rows.Count) ssView_Sheet1.Rows.Count = i + 10;
                    strNewData = dt.Rows [i] ["SuCode"].ToString ().Trim ();
                    if (strOldData != strNewData)
                    {
                        ssView_Sheet1.Cells [i , 0].Text = dt.Rows [i] ["SuCode"].ToString ().Trim ();
                        strOldData = strNewData;
                    }
                    ssView_Sheet1.Cells [i , 1].Text = dt.Rows [i] ["SuNext"].ToString ().Trim ();
                    ssView_Sheet1.Cells [i , 2].Text = dt.Rows [i] ["Gbn"].ToString ().Trim ();
                    ssView_Sheet1.Cells [i , 3].Text = dt.Rows [i] ["Bun"].ToString ().Trim () + "," + dt.Rows [i] ["Nu"].ToString ().Trim ();
                    strData = dt.Rows [i] ["SugbA"].ToString ().Trim ();
                    strData = strData + dt.Rows [i] ["SugbB"].ToString ().Trim ();
                    strData = strData + dt.Rows [i] ["SugbC"].ToString ().Trim ();
                    strData = strData + dt.Rows [i] ["SugbD"].ToString ().Trim ();
                    strData = strData + dt.Rows [i] ["SugbE"].ToString ().Trim ();
                    strData = strData + dt.Rows [i] ["SugbF"].ToString ().Trim ();
                    strData = strData + dt.Rows [i] ["SugbG"].ToString ().Trim ();
                    strData = strData + dt.Rows [i] ["SugbH"].ToString ().Trim ();
                    strData = strData + dt.Rows [i] ["SugbI"].ToString ().Trim ();
                    strData = strData + dt.Rows [i] ["SugbJ"].ToString ().Trim ();
                    strData = strData + dt.Rows [i] ["SugbK"].ToString ().Trim ();
                    strData = strData + dt.Rows [i] ["SugbL"].ToString ().Trim ();
                    strData = strData + dt.Rows [i] ["SugbM"].ToString ().Trim ();
                    strData = strData + dt.Rows [i] ["SugbN"].ToString ().Trim ();
                    strData = strData + dt.Rows [i] ["SugbO"].ToString ().Trim ();
                    strData = strData + dt.Rows [i] ["SugbP"].ToString ().Trim ();
                    strData = strData + dt.Rows [i] ["SugbQ"].ToString ().Trim ();
                    strData = strData + dt.Rows [i] ["SugbR"].ToString ().Trim ();
                    strData = strData + dt.Rows [i] ["SugbS"].ToString ().Trim ();
                    strData = strData + dt.Rows [i] ["SugbT"].ToString ().Trim ();
                    strData = strData + dt.Rows [i] ["SugbU"].ToString ().Trim ();
                    strData = strData + dt.Rows [i] ["SugbV"].ToString ().Trim ();

                    ssView_Sheet1.Cells [i , 4].Text = VB.Left (strData + VB.Space (22) , 22);

                    if (dt.Rows [i] ["Gbn"].ToString ().Trim () == "H")
                    {
                        ssView_Sheet1.Cells [i , 5].Text = dt.Rows [i] ["SugbSS"].ToString ().Trim ();
                        ssView_Sheet1.Cells [i , 6].Text = dt.Rows [i] ["SugbBi"].ToString ().Trim ();
                        ssView_Sheet1.Cells [i , 7].Text = dt.Rows [i] ["SuQty"].ToString ().Trim ();
                    }

                    ssView_Sheet1.Cells [i , 8].Text = dt.Rows [i] ["SunameK"].ToString ().Trim ();
                    ssView_Sheet1.Cells [i , 9].Text = VB.Format (VB.Val (dt.Rows [i] ["BAmt"].ToString ().Trim ()) , "#######0");
                    ssView_Sheet1.Cells [i , 10].Text = VB.Format (VB.Val (dt.Rows [i] ["TAmt"].ToString ().Trim ()) , "#######0");
                    ssView_Sheet1.Cells [i , 11].Text = VB.Format (VB.Val (dt.Rows [i] ["IAmt"].ToString ().Trim ()) , "#######0");
                    ssView_Sheet1.Cells [i , 12].Text = dt.Rows [i] ["SuDate"].ToString ().Trim ();
                    ssView_Sheet1.Cells [i , 13].Text = VB.Format (VB.Val (dt.Rows [i] ["OldBAmt"].ToString ().Trim ()) , "#######0");
                    ssView_Sheet1.Cells [i , 14].Text = VB.Format (VB.Val (dt.Rows [i] ["OldTAmt"].ToString ().Trim ()) , "#######0");
                    ssView_Sheet1.Cells [i , 15].Text = VB.Format (VB.Val (dt.Rows [i] ["OldIAmt"].ToString ().Trim ()) , "#######0");
                    ssView_Sheet1.Cells [i , 16].Text = dt.Rows [i] ["Unit"].ToString ().Trim ();
                    ssView_Sheet1.Cells [i , 17].Text = dt.Rows [i] ["HCode"].ToString ().Trim ();
                    ssView_Sheet1.Cells [i , 18].Text = dt.Rows [i] ["BCode"].ToString ().Trim ();
                }
                //ssView_Sheet1.Rows.Count = nRow;

                dt.Dispose ();
                dt = null;
                Cursor.Current = Cursors.Default;

                btnPrintJob.Enabled = true;
                btnDOSPrint.Enabled = true;

                if (clsType.User.Sabun == "4349") btnDePastPrint.Enabled = true;
                return;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void btnSearch_Click (object sender , EventArgs e)
        {
            Search ();
        }

        private int btnPrint2Gead (ref int nLine)//CmdPrint2_Head
        {
            nLine = nLine + 1;

            if (nLine > 48)
            {
                nLine = 1;
                btnPrint2Head (ref nLine);
            }
            
            return nLine;
        }

        private void btnPrint2Head(ref int nPage)
        {
            PrintDocument RcpPrint = new PrintDocument ();
            PrintController printController = new StandardPrintController ();
            RcpPrint.PrintController = printController;  //기본인쇄창 없애기

            RcpPrint.Print ();

            PageSettings ps = new PageSettings ();
            ps.PrinterSettings.PrinterName = "인쇄 작업";
            ps.Margins = new Margins (10 , 10 , 10 , 10);
            ps.Landscape = true;
            RcpPrint.DefaultPageSettings = ps;
            RcpPrint.PrinterSettings.PrinterName = "인쇄 작업";

            nPage = nPage + 1;

            RcpPrint.PrintPage += new PrintPageEventHandler (RcpPrint_PrintPage1);



            RcpPrint.Print ();
        }

        private void RcpPrint_PrintPage1 (object sender , PrintPageEventArgs e)
        {
            e.Graphics.DrawString ("수 가   코 드    L I  S T" , new Font ("굴림" , 9 ,FontStyle.Regular) , Brushes.Black , 5 , 5);
            e.Graphics.DrawString ("===============================" , new Font ("굴림" , 9 , FontStyle.Regular) , Brushes.Black , 5 ,6);
            e.Graphics.DrawString ("인쇄일자 : " + mdtp.ToString ("yyyy-MM-dd") + mdtp.ToString ("mm-DD") , new Font ("굴림" , 9 , FontStyle.Regular) , Brushes.Black , 5 , 7);
            e.Graphics.DrawString ("===============================" , new Font ("굴림" , 9 , FontStyle.Regular) , Brushes.Black , 5 , 8);
            e.Graphics.DrawString ("수가코드 BUN NU ABCDEFGHIJKLMN QTY " , new Font ("굴림" , 9 , FontStyle.Regular) , Brushes.Black , 5 , 9);
            e.Graphics.DrawString ("보험수가 일반수가 자보수가 변경일자 종전보험 종전일반 종전자보 SUNEXT   " , new Font ("굴림체" , 9 , FontStyle.Regular) , Brushes.Black , 5 , 10);
            e.Graphics.DrawString ("        수        가       명" , new Font ("굴림" , 9 , FontStyle.Regular) , Brushes.Black , 5 , 11);
            e.Graphics.DrawString ("===============================" , new Font ("굴림" , 9 , FontStyle.Regular) , Brushes.Black , 5 , 12);
        }

        private void btnDOSPrint_Click (object sender , EventArgs e)
        {
            int i = 0;
            int nFILE = 0;
            int nLine = 0;
            int nPage = 0;

            if (ComQuery.IsJobAuth(this , "P", clsDB.DbCon) == false) return;//권한 확인

            Cursor.Current = Cursors.WaitCursor;

            nFILE = FileSystem.FreeFile ();
            FileSystem.FileOpen(nFILE, "PRN", OpenMode.Output);

            //CmdPrint2_Head
            //If nPage <> 0 Then Print #nFILE, String$(150, "="): Print #nFILE, Chr$(12)
            //
            if (nPage != 0)
            {
                FileSystem.WriteLine(nFILE, Strings.StrDup(150, '='));
                FileSystem.WriteLine(nFILE, VB.Chr(12));
            }
            nPage = nPage + 1;

            FileSystem.WriteLine(nFILE, VB.Space(53) + "수   가   코   드    L  I  S  T");
            FileSystem.WriteLine(nFILE, VB.Space(53) + "===============================");
            FileSystem.WriteLine(nFILE, "인쇄일자 : " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + VB.Space(99));
            FileSystem.WriteLine(nFILE, "Page : " + VB.Format(nPage, "0000"));
            FileSystem.WriteLine(nFILE, Strings.StrDup(150, "="));
            FileSystem.WriteLine(nFILE, "수가코드 BUN NU ABCDEFGHIJKLMN QTY ");
            FileSystem.WriteLine(nFILE, "보험수가 일반수가 자보수가 변경일자 종전보험 종전일반 종전자보 SUNEXT   ");
            FileSystem.WriteLine(nFILE, "        수        가       명");
            FileSystem.WriteLine(nFILE, Strings.StrDup(150, "="));

            for (i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                //CmdPrint2_Line_Count

                nLine = nLine + 1;
                if(nLine > 48)
                {

                    if (nPage != 0)
                    {
                        FileSystem.WriteLine(nFILE, Strings.StrDup(150, '='));
                        FileSystem.WriteLine(nFILE, VB.Chr(12));
                    }
                    nPage = nPage + 1;

                    FileSystem.WriteLine(nFILE, VB.Space(53) + "수   가   코   드    L  I  S  T");
                    FileSystem.WriteLine(nFILE, VB.Space(53) + "===============================");
                    FileSystem.WriteLine(nFILE, "인쇄일자 : " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + VB.Space(99));
                    FileSystem.WriteLine(nFILE, "Page : " + VB.Format(nPage, "0000"));
                    FileSystem.WriteLine(nFILE, Strings.StrDup(150, "="));
                    FileSystem.WriteLine(nFILE, "수가코드 BUN NU ABCDEFGHIJKLMN QTY ");
                    FileSystem.WriteLine(nFILE, "보험수가 일반수가 자보수가 변경일자 종전보험 종전일반 종전자보 SUNEXT   ");
                    FileSystem.WriteLine(nFILE, "        수        가       명");
                    FileSystem.WriteLine(nFILE, Strings.StrDup(150, "="));

                    nLine = 1;
                }

                //CmdPrint2_Line_Count


                FileSystem.WriteLine(nFILE, VB.Left(ssView_Sheet1.Cells[i, 0].Text + VB.Space(9), 9));

                FileSystem.WriteLine(nFILE, VB.Left(ssView_Sheet1.Cells[i, 3].Text + VB.Space(2), 2) + "  "); //'분류
                FileSystem.WriteLine(nFILE, VB.Right(VB.Space(2) + ssView_Sheet1.Cells[i, 3].Text, 2) + " "); //'누적
                FileSystem.WriteLine(nFILE, VB.Left(ssView_Sheet1.Cells[i, 4].Text + VB.Space(14), 14));      //'A-N항
                FileSystem.WriteLine(nFILE, VB.Right(VB.Space(4) + VB.Format(VB.Val(ssView_Sheet1.Cells[i, 7].Text), "###"), 4)); //'수량
                FileSystem.WriteLine(nFILE, VB.Right(VB.Space(9) + VB.Format(VB.Val(ssView_Sheet1.Cells[i, 9].Text), "########0"), 9)); //'보험수가
                FileSystem.WriteLine(nFILE, VB.Right(VB.Space(9) + VB.Format(VB.Val(ssView_Sheet1.Cells[i, 10].Text), "########0"), 9)); //'자보수가
                FileSystem.WriteLine(nFILE, VB.Right(VB.Space(9) + VB.Format(VB.Val(ssView_Sheet1.Cells[i, 11].Text), "########0"), 9)); //'일반수가
                 FileSystem.WriteLine(nFILE,  " " + VB.Right(VB.Space(8) + ssView_Sheet1.Cells[i, 12].Text, 8)); //'변경일자
                 FileSystem.WriteLine(nFILE,  VB.Right(VB.Space(9) + VB.Format(VB.Val(ssView_Sheet1.Cells[i, 13].Text), "########0"), 9)); //'Old보험수가
                FileSystem.WriteLine(nFILE,  VB.Right(VB.Space(9) + VB.Format(VB.Val(ssView_Sheet1.Cells[i, 14].Text), "########0"), 9)); //'Old자보수가
                FileSystem.WriteLine(nFILE, VB.Right(VB.Space(9) + VB.Format(VB.Val(ssView_Sheet1.Cells[i, 15].Text), "########0"), 9)); //'Old일반수가
                 FileSystem.WriteLine(nFILE,  " " + VB.Left(ssView_Sheet1.Cells[i, 1].Text + VB.Space(9), 9)); //'품명코드
                 FileSystem.WriteLine(nFILE,  ssView_Sheet1.Cells[i, 8].Text.Trim());                 //'수가명칭

            }

            FileSystem.WriteLine(nFILE, Strings.StrDup(150, '='));
            FileSystem.WriteLine(nFILE, VB.Chr(12));
            FileSystem.FileClose();

            
            //Open "PRN" For Output As #nFILE

            btnPrint2Head (ref nLine);


            //Cursor.Current = Cursors.Default;

        }

        private void btnDePastPrint_Click (object sender , EventArgs e)
        {

        }
    }
}
