using ComBase;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ComBase.Controls;
using ComLibB;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcLifeTools.cs
/// Description     : 생활습관 평가 도구
/// Author          : 이상훈
/// Create Date     : 2020-08-01
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmTools_2018.frm(FrmLife05)" />

namespace ComHpcLibB
{
    public partial class frmHcLifeTools : Form
    {
        HicJepsuService hicJepsuService = null;
        HicTitemService hicTitemService = null;
        HicResultService hicResultService = null;
        HicResBohum2Service hicResBohum2Service = null;
        HicTCodeService hicTCodeService = null;

        clsSpread sp = new clsSpread();
        clsHaBase hb = new clsHaBase();

        int[,] FnRowNo = new int[31, 31];
        long FnAge = 0;
        long FnPano = 0;
        long FnWRTNO = 0;
        long FnWrtno1 = 0;
        string FstrJepDate = "";
        string FstrTool;

        public frmHcLifeTools()
        {
            InitializeComponent();
            SetEvent();
        }

        public frmHcLifeTools(string strTool, long nWrtNo, string sJepDate, long nPano, long nAge)
        {
            InitializeComponent();
            FnWRTNO = nWrtNo;
            FstrJepDate = sJepDate;
            FnPano = nPano;
            FnAge = nAge;
            FstrTool = strTool;
            SetEvent();
        }

        void SetEvent()
        {
            hicJepsuService = new HicJepsuService();
            hicTitemService = new HicTitemService();
            hicResultService = new HicResultService();
            hicResBohum2Service = new HicResBohum2Service();
            hicTCodeService = new HicTCodeService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnOK.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick); 
            this.chkFirstPan.Click += new EventHandler(eChkBoxClick);
            this.SS10.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SS11.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SS12.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SS13.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SS14.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SS15.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SS16.CellDoubleClick += new CellClickEventHandler(eSpdDClick);

            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpdDClick); 

            this.SS10.EditModeOff += new EventHandler(eSpdEditModeOff);
            this.SS11.EditModeOff += new EventHandler(eSpdEditModeOff);
            this.SS12.EditModeOff += new EventHandler(eSpdEditModeOff);
            this.SS13.EditModeOff += new EventHandler(eSpdEditModeOff);
            this.SS14.EditModeOff += new EventHandler(eSpdEditModeOff);
            this.SS15.EditModeOff += new EventHandler(eSpdEditModeOff);
            this.SS16.EditModeOff += new EventHandler(eSpdEditModeOff);
            this.SS20.Change += new ChangeEventHandler(eSpdChange);
            this.SS21.Change += new ChangeEventHandler(eSpdChange);
            this.SS22.Change += new ChangeEventHandler(eSpdChange);
            this.SS23.Change += new ChangeEventHandler(eSpdChange);
            this.SS24.Change += new ChangeEventHandler(eSpdChange);
            this.SS25.Change += new ChangeEventHandler(eSpdChange);
            this.SS26.Change += new ChangeEventHandler(eSpdChange);
            this.txtBmi.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtCode1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtCode2.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtCode3.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtCode4.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtCode5.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtCode6.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtCode7.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtCode8.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtCode10.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtCode11.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtCode12.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtCode13.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtCode14.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtCode15.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtCode16.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtCode17.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtPaNo.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtWrtNo.KeyPress += new KeyPressEventHandler(eTxtKeyPress);

            this.txtCode1.TextChanged += new EventHandler(eTxtChanged);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            int nYEAR = 0;
            string strCount1 = "";
            string strCount2 = "";
            string strCount3 = "";

            this.Location = new Point(10, 10);

            strCount1 = "7";
            strCount2 = "24";
            strCount3 = "60";

            ComFunc.ReadSysDate(clsDB.DbCon);

            nYEAR = int.Parse(VB.Left(clsPublic.GstrSysDate, 4));

            //검진년도 Combo Set
            cboYear.Items.Clear();
            for (int i = 0; i <= 5; i++)
            {
                cboYear.Items.Add(string.Format("{0:0000}", nYEAR));
                nYEAR -= 1;
            }

            if (!VB.Pstr(FstrJepDate, "^^", 4).IsNullOrEmpty())
            {
                cboYear.Text = VB.Pstr(FstrJepDate, "^^", 4);
            }
            else
            {
                cboYear.SelectedIndex = 0;
            }

            FnWrtno1 = 0;

            ssList_Sheet1.Columns.Get(6).Visible = false;

            lblMsg.Text = "";
            txtSName.Text = "";
            txtWrtNo.Text = "";

            dtpFrDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-3).ToShortDateString();
            dtpToDate.Text = clsPublic.GstrSysDate;
            txtPaNo.Text = "";
            txtFirstSogen.Text = "";
            txtFirstSogen.Enabled = false;

            if (FstrTool == "접수")
            {
                if (!FnPano.IsNullOrEmpty())
                {
                    HIC_JEPSU list = hicJepsuService.GetWrtNoJepDatebyPaNoGjJongGjYear(FnPano, "11", cboYear.Text);
                    FnWrtno1 = 0;
                    if (!list.IsNullOrEmpty())
                    {
                        FstrJepDate = list.JEPDATE;
                        if (FnWRTNO == 0)
                        {
                            FnWRTNO = list.WRTNO;
                        }

                        if (FnWRTNO > 0)
                        {
                            txtWrtNo.Text = FnWRTNO.ToString(); //접수번호
                        }

                        FnWrtno1 = FnWRTNO; //1차접수번호
                        txtPaNo.Text = FnPano.ToString();
                        fn_Display_Munjin();
                    }
                }
                else
                {
                    tabControl1.Enabled = false;
                }
            }
            else
            {
                tabControl1.Enabled = false;
            }

            pnlSave.Visible = true;
        }

        void fn_Display_Munjin()
        {
            int nREAD = 0;
            int nHeight = 0;
            string strResult = "";
            string strExCode = "";
            string strJob = "";
            string strList = "";
            string strLtd = "";
            string strLtdCode = "";
            string strOldcode = "";
            int nRow = 0;
            string strOK = "";
            string strROWID = "";
            string strMsg = "";

            fn_Screen_Clear();

            //인적정보 표시
            HIC_JEPSU list = hicJepsuService.GetItembyWrtNo(FnWRTNO);

            if (!list.IsNullOrEmpty())
            {
                strMsg = "[성명:" + list.SNAME + "]";
                strMsg += " 접수번호:" + list.WRTNO;
                strMsg += " 접수일자:" + list.JEPDATE;
                strMsg += " 성별:" + list.SEX;
                strMsg += " 나이:" + list.AGE + "세";
                lblMsg.Text = strMsg;
            }
            else
            {
                lblMsg.Text = "환자정보 ERROR!!";
            }

            strROWID = "";
            strROWID = hicTitemService.GetRowIdRowIdbyWrtNo(FnWRTNO);

            tabControl1.Enabled = true;

            List<HIC_RESULT> list3 = hicResultService.GetExCodeResultbyWrtno1(FnWRTNO);

            for (int i = 0; i < list3.Count; i++)
            {
                switch (list3[i].EXCODE)
                {
                    case "A143":
                        tab0.Visible = true;
                        tabpnl0.Enabled = true;
                        SS10.Enabled = true;    //흡연
                        break;
                    case "A144":
                        tab1.Visible = true;
                        SS11.Enabled = true;    //음주
                        tabpnl1.Enabled = true;
                        break;
                    case "A145":
                        tab2.Visible = true;
                        SS12.Enabled = true;    //운동
                        tabpnl2.Enabled = true;
                        break;
                    case "A146":
                        tab3.Visible = true;
                        SS13.Enabled = true;    //영양
                        tabpnl3.Enabled = true;
                        break;
                    case "A147":
                        tab4.Visible = true;
                        SS14.Enabled = true;    //비만
                        tabpnl4.Enabled = true;
                        break;                    
                    case "A130":
                        tab5.Visible = true;
                        SS15.Enabled = true;    //우울증
                        tabpnl5.Enabled = true;
                        break;
                    case "A129":
                        tab6.Visible = true;
                        SS16.Enabled = true;    //인지기능
                        tabpnl6.Enabled = true;
                        break;
                    default:
                        break;
                }
            }

            fn_Item_Refresh();

            //자료검색
            fn_Screen_Display(FnWRTNO);
            fn_Screen_Display_ADD(FnWRTNO);

            if (tabpnl4.Enabled == true)
            {
                if (chkFirstPan.Checked == false)
                {
                    List<HIC_RESULT> list4 = hicResultService.GetExCodeResultbyWrtno1(FnWrtno1);

                    if (list4.Count > 0)
                    {
                        for (int i = 0; i < list4.Count; i++)
                        {
                            strExCode = list4[i].EXCODE;
                            switch (strExCode)
                            {
                                case "A101":
                                    txtHeight.Text = list4[i].RESULT;
                                    break;
                                case "A102":
                                    txtWeight.Text = list4[i].RESULT;
                                    break;
                                case "A115":
                                    txtWaist.Text = list4[i].RESULT;
                                    break;
                                case "A117":
                                    txtBmi.Text = list4[i].RESULT;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }                        
                }
                else
                {
                    MessageBox.Show("1차판정 내역이 없으므로 신장, 몸무게, ,허리둘레, 비만도 수치는 적용되지 않았습니다.", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                txtFirstSogen.Text = hicResBohum2Service.GetTSangdam1byWrtNo(FnWRTNO);
            }           
        }

        void eBtnClick(object sender, EventArgs e)
        {
            int result = 0;

            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSearch)
            {
                int nRow = 0;
                int nREAD = 0;
                string strOK = "";
                string strJong = "";
                string strROWID = "";

                long nWrtno = 0;
                string strFrDate = "";
                string strToDate = "";
                string strSName = "";

                sp.Spread_All_Clear(ssList);

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;
                if (txtWrtNo.Text != "")
                {
                    nWrtno = Convert.ToInt32(txtWrtNo.Text);
                }
                if (txtSName.Text.Trim() != "")
                {
                    strSName = txtSName.Text.Trim();
                }

                sp.Spread_All_Clear(ssList);

                List<HIC_JEPSU> list = hicJepsuService.GetItemByJepdateLife(strFrDate, strToDate, strSName, nWrtno);

                nREAD = list.Count;
                progressBar1.Maximum = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    nWrtno = list[i].WRTNO;
                    strOK = "OK";
                    strROWID = hicTitemService.GetItembyWrtNo(nWrtno);

                    if (rdoJob1.Checked == true)
                    {
                        if (!strROWID.IsNullOrEmpty())
                        {
                            strOK = "";
                        }
                    }
                    else
                    {
                        if (strROWID.IsNullOrEmpty())
                        {
                            strOK = "";
                        }
                    }
                    strJong = list[i].GJJONG;

                    if (strOK == "OK")
                    {
                        nRow = nRow + 1;
                        ssList.ActiveSheet.RowCount = nRow;
                        ssList.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].GJJONG;
                        ssList.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].SNAME;
                        ssList.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].WRTNO.ToString();
                        ssList.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].JEPDATE;
                        ssList.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].SEX;
                        ssList.ActiveSheet.Cells[nRow - 1, 5].Text = hb.READ_HIC_AGE_GESAN(clsAES.DeAES(list[i].JUMIN2.Trim())).ToString();

                        ssList.ActiveSheet.Cells[nRow - 1, 6].Text = list[i].PANO.ToString();
                        //strOldcode = strLtdCode;
                    }
                    progressBar1.Value = i + 1;
                }
                ssList.ActiveSheet.RowCount = nRow;
            }
            else if (sender == btnMenuSave)
            {
                eBtnClick(btnOK, new EventArgs());
            }
            else if (sender == btnMenuCancel)
            {
                eBtnClick(btnCancel, new EventArgs());
            }
            else if (sender == btnCancel)
            {
                fn_Screen_Clear();
                fn_Item_Refresh();
            }
            else if (sender == btnOK)
            {
                string strROWID = "";
                long nWRTNO = 0;
                string strCODE = "";
                long nJumsu = 0;
                string strJumsu = "";
                long nOldSelect = 0;
                int strGubun = 0;
                int nCnt = 0;

                nWRTNO = txtWrtNo.Text.To<long>();

                if (nWRTNO == 0 && FnPano == 0)
                {
                    MessageBox.Show("해당환자를 먼저 선택후 저장하십시오", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                for (int i = 0; i < 7; i++)
                {
                    if (tabControl1.Tabs[i].Visible == true)
                    {
                        nCnt++;
                        break;
                    }
                }

                if (nCnt == 0)
                {
                    MessageBox.Show("저장 할 Data가 존재하지 않습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                nCnt = 0;
                for (int i = 0; i < 7; i++)
                {
                    FpSpread SS1 = (Controls.Find("SS1" + i.ToString(), true)[0] as FpSpread);
                    if (SS1.ActiveSheet.NonEmptyRowCount > 0)
                    {
                        nCnt++;
                        break;
                    }
                }

                if (nCnt == 0)
                {
                    MessageBox.Show("저장 할 Data가 존재하지 않습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                clsDB.setBeginTran(clsDB.DbCon);

                for (int i = 10; i <= 16; i++)
                {
                    Panel tabpnl = (Controls.Find("tabpnl" + (i - 10).ToString(), true)[0] as Panel);
                    if (i < 15)
                    {   
                        if (tabpnl.Enabled == false)
                        {
                            //기존자료 삭제
                            result = hicTitemService.DeletebyWrtNoGuybun(nWRTNO, i + 1);

                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                            }
                        }
                    }
                    else
                    {

                        if (tabpnl.Enabled == false)
                        {
                            //기존자료 삭제
                            result = hicTitemService.DeletebyWrtNoGuybun(nWRTNO, i + 3);

                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                            }
                        }
                    }
                }

                for (int j = 10; j <= 16; j++)
                {
                    FpSpread SS1 = (Controls.Find("SS1" + (j - 10).ToString(), true)[0] as FpSpread);
                    for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                    {
                        if (!SS1.ActiveSheet.Cells[i, 5].Text.IsNullOrEmpty())
                        {
                            nOldSelect = long.Parse(SS1.ActiveSheet.Cells[i, 5].Text);
                        }
                        else
                        {
                            nOldSelect = 0;
                        }
                        nJumsu = 0;
                        strJumsu = "";
                        if (j < 15)
                        {
                            strGubun = j + 1;
                        }
                        else
                        {
                            strGubun = j + 3;
                        }

                        if (SS1.ActiveSheet.Cells[i, 1].Text != "" || nOldSelect > 0)
                        {
                            strCODE = SS1.ActiveSheet.Cells[i, 3].Text;
                            if (!SS1.ActiveSheet.Cells[i, 4].Text.IsNullOrEmpty())
                            {
                                nJumsu = long.Parse(SS1.ActiveSheet.Cells[i, 4].Text);
                            }
                            else
                            {
                                nJumsu = 0;
                            }
                            strJumsu = SS1.ActiveSheet.Cells[i, 4].Text;

                            HIC_TITEM list = hicTitemService.GetRowIdbyGubun(strGubun, strCODE, nWRTNO, FnPano);

                            strROWID = "";
                            if (!list.IsNullOrEmpty())
                            {
                                strROWID = list.ROWID;
                            }

                            if (strROWID.IsNullOrEmpty()) 
                            {
                                result = hicTitemService.Insert(nWRTNO, strGubun.ToString(), strCODE, nJumsu, FnPano);

                                if (result < 0)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    MessageBox.Show("HIC_TITEM 저장중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                            }
                            else if (!strROWID.IsNullOrEmpty() && strJumsu.IsNullOrEmpty())
                            {
                                result = hicTitemService.Delete(strROWID);

                                if (result < 0)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    MessageBox.Show("HIC_TITEM 삭제중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                            else
                            {
                                result = hicTitemService.Update(strCODE, nWRTNO, FnPano, nJumsu, strROWID);

                                if (result < 0)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    MessageBox.Show("HIC_TITEM UPDATE중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                        }
                    }

                    //(운동)start
                    if (tabpnl2.Enabled == true && j == 12)
                    {
                        List<HIC_TITEM> list = hicTitemService.GetRowId(nWRTNO, FnPano);

                        if (list.Count == 17)
                        {
                            //1-2
                            result = hicTitemService.UpdateCodePaNobyWrtNoPaNo(string.Format("{0:0}", txtCode1.Text), FnPano, nWRTNO, "13", 901);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //1-3(1)
                            result = hicTitemService.UpdateCodePaNobyWrtNoPaNo(string.Format("{0:0}", txtCode2.Text), FnPano, nWRTNO, "13", 902);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //1-3(2)
                            result = hicTitemService.UpdateCodePaNobyWrtNoPaNo(string.Format("{0:0}", txtCode3.Text), FnPano, nWRTNO, "13", 903);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //1-5
                            result = hicTitemService.UpdateCodePaNobyWrtNoPaNo(string.Format("{0:0}", txtCode4.Text), FnPano, nWRTNO, "13", 904);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //1-6(1)
                            result = hicTitemService.UpdateCodePaNobyWrtNoPaNo(string.Format("{0:0}", txtCode5.Text), FnPano, nWRTNO, "13", 905);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //1-6(2)
                            result = hicTitemService.UpdateCodePaNobyWrtNoPaNo(string.Format("{0:0}", txtCode6.Text), FnPano, nWRTNO, "13", 906);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //2-2
                            result = hicTitemService.UpdateCodePaNobyWrtNoPaNo(string.Format("{0:0}", txtCode7.Text), FnPano, nWRTNO, "13", 907);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //2-3(1)
                            result = hicTitemService.UpdateCodePaNobyWrtNoPaNo(string.Format("{0:0}", txtCode8.Text), FnPano, nWRTNO, "13", 908);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //2-3(2)
                            result = hicTitemService.UpdateCodePaNobyWrtNoPaNo(string.Format("{0:0}", txtCode9.Text), FnPano, nWRTNO, "13", 909);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //3-2
                            result = hicTitemService.UpdateCodePaNobyWrtNoPaNo(string.Format("{0:0}", txtCode10.Text), FnPano, nWRTNO, "13", 910);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //3-3(1)
                            result = hicTitemService.UpdateCodePaNobyWrtNoPaNo(string.Format("{0:0}", txtCode11.Text), FnPano, nWRTNO, "13", 911);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //3-3(2)
                            result = hicTitemService.UpdateCodePaNobyWrtNoPaNo(string.Format("{0:0}", txtCode12.Text), FnPano, nWRTNO, "13", 912);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //3-5
                            result = hicTitemService.UpdateCodePaNobyWrtNoPaNo(string.Format("{0:0}", txtCode13.Text), FnPano, nWRTNO, "13", 913);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //3-6(1)
                            result = hicTitemService.UpdateCodePaNobyWrtNoPaNo(string.Format("{0:0}", txtCode14.Text), FnPano, nWRTNO, "13", 914);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //3-6(2)
                            result = hicTitemService.UpdateCodePaNobyWrtNoPaNo(string.Format("{0:0}", txtCode15.Text), FnPano, nWRTNO, "13", 915);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //4-1(1)
                            result = hicTitemService.UpdateCodePaNobyWrtNoPaNo(string.Format("{0:0}", txtCode16.Text), FnPano, nWRTNO, "13", 916);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //4-1(2)
                            result = hicTitemService.UpdateCodePaNobyWrtNoPaNo(string.Format("{0:0}", txtCode17.Text), FnPano, nWRTNO, "13", 917);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }
                        }
                        else
                        {
                            //기존자료 삭제
                            result = hicTitemService.DeletebyWrtNoPaNo(nWRTNO, FnPano, "", 900);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            for (int i = 1; i <= 17; i++)
                            {
                                TextBox txtCode = (Controls.Find("txtCode" + i.ToString(), true)[0] as TextBox);
                                if (!txtCode.Text.IsNullOrEmpty())
                                {
                                    txtCode.Text = txtCode.Text.Trim();
                                }
                            }

                            //신규자료 입력
                            //1-2
                            result = hicTitemService.Insert(nWRTNO, "13", string.Format("{0:0}", txtCode1.Text), 901, FnPano);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //1-3(1)
                            result = hicTitemService.Insert(nWRTNO, "13", string.Format("{0:0}", txtCode2.Text), 902, FnPano);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //1-3(2)
                            result = hicTitemService.Insert(nWRTNO, "13", string.Format("{0:0}", txtCode3.Text), 903, FnPano);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //1-5
                            result = hicTitemService.Insert(nWRTNO, "13", string.Format("{0:0}", txtCode4.Text), 904, FnPano);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //1-6(1)
                            result = hicTitemService.Insert(nWRTNO, "13", string.Format("{0:0}", txtCode5.Text), 905, FnPano);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //1-6(2)
                            result = hicTitemService.Insert(nWRTNO, "13", string.Format("{0:0}", txtCode6.Text), 906, FnPano);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //2-2
                            result = hicTitemService.Insert(nWRTNO, "13", string.Format("{0:0}", txtCode7.Text), 907, FnPano);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //2-3(1)
                            result = hicTitemService.Insert(nWRTNO, "13", string.Format("{0:0}", txtCode8.Text), 908, FnPano);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //2-3(2)
                            result = hicTitemService.Insert(nWRTNO, "13", string.Format("{0:0}", txtCode9.Text), 909, FnPano);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //3-2
                            result = hicTitemService.Insert(nWRTNO, "13", string.Format("{0:0}", txtCode10.Text), 910, FnPano);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //3-3(1)
                            result = hicTitemService.Insert(nWRTNO, "13", string.Format("{0:0}", txtCode11.Text), 911, FnPano);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //3-3(2)
                            result = hicTitemService.Insert(nWRTNO, "13", string.Format("{0:0}", txtCode12.Text), 912, FnPano);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //3-5
                            result = hicTitemService.Insert(nWRTNO, "13", string.Format("{0:0}", txtCode13.Text), 913, FnPano);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //3-6(1)
                            result = hicTitemService.Insert(nWRTNO, "13", string.Format("{0:0}", txtCode14.Text), 914, FnPano);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //3-6(2)
                            result = hicTitemService.Insert(nWRTNO, "13", string.Format("{0:0}", txtCode15.Text), 915, FnPano);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //4-1(1)
                            result = hicTitemService.Insert(nWRTNO, "13", string.Format("{0:0}", txtCode16.Text), 916, FnPano);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //4-1(2)
                            result = hicTitemService.Insert(nWRTNO, "13", string.Format("{0:0}", txtCode17.Text), 917, FnPano);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }
                        }
                    }

                    //신장: 901 / 체중: 902 / 허리둘레: 903 / 체질량: 904
                    if (tabpnl4.Enabled == true && j == 14)
                    {
                        List<HIC_TITEM> list = hicTitemService.GetRowIdbyGubunWrtNoPaNo("15", 900, nWRTNO, FnPano);

                        if (list.Count == 4)
                        {
                            //신장
                            result = hicTitemService.UpdateCodePaNobyWrtNoPaNo(txtHeight.Text, FnPano, nWRTNO, "15", 901);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //체중
                            result = hicTitemService.UpdateCodePaNobyWrtNoPaNo(txtWeight.Text, FnPano, nWRTNO, "15", 902);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //허리둘레
                            result = hicTitemService.UpdateCodePaNobyWrtNoPaNo(txtWaist.Text, FnPano, nWRTNO, "15", 903);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //체질량
                            result = hicTitemService.UpdateCodePaNobyWrtNoPaNo(txtBmi.Text, FnPano, nWRTNO, "15", 904);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }
                        }
                        else
                        {
                            //기존자료 삭제
                            result = hicTitemService.DeletebyWrtNoPaNo(nWRTNO, FnPano, "15", 900);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }
                            //신규자료 입력
                            //신장
                            result = hicTitemService.Insert(nWRTNO, "15", txtHeight.Text, 901, FnPano);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }
                            //체중
                            result = hicTitemService.Insert(nWRTNO, "15", txtWeight.Text, 902, FnPano);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }
                            //허리둘레
                            result = hicTitemService.Insert(nWRTNO, "15", txtWaist.Text, 903, FnPano);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }
                            //체질량
                            result = hicTitemService.Insert(nWRTNO, "15", txtBmi.Text, 904, FnPano);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }
                        }
                    }
                }

                //1차검진소견요약 UPDATE
                if (chkFirstPan.Checked == true)
                {
                    result = hicResBohum2Service.UpdateTSangdam1byWrtNo(txtFirstSogen.Text, long.Parse(txtWrtNo.Text));
                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                MessageBox.Show("저장 되었습니다!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);

                fn_Item_Refresh();
                fn_Screen_Clear();

                pnlSave.Visible = false;
            }
        }

        void fn_Item_Refresh()
        {
            //생활습관 항목별 조회
            for (int i = 11; i <= 15; i++)
            {
                fn_Tools_Items_Display(i);
                FpSpread SS2 = (Controls.Find("SS2" + (i - 11).ToString(), true)[0] as FpSpread);
                //sp.Spread_Clear(SS2, SS2.ActiveSheet.RowCount, SS2.ActiveSheet.ColumnCount);
                sp.Spread_All_Clear(SS2);
                SS2.ActiveSheet.RowCount = 1;
            }

            //정신건강
            sp.Spread_All_Clear(SS25);
            SS25.ActiveSheet.RowCount = 1;            
            sp.Spread_All_Clear(SS26);
            SS26.ActiveSheet.RowCount = 1;

            fn_Tools_Items_Display_Add(5);
            fn_Tools_Items_Display_Add(6);

            for (int i = 0; i <= 6; i++)
            {   
                if (tabControl1.Tabs[i].Visible == true)
                {
                    tabControl1.SelectedTabIndex = i;
                    switch (i)
                    {
                        case 0:
                            tabControl1.SelectedTab = tab0;
                            break;
                        case 1:
                            tabControl1.SelectedTab = tab1;
                            break;
                        case 2:
                            tabControl1.SelectedTab = tab2;
                            break;
                        case 3:
                            tabControl1.SelectedTab = tab3;
                            break;
                        case 4:
                            tabControl1.SelectedTab = tab4;
                            break;
                        case 5:
                            tabControl1.SelectedTab = tab5;
                            break;
                        case 6:
                            tabControl1.SelectedTab = tab6;
                            break;
                        default:
                            break;
                    }
                    
                    break;
                }
            }
        }

        void fn_Tools_Items_Display(int argGubun)
        {
            int j = 0;
            int nREAD = 0;
            int nRow = 0;
            string strNew = "";
            string strOLD = "";

            for (int i = 0; i <= 30; i++)
            {
                for (int k = 0; k <= 30; k++)
                {
                    FnRowNo[i, k] = 0;
                }
            }
            
            FpSpread SS1 = (Controls.Find("SS1" + (argGubun - 11).ToString(), true)[0] as FpSpread);
            sp.Spread_All_Clear(SS1);

            //코드를 읽어 Sheet에 표시함
            List<HIC_TCODE> list = hicTCodeService.GetItembyGubun(argGubun);

            nREAD = list.Count;
            SS1.ActiveSheet.RowCount = nREAD;
            strOLD = "";

            for (int i = 0; i < nREAD; i++)
            {
                strNew = VB.Left(list[i].CODE, 3);
                if (strOLD != strNew)
                {
                    nRow += 1;
                    if (nRow > SS1.ActiveSheet.RowCount)
                    {
                        SS1.ActiveSheet.RowCount = nRow;
                    }

                    if (list[i].CODE.Trim().Length == 3)
                    {
                        j += 1;
                        FnRowNo[argGubun - 1, j] = nRow;
                        SS1.ActiveSheet.Cells[nRow - 1, 0, nRow - 1, SS1.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(148, 215, 248);
                    }

                    SS1.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].NAME;
                    SS1.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].CODE;
                }
                else
                {
                    nRow += 1;
                    if (nRow > SS1.ActiveSheet.RowCount)
                    {
                        SS1.ActiveSheet.RowCount = nRow;
                    }

                    SS1.ActiveSheet.Cells[nRow - 1, 0].Text = VB.Space(3) + list[i].NAME;
                    SS1.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].JUMSU.ToString();
                    SS1.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].CODE;
                }
                strOLD = strNew;
            }

            //RowHeight 설정
            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                if (!SS1.ActiveSheet.Cells[i, 0].Text.IsNullOrEmpty())
                {
                    Size size = SS1.ActiveSheet.GetPreferredCellSize(i, 0);
                    SS1.ActiveSheet.Rows[i].Height = size.Height;
                }
                else
                {
                    SS1.ActiveSheet.Rows[i].Height = 20;
                }
            }
            
            FpSpread SS2 = (Controls.Find("SS2" + (argGubun - 11).ToString(), true)[0] as FpSpread);
            if (argGubun == 13)
            {   
                SS2.ActiveSheet.ColumnCount = j + 1;
                for (int i = 2; i <= SS2.ActiveSheet.ColumnCount ; i++)
                {
                    SS2.ActiveSheet.Columns.Get(i - 1).Label = i + 3 + "항목";
                }
            }
            else
            {
                SS2.ActiveSheet.ColumnCount = j + 1;
                for (int i = 2; i <= SS2.ActiveSheet.ColumnCount; i++)
                {
                    SS2.ActiveSheet.Columns.Get(i - 1).Label = i - 1 + "항목";
                }
            }
        }

        void fn_Tools_Items_Display_Add(int argGubun)
        {
            int K = 0;
            int j = 0;
            int nREAD = 0;
            int nRow = 0;
            string strNew = "";
            string strOLD = "";
            int nSS_Sheet = 0;
            int nSeqSS = 0;

            for (int i = 0; i <= 30; i++)
            {
                for (int k = 0; k <= 30; k++)
                {
                    FnRowNo[i, k] = 0;
                }
            }

            nSS_Sheet = argGubun + 13;

            if (argGubun == 5)
            {
                nSeqSS = 6;
            }
            else if (argGubun == 6)
            {
                nSeqSS = 5;
            }
            else
            {
                nSeqSS = argGubun;
            }

            if (nSS_Sheet == 18)
            {
                nSS_Sheet = 19;
            }
            else if (nSS_Sheet == 19)
            {
                nSS_Sheet = 18;
            }

            if (nSS_Sheet == 14)
            {
                int i = 0;
            }

            //FpSpread SS1 = (Controls.Find("SS1" + argGubun.ToString(), true)[0] as FpSpread);
            FpSpread SS1 = (Controls.Find("SS1" + nSeqSS.ToString(), true)[0] as FpSpread);
            sp.Spread_All_Clear(SS1);

            //코드를 읽어 Sheet에 표시함
            List <HIC_TCODE> list = hicTCodeService.GetItembyGubun(nSS_Sheet);

            nREAD = list.Count;
            SS1.ActiveSheet.RowCount = nREAD;
            strOLD = "";
            for (int i = 0; i < nREAD; i++)
            {
                strNew = VB.Left(list[i].CODE, 3);
                if (strOLD != strNew)
                {
                    nRow += 1;
                    if (nRow > SS1.ActiveSheet.RowCount)
                    {
                        SS1.ActiveSheet.RowCount = nRow;
                    }

                    if (list[i].CODE.Trim().Length == 3)
                    {
                        j += 1;
                        FnRowNo[argGubun - 1, j] = nRow;
                        SS1.ActiveSheet.Cells[nRow - 1, 0, nRow - 1, SS1.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(148, 215, 248);
                    }
                    SS1.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].NAME;
                    SS1.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].CODE;
                }
                else
                {
                    nRow += 1;
                    if (nRow > SS1.ActiveSheet.RowCount)
                    {
                        SS1.ActiveSheet.RowCount = nRow;
                    }
                    SS1.ActiveSheet.Cells[nRow - 1, 0].Text = VB.Space(3) + list[i].NAME;
                    SS1.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].JUMSU.ToString();
                    SS1.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].CODE;
                }
                strOLD = strNew;
            }

            FpSpread SS2 = (Controls.Find("SS2" + nSeqSS.ToString(), true)[0] as FpSpread);

            //RowHeight 설정
            for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
            {
                if (!SS2.ActiveSheet.Cells[i, 0].Text.IsNullOrEmpty())
                {
                    Size size = SS2.ActiveSheet.GetPreferredCellSize(i, 0);
                    SS2.ActiveSheet.Rows[i].Height = size.Height;
                }
                else
                {
                    SS2.ActiveSheet.Rows[i].Height = 20;
                }
            }

            SS2.ActiveSheet.ColumnCount = j + 1;
            for (int i = 2; i <= SS2.ActiveSheet.ColumnCount; i++)
            {
                SS2.ActiveSheet.Columns.Get(i - 1).Label = i - 1 + "항목";
            }
        }

        void fn_Screen_Clear()
        {
            lblMsg.Text = "";
            txtSName.Text = "";

            tabControl1.Enabled = false;

            for (int i = 0; i <= 6; i++)
            {
                tabControl1.Tabs[i].Visible = false;
                Panel tabpnl = (Controls.Find("tabpnl" + i.ToString(), true)[0] as Panel);
                tabpnl.Enabled = false;
                FpSpread SS1 = (Controls.Find("SS1" + i.ToString(), true)[0] as FpSpread);
                SS1.Enabled = false;
            }

            txtHeight.Text = "";
            txtWeight.Text = "";
            txtWaist.Text = "";
            txtBmi.Text = "";
            txtFirstSogen.Text = "";

            //운동Clear start
            for (int i = 1; i <= 17; i++)
            {
                TextBox txtCode = (Controls.Find("txtCode" + i.ToString(), true)[0] as TextBox);
                txtCode.Text = "";
            }
        }

        void fn_Screen_Display(long argWrtNo)
        {
            int nREAD = 0;
            string strGbn = "";
            string strCODE = "";
            long nJumsu = 0;
            int nIndex = 0;

            List<HIC_TITEM> list = hicTitemService.GetGubunCodeJumsubyWrtNo(argWrtNo, "17", 900, "");

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                strGbn = list[i].GUBUN;
                nIndex = int.Parse(strGbn) - 11;
                strCODE = list[i].CODE;
                nJumsu = list[i].JUMSU;
                FpSpread SS1 = (Controls.Find("SS1" + nIndex.ToString(), true)[0] as FpSpread);
                for (int k = 0; k < SS1.ActiveSheet.RowCount; k++)
                {
                    if (SS1.ActiveSheet.Cells[k, 3].Text == strCODE)
                    {
                        SS1.ActiveSheet.Cells[k, 4].Text = nJumsu.ToString();
                        if (SS1.ActiveSheet.Cells[k, 2].Text == nJumsu.ToString())
                        {
                            SS1.ActiveSheet.Cells[k, 1].Text = "1";
                            SS1.ActiveSheet.Cells[k, 5].Text = "1";
                        }
                        fn_Total_Jemsu_Display(nIndex);
                    }
                }
            }

            //(운동)start
            List<HIC_TITEM> list2 = hicTitemService.GetGubunCodeJumsubyWrtNo(argWrtNo, "13", 900, "EXERCISE");

            nREAD = list2.Count;
            if (nREAD > 0)
            {
                for (int i = 0; i <= 16; i++)
                {
                    nJumsu = list2[i].JUMSU;
                    switch (nJumsu)
                    {
                        case 901:
                            txtCode1.Text = list2[i].CODE;
                            txtCode1.Text = txtCode1.Text.Trim();                            
                            break;
                        case 902:
                            txtCode2.Text = list2[i].CODE;
                            txtCode2.Text = txtCode2.Text.Trim();
                            break;
                        case 903:
                            txtCode3.Text = list2[i].CODE;
                            txtCode3.Text = txtCode3.Text.Trim();
                            break;
                        case 904:
                            txtCode4.Text = list2[i].CODE;
                            txtCode4.Text = txtCode4.Text.Trim();
                            break;
                        case 905:
                            txtCode5.Text = list2[i].CODE;
                            txtCode5.Text = txtCode5.Text.Trim();
                            break;
                        case 906:
                            txtCode6.Text = list2[i].CODE;
                            txtCode6.Text = txtCode6.Text.Trim();
                            break;
                        case 907:
                            txtCode7.Text = list2[i].CODE;
                            txtCode7.Text = txtCode7.Text.Trim();
                            break;
                        case 908:
                            txtCode8.Text = list2[i].CODE;
                            txtCode8.Text = txtCode8.Text.Trim();
                            break;
                        case 909:
                            txtCode9.Text = list2[i].CODE;
                            txtCode9.Text = txtCode9.Text.Trim();
                            break;
                        case 910:
                            txtCode10.Text = list2[i].CODE;
                            txtCode10.Text = txtCode10.Text.Trim();
                            break;
                        case 911:
                            txtCode11.Text = list2[i].CODE;
                            txtCode11.Text = txtCode11.Text.Trim();
                            break;
                        case 912:
                            txtCode12.Text = list2[i].CODE;
                            txtCode12.Text = txtCode12.Text.Trim();
                            break;
                        case 913:
                            txtCode13.Text = list2[i].CODE;
                            txtCode13.Text = txtCode13.Text.Trim();
                            break;
                        case 914:
                            txtCode14.Text = list2[i].CODE;
                            txtCode14.Text = txtCode14.Text.Trim();
                            break;
                        case 915:
                            txtCode15.Text = list2[i].CODE;
                            txtCode15.Text = txtCode15.Text.Trim();
                            break;
                        case 916:
                            txtCode16.Text = list2[i].CODE;
                            txtCode16.Text = txtCode16.Text.Trim();
                            break;
                        case 917:
                            txtCode17.Text = list2[i].CODE;
                            txtCode17.Text = txtCode17.Text.Trim();
                            break;
                        default:
                            break;
                    }
                }
            }

            //(비만)end
            List<HIC_TITEM> list3 = hicTitemService.GetGubunCodeJumsubyWrtNo(argWrtNo, "15", 900, "BIMAN");

            if (list3.Count > 0)
            {
                for (int i = 0; i <= 3; i++)
                {
                    nJumsu = list3[i].JUMSU;
                    switch (nJumsu)
                    {
                        case 901:
                            txtHeight.Text = list3[i].CODE;
                            break;
                        case 902:
                            txtWeight.Text = list3[i].CODE;
                            break;
                        case 903:
                            txtWaist.Text = list3[i].CODE;
                            break;
                        case 904:
                            txtBmi.Text = list3[i].CODE;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        void fn_Screen_Display_ADD(long argWrtno)
        {
            int P = 0;
            int nREAD = 0;
            string strGbn = "";
            string strCODE = "";
            long nJumsu = 0;
            int nIndex = 0;
            int nSS_Sheet = 0;

            nSS_Sheet = 13;

            List<HIC_TITEM> list = hicTitemService.GetGubunCodeJumsubyWrtNo(argWrtno);

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                strGbn = list[i].GUBUN;
                nIndex = int.Parse(strGbn) - nSS_Sheet;
                strCODE = list[i].CODE;
                nJumsu = list[i].JUMSU;
                FpSpread SS1 = (Controls.Find("SS1" + nIndex.ToString(), true)[0] as FpSpread);
                for (int k = 0; k < SS1.ActiveSheet.RowCount; k++)
                {
                    if (SS1.ActiveSheet.Cells[k, 3].Text.Trim() == strCODE.Trim())
                    {
                        SS1.ActiveSheet.Cells[k, 4].Text = nJumsu.ToString();
                        if (SS1.ActiveSheet.Cells[k, 2].Text == nJumsu.ToString())
                        {
                            SS1.ActiveSheet.Cells[k, 1].Text = "1";
                            SS1.ActiveSheet.Cells[k, 5].Text = "1";
                        }
                        fn_Total_Jemsu_Display(nIndex);
                    }
                }
            }
        }

        void fn_Total_Jemsu_Display(int argIndex)
        {
            int j = 0;
            int nCNT = 0;
            long nJemsu = 0;
            long nBJemsu = 0;
            string strCODE = "";
            long[] nTOT = new long[31];

            //누적할 배열을 Clear
            for (int i = 0; i <= 30; i++)
            {
                nTOT[i] = 0;
            }

            FpSpread SS1 = (Controls.Find("SS1" + argIndex.ToString(), true)[0] as FpSpread);
            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                if (!SS1.ActiveSheet.Cells[i, 1].Text.IsNullOrEmpty())
                {
                    nCNT = int.Parse(SS1.ActiveSheet.Cells[i, 1].Text);
                }
                else
                {
                    nCNT = 0;
                }

                if (nCNT > 0)
                {
                    nBJemsu = long.Parse(SS1.ActiveSheet.Cells[i, 2].Text);
                    strCODE = SS1.ActiveSheet.Cells[i, 3].Text;
                    nJemsu = nBJemsu * nCNT;
                    j = int.Parse(VB.Mid(strCODE, 2, 2));
                    nTOT[j + 1] += nJemsu;
                    nTOT[0] += nJemsu;
                }
            }

            //For i = 2 To SS2(ArgIndex).MaxCols
            //    For j = 1 To.MaxRows
            //        .Row = j
            //        If j = FnRowNo(ArgIndex, i - 1) Then
            //           '.Col = 5: .Text = " ( " & nTOT(i) & " 점 ) "
            //        End If
            //    Next j
            //Next i

            FpSpread SS2 = (Controls.Find("SS2" + argIndex.ToString(), true)[0] as FpSpread);
            for (int i = 2; i < SS2.ActiveSheet.RowCount; i++)
            {
                for (int k = 1; k < SS2.ActiveSheet.RowCount; k++)
                {
                    if (k == FnRowNo[argIndex, i - 1])
                    {
                        //SS2.ActiveSheet.Cells[k, 4].Text = " ( " + nTOT[i] + " 점 ) ";
                    }
                }
            }

            for (int i = 2; i <= SS2.ActiveSheet.ColumnCount; i++)
            {
                SS2.ActiveSheet.Cells[0, i - 1].Text = nTOT[i].ToString();
            }
            SS2.ActiveSheet.Cells[0, 0].Text = nTOT[0].ToString();
        }

        void eSpdEditModeOff(object sender, EventArgs e)
        {
            if (sender == SS10)
            {
                fn_Total_Jemsu_Display(0);
            }
            else if (sender == SS11)
            {
                fn_Total_Jemsu_Display(1);
            }
            else if (sender == SS12)
            {
                fn_Total_Jemsu_Display(2);
            }
            else if (sender == SS13)
            {
                fn_Total_Jemsu_Display(3);
            }
            else if (sender == SS14)
            {
                fn_Total_Jemsu_Display(4);
            }
            else if (sender == SS15)
            {
                fn_Total_Jemsu_Display(5);
            }
            else if (sender == SS16)
            {
                fn_Total_Jemsu_Display(6);
            }
            btnOK.Enabled = true;
        }

        void eSpdChange(object sender, ChangeEventArgs e)
        {
            if (sender == SS20 || sender == SS21 || sender == SS22 || sender == SS23 || sender == SS24 || sender == SS25 || sender == SS26)
            {
                btnOK.Enabled = true;
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtBmi || sender == txtCode1 || sender == txtCode2 || sender == txtCode3 || sender == txtCode4 || sender == txtCode5 || sender == txtCode6 ||
                sender == txtCode7 || sender == txtCode8 || sender == txtCode9 || sender == txtCode10 || sender == txtCode11 || sender == txtCode12 || sender == txtCode13 ||
                sender == txtCode14 || sender == txtCode15 || sender == txtCode16 || sender == txtCode17 ||
                sender == txtWeight || sender == txtWaist || sender == txtHeight)
            {
                if (e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");
                }
            }      
            else if (sender == txtPaNo)
            {
                if (e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");
                    fn_Display_Munjin();
                }
            }
            else if (sender == txtWrtNo)
            {
                int nRead = 0;
                long nAge = 0;
                List<string> strGjJong = new List<string>();

                if (e.KeyChar == 13)
                {
                    if (txtWrtNo.Text.IsNullOrEmpty())
                    {
                        return;
                    }

                    nAge = hicJepsuService.GetAgeByWrtno(long.Parse(txtWrtNo.Text));

                    strGjJong.Clear();
                    strGjJong.Add("16");
                    strGjJong.Add("17");
                    strGjJong.Add("18");

                    if (nAge == 70 || nAge == 74)
                    {
                        //신규접수 및 접수수정 자료를 SELECT
                        HIC_JEPSU list = hicJepsuService.GetItembyWrtNoINGjJong(long.Parse(txtWrtNo.Text), strGjJong);

                        if (!list.IsNullOrEmpty())
                        {
                            MessageBox.Show("해당자료가 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        FnAge = list.AGE;
                        FnWRTNO = long.Parse(txtWrtNo.Text);
                        FnPano = list.PANO;
                        FstrJepDate = list.JEPDATE;

                        fn_Display_Munjin();
                    }
                    else
                    {
                        strGjJong.Clear();
                        strGjJong.Add("44");
                        strGjJong.Add("45");
                        strGjJong.Add("46");
                        //신규접수 및 접수수정 자료를 SELECT
                        HIC_JEPSU list = hicJepsuService.GetItembyWrtNoINGjJong(long.Parse(txtWrtNo.Text), strGjJong);

                        if (list.IsNullOrEmpty())
                        {
                            MessageBox.Show("해당자료가 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        FnAge = list.AGE;
                        FnWRTNO = long.Parse(txtWrtNo.Text);
                        FnPano = list.PANO;
                        FstrJepDate = list.JEPDATE;

                        fn_Display_Munjin();
                    }
                }
            }
        }

        void eTxtChanged(object sender, EventArgs e)
        {
            if (sender == txtCode1)
            {
                if (!txtCode1.Text.IsNullOrEmpty())
                {
                    if (int.Parse(txtCode1.Text) >= 8)
                    {
                        MessageBox.Show("입력가능한 숫자가 초과되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                if (!txtCode1.Text.IsNullOrEmpty())
                {
                    if (VB.IsNumeric(txtCode1.Text) == false)
                    {
                        MessageBox.Show("숫자만 입력가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtCode1.Text = "";
                        return;
                    }
                }
            }
            else if (sender == txtCode2)
            {
                if (int.Parse(txtCode2.Text) > 24)
                {
                    MessageBox.Show("입력가능한 숫자가 초과되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else if (sender == txtCode3)
            {
                if (int.Parse(txtCode3.Text) > 60)
                {
                    MessageBox.Show("입력가능한 숫자가 초과되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else if (sender == txtCode4)
            {
                if (int.Parse(txtCode4.Text) > 7)
                {
                    MessageBox.Show("입력가능한 숫자가 초과되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (!txtCode4.Text.IsNullOrEmpty())
                {
                    if (VB.IsNumeric(txtCode4.Text) == false)
                    {
                        MessageBox.Show("숫자만 입력가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtCode4.Text = "";
                        return;
                    }
                }
            }
            else if (sender == txtCode5)
            {
                if (int.Parse(txtCode5.Text) > 24)
                {
                    MessageBox.Show("입력가능한 숫자가 초과되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else if (sender == txtCode6)
            {
                if (int.Parse(txtCode6.Text) > 60)
                {
                    MessageBox.Show("입력가능한 숫자가 초과되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else if (sender == txtCode7)
            {
                if (int.Parse(txtCode7.Text) > 7)
                {
                    MessageBox.Show("입력가능한 숫자가 초과되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (!txtCode7.Text.IsNullOrEmpty())
                {
                    if (VB.IsNumeric(txtCode7.Text) == false)
                    {
                        MessageBox.Show("숫자만 입력가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtCode7.Text = "";
                        return;
                    }
                }
            }
            else if (sender == txtCode8)
            {
                if (int.Parse(txtCode8.Text) > 24)
                {
                    MessageBox.Show("입력가능한 숫자가 초과되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else if (sender == txtCode9)
            {
                if (int.Parse(txtCode9.Text) > 24)
                {
                    MessageBox.Show("입력가능한 숫자가 초과되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else if (sender == txtCode10)
            {
                if (int.Parse(txtCode10.Text) >= 8)
                {
                    MessageBox.Show("입력가능한 숫자가 초과되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (!txtCode10.Text.IsNullOrEmpty())
                {
                    if (VB.IsNumeric(txtCode10.Text) == false)
                    {
                        MessageBox.Show("숫자만 입력가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtCode10.Text = "";
                        return;
                    }
                }
            }
            else if (sender == txtCode11)
            {
                if (int.Parse(txtCode11.Text) > 24)
                {
                    MessageBox.Show("입력가능한 숫자가 초과되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else if (sender == txtCode12)
            {
                if (int.Parse(txtCode12.Text) > 60)
                {
                    MessageBox.Show("입력가능한 숫자가 초과되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else if (sender == txtCode13)
            {
                if (int.Parse(txtCode13.Text) > 7)
                {
                    MessageBox.Show("입력가능한 숫자가 초과되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (!txtCode13.Text.IsNullOrEmpty())
                {
                    if (VB.IsNumeric(txtCode13.Text) == false)
                    {
                        MessageBox.Show("숫자만 입력가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtCode13.Text = "";
                        return;
                    }
                }
            }
            else if (sender == txtCode14)
            {
                if (long.Parse(txtCode14.Text) > 24)
                {
                    MessageBox.Show("입력가능한 숫자가 초과되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else if (sender == txtCode15)
            {
                if (long.Parse(txtCode15.Text) > 60)
                {
                    MessageBox.Show("입력가능한 숫자가 초과되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else if (sender == txtCode16)
            {
                if (long.Parse(txtCode16.Text) > 24)
                {
                    MessageBox.Show("입력가능한 숫자가 초과되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else if (sender == txtCode17)
            {
                if (long.Parse(txtCode17.Text) > 60)
                {
                    MessageBox.Show("입력가능한 숫자가 초과되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS10 || sender == SS11 || sender == SS12 || sender == SS13 || sender == SS14 || sender == SS15 || sender == SS16)
            {
                int nCNT = 0;
                long nBJemsu = 0;
                long nJemsu = 0;
                string strCODE = "";
                string strMCode = "";
                string strJumsu = "";
                int nRow = 0;
                int nCount = 0;

                if (sender == SS10)
                {
                    fn_SS1_Display(SS10, 0, e.Row);
                }
                else if (sender == SS11)
                {
                    fn_SS1_Display(SS11, 1, e.Row);
                }
                else if (sender == SS12)
                {
                    fn_SS1_Display(SS12, 2, e.Row);
                }
                else if (sender == SS13)
                {
                    fn_SS1_Display(SS13, 3, e.Row);
                }
                else if (sender == SS14)
                {
                    fn_SS1_Display(SS14, 4, e.Row);
                }
                else if (sender == SS15)
                {
                    fn_SS1_Display(SS15, 5, e.Row);
                }
                else if (sender == SS16)
                {
                    fn_SS1_Display(SS16, 6, e.Row);
                }
            }
            else if (sender == ssList)
            {
                string strMsg = "";
                string strExCode = "";

                pnlSave.Visible = true;

                txtWrtNo.Text = ssList.ActiveSheet.Cells[e.Row, 2].Text;
                FnWRTNO = long.Parse(ssList.ActiveSheet.Cells[e.Row, 2].Text);
                strMsg = "[성명:" + ssList.ActiveSheet.Cells[e.Row, 1].Text + "]";
                strMsg += " 접수번호:" + ssList.ActiveSheet.Cells[e.Row, 2].Text;
                strMsg += " 접수일자:" + ssList.ActiveSheet.Cells[e.Row, 3].Text;
                FstrJepDate = ssList.ActiveSheet.Cells[e.Row, 3].Text;
                strMsg += " 성별:" + ssList.ActiveSheet.Cells[e.Row, 4].Text;
                FnAge = long.Parse(ssList.ActiveSheet.Cells[e.Row, 5].Text);
                strMsg = strMsg + " 나이:" + FnAge + "세";
                if (!ssList.ActiveSheet.Cells[e.Row, 6].Text.IsNullOrEmpty())
                {
                    FnPano = long.Parse(ssList.ActiveSheet.Cells[e.Row, 6].Text);
                }
                else
                {
                    FnPano = 0;
                }
                lblMsg.Text = strMsg;

                fn_Display_Munjin();                
            }
        }

        void fn_SS1_Display(FpSpread SpdNm, int nInx, int nRow)
        {
            int nCNT = 0;
            long nBJemsu = 0;
            long nJemsu = 0;
            string strCODE = "";
            string strMCode = "";
            string strJumsu = "";
            int nCount = 0;

            if (!SpdNm.ActiveSheet.Cells[nRow, 2].Text.IsNullOrEmpty())
            {
                nBJemsu = long.Parse(SpdNm.ActiveSheet.Cells[nRow, 2].Text);
                strJumsu = SpdNm.ActiveSheet.Cells[nRow, 2].Text;
            }
            else
            {
                nBJemsu = 0;
                strJumsu = "";
            }
            if (strJumsu.IsNullOrEmpty())
            {
                SpdNm.ActiveSheet.Cells[nRow, 1].Text = "";
                return;
            }

            if (SpdNm.ActiveSheet.Cells[nRow, 1].Text.IsNullOrEmpty())
            {
                nCNT = 0;
            }
            else
            {
                nCNT = int.Parse(SpdNm.ActiveSheet.Cells[nRow, 1].Text);
            }

            if (nCNT == 1)
            {
                SpdNm.ActiveSheet.Cells[nRow, 1].Text = "";
            }
            else
            {
                SpdNm.ActiveSheet.Cells[nRow, 1].Text = "1";
                strMCode = SpdNm.ActiveSheet.Cells[nRow, 3].Text;
                strCODE = VB.Left(SpdNm.ActiveSheet.Cells[nRow, 3].Text, 3);

                for (int i = 0; i < SpdNm.ActiveSheet.RowCount; i++)
                {
                    if (VB.Left(SpdNm.ActiveSheet.Cells[i, 3].Text, 3) == strCODE)
                    {
                        if (SpdNm.ActiveSheet.Cells[i, 1].Text != "")
                        {
                            if (SpdNm.ActiveSheet.Cells[i, 3].Text != strMCode)
                            {
                                SpdNm.ActiveSheet.Cells[i, 1].Text = "";
                                SpdNm.ActiveSheet.Cells[i, 4].Text = "";
                            }
                        }
                    }
                }
            }

            if (nCNT == 1)
            {
                SpdNm.ActiveSheet.Cells[nRow, 4].Text = "";
            }
            else
            {
                SpdNm.ActiveSheet.Cells[nRow, 4].Text = (nBJemsu * 1).ToString();
            }

            fn_Total_Jemsu_Display(nInx);
            btnOK.Enabled = true;
        }

        void eChkBoxClick(object sender, EventArgs e)
        {
            if (sender == chkFirstPan)
            {
                if (chkFirstPan.Checked == true)
                {
                    txtFirstSogen.Enabled = true;
                }
                else
                {
                    txtFirstSogen.Enabled = false;
                }
            }
        }
    }
}
