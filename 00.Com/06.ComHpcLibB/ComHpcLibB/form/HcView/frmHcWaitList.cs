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
    public partial class frmHcWaitList : Form
    {
        HicWaitService hicWaitService = null;
        HicJepsuPatientService hicJepsuPatientService = null;
        HicJepsuWorkService hicJepsuWorkService = null;
        HicCancerResv2Service hicCancerResv2Service = null;
        HicPatientLtdService hicPatientLtdService = null;

        public delegate void SetGstrValue(string GstrTempValue1, string GstrTempValue2, string GstrTempValue3);
        public static event SetGstrValue rSetGstrValue;

        ComFunc CF = new ComFunc();
        clsHaBase hb = new clsHaBase();

        public frmHcWaitList()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnDelete.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDbClick);
        }

        private void SetControl()
        {
            hicWaitService = new HicWaitService();
            hicJepsuPatientService = new HicJepsuPatientService();
            hicJepsuWorkService = new HicJepsuWorkService();
            hicCancerResv2Service = new HicCancerResv2Service();
            hicPatientLtdService = new HicPatientLtdService();
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
            else if (sender == btnDelete)
            {
                int nSeqno = 0;
                int nREAD = 0;
                string strJOBDATE = "";

                nREAD = SSList_Sheet1.RowCount;

                for (int i = 0; i < nREAD; i++)
                {
                    if (SSList.ActiveSheet.Cells[i, 16].Text == "True" )
                    {
                        if (ComFunc.MsgBoxQ("선택한 것을 삭제하시겠습니까?", "작업확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                        {
                            return;
                        }

                        nSeqno = Convert.ToInt32(SSList.ActiveSheet.Cells[i, 0].Text);
                        strJOBDATE = clsPublic.GstrSysDate;

                        int result = hicWaitService.DeleteBySeqNo(nSeqno, strJOBDATE);

                        if (result > 0)
                        {
                            MessageBox.Show("삭제 하였습니다.");
                            return;
                        }
                    }
                }
            }
        
            if (sender == btnExit)
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
                sValue3 = SSList.ActiveSheet.Cells[e.Row, 15].Text;     //주민번호

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
            string strAesJumin = "";
            string strYear = "";
            string strFDATE = "";
            string strTDATE = "";
            string strMundate = "";

            strYear = VB.Left(clsPublic.GstrSysDate, 4);
            strMundate = strYear + "-01-01";

            List<HIC_WAIT> list = hicWaitService.GetItembyJobDate1(clsPublic.GstrSysDate, strMundate);
            Spd.ActiveSheet.RowCount = 0;
            nREAD = list.Count;
            SSList.ActiveSheet.RowCount = nREAD;

            for (int i = 0; i < nREAD; i++)
            {

                strJumin = hb.Read_Jumin_Decrypt(list[i].JUMIN2, list[i].JUMIN);
                strAesJumin = clsAES.AES(strJumin);

                nSeqNo = (int)list[i].SEQNO;

                SSList.ActiveSheet.Cells[i, 0].Text = nSeqNo.ToString();
                SSList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                SSList.ActiveSheet.Cells[i, 2].Text = list[i].JEPTIME;
                SSList.ActiveSheet.Cells[i, 3].Text = (hb.READ_HIC_AGE_GESAN2(strJumin)).ToString();
                SSList.ActiveSheet.Cells[i, 15].Text = strJumin;

                HIC_JEPSU_PATIENT item1 = hicJepsuPatientService.GetItembyJuMin(strAesJumin, list[i].JOBDATE);
                if (!item1.IsNullOrEmpty())
                {
                    SSList.ActiveSheet.Cells[i, 4].BackColor = Color.Yellow;
                }
                else
                {
                    SSList.ActiveSheet.Cells[i, 4].BackColor = Color.White;
                }
                SSList.ActiveSheet.Cells[i, 2].BackColor = Color.White;

                List<HIC_JEPSU_WORK> item2 = hicJepsuWorkService.GetItembyJuMin(strAesJumin, strYear);
                if (item2.Count > 0)
                {
                    SSList.ActiveSheet.Cells[i, 1].BackColor = Color.Aqua;
                }
                strFDATE = clsPublic.GstrSysDate;
                strTDATE = CF.DATE_ADD(clsDB.DbCon, clsPublic.GstrSysDate, 1);

                HIC_CANCER_RESV2 item3 = hicCancerResv2Service.GetItembyJumin(strAesJumin, strFDATE, strTDATE);

                if (!item3.IsNullOrEmpty())
                {
                    if (item3.GBUGI == "Y" ) { SSList.ActiveSheet.Cells[i, 5].Text = "◎"; }
                    if (item3.GBGFS == "Y") { SSList.ActiveSheet.Cells[i, 6].Text = "◎"; }
                    if (item3.GBGFSH == "Y" ) { SSList.ActiveSheet.Cells[i, 7].Text = "◎"; }
                    if (item3.GBMAMMO == "Y") { SSList.ActiveSheet.Cells[i, 8].Text = "◎"; }
                    if (item3.GBRECUTM == "Y") { SSList.ActiveSheet.Cells[i, 9].Text = "◎"; }
                    if (item3.GBSONO == "Y") { SSList.ActiveSheet.Cells[i, 10].Text = "◎"; }
                    if (item3.GBWOMB == "Y") { SSList.ActiveSheet.Cells[i, 11].Text = "◎"; }
                    if (item3.GBBOHUM == "Y") { SSList.ActiveSheet.Cells[i, 12].Text = "◎"; }
                    if (item3.GBCT == "Y") { SSList.ActiveSheet.Cells[i, 13].Text = "◎"; }
                    SSList.ActiveSheet.Cells[i, 14].Text = item3.SDOCT;

                    //if (item3.GBSONO == "Y")
                    //{
                    //    SSList.ActiveSheet.Cells[i, 1].BackColor = Color.Pink;
                    //}

                    if (item3.GBGFS == "Y" || (item3.GBGFSH == "Y" || item3.GBCT =="Y"))
                    {
                        SSList.ActiveSheet.Cells[i, 1].BackColor = Color.HotPink;
                    }
                    else
                    {
                        SSList.ActiveSheet.Cells[i, 1].BackColor = Color.Gold;
                    }
                }

                HIC_PATIENT_LTD item4 = hicPatientLtdService.GetItembyJumin(strAesJumin); ;
                if (!item4.IsNullOrEmpty())
                {
                    SSList.ActiveSheet.Cells[i, 17].Text = item4.NAME;
                }

                SSList.ActiveSheet.Cells[i, 18].Text = list[i].IEMUNYN;
            }
           
            //종합검진 대기인원 표시함
            HIC_WAIT item5 = hicWaitService.GetItembyJobDate2(clsPublic.GstrSysDate, "1");
            if (!item5.IsNullOrEmpty())
            {
                lblHea.Text = "종검대기인원:" + item5.CNT + " 명";
            }
        }
    }
}
