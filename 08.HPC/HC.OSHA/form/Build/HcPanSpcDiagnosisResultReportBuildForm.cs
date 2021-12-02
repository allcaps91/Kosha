using ComBase;
using ComBase.Mvc.Utils;
using ComHpcLibB;
using ComHpcLibB.Model;
using ComHpcLibB.Repository;
using ComHpcLibB.Service;
using HC.OSHA.Dto;
using HC.OSHA.Model;
using HC.OSHA.Repository;
using HC_Core;
using System;
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
    public partial class HcPanSpcDiagnosisResultReportBuildForm : CommonForm
    {    //특수검진표
        frmHcPanSpcDiagnosisResultReport frmHcPanSpcDiagnosisResultReport;
        HicJepsuResSpecialLtdService hicJepsuResSpecialLtdService;
        HicOshaSpecialResultRepository hicOshaSpecialResultRepository;
        HcOshaSiteRepository hcOshaSiteRepository;
        public HcPanSpcDiagnosisResultReportBuildForm()
        {
            InitializeComponent();
            this.hicJepsuResSpecialLtdService = new HicJepsuResSpecialLtdService();
            this.frmHcPanSpcDiagnosisResultReport = new frmHcPanSpcDiagnosisResultReport();
            this.hicOshaSpecialResultRepository = new HicOshaSpecialResultRepository();
            hcOshaSiteRepository = new HcOshaSiteRepository();
        }
        public void Build(bool isAuto)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                string strYear = CboYear.Text; //siteStatusControl.VisitDate;
                string strFDate = strYear + "-01-01";
                string strTDate = strYear + "-12-31";

                if (strYear == "")
                {
                    MessageUtil.Alert("년도가 없습니다");
                    return;
                }

                List<HIC_JEPSU_RES_SPECIAL_LTD> list = hicJepsuResSpecialLtdService.GetItemCountbyJepDateGjYearGjBangi(strFDate, strTDate, strYear, "", "1", "");
                foreach (HIC_JEPSU_RES_SPECIAL_LTD model in list)
                {
                    try
                    {
                        HC_OSHA_SITE site = hcOshaSiteRepository.FindById(model.LTDCODE);
                        if (site != null)
                        {
                            if (model.LTDCODE == 2088)
                            {
                                string x = "";
                            }
                            DESEASE_COUNT_MODEL count = frmHcPanSpcDiagnosisResultReport.Search(model.MINDATE, model.MAXDATE, strYear, model.LTDCODE);

                            HIC_OSHA_SPECIAL_RESULT dto = new HIC_OSHA_SPECIAL_RESULT();
                            dto.YEAR = strYear;
                            dto.SITE_ID = model.LTDCODE;
                            dto.D2COUNT = count.D2;
                            dto.C2COUNT = count.C2;
                            dto.C1COUNT = count.C1;
                            dto.D1COUNT = count.D1;
                            dto.D2COUNT = count.D2;
                            dto.DNCOUNT = count.DN;
                            dto.CNCOUNT = count.CN;
                            dto.TOTALCOUNT = count.SpecialTotalCount;
                            hicOshaSpecialResultRepository.Delete(strYear, model.LTDCODE);
                            hicOshaSpecialResultRepository.Insert(dto);
                        }

                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                    }
                }

                if (isAuto == false)
                {
                    MessageUtil.Alert("특별건강진단 유소견자수 빌드 완료");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                if (isAuto == false)
                {
                    MessageUtil.Alert(ex.Message);

                }

            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
                Build(false);
        }

        private void HcPanSpcDiagnosisResultReportBuildForm_Load(object sender, EventArgs e)
        {
            DateTime dateTime = codeService.CurrentDate;
            for (int i = 0; i <= 5; i++)
            {
                CboYear.Items.Add(dateTime.AddYears(-i).ToString("yyyy"));
            }
            CboYear.SelectedIndex = 0;
        }
    }
}
