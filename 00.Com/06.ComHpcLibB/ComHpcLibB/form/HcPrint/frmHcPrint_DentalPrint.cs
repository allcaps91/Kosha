using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComHpcLibB
{
    public partial class frmHcPrint_DentalPrint : Form
    {

        HicResDentalService hicResDentalService = null;

        long FnWRTNO = 0;
        long FnDrno = 0;
        string FstrTongboGbn = "";
        string FstrGbSend = "";

        int i = 0;
        int j = 0;
        string strBanGi = "";
        string strGbn = "";
        string strGjjong = "";
        string strJepdate = "";
        string strUpdateOK = "";
        string strREC = "";
        string FstrParam;


        public frmHcPrint_DentalPrint()
        {
            InitializeComponent();
            SetControl();
            SetEvents();
        }

        /// <summary>
        /// 2020.09.19 이상훈 생성자 추가
        /// </summary>
        /// <param name="strParam"></param>
        public frmHcPrint_DentalPrint(string strParam)
        {
            InitializeComponent();
            FstrParam = strParam;
            SetControl();
            SetEvents();
        }

        private void SetControl()
        {
            hicResDentalService = new HicResDentalService();
        }

        private void SetEvents()
        {
            this.Load += new EventHandler(eFormLoad);
            this.timer1.Tick += new EventHandler(eTimertick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            FnWRTNO = VB.Pstr(FstrParam, ";", 1).To<long>();
            FstrTongboGbn = VB.Pstr(FstrParam, ";", 2);
            FstrGbSend = VB.Pstr(FstrParam, ";", 3);

            if (FstrTongboGbn.IsNullOrEmpty())
            {
                FstrTongboGbn = "2";
            }

            if (FstrGbSend.IsNullOrEmpty())
            {
                FstrGbSend = "P";
            }

            timer1.Start();

        }
        private void eTimertick(object sender, EventArgs e)
        {

            timer1.Stop();

            Print_Den_Report();

        }

        void Print_Den_Report()
        {

            

            Result_Print_Main_1();
            Result_Print_Sub1();
            Result_Print_Main_2();

        }

        void Result_Print_Main_1()
        {

            if (FstrTongboGbn =="")
            {
                FstrTongboGbn = "2";
            }


            HIC_RES_DENTAL item = hicResDentalService.GetItemByWrtno(FnWRTNO);

            if (!item.IsNullOrEmpty())
            {
                if (item.GBPRINT != "Y" )
                {
                    strUpdateOK = "OK";
                }

                if (item.TONGBODATE == "")
                {
                    strUpdateOK = "OK";
                }

                if (item.TONGBOGBN == "")
                {
                    strUpdateOK = "OK";
                }


            }
            else
            {
                strUpdateOK = "OK";
            }
        }
        void Result_Print_Sub1()
        {

        }
        void Result_Print_Main_2()
        {

        }


    }
}
