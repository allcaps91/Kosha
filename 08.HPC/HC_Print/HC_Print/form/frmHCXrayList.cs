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
    public partial class frmHCXrayList : Form
    {

        clsHaBase cHB = null;
        clsSpread sp = new clsSpread();
        HicXrayResultService hicXrayResultService = null;

        string strFDate = "";
        string strTDate = "";
        string strGubun = "";

        public frmHCXrayList()
        {
            InitializeComponent();
            SetControl();
            SetEvents();
        }

        private void SetControl()
        {
            hicXrayResultService = new HicXrayResultService();

            SSList.Initialize();
            SSList.ActiveSheet.ColumnHeader.Rows[0].Height = 30;
            //SSList.AddColumn("접수번호", nameof(HIC_MIR_ERROR_TONGBO.WRTNO), 88, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsSort = true});
            SSList.AddColumn("접수일자", nameof(HIC_XRAY_RESULT.JEPDATE), 88, FpSpreadCellType.TextCellType);
            SSList.AddColumn("회사명", nameof(HIC_XRAY_RESULT.LTDCODE), 150, FpSpreadCellType.TextCellType);
            SSList.AddColumn("출장구분", nameof(HIC_XRAY_RESULT.GBCHUL), 88, FpSpreadCellType.TextCellType);
            SSList.AddColumn("촬영건수", nameof(HIC_XRAY_RESULT.CNT), 88, FpSpreadCellType.TextCellType);

        }

        private void SetEvents()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);
            dtpFDate.Text = DateTime.Now.AddDays(-1).ToShortDateString();
            dtpTDate.Text = DateTime.Now.AddDays(-1).ToShortDateString();
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                Screen_Display(SSList);
            }

            else if (sender == btnPrint)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "일반검진 Xray 명단";
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);
                sp.setSpdPrint(SSList, PrePrint, setMargin, setOption, strHeader, strFooter);

            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
        }

        private void Screen_Display(FpSpread Spd)
        {

            ComFunc CF = null;
            CF = new ComFunc();

            clsHaBase hb = new clsHaBase();

            int nRow = 0;
            long nSUM = 0;

            strFDate = dtpFDate.Value.ToShortDateString();
            strTDate = dtpTDate.Value.ToShortDateString();

            if (rdoJob2.Checked == true)
            {
                strGubun = "내원";
            }
            else if (rdoJob3.Checked == true)
            {
                strGubun = "출장";
            }


            List<HIC_XRAY_RESULT> list = hicXrayResultService.GetItemByJepDate(strFDate, strTDate, strGubun);

            Spd.ActiveSheet.RowCount = 0;
            nRow = list.Count;
            SSList.ActiveSheet.RowCount = nRow + 1;

            for (int i = 0; i < nRow; i++)
            {
                SSList.ActiveSheet.Cells[i, 0].Text = list[i].JEPDATE.ToString();
                SSList.ActiveSheet.Cells[i, 1].Text = hb.READ_Ltd_One_Name(list[i].LTDCODE.ToString());
                SSList.ActiveSheet.Cells[i, 2].Text = list[i].GBCHUL;
                SSList.ActiveSheet.Cells[i, 3].Text = list[i].CNT.ToString();
                nSUM = nSUM + list[i].CNT;
            }

            //합산표시
            SSList.ActiveSheet.Cells[nRow, 1].Text = "합산";
            SSList.ActiveSheet.Cells[nRow, 3].Text = nSUM.ToString();
        }
        
    }
}
