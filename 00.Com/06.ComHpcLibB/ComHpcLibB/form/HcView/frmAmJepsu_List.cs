using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComHpcLibB
{
    public partial class frmAmJepsu_List : Form
    {

        EndoJupmstService endoJupmstService = null;
        HicCancerResv1Service hicCancerResv1Service = null;
        HicCancerResv2Service hicCancerResv2Service = null;
        HicWaitService hicWaitService = null;

        clsPublic cpublic = new clsPublic();

        public frmAmJepsu_List()
        {
            InitializeComponent();
            SetControl();
            SetEvents();
        }


        private void SetControl()
        {
            endoJupmstService = new EndoJupmstService();
            hicCancerResv1Service = new HicCancerResv1Service();
            hicCancerResv2Service = new HicCancerResv2Service();
            hicWaitService = new HicWaitService();

            SSList1.Initialize();
            SSList1.ActiveSheet.ColumnHeader.Rows[0].Height = 30;
            SSList1.AddColumn("종류", nameof(HIC_WAIT.RID), 100, FpSpreadCellType.TextCellType);
            SSList1.AddColumn("정원", nameof(HIC_WAIT.RID), 50, FpSpreadCellType.TextCellType);
            SSList1.AddColumn("당일", nameof(HIC_WAIT.RID), 50, FpSpreadCellType.TextCellType);
            SSList1.AddColumn("도착인원", nameof(HIC_WAIT.RID), 60, FpSpreadCellType.TextCellType);
            SSList1.AddColumn("여유", nameof(HIC_WAIT.RID), 50, FpSpreadCellType.TextCellType);

            SSList2.Initialize();
            SSList2.ActiveSheet.ColumnHeader.Rows[0].Height = 30;
            SSList2.AddColumn("등록번호", nameof(ENDO_JUPMST.PTNO), 70, FpSpreadCellType.TextCellType);
            SSList2.AddColumn("성명", nameof(ENDO_JUPMST.SNAME), 70, FpSpreadCellType.TextCellType);
            SSList2.AddColumn("검사완료", nameof(ENDO_JUPMST.RESULTDATE), 70, FpSpreadCellType.TextCellType);
            SSList2.AddColumn("비고", nameof(ENDO_JUPMST.BDATE), 100, FpSpreadCellType.TextCellType);
        }

        private void SetEvents()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
        }


        void eFormLoad(object sender, EventArgs e)
        {
            //Load
            Screen_Display();
        }

        void eBtnClick(object sender, EventArgs e)
        {

            if (sender == btnSearch)
            {
                Screen_Display();
            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }

        }

        private void Screen_Display()
        {
            read_sysdate();
            lblTime.Text = cpublic.strSysDate + " " + cpublic.strSysTime;

            Screen_Display1(SSList1);

            Screen_Display2(SSList2);
        }


        void read_sysdate()
        {
            //시간확인
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        private void Screen_Display1(FpSpread Spd)
        {
            int nRow = 0;
            int[,] nCNT = new int[3, 6];

            string strFDate = "";
            string strTDate = "";

            string strAMPM = "";
            string strJumin = "";


            strFDate = DateTime.Now.AddDays(0).ToShortDateString();
            strTDate = DateTime.Now.AddDays(0).ToShortDateString();

            List<HIC_CANCER_RESV1> list = hicCancerResv1Service.GetItembyJobDate(strFDate, strTDate);

            Spd.ActiveSheet.RowCount = 0;
            nRow = list.Count;
            SSList1.ActiveSheet.RowCount = 3;

            if (DateTime.Now.Hour < 12)
            {
                nCNT[0, 0] = Convert.ToInt32(list[0].GFS);
                nCNT[1, 0] = Convert.ToInt32(list[0].GFSH);
                nCNT[2, 0] = Convert.ToInt32(list[0].UGI);

                strAMPM = "AM";
                strTDate = strTDate + " 12:00";
            }
            else
            {
                nCNT[0, 0] = Convert.ToInt32(list[0].GFS1);
                nCNT[1, 0] = Convert.ToInt32(list[0].GFSH1);
                nCNT[2, 0] = Convert.ToInt32(list[0].UGI1);

                strAMPM = "PM";
                strFDate = strFDate + " 12:01";
                strTDate = strTDate + " 23:59";
            }

            List<HIC_CANCER_RESV2> list2 = hicCancerResv2Service.GetItembyRTime3(strAMPM, strFDate, strTDate);

            nRow = list2.Count;

            for (int i = 0; i < nRow; i++)
            {
                //2)당일예약인원수
                if (list2[i].GBGFS == "Y") { nCNT[0, 1] = nCNT[0, 1] + 1; }
                if (list2[i].GBGFSH == "Y") { nCNT[1, 1] = nCNT[1, 1] + 1; }
                if (list2[i].GBUGI == "Y") { nCNT[2, 1] = nCNT[2, 1] + 1; }

                strJumin = list2[i].JUMIN2;

                List<HIC_WAIT> list3 = hicWaitService.GetListbyJobdateJumin(strFDate, strJumin);

                if (list3.Count > 0 && list3[0].RID != "")
                //if (list3[0].RID != "")
                {
                    //4)도착 인원수
                    if (list2[i].GBGFS == "Y") { nCNT[0, 3] = nCNT[0, 3] + 1; }
                    if (list2[i].GBGFSH == "Y") { nCNT[1, 3] = nCNT[1, 3] + 1; }
                    if (list2[i].GBUGI == "Y") { nCNT[2, 3] = nCNT[2, 3] + 1; }
                }
                else
                {
                    //5)미도착 인원수
                    if (list2[i].GBGFS == "Y") { nCNT[0, 5] = nCNT[0, 5] + 1; }
                    if (list2[i].GBGFSH == "Y") { nCNT[1, 5] = nCNT[1, 5] + 1; }
                    if (list2[i].GBUGI == "Y") { nCNT[2, 5] = nCNT[2, 5] + 1; }
                }

            }

            SSList1.ActiveSheet.Cells[0, 0].Text = "본관 내시경";
            SSList1.ActiveSheet.Cells[1, 0].Text = "종검 내시경";
            SSList1.ActiveSheet.Cells[2, 0].Text = "위조영촬영";

            for (int i =0; i < 3; i++)
            {

                nCNT[i, 2] = nCNT[i, 0] - nCNT[i, 1]; //여유1
                nCNT[i, 4] = nCNT[i, 0] - nCNT[i, 3]; //여유2
                nCNT[i, 5] = nCNT[i, 1] - nCNT[i, 3]; //미도착

                SSList1.ActiveSheet.Cells[i, 1].Text = VB.Format(nCNT[i, 0], "#0"); //정원
                SSList1.ActiveSheet.Cells[i, 2].Text = VB.Format(nCNT[i, 2], "#0"); //당일가능
                SSList1.ActiveSheet.Cells[i, 3].Text = VB.Format(nCNT[i, 3], "#0"); //도착인원
                SSList1.ActiveSheet.Cells[i, 4].Text = VB.Format(nCNT[i, 4], "#0"); //여유
            }

        }

        private void Screen_Display2(FpSpread Spd)
        {
            int nRow = 0;
            string strBDate = "";

            strBDate = DateTime.Now.AddDays(0).ToShortDateString();
            List<ENDO_JUPMST> list = endoJupmstService.GetListbyBdateDeptcodeBuse(strBDate, "HR", "044500");

            Spd.ActiveSheet.RowCount = 0;
            nRow = list.Count;
            SSList2.ActiveSheet.RowCount = nRow;

            for (int i = 0; i < nRow; i++)
            {

                SSList2.ActiveSheet.Cells[i, 0].Text = list[i].PTNO;
                SSList2.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                if(list[i].RESULTDATE != "")
                {
                    SSList2.ActiveSheet.Cells[i, 2].Text = "완료";
                }
            }
        }
    }
}
