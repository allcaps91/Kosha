using ComBase; //기본 클래스
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary> 상병코드 TREE </summary>
    public partial class frmTreeIll : Form
    {
        //string GstrHelpCode = string.Empty; //Global
        //Boolean indrag = true; //끌어서 놓기 동작을 신호하는 플래그입니다.
        //Object nodX = null; //끌어가는 항목입니다.
        Font boldFont = null;

        /// <summary> 상병코드 TREE </summary>
        public frmTreeIll()
        {
            InitializeComponent();
        }

        private void frmTreeIll_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            boldFont = new Font("맑은 고딕", 10, FontStyle.Bold);
            TreeMake("K");
        }

        void TreeMake(string ArgGb)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            string relative = string.Empty;
            string strKey = string.Empty;
            string strTxt = string.Empty;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            trvView.Nodes.Clear();
            //Tree.ImageList = ImageList

            TreeNode treeNode =  trvView.Nodes.Add("NODETOP", "포항성모병원", "root");
            Dictionary<string, TreeNode> _treeNodes = new Dictionary<string, TreeNode>();
            _treeNodes.Add("NODETOP", treeNode);

            try
            {

                SQL = "";
                SQL = "SELECT ILLCLASS, to_single_byte(ILLCODE) AS ILLCODE, ILLNAMEK, ILLNAMEE, ILLUPCODE, NOUSE, GBINFECT , to_single_byte(ILLCODED) AS ILLCODED ";
                SQL = SQL + ComNum.VBLF + "  From " + ComNum.DB_PMPA + "BAS_ILLS_KCD6 ";
                SQL = SQL + ComNum.VBLF + " WHERE ILLCLASS IN ('A' ,'B','C','1') ";
                SQL = SQL + ComNum.VBLF + "   AND NOUSE IS NULL ";
                SQL = SQL + ComNum.VBLF + "   AND DDATE IS NULL";
                SQL = SQL + ComNum.VBLF + " ORDER BY DECODE(ILLCLASS,'A', 1, 'B',2,'C',3,'1',4,7), ILLCODE";

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
                    switch (dt.Rows[i]["illclass"].ToString().Trim())
                    {
                        case "A":
                            relative = "NODETOP";
                            break;
                        default:
                            relative = "NODE";
                            if (dt.Rows[i]["illclass"].ToString().Trim() == "B")
                            {
                                relative = relative + "A" + dt.Rows[i]["ILLUPCODE"].ToString().Trim();
                            }
                            else if (dt.Rows[i]["illclass"].ToString().Trim() == "C")
                            {
                                relative = relative + "B" + dt.Rows[i]["ILLUPCODE"].ToString().Trim();
                            }
                            else if (dt.Rows[i]["illclass"].ToString().Trim() == "1")
                            {
                                relative = relative + "C" + dt.Rows[i]["ILLUPCODE"].ToString().Trim();
                            }
                            break;
                    }

                    strKey = "NODE" + dt.Rows[i]["illclass"].ToString().Trim() + dt.Rows[i]["illcode"].ToString().Trim();

                    if (ArgGb == "E")
                    {
                        if (dt.Rows[i]["illclass"].ToString().Trim() == "1")
                        {
                            strTxt = dt.Rows[i]["illcodeD"].ToString().Trim() + " " + dt.Rows[i]["IllnameE"].ToString().Trim();
                        }
                        else
                        {
                            strTxt = dt.Rows[i]["IllnameE"].ToString().Trim();
                        }
                    }
                    else
                    {
                        if (dt.Rows[i]["illclass"].ToString().Trim() == "1")
                        {
                            strTxt = dt.Rows[i]["illcodeD"].ToString().Trim() + " " + dt.Rows[i]["ILLNAMEK"].ToString().Trim();
                        }
                        else
                        {
                            strTxt = dt.Rows[i]["ILLNAMEK"].ToString().Trim();
                        }
                    }

                    if (dt.Rows[i]["GBINFECT"].ToString().Trim() != "")
                    {
                        strTxt = strTxt + "[" + dt.Rows[i]["GBINFECT"].ToString().Trim() + "]";
                    }

                    if (_treeNodes.ContainsKey(relative) == false)
                    {
                        _treeNodes.Add(relative, treeNode.Nodes.Add(strKey, strTxt, "close"));
                    }
                    else
                    {
                        
                        if (_treeNodes.TryGetValue(relative, out treeNode))
                        {
                            treeNode = treeNode.Nodes.Add(strKey, strTxt, "close");
                            _treeNodes.Add(strKey, treeNode);
                            treeNode.Tag = dt.Rows[i]["illcode"].ToString().Trim();

                            switch (dt.Rows[i]["illclass"].ToString().Trim())
                            {
                                case "A":
                                case "B":
                                case "C":
                                    break;
                                default:
                                    treeNode.NodeFont = boldFont;
                                    treeNode.BackColor = Color.FromArgb(255, 255, 0);
                                    break;
                            }


                            if (dt.Rows[i]["Nouse"].ToString().Trim() == "Y")
                            {
                                treeNode.ForeColor = Color.FromArgb(100, 100, 100);
                                treeNode.BackColor = Color.FromArgb(255, 255, 0);
                            }
                        }
                    }
                }

                if (i >= 0)
                {
                    //trvView.Nodes[7].EnsureVisible();
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        //TODO: "\"에서 뒷 부분이 주황색 글씨가 됨.
        void btnSelect_Click(object sender, EventArgs e)
        {
            clsPublic.GstrHelpCode = trvView.SelectedNode.Tag.ToString() + @"\" + trvView.SelectedNode.Text;
            this.Close();
        }

        void btnEng_Click(object sender, EventArgs e)
        {
            TreeMake("E");
        }

        void btnKor_Click(object sender, EventArgs e)
        {
            TreeMake("K");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void trvView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.BackColor != Color.FromArgb(255, 255, 0))
            {
                lblName.Text = "";
            }
            else
            {
                lblName.Text = e.Node.Text;
                clsPublic.GstrHelpCode = e.Node.Tag.ToString();
            }
        }
    }
}
