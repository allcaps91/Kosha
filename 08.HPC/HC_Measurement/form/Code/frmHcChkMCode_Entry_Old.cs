using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using HC.Core.Dto;
using HC.Core.Service;
using HC_Core;
using HC_Measurement.Dto;
using HC_Measurement.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HC_Measurement
{
    public partial class frmHcChkMCode_Entry_Old :CommonForm
    {
        HicChkMcodeService hicChkMcodeService = null;
        HicChkUcodeService hicChkUcodeService = null;
        HicCodeService hicCodeService = null;
        HcCodeService hcCodeService = null;

        string FstrRowid = string.Empty;

        public frmHcChkMCode_Entry_Old()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHcChkMCode_Entry_Old(string argRid)
        {
            InitializeComponent();
            SetEvent();
            SetControl();

            FstrRowid = argRid;
        }

        private void SetControl()
        {
            hicChkMcodeService = new HicChkMcodeService();
            hicChkUcodeService = new HicChkUcodeService();
            hicCodeService = new HicCodeService();
            hcCodeService = new HcCodeService();

            //분류
            List<HIC_CHK_UCODE> lstBun = hicChkUcodeService.GetItemAll();
            cboBun.SetOptions(new ComboBoxOption { DataField = nameof(HIC_CHK_MCODE.BUN) });
            cboBun.SetItems2(lstBun, "NAME", "CODE", true, true, "", "", AddComboBoxPosition.Top);

            //분석분류
            List<HIC_CODE> lstAnalBun = hicCodeService.GetCodeNamebyGubun("C0");
            cboAnalBun.SetOptions(new ComboBoxOption { DataField = nameof(HIC_CHK_MCODE.ANAL_BUN) });
            cboAnalBun.SetItems2(lstAnalBun, "NAME", "CODE", true, true, "", "", AddComboBoxPosition.Top);

            //주기
            nmrCycle.SetOptions(new NumericUpDownOption { DataField = nameof(HIC_CHK_MCODE.CYCLE), Min = 0 });

            //Ceiling Unit
            List<HIC_CODE> lstUnit3 = hicCodeService.GetCodeNamebyGubun("C2");
            cboCeilingUnit.SetOptions(new ComboBoxOption { DataField = nameof(HIC_CHK_MCODE.CEILING_UNIT) });
            cboCeilingUnit.SetItems2(lstUnit3, "NAME", "CODE", true, true, "", "", AddComboBoxPosition.Top);

            //측정방법
            List<HIC_CODE> lstChkWay = hicCodeService.GetCodeNamebyGubun("15");
            cboChkWay.SetOptions(new ComboBoxOption { DataField = nameof(HIC_CHK_MCODE.CHK_WAY) });
            cboChkWay.SetItems2(lstChkWay, "NAME", "CODE", true, true, "", "", AddComboBoxPosition.Top);

            //측정단위
            List<HC_CODE> lstUnit4 = hcCodeService.GetComboBoxData("CRITERIA_UNIT", "WEM");
            cboChkUnit.SetOptions(new ComboBoxOption { DataField = nameof(HIC_CHK_MCODE.CHK_UNIT) });
            cboChkUnit.SetItems2(lstUnit4, "CODENAME", "CODE", true, true, "", "", AddComboBoxPosition.Top);

            //측정여부
            chkGbChk.SetOptions(new CheckBoxOption { DataField = nameof(HIC_CHK_MCODE.GBCHK), CheckValue = "Y", UnCheckValue = "N" });
            //특검여부
            chkGbSpc.SetOptions(new CheckBoxOption { DataField = nameof(HIC_CHK_MCODE.GBSPC), CheckValue = "Y", UnCheckValue = "N" });
            //혼합물여부
            chkGbMix.SetOptions(new CheckBoxOption { DataField = nameof(HIC_CHK_MCODE.GBMIX), CheckValue = "Y", UnCheckValue = "N" });
            //발암성여부
            chkGbCan.SetOptions(new CheckBoxOption { DataField = nameof(HIC_CHK_MCODE.GBCAN), CheckValue = "Y", UnCheckValue = "N" });
            //사용여부
            chkGbUse.SetOptions(new CheckBoxOption { DataField = nameof(HIC_CHK_MCODE.GBUSE), CheckValue = "Y", UnCheckValue = "N" });

            panMain.SetEnterKey();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);

            this.txtCode.KeyDown += new KeyEventHandler(eTxtKeyDown);
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            this.panMain.AddRequiredControl(txtCode);
            this.panMain.AddRequiredControl(txtFullName);

            panMain.Initialize();

            if (!FstrRowid.IsNullOrEmpty())
            {
                Screen_Display(FstrRowid);
            }
        }

        private void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            if (sender == txtCode)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (txtCode.Text.Trim() != "")
                    {
                        txtCode.Text = VB.Format(txtCode.Text.To<long>(0), "####0");
                        Screen_Display_Code(txtCode.Text.Trim());
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


                HIC_CHK_MCODE item = panMain.GetData<HIC_CHK_MCODE>();

                item.RID = FstrRowid;
                item.ENTDATE = DateTime.Now;
                item.ENTSABUN = clsType.User.IdNumber.To<long>(0);

                if (!item.CODE.IsNullOrEmpty())
                {
                    if (hicChkMcodeService.Save(item))
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
        }

        private void Screen_Display(string argRid)
        {
            HIC_CHK_MCODE item = hicChkMcodeService.GetItemByRid(argRid);

            panMain.SetData(item);
        }

        private void Screen_Display_Code(string argCode)
        {
            HIC_CHK_MCODE item = hicChkMcodeService.GetItemByCode(argCode);

            if (!item.IsNullOrEmpty())
            {
                panMain.SetData(item);
                FstrRowid = item.RID;
            }
        }

    }
}
