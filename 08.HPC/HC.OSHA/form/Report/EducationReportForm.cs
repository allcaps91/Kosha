using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Utils;
using FarPoint.Win.Spread;
using HC.Core.Dto;
using HC.Core.Service;
using HC.Core.Common.Interface;
using HC_Core;
using HC.Core.Common.Util;
using HC.Core.Model;
using HC.OSHA.Dto;
using HC.OSHA.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace HC_OSHA
{
    public partial class EducationReportForm : BaseForm
    {
        HcOshaVisitEduService hcOshaVisitEduService;
        public EducationReportForm()
        {
            InitializeComponent();
            hcOshaVisitEduService = new HcOshaVisitEduService();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            IList list = hcOshaVisitEduService.hcOshaVisitEduRepository.FindAll(DtpStartDate.GetValue(), DtpEndDate.GetValue());

            SSList.SetDataSource(list);

        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            //string strTitle = "장기 진료 환자 명단";
            //string strHeader = "";
            //string strFooter = "";
            //bool PrePrint = true;

            //clsSpread CS = new clsSpread();
            //clsSpread.SpdPrint_Margin setMargin;
            //clsSpread.SpdPrint_Option setOption;

            //strHeader = CS.setSpdPrint_String(strTitle, new Font("맑은 고딕", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            //strHeader += CS.setSpdPrint_String("작업일자: ", new Font("맑은 고딕", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Left, false, false);
            //strHeader += CS.setSpdPrint_String("출력시간: " + Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).ToString("yyyy-MM-dd HH:mm") + " 출력자 : " + clsType.User.UserName, new Font("맑은 고딕", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, false);
            //strHeader += CS.setSpdPrint_String("PAGE : /p ", new Font("맑은 고딕", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, false);

            //setMargin = new clsSpread.SpdPrint_Margin(10, 10, 90, 200, 20, 10);
            //setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, true, false, true, false);

            // CS.setSpdPrint(SSList, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);

            SpreadPrint print = new SpreadPrint(SSList, PrintStyle.STANDARD_REPORT, true);
            print.Title = "보건교육지원 대장";
            print.Execute(SSList.ActiveSheet);
            
        }

        private void EducationReportForm_Load(object sender, EventArgs e)
        {
            SSList.Initialize(new SpreadOption() { IsRowSelectColor = true, RowHeaderVisible = true  });

            SSList.AddColumnText("교육일자", nameof(HC_OSHA_VISIT_EDU.EDUDATE), 120, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true });
            SSList.AddColumnText("종류", nameof(HC_OSHA_VISIT_EDU.EDUTYPE), 49, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("교육주제", nameof(HC_OSHA_VISIT_EDU.TITLE), 200, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("참석자", nameof(HC_OSHA_VISIT_EDU.TARGET), 125, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("교육장소", nameof(HC_OSHA_VISIT_EDU.LOCATION), 150, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("실시자", nameof(HC_OSHA_VISIT_EDU.EDUUSERNAME), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });

            DtpStartDate.SetValue(DateTime.Now.AddMonths(-6));
            DtpEndDate.SetValue(DateTime.Now);

            BtnSearch.PerformClick();
        }
    }
}
