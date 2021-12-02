using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;
using HC_Core;
using HC_Measurement.Dto;
using HC_Measurement.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace HC_Measurement
{
    public partial class frmHcChkCard08 :CommonForm
    {
        clsSpread cSpd = null;
        HicChukMstNewService hicChukMstNewService = null;
        HicChukDtlLocationService hicChukDtlLocationService = null;
        
        long FnWRTNO = 0;

        public frmHcChkCard08()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHcChkCard08(long nWRTNO)
        {
            InitializeComponent();
            SetEvent();
            SetControl();

            FnWRTNO = nWRTNO;
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnAdd_Add.Click += new EventHandler(eBtnClick);
        }

       
        private void SetControl()
        {
            cSpd = new clsSpread();
            hicChukMstNewService = new HicChukMstNewService();
            hicChukDtlLocationService = new HicChukDtlLocationService();

            #region 측정사업장 측정위치도
            SSList.Initialize(new SpreadOption { RowHeaderVisible = false, RowHeight = 26, ColumnHeaderHeight = 38 });
            SSList.AddColumn("일련번호", nameof(HIC_CHUKDTL_LOCATION.WRTNO),          74, new SpreadCellTypeOption { IsVisivle = false });
            SSList.AddColumnNumber("순번", nameof(HIC_CHUKDTL_LOCATION.SEQNO),          40, new SpreadCellTypeOption { IsSort = true, sortIndicator = SortIndicator.Ascending });
            SSList.AddColumn("파일명", nameof(HIC_CHUKDTL_LOCATION.FILENAME), 120, new SpreadCellTypeOption { });
            SSList.AddColumn("파일설명", nameof(HIC_CHUKDTL_LOCATION.REMARK), 240, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumnImage("이미지", nameof(HIC_CHUKDTL_LOCATION.IMAGEDATA), 280, new SpreadCellTypeOption { });
            SSList.AddColumnButton("H", 64, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += ssList_Delete;
            SSList.AddColumnButton("H", 78, new SpreadCellTypeOption { ButtonText = "미리보기"}).ButtonClick += ssList_Display_Image;
            SSList.AddColumn("ROWID", nameof(HIC_CHUKDTL_LOCATION.RID),        74, new SpreadCellTypeOption { IsVisivle = false });                                 
            #endregion
        }

        private void ssList_Display_Image(object sender, EditorNotifyEventArgs e)
        {
            string strRid = SSList.ActiveSheet.Cells[e.Row, 7].Text.Trim();

            if (!strRid.IsNullOrEmpty())
            {
                pictureBox1.Image = null;
                tabPage1.Text = "위치도";

                HIC_CHUKDTL_LOCATION saved = hicChukDtlLocationService.FindImageByRowid(strRid);

                if (!saved.IsNullOrEmpty())
                {
                    pictureBox1.Image = byteArrayToImage(saved.IMAGEDATA);
                    tabPage1.Text = saved.FILENAME;
                }
            }
        }

        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn, 0, byteArrayIn.Length);
            ms.Write(byteArrayIn, 0, byteArrayIn.Length);
            return Image.FromStream(ms, true);
        }

        private void ssList_Delete(object sender, EditorNotifyEventArgs e)
        {
            if (MessageBox.Show("선택항목을 삭제하시겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            HIC_CHUKDTL_LOCATION code = SSList.GetRowData(e.Row) as HIC_CHUKDTL_LOCATION;

            SSList.DeleteRow(e.Row);

            Data_Save("DEL");
            Screen_Display(FnWRTNO);
        }

        void Screen_Display(long nWRTNO)
        {
            //SSList.DataSource = null;
            SSList.ActiveSheet.RowCount = 0;

            HIC_CHUKMST_NEW hCMN = hicChukMstNewService.GetItemByWrtno(nWRTNO);

            if (!hCMN.IsNullOrEmpty())
            {
                List<HIC_CHUKDTL_LOCATION> lstHCL = hicChukDtlLocationService.GetListByWrtno(nWRTNO);

                if (lstHCL.Count > 0)
                {
                    SSList.DataSource = lstHCL;
                }
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnAdd_Add)
            {
                SSList.AddRows(1);

                int nRowCnt = SSList.ActiveSheet.RowCount - 1;
                
                Import_ImageFile(nRowCnt);

                SSList.ActiveSheet.Cells[nRowCnt, 1].Text = SSList.ActiveSheet.RowCount.To<string>();
            }
            else if (sender == btnSave)
            {
                Data_Save();
                Screen_Display(FnWRTNO);
            }
        }

        private void Import_ImageFile(int nCurrRow)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult result =  dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                SSList.ActiveSheet.Cells[nCurrRow, 2].Text = dialog.FileName;

                FileStream fs = new FileStream(dialog.FileName, FileMode.Open, FileAccess.Read);
                byte[] b = new byte[fs.Length - 1];
                fs.Read(b, 0, b.Length);
                fs.Close();
            }
        }

        private void Data_Save(string sDel = "")
        {
            try
            {
                if (FnWRTNO == 0) { return; }

                List<HIC_CHUKDTL_LOCATION> lstHCL = SSList.GetEditbleData<HIC_CHUKDTL_LOCATION>();

                for (int i = 0; i < lstHCL.Count; i++)
                {
                    FileStream fs = new FileStream(lstHCL[i].FILENAME, FileMode.Open, FileAccess.Read);
                    byte[] b = new byte[fs.Length - 1];
                    fs.Read(b, 0, b.Length);
                    fs.Close();

                    lstHCL[i].IMAGEDATA = b;
                }

                if (!hicChukDtlLocationService.Save(lstHCL, FnWRTNO))
                {
                    MessageBox.Show("측정위치도 등록중 오류가 발생하였습니다. ");
                    return;
                }

                MessageBox.Show("저장완료. ");
                Screen_Clear();
            }
            catch (Exception)
            {
                MessageBox.Show("저장실패. ", "오류");
                return;
            }
        }

        private void Screen_Clear()
        {
            pictureBox1.Clear();
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            if (FnWRTNO > 0)
            {
                Screen_Display(FnWRTNO);
            }
        }
    }
}
