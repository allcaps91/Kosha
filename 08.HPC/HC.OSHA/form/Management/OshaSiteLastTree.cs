using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase.Controls;
using HC.OSHA.Model;
using HC.OSHA.Service;
using HC_Core.Dto;
using HC.Core.Service;
using HC.Core.Dto;
using ComHpcLibB.Model;
using ComBase.Mvc.Utils;

namespace HC_OSHA
{
    /// <summary>
    /// insert into HC_OSHA_SITE (id, isActive, TaskName, MODIFIED, MODIFIEDUSER, PARENTSITE_ID)
    //select trim(code), 'Y', '사업장등록',  sysdate, '25091', null from hic_ltd
    //where GbDaeHang = 'Y'
    /// </summary>
    public partial class OshaSiteLastTree : UserControl
    {
        [Category("A-MTS-Framework-Event")]
        [Description("체크박스")]
        public bool IsCheckbox { get; set; }

        private List<HC_OSHA_SITE_MODEL> siteList;
        private List<TreeViewItem> treeViewList;
        private HcOshaSiteService service;
        private HcUserService hcUsersService;
        /// <summary>
        /// 선택된 사업장
        /// </summary>
        public HC_OSHA_SITE_MODEL GetSite { get; set; }
        
      //  public delegate void SiteTreeViewNodeMouseDoubleClickEventHandler(object sender, TreeNodeMouseClickEventArgs e);
    
        public delegate void SiteTreeViewNodeMouseClickEventHandler(object sender, TreeNodeMouseClickEventArgs e);
        public delegate void SiteTreeViewNodeCheckboxClickEventHandler(object sender, TreeViewEventArgs e);

        
        //[Category("A-MTS-Framework-Event")]
        //[Description("사이트 노드 더블클릭")]
        //public event SiteTreeViewNodeMouseDoubleClickEventHandler NodeDoubleClick;

        [Category("A-MTS-Framework-Event")]
        [Description("사이트 노드 클릭")]
        public event SiteTreeViewNodeMouseClickEventHandler NodeClick;

        [Category("A-MTS-Framework-Event")]
        [Description("사이트 노드 체크박스 클릭")]
        public event SiteTreeViewNodeCheckboxClickEventHandler NodeCheckClick;

        public OshaSiteLastTree()
        {
            InitializeComponent();
            GetSite = new HC_OSHA_SITE_MODEL();
            service = new HcOshaSiteService();
            treeViewList = new List<TreeViewItem>();
            hcUsersService = new HcUserService();
            siteList = new List<HC_OSHA_SITE_MODEL>();
        }

        private void OshaSiteLastTree_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                SiteTreeView.CheckBoxes = this.IsCheckbox;
                List<HC_USER> OSHAUsers = hcUsersService.GetOsha();
                CboManager.SetItems(OSHAUsers, "Name", "UserId", "전체", "", AddComboBoxPosition.Top);
                CboManager.SetValue(CommonService.Instance.Session.UserId);

                TxtName.SetExecuteButton(BtnSearch);
                SearchSite();
            }
        }
        /// <summary>
        /// 아이디 또는 사업장명으로 검색
        /// </summary>
        /// <param name="idOrName"></param>
        public void Search(string idOrName)
        {
            TxtName.Text = idOrName;
            //BtnSearch.PerformClick();
            SearchSite();

        }
        public void Search()
        {
          //    BtnSearch.PerformClick();
            SearchSite();
        }

        /// <summary>
        /// 선택된 사업장 목록
        /// </summary>
        /// <returns></returns>
        public List<HC_OSHA_SITE_MODEL> GetSiteList()
        {
            siteList.Clear();
            foreach (TreeNode node in SiteTreeView.Nodes)
            {
                if (node.Checked)
                {
                    HC_OSHA_SITE_MODEL site = node.Tag as HC_OSHA_SITE_MODEL;
                    siteList.Add(site);
                    foreach(TreeNode child in node.Nodes)
                    {
                        if (child.Checked)
                        {
                            siteList.Add(child.Tag as HC_OSHA_SITE_MODEL);
                        }
                    }
                }
                else
                {
                    foreach(TreeNode child in node.Nodes)
                    {
                        if (child.Checked)
                        {
                            siteList.Add(child.Tag as HC_OSHA_SITE_MODEL);
                        }
                    }
                }
            }
            return siteList;
        }

        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSearch_Click(object sender, EventArgs e)
        {

            //SearchGroupByHistoryDate()
            SearchSite();
        }
        private void SearchSite()
        {
            try
            {
                SiteTreeView.Nodes.Clear();
                treeViewList.Clear();
                List<HC_OSHA_SITE_MODEL> list = service.Search(TxtName.Text, CboManager.GetValue(), false, false);

                foreach (HC_OSHA_SITE_MODEL model in list)
                {
                    TreeViewItem item = new TreeViewItem();
                    item.ID = model.ID;
                    item.ParentID = model.PARENTSITE_ID;
                    item.Text = model.NAME;
                    item.Model = model;
                    if (model.ISACTIVE == "N")
                    {
                        item.Color = Color.Gray;
                    }
                    treeViewList.Add(item);
                }

                PopulateTreeView(0, null);
                SiteTreeView.ExpandAll();
                if (SiteTreeView.Nodes.Count > 0)
                {
                    SiteTreeView.Nodes[0].EnsureVisible();
                }

            }
            catch(Exception ex)
            {

                MessageUtil.Alert(ex.Message);
            }

            
            ////int count = modifiedGroup.Count();
            //foreach (HC_OSHA_SITE_MODEL model in list)
            //{
            //    TreeNode parentNode = new TreeNode(model.TEL);
            //    if (model.PARENTSITE_ID <= 0)
            //    {
            //        parentNode.Tag = model.ID;
            //        SiteTreeView.Nodes.Add(parentNode);

            //    }

            //}
            //foreach (HC_OSHA_SITE_MODEL model in list)
            //{
            //    if (model.PARENTSITE_ID > 0)
            //    {
            //        TreeNode childNode = SiteTreeView.Nodes.Find(model.PARENTSITE_ID.ToString(), true)[0].Nodes.Add(model.ID.ToString(), model.NAME + " [" + model.ID + "]");
            //        childNode.Tag = model;

            //    }


            //}
            //SiteTreeView.ExpandAll();
            //if (SiteTreeView.Nodes.Count > 0)
            //{
            //    SiteTreeView.Nodes[0].EnsureVisible();
            //}

        }

     
        /// <summary>
        /// 
        /// 사업장 최종 변경일자별
        /// </summary>
        private void SearchGroupByHistoryDate()
        {
            SiteTreeView.Nodes.Clear();

            List<HC_OSHA_SITE_MODEL> list = service.Search(TxtName.Text,"", false, false);

            var modifiedGroup = from model in list
                                group model by model.LASTMODIFIED into dateGroup
                                //        orderby dateGroup.Key
                                select dateGroup;

            //int count = modifiedGroup.Count();
            foreach (var groupModel in modifiedGroup)
            {
                TreeNode parentNode = new TreeNode(groupModel.Key);
                SiteTreeView.Nodes.Add(parentNode);

                foreach (var model in groupModel)
                {
                    TreeNode siteNode = null;
                    if (groupModel.Key == model.LASTMODIFIED && model.PARENTSITE_ID == 0)
                    {
                        siteNode = parentNode.Nodes.Add(model.ID.ToString(), model.NAME + "[" + model.ID + "]");
                        siteNode.Tag = model;
                       
                    }
                    if (model.PARENTSITE_ID > 0)
                    {
                        TreeNode childNode = SiteTreeView.Nodes.Find(model.PARENTSITE_ID.ToString(), true)[0].Nodes.Add(model.ID.ToString(), model.NAME + " [" + model.ID + "]");
                        childNode.Tag = model;
                    }
                }
            }

            //if (SiteTreeView.Nodes.Count > 0)
            //{
            //    SiteTreeView.Nodes[0].Expand();

            //}
            SiteTreeView.ExpandAll();
            if (SiteTreeView.Nodes.Count > 0)
            {
                SiteTreeView.Nodes[0].EnsureVisible();
            }

            // dateTreeList.Nodes.Add(li)
        }
        //private void SiteTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        //{
        //    if (e.Node.Tag != null)
        //    {
        //        e.Node.ExpandAll();
        //        GetSite = e.Node.Tag as HC_OSHA_SITE_MODEL;
        //        NodeDoubleClick?.Invoke(sender, e);
        //    }

        //}

        private void SiteTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
        

            if(e.X < e.Node.Bounds.Left)
            {
                //트리아이콘 클릭
            }
            else if (e.Button == MouseButtons.Right)
            {
                //사업자 정보
                GetSite = e.Node.Tag as HC_OSHA_SITE_MODEL;
                SiteManageForm form = new SiteManageForm();
                form.Show();
            }
            else
            {
                if (e.Node.Tag != null)
                {
                    //if (SiteTreeView.SelectedNode != null)
                    //{
                    //    SiteTreeView.SelectedNode.BackColor = Color.White;
                    //}
                    
                 //   e.Node.BackColor = Color.FromArgb(0, 120, 215);

                    //e.Node.ExpandAll();
                    GetSite = e.Node.Tag as HC_OSHA_SITE_MODEL;
                    NodeClick?.Invoke(sender, e);
                }
            }
            
        }

        public void SiteName(string name)
        {
            TxtName.Text = name;

        }

        private void SiteTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
          //  NodeCheckClick?.Invoke(sender, e);
        }

        public void SelecteNode(long SiteId)
        {
           TreeNode[] nodes = SiteTreeView.Nodes.Find(SiteId.ToString(), true);
            if (nodes.Length > 0)
            {
                SiteTreeView.Focus();
                SiteTreeView.SelectedNode = SiteTreeView.Nodes.Find(SiteId.ToString(), true)[0];
                SiteTreeView_NodeMouseClick(SiteTreeView.SelectedNode, new TreeNodeMouseClickEventArgs(SiteTreeView.SelectedNode,MouseButtons.Left,1,22,0));
            }
        }
        private void PopulateTreeView(long parentId, TreeNode parentNode)
        {
            var filteredItems = treeViewList.Where(item =>
                                        item.ParentID == parentId).OrderBy(item=> item.Text);

            TreeNode childNode;
            foreach (var i in filteredItems.ToList())
            {
                if (parentNode == null)
                {
                    
                    childNode = SiteTreeView.Nodes.Add(i.ID.ToString(), i.Text + " [" + i.ID + "]");
                    childNode.Tag = i.Model;
                }
                else
                {
                    childNode = parentNode.Nodes.Add(i.ID.ToString(), i.Text + " [" + i.ID + "]");
                    childNode.Tag = i.Model;
                }
                if (i.Color.NotEmpty())
                {
                   // childNode.BackColor = i.Color;
                }
                
                PopulateTreeView(i.ID, childNode);
            }
        }

        private void TxtName_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void CboManager_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchSite();
        }

        private void SiteTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (SiteTreeView.SelectedNode != null)
            {
                SiteTreeView.SelectedNode.BackColor = Color.White;
            }
        }

        private void SiteTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
          //  NodeCheckClick?.Invoke(sender, e);

            if (e.Node.Nodes.Count > 0)
            {
                bool isCheck = e.Node.Checked;
               
                foreach (TreeNode child in e.Node.Nodes)
                {
                    child.Checked = isCheck;
                }
            }
        }

        private void TxtName_KeyUp(object sender, KeyEventArgs e)
        {
            if (TxtName.Text.Length == 0)
            {
                if (!TxtName.Text.NotEmpty())
                {
                    Search();
                }

            }
        }
    }
}
