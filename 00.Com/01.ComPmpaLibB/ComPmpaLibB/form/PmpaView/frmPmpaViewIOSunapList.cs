using System;
using System.Data;
using System.Windows.Forms;
using ComLibB;
using FarPoint.Win.Spread;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewIOSunapList.cs
    /// Description     : 입원 / 외래 진료비 수납 내역 조회
    /// Author          : 안정수
    /// Create Date     : 2017-10-06
    /// Update History  : 
    /// <history>       
    /// d:\psmh\OPD\jengsan\Frm진료비수납내역서_2012.frm(Frm진료비수납내역서_2012) => frmPmpaViewIOSunapList.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\jengsan\Frm진료비수납내역서_2012.frm(Frm진료비수납내역서_2012)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewIOSunapList : Form, MainFormMessage
    {
        private frmPmpaViewSname frmPmpaViewSnameX = null;

        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();
        clsPmpaPrint CPP = new clsPmpaPrint();
        clsPmpaFunc CPF = new clsPmpaFunc();
        clsIument CIT = new clsIument();
        clsBasAcct CBA = new clsBasAcct();


        string FstrFlag = "";       // 정해진 프린트가 없을경우
        string FstrFlag1 = "";      // 진료기간 선택시
        string FstrSimChk = "";     // 심사과 여부
        string[] FstrROWID = new string[51];

        string mstrRetValue = "";
        string mstrJobName = "";

        int mnJobSabun = 0;

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

        public frmPmpaViewIOSunapList()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewIOSunapList(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();

        }

        public frmPmpaViewIOSunapList(int GnJobSabun)
        {
            InitializeComponent();
            setEvent();
            mnJobSabun = GnJobSabun;
        }

        public frmPmpaViewIOSunapList(string GstrJobName)
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
            this.btnSim.Click += new EventHandler(eBtnEvent);

            this.txtPano.KeyPress += new KeyPressEventHandler(eControl_KeyPress);

            this.txtCnt.GotFocus += new EventHandler(eControl_GotFocus);


        }

        void ssList1_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            int i = 0;
            double nAmt = 0;

            if (e.ColumnHeader == false) return;
            if (e.Column == 0)
            {
                for (i = 0; i < ssList1_Sheet1.RowCount - 1; i++)
                {
                    nAmt = 0;

                    nAmt = VB.Val(ssList1_Sheet1.Cells[i, 66].Text);

                    if (nAmt != 0 || chkbon.Checked == true)
                    {
                        ssList1_Sheet1.Cells[i, 0].Text = "1";
                    }
                }
            }

        }

        void eControl_GotFocus(object sender, EventArgs e)
        {
            txtCnt.SelectionStart = 0;
            txtCnt.SelectionLength = txtCnt.Text.Length;
        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtPano)
            {
                if (e.KeyChar == 13)
                {
                    if (optBun1.Checked == true)
                    {
                        if (String.Compare(VB.Left(txtPano.Text.Trim(), 1), "ㄱ") < 0)
                        {
                            return;
                        }

                        clsPmpaPb.GstrName = txtPano.Text.Trim();
                        clsPmpaPb.GstrFal = "1";

                        clsPmpaPb.GstrView1 = "1^^";
                        clsPmpaPb.GnStart = 1;
                        clsPmpaPb.GstrView1 += clsPmpaPb.GstrName + "^^";


                        frmPmpaViewSnameX = new frmPmpaViewSname(clsPmpaPb.GstrView1);
                        frmPmpaViewSnameX.rSendText += new frmPmpaViewSname.SendText(GetText);
                        frmPmpaViewSnameX.rEventExit += new frmPmpaViewSname.EventExit(frmPmpaViewSnameX_rEventExit);
                        frmPmpaViewSnameX.ShowDialog(this);

                        if (CPF.READ_BAS_PATIENT2(clsDB.DbCon, txtPano.Text, "1") == "NO")
                        {
                            ComFunc.MsgBox("[" + txtPano.Text + "]해당하는 환자는 없습니다.");
                            txtPano.Text = "";
                        }

                        txtSName.Text = clsPmpaType.TBP.Sname;
                        btnView.Enabled = true;
                        eGetData();
                    }

                    else
                    {
                        txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);
                        if (CPF.READ_BAS_PATIENT2(clsDB.DbCon, txtPano.Text, "1") == "NO")
                        {
                            ComFunc.MsgBox("[" + txtPano.Text + "]해당하는 환자는 없습니다.");
                            txtPano.Text = "";
                        }

                        txtSName.Text = clsPmpaType.TBP.Sname;
                        SendKeys.Send("{TAB}");
                        btnView.Enabled = true;
                    }
                }
            }
        }

        void GetText(string ArgROWID)
        {
            CPF.READ_BAS_PATIENT2(clsDB.DbCon, ArgROWID, "2");
            txtPano.Text = clsPmpaType.TBP.Ptno;
        }

        void frmPmpaViewSnameX_rEventExit()
        {
            frmPmpaViewSnameX.Dispose();
            frmPmpaViewSnameX = null;
        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등                  

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            optIO1.Checked = true;
            optBun1.Checked = true;

            ssJin.Visible = false;
            ssList2.Visible = false;

            string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");

            SCREEN_CLEAR();

            //프린터 셋팅
            CPP.Printer_Setting();

            FstrSimChk = "";

            btnSim.Visible = false;

            if (clsType.User.Sabun == "4349" || clsType.User.Sabun == "19399" || clsType.User.Sabun == "468")
            {
                btnSim.Visible = true;
            }

            for (int i = 80; i < 100; i++)
            {
                ssList1_Sheet1.Columns[i].Visible = false;
            }

            dtpFDate.Text = VB.Left(CurrentDate, 4) + "-01-01";

            Set_Combo();

            txtCnt.Text = "1";

            clsPmpaPb.GstrMirFlag = "";
            ComFunc.ReadSysDate(clsDB.DbCon);

            CBA.Bas_Opd_Bon();              //외래본인부담율
            CBA.Bas_Ipd_Bon();              //입원본인부담율
            CBA.Bas_Joje();                 //내복약조제료 일수
            CBA.Bas_Gisul();                //병원기술료가산
            CBA.Bas_Night();                //심야가산
            CBA.Bas_Night_22();             //중복가산(신생아,소아,노인등)
            CBA.Bas_Gamek();                //감액율(진찰료, 보험, 일반, 보험100%)

            CBA.Bas_PedAdd();               //만6세미만

        }

        void SCREEN_CLEAR()
        {
            txtPano.Text = "";
            txtSName.Text = "";

            cboBi.Items.Clear();
            cboDept.Items.Clear();

            CS.Spread_All_Clear(ssList1);
            //btnView.Enabled = false;
        }

        //진료비 수납내역서용
        //--------------------------------------------
        void Set_Combo()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            int i = 0;

            cboDept.Items.Clear();
            cboDept.Items.Add("전체");

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                ";
            SQL += ComNum.VBLF + " DEPTCODE                                                             ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT                             ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                             ";
            SQL += ComNum.VBLF + "      AND DEPTCODE NOT IN ('II','R6','TO','HR','PT','MD')   ";
            SQL += ComNum.VBLF + "ORDER BY PRINTRANKING                                                 ";

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
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboDept.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim());

                    }

                    cboDept.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            cboBi.Items.Clear();
            cboBi.Items.Add("전체");

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                ";
            SQL += ComNum.VBLF + "  Code                                ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BUN    ";
            SQL += ComNum.VBLF + "WHERE 1=1                             ";
            SQL += ComNum.VBLF + "AND Jong = '5'                        ";  //환자종류
            SQL += ComNum.VBLF + "ORDER BY Code                         ";

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
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboBi.Items.Add(dt.Rows[i]["Code"].ToString().Trim());
                    }

                    cboBi.SelectedIndex = 0;
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
                return;
            }

            else if (sender == this.btnView)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();

            }

            else if (sender == this.btnPrint)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ePrint();
            }

            else if (sender == this.btnSim)
            {
                btnSim_Click();
            }
        }

        void btnSim_Click()
        {
            FstrSimChk = "Y";

            //Define cells as type EDIT
            //SS1.CellType = SS_CELL_TYPE_EDIT
            //SS1.TypeTextOrient = SS_CELL_TEXTORIENT_HORIZONTAL
            //SS1.TypeEllipses = False
            //SS1.TypeHAlign = TypeHAlignRight
            //SS1.TypeVAlign = SS_CELL_V_ALIGN_VCENTER
            //SS1.TypeEditCharSet = SS_CELL_EDIT_CHAR_SET_ASCII
            //SS1.TypeEditCharCase = SS_CELL_EDIT_CASE_NO_CASE
            //SS1.TypeMaxEditLen = 300

            //ssList1_Sheet1.Cells[0, 0, ssList1_Sheet1.Rows.Count - 1, ssList1_Sheet1.Columns.Count - 1].Locked = false;

            ssList2_Sheet1.Cells[23, 1].Text = "<참고사항";

            chkManual.Visible = true;

            optBun0.Checked = true;

            ComFunc.MsgBox("심사과 전용 화면입니다..!!");
        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();

            bool PrePrint = chkPrePrint.Checked;

            int i = 0;
            int j = 0;

            //인쇄용 ssList2 스프레드

            string strIO = "";

            double nBAmt = 0;
            int nJCnt = 0;
            double[] nAmt = new double[8];
            double[] nTotAmt = new double[8];

            strIO = (optIO0.Checked == true) ? "입원" : "외래";

            if (mnJobSabun != 4349)
            {
                clsPublic.GstrRetValue = txtPano.Text + "^^" + "외래^^" + dtpFDate.Text + "~" + dtpTDate.Text + "^^";
                frmSetPrintInfo frmSetPrintInfoX = new frmSetPrintInfo();
                frmSetPrintInfoX.StartPosition = FormStartPosition.CenterParent;
                frmSetPrintInfoX.ShowDialog(this);
            }

            for (i = 0; i < nAmt.Length; i++)
            {
                nAmt[i] = 0;
                nTotAmt[i] = 0;
            }

            nBAmt = 0;
            nJCnt = 0;
             
            if (FstrSimChk == "Y")
            {
                ssList2_Sheet1.Rows[24].Visible = true;
                ssList2_Sheet1.Rows[25].Visible = true;
                ssList2_Sheet1.Rows[26].Visible = true;
                ssList2_Sheet1.Rows[27].Visible = true;

                //ssList2_Sheet1.Rows[31].Height = 10;
                //ssList2_Sheet1.Rows[33].Height = 10;

                //ssList2_Sheet1.Rows[34].Height = 15;
                //ssList2_Sheet1.Rows[36].Height = 15;
            }

            else
            {
                ssList2_Sheet1.Rows[24].Visible = false;
                ssList2_Sheet1.Rows[25].Visible = false;
                ssList2_Sheet1.Rows[26].Visible = false;
                ssList2_Sheet1.Rows[27].Visible = false;

                //ssList2_Sheet1.Rows[31].Height = 20;
                //ssList2_Sheet1.Rows[33].Height = 20;

                //ssList2_Sheet1.Rows[34].Height = 25;
                //ssList2_Sheet1.Rows[36].Height = 25;
            }
            
            //종류선택
            if (chkJinPrt.Checked == true)
            {
                //2. 진료수납확인 - clear
                Screen_clear_ssJin();
                Read_infoSet_ssjint();

                for (i = 0; i < ssList1_Sheet1.Rows.Count; i++)
                {
                    if (ssList1_Sheet1.Cells[i, 0].Text == "True")
                    {
                        nBAmt = VB.Val(ssList1_Sheet1.Cells[i, 66].Text);

                        if ((nBAmt != 0 || chkbon.Checked == true) && (chkAll.Checked == false && ssList1_Sheet1.Cells[i, 3].Text != "전체과")
                                                                   || (chkAll.Checked == true && ssList1_Sheet1.Cells[i, 3].Text == "전체과"))
                        {
                            //전체금액
                            nAmt[1] = VB.Val(ssList1_Sheet1.Cells[i, 65].Text);
                            nTotAmt[1] += nAmt[1];
                            //공단부담-급
                            nAmt[2] = VB.Val(ssList1_Sheet1.Cells[i, 64].Text);
                            nTotAmt[2] += nAmt[2];
                            //본인부담-급
                            nAmt[3] = VB.Val(ssList1_Sheet1.Cells[i, 63].Text);
                            nTotAmt[3] += nAmt[3];
                            //비급여
                            nAmt[4] = VB.Val(ssList1_Sheet1.Cells[i, 62].Text);
                            nTotAmt[4] += nAmt[4];
                            //환자부담->본인급+비급
                            nAmt[5] = VB.Val(ssList1_Sheet1.Cells[i, 69].Text);
                            nTotAmt[5] += nAmt[5];
                            //감액
                            nAmt[6] = VB.Val(ssList1_Sheet1.Cells[i, 67].Text);
                            nTotAmt[6] += nAmt[6];

                            nJCnt += 1;

                            if (chkAll.Checked == true)
                            {
                                ssJin_Sheet1.Cells[(nJCnt - 1) + 9, 1].Text = dtpFDate.Text + "~" + dtpTDate.Text;
                            }
                            else if (chkView.Checked == true && optIO0.Checked == false)
                            {
                                ssJin_Sheet1.Cells[(nJCnt - 1) + 9, 1].Text = VB.Pstr(ssList1_Sheet1.Cells[i, 4].Text, " ", 1);
                            }
                            else
                            {
                                ssJin_Sheet1.Cells[(nJCnt - 1) + 9, 1].Text = ssList1_Sheet1.Cells[i, 4].Text.Replace(" ", "~");
                            }

                            ssJin_Sheet1.Cells[(nJCnt - 1) + 9, 2].Text = ssList1_Sheet1.Cells[i, 7].Text;

                            if (optIO1.Checked == true)
                            {
                                ssJin_Sheet1.Cells[(nJCnt - 1) + 9, 3].Text = "외래";
                            }

                            else if (optIO0.Checked == true)
                            {
                                ssJin_Sheet1.Cells[(nJCnt - 1) + 9, 3].Text = "입원";
                            }

                            ssJin_Sheet1.Cells[(nJCnt - 1) + 9, 4].Text = ssList1_Sheet1.Cells[i, 3].Text;
                            ssJin_Sheet1.Columns[4].Width = ssJin_Sheet1.Columns[4].GetPreferredWidth() + 4;

                            ssJin_Sheet1.Cells[(nJCnt - 1) + 9, 5].Text = string.Format("{0:###,###,###,##0}", nAmt[1]);
                            ssJin_Sheet1.Cells[(nJCnt - 1) + 9, 6].Text = string.Format("{0:###,###,###,##0}", nAmt[2]);
                            ssJin_Sheet1.Cells[(nJCnt - 1) + 9, 7].Text = string.Format("{0:###,###,###,##0}", nAmt[3]);
                            ssJin_Sheet1.Cells[(nJCnt - 1) + 9, 8].Text = string.Format("{0:###,###,###,##0}", nAmt[4]);
                            ssJin_Sheet1.Cells[(nJCnt - 1) + 9, 9].Text = string.Format("{0:###,###,###,##0}", nAmt[6]);
                            ssJin_Sheet1.Cells[(nJCnt - 1) + 9, 10].Text = string.Format("{0:###,###,###,##0}", nAmt[5]);

                            if (nJCnt >= 20)
                            {
                                ssJin_Sheet1.Cells[29, 1].Text = "합계";

                                ssJin_Sheet1.Cells[29, 5].Text = string.Format("{0:###,###,###,##0}", nTotAmt[1]);
                                ssJin_Sheet1.Cells[29, 6].Text = string.Format("{0:###,###,###,##0}", nTotAmt[2]);
                                ssJin_Sheet1.Cells[29, 7].Text = string.Format("{0:###,###,###,##0}", nTotAmt[3]);
                                ssJin_Sheet1.Cells[29, 8].Text = string.Format("{0:###,###,###,##0}", nTotAmt[4]);
                                ssJin_Sheet1.Cells[29, 9].Text = string.Format("{0:###,###,###,##0}", nTotAmt[6]);
                                ssJin_Sheet1.Cells[29, 10].Text = string.Format("{0:###,###,###,##0}", nTotAmt[5]);

                                //strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

                                //strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                                //strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3) + "PAGE:" + "/p", new Font("굴림체", 11), clsSpread.enmSpdHAlign.Right, false, true);

                                //setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
                                //setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, false, false, false, false, false);
                                //ssJin_Sheet1.PrintInfo.UseMax = false;
                                //SPR.setSpdPrint(ssJin, PrePrint, setMargin, setOption, strHeader, strFooter);

                                ssJin_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
                                ssJin_Sheet1.PrintInfo.Margin.Top = 60;
                                ssJin_Sheet1.PrintInfo.Margin.Bottom = 20;
                                ssJin_Sheet1.PrintInfo.ShowColor = false;
                                ssJin_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
                                ssJin_Sheet1.PrintInfo.ShowBorder = false;
                                ssJin_Sheet1.PrintInfo.ShowGrid = false;
                                ssJin_Sheet1.PrintInfo.ShowShadows = false;
                                ssJin_Sheet1.PrintInfo.UseMax = false;
                                ssJin_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
                                ssJin_Sheet1.PrintInfo.UseSmartPrint = false;
                                ssJin_Sheet1.PrintInfo.ShowPrintDialog = false;
                                ssJin_Sheet1.PrintInfo.Preview = PrePrint;
                                ssJin.PrintSheet(0);
                                ComFunc.Delay(100);

                                Screen_clear_ssJin2();

                                nJCnt = 0;

                                for (j = 0; j < nAmt.Length; j++)
                                {
                                    nAmt[j] = 0;
                                    nTotAmt[j] = 0;
                                }

                            }
                        }

                        ssList1_Sheet1.Cells[i, 0].Text = "0";
                    }
                }

                if (nJCnt != 0)
                {
                    ssJin_Sheet1.Cells[29, 1].Text = "합계";

                    ssJin_Sheet1.Cells[29, 5].Text = string.Format("{0:###,###,###,##0}", nTotAmt[1]);
                    ssJin_Sheet1.Cells[29, 6].Text = string.Format("{0:###,###,###,##0}", nTotAmt[2]);
                    ssJin_Sheet1.Cells[29, 7].Text = string.Format("{0:###,###,###,##0}", nTotAmt[3]);
                    ssJin_Sheet1.Cells[29, 8].Text = string.Format("{0:###,###,###,##0}", nTotAmt[4]);
                    ssJin_Sheet1.Cells[29, 9].Text = string.Format("{0:###,###,###,##0}", nTotAmt[6]);
                    ssJin_Sheet1.Cells[29, 10].Text = string.Format("{0:###,###,###,##0}", nTotAmt[5]);

                    //strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

                    //strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                    //strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 11), clsSpread.enmSpdHAlign.Right, false, true);

                    //setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
                    //setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, false, false, false, false, false);
                    //ssJin_Sheet1.PrintInfo.UseMax = false;
                    //SPR.setSpdPrint(ssJin, PrePrint, setMargin, setOption, strHeader, strFooter);

                    ssJin_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
                    ssJin_Sheet1.PrintInfo.Margin.Top = 60;
                    ssJin_Sheet1.PrintInfo.Margin.Bottom = 20;
                    ssJin_Sheet1.PrintInfo.ShowColor = false;
                    ssJin_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
                    ssJin_Sheet1.PrintInfo.ShowBorder = false;
                    ssJin_Sheet1.PrintInfo.ShowGrid = false;
                    ssJin_Sheet1.PrintInfo.ShowShadows = false;
                    ssJin_Sheet1.PrintInfo.UseMax = false;
                    ssJin_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
                    ssJin_Sheet1.PrintInfo.UseSmartPrint = false;
                    ssJin_Sheet1.PrintInfo.ShowPrintDialog = false;
                    ssJin_Sheet1.PrintInfo.Preview = PrePrint;
                    ssJin.PrintSheet(0);
                    ComFunc.Delay(100);

                }

            }
            else
            {
                //If ChkPrePrint.Value = False Then
                //    '1.진료비 수납 내역서 세팅
                //    Call Print_Sheet
                //End If

                if (chkAll.Checked == true)
                {
                    FstrFlag1 = "";

                    for (j = 1; j <= VB.Val(txtCnt.Text); j++)
                    {
                        PRINT_ALL(ssList1_Sheet1.Rows.Count - 1);

                        //strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

                        //strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                        //strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 11), clsSpread.enmSpdHAlign.Right, false, true);

                        //setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
                        //setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, false, false, false, false, false);
                        //ssList2_Sheet1.PrintInfo.UseMax = false;
                        //SPR.setSpdPrint(ssList2, PrePrint, setMargin, setOption, strHeader, strFooter);


                        ssList2_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
                        ssList2_Sheet1.PrintInfo.Margin.Top = 60;
                        ssList2_Sheet1.PrintInfo.Margin.Bottom = 20;
                        ssList2_Sheet1.PrintInfo.ShowColor = false;
                        ssList2_Sheet1.PrintInfo.ShowRowHeader = PrintHeader.Hide;
                        ssList2_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
                        ssList2_Sheet1.PrintInfo.ShowBorder = false;
                        ssList2_Sheet1.PrintInfo.ShowGrid = false;
                        ssList2_Sheet1.PrintInfo.ShowShadows = false;
                        ssList2_Sheet1.PrintInfo.UseMax = false;
                        ssList2_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
                        ssList2_Sheet1.PrintInfo.UseSmartPrint = false;
                        ssList2_Sheet1.PrintInfo.ShowPrintDialog = false;
                        ssList2_Sheet1.PrintInfo.Preview = PrePrint;
                        ssList2.PrintSheet(0);
                        ComFunc.Delay(100);

                    }
                }
                else
                {
                    FstrFlag1 = "N";

                    for (i = 0; i < ssList1_Sheet1.Rows.Count; i++)
                    {
                        if (ssList1_Sheet1.Cells[i, 0].Text == "True" && chkAll.Checked == false)
                        {
                            //nDrgAmt가 어디서 쓰는지 알수 없음...
                            //SS1.Col = 75: nDrgAmt = Val(SS1.Text)
                            PRINT_ALL(i);

                            if (FstrFlag != "N")
                            {
                                for (j = 1; j <= VB.Val(txtCnt.Text); j++)
                                {
                                    //strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

                                    //strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                                    //strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 11), clsSpread.enmSpdHAlign.Right, false, true);

                                    //setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
                                    //setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, false, false, false, false, false);
                                    //ssList2_Sheet1.PrintInfo.UseMax = false;
                                    //SPR.setSpdPrint(ssList2, PrePrint, setMargin, setOption, strHeader, strFooter);

                                    ssList2_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
                                    ssList2_Sheet1.PrintInfo.Margin.Top = 60;
                                    ssList2_Sheet1.PrintInfo.Margin.Bottom = 20;
                                    ssList2_Sheet1.PrintInfo.ShowColor = false;
                                    ssList2_Sheet1.PrintInfo.ShowRowHeader = PrintHeader.Hide;
                                    ssList2_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
                                    ssList2_Sheet1.PrintInfo.ShowBorder = false;
                                    ssList2_Sheet1.PrintInfo.ShowGrid = false;
                                    ssList2_Sheet1.PrintInfo.ShowShadows = false;
                                    ssList2_Sheet1.PrintInfo.UseMax = false;
                                    ssList2_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
                                    ssList2_Sheet1.PrintInfo.UseSmartPrint = false;
                                    ssList2_Sheet1.PrintInfo.ShowPrintDialog = false;
                                    ssList2_Sheet1.PrintInfo.Preview = PrePrint;
                                    ssList2.PrintSheet(0);
                                    ComFunc.Delay(100);

                                    ssList1_Sheet1.Cells[i, 0].Text = "0";
                                }
                            }
                        }
                    }
                }


            }
            
        }

        void Screen_clear_ssJin()
        {
            ssJin_Sheet1.Cells[4, 1].Text = "";
            ssJin_Sheet1.Cells[4, 3].Text = "";
            ssJin_Sheet1.Cells[4, 6].Text = "";

            ssJin_Sheet1.Cells[9, 1, 29, 10].Text = "";
            ssJin_Sheet1.Cells[29, 1].Text = "합계";
        }

        void Read_infoSet_ssjint()
        {
            ssJin_Sheet1.Cells[4, 1].Text = txtPano.Text;
            ssJin_Sheet1.Cells[4, 3].Text = txtSName.Text;

            ssJin_Sheet1.Cells[4, 6].Text = ssList1_Sheet1.Cells[0, 6].Text;
        }

        void Screen_clear_ssJin2()
        {
            ssJin_Sheet1.Cells[9, 1, 29, 10].Text = "";
            ssJin_Sheet1.Cells[29, 1].Text = "합계";
        }

        void PRINT_ALL(int argROW)
        {
            //double nAmt1 = 0;     //수납금액
            double nAmt2 = 0;     //희귀난치금액
            double nAmt3 = 0;     //선택진료금액
            double nAmt4 = 0;     //물리바우처

            double nDrgAmt = 0;   //DRG금액

            //double nTempAmt1 = 0; //공단
            //double nTempAmt2 = 0; //본인
            //double nTempAmt3 = 0; //비급

            string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");

            //Const PAPER_LEFT = 5
            //Const PAPER_TOP = 10
            //Const PAPER_WIDTH = 190
            //Const OPIN_MAX = 42
            //Const PRINT_FONT_SIZE = 12
            //Const PRINT_FONT_NAME = "굴림체"            'init 프린터

            //Printer.ScaleMode = vbMillimeters

            //Printer.Font.Size = PRINT_FONT_SIZE
            //Printer.Font.Name = "바탕체"
            //Printer.DrawMode = vbMaskPen

            if (optIO0.Checked == true)
            {
                ssList2_Sheet1.Cells[1, 3].Text = "입　원";
            }

            if (optIO1.Checked == true)
            {
                ssList2_Sheet1.Cells[1, 3].Text = "외　래";
            }

            nAmt2 = 0;
            nAmt3 = 0;
            nAmt4 = 0;

            //희귀난치지원금 입원
            nAmt2 = VB.Val(ssList1_Sheet1.Cells[argROW, 70].Text);
            //물리치료 바우처  2012-02-14
            nAmt4 = VB.Val(ssList1_Sheet1.Cells[argROW, 72].Text);

            //DRG금액
            nDrgAmt = VB.Val(ssList1_Sheet1.Cells[argROW, 74].Text);

            //등록번호
            ssList2_Sheet1.Cells[4, 1].Text = ssList1_Sheet1.Cells[argROW, 1].Text;

            //환자성명
            ssList2_Sheet1.Cells[4, 3].Text = ssList1_Sheet1.Cells[argROW, 2].Text;

            //진료과목
            ssList2_Sheet1.Cells[4, 5].Text = ssList1_Sheet1.Cells[argROW, 3].Text;

            if (FstrFlag1 == "N")
            {
                ssList2_Sheet1.Cells[4, 7].Text = VB.Left(ssList1_Sheet1.Cells[argROW, 4].Text, 10) + "~" + VB.Right(ssList1_Sheet1.Cells[argROW, 4].Text, 10); //진료기간
            }
            else if (FstrFlag1 == "")
            {
                ssList2_Sheet1.Cells[4, 7].Text = VB.Left(dtpFDate.Text, 4) + "." + VB.Mid(dtpFDate.Text, 6, 2) + "." + VB.Right(dtpFDate.Text, 2) + "~" + VB.Left(dtpTDate.Text, 4) + "." + VB.Mid(dtpTDate.Text, 6, 2) + "." + VB.Right(dtpTDate.Text, 2);
            }

            //주민번호
            ssList2_Sheet1.Cells[5, 3].Text = "";
            ssList2_Sheet1.Cells[5, 3].Text = ssList1_Sheet1.Cells[argROW, 6].Text;

            //보험유형
            ssList2_Sheet1.Cells[5, 7].Text = "";
            ssList2_Sheet1.Cells[5, 7].Text = ssList1_Sheet1.Cells[argROW, 7].Text;

            //진찰료공단
            ssList2_Sheet1.Cells[8, 3].Text = "";
            ssList2_Sheet1.Cells[8, 3].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 12].Text).ToString("###,###,##0"), 10);
            //진찰료본인
            ssList2_Sheet1.Cells[8, 4].Text = "";
            ssList2_Sheet1.Cells[8, 4].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 13].Text).ToString("###,###,##0"), 10);
            //진찰료비급
            ssList2_Sheet1.Cells[8, 5].Text = "";
            ssList2_Sheet1.Cells[8, 5].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 14].Text).ToString("###,###,##0"), 10);


            //입원료공단
            ssList2_Sheet1.Cells[9, 3].Text = "";
            ssList2_Sheet1.Cells[9, 3].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 15].Text).ToString("###,###,##0"), 10);
            //본인
            ssList2_Sheet1.Cells[9, 4].Text = "";
            ssList2_Sheet1.Cells[9, 4].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 16].Text).ToString("###,###,##0"), 10);
            //비급
            ssList2_Sheet1.Cells[9, 5].Text = "";
            ssList2_Sheet1.Cells[9, 5].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 17].Text).ToString("###,###,##0"), 10);


            //투약료공단
            ssList2_Sheet1.Cells[10, 3].Text = "";
            ssList2_Sheet1.Cells[10, 3].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 18].Text).ToString("###,###,##0"), 10);
            //본인
            ssList2_Sheet1.Cells[10, 4].Text = "";
            ssList2_Sheet1.Cells[10, 4].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 19].Text).ToString("###,###,##0"), 10);
            //비급
            ssList2_Sheet1.Cells[10, 5].Text = "";
            ssList2_Sheet1.Cells[10, 5].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 20].Text).ToString("###,###,##0"), 10);


            //주사료공단
            ssList2_Sheet1.Cells[11, 3].Text = "";
            ssList2_Sheet1.Cells[11, 3].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 21].Text).ToString("###,###,##0"), 10);
            //본인
            ssList2_Sheet1.Cells[11, 4].Text = "";
            ssList2_Sheet1.Cells[11, 4].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 22].Text).ToString("###,###,##0"), 10);
            //비급
            ssList2_Sheet1.Cells[11, 5].Text = "";
            ssList2_Sheet1.Cells[11, 5].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 23].Text).ToString("###,###,##0"), 10);


            //마취료공단
            ssList2_Sheet1.Cells[12, 3].Text = "";
            ssList2_Sheet1.Cells[12, 3].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 24].Text).ToString("###,###,##0"), 10);
            //본인
            ssList2_Sheet1.Cells[12, 4].Text = "";
            ssList2_Sheet1.Cells[12, 4].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 25].Text).ToString("###,###,##0"), 10);
            //비급
            ssList2_Sheet1.Cells[12, 5].Text = "";
            ssList2_Sheet1.Cells[12, 5].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 26].Text).ToString("###,###,##0"), 10);


            //물리,정신료공단
            ssList2_Sheet1.Cells[13, 3].Text = "";
            ssList2_Sheet1.Cells[13, 3].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 27].Text).ToString("###,###,##0"), 10);
            //본인
            ssList2_Sheet1.Cells[13, 4].Text = "";
            ssList2_Sheet1.Cells[13, 4].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 28].Text).ToString("###,###,##0"), 10);
            //비급
            ssList2_Sheet1.Cells[13, 5].Text = "";
            ssList2_Sheet1.Cells[13, 5].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 29].Text).ToString("###,###,##0"), 10);


            //처치료공단
            ssList2_Sheet1.Cells[14, 3].Text = "";
            ssList2_Sheet1.Cells[14, 3].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 30].Text).ToString("###,###,##0"), 10);
            //본인
            ssList2_Sheet1.Cells[14, 4].Text = "";
            ssList2_Sheet1.Cells[14, 4].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 31].Text).ToString("###,###,##0"), 10);
            //비급
            ssList2_Sheet1.Cells[14, 5].Text = "";
            ssList2_Sheet1.Cells[14, 5].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 32].Text).ToString("###,###,##0"), 10);


            //수술료공단
            ssList2_Sheet1.Cells[15, 3].Text = "";
            ssList2_Sheet1.Cells[15, 3].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 33].Text).ToString("###,###,##0"), 10);
            //본인
            ssList2_Sheet1.Cells[15, 4].Text = "";
            ssList2_Sheet1.Cells[15, 4].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 34].Text).ToString("###,###,##0"), 10);
            //비급
            ssList2_Sheet1.Cells[15, 5].Text = "";
            ssList2_Sheet1.Cells[15, 5].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 35].Text).ToString("###,###,##0"), 10);


            //검사료공단
            ssList2_Sheet1.Cells[16, 3].Text = "";
            ssList2_Sheet1.Cells[16, 3].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 36].Text).ToString("###,###,##0"), 10);
            //본인
            ssList2_Sheet1.Cells[16, 4].Text = "";
            ssList2_Sheet1.Cells[16, 4].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 37].Text).ToString("###,###,##0"), 10);
            //비급
            ssList2_Sheet1.Cells[16, 5].Text = "";
            ssList2_Sheet1.Cells[16, 5].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 38].Text).ToString("###,###,##0"), 10);


            //방사선료공단
            ssList2_Sheet1.Cells[17, 3].Text = "";
            ssList2_Sheet1.Cells[17, 3].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 39].Text).ToString("###,###,##0"), 10);
            //본인
            ssList2_Sheet1.Cells[17, 4].Text = "";
            ssList2_Sheet1.Cells[17, 4].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 40].Text).ToString("###,###,##0"), 10);
            //비급
            ssList2_Sheet1.Cells[17, 5].Text = "";
            ssList2_Sheet1.Cells[17, 5].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 41].Text).ToString("###,###,##0"), 10);


            //CT료공단
            ssList2_Sheet1.Cells[8, 7].Text = "";
            ssList2_Sheet1.Cells[8, 7].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 42].Text).ToString("###,###,##0"), 10);
            //본인
            ssList2_Sheet1.Cells[8, 8].Text = "";
            ssList2_Sheet1.Cells[8, 8].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 43].Text).ToString("###,###,##0"), 10);
            //비급
            ssList2_Sheet1.Cells[8, 9].Text = "";
            ssList2_Sheet1.Cells[8, 9].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 44].Text).ToString("###,###,##0"), 10);


            //MRI료공단
            ssList2_Sheet1.Cells[9, 7].Text = "";
            ssList2_Sheet1.Cells[9, 7].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 45].Text).ToString("###,###,##0"), 10);
            //본인
            ssList2_Sheet1.Cells[9, 8].Text = "";
            ssList2_Sheet1.Cells[9, 8].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 46].Text).ToString("###,###,##0"), 10);
            //비급
            ssList2_Sheet1.Cells[9, 9].Text = "";
            ssList2_Sheet1.Cells[9, 9].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 47].Text).ToString("###,###,##0"), 10);


            //초음파료공단
            ssList2_Sheet1.Cells[10, 7].Text = "";
            ssList2_Sheet1.Cells[10, 7].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 48].Text).ToString("###,###,##0"), 10);
            //본인
            ssList2_Sheet1.Cells[10, 8].Text = "";
            ssList2_Sheet1.Cells[10, 8].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 49].Text).ToString("###,###,##0"), 10);
            //비급
            ssList2_Sheet1.Cells[10, 9].Text = "";
            ssList2_Sheet1.Cells[10, 9].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 50].Text).ToString("###,###,##0"), 10);


            //식대,실료공단
            ssList2_Sheet1.Cells[11, 7].Text = "";
            ssList2_Sheet1.Cells[11, 7].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 51].Text).ToString("###,###,##0"), 10);
            //본인
            ssList2_Sheet1.Cells[11, 8].Text = "";
            ssList2_Sheet1.Cells[11, 8].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 52].Text).ToString("###,###,##0"), 10);
            //비급
            ssList2_Sheet1.Cells[11, 9].Text = "";
            ssList2_Sheet1.Cells[11, 9].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 53].Text).ToString("###,###,##0"), 10);


            //증명료공단
            ssList2_Sheet1.Cells[12, 7].Text = "";
            ssList2_Sheet1.Cells[12, 7].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 54].Text).ToString("###,###,##0"), 10);
            //본인
            ssList2_Sheet1.Cells[12, 8].Text = "";
            ssList2_Sheet1.Cells[12, 8].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 55].Text).ToString("###,###,##0"), 10);
            //비급
            ssList2_Sheet1.Cells[12, 9].Text = "";
            ssList2_Sheet1.Cells[12, 9].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 56].Text).ToString("###,###,##0"), 10);


            //선택진료비급
            ssList2_Sheet1.Cells[13, 9].Text = "";
            ssList2_Sheet1.Cells[13, 9].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 71].Text).ToString("###,###,##0"), 10);

            nAmt3 = VB.Val(ssList1_Sheet1.Cells[argROW, 71].Text);


            //기타공단
            ssList2_Sheet1.Cells[14, 7].Text = "";
            ssList2_Sheet1.Cells[14, 7].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 57].Text).ToString("###,###,##0"), 10);
            //본인
            ssList2_Sheet1.Cells[14, 8].Text = "";
            ssList2_Sheet1.Cells[14, 8].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 58].Text).ToString("###,###,##0"), 10);
            //비급
            ssList2_Sheet1.Cells[14, 9].Text = "";
            ssList2_Sheet1.Cells[14, 9].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 59].Text).ToString("###,###,##0"), 10);



            if (nDrgAmt > 0)
            {
                ssList2_Sheet1.Cells[15, 6].Text = "포괄수가금액";


                //기타공단
                ssList2_Sheet1.Cells[15, 7].Text = "";
                ssList2_Sheet1.Cells[15, 7].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 74].Text).ToString("###,###,##0"), 10);
                //본인
                ssList2_Sheet1.Cells[15, 8].Text = "";
                ssList2_Sheet1.Cells[15, 8].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 75].Text).ToString("###,###,##0"), 10);
                //비급
                ssList2_Sheet1.Cells[15, 9].Text = "";
                ssList2_Sheet1.Cells[15, 9].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 76].Text).ToString("###,###,##0"), 10);
            }
            else
            {
                ssList2_Sheet1.Cells[15, 6].Text = "";
                ssList2_Sheet1.Cells[15, 7].Text = "";
                ssList2_Sheet1.Cells[15, 8].Text = "";
                ssList2_Sheet1.Cells[15, 9].Text = "";
            }



            ssList2_Sheet1.Cells[16, 6].Text = "선별급여";
            //본인
            ssList2_Sheet1.Cells[16, 7].Text = "";
            ssList2_Sheet1.Cells[16, 7].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 77].Text).ToString("###,###,##0"), 10);
            //비급
            ssList2_Sheet1.Cells[16, 8].Text = "";
            ssList2_Sheet1.Cells[16, 8].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 78].Text).ToString("###,###,##0"), 10);



            //합계공단
            ssList2_Sheet1.Cells[17, 7].Text = "";
            ssList2_Sheet1.Cells[17, 7].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 60].Text).ToString("###,###,##0"), 10);
            //본인
            ssList2_Sheet1.Cells[17, 8].Text = "";
            ssList2_Sheet1.Cells[17, 8].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 61].Text).ToString("###,###,##0"), 10);
            //비급
            ssList2_Sheet1.Cells[17, 9].Text = "";
            ssList2_Sheet1.Cells[17, 9].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 62].Text).ToString("###,###,##0"), 10);


            //진료비총액
            ssList2_Sheet1.Cells[20, 3].Text = "";
            ssList2_Sheet1.Cells[20, 3].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 65].Text).ToString("###,###,##0"), 10);
            //환자부담총액
            ssList2_Sheet1.Cells[20, 8].Text = "";
            ssList2_Sheet1.Cells[20, 8].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 66].Text).ToString("###,###,##0"), 10);



            //진료비감액
            ssList2_Sheet1.Cells[21, 3].Text = "";
            ssList2_Sheet1.Cells[21, 3].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 67].Text).ToString("###,###,##0"), 10);


            ssList2_Sheet1.Cells[21, 8].Text = "";

            if (nAmt2 > 0)
            {
                //수납금액(희귀난치지원금뺀금액)
                ssList2_Sheet1.Cells[21, 8].Text = VB.Right(VB.Space(10) + (VB.Val(ssList1_Sheet1.Cells[argROW, 69].Text) - nAmt2).ToString("###,###,##0"), 10);
            }
            else
            {
                //수납금액
                ssList2_Sheet1.Cells[21, 8].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 69].Text).ToString("###,###,##0"), 10);
            }


            //희귀난치지원금 2010-02-26, 2012-02-14 물리바우처 지원금 포함
            ssList2_Sheet1.Cells[22, 3].Text = "";
            ssList2_Sheet1.Cells[22, 3].Text = VB.Right(VB.Space(10) + (VB.Val(ssList1_Sheet1.Cells[argROW, 70].Text) - nAmt4).ToString("###,###,##0"), 10);

            //미수금
            ssList2_Sheet1.Cells[22, 8].Text = "";
            ssList2_Sheet1.Cells[22, 8].Text = VB.Right(VB.Space(10) + VB.Val(ssList1_Sheet1.Cells[argROW, 68].Text).ToString("###,###,##0"), 10);


            ssList2_Sheet1.Cells[24, 0].Text = "";
            ssList2_Sheet1.Cells[24, 0].Text = " " + ssList1_Sheet1.Cells[argROW, 8].Text;

            ssList2_Sheet1.Cells[25, 0].Text = "";
            ssList2_Sheet1.Cells[25, 0].Text = " " + ssList1_Sheet1.Cells[argROW, 9].Text;

            ssList2_Sheet1.Cells[26, 0].Text = "";
            ssList2_Sheet1.Cells[26, 0].Text = " " + ssList1_Sheet1.Cells[argROW, 10].Text;

            ssList2_Sheet1.Cells[27, 0].Text = "";
            ssList2_Sheet1.Cells[27, 0].Text = " " + ssList1_Sheet1.Cells[argROW, 11].Text;


            ssList2_Sheet1.Cells[29, 1].Text = "     * 수납내역서는 진료비 확인용으로만 가능하며 영수증으로 사용할 수 없습니다.";

            //2013-11-01
            if (chk22.Checked == true)
            {
                ssList2_Sheet1.Cells[29, 1].Text = "";
            }

            ssList2_Sheet1.Cells[32, 5].Text = "";
            ssList2_Sheet1.Cells[32, 5].Text = VB.Left(CurrentDate, 4) + " 년 " + VB.Mid(CurrentDate, 6, 2) + " 월 " + VB.Right(CurrentDate, 2) + " 일";
        }

        void eGetData()
        {
            int i = 0;
            int j = 0;
            int k = 0;
            int nX = 0;

            int nREAD = 0;

            clsPmpaPrint PPT = new clsPmpaPrint();
            FarPoint.Win.Spread.FpSpread ssSpread = null;

            string[] data = new string[58];
            string strBi = "";
            string strDept = "";
            string strBun = "";
            string strSelf = "";

            int nNUM = 0;
            int nREAD2 = 0;

            string strChk = "";
            string strDept_New = "";
            string strDept_Old = "";
            double nMisu = 0;
            double nBonAmt = 0;
            int nSeqNo = 0;
            string strACTDATE = "";
            string strBDATE2 = "";
            
            double n물리치료바우처 = 0;   //물리치료 바우쳐 금액 누적 2012-02-14
            long nTRSNo = 0;
            long nIPDNO = 0;
            string strDrgCode = "";

            string strNgt = "";
            string strInDate = "";
            string strOutDate = "";

            double nDrgBiAmt = 0;
            double nDrgBonAmt = 0;
            double nDrgBonBiAmt = 0;

            double nSangAmt = 0;
            double nBoninTAmt = 0;

            double nMisuTo = 0;
            double nMisuTo2 = 0;

            string strOK = "";
            string strSunap = "";

            DataTable dt = null;
            DataTable dt1 = null;
            DataTable dt2 = null;
            DataTable dtTemp2 = null;
            DataTable dtTemp3 = null;

            string SQL = "";
            string SqlErr = "";

            string strBDATE = "";
            string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");

            ssList1_Sheet1.Rows.Count = 0;

            btnPrint.Enabled = false;

            for (i = 0; i < FstrROWID.Length; i++)
            {
                FstrROWID[i] = "";
            }

            if (DateTime.Compare(dtpFDate.Value, Convert.ToDateTime(CurrentDate).AddDays(-1830)) <= 0)
            {
                ComFunc.MsgBox("외래 진료비 수납 내역서는 5년까지만 출력이 가능합니다.!!!");
                if (CF.JIN_AMT_PRINT_CHK(clsDB.DbCon, clsType.User.Sabun) == "OK")
                {
                    btnPrint.Enabled = true;
                }
            }
            else
            {
                btnPrint.Enabled = true;
            }

            //주민번호 2 읽은것이 엉망이라 해로 하나 만듬
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                    ";
            SQL += ComNum.VBLF + "  JUMIN2                                                    ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT                     ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                 ";
            SQL += ComNum.VBLF + "      AND PANO = '" + txtPano.Text + "'                   ";
            
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                clsPmpaType.TBP.Jumin2 = dt.Rows[0]["JUMIN2"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;

            if (chkManual.Checked == true)
            {
                #region 입원수동데이타, READ_IPD_SU_SLIP(GoSub)

                SQL = "";
                SQL += ComNum.VBLF + "SELECT";
                SQL += ComNum.VBLF + "  A.TRSNO,A.PANO,A.IPDNO,TO_CHAR(A.INDATE,'YYYY.MM.DD') INDATE,";
                SQL += ComNum.VBLF + "  TO_CHAR(A.OUTDATE,'YYYY.MM.DD') OUTDATE ,";
                SQL += ComNum.VBLF + "  MIN(TO_CHAR(c.BDate,'YYYY-MM-DD')) MinDate, MAX(TO_CHAR(c.BDate,'YYYY-MM-DD')) MaxDate ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_TRANS A, " + ComNum.DB_PMPA + "IPD_SU_SLIP C";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND A.PANO = '" + txtPano.Text + "'";
                SQL += ComNum.VBLF + "      AND A.GBIPD <> 'D'";
                SQL += ComNum.VBLF + "      AND A.IPDNO = C.IPDNO";
                SQL += ComNum.VBLF + "      AND A.TRSNO = C.TRSNO ";
                SQL += ComNum.VBLF + "GROUP BY A.TRSNO,A.PANO,A.IPDNO,TO_CHAR(A.INDATE,'YYYY.MM.DD'),";
                SQL += ComNum.VBLF + "          TO_CHAR(A.OUTDATE,'YYYY.MM.DD')";
                SQL += ComNum.VBLF + "ORDER BY A.PANO,A.IPDNO,A.TRSNO";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nREAD = dt.Rows.Count;

                    ssList1_Sheet1.Rows.Count = nREAD;

                    for (i = 0; i < nREAD; i++)
                    {

                        Array.Clear(data, 0, data.Length);

                        clsPmpaType.TRPG.TotAmt1 = new long[51];
                        clsPmpaType.TRPG.TotAmt2 = new long[51];
                        clsPmpaType.TRPG.TotAmt3 = new long[51];
                        clsPmpaType.TRPG.TotAmt4 = new long[51];
                        clsPmpaType.TRPG.TotAmt5 = new long[51];
                        clsPmpaType.TRPG.TotAmt6 = new long[51];
                        clsPmpaType.TRPG.TotAmt7 = new long[51];
                        clsPmpaType.TRPG.TotAmt8 = new long[51];

                        for (j = 0; j <= 50; j++)
                        {
                            clsPmpaType.TRPG.TotAmt1[j] = 0;
                            clsPmpaType.TRPG.TotAmt2[j] = 0;
                            clsPmpaType.TRPG.TotAmt3[j] = 0;
                            clsPmpaType.TRPG.TotAmt4[j] = 0;
                            clsPmpaType.TRPG.TotAmt5[j] = 0;
                            clsPmpaType.TRPG.TotAmt6[j] = 0;
                            clsPmpaType.TRPG.TotAmt7[j] = 0;
                            clsPmpaType.TRPG.TotAmt8[j] = 0;
                        }

                        nTRSNo = (long)VB.Val(dt.Rows[i]["Trsno"].ToString().Trim());

                        nSangAmt = Sang_Check();
                        if (nSangAmt > 0)
                        {
                            ComFunc.MsgBox("해당년에 상한금액이 있습니다... [" + String.Format("{0:###,###,###,##0}", nSangAmt) + "]" + ComNum.VBLF + ComNum.VBLF + "출력기간내 상한이 적용된다면 진료비영수증은 수동처리하십시오!!");
                        }

                        //입원기본사항
                        strBi = clsPmpaType.TIT.Bi;
                        strDept = clsPmpaType.TIT.DeptCode;

                        data[4] = dt.Rows[i]["MinDATE"].ToString().Trim() + " " + dt.Rows[i]["MaxDATE"].ToString().Trim();
                        data[5] = (CF.DATE_ILSU(clsDB.DbCon, VB.Pstr(data[4], " ", 2), VB.Pstr(data[4], " ", 1))).ToString();

                        Ipd_Slip_Amt_Set();

                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT";
                        SQL += ComNum.VBLF + "  BUN , GBSELF, SUM(AMT1) SAMT,SUM(AMT2) SAMT2 ";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_SU_SLIP";
                        SQL += ComNum.VBLF + "WHERE 1=1";
                        SQL += ComNum.VBLF + "      AND PANO = '" + txtPano.Text + "'";
                        SQL += ComNum.VBLF + "      AND TRSNO = " + dt.Rows[i]["TRSNO"].ToString().Trim() + "";
                        SQL += ComNum.VBLF + "GROUP BY BUN, GBSELF ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            for (j = 0; j < dt1.Rows.Count; j++)
                            {
                                strBun = dt1.Rows[j]["BUN"].ToString().Trim();
                                strSelf = dt1.Rows[j]["GBSELF"].ToString().Trim();

                                #region BUN_MOVE(GoSub)

                                if (strSelf != "0")
                                {
                                    switch (strBun)
                                    {
                                        case "01":
                                        case "02":
                                            nNUM = 13;  //진찰료
                                            break;

                                        case "03":
                                        case "04":
                                        case "05":
                                        case "06":
                                        case "07":
                                        case "08":
                                        case "09":
                                        case "10":
                                            nNUM = 15;  //입원료
                                            break;

                                        case "11":
                                        case "12":
                                        case "13":
                                        case "14":
                                        case "15":
                                            nNUM = 17;  //투약료 및 조제료
                                            break;

                                        case "16":
                                        case "17":
                                        case "18":
                                        case "19":
                                        case "20":
                                        case "21":
                                            nNUM = 19;  //주사료
                                            break;

                                        case "22":
                                        case "23":
                                            nNUM = 21;  //마취료
                                            break;

                                        case "24":
                                        case "25":
                                        case "26":
                                        case "27":
                                            nNUM = 23;  //이학요법료(물리치료) 정신요법료
                                            break;

                                        case "28":
                                        case "29":
                                        case "30":
                                        case "31":
                                        case "32":
                                        case "33":
                                            nNUM = 25;  //처치
                                            break;

                                        case "34":
                                        case "35":
                                        case "36":
                                        case "37":
                                        case "38":
                                        case "39":
                                        case "40":
                                            nNUM = 27;  //수술
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
                                            nNUM = 29;  //검사료
                                            break;

                                        case "65":
                                        case "66":
                                        case "67":
                                        case "68":
                                        case "69":
                                        case "70":
                                            nNUM = 31;  //방사선료
                                            break;

                                        case "72":
                                            if (strBi == "43" || strBi == "42" || strBi == "41")
                                            {
                                                nNUM = 33;  //CT
                                            }

                                            else if (String.Compare(strBi, "11") >= 0 && String.Compare(strBi, "22") <= 0)
                                            {
                                                nNUM = 33;  //CT
                                            }

                                            else
                                            {
                                                nNUM = 32;
                                            }
                                            break;

                                        case "73":
                                            nNUM = 35;  //MRI
                                            break;

                                        case "71":
                                            nNUM = 37;  //초음파
                                            break;

                                        case "74":
                                        case "77":
                                            nNUM = 39;  //식대, 실료차
                                            break;

                                        case "75":
                                            nNUM = 41;  //증명료
                                            break;

                                        case "99":
                                            nNUM = 52;  //영수액
                                            break;

                                        case "98":
                                            nNUM = 47;  //조합부담액
                                            break;

                                        case "92":
                                            nNUM = 50;  //감액
                                            break;

                                        case "96":
                                            nNUM = 51;  //미수액
                                            break;

                                        default:
                                            nNUM = 43;  //비급여기타
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (strBun)
                                    {
                                        case "01":
                                        case "02":
                                            nNUM = 12;  //진찰료
                                            break;

                                        case "03":
                                        case "04":
                                        case "05":
                                        case "06":
                                        case "07":
                                        case "08":
                                        case "09":
                                        case "10":
                                            nNUM = 14;  //입원료
                                            break;

                                        case "11":
                                        case "12":
                                        case "13":
                                        case "14":
                                        case "15":
                                            nNUM = 16;  //투약료 및 조제료
                                            break;

                                        case "16":
                                        case "17":
                                        case "18":
                                        case "19":
                                        case "20":
                                        case "21":
                                            nNUM = 18;  //주사료
                                            break;

                                        case "22":
                                        case "23":
                                            nNUM = 20;  //마취료
                                            break;

                                        case "24":
                                        case "25":
                                        case "26":
                                        case "27":
                                            nNUM = 22;  //이학요법료(물리치료) 정신요법료
                                            break;

                                        case "28":
                                        case "29":
                                        case "30":
                                        case "31":
                                        case "32":
                                        case "33":
                                            nNUM = 24;  //처치
                                            break;

                                        case "34":
                                        case "35":
                                        case "36":
                                        case "37":
                                        case "38":
                                        case "39":
                                        case "40":
                                            nNUM = 26;  //수술
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
                                            nNUM = 28;  //검사료
                                            break;

                                        case "65":
                                        case "66":
                                        case "67":
                                        case "68":
                                        case "69":
                                        case "70":
                                            nNUM = 30;  //방사선료
                                            break;

                                        case "72":
                                            if (strBi == "43" || strBi == "42" || strBi == "41")
                                            {
                                                nNUM = 33;  //CT
                                            }

                                            else
                                            {
                                                nNUM = 32;
                                            }
                                            break;

                                        case "73":
                                            nNUM = 34;  //MRI
                                            break;

                                        case "71":
                                            nNUM = 36;  //초음파
                                            break;

                                        case "74":
                                        case "77":
                                            nNUM = 38;  //식대, 실료차
                                            break;

                                        case "75":
                                            nNUM = 40;  //증명료
                                            break;

                                        case "99":
                                            nNUM = 52;  //영수액
                                            break;

                                        case "98":
                                            nNUM = 47;  //조합부담액
                                            break;

                                        case "92":
                                            nNUM = 50;  //감액
                                            break;

                                        case "96":
                                            nNUM = 51;  //미수액
                                            break;

                                        default:
                                            nNUM = 42;  //비급여기타
                                            break;
                                    }
                                }


                                #endregion BUN_MOVE(GoSub) End

                                data[nNUM] = (VB.Val(data[nNUM]) + VB.Val(dt1.Rows[j]["SAMT"].ToString().Trim())).ToString();

                                if (nNUM <= 43)
                                {
                                    if (strSelf == "0")
                                    {
                                        data[44] = (VB.Val(data[44]) + VB.Val(dt1.Rows[j]["SAMT"].ToString().Trim())).ToString();
                                    }

                                    else
                                    {
                                        data[45] = (VB.Val(data[45]) + VB.Val(dt1.Rows[j]["SAMT"].ToString().Trim())).ToString();
                                    }
                                }
                            }

                            data[47] = clsPmpaType.RPG.Amt6[50].ToString();

                            data[48] = (clsPmpaType.RPG.Amt1[50] + clsPmpaType.RPG.Amt2[50] + clsPmpaType.RPG.Amt3[50] + clsPmpaType.RPG.Amt4[50]).ToString();

                            data[50] = dt.Rows[i]["AMT54"].ToString().Trim();   //진료비감액
                            data[51] = dt.Rows[i]["AMT56"].ToString().Trim();   //미수금액

                            data[53] = dt.Rows[i]["AMT52"].ToString().Trim();   //희귀난치지원금 2010-02-26

                            data[54] = clsPmpaType.RPG.Amt3[50].ToString();

                            data[45] = (VB.Val(data[45]) + VB.Val(data[54])).ToString();    //비급여총액에 선택포함 2011-06-17

                            data[46] = (VB.Val(data[48]) - VB.Val(data[47]) - VB.Val(data[45])).ToString(); //본인부담금

                            data[49] = (VB.Val(data[46]) + VB.Val(data[45])).ToString();

                            data[52] = (VB.Val(data[49]) - VB.Val(data[50]) - VB.Val(data[51])).ToString();

                            dt1.Dispose();
                            dt1 = null;
                        }


                        string strDum = "";

                        for (j = 1; j <= 4; j++)
                        {
                            #region //에러남 무조건 ""
                            //strDum = "ILLCODE" + j;

                            //if (dt.Rows[i]["strDum"].ToString().Trim() != "")
                            //{
                            //    data[8 + j - 1] = VB.Left(dt.Rows[i]["strDum"].ToString().Trim() + VB.Space(10), 10) + CF.Read_IllsName(clsDB.DbCon, dt.Rows[i]["strDum"].ToString().Trim(), "2");
                            //}
                            #endregion //에러남 무조건 ""
                        }


                        //DISPLAY
                        ssList1_Sheet1.Cells[i, 1].Text = txtPano.Text;                                             //환자등록번호
                        ssList1_Sheet1.Cells[i, 2].Text = clsPmpaType.TBP.Sname;                                    //환자성명
                        ssList1_Sheet1.Cells[i, 3].Text = CF.READ_DEPTNAMEK(clsDB.DbCon, strDept);                               //진료과목
                        ssList1_Sheet1.Cells[i, 4].Text = data[4];                                                  //진료기간
                        ssList1_Sheet1.Cells[i, 5].Text = data[5];                                                  //진료일수
                        ssList1_Sheet1.Cells[i, 6].Text = clsPmpaType.TBP.Jumin1 + "-" + clsPmpaType.TBP.Jumin2;    //주민등록번호
                        ssList1_Sheet1.Cells[i, 7].Text = CF.Read_Bi_Name(clsDB.DbCon, strBi, "2");                              //보험유형

                        for (j = 9; j <= 12; j++)
                        {
                            ssList1_Sheet1.Cells[i, j - 1].Text = data[j - 1];
                        }

                        //진찰료
                        ssList1_Sheet1.Cells[i, 12].Text = clsPmpaType.TRPG.TotAmt6[1].ToString();
                        ssList1_Sheet1.Cells[i, 13].Text = clsPmpaType.TRPG.TotAmt5[1].ToString();
                        ssList1_Sheet1.Cells[i, 14].Text = (clsPmpaType.TRPG.TotAmt2[1] + clsPmpaType.TRPG.TotAmt4[1]).ToString();

                        //입원료
                        ssList1_Sheet1.Cells[i, 15].Text = clsPmpaType.TRPG.TotAmt6[2].ToString();
                        ssList1_Sheet1.Cells[i, 16].Text = clsPmpaType.TRPG.TotAmt5[2].ToString();
                        ssList1_Sheet1.Cells[i, 17].Text = (clsPmpaType.TRPG.TotAmt2[2] + clsPmpaType.TRPG.TotAmt4[2]).ToString();

                        //투약료
                        ssList1_Sheet1.Cells[i, 18].Text = (clsPmpaType.TRPG.TotAmt6[4] + clsPmpaType.TRPG.TotAmt6[5]).ToString();
                        ssList1_Sheet1.Cells[i, 19].Text = (clsPmpaType.TRPG.TotAmt5[4] + clsPmpaType.TRPG.TotAmt5[5]).ToString();
                        ssList1_Sheet1.Cells[i, 20].Text = (clsPmpaType.TRPG.TotAmt2[4] + clsPmpaType.TRPG.TotAmt4[4] + clsPmpaType.TRPG.TotAmt2[5] + clsPmpaType.TRPG.TotAmt4[5]).ToString();

                        //주사료
                        ssList1_Sheet1.Cells[i, 21].Text = (clsPmpaType.TRPG.TotAmt6[6] + clsPmpaType.TRPG.TotAmt6[7]).ToString();
                        ssList1_Sheet1.Cells[i, 22].Text = (clsPmpaType.TRPG.TotAmt5[6] + clsPmpaType.TRPG.TotAmt5[7]).ToString();
                        ssList1_Sheet1.Cells[i, 23].Text = (clsPmpaType.TRPG.TotAmt2[6] + clsPmpaType.TRPG.TotAmt4[6] + clsPmpaType.TRPG.TotAmt2[7] + clsPmpaType.TRPG.TotAmt4[7]).ToString();

                        //마취료
                        ssList1_Sheet1.Cells[i, 24].Text = clsPmpaType.TRPG.TotAmt6[8].ToString();
                        ssList1_Sheet1.Cells[i, 25].Text = clsPmpaType.TRPG.TotAmt5[8].ToString();
                        ssList1_Sheet1.Cells[i, 26].Text = (clsPmpaType.TRPG.TotAmt2[8] + clsPmpaType.TRPG.TotAmt4[8]).ToString();

                        //물리,정신
                        ssList1_Sheet1.Cells[i, 27].Text = (clsPmpaType.TRPG.TotAmt6[14] + clsPmpaType.TRPG.TotAmt6[15]).ToString();
                        ssList1_Sheet1.Cells[i, 28].Text = (clsPmpaType.TRPG.TotAmt5[14] + clsPmpaType.TRPG.TotAmt5[15]).ToString();
                        ssList1_Sheet1.Cells[i, 29].Text = (clsPmpaType.TRPG.TotAmt2[14] + clsPmpaType.TRPG.TotAmt4[14] + clsPmpaType.TRPG.TotAmt2[15] + clsPmpaType.TRPG.TotAmt4[15]).ToString();

                        //처치
                        ssList1_Sheet1.Cells[i, 30].Text = clsPmpaType.TRPG.TotAmt6[9].ToString();
                        ssList1_Sheet1.Cells[i, 31].Text = clsPmpaType.TRPG.TotAmt5[9].ToString();
                        ssList1_Sheet1.Cells[i, 32].Text = (clsPmpaType.TRPG.TotAmt2[9] + clsPmpaType.TRPG.TotAmt4[9]).ToString();

                        //수술
                        ssList1_Sheet1.Cells[i, 33].Text = clsPmpaType.TRPG.TotAmt6[12].ToString();
                        ssList1_Sheet1.Cells[i, 34].Text = clsPmpaType.TRPG.TotAmt5[12].ToString();
                        ssList1_Sheet1.Cells[i, 35].Text = (clsPmpaType.TRPG.TotAmt2[12] + clsPmpaType.TRPG.TotAmt4[12]).ToString();

                        //검사
                        ssList1_Sheet1.Cells[i, 36].Text = clsPmpaType.TRPG.TotAmt6[10].ToString();
                        ssList1_Sheet1.Cells[i, 37].Text = clsPmpaType.TRPG.TotAmt5[10].ToString();
                        ssList1_Sheet1.Cells[i, 38].Text = (clsPmpaType.TRPG.TotAmt2[10] + clsPmpaType.TRPG.TotAmt4[10]).ToString();

                        //방사선
                        ssList1_Sheet1.Cells[i, 39].Text = clsPmpaType.TRPG.TotAmt6[11].ToString();
                        ssList1_Sheet1.Cells[i, 40].Text = clsPmpaType.TRPG.TotAmt5[11].ToString();
                        ssList1_Sheet1.Cells[i, 41].Text = (clsPmpaType.TRPG.TotAmt2[11] + clsPmpaType.TRPG.TotAmt4[11]).ToString();

                        //CT
                        ssList1_Sheet1.Cells[i, 42].Text = clsPmpaType.TRPG.TotAmt6[17].ToString();
                        ssList1_Sheet1.Cells[i, 43].Text = clsPmpaType.TRPG.TotAmt5[17].ToString();
                        ssList1_Sheet1.Cells[i, 44].Text = (clsPmpaType.TRPG.TotAmt2[17] + clsPmpaType.TRPG.TotAmt4[17]).ToString();

                        //MRI
                        ssList1_Sheet1.Cells[i, 45].Text = clsPmpaType.TRPG.TotAmt6[18].ToString();
                        ssList1_Sheet1.Cells[i, 46].Text = clsPmpaType.TRPG.TotAmt5[18].ToString();
                        ssList1_Sheet1.Cells[i, 47].Text = (clsPmpaType.TRPG.TotAmt2[18] + clsPmpaType.TRPG.TotAmt4[18]).ToString();

                        //초음파
                        ssList1_Sheet1.Cells[i, 48].Text = clsPmpaType.TRPG.TotAmt6[19].ToString();
                        ssList1_Sheet1.Cells[i, 49].Text = clsPmpaType.TRPG.TotAmt5[19].ToString();
                        ssList1_Sheet1.Cells[i, 50].Text = (clsPmpaType.TRPG.TotAmt2[19] + clsPmpaType.TRPG.TotAmt4[19]).ToString();

                        //식대,실료
                        ssList1_Sheet1.Cells[i, 51].Text = (clsPmpaType.TRPG.TotAmt6[3] + clsPmpaType.TRPG.TotAmt6[21]).ToString();
                        ssList1_Sheet1.Cells[i, 52].Text = (clsPmpaType.TRPG.TotAmt5[3] + clsPmpaType.TRPG.TotAmt5[21]).ToString();
                        ssList1_Sheet1.Cells[i, 53].Text = (clsPmpaType.TRPG.TotAmt2[3] + clsPmpaType.TRPG.TotAmt4[3] + clsPmpaType.TRPG.TotAmt2[21] + clsPmpaType.TRPG.TotAmt4[21]).ToString();

                        //증명료
                        ssList1_Sheet1.Cells[i, 54].Text = clsPmpaType.TRPG.TotAmt6[22].ToString();
                        ssList1_Sheet1.Cells[i, 55].Text = clsPmpaType.TRPG.TotAmt5[22].ToString();
                        ssList1_Sheet1.Cells[i, 56].Text = (clsPmpaType.TRPG.TotAmt2[22] + clsPmpaType.TRPG.TotAmt4[22]).ToString();

                        //기타
                        ssList1_Sheet1.Cells[i, 57].Text = (clsPmpaType.TRPG.TotAmt6[49] + clsPmpaType.TRPG.TotAmt6[13] + clsPmpaType.TRPG.TotAmt6[16] + clsPmpaType.TRPG.TotAmt6[20]).ToString();
                        ssList1_Sheet1.Cells[i, 58].Text = (clsPmpaType.TRPG.TotAmt5[49] + clsPmpaType.TRPG.TotAmt5[13] + clsPmpaType.TRPG.TotAmt5[16] + clsPmpaType.TRPG.TotAmt5[20]).ToString();
                        ssList1_Sheet1.Cells[i, 59].Text = (clsPmpaType.TRPG.TotAmt2[49] + clsPmpaType.TRPG.TotAmt4[49] + clsPmpaType.TRPG.TotAmt2[13] + clsPmpaType.TRPG.TotAmt2[16] + clsPmpaType.TRPG.TotAmt2[13] + clsPmpaType.TRPG.TotAmt4[13] + clsPmpaType.TRPG.TotAmt2[16] + clsPmpaType.TRPG.TotAmt4[16]).ToString();




                        ssList1_Sheet1.Cells[i, 60].Text = data[47];    //급여공단합
                        ssList1_Sheet1.Cells[i, 61].Text = data[46];    //급여본인합
                        ssList1_Sheet1.Cells[i, 62].Text = data[45];    //비급여합

                        ssList1_Sheet1.Cells[i, 60].Text = data[46];    //급여본인
                        ssList1_Sheet1.Cells[i, 61].Text = data[47];    //급여공단합
                        ssList1_Sheet1.Cells[i, 62].Text = data[48];    //진료비총액
                        ssList1_Sheet1.Cells[i, 66].Text = (VB.Val(data[46]) + VB.Val(data[45])).ToString();    //환자부담총액

                        ssList1_Sheet1.Cells[i, 67].Text = data[50];    //감액
                        ssList1_Sheet1.Cells[i, 68].Text = data[51];    //미수
                        ssList1_Sheet1.Cells[i, 69].Text = data[52];    //수납금
                        ssList1_Sheet1.Cells[i, 70].Text = data[53];    //희귀
                        ssList1_Sheet1.Cells[i, 71].Text = data[54];    //선택

                        ssList1_Sheet1.Cells[i, 73].Text = nSangAmt.ToString(); //입원상한금액

                    }

                }

                dt.Dispose();
                dt = null;



                #endregion READ_IPD_SU_SLIP(GoSub) End
            }
            else
            {
                //외래
                if (optIO1.Checked == true)
                {
                    #region READ_OPD_SLIP(GoSub)

                    //당일 접수
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                                    ";
                    SQL += ComNum.VBLF + "  AMT7                                                    ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER                     ";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                 ";
                    SQL += ComNum.VBLF + "      AND PANO = '" + txtPano.Text + "'                   ";
                    SQL += ComNum.VBLF + "      AND ACTDATE = TRUNC(SYSDATE)                        ";
                    if (cboBi.SelectedItem.ToString() != "전체")
                    {
                        SQL += ComNum.VBLF + "  AND BI  = '" + cboBi.SelectedItem.ToString() + "'   ";
                    }

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        nREAD2 = dt1.Rows.Count;
                    }

                    dt1.Dispose();
                    dt1 = null;

                    if (chkView.Checked == false)
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT";
                        SQL += ComNum.VBLF + "  X.BI, X.DEPTCODE FROM (";
                        SQL += ComNum.VBLF + "                          SELECT /*+index(A INDEX_OPDSL4)*/BI, DEPTCODE";
                        SQL += ComNum.VBLF + "                          FROM " + ComNum.DB_PMPA + "OPD_SLIP";
                        SQL += ComNum.VBLF + "                          WHERE BDATE >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')";
                        SQL += ComNum.VBLF + "                          AND BDATE <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "                          AND PANO = '" + txtPano.Text + "'";
                        SQL += ComNum.VBLF + "                          AND TRIM(SUNEXT) NOT IN ( SELECT CODE FROM ADMIN.BAS_BCODE WHERE GUBUN ='원무영수제외코드')";
                        if (cboDept.SelectedItem.ToString() != "전체")
                        {
                            SQL += ComNum.VBLF + "                      AND DeptCode = '" + cboDept.SelectedItem.ToString() + "'";
                        }
                        if (cboBi.SelectedItem.ToString() != "전체")
                        {
                            SQL += ComNum.VBLF + "                      AND BI  = '" + cboBi.SelectedItem.ToString() + "'";
                        }
                        SQL += ComNum.VBLF + "                          GROUP BY BI, DEPTCODE";
                        SQL += ComNum.VBLF + "                          UNION ALL";
                        SQL += ComNum.VBLF + "                          SELECT BI, DEPTCODE";
                        SQL += ComNum.VBLF + "                          FROM " + ComNum.DB_PMPA + "OPD_MASTER";
                        SQL += ComNum.VBLF + "                          WHERE BDATE >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')";
                        SQL += ComNum.VBLF + "                          AND BDATE <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "                          AND PANO = '" + txtPano.Text + "'";
                        if (cboDept.SelectedItem.ToString() != "전체")
                        {
                            SQL += ComNum.VBLF + "                      AND DeptCode = '" + cboDept.SelectedItem.ToString() + "'";
                        }
                        if (cboBi.SelectedItem.ToString() != "전체")
                        {
                            SQL += ComNum.VBLF + "                      AND BI  = '" + cboBi.SelectedItem.ToString() + "'";
                        }
                        SQL += ComNum.VBLF + "                          GROUP BY BI, DEPTCODE) X";
                        SQL += ComNum.VBLF + "GROUP BY X.BI, X.DEPTCODE";
                        SQL += ComNum.VBLF + "ORDER BY X.BI, X.DEPTCODE";
                    }

                    else
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT";
                        SQL += ComNum.VBLF + "  TO_CHAR(X.BDATE, 'YYYY-MM-DD') AS BDATE, X.BI, X.DEPTCODE FROM (";
                        SQL += ComNum.VBLF + "                                  SELECT /*+index(A INDEX_OPDSL4)*/BDATE, BI, DEPTCODE";
                        SQL += ComNum.VBLF + "                                  FROM " + ComNum.DB_PMPA + "OPD_SLIP";
                        SQL += ComNum.VBLF + "                                  WHERE BDATE >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')";
                        SQL += ComNum.VBLF + "                                  AND BDATE <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "                                  AND PANO = '" + txtPano.Text + "'";
                        SQL += ComNum.VBLF + "                                  AND TRIM(SUNEXT) NOT IN ( SELECT CODE FROM ADMIN.BAS_BCODE WHERE GUBUN ='원무영수제외코드')";
                        if (cboDept.SelectedItem.ToString() != "전체")
                        {
                            SQL += ComNum.VBLF + "                              AND DeptCode = '" + cboDept.SelectedItem.ToString() + "'";
                        }
                        if (cboBi.SelectedItem.ToString() != "전체")
                        {
                            SQL += ComNum.VBLF + "                              AND BI  = '" + cboBi.SelectedItem.ToString() + "'";
                        }
                        SQL += ComNum.VBLF + "                                  GROUP BY BDATE, BI, DEPTCODE";
                        SQL += ComNum.VBLF + "                                  UNION ALL";
                        SQL += ComNum.VBLF + "                                  SELECT BDATE, BI, DEPTCODE";
                        SQL += ComNum.VBLF + "                                  FROM " + ComNum.DB_PMPA + "OPD_MASTER";
                        SQL += ComNum.VBLF + "                                  WHERE BDATE >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')";
                        SQL += ComNum.VBLF + "                                  AND BDATE <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "                                  AND PANO = '" + txtPano.Text + "'";
                        if (cboDept.SelectedItem.ToString() != "전체")
                        {
                            SQL += ComNum.VBLF + "                              AND DeptCode = '" + cboDept.SelectedItem.ToString() + "'";
                        }
                        if (cboBi.SelectedItem.ToString() != "전체")
                        {
                            SQL += ComNum.VBLF + "                              AND BI  = '" + cboBi.SelectedItem.ToString() + "'";
                        }
                        SQL += ComNum.VBLF + "                                  GROUP BY BDATE, BI, DEPTCODE) X";
                        SQL += ComNum.VBLF + "GROUP BY X.BDATE, X.BI, X.DEPTCODE";
                        SQL += ComNum.VBLF + "ORDER BY X.BDATE, X.BI, X.DEPTCODE";
                    }

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }


                    nREAD = dt.Rows.Count;

                    if (nREAD == 0 && nREAD2 == 0)
                    {
                        ComFunc.MsgBox("해당하는 DATA가 없습니다.");
                        return;
                    }

                    //CLEAR
                    ssList1_Sheet1.Rows.Count = nREAD + 1;

                    for (i = 0; i < nREAD; i++)
                    {
                        //2016-02-17 미수내역 점검
                        if (CPF.READ_MISU_CHECK(clsDB.DbCon, txtPano.Text) == true)
                        {
                            ComFunc.MsgBox("미수내역이 있습니다. 확인바랍니다.");
                        }

                        nMisuTo = 0;
                        nMisuTo2 = 0;
                        nMisu = 0;
                        nBonAmt = 0;

                        for (j = 0; j < data.Length; j++)
                        {
                            data[j] = "";
                        }

                        clsPmpaType.TRPG.TotAmt1 = new long[51];
                        clsPmpaType.TRPG.TotAmt2 = new long[51];
                        clsPmpaType.TRPG.TotAmt3 = new long[51];
                        clsPmpaType.TRPG.TotAmt4 = new long[51];
                        clsPmpaType.TRPG.TotAmt5 = new long[51];
                        clsPmpaType.TRPG.TotAmt6 = new long[51];
                        clsPmpaType.TRPG.TotAmt7 = new long[51];
                        clsPmpaType.TRPG.TotAmt8 = new long[51];
                        clsPmpaType.TRPG.TotAmt9 = new long[51];

                        for (j = 0; j <= 50; j++)
                        {
                            clsPmpaType.TRPG.TotAmt1[j] = 0;
                            clsPmpaType.TRPG.TotAmt2[j] = 0;
                            clsPmpaType.TRPG.TotAmt3[j] = 0;
                            clsPmpaType.TRPG.TotAmt4[j] = 0;
                            clsPmpaType.TRPG.TotAmt5[j] = 0;
                            clsPmpaType.TRPG.TotAmt6[j] = 0;
                            clsPmpaType.TRPG.TotAmt7[j] = 0;
                            clsPmpaType.TRPG.TotAmt8[j] = 0;
                            clsPmpaType.TRPG.TotAmt9[j] = 0;
                        }

                        strBi = dt.Rows[i]["BI"].ToString().Trim();
                        strDept = dt.Rows[i]["DEPTCODE"].ToString().Trim();

                        strDept_New = strDept;

                        if (chkView.Checked == true)
                        {
                            strBDATE = dt.Rows[i]["BDate"].ToString().Trim();
                        }
                        else
                        {
                            strBDATE = dtpTDate.Text;
                        }

                        if (chkView.Checked == true)
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + "SELECT                                                                                    ";
                            SQL += ComNum.VBLF + "  PANO,DEPTCODE,BI,TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE,TO_CHAR(bDATE,'YYYY-MM-DD') bDATE,SEQNO,PART                       ";
                            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP                                                       ";
                            SQL += ComNum.VBLF + "WHERE 1=1                                                                                 ";
                            SQL += ComNum.VBLF + "      AND PANO ='" + txtPano.Text + "'                                                    ";
                            SQL += ComNum.VBLF + "      AND BDATE = TO_DATE('" + dt.Rows[i]["BDATE"].ToString().Trim() + "','YYYY-MM-DD')   ";
                            SQL += ComNum.VBLF + "      AND BI ='" + strBi + "'                                                             ";
                            SQL += ComNum.VBLF + "      AND DEPTCODE ='" + strDept + "'                                                     ";
                            SQL += ComNum.VBLF + "GROUP BY PANO,DEPTCODE,BI,TO_CHAR(ACTDATE,'YYYY-MM-DD'),TO_CHAR(bDATE,'YYYY-MM-DD'),SEQNO,PART                        ";

                            SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                //clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }

                            n물리치료바우처 = 0;

                            if (dt2.Rows.Count > 0)
                            {
                                for (k = 0; k < dt2.Rows.Count; k++)
                                {
                                    nSeqNo = Convert.ToInt32(VB.Val(dt2.Rows[k]["Seqno"].ToString().Trim()));
                                    strACTDATE = dt2.Rows[k]["ActDate"].ToString().Trim();
                                    strBDATE2 = dt2.Rows[k]["bDATE"].ToString().Trim();
                                    clsPmpaPb.GstrJeaDate = strACTDATE;
                                    clsPmpaPb.GstrJeaPart = dt2.Rows[k]["Part"].ToString().Trim();

                                    if (mstrJobName != "")
                                    {
                                        CPF.Report_Print_Sunap_2012_Gesan(clsDB.DbCon, txtPano.Text, "과이름", "성명", "", nSeqNo, "", "", strBi, strBDATE2, "", "", strDept, "", "", "", "", "", "", "", mstrJobName);
                                       //PPT.Report_Print_Sunap_A4_New(clsDB.DbCon, 0, txtPano.Text, "과이름", "성명", "", nSeqNo, "", "", strBi, strACTDATE, "", "", strDept, picSign, "", "", "", "", "", "", "", ssSpread, "", "");
                                    }
                                    else
                                    {
                                        CPF.Report_Print_Sunap_2012_Gesan(clsDB.DbCon, txtPano.Text, "과이름", "성명", "", nSeqNo, "", "", strBi, strBDATE2, "", "", strDept, "", "", "", "", "", "", "", "");
                                        //PPT.Report_Print_Sunap_A4_New(clsDB.DbCon, 0, txtPano.Text, "과이름", "성명", "", nSeqNo, "", "", strBi, strACTDATE, "", "", strDept, picSign, "", "", "", "", "", "", "", ssSpread, "", "");
                                    }

                                    n물리치료바우처 += clsPmpaPb.GnPtVoucherAmt;

                                    Opd_Slip_Amt_Set();
                                }
                            }

                            dt2.Dispose();
                            dt2 = null;


                            SQL = "";
                            SQL += ComNum.VBLF + "SELECT";
                            SQL += ComNum.VBLF + "  BDATE, BUN , GBSELF, SUM(AMT1+AMT2) SAMT , 0 SAMT2";
                            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP";
                            SQL += ComNum.VBLF + "WHERE 1=1";
                            SQL += ComNum.VBLF + "      AND BDATE = TO_DATE('" + dt.Rows[i]["BDATE"].ToString().Trim() + "','YYYY-MM-DD')";
                            SQL += ComNum.VBLF + "      AND PANO = '" + txtPano.Text + "'";
                            SQL += ComNum.VBLF + "      AND BI = '" + strBi + "'";
                            SQL += ComNum.VBLF + "      AND DEPTCODE = '" + strDept + "'";
                            SQL += ComNum.VBLF + "      AND TRIM(SUNEXT) NOT IN ( SELECT CODE FROM ADMIN.BAS_BCODE WHERE GUBUN ='원무영수제외코드')"; //저가약제 제외코드 2011-04-01                            
                            SQL += ComNum.VBLF + "GROUP BY BDATE, BUN, GBSELF";
                        }
                        else
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + "SELECT";
                            SQL += ComNum.VBLF + "  /*+index(A INDEX_OPDSL4)*/PANO,DEPTCODE,BI,TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE,TO_CHAR(bDATE,'YYYY-MM-DD') bDATE,SEQNO,PART";
                            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP";
                            SQL += ComNum.VBLF + "WHERE 1=1";
                            SQL += ComNum.VBLF + "      AND PANO ='" + txtPano.Text + "'";
                            SQL += ComNum.VBLF + "      AND BDATE >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')";
                            SQL += ComNum.VBLF + "      AND BDATE <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')";
                            SQL += ComNum.VBLF + "      AND BI ='" + strBi + "'";
                            SQL += ComNum.VBLF + "      AND DEPTCODE ='" + strDept + "'";
                            SQL += ComNum.VBLF + "GROUP BY PANO,DEPTCODE,BI,TO_CHAR(ACTDATE,'YYYY-MM-DD'),TO_CHAR(bDATE,'YYYY-MM-DD'),SEQNO,PART";

                            SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                            n물리치료바우처 = 0;

                            if (dt2.Rows.Count > 0)
                            {
                                for (k = 0; k < dt2.Rows.Count; k++)
                                {
                                    nSeqNo = Convert.ToInt32(dt2.Rows[k]["Seqno"].ToString().Trim());
                                    strACTDATE = dt2.Rows[k]["ActDate"].ToString().Trim();
                                    strBDATE2 = dt2.Rows[k]["bDATE"].ToString().Trim();
                                    clsPmpaPb.GstrJeaDate = strACTDATE;
                                    clsPmpaPb.GstrJeaPart = dt2.Rows[k]["Part"].ToString().Trim();

                                    CPF.Report_Print_Sunap_2012_Gesan(clsDB.DbCon, txtPano.Text, "과이름", "성명", "", nSeqNo, "", "", strBi, strBDATE2, "", "", strDept, "", "", "", "", "", "", "", "");
                                    //CPP.Report_Print_Sunap_A4_New(clsDB.DbCon, 0, txtPano.Text, "과이름", "성명", "", nSeqNo, "", "", strBi, strACTDATE, "", "", strDept, picSign, "", "", "", "", "", "", "", ssSpread, "", "");
                                    n물리치료바우처 += clsPmpaPb.GnPtVoucherAmt;

                                    Opd_Slip_Amt_Set();
                                }
                            }

                            dt2.Dispose();
                            dt2 = null;

                            SQL = "";
                            SQL += ComNum.VBLF + "SELECT";
                            SQL += ComNum.VBLF + "  /*+index(A INDEX_OPDSL4)*/ BUN , GBSELF, SUM(AMT1) SAMT,SUM(AMT2) SAMT2 ";
                            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP";
                            SQL += ComNum.VBLF + "WHERE 1=1";
                            SQL += ComNum.VBLF + "      AND BDATE >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')";
                            SQL += ComNum.VBLF + "      AND BDATE <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')";
                            SQL += ComNum.VBLF + "      AND PANO = '" + txtPano.Text + "'";
                            SQL += ComNum.VBLF + "      AND BI = '" + strBi + "'";
                            SQL += ComNum.VBLF + "      AND DEPTCODE = '" + strDept + "' ";
                            SQL += ComNum.VBLF + "      AND TRIM(SUNEXT) NOT IN ( SELECT CODE FROM ADMIN.BAS_BCODE WHERE GUBUN ='원무영수제외코드')"; //저가약제 제외코드 2011-04-01
                            SQL += ComNum.VBLF + "GROUP BY BUN, GBSELF";
                        }

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            for (j = 0; j < dt1.Rows.Count; j++)
                            {
                                strBun = dt1.Rows[j]["BUN"].ToString().Trim();
                                strSelf = dt1.Rows[j]["GBSELF"].ToString().Trim();

                                #region BUN_MOVE(GoSub)

                                if (strSelf != "0")
                                {
                                    switch (strBun)
                                    {
                                        case "01":
                                        case "02":
                                            nNUM = 13;  //진찰료
                                            break;

                                        case "03":
                                        case "04":
                                        case "05":
                                        case "06":
                                        case "07":
                                        case "08":
                                        case "09":
                                        case "10":
                                            nNUM = 15;  //입원료
                                            break;

                                        case "11":
                                        case "12":
                                        case "13":
                                        case "14":
                                        case "15":
                                            nNUM = 17;  //투약료 및 조제료
                                            break;

                                        case "16":
                                        case "17":
                                        case "18":
                                        case "19":
                                        case "20":
                                        case "21":
                                            nNUM = 19;  //주사료
                                            break;

                                        case "22":
                                        case "23":
                                            nNUM = 21;  //마취료
                                            break;

                                        case "24":
                                        case "25":
                                        case "26":
                                        case "27":
                                            nNUM = 23;  //이학요법료(물리치료) 정신요법료
                                            break;

                                        case "28":
                                        case "29":
                                        case "30":
                                        case "31":
                                        case "32":
                                        case "33":
                                            nNUM = 25;  //처치
                                            break;

                                        case "34":
                                        case "35":
                                        case "36":
                                        case "37":
                                        case "38":
                                        case "39":
                                        case "40":
                                            nNUM = 27;  //수술
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
                                            nNUM = 29;  //검사료
                                            break;

                                        case "65":
                                        case "66":
                                        case "67":
                                        case "68":
                                        case "69":
                                        case "70":
                                            nNUM = 31;  //방사선료
                                            break;

                                        case "72":
                                            if (strBi == "43" || strBi == "42" || strBi == "41")
                                            {
                                                nNUM = 33;  //CT
                                            }

                                            else if (String.Compare(strBi, "11") >= 0 && String.Compare(strBi, "22") <= 0)
                                            {
                                                nNUM = 33;  //CT
                                            }

                                            else
                                            {
                                                nNUM = 32;
                                            }
                                            break;

                                        case "73":
                                            nNUM = 35;  //MRI
                                            break;

                                        case "71":
                                            nNUM = 37;  //초음파
                                            break;

                                        case "74":
                                        case "77":
                                            nNUM = 39;  //식대, 실료차
                                            break;

                                        case "75":
                                            nNUM = 41;  //증명료
                                            break;

                                        case "99":
                                            nNUM = 52;  //영수액
                                            break;

                                        case "98":
                                            nNUM = 47;  //조합부담액
                                            break;

                                        case "92":
                                            nNUM = 50;  //감액
                                            break;

                                        case "96":
                                            nNUM = 51;  //미수액
                                            break;

                                        default:
                                            nNUM = 43;  //비급여기타
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (strBun)
                                    {
                                        case "01":
                                        case "02":
                                            nNUM = 12;  //진찰료
                                            break;

                                        case "03":
                                        case "04":
                                        case "05":
                                        case "06":
                                        case "07":
                                        case "08":
                                        case "09":
                                        case "10":
                                            nNUM = 14;  //입원료
                                            break;

                                        case "11":
                                        case "12":
                                        case "13":
                                        case "14":
                                        case "15":
                                            nNUM = 16;  //투약료 및 조제료
                                            break;

                                        case "16":
                                        case "17":
                                        case "18":
                                        case "19":
                                        case "20":
                                        case "21":
                                            nNUM = 18;  //주사료
                                            break;

                                        case "22":
                                        case "23":
                                            nNUM = 20;  //마취료
                                            break;

                                        case "24":
                                        case "25":
                                        case "26":
                                        case "27":
                                            nNUM = 22;  //이학요법료(물리치료) 정신요법료
                                            break;

                                        case "28":
                                        case "29":
                                        case "30":
                                        case "31":
                                        case "32":
                                        case "33":
                                            nNUM = 24;  //처치
                                            break;

                                        case "34":
                                        case "35":
                                        case "36":
                                        case "37":
                                        case "38":
                                        case "39":
                                        case "40":
                                            nNUM = 26;  //수술
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
                                            nNUM = 28;  //검사료
                                            break;

                                        case "65":
                                        case "66":
                                        case "67":
                                        case "68":
                                        case "69":
                                        case "70":
                                            nNUM = 30;  //방사선료
                                            break;

                                        case "72":
                                            if (strBi == "43" || strBi == "42" || strBi == "41")
                                            {
                                                nNUM = 33;  //CT
                                            }

                                            else
                                            {
                                                nNUM = 32;
                                            }
                                            break;

                                        case "73":
                                            nNUM = 34;  //MRI
                                            break;

                                        case "71":
                                            nNUM = 36;  //초음파
                                            break;

                                        case "74":
                                        case "77":
                                            nNUM = 38;  //식대, 실료차
                                            break;

                                        case "75":
                                            nNUM = 40;  //증명료
                                            break;

                                        case "99":
                                            nNUM = 52;  //영수액
                                            break;

                                        case "98":
                                            nNUM = 47;  //조합부담액
                                            break;

                                        case "92":
                                            nNUM = 50;  //감액
                                            break;

                                        case "96":
                                            nNUM = 51;  //미수액
                                            break;

                                        default:
                                            nNUM = 42;  //비급여기타
                                            break;
                                    }
                                }


                                #endregion BUN_MOVE(GoSub) End

                                data[nNUM] = (VB.Val(data[nNUM]) + Convert.ToInt32(dt1.Rows[j]["SAMT"].ToString().Trim())).ToString();
                                //선택진료
                                data[54] = (VB.Val(data[54]) + Convert.ToInt32(dt1.Rows[j]["SAMT2"].ToString().Trim())).ToString();

                                if (nNUM <= 43)
                                {
                                    //진료비총액
                                    data[48] = (VB.Val(data[48]) + Convert.ToInt32(dt1.Rows[j]["SAMT"].ToString().Trim())).ToString();

                                    if (strSelf == "0" && strBi != "43")
                                    {
                                        data[44] = (VB.Val(data[44]) + Convert.ToInt32(dt1.Rows[j]["SAMT"].ToString().Trim())).ToString();
                                    }

                                    else
                                    {
                                        data[45] = (VB.Val(data[45]) + Convert.ToInt32(dt1.Rows[j]["SAMT"].ToString().Trim())).ToString();
                                    }
                                }
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;

                        //당일 접수
                        strChk = "";

                       // if (strBDATE == clsPmpaPb.GstrSysDate && nREAD2 > 0 && strDept_New != strDept_Old)
                        if (strBDATE == clsPmpaPb.GstrSysDate && nREAD2 > 0)
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + "SELECT                                                            ";
                            SQL += ComNum.VBLF + "  AMT1,Amt2, AMT4, AMT5, AMT6, AMT7                               ";
                            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER                             ";
                            SQL += ComNum.VBLF + "WHERE 1=1                                                         ";
                            SQL += ComNum.VBLF + "      AND PANO = '" + txtPano.Text + "'                           ";
                            SQL += ComNum.VBLF + "      AND ACTDATE = TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "      AND BI  = '" + strBi + "'                                   ";
                            SQL += ComNum.VBLF + "      AND DEPTCODE  = '" + strDept + "'                           ";
                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            if (dt1.Rows.Count > 0)
                            {
                                nMisu = VB.Val(dt1.Rows[0]["AMT6"].ToString().Trim());
                                nBonAmt = VB.Val(dt1.Rows[0]["AMT7"].ToString().Trim());

                                //선택진료
                                data[54] = (VB.Val(data[54]) + VB.Val(dt1.Rows[0]["AMT2"].ToString().Trim())).ToString();

                                data[48] = (VB.Val(data[48]) + VB.Val(dt1.Rows[0]["AMT1"].ToString().Trim())).ToString();  //진료비총액
                                data[47] = (VB.Val(data[47]) + VB.Val(dt1.Rows[0]["AMT4"].ToString().Trim())).ToString();  //조합부담금
                                data[50] = (VB.Val(data[50]) + VB.Val(dt1.Rows[0]["AMT5"].ToString().Trim())).ToString();  //감액
                                data[51] = (VB.Val(data[51]) + VB.Val(dt1.Rows[0]["AMT6"].ToString().Trim())).ToString();  //미수
                                data[52] = (VB.Val(data[52]) + VB.Val(dt1.Rows[0]["AMT7"].ToString().Trim())).ToString();  //본인부담액

                                //2013-12-14  계약처 미수등록자는 미수액을 본인부담액으로 표시함
                                if (READ_MISU_GYEPANO(txtPano.Text.Trim(), dtpTDate.Text, strDept, strBi) == "OK")
                                {
                                    if (data[52] == "0")
                                    {
                                        data[52] = (VB.Val(data[52]) + VB.Val(dt1.Rows[0]["AMT6"].ToString().Trim())).ToString();
                                    }
                                }

                                if (strBi != "43")
                                {
                                    data[12] = (VB.Val(data[12]) + VB.Val(dt1.Rows[0]["AMT1"].ToString().Trim())).ToString();

                                    data[44] = (VB.Val(data[44]) + VB.Val(dt1.Rows[0]["AMT1"].ToString().Trim())).ToString();

                                    if (READ_MISU_GYEPANO(txtPano.Text.Trim(), dtpTDate.Text, strDept, strBi) == "OK")
                                    {
                                        clsPmpaType.TRPG.TotAmt5[1] += Convert.ToInt64(nMisu) - Convert.ToInt64(VB.Val(dt1.Rows[0]["AMT2"].ToString().Trim()));     //본인
                                    }

                                    else
                                    {
                                        clsPmpaType.TRPG.TotAmt5[1] += Convert.ToInt64(nBonAmt) - Convert.ToInt64(VB.Val(dt1.Rows[0]["AMT2"].ToString().Trim()));   //본인
                                    }

                                    clsPmpaType.TRPG.TotAmt6[1] += Convert.ToInt64(VB.Val(dt1.Rows[0]["AMT4"].ToString().Trim()));      //조합
                                    clsPmpaType.TRPG.TotAmt3[1] += Convert.ToInt64(VB.Val(dt1.Rows[0]["AMT2"].ToString().Trim()));      //선택
                                }

                                else
                                {
                                    data[13] = (VB.Val(data[13]) + VB.Val(dt1.Rows[0]["AMT1"].ToString().Trim())).ToString();    //조합;// Convert.ToInt32(VB.Val(dt1.Rows[0]["AMT1"].ToString().Trim())).ToString());
                                    data[45] = (VB.Val(data[45]) + VB.Val(dt1.Rows[0]["AMT1"].ToString().Trim())).ToString(); //Convert.ToInt32(VB.Val(dt1.Rows[0]["AMT1"].ToString().Trim())).ToString());

                                    clsPmpaType.TRPG.TotAmt5[1] += Convert.ToInt64(VB.Val(dt1.Rows[0]["AMT1"].ToString().Trim())) - Convert.ToInt64(VB.Val(dt1.Rows[0]["AMT2"].ToString().Trim()));     //본인
                                    clsPmpaType.TRPG.TotAmt3[1] += Convert.ToInt64(VB.Val(dt1.Rows[0]["AMT2"].ToString().Trim()));     //선택
                                }

                                strChk = "OK";
                                data[5] = 1.ToString();
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }

                        data[45] = (VB.Val(data[45]) + VB.Val(data[54])).ToString();                        //진료총액

                        data[48] = (VB.Val(data[48]) + VB.Val(data[54])).ToString();                        //진료비총액, 선택진료포함
                        data[46] = (VB.Val(data[48]) - VB.Val(data[47]) - VB.Val(data[45])).ToString();     //본인부담금
                        data[49] = (VB.Val(data[46]) + VB.Val(data[45])).ToString();                        //

                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT                                                                    ";
                        SQL += ComNum.VBLF + "  A.ILLCODE , B.ILLNAMEK                                                  ";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OILLS A, " + ComNum.DB_PMPA + "BAS_IllS B   ";
                        SQL += ComNum.VBLF + "WHERE 1=1                                                                 ";
                        SQL += ComNum.VBLF + "      AND A.BDATE >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')        ";
                        SQL += ComNum.VBLF + "      AND A.BDATE <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')        ";
                        SQL += ComNum.VBLF + "      AND A.PTNO = '" + txtPano.Text + "'                                 ";
                        SQL += ComNum.VBLF + "      AND A.DEPTCODE = '" + strDept + "'                                  ";
                        SQL += ComNum.VBLF + "      AND A.ILLCODE = B.ILLCODE                                           ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            for (j = 0; j < dt1.Rows.Count; j++)
                            {
                                if (j > 3)
                                {
                                    break;
                                }

                                data[8 + j] = VB.Left(dt1.Rows[j]["ILLCODE"].ToString().Trim() + VB.Space(10), 10) + dt1.Rows[j]["ILLNAMEK"].ToString().Trim();
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;

                        if (chkView.Checked == true)
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + "SELECT";
                            SQL += ComNum.VBLF + "  TO_CHAR(BDATE,'YYYY.MM.DD') BDATE, SUM(AMT1+AMT2)";
                            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP";
                            SQL += ComNum.VBLF + "WHERE 1=1";
                            SQL += ComNum.VBLF + "      AND BDATE = TO_DATE('" + dt.Rows[i]["BDATE"].ToString().Trim() + "','YYYY-MM-DD')";
                            SQL += ComNum.VBLF + "      AND PANO = '" + txtPano.Text + "'";
                            SQL += ComNum.VBLF + "      AND BI = '" + strBi + "'";
                            SQL += ComNum.VBLF + "      AND DEPTCODE = '" + strDept + "'";
                            SQL += ComNum.VBLF + "      AND TRIM(SUNEXT) NOT IN ( SELECT CODE FROM ADMIN.BAS_BCODE WHERE GUBUN ='원무영수제외코드')"; //저가약제 제외코드 2011-04-01
                            SQL += ComNum.VBLF + "GROUP BY BDATE";
                            SQL += ComNum.VBLF + "HAVING SUM(AMT1) >0 ";
                            SQL += ComNum.VBLF + "ORDER BY BDATE ";
                        }

                        else
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + "SELECT";
                            SQL += ComNum.VBLF + "  /*+index(A INDEX_OPDSL4)*/TO_CHAR(BDATE,'YYYY.MM.DD') BDATE, SUM(AMT1+AMT2)";
                            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP";
                            SQL += ComNum.VBLF + "WHERE 1=1";
                            SQL += ComNum.VBLF + "      AND BDATE >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')";
                            SQL += ComNum.VBLF + "      AND BDATE <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')";
                            SQL += ComNum.VBLF + "      AND PANO = '" + txtPano.Text + "'";
                            SQL += ComNum.VBLF + "      AND BI = '" + strBi + "'";
                            SQL += ComNum.VBLF + "      AND DEPTCODE = '" + strDept + "'";
                            SQL += ComNum.VBLF + "      AND TRIM(SUNEXT) NOT IN ( SELECT CODE FROM ADMIN.BAS_BCODE WHERE GUBUN ='원무영수제외코드') "; //저가약제 제외코드 2011-04-01
                            SQL += ComNum.VBLF + "GROUP BY BDATE";
                            SQL += ComNum.VBLF + "HAVING SUM(AMT1) >0 ";
                            SQL += ComNum.VBLF + "ORDER BY BDATE ";
                        }

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            if (strChk == "OK")
                            {
                                data[4] = dt1.Rows[0]["BDATE"].ToString().Trim() + " " + clsPmpaPb.GstrSysDate;


                                //ex) 2017.10.11 -> 2017-10-11 형식으로 변환하기 위함
                                string Temp = "";
                                Temp = VB.Left(dt1.Rows[0]["BDATE"].ToString().Trim(), 4) + "-" + VB.Mid(dt1.Rows[0]["BDATE"].ToString().Trim(), 6, 2) + "-" + VB.Right(dt1.Rows[0]["BDATE"].ToString().Trim(), 2);

                                if (Temp != VB.Left(clsPmpaPb.GstrSysDate, 4) + "." + VB.Mid(clsPmpaPb.GstrSysDate, 6, 2) + "." + VB.Right(clsPmpaPb.GstrSysDate, 2))
                                {
                                    if (nREAD2 > 0)
                                    {
                                        data[5] = (VB.Val(data[5]) + dt1.Rows.Count - 1).ToString();
                                    }
                                    else
                                    {
                                        data[5] = (VB.Val(data[5]) + dt1.Rows.Count).ToString();
                                    }
                                }
                            }
                            else
                            {
                                //본소스 data(4) = AdoGetString(rs1, "BDATE", 0) & " " & AdoGetString(rs1, "BDATE", RowIndicator - 1)
                                data[4] = dt1.Rows[0]["BDATE"].ToString().Trim() + " " + dt1.Rows[dt1.Rows.Count - 1]["BDATE"].ToString().Trim();
                                data[5] = dt1.Rows.Count.ToString();
                            }

                            if (chkMisu.Checked == true)
                            {
                                //개별조회일 경우 해당일 발생된 미수금액 조회
                                if (chkView.Checked == true)
                                {
                                    SQL = "";
                                    SQL += ComNum.VBLF + "SELECT";
                                    SQL += ComNum.VBLF + "  SUM(AMT) SAMT";
                                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP";
                                    SQL += ComNum.VBLF + "WHERE 1=1";
                                    SQL += ComNum.VBLF + "      AND BDATE = TO_DATE('" + dt1.Rows[0]["BDATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                                    SQL += ComNum.VBLF + "      AND PANO = '" + txtPano.Text + "'";
                                    SQL += ComNum.VBLF + "      AND GUBUN1 ='1'";
                                    SQL += ComNum.VBLF + "      AND SUBSTR(MISUDTL,2,2) = '" + strDept + "' ";  //2012-12-13

                                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        return;
                                    }

                                    if (dt2.Rows.Count > 0)
                                    {
                                        strOK = "";

                                        SQL = "";
                                        SQL += ComNum.VBLF + "SELECT";
                                        SQL += ComNum.VBLF + "  ROWID";
                                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP";
                                        SQL += ComNum.VBLF + "WHERE 1=1";
                                        SQL += ComNum.VBLF + "      AND Pano ='" + txtPano.Text + "'";
                                        SQL += ComNum.VBLF + "      AND AMT = " + dt2.Rows[0]["SAMT"].ToString().Trim() + "";
                                        SQL += ComNum.VBLF + "      AND GUBUN1 ='2'";
                                        SQL += ComNum.VBLF + "      AND BDATE>= TO_DATE('" + dt1.Rows[0]["BDATE"].ToString().Trim() + "','YYYY-MM-DD')";

                                        SqlErr = clsDB.GetDataTable(ref dtTemp2, SQL, clsDB.DbCon);

                                        if (SqlErr != "")
                                        {
                                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                            return;
                                        }

                                        if (dtTemp2.Rows.Count > 0)
                                        {
                                            for (j = 0; j < dtTemp2.Rows.Count; j++)
                                            {
                                                if (strOK == "OK")
                                                {
                                                    break;
                                                }

                                                strSunap = "";

                                                for (k = 0; k <= 50; k++)
                                                {
                                                    if (FstrROWID[k] != "")
                                                    {
                                                        if (FstrROWID[k] == dtTemp2.Rows[j]["ROWID"].ToString().Trim())
                                                        {
                                                            SQL = "";
                                                            SQL += ComNum.VBLF + "SELECT";
                                                            SQL += ComNum.VBLF + "  ROWID";
                                                            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP";
                                                            SQL += ComNum.VBLF + "WHERE 1=1";
                                                            SQL += ComNum.VBLF + "      AND Pano ='" + txtPano.Text + "'";
                                                            SQL += ComNum.VBLF + "      AND AMT = " + dt2.Rows[0]["SAMT"].ToString().Trim() + "";
                                                            SQL += ComNum.VBLF + "      AND GUBUN1 ='2'";
                                                            SQL += ComNum.VBLF + "      AND BDATE>= TO_DATE('" + dt1.Rows[0]["BDATE"].ToString().Trim() + "','YYYY-MM-DD')";
                                                            SQL += ComNum.VBLF + "      AND ROWID NOT IN ('" + FstrROWID[k] + "')";

                                                            SqlErr = clsDB.GetDataTable(ref dtTemp3, SQL, clsDB.DbCon);

                                                            if (SqlErr != "")
                                                            {
                                                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                                                return;
                                                            }

                                                            if (dtTemp3.Rows.Count > 0)
                                                            {
                                                                data[52] = (VB.Val(data[52] + VB.Val(dt2.Rows[0]["SAMT"].ToString().Trim()))).ToString();
                                                                nMisuTo += VB.Val(dt2.Rows[0]["SAMT"].ToString().Trim());
                                                            }

                                                            dtTemp3.Dispose();
                                                            dtTemp3 = null;

                                                            strSunap = "OK";
                                                            strOK = "OK";
                                                        }
                                                    }
                                                }

                                                if (strSunap == "")
                                                {
                                                    FstrROWID[nX] = dtTemp2.Rows[j]["ROWID"].ToString().Trim();
                                                    nX += 1;
                                                    data[52] = (VB.Val(data[52]) + VB.Val(dt2.Rows[0]["SAMT"].ToString().Trim())).ToString();
                                                    nMisuTo += VB.Val(dt2.Rows[0]["SAMT"].ToString().Trim());
                                                    strOK = "OK";
                                                }
                                            }
                                        }
                                        dtTemp2.Dispose();
                                        dtTemp2 = null;

                                    }
                                    dt2.Dispose();
                                    dt2 = null;
                                }

                                //미수입금내역 조회
                                SQL = "";
                                SQL += ComNum.VBLF + "SELECT";
                                SQL += ComNum.VBLF + "  SUM(AMT) SAMT";
                                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP";
                                SQL += ComNum.VBLF + "WHERE 1=1";
                                if (chkView.Checked == true)
                                {
                                    //미수내역 조회할때 조회기간내 자료만 집계함 2014-03-12                                    
                                    SQL += ComNum.VBLF + "  AND BDATE = TO_DATE('" + dt1.Rows[0]["BDATE"].ToString().Trim() + "','YYYY-MM-DD')";
                                }

                                else
                                {
                                    SQL += ComNum.VBLF + "  AND BDATE >= TO_DATE('" + VB.Left(data[4], 10) + "','YYYY-MM-DD')";
                                    SQL += ComNum.VBLF + "  AND BDATEBDATE <= TO_DATE('" + VB.Right(data[4], 10) + "','YYYY-MM-DD')";
                                }

                                SQL += ComNum.VBLF + "      AND PANO = '" + txtPano.Text + "'";
                                SQL += ComNum.VBLF + "      AND GUBUN1 ='2'";
                                SQL += ComNum.VBLF + "      AND SUBSTR(MISUDTL,2,2) = '" + strDept + "' ";  //2012-12-13

                                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    return;
                                }

                                if (dt2.Rows.Count > 0)
                                {
                                    data[52] = (VB.Val(data[52]) + VB.Val(dt2.Rows[0]["SAMT"].ToString().Trim())).ToString();
                                    nMisuTo += VB.Val(dt2.Rows[0]["SAMT"].ToString().Trim());
                                }

                                dt2.Dispose();
                                dt2 = null;

                                //당일미수 2013-07-02
                                SQL = "";
                                SQL += ComNum.VBLF + "SELECT";
                                SQL += ComNum.VBLF + "  SUM(AMT) SAMT";
                                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP";
                                SQL += ComNum.VBLF + "WHERE 1=1";
                                if (chkView.Checked == true)
                                {
                                    SQL += ComNum.VBLF + "  AND BDATE = TO_DATE('" + dt1.Rows[0]["BDATE"].ToString().Trim() + "','YYYY-MM-DD')";
                                }

                                else
                                {
                                    SQL += ComNum.VBLF + "  AND BDATE >= TO_DATE('" + VB.Left(data[4], 10) + "','YYYY-MM-DD')";
                                    SQL += ComNum.VBLF + "  AND BDATE <= TO_DATE('" + VB.Right(data[4], 10) + "','YYYY-MM-DD')";
                                }

                                SQL += ComNum.VBLF + "      AND PANO = '" + txtPano.Text + "'";
                                SQL += ComNum.VBLF + "      AND GUBUN1 ='1'";

                                SQL += ComNum.VBLF + "      AND SUBSTR(MISUDTL,2,2) = '" + strDept + "' ";  //2012-12-13

                                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    return;
                                }

                                if (dt2.Rows.Count > 0)
                                {
                                    nMisuTo2 += VB.Val(dt2.Rows[0]["SAMT"].ToString().Trim());
                                }

                                dt2.Dispose();
                                dt2 = null;

                            }
                        }

                        dt1.Dispose();
                        dt1 = null;

                        if (data[4] == "" && nREAD2 > 0)
                        {
                            data[4] = clsPmpaPb.GstrSysDate;
                        }

                        //DISPLAY
                        ssList1_Sheet1.Cells[i, 1].Text = txtPano.Text;                                             //환자등록번호
                        ssList1_Sheet1.Cells[i, 2].Text = clsPmpaType.TBP.Sname;                                    //환자성명
                        ssList1_Sheet1.Cells[i, 3].Text = CF.READ_DEPTNAMEK(clsDB.DbCon, strDept);                               //진료과목
                        ssList1_Sheet1.Cells[i, 4].Text = data[4];                                                  //진료기간
                        ssList1_Sheet1.Cells[i, 5].Text = data[5];                                                  //진료일수
                        ssList1_Sheet1.Cells[i, 6].Text = clsPmpaType.TBP.Jumin1 + "-" + clsPmpaType.TBP.Jumin2;    //주민등록번호
                        ssList1_Sheet1.Cells[i, 7].Text = CF.Read_Bi_Name(clsDB.DbCon, strBi, "2");                              //보험유형
                        strDept_Old = strDept_New;

                        //상병
                        for (j = 9; j <= 12; j++)
                        {
                            ssList1_Sheet1.Cells[i, j - 1].Text = data[j - 1];
                        }

                        //진찰료
                        ssList1_Sheet1.Cells[i, 12].Text = clsPmpaType.TRPG.TotAmt6[1].ToString();
                        ssList1_Sheet1.Cells[i, 13].Text = clsPmpaType.TRPG.TotAmt5[1].ToString();
                        ssList1_Sheet1.Cells[i, 14].Text = (clsPmpaType.TRPG.TotAmt2[1] + clsPmpaType.TRPG.TotAmt4[1]).ToString();

                        //입원료
                        ssList1_Sheet1.Cells[i, 15].Text = clsPmpaType.TRPG.TotAmt6[2].ToString();
                        ssList1_Sheet1.Cells[i, 16].Text = clsPmpaType.TRPG.TotAmt5[2].ToString();
                        ssList1_Sheet1.Cells[i, 17].Text = (clsPmpaType.TRPG.TotAmt2[2] + clsPmpaType.TRPG.TotAmt4[2]).ToString();

                        //투약료
                        ssList1_Sheet1.Cells[i, 18].Text = (clsPmpaType.TRPG.TotAmt6[4] + clsPmpaType.TRPG.TotAmt6[5]).ToString();
                        ssList1_Sheet1.Cells[i, 19].Text = (clsPmpaType.TRPG.TotAmt5[4] + clsPmpaType.TRPG.TotAmt5[5]).ToString();
                        ssList1_Sheet1.Cells[i, 20].Text = (clsPmpaType.TRPG.TotAmt2[4] + clsPmpaType.TRPG.TotAmt4[4] + clsPmpaType.TRPG.TotAmt2[5] + clsPmpaType.TRPG.TotAmt4[5]).ToString();

                        //주사료
                        ssList1_Sheet1.Cells[i, 21].Text = (clsPmpaType.TRPG.TotAmt6[6] + clsPmpaType.TRPG.TotAmt6[7]).ToString();
                        ssList1_Sheet1.Cells[i, 22].Text = (clsPmpaType.TRPG.TotAmt5[6] + clsPmpaType.TRPG.TotAmt5[7]).ToString();
                        ssList1_Sheet1.Cells[i, 23].Text = (clsPmpaType.TRPG.TotAmt2[6] + clsPmpaType.TRPG.TotAmt4[6] + clsPmpaType.TRPG.TotAmt2[7] + clsPmpaType.TRPG.TotAmt4[7]).ToString();

                        //마취료
                        ssList1_Sheet1.Cells[i, 24].Text = clsPmpaType.TRPG.TotAmt6[8].ToString();
                        ssList1_Sheet1.Cells[i, 25].Text = clsPmpaType.TRPG.TotAmt5[8].ToString();
                        ssList1_Sheet1.Cells[i, 26].Text = (clsPmpaType.TRPG.TotAmt2[8] + clsPmpaType.TRPG.TotAmt4[8]).ToString();

                        //물리,정신
                        ssList1_Sheet1.Cells[i, 27].Text = (clsPmpaType.TRPG.TotAmt6[14] + clsPmpaType.TRPG.TotAmt6[15]).ToString();
                        ssList1_Sheet1.Cells[i, 28].Text = (clsPmpaType.TRPG.TotAmt5[14] + clsPmpaType.TRPG.TotAmt5[15]).ToString();
                        ssList1_Sheet1.Cells[i, 29].Text = (clsPmpaType.TRPG.TotAmt2[14] + clsPmpaType.TRPG.TotAmt4[14] + clsPmpaType.TRPG.TotAmt2[15] + clsPmpaType.TRPG.TotAmt4[15]).ToString();

                        //처치
                        ssList1_Sheet1.Cells[i, 30].Text = clsPmpaType.TRPG.TotAmt6[9].ToString();
                        ssList1_Sheet1.Cells[i, 31].Text = clsPmpaType.TRPG.TotAmt5[9].ToString();
                        ssList1_Sheet1.Cells[i, 32].Text = (clsPmpaType.TRPG.TotAmt2[9] + clsPmpaType.TRPG.TotAmt4[9]).ToString();

                        //수술
                        ssList1_Sheet1.Cells[i, 33].Text = clsPmpaType.TRPG.TotAmt6[12].ToString();
                        ssList1_Sheet1.Cells[i, 34].Text = clsPmpaType.TRPG.TotAmt5[12].ToString();
                        ssList1_Sheet1.Cells[i, 35].Text = (clsPmpaType.TRPG.TotAmt2[12] + clsPmpaType.TRPG.TotAmt4[12]).ToString();

                        //검사
                        ssList1_Sheet1.Cells[i, 36].Text = clsPmpaType.TRPG.TotAmt6[10].ToString();
                        ssList1_Sheet1.Cells[i, 37].Text = clsPmpaType.TRPG.TotAmt5[10].ToString();
                        ssList1_Sheet1.Cells[i, 38].Text = (clsPmpaType.TRPG.TotAmt2[10] + clsPmpaType.TRPG.TotAmt4[10]).ToString();

                        //방사선
                        ssList1_Sheet1.Cells[i, 39].Text = clsPmpaType.TRPG.TotAmt6[11].ToString();
                        ssList1_Sheet1.Cells[i, 40].Text = clsPmpaType.TRPG.TotAmt5[11].ToString();
                        ssList1_Sheet1.Cells[i, 41].Text = (clsPmpaType.TRPG.TotAmt2[11] + clsPmpaType.TRPG.TotAmt4[11]).ToString();

                        //CT
                        ssList1_Sheet1.Cells[i, 42].Text = clsPmpaType.TRPG.TotAmt6[17].ToString();
                        ssList1_Sheet1.Cells[i, 43].Text = clsPmpaType.TRPG.TotAmt5[17].ToString();
                        ssList1_Sheet1.Cells[i, 44].Text = (clsPmpaType.TRPG.TotAmt2[17] + clsPmpaType.TRPG.TotAmt4[11]).ToString();

                        //MRI
                        ssList1_Sheet1.Cells[i, 45].Text = clsPmpaType.TRPG.TotAmt6[18].ToString();
                        ssList1_Sheet1.Cells[i, 46].Text = clsPmpaType.TRPG.TotAmt5[18].ToString();
                        ssList1_Sheet1.Cells[i, 47].Text = (clsPmpaType.TRPG.TotAmt2[18] + clsPmpaType.TRPG.TotAmt4[18]).ToString();

                        //초음파
                        ssList1_Sheet1.Cells[i, 48].Text = clsPmpaType.TRPG.TotAmt6[19].ToString();
                        ssList1_Sheet1.Cells[i, 49].Text = clsPmpaType.TRPG.TotAmt5[19].ToString();
                        ssList1_Sheet1.Cells[i, 50].Text = (clsPmpaType.TRPG.TotAmt2[19] + clsPmpaType.TRPG.TotAmt4[19]).ToString();

                        //식대,실료
                        ssList1_Sheet1.Cells[i, 51].Text = (clsPmpaType.TRPG.TotAmt6[3] + clsPmpaType.TRPG.TotAmt6[21]).ToString();
                        ssList1_Sheet1.Cells[i, 52].Text = (clsPmpaType.TRPG.TotAmt5[3] + clsPmpaType.TRPG.TotAmt5[21]).ToString();
                        ssList1_Sheet1.Cells[i, 53].Text = (clsPmpaType.TRPG.TotAmt2[3] + clsPmpaType.TRPG.TotAmt4[3] + clsPmpaType.TRPG.TotAmt2[21] + clsPmpaType.TRPG.TotAmt4[21]).ToString();

                        //증명료
                        ssList1_Sheet1.Cells[i, 54].Text = clsPmpaType.TRPG.TotAmt6[22].ToString();
                        ssList1_Sheet1.Cells[i, 55].Text = clsPmpaType.TRPG.TotAmt5[22].ToString();
                        ssList1_Sheet1.Cells[i, 56].Text = (clsPmpaType.TRPG.TotAmt2[22] + clsPmpaType.TRPG.TotAmt4[22]).ToString();

                        //기타
                        ssList1_Sheet1.Cells[i, 57].Text = (clsPmpaType.TRPG.TotAmt6[30] + clsPmpaType.TRPG.TotAmt6[13] + clsPmpaType.TRPG.TotAmt6[16] + clsPmpaType.TRPG.TotAmt6[20]).ToString();
                        ssList1_Sheet1.Cells[i, 58].Text = (clsPmpaType.TRPG.TotAmt5[30] + clsPmpaType.TRPG.TotAmt5[13] + clsPmpaType.TRPG.TotAmt5[16] + clsPmpaType.TRPG.TotAmt5[20]).ToString();
                        ssList1_Sheet1.Cells[i, 59].Text = (clsPmpaType.TRPG.TotAmt2[30] + clsPmpaType.TRPG.TotAmt4[30] + clsPmpaType.TRPG.TotAmt2[13] + clsPmpaType.TRPG.TotAmt4[13] + clsPmpaType.TRPG.TotAmt2[16] + clsPmpaType.TRPG.TotAmt4[16] + clsPmpaType.TRPG.TotAmt2[20] + clsPmpaType.TRPG.TotAmt4[20]).ToString();




                        ssList1_Sheet1.Cells[i, 60].Text = data[47];    //급여공단합
                        ssList1_Sheet1.Cells[i, 61].Text = data[46];    //급여본인합
                        ssList1_Sheet1.Cells[i, 62].Text = data[45];    //비급여합

                        ssList1_Sheet1.Cells[i, 63].Text = data[46];    //급여본인
                        ssList1_Sheet1.Cells[i, 64].Text = data[47];    //급여공단합
                        ssList1_Sheet1.Cells[i, 65].Text = data[48];    //진료비총액
                        ssList1_Sheet1.Cells[i, 66].Text = (VB.Val(data[46]) + VB.Val(data[45])).ToString();    //환자부담총액

                        ssList1_Sheet1.Cells[i, 67].Text = data[50];    //감액

                        if (chkMisu.Checked == true)
                        {
                            ssList1_Sheet1.Cells[i, 68].Text = (VB.Val(data[51]) - nMisuTo2).ToString();            //미수
                            ssList1_Sheet1.Cells[i, 69].Text = (VB.Val(data[52]) - n물리치료바우처).ToString();     //수납금
                        }

                        else
                        {
                            ssList1_Sheet1.Cells[i, 68].Text = VB.Val(data[51]).ToString();    //미수
                            if (READ_MISU_GYEPANO(txtPano.Text.Trim(), dtpTDate.Text, strDept, strBi) == "OK")
                            {
                                data[52] = (VB.Val(data[52]) - nMisu + nBonAmt).ToString();
                            }
                            ssList1_Sheet1.Cells[i, 69].Text = (VB.Val(data[52]) - n물리치료바우처).ToString();     //수납금
                        }

                        ssList1_Sheet1.Cells[i, 70].Text = VB.Val(data[53]).ToString(); //희귀
                        ssList1_Sheet1.Cells[i, 71].Text = data[54];                    //선택
                        ssList1_Sheet1.Cells[i, 72].Text = n물리치료바우처.ToString();  //물리바우처

                        ssList1_Sheet1.Cells[i, 77].Text =( clsPmpaType.TRPG.TotAmt8[50]).ToString(); ; //선별 공단           
                        ssList1_Sheet1.Cells[i, 78].Text =( clsPmpaType.TRPG.TotAmt9[50]).ToString(); ; //선별 본인
                    }

                    #endregion READ_OPD_SLIP(GoSub) End
                }
                else
                {
                    #region READ_IPD_SLIP(GoSub)

                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT";
                    SQL += ComNum.VBLF + "      M.IPDNO, M.LASTTRS, T.TRSNO, M.PANO, M.SNAME, T.DEPTCODE,";
                    SQL += ComNum.VBLF + "      TO_CHAR(T.INDATE,'YYYY.MM.DD') INDATE,";
                    SQL += ComNum.VBLF + "      TO_CHAR(T.OUTDATE,'YYYY.MM.DD') OUTDATE,";
                    SQL += ComNum.VBLF + "      TO_CHAR(T.INDATE,'YYYY-MM-DD') INDATE2,";
                    SQL += ComNum.VBLF + "      TO_CHAR(T.OUTDATE,'YYYY-MM-DD') OUTDATE2,";
                    SQL += ComNum.VBLF + "      TO_CHAR(T.actDATE,'YYYY.MM.DD') actDATE, T.ILSU, T.BI, M.ILLCODE1, ";
                    SQL += ComNum.VBLF + "      M.ILLCODE2, M.ILLCODE3, M.ILLCODE4, TO_CHAR(M.ACTDATE,'YYYY-MM-DD') ACTDATE,";
                    SQL += ComNum.VBLF + "      T.AMT50,T.AMT52, T.AMT53, T.AMT56, T.AMT57, T.AMT54,T.AMT64,T.AMT44,T.GBDRG,T.DRGCODE ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER M, " + ComNum.DB_PMPA + "IPD_TRANS T";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "      AND T.ACTDATE >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "      AND T.ACTDATE <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "      AND M.PANO = '" + txtPano.Text + "' ";
                    SQL += ComNum.VBLF + "      AND T.GBIPD <> 'D' ";
                    SQL += ComNum.VBLF + "      AND M.IPDNO = T.IPDNO ";

                    if (cboDept.SelectedItem.ToString() != "전체")
                    {
                        SQL += ComNum.VBLF + "  AND T.DeptCode = '" + cboDept.SelectedItem.ToString() + "'";
                    }

                    if (cboBi.SelectedItem.ToString() != "전체")
                    {
                        SQL += ComNum.VBLF + "  AND T.BI  = '" + cboBi.SelectedItem.ToString() + "'";
                    }

                    SQL += ComNum.VBLF + "ORDER BY M.PANO, M.BI, M.ACTDATE";

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
                        nREAD = dt.Rows.Count;
                        ssList1_Sheet1.Rows.Count = nREAD + 1;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            for (j = 1; j <= 57; j++)
                            {
                                data[j] = "";
                            }

                            clsPmpaType.TRPG.TotAmt1 = new long[51];
                            clsPmpaType.TRPG.TotAmt2 = new long[51];
                            clsPmpaType.TRPG.TotAmt3 = new long[51];
                            clsPmpaType.TRPG.TotAmt4 = new long[51];
                            clsPmpaType.TRPG.TotAmt5 = new long[51];
                            clsPmpaType.TRPG.TotAmt6 = new long[51];
                            clsPmpaType.TRPG.TotAmt7 = new long[51];
                            clsPmpaType.TRPG.TotAmt8 = new long[51];
                            clsPmpaType.TRPG.TotAmt9 = new long[51];

                            for (j = 0; j <= 50; j++)
                            {
                                clsPmpaType.TRPG.TotAmt1[j] = 0;
                                clsPmpaType.TRPG.TotAmt2[j] = 0;
                                clsPmpaType.TRPG.TotAmt3[j] = 0;
                                clsPmpaType.TRPG.TotAmt4[j] = 0;
                                clsPmpaType.TRPG.TotAmt5[j] = 0;
                                clsPmpaType.TRPG.TotAmt6[j] = 0;
                                clsPmpaType.TRPG.TotAmt7[j] = 0;
                                clsPmpaType.TRPG.TotAmt8[j] = 0;
                                clsPmpaType.TRPG.TotAmt9[j] = 0;
                            }

                            //항목별 합
                            clsPmpaType.RPG.Amt1 = new long[51]; //급여합
                            clsPmpaType.RPG.Amt2 = new long[51]; //비급여합
                            clsPmpaType.RPG.Amt3 = new long[51]; //특진합
                            clsPmpaType.RPG.Amt4 = new long[51]; //본인총액합
                            clsPmpaType.RPG.Amt5 = new long[51]; //본인
                            clsPmpaType.RPG.Amt6 = new long[51]; //공단
                            clsPmpaType.RPG.Amt7 = new long[51]; //선별급여 총액
                            clsPmpaType.RPG.Amt8 = new long[51]; //선별급여 조합
                            clsPmpaType.RPG.Amt9 = new long[51]; //선별급여 본인
                            for (j = 0; j <= 50; j++)
                            {
                                clsPmpaType.RPG.Amt1[j] = 0; //급여합
                                clsPmpaType.RPG.Amt2[j] = 0; //비급여합
                                clsPmpaType.RPG.Amt3[j] = 0; //특진합
                                clsPmpaType.RPG.Amt4[j] = 0; //본인총액합
                                clsPmpaType.RPG.Amt5[j] = 0; //본인
                                clsPmpaType.RPG.Amt6[j] = 0; //공단
                                clsPmpaType.RPG.Amt7[j] = 0; //선별급여 총액
                                clsPmpaType.RPG.Amt8[j] = 0; //선별급여 조합
                                clsPmpaType.RPG.Amt9[j] = 0; //선별급여 본인
                            }

                            strBi = dt.Rows[i]["BI"].ToString().Trim();
                            strDept = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                            nTRSNo = (long)VB.Val(dt.Rows[i]["Trsno"].ToString().Trim());
                            nIPDNO = (long)VB.Val(dt.Rows[i]["IPDNO"].ToString().Trim());
                            strDrgCode = dt.Rows[i]["DRGCODE"].ToString().Trim();

                            strInDate = VB.Left(dt.Rows[i]["InDate2"].ToString().Trim(), 4) + "-" + VB.Mid(dt.Rows[i]["InDate2"].ToString().Trim(), 6, 2) + "-" + VB.Right(dt.Rows[i]["InDate2"].ToString().Trim(), 2);
                            strOutDate = VB.Left(dt.Rows[i]["OutDate2"].ToString().Trim(), 4) + "-" + VB.Mid(dt.Rows[i]["OutDate2"].ToString().Trim(), 6, 2) + "-" + VB.Right(dt.Rows[i]["OutDate2"].ToString().Trim(), 2);

                            clsPmpaPb.Gstr누적계산New = "OK";

                            clsIument CI = new clsIument();

                            CI.Ipd_Trans_PrtAmt_Read_New_Junsan(clsDB.DbCon, Convert.ToInt64(nTRSNo), "");
                            CIT.Ipd_Tewon_PrtAmt_Gesan_Junsan(clsDB.DbCon, txtPano.Text, nIPDNO, nTRSNo, "");

                            if (dt.Rows[i]["GBDRG"].ToString().Trim() != "D")
                            {
                                Ipd_Slip_Amt_Set();
                            }

                            SQL = "";
                            SQL += ComNum.VBLF + "SELECT";
                            SQL += ComNum.VBLF + "  BUN , GBSELF, SUM(AMT1) SAMT,SUM(AMT2) SAMT2";
                            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP";
                            SQL += ComNum.VBLF + "WHERE 1=1";
                            SQL += ComNum.VBLF + "      AND PANO = '" + txtPano.Text + "'";
                            SQL += ComNum.VBLF + "      AND BI = '" + strBi + "'";
                            SQL += ComNum.VBLF + "      AND TRSNO = " + dt.Rows[i]["TRSNO"].ToString().Trim() + " ";
                            SQL += ComNum.VBLF + "      AND TRIM(SUNEXT) NOT IN ( SELECT CODE FROM ADMIN.BAS_BCODE WHERE GUBUN ='원무영수제외코드')"; //저가약제 제외코드 2011-04-01
                            SQL += ComNum.VBLF + "GROUP BY BUN, GBSELF";
                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }


                            for (j = 0; j < dt1.Rows.Count; j++)
                            {
                                strBun = dt1.Rows[j]["BUN"].ToString().Trim();
                                strSelf = dt1.Rows[j]["GBSELF"].ToString().Trim();

                                #region BUN_MOVE(GoSub)

                                if (strSelf != "0")
                                {
                                    switch (strBun)
                                    {
                                        case "01":
                                        case "02":
                                            nNUM = 13;  //진찰료
                                            break;

                                        case "03":
                                        case "04":
                                        case "05":
                                        case "06":
                                        case "07":
                                        case "08":
                                        case "09":
                                        case "10":
                                            nNUM = 15;  //입원료
                                            break;

                                        case "11":
                                        case "12":
                                        case "13":
                                        case "14":
                                        case "15":
                                            nNUM = 17;  //투약료 및 조제료
                                            break;

                                        case "16":
                                        case "17":
                                        case "18":
                                        case "19":
                                        case "20":
                                        case "21":
                                            nNUM = 19;  //주사료
                                            break;

                                        case "22":
                                        case "23":
                                            nNUM = 21;  //마취료
                                            break;

                                        case "24":
                                        case "25":
                                        case "26":
                                        case "27":
                                            nNUM = 23;  //이학요법료(물리치료) 정신요법료
                                            break;

                                        case "28":
                                        case "29":
                                        case "30":
                                        case "31":
                                        case "32":
                                        case "33":
                                            nNUM = 25;  //처치
                                            break;

                                        case "34":
                                        case "35":
                                        case "36":
                                        case "37":
                                        case "38":
                                        case "39":
                                        case "40":
                                            nNUM = 27;  //수술
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
                                            nNUM = 29;  //검사료
                                            break;

                                        case "65":
                                        case "66":
                                        case "67":
                                        case "68":
                                        case "69":
                                        case "70":
                                            nNUM = 31;  //방사선료
                                            break;

                                        case "72":
                                            if (strBi == "43" || strBi == "42" || strBi == "41")
                                            {
                                                nNUM = 33;  //CT
                                            }

                                            else if (String.Compare(strBi, "11") >= 0 && String.Compare(strBi, "22") <= 0)
                                            {
                                                nNUM = 33;  //CT
                                            }

                                            else
                                            {
                                                nNUM = 32;
                                            }
                                            break;

                                        case "73":
                                            nNUM = 35;  //MRI
                                            break;

                                        case "71":
                                            nNUM = 37;  //초음파
                                            break;

                                        case "74":
                                        case "77":
                                            nNUM = 39;  //식대, 실료차
                                            break;

                                        case "75":
                                            nNUM = 41;  //증명료
                                            break;

                                        case "99":
                                            nNUM = 52;  //영수액
                                            break;

                                        case "98":
                                            nNUM = 47;  //조합부담액
                                            break;

                                        case "92":
                                            nNUM = 50;  //감액
                                            break;

                                        case "96":
                                            nNUM = 51;  //미수액
                                            break;

                                        default:
                                            nNUM = 43;  //비급여기타
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (strBun)
                                    {
                                        case "01":
                                        case "02":
                                            nNUM = 12;  //진찰료
                                            break;

                                        case "03":
                                        case "04":
                                        case "05":
                                        case "06":
                                        case "07":
                                        case "08":
                                        case "09":
                                        case "10":
                                            nNUM = 14;  //입원료
                                            break;

                                        case "11":
                                        case "12":
                                        case "13":
                                        case "14":
                                        case "15":
                                            nNUM = 16;  //투약료 및 조제료
                                            break;

                                        case "16":
                                        case "17":
                                        case "18":
                                        case "19":
                                        case "20":
                                        case "21":
                                            nNUM = 18;  //주사료
                                            break;

                                        case "22":
                                        case "23":
                                            nNUM = 20;  //마취료
                                            break;

                                        case "24":
                                        case "25":
                                        case "26":
                                        case "27":
                                            nNUM = 22;  //이학요법료(물리치료) 정신요법료
                                            break;

                                        case "28":
                                        case "29":
                                        case "30":
                                        case "31":
                                        case "32":
                                        case "33":
                                            nNUM = 24;  //처치
                                            break;

                                        case "34":
                                        case "35":
                                        case "36":
                                        case "37":
                                        case "38":
                                        case "39":
                                        case "40":
                                            nNUM = 26;  //수술
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
                                            nNUM = 28;  //검사료
                                            break;

                                        case "65":
                                        case "66":
                                        case "67":
                                        case "68":
                                        case "69":
                                        case "70":
                                            nNUM = 30;  //방사선료
                                            break;

                                        case "72":
                                            if (strBi == "43" || strBi == "42" || strBi == "41")
                                            {
                                                nNUM = 33;  //CT
                                            }

                                            else
                                            {
                                                nNUM = 32;
                                            }
                                            break;

                                        case "73":
                                            nNUM = 34;  //MRI
                                            break;

                                        case "71":
                                            nNUM = 36;  //초음파
                                            break;

                                        case "74":
                                        case "77":
                                            nNUM = 38;  //식대, 실료차
                                            break;

                                        case "75":
                                            nNUM = 40;  //증명료
                                            break;

                                        case "99":
                                            nNUM = 52;  //영수액
                                            break;

                                        case "98":
                                            nNUM = 47;  //조합부담액
                                            break;

                                        case "92":
                                            nNUM = 50;  //감액
                                            break;

                                        case "96":
                                            nNUM = 51;  //미수액
                                            break;

                                        default:
                                            nNUM = 42;  //비급여기타
                                            break;
                                    }
                                }

                                #endregion BUN_MOVE(GoSub) End

                                data[nNUM] = (VB.Val(data[nNUM]) + VB.Val(dt1.Rows[j]["SAMT"].ToString().Trim())).ToString();

                                if (nNUM <= 43)
                                {
                                    if (strSelf == "0")
                                    {
                                        data[44] = (VB.Val(data[44]) + VB.Val(dt1.Rows[j]["SAMT"].ToString().Trim())).ToString();
                                    }

                                    else
                                    {
                                        data[45] = (VB.Val(data[45]) + VB.Val(dt1.Rows[j]["SAMT"].ToString().Trim())).ToString();
                                    }
                                }
                            }

                            nMisuTo = 0;

                            data[47] = dt.Rows[i]["AMT53"].ToString().Trim();   //공단부담금
                            data[48] = (VB.Val(dt.Rows[i]["AMT50"].ToString().Trim()) - VB.Val(dt.Rows[i]["amt64"].ToString().Trim())).ToString();  //진료비총액
                            data[50] = dt.Rows[i]["AMT54"].ToString().Trim();               //진료비감액
                            data[51] = dt.Rows[i]["AMT56"].ToString().Trim();               //미수금액
                            data[53] = dt.Rows[i]["AMT52"].ToString().Trim();               //희귀난치지원금 2010-02-26
                            data[54] = dt.Rows[i]["AMT44"].ToString().Trim();               //선택진료비 2011-06-16
                            data[45] = (VB.Val(data[45]) + VB.Val(data[54])).ToString();    //비급여총액에 선택포함 2011-06-17

                            if (chkMisu.Checked == true)
                            {
                                data[46] = (VB.Val(data[48]) - VB.Val(data[47]) + VB.Val(data[45]) + nMisuTo).ToString();   //본인부담금
                            }

                            else
                            {
                                data[46] = (VB.Val(data[48]) - VB.Val(data[47]) - VB.Val(data[45])).ToString();             //본인부담금
                            }

                            data[49] = (VB.Val(data[46]) + VB.Val(data[45])).ToString();
                            data[52] = (VB.Val(data[49]) - VB.Val(data[50]) - VB.Val(data[51])).ToString();

                            #region //DRG
                            if (dt.Rows[i]["GBDRG"].ToString().Trim() == "D")
                            {
                                nDrgBiAmt = 0;

                                DRG DRG = new DRG();
                                strNgt = DRG.Read_GbNgt_DRG(clsDB.DbCon, txtPano.Text.Trim(), Convert.ToInt64(nIPDNO), Convert.ToInt64(nTRSNo));

                                DRG.READ_DRG_AMT_MASTER(clsDB.DbCon, strDrgCode, clsPmpaType.TIT.Pano, nIPDNO, nTRSNo, strNgt, strInDate, strOutDate);
                                CIT.Ipd_Trans_PrtAmt_Read_Drg(clsDB.DbCon, nTRSNo);

                                data[44] = "0";
                                data[45] = "0";

                                //비급여에 선택진료 포함
                                data[45] = data[54];

                                //본인부담금 = 급여 본인부담금 +  비급여총액 (원단위반올림)
                                nBoninTAmt = Convert.ToInt32(DRG.GnDrgBonAmt + DRG.GnDrgBiTAmt + DRG.GnDrgFoodAmt[0] + DRG.GnDrgRoomAmt[0] + DRG.GnGs100Amt + DRG.GnGs80Amt_B + DRG.GnGs50Amt_B);
                                nBoninTAmt = CF.FIX_N(nBoninTAmt / 10) * 10;    //절사

                                //포괄수가조합부담 금액안에는 병실차액/식대료가 빠져있음
                                //2014-12-29 nBoninTAmt 금액에는 GnDrgRoomAmt(2), GnDrgRoomAmt(3) 금액이 포함되어 있으므로 제외함
                                //data(47) = GnDRG_TAmt - nBoninTAmt - GnDrgFoodAmt(1) - GnDrgRoomAmt(1) - GnDrgRoomAmt(2) - GnDrgRoomAmt(3)
                                data[47] = (DRG.GnDRG_TAmt - Convert.ToInt64(nBoninTAmt) - DRG.GnDrgFoodAmt[1] - DRG.GnDrgRoomAmt[1] - DRG.GnGs80Amt_J - DRG.GnGs50Amt_J).ToString();
                                data[48] = DRG.GnDRG_TAmt.ToString();

                                if (chkMisu.Checked == true)
                                {
                                    data[46] = (DRG.GnDrgBonAmt + Convert.ToInt64(nMisuTo)).ToString();
                                }

                                else
                                {
                                    data[46] = DRG.GnDrgBonAmt.ToString();
                                }

                                Ipd_Slip_Amt_Set_DRG();

                                int nxx = 0;

                                for (nxx = 1; nxx <= 49; nxx++)
                                {
                                    //DRG 비급여 총액에 병실료차액 더블되어 막음
                                    if (nxx != 3 && nxx != 21)
                                    {
                                        nDrgBiAmt += clsPmpaType.RPG.Amt2[nxx];
                                        nDrgBiAmt += clsPmpaType.RPG.Amt4[nxx];
                                    }
                                }
                            }
                            #endregion //DRG

                            dt1.Dispose();
                            dt1 = null;

                            for (j = 1; j <= 4; j++)
                            {
                                #region //에러남 무조건 ""
                                //string strDum = "";
                                //strDum = "ILLCODE" + j;

                                //if (dt.Rows[i]["strDum"].ToString().Trim() != "")
                                //{
                                //    data[8 + j - 1] = VB.Left(dt.Rows[i]["strDum"].ToString().Trim() + VB.Space(10), 10) + CF.Read_IllsName(clsDB.DbCon, dt.Rows[i]["strDum"].ToString().Trim(), "2");
                                //}
                                #endregion //에러남 무조건 ""
                            }

                            data[4] = dt.Rows[i]["INDATE"].ToString().Trim() + " " + dt.Rows[i]["OUTDATE"].ToString().Trim();
                            data[5] = dt.Rows[i]["ILSU"].ToString().Trim();

                            #region //미수입금내역조회
                            if (chkMisu.Checked == true)
                            {
                                
                                SQL = "";
                                SQL += ComNum.VBLF + "SELECT";
                                SQL += ComNum.VBLF + "  SUM(AMT) SAMT ";
                                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP";
                                SQL += ComNum.VBLF + "WHERE 1=1";
                                SQL += ComNum.VBLF + "      AND BDATE = TO_DATE('" + dt.Rows[i]["ActDAte"].ToString().Trim() + "','YYYY-MM-DD')";
                                SQL += ComNum.VBLF + "      AND PANO = '" + txtPano.Text + "'";
                                SQL += ComNum.VBLF + "      AND GUBUN1 in ('2','4')";
                                SQL += ComNum.VBLF + "      AND SUBSTR(MISUDTL,1,1) = 'I'"; //입원
                                SQL += ComNum.VBLF + "      AND SUBSTR(MISUDTL,2,2) = '" + strDept + "'";   //과


                                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    return;
                                }

                                if (dt2.Rows.Count > 0)
                                {
                                    data[51] = (VB.Val(data[51]) - VB.Val(dt2.Rows[0]["SAMT"].ToString().Trim())).ToString();
                                    nMisuTo += VB.Val(dt2.Rows[0]["SAMT"].ToString().Trim());
                                }

                                dt2.Dispose();
                                dt2 = null;
                            }
                            #endregion //미수입금내역조회

                            #region //DISPLAY
                            ssList1_Sheet1.Cells[i, 1].Text = txtPano.Text;                                             //환자등록번호
                            ssList1_Sheet1.Cells[i, 2].Text = clsPmpaType.TBP.Sname;                                    //환자성명
                            ssList1_Sheet1.Cells[i, 3].Text = CF.READ_DEPTNAMEK(clsDB.DbCon, strDept);                               //진료과목
                            ssList1_Sheet1.Cells[i, 4].Text = data[4];                                                  //진료기간
                            ssList1_Sheet1.Cells[i, 5].Text = data[5];                                                  //진료일수
                            ssList1_Sheet1.Cells[i, 6].Text = clsPmpaType.TBP.Jumin1 + "-" + clsPmpaType.TBP.Jumin2;    //주민등록번호
                            ssList1_Sheet1.Cells[i, 7].Text = CF.Read_Bi_Name(clsDB.DbCon, strBi, "2");                              //보험유형

                            for (j = 9; j <= 12; j++)
                            {
                                ssList1_Sheet1.Cells[i, j].Text = data[j - 1];
                            }

                            //진찰료
                            ssList1_Sheet1.Cells[i, 12].Text = clsPmpaType.TRPG.TotAmt6[1].ToString();
                            ssList1_Sheet1.Cells[i, 13].Text = clsPmpaType.TRPG.TotAmt5[1].ToString();
                            ssList1_Sheet1.Cells[i, 14].Text = (clsPmpaType.TRPG.TotAmt2[1] + clsPmpaType.TRPG.TotAmt4[1]).ToString();

                            //입원료
                            ssList1_Sheet1.Cells[i, 15].Text = clsPmpaType.TRPG.TotAmt6[2].ToString();
                            ssList1_Sheet1.Cells[i, 16].Text = clsPmpaType.TRPG.TotAmt5[2].ToString();
                            ssList1_Sheet1.Cells[i, 17].Text = (clsPmpaType.TRPG.TotAmt2[2] + clsPmpaType.TRPG.TotAmt4[2]).ToString();

                            //투약료
                            ssList1_Sheet1.Cells[i, 18].Text = (clsPmpaType.TRPG.TotAmt6[4] + clsPmpaType.TRPG.TotAmt6[5]).ToString();
                            ssList1_Sheet1.Cells[i, 19].Text = (clsPmpaType.TRPG.TotAmt5[4] + clsPmpaType.TRPG.TotAmt5[5]).ToString();
                            ssList1_Sheet1.Cells[i, 20].Text = (clsPmpaType.TRPG.TotAmt2[4] + clsPmpaType.TRPG.TotAmt4[4] + clsPmpaType.TRPG.TotAmt2[5] + clsPmpaType.TRPG.TotAmt4[5]).ToString();

                            //주사료
                            ssList1_Sheet1.Cells[i, 21].Text = (clsPmpaType.TRPG.TotAmt6[6] + clsPmpaType.TRPG.TotAmt6[7]).ToString();
                            ssList1_Sheet1.Cells[i, 22].Text = (clsPmpaType.TRPG.TotAmt5[6] + clsPmpaType.TRPG.TotAmt5[7]).ToString();
                            ssList1_Sheet1.Cells[i, 23].Text = (clsPmpaType.TRPG.TotAmt2[6] + clsPmpaType.TRPG.TotAmt4[6] + clsPmpaType.TRPG.TotAmt2[7] + clsPmpaType.TRPG.TotAmt4[7]).ToString();

                            //마취료
                            ssList1_Sheet1.Cells[i, 24].Text = clsPmpaType.TRPG.TotAmt6[8].ToString();
                            ssList1_Sheet1.Cells[i, 25].Text = clsPmpaType.TRPG.TotAmt5[8].ToString();
                            ssList1_Sheet1.Cells[i, 26].Text = (clsPmpaType.TRPG.TotAmt2[8] + clsPmpaType.TRPG.TotAmt4[8]).ToString();

                            //물리,정신
                            ssList1_Sheet1.Cells[i, 27].Text = (clsPmpaType.TRPG.TotAmt6[14] + clsPmpaType.TRPG.TotAmt6[15]).ToString();
                            ssList1_Sheet1.Cells[i, 28].Text = (clsPmpaType.TRPG.TotAmt5[14] + clsPmpaType.TRPG.TotAmt5[15]).ToString();
                            ssList1_Sheet1.Cells[i, 29].Text = (clsPmpaType.TRPG.TotAmt2[14] + clsPmpaType.TRPG.TotAmt4[14] + clsPmpaType.TRPG.TotAmt2[15] + clsPmpaType.TRPG.TotAmt4[15]).ToString();

                            //처치
                            ssList1_Sheet1.Cells[i, 30].Text = clsPmpaType.TRPG.TotAmt6[9].ToString();
                            ssList1_Sheet1.Cells[i, 31].Text = clsPmpaType.TRPG.TotAmt5[9].ToString();
                            ssList1_Sheet1.Cells[i, 32].Text = (clsPmpaType.TRPG.TotAmt2[9] + clsPmpaType.TRPG.TotAmt4[9]).ToString();

                            //수술
                            ssList1_Sheet1.Cells[i, 33].Text = clsPmpaType.TRPG.TotAmt6[12].ToString();
                            ssList1_Sheet1.Cells[i, 34].Text = clsPmpaType.TRPG.TotAmt5[12].ToString();
                            ssList1_Sheet1.Cells[i, 35].Text = (clsPmpaType.TRPG.TotAmt2[12] + clsPmpaType.TRPG.TotAmt4[12]).ToString();

                            //검사
                            ssList1_Sheet1.Cells[i, 36].Text = clsPmpaType.TRPG.TotAmt6[10].ToString();
                            ssList1_Sheet1.Cells[i, 37].Text = clsPmpaType.TRPG.TotAmt5[10].ToString();
                            ssList1_Sheet1.Cells[i, 38].Text = (clsPmpaType.TRPG.TotAmt2[10] + clsPmpaType.TRPG.TotAmt4[10]).ToString();

                            //방사선
                            ssList1_Sheet1.Cells[i, 39].Text = clsPmpaType.TRPG.TotAmt6[11].ToString();
                            ssList1_Sheet1.Cells[i, 40].Text = clsPmpaType.TRPG.TotAmt5[11].ToString();
                            ssList1_Sheet1.Cells[i, 41].Text = (clsPmpaType.TRPG.TotAmt2[11] + clsPmpaType.TRPG.TotAmt4[11]).ToString();

                            //CT
                            ssList1_Sheet1.Cells[i, 42].Text = clsPmpaType.TRPG.TotAmt6[17].ToString();
                            ssList1_Sheet1.Cells[i, 43].Text = clsPmpaType.TRPG.TotAmt5[17].ToString();
                            ssList1_Sheet1.Cells[i, 44].Text = (clsPmpaType.TRPG.TotAmt2[17] + clsPmpaType.TRPG.TotAmt4[17]).ToString();

                            //MRI
                            ssList1_Sheet1.Cells[i, 45].Text = clsPmpaType.TRPG.TotAmt6[18].ToString();
                            ssList1_Sheet1.Cells[i, 46].Text = clsPmpaType.TRPG.TotAmt5[18].ToString();
                            ssList1_Sheet1.Cells[i, 47].Text = (clsPmpaType.TRPG.TotAmt2[18] + clsPmpaType.TRPG.TotAmt4[18]).ToString();

                            //초음파
                            ssList1_Sheet1.Cells[i, 48].Text = clsPmpaType.TRPG.TotAmt6[19].ToString();
                            ssList1_Sheet1.Cells[i, 49].Text = clsPmpaType.TRPG.TotAmt5[19].ToString();
                            ssList1_Sheet1.Cells[i, 50].Text = (clsPmpaType.TRPG.TotAmt2[19] + clsPmpaType.TRPG.TotAmt4[19]).ToString();

                            //식대,실료
                            ssList1_Sheet1.Cells[i, 51].Text = (clsPmpaType.TRPG.TotAmt6[3] + clsPmpaType.TRPG.TotAmt6[21]).ToString();
                            ssList1_Sheet1.Cells[i, 52].Text = (clsPmpaType.TRPG.TotAmt5[3] + clsPmpaType.TRPG.TotAmt5[21]).ToString();
                            ssList1_Sheet1.Cells[i, 53].Text = (clsPmpaType.TRPG.TotAmt2[3] + clsPmpaType.TRPG.TotAmt4[3] + clsPmpaType.TRPG.TotAmt2[21] + clsPmpaType.TRPG.TotAmt4[21]).ToString();

                            //증명료
                            ssList1_Sheet1.Cells[i, 54].Text = clsPmpaType.TRPG.TotAmt6[22].ToString();
                            ssList1_Sheet1.Cells[i, 55].Text = clsPmpaType.TRPG.TotAmt5[22].ToString();
                            ssList1_Sheet1.Cells[i, 56].Text = (clsPmpaType.TRPG.TotAmt2[22] + clsPmpaType.TRPG.TotAmt4[22]).ToString();

                            //기타
                            ssList1_Sheet1.Cells[i, 57].Text = (clsPmpaType.TRPG.TotAmt6[49] + clsPmpaType.TRPG.TotAmt6[13] + clsPmpaType.TRPG.TotAmt6[16]).ToString();
                            ssList1_Sheet1.Cells[i, 58].Text = (clsPmpaType.TRPG.TotAmt5[49] + clsPmpaType.TRPG.TotAmt5[13] + clsPmpaType.TRPG.TotAmt5[16]).ToString();
                            ssList1_Sheet1.Cells[i, 59].Text = (clsPmpaType.TRPG.TotAmt2[49] + clsPmpaType.TRPG.TotAmt4[49] + clsPmpaType.TRPG.TotAmt2[13] + clsPmpaType.TRPG.TotAmt4[13] + clsPmpaType.TRPG.TotAmt2[16] + clsPmpaType.TRPG.TotAmt4[16]).ToString();

                            ssList1_Sheet1.Cells[i, 74].Text = nSangAmt.ToString();

                            ssList1_Sheet1.Cells[i, 77].Text = (DRG.GnGs80Amt_J + DRG.GnGs50Amt_J + clsPmpaType.TRPG.TotAmt8[50]).ToString();   //선별 공단 GnGs100Amt
                            ssList1_Sheet1.Cells[i, 78].Text = (DRG.GnGs80Amt_B + DRG.GnGs50Amt_B + clsPmpaType.TRPG.TotAmt9[50]).ToString();   //선별 본인

                            //DRG
                            if (dt.Rows[i]["GBDRG"].ToString().Trim() == "D")
                            {
                                nDrgBonAmt = DRG.GnDrgBonAmt +  DRG.GnDrgFoodAmt[0] + DRG.GnDrgFoodAmt[2];
                                nDrgBonAmt = nDrgBonAmt +  DRG.GnDrgRoomAmt[0] + DRG.GnDrgRoomAmt[2];

                                nDrgBonBiAmt = DRG.GnDrgFoodAmt[3] + DRG.GnDrgFoodAmt[4] + DRG.GnDrgRoomAmt[3] + DRG.GnDrgRoomAmt[4];

                                ssList1_Sheet1.Cells[i, 74].Text = data[47];
                                ssList1_Sheet1.Cells[i, 75].Text = data[46];
                                ssList1_Sheet1.Cells[i, 76].Text = "0";

                                //합계
                                ssList1_Sheet1.Cells[i, 60].Text = (Convert.ToInt64(VB.Val(data[47])) + DRG.GnDrgFoodAmt[1] + DRG.GnDrgRoomAmt[1] + DRG.GnGs80Amt_J + DRG.GnGs50Amt_J).ToString();    //급여공단합

                                ssList1_Sheet1.Cells[i, 61].Text = (Convert.ToInt64(nDrgBonAmt) + DRG.GnGs80Amt_B + DRG.GnGs50Amt_B).ToString();    //급여본인합

                                ssList1_Sheet1.Cells[i, 62].Text = (VB.Val(data[45]) + nDrgBonBiAmt + nDrgBiAmt).ToString();    // 비급여합

                                ssList1_Sheet1.Cells[i, 63].Text = nDrgBonAmt.ToString();   //급여본인

                                ssList1_Sheet1.Cells[i, 64].Text = data[47];    //급여공단합
                                ssList1_Sheet1.Cells[i, 65].Text = data[48];    //진료비총액                        
                            }
                            else
                            {
                                ssList1_Sheet1.Cells[i, 60].Text = data[47];    //급여공단합
                                ssList1_Sheet1.Cells[i, 61].Text = data[46];    //급여본인합
                                ssList1_Sheet1.Cells[i, 62].Text = data[45];    //비급여합

                                ssList1_Sheet1.Cells[i, 63].Text = data[46];    //급여본인
                                ssList1_Sheet1.Cells[i, 64].Text = data[47];    //급여공단합
                                ssList1_Sheet1.Cells[i, 65].Text = data[48];    //진료비총액
                            }

                            if (dt.Rows[i]["GBDRG"].ToString().Trim() == "D")
                            {
                                ssList1_Sheet1.Cells[i, 66].Text = nBoninTAmt.ToString();
                            }

                            else
                            {
                                ssList1_Sheet1.Cells[i, 66].Text = (VB.Val(data[46]) + VB.Val(data[45])).ToString();    //환자총액
                            }

                            ssList1_Sheet1.Cells[i, 67].Text = data[50];    //감액
                            ssList1_Sheet1.Cells[i, 68].Text = data[51];    //미수

                            if (chkMisu.Checked == true)
                            {
                                if (dt.Rows[i]["GBDRG"].ToString().Trim() == "D")
                                {
                                    ssList1_Sheet1.Cells[i, 69].Text = (CF.FIX_N(VB.Val(data[52]) / 10) * 10 + nMisuTo).ToString(); // 수납금
                                }

                                else
                                {
                                    ssList1_Sheet1.Cells[i, 69].Text = (VB.Val(data[52]) + nMisuTo).ToString();  //수납금
                                }
                            }

                            else
                            {
                                if (dt.Rows[i]["GBDRG"].ToString().Trim() == "D")
                                {
                                    ssList1_Sheet1.Cells[i, 69].Text = (CF.FIX_N(VB.Val(data[52]) / 10) * 10).ToString();   //수납금
                                }

                                else
                                {
                                    ssList1_Sheet1.Cells[i, 69].Text = data[52];    //수납금
                                }
                            }

                            ssList1_Sheet1.Cells[i, 70].Text = data[53];    //희귀
                            ssList1_Sheet1.Cells[i, 71].Text = data[54];    //선택
                            #endregion //DISPLAY
                        }
                    }
                    #endregion READ_IPD_SLIP(GoSub) End
                }
            }

            if (chkManual.Checked == false)
            {
                #region TotalCount(GoSub)

                long[] njin = new long[59];

                string strPANO = "";
                string strName = "";
                string strDept1 = "";
                string strJumin = "";
                string strBoHum = "";

                string strTFDate = "";  //전체시작일자
                string strTEDate = "";  //전체종료일자

                DataTable dtTemp = null;

                strDept1 = "전체과";
                strBoHum = "전 체";

                for (i = 0; i < njin.Length; i++)
                {
                    njin[i] = 0;
                }

                //전체과 기간설정
                if (chkTerm.Checked == true && optIO1.Checked == true)
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT";
                    SQL += ComNum.VBLF + "  TO_CHAR(MIN(BDATE), 'YYYY-MM-DD') MinBDATE, TO_CHAR(MAX(BDATE), 'YYYY-MM-DD') MaxBDATE";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "      AND BDATE >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "      AND BDATE <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "      AND PANO = '" + txtPano.Text + "'";
                    if (cboDept.SelectedItem.ToString() != "전체")
                    {
                        SQL += ComNum.VBLF + "  AND DeptCode = '" + cboDept.SelectedItem.ToString() + "'";
                    }
                    if (cboBi.SelectedItem.ToString() != "전체")
                    {
                        SQL += ComNum.VBLF + "  AND BI  = '" + cboBi.SelectedItem.ToString() + "'";
                    }
                    SQL += ComNum.VBLF + "UNION";
                    SQL += ComNum.VBLF + "SELECT";
                    SQL += ComNum.VBLF + "  TO_CHAR(MIN(BDATE), 'YYYY-MM-DD') MinBDATE, TO_CHAR(MAX(BDATE), 'YYYY-MM-DD') MaxBDATE";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "      AND BDATE >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "      AND BDATE <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "      AND PANO = '" + txtPano.Text + "'";
                    if (cboDept.SelectedItem.ToString() != "전체")
                    {
                        SQL += ComNum.VBLF + "  AND DeptCode = '" + cboDept.SelectedItem.ToString() + "'";
                    }
                    if (cboBi.SelectedItem.ToString() != "전체")
                    {
                        SQL += ComNum.VBLF + "  AND BI  = '" + cboBi.SelectedItem.ToString() + "' ";
                    }
                    SqlErr = clsDB.GetDataTable(ref dtTemp, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dtTemp.Rows.Count > 0)
                    {
                        strTFDate = dtTemp.Rows[0]["MinBDate"].ToString().Trim().Replace("-", ".");
                        strTEDate = dtTemp.Rows[0]["MaxBDate"].ToString().Trim().Replace("-", ".");
                    }

                    dtTemp.Dispose();
                    dtTemp = null;
                }

                for (i = 0; i < ssList1_Sheet1.Rows.Count; i++)
                {
                    strPANO = ssList1_Sheet1.Cells[i, 1].Text;
                    strName = ssList1_Sheet1.Cells[i, 2].Text;
                    strJumin = ssList1_Sheet1.Cells[i, 6].Text;

                    if (ssList1_Sheet1.Cells[i, 12].Text != "")
                    {
                        njin[0] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 12].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 13].Text != "")
                    {
                        njin[1] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 13].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 14].Text != "")
                    {
                        njin[2] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 14].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 15].Text != "")
                    {
                        njin[3] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 15].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 16].Text != "")
                    {
                        njin[4] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 16].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 17].Text != "")
                    {
                        njin[5] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 17].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 18].Text != "")
                    {
                        njin[6] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 18].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 19].Text != "")
                    {
                        njin[7] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 19].Text));
                    }


                    if (ssList1_Sheet1.Cells[i, 20].Text != "")
                    {
                        njin[8] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 20].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 21].Text != "")
                    {
                        njin[9] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 21].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 22].Text != "")
                    {
                        njin[10] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 22].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 23].Text != "")
                    {
                        njin[11] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 23].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 24].Text != "")
                    {
                        njin[12] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 24].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 25].Text != "")
                    {
                        njin[13] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 25].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 26].Text != "")
                    {
                        njin[14] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 26].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 27].Text != "")
                    {
                        njin[15] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 27].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 28].Text != "")
                    {
                        njin[16] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 28].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 29].Text != "")
                    {
                        njin[17] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 29].Text));
                    }


                    if (ssList1_Sheet1.Cells[i, 30].Text != "")
                    {
                        njin[18] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 30].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 31].Text != "")
                    {
                        njin[19] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 31].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 32].Text != "")
                    {
                        njin[20] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 32].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 33].Text != "")
                    {
                        njin[21] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 33].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 34].Text != "")
                    {
                        njin[22] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 34].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 35].Text != "")
                    {
                        njin[23] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 35].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 36].Text != "")
                    {
                        njin[24] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 36].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 37].Text != "")
                    {
                        njin[25] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 37].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 38].Text != "")
                    {
                        njin[26] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 38].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 39].Text != "")
                    {
                        njin[27] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 39].Text));
                    }


                    if (ssList1_Sheet1.Cells[i, 40].Text != "")
                    {
                        njin[28] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 40].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 41].Text != "")
                    {
                        njin[29] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 41].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 42].Text != "")
                    {
                        njin[30] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 42].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 43].Text != "")
                    {
                        njin[31] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 43].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 44].Text != "")
                    {
                        njin[32] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 44].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 45].Text != "")
                    {
                        njin[33] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 45].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 46].Text != "")
                    {
                        njin[34] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 46].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 47].Text != "")
                    {
                        njin[35] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 47].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 48].Text != "")
                    {
                        njin[36] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 48].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 49].Text != "")
                    {
                        njin[37] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 49].Text));
                    }

                    if (ssList1_Sheet1.Cells[i, 50].Text != "")
                    {
                        njin[38] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 50].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 51].Text != "")
                    {
                        njin[39] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 51].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 52].Text != "")
                    {
                        njin[40] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 52].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 53].Text != "")
                    {
                        njin[41] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 53].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 54].Text != "")
                    {
                        njin[42] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 54].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 55].Text != "")
                    {
                        njin[43] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 55].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 56].Text != "")
                    {
                        njin[44] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 56].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 57].Text != "")
                    {
                        njin[45] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 57].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 58].Text != "")
                    {
                        njin[46] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 58].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 59].Text != "")
                    {
                        njin[47] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 59].Text));
                    }


                    if (ssList1_Sheet1.Cells[i, 60].Text != "")
                    {
                        njin[48] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 60].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 61].Text != "")
                    {
                        njin[49] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 61].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 62].Text != "")
                    {
                        njin[50] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 62].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 63].Text != "")
                    {
                        njin[51] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 63].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 64].Text != "")
                    {
                        njin[52] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 64].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 65].Text != "")
                    {
                        njin[53] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 65].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 66].Text != "")
                    {
                        njin[54] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 66].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 67].Text != "")
                    {
                        njin[55] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 67].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 68].Text != "")
                    {
                        njin[56] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 68].Text));
                    }
                    if (ssList1_Sheet1.Cells[i, 69].Text != "")
                    {
                        njin[57] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 69].Text));
                    }

                    if (ssList1_Sheet1.Cells[i, 70].Text != "")
                    {
                        njin[58] += Convert.ToInt64(VB.Val(ssList1_Sheet1.Cells[i, 70].Text));
                    }
                }

                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 1].Text = strPANO;
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 2].Text = strName;
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 3].Text = strDept1;

                if (chkTerm.Checked == true && optIO1.Checked == true)
                {
                    ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 4].Text = strTFDate + "   " + strTEDate;    //전체진료기간
                }

                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 6].Text = strJumin;
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 7].Text = strBoHum;

                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 12].Text = njin[0].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 13].Text = njin[1].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 14].Text = njin[2].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 15].Text = njin[3].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 16].Text = njin[4].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 17].Text = njin[5].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 18].Text = njin[6].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 19].Text = njin[7].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 20].Text = njin[8].ToString();
                
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 21].Text = njin[9].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 22].Text = njin[10].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 23].Text = njin[11].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 24].Text = njin[12].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 25].Text = njin[13].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 26].Text = njin[14].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 27].Text = njin[15].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 28].Text = njin[16].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 29].Text = njin[17].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 30].Text = njin[18].ToString();
                
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 31].Text = njin[19].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 32].Text = njin[20].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 33].Text = njin[21].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 34].Text = njin[22].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 35].Text = njin[23].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 36].Text = njin[24].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 37].Text = njin[25].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 38].Text = njin[26].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 39].Text = njin[27].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 40].Text = njin[28].ToString();
                
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 41].Text = njin[29].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 42].Text = njin[30].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 43].Text = njin[31].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 44].Text = njin[32].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 45].Text = njin[33].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 46].Text = njin[34].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 47].Text = njin[35].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 48].Text = njin[36].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 49].Text = njin[37].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 50].Text = njin[38].ToString();
                
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 51].Text = njin[39].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 52].Text = njin[40].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 53].Text = njin[41].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 54].Text = njin[42].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 55].Text = njin[43].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 56].Text = njin[44].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 57].Text = njin[45].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 58].Text = njin[46].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 59].Text = njin[47].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 60].Text = njin[48].ToString();
               
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 61].Text = njin[49].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 62].Text = njin[50].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 63].Text = njin[51].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 64].Text = njin[52].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 65].Text = njin[53].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 66].Text = njin[54].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 67].Text = njin[55].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 68].Text = njin[56].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 69].Text = njin[57].ToString();
                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 70].Text = njin[58].ToString();

                #endregion TotalCount(GoSub) End
            }


            //ssList1_Sheet1.Cells[0, 0, ssList1_Sheet1.Rows.Count - 1, ssList1_Sheet1.Columns.Count - 1].Locked = true;

        }

        public void Opd_Slip_Amt_Set()
        {
            int nxx = 0;
            //외래용 금액 set

            for (nxx = 1; nxx <= 50; nxx++)
            {
                clsPmpaType.TRPG.TotAmt1[nxx] += clsPmpaType.RPG.Amt1[nxx];
                clsPmpaType.TRPG.TotAmt2[nxx] += clsPmpaType.RPG.Amt2[nxx];
                clsPmpaType.TRPG.TotAmt3[nxx] += clsPmpaType.RPG.Amt3[nxx];
                clsPmpaType.TRPG.TotAmt4[nxx] += clsPmpaType.RPG.Amt4[nxx];
                clsPmpaType.TRPG.TotAmt5[nxx] += clsPmpaType.RPG.Amt5[nxx];
                clsPmpaType.TRPG.TotAmt6[nxx] += clsPmpaType.RPG.Amt6[nxx];
                clsPmpaType.TRPG.TotAmt7[nxx] += clsPmpaType.RPG.Amt7[nxx];
                clsPmpaType.TRPG.TotAmt8[nxx] += clsPmpaType.RPG.Amt8[nxx];
                clsPmpaType.TRPG.TotAmt9[nxx] += clsPmpaType.RPG.Amt9[nxx];

            }
        }

        public void Ipd_Slip_Amt_Set()
        {
            int nxx = 0;
            //입원용 금액 set

            for (nxx = 1; nxx <= 50; nxx++)
            {
                clsPmpaType.TRPG.TotAmt1[nxx] += clsPmpaType.RPG.Amt1[nxx];
                clsPmpaType.TRPG.TotAmt2[nxx] += clsPmpaType.RPG.Amt2[nxx];
                clsPmpaType.TRPG.TotAmt3[nxx] += clsPmpaType.RPG.Amt3[nxx];
                clsPmpaType.TRPG.TotAmt4[nxx] += clsPmpaType.RPG.Amt4[nxx];
                clsPmpaType.TRPG.TotAmt5[nxx] += clsPmpaType.RPG.Amt5[nxx];
                clsPmpaType.TRPG.TotAmt6[nxx] += clsPmpaType.RPG.Amt6[nxx];
                clsPmpaType.TRPG.TotAmt7[nxx] += clsPmpaType.RPG.Amt7[nxx];
                clsPmpaType.TRPG.TotAmt8[nxx] += clsPmpaType.RPG.Amt8[nxx];
                clsPmpaType.TRPG.TotAmt9[nxx] += clsPmpaType.RPG.Amt9[nxx];

            }

        }

        public string READ_MISU_GYEPANO(string ArgPano, string ArgDate, string ArgDept, string ArgBi)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                ";
            SQL += ComNum.VBLF + "  PANO,SNAME,GELCODE,FROMDATE,TODATE,DEPTCODE1,DEPTCODE2,BI1,BI2,REMARK,                              ";
            SQL += ComNum.VBLF + "  ENTDATE , ENTSABUN, DEPTCODE3, BI3                                                                  ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_GYEPANO                                                               ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                             ";
            SQL += ComNum.VBLF + "      AND PANO = '" + ArgPano + "'                                                                    ";
            SQL += ComNum.VBLF + "      AND TODATE >=TO_DATE('" + ArgDate + "','YYYY-MM-DD')                                            ";
            SQL += ComNum.VBLF + "      AND (DeptCode1 ='" + ArgDept + "' OR DeptCode2 ='" + ArgDept + "' OR DeptCode3 ='" + ArgDept + "') ";
            SQL += ComNum.VBLF + "      AND (BI1 ='" + ArgBi + "' OR BI2 ='" + ArgBi + "' OR BI3 ='" + ArgBi + "')                      ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
            {
                rtnVal = "OK";
            }

            dt.Dispose();
            dt = null;
            return rtnVal;
        }

        public void Ipd_Slip_Amt_Set_DRG()
        {
            int nxx = 0;

            //DRG입원용 금액 set
            //식대와 실료차액만 저장

            //식대계산 금액 변수저장
            clsPmpaType.RPG.Amt5[3] = DRG.GnDrgFoodAmt[0];     //식대 급여본인
            clsPmpaType.RPG.Amt6[3] = DRG.GnDrgFoodAmt[1];     //식대 급여공단
            clsPmpaType.RPG.Amt1[3] = DRG.GnDrgFoodAmt[2];     //식대 전액부담
            clsPmpaType.RPG.Amt3[3] = DRG.GnDrgFoodAmt[3];     //식대 선택비
            clsPmpaType.RPG.Amt2[3] = DRG.GnDrgFoodAmt[4];     //식대 비급여

            clsPmpaType.RPG.Amt5[50] += DRG.GnDrgFoodAmt[0];
            clsPmpaType.RPG.Amt6[50] += DRG.GnDrgFoodAmt[1];
            clsPmpaType.RPG.Amt1[50] += DRG.GnDrgFoodAmt[2];
            clsPmpaType.RPG.Amt3[50] += DRG.GnDrgFoodAmt[3];
            clsPmpaType.RPG.Amt2[50] += DRG.GnDrgFoodAmt[4];

            //병실차액료 변수저장
            clsPmpaType.RPG.Amt5[21] = DRG.GnDrgRoomAmt[0];    //병실차액 급여본인
            clsPmpaType.RPG.Amt6[21] = DRG.GnDrgRoomAmt[1];    //병실차액 급여본인
            clsPmpaType.RPG.Amt1[21] = DRG.GnDrgRoomAmt[2];    //병실차액 전액부담
            clsPmpaType.RPG.Amt3[21] = DRG.GnDrgRoomAmt[3];    //병실차액 선택비
            clsPmpaType.RPG.Amt2[21] = DRG.GnDrgRoomAmt[4];    //병실차액 비급여총액

            clsPmpaType.RPG.Amt5[50] += DRG.GnDrgRoomAmt[0];
            clsPmpaType.RPG.Amt6[50] += DRG.GnDrgRoomAmt[1];
            clsPmpaType.RPG.Amt1[50] += DRG.GnDrgRoomAmt[2];
            clsPmpaType.RPG.Amt3[50] += DRG.GnDrgRoomAmt[3];
            clsPmpaType.RPG.Amt2[50] += DRG.GnDrgRoomAmt[4];

            for (nxx = 1; nxx <= 50; nxx++)
            {
                //DRG 비급여 총액에 병실료차액 더블되어 막음
                if (nxx != 3 && nxx != 21)
                {
                    clsPmpaType.TRPG.TotAmt2[nxx] += clsPmpaType.RPG.Amt2[nxx];
                    clsPmpaType.TRPG.TotAmt3[nxx] += clsPmpaType.RPG.Amt3[nxx];
                    clsPmpaType.TRPG.TotAmt4[nxx] += clsPmpaType.RPG.Amt4[nxx];
                }
            }

            clsPmpaType.TRPG.TotAmt1[3] += clsPmpaType.RPG.Amt1[3];
            clsPmpaType.TRPG.TotAmt2[3] += clsPmpaType.RPG.Amt2[3];
            clsPmpaType.TRPG.TotAmt3[3] += clsPmpaType.RPG.Amt3[3];
            clsPmpaType.TRPG.TotAmt4[3] += clsPmpaType.RPG.Amt4[3];
            clsPmpaType.TRPG.TotAmt5[3] += clsPmpaType.RPG.Amt5[3];
            clsPmpaType.TRPG.TotAmt6[3] += clsPmpaType.RPG.Amt6[3];
            //clsPmpaType.TRPG.TotAmt7[3] += clsPmpaType.RPG.Amt7[3];
            //clsPmpaType.TRPG.TotAmt8[3] += clsPmpaType.RPG.Amt8[3];

            clsPmpaType.TRPG.TotAmt1[21] += clsPmpaType.RPG.Amt1[21];
            clsPmpaType.TRPG.TotAmt2[21] += clsPmpaType.RPG.Amt2[21];
            clsPmpaType.TRPG.TotAmt3[21] += clsPmpaType.RPG.Amt3[21];
            clsPmpaType.TRPG.TotAmt4[21] += clsPmpaType.RPG.Amt4[21];
            clsPmpaType.TRPG.TotAmt5[21] += clsPmpaType.RPG.Amt5[21];
            clsPmpaType.TRPG.TotAmt6[21] += clsPmpaType.RPG.Amt6[21];
            //clsPmpaType.TRPG.TotAmt7[21] += clsPmpaType.RPG.Amt7[21];
            //clsPmpaType.TRPG.TotAmt8[21] += clsPmpaType.RPG.Amt8[21];
        }

        /// <summary>
        /// author : 안정수
        /// Create Date : 2017-10-12
        /// <seealso cref="JengSan, Frm진료비수납내역서_2012 : 상한제체크()"/>
        /// </summary>
        /// <returns></returns>
        public long Sang_Check()
        {
            int kk = 0;
            int jj = 0;

            string[] SangFDate_New = new string[21];
            string[] SangTDate_New = new string[21];

            string strToYear = "";  //해당년도1월1일

            long rtnVal = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            //차상위2종 이면 입원일자가 2009-04-01 이후라야 상한제 적용됨
            if ((clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11") && (clsPmpaType.TIT.OgPdBun == "E" || clsPmpaType.TIT.OgPdBun == "F" || clsPmpaType.TIT.OgPdBun == "J" || clsPmpaType.TIT.OgPdBun == "K"))
            {
                if (String.Compare(clsPmpaType.TIT.InDate, "2009-04-01") < 0)
                {
                    return rtnVal;
                }
            }

            strToYear = VB.Left(clsPmpaType.TIT.InDate, 4) + "-01-01";

            //건강보험으로 최초입원일자 구함 - 상한제 처음날짜 2009-07-23 윤조연
            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  TO_CHAR(b.INDATE,'YYYY-MM-DD') INDATE";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER a, " + ComNum.DB_PMPA + "IPD_TRANS b";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND a.IPDNO=b.IPDNO";
            SQL += ComNum.VBLF + "      AND a.Pano = '" + clsPmpaType.TIT.Pano + "'";
            SQL += ComNum.VBLF + "      AND b.InDate >= TO_DATE('" + strToYear + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND b.BI IN ('11','12','13')";
            SQL += ComNum.VBLF + "      AND ( b.GBIPD NOT IN ('D') OR b.GBIPD IS NULL)";
            SQL += ComNum.VBLF + "ORDER BY b.INDATE";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
            {
                if (String.Compare(dt.Rows[0]["INDATE"].ToString().Trim(), "2004-06-30") <= 0)
                {
                    SangFDate_New[0] = "2004-07-01";
                }
                else
                {
                    SangFDate_New[0] = dt.Rows[0]["INDATE"].ToString().Trim();
                }

                for (kk = 1; kk <= 20; kk++)
                {
                    if (kk == 1)
                    {
                        SangFDate_New[kk] = SangFDate_New[kk - 1];
                        SangTDate_New[kk] = CPF.SangHan_MagamDay(clsDB.DbCon, SangFDate_New[kk]);
                    }

                    else
                    {
                        SangFDate_New[kk] = Convert.ToDateTime(SangTDate_New[kk - 1]).AddDays(1).ToShortDateString();
                        SangTDate_New[kk] = CPF.SangHan_MagamDay(clsDB.DbCon, SangFDate_New[kk]);
                    }
                }
            }

            dt.Dispose();
            dt = null;

            for (kk = 1; kk <= 20; kk++)
            {
                if (String.Compare(SangFDate_New[kk], clsPmpaType.TIT.InDate) <= 0 && String.Compare(SangTDate_New[kk], clsPmpaType.TIT.InDate) >= 0)
                {
                    break;
                }
            }
            if (kk == 21)
            {
                kk = 20;
            }

            SangFDate_New[kk] = SangFDate_New[kk];
            SangTDate_New[kk] = SangTDate_New[kk];

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  SUM(SangAmt) SangAmt";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_TRANS";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND Pano = '" + clsPmpaType.TIT.Pano + "' ";
            SQL += ComNum.VBLF + "      AND InDate >= TO_DATE('" + SangFDate_New[1] + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND InDate <= TO_DATE('" + SangTDate_New[1] + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND BI IN ('11','12','13')";        //11, 12, 13건강보험만
            SQL += ComNum.VBLF + "      AND ( GBIPD NOT IN ('D') OR GBIPD IS NULL)";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }

            rtnVal = 0;

            if (dt.Rows.Count > 0)
            {
                rtnVal = Convert.ToInt64(VB.Val(dt.Rows[0]["SangAmt"].ToString().Trim()));
            }

            dt.Dispose();
            dt = null;

            return rtnVal;

        }


    }
}

