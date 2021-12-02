using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스


namespace ComPmpaLibB
{
    public partial class frmPmpaPoscoTax : Form
    {
        clsSpread cSpd = new clsSpread();
        clsComPmpaSpd cPmpaSpd = new clsComPmpaSpd();
        ComFunc CF = new ComFunc();
        clsPmpaFunc cPF = new clsPmpaFunc();
        int FnGbn = 0;
        public frmPmpaPoscoTax()
        {
            InitializeComponent();
            SetEvent();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);

            this.CmdExit.Click += new EventHandler(eBtnClick);
            this.CmdView.Click += new EventHandler(eBtnClick);
            this.CmdPrint.Click += new EventHandler(eBtnClick);
        }
        private void eFormLoad(object sender, EventArgs e)
        {
            int i = 0;
            int nYY = 0;
            int nMM = 0;
            string strBDate = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            FnGbn = 0;

            

            nYY = Convert.ToInt32(VB.Left(clsPublic.GstrSysDate, 4));
            nMM = Convert.ToInt32(VB.Mid(clsPublic.GstrSysDate, 6, 2));


            ComboYYMM.Items.Clear();

            for (i = 1; i <= 12; i++)
            {
                ComboYYMM.Items.Add(ComFunc.SetAutoZero(nYY.ToString(), 4) + "년" + ComFunc.SetAutoZero(nMM.ToString(), 2) + "월");
                //  cboYYMM.Items.Add(ComFunc.SetAutoZero(nYY.ToString(), 4) + "-" + ComFunc.SetAutoZero(nMM.ToString(), 2));
                nMM -= 1;
                if (nMM == 0)
                {
                    nYY -= 1;
                    nMM = 12;
                }
            }

            ComboYYMM.SelectedIndex = 1;

            strBDate = VB.Replace(VB.Replace(VB.Replace(ComboYYMM.Text, " ", ""), "년", "-"), "월", "-") + "01";

            READ_EXAM_AMT_SET(strBDate, FnGbn);

        }
        private void READ_EXAM_AMT_SET( string ArgDate, int  argGubun)
        {
            string rtnVal = string.Empty;

            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";
            string strCode1 = "";
            string strCode2 = "";
            int i = 0;
            int inRow = 0; 
            SS1_Sheet1.Cells[83, 7].Text = "";

            try
            {
                for (i = 8; i < 71; i++)
                {
                    inRow = i;
                    strCode1 = SS1_Sheet1.Cells[inRow, 9].Text;
                    strCode2 = SS1_Sheet1.Cells[inRow, 10].Text;

                    SQL = "";
                    //명칭
                    SQL += ComNum.VBLF + " SELECT AMT FROM " + ComNum.DB_PMPA + "ETC_BCODE ";
                    SQL += ComNum.VBLF + "  WHERE GUBUN = '" + strCode1 + "' ";
                    SQL += ComNum.VBLF + "    AND CODE = '" + strCode2 + "' ";
                    SQL += ComNum.VBLF + "    AND JDate <= TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "     ORDER BY JDATE DESC  ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                    if (Dt.Rows.Count > 0)
                    {
                        SS1_Sheet1.Cells[inRow , 5].Text = VB.Val(Dt.Rows[0]["AMT"].ToString().Trim()).ToString("###,###,###");
                    }


                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    }
                    Dt.Dispose();
                    Dt = null;
                }

             
            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
               
            }

        }



        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.CmdExit)             //닫기
            {
                this.Close();
            }
            else if (sender == this.CmdView)      //조회
            {
                Screen_Display();
            }

            else if (sender == this.CmdPrint)      
            {
                 ePrint();
            }
      
            


        }

        private void ePrint()
        {
           
            Cursor.Current = Cursors.WaitCursor;


            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";
            string strBun = "";

            DateTime mdtp;
            mdtp = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A"));

            SS1_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            SS1_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            //ssView_Sheet1 . PrintInfo . Orientation = FarPoint . Win . Spread . PrintOrientation . Landscape;
            SS1_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            SS1_Sheet1.PrintInfo.Margin.Top = 40;
            SS1_Sheet1.PrintInfo.Margin.Bottom = 60;
            SS1_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            SS1_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            SS1_Sheet1.PrintInfo.ShowBorder = true;
            SS1_Sheet1.PrintInfo.ShowColor = false;
            SS1_Sheet1.PrintInfo.ShowGrid = true;
            SS1_Sheet1.PrintInfo.ShowShadows = true;
            SS1_Sheet1.PrintInfo.UseMax = false;
            SS1_Sheet1.PrintInfo.PrintType = 0;
            SS1.PrintSheet(0);
           

            Cursor.Current = Cursors.Default;
        }

            private void Screen_Display()
        {
            DataTable Dt = null;
            DataTable Dt1 = null;
            DataTable Dt2 = null;
            string SQL = "";
            string SqlErr = "";

            int i = 0 , j = 0, nRead = 0, nRead1 = 0 , nRead2 = 0;
            long nAmt1 = 0, nCNT = 0, nCnt2 = 0;
            String strBDate = "", strEdate = "", strCode1 = "", strCode2 = "" ;
            Double nSum =  0;
            strBDate = strBDate = VB.Replace(VB.Replace(VB.Replace(ComboYYMM.Text, " ", ""), "년", "-"), "월", "-") + "01";
            strEdate = CF.READ_LASTDAY(clsDB.DbCon, strBDate);
            for (i = 8; i < 69; i++)
            {
                for (j = 5; j < 9; j++)
                {
                    SS1_Sheet1.Cells[i, j].Text = "";
                }

            }


            READ_EXAM_AMT_SET(strBDate, FnGbn);

            try
            {


                SQL = "";
                SQL += ComNum.VBLF + " SELECT a.GUBUN,a.CODE,SUM(a.CNT) CNT  ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ETC_BCODE_DETAIL a ";
                SQL += ComNum.VBLF + "  WHERE BDATE >=TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND BDATE <=TO_DATE('" + strEdate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND (DELDATE IS NULL OR DELDATE ='')  ";
                SQL += ComNum.VBLF + "  AND GUBUN <> '00000'  ";
                SQL += ComNum.VBLF + "  GROUP BY a.GUBUN,a.CODE  ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                nRead = Dt.Rows.Count;
                
                
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        strCode1 = Dt.Rows[i]["Gubun"].ToString().Trim() + Dt.Rows[i]["Code"].ToString().Trim();

                        if (OptPart1.Checked)
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " SELECT PART ";
                            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ETC_BCODE ";
                            SQL += ComNum.VBLF + "  WHERE GUBUN ='" + VB.Left(strCode1, 5) + "' ";
                            SQL += ComNum.VBLF + "  AND CODE ='" + VB.Right(strCode1, 5) + "' ";
                            SQL += ComNum.VBLF + "  AND PART IN ('3')  ";


                            SqlErr = clsDB.GetDataTable(ref Dt1, SQL, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }
                            nRead1 = Dt1.Rows.Count;
                            if (nRead1 > 0)
                            {
                                strCode1 = "xxx";
                            }
                            Dt1.Dispose();
                            Dt1 = null;
                        }

                        else if (OptPart2.Checked)
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " SELECT PART ";
                            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ETC_BCODE ";
                            SQL += ComNum.VBLF + "  WHERE GUBUN ='" + VB.Left(strCode1, 5) + "' ";
                            SQL += ComNum.VBLF + "  AND CODE ='" + VB.Right(strCode1, 5) + "' ";
                            SQL += ComNum.VBLF + "  AND PART IN ('1','2')  ";


                            SqlErr = clsDB.GetDataTable(ref Dt1, SQL, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }
                            nRead1 = Dt1.Rows.Count;
                            if (nRead1 > 0)
                            {
                                strCode1 = "xxx";
                            }
                            Dt1.Dispose();
                            Dt1 = null;
                        }

                        for (j = 9; j < 71; j++)
                        {
                            //strCode1 = Dt.Rows[i]["Gubun"].ToString().Trim() + Dt.Rows[i]["Code"].ToString().Trim();
                            nAmt1 = (long)VB.Val(VB.Replace(SS1_Sheet1.Cells[j -1 , 5].Text, ",", ""));
                            strCode2 = SS1_Sheet1.Cells[j - 1, 9].Text.Trim();
                            strCode2 += SS1_Sheet1.Cells[j - 1, 10].Text.Trim();

                            if ((strCode1 == strCode2 ) || (strCode2 == "0000400001" && strCode1 == "0000400002") || (strCode1 == "0000800003" && strCode2 == "0000800001") || (strCode1 == "0000800004" && strCode2 == "0000800002") )
                            {
                                nCnt2 = 0;
                                nCNT = (long)VB.Val(Dt.Rows[i]["CNT"].ToString().Trim());
                                nCnt2 = (long)VB.Val(VB.TR(SS1_Sheet1.Cells[j - 1, 6].Text,",","")) ;
                                SS1_Sheet1.Cells[j - 1, 6].Text = VB.Val((nCNT+ nCnt2).ToString().Trim()).ToString("###,###,###"); 
                                SS1_Sheet1.Cells[j - 1, 7].Text = VB.Val((nAmt1*(nCNT + nCnt2)).ToString().Trim()).ToString("###,###,###");
                                nSum = nSum + (nAmt1 * nCNT);
                            }
                        }
                    }
                }
                Dt.Dispose();
                Dt = null;
                SS1_Sheet1.Cells[82, 7].Text = VB.Val(nSum.ToString().Trim()).ToString("###,###,###"); ;
                SS1_Sheet1.Cells[85, 1].Text = "* 본 원에서는 " + VB.Left(strBDate, 4) + "." + VB.Mid(strBDate, 6, 2) + "." + VB.Right(strBDate, 2) + "~" + VB.Left(strEdate, 4) + "." + VB.Mid(strEdate, 6, 2) + "." + VB.Right(strEdate, 2) + ". " + " 까지 실시한 위탁검사자의 검사비용을 아래와 같이 청구합니다.";
                SS1_Sheet1.Cells[87, 1].Text = "청구일 : " + VB.Left(clsPublic.GstrSysDate, 4) + "년 " + VB.Mid(clsPublic.GstrSysDate, 6, 2) + "월 " + VB.Right(clsPublic.GstrSysDate, 2) + "일";
                SS1_Sheet1.Cells[89, 1].Text = "병원장 : 이  종  녀  (인)";
            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }




        }
        private void Display_ComboYYMM()
        {
            String strBDate = "";


            strBDate = strBDate = VB.Replace(VB.Replace(VB.Replace(ComboYYMM.Text, " ", ""), "년", "-"), "월", "-") + "01";

            READ_EXAM_AMT_SET(strBDate, FnGbn);
        }

        private void OptPart1_Click(object sender, EventArgs e)
        {
            FnGbn = 1;
            SS1_Sheet1.Cells[41, 10].Text = "00001";
            Display_ComboYYMM();
            Screen_Display();
        }

        private void OptPart2_Click(object sender, EventArgs e)
        {
            FnGbn = 2;
            SS1_Sheet1.Cells[41, 10].Text = "00003";
            Display_ComboYYMM();
            Screen_Display();
        }

        private void OptPart_Click(object sender, EventArgs e)
        {
            FnGbn = 0;
            SS1_Sheet1.Cells[41, 10].Text = "00001";
            Display_ComboYYMM();
            Screen_Display();
        }

        private void ComboYYMM_Click(object sender, EventArgs e)
        {
            Display_ComboYYMM();
        }
    }
}
