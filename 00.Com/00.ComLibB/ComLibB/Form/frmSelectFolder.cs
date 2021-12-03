using System;
using System.IO;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmSelectFolder
    /// File Name : frmSelectFolder.cs
    /// Title or Description : 사진이 저장된 폴더 찾기 페이지
    /// Author : 박성완
    /// Create Date : 2017-06-01
    /// <history> 
    /// </history>
    /// </summary>
    public partial class frmSelectFolder : Form
    {
        public frmSelectFolder()
        {
            InitializeComponent();
        }

        private void frmSelectFolder_Load(object sender, EventArgs e)
        {

            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            TreeNode root = tvFolder.Nodes.Add("C:\\NVC");

            string[] directories = Directory.GetDirectories("C:\\NVC");
            foreach (string directory in directories)
            {
                TreeNode node = root.Nodes.Add(directory.Substring(directory.LastIndexOf("\\") + 1));
                node.Nodes.Add("@%");
            }
        }

        private void tvFolder_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            TreeNode current = e.Node;
 
            if(current.Nodes.Count == 1 && current.Nodes[0].Text.Equals("@%"))
            {
                current.Nodes.Clear();

                string path = current.FullPath;

                try //하위 장치(목록)이 없을 경우 예외 처리
                {
                    string[] directories = Directory.GetDirectories(path);
                    foreach(string directory in directories)
                    {
                        TreeNode newNode = current.Nodes.Add(directory.Substring(directory.LastIndexOf("\\") + 1));
                        newNode.Nodes.Add("@%");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            tvFolder.Refresh();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //TODO:GstrHelpCode가 경로를 가져오는지?
            clsPublic.GstrHelpCode = tvFolder.SelectedNode.FullPath;
            this.Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            clsPublic.GstrHelpCode = "";
            this.Hide();
        }
    }
}
