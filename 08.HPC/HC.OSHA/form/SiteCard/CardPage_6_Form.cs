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
using HC.Core.Service;
using ComHpcLibB.Model;

namespace HC_OSHA
{
    public partial class CardPage_6_Form : CommonForm, ISelectSite, ISelectEstimate, IPrint
    {
        private HcOshaCard7Service hcOshaCard7Service;
        private HcOshaVisitCommitteeService hcOshaVisitCommitteeService;
        private HcUserService hcUserService;
        public CardPage_6_Form()
        {
            InitializeComponent();
            hcOshaCard7Service = new HcOshaCard7Service();
            hcOshaVisitCommitteeService = new HcOshaVisitCommitteeService();
            hcUserService = new HcUserService();
        }

        private void CardPage_6_Form_Load(object sender, EventArgs e)
        {
            SSList.Initialize(new SpreadOption() { IsRowSelectColor = false, RowHeight = 50 }) ;
            SSList.AddColumnText("구분", nameof(HC_OSHA_CARD7.RULETYPE), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnDateTime("제.개정일", nameof(HC_OSHA_CARD7.RULEDATE), 100, IsReadOnly.N, DateTimeType.YYYY_MM_DD, new SpreadCellTypeOption { IsShowCalendarButton = true, dateTimeEditorValue = FarPoint.Win.Spread.CellType.DateTimeEditorValue.String });
            SSList.AddColumnText("주요내용", nameof(HC_OSHA_CARD7.CONTENT), 350, IsReadOnly.N, new SpreadCellTypeOption { IsMulti=true,IsSort = false });
            SSList.AddColumnButton(" ", 60, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += (s, ev) => { SSList.DeleteRow(ev.Row); };

      
            SSList2.Initialize(new SpreadOption() { IsRowSelectColor = false, RowHeight= 50 });
            SSList2.AddColumnDateTime("회의개최일", nameof(HC_OSHA_CARD7_1.MEETDATE), 100, IsReadOnly.N, DateTimeType.YYYY_MM_DD, new SpreadCellTypeOption { IsShowCalendarButton = true, dateTimeEditorValue = FarPoint.Win.Spread.CellType.DateTimeEditorValue.String });
            SSList2.AddColumnText("정기임시회의여부", nameof(HC_OSHA_CARD7_1.ISREGULAR), 200, IsReadOnly.N, new SpreadCellTypeOption { IsMulti = false, IsSort = false });
            SSList2.AddColumnText("주요의견", nameof(HC_OSHA_CARD7_1.CONTENT), 350, IsReadOnly.N, new SpreadCellTypeOption { IsMulti = true, IsSort = false, Aligen = CellHorizontalAlignment.Left });
            SSList2.AddColumnButton(" ", 60, new SpreadCellTypeOption { ButtonText = "참석" }).ButtonClick += CardPage_6_Form_ButtonClick;
            SSList2.AddColumnButton(" ", 60, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += (s, ev) => { SSList2.DeleteRow(ev.Row); };

            Search();
        }

        //참석시 방문등록 산업안전보건위원회 대장에 등록
        private void CardPage_6_Form_ButtonClick(object sender, EditorNotifyEventArgs e)
        {
            HC_OSHA_CARD7_1 card = SSList2.GetRowData(e.Row) as HC_OSHA_CARD7_1;
            if (card.ID > 0)
            {
                HC_OSHA_VISIT_COMMITTEE dto = new HC_OSHA_VISIT_COMMITTEE();
                dto.REGDATE = card.MEETDATE;
                dto.MEETINGUSER = hcUserService.FindByUserId(CommonService.Instance.Session.UserId).Name;
                if (card.ISREGULAR == "정기")
                {
                    dto.MEETINGKIND = "1";
                }
                else
                {
                    dto.MEETINGKIND = "2"; // 임시
                }

                dto.METTINGTYPE = "1";
                dto.SITE_ID = base.SelectedSite.ID;
                hcOshaVisitCommitteeService.Save(dto);

                MessageUtil.Info(" 산업안전 보건 위원회 대장에 저장되었습니다. ");
            }
            else
            {
                MessageUtil.Alert(" 저장후 참석을 눌러 주세요");

            }
        }

        public void Select(IEstimateModel estimateModel)
        {
            base.SelectedEstimate = estimateModel;

            Search();
        }

        public void Select(ISiteModel siteModel)
        {
            base.SelectedSite = siteModel;
            Clear();
        }

        private void Clear()
        {
            int rowIndex = 3;
            for (int i = 0; i < 8; i++)
            {
                SSCard.ActiveSheet.Cells[rowIndex + i, 0].Text = string.Empty;
                SSCard.ActiveSheet.Cells[rowIndex + i, 4].Text = string.Empty;
                SSCard.ActiveSheet.Cells[rowIndex + i, 8].Text = string.Empty;
            }

            SSList.SetDataSource(new List<HC_OSHA_CARD7>());
            SSList2.SetDataSource(new List<HC_OSHA_CARD7_1>());

            for (int i = 0; i < 4; i++)
            {
                SSCard.ActiveSheet.Cells[i + 15, 0].Value = "";
                SSCard.ActiveSheet.Cells[i + 15, 4].Value = "";
                SSCard.ActiveSheet.Cells[i + 15, 7].Value = "";
                SSCard.ActiveSheet.Cells[i + 15,11].Value = "";
            }
            for (int i = 0; i < 4; i++)
            {
                SSCard.ActiveSheet.Cells[i + 19, 0].Value = "";
                SSCard.ActiveSheet.Cells[i + 19, 4].Value = "";
                SSCard.ActiveSheet.Cells[i + 19, 7].Value = "";
                SSCard.ActiveSheet.Cells[i + 19, 11].Value = "";
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (base.SelectedSite == null || base.SelectedEstimate == null)
            {
                MessageUtil.Info("사업장 계약 정보를 선택하세요");
            }
            else
            {
                SSList.AddRows();
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
                if (SSList.Validate())
                {
                    IList<HC_OSHA_CARD7> list = SSList.GetEditbleData<HC_OSHA_CARD7>();
                    if (list.Count > 0)
                    {
                        if (hcOshaCard7Service.Save(list, base.SelectedEstimate.ID, base.SelectedSite.ID))
                        {
                        //    MessageUtil.Info("저장하였습니다");
                            Search();
                        }
                        else
                        {
                            MessageUtil.Alert("오류가 발생하였습니다. ");

                        }
                    }
                }
            }
        }

        private void Search()
        {
            Clear();

            if (base.SelectedSite != null && base.SelectedEstimate != null)
            {
                if(base.SelectedEstimate.CONTRACTENDDATE != null)
                {
                    List<HC_OSHA_CARD7> list = hcOshaCard7Service.FindAll(base.SelectedEstimate.ID);
                    SSList.SetDataSource(list);

                    //  내용이 없으면 제목 표시줄에 해당없음 표시
                    SSCard.ActiveSheet.Cells[1, 0].Text = "7. 안전보건관리규정 제·개정내용 - 해당없음";

                    int rowIndex = 3;
                    for (int i = 0; i < list.Count; i++)
                    {
                        SSCard.ActiveSheet.Cells[1, 0].Text = "7. 안전보건관리규정 제·개정내용";

                        SSCard.ActiveSheet.Cells[rowIndex, 0].Text = list[i].RULETYPE;
                        SSCard.ActiveSheet.Cells[rowIndex, 4].Text = list[i].RULEDATE;
                        SSCard.ActiveSheet.Cells[rowIndex, 8].Text = list[i].CONTENT;

                        rowIndex += 1;
                    }

                    List<HC_OSHA_CARD7_1> list2 = hcOshaCard7Service.FindAllByMeet(base.SelectedEstimate.ID);
                    SSList2.SetDataSource(list2);

                   SetMeet();
                }
            }
            else
            {
                //  내용이 없으면 제목 표시줄에 해당없음 표시
                SSCard.ActiveSheet.Cells[1, 0].Text = "7. 안전보건관리규정 제·개정내용 - 해당없음";
                SSCard.ActiveSheet.Cells[13, 0].Text = "8. 산업안전보건위원회 운영 - 해당없음";

                SSList.SetDataSource(new List<HC_OSHA_CARD7>());
                SSList2.SetDataSource(new List<HC_OSHA_CARD7_1>());
            }
        }

        private void SetMeet()
        {
            string year = base.SelectedEstimate.CONTRACTSTARTDATE.Left(4);// GetCurrentYear();
            string lastYear = GetLastYear(year);

            string lastStartYear = lastYear + "-01-01 00:00:00";
            string lastEndYear = lastYear + "-12-31 23:59:59";
            //작년
            List<HC_OSHA_CARD7_1> list = hcOshaCard7Service.FindAllByMeet(base.SelectedEstimate.ID, lastStartYear, lastEndYear);

            //  내용이 없으면 제목표시줄에 해당없음 표시
            SSCard.ActiveSheet.Cells[13, 0].Text = "8. 산업안전보건위원회 운영 - 해당없음";
            for (int i=0; i<list.Count; i++)
            {
                SSCard.ActiveSheet.Cells[13, 0].Text = "8. 산업안전보건위원회 운영";

                SSCard.ActiveSheet.Cells[i + 15, 0].Value = lastYear.Substring(2, 2);
                SSCard.ActiveSheet.Cells[i + 15, 4].Value = list[i].MEETDATE;
                SSCard.ActiveSheet.Cells[i + 15, 7].Value = list[i].ISREGULAR;
                SSCard.ActiveSheet.Cells[i + 15, 11].Value = list[i].CONTENT;
            }

            //금년
            string startYear = year + "-01-01 00:00:00";
            string endYear = year + "-12-31 23:59:59";
            List<HC_OSHA_CARD7_1> list2 = hcOshaCard7Service.FindAllByMeet(base.SelectedEstimate.ID, startYear, endYear);

            for (int i = 0; i < list2.Count; i++)
            {
                SSCard.ActiveSheet.Cells[13, 0].Text = "8. 산업안전보건위원회 운영";

                SSCard.ActiveSheet.Cells[i + 19, 0].Value = year.Substring(2, 2);
                SSCard.ActiveSheet.Cells[i + 19, 4].Value = list2[i].MEETDATE;
                SSCard.ActiveSheet.Cells[i + 19, 7].Value = list2[i].ISREGULAR;
                SSCard.ActiveSheet.Cells[i + 19, 11].Value = list2[i].CONTENT;
            }
        }

        public void Print()
        {
            Search();
            SpreadPrint print = new SpreadPrint(SSCard, PrintStyle.FORM, false);
            print.Execute();
        }

        public bool NewPrint()
        {
            Search();
            SpreadPrint print = new SpreadPrint(SSCard, PrintStyle.FORM, false);
            print.Execute();
            return true;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Print();
        }

        private void BtnSaveCommit_Click(object sender, EventArgs e)
        {
            if (base.SelectedEstimate == null)
            {
                MessageUtil.Info("사업장 계약 정보를 선택하세요");
            }
            else
            {
                if (SSList2.Validate())
                {
                    IList<HC_OSHA_CARD7_1> list = SSList2.GetEditbleData<HC_OSHA_CARD7_1>();
                    if (list.Count > 0)
                    {
                        //if (hcOshaCard7Service.SaveMeet(list, base.SelectedEstimate.ID, base.SelectedSite.ID, base.GetCurrentYear()))
                        if (hcOshaCard7Service.SaveMeet(list, base.SelectedEstimate.ID, base.SelectedSite.ID, base.SelectedEstimate.CONTRACTSTARTDATE.Left(4)))
                        {
                            Search();
                        }
                        else
                        {
                            MessageUtil.Alert("오류가 발생하였습니다. ");

                        }
                    }
                }
            }
        }

        private void BtnAddCommit_Click(object sender, EventArgs e)
        {
            if (base.SelectedSite == null || base.SelectedEstimate == null)
            {
                MessageUtil.Info("사업장 계약 정보를 선택하세요");
            }
            else
            {
                SSList2.AddRows();
            }
        }
    }
}
