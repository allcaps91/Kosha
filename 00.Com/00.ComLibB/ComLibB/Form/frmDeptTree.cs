using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary> 부서코드 TREE </summary>
    public partial class frmDeptTree : Form
    {
        string GstrHelpCode = string.Empty; //TODO: Global

        /// <summary> 부서코드 TREE </summary>
        public frmDeptTree()
        {
            InitializeComponent();
        }

        void frmDeptTree_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }


            int i = 0;
            string strRelative = string.Empty;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            try
            {
                //Tree.ImageList = ImageList
                TreeNode root = new TreeNode();
                TreeNode subnode = new TreeNode();

                root = trvView.Nodes.Add("NODE10", "포항성모병원", "root");

                SQL = " SELECT DEPT_LEVEL, BUCODE, DEPT_ID, NAME, DEPT_ID_UP, DELDATE, GBABC, BUSE1 ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_BUSE  ";
                SQL = SQL + ComNum.VBLF + " ORDER BY DEPT_LEVEL, DEPT_SORT ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {

                    if (dt.Rows[i]["DEPT_ID_UP"].ToString().Trim() == "")
                    {
                        strRelative = "NODE10";
                    }
                    else
                    {
                        strRelative = "NODE" + dt.Rows[i]["DEPT_ID_UP"].ToString().Trim();
                    }

                    if (dt.Rows[i]["GBABC"].ToString().Trim() != "Y")
                    {
                        if (dt.Rows[i]["BUSE1"].ToString().Trim() == "*")
                        {
                            root.Nodes.Add("NODE" + dt.Rows[i]["DEPT_ID"].ToString().Trim(), "tx" + dt.Rows[i]["Name"].ToString().Trim(), "close");
                        }
                        else
                        {
                            root.Nodes.Add("NODE" + dt.Rows[i]["DEPT_ID"].ToString().Trim(), "x" + dt.Rows[i]["Name"].ToString().Trim(), "close");
                        }
                    }
                    else
                    {
                        root.Nodes.Add("NODE" + dt.Rows[i]["DEPT_ID"].ToString().Trim(), dt.Rows[i]["Name"].ToString().Trim(), "close");
                    }

                        root.Nodes["NODE" + dt.Rows[i]["DEPT_ID"].ToString().Trim()].Tag = dt.Rows[i]["BuCode"].ToString().Trim();

                    if (dt.Rows[i]["DelDate"].ToString().Trim() != "")
                    {
                        root.Nodes["NODE" + dt.Rows[i]["DEPT_ID"].ToString().Trim()].ForeColor = Color.FromArgb(192, 192, 192);
                    }
                }

                //trvView.Nodes[8].EnsureVisible();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            //GstrHelpCode = Tree.SelectedItem.Tag & "\" & Tree.SelectedItem.Text
            this.Close();
        }

        void trvView_Click(object sender, EventArgs e)
        {
            //GstrHelpCode = Node.Tag
        }
    }
}
