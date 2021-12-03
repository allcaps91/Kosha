using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using HC_Core;
using HC_Measurement.Dto;
using HC_Measurement.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HC_Measurement
{
    public partial class frmHcChkSuga_Entry :CommonForm
    {
        HicChkSugaService hicChkSugaService = null;
        HicCodeService hicCodeService = null;

        string FstrRowid = string.Empty;
        string FstrComCode = string.Empty;
        string FstrComName = string.Empty;
        string FstrComGCode = string.Empty;
        string FstrComGCode1 = string.Empty;

        public frmHcChkSuga_Entry()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHcChkSuga_Entry(string argRid)
        {
            InitializeComponent();
            SetEvent();
            SetControl();

            FstrRowid = argRid;
        }

        private void SetControl()
        {
            hicChkSugaService = new HicChkSugaService();
            hicCodeService = new HicCodeService();

            ////측정코드
            //List<HIC_CODE> lstChkCD = hicCodeService.GetCodeNamebyGubun("15");
            //cboChkCode.SetOptions(new ComboBoxOption { DataField = nameof(HIC_CHK_SUGA.CHKCODE) });
            //cboChkCode.SetItems2(lstChkCD, "NAME", "CODE", true, true, "", "", AddComboBoxPosition.Top);

            //사용여부
            chkGbUse.SetOptions(new CheckBoxOption { DataField = nameof(HIC_CHK_SUGA.GBUSE), CheckValue = "Y", UnCheckValue = "N" });

            panMain.SetEnterKey();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnMCode.Click += new EventHandler(eBtnClick);
            this.btnHelp.Click += new EventHandler(eBtnClick);
            this.txtSuCode.KeyDown += new KeyEventHandler(eTxtKeyDown);
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            this.panMain.AddRequiredControl(txtSuCode);
            this.panMain.AddRequiredControl(txtSuName);

            panMain.Initialize();
            panSub01.Initialize();

            int nYY = DateTime.Now.ToShortDateString().Substring(0, 4).To<int>();
            nmrGjYear.Value = nYY;
            chkGbUse.Checked = true;
            txtChkCode.Text = "";

            if (!FstrRowid.IsNullOrEmpty())
            {
                Screen_Display(FstrRowid);
            }
        }

        private void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            if (sender == txtSuCode)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (txtSuCode.Text.Trim() != "")
                    {
                        Screen_Display_Code(txtSuCode.Text.Trim());
                    }
                }
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSave)
            {
                if (!panMain.RequiredValidate())
                {
                    MessageBox.Show("필수 입력항목이 누락되었습니다.");
                    return;
                }

                //TODO : 코드중복 확인


                HIC_CHK_SUGA item = panMain.GetData<HIC_CHK_SUGA>();
                item.CHKCODE = txtChkCode.Text.Trim();
                item.RID = FstrRowid;
                item.ENTDATE = DateTime.Now;
                item.ENTSABUN = clsType.User.IdNumber.To<long>(0);
                item.GAMT = nmrGAmt.Value.To<long>(0);

                if (!item.SUCODE.IsNullOrEmpty())
                {
                    if (hicChkSugaService.Save(item))
                    {
                        MessageBox.Show("저장하였습니다");
                        this.Close();
                        return;
                    }
                    else
                    {
                        MessageBox.Show("오류가 발생하였습니다. ");
                        return;
                    }
                }
            }
            else if (sender == btnMCode)
            {
                frmHcChkCodeHelp frm = new frmHcChkCodeHelp("MCODE", "");
                frm.rSetGstrValue += new frmHcChkCodeHelp.SetGstrValue(eValueCode);
                frm.ShowDialog();
                frm.rSetGstrValue -= new frmHcChkCodeHelp.SetGstrValue(eValueCode);

                if (!FstrComCode.IsNullOrEmpty())
                {
                    txtMCode.Text = FstrComCode.Trim();

                    lblMCodeName.Text = FstrComName.Trim();
                }
                else
                {
                    txtMCode.Text = "";
                    lblMCodeName.Text = "";
                }
            }
            else if (sender == btnHelp)
            {
                frmHcChkCodeHelp frm = new frmHcChkCodeHelp("ANAL", "");
                frm.rSetGstrValue += new frmHcChkCodeHelp.SetGstrValue(eValueCode);
                frm.ShowDialog();
                frm.rSetGstrValue -= new frmHcChkCodeHelp.SetGstrValue(eValueCode);

                if (!FstrComCode.IsNullOrEmpty())
                {
                    txtChkCode.Text = FstrComCode.Trim();

                    lblHang1.Text = FstrComName.Trim();
                    lblHang2.Text = FstrComGCode.Trim();
                    lblHang3.Text = FstrComGCode1.Trim();
                }
                else
                {
                    txtMCode.Text = "";
                    lblHang1.Text = "";
                    lblHang2.Text = "";
                    lblHang3.Text = "";
                }
            }
        }

        private void eValueCode(string strCode, string strName, string strGCode, string strGCode1)
        {
            FstrComCode = strCode;
            FstrComName = strName;
            FstrComGCode = strGCode;
            FstrComGCode1 = strGCode1;
        }

        private void Screen_Display(string argRid)
        {
            HIC_CHK_SUGA item = hicChkSugaService.GetItemByRid(argRid);

            panMain.SetData(item);

            lblMCodeName.Text = item.MCODE_NM;
            lblHang1.Text = item.HANG1;
            lblHang2.Text = item.HANG2;
            lblHang3.Text = item.HANG3;
        }

        private void Screen_Display_Code(string argCode)
        {
            HIC_CHK_SUGA item = hicChkSugaService.GetItemByCode(argCode);

            if (!item.IsNullOrEmpty())
            {
                panMain.SetData(item);
                FstrRowid = item.RID;
                lblMCodeName.Text = item.MCODE_NM;
                lblHang1.Text = item.HANG1;
                lblHang2.Text = item.HANG2;
                lblHang3.Text = item.HANG3;
            }
        }

    }
}
