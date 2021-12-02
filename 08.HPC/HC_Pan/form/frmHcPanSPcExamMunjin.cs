using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Pan
/// File Name       : frmHcPanSPcExamMunjin.cs
/// Description     : 특수검진 문진표
/// Author          : 이상훈
/// Create Date     : 2019-12-12
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm특수검진문진_2010.frm(Frm특수검진문진_2010)" />
namespace HC_Pan
{
    public partial class frmHcPanSPcExamMunjin : Form
    {
        HicJepsuService hicJepsuService = null;
        HicResSpecialService hicResSpecialService = null;
        HicResBohum1Service hicResBohum1Service = null;
        HicCodeService hicCodeService = null;
        HicPatientService hicPatientService = null;
        ComHpcLibBService comHpcLibBService = null;
        HicJepsuExjongPatientService hicJepsuExjongPatientService = null;
        HicExjongService hicExjongService = null;

        frmHcCodeHelp FrmHcCodeHelp = null;
        frmHcLtdHelp FrmHcLtdHelp = null;

        HIC_LTD LtdHelpItem = null;
        HIC_CODE CodeHelpItem = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        long FnWRTNO = 0;
        long FnPano = 0;
        string FstrSex = "";
        string FstrUCodes = "";
        string FstrROWID = "";
        string FstrCOMMIT = "";
        string FstrJong = "";
        string FstrYear = "";
        List<string> strExCodes = new List<string>();

        string FstrCode;
        string FstrName;

        public frmHcPanSPcExamMunjin(long nWrtNo)
        {
            InitializeComponent();

            FnWRTNO = nWrtNo;

            SetEvent();
            SetControl();
        }

        public frmHcPanSPcExamMunjin()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicJepsuService = new HicJepsuService();
            hicResSpecialService = new HicResSpecialService();
            hicResBohum1Service = new HicResBohum1Service();
            hicCodeService = new HicCodeService();
            hicPatientService = new HicPatientService();
            comHpcLibBService = new ComHpcLibBService();
            hicJepsuExjongPatientService = new HicJepsuExjongPatientService();
            hicExjongService = new HicExjongService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnJengSang.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnGongjengHelp.Click += new EventHandler(eBtnClick); 
            this.btnMenuSave.Click += new EventHandler(eBtnClick);
            this.btnMenuCancel.Click += new EventHandler(eBtnClick);
            this.btnMenuTempSave.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.rdoHuyu1.KeyPress += new KeyPressEventHandler(eRdoKeyPress);
            this.rdoHuyu2.KeyPress += new KeyPressEventHandler(eRdoKeyPress);
            this.SSJik.ButtonClicked += new EditorNotifyEventHandler(eSpdBtnClick);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SSTMun.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SSMun.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.txtBuse.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtChest.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtDent.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtDentDoct.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtGajok.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtGiinsung.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtGongjeng.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtHead.KeyPress += new KeyPressEventHandler(eTxtKeyPress);

            this.txtHeight.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtHName.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtIpDate.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eTxtKeyPress);

            this.txtJenipDate.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtNeuro.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtPanDrNo.KeyPress += new KeyPressEventHandler(eTxtKeyPress);

            this.txtSabun.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtSkin.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtSyymm1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtSyymm2.KeyPress += new KeyPressEventHandler(eTxtKeyPress);

            this.txtWrtNo.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtWstat.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtMstat.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtYear1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtYear2.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtYear3.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtYear4.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtYear5.KeyPress += new KeyPressEventHandler(eTxtKeyPress);

            this.txtMstat.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtWstat.GotFocus += new EventHandler(eTxtGotFocus); 
            this.txtDentDoct.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtPanDrNo.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtDentDoct.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtDentDoct.GotFocus += new EventHandler(eTxtGotFocus);

            this.txtDentDoct.KeyUp += new KeyEventHandler(eTxtKeyUp);

            this.txtDentDoct.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtGongjeng.LostFocus += new EventHandler(eTxtLostFocus); 
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            string strData = "";

            SSMun_Sheet1.Columns.Get(3).Visible = false;    //코드
            sp.Spread_All_Clear(SSMun);
            
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFrDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-10).ToShortDateString(); 
            dtpToDate.Text = clsPublic.GstrSysDate;

            cboJong.Items.Clear();
            cboJong.Items.Add("**.전체");
            List<HIC_EXJONG> list = hicExjongService.Read_ExJong_Add(true);
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    cboJong.Items.Add(list[i].CODE.Trim() + "." + list[i].NAME);
                }
                cboJong.SelectedIndex = 0;
            }

            cboGbSpc.Items.Clear();
            cboGbSpc.Items.Add("**.전체");
            List<HIC_CODE> list2 = hicCodeService.GetCodeNamebyGubun("54");
            if (list2.Count > 0)
            {
                for (int i = 0; i < list2.Count; i++)
                {
                    cboGbSpc.Items.Add(list2[i].CODE.Trim() + "." + list2[i].NAME);
                }
                cboGbSpc.SelectedIndex = 0;
            }

            eBtnClick(btnSearch, new EventArgs());

            if (!FnWRTNO.IsNullOrEmpty())
            {
                txtWrtNo.Text = FnWRTNO.To<string>();
                fn_Screen_Display();
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                string strList = "";
                string strFrDate = "";
                string strToDate = "";
                string strJob = "";
                string strJong = "";
                long nLtdCode = 0;
                string strOK = "";
                string strSpc = "";
                int nHeight = 0;
                int nRow = 0;

                Cursor.Current = Cursors.WaitCursor;

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;
                if (rdoJob1.Checked == true)
                {
                    strJob = "1";
                }
                else if (rdoJob2.Checked == true)
                {
                    strJob = "2";
                }

                strJong = VB.Left(cboJong.Text, 2);
                nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();

                sp.Spread_All_Clear(ssList);
                Application.DoEvents();

                List<HIC_JEPSU> list = hicJepsuService.GetItembyJepDateLtdCodeJong(strFrDate, strToDate, strJong, nLtdCode, strJob);

                nREAD = list.Count;
                ssList.ActiveSheet.RowCount = nREAD;
                if (nREAD > 0)
                {
                    progressBar1.Maximum = nREAD;
                    for (int i = 0; i < nREAD; i++)
                    {
                        //문진등록 여부를 읽음
                        HIC_RES_SPECIAL list2 = hicResSpecialService.GetItembyWrtNo(list[i].WRTNO);

                        strOK = "";
                        if (!list2.IsNullOrEmpty())
                        {
                            strSpc = list2.GBSPC;
                            if (list2.MUNJINDATE.IsNullOrEmpty())
                            {
                                strOK = "";
                            }
                            else
                            {
                                strOK = "OK";
                            }
                        }

                        if (chkSpc.Checked == true)
                        {
                            strOK = "";
                            if (strSpc.IsNullOrEmpty())
                            {
                                strOK = "OK";
                            }
                        }

                        if (strOK == "OK")
                        {
                            nRow += 1;
                            if (nRow > ssList.ActiveSheet.RowCount)
                            {
                                ssList.ActiveSheet.RowCount = nRow;
                            }
                            ssList.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].SNAME;     //성명
                            ssList.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].GJJONG;    //건진종류
                            ssList.ActiveSheet.Cells[nRow - 1, 2].Text = hb.READ_Ltd_Name(list[i].LTDCODE.To<string>());   
                            ssList.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].WRTNO.To<string>();    //접수번호
                            ssList.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].JEPDATE;               //접수일자
                        }
                        progressBar1.Value = i + 1;
                    }
                }
                ssList.ActiveSheet.RowCount = nRow;

                Cursor.Current = Cursors.Default;
            }
            else if (sender == btnLtdCode)
            {
                string strLtdCode = "";

                if (txtLtdCode.Text.IndexOf(".") > 0)
                {
                    strLtdCode = VB.Pstr(txtLtdCode.Text, ".", 2);
                }
                else
                {
                    strLtdCode = txtLtdCode.Text;
                }

                FrmHcLtdHelp = new frmHcLtdHelp(strLtdCode);
                FrmHcLtdHelp.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(LtdCd_value);
                FrmHcLtdHelp.ShowDialog();
                FrmHcLtdHelp.rSetGstrValue -= new frmHcLtdHelp.SetGstrValue(LtdCd_value);

                if (!LtdHelpItem.IsNullOrEmpty())
                {
                    txtLtdCode.Text = LtdHelpItem.CODE + "." + LtdHelpItem.SANGHO.Trim();
                }
                else
                {
                    txtLtdCode.Text = "";
                }
            }
            else if (sender == btnJengSang)
            {
                FrmHcCodeHelp = new frmHcCodeHelp("84");    //현재증상 및 자타각 증상
                FrmHcCodeHelp.rSetGstrValue += new frmHcCodeHelp.SetGstrValue(Code_value);
                FrmHcCodeHelp.ShowDialog();
                FrmHcCodeHelp.rSetGstrValue -= new frmHcCodeHelp.SetGstrValue(Code_value);

                if (!FstrCode.IsNullOrEmpty())
                {
                    txtJengSang.Text = FstrCode.Trim() + "." + FstrName.Trim();
                }
                else
                {
                    txtJengSang.Text = "";
                }
            }
            else if (sender == btnGongjengHelp)
            {
                FrmHcCodeHelp = new frmHcCodeHelp("A2");    //작업공정
                FrmHcCodeHelp.rSetGstrValue += new frmHcCodeHelp.SetGstrValue(Code_value);
                FrmHcCodeHelp.ShowDialog();
                FrmHcCodeHelp.rSetGstrValue -= new frmHcCodeHelp.SetGstrValue(Code_value);

                if (!FstrCode.IsNullOrEmpty())
                {
                    txtGongjeng.Text = FstrCode.Trim() + "." + FstrName.Trim();
                }
                else
                {
                    txtGongjeng.Text = "";
                }
            }
            else if (sender == btnMenuCancel)
            {
                fn_Screen_Clear();
            }
            else if (sender == btnMenuSave)
            {
                fn_Munjin_DB_Update("저장");
                fn_SpcMunjin_Update();
                fn_SpcCommonMunjin_Update();
                fn_LungCapacityMunjin_Update();

                if (FstrCOMMIT == "OK")
                {
                    fn_Screen_Clear();
                    ssList.Focus();
                }
            }
            else if (sender == btnMenuTempSave)
            {
                fn_Munjin_DB_Update("임시저장");
                fn_SpcMunjin_Update();
                fn_SpcCommonMunjin_Update();

                if (FstrCOMMIT == "OK")
                {
                    fn_Screen_Clear();
                    ssList.Focus();
                }
            }
        }

        private void Code_value(string strCode, string strName)
        {
            FstrCode = strCode;
            FstrName = strName;
        }

        void eRdoKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == rdoHuyu1 || sender == rdoHuyu2)
            {
                SendKeys.Send("{Tab}");
            }
        }

        void eSpdBtnClick(object sender, EditorNotifyEventArgs e)
        {
            if (sender == SSJik)
            {
                if (e.Column != 2) return;

                if (SSJik.ActiveSheet.Cells[e.Row, 2].Text != "True") return;

                FrmHcCodeHelp = new frmHcCodeHelp("51");    //취급물질명
                FrmHcCodeHelp.rSetGstrValue += new frmHcCodeHelp.SetGstrValue(Code_value);
                FrmHcCodeHelp.ShowDialog();
                FrmHcCodeHelp.rSetGstrValue -= new frmHcCodeHelp.SetGstrValue(Code_value);

                if (!FstrCode.IsNullOrEmpty())
                {
                    SSJik.ActiveSheet.Cells[e.Row, 1].Text = FstrCode.Trim();
                    SSJik.ActiveSheet.Cells[e.Row, 2].Text = "";
                    SSJik.ActiveSheet.Cells[e.Row, 3].Text = " " + FstrName;
                }
                else
                {
                    SSJik.ActiveSheet.Cells[e.Row, 1].Text = "";
                    SSJik.ActiveSheet.Cells[e.Row, 2].Text = "";
                    SSJik.ActiveSheet.Cells[e.Row, 3].Text = "";
                }
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            long nWrtNo = 0;

            if (sender == ssList)
            {
                nWrtNo = ssList.ActiveSheet.Cells[e.Row, 3].Text.To<long>();
                FstrJong = ssList.ActiveSheet.Cells[e.Row, 1].Text.Trim();

                fn_Screen_Clear();

                txtWrtNo.Text = nWrtNo.To<string>();

                fn_Screen_Display();
            }
            else if (sender == SSMun)
            {
                if (SSMun.ActiveSheet.Cells[e.Row, 3].Text.IsNullOrEmpty())
                {
                    return;
                }
                SSMun.ActiveSheet.Cells[e.Row, 2].Text = string.Format("{0:#0}", SSMun.ActiveSheet.Cells[e.Row, 2].Text + 1);

                SSMun.ActiveSheet.Cells[e.Row, 0, e.Row, SSMun.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 128);
                SSMun.ActiveSheet.Cells[e.Row, 0, e.Row, SSMun.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
            }
            else if (sender == SSTMun)
            {
                if (SSTMun.ActiveSheet.Cells[e.Row, 5].Text.IsNullOrEmpty()) return;   //코드가 없으면 EXIT
                if (e.Column != 1 && e.Column != 2 && e.Column != 3 && e.Column != 4) return;

                switch (e.Column)
                {
                    case 1:
                        SSTMun.ActiveSheet.Cells[e.Row, 2].Text = "";
                        SSTMun.ActiveSheet.Cells[e.Row, 3].Text = "";
                        SSTMun.ActiveSheet.Cells[e.Row, 4].Text = "";
                        SSTMun.ActiveSheet.Cells[e.Row, 6].Text = "";
                        break;
                    case 2:
                        SSTMun.ActiveSheet.Cells[e.Row, 2].Text = "2";
                        SSTMun.ActiveSheet.Cells[e.Row, 3].Text = "";
                        SSTMun.ActiveSheet.Cells[e.Row, 4].Text = "";
                        SSTMun.ActiveSheet.Cells[e.Row, 6].Text = "2";
                        break;
                    case 3:
                        SSTMun.ActiveSheet.Cells[e.Row, 2].Text = "";
                        SSTMun.ActiveSheet.Cells[e.Row, 3].Text = "1";
                        SSTMun.ActiveSheet.Cells[e.Row, 4].Text = "";
                        SSTMun.ActiveSheet.Cells[e.Row, 6].Text = "1";
                        break;
                    case 4:
                        SSTMun.ActiveSheet.Cells[e.Row, 2].Text = "";
                        SSTMun.ActiveSheet.Cells[e.Row, 3].Text = "";
                        SSTMun.ActiveSheet.Cells[e.Row, 4].Text = "0";
                        SSTMun.ActiveSheet.Cells[e.Row, 6].Text = "0";
                        break;
                    default:
                        break;
                }
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (sender == txtBuse || sender == txtChest || sender == txtDent || sender == txtEtcMsym || sender == txtGajok || 
                    sender == txtGiinsung || sender == txtGongjeng || sender == txtHead || sender == txtHeight || sender == txtHName ||
                    sender == txtIpDate || sender == txtJengSang || sender == txtJenipDate || sender == txtNeuro || sender == txtPanDrNo ||
                    sender == txtSabun || sender == txtSkin || sender == txtWeight || sender == txtWrtNo || sender == txtSyymm1 || sender == txtSyymm2)
                {
                    SendKeys.Send("{Tab}");
                }
                else if (sender == txtLtdCode)
                {
                    if (e.KeyChar == (char)13)
                    {
                        if (txtLtdCode.Text.Length >= 2)
                        {
                            eBtnClick(btnLtdCode, new EventArgs());
                        }
                    }
                }
                else if (sender == txtDentDoct)
                {
                    long nDrNO = 0;
                    string strResCode = "";
                    string strResType = "";

                    nDrNO = hf.B01_GET_Sangdam_DrNo(Keys.Enter);
                    if (nDrNO > 0)
                    {
                        txtDentDoct.Text = nDrNO.To<string>();
                        lblDentDrName.Text = hb.READ_License_DrName(nDrNO);
                        SendKeys.Send("{Tab}");
                    }
                }
                else if (sender == txtWstat)
                {
                    if (!txtWstat.Text.IsNullOrEmpty())
                    {
                        lblWstat.Text = hm.READ_Munjin_Name_New("특수검진", "예아니오", txtWstat.Text.Trim());
                        SendKeys.Send("{Tab}");
                    }
                }
                else if (sender == txtMstat)
                {
                    if (!txtMstat.Text.IsNullOrEmpty())
                    {
                        lblMstat.Text = hm.READ_Munjin_Name_New("특수검진", "예아니오", txtMstat.Text.Trim());
                        SendKeys.Send("{Tab}");
                    }
                }
            }            
        }
        
        void eTxtGotFocus(object sender, EventArgs e)
        {
            if (sender == txtMstat)
            {
                lblMsg.Text = "1.예  2.아니오";
            }
            else if (sender == txtWstat)
            {
                lblMsg.Text = "1.예  2.아니오";
            }
            else if (sender == txtDentDoct)
            {
                lblMsg.Text = clsHcVariable.B01_SANGDAM_DRLIST;
            }
            else if (sender == txtPanDrNo)
            {
                lblMsg.Text = clsHcVariable.B01_SANGDAM_DRLIST;
            }
        }

        void eTxtKeyUp(object sender, KeyEventArgs e)
        {
            long nDrno = 0;
            string strResCode = "";
            string strResType = "";

            nDrno = hf.B01_GET_Sangdam_DrNo(e.KeyCode);

            if (nDrno > 0)
            {
                txtDentDoct.Text = nDrno.To<string>();
                lblDentDrName.Text = hb.READ_License_DrName(nDrno);
                SendKeys.Send("{Tab}");
            }
        }

        void eTxtLostFocus(object sender, EventArgs e)
        {
            if (sender == txtDentDoct)
            {
                lblDentDrName.Text = hb.READ_License_DrName(txtDentDoct.Text.To<long>());
                SendKeys.Send("{Tab}");
            }
            else if (sender == txtGongjeng)
            {
                txtGongjeng.Text = hb.READ_HIC_CODE("A2", txtGongjeng.Text.Trim());
            }
        }

        void fn_Screen_Display_SpcMunjin(string ArgUCodes)
        {
            int nRow = 0;
            int nREAD = 0;
            List<string> strCodes = new List<string>();
            string strGCode = "";
            string strCODE = "";
            string strData = "";
            string strRes = "";
            string strTitle = "";
            const string strMGubun = "ABCDEFGHIJKLMNO";
            bool[] bMun = new bool[15];
            string strTempMGubun = "";

            FarPoint.Win.Spread.CellType.TextCellType txt = new FarPoint.Win.Spread.CellType.TextCellType();

            if (ArgUCodes.IsNullOrEmpty())
            {
                SSMun.ActiveSheet.RowCount = 0;
                return;
            }
            SSMun.ActiveSheet.RowCount = 0;

            if (ArgUCodes == "ZZZ")
            {
                return;
            }

            //항목별 문진종류 여부 False SET
            for (int i = 0; i < 15; i++)
            {
                bMun[i] = false;
            }

            //유해인자를 IN Type로 변환
            strCodes.Clear();
            for (int i = 0; i < VB.L(ArgUCodes, ","); i++)
            {
                strCodes.Add(strExCodes[i]);
            }

            //해당 유해인자의 입력할 문진표 종류를 읽음
            List<HIC_CODE> list = hicCodeService.GetCodeGCodebyCode(strCodes);

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                strCODE = list[i].CODE;
                strGCode = list[i].GCODE;
                switch (strGCode)
                {
                    case "01":
                        bMun[0] = true; //A:소음
                        break;
                    case "03":
                    case "04":
                    case "05":
                        bMun[1] = true; //B:분진
                        break;
                    case "13":
                        bMun[2] = true; //C:진동
                        break;
                    case "06":
                        bMun[3] = true; //D:유기용제
                        break;
                    case "07":
                        bMun[4] = true; //E:연,4알킬연
                        break;
                    case "08":
                        bMun[5] = true; //F:수은
                        break;
                    case "14":
                        if (strCODE == "M00")
                        {
                            bMun[13] = true;    //N:전리방사선
                        }
                        else
                        {
                            bMun[6] = true;     //G:비전리방사선
                        }
                        break;
                    case "09":
                        bMun[7] = true;         //H:크롬
                        break;
                    case "11":
                        if (strCODE == "F10")
                        {
                            bMun[8] = true;     //I:특정화학물질I류
                        }
                        else
                        {
                            bMun[10] = true;    //K:특정화학물질II,III류
                        }
                        break;
                    case "10":
                        bMun[9] = true;         //J:카드뮴
                        break;
                    case "12":
                        bMun[11] = true;         //망간
                        break;
                    case "02":
                        bMun[12] = true;         //M:이상기압
                        break;
                    case "17":
                        bMun[14] = true;         //휘발성콜타르피치
                        break;
                    default:
                        break;
                }
            }

            //기존의 문진결과를 읽음
            HIC_RES_SPECIAL list2 = hicResSpecialService.GetItembyWrtNo(txtWrtNo.Text.To<long>());

            //입력할 문진표를 Display
            nRow = 0;
            for (int i = 0; i < 15; i++)
            {
                if (bMun[i] == true)
                {
                    switch (i)
                    {
                        case 0:
                            strTitle = "▶소음";
                            break;
                        case 1:
                            strTitle = "▶분진";
                            break;
                        case 2:
                            strTitle = "▶진동";
                            break;
                        case 3:
                            strTitle = "▶유기용제";
                            break;
                        case 4:
                            strTitle = "▶연";
                            break;
                        case 5:
                            strTitle = "▶수은";
                            break;
                        case 6:
                            strTitle = "▶비전리방사선";
                            break;
                        case 7:
                            strTitle = "▶크롬";
                            break;
                        case 8:
                            strTitle = "▶특정화학물질1류";
                            break;
                        case 9:
                            strTitle = "▶카드뮴";
                            break;
                        case 10:
                            strTitle = "▶산,알카리,가스";
                            break;
                        case 11:
                            strTitle = "▶망간";
                            break;
                        case 12:
                            strTitle = "▶이상기압";
                            break;
                        case 13:
                            strTitle = "▶전리방사선";
                            break;
                        case 14:
                            strTitle = "▶휘발성콜타르피치";
                            break;
                        default:
                            break;
                    }

                    nRow += 1;
                    if (nRow > SSMun.ActiveSheet.RowCount)
                    {
                        SSMun.ActiveSheet.RowCount = nRow;
                    }
                    //칼럼의 Type을 변경함
                    SSMun.ActiveSheet.Cells[i, 0, i, 3].CellType = txt;
                    SSMun.ActiveSheet.Cells[i, 0, i, 3].Locked = true;
                    SSMun.ActiveSheet.Cells[i, 0, i, 3].HorizontalAlignment = CellHorizontalAlignment.Left;
                    SSMun.ActiveSheet.Cells[i, 0, i, 3].VerticalAlignment = CellVerticalAlignment.Center;
                    SSMun.ActiveSheet.Cells[i, 0, i, 3].Text = "";

                    SSMun.ActiveSheet.Cells[i, 0].Text = " " + strTitle;

                    //결과를 변수에 저장
                    switch (i)
                    {
                        case 0:
                            strRes = list2.MUNJIN_A;
                            strTempMGubun = VB.Mid(strMGubun, 0, 1);
                            break;
                        case 1:
                            strRes = list2.MUNJIN_B;
                            strTempMGubun = VB.Mid(strMGubun, 1, 1);
                            break;
                        case 2:
                            strRes = list2.MUNJIN_C;
                            strTempMGubun = VB.Mid(strMGubun, 2, 1);
                            break;
                        case 3:
                            strRes = list2.MUNJIN_D;
                            strTempMGubun = VB.Mid(strMGubun, 3, 1);
                            break;
                        case 4:
                            strRes = list2.MUNJIN_E;
                            strTempMGubun = VB.Mid(strMGubun, 4, 1);
                            break;
                        case 5:
                            strRes = list2.MUNJIN_F;
                            strTempMGubun = VB.Mid(strMGubun, 5, 1);
                            break;
                        case 6:
                            strRes = list2.MUNJIN_G;
                            strTempMGubun = VB.Mid(strMGubun, 6, 1);
                            break;
                        case 7:
                            strRes = list2.MUNJIN_H;
                            strTempMGubun = VB.Mid(strMGubun, 7, 1);
                            break;
                        case 8:
                            strRes = list2.MUNJIN_I;
                            strTempMGubun = VB.Mid(strMGubun, 8, 1);
                            break;
                        case 9:
                            strRes = list2.MUNJIN_J;
                            strTempMGubun = VB.Mid(strMGubun, 9, 1);
                            break;
                        case 10:
                            strRes = list2.MUNJIN_K;
                            strTempMGubun = VB.Mid(strMGubun, 10, 1);
                            break;
                        case 11:
                            strRes = list2.MUNJIN_L;
                            strTempMGubun = VB.Mid(strMGubun, 11, 1);
                            break;
                        case 12:
                            strRes = list2.MUNJIN_M;
                            strTempMGubun = VB.Mid(strMGubun, 12, 1);
                            break;
                        case 13:
                            strRes = list2.MUNJIN_N;
                            strTempMGubun = VB.Mid(strMGubun, 13, 1);
                            break;
                        case 14:
                            //strRes = list2.MUNJIN_O;
                            strTempMGubun = VB.Mid(strMGubun, 14, 1);
                            break;
                        default:
                            break;
                    }

                    List<HIC_CODE> list3 = hicCodeService.GetCodeNamebyCode(strTempMGubun);

                    SSMun.ActiveSheet.RowCount = list3.Count;
                    for (int j = 0; j < list3.Count; j++)
                    {
                        strCODE = list3[i].CODE;
                        strData = strTempMGubun;
                        strData = VB.Left(strData + VB.Space(100), 100);
                        strRes = VB.Mid(strData, j * 3 + 1, 3);

                        nRow += 1;
                        if (nRow > SSMun.ActiveSheet.RowCount)
                        {
                            SSMun.ActiveSheet.RowCount = nRow;
                        }
                        SSMun.ActiveSheet.Cells[j, 0].Text = " " + list3[j].NAME;
                        SSMun.ActiveSheet.Cells[j, 5].Text = strCODE;
                        strTitle = "";
                        SSMun.ActiveSheet.Cells[j, 1].Text = "";
                        if (!strRes.IsNullOrEmpty())
                        {
                            if (VB.Left(strRes, 1) == "M")
                            {
                                SSMun.ActiveSheet.Cells[j, 1].Text = "True";
                                SSMun.ActiveSheet.Cells[i, 2].Text = VB.Right(strRes, 2);
                                //해당줄에 바탕색을 변경함
                                SSMun.ActiveSheet.Cells[j, 0, j, SSMun.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                                SSMun.ActiveSheet.Cells[j, 0, j, SSMun.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 128);
                            }
                        }
                    }
                }
            }
            //야간작업 문진표
            fn_Screen_Munjin_Night();
        }

        /// <summary>
        /// 야간작업 문진표
        /// </summary>
        void fn_Screen_Munjin_Night()
        {
            int Inx = 0;
            int nREAD = 0;
            string strChasu = "";
            string strMsg = "";
            string strChar = "";
            string strDAT1 = "";
            long nJemsu1 = 0;
            string strPAN1 = "";
            string strDAT2 = "";
            long nJemsu2 = 0;
            string strPAN2 = "";
            string strDAT3 = "";
            long nJemsu3 = 0;
            string strPAN3 = "";
            long nWRTNO = 0;

            txtSum1.Text = ""; lblPan1.Text = "";
            txtSum2.Text = ""; lblPan2.Text = "";
            txtSum3.Text = ""; lblPan3.Text = "";
            nWRTNO = txtWrtNo.Text.To<long>();

            List<COMHPC> list = comHpcLibBService.GetMunjinNightbyWrtNo(nWRTNO);

            //야간작업 문진표를 읽음
            if (list.IsNullOrEmpty())
            {
                tabControl2.Visible = false;
                tab4.Visible = false;
                return;
            }

            tab4.Visible = true;
            tabControl2.Visible = true;
            tabControl2.TabIndex = 0;

            for (int i = 0; i < nREAD; i++)
            {
                if (!list[i].ITEM1_DATA.IsNullOrEmpty())
                {
                    strDAT1 = list[i].ITEM1_DATA;
                    nJemsu1 = list[i].ITEM1_JEMSU;
                    strPAN1 = list[i].ITEM1_PANJENG;
                    for (int j = 1; j <= 7; j++)
                    {
                        strChar = VB.Mid(strDAT1, i, 1);
                        SS1.ActiveSheet.Cells[i, 1].Text = strChar;
                        if (string.Compare(strChar, "0") > 0)
                        {
                            SS1.ActiveSheet.Cells[i, 1].Text = (strChar.To<long>() - 1).To<string>();
                            SS1.ActiveSheet.Cells[i, 1].Text = hf.Munjin_Night_Value1(i, strChar);
                        }
                    }
                    txtSum1.Text = nJemsu1.To<string>();
                    lblPan1.Text = hf.Munjin_Night_Panjeng("1", strPAN1);
                }
                else if (!list[i].ITEM2_DATA.IsNullOrEmpty())
                {
                    //수면의 질
                    strDAT2 = list[i].ITEM2_DATA;
                    nJemsu2 = list[i].ITEM2_JEMSU;
                    strPAN2 = list[i].ITEM2_PANJENG;
                    for (int j = 1; j <= 18; j++)
                    {
                        strChar = VB.Pstr(strDAT2, ".", j);
                        SS2.ActiveSheet.Cells[i, 1].Text = strChar;
                        strChar = VB.Pstr(strDAT2, ",", i);
                        if (string.Compare(strChar, "0") > 0)
                        {
                            SS2.ActiveSheet.Cells[i, 2].Text = (strChar.To<long>() - 1).To<string>();
                            SS2.ActiveSheet.Cells[i, 3].Text = hf.Munjin_Night_Value1(i, strChar);
                        }
                    }
                    txtSum2.Text = nJemsu2.To<string>();
                    lblPan2.Text = hf.Munjin_Night_Panjeng("2", strPAN2);
                    //주간졸림증
                    strDAT3 = list[i].ITEM3_DATA;
                    nJemsu3 = list[i].ITEM3_JEMSU;
                    strPAN3 = list[i].ITEM3_PANJENG;
                    for (int j = 1; j <= 8; j++)
                    {
                        strChar = VB.Mid(strDAT3, i, 1);
                        SS3.ActiveSheet.Cells[i, 1].Text = strChar;
                        if (string.Compare(strChar, "0") > 0)
                        {
                            SS3.ActiveSheet.Cells[i, 2].Text = (strChar.To<long>() - 1).To<string>();
                            SS3.ActiveSheet.Cells[i, 3].Text = hf.Munjin_Night_Value1(i, strChar);
                        }
                    }
                    txtSum3.Text = nJemsu3.To<string>();                    
                    lblPan3.Text = hf.Munjin_Night_Panjeng("3", strPAN3);
                }
            }
        }

        void fn_Munjin_DB_Update(string ArgGbn)
        {
            int nUCodeCNT = 0;
            int nPGigan_YY = 0;
            int nPGigan_MM = 0;
            string strOldGong1 = "";
            string strOldMCode1 = "";
            string strOldYear1 = "";
            string strPGigan1 = "";
            string strOldDayTime1 = "";
            string strOldGong2 = "";
            string strOldMCode2 = "";
            string strOldYear2 = "";
            string strPGigan2 = "";
            string strOldDayTime2 = "";
            string strOldGong3 = "";
            string strOldMCode3 = "";
            string strOldYear3 = "";
            string strPGigan3 = "";
            string strOldDayTime3 = "";
            string[] strHabit = new string[5];
            string strGbHuyu = "";
            string strGbSangtae = "";
            string strGbSuchup = "";

            string strGbOHMS = "";
            string strCODE = "";
            string strGbSPC = "";
            string strROWID = "";
            long nJohapAmt = 0;
            string strWstat = "";
            string strMstat = "";

            int result = 0;

            if (txtGongjeng.Text.IsNullOrEmpty())
            {
                MessageBox.Show("현작업공정 코드가 맞지않습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (txtIpDate.Text.IsNullOrEmpty())
            {
                MessageBox.Show("입사입자 공란", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (txtJenipDate.Text.IsNullOrEmpty())
            {
                MessageBox.Show("현직전입입자 공란", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!txtDent.Text.IsNullOrEmpty() && lblDentDrName.Text.IsNullOrEmpty())
            {
                MessageBox.Show("치과의사 면허번호가 공란 또는 오류입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            strGbSPC = VB.Left(cboGbSpc.Text, 2);
            if (strGbSPC.IsNullOrEmpty())
            {
                MessageBox.Show("특검종류가 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (FstrJong == "23")
            {
                if (strGbSPC != "02")
                {
                    MessageBox.Show("특검종류가 02.특검이 아님.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (txtJenipDate.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("현직전입입자 공란", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            if (FstrJong == "21" && strGbSPC.IsNullOrEmpty() )
            {
                MessageBox.Show("채용검진인데, 특검종류가 공백입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (txtPanDrNo.Text.Trim().To<long>() == 0 || lblDrName.Text.IsNullOrEmpty())
            {
                MessageBox.Show("진찰의사 오류입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            txtRemark.Text = txtRemark.Text.Replace("'", "`");

            //수첩소지
            strGbSuchup = "N";
            if (chkSuChup.Checked == true)
            {
                strGbSuchup = "Y";
            }

            strGbOHMS = "N";
            if (chkOHMS.Checked == true)
            {
                strGbOHMS = "Y";
            }

            //과거직력
            strOldGong1 = SSJik.ActiveSheet.Cells[0, 0].Text.Trim();
            strOldMCode1 = SSJik.ActiveSheet.Cells[0, 1].Text.Trim();
            strOldYear1 = SSJik.ActiveSheet.Cells[0, 4].Text.Trim();
            strPGigan1 = SSJik.ActiveSheet.Cells[0, 5].Text.Trim();
            strOldDayTime1 = SSJik.ActiveSheet.Cells[0, 6].Text.Trim();

            strOldGong2 = SSJik.ActiveSheet.Cells[1, 0].Text.Trim();
            strOldMCode2 = SSJik.ActiveSheet.Cells[1, 1].Text.Trim();
            strOldYear2 = SSJik.ActiveSheet.Cells[1, 4].Text.Trim();
            strPGigan2 = SSJik.ActiveSheet.Cells[1, 5].Text.Trim();
            strOldDayTime2 = SSJik.ActiveSheet.Cells[1, 6].Text.Trim();

            strOldGong3 = SSJik.ActiveSheet.Cells[2, 0].Text.Trim();
            strOldMCode3 = SSJik.ActiveSheet.Cells[2, 1].Text.Trim();
            strOldYear3 = SSJik.ActiveSheet.Cells[2, 4].Text.Trim();
            strPGigan3 = SSJik.ActiveSheet.Cells[2, 5].Text.Trim();
            strOldDayTime3 = SSJik.ActiveSheet.Cells[2, 6].Text.Trim();

            if (FstrJong == "21" || FstrJong == "32")
            {
                if (txtGongjeng.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("현작업공정이 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            //자료에 오류가 있는지 점검
            if (chkOHMS.Checked == true && ArgGbn == "저장")
            {
                //과거직력 점검
                if (!strOldMCode1.IsNullOrEmpty() && (strOldYear1.IsNullOrEmpty() || strOldDayTime1.IsNullOrEmpty()))
                {
                    MessageBox.Show("과거직력① 오류", "오류");
                    return;
                }
                if (!strOldMCode2.IsNullOrEmpty() && (strOldYear2.IsNullOrEmpty() || strOldDayTime2.IsNullOrEmpty()))
                {
                    MessageBox.Show("과거직력② 오류", "오류");
                    return;
                }
                if (!strOldMCode3.IsNullOrEmpty() && (strOldYear3.IsNullOrEmpty() || strOldDayTime3.IsNullOrEmpty()))
                {
                    MessageBox.Show("과거직력③ 오류", "오류");
                    return;
                }
            }

            //생활습관
            strHabit[0] = "N";
            if (chkSang1.Checked == true)
            {
                strHabit[0] = "Y";
            }
            strHabit[1] = "N";
            if (chkSang2.Checked == true)
            {
                strHabit[1] = "Y";
            }
            strHabit[2] = "N";
            if (chkSang3.Checked == true)
            {
                strHabit[2] = "Y";
            }
            strHabit[3] = "N";
            if (chkSang4.Checked == true)
            {
                strHabit[3] = "Y";
            }
            strHabit[4] = "N";
            if (chkSang5.Checked == true)
            {
                strHabit[4] = "Y";
            }

            //외상및휴유증 점검
            strGbHuyu = "";
            if (rdoHuyu1.Checked == true) strGbHuyu = "Y";
            if (rdoHuyu2.Checked == true) strGbHuyu = "N";

            //일반상태
            strGbSangtae = "";
            if (rdoSang1.Checked == true) strGbSangtae = "1";
            if (rdoSang2.Checked == true) strGbSangtae = "2";
            if (rdoSang3.Checked == true) strGbSangtae = "3";

            if (ArgGbn == "저장")
            {
                if (!txtDent.Text.IsNullOrEmpty() && txtDentDoct.Text.To<long>() == 0)
                {
                    MessageBox.Show("치과의사 면허번호 누락입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (txtDentDoct.Text.To<long>() > 0 && lblDentDrName.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("치과의사 면허번호가 오류입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            //치과 소견
            for (int i = 0; i < VB.I(FstrUCodes, ","); i++)
            {
                if (VB.Pstr(FstrUCodes, ",", i) == "F23" || VB.Pstr(FstrUCodes, ",", i) == "F29" || VB.Pstr(FstrUCodes, ",", i) == "F41" || VB.Pstr(FstrUCodes, ",", i) == "F11" || VB.Pstr(FstrUCodes, ",", i) == "F20")
                {
                    if (txtDent.Text.IsNullOrEmpty())
                    {
                        MessageBox.Show("치과소견 입력 오류입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            //작업중 건강문제
            strWstat = txtWstat.Text.Trim();
            //작업중 취급물질로 건강문제
            strMstat = txtMstat.Text.Trim();
            //취급물질 건수
            nUCodeCNT = 0;
            if (!FstrUCodes.IsNullOrEmpty())
            {
                nUCodeCNT = VB.I(FstrUCodes, ",") - 1;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            HIC_RES_SPECIAL item = new HIC_RES_SPECIAL();

            item.GBOHMS = strGbOHMS;
            item.GBSPC = strGbSPC;
            item.UCODECNT = nUCodeCNT;
            item.UCODENAME = FstrUCodes;
            item.SABUN = txtSabun.Text.Trim();
            item.BUSE = txtBuse.Text.Trim();
            item.HNAME = txtHName.Text.Trim();
            item.IPSADATE = txtIpDate.Text;
            item.SUCHUPYN = strGbSuchup;
            item.GONGJENG = VB.Pstr(txtGongjeng.Text.Trim(), ".", 1);
            item.JENIPDATE = txtJenipDate.Text;
            item.PGIGAN_YY = nPGigan_YY;
            item.PGIGAN_MM = nPGigan_MM;
            item.OLDGONG1 = strOldGong1;
            item.OLDMCODE1 = strOldMCode1;
            item.OLDYEAR1 = strOldYear1.To<long>();
            item.OLDPGIGAN1 = strPGigan1;
            item.OLDDAYTIME1 = strOldDayTime1.To<long>();
            item.OLDGONG2 = strOldGong2;
            item.OLDMCODE2 = strOldMCode2;
            item.OLDYEAR2 = strOldYear2.To<long>();
            item.OLDPGIGAN2 = strPGigan2;
            item.OLDDAYTIME2 = strOldDayTime2.To<long>();
            item.OLDGONG3 = strOldGong3;
            item.OLDMCODE3 = strOldMCode3;
            item.OLDYEAR3 = strOldYear3.To<long>();
            item.OLDPGIGAN3 = strPGigan3;
            item.OLDDAYTIME3 = strOldDayTime3.To<long>();
            item.HABIT1 = strHabit[0];
            item.HABIT2 = strHabit[1];
            item.HABIT3 = strHabit[2];
            item.HABIT4 = strHabit[3];
            item.HABIT5 = strHabit[4];
            item.GBHUYU = strGbHuyu;
            item.GBSANGTAE = strGbSangtae;
            item.OLDMYEAR1 = txtYear1.Text;
            item.OLDMYEAR2 = txtYear2.Text;
            item.OLDMYEAR3 = txtYear3.Text;
            item.OLDMYEAR4 = txtYear4.Text;
            item.OLDMYEAR5 = txtYear5.Text;
            item.OLDETCMSYM = txtEtcMsym.Text.Trim();
            item.MUN_GAJOK = txtGajok.Text.Trim();
            item.MUN_GIINSUNG = txtGiinsung.Text.Trim();
            item.JIN_NEURO = txtNeuro.Text.Trim();
            item.JIN_HEAD = txtHead.Text.Trim();
            item.JIN_SKIN = txtSkin.Text.Trim();
            item.JIN_CHEST = txtChest.Text.Trim();
            item.JENGSANG = VB.Pstr(txtJengSang.Text.Trim(), ".", 1);
            item.DENTSOGEN = txtDent.Text.Trim();
            item.DENTDOCT = txtDentDoct.Text.To<long>();
            item.JINDRNO = txtPanDrNo.Text.To<long>();
            item.HSTAT = strWstat;
            item.MCODE_STAT = strMstat;
            item.JINREMARK = txtRemark.Text.Trim();
            item.WRTNO = FnWRTNO;

            //변경한 자료를 DB에 UPDATE
            result = hicResSpecialService.UpdateAllbyWrtNo(item, ArgGbn);

            if (result < 0)
            {
                MessageBox.Show("문진내용 DB에 UPDATE시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }

            HIC_JEPSU item2 = new HIC_JEPSU();

            item2.SABUN = txtSabun.Text.Trim();
            item2.IPSADATE = txtIpDate.Text;
            item2.BUSENAME = txtBuse.Text.Trim();
            item2.BUSEIPSA = txtJenipDate.Text;
            item2.GBSUCHEP = strGbSuchup;
            item2.WRTNO = FnWRTNO;

            //접수마스타에 인적사항 변경내역 Update
            result = hicJepsuService.UpdatebyWrtNo(item2, ArgGbn);

            if (result < 0)
            {
                MessageBox.Show("접수마스타 DB에 UPDATE시 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }

            HIC_PATIENT item3 = new HIC_PATIENT();

            item3.GONGJENG = VB.Pstr(txtGongjeng.Text, ".", 1);
            item3.SABUN = txtSabun.Text;
            item3.IPSADATE = txtIpDate.Text;
            item3.BUSENAME = txtBuse.Text;
            item3.BUSEIPSA = txtJenipDate.Text;
            item3.PANO = FnPano;

            //환자마스타에 인적사항 변경내역 Update
            result = hicPatientService.UpdatebyPaNo(item3, strGbSuchup);

            if (result < 0)
            {
                MessageBox.Show("환자마스타 DB에 UPDATE시 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }
            clsDB.setCommitTran(clsDB.DbCon);
            FstrCOMMIT = "OK";
        }

        void fn_SpcMunjin_Update()
        {
            int j = 0;
            int K = 0;
            int No = 0;
            string[] strMunjin = new string[14];
            string strRes = "";
            string strTemp = "";
            string strCODE = "";
            int result = 0;

            if (FstrCOMMIT != "OK")
            {
                return;
            }

            //결과에 공란을 채움            
            for (int i = 0; i < 14; i++)
            {
                strMunjin[i] = "";
            }

            for (int i = 0; i < SSMun.ActiveSheet.RowCount; i++)
            {
                if (VB.Left(SSMun.ActiveSheet.Cells[i, 0].Text.Trim(), 2) != "▶")
                {
                    if (SSMun.ActiveSheet.Cells[i, 2].Text.To<long>() > 0)
                    {
                        strRes = "W";
                        if (SSMun.ActiveSheet.Cells[i, 1].Text == "1")
                        {
                            strRes = "M";
                        }
                        strRes += string.Format("{0:00}", SSMun.ActiveSheet.Cells[i, 2].Text);
                        strCODE = SSMun.ActiveSheet.Cells[i, 3].Text.Trim();
                        //코드값으로 배열항목 지정
                        switch (VB.Left(strCODE, 1))
                        {
                            case "A":
                                No = 0;
                                break;
                            case "B":
                                No = 1;
                                break;
                            case "C":
                                No = 2;
                                break;
                            case "D":
                                No = 3;
                                break;
                            case "E":
                                No = 4;
                                break;
                            case "F":
                                No = 5;
                                break;
                            case "G":
                                No = 6;
                                break;
                            case "H":
                                No = 7;
                                break;
                            case "I":
                                No = 8;
                                break;
                            case "J":
                                No = 9;
                                break;
                            case "K":
                                No = 10;
                                break;
                            case "L":
                                No = 11;
                                break;
                            case "M":
                                No = 12;
                                break;
                            case "N":
                                No = 13;
                                break;
                            default:
                                break;
                        }
                        j = VB.Right(strCODE, 2).To<int>();
                        strMunjin[No] += strRes;
                    }
                    else
                    {
                        strMunjin[No] += "   ";
                    }
                }
                else if (VB.Left(SSMun.ActiveSheet.Cells[i, 2].Text.Trim(), 2) == "▶")
                {
                    strRes += string.Format("{0:00}", SSMun.ActiveSheet.Cells[i, 2].Text);
                    strCODE = SSMun.ActiveSheet.Cells[i, 3].Text.Trim();
                    //코드값으로 배열항목 지정
                    switch (VB.Left(strCODE, 1))
                    {
                        case "A":
                            No = 0;
                            break;
                        case "B":
                            No = 1;
                            break;
                        case "C":
                            No = 2;
                            break;
                        case "D":
                            No = 3;
                            break;
                        case "E":
                            No = 4;
                            break;
                        case "F":
                            No = 5;
                            break;
                        case "G":
                            No = 6;
                            break;
                        case "H":
                            No = 7;
                            break;
                        case "I":
                            No = 8;
                            break;
                        case "J":
                            No = 9;
                            break;
                        case "K":
                            No = 10;
                            break;
                        case "L":
                            No = 11;
                            break;
                        case "M":
                            No = 12;
                            break;
                        case "N":
                            No = 13;
                            break;
                        default:
                            break;
                    }                    
                }
            }

            clsDB.setBeginTran(clsDB.DbCon);

            //결과를 DB에 UPDATE
            result = hicResSpecialService.UpdateMunjinbyWrtNo(txtWrtNo.Text.To<long>(), strMunjin[0], strMunjin[1], strMunjin[2], strMunjin[3]
                                                            , strMunjin[4], strMunjin[5], strMunjin[6], strMunjin[7], strMunjin[8], strMunjin[9]
                                                            , strMunjin[10], strMunjin[11], strMunjin[12], strMunjin[13]);

            clsDB.setCommitTran(clsDB.DbCon);
        }

        /// <summary>
        /// 특검공통문진_UpDate
        /// </summary>
        void fn_SpcCommonMunjin_Update()
        {
            int j = 0;
            int K = 0;
            int No = 0;
            string[] strMunjinT = new string[14];
            string strRes = "";
            string strTemp = "";
            string strCODE = "";
            int result = 0;

            if (FstrCOMMIT != "OK")
            {
                return;
            }

            for (int i = 0; i < 14; i++)
            {
                strMunjinT[i] = "";
            }

            for (int i = 0; i < SSTMun.ActiveSheet.RowCount; i++)
            {
                strRes = SSTMun.ActiveSheet.Cells[i, 6].Text.Trim(); //값
                strCODE = SSTMun.ActiveSheet.Cells[i, 5].Text.Trim(); //코드
                //코드값으로 배열항목 지정
                switch (VB.Left(strCODE, 1))
                {
                    case "A":
                        No = 0;
                        break;
                    case "B":
                        No = 1;
                        break;
                    case "C":
                        No = 2;
                        break;
                    case "D":
                        No = 3;
                        break;
                    case "E":
                        No = 4;
                        break;
                    case "F":
                        No = 5;
                        break;
                    case "G":
                        No = 6;
                        break;
                    case "H":
                        No = 7;
                        break;
                    case "I":
                        No = 8;
                        break;
                    case "J":
                        No = 9;
                        break;
                    case "K":
                        No = 10;
                        break;
                    case "L":
                        No = 11;
                        break;
                    case "M":
                        No = 12;
                        break;
                    case "N":
                        No = 13;
                        break;
                    default:
                        break;
                }
                strMunjinT[No] += strCODE + "," + strRes + ";";
            }

            clsDB.setBeginTran(clsDB.DbCon);

            //결과를 DB에 UPDATE
            result = hicResSpecialService.UpdateMunjinToKbyWrtNo(txtWrtNo.Text.To<long>(), strMunjinT[0], strMunjinT[1], strMunjinT[2], strMunjinT[3]
                                                            , strMunjinT[4], strMunjinT[5], strMunjinT[6], strMunjinT[7], strMunjinT[8], strMunjinT[9], strMunjinT[10]);

            clsDB.setCommitTran(clsDB.DbCon);
        }

        /// <summary>
        /// 폐활량문진_UpDate
        /// </summary>
        void fn_LungCapacityMunjin_Update()
        {
            string strCapacity = "";
            string strOldJilHwan = "";
            string strOldJilHwan_ETC = "";
            string strSusul = "";
            string strSusul_ETC = "";
            string strDrug = "";
            string strDrug_Etc = "";
            string strSmoke = "";
            string strGbSmoke = "";
            string strDenty = "";
            string strDyspnea = "";
            string strGeumGi = "";
            string strGbSmoke1 = "";
            int result = 0;

            if (FstrCOMMIT != "OK")
            {
                return;
            }

            //폐활량검사 경험 유무(무:0, 유:1)
            strCapacity = "";
            if (chkYN1.Checked == true)
            {
                strCapacity = "1";
            }

            if (chkYN2.Checked == true)
            {
                strCapacity = "2";
            }

            //금기사항확인
            strGeumGi = "";
            if (chkGeumGi1.Checked == true)
            {
                strGeumGi += "1" + ",";
            }
            else
            {
                strGeumGi += "0" + ",";
            }

            if (chkGeumGi2.Checked == true)
            {
                strGeumGi += "1" + ",";
            }
            else
            {
                strGeumGi += "0" + ",";
            }

            if (chkGeumGi3.Checked == true)
            {
                strGeumGi += "1" + ",";
            }
            else
            {
                strGeumGi += "0" + ",";
            }

            if (chkGeumGi4.Checked == true)
            {
                strGeumGi += "1" + ",";
            }
            else
            {
                strGeumGi += "0" + ",";
            }

            if (chkGeumGi5.Checked == true)
            {
                strGeumGi += "1" + ",";
            }
            else
            {
                strGeumGi += "0" + ",";
            }

            if (chkGeumGi6.Checked == true)
            {
                strGeumGi += "1" + ",";
            }
            else
            {
                strGeumGi += "0" + ",";
            }

            if (chkGeumGi7.Checked == true)
            {
                strGeumGi += "1" + ",";
            }
            else
            {
                strGeumGi += "0" + ",";
            }

            //과거또는현재질병
            strOldJilHwan = "";
            if (chkJilhwan1.Checked == true)
            {
                strOldJilHwan += "1" + ",";
            }
            else
            {
                strOldJilHwan += "0" + ",";
            }

            if (chkJilhwan2.Checked == true)
            {
                strOldJilHwan += "1" + ",";
            }
            else
            {
                strOldJilHwan += "0" + ",";
            }

            if (chkJilhwan3.Checked == true)
            {
                strOldJilHwan += "1" + ",";
            }
            else
            {
                strOldJilHwan += "0" + ",";
            }

            if (chkJilhwan4.Checked == true)
            {
                strOldJilHwan += "1" + ",";
            }
            else
            {
                strOldJilHwan += "0" + ",";
            }

            if (chkJilhwan5.Checked == true)
            {
                strOldJilHwan += "1" + ",";
            }
            else
            {
                strOldJilHwan += "0" + ",";
            }

            if (chkJilhwan6.Checked == true)
            {
                strOldJilHwan += "1" + ",";
            }
            else
            {
                strOldJilHwan += "0" + ",";
            }

            if (chkJilhwan7.Checked == true)
            {
                strOldJilHwan += "1" + ",";
            }
            else
            {
                strOldJilHwan += "0" + ",";
            }

            if (chkJilhwan8.Checked == true)
            {
                strOldJilHwan += "1" + ",";
            }
            else
            {
                strOldJilHwan += "0" + ",";
            }

            if (chkJilhwan9.Checked == true)
            {
                strOldJilHwan += "1" + ",";
            }
            else
            {
                strOldJilHwan += "0" + ",";
            }

            if (chkJilhwan10.Checked == true)
            {
                strOldJilHwan += "1" + ",";
            }
            else
            {
                strOldJilHwan += "0" + ",";
            }

            if (chkJilhwan11.Checked == true)
            {
                strOldJilHwan += "1" + ",";
            }
            else
            {
                strOldJilHwan += "0" + ",";
            }

            //과거또는현재질병기타사항
            strOldJilHwan_ETC = "";
            if (!txtJilHwan.Text.IsNullOrEmpty())
            {
                strOldJilHwan_ETC = txtJilHwan.Text.Trim();
            }

            //수술경험
            strSusul = "";
            if (chkSusul1.Checked == true)
            {
                strSusul += "1" + ",";
            }
            else
            {
                strSusul += "0" + ",";
            }

            if (chkSusul2.Checked == true)
            {
                strSusul += "1" + ",";
            }
            else
            {
                strSusul += "0" + ",";
            }

            if (chkSusul3.Checked == true)
            {
                strSusul += "1" + ",";
            }
            else
            {
                strSusul += "0" + ",";
            }

            if (chkSusul4.Checked == true)
            {
                strSusul += "1" + ",";
            }
            else
            {
                strSusul += "0" + ",";
            }

            if (chkSusul5.Checked == true)
            {
                strSusul += "1" + ",";
            }
            else
            {
                strSusul += "0" + ",";
            }

            if (chkSusul6.Checked == true)
            {
                strSusul += "1" + ",";
            }
            else
            {
                strSusul += "0" + ",";
            }

            if (chkSusul7.Checked == true)
            {
                strSusul += "1" + ",";
            }
            else
            {
                strSusul += "0" + ",";
            }

            if (chkSusul8.Checked == true)
            {
                strSusul += "1" + ",";
            }
            else
            {
                strSusul += "0" + ",";
            }

            //수술경험기타사항
            strSusul_ETC = "";
            if (!txtSusul.Text.IsNullOrEmpty()) strSusul_ETC = txtSusul.Text.Trim();

            //현재약물복용여부(0:무, 2:기타)
            strDrug = "";
            if (chkDrug1.Checked == true)
            {
                strDrug += "1" + ",";
            }
            else
            {
                strDrug += "0" + ",";
            }

            if (chkDrug2.Checked == true)
            {
                strDrug += "1" + ",";
            }
            else
            {
                strDrug += "0" + ",";
            }

            if (chkDrug3.Checked == true)
            {
                strDrug += "1" + ",";
            }
            else
            {
                strDrug += "0" + ",";
            }

            if (chkDrug4.Checked == true)
            {
                strDrug += "1" + ",";
            }
            else
            {
                strDrug += "0" + ",";
            }

            //현재약물복용여부기타사항
            strDrug_Etc = "";
            if (!txtDrug.Text.IsNullOrEmpty()) strDrug_Etc = txtDrug.Text.Trim();

            //흡연관련문진
            strSmoke = "";
            strSmoke = txtSyymm1.Text.Trim() + "," + txtSyymm2.Text.Trim() + ",";
            strSmoke += txtDayCnt.Text.Trim() + ",";

            if (chkSmoke.Checked == true) strGbSmoke1 = "1";

            //금일흡연여부
            if (chkSmk1.Checked == true)
            {
                strGbSmoke = "0";
            }

            if (chkSmk2.Checked == true)
            {
                strGbSmoke = "1";
            }

            if (chkSmk3.Checked == true)
            {
                strGbSmoke = "2";
            }

            //의치착용여부
            strDenty = "";
            if (chkDenT1.Checked == true)
            {
                strDenty = "0";
            }

            if (chkDenT2.Checked == true)
            {
                strDenty = "1";
            }

            if (chkDenT3.Checked == true)
            {
                strDenty = "2";
            }

            //호흡곤란정도
            strDyspnea = "";
            if (chkGrade1.Checked == true)
            {
                strDenty = "0";
            }

            if (chkGrade2.Checked == true)
            {
                strDenty = "1";
            }

            if (chkGrade3.Checked == true)
            {
                strDenty = "2";
            }

            if (chkGrade4.Checked == true)
            {
                strDenty = "3";
            }

            if (chkGrade5.Checked == true)
            {
                strDenty = "4";
            }

            clsDB.setBeginTran(clsDB.DbCon);

            HIC_RES_SPECIAL item = new HIC_RES_SPECIAL();

            item.WEIGHT = txtWeight.Text.To<decimal>();
            item.HEIGHT = txtHeight.Text.To<decimal>();
            item.GBCAPACITY = strCapacity;
            item.OLDJILHWAN = strOldJilHwan;
            item.OLDJILHWAN_ETC = strOldJilHwan_ETC;
            item.GBDRUG = strDrug;
            item.GBDRUG_ETC = strSusul_ETC;
            item.SMOKEYEAR = strSmoke;
            item.GBSMOKE = strGbSmoke;
            item.GBDENTY = strDenty;
            item.DYSPNEA = strDyspnea;
            item.GBGEUMGI = strGeumGi;
            item.GBSMOKE1 = strGbSmoke1;

            result = hicResSpecialService.UpdateLungbyWrtNo(item);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("폐활량문진 Update 중 오류발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);
        }

        /// <summary>
        /// 거래처코드 찾기 화면 스프레드더블클릭이벤트
        /// </summary>
        /// <param name="item"></param>
        private void LtdCd_value(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        void fn_Screen_Clear()
        {
            sp.Spread_All_Clear(ssPatInfo);
            ssPatInfo.ActiveSheet.RowCount = 1;
            sp.Spread_All_Clear(SSJik);
            SSMun.ActiveSheet.RowCount = 0;

            FnWRTNO = 0;
            FstrSex = "";
            FstrROWID = "";
            FstrUCodes = "";
            FstrCOMMIT = "";
            FnPano = 0;

            txtWrtNo.Text = "";     txtIpDate.Text = "";        txtBuse.Text = "";
            txtGongjeng.Text = "";  txtJenipDate.Text = "";
            txtEtcMsym.Text = "";   txtHName.Text = "";
            txtGajok.Text = "";     txtGiinsung.Text = "";
            txtNeuro.Text = "";     txtHead.Text = "";          txtSkin.Text = "";
            txtChest.Text = "";     txtJengSang.Text = "";      txtDent.Text = "";
            txtDentDoct.Text = "";  lblDentDrName.Text = "";
            txtPanDrNo.Text = "";   lblDrName.Text = "";
            txtRemark.Text = "";

            txtSabun.Text = "";

            
            chkSang1.Checked = false;
            chkSang2.Checked = false;
            chkSang3.Checked = false;
            chkSang4.Checked = false;
            chkSang5.Checked = false;

            txtYear1.Text = "";
            txtYear2.Text = "";
            txtYear3.Text = "";
            txtYear4.Text = "";
            txtYear5.Text = "";

            cboGbSpc.SelectedIndex = -1;

            txtGongjeng.Text = "";
            lblHGigan.Text = "";
            lblUCodeName.Text = "";
            txtJengSang.Text = "";
            cboGbSpc.SelectedIndex = -1;

            chkSuChup.Checked = false;
            chkOHMS.Checked = false;
            rdoHuyu1.Checked = false;
            rdoHuyu2.Checked = false;
            rdoSang1.Checked = false;
            rdoSang2.Checked = false;
            rdoSang3.Checked = false;

            //작업중 상태
            txtWstat.Text = ""; lblWstat.Text = "";
            txtMstat.Text = ""; lblMstat.Text = "";

            //공통문진
            sp.Spread_All_Clear(SSMun);

            //폐활량 문진표
            txtWeight.Text = ""; txtHeight.Text = ""; txtJilHwan.Text = "";
            txtSusul.Text = ""; txtDrug.Text = ""; txtDayCnt.Text = "";

            chkYN1.Checked = false;
            txtSyymm1.Text = "";

            chkYN2.Checked = false;
            txtSyymm2.Text = "";

            chkJilhwan1.Checked = false;
            chkJilhwan2.Checked = false;
            chkJilhwan3.Checked = false;
            chkJilhwan4.Checked = false;
            chkJilhwan5.Checked = false;
            chkJilhwan6.Checked = false;
            chkJilhwan7.Checked = false;
            chkJilhwan8.Checked = false;
            chkJilhwan9.Checked = false;
            chkJilhwan10.Checked = false;
            chkJilhwan11.Checked = false;

            chkSusul1.Checked = false;
            chkSusul2.Checked = false;
            chkSusul3.Checked = false;
            chkSusul4.Checked = false;
            chkSusul5.Checked = false;
            chkSusul6.Checked = false;
            chkSusul7.Checked = false;
            chkSusul8.Checked = false;

            //chkGeumGi1
            chkGeumGi1.Checked = false;
            chkGeumGi2.Checked = false;
            chkGeumGi3.Checked = false;
            chkGeumGi4.Checked = false;
            chkGeumGi5.Checked = false;
            chkGeumGi6.Checked = false;
            chkGeumGi7.Checked = false;

            chkDrug1.Checked = false;
            chkDrug2.Checked = false;
            chkDrug3.Checked = false;
            chkDrug4.Checked = false;

            chkSmk1.Checked = false;
            chkSmk2.Checked = false;
            chkSmk3.Checked = false;

            chkDenT1.Checked = false;
            chkDenT2.Checked = false;
            chkDenT3.Checked = false;

            chkGrade1.Checked = false;
            chkGrade2.Checked = false;
            chkGrade3.Checked = false;
            chkGrade4.Checked = false;
            chkGrade5.Checked = false;

            chkSmoke.Checked = false;
            txtJilHwan.Enabled = false;
            txtSusul.Enabled = false;
            txtDrug.Enabled = false;

            //야간작업
            txtSum1.Text = ""; lblPan1.Text = "";
            txtSum2.Text = ""; lblPan2.Text = "";

            btnMenuSave.Enabled = false;
            btnMenuCancel.Enabled = false;
            pnlMain.Enabled = false;
        }

        void fn_Screen_Display()
        {
            long nWrtNo = 0;

            nWrtNo = txtWrtNo.Text.To<long>();
            FstrROWID = "";

            FnWRTNO = nWrtNo;
            if (FnWRTNO == 0) return;

            //삭제된것 체크
            if (hb.READ_JepsuSTS(FnWRTNO) == "D")
            {
                MessageBox.Show(FnWRTNO + "접수번호는 삭제된것 입니다. 확인하십시오", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //인적사항을 Display
            HIC_JEPSU list = hicJepsuService.GetItembyWrtNo(FnWRTNO);

            if (list.IsNullOrEmpty())
            {
                MessageBox.Show("접수번호 " + FnWRTNO + "번이 자료가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            FstrSex = list.SEX;
            FnPano = list.PANO;
            FstrJong = list.GJJONG;

            pnlMain.Enabled = true;

            ssPatInfo.ActiveSheet.Cells[0, 0].Text = list.PANO.To<string>();
            ssPatInfo.ActiveSheet.Cells[0, 1].Text = list.SNAME;
            ssPatInfo.ActiveSheet.Cells[0, 2].Text = list.AGE + "/" + FstrSex;
            ssPatInfo.ActiveSheet.Cells[0, 3].Text = hb.READ_Ltd_Name(list.LTDCODE.To<string>());
            ssPatInfo.ActiveSheet.Cells[0, 4].Text = list.JEPDATE;
            ssPatInfo.ActiveSheet.Cells[0, 5].Text = hb.READ_GjJong_Name(list.GJJONG);

            FstrYear = list.GJYEAR;
            txtSabun.Text = list.SABUN;
            txtIpDate.Text = list.IPSADATE;
            txtBuse.Text = list.BUSENAME;
            chkSuChup.Checked = false;
            if (list.GBSUCHEP == "Y") chkSuChup.Checked = true;

            FstrUCodes = list.UCODES;
            lblUCodeName.Text = hm.UCode_Names_Display(FstrUCodes);
            
            //Screen_Munjin_Display  '특수검진 진찰 및 문진내역
            //특수검진 기존의 문진내역을 Display
            string strGunGbn = "";

            HIC_RES_SPECIAL list2 = hicResSpecialService.GetItemByWrtno(nWrtNo);

            if (list2.IsNullOrEmpty())
            {
                FstrROWID = "";
                fn_First_Munjin_Display();
                return;
            }

            FstrROWID = list.RID;
            //사번,입사일자,현작업부서,수첩소지여부
            if (txtSabun.Text.IsNullOrEmpty()) txtSabun.Text = list2.SABUN;
            if (txtIpDate.Text.IsNullOrEmpty()) txtIpDate.Text = list2.IPSADATE;
            if (txtBuse.Text.IsNullOrEmpty()) txtBuse.Text = list2.BUSE;
            chkSuChup.Checked = false;
            if (list2.SUCHUPYN == "Y") chkSuChup.Checked = true;
            //현작업공정,현직전입일자,현직근무기간
            txtGongjeng.Text = list2.GONGJENG;
            txtGongjeng.Text = txtGongjeng.Text + "." + hb.READ_HIC_CODE("A2", txtGongjeng.Text);
            txtJenipDate.Text = list2.JENIPDATE;
            if (!list2.PGIGAN_YY.IsNullOrEmpty() && list2.PGIGAN_YY != 0)
            {
                lblHGigan.Text = list2.PGIGAN_YY.To<string>() + "년" + list2.PGIGAN_MM.To<string>() + "개월";
            }
            else
            {
                lblHGigan.Text = "";
            }
            //현직종,검진구분,일노출시간
            chkOHMS.Checked = false;
            if (list2.GBOHMS == "Y") chkOHMS.Checked = true;
            txtHName.Text = list2.HNAME;
            //특검종류
            cboGbSpc.SelectedIndex = -1;
            for (int j = 0; j < cboGbSpc.Items.Count; j++)
            {
                cboGbSpc.SelectedIndex = j;
                if (list2.GBSPC == VB.Left(cboGbSpc.Text, 2))
                {
                    cboGbSpc.SelectedIndex = j;
                    break;
                }
            }

            //과거직력 1,2,3(작업공정,취급화학물코드,물질명,근무년수,1일노출
            SSJik.ActiveSheet.RowCount = 3;
            //작업공정명
            SSJik.ActiveSheet.Cells[0, 0].Text = list2.OLDGONG1;
            SSJik.ActiveSheet.Cells[1, 0].Text = list2.OLDGONG2;
            SSJik.ActiveSheet.Cells[2, 0].Text = list2.OLDGONG3;

            //취급물질코드
            SSJik.ActiveSheet.Cells[0, 1].Text = list2.OLDMCODE1;
            SSJik.ActiveSheet.Cells[1, 1].Text = list2.OLDMCODE2;
            SSJik.ActiveSheet.Cells[2, 1].Text = list2.OLDMCODE3;

            //취급물질명칭
            SSJik.ActiveSheet.Cells[0, 3].Text = list2.OLDMCODE1;
            SSJik.ActiveSheet.Cells[1, 3].Text = list2.OLDMCODE2;
            SSJik.ActiveSheet.Cells[2, 3].Text = list2.OLDMCODE3;

            //근무년수
            SSJik.ActiveSheet.Cells[0, 4].Text = list2.OLDMCODE1;
            SSJik.ActiveSheet.Cells[1, 4].Text = list2.OLDMCODE2;
            SSJik.ActiveSheet.Cells[2, 4].Text = list2.OLDMCODE3;

            //노출기간
            SSJik.ActiveSheet.Cells[0, 5].Text = list2.OLDPGIGAN1;
            SSJik.ActiveSheet.Cells[1, 5].Text = list2.OLDPGIGAN2;
            SSJik.ActiveSheet.Cells[2, 5].Text = list2.OLDPGIGAN3;

            //일일노출시간
            SSJik.ActiveSheet.Cells[0, 6].Text = list2.OLDDAYTIME1.To<string>();
            SSJik.ActiveSheet.Cells[1, 6].Text = list2.OLDDAYTIME2.To<string>();
            SSJik.ActiveSheet.Cells[2, 6].Text = list2.OLDDAYTIME3.To<string>();

            //생활습관,외상및휴유증,과거병력,일반상태
            if (list2.HABIT1 == "Y") chkSang1.Checked = true;
            if (list2.HABIT2 == "Y") chkSang2.Checked = true;
            if (list2.HABIT3 == "Y") chkSang3.Checked = true;
            if (list2.HABIT4 == "Y") chkSang4.Checked = true;
            if (list2.HABIT5 == "Y") chkSang5.Checked = true;
            if (list2.GBHUYU == "Y") rdoHuyu1.Checked = true;
            if (list2.GBHUYU == "N") rdoHuyu2.Checked = true;
            txtYear1.Text = list2.OLDMYEAR1;
            txtYear2.Text = list2.OLDMYEAR2;
            txtYear3.Text = list2.OLDMYEAR3;
            txtYear4.Text = list2.OLDMYEAR4;
            txtYear5.Text = list2.OLDMYEAR5;
            txtEtcMsym.Text = list2.OLDETCMSYM;
            //문진(가족력,업무기인성,임상진찰(신경계,두경부,피부,흉복부)
            txtGajok.Text = list2.MUN_GAJOK;
            txtGiinsung.Text = list2.MUN_GIINSUNG; //업무기인성
            txtNeuro.Text = list2.JIN_NEURO;
            txtHead.Text = list2.JIN_HEAD;
            txtSkin.Text = list2.JIN_SKIN;
            txtChest.Text = list2.JIN_CHEST;
            //일반상태,현재증상및자타각증상
            if (list2.GBSANGTAE == "1") rdoSang1.Checked = true;
            if (list2.GBSANGTAE == "2") rdoSang2.Checked = true;
            if (list2.GBSANGTAE == "3") rdoSang3.Checked = true;
            txtJengSang.Text = list2.JENGSANG + "." + hb.READ_HIC_CODE("84", list2.JENGSANG);
            //치과소견,치과의사
            txtDent.Text = list2.DENTSOGEN;
            txtDentDoct.Text = list2.DENTDOCT.To<string>();
            if (txtDentDoct.Text.Trim() == "0")
            {
                txtDentDoct.Text = "";
            }
            lblDentDrName.Text = hb.READ_License_DrName(txtDentDoct.Text.To<long>());
            //진찰의사,참고사항
            txtPanDrNo.Text = list2.JINDRNO.To<string>();
            lblDrName.Text = hb.READ_License_DrName(txtPanDrNo.Text.To<long>());
            txtRemark.Text = list2.JINREMARK;

            //작업중 건강, 취급물질로 건강문제
            txtWstat.Text = list2.HSTAT;
            if (!txtWstat.Text.IsNullOrEmpty()) lblWstat.Text = hm.READ_Munjin_Name("특수검진", "예아니오", txtWstat.Text.Trim());
            lblWstat.Text = hm.READ_Munjin_Name_New("특수검진", "예아니오", txtWstat.Text.Trim());
            txtMstat.Text = list2.MCODE_STAT;
            if (!txtMstat.Text.IsNullOrEmpty()) lblMstat.Text = hm.READ_Munjin_Name("특수검진", "예아니오", txtMstat.Text.Trim());
            lblMstat.Text = hm.READ_Munjin_Name_New("특수검진", "예아니오", txtMstat.Text.Trim());

            //특수문진 판정의사가 없을경우 1차문진입력 의사나옴
            if (list2.JINDRNO == 0)
            {
                HIC_RES_BOHUM1 list3 = hicResBohum1Service.GetMunjinDrNobyWrtNo(FnWRTNO);

                if (!list3.IsNullOrEmpty())
                {
                    if (list3.MUNJINDRNO > 0)
                    {
                        txtPanDrNo.Text = list3.MUNJINDRNO.To<string>();
                        lblDrName.Text = hb.READ_License_DrName(txtPanDrNo.Text.To<long>());
                    }
                }
            }

            if (!FstrUCodes.IsNullOrEmpty())
            {
                //Call INSERT_HicResSpecial(FnWRTNO)
            }

            //특검공통문진
            fn_SSTMun_SpcMunjin(FnWRTNO);

            //폐활량문진표
            fn_LungCapacityMunjinReport(FnWRTNO);

            btnMenuSave.Enabled = true;
            btnMenuCancel.Enabled = true;
        }

        /// <summary>
        /// 특검공통문진 (SSTMun_특검문진(wrtno))
        /// </summary>
        /// <param name="nWrtNo"></param>
        void fn_SSTMun_SpcMunjin(long nWrtNo)
        {
            int nREAD = 0;
            string strCODE = "";
            string strTMunRes = "";
            string strValue = "";
            string strRemark = "";

            //특수검짐 문진표
            sp.Spread_All_Clear(SSTMun);

            //특수검진 문진을 읽음
            strTMunRes = hicResSpecialService.GetTmunAllbyWrtNo(nWrtNo);

            //코드을 읽음
            List<HIC_CODE> list = hicCodeService.GetGrpByNameGcode1ByGubun("56");

            nREAD = list.Count;
            SSTMun.ActiveSheet.RowCount = nREAD;
            for (int i = 0; i < nREAD; i++)
            {
                SSTMun.ActiveSheet.Cells[i, 0].Text = !list[i].GCODE1.IsNullOrEmpty() ? list[i].GCODE1 : ""; //신체부위
                SSTMun.ActiveSheet.Cells[i, 1].Text = list[i].NAME;
                strRemark = SSTMun.ActiveSheet.Cells[i, 1].Text.Trim();
                SSTMun.ActiveSheet.Cells[i, 5].Text = list[i].CODE;
                SSTMun.ActiveSheet.Cells[i, 6].Text = "";   //값
                SSTMun.ActiveSheet.Cells[i, 7].Text = list[i].ROWID;    

                //기본없음선택함
                SSTMun.ActiveSheet.Cells[i, 4].Text = "0";
                SSTMun.ActiveSheet.Cells[i, 6].Text = "0";
                if (FstrSex == "M")
                {
                    switch (strRemark)
                    {
                        case "3.생리가 불규칙해졌다.":
                            SSTMun.ActiveSheet.Cells[i, 4].Text = "";
                            SSTMun.ActiveSheet.Cells[i, 6].Text = "";
                            break;
                        case "4.자연유산을 한 적이 있다.":
                            SSTMun.ActiveSheet.Cells[i, 4].Text = "";
                            SSTMun.ActiveSheet.Cells[i, 6].Text = "";
                            break;
                        default:
                            break;
                    }
                }

                if (list[i].GCODE2 == "1")
                {
                    SSTMun.ActiveSheet.Cells[i, 0, i, SSTMun.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 255);
                }
                else
                {
                    SSTMun.ActiveSheet.Cells[i, 0, i, SSTMun.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(217, 255, 217);
                }

                strCODE = list[i].CODE;
                strCODE = strCODE.Trim();

                //저장된 문진값을 세팅함
                if (VB.I(strTMunRes, strCODE) > 1)
                {
                    strValue = VB.Pstr(VB.Pstr(VB.Pstr(strTMunRes, strCODE, 2), ";", 1), ",", 2).Trim();
                    switch (strValue)
                    {
                        case "0":
                            SSTMun.ActiveSheet.Cells[i, 4].Text = "0";
                            SSTMun.ActiveSheet.Cells[i, 6].Text = "0";
                            break;
                        case "1":
                            SSTMun.ActiveSheet.Cells[i, 3].Text = "1";
                            SSTMun.ActiveSheet.Cells[i, 4].Text = "";
                            SSTMun.ActiveSheet.Cells[i, 6].Text = "1";                            
                            break;
                        case "2":
                            SSTMun.ActiveSheet.Cells[i, 2].Text = "2";
                            SSTMun.ActiveSheet.Cells[i, 4].Text = "";
                            SSTMun.ActiveSheet.Cells[i, 6].Text = "2";                            
                            break;
                        default:
                            SSTMun.ActiveSheet.Cells[i, 6].Text = "";
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 폐활량문진표
        /// </summary>
        /// <param name="nWrtNo"></param>
        void fn_LungCapacityMunjinReport(long nWrtNo)
        {
            string strResult = "";

            HIC_RES_SPECIAL list = hicResSpecialService.GetItemByWrtno(nWrtNo);

            if (!list.IsNullOrEmpty())
            {
                if (list.WEIGHT == 0)
                {
                    txtWeight.Text = "";
                }
                else
                {
                    txtWeight.Text = list.WEIGHT.To<string>();
                }

                if (list.HEIGHT == 0)
                {
                    txtHeight.Text = "";
                }
                else
                {
                    txtHeight.Text = list.HEIGHT.To<string>();
                }

                //폐활량검사 경험
                if (!list.GBCAPACITY.IsNullOrEmpty())
                {
                    strResult = list.GBCAPACITY;
                    switch (strResult)
                    {
                        case "0":
                            chkYN1.Checked = true;
                            break;
                        case "1":
                            chkYN2.Checked = true;
                            break;
                        default:
                            break;
                    }
                }

                //2019추가사항
                if (!list.GBGEUMGI.IsNullOrEmpty())
                {
                    strResult = list.GBGEUMGI;                    
                    if (VB.Pstr(strResult, ",", 1) == "1")
                    {
                        chkGeumGi1.Checked = true;
                    }
                    else
                    {
                        chkGeumGi1.Checked = false;
                    }

                    if (VB.Pstr(strResult, ",", 2) == "1")
                    {
                        chkGeumGi2.Checked = true;
                    }
                    else
                    {
                        chkGeumGi2.Checked = false;
                    }

                    if (VB.Pstr(strResult, ",", 3) == "1")
                    {
                        chkGeumGi3.Checked = true;
                    }
                    else
                    {
                        chkGeumGi3.Checked = false;
                    }

                    if (VB.Pstr(strResult, ",", 4) == "1")
                    {
                        chkGeumGi4.Checked = true;
                    }
                    else
                    {
                        chkGeumGi4.Checked = false;
                    }

                    if (VB.Pstr(strResult, ",", 5) == "1")
                    {
                        chkGeumGi5.Checked = true;
                    }
                    else
                    {
                        chkGeumGi5.Checked = false;
                    }

                    if (VB.Pstr(strResult, ",", 6) == "1")
                    {
                        chkGeumGi6.Checked = true;
                    }
                    else
                    {
                        chkGeumGi6.Checked = false;
                    }

                    if (VB.Pstr(strResult, ",", 7) == "1")
                    {
                        chkGeumGi7.Checked = true;
                    }
                    else
                    {
                        chkGeumGi7.Checked = false;
                    }
                }

                //과거또는현재의 질병
                if (!list.OLDJILHWAN.IsNullOrEmpty())
                {
                    strResult = list.OLDJILHWAN;
                    if (VB.Pstr(strResult, ",", 1) == "1")
                    {
                        chkJilhwan1.Checked = true;
                    }
                    else
                    {
                        chkJilhwan1.Checked = false;
                    }

                    if (VB.Pstr(strResult, ",", 2) == "1")
                    {
                        chkJilhwan2.Checked = true;
                    }
                    else
                    {
                        chkJilhwan2.Checked = false;
                    }

                    if (VB.Pstr(strResult, ",", 3) == "1")
                    {
                        chkJilhwan3.Checked = true;
                    }
                    else
                    {
                        chkJilhwan3.Checked = false;
                    }

                    if (VB.Pstr(strResult, ",", 4) == "1")
                    {
                        chkJilhwan4.Checked = true;
                    }
                    else
                    {
                        chkJilhwan4.Checked = false;
                    }

                    if (VB.Pstr(strResult, ",", 5) == "1")
                    {
                        chkJilhwan5.Checked = true;
                    }
                    else
                    {
                        chkJilhwan5.Checked = false;
                    }

                    if (VB.Pstr(strResult, ",", 6) == "1")
                    {
                        chkJilhwan6.Checked = true;
                    }
                    else
                    {
                        chkJilhwan6.Checked = false;
                    }

                    if (VB.Pstr(strResult, ",", 7) == "1")
                    {
                        chkJilhwan7.Checked = true;
                    }
                    else
                    {
                        chkJilhwan7.Checked = false;
                    }

                    if (VB.Pstr(strResult, ",", 8) == "1")
                    {
                        chkJilhwan8.Checked = true;
                    }
                    else
                    {
                        chkJilhwan8.Checked = false;
                    }

                    if (VB.Pstr(strResult, ",", 9) == "1")
                    {
                        chkJilhwan9.Checked = true;
                    }
                    else
                    {
                        chkJilhwan9.Checked = false;
                    }

                    if (VB.Pstr(strResult, ",", 10) == "1")
                    {
                        chkJilhwan10.Checked = true;
                    }
                    else
                    {
                        chkJilhwan10.Checked = false;
                    }

                    if (VB.Pstr(strResult, ",", 11) == "1")
                    {
                        chkJilhwan11.Checked = true;
                    }
                    else
                    {
                        chkJilhwan11.Checked = false;
                    }
                }

                //과거또는현재의 질병기타사항
                if (!list.OLDJILHWAN_ETC.IsNullOrEmpty())
                {
                    txtJilHwan.Text = list.OLDJILHWAN_ETC;
                }

                //수술경험
                if (!list.GBSUSUL.IsNullOrEmpty())
                {
                    strResult = list.GBSUSUL;
                    if (VB.Pstr(strResult, ",", 1) == "1")
                    {
                        chkSusul1.Checked = true;
                    }
                    else
                    {
                        chkSusul1.Checked = false;
                    }

                    if (VB.Pstr(strResult, ",", 2) == "1")
                    {
                        chkSusul2.Checked = true;
                    }
                    else
                    {
                        chkSusul2.Checked = false;
                    }

                    if (VB.Pstr(strResult, ",", 3) == "1")
                    {
                        chkSusul3.Checked = true;
                    }
                    else
                    {
                        chkSusul3.Checked = false;
                    }

                    if (VB.Pstr(strResult, ",", 4) == "1")
                    {
                        chkSusul4.Checked = true;
                    }
                    else
                    {
                        chkSusul4.Checked = false;
                    }

                    if (VB.Pstr(strResult, ",", 5) == "1")
                    {
                        chkSusul5.Checked = true;
                    }
                    else
                    {
                        chkSusul5.Checked = false;
                    }

                    if (VB.Pstr(strResult, ",", 6) == "1")
                    {
                        chkSusul6.Checked = true;
                    }
                    else
                    {
                        chkSusul6.Checked = false;
                    }

                    if (VB.Pstr(strResult, ",", 7) == "1")
                    {
                        chkSusul7.Checked = true;
                    }
                    else
                    {
                        chkSusul7.Checked = false;
                    }

                    if (VB.Pstr(strResult, ",", 8) == "1")
                    {
                        chkSusul8.Checked = true;
                    }
                    else
                    {
                        chkSusul8.Checked = false;
                    }
                }

                //수술경험기타사항
                if (!list.GBSUSUL_ETC.IsNullOrEmpty())
                {
                    txtSusul.Text = list.GBSUSUL_ETC;
                }

                //현재복용약물
                if (!list.GBDRUG.IsNullOrEmpty())
                {
                    strResult = list.GBDRUG;
                    if (VB.Pstr(strResult, ",", 1) == "1")
                    {
                        chkDrug1.Checked = true;
                    }
                    else
                    {
                        chkDrug1.Checked = false;
                    }

                    if (VB.Pstr(strResult, ",", 2) == "1")
                    {
                        chkDrug2.Checked = true;
                    }
                    else
                    {
                        chkDrug2.Checked = false;
                    }

                    if (VB.Pstr(strResult, ",", 3) == "1")
                    {
                        chkDrug3.Checked = true;
                    }
                    else
                    {
                        chkDrug3.Checked = false;
                    }

                    if (VB.Pstr(strResult, ",", 4) == "1")
                    {
                        chkDrug4.Checked = true;
                    }
                    else
                    {
                        chkDrug4.Checked = false;
                    }
                }

                //현재복용약물기타사항
                if (!list.GBDRUG_ETC.IsNullOrEmpty())
                {
                    txtDrug.Text = list.GBDRUG_ETC;
                }

                //흡연력
                if (list.GBSMOKE1 == "1")
                {
                    chkSmoke.Checked = true;
                }

                if (!list.SMOKEYEAR.IsNullOrEmpty())
                {
                    strResult = list.SMOKEYEAR;                    
                    txtSyymm1.Text = VB.Pstr(strResult, ",", 1);
                    txtSyymm2.Text = VB.Pstr(strResult, ",", 2);
                    txtDayCnt.Text = VB.Pstr(strResult, ",", 3);
                }

                //금일흡연여부
                if (!list.GBSMOKE.IsNullOrEmpty())
                {
                    strResult = list.GBSMOKE;
                    switch (strResult)
                    {
                        case "0":
                            chkSmk1.Checked = true;
                            break;
                        case "1":
                            chkSmk2.Checked = true;
                            break;
                        case "2":
                            chkSmk3.Checked = true;
                            break;
                        default:
                            break;
                    }
                }

                //의치착용여부
                if (!list.GBDENTY.IsNullOrEmpty())
                {
                    strResult = list.GBDENTY;
                    switch (strResult)
                    {
                        case "0":
                            chkDenT1.Checked = true;
                            break;
                        case "1":
                            chkDenT2.Checked = true;
                            break;
                        case "2":
                            chkDenT3.Checked = true;
                            break;
                        default:
                            break;
                    }
                }

                //호흡곤란정도
                if (!list.DYSPNEA.IsNullOrEmpty())
                {
                    strResult = list.DYSPNEA;
                    switch (strResult)
                    {
                        case "0":
                            chkGrade1.Checked = true;
                            break;
                        case "1":
                            chkGrade2.Checked = true;
                            break;
                        case "2":
                            chkGrade3.Checked = true;
                            break;
                        case "3":
                            chkGrade4.Checked = true;
                            break;
                        case "4":
                            chkGrade5.Checked = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        void fn_First_Munjin_Display()
        {
            //건강보험 문진내역을 READ
            HIC_RES_BOHUM1 list = hicResBohum1Service.GetItemByWrtno(FnWRTNO);

            if (list.IsNullOrEmpty()) return;

            //생활습관 개선필요
            if (list.HABIT1 == "1")
            {
                chkSang1.Checked = true;
            }
            else
            {
                chkSang1.Checked = false;
            }

            if (list.HABIT2 == "1")
            {
                chkSang2.Checked = true;
            }
            else
            {
                chkSang2.Checked = false;
            }

            if (list.HABIT3 == "1")
            {
                chkSang3.Checked = true;
            }
            else
            {
                chkSang3.Checked = false;
            }

            if (list.HABIT4 == "1")
            {
                chkSang4.Checked = true;
            }
            else
            {
                chkSang4.Checked = false;
            }

            if (list.HABIT5 == "1")
            {
                chkSang5.Checked = true;
            }
            else
            {
                chkSang5.Checked = false;
            }

            //외상,휴유증
            if (list.JINCHAL1 == "1")
            {
                rdoHuyu2.Checked = true;
            }
            else
            {
                rdoHuyu1.Checked = true;
            }

            //일반상태(양호,보통,불량)
            switch (list.JINCHAL2)
            {
                case "1":
                    rdoSang1.Checked = true;
                    break;
                case "2":
                    rdoSang2.Checked = true;
                    break;
                case "3":
                    rdoSang3.Checked = true;
                    break;
                default:
                    break;
            }
        }
    }
}
