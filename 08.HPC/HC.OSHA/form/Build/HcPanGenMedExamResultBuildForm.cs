using ComBase;
using ComBase.Mvc.Utils;
using ComHpcLibB;
using ComHpcLibB.Model;
using ComHpcLibB.Repository;
using ComHpcLibB.Service;
using HC.Core.Service;
using HC.OSHA.Dto;
using HC.OSHA.Model;
using HC.OSHA.Repository;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HC_OSHA
{
    public partial class HcPanGenMedExamResultBuildForm : Form
    {
        HicJepsuLtdService hicJepsuLtdService;
        HicOshaGeneralResultRepository hicOshaGeneralResultRepository;
        //일반검진표
        frmHcPanGenMedExamResult_New frmHcPanGenMedExamResult_New;
        HcOshaSiteRepository hcOshaSiteRepository;

        public HcPanGenMedExamResultBuildForm()
        {
            InitializeComponent();
            this.hicJepsuLtdService = new HicJepsuLtdService();
            this.hcOshaSiteRepository = new HcOshaSiteRepository();
            this.hicOshaGeneralResultRepository = new HicOshaGeneralResultRepository();
            this.frmHcPanGenMedExamResult_New = new frmHcPanGenMedExamResult_New();
           

        }

        private void HcPanGenMedExamResultBuildForm_Load(object sender, EventArgs e)
        {
            HcCodeService codeService = new HcCodeService();
            DateTime dateTime = codeService.CurrentDate;
            for (int i = 0; i <= 5; i++)
            {
                CboYear.Items.Add(dateTime.AddYears(-i).ToString("yyyy"));
            }
            CboYear.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Build(false);

        }
        public void Build(bool isAuto)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                string strYear = CboYear.Text;
                string strFDate = strYear + "-01-01";
                string strTDate = strYear + "-12-31";
                string strBangi = "";

                if (strYear=="")
                {
                    MessageUtil.Alert("년도가 없습니다");
                    return;
                }

                List<HIC_JEPSU_LTD> list2 = hicJepsuLtdService.GetItembyJepDateGjYearGjBangiLtdCode_New(strFDate, strTDate, strYear, strBangi,0);
                //list2 = list2.FindAll(r => r.LTDCODE.Equals(127));
                foreach (HIC_JEPSU_LTD model in list2)
                {
                    HC_OSHA_SITE site = hcOshaSiteRepository.FindById(model.LTDCODE);
                    if (site != null)
                    {
                        try
                        {
                            DESEASE_COUNT_MODEL count = frmHcPanGenMedExamResult_New.Search(model.MINDATE, model.MAXDATE, strYear, model.LTDCODE);

                            HIC_OSHA_GENEAL_RESULT dto = new HIC_OSHA_GENEAL_RESULT();
                            dto.YEAR = strYear;
                            dto.SITE_ID = model.LTDCODE;
                            dto.D2COUNT = count.D2;
                            dto.C2COUNT = count.C2;
                            dto.TOTALCOUNT = count.GeneralTotalCount;
                            hicOshaGeneralResultRepository.Delete(dto.YEAR, dto.SITE_ID);
                            hicOshaGeneralResultRepository.Insert(dto);
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex);
                        }
                    }
                }

                if (isAuto == false)
                {
                    MessageUtil.Alert(" 일반건강진단 유소견자수 빌드 완료");
                }
            }
            catch(Exception ex)
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
    }
}
