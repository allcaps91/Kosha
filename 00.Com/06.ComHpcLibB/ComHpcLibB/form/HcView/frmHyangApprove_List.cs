using ComBase;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ComHpcLibB
{
    public partial class frmHyangApprove_List : Form
    {
        clsHcMain hm = new clsHcMain();

        HicHyangApproveService hicHyangApproveService = null;
        public frmHyangApprove_List()
        {
            InitializeComponent();
            SetControl();
            SetEvents();
        }

        private void SetControl()
        {
            hicHyangApproveService = new HicHyangApproveService();
        }

        private void SetEvents()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);
            dtpDate.Text = DateTime.Now.AddDays(0).ToShortDateString();
        }

        void eBtnClick(object sender, EventArgs e)
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

        private void Screen_Display(FpSpread Spd)
        {
            int nRow = 0;

            string strSDate = "";
            string strGubun = "";

            if (rdoJob2.Checked == true)
            {
                strGubun = "2";
            }
            else if (rdoJob3.Checked == true)
            {
                strGubun = "1";
            }

            strSDate = dtpDate.Value.ToShortDateString();

            List<HIC_HYANG_APPROVE> list = hicHyangApproveService.GetItembySDateDeptCodeSite(strSDate, "HR", strGubun);

            Spd.ActiveSheet.RowCount = 0;
            nRow = list.Count;
            SSList.ActiveSheet.RowCount = nRow;

            for (int i = 0; i < nRow; i++)
            {
                SSList.ActiveSheet.Cells[i, 0].Text = list[i].WRTNO.ToString();
                SSList.ActiveSheet.Cells[i, 1].Text = list[i].PANO.ToString();
                SSList.ActiveSheet.Cells[i, 2].Text = list[i].SNAME;
                SSList.ActiveSheet.Cells[i, 3].Text = list[i].AGE.ToString()+"/"+ list[i].SEX;
                if (list[i].GBSITE == " 1")
                {
                    SSList.ActiveSheet.Cells[i, 4].Text = "종검";
                }
                else
                {
                    SSList.ActiveSheet.Cells[i, 4].Text = "외래";
                }
                SSList.ActiveSheet.Cells[i, 5].Text = list[i].SUCODE;
                SSList.ActiveSheet.Cells[i, 6].Text = VB.Format(Convert.ToInt32(list[i].QTY), "#0.00");
                SSList.ActiveSheet.Cells[i, 7].Text = "1";
                SSList.ActiveSheet.Cells[i, 8].Text = hm.READ_DrName_Sabun(Convert.ToInt32(list[i].DRSABUN));
                SSList.ActiveSheet.Cells[i, 9].Text = list[i].APPROVETIME;
            }
        }
    }
}
