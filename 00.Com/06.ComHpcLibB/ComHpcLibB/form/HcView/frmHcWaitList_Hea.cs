using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ComHpcLibB
{
    public partial class frmHcWaitList_Hea : Form
    {
        HicWaitService hicWaitService = null;
        HeaJepsuPatientService heaJepsuPatientService = null;
        HeaSunapService heaSunapService = null;
        HeaSunapdtlService heaSunapdtlService = null;
        HicPatientLtdService hicPatientLtdService = null;
        HicPatientService hicPatientService = null;
        
        public delegate void SetGstrValue(string GstrTempValue1, string GstrTempValue2, string GstrTempValue3);
        public static event SetGstrValue rSetGstrValue;

        ComFunc CF = new ComFunc();
        clsHaBase hb = new clsHaBase();

        public frmHcWaitList_Hea()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.chkAll.CheckedChanged += new EventHandler(eChkChanged);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDbClick);
        }

        private void eChkChanged(object sender, EventArgs e)
        {
            Screen_Display(SSList);
        }

        private void SetControl()
        {
            hicWaitService = new HicWaitService();
            heaJepsuPatientService = new HeaJepsuPatientService();
            heaSunapService = new HeaSunapService();
            heaSunapdtlService = new HeaSunapdtlService();
            hicPatientLtdService = new HicPatientLtdService();
            hicPatientService = new HicPatientService();
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            Screen_Display(SSList);
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                Screen_Display(SSList);
            }
            else if (sender == btnExit)
            {
                this.Close(); 
                return;
            }
        }
        private void eSpdDbClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true)
            {
                return;
            }

            if (sender == SSList)
            {
                string sValue1 = "";
                string sValue2 = "";
                string sValue3 = "";

                sValue1 = SSList.ActiveSheet.Cells[e.Row, 0].Text;      //순번
                sValue2 = SSList.ActiveSheet.Cells[e.Row, 1].Text;      //이름
                sValue3 = SSList.ActiveSheet.Cells[e.Row, 4].Text;     //주민번호

                if (rSetGstrValue.IsNullOrEmpty())
                {
                    this.Close();
                }
                else
                {
                    rSetGstrValue(sValue1, sValue2, sValue3);
                    this.Close();
                }
                
            }
        }

        private void Screen_Display(FpSpread Spd)
        {
            int nREAD = 0;
            int nSeqNo = 0;
            string strJumin = "";
            string strYear = "";
            string strLtdName = "";
            string strCodeNM = "";

            strYear = VB.Left(clsPublic.GstrSysDate, 4);

            List<HIC_WAIT> list = hicWaitService.GetItembyJobDate(clsPublic.GstrSysDate, chkAll.Checked, "1");
            Spd.ActiveSheet.RowCount = 0;
            nREAD = list.Count;
            SSList.ActiveSheet.RowCount = nREAD;

            for (int i = 0; i < nREAD; i++)
            {

                strLtdName = "";
                strJumin = list[i].JUMIN2.Trim();
                nSeqNo = (int)list[i].SEQNO;

                HEA_JEPSU_PATIENT haJEP = heaJepsuPatientService.GetItemByJumin2(strJumin);

                if (!haJEP.IsNullOrEmpty())
                {
                    if (!haJEP.LTDNAME.IsNullOrEmpty())
                    {
                        strLtdName = haJEP.LTDNAME.To<string>("");
                    }
                    
                    SSList.ActiveSheet.Cells[i, 0].Text = nSeqNo.ToString();
                    SSList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                    SSList.ActiveSheet.Cells[i, 2].Text = list[i].JEPTIME;
                    SSList.ActiveSheet.Cells[i, 3].Text = strLtdName;
                    SSList.ActiveSheet.Cells[i, 4].Text = clsAES.DeAES(haJEP.JUMIN2.To<string>("").Trim());

                    //명품검진,골드검진,VIP검진 표시
                    if (haJEP.GJJONG == "11" || haJEP.GJJONG == "12")
                    {
                        strCodeNM = heaSunapdtlService.CheckVipByWRTNOLikeCodeName(haJEP.WRTNO);

                        if (!strCodeNM.IsNullOrEmpty())
                        {
                            SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(0, 255, 255);
                        }
                    }

                    //금액 150만원 이상 고액 종검자
                    long nAMT = heaSunapService.GetSumTotAmtByWrtno(haJEP.WRTNO);

                    if (nAMT > 1500000)
                    {
                        SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(0, 255, 255);
                    }
                }
            }
        }
    }
}
