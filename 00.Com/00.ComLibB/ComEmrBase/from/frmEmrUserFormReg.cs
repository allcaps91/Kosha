using ComBase;
using ComBase.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrUserFormReg : Form
    {
        int nImage;
        int nSelectedImage;
        //int nImageSaved;
        //int nSelectedImageSaved;
        private string sKeyHead = "^";

        private string mstrGRPFORMNO = "";
        private string mstrDeptCd = "";

        private string mstrGRPGB = "";
        private string mstrUSEGB = "";

        string mstrSubSql = "";
        string mstrSubSql1 = "";

        public frmEmrUserFormReg()
        {
            InitializeComponent();
        }

        public frmEmrUserFormReg(string strGRPGB, string strUSEGB)
        {
            InitializeComponent();
            mstrGRPGB = strGRPGB;
            mstrUSEGB = strUSEGB;
        }

        private void frmEmrUserFormReg_Load(object sender, EventArgs e)
        {
            nImage = 0;
            nSelectedImage = 1;

            //nImageSaved = 2;
            //nSelectedImageSaved = 3;

            trvEmrGroup.ImageList = this.ImageList1;
            trvUserForm.ImageList = this.ImageList1;

            if (mstrGRPGB == "D")
            {
                panDept.Visible = true;
                this.Text = "과별 상용기록지 등록";
                lblTitle.Text = "과별 상용기록지 등록";
                lblGRPGB.Text = "과별 상용기록지";
            }
            else if (mstrGRPGB == "U")
            {
                panDept.Visible = false;
                this.Text = "개인별 상용기록지 등록";
                lblTitle.Text = "개인별 상용기록지 등록";
                lblGRPGB.Text = "개인별 상용기록지";
            }

            SetSql();

            GetDataGrpFormList();

            if (mstrGRPGB == "D")
            {
                SetDeptCd();
                mstrDeptCd = clsType.User.IdNumber;
            }
            else
            {
                GetUserForm();
            }
        }

        private void SetSql()
        {
            if (mstrGRPGB == "D")
            {
                mstrSubSql = "";
                mstrSubSql = mstrSubSql + ComNum.VBLF + "                                                        AND GRPGB = 'D' ";
                mstrSubSql = mstrSubSql + ComNum.VBLF + "                                                        AND USEGB = '" + mstrDeptCd + "' ";
                mstrSubSql1 = "";
                mstrSubSql1 = mstrSubSql1 + ComNum.VBLF + "   'D', ";
                mstrSubSql1 = mstrSubSql1 + ComNum.VBLF + "   '" + mstrDeptCd + "',";
            }
            else if (mstrGRPGB == "U")
            {
                mstrSubSql = "";
                mstrSubSql = mstrSubSql + ComNum.VBLF + "                                                        AND GRPGB = 'U' ";
                mstrSubSql = mstrSubSql + ComNum.VBLF + "                                                        AND USEGB = '" + clsType.User.IdNumber + "' ";
                mstrSubSql1 = "";
                mstrSubSql1 = mstrSubSql1 + ComNum.VBLF + "   'U', ";
                mstrSubSql1 = mstrSubSql1 + ComNum.VBLF + "   '" + clsType.User.IdNumber + "',";
            }
        }

        private void SetDeptCd()
        {
            int i = 0;
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            cboDept.Items.Clear();
            cboDept.Items.Add("");

            dt = clsEmrQuery.GetMedDeptInfo(clsDB.DbCon, "");
            
            if (dt == null)
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                cboDept.Items.Add(dt.Rows[i]["MEDDEPTCD"].ToString().Trim());
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        private void GetDataGrpFormList()
        {
            TreeNode oNodex = new TreeNode();

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            trvEmrGroup.Nodes.Clear();
            ssForm_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            //그룹 1 조회
            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "     G1.GRPFORMNO, G1.GRPFORMNAME, G1.GROUPPARENT, G1.DEPTH, G1.DISPSEQ ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRGRPFORM G1";
            SQL = SQL + ComNum.VBLF + "WHERE  G1.DEPTH = '0'";
            SQL = SQL + ComNum.VBLF + "     AND  G1.DELYN = '0'";
            SQL = SQL + ComNum.VBLF + "ORDER BY G1.DISPSEQ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                oNodex = trvEmrGroup.Nodes.Add(dt.Rows[i]["GRPFORMNO"].ToString().Trim(), dt.Rows[i]["GRPFORMNAME"].ToString().Trim(), nImage, nSelectedImage);
            }
            dt.Dispose();
            dt = null;

            //그룹 2 조회
            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "     GRPFORMNO, GRPFORMNAME, GROUPPARENT, DEPTH, DISPSEQ ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRGRPFORM ";
            SQL = SQL + ComNum.VBLF + "WHERE  DEPTH = '1'";
            SQL = SQL + ComNum.VBLF + "     AND  DELYN = '0'";
            SQL = SQL + ComNum.VBLF + "ORDER BY DISPSEQ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                oNodex = trvEmrGroup.Nodes.Find(dt.Rows[i]["GROUPPARENT"].ToString().Trim(), true)[0].Nodes.Add(dt.Rows[i]["GRPFORMNO"].ToString().Trim(), dt.Rows[i]["GRPFORMNAME"].ToString().Trim(), nImage, nSelectedImage);
            }
            dt.Dispose();
            dt = null;

            Cursor.Current = Cursors.Default;

        }

        private void GetUserForm()
        {
            TreeNode oNodex = new TreeNode();

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            trvUserForm.Nodes.Clear();

            Cursor.Current = Cursors.WaitCursor;

            SetSql();

            //그룹 1 조회
            SQL = " SELECT G1.GRPFORMNO, G1.GRPFORMNAME, G1.GROUPPARENT, G1.DEPTH, G1.DISPSEQ  ";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRGRPFORM G1 ";
            SQL = SQL + ComNum.VBLF + "    WHERE G1.GRPFORMNO IN (SELECT G2.GROUPPARENT ";
            SQL = SQL + ComNum.VBLF + "                            FROM " + ComNum.DB_EMR + "AEMRGRPFORM G2 ";
            SQL = SQL + ComNum.VBLF + "                            WHERE GRPFORMNO IN ( ";
            SQL = SQL + ComNum.VBLF + "                                                 SELECT A.GRPFORMNO ";
            SQL = SQL + ComNum.VBLF + "                                                    FROM " + ComNum.DB_EMR + "AEMRFORM A ";
            SQL = SQL + ComNum.VBLF + "                                                    INNER JOIN " + ComNum.DB_EMR + "AEMRUSERFORM U ";
            SQL = SQL + ComNum.VBLF + "                                                        ON U.FORMNO = A.FORMNO ";
            SQL = SQL + ComNum.VBLF + "                                                        AND U.GRPTYPE = 'FD' ";
            SQL = SQL + mstrSubSql;
            SQL = SQL + ComNum.VBLF + "                                                WHERE A.UPDATENO = (SELECT MAX(B.UPDATENO) FROM " + ComNum.DB_EMR + "AEMRFORM B ";
            SQL = SQL + ComNum.VBLF + "                                                                WHERE B.FORMNO = A.FORMNO AND B.USECHECK = '1') ";
            SQL = SQL + ComNum.VBLF + "                                                        AND A.USECHECK = '1' ";
            SQL = SQL + ComNum.VBLF + "                                                ) ";
            SQL = SQL + ComNum.VBLF + "                                AND G2.DEPTH = '1' ";
            SQL = SQL + ComNum.VBLF + "                            )  ";
            SQL = SQL + ComNum.VBLF + "    AND  G1.DEPTH = '0' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY G1.DISPSEQ ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                oNodex = trvUserForm.Nodes.Add(dt.Rows[i]["GRPFORMNO"].ToString().Trim(), dt.Rows[i]["GRPFORMNAME"].ToString().Trim(), nImage, nSelectedImage);
            }
            dt.Dispose();
            dt = null;

            //그룹 2 조회
            
            SQL = " SELECT G2.GRPFORMNO, G2.GRPFORMNAME, G2.GROUPPARENT, G2.DEPTH, G2.DISPSEQ ";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRGRPFORM G2 ";
            SQL = SQL + ComNum.VBLF + "    WHERE GRPFORMNO IN ( ";
            SQL = SQL + ComNum.VBLF + "                         SELECT A.GRPFORMNO ";
            SQL = SQL + ComNum.VBLF + "                            FROM " + ComNum.DB_EMR + "AEMRFORM A ";
            SQL = SQL + ComNum.VBLF + "                            INNER JOIN " + ComNum.DB_EMR + "AEMRUSERFORM U ";
            SQL = SQL + ComNum.VBLF + "                                ON U.FORMNO = A.FORMNO ";
            SQL = SQL + ComNum.VBLF + "                                AND U.GRPTYPE = 'FD' ";
            SQL = SQL + mstrSubSql;
            SQL = SQL + ComNum.VBLF + "                        WHERE A.UPDATENO = (SELECT MAX(B.UPDATENO) FROM " + ComNum.DB_EMR + "AEMRFORM B ";
            SQL = SQL + ComNum.VBLF + "                                                 WHERE B.FORMNO = A.FORMNO AND B.USECHECK = '1') ";
            SQL = SQL + ComNum.VBLF + "                              AND A.USECHECK = '1' ";
            SQL = SQL + ComNum.VBLF + "                        ) ";
            SQL = SQL + ComNum.VBLF + "    AND G2.DEPTH = '1' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY G2.DISPSEQ ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                oNodex = trvUserForm.Nodes.Find(dt.Rows[i]["GROUPPARENT"].ToString().Trim(), true)[0].Nodes.Add(dt.Rows[i]["GRPFORMNO"].ToString().Trim(), dt.Rows[i]["GRPFORMNAME"].ToString().Trim(), nImage, nSelectedImage);
            }
            dt.Dispose();
            dt = null;

            // 기록지 조회
            SQL = " SELECT  ";
            SQL = SQL + ComNum.VBLF + "A.GRPFORMNO, A.FORMNO, A.UPDATENO, A.FORMNAME, A.FORMNAMEPRINT, A.USECHECK  ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRFORM A ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRUSERFORM U ";
            SQL = SQL + ComNum.VBLF + "    ON U.FORMNO = A.FORMNO ";
            SQL = SQL + ComNum.VBLF + "    AND U.GRPTYPE = 'FD' ";
            SQL = SQL + mstrSubSql;
            SQL = SQL + ComNum.VBLF + "WHERE A.UPDATENO = (SELECT MAX(B.UPDATENO) FROM " + ComNum.DB_EMR + "AEMRFORM B ";
            SQL = SQL + ComNum.VBLF + "                                        WHERE B.FORMNO = A.FORMNO AND B.USECHECK = '1') ";
            SQL = SQL + ComNum.VBLF + "    AND A.USECHECK = '1' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY A.DISPSEQ ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (trvUserForm.Nodes.Find(dt.Rows[i]["GRPFORMNO"].ToString().Trim(), true).Length > 0 )
                {
                    trvUserForm.Nodes.Find(dt.Rows[i]["GRPFORMNO"].ToString().Trim(), true)[0].Nodes.Add(dt.Rows[i]["FORMNO"].ToString().Trim() + sKeyHead + dt.Rows[i]["UPDATENO"].ToString().Trim(), dt.Rows[i]["FORMNAME"].ToString().Trim(), nImage, nSelectedImage);
                }
            }
            dt.Dispose();
            dt = null;

            Cursor.Current = Cursors.Default;

            trvUserForm.ExpandAll();
        }

        private void trvEmrGroup_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode Node;
            string strGRPFORMNO = "";

            Node = e.Node;
            if (Node.GetNodeCount(false) > 0)
            {
                return;
            }

            strGRPFORMNO = Node.Name.ToString();

            mstrGRPFORMNO = strGRPFORMNO;

            GetFormList(strGRPFORMNO);
        }

        private void GetFormList(string strGRPFORMNO)
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssForm_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            SQL = " SELECT A.FORMNO, A.UPDATENO, A.FORMNAME, A.FORMNAMEPRINT, A.USECHECK ";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRFORM A";
            SQL = SQL + ComNum.VBLF + "    WHERE A.GRPFORMNO = " + strGRPFORMNO;
            SQL = SQL + ComNum.VBLF + "        AND A.UPDATENO = (SELECT MAX(B.UPDATENO) FROM " + ComNum.DB_EMR + "AEMRFORM B";
            SQL = SQL + ComNum.VBLF + "                                        WHERE B.FORMNO = A.FORMNO AND B.USECHECK = '1')";
            SQL = SQL + ComNum.VBLF + "        AND A.USECHECK = '1'";
            SQL = SQL + ComNum.VBLF + "    ORDER BY A.DISPSEQ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            ssForm_Sheet1.RowCount = dt.Rows.Count;
            ssForm_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssForm_Sheet1.Cells[i, 1].Text = dt.Rows[i]["FORMNO"].ToString().Trim();
                ssForm_Sheet1.Cells[i, 2].Text = dt.Rows[i]["FORMNAME"].ToString().Trim();
                ssForm_Sheet1.Cells[i, 3].Text = dt.Rows[i]["FORMNAMEPRINT"].ToString().Trim();
                if (dt.Rows[i]["USECHECK"].ToString().Trim() == "1")
                {
                    ssForm_Sheet1.Cells[i, 4].Text = "Y";
                }
                else
                {
                    ssForm_Sheet1.Cells[i, 4].Text = "";
                    ssForm_Sheet1.Cells[i, 0, i, ssForm_Sheet1.ColumnCount - 1].ForeColor = Color.Red;
                }

            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        private void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDept.Text.Trim() == "") return;
            mstrDeptCd = cboDept.Text.Trim();
            GetUserForm();
        }

        private void mbtnSave_Click(object sender, EventArgs e)
        {
            if (mstrGRPGB == "D")
            {
                if (mstrDeptCd.Trim() == "")
                {
                    ComFunc.MsgBoxEx(this, "진료과를 선택해주십시요.");
                    return;
                }
            }

            if (SaveData() == true)
            {
                GetUserForm();
            }
        }

        private bool SaveData()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            SetSql();

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                string strFormNo = "";
                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");

                for (i = 0; i < ssForm_Sheet1.RowCount; i++)
                {
                    strFormNo = "";
                    if (Convert.ToBoolean(ssForm_Sheet1.Cells[i, 0].Value) == true)
                    {
                        int SeqNo = 0;
                        strFormNo = ssForm_Sheet1.Cells[i, 1].Text.Trim();

                        SQL = " SELECT COUNT(FORMNO) CNT";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRUSERFORM ";
                        SQL = SQL + ComNum.VBLF + "WHERE GRPTYPE = 'FD' ";
                        SQL = SQL + mstrSubSql;

                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return false;
                        }

                        if (dt.Rows.Count <= 0)
                        {
                            SeqNo = 0;
                        }
                        else
                        {
                            SeqNo = (int)VB.Val(dt.Rows[0]["CNT"].ToString().Trim());
                        }

                        dt.Dispose();
                        dt = null;


                        SQL = " SELECT FORMNO";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRUSERFORM ";
                        SQL = SQL + ComNum.VBLF + "WHERE GRPTYPE = 'FD' ";
                        SQL = SQL + mstrSubSql;
                        SQL = SQL + ComNum.VBLF + "    AND FORMNO = " + strFormNo;
                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return false;
                        }
                        if (dt.Rows.Count <= 0)
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRUSERFORM";
                            SQL = SQL + ComNum.VBLF + "(GRPTYPE, GRPGB, USEGB, FORMNO, DISPSEQ, WRITEDATE, WRITETIME)";
                            SQL = SQL + ComNum.VBLF + "VALUES (";
                            SQL = SQL + ComNum.VBLF + "   'FD',";
                            SQL = SQL + mstrSubSql1;
                            SQL = SQL + ComNum.VBLF + "   " + strFormNo + ",";
                            SQL = SQL + ComNum.VBLF + "   " + SeqNo + ",";
                            SQL = SQL + ComNum.VBLF + "   '" + VB.Left(strCurDateTime, 8) + "',";
                            SQL = SQL + ComNum.VBLF + "   '" + VB.Right(strCurDateTime, 6) + "'";
                            SQL = SQL + ComNum.VBLF + ")";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBoxEx(this, SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return false;
                            }
                        }
                        dt.Dispose();
                        dt = null;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                //ComFunc.MsgBoxEx(this, "저장 하였습니다.");
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void mbtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mbtnDeleate_Click(object sender, EventArgs e)
        {
            if (DeleteData() == true)
            {
                GetUserForm();
            }
        }

        List<String> CheckedNames(TreeNodeCollection theNodes)
        {
            List<String> aResult = new List<String>();

            if (theNodes != null)
            {
                foreach (TreeNode aNode in theNodes)
                {
                    if (aNode.Checked)
                    {
                        string strIndex = aNode.Name.ToString();
                        string[] strParams = VB.Split(VB.Trim(strIndex), sKeyHead);
                        string strFormNo = strParams[0].ToString();
                        //aResult.Add(aNode.Text);
                        if (aNode.GetNodeCount(false) == 0)
                        {
                            aResult.Add(strFormNo);
                        }
                    }

                    aResult.AddRange(CheckedNames(aNode.Nodes));
                }
            }

            return aResult;
        }

        private bool DeleteData()
        {
            List<String> strTree = CheckedNames(trvUserForm.Nodes);
            if (strTree.Count == 0) return false;

            int i = 0;
            //DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            SetSql();

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {

                for (i = 0; i < strTree.Count; i++)
                {
                    SQL = "DELETE FROM " + ComNum.DB_EMR + "AEMRUSERFORM";
                    SQL = SQL + ComNum.VBLF + "WHERE GRPTYPE = 'FD'";
                    SQL = SQL + mstrSubSql;
                    SQL = SQL + ComNum.VBLF + "    AND FORMNO = " + strTree[i].Trim();
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                //ComFunc.MsgBoxEx(this, "삭제 하였습니다.");
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private bool DeleteDataOld()
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            SetSql();

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                TreeNodeCollection nodesCollection = null;
                nodesCollection = trvUserForm.Nodes;
                for (int i = 0; i < nodesCollection.Count; i++)
                {
                    TreeNode node = nodesCollection[i];

                    if (node.Nodes.Count == 0)
                    {
                        if (node.Checked == true)
                        {
                            string strIndex = node.Name.ToString();
                            string[] strParams = VB.Split(VB.Trim(strIndex), sKeyHead);
                            string strFormNo = strParams[0].ToString();
                            string strUpdateNo = strParams[1].ToString();

                            SQL = "DELETE FROM " + ComNum.DB_EMR + "AEMRUSERFORM";
                            SQL = SQL + ComNum.VBLF + "WHERE GRPTYPE = 'FD'";
                            SQL = SQL + mstrSubSql;
                            SQL = SQL + ComNum.VBLF + "    AND FORMNO = " + strFormNo;
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBoxEx(this, SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (DeleteDataSub(node) == false)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, "저장중 에러가 발생했습니다.");
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                //ComFunc.MsgBoxEx(this, "삭제 하였습니다.");
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private bool DeleteDataSub(TreeNode tnc)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            try
            {
                TreeNodeCollection nodesCollection = null;
                nodesCollection = tnc.Nodes;

                for (int i = 0; i < nodesCollection.Count; i++)
                {
                    TreeNode node = nodesCollection[i];
                    if (node.Nodes.Count == 0)
                    {
                        if (node.Checked == true)
                        {
                            string strIndex = node.Name.ToString();
                            string[] strParams = VB.Split(VB.Trim(strIndex), sKeyHead);
                            string strFormNo = strParams[0].ToString();
                            string strUpdateNo = strParams[1].ToString();

                            SQL = "DELETE FROM " + ComNum.DB_EMR + "AEMRUSERFORM";
                            SQL = SQL + ComNum.VBLF + "WHERE GRPTYPE = 'FD'";
                            SQL = SQL + mstrSubSql;
                            SQL = SQL + ComNum.VBLF + "    AND FORMNO = " + strFormNo;
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return DeleteDataSub(node);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        private void mbtnExpandAll_Click(object sender, EventArgs e)
        {
            trvEmrGroup.ExpandAll(); 
        }

        private void mbtnCollapseAll_Click(object sender, EventArgs e)
        {
            trvEmrGroup.CollapseAll();
        }

        private void mbtnExpandDept_Click(object sender, EventArgs e)
        {
            trvUserForm.ExpandAll();
        }

        private void mbtnCollapseDept_Click(object sender, EventArgs e)
        {
            trvUserForm.CollapseAll();
        }

        private void mbtnSearch_Click(object sender, EventArgs e)
        {
            GetFormListSearch();
        }

        private void txtFormName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                GetFormListSearch();
            }
        }

        private void GetFormListSearch()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssForm_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            SQL = " SELECT A.FORMNO, A.UPDATENO, A.FORMNAME, A.FORMNAMEPRINT, A.USECHECK ";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRFORM A";
            SQL = SQL + ComNum.VBLF + "    WHERE UPPER(A.FORMNAME) LIKE '%" + txtFormName.Text.Trim().ToUpper() + "%'";
            SQL = SQL + ComNum.VBLF + "        AND A.UPDATENO = (SELECT MAX(B.UPDATENO) FROM " + ComNum.DB_EMR + "AEMRFORM B";
            SQL = SQL + ComNum.VBLF + "                                        WHERE B.FORMNO = A.FORMNO AND B.USECHECK = '1' )";
            SQL = SQL + ComNum.VBLF + "        AND A.USECHECK = '1'";
            SQL = SQL + ComNum.VBLF + "        AND A.GRPFORMNO <> 1078";
            SQL = SQL + ComNum.VBLF + "    ORDER BY A.FORMNAME , A.DISPSEQ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            ssForm_Sheet1.RowCount = dt.Rows.Count;
            ssForm_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssForm_Sheet1.Cells[i, 1].Text = dt.Rows[i]["FORMNO"].ToString().Trim();
                ssForm_Sheet1.Cells[i, 2].Text = dt.Rows[i]["FORMNAME"].ToString().Trim();
                ssForm_Sheet1.Cells[i, 3].Text = dt.Rows[i]["FORMNAMEPRINT"].ToString().Trim();
                if (dt.Rows[i]["USECHECK"].ToString().Trim() == "1")
                {
                    ssForm_Sheet1.Cells[i, 4].Text = "Y";
                }
                else
                {
                    ssForm_Sheet1.Cells[i, 4].Text = "";
                    ssForm_Sheet1.Cells[i, 0, i, ssForm_Sheet1.ColumnCount - 1].ForeColor = Color.Red;
                }

            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        

    }
}
