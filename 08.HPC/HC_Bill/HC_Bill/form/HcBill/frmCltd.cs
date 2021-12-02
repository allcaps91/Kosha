using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace HC_Bill
{
    public partial class frmCltd : Form
    {

        clsSpread sp = new clsSpread();
        HicJepsuMirCancerService HicJepsuMirCancerService = null;
        HicJepsuMirCancerBoService HicJepsuMirCancerBoService = null;

        HIC_LTD LtdHelpItem = null;

        string strFDate = "";
        string strTDate = "";
        string strYear = "";
        string strJohap = "";
        string strJong = "";
        string strLtdcode = "";

        public frmCltd()
        {
            InitializeComponent();
            SetEvents();
            SetControl();
        }

        private void SetControl()
        {
            HicJepsuMirCancerService = new HicJepsuMirCancerService();
            HicJepsuMirCancerBoService = new HicJepsuMirCancerBoService();

            SSList.Initialize();
            SSList.ActiveSheet.ColumnHeader.Rows[0].Height = 30;
            //SSList.AddColumn("접수번호", nameof(HIC_MIR_ERROR_TONGBO.WRTNO), 88, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsSort = true});
            SSList.AddColumn("청구번호", nameof(HIC_JEPSU_MIR_CANCER.MIRNO), 88, FpSpreadCellType.TextCellType);
            SSList.AddColumn("청구일자", nameof(HIC_JEPSU_MIR_CANCER.JEPDATE), 88, FpSpreadCellType.TextCellType);
            SSList.AddColumn("접수일자", nameof(HIC_JEPSU_MIR_CANCER.JEPDATE), 100, FpSpreadCellType.TextCellType);
            SSList.AddColumn("접수번호", nameof(HIC_JEPSU_MIR_CANCER.WRTNO), 88, FpSpreadCellType.TextCellType);
            SSList.AddColumn("성명", nameof(HIC_JEPSU_MIR_CANCER.SNAME), 88, FpSpreadCellType.TextCellType);
            SSList.AddColumn("나이", nameof(HIC_JEPSU_MIR_CANCER.AGE), 88, FpSpreadCellType.TextCellType);
            SSList.AddColumn("검진종류", nameof(HIC_JEPSU_MIR_CANCER.GJJONG), 88, FpSpreadCellType.TextCellType);
            SSList.AddColumn("차수", nameof(HIC_JEPSU_MIR_CANCER.GJCHASU), 88, FpSpreadCellType.TextCellType);
            SSList.AddColumn("회사명", nameof(HIC_JEPSU_MIR_CANCER.LTDCODE), 88, FpSpreadCellType.TextCellType);
            SSList.AddColumn("KIHO", nameof(HIC_JEPSU_MIR_CANCER.KIHO), 88, FpSpreadCellType.TextCellType);
        }

        private void SetEvents()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnLtdHelp.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
        }



        void eFormLoad(object sender, EventArgs e)
        {

            int nYY = 0;
            txtLtdCode.Text = "";
            lblLtdName.Text = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-5).ToShortDateString();
            dtpTDate.Text = clsPublic.GstrSysDate;

            nYY = int.Parse(VB.Left(clsPublic.GstrSysDate, 4));
            for (int i = 0; i < 5; i++)
            {
                cboYear.Items.Add(string.Format("{0:0000}", nYY));
                nYY -= 1;
            }

            cboYear.SelectedIndex = 0;

            cboJong.Items.Clear();
            cboJong.Items.Add("4.공단암");
            cboJong.Items.Add("5.보건소암");
            cboJong.Items.Add("E.의료급여");
            cboJong.SelectedIndex = 0;

            cboJohap.Items.Clear();
            cboJohap.Items.Add("");
            cboJohap.Items.Add("사업장");
            cboJohap.Items.Add("공무원");
            cboJohap.Items.Add("성인병");
            cboJohap.SelectedIndex = 0;
        }


        private void eBtnClick(object sender, EventArgs e)
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

                strTitle = "청구오류명단";
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);
                sp.setSpdPrint(SSList, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
            else if (sender == btnLtdHelp)
            {
                Ltd_Code_Help();
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

            int nRow = 0;

            Spd.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            strFDate = dtpFDate.Value.ToShortDateString();
            strTDate = dtpTDate.Value.ToShortDateString();
            strYear = cboYear.Text;
            strJohap = cboJohap.Text;
            strJong = VB.Left(cboJong.Text,1);
            strLtdcode = txtLtdCode.Text;
            
            if (strJong == "E")
            {
                List<HIC_JEPSU_MIR_CANCER_BO> list = HicJepsuMirCancerBoService.GetListByItems(strYear, strFDate, strTDate, strJohap, strJong, strLtdcode);

                Spd.ActiveSheet.RowCount = 0;
                nRow = list.Count;
                SSList.ActiveSheet.RowCount = nRow;

                for (int i = 0; i < nRow; i++)
                {
                    SSList.ActiveSheet.Cells[i, 0].Text = list[i].WRTNO;
                    SSList.ActiveSheet.Cells[i, 1].Text = "청구일자";
                    SSList.ActiveSheet.Cells[i, 2].Text = "접수일자";
                    SSList.ActiveSheet.Cells[i, 3].Text = list[i].WRTNO;
                    SSList.ActiveSheet.Cells[i, 4].Text = list[i].SNAME;
                    SSList.ActiveSheet.Cells[i, 5].Text = list[i].AGE.ToString();
                    SSList.ActiveSheet.Cells[i, 6].Text = list[i].GJJONG;
                    SSList.ActiveSheet.Cells[i, 7].Text = list[i].GJCHASU;
                    SSList.ActiveSheet.Cells[i, 8].Text = list[i].LTDCODE;
                    SSList.ActiveSheet.Cells[i, 9].Text = list[i].KIHO;
                }
            }
            else
            {
                List<HIC_JEPSU_MIR_CANCER> list = HicJepsuMirCancerService.GetListByItems(strYear, strFDate, strTDate, strJohap, strJong, strLtdcode);

                Spd.ActiveSheet.RowCount = 0;
                nRow = list.Count;
                SSList.ActiveSheet.RowCount = nRow;

                for (int i = 0; i < nRow; i++)
                {
                    SSList.ActiveSheet.Cells[i, 0].Text = list[i].WRTNO;
                    SSList.ActiveSheet.Cells[i, 1].Text = "청구일자";
                    SSList.ActiveSheet.Cells[i, 2].Text = "접수일자";
                    SSList.ActiveSheet.Cells[i, 3].Text = list[i].WRTNO;
                    SSList.ActiveSheet.Cells[i, 4].Text = list[i].SNAME;
                    SSList.ActiveSheet.Cells[i, 5].Text = list[i].AGE.ToString();
                    SSList.ActiveSheet.Cells[i, 6].Text = list[i].GJJONG;
                    SSList.ActiveSheet.Cells[i, 7].Text = list[i].GJCHASU;
                    SSList.ActiveSheet.Cells[i, 8].Text = list[i].LTDCODE;
                    SSList.ActiveSheet.Cells[i, 9].Text = list[i].KIHO;
                }
            }

            //Spd.ActiveSheet.RowCount = 0;
            //nRow = list.Count;
            //SSList.ActiveSheet.RowCount = nRow;

            //for (int i = 0; i < nRow; i++)
            //{
            //    SSList.ActiveSheet.Cells[i, 0].Text = list[i].WRTNO;
            //    SSList.ActiveSheet.Cells[i, 1].Text = "청구일자";
            //    SSList.ActiveSheet.Cells[i, 2].Text = "접수일자";
            //    SSList.ActiveSheet.Cells[i, 3].Text = list[i].WRTNO;
            //    SSList.ActiveSheet.Cells[i, 4].Text = list[i].SNAME;
            //    SSList.ActiveSheet.Cells[i, 5].Text = list[i].AGE.ToString();
            //    SSList.ActiveSheet.Cells[i, 6].Text = list[i].GJJONG;
            //    SSList.ActiveSheet.Cells[i, 7].Text = list[i].GJCHASU;
            //    SSList.ActiveSheet.Cells[i, 8].Text = list[i].LTDCODE;
            //    SSList.ActiveSheet.Cells[i, 9].Text = list[i].KIHO;

            //}

        }
        private void ePost_value_LTD(HIC_LTD item)
        {
            LtdHelpItem = item;
        }
        /// <summary>
        /// 사업장 코드 검색창 연동
        /// </summary>
        private void Ltd_Code_Help()
        {
            frmHcLtdHelp frm = new frmHcLtdHelp();
            frm.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(ePost_value_LTD);
            frm.ShowDialog();

            if (!LtdHelpItem.IsNullOrEmpty())
            {
                txtLtdCode.Text = LtdHelpItem.CODE.To<string>();
                lblLtdName.Text = LtdHelpItem.SANGHO;
            }
            else
            {
                txtLtdCode.Text = "";
                lblLtdName.Text = "";
            }
        }
    }
}
