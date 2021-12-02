using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewHealthlivingKeepTotal.cs
    /// Description     : 건강생활 유지비 총괄표
    /// Author          : 안정수
    /// Create Date     : 2017-09-07
    /// Update History  : 2017-11-03
    /// <history>       
    /// TODO : 빌더 실제 테스트 필요
    /// d:\psmh\OPD\olrepa\Frm건강생활유지총괄표.frm(Frm건강생활유지총괄표) => frmPmpaViewHealthlivingKeepTotal.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\olrepa\Frm건강생활유지총괄표.frm(Frm건강생활유지총괄표)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewHealthlivingKeepTotal : Form
    {
        ComFunc CF = new ComFunc();
        clsPmpaFunc CPF = new clsPmpaFunc();
        string FstrGubun = "";

        int mJobSabun = 0;

        public frmPmpaViewHealthlivingKeepTotal()
        {
            InitializeComponent();
            SetEvent();
        }

        public frmPmpaViewHealthlivingKeepTotal(int GnJobSabun)
        {
            InitializeComponent();
            SetEvent();
            mJobSabun = GnJobSabun;
        }

        void SetEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnBuild.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);
            this.btnPrint2.Click += new EventHandler(eBtnEvent);

            this.tabItem1.Click += new EventHandler(eBtnEvent);
            this.tabItem2.Click += new EventHandler(eBtnEvent);

        }

        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등    

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            //두번째 탭에서 출력시 비고란 짤림 방지하기 위함
            ssList2_Sheet1.Columns[ssList2_Sheet1.Columns.Count - 1].Visible = false;

            tabControl1.SelectedTabIndex = 0;

            optGbn2.Checked = true;
            btnPrint2.Visible = false;

            CF.ComboMonth_Set(cboYYMM, 10);
            cboYYMM.SelectedIndex = 1;

            CF.ComboMonth_Set(cboYYMM2, 10);
            cboYYMM2.SelectedIndex = 1;

            Set_Remark();
        } 

        void Set_Remark()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                                    ";
            SQL += ComNum.VBLF + "  REMARK, SABUN,                                                                                                          ";
            SQL += ComNum.VBLF + "  TO_CHAR(ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE                                                                           ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BOHO_MONTHLY                                                                              ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                                 ";
            SQL += ComNum.VBLF + "      AND YYMM  = '" + VB.Left(cboYYMM2.Text.ToString(), 4) + VB.Mid(cboYYMM2.Text.ToString(), 7, 2) + "' ";
            SQL += ComNum.VBLF + "ORDER BY ENTDATE DESC,REMARK, SABUN                                                                                       ";

            try
            {
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

                if (dt.Rows.Count > 0)
                {
                    txtRemark.Text = " 작업년월 : " + VB.Left(cboYYMM2.Text.ToString(), 4) + VB.Mid(cboYYMM2.Text.ToString(), 7, 2) + "\r\n" +
                                     " 작업일자 : " + VB.Left(dt.Rows[0]["ENTDATE"].ToString().Trim(), 10) + "\r\n" +
                                     " 작업시간 : " + VB.Right(dt.Rows[0]["ENTDATE"].ToString().Trim(), 5) + "\r\n" +
                                     " 작 업 자 : " + dt.Rows[0]["SABUN"].ToString().Trim();

                    txtRemark2.Text = " " + dt.Rows[0]["REMARK"].ToString().Trim();
                }

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnView)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();
            }

            else if (sender == this.btnBuild)
            {
                btnBuild_Click();
            }

            else if (sender == this.btnPrint)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ePrint();
            }

            else if (sender == this.btnPrint2)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ssList2_Sheet1.Columns[ssList2_Sheet1.Columns.Count - 1].Visible = true;
                ePrint2();
            }

            else if (sender == this.tabItem1)
            {
                btnPrint2.Visible = false;
                this.Width = 982;

            }

            else if (sender == this.tabItem2)
            {
                this.Width = 710;
                btnPrint2.Visible = true;
            }
        }
        private void ssList1_PrintHeaderFooterArea(object sender, PrintHeaderFooterAreaEventArgs e)
        {
            Pen cPen = new Pen(Color.Black);
            cPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            cPen.Width = 3;
            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Far;

            if (e.IsHeader == true)
            {
                #region 칸 그리기
                e.Graphics.DrawRectangle(new Pen(Brushes.Black, 1), 650, 90, 400, 70);
                e.Graphics.DrawRectangle(new Pen(Brushes.Black, 1), 680, 90, 75, 70);
                e.Graphics.DrawRectangle(new Pen(Brushes.Black, 1), 680, 115, 370, 45);
                e.Graphics.DrawRectangle(new Pen(Brushes.Black, 1), 825, 90, 75, 70);
                e.Graphics.DrawRectangle(new Pen(Brushes.Black, 1), 900, 90, 75, 70);
                //  e.Graphics.DrawRectangle(new Pen(Brushes.Black, 1), 820, 90, 125, 70);

               
                
                //e.Graphics.DrawRectangle(new Pen(Brushes.Black, 1), 570, 90, 220, 70);
                //e.Graphics.DrawRectangle(new Pen(Brushes.Black, 1), 570, 90, 30, 70);
                //e.Graphics.DrawRectangle(new Pen(Brushes.Black, 1), 600, 115, 190, 45);
                //e.Graphics.DrawRectangle(new Pen(Brushes.Black, 1), 660, 90, 65, 70);



                #endregion

                #region 칸안에 글
                e.Graphics.DrawString("결", new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 675, 93, drawFormat);
                e.Graphics.DrawString("재", new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 675, 135, drawFormat);
                e.Graphics.DrawString("담  당", new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 743, 93, drawFormat);
                e.Graphics.DrawString("계  장", new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 813, 93, drawFormat);
                e.Graphics.DrawString("팀  장", new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 887, 93, drawFormat);
                e.Graphics.DrawString("행정처장", new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 973, 93, drawFormat);
                e.Graphics.DrawString("병원장", new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 1038, 93, drawFormat);

                //e.Graphics.DrawString("결", new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 625, 93, drawFormat);
                //e.Graphics.DrawString("재", new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 595, 135, drawFormat);
                //e.Graphics.DrawString("담  당", new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 657, 93, drawFormat);
                //e.Graphics.DrawString("계  장", new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 720, 93, drawFormat);
                //e.Graphics.DrawString("팀  장", new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 780, 93, drawFormat);
                //e.Graphics.DrawString("행정처장", new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 860, 93, drawFormat);
                //e.Graphics.DrawString("병원장", new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 920, 93, drawFormat);
                #endregion


                #region 작성자 
                drawFormat.Alignment = StringAlignment.Far;
               // e.Graphics.DrawString("작성자 : " + clsType.User.UserName, new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 150, 103, drawFormat);
                e.Graphics.DrawString("작업일자 : " + cboYYMM.Text.ToString().Trim(), new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 225, 123, drawFormat);
                e.Graphics.DrawString("출력시간 : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm"), new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 240, 143, drawFormat);
               // e.Graphics.DrawString("Page : " + e.PageNumber, new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 100, 163, drawFormat);
                #endregion
            }
        }
        void ePrint()
        {

            string strFont1 = "";
            string strHead1 = "";
            string strFont2 = "";
            string strHead2 = "";
            string strFont3 = "";
            string strFoot1 = "";
            string strTitle = "";
            strFont1 = "/fn\"맑은 고딕\" /fz\"16\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"맑은 고딕\" /fz\"11\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strFont3 = "/fn\"맑은 고딕\" /fz\"9\" /fb0 /fi0 /fu0 /fk0 /fs3";

            if (optGbn0.Checked == true)
            {
                strTitle = "의료급여(건강생활유지비) 총괄표";
            }

            else if (optGbn1.Checked == true)
            {
                strTitle = "의료급여(산전지원금) 총괄표";
            }

            else if (optGbn2.Checked == true)
            {
                strTitle = "의료급여(건강생활유지비) 총괄표";
            }
            clsSpread SPR = new clsSpread();

            strHead1 = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 18, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);


            //  strHead2 = "/l/f2" + "작성자 : " + clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun) + "/f2/n";
            //strHead2 += "/l/f2" + VB.Space(11) + "작업일자 : " + cboYYMM.SelectedItem.ToString().Trim() + "/f2/n";
            //strHead2 += "/l/f2" + VB.Space(11) + "출력시간 : " + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-") + "/f2/n";

            //  strFoot1 = "/r/f3" + "작성자 : " + clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun) + "/f3/n";

            ssList1_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssList1_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssList1_Sheet1.PrintInfo.Footer = strFont3 + strFoot1;
            ssList1_Sheet1.PrintInfo.Margin.Top = 50;
            ssList1_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssList1_Sheet1.PrintInfo.Margin.Left = 10;
            ssList1_Sheet1.PrintInfo.Margin.Header = 80;
            ssList1_Sheet1.PrintInfo.Margin.Footer = 10;
            ssList1_Sheet1.PrintInfo.ShowColor = false;
            ssList1_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssList1_Sheet1.PrintInfo.ShowBorder = false;
            ssList1_Sheet1.PrintInfo.ShowGrid = true;
            ssList1_Sheet1.PrintInfo.ShowShadows = false;
            ssList1_Sheet1.PrintInfo.UseMax = true;
            ssList1_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssList1_Sheet1.PrintInfo.UseSmartPrint = false;
            ssList1_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssList1_Sheet1.PrintInfo.Preview = true;
            ssList1.PrintSheet(0);



        }

        void ePrint2()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            ssList2_Sheet1.Cells[0, ssList2_Sheet1.Columns.Count - 1].Text = "zzz";
            ssList2_Sheet1.Columns[ssList2_Sheet1.Columns.Count - 1].Visible = false;


            if (FstrGubun == "1")
            {
                strTitle = "당월잔액 현황(" + cboYYMM.Text.ToString() + ")";
            }
            else if (FstrGubun == "2")
            {
                strTitle = "당월발생 현황(" + cboYYMM.Text.ToString() + ")";
            }
            else
            {
                strTitle = "당월처리 현황(" + cboYYMM.Text.ToString() + ")";
            }

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 18, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 105, 30);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, false, false, false, false);

            SPR.setSpdPrint(ssList2, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        void eGetData()
        {
            int i = 0;

            string strYYMM = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            for (i = 0; i <= 7; i++)
            {
                ssList1_Sheet1.Cells[2, i].Text = "";
            }

            strYYMM = VB.Left(cboYYMM.Text.ToString(), 4) + VB.Mid(cboYYMM.Text.ToString(), 7, 2);

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                    ";
            SQL += ComNum.VBLF + "  sum(IWOLQTY) IWOLQTY,sum(IWOLAMT) IWOLAMT,sum(BALQTY) BALQTY,sum(BALAMT) BALAMT,sum(CHULQTY) CHULQTY,   ";
            SQL += ComNum.VBLF + "  sum(CHULAMT) CHULAMT,sum(MISUQTY) MISUQTY,sum(MISUAMT) MISUAMT                                          ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BOHO_MONTHLY                                                              ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                 ";
            SQL += ComNum.VBLF + "      AND YYMM  = '" + strYYMM + "'                                                                       ";
            if (optGbn0.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND GbBun ='1'                                                                                      ";  //생활유지비만
            }
            else if (optGbn1.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND GbBun ='2'                                                                                      ";  //산적지원
            }
            else
            {
                SQL += ComNum.VBLF + "  AND GbBun in ('2','1')";
            }

            SQL += ComNum.VBLF + "      AND (GbBun,SEQNO) IN (SELECT GbBun, MAX(SEQNO) SEQNO FROM MISU_BOHO_MONTHLY                         ";
            SQL += ComNum.VBLF + "                      WHERE 1=1                                                                           ";
            SQL += ComNum.VBLF + "                          AND YYMM  = '" + strYYMM + "'                                                   ";

            if (optGbn0.Checked == true)
            {
                SQL += ComNum.VBLF + "                      AND GbBun = '1'                                                                 ";  //생활유지비만
            }
            else if (optGbn1.Checked == true)
            {
                SQL += ComNum.VBLF + "                      AND GbBun = '2'                                                                 ";  //산전지원
            }
            else
            {
                SQL += ComNum.VBLF + "                      AND GbBun  in ('2','1')                                                         ";
            }
            SQL += ComNum.VBLF + "group by GbBun                                                                                            ";
            SQL += ComNum.VBLF + "                           )                                                                              ";


            try
            {
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

                if (dt.Rows.Count > 0)
                {
                    ssList1_Sheet1.Cells[2, 0].Text = String.Format("{0:###,##0}", VB.Val(dt.Rows[0]["IWOLQTY"].ToString().Trim()));
                    ssList1_Sheet1.Cells[2, 1].Text = String.Format("{0:###,##0}", VB.Val(dt.Rows[0]["IWOLAMT"].ToString().Trim()));
                    ssList1_Sheet1.Cells[2, 2].Text = String.Format("{0:###,##0}", VB.Val(dt.Rows[0]["BALQTY"].ToString().Trim()));
                    ssList1_Sheet1.Cells[2, 3].Text = String.Format("{0:###,##0}", VB.Val(dt.Rows[0]["BALAMT"].ToString().Trim()));
                    ssList1_Sheet1.Cells[2, 4].Text = String.Format("{0:###,##0}", VB.Val(dt.Rows[0]["CHULQTY"].ToString().Trim()));
                    ssList1_Sheet1.Cells[2, 5].Text = String.Format("{0:###,##0}", VB.Val(dt.Rows[0]["CHULAMT"].ToString().Trim()));
                    ssList1_Sheet1.Cells[2, 6].Text = String.Format("{0:###,##0}", VB.Val(dt.Rows[0]["MISUQTY"].ToString().Trim()));
                    ssList1_Sheet1.Cells[2, 7].Text = String.Format("{0:###,##0}", VB.Val(dt.Rows[0]["MISUAMT"].ToString().Trim()));

                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;
        }

        void btnBuild_Click()
        {
            int i = 0;
            string strYYMM = "";
            string strYYMM2 = "";
            string strFDate = "";
            string strTDate = "";
            int nIwoLQty = 0;
            double nIwoLAmt = 0;
            int nBalQty = 0;
            double nBalAmt = 0;
            int nChulQty = 0;
            double nchulAmt = 0;
            int nMisuQty = 0;
            double nMisuAmt = 0;
            string strRemark = "";
            string strFDate2 = "";
            string strTDate2 = "";
            string strFDate3 = "";

            DataTable dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            int nSeqno = 0;

            Cursor.Current = Cursors.WaitCursor;


            //생활유지비 빌드--------------------------------------------

            strYYMM = VB.Left(cboYYMM2.Text.ToString(), 4) + VB.Mid(cboYYMM2.Text.ToString().Trim(), 7, 2);
            strYYMM2 = CPF.DATE_YYMM_ADD(strYYMM, 1);

            strFDate = VB.Left(cboYYMM2.Text.ToString(), 4) + "-" + VB.Mid(cboYYMM2.Text.ToString(), 7, 2) + "-01";
            strTDate = CF.READ_LASTDAY(clsDB.DbCon, strFDate);
            strFDate2 = VB.Left(strYYMM2, 4) + "-" + VB.Right(strYYMM2, 2) + "-01";
            strTDate2 = CF.READ_LASTDAY(clsDB.DbCon, strFDate2);

            strFDate3 = VB.Left(CPF.DATE_YYMM_ADD(strYYMM, -12), 4) + "-" + VB.Right(CPF.DATE_YYMM_ADD(strYYMM, -12), 2) + "-01";

            nSeqno = 1;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  MAX(SEQNO) SEQNO";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BOHO_MONTHLY";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND YYMM  = '" + strYYMM + "'";
            SQL += ComNum.VBLF + "      AND GbBun ='1'";    //생활유지비만            

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0 && Convert.ToInt32(VB.Val(dt.Rows[0]["SEQNO"].ToString().Trim())) > 0)
                {
                    if (MessageBox.Show("빌더한 내역이 있습니다 다시 빌더 작업을 하시겠습니까???", "작업선택", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        dt.Dispose();
                        dt = null;
                        return;
                    }
                    else
                    {
                        strRemark = VB.InputBox("다시 작업하는 이유를 간략하게 기술바랍니다. 입력하지 않으면 프로그램 종료됩니다.");
                        if (strRemark == "")
                        {
                            dt.Dispose();
                            dt = null;
                            return;
                        }
                    }

                    nSeqno = Convert.ToInt32(VB.Val(dt.Rows[0]["SEQNO"].ToString().Trim())) + 1;
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            nIwoLQty = 0; nIwoLAmt = 0; nBalQty = 0; nBalAmt = 0;
            nChulQty = 0; nchulAmt = 0; nMisuQty = 0; nMisuAmt = 0;

            clsDB.setBeginTran(clsDB.DbCon);

            SQL = "";
            SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "CARD_APPROV_BI " + "SET             ";
            SQL += ComNum.VBLF + "BUDATE  = ''                                                 ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                       ";
            SQL += ComNum.VBLF + "      AND ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')   ";
            SQL += ComNum.VBLF + "      AND ACTDATE <= TO_dATE('" + strTDate + "','YYYY-MM-DD')   ";
            SQL += ComNum.VBLF + "      AND BUDATE >= TO_DATE('" + strFDate2 + "','YYYY-MM-DD')   ";
            SQL += ComNum.VBLF + "      AND GbBun ='1'   "; //생활유지비만

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            clsDB.setCommitTran(clsDB.DbCon);

            clsDB.setBeginTran(clsDB.DbCon);

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                 ";
            SQL += ComNum.VBLF + "  MSEQNO, PANO, SUM(AMT) AMT,                                                          ";
            SQL += ComNum.VBLF + "  TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE                                                ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "CARD_APPROV_BI                                              ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                              ";
            SQL += ComNum.VBLF + "      AND ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')                          ";
            SQL += ComNum.VBLF + "      AND ACTDATE <= TO_dATE('" + strTDate + "','YYYY-MM-DD')                          ";
            SQL += ComNum.VBLF + "      AND (IPGUMDATE IS NULL or IPGUMDATE >= TO_DATE('" + strFDate2 + "','YYYY-MM-DD'))";
            SQL += ComNum.VBLF + "      AND AMT <> 0                                                                     ";
            SQL += ComNum.VBLF + "      AND GbBun ='1'                                                                   ";    //생활유지비만
            SQL += ComNum.VBLF + "GROUP BY MSEQNO,PANO, ACTDATE                                                          ";
            SQL += ComNum.VBLF + "Having Sum(AMT) <> 0                                                                   ";

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
                    //if (dt.Rows[i]["Pano"].ToString().Trim() == "06447267" || dt.Rows[i]["Pano"].ToString().Trim() == "06144803")
                    //{
                    //    i = i;
                    //}

                    SQL = "";
                    SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "CARD_APPROV_BI " + "SET                                           ";
                    SQL += ComNum.VBLF + "  BUDATE = TO_DATE('" + strFDate2 + "','YYYY-MM-DD')                                          ";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                                                     ";
                    SQL += ComNum.VBLF + "      AND PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "'                               ";
                    SQL += ComNum.VBLF + "      AND MSEQNO = '" + dt.Rows[i]["MSEQNO"].ToString().Trim() + "'                           ";
                    SQL += ComNum.VBLF + "      AND ACTDATE = TO_DATE('" + dt.Rows[i]["ACTDATE"].ToString().Trim() + "','YYYY-MM-DD')   ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                }
            }

            dt.Dispose();
            dt = null;

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            clsDB.setCommitTran(clsDB.DbCon);

            //전월이월 건수, 금액
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                            ";
            SQL += ComNum.VBLF + "  COUNT(MSEQNO) CNT, SUM(AMT) AMT                                 ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "CARD_APPROV_BI                          ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                         ";
           // SQL += ComNum.VBLF + "      AND BUDATE >= TO_DATE('" + strFDate3 + "','YYYY-MM-DD')     ";
            SQL += ComNum.VBLF + "      AND BUDATE <= TO_DATE('" + strFDate + "','YYYY-MM-DD')      ";
            SQL += ComNum.VBLF + "      AND (IPGUMDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')  ";
            SQL += ComNum.VBLF + "              OR IPGUMDATE IS NULL)                               ";
            SQL += ComNum.VBLF + "      AND AMT <> 0                                                ";
            SQL += ComNum.VBLF + "      AND GbBun ='1'                                              ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                nIwoLQty = Convert.ToInt32(VB.Val(dt.Rows[0]["CNT"].ToString().Trim()));
                nIwoLAmt = Convert.ToInt32(VB.Val(dt.Rows[0]["AMT"].ToString().Trim()));
            }

            dt.Dispose();
            dt = null;

            //당월발생금액
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                            ";
            SQL += ComNum.VBLF + "  COUNT(X.MSEQNO) CNT, SUM(X.AMT) AMT                             ";
            SQL += ComNum.VBLF + "FROM (                                                            ";
            SQL += ComNum.VBLF + "SELECT MSEQNO, SUM(AMT) AMT, ACTDATE  FROM CARD_APPROV_BI         ";
            SQL += ComNum.VBLF + "WHERE ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')         ";
            SQL += ComNum.VBLF + "  AND ACTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')         ";
            SQL += ComNum.VBLF + "  AND AMT <> 0                                                    ";
            SQL += ComNum.VBLF + "  AND GbBun ='1'                                                  ";
            SQL += ComNum.VBLF + "GROUP BY MSEQNO, ACTDATE                                          ";
            SQL += ComNum.VBLF + "HAVING SUM(AMT) <> 0) X                                           ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                nBalQty = Convert.ToInt32(VB.Val(dt.Rows[0]["CNT"].ToString().Trim()));
                nBalAmt = Convert.ToInt32(VB.Val(dt.Rows[0]["AMT"].ToString().Trim()));
            }

            dt.Dispose();
            dt = null;

            //당월처리금액
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                            ";
            SQL += ComNum.VBLF + "  COUNT(X.MSEQNO) CNT, SUM(X.AMT) AMT                             ";
            SQL += ComNum.VBLF + "FROM (                                                            ";
            SQL += ComNum.VBLF + "SELECT MSEQNO, SUM(AMT) AMT, ACTDATE  FROM CARD_APPROV_BI         ";
            SQL += ComNum.VBLF + "WHERE IPGUMDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')       ";
            SQL += ComNum.VBLF + "  AND IPGUMDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')       ";
            SQL += ComNum.VBLF + "  AND AMT <> 0                                                    ";
            SQL += ComNum.VBLF + "  AND GbBun ='1'                                                  ";
            SQL += ComNum.VBLF + "GROUP BY MSEQNO, ACTDATE                                          ";
            SQL += ComNum.VBLF + "HAVING SUM(AMT) <> 0) X                                           ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                nChulQty = Convert.ToInt32(VB.Val(dt.Rows[0]["CNT"].ToString().Trim()));
                nchulAmt = Convert.ToInt32(VB.Val(dt.Rows[0]["AMT"].ToString().Trim()));
            }

            dt.Dispose();
            dt = null;

            nMisuQty = nIwoLQty + nBalQty - nChulQty;
            nMisuAmt = nIwoLAmt + nBalAmt - nchulAmt;

            clsDB.setBeginTran(clsDB.DbCon);
            SQL = "";
            SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "MISU_BOHO_MONTHLY               ";
            SQL += ComNum.VBLF + "(                                                                 ";
            SQL += ComNum.VBLF + "YYMM,SEQNO,IPDOPD,                                                ";
            SQL += ComNum.VBLF + "IWOLQTY,IWOLAMT,BALQTY,BALAMT,CHULQTY,CHULAMT,                    ";
            SQL += ComNum.VBLF + "MISUQTY, MISUAMT,REMARK,ENTDATE,SABUN,GbBun)                      ";
            SQL += ComNum.VBLF + "VALUES(                                                           ";
            SQL += ComNum.VBLF + "'" + strYYMM + "',                                                ";
            SQL += ComNum.VBLF + "" + nSeqno + ",                                                   ";
            SQL += ComNum.VBLF + "'O',                                                              ";
            SQL += ComNum.VBLF + "" + nIwoLQty + ",                                                 ";
            SQL += ComNum.VBLF + "" + nIwoLAmt + ",                                                 ";
            SQL += ComNum.VBLF + "" + nBalQty + ",                                                  ";
            SQL += ComNum.VBLF + "" + nBalAmt + ",                                                  ";
            SQL += ComNum.VBLF + "" + nChulQty + ",                                                 ";
            SQL += ComNum.VBLF + "" + nchulAmt + ",                                                 ";
            SQL += ComNum.VBLF + "" + nMisuQty + ",                                                 ";
            SQL += ComNum.VBLF + "" + nMisuAmt + ",                                                 ";
            SQL += ComNum.VBLF + "'" + strRemark + "',                                              ";
            SQL += ComNum.VBLF + "SYSDATE,                                                          ";
            SQL += ComNum.VBLF + "'" + ComFunc.SetAutoZero(clsType.User.Sabun, 5) + "',           ";
            SQL += ComNum.VBLF + "'1'                                                               ";
            SQL += ComNum.VBLF + ")                                                                 ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            clsDB.setCommitTran(clsDB.DbCon);

            ComFunc.MsgBox("생활유지비 - 정상적으로 빌더 완료");

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                                    ";
            SQL += ComNum.VBLF + "  REMARK, SABUN,                                                                                                          ";
            SQL += ComNum.VBLF + "  TO_CHAR(ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE                                                                           ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BOHO_MONTHLY                                                                              ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                                 ";
            SQL += ComNum.VBLF + "      AND YYMM  = '" + VB.Left(cboYYMM2.Text.ToString(), 4) + VB.Mid(cboYYMM2.Text.ToString(), 7, 2) + "' ";
            SQL += ComNum.VBLF + "      AND GbBun ='1'                                                                                                      ";
            SQL += ComNum.VBLF + "ORDER BY ENTDATE DESC,REMARK, SABUN                                                                                       ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                txtRemark.Text = " 작업년월 : " + VB.Left(cboYYMM2.Text.ToString(), 4) + VB.Mid(cboYYMM2.Text.ToString(), 7, 2) + "\r\n" +
                                 " 작업일자 : " + VB.Left(dt.Rows[0]["ENTDATE"].ToString().Trim(), 10) + "\r\n" +
                                 " 작업시간 : " + VB.Right(dt.Rows[0]["ENTDATE"].ToString().Trim(), 5) + "\r\n" +
                                 " 작 업 자 : " + dt.Rows[0]["SABUN"].ToString().Trim();

                txtRemark2.Text = " " + dt.Rows[0]["REMARK"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;

            //산전지원 빌드

            strYYMM = VB.Left(cboYYMM2.Text.ToString(), 4) + VB.Mid(cboYYMM2.Text.ToString().Trim(), 7, 2);
            strYYMM2 = CPF.DATE_YYMM_ADD(strYYMM, 1);

            strFDate = VB.Left(cboYYMM2.Text.ToString(), 4) + "-" + VB.Mid(cboYYMM2.Text.ToString(), 7, 2) + "-01";
            strTDate = CF.READ_LASTDAY(clsDB.DbCon, strFDate);
            strFDate2 = VB.Left(strYYMM2, 4) + "-" + VB.Right(strYYMM2, 2) + "-01";
            strTDate2 = CF.READ_LASTDAY(clsDB.DbCon, strFDate2);

            strFDate3 = VB.Left(CPF.DATE_YYMM_ADD(strYYMM, -1), 4) + "-" + VB.Right(CPF.DATE_YYMM_ADD(strYYMM, -1), 2) + "-01";

            nSeqno = 1;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                        ";
            SQL += ComNum.VBLF + "  MAX(SEQNO) SEQNO                            ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BOHO_MONTHLY  ";
            SQL += ComNum.VBLF + "WHERE 1=1                                     ";
            SQL += ComNum.VBLF + "      AND YYMM  = '" + strYYMM + "'           ";
            SQL += ComNum.VBLF + "      AND GbBun ='2'                          ";  //산전지원

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (Convert.ToInt32(VB.Val(dt.Rows[0]["SEQNO"].ToString().Trim())) > 0)
            {
                if (MessageBox.Show("빌더한 내역이 있습니다 다시 빌더 작업을 하시겠습니까???", "작업선택", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }
                else
                {
                    strRemark = VB.InputBox("다시 작업하는 이유를 간략하게 기술하시오? 입력하지 않으면 프로그램 종료됩니다.");
                    if (strRemark == "")
                    {
                        dt.Dispose();
                        dt = null;
                        return;
                    }
                }
                nSeqno = Convert.ToInt32(VB.Val(dt.Rows[0]["SEQNO"].ToString().Trim())) + 1;
            }

            dt.Dispose();
            dt = null;

            nIwoLQty = 0; nIwoLAmt = 0; nBalQty = 0; nBalAmt = 0;
            nChulQty = 0; nchulAmt = 0; nMisuQty = 0; nMisuAmt = 0;

            clsDB.setBeginTran(clsDB.DbCon);

            SQL = "";
            SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "CARD_APPROV_BI " + "SET             ";
            SQL += ComNum.VBLF + "BUDATE  = ''                                                    ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                       ";
            SQL += ComNum.VBLF + "      AND ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')   ";
            SQL += ComNum.VBLF + "      AND ACTDATE <= TO_dATE('" + strTDate + "','YYYY-MM-DD')   ";
            SQL += ComNum.VBLF + "      AND BUDATE >= TO_DATE('" + strFDate2 + "','YYYY-MM-DD')   ";
            SQL += ComNum.VBLF + "      AND GbBun ='2'                                            ";     //산전지원

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            clsDB.setCommitTran(clsDB.DbCon);

            clsDB.setBeginTran(clsDB.DbCon);

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                 ";
            SQL += ComNum.VBLF + "  MSEQNO, PANO, SUM(AMT1) AMT,                                                         ";
            SQL += ComNum.VBLF + "  TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE                                                ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "CARD_APPROV_BI                                              ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                              ";
            SQL += ComNum.VBLF + "      AND ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')                          ";
            SQL += ComNum.VBLF + "      AND ACTDATE <= TO_dATE('" + strTDate + "','YYYY-MM-DD')                          ";
            SQL += ComNum.VBLF + "      AND (IPGUMDATE IS NULL or IPGUMDATE >= TO_DATE('" + strFDate2 + "','YYYY-MM-DD'))";
            SQL += ComNum.VBLF + "      AND AMT1 <> 0                                                                    ";
            SQL += ComNum.VBLF + "      AND GbBun ='2'                                                                   ";    //생활유지비만
            SQL += ComNum.VBLF + "GROUP BY MSEQNO,PANO, ACTDATE                                                          ";
            SQL += ComNum.VBLF + "Having Sum(AMT1) <> 0                                                                  ";

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
                    SQL = "";
                    SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "CARD_APPROV_BI" + " SET                                          ";
                    SQL += ComNum.VBLF + "  BUDATE = TO_DATE('" + strFDate2 + "','YYYY-MM-DD')                                          ";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                                                     ";
                    SQL += ComNum.VBLF + "      AND PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "'                               ";
                    SQL += ComNum.VBLF + "      AND MSEQNO = '" + dt.Rows[i]["MSEQNO"].ToString().Trim() + "'                           ";
                    SQL += ComNum.VBLF + "      AND ACTDATE = TO_DATE('" + dt.Rows[i]["ACTDATE"].ToString().Trim() + "','YYYY-MM-DD')   ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                }
            }

            dt.Dispose();
            dt = null;

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            clsDB.setCommitTran(clsDB.DbCon);

            //전월이월 건수, 금액
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                            ";
            SQL += ComNum.VBLF + "  COUNT(MSEQNO) CNT, SUM(AMT1) AMT                                ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "CARD_APPROV_BI                         ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                         ";
            SQL += ComNum.VBLF + "      AND BUDATE >= TO_DATE('" + strFDate3 + "','YYYY-MM-DD')     ";
            SQL += ComNum.VBLF + "      AND BUDATE <= TO_DATE('" + strFDate + "','YYYY-MM-DD')      ";
            SQL += ComNum.VBLF + "      AND (IPGUMDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')  ";
            SQL += ComNum.VBLF + "              OR IPGUMDATE IS NULL)                               ";
            SQL += ComNum.VBLF + "      AND AMT1 <> 0                                               ";
            SQL += ComNum.VBLF + "      AND GbBun ='2'                                              ";  //산전지원
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                nIwoLQty = Convert.ToInt32(VB.Val(dt.Rows[0]["CNT"].ToString().Trim()));
                nIwoLAmt = Convert.ToInt32(VB.Val(dt.Rows[0]["AMT"].ToString().Trim()));
            }

            dt.Dispose();
            dt = null;

            //당월발생금액
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                            ";
            SQL += ComNum.VBLF + "  COUNT(X.MSEQNO) CNT, SUM(X.AMT1) AMT                            ";
            SQL += ComNum.VBLF + "FROM (                                                            ";
            SQL += ComNum.VBLF + "SELECT MSEQNO, SUM(AMT1) AMT1, ACTDATE  FROM CARD_APPROV_BI       ";
            SQL += ComNum.VBLF + "WHERE ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')         ";
            SQL += ComNum.VBLF + "  AND ACTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')         ";
            SQL += ComNum.VBLF + "  AND AMT1 <> 0                                                   ";  
            SQL += ComNum.VBLF + "  AND GbBun ='2'                                                  ";  //산전지원
            SQL += ComNum.VBLF + "GROUP BY MSEQNO, ACTDATE                                          ";
            SQL += ComNum.VBLF + "HAVING SUM(AMT1) <> 0) X                                          ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                nBalQty = Convert.ToInt32(VB.Val(dt.Rows[0]["CNT"].ToString().Trim()));
                nBalAmt = Convert.ToInt32(VB.Val(dt.Rows[0]["AMT"].ToString().Trim()));
            }

            dt.Dispose();
            dt = null;

            //당월처리금액
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                            ";
            SQL += ComNum.VBLF + "  COUNT(X.MSEQNO) CNT, SUM(X.AMT1) AMT                            ";
            SQL += ComNum.VBLF + "FROM (                                                            ";
            SQL += ComNum.VBLF + "SELECT MSEQNO, SUM(AMT1) AMT1, ACTDATE  FROM CARD_APPROV_BI       ";
            SQL += ComNum.VBLF + "WHERE IPGUMDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')       ";
            SQL += ComNum.VBLF + "  AND IPGUMDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')       ";
            SQL += ComNum.VBLF + "  AND AMT1 <> 0                                                   ";  //생활유지비만
            SQL += ComNum.VBLF + "  AND GbBun ='2'                                                  ";
            SQL += ComNum.VBLF + "GROUP BY MSEQNO, ACTDATE                                          ";
            SQL += ComNum.VBLF + "HAVING SUM(AMT1) <> 0) X                                          ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                nChulQty = Convert.ToInt32(VB.Val(dt.Rows[0]["CNT"].ToString().Trim()));
                nchulAmt = Convert.ToInt32(VB.Val(dt.Rows[0]["AMT"].ToString().Trim()));
            }

            dt.Dispose();
            dt = null;

            nMisuQty = nIwoLQty + nBalQty - nChulQty;
            nMisuAmt = nIwoLAmt + nBalAmt - nchulAmt;

            clsDB.setBeginTran(clsDB.DbCon);
            SQL = "";
            SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "MISU_BOHO_MONTHLY               ";
            SQL += ComNum.VBLF + "(                                                                 ";
            SQL += ComNum.VBLF + "YYMM,SEQNO,IPDOPD,                                                ";
            SQL += ComNum.VBLF + "IWOLQTY,IWOLAMT,BALQTY,BALAMT,CHULQTY,CHULAMT,                    ";
            SQL += ComNum.VBLF + "MISUQTY, MISUAMT,REMARK,ENTDATE,SABUN,GbBun)                      ";
            SQL += ComNum.VBLF + "VALUES(                                                           ";
            SQL += ComNum.VBLF + "'" + strYYMM + "',                                                ";
            SQL += ComNum.VBLF + "" + nSeqno + ",                                                   ";
            SQL += ComNum.VBLF + "'O',                                                              ";
            SQL += ComNum.VBLF + "" + nIwoLQty + ",                                                 ";
            SQL += ComNum.VBLF + "" + nIwoLAmt + ",                                                 ";
            SQL += ComNum.VBLF + "" + nBalQty + ",                                                  ";
            SQL += ComNum.VBLF + "" + nBalAmt + ",                                                  ";
            SQL += ComNum.VBLF + "" + nChulQty + ",                                                 ";
            SQL += ComNum.VBLF + "" + nchulAmt + ",                                                 ";
            SQL += ComNum.VBLF + "" + nMisuQty + ",                                                 ";
            SQL += ComNum.VBLF + "" + nMisuAmt + ",                                                 ";
            SQL += ComNum.VBLF + "'" + strRemark + "',                                              ";
            SQL += ComNum.VBLF + "SYSDATE,                                                          ";
            SQL += ComNum.VBLF + "'" + ComFunc.SetAutoZero(clsType.User.Sabun, 5) + "',           ";
            SQL += ComNum.VBLF + "'2'                                                               ";
            SQL += ComNum.VBLF + ")                                                                 ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            clsDB.setCommitTran(clsDB.DbCon);

            ComFunc.MsgBox("산전지원 - 정상적으로 빌더 완료");

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                                    ";
            SQL += ComNum.VBLF + "  REMARK, SABUN,                                                                                                          ";
            SQL += ComNum.VBLF + "  TO_CHAR(ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE                                                                           ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BOHO_MONTHLY                                                                              ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                                 ";
            SQL += ComNum.VBLF + "      AND YYMM  = '" + VB.Left(cboYYMM2.Text.ToString(), 4) + VB.Mid(cboYYMM2.Text.ToString(), 7, 2) + "' ";
            SQL += ComNum.VBLF + "      AND GbBun ='2'                                                                                                      ";  //산전지원
            SQL += ComNum.VBLF + "ORDER BY ENTDATE DESC,REMARK, SABUN                                                                                       ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                txtRemark.Text = " 작업년월 : " + VB.Left(cboYYMM2.Text.ToString(), 4) + VB.Mid(cboYYMM2.Text.ToString(), 7, 2) + "\r\n" +
                                 " 작업일자 : " + VB.Left(dt.Rows[0]["ENTDATE"].ToString().Trim(), 10) + "\r\n" +
                                 " 작업시간 : " + VB.Right(dt.Rows[0]["ENTDATE"].ToString().Trim(), 5) + "\r\n" +
                                 " 작 업 자 : " + dt.Rows[0]["SABUN"].ToString().Trim();

                txtRemark2.Text = " " + dt.Rows[0]["REMARK"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;
        }

        void ssList1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;
            string strYYMM = "";
            string strYYMM2 = "";
            string strFDate = "";
            string strTDate = "";
            string strTDate2 = "";
            string strFDate2 = "";
            double nTotAmt = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            strYYMM = VB.Left(cboYYMM.Text.ToString(), 4) + VB.Mid(cboYYMM.Text.ToString().Trim(), 7, 2);
            strYYMM2 = CPF.DATE_YYMM_ADD(strYYMM, -1);

            strFDate = VB.Left(cboYYMM.Text.ToString(), 4) + "-" + VB.Mid(cboYYMM.Text.ToString(), 7, 2) + "-01";
            strTDate = CF.READ_LASTDAY(clsDB.DbCon, strFDate);
            strFDate2 = VB.Left(strYYMM2, 4) + "-" + VB.Right(strYYMM2, 2) + "-01";
            strTDate2 = CF.READ_LASTDAY(clsDB.DbCon, strFDate2);

            ssList2_Sheet1.Rows.Count = 0;

            nTotAmt = 0;

            Cursor.Current = Cursors.WaitCursor;

            if (MessageBox.Show("세부내역 조회 시 시간이 다소 소요됩니다. " + "\r\n" + "조회 완료되면 자동으로 탭이 바뀝니다. " + "\r\n" + "정말로 조회를 하시겠습니까?", "작업선택", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            switch (e.Column)
            {
                case 0:
                case 1:
                    //전월이월 건수, 금액
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                                          ";
                    SQL += ComNum.VBLF + "  TO_CHAR(ACTDATE,'YYYY-MM-DD'), ACTDATE,                       ";
                    SQL += ComNum.VBLF + "  PANO, SUM(DECODE(GbBun,'1',AMT,'2',AMT1)) AMT, MSEQNO,        ";
                    SQL += ComNum.VBLF + "  TO_CHAR(IPGUMDATE,'YYYY-MM-DD'), IPGUMDATE                    ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "CARD_APPROV_BI                       ";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                       ";
                    //SQL += ComNum.VBLF + "      AND BUDATE >= TO_DATE('" + strFDate2 + "','YYYY-MM-DD')   ";
                    SQL += ComNum.VBLF + "      AND BUDATE <= TO_DATE('" + strFDate + "','YYYY-MM-DD')    ";
                    SQL += ComNum.VBLF + "      AND (IPGUMDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "              OR IPGUMDATE IS NULL)                             ";

                    if (optGbn0.Checked == true)
                    {
                        SQL += ComNum.VBLF + "  AND GbBun ='1'                                            ";    //생활유지비만
                    }
                    else if (optGbn1.Checked == true)
                    {
                        SQL += ComNum.VBLF + "  AND GbBun ='2'                                            ";    //산전지원
                    }
                    SQL += ComNum.VBLF + "GROUP BY MSEQNO,ACTDATE,PANO,IPGUMDATE                          ";
                    SQL += ComNum.VBLF + "HAVING SUM(DECODE(GbBun,'1',AMT,'2',AMT1))  <> 0                ";
                    SQL += ComNum.VBLF + "ORDER BY ACTDATE,PANO                                           ";

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

                    if (dt.Rows.Count > 0)
                    {
                        ssList2_Sheet1.Rows.Count = dt.Rows.Count + 1;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            ssList2_Sheet1.Cells[i, 0].Text =  (i + 1).ToString();    
                            ssList2_Sheet1.Cells[i, 1].Text = VB.Left(dt.Rows[i]["ACTDATE"].ToString().Trim(), 10);
                            ssList2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();
                            ssList2_Sheet1.Cells[i, 3].Text = CF.Read_Patient(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim(), "2");
                            ssList2_Sheet1.Cells[i, 4].Text = String.Format("{0:###,###,###}", VB.Val(dt.Rows[i]["AMT"].ToString().Trim()));
                            ssList2_Sheet1.Cells[i, 5].Text = VB.Left(dt.Rows[i]["IPGUMDATE"].ToString().Trim(), 10);

                            nTotAmt += VB.Val(dt.Rows[i]["AMT"].ToString().Trim());

                        }
                    }

                    dt.Dispose();
                    dt = null;

                    ssList2_Sheet1.Cells[ssList2_Sheet1.Rows.Count - 1, 3].Text = "합 계";
                    ssList2_Sheet1.Cells[ssList2_Sheet1.Rows.Count - 1, 4].Text = String.Format("{0:###,###,###}", nTotAmt);
                    FstrGubun = "1";
                    break;

                case 2:
                case 3:
                    //당월발생금액
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                                          ";
                    SQL += ComNum.VBLF + "  MSEQNO, SUM(DECODE(GbBun,'1',AMT,'2',AMT1))  AMT, PANO,       ";
                    SQL += ComNum.VBLF + "  TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE,                        ";
                    SQL += ComNum.VBLF + "  TO_CHAR(IPGUMDATE,'YYYY-MM-DD'), IPGUMDATE                    ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "CARD_APPROV_BI                       ";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                       ";
                    SQL += ComNum.VBLF + "      AND ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')   ";
                    SQL += ComNum.VBLF + "      AND ACTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')   ";

                    if (optGbn0.Checked == true)
                    {
                        SQL += ComNum.VBLF + "  AND GbBun ='1'                                            ";    //생활유지비만
                    }
                    else if (optGbn1.Checked == true)
                    {
                        SQL += ComNum.VBLF + "  AND GbBun ='2'                                            ";    //산전지원
                    }
                    SQL += ComNum.VBLF + "GROUP BY MSEQNO, ACTDATE, IPGUMDATE, PANO                       ";
                    SQL += ComNum.VBLF + "HAVING SUM(DECODE(GbBun,'1',AMT,'2',AMT1))  <> 0                ";
                    SQL += ComNum.VBLF + "ORDER BY ACTDATE,PANO                                           ";

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

                    if (dt.Rows.Count > 0)
                    {
                        ssList2_Sheet1.Rows.Count = dt.Rows.Count + 1;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            ssList2_Sheet1.Cells[i, 0].Text = (i + 1).ToString();
                            ssList2_Sheet1.Cells[i, 1].Text = VB.Left(dt.Rows[i]["ACTDATE"].ToString().Trim(), 10);
                            ssList2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();
                            ssList2_Sheet1.Cells[i, 3].Text = CF.Read_Patient(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim(), "2");
                            ssList2_Sheet1.Cells[i, 4].Text = String.Format("{0:###,###,###}", VB.Val(dt.Rows[i]["AMT"].ToString().Trim()));
                            ssList2_Sheet1.Cells[i, 5].Text = VB.Left(dt.Rows[i]["IPGUMDATE"].ToString().Trim(), 10);

                            nTotAmt += VB.Val(dt.Rows[i]["AMT"].ToString().Trim());

                        }
                    }

                    dt.Dispose();
                    dt = null;

                    ssList2_Sheet1.Cells[ssList2_Sheet1.Rows.Count - 1, 3].Text = "합 계";
                    ssList2_Sheet1.Cells[ssList2_Sheet1.Rows.Count - 1, 4].Text = String.Format("{0:###,###,###}", nTotAmt);
                    FstrGubun = "1";
                    break;

                case 4:
                case 5:
                    //당월처리금액
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                                          ";
                    SQL += ComNum.VBLF + "  MSEQNO, SUM(DECODE(GbBun,'1',AMT,'2',AMT1))  AMT, PANO,       ";
                    SQL += ComNum.VBLF + "  TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE,                        ";
                    SQL += ComNum.VBLF + "  TO_CHAR(IPGUMDATE,'YYYY-MM-DD'), IPGUMDATE                    ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "CARD_APPROV_BI                       ";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                       ";
                    SQL += ComNum.VBLF + "      AND IPGUMDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "      AND IPGUMDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";

                    if (optGbn0.Checked == true)
                    {
                        SQL += ComNum.VBLF + "  AND GbBun ='1'                                            ";    //생활유지비만
                    }
                    else if (optGbn1.Checked == true)
                    {
                        SQL += ComNum.VBLF + "  AND GbBun ='2'                                            ";    //산전지원
                    }
                    SQL += ComNum.VBLF + "GROUP BY MSEQNO, ACTDATE, PANO, IPGUMDATE                       ";
                    SQL += ComNum.VBLF + "HAVING SUM(DECODE(GbBun,'1',AMT,'2',AMT1))  <> 0                ";
                    SQL += ComNum.VBLF + "ORDER BY ACTDATE,PANO                                           ";

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

                    if (dt.Rows.Count > 0)
                    {
                        ssList2_Sheet1.Rows.Count = dt.Rows.Count + 1;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            ssList2_Sheet1.Cells[i, 0].Text = (i + 1).ToString();
                            ssList2_Sheet1.Cells[i, 1].Text = VB.Left(dt.Rows[i]["ACTDATE"].ToString().Trim(), 10);
                            ssList2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();
                            ssList2_Sheet1.Cells[i, 3].Text = CF.Read_Patient(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim(), "2");
                            ssList2_Sheet1.Cells[i, 4].Text = String.Format("{0:###,###,###}", VB.Val(dt.Rows[i]["AMT"].ToString().Trim()));
                            ssList2_Sheet1.Cells[i, 5].Text = VB.Left(dt.Rows[i]["IPGUMDATE"].ToString().Trim(), 10);

                            nTotAmt += VB.Val(dt.Rows[i]["AMT"].ToString().Trim());

                        }
                    }

                    dt.Dispose();
                    dt = null;

                    ssList2_Sheet1.Cells[ssList2_Sheet1.Rows.Count - 1, 3].Text = "합 계";
                    ssList2_Sheet1.Cells[ssList2_Sheet1.Rows.Count - 1, 4].Text = String.Format("{0:###,###,###}", nTotAmt);
                    FstrGubun = "1";
                    break;

                case 6:
                case 7:
                    //미처리금액
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                                                                                             ";
                    SQL += ComNum.VBLF + "  MSEQNO, PANO, SUM(DECODE(GbBun,'1',AMT,'2',AMT1))  AMT,                                                          ";
                    SQL += ComNum.VBLF + "  TO_CHAR(ACTDATE,'YYYY-MM-DD'), ACTDATE,                                                                          ";
                    SQL += ComNum.VBLF + "  TO_CHAR(IPGUMDATE,'YYYY-MM-DD'), IPGUMDATE                                                                       ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "CARD_APPROV_BI                                                                          ";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                                                                          ";
                    SQL += ComNum.VBLF + "      AND BUDATE <= TO_DATE('" + Convert.ToDateTime(strTDate).AddDays(1).ToShortDateString() + "','YYYY-MM-DD')    ";
                    SQL += ComNum.VBLF + "      AND (IPGUMDATE >= TO_DATE('" + Convert.ToDateTime(strTDate).AddDays(1).ToShortDateString() + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "          OR IPGUMDATE IS NULL)                                                                                    ";

                    if (optGbn0.Checked == true)
                    {
                        SQL += ComNum.VBLF + "  AND GbBun ='1'                                            ";    //생활유지비만
                    }
                    else if (optGbn1.Checked == true)
                    {
                        SQL += ComNum.VBLF + "  AND GbBun ='2'                                            ";    //산전지원
                    }
                    SQL += ComNum.VBLF + "GROUP BY ACTDATE, IPGUMDATE, PANO,MSEQNO                        ";
                    SQL += ComNum.VBLF + "HAVING SUM(DECODE(GbBun,'1',AMT,'2',AMT1))  <> 0                ";

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

                    if (dt.Rows.Count > 0)
                    {
                        ssList2_Sheet1.Rows.Count = dt.Rows.Count + 1;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            ssList2_Sheet1.Cells[i + 1, 0].Text = (i + 1).ToString();
                            ssList2_Sheet1.Cells[i + 1, 1].Text = VB.Left(dt.Rows[i]["ACTDATE"].ToString().Trim(), 10);
                            ssList2_Sheet1.Cells[i + 1, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();
                            ssList2_Sheet1.Cells[i + 1, 3].Text = CF.Read_Patient(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim(), "2");
                            ssList2_Sheet1.Cells[i + 1, 4].Text = String.Format("{0:###,###,###}", VB.Val(dt.Rows[i]["AMT"].ToString().Trim()));
                            ssList2_Sheet1.Cells[i + 1, 5].Text = VB.Left(dt.Rows[i]["IPGUMDATE"].ToString().Trim(), 10);

                            nTotAmt += VB.Val(dt.Rows[i]["AMT"].ToString().Trim());

                        }
                    }

                    dt.Dispose();
                    dt = null;

                    ssList2_Sheet1.Cells[0, 3].Text = "합 계";
                    ssList2_Sheet1.Cells[0, 4].Text = String.Format("{0:###,###,###}", nTotAmt);
                    FstrGubun = "1";
                    break;                    
            }

            tabControl1.SelectedTabIndex = 1;
            btnPrint2.Visible = true;

            Cursor.Current = Cursors.Default;


        }

        private void ssList1_CellClick(object sender, CellClickEventArgs e)
        {

        }
    }
}
