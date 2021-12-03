using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Utils;
using ComHpcLibB;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using HC.Core.Dto;
using HC.Core.Repository;
using HC.Core.Service;
using HC.OSHA.Dto;
using HC.OSHA.Model;
using HC.OSHA.Repository;
using HC.OSHA.Service;
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
    /// <summary>
    /// 질병유소견자 대행 빌드
    /// 요관찰자 직업병(C1), 요관찰자 야간(CN), 요관찰자 일반(C2)
    /// 질병 직업병(D1), 질병 야간(DN), 질병일반(D2)
    /// </summary>
    public partial class PanjengBuildForm : CommonForm
    {
        private HicJepsuLtdResBohum1Service hicJepsuLtdResBohum1Service;
        private frmHcPanOpinionAfterMgmtGenSpc form;
        private HicOshaPanjengRepository hicOshaPanjengRepository;
        private HcSiteWorkerService hcSiteWorkerService;
        private HcOshaSiteService hcOshaSiteService;
        public PanjengBuildForm()
        {
            InitializeComponent();
            hicJepsuLtdResBohum1Service = new HicJepsuLtdResBohum1Service();
            form = new frmHcPanOpinionAfterMgmtGenSpc();
            hicOshaPanjengRepository = new HicOshaPanjengRepository();
            hcSiteWorkerService = new HcSiteWorkerService();
            hcOshaSiteService = new HcOshaSiteService();
        }

        public void CheckAll()
        {
            for (int i = 0; i < SSSiteList.RowCount(); i++)
            {
                SSSiteList.ActiveSheet.Cells[i, 0].Value = true;
            }
        }
        public void Search()
        {
            string strFrDate = DtpStartDate.Text;
            string strToDate = DtpEndDate.Text;
            string strGjYear = CboYear.Text;
            long siteId = 0;
            
            SSSiteList.ActiveSheet.RowCount = 0;

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                List<HIC_JEPSU_LTD_RES_BOHUM1> jepList = new List<HIC_JEPSU_LTD_RES_BOHUM1>();

                List<HC_OSHA_SITE_MODEL> list = hcOshaSiteService.Search("", "", true,false);
                foreach (HC_OSHA_SITE_MODEL model in list)
                {
                    List<HIC_JEPSU_LTD_RES_BOHUM1> jep = hicJepsuLtdResBohum1Service.GetItembyJepDateGjYearGjBangi_GenSpc(strFrDate, strToDate, strGjYear, "", "1", model.ID);
                    foreach (HIC_JEPSU_LTD_RES_BOHUM1 res in jep)
                    {
                        jepList.Add(res);
                    }
                }

                SSSiteList.SetDataSource(jepList);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                MessageUtil.Alert(ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
         
        }
        public void Build(bool isAuto)
        {
            try
            {
                int checkedCount = 0;
                Cursor.Current = Cursors.WaitCursor;
                for (int i = 0; i < SSSiteList.RowCount(); i++)
                {
                    if (SSSiteList.ActiveSheet.Cells[i, 0].Value == null)
                    {
                        continue;
                    }
                    if (SSSiteList.ActiveSheet.Cells[i, 0].Value.Equals(true))
                    {
                        checkedCount++;
                        HIC_JEPSU_LTD_RES_BOHUM1 jepsu = SSSiteList.GetRowData(i) as HIC_JEPSU_LTD_RES_BOHUM1;

                        FpSpread spead = form.GetPanjeongBySite(jepsu.MINDATE, jepsu.MAXDATE, jepsu.LTDCODE.ToString(), CboYear.Text);
                        if (jepsu.LTDCODE == 8222)
                        {
                            string name = "영일";
                        }

                        hicOshaPanjengRepository.Delete(CboYear.Text, jepsu.LTDCODE);

                        List<HIC_OSHA_PANJEONG> patientList = new List<HIC_OSHA_PANJEONG>();
                        for (int row = 0; row < spead.ActiveSheet.RowCount; row++)
                        {
                            string name = spead.ActiveSheet.Cells[row, 1].Text;
                            if (!spead.ActiveSheet.Cells[row, 7].Text.IsNullOrEmpty())
                            {

                                HIC_OSHA_PANJEONG dto = new HIC_OSHA_PANJEONG();
                                if (spead.ActiveSheet.Cells[row, 11].Text.IsNullOrEmpty())
                                {
                                    //환자번호가 없으면 이전 로우의 환자정보를 사용
                                    dto.PANO = patientList[patientList.Count - 1].PANO; //spead.ActiveSheet.Cells[index, 11].Text;
                                }
                                else
                                {
                                    dto.PANO = spead.ActiveSheet.Cells[row, 11].Text;
                                }
                                if (dto.PANO == "150025")
                                {
                                    string x = "";
                                }

                                //HC_SITE_WORKER worker = hcSiteWorkerService.CopyPatient(jepsu.LTDCODE, dto.PANO, jepsu.MINDATE, jepsu.MAXDATE);
                                HC_SITE_WORKER worker = hcSiteWorkerService.FindOneByPano(dto.PANO);
                                if (worker != null)
                                {
                                    dto.WORKER_ID = worker.ID;
                                    dto.YEAR = CboYear.Text;
                                    if (spead.ActiveSheet.Cells[row, 11].Text.IsNullOrEmpty())
                                    {
                                        //환자번호가 없으면 이전 로우의 환자정보를 사용
                                        dto.PANO = patientList[patientList.Count - 1].PANO;
                                        dto.TASK = patientList[patientList.Count - 1].TASK;
                                        dto.NAME = patientList[patientList.Count - 1].NAME;
                                        dto.SEX = patientList[patientList.Count - 1].SEX;
                                        dto.AGE = patientList[patientList.Count - 1].AGE;
                                    }
                                    else
                                    {
                                        dto.TASK = spead.ActiveSheet.Cells[row, 0].Text;
                                        dto.NAME = spead.ActiveSheet.Cells[row, 1].Text;
                                        dto.SEX = spead.ActiveSheet.Cells[row, 2].Text;
                                        dto.AGE = spead.ActiveSheet.Cells[row, 3].Text;

                                    }


                                    dto.INJA = spead.ActiveSheet.Cells[row, 5].Text;
                                    dto.JIPYO = spead.ActiveSheet.Cells[row, 6].Text;
                                    dto.JOBYEAR = spead.ActiveSheet.Cells[row, 4].Text;
                                    dto.PANJEONG = spead.ActiveSheet.Cells[row, 7].Text;
                                    dto.OPINION = spead.ActiveSheet.Cells[row, 8].Text;
                                    dto.RESULT = spead.ActiveSheet.Cells[row, 9].Text;
                                    dto.GRADE = spead.ActiveSheet.Cells[row, 10].Text;
                                    dto.SITENAME = jepsu.NAME;
                                    dto.SITE_ID = jepsu.LTDCODE;

                                    if (dto.INJA == "▶일반건진")
                                    {

                                        dto.ISSPECIAL = "N";
                                    }
                                    else if (patientList.Count > 0 && dto.INJA.IsNullOrEmpty())
                                    {
                                        if (patientList[patientList.Count - 1].INJA == "▶일반건진")
                                        {
                                            dto.ISSPECIAL = "N";
                                        }
                                        else
                                        {
                                            dto.ISSPECIAL = "Y";
                                        }
                                    }
                                    else
                                    {
                                        dto.ISSPECIAL = "Y";
                                    }
                                    if (dto.INJA.IsNullOrEmpty())
                                    {
                                        dto.INJA = patientList[patientList.Count - 1].INJA;
                                    }
                                    if (!dto.PANO.IsNullOrEmpty())
                                    {
                                        patientList.Add(dto);
                                    }
                                    hicOshaPanjengRepository.Insert(dto);
                                }

                            }
                        }
                    }

                   
                }//for
                if (checkedCount > 0)
                {
                    if (isAuto == false)
                    {
                        MessageUtil.Info(" 판정 빌드 완료 ");
                    }
                    
                }
                
            }
            catch(Exception ex)
            {
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

        private void SSWorkerList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

        }

        private void PanjengBuildForm_Load(object sender, EventArgs e)
        {
            DateTime dateTime = codeService.CurrentDate;
            for (int i = 0; i <= 5; i++)
            {
                CboYear.Items.Add(dateTime.AddYears(-i).ToString("yyyy"));
            }
            CboYear.SelectedIndex = 0;

            DtpStartDate.Text = VB.Left(clsPublic.GstrSysDate, 4) + "-01-01";
            DtpEndDate.Text = clsPublic.GstrSysDate;

            SSSiteList.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SSSiteList.AddColumnCheckBox("", "", 30, new CheckBoxBooleanCellType());
            SSSiteList.AddColumnText("회사코드", nameof(HIC_JEPSU_LTD_RES_BOHUM1.LTDCODE), 75, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
            SSSiteList.AddColumnText("회사명", nameof(HIC_JEPSU_LTD_RES_BOHUM1.NAME), 190, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
            SSSiteList.AddColumnText("건수", nameof(HIC_JEPSU_LTD_RES_BOHUM1.CNT), 190, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
            

        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void BtnBuild_Click(object sender, EventArgs e)
        {
            Build(false);
        }

        private void CboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            DtpStartDate.Value = DateUtil.stringToDateTime(CboYear.Text + "-01" + "-01", DateTimeType.YYYY_MM_DD);
        }
    }
}
