using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmDepartmentrescue : Form
    {
        string mstrMenuCode = ""; //메뉴코드
        string mstrMenuName = ""; //메뉴이름
        string mstrRootCode = "ROOT";
        string mstrRootMenu = "**.포항성모병원";

        Image imgClose = null;
        Image imgOpen = null;
        Image imgPage = null;

        public frmDepartmentrescue ()
        {
            InitializeComponent ();
        }

        private void frmDepartmentrescue_Load (object sender , EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            imgClose = ComFunc.FileToImage (@"C:\HealthSoft\icon\close.png");
            imgOpen = ComFunc.FileToImage (@"C:\HealthSoft\icon\open.png");
            imgPage = ComFunc.FileToImage (@"C:\HealthSoft\icon\page.png");

            SetMenu ();
        }

        private void SetMenu ()
        {
            int i = 0;
            int j = 0;
            int k = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            string strRootCode = "";

            treeMenu.Nodes.Clear ();
            DevComponents.AdvTree.Node node1;
            node1 = new DevComponents.AdvTree.Node ();

            node1.Expanded = true;
            node1.Name = "ROOT";
            node1.Text = "**.포항성모병원";
            node1.Image = imgClose;
            node1.ImageExpanded = imgOpen;
            treeMenu.Nodes.Add (node1);

            Cursor.Current = Cursors.WaitCursor;

            node1 = null;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = " Select TREE, NAME FROM " + ComNum.VBLF + "BAS_BUSE ";
                SQL = SQL + ComNum.VBLF + " Where SUBSTR(TREE, 3, 6)='000000' ";
                SQL = SQL + ComNum.VBLF + " ORDER By TREE ";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox ("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.WaitCursor;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose ();
                    dt = null;
                    ComFunc.MsgBox ("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.WaitCursor;
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    DevComponents.AdvTree.Node pNode = new DevComponents.AdvTree.Node ();
                    pNode = treeMenu.FindNodeByName (mstrRootCode);

                    DevComponents.AdvTree.Node cNode = new DevComponents.AdvTree.Node ();

                    cNode.Name = dt.Rows [i] ["TREE"].ToString ().Trim ();
                    cNode.Text = dt.Rows [i] ["NAME"].ToString ().Trim ();
                    cNode.Image = imgClose;
                    cNode.ImageExpanded = imgOpen;
                    pNode.Nodes.Add (cNode);
                }
                dt.Dispose ();
                dt = null;


                SQL = "Select TREE, NAME FROM " + ComNum.DB_PMPA + "BAS_BUSE ";
                SQL = SQL + ComNum.VBLF + " Where SUBSTR(TREE, 5, 4)='0000' ";
                SQL = SQL + ComNum.VBLF + "   AND SUBSTR(TREE, 3, 6) <> '000000' ";
                SQL = SQL + ComNum.VBLF + " ORDER By TREE ";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox ("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.WaitCursor;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose ();
                    dt = null;
                    ComFunc.MsgBox ("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.WaitCursor;
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strRootCode = VB.Left (dt.Rows [i] ["TREE"].ToString ().Trim () , 2) + "000000";
                    DevComponents.AdvTree.Node pNode = new DevComponents.AdvTree.Node ();
                    pNode = treeMenu.FindNodeByName (strRootCode);

                    DevComponents.AdvTree.Node cNode = new DevComponents.AdvTree.Node ();

                    cNode.Name = dt.Rows [i] ["TREE"].ToString ().Trim ();
                    cNode.Text = dt.Rows [i] ["NAME"].ToString ().Trim ();

                    cNode.Image = imgClose;
                    cNode.ImageExpanded = imgOpen;
                    pNode.Nodes.Add (cNode);
                }
                dt.Dispose ();
                dt = null;


                SQL = "Select TREE, NAME FROM " + ComNum.DB_PMPA + "BAS_BUSE ";
                SQL = SQL + ComNum.VBLF + " Where SUBSTR(TREE, 7, 2)='00' ";
                SQL = SQL + ComNum.VBLF + "   AND SUBSTR(TREE, 5, 4)='0000' ";
                SQL = SQL + ComNum.VBLF + "   AND SUBSTR(TREE, 3, 6) <> '000000' ";
                SQL = SQL + ComNum.VBLF + " ORDER By TREE ";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox ("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.WaitCursor;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose ();
                    dt = null;
                    ComFunc.MsgBox ("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.WaitCursor;
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strRootCode = VB.Left (dt.Rows [i] ["TREE"].ToString ().Trim () , 4) + "0000";
                    DevComponents.AdvTree.Node pNode = new DevComponents.AdvTree.Node ();
                    pNode = treeMenu.FindNodeByName (strRootCode);

                    DevComponents.AdvTree.Node cNode = new DevComponents.AdvTree.Node ();

                    cNode.Name = dt.Rows [i] ["TREE"].ToString ().Trim ();
                    cNode.Text = dt.Rows [i] ["NAME"].ToString ().Trim ();

                    cNode.Image = imgClose;
                    cNode.ImageExpanded = imgOpen;
                    pNode.Nodes.Add (cNode);
                }
                dt.Dispose ();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
