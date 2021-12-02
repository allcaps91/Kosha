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
/// File Name       : frmHcPanMunjin_2019.cs
/// Description     : 건강검진 문진 및 진찰결과 등록(2019)
/// Author          : 이상훈
/// Create Date     : 2019-11-07
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm건강검진문진_2019.frm(Frm건강검진문진_2019)" />

namespace HC_Pan
{
    public partial class frmHcPanMunjin_2019 : Form
    {
        HicJepsuService hicJepsuService = null;
        HicResBohum1Service hicResBohum1Service = null;
        HicJepsuHeaExjongService hicJepsuHeaExjongService = null;
        ComHpcLibBService comHpcLibBService = null;
        HicExjongService hicExjongService = null;

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        string FstrROWID;
        long FnWRTNO;
        long FnAge;
        string FstrSex;
        string FstrGjJong;
        string Fstr생애체크;
        string Fstr인지능력;

        public frmHcPanMunjin_2019()
        {
            InitializeComponent();

            SetEvent();
            SetControl();
        }

        public frmHcPanMunjin_2019(long nWrtNo)
        {
            InitializeComponent();

            FnWRTNO = nWrtNo;
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicJepsuService = new HicJepsuService();
            hicResBohum1Service = new HicResBohum1Service();
            hicJepsuHeaExjongService = new HicJepsuHeaExjongService();
            comHpcLibBService = new ComHpcLibBService();
            hicExjongService = new HicExjongService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick); 
            this.btnTempSave.Click += new EventHandler(eBtnClick); 
            this.btnDelete.Click += new EventHandler(eBtnClick); 
            this.btnNoSave.Click += new EventHandler(eBtnClick); 
            this.cboDrink1.KeyPress += new KeyPressEventHandler(eCboKeyPress);

            this.cboDrink21.KeyPress += new KeyPressEventHandler(eCboKeyPress);
            this.cboDrink22.KeyPress += new KeyPressEventHandler(eCboKeyPress);
            this.cboDrink23.KeyPress += new KeyPressEventHandler(eCboKeyPress);

            this.cboDrinkUnit1.KeyPress += new KeyPressEventHandler(eCboKeyPress);
            this.cboDrinkUnit2.KeyPress += new KeyPressEventHandler(eCboKeyPress);
            this.cboDrinkUnit3.KeyPress += new KeyPressEventHandler(eCboKeyPress);
            this.cboDrinkUnit4.KeyPress += new KeyPressEventHandler(eCboKeyPress);
            this.cboDrinkUnit5.KeyPress += new KeyPressEventHandler(eCboKeyPress);

            this.cboSmoking1.KeyPress += new KeyPressEventHandler(eCboKeyPress);
            this.cboSmoking2.KeyPress += new KeyPressEventHandler(eCboKeyPress);
            this.cboSmoking3.KeyPress += new KeyPressEventHandler(eCboKeyPress);
            this.cboSmoking4.KeyPress += new KeyPressEventHandler(eCboKeyPress);
            this.cboSmoking5.KeyPress += new KeyPressEventHandler(eCboKeyPress);
            this.cboSmoking6.KeyPress += new KeyPressEventHandler(eCboKeyPress);


            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            //this.SS1.ButtonClicked += new EditorNotifyEventHandler(eSpdBtnClick);
            this.chkSick11.KeyDown += new KeyEventHandler(eKeyDown);
            this.chkSick21.KeyDown += new KeyEventHandler(eKeyDown);
            this.chkSick63.KeyDown += new KeyEventHandler(eKeyDown);
            this.chkSick73.KeyDown += new KeyEventHandler(eKeyDown);
            this.chkSick53.KeyDown += new KeyEventHandler(eKeyDown);
            this.chkSick43.KeyDown += new KeyEventHandler(eKeyDown);
            this.chkSick33.KeyDown += new KeyEventHandler(eKeyDown);
            this.chkSick23.KeyDown += new KeyEventHandler(eKeyDown);
            this.chkSick13.KeyDown += new KeyEventHandler(eKeyDown);
            this.chkSick61.KeyDown += new KeyEventHandler(eKeyDown);
            this.chkSick71.KeyDown += new KeyEventHandler(eKeyDown);
            this.chkSick51.KeyDown += new KeyEventHandler(eKeyDown);
            this.chkSick41.KeyDown += new KeyEventHandler(eKeyDown);
            this.chkSick31.KeyDown += new KeyEventHandler(eKeyDown);

            this.chkGajok1.KeyDown += new KeyEventHandler(eKeyDown);
            this.chkGajok2.KeyDown += new KeyEventHandler(eKeyDown);
            this.chkGajok3.KeyDown += new KeyEventHandler(eKeyDown);
            this.chkGajok4.KeyDown += new KeyEventHandler(eKeyDown);
            this.chkGajok5.KeyDown += new KeyEventHandler(eKeyDown);

            this.txtWrtNo.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eTxtKeyPress);

            this.txt66Inject.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txt66Fall.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txt66Inject2.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txt66Stat1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txt66Stat2.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txt66Stat3.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txt66Stat4.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txt66Stat5.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txt66Stat6.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtActive11.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtActive12.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtActive13.KeyPress += new KeyPressEventHandler(eTxtKeyPress);

            this.txtActive21.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtActive22.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtActive23.KeyPress += new KeyPressEventHandler(eTxtKeyPress);

            this.txtActive31.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtDrink1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtDrink21.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtDrink22.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtDrink23.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtDrink24.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtDrink25.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtDrink26.KeyPress += new KeyPressEventHandler(eTxtKeyPress);

            this.txtSmoke1_1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtSmoke1_2.KeyPress += new KeyPressEventHandler(eTxtKeyPress);

            this.txtSmoke2_1.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtSmoke2_2.KeyPress += new KeyPressEventHandler(eTxtKeyPress);

            this.txt66Fall.LostFocus += new EventHandler(etxtLostFocus);
            this.txt66Inject.LostFocus += new EventHandler(etxtLostFocus);
            this.txt66Inject2.LostFocus += new EventHandler(etxtLostFocus);
            this.txt66Stat1.LostFocus += new EventHandler(etxtLostFocus);
            this.txt66Stat2.LostFocus += new EventHandler(etxtLostFocus);
            this.txt66Stat3.LostFocus += new EventHandler(etxtLostFocus);

            this.txt66Stat4.LostFocus += new EventHandler(etxtLostFocus);
            this.txt66Stat5.LostFocus += new EventHandler(etxtLostFocus);
            this.txt66Stat6.LostFocus += new EventHandler(etxtLostFocus);
            this.txt66Uro.LostFocus += new EventHandler(etxtLostFocus);

            this.txtLtdCode.LostFocus += new EventHandler(etxtLostFocus);


            this.txt66Inject.GotFocus += new EventHandler(etxtGotFocus);
            this.txt66Inject2.GotFocus += new EventHandler(etxtGotFocus);
            this.txt66Stat1.GotFocus += new EventHandler(etxtGotFocus);
            this.txt66Stat2.GotFocus += new EventHandler(etxtGotFocus);
            this.txt66Stat3.GotFocus += new EventHandler(etxtGotFocus);
            this.txt66Stat4.GotFocus += new EventHandler(etxtGotFocus);
            this.txt66Stat5.GotFocus += new EventHandler(etxtGotFocus);
            this.txt66Stat6.GotFocus += new EventHandler(etxtGotFocus);
            this.txt66Uro.GotFocus += new EventHandler(etxtGotFocus); 

        }

        void eKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (sender == txtWrtNo)
                {
                    if (txtWrtNo.Text.IsNullOrEmpty())
                    {
                        return;
                    }
                    fn_Screen_Clear();
                    fn_Screen_Display();
                }
                else if (sender == txtSmoke1_2)
                {
                    txtDrink1.Focus();
                }
                else if (sender == txtLtdCode)
                {   
                    if (txtLtdCode.Text.Length >= 2)
                    {
                        eBtnClick(btnLtdCode, new EventArgs());
                    }
                }
            }
        }

        void etxtLostFocus(object sender, EventArgs e)
        {
            if (sender == txt66Fall)
            {
                lbl66Fall.Text = hm.READ_Munjin_Name_New("건강검진", "예아니오", txt66Fall.Text);
                if (!txt66Fall.Text.IsNullOrEmpty() && lbl66Fall.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("입력값 오류 확인하세요", "입력오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txt66Fall.Focus();
                }
            }
            else if (sender == txt66Inject)
            {
                lbl66Inject.Text = hm.READ_Munjin_Name_New("건강검진", "예아니오", txt66Inject.Text.Trim());
                if (!txt66Inject.Text.IsNullOrEmpty() && lbl66Inject.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("입력값 오류 확인하세요", "입력오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txt66Inject.Focus();
                }
            }
            else if (sender == txt66Inject2)
            {
                lbl66Inject2.Text = hm.READ_Munjin_Name_New("건강검진", "예아니오", txt66Inject2.Text.Trim());
                if (!txt66Inject2.Text.IsNullOrEmpty() && lbl66Inject2.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("입력값 오류 확인하세요", "입력오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txt66Inject2.Focus();
                }
            }
            else if (sender == txt66Stat1)
            {
                lbl66Stat1.Text = hm.READ_Munjin_Name_New("건강검진", "예아니오", txt66Stat1.Text.Trim());
                if (!txt66Stat1.Text.IsNullOrEmpty() && lbl66Stat1.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("입력값 오류 확인하세요", "입력오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txt66Stat1.Focus();
                }
            }
            else if (sender == txt66Stat2)
            {
                lbl66Stat2.Text = hm.READ_Munjin_Name_New("건강검진", "예아니오", txt66Stat2.Text.Trim());
                if (!txt66Stat2.Text.IsNullOrEmpty() && lbl66Stat2.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("입력값 오류 확인하세요", "입력오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txt66Stat2.Focus();
                }
            }
            else if (sender == txt66Stat3)
            {
                lbl66Stat3.Text = hm.READ_Munjin_Name_New("건강검진", "예아니오", txt66Stat3.Text.Trim());
                if (!txt66Stat3.Text.IsNullOrEmpty() && lbl66Stat3.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("입력값 오류 확인하세요", "입력오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txt66Stat3.Focus();
                }
            }
            else if (sender == txt66Stat4)
            {
                lbl66Stat4.Text = hm.READ_Munjin_Name_New("건강검진", "예아니오", txt66Stat4.Text.Trim());
                if (!txt66Stat4.Text.IsNullOrEmpty() && lbl66Stat4.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("입력값 오류 확인하세요", "입력오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txt66Stat4.Focus();
                }
            }
            else if (sender == txt66Stat5)
            {
                lbl66Stat5.Text = hm.READ_Munjin_Name_New("건강검진", "예아니오", txt66Stat5.Text.Trim());
                if (!txt66Stat5.Text.IsNullOrEmpty() && lbl66Stat5.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("입력값 오류 확인하세요", "입력오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txt66Stat5.Focus();
                }
            }
            else if (sender == txt66Stat6)
            {
                lbl66Stat6.Text = hm.READ_Munjin_Name_New("건강검진", "예아니오", txt66Stat6.Text.Trim());
                if (!txt66Stat6.Text.IsNullOrEmpty() && lbl66Stat6.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("입력값 오류 확인하세요", "입력오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txt66Stat6.Focus();
                }
            }
            else if (sender == txt66Uro)
            {
                lbl66Uro.Text = hm.READ_Munjin_Name_New("건강검진", "예아니오", txt66Uro.Text.Trim());
                if (!txt66Uro.Text.IsNullOrEmpty() && lbl66Uro.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("입력값 오류 확인하세요", "입력오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txt66Uro.Focus();
                }
            }
            else if (sender == txtLtdCode)
            {
                lblMsg.Text = "";
            }
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            btnTempSave.Enabled = false;

            txtLtdCode.Text = "";
            dtpFrDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-30).ToShortDateString();
            dtpToDate.Text = clsPublic.GstrSysDate;
            if (string.Compare(dtpFrDate.Text, "2018-01-01") < 0) dtpFrDate.Text = "2018-01-01";
            if (string.Compare(dtpToDate.Text, "2018-01-01") < 0) dtpToDate.Text = "2018-01-01";

            sp.Spread_Clear_Simple(ssList, 1);
            ssList.ActiveSheet.RowCount = 100;
            List<HIC_EXJONG> list = hicExjongService.Read_ExJong_Add(true);

            if (!list.IsNullOrEmpty())
            {
                cboJong.Items.Clear();
                cboJong.Items.Add("**.전체");
                for (int i = 0; i < list.Count; i++)
                {
                    cboJong.Items.Add(list[i].CODE + "." + list[i].NAME);
                }
                cboJong.SelectedIndex = 0;
            }
            //hb.ComboJong_Set(cboJong);

            cboBTypeHepatitis.Items.Clear();
            cboBTypeHepatitis.Items.Add(" ");
            cboBTypeHepatitis.Items.Add("1.예");
            cboBTypeHepatitis.Items.Add("2.아니오 ");
            cboBTypeHepatitis.Items.Add("3.모름");

            cboSmoking1.Items.Clear();
            cboSmoking1.Items.Add(" ");
            cboSmoking1.Items.Add("1.현재 흡연중");
            cboSmoking1.Items.Add("2.현재 금연중(과거흡연자)");

            cboSmoking2.Items.Clear();
            cboSmoking2.Items.Add(" ");
            cboSmoking2.Items.Add("1.예");
            cboSmoking2.Items.Add("2.아니오");

            cboSmoking3.Items.Clear();
            cboSmoking3.Items.Add(" ");
            cboSmoking3.Items.Add("1.아니오");
            cboSmoking3.Items.Add("2.월1~2일");
            cboSmoking3.Items.Add("3.월3~9일");
            cboSmoking3.Items.Add("4.월10~29일");
            cboSmoking3.Items.Add("5.매일");

            cboSmoking4.Items.Clear();
            cboSmoking4.Items.Add(" ");
            cboSmoking4.Items.Add("1.현재 전자담배 흡연중");
            cboSmoking4.Items.Add("2.현재 전자담배 금연중(과거흡연자)");

            cboSmoking5.Items.Clear();
            cboSmoking5.Items.Add(" ");
            cboSmoking5.Items.Add("1.아니오");
            cboSmoking5.Items.Add("2.예");

            cboSmoking6.Items.Clear();
            cboSmoking6.Items.Add(" ");
            cboSmoking6.Items.Add("1.아니오");
            cboSmoking6.Items.Add("2.예");

            cboDrink1.Items.Clear();
            cboDrink1.Items.Add(" ");
            cboDrink1.Items.Add("1.일주일");
            cboDrink1.Items.Add("2.한달");
            cboDrink1.Items.Add("3.일년");
            cboDrink1.Items.Add("4.술을 마시지 않는다");

            for (int i = 0; i < 10; i++)
            {
                ComboBox cboDrink2 = (Controls.Find("cboDrink" + (i + 21).ToString(), true)[0] as ComboBox);
                cboDrink2.Items.Clear();
                cboDrink2.Items.Add(" ");
                cboDrink2.Items.Add("1.소주");
                cboDrink2.Items.Add("2.맥주");
                cboDrink2.Items.Add("3.양주");
                cboDrink2.Items.Add("4.막걸리");
                cboDrink2.Items.Add("5.와인");
            }

            for (int i = 0; i < 10; i++)
            {
                ComboBox cboDrinkUnit = (Controls.Find("cboDrinkUnit" + (i + 1).ToString(), true)[0] as ComboBox);
                cboDrinkUnit.Items.Clear();
                cboDrinkUnit.Items.Add(" ");
                cboDrinkUnit.Items.Add("잔");
                cboDrinkUnit.Items.Add("병");
                cboDrinkUnit.Items.Add("캔");
                cboDrinkUnit.Items.Add("CC");
            }

            txtWrtNo.Text = "";
            fn_Screen_Clear();
            eBtnClick(btnSearch, new EventArgs());

            if (FnWRTNO > 0)
            {
                txtWrtNo.Text = FnWRTNO.To<string>();
                eTxtKeyPress(txtWrtNo, new KeyPressEventArgs((char)Keys.Enter));
                //fn_Screen_Clear();
                //eBtnClick(btnSearch, new EventArgs());
            }

            //판정프로그램에서 접근시 Data 수정불가 조회만가능
            if (clsHcVariable.GstrTempValue == "1")
            {
                btnSave.Enabled = false;
                btnTempSave.Enabled = false;
                btnDelete.Enabled = false;
            }
        }

        void etxtGotFocus(object sender, EventArgs e)
        {
            if (sender == txt66Fall)
            {
                lblMsg.Text = "1.예    2.아니오";
            }
            else if (sender == txt66Inject2)
            {
                lblMsg.Text = "1.예    2.아니오";
            }
            else if (sender == txt66Stat1)
            {
                lblMsg.Text = "1.예    2.아니오";
            }
            else if (sender == txt66Stat2)
            {
                lblMsg.Text = "1.예    2.아니오";
            }
            else if (sender == txt66Stat3)
            {
                lblMsg.Text = "1.예    2.아니오";
            }
            else if (sender == txt66Stat4)
            {
                lblMsg.Text = "1.예    2.아니오";
            }
            else if (sender == txt66Stat5)
            {
                lblMsg.Text = "1.예    2.아니오";
            }
            else if (sender == txt66Stat6)
            {
                lblMsg.Text = "1.예    2.아니오";
            }
            else if (sender == txt66Uro)
            {
                lblMsg.Text = "1.예    2.아니오";
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
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
                    txtLtdCode.Text = LtdHelpItem.CODE.ToString() + "." + LtdHelpItem.SANGHO;
                }
                else
                {
                    txtLtdCode.Text = "";
                }
            }
            else if (sender == btnSearch)
            {
                int nRead = 0;
                string strList = "";
                string strFrDate = "";
                string strToDate = "";
                string strJob = "";
                string strLtdCode = "";
                string strGjJong = "";

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;

                sp.Spread_All_Clear(ssList);
                ssList.ActiveSheet.RowCount = 50;

                txtLtdCode.Text = txtLtdCode.Text + "." + hb.READ_Ltd_Name(VB.Pstr(txtLtdCode.Text, ".", 1));
                if (txtLtdCode.Text.Trim() == ".") txtLtdCode.Text = "";
                strLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1);
                //strGjJong = VB.Left(cboJong.Text, 2);
                strGjJong = "11";

                if (rdoJob1.Checked == true)
                {
                    strJob = "1";
                }
                else if (rdoJob2.Checked == true)
                {
                    strJob = "2";
                }
                else
                {
                    strJob = "";
                }

                List<HIC_JEPSU_HEA_EXJONG> list = hicJepsuHeaExjongService.GetItembyJepDate(strFrDate, strToDate, strJob, strLtdCode, strGjJong);

                nRead = list.Count;
                ssList.ActiveSheet.RowCount = nRead;
                for (int i = 0; i < nRead; i++)
                {
                    ssList.ActiveSheet.Cells[i, 0].Text = list[i].WRTNO.To<string>();
                    ssList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                    ssList.ActiveSheet.Cells[i, 2].Text = list[i].JEPDATE;
                    ssList.ActiveSheet.Cells[i, 3].Text = list[i].GJJONG;
                }
            }
            else if (sender == btnNoSave)
            {
                txtWrtNo.Text = "";
                fn_Screen_Clear();
                txtWrtNo.Focus();
            }
            else if (sender == btnDelete)
            {
                string sMsg = "";
                int result = 0;

                sMsg = "정말로 문진표 및 판정내역을" + "\r\n";
                sMsg += "삭제 하시겠습니까?";

                if (MessageBox.Show(sMsg, "선택", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                //문진내역,보험청구내역을 삭제
                result = hicResBohum1Service.DeletebyWrtNo(FnWRTNO);

                if (result < 0)
                {
                    MessageBox.Show("문진내역을 삭제시 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //접수의 문진결과 등록 Flag를 미등록으로 변경
                result = hicJepsuService.UpdateGbMunjin1NullbyWrtNo(FnWRTNO);

                if (result < 0)
                {
                    MessageBox.Show("접수마스타에 문진미등록 FLAG변경시 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                eBtnClick(btnSearch, new EventArgs());
                txtWrtNo.Text = "";
                fn_Screen_Clear();
                txtWrtNo.Focus();
            }
            else if (sender == btnSave)
            {
                fn_DB_Update_Main("저장");
            }
            else if (sender == btnTempSave)
            {
                fn_DB_Update_Main("임시저장");
            }
        }

        void eCboKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (sender == cboSmoking1)
                {
                    if (VB.Left(cboSmoking1.Text, 1) == "1")
                    {
                        //아니오 경우 음주로 감
                        txtDrink1.Focus();
                    }
                    else if (VB.Left(cboSmoking1.Text, 1) == "2")
                    {
                        //예,지금은 끊었음 경우 금연으로 감
                        txtSmoke1_1.Focus();
                    }
                    else if (VB.Left(cboSmoking1.Text, 1) == "3")
                    {
                        //예.현재도 피움 경우 흡연으로 감
                        txtSmoke2_1.Focus();
                    }
                }
                else if (sender == cboSmoking2 || sender == cboSmoking3)
                {
                    SendKeys.Send("{Tab}");
                }
                else
                {
                    SendKeys.Send("{Tab}");
                }
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == ssList)
            {
                txtWrtNo.Text = ssList.ActiveSheet.Cells[e.Row, 0].Text;
                fn_Screen_Display();
                chkSick11.Focus();
            }
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
            FstrROWID = "";
            FstrSex = "";
            //FnWRTNO = 0;
            FstrGjJong = "";
            Fstr생애체크 = "";

            pnlMain.Enabled = false;
            pnlList.Enabled = true;

            sp.Spread_Clear_Simple(ssPatInfo, 1);

            txtLtdCode.Text = "";

            for (int i = 1; i <= 5; i++)
            {
                CheckBox chkHabit = (Controls.Find("chkHabit" + i.ToString(), true)[0] as CheckBox);
                chkHabit.Checked = false;
            }
            rdoJinchal11.Checked = false;
            rdoJinchal12.Checked = false;

            rdoJinchal21.Checked = false;
            rdoJinchal22.Checked = false;
            rdoJinchal23.Checked = false;

            for (int i = 1; i <= 7; i++)
            {
                CheckBox chkOldByeng = (Controls.Find("chkOldByeng" + i.ToString(), true)[0] as CheckBox);
                chkOldByeng.Checked = false;
            }

            //과거질병 진단여부,치료여부
            chkSick63.Checked = false;
            chkSick73.Checked = false;
            chkSick53.Checked = false;
            chkSick43.Checked = false;
            chkSick33.Checked = false;
            chkSick23.Checked = false;
            chkSick13.Checked = false;
            chkSick61.Checked = false;
            chkSick71.Checked = false;
            chkSick51.Checked = false;
            chkSick41.Checked = false;
            chkSick31.Checked = false;
            chkSick21.Checked = false;
            chkSick11.Checked = false;

            //부모,형제,자매 질환여부
            chkGajok1.Checked = false; 
            chkGajok2.Checked = false;
            chkGajok3.Checked = false; 
            chkGajok4.Checked = false;
            chkGajok5.Checked = false;

            cboBTypeHepatitis.SelectedIndex = -1;

            //흡연
            cboSmoking1.SelectedIndex = -1;
            txtSmoke1_1.Text = "";
            txtSmoke1_2.Text = "";
            txtSmoke2_1.Text = ""; 
            txtSmoke2_2.Text = "";
            txtSmoke3_1.Text = "";

            txtSmoke4_1.Text = ""; 
            txtSmoke4_2.Text = "";
            txtSmoke5_1.Text = ""; 
            txtSmoke5_2.Text = "";
            txtSmoke6_1.Text = "";

            cboSmoking2.SelectedIndex = -1;
            cboSmoking3.SelectedIndex = -1;
            cboSmoking4.SelectedIndex = -1;
            cboSmoking5.SelectedIndex = -1;
            cboSmoking6.SelectedIndex = -1;

            //음주
            cboDrink1.SelectedIndex = -1;
            txtDrink1.Text = "";

            for (
                int i = 1; i <= 10; i++)
            {
                ComboBox cboDrink2 = (Controls.Find("cboDrink" + (i + 20).ToString(), true)[0] as ComboBox);
                ComboBox cboDrinkUnit = (Controls.Find("cboDrinkUnit" + i.ToString(), true)[0] as ComboBox);
                TextBox txtDrink2 = (Controls.Find("txtDrink" + (i + 20).ToString(), true)[0] as TextBox); 
                cboDrink2.SelectedIndex = -1;
                cboDrinkUnit.SelectedIndex = -1;
                txtDrink2.Text = "";
            }

            //신체활동,운동
            for (int i = 1; i <= 3; i++)
            {   
                TextBox txtActive1 = (Controls.Find("txtActive1" + i.ToString(), true)[0] as TextBox);
                TextBox txtActive2 = (Controls.Find("txtActive2" + i.ToString(), true)[0] as TextBox);
                txtActive1.Text = "";
                txtActive2.Text = "";
            }
            txtActive31.Text = "";

            //만66세 생애대상자만
            txt66Inject.Text = "";
            lbl66Inject.Text = "";
            txt66Inject2.Text = "";
            lbl66Inject2.Text = "";

            txt66Stat1.Text = "";
            lbl66Stat1.Text = "";
            txt66Stat2.Text = "";
            lbl66Stat2.Text = "";
            txt66Stat3.Text = "";
            lbl66Stat3.Text = "";
            txt66Stat4.Text = "";
            lbl66Stat4.Text = "";
            txt66Stat5.Text = "";
            lbl66Stat5.Text = "";
            txt66Stat6.Text = "";
            lbl66Stat6.Text = "";

            txt66Fall.Text = "";
            lbl66Fall.Text = "";
            txt66Uro.Text = "";
            lbl66Uro.Text = "";
        }

        void fn_Screen_Display()
        {
            long nAge = 0;
            string strData = "";
            string strYear = "";

            FnWRTNO = txtWrtNo.Text.To<long>();
            if (FnWRTNO == 0) return;

            HIC_JEPSU item = hicJepsuService.Read_Jepsu_Wrtno(FnWRTNO);
            if (item.IsNullOrEmpty()) { return; }

            //삭제된것 체크
            if (hb.READ_JepsuSTS(FnWRTNO) == "D")
            {
                MessageBox.Show(FnWRTNO + " 접수번호는 삭제된것 입니다. 확인하십시오", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //인적사항을 Display
            HIC_JEPSU list = hicJepsuService.GetItembyWrtNo(FnWRTNO);

            if (list.IsNullOrEmpty())
            {
                MessageBox.Show("접수번호 " + FnWRTNO + " 번이 자료가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FstrSex = list.SEX;
            FstrGjJong = list.GJJONG;
            FnAge = list.AGE;
            strYear = list.GJYEAR;
            ssPatInfo.ActiveSheet.Cells[0, 0].Text = list.PANO.To<string>();
            ssPatInfo.ActiveSheet.Cells[0, 1].Text = list.SNAME;
            ssPatInfo.ActiveSheet.Cells[0, 2].Text = list.AGE.To<string>() + "/" + list.SEX;
            ssPatInfo.ActiveSheet.Cells[0, 3].Text = hb.READ_Ltd_Name(list.LTDCODE.To<string>());
            ssPatInfo.ActiveSheet.Cells[0, 4].Text = list.JEPDATE;
            ssPatInfo.ActiveSheet.Cells[0, 5].Text = hb.READ_GjJong_Name(list.GJJONG);

            pnlMain.Enabled = true;
            btnSave.Enabled = true;
            btnNoSave.Enabled = true;
            //pnlList.Enabled = false;

            pnlNoin.Enabled = false;

            switch (FnAge)
            {
                case 66:
                case 70:
                case 80:
                    pnlNoin.Enabled = true;
                    Fstr인지능력 = "OK";
                    break;
                default:
                    pnlNoin.Enabled = false;
                    Fstr인지능력 = "";
                    break;
            }


            if(strYear == "2021" && Fstr인지능력 =="")
            {
                switch (FnAge)
                {
                    case 67:
                    case 71:
                    case 81:
                        pnlNoin.Enabled = true;
                        Fstr인지능력 = "OK";
                        break;
                    default:
                        pnlNoin.Enabled = false;
                        Fstr인지능력 = "";
                        break;
                }
            }

            //문진내역을 READ
            FstrROWID = "";
            HIC_RES_BOHUM1 list2 = hicResBohum1Service.GetItemByWrtno(FnWRTNO);

            if (list2.IsNullOrEmpty())
            {
                pnlMain.Enabled = true;
                btnSave.Enabled = true;
                btnDelete.Enabled = false;
                btnNoSave.Enabled = true;
                pnlList.Enabled = false;
                return;
            }

            FstrROWID = list2.RID;

            //현재상태
            if (list2.T_STAT01 == "1") chkSick11.Checked = true;
            if (list2.T_STAT02 == "1") chkSick13.Checked = true;
            if (list2.T_STAT11 == "1") chkSick21.Checked = true;
            if (list2.T_STAT12 == "1") chkSick23.Checked = true;
            if (list2.T_STAT21 == "1") chkSick31.Checked = true;
            if (list2.T_STAT22 == "1") chkSick33.Checked = true;
            if (list2.T_STAT31 == "1") chkSick41.Checked = true;
            if (list2.T_STAT32 == "1") chkSick43.Checked = true;
            if (list2.T_STAT41 == "1") chkSick51.Checked = true;
            if (list2.T_STAT42 == "1") chkSick53.Checked = true;
            if (list2.T_STAT51 == "1") chkSick61.Checked = true;
            if (list2.T_STAT52 == "1") chkSick63.Checked = true;
            if (list2.T_STAT61 == "1") chkSick71.Checked = true;
            if (list2.T_STAT62 == "1") chkSick73.Checked = true;

            //가족병력
            if (list2.T_GAJOK1 == "1") chkGajok1.Checked = true;
            if (list2.T_GAJOK2 == "1") chkGajok2.Checked = true;
            if (list2.T_GAJOK3 == "1") chkGajok3.Checked = true;
            if (list2.T_GAJOK4 == "1") chkGajok4.Checked = true;
            if (list2.T_GAJOK5 == "1") chkGajok5.Checked = true;

            //과거병력
            //for (int i = 1; i <= 7; i++)
            //{
            //    CheckBox chkOldByeng = (Controls.Find("chkOldByeng" + i.ToString(), true)[0] as CheckBox);

            //    if (list2.OLDBYENG1 == "1")
            //    {
            //        chkOldByeng.Checked = true;
            //    }
            //    else
            //    {
            //        chkOldByeng.Checked = false;
            //    }

            //}
            if (list2.OLDBYENG1 == "1")
            {
                chkOldByeng1.Checked = true;
            }
            else
            {
                chkOldByeng1.Checked = false;
            }
            if (list2.OLDBYENG2 == "1")
            {
                chkOldByeng2.Checked = true;
            }
            else
            {
                chkOldByeng2.Checked = false;
            }
            if (list2.OLDBYENG3 == "1")
            {
                chkOldByeng3.Checked = true;
            }
            else
            {
                chkOldByeng3.Checked = false;
            }
            if (list2.OLDBYENG4 == "1")
            {
                chkOldByeng4.Checked = true;
            }
            else
            {
                chkOldByeng4.Checked = false;
            }
            if (list2.OLDBYENG5 == "1")
            {
                chkOldByeng5.Checked = true;
            }
            else
            {
                chkOldByeng5.Checked = false;
            }
            if (list2.OLDBYENG6 == "1")
            {
                chkOldByeng6.Checked = true;
            }
            else
            {
                chkOldByeng6.Checked = false;
            }
            if (list2.OLDBYENG7 == "1")
            {
                chkOldByeng7.Checked = true;
            }
            else
            {
                chkOldByeng7.Checked = false;
            }
            txtOldByengName.Text = list2.OLDBYENGNAME;

            //B형간염
            strData = list2.T_BLIVER;
            if (!strData.IsNullOrEmpty())
            {
                for (int i = 0; i < cboBTypeHepatitis.Items.Count; i++)
                {
                    cboBTypeHepatitis.SelectedIndex = i;
                    if (strData == VB.Left(cboBTypeHepatitis.Text, 1))
                    {
                        cboBTypeHepatitis.SelectedIndex = i;
                        break;
                    }
                }
            }

            //흡연
            strData = list2.T_SMOKE1;
            if (!strData.IsNullOrEmpty())
            {   
                for (int i = 0; i < cboSmoking1.Items.Count; i++)
                {
                    cboSmoking1.SelectedIndex = i;
                    if (strData == VB.Left(cboSmoking1.Text, 1))
                    {
                        cboSmoking1.SelectedIndex = i;
                        break;
                    }
                }
            }

            txtSmoke1_1.Text = list2.T_SMOKE2.ToString();
            txtSmoke1_2.Text = list2.T_SMOKE3.ToString();
            txtSmoke2_1.Text = list2.T_SMOKE4.ToString();
            txtSmoke2_2.Text = list2.T_SMOKE5.ToString();
            txtSmoke3_1.Text = list2.TMUN0096;

            strData = list2.TMUN0097;
            if (!strData.IsNullOrEmpty())
            {
                for (int i = 0; i < cboSmoking4.Items.Count; i++)
                {
                    cboSmoking4.SelectedIndex = i;
                    if (strData == VB.Left(cboSmoking4.Text, 1))
                    {
                        cboSmoking4.SelectedIndex = i;
                        break;
                    }
                }
            }

            //궐련형 전자담배
            txtSmoke4_1.Text = list2.TMUN0098;
            txtSmoke4_2.Text = list2.TMUN0099;
            txtSmoke5_1.Text = list2.TMUN0100;
            txtSmoke5_2.Text = list2.TMUN0101;
            txtSmoke6_1.Text = list2.TMUN0102;

            //전자담배
            strData = list2.TMUN0001;
            if (!strData.IsNullOrEmpty())
            {
                for (int i = 0; i < cboSmoking2.Items.Count; i++)
                {
                    cboSmoking2.SelectedIndex = i;
                    if (strData == VB.Left(cboSmoking2.Text, 1))
                    {
                        cboSmoking2.SelectedIndex = i;
                        break;
                    }
                }
            }

            strData = list2.TMUN0002;
            if (!strData.IsNullOrEmpty())
            {
                for (int i = 0; i < cboSmoking3.Items.Count; i++)
                {
                    cboSmoking3.SelectedIndex = i;
                    if (strData == VB.Left(cboSmoking3.Text, 1))
                    {
                        cboSmoking3.SelectedIndex = i;
                        break;
                    }
                }
            }

            strData = list2.TMUN0103;
            if (!strData.IsNullOrEmpty())
            {
                for (int i = 0; i < cboSmoking5.Items.Count; i++)
                {
                    cboSmoking5.SelectedIndex = i;
                    if (strData == VB.Left(cboSmoking5.Text, 1))
                    {
                        cboSmoking5.SelectedIndex = i;
                        break;
                    }
                }
            }

            strData = list2.TMUN0104;
            if (!strData.IsNullOrEmpty())
            {
                for (int i = 0; i < cboSmoking6.Items.Count; i++)
                {
                    cboSmoking6.SelectedIndex = i;
                    if (strData == VB.Left(cboSmoking6.Text, 1))
                    {
                        cboSmoking6.SelectedIndex = i;
                        break;
                    }
                }
            }

            //음주
            //술을 마시는 횟수 구분(1.일주일,2.한달 3.1년 4.술을 마시지 않는다)
            strData = list2.TMUN0003;
            if (!strData.IsNullOrEmpty())
            {
                for (int i = 0; i < cboDrink1.Items.Count; i++)
                {
                    cboDrink1.SelectedIndex = i;
                    if (strData == VB.Left(cboDrink1.Text, 1))
                    {
                        cboDrink1.SelectedIndex = i;
                        break;
                    }
                }
            }

            txtDrink1.Text = list2.TMUN0004;

            //보통음주1
            strData = list2.TMUN0005;
            if (!strData.IsNullOrEmpty())
            {
                for (int i = 0; i < cboDrink21.Items.Count; i++)
                {
                    cboDrink21.SelectedIndex = i;
                    if (VB.Left(strData, 1) == VB.Left(cboDrink21.Text, 1))
                    {
                        cboDrink21.SelectedIndex = i;
                        break;
                    }
                }

                txtDrink21.Text = VB.Pstr(strData, ";", 2);
                for (int j = 0; j < cboDrinkUnit1.Items.Count; j++)
                {
                    cboDrinkUnit1.SelectedIndex = j;
                    if (VB.Pstr(strData, ";", 3) == cboDrinkUnit1.Text)
                    {
                        cboDrinkUnit1.SelectedIndex = j;
                        break;
                    }
                }
            }

            //보통음주2
            strData = list2.TMUN0006;
            if (!strData.IsNullOrEmpty())
            {
                for (int j = 0; j < cboDrink22.Items.Count; j++)
                {
                    cboDrink22.SelectedIndex = j;
                    if (VB.Left(strData, 1) == VB.Left(cboDrink22.Text, 1))
                    {
                        cboDrink22.SelectedIndex = j;
                        break;
                    }
                }

                txtDrink22.Text = VB.Pstr(strData, ";", 2);
                for (int j = 0; j < cboDrinkUnit2.Items.Count; j++)
                {
                    cboDrinkUnit2.SelectedIndex = j;
                    if (VB.Pstr(strData, ";", 3) == cboDrinkUnit2.Text)
                    {
                        cboDrinkUnit2.SelectedIndex = j;
                        break;
                    }
                }
            }

            //보통음주3
            strData = list2.TMUN0007;
            if (!strData.IsNullOrEmpty())
            {
                for (int i = 0; i < cboDrink23.Items.Count; i++)
                {
                    cboDrink23.SelectedIndex = i;
                    if (VB.Left(strData, 1) == VB.Left(cboDrink23.Text, 1))
                    {
                        cboDrink23.SelectedIndex = i;
                        break;
                    }
                }
                txtDrink23.Text = VB.Pstr(strData, ";", 2);
                for (int j = 0; j < cboDrinkUnit3.Items.Count; j++)
                {
                    cboDrinkUnit3.SelectedIndex = j;
                    if (VB.Pstr(strData, ";", 3) == cboDrinkUnit3.Text)
                    {
                        cboDrinkUnit3.SelectedIndex = j;
                        break;
                    }
                }
            }

            //2021-02-03
            //보통음주4
            strData = list2.TMUN0125;
            if (!strData.IsNullOrEmpty())
            {
                for (int i = 0; i < cboDrink27.Items.Count; i++)
                {
                    cboDrink27.SelectedIndex = i;
                    if (VB.Left(strData, 1) == VB.Left(cboDrink27.Text, 1))
                    {
                        cboDrink27.SelectedIndex = i;
                        break;
                    }
                }
                txtDrink27.Text = VB.Pstr(strData, ";", 2);
                for (int j = 0; j < cboDrinkUnit7.Items.Count; j++)
                {
                    cboDrinkUnit7.SelectedIndex = j;
                    if (VB.Pstr(strData, ";", 3) == cboDrinkUnit7.Text)
                    {
                        cboDrinkUnit7.SelectedIndex = j;
                        break;
                    }
                }
            }
            //보통음주5
            strData = list2.TMUN0126;
            if (!strData.IsNullOrEmpty())
            {
                for (int i = 0; i < cboDrink28.Items.Count; i++)
                {
                    cboDrink28.SelectedIndex = i;
                    if (VB.Left(strData, 1) == VB.Left(cboDrink28.Text, 1))
                    {
                        cboDrink28.SelectedIndex = i;
                        break;
                    }
                }
                txtDrink28.Text = VB.Pstr(strData, ";", 2);
                for (int j = 0; j < cboDrinkUnit8.Items.Count; j++)
                {
                    cboDrinkUnit8.SelectedIndex = j;
                    if (VB.Pstr(strData, ";", 3) == cboDrinkUnit8.Text)
                    {
                        cboDrinkUnit8.SelectedIndex = j;
                        break;
                    }
                }
            }

            //최대음주1
            strData = list2.TMUN0008;
            if (!strData.IsNullOrEmpty())
            {
                for (int i = 0; i < cboDrink24.Items.Count; i++)
                {
                    cboDrink24.SelectedIndex = i;
                    if (VB.Left(strData, 1) == VB.Left(cboDrink24.Text, 1))
                    {
                        cboDrink24.SelectedIndex = i;
                        break;
                    }
                }
                txtDrink24.Text = VB.Pstr(strData, ";", 2);
                for (int j = 0; j < cboDrinkUnit4.Items.Count; j++)
                {
                    cboDrinkUnit4.SelectedIndex = j;
                    if (VB.Pstr(strData, ";", 3) == cboDrinkUnit4.Text)
                    {
                        cboDrinkUnit4.SelectedIndex = j;
                        break;
                    }
                }
            }

            //최대음주2
            strData = list2.TMUN0009;
            if (!strData.IsNullOrEmpty())
            {
                for (int i = 0; i < cboDrink25.Items.Count; i++)
                {
                    cboDrink25.SelectedIndex = i;
                    if (VB.Left(strData, 1) == VB.Left(cboDrink25.Text, 1))
                    {
                        cboDrink25.SelectedIndex = i;
                        break;
                    }
                }
                txtDrink25.Text = VB.Pstr(strData, ";", 2);
                for (int j = 0; j < cboDrinkUnit5.Items.Count; j++)
                {
                    cboDrinkUnit5.SelectedIndex = j;
                    if (VB.Pstr(strData, ";", 3) == cboDrinkUnit5.Text)
                    {
                        cboDrinkUnit5.SelectedIndex = j;
                        break;
                    }
                }
            }

            //최대음주3
            strData = list2.TMUN0010;
            if (!strData.IsNullOrEmpty())
            {
                for (int i = 0; i < cboDrink26.Items.Count; i++)
                {
                    cboDrink26.SelectedIndex = i;
                    if (VB.Left(strData, 1) == VB.Left(cboDrink26.Text, 1))
                    {
                        cboDrink26.SelectedIndex = i;
                        break;
                    }
                }
                txtDrink26.Text = VB.Pstr(strData, ";", 2);
                for (int j = 0; j < cboDrinkUnit6.Items.Count; j++)
                {
                    cboDrinkUnit6.SelectedIndex = j;
                    if (VB.Pstr(strData, ";", 3) == cboDrinkUnit6.Text)
                    {
                        cboDrinkUnit6.SelectedIndex = j;
                        break;
                    }
                }
            }

            //2021-02-03
            //최대음주4
            strData = list2.TMUN0127;
            if (!strData.IsNullOrEmpty())
            {
                for (int i = 0; i < cboDrink29.Items.Count; i++)
                {
                    cboDrink29.SelectedIndex = i;
                    if (VB.Left(strData, 1) == VB.Left(cboDrink29.Text, 1))
                    {
                        cboDrink29.SelectedIndex = i;
                        break;
                    }
                }
                txtDrink29.Text = VB.Pstr(strData, ";", 2);
                for (int j = 0; j < cboDrinkUnit9.Items.Count; j++)
                {
                    cboDrinkUnit9.SelectedIndex = j;
                    if (VB.Pstr(strData, ";", 3) == cboDrinkUnit9.Text)
                    {
                        cboDrinkUnit9.SelectedIndex = j;
                        break;
                    }
                }
            }
            //최대음주5
            strData = list2.TMUN0128;
            if (!strData.IsNullOrEmpty())
            {
                for (int i = 0; i < cboDrink30.Items.Count; i++)
                {
                    cboDrink30.SelectedIndex = i;
                    if (VB.Left(strData, 1) == VB.Left(cboDrink30.Text, 1))
                    {
                        cboDrink30.SelectedIndex = i;
                        break;
                    }
                }
                txtDrink30.Text = VB.Pstr(strData, ";", 2);
                for (int j = 0; j < cboDrinkUnit10.Items.Count; j++)
                {
                    cboDrinkUnit10.SelectedIndex = j;
                    if (VB.Pstr(strData, ";", 3) == cboDrinkUnit10.Text)
                    {
                        cboDrinkUnit10.SelectedIndex = j;
                        break;
                    }
                }
            }

            //신체활동
            txtActive11.Text = list2.T_ACTIVE1;
            txtActive12.Text = VB.Pstr(list2.TMUN0011, ":", 1);
            txtActive13.Text = VB.Pstr(list2.TMUN0011, ":", 2);
            txtActive21.Text = list2.T_ACTIVE2;
            txtActive22.Text = VB.Pstr(list2.TMUN0012, ":", 1);
            txtActive23.Text = VB.Pstr(list2.TMUN0012, ":", 2);
            txtActive31.Text = list2.T_ACTIVE3;

            //만66세 생애
            if (Fstr인지능력 == "OK")
            {
                txt66Inject.Text = list2.T66_INJECT;
                lbl66Inject.Text = hm.READ_Munjin_Name_New("건강검진", "예아니오", txt66Inject.Text.Trim());
                txt66Inject2.Text = list2.TMUN0013;
                lbl66Inject2.Text = hm.READ_Munjin_Name_New("건강검진", "예아니오", txt66Inject2.Text.Trim());

                txt66Stat1.Text = list2.T66_STAT1;
                lbl66Stat1.Text = hm.READ_Munjin_Name_New("건강검진", "예아니오", txt66Stat1.Text.Trim());
                txt66Stat2.Text = list2.T66_STAT2;
                lbl66Stat2.Text = hm.READ_Munjin_Name_New("건강검진", "예아니오", txt66Stat2.Text.Trim());
                txt66Stat3.Text = list2.T66_STAT3;
                lbl66Stat3.Text = hm.READ_Munjin_Name_New("건강검진", "예아니오", txt66Stat3.Text.Trim());
                txt66Stat4.Text = list2.T66_STAT4;
                lbl66Stat4.Text = hm.READ_Munjin_Name_New("건강검진", "예아니오", txt66Stat4.Text.Trim());
                txt66Stat5.Text = list2.T66_STAT5;
                lbl66Stat5.Text = hm.READ_Munjin_Name_New("건강검진", "예아니오", txt66Stat5.Text.Trim());
                txt66Stat6.Text = list2.T66_STAT6;
                lbl66Stat6.Text = hm.READ_Munjin_Name_New("건강검진", "예아니오", txt66Stat6.Text.Trim());

                txt66Fall.Text = list2.T66_FALL;
                lbl66Fall.Text = hm.READ_Munjin_Name_New("건강검진", "예아니오", txt66Fall.Text.Trim());
                txt66Uro.Text = list2.T66_URO;
                lbl66Uro.Text = hm.READ_Munjin_Name_New("건강검진", "예아니오", txt66Uro.Text.Trim());
            }

            //생활습관 개선필요
            //for (int i = 0; i <= 5; i++)
            //{
            //    CheckBox chkHabit = (Controls.Find("chkHabit" + i.ToString(), true)[0] as CheckBox);
            //    if (list2.HABIT == "1")
            //    {
            //        chkHabit.Checked = true;
            //    }
            //    else
            //    {
            //        chkHabit.Checked = false;
            //    }
            //}
            if (list2.HABIT1 == "1")
            {
                chkHabit1.Checked = true;
            }
            else
            {
                chkHabit1.Checked = false;
            }
            if (list2.HABIT2 == "1")
            {
                chkHabit2.Checked = true;
            }
            else
            {
                chkHabit2.Checked = false;
            }
            if (list2.HABIT3 == "1")
            {
                chkHabit3.Checked = true;
            }
            else
            {
                chkHabit3.Checked = false;
            }
            if (list2.HABIT4 == "1")
            {
                chkHabit4.Checked = true;
            }
            else
            {
                chkHabit4.Checked = false;
            }
            if (list2.HABIT5 == "1")
            {
                chkHabit5.Checked = true;
            }
            else
            {
                chkHabit5.Checked = false;
            }

            //외상,휴유증
            if (list2.JINCHAL1 == "1")
            {
                rdoJinchal11.Checked = true;
            }
            else if (list2.JINCHAL1 == "2")
            {
                rdoJinchal12.Checked = true;
            }

            //일반상태(양호,보통,불량)
            switch (list2.JINCHAL2)
            {
                case "1":
                    rdoJinchal21.Checked = true;
                    break;
                case "2":
                    rdoJinchal22.Checked = true;
                    break;
                case "3":
                    rdoJinchal23.Checked = true;
                    break;
                default:
                    break;
            }

            txtPanDrNo.Text = list2.MUNJINDRNO.ToString();
            lblDrName.Text = hb.READ_License_DrName(long.Parse(txtPanDrNo.Text));

            btnDelete.Enabled = true;
            if (list2.PANJENGDRNO != 0)
            {
                btnDelete.Enabled = false;
            }
        }

        void fn_DB_Update_Main(string argGbn)
        {
            string[] strSick1 = new string[2];
            string[] strSick2 = new string[2];
            string[] strSick3 = new string[2];
            string[] strSick4 = new string[2];
            string[] strSick5 = new string[2];
            string[] strSick6 = new string[2];
            string[] strSick7 = new string[2];


            string[] strGajok = new string[6];
            string strBLiver = "";
            string[] strSmoke = new string[8];
            string[] strDrink = new string[20];
            string[] strActive = new string[5];

            string[] str40Feel = new string[4];

            string str66Inject = "";
            string str66Inject2 = "";
            string[] str66Stat = new string[6];
            string[] str66Feel = new string[3];
            string[] str66Mem = new string[5];
            string str66Fall = "";
            string str66Uro = "";

            string strGbHabit;
            string[] strHabit = new string[5];
            string strJinchal1 = "";
            string strJinchal2 = "";
            string strGbOldByeng   = "";
            string stroldByengName = "";
            string[] strOldByeng = new string[7];

            string[] strSmoke1 = new string[8];

            int result = 0;

            if (clsType.User.IdNumber != "28048" && clsType.User.IdNumber != "16341")
            {
                if (hicJepsuService.GetCountbyPanjengDrNoWrtNo(FnWRTNO) > 0)
                {
                    MessageBox.Show("판정완료 수검자 입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            //변수 Clear
            for (int i = 0; i < 2; i++)
            {   
                strSick1[i] = "2";
                strSick2[i] = "2";
                strSick3[i] = "2";
                strSick4[i] = "2";
                strSick5[i] = "2";
                strSick6[i] = "2";
                strSick7[i] = "2";
            }

            for (int i = 0; i < 6; i++)
            {
                strGajok[i] = "";
            }

            strBLiver = "";

            for (int i = 0; i < 7; i++)
            {
                strSmoke[i] = "";
            }

            for (int i = 0; i < 8; i++)
            {
                strSmoke1[i] = "";
            }

            for (int i = 0; i < 20; i++)
            {
                strDrink[i] = "";
            }

            for (int i = 0; i < 5; i++)
            {
                strActive[i] = "";
            }

            for (int i = 0; i < 4; i++)
            {
                str40Feel[i] = "";
            }

            str66Inject = "";
            str66Inject2 = "";

            for (int i = 0; i < 6; i++)
            {
                str66Stat[i] = "";
            }

            for (int i = 0; i < 3; i++)
            {
                str66Feel[i] = "";
            }

            for (int i = 0; i < 5; i++)
            {
                str66Mem[i] = "";
            }

            str66Fall = "";
            str66Uro = "";

            //변수값 저장

            //생활습관 개선필요
            strGbHabit = "1";
            for (int i = 0; i <= 4; i++)
            {
                CheckBox chkHabit = (Controls.Find("chkHabit" + (i + 1).ToString(), true)[0] as CheckBox);
                if (chkHabit.Checked == true)
                {
                    strGbHabit = "2";
                    strHabit[i] = "1";
                }
                else
                {
                    strHabit[i] = "0";
                }
            }

            //일반상태, 외상후유증
            strJinchal1 = "";
            strJinchal2 = "";

            if (rdoJinchal11.Checked == true) strJinchal1 = "1";
            if (rdoJinchal12.Checked == true) strJinchal1 = "2";
            if (rdoJinchal21.Checked == true) strJinchal2 = "1";
            if (rdoJinchal22.Checked == true) strJinchal2 = "2";
            if (rdoJinchal23.Checked == true) strJinchal2 = "3";

            //과거병력
            strGbOldByeng = "1";
            for (int i = 0; i <= 6; i++)
            {
                CheckBox chkOldByeng = (Controls.Find("chkOldByeng" + (i + 1).ToString(), true)[0] as CheckBox);
                if (chkOldByeng.Checked == true)
                {
                    strOldByeng[i] = "1";
                    strGbOldByeng = "2";
                }
                else
                {
                    strOldByeng[i] = "0";
                }
            }
            stroldByengName = txtOldByengName.Text.Trim().Replace("'", "`");
            if (!stroldByengName.IsNullOrEmpty())
            {
                strOldByeng[6] = "1";
            }

            //현재상태
            if (chkSick11.Checked == true) strSick1[0] = "1";
            if (chkSick13.Checked == true) strSick1[1] = "1";
            if (chkSick21.Checked == true) strSick2[0] = "1";
            if (chkSick23.Checked == true) strSick2[1] = "1";
            if (chkSick31.Checked == true) strSick3[0] = "1";
            if (chkSick33.Checked == true) strSick3[1] = "1";
            if (chkSick41.Checked == true) strSick4[0] = "1";
            if (chkSick43.Checked == true) strSick4[1] = "1";
            if (chkSick51.Checked == true) strSick5[0] = "1";
            if (chkSick53.Checked == true) strSick5[1] = "1";
            if (chkSick61.Checked == true) strSick6[0] = "1";
            if (chkSick63.Checked == true) strSick6[1] = "1";
            if (chkSick71.Checked == true) strSick7[0] = "1";
            if (chkSick73.Checked == true) strSick7[1] = "1";
            //가족병력
            if (chkGajok1.Checked == true) strGajok[0] = "1";
            if (chkGajok2.Checked == true) strGajok[1] = "1";
            if (chkGajok3.Checked == true) strGajok[2] = "1";
            if (chkGajok4.Checked == true) strGajok[3] = "1";
            if (chkGajok5.Checked == true) strGajok[4] = "1";

            strBLiver = VB.Left(cboBTypeHepatitis.Text, 1);
            if (!strBLiver.IsNullOrEmpty())
            {
                if (string.Compare(strBLiver, "3") > 0)
                {
                    MessageBox.Show("B형 간염 항원보유자 값 오류", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            //흡연
            strSmoke[0] = VB.Left(cboSmoking1.Text, 1);
            strSmoke[1] = txtSmoke1_1.Text.Trim();
            strSmoke[2] = txtSmoke1_2.Text.Trim();
            strSmoke[3] = txtSmoke2_1.Text.Trim();
            strSmoke[4] = txtSmoke2_2.Text.Trim();
            strSmoke[5] = VB.Left(cboSmoking2.Text, 1);
            strSmoke[6] = VB.Left(cboSmoking3.Text, 1);
            //2019년도 추가사항
            strSmoke[7] = txtSmoke3_1.Text.Trim();

            strSmoke1[0] = VB.Left(cboSmoking4.Text, 1);
            strSmoke1[1] = txtSmoke4_1.Text.Trim();
            strSmoke1[2] = txtSmoke4_2.Text.Trim();
            strSmoke1[3] = txtSmoke5_1.Text.Trim();
            strSmoke1[4] = txtSmoke5_2.Text.Trim();
            strSmoke1[5] = txtSmoke6_1.Text.Trim();
            strSmoke1[6] = VB.Left(cboSmoking5.Text, 1);
            strSmoke1[7] = VB.Left(cboSmoking6.Text, 1);

            //음주
            strDrink[0] = VB.Left(cboDrink1.Text, 1);
            strDrink[1] = txtDrink1.Text;
            //for (int i = 0; i <= 5; i++)
            //{
            //    ComboBox cboDrink2 = (Controls.Find("cboDrink2" + (i + 1).ToString(), true)[0] as ComboBox);
            //    ComboBox cboDrinkUnit = (Controls.Find("cboDrinkUnit" + (i + 1).ToString(), true)[0] as ComboBox);
            //    TextBox txtDrink2 = (Controls.Find("txtDrink2" + (i + 1).ToString(), true)[0] as TextBox);
            //    if (!cboDrink2.Text.IsNullOrEmpty())
            //    {
            //        strDrink[i + 2] = VB.Left(cboDrink2.Text, 1) + ";" + txtDrink2.Text + ";" + cboDrinkUnit.Text;
            //    }
            //}

            //2021-02-03
            for (int i = 0; i <= 9; i++)
            {
                ComboBox cboDrink2 = (Controls.Find("cboDrink" + (i + 21).ToString(), true)[0] as ComboBox);
                ComboBox cboDrinkUnit = (Controls.Find("cboDrinkUnit" + (i + 1).ToString(), true)[0] as ComboBox);
                TextBox txtDrink2 = (Controls.Find("txtDrink" + (i + 21).ToString(), true)[0] as TextBox);
                if (!cboDrink2.Text.IsNullOrEmpty())
                {
                    strDrink[i + 2] = VB.Left(cboDrink2.Text, 1) + ";" + txtDrink2.Text + ";" + cboDrinkUnit.Text;
                }
            }

            //신체활동
            strActive[0] = txtActive11.Text.Trim();
            strActive[1] = txtActive21.Text.Trim();
            strActive[2] = txtActive31.Text.Trim();
            if (strActive[0].To<long>() != 0) strActive[3] = txtActive12.Text.Trim() + ":" + txtActive13.Text.Trim();
            if (strActive[1].To<long>() != 0) strActive[4] = txtActive22.Text.Trim() + ":" + txtActive23.Text.Trim();

            //만66세
            str66Inject = txt66Inject.Text.Trim();
            str66Inject2 = txt66Inject2.Text.Trim();

            str66Stat[0] = txt66Stat1.Text.Trim();
            str66Stat[1] = txt66Stat2.Text.Trim();
            str66Stat[2] = txt66Stat3.Text.Trim();
            str66Stat[3] = txt66Stat4.Text.Trim();
            str66Stat[4] = txt66Stat5.Text.Trim();
            str66Stat[5] = txt66Stat6.Text.Trim();

            str66Fall = txt66Fall.Text.Trim();
            str66Uro = txt66Uro.Text.Trim();

            //자료 저장 및 갱신
            if (FstrROWID.IsNullOrEmpty())
            {
                result = hicResBohum1Service.Insert(FnWRTNO);

                if (result < 0)
                {
                    MessageBox.Show("저장중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            HIC_RES_BOHUM1 item = new HIC_RES_BOHUM1();

            item.WRTNO = FnWRTNO;
            item.T_STAT01 = strSick1[0];
            item.T_STAT02 = strSick1[1];
            item.T_STAT11 = strSick2[0];
            item.T_STAT12 = strSick2[1];
            item.T_STAT21 = strSick3[0];
            item.T_STAT22 = strSick3[1];
            item.T_STAT31 = strSick4[0];
            item.T_STAT32 = strSick4[1];
            item.T_STAT41 = strSick5[0];
            item.T_STAT42 = strSick5[1];
            item.T_STAT51 = strSick6[0];
            item.T_STAT52 = strSick6[1];
            item.T_STAT61 = strSick7[0];
            item.T_STAT62 = strSick7[1];
            item.T_GAJOK1 = strGajok[0];
            item.T_GAJOK2 = strGajok[1];
            item.T_GAJOK3 = strGajok[2];
            item.T_GAJOK4 = strGajok[3];
            item.T_GAJOK5 = strGajok[4];
            item.T_BLIVER = strBLiver;
            item.T_SMOKE1 = strSmoke[0];
            item.T_SMOKE2 = strSmoke[1].To<long>();
            item.T_SMOKE3 = strSmoke[2].To<long>();
            item.T_SMOKE4 = strSmoke[3].To<long>();
            item.T_SMOKE5 = strSmoke[4].To<long>();
            item.T_DRINK1 = strDrink[0];
            item.T_DRINK2 = strDrink[1].To<long>();
            item.T_ACTIVE1 = strActive[0];
            item.T_ACTIVE2 = strActive[1];
            item.T_ACTIVE3 = strActive[2];
            item.T66_INJECT = str66Inject;
            item.T66_STAT1 = str66Stat[0];
            item.T66_STAT2 = str66Stat[1];
            item.T66_STAT3 = str66Stat[2];
            item.T66_STAT4 = str66Stat[3];
            item.T66_STAT5 = str66Stat[4];
            item.T66_STAT6 = str66Stat[5];
            item.T66_FALL = str66Fall;
            item.T66_URO = str66Uro;
            item.TMUN0001 = strSmoke[5];
            item.TMUN0002 = strSmoke[6];
            item.TMUN0003 = strDrink[0];
            item.TMUN0004 = strDrink[1];
            item.TMUN0005 = strDrink[2];
            item.TMUN0006 = strDrink[3];
            item.TMUN0007 = strDrink[4];
            item.TMUN0008 = strDrink[5];
            item.TMUN0009 = strDrink[6];
            item.TMUN0010 = strDrink[7];
            item.TMUN0011 = strActive[3];
            item.TMUN0012 = strActive[4];
            item.TMUN0013 = str66Inject2;
            item.TMUN0096 = strSmoke[7];
            item.TMUN0097 = strSmoke1[0];
            item.TMUN0098 = strSmoke1[1];
            item.TMUN0099 = strSmoke1[2];
            item.TMUN0100 = strSmoke1[3];
            item.TMUN0101 = strSmoke1[4];
            item.TMUN0102 = strSmoke1[5];
            item.TMUN0103 = strSmoke1[6];
            item.TMUN0104 = strSmoke1[7];

            //2021-02-03(보통,최대 음주 추가)
            item.TMUN0125 = strDrink[8];;
            item.TMUN0126 = strDrink[9];;
            item.TMUN0127 = strDrink[10];;
            item.TMUN0128 = strDrink[11]; ;

            item.SABUN = clsType.User.IdNumber.To<long>();

            result = hicResBohum1Service.UpdateResultbyWrtNo(item, argGbn);

            if (result < 0)
            {
                MessageBox.Show("저장중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string strGbMinjin = "";

            if (rdoJob1.Checked == true)
            {
                strGbMinjin = "신규";
            }
            else if (rdoJob2.Checked == true)
            {
                strGbMinjin = "수정";
            }
            else
            {
                strGbMinjin = "";
            }

            //접수마스타에 문진 등록 완료 SET
            result = hicJepsuService.UpdateGbMunJinbyWrtNo(FnWRTNO, argGbn, strGbMinjin);

            if (result < 0)
            {
                MessageBox.Show("접수마스타에 문진완료 등록중 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //스캔Data에 문진완료 SET
            if (argGbn == "저장")
            {
                result = comHpcLibBService.UpdateHicOmrInfobyWrtNo(FnWRTNO);

                if (result < 0)
                {
                    MessageBox.Show("OMR 스캔 DB에 문진완료 등록중 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            txtWrtNo.Text = "";
            fn_Screen_Clear();
            eBtnClick(btnSearch, new EventArgs());
            txtWrtNo.Focus();            
        }
    }
}
