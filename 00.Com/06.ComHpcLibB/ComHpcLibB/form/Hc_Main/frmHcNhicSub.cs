using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using System;
using System.Windows.Forms;
/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHcNhicSub.cs
/// Description     : 자격조회 Sub
/// Author          : 김민철
/// Create Date     : 2019-08-21
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm자격sub.frm(Frm자격sub)" />
namespace ComHpcLibB
{
    public partial class frmHcNhicSub : Form
    {
        WorkNhicService workNhicService = null;
        
        int FnCnt = 0;
        string FstrSName = string.Empty;
        string FstrJumin = string.Empty;
        string FstrYear = string.Empty;
        string FstrGbn = string.Empty;
        string FstrPtno = string.Empty;

        public frmHcNhicSub()
        {
            InitializeComponent();
            SetControl();
            SetEvents();
        }

        public frmHcNhicSub(string argSName, string argJumin, string argYear, string argGbn, string argPtno)
        {
            InitializeComponent();
            SetControl();
            SetEvents();

            FstrSName = argSName;
            FstrJumin = argJumin;
            FstrYear = argYear;
            FstrGbn = argGbn;
            if (FstrGbn.IsNullOrEmpty()) { FstrGbn = "H"; }
            FstrPtno = argPtno;
        }

        private void SetEvents()
        {
            this.Load       += new EventHandler(eFormLoad);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);
            this.timer.Tick += new EventHandler(eTimertick);
        }

        private void eFormClosed(object sender, FormClosedEventArgs e)
        {
            timer.Stop();
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            timer.Start();
        }

        private void SetControl()
        {
            workNhicService = new WorkNhicService();
        }

        private void eTimertick(object sender, EventArgs e)
        {
            timer.Stop();
            FnCnt = FnCnt + 1;
            lblTime.Text = FnCnt.To<string>();

            if (FnCnt >= 10)
            {
                FnCnt = 0;
                timer.Stop();
                this.Close();
                return;
            }

            //당일 조회된 자격내역이 있는지 점검
            WORK_NHIC item = workNhicService.GetNhicInfoByTime("", clsAES.AES(FstrJumin), FstrSName, FstrYear, "1", "C");

            if (!item.IsNullOrEmpty())
            {
                FnCnt = 0;
                timer.Stop();
                this.Close();
                return;
            }
            else
            {

                WORK_NHIC item_sub = workNhicService.GetNhicInfoByTime("", clsAES.AES(FstrJumin), FstrSName, FstrYear, "", "R");

                if (item_sub.IsNullOrEmpty())
                {
                    WORK_NHIC item2 = new WORK_NHIC
                    {
                        GUBUN = FstrGbn,
                        SNAME = FstrSName,
                        JUMIN = VB.Left(FstrJumin, 7) + "******",
                        JUMIN2 = clsAES.AES(FstrJumin),
                        PANO = FstrPtno,
                        GBSTS = "0",
                        YEAR = FstrYear
                    };

                    int result = workNhicService.InsertData(item2);

                    #region 이전 로직 기준
                    //WORK_NHIC item_sub2 = workNhicService.GetNhicInfoByTime("", clsAES.AES(FstrJumin), "", FstrYear, "2", "R");

                    //if (item_sub2.IsNullOrEmpty())
                    //{
                    //    WORK_NHIC item2 = new WORK_NHIC
                    //    {
                    //        GUBUN = FstrGbn,
                    //        SNAME = FstrSName,
                    //        JUMIN = VB.Left(FstrJumin, 7) + "******",
                    //        JUMIN2 = clsAES.AES(FstrJumin),
                    //        PANO = FstrPtno,
                    //        GBSTS = "0",
                    //        YEAR = FstrYear
                    //    };

                    //    int result = workNhicService.InsertData(item2);
                    //} 
                    #endregion
                }
            }

            //Thread.Sleep(2000);
            timer.Start();

        }
    }
}
