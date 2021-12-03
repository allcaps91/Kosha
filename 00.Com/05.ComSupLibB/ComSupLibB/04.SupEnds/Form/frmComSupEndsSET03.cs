using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using ComSupLibB.SupXray;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComSupLibB.SupEnds
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupEnds
    /// File Name       : frmComSupEndsSET03.cs
    /// Description     : 내시경 간호기록지 기본사항 입력폼
    /// Author          : 윤조연
    /// Create Date     : 2017-10-18
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\ocs\endo\endo_new\Frm내시경기록지.frm(Frm내시경기록지) >> frmComSupEndsSET03.cs 폼이름 재정의" />
    /// 
    public partial class frmComSupEndsSET03 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic();
        clsComSup sup = new clsComSup();
        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();
        clsQuery query = new clsQuery();
        clsComSupEndsSQL cendsSQL = new clsComSupEndsSQL();
        clsComSupEndsSpd cendsSpd = new clsComSupEndsSpd();
        clsComSupXraySQL cxraySQL = new clsComSupXraySQL();

        clsComSupEndsSQL.cEndoJupmst cEndoJupmst = null;
        clsComSupEndsSQL.cEndoChart cEndoChart = null;
        clsComSupEndsSQL.cEndoAddHis cEndoAddHis = null;

        frmAgreePrint frmAgreePrintEvent = null;

        string gPtno = "";
        string gBDate = "";
        string gRDate = "";
        string gDept = "";
        string gROWID = "";

        string gChk1 = "";
        string gChk2 = "";
        string gChk3 = "";

        #endregion

        #region //MainFormMessage
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
        #endregion //MainFormMessage
        public frmComSupEndsSET03()
        {
            InitializeComponent();
            setEvent();
        }

        public frmComSupEndsSET03(MainFormMessage pform)
        {
            InitializeComponent();

            this.mCallForm = pform;

            setEvent();
        }

        //기본값 세팅
        void setCtrlData(PsmhDb pDbCon)
        {
            //txtPano.Text = gstrPano;
            //lblSName.Text = gstrSName;

            setCombo(pDbCon);

        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnClear.Click += new EventHandler(eBtnClick);
            this.btnSet.Click += new EventHandler(eBtnClick);

            this.btnSearch1.Click += new EventHandler(eBtnSearch);
            this.btnSearch3.Click += new EventHandler(eBtnSearch);
            this.btnSearch4.Click += new EventHandler(eBtnSearch);
            
            this.btnSave1.Click += new EventHandler(eBtnSave);
            this.btnSave2.Click += new EventHandler(eBtnSave);

            this.btnPrint.Click += new EventHandler(eBtnPrint);

            //시트관련 이벤트
            this.ssList.ButtonClicked += new EditorNotifyEventHandler(eSpreadButtonClick);
            this.ssList.CellClick += new CellClickEventHandler(eSpreadClick);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.ssList2.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            //this.ssList.TextTipFetch += new TextTipFetchEventHandler(eTxtTipFetch);
            //this.ssList.SelectionChanged += new SelectionChangedEventHandler(eSpreadSelChanged);

            this.dtpFDate.ValueChanged += new EventHandler(eDtpValueChanged);

            this.txtPtno.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.txtNrSabun.KeyDown += new KeyEventHandler(eTxtKeyDown);
            

        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            else
            {
                //            
                cendsSpd.sSpd_enmSupEndsSet02A(ssList, cendsSpd.sSpdenmSupEndsSet02A, cendsSpd.nSpdenmSupEndsSet02A, 1, 0);
                cendsSpd.sSpd_enmSupEndsSet02B(ssList2, cendsSpd.sSpdenmSupEndsSet02B, cendsSpd.nSpdenmSupEndsSet02B, 1, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등            


                //툴팁
                //ssList.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Fixed;
                //ssList.TextTipDelay = 1000;


                screen_clear();

                setCtrlData(clsDB.DbCon);

                //screen_display();

            }

        }

        void eFormActivated(object sender, EventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgActivedForm(this);
            }
        }

        void eFormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == this.btnClear)
            {
                screen_clear();
                screen_clear2();
            }
            else if (sender == this.btnSet)
            {
                //콘트롤 값 clear
                Control[] controls = ComFunc.GetAllControls(this);

                foreach (Control ctl in controls)
                {
                    if (ctl is CheckBox)
                    {                        
                        if (VB.Left(((CheckBox)ctl).Name, 6) == "chkOUT")
                        {
                            ((CheckBox)ctl).Checked = true;
                        }
                    }                    
                }
            }
        }

        void eBtnSearch(object sender, EventArgs e)
        {
            if (sender == this.btnSearch1)
            {
                screen_display();
            }
            else if (sender == this.btnSearch3)
            {
                //투여약 정보 불러오기
                read_endo_drug_set(clsDB.DbCon);
            }
            else if (sender == this.btnSearch4)
            {
                //입력 정보 불러오기
                read_endo_chart_set(clsDB.DbCon);
            }
        }

        void eBtnSave(object sender, EventArgs e)
        {
            if (sender == this.btnSave1)
            {
                eSave(clsDB.DbCon);
            }
            else if (sender == this.btnSave2)
            {
                endo_newSet(clsDB.DbCon);
            }
        }

        void eBtnPrint(object sender, EventArgs e)
        {
            if (sender == this.btnPrint)
            {
                ePrint();
            }            
        }

        void eBtnPopUp(object sender, EventArgs e)
        {

        }

        void eSpreadClick(object sender, CellClickEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            if (e.Row < 0 || e.Column < 0) return;

            //마우스 우클릭
            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            if (sender == this.ssList)
            {
                for (int i = 0; i < o.ActiveSheet.RowCount; i++)
                {
                    o.ActiveSheet.Rows.Get(i).BackColor = System.Drawing.Color.White;
                }

                o.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.LightGreen;
            }
        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            FpSpread o = (FpSpread)sender;                      

            if (e.Row < 0 || e.Column < 0) return;

            //마우스 우클릭
            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(o, e.Column);
                return;
            }
            else if (sender == this.ssList)
            {                
                gPtno = "";

                for (int i = 0; i < o.ActiveSheet.RowCount; i++)
                {
                    o.ActiveSheet.Rows.Get(i).BackColor = System.Drawing.Color.White;
                }

                o.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.LightGreen;
                 
                gPtno = o.ActiveSheet.Cells[e.Row, (int)clsComSupEndsSpd.enmSupEndsSet02A.Ptno].Text.Trim();
                gBDate = o.ActiveSheet.Cells[e.Row, (int)clsComSupEndsSpd.enmSupEndsSet02A.BDate].Text.Trim();
                gRDate = o.ActiveSheet.Cells[e.Row, (int)clsComSupEndsSpd.enmSupEndsSet02A.RDate].Text.Trim();
                gDept = o.ActiveSheet.Cells[e.Row, (int)clsComSupEndsSpd.enmSupEndsSet02A.DeptCode].Text.Trim();

                if (gPtno != "")
                {
                    screen_display2(gPtno);
                }

            }
            else if (sender == this.ssList2)
            {
                gROWID = "";

                for (int i = 0; i < o.ActiveSheet.RowCount; i++)
                {
                    o.ActiveSheet.Rows.Get(i).BackColor = System.Drawing.Color.White;
                }
                o.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.LightGreen;
                                
                #region 클래스 생성 및 변수세팅
                cEndoChart = new clsComSupEndsSQL.cEndoChart();
                cEndoChart.BDate = o.ActiveSheet.Cells[e.Row, (int)clsComSupEndsSpd.enmSupEndsSet02B.BDate].Text.Trim();
                cEndoChart.RDate = o.ActiveSheet.Cells[e.Row, (int)clsComSupEndsSpd.enmSupEndsSet02B.RDate].Text.Trim();
                if (o.ActiveSheet.Cells[e.Row, (int)clsComSupEndsSpd.enmSupEndsSet02B.EMRNO].Text.Trim()!="")
                {
                    cEndoChart.EMRNO = Convert.ToInt32(o.ActiveSheet.Cells[e.Row, (int)clsComSupEndsSpd.enmSupEndsSet02B.EMRNO].Text.Trim());
                }                
                cEndoChart.ROWID = o.ActiveSheet.Cells[e.Row, (int)clsComSupEndsSpd.enmSupEndsSet02B.ROWID].Text.Trim();
                gROWID = cEndoChart.ROWID;
                #endregion

                if (e.Column == (int)clsComSupEndsSpd.enmSupEndsSet02B.NrName)
                {
                    //삭제
                    if (cEndoChart.EMRNO > 0)
                    {                            
                        ComFunc.MsgBox("차트형성이 이미 완료된 기록입니다.. 수정 및 삭제 불가합니다..");
                        return;
                    }
                    else
                    {
                        if (cEndoChart.ROWID != "")
                        {
                            if (ComFunc.MsgBoxQ("선택하신 자료를 삭제하시겠습니까??", "삭제확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                            {                                    
                                return;
                            }
                            else
                            {
                                #region //자료삭제
                                string SQL = "";
                                string SqlErr = ""; //에러문 받는 변수
                                int intRowAffected = 0; //변경된 Row 받는 변수

                                // clsTrans DT = new clsTrans();
                                clsDB.setBeginTran(clsDB.DbCon);
                                                                

                                try
                                {
                                    SqlErr = cendsSQL.del_ENDO_CHART(clsDB.DbCon,cEndoChart,  ref intRowAffected);
                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        ComFunc.MsgBox(SqlErr);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        Cursor.Current = Cursors.Default;
                                        return;
                                    }
                                    else
                                    {
                                        clsDB.setCommitTran(clsDB.DbCon);
                                        ComFunc.MsgBox("저장하였습니다.");
                                    }
                                }
                                catch(Exception ex)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(ex.Message);
                                }
                                #endregion
                            }
                        }
                    }
                }
                else
                {
                    if (cEndoChart.ROWID != "")
                    {
                        GetData3(clsDB.DbCon, cEndoChart.ROWID, cEndoChart.EMRNO);
                    }
                } 
            }
        }

        void eSpreadButtonClick(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            if (e.Row < 0 || e.Column < 0) return;

            if (sender == this.ssList)
            {
                //if (e.Column == (int)clsSupEndsSpd.enmSupEndsRCP02A.Chk)
                //{
                //    if (o.ActiveSheet.Cells[e.Row, (int)clsSupEndsSpd.enmSupEndsRCP02A.Chk].Text == "True")
                //    {
                //        o.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.LightPink;
                //    }
                //    else
                //    {
                //        o.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.White;
                //    }
                //}
            }

        }

        void eDtpValueChanged(object sender, EventArgs e)
        {
            dtpTDate.Text = dtpFDate.Text;
        }

        void eSave(PsmhDb pDbCon)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            
            // clsTrans DT = new clsTrans();           
            clsDB.setBeginTran(pDbCon);

            try
            {
                #region //클래스 생성 및 변수세팅
                cEndoChart = new clsComSupEndsSQL.cEndoChart();
                cEndoChart.Ptno = txtPtno.Text.Trim();
                cEndoChart.ROWID = gROWID;
                cEndoChart.BDate = dtpBDate.Text.Trim();
                cEndoChart.RDate = dtpRDate.Text.Trim();
                cEndoChart.RDrName = cboBDrName.Text.Trim();

                if (cEndoChart.BDate =="")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("처방일자 공란!!");
                    return;
                }
                if (cEndoChart.RDate == "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("검사일자 공란!!");
                    return;
                }

                cEndoChart.Gubun = "";
                if (optBOpd.Checked == true)
                {
                    cEndoChart.Gubun = "1";
                }
                else if (optBWard.Checked == true)
                {
                    cEndoChart.Gubun = "2";
                }
                else if (optBHR.Checked == true)
                {
                    cEndoChart.Gubun = "3";
                }
                else if (optBTO.Checked == true)
                {
                    cEndoChart.Gubun = "4";
                }
                else if (optBER.Checked == true)
                {
                    cEndoChart.Gubun = "5";
                }
                else if (optBHRTO.Checked == true)
                {
                    cEndoChart.Gubun = "6";
                }
                
                cEndoChart.EGD1 = chkBEGD1.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.EGD2 = chkBEGD2.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.CFS1 = chkBCFS1.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.CFS2 = chkBCFS2.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.SIG1 = chkBSig1.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.SIG2 = chkBSig2.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.BRO1 = chkBBro1.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.BRO2 = chkBBro2.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.ERCP1 = chkBERCP1.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.ERCP2 = chkBERCP2.Checked.ToString() == "True" ? "1" : "";

                //파트1
                cEndoChart.DIET = chkEat.Checked.ToString() == "True" ? "1" : "";
                //전신상태
                if (chkBodySTS1.Checked == true)
                {
                    cEndoChart.STS = "1";
                }
                else if (chkBodySTS1.Checked == true)
                {
                    cEndoChart.STS = "2";
                }
                else if (chkBodySTS3.Checked == true)
                {
                    cEndoChart.STS = "3";
                }
                else if (chkBodySTS4.Checked == true)
                {
                    cEndoChart.STS = "4";
                }
                
                //병력
                cEndoChart.OLD_0 = chkOld0.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.OLD_1 = chkOld1.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.OLD_2 = chkOld2.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.OLD_3 = chkOld3.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.OLD_4 = chkOld4.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.OLD_5 = chkOld5.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.OLD_6 = chkOld6.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.OLD_7 = chkOld7.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.OLD_8 = chkOld8.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.OLD_9 = chkOld9.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.OLD_10 = chkOld10.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.OLD_11 = chkOld11.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.OLD_12 = chkOld12.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.OLD_131 = chkOld13.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.OLD_132 = txtOld13.Text.Trim();
                cEndoChart.OLD_14 = chkOld14.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.OLD_15 = ComFunc.QuotConv(txtOldEtc.Text.Trim());
                //약물
                cEndoChart.DRUG_0 = chkDrug0.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.DRUG_1 = chkDrug1.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.DRUG_2 = chkDrug2.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.DRUG_3 = chkDrug3.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.DRUG_4 = chkDrug4.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.DRUG_5 = chkDrug5.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.DRUG_6 = chkDrug6.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.DRUG_7 = chkDrug7.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.DRUG_8 = txtDrugEtc.Text.Trim();
                cEndoChart.DRUG_9 = ComFunc.QuotConv(txtDrugA.Text.Trim());
                cEndoChart.DRUG_10 = ComFunc.QuotConv(txtDrugB.Text.Trim());
                //전처치
                cEndoChart.B_DRUG = chkDrugB1.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.B_DRUG1 = chkDrugB2.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.B_DRUG1_1 = ComFunc.QuotConv(txtDrugB1.Text.Trim());
                cEndoChart.BIGO = ComFunc.QuotConv(txtBigo.Text.Trim());

                //파트2
                //Midazolam
                cEndoChart.SLEEP_DRUG1 = txtDrugMid.Text.Trim();
                cEndoChart.SLEEP_DRUG2 = txtDrugPro.Text.Trim();
                cEndoChart.SLEEP_DRUG3 = txtDrugPet.Text.Trim();
                cEndoChart.SLEEP_DRUG_ETC = txtDrug.Text.Trim();
                cEndoChart.SLEEP_RE_DRUG = chkSleep1.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.SLEEP_RE_DRUG1 = chkSleep2.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.SLEEP_RE_DRUG1_1 = txtSleep.Text.Trim();
                //장정결
                if (chkClean1.Checked == true)
                {
                    cEndoChart.STS1 = "1";
                }
                else if (chkClean2.Checked == true)
                {
                    cEndoChart.STS1 = "2";
                }
                else if (chkClean3.Checked == true)
                {
                    cEndoChart.STS1 = "3";
                }
                //장정결도
                if (chkCleanSTS1.Checked == true)
                {
                    cEndoChart.STS2 = "1";
                }
                else if (chkCleanSTS2.Checked == true)
                {
                    cEndoChart.STS2 = "2";
                }
                else if (chkCleanSTS3.Checked == true)
                {
                    cEndoChart.STS2 = "3";
                }
                //시작,종료
                cEndoChart.STIME = txtSTime.Text.Trim().Replace(":","");
                cEndoChart.ETIME = txtETime.Text.Trim().Replace(":", "");
                if ((cEndoChart.STIME !="" && cEndoChart.STIME.Length != 4) || (cEndoChart.ETIME != "" && cEndoChart.ETIME.Length != 4))
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("검사시작,종료 시간 확인!! ex)11:00");
                    return;
                }
                //활력
                cEndoChart.SP0_11 = txtSTSA1.Text.Trim();
                cEndoChart.SP0_12 = txtSTSA2.Text.Trim();
                cEndoChart.SP0_13 = txtSTSA3.Text.Trim();
                cEndoChart.SP0_14 = txtSTSA4.Text.Trim();
                cEndoChart.SP0_21 = txtSTSB1.Text.Trim();
                cEndoChart.SP0_22 = txtSTSB2.Text.Trim();
                cEndoChart.SP0_23 = txtSTSB3.Text.Trim();
                cEndoChart.SP0_24 = txtSTSB4.Text.Trim();
                cEndoChart.SP0_31 = txtSTSC1.Text.Trim();
                cEndoChart.SP0_32 = txtSTSC2.Text.Trim();
                cEndoChart.SP0_33 = txtSTSC3.Text.Trim();
                cEndoChart.SP0_34 = txtSTSC4.Text.Trim();
                cEndoChart.SP0_41 = txtSTSD1.Text.Trim();
                cEndoChart.SP0_42 = txtSTSD2.Text.Trim();
                //수면평가
                cEndoChart.SLEEP_STS1 = chkSleepSTS1.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.SLEEP_STS2 = chkSleepSTS2.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.SLEEP_STS3 = chkSleepSTS3.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.SLEEP_STS4 = chkSleepSTS4.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.SLEEP_STS5 = chkSleepSTS5.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.SLEEP_STS6 = chkSleepSTS6.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.SLEEP_STS7 = chkSleepSTS7.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.SLEEP_STS7_1 = ComFunc.QuotConv(txtSleepSTS.Text.Trim());
                //생검
                cEndoChart.EXAM = "0";
                if (optBio2.Checked ==true)
                {
                    cEndoChart.EXAM = "1";
                }
                cEndoChart.CLO = chkCLO.Checked.ToString() == "True" ? "1" : "";
                //퇴실기준                
                if (optOUT1.Checked == true)
                {
                    cEndoChart.OUT_GUBUN = "1";
                }
                else if (optOUT2.Checked ==true)
                {
                    cEndoChart.OUT_GUBUN = "2";
                }
                cEndoChart.OUT_GUBUN1 = chkOUT1.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.OUT_GUBUN2 = chkOUT2.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.OUT_GUBUN3 = chkOUT3.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.OUT_GUBUN4 = chkOUT4.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.OUT_GUBUN5 = chkOUT5.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.OUT_GUBUN6 = chkOUT6.Checked.ToString() == "True" ? "1" : "";
                cEndoChart.OUT_GUBUN7 = chkOUT7.Checked.ToString() == "True" ? "1" : "";
                //간호기록유무-특이사항
                cEndoChart.NUR_CHART = "0";
                if (optETC2.Checked ==true)
                {
                    cEndoChart.NUR_CHART = "1";
                }
                cEndoChart.NUR_CHART_REMARK = ComFunc.QuotConv(txtETC.Text.Trim());
                cEndoChart.NUR_NAME = lblNrName.Text.Trim();
                cEndoChart.EntSabun = clsType.User.Sabun;
                if (cEndoChart.NUR_NAME !="" && txtNrSabun.Text.Trim() !="")
                {
                    if (cEndoChart.NUR_NAME != clsVbfunc.GetInSaName(pDbCon, txtNrSabun.Text.Trim()))
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("간호사 사번과 간호사 이름이 일치하지 않습니다");
                        return;
                    }
                }
                else if (cEndoChart.NUR_NAME == "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("간호사 이름이 공백입니다");
                    return;
                }
                
                #endregion

                #region //자료저장
                if (gROWID != "")
                {
                    //갱신
                    SqlErr = cendsSQL.up_ENDO_CHART(pDbCon,cEndoChart, ref intRowAffected);
                }
                else
                {
                    SqlErr = cendsSQL.ins_ENDO_CHART(pDbCon,cEndoChart, ref intRowAffected);
                }

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                else
                {
                    clsDB.setCommitTran(pDbCon);
                    ComFunc.MsgBox("저장하였습니다.");
                    screen_clear();
                    screen_clear2();
                }
                #endregion
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
            }
            
        }

        void ePrint()
        {
            if (gDept =="" || gPtno =="")
            {
                ComFunc.MsgBox("대상자를 선택후 출력하십시오!!");
                return;
            }

            if (frmAgreePrintEvent != null)
            {
                frmAgreePrintEvent.Dispose();
                frmAgreePrintEvent = null;
            }

            frmAgreePrintEvent = new frmAgreePrint(txtPtno.Text.Trim(), "0", "O", dtpRDate.Text.Replace("-", "").Trim(), "120000", "", "120000", gDept, "", "" , "1", "2175");
            frmAgreePrintEvent.rEventClose += Frm_rEventClose;
            frmAgreePrintEvent.ShowDialog();
        }

        void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            if (sender == this.txtPtno)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    lblSName.Text = "";
                    if (txtPtno.Text.Trim() != "")
                    {
                        string strPtno = ComFunc.SetAutoZero(txtPtno.Text.Trim(), ComNum.LENPTNO);
                        txtPtno.Text = strPtno;
                        lblSName.Text = clsVbfunc.GetPatientName( clsDB.DbCon, strPtno);
                    }
                }
            }
            else if (sender == this.txtNrSabun)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    lblNrName.Text = "";
                    if (txtNrSabun.Text.Trim() != "")
                    {
                        //string strPtno = ComFunc.SetAutoZero(txtPtno.Text.Trim(), ComNum.LENUSEID);
                        //txtPtno.Text = strPtno;
                        lblNrName.Text = clsVbfunc.GetInSaName(clsDB.DbCon, txtNrSabun.Text.Trim());
                    }
                }
            }
        }

        private void Frm_rEventClose()
        {
            frmAgreePrintEvent.Dispose();
            frmAgreePrintEvent = null;
        }

        void setCombo(PsmhDb pDbCon)
        {
            DataTable dt = null;

            dt = sup.sel_Ocs_Doctor(pDbCon, " Sabun || '.' || DrName AS CODE ", " AND DeptCode IN ('MG','FM') ","",""," DeptCode ",true);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                method.setCombo_View(this.cboDrCode, dt, clsParam.enmComParamComboType.ALL);
            }

            dt = query.Get_BasBcode(pDbCon, "C#_ENDO_CHART_검사자", ""," Name ");
            if (ComFunc.isDataTableNull(dt) == false)
            {
                method.setCombo_View(this.cboBDrName, dt, clsParam.enmComParamComboType.NULL);
            }
        }

        void screen_clear(string Job = "")
        {
            read_sysdate();

            dtpBDate.Enabled = true;                       

            if (gDept =="TO")
            {
                btnSave2.Enabled = false;
            }
            else
            {
                btnSave2.Enabled = true;
            }


            //콘트롤 값 clear
            Control[] controls = ComFunc.GetAllControls(this);

            foreach (Control ctl in controls)
            {
                if (ctl is TextBox)
                {
                    ctl.Text = "";
                }
                else if (ctl is Label)
                {
                    if (((Label)ctl).Name == "lblSName" || ((Label)ctl).Name == "lblNrName" || ((Label)ctl).Name == "lblSTS")
                    {
                        ((Label)ctl).Text = "";
                    }
                }
                else if (ctl is CheckBox)
                {
                    ((CheckBox)ctl).Checked = false;
                }
                else if (ctl is DateTimePicker)
                {
                    if (((DateTimePicker)ctl).Name == "dtpBDate" || ((DateTimePicker)ctl).Name == "dtpRDate" )
                    {
                        ((DateTimePicker)ctl).Text = "";
                    }
                }
                else if (ctl is RadioButton)
                {
                    if ( VB.Left(((RadioButton)ctl).Name,6) != "optJob")
                    {
                        ((RadioButton)ctl).Checked = false;
                    }                    
                }
                else if (ctl is ComboBox)
                {
                    if (((ComboBox)ctl).Name == "cboBDrName")
                    {
                        ((ComboBox)ctl).Text = "";
                    }
                }
            }

            lblSTS.BackColor = System.Drawing.Color.FromArgb(192, 255, 192);
            lblSTS.Text = "신규작업중";

        }

        void screen_clear2()
        {
            gROWID = "";
            ssList2.ActiveSheet.RowCount = 0;
        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void read_endo_chart_set(PsmhDb pDbCon)
        {
            if (txtPtno.Text.Trim() == "")
            {
                return;
            }

            DataTable dt = null;
            string s1 = "";
            string s2 = "";

            #region //내시경 약물 등록내용 및 적용선택

            #region //Biz 쿼리 변수세팅 및 쿼리실행
            cEndoJupmst = new clsComSupEndsSQL.cEndoJupmst();
            cEndoJupmst.STS = "7";
            cEndoJupmst.Job = "*";
            cEndoJupmst.Job2 = "00";
            cEndoJupmst.Ptno = gPtno;
            cEndoJupmst.Date1 = dtpFDate.Text.Trim();
            cEndoJupmst.Date2 = dtpTDate.Text.Trim();
            cEndoJupmst.DrCode = "******";
            cEndoJupmst.BDate = gBDate;
            cEndoJupmst.GbSunap = "ALL";            
            cEndoJupmst.Buse = "ALL";            

            #endregion

            dt = cendsSQL.sel_ENDO_JUPMST(pDbCon, cEndoJupmst,false);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (s1 =="" && (dt.Rows[i]["GBCON_2"].ToString().Trim() == "Y" || dt.Rows[i]["GBCON_3"].ToString().Trim() == "Y" || dt.Rows[i]["GBCON_4"].ToString().Trim() == "Y"))
                    {
                        s1 = "Y";
                        s2 = "내시경 입력정보!! \r\n";
                    }
                    if (dt.Rows[i]["GBCON_2"].ToString().Trim() == "Y")
                    {
                        s2 += "Midazolam >> " + dt.Rows[i]["GBCON_21"].ToString().Trim() + "mg " + dt.Rows[i]["GBCON_22"].ToString().Trim() + "mg \r\n\r\n";
                    }
                    if (dt.Rows[i]["GBCON_3"].ToString().Trim() == "Y")
                    {
                        s2 += "Propofol >> " + dt.Rows[i]["GBCON_31"].ToString().Trim() + "mg " + dt.Rows[i]["GBCON_32"].ToString().Trim() + "mg \r\n\r\n";
                    }
                    if (dt.Rows[i]["GBCON_4"].ToString().Trim() == "Y")
                    {
                        s2 += "Pethidine >> " + dt.Rows[i]["GBCON_41"].ToString().Trim() + "mg " + dt.Rows[i]["GBCON_42"].ToString().Trim() + "mg \r\n\r\n";
                    }
                }

                if (s2 != "")
                {
                    if (ComFunc.MsgBoxQ(s2 + "\r\n" + "위의 정보를 기록지에 사용하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {         
                            if (dt.Rows[i]["GBCON_2"].ToString().Trim() == "Y")
                            {
                                txtDrugMid.Text = dt.Rows[i]["GBCON_21"].ToString().Trim();
                            }
                            if (dt.Rows[i]["GBCON_3"].ToString().Trim() == "Y")
                            {
                                txtDrugPro.Text = dt.Rows[i]["GBCON_31"].ToString().Trim();
                            }
                            if (dt.Rows[i]["GBCON_4"].ToString().Trim() == "Y")
                            {
                                txtDrugPet.Text = dt.Rows[i]["GBCON_41"].ToString().Trim();
                            }
                        }
                    }
                }

            }
            #endregion
            
        }

        void read_endo_drug_set(PsmhDb pDbCon)
        {
            if (txtPtno.Text.Trim() =="")
            {
                return;
            }

            DataTable dt = cendsSQL.sel_ENDO_HYANG_CNT(pDbCon, gPtno, gBDate, gRDate);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {                    
                    if (dt.Rows[i]["OrderCode"].ToString().Trim()== "A-POL12G" && dt.Rows[i]["CONTENT"].ToString().Trim()!="")
                    {
                        txtDrugPro.Text = dt.Rows[i]["CONTENT"].ToString().Trim();
                    }
                    else if (dt.Rows[i]["OrderCode"].ToString().Trim() == "A-BASCA" && dt.Rows[i]["CONTENT"].ToString().Trim() != "")
                    {
                        txtDrugMid.Text = dt.Rows[i]["CONTENT"].ToString().Trim();
                    }
                    else if (dt.Rows[i]["OrderCode"].ToString().Trim() == "N-PTD25" && dt.Rows[i]["CONTENT"].ToString().Trim() != "")
                    {
                        txtDrugPet.Text = dt.Rows[i]["CONTENT"].ToString().Trim();
                    }
                }                
            }
        }

        void endo_newSet(PsmhDb pDbCon)
        {
            screen_clear();

            gPtno = "";

            dtpBDate.Text = cpublic.strSysDate;
            dtpRDate.Text = cpublic.strSysDate;

            if (gDept =="TO")
            {
                optBTO.Checked = true;//종검

                chkBEGD1.Checked = true;//위
                chkBEGD2.Checked = true;//수면

                chkBCFS1.Checked = true;//대장
                chkBCFS2.Checked = true;//수면

            }

            chkEat.Checked = true;//금식 예
            chkBodySTS1.Checked = true;//전신상태 양호

            chkDrugB1.Checked = true;//전처치 없음
            chkSleep1.Checked = true;//수면회복제 없음

            optETC1.Checked = true;//특이사항 무

            read_endo_autoSet(pDbCon, gPtno, gRDate);

        }
        
        void read_endo_autoSet(PsmhDb pDbCon, string argPano,string argDate)
        {

            if (gDept =="TO")
            {                
                DataTable dt = null;

                Cursor.Current = Cursors.WaitCursor;

                #region //기본사항
                optBTO.Checked = true;//종검
                chkEat.Checked = true;//금식 예
                chkBodySTS1.Checked = true;//전신상태 양호
                chkDrugB1.Checked = true;//전처치 액제 없음
                optETC1.Checked = true;//특이사항 무
                #endregion

                #region //ENDO_ADD_HIS 병력,약제 - 입력2

                #region //병력 히스토리 관련 변수 세팅
                cEndoAddHis = new clsComSupEndsSQL.cEndoAddHis();
                cEndoAddHis.Ptno = argPano;
                cEndoAddHis.RDate = argDate;
                #endregion

                //쿼리실행      
                dt = cendsSQL.sel_ENDO_ADD_HIS(pDbCon, cEndoAddHis);

                #region //데이터셋 읽어 자료 표시

                if (ComFunc.isDataTableNull(dt) == false)
                {
                    #region //데이타 표시
                    //병력
                    if (dt.Rows[0]["GB_OLD"].ToString().Trim() == "1")
                    {
                        chkOld0.Checked = true;
                    }
                    if (dt.Rows[0]["GB_OLD1"].ToString().Trim() == "1")
                    {
                        chkOld1.Checked = true;
                    }
                    if (dt.Rows[0]["GB_OLD2"].ToString().Trim() == "1")
                    {
                        chkOld2.Checked = true;
                    }
                    if (dt.Rows[0]["GB_OLD3"].ToString().Trim() == "1")
                    {
                        chkOld3.Checked = true;
                    }
                    if (dt.Rows[0]["GB_OLD4"].ToString().Trim() == "1")
                    {
                        chkOld4.Checked = true;
                    }
                    if (dt.Rows[0]["GB_OLD5"].ToString().Trim() == "1")
                    {
                        chkOld5.Checked = true;
                    }
                    if (dt.Rows[0]["GB_OLD6"].ToString().Trim() == "1")
                    {
                        chkOld6.Checked = true;
                    }
                    if (dt.Rows[0]["GB_OLD7"].ToString().Trim() == "1")
                    {
                        chkOld7.Checked = true;
                    }
                    if (dt.Rows[0]["GB_OLD8"].ToString().Trim() == "1")
                    {
                        chkOld8.Checked = true;
                    }
                    if (dt.Rows[0]["GB_OLD9"].ToString().Trim() == "1")
                    {
                        chkOld9.Checked = true;
                    }
                    if (dt.Rows[0]["GB_OLD10"].ToString().Trim() == "1")
                    {
                        chkOld10.Checked = true;
                    }
                    if (dt.Rows[0]["GB_OLD11"].ToString().Trim() == "1")
                    {
                        chkOld11.Checked = true;
                    }
                    if (dt.Rows[0]["GB_OLD12"].ToString().Trim() == "1")
                    {
                        chkOld12.Checked = true;
                    }
                    if (dt.Rows[0]["GB_OLD13"].ToString().Trim() == "1")
                    {
                        chkOld13.Checked = true;
                    }
                    txtOld13.Text = dt.Rows[0]["GB_OLD13_1"].ToString().Trim();
                    if (dt.Rows[0]["GB_OLD14"].ToString().Trim() == "1")
                    {
                        chkOld14.Checked = true;
                    }
                    txtOldEtc.Text = dt.Rows[0]["GB_OLD15_1"].ToString().Trim();

                    //약물
                    if (dt.Rows[0]["GB_DRUG"].ToString().Trim() == "1")
                    {
                        chkDrug0.Checked = true;
                    }
                    if (dt.Rows[0]["GB_DRUG1"].ToString().Trim() == "1")
                    {
                        chkDrug1.Checked = true;
                    }
                    if (dt.Rows[0]["GB_DRUG2"].ToString().Trim() == "1")
                    {
                        chkDrug2.Checked = true;
                    }
                    if (dt.Rows[0]["GB_DRUG3"].ToString().Trim() == "1")
                    {
                        chkDrug3.Checked = true;
                    }
                    if (dt.Rows[0]["GB_DRUG4"].ToString().Trim() == "1")
                    {
                        chkDrug4.Checked = true;
                    }
                    if (dt.Rows[0]["GB_DRUG5"].ToString().Trim() == "1")
                    {
                        chkDrug5.Checked = true;
                    }
                    if (dt.Rows[0]["GB_DRUG6"].ToString().Trim() == "1")
                    {
                        chkDrug6.Checked = true;
                    }
                    if (dt.Rows[0]["GB_DRUG7"].ToString().Trim() == "1")
                    {
                        chkDrug7.Checked = true;
                    }
                    txtDrugEtc.Text = dt.Rows[0]["GB_DRUG8_1"].ToString().Trim();

                    txtDrugA.Text = dt.Rows[0]["GB_DRUG_STOP1"].ToString().Trim();
                    txtDrugB.Text = dt.Rows[0]["GB_DRUG_STOP2"].ToString().Trim();

                    #endregion
                }                
                dt.Dispose();
                dt = null;                

                #endregion

                #endregion

                #region //ENDO_RESULT 수면약제 - 입력2

                dt = cendsSQL.sel_Endo_JupMst_Result(pDbCon, "", argPano, argDate, "TO"," '1','2','7' ");

                #region //데이터셋 읽어 자료 표시

                if (ComFunc.isDataTableNull(dt) == false)
                {
                    #region //데이타 표시
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (chkBEGD1.Checked ==false)
                        {
                            if (dt.Rows[i]["GbJob"].ToString().Trim() == "2")
                            {
                                chkBEGD1.Checked = true;
                                if (dt.Rows[i]["OrderName"].ToString().Trim().Contains("수면") ==true)
                                {
                                    chkBEGD2.Checked = true;
                                    chkSleep1.Checked = true;//수면회복제 없음
                                }
                            }
                        }

                        if (chkBCFS1.Checked == false)
                        {
                            if (dt.Rows[i]["GbJob"].ToString().Trim() == "3")
                            {
                                chkBCFS1.Checked = true;
                                if (dt.Rows[i]["OrderName"].ToString().Trim().Contains("수면") == true)
                                {
                                    chkBCFS2.Checked = true;
                                    chkSleep1.Checked = true;//수면회복제 없음
                                }
                            }
                        }
                        //투여약물
                        if (dt.Rows[i]["GBCON_21"].ToString().Trim() != "")
                        {
                            txtDrugMid.Text = dt.Rows[i]["GBCON_21"].ToString().Trim();
                        }
                        if (dt.Rows[i]["GBCON_31"].ToString().Trim() != "")
                        {
                            txtDrugPro.Text = dt.Rows[i]["GBCON_31"].ToString().Trim();
                        }
                        if (dt.Rows[i]["GBCON_41"].ToString().Trim() != "")
                        {
                            txtDrugPet.Text = dt.Rows[i]["GBCON_41"].ToString().Trim();
                        }
                        //생검
                        
                        if (dt.Rows[i]["Remark6"].ToString().Trim() != "" || dt.Rows[i]["Remark6_2"].ToString().Trim() != "" || dt.Rows[i]["Remark6_3"].ToString().Trim() != "")
                        {
                            optBio2.Checked = true;
                        }
                        else
                        {
                            optBio1.Checked = true;
                        }
                        //CLO
                        if (dt.Rows[i]["GbPro_2"].ToString().Trim() == "Y")
                        {
                            chkCLO.Checked = true;
                        }
                        if (cboBDrName.Text.Trim() =="" && dt.Rows[i]["ResultDrCode"].ToString().Trim() !="")
                        {
                            cboBDrName.Text = clsVbfunc.GetOCSDrNameSabun(pDbCon, dt.Rows[i]["ResultDrCode"].ToString().Trim());
                        }
                        

                    }

                    #endregion
                }
                dt.Dispose();
                dt = null;

                #endregion

                #endregion

                Cursor.Current = Cursors.Default;

            }

        }

        string read_endo_chart_gubun(string argGubun)
        {
            if (argGubun =="1")
            {
                return "외래";
            }
            else if (argGubun == "2")
            {
                return "병실";
            }
            else if (argGubun == "3")
            {
                return "신검";
            }
            else if (argGubun == "4")
            {
                return "종검";
            }
            else if (argGubun == "5")
            {
                return "응급";
            }
            else if (argGubun == "6")
            {
                return "신/종";
            }
            else 
            {
                return "";
            }            
        }

        string read_endo_chart_exam(PsmhDb pDbCon, DataTable dt,int row)
        {
            string s = "";
            string s2 = "";
            DataTable dt2 = cendsSQL.sel_Endo_JupMst_Result(pDbCon, "", dt.Rows[row]["Ptno"].ToString().Trim(), dt.Rows[row]["RDate"].ToString().Trim(), "TO", " '1','2','7' ");
            if (ComFunc.isDataTableNull(dt2) == false)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    if (dt.Rows[row]["Gb_EGD1"].ToString().Trim() != "1")
                    {
                        if (dt2.Rows[i]["GbJob"].ToString().Trim() == "2")
                        {
                            if (dt2.Rows[i]["OrderName"].ToString().Trim().Contains("수면")==true)
                            {
                                s += "EGD 수면,";
                            }
                            else
                            {
                                s += "EGD 일반,";
                            }
                        }
                    }
                    if (dt.Rows[row]["Gb_CFS1"].ToString().Trim() != "1")
                    {
                        if (dt2.Rows[i]["GbJob"].ToString().Trim() == "3")
                        {
                            if (dt2.Rows[i]["OrderName"].ToString().Trim().Contains("수면") == true)
                            {
                                s += "CFS 수면,";
                            }
                            else
                            {
                                s += "CFS 일반,";
                            }
                        }
                    }
                    if (s2 =="")
                    {
                        s2 =  clsVbfunc.GetOCSDrNameSabun(pDbCon, dt2.Rows[i]["ResultDrCode"].ToString().Trim());
                    }                    
                }
                ssList2.ActiveSheet.Cells[row, (int)clsComSupEndsSpd.enmSupEndsSet02B.DrName].Text = s2;
            }
            else
            {
                if (dt.Rows[row]["Gb_EGD1"].ToString().Trim()=="1")
                {
                    if (dt.Rows[row]["Gb_EGD2"].ToString().Trim() == "1")
                    {
                        s += "EGD 수면,";
                    }
                    else
                    {
                        s += "EGD 일반,";
                    }
                }
                if (dt.Rows[row]["Gb_CFS1"].ToString().Trim() == "1")
                {
                    if (dt.Rows[row]["Gb_CFS2"].ToString().Trim() == "1")
                    {
                        s += "CFS 수면,";
                    }
                    else
                    {
                        s += "CFS 일반,";
                    }
                }
                if (dt.Rows[row]["Gb_Sig1"].ToString().Trim() == "1")
                {
                    if (dt.Rows[row]["Gb_Sig2"].ToString().Trim() == "1")
                    {
                        s += "Sigmodioscopy 수면,";
                    }
                    else
                    {
                        s += "Sigmodioscopy 일반,";
                    }
                }
                if (dt.Rows[row]["Gb_Bro1"].ToString().Trim() == "1")
                {
                    if (dt.Rows[row]["Gb_Bro2"].ToString().Trim() == "1")
                    {
                        s += "Bronchoscopy 수면,";
                    }
                    else
                    {
                        s += "Bronchoscopy 일반,";
                    }
                }
                if (dt.Rows[row]["Gb_ERCP1"].ToString().Trim() == "1")
                {
                    if (dt.Rows[row]["Gb_ERCP2"].ToString().Trim() == "1")
                    {
                        s += "ERCP 수면,";
                    }
                    else
                    {
                        s += "ERCP 일반,";
                    }
                }
                ssList2.ActiveSheet.Cells[row, (int)clsComSupEndsSpd.enmSupEndsSet02B.DrName].Text = dt.Rows[row]["R_DrName"].ToString().Trim();
            }

            return s;
        }

        bool read_endo_chart(PsmhDb pDbCon, string argPtno,string argBDate,string argRDate)
        {
            gChk1 = "";
            gChk2 = "";
            gChk3 = "";

            DataTable dt = cendsSQL.sel_ENDO_CHART(pDbCon, argPtno, "", "",argRDate);
            if (ComFunc.isDataTableNull(dt) == false)
            {                
                if (dt.Rows[0]["GB_OLD"].ToString().Trim()=="1")
                {
                    gChk1 = "OK";
                }
                else
                {
                    if (dt.Rows[0]["GB_OLD1"].ToString().Trim() == "1") gChk1 = "OK";
                    if (dt.Rows[0]["GB_OLD2"].ToString().Trim() == "1") gChk1 = "OK";
                    if (dt.Rows[0]["GB_OLD3"].ToString().Trim() == "1") gChk1 = "OK";
                    if (dt.Rows[0]["GB_OLD4"].ToString().Trim() == "1") gChk1 = "OK";
                    if (dt.Rows[0]["GB_OLD5"].ToString().Trim() == "1") gChk1 = "OK";
                    if (dt.Rows[0]["GB_OLD6"].ToString().Trim() == "1") gChk1 = "OK";
                    if (dt.Rows[0]["GB_OLD7"].ToString().Trim() == "1") gChk1 = "OK";
                    if (dt.Rows[0]["GB_OLD8"].ToString().Trim() == "1") gChk1 = "OK";
                    if (dt.Rows[0]["GB_OLD9"].ToString().Trim() == "1") gChk1 = "OK";
                    if (dt.Rows[0]["GB_OLD10"].ToString().Trim() == "1") gChk1 = "OK";
                    if (dt.Rows[0]["GB_OLD11"].ToString().Trim() == "1") gChk1 = "OK";
                    if (dt.Rows[0]["GB_OLD12"].ToString().Trim() == "1") gChk1 = "OK";
                    if (dt.Rows[0]["GB_OLD13"].ToString().Trim() == "1") gChk1 = "OK";
                    if (dt.Rows[0]["GB_OLD14"].ToString().Trim() == "1") gChk1 = "OK";
                    if (dt.Rows[0]["GB_OLD15_1"].ToString().Trim() != "") gChk1 = "OK";
                }

                if (dt.Rows[0]["GB_DRUG"].ToString().Trim() == "1")
                {
                    gChk2 = "OK";
                }
                else
                {
                    if (dt.Rows[0]["GB_DRUG1"].ToString().Trim() == "1") gChk2 = "OK";
                    if (dt.Rows[0]["GB_DRUG2"].ToString().Trim() == "1") gChk2 = "OK";
                    if (dt.Rows[0]["GB_DRUG3"].ToString().Trim() == "1") gChk2 = "OK";
                    if (dt.Rows[0]["GB_DRUG4"].ToString().Trim() == "1") gChk2 = "OK";
                    if (dt.Rows[0]["GB_DRUG5"].ToString().Trim() == "1") gChk2 = "OK";
                    if (dt.Rows[0]["GB_DRUG6"].ToString().Trim() == "1") gChk2 = "OK";
                    if (dt.Rows[0]["GB_DRUG7"].ToString().Trim() == "1") gChk2 = "OK";
                    if (dt.Rows[0]["GB_DRUG8_1"].ToString().Trim() == "1") gChk2 = "OK";
                }

                if (dt.Rows[0]["Gb_NUR_NAME"].ToString().Trim() != "")
                {
                    gChk3 = "OK";
                }

                return true;

            }
            else
            {
                return false;
            }

        }

        bool read_endo_hic_am_chk(PsmhDb pDbCon, string argPtno,string argRDate)
        {
            DataTable dt = cendsSQL.sel_HIC_JEPSU_RESULT(pDbCon, argRDate, argPtno, " 'TX20','TX23','TX32','TX41','TX64' ", " '31','35' ");
            if (ComFunc.isDataTableNull(dt) == false)
            {
                return true;
            }
            else
            {
                return false;
            }                
        }

        void screen_display()
        {
            GetData(clsDB.DbCon, ssList, dtpFDate.Text.Trim());
        }

        void screen_display2(string argPtno)
        {
            screen_clear();
            screen_clear2();
            GetData2(clsDB.DbCon, ssList2, argPtno);            
        }                

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argDate)
        {
            int i = 0;
            int nRow = -1;
            string strOK = "";
            string strPtno = "";
            string strBDate = "";
            string strRDate = "";
            bool bInputChk = false;

            DataTable dt = null;
            
            Spd.ActiveSheet.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            #region //Biz 쿼리 변수세팅 및 쿼리실행
            cEndoJupmst = new clsComSupEndsSQL.cEndoJupmst();
            cEndoJupmst.STS = "7";
            cEndoJupmst.Job = "*";
            cEndoJupmst.Date1 = dtpFDate.Text.Trim();
            cEndoJupmst.Date2 = dtpTDate.Text.Trim();
            cEndoJupmst.DrCode =  clsComSup.setP(cboDrCode.SelectedItem.ToString().Trim(),".",1);
            cEndoJupmst.GbSunap = "ALL";
            if (optJobC2.Checked == true)
            {
                cEndoJupmst.GbSunap = "1";
            }
            else if (optJobC3.Checked == true)
            {
                cEndoJupmst.GbSunap = "2";
            }
            else if (optJobC4.Checked == true)
            {
                cEndoJupmst.GbSunap = "3";
            }
            cEndoJupmst.Buse = "ALL";
            if (optJobB2.Checked == true)
            {
                cEndoJupmst.Buse = "본관";
            }
            else if (optJobB3.Checked == true)
            {
                cEndoJupmst.Buse = "마리아관";
            }

            #endregion

            dt = cendsSQL.sel_ENDO_JUPMST(pDbCon, cEndoJupmst,false);

            #region //데이터셋 읽어 자료 표시
            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strOK = "OK";

                    strPtno = dt.Rows[i]["Ptno"].ToString().Trim();
                    strBDate = dt.Rows[i]["BDate"].ToString().Trim();
                    strRDate = dt.Rows[i]["RDate"].ToString().Trim();

                    bInputChk = read_endo_chart(pDbCon,strPtno, strBDate, strRDate);

                    if (optJobA1.Checked ==true)
                    {
                        if (bInputChk == true)
                        {
                            strOK = "";
                        }
                    }
                    else if (optJobA2.Checked == true)
                    {
                        if (bInputChk == false)
                        {
                            strOK = "";
                        }
                    }

                    if (chkAM.Checked ==true)
                    {
                        if (read_endo_hic_am_chk(pDbCon,strPtno, strRDate) == false)
                        {
                            strOK = "";
                        }
                    }

                    if (strOK =="OK")
                    {
                        nRow++;

                        Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsSet02A.Ptno].Text = strPtno;
                        Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsSet02A.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                        if (gChk1 =="" || gChk2 =="")
                        {
                            Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsSet02A.SName].BackColor = System.Drawing.Color.LightPink;
                        }
                        else
                        {
                            Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsSet02A.SName].BackColor = System.Drawing.Color.White;
                        }
                        Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsSet02A.BDate].Text = strBDate;
                        Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsSet02A.RDate].Text = strRDate;
                        Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsSet02A.DeptCode].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        if (gChk3 == "" )
                        {
                            Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsSet02A.DeptCode].BackColor = System.Drawing.Color.LightPink;
                        }
                        else
                        {
                            Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsSet02A.DeptCode].BackColor = System.Drawing.Color.White;
                        }
                        Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsSet02A.Gubun].Text = dt.Rows[i]["JupsuSTS"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsSet02A.JDate].Text = dt.Rows[i]["JDate"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsSet02A.STS].Text = "";
                        if (bInputChk==true)
                        {
                            Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsSet02A.STS].Text = "Y";
                            Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsSet02A.STS].BackColor = System.Drawing.Color.FromArgb(255, 255, 0);
                        }
                        Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsSet02A.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                        if (dt.Rows[i]["GbSunap"].ToString().Trim() == "*")
                        {
                            Spd.ActiveSheet.Rows.Get(nRow).ForeColor = System.Drawing.Color.FromArgb(255, 0, 0); 
                        }
                        else  if (dt.Rows[i]["ResultDate"].ToString().Trim() !="")
                        {
                            Spd.ActiveSheet.Rows.Get(nRow).ForeColor = System.Drawing.Color.FromArgb(0,128,0); 
                        }
                    }
                }

                Spd.ActiveSheet.RowCount = nRow + 1;
            }

            #endregion

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

        }

        void GetData2(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argPtno)
        {
            int i = 0;
            DataTable dt = null;

            Spd.ActiveSheet.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;                       
                       
            dt = cendsSQL.sel_ENDO_CHART(pDbCon, argPtno,"","",""," EntDate ");

            #region //데이터셋 읽어 자료 표시
            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT-3);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsSet02B.chk].Text = "";
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsSet02B.Ptno].Text = dt.Rows[i]["Ptno"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsSet02B.Gubun].Text = read_endo_chart_gubun(dt.Rows[i]["Gubun"].ToString().Trim());
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsSet02B.BDate].Text = dt.Rows[i]["BDate"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsSet02B.RDate].Text = dt.Rows[i]["RDate"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsSet02B.STS].Text = read_endo_chart_exam( pDbCon, dt,i);
                    //Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsSet02B.DrName].Text = dt.Rows[i]["R_DrName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsSet02B.NrName].Text = dt.Rows[i]["Gb_Nur_Name"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsSet02B.EMRNO].Text = dt.Rows[i]["EMRNO"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsSet02B.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }
            }

            #endregion

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

        }

        void GetData3(PsmhDb pDbCon, string argROWID,long argEMRNO)
        {            
            DataTable dt = null;

            screen_clear();
            
            //쿼리실행      
            dt = cendsSQL.sel_ENDO_CHART(pDbCon, "", argROWID);

            #region //데이터셋 읽어 자료 표시

            if (ComFunc.isDataTableNull(dt) == false)
            {
                #region //기본사항
                lblSTS.Text = "수정작업중";
                lblSTS.BackColor = System.Drawing.Color.FromArgb(255, 192, 255);

                txtPtno.Text = dt.Rows[0]["Ptno"].ToString().Trim();
                lblSName.Text = clsVbfunc.GetPatientName(pDbCon, dt.Rows[0]["Ptno"].ToString().Trim());
                                
                if (dt.Rows[0]["Gubun"].ToString().Trim()=="1")
                {
                    optBOpd.Checked = true;
                }
                else if (dt.Rows[0]["Gubun"].ToString().Trim() == "2")
                {
                    optBWard.Checked = true;
                }
                else if (dt.Rows[0]["Gubun"].ToString().Trim() == "3")
                {
                    optBHR.Checked = true;
                }
                else if (dt.Rows[0]["Gubun"].ToString().Trim() == "4")
                {
                    optBTO.Checked = true;
                }
                else if (dt.Rows[0]["Gubun"].ToString().Trim() == "5")
                {
                    optBER.Checked = true;
                }
                else if (dt.Rows[0]["Gubun"].ToString().Trim() == "6")
                {
                    optBHRTO.Checked = true;
                }

                dtpBDate.Enabled = false;
                dtpBDate.Text = dt.Rows[0]["BDate"].ToString().Trim();
                dtpRDate.Text = dt.Rows[0]["RDate"].ToString().Trim();

                //검사종류
                if (dt.Rows[0]["GB_EGD1"].ToString().Trim()=="1")
                {
                    chkBEGD1.Checked = true;
                }
                if (dt.Rows[0]["GB_EGD2"].ToString().Trim() == "1")
                {
                    chkBEGD2.Checked = true;
                }
                if (dt.Rows[0]["GB_CFS1"].ToString().Trim() == "1")
                {
                    chkBCFS1.Checked = true;
                }
                if (dt.Rows[0]["GB_CFS2"].ToString().Trim() == "1")
                {
                    chkBCFS2.Checked = true;
                }
                if (dt.Rows[0]["GB_SIG1"].ToString().Trim() == "1")
                {
                    chkBSig1.Checked = true;
                }
                if (dt.Rows[0]["GB_SIG2"].ToString().Trim() == "1")
                {
                    chkBSig2.Checked = true;
                }
                if (dt.Rows[0]["GB_BRO1"].ToString().Trim() == "1")
                {
                    chkBBro1.Checked = true;
                }
                if (dt.Rows[0]["GB_BRO2"].ToString().Trim() == "1")
                {
                    chkBBro2.Checked = true;
                }
                if (dt.Rows[0]["GB_ERCP1"].ToString().Trim() == "1")
                {
                    chkBERCP1.Checked = true;
                }
                if (dt.Rows[0]["GB_ERCP2"].ToString().Trim() == "1")
                {
                    chkBERCP2.Checked = true;
                }

                cboBDrName.Text = dt.Rows[0]["R_DrName"].ToString().Trim();

                #endregion

                #region //입력1 데이타 표시
                //
                if (dt.Rows[0]["GB_DIET"].ToString().Trim() == "1")
                {
                    chkEat.Checked = true;
                }
                //전신상태
                if (dt.Rows[0]["GB_STS"].ToString().Trim() == "1")
                {
                    chkBodySTS1.Checked = true;
                }
                else if (dt.Rows[0]["GB_STS"].ToString().Trim() == "2")
                {
                    chkBodySTS2.Checked = true;
                }
                else if (dt.Rows[0]["GB_STS"].ToString().Trim() == "3")
                {
                    chkBodySTS3.Checked = true;
                }
                else if (dt.Rows[0]["GB_STS"].ToString().Trim() == "4")
                {
                    chkBodySTS4.Checked = true;
                }


                //병력
                if (dt.Rows[0]["GB_OLD"].ToString().Trim() == "1")
                {
                    chkOld0.Checked = true;
                }
                if (dt.Rows[0]["GB_OLD1"].ToString().Trim() == "1")
                {
                    chkOld1.Checked = true;
                }
                if (dt.Rows[0]["GB_OLD2"].ToString().Trim() == "1")
                {
                    chkOld2.Checked = true;
                }
                if (dt.Rows[0]["GB_OLD3"].ToString().Trim() == "1")
                {
                    chkOld3.Checked = true;
                }
                if (dt.Rows[0]["GB_OLD4"].ToString().Trim() == "1")
                {
                    chkOld4.Checked = true;
                }
                if (dt.Rows[0]["GB_OLD5"].ToString().Trim() == "1")
                {
                    chkOld5.Checked = true;
                }
                if (dt.Rows[0]["GB_OLD6"].ToString().Trim() == "1")
                {
                    chkOld6.Checked = true;
                }
                if (dt.Rows[0]["GB_OLD7"].ToString().Trim() == "1")
                {
                    chkOld7.Checked = true;
                }
                if (dt.Rows[0]["GB_OLD8"].ToString().Trim() == "1")
                {
                    chkOld8.Checked = true;
                }
                if (dt.Rows[0]["GB_OLD9"].ToString().Trim() == "1")
                {
                    chkOld9.Checked = true;
                }
                if (dt.Rows[0]["GB_OLD10"].ToString().Trim() == "1")
                {
                    chkOld10.Checked = true;
                }
                if (dt.Rows[0]["GB_OLD11"].ToString().Trim() == "1")
                {
                    chkOld11.Checked = true;
                }
                if (dt.Rows[0]["GB_OLD12"].ToString().Trim() == "1")
                {
                    chkOld12.Checked = true;
                }
                if (dt.Rows[0]["GB_OLD13"].ToString().Trim() == "1")
                {
                    chkOld13.Checked = true;
                }
                txtOld13.Text = dt.Rows[0]["GB_OLD13_1"].ToString().Trim();
                if (dt.Rows[0]["GB_OLD14"].ToString().Trim() == "1")
                {
                    chkOld14.Checked = true;
                }
                txtOldEtc.Text = dt.Rows[0]["GB_OLD15_1"].ToString().Trim();

                //약물
                if (dt.Rows[0]["GB_DRUG"].ToString().Trim() == "1")
                {
                    chkDrug0.Checked = true;
                }
                if (dt.Rows[0]["GB_DRUG1"].ToString().Trim() == "1")
                {
                    chkDrug1.Checked = true;
                }
                if (dt.Rows[0]["GB_DRUG2"].ToString().Trim() == "1")
                {
                    chkDrug2.Checked = true;
                }
                if (dt.Rows[0]["GB_DRUG3"].ToString().Trim() == "1")
                {
                    chkDrug3.Checked = true;
                }
                if (dt.Rows[0]["GB_DRUG4"].ToString().Trim() == "1")
                {
                    chkDrug4.Checked = true;
                }
                if (dt.Rows[0]["GB_DRUG5"].ToString().Trim() == "1")
                {
                    chkDrug5.Checked = true;
                }
                if (dt.Rows[0]["GB_DRUG6"].ToString().Trim() == "1")
                {
                    chkDrug6.Checked = true;
                }
                if (dt.Rows[0]["GB_DRUG7"].ToString().Trim() == "1")
                {
                    chkDrug7.Checked = true;
                }
                txtDrugEtc.Text = dt.Rows[0]["GB_DRUG8_1"].ToString().Trim();

                txtDrugA.Text = dt.Rows[0]["GB_DRUG_STOP1"].ToString().Trim();
                txtDrugB.Text = dt.Rows[0]["GB_DRUG_STOP2"].ToString().Trim();

                //전처지 약제
                if (dt.Rows[0]["GB_B_DRUG"].ToString().Trim()=="1")
                {
                    chkDrugB1.Checked = true;
                }
                if (dt.Rows[0]["GB_B_DRUG1"].ToString().Trim() == "1")
                {
                    chkDrugB2.Checked = true;
                }
                txtDrugB1.Text = dt.Rows[0]["GB_B_DRUG1_1"].ToString().Trim();
                txtBigo.Text = dt.Rows[0]["Gb_Bigo"].ToString().Trim();

                #endregion

                #region //입력2 데이타 표시
                //투여약물
                txtDrugMid.Text = dt.Rows[0]["GB_SLEEP_DRUG1"].ToString().Trim();
                txtDrugPro.Text = dt.Rows[0]["GB_SLEEP_DRUG2"].ToString().Trim();
                txtDrugPet.Text = dt.Rows[0]["GB_SLEEP_DRUG3"].ToString().Trim();
                txtDrug.Text = dt.Rows[0]["GB_SLEEP_DRUG_ETC"].ToString().Trim();
                //수면회복제
                if (dt.Rows[0]["GB_SLEEP_RE_DRUG"].ToString().Trim()=="1")
                {
                    chkSleep1.Checked = true;
                }
                if (dt.Rows[0]["GB_SLEEP_RE_DRUG1"].ToString().Trim() == "1")
                {
                    chkSleep2.Checked = true;
                }
                txtSleep.Text = dt.Rows[0]["GB_SLEEP_RE_DRUG1_1"].ToString().Trim();
                //장정결
                if (dt.Rows[0]["GB_STS1"].ToString().Trim() == "1")
                {
                    chkClean1.Checked = true;
                }
                else if (dt.Rows[0]["GB_STS1"].ToString().Trim() == "2")
                {
                    chkClean2.Checked = true;
                }
                else if (dt.Rows[0]["GB_STS1"].ToString().Trim() == "3")
                {
                    chkClean3.Checked = true;
                }
                if (dt.Rows[0]["GB_STS2"].ToString().Trim() == "1")
                {
                    chkCleanSTS1.Checked = true;
                }
                else if (dt.Rows[0]["GB_STS2"].ToString().Trim() == "2")
                {
                    chkCleanSTS2.Checked = true;
                }
                else if (dt.Rows[0]["GB_STS2"].ToString().Trim() == "3")
                {
                    chkCleanSTS3.Checked = true;
                }
                //검사시작,종료
                if ( VB.Len(dt.Rows[0]["Gb_STIME"].ToString().Trim()) == 4)
                {
                    txtSTime.Text = VB.Left(dt.Rows[0]["Gb_STIME"].ToString().Trim(),2) + ":" +  VB.Right(dt.Rows[0]["Gb_STIME"].ToString().Trim(),2);
                }
                if (VB.Len(dt.Rows[0]["Gb_ETIME"].ToString().Trim()) == 4)
                {
                    txtETime.Text = VB.Left(dt.Rows[0]["Gb_ETIME"].ToString().Trim(), 2) + ":" + VB.Right(dt.Rows[0]["Gb_ETIME"].ToString().Trim(), 2);
                }
                //활력징후
                txtSTSA1.Text = dt.Rows[0]["GB_SP0_11"].ToString().Trim();
                txtSTSA2.Text = dt.Rows[0]["GB_SP0_12"].ToString().Trim();
                txtSTSA3.Text = dt.Rows[0]["GB_SP0_13"].ToString().Trim();
                txtSTSA4.Text = dt.Rows[0]["GB_SP0_14"].ToString().Trim();

                txtSTSB1.Text = dt.Rows[0]["GB_SP0_21"].ToString().Trim();
                txtSTSB2.Text = dt.Rows[0]["GB_SP0_22"].ToString().Trim();
                txtSTSB3.Text = dt.Rows[0]["GB_SP0_23"].ToString().Trim();
                txtSTSB4.Text = dt.Rows[0]["GB_SP0_24"].ToString().Trim();

                txtSTSC1.Text = dt.Rows[0]["GB_SP0_31"].ToString().Trim();
                txtSTSC2.Text = dt.Rows[0]["GB_SP0_32"].ToString().Trim();
                txtSTSC3.Text = dt.Rows[0]["GB_SP0_33"].ToString().Trim();
                txtSTSC4.Text = dt.Rows[0]["GB_SP0_34"].ToString().Trim();

                txtSTSD1.Text = dt.Rows[0]["GB_SP0_41"].ToString().Trim();
                txtSTSD2.Text = dt.Rows[0]["GB_SP0_42"].ToString().Trim();
                //수면평가
                if (dt.Rows[0]["GB_SLEEP_STS1"].ToString().Trim() == "1")
                {
                    chkSleepSTS1.Checked = true;
                }
                if (dt.Rows[0]["GB_SLEEP_STS2"].ToString().Trim() == "1")
                {
                    chkSleepSTS2.Checked = true;
                }
                if (dt.Rows[0]["GB_SLEEP_STS3"].ToString().Trim() == "1")
                {
                    chkSleepSTS3.Checked = true;
                }
                if (dt.Rows[0]["GB_SLEEP_STS4"].ToString().Trim() == "1")
                {
                    chkSleepSTS4.Checked = true;
                }
                if (dt.Rows[0]["GB_SLEEP_STS5"].ToString().Trim() == "1")
                {
                    chkSleepSTS5.Checked = true;
                }
                if (dt.Rows[0]["GB_SLEEP_STS6"].ToString().Trim() == "1")
                {
                    chkSleepSTS6.Checked = true;
                }
                if (dt.Rows[0]["GB_SLEEP_STS7"].ToString().Trim() == "1")
                {
                    chkSleepSTS7.Checked = true;
                }
                txtSleepSTS.Text = dt.Rows[0]["GB_SLEEP_STS7_1"].ToString().Trim();
                //생검
                optBio1.Checked = true;
                if (dt.Rows[0]["GB_EXAM"].ToString().Trim() == "1")
                {
                    optBio2.Checked = true;
                }
                if (dt.Rows[0]["GB_CLO"].ToString().Trim() == "1")
                {
                    chkCLO.Checked = true;
                }
                //퇴실기준
                if (dt.Rows[0]["GB_OUT_GUBUN"].ToString().Trim() == "1")
                {
                    optOUT1.Checked = true;
                }
                else if (dt.Rows[0]["GB_OUT_GUBUN"].ToString().Trim() == "2")
                {
                    optOUT2.Checked = true;
                }
                if (dt.Rows[0]["GB_OUT_GUBUN1"].ToString().Trim() == "1")
                {
                    chkOUT1.Checked = true;
                }
                if (dt.Rows[0]["GB_OUT_GUBUN2"].ToString().Trim() == "1")
                {
                    chkOUT2.Checked = true;
                }
                if (dt.Rows[0]["GB_OUT_GUBUN3"].ToString().Trim() == "1")
                {
                    chkOUT3.Checked = true;
                }
                if (dt.Rows[0]["GB_OUT_GUBUN4"].ToString().Trim() == "1")
                {
                    chkOUT4.Checked = true;
                }
                if (dt.Rows[0]["GB_OUT_GUBUN5"].ToString().Trim() == "1")
                {
                    chkOUT5.Checked = true;
                }
                if (dt.Rows[0]["GB_OUT_GUBUN6"].ToString().Trim() == "1")
                {
                    chkOUT6.Checked = true;
                }
                if (dt.Rows[0]["GB_OUT_GUBUN7"].ToString().Trim() == "1")
                {
                    chkOUT7.Checked = true;
                }
                //특이사항
                optETC1.Checked = true;
                if (dt.Rows[0]["GB_NUR_CHART"].ToString().Trim() == "1")
                {
                    optETC2.Checked = true;
                }
                txtETC.Text = dt.Rows[0]["GB_NUR_CHART_Remark"].ToString().Trim();

                //
                if (gDept == "TO")
                {                    
                    if (dt.Rows[0]["GB_NUR_Name"].ToString().Trim() != "")
                    {
                        txtNrSabun.Text = dt.Rows[0]["EntSabun"].ToString().Trim();
                        lblNrName.Text = dt.Rows[0]["GB_NUR_Name"].ToString().Trim();
                    }
                    else
                    {
                        txtNrSabun.Text = clsType.User.Sabun;
                        lblNrName.Text = clsVbfunc.GetInSaName(pDbCon, clsType.User.Sabun);
                    }
                }
                else
                {                    
                    if (dt.Rows[0]["GB_NUR_Name"].ToString().Trim() != "")
                    {
                        txtNrSabun.Text = dt.Rows[0]["EntSabun"].ToString().Trim();
                        lblNrName.Text = dt.Rows[0]["GB_NUR_Name"].ToString().Trim();
                    }
                }

                //신규이면 기본세팅값 
                if (dt.Rows[0]["TO_OK"].ToString().Trim() == "")
                {
                    read_endo_autoSet(pDbCon, txtPtno.Text.Trim(),dtpRDate.Text.Trim());
                }

                lblEMRNO.Text = dt.Rows[0]["EMRNO"].ToString().Trim();

                #endregion

            }
            else
            {
                lblSTS.Text = "신규작업중";
                lblSTS.BackColor = System.Drawing.Color.FromArgb(192, 255, 192);

                dtpBDate.Enabled = true;

            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

            #endregion
        }

    }
}
