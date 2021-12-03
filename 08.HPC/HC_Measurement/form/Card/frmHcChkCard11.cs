using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;
using HC_Core;
using HC_Measurement.Dto;
using HC_Measurement.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace HC_Measurement
{
    public partial class frmHcChkCard11 :CommonForm
    {
        clsSpread cSpd = null;
        Button[] arrBtns = new Button[63];

        HicChukDtlDstrbService hicChukDtlDstrbService = null;

        int currentPosition = 0;
        long FnWRTNO = 0;

        public frmHcChkCard11()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHcChkCard11(long nWRTNO)
        {
            InitializeComponent();
            SetEvent();
            SetControl();

            FnWRTNO = nWRTNO;
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.txtDistrb_Accdt.Leave += new EventHandler(eTxtLeave);
        }


        private void SetControl()
        {
            cSpd = new clsSpread();
            hicChukDtlDstrbService = new HicChukDtlDstrbService();

            for (int i = 0; i < 63; i++)
            {
                arrBtns[i] = (Controls.Find("btnChr" + (i + 1).ToString(), true)[0] as Button);
                arrBtns[i].Click += new EventHandler(eBtnClick);
            }
            
        }

        private void eTxtLeave(object sender, EventArgs e)
        {
            currentPosition = txtDistrb_Accdt.SelectionStart;
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            txtDistrb_Accdt.Text = txtDistrb_Accdt.Text.Insert(txtDistrb_Accdt.SelectionStart, ((Button)sender).Text);
            txtDistrb_Accdt.Focus();
            txtDistrb_Accdt.Select(currentPosition + 1, 0);
        }

        void Screen_Display(long nWRTNO)
        {
            HIC_CHUKDTL_DSTRB item = hicChukDtlDstrbService.GetItemByWrtno(nWRTNO);

            if (!item.IsNullOrEmpty())
            {
                txtDistrb_Accdt.Text = item.REMARK.To<string>("");
            }

        }

        private void Data_Save()
        {
            try
            {
                

                MessageBox.Show("저장완료. ");
                Screen_Clear();
            }
            catch (Exception)
            {
                MessageBox.Show("저장실패. ", "오류");
                return;
            }
        }

        private void Screen_Clear()
        {
            txtDistrb_Accdt.Text = "";
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            if (FnWRTNO > 0)
            {
                Screen_Display(FnWRTNO);
            }
        }
    }
}
