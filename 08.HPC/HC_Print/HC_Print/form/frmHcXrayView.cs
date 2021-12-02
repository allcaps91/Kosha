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

namespace HC_Print
{
    public partial class frmHcXrayView : Form
    {

        HicPatientService hicPatientService = null;
        HicJepsuXrayResultService hicJepsuXrayResultService = null;

        clsSpread sp = new clsSpread();
        clsHaBase hb = new clsHaBase();

        public frmHcXrayView()
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
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SS2.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.rdoJob1.Click += new EventHandler(eRdoClick);
            this.rdoJob2.Click += new EventHandler(eRdoClick);
            this.rdoJob3.Click += new EventHandler(eRdoClick);
            this.rdoJob4.Click += new EventHandler(eRdoClick);
        }

        private void SetControl()
        {


            hicPatientService = new HicPatientService();
            hicJepsuXrayResultService = new HicJepsuXrayResultService();

            SSList.Initialize();
            SSList.ActiveSheet.ColumnHeader.Rows[0].Height = 30;
            //SSList.AddColumn("접수번호", nameof(HIC_MIR_ERROR_TONGBO.WRTNO), 88, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsSort = true});
            SSList.AddColumn("건진번호", nameof(HIC_PATIENT.PANO), 88, FpSpreadCellType.TextCellType);
            SSList.AddColumn("성명", nameof(HIC_PATIENT.SNAME), 88, FpSpreadCellType.TextCellType);
            SSList.AddColumn("주민번호", nameof(HIC_PATIENT.JUMIN), 150, FpSpreadCellType.TextCellType);
            SSList.AddColumn("병원번호", nameof(HIC_PATIENT.PTNO), 88, FpSpreadCellType.TextCellType);

        }

        private void eFormLoad(object sender, EventArgs e)
        {

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

        void eRdoClick(object sender, EventArgs e)
        {
            if (sender == rdoJob1)
            {
                label1.Text = "수진자명";
                txtSName.Text = "";
            }
            else if (sender == rdoJob2)
            {
                label1.Text = "주민번호";
                txtSName.Text = "";
            }
            else if (sender == rdoJob3)
            {
                label1.Text = "회사명";
                txtSName.Text = "";
            }
            else if (sender == rdoJob4)
            {
                label1.Text = "건진번호";
                txtSName.Text = "";
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SSList)
            {
                long nPano = 0;
                string strSname = "";
                string strJumin = "";
                string strXrayNo = "";
                int nRow = 0;
                int nREAD = 0;
                long nWRTNO = 0;

                sp.Spread_All_Clear(SS2);

                nPano = long.Parse(SSList.ActiveSheet.Cells[e.Row, 0].Text);
                strSname = SSList.ActiveSheet.Cells[e.Row, 1].Text;
                strJumin = SSList.ActiveSheet.Cells[e.Row, 2].Text;

                HIC_PATIENT list = hicPatientService.GetJusobyPano(nPano, "");

                if (!list.IsNullOrEmpty())
                {
                    SS1.ActiveSheet.Cells[0, 1].Text = nPano.ToString();    //종검번호
                    SS1.ActiveSheet.Cells[0, 3].Text = strSname;    //수진자명
                    if (VB.Mid(strJumin, 7, 1) == "1" || VB.Mid(strJumin, 7, 1) == "3") //성별
                    {
                        SS1.ActiveSheet.Cells[1, 1].Text = "남";
                    }
                    else if (VB.Mid(strJumin, 7, 1) == "2" || VB.Mid(strJumin, 7, 1) == "4")
                    {
                        SS1.ActiveSheet.Cells[1, 1].Text = "여";
                    }
                    SS1.ActiveSheet.Cells[1, 3].Text = VB.Left(strJumin, 2) + "년" + VB.Mid(strJumin, 3, 2) + "월" + VB.Mid(strJumin, 5, 2) + "일";
                    SS1.ActiveSheet.Cells[2, 1].Text = VB.Left(strJumin, 6) + "-" + VB.Right(strJumin, 7);
                    SS1.ActiveSheet.Cells[2, 3].Text = list.TEL;
                    SS1.ActiveSheet.Cells[3, 1].Text = list.JUSO1 + " " + list.JUSO2;
                    SS1.ActiveSheet.Cells[4, 1].Text = hb.READ_Ltd_Name(list.LTDCODE.ToString());
                }



                //영상자료
                List<HIC_JEPSU_XRAY_RESULT> list2 = hicJepsuXrayResultService.GetListbyPaNo(nPano);

                nREAD = list2.Count;
                SS2.ActiveSheet.RowCount = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    //nWRTNO = list2[i].WRTNO;
                    //SS2.ActiveSheet.Cells[i, 0].Text = nWRTNO.ToString();
                    if (list2[i].GBREAD == "1")
                    {
                        SS2.ActiveSheet.Cells[i, 0].Text = "일반";
                    }
                    else
                    {
                        SS2.ActiveSheet.Cells[i, 0].Text = "분진";
                    }
                    strXrayNo = list2[i].XRAYNO;
                    SS2.ActiveSheet.Cells[i, 3].Text = strXrayNo;

                    List<HIC_JEPSU_XRAY_RESULT> list3 = hicJepsuXrayResultService.GetListbyXrayNo(strXrayNo);

                    if (list3.Count > 0)
                    {
                        SS2.ActiveSheet.Cells[i, 1].Text = list3[0].WRTNO;
                        SS2.ActiveSheet.Cells[i, 2].Text = list3[0].JEPDATE;
                        SS2.ActiveSheet.Cells[i, 4].Text = list3[0].GJJONG;
                        SS2.ActiveSheet.Cells[i, 5].Text = list3[0].RESULT2;
                        SS2.ActiveSheet.Cells[i, 6].Text = list3[0].RESULT4;
                    }
                }
            }
            else if (sender == SS2)
            {
                //txtRemark.Text = SS2.ActiveSheet.Cells[e.Row, 6].Text;
            }

        }

        private void Screen_Display(FpSpread Spd)
        {

            int nRow = 0;
            string strGubun = "";
            string strItem = "";

            sp.Spread_All_Clear(SSList);


            if (txtSName.Text.Trim() == "") return;
            strItem = txtSName.Text;

            if (rdoJob1.Checked == true)
            {
                strGubun = "2";
            }
            else if (rdoJob2.Checked == true)
            {
                strGubun = "3";
            }
            else if (rdoJob3.Checked == true)
            {
                strGubun = "4";
            }
            else if (rdoJob4.Checked == true)
            {
                strGubun = "5";
            }

            List<HIC_PATIENT> list = hicPatientService.GetPanobyItem(strItem, strGubun);

            Spd.ActiveSheet.RowCount = 0;
            nRow = list.Count;
            SSList.ActiveSheet.RowCount = nRow;

            for (int i = 0; i < nRow; i++)
            {
                SSList.ActiveSheet.Cells[i, 0].Text = list[i].PANO.ToString(); ;
                SSList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                SSList.ActiveSheet.Cells[i, 2].Text = list[i].JUMIN;
                SSList.ActiveSheet.Cells[i, 3].Text = list[i].PTNO;

            }
        }
    }
}
