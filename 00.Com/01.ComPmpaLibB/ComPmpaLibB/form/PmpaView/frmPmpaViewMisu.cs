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
    /// File Name       : frmPmpaViewMisu.cs
    /// Description     : 미수금 명부 조회 폼
    /// Author          : 안정수
    /// Create Date     : 2017-08-25
    /// Update History  : 2017-11-02
    /// <history>           
    /// d:\psmh\OPD\olrepa\OLREPA04.FRM(frmmisu) => frmPmpaViewMisu.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\olrepa\OLREPA04.FRM(frmmisu)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewMisu : Form, MainFormMessage
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();

        FarPoint.Win.LineBorder lineBorder = new FarPoint.Win.LineBorder(Color.Black, 1, false, false, false, true);        

        string mstrJobName = "";

        //************************************


        int[] nCho1 = new int[5];
        int[] nJe1 = new int[5];
        int[] ninTot1 = new int[5];

        int[] nCho2 = new int[5];
        int[] nJe2 = new int[5];
        int[] ninTot2 = new int[5];

        double[] nAmt1 = new double[5];
        double[] nAmt2 = new double[5];
        double[] nAmt3 = new double[5];
        double[] nAmt4 = new double[5];
        double[] nAmt5 = new double[5];
        double[] nAmt6 = new double[5];
        double[] nAmt7 = new double[5];

        double[] nJinChal = new double[5];
        double[] nToYak = new double[5];
        double[] nJusa = new double[5];
        double[] nMaChi = new double[5];
        double[] nMulri = new double[5];
        double[] nSin = new double[5];
        double[] nCher = new double[5];
        double[] nIngong = new double[5];
        double[] nSusul = new double[5];
        double[] nGiv = new double[5];
        double[] nTSG = new double[5];
        double[] nGTG = new double[5];
        double[] nXray = new double[5];
        double[] nBang = new double[5];

        double[] nTot1 = new double[5];

        double[] nCT = new double[5];
        double[] nJY = new double[5];
        double[] nCar = new double[5];
        double[] nTJ = new double[5];
        double[] nGita = new double[5];
        double[] nTot2 = new double[5];

        double[] nSuTot = new double[5];

        double[] nChomi = new double[5];
        double[] nGemi = new double[5];
        double[] nGam = new double[5];
        double[] nTot3 = new double[5];

        double[] nHpay = new double[5];

        double[] nYpay = new double[5];

        double[] nMisuAmt = new double[5];
        double[] nEtcAmt = new double[5];
        double[] nYTot = new double[5];

        //************************************

        int nCount = 0;
        string strSelect = "";

        string[] StrGamSel = new string[300];
        string[] StrGamPano = new string[300];
        string[] StrGamName = new string[300];
        string[] strGamGwa = new string[300];
        string[] strGamDr = new string[300];

        double[] nJubsuAmt = new double[300];
        double[] nJinRuAmt = new double[300];
        double[] nGamTot = new double[300];

        string[] strGamPart = new string[300];
        string[] strGamBigo = new string[300];

        double nJubSuTot = 0;
        double nJinRuTot = 0;
        double nTotTotAmt = 0;

        //************************************

        string StrGubun1 = "";

        string[] StrMiSel = new string[3500];
        string[] strMiPano = new string[3500];
        string[] strMIName = new string[3500];
        string[] strMIGwa = new string[3500];
        string[] strMiDr = new string[3500];
        string[] StrMiBohun = new string[3500];
        string[] StrMiGelCode = new string[3500];

        double[] nJubsuMi = new double[3500];
        double[] nJinruMi = new double[3500];
        double[] nMiTot = new double[3500];

        string strDate = "";

        FarPoint.Win.LineBorder lb = new FarPoint.Win.LineBorder(Color.Black);

        #region MainFormMessage InterFace

        public MainFormMessage mCallForm = null;

        public void MsgActivedForm(Form frm)
        {

        }

        public void MsgUnloadForm(Form frm)
        {

        }

        public void MsgFormClear()
        {

        }

        public void MsgSendPara(string strPara)
        {

        }

        #endregion

        public frmPmpaViewMisu()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewMisu(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();

        }

        public frmPmpaViewMisu(string GstrJobName, int GnJobSabun)
        {
            InitializeComponent();
            setEvent();
            mstrJobName = GstrJobName;
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}

            //else
            {
                //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등  

                this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

                txtPart.Text = clsType.User.Sabun;

                chkRemark.Checked = true;
                optSelect10.Checked = true;

                //if (mnJobSabun != 0)
                //{
                //    txtPart.Text = mnJobSabun.ToString();
                //}
            }

        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnView)
            {
                //                
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();
            }

            else if (sender == this.btnPrint)
            {
                //     
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ePrint();
            }
        }

        void MisuReportGubun()
        {
            switch (strSelect)
            {
                case "11":
                    StrGubun1 = "공     단";
                    break;
                case "12":
                    StrGubun1 = "직     장";
                    break;
                case "13":
                    StrGubun1 = "지     역";
                    break;
                case "21":
                    StrGubun1 = "보    호1";
                    break;
                case "22":
                    StrGubun1 = "보    호2";
                    break;
                case "23":
                    StrGubun1 = "보    호3";
                    break;
                case "24":
                    StrGubun1 = "행     려";
                    break;
                case "31":
                    StrGubun1 = "산     재";
                    break;
                case "32":
                    StrGubun1 = "공     상";
                    break;
                case "33":
                    StrGubun1 = "산재공상";
                    break;
                case "41":
                    StrGubun1 = "공  단180";
                    break;
                case "42":
                    StrGubun1 = "직  장180";
                    break;
                case "43":
                    StrGubun1 = "지  역180";
                    break;
                case "44":
                    StrGubun1 = "가족계획";
                    break;
                case "45":
                    StrGubun1 = "보험계약";
                    break;
                case "51":
                    StrGubun1 = "일    반";
                    break;
                case "52":
                    StrGubun1 = "T A 보 험";
                    break;
                case "53":
                    StrGubun1 = "계    약";
                    break;
                case "54":
                    StrGubun1 = "미 확 인";
                    break;
                case "55":
                    StrGubun1 = "T A 일 반";
                    break;
            }
        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strSubTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            string JobDate = "";
            string JobMan = "";
            string strFont1 = "";
            string strHead1 = "";
            string strFont2 = "";
            string strHead2 = "";
            string strFont3 = "";
            string strFoot1 = "";

            if (ssList_Sheet1.Rows.Count == 0)
            {
                return;
            }

            
            JobDate = dtpDate.Text;
            JobMan = mstrJobName;

       
            strFont1 = "/fn\"맑은 고딕\" /fz\"16\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"맑은 고딕\" /fz\"11\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strFont3 = "/fn\"맑은 고딕\" /fz\"9\" /fb0 /fi0 /fu0 /fk0 /fs3";

            strHead1 = "/c/f1" + "외 래 미 수 금 명 세 서" + "/f1/n"; 

            ssList_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssList_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssList_Sheet1.PrintInfo.Footer = strFont3 + strFoot1;
            ssList_Sheet1.PrintInfo.Margin.Top = 130;
            ssList_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssList_Sheet1.PrintInfo.Margin.Left = 0;
            ssList_Sheet1.PrintInfo.Margin.Header = 10;
            ssList_Sheet1.PrintInfo.Margin.Footer = 10;
            ssList_Sheet1.PrintInfo.ShowColor = false;
            ssList_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssList_Sheet1.PrintInfo.ShowBorder = false;
            ssList_Sheet1.PrintInfo.ShowGrid = true;
            ssList_Sheet1.PrintInfo.ShowShadows = false;
            ssList_Sheet1.PrintInfo.UseMax = true;
            ssList_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssList_Sheet1.PrintInfo.UseSmartPrint = false;
            ssList_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssList_Sheet1.PrintInfo.Preview = true;
            ssList.PrintSheet(0);


           // setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 85, 20);
          //  setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, false, true, true, false, false);

          //  SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);        
        }

        void eGetData()
        {
            Cursor.Current = Cursors.WaitCursor;
            ClearProcess();
            Print_Build();
            Print_Move();
            Cursor.Current = Cursors.Default;
        }

        void ClearProcess()
        {
            int i = 0;

            for (i = 0; i <= 100; i++)
            {
                StrMiSel[i] = "";
                strMiPano[i] = "";
                strMIName[i] = "";
                strMIGwa[i] = "";
                strMiDr[i] = "";
                nJubsuMi[i] = 0;
                nJinruMi[i] = 0;
                nMiTot[i] = 0;
            }

            nJubSuTot = 0;
            nJinRuTot = 0;
            nTotTotAmt = 0;
            nCount = 0;

            CS.Spread_All_Clear(ssList);

        
            
        }

        void Print_Build()
        {
            int i = 0;
            int nNum1 = 0;
            int nNum2 = 0;
            int nNum3 = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            nNum1 = 0;
            nNum2 = 0;
            nNum3 = 0;

            //* 미수금 명세 오류로 수정함(lyj) - 접수와 수납을 분리함
            // ----( 접수비중 미수 Select )---------

            strDate = dtpDate.Text;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                            ";
            SQL += ComNum.VBLF + "  BI, Pano, Sname, DeptCode, Drcode, Amt4, Amt6                   ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "Opd_Master                             ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                         ";
            SQL += ComNum.VBLF + "      AND ActDate = TO_DATE('" + strDate + "','YYYY-MM-DD')       ";
            if (optSelect10.Checked == true)
            {
                SQL += ComNum.VBLF + "  And Amt6 > 0                                                ";
            }
            else if (optSelect11.Checked == true)
            {
                SQL += ComNum.VBLF + "  And Amt4 > 0                                                ";
            }

            if (txtPart.Text.Length > 0)
            {
                SQL += ComNum.VBLF + "  And Part = '" + txtPart.Text + "'                           ";
            }
            SQL += ComNum.VBLF + "      AND PANO <>'81000004'                                       ";
            SQL += ComNum.VBLF + "Order By Bi, Pano                                                 ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                //if (dt.Rows.Count == 0)
                //{
                //    dt.Dispose();
                //    dt = null;
                //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                //    return;
                //}

                if (dt.Rows.Count > 0)
                {
                    nCount = 0;
                    //nSelect1 = 1;
                    nNum1 = dt.Rows.Count;

                    for (i = 0; i < nNum1; i++)
                    {
                        nCount += 1;

                        StrMiSel[nCount] = dt.Rows[i]["Bi"].ToString().Trim();
                        strMiPano[nCount] = dt.Rows[i]["Pano"].ToString().Trim();
                        strMIName[nCount] = dt.Rows[i]["Sname"].ToString().Trim();
                        strMIGwa[nCount] = dt.Rows[i]["DeptCode"].ToString().Trim();
                        strMiDr[nCount] = dt.Rows[i]["DrCode"].ToString().Trim();

                        nJinruMi[nCount] = 0;

                        if (optSelect10.Checked == true)
                        {
                            nJubsuMi[nCount] = VB.Val(dt.Rows[i]["Amt6"].ToString().Trim());
                            nMiTot[nCount] = VB.Val(dt.Rows[i]["Amt6"].ToString().Trim());
                        }
                        else if (optSelect11.Checked == true)
                        {
                            nJubsuMi[nCount] = VB.Val(dt.Rows[i]["Amt4"].ToString().Trim());
                            nMiTot[nCount] = VB.Val(dt.Rows[i]["Amt4"].ToString().Trim());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            //*-----( 수납자료중 미수내역 Select )---------------*
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                ";
            SQL += ComNum.VBLF + "  O.BI, O.Pano, M.Sname, O.DeptCode, O.Drcode,                                        ";
            SQL += ComNum.VBLF + "  SUM(O.Amt1+O.Amt2) JUBMI                                                            ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "Bas_Patient M, " + ComNum.DB_PMPA + "Opd_Slip O            ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                             ";
            SQL += ComNum.VBLF + "      AND O.ActDate = TO_DATE('" + strDate + "','YYYY-MM-DD')                         ";
            SQL += ComNum.VBLF + "      And O.Pano = M.Pano                                                             ";
            if (optSelect10.Checked == true)
            {
                SQL += ComNum.VBLF + "  And O.Bun = '96'                                                                ";
            }
            else if (optSelect11.Checked == true)
            {
                SQL += ComNum.VBLF + "  And O.Bun = '98'                                                                ";
            }
            if (txtPart.Text != "" && !(VB.IsNull(txtPart.Text)))
            {
                SQL += ComNum.VBLF + "  And O.Part = '" + txtPart.Text + "'                                             ";
            }
            SQL += ComNum.VBLF + "      AND O.SeqNo NOT IN  ('0','-1')                                                  ";   //0:접수비미수,-1:예약 중복산정으로 제외
            SQL += ComNum.VBLF + "      AND O.PANO <>'81000004'";
            SQL += ComNum.VBLF + "Group By O.Bi, O.Pano, M.Sname, O.Deptcode, O.DrCode                                  ";
            SQL += ComNum.VBLF + "Order By O.Bi, O.Pano, M.Sname, O.Deptcode, O.DrCode                                  ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                //if (dt.Rows.Count == 0)
                //{
                //    dt.Dispose();
                //    dt = null;
                //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                //    return;
                //}

                if (dt.Rows.Count > 0)
                {
                    //nSelect = 1;
                    nNum2 = dt.Rows.Count;

                    for (i = 0; i < nNum2; i++)
                    {
                        nCount += 1;
                        StrMiSel[nCount] = dt.Rows[i]["Bi"].ToString().Trim();
                        strMiPano[nCount] = dt.Rows[i]["Pano"].ToString().Trim();
                        strMIName[nCount] = dt.Rows[i]["Sname"].ToString().Trim();
                        strMIGwa[nCount] = dt.Rows[i]["DeptCode"].ToString().Trim();
                        strMiDr[nCount] = dt.Rows[i]["DrCode"].ToString().Trim();
                        nJinruMi[nCount] = VB.Val(dt.Rows[i]["JUBMI"].ToString().Trim());
                        nMiTot[nCount] = VB.Val(dt.Rows[i]["JUBMI"].ToString().Trim());

                        nJubsuMi[nCount] = 0;

                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            //기타수납에 Y96코드 발생시 - 추가함 2009-07-22 윤조연
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                            ";
            SQL += ComNum.VBLF + "  O.BI, O.Pano, M.Sname, '기타수납' AS DeptCode, 'ETC' AS Drcode,                 ";
            SQL += ComNum.VBLF + "  SUM(O.Amt) JUBMI                                                                ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "Bas_Patient M, " + ComNum.DB_PMPA + "OPD_ETCSLIP O     ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                         ";
            SQL += ComNum.VBLF + "      AND O.BDate = TO_DATE('" + strDate + "','YYYY-MM-DD')                       ";
            SQL += ComNum.VBLF + "      And O.Pano = M.Pano                                                         ";
            if (optSelect10.Checked == true)
            {
                SQL += ComNum.VBLF + "  And O.SUNEXT = 'Y96'                                                        ";
            }
            else if (optSelect11.Checked == true)
            {
                SQL += ComNum.VBLF + "  And O.Bun = '98'                                                            ";
            }
            if (txtPart.Text != "" && !(VB.IsNull(txtPart.Text)))
            {
                SQL += ComNum.VBLF + "  And O.Part = '" + txtPart.Text + "'                                         ";
            }
            SQL += ComNum.VBLF + "      AND O.PANO <>'81000004'                                                     ";
            SQL += ComNum.VBLF + "Group By O.Bi, O.Pano, M.Sname                                                    ";
            SQL += ComNum.VBLF + "Order By O.Bi, O.Pano, M.Sname                                                    ";

            try
            {

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                //if (dt.Rows.Count == 0)
                //{
                //    dt.Dispose();
                //    dt = null;
                //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                //    return;
                //}

                if (dt.Rows.Count > 0)
                {
                    //nSelect = 1;
                    nNum3 = dt.Rows.Count;

                    for (i = 0; i < nNum3; i++)
                    {
                        nCount += 1;
                        StrMiSel[nCount] = dt.Rows[i]["Bi"].ToString().Trim();
                        strMiPano[nCount] = dt.Rows[i]["Pano"].ToString().Trim();
                        strMIName[nCount] = dt.Rows[i]["Sname"].ToString().Trim();
                        strMIGwa[nCount] = dt.Rows[i]["DeptCode"].ToString().Trim();
                        strMiDr[nCount] = dt.Rows[i]["DrCode"].ToString().Trim();
                        nJinruMi[nCount] = VB.Val(dt.Rows[i]["JUBMI"].ToString().Trim());
                        nMiTot[nCount] = VB.Val(dt.Rows[i]["JUBMI"].ToString().Trim());

                        nJinruMi[nCount] = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            if (nNum1 == 0 && nNum2 == 0 && nNum3 == 0)
            {
                ComFunc.MsgBox("데이터가 없습니다.");
            }

            btnPrint.Focus();
        }
        private void ssList_PrintHeaderFooterArea(object sender, PrintHeaderFooterAreaEventArgs e)
        {
            Pen cPen = new Pen(Color.Black);
            cPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            cPen.Width = 3;
            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Far;

            if (e.IsHeader == true)
            {
                #region 칸 그리기
                e.Graphics.DrawRectangle(new Pen(Brushes.Black, 1), 850, 90, 220, 70);
                e.Graphics.DrawRectangle(new Pen(Brushes.Black, 1), 850, 90, 30, 70);
                e.Graphics.DrawRectangle(new Pen(Brushes.Black, 1), 880, 115, 190, 45);
                e.Graphics.DrawRectangle(new Pen(Brushes.Black, 1), 940, 90, 65, 70);


                #endregion

                #region 칸안에 글
                e.Graphics.DrawString("결", new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 875, 93, drawFormat);
                e.Graphics.DrawString("재", new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 875, 135, drawFormat);
                e.Graphics.DrawString("담  당", new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 937, 93, drawFormat);
                e.Graphics.DrawString("계  장", new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 1000, 93, drawFormat);
                e.Graphics.DrawString("팀  장", new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 1060, 93, drawFormat);
                #endregion


                #region 작성자 
                drawFormat.Alignment = StringAlignment.Far;
               // e.Graphics.DrawString("작성자 : " + clsType.User.UserName, new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 185, 103, drawFormat);
               // e.Graphics.DrawString("출력시간 : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm"), new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 275, 123, drawFormat);
                e.Graphics.DrawString("작성자 : " + clsType.User.UserName, new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 185, 103, drawFormat);
                e.Graphics.DrawString("작업일자 : " + dtpDate.Value.ToString("yyyy-MM-dd"), new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 233, 123, drawFormat);
                e.Graphics.DrawString("출력시간 : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm"), new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 275, 143, drawFormat);

                e.Graphics.DrawString("Page : " + e.PageNumber, new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 135, 163, drawFormat);
                #endregion
            }
        }
        void Print_Move()
        {
            int i = 0;
            int num = 0;
            string SSel = "";
            string SaveSel = "";
            string SPano = "";
            string SGwa = "";
            string SGwaName = "";
            string SDrCode = "";
            string SDrName = "";
            string SSelSave = "";

            double nJubSo = 0;
            double nJinSo = 0;
            double nMiSo = 0;

            string strGelCode = "";
            string strBohun = "";
            string strComment = "";
            string strDeptCode = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;


            SSelSave = "";
            //nFlag = 0; // false

            if (nCount == 0)
            {
                return;
            }

            for (i = 1; i <= nCount; i++)
            {
                if (nJubsuMi[i] != 0 || nJinruMi[i] != 0)
                {
                    strComment = "";
                    strGelCode = "";
                    strBohun = "";

                    num += 1;
                    SPano = strMiPano[i].Trim();
                    strDeptCode = strMIGwa[i].Trim();

                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                                        ";
                    SQL += ComNum.VBLF + "  Bohun,GelCode                                               ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "Opd_master                         ";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
                    SQL += ComNum.VBLF + "      AND Pano = '" + SPano + "'                              ";
                    SQL += ComNum.VBLF + "      AND Actdate = TO_DATE('" + strDate + "','YYYY-MM-DD')   ";
                    SQL += ComNum.VBLF + "      AND DeptCode ='" + strDeptCode + "'                     ";

                    try
                    {
                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        //if (dt.Rows.Count == 0)
                        //{
                        //    dt.Dispose();
                        //    dt = null;
                        //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                        //    return;
                        //}

                        if (dt.Rows.Count > 0)
                        {
                            strGelCode = dt.Rows[0]["GelCode"].ToString().Trim();
                            strBohun = dt.Rows[0]["Bohun"].ToString().Trim();
                        }
                    }
                    catch (Exception ex)
                    {
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    }
                    dt.Dispose();
                    dt = null;

                    //nFlag = False
                    SSel = StrMiSel[i].Trim();
                    SPano = strMiPano[i].Trim();
                    SGwa = strMIGwa[i].Trim();
                    SDrCode = strMiDr[i].Trim();
                    strSelect = StrMiSel[i].Trim();
                    strComment = "";
                    if (strBohun == "1")
                    {
                        strComment = "보훈청 미수";
                    }

                    else if (strBohun == "2")
                    {
                        strComment = "시각장애자 미수";
                    }

                    else if (String.Compare(strGelCode, "0") > 0)
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT                                    ";
                        SQL += ComNum.VBLF + "  MiaName                                 ";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "Bas_Mia        ";
                        SQL += ComNum.VBLF + "WHERE MiaCode = '" + strGelCode + "'      ";

                        try
                        {
                            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            //if (dt.Rows.Count == 0)
                            //{
                            //    dt.Dispose();
                            //    dt = null;
                            //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                            //    return;
                            //}

                            if (dt.Rows.Count > 0)
                            {
                                strComment = dt.Rows[0]["MiaName"].ToString().Trim();
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

                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                    ";
                    SQL += ComNum.VBLF + "  DeptNamek                               ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "Bas_ClinicDept ";
                    SQL += ComNum.VBLF + "WHERE DeptCode = '" + SGwa + "'           ";

                    try
                    {
                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        //if (dt.Rows.Count == 0)
                        //{
                        //    dt.Dispose();
                        //    dt = null;
                        //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                        //    return;
                        //}

                        if (dt.Rows.Count > 0)
                        {
                            SGwaName = dt.Rows[0]["DeptNamek"].ToString().Trim();
                        }

                    }
                    catch (Exception ex)
                    {
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    }
                    dt.Dispose();
                    dt = null;

                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                     ";
                    SQL += ComNum.VBLF + "  DrName                                   ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "Bas_Doctor      ";
                    SQL += ComNum.VBLF + "WHERE DrCode = '" + SDrCode + "'           ";

                    try
                    {
                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        //if (dt.Rows.Count == 0)
                        //{
                        //    dt.Dispose();
                        //    dt = null;
                        //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                        //    return;
                        //}

                        if (dt.Rows.Count > 0)
                        {
                            SDrName = dt.Rows[0]["DrName"].ToString().Trim();
                        }

                    }
                    catch (Exception ex)
                    {
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    }
                    dt.Dispose();
                    dt = null;

                    if (SGwa == "기타수납")
                    {
                        SGwaName = "기타수납";
                        SDrName = "기타";
                    }

                    if (optSelect10.Checked == true && chkRemark.Checked == true)
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT                                                        ";
                        SQL += ComNum.VBLF + "  REMARK                                                      ";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP                      ";
                        SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
                        SQL += ComNum.VBLF + "      AND PANO ='" + SPano + "'                               ";
                        SQL += ComNum.VBLF + "      AND GUBUN1 ='1'                                         ";
                        SQL += ComNum.VBLF + "      AND BDATE =TO_DATE('" + strDate + "','YYYY-MM-DD')      ";
                        SQL += ComNum.VBLF + "      AND SUBSTR(MISUdtL,2,2) ='" + strDeptCode + "'          ";
                        SQL += ComNum.VBLF + "      AND SUBSTR(MISUdtL,1,1) ='O'                            ";
                        SQL += ComNum.VBLF + "      AND AMT = " + nMiTot[i] + "                             ";

                        try
                        {
                            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            //if (dt.Rows.Count == 0)
                            //{
                            //    dt.Dispose();
                            //    dt = null;
                            //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                            //    return;
                            //}

                            if (dt.Rows.Count > 0)
                            {
                                strComment += " " + dt.Rows[0]["REMARK"].ToString().Trim();
                                strComment = strComment.Trim();
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

                    ssList_Sheet1.Rows.Count = num;

                    if (SaveSel == SSel)
                    {
                        ssList_Sheet1.Cells[num - 1, 0].Text = "";
                        ssList_Sheet1.Cells[num - 1, 1].Text = SPano;
                        ssList_Sheet1.Cells[num - 1, 2].Text = strMIName[i];
                        ssList_Sheet1.Cells[num - 1, 3].Text = SGwaName;
                        ssList_Sheet1.Cells[num - 1, 4].Text = SDrName;
                        ssList_Sheet1.Cells[num - 1, 5].Text = String.Format("{0:###,###,##0}", nJubsuMi[i]);
                        ssList_Sheet1.Cells[num - 1, 6].Text = String.Format("{0:###,###,##0}", nJinruMi[i]);
                        ssList_Sheet1.Cells[num - 1, 7].Text = String.Format("{0:###,###,##0}", nMiTot[i]);
                        ssList_Sheet1.Cells[num - 1, 8].Text = strComment;

                        nJubSo += nJubsuMi[i];
                        nJubSuTot += nJubsuMi[i];

                        nJinSo += nJinruMi[i];
                        nJinRuTot += nJinruMi[i];

                        nMiSo += nJubsuMi[i] + nJinruMi[i];
                        nTotTotAmt += (nJubsuMi[i] + nJinruMi[i]);
                    }

                    else if (i == 1)
                    {
                        SaveSel = SSel;
                        MisuReportGubun();

                        ssList_Sheet1.Cells[num - 1, 0].Text = StrGubun1;
                        ssList_Sheet1.Cells[num - 1, 1].Text = SPano;
                        ssList_Sheet1.Cells[num - 1, 2].Text = strMIName[i];
                        ssList_Sheet1.Cells[num - 1, 3].Text = SGwaName;
                        ssList_Sheet1.Cells[num - 1, 4].Text = SDrName;
                        ssList_Sheet1.Cells[num - 1, 5].Text = String.Format("{0:###,###,##0}", nJubsuMi[i]);
                        ssList_Sheet1.Cells[num - 1, 6].Text = String.Format("{0:###,###,##0}", nJinruMi[i]);
                        ssList_Sheet1.Cells[num - 1, 7].Text = String.Format("{0:###,###,##0}", nMiTot[i]);
                        ssList_Sheet1.Cells[num - 1, 8].Text = strComment;

                        nJubSo += nJubsuMi[i];
                        nJubSuTot += nJubsuMi[i];

                        nJinSo += nJinruMi[i];
                        nJinRuTot += nJinruMi[i];

                        nMiSo += nJubsuMi[i] + nJinruMi[i];
                        nTotTotAmt += (nJubsuMi[i] + nJinruMi[i]);
                    }

                    else if (num == 1)
                    {
                        strSelect = SSel;
                        SSelSave = SSel;
                        MisuReportGubun();

                        ssList_Sheet1.Cells[num - 1, 0].Text = StrGubun1;
                        ssList_Sheet1.Cells[num - 1, 1].Text = SPano;
                        ssList_Sheet1.Cells[num - 1, 2].Text = strMIName[i];
                        ssList_Sheet1.Cells[num - 1, 3].Text = SGwaName;
                        ssList_Sheet1.Cells[num - 1, 4].Text = SDrName;
                        ssList_Sheet1.Cells[num - 1, 5].Text = String.Format("{0:###,###,##0}", nJubsuMi[i]);
                        ssList_Sheet1.Cells[num - 1, 6].Text = String.Format("{0:###,###,##0}", nJinruMi[i]);
                        ssList_Sheet1.Cells[num - 1, 7].Text = String.Format("{0:###,###,##0}", nMiTot[i]);
                        ssList_Sheet1.Cells[num - 1, 8].Text = strComment;

                        nJubSo += nJubsuMi[i];
                        nJubSuTot += nJubsuMi[i];

                        nJinSo += nJinruMi[i];
                        nJinRuTot += nJinruMi[i];

                        nMiSo += nJubsuMi[i] + nJinruMi[i];
                        nTotTotAmt += (nJubsuMi[i] + nJinruMi[i]);
                    }

                    else if (SaveSel != SSel && i != 0 && num > 1)
                    {
                        SaveSel = SSel;
                        strSelect = SSel;

                        ssList_Sheet1.Cells[num - 1, 0].Text = "소     계";
                        ssList_Sheet1.Cells[num - 1, 1].Text = "";
                        ssList_Sheet1.Cells[num - 1, 2].Text = "";
                        ssList_Sheet1.Cells[num - 1, 3].Text = "";
                        ssList_Sheet1.Cells[num - 1, 4].Text = "";
                        ssList_Sheet1.Cells[num - 1, 5].Text = String.Format("{0:###,###,##0}", nJubSo);
                        ssList_Sheet1.Cells[num - 1, 6].Text = String.Format("{0:###,###,##0}", nJinSo);
                        ssList_Sheet1.Cells[num - 1, 7].Text = String.Format("{0:###,###,##0}", nMiSo);

                        ssList_Sheet1.Rows[num - 1].Border = lineBorder;

                        

                        nJubSo = 0;
                        nJinSo = 0;
                        nMiSo = 0;

                        num += 1;
                        ssList_Sheet1.Rows.Count = num;
                        MisuReportGubun();

                        ssList_Sheet1.Cells[num - 1, 0].Text = StrGubun1;
                        ssList_Sheet1.Cells[num - 1, 1].Text = SPano;
                        ssList_Sheet1.Cells[num - 1, 2].Text = strMIName[i];
                        ssList_Sheet1.Cells[num - 1, 3].Text = SGwaName;
                        ssList_Sheet1.Cells[num - 1, 4].Text = SDrName;
                        ssList_Sheet1.Cells[num - 1, 5].Text = String.Format("{0:###,###,##0}", nJubsuMi[i]);
                        ssList_Sheet1.Cells[num - 1, 6].Text = String.Format("{0:###,###,##0}", nJinruMi[i]);
                        ssList_Sheet1.Cells[num - 1, 7].Text = String.Format("{0:###,###,##0}", nMiTot[i]);
                        ssList_Sheet1.Cells[num - 1, 8].Text = strComment;

                        nJubSo += nJubsuMi[i];
                        nJubSuTot += nJubsuMi[i];

                        nJinSo += nJinruMi[i];
                        nJinRuTot += nJinruMi[i];

                        nMiSo += nJubsuMi[i] + nJinruMi[i];
                        nTotTotAmt += (nJubsuMi[i] + nJinruMi[i]);

                    }
                    
                }
            }

            if (num > 0)
            {
                num += 1;
                ssList_Sheet1.Rows.Count = num;

                ssList_Sheet1.Cells[num - 1, 0].Text = "소     계";
                ssList_Sheet1.Cells[num - 1, 1].Text = "";
                ssList_Sheet1.Cells[num - 1, 2].Text = "";
                ssList_Sheet1.Cells[num - 1, 3].Text = "";
                ssList_Sheet1.Cells[num - 1, 4].Text = "";
                ssList_Sheet1.Cells[num - 1, 5].Text = String.Format("{0:###,###,##0}", nJubSo);
                ssList_Sheet1.Cells[num - 1, 6].Text = String.Format("{0:###,###,##0}", nJinSo);
                ssList_Sheet1.Cells[num - 1, 7].Text = String.Format("{0:###,###,##0}", nMiSo);

                ssList_Sheet1.Rows[num - 1].Border = lineBorder;
                nJubSo = 0;
                nJinSo = 0;
                nMiSo = 0;

                num += 1;
                ssList_Sheet1.Rows.Count = num;

                ssList_Sheet1.Cells[num - 1, 0].Text = "총     계";
                ssList_Sheet1.Cells[num - 1, 1].Text = "";
                ssList_Sheet1.Cells[num - 1, 2].Text = "";
                ssList_Sheet1.Cells[num - 1, 3].Text = "";
                ssList_Sheet1.Cells[num - 1, 4].Text = "";
                ssList_Sheet1.Cells[num - 1, 5].Text = String.Format("{0:###,###,##0}", nJubSuTot);
                ssList_Sheet1.Cells[num - 1, 6].Text = String.Format("{0:###,###,##0}", nJinRuTot);
                ssList_Sheet1.Cells[num - 1, 7].Text = String.Format("{0:###,###,##0}", nTotTotAmt);

                

                nJubSuTot = 0;
                nJinRuTot = 0;
                nTotTotAmt = 0;
            }
        }

        void txtPart_Enter(object sender, EventArgs e)
        {
            TextBox tP = sender as TextBox;
            tP.SelectionStart = 0;
            tP.SelectionLength = tP.Text.Length;
        }

        void txtPart_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.A)
                {
                    txtPart.SelectAll();
                }
            }
        }

        void txtPart_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
                txtPart.Text = txtPart.Text.ToUpper();
            }
        }


    }
}
