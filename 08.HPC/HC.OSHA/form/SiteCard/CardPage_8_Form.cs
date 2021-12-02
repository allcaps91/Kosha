using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Utils;
using FarPoint.Win.Spread;
using HC.Core.Common.Interface;
using HC_Core;
using HC.Core.Common.Util;
using HC.Core.Model;
using HC.OSHA.Dto;
using HC.OSHA.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComHpcLibB.Model;

namespace HC_OSHA
{
    public partial class CardPage_8_Form : CommonForm, ISelectSite, ISelectEstimate, IPrint
    {
        private HcOshaCard11_1Service hcOshaCard11_1Service;
        private HcOshaCard11_2Service hcOshaCard11_2Service;
        public CardPage_8_Form()
        {
            InitializeComponent();
            hcOshaCard11_1Service = new HcOshaCard11_1Service();
            hcOshaCard11_2Service = new HcOshaCard11_2Service();
        }

        private void CardPage_8_Form_Load(object sender, EventArgs e)
        {
            Init_11_1();
            Init_11_2();
        }

        public void Select(IEstimateModel estimateModel)
        {
            base.SelectedEstimate = estimateModel;
            Clear();
            Search11_1();
            Search11_2();
        }

        public void Select(ISiteModel siteModel)
        {
            base.SelectedSite = siteModel;
            Clear();
        }

        private void Clear()
        {
            SSList11_1.ActiveSheet.RowCount = 0;
            SSList11_2.ActiveSheet.RowCount = 0;
        }


        #region 건강증진운동 추진
        private void Init_11_1()
        {

            DtpPUBLISHDATE.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CARD11_1.PUBLISHDATE), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            DtpACTDATE.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD11_1.ACTDATE) });
            TxtSTATUS.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD11_1.STATUS) });
            TxtCONTENT.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD11_1.CONTENT) });

            SSList11_1.Initialize(new SpreadOption() { IsRowSelectColor = true });
            SSList11_1.AddColumnText("개시일자", nameof(HC_OSHA_CARD11_1.PUBLISHDATE), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList11_1.AddColumnText("체력측정용 시설장비현황", nameof(HC_OSHA_CARD11_1.STATUS), 167, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList11_1.AddColumnText("추진일자", nameof(HC_OSHA_CARD11_1.ACTDATE), 130, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList11_1.AddColumnText("내용", nameof(HC_OSHA_CARD11_1.CONTENT), 250, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });

        }


        private void BtnSave11_1_Click(object sender, EventArgs e)
        {
            if (base.SelectedEstimate == null)
            {
                MessageUtil.Info("사업장 계약 정보를 선택하세요");
            }
            else
            {
                if (pan11_1.Validate<HC_OSHA_CARD11_1>())
                {
                    HC_OSHA_CARD11_1 dto = pan11_1.GetData<HC_OSHA_CARD11_1>();
                    dto.ESTIMATE_ID = base.SelectedEstimate.ID;
                    dto.SITE_ID = base.SelectedSite.ID;
                    //HC_OSHA_CARD11_1 saved = hcOshaCard11_1Service.Save(dto, base.GetCurrentYear());
                    HC_OSHA_CARD11_1 saved = hcOshaCard11_1Service.Save(dto, base.SelectedEstimate.CONTRACTSTARTDATE.Left(4));

                    pan11_1.SetData(saved);
                    pan11_1.Initialize();
                    Search11_1();
                }
            }
        }
        private void BtnDelete11_1_Click(object sender, EventArgs e)
        {
            HC_OSHA_CARD11_1 dto = pan11_1.GetData<HC_OSHA_CARD11_1>();

            if (dto.ID > 0)
            {
                if (MessageUtil.Confirm("삭제하시겠습니까?") == DialogResult.Yes)
                {
                    hcOshaCard11_1Service.Delete(dto);
                    pan11_1.Initialize();

                    Search11_1();
                }
            }
        }

        private void BtnNew11_1_Click(object sender, EventArgs e)
        {
            pan11_1.Initialize();
        }

        private void Search11_1_Clear()
        {

            SSCard.ActiveSheet.Cells[3, 0].Value = "";
            SSCard.ActiveSheet.Cells[5, 0].Value = "";
            SSCard.ActiveSheet.Cells[4, 2].Value = "";
            SSCard.ActiveSheet.Cells[4, 3].Value = "";
        }
        //건강증진운동 추진
        private void Search11_1()
        {
            Search11_1_Clear();

            if (base.SelectedSite == null)
            {
                return;
            }

            List<HC_OSHA_CARD11_1> list = hcOshaCard11_1Service.FindAll(base.SelectedSite.ID, base.SelectedEstimate.CONTRACTSTARTDATE.Left(4));
            //List<HC_OSHA_CARD11_1> list = hcOshaCard11_1Service.FindAll(base.SelectedSite.ID, base.GetCurrentYear());
            if (list.Count > 0)
            {
                SSList11_1.SetDataSource(list);

                SSCard.ActiveSheet.Cells[3, 0].Value = list[0].PUBLISHDATE;
                SSCard.ActiveSheet.Cells[5, 0].Value = list[0].STATUS;
                SSCard.ActiveSheet.Cells[4, 2].Value = list[0].ACTDATE;
                SSCard.ActiveSheet.Cells[4, 3].Value = list[0].CONTENT;
            }
            else
            {
                SSList11_1.SetDataSource(new List<HC_OSHA_CARD11_1>());
            }
        }

        private void SSList11_1_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            HC_OSHA_CARD11_1 dto = SSList11_1.GetRowData(e.Row) as HC_OSHA_CARD11_1;
            pan11_1.SetData(dto);

        }
        #endregion



        #region 건강운동지도자 양성교육
        private void Init_11_2()
        {
            TxtNAME.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD11_2.NAME) });
            DtpSTARTDATE.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CARD11_2.STARTDATE), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            DtpENDDATE.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CARD11_2.ENDDATE), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            Txt11_2CONTENT.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD11_2.CONTENT) });

            SSList11_2.Initialize(new SpreadOption() { IsRowSelectColor = true });
            SSList11_2.AddColumnText("양성교육이수자명", nameof(HC_OSHA_CARD11_2.NAME), 120, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList11_2.AddColumnText("교육과정 및 내용", nameof(HC_OSHA_CARD11_2.CONTENT), 200, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList11_2.AddColumnText("교육기간", nameof(HC_OSHA_CARD11_2.PERIOD), 180, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });

        }


        private void BtnSave11_2_Click(object sender, EventArgs e)
        {
            if (base.SelectedEstimate == null)
            {
                MessageUtil.Info("사업장 계약 정보를 선택하세요");
            }
            else
            {
                if (pan11_2.Validate<HC_OSHA_CARD11_2>())
                {
                    HC_OSHA_CARD11_2 dto = pan11_2.GetData<HC_OSHA_CARD11_2>();
                    dto.ESTIMATE_ID = base.SelectedEstimate.ID;
                    dto.SITE_ID = base.SelectedSite.ID;
                    HC_OSHA_CARD11_2 saved = hcOshaCard11_2Service.Save(dto, base.GetCurrentYear());

                    pan11_2.SetData(saved);
                    pan11_2.Initialize();
                    Search11_2();
                }
            }
        }
        private void BtnDelete11_2_Click(object sender, EventArgs e)
        {
            HC_OSHA_CARD11_2 dto = pan11_2.GetData<HC_OSHA_CARD11_2>();

            if (dto.ID > 0)
            {
                if (MessageUtil.Confirm("삭제하시겠습니까?") == DialogResult.Yes)
                {
                    hcOshaCard11_2Service.Delete(dto);
                    pan11_2.Initialize();

                    Search11_2();
                }
            }
        }

        private void BtnNew11_2_Click(object sender, EventArgs e)
        {
            pan11_2.Initialize();
        }

        private void Search11_2_Clear()
        {
            for (int i = 0; i < 13; i++)
            {
                SSCard.ActiveSheet.Cells[i + 9, 0].Value = "";
                SSCard.ActiveSheet.Cells[i + 9, 1].Value = "";
                SSCard.ActiveSheet.Cells[i + 9, 4].Value = "";
            }
        }

        //건강증진운동지도자 양성교육
        private void Search11_2()
        {
            Search11_2_Clear();

            List<HC_OSHA_CARD11_2> list = hcOshaCard11_2Service.FindAll(base.SelectedSite.ID, base.SelectedEstimate.CONTRACTENDDATE.Substring(0, 4));
            if (list.Count > 0)
            {
                SSList11_2.SetDataSource(list);

                for(int i=0; i<list.Count; i++)
                {
                    SSCard.ActiveSheet.Cells[i + 9, 0].Value = list[i].NAME;
                    SSCard.ActiveSheet.Cells[i + 9, 1].Value = list[i].CONTENT;
                    SSCard.ActiveSheet.Cells[i + 9, 4].Value = list[i].STARTDATE + list[i].ENDDATE;
                }
            }
            else
            {
                SSList11_2.SetDataSource(new List<HC_OSHA_CARD11_2>());
            }
        }

        private void SSList11_2_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            HC_OSHA_CARD11_2 dto = SSList11_2.GetRowData(e.Row) as HC_OSHA_CARD11_2;
            pan11_2.SetData(dto);

        }


        #endregion

        public void Print()
        {
            Search11_1();
            Search11_2();
       
            SpreadPrint print = new SpreadPrint(SSCard, PrintStyle.FORM, false);
            print.Execute();
        }

        public bool NewPrint()
        {
            Search11_1();
            Search11_2();

            SpreadPrint print = new SpreadPrint(SSCard, PrintStyle.FORM, false);
            print.Execute();
            return true;
        }


        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Print();
        }
    }
}
