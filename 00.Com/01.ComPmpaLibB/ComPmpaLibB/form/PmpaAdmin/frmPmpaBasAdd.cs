using ComBase;
using ComDbB;
using DevComponents.DotNetBar;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace ComPmpaLibB
{
    public partial class frmPmpaBasAdd : Form
    {
        ComFunc CF = new ComFunc();
        clsPmpaPb cPb = new clsPmpaPb();
        clsSpread cSpd = new clsSpread();
        clsComPmpaSpd cPmpaSpd = new clsComPmpaSpd();
        clsIuSent cISent = new clsIuSent();
        clsIuSentChk cISentChk = new clsIuSentChk();
        clsIument cIMent = new clsIument();
        clsBasAcct cBAcct = new clsBasAcct();
        clsPmpaFunc cPF = new clsPmpaFunc();

        clsPmpaType.cBas_Add_Arg cBArg = null;

        Thread thread;
        FpSpread spd;

        //기타업무 버튼x 배열
        ButtonItem[] bitem1 = null;
        
        private string FstrPano = string.Empty;
        private long FnTrsno = 0;
        private string FstrFNu = string.Empty;
        private string FstrTNu = string.Empty;

        public frmPmpaBasAdd()
        {
            InitializeComponent();
            SetEvent();
            SetControl(buttonX1, bitem1);
        }

        public frmPmpaBasAdd(string strPano, long nTrsno)
        {
            FstrPano = strPano;
            FnTrsno = nTrsno;
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnPlus.Click += new EventHandler(eBtnClick);
            this.btnMinus.Click += new EventHandler(eBtnClick);
            this.txtPano.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            //this.btnSave.Click += new EventHandler(eBtnClick);
            this.SS1.CellClick += new CellClickEventHandler(eSpdClick);
            this.SS1.KeyPress += new KeyPressEventHandler(eSpdKeyPress);
            this.SS1.Change += new ChangeEventHandler(eSpdChange);
            this.SS1.EditChange += new EditorNotifyEventHandler(eSpdBtnClick);
            this.SS1.ButtonClicked += new EditorNotifyEventHandler(eSpdBtnClick);
            this.SS1.EditorFocused += new EditorNotifyEventHandler(eSpdFocus);
            this.SSILL.ButtonClicked += new EditorNotifyEventHandler(eSpdBtnClick);
            this.cboNu.Enter += new EventHandler(eFocus);
            this.cboNu.SelectedIndexChanged += new EventHandler(eCboChange);
            this.cboNu.KeyPress += new KeyPressEventHandler(eKeyPress);
            //this.SS1.EditChange += new EditorNotifyEventHandler(eSpreadEditChange);
            //this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);

            this.dtpFDate.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.dtpTDate.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.dtpFDate.ValueChanged += new EventHandler(CF.eDtpFormatSet);
            this.dtpTDate.ValueChanged += new EventHandler(CF.eDtpFormatSet);
            this.txtPano.Click += new EventHandler(eSelStart);
        }

        void eSelStart(object sender, EventArgs e)
        {
            TextBox tP = sender as TextBox;
            tP.SelectionStart = 0;
            tP.SelectionLength = tP.Text.Length;
        }

        private void SetControl(ButtonX btn, ButtonItem[] argBitem)
        {
            int i = 0;
            #region ButtonX Set
            argBitem = new ButtonItem[Enum.GetValues(typeof(clsPmpaPb.enmJSimBtn)).Length];

            for (i = 0; i < Enum.GetValues(typeof(clsPmpaPb.enmJSimBtn)).Length; i++)
            {
                argBitem[i] = new ButtonItem();
            }
            
            argBitem[(int)clsPmpaPb.enmJSimBtn.Diet].Text = cPb.sBtnJSim[(int)clsPmpaPb.enmJSimBtn.Diet];
            argBitem[(int)clsPmpaPb.enmJSimBtn.Diet].Click += new EventHandler(eBtnitemClick);
            argBitem[(int)clsPmpaPb.enmJSimBtn.Exam].Text = cPb.sBtnJSim[(int)clsPmpaPb.enmJSimBtn.Exam];
            argBitem[(int)clsPmpaPb.enmJSimBtn.Exam].Click += new EventHandler(eBtnitemClick);
            argBitem[(int)clsPmpaPb.enmJSimBtn.Consult].Text = cPb.sBtnJSim[(int)clsPmpaPb.enmJSimBtn.Consult];
            argBitem[(int)clsPmpaPb.enmJSimBtn.Consult].Click += new EventHandler(eBtnitemClick);
            argBitem[(int)clsPmpaPb.enmJSimBtn.OCS].Text = cPb.sBtnJSim[(int)clsPmpaPb.enmJSimBtn.OCS];
            argBitem[(int)clsPmpaPb.enmJSimBtn.OCS].Click += new EventHandler(eBtnitemClick);
            argBitem[(int)clsPmpaPb.enmJSimBtn.NonExe].Text = cPb.sBtnJSim[(int)clsPmpaPb.enmJSimBtn.NonExe];
            argBitem[(int)clsPmpaPb.enmJSimBtn.NonExe].Click += new EventHandler(eBtnitemClick);
            argBitem[(int)clsPmpaPb.enmJSimBtn.ExReult].Text = cPb.sBtnJSim[(int)clsPmpaPb.enmJSimBtn.ExReult];
            argBitem[(int)clsPmpaPb.enmJSimBtn.ExReult].Click += new EventHandler(eBtnitemClick);
            argBitem[(int)clsPmpaPb.enmJSimBtn.OP].Text = cPb.sBtnJSim[(int)clsPmpaPb.enmJSimBtn.OP];
            argBitem[(int)clsPmpaPb.enmJSimBtn.OP].Click += new EventHandler(eBtnitemClick);
            argBitem[(int)clsPmpaPb.enmJSimBtn.EMR].Text = cPb.sBtnJSim[(int)clsPmpaPb.enmJSimBtn.EMR];
            argBitem[(int)clsPmpaPb.enmJSimBtn.EMR].Click += new EventHandler(eBtnitemClick);
            argBitem[(int)clsPmpaPb.enmJSimBtn.DM].Text = cPb.sBtnJSim[(int)clsPmpaPb.enmJSimBtn.DM];
            argBitem[(int)clsPmpaPb.enmJSimBtn.DM].Click += new EventHandler(eBtnitemClick);
            argBitem[(int)clsPmpaPb.enmJSimBtn.ILL].Text = cPb.sBtnJSim[(int)clsPmpaPb.enmJSimBtn.ILL];
            argBitem[(int)clsPmpaPb.enmJSimBtn.ILL].Click += new EventHandler(eBtnitemClick);
            argBitem[(int)clsPmpaPb.enmJSimBtn.DRG].Text = cPb.sBtnJSim[(int)clsPmpaPb.enmJSimBtn.DRG];
            argBitem[(int)clsPmpaPb.enmJSimBtn.DRG].Click += new EventHandler(eBtnitemClick);
            argBitem[(int)clsPmpaPb.enmJSimBtn.Anti].Text = cPb.sBtnJSim[(int)clsPmpaPb.enmJSimBtn.Anti];
            argBitem[(int)clsPmpaPb.enmJSimBtn.Anti].Click += new EventHandler(eBtnitemClick);

            btn.SubItems.AddRange(argBitem);
            btn.AutoExpandOnClick = true;
            #endregion

            CF.Combo_BCode_SET(clsDB.DbCon, cboNu, "IPD_누적(재원처방수정)", true, 1, "");
            
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            Screen_Clear();

            cPmpaSpd.sSpd_enmJSimSlip(SS1, cPb.sSpdJSimSlip, cPb.nSpdJSimSlip, 10, 0);
            cPmpaSpd.sSpd_enmJSimMirILL(SSILL, cPb.sJSimMirILL, cPb.nJSimMirILL, 10, 0);

            txtPano.Focus();
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)             //닫기
            {
                this.Close();
            }
            else if (sender == this.btnPlus)
            {
                cSpd.setDel_Ins(SS1, true);
            }
            else if (sender == this.btnMinus)
            {
                if (SS1.ActiveSheet.Cells[SS1.ActiveSheet.ActiveCell.Row.Index, (int)clsPmpaPb.enmJSimSlip.SUCODE].Text == "")
                {
                    //수가항목이 공란이고 소계항목이 아닌경우만
                    if (SS1.ActiveSheet.Cells[SS1.ActiveSheet.ActiveCell.Row.Index, (int)clsPmpaPb.enmJSimSlip.SUB].Text != "Y")
                    {
                        cSpd.setDel_Ins(SS1, false);
                    }
                }
            }
            else if (sender == this.btnCancel)
            {
                Screen_Clear();
            }
            else if (sender == this.btnSearch)      //조회
            {
                DisPlay_MirILLs(clsDB.DbCon);       //청구상병정보 Display
                Display_Slip(clsDB.DbCon, SS1);     //Slip DisPlay
            }
            //else if (sender == this.btnSave)        //등록
            //{
            //    eSaveData(clsDB.DbCon, FstrGbn);
            //}
        }

        private void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                cSpd.setSpdSort(SS1, e.Column, true);
                return;
            }
        }

        private void eSpdKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                cSpd.setEnterKey((FarPoint.Win.Spread.FpSpread)sender, clsSpread.enmSpdEnterKey.Right);
            }
            else if (e.KeyChar == (char)Keys.Down)
            {
                cSpd.setEnterKey((FarPoint.Win.Spread.FpSpread)sender, clsSpread.enmSpdEnterKey.Down);
            }
        }

        private void eSpdChange(object sender, ChangeEventArgs e)
        {
            DataTable Dt = null;
            int nCRow = 0, nREAD = 0, i = 0;
            string strCode = string.Empty;
            string strCurDate = string.Empty;
            string strSugbA = string.Empty;
            string strBun = string.Empty;
            string strPCode = string.Empty;
            string strHang = string.Empty;
            string strPCodeDtl = string.Empty;

            double nBaseAmt = 0, nAMT1 = 0;

            #region Check Data
            if (e.Row < 0 || e.Column < 0)
            {
                return;
            }

            if (SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.SUNAMEK].Text.Trim() != "" && SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.NEW].Text.Trim() != "Y")
            {
                return;
            }

            if (SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.BDATE].Text.Trim() == "")
            {
                SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.BDATE].Text = clsPublic.GstrSysDate;
            }
            
            if (SS1.ActiveSheet.Cells[e.Row, e.Column].Text != "")
            {
                SS1.ActiveSheet.Cells[e.Row, e.Column].Text = SS1.ActiveSheet.Cells[e.Row, e.Column].Text.ToUpper();
            }
            #endregion

            strCurDate = SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.BDATE].Text == "" ? clsPublic.GstrSysDate : SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.BDATE].Text;

            if (e.Column == (int)clsPmpaPb.enmJSimSlip.BCODE)
            {
                if (string.Compare(strCurDate, clsPmpaType.TIT.InDate) < 0)
                {
                    //TODO : 실제 오픈시 입원일자 체크해야함
                    //ComFunc.MsgBox("처방일자가 입원일자 이전 입니다.","확인");
                    //SS1.Focus();
                    //return;
                }
            }
            else if (e.Column == (int)clsPmpaPb.enmJSimSlip.SUCODE)
            {
                #region 수가입력
                strCode = SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.SUCODE].Text.Trim();
                
                try
                {
                    #region A항 값 읽어오기 BAS_SUT, BAS_SUN (그룹인지 아닌지)
                    Dt = cISent.sel_Suga_Read_SugbA(clsDB.DbCon, strCode);
                    if (Dt == null) return;

                    if (Dt.Rows.Count > 0)
                    {
                        strBun = Dt.Rows[0]["BUN"].ToString().Trim();
                        strSugbA = Dt.Rows[0]["SUGBA"].ToString().Trim();
                    }

                    Dt.Dispose();
                    Dt = null;
                    #endregion

                    #region 수가 Read View_Suga_Code
                    Dt = cISent.sel_ViewSuga_Read(clsDB.DbCon, strCode, strSugbA);
                    if (Dt == null) return;
                    
                    nREAD = Dt.Rows.Count;
                    nCRow = SS1.ActiveSheet.ActiveRowIndex;
                    #endregion

                    if (nREAD > 0)
                    {
                        for (i = 0; i < nREAD; i++)
                        {
                            #region 수가기본정보 Display
                            if (i > 0)
                            {
                                cSpd.setDel_Ins(SS1, true);
                            }

                            if (string.Compare(Dt.Rows[i]["DelDate"].ToString(), strCurDate) >= 0)
                            {
                                ComFunc.MsgBox(strCode + " 삭제된 코드입니다", "확인");
                                Dt.Dispose();
                                Dt = null;
                                cSpd.Spread_Clear_Range(SS1, e.Row, 0, 1, SS1_Sheet1.ColumnCount);
                                SS1.Focus();
                                return;
                            }

                            cSpd.setSpdForeColor(SS1, nCRow, 0, nCRow, SS1_Sheet1.ColumnCount - 1, Color.Blue);

                            clsSpread.gSdCboItemFindLeft(SS1, nCRow, (int)clsPmpaPb.enmJSimSlip.GBSELF, 1, "");

                            SS1_Sheet1.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.BDATE].Text = strCurDate;
                            SS1_Sheet1.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.SUCODE].Text = Dt.Rows[i]["SUCODE"].ToString().Trim();
                            SS1_Sheet1.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.SUNEXT].Text = Dt.Rows[i]["SUNEXT"].ToString().Trim();
                            SS1_Sheet1.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.SUNAMEK].Text = Dt.Rows[i]["SUNAMEK"].ToString().Trim();
                            SS1_Sheet1.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.BUN].Text = Dt.Rows[i]["BUN"].ToString().Trim();
                            SS1_Sheet1.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.NU].Text = Dt.Rows[i]["NU"].ToString().Trim();
                            SS1_Sheet1.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.GBSGADD].Locked = Dt.Rows[i]["SUGBY"].ToString().Trim() == "0" ? true : false;
                            SS1_Sheet1.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.GBSUGBZ].Locked = Dt.Rows[i]["SUGBZ"].ToString().Trim() == "0" ? true : false;
                            SS1_Sheet1.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.GBSUGBAB].Locked = Dt.Rows[i]["SUGBAB"].ToString().Trim() == "0" ? true : false;
                            SS1_Sheet1.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.GBSUGBAC].Locked = Dt.Rows[i]["SUGBAC"].ToString().Trim() == "0" ? true : false;
                            SS1_Sheet1.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.GBSUGBAD].Locked = Dt.Rows[i]["SUGBAD"].ToString().Trim() == "0" ? true : false;
                            SS1_Sheet1.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.BCODE].Text = Dt.Rows[i]["BCODE"].ToString().Trim();
                            SS1_Sheet1.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.QTY].Text = "1";
                            SS1_Sheet1.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.NAL].Text = "1";
                            #endregion
                            
                            if (Dt.Rows[i]["BCODE"].ToString().Trim() == "999999" || Dt.Rows[i]["SUGBP"].ToString().Trim() == "1")
                            {
                                #region BAS_SUN Table
                                if (string.Compare(Dt.Rows[i]["SUDATE"].ToString(), strCurDate) <= 0)
                                {
                                    SS1_Sheet1.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.BASEAMT].Text = VB.Val(Dt.Rows[i]["Bamt"].ToString().Trim()).ToString("###,###,##0");
                                }
                                else if (string.Compare(Dt.Rows[i]["SUDATE3"].ToString(), strCurDate) <= 0)
                                {
                                    SS1_Sheet1.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.BASEAMT].Text = VB.Val(Dt.Rows[i]["OldBamt"].ToString().Trim()).ToString("###,###,##0");
                                }
                                else if (string.Compare(Dt.Rows[i]["SUDATE4"].ToString(), strCurDate) <= 0)
                                {
                                    SS1_Sheet1.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.BASEAMT].Text = VB.Val(Dt.Rows[i]["Bamt3"].ToString().Trim()).ToString("###,###,##0");
                                }
                                else if (string.Compare(Dt.Rows[i]["SUDATE5"].ToString(), strCurDate) <= 0)
                                {
                                    SS1_Sheet1.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.BASEAMT].Text = VB.Val(Dt.Rows[i]["Bamt4"].ToString().Trim()).ToString("###,###,##0");
                                }
                                else
                                {
                                    SS1_Sheet1.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.BASEAMT].Text = VB.Val(Dt.Rows[i]["Bamt5"].ToString().Trim()).ToString("###,###,##0");
                                }

                                SS1_Sheet1.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.AMT1].Text = SS1_Sheet1.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.BASEAMT].Text;
                                #endregion
                            }
                            else
                            {
                                #region EDI_SUGA Table

                                strPCode = cISentChk.Rtn_Bas_Sun_BCode(clsDB.DbCon, SS1_Sheet1.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.SUNEXT].Text, strCurDate);  //보험코드
                                strHang = cBAcct.Bas_Acct_Hang_Set(SS1_Sheet1.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.BUN].Text);

                                cBArg = new clsPmpaType.cBas_Add_Arg();

                                cBArg.AGE       = Convert.ToInt16(clsPmpaType.TIT.Age);
                                cBArg.JUMINNO   = clsPmpaType.TIT.Jumin1 + clsPmpaType.TIT.Jumin3;
                                cBArg.BDATE     = strCurDate;
                                cBArg.GBER      = VB.Left(SS1.ActiveSheet.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.GBER].Text, 1);                       //응급 가산
                                cBArg.NIGHT     = VB.Left(SS1.ActiveSheet.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.GBNGT].Text, 1);                      //공휴, 야간 가산
                                cBArg.AN1       = VB.Left(SS1.ActiveSheet.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.GBSUGBAC].Text, 1);                   //마취 가산
                                cBArg.OP1       = SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.GBSGADD].Text == "True" ? "1" : cBArg.OP1;    //외과 가산
                                cBArg.OP1       = SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.GBSUGBZ].Text == "True" ? "2" : cBArg.OP1;    //흉부외과 가산
                                cBArg.OP2       = VB.Left(SS1.ActiveSheet.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.GBSUGBAD].Text, 1);                   //화상 가산          
                                cBArg.XRAY1     = VB.Left(SS1.ActiveSheet.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.GBSUGBAB].Text, 1);                   //판독 가산

                                cBAcct.Bas_Add_Set(ref cBArg, strHang);     //가산항목 세팅
                                strPCodeDtl = cBAcct.PCode_00_Process(clsDB.DbCon, strPCode, cBArg, strHang);   //최종가산코드를 붙인 EDI코드
                                nBaseAmt = Convert.ToInt64(cBAcct.Read_EDI_SUGA_PCode(strPCode, strCurDate));   //단가

                                //보험코드가 이미 가산된코드는 그냥 조회
                                if (strPCode.Length > 7)
                                {
                                    nAMT1 = Convert.ToInt64(cBAcct.Read_EDI_SUGA_PCode(strPCode, strCurDate));      //계산금액
                                    SS1_Sheet1.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.BCODE].Text = strPCode;
                                }
                                else
                                {
                                    nAMT1 = Convert.ToInt64(cBAcct.Read_EDI_SUGA_PCode(strPCodeDtl, strCurDate));   //계산금액
                                    SS1_Sheet1.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.BCODE].Text = strPCodeDtl;
                                }
                                
                                SS1_Sheet1.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.BCODE].Text = strPCodeDtl;
                                SS1_Sheet1.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.BASEAMT].Text = nBaseAmt.ToString(); //단가
                                SS1_Sheet1.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.AMT1].Text = nAMT1.ToString();       //발생금액
                                #endregion
                            }
                        }
                        SS1_Sheet1.Cells[nCRow, (int)clsPmpaPb.enmJSimSlip.NEW].Text = "Y";
                    }

                    Dt.Dispose();
                    Dt = null;
                    
                }
                catch (Exception ex)
                {
                    if (Dt != null)
                    {
                        Dt.Dispose();
                        Dt = null;
                    }
                    ComFunc.MsgBox(ex.Message);
                }
                #endregion
            }
        }

        private void eSpdBtnClick(object sender, EditorNotifyEventArgs e)
        {
            string strPCode = string.Empty;
            string strPCodeDtl = string.Empty;
            string strHang = string.Empty;
            string strCurDate = string.Empty;
            double nAMT1 = 0;

            FpSpread o = (FpSpread)sender;

            if (o.Name == "SS1")
            {
                if (SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.chk01].Text == "True")
                {
                    cSpd.setSpdForeColor(SS1, e.Row, 0, e.Row, SS1_Sheet1.ColumnCount - 1, System.Drawing.Color.Red);
                }
                
                else if (e.Column >= (int)clsPmpaPb.enmJSimSlip.GBNGT && e.Column <= (int)clsPmpaPb.enmJSimSlip.GBSUGBAD)
                {
                    #region 가산항목수정
                    if (SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.SUCODE].Text != "")
                    {
                        if (SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.QTY].Text == "" || SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.NAL].Text == "")
                        {
                            SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.AMT1].Text = "0";
                            return;
                        }

                        strCurDate = SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.BDATE].Text == "" ? clsPublic.GstrSysDate : SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.BDATE].Text;
                        strPCode = cISentChk.Rtn_Bas_Sun_BCode(clsDB.DbCon, SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.SUNEXT].Text, strCurDate);  //보험코드
                        strHang = cBAcct.Bas_Acct_Hang_Set(SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.BUN].Text);

                        cBArg = new clsPmpaType.cBas_Add_Arg();

                        cBArg.AGE = Convert.ToInt16(clsPmpaType.TIT.Age);
                        cBArg.JUMINNO = clsPmpaType.TIT.Jumin1 + clsPmpaType.TIT.Jumin3;
                        cBArg.BDATE = strCurDate;
                        cBArg.GBER = VB.Left(SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.GBER].Text, 1);                       //응급 가산
                        cBArg.NIGHT = VB.Left(SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.GBNGT].Text, 1);                      //공휴, 야간 가산
                        cBArg.AN1 = VB.Left(SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.GBSUGBAC].Text, 1);                   //마취 가산
                        cBArg.OP1 = SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.GBSGADD].Text == "True" ? "1" : cBArg.OP1;    //외과 가산
                        cBArg.OP1 = SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.GBSUGBZ].Text == "True" ? "2" : cBArg.OP1;    //흉부외과 가산
                        cBArg.OP2 = VB.Left(SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.GBSUGBAD].Text, 1);                   //화상 가산          
                        cBArg.XRAY1 = VB.Left(SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.GBSUGBAB].Text, 1);                   //판독 가산

                        cBAcct.Bas_Add_Set(ref cBArg, strHang);     //가산항목 세팅
                        strPCodeDtl = cBAcct.PCode_00_Process(clsDB.DbCon, strPCode, cBArg, strHang);

                        //기 가산된코드는 그냥 조회
                        if (strPCode.Length > 7)
                        {
                            nAMT1 = Convert.ToInt64(cBAcct.Read_EDI_SUGA_PCode(strPCode, strCurDate)); //발생금액
                            SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.BCODE].Text = strPCode;
                        }
                        else
                        {
                            nAMT1 = Convert.ToInt64(cBAcct.Read_EDI_SUGA_PCode(strPCodeDtl, strCurDate)); //발생금액
                            SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.BCODE].Text = strPCodeDtl;
                        }

                        nAMT1 *= Convert.ToDouble(SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.QTY].Text);
                        nAMT1 *= Convert.ToInt16(SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.NAL].Text);


                        SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.AMT1].Text = nAMT1.ToString(); //발생금액
                    }
                    #endregion
                }
                else if (e.Column == (int)clsPmpaPb.enmJSimSlip.QTY || e.Column == (int)clsPmpaPb.enmJSimSlip.NAL)
                {
                    #region 수량/날수 수정
                    if (SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.QTY].Text == "" || SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.NAL].Text == "")
                    {
                        SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.AMT1].Text = "0";
                        return;
                    }
                    strCurDate = SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.BDATE].Text == "" ? clsPublic.GstrSysDate : SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.BDATE].Text;
                    strPCodeDtl = SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.BCODE].Text;
                    nAMT1 = Convert.ToInt64(cBAcct.Read_EDI_SUGA_PCode(strPCodeDtl, strCurDate)); //발생금액
                    nAMT1 *= Convert.ToDouble(SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.QTY].Text);
                    nAMT1 *= Convert.ToInt16(SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.NAL].Text);

                    SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.AMT1].Text = nAMT1.ToString(); //발생금액
                    #endregion
                }
                else
                {
                    #region 셀 폰트 표시 (삭제: Red, 일반: Black, 신규: Blue)
                    if (SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmJSimSlip.NEW].Text == "Y")
                    {
                        cSpd.setSpdForeColor(SS1, e.Row, 0, e.Row, SS1_Sheet1.ColumnCount - 1, System.Drawing.Color.Blue);
                    }
                    else
                    {
                        cSpd.setSpdForeColor(SS1, e.Row, 0, e.Row, SS1_Sheet1.ColumnCount - 1, System.Drawing.Color.Black);
                    }
                    #endregion
                }
            }
            else if (o.Name == "SSILL")
            {
                if (SSILL.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmJSimMirILL.chk01].Text == "True")
                {
                    cSpd.setSpdForeColor(SSILL, e.Row, 0, e.Row, SSILL_Sheet1.ColumnCount - 1, System.Drawing.Color.Red);
                }
                else
                {
                    cSpd.setSpdForeColor(SSILL, e.Row, 0, e.Row, SSILL_Sheet1.ColumnCount - 1, System.Drawing.Color.Black);
                }
            }
        }
                
        private void eSpdFocus(object sender, EditorNotifyEventArgs e)
        {
            if (e.Column != (int)clsPmpaPb.enmJSimSlip.QTY && e.Column != (int)clsPmpaPb.enmJSimSlip.NAL)
            {
                ComFunc.MsgBox("");
            }
        }

        private void eBtnitemClick(object sender, EventArgs e)
        {

        }
        
        private void eFocus(object sender, EventArgs e)
        {
            if (sender == this.cboNu)
            {
                cboNu.SelectedIndex = 1;
            }
        }

        private void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtPano && e.KeyChar == (Char)13)
            {
                txtPano.Text = VB.Format(VB.Val(txtPano.Text), "00000000");
                FstrPano = txtPano.Text;
                cIMent.Read_Ipd_Mst_Trans(clsDB.DbCon, FstrPano, FnTrsno);

                if (clsPmpaType.TIT.Ipdno == 0)
                {
                    ComFunc.MsgBox("현재 입원정보가 없습니다. 등록번호를 확인해주세요.");
                    txtPano.Focus();
                    return;
                }
                
                txtPano.Enabled = false;
                
                DisPlay_PatInfo(clsDB.DbCon);   //환자 입원정보 및 자격정보 Display

                dtpFDate.Focus();
                
            }
            if (sender == this.dtpFDate && e.KeyChar == (Char)13) { dtpTDate.Focus(); }
            if (sender == this.dtpTDate && e.KeyChar == (Char)13) { cboNu.Focus(); }
            if (sender == this.cboNu && e.KeyChar == (Char)13) { btnSearch.Focus(); }
        }

        private void eCboChange(object sender, EventArgs e)
        {
            switch (VB.Left(cboNu.Text, 2))
            {
                case "00":  FstrFNu = "01"; FstrTNu = "50"; break;
                case "91":  FstrFNu = "01"; FstrTNu = "20"; break;
                case "92":  FstrFNu = "21"; FstrTNu = "50"; break;
                case "01":  FstrFNu = "01"; FstrTNu = "01"; break;
                case "02":  FstrFNu = "02"; FstrTNu = "03"; break;
                case "03":  FstrFNu = "04"; FstrTNu = "04"; break;
                case "04":  FstrFNu = "05"; FstrTNu = "05"; break;
                case "05":  FstrFNu = "06"; FstrTNu = "06"; break;
                case "06":  FstrFNu = "07"; FstrTNu = "08"; break;
                case "07":  FstrFNu = "09"; FstrTNu = "09"; break;
                case "08":  FstrFNu = "10"; FstrTNu = "12"; break;
                case "09":  FstrFNu = "13"; FstrTNu = "14"; break;
                case "10":  FstrFNu = "15"; FstrTNu = "15"; break;
                case "11":  FstrFNu = "16"; FstrTNu = "20"; break;
                case "12":  FstrFNu = "21"; FstrTNu = "33"; break;
                case "13":  FstrFNu = "34"; FstrTNu = "35"; break;
                case "14":  FstrFNu = "36"; FstrTNu = "38"; break;                
                default:    FstrFNu = "39"; FstrTNu = "50"; break;
            }
        }

        private void Screen_Clear()
        {
            txtPano.Text = "";
            CF.dtpClear(dtpFDate);
            CF.dtpClear(dtpTDate);
            lblSname.Text = "";
            lblAgeSex.Text = "";
            lblBi.Text = "";
            lblGamek.Text = "";
            lblInDate.Text = "";
            lblOutDate.Text = "";
            lblWard.Text = "";
            lblSTS.Text = "";
            lblDeptName.Text = "";
            lblDrName.Text = "";
            lblKTAS.Text = "";
            lblTotal.Text = "";
            lblDrgCode.Text = "";
            lblJiwon.Text = "";
            txtRemark.Text = "";
            txtJsim_Remark.Text = "";
            txtPano.Enabled = true;

            SS1.ActiveSheet.ClearRange(0, 0, SS1_Sheet1.Rows.Count, SS1_Sheet1.ColumnCount, true);
            SS1_Sheet1.Rows.Count = 0;

            SSILL.ActiveSheet.ClearRange(0, 0, SSILL_Sheet1.Rows.Count, SSILL_Sheet1.ColumnCount, true);
            SSILL_Sheet1.Rows.Count = 0;

            cIMent.Read_Ipd_Mst_Clear();
            cIMent.Read_Ipd_Trans_Clear();
        }

        private void DisPlay_PatInfo(PsmhDb pDbCon)
        {
            lblSname.Text = clsPmpaType.TIT.Sname;
            lblAgeSex.Text = clsPmpaType.TIT.Age.ToString() + " / " + clsPmpaType.TIT.Sex;
            lblBi.Text = clsPmpaType.TIT.Bi + "." + CF.Read_Bi_Name(pDbCon, clsPmpaType.TIT.Bi, "2");
            lblGamek.Text = clsPmpaType.TIT.GbGameK + "." + CF.Read_Bcode_Name(pDbCon, "BAS_감액코드명", clsPmpaType.TIT.GbGameK);
            lblInDate.Text = clsPmpaType.TIT.InDate;
            lblOutDate.Text = clsPmpaType.TIT.OutDate;
            dtpFDate.Text = clsPmpaType.TIT.InDate;
            if (clsPmpaType.TIT.OutDate == "")
            {
                dtpTDate.Text = clsPublic.GstrSysDate;
            }
            else
            {
                dtpTDate.Text = clsPmpaType.TIT.OutDate;
            }
            lblWard.Text = clsPmpaType.TIT.WardCode + " / " + clsPmpaType.TIT.RoomCode;
            lblSTS.Text = clsPmpaType.TIT.TGbSts + "." + CF.Read_Bcode_Name(pDbCon, "IPD_입원상태", clsPmpaType.TIT.TGbSts);
            lblDeptName.Text = clsPmpaType.TIT.DeptCode + "." + CF.READ_DEPTNAMEK(pDbCon, clsPmpaType.TIT.DeptCode);
            lblDrName.Text = CF.READ_DrName(pDbCon, clsPmpaType.TIT.DrCode);
            if (clsPmpaType.TIT.KTASLEVL != "" && clsPmpaType.TIT.KTASLEVL != "0")
            {
                lblKTAS.Text = "KTAS " + clsPmpaType.TIT.KTASLEVL + "단계";
            }
            lblGBIPD.Text = clsPmpaType.TIT.GbIpd == "9" ? "지병" : "";
            lblTotal.Text = clsPmpaType.TIT.T_CARE == "Y" ? "대상" : "비대상";
            if (lblTotal.Text == "대상")
            {
                lblTotal.Text += cISentChk.Chk_Total_Care(pDbCon, clsPmpaType.TIT.Ipdno, clsPmpaType.TIT.Pano) == true ? " (동의서 Y)" : "(동의서 N)";
            }
            lblDrgCode.Text = clsPmpaType.TIT.DrgCode;
            lblJiwon.Text = clsPmpaType.TIT.GBJIWON == "" ? "N" : clsPmpaType.TIT.GBJIWON;
            
        }

        private void DisPlay_MirILLs(PsmhDb pDbCon)
        {
            DataTable Dt = null;
            DataTable Dt2 = null;
            string SQL = "";
            string SqlErr = "";

            int i = 0, nRead = 0, nRow = 0;

            SSILL.ActiveSheet.RowCount += 10;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT RowId,IllCode,IllName,RANK,REMARK,GBILL ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MIR_ILLS  ";
                SQL += ComNum.VBLF + "  WHERE Pano = '" + clsPmpaType.TIT.Pano + "' ";
                SQL += ComNum.VBLF + "    AND Bi = '" + clsPmpaType.TIT.Bi + "' ";
                SQL += ComNum.VBLF + "    AND IpdOpd = 'I' ";
                SQL += ComNum.VBLF + "    AND InDate  = TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD')  ";
                SQL += ComNum.VBLF + "    AND (IPDNO IS NULL OR IPDNO = " + clsPmpaType.TIT.Ipdno + ") ";
                SQL += ComNum.VBLF + "    AND TRSNO =" + clsPmpaType.TIT.Trsno + " ";
                SQL += ComNum.VBLF + "  ORDER BY Rank,IllCode ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                if (Dt.Rows.Count == 0)
                {
                    Dt.Dispose();
                    Dt = null;
                    return;
                }
                else
                {
                    nRow = 0;
                    nRead = Dt.Rows.Count;

                    for (i = 0; i < nRead; i++)
                    {
                        if (Dt.Rows[i]["RANK"].ToString().Trim() == "0")
                        {
                            txtRemark.Text = Dt.Rows[i][(int)clsPmpaPb.enmJSimMirILL.ILLNAME].ToString().Trim(); 
                        }
                        else
                        {
                            nRow += 1;
                            if (SSILL_Sheet1.Rows.Count < nRow)
                            {
                                SSILL_Sheet1.Rows.Count = nRow;
                            }
                            SSILL_Sheet1.Cells[nRow - 1, (int)clsPmpaPb.enmJSimMirILL.RANK].Text = Dt.Rows[i]["RANK"].ToString().Trim();
                            SSILL_Sheet1.Cells[nRow - 1, (int)clsPmpaPb.enmJSimMirILL.ILLCODE].Text = Dt.Rows[i]["ILLCODE"].ToString().Trim();
                            SSILL_Sheet1.Cells[nRow - 1, (int)clsPmpaPb.enmJSimMirILL.GBILL].Text = Dt.Rows[i]["GBILL"].ToString().Trim();
                            SSILL_Sheet1.Cells[nRow - 1, (int)clsPmpaPb.enmJSimMirILL.ILLNAME].Text = Dt.Rows[i]["ILLNAME"].ToString().Trim();
                            SSILL_Sheet1.Cells[nRow - 1, (int)clsPmpaPb.enmJSimMirILL.ROWID].Text = Dt.Rows[i]["ROWID"].ToString().Trim();

                            if (VB.Left(clsPmpaType.TIT.Bi, 1) == "1")
                            {
                                SQL = "";
                                SQL += ComNum.VBLF + " SELECT ILLCODE FROM " + ComNum.DB_PMPA + "BAS_ILLS ";
                                SQL += ComNum.VBLF + "  WHERE ILLCODE = '" + Dt.Rows[i][(int)clsPmpaPb.enmJSimMirILL.ILLCODE].ToString().Trim() + "' ";
                                SQL += ComNum.VBLF + "    AND DDATE IS NULL ";
                                SQL += ComNum.VBLF + "    AND IPDETC = 'Y' ";
                                SqlErr = clsDB.GetDataTable(ref Dt2, SQL, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return;
                                }
                                if (Dt2.Rows.Count > 0)
                                {
                                    SSILL_Sheet1.Cells[nRow - 1, (int)clsPmpaPb.enmJSimMirILL.LONGT].Text = "◎";
                                }

                                Dt2.Dispose();
                                Dt2 = null;
                            }
                        }
                    }
                }

                Dt.Dispose();
                Dt = null;

                SSILL.ActiveSheet.RowCount += 10;
            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            }
        }
        
        private void Display_Slip(PsmhDb pDbCon, FpSpread Spd)
        {
            Spd.ActiveSheet.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            spd = SS1;

            //스레드 시작
            thread = new Thread(tProcess);
            thread.Start();

            Cursor.Current = Cursors.Default;
        }

        #region 쓰레드적용
        
        delegate void threadSpdTypeDelegate(FpSpread spd, DataTable dt);

        void tProcess()
        {
            string strSort = string.Empty;

            this.Invoke(new threadProcessDelegate(trunCircular), new object[] { true });
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = false));
            this.spd.BeginInvoke(new System.Action(() => this.spd.Enabled = false));

            if (rdoSort1.Checked)
            {
                strSort = "1";
            }
            else if (rdoSort2.Checked)
            {
                strSort = "2";
            }
            else
            {
                strSort = "3";
            }

            DataTable dt = null;
            dt = cISent.sel_Ipd_New_Slip(clsDB.DbCon, chkMir.Checked, clsPmpaType.TIT.Trsno, dtpFDate.Text, dtpTDate.Text, FstrFNu, FstrTNu, strSort);

            this.Invoke(new threadSpdTypeDelegate(tShowSpread), spd, dt);

            this.Invoke(new threadProcessDelegate(trunCircular), new object[] { false });
            this.spd.BeginInvoke(new System.Action(() => this.spd.Enabled = true));
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = true));

        }

        void tShowSpread(FpSpread spd, DataTable Dt)
        {
            int i = 0, nRead = 0, nRow = 0;
            string strNu = string.Empty;
            long nSubAmt1 = 0, nSubAmt2 = 0;

            //spd.ActiveSheet.RowCount = 0;

            if (Dt == null)
            {
                return;
            }

            nRead = Dt.Rows.Count;

            if (nRead > 0)
            {
                for (i = 0; i < nRead; i++)
                {
                    if (Dt.Rows[i]["NU"].ToString().Trim() != strNu)
                    {
                        if (strNu != "")
                        {
                            nRow += 1;
                            if (spd.ActiveSheet.Rows.Count < nRow)
                            {
                                spd.ActiveSheet.Rows.Count = nRow;
                            }

                            spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.SUNAMEK].Text = CF.Read_Bcode_Name(clsDB.DbCon, "BAS_누적행위명", strNu);
                            spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.AMT1].Text = nSubAmt1.ToString("###,###,##0");
                            spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.AMT2].Text = nSubAmt2.ToString("###,###,##0");
                            spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.SUB].Text = "Y";     //소계항목 구분
                            cSpd.setSpdCellColor(spd, nRow - 1, 0, nRow - 1, spd.ActiveSheet.ColumnCount - 1, Color.FromArgb(202, 232, 209));
                            spd.ActiveSheet.Cells[nRow - 1, 0, nRow - 1, spd.ActiveSheet.ColumnCount - 1].Locked = true;
                        }
                        strNu = Dt.Rows[i]["NU"].ToString().Trim();
                        nSubAmt1 = 0;
                        nSubAmt2 = 0;
                    }

                    nRow += 1;
                    if (spd.ActiveSheet.Rows.Count < nRow)
                    {
                        spd.ActiveSheet.Rows.Count = nRow;
                    }

                    clsSpread.gSdCboItemFindLeft(SS1, nRow - 1, (int)clsPmpaPb.enmJSimSlip.GBSELF, 1, Dt.Rows[i]["GBSELF"].ToString().Trim());
                    clsSpread.gSdCboItemFindLeft(SS1, nRow - 1, (int)clsPmpaPb.enmJSimSlip.GBNGT, 1, Dt.Rows[i]["GBNGT"].ToString().Trim());
                    clsSpread.gSdCboItemFindLeft(SS1, nRow - 1, (int)clsPmpaPb.enmJSimSlip.GBER, 1, Dt.Rows[i]["GBER"].ToString().Trim());

                    spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.BDATE].Text = Dt.Rows[i]["BDATE"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.SUCODE].Text = Dt.Rows[i]["SUCODE"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.SUNEXT].Text = Dt.Rows[i]["SUNEXT"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.SUNAMEK].Text = Dt.Rows[i]["SUNAMEK"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.BUN].Text = Dt.Rows[i]["BUN"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.NU].Text = Dt.Rows[i]["NU"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.GBSGADD].Text = Dt.Rows[i]["GBSGADD"].ToString().Trim();     //1: 외과가산   2: 흉부외과 가산
                    spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.GBSUGBZ].Text = Dt.Rows[i]["GBSUGBZ"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.GBSUGBAB].Text = Dt.Rows[i]["GBSUGBAB"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.GBSUGBAC].Text = Dt.Rows[i]["GBSUGBAC"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.GBSUGBAD].Text = Dt.Rows[i]["GBSUGBAD"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.BCODE].Text = Dt.Rows[i]["BCODE"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.BASEAMT].Text = Dt.Rows[i]["BASEAMT"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.QTY].Text = VB.Val(Dt.Rows[i]["QTY"].ToString().Trim()).ToString("#0.#0");
                    spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.NAL].Text = VB.Val(Dt.Rows[i]["NAL"].ToString().Trim()).ToString("##0");
                    spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.AMT1].Text = Dt.Rows[i]["AMT1"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.AMT2].Text = Dt.Rows[i]["AMT2"].ToString().Trim();

                    spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.GBSGADD].Locked = Dt.Rows[i]["SUGBY"].ToString().Trim() == "0" ? true : false;
                    spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.GBSUGBZ].Locked = Dt.Rows[i]["SUGBZ"].ToString().Trim() == "0" ? true : false;
                    spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.GBSUGBAB].Locked = Dt.Rows[i]["SUGBAB"].ToString().Trim() == "0" ? true : false;
                    spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.GBSUGBAC].Locked = Dt.Rows[i]["SUGBAC"].ToString().Trim() == "0" ? true : false;
                    spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.GBSUGBAD].Locked = Dt.Rows[i]["SUGBAD"].ToString().Trim() == "0" ? true : false;

                    spd.ActiveSheet.Cells[nRow - 1, 1, nRow - 1, spd.ActiveSheet.ColumnCount - 1].Locked = true;

                    nSubAmt1 += Convert.ToInt64(Dt.Rows[i]["AMT1"].ToString().Trim());
                    nSubAmt2 += Convert.ToInt64(Dt.Rows[i]["AMT2"].ToString().Trim());

                }

                if (strNu != "")
                {
                    nRow += 1;
                    if (spd.ActiveSheet.Rows.Count < nRow)
                    {
                        spd.ActiveSheet.Rows.Count = nRow;
                    }
                    spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.SUNAMEK].Text = CF.Read_Bcode_Name(clsDB.DbCon, "BAS_누적행위명", strNu);
                    spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.AMT1].Text = nSubAmt1.ToString("###,###,##0");
                    spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.AMT2].Text = nSubAmt2.ToString("###,###,##0");
                    spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.SUB].Text = "Y";     //소계항목구분
                    cSpd.setSpdCellColor(spd, nRow - 1, 0, nRow - 1, spd.ActiveSheet.ColumnCount - 1, Color.FromArgb(202, 232, 209));
                    spd.ActiveSheet.Cells[nRow - 1, 0, nRow - 1, spd.ActiveSheet.ColumnCount - 1].Locked = true;
                }

                Dt.Dispose();
                Dt = null;
            }
        }

        delegate void threadProcessDelegate(bool b);

        void trunCircular(bool b)
        {
            this.Progress.Visible = b;
            this.Progress.IsRunning = b;
        }

        #endregion

        private void Display_Slip_Old(PsmhDb pDbCon)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";
            string strGbSelf = string.Empty;
            string strNgt = string.Empty;

            int i = 0, nRead = 0, nRow = 0;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(BDate,'yyyy-mm-dd') BDate,b.BCode,                 ";
                SQL += ComNum.VBLF + "        A.Sucode,A.Bun,A.Nu,Qty,BaseAmt,GbSpc,GbNgt,GbGisul,       ";
                SQL += ComNum.VBLF + "        GbSelf,GbChild,DeptCode,DrCode,WardCode,GbSlip,            ";
                SQL += ComNum.VBLF + "        GbHost,a.Sunext,b.SunameK,A.ROOMCODE,                      ";
                SQL += ComNum.VBLF + "        SUM(Nal) Nal,SUM(Amt1) Amt1,SUM(Amt2) Amt2,                ";
                SQL += ComNum.VBLF + "        DECODE(DIV,'',1,DIV) DIV , T.SUGBC, a.GBSUGBS,             ";
                SQL += ComNum.VBLF + "        DECODE(A.GBER,'','0',' ','0',A.GBER) AS GBER,              ";
                SQL += ComNum.VBLF + "        b.SUGBY,b.SUGBZ,b.SUGBAB,b.SUGBAC,b.SUGBAD                 ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a,                      ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b,                           ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUT T                            ";
                SQL += ComNum.VBLF + "  WHERE a.TRSNO  = " + clsPmpaType.TIT.Trsno + "                   ";
                SQL += ComNum.VBLF + "    AND a.Bdate >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')   ";
                SQL += ComNum.VBLF + "    AND a.Bdate <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')   ";
                SQL += ComNum.VBLF + "    AND a.Nu    >= '" + FstrFNu + "'                               ";
                SQL += ComNum.VBLF + "    AND a.Nu    <= '" + FstrTNu + "'                               ";
                SQL += ComNum.VBLF + "    AND a.SUNEXT NOT IN ('BBBBBB')                                 ";
                //청구분 수정여부
                if (chkMir.Checked == false)
                {
                    SQL += ComNum.VBLF + "    AND (a.YYMM IS NULL OR a.YYMM = '    ')                    ";
                }
                SQL += ComNum.VBLF + "    AND (b.WONCODE NOT IN('1118') OR b.WONCODE IS NULL )           "; //간호행위제외
                SQL += ComNum.VBLF + "    AND a.Sunext = b.Sunext(+)                                     ";
                SQL += ComNum.VBLF + "    AND a.Sunext = T.Sunext(+)                                     ";
                SQL += ComNum.VBLF + "  GROUP BY A.BDate,b.BCode,A.Sucode,A.Bun,A.Nu,Qty,BaseAmt,GbSpc,GbNgt,    ";
                SQL += ComNum.VBLF + "           GbGisul,GbSelf,GbChild,DeptCode,DrCode,WardCode,        ";
                SQL += ComNum.VBLF + "           GbSlip,GbHost,a.SuNext,b.SunameK,A.ROOMCODE,            ";
                SQL += ComNum.VBLF + "           DECODE(DIV,'',1,DIV), T.SUGBC ,a.GBSUGBS,               ";
                SQL += ComNum.VBLF + "           DECODE(A.GBER,'','0',' ','0',A.GBER),                   ";
                SQL += ComNum.VBLF + "           b.SUGBY,b.SUGBZ,b.SUGBAB,b.SUGBAC,b.SUGBAD              ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                nRead = Dt.Rows.Count;

                if (nRead == 0)
                {
                    Dt.Dispose();
                    Dt = null;
                    SS1_Sheet1.Rows.Count += 10;
                    return;
                }
                else
                {
                    SS1.ActiveSheet.DataSource = Dt;

                    for (i = 0; i < nRead; i++)
                    {
                        nRow += 1;
                        if (SS1_Sheet1.Rows.Count < nRow)
                        {
                            SS1_Sheet1.Rows.Count = nRow;
                        }

                        strGbSelf = Dt.Rows[i]["GBSELF"].ToString().Trim() == "0" ? "" : Dt.Rows[i]["GBSELF"].ToString().Trim();
                        strNgt = Dt.Rows[i]["GBNGT"].ToString().Trim() == "0" ? "" : Dt.Rows[i]["GBNGT"].ToString().Trim();

                        clsSpread.gSdCboItemFindLeft(SS1, nRow - 1, (int)clsPmpaPb.enmJSimSlip.GBSELF, 1, strGbSelf);
                        clsSpread.gSdCboItemFindLeft(SS1, nRow - 1, (int)clsPmpaPb.enmJSimSlip.GBNGT, 1, strNgt);

                        SS1_Sheet1.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.BDATE].Text = Dt.Rows[i]["BDATE"].ToString().Trim();
                        SS1_Sheet1.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.SUCODE].Text = Dt.Rows[i]["SUCODE"].ToString().Trim();
                        SS1_Sheet1.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.SUNEXT].Text = Dt.Rows[i]["SUNEXT"].ToString().Trim();
                        SS1_Sheet1.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.SUNAMEK].Text = Dt.Rows[i]["SUNAMEK"].ToString().Trim();
                        SS1_Sheet1.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.BUN].Text = Dt.Rows[i]["BUN"].ToString().Trim();
                        SS1_Sheet1.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.NU].Text = Dt.Rows[i]["NU"].ToString().Trim();
                        SS1_Sheet1.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.GBSGADD].Text = Dt.Rows[i]["GBSGADD"].ToString().Trim();
                        SS1_Sheet1.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.GBSUGBZ].Text = Dt.Rows[i]["GBSUGBZ"].ToString().Trim();
                        SS1_Sheet1.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.GBSUGBAB].Text = Dt.Rows[i]["GBSUGBAB"].ToString().Trim();
                        SS1_Sheet1.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.GBSUGBAC].Text = Dt.Rows[i]["GBSUGBAC"].ToString().Trim();
                        SS1_Sheet1.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.GBSUGBAD].Text = Dt.Rows[i]["GBSUGBAD"].ToString().Trim();
                        SS1_Sheet1.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.BCODE].Text = Dt.Rows[i]["BCODE"].ToString().Trim();
                        SS1_Sheet1.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.BASEAMT].Text = Dt.Rows[i]["BASEAMT"].ToString().Trim();
                        SS1_Sheet1.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.AMT1].Text = Dt.Rows[i]["AMT1"].ToString().Trim();
                        SS1_Sheet1.Cells[nRow - 1, (int)clsPmpaPb.enmJSimSlip.AMT2].Text = Dt.Rows[i]["AMT2"].ToString().Trim();
                    }
                }

                Dt.Dispose();
                Dt = null;
            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            }
        }

    }
}
