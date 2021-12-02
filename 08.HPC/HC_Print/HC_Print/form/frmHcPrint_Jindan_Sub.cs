using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Print
/// File Name       : frmHcPrint_Jindan_Sub.cs
/// Description     : 건강진단서
/// Author          : 김경동
/// Create Date     : 2021-02-19
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmJindan_new.frm(FrmIDateChange)" />

namespace HC_Print
{
    public partial class frmHcPrint_Jindan_Sub : Form
    {

        long fnWrtno = 0;

        ComFunc cf = new ComFunc();
        clsHaBase hb = new clsHaBase();
        clsSpread cSpd = new clsSpread();

        HicJepsuPatientService hicJepsuPatientService = null;

        public frmHcPrint_Jindan_Sub()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        public frmHcPrint_Jindan_Sub(long argWRTNO)
        {
            InitializeComponent();
            SetControl();
            SetEvent();

            fnWrtno = argWRTNO;
        }
        private void SetControl()
        {
            hicJepsuPatientService = new HicJepsuPatientService();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormload);
        }

        private void eFormload(object sender, EventArgs e)
        {
            this.Hide();

            Result_Print_Sub();

            ComFunc.Delay(1500);
            this.Close();

        }

        private void Result_Print_Sub()
        {



            //출력
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "";
            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Auto, PrintType.All, 0, 0, false, false, true, true, false, false, true);

            cSpd.setSpdPrint(SSPrint, PrePrint, setMargin, setOption, strHeader, strFooter);
        }


    }
}
