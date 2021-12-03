using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComLibB;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewTreatment.cs
    /// Description     : 진료내역 조회
    /// Author          : 박창욱
    /// Create Date     : 2017-11-09
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\IPD\Frm진료내역조회.FRM(Frm진료내역조회.frm) >> frmPmpaViewTreatment.cs 폼이름 재정의" />
    public partial class frmPmpaViewTreatment : Form
    {
        #region 변수선언 및 클래스 선언
        
        string GstrPANO = "";
        double GnIPDNO = 0;
        
        double G7AMT = 0;
        double G7TAMT = 0;
        double GnbAmt_new = 0;
        string FstrPassGrade = "";
        string FstrPano = string.Empty;
        int FnJobNo = 0;
        int FnCnt_SU = 0;
        double FnTRSNO = 0;
        double nGETcount = 0;
        double[] nAmAMT = new double[61];     //Master 금액 Setting
        string strFnu = "";
        string strTnu = "";
        string strSDate = "";
        string strEdate = "";
        
        string FstrChk = "";
        string FstrDrg = "";       //2013-07-08
        string FstrMinActDate = "";
        string FstrView = "";       //2015-01-31
        string[] strIndates = new string[10];
        string[] strSexAge = new string[10];
        string[] strWardRoom = new string[10];
        string[] strAmset1 = new string[10];
        string[] StrGbGamek = new string[10];
        string[] strBohun = new string[10];
        string[] strRowIDs = new string[10];
        
        clsIpdAcct cIAcct = new clsIpdAcct();
        clsPmpaFunc cpf = new clsPmpaFunc();
        DRG DRG = new DRG();
        #endregion
        
        public frmPmpaViewTreatment()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        public frmPmpaViewTreatment(string strPano, long nIPDNO)
        {
            InitializeComponent();
            SetControl();
            SetEvent();

            FstrPano = strPano;
            GnIPDNO = nIPDNO;
        }

        void SetEvent()
        {
            this.Load                           += new EventHandler(eFormLoad);
            this.Activated                      += new EventHandler(eFormActivated);
            this.cboTewon.SelectedIndexChanged  += new EventHandler(eCboChange);
            this.cboJob.SelectedIndexChanged    += new EventHandler(eCboChange);
            this.cboGbn.SelectedIndexChanged    += new EventHandler(eCboChange);
            this.cboFnu.SelectedIndexChanged    += new EventHandler(eCboChange);
            this.cboTnu.SelectedIndexChanged    += new EventHandler(eCboChange);

            this.btnSearch.Click                += new EventHandler(eBtnClick);
            this.btnView.Click                  += new EventHandler(eBtnClick);
            this.btnAmtSet.Click                += new EventHandler(eBtnClick);
            this.btnSave.Click                  += new EventHandler(eBtnClick);
            this.btnPrint.Click                 += new EventHandler(eBtnClick);
            this.btnExit.Click                  += new EventHandler(eBtnClick);

            this.txtPano.KeyPress               += new KeyPressEventHandler(eKeyPress);
            this.dtpOutDate.KeyPress            += new KeyPressEventHandler(eKeyPress);

            this.ssTrans.CellDoubleClick        += new CellClickEventHandler(eSpdDblClick);
            this.ssList.CellDoubleClick         += new CellClickEventHandler(eSpdDblClick);
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (sender == txtPano)
                {
                    if (txtPano.Text.Trim() == "")
                    {
                        return;
                    }

                    if (cboJob.Text.Trim() == "등록번호")
                    {
                        txtPano.Text = VB.Format(VB.Val(txtPano.Text), "00000000");
                    }

                    btnSearch.Focus();

                }
                else if (sender == dtpOutDate)
                {
                    btnSearch.Focus();
                }
            }
        }

        void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            int nCol, nRow;

            Cursor.Current = Cursors.WaitCursor;
            
            nCol = e.Column;
            nRow = e.Row;

            if (sender == ssTrans)
            {
                eSpdDblClick_ssTrans(nRow, nCol);
            }
            else if (sender == ssList)
            {
                eSpdDblClick_ssList(nRow, nCol);
            }
            Cursor.Current = Cursors.Default;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnView)
            {
                View_Data();
            }
            else if (sender == btnSearch)
            {
                Search_Data();
            }
            else if (sender == btnAmtSet)
            {
                AmtSet();   //금액맞춤
            }
            else if (sender == btnSave)
            {
                eSave(clsDB.DbCon);
            }
            else if (sender == btnPrint)
            {
                ePrint();
            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
        }

        void ePrint()
        {
            int i = 0;
            int k = 0;
            int nPage = 0;
            double nAmt1 = 0;
            double nAmt2 = 0;
            string strMsg = "";
            string strFont1 = "";
            string strFont2 = "";
            string strFont3 = "";
            string strHead1 = "";
            string strHead2 = "";
            string strFoot = "";
            string strYN = "";
            string strAmt0 = "";

            clsPmpaFunc cpf = new clsPmpaFunc();

            ssView_Sheet1.Columns[8].Visible = false;
            ssView_Sheet1.Columns[9].Visible = false;
            ssView_Sheet1.Columns[10].Visible = false;
            ssView_Sheet1.Columns[11].Visible = false;
            ssView_Sheet1.Columns[12].Visible = false;
            ssView_Sheet1.Columns[13].Visible = false;
            ssView_Sheet1.Columns[14].Visible = false;
            ssView_Sheet1.Columns[21].Visible = false;
            ssView_Sheet1.Columns[26].Visible = false;
            ssView_Sheet1.Columns[27].Visible = false;


            if (ComFunc.MsgBoxQ("심사 PART 프린터를 이용하시겠습니까?", "", MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                strYN = "YES";
            }
            else
            {
                strYN = "";
            }

            if (ComFunc.MsgBoxQ("금액이 0인 항목을 제외하고 출력하시겠습니까?", "", MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                strAmt0 = "YES";
            }
            else
            {
                strAmt0 = "";
            }

            if (clsPmpaType.IMST.Pano == "")
            {
                ComFunc.MsgBox("먼저 환자를 선택 후 인쇄하십시오");
                return;
            }

            clsPublic.GstrRetValue = clsPmpaType.IMST.Pano + "^^" + "입원^^" + clsPmpaType.TIT.InDate + "~" + clsPmpaType.TIT.OutDate + "^^" + "01^^";

            frmSetPrintInfo fx = new frmSetPrintInfo();
            fx.ShowDialog();

            if (VB.Pstr(clsPublic.GstrRetValue, "^^", 1).Trim() != "OK")
            {
                return;
            }

            nPage = Convert.ToInt32(VB.Pstr(clsPublic.GstrRetValue, "^^", 2).Trim());
            clsPublic.GstrRetValue = "";


            if (strYN == "YES")
            {
                for (i = 1; i <= nPage; i++)
                {
                    #region CHECK_AMT0_PRINT

                    if (strAmt0 == "YES")
                    {
                        for (k = 0; k <= ssView_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); k++)
                        {
                            nAmt1 = VB.Val(ssView_Sheet1.Cells[k, 7].Text.Replace(",", ""));
                            nAmt2 = VB.Val(ssView_Sheet1.Cells[k, 8].Text.Replace(",", ""));
                            if (nAmt1 == 0 && nAmt2 == 0)
                            {
                                ssView_Sheet1.Rows[k].Visible = false;
                            }
                        }
                    }
                    else
                    {
                        for (k = 0; k <= ssView_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); k++)
                        {
                            nAmt1 = VB.Val(ssView_Sheet1.Cells[k, 7].Text.Replace(",", ""));
                            nAmt2 = VB.Val(ssView_Sheet1.Cells[k, 8].Text.Replace(",", ""));
                            if (nAmt1 == 0 && nAmt2 == 0)
                            {
                                ssView_Sheet1.Rows[k].Visible = true;
                            }
                        }
                    }

                    #endregion


                    #region Serial_Printer_Print

                    strFont1 = "/fn\"굴림체\" /fz\"16\" /fb1 /fi0 /fu0 /fk0 /fs1";
                    strFont2 = "/fn\"굴림체\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";
                    strFont3 = "/fn\"굴림체\" /fz\"9\" /fb0 /fi0 /fu0 /fk0 /fs3";
                    strHead1 = "/f1" + VB.Space(18) + "입 원 진 료 비   세 부 산 정   내 역" + "/n/n";
                    strHead2 = "/l/f2" + "등록번호: " + clsPmpaType.IMST.Pano +
                                        " 환자성명: " + cpf.Get_BasPatient(clsDB.DbCon, clsPmpaType.IMST.Pano).Rows[0]["Sname"].ToString().Trim() + VB.Space(4);
                    //strHead2 += "진료과: " + clsPmpaType.TIT.DeptCode + " " + clsVbfunc.GetBASDoctorName(clsDB.DbCon, clsPmpaType.TIT.DrCode) + "/n/n";
                    strHead2 += VB.Space(8) + "진료기간: " + strSDate + " ~ " + strEdate + VB.Space(10);
                    strHead2 += "병실: " + clsPmpaType.IMST.RoomCode + VB.Space(8);
                    strHead2 += "환자구분: " + clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_환자종류", clsPmpaType.TIT.Bi) + VB.Space(4);
                    //strHead2 += "출력일자: " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
                    //Print Flooter  지정
                    strFoot = "/f3/n" + VB.Space(62) + "신청인(      ) 의 요청에 따라 진료비 계산서 영수증 세부산정내역을 발급합니다. /n";
                    strFoot += "/l/f3" + VB.Space(88) + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D") + "/n";
                    strFoot += "/l/f3" + VB.Space(10) + "요양기관명칭  포항성모병원" + VB.Space(90) + "대표자  최 순 호 /n";
                    strFoot += "/l/f3" + VB.Space(90) + "PAGE : " + "/p";
                    
                    //Print Body
                    ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
                    ssView_Sheet1.PrintInfo.Footer = strFont3 + strFoot;
                    ssView_Sheet1.PrintInfo.Margin.Left = 0;
                    ssView_Sheet1.PrintInfo.Margin.Right = 0;
                    ssView_Sheet1.PrintInfo.Margin.Top = 50;
                    ssView_Sheet1.PrintInfo.Margin.Bottom = 100;
                    ssView_Sheet1.PrintInfo.ZoomFactor = 0.85f;
                    ssView_Sheet1.PrintInfo.ShowColumnHeader = PrintHeader.Show;
                    ssView_Sheet1.PrintInfo.ShowRowHeader = PrintHeader.Hide;
                    ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
                    ssView_Sheet1.PrintInfo.ShowColor = true;
                    ssView_Sheet1.PrintInfo.Centering = Centering.Horizontal;
                    ssView_Sheet1.PrintInfo.ShowBorder = true;
                    ssView_Sheet1.PrintInfo.ShowGrid = false;
                    ssView_Sheet1.PrintInfo.ShowShadows = false;
                    ssView_Sheet1.PrintInfo.UseMax = false;
                    ssView_Sheet1.PrintInfo.PrintType = PrintType.All;
                    ssView_Sheet1.PrintInfo.UseSmartPrint = false;
                    ssView_Sheet1.PrintInfo.ShowPrintDialog = false;
                    ssView_Sheet1.PrintInfo.Preview = false;
                    ssView.PrintSheet(0);
                    
                    //ssView_Sheet1.Columns[21].Visible = true;

                    #endregion

                }

            }
            else
            {
                #region CHECK_AMT0_PRINT

                if (strAmt0 == "YES")
                {
                    for (k = 0; k <= ssView_Sheet1.GetLastNonEmptyRow(NonEmptyItemFlag.Data); k++)
                    {
                        nAmt1 = VB.Val(ssView_Sheet1.Cells[k, 7].Text.Replace(",", ""));
                        nAmt2 = VB.Val(ssView_Sheet1.Cells[k, 8].Text.Replace(",", ""));
                        if (nAmt1 == 0 && nAmt2 == 0)
                        {
                            ssView_Sheet1.Rows[k].Visible = false;
                        }
                    }
                }
                else
                {
                    for (k = 0; k <= ssView_Sheet1.GetLastNonEmptyRow(NonEmptyItemFlag.Data); k++)
                    {
                        nAmt1 = VB.Val(ssView_Sheet1.Cells[k, 7].Text.Replace(",", ""));
                        nAmt2 = VB.Val(ssView_Sheet1.Cells[k, 8].Text.Replace(",", ""));
                        if (nAmt1 == 0 && nAmt2 == 0)
                        {
                            ssView_Sheet1.Rows[k].Visible = true;
                        }
                    }
                }

                #endregion


                #region Hard_printer_Print

                if (clsPmpaType.IMST.GbSTS == "6" || clsPmpaType.IMST.GbSTS == "7")
                {
                    strMsg = "원무과에서 이미 퇴원처리를 하였습니다.";
                    strMsg += ComNum.VBLF + "퇴원취소 가능여부를 담당자와 상의 하십시오.";
                    ComFunc.MsgBox(strMsg);
                    return;
                }
                else if (clsPmpaType.IMST.GbSTS == "4" || clsPmpaType.IMST.GbSTS == "5")
                {
                    strMsg = "이미 심사완료를 하였습니다.";
                    strMsg += ComNum.VBLF + "심사완료를 심사중으로 처리 후 ";
                    strMsg += ComNum.VBLF + "대조리스트를 인쇄하시겠습니까?";
                    if (ComFunc.MsgBoxQ(strMsg) == DialogResult.No)
                    {
                        return;
                    }
                }

                #endregion
            }
            
        }

        void eCboChange(object sender, EventArgs e)
        {
            string strGbn = string.Empty;

            if (sender == cboJob)
            {
                #region //검색방법 설정
                if (cboJob.Text.ToString() == "등록번호" || cboJob.Text.ToString() == "환자성명")
                {
                    dtpOutDate.Visible = false;;
                    txtPano.Visible = true;
                    txtPano.BringToFront();
                    txtPano.Focus();
                }
                else
                {
                    txtPano.Visible = false;
                    dtpOutDate.Visible = true;
                    dtpOutDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
                    dtpOutDate.BringToFront();
                    dtpOutDate.Focus();
                }
                #endregion
            }
            else if (sender == cboGbn)
            {
                #region //조회구분 설정
                strGbn = cboGbn.Text.Trim().Substring(0, 1);

                if (strGbn == "1")
                {
                    this.Text = "진료내역 조회 : 누적별합산조회(원무팀공용)";
                    ssView_Sheet1.ColumnHeader.Cells[0, 1].Text = "발생일자";
                    ssView_Sheet1.ColumnHeader.Cells[0, 2].Text = "표준코드";
                    ssView_Sheet1.ColumnHeader.Cells[0, 10].Text = "선택";
                    FnJobNo = 1;
                    btnSave.Enabled = false;
                    lblPrice.Enabled = false;
                    btnAmtSet.Enabled = false;
                }
                else if (strGbn == "2")
                {
                    this.Text = "진료내역 조회 : 항목별상세조회";
                    ssView_Sheet1.ColumnHeader.Cells[0, 1].Text = "수가코드";
                    ssView_Sheet1.ColumnHeader.Cells[0, 2].Text = "발생일자";
                    ssView_Sheet1.ColumnHeader.Cells[0, 10].Text = "선택";
                    FnJobNo = 2;
                    btnSave.Enabled = true;
                    lblPrice.Enabled = true;
                    btnAmtSet.Enabled = true;
                }
                else if (strGbn == "3")
                {
                    this.Text = "진료내역 조회 : 일자별상세조회";
                    ssView_Sheet1.ColumnHeader.Cells[0, 1].Text = "발생일자";
                    ssView_Sheet1.ColumnHeader.Cells[0, 2].Text = "수가코드";
                    ssView_Sheet1.ColumnHeader.Cells[0, 10].Text = "응급";
                    FnJobNo = 3;
                    btnSave.Enabled = false;
                    lblPrice.Enabled = false;
                    btnAmtSet.Enabled = false;
                }
                else if (strGbn == "4")
                {
                    this.Text = "진료내역 조회 : 누적별조회(심사팀용)";
                    ssView_Sheet1.ColumnHeader.Cells[0, 1].Text = "수가코드";
                    ssView_Sheet1.ColumnHeader.Cells[0, 2].Text = "표준코드";
                    ssView_Sheet1.ColumnHeader.Cells[0, 10].Text = "응급";
                    FnJobNo = 4;
                    btnSave.Enabled = false;
                    lblPrice.Enabled = false;
                    btnAmtSet.Enabled = false;
                }
                else if (strGbn == "5")
                {
                    this.Text = "진료내역 조회 : 일자별상세조회 -> 누적별 II";
                    ssView_Sheet1.ColumnHeader.Cells[0, 1].Text = "발생일자";
                    ssView_Sheet1.ColumnHeader.Cells[0, 2].Text = "수가코드";
                    ssView_Sheet1.ColumnHeader.Cells[0, 10].Text = "선택";
                    FnJobNo = 5;
                    btnSave.Enabled = false;
                    lblPrice.Enabled = false;
                    btnAmtSet.Enabled = false;
                }
                else if (strGbn == "6")
                {
                    this.Text = "진료내역 조회 : 누적별합산조회(1회투여량)";
                    ssView_Sheet1.ColumnHeader.Cells[0, 1].Text = "수가코드";
                    ssView_Sheet1.ColumnHeader.Cells[0, 2].Text = "표준코드";
                    ssView_Sheet1.ColumnHeader.Cells[0, 10].Text = "선택";
                    FnJobNo = 6;
                    btnSave.Enabled = false;
                    lblPrice.Enabled = false;
                    btnAmtSet.Enabled = false;
                }
                else if (strGbn == "7")
                {
                    this.Text = "진료내역 조회 : 표준코드조회";
                    ssView_Sheet1.ColumnHeader.Cells[0, 1].Text = "수가코드";
                    ssView_Sheet1.ColumnHeader.Cells[0, 2].Text = "표준코드";
                    ssView_Sheet1.ColumnHeader.Cells[0, 10].Text = "선택";
                    FnJobNo = 7;
                    btnSave.Enabled = false;
                    lblPrice.Enabled = false;
                    btnAmtSet.Enabled = false;
                }
                else if (strGbn == "8")
                {
                    this.Text = "진료내역 조회 : 누적별합산조회(마취합산)";
                    ssView_Sheet1.ColumnHeader.Cells[0, 1].Text = "수가코드";
                    ssView_Sheet1.ColumnHeader.Cells[0, 2].Text = "표준코드";
                    ssView_Sheet1.ColumnHeader.Cells[0, 10].Text = "선택";
                    FnJobNo = 8;
                    btnSave.Enabled = false;
                    lblPrice.Enabled = false;
                    btnAmtSet.Enabled = false;
                }
                #endregion
            }
            else if (sender == cboFnu)
            {
                SelChangeFnu(cboFnu.SelectedIndex);
            }
            else if (sender == cboTnu)
            {
                SelChangeTnu(cboFnu.SelectedIndex, cboTnu.SelectedIndex);
            }
        }

        void SetControl()
        {
            cboGbn.Items.Clear();
            cboGbn.Items.Add("1.누적별 합산(원무팀공용)");
            cboGbn.Items.Add("2.항목별 합산");
            cboGbn.Items.Add("3.일자별 상세");
            cboGbn.Items.Add("4.누적별 (심사팀용)");
            cboGbn.Items.Add("5.일자 누적별 II");
            cboGbn.Items.Add("6.누적별 합산(1투량)");
            cboGbn.Items.Add("7.표준코드 인쇄");
            cboGbn.Items.Add("8.누적별 합산(마취합산)");          

            cboTewon.Items.Clear();
            cboTewon.Items.Add("전체");
            cboTewon.Items.Add("가퇴원");
            cboTewon.Items.Add("퇴원환자");

            cboJob.Items.Clear();
            cboJob.Items.Add("등록번호");
            cboJob.Items.Add("환자성명");
            cboJob.Items.Add("퇴원일자");
        }

        void eFormActivated(object sender, EventArgs e)
        {
            if (clsPublic.GstrPANO != txtPano.Text && clsPublic.GstrPANO !="")
            {
                txtPano.Text = clsPublic.GstrPANO;

                Search_Data();
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            
            if (GstrPANO != "")
            {
                txtPano.Text = GstrPANO;

                Search_Data();
            }

            txtPano.Text = "";
            dtpOutDate.Value = Convert.ToDateTime(strSysDate);
            dtpSDate.Value = Convert.ToDateTime(strSysDate);
            dtpTDate.Value = Convert.ToDateTime(strSysDate);

            cboGbn.SelectedIndex = 0;

            ssView_Sheet1.Columns[15].Visible = false;
            ssView_Sheet1.Columns[16].Visible = false;
            ssView_Sheet1.Columns[17].Visible = false;
            ssView_Sheet1.Columns[18].Visible = false;
            ssView_Sheet1.Columns[19].Visible = false;
            ssView_Sheet1.Columns[20].Visible = false;

            ssView_Sheet1.Columns[22].Visible = false;
            ssView_Sheet1.Columns[23].Visible = false;
            ssView_Sheet1.Columns[24].Visible = false;
            ssView_Sheet1.Columns[25].Visible = false;

            //cboFnu.SelectedIndex = -1;
            //cboTnu.SelectedIndex = -1;

            if (FstrPassGrade != "EDPS")
            {
                btnEtc1.Enabled = false;
                btnEtc2.Enabled = false;
                btnSave.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
            }

            FnJobNo = 1;    //누적별 합산조회

            clsVbfunc.SetBCodeCombo(clsDB.DbCon, cboFnu, "IPD_누적행위구분", 3, true, "N");
            clsVbfunc.SetBCodeCombo(clsDB.DbCon, cboTnu, "IPD_누적행위구분", 3, true, "N");

            Screen_Claer();

            if (GnIPDNO > 0)
            {
                //기본을 일자별 상세조회로
                cboGbn.SelectedIndex = 0;
                FnTRSNO = 0;
                Display_IPD_Master(GnIPDNO);
                GnIPDNO = 0;
            }

            if (FstrPano != "")
            {
                txtPano.Text = FstrPano;
            }

            try
            {
                if (FstrPassGrade.Trim() != "EDPS")
                {
                    ssRate.Enabled = false;

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT BUSE";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "INSA_MST ";
                    SQL = SQL + ComNum.VBLF + " WHERE SABUN = '" + clsType.User.Sabun + "' ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        //보험심사과만 사용
                        if (FnJobNo == 1 && VB.Left(dt.Rows[0]["BUSE"].ToString().Trim(), 4) == "0782")
                        {
                            btnSave.Enabled = true;
                            ssRate.Enabled = true;
                            FstrView = "OK";    //심사과 조회기능 관련
                        }
                        else
                        {
                            btnSave.Enabled = false;
                            ssRate.Enabled = false;
                        }
                    }
                    dt.Dispose();
                    dt = null;
                }

                if (FstrPassGrade.Trim() != "EDPS" && clsType.User.Sabun != "19399")
                {
                    ssRate.Enabled = false;
                }

                //함종현계장 임시 권한
                if (clsType.User.Sabun == "20175")
                {
                    btnSave.Enabled = true;
                    ssRate.Enabled = true;
                    FstrView = "OK";
                }

                cboGbn.SelectedIndex = 0;
                cboTewon.SelectedIndex = 0;
                cboJob.SelectedIndex = 0;

                dtpOutDate.Visible = false;
                txtPano.Visible = true;
                txtPano.BringToFront();

                this.Activate();
                this.txtPano.Focus();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }
        
        void Screen_Claer()
        {
            dtpSDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
            dtpTDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
            
            cboFnu.SelectedIndex = 0;
            //cboTnu.SelectedIndex = 0;
            cboTnu.SelectedIndex = cboTnu.Items.Count - 1;
            strFnu = "";
            strTnu = "";
            txtRemark.Text = "";

            ssInfo_Sheet1.RowCount = 0;
            ssInfo_Sheet1.RowCount = 1;

            ssTrans_Sheet1.RowCount = 0;
            ssTrans_Sheet1.RowCount = 20;

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;

            ssView_Sheet1.Columns[8].Visible = true;
            ssView_Sheet1.Columns[9].Visible = true;
            ssView_Sheet1.Columns[10].Visible = true;
            ssView_Sheet1.Columns[11].Visible = true;
            ssView_Sheet1.Columns[12].Visible = true;
            ssView_Sheet1.Columns[13].Visible = true;
            ssView_Sheet1.Columns[14].Visible = true;
            ssView_Sheet1.Columns[21].Visible = true;
            ssView_Sheet1.Columns[26].Visible = true;
            ssView_Sheet1.Columns[27].Visible = true;
        }

        void AmtSet()
        {
            int i = 0;
            double nAmt = 0;
            double nTAmt = 0;
            double nTAmt2 = 0;

            for (i = 1; i < ssView_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data) + 1; i++)
            {
                if (ssView_Sheet1.Cells[i - 1, 18].BackColor == Color.FromArgb(255, 0, 0))
                {
                    nAmt = VB.Val(VB.TR(ssView_Sheet1.Cells[i - 1, 7].Text, ",", ""));
                    ssView_Sheet1.Cells[i - 1, 18].Text = nAmt.ToString();
                    nAmt = VB.Val(VB.TR(ssView_Sheet1.Cells[i - 1, 8].Text, ",", ""));
                    ssView_Sheet1.Cells[i - 1, 19].Text = nAmt.ToString();
                    ssView_Sheet1.Cells[i - 1, 18].BackColor = Color.FromArgb(255, 0, 255);
                }
            }

            nTAmt = 0;
            nTAmt2 = 0;

            for (i = 1; i < ssView_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data) + 1; i++)
            {
                if (ssView_Sheet1.Cells[i - 1, 15].Text == "1")
                {
                    nTAmt += VB.Val(VB.TR(ssView_Sheet1.Cells[i - 1, 18].Text, ",", ""));
                    nTAmt2 += VB.Val(VB.TR(ssView_Sheet1.Cells[i - 1, 19].Text, ",", ""));
                }
            }

            ssView_Sheet1.Cells[ssView_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data), 18].Text = nTAmt.ToString("###,###,###,##0");
            ssView_Sheet1.Cells[ssView_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data), 19].Text = nTAmt2.ToString("###,###,###,##0");
        }

        void eSave(PsmhDb pDbCon)
        {
            int i = 0;
            string SQL = "";    //Query문
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            int k = 0;
            int nRATE = 0;
            int nNal = 0;
            int nSeqNo = 0;
            double nTAmt = 0;
            double nAmt1 = 0;
            double nAmt2 = 0;  //선택
            double nBoAmt = 0;
            double nQty = 0;
            string strSucode = "";
            string strSuNext = "";
            string strCHK = "";
            string strRemark = "";
            string strGBSELF = "";
            string strBun = "";
            string strNu = "";
            string strBDate = "";
            string strSDate = "";
            string strEdate = "";
            string strRowID = "";

            strRemark = txtRemark.Text.Trim();

            strSDate = dtpSDate.Value.ToString("yyyy-MM-dd");
            strEdate = dtpTDate.Value.ToString("yyyy-MM-dd");

            if (strRemark == "")
            {
                ComFunc.MsgBox("사유가 공란입니다.");
                txtRemark.Focus();
                return;
            }

            k = 0;

            if (ComFunc.MsgBoxQ("선택하신 명세서를 등록하시겠습니까?") == DialogResult.No)
            {
                return;
            }

            nSeqNo = 0;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(pDbCon);

            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ROWID";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_SU_SLIP";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND PANO = '" + clsPmpaType.TIT.Pano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                SQL = SQL + ComNum.VBLF + "   AND TRSNO = " + clsPmpaType.TIT.Trsno + " ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "DELETE " + ComNum.DB_PMPA + "IPD_SU_SLIP ";
                    SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "   AND PANO = '" + clsPmpaType.TIT.Pano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                    SQL = SQL + ComNum.VBLF + "   AND TRSNO = " + clsPmpaType.TIT.Trsno + " ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                dt.Dispose();
                dt = null;

                for (i = 1; i < ssView_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data) + 1; i++)
                {
                    strCHK = ssView_Sheet1.Cells[i - 1, 15].Text.Trim();

                    if (strCHK == "1")
                    {
                        k += 1;

                        strSucode = ssView_Sheet1.Cells[i - 1, 1].Text.Trim();
                        strBDate = Convert.ToDateTime(ssView_Sheet1.Cells[i - 1, 2].Text.Trim()).ToString("yyyy-MM-dd");
                        strGBSELF = ssView_Sheet1.Cells[i - 1, 13].Text.Trim();
                        nQty = VB.Val(VB.Val(ssView_Sheet1.Cells[i - 1, 16].Text.Trim()).ToString("#####0.00"));
                        nNal = Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[i - 1, 17].Text.Trim()).ToString("########0"));
                        nAmt1 = VB.Val(VB.Val(VB.TR(ssView_Sheet1.Cells[i - 1, 18].Text, ",", "")).ToString("########0"));
                        nAmt2 = VB.Val(VB.Val(VB.TR(ssView_Sheet1.Cells[i - 1, 19].Text, ",", "")).ToString("########0"));
                        strSuNext = ssView_Sheet1.Cells[i - 1, 23].Text.Trim();
                        strRowID = ssView_Sheet1.Cells[i - 1, 24].Text;

                        nRATE = 0;

                        nBoAmt = VB.Val(ssView_Sheet1.Cells[i - 1, 19].Text.Trim() == "" ? "0" : VB.Val(ssView_Sheet1.Cells[i - 1, 19].Text.Trim()).ToString("#########0.#"));
                        strBun = ssView_Sheet1.Cells[i - 1, 20].Text.Trim();
                        strNu = ssView_Sheet1.Cells[i - 1, 22].Text.Trim();

                        nTAmt = nAmt1 + nAmt2;

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "IPD_SU_SLIP";
                        SQL = SQL + ComNum.VBLF + "             (WRTNO, SDate, EDate, BDate, IPDNO, TRSNO, PANO, BI, SUCODE, SuNext, BUN, NU,";
                        SQL = SQL + ComNum.VBLF + "              GBSELF, RATE, REMARK, TAMT, amt1, amt2, BOAMT, QTY, NAL, t_ROWID)";
                        SQL = SQL + ComNum.VBLF + "        VALUES (";
                        SQL = SQL + ComNum.VBLF + "                " + nSeqNo + ",";
                        SQL = SQL + ComNum.VBLF + "                 TO_DATE('" + strSDate + "','YYYY-MM-DD') , ";
                        SQL = SQL + ComNum.VBLF + "                 TO_DATE('" + strEdate + "','YYYY-MM-DD') , ";
                        SQL = SQL + ComNum.VBLF + "                 TO_DATE('" + strBDate + "','YYYY-MM-DD') , ";
                        SQL = SQL + ComNum.VBLF + "                  " + clsPmpaType.TIT.Ipdno + ", " + clsPmpaType.TIT.Trsno + ",";
                        SQL = SQL + ComNum.VBLF + "                 '" + clsPmpaType.TIT.Pano + "', '" + clsPmpaType.TIT.Bi + "',";
                        SQL = SQL + ComNum.VBLF + "                 '" + strSucode + "','" + strSuNext + "', '" + strBun + "','" + strNu + "',";
                        SQL = SQL + ComNum.VBLF + "                 '" + strGBSELF + "', " + nRATE + ",";
                        if (k == 1)
                        {
                            SQL = SQL + ComNum.VBLF + " '" + strRemark + "',";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + " '',";
                        }
                        SQL = SQL + ComNum.VBLF + " " + nTAmt + "," + nAmt1 + "," + nAmt2 + ", " + nBoAmt + ", ";
                        SQL = SQL + ComNum.VBLF + " " + nQty + ", " + nNal + ",'" + strRowID + "'  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                clsDB.setCommitTran(pDbCon);
                ComFunc.MsgBox("수동 명세서에 정상적으로 등록 되었습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }
        
        void Search_Data()
        {          
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nRead = 0;
            string strInput = "";   //txtPano, txtSname, dtpOutDate의 값을 저장하기 위한 변수

            if (cboJob.Text == "등록번호" && txtPano.Text.Trim() == "")
            {
                ComFunc.MsgBox("등록번호를 입력해주세요.");
                return;
            }
            else if (cboJob.Text == "환자성명" && txtPano.Text.Trim() == "")
            {
                ComFunc.MsgBox("환자성명을 입력해주세요.");
                return;
            }

            if (cboJob.Text == "등록번호" && txtPano.Text.Trim() != "")
            {
                strInput = ComFunc.LPAD(txtPano.Text, 8, "0");
            }
            else if (cboJob.Text == "환자성명" && txtPano.Text.Trim() != "")
            {
                strInput = txtPano.Text.Trim();
            }
            else if (cboJob.Text == "퇴원일자")
            {
                strInput = dtpOutDate.Value.ToString("yyyy-MM-dd");
            }

            try
            {
                string strTewon = cboTewon.Text.Trim().Substring(0, 1);

                //환자명단을 SELECT
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT IPDNO,  Pano, SName,";
                SQL = SQL + ComNum.VBLF + "       DeptCode, RoomCode, GbSTS,";
                SQL = SQL + ComNum.VBLF + "       Bi, GbDrg, TO_CHAR(InDate,'YYYY-MM-DD') InDate,";
                SQL = SQL + ComNum.VBLF + "       LastTrs";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                if (cboTewon.Text.Trim() == "전체")
                {
                    if (cboJob.Text.Trim() == "등록번호" && strInput != "")
                    {
                        SQL = SQL + ComNum.VBLF + "   AND Pano='" + strInput + "' ";
                    }
                    else if (cboJob.Text.Trim() == "환자성명" && strInput != "")
                    {
                        SQL = SQL + ComNum.VBLF + "   AND SName LIKE '%" + strInput + "%' ";
                    }

                    SQL = SQL + ComNum.VBLF + " ORDER BY INDATE DESC";
                }
                else if (cboTewon.Text.Trim() == "가퇴원")
                {
                    SQL = SQL + ComNum.VBLF + "   AND ActDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + "   AND GbSTS = '1' ";
                    if (cboJob.Text.Trim() == "등록번호" && strInput != "")
                    {
                        SQL = SQL + ComNum.VBLF + "   AND Pano='" + strInput + "' ";
                    }
                    else if (cboJob.Text.Trim() == "환자성명" && strInput != "")
                    {
                        SQL = SQL + ComNum.VBLF + "   AND SName LIKE '%" + strInput + "%' ";
                    }
                }
                else
                {
                    if (cboJob.Text.Trim() == "등록번호")
                    {
                        SQL = SQL + ComNum.VBLF + "   AND Pano = '" + strInput + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND ActDate IS NOT NULL ";
                        SQL = SQL + ComNum.VBLF + " ORDER BY INDATE DESC";
                    }
                    else if (cboJob.Text.Trim() == "환자성명")
                    {
                        SQL = SQL + ComNum.VBLF + "   AND SName = '" + strInput + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND ActDate IS NOT NULL ";
                        SQL = SQL + ComNum.VBLF + " ORDER BY SName, Pano ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "   AND ActDate = TO_DATE('" + strInput + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + " ORDER BY SName, Pano ";
                    }
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                nRead = dt.Rows.Count;

                ssList_Sheet1.RowCount = 0;
                ssList_Sheet1.RowCount = nRead;
                ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < nRead; i++)
                {
                    ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 1].Text = cpf.Get_BasPatient(clsDB.DbCon, dt.Rows[i]["Pano"].ToString().Trim()).Rows[0]["Sname"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Bi"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["InDate"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["IPDNO"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["LastTrs"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["GBDRG"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;


                ssList.Focus();

                GstrPANO = strInput;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }
        
        void View_Data()
        {
            Cursor.Current = Cursors.WaitCursor;

            DRG drg = new DRG();

            string strNgt = "";
            long nGubTot = 0;

            ssView_Sheet1.Columns[8].Visible = true;
            ssView_Sheet1.Columns[9].Visible = true;
            ssView_Sheet1.Columns[10].Visible = true;
            ssView_Sheet1.Columns[11].Visible = true;
            ssView_Sheet1.Columns[12].Visible = true;
            ssView_Sheet1.Columns[13].Visible = true;
            ssView_Sheet1.Columns[14].Visible = true;
            ssView_Sheet1.Columns[21].Visible = true;
            ssView_Sheet1.Columns[26].Visible = true;
            ssView_Sheet1.Columns[27].Visible = true;

            if (FstrDrg == "D" && chkDrg.Checked == false)
            {
                strNgt = drg.Read_GbNgt_DRG(clsDB.DbCon, clsPmpaType.TIT.Pano, clsPmpaType.TIT.Ipdno, clsPmpaType.TIT.Trsno);
                DRG.READ_DRG_AMT_MASTER(clsDB.DbCon, clsPmpaType.TIT.DrgCode, clsPmpaType.TIT.Pano, clsPmpaType.TIT.Ipdno, clsPmpaType.TIT.Trsno, strNgt, clsPmpaType.TIT.InDate, clsPmpaType.TIT.OutDate);
                nGubTot = DRG.READ_행위별진료비총액_급여(clsDB.DbCon, clsPmpaType.TIT.Trsno, clsPmpaType.TIT.InDate);

                if (string.Compare(clsPmpaType.TIT.InDate, "2019-07-01") >= 0)
                {
                    DRG.GnDrg열외군금액 = nGubTot - (DRG.GnDrg급여총액 - DRG.GnGs80Amt_J - DRG.GnGs50Amt_J - DRG.GnGs90Amt_J) - 1000000;  
                   
                }
                else
                {
                    DRG.GnDrg열외군금액 = nGubTot - DRG.GnDrg급여총액 - 1000000;
                }

                if (DRG.GnDrg열외군금액 < 0) { DRG.GnDrg열외군금액 = 0; }

                DRG.GnDrg열외군금액_Bon = (DRG.GnDrg열외군금액 * clsPmpaType.IBR.Bohum / 100);
                DRG.GnDRG_TAmt = DRG.GnDRG_TAmt + DRG.GnDrg열외군금액_Bon;
                
           
            }
            
            strSDate = dtpSDate.Value.ToString("yyyy-MM-dd");
            strEdate = dtpTDate.Value.ToString("yyyy-MM-dd");

            switch (cboFnu.SelectedIndex)
            {
                #region From 누적
                case 0:
                    strFnu = "01";
                    break;   //진찰료
                case 1:
                    strFnu = "02";
                    break;   //입원료
                case 2:
                    strFnu = "04";
                    break;   //투약료
                case 3:
                    strFnu = "05";
                    break;   //주사료
                case 4:
                    strFnu = "06";
                    break;   //마취료
                case 5:
                    strFnu = "07";
                    break;   //pt/ns
                case 6:
                    strFnu = "09";
                    break;   //처치료
                case 7:
                    strFnu = "10";
                    break;   //수술료
                case 8:
                    strFnu = "13";
                    break;   //검사료
                case 9:
                    strFnu = "15";
                    break;   //방사선료
                case 10:
                    strFnu = "20";
                    break;   //식대급여
                case 11:
                    strFnu = "21";
                    break;   //비급여
                default:
                    strFnu = "01";
                    break; 
                    #endregion
            }

            strTnu = "00";

            switch (cboTnu.SelectedIndex)
            {
                #region To 누적
                case 0:
                    strTnu = "01";
                    break;   //진찰료
                case 1:
                    strTnu = "03";
                    break;   //입원료
                case 2:
                    strTnu = "04";
                    break;   //투약료
                case 3:
                    strTnu = "05";
                    break;   //주사료
                case 4:
                    strTnu = "06";
                    break;   //마취료
                case 5:
                    strTnu = "08";
                    break;   //pt/ns
                case 6:
                    strTnu = "09";
                    break;   //처치료
                case 7:
                    strTnu = "12";
                    break;   //수술료
                case 8:
                    strTnu = "14";
                    break;   //검사료
                case 9:
                    strTnu = "15";
                    break;   //방사선료
                case 10:
                    strTnu = "20";
                    break;   //식대급여
                case 11:
                    strTnu = "90";
                    break;   //비급여
                default:
                    strTnu = "90";
                    break; 
                    #endregion
            }

            if (string.Compare(strTnu, strFnu) < 0)
            {
                ComFunc.MsgBox("행위범위 From < To Error!!");
                cboFnu.Focus();
            }
            else
            {
                btnPrint.Enabled = false;
                ssView_Sheet1.RowCount = 0;

                switch (FnJobNo)
                {
                    case 1:
                        Read_IPD_SLIP1();
                        break;
                    case 2:
                        Read_IPD_SLIP2();
                        break; //임의출력 추가됨
                    case 3:
                        Read_IPD_SLIP3();
                        break;
                    case 4:
                        // Read_IPD_SLIP4_1();
                         Read_IPD_SLIP4();
                        break;
                    case 5:
                        Read_IPD_SLIP3_1();
                        break;
                    case 6:
                        Read_IPD_SLIP6();
                        break;
                    case 7:
                        Read_IPD_SLIP7();
                        break;
                    case 8:
                        Read_IPD_SLIP1("8");
                        break;  //마취합산
                }

            }
            Cursor.Current = Cursors.Default;
        }
        
        void SelChangeFnu(int nInx)
        {
            switch (nInx)
            {
                case 0:
                    strFnu = "01";
                    break;
                case 1:
                    strFnu = "02";
                    break;
                case 2:
                    strFnu = "04";
                    break;
                case 3:
                    strFnu = "05";
                    break;
                case 4:
                    strFnu = "06";
                    break;
                case 5:
                    strFnu = "07";
                    break;
                case 6:
                    strFnu = "09";
                    break;
                case 7:
                    strFnu = "10";
                    break;
                case 8:
                    strFnu = "13";
                    break;
                case 9:
                    strFnu = "15";
                    break;
                case 10:
                    strFnu = "20";
                    break;
                case 11:
                    strFnu = "21";
                    break;
                default:
                    strFnu = "01";
                    break;
            }
        }

        void SelChangeTnu(int nFInx, int nTInx)
        {
            if (nTInx < nFInx)
            {
                cboTnu.SelectedIndex = nFInx;
            }

            switch (nTInx)
            {
                case 0:
                    strTnu = "01";
                    break;
                case 1:
                    strTnu = "03";
                    break;
                case 2:
                    strTnu = "04";
                    break;
                case 3:
                    strTnu = "05";
                    break;
                case 4:
                    strTnu = "06";
                    break;
                case 5:
                    strTnu = "08";
                    break;
                case 6:
                    strTnu = "09";
                    break;
                case 7:
                    strTnu = "12";
                    break;
                case 8:
                    strTnu = "14";
                    break;
                case 9:
                    strTnu = "15";
                    break;
                case 10:
                    strTnu = "20";
                    break;   //식대급여
                case 11:
                    strTnu = "90";
                    break;   //비급여
                default:
                    strTnu = "90";
                    break;
            }
        }
        
        void Display_IPD_Master(double argIpdNo)
        {
            
            clsPmpaFunc cpf = new clsPmpaFunc();

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nRow = 0;
            string strBi = "";
            string strDept = "";
            string strPano = "";

            Screen_Claer();

            //재원마스타 READ
            clsIument ci = new clsIument();
            ci.Read_Ipd_Master(clsDB.DbCon, "", (long)argIpdNo);

            ssInfo_Sheet1.Cells[0, 0].Text = clsPmpaType.IMST.Pano;
            ssInfo_Sheet1.Cells[0, 1].Text = cpf.Get_BasPatient(clsDB.DbCon, (clsPmpaType.IMST.Pano)).Rows[0]["Sname"].ToString().Trim();
            ssInfo_Sheet1.Cells[0, 2].Text = clsPmpaType.IMST.Age + "/" + clsPmpaType.IMST.Sex;
            ssInfo_Sheet1.Cells[0, 3].Text = clsPmpaType.IMST.WardCode;
            ssInfo_Sheet1.Cells[0, 4].Text = clsPmpaType.IMST.RoomCode.ToString();
            ssInfo_Sheet1.Cells[0, 5].Text = clsPmpaType.IMST.InDate;
            ssInfo_Sheet1.Cells[0, 6].Text = clsPmpaType.IMST.Ilsu.ToString();
            ssInfo_Sheet1.Cells[0, 7].Text = clsPmpaType.IMST.Bi;
            ssInfo_Sheet1.Cells[0, 8].Text = clsPmpaType.IMST.DeptCode;
            ssInfo_Sheet1.Cells[0, 9].Text = cpf.READ_DOCTOR_NAME(clsDB.DbCon, (clsPmpaType.IMST.DeptCode));
            ssInfo_Sheet1.Cells[0, 10].Text = clsPmpaType.IMST.IPDNO.ToString();
            ssInfo_Sheet1.Cells[0, 11].Text = "";

            try
            {
                //2014-01-13 퇴원일 이후 작업SLIP 파란색 표시관련 보완
                FstrMinActDate = "";

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(MIN(ACTDATE),'YYYY-MM-DD') MINACTDATE ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_TRANS ";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + argIpdNo + " ";
                //SQL = SQL + ComNum.VBLF + "   AND GBIPD <> 'D' ";  //jjy : 2018-07-06 삭제된 것도 표시되어야함(심사팀요청사항임)
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    FstrMinActDate = dt.Rows[0]["MINACTDATE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                //IPD_TRANS를 읽어 Sheet에 표시함
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TRSNO, TO_CHAR(InDate,'YYYYMMDD') InDate, GBDRG,";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(OutDate,'YYYYMMDD') OutDate, TO_CHAR(ActDate,'YYYYMMDD') ActDate, Ilsu,";
                SQL = SQL + ComNum.VBLF + "       Bi, Pano, DeptCode,";
                SQL = SQL + ComNum.VBLF + "       DrCode, GbIPD, SangAmt,OGPDBUN2,JINDTL,";
                SQL = SQL + ComNum.VBLF + "       OgPdBun, OgPdBundtl, AmSet3,";
                SQL = SQL + ComNum.VBLF + "       VCODE, BONRATE,FCODE ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_TRANS ";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO=" + argIpdNo + " ";
                //SQL = SQL + ComNum.VBLF + "   AND GBIPD <> 'D' ";   //jjy : 2018-07-06 삭제된 것도 표시되어야함(심사팀요청사항임)
                SQL = SQL + ComNum.VBLF + " ORDER BY GbIPD,InDate DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                nRow = 0;
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nRow += 1;
                    if (nRow > ssTrans_Sheet1.RowCount)
                    {
                        ssTrans_Sheet1.RowCount = nRow;
                    }

                    ssTrans_Sheet1.RowCount = nRow;
                    ssTrans_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["InDate"].ToString().Trim();
                    ssTrans_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["OutDate"].ToString().Trim();
                    ssTrans_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["Ilsu"].ToString().Trim();
                    ssTrans_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["Bi"].ToString().Trim();
                    ssTrans_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    ssTrans_Sheet1.Cells[nRow - 1, 5].Text = cpf.READ_DOCTOR_NAME(clsDB.DbCon, dt.Rows[i]["DrCode"].ToString().Trim());
                    ssTrans_Sheet1.Cells[nRow - 1, 6].Text = "";
                    if (dt.Rows[i]["GbIPD"].ToString().Trim() == "9")
                    {
                        ssTrans_Sheet1.Cells[nRow - 1, 6].Text = "지병";
                        if (dt.Rows[i]["JINDTL"].ToString().Trim() == "01") { ssTrans_Sheet1.Cells[nRow - 1, 6].Text = "지병+틀니"; }
                    }
                    ssTrans_Sheet1.Cells[nRow - 1, 7].Text = "";
                    if (VB.Val(dt.Rows[i]["SangAmt"].ToString().Trim()) > 0)
                    {
                        ssTrans_Sheet1.Cells[nRow - 1, 7].Text = "상한";
                    }
                    ssTrans_Sheet1.Cells[nRow - 1, 8].Text = dt.Rows[i]["OgPdBun"].ToString().Trim();
                    ssTrans_Sheet1.Cells[nRow - 1, 9].Text = "";
                    if (dt.Rows[i]["VCODE"].ToString().Trim() == "V191" || dt.Rows[i]["VCODE"].ToString().Trim() == "V192" ||
                        dt.Rows[i]["VCODE"].ToString().Trim() == "V193" || dt.Rows[i]["VCODE"].ToString().Trim() == "V194" ||
                        dt.Rows[i]["VCODE"].ToString().Trim() == "V268" || dt.Rows[i]["VCODE"].ToString().Trim() == "V275")
                    {
                        ssTrans_Sheet1.Cells[nRow - 1, 9].Text = "중증";
                    }
                    if (dt.Rows[i]["VCODE"].ToString().Trim() == "V247" || dt.Rows[i]["VCODE"].ToString().Trim() == "V248" ||
                        dt.Rows[i]["VCODE"].ToString().Trim() == "V249" || dt.Rows[i]["VCODE"].ToString().Trim() == "V250")
                    {
                        ssTrans_Sheet1.Cells[nRow - 1, 9].Text = "화상";
                    }
                    ssTrans_Sheet1.Cells[nRow - 1, 10].Text = "";
                    if (dt.Rows[i]["AmSet3"].ToString().Trim() == "9")
                    {
                        ssTrans_Sheet1.Cells[nRow - 1, 10].Text = "완료";
                    }
                    ssTrans_Sheet1.Cells[nRow - 1, 11].Text = dt.Rows[i]["ActDate"].ToString().Trim();
                    ssTrans_Sheet1.Cells[nRow - 1, 12].Text = argIpdNo.ToString();
                    ssTrans_Sheet1.Cells[nRow - 1, 13].Text = dt.Rows[i]["TRSNO"].ToString().Trim();
                    ssTrans_Sheet1.Cells[nRow - 1, 14].Text = dt.Rows[i]["BONRATE"].ToString().Trim();
                    FnTRSNO = Convert.ToInt32(dt.Rows[i]["TRSNO"].ToString().Trim());
                    ssTrans_Sheet1.Cells[nRow - 1, 15].Text = dt.Rows[i]["OgPdBundtl"].ToString().Trim();
                    ssTrans_Sheet1.Cells[nRow - 1, 16].Text = dt.Rows[i]["GBDRG"].ToString().Trim();
                    if (i == 0)
                    {
                        strPano = dt.Rows[0]["Pano"].ToString().Trim();
                        strBi = dt.Rows[0]["Bi"].ToString().Trim();
                        strDept = dt.Rows[0]["DeptCode"].ToString().Trim();
                    }
                    ssTrans_Sheet1.Cells[nRow - 1, 17].Text = dt.Rows[i]["FCODE"].ToString().Trim();
                }

                ssTrans_Sheet1.RowCount = dt.Rows.Count;
                ssTrans_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                if (dt.Rows.Count == 1)
                {
                    ci.Read_Ipd_Mst_Trans(clsDB.DbCon, strPano, (long)FnTRSNO, "");

                    dtpSDate.Value = Convert.ToDateTime(clsPmpaType.TIT.InDate);

                    if (clsPmpaType.TIT.OutDate == "")
                    {
                        dtpTDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
                    }
                    else
                    {
                        dtpTDate.Value = Convert.ToDateTime(clsPmpaType.TIT.OutDate);
                    }
                    cboFnu.SelectedIndex = 0;
                    cboTnu.SelectedIndex = cboTnu.Items.Count - 1;
                    
                    if (FstrView != "OK")
                    {
                        View_Data();
                    }

                    ssTrans_Sheet1.Rows[0].BackColor = Color.FromArgb(164, 255, 164);

                    cpf.Read_Patient_Rate_Chk(clsDB.DbCon, ssRate_Sheet1, "I", strPano, "", clsPmpaType.IMST.InDate, strBi, strDept, "", 0, (int)argIpdNo, (int)FnTRSNO);

                    if (dtpSDate.Value <= Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).AddDays(-1830))
                    {
                        btnPrint.Enabled = false;
                        if (clsVbfunc.JinAmtPrintChk(clsDB.DbCon, clsType.User.Sabun.ToString().Trim()) == true)
                        {
                            btnPrint.Enabled = true;
                        }
                    }
                    else
                    {
                        btnPrint.Enabled = true;
                    }

                    if (FstrView == "OK")
                    {
                        cboFnu.Focus();
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void btnBed_Click(object sender, EventArgs e)
        {
            
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nNu = 0;
            int nNuChk = 0;
            int nRow = 0;
            int nRead = 0;
            long nAmt1 = 0;
            long nAmt2 = 0;
            long nStot1 = 0;
            long nStot2 = 0;
            long nGtot1 = 0;
            long nGtot2 = 0;
            string strNujuk = "";
            string strBaseAmt = "";
            string strQty = "";
            string strNal = "";
            string strAMT1 = "";
            string strAMT2 = "";
            string strCFdate = "";
            string strBDate = "";

            clsPmpaType.ISBR ISBR = new clsPmpaType.ISBR();
            clsIument cIUM = new clsIument();

            ssView_Sheet1.RowCount = 0;

            ssView_Sheet1.ColumnHeader.Cells[0, 1].Text = "발생일자";
            ssView_Sheet1.ColumnHeader.Cells[0, 2].Text = "수가코드";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(Bdate, 'yy-mm-dd') Bday, Bun, Nu,";
                SQL = SQL + ComNum.VBLF + "        Sucode, SunameK, BaseAmt,";
                SQL = SQL + ComNum.VBLF + "        Qty, Nal, GbSpc,";
                SQL = SQL + ComNum.VBLF + "        GbNgt, GbGisul, GbSelf,";
                SQL = SQL + ComNum.VBLF + "        GbChild, Amt1, Amt2,";
                SQL = SQL + ComNum.VBLF + "        Part";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP I, " + ComNum.DB_PMPA + "BAS_SUN B";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND I.IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                SQL = SQL + ComNum.VBLF + "    AND I.NU IN ('02','03','35','21') ";
                SQL = SQL + ComNum.VBLF + "    AND I.Sunext = B.Sunext(+) ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY I.Nu,I.Bdate,I.Sucode,I.Sunext ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                nRead = dt.Rows.Count;
                nGETcount += nRead;

                for (i = 0; i < nRead; i++)
                {
                    nNu = (int)VB.Val(dt.Rows[i]["Nu"].ToString().Trim());

                    if (nNuChk == 0)
                    {
                        nNuChk = nNu;
                        strCFdate = dt.Rows[i]["Bday"].ToString().Trim();
                        strBDate = dt.Rows[i]["Bday"].ToString().Trim();
                        strNujuk = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_누적행위명", nNu.ToString("00"));
                    }

                    if (nNu != nNuChk)
                    {
                        SUB_TOT_SLIP1(ref nRow, ref nStot1, ref nStot2, ISBR);

                        nNuChk = nNu;
                        strCFdate = dt.Rows[i]["Bday"].ToString().Trim();
                        strBDate = dt.Rows[i]["Bday"].ToString().Trim();
                        strNujuk = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_누적행위명", nNu.ToString("00"));
                    }

                    if (strCFdate != dt.Rows[i]["Bday"].ToString().Trim())
                    {
                        strCFdate = dt.Rows[i]["Bday"].ToString().Trim();
                        strBDate = dt.Rows[i]["Bday"].ToString().Trim();
                    }

                    #region DATA_MOVE_Room

                    nRow += 1;
                    if (ssView_Sheet1.RowCount < nRow)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    nAmt1 = (long)VB.Val(dt.Rows[i]["Amt1"].ToString().Trim());
                    nAmt2 = (long)VB.Val(dt.Rows[i]["Amt2"].ToString().Trim());

                    nStot1 += nAmt1;
                    nStot2 += nAmt2;
                    nGtot1 += nAmt1;
                    nGtot2 += nAmt2;

                    strBaseAmt = VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("##,###,##0");
                    strQty = VB.Val(dt.Rows[i]["Qty"].ToString().Trim()).ToString("##0.00");
                    strNal = VB.Val(dt.Rows[i]["Nal"].ToString().Trim()).ToString("##0");
                    strAMT1 = nAmt1.ToString("###,###,##0");
                    strAMT2 = nAmt2.ToString("###,###,##0");

                    ssView_Sheet1.Cells[nRow - 1, 0].Text = strNujuk;
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = strBDate;
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["Sucode"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["SunameK"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = strBaseAmt;
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = strQty;
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = strNal;
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = strAMT1;
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = strAMT2;
                    ssView_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["Part"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["GbSpc"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 11].Text = dt.Rows[i]["GbNgt"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 12].Text = dt.Rows[i]["GbGisul"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 13].Text = dt.Rows[i]["GbSelf"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 27].Text = dt.Rows[i]["GbChild"].ToString().Trim();

                    #endregion


                    strNujuk = "";
                    strBDate = "";
                }
                dt.Dispose();
                dt = null;

                if (nGETcount > 0)
                {
                    SUB_TOT_SLIP1(ref nRow, ref nStot1, ref nStot2, ISBR);

                    GRAND_TOT_SLIP1(ref nRow, ref nGtot1, ref nGtot2, ISBR);

                    btnPrint.Enabled = true;
                    ssView.Enabled = true;
                    ssView.Focus();
                }

                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void btnBigub_Click(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nNu = 0;
            int nNuChk = 0;
            int nRow = 0;
            int nRead = 0;
            long nAmt1 = 0;
            long nAmt2 = 0;
            long nStot1 = 0;
            long nStot2 = 0;
            long nGtot1 = 0;
            long nGtot2 = 0;
            string strNujuk = "";
            string strBaseAmt = "";
            string strQty = "";
            string strNal = "";
            string strAMT1 = "";
            string strAMT2 = "";
            string strCFdate = "";
            string strBDate = "";
            string strBun = "";
            string str100 = "";
            string strF = "";

            clsPmpaType.ISBR ISBR = new clsPmpaType.ISBR();
            clsIument cIUM = new clsIument();

            ssView_Sheet1.RowCount = 0;

            ssView_Sheet1.ColumnHeader.Cells[0, 1].Text = "발생일자";
            ssView_Sheet1.ColumnHeader.Cells[0, 2].Text = "수가코드";

            nNuChk = 0;
            nStot1 = 0;
            nStot2 = 0;
            nGtot1 = 0;
            nGtot2 = 0;
            strCFdate = "";
            nRow = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(Bdate, 'yy-mm-dd') Bday, Bun, Nu,";
                SQL = SQL + ComNum.VBLF + "        Sucode, SunameK, BaseAmt,";
                SQL = SQL + ComNum.VBLF + "        Qty, Nal, DRG100,";
                SQL = SQL + ComNum.VBLF + "        DRGF, GbSpc, GbNgt,";
                SQL = SQL + ComNum.VBLF + "        GbGisul, GbSelf, GbChild,";
                SQL = SQL + ComNum.VBLF + "        Amt1, Amt2, Part";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP I, " + ComNum.DB_PMPA + "BAS_SUN B";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND I.IPDNO = " + clsPmpaType.TIT.Ipdno;
                SQL = SQL + ComNum.VBLF + "    AND I.SUNEXT NOT IN ('DRG001','DRG002')";
                SQL = SQL + ComNum.VBLF + "    AND I.NU >= '21' ";
                SQL = SQL + ComNum.VBLF + "    AND I.Sunext = B.Sunext(+) ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY I.Nu,I.Bdate,I.Sucode,I.Sunext ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                nRead = dt.Rows.Count;
                nGETcount += nRead;

                for (i = 0; i < nRead; i++)
                {
                    strBun = dt.Rows[i]["Bun"].ToString().Trim();
                    str100 = dt.Rows[i]["DRG100"].ToString().Trim();
                    strF = dt.Rows[i]["DRGF"].ToString().Trim();

                    if (DRG.READ_DRG_100(clsDB.DbCon, dt.Rows[i]["Sucode"].ToString().Trim(), "", clsPmpaType.TIT.InDate) == "OK")
                    {
                        str100 = "Y";
                    }
                    else
                    {
                        str100 = "";
                    }

                    nNu = Convert.ToInt32(dt.Rows[i]["Nu"].ToString().Trim());

                    if (nNuChk == 0)
                    {
                        nNuChk = nNu;
                        strCFdate = dt.Rows[i]["Bday"].ToString().Trim();
                        strBDate = dt.Rows[i]["Bday"].ToString().Trim();
                        strNujuk = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_누적행위명", nNu.ToString("00"));
                    }

                    if (nNu != nNuChk)
                    {
                        SUB_TOT_SLIP1(ref nRow, ref nStot1, ref nStot2, ISBR);

                        nNuChk = nNu;
                        strCFdate = dt.Rows[i]["Bday"].ToString().Trim();
                        strBDate = dt.Rows[i]["Bday"].ToString().Trim();
                        strNujuk = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_누적행위명", nNu.ToString("00"));
                    }

                    if (strCFdate != dt.Rows[i]["Bday"].ToString().Trim())
                    {
                        strCFdate = dt.Rows[i]["Bday"].ToString().Trim();
                        strBDate = dt.Rows[i]["Bday"].ToString().Trim();
                    }

                    #region DATA_MOVE_Bigub

                    nRow += 1;
                    if (ssView_Sheet1.RowCount < nRow)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    nAmt1 = (long)VB.Val(dt.Rows[i]["Amt1"].ToString().Trim());
                    nAmt2 = (long)VB.Val(dt.Rows[i]["Amt2"].ToString().Trim());

                    strQty = VB.Val(dt.Rows[i]["Qty"].ToString().Trim()).ToString("##0.00");
                    strNal = VB.Val(dt.Rows[i]["Nal"].ToString().Trim()).ToString("##0");

                    if (FstrDrg == "D" && chkDrg.Checked == false)
                    {
                        if (strBun != "74" && strF != "Y" && str100 != "Y")
                        {
                            nAmt1 = 0;
                            strBaseAmt = "0";
                        }
                        else
                        {
                            strBaseAmt = VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("##,###,##0");
                        }

                        if (nAmt1 == 0 && strBaseAmt != "0")
                        {
                            nAmt1 = (long)VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()) + (long)VB.Val(dt.Rows[i]["Qty"].ToString().Trim());
                        }

                        if (strNal == "0")
                        {
                            nAmt1 = 0;
                            nAmt2 = 0;
                        }
                    }
                    else
                    {
                        strBaseAmt = VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("##,###,##0");
                    }

                    nStot1 += nAmt1;
                    nStot2 += nAmt2;
                    nGtot1 += nAmt1;
                    nGtot2 += nAmt2;

                    strAMT1 = nAmt1.ToString("###,###,##0");
                    strAMT2 = nAmt2.ToString("###,###,##0");

                    ssView_Sheet1.Cells[nRow - 1, 0].Text = strNujuk;
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = strBDate;
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["Sucode"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["SunameK"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = strBaseAmt;
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = strQty;
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = strNal;
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = strAMT1;
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = strAMT2;
                    ssView_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["Part"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["GbSpc"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 11].Text = dt.Rows[i]["GbNgt"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 12].Text = dt.Rows[i]["GbGisul"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 13].Text = dt.Rows[i]["GbSelf"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 27].Text = dt.Rows[i]["GbChild"].ToString().Trim();

                    #endregion

                    strNujuk = "";
                    strBDate = "";
                }
                dt.Dispose();
                dt = null;

                if (FstrDrg == "D" && chkDrg.Checked == false)
                {
                    if (nGETcount > 0)
                    {
                        SUB_TOT_SLIP1(ref nRow, ref nStot1, ref nStot2, ISBR);
                    }

                    Display_Drg_Amt(ref nRow, ref nAmt1, ref nAmt2, ref nStot1, ref nStot2, ref nGtot1, ref nGtot2, ref strBaseAmt, ref strQty, ref strNal, ISBR);
                }

                if (nGETcount > 0)
                {
                    SUB_TOT_SLIP1(ref nRow, ref nStot1, ref nStot2, ISBR);

                    GRAND_TOT_SLIP1(ref nRow, ref nGtot1, ref nGtot2, ISBR);

                    btnPrint.Enabled = true;
                    ssView.Enabled = true;
                    ssView.Focus();
                }


                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void btnEtc1_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            string SQL = "";    //Query문
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            int nTRSCNT = 0;
            double nIPDNO = 0;
            double nIPDNO2 = 0;
            double nOldIPDNO = 0;
            double[] nTRSNO = new double[11];

            clsPmpaFunc cpf = new clsPmpaFunc();

            nOldIPDNO = clsPmpaType.IMST.IPDNO;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //기존에 전산실연습이 있으면 삭제함


                SQL = "";
                SQL = SQL + "DELETE " + ComNum.DB_PMPA + "IPD_NEW_MASTER WHERE PANO='81000004'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = SQL + "DELETE " + ComNum.DB_PMPA + "IPD_TRANS  WHERE PANO='81000004'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = SQL + "DELETE " + ComNum.DB_PMPA + "IPD_NEW_SLIP   WHERE PANO='81000004'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = SQL + "DELETE " + ComNum.DB_PMPA + "IPD_NEW_CASH   WHERE PANO='81000004'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = SQL + "DELETE " + ComNum.DB_PMPA + "IPD_BM WHERE PANO='81000004'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TRSNO FROM " + ComNum.DB_PMPA + "IPD_TRANS ";
                SQL = SQL + ComNum.VBLF + "WHERE IPDNO=" + nOldIPDNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                nTRSCNT = dt.Rows.Count;
                for (i = 0; i < nTRSCNT; i++)
                {
                    nTRSNO[i + 1] = VB.Val(dt.Rows[i]["TRSNO"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;

                nIPDNO = cpf.GET_NEXT_IPDNO(clsDB.DbCon);

                //IPD_NEW_MASTER
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "IPD_NEW_MASTER (";
                SQL = SQL + ComNum.VBLF + "       IPDNO,PANO,SNAME,SEX,AGE,BI,INDATE,OUTDATE,ACTDATE,ILSU,GBSTS,DEPTCODE,";
                SQL = SQL + ComNum.VBLF + "       DRCODE,WARDCODE,ROOMCODE,PNAME,GBSPC,GBKEKLI,GBGAMEK,FEE6,BOHUN,JIYUK,GELCODE,";
                SQL = SQL + ComNum.VBLF + "       RELIGION,GBCANCER,INOUT,OTHER,GBDONGGI,OGPDBUN,OGPDBUNdtl,ARTICLE,JUPBONO,FROMTRANS,ERAMT,";
                SQL = SQL + ComNum.VBLF + "       ARCDATE,ARCQTY,ICUQTY,IM180,ILLCODE1,ILLCODE2,ILLCODE3,ILLCODE4,ILLCODE5,ILLCODE6,";
                SQL = SQL + ComNum.VBLF + "       TRSDATE,DEPT1,DEPT2,DEPT3,DOCTOR1,DOCTOR2,DOCTOR3,ILSU1,ILSU2,ILSU3,AMSET1,AMSET4,";
                SQL = SQL + ComNum.VBLF + "       AMSET5,AMSET6,AMSET7,AMSET8,AMSET9,AMSETA,RDATE,TRSCNT,LASTTRS,IPWONTIME,CANCELTIME,";
                SQL = SQL + ComNum.VBLF + "       GATEWONTIME,ROUTDATE,SIMSATIME,PRINTTIME,SUNAPTIME,GBCHECKLIST,MIRBUILDTIME,";
                SQL = SQL + ComNum.VBLF + "       REMARK)";
                SQL = SQL + ComNum.VBLF + "SELECT '" + nIPDNO + "','81000004','전산실연습',Sex,Age,";
                SQL = SQL + ComNum.VBLF + "       BI,INDATE,OUTDATE,ACTDATE,ILSU,GBSTS,DEPTCODE,";
                SQL = SQL + ComNum.VBLF + "       DRCODE,WARDCODE,ROOMCODE,PNAME,GBSPC,GBKEKLI,GBGAMEK,FEE6,BOHUN,JIYUK,GELCODE,";
                SQL = SQL + ComNum.VBLF + "       RELIGION,GBCANCER,INOUT,OTHER,GBDONGGI,OGPDBUN,OGPDBUNdtl,ARTICLE,JUPBONO,FROMTRANS,ERAMT,";
                SQL = SQL + ComNum.VBLF + "       ARCDATE,ARCQTY,ICUQTY,IM180,ILLCODE1,ILLCODE2,ILLCODE3,ILLCODE4,ILLCODE5,ILLCODE6,";
                SQL = SQL + ComNum.VBLF + "       TRSDATE,DEPT1,DEPT2,DEPT3,DOCTOR1,DOCTOR2,DOCTOR3,ILSU1,ILSU2,ILSU3,AMSET1,AMSET4,";
                SQL = SQL + ComNum.VBLF + "       AMSET5,AMSET6,AMSET7,AMSET8,AMSET9,AMSETA,RDATE,TRSCNT,LASTTRS,IPWONTIME,CANCELTIME,";
                SQL = SQL + ComNum.VBLF + "       GATEWONTIME,ROUTDATE,SIMSATIME,PRINTTIME,SUNAPTIME,GBCHECKLIST,MIRBUILDTIME,";
                SQL = SQL + ComNum.VBLF + "       REMARK";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO=" + nOldIPDNO + " ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //IPD_BM
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "IPD_BM (";
                SQL = SQL + ComNum.VBLF + "       JOBDATE,GBBACKUP,IPDNO,PANO,SNAME,SEX,AGE,BI,INDATE,OUTDATE,ACTDATE,";
                SQL = SQL + ComNum.VBLF + "       ILSU,GBSTS,TRSCNT,LASTTRS,DEPTCODE,DRCODE,WARDCODE,ROOMCODE,WARD,PNAME,GBSPC,";
                SQL = SQL + ComNum.VBLF + "       GBKEKLI,GBGAMEK,FEE6,BOHUN,JIYUK,GELCODE,RELIGION,GBCANCER,INOUT,OTHER,GBDONGGI,";
                SQL = SQL + ComNum.VBLF + "       OGPDBUN,OGPDBUNdtl,ARTICLE,GBDRG,DRGWRTNO,ARCDATE,ARCQTY,ICUQTY,IM180,AMSET1,AMSET4,AMSET5,";
                SQL = SQL + ComNum.VBLF + "       AMSET6,AMSET7,AMSET8,AMSET9,AMSETA,IPWONTIME,CANCELTIME,GATEWONTIME,SUNAPTIME,";
                SQL = SQL + ComNum.VBLF + "       BUILDJAMT) ";
                SQL = SQL + ComNum.VBLF + "SELECT JOBDATE,GBBACKUP,'" + nIPDNO + "','81000004','전산실연습',SEX,AGE,BI,INDATE,OUTDATE,ACTDATE,";
                SQL = SQL + ComNum.VBLF + "       ILSU,GBSTS,TRSCNT,LASTTRS,DEPTCODE,DRCODE,WARDCODE,ROOMCODE,WARD,PNAME,GBSPC,";
                SQL = SQL + ComNum.VBLF + "       GBKEKLI,GBGAMEK,FEE6,BOHUN,JIYUK,GELCODE,RELIGION,GBCANCER,INOUT,OTHER,GBDONGGI,";
                SQL = SQL + ComNum.VBLF + "       OGPDBUN,OGPDBUNdtl,ARTICLE,GBDRG,DRGWRTNO,ARCDATE,ARCQTY,ICUQTY,IM180,AMSET1,AMSET4,AMSET5,";
                SQL = SQL + ComNum.VBLF + "       AMSET6,AMSET7,AMSET8,AMSET9,AMSETA,IPWONTIME,CANCELTIME,GATEWONTIME,SUNAPTIME,";
                SQL = SQL + ComNum.VBLF + "       BUILDJAMT ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_BM ";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO=" + nOldIPDNO + " ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }


                //IPD_TRANS
                for (i = 1; i <= nTRSCNT; i++)
                {
                    nIPDNO2 = cpf.GET_NEXT_TRSNO(clsDB.DbCon);

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "INSERT INTO IPD_TRANS (";
                    SQL = SQL + ComNum.VBLF + "       TRSNO,IPDNO,PANO,GBIPD,INDATE,OUTDATE,ACTDATE,DEPTCODE,DRCODE,ILSU, ";
                    SQL = SQL + ComNum.VBLF + "       BI,KIHO,GKIHO,PNAME,GWANGE,BONRATE,GISULRATE,GBGAMEK,BOHUN,AMSET1,AMSET2,AMSET3,";
                    SQL = SQL + ComNum.VBLF + "       AMSET4,AMSET5,AMSETB,FROMTRANS,ERAMT,JUPBONO,GBDRG,DRGWRTNO,SANGAMT,DTGAMEK, ";
                    SQL = SQL + ComNum.VBLF + "       OGPDBUN,OGPDBUN2,OGPDBUNdtl,AMT01,AMT02,AMT03,AMT04,AMT05,AMT06,AMT07,AMT08,AMT09,AMT10,AMT11,AMT12,";
                    SQL = SQL + ComNum.VBLF + "       AMT13,AMT14,AMT15,AMT16,AMT17,AMT18,AMT19,AMT20,AMT21,AMT22,AMT23,AMT24,AMT25,AMT26,";
                    SQL = SQL + ComNum.VBLF + "       AMT27,AMT28,AMT29,AMT30,AMT31,AMT32,AMT33,AMT34,AMT35,AMT36,AMT37,AMT38,AMT39,AMT40,";
                    SQL = SQL + ComNum.VBLF + "       AMT41,AMT42,AMT43,AMT44,AMT45,AMT46,AMT47,AMT48,AMT49,AMT50,AMT51,AMT52,AMT53,AMT54,";
                    SQL = SQL + ComNum.VBLF + "       AMT55,AMT56,AMT57,AMT58,AMT59,AMT60,REMARK,ENTDATE,ENTSABUN,GbSPC) ";
                    SQL = SQL + ComNum.VBLF + "SELECT '" + nIPDNO2 + "','" + nIPDNO + "',";
                    SQL = SQL + ComNum.VBLF + "       '81000004',GBIPD,INDATE,OUTDATE,ACTDATE,DEPTCODE,DRCODE,ILSU, ";
                    SQL = SQL + ComNum.VBLF + "       BI,KIHO,GKIHO,PNAME,GWANGE,BONRATE,GISULRATE,GBGAMEK,BOHUN,AMSET1,AMSET2,AMSET3,";
                    SQL = SQL + ComNum.VBLF + "       AMSET4,AMSET5,AMSETB,FROMTRANS,ERAMT,JUPBONO,GBDRG,DRGWRTNO,SANGAMT,DTGAMEK, ";
                    SQL = SQL + ComNum.VBLF + "       OGPDBUN,OGPDBUN2,OGPDBUNdtl,AMT01,AMT02,AMT03,AMT04,AMT05,AMT06,AMT07,AMT08,AMT09,AMT10,AMT11,AMT12,";
                    SQL = SQL + ComNum.VBLF + "       AMT13,AMT14,AMT15,AMT16,AMT17,AMT18,AMT19,AMT20,AMT21,AMT22,AMT23,AMT24,AMT25,AMT26,";
                    SQL = SQL + ComNum.VBLF + "       AMT27,AMT28,AMT29,AMT30,AMT31,AMT32,AMT33,AMT34,AMT35,AMT36,AMT37,AMT38,AMT39,AMT40,";
                    SQL = SQL + ComNum.VBLF + "       AMT41,AMT42,AMT43,AMT44,AMT45,AMT46,AMT47,AMT48,AMT49,AMT50,AMT51,AMT52,AMT53,AMT54,";
                    SQL = SQL + ComNum.VBLF + "       AMT55,AMT56,AMT57,AMT58,AMT59,AMT60,REMARK,ENTDATE,ENTSABUN,GbSPC ";
                    SQL = SQL + ComNum.VBLF + "  FROM ADMIN.IPD_TRANS ";
                    SQL = SQL + ComNum.VBLF + " WHERE TRSNO=" + nTRSNO[i] + " ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }


                    //IPD_NEW_SLIP
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "IPD_NEW_SLIP (";
                    SQL = SQL + ComNum.VBLF + "       IPDNO,TRSNO,ACTDATE,PANO,BI,BDATE,ENTDATE,SUNEXT,BUN,NU,QTY,NAL,BASEAMT,";
                    SQL = SQL + ComNum.VBLF + "       GBSPC,GBNGT,GBGISUL,GBSELF,GBCHILD,DEPTCODE,DRCODE,WARDCODE,SUCODE,GBSLIP,GBHOST,";
                    SQL = SQL + ComNum.VBLF + "       PART,AMT1,AMT2,SEQNO,YYMM,DRGSELF,ORDERNO,OPER_DEPT,OPER_DCT,ORDER_DEPT,";
                    SQL = SQL + ComNum.VBLF + "       ORDER_DCT,EXAM_WRTNO,ROOMCODE,GBSELNOT) ";
                    SQL = SQL + ComNum.VBLF + "SELECT '" + nIPDNO + "','" + nIPDNO2 + "',ACTDATE,'81000004',";
                    SQL = SQL + ComNum.VBLF + "       BI,BDATE,ENTDATE,SUNEXT,BUN,NU,QTY,NAL,BASEAMT,";
                    SQL = SQL + ComNum.VBLF + "       GBSPC,GBNGT,GBGISUL,GBSELF,GBCHILD,DEPTCODE,DRCODE,WARDCODE,SUCODE,GBSLIP,GBHOST,";
                    SQL = SQL + ComNum.VBLF + "       PART,AMT1,AMT2,SEQNO,YYMM,DRGSELF,ORDERNO,OPER_DEPT,OPER_DCT,ORDER_DEPT,";
                    SQL = SQL + ComNum.VBLF + "        ORDER_DCT,EXAM_WRTNO,ROOMCODE,GBSELNOT ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                    SQL = SQL + ComNum.VBLF + " WHERE TRSNO=" + nTRSNO[i] + " ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }


                    //IPD_NEW_CASH
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "INSERT INTO  IPD_NEW_CASH (";
                    SQL = SQL + ComNum.VBLF + "       IPDNO,TRSNO,ACTDATE,PANO,BI,BDATE,SUNEXT,BUN,QTY,NAL,AMT,DEPTCODE,DRCODE,";
                    SQL = SQL + ComNum.VBLF + "       GBGAMEK,GELCODE,BIGO,ENTDATE,PART,CARDSEQNO) ";
                    SQL = SQL + ComNum.VBLF + "SELECT '" + nIPDNO + "','" + nIPDNO2 + "',ACTDATE,'81000004',BI,BDATE,SUNEXT,";
                    SQL = SQL + ComNum.VBLF + "       BUN,QTY,NAL,AMT,DEPTCODE,DRCODE,";
                    SQL = SQL + ComNum.VBLF + "       GBGAMEK,GELCODE,BIGO,ENTDATE,PART,CARDSEQNO ";
                    SQL = SQL + ComNum.VBLF + "  FROM ADMIN.IPD_NEW_CASH ";
                    SQL = SQL + ComNum.VBLF + " WHERE TRSNO=" + nTRSNO[i] + " ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                }

                clsDB.setRollbackTran(clsDB.DbCon);
                //clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("작업완료");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        void btnEtc2_Click(object sender, EventArgs e)
        {
            int i = 0;
            string SQL = "";    //Query문

            double nIPDNO = 0;
            double nTRSNO = 0;

            clsIpdAcct cia = new clsIpdAcct();


            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 1; i <= ssTrans_Sheet1.RowCount; i++)
                {
                    nIPDNO = VB.Val(ssView_Sheet1.Cells[i - 1, 11].Text);
                    nTRSNO = VB.Val(ssView_Sheet1.Cells[i - 1, 12].Text);

                    //처방전을 다시 읽어 IPD_TRANS의 Amt01~Amt50의 금액을 다시 누적함
                    if (cia.Ipd_Trans_Amt_ReBuild(clsDB.DbCon, (long)nTRSNO, "") == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("총진료비를 재계산 도중에 오류가 발생함!!, 전산실로 연락바람!!!");
                        return;
                    }
                }


                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("금액 재집계 완료");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }
        
        void btnFood_Click(object sender, EventArgs e)
        {
            

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nNu = 0;
            int nNuChk = 0;
            int nRow = 0;
            int nRead = 0;
            long nAmt1 = 0;
            long nAmt2 = 0;
            long nStot1 = 0;
            long nStot2 = 0;
            long nGtot1 = 0;
            long nGtot2 = 0;
            string strNujuk = "";
            string strBaseAmt = "";
            string strQty = "";
            string strNal = "";
            string strAMT1 = "";
            string strAMT2 = "";
            string strCFdate = "";
            string strBDate = "";

            clsPmpaType.ISBR ISBR = new clsPmpaType.ISBR();
            clsIument cIUM = new clsIument();

            ssView_Sheet1.RowCount = 0;

            ssView_Sheet1.ColumnHeader.Cells[0, 1].Text = "발생일자";
            ssView_Sheet1.ColumnHeader.Cells[0, 2].Text = "수가코드";

            nNuChk = 0;
            nStot1 = 0;
            nStot2 = 0;
            nGtot1 = 0;
            nGtot2 = 0;
            strCFdate = "";
            nRow = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(Bdate, 'yy-mm-dd') Bday, Bun, Nu,";
                SQL = SQL + ComNum.VBLF + "        Sucode, SunameK, BaseAmt,";
                SQL = SQL + ComNum.VBLF + "        Qty, Nal, GbSpc,";
                SQL = SQL + ComNum.VBLF + "        GbNgt, GbGisul, GbSelf,";
                SQL = SQL + ComNum.VBLF + "        GbChild, Amt1, Amt2,";
                SQL = SQL + ComNum.VBLF + "        Part";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP I, " + ComNum.DB_PMPA + "BAS_SUN B";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND I.IPDNO = " + clsPmpaType.TIT.Ipdno;
                SQL = SQL + ComNum.VBLF + "    AND I.Bun = '74' ";
                SQL = SQL + ComNum.VBLF + "    AND I.Sunext = B.Sunext(+) ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY I.Bdate,I.Sucode,I.Sunext ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                nRead = dt.Rows.Count;
                nGETcount += nRead;

                for (i = 0; i < nRead; i++)
                {
                    nNu = Convert.ToInt32(dt.Rows[i]["Bun"].ToString().Trim());

                    if (nNuChk == 0)
                    {
                        nNuChk = nNu;
                        strCFdate = dt.Rows[i]["Bday"].ToString().Trim();
                        strBDate = dt.Rows[i]["Bday"].ToString().Trim();
                        strNujuk = "식대";
                    }

                    if (nNu != nNuChk)
                    {
                        #region SUB_TOT_FOOD

                        nRow += 1;
                        if (ssView_Sheet1.RowCount < nRow)
                        {
                            ssView_Sheet1.RowCount = nRow;
                        }

                        ssView_Sheet1.Rows[nRow - 1].BackColor = Color.FromArgb(128, 255, 255);

                        strAMT1 = nStot1.ToString("###,###,##0");
                        strAMT2 = nStot2.ToString("###,###,##0");

                        ssView_Sheet1.Cells[nRow - 1, 1].Text = "누적별계";
                        ssView_Sheet1.Cells[nRow - 1, 7].Text = strAMT1;
                        ssView_Sheet1.Cells[nRow - 1, 8].Text = strAMT2;

                        nStot1 = 0;
                        nStot2 = 0;

                        #endregion

                        nNuChk = nNu;
                        strCFdate = dt.Rows[i]["Bday"].ToString().Trim();
                        strBDate = dt.Rows[i]["Bday"].ToString().Trim();
                        strNujuk = "식대";
                    }

                    if (strCFdate != dt.Rows[i]["Bday"].ToString().Trim())
                    {
                        strCFdate = dt.Rows[i]["Bday"].ToString().Trim();
                        strBDate = dt.Rows[i]["Bday"].ToString().Trim();
                    }

                    #region DATA_MOVE_FODD

                    nRow += 1;
                    if (ssView_Sheet1.RowCount < nRow)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    nAmt1 = (long)VB.Val(dt.Rows[i]["Amt1"].ToString().Trim());
                    nAmt2 = (long)VB.Val(dt.Rows[i]["Amt2"].ToString().Trim());
                    nStot1 += nAmt1;
                    nStot2 += nAmt2;
                    nGtot1 += nAmt1;
                    nGtot2 += nAmt2;

                    strBaseAmt = VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("##,###,##0");
                    strQty = VB.Val(dt.Rows[i]["Qty"].ToString().Trim()).ToString("##0.00");
                    strNal = VB.Val(dt.Rows[i]["Nal"].ToString().Trim()).ToString("##0");
                    strAMT1 = nAmt1.ToString("###,###,##0");
                    strAMT2 = nAmt2.ToString("###,###,##0");

                    ssView_Sheet1.Cells[nRow - 1, 0].Text = strNujuk;
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = strBDate;
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["SunameK"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = strBaseAmt;
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = strQty;
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = strNal;
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = strAMT1;
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = strAMT2;
                    ssView_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["Part"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["GbSpc"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 11].Text = dt.Rows[i]["GbNgt"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 12].Text = dt.Rows[i]["GbGisul"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 13].Text = dt.Rows[i]["GbSelf"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 27].Text = dt.Rows[i]["GbChild"].ToString().Trim();

                    #endregion

                    strNujuk = "";
                    strBDate = "";
                }
                dt.Dispose();
                dt = null;

                if (nGETcount > 0)
                {
                    SUB_TOT_SLIP1(ref nRow, ref nStot1, ref nStot2, ISBR);

                    GRAND_TOT_SLIP1(ref nRow, ref nGtot1, ref nGtot2, ISBR);

                    btnPrint.Enabled = true;
                    ssView.Enabled = true;
                    ssView.Focus();
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }
        
        void eSpdDblClick_ssTrans(int nRow, int nCol)
        {
            int i = 0;
            double nIPDNO = 0;
            double nTRSNO = 0;
            string strSDate = "";
            string strPano = "";
            string strBi = "";
            string strDept = "";

            clsPmpaFunc cpf = new clsPmpaFunc();
            clsIument ci = new clsIument();

            for (i = 0; i < ssTrans_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
            {
                ssTrans_Sheet1.Rows[i].BackColor = Color.FromArgb(255, 255, 255);
            }

            ssTrans_Sheet1.Rows[nRow].BackColor = Color.FromArgb(164, 255, 164);
            strPano = ssInfo_Sheet1.Cells[0, 0].Text.Trim();
            strSDate    = ssTrans_Sheet1.Cells[nRow, 0].Text.Trim();
            strBi       = ssTrans_Sheet1.Cells[nRow, 3].Text.Trim();
            strDept     = ssTrans_Sheet1.Cells[nRow, 4].Text.Trim();
            nIPDNO      = VB.Val(ssTrans_Sheet1.Cells[nRow, 12].Text);
            nTRSNO      = VB.Val(ssTrans_Sheet1.Cells[nRow, 13].Text);
            FstrDrg     = ssTrans_Sheet1.Cells[nRow, 16].Text.Trim();
            FnTRSNO     = nTRSNO;
            
            ci.Read_Ipd_Mst_Trans(clsDB.DbCon, strPano, (long)nTRSNO, "");

            if (clsPmpaType.TIT.InDate !="") dtpSDate.Value = Convert.ToDateTime(clsPmpaType.TIT.InDate);

            if (clsPmpaType.TIT.OutDate == "")
            {
                dtpTDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
            }
            else
            {
                dtpTDate.Value = Convert.ToDateTime(clsPmpaType.TIT.OutDate);
            }

            //cboFnu.SelectedIndex = 0;
            //cboTnu.SelectedIndex = cboFnu.Items.Count - 1;

            if (FstrView != "OK")
            {
                View_Data();
            }

            ssRate_Sheet1.RowCount = 0;
            ssRate_Sheet1.RowCount = 1;

            if (nCol > 0 && nRow > 0)
            {
                strPano = ssInfo_Sheet1.Cells[0, 0].Text.Trim();
                //cpf.Read_Patient_Rate_Chk(clsDB.DbCon, ssRate_Sheet1, "I", strPano, "", strSDate, strBi, strDept, "", 0, (int)nIPDNO, (int)nTRSNO);
            }
            
            if (Convert.ToDateTime(ComFunc.FormatStrToDate(strSDate, "D")) <= Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).AddDays(-1830))
            {
                ComFunc.MsgBox("영수증 재발행은 5년까지만 가능합니다.");

                btnPrint.Enabled = false;
                if (clsVbfunc.JinAmtPrintChk(clsDB.DbCon, clsType.User.Sabun) == true)
                {
                    btnPrint.Enabled = true;
                }
            }
            else
            {
                btnPrint.Enabled = true;
            }

            ssView.Focus();

            if (FstrView == "OK")
            {
                cboFnu.Focus();
            }
        }

        void eSpdDblClick_ssList(int nRow, int nCol)
        {
            int i = 0;
            string strSDate = "";
            double nIPDNO = 0;

            ssList_Sheet1.Cells[0, 0, ssList_Sheet1.RowCount - 1, ssList_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssList_Sheet1.Cells[nRow, 0, nRow, ssList_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            for (i = 0; i <= ssTrans_Sheet1.GetLastNonEmptyRow(NonEmptyItemFlag.Data); i++)
            {
                ssTrans_Sheet1.Rows[i].BackColor = Color.FromArgb(255, 255, 255);
            }

            nIPDNO = VB.Val(ssList_Sheet1.Cells[nRow, 4].Text.Replace(",", ""));
            strSDate = ssList_Sheet1.Cells[nRow, 3].Text.Trim();
            FstrDrg = ssList_Sheet1.Cells[nRow, 6].Text.Trim();

            if (nIPDNO == 0)
            {
                return;
            }

            Display_IPD_Master(nIPDNO);

            if (Convert.ToDateTime(strSDate) <= Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).AddDays(-1830))
            {
                ComFunc.MsgBox("영수증 재발행은 5년까지만 가능합니다.");

                btnPrint.Enabled = false;
                if (clsVbfunc.JinAmtPrintChk(clsDB.DbCon, clsType.User.Sabun) == true)
                {
                    btnPrint.Enabled = true;
                }
            }
            else
            {
                btnPrint.Enabled = true;
            }
        }

        void ssView_ButtonClicked(object sender, EditorNotifyEventArgs e)
        {
            int i = 0;
            int nNal = 0;
            int nNal_2 = 0;
            double nAmt = 0;
            double nAmt2 = 0;
            double nQty = 0;
            double nTAmt = 0;
            double nTAmt2 = 0;
            double nQty_2 = 0;
            double nAmt_2 = 0;
            double nAmt2_2 = 0;
            string strSucode = "";
            string strSuNext = "";
            string strGbNgt = "";
            string strGBSELF = "";
            string strBDate = "";
            string strBun = "";
            string strDan0 = "";
            string strChild = "";

            if (FstrChk == "")
            {
                strSucode = ssView_Sheet1.Cells[e.Row, 1].Text.Trim();
                strBDate = ssView_Sheet1.Cells[e.Row, 2].Text.Trim();
                nQty = VB.Val(ssView_Sheet1.Cells[e.Row, 3].Text.Trim().Replace(",", ""));
                nNal = (int)VB.Val(ssView_Sheet1.Cells[e.Row, 4].Text.Trim().Replace(",", ""));
                nQty_2 = VB.Val(ssView_Sheet1.Cells[e.Row, 5].Text.Trim().Replace(",", ""));
                nNal_2 = (int)VB.Val(ssView_Sheet1.Cells[e.Row, 6].Text.Trim().Replace(",", ""));
                nAmt_2 = VB.Val(VB.TR(ssView_Sheet1.Cells[e.Row, 7].Text, ",", ""));
                nAmt2_2 = VB.Val(VB.TR(ssView_Sheet1.Cells[e.Row, 8].Text, ",", ""));
                strBun = ssView_Sheet1.Cells[e.Row, 20].Text.Trim();
                strSuNext = ssView_Sheet1.Cells[e.Row, 23].Text.Trim();
                strGbNgt = ssView_Sheet1.Cells[e.Row, 11].Text.Trim();
                strGBSELF = ssView_Sheet1.Cells[e.Row, 13].Text.Trim();
                strDan0 = ssView_Sheet1.Cells[e.Row, 25].Text.Trim();
                strChild = ssView_Sheet1.Cells[e.Row, 27].Text.Trim();

                if (Convert.ToBoolean(ssView_Sheet1.Cells[e.Row, 15].Value) == true)
                {
                   
                    if (cIAcct.Account_Process_JSIM(clsDB.DbCon, strSucode, strSuNext, strGbNgt, strGBSELF, strBDate, nQty, nNal, 1, "", "", strChild,"") == false)
                    {
                        ComFunc.MsgBox("수가항목 계산실패.", "오류발생");
                        return;
                    }

                    if (strBun == "22" || strBun == "23")
                    {
                        nAmt = VB.Val(VB.TR(ssView_Sheet1.Cells[e.Row, 7].Text, ",", ""));
                        nAmt2 = VB.Val(VB.TR(ssView_Sheet1.Cells[e.Row, 8].Text, ",", ""));
                        ssView_Sheet1.Cells[e.Row, 18].Text = nAmt.ToString("###,###,###");
                        ssView_Sheet1.Cells[e.Row, 19].Text = nAmt2.ToString("###,###,###");
                    }
                    else
                    {
                        if (strDan0 == "Y")
                        {
                            nAmt = VB.Val(VB.TR(ssView_Sheet1.Cells[e.Row, 7].Text, ",", ""));
                            nAmt2 = VB.Val(VB.TR(ssView_Sheet1.Cells[e.Row, 8].Text, ",", ""));
                            nAmt = nAmt * nQty * nNal;
                            ssView_Sheet1.Cells[e.Row, 18].Text = nAmt.ToString("###,###,###");
                            ssView_Sheet1.Cells[e.Row, 19].Text = nAmt2.ToString("###,###,###");
                        }
                        else
                        {
                            nAmt = G7AMT * nQty * nNal;
                            nAmt2 = G7TAMT;
                            ssView_Sheet1.Cells[e.Row, 18].Text = nAmt.ToString("###,###,###");
                            ssView_Sheet1.Cells[e.Row, 19].Text = nAmt2.ToString("###,###,###");
                        }
                    }

                    if (nNal == nNal_2 && nQty == nQty_2 && nAmt != nAmt_2 || nAmt2 != nAmt2_2)
                    {
                        ssView_Sheet1.Cells[e.Row, 18].BackColor = Color.FromArgb(255, 0, 0);
                        FnCnt_SU += 1;
                    }
                }
                else
                {
                    nQty = VB.Val(ssView_Sheet1.Cells[e.Row, 5].Text.Replace(",", ""));
                    nNal = (int)VB.Val(ssView_Sheet1.Cells[e.Row, 6].Text.Replace(",", ""));
                    ssView_Sheet1.Cells[e.Row, 16].Text = nQty.ToString("#,##0.00");
                    ssView_Sheet1.Cells[e.Row, 17].Text = nNal.ToString("##0");
                    ssView_Sheet1.Cells[e.Row, 18].Text = "";
                    ssView_Sheet1.Cells[e.Row, 19].Text = "";
                }
            }

            nTAmt = 0;
            nTAmt2 = 0;

            for (i = 0; i <= ssView_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
            {
                if (ssView_Sheet1.Cells[i, 15].Text == "1")
                {
                    nTAmt += VB.Val(VB.TR(ssView_Sheet1.Cells[i, 18].Text, ",", ""));
                    nTAmt2 += VB.Val(VB.TR(ssView_Sheet1.Cells[i, 19].Text, ",", ""));
                }
            }

            ssView_Sheet1.Cells[ssView_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data), 18].Text = nTAmt.ToString("###,###,###,##0");
            ssView_Sheet1.Cells[ssView_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data), 19].Text = nTAmt2.ToString("###,###,###,##0");

        }

        void ssView_EditModeOff(object sender, EventArgs e)
        {
            int i = 0;
            int nNal = 0;
            double nAmt = 0;
            double nAmt2 = 0;
            double nQty = 0;
            double nPice = 0;
            double nTAmt = 0;
            double nTAmt2 = 0;
            string strCHK = "";
            string strSucode = "";
            string strSuNext = "";
            string strGbNgt = "";
            string strGBSELF = "";
            string strBDate = "";
            string strBun = "";
            string strDan0 = "";
            string strChild = "";

            nPice = VB.Val(ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 4].Text.Replace(",", ""));
            strCHK = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 15].Text;
            nQty = VB.Val(ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 16].Text.Replace(",", ""));
            nNal = (int)VB.Val(ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 17].Text.Replace(",", ""));
            strSucode = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 1].Text;
            strBDate = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 2].Text;
            strBun = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 20].Text.Trim();
            strSuNext = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 23].Text.Trim();
            strGbNgt = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 11].Text.Trim();
            strGBSELF = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 13].Text.Trim();
            strDan0 = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 25].Text.Trim();
            strChild = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 27].Text.Trim();

            if (strCHK == "1" && (ssView_Sheet1.ActiveColumnIndex == 16 || ssView_Sheet1.ActiveColumnIndex == 17))
            {
                if (cIAcct.Account_Process_JSIM(clsDB.DbCon, strSucode, strSuNext, strGbNgt, strGBSELF, strBDate, nQty, nNal, 1, "", "", strChild,"") == false)
                {
                    ComFunc.MsgBox("수가항목 계산실패.", "오류발생");
                    return;
                }

                if (strBun == "22" || strBun == "23")
                {
                    nAmt = G7AMT;
                    nAmt2 = G7TAMT;
                    ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 18].Text = nAmt.ToString("###,###,###,###");
                    ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 19].Text = nAmt2.ToString("###,###,###,###");
                }
                else
                {
                    if (strDan0 == "Y")
                    {
                        nAmt = VB.Val(VB.TR(ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 7].Text, ",", ""));
                        nAmt2 = VB.Val(VB.TR(ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 8].Text, ",", ""));
                        nAmt = nAmt * nQty * nNal;
                        ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 18].Text = nAmt.ToString("###,###,###,###");
                        ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 19].Text = nAmt2.ToString("###,###,###,###");
                    }
                    else
                    {
                        nAmt = G7AMT * nQty * nNal;
                        nAmt2 = G7TAMT;
                        ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 18].Text = nAmt.ToString("###,###,###,###");
                        ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 19].Text = nAmt2.ToString("###,###,###,###");
                    }
                }
            }

            nTAmt = 0;
            nTAmt2 = 0;

            for (i = 0; i < ssView_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
            {
                if (ssView_Sheet1.Cells[i, 15].Text == "1")
                {
                    nTAmt += VB.Val(VB.TR(ssView_Sheet1.Cells[i, 18].Text, ",", ""));
                    nTAmt2 += VB.Val(VB.TR(ssView_Sheet1.Cells[i, 19].Text, ",", ""));
                }
            }

            ssView_Sheet1.Cells[ssView_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data), 18].Text = nTAmt.ToString("###,###,###,##0");
            ssView_Sheet1.Cells[ssView_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data), 19].Text = nTAmt2.ToString("###,###,###,##0");

        }

        void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            int i = 0;
            string strCHK = "";

            FnCnt_SU = 0;

            if (e.ColumnHeader == true && e.Column == 15)
            {
                for (i = 0; i <= ssView_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
                {
                    if (i == 0)
                    {
                        if (ssView_Sheet1.Cells[i, 15].Text != "1")
                        {
                            strCHK = "1";  //all
                        }
                        else
                        {
                            strCHK = "2";
                        }
                    }

                    if (ssView_Sheet1.Cells[i, 3].Text != "")
                    {
                        if (strCHK == "1")
                        {
                            ssView_Sheet1.Cells[i, 15].Text = "1";
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i, 15].Text = "0";
                        }
                    }
                }
            }

            if (FnCnt_SU != 0)
            {
                ComFunc.MsgBox("계산금액이 선택한 항목중 다른것이 있습니다. 확인 후 수동 수정하십시오!!");
            }
        }

        void Read_IPD_SLIP6()
        {
            int i = 0;
            DataTable dt = null;
            DataTable dt2 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nNu = 0;
            int nNuChk = 0;
            int k = 0;
            int nRow = 0;
            int nRead = 0;
            int nCNT2 = 0;
            int nInitNo = 0;
            long nAmt1 = 0;
            long nAmt2 = 0;
            long nStot1 = 0;
            long nStot2 = 0;
            long nGtot1 = 0;
            long nGtot2 = 0;
            string strNujuk = "";
            string strBaseAmt = "";
            string strQty = "";
            string strNal = "";
            string strAMT1 = "";
            string strAMT2 = "";
            string strBun = "";

            clsPmpaType.ISBR ISBR = new clsPmpaType.ISBR();
            clsIument cIUM = new clsIument();

            ssView_Sheet1.ColumnHeader.Cells[0, 1].Text = "수가코드";
            ssView_Sheet1.ColumnHeader.Cells[0, 2].Text = "표준코드";

            nInitNo = 66;

            nNuChk = 0;
            nStot1 = 0;
            nStot2 = 0;
            nGtot1 = 0;
            nGtot2 = 0;
            nRow = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Sucode, I.Sunext, SunameK,";
                SQL = SQL + ComNum.VBLF + "        Hcode, BaseAmt, Qty,";
                SQL = SQL + ComNum.VBLF + "        GbSpc, GbNgt, GbGisul,";
                SQL = SQL + ComNum.VBLF + "        GbSelf, GbChild, Nu,";
                SQL = SQL + ComNum.VBLF + "        Bun, SUM(Nal) Nalsu, SUM(Amt1) Amtt1,";
                SQL = SQL + ComNum.VBLF + "        SUM(Amt2) Amtt2";
                SQL = SQL + ComNum.VBLF + "  FROM ";
                if (clsPmpaType.IMST.GbOldSlip == false)
                {
                    SQL = SQL + ComNum.VBLF + "  ( SELECT BDATE, IPDNO,  SUCODE,";
                    SQL = SQL + ComNum.VBLF + "           SUNEXT, BASEAMT, SUM(QTY * NAL) QTY ,";
                    SQL = SQL + ComNum.VBLF + "           DECODE(DIV,'0',1,'',1, DIV) DIV, GBSPC, GBNGT,";
                    SQL = SQL + ComNum.VBLF + "           GBGISUL, GBSELF, GBCHILD,";
                    SQL = SQL + ComNum.VBLF + "           NU, BUN, 1 NAL,";
                    SQL = SQL + ComNum.VBLF + "           SUM(AMT1) AMT1, SUM(AMT2) AMT2";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";

                    if (chkAll.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "  AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "  AND TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                    }

                    if (strFnu.Trim() != "" && strTnu == "00")
                    {
                        SQL = SQL + ComNum.VBLF + "   AND NU = '" + strFnu + "' ";
                    }
                    else if (strFnu.Trim() != "" && strTnu != "00")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND Nu >= '" + strFnu + "'";
                        SQL = SQL + ComNum.VBLF + "    AND Nu <= '" + strTnu + "'";
                    }

                    if (strSDate != "")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strSDate + "','yyyy-mm-dd')   ";
                    }
                    if (strEdate != "")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strEdate + "','yyyy-mm-dd')   ";
                    }

                    SQL = SQL + ComNum.VBLF + "   AND  NAL IN ( '1','-1')  ";
                    SQL = SQL + ComNum.VBLF + " GROUP BY BDATE, IPDNO,  SUCODE, SUNEXT, BASEAMT,  DECODE(DIV,'0',1,'',1, DIV),";
                    SQL = SQL + ComNum.VBLF + "          GBSPC, GBNGT, GBGISUL, GBSELF, GBCHILD, NU, BUN";
                    SQL = SQL + ComNum.VBLF + " HAVING SUM(QTY * NAL) <> 0 ";

                    SQL = SQL + ComNum.VBLF + " UNION ALL ";

                    SQL = SQL + ComNum.VBLF + " SELECT BDATE, IPDNO,  SUCODE,";
                    SQL = SQL + ComNum.VBLF + "        SUNEXT, BASEAMT, QTY,";
                    SQL = SQL + ComNum.VBLF + "        DIV, GBSPC, GBNGT,";
                    SQL = SQL + ComNum.VBLF + "        GBGISUL, GBSELF, GBCHILD,";
                    SQL = SQL + ComNum.VBLF + "        NU, BUN, NAL,";
                    SQL = SQL + ComNum.VBLF + "        AMT1, AMT2";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP  ";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";

                    if (chkAll.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "  AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "  AND TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                    }

                    if (strFnu.Trim() != "" && strTnu == "00")
                    {
                        SQL = SQL + ComNum.VBLF + "   AND NU = '" + strFnu + "' ";
                    }
                    else if (strFnu.Trim() != "" && strTnu != "00")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND Nu >= '" + strFnu + "'";
                        SQL = SQL + ComNum.VBLF + "    AND Nu <= '" + strTnu + "'";
                    }

                    if (strSDate != "")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strSDate + "','yyyy-mm-dd')   ";
                    }
                    if (strEdate != "")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strEdate + "','yyyy-mm-dd')   ";
                    }

                    SQL = SQL + ComNum.VBLF + "   AND NAL NOT IN ('1' ,'-1') ";
                    SQL = SQL + ComNum.VBLF + " ) I,  " + ComNum.DB_PMPA + "BAS_SUN B";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " ( SELECT BDATE, IPDNO,  SUCODE, SUNEXT,";
                    SQL = SQL + ComNum.VBLF + "          BASEAMT, SUM(QTY * NAL) QTY , DECODE(DIV,'0',1,'',1, DIV) DIV,";
                    SQL = SQL + ComNum.VBLF + "          GBSPC, GBNGT, GBGISUL,";
                    SQL = SQL + ComNum.VBLF + "          GBSELF, GBCHILD, NU,";
                    SQL = SQL + ComNum.VBLF + "          BUN, 1 NAL, SUM(AMT1) AMT1,";
                    SQL = SQL + ComNum.VBLF + "          SUM(AMT2) AMT2";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_OLD_SLIP  ";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";

                    if (chkAll.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "  AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "  AND TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                    }

                    if (strFnu.Trim() != "" && strTnu == "00")
                    {
                        SQL = SQL + ComNum.VBLF + "   AND NU = '" + strFnu + "' ";
                    }
                    else if (strFnu.Trim() != "" && strTnu != "00")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND Nu >= '" + strFnu + "'";
                        SQL = SQL + ComNum.VBLF + "    AND Nu <= '" + strTnu + "'";
                    }

                    if (strSDate != "")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strSDate + "','yyyy-mm-dd')   ";
                    }
                    if (strEdate != "")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strEdate + "','yyyy-mm-dd')   ";
                    }

                    SQL = SQL + ComNum.VBLF + "   AND  NAL IN ( '1','-1')  ";
                    //저가약제 제외코드 2011-04-09
                    SQL = SQL + ComNum.VBLF + "   AND TRIM(SUNEXT) NOT IN ( SELECT CODE FROM ADMIN.BAS_BCODE WHERE GUBUN ='원무영수제외코드') ";

                    SQL = SQL + ComNum.VBLF + " GROUP BY BDATE, IPDNO,  SUCODE, SUNEXT, BASEAMT, QTY , DECODE(DIV,'0',1,'',1, DIV) ,";
                    SQL = SQL + ComNum.VBLF + "          GBSPC, GBNGT, GBGISUL, GBSELF, GBCHILD, NU, BUN  ";
                    SQL = SQL + ComNum.VBLF + " HAVING SUM(QTY * NAL) <> 0 ";

                    SQL = SQL + ComNum.VBLF + " UNION ALL ";

                    SQL = SQL + ComNum.VBLF + " SELECT BDATE, IPDNO, SUCODE,";
                    SQL = SQL + ComNum.VBLF + "        SUNEXT, BASEAMT, QTY,";
                    SQL = SQL + ComNum.VBLF + "        DIV, GBSPC, GBNGT,";
                    SQL = SQL + ComNum.VBLF + "        GBGISUL, GBSELF, GBCHILD,";
                    SQL = SQL + ComNum.VBLF + "        NU, BUN, NAL,";
                    SQL = SQL + ComNum.VBLF + "        AMT1, AMT2";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP  ";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";

                    if (chkAll.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "  AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "  AND TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                    }

                    if (strFnu.Trim() != "" && strTnu == "00")
                    {
                        SQL = SQL + ComNum.VBLF + "   AND NU = '" + strFnu + "' ";
                    }
                    else if (strFnu.Trim() != "" && strTnu != "00")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND Nu >= '" + strFnu + "'";
                        SQL = SQL + ComNum.VBLF + "    AND Nu <= '" + strTnu + "'";
                    }

                    if (strSDate != "")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strSDate + "','yyyy-mm-dd')   ";
                    }
                    if (strEdate != "")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strEdate + "','yyyy-mm-dd')   ";
                    }
                    SQL = SQL + ComNum.VBLF + "   AND NAL NOT IN ('1' ,'-1') ";

                    SQL = SQL + ComNum.VBLF + " ) I,  " + ComNum.DB_PMPA + "BAS_SUN B";
                }
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1"; //간호행위제외
                SQL = SQL + ComNum.VBLF + "    AND  (b.WONCODE NOT IN('1118') OR b.WONCODE IS NULL )"; //간호행위제외
                SQL = SQL + ComNum.VBLF + "    AND I.Sunext = B.Sunext";

                //저가약제 제외코드 2011-04-09
                SQL = SQL + ComNum.VBLF + "   AND TRIM(i.SUNEXT) NOT IN ( SELECT CODE FROM ADMIN.BAS_BCODE WHERE GUBUN ='원무영수제외코드') ";

                SQL = SQL + ComNum.VBLF + "  GROUP BY Sucode, I.Sunext, SunameK, Hcode, BaseAmt, Qty,";
                SQL = SQL + ComNum.VBLF + "           GbSpc, GbNgt, GbGisul, GbSelf, GbChild, Nu, Bun";

                SQL = SQL + ComNum.VBLF + "  HAVING  SUM(QTY * NAL) <> 0";
                SQL = SQL + ComNum.VBLF + "  ORDER BY  Nu, Bun, Sucode";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                nRead = dt.Rows.Count;
                nGETcount += nRead;

                for (i = 0; i < nRead; i++)
                {
                    strBun = dt.Rows[i]["BUN"].ToString().Trim();

                    if (VB.Val(dt.Rows[i]["Nu"].ToString().Trim()) != 0)
                    {
                        nNu = (int)VB.Val(dt.Rows[i]["Nu"].ToString().Trim());
                    }

                    if (nNuChk == 0)
                    {
                        nNuChk = nNu;
                        strNujuk = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_누적행위명", nNu.ToString("00"));
                    }

                    if (nNu != nNuChk)
                    {
                        SUB_TOT_SLIP1(ref nRow, ref nStot1, ref nStot2, ISBR);

                        nNuChk = nNu;
                        strNujuk = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_누적행위명", nNu.ToString("00"));
                    }

                    //심사계 요청(2004-06-03)
                    if (nNu == 6)
                    {
                        //마취 합산분 풀어서 Display
                        #region DATA_MARCHI_MOVE_SLIP1

                        if (nInitNo != 66)
                        {
                            continue;
                        }

                        nInitNo = 88;

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT Sucode Sucode1, I.Sunext Sunext1, SunameK SunameK1,";
                        SQL = SQL + ComNum.VBLF + "        Hcode Hcode1, BaseAmt BaseAmt1, Qty Qty1,";
                        SQL = SQL + ComNum.VBLF + "        GbSpc GbSpc1, GbNgt GbNgt1, GbGisul GbGisul1,";
                        SQL = SQL + ComNum.VBLF + "        GbSelf GbSelf1, GbChild GbChild1, Nu Nu1,";
                        SQL = SQL + ComNum.VBLF + "        Nal Nalsu1, Amt1 Amtt11, Amt2 Amtt21,";
                        SQL = SQL + ComNum.VBLF + "        BUN";
                        SQL = SQL + ComNum.VBLF + "   FROM ";
                        SQL = SQL + ComNum.VBLF + "  ( SELECT BDATE, IPDNO,  SUCODE,";
                        SQL = SQL + ComNum.VBLF + "           SUNEXT, BASEAMT, SUM(QTY * NAL) QTY ,";
                        SQL = SQL + ComNum.VBLF + "           DECODE(DIV,'0',1,'',1, DIV) DIV, GBSPC, GBNGT,";
                        SQL = SQL + ComNum.VBLF + "           GBGISUL, GBSELF, GBCHILD,";
                        SQL = SQL + ComNum.VBLF + "           NU, BUN, 1 NAL,";
                        SQL = SQL + ComNum.VBLF + "           SUM(AMT1) AMT1, SUM(AMT2) AMT2";
                        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP  ";
                        SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";

                        if (chkAll.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "  AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "  AND TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                        }

                        SQL = SQL + ComNum.VBLF + "    AND Nu = '06'";
                        //*------------( 96/01/31 기간 Check 추가 lyj)--------------------------*
                        if (strSDate != "")
                        {
                            SQL = SQL + ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strSDate + "','yyyy-mm-dd')";
                        }
                        if (strEdate != "")
                        {
                            SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strEdate + "','yyyy-mm-dd')";
                        }
                        SQL = SQL + ComNum.VBLF + "   AND  NAL IN ( '1','-1')";
                        SQL = SQL + ComNum.VBLF + " GROUP BY BDATE, IPDNO,  SUCODE, SUNEXT, BASEAMT, QTY , DECODE(DIV,'0',1,'',1, DIV),";
                        SQL = SQL + ComNum.VBLF + "          GBSPC, GBNGT, GBGISUL, GBSELF, GBCHILD, NU, BUN  ";
                        SQL = SQL + ComNum.VBLF + " HAVING SUM(QTY * NAL) <> 0 ";

                        SQL = SQL + ComNum.VBLF + " UNION ALL ";

                        SQL = SQL + ComNum.VBLF + " SELECT BDATE, IPDNO, SUCODE,";
                        SQL = SQL + ComNum.VBLF + "        SUNEXT, BASEAMT, QTY,";
                        SQL = SQL + ComNum.VBLF + "        DIV, GBSPC, GBNGT,";
                        SQL = SQL + ComNum.VBLF + "        GBGISUL, GBSELF, GBCHILD,";
                        SQL = SQL + ComNum.VBLF + "        NU, BUN, NAL,";
                        SQL = SQL + ComNum.VBLF + "        AMT1, AMT2";
                        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP";

                        if (chkAll.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "  WHERE IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "  WHERE TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                        }

                        SQL = SQL + ComNum.VBLF + "    AND Nu   = '06'";
                        //*------------( 96/01/31 기간 Check 추가 lyj)--------------------------*
                        if (strSDate != "")
                        {
                            SQL = SQL + ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strSDate + "','yyyy-mm-dd')";
                        }
                        if (strEdate != "")
                        {
                            SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strEdate + "','yyyy-mm-dd')";
                        }
                        SQL = SQL + ComNum.VBLF + "   AND NAL NOT IN ('1' ,'-1')  ) I , " + ComNum.DB_PMPA + "BAS_SUN B";
                        SQL = SQL + ComNum.VBLF + "  WHERE I.Sunext = B.Sunext";
                        SQL = SQL + ComNum.VBLF + "  ORDER BY Nu, Sucode, I.Sunext";

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt2.Rows.Count == 0)
                        {
                            dt2.Dispose();
                            dt2 = null;
                        }
                        else
                        {
                            nCNT2 = dt2.Rows.Count;

                            for (k = 0; k < nCNT2; k++)
                            {
                                #region DATA_MOVE_SLIP1_2

                                nRow += 1;
                                if (ssView_Sheet1.RowCount < nRow)
                                {
                                    ssView_Sheet1.RowCount = nRow;
                                }

                                if (k > 0)
                                {
                                    strNujuk = "          ";
                                }

                                nAmt1 = (long)VB.Val(dt2.Rows[k]["Amtt11"].ToString().Trim());
                                nAmt2 = (long)VB.Val(dt2.Rows[k]["Amtt21"].ToString().Trim());

                                nStot1 += nAmt1;
                                nStot2 += nAmt2;
                                nGtot1 += nAmt1;
                                nGtot2 += nAmt2;

                                strBaseAmt = VB.Val(dt2.Rows[k]["BaseAmt1"].ToString().Trim()).ToString("##,###,##0");
                                strQty = VB.Val(dt2.Rows[k]["Qty1"].ToString().Trim()).ToString("##0.00");
                                strNal = VB.Val(dt2.Rows[k]["Nalsu1"].ToString().Trim()).ToString("##0");
                                strAMT1 = nAmt1.ToString("###,###,##0");
                                strAMT2 = nAmt2.ToString("###,###,##0");

                                ssView_Sheet1.Cells[nRow - 1, 0].Text = strNujuk;
                                ssView_Sheet1.Cells[nRow - 1, 1].Text = dt2.Rows[k]["Sucode1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 2].Text = dt2.Rows[k]["Hcode1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 3].Text = dt2.Rows[k]["SunameK1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 4].Text = strBaseAmt;
                                ssView_Sheet1.Cells[nRow - 1, 5].Text = strQty;
                                ssView_Sheet1.Cells[nRow - 1, 6].Text = strNal;
                                ssView_Sheet1.Cells[nRow - 1, 7].Text = strAMT1;
                                ssView_Sheet1.Cells[nRow - 1, 8].Text = strAMT2;
                                ssView_Sheet1.Cells[nRow - 1, 10].Text = dt2.Rows[k]["GbSpc1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 11].Text = dt2.Rows[k]["GbNgt1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 12].Text = dt2.Rows[k]["GbGisul1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 13].Text = dt2.Rows[k]["GbSelf1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 20].Text = dt2.Rows[k]["BUN"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 27].Text = dt2.Rows[k]["GbChild1"].ToString().Trim();

                                //마취코드는 변경 안 됨
                                ssView_Sheet1.Cells[nRow - 1, 16, nRow - 1, 17].Locked = true;

                                #endregion
                            }

                            dt2.Dispose();
                            dt2 = null;
                        }



                        #endregion
                    }
                    else
                    {
                        #region DATA_MOVE_SLIP1

                        nRow += 1;
                        if (ssView_Sheet1.RowCount < nRow)
                        {
                            ssView_Sheet1.RowCount = nRow;
                        }

                        nAmt1 = (long)VB.Val(dt.Rows[i]["Amtt1"].ToString().Trim());
                        nAmt2 = (long)VB.Val(dt.Rows[i]["Amtt2"].ToString().Trim());

                        nStot1 += nAmt1;
                        nStot2 += nAmt2;
                        nGtot1 += nAmt1;
                        nGtot2 += nAmt2;

                        strBaseAmt = VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("##,###,##0");
                        strQty = VB.Val(dt.Rows[i]["Qty"].ToString().Trim()).ToString("##0.00");
                        strNal = VB.Val(dt.Rows[i]["Nalsu"].ToString().Trim()).ToString("##0");
                        strAMT1 = nAmt1.ToString("###,###,##0");
                        strAMT2 = nAmt2.ToString("###,###,##0");

                        ssView_Sheet1.Cells[nRow - 1, 0].Text = strNujuk;
                        ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[k]["Sucode"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[k]["Hcode"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[k]["SunameK"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 4].Text = strBaseAmt;
                        ssView_Sheet1.Cells[nRow - 1, 5].Text = strQty;
                        ssView_Sheet1.Cells[nRow - 1, 6].Text = strNal;
                        ssView_Sheet1.Cells[nRow - 1, 7].Text = strAMT1;
                        ssView_Sheet1.Cells[nRow - 1, 8].Text = strAMT2;
                        ssView_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[k]["GbSpc"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 11].Text = dt.Rows[k]["GbNgt"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 12].Text = dt.Rows[k]["GbGisul"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 13].Text = dt.Rows[k]["GbSelf"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 20].Text = dt.Rows[k]["BUN"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 27].Text = dt.Rows[k]["GbChild"].ToString().Trim();


                        #endregion
                    }

                    strNujuk = "";
                }

                dt.Dispose();
                dt = null;

                if (nGETcount > 0)
                {
                    SUB_TOT_SLIP1(ref nRow, ref nStot1, ref nStot2, ISBR);

                    GRAND_TOT_SLIP1(ref nRow, ref nGtot1, ref nGtot2, ISBR);

                    btnPrint.Enabled = true;
                    ssView.Enabled = true;
                }

                //수동명세서 사유 SELECT
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT REMARK";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_SU_SLIP ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND PANO = '" + clsPmpaType.TIT.Pano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                SQL = SQL + ComNum.VBLF + "   AND TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                SQL = SQL + ComNum.VBLF + "   AND REMARK IS NOT NULL ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    txtRemark.Text = dt.Rows[0]["REMARK"].ToString().Trim();
                }
                else
                {
                    txtRemark.Text = "";
                }

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }


        }

        void Read_IPD_SLIP8()
        {
            //마취 합산
            int i = 0;
            DataTable dt = null;
            DataTable dt2 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nNu = 0;
            int nNuChk = 0;
            int k = 0;
            int nRow = 0;
            int nCNT2 = 0;
            int nInitNo = 0;
            int nRead = 0;
            long nAmt1 = 0;
            long nAmt2 = 0;
            long nStot1 = 0;
            long nStot2 = 0;
            long nGtot1 = 0;
            long nGtot2 = 0;
            string strNujuk = "";
            string strBaseAmt = "";
            string strQty = "";
            string strNal = "";
            string strAMT1 = "";
            string strAMT2 = "";
            string strBun = "";
            string str100 = "";
            string strF = "";

            clsIuSentChk cisc = new clsIuSentChk();
            ComFunc cf = new ComFunc();
            clsPmpaType.ISBR ISBR = new clsPmpaType.ISBR();
            clsIument cIUM = new clsIument();

            ssView_Sheet1.ColumnHeader.Cells[0, 1].Text = "수가코드";
            ssView_Sheet1.ColumnHeader.Cells[0, 2].Text = "표준코드";

            nInitNo = 66;

            nNuChk = 0;
            nStot1 = 0;
            nStot2 = 0;
            nGtot1 = 0;
            nGtot2 = 0;
            nRow = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Sucode, I.Sunext, SunameK,";
                SQL = SQL + ComNum.VBLF + "        BCode, BaseAmt, Qty,";
                SQL = SQL + ComNum.VBLF + "        I.GBSugbs, b.Sugbp, GbSpc,";
                SQL = SQL + ComNum.VBLF + "        GbNgt, GbGisul, GbSelf,";
                SQL = SQL + ComNum.VBLF + "        GbChild, Nu, Bun,";
                SQL = SQL + ComNum.VBLF + "        DRG100, DRGF, SUM(Nal) Nalsu,";
                SQL = SQL + ComNum.VBLF + "        SUM(Amt1) Amtt1, SUM(Amt2) Amtt2";

                if (clsPmpaType.IMST.GbOldSlip == false)
                {
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP I, " + ComNum.DB_PMPA + "BAS_SUN B";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_OLD_SLIP I, " + ComNum.DB_PMPA + "BAS_SUN B";
                }

                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";

                if (chkAll.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                }

                SQL = SQL + ComNum.VBLF + "    AND Nu >= '" + strFnu + "'";
                SQL = SQL + ComNum.VBLF + "    AND Nu <= '" + strTnu + "'";
                //*------------( 96/01/31 기간 Check 추가 lyj)--------------------------*

                if (strSDate != "")
                {
                    SQL = SQL + ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strSDate + "','yyyy-mm-dd')   ";
                }

                if (strEdate != "")
                {
                    SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strEdate + "','yyyy-mm-dd')   ";
                }

                //*-------------------------------------------------------------------------*
                SQL = SQL + ComNum.VBLF + "    AND (b.WONCODE NOT IN('1118') OR b.WONCODE IS NULL )"; //간호행위제외
                SQL = SQL + ComNum.VBLF + "    AND I.Sunext = B.Sunext";

                //저가약제 제외코드 2011-04-09
                SQL = SQL + ComNum.VBLF + "   AND TRIM(i.SUNEXT) NOT IN ( SELECT CODE FROM ADMIN.BAS_BCODE WHERE GUBUN ='원무영수제외코드') ";

                SQL = SQL + ComNum.VBLF + "  GROUP BY Sucode, I.Sunext, SunameK, Bcode, BaseAmt, Qty,I.GBSugbs,b.Sugbp,  ";
                SQL = SQL + ComNum.VBLF + "           GbSpc, GbNgt, GbGisul, GbSelf, GbChild, Nu, Bun,DRG100,DRGF  ";

                SQL = SQL + ComNum.VBLF + "  ORDER BY  Nu, Bun, Sucode";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                nRead = dt.Rows.Count;

                nGETcount += nRead;

                for (i = 0; i < nRead; i++)
                {
                    strBun = dt.Rows[i]["Bun"].ToString().Trim();
                    str100 = dt.Rows[i]["DRG100"].ToString().Trim();
                    strF = dt.Rows[i]["DRGF"].ToString().Trim();

                    if (DRG.READ_DRG_100(clsDB.DbCon, dt.Rows[i]["Sucode"].ToString().Trim(), "", clsPmpaType.TIT.InDate) == "OK")
                    {
                        str100 = "Y";
                    }
                    else
                    {
                        str100 = "";
                    }
                    if (VB.Val(dt.Rows[i]["Nu"].ToString().Trim()) != 0)
                    {
                        nNu = (int)VB.Val(dt.Rows[i]["Nu"].ToString().Trim());
                    }

                    if (nNuChk == 0)
                    {
                        nNuChk = nNu;
                        strNujuk = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_누적행위명", nNu.ToString("00"));
                    }

                    if (nNu != nNuChk)
                    {
                        SUB_TOT_SLIP1(ref nRow, ref nStot1, ref nStot2, ISBR);

                        nNuChk = nNu;
                        strNujuk = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_누적행위명", nNu.ToString("00"));
                    }

                    //심사계 요청
                    if (nNu == 6)
                    {
                        //마취 합산분 풀어서 Display
                        #region DATA_MARCHI_MOVE_SLIP1

                        if (nInitNo != 66)
                        {
                            continue;
                        }

                        nInitNo = 88;

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT Sucode Sucode1, I.Sunext Sunext1, SunameK SunameK1,";
                        SQL = SQL + ComNum.VBLF + "        Hcode Hcode1, BaseAmt BaseAmt1, Qty Qty1,";
                        SQL = SQL + ComNum.VBLF + "        GbSpc GbSpc1, GbNgt GbNgt1, GbGisul GbGisul1,";
                        SQL = SQL + ComNum.VBLF + "        GbSelf GbSelf1, GbChild GbChild1, Nu Nu1,";
                        SQL = SQL + ComNum.VBLF + "        Nal Nalsu1, Amt1 Amtt11, Amt2 Amtt21,";
                        SQL = SQL + ComNum.VBLF + "        BUN";
                        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP I,     ";
                        SQL = SQL + ComNum.VBLF + "       " + ComNum.DB_PMPA + "BAS_SUN B           ";
                        SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";

                        if (chkAll.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "  AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "  AND TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                        }

                        SQL = SQL + ComNum.VBLF + "    AND Nu = '06'";
                        SQL = SQL + ComNum.VBLF + "    AND I.Sunext = B.Sunext";
                        //*------------( 96/01/31 기간 Check 추가 lyj)--------------------------*
                        if (strSDate != "")
                        {
                            SQL = SQL + ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strSDate + "','yyyy-mm-dd')";
                        }
                        if (strEdate != "")
                        {
                            SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strEdate + "','yyyy-mm-dd')";
                        }
                        SQL = SQL + ComNum.VBLF + "  ORDER BY Nu, Sucode, I.Sunext";

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt2.Rows.Count == 0)
                        {
                            dt2.Dispose();
                            dt2 = null;
                        }
                        else
                        {
                            nCNT2 = dt2.Rows.Count;

                            for (k = 0; k < nCNT2; k++)
                            {
                                #region DATA_MOVE_SLIP1_2

                                nRow += 1;
                                if (ssView_Sheet1.RowCount < nRow)
                                {
                                    ssView_Sheet1.RowCount = nRow;
                                }

                                if (k > 0)
                                {
                                    strNujuk = "          ";
                                }

                                nAmt1 = (long)VB.Val(dt2.Rows[k]["Amtt11"].ToString().Trim());
                                nAmt2 = (long)VB.Val(dt2.Rows[k]["Amtt21"].ToString().Trim());

                                strQty = VB.Val(dt2.Rows[k]["Qty1"].ToString().Trim()).ToString("##0.00");
                                strNal = VB.Val(dt2.Rows[k]["Nalsu1"].ToString().Trim()).ToString("##0");

                                if (FstrDrg == "D" && chkDrg.Checked == false)
                                {
                                    if (strBun != "74" && strF != "Y" && str100 != "Y")
                                    {
                                        nAmt1 = 0;
                                        strBaseAmt = "0";
                                    }
                                    else
                                    {
                                        strBaseAmt = VB.Val(dt2.Rows[k]["BaseAmt1"].ToString().Trim()).ToString("##,###,##0");
                                    }

                                    //날수가 0인것 금액 0
                                    if (strNal == "0")
                                    {
                                        nAmt1 = 0;
                                        nAmt2 = 0;
                                    }
                                }
                                else
                                {
                                    strBaseAmt = VB.Val(dt2.Rows[k]["BaseAmt1"].ToString().Trim()).ToString("##,###,##0");
                                }

                                nStot1 += nAmt1;
                                nStot2 += nAmt2;
                                nGtot1 += nAmt1;
                                nGtot2 += nAmt2;


                                strAMT1 = nAmt1.ToString("###,###,##0");
                                strAMT2 = nAmt2.ToString("###,###,##0");

                                ssView_Sheet1.Cells[nRow - 1, 0].Text = strNujuk;
                                ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[k]["Sucode"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[k]["Bcode"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[k]["SunameK"].ToString().Trim();

                                //상급병실 종류표시
                                if (dt2.Rows[k]["BUN"].ToString().Trim() == "77")
                                {
                                    ssView_Sheet1.Cells[nRow - 1, 3].Text +=
                                        "(" + cisc.Chk_Senior_Ward(clsDB.DbCon, clsPmpaType.TIT.Ipdno, clsPmpaType.TIT.Trsno, (long)VB.Val(dt2.Rows[k]["BaseAmt1"].ToString().Trim())) + ")";
                                }

                                ssView_Sheet1.Cells[nRow - 1, 4].Text = strBaseAmt;
                                ssView_Sheet1.Cells[nRow - 1, 5].Text = strQty;
                                ssView_Sheet1.Cells[nRow - 1, 6].Text = strNal;
                                ssView_Sheet1.Cells[nRow - 1, 7].Text = strAMT1;
                                ssView_Sheet1.Cells[nRow - 1, 8].Text = strAMT2;
                                ssView_Sheet1.Cells[nRow - 1, 10].Text = dt2.Rows[k]["GbSpc1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 11].Text = dt2.Rows[k]["GbNgt1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 12].Text = dt2.Rows[k]["GbGisul1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 13].Text = dt2.Rows[k]["GbSelf1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 14].Text = dt2.Rows[k]["Sugbp1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 20].Text = dt2.Rows[k]["BUN"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 21].Text = dt2.Rows[k]["Sugbs1"].ToString().Trim();

                                //선별급여색깔 표시
                                if (cf.READ_BAS_Sun_S(clsDB.DbCon, dt2.Rows[k]["SuNext1"].ToString().Trim()) == "6" || cf.READ_BAS_Sun_S(clsDB.DbCon, dt2.Rows[k]["SuNext1"].ToString().Trim()) == "7")
                                {
                                    ssView_Sheet1.Cells[nRow - 1, 21].BackColor = Color.FromArgb(169, 208, 142);
                                }
                                else
                                {
                                    ssView_Sheet1.Cells[nRow - 1, 21].BackColor = Color.FromArgb(255, 255, 255);
                                }
                                ssView_Sheet1.Cells[nRow - 1, 22].Text = dt2.Rows[k]["Nu1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 23].Text = dt2.Rows[k]["SuNext1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 27].Text = dt2.Rows[k]["GbChild1"].ToString().Trim();

                                //P항 표시관련
                                if (nAmt1 != 0 && string.Compare(dt2.Rows[k]["Nu1"].ToString().Trim(), "21") >= 0)
                                {
                                    if ((dt2.Rows[k]["SugbP1"].ToString().Trim() == "0" || dt2.Rows[k]["SugbP1"].ToString().Trim() == "2" ||
                                       dt2.Rows[k]["SugbP1"].ToString().Trim() == "9") && dt2.Rows[k]["SugbS1"].ToString().Trim() != "1")
                                    {
                                        ssView_Sheet1.Cells[nRow - 1, 27].BackColor = Color.FromArgb(255, 0, 0);
                                    }
                                }

                                //마취코드는 변경 안 됨
                                ssView_Sheet1.Cells[nRow - 1, 16, nRow - 1, 17].Locked = true;

                                #endregion
                            }

                            dt2.Dispose();
                            dt2 = null;
                        }

                        #endregion
                    }
                    else
                    {
                        #region DATA_MOVE_SLIP1

                        nRow += 1;
                        if (ssView_Sheet1.RowCount < nRow)
                        {
                            ssView_Sheet1.RowCount = nRow;
                        }

                        nAmt1 = (long)VB.Val(dt.Rows[i]["Amtt1"].ToString().Trim());
                        nAmt2 = (long)VB.Val(dt.Rows[i]["Amtt2"].ToString().Trim());

                        strQty = VB.Val(dt.Rows[i]["Qty"].ToString().Trim()).ToString("##0.00");
                        strNal = VB.Val(dt.Rows[i]["Nalsu"].ToString().Trim()).ToString("##0");

                        if (FstrDrg == "D" && chkDrg.Checked == false)
                        {
                            if (strBun != "74" && strF != "Y" && str100 != "Y")
                            {
                                nAmt1 = 0;
                                strBaseAmt = "0";
                            }
                            else
                            {
                                strBaseAmt = VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("##,###,##0");
                            }

                            //날수가 0인것 금액 0
                            if (strNal == "0")
                            {
                                nAmt1 = 0;
                                nAmt2 = 0;
                            }
                        }
                        else
                        {
                            strBaseAmt = VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("##,###,##0");
                        }

                        nStot1 += nAmt1;
                        nStot2 += nAmt2;
                        nGtot1 += nAmt1;
                        nGtot2 += nAmt2;


                        strAMT1 = nAmt1.ToString("###,###,##0");
                        strAMT2 = nAmt2.ToString("###,###,##0");

                        ssView_Sheet1.Cells[nRow - 1, 0].Text = strNujuk;
                        ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["Sucode"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["Bcode"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["SunameK"].ToString().Trim();

                        //상급병실 종류표시
                        if (dt.Rows[i]["BUN"].ToString().Trim() == "77")
                        {
                            ssView_Sheet1.Cells[nRow - 1, 3].Text +=
                                "(" + cisc.Chk_Senior_Ward(clsDB.DbCon, clsPmpaType.TIT.Ipdno, clsPmpaType.TIT.Trsno, (long)VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim())) + ")";
                        }

                        ssView_Sheet1.Cells[nRow - 1, 4].Text = strBaseAmt;
                        ssView_Sheet1.Cells[nRow - 1, 5].Text = strQty;
                        ssView_Sheet1.Cells[nRow - 1, 6].Text = strNal;
                        ssView_Sheet1.Cells[nRow - 1, 7].Text = strAMT1;
                        ssView_Sheet1.Cells[nRow - 1, 8].Text = strAMT2;
                        ssView_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["GbSpc"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 11].Text = dt.Rows[i]["GbNgt"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 12].Text = dt.Rows[i]["GbGisul"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 13].Text = dt.Rows[i]["GbSelf"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 14].Text = dt.Rows[i]["Sugbp"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 20].Text = dt.Rows[i]["BUN"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 21].Text = dt.Rows[i]["Sugbs"].ToString().Trim();

                        //선별급여색깔 표시
                        if (cf.READ_BAS_Sun_S(clsDB.DbCon, dt.Rows[i]["SuNext1"].ToString().Trim()) == "6" || cf.READ_BAS_Sun_S(clsDB.DbCon, dt.Rows[i]["SuNext1"].ToString().Trim()) == "7")
                        {
                            ssView_Sheet1.Cells[nRow - 1, 21].BackColor = Color.FromArgb(169, 208, 142);
                        }
                        else
                        {
                            ssView_Sheet1.Cells[nRow - 1, 21].BackColor = Color.FromArgb(255, 255, 255);
                        }
                        ssView_Sheet1.Cells[nRow - 1, 22].Text = dt.Rows[i]["Nu"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 23].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 27].Text = dt.Rows[i]["GbChild"].ToString().Trim();

                        //P항 표시관련
                        if (nAmt1 != 0 && string.Compare(dt.Rows[i]["Nu"].ToString().Trim(), "21") >= 0)
                        {
                            if ((dt.Rows[i]["SugbP"].ToString().Trim() == "0" || dt.Rows[i]["SugbP"].ToString().Trim() == "2" ||
                                 dt.Rows[i]["SugbP"].ToString().Trim() == "9") && dt.Rows[i]["SugbS"].ToString().Trim() != "1")
                            {
                                ssView_Sheet1.Cells[nRow - 1, 27].BackColor = Color.FromArgb(255, 0, 0);
                            }
                        }

                        #endregion
                    }

                    strNujuk = "";
                }

                dt.Dispose();
                dt = null;

                if (FstrDrg == "D" && chkDrg.Checked == false)
                {
                    if (nGETcount > 0)
                    {
                        SUB_TOT_SLIP1(ref nRow, ref nStot1, ref nStot2, ISBR);
                    }

                    Display_Drg_Amt(ref nRow, ref nAmt1, ref nAmt2, ref nStot1, ref nStot2, ref nGtot1, ref nGtot2, ref strBaseAmt, ref strQty, ref strNal, ISBR);
                }

                if (nGETcount > 0)
                {
                    SUB_TOT_SLIP1(ref nRow, ref nStot1, ref nStot2, ISBR);

                    GRAND_TOT_SLIP1(ref nRow, ref nGtot1, ref nGtot2, ISBR);

                    btnPrint.Enabled = true;
                    ssView.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void Read_IPD_SLIP1(string strGbMACH = "")
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nNu = 0;
            int nNuChk = 0;
            int nRow = 0;
            int nRead = 0;

            long nAmt1 = 0;
            long nAmt2 = 0;
            long nStot1 = 0;
            long nStot2 = 0;
            long nGtot1 = 0;
            long nGtot2 = 0;

            string strSuNext = "";
            string strNujuk = "";
            string strBaseAmt = "";
            string strQty = "";
            string strNal = "";
            string strAMT1 = "";
            string strAMT2 = "";
            string strBun = "";
            string str100 = "";
            string strF = "";
            
            long nBAMT = 0;
            int nBonRate = 0;

            string strSuGbP = string.Empty;
            string strGbSuGbS = string.Empty;
            string strGbSelf = string.Empty;

            clsIuSentChk cisc = new clsIuSentChk();
            ComFunc cf = new ComFunc();
            clsPmpaType.ISBR ISBR = new clsPmpaType.ISBR();
            clsIument cIUM = new clsIument();

            ssView_Sheet1.ColumnHeader.Cells[0, 1].Text = "발생일자";
            ssView_Sheet1.ColumnHeader.Cells[0, 2].Text = "표준코드";

            for (i = 0; i < 2; i++)
            {
                ISBR.nBBAmt[i] = 0; ISBR.nBGAmt[i] = 0; ISBR.nBJAmt[i] = 0; ISBR.nBFAmt[i] = 0;
            }

            nNuChk = 0;
            nStot1 = 0;
            nStot2 = 0;
            nGtot1 = 0;
            nGtot2 = 0;
            nRow = 0;

            try
            {
                #region SQL Query
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT I.Sucode, I.Sunext, SunameK,";
                SQL = SQL + ComNum.VBLF + "        B.BCode, BaseAmt, Qty,";
                SQL = SQL + ComNum.VBLF + "        I.GBSugbs, b.Sugbp, GbSpc,";
                SQL = SQL + ComNum.VBLF + "        GbNgt, GbGisul, GbSelf,";
                SQL = SQL + ComNum.VBLF + "        GbChild, Nu, Bun,";
                SQL = SQL + ComNum.VBLF + "        DRG100, DRGF, ";
                if (chkHU.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "        SUM(Nal * -1) Nalsu, SUM(Amt1 * -1) Amtt1, SUM(Amt2 * -1) Amtt2, ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "        SUM(Nal) Nalsu, SUM(Amt1) Amtt1, SUM(Amt2) Amtt2, ";
                }
                SQL = SQL + ComNum.VBLF + "        to_char(min(i.bdate),'mm-dd') sdate, to_char(max(i.bdate),'mm-dd') tdate  ";
                //SQL = SQL + ComNum.VBLF + "        FC_ACCOUNT_BON_AMT(i.pano,i.bi,i.SuNext,SUM(Amt1) ,Qty,bun,nu,to_char(min(i.bdate),'yyyy-mm-dd'),'I','**',gbsugbs,trsno,GbSelf) BON ";

                if (clsPmpaType.IMST.GbOldSlip == false)
                {
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP I, " + ComNum.DB_PMPA + "BAS_SUN B";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_OLD_SLIP I, " + ComNum.DB_PMPA + "BAS_SUN B";
                }

                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                if (chkHU.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND TRIM(PART) = '!-' ";
                }

                if (chkAll.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                }

                SQL = SQL + ComNum.VBLF + "    AND Nu >= '" + strFnu + "'";
                SQL = SQL + ComNum.VBLF + "    AND Nu <= '" + strTnu + "'";
                //*------------( 96/01/31 기간 Check 추가 lyj)--------------------------*

                if (strSDate != "")
                {
                    SQL = SQL + ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strSDate + "','yyyy-mm-dd')   ";
                }

                if (strEdate != "")
                {
                    SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strEdate + "','yyyy-mm-dd')   ";
                }

                //*-------------------------------------------------------------------------*
                SQL = SQL + ComNum.VBLF + "    AND (b.WONCODE NOT IN('1118') OR b.WONCODE IS NULL )"; //간호행위제외
                SQL = SQL + ComNum.VBLF + "    AND I.Sunext = B.Sunext";

                //저가약제 제외코드 2011-04-09
                SQL = SQL + ComNum.VBLF + "   AND TRIM(i.SUNEXT) NOT IN ( SELECT CODE FROM " + ComNum.DB_PMPA + "BAS_BCODE WHERE GUBUN ='원무영수제외코드') ";

                SQL = SQL + ComNum.VBLF + "  GROUP BY I.Sucode, I.Sunext, SunameK, B.Bcode, BaseAmt, Qty,I.GBSugbs, b.Sugbp, i.pano,i.bi, ";
                SQL = SQL + ComNum.VBLF + "           GbSpc, GbNgt, GbGisul, GbSelf, GbChild, Nu, Bun,DRG100,DRGF,trsno  ";

                SQL = SQL + ComNum.VBLF + "  ORDER BY  Nu, Bun, I.Sucode"; 
                #endregion

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                nRead = dt.Rows.Count;

                nGETcount += nRead;

                for (i = 0; i < nRead; i++)
                {
                    strBun      = dt.Rows[i]["Bun"].ToString().Trim();
                    str100      = dt.Rows[i]["DRG100"].ToString().Trim();
                    strF        = dt.Rows[i]["DRGF"].ToString().Trim();
                    strSuNext   = dt.Rows[i]["SUNEXT"].ToString().Trim();
                    strSuGbP    = dt.Rows[i]["SUGBP"].ToString().Trim();
                    strGbSuGbS  = dt.Rows[i]["GBSUGBS"].ToString().Trim();
                    strGbSelf   = dt.Rows[i]["GBSELF"].ToString().Trim();

                    if (DRG.READ_DRG_100(clsDB.DbCon, dt.Rows[i]["Sucode"].ToString().Trim(), "NEW", clsPmpaType.TIT.InDate) == "OK")
                    {
                        str100 = "Y";
                    }
                    else
                    {
                        str100 = "";
                    }
                    if (VB.Val(dt.Rows[i]["Nu"].ToString().Trim()) != 0)
                    {
                        nNu = (int)VB.Val(dt.Rows[i]["Nu"].ToString().Trim());
                    }

                    if (nNuChk == 0)
                    {
                        nNuChk = nNu;
                        strNujuk = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_누적행위명", nNu.ToString("00"));
                    }

                    if (nNu != nNuChk)
                    {
                        SUB_TOT_SLIP1(ref nRow, ref nStot1, ref nStot2, ISBR);

                        nNuChk = nNu;
                        strNujuk = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_누적행위명", nNu.ToString("00"));
                    }

                    //심사계 요청
                    #region DATA_MOVE_SLIP1

                    nRow += 1;
                    if (ssView_Sheet1.RowCount < nRow)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    nAmt1 = (long)VB.Val(dt.Rows[i]["Amtt1"].ToString().Trim());
                    nAmt2 = (long)VB.Val(dt.Rows[i]["Amtt2"].ToString().Trim());

                    strQty = VB.Val(dt.Rows[i]["Qty"].ToString().Trim()).ToString("##0.00");
                    strNal = VB.Val(dt.Rows[i]["Nalsu"].ToString().Trim()).ToString("##0");

                    if (FstrDrg == "D" && chkDrg.Checked == false)
                    {
                        if (strBun != "74" && strF != "Y" && str100 != "Y")
                        {
                            nAmt1 = 0;
                            strBaseAmt = "0";
                        }
                        else
                        {
                            strBaseAmt = VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("##,###,##0");
                        }

                        //날수가 0인것 금액 0
                        if (strNal == "0")
                        {
                            nAmt1 = 0;
                            nAmt2 = 0;
                        }
                    }
                    else
                    {
                        strBaseAmt = VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("##,###,##0");
                    }

                    nStot1 += nAmt1;
                    nStot2 += nAmt2;
                    nGtot1 += nAmt1;
                    nGtot2 += nAmt2;
                    
                    strAMT1 = nAmt1.ToString("###,###,##0");
                    strAMT2 = nAmt2.ToString("###,###,##0");

                    ssView_Sheet1.Cells[nRow - 1, 0].Text = strNujuk;
                    //ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["Sucode"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["sdate"].ToString().Trim() + "~" + dt.Rows[i]["Tdate"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["Bcode"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["SunameK"].ToString().Trim();

                    //상급병실 종류표시
                    if (dt.Rows[i]["BUN"].ToString().Trim() == "77")
                    {
                        ssView_Sheet1.Cells[nRow - 1, 3].Text +=
                            "(" + cisc.Chk_Senior_Ward(clsDB.DbCon, clsPmpaType.TIT.Ipdno, clsPmpaType.TIT.Trsno, (long)VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim())) + ")";
                    }

                    ssView_Sheet1.Cells[nRow - 1, 4].Text = strBaseAmt;
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = strQty;
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = strNal;
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = strAMT1;
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = strAMT2;
                    ssView_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["GbSpc"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 11].Text = dt.Rows[i]["GbNgt"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 12].Text = dt.Rows[i]["GbGisul"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 13].Text = dt.Rows[i]["GbSelf"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 14].Text = dt.Rows[i]["Sugbp"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 20].Text = dt.Rows[i]["BUN"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 21].Text = dt.Rows[i]["GBSugbs"].ToString().Trim();

                    //선별급여색깔 표시
                    if (cf.READ_BAS_Sun_S(clsDB.DbCon, dt.Rows[i]["SuNext"].ToString().Trim()) == "6" || cf.READ_BAS_Sun_S(clsDB.DbCon, dt.Rows[i]["SuNext"].ToString().Trim()) == "7")
                    {
                        ssView_Sheet1.Cells[nRow - 1, 21].BackColor = Color.FromArgb(169, 208, 142);
                    }
                    else
                    {
                        ssView_Sheet1.Cells[nRow - 1, 21].BackColor = Color.FromArgb(255, 255, 255);
                    }
                    ssView_Sheet1.Cells[nRow - 1, 22].Text = dt.Rows[i]["Nu"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 23].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 27].Text = dt.Rows[i]["GbChild"].ToString().Trim();

                    //P항 표시관련
                    if (nAmt1 != 0 && string.Compare(dt.Rows[i]["Nu"].ToString().Trim(), "21") >= 0)
                    {
                        if ((dt.Rows[i]["SugbP"].ToString().Trim() == "0" || dt.Rows[i]["SugbP"].ToString().Trim() == "2" ||
                             dt.Rows[i]["SugbP"].ToString().Trim() == "9") && dt.Rows[i]["GBSugbs"].ToString().Trim() != "1")
                        {
                            ssView_Sheet1.Cells[nRow - 1, 27].BackColor = Color.FromArgb(255, 0, 0);
                        }
                    }

                    //수가별 본인부담금 표시
                    clsIuSentChk cISCHK = new clsIuSentChk();

                    ISBR.BI         = clsPmpaType.TIT.Bi;
                    ISBR.INDATE     = clsPmpaType.TIT.M_InDate;
                    ISBR.VCODE      = clsPmpaType.TIT.VCode;
                    ISBR.FCODE      = clsPmpaType.TIT.FCode;
                    ISBR.OGPDBUN    = clsPmpaType.TIT.OgPdBun;
                    ISBR.BOHUN      = clsPmpaType.TIT.Bohun;
                    ISBR.GBDRG      = clsPmpaType.TIT.GbDRG;
                    ISBR.DTLBUN     = cISCHK.Read_Bas_Sun_ColName(clsDB.DbCon, strSuNext, "DTLBUN", dt.Rows[i]["Sucode"].ToString().Trim());  //2018-10-29 
                    ISBR.QTY        = VB.Val(strQty);
                    ISBR.NAL        = (int)VB.Val(strNal);
                    ISBR.DRGF       = strF;
                    ISBR.DRG100     = str100;
                    ISBR.SUGBP      = strSuGbP;
                    ISBR.GBSUGBS    = strGbSuGbS;
                    ISBR.AMT        = nAmt1;
                    ISBR.GBSELF     = strGbSelf;
                    ISBR.BUN        = strBun;
                    ISBR.NU         = nNu;
                    ISBR.SUNEXT     = strSuNext;

                    if (chkGbSelf.Checked == true)
                    {
                        cIUM.Ipd_Slip_Bon_Rate(clsDB.DbCon, ISBR, "OK");
                    }
                    else
                    {
                        cIUM.Ipd_Slip_Bon_Rate(clsDB.DbCon, ISBR);
                    }
                    ssView_Sheet1.Cells[nRow - 1, 28].Text = ISBR.nBBAmt[0].ToString("###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 29].Text = ISBR.nBGAmt[0].ToString("###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 30].Text = ISBR.nBJAmt[0].ToString("###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 31].Text = ISBR.nBFAmt[0].ToString("###,##0");

                    #endregion

                    #region AS-IS에서 주석처리 되어있음
                    //if (nNu == 6)
                    //{
                    //    //마취 합산분 풀어서 Display

                    //    if (strGbMACH == "8")
                    //    {
                    //        #region DATA_MARCHI_MOVE_SLIP8

                    //        if (nInitNo != 66)
                    //        {
                    //            return;
                    //        }

                    //        nInitNo = 88;

                    //        SQL = "";
                    //        SQL = SQL + ComNum.VBLF + " SELECT Sucode Sucode1, I.Sunext Sunext1, SunameK SunameK1,      ";
                    //        SQL = SQL + ComNum.VBLF + "        Hcode Hcode1, BaseAmt BaseAmt1, Qty Qty1, I.GBSugbs Sugbs1,b.Sugbp Sugbp1,";
                    //        SQL = SQL + ComNum.VBLF + "        GbSpc GbSpc1, GbNgt GbNgt1, GbGisul GbGisul1,            ";
                    //        SQL = SQL + ComNum.VBLF + "        GbSelf GbSelf1, GbChild GbChild1, Nu Nu1,                ";
                    //        SQL = SQL + ComNum.VBLF + "        Nal Nalsu1, Amt1 Amtt11, Amt2 Amtt21,                    ";
                    //        SQL = SQL + ComNum.VBLF + "        BUN                                                      ";
                    //        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP I,                     ";
                    //        SQL = SQL + ComNum.VBLF + "       " + ComNum.DB_PMPA + "BAS_SUN B                           ";
                    //        SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                                                    ";

                    //        if (chkAll.Checked == true)
                    //        {
                    //            SQL = SQL + ComNum.VBLF + "  AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                    //        }
                    //        else
                    //        {
                    //            SQL = SQL + ComNum.VBLF + "  AND TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                    //        }

                    //        SQL = SQL + ComNum.VBLF + "    AND Nu = '06'                                ";
                    //        SQL = SQL + ComNum.VBLF + "    AND I.Sunext = B.Sunext                      ";
                    //        //*------------( 96/01/31 기간 Check 추가 lyj)--------------------------*
                    //        if (strSDate != "")
                    //        {
                    //            SQL = SQL + ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strSDate + "','yyyy-mm-dd')    ";
                    //        }
                    //        if (strEdate != "")
                    //        {
                    //            SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strEdate + "','yyyy-mm-dd')    ";
                    //        }
                    //        SQL = SQL + ComNum.VBLF + " GROUP BY Sucode , I.Sunext , SunameK , Bcode , BaseAmt , Qty ,I.GBSugbs ,";
                    //        SQL = SQL + ComNum.VBLF + "          b.Sugbp ,   GbSpc , GbNgt , GbGisul , GbSelf, GbChild , Nu ,  BUN ";
                    //        SQL = SQL + ComNum.VBLF + "  HAVING SUM(NAL) <> 0";
                    //        SQL = SQL + ComNum.VBLF + "  ORDER BY Nu, Sucode, I.Sunext";

                    //        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                    //        if (SqlErr != "")
                    //        {
                    //            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    //            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    //            return;
                    //        }

                    //        if (dt2.Rows.Count == 0)
                    //        {
                    //            dt2.Dispose();
                    //            dt2 = null;
                    //        }
                    //        else
                    //        {
                    //            nCNT2 = dt2.Rows.Count;

                    //            for (k = 0; k < nCNT2; k++)
                    //            {
                    //                #region DATA_MOVE_SLIP1_2

                    //                nRow += 1;
                    //                if (ssView_Sheet1.RowCount < nRow)
                    //                {
                    //                    ssView_Sheet1.RowCount = nRow;
                    //                }

                    //                if (k > 0)
                    //                {
                    //                    strNujuk = "          ";
                    //                }

                    //                nAmt1 = (long)VB.Val(dt2.Rows[k]["Amtt11"].ToString().Trim());
                    //                nAmt2 = (long)VB.Val(dt2.Rows[k]["Amtt21"].ToString().Trim());

                    //                strQty = VB.Val(dt2.Rows[k]["Qty1"].ToString().Trim()).ToString("##0.00");
                    //                strNal = VB.Val(dt2.Rows[k]["Nalsu1"].ToString().Trim()).ToString("##0");

                    //                if (FstrDrg == "D" && chkDrg.Checked == false)
                    //                {
                    //                    if (strBun != "74" && strF != "Y" && str100 != "Y")
                    //                    {
                    //                        nAmt1 = 0;
                    //                        strBaseAmt = "0";
                    //                    }
                    //                    else
                    //                    {
                    //                        strBaseAmt = VB.Val(dt2.Rows[k]["BaseAmt1"].ToString().Trim()).ToString("##,###,##0");
                    //                    }

                    //                    //날수가 0인것 금액 0
                    //                    if (strNal == "0")
                    //                    {
                    //                        nAmt1 = 0;
                    //                        nAmt2 = 0;
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    strBaseAmt = VB.Val(dt2.Rows[k]["BaseAmt1"].ToString().Trim()).ToString("##,###,##0");
                    //                }

                    //                nStot1 += nAmt1;
                    //                nStot2 += nAmt2;
                    //                nGtot1 += nAmt1;
                    //                nGtot2 += nAmt2;


                    //                strAMT1 = nAmt1.ToString("###,###,##0");
                    //                strAMT2 = nAmt2.ToString("###,###,##0");

                    //                ssView_Sheet1.Cells[nRow - 1, 0].Text = strNujuk;
                    //                ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[k]["Sucode"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[k]["Bcode"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[k]["SunameK"].ToString().Trim();

                    //                //상급병실 종류표시
                    //                if (dt2.Rows[k]["BUN"].ToString().Trim() == "77")
                    //                {
                    //                    ssView_Sheet1.Cells[nRow - 1, 3].Text +=
                    //                        "(" + cisc.Chk_Senior_Ward(clsDB.DbCon, clsPmpaType.TIT.Ipdno, clsPmpaType.TIT.Trsno, (long)VB.Val(dt2.Rows[k]["BaseAmt"].ToString().Trim())) + ")";
                    //                }

                    //                ssView_Sheet1.Cells[nRow - 1, 4].Text = strBaseAmt;
                    //                ssView_Sheet1.Cells[nRow - 1, 5].Text = strQty;
                    //                ssView_Sheet1.Cells[nRow - 1, 6].Text = strNal;
                    //                ssView_Sheet1.Cells[nRow - 1, 7].Text = strAMT1;
                    //                ssView_Sheet1.Cells[nRow - 1, 8].Text = strAMT2;
                    //                ssView_Sheet1.Cells[nRow - 1, 10].Text = dt2.Rows[k]["GbSpc1"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 11].Text = dt2.Rows[k]["GbNgt1"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 12].Text = dt2.Rows[k]["GbGisul1"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 13].Text = dt2.Rows[k]["GbSelf1"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 14].Text = dt2.Rows[k]["Sugbp1"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 20].Text = dt2.Rows[k]["BUN"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 21].Text = dt2.Rows[k]["Sugbs1"].ToString().Trim();

                    //                //선별급여색깔 표시
                    //                if (cf.READ_BAS_Sun_S(clsDB.DbCon, dt2.Rows[k]["SuNext1"].ToString().Trim()) == "6" || cf.READ_BAS_Sun_S(clsDB.DbCon, dt2.Rows[k]["SuNext1"].ToString().Trim()) == "7")
                    //                {
                    //                    ssView_Sheet1.Cells[nRow - 1, 21].BackColor = Color.FromArgb(169, 208, 142);
                    //                }
                    //                else
                    //                {
                    //                    ssView_Sheet1.Cells[nRow - 1, 21].BackColor = Color.FromArgb(255, 255, 255);
                    //                }
                    //                ssView_Sheet1.Cells[nRow - 1, 22].Text = dt2.Rows[k]["Nu1"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 23].Text = dt2.Rows[k]["SuNext1"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 27].Text = dt2.Rows[k]["GbChild1"].ToString().Trim();

                    //                //P항 표시관련
                    //                if (nAmt1 != 0 && string.Compare(dt2.Rows[k]["Nu1"].ToString().Trim(), "21") >= 0)
                    //                {
                    //                    if ((dt2.Rows[k]["SugbP1"].ToString().Trim() == "0" || dt2.Rows[k]["SugbP1"].ToString().Trim() == "2" ||
                    //                       dt2.Rows[k]["SugbP1"].ToString().Trim() == "9") && dt2.Rows[k]["SugbS1"].ToString().Trim() != "1")
                    //                    {
                    //                        ssView_Sheet1.Cells[nRow - 1, 27].BackColor = Color.FromArgb(255, 0, 0);
                    //                    }
                    //                }

                    //                nTAMT = nAmt1;

                    //                if (dt2.Rows[k]["Sugbs1"].ToString().Trim() == "6" || dt2.Rows[k]["Sugbs1"].ToString().Trim() == "7" || dt2.Rows[k]["Sugbs1"].ToString().Trim() == "8")
                    //                {
                    //                    nBonRate = Bon_Rate_100(dt2.Rows[k]["Sugbs1"].ToString().Trim());
                    //                }
                    //                else
                    //                {
                    //                    nBonRate = 0;
                    //                }

                    //                //전액본인부담
                    //                if (nBonRate > 0)
                    //                {
                    //                    nBAMT = (long)Math.Truncate(nAmt1 * (nBonRate / 100.0));
                    //                    ssView_Sheet1.Cells[nRow - 1, 30].Text = nBAMT.ToString("###,##0");
                    //                }
                    //                else
                    //                {
                    //                    //비급여
                    //                    if (dt2.Rows[k]["GbSelf1"].ToString().Trim() != "0")
                    //                    {
                    //                        nBonRate = Bon_Rate_100(dt2.Rows[k]["Sugbs1"].ToString().Trim());

                    //                        if (nBonRate > 0)
                    //                        {
                    //                            nBAMT = (long)Math.Truncate(nAmt1 * (nBonRate / 100.0));
                    //                        }
                    //                        else
                    //                        {
                    //                            nBAMT = nAmt1;
                    //                        }

                    //                        ssView_Sheet1.Cells[nRow - 1, 31].Text = nBAMT.ToString("###,##0");
                    //                    }
                    //                    else
                    //                    {
                    //                        //본인부담
                    //                        if (dt2.Rows[k]["BUN"].ToString().Trim() == "01" || dt2.Rows[k]["BUN"].ToString().Trim() == "02")
                    //                        {
                    //                            nBAMT = (long)Math.Truncate(nAmt1 * (clsPmpaType.IBR.Jin / 100.0));
                    //                        }
                    //                        else if (dt2.Rows[k]["BUN"].ToString().Trim() == "72" || dt2.Rows[k]["BUN"].ToString().Trim() == "73")
                    //                        {
                    //                            nBAMT = (long)Math.Truncate(nAmt1 * (clsPmpaType.IBR.CTMRI / 100.0));
                    //                        }
                    //                        else if (dt2.Rows[k]["BUN"].ToString().Trim() == "74")
                    //                        {
                    //                            nBAMT = (long)Math.Truncate(nAmt1 * (clsPmpaType.IBR.Food / 100.0));
                    //                        }
                    //                        else
                    //                        {
                    //                            nBAMT = (long)Math.Truncate(nAmt1 * (clsPmpaType.IBR.Bohum / 100.0));
                    //                        }

                    //                        ssView_Sheet1.Cells[nRow - 1, 28].Text = nBAMT.ToString("###,##0");

                    //                    }

                    //                }

                    //                ssView_Sheet1.Cells[nRow - 1, 29].Text = (nTAMT - nBAMT).ToString("###,##0");   //공단부담

                    //                //마취코드는 변경 안 됨
                    //                ssView_Sheet1.Cells[nRow - 1, 16, nRow - 1, 17].Locked = true;

                    //                #endregion
                    //            }

                    //            dt2.Dispose();
                    //            dt2 = null;
                    //        }

                    //        #endregion
                    //    }
                    //    else
                    //    {
                    //        #region DATA_MARCHI_MOVE_SLIP1

                    //        if (nInitNo != 66)
                    //        {
                    //            return;
                    //        }

                    //        nInitNo = 88;

                    //        SQL = "";
                    //        SQL = SQL + ComNum.VBLF + " SELECT Sucode Sucode1, I.Sunext Sunext1, SunameK SunameK1,  ";
                    //        SQL = SQL + ComNum.VBLF + "        Hcode Hcode1, BaseAmt BaseAmt1, Qty Qty1,            ";
                    //        SQL = SQL + ComNum.VBLF + "        I.GBSugbs Sugbs1,b.Sugbp Sugbp1,                     ";
                    //        SQL = SQL + ComNum.VBLF + "        GbSpc GbSpc1, GbNgt GbNgt1, GbGisul GbGisul1,        ";
                    //        SQL = SQL + ComNum.VBLF + "        GbSelf GbSelf1, GbChild GbChild1, Nu Nu1,            ";
                    //        SQL = SQL + ComNum.VBLF + "        Nal Nalsu1, Amt1 Amtt11, Amt2 Amtt21,                ";
                    //        SQL = SQL + ComNum.VBLF + "        BUN                                                  ";
                    //        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP I,                 ";
                    //        SQL = SQL + ComNum.VBLF + "       " + ComNum.DB_PMPA + "BAS_SUN B                       ";
                    //        SQL = SQL + ComNum.VBLF + " WHERE 1 = 1                                                 ";

                    //        if (chkAll.Checked == true)
                    //        {
                    //            SQL = SQL + ComNum.VBLF + "  AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                    //        }
                    //        else
                    //        {
                    //            SQL = SQL + ComNum.VBLF + "  AND TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                    //        }

                    //        SQL = SQL + ComNum.VBLF + "    AND Nu = '06'";
                    //        SQL = SQL + ComNum.VBLF + "    AND I.Sunext = B.Sunext";
                    //        //*------------( 96/01/31 기간 Check 추가 lyj)--------------------------*
                    //        if (strSDate != "")
                    //        {
                    //            SQL = SQL + ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strSDate + "','yyyy-mm-dd')";
                    //        }
                    //        if (strEdate != "")
                    //        {
                    //            SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strEdate + "','yyyy-mm-dd')";
                    //        }
                    //        SQL = SQL + ComNum.VBLF + "  ORDER BY Nu, Sucode, I.Sunext";

                    //        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                    //        if (SqlErr != "")
                    //        {
                    //            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    //            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    //            return;
                    //        }

                    //        if (dt2.Rows.Count == 0)
                    //        {
                    //            dt2.Dispose();
                    //            dt2 = null;
                    //        }
                    //        else
                    //        {
                    //            nCNT2 = dt2.Rows.Count;

                    //            for (k = 0; k < nCNT2; k++)
                    //            {
                    //                #region DATA_MOVE_SLIP1_2

                    //                nRow += 1;
                    //                if (ssView_Sheet1.RowCount < nRow)
                    //                {
                    //                    ssView_Sheet1.RowCount = nRow;
                    //                }

                    //                if (k > 0)
                    //                {
                    //                    strNujuk = "          ";
                    //                }

                    //                nAmt1 = (long)VB.Val(dt2.Rows[k]["Amtt11"].ToString().Trim());
                    //                nAmt2 = (long)VB.Val(dt2.Rows[k]["Amtt21"].ToString().Trim());

                    //                strQty = VB.Val(dt2.Rows[k]["Qty1"].ToString().Trim()).ToString("##0.00");
                    //                strNal = VB.Val(dt2.Rows[k]["Nalsu1"].ToString().Trim()).ToString("##0");

                    //                if (FstrDrg == "D" && chkDrg.Checked == false)
                    //                {
                    //                    if (strBun != "74" && strF != "Y" && str100 != "Y")
                    //                    {
                    //                        nAmt1 = 0;
                    //                        strBaseAmt = "0";
                    //                    }
                    //                    else
                    //                    {
                    //                        strBaseAmt = VB.Val(dt2.Rows[k]["BaseAmt1"].ToString().Trim()).ToString("##,###,##0");
                    //                    }

                    //                    //날수가 0인것 금액 0
                    //                    if (strNal == "0")
                    //                    {
                    //                        nAmt1 = 0;
                    //                        nAmt2 = 0;
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    strBaseAmt = VB.Val(dt2.Rows[k]["BaseAmt1"].ToString().Trim()).ToString("##,###,##0");
                    //                }

                    //                nStot1 += nAmt1;
                    //                nStot2 += nAmt2;
                    //                nGtot1 += nAmt1;
                    //                nGtot2 += nAmt2;

                    //                strAMT1 = nAmt1.ToString("###,###,##0");
                    //                strAMT2 = nAmt2.ToString("###,###,##0");

                    //                ssView_Sheet1.Cells[nRow - 1, 0].Text = strNujuk;
                    //                ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[k]["Sucode"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[k]["Bcode"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[k]["SunameK"].ToString().Trim();

                    //                //상급병실 종류표시
                    //                if (dt2.Rows[k]["BUN"].ToString().Trim() == "77")
                    //                {
                    //                    ssView_Sheet1.Cells[nRow - 1, 3].Text +=
                    //                        "(" + cisc.Chk_Senior_Ward(clsDB.DbCon, clsPmpaType.TIT.Ipdno, clsPmpaType.TIT.Trsno, (long)VB.Val(dt2.Rows[k]["BaseAmt"].ToString().Trim())) + ")";
                    //                }

                    //                ssView_Sheet1.Cells[nRow - 1, 4].Text = strBaseAmt;
                    //                ssView_Sheet1.Cells[nRow - 1, 5].Text = strQty;
                    //                ssView_Sheet1.Cells[nRow - 1, 6].Text = strNal;
                    //                ssView_Sheet1.Cells[nRow - 1, 7].Text = strAMT1;
                    //                ssView_Sheet1.Cells[nRow - 1, 8].Text = strAMT2;
                    //                ssView_Sheet1.Cells[nRow - 1, 10].Text = dt2.Rows[k]["GbSpc1"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 11].Text = dt2.Rows[k]["GbNgt1"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 12].Text = dt2.Rows[k]["GbGisul1"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 13].Text = dt2.Rows[k]["GbSelf1"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 14].Text = dt2.Rows[k]["Sugbp1"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 20].Text = dt2.Rows[k]["BUN"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 21].Text = dt2.Rows[k]["Sugbs1"].ToString().Trim();

                    //                //선별급여색깔 표시
                    //                if (cf.READ_BAS_Sun_S(clsDB.DbCon, dt2.Rows[k]["SuNext1"].ToString().Trim()) == "6" || cf.READ_BAS_Sun_S(clsDB.DbCon, dt2.Rows[k]["SuNext1"].ToString().Trim()) == "7")
                    //                {
                    //                    ssView_Sheet1.Cells[nRow - 1, 21].BackColor = Color.FromArgb(169, 208, 142);
                    //                }
                    //                else
                    //                {
                    //                    ssView_Sheet1.Cells[nRow - 1, 21].BackColor = Color.FromArgb(255, 255, 255);
                    //                }
                    //                ssView_Sheet1.Cells[nRow - 1, 22].Text = dt2.Rows[k]["Nu1"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 23].Text = dt2.Rows[k]["SuNext1"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 27].Text = dt2.Rows[k]["GbChild1"].ToString().Trim();

                    //                //P항 표시관련
                    //                if (nAmt1 != 0 && string.Compare(dt2.Rows[k]["Nu1"].ToString().Trim(), "21") >= 0)
                    //                {
                    //                    if ((dt2.Rows[k]["SugbP1"].ToString().Trim() == "0" || dt2.Rows[k]["SugbP1"].ToString().Trim() == "2" ||
                    //                       dt2.Rows[k]["SugbP1"].ToString().Trim() == "9") && dt2.Rows[k]["SugbS1"].ToString().Trim() != "1")
                    //                    {
                    //                        ssView_Sheet1.Cells[nRow - 1, 27].BackColor = Color.FromArgb(255, 0, 0);
                    //                    }
                    //                }

                    //                nTAMT = nAmt1;

                    //                if (dt2.Rows[k]["Sugbs1"].ToString().Trim() == "6" || dt2.Rows[k]["Sugbs1"].ToString().Trim() == "7" || dt2.Rows[k]["Sugbs1"].ToString().Trim() == "8")
                    //                {
                    //                    nBonRate = Bon_Rate_100(dt2.Rows[k]["Sugbs1"].ToString().Trim());
                    //                }
                    //                else
                    //                {
                    //                    nBonRate = 0;
                    //                }

                    //                //전액본인부담
                    //                if (nBonRate > 0)
                    //                {
                    //                    nBAMT = (long)Math.Truncate(nAmt1 * (nBonRate / 100.0));
                    //                    ssView_Sheet1.Cells[nRow - 1, 30].Text = nBAMT.ToString("###,##0");
                    //                }
                    //                else
                    //                {
                    //                    //비급여
                    //                    if (dt2.Rows[k]["GbSelf1"].ToString().Trim() != "0")
                    //                    {
                    //                        nBonRate = Bon_Rate_100(dt2.Rows[k]["Sugbs1"].ToString().Trim());

                    //                        if (nBonRate > 0)
                    //                        {
                    //                            nBAMT = (long)Math.Truncate(nAmt1 * (nBonRate / 100.0));
                    //                        }
                    //                        else
                    //                        {
                    //                            nBAMT = nAmt1;
                    //                        }

                    //                        ssView_Sheet1.Cells[nRow - 1, 31].Text = nBAMT.ToString("###,##0");
                    //                    }
                    //                    else
                    //                    {
                    //                        //본인부담
                    //                        if (dt2.Rows[k]["BUN"].ToString().Trim() == "01" || dt2.Rows[k]["BUN"].ToString().Trim() == "02")
                    //                        {
                    //                            nBAMT = (long)Math.Truncate(nAmt1 * (clsPmpaType.IBR.Jin / 100.0));
                    //                        }
                    //                        else if (dt2.Rows[k]["BUN"].ToString().Trim() == "72" || dt2.Rows[k]["BUN"].ToString().Trim() == "73")
                    //                        {
                    //                            nBAMT = (long)Math.Truncate(nAmt1 * (clsPmpaType.IBR.CTMRI / 100.0));
                    //                        }
                    //                        else if (dt2.Rows[k]["BUN"].ToString().Trim() == "74")
                    //                        {
                    //                            nBAMT = (long)Math.Truncate(nAmt1 * (clsPmpaType.IBR.Food / 100.0));
                    //                        }
                    //                        else
                    //                        {
                    //                            nBAMT = (long)Math.Truncate(nAmt1 * (clsPmpaType.IBR.Bohum / 100.0));
                    //                        }

                    //                        ssView_Sheet1.Cells[nRow - 1, 28].Text = nBAMT.ToString("###,##0");

                    //                    }

                    //                }

                    //                ssView_Sheet1.Cells[nRow - 1, 29].Text = (nTAMT - nBAMT).ToString("###,##0");   //공단부담


                    //                //마취코드는 변경 안 됨
                    //                ssView_Sheet1.Cells[nRow - 1, 16, nRow - 1, 17].Locked = true;

                    //                #endregion
                    //            }

                    //            dt2.Dispose();
                    //            dt2 = null;
                    //        }

                    //        #endregion
                    //    }

                    //}
                    //else
                    //{
                    //    #region DATA_MOVE_SLIP1

                    //    nRow += 1;
                    //    if (ssView_Sheet1.RowCount < nRow)
                    //    {
                    //        ssView_Sheet1.RowCount = nRow;
                    //    }

                    //    nAmt1 = (long)VB.Val(dt.Rows[i]["Amtt1"].ToString().Trim());
                    //    nAmt2 = (long)VB.Val(dt.Rows[i]["Amtt2"].ToString().Trim());

                    //    strQty = VB.Val(dt.Rows[i]["Qty"].ToString().Trim()).ToString("##0.00");
                    //    strNal = VB.Val(dt.Rows[i]["Nalsu"].ToString().Trim()).ToString("##0");

                    //    if (FstrDrg == "D" && chkDrg.Checked == false)
                    //    {
                    //        if (strBun != "74" && strF != "Y" && str100 != "Y")
                    //        {
                    //            nAmt1 = 0;
                    //            strBaseAmt = "0";
                    //        }
                    //        else
                    //        {
                    //            strBaseAmt = VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("##,###,##0");
                    //        }

                    //        //날수가 0인것 금액 0
                    //        if (strNal == "0")
                    //        {
                    //            nAmt1 = 0;
                    //            nAmt2 = 0;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        strBaseAmt = VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("##,###,##0");
                    //    }

                    //    nStot1 += nAmt1;
                    //    nStot2 += nAmt2;
                    //    nGtot1 += nAmt1;
                    //    nGtot2 += nAmt2;


                    //    strAMT1 = nAmt1.ToString("###,###,##0");
                    //    strAMT2 = nAmt2.ToString("###,###,##0");

                    //    ssView_Sheet1.Cells[nRow - 1, 0].Text = strNujuk;
                    //    ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["Sucode"].ToString().Trim();
                    //    ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["Bcode"].ToString().Trim();
                    //    ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["SunameK"].ToString().Trim();

                    //    //상급병실 종류표시
                    //    if (dt.Rows[i]["BUN"].ToString().Trim() == "77")
                    //    {
                    //        ssView_Sheet1.Cells[nRow - 1, 3].Text +=
                    //            "(" + cisc.Chk_Senior_Ward(clsDB.DbCon, clsPmpaType.TIT.Ipdno, clsPmpaType.TIT.Trsno, (long)VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim())) + ")";
                    //    }

                    //    ssView_Sheet1.Cells[nRow - 1, 4].Text = strBaseAmt;
                    //    ssView_Sheet1.Cells[nRow - 1, 5].Text = strQty;
                    //    ssView_Sheet1.Cells[nRow - 1, 6].Text = strNal;
                    //    ssView_Sheet1.Cells[nRow - 1, 7].Text = strAMT1;
                    //    ssView_Sheet1.Cells[nRow - 1, 8].Text = strAMT2;
                    //    ssView_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["GbSpc"].ToString().Trim();
                    //    ssView_Sheet1.Cells[nRow - 1, 11].Text = dt.Rows[i]["GbNgt"].ToString().Trim();
                    //    ssView_Sheet1.Cells[nRow - 1, 12].Text = dt.Rows[i]["GbGisul"].ToString().Trim();
                    //    ssView_Sheet1.Cells[nRow - 1, 13].Text = dt.Rows[i]["GbSelf"].ToString().Trim();
                    //    ssView_Sheet1.Cells[nRow - 1, 14].Text = dt.Rows[i]["Sugbp"].ToString().Trim();
                    //    ssView_Sheet1.Cells[nRow - 1, 20].Text = dt.Rows[i]["BUN"].ToString().Trim();
                    //    ssView_Sheet1.Cells[nRow - 1, 21].Text = dt.Rows[i]["GBSugbs"].ToString().Trim();

                    //    //선별급여색깔 표시
                    //    if (cf.READ_BAS_Sun_S(clsDB.DbCon, dt.Rows[i]["SuNext"].ToString().Trim()) == "6" || cf.READ_BAS_Sun_S(clsDB.DbCon, dt.Rows[i]["SuNext"].ToString().Trim()) == "7")
                    //    {
                    //        ssView_Sheet1.Cells[nRow - 1, 21].BackColor = Color.FromArgb(169, 208, 142);
                    //    }
                    //    else
                    //    {
                    //        ssView_Sheet1.Cells[nRow - 1, 21].BackColor = Color.FromArgb(255, 255, 255);
                    //    }
                    //    ssView_Sheet1.Cells[nRow - 1, 22].Text = dt.Rows[i]["Nu"].ToString().Trim();
                    //    ssView_Sheet1.Cells[nRow - 1, 23].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                    //    ssView_Sheet1.Cells[nRow - 1, 27].Text = dt.Rows[i]["GbChild"].ToString().Trim();

                    //    //P항 표시관련
                    //    if (nAmt1 != 0 && string.Compare(dt.Rows[i]["Nu"].ToString().Trim(), "21") >= 0)
                    //    {
                    //        if ((dt.Rows[i]["SugbP"].ToString().Trim() == "0" || dt.Rows[i]["SugbP"].ToString().Trim() == "2" ||
                    //             dt.Rows[i]["SugbP"].ToString().Trim() == "9") && dt.Rows[i]["GBSugbs"].ToString().Trim() != "1")
                    //        {
                    //            ssView_Sheet1.Cells[nRow - 1, 27].BackColor = Color.FromArgb(255, 0, 0);
                    //        }
                    //    }

                    //    nTAMT = nAmt1;

                    //    if (dt.Rows[i]["GBSugbs"].ToString().Trim() == "6" || dt.Rows[i]["GBSugbs"].ToString().Trim() == "7" || dt.Rows[i]["GBSugbs"].ToString().Trim() == "8")
                    //    {
                    //        nBonRate = Bon_Rate_100(dt.Rows[i]["GBSugbs"].ToString().Trim());
                    //    }
                    //    else
                    //    {
                    //        nBonRate = 0;
                    //    }

                    //    //전액본인부담
                    //    if (nBonRate > 0)
                    //    {
                    //        nBAMT = (long)Math.Truncate(nAmt1 * (nBonRate / 100.0));
                    //        ssView_Sheet1.Cells[nRow - 1, 30].Text = nBAMT.ToString("###,##0");
                    //    }
                    //    else
                    //    {
                    //        //비급여
                    //        if (dt.Rows[i]["GbSelf"].ToString().Trim() != "0")
                    //        {
                    //            nBonRate = Bon_Rate_100(dt.Rows[i]["GBSugbs"].ToString().Trim());

                    //            if (nBonRate > 0)
                    //            {
                    //                nBAMT = (long)Math.Truncate(nAmt1 * (nBonRate / 100.0));
                    //            }
                    //            else
                    //            {
                    //                nBAMT = nAmt1;
                    //            }

                    //            ssView_Sheet1.Cells[nRow - 1, 31].Text = nBAMT.ToString("###,##0");
                    //        }
                    //        else
                    //        {
                    //            //본인부담
                    //            if (dt.Rows[i]["BUN"].ToString().Trim() == "01" || dt.Rows[i]["BUN"].ToString().Trim() == "02")
                    //            {
                    //                nBAMT = (long)Math.Truncate(nAmt1 * (clsPmpaType.IBR.Jin / 100.0));
                    //            }
                    //            else if (dt.Rows[i]["BUN"].ToString().Trim() == "72" || dt.Rows[i]["BUN"].ToString().Trim() == "73")
                    //            {
                    //                nBAMT = (long)Math.Truncate(nAmt1 * (clsPmpaType.IBR.CTMRI / 100.0));
                    //            }
                    //            else if (dt.Rows[i]["BUN"].ToString().Trim() == "74")
                    //            {
                    //                nBAMT = (long)Math.Truncate(nAmt1 * (clsPmpaType.IBR.Food / 100.0));
                    //            }
                    //            else
                    //            {
                    //                nBAMT = (long)Math.Truncate(nAmt1 * (clsPmpaType.IBR.Bohum / 100.0));
                    //            }

                    //            ssView_Sheet1.Cells[nRow - 1, 28].Text = nBAMT.ToString("###,##0");

                    //        }

                    //    }

                    //    ssView_Sheet1.Cells[nRow - 1, 29].Text = (nTAMT - nBAMT).ToString("###,##0");   //공단부담
                    //    #endregion
                    //} 
                    #endregion

                    strNujuk = "";
                }

                dt.Dispose();
                dt = null;

                if (FstrDrg == "D" && chkDrg.Checked == false)
                {
                    if (nGETcount > 0)
                    {
                        SUB_TOT_SLIP1(ref nRow, ref nStot1, ref nStot2, ISBR);
                    }

                    Display_Drg_Amt(ref nRow, ref nAmt1, ref nAmt2, ref nStot1, ref nStot2, ref nGtot1, ref nGtot2, ref strBaseAmt, ref strQty, ref strNal, ISBR);
                }

                if (nGETcount > 0)
                {
                    SUB_TOT_SLIP1(ref nRow, ref nStot1, ref nStot2, ISBR);

                    GRAND_TOT_SLIP1(ref nRow, ref nGtot1, ref nGtot2, ISBR);

                    btnPrint.Enabled = true;
                    ssView.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void Read_IPD_SLIP3_1()
        {
            int i = 0;
            DataTable dt = null;
            DataTable dt2 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nNu = 0;
            int nNuChk = 0;
            int k = 0;
            int nRow = 0;
            int nCNT2 = 0;
            int nInitNo = 0;
            int nRead = 0;
            long nAmt1 = 0;
            long nAmt2 = 0;
            long nStot1 = 0;
            long nStot2 = 0;
            long nGtot1 = 0;
            long nGtot2 = 0;
            string strNujuk = "";
            string strBaseAmt = "";
            string strQty = "";
            string strNal = "";
            string strAMT1 = "";
            string strAMT2 = "";
            string strBun = "";
            string str100 = "";
            string strF = "";

            ComFunc cf = new ComFunc();
            clsPmpaType.ISBR ISBR = new clsPmpaType.ISBR();
            clsIument cIUM = new clsIument();

            ssView_Sheet1.ColumnHeader.Cells[0, 1].Text = "발생일자";
            ssView_Sheet1.ColumnHeader.Cells[0, 2].Text = "표준코드";

            nInitNo = 66;

            nNuChk = 0;
            nStot1 = 0;
            nStot2 = 0;
            nGtot1 = 0;
            nGtot2 = 0;
            nRow = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(I.BDATE,'yy-mm-dd') BDATE, DRG100, DRGF,";
                SQL = SQL + ComNum.VBLF + "        Sucode, SunameK, B.BCode,";
                SQL = SQL + ComNum.VBLF + "        BaseAmt, Qty, I.GBSugbs,";
                SQL = SQL + ComNum.VBLF + "        GbSpc, GbNgt, GbGisul,";
                SQL = SQL + ComNum.VBLF + "        GbSelf, GbChild, SUM(Nal) Nalsu,";
                SQL = SQL + ComNum.VBLF + "        SUM(Amt1) Amtt1, SUM(Amt2) Amtt2,";
                SQL = SQL + ComNum.VBLF + "        CASE WHEN BUN IN ('01','02') THEN '01'                     ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('03','05','07','09') THEN '02'           ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('04','06','08','10') THEN '03'           ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('11','12','13','14','15') THEN '04'      ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('16','17','18','19','20','21') THEN '05' ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('22','23') THEN '06'                     ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('24','25') THEN '07'                     ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('26','27') THEN '08'                     ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('28','29','30','31','32','33') THEN '09' ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('34','35','36') THEN '10'                ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('37') THEN '11'                          ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('38','39') THEN '12'                     ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('40') THEN '40'                          ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN >= '41' AND BUN <= '51' THEN '13'            ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN >= '52' AND BUN <= '64' THEN '14'            ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN >= '65' AND BUN <= '70' THEN '15'            ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('71') THEN '36'                          ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('72') THEN  '37'                         ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('73') THEN  '38'                         ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('74') THEN  '34'                         ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('75') THEN  '47'                         ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('76') THEN  '46'                         ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('78') THEN  '43'                         ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('79') THEN  '41'                         ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('80') THEN  '42'                         ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('81') THEN  '44'                         ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('82') THEN  '61'                         ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('83') THEN  '62'                         ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('84') THEN  '63'                         ";
                SQL = SQL + ComNum.VBLF + "             ELSE '20' END BUN                                     ";

                if (clsPmpaType.IMST.GbOldSlip == false)
                {
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP I, " + ComNum.DB_PMPA + "BAS_SUN B";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_OLD_SLIP I, " + ComNum.DB_PMPA + "BAS_SUN B";
                }

                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";

                if (chkAll.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                }

                SQL = SQL + ComNum.VBLF + "    AND Nu >= '" + strFnu + "'";
                SQL = SQL + ComNum.VBLF + "    AND Nu <= '" + strTnu + "'";
                //*------------( 96/01/31 기간 Check 추가 lyj)--------------------------*

                if (strSDate != "")
                {
                    SQL = SQL + ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strSDate + "','yyyy-mm-dd')   ";
                }

                if (strEdate != "")
                {
                    SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strEdate + "','yyyy-mm-dd')   ";
                }

                //*-------------------------------------------------------------------------*
                SQL = SQL + ComNum.VBLF + "    AND (b.WONCODE NOT IN('1118') OR b.WONCODE IS NULL )"; //간호행위제외
                SQL = SQL + ComNum.VBLF + "    AND I.Sunext = B.Sunext";

                //저가약제 제외코드 2011-04-09
                SQL = SQL + ComNum.VBLF + "   AND TRIM(i.SUNEXT) NOT IN ( SELECT CODE FROM ADMIN.BAS_BCODE WHERE GUBUN ='원무영수제외코드') ";

                SQL = SQL + ComNum.VBLF + "  GROUP BY Sucode, SunameK, B.Bcode, BaseAmt, Qty, I.GBSugbs,";
                SQL = SQL + ComNum.VBLF + "           GbSpc, GbNgt, GbGisul, GbSelf, GbChild,";
                SQL = SQL + ComNum.VBLF + "           TO_CHAR(I.BDATE,'yy-mm-dd'), DRG100,DRGF,               ";
                SQL = SQL + ComNum.VBLF + "        CASE WHEN BUN IN ('01','02') THEN '01'                     ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('03','05','07','09') THEN '02'           ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('04','06','08','10') THEN '03'           ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('11','12','13','14','15') THEN '04'      ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('16','17','18','19','20','21') THEN '05' ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('22','23') THEN '06'                     ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('24','25') THEN '07'                     ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('26','27') THEN '08'                     ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('28','29','30','31','32','33') THEN '09' ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('34','35','36') THEN '10'                ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('37') THEN '11'                          ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('38','39') THEN '12'                     ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('40') THEN '40'                          ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN >= '41' AND BUN <= '51' THEN '13'            ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN >= '52' AND BUN <= '64' THEN '14'            ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN >= '65' AND BUN <= '70' THEN '15'            ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('71') THEN '36'                          ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('72') THEN  '37'                         ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('73') THEN  '38'                         ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('74') THEN  '34'                         ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('75') THEN  '47'                         ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('76') THEN  '46'                         ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('78') THEN  '43'                         ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('79') THEN  '41'                         ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('80') THEN  '42'                         ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('81') THEN  '44'                         ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('82') THEN  '61'                         ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('83') THEN  '62'                         ";
                SQL = SQL + ComNum.VBLF + "             WHEN BUN IN ('84') THEN  '63'                         ";
                SQL = SQL + ComNum.VBLF + "             ELSE '20' END                                         ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY Bun, BDATE, Sucode";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                nRead = dt.Rows.Count;

                nGETcount += nRead;

                for (i = 0; i < nRead; i++)
                {
                    strBun = dt.Rows[i]["Bun"].ToString().Trim();
                    str100 = dt.Rows[i]["DRG100"].ToString().Trim();
                    strF = dt.Rows[i]["DRGF"].ToString().Trim();

                    if (DRG.READ_DRG_100(clsDB.DbCon, dt.Rows[i]["Sucode"].ToString().Trim(), "", clsPmpaType.TIT.InDate) == "OK")
                    {
                        str100 = "Y";
                    }
                    else
                    {
                        str100 = "";
                    }
                    nNu = (int)VB.Val(dt.Rows[i]["BUN"].ToString().Trim());

                    if (nNuChk == 0)
                    {
                        nNuChk = nNu;
                        strNujuk = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_누적행위명", nNu.ToString("00"));
                    }

                    if (nNu != nNuChk)
                    {
                        SUB_TOT_SLIP1(ref nRow, ref nStot1, ref nStot2, ISBR);

                        nNuChk = nNu;
                        strNujuk = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_누적행위명", nNu.ToString("00")); 
                    }

                    //심사계 요청
                    if (nNu == 6)
                    {
                        //마취 합산분 풀어서 Display
                        #region DATA_MARCHI_MOVE_SLIP1

                        if (nInitNo != 66)
                        {
                            continue;
                        }

                        nInitNo = 88;

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT Sucode Sucode1, I.Sunext Sunext1, SunameK SunameK1,";
                        SQL = SQL + ComNum.VBLF + "        B.Bcode Bcode1, BaseAmt BaseAmt1, Qty Qty1,";
                        SQL = SQL + ComNum.VBLF + "        I.GBSugbs Sugbs1, GbSpc GbSpc1, GbNgt GbNgt1,";
                        SQL = SQL + ComNum.VBLF + "        GbGisul GbGisul1, GbSelf GbSelf1, GbChild GbChild1,";
                        SQL = SQL + ComNum.VBLF + "        Nu Nu1, Nal Nalsu1, Amt1 Amtt11,";
                        SQL = SQL + ComNum.VBLF + "        Amt2 Amtt21, TO_CHAR(BDATE,'YY-MM-DD') BDATE";
                        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP I,     ";
                        SQL = SQL + ComNum.VBLF + "       " + ComNum.DB_PMPA + "BAS_SUN B           ";
                        SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";

                        if (chkAll.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "  AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "  AND TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                        }

                        SQL = SQL + ComNum.VBLF + "    AND Nu IN ('06','24')";
                        SQL = SQL + ComNum.VBLF + "    AND I.Sunext = B.Sunext";
                        //*------------( 96/01/31 기간 Check 추가 lyj)--------------------------*
                        if (strSDate != "")
                        {
                            SQL = SQL + ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strSDate + "','yyyy-mm-dd')";
                        }
                        if (strEdate != "")
                        {
                            SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strEdate + "','yyyy-mm-dd')";
                        }
                        SQL = SQL + ComNum.VBLF + "  ORDER BY Nu, Sucode, I.Sunext";

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt2.Rows.Count == 0)
                        {
                            dt2.Dispose();
                            dt2 = null;
                        }
                        else
                        {
                            nCNT2 = dt2.Rows.Count;

                            for (k = 0; k < nCNT2; k++)
                            {
                                #region DATA_MOVE_SLIP1_2

                                nRow += 1;
                                if (ssView_Sheet1.RowCount < nRow)
                                {
                                    ssView_Sheet1.RowCount = nRow;
                                }

                                if (k > 0)
                                {
                                    strNujuk = "          ";
                                }

                                nAmt1 = (long)VB.Val(dt2.Rows[k]["Amtt11"].ToString().Trim());
                                nAmt2 = (long)VB.Val(dt2.Rows[k]["Amtt21"].ToString().Trim());

                                if (FstrDrg == "D" && chkDrg.Checked == false)
                                {
                                    if (strBun != "74" && strF != "Y" && str100 != "Y")
                                    {
                                        nAmt1 = 0;
                                        strBaseAmt = "0";
                                    }
                                    else
                                    {
                                        strBaseAmt = VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("##,###,##0");
                                    }

                                    if (nAmt1 == 0 && strBaseAmt != "0")
                                    {
                                        nAmt1 = (long)VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()) + (long)VB.Val(dt.Rows[i]["Qty"].ToString().Trim());
                                    }

                                }
                                else
                                {
                                    strBaseAmt = VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("##,###,##0");
                                }

                                nStot1 += nAmt1;
                                nStot2 += nAmt2;
                                nGtot1 += nAmt1;
                                nGtot2 += nAmt2;

                                strQty = VB.Val(dt2.Rows[k]["Qty1"].ToString().Trim()).ToString("##0.00");
                                strNal = VB.Val(dt2.Rows[k]["Nalsu1"].ToString().Trim()).ToString("##0");
                                strAMT1 = nAmt1.ToString("###,###,##0");
                                strAMT2 = nAmt2.ToString("###,###,##0");

                                ssView_Sheet1.Cells[nRow - 1, 0].Text = strNujuk;
                                ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[k]["BDATE"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[k]["Sucode"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[k]["SunameK"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 4].Text = strBaseAmt;
                                ssView_Sheet1.Cells[nRow - 1, 5].Text = strQty;
                                ssView_Sheet1.Cells[nRow - 1, 6].Text = strNal;
                                ssView_Sheet1.Cells[nRow - 1, 7].Text = strAMT1;
                                ssView_Sheet1.Cells[nRow - 1, 8].Text = strAMT2;
                                ssView_Sheet1.Cells[nRow - 1, 10].Text = dt2.Rows[k]["GbSpc1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 11].Text = dt2.Rows[k]["GbNgt1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 12].Text = dt2.Rows[k]["GbGisul1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 13].Text = dt2.Rows[k]["GbSelf1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 27].Text = dt2.Rows[k]["GbChild1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 21].Text = dt2.Rows[k]["Sugbs1"].ToString().Trim();

                                //선별급여색깔 표시
                                if (cf.READ_BAS_Sun_S(clsDB.DbCon, dt2.Rows[k]["SuNext1"].ToString().Trim()) == "6" || cf.READ_BAS_Sun_S(clsDB.DbCon, dt2.Rows[k]["SuNext1"].ToString().Trim()) == "7")
                                {
                                    ssView_Sheet1.Cells[nRow - 1, 21].BackColor = Color.FromArgb(169, 208, 142);
                                }
                                else
                                {
                                    ssView_Sheet1.Cells[nRow - 1, 21].BackColor = Color.FromArgb(255, 255, 255);
                                }

                                #endregion
                            }

                            dt2.Dispose();
                            dt2 = null;
                        }

                        #endregion
                    }
                    else
                    {
                        #region DATA_MOVE_SLIP1

                        nRow += 1;
                        if (ssView_Sheet1.RowCount < nRow)
                        {
                            ssView_Sheet1.RowCount = nRow;
                        }

                        nAmt1 = (long)VB.Val(dt.Rows[i]["Amtt1"].ToString().Trim());
                        nAmt2 = (long)VB.Val(dt.Rows[i]["Amtt2"].ToString().Trim()); 

                        if (FstrDrg == "D" && chkDrg.Checked == false)
                        {
                            if (strBun != "74" && strF != "Y" && str100 != "Y")
                            {
                                nAmt1 = 0;
                                strBaseAmt = "0";
                            }
                            else
                            {
                                strBaseAmt = VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("##,###,##0");
                            }

                        }
                        else
                        {
                            strBaseAmt = VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("##,###,##0");
                        }

                        nStot1 += nAmt1;
                        nStot2 += nAmt2;
                        nGtot1 += nAmt1;
                        nGtot2 += nAmt2;

                        strQty = VB.Val(dt.Rows[i]["Qty"].ToString().Trim()).ToString("##0.00");
                        strNal = VB.Val(dt.Rows[i]["Nalsu"].ToString().Trim()).ToString("##0");
                        strAMT1 = nAmt1.ToString("###,###,##0");
                        strAMT2 = nAmt2.ToString("###,###,##0");

                        ssView_Sheet1.Cells[nRow - 1, 0].Text = strNujuk;
                        ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["Sucode"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["SunameK"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 4].Text = strBaseAmt;
                        ssView_Sheet1.Cells[nRow - 1, 5].Text = strQty;
                        ssView_Sheet1.Cells[nRow - 1, 6].Text = strNal;
                        ssView_Sheet1.Cells[nRow - 1, 7].Text = strAMT1;
                        ssView_Sheet1.Cells[nRow - 1, 8].Text = strAMT2;
                        ssView_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["GbSpc"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 11].Text = dt.Rows[i]["GbNgt"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 12].Text = dt.Rows[i]["GbGisul"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 13].Text = dt.Rows[i]["GbSelf"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 27].Text = dt.Rows[i]["GbChild"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 21].Text = dt.Rows[i]["GbSugbs"].ToString().Trim();

                        //선별급여색깔 표시
                        if (cf.READ_BAS_Sun_S(clsDB.DbCon, dt.Rows[i]["Sucode"].ToString().Trim()) == "6" || cf.READ_BAS_Sun_S(clsDB.DbCon, dt.Rows[i]["Sucode"].ToString().Trim()) == "7")
                        {
                            ssView_Sheet1.Cells[nRow - 1, 21].BackColor = Color.FromArgb(169, 208, 142);
                        }
                        else
                        {
                            ssView_Sheet1.Cells[nRow - 1, 21].BackColor = Color.FromArgb(255, 255, 255);
                        }

                        #endregion
                    }

                    strNujuk = "";
                }

                dt.Dispose();
                dt = null;

                if (FstrDrg == "D" && chkDrg.Checked == false)
                {
                    if (nGETcount > 0)
                    {
                        SUB_TOT_SLIP1(ref nRow, ref nStot1, ref nStot2, ISBR);
                    }

                    Display_Drg_Amt(ref nRow, ref nAmt1, ref nAmt2, ref nStot1, ref nStot2, ref nGtot1, ref nGtot2, ref strBaseAmt, ref strQty, ref strNal, ISBR);
                }

                if (nGETcount > 0)
                {
                    SUB_TOT_SLIP1(ref nRow, ref nStot1, ref nStot2, ISBR);

                    GRAND_TOT_SLIP1(ref nRow, ref nGtot1, ref nGtot2, ISBR);

                    btnPrint.Enabled = true;
                    ssView.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void Read_IPD_SLIP4_1()
        {
            int i = 0;
            DataTable dt = null;
            DataTable dt2 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nNu = 0;
            int nNuChk = 0;
            int k = 0;
            int nRow = 0;
            int nCNT2 = 0;
            int nInitNo = 0;
            int nRead = 0;
            long nAmt1 = 0;
            long nAmt2 = 0;
            long nStot1 = 0;
            long nStot2 = 0;
            long nGtot1 = 0;
            long nGtot2 = 0;
            string strNujuk = "";
            string strBaseAmt = "";
            string strQty = "";
            string strNal = "";
            string strAMT1 = "";
            string strAMT2 = "";
            string strBun = "";
            string str100 = "";
            string strF = "";

            ComFunc cf = new ComFunc();
            clsPmpaType.ISBR ISBR = new clsPmpaType.ISBR();
            clsIument cIUM = new clsIument();

            ssView_Sheet1.ColumnHeader.Cells[0, 1].Text = "수가코드";
            ssView_Sheet1.ColumnHeader.Cells[0, 2].Text = "표준코드";

            nInitNo = 66;

            nNuChk = 0;
            nStot1 = 0;
            nStot2 = 0;
            nGtot1 = 0;
            nGtot2 = 0;
            nRow = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Sucode, I.Sunext, SunameK,";
                SQL = SQL + ComNum.VBLF + "        B.BCode, BaseAmt, Qty,";
                SQL = SQL + ComNum.VBLF + "        I.GBSugbs, b.Sugbp, GbSpc,";
                SQL = SQL + ComNum.VBLF + "        GbNgt, GbGisul, GbSelf,";
                SQL = SQL + ComNum.VBLF + "        GbChild, Bun, SUM(Nal) Nalsu,";
                SQL = SQL + ComNum.VBLF + "        SUM(Amt1) Amtt1, SUM(Amt2) Amtt2, DRG100,";
                SQL = SQL + ComNum.VBLF + "        DRGF";

                if (clsPmpaType.IMST.GbOldSlip == false)
                {
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP I, " + ComNum.DB_PMPA + "BAS_SUN B";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_OLD_SLIP I, " + ComNum.DB_PMPA + "BAS_SUN B";
                }

                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";

                if (chkAll.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                }

                SQL = SQL + ComNum.VBLF + "    AND Nu >= '" + strFnu + "'";
                SQL = SQL + ComNum.VBLF + "    AND Nu <= '" + strTnu + "'";
                //*------------( 96/01/31 기간 Check 추가 lyj)--------------------------*

                if (strSDate != "")
                {
                    SQL = SQL + ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strSDate + "','yyyy-mm-dd')   ";
                }

                if (strEdate != "")
                {
                    SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strEdate + "','yyyy-mm-dd')   ";
                }

                //*-------------------------------------------------------------------------*
                SQL = SQL + ComNum.VBLF + "    AND (b.WONCODE NOT IN('1118') OR b.WONCODE IS NULL )"; //간호행위제외
                SQL = SQL + ComNum.VBLF + "    AND I.Sunext = B.Sunext";

                //저가약제 제외코드 2011-04-09
                SQL = SQL + ComNum.VBLF + "   AND TRIM(i.SUNEXT) NOT IN ( SELECT CODE FROM ADMIN.BAS_BCODE WHERE GUBUN ='원무영수제외코드') ";

                SQL = SQL + ComNum.VBLF + "  GROUP BY Sucode, I.Sunext, SunameK, B.Bcode, BaseAmt, Qty,I.GBSugbs,b.Sugbp,  ";
                SQL = SQL + ComNum.VBLF + "           GbSpc, GbNgt, GbGisul, GbSelf, GbChild, Bun,DRG100,DRGF  ";

                SQL = SQL + ComNum.VBLF + "  ORDER BY Bun, Sucode";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                nRead = dt.Rows.Count;

                nGETcount += nRead;

                for (i = 0; i < nRead; i++)
                {
                    strBun = dt.Rows[i]["Bun"].ToString().Trim();
                    str100 = dt.Rows[i]["DRG100"].ToString().Trim();
                    strF = dt.Rows[i]["DRGF"].ToString().Trim();

                    if (DRG.READ_DRG_100(clsDB.DbCon, dt.Rows[i]["Sucode"].ToString().Trim(), "", clsPmpaType.TIT.InDate) == "OK")
                    {
                        str100 = "Y";
                    }
                    else
                    {
                        str100 = "";
                    }
                    switch (dt.Rows[i]["BUN"].ToString().Trim())
                    {
                        case "01":
                        case "02":
                            nNu = 1;           //진찰료
                            break;
                        case "03":
                        case "05":
                        case "07":
                        case "09":
                            nNu = 2; //입원실료
                            break;
                        case "04":
                        case "06":
                        case "08":
                        case "10":
                            nNu = 3; //환자관리
                            break;
                        case "11":
                        case "12":
                        case "13":
                        case "14":
                        case "15":
                            nNu = 4;
                            break;
                        case "16":
                        case "17":
                        case "18":
                        case "19":
                        case "20":
                        case "21":
                            nNu = 5;
                            break;
                        case "22":
                        case "23":
                            nNu = 6;
                            break;
                        case "24":
                        case "25":
                            nNu = 7;
                            break;
                        case "26":
                        case "27":
                            nNu = 8;
                            break;
                        case "28":
                        case "29":
                        case "30":
                        case "31":
                        case "32":
                        case "33":
                            nNu = 9;
                            break;
                        case "34":
                        case "35":
                        case "36":
                            nNu = 10;
                            break;
                        case "37":
                            nNu = 11;
                            break;
                        case "38":
                        case "39":
                            nNu = 12;
                            break;
                        case "40":
                            nNu = 40;
                            break;
                        case "41":
                        case "42":
                        case "43":
                        case "44":
                        case "45":
                        case "46":
                        case "47":
                        case "48":
                        case "49":
                        case "50":
                        case "51":
                            nNu = 13;
                            break;
                        case "52":
                        case "53":
                        case "54":
                        case "55":
                        case "56":
                        case "57":
                        case "58":
                        case "59":
                        case "60":
                        case "61":
                        case "62":
                        case "63":
                        case "64":
                            nNu = 14;
                            break;
                        case "65":
                        case "66":
                        case "67":
                        case "68":
                        case "69":
                        case "70":
                            nNu = 15;
                            break;
                        case "71":
                            nNu = 36;
                            break;
                        case "72":
                            nNu = 37;
                            break;
                        case "73":
                            nNu = 38;
                            break;
                        case "74":
                            nNu = 34;
                            break;
                        case "75":
                            nNu = 47;
                            break;
                        case "76":
                            nNu = 46;
                            break;
                        case "77":
                            nNu = 35;
                            break;
                        case "78":
                            nNu = 43;
                            break;
                        case "79":
                            nNu = 41;
                            break;
                        case "80":
                            nNu = 42;
                            break;
                        case "81":
                            nNu = 44;
                            break;
                        case "82":
                            nNu = 61;
                            break;
                        case "83":
                            nNu = 62;
                            break;
                        case "84":
                            nNu = 63;
                            break;
                        default:
                            nNu = 20;
                            break;
                    }

                    if (nNuChk == 0)
                    {
                        nNuChk = nNu;
                        strNujuk = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_누적행위명", nNu.ToString("00"));
                    }

                    if (nNu != nNuChk)
                    {
                        SUB_TOT_SLIP1(ref nRow, ref nStot1, ref nStot2, ISBR);

                        nNuChk = nNu;
                        strNujuk = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_누적행위명", nNu.ToString("00"));
                    }

                    //심사계 요청
                    if (nNu == 6)
                    {
                        //마취 합산분 풀어서 Display
                        #region DATA_MARCHI_MOVE_SLIP1

                        if (nInitNo != 66)
                        {
                            continue;
                        }

                        nInitNo = 88;

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT Sucode Sucode1, I.Sunext Sunext1, SunameK SunameK1,";
                        SQL = SQL + ComNum.VBLF + "        Hcode Hcode1, BaseAmt BaseAmt1, Qty Qty1,";
                        SQL = SQL + ComNum.VBLF + "        GbSpc GbSpc1, GbNgt GbNgt1, GbGisul GbGisul1,";
                        SQL = SQL + ComNum.VBLF + "        GbSelf GbSelf1, GbChild GbChild1, Nu Nu1,";
                        SQL = SQL + ComNum.VBLF + "        Nal Nalsu1, Amt1 Amtt11, Amt2 Amtt21,";
                        SQL = SQL + ComNum.VBLF + "        BUN,SUGBP Sugbp1, GBSUGBS Sugbs1";
                        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP I, ";
                        SQL = SQL + ComNum.VBLF + "       " + ComNum.DB_PMPA + "BAS_SUN B       ";
                        SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";

                        if (chkAll.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "  AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "  AND TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                        }

                        SQL = SQL + ComNum.VBLF + "    AND Nu   IN ('06','24')";
                        SQL = SQL + ComNum.VBLF + "    AND I.Sunext = B.Sunext";
                        //*------------( 96/01/31 기간 Check 추가 lyj)--------------------------*
                        if (strSDate != "")
                        {
                            SQL = SQL + ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strSDate + "','yyyy-mm-dd')";
                        }
                        if (strEdate != "")
                        {
                            SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strEdate + "','yyyy-mm-dd')";
                        }
                        SQL = SQL + ComNum.VBLF + "  ORDER BY Nu, Sucode, I.Sunext";

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt2.Rows.Count == 0)
                        {
                            dt2.Dispose();
                            dt2 = null;
                        }
                        else
                        {
                            nCNT2 = dt2.Rows.Count;

                            for (k = 0; k < nCNT2; k++)
                            {
                                #region DATA_MOVE_SLIP1_2

                                nRow += 1;
                                if (ssView_Sheet1.RowCount < nRow)
                                {
                                    ssView_Sheet1.RowCount = nRow;
                                }

                                if (k > 0)
                                {
                                    strNujuk = "          ";
                                }

                                nAmt1 = (long)VB.Val(dt2.Rows[k]["Amtt11"].ToString().Trim());
                                nAmt2 = (long)VB.Val(dt2.Rows[k]["Amtt21"].ToString().Trim());



                                if (FstrDrg == "D" && chkDrg.Checked == false)
                                {
                                    if (strBun != "74" && strF != "Y" && str100 != "Y")
                                    {
                                        nAmt1 = 0;
                                        strBaseAmt = "0";
                                    }
                                    else
                                    {
                                        strBaseAmt = VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("##,###,##0");
                                    }

                                }
                                else
                                {
                                    strBaseAmt = VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("##,###,##0");
                                }

                                nStot1 += nAmt1;
                                nStot2 += nAmt2;
                                nGtot1 += nAmt1;
                                nGtot2 += nAmt2;

                                strQty = VB.Val(dt2.Rows[k]["Qty1"].ToString().Trim()).ToString("##0.00");
                                strNal = VB.Val(dt2.Rows[k]["Nalsu1"].ToString().Trim()).ToString("##0");
                                strAMT1 = nAmt1.ToString("###,###,##0");
                                strAMT2 = nAmt2.ToString("###,###,##0");

                                ssView_Sheet1.Cells[nRow - 1, 0].Text = strNujuk;
                                ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[k]["Sucode"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[k]["Bcode"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[k]["SunameK"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 4].Text = strBaseAmt;
                                ssView_Sheet1.Cells[nRow - 1, 5].Text = strQty;
                                ssView_Sheet1.Cells[nRow - 1, 6].Text = strNal;
                                ssView_Sheet1.Cells[nRow - 1, 7].Text = strAMT1;
                                ssView_Sheet1.Cells[nRow - 1, 8].Text = strAMT2;
                                ssView_Sheet1.Cells[nRow - 1, 10].Text = dt2.Rows[k]["GbSpc1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 11].Text = dt2.Rows[k]["GbNgt1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 12].Text = dt2.Rows[k]["GbGisul1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 13].Text = dt2.Rows[k]["GbSelf1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 14].Text = dt2.Rows[k]["Sugbp1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 21].Text = dt2.Rows[k]["Sugbs1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 27].Text = dt2.Rows[k]["GbChild1"].ToString().Trim();

                                #endregion
                            }

                            dt2.Dispose();
                            dt2 = null;
                        }

                        #endregion
                    }
                    else
                    {
                        #region DATA_MOVE_SLIP1

                        nRow += 1;
                        if (ssView_Sheet1.RowCount < nRow)
                        {
                            ssView_Sheet1.RowCount = nRow;
                        }

                        nAmt1 = (long)VB.Val(dt.Rows[i]["Amtt1"].ToString().Trim());
                        nAmt2 = (long)VB.Val(dt.Rows[i]["Amtt2"].ToString().Trim());

                        strQty = VB.Val(dt.Rows[i]["Qty"].ToString().Trim()).ToString("##0.00");
                        strNal = VB.Val(dt.Rows[i]["Nalsu"].ToString().Trim()).ToString("##0");

                        if (FstrDrg == "D" && chkDrg.Checked == false)
                        {
                            if (strBun != "74" && strF != "Y" && str100 != "Y")
                            {
                                nAmt1 = 0;
                                strBaseAmt = "0";
                            }
                            else
                            {
                                strBaseAmt = VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("##,###,##0");
                            }

                            //날수가 0인것 금액 0
                            if (strNal == "0")
                            {
                                nAmt1 = 0;
                                nAmt2 = 0;
                            }
                        }
                        else
                        {
                            strBaseAmt = VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("##,###,##0");
                        }

                        nStot1 += nAmt1;
                        nStot2 += nAmt2;
                        nGtot1 += nAmt1;
                        nGtot2 += nAmt2;


                        strAMT1 = nAmt1.ToString("###,###,##0");
                        strAMT2 = nAmt2.ToString("###,###,##0");

                        ssView_Sheet1.Cells[nRow - 1, 0].Text = strNujuk;
                        ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["Sucode"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["Bcode"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["SunameK"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 4].Text = strBaseAmt;
                        ssView_Sheet1.Cells[nRow - 1, 5].Text = strQty;
                        ssView_Sheet1.Cells[nRow - 1, 6].Text = strNal;
                        ssView_Sheet1.Cells[nRow - 1, 7].Text = strAMT1;
                        ssView_Sheet1.Cells[nRow - 1, 8].Text = strAMT2;
                        ssView_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["GbSpc"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 11].Text = dt.Rows[i]["GbNgt"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 12].Text = dt.Rows[i]["GbGisul"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 13].Text = dt.Rows[i]["GbSelf"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 14].Text = dt.Rows[i]["Sugbp"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 21].Text = dt.Rows[i]["GBSugbs"].ToString().Trim();

                        //선별급여색깔 표시
                        if (cf.READ_BAS_Sun_S(clsDB.DbCon, dt.Rows[i]["SuNext"].ToString().Trim()) == "6" || cf.READ_BAS_Sun_S(clsDB.DbCon, dt.Rows[i]["SuNext"].ToString().Trim()) == "7")
                        {
                            ssView_Sheet1.Cells[nRow - 1, 21].BackColor = Color.FromArgb(169, 208, 142);
                        }
                        else
                        {
                            ssView_Sheet1.Cells[nRow - 1, 21].BackColor = Color.FromArgb(255, 255, 255);
                        }

                        ssView_Sheet1.Cells[nRow - 1, 27].Text = dt.Rows[i]["GbChild"].ToString().Trim();

                        #endregion
                    }

                    strNujuk = "";
                }

                dt.Dispose();
                dt = null;

                if (FstrDrg == "D" && chkDrg.Checked == false)
                {
                    if (nGETcount > 0)
                    {
                        SUB_TOT_SLIP1(ref nRow, ref nStot1, ref nStot2, ISBR);
                    }

                    Display_Drg_Amt(ref nRow, ref nAmt1, ref nAmt2, ref nStot1, ref nStot2, ref nGtot1, ref nGtot2, ref strBaseAmt, ref strQty, ref strNal, ISBR);
                }

                if (nGETcount > 0)
                {
                    SUB_TOT_SLIP1(ref nRow, ref nStot1, ref nStot2, ISBR);

                    GRAND_TOT_SLIP1(ref nRow, ref nGtot1, ref nGtot2, ISBR);

                    btnPrint.Enabled = true;
                    ssView.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void Read_IPD_SLIP2()
        {
            int i = 0;
            DataTable dt = null;
            DataTable dt2 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nNu = 0;
            int nNuChk = 0;
            int nRow = 0;
            int nRead = 0;
            int nInitNo = 0;
            long nAmt1 = 0;
            long nAmt2 = 0;
            long nStot1 = 0;
            long nStot2 = 0;
            long nGtot1 = 0;
            long nGtot2 = 0;
            long nTAmt = 0;
            long nTAmt2 = 0;
            string strNujuk = "";
            string strBaseAmt = "";
            string strQty = "";
            string strNal = "";
            string strAMT1 = "";
            string strAMT2 = "";
            string strBun = "";
            string strF = "";
            string str100 = "";

            //clsPmpaFunc cpf = new clsPmpaFunc();
            clsPmpaQuery cpq = new clsPmpaQuery();
            clsIuSentChk cisc = new clsIuSentChk();
            ComFunc cf = new ComFunc();
            clsPmpaType.ISBR ISBR = new clsPmpaType.ISBR();
            clsIument cIUM = new clsIument();

            ssView_Sheet1.ColumnHeader.Cells[0, 1].Text = "수가코드";
            ssView_Sheet1.ColumnHeader.Cells[0, 2].Text = "발생일자";

            nInitNo = 66;

            nNuChk = 0;
            nStot1 = 0;
            nStot2 = 0;
            nGtot1 = 0;
            nGtot2 = 0;
            nRow = 0;

            FnCnt_SU = 0;

            try
            {
                SQL = "";

                //SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(Bdate, 'yy-mm-dd') Bday, TO_CHAR(Bdate, 'yyyy-mm-dd') Bdate, I.GBSugbs,";
                //SQL = SQL + ComNum.VBLF + "        b.Sugbp, i.bun, Nu,";
                //SQL = SQL + ComNum.VBLF + "        Sucode, i.SuNext, SunameK,";
                //SQL = SQL + ComNum.VBLF + "        BaseAmt, Qty, Nal,";
                //SQL = SQL + ComNum.VBLF + "        DRG100, DRGF, TO_CHAR(i.EntDate, 'yyyy-mm-dd HH24:MI') Entdate,";
                //SQL = SQL + ComNum.VBLF + "        GbSpc, GbNgt, GbGisul,";
                //SQL = SQL + ComNum.VBLF + "        GbSelf, GbChild, Amt1,";
                //SQL = SQL + ComNum.VBLF + "        Amt2, Part, i.ROWID";

                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(Bdate, 'yy-mm-dd') Bday,TO_CHAR(Bdate, 'yyyy-mm-dd') Bdate,I.GBSugbs,b.Sugbp,";
                SQL = SQL + ComNum.VBLF + "i.bun,Nu,Sucode,i.SuNext,SunameK,BaseAmt,Qty,Nal,DRG100,DRGF,";
                SQL = SQL + ComNum.VBLF + "TO_CHAR(i.EntDate, 'yyyy-mm-dd HH24:MI') Entdate,TO_CHAR(i.EntDate, 'yyyy-mm-dd') Entdate2,";
                SQL = SQL + ComNum.VBLF + "GbSpc,GbNgt,GbGisul,GbSelf,GbChild,Amt1,Amt2,Part ,i.ROWID";


                if (clsPmpaType.IMST.GbOldSlip == false)
                {
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP I, " + ComNum.DB_PMPA + "BAS_SUN B";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_OLD_SLIP I, " + ComNum.DB_PMPA + "BAS_SUN B";
                }

                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";

                if (chkAll.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                }

                SQL = SQL + ComNum.VBLF + "    AND Nu >= '" + strFnu + "'";
                SQL = SQL + ComNum.VBLF + "    AND Nu <= '" + strTnu + "'";
                //*------------( 96/01/31 기간 Check 추가 lyj)--------------------------*

                if (strSDate != "")
                {
                    SQL = SQL + ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strSDate + "','yyyy-mm-dd')   ";
                }

                if (strEdate != "")
                {
                    SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strEdate + "','yyyy-mm-dd')   ";
                }

                //*-------------------------------------------------------------------------*
                SQL = SQL + ComNum.VBLF + "    AND (b.WONCODE NOT IN('1118') OR b.WONCODE IS NULL )"; //간호행위제외
                SQL = SQL + ComNum.VBLF + "    AND I.Sunext = B.Sunext";

                //저가약제 제외코드 2011-04-09
                SQL = SQL + ComNum.VBLF + "   AND TRIM(i.SUNEXT) NOT IN ( SELECT CODE FROM ADMIN.BAS_BCODE WHERE GUBUN ='원무영수제외코드') ";

                SQL = SQL + ComNum.VBLF + "  ORDER BY Nu, Sucode, I.Sunext, Bdate";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                nRead = dt.Rows.Count;
                nGETcount += nRead;

                for (i = 0; i < nRead; i++)
                {
                    strBun = dt.Rows[i]["BUN"].ToString().Trim();
                    str100 = dt.Rows[i]["DRG100"].ToString().Trim();
                    strF = dt.Rows[i]["DRGF"].ToString().Trim();//

                    if (DRG.READ_DRG_100(clsDB.DbCon, dt.Rows[i]["Sucode"].ToString().Trim(), "", clsPmpaType.TIT.InDate) == "OK")
                    {
                        str100 = "Y";
                    }
                    else
                    {
                        str100 = "";
                    }
                    nNu = (int)VB.Val(dt.Rows[i]["Nu"].ToString().Trim());
                    if (nNuChk == 0)
                    {
                        nNuChk = nNu;
                        strNujuk = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_누적행위명", nNu.ToString("00"));
                    }

                    if (nNu != nNuChk)
                    {
                        SUB_TOT_SLIP1(ref nRow, ref nStot1, ref nStot2, ISBR);

                        nNuChk = nNu;
                        strNujuk = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_누적행위명", nNu.ToString("00"));
                    }

                    #region DATA_MOVE_SLIP2

                    nRow += 1;
                    if (ssView_Sheet1.RowCount < nRow)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    nAmt1 = (long)VB.Val(dt.Rows[i]["Amt1"].ToString().Trim());
                    nAmt2 = (long)VB.Val(dt.Rows[i]["Amt2"].ToString().Trim());

                    strQty = VB.Val(dt.Rows[i]["Qty"].ToString().Trim()).ToString("##0.00");
                    strNal = VB.Val(dt.Rows[i]["Nal"].ToString().Trim()).ToString("##0");

                    if (FstrDrg == "D" && chkDrg.Checked == false)
                    {
                        if (strBun != "74" && strF != "Y" && str100 != "Y")
                        {
                            nAmt1 = 0;
                            strBaseAmt = "0";
                        }
                        else
                        {
                            strBaseAmt = VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("##,###,##0");
                        }

                        //날수가 0인것 금액 0
                        if (strNal == "0")
                        {
                            nAmt1 = 0;
                            nAmt2 = 0;
                        }
                    }
                    else
                    {
                        strBaseAmt = VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("##,###,##0");
                    }

                    nStot1 += nAmt1;
                    nStot2 += nAmt2;
                    nGtot1 += nAmt1;
                    nGtot2 += nAmt2;


                    strAMT1 = nAmt1.ToString("###,###,##0");
                    strAMT2 = nAmt2.ToString("###,###,##0");

                    ssView_Sheet1.Cells[nRow - 1, 0].Text = strNujuk;
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["Sucode"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["Bday"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["SunameK"].ToString().Trim();

                    //상급병실 종류표시
                    if (dt.Rows[i]["BUN"].ToString().Trim() == "77")
                    {
                        ssView_Sheet1.Cells[nRow - 1, 3].Text +=
                            "(" + cisc.Chk_Senior_Ward(clsDB.DbCon, clsPmpaType.TIT.Ipdno, clsPmpaType.TIT.Trsno, (long)VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim())) + ")";
                    }

                    ssView_Sheet1.Cells[nRow - 1, 4].Text = strBaseAmt;
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = strQty;
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = strNal;
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = strAMT1;
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = strAMT2;
                    ssView_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["Part"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["GbSpc"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 11].Text = dt.Rows[i]["GbNgt"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 12].Text = dt.Rows[i]["GbGisul"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 13].Text = dt.Rows[i]["GbSelf"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 14].Text = dt.Rows[i]["Sugbp"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 21].Text = dt.Rows[i]["GBSugbs"].ToString().Trim();

                    //P항 표시관련
                    if (nAmt1 != 0 && string.Compare(dt.Rows[i]["Nu"].ToString().Trim(), "21") >= 0)
                    {
                        if ((dt.Rows[i]["SugbP"].ToString().Trim() == "0" || dt.Rows[i]["SugbP"].ToString().Trim() == "2" ||
                             dt.Rows[i]["SugbP"].ToString().Trim() == "9") && dt.Rows[i]["GBSugbS"].ToString().Trim() != "1")
                        {
                            ssView_Sheet1.Cells[nRow - 1, 27].BackColor = Color.FromArgb(255, 0, 0);
                        }
                    }

                    ssView_Sheet1.Cells[nRow - 1, 15].CellType = new FarPoint.Win.Spread.CellType.CheckBoxCellType();

                    SQL = "";
                    SQL = SQL + "SELECT RATE, BOAMT, REMARK,";
                    SQL = SQL + "       QTY, NAL, Amt1,";
                    SQL = SQL + "       Amt2 ";
                    SQL = SQL + "  FROM " + ComNum.DB_PMPA + "IPD_SU_SLIP ";
                    SQL = SQL + " WHERE 1 = 1";
                    SQL = SQL + "   AND PANO  = '" + clsPmpaType.TIT.Pano + "' ";
                    SQL = SQL + "   AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                    SQL = SQL + "   AND TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                    SQL = SQL + "   AND SUCODE = '" + dt.Rows[i]["Sucode"].ToString().Trim() + "' ";
                    SQL = SQL + "   AND SuNext = '" + dt.Rows[i]["Sunext"].ToString().Trim() + "' ";
                    SQL = SQL + "   AND BDate =TO_DATE('" + dt.Rows[i]["Bday"].ToString().Trim() + "','YY-MM-DD') ";
                    SQL = SQL + "   AND t_ROWID ='" + dt.Rows[i]["ROWID"].ToString().Trim() + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt2.Rows.Count > 0)
                    {
                        FstrChk = "NO";
                        ssView_Sheet1.Cells[nRow - 1, 16].Text = dt2.Rows[0]["QTY"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 17].Text = dt2.Rows[0]["NAL"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 18].Text = VB.Val(dt2.Rows[0]["Amt1"].ToString().Trim()).ToString("###,###,###,###");
                        ssView_Sheet1.Cells[nRow - 1, 19].Text = VB.Val(dt2.Rows[0]["Amt2"].ToString().Trim()).ToString("###,###,###,###");

                        if (dt.Rows[i]["Qty"].ToString().Trim() == dt2.Rows[0]["QTY"].ToString().Trim() &&
                           dt.Rows[i]["Nal"].ToString().Trim() == dt2.Rows[0]["Nal"].ToString().Trim() &&
                           (nAmt1 + nAmt2) != (VB.Val(dt2.Rows[0]["nAmt1"].ToString().Trim()) + VB.Val(dt2.Rows[0]["Amt2"].ToString().Trim())))
                        {
                            FnCnt_SU += 1;
                            ssView_Sheet1.Cells[nRow - 1, 18].BackColor = Color.FromArgb(255, 0, 0);
                        }

                        FstrChk = "";
                    }
                    else
                    {
                        ssView_Sheet1.Cells[nRow - 1, 16].Text = strQty;
                        ssView_Sheet1.Cells[nRow - 1, 17].Text = strNal;
                    }

                    dt2.Dispose();
                    dt2 = null;

                    ssView_Sheet1.Cells[nRow - 1, 20].Text = dt.Rows[i]["BUN"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 21].Text = dt.Rows[i]["GBSugbs"].ToString().Trim();

                    //선별급여색깔 표시
                    if (cf.READ_BAS_Sun_S(clsDB.DbCon, dt.Rows[i]["SuNext"].ToString().Trim()) == "6" || cf.READ_BAS_Sun_S(clsDB.DbCon, dt.Rows[i]["SuNext"].ToString().Trim()) == "7")
                    {
                        ssView_Sheet1.Cells[nRow - 1, 21].BackColor = Color.FromArgb(169, 208, 142);
                    }
                    else
                    {
                        ssView_Sheet1.Cells[nRow - 1, 21].BackColor = Color.FromArgb(255, 255, 255);
                    }

                    ssView_Sheet1.Cells[nRow - 1, 22].Text = dt.Rows[i]["Nu"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 23].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 24].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                    cpq.Read_Suga_Amt(clsDB.DbCon, dt.Rows[i]["SuCode"].ToString().Trim(), dt.Rows[i]["SuNext"].ToString().Trim(), dt.Rows[i]["BDate"].ToString().Trim());

                    if (GnbAmt_new == 0)
                    {
                        ssView_Sheet1.Cells[nRow - 1, 25].Text = "Y";
                        ssView_Sheet1.Cells[nRow - 1, 4].BackColor = Color.FromArgb(255, 255, 0);
                    }

                    ssView_Sheet1.Cells[nRow - 1, 26].Text = dt.Rows[i]["Entdate"].ToString().Trim();

                    if (string.Compare(FstrMinActDate, dt.Rows[i]["Entdate2"].ToString().Trim()) < 0)
                    {
                        ssView_Sheet1.Cells[nRow - 1, 26].ForeColor = Color.Blue;
                    }
                    else
                    {
                        ssView_Sheet1.Cells[nRow - 1, 26].ForeColor = Color.Black;
                    }

                    ssView_Sheet1.Cells[nRow - 1, 27].Text = dt.Rows[i]["GbChild"].ToString().Trim();

                    if (dt.Rows[i]["Bun"].ToString().Trim() == "22" || dt.Rows[i]["Bun"].ToString().Trim() == "23")
                    {
                        //마취코드는 변경안됨
                        ssView_Sheet1.Cells[nRow - 1, 16, nRow - 1, 17].Locked = true;
                    }


                    #endregion

                    strNujuk = "";
                }

                dt.Dispose();
                dt = null;

                if (FstrDrg == "D" && chkDrg.Checked == false)
                {
                    if (nGETcount > 0)
                    {
                        SUB_TOT_SLIP1(ref nRow, ref nStot1, ref nStot2, ISBR);

                        Display_Drg_Amt(ref nRow, ref nAmt1, ref nAmt2, ref nStot1, ref nStot2, ref nGtot1, ref nGtot2, ref strBaseAmt, ref strQty, ref strNal, ISBR);
                    }
                }

                if (nGETcount > 0)
                {
                    SUB_TOT_SLIP1(ref nRow, ref nStot1, ref nStot2, ISBR);

                    GRAND_TOT_SLIP1(ref nRow, ref nGtot1, ref nGtot2, ISBR);

                    btnPrint.Enabled = true;
                    ssView.Enabled = true;
                }

                //수동명세서 사유 SELECT
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT REMARK";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_SU_SLIP ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND PANO = '" + clsPmpaType.TIT.Pano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                SQL = SQL + ComNum.VBLF + "   AND TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                SQL = SQL + ComNum.VBLF + "   AND REMARK IS NOT NULL ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    txtRemark.Text = dt.Rows[0]["REMARK"].ToString().Trim();
                }
                else
                {
                    txtRemark.Text = "";
                }

                dt.Dispose();
                dt = null;

                nTAmt = 0;
                nTAmt2 = 0;

                for (i = 0; i < ssView_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
                {
                    if (ssView_Sheet1.Cells[i, 15].Text == "1")
                    {
                        nTAmt += (long)VB.Val(VB.TR(ssView_Sheet1.Cells[i, 18].Text, ",", ""));
                        nTAmt2 += (long)VB.Val(VB.TR(ssView_Sheet1.Cells[i, 19].Text, ",", ""));
                    }
                }
                
                ssView_Sheet1.Cells[ssView_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data) -1, 18].Text = nTAmt.ToString("###,###,###,##0");
                ssView_Sheet1.Cells[ssView_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data) -1, 19].Text = nTAmt2.ToString("###,###,###,##0");

                if (FnCnt_SU != 0)
                {
                    ComFunc.MsgBox("계산금액이 선택한 항목중 다른것이 있습니다. 확인 후 수동수정하십시오.");
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void Read_IPD_SLIP4()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nNu = 0;
            int nNuChk = 0;
            int nRow = 0;
            int nRead = 0;

            long nAmt1 = 0;
            long nAmt2 = 0;
            long nStot1 = 0;
            long nStot2 = 0;
            long nGtot1 = 0;
            long nGtot2 = 0;

            string strSuNext = "";
            string strNujuk = "";
            string strBaseAmt = "";
            string strQty = "";
            string strNal = "";
            string strAMT1 = "";
            string strAMT2 = "";
            string strBun = "";
            string str100 = "";
            string strF = "";

            long nBAMT = 0;
            int nBonRate = 0;

            string strSuGbP = string.Empty;
            string strGbSuGbS = string.Empty;
            string strGbSelf = string.Empty;

            clsIuSentChk cisc = new clsIuSentChk();
            ComFunc cf = new ComFunc();
            clsPmpaType.ISBR ISBR = new clsPmpaType.ISBR();
            clsIument cIUM = new clsIument();

            ssView_Sheet1.ColumnHeader.Cells[0, 1].Text = "수가코드";
            ssView_Sheet1.ColumnHeader.Cells[0, 2].Text = "표준코드";

            for (i = 0; i < 2; i++)
            {
                ISBR.nBBAmt[i] = 0; ISBR.nBGAmt[i] = 0; ISBR.nBJAmt[i] = 0; ISBR.nBFAmt[i] = 0;
            }

            nNuChk = 0;
            nStot1 = 0;
            nStot2 = 0;
            nGtot1 = 0;
            nGtot2 = 0;
            nRow = 0;

            try
            {
                #region SQL Query
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT I.Sucode, I.Sunext, SunameK,";
                SQL = SQL + ComNum.VBLF + "        B.BCode, BaseAmt, Qty,";
                SQL = SQL + ComNum.VBLF + "        I.GBSugbs, b.Sugbp, GbSpc,";
                SQL = SQL + ComNum.VBLF + "        GbNgt,nvl(gber,'0') gber, GbGisul, GbSelf,";
                SQL = SQL + ComNum.VBLF + "        GbChild, Nu, Bun,";
                SQL = SQL + ComNum.VBLF + "        DRG100, DRGF, SUM(Nal) Nalsu,";
                SQL = SQL + ComNum.VBLF + "        SUM(Amt1) Amtt1, SUM(Amt2) Amtt2, ";
                SQL = SQL + ComNum.VBLF + "        to_char(min(i.bdate),'mm-dd') sdate, to_char(max(i.bdate),'mm-dd') tdate  ";
                //SQL = SQL + ComNum.VBLF + "        FC_ACCOUNT_BON_AMT(i.pano,i.bi,i.SuNext,SUM(Amt1) ,Qty,bun,nu,to_char(min(i.bdate),'yyyy-mm-dd'),'I','**',gbsugbs,trsno,GbSelf) BON ";

                if (clsPmpaType.IMST.GbOldSlip == false)
                {
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP I, " + ComNum.DB_PMPA + "BAS_SUN B";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_OLD_SLIP I, " + ComNum.DB_PMPA + "BAS_SUN B";
                }

                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";

                if (chkAll.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                }

                SQL = SQL + ComNum.VBLF + "    AND Nu >= '" + strFnu + "'";
                SQL = SQL + ComNum.VBLF + "    AND Nu <= '" + strTnu + "'";
                //*------------( 96/01/31 기간 Check 추가 lyj)--------------------------*

                if (strSDate != "")
                {
                    SQL = SQL + ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strSDate + "','yyyy-mm-dd')   ";
                }

                if (strEdate != "")
                {
                    SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strEdate + "','yyyy-mm-dd')   ";
                }

                //*-------------------------------------------------------------------------*
                SQL = SQL + ComNum.VBLF + "    AND (b.WONCODE NOT IN('1118') OR b.WONCODE IS NULL )"; //간호행위제외
                SQL = SQL + ComNum.VBLF + "    AND I.Sunext = B.Sunext";

                //저가약제 제외코드 2011-04-09
                SQL = SQL + ComNum.VBLF + "   AND TRIM(i.SUNEXT) NOT IN ( SELECT CODE FROM " + ComNum.DB_PMPA + "BAS_BCODE WHERE GUBUN ='원무영수제외코드') ";

                SQL = SQL + ComNum.VBLF + "  GROUP BY I.Sucode, I.Sunext, SunameK, B.Bcode, BaseAmt, Qty,I.GBSugbs, b.Sugbp, i.pano,i.bi, ";
                SQL = SQL + ComNum.VBLF + "           GbSpc, GbNgt,nvl(gber,'0'), GbGisul, GbSelf, GbChild, Nu, Bun,DRG100,DRGF,trsno  ";

                SQL = SQL + ComNum.VBLF + "  ORDER BY  Nu, Bun, I.Sucode";
                #endregion

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                nRead = dt.Rows.Count;

                nGETcount += nRead;

                for (i = 0; i < nRead; i++)
                {
                    strBun = dt.Rows[i]["Bun"].ToString().Trim();
                    str100 = dt.Rows[i]["DRG100"].ToString().Trim();
                    strF = dt.Rows[i]["DRGF"].ToString().Trim();
                    strSuNext = dt.Rows[i]["SUNEXT"].ToString().Trim();
                    strSuGbP = dt.Rows[i]["SUGBP"].ToString().Trim();
                    strGbSuGbS = dt.Rows[i]["GBSUGBS"].ToString().Trim();
                    strGbSelf = dt.Rows[i]["GBSELF"].ToString().Trim();

                    if (DRG.READ_DRG_100(clsDB.DbCon, dt.Rows[i]["Sucode"].ToString().Trim(), "NEW", clsPmpaType.TIT.InDate) == "OK")
                    {
                        str100 = "Y";
                    }
                    else
                    {
                        str100 = "";
                    }
                    if (VB.Val(dt.Rows[i]["Nu"].ToString().Trim()) != 0)
                    {
                        nNu = (int)VB.Val(dt.Rows[i]["Nu"].ToString().Trim());
                    }

                    if (nNuChk == 0)
                    {
                        nNuChk = nNu;
                        strNujuk = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_누적행위명", nNu.ToString("00"));
                    }

                    if (nNu != nNuChk)
                    {
                        SUB_TOT_SLIP1(ref nRow, ref nStot1, ref nStot2, ISBR);

                        nNuChk = nNu;
                        strNujuk = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_누적행위명", nNu.ToString("00"));
                    }

                    //심사계 요청
                    #region DATA_MOVE_SLIP1

                    nRow += 1;
                    if (ssView_Sheet1.RowCount < nRow)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    nAmt1 = (long)VB.Val(dt.Rows[i]["Amtt1"].ToString().Trim());
                    nAmt2 = (long)VB.Val(dt.Rows[i]["Amtt2"].ToString().Trim());

                    strQty = VB.Val(dt.Rows[i]["Qty"].ToString().Trim()).ToString("##0.00");
                    strNal = VB.Val(dt.Rows[i]["Nalsu"].ToString().Trim()).ToString("##0");

                    if (FstrDrg == "D" && chkDrg.Checked == false)
                    {
                        if (strBun != "74" && strF != "Y" && str100 != "Y")
                        {
                            nAmt1 = 0;
                            strBaseAmt = "0";
                        }
                        else
                        {
                            strBaseAmt = VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("##,###,##0");
                        }

                        //날수가 0인것 금액 0
                        if (strNal == "0")
                        {
                            nAmt1 = 0;
                            nAmt2 = 0;
                        }
                    }
                    else
                    {
                        strBaseAmt = VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("##,###,##0");
                    }

                    nStot1 += nAmt1;
                    nStot2 += nAmt2;
                    nGtot1 += nAmt1;
                    nGtot2 += nAmt2;

                    strAMT1 = nAmt1.ToString("###,###,##0");
                    strAMT2 = nAmt2.ToString("###,###,##0");

                    ssView_Sheet1.Cells[nRow - 1, 0].Text = strNujuk;
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["Sucode"].ToString().Trim();
                    //ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["sdate"].ToString().Trim() + "~" + dt.Rows[i]["Tdate"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["Bcode"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["SunameK"].ToString().Trim();

                    //상급병실 종류표시
                    if (dt.Rows[i]["BUN"].ToString().Trim() == "77")
                    {
                        ssView_Sheet1.Cells[nRow - 1, 3].Text +=
                            "(" + cisc.Chk_Senior_Ward(clsDB.DbCon, clsPmpaType.TIT.Ipdno, clsPmpaType.TIT.Trsno, (long)VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim())) + ")";
                    }

                    ssView_Sheet1.Cells[nRow - 1, 4].Text = strBaseAmt;
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = strQty;
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = strNal;
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = strAMT1;
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = strAMT2;
                    ssView_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["gber"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 11].Text = dt.Rows[i]["GbNgt"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 12].Text = dt.Rows[i]["GbGisul"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 13].Text = dt.Rows[i]["GbSelf"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 14].Text = dt.Rows[i]["Sugbp"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 20].Text = dt.Rows[i]["BUN"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 21].Text = dt.Rows[i]["GBSugbs"].ToString().Trim();

                    //선별급여색깔 표시
                    if (cf.READ_BAS_Sun_S(clsDB.DbCon, dt.Rows[i]["SuNext"].ToString().Trim()) == "6" || cf.READ_BAS_Sun_S(clsDB.DbCon, dt.Rows[i]["SuNext"].ToString().Trim()) == "7")
                    {
                        ssView_Sheet1.Cells[nRow - 1, 21].BackColor = Color.FromArgb(169, 208, 142);
                    }
                    else
                    {
                        ssView_Sheet1.Cells[nRow - 1, 21].BackColor = Color.FromArgb(255, 255, 255);
                    }
                    ssView_Sheet1.Cells[nRow - 1, 22].Text = dt.Rows[i]["Nu"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 23].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 27].Text = dt.Rows[i]["GbChild"].ToString().Trim();

                    //P항 표시관련
                    if (nAmt1 != 0 && string.Compare(dt.Rows[i]["Nu"].ToString().Trim(), "21") >= 0)
                    {
                        if ((dt.Rows[i]["SugbP"].ToString().Trim() == "0" || dt.Rows[i]["SugbP"].ToString().Trim() == "2" ||
                             dt.Rows[i]["SugbP"].ToString().Trim() == "9") && dt.Rows[i]["GBSugbs"].ToString().Trim() != "1")
                        {
                            ssView_Sheet1.Cells[nRow - 1, 27].BackColor = Color.FromArgb(255, 0, 0);
                        }
                    }

                    //수가별 본인부담금 표시
                    clsIuSentChk cISCHK = new clsIuSentChk();

                    ISBR.BI = clsPmpaType.TIT.Bi;
                    ISBR.INDATE = clsPmpaType.TIT.M_InDate;
                    ISBR.VCODE = clsPmpaType.TIT.VCode;
                    ISBR.FCODE = clsPmpaType.TIT.FCode;
                    ISBR.OGPDBUN = clsPmpaType.TIT.OgPdBun;
                    ISBR.BOHUN = clsPmpaType.TIT.Bohun;
                    ISBR.GBDRG = clsPmpaType.TIT.GbDRG;
                    ISBR.DTLBUN = cISCHK.Read_Bas_Sun_ColName(clsDB.DbCon, strSuNext, "DTLBUN", dt.Rows[i]["Sucode"].ToString().Trim());  //2018-10-29 
                    ISBR.QTY = VB.Val(strQty);
                    ISBR.NAL = (int)VB.Val(strNal);
                    ISBR.DRGF = strF;
                    ISBR.DRG100 = str100;
                    ISBR.SUGBP = strSuGbP;
                    ISBR.GBSUGBS = strGbSuGbS;
                    ISBR.AMT = nAmt1;
                    ISBR.GBSELF = strGbSelf;
                    ISBR.BUN = strBun;
                    ISBR.NU = nNu;
                    ISBR.SUNEXT = strSuNext;

                    cIUM.Ipd_Slip_Bon_Rate(clsDB.DbCon, ISBR);

                    ssView_Sheet1.Cells[nRow - 1, 28].Text = ISBR.nBBAmt[0].ToString("###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 29].Text = ISBR.nBGAmt[0].ToString("###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 30].Text = ISBR.nBJAmt[0].ToString("###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 31].Text = ISBR.nBFAmt[0].ToString("###,##0");

                    #endregion

                    #region AS-IS에서 주석처리 되어있음
                    //if (nNu == 6)
                    //{
                    //    //마취 합산분 풀어서 Display

                    //    if (strGbMACH == "8")
                    //    {
                    //        #region DATA_MARCHI_MOVE_SLIP8

                    //        if (nInitNo != 66)
                    //        {
                    //            return;
                    //        }

                    //        nInitNo = 88;

                    //        SQL = "";
                    //        SQL = SQL + ComNum.VBLF + " SELECT Sucode Sucode1, I.Sunext Sunext1, SunameK SunameK1,      ";
                    //        SQL = SQL + ComNum.VBLF + "        Hcode Hcode1, BaseAmt BaseAmt1, Qty Qty1, I.GBSugbs Sugbs1,b.Sugbp Sugbp1,";
                    //        SQL = SQL + ComNum.VBLF + "        GbSpc GbSpc1, GbNgt GbNgt1, GbGisul GbGisul1,            ";
                    //        SQL = SQL + ComNum.VBLF + "        GbSelf GbSelf1, GbChild GbChild1, Nu Nu1,                ";
                    //        SQL = SQL + ComNum.VBLF + "        Nal Nalsu1, Amt1 Amtt11, Amt2 Amtt21,                    ";
                    //        SQL = SQL + ComNum.VBLF + "        BUN                                                      ";
                    //        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP I,                     ";
                    //        SQL = SQL + ComNum.VBLF + "       " + ComNum.DB_PMPA + "BAS_SUN B                           ";
                    //        SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                                                    ";

                    //        if (chkAll.Checked == true)
                    //        {
                    //            SQL = SQL + ComNum.VBLF + "  AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                    //        }
                    //        else
                    //        {
                    //            SQL = SQL + ComNum.VBLF + "  AND TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                    //        }

                    //        SQL = SQL + ComNum.VBLF + "    AND Nu = '06'                                ";
                    //        SQL = SQL + ComNum.VBLF + "    AND I.Sunext = B.Sunext                      ";
                    //        //*------------( 96/01/31 기간 Check 추가 lyj)--------------------------*
                    //        if (strSDate != "")
                    //        {
                    //            SQL = SQL + ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strSDate + "','yyyy-mm-dd')    ";
                    //        }
                    //        if (strEdate != "")
                    //        {
                    //            SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strEdate + "','yyyy-mm-dd')    ";
                    //        }
                    //        SQL = SQL + ComNum.VBLF + " GROUP BY Sucode , I.Sunext , SunameK , Bcode , BaseAmt , Qty ,I.GBSugbs ,";
                    //        SQL = SQL + ComNum.VBLF + "          b.Sugbp ,   GbSpc , GbNgt , GbGisul , GbSelf, GbChild , Nu ,  BUN ";
                    //        SQL = SQL + ComNum.VBLF + "  HAVING SUM(NAL) <> 0";
                    //        SQL = SQL + ComNum.VBLF + "  ORDER BY Nu, Sucode, I.Sunext";

                    //        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                    //        if (SqlErr != "")
                    //        {
                    //            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    //            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    //            return;
                    //        }

                    //        if (dt2.Rows.Count == 0)
                    //        {
                    //            dt2.Dispose();
                    //            dt2 = null;
                    //        }
                    //        else
                    //        {
                    //            nCNT2 = dt2.Rows.Count;

                    //            for (k = 0; k < nCNT2; k++)
                    //            {
                    //                #region DATA_MOVE_SLIP1_2

                    //                nRow += 1;
                    //                if (ssView_Sheet1.RowCount < nRow)
                    //                {
                    //                    ssView_Sheet1.RowCount = nRow;
                    //                }

                    //                if (k > 0)
                    //                {
                    //                    strNujuk = "          ";
                    //                }

                    //                nAmt1 = (long)VB.Val(dt2.Rows[k]["Amtt11"].ToString().Trim());
                    //                nAmt2 = (long)VB.Val(dt2.Rows[k]["Amtt21"].ToString().Trim());

                    //                strQty = VB.Val(dt2.Rows[k]["Qty1"].ToString().Trim()).ToString("##0.00");
                    //                strNal = VB.Val(dt2.Rows[k]["Nalsu1"].ToString().Trim()).ToString("##0");

                    //                if (FstrDrg == "D" && chkDrg.Checked == false)
                    //                {
                    //                    if (strBun != "74" && strF != "Y" && str100 != "Y")
                    //                    {
                    //                        nAmt1 = 0;
                    //                        strBaseAmt = "0";
                    //                    }
                    //                    else
                    //                    {
                    //                        strBaseAmt = VB.Val(dt2.Rows[k]["BaseAmt1"].ToString().Trim()).ToString("##,###,##0");
                    //                    }

                    //                    //날수가 0인것 금액 0
                    //                    if (strNal == "0")
                    //                    {
                    //                        nAmt1 = 0;
                    //                        nAmt2 = 0;
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    strBaseAmt = VB.Val(dt2.Rows[k]["BaseAmt1"].ToString().Trim()).ToString("##,###,##0");
                    //                }

                    //                nStot1 += nAmt1;
                    //                nStot2 += nAmt2;
                    //                nGtot1 += nAmt1;
                    //                nGtot2 += nAmt2;


                    //                strAMT1 = nAmt1.ToString("###,###,##0");
                    //                strAMT2 = nAmt2.ToString("###,###,##0");

                    //                ssView_Sheet1.Cells[nRow - 1, 0].Text = strNujuk;
                    //                ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[k]["Sucode"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[k]["Bcode"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[k]["SunameK"].ToString().Trim();

                    //                //상급병실 종류표시
                    //                if (dt2.Rows[k]["BUN"].ToString().Trim() == "77")
                    //                {
                    //                    ssView_Sheet1.Cells[nRow - 1, 3].Text +=
                    //                        "(" + cisc.Chk_Senior_Ward(clsDB.DbCon, clsPmpaType.TIT.Ipdno, clsPmpaType.TIT.Trsno, (long)VB.Val(dt2.Rows[k]["BaseAmt"].ToString().Trim())) + ")";
                    //                }

                    //                ssView_Sheet1.Cells[nRow - 1, 4].Text = strBaseAmt;
                    //                ssView_Sheet1.Cells[nRow - 1, 5].Text = strQty;
                    //                ssView_Sheet1.Cells[nRow - 1, 6].Text = strNal;
                    //                ssView_Sheet1.Cells[nRow - 1, 7].Text = strAMT1;
                    //                ssView_Sheet1.Cells[nRow - 1, 8].Text = strAMT2;
                    //                ssView_Sheet1.Cells[nRow - 1, 10].Text = dt2.Rows[k]["GbSpc1"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 11].Text = dt2.Rows[k]["GbNgt1"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 12].Text = dt2.Rows[k]["GbGisul1"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 13].Text = dt2.Rows[k]["GbSelf1"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 14].Text = dt2.Rows[k]["Sugbp1"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 20].Text = dt2.Rows[k]["BUN"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 21].Text = dt2.Rows[k]["Sugbs1"].ToString().Trim();

                    //                //선별급여색깔 표시
                    //                if (cf.READ_BAS_Sun_S(clsDB.DbCon, dt2.Rows[k]["SuNext1"].ToString().Trim()) == "6" || cf.READ_BAS_Sun_S(clsDB.DbCon, dt2.Rows[k]["SuNext1"].ToString().Trim()) == "7")
                    //                {
                    //                    ssView_Sheet1.Cells[nRow - 1, 21].BackColor = Color.FromArgb(169, 208, 142);
                    //                }
                    //                else
                    //                {
                    //                    ssView_Sheet1.Cells[nRow - 1, 21].BackColor = Color.FromArgb(255, 255, 255);
                    //                }
                    //                ssView_Sheet1.Cells[nRow - 1, 22].Text = dt2.Rows[k]["Nu1"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 23].Text = dt2.Rows[k]["SuNext1"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 27].Text = dt2.Rows[k]["GbChild1"].ToString().Trim();

                    //                //P항 표시관련
                    //                if (nAmt1 != 0 && string.Compare(dt2.Rows[k]["Nu1"].ToString().Trim(), "21") >= 0)
                    //                {
                    //                    if ((dt2.Rows[k]["SugbP1"].ToString().Trim() == "0" || dt2.Rows[k]["SugbP1"].ToString().Trim() == "2" ||
                    //                       dt2.Rows[k]["SugbP1"].ToString().Trim() == "9") && dt2.Rows[k]["SugbS1"].ToString().Trim() != "1")
                    //                    {
                    //                        ssView_Sheet1.Cells[nRow - 1, 27].BackColor = Color.FromArgb(255, 0, 0);
                    //                    }
                    //                }

                    //                nTAMT = nAmt1;

                    //                if (dt2.Rows[k]["Sugbs1"].ToString().Trim() == "6" || dt2.Rows[k]["Sugbs1"].ToString().Trim() == "7" || dt2.Rows[k]["Sugbs1"].ToString().Trim() == "8")
                    //                {
                    //                    nBonRate = Bon_Rate_100(dt2.Rows[k]["Sugbs1"].ToString().Trim());
                    //                }
                    //                else
                    //                {
                    //                    nBonRate = 0;
                    //                }

                    //                //전액본인부담
                    //                if (nBonRate > 0)
                    //                {
                    //                    nBAMT = (long)Math.Truncate(nAmt1 * (nBonRate / 100.0));
                    //                    ssView_Sheet1.Cells[nRow - 1, 30].Text = nBAMT.ToString("###,##0");
                    //                }
                    //                else
                    //                {
                    //                    //비급여
                    //                    if (dt2.Rows[k]["GbSelf1"].ToString().Trim() != "0")
                    //                    {
                    //                        nBonRate = Bon_Rate_100(dt2.Rows[k]["Sugbs1"].ToString().Trim());

                    //                        if (nBonRate > 0)
                    //                        {
                    //                            nBAMT = (long)Math.Truncate(nAmt1 * (nBonRate / 100.0));
                    //                        }
                    //                        else
                    //                        {
                    //                            nBAMT = nAmt1;
                    //                        }

                    //                        ssView_Sheet1.Cells[nRow - 1, 31].Text = nBAMT.ToString("###,##0");
                    //                    }
                    //                    else
                    //                    {
                    //                        //본인부담
                    //                        if (dt2.Rows[k]["BUN"].ToString().Trim() == "01" || dt2.Rows[k]["BUN"].ToString().Trim() == "02")
                    //                        {
                    //                            nBAMT = (long)Math.Truncate(nAmt1 * (clsPmpaType.IBR.Jin / 100.0));
                    //                        }
                    //                        else if (dt2.Rows[k]["BUN"].ToString().Trim() == "72" || dt2.Rows[k]["BUN"].ToString().Trim() == "73")
                    //                        {
                    //                            nBAMT = (long)Math.Truncate(nAmt1 * (clsPmpaType.IBR.CTMRI / 100.0));
                    //                        }
                    //                        else if (dt2.Rows[k]["BUN"].ToString().Trim() == "74")
                    //                        {
                    //                            nBAMT = (long)Math.Truncate(nAmt1 * (clsPmpaType.IBR.Food / 100.0));
                    //                        }
                    //                        else
                    //                        {
                    //                            nBAMT = (long)Math.Truncate(nAmt1 * (clsPmpaType.IBR.Bohum / 100.0));
                    //                        }

                    //                        ssView_Sheet1.Cells[nRow - 1, 28].Text = nBAMT.ToString("###,##0");

                    //                    }

                    //                }

                    //                ssView_Sheet1.Cells[nRow - 1, 29].Text = (nTAMT - nBAMT).ToString("###,##0");   //공단부담

                    //                //마취코드는 변경 안 됨
                    //                ssView_Sheet1.Cells[nRow - 1, 16, nRow - 1, 17].Locked = true;

                    //                #endregion
                    //            }

                    //            dt2.Dispose();
                    //            dt2 = null;
                    //        }

                    //        #endregion
                    //    }
                    //    else
                    //    {
                    //        #region DATA_MARCHI_MOVE_SLIP1

                    //        if (nInitNo != 66)
                    //        {
                    //            return;
                    //        }

                    //        nInitNo = 88;

                    //        SQL = "";
                    //        SQL = SQL + ComNum.VBLF + " SELECT Sucode Sucode1, I.Sunext Sunext1, SunameK SunameK1,  ";
                    //        SQL = SQL + ComNum.VBLF + "        Hcode Hcode1, BaseAmt BaseAmt1, Qty Qty1,            ";
                    //        SQL = SQL + ComNum.VBLF + "        I.GBSugbs Sugbs1,b.Sugbp Sugbp1,                     ";
                    //        SQL = SQL + ComNum.VBLF + "        GbSpc GbSpc1, GbNgt GbNgt1, GbGisul GbGisul1,        ";
                    //        SQL = SQL + ComNum.VBLF + "        GbSelf GbSelf1, GbChild GbChild1, Nu Nu1,            ";
                    //        SQL = SQL + ComNum.VBLF + "        Nal Nalsu1, Amt1 Amtt11, Amt2 Amtt21,                ";
                    //        SQL = SQL + ComNum.VBLF + "        BUN                                                  ";
                    //        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP I,                 ";
                    //        SQL = SQL + ComNum.VBLF + "       " + ComNum.DB_PMPA + "BAS_SUN B                       ";
                    //        SQL = SQL + ComNum.VBLF + " WHERE 1 = 1                                                 ";

                    //        if (chkAll.Checked == true)
                    //        {
                    //            SQL = SQL + ComNum.VBLF + "  AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                    //        }
                    //        else
                    //        {
                    //            SQL = SQL + ComNum.VBLF + "  AND TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                    //        }

                    //        SQL = SQL + ComNum.VBLF + "    AND Nu = '06'";
                    //        SQL = SQL + ComNum.VBLF + "    AND I.Sunext = B.Sunext";
                    //        //*------------( 96/01/31 기간 Check 추가 lyj)--------------------------*
                    //        if (strSDate != "")
                    //        {
                    //            SQL = SQL + ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strSDate + "','yyyy-mm-dd')";
                    //        }
                    //        if (strEdate != "")
                    //        {
                    //            SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strEdate + "','yyyy-mm-dd')";
                    //        }
                    //        SQL = SQL + ComNum.VBLF + "  ORDER BY Nu, Sucode, I.Sunext";

                    //        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                    //        if (SqlErr != "")
                    //        {
                    //            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    //            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    //            return;
                    //        }

                    //        if (dt2.Rows.Count == 0)
                    //        {
                    //            dt2.Dispose();
                    //            dt2 = null;
                    //        }
                    //        else
                    //        {
                    //            nCNT2 = dt2.Rows.Count;

                    //            for (k = 0; k < nCNT2; k++)
                    //            {
                    //                #region DATA_MOVE_SLIP1_2

                    //                nRow += 1;
                    //                if (ssView_Sheet1.RowCount < nRow)
                    //                {
                    //                    ssView_Sheet1.RowCount = nRow;
                    //                }

                    //                if (k > 0)
                    //                {
                    //                    strNujuk = "          ";
                    //                }

                    //                nAmt1 = (long)VB.Val(dt2.Rows[k]["Amtt11"].ToString().Trim());
                    //                nAmt2 = (long)VB.Val(dt2.Rows[k]["Amtt21"].ToString().Trim());

                    //                strQty = VB.Val(dt2.Rows[k]["Qty1"].ToString().Trim()).ToString("##0.00");
                    //                strNal = VB.Val(dt2.Rows[k]["Nalsu1"].ToString().Trim()).ToString("##0");

                    //                if (FstrDrg == "D" && chkDrg.Checked == false)
                    //                {
                    //                    if (strBun != "74" && strF != "Y" && str100 != "Y")
                    //                    {
                    //                        nAmt1 = 0;
                    //                        strBaseAmt = "0";
                    //                    }
                    //                    else
                    //                    {
                    //                        strBaseAmt = VB.Val(dt2.Rows[k]["BaseAmt1"].ToString().Trim()).ToString("##,###,##0");
                    //                    }

                    //                    //날수가 0인것 금액 0
                    //                    if (strNal == "0")
                    //                    {
                    //                        nAmt1 = 0;
                    //                        nAmt2 = 0;
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    strBaseAmt = VB.Val(dt2.Rows[k]["BaseAmt1"].ToString().Trim()).ToString("##,###,##0");
                    //                }

                    //                nStot1 += nAmt1;
                    //                nStot2 += nAmt2;
                    //                nGtot1 += nAmt1;
                    //                nGtot2 += nAmt2;

                    //                strAMT1 = nAmt1.ToString("###,###,##0");
                    //                strAMT2 = nAmt2.ToString("###,###,##0");

                    //                ssView_Sheet1.Cells[nRow - 1, 0].Text = strNujuk;
                    //                ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[k]["Sucode"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[k]["Bcode"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[k]["SunameK"].ToString().Trim();

                    //                //상급병실 종류표시
                    //                if (dt2.Rows[k]["BUN"].ToString().Trim() == "77")
                    //                {
                    //                    ssView_Sheet1.Cells[nRow - 1, 3].Text +=
                    //                        "(" + cisc.Chk_Senior_Ward(clsDB.DbCon, clsPmpaType.TIT.Ipdno, clsPmpaType.TIT.Trsno, (long)VB.Val(dt2.Rows[k]["BaseAmt"].ToString().Trim())) + ")";
                    //                }

                    //                ssView_Sheet1.Cells[nRow - 1, 4].Text = strBaseAmt;
                    //                ssView_Sheet1.Cells[nRow - 1, 5].Text = strQty;
                    //                ssView_Sheet1.Cells[nRow - 1, 6].Text = strNal;
                    //                ssView_Sheet1.Cells[nRow - 1, 7].Text = strAMT1;
                    //                ssView_Sheet1.Cells[nRow - 1, 8].Text = strAMT2;
                    //                ssView_Sheet1.Cells[nRow - 1, 10].Text = dt2.Rows[k]["GbSpc1"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 11].Text = dt2.Rows[k]["GbNgt1"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 12].Text = dt2.Rows[k]["GbGisul1"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 13].Text = dt2.Rows[k]["GbSelf1"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 14].Text = dt2.Rows[k]["Sugbp1"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 20].Text = dt2.Rows[k]["BUN"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 21].Text = dt2.Rows[k]["Sugbs1"].ToString().Trim();

                    //                //선별급여색깔 표시
                    //                if (cf.READ_BAS_Sun_S(clsDB.DbCon, dt2.Rows[k]["SuNext1"].ToString().Trim()) == "6" || cf.READ_BAS_Sun_S(clsDB.DbCon, dt2.Rows[k]["SuNext1"].ToString().Trim()) == "7")
                    //                {
                    //                    ssView_Sheet1.Cells[nRow - 1, 21].BackColor = Color.FromArgb(169, 208, 142);
                    //                }
                    //                else
                    //                {
                    //                    ssView_Sheet1.Cells[nRow - 1, 21].BackColor = Color.FromArgb(255, 255, 255);
                    //                }
                    //                ssView_Sheet1.Cells[nRow - 1, 22].Text = dt2.Rows[k]["Nu1"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 23].Text = dt2.Rows[k]["SuNext1"].ToString().Trim();
                    //                ssView_Sheet1.Cells[nRow - 1, 27].Text = dt2.Rows[k]["GbChild1"].ToString().Trim();

                    //                //P항 표시관련
                    //                if (nAmt1 != 0 && string.Compare(dt2.Rows[k]["Nu1"].ToString().Trim(), "21") >= 0)
                    //                {
                    //                    if ((dt2.Rows[k]["SugbP1"].ToString().Trim() == "0" || dt2.Rows[k]["SugbP1"].ToString().Trim() == "2" ||
                    //                       dt2.Rows[k]["SugbP1"].ToString().Trim() == "9") && dt2.Rows[k]["SugbS1"].ToString().Trim() != "1")
                    //                    {
                    //                        ssView_Sheet1.Cells[nRow - 1, 27].BackColor = Color.FromArgb(255, 0, 0);
                    //                    }
                    //                }

                    //                nTAMT = nAmt1;

                    //                if (dt2.Rows[k]["Sugbs1"].ToString().Trim() == "6" || dt2.Rows[k]["Sugbs1"].ToString().Trim() == "7" || dt2.Rows[k]["Sugbs1"].ToString().Trim() == "8")
                    //                {
                    //                    nBonRate = Bon_Rate_100(dt2.Rows[k]["Sugbs1"].ToString().Trim());
                    //                }
                    //                else
                    //                {
                    //                    nBonRate = 0;
                    //                }

                    //                //전액본인부담
                    //                if (nBonRate > 0)
                    //                {
                    //                    nBAMT = (long)Math.Truncate(nAmt1 * (nBonRate / 100.0));
                    //                    ssView_Sheet1.Cells[nRow - 1, 30].Text = nBAMT.ToString("###,##0");
                    //                }
                    //                else
                    //                {
                    //                    //비급여
                    //                    if (dt2.Rows[k]["GbSelf1"].ToString().Trim() != "0")
                    //                    {
                    //                        nBonRate = Bon_Rate_100(dt2.Rows[k]["Sugbs1"].ToString().Trim());

                    //                        if (nBonRate > 0)
                    //                        {
                    //                            nBAMT = (long)Math.Truncate(nAmt1 * (nBonRate / 100.0));
                    //                        }
                    //                        else
                    //                        {
                    //                            nBAMT = nAmt1;
                    //                        }

                    //                        ssView_Sheet1.Cells[nRow - 1, 31].Text = nBAMT.ToString("###,##0");
                    //                    }
                    //                    else
                    //                    {
                    //                        //본인부담
                    //                        if (dt2.Rows[k]["BUN"].ToString().Trim() == "01" || dt2.Rows[k]["BUN"].ToString().Trim() == "02")
                    //                        {
                    //                            nBAMT = (long)Math.Truncate(nAmt1 * (clsPmpaType.IBR.Jin / 100.0));
                    //                        }
                    //                        else if (dt2.Rows[k]["BUN"].ToString().Trim() == "72" || dt2.Rows[k]["BUN"].ToString().Trim() == "73")
                    //                        {
                    //                            nBAMT = (long)Math.Truncate(nAmt1 * (clsPmpaType.IBR.CTMRI / 100.0));
                    //                        }
                    //                        else if (dt2.Rows[k]["BUN"].ToString().Trim() == "74")
                    //                        {
                    //                            nBAMT = (long)Math.Truncate(nAmt1 * (clsPmpaType.IBR.Food / 100.0));
                    //                        }
                    //                        else
                    //                        {
                    //                            nBAMT = (long)Math.Truncate(nAmt1 * (clsPmpaType.IBR.Bohum / 100.0));
                    //                        }

                    //                        ssView_Sheet1.Cells[nRow - 1, 28].Text = nBAMT.ToString("###,##0");

                    //                    }

                    //                }

                    //                ssView_Sheet1.Cells[nRow - 1, 29].Text = (nTAMT - nBAMT).ToString("###,##0");   //공단부담


                    //                //마취코드는 변경 안 됨
                    //                ssView_Sheet1.Cells[nRow - 1, 16, nRow - 1, 17].Locked = true;

                    //                #endregion
                    //            }

                    //            dt2.Dispose();
                    //            dt2 = null;
                    //        }

                    //        #endregion
                    //    }

                    //}
                    //else
                    //{
                    //    #region DATA_MOVE_SLIP1

                    //    nRow += 1;
                    //    if (ssView_Sheet1.RowCount < nRow)
                    //    {
                    //        ssView_Sheet1.RowCount = nRow;
                    //    }

                    //    nAmt1 = (long)VB.Val(dt.Rows[i]["Amtt1"].ToString().Trim());
                    //    nAmt2 = (long)VB.Val(dt.Rows[i]["Amtt2"].ToString().Trim());

                    //    strQty = VB.Val(dt.Rows[i]["Qty"].ToString().Trim()).ToString("##0.00");
                    //    strNal = VB.Val(dt.Rows[i]["Nalsu"].ToString().Trim()).ToString("##0");

                    //    if (FstrDrg == "D" && chkDrg.Checked == false)
                    //    {
                    //        if (strBun != "74" && strF != "Y" && str100 != "Y")
                    //        {
                    //            nAmt1 = 0;
                    //            strBaseAmt = "0";
                    //        }
                    //        else
                    //        {
                    //            strBaseAmt = VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("##,###,##0");
                    //        }

                    //        //날수가 0인것 금액 0
                    //        if (strNal == "0")
                    //        {
                    //            nAmt1 = 0;
                    //            nAmt2 = 0;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        strBaseAmt = VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("##,###,##0");
                    //    }

                    //    nStot1 += nAmt1;
                    //    nStot2 += nAmt2;
                    //    nGtot1 += nAmt1;
                    //    nGtot2 += nAmt2;


                    //    strAMT1 = nAmt1.ToString("###,###,##0");
                    //    strAMT2 = nAmt2.ToString("###,###,##0");

                    //    ssView_Sheet1.Cells[nRow - 1, 0].Text = strNujuk;
                    //    ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["Sucode"].ToString().Trim();
                    //    ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["Bcode"].ToString().Trim();
                    //    ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["SunameK"].ToString().Trim();

                    //    //상급병실 종류표시
                    //    if (dt.Rows[i]["BUN"].ToString().Trim() == "77")
                    //    {
                    //        ssView_Sheet1.Cells[nRow - 1, 3].Text +=
                    //            "(" + cisc.Chk_Senior_Ward(clsDB.DbCon, clsPmpaType.TIT.Ipdno, clsPmpaType.TIT.Trsno, (long)VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim())) + ")";
                    //    }

                    //    ssView_Sheet1.Cells[nRow - 1, 4].Text = strBaseAmt;
                    //    ssView_Sheet1.Cells[nRow - 1, 5].Text = strQty;
                    //    ssView_Sheet1.Cells[nRow - 1, 6].Text = strNal;
                    //    ssView_Sheet1.Cells[nRow - 1, 7].Text = strAMT1;
                    //    ssView_Sheet1.Cells[nRow - 1, 8].Text = strAMT2;
                    //    ssView_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["GbSpc"].ToString().Trim();
                    //    ssView_Sheet1.Cells[nRow - 1, 11].Text = dt.Rows[i]["GbNgt"].ToString().Trim();
                    //    ssView_Sheet1.Cells[nRow - 1, 12].Text = dt.Rows[i]["GbGisul"].ToString().Trim();
                    //    ssView_Sheet1.Cells[nRow - 1, 13].Text = dt.Rows[i]["GbSelf"].ToString().Trim();
                    //    ssView_Sheet1.Cells[nRow - 1, 14].Text = dt.Rows[i]["Sugbp"].ToString().Trim();
                    //    ssView_Sheet1.Cells[nRow - 1, 20].Text = dt.Rows[i]["BUN"].ToString().Trim();
                    //    ssView_Sheet1.Cells[nRow - 1, 21].Text = dt.Rows[i]["GBSugbs"].ToString().Trim();

                    //    //선별급여색깔 표시
                    //    if (cf.READ_BAS_Sun_S(clsDB.DbCon, dt.Rows[i]["SuNext"].ToString().Trim()) == "6" || cf.READ_BAS_Sun_S(clsDB.DbCon, dt.Rows[i]["SuNext"].ToString().Trim()) == "7")
                    //    {
                    //        ssView_Sheet1.Cells[nRow - 1, 21].BackColor = Color.FromArgb(169, 208, 142);
                    //    }
                    //    else
                    //    {
                    //        ssView_Sheet1.Cells[nRow - 1, 21].BackColor = Color.FromArgb(255, 255, 255);
                    //    }
                    //    ssView_Sheet1.Cells[nRow - 1, 22].Text = dt.Rows[i]["Nu"].ToString().Trim();
                    //    ssView_Sheet1.Cells[nRow - 1, 23].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                    //    ssView_Sheet1.Cells[nRow - 1, 27].Text = dt.Rows[i]["GbChild"].ToString().Trim();

                    //    //P항 표시관련
                    //    if (nAmt1 != 0 && string.Compare(dt.Rows[i]["Nu"].ToString().Trim(), "21") >= 0)
                    //    {
                    //        if ((dt.Rows[i]["SugbP"].ToString().Trim() == "0" || dt.Rows[i]["SugbP"].ToString().Trim() == "2" ||
                    //             dt.Rows[i]["SugbP"].ToString().Trim() == "9") && dt.Rows[i]["GBSugbs"].ToString().Trim() != "1")
                    //        {
                    //            ssView_Sheet1.Cells[nRow - 1, 27].BackColor = Color.FromArgb(255, 0, 0);
                    //        }
                    //    }

                    //    nTAMT = nAmt1;

                    //    if (dt.Rows[i]["GBSugbs"].ToString().Trim() == "6" || dt.Rows[i]["GBSugbs"].ToString().Trim() == "7" || dt.Rows[i]["GBSugbs"].ToString().Trim() == "8")
                    //    {
                    //        nBonRate = Bon_Rate_100(dt.Rows[i]["GBSugbs"].ToString().Trim());
                    //    }
                    //    else
                    //    {
                    //        nBonRate = 0;
                    //    }

                    //    //전액본인부담
                    //    if (nBonRate > 0)
                    //    {
                    //        nBAMT = (long)Math.Truncate(nAmt1 * (nBonRate / 100.0));
                    //        ssView_Sheet1.Cells[nRow - 1, 30].Text = nBAMT.ToString("###,##0");
                    //    }
                    //    else
                    //    {
                    //        //비급여
                    //        if (dt.Rows[i]["GbSelf"].ToString().Trim() != "0")
                    //        {
                    //            nBonRate = Bon_Rate_100(dt.Rows[i]["GBSugbs"].ToString().Trim());

                    //            if (nBonRate > 0)
                    //            {
                    //                nBAMT = (long)Math.Truncate(nAmt1 * (nBonRate / 100.0));
                    //            }
                    //            else
                    //            {
                    //                nBAMT = nAmt1;
                    //            }

                    //            ssView_Sheet1.Cells[nRow - 1, 31].Text = nBAMT.ToString("###,##0");
                    //        }
                    //        else
                    //        {
                    //            //본인부담
                    //            if (dt.Rows[i]["BUN"].ToString().Trim() == "01" || dt.Rows[i]["BUN"].ToString().Trim() == "02")
                    //            {
                    //                nBAMT = (long)Math.Truncate(nAmt1 * (clsPmpaType.IBR.Jin / 100.0));
                    //            }
                    //            else if (dt.Rows[i]["BUN"].ToString().Trim() == "72" || dt.Rows[i]["BUN"].ToString().Trim() == "73")
                    //            {
                    //                nBAMT = (long)Math.Truncate(nAmt1 * (clsPmpaType.IBR.CTMRI / 100.0));
                    //            }
                    //            else if (dt.Rows[i]["BUN"].ToString().Trim() == "74")
                    //            {
                    //                nBAMT = (long)Math.Truncate(nAmt1 * (clsPmpaType.IBR.Food / 100.0));
                    //            }
                    //            else
                    //            {
                    //                nBAMT = (long)Math.Truncate(nAmt1 * (clsPmpaType.IBR.Bohum / 100.0));
                    //            }

                    //            ssView_Sheet1.Cells[nRow - 1, 28].Text = nBAMT.ToString("###,##0");

                    //        }

                    //    }

                    //    ssView_Sheet1.Cells[nRow - 1, 29].Text = (nTAMT - nBAMT).ToString("###,##0");   //공단부담
                    //    #endregion
                    //} 
                    #endregion

                    strNujuk = "";
                }

                dt.Dispose();
                dt = null;

                if (FstrDrg == "D" && chkDrg.Checked == false)
                {
                    if (nGETcount > 0)
                    {
                        SUB_TOT_SLIP1(ref nRow, ref nStot1, ref nStot2, ISBR);
                    }

                    Display_Drg_Amt(ref nRow, ref nAmt1, ref nAmt2, ref nStot1, ref nStot2, ref nGtot1, ref nGtot2, ref strBaseAmt, ref strQty, ref strNal, ISBR);
                }

                if (nGETcount > 0)
                {
                    SUB_TOT_SLIP1(ref nRow, ref nStot1, ref nStot2, ISBR);

                    GRAND_TOT_SLIP1(ref nRow, ref nGtot1, ref nGtot2, ISBR);

                    btnPrint.Enabled = true;
                    ssView.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void Read_IPD_SLIP3()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nNu = 0;
            int nNuChk = 0;
            int nRow = 0;
            int nInitNo = 0;
            int nRead = 0;
            long nAmt1 = 0;
            long nAmt2 = 0;
            long nStot1 = 0;
            long nStot2 = 0;
            long nGtot1 = 0;
            long nGtot2 = 0;
            string strNujuk = "";
            string strBaseAmt = "";
            string strQty = "";
            string strNal = "";
            string strAMT1 = "";
            string strAMT2 = "";
            string strBun = "";
            string str100 = "";
            string strF = "";
            string strCFdate = "";
            string strBDate = "";

            ComFunc cf = new ComFunc();
            clsPmpaType.ISBR ISBR = new clsPmpaType.ISBR();
            clsIument cIUM = new clsIument();

            ssView_Sheet1.ColumnHeader.Cells[0, 1].Text = "발생일자";
            ssView_Sheet1.ColumnHeader.Cells[0, 2].Text = "수가코드";

            nInitNo = 66;

            nNuChk = 0;
            nStot1 = 0;
            nStot2 = 0;
            nGtot1 = 0;
            nGtot2 = 0;
            nRow = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(Bdate, 'yy-mm-dd') Bday, I.GBSugbs, b.Sugbp,";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(i.entdate, 'yyyy-mm-dd hh24:mi') entdate,TO_CHAR(i.entdate, 'yyyy-mm-dd') entdate2, i.Bun, Nu,";
                SQL = SQL + ComNum.VBLF + "        I.SuNext SUNEXT, Sucode, SunameK, BaseAmt,";
                SQL = SQL + ComNum.VBLF + "        Qty, Nal,nvl(gber,'0') gber, DRG100,SugbS,";
                SQL = SQL + ComNum.VBLF + "        DRGF, GbSpc, GbNgt,";
                SQL = SQL + ComNum.VBLF + "        GbGisul, GbSelf, GbChild,";
                SQL = SQL + ComNum.VBLF + "        Amt1, Amt2, Part";

                if (clsPmpaType.IMST.GbOldSlip == false)
                {
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP I, " + ComNum.DB_PMPA + "BAS_SUN B";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_OLD_SLIP I, " + ComNum.DB_PMPA + "BAS_SUN B";
                }

                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";

                if (chkAll.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                }

                SQL = SQL + ComNum.VBLF + "    AND Nu >= '" + strFnu + "'";
                SQL = SQL + ComNum.VBLF + "    AND Nu <= '" + strTnu + "'";
                //*------------( 96/01/31 기간 Check 추가 lyj)--------------------------*

                if (strSDate != "")
                {
                    SQL = SQL + ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strSDate + "','yyyy-mm-dd')   ";
                }

                if (strEdate != "")
                {
                    SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strEdate + "','yyyy-mm-dd')   ";
                }

                //*-------------------------------------------------------------------------*
                SQL = SQL + ComNum.VBLF + "    AND (b.WONCODE NOT IN('1118') OR b.WONCODE IS NULL )"; //간호행위제외
                SQL = SQL + ComNum.VBLF + "    AND I.Sunext = B.Sunext";

                //저가약제 제외코드 2011-04-09
                SQL = SQL + ComNum.VBLF + "   AND TRIM(i.SUNEXT) NOT IN ( SELECT CODE FROM ADMIN.BAS_BCODE WHERE GUBUN ='원무영수제외코드') ";

                SQL = SQL + ComNum.VBLF + "  ORDER BY  Nu, Bdate, Sucode, I.Sunext";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                nRead = dt.Rows.Count;

                nGETcount += nRead;

                for (i = 0; i < nRead; i++)
                {
                    strBun = dt.Rows[i]["Bun"].ToString().Trim();
                    str100 = dt.Rows[i]["DRG100"].ToString().Trim();
                    strF = dt.Rows[i]["DRGF"].ToString().Trim();

                    if (DRG.READ_DRG_100(clsDB.DbCon, dt.Rows[i]["Sucode"].ToString().Trim(), "", clsPmpaType.TIT.InDate) == "OK")
                    {
                        str100 = "Y";
                    }
                    else
                    {
                        str100 = "";
                    }
                    nNu = (int)VB.Val(dt.Rows[i]["Nu"].ToString().Trim());

                    if (nNuChk == 0)
                    {
                        nNuChk = nNu;
                        strCFdate = dt.Rows[i]["Bday"].ToString().Trim();
                        strBDate = dt.Rows[i]["Bday"].ToString().Trim();
                        strNujuk = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_누적행위명", nNu.ToString("00"));
                    }

                    if (nNu != nNuChk)
                    {
                        SUB_TOT_SLIP1(ref nRow, ref nStot1, ref nStot2, ISBR);

                        nNuChk = nNu;
                        strCFdate = dt.Rows[i]["Bday"].ToString().Trim();
                        strBDate = dt.Rows[i]["Bday"].ToString().Trim();
                        strNujuk = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_누적행위명", nNu.ToString("00"));
                    }

                    if (strCFdate != dt.Rows[i]["Bday"].ToString().Trim())
                    {
                        strCFdate = dt.Rows[i]["Bday"].ToString().Trim();
                        strBDate = dt.Rows[i]["Bday"].ToString().Trim();
                    }

                    #region DATA_MOVE_SLIP3

                    nRow += 1;
                    if (ssView_Sheet1.RowCount < nRow)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    nAmt1 = (long)VB.Val(dt.Rows[i]["Amt1"].ToString().Trim());
                    nAmt2 = (long)VB.Val(dt.Rows[i]["Amt2"].ToString().Trim());

                    strQty = VB.Val(dt.Rows[i]["Qty"].ToString().Trim()).ToString("##0.00");
                    strNal = VB.Val(dt.Rows[i]["Nal"].ToString().Trim()).ToString("##0");

                    if (FstrDrg == "D" && chkDrg.Checked == false)
                    {
                        if (strBun != "74" && strF != "Y" && str100 != "Y")
                        {
                            nAmt1 = 0;
                            strBaseAmt = "0";
                        }
                        else
                        {
                            strBaseAmt = VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("##,###,##0");
                        }

                        //날수가 0인것 금액 0
                        if (strNal == "0")
                        {
                            nAmt1 = 0;
                            nAmt2 = 0;
                        }
                    }
                    else
                    {
                        strBaseAmt = VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("##,###,##0");
                    }

                    nStot1 += nAmt1;
                    nStot2 += nAmt2;
                    nGtot1 += nAmt1;
                    nGtot2 += nAmt2;

                    strAMT1 = nAmt1.ToString("###,###,##0");
                    strAMT2 = nAmt2.ToString("###,###,##0");

                    ssView_Sheet1.Cells[nRow - 1, 0].Text = strNujuk;
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = strBDate;
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["Sucode"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["SunameK"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = strBaseAmt;
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = strQty;
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = strNal;
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = strAMT1;
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = strAMT2;
                    ssView_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["Part"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["Gber"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 11].Text = dt.Rows[i]["GbNgt"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 12].Text = dt.Rows[i]["GbGisul"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 13].Text = dt.Rows[i]["GbSelf"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 14].Text = dt.Rows[i]["Sugbp"].ToString().Trim();

                    //P항 표시관련
                    if (nAmt1 != 0 && string.Compare(dt.Rows[i]["Nu"].ToString().Trim(), "21") >= 0)
                    {
                        if ((dt.Rows[i]["SugbP"].ToString().Trim() == "0" || dt.Rows[i]["SugbP"].ToString().Trim() == "2" ||
                             dt.Rows[i]["SugbP"].ToString().Trim() == "9") && dt.Rows[i]["SugbS"].ToString().Trim() != "1")
                        {
                            ssView_Sheet1.Cells[nRow - 1, 14].BackColor = Color.FromArgb(255, 0, 0);
                        }
                    }

                    ssView_Sheet1.Cells[nRow - 1, 21].Text = dt.Rows[i]["GBSugbs"].ToString().Trim();

                    //선별급여색깔 표시
                    if (cf.READ_BAS_Sun_S(clsDB.DbCon, dt.Rows[i]["SuNext"].ToString().Trim()) == "6" || cf.READ_BAS_Sun_S(clsDB.DbCon, dt.Rows[i]["SuNext"].ToString().Trim()) == "7")
                    {
                        ssView_Sheet1.Cells[nRow - 1, 21].BackColor = Color.FromArgb(169, 208, 142);
                    }
                    else
                    {
                        ssView_Sheet1.Cells[nRow - 1, 21].BackColor = Color.FromArgb(255, 255, 255);
                    }

                    ssView_Sheet1.Cells[nRow - 1, 26].Text = dt.Rows[i]["EntDate"].ToString().Trim();

                    //2014-01-13 의뢰서 처리
                    if (string.Compare(FstrMinActDate, dt.Rows[i]["EntDate2"].ToString().Trim()) < 0)
                    {
                        ssView_Sheet1.Cells[nRow - 1, 26].ForeColor = Color.Blue;
                    }
                    else
                    {
                        ssView_Sheet1.Cells[nRow - 1, 26].ForeColor = Color.Black;
                    }

                    ssView_Sheet1.Cells[nRow - 1, 27].Text = dt.Rows[i]["GbChild"].ToString().Trim();

                    #endregion

                    strNujuk = "";
                    strBDate = "";
                }

                dt.Dispose();
                dt = null;

                if (FstrDrg == "D" && chkDrg.Checked == false)
                {
                    if (nGETcount > 0)
                    {
                        SUB_TOT_SLIP1(ref nRow, ref nStot1, ref nStot2, ISBR);
                    }

                    Display_Drg_Amt(ref nRow, ref nAmt1, ref nAmt2, ref nStot1, ref nStot2, ref nGtot1, ref nGtot2, ref strBaseAmt, ref strQty, ref strNal, ISBR);
                }

                if (nGETcount > 0)
                {
                    SUB_TOT_SLIP1(ref nRow, ref nStot1, ref nStot2, ISBR);

                    GRAND_TOT_SLIP1(ref nRow, ref nGtot1, ref nGtot2, ISBR);

                    btnPrint.Enabled = true;
                    ssView.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void Read_IPD_SLIP7()
        {
            int i = 0;
            DataTable dt = null;
            DataTable dt2 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nNu = 0;
            int nNuChk = 0;
            int k = 0;
            int nRow = 0;
            int nCNT2 = 0;
            int nInitNo = 0;
            int nRead = 0;
            long nAmt1 = 0;
            long nAmt2 = 0;
            long nStot1 = 0;
            long nStot2 = 0;
            long nGtot1 = 0;
            long nGtot2 = 0;
            string strNujuk = "";
            string strBaseAmt = "";
            string strQty = "";
            string strNal = "";
            string strAMT1 = "";
            string strAMT2 = "";
            string strBun = "";
            string str100 = "";
            string strF = "";

            ComFunc cf = new ComFunc();
            clsPmpaType.ISBR ISBR = new clsPmpaType.ISBR();
            clsIument cIUM = new clsIument();

            ssView_Sheet1.ColumnHeader.Cells[0, 1].Text = "수가코드";
            ssView_Sheet1.ColumnHeader.Cells[0, 2].Text = "표준코드";

            nInitNo = 66;

            nNuChk = 0;
            nStot1 = 0;
            nStot2 = 0;
            nGtot1 = 0;
            nGtot2 = 0;
            nRow = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Sucode, I.Sunext, SunameK, B.Bcode, BaseAmt, Qty,";
                SQL = SQL + ComNum.VBLF + "        GbSpc, GbNgt, GbGisul,";
                SQL = SQL + ComNum.VBLF + "        GbSelf, GbChild, Nu,";
                SQL = SQL + ComNum.VBLF + "        Bun, SUM(Nal) Nalsu, SUM(Amt1) Amtt1,";
                SQL = SQL + ComNum.VBLF + "        SUM(Amt2) Amtt2, DRG100, DRGF, GBSUGBS";

                if (clsPmpaType.IMST.GbOldSlip == false)
                {
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP I, " + ComNum.DB_PMPA + "BAS_SUN B";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_OLD_SLIP I, " + ComNum.DB_PMPA + "BAS_SUN B";
                }

                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";

                if (chkAll.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                }

                SQL = SQL + ComNum.VBLF + "    AND Nu >= '" + strFnu + "'";
                SQL = SQL + ComNum.VBLF + "    AND Nu <= '" + strTnu + "'";
                //*------------( 96/01/31 기간 Check 추가 lyj)--------------------------*

                if (strSDate != "")
                {
                    SQL = SQL + ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strSDate + "','yyyy-mm-dd')   ";
                }

                if (strEdate != "")
                {
                    SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strEdate + "','yyyy-mm-dd')   ";
                }

                //*-------------------------------------------------------------------------*
                SQL = SQL + ComNum.VBLF + "    AND (b.WONCODE NOT IN('1118') OR b.WONCODE IS NULL )"; //간호행위제외
                SQL = SQL + ComNum.VBLF + "    AND I.Sunext = B.Sunext";

                //저가약제 제외코드 2011-04-09
                SQL = SQL + ComNum.VBLF + "   AND TRIM(i.SUNEXT) NOT IN ( SELECT CODE FROM ADMIN.BAS_BCODE WHERE GUBUN ='원무영수제외코드') ";

                SQL = SQL + ComNum.VBLF + "  GROUP BY Sucode, I.Sunext, SunameK, B.Bcode, BaseAmt, Qty,";
                SQL = SQL + ComNum.VBLF + "           GbSpc, GbNgt, GbGisul, GbSelf, GbChild, Nu, Bun, DRG100, DRGF, GBSUGBS";

                SQL = SQL + ComNum.VBLF + "  ORDER BY  Nu, Bun, Sucode";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                nRead = dt.Rows.Count;

                nGETcount += nRead;

                for (i = 0; i < nRead; i++)
                {
                    strBun = dt.Rows[i]["Bun"].ToString().Trim();
                    str100 = dt.Rows[i]["DRG100"].ToString().Trim();
                    strF = dt.Rows[i]["DRGF"].ToString().Trim();

                    if (DRG.READ_DRG_100(clsDB.DbCon, dt.Rows[i]["Sucode"].ToString().Trim(), "", clsPmpaType.TIT.InDate) == "OK")
                    {
                        str100 = "Y";
                    }
                    else
                    {
                        str100 = "";
                    }

                    if (VB.Val(dt.Rows[i]["Nu"].ToString().Trim()) != 0)
                    {
                        nNu = (int)VB.Val(dt.Rows[i]["Nu"].ToString().Trim());
                    }

                    if (nNuChk == 0)
                    {
                        nNuChk = nNu;
                        strNujuk = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_누적행위명", nNu.ToString("00"));
                    }

                    if (nNu != nNuChk)
                    {
                        SUB_TOT_SLIP1(ref nRow, ref nStot1, ref nStot2, ISBR);

                        nNuChk = nNu;
                        strNujuk = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_누적행위명", nNu.ToString("00"));
                    }

                    //심사계 요청
                    if (nNu == 6)
                    {
                        //마취 합산분 풀어서 Display
                        #region DATA_MARCHI_MOVE_SLIP1

                        if (nInitNo != 66)
                        {
                            continue;
                        }

                        nInitNo = 88;

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT Sucode Sucode1, I.Sunext Sunext1, SunameK SunameK1,";
                        SQL = SQL + ComNum.VBLF + "        Hcode Hcode1, BaseAmt BaseAmt1, Qty Qty1, I.GBSugbs Sugbs1,";
                        SQL = SQL + ComNum.VBLF + "        GbSpc GbSpc1, GbNgt GbNgt1, GbGisul GbGisul1,";
                        SQL = SQL + ComNum.VBLF + "        GbSelf GbSelf1, GbChild GbChild1, Nu Nu1,";
                        SQL = SQL + ComNum.VBLF + "        Nal Nalsu1, Amt1 Amtt11, Amt2 Amtt21,";
                        SQL = SQL + ComNum.VBLF + "        BUN";
                        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP I,     ";
                        SQL = SQL + ComNum.VBLF + "       " + ComNum.DB_PMPA + "BAS_SUN B           ";
                        SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";

                        if (chkAll.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "  AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "  AND TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                        }

                        SQL = SQL + ComNum.VBLF + "    AND Nu = '06'";
                        SQL = SQL + ComNum.VBLF + "    AND I.Sunext = B.Sunext";
                        //*------------( 96/01/31 기간 Check 추가 lyj)--------------------------*
                        if (strSDate != "")
                        {
                            SQL = SQL + ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strSDate + "','yyyy-mm-dd')";
                        }
                        if (strEdate != "")
                        {
                            SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strEdate + "','yyyy-mm-dd')";
                        }
                        SQL = SQL + ComNum.VBLF + "  ORDER BY Nu, Sucode, I.Sunext";

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt2.Rows.Count == 0)
                        {
                            dt2.Dispose();
                            dt2 = null;
                        }
                        else
                        {
                            nCNT2 = dt2.Rows.Count;

                            for (k = 0; k < nCNT2; k++)
                            {
                                #region DATA_MOVE_SLIP1_2

                                nRow += 1;
                                if (ssView_Sheet1.RowCount < nRow)
                                {
                                    ssView_Sheet1.RowCount = nRow;
                                }

                                if (k > 0)
                                {
                                    strNujuk = "          ";
                                }

                                nAmt1 = (long)VB.Val(dt2.Rows[k]["Amtt11"].ToString().Trim());
                                nAmt2 = (long)VB.Val(dt2.Rows[k]["Amtt21"].ToString().Trim());

                                strQty = VB.Val(dt2.Rows[k]["Qty1"].ToString().Trim()).ToString("##0.00");
                                strNal = VB.Val(dt2.Rows[k]["Nalsu1"].ToString().Trim()).ToString("##0");

                                if (FstrDrg == "D" && chkDrg.Checked == false)
                                {
                                    if (strBun != "74" && strF != "Y" && str100 != "Y")
                                    {
                                        nAmt1 = 0;
                                        strBaseAmt = "0";
                                    }
                                    else
                                    {
                                        strBaseAmt = VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("##,###,##0");
                                    }

                                    //날수가 0인것 금액 0
                                    if (strNal == "0")
                                    {
                                        nAmt1 = 0;
                                        nAmt2 = 0;
                                    }
                                }
                                else
                                {
                                    strBaseAmt = VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("##,###,##0");
                                }

                                nStot1 += nAmt1;
                                nStot2 += nAmt2;
                                nGtot1 += nAmt1;
                                nGtot2 += nAmt2;


                                strAMT1 = nAmt1.ToString("###,###,##0");
                                strAMT2 = nAmt2.ToString("###,###,##0");

                                ssView_Sheet1.Cells[nRow - 1, 0].Text = strNujuk;
                                ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[k]["Sucode"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[k]["Bcode"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[k]["SunameK"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 4].Text = strBaseAmt;
                                ssView_Sheet1.Cells[nRow - 1, 5].Text = strQty;
                                ssView_Sheet1.Cells[nRow - 1, 6].Text = strNal;
                                ssView_Sheet1.Cells[nRow - 1, 7].Text = strAMT1;
                                ssView_Sheet1.Cells[nRow - 1, 8].Text = strAMT2;
                                ssView_Sheet1.Cells[nRow - 1, 10].Text = dt2.Rows[k]["GbSpc1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 11].Text = dt2.Rows[k]["GbNgt1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 12].Text = dt2.Rows[k]["GbGisul1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 13].Text = dt2.Rows[k]["GbSelf1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 20].Text = dt2.Rows[k]["BUN"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 21].Text = dt2.Rows[k]["Sugbs1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 22].Text = dt2.Rows[k]["Nu1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 23].Text = dt2.Rows[k]["SuNext1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 27].Text = dt2.Rows[k]["GbChild1"].ToString().Trim();

                                #endregion
                            }

                            dt2.Dispose();
                            dt2 = null;
                        }

                        #endregion
                    }
                    else
                    {
                        #region DATA_MOVE_SLIP1

                        nRow += 1;
                        if (ssView_Sheet1.RowCount < nRow)
                        {
                            ssView_Sheet1.RowCount = nRow;
                        }

                        nAmt1 = (long)VB.Val(dt.Rows[i]["Amtt1"].ToString().Trim());
                        nAmt2 = (long)VB.Val(dt.Rows[i]["Amtt2"].ToString().Trim());

                        strQty = VB.Val(dt.Rows[i]["Qty"].ToString().Trim()).ToString("##0.00");
                        strNal = VB.Val(dt.Rows[i]["Nalsu"].ToString().Trim()).ToString("##0");

                        if (FstrDrg == "D" && chkDrg.Checked == false)
                        {
                            if (strBun != "74" && strF != "Y" && str100 != "Y")
                            {
                                nAmt1 = 0;
                                strBaseAmt = "0";
                            }
                            else
                            {
                                strBaseAmt = VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("##,###,##0");
                            }

                            //날수가 0인것 금액 0
                            if (strNal == "0")
                            {
                                nAmt1 = 0;
                                nAmt2 = 0;
                            }
                        }
                        else
                        {
                            strBaseAmt = VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("##,###,##0");
                        }

                        nStot1 += nAmt1;
                        nStot2 += nAmt2;
                        nGtot1 += nAmt1;
                        nGtot2 += nAmt2;


                        strAMT1 = nAmt1.ToString("###,###,##0");
                        strAMT2 = nAmt2.ToString("###,###,##0");

                        ssView_Sheet1.Cells[nRow - 1, 0].Text = strNujuk;
                        ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["Sucode"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["Bcode"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["SunameK"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 4].Text = strBaseAmt;
                        ssView_Sheet1.Cells[nRow - 1, 5].Text = strQty;
                        ssView_Sheet1.Cells[nRow - 1, 6].Text = strNal;
                        ssView_Sheet1.Cells[nRow - 1, 7].Text = strAMT1;
                        ssView_Sheet1.Cells[nRow - 1, 8].Text = strAMT2;
                        ssView_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["GbSpc"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 11].Text = dt.Rows[i]["GbNgt"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 12].Text = dt.Rows[i]["GbGisul"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 13].Text = dt.Rows[i]["GbSelf"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 20].Text = dt.Rows[i]["BUN"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 21].Text = dt.Rows[i]["GBSugbs"].ToString().Trim();

                        //선별급여색깔 표시
                        if (cf.READ_BAS_Sun_S(clsDB.DbCon, dt.Rows[i]["SuNext"].ToString().Trim()) == "6" || cf.READ_BAS_Sun_S(clsDB.DbCon, dt.Rows[i]["SuNext"].ToString().Trim()) == "7")
                        {
                            ssView_Sheet1.Cells[nRow - 1, 21].BackColor = Color.FromArgb(169, 208, 142);
                        }
                        else
                        {
                            ssView_Sheet1.Cells[nRow - 1, 21].BackColor = Color.FromArgb(255, 255, 255);
                        }
                        ssView_Sheet1.Cells[nRow - 1, 22].Text = dt.Rows[i]["Nu"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 23].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 27].Text = dt.Rows[i]["GbChild"].ToString().Trim();

                        #endregion
                    }

                    strNujuk = "";
                }

                dt.Dispose();
                dt = null;

                if (FstrDrg == "D" && chkDrg.Checked == false)
                {
                    if (nGETcount > 0)
                    {
                        SUB_TOT_SLIP1(ref nRow, ref nStot1, ref nStot2, ISBR);
                    }

                    Display_Drg_Amt(ref nRow, ref nAmt1, ref nAmt2, ref nStot1, ref nStot2, ref nGtot1, ref nGtot2, ref strBaseAmt, ref strQty, ref strNal, ISBR);
                }

                if (nGETcount > 0)
                {
                    SUB_TOT_SLIP1(ref nRow, ref nStot1, ref nStot2, ISBR);

                    GRAND_TOT_SLIP1(ref nRow, ref nGtot1, ref nGtot2, ISBR);

                    btnPrint.Enabled = true;
                    ssView.Enabled = true;
                }

                //수동명세서 사유 SELECT
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT REMARK";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_SU_SLIP ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND PANO = '" + clsPmpaType.TIT.Pano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                SQL = SQL + ComNum.VBLF + "   AND TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                SQL = SQL + ComNum.VBLF + "   AND REMARK IS NOT NULL ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    txtRemark.Text = dt.Rows[0]["REMARK"].ToString().Trim();
                }
                else
                {
                    txtRemark.Text = "";
                }

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void SUB_TOT_SLIP1(ref int nRow, ref long nStot1, ref long nStot2, clsPmpaType.ISBR ISBR)
        {
            nRow += 1;
            if (ssView_Sheet1.RowCount < nRow)
            {
                ssView_Sheet1.RowCount = nRow;
            }

            ssView_Sheet1.Rows[nRow - 1].BackColor = Color.FromArgb(128, 255, 255);

            ssView_Sheet1.Cells[nRow - 1, 1].Text = "누적별계";
            ssView_Sheet1.Cells[nRow - 1, 7].Text = nStot1.ToString("###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 8].Text = nStot2.ToString("###,###,##0");

            nStot1 = 0;
            nStot2 = 0;

            ssView_Sheet1.Cells[nRow - 1, 28].Text = ISBR.nBBAmt[1].ToString("###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 29].Text = ISBR.nBGAmt[1].ToString("###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 30].Text = ISBR.nBJAmt[1].ToString("###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 31].Text = ISBR.nBFAmt[1].ToString("###,###,##0");

            ISBR.nBBAmt[2] += ISBR.nBBAmt[1];
            ISBR.nBGAmt[2] += ISBR.nBGAmt[1];
            ISBR.nBJAmt[2] += ISBR.nBJAmt[1];
            ISBR.nBFAmt[2] += ISBR.nBFAmt[1];

            ISBR.nBBAmt[1] = 0;
            ISBR.nBGAmt[1] = 0;
            ISBR.nBJAmt[1] = 0;
            ISBR.nBFAmt[1] = 0;
        }

        void GRAND_TOT_SLIP1(ref int nRow, ref long nGtot1, ref long nGtot2, clsPmpaType.ISBR ISBR)
        {
            nRow += 1;
            if (ssView_Sheet1.RowCount < nRow)
            {
                ssView_Sheet1.RowCount = nRow;
            }

            ssView_Sheet1.Rows[nRow - 1].BackColor = Color.FromArgb(128, 255, 128);

            ssView_Sheet1.Cells[nRow - 1, 1].Text = "전체합계";
            ssView_Sheet1.Cells[nRow - 1, 7].Text = nGtot1.ToString("###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 8].Text = nGtot2.ToString("###,###,##0");

            ssView_Sheet1.Cells[nRow - 1, 28].Text = ISBR.nBBAmt[2].ToString("###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 29].Text = ISBR.nBGAmt[2].ToString("###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 30].Text = ISBR.nBJAmt[2].ToString("###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 31].Text = ISBR.nBFAmt[2].ToString("###,###,##0");

            nGtot1 = 0;
            nGtot2 = 0;
        }

        void Display_Drg_Amt(ref int nRow, ref long nAmt1, ref long nAmt2, ref long nStot1, ref long nStot2, ref long nGtot1, ref long nGtot2,
                             ref string strBaseAmt, ref string strQty, ref string strNal, clsPmpaType.ISBR ISBR)
        {
            long nBAMT = 0, nGAMT = 0;

            nRow += 1;
            if (ssView_Sheet1.RowCount < nRow)
            {
                ssView_Sheet1.RowCount = nRow;
            }

            if (clsPmpaType.TIT.Amt[70] > 0 && 1==2)
            {
                nAmt1 = clsPmpaType.TIT.Amt[70] - clsPmpaType.TIT.Amt[62];
                nBAMT = nAmt1 - clsPmpaType.TIT.Amt[68];
                nGAMT = clsPmpaType.TIT.Amt[68];
            }
            else
            {
                nAmt1 = DRG.GnDRG_Amt2;
                //nBAMT = DRG.GnDrg열외군금액_Bon + DRG.GnGs80Amt_B + DRG.GnGs50Amt_B + DRG.GnGs90Amt_B + DRG.GnDrg간호간병료_Bon + DRG.GnDrgSono_Bon + DRG.Gn재왕절개수가_Bon;
                //nBAMT += DRG.Gn응급가산수가_Bon + DRG.GnDrg부수술총액_Bon + (long)Math.Truncate(DRG.GnGsAddAmt * (clsPmpaType.IBR.Bohum / 100.0));
                //nBAMT += DRG.GnDrgJinSAmt_Bon + DRG.GnDrgJinAmt_Bon + DRG.GnDrg추가입원료_Bon + DRG.GnDRG_WBonAmt + DRG.GnDrgRoomAmt[0];
                nBAMT = DRG.GnDrg열외군금액_Bon + DRG.GnDrgBonAmt + DRG.GnGs80Amt_B + DRG.GnGs50Amt_B + DRG.GnGs90Amt_B  + DRG.GnDrgRoomAmt[0]+ DRG.GnDrgFoodAmt[0];//+ DRG.GnGs100Amt
                nGAMT = DRG.GnDrgJohapAmt + DRG.GnGs80Amt_J + DRG.GnGs50Amt_J + DRG.GnGs90Amt_J + DRG.GnDrgRoomAmt[1] + DRG.GnDrgFoodAmt[1];
            }
            
            nAmt2 = 0;

            nStot1 += nAmt1;
            nStot2 += nAmt2;
            nGtot1 += nAmt1;
            nGtot2 += nAmt2;

            strBaseAmt = nAmt1.ToString("###,###,##0");
            strQty = "1.00";
            strNal = "0";
            
            ssView_Sheet1.Cells[nRow - 1, 0].Text = "포괄수가진료비";
            ssView_Sheet1.Cells[nRow - 1, 1].Text = clsPmpaType.TIT.DrgCode;
            ssView_Sheet1.Cells[nRow - 1, 2].Text = "";
            ssView_Sheet1.Cells[nRow - 1, 3].Text = DRG.READ_DRG_CODE_NAME(clsDB.DbCon, clsPmpaType.TIT.DrgCode);
            ssView_Sheet1.Cells[nRow - 1, 4].Text = strBaseAmt;
            ssView_Sheet1.Cells[nRow - 1, 5].Text = strQty;
            ssView_Sheet1.Cells[nRow - 1, 6].Text = strNal;
            ssView_Sheet1.Cells[nRow - 1, 7].Text = nAmt1.ToString("###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 8].Text = nAmt2.ToString("###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 10].Text = clsPmpaType.TIT.GbSpc;
            ssView_Sheet1.Cells[nRow - 1, 11].Text = "";
            ssView_Sheet1.Cells[nRow - 1, 12].Text = "";
            ssView_Sheet1.Cells[nRow - 1, 13].Text = "";
            ssView_Sheet1.Cells[nRow - 1, 14].Text = "";
            ssView_Sheet1.Cells[nRow - 1, 20].Text = "";
            ssView_Sheet1.Cells[nRow - 1, 21].Text = "";
            ssView_Sheet1.Cells[nRow - 1, 22].Text = "";
            ssView_Sheet1.Cells[nRow - 1, 23].Text = "";

            ssView_Sheet1.Cells[nRow - 1, 28].Text = nBAMT.ToString("###,###,##0"); 
            ssView_Sheet1.Cells[nRow - 1, 29].Text = nGAMT.ToString("###,###,##0");
            ssView_Sheet1.Cells[nRow - 1, 30].Text = "";
            ssView_Sheet1.Cells[nRow - 1, 31].Text = "";

            ISBR.nBBAmt[1] = nBAMT;
            ISBR.nBGAmt[1] = nGAMT;

        }

        
    }
}
