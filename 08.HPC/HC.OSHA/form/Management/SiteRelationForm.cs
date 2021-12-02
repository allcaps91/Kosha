using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Utils;
using HC.Core.Dto;
using HC.Core.Model;
using HC.Core.Repository;
using HC.OSHA.Dto;
using HC.OSHA.Model;
using HC.OSHA.Repository.StatusReport;
using HC_Core;
using HC_Core.Model.DataSync;
using HC_Core.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace HC_OSHA
{
    public partial class SiteRelationForm : CommonForm
    {
        HcOshaRelationRepository hcOshaRelationRepository;
        public SiteRelationForm()
        {
            InitializeComponent();
            hcOshaRelationRepository = new HcOshaRelationRepository();
        }

        private void SiteRelationForm_Load(object sender, EventArgs e)
        {
            SSSiteList.Initialize(new SpreadOption() { IsRowSelectColor = false, ColumnHeaderHeight = 40 });
            SSSiteList.AddColumnText("코드", nameof(HC_OSHA_RELATION_MODEL.CHILD_ID), 60, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
            SSSiteList.AddColumnText("사업장명", nameof(HC_OSHA_RELATION_MODEL.CHILD_NAME), 200, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
            SSSiteList.AddColumnButton("삭제", 60, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += SiteRelationForm_ButtonClick;
            SSSiteList.SetDataSource(new List<HC_OSHA_RELATION_MODEL>());

            SSRelationList .Initialize(new SpreadOption() { IsRowSelectColor = false, ColumnHeaderHeight = 40 });
            SSRelationList.AddColumnText("코드", nameof(HC_OSHA_RELATION_MODEL.PARENT_ID), 60, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left, mergePolicy = FarPoint.Win.Spread.Model.MergePolicy.Always });
            SSRelationList.AddColumnText("원청사업장", nameof(HC_OSHA_RELATION_MODEL.PARENT_NAME), 300, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left, mergePolicy = FarPoint.Win.Spread.Model.MergePolicy.Always });
            SSRelationList.AddColumnText("코드", nameof(HC_OSHA_RELATION_MODEL.CHILD_ID), 60, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
            SSRelationList.AddColumnText("하청사업장", nameof(HC_OSHA_RELATION_MODEL.CHILD_NAME), 300, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
            SSRelationList.SetDataSource(new List<HC_OSHA_RELATION_MODEL>());


            Search();

            this.WindowState = FormWindowState.Maximized;
        }

        private void SiteRelationForm_ButtonClick(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            SSSiteList.DeleteRow();
        }

        private void BtnAddParent_Click(object sender, EventArgs e)
        {
            List<HC_OSHA_SITE_MODEL> list = oshaSiteLastTree1.GetSiteList();
            if (list.Count == 0)
            {
                MessageUtil.Alert("사업장을 선택하세요");
            }
 
            else
            {
                List<HC_OSHA_RELATION_MODEL> parentlist = hcOshaRelationRepository.FindAll(list[0].ID);
                if (parentlist.Count>0)
                {
                    MessageUtil.Alert("이미 원청으로 등록되어 있습니다");
                    //TxtParentId.Text = list[0].ID.ToString();
                    //TxtParentName.Text = list[0].NAME;

                    //List<HC_OSHA_RELATION_MODEL> relationList = hcOshaRelationRepository.FindAll(list[0].ID);
                    //SSSiteList.SetDataSource(relationList);


                }
                else
                {
                    TxtParentId.Text = list[0].ID.ToString();
                    TxtParentName.Text = list[0].NAME;
                    List<HC_OSHA_RELATION_MODEL> relationList = hcOshaRelationRepository.FindAll(list[0].ID);
                    SSSiteList.SetDataSource(relationList);

                }



            }
           
         
        }

        private void BtnAddChild_Click(object sender, EventArgs e)
        {
            // List<HC_OSHA_SITE_MODEL> siteList = SSSiteList.GetEditbleData<HC_OSHA_SITE_MODEL>();
            if (TxtParentId.Text.Length <= 0)
            {
                MessageUtil.Alert("원청을 먼저 선택하세요");
                return;
            }

            long parentId = TxtParentId.Text.To<long>(0);
            List<HC_OSHA_SITE_MODEL> list = oshaSiteLastTree1.GetSiteList();
            //foreach (HC_OSHA_SITE_MODEL sm in list)
            //{
            //    if(sm.ID == parentId)
            //    {
            //        MessageUtil.Alert("선택한 사업장은 원청으로 선택하셨습니다.");
            //        break; 
            //    }
            //}

            List<HC_OSHA_RELATION_MODEL> childList = new List<HC_OSHA_RELATION_MODEL>();
            for (int i=0; i< SSSiteList.ActiveSheet.RowCount; i++)
            {
                HC_OSHA_RELATION_MODEL model = SSSiteList.GetRowData(i) as HC_OSHA_RELATION_MODEL;
                childList.Add(model);
            }

        
            foreach (HC_OSHA_SITE_MODEL sm in list)
            {
                if (sm.ID != parentId)
                {
                    HC_OSHA_RELATION_MODEL model = new HC_OSHA_RELATION_MODEL()
                    {
                        ID = 0,
                        CHILD_ID = sm.ID,
                        PARENT_ID = parentId,
                        CHILD_NAME = sm.NAME
                    };

                    bool isExistChild = false; 

                    foreach(HC_OSHA_RELATION_MODEL child in childList)
                    {
                        if(child.CHILD_ID == sm.ID)
                        {
                            isExistChild = true;
                        }
                    }
                    if(isExistChild == false)
                    {
                        model.RowStatus = ComBase.Mvc.RowStatus.Insert;

                        childList.Add(model);
                    }
                    
                }
            }

            SSSiteList.SetDataSource(new List<HC_OSHA_RELATION_MODEL>());

            SSSiteList.SetDataSource(childList);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            List<HC_OSHA_RELATION_MODEL> list = SSSiteList.GetEditbleData<HC_OSHA_RELATION_MODEL>();

            foreach(HC_OSHA_RELATION_MODEL model in list)
            {
                if(model.RowStatus == ComBase.Mvc.RowStatus.Insert)
                {
                    HC_OSHA_RELATION dto = new HC_OSHA_RELATION();
                    dto.PARENT_ID = model.PARENT_ID;
                    dto.CHILD_ID = model.CHILD_ID;
                    hcOshaRelationRepository.Insert(dto);
                }
                else
                {
                    hcOshaRelationRepository.Delete(model.ID);
                }
            }

            //long parentId = TxtParentId.Text.To<long>(0);
            //List<HC_OSHA_RELATION_MODEL> relationList = hcOshaRelationRepository.FindAll(parentId);
            //SSSiteList.SetDataSource(relationList);

            TxtParentId.Text = "";
            TxtParentName.Text = "";
            SSSiteList.SetDataSource(new List<HC_OSHA_RELATION_MODEL>()  );

            Search();
            oshaSiteLastTree1.Search();

        }

        private void SSSiteList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

        }


        private void Search()
        {
            SSRelationList.SetDataSource(hcOshaRelationRepository.FindAll());
        }

        private void SSRelationList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

        }

        private void SSRelationList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

        }

        private void oshaSiteLastTree1_NodeClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            HC_OSHA_SITE_MODEL model = oshaSiteLastTree1.GetSite;
            List<HC_OSHA_RELATION_MODEL> parentlist = hcOshaRelationRepository.FindAll(model.ID);
            if (parentlist.Count > 0)
            {
                TxtParentId.Text = parentlist[0].PARENT_ID.ToString();
                TxtParentName.Text = parentlist[0].PARENT_NAME.ToString();

                List<HC_OSHA_RELATION_MODEL> childList = new List<HC_OSHA_RELATION_MODEL>();

                foreach (HC_OSHA_RELATION_MODEL c in parentlist)
                {
                    HC_OSHA_RELATION_MODEL m = new HC_OSHA_RELATION_MODEL()
                    {
                        ID = c.ID,
                        CHILD_ID = c.CHILD_ID,
                        PARENT_ID = c.PARENT_ID,
                        CHILD_NAME = c.CHILD_NAME
                    };

                    childList.Add(m);
                }

                SSSiteList.SetDataSource(childList);
            }
            else{
                TxtParentId.Text = "";
                TxtParentName.Text = "";
                SSSiteList.ActiveSheet.RowCount = 0;
            }
        }

        private void TxtParentName_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtParentId_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
