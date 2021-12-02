using System;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using HC.OSHA.Model;
using HC.OSHA.Service;
using HC_Core;
using HC.OSHA.Dto;
using ComBase.Mvc.Utils;
using ComBase.Mvc.Spread;
using System.Collections.Generic;
using System.Collections;
using HC_OSHA;
using HC.Core.Model;
using HC.Core.Common.Interface;
using ComBase;
using HC.Core.Dto;
using ComHpcLibB.Model;
using HC.Core.Service;

namespace HC_OSHA
{
    public partial class SiteWorkerForm : CommonForm, ISelectSite
    {

        private HcSiteWorkerService hcSiteWorkerService;
        
        public SiteWorkerForm()
        {
            InitializeComponent();

            hcSiteWorkerService = new HcSiteWorkerService();
        
        }
       
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (SelectedSite.ID > 0)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    hcSiteWorkerService.CopyHealthCheck(SelectedSite.ID);

                    MessageUtil.Info("근로자 목록이 복사 되었습니다");
                    
                    SetDeptCombo();

                    Search();
                }
                catch(Exception ex)
                {
                    MessageUtil.Alert(ex.Message);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
               
            }
          
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if(SelectedSite != null)
            {
                SSWorkerList.AddRows();
            }
            else
            {
                MessageUtil.Alert("사업장을 선택하세요");
            }
        }
        private void SetDeptCombo()
        {
            if (base.SelectedSite == null)
            {
                return;
            }
            List<HC_SITE_WORKER> list = hcSiteWorkerService.hcSiteWorkerRepository.FindAllGroupByDept(base.SelectedSite.ID);
            CboDept.Items.Clear();
            CboDept.Items.Add("전체");
            foreach (HC_SITE_WORKER worker in list)
            {
                if (worker.DEPT != null )
                {
                    if (worker.DEPT !="")
                    {
                        if (!CboDept.Items.Contains(worker.DEPT))
                        {
                            CboDept.Items.Add(worker.DEPT);
                        }
                    }
                }              
            }
            CboDept.SelectedIndex = 0;


        }
        private void SiteWorkerForm_Load(object sender, EventArgs e)
        {
            TxtNameOrPano.SetExecuteButton(BtnSearch);

            SpreadComboBoxData comboBoxData = codeService.GetSpreadComboBoxData("WORKER_ROLE", "OSHA");
            
            SSWorkerList.Initialize(new SpreadOption() { IsRowSelectColor = false });
            //성별.나이 추가해야함            
            SSWorkerList.AddColumnText("등록번호", nameof(HC_SITE_WORKER.ID), 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true, sortIndicator = FarPoint.Win.Spread.Model.SortIndicator.Ascending });
            SSWorkerList.AddColumnText("PTNO", nameof(HC_SITE_WORKER.PTNO), 70, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true, IsVisivle = false, sortIndicator = FarPoint.Win.Spread.Model.SortIndicator.Ascending }) ;
            SSWorkerList.AddColumnText("이름", nameof(HC_SITE_WORKER.NAME), 75, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true, sortIndicator = FarPoint.Win.Spread.Model.SortIndicator.Ascending });
            SSWorkerList.AddColumnText("부서", nameof(HC_SITE_WORKER.DEPT), 140, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSWorkerList.AddColumnComboBox("직책", nameof(HC_SITE_WORKER.WORKER_ROLE), 150, IsReadOnly.N, comboBoxData, new SpreadCellTypeOption { IsSort = false });
            SSWorkerList.AddColumnText("주민번호", nameof(HC_SITE_WORKER.JUMIN), 128, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSWorkerList.AddColumnText("전화", nameof(HC_SITE_WORKER.TEL), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });            
            SSWorkerList.AddColumnText("휴대폰", nameof(HC_SITE_WORKER.HP), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSWorkerList.AddColumnText("이메일", nameof(HC_SITE_WORKER.EMAIL), 200, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
        //    SSWorkerList.AddColumnText("입사일", nameof(HC_SITE_WORKER.IPSADATE), 85, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
        //    SSWorkerList.AddColumnCheckBox("퇴사여부", nameof(HC_SITE_WORKER.ISRETIRE), 35, new CheckBoxStringCellType { IsHeaderCheckBox = false, CheckedValue="Y", UnCheckedValue="N"}, new SpreadCellTypeOption { IsSort = false,  });
            SSWorkerList.AddColumnButton("삭제", 60, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += SSWorkerListDelete_ButtonClick;
            

            SSWorkerList.SetDataSource(new List<HC_SITE_WORKER>());


            List<HC_CODE> roles = codeService.FindActiveCodeByGroupCode("WORKER_ROLE", "OSHA");
            CboRole.SetItems(roles, "CodeName", "Code", "전체", "", AddComboBoxPosition.Top);
            CboRole.SelectedIndex = 0;

            Search();
        }

        private void SSWorkerListDelete_ButtonClick(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            HC_SITE_WORKER dto =  SSWorkerList.GetRowData(e.Row) as HC_SITE_WORKER;
            if(dto.ID != null)
            {
                if (!dto.ID.IsNumeric())
                {
                    SSWorkerList.DeleteRow(e.Row);
                }
                else
                {
                    MessageUtil.Alert("삭제할 수 없습니다");
                }
            }
            else
            {
                SSWorkerList.DeleteRow(e.Row);
            }
      
            
           
        }

        private void Search()
        {
           if(base.SelectedSite == null)
            {
                SSWorkerList.SetDataSource(new List<HC_SITE_WORKER> ());
            }
            else
            {
                List<HC_SITE_WORKER> list = hcSiteWorkerService.FindAll(base.SelectedSite.ID, TxtNameOrPano.Text, CboDept.GetValue(), CboRole.GetValue());
                foreach(HC_SITE_WORKER w in list)
                {//"4909181117528"  91WWPnCPpgbymOHlAtr7zi47D7Kr0cmnt7EAc7t8g0g=
                    if (w.JUMIN != null)
                    {
                        string jumin = clsAES.DeAES(w.JUMIN);
                        if (w.NAME == "f")
                        {
                            string x = "";
                        }
                        if (jumin.IsNullOrEmpty())
                        {
                            w.JUMIN = w.JUMIN.Replace("-", "");
                            if (w.JUMIN.Length > 6)
                            {
                                w.JUMIN = w.JUMIN.Substring(0, 6) + "-" + w.JUMIN.Substring(6, 1);

                            }
                        }
                        else if (jumin.Length > 6)
                        {
                            w.JUMIN = jumin.Substring(0, 6) + "-" + jumin.Substring(6, 1);

                        }
                    }
                
                    
                    
                }
                SSWorkerList.SetDataSource(list);

                SetDeptCombo();
            }
        }

    

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if(base.SelectedSite == null)
            {
                MessageUtil.Alert("사업장을 선택하세요");
            }
            else
            {
                if (SSWorkerList.Validate())
                {
                    IList<HC_SITE_WORKER> list = SSWorkerList.GetEditbleData<HC_SITE_WORKER>();
                    //bool isValidJumin = false;
                    //foreach(HC_SITE_WORKER dto in list)
                    //{
                    //    if(dto.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    //    {
                    //        //string jumin = dto.JUMIN.Replace("-", "");
                    //        //if (jumin.Length == 13)
                    //        //{
                    //        //    string result = ComFunc.JuminNoCheck(clsDB.DbCon, jumin.Substring(0, 6), jumin.Substring(6, 7));

                    //        //    if (!result.IsNullOrEmpty())
                    //        //    {
                    //        //        MessageUtil.Alert(result);
                    //        //        break;
                    //        //    }
                    //        //}
                    //        //else
                    //        //{
                    //        //    isValidJumin = false;
                    //        //    MessageUtil.Alert("주민번호가 유효하지 않습니다");
                    //        //    break;
                    //        //}
                           
                    //    }
                    //    isValidJumin = true;
                    //}
                    //if (isValidJumin == false)
                    //{
                    //    return;
                    //}
                    if (list.Count > 0)
                    {
                        
                        if (hcSiteWorkerService.Save(base.SelectedSite.ID, list))
                        {
                            Search();
                        }
                        else
                        {
                            MessageUtil.Error("트랜잭션 오류로 저장 할 수 없습니다");
                        }
                    }
                    else
                    {
                        MessageUtil.Info("저장할 데이타가 없습니다");
                    }
                }
            }
        }


        void ISelectSite.Select(ISiteModel siteModel)
        {
            SSWorkerList.SetDataSource(new List<HC_SITE_WORKER>());

            base.SelectedSite = siteModel;
            if (base.SelectedSite.ID > 0)
            {
                lblSiteName.Text = siteModel.NAME;
                Search();
            }
                
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSearch_Click_1(object sender, EventArgs e)
        {
            Search();
        }

        private void CboRole_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

