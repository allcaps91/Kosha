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
    /// <summary>
    /// Description     : 미수 관리 프로그램
    /// Author          : 김민철
    /// Create Date     : 2017-10-19
    /// </summary>
    /// <history>
    /// 기존 OUMSAD30.frm 폼, OUMSAD36.frm 폼 frmPmpaMisu.cs 으로 변경함
    /// </history>
    /// <seealso cref="OUMSAD30.frm(FrmMisu2), OUMSAD36.frm(FrmMisu) => frmPmpaMisu.cs"/>
    public partial class frmPmpaMisu : Form
    {
        clsSpread cSpd          = new clsSpread();
        clsComPmpaSpd cPmpaSpd  = new clsComPmpaSpd();
        ComFunc CF              = new ComFunc();
        clsPmpaFunc cPF         = new clsPmpaFunc();
        clsPmpaMisu cPM         = new clsPmpaMisu();
        Card CARD               = new Card();
        clsPmpaPb cPb           = new clsPmpaPb();

        clsPmpaType.AcctReqData RSD = new clsPmpaType.AcctReqData();
        clsPmpaType.AcctResData RD = new clsPmpaType.AcctResData();

        string FstrPoscoView = string.Empty;    //팝업시 포스코 내역 Display 여부
        string GstrPANO = string.Empty;    //팝업시 포스코 내역 Display 여부
        long FnWrtno = 0;           //포스코 위탁
        long nAmt1 = 0, nAmt2 = 0, nAmt3 = 0;
        
        public frmPmpaMisu()
        {
            InitializeComponent();
            SetEvent();
        }

        public frmPmpaMisu(string strPano, string strDept)
        {
            InitializeComponent();
            SetEvent();
            GstrPANO = strPano;
        }

        
        public frmPmpaMisu(string strMode)
        {
            InitializeComponent();
            SetEvent();
            FstrPoscoView = strMode;
        }

        private void SetEvent()
        {
            this.Load                   += new EventHandler(eFormLoad);
            this.dtpDate.ValueChanged   += new EventHandler(CF.eDtpFormatSet);
            
            this.txtPano.KeyPress       += new KeyPressEventHandler(eKeyPress);
            this.cboGubun.KeyPress      += new KeyPressEventHandler(eKeyPress);
            this.dtpDate.KeyPress       += new KeyPressEventHandler(eKeyPress);
            this.cboBuse.KeyPress       += new KeyPressEventHandler(eKeyPress);
            this.txtAmt.KeyPress        += new KeyPressEventHandler(eKeyPress);
            this.cboDept.KeyPress       += new KeyPressEventHandler(eKeyPress);
            this.cboIO.KeyPress         += new KeyPressEventHandler(eKeyPress);
            this.cboMisuGbn.KeyPress    += new KeyPressEventHandler(eKeyPress);
            this.cboBun.KeyPress        += new KeyPressEventHandler(eKeyPress);
            this.txtRemark.KeyPress     += new KeyPressEventHandler(eKeyPress);
            this.txtSabun.KeyPress      += new KeyPressEventHandler(eKeyPress);

            this.cboGubun.Leave         += new EventHandler(eKeyDown);
            this.cboBuse.Leave          += new EventHandler(eKeyDown);
            this.cboDept.Leave          += new EventHandler(eKeyDown);
            this.cboIO.Leave            += new EventHandler(eKeyDown);
            this.cboMisuGbn.Leave       += new EventHandler(eKeyDown);
            this.cboBun.Leave           += new EventHandler(eKeyDown);

            this.btnExit.Click          += new EventHandler(eBtnClick);
            this.btnSearch.Click        += new EventHandler(eBtnClick);
            this.btnSave.Click          += new EventHandler(eBtnClick);
            this.btnSave2.Click         += new EventHandler(eBtnClick);
            this.btnSave3.Click         += new EventHandler(eBtnClick);
            this.btnCancel.Click        += new EventHandler(eBtnClick);
            this.btnSMS.Click           += new EventHandler(eBtnClick);
            this.btnSet2.Click          += new EventHandler(eBtnClick);
            this.btnSet3.Click          += new EventHandler(eBtnClick);
            this.btnSet4.Click          += new EventHandler(eBtnClick);
            this.btnSet5.Click          += new EventHandler(eBtnClick);
            this.btnSet6.Click          += new EventHandler(eBtnClick);
            this.btnSet7.Click          += new EventHandler(eBtnClick);
            this.btnAmtSet.Click        += new EventHandler(eBtnClick);
            this.lblAmt.Click           += new EventHandler(eBtnClick);

            this.chkCard.CheckedChanged += new EventHandler(eChecked);
            this.chkCash.CheckedChanged += new EventHandler(eChecked);

            this.SS3.CellDoubleClick    += new CellClickEventHandler(eSpdDblClick);
        }

        void eKeyDown(object sender, EventArgs e)
        {
            if (sender == this.cboGubun)
            {
                if (cboGubun.FindString(cboGubun.Text.Trim(), -1) > 0)
                {
                    cboGubun.SelectedIndex = cboGubun.FindString(cboGubun.Text.ToUpper().Trim(), -1);
                }
                else
                {
                    cboGubun.SelectedIndex = 0;
                }
            }
            else if (sender == this.cboBuse)
            {
                if (cboBuse.FindString(cboBuse.Text.Trim(), -1) > 0)
                {
                    cboBuse.SelectedIndex = cboBuse.FindString(cboBuse.Text.ToUpper().Trim(), -1);
                }
                else
                {
                    cboBuse.SelectedIndex = 0;
                }
            }
            else if (sender == this.cboDept)
            {
                if (cboDept.FindString(cboDept.Text.Trim(), -1) > 0)
                {
                    cboDept.SelectedIndex = cboDept.FindString(cboDept.Text.ToUpper().Trim(), -1);
                }
                else
                {
                    cboDept.SelectedIndex = 0;
                }
            }
            else if (sender == this.cboIO)
            {
                if (cboIO.FindString(cboIO.Text.Trim(), -1) > 0)
                {
                    cboIO.SelectedIndex = cboIO.FindString(cboIO.Text.ToUpper().Trim(), -1);
                }
                else
                {
                    cboIO.SelectedIndex = 0;
                }
            }
            else if (sender == this.cboMisuGbn)
            {
                int nInx = 0;
                string strOK = string.Empty;

                if (cboMisuGbn.Text.Trim() == "")
                {
                    return;
                }

                nInx = (int)VB.Val(VB.Left(cboMisuGbn.Text, 2));

                for (int i = 0; i < cboMisuGbn.Items.Count; i++)
                {
                    cboMisuGbn.SelectedIndex = i;
                    if (nInx == VB.Val(VB.Left(cboMisuGbn.SelectedItem.ToString(), 2)))
                    {
                        strOK = "OK";
                        break;
                    }
                }

                if (strOK == "")
                {
                    cboMisuGbn.SelectedIndex = 0;
                }
            }
            else if (sender == this.cboBun)
            {
                if (cboBun.FindString(cboBun.Text.Trim(), -1) > 0)
                {
                    cboBun.SelectedIndex = cboBun.FindString(cboBun.Text.ToUpper().Trim(), -1);
                }
                else
                {
                    cboBun.SelectedIndex = 0;
                }
            }
        }

        void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            string strMsg = "";
            if (e.Row < 0 || e.Column < 0)
            {
                return;
            }

            if (sender == SS3)
            {
                txtAmt.Text = VB.Replace(SS3.ActiveSheet.Cells[e.Row, 1].Text, ",", "").ToString();

             //   cboDept.SelectedIndex = 0;

                if (cboDept.FindString(SS3.ActiveSheet.Cells[e.Row, 4].Text, -1) > 0)
                {
                    cboDept.SelectedIndex = cboDept.FindString(SS3.ActiveSheet.Cells[e.Row, 4].Text, -1);
                }
                else
                {
           //         cboDept.SelectedIndex = 0;
                }
            }

           

            
        }

        void eChecked(object sender, EventArgs e)
        {
            if (sender == this.chkCard)
            {
                CardApprov_Amt(this.chkCard, "CARD");
            }
            else if (sender == this.chkCash)
            {
                CardApprov_Amt(this.chkCash, "CASH");
            }
        }

        void CardApprov_Amt(CheckBox ch, string strJob)
        {
            if (ch.Checked == false) { return; }

            CARD.CardVariable_Clear(ref RSD, ref RD);

            if (txtPano.Text.Trim() == "")
            {
                ComFunc.MsgBox("등록번호를 입력하세요.", "확인");
                ch.Checked = false;
                return;
            }

            if (cboDept.Text.Trim() == "")
            {
                ComFunc.MsgBox("미수내역에 진료과를 꼭 입력하세요.", "확인");
                ch.Checked = false;
                return;
            }

            CARD.gstrCdPtno     = txtPano.Text.Trim();
            CARD.gstrCdSName    = lblSName.Text.Trim();
            CARD.gstrCdDeptCode = VB.Left(cboDept.Text, 2);
            CARD.gstrCdPart     = clsType.User.IdNumber;
            CARD.gstrCdGbIo     = VB.Left(cboIO.Text, 1);
            CARD.gstrCdPCode    = "MIS+";

            CARD.glngCdAmt      = (long)Math.Truncate(VB.Val(txtAmt.Text.Replace(",", "")));

            if (clsPmpaPb.GstrCreditBand == "0")
            {
                frmPmpaEntryCardDaou frm = new frmPmpaEntryCardDaou(CARD.gstrCdPtno, CARD.gstrCdSName, CARD.gstrCdDeptCode, CARD.gstrCdGbIo, CARD.glngCdAmt, strJob, "");
                frm.ShowDialog();
            }
            else
            {
                ComFunc.MsgBox("카드 결제사를 세팅해주세요.", "환경세팅 필요");
            }

            ch.Checked = false;
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtPano && e.KeyChar == (char)13)
            {
                #region txtPano KeyPres
                //txtPano.Text = string.Format("{0:D8}", Convert.ToInt32(txtPano.Text));
                txtPano.Text = VB.Format(VB.Val(txtPano.Text), "00000000");

                if (CF.READ_BARCODE(txtPano.Text.Trim()) == true)
                {
                    txtPano.Text = clsPublic.GstrBarPano;

                    if (cboDept.FindString(clsPublic.GstrBarDept, -1) > 0)
                    {
                        cboDept.SelectedIndex = cboDept.FindString(clsPublic.GstrBarDept, -1);
                    }
                    else
                    {
                        cboDept.SelectedIndex = 0;
                    }

                    txtPano.Text = VB.Format(VB.Val(txtPano.Text), "0#######");
                  //  lblSName.Text = Card.CVar.gstrCdSName;
                }
                else
                {
                    txtPano.Text = VB.Format(VB.Val(txtPano.Text), "0#######");
                 //   lblSName.Text = Card.CVar.gstrCdSName;
                }

                Screen_Display();
                cboGubun.Focus(); 
                #endregion
            }
            else if (sender == cboGubun && e.KeyChar == (char)13) { dtpDate.Focus(); }
            else if (sender == dtpDate && e.KeyChar == (char)13) { cboBuse.Focus(); }
            else if (sender == cboBuse && e.KeyChar == (char)13) { txtAmt.Focus(); }
            else if (sender == txtAmt && e.KeyChar == (char)13) { cboDept.Focus(); }
            else if (sender == cboDept && e.KeyChar == (char)13) { cboIO.Focus(); }
            else if (sender == cboIO && e.KeyChar == (char)13) { cboMisuGbn.Focus(); }
            else if (sender == cboMisuGbn && e.KeyChar == (char)13) { cboBun.Focus(); }
            else if (sender == cboBun && e.KeyChar == (char)13) { txtRemark.Focus(); }
            else if (sender == txtRemark && e.KeyChar == (char)13) { txtSabun.Focus(); }
            else if (sender == txtSabun && e.KeyChar == (char)13) { btnSave.Focus(); }

        }

        private void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            cPmpaSpd.sSpd_enmPmpaMisu(SS1, cPb.sSpdPmpaMisu, cPb.nSpdPmpaMisu, 10, 0);

            #region ComboBox Data Set
            cboGubun.Items.Clear();
            cboGubun.Items.Add("1.미수발생");
            cboGubun.Items.Add("2.미수입금");
            cboGubun.Items.Add("3.미수반송");
            cboGubun.Items.Add("4.미수감액");
            cboGubun.Items.Add("5.미수삭감");
            cboGubun.SelectedIndex = 0;

            cboBuse.Items.Clear();
            cboBuse.Items.Add("1.외래수납");
            cboBuse.Items.Add("2.응급실");
            cboBuse.Items.Add("3.입원수납");
            cboBuse.Items.Add("4.심사계");
            cboBuse.Items.Add("5.원무과");
            cboBuse.SelectedIndex = 0;

            cboBun.Items.Clear();
            cboBun.Items.Add("1.포스코");
            cboBun.Items.Add("2.계산서 발부");
            cboBun.SelectedIndex = 0;

            cboMisuGbn.Items.Clear();
            cboMisuGbn.Items.Add("01.가퇴원");
            cboMisuGbn.Items.Add("02.업무착오");
            cboMisuGbn.Items.Add("03.탈원");
            cboMisuGbn.Items.Add("04.지불각서");
            cboMisuGbn.Items.Add("05.응급실");
            cboMisuGbn.Items.Add("06.외래");
            cboMisuGbn.Items.Add("07.청구심사");
            cboMisuGbn.Items.Add("08.책임보험");
            cboMisuGbn.Items.Add("09.퇴원");
            cboMisuGbn.Items.Add("10.기타미수");
            cboMisuGbn.Items.Add("11.기관청구");
            cboMisuGbn.Items.Add("12.입원정밀");
            cboMisuGbn.Items.Add("13.필수접종국가지원");
            cboMisuGbn.Items.Add("14.회사접종");
            cboMisuGbn.Items.Add("15.금연처방");
            cboMisuGbn.SelectedIndex = 0;

            cboIO.Items.Clear();
            cboIO.Items.Add("I.입원");
            cboIO.Items.Add("O.외래");
            cboIO.SelectedIndex = 0;
            #endregion

            clsVbfunc.SetComboDept(clsDB.DbCon,cboDept, "2", 1);

            Screen_Clear();

            if (FstrPoscoView == "N")
            {
                this.Width = 850;
            }
            txtPano.Text = GstrPANO;

            if (txtPano.Text != "")
            {
                KeyPressEventArgs k = new KeyPressEventArgs((char)13);

                eKeyPress(this.txtPano, k);

            }
            else
            {
                txtPano.Select();
            }
  

        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)             //닫기
            {
                this.Close();
            }
            else if (sender == this.btnSearch)      //조회
            {
                Screen_Display();
            }
            else if (sender == this.btnSave)        //등록
            {
                Insert_Data();
            }
            else if (sender == this.btnSave2)       //포스코항목 저장
            {
                Posco_Save();
            }
            else if (sender == this.btnSave3)       //선택일괄 입금
            {
                Insert_Data_Batch();
            }
            else if (sender == this.btnCancel)      //취소
            {
                Screen_Clear();
            }
            else if (sender == this.btnSMS)         //SMS
            {
                Send_SMS();
            }
            else if (sender == this.btnSet2)        //필수예방 미수 세팅
            {
                Set_Vacc();
            }
            else if (sender == this.btnSet3)        //회사접종 미수 세팅
            {
                Set_LtdVacc();
            }
            else if (sender == this.btnSet4)        //포스코위탁 미수 세팅
            {
                Set_Posco();
            }
            else if (sender == this.btnSet5)        //금연처방 미수 세팅
            {
                Set_Smoking();
            }
            else if (sender == this.btnSet6)        //가다실 접종 미수 세팅
            {
                Set_GadaSil();
            }
            else if (sender == this.btnSet7)        //코로나 필수예방 미수 세팅
            {
                Set_Vacc_Covid();
            }
            else if (sender == this.btnAmtSet)        //금액 set
            {
                Read_OpdSlip_Y96_Amt(clsDB.DbCon,  txtPano.Text);
            }
            else if (sender == this.lblAmt)
            {
                if (VB.Val(lblAmt.Text) == 0)       //포스코 금액
                {
                    return;
                }
                else
                {
                    txtAmt.Text = lblAmt.Text;
                }
            }
        }

        private void Read_OpdSlip_Y96_Amt(PsmhDb pDbCon, string ArgPtno)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";
            int i = 0;
            string strMsg = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT nvl(SUM(a.AMT1+a.AMT2), 0) MISUAMT, COUNT(a.PANO) CNT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP a ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND a.ACTDATE = TRUNC(SYSDATE) ";
            SQL += ComNum.VBLF + "    AND a.PANO    = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND a.SUNEXT  = 'Y96' ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Dt.Dispose();
                Dt = null;
                return;
            }

            if (Dt.Rows.Count > 0)
            {
                if (Convert.ToInt64(Dt.Rows[i]["MISUAMT"].ToString()) > 0)
                {
                    strMsg = "당일 외래수납 미수계정(Y96) " + Convert.ToInt64(Dt.Rows[i]["MISUAMT"].ToString()) + "건 발생" + '\r' ;
                    strMsg += "금액 : " + Convert.ToInt64(Dt.Rows[i]["MISUAMT"].ToString()) + "원" + '\r' + '\r';
                    strMsg += "개인미수등록을 확인후 미등록시 등록요망" + '\r';
                    strMsg += "해당 금액을 기본설정으로 하시겠습니까?";
                    
                    if (DialogResult.Yes == ComFunc.MsgBoxQ(strMsg, "알림"))
                        txtAmt.Text = Dt.Rows[i]["MISUAMT"].ToString();
                    else
                        txtAmt.Text = "";

                    txtAmt.Select();
                }
            }

            Dt.Dispose();
            Dt = null;
        }

        private void Posco_Save()
        {
             

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                clsDB.setBeginTran(clsDB.DbCon);

                if (FnWrtno > 0)
                {
                    if (Posco_Exam_Detail_Set(clsDB.DbCon, FnWrtno, txtPano.Text.Trim(), dtpDate.Text) == false)
                    if (Posco_Exam_Detail_Set(clsDB.DbCon, FnWrtno, txtPano.Text.Trim(), dtpDate.Text) == false)
                    {
                            clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void Screen_Clear()
        {
            int i = 0;

            txtPano.Enabled = true;
            cboGubun.Enabled = true;
            txtAmt.Enabled = true;
            cboDept.Enabled = true;
            cboIO.Enabled = true;
            dtpDate.Enabled = true;

            cboGubun.Text = "";
            cboBuse.Text = "";
            cboBun.Text = "";
            cboDept.Text = "";
            cboIO.Text = "";
            cboMisuGbn.Text = "";
            CF.dtpClear(dtpDate);
            txtAmt.Text = "";
            txtRemark.Text = "";
            txtPano.Text = "";
            txtSabun.Text = clsType.User.IdNumber;
            lblSName.Text = "";
            lblJobName.Text = clsType.User.JobName;
            lblJobBuse.Text = "";
            lblAmt.Text = "";
            lbl_Pos_Sts.Text = "";

            chkCash.Checked = false;
            chkCard.Checked = false;
            chkAll.Checked = false;
            chkSel.Checked = false;

            SS1.ActiveSheet.ClearRange(0, 0, SS1_Sheet1.Rows.Count, SS1_Sheet1.ColumnCount, false);
            SS1_Sheet1.Rows.Count = 0;

            SS3.ActiveSheet.ClearRange(0, 0, SS3_Sheet1.Rows.Count, SS3_Sheet1.ColumnCount, false);
            SS3_Sheet1.Rows.Count = 0;

            SS_Posco.ActiveSheet.ClearRange(0, 0, SS_Posco_Sheet1.Rows.Count, SS_Posco_Sheet1.ColumnCount, false);
            SS_Posco_Sheet1.Rows.Count = 0;

            for (i = 0; i < 12; i++)
            {
                SS2_Sheet1.Cells[i, 1].Text = "";
            }

            SSSunap_Sheet1.RowCount = 0;

            sTab.SelectedTab = sTab1;
        }

        private void Screen_Display()
        {

            if (txtPano.Text.Trim() == "")
            {
                return;
            }
            
            if (clsPmpaPb.GstrLtdVac == "OK" || clsPmpaPb.GstrPapillomaVacc == "OK")
                SS3_Display_2();
            else
                Read_OpdSlip_Y96(txtPano.Text);

            Master_Id_Display();

            dtpDate.Text = clsPmpaPb.GstrActDate;
            cboBuse.Text = "1.외래수납";
            cboGubun.Text = "1.미수발생";

            //포스코 청구내역 2013-05-28
            READ_Posco_Exam("1", txtPano.Text, dtpDate.Text, 0, chkSel.Checked);

            btnSave.Enabled = true;
            btnCancel.Enabled = true;

            btnSave2.Enabled = false;

            lblAmt.Text = "";

            //미수작업종류
            if (VB.SinglePiece(clsPmpaPb.GstrMisuMsg, "^^", 3) != "")
                cboGubun.SelectedIndex = (int)VB.Val(VB.SinglePiece(clsPmpaPb.GstrMisuMsg, "^^", 3)) - 1;

            ReadOpdSunap(txtPano.Text.Trim());

        }

        private void SS3_Display_2()
        {
            DataTable Dt = null;
            DataTable Dt2 = null;
            string SQL = "";
            string SqlErr = "";

            int i = 0, nRead = 0, nRow = 0;

            try
            {
                cSpd.Spread_All_Clear(SS3);

                SQL = "";
                SQL += ComNum.VBLF + " SELECT SuCode,Amt1,SEQNO,PART,DeptCode ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND PANO ='" + txtPano.Text + "' ";
                SQL += ComNum.VBLF + "    AND BDATE =TRUNC(SYSDATE) ";
                SQL += ComNum.VBLF + "    AND SUCODE ='Y96' ";
                SQL += ComNum.VBLF + "  ORDER BY ENTDATE  DESC ";
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
                        nRow += 1;
                        if (SS3_Sheet1.Rows.Count < nRow)
                        {
                            SS3_Sheet1.Rows.Count = nRow;
                        }

                        SS3_Sheet1.Cells[nRow - 1, 0].Text = Dt.Rows[i]["SuCode"].ToString().Trim();
                        SS3_Sheet1.Cells[nRow - 1, 1].Text = VB.Val(Dt.Rows[i]["Amt1"].ToString().Trim()).ToString("###,###,###");
                        SS3_Sheet1.Cells[nRow - 1, 2].Text = Dt.Rows[i]["Part"].ToString().Trim();
                        SS3_Sheet1.Cells[nRow - 1, 3].Text = Dt.Rows[i]["SEQNO"].ToString().Trim();
                        SS3_Sheet1.Cells[nRow - 1, 4].Text = Dt.Rows[i]["DeptCode"].ToString().Trim();

                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT ROWID ";
                        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP ";
                        SQL += ComNum.VBLF + "  WHERE PANO ='" + txtPano.Text + "' ";
                        SQL += ComNum.VBLF + "    AND BDATE = TRUNC(SYSDATE) ";
                        SQL += ComNum.VBLF + "    AND Gubun1 ='1' ";
                        SQL += ComNum.VBLF + "    AND PoBun ='2' ";
                        SQL += ComNum.VBLF + "    AND Amt = " + Convert.ToInt64(Dt.Rows[i]["Amt1"].ToString()).ToString() + " ";
                        SqlErr = clsDB.GetDataTable(ref Dt2, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (Dt2.Rows.Count > 0)
                        {
                            cSpd.setSpdCellColor(SS3, nRow - 1, 0, nRow - 1, SS3_Sheet1.ColumnCount - 1, Color.FromArgb(255, 196, 196));
                        }
                        else
                        {
                            cSpd.setSpdCellColor(SS3, nRow - 1, 0, nRow - 1, SS3_Sheet1.ColumnCount - 1, Color.FromArgb(255, 255, 255));
                        }
                        Dt2.Dispose();
                        Dt2 = null;
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void Read_OpdSlip_Y96(string ArgPano)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            int i = 0;
            long nTotAmt = 0;
            string strMsg = string.Empty;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT a.DeptCode, a.Seqno, SUM(a.AMT1+a.AMT2) MISUAMT, COUNT(a.PANO) CNT ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP a ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND a.PANO ='" + ArgPano + "' ";
                SQL += ComNum.VBLF + "    AND a.ACTDATE =TRUNC(SYSDATE) ";
                SQL += ComNum.VBLF + "    AND a.SUNEXT ='Y96' ";

                SQL += ComNum.VBLF + "  GROUP BY a.DeptCode,a.Seqno";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        nTotAmt += Convert.ToInt64(Dt.Rows[i]["MISUAMT"].ToString());
                        strMsg += Dt.Rows[i]["DeptCode"].ToString().Trim();
                        strMsg += " 영수번호: " + Dt.Rows[i]["Seqno"].ToString().Trim();
                        strMsg += " 미수금액: " + VB.Val(Dt.Rows[i]["MISUAMT"].ToString().Trim()).ToString("###,###,###");
                        strMsg += ComNum.VBLF;
                    }

                    strMsg = "당일 외래수납미수 발생건수:" + Dt.Rows.Count + "건  총미수액:" + nTotAmt + ComNum.VBLF + ComNum.VBLF + strMsg + ComNum.VBLF + ComNum.VBLF;
                    strMsg += "개인미수등록을 확인후 미등록시 등록하십시오!!";

                    ComFunc.MsgBox(strMsg, "확인");
                }

                Dt.Dispose();
                Dt = null;

                SS3_Display_2();

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

        private void Master_Id_Display()
        {
            DataTable Dt = null;
            DataTable Dt2 = null;
            string SQL = "";
            string SqlErr = "";

            int i = 0, j = 0, nRead = 0, nRow = 0, nCNT = 0;
            long nHisAmt = 0;
            long nIDNO = 0;
            string strName = string.Empty;
            string strRemark = string.Empty;
            string strMisuDtl = string.Empty;
            string strDate = string.Empty;
            
            try
            {
                #region Bas_Patient 정보읽기
                Dt = cPF.Get_BasPatient(clsDB.DbCon, txtPano.Text);
                if (Dt.Rows.Count == 0)
                {
                    ComFunc.MsgBox("등록번호가 오류입니다." + ComNum.VBLF + "다시 입력하세요.", "확인");
                    Dt.Dispose();
                    Dt = null;
                    return;
                }
                else
                {
                    lblSName.Text = Dt.Rows[0]["Sname"].ToString().Trim();
                    strName = Dt.Rows[0]["Sname"].ToString().Trim();
                    SS2_Sheet1.Cells[0, 1].Text = strName;
                    SS2_Sheet1.Cells[1, 1].Text = CF.Read_Bcode_Name(clsDB.DbCon, "BAS_환자종류", Dt.Rows[0]["Bi"].ToString().Trim()); 
                    SS2_Sheet1.Cells[2, 1].Text = Dt.Rows[0]["JUMIN1"].ToString().Trim() + "-" + clsAES.DeAES(Dt.Rows[0]["JUMIN3"].ToString().Trim());
                    SS2_Sheet1.Cells[3, 1].Text = Dt.Rows[0]["Gwange"].ToString().Trim();
                    SS2_Sheet1.Cells[4, 1].Text = Dt.Rows[0]["Sex"].ToString().Trim();
                    SS2_Sheet1.Cells[5, 1].Text = Dt.Rows[0]["Pname"].ToString().Trim();
                    SS2_Sheet1.Cells[6, 1].Text = Dt.Rows[0]["Kiho"].ToString().Trim();
                    SS2_Sheet1.Cells[7, 1].Text = Dt.Rows[0]["Tel"].ToString().Trim();
                    strDate = Dt.Rows[0]["LastDate"].ToString().Trim();
                    if (strDate != "")
                    {
                        SS2_Sheet1.Cells[8, 1].Text = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(strDate));
                    }
                    else
                    {
                        SS2_Sheet1.Cells[8, 1].Text = strDate;
                    }
                    
                }

                Dt.Dispose();
                Dt = null;
                #endregion

                #region //---------------------( 미수발생 마스타 SELECT )---------------------*
                SQL = "";
                SQL += ComNum.VBLF + " SELECT Pano,MAmt,IAmt,JAmt FROM " + ComNum.DB_PMPA + "MISU_GAINMST  ";
                SQL += ComNum.VBLF + "  WHERE Pano = '" + txtPano.Text + "' ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (Dt.Rows.Count > 0)
                {
                    nAmt1 = Convert.ToInt64(VB.Val(Dt.Rows[0]["MAmt"].ToString()));
                    nAmt2 = Convert.ToInt64(VB.Val(Dt.Rows[0]["IAmt"].ToString()));
                    nAmt3 = nAmt1 - nAmt2;
                }
                else
                {
                    nAmt1 = 0; nAmt2 = 0; nAmt3 = 0;
                }

                SS2_Sheet1.Cells[9, 1].Text = VB.Format(nAmt1, "###,###,###");
                SS2_Sheet1.Cells[10, 1].Text = VB.Format(nAmt2, "###,###,###");
                SS2_Sheet1.Cells[11, 1].Text = VB.Format(nAmt3, "###,###,###");
                Dt.Dispose();
                Dt = null;

                if (chkAll.Checked == false)
                {
                    if (nAmt3 == 0)
                        return;
                }
                #endregion

                #region //---------------------( 미수내역 History 조회 )-------------------------+

                cSpd.Spread_All_Clear(SS1);

                SQL = "";
                SQL += ComNum.VBLF + " SELECT To_Char(Bdate,'YYYY-MM-DD') Bdate, Gubun1, Gubun2, Amt,Flag, ";
                SQL += ComNum.VBLF + "        substr(Remark,1,500) remark,Idno,MisuDtl, CardSeqNo, SABUN,  ";
                SQL += ComNum.VBLF + "        POBUN, ENTTIME,WRTNO                                         ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP                          ";
                SQL += ComNum.VBLF + "  WHERE Pano = '" + txtPano.Text + "'                                ";
                SQL += ComNum.VBLF + "  ORDER BY ENTTIME DESC, Bdate,Gubun1                                ";
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
                        nRow += 1;
                        if (SS1_Sheet1.Rows.Count < nRow)
                        {
                            SS1_Sheet1.Rows.Count = nRow;
                        }

                        SS1_Sheet1.Cells[nRow - 1, 1].Text = Dt.Rows[i]["Bdate"].ToString().Trim();

                        nHisAmt = Convert.ToInt64(Dt.Rows[i]["Amt"].ToString());

                        switch (Dt.Rows[i]["Gubun1"].ToString().Trim())
                        {
                            case "1":
                                SS1_Sheet1.Cells[nRow - 1, 2].Text = VB.Format(nHisAmt, "###,###,###");
                                if (Dt.Rows[i]["FLAG"].ToString().Trim() == "*")
                                {
                                    cSpd.setColStyle(SS1, nRow - 1, 0, clsSpread.enmSpdType.Text);
                                    //SS1_Sheet1.Cells[nRow - 1, 0].Text = "False";
                                }
                                break;
                            case "2":
                            case "3":
                            case "4":
                            case "5":
                                SS1_Sheet1.Cells[nRow - 1, 3].Text = VB.Format(nHisAmt, "###,###,###");
                                cSpd.setColStyle(SS1, nRow - 1, 0, clsSpread.enmSpdType.Text);
                                //SS1_Sheet1.Cells[nRow - 1, 0].Text = "False";
                                break;
                            default:
                                break;
                        }

                        switch (Dt.Rows[i]["Gubun2"].ToString().Trim())
                        {
                            case "1": SS1_Sheet1.Cells[nRow - 1, 4].Text = "1.외래"; break;
                            case "2": SS1_Sheet1.Cells[nRow - 1, 4].Text = "2.응급"; break;
                            case "3": SS1_Sheet1.Cells[nRow - 1, 4].Text = "3.입원"; break;
                            case "4": SS1_Sheet1.Cells[nRow - 1, 4].Text = "4.심사"; break;
                            case "5": SS1_Sheet1.Cells[nRow - 1, 4].Text = "5.원무"; break;
                            default: break;
                        }

                        SS1_Sheet1.Cells[nRow - 1, 5].Text = Dt.Rows[i]["Remark"].ToString().Trim();

                        nCNT = 0;
                        strRemark = Dt.Rows[i]["Remark"].ToString().Trim();

                        for (j = 0; j < strRemark.Length; j++)
                        {
                            if (VB.Mid(strRemark, j, 1) == VB.Chr(13).ToString())
                            {
                                nCNT += 1;
                            }
                        }

                        if (nCNT == 0)
                            nCNT = 1;

                        cSpd.SetfpsRowHeight(SS1, SS1_Sheet1.Cells[nRow - 1, 1].Row.Height * nCNT);

                        nIDNO = Convert.ToInt64(Dt.Rows[i]["IDNO"].ToString());

                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT Name FROM " + ComNum.DB_PMPA + "BAS_PASS  ";
                        SQL += ComNum.VBLF + " WHERE IDnumber = " + nIDNO + " ";
                        SqlErr = clsDB.GetDataTable(ref Dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (Dt2.Rows.Count > 0)
                        {
                            SS1_Sheet1.Cells[nRow - 1, 6].Text = Dt2.Rows[0]["Name"].ToString().Trim();
                        }
                        Dt2.Dispose();
                        Dt2 = null;

                        strMisuDtl = Dt.Rows[i]["MisuDtl"].ToString().Trim();

                        SS1_Sheet1.Cells[nRow - 1, 7].Text = VB.Left(strMisuDtl, 1);
                        SS1_Sheet1.Cells[nRow - 1, 8].Text = VB.Mid(strMisuDtl, 2, 2);
                        SS1_Sheet1.Cells[nRow - 1, 9].Text = VB.Mid(strMisuDtl, 4, 2) + "." + cPM.READ_MisuGye(VB.Mid(strMisuDtl, 4, 2));
                        SS1_Sheet1.Cells[nRow - 1, 10].Text = VB.Val(VB.Mid(strMisuDtl, 6, 9)).ToString("###,###,###");
                        SS1_Sheet1.Cells[nRow - 1, 11].Text = VB.Mid(strMisuDtl, 15, 8);
                        SS1_Sheet1.Cells[nRow - 1, 12].Text = VB.Right(strMisuDtl, 8);
                        SS1_Sheet1.Cells[nRow - 1, 13].Text = Dt.Rows[i]["CardSeqno"].ToString().Trim();

                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT SABUN, KORNAME FROM " + ComNum.DB_ERP + "INSA_MST ";
                        SQL += ComNum.VBLF + " WHERE SABUN  = '" + VB.Format(VB.Val(Dt.Rows[i]["SABUN"].ToString().Trim()), "####0") + "' ";
                        SqlErr = clsDB.GetDataTable(ref Dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (Dt2.Rows.Count > 0)
                        {
                            SS1_Sheet1.Cells[nRow - 1, 14].Text = Dt2.Rows[0]["KORNAME"].ToString().Trim() + "(" + Dt2.Rows[0]["SABUN"].ToString().Trim() + ")";
                        }
                        Dt2.Dispose();
                        Dt2 = null;

                        if (Dt.Rows[i]["POBUN"].ToString().Trim() == "1")
                        {
                            SS1_Sheet1.Cells[nRow - 1, 15].Text = "1.포스코";
                        }
                        else if (Dt.Rows[i]["POBUN"].ToString().Trim() == "2")
                        {
                            SS1_Sheet1.Cells[nRow - 1, 15].Text = "2.계산서 발부";
                        }
                        else
                        {
                            SS1_Sheet1.Cells[nRow - 1, 15].Text = "";
                        }
                        SS1_Sheet1.Cells[nRow - 1, 16].Text = Dt.Rows[i]["WRTNO"].ToString().Trim();
                    }
                }

                Dt.Dispose();
                Dt = null;

                #endregion
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

        private void READ_Posco_Exam(string ArgPart, string ArgPano, string ArgBDate, long ArgWRTNO, bool ArgSel)
        {
            DataTable Dt = null;
            DataTable Dt2 = null;
            string SQL = "";
            string SqlErr = "";

            int i = 0, nRead = 0, nRow = 0;

            try
            {
                cSpd.Spread_All_Clear(SS_Posco);

                SQL = "";
                SQL += ComNum.VBLF + " SELECT Sort,Gubun,Code";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ETC_BCODE ";
                SQL += ComNum.VBLF + "  WHERE (DELDATE IS NULL OR DELDATE ='') ";
                SQL += ComNum.VBLF + "    AND GUBUN <> '00000' ";  //타이틀 제외
                if (ArgPart != "")
                {
                    switch (ArgPart)
                    {
                        case "1":
                            SQL += ComNum.VBLF + "  AND Part IN ('1','2') "; //급여,비급여
                            break;
                        case "2":
                            SQL += ComNum.VBLF + "  AND Part IN ('3') ";   //공단
                            break;
                         
                        default:
                            break;
                    }
                }
                SQL += ComNum.VBLF + "  GROUP BY Sort,Gubun,Code ";
                SQL += ComNum.VBLF + "  ORDER BY SORT,GUBUN,CODE ";
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
                        nRow += 1;
                        if (SS_Posco_Sheet1.Rows.Count < nRow)
                        {
                            SS_Posco_Sheet1.Rows.Count = nRow;
                        }

                        SS_Posco_Sheet1.Cells[nRow - 1, 0].Text = "False";
                        SS_Posco_Sheet1.Cells[nRow - 1, 1].Text = READ_Posco_Exam_Name("00", clsPublic.GstrSysDate, "", Dt.Rows[i]["Gubun"].ToString().Trim());
                        SS_Posco_Sheet1.Cells[nRow - 1, 2].Text = READ_Posco_Exam_Name("01", clsPublic.GstrSysDate, Dt.Rows[i]["Gubun"].ToString().Trim(), Dt.Rows[i]["Code"].ToString().Trim());
                        SS_Posco_Sheet1.Cells[nRow - 1, 3].Text = READ_Posco_Exam_Name("02", clsPublic.GstrSysDate, Dt.Rows[i]["Gubun"].ToString().Trim(), Dt.Rows[i]["Code"].ToString().Trim());
                        SS_Posco_Sheet1.Cells[nRow - 1, 4].Text = Dt.Rows[i]["Gubun"].ToString().Trim();
                        SS_Posco_Sheet1.Cells[nRow - 1, 5].Text = Dt.Rows[i]["Code"].ToString().Trim();

                        SS_Posco_Sheet1.SetRowVisible(nRow - 1, true);

                        if (ArgWRTNO > 0)
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " SELECT ROWID FROM " + ComNum.DB_PMPA + "ETC_BCODE_DETAIL ";
                            SQL += ComNum.VBLF + "  WHERE GUBUN ='" + Dt.Rows[i]["Gubun"].ToString().Trim() + "' ";
                            SQL += ComNum.VBLF + "    AND CODE ='" + Dt.Rows[i]["Code"].ToString().Trim() + "' ";
                            SQL += ComNum.VBLF + "    AND BDATE =TO_DATE('" + ArgBDate + "','YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "    AND (DELDATE IS NULL OR DELDATE ='') ";
                            SQL += ComNum.VBLF + "    AND WRTNO =" + ArgWRTNO + "   ";
                            SqlErr = clsDB.GetDataTable(ref Dt2, SQL, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }
                            if (Dt2.Rows.Count > 0)
                            {
                                SS_Posco_Sheet1.Cells[nRow - 1, 0].Text = "True";
                                SS_Posco_Sheet1.Cells[nRow - 1, 7].Text = Dt2.Rows[0]["ROWID"].ToString().Trim();
                            }
                            else
                            {
                                if (ArgSel)
                                {
                                    SS_Posco_Sheet1.SetRowVisible(nRow - 1, false);
                                }
                            }
                            Dt2.Dispose();
                            Dt2 = null;
                        }
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private string READ_Posco_Exam_Name(string argGubun, string ArgDate, string ArgCode1, string ArgCode2)
        {
            string rtnVal = string.Empty;

            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            int i = 0;

            try
            {
                SQL = "";
                //타이틀
                if (argGubun == "00")
                {
                    SQL += ComNum.VBLF + " SELECT NAME FROM " + ComNum.DB_PMPA + "ETC_BCODE ";
                    SQL += ComNum.VBLF + "  WHERE GUBUN ='00000' ";
                    SQL += ComNum.VBLF + "    AND CODE ='" + ArgCode2 + "' ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                    if (Dt.Rows.Count > 0)
                    {
                        rtnVal = Dt.Rows[0]["Name"].ToString().Trim();
                    }
                }
                else if (argGubun == "01")
                {
                    //명칭
                    SQL += ComNum.VBLF + " SELECT NAME FROM " + ComNum.DB_PMPA + "ETC_BCODE ";
                    SQL += ComNum.VBLF + "  WHERE GUBUN = '" + ArgCode1 + "' ";
                    SQL += ComNum.VBLF + "    AND CODE = '" + ArgCode2 + "' ";
                    SQL += ComNum.VBLF + "    AND JDate <= TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND (DELDATE IS NULL OR DELDATE ='')  ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                    if (Dt.Rows.Count > 0)
                    {
                        rtnVal = Dt.Rows[0]["Name"].ToString().Trim();
                    }
                }
                else if (argGubun == "02")
                {
                    //수가
                    SQL += ComNum.VBLF + " SELECT Amt FROM " + ComNum.DB_PMPA + "ETC_BCODE ";
                    SQL += ComNum.VBLF + "  WHERE GUBUN ='" + ArgCode1 + "' ";
                    SQL += ComNum.VBLF + "    AND CODE ='" + ArgCode2 + "' ";
                    SQL += ComNum.VBLF + "    AND JDate <=TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND (DELDATE IS NULL OR DELDATE ='')  ";
                    SQL += ComNum.VBLF + "  ORDER BY JDATE DESC ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                    if (Dt.Rows.Count > 0)
                    {
                        rtnVal = VB.Val(Dt.Rows[0]["AMT"].ToString().Trim()).ToString("###,###,###");
                    }
                }
                else
                {
                    //rowid
                    SQL += ComNum.VBLF + " SELECT ROWID FROM " + ComNum.DB_PMPA + "ETC_BCODE ";
                    SQL += ComNum.VBLF + "  WHERE GUBUN ='" + ArgCode1 + "' ";
                    SQL += ComNum.VBLF + "    AND CODE ='" + ArgCode2 + "' ";
                    SQL += ComNum.VBLF + "    AND JDate <=TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND (DELDATE IS NULL OR DELDATE ='')  ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                    if (Dt.Rows.Count > 0)
                    {
                        rtnVal = Dt.Rows[0]["ROWID"].ToString().Trim();
                    }
                }

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return "";
                }
                Dt.Dispose();
                Dt = null;

                return rtnVal;
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
                return "";
            }

        }

        private bool Posco_Exam_Detail_Set(PsmhDb pDbCon, long ArgWRTNO, string ArgPano, string ArgBDate)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            int i = 0;
            string strChk = string.Empty;
            string strCode1 = string.Empty;
            string strCode2 = string.Empty;
            //string strChange = string.Empty;
            string strROWID = string.Empty;

            bool rtnVal = false;

            try
            {
                if (ArgWRTNO > 0)
                {
                    for (i = 0; i < SS1_Sheet1.Rows.Count; i++)
                    {
                        strChk = SS1_Sheet1.Cells[i, 0].Text;
                        strCode1 = SS1_Sheet1.Cells[i, 4].Text;
                        strCode2 = SS1_Sheet1.Cells[i, 5].Text;
                        //strChange = SS1_Sheet1.Cells[i, 6].Text;
                        strROWID = SS1_Sheet1.Cells[i, 7].Text;

                        if (strChk == "True" && strROWID == "")
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "ETC_BCODE_DETAIL ";
                            SQL += ComNum.VBLF + "        (PANO,WRTNO,GUBUN,CODE,BDATE,CNT,ENTSABUN,ENTDATE ) ";
                            SQL += ComNum.VBLF + " VALUES (";
                            SQL += ComNum.VBLF + "        '" + ArgPano + "', " + ArgWRTNO + ",'" + strCode1 + "','" + strCode2 + "',  ";
                            SQL += ComNum.VBLF + "        TO_DATE('" + ArgBDate + "','YYYY-MM-DD'), 1 ," + clsType.User.IdNumber + ",SYSDATE ) ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                        }
                        else if (strChk == "False" && strROWID != "")
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "ETC_BCODE_DETAIL ";
                            SQL += ComNum.VBLF + "    SET DELDATE =SYSDATE, ";
                            SQL += ComNum.VBLF + "        ENTSABUN =" + clsType.User.IdNumber + ", ";
                            SQL += ComNum.VBLF + "        ENTDATE =SYSDATE ";
                            SQL += ComNum.VBLF + "  WHERE ROWID ='" + strROWID + "' ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                        }
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return rtnVal;
                        }
                    }
                }
                //Screen_Display();

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private void SS1_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            string strBDate = string.Empty;
            string strGubun = string.Empty;
            string strMsg = string.Empty;

            if (e.Row < 0 || e.Column < 0)
            {
                return;
            }
            strBDate = SS1_Sheet1.Cells[e.Row, 1].Text;
            strGubun = SS1_Sheet1.Cells[e.Row, 15].Text;
            FnWrtno = Convert.ToInt64(SS1_Sheet1.Cells[e.Row, 16].Text);

            //포스코 청구 상세내역
            if (strGubun == "1.포스코" && e.Row > 0 && FnWrtno > 0)
            {
                READ_Posco_Exam("1", txtPano.Text, strBDate, FnWrtno, chkSel.Checked);
                lbl_Pos_Sts.Text = txtPano.Text + "  " + strBDate + "  고유번호: " + FnWrtno.ToString();
                btnSave2.Enabled = true;
            }

           
            if ( e.Column == 5)
            {
                ComFunc.MsgBox( SS1_Sheet1.Cells[e.Row, 5].Text, "확인");
             
            }

            if (sender == SS1)
            {
                if (lbl_SelCnt.Text == "0" && SS1_Sheet1.Cells[e.Row, 2].Text != "" )
                {
                    strMsg = "해당 미수금액을 입금처리하시겠습니까?" + '\r';

                    if (DialogResult.Yes == ComFunc.MsgBoxQ(strMsg, "알림"))
                    {

                        txtAmt.Text = VB.Replace(SS1_Sheet1.Cells[e.Row, 2].Text, ",", "").ToString();
                        cboGubun.SelectedIndex = 1;  //미수입금
                        cboBuse.Text = SS1_Sheet1.Cells[e.Row, 4].Text;
                        cboDept.SelectedIndex = cboDept.FindString(SS1_Sheet1.Cells[e.Row, 8].Text, -1);
                        cboIO.Text = SS1_Sheet1.Cells[e.Row, 7].Text;
                        cboMisuGbn.Text = SS1_Sheet1.Cells[e.Row, 9].Text;
                        //    cboBun
                        //cboDept.SelectedIndex = cboDept.FindString(SS1.ActiveSheet.Cells[e.Row, 4].Text, -1);
                        //cboDept.SelectedIndex = cboDept.FindString(SS1.ActiveSheet.Cells[e.Row, 4].Text, -1);
                    }


                    else
                    {
                        return;
                    }

                }
                else
                {
                    return;
                }

            }
          


        }

        private void SS1_ButtonClicked(object sender, EditorNotifyEventArgs e)
        {
            int i = 0;
            int nCNT = 0;
            double nSum = 0;

            if (e.Column != 0)
            {
                return;
            }

            btnSave.Enabled = false;
            btnSave3.Enabled = false;
            lbl_SelCnt.Text = nCNT.ToString();
            lbl_SelAmt.Text = nSum.ToString();

            for (i = 0; i < SS1_Sheet1.Rows.Count; i++)
            {
                if (SS1_Sheet1.Cells[i, 0].Text == "True")
                {
                    nCNT += 1;
                    nSum += Convert.ToInt64(VB.Replace(SS1_Sheet1.Cells[i, 2].Text, ",", ""));
                }

            }

            if (nCNT > 0)
            {
                btnSave3.Enabled = true;
                lbl_SelCnt.Text = nCNT.ToString();
                lbl_SelAmt.Text = VB.Format(nSum, "###,###,###,##0");
                Sel_USel_CHK("1");
            }
            else
            {
                Sel_USel_CHK("");
            }
        }

        private void Sel_USel_CHK(string argGubun)
        {
            if (argGubun == "1")
            {
                txtPano.Enabled = false;
                cboGubun.Text = "2.미수입금";
                cboGubun.Enabled = false;
                //cboBuse.Text = "";
                //cboBuse.Enabled = false;
                dtpDate.Text = clsPmpaPb.GstrActDate;
                dtpDate.Enabled = false;
                btnAmtSet.Enabled = false;
                cboDept.Text = "";
                cboDept.Enabled = false;
                cboIO.Text = "";
                cboIO.Enabled = false;
                txtAmt.Text = "";
                txtAmt.Enabled = false;
                cboMisuGbn.Text = "";
                cboMisuGbn.Enabled = false;
                cboBun.BackColor = Color.FromArgb(203, 248, 192);
                txtRemark.BackColor = Color.FromArgb(203, 248, 192);
                txtSabun.BackColor = Color.FromArgb(203, 248, 192);
            }
            else
            {
                txtPano.Enabled = true;
                cboGubun.Text = "";
                cboGubun.Enabled = true;
                cboBuse.Text = "";
                cboBuse.Enabled = true;
                dtpDate.Enabled = true;
                btnAmtSet.Enabled = true;
                cboDept.Enabled = true;
                cboIO.Enabled = true;
                txtAmt.Enabled = true;
                cboMisuGbn.Enabled = true;
                cboBuse.BackColor = Color.White;
                cboMisuGbn.BackColor = Color.White;
                cboBun.BackColor = Color.White;
                txtRemark.BackColor = Color.White;
                txtSabun.BackColor = Color.White;
            }
        }

        private void SS_Posco_ButtonClicked(object sender, EditorNotifyEventArgs e)
        {
            int k = 0;
            long nSum = 0;

            lblAmt.Text = "";

            if (e.Column != 0)
            {
                return;
            }

            if (SS_Posco_Sheet1.Cells[e.Row, 0].Text == "True")
            {
                SS_Posco_Sheet1.Cells[e.Row, 0, e.Row, SS_Posco_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(203, 248, 192);
            }
            else
            {
                SS_Posco_Sheet1.Cells[e.Row, 0, e.Row, SS_Posco_Sheet1.ColumnCount - 1].BackColor = Color.White;
            }

            for (k = 0; k < SS_Posco_Sheet1.Rows.Count; k++)
            {
                if (SS_Posco_Sheet1.Cells[k, 0].Text == "True")
                {
                    nSum += Convert.ToInt64(VB.Replace(SS_Posco_Sheet1.Cells[k, 3].Text, ",", ""));
                }

            }

            lblAmt.Text = VB.Format(nSum, "###,###,###,##0");
        }

        private void Insert_Data()
        {
            string strOkPano = string.Empty;
            string strOkAmt = string.Empty;
            string strOkName = string.Empty;
            string strMisu = string.Empty;
            string strPobun = string.Empty;
            string strMsg = string.Empty;
            long nMamt = 0;

            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            clsPmpaPrint CPP = new clsPmpaPrint();

            Cursor.Current = Cursors.WaitCursor;
             

            try
            {
                ComFunc.ReadSysDate(clsDB.DbCon);

                clsPmpaPb.GstrActDate = clsPublic.GstrSysDate;

                #region Data_Check
                if (VB.Left(cboGubun.Text, 1) == "2" && VB.Left(cboGubun.Text, 1) == "3" && VB.Left(cboGubun.Text, 1) == "4" && VB.Left(cboGubun.Text, 1) == "5" && dtpDate.Text != clsPmpaPb.GstrActDate)
                {
                    ComFunc.MsgBox("입금은 회계일자를 수정하지 못함", "확인");
                    dtpDate.Text = clsPmpaPb.GstrActDate;
                    dtpDate.Focus();
                    return;
                }

                if (cboGubun.Text.Length == 0)
                {
                    ComFunc.MsgBox("발생,입금 구분을 입력하세요", "확인");
                    cboGubun.Focus();
                    return;
                }
                else if (txtPano.Text.Length != 8)
                {
                    ComFunc.MsgBox("등록번호를 정확히 입력하세요", "확인");
                    txtPano.Focus();
                    return;
                }
                else if (cboBuse.Text.Length == 0)
                {
                    ComFunc.MsgBox("부서를 입력하세요", "확인");
                    cboBuse.Focus();
                    return;
                }
                else if (Convert.ToInt64(txtAmt.Text) == 0)
                {
                    ComFunc.MsgBox("금액이 0원 입니다", "확인");
                    txtAmt.Focus();
                    return;
                }
                else if (txtRemark.Text.Length == 0)
                {
                    ComFunc.MsgBox("적요란에 자세히 입력하세요", "확인");
                    txtRemark.Focus();
                    return;
                }
                //else if (nAmt2 == 0 && cboGubun.Text.Trim() == "2.미수입금")
                //{
                //    ComFunc.MsgBox("미수 잔액이 없읍니다", "확인");
                //    cboGubun.Focus();
                //    return;
                //}
                else if (Convert.ToInt16(VB.Left(cboBuse.Text, 1)) > 3 && VB.Left(cboGubun.Text, 1) == "2" && VB.Left(cboGubun.Text, 1) == "3" && VB.Left(cboGubun.Text, 1) == "4" && VB.Left(cboGubun.Text, 1) == "5")
                {
                    ComFunc.MsgBox("미수입금은 외래,응급실,입원수납만 가능합니다", "확인");
                    cboBuse.Focus();
                    return;
                }
                else if (Convert.ToInt64(txtAmt.Text) > nAmt3 && VB.Left(cboGubun.Text, 1) == "2" && VB.Left(cboGubun.Text, 1) == "3" && VB.Left(cboGubun.Text, 1) == "4" && VB.Left(cboGubun.Text, 1) == "5")
                {
                    ComFunc.MsgBox("입금액이 미수잔액 보다 큼 ! ", "확인");
                    txtAmt.Focus();
                    return;
                }
                else if (cboDept.Text.Length == 0)
                {
                    ComFunc.MsgBox("진료과를 입력 해주세요!!!", "확인");
                    cboDept.Focus();
                    return;
                }
                else if (cboIO.Text.Length == 0)
                {
                    ComFunc.MsgBox("입원.외래 구분을 입력 해주세요!!!", "확인");
                    cboIO.Focus();
                    return;
                }
                else if (cboBuse.Text.Length == 0)
                {
                    ComFunc.MsgBox("미수구분을 입력 해주세요!!!", "확인");
                    cboBuse.Focus();
                    return;
                }

                string strJumin = SS2_Sheet1.Cells[2, 1].Text.Trim();

                //2012-06-25
                if (cboDept.Text.Trim() == "PD" && VB.Left(cboMisuGbn.Text, 2) == "13" && ComFunc.AgeCalcEx_Zero(strJumin, dtpDate.Text.Trim()) < 15.6)
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT SUM(AMT1) AMT1 FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
                    SQL += ComNum.VBLF + "  WHERE PANO ='" + txtPano.Text + "' ";
                    SQL += ComNum.VBLF + "    AND SUCODE ='Y96' ";
                    SQL += ComNum.VBLF + "    AND DEPTCODE ='" + cboDept.Text + "' ";
                    SQL += ComNum.VBLF + "    AND BDATE =TRUNC(SYSDATE) ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (Dt.Rows.Count > 0)
                    {
                        if (Convert.ToInt64(Dt.Rows[0]["AMT1"].ToString()) <= 0)
                        {
                            clsPublic.GstrMsgList = "";
                            clsPublic.GstrMsgList += "필수예방접종 미수인데 당일 미수가 없습니다." + ComNum.VBLF + ComNum.VBLF;
                            clsPublic.GstrMsgList += "이대로 등록하시겠습니까?";
                            if (ComFunc.MsgBoxQ(clsPublic.GstrMsgList, "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                                return;
                        }
                    }

                    Dt.Dispose();
                    Dt = null;
                }
                else if (cboDept.Text.Trim() == "PD" && VB.Left(cboMisuGbn.Text, 2) != "13" && ComFunc.AgeCalcEx_Zero(strJumin, dtpDate.Text.Trim()) < 15.6)
                {
                    clsPublic.GstrMsgList = "";
                    clsPublic.GstrMsgList += "소아가 미수구분이 필수예방접종이 아닙니다." + ComNum.VBLF + ComNum.VBLF;
                    clsPublic.GstrMsgList += "이대로 등록하시겠습니까?";
                    if (ComFunc.MsgBoxQ(clsPublic.GstrMsgList, "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                        return;
                }

                if (Convert.ToInt64(txtAmt.Text) < 0 && VB.Left(cboGubun.Text, 1) == "1")
                {
                    if (dtpDate.Text != clsPublic.GstrSysDate)
                    {
                        clsPublic.GstrMsgList = "회계일자가 오늘이 아닌 미수내역은 취소불가";
                        ComFunc.MsgBox(clsPublic.GstrMsgList, "경고");
                        txtAmt.Focus();
                        return;
                    }
                    else
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT SUM(Amt) cAmt                                          ";
                        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP                    ";
                        SQL += ComNum.VBLF + "  WHERE PANO = '" + txtPano.Text + "'                          ";
                        SQL += ComNum.VBLF + "    AND BDate = TO_DATE('" + dtpDate.Text + "','yyyy-mm-dd')   ";
                        SQL += ComNum.VBLF + "    AND Gubun1 = '1'                                           ";
                        SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (Dt.Rows.Count == 1)
                        {
                            nMamt = Convert.ToInt64(Dt.Rows[0]["cAMT"].ToString());
                        }

                        if ((Convert.ToInt64(txtAmt.Text) * -1) > nMamt)
                        {
                            ComFunc.MsgBox("오늘 발생한 미수금액 보다 취소금액이 더 큽니다.", "경고");
                            txtAmt.Focus();
                            Dt.Dispose();
                            Dt = null;
                            return;
                        }
                        Dt.Dispose();
                        Dt = null;
                    }
                }

                strMisu = "";
                strMisu += VB.Left(cboIO.Text.Trim(), 1);
                strMisu += VB.Left(cboDept.Text + "  ", 2);
                strMisu += VB.Left(cboMisuGbn.Text + "  ", 2);
                strMisu += VB.Format("000000000", "000000000");
                strMisu += VB.Left("" + VB.Space(8), 8);
                strMisu += VB.Left("" + VB.Space(8), 8);

                if (VB.Left(strMisu, 1) != "I" && VB.Left(strMisu, 1) != "O")
                {
                    ComFunc.MsgBox("외래,입원 구분이 오류입니다.", "확인");
                    return;
                }
                if (VB.Mid(strMisu, 2, 2).Trim() == "")
                {
                    ComFunc.MsgBox("진료과가 공란입니다.", "확인");
                    cboDept.Focus();
                    return;
                }

                if (cPM.READ_MisuGye(VB.Mid(strMisu, 4, 2).Trim()) == "")
                {
                    ComFunc.MsgBox("미수계정이 오류입니다.", "확인");
                    cboMisuGbn.Focus();
                    return;
                }

                #endregion

                clsDB.setBeginTran(clsDB.DbCon);

                if (Misu_Mst_Insert(clsDB.DbCon) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                if (VB.Left(cboGubun.Text, 1) == "2")         //미수입금
                {
                    clsPmpaPb.GstrJeaSunap = "YES";

                    string strName = string.Empty;
                    string strBi = string.Empty;

                    Dt = cPF.Get_BasPatient(clsDB.DbCon, txtPano.Text);
                    if (Dt.Rows.Count == 0)
                    {
                        ComFunc.MsgBox("등록번호가 오류입니다." + ComNum.VBLF + "다시 입력하세요.", "확인");
                        Dt.Dispose();
                        Dt = null;
                        return;
                    }
                    else
                    {
                        strName = Dt.Rows[0]["Sname"].ToString().Trim();
                        strBi = Dt.Rows[0]["Bi"].ToString().Trim();
                    }

                    Dt.Dispose();
                    Dt = null;

                    CPP.Report_Print_Misu_New(clsDB.DbCon, txtPano.Text.Trim(), strName, VB.Left(cboDept.Text, 2), strBi, VB.Replace(txtAmt.Text, ",", "").Trim(), chkPrt.Checked);
                    
                    //미수입금시 현금영수증 자동 팝업 의뢰서 kyo 2017-04-12
                    CARD.CardVariable_Clear(ref RSD, ref RD);
                    CARD.gstrCdPtno = txtPano.Text.Trim();
                    CARD.gstrCdSName = SS2_Sheet1.Cells[0, 1].Text.Trim();
                    CARD.gstrCdDeptCode = cboDept.Text.Trim();
                    CARD.gstrCdPart = clsType.User.JobPart;
                    CARD.gstrCdGbIo = "O";
                    CARD.gstrCdPCode = "MIS+";
                    CARD.glngCdAmt = Convert.ToInt64(txtAmt.Text);
                    CARD.GstrCardJob = "Menual2";

                    frmPmpaEntryCardDaou f = new frmPmpaEntryCardDaou(CARD.gstrCdPtno, CARD.gstrCdSName,"", "O", 0, "CASH", dtpDate.Text);
                    f.ShowDialog();
                }

                Screen_Clear();
                CARD.CardVariable_Clear(ref RSD, ref RD);

                Screen_Display();
                txtPano.Focus();

                cboGubun.SelectedIndex = 0;
                cboBuse.SelectedIndex = 0;
                cboBun.SelectedIndex = 0;

                return;
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

        private bool Misu_Mst_Insert(PsmhDb pDbCon)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            long nAmt = 0;
            string strMisu = string.Empty;
            string strROWID = string.Empty;
            string strPobun = string.Empty;

            long nWRTNO = 0;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT ROWID,MAmt,IAmt,JAmt ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GAINMST ";
                SQL += ComNum.VBLF + "  WHERE Pano = '" + txtPano.Text + "' ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (Dt.Rows.Count == 1)
                {
                    strROWID = Dt.Rows[0]["ROWID"].ToString().Trim();
                    nAmt1 = Convert.ToInt64(VB.Val(Dt.Rows[0]["MAmt"].ToString()));
                    nAmt2 = Convert.ToInt64(VB.Val(Dt.Rows[0]["IAmt"].ToString()));
                }
                else
                {
                    strROWID = ""; nAmt1 = 0; nAmt2 = 0;
                }
                Dt.Dispose();
                Dt = null;

                switch (VB.Left(cboGubun.Text, 1))
                {
                    case "1":
                        nAmt1 += Convert.ToInt64(txtAmt.Text);
                        break;
                    case "2":
                    case "3":
                    case "4":
                        nAmt2 += Convert.ToInt64(txtAmt.Text);
                        break;
                    default:
                        break;
                }

                strPobun = VB.Left(cboBun.Text, 1);
                nAmt = nAmt1 - nAmt2;

                SQL = "";
                if (strROWID == "")
                {
                    SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "MISU_GAINMST    ";
                    SQL += ComNum.VBLF + "        (Pano, MAmt, IAmt, JAmt)                   ";
                    SQL += ComNum.VBLF + " VALUES                                            ";
                    SQL += ComNum.VBLF + "        ('" + txtPano.Text + "', " + nAmt1 + ",    ";
                    SQL += ComNum.VBLF + "         " + nAmt2 + ", " + nAmt + ")              ";
                }
                else
                {
                    SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "MISU_GAINMST ";
                    SQL += ComNum.VBLF + "    SET MAmt = " + nAmt1 + ",              ";
                    SQL += ComNum.VBLF + "        IAmt = " + nAmt2 + ",              ";
                    SQL += ComNum.VBLF + "        JAmt = " + nAmt + "                ";
                    SQL += ComNum.VBLF + "  WHERE ROWID = '" + strROWID + "'         ";
                }
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //포스코 위탁 상세내역관련 wrtno
                if (strPobun == "1")
                {
                    nWRTNO = cPF.READ_New_Mir_Posco_Wrtno(clsDB.DbCon);
                }

                strMisu = "";
                strMisu += VB.Left(cboIO.Text.Trim(), 1);
                strMisu += VB.Left(cboDept.Text + "  ", 2);
                strMisu += VB.Left(cboMisuGbn.Text + "  ", 2);
                strMisu += VB.Format("000000000", "000000000");
                strMisu += VB.Left("" + VB.Space(8), 8);
                strMisu += VB.Left("" + VB.Space(8), 8);

                if (Misu_Dtl_Data_Insert(clsDB.DbCon, strMisu, strPobun, nWRTNO, txtAmt.Text) == false)
                {
                    return rtnVal;
                }

                if (nAmt == 0)
                {
                    if (Misu_Dtl_Data_Update(clsDB.DbCon) == false)
                    {
                        return rtnVal;
                    }
                }

                //포스코 위탁 상세내역 저장
                if (nWRTNO > 0)
                {
                    if (Posco_Exam_Detail_Set(clsDB.DbCon, nWRTNO, txtPano.Text.Trim(), dtpDate.Text) == false)
                    {
                        return rtnVal;
                    }
                }

                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private bool Misu_Dtl_Data_Insert(PsmhDb pDbCon, string ArgMisu, string strPobun, long nWRTNO, string argAmt)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;

            string strMisu = string.Empty;

            try
            {               
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "MISU_GAINSLIP                                          ";
                SQL += ComNum.VBLF + "        (Pano,Gubun1,Gubun2,Bdate,Amt,Remark,IDno,Flag,                                   ";
                SQL += ComNum.VBLF + "         Part,MisuDtl,EntTime, CardSeqNo, SABUN, POBUN,WRTNO)                             ";
                SQL += ComNum.VBLF + " VALUES ('" + txtPano.Text + "', '" + VB.Left(cboGubun.Text, 1) + "',                     ";
                SQL += ComNum.VBLF + "        '" + VB.Left(cboBuse.Text, 1) + "',                                               ";
                SQL += ComNum.VBLF + "        TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD'),                                     ";
                SQL += ComNum.VBLF + "         " + Convert.ToInt64(VB.Val(argAmt)) + ", '" + txtRemark.Text.Trim() + "',           ";
                SQL += ComNum.VBLF + "        '" + clsType.User.IdNumber + "', '0', '" + clsType.User.JobPart + "',             ";
                SQL += ComNum.VBLF + "        '" + ArgMisu + "', SYSDATE,                                                       ";
                SQL += ComNum.VBLF + "        '" + VB.IIf(clsPmpaType.RSD.TotAmt > 0, clsPmpaType.RSD.CardSeqNo, "").ToString() + "',   ";
                SQL += ComNum.VBLF + "        '" + txtSabun.Text.Trim() + "','" + strPobun + "'," + nWRTNO + " )                ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private bool Misu_Dtl_Data_Update(PsmhDb pDbCon)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "MISU_GAINSLIP  ";
                SQL += ComNum.VBLF + "    SET FLAG = '*' ";
                SQL += ComNum.VBLF + "  WHERE Pano = '" + txtPano.Text + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void Insert_Data_Batch()
        {
            int i = 0;
            long nMamt = 0;
            long nAmt = 0;
            string strOkPano = string.Empty;
            string strOkAmt = string.Empty;
            string strOkName = string.Empty;
            string strMisu = string.Empty;
            string strPobun = string.Empty;
            string strMsg = string.Empty;
            string strDept = string.Empty;
            string strIO = string.Empty;
            string strBuse = string.Empty;
            string strMisuGbn = string.Empty;
            string strPrt = string.Empty;
            
            Cursor.Current = Cursors.WaitCursor;
            clsPmpaPrint CPP = new clsPmpaPrint();


            try
            {
                
                if (ComFunc.MsgBoxQ("선택한것을 일괄입금 작업을 하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                    return;

                if (chkPrt.Checked)
                {
                    strPrt = "N";
                }

                clsPmpaPb.GstrActDate = clsPublic.GstrSysDate;

                if (VB.Left(cboGubun.Text, 1) == "2" && dtpDate.Text != clsPmpaPb.GstrActDate)
                {
                    ComFunc.MsgBox("입금은 회계일자를 수정하지 못함", "확인");
                    dtpDate.Text = clsPmpaPb.GstrActDate;
                    dtpDate.Focus();
                    return;
                }

                if (VB.Left(cboGubun.Text, 1) != "2")
                {
                    ComFunc.MsgBox("미수구분에 입금만 가능합니다..", "확인");
                    cboGubun.Focus();
                    return;
                }
                else if (txtPano.Text.Length != 8)
                {
                    ComFunc.MsgBox("등록번호를 정확히 입력하세요", "확인");
                    txtPano.Focus();
                    return;
                }
                else if (txtRemark.Text.Length == 0)
                {
                    ComFunc.MsgBox("적요란에 자세히 입력하세요", "확인");
                    txtRemark.Focus();
                    return;
                }
                else if (Convert.ToInt64(VB.Replace(lbl_SelAmt.Text, ",", "")) > nAmt3 && VB.Left(cboGubun.Text, 1) == "2")
                {
                    ComFunc.MsgBox("입금액이 미수잔액 보다 큼 ! ", "확인");
                    return;
                }

                

                for (i = 0; i < SS1_Sheet1.Rows.Count; i++)
                {
                    if (SS1_Sheet1.Cells[i, 0].Text == "True")
                    {
                        nAmt = Convert.ToInt64(VB.Replace(SS1_Sheet1.Cells[i, 2].Text, ",", ""));
                        strBuse = VB.Left(SS1_Sheet1.Cells[i, 4].Text, 1);
                        strIO = SS1_Sheet1.Cells[i, 7].Text.Trim();
                        strDept = SS1_Sheet1.Cells[i, 8].Text.Trim();
                        strMisuGbn = VB.Left(SS1_Sheet1.Cells[i, 9].Text, 2);

                        clsDB.setBeginTran(clsDB.DbCon);

                        if (Misu_Mst_Insert_SEL(clsDB.DbCon, strIO, strDept, nAmt, strBuse, strMisuGbn) == false)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        clsDB.setCommitTran(clsDB.DbCon);

                        //미수입금2 이면 인쇄
                        clsPmpaPb.GstrJeaSunap = "YES";
                        //TODO : 프린트 모듈 작성
                      //  Report_Print_Misu_new(strDept, Trim(str(nAmt)), strPrt);    //영수증 발행 new 2010영수증
                        CPP.Report_Print_Misu_New(clsDB.DbCon, txtPano.Text.Trim(), lblSName.Text, VB.Left(cboDept.Text, 2), "", VB.Replace(SS1_Sheet1.Cells[i, 2].Text, ",", "").Trim(), chkPrt.Checked);
                    }
                }

                
                Cursor.Current = Cursors.Default;

                Screen_Clear();
                CARD.CardVariable_Clear(ref RSD, ref RD);
                Sel_USel_CHK("");

                Screen_Display();
                txtPano.Focus();

                cboBuse.SelectedIndex = 0;
                cboBun.SelectedIndex = 0;
                
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private bool Misu_Mst_Insert_SEL(PsmhDb pDbCon, string ArgIO, string ArgDept, long ArgAmt, string ArgBuse, string ArgMisuGbn)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            long nAmt = 0;
            string strAmt = string.Empty;
            string strMisu = string.Empty;
            string strROWID = string.Empty;
            string strPobun = string.Empty;

            long nWRTNO = 0;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT ROWID,MAmt,IAmt,JAmt ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GAINMST ";
                SQL += ComNum.VBLF + "  WHERE Pano = '" + txtPano.Text + "' ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                if (Dt.Rows.Count == 1)
                {
                    strROWID = Dt.Rows[0]["ROWID"].ToString().Trim();
                    nAmt1 = Convert.ToInt64(VB.Val(Dt.Rows[0]["MAmt"].ToString()));
                    nAmt2 = Convert.ToInt64(VB.Val(Dt.Rows[0]["IAmt"].ToString()));
                }
                else
                {
                    strROWID = ""; nAmt1 = 0; nAmt2 = 0;
                }
                Dt.Dispose();
                Dt = null;

                switch (VB.Left(cboGubun.Text, 1))
                {
                    case "1":
                        nAmt1 += ArgAmt;
                        break;
                    case "2":
                    case "3":
                    case "4":
                        nAmt2 += ArgAmt;
                        break;
                    default:
                        break;
                }

                strPobun = VB.Left(cboBun.Text, 1);
                nAmt = nAmt1 - nAmt2;

                SQL = "";
                if (strROWID == "")
                {
                    SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "MISU_GAINMST    ";
                    SQL += ComNum.VBLF + "        (Pano, MAmt, IAmt, JAmt)                   ";
                    SQL += ComNum.VBLF + " VALUES                                            ";
                    SQL += ComNum.VBLF + "        ('" + txtPano.Text + "', " + nAmt1 + ",    ";
                    SQL += ComNum.VBLF + "         " + nAmt2 + ", " + nAmt + ")              ";
                }
                else
                {
                    SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "MISU_GAINMST ";
                    SQL += ComNum.VBLF + "    SET MAmt = " + nAmt1 + ",              ";
                    SQL += ComNum.VBLF + "        IAmt = " + nAmt2 + ",              ";
                    SQL += ComNum.VBLF + "        JAmt = " + nAmt + "                ";
                    SQL += ComNum.VBLF + "  WHERE ROWID = '" + strROWID + "'         ";
                }
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                strMisu = "";
                strMisu += ArgIO;
                strMisu += VB.Left(ArgDept + "  ", 2);
                strMisu += VB.Left(ArgMisuGbn + "  ", 2);
                strMisu += VB.Format("000000000", "000000000");
                strMisu += VB.Left("" + VB.Space(8), 8);
                strMisu += VB.Left("" + VB.Space(8), 8);
                
                if (Misu_Dtl_Data_Insert(clsDB.DbCon, strMisu, strPobun, nWRTNO, ArgAmt.ToString()) == false)
                {
                    return rtnVal;
                }

                if (nAmt == 0)
                {
                    if (Misu_Dtl_Data_Update(clsDB.DbCon) == false)
                    {
                        return rtnVal;
                    }
                }
                
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void Send_SMS()
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";
            
            try
            {
                clsPublic.GstrRetValue = "";
                
                SQL = "";
                SQL += ComNum.VBLF + " SELECT HPhone,Tel,Sname From " + ComNum.DB_PMPA + "Bas_Patient ";
                SQL += ComNum.VBLF + "  Where Pano = '" + txtPano.Text.Trim() + "' ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (Dt.Rows.Count > 0)
                {
                    clsPublic.GstrRetValue = Dt.Rows[0]["HPhone"].ToString().Trim();
                    if (clsPublic.GstrRetValue == "")
                        clsPublic.GstrRetValue = Dt.Rows[0]["Tel"].ToString().Trim(); 
                }

                clsPublic.GstrRetValue += "^^" + Dt.Rows[0]["SNAME"].ToString().Trim(); 
                
                Dt.Dispose();
                Dt = null;
                
                //이미 폼이 떠있는지 확인한다.
                foreach (Form frmFindform in Application.OpenForms)
                {
                    if (frmFindform.GetType() == typeof(frmSMS_Misu))
                    {
                        frmFindform.Activate();
                        frmFindform.BringToFront();
                        return;
                    }
                }
               
                frmSMS_Misu frm = new frmSMS_Misu(1,VB.Left(clsPublic.GstrRetValue,13));
                frm.ShowDialog();
                
                clsPublic.GstrRetValue = "";
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

        //코로나 필수예방 접종
        private void Set_Vacc_Covid()
        {
            DataTable Dt = null;
            DataTable Dt2 = null;
            string SQL = "";
            string SqlErr = "";

            int i = 0, nRead = 0, nRow = 0;

            try
            {
                SS3.ActiveSheet.ClearRange(0, 0, SS3_Sheet1.Rows.Count, SS3_Sheet1.ColumnCount, false);
                SS3_Sheet1.Rows.Count = 0;

                //필수예방접종 미수조회함
                SQL = "";
                SQL += ComNum.VBLF + " SELECT SuCode,Amt1,SEQNO,PART ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
                SQL += ComNum.VBLF + "  WHERE PANO ='" + txtPano.Text + "' ";
                SQL += ComNum.VBLF + "    AND BDATE =TRUNC(SYSDATE) ";
                SQL += ComNum.VBLF + "    AND DEPTCODE ='MD' ";
                SQL += ComNum.VBLF + "    AND SUCODE ='Y96' ";
                SQL += ComNum.VBLF + "  ORDER BY ENTDATE  DESC ";
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
                        nRow += 1;
                        if (nRow > SS3_Sheet1.Rows.Count)
                        {
                            SS3_Sheet1.Rows.Count = nRow;
                        }

                        SS3_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["SuCode"].ToString().Trim();
                        SS3_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["Amt1"].ToString().Trim();
                        SS3_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["Part"].ToString().Trim();
                        SS3_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["SEQNO"].ToString().Trim();

                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT ROWID FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP ";
                        SQL += ComNum.VBLF + " WHERE PANO ='" + txtPano.Text + "' ";
                        SQL += ComNum.VBLF + "  AND BDATE =  TRUNC(SYSDATE)";
                        SQL += ComNum.VBLF + "  AND Gubun1 ='1' ";
                        SQL += ComNum.VBLF + "  AND PoBun ='2' ";
                        SQL += ComNum.VBLF + "  AND Amt =" + Convert.ToInt64(Dt.Rows[i]["AMT1"].ToString()) + " ";
                        SqlErr = clsDB.GetDataTable(ref Dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (Dt2.Rows.Count > 0)
                        {
                            SS3_Sheet1.Cells[i, 0, i, SS3_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(255, 196, 196);
                        }
                        else
                        {
                            SS3_Sheet1.Cells[i, 0, i, SS3_Sheet1.ColumnCount - 1].BackColor = Color.White;
                        }

                        Dt2.Dispose();
                        Dt2 = null;
                    }
                }

                Dt.Dispose();
                Dt = null;

                if (cboDept.Text == "")
                {
                    cboDept.Text = "MD.내과";
                    cboIO.Text = "O.외래";
                    cboMisuGbn.Text = "13.필수접종국가지원";
                    cboBun.Text = "2.계산서 발부";
                    txtRemark.Text = "보건소 청구건-코로나접종";
                    SS3.Visible = true;
                }
                else if (VB.Left(cboDept.Text, 2) != "MD")
                {
                    ComFunc.MsgBox("코로나 예방접종 미수발생은 내과만 가능합니다..!!", "");
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

        //필수예방 접종
        private void Set_Vacc()
        {
            DataTable Dt = null;
            DataTable Dt2 = null;
            string SQL = "";
            string SqlErr = "";

            int i = 0, nRead = 0, nRow = 0;

            try
            {
                SS3.ActiveSheet.ClearRange(0, 0, SS3_Sheet1.Rows.Count, SS3_Sheet1.ColumnCount, false);
                SS3_Sheet1.Rows.Count = 0;

                //필수예방접종 미수조회함
                SQL = "";
                SQL += ComNum.VBLF + " SELECT SuCode,Amt1,SEQNO,PART ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
                SQL += ComNum.VBLF + "  WHERE PANO ='" + txtPano.Text + "' ";
                SQL += ComNum.VBLF + "    AND BDATE =TRUNC(SYSDATE) ";
                SQL += ComNum.VBLF + "    AND DEPTCODE ='PD' ";
                SQL += ComNum.VBLF + "    AND SUCODE ='Y96' ";
                SQL += ComNum.VBLF + "  ORDER BY ENTDATE  DESC ";
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
                        nRow += 1;
                        if (nRow > SS3_Sheet1.Rows.Count)
                        {
                            SS3_Sheet1.Rows.Count = nRow;
                        }

                        SS3_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["SuCode"].ToString().Trim();
                        SS3_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["Amt1"].ToString().Trim();
                        SS3_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["Part"].ToString().Trim();
                        SS3_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["SEQNO"].ToString().Trim();
                        
                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT ROWID FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP ";
                        SQL += ComNum.VBLF + " WHERE PANO ='" + txtPano.Text + "' ";
                        SQL += ComNum.VBLF + "  AND BDATE =  TRUNC(SYSDATE)";
                        SQL += ComNum.VBLF + "  AND Gubun1 ='1' ";
                        SQL += ComNum.VBLF + "  AND PoBun ='2' ";
                        SQL += ComNum.VBLF + "  AND Amt =" + Convert.ToInt64(Dt.Rows[i]["AMT1"].ToString()) + " ";
                        SqlErr = clsDB.GetDataTable(ref Dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (Dt2.Rows.Count > 0)
                        {
                            SS3_Sheet1.Cells[i, 0, i, SS3_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(255, 196, 196);
                        }
                        else
                        {
                            SS3_Sheet1.Cells[i, 0, i, SS3_Sheet1.ColumnCount - 1].BackColor = Color.White;
                        }

                        Dt2.Dispose();
                        Dt2 = null;
                    }
                }

                Dt.Dispose();
                Dt = null;

                if (cboDept.Text == "")
                {
                    cboDept.Text = "PD.소아청소년과";
                    cboIO.Text = "O.외래";
                    cboMisuGbn.Text = "13.필수접종국가지원";
                    cboBun.Text = "2.계산서 발부";
                    txtRemark.Text = "필수예방접종";
                    SS3.Visible = true;
                }
                else if (VB.Left(cboDept.Text, 2) != "PD")
                {
                    ComFunc.MsgBox("필수예방접종 미수발생은 소아청소년과만 가능합니다..!!", "");
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

        //회사예방 접종 세팅
        private void Set_LtdVacc()
        {
            Screen_Display();

            dtpDate.Text = clsPublic.GstrSysDate;
                
            cboIO.Text = "O.외래";
            cboMisuGbn.Text = "14.회사접종";            
            cboBun.Text = "2.계산서 발부";
            SS3.Visible = true;
        }

        //포스코 위탁검사
        private void Set_Posco()
        {
            this.Size = new System.Drawing.Size(1266, 830);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int i = 0;
            string strROWID = "";

            if (DialogResult.No == ComFunc.MsgBoxQ("선택하신 내용을 삭제하시겠습니까?", "알림"))
            {
                return;
            }

            for (i = 0; i < SSSunap_Sheet1.RowCount; i++)
            {
                if (SSSunap_Sheet1.Cells[i, 0].Value.ToString().Trim() == "True")
                {
                    strROWID = SSSunap_Sheet1.Cells[i, 5].Text.Trim();

                    if (strROWID != "")
                    {
                        if (DeleteOPDSunap(strROWID) == false)
                            return;
                    }
                }
            }

            ComFunc.MsgBox("미수입금 수납내역 삭제되었습니다. 미수마스터를 반드시 정리하시기 바랍니다. ", "확인");

            ReadOpdSunap(txtPano.Text.Trim());
        }

        //금연처방
        private void Set_Smoking()
        {
            Screen_Display();

            dtpDate.Text = clsPublic.GstrSysDate;

            cboIO.Text = "O.외래";
            cboMisuGbn.Text = "15.금연처방";
            cboBun.Text = "2.계산서 발부";
            SS3.Visible = true;
        }

        //가다실접종
        private void Set_GadaSil()
        {
            Screen_Display();

            dtpDate.Text = clsPublic.GstrSysDate;

            cboIO.Text = "O.외래";
            cboMisuGbn.Text = "11.기관청구";
            cboBun.Text = "2.계산서 발부";
            SS3.Visible = true;
        }

        private bool DeleteOPDSunap(string strROWID)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            SQL = "";
            SQL += ComNum.VBLF + " INSERT INTO KOSMOS_PMPA.OPD_SUNAP_DEL( ";
            SQL += ComNum.VBLF + "         ACTDATE, STIME, PANO, AMT, PART, ";
            SQL += ComNum.VBLF + "         SEQNO, REMARK, BIGO, TOTAMT, JANAMT, ";
            SQL += ComNum.VBLF + "         AMT1, AMT2, AMT3, AMT5, AMT4, ";
            SQL += ComNum.VBLF + "         SEQNO2, DEPTCODE, BI, CARDGB, YAKAMT, ";
            SQL += ComNum.VBLF + "         DELDATE, DELPART, WORKGBN, GBSPC, REMARK2, ";
            SQL += ComNum.VBLF + "         GELCODE, MCODE, VCODE, BDAN, CDAN, ";
            SQL += ComNum.VBLF + "         JIN, JINDTL, ETCAMT, ETCAMT2, CARDAMT, ";
            SQL += ComNum.VBLF + "         ENTDATE, BDATE, PTAMT, JINDTL2, AL200, ";
            SQL += ComNum.VBLF + "         PART2, AMT6, AMT7, TAXDAN, GBCAN, OPDNO, ";
            SQL += ComNum.VBLF + "         WRITEDATE, WRITESABUN) ";
            SQL += ComNum.VBLF + " SELECT ";
            SQL += ComNum.VBLF + "         ACTDATE, STIME, PANO, AMT, PART, ";
            SQL += ComNum.VBLF + "         SEQNO, REMARK, BIGO, TOTAMT, JANAMT, ";
            SQL += ComNum.VBLF + "         AMT1, AMT2, AMT3, AMT5, AMT4, ";
            SQL += ComNum.VBLF + "         SEQNO2, DEPTCODE, BI, CARDGB, YAKAMT, ";
            SQL += ComNum.VBLF + "         DELDATE, DELPART, WORKGBN, GBSPC, REMARK2, ";
            SQL += ComNum.VBLF + "         GELCODE, MCODE, VCODE, BDAN, CDAN, ";
            SQL += ComNum.VBLF + "         JIN, JINDTL, ETCAMT, ETCAMT2, CARDAMT, ";
            SQL += ComNum.VBLF + "         ENTDATE, BDATE, PTAMT, JINDTL2, AL200, ";
            SQL += ComNum.VBLF + "         PART2, AMT6, AMT7, TAXDAN, GBCAN, OPDNO, ";
            SQL += ComNum.VBLF + "         SYSDATE, '" + clsType.User.Sabun + "' ";
            SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.OPD_SUNAP ";
            SQL += ComNum.VBLF + "  WHERE ROWID = '" + strROWID + "' ";
            SQL += ComNum.VBLF + "    AND ACTDATE = TRUNC(SYSDATE) ";
            SQL += ComNum.VBLF + "    AND REMARK = '미수입금' ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("미수입금 삭제 중 오류 발생 - 데이터 백업 중");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }

            SQL = " DELETE KOSMOS_PMPA.OPD_SUNAP ";
            SQL += ComNum.VBLF + "  WHERE ROWID = '" + strROWID + "' ";
            SQL += ComNum.VBLF + "    AND ACTDATE = TRUNC(SYSDATE) ";
            SQL += ComNum.VBLF + "    AND REMARK = '미수입금' ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("미수입금 삭제 중 오류 발생 - 데이터 삭제 중");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }

            return true;
        }


        private void ReadOpdSunap(string strPANO)
        {
            string rtnVal = string.Empty;

            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            int i = 0;

            //plOpdSunap.Visible = false;
            //SSSunap_Sheet1.Visible = false;


            SSSunap_Sheet1.Rows.Count = 0;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE, STIME, AMT, PART, ROWID ROWID1 ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SUNAP ";
                SQL += ComNum.VBLF + "  WHERE PANO = '" + strPANO + "' ";
                SQL += ComNum.VBLF + "    AND REMARK ='미수입금' ";
                SQL += ComNum.VBLF + "    AND ACTDATE = TRUNC(SYSDATE) ";
                SQL += ComNum.VBLF + "  ORDER BY STIME ASC ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Dt.Dispose();
                    Dt = null;
                    return;
                }
                if (Dt.Rows.Count > 0)
                {
                    //plOpdSunap.Visible = true;
                    //SSSunap_Sheet1.Visible = true;

                    SSSunap_Sheet1.Rows.Count = Dt.Rows.Count;

                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        SSSunap_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["ACTDATE"].ToString().Trim();
                        SSSunap_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["STIME"].ToString().Trim();
                        SSSunap_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["AMT"].ToString().Trim();
                        SSSunap_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["PART"].ToString().Trim();
                        SSSunap_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["ROWID1"].ToString().Trim();
                    }
                }

                Dt.Dispose();
                Dt = null;

                return;
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
                return;
            }

        }

    }
}
