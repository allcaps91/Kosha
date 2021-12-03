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
    public partial class frmHcChkCard10 :CommonForm
    {
        clsSpread cSpd = null;
        
        long FnWRTNO = 0;

        public frmHcChkCard10()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHcChkCard10(long nWRTNO)
        {
            InitializeComponent();
            SetEvent();
            SetControl();

            FnWRTNO = nWRTNO;
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
           
        }

       
        private void SetControl()
        {
            cSpd = new clsSpread();
        }

        void Screen_Display(long nWRTNO)
        {
          
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
