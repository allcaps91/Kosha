using ComBase.Controls;
using ComBase.Mvc.Enums;
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
    public partial class CardPage_7_Form : CommonForm, ISelectSite, ISelectEstimate, IPrint
    {
        private HcOshaCard91Service hcOshaCard91Service;
        private HcOshaCard92Service hcOshaCard92Service;
        private HcOshaCard93Service hcOshaCard93Service;
        private HcOshaCard94Service hcOshaCard94Service;
        private HcOshaCard95Service hcOshaCard95Service;
        private HcOshaCard10Service hcOshaCard10Service;

        public CardPage_7_Form()
        {
            InitializeComponent();
            hcOshaCard91Service = new HcOshaCard91Service();
            hcOshaCard92Service = new HcOshaCard92Service();
            hcOshaCard93Service = new HcOshaCard93Service();
            hcOshaCard94Service = new HcOshaCard94Service();
            hcOshaCard95Service = new HcOshaCard95Service();
            hcOshaCard10Service = new HcOshaCard10Service();
        }

        private void CardPage_7_Form_Load(object sender, EventArgs e)
        {
            Init_9_1();
            Init_9_2();
            Init_9_3();
            Init_9_4();
            Init_9_5();
            Clear();
        }

        public void Select(IEstimateModel estimateModel)
        {
            base.SelectedEstimate = estimateModel;
            Clear();
            Search9_1();
            Search9_2();
            Search9_3();
            Search9_4();
            Search9_5();
            Init_10();
        }

        public void Select(ISiteModel siteModel)
        {
            base.SelectedSite = siteModel;
            Clear();
        }

        private void Clear()
        {
            pan9_1.Initialize();
            SSList9_1.ActiveSheet.RowCount = 0;

            SSCard.ActiveSheet.Cells[27, 0].Value = "10. 무재해운동추진";
            
            Search9_1_Clear();

            pan9_2.Initialize();
            SSList9_2.ActiveSheet.RowCount = 0;
            Search9_2_Clear();

            pan9_3.Initialize();
            SSList9_3.ActiveSheet.RowCount = 0;
            SSList9_3.SetDataSource(new List<HC_OSHA_CARD9_3>());
            Search9_3_Clear();

            pan9_4.Initialize();
            SSList9_4.ActiveSheet.RowCount = 0;
            SSList9_4.SetDataSource(new List<HC_OSHA_CARD9_4>());
            Search9_4_Clear();

            pan9_5.Initialize();
            SSList9_5.ActiveSheet.RowCount = 0;
            Search9_5_Clear();


            SSList10.ActiveSheet.RowCount = 0;
            SearchCard10_Clear();
        }

        #region 정기평가
        private void Init_9_1()
        {
            Dtp9_1STARTDATE.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CARD9_1.STARTDATE), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            Dtp9_1ENDDATE.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CARD9_1.ENDDATE), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            TxtCONTENT.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD9_1.CONTENT) });
            TxtGRADE1.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD9_1.GRADE1) });
            TxtNAME1.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD9_1.NAME1) });
            TxtGRADE2.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD9_1.GRADE2) });
            TxtNAME2.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD9_1.NAME2) });
            TxtGRADE3.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD9_1.GRADE3) });
            TxtNAME3.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD9_1.NAME3) });
            TxtGRADE4.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD9_1.GRADE4) });
            TxtNAME4.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD9_1.NAME4) });


            SSList9_1.Initialize(new SpreadOption() { IsRowSelectColor = true });
            SSList9_1.AddColumnText("실시기간", nameof(HC_OSHA_CARD9_1.PERIOD), 161, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList9_1.AddColumnText("실시결과", nameof(HC_OSHA_CARD9_1.CONTENT), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList9_1.AddColumnText("실시자1직책", nameof(HC_OSHA_CARD9_1.GRADE1), 66, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList9_1.AddColumnText("실시자1성명", nameof(HC_OSHA_CARD9_1.NAME1), 60, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList9_1.AddColumnText("실시자2직책", nameof(HC_OSHA_CARD9_1.GRADE2), 66, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList9_1.AddColumnText("실시자2성명", nameof(HC_OSHA_CARD9_1.NAME2), 60, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList9_1.AddColumnText("실시자3직책", nameof(HC_OSHA_CARD9_1.GRADE3), 66, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList9_1.AddColumnText("실시자3성명", nameof(HC_OSHA_CARD9_1.NAME3), 60, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList9_1.AddColumnText("실시자4직책", nameof(HC_OSHA_CARD9_1.GRADE4), 66, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList9_1.AddColumnText("실시자4성명", nameof(HC_OSHA_CARD9_1.NAME4), 60, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });

        }


        private void BtnSave9_1_Click(object sender, EventArgs e)
        {
            if (base.SelectedEstimate == null)
            {
                MessageUtil.Info("사업장 계약 정보를 선택하세요");
            }
            else
            {
                if (pan9_1.Validate<HC_OSHA_CARD9_1>())
                {
                    HC_OSHA_CARD9_1 dto = pan9_1.GetData<HC_OSHA_CARD9_1>();
                    dto.ESTIMATE_ID = base.SelectedEstimate.ID;
                    dto.SITE_ID = base.SelectedSite.ID;
                    HC_OSHA_CARD9_1 saved = hcOshaCard91Service.Save(dto);

                    pan9_1.SetData(saved);
                    pan9_1.Initialize();
                    Search9_1();
                }
            }

        }
        private void BtnDelete9_1_Click(object sender, EventArgs e)
        {
            HC_OSHA_CARD9_1 dto = pan9_1.GetData<HC_OSHA_CARD9_1>();

            if (dto.ID > 0)
            {
                if (MessageUtil.Confirm("삭제하시겠습니까?") == DialogResult.Yes)
                {
                    hcOshaCard91Service.Delete(dto);
                    pan9_1.Initialize();

                    Search9_1();
                }
            }
        }
        private void BtnNew9_1_Click(object sender, EventArgs e)
        {
            pan9_1.Initialize();
        }

         private void Search9_1_Clear()
        {
            int row = 0;
            for (int i = 0; i < 2; i++)
            {
                row = i * 2 + 3;
                SSCard.ActiveSheet.Cells[row, 1].Value = "";
                SSCard.ActiveSheet.Cells[row, 3].Value = "";
                SSCard.ActiveSheet.Cells[row, 5].Value = "";
                SSCard.ActiveSheet.Cells[row, 8].Value = "";
                row = i * 2 + 4;
                SSCard.ActiveSheet.Cells[row, 3].Value = "";
                SSCard.ActiveSheet.Cells[row, 5].Value = "";

            }
        }
        private void Search9_1()
        {
            Search9_1_Clear();

            if (base.SelectedSite == null)
            {
                return;
            }

            if (base.SelectedEstimate.CONTRACTENDDATE == null)
            {
                return;
            }
            List<HC_OSHA_CARD9_1> list = hcOshaCard91Service.FindAll(base.SelectedSite.ID, base.SelectedEstimate.CONTRACTSTARTDATE.Left(4));
            if (list.Count > 0)
            {
                SSList9_1.SetDataSource(list);

                int row = 0;
                int count = list.Count;
                if(list.Count > 2)
                {
                    count = 2;
                }
                for (int i=0; i< count; i++)
                {
                    row = i * 2 + 3;
                    SSCard.ActiveSheet.Cells[row, 1].Value = list[i].STARTDATE + "\n ~ " + list[i].ENDDATE;
                    SSCard.ActiveSheet.Cells[row, 3].Value = list[i].GRADE1 + " / " + list[i].NAME1;
                    SSCard.ActiveSheet.Cells[row, 5].Value = list[i].GRADE2 + " / " + list[i].NAME2;
                    SSCard.ActiveSheet.Cells[row, 8].Value = list[i].CONTENT;
                    row = i * 2 + 4;
                    SSCard.ActiveSheet.Cells[row, 3].Value = list[i].GRADE3 + " / " + list[i].NAME3;
                    SSCard.ActiveSheet.Cells[row, 5].Value = list[i].GRADE4 + " / " + list[i].NAME4;

                }
            }
            
        }
        private void SSList9_1_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            HC_OSHA_CARD9_1 dto = SSList9_1.GetRowData(e.Row) as HC_OSHA_CARD9_1;
            pan9_1.SetData(dto);
        }

        #endregion

        #region 수시평가
        private void Init_9_2()
        {
            TxtWORKNAME.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD9_2.WORKNAME) });
            Dtp9_2STARTDATE.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CARD9_2.STARTDATE), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            Dtp9_2ENDDATE.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CARD9_2.ENDDATE), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            Txt9_2CONTENT.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD9_2.CONTENT) });
            Txt9_2GRADE1.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD9_2.GRADE1) });
            Txt9_2NAME1.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD9_2.NAME1) });
            Txt9_2GRADE2.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD9_2.GRADE2) });
            Txt9_2NAME2.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD9_2.NAME2) });
            
            SSList9_2.Initialize(new SpreadOption() { IsRowSelectColor = true });
            SSList9_2.AddColumnText("대상작업또는공정", nameof(HC_OSHA_CARD9_2.WORKNAME), 140, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList9_2.AddColumnText("실시기간", nameof(HC_OSHA_CARD9_2.PERIOD), 165, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList9_2.AddColumnText("실시결과", nameof(HC_OSHA_CARD9_2.CONTENT), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList9_2.AddColumnText("실시자1직책", nameof(HC_OSHA_CARD9_2.GRADE1), 66, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList9_2.AddColumnText("실시자1성명", nameof(HC_OSHA_CARD9_2.NAME1), 60, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList9_2.AddColumnText("실시자2직책", nameof(HC_OSHA_CARD9_2.GRADE2), 66, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList9_2.AddColumnText("실시자2성명", nameof(HC_OSHA_CARD9_2.NAME2), 60, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });


        }
        private void BtnSave9_2_Click(object sender, EventArgs e)
        {
            if (base.SelectedEstimate == null)
            {
                MessageUtil.Info("사업장 계약 정보를 선택하세요");
            }
            else
            {
                if (pan9_2.Validate<HC_OSHA_CARD9_2>())
                {
                    HC_OSHA_CARD9_2 dto = pan9_2.GetData<HC_OSHA_CARD9_2>();
                    dto.ESTIMATE_ID = base.SelectedEstimate.ID;
                    dto.SITE_ID = base.SelectedSite.ID;
                    HC_OSHA_CARD9_2 saved = hcOshaCard92Service.Save(dto);

                    pan9_2.SetData(saved);
                    pan9_2.Initialize();
                    Search9_2();
                }
            }
        }

        private void BtnDelete9_2_Click(object sender, EventArgs e)
        {
            HC_OSHA_CARD9_2 dto = pan9_2.GetData<HC_OSHA_CARD9_2>();

            if (dto.ID > 0)
            {
                if (MessageUtil.Confirm("삭제하시겠습니까?") == DialogResult.Yes)
                {
                    hcOshaCard92Service.Delete(dto);
                    pan9_2.Initialize();

                    Search9_2();
                }
            }
        }

        private void BtnNew9_2_Click(object sender, EventArgs e)
        {
            pan9_2.Initialize();
        }

        private void Search9_2_Clear()
        {
            int row = 0;
            for (int i = 0; i < 4; i++)
            {
                row = (i * 2) + 8;
                SSCard.ActiveSheet.Cells[row, 1].Value = "";
                SSCard.ActiveSheet.Cells[row, 3].Value = "";
                SSCard.ActiveSheet.Cells[row, 5].Value = "";
                SSCard.ActiveSheet.Cells[row, 8].Value = "";
                row = (i * 2) + 9;
                SSCard.ActiveSheet.Cells[row, 5].Value = "";
            }
        }
        //수시평가
        private void Search9_2()
        {
            Search9_2_Clear();
            if(base.SelectedSite == null)
            {
                return;
            }
            if (base.SelectedEstimate.CONTRACTENDDATE == null)
            {
                return;
            }

            //List<HC_OSHA_CARD9_2> list = hcOshaCard92Service.FindAll(base.SelectedSite.ID, base.SelectedEstimate.CONTRACTENDDATE.Substring(0, 4));
            List<HC_OSHA_CARD9_2> list = hcOshaCard92Service.FindAll(base.SelectedSite.ID);
            if (list.Count > 0)
            {
                SSList9_2.SetDataSource(list);
                
                int row = 0;
                for(int i=0; i< list.Count; i++)
                {
                    row = (i * 2) + 8;
                    SSCard.ActiveSheet.Cells[row, 1].Value = list[i].WORKNAME;
                    SSCard.ActiveSheet.Cells[row, 3].Value = list[i].STARTDATE.Trim() +"~" +  list[i].ENDDATE.Trim();

                    SSCard.ActiveSheet.Cells[row, 5].Value = list[i].GRADE1 + " / " + list[i].NAME1;
                    SSCard.ActiveSheet.Cells[row, 8].Value = list[i].CONTENT;
                    row = (i * 2) + 9;
                    SSCard.ActiveSheet.Cells[row, 5].Value = list[i].GRADE2 + " / " + list[i].NAME2;
                }

            }
            else
            {
                SSList9_2.SetDataSource(new List<HC_OSHA_CARD9_2>());
            }
           
        }
        private void SSLIST9_2_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            HC_OSHA_CARD9_2 dto = SSList9_2.GetRowData(e.Row) as HC_OSHA_CARD9_2;
            pan9_2.SetData(dto);
        }

        #endregion

        #region 교육
        private void Init_9_3()
        {
            
            DtpSITESTARTDATE.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CARD9_3.SITESTARTDATE), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            DtpSITEENDDATE.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CARD9_3.SITEENDDATE), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            TxtSITEGRADE.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD9_3.SITEGRADE) });
            TxtSITENAME.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD9_3.SITENAME) });
            DtpTESTSTARTDATE.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CARD9_3.TESTSTARTDATE), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            DtpTESTENDDATE.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CARD9_3.TESTENDDATE), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            TxtTESTGRADE.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD9_3.TESTGRADE) });
            TxtTESTNAME.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD9_3.TESTNAME) });

            SSList9_3.Initialize(new SpreadOption() { IsRowSelectColor = true });
            SSList9_3.AddColumnText("교육기간(사업주)", nameof(HC_OSHA_CARD9_3.SITE_PERIOD), 157, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList9_3.AddColumnText("수료자(사업주)", nameof(HC_OSHA_CARD9_3.SITE_GRADE_NAME), 150, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
         //   SSList9_3.AddColumnText("실시자직책", nameof(HC_OSHA_CARD9_3.SITEGRADE), 66, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
         //   SSList9_3.AddColumnText("실시자성명", nameof(HC_OSHA_CARD9_3.SITENAME), 48, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });

            SSList9_3.AddColumnText("교육기간(평가자)", nameof(HC_OSHA_CARD9_3.TEST_PERIOD), 157, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList9_3.AddColumnText("수료자(평가자)", nameof(HC_OSHA_CARD9_3.TEST_GRADE_NAME), 150, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
         //   SSList9_3.AddColumnText("실시자직책", nameof(HC_OSHA_CARD9_3.TESTGRADE), 66, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
        //    SSList9_3.AddColumnText("실시자성명", nameof(HC_OSHA_CARD9_3.TESTNAME), 48, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });

        }


        private void BtnSave9_3_Click(object sender, EventArgs e)
        {
            if (base.SelectedEstimate == null)
            {
                MessageUtil.Info("사업장 계약 정보를 선택하세요");
            }
            else
            {
                if (pan9_3.Validate<HC_OSHA_CARD9_3>())
                {
                    HC_OSHA_CARD9_3 dto = pan9_3.GetData<HC_OSHA_CARD9_3>();
                    dto.ESTIMATE_ID = base.SelectedEstimate.ID;
                    dto.SITE_ID = base.SelectedSite.ID;
                    HC_OSHA_CARD9_3 saved = hcOshaCard93Service.Save(dto);

                    pan9_3.SetData(saved);
                    pan9_3.Initialize();
                    Search9_3();
                }
            }
        }
        private void BtnDelete9_3_Click(object sender, EventArgs e)
        {
            HC_OSHA_CARD9_3 dto = pan9_3.GetData<HC_OSHA_CARD9_3>();

            if (dto.ID > 0)
            {
                if (MessageUtil.Confirm("삭제하시겠습니까?") == DialogResult.Yes)
                {
                    hcOshaCard93Service.Delete(dto);
                    pan9_3.Initialize();

                    Search9_3();
                }
            }
        }

        private void BtnNew9_3_Click(object sender, EventArgs e)
        {
            pan9_3.Initialize();
        }

        private void Search9_3_Clear()
        {
            SSCard.ActiveSheet.Cells[18, 1].Value = "";
            SSCard.ActiveSheet.Cells[18, 3].Value = "";
            SSCard.ActiveSheet.Cells[18, 5].Value = "";
            SSCard.ActiveSheet.Cells[18, 8].Value = "";
        }
         //교육
        private void Search9_3()
        {
            Search9_3_Clear();

            if (base.SelectedEstimate.CONTRACTENDDATE == null)
            {
                return;
            }

            List<HC_OSHA_CARD9_3> list = hcOshaCard93Service.FindAll(base.SelectedSite.ID);
            if (list.Count > 0)
            {
                SSList9_3.SetDataSource(list);

                //for(int i=0; i<list.Count; i++)
                //{
                //    SSCard.ActiveSheet.Cells[18, 1].Value = list[i].SITESTARTDATE +"\n ~ " + list[i].SITEENDDATE;
                //    SSCard.ActiveSheet.Cells[18, 3].Value = list[i].SITE_GRADE_NAME;
                //    SSCard.ActiveSheet.Cells[18, 5].Value = list[i].TESTSTARTDATE + "\n ~ " + list[i].TESTENDDATE;
                //    SSCard.ActiveSheet.Cells[18, 8].Value = list[i].TEST_GRADE_NAME;
                //}
                if (list.Count>0)
                {
                    SSCard.ActiveSheet.Cells[18, 1].Value = list[0].SITESTARTDATE + "\n ~ " + list[0].SITEENDDATE;
                    SSCard.ActiveSheet.Cells[18, 3].Value = list[0].SITE_GRADE_NAME;
                    SSCard.ActiveSheet.Cells[18, 5].Value = list[0].TESTSTARTDATE + "\n ~ " + list[0].TESTENDDATE;
                    SSCard.ActiveSheet.Cells[18, 8].Value = list[0].TEST_GRADE_NAME;
                }
            }
            else
            {
                SSList9_3.SetDataSource(new List<HC_OSHA_CARD9_3>());
            }
        }

        private void SSList9_3_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            HC_OSHA_CARD9_3 dto = SSList9_3.GetRowData(e.Row) as HC_OSHA_CARD9_3;
            pan9_3.SetData(dto);

        }
        #endregion 

        #region 컨설팅
        private void Init_9_4()
        {

            Dtp9_4StartDate.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CARD9_4.STARTDATE), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            Dtp9_4EndDate.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CARD9_4.ENDDATE), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            Txt9_4Name.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD9_4.NAME) });
            Txt9_4Content.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD9_4.CONTENT) });
            Txt9_3Remark.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD9_4.REMARK) });

            SSList9_4.Initialize(new SpreadOption() { IsRowSelectColor = true });
            SSList9_4.AddColumnText("컨설팅기간", nameof(HC_OSHA_CARD9_4.PERIOD), 168, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList9_4.AddColumnText("수료기관및전문가", nameof(HC_OSHA_CARD9_4.NAME), 153, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList9_4.AddColumnText("주요컨설팅내용", nameof(HC_OSHA_CARD9_4.CONTENT), 147, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList9_4.AddColumnText("참고사항", nameof(HC_OSHA_CARD9_4.REMARK), 144, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });

        }


        private void BtnSave9_4_Click(object sender, EventArgs e)
        {
            if (base.SelectedEstimate == null)
            {
                MessageUtil.Info("사업장 계약 정보를 선택하세요");
            }
            else
            {
                if (pan9_4.Validate<HC_OSHA_CARD9_4>())
                {
                    HC_OSHA_CARD9_4 dto = pan9_4.GetData<HC_OSHA_CARD9_4>();
                    dto.ESTIMATE_ID = base.SelectedEstimate.ID;
                    dto.SITE_ID = base.SelectedSite.ID;
                    HC_OSHA_CARD9_4 saved = hcOshaCard94Service.Save(dto);

                 //   pan9_4.SetData(saved);
                    pan9_4.Initialize();
                    Search9_4();
                }
            }
        }
        private void BtnDelete9_4_Click(object sender, EventArgs e)
        {
            HC_OSHA_CARD9_4 dto = pan9_4.GetData<HC_OSHA_CARD9_4>();

            if (dto.ID > 0)
            {
                if (MessageUtil.Confirm("삭제하시겠습니까?") == DialogResult.Yes)
                {
                    hcOshaCard94Service.Delete(dto);
                    pan9_4.Initialize();

                    Search9_4();
                }
            }
        }

        private void BtnNew9_4_Click(object sender, EventArgs e)
        {
            pan9_4.Initialize();
        }


        private void Search9_4_Clear()
        {
            int  row = 20;

            SSCard.ActiveSheet.Cells[row, 1].Value = "";
            SSCard.ActiveSheet.Cells[row, 3].Value = "";
            SSCard.ActiveSheet.Cells[row, 5].Value = "";
            SSCard.ActiveSheet.Cells[row, 8].Value = "";
        }
        //컨설팅
        private void Search9_4()
        {
            Search9_4_Clear();

            if (base.SelectedEstimate.CONTRACTENDDATE == null)
            {
                return;
            }
            List<HC_OSHA_CARD9_4> list = hcOshaCard94Service.FindAll(base.SelectedSite.ID, base.SelectedEstimate.CONTRACTSTARTDATE.Left(4));
            if (list.Count > 0)
            {
                SSList9_4.SetDataSource(list);

                int row = 20;
                for (int i = 0; i < list.Count; i++){
                    SSCard.ActiveSheet.Cells[row, 1].Value = list[i].STARTDATE +"\n ~ " + list[i].ENDDATE;
                    SSCard.ActiveSheet.Cells[row, 3].Value = list[i].NAME;
                    SSCard.ActiveSheet.Cells[row, 5].Value = list[i].CONTENT;
                    SSCard.ActiveSheet.Cells[row, 8].Value = list[i].REMARK;
                }
            }
            else
            {
                SSList9_4.SetDataSource(new List<HC_OSHA_CARD9_4>());
            }
        }

        private void SSList9_4_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            HC_OSHA_CARD9_4 dto = SSList9_4.GetRowData(e.Row) as HC_OSHA_CARD9_4;
            pan9_4.SetData(dto);

        }
        #endregion

        #region 인정제도의 참여
        private void Init_9_5()
        {

            DtpREQUESTDATE.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CARD9_5.REQUESTDATE), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            DtpAPPROVEDATE.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CARD9_5.APPROVEDATE), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            CboISAPPROVE.SetOptions(new ComboBoxOption { DataField = nameof(HC_OSHA_CARD9_5.ISAPPROVE) });
            CboISAPPROVE.SetItems(codeService.FindActiveCodeByGroupCode("SITE_CARD_APPROVE", "OSHA"), "CodeName", "Code", "OSHA");


            Dtp9_5StartDate.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CARD9_5.STARTDATE), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            Drp9_5EndDate.SetOptions(new DateTimePickerOption { DataField = nameof(HC_OSHA_CARD9_5.ENDDATE), DataBaseFormat = DateTimeType.YYYY_MM_DD, DisplayFormat = DateTimeType.YYYY_MM_DD });
            Txt9_5Remark.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD9_5.REMARK) });

            SSList9_5.Initialize(new SpreadOption() { IsRowSelectColor = true });
            SSList9_5.AddColumnText("인정신청일", nameof(HC_OSHA_CARD9_5.REQUESTDATE), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList9_5.AddColumnText("결정일", nameof(HC_OSHA_CARD9_5.APPROVEDATE), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList9_5.AddColumnText("인정여부", nameof(HC_OSHA_CARD9_5.ISAPPROVE_TEXT), 75, IsReadOnly.N,  new SpreadCellTypeOption { IsSort = false });
            SSList9_5.AddColumnText("인정유효기간", nameof(HC_OSHA_CARD9_5.PERIOD), 165, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList9_5.AddColumnText("기타", nameof(HC_OSHA_CARD9_5.REMARK), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });

        }


        private void BtnSave9_5_Click(object sender, EventArgs e)
        {
            if (base.SelectedEstimate == null)
            {
                MessageUtil.Info("사업장 계약 정보를 선택하세요");
            }
            else
            {
                if (pan9_5.Validate<HC_OSHA_CARD9_5>())
                {
                    HC_OSHA_CARD9_5 dto = pan9_5.GetData<HC_OSHA_CARD9_5>();
                    dto.ESTIMATE_ID = base.SelectedEstimate.ID;
                    dto.SITE_ID = base.SelectedSite.ID;
                    HC_OSHA_CARD9_5 saved = hcOshaCard95Service.Save(dto);

                    //pan9_5.SetData(saved);
                    pan9_5.Initialize();

                    Search9_5();
                }
            }
        }
        private void BtnDelete9_5_Click(object sender, EventArgs e)
        {
            HC_OSHA_CARD9_5 dto = pan9_5.GetData<HC_OSHA_CARD9_5>();

            if (dto.ID > 0)
            {
                if (MessageUtil.Confirm("삭제하시겠습니까?") == DialogResult.Yes)
                {
                    hcOshaCard95Service.Delete(dto);
                    pan9_5.Initialize();

                    Search9_5();
                }
            }
        }

        private void BtnNew9_5_Click(object sender, EventArgs e)
        {
            pan9_5.Initialize();
        }

        private void Search9_5_Clear()
        {
            int row = 25;
            SSCard.ActiveSheet.Cells[row, 0].Value = "";
            SSCard.ActiveSheet.Cells[row, 1].Value = "";
            SSCard.ActiveSheet.Cells[row, 3].Value = "";
            SSCard.ActiveSheet.Cells[row, 5].Value = "";
            SSCard.ActiveSheet.Cells[row, 7].Value = "";
        }
        //인정제도의 참여
        private void Search9_5()
        {
            Search9_5_Clear();

            if (base.SelectedEstimate.CONTRACTENDDATE == null)
            {
                return;
            }
            List<HC_OSHA_CARD9_5> list = hcOshaCard95Service.FindAll(base.SelectedSite.ID, base.SelectedEstimate.CONTRACTSTARTDATE.Left(4));
            if (list.Count > 0)
            {
                SSList9_5.SetDataSource(list);

                int row = 25;
                if( list.Count>0)
                {
                    SSCard.ActiveSheet.Cells[row, 0].Value = list[0].REQUESTDATE;
                    SSCard.ActiveSheet.Cells[row, 1].Value = list[0].APPROVEDATE;
                    SSCard.ActiveSheet.Cells[row, 3].Value = list[0].ISAPPROVE_TEXT;
                    SSCard.ActiveSheet.Cells[row, 5].Value = list[0].STARTDATE +"\n ~ " + list[0].ENDDATE;
                    SSCard.ActiveSheet.Cells[row, 7].Value = list[0].REMARK;
                }
            }
            else
            {
                SSList9_5.SetDataSource(new List<HC_OSHA_CARD9_5>());

           
            }
        }

        private void SSList9_5_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            HC_OSHA_CARD9_5 dto = SSList9_5.GetRowData(e.Row) as HC_OSHA_CARD9_5;
            pan9_5.SetData(dto);

        }
        #endregion

        #region 무재해운동추진

        private void Init_10()
        {
            
            SSList10.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SSList10.AddColumnDateTime("게시일자(재개시)", nameof(HC_OSHA_CARD10.PUBLISHDATE), 120, IsReadOnly.N, DateTimeType.YYYY_MM_DD, new SpreadCellTypeOption { IsShowCalendarButton = true, dateTimeEditorValue = FarPoint.Win.Spread.CellType.DateTimeEditorValue.String });
            SSList10.AddColumnText("목표시간(기간)", nameof(HC_OSHA_CARD10.GOAL), 150, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList10.AddColumnText("달성시간(기간)", nameof(HC_OSHA_CARD10.COMPLETE), 150, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList10.AddColumnText("목표달성(시상현황)", nameof(HC_OSHA_CARD10.STATUS), 150, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList10.AddColumnButton(" ", 60, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += (s, ev) => { 
                SSList10.DeleteRow(ev.Row); 
            };

            SearchCard10();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (base.SelectedSite == null || base.SelectedEstimate == null)
            {
                MessageUtil.Info("사업장 계약 정보를 선택하세요");
            }
            else
            {
                SSList10.AddRows();
            }
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (base.SelectedEstimate == null)
            {
                MessageUtil.Info("사업장 계약 정보를 선택하세요");
            }
            else
            {
                if (SSList10.Validate())
                {
                    IList<HC_OSHA_CARD10> list = SSList10.GetEditbleData<HC_OSHA_CARD10>();
                    if (list.Count > 0)
                    {
                        //if (hcOshaCard10Service.Save(list, base.SelectedEstimate.ID, base.SelectedSite.ID, base.GetCurrentYear()) )
                        if (hcOshaCard10Service.Save(list, base.SelectedEstimate.ID, base.SelectedSite.ID, base.SelectedEstimate.CONTRACTSTARTDATE.Left(4)))
                        {
                            SearchCard10();
                        }
                        else
                        {
                            MessageUtil.Alert("오류가 발생하였습니다. ");

                        }
                    }
                }
            }

        }
        private void SearchCard10_Clear()
        {
            int row = 29;
            for (int i = 0; i < 6; i++)
            {
                row = row + 1;
                SSCard.ActiveSheet.Cells[row, 0].Value = "";
                SSCard.ActiveSheet.Cells[row, 1].Value = "";
                SSCard.ActiveSheet.Cells[row, 2].Value = "";
                SSCard.ActiveSheet.Cells[row, 3].Value = "";
                SSCard.ActiveSheet.Cells[row, 4].Value = "";
                SSCard.ActiveSheet.Cells[row, 5].Value = "";
                SSCard.ActiveSheet.Cells[row, 6].Value = "";
                SSCard.ActiveSheet.Cells[row, 7].Value = "";
                SSCard.ActiveSheet.Cells[row, 8].Value = "";
                SSCard.ActiveSheet.Cells[row, 9].Value = "";
            }
        }
        //무재해운동추진
        private void SearchCard10()
        {
            SearchCard10_Clear();

            if (base.SelectedSite != null && base.SelectedEstimate != null)
            {
                if (base.SelectedEstimate.CONTRACTENDDATE == null)
                {
                    return;
                }
                //List<HC_OSHA_CARD10>  list = hcOshaCard10Service.FindAll(base.SelectedSite.ID, base.GetCurrentYear());
                List<HC_OSHA_CARD10> list = hcOshaCard10Service.FindAll(base.SelectedSite.ID, base.SelectedEstimate.CONTRACTSTARTDATE.Left(4));
                SSList10.SetDataSource(list);

                int row = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    SSCard.ActiveSheet.Cells[27, 3].Text = string.Empty;

                    row = i + 30;
                    SSCard.ActiveSheet.Cells[row, 0].Value = list[i].PUBLISHDATE;
                    SSCard.ActiveSheet.Cells[row, 1].Value = list[i].GOAL;
                    SSCard.ActiveSheet.Cells[row, 2].Value = list[i].COMPLETE;
              //      SSCard.ActiveSheet.Cells[row, 3].Value = list[i].STATUS;
                    SSCard.ActiveSheet.Cells[row, 4].Value = list[i].STATUS;
                }
                if (list.Count == 0)
                {
                    SSCard.ActiveSheet.Cells[27, 3].Text = "10. 무재해운동추진 - 해당없음";
                }
            }
            else
            {
                SSList10.SetDataSource(new List<HC_OSHA_CARD10>());
            }
        }
        #endregion

        public void Print()
        {
            Search9_1();
            Search9_2();
            Search9_3();
            Search9_4();
            Search9_5();
            SearchCard10();

            SpreadPrint print = new SpreadPrint(SSCard, PrintStyle.FORM, false);
            print.Execute();
        }

        public bool NewPrint()
        {
            Search9_1();
            Search9_2();
            Search9_3();
            Search9_4();
            Search9_5();
            SearchCard10();

            SpreadPrint print = new SpreadPrint(SSCard, PrintStyle.FORM, false);
            print.Execute();
            return true;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Print();
        }

        private void pan9_1_Paint(object sender, PaintEventArgs e)
        {

        }
    }

}
