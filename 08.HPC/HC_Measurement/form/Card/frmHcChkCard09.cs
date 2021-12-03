using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;
using HC_Core;
using HC_Measurement.Dto;
using HC_Measurement.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace HC_Measurement
{
    public partial class frmHcChkCard09 :CommonForm
    {
        clsSpread cSpd = null;
        HicChukMstNewService hicChukMstNewService = null;
        HicChukDtlLocationService hicChukDtlLocationService = null;

        long FnWRTNO = 0;

        public frmHcChkCard09()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHcChkCard09(long nWRTNO)
        {
            InitializeComponent();
            SetEvent();
            SetControl();

            FnWRTNO = nWRTNO;
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
           
        }

       
        private void SetControl()
        {
            cSpd = new clsSpread();
            hicChukMstNewService = new HicChukMstNewService();
            hicChukDtlLocationService = new HicChukDtlLocationService();

            #region 종합의견 소음
            ssNOISE.Initialize(new SpreadOption { RowHeaderVisible = false, RowHeight = 26, ColumnHeaderHeight = 38 });
            ssNOISE.AddColumn("일련번호", nameof(HIC_CHUKDTL_RESULT.WRTNO), 74, new SpreadCellTypeOption { IsVisivle = false });
            ssNOISE.AddColumn("순번", nameof(HIC_CHUKDTL_RESULT.SEQNO), 40, new SpreadCellTypeOption { IsEditble = false });
            ssNOISE.AddColumn("공정코드", nameof(HIC_CHUKDTL_RESULT.PROCS_CD), 78, new SpreadCellTypeOption { IsEditble = false });
            ssNOISE.AddColumn("공정명", nameof(HIC_CHUKDTL_RESULT.PROCS_NM), 180, new SpreadCellTypeOption { IsEditble = false });
            ssNOISE.AddColumn("단위작업장소", nameof(HIC_CHUKDTL_RESULT.UNIT_WRKRUM_NM), 180, new SpreadCellTypeOption { IsEditble = false });
            ssNOISE.AddColumn("측정위치", nameof(HIC_CHUKDTL_RESULT.WEM_LC), 98, new SpreadCellTypeOption { IsEditble = false });
            ssNOISE.AddColumn("작업자", nameof(HIC_CHUKDTL_RESULT.LABRR_NM), 88, new SpreadCellTypeOption { IsEditble = false });
            ssNOISE.AddColumn("측정치", nameof(HIC_CHUKDTL_RESULT.WEM_VALUE_NOW), 88, new SpreadCellTypeOption { IsEditble = false });
            ssNOISE.AddColumn("단위", nameof(HIC_CHUKDTL_RESULT.EXPSR_STDR_UNIT), 48, new SpreadCellTypeOption { IsEditble = false });
            ssNOISE.AddColumn("노출기준", nameof(HIC_CHUKDTL_RESULT.EXPSR_STDR_VALUE), 54, new SpreadCellTypeOption { IsEditble = false });
            ssNOISE.AddColumn("평가", nameof(HIC_CHUKDTL_RESULT.WEN_EVL_RESULT), 74, new SpreadCellTypeOption { IsEditble = false });
            ssNOISE.AddColumn("ROWID", nameof(HIC_CHUKDTL_RESULT.RID), 74, new SpreadCellTypeOption { IsVisivle = false });
            #endregion

            #region 종합의견 단일물질
            ssUCD.Initialize(new SpreadOption { RowHeaderVisible = false, RowHeight = 26, ColumnHeaderHeight = 38 });
            ssUCD.AddColumn("일련번호", nameof(HIC_CHUKDTL_RESULT.WRTNO), 74, new SpreadCellTypeOption { IsVisivle = false });
            ssUCD.AddColumn("순번", nameof(HIC_CHUKDTL_RESULT.SEQNO), 40, new SpreadCellTypeOption { IsEditble = false });
            ssUCD.AddColumn("공정코드", nameof(HIC_CHUKDTL_RESULT.PROCS_CD), 78, new SpreadCellTypeOption { IsEditble = false });
            ssUCD.AddColumn("공정명", nameof(HIC_CHUKDTL_RESULT.PROCS_NM), 180, new SpreadCellTypeOption { IsEditble = false });
            ssUCD.AddColumn("단위작업장소", nameof(HIC_CHUKDTL_RESULT.UNIT_WRKRUM_NM), 180, new SpreadCellTypeOption { IsEditble = false });
            ssUCD.AddColumn("유해물질", nameof(HIC_CHUKDTL_RESULT.CHMCLS_NM), 98, new SpreadCellTypeOption { IsEditble = false });
            ssUCD.AddColumn("측정위치", nameof(HIC_CHUKDTL_RESULT.WEM_LC), 98, new SpreadCellTypeOption { IsEditble = false });
            ssUCD.AddColumn("작업자", nameof(HIC_CHUKDTL_RESULT.LABRR_NM), 88, new SpreadCellTypeOption { IsEditble = false });
            ssUCD.AddColumn("측정치", nameof(HIC_CHUKDTL_RESULT.WEM_VALUE_NOW), 88, new SpreadCellTypeOption { IsEditble = false });
            ssUCD.AddColumn("단위", nameof(HIC_CHUKDTL_RESULT.EXPSR_STDR_UNIT), 48, new SpreadCellTypeOption { IsEditble = false });
            ssUCD.AddColumn("평가", nameof(HIC_CHUKDTL_RESULT.WEN_EVL_RESULT), 74, new SpreadCellTypeOption { IsEditble = false });
            ssUCD.AddColumn("ROWID", nameof(HIC_CHUKDTL_RESULT.RID), 74, new SpreadCellTypeOption { IsVisivle = false });
            #endregion

            #region 종합의견 혼합유기물
            ssUCD_Mix.Initialize(new SpreadOption { RowHeaderVisible = false, RowHeight = 26, ColumnHeaderHeight = 38 });
            ssUCD_Mix.AddColumn("일련번호", nameof(HIC_CHUKDTL_RESULT.WRTNO), 74, new SpreadCellTypeOption { IsVisivle = false });
            ssUCD_Mix.AddColumn("순번", nameof(HIC_CHUKDTL_RESULT.SEQNO), 40, new SpreadCellTypeOption { IsEditble = false });
            ssUCD_Mix.AddColumn("공정코드", nameof(HIC_CHUKDTL_RESULT.PROCS_CD), 78, new SpreadCellTypeOption { IsEditble = false });
            ssUCD_Mix.AddColumn("공정명", nameof(HIC_CHUKDTL_RESULT.PROCS_NM), 180, new SpreadCellTypeOption { IsEditble = false });
            ssUCD_Mix.AddColumn("단위작업장소", nameof(HIC_CHUKDTL_RESULT.UNIT_WRKRUM_NM), 180, new SpreadCellTypeOption { IsEditble = false });            
            ssUCD_Mix.AddColumn("측정위치", nameof(HIC_CHUKDTL_RESULT.WEM_LC), 98, new SpreadCellTypeOption { IsEditble = false });
            ssUCD_Mix.AddColumn("작업자", nameof(HIC_CHUKDTL_RESULT.LABRR_NM), 88, new SpreadCellTypeOption { IsEditble = false });
            ssUCD_Mix.AddColumn("혼합물노출계수(EM)", "", 150, new SpreadCellTypeOption { IsEditble = false });
            ssUCD_Mix.AddColumn("단위", nameof(HIC_CHUKDTL_RESULT.EXPSR_STDR_UNIT), 48, new SpreadCellTypeOption { IsEditble = false });
            ssUCD_Mix.AddColumn("평가", nameof(HIC_CHUKDTL_RESULT.WEN_EVL_RESULT), 74, new SpreadCellTypeOption { IsEditble = false });
            ssUCD_Mix.AddColumn("ROWID", nameof(HIC_CHUKDTL_RESULT.RID), 74, new SpreadCellTypeOption { IsVisivle = false });
            #endregion
        }

        void Screen_Display(long nWRTNO)
        {
            //HIC_CHUKMST_NEW hCMN = hicChukMstNewService.GetItemByWrtno(nWRTNO);

            //if (!hCMN.IsNullOrEmpty())
            //{
            //    List<HIC_CHUKDTL_LOCATION> lstHCL = hicChukDtlLocationService.GetListByWrtno(nWRTNO);

            //    if (lstHCL.Count > 0)
            //    {
            //        SSList.DataSource = lstHCL;
            //    }
            //}
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnAdd_Add)
            {

            }
        }

        private void Data_Save()
        {
            try
            {
                if (FnWRTNO == 0) { return; }

                List<HIC_CHUKDTL_LOCATION> lstHCL = new List<HIC_CHUKDTL_LOCATION>();
                HIC_CHUKDTL_LOCATION hCL = new HIC_CHUKDTL_LOCATION();

                //for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
                //{
                //    hCL = SSList.GetRowData(i) as HIC_CHUKDTL_LOCATION;
                //    lstHCL.Add(hCL);
                //}

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
