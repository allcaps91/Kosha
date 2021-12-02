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
    public partial class frmConfirmList : Form
    {
        clsSpread sp = new clsSpread();
        HicJepsuPatientService HicJepsuPatientService = null;
        HicResBohum1Service HicResBohum1Service = null;

        string strFDate = "";
        string strTDate = "";
        long nWrtno = 0;

        public frmConfirmList()
        {
            InitializeComponent();
            SetEvents();
            SetControl();
        }

        private void SetControl()
        {
            HicJepsuPatientService = new HicJepsuPatientService();
            HicResBohum1Service = new HicResBohum1Service();

            SSList.Initialize();
            SSList.ActiveSheet.ColumnHeader.Rows[0].Height = 30;
            //SSList.AddColumn("접수번호", nameof(HIC_MIR_ERROR_TONGBO.WRTNO), 88, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsSort = true});
            SSList.AddColumn("순번", nameof(HIC_JEPSU_PATIENT.WRTNO), 88, FpSpreadCellType.TextCellType);
            SSList.AddColumn("검진년도", nameof(HIC_JEPSU_PATIENT.GJYEAR), 88, FpSpreadCellType.TextCellType);
            SSList.AddColumn("주민등록번호", nameof(HIC_JEPSU_PATIENT.JUMIN2), 100, FpSpreadCellType.TextCellType);
            SSList.AddColumn("이름", nameof(HIC_JEPSU_PATIENT.SNAME), 88, FpSpreadCellType.TextCellType);
            SSList.AddColumn("접수일자", nameof(HIC_JEPSU_PATIENT.JEPDATE), 88, FpSpreadCellType.TextCellType);
            SSList.AddColumn("확진구분", nameof(HIC_JEPSU_PATIENT.JEPDATE), 88, FpSpreadCellType.TextCellType);
        }

        private void SetEvents()
        {
            //this.Load += new EventHandler(eFormload);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnExcel.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                Screen_Display(SSList);
            }
            else if (sender == btnExcel)
            {
                
                bool x = false;

                if (MessageBox.Show("엑셀파일로 만드시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                x = SSList.SaveExcel("C:\\청구작업.xls", FarPoint.Excel.ExcelSaveFlags.UseOOXMLFormat);

                if (x == true)
                {
                    MessageBox.Show("C:\\청구작업.xls" + " 엑셀파일이 생성되었습니다.", "작업완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("엑셀파일이 생성에 오류가 발생되었습니다.", "작업완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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



            //List<HIC_MIR_ERROR_TONGBO> list = hicMirErrorTongboService.GetListByItems(nWRTNO, strFDate, strTDate, strGubun, "0");
            IList<HIC_JEPSU_PATIENT> list = HicJepsuPatientService.GetNhicListByDate(strFDate, strTDate);

            //
           
            nRow = list.Count;
            SSList.ActiveSheet.RowCount = nRow;

            for (int i = 0; i < nRow; i++)
            {
                SSList.ActiveSheet.Cells[i, 0].Text = i.ToString();
                SSList.ActiveSheet.Cells[i, 1].Text = list[i].GJYEAR;
                SSList.ActiveSheet.Cells[i, 2].Text = clsAES.DeAES(list[i].JUMIN2);
                SSList.ActiveSheet.Cells[i, 3].Text = list[i].SNAME;
                SSList.ActiveSheet.Cells[i, 4].Text = list[i].JEPDATE;
                SSList.ActiveSheet.Cells[i, 5].Text = "";


            }

            //List<HIC_RES_BOHUM1> list1 = HicResBohum1Service.GetItemByWrtno(nWrtno);
        }
    }
}
