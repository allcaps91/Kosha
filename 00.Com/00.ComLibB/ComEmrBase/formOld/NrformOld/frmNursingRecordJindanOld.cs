using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmNursingRecordJindanOld : Form
    {
        //private string mOption;
        private string mMACROGB;
        private string mMACROGBOLD;
        private string mlngMACROINDEX;

        //int nImage;
        //int nSelectedImage;
        int nImageSaved;
        int nSelectedImageSaved;

        string treeName = "";
        int treeVal = 0;
        //int iSeq = 0;
        int depthInt = 0;
        //int depthMax = 0;
        //bool treeCheck = false;
        //System.Windows.Forms.TreeNode tree;
        //string gStrUserGb = "";
        //string gStrUserGbCopy = "";
        //bool pbolCopy = false;
        //string strNanda = ""; //'병동간호팀장
        //int gCurrentNodeLevel = 0;
        

        public frmNursingRecordJindanOld()
        {
            InitializeComponent();
        }

        private void frmNursingRecordJindanOld_Load(object sender, EventArgs e)
        {
            //nImage = 0;
            //nSelectedImage = 1;

            nImageSaved = 2;
            nSelectedImageSaved = 3;

            trvJindan.ImageList = this.ImageList2;
            trvJindanOld.ImageList = this.ImageList2;

            pComboSet();
            //setWardTree("0", "NANDAJIN", trvJindanOld);
            optDept.Checked = true;
        }

        private void pComboSet()
        {
            int i = 0;
            DataTable dt = null;

            cboWard.Items.Clear();
            //-->병동콤보 세팅
            cboWard.Items.Add("전  체" + VB.Space(50) + "0");

            dt = clsEmrQueryPohangS.READ_WARD_LIST(clsDB.DbCon);

            if (dt == null)
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count != 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboWard.Items.Add((dt.Rows[i]["WARDNAME"].ToString() + "").Trim() + VB.Space(50) + (dt.Rows[i]["WARDCD"].ToString() + "").Trim());
                }
            }
            dt.Dispose();
            dt = null;
            cboWard.SelectedIndex = 0;

            for (i = 0; i < cboWard.Items.Count; i++)
            {
                if ((VB.Right(cboWard.Items[i].ToString(), 10)).Trim() == clsType.User.BuseCode)
                {
                    cboWard.SelectedIndex = i;
                    break;
                }
            }
        }

        private void setWardTree(string strFlag, string strValue, TreeView trvJindanX)
        {
            ssData_Sheet1.RowCount = 0;
            ssAction_Sheet1.RowCount = 0;
            ssResponse_Sheet1.RowCount = 0;
            ssNarration_Sheet1.RowCount = 0;

            trvJindanX.Nodes.Clear();  // '트리 초기화

            //gStrUserGb = strValue;
            string strMACROGB = strValue;


            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            
            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT A.MACROGB, A.MACROINDEX, A.MACROKEY, A.MACROPARENT, A.MACRONAME,";
            SQL = SQL + ComNum.VBLF + "      (SELECT MAX(O.MACROINDEX) FROM ADMIN.EMRMACROETCDTL O";
            SQL = SQL + ComNum.VBLF + "              WHERE O.MACROGB = A.MACROGB AND O.MACROINDEX = A.MACROINDEX) AS DTLYN";
            SQL = SQL + ComNum.VBLF + "          FROM ADMIN.EMRMACROETC A";
            SQL = SQL + ComNum.VBLF + "         WHERE A.MACROGB = '" + strMACROGB + "'";
            SQL = SQL + ComNum.VBLF + "         ORDER BY A.MACROPARENTV, A.SYSDSPINDEX";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
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

            if (dt.Rows.Count > 0)
            {
                //System.Windows.Forms.TreeNode /*oNodex*/;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["MACROPARENT"].ToString().Trim() == "0_")
                    {
                        trvJindanX.Nodes.Add((dt.Rows[i]["MACROKEY"].ToString() + "").Trim(), (dt.Rows[i]["MACRONAME"].ToString() + "").Trim(), nImageSaved, nSelectedImageSaved);                        
                    }
                    else
                    {
                        if (trvJindanX.Nodes.Find((dt.Rows[i]["MACROPARENT"].ToString() + "").Trim(), true).Length > 0)
                        {
                            trvJindanX.Nodes.Find((dt.Rows[i]["MACROPARENT"].ToString() + "").Trim(), true)[0].Nodes.Add((dt.Rows[i]["MACROKEY"].ToString() + "").Trim(), (dt.Rows[i]["MACRONAME"].ToString() + "").Trim());
                        }
                    }
                }
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.WaitCursor;
        }

        //private void optNandaCopy_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (optNandaCopy.Checked == true)
        //    {
        //        cboWard.Enabled = false;
        //        setWardTree("0", "NANDAJIN", trvJindanCopy);
        //    }
        //}

        //private void optWardCopy_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (optWardCopy.Checked == true)
        //    {
        //        cboWard.Enabled = true;
        //        if (cboWard.Text.Trim() == "")
        //        {
        //            return;
        //        }
        //        setWardTree("0", cboWard.Text.Trim().Substring(cboWard.Text.Trim().Length - 6, 6).Trim(), trvJindanCopy);
        //    }
        //}

        //private void cboWardCopy_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (optWardCopy.Checked == true)
        //    {
        //        cboWard.Enabled = true;
        //        if (cboWard.Text.Trim() == "")
        //        {
        //            return;
        //        }
        //        setWardTree("0", cboWard.Text.Trim().Substring(cboWard.Text.Trim().Length - 6, 6).Trim(), trvJindanCopy);
        //    }
        //}

        //private void optNanda_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (optUse.Checked == true)
        //    {
        //        cboWard.Enabled = false;
        //        setWardTree("1", "NANDAJIN", trvJindan);
        //    }
        //}

        //private void optWard_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (optAll.Checked == true)
        //    {
        //        cboWard.Enabled = true;
        //        if (cboWard.Text.Trim() == "")
        //        {
        //            return;
        //        }
        //        txtGrpformNo.Text = "";
        //        txtJindan.Text = "";
        //        setWardTree("1", cboWard.Text.Trim().Substring(cboWard.Text.Trim().Length - 6, 6).Trim(), trvJindan);
        //    }
        //}

        //private void cboWard_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (optAll.Checked == true)
        //    {
        //        cboWard.Enabled = true;
        //        if (cboWard.Text.Trim() == "")
        //        {
        //            return;
        //        }
        //        txtGrpformNo.Text = "";
        //        txtJindan.Text = "";
        //        setWardTree("1", cboWard.Text.Trim().Substring(cboWard.Text.Trim().Length - 6, 6).Trim(), trvJindan);
        //    }
        //}

        private void mbtnRootAdd_Click(object sender, EventArgs e)
        {
            if (AddFirstSibling() == true)
            {
                txtNodeName.Text = "";
                if (optUse.Checked == true)
                {
                    cboWard.Enabled = false;

                    //mOption = "U";
                    switch (clsType.User.Sabun)
                    {
                        case "14472":
                        case "16047":
                        case "22901":
                        case "28727":
                        case "28754":
                            mMACROGB = "21987";
                            break;
                        case "15317":
                        case "13662":
                            mMACROGB = "15317";
                            break;
                        default:
                            mMACROGB = clsType.User.IdNumber;
                            break;
                    }

                    setWardTree("0", mMACROGB, trvJindan);
                }
                else
                {
                    cboWard.Enabled = true;
                    if (cboWard.Text.Trim() == "" || cboWard.Text.Trim().Length < 6)
                    {
                        return;
                    }

                    //mOption = "D";
                    mMACROGB = cboWard.Text.Trim().Substring(cboWard.Text.Trim().Length - 6, 6).Trim();
                    setWardTree("0", mMACROGB, trvJindan);
                }
            }
        }

        private bool SaveAddNode(string sKey, TreeNode tnParent)
        {
            bool rtnVal = false;            
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                SQL = "";                
                SQL = SQL + ComNum.VBLF + "  INSERT INTO ADMIN.EMRMACROETC ";
                SQL = SQL + ComNum.VBLF + "      (MACROGB,MACROINDEX,MACROKEY,MACROPARENT,MACRONAME,MACROKEYV,MACROPARENTV) ";
                SQL = SQL + ComNum.VBLF + "  VALUES  (";
                SQL = SQL + ComNum.VBLF + "      '" + mMACROGB + "',";
                SQL = SQL + ComNum.VBLF + "      '" + sKey.Split('_')[1] + "',";
                SQL = SQL + ComNum.VBLF + "      '" + sKey + "',";
                SQL = SQL + ComNum.VBLF + "      '" + tnParent.Name + "', ";
                SQL = SQL + ComNum.VBLF + "      '" + trvJindan.Nodes.Find(sKey, true)[0].Text + "',";
                SQL = SQL + ComNum.VBLF + "      '" + sKey.Split('_')[0] + "',";
                SQL = SQL + ComNum.VBLF + "      '" + tnParent.Name.Split('_')[0] + "')";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                try
                {
                    int iFirstNode = tnParent.FirstNode.Index;
                    int intNode = tnParent.FirstNode.Index;
                    int iLastNode = tnParent.LastNode.Index;
                    
                    int k = 1;
                    for (k = 1; k <= tnParent.GetNodeCount(false);  k++)
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "UPDATE ADMIN.EMRMACROETC ";
                        SQL = SQL + ComNum.VBLF + "  SET SYSDSPINDEX = " + k;
                        SQL = SQL + ComNum.VBLF + "  WHERE MACROINDEX = " + (int)VB.Val(trvJindan.Nodes[intNode].Name.Split('_')[1]);

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return false;
                        }

                        intNode = trvJindan.Nodes[intNode].NextNode.Index;
                    }                    
                }
                catch
                { }

                clsDB.setCommitTran(clsDB.DbCon);
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this,  ex.Message);
                return rtnVal;
            }
        }
        
        private bool AddFirstSibling()
        {
            bool rtnVal = false;
            string sKey = "";

            //TreeNode tnParent = null;
            string nodeName = "";

            nodeName = txtNodeName.Text.Trim();

            if (nodeName == "")
            {
                ComFunc.MsgBoxEx(this,  "명칭을 입력하셔야 합니다.!");
                txtNodeName.Focus();
                return rtnVal;
            }
            
            txtNodeName.Text = "";
            //tnParent = trvJindan.SelectedNode;
            //
            //if (tnParent == null)
            //{
            //    ComFunc.MsgBoxEx(this,  "추가할 위치의 노드를 선택하십시오.");
            //    return rtnVal;
            //}

            ////' 명칭 중복 검사
            //treeVal = 0;
            //if (ExistData() == true)
            //{
            //    ComFunc.MsgBoxEx(this,  "중복된 명칭이 존재합니다." + ComNum.VBLF + "하위그룹(자식)을 추가할 수 없습니다.");
            //    txtNodeName.Text = "";
            //    txtNodeName.Focus();
            //    return rtnVal;
            //}

            string lngMACRONo = ComQuery.GetSequencesNo(clsDB.DbCon, "" + ComNum.DB_EMR + "GetSeqMecroEtc").ToString();
            
            if (lngMACRONo == "0")
            {
                ComFunc.MsgBoxEx(this,  "일련번호 생성중 오류가 발생했습니다.");
                return rtnVal;
            }

            sKey = GetNextKey(trvJindan);
            trvJindan.Nodes.Insert(0, sKey + lngMACRONo, nodeName);

            return SaveAddNode(sKey, lngMACRONo);
        }

        private bool AddLastSibling()
        {
            bool rtnVal = false;
            string nodeName = txtNodeName.Text.Trim();

            if (string.IsNullOrWhiteSpace(nodeName))
            {
                ComFunc.MsgBoxEx(this, "명칭을 입력하셔야 합니다.!");
                txtNodeName.Focus();
                return rtnVal;
            }

            txtNodeName.Clear();
            string lngMACRONo = ComQuery.GetSequencesNo(clsDB.DbCon, "" + ComNum.DB_EMR + "GetSeqMecroEtc").ToString();

            if (lngMACRONo == "0")
            {
                ComFunc.MsgBoxEx(this, "일련번호 생성중 오류가 발생했습니다.");
                return rtnVal;
            }

            string sKey = GetNextKey(trvJindan);

            int LastIndex = trvJindan.Nodes.Count;

            trvJindan.Nodes.Insert(LastIndex, sKey + lngMACRONo, nodeName);

            return SaveAddNode(sKey, lngMACRONo);
        }

        private bool AddNextSibling()
        {
            bool rtnVal = false;
            string sKey = "";

            TreeNode tnParent = null;
            string nodeName = "";
            int iIndex = 0;

            nodeName = txtNodeName.Text.Trim();

            if (nodeName == "")
            {
                ComFunc.MsgBoxEx(this,  "명칭을 입력하셔야 합니다.!");
                txtNodeName.Focus();
                return rtnVal;
            }

            txtNodeName.Text = "";
            tnParent = trvJindan.SelectedNode;

            if (tnParent == null)
            {
                ComFunc.MsgBoxEx(this,  "추가할 위치의 노드를 선택하십시오.");
                return rtnVal;
            }

            iIndex = trvJindan.SelectedNode.Index + 1;

            string lngMACRONo = ComQuery.GetSequencesNo(clsDB.DbCon, "" + ComNum.DB_EMR + "GetSeqMecroEtc").ToString();

            if (lngMACRONo == "0")
            {
                ComFunc.MsgBoxEx(this,  "일련번호 생성중 오류가 발생했습니다.");
                return rtnVal;
            }

            sKey = GetNextKey(trvJindan);
            trvJindan.Nodes.Insert(iIndex, sKey + lngMACRONo, nodeName);

            return SaveAddNode(sKey, lngMACRONo);
        }

        private bool AddPrevSibling()
        {
            bool rtnVal = false;
            string sKey = "";

            TreeNode tnParent = null;
            string nodeName = "";
            int iIndex = 0;

            nodeName = txtNodeName.Text.Trim();

            if (nodeName == "")
            {
                ComFunc.MsgBoxEx(this,  "명칭을 입력하셔야 합니다.!");
                txtNodeName.Focus();
                return rtnVal;
            }

            txtNodeName.Text = "";
            tnParent = trvJindan.SelectedNode;

            if (tnParent == null)
            {
                ComFunc.MsgBoxEx(this,  "추가할 위치의 노드를 선택하십시오.");
                return rtnVal;
            }

            iIndex = trvJindan.SelectedNode.Index;

            string lngMACRONo = ComQuery.GetSequencesNo(clsDB.DbCon, "" + ComNum.DB_EMR + "GetSeqMecroEtc").ToString();

            if (lngMACRONo == "0")
            {
                ComFunc.MsgBoxEx(this,  "일련번호 생성중 오류가 발생했습니다.");
                return rtnVal;
            }

            sKey = GetNextKey(trvJindan);
            trvJindan.Nodes.Insert(iIndex, sKey + lngMACRONo, nodeName);

            return SaveAddNode(sKey, lngMACRONo);
        }

        private bool SaveAddNode(string sKey, string grpFormNoPt)
        {
            bool rtnVal = false;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            int iIndex = GetNewIndex(sKey + grpFormNoPt, trvJindan);
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "  INSERT INTO ADMIN.EMRMACROETC ";
                SQL = SQL + ComNum.VBLF + "      (MACROGB,MACROINDEX,MACROKEY,MACROPARENT,MACRONAME,MACROKEYV,MACROPARENTV) ";
                SQL = SQL + ComNum.VBLF + "  VALUES  (";
                SQL = SQL + ComNum.VBLF + "      '" + mMACROGB + "', ";
                SQL = SQL + ComNum.VBLF + "      '" + trvJindan.Nodes[iIndex].Name.Split('_')[1] + "', ";
                SQL = SQL + ComNum.VBLF + "      '" + trvJindan.Nodes[iIndex].Name + "', ";
                SQL = SQL + ComNum.VBLF + "      '" + GetParentKey(iIndex, trvJindan) + "', ";
                SQL = SQL + ComNum.VBLF + "      '" + trvJindan.Nodes[iIndex].Text + "', ";
                SQL = SQL + ComNum.VBLF + "      '" + trvJindan.Nodes[iIndex].Name.Split('_')[0] + "', ";
                SQL = SQL + ComNum.VBLF + "      '" + GetParentKey(iIndex, trvJindan).Split('_')[0] + "')";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                try
                {
                    if (trvJindan.Nodes[iIndex].Level == 0)
                    {
                        foreach (TreeNode n in trvJindan.Nodes)
                        {
                            if (n.Level == 0)
                            {
                                SQL = "";
                                SQL = SQL + ComNum.VBLF + "UPDATE ADMIN.EMRMACROETC ";
                                SQL = SQL + ComNum.VBLF + "  SET SYSDSPINDEX = " + n.Index;
                                SQL = SQL + ComNum.VBLF + "  WHERE MACROINDEX = " + (int)VB.Val(n.Name.Split('_')[1]);

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return false;
                                }
                            }
                        }
                    }
                    else
                    {
                        TreeNode mFirstNode = trvJindan.Nodes[iIndex].Parent.FirstNode;
                        TreeNode mNode = trvJindan.Nodes[iIndex].Parent.FirstNode;
                        TreeNode mLastNode = trvJindan.Nodes[iIndex].Parent.LastNode;

                        int k = 1;
                        while (mNode.Equals(mNode.Parent.LastNode) == false)
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "UPDATE ADMIN.EMRMACROETC ";
                            SQL = SQL + ComNum.VBLF + "  SET SYSDSPINDEX = " + k;
                            SQL = SQL + ComNum.VBLF + "  WHERE MACROINDEX = " + (int)VB.Val(mNode.Name.Split('_')[1]);

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return false;
                            }

                            k = k + 1;
                            mNode = mNode.NextNode;
                        }

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "UPDATE ADMIN.EMRMACROETC ";
                        SQL = SQL + ComNum.VBLF + "  SET SYSDSPINDEX = " + k;
                        SQL = SQL + ComNum.VBLF + "  WHERE MACROINDEX = " + (int)VB.Val(mLastNode.Name.Split('_')[1]);

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                    }
                }
                catch
                { }

                clsDB.setCommitTran(clsDB.DbCon);
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this,  ex.Message);
                return rtnVal;
            }
        }

        private int GetNewIndex(string sKey, TreeView trvJindanX)
        {
            TreeNode[] oNode = null;
            int rtnVal = 0;

            oNode = trvJindanX.Nodes.Find(sKey, true);

            //for (int i = 0; i < trvJindanX.Nodes.Count; i++)
            //{
            //    if (trvJindanX.Nodes[i].Name.Trim() == sKey)
            //    {
            //        rtnVal = trvJindanX.Nodes[i].Index;
            //    }
            //}

            if (oNode[0] != null)
            {
                rtnVal = oNode[0].Index;
            } 

            return rtnVal;
        }

        private string GetNextKey(TreeView trvJindanX)
        {
            string sNewKey = "";
            int iHold = 0;

            try
            {
                iHold = (int)VB.Val(trvJindanX.Nodes[0].Name.Split('_')[0]);

                for (int i = 0; i < trvJindanX.Nodes.Count; i++)
                {
                    if ((int)VB.Val(trvJindanX.Nodes[i].Name.Split('_')[0]) > iHold)
                    {
                        iHold = (int)VB.Val(trvJindanX.Nodes[i].Name.Split('_')[0]);
                    }
                }

                iHold = iHold + 1;
                sNewKey = Convert.ToString(iHold) + "_";

                return sNewKey;
            }
            catch
            {
                return "1_";
            }
        }

        private string GetParentKey(int index, TreeView trvJindanX)
        {
            try
            {
                return trvJindanX.Nodes[index].Parent.Name;
            }
            catch
            {
                return "0_";
            }
        }

        private bool ExistData()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            bool rtnVal = false;

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT * FROM ADMIN.EMRMACROETCDTL";
            SQL = SQL + ComNum.VBLF + "       WHERE MACROGB = '" + mMACROGB + "'";
            SQL = SQL + ComNum.VBLF + "       AND MACROINDEX = " + mlngMACROINDEX;

            //SQL = SQL + ComNum.VBLF + "    SELECT GRPFORMNAME ";
            //SQL = SQL + ComNum.VBLF + "        FROM MHEMR.EMRNRDIAGNOSISGROUP";
            //SQL = SQL + ComNum.VBLF + "        WHERE     GRPFORMNAME = '" + grpFormName + "'";
            //SQL = SQL + ComNum.VBLF + "            AND     GROUPPARENT = " + treeVal;

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this,  "조회중 문제가 발생했습니다");
                return rtnVal;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return rtnVal;
            }

            dt.Dispose();
            dt = null;
            rtnVal = true;
            return rtnVal;
        }

        private void mBtnChildAdd_Click(object sender, EventArgs e)
        {            
            if (txtNodeName.Text.Trim() == "")
            {
                ComFunc.MsgBoxEx(this,  "명칭을 입력하셔야 합니다.!");
                txtNodeName.Focus();
                return;
            }

            TreeNode tnParent = trvJindan.SelectedNode;

            if (tnParent == null)
            {
                ComFunc.MsgBoxEx(this,  "추가할 위치의 노드를 선택하십시오.");
                return;
            }

            System.Windows.Forms.TreeNode oNodex;            
            int iIndex = trvJindan.SelectedNode.Index;
            string grpFormNoPt = ComQuery.GetSequencesNo(clsDB.DbCon, "" + ComNum.DB_EMR + "GetSeqMecroEtc").ToString();
            string sKey = GetNextKey(trvJindan) + grpFormNoPt;


            oNodex = trvJindan.Nodes.Find(tnParent.Name, true)[0].Nodes.Add(sKey, txtNodeName.Text);

            Application.DoEvents();

            SaveAddNode(sKey, tnParent);

            txtNodeName.Text = "";
            if (optUse.Checked == true)
            {
                cboWard.Enabled = false;

                //mOption = "U";
                switch (clsType.User.Sabun)
                {
                    case "14472":
                    case "16047":
                    case "22901":
                    case "28727":
                    case "28754":
                        mMACROGB = "21987";
                        break;
                    case "15317":
                    case "13662":
                        mMACROGB = "15317";
                        break;
                    default:
                        mMACROGB = clsType.User.IdNumber;
                        break;
                }

                setWardTree("0", mMACROGB, trvJindan);
            }
            else
            {
                cboWard.Enabled = true;
                if (cboWard.Text.Trim() == "" || cboWard.Text.Trim().Length < 6)
                {
                    return;
                }

                //mOption = "D";
                mMACROGB = cboWard.Text.Trim().Substring(cboWard.Text.Trim().Length - 6, 6).Trim();
                setWardTree("0", mMACROGB, trvJindan);
            }
        }

        private bool SaveChild()
        {
            bool rtnVal = false;
            string sKey = "";

            TreeNode tnParent = null;
            string nodeName = "";

            nodeName = txtNodeName.Text.Trim();

            string strUserGb = "";

            if (optAll.Checked == true)
            {
                if (cboWard.Text.Trim() == "" || cboWard.Text.Trim().Length < 6)
                {
                    ComFunc.MsgBoxEx(this,  "병동을 선택하세요");
                    return rtnVal;
                }
                strUserGb = cboWard.Text.Trim().Substring(cboWard.Text.Trim().Length - 6, 6).Trim();
            }

            //if (optUse.Checked == true)
            //{
            //    strUserGb = "NANDAJIN";
            //}

            if (nodeName == "")
            {
                ComFunc.MsgBoxEx(this,  "명칭을 입력하셔야 합니다.!");
                txtNodeName.Focus();
                return rtnVal;
            }

            //if (txtGroupSeq.Text.Trim() == "")
            //{
            //    txtGroupSeq.Text = "999";
            //}

            txtNodeName.Text = "";
            tnParent = trvJindan.SelectedNode;

            if (tnParent == null)
            {
                ComFunc.MsgBoxEx(this,  "선택된 항목이 없습니다.!");
                return rtnVal;
            }
            //' 명칭 중복 검사
            treeVal = 0;
            if (ExistData() == true)
            {
                ComFunc.MsgBoxEx(this,  "중복된 명칭이 존재합니다." + ComNum.VBLF + "하위그룹(자식)을 추가할 수 없습니다.");
                txtNodeName.Text = "";
                txtNodeName.Focus();
                return rtnVal;
            }

            string grpFormNoPt = ComQuery.GetSequencesNo(clsDB.DbCon, "" + ComNum.DB_EMR + "GetSeqMecroEtc").ToString();

            if (grpFormNoPt == "0")
            {
                ComFunc.MsgBoxEx(this,  "일련번호 생성중 오류가 발생했습니다.");
                return rtnVal;
            }

            sKey = GetNextKey(trvJindan);

            trvJindan.Nodes.Add(sKey + grpFormNoPt, nodeName);
            int iIndex = GetNewIndex(sKey + grpFormNoPt, trvJindan);


            if (depth(Convert.ToInt32(VB.Val(tnParent.Name))) == false)
            {
                ComFunc.MsgBoxEx(this,  "병동을 선택하세요.");
                return rtnVal;
            }

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "  INSERT INTO ADMIN.EMRMACROETC ";
                SQL = SQL + ComNum.VBLF + "      (MACROGB,MACROINDEX,MACROKEY,MACROPARENT,MACRONAME,MACROKEYV,MACROPARENTV) ";
                SQL = SQL + ComNum.VBLF + "  VALUES  (";
                SQL = SQL + ComNum.VBLF + "      '" + mMACROGB + "',";
                SQL = SQL + ComNum.VBLF + "      '" + trvJindan.Nodes[iIndex].Name.Split('_')[1] + "',";
                SQL = SQL + ComNum.VBLF + "      '" + trvJindan.Nodes[iIndex].Name + "',";
                SQL = SQL + ComNum.VBLF + "      '" + GetParentKey(iIndex, trvJindan) + "', ";
                SQL = SQL + ComNum.VBLF + "      '" + trvJindan.Nodes[iIndex].Text + "',";
                SQL = SQL + ComNum.VBLF + "      '" + trvJindan.Nodes[iIndex].Name.Split('_')[0] + "',";
                SQL = SQL + ComNum.VBLF + "      '" + GetParentKey(iIndex, trvJindan).Split('_')[0] + "')";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }


                try
                {
                    int iFirstNode = trvJindan.Nodes[iIndex].FirstNode.Index;
                    int iLastNode = trvJindan.Nodes[iIndex].LastNode.Index;


                    int k = 1;
                    while (iFirstNode != trvJindan.Nodes[iFirstNode].LastNode.Index)
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "UPDATE ADMIN.EMRMACROETC ";
                        SQL = SQL + ComNum.VBLF + "  SET SYSDSPINDEX = " + k;
                        SQL = SQL + ComNum.VBLF + "  WHERE MACROINDEX = " + (int)VB.Val(trvJindan.Nodes[iFirstNode].LastNode.Name.Split('_')[1]);

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "UPDATE ADMIN.EMRMACROETC ";
                    SQL = SQL + ComNum.VBLF + "  SET SYSDSPINDEX = " + k;
                    SQL = SQL + ComNum.VBLF + "  WHERE MACROINDEX = " + (int)VB.Val(trvJindan.Nodes[iLastNode].LastNode.Name.Split('_')[1]);

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }
                catch
                { }

                clsDB.setCommitTran(clsDB.DbCon);
                rtnVal = true;
                ComFunc.MsgBoxEx(this,  "저장 하였습니다.");
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this,  ex.Message);
                return rtnVal;
            }
        }

        private bool depth(int intVal)
        {
            bool rtnVal = false;
            bool rtnValX = false;
            int ordParent = 0;
            string strUserGb = "";

            if (optAll.Checked == true)
            {
                if (cboWard.Text.Trim() == "" || cboWard.Text.Trim().Length < 6)
                {
                    ComFunc.MsgBoxEx(this,  "병동을 선택하세요");
                    return rtnVal;
                }
                strUserGb = cboWard.Text.Trim().Substring(cboWard.Text.Trim().Length - 6, 6).Trim();
            }

            if (optUse.Checked == true)
            {
                strUserGb = clsType.User.Sabun;
            }

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL = SQL + ComNum.VBLF + "  SELECT GROUPPARENT ";
            SQL = SQL + ComNum.VBLF + "      FROM MHEMR.EMRNRDIAGNOSISGROUP ";
            SQL = SQL + ComNum.VBLF + "  WHERE GRPFORMNO = '" + intVal + "'";
            SQL = SQL + ComNum.VBLF + "         AND USECLS ='1'";
            SQL = SQL + ComNum.VBLF + "         AND USERGB ='" + strUserGb + "'";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this,  "조회중 문제가 발생했습니다");
                return rtnVal;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return true;
            }

            if (dt.Rows[0]["GROUPPARENT"].ToString().Trim() != "0")
            {
                depthInt = depthInt + 1;
                ordParent = Convert.ToInt32(dt.Rows[0]["GROUPPARENT"].ToString().Trim());
                dt.Dispose();
                dt = null;
                rtnValX = depth(ordParent);
                return rtnValX;
            }
            else
            {
                dt.Dispose();
                dt = null;
                depthInt = depthInt + 1;
                return true;
            }
        }

        private void trvJindan_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.GetNodeCount(true) > 0)
            {
                return;
            }

            txtGrpformNo.Text = "";
            txtJindan.Text = "";
            ssData_Sheet1.RowCount = 0;
            ssAction_Sheet1.RowCount = 0;
            ssResponse_Sheet1.RowCount = 0;
            ssNarration_Sheet1.RowCount = 0;

            //tree = trvJindan.SelectedNode;
            treeName = e.Node.Text.Trim();
            treeVal = Convert.ToInt32(VB.Val(e.Node.Name));
            txtGrpformNo.Text = Convert.ToString(treeVal);
            txtJindan.Text = treeName;

            mlngMACROINDEX = e.Node.Name.Split('_')[1];

            //string strUserGb = "";

            if (optUse.Checked == true)
            {
                mMACROGBOLD = clsType.User.IdNumber;
            }
            else
            {
                mMACROGBOLD = cboWard.Text.Trim().Substring(cboWard.Text.Trim().Length - 6, 6).Trim();
            }
            GetNrMacroList("DATA", (int)VB.Val(mlngMACROINDEX), mMACROGBOLD, ssData_Sheet1, "");
            GetNrMacroList("ACTION", (int)VB.Val(mlngMACROINDEX), mMACROGBOLD, ssAction_Sheet1, "");
            GetNrMacroList("RESULT", (int)VB.Val(mlngMACROINDEX), mMACROGBOLD, ssResponse_Sheet1, "");
            GetNrMacroList("NARA", (int)VB.Val(mlngMACROINDEX), mMACROGBOLD, ssNarration_Sheet1, "");

            GetJindanInfo(treeVal, treeName);
        }

        private void GetNrMacroList(string mMACROCD, int mlngMACROINDEX, string mMACROGB, FarPoint.Win.Spread.SheetView spd, string strCopy)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            //SQL = "";
            //SQL = SQL + ComNum.VBLF + "  SELECT MACRONO, JINDANNO, TYPE, CONTENT, DISPSEQ, USERGB, USEID, USECLS";
            //SQL = SQL + ComNum.VBLF + "  FROM MHEMR.EMRNRMACRO";
            //SQL = SQL + ComNum.VBLF + "  WHERE JINDANNO = " + intJinDanNo;
            //SQL = SQL + ComNum.VBLF + "  AND TYPE = '" + strOption + "'";
            //SQL = SQL + ComNum.VBLF + "  AND USERGB = '" + strUserGb + "'";
            //SQL = SQL + ComNum.VBLF + "  ORDER BY DISPSEQ ";
            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT ";
            SQL = SQL + ComNum.VBLF + "  A.MACROCD, MACROINDEX, ";
            SQL = SQL + ComNum.VBLF + "  A.MACROSEQ, A.MACROTEXT, A.MACRODSP";
            SQL = SQL + ComNum.VBLF + " FROM ADMIN.EMRMACROETCDTL A";
            SQL = SQL + ComNum.VBLF + "          WHERE A.MACROGB = '" + mMACROGB + "'";
            SQL = SQL + ComNum.VBLF + "          AND A.MACROINDEX = " + mlngMACROINDEX;
            SQL = SQL + ComNum.VBLF + "          AND A.MACROCD = '"+ mMACROCD + "'";


            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this,  "조회중 문제가 발생했습니다");
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                spd.RowCount = 1;
                spd.SetRowHeight(-1, ComNum.SPDROWHT);
                Cursor.Current = Cursors.Default;
                return;
            }

            spd.RowCount = dt.Rows.Count + 1;
            spd.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                spd.Cells[i, 0].Value = true;
                spd.Cells[i, 1].Text = dt.Rows[i]["MACROTEXT"].ToString().Trim();
                spd.Cells[i, 2].Text = dt.Rows[i]["MACRODSP"].ToString().Trim();
                if (strCopy != "COPY")
                {
                    spd.Cells[i, 3].Text = dt.Rows[i]["MACROSEQ"].ToString().Trim();
                }
                //if (dt.Rows[i]["USECLS"].ToString().Trim() == "0")
                //{
                //    spd.Cells[i, 0, i, spd.ColumnCount - 1].BackColor = Color.LightGray;
                //}
                //else
                //{
                //    spd.Cells[i, 0].Value = true;
                //}
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        private void trvJindanCopy_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ssData_Sheet1.RowCount = 0;
            ssAction_Sheet1.RowCount = 0;
            ssResponse_Sheet1.RowCount = 0;
            ssNarration_Sheet1.RowCount = 0;
            //string strUserGb = "";

            mlngMACROINDEX = e.Node.Name.Split('_')[1];

            //if (optNandaCopy.Checked == true)
            //{
            //    strUserGb = "NANDAJIN";
            //}
            //else
            //{
            //  strUserGb = cboWard.Text.Trim().Substring(cboWard.Text.Trim().Length - 6, 6).Trim();
            //}

            GetNrMacroList("DATA", (int)VB.Val(mlngMACROINDEX), mMACROGBOLD, ssData_Sheet1, "");
            GetNrMacroList("ACTION", (int)VB.Val(mlngMACROINDEX), mMACROGBOLD, ssAction_Sheet1, "");
            GetNrMacroList("RESULT", (int)VB.Val(mlngMACROINDEX), mMACROGBOLD, ssResponse_Sheet1, "");
            GetNrMacroList("NARA", (int)VB.Val(mlngMACROINDEX), mMACROGBOLD, ssNarration_Sheet1, "");

            //GetNrMacroList("D", Convert.ToInt32(VB.Val(e.Node.Name)), e.Node.Text.Trim(), strUserGb, ssData_Sheet1, "COPY");
            //GetNrMacroList("A", Convert.ToInt32(VB.Val(e.Node.Name)), e.Node.Text.Trim(), strUserGb, ssAction_Sheet1, "COPY");
            //GetNrMacroList("R", Convert.ToInt32(VB.Val(e.Node.Name)), e.Node.Text.Trim(), strUserGb, ssResponse_Sheet1, "COPY");
        }


        private void GetJindanInfo(int intVal, string strName)
        {
            //string strUserGb = "";

            //if (optUse.Checked == true)
            //{
            //    strUserGb = "NANDAJIN";
            //}
            //else
            //{
            //    strUserGb = cboWard.Text.Trim().Substring(cboWard.Text.Trim().Length - 6, 6).Trim();
            //}
            GetNrMacroList("DATA", (int)VB.Val(mlngMACROINDEX), mMACROGBOLD, ssData_Sheet1, "");
            GetNrMacroList("ACTION", (int)VB.Val(mlngMACROINDEX), mMACROGBOLD, ssAction_Sheet1, "");
            GetNrMacroList("RESULT", (int)VB.Val(mlngMACROINDEX), mMACROGBOLD, ssResponse_Sheet1, "");
            GetNrMacroList("NARA", (int)VB.Val(mlngMACROINDEX), mMACROGBOLD, ssNarration_Sheet1, "");
        }

        private void mBtnNameChange_Click(object sender, EventArgs e)
        {
            if (ChangeNode() == true)
            {
                txtNodeName.Text = "";
                if (optUse.Checked == true)
                {
                    cboWard.Enabled = false;

                    //mOption = "U";
                    switch (clsType.User.Sabun)
                    {
                        case "14472":
                        case "16047":
                        case "22901":
                        case "28727":
                        case "28754":
                            mMACROGB = "21987";
                            break;
                        case "15317":
                        case "13662":
                            mMACROGB = "15317";
                            break;
                        default:
                            mMACROGB = clsType.User.IdNumber;
                            break;
                    }

                    setWardTree("0", mMACROGB, trvJindan);
                }
                else
                {
                    cboWard.Enabled = true;
                    if (cboWard.Text.Trim() == "" || cboWard.Text.Trim().Length < 6)
                    {
                        return;
                    }

                    //mOption = "D";
                    mMACROGB = cboWard.Text.Trim().Substring(cboWard.Text.Trim().Length - 6, 6).Trim();
                    setWardTree("0", mMACROGB, trvJindan);
                }
            }
        }

        private bool ChangeNode()
        {
            bool rtnVal = false;

            TreeNode tnParent = null;
            string nodeName = "";

            nodeName = txtNodeName.Text.Trim();

            if (nodeName == "")
            {
                ComFunc.MsgBoxEx(this,  "명칭을 입력하셔야 합니다.!");
                txtNodeName.Focus();
                return rtnVal;
            }
            txtNodeName.Text = "";
            tnParent = trvJindan.SelectedNode;

            if (tnParent == null)
            {
                ComFunc.MsgBoxEx(this,  "선택된 항목이 없습니다.!");
                return rtnVal;
            }
            //' 명칭 중복 검사
            treeVal = 0;
            if (ExistData() == true)
            {
                ComFunc.MsgBoxEx(this,  "중복된 명칭이 존재합니다." + ComNum.VBLF + "하위그룹(자식)을 추가할 수 없습니다.");
                txtNodeName.Text = "";
                txtNodeName.Focus();
                return rtnVal;
            }

            //string strUserGb = "";

            //if (optUse.Checked == true)
            //{
            //    strUserGb = "NANDAJIN";
            //}
            //else
            //{
            //    strUserGb = cboWard.Text.Trim().Substring(cboWard.Text.Trim().Length - 6, 6).Trim();
            //}

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                SQL = "";
                //SQL = SQL + ComNum.VBLF + " UPDATE   MHEMR.EMRNRDIAGNOSISGROUP   SET ";
                //SQL = SQL + ComNum.VBLF + "      GRPFORMNAME = '" + nodeName + "'";
                //SQL = SQL + ComNum.VBLF + " WHERE    GRPFORMNO = " + Convert.ToInt32(VB.Val(tnParent.Name));
                //SQL = SQL + ComNum.VBLF + "     AND    USERGB = '" + strUserGb + "'";
                //SQL = SQL + ComNum.VBLF + "     AND    USECLS ='1'";
                SQL = SQL + ComNum.VBLF + " UPDATE ADMIN.EMRMACROETC SET MACRONAME = '" + nodeName + "'";
                SQL = SQL + ComNum.VBLF + "       WHERE MACROGB = '" + mMACROGB + "'";
                SQL = SQL + ComNum.VBLF + "       AND MACROINDEX = " + mlngMACROINDEX;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                rtnVal = true;
                ComFunc.MsgBoxEx(this,  "저장 하였습니다.");
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this,  ex.Message);
                return rtnVal;
            }

        }

        private void mBtnDelete_Click(object sender, EventArgs e)
        {
            if (DeleteNode() == true)
            {
                txtNodeName.Text = "";
                if (optUse.Checked == true)
                {
                    cboWard.Enabled = false;

                    //mOption = "U";
                    switch (clsType.User.Sabun)
                    {
                        case "14472":
                        case "16047":
                        case "22901":
                        case "28727":
                        case "28754":
                            mMACROGB = "21987";
                            break;
                        case "15317":
                        case "13662":
                            mMACROGB = "15317";
                            break;
                        default:
                            mMACROGB = clsType.User.IdNumber;
                            break;
                    }

                    setWardTree("0", mMACROGB, trvJindan);
                }
                else
                {
                    cboWard.Enabled = true;
                    if (cboWard.Text.Trim() == "" || cboWard.Text.Trim().Length < 6)
                    {
                        return;
                    }

                    //mOption = "D";
                    mMACROGB = cboWard.Text.Trim().Substring(cboWard.Text.Trim().Length - 6, 6).Trim();
                    setWardTree("0", mMACROGB, trvJindan);
                }
            }
        }

        private bool DeleteNode()
        {
            bool rtnVal = false;

            TreeNode tnParent = null;
            string nodeName = "";

            nodeName = txtNodeName.Text.Trim();

            tnParent = trvJindan.SelectedNode;

            if (tnParent == null)
            {
                ComFunc.MsgBoxEx(this,  "선택된 항목이 없습니다.!");
                return rtnVal;
            }
            //' 명칭 중복 검사
            treeVal = 0;
            if (ExistData() == true)
            {
                ComFunc.MsgBoxEx(this,  "중복된 명칭이 존재합니다." + ComNum.VBLF + "하위그룹(자식)을 추가할 수 없습니다.");
                txtNodeName.Text = "";
                txtNodeName.Focus();
                return rtnVal;
            }

            //string strUserGb = "";

            //if (optUse.Checked == true)
            //{
            //    strUserGb = "NANDAJIN";
            //}
            //else
            //{
            //    strUserGb = cboWard.Text.Trim().Substring(cboWard.Text.Trim().Length - 6, 6).Trim();
            //}

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE FROM ADMIN.EMRMACROETCDTL";
                SQL = SQL + ComNum.VBLF + "  WHERE MACROGB = '" + mMACROGB + "'";
                SQL = SQL + ComNum.VBLF + "    AND MACROINDEX = " + tnParent.Name.Split('_')[1];

                //SQL = SQL + ComNum.VBLF + " DELETE   FROM    MHEMR.EMRNRDIAGNOSISGROUP";
                //SQL = SQL + ComNum.VBLF + " WHERE    GRPFORMNO = " + Convert.ToInt32(VB.Val(tnParent.Name));
                //SQL = SQL + ComNum.VBLF + "     AND    USERGB = '" + strUserGb + "'";
                //SQL = SQL + ComNum.VBLF + "     AND    USECLS ='1'";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE FROM ADMIN.EMRMACROETC ";
                SQL = SQL + ComNum.VBLF + "  WHERE MACROGB = '" + mMACROGB + "'";
                SQL = SQL + ComNum.VBLF + "    AND MACROINDEX = " + tnParent.Name.Split('_')[1];

                //SQL = SQL + ComNum.VBLF + " DELETE   FROM    MHEMR.EMRNRMACRO ";
                //SQL = SQL + ComNum.VBLF + " WHERE    JINDANNO  = " + Convert.ToInt32(VB.Val(tnParent.Name));
                //SQL = SQL + ComNum.VBLF + "        AND USERGB = '" + strUserGb + "'";
                //SQL = SQL + ComNum.VBLF + "     AND    USECLS ='1'";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                rtnVal = true;
                //ComFunc.MsgBoxEx(this,  "저장 하였습니다.");
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this,  ex.Message);
                return rtnVal;
            }

        }

        private void txtUpdateSeq_Click(object sender, EventArgs e)
        {
            if (SortNode() == true)
            {
                //txtGroupSeq.Text = "";
                //if (optNanda.Checked == true)
                //{
                //    cboWard.Enabled = false;
                //    setWardTree("1", "NANDAJIN", trvJindan);
                //}
                //else
                //{
                //    cboWard.Enabled = true;
                //    if (cboWard.Text.Trim() == "" || cboWard.Text.Trim().Length < 6)
                //    {
                //        return;
                //    }
                //    setWardTree("1", cboWard.Text.Trim().Substring(cboWard.Text.Trim().Length - 6, 6).Trim(), trvJindan);
                //}
            }
        }

        private bool SortNode()
        {
            bool rtnVal = false;

            TreeNode tnParent = null;
            string nodeName = "";

            nodeName = txtNodeName.Text.Trim();

            if (nodeName == "")
            {
                ComFunc.MsgBoxEx(this,  "명칭을 입력하셔야 합니다.!");
                txtNodeName.Focus();
                return rtnVal;
            }
            txtNodeName.Text = "";
            tnParent = trvJindan.SelectedNode;

            if (tnParent == null)
            {
                ComFunc.MsgBoxEx(this,  "선택된 항목이 없습니다.!");
                return rtnVal;
            }
            //' 명칭 중복 검사
            treeVal = 0;
            if (ExistData() == true)
            {
                ComFunc.MsgBoxEx(this,  "중복된 명칭이 존재합니다." + ComNum.VBLF + "하위그룹(자식)을 추가할 수 없습니다.");
                txtNodeName.Text = "";
                txtNodeName.Focus();
                return rtnVal;
            }

            string strGroupSeq = "";    // txtGroupSeq.Text.Trim();

            if (VB.Val(strGroupSeq) == 0)
            {
                strGroupSeq = "999";
            }

            string strUserGb = "";

            //if (optNandaCopy.Checked == true)
            //{
            //    strUserGb = "NANDAJIN";
            //}
            //else
            //{
                strUserGb = cboWard.Text.Trim().Substring(cboWard.Text.Trim().Length - 6, 6).Trim();
            //}

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " UPDATE   MHEMR.EMRNRDIAGNOSISGROUP   SET ";
                SQL = SQL + ComNum.VBLF + "      GROUPSEQ = '" + strGroupSeq + "'";
                SQL = SQL + ComNum.VBLF + " WHERE    GRPFORMNO = " + Convert.ToInt32(VB.Val(tnParent.Name));
                SQL = SQL + ComNum.VBLF + "     AND    USERGB = '" + strUserGb + "'";
                SQL = SQL + ComNum.VBLF + "     AND    USECLS ='1'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                rtnVal = true;
                ComFunc.MsgBoxEx(this,  "저장 하였습니다.");
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this,  ex.Message);
                return rtnVal;
            }

        }

        private void CheckSpd(FarPoint.Win.Spread.SheetView spd, bool chkVal)
        {
            int i = 0;
            for (i = 0; i < spd.RowCount; i++)
            {
                spd.Cells[i, 0].Value = chkVal;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            CheckSpd(ssData_Sheet1, true);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            CheckSpd(ssData_Sheet1, false);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            CheckSpd(ssAction_Sheet1, true);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            CheckSpd(ssAction_Sheet1, false);
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            CheckSpd(ssResponse_Sheet1, true);
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            CheckSpd(ssResponse_Sheet1, false);
        }
        
        private void button7_Click(object sender, EventArgs e)
        {
            CheckSpd(ssNarration_Sheet1, true);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            CheckSpd(ssNarration_Sheet1, false);
        }

        private void DelNrMacro(FarPoint.Win.Spread.SheetView spd, string strMacroCd)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            int lngMACROSEQ = 0;

            int i = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                for (i = 0; i < spd.RowCount; i++)
                {                    
                    if (Convert.ToBoolean(spd.Cells[i, 0].Value) == true)
                    {
                        lngMACROSEQ = (int)VB.Val(spd.Cells[i, 3].Text);

                        SQL = " DELETE FROM ADMIN.EMRMACROETCDTL";
                        SQL = SQL + ComNum.VBLF + " WHERE MACROGB = '" + mMACROGB + "'";
                        SQL = SQL + ComNum.VBLF + " AND MACROINDEX = " + mlngMACROINDEX;
                        SQL = SQL + ComNum.VBLF + " AND MACROCD = '" + strMacroCd + "'";
                        SQL = SQL + ComNum.VBLF + " AND MACROSEQ = " + lngMACROSEQ;

                        //SQL = " DELETE FROM MHEMR.EMRNRMACRO ";
                        //SQL = SQL + ComNum.VBLF + " WHERE MACRONO =  " + VB.Val(spd.Cells[i, 3].Text.Trim());
                        //SQL = SQL + ComNum.VBLF + "   AND TYPE =  'D'";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this,  "삭제 하였습니다.");

                GetJindanInfo(treeVal, treeName);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this,  ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnDelData_Click(object sender, EventArgs e)
        {
            DelNrMacro(ssData_Sheet1, "DATA");
        }

        private void btnDelAction_Click(object sender, EventArgs e)
        {
            DelNrMacro(ssAction_Sheet1, "ACTION");
        }

        private void btnDelResponse_Click(object sender, EventArgs e)
        {
            DelNrMacro(ssResponse_Sheet1, "RESULT");
        }

        private void btnDelNarration_Click(object sender, EventArgs e)
        {
            DelNrMacro(ssNarration_Sheet1, "NARA");
        }

        private void btnAddData_Click(object sender, EventArgs e)
        {
            ssData_Sheet1.RowCount = ssData_Sheet1.RowCount + 1;
            ssData_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
        }

        private void btnAddAction_Click(object sender, EventArgs e)
        {
            ssAction_Sheet1.RowCount = ssAction_Sheet1.RowCount + 1;
            ssAction_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
        }

        private void btnAddResponse_Click(object sender, EventArgs e)
        {
            ssResponse_Sheet1.RowCount = ssResponse_Sheet1.RowCount + 1;
            ssResponse_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
        }

        private void btnAddNarration_Click(object sender, EventArgs e)
        {
            ssNarration_Sheet1.RowCount = ssNarration_Sheet1.RowCount + 1;
            ssNarration_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
        }

        private void SaveNrMacro(FarPoint.Win.Spread.SheetView spd, string strTYPE)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strJindanno = txtGrpformNo.Text.Trim();
            if (strJindanno == "")
            {
                ComFunc.MsgBoxEx(this, "간호진단을 선택해주십시요.");
                return;
            }

            //string strUserGb = "";

            //if (optUse.Checked == true)
            //{
            //    strUserGb = "NANDAJIN";
            //}
            //else
            //{
            //    strUserGb = cboWard.Text.Trim().Substring(cboWard.Text.Trim().Length - 6, 6).Trim();
            //}

            //string strUSECLS = "";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE FROM ADMIN.EMRMACROETCDTL";
                SQL = SQL + ComNum.VBLF + "       WHERE MACROGB = '" + mMACROGB + "'";
                SQL = SQL + ComNum.VBLF + "       AND MACROINDEX = " + mlngMACROINDEX;
                SQL = SQL + ComNum.VBLF + "       AND MACROCD = '" + strTYPE + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                for (int i = 0; i < spd.RowCount; i++)
                {                    
                    //if (Convert.ToBoolean(spd.Cells[i, 0].Value) == true)
                    //{
                    //    strUSECLS = "1";
                    //}
                    //else
                    //{
                    //    strUSECLS = "0";
                    //}
                    
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " INSERT INTO ADMIN.EMRMACROETCDTL";
                    SQL = SQL + ComNum.VBLF + "      (MACROGB, MACROINDEX, MACROCD, MACROSEQ, MACROTEXT, MACRODSP)";
                    SQL = SQL + ComNum.VBLF + "      VALUES(";
                    SQL = SQL + ComNum.VBLF + "     '" + mMACROGB + "',";
                    SQL = SQL + ComNum.VBLF + "     " + mlngMACROINDEX + ",";
                    SQL = SQL + ComNum.VBLF + "     '" + strTYPE + "',";
                    SQL = SQL + ComNum.VBLF + "     " + i + ",";
                    SQL = SQL + ComNum.VBLF + "     '" + spd.Cells[i, 1].Text.Trim().Replace("'", "`") + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + spd.Cells[i, 2].Text.Trim() + "')";
                        
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this,  "저장 하였습니다.");

                GetJindanInfo(treeVal, treeName);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this,  ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSavaData_Click(object sender, EventArgs e)
        {
            SaveNrMacro(ssData_Sheet1, "DATA");
        }

        private void btnSavaAction_Click(object sender, EventArgs e)
        {
            SaveNrMacro(ssAction_Sheet1, "ACTION");
        }

        private void btnSavaResponse_Click(object sender, EventArgs e)
        {
            SaveNrMacro(ssResponse_Sheet1, "RESULT");
        }


        private void btnSaveNarration_Click(object sender, EventArgs e)
        {
            SaveNrMacro(ssNarration_Sheet1, "NARA");
        }

        private void btnSaveAll_Click(object sender, EventArgs e)
        {
            int iIndex = trvJindan.SelectedNode.Index;

            if (trvJindan.Nodes[iIndex].GetNodeCount(true) > 0)
            {
                ComFunc.MsgBox("하위 그룹이 존재함으로 사용문구를 등록할 수 없습니다.");
                return;
            }
            
            mlngMACROINDEX = trvJindan.Nodes[iIndex].Name.Split('_')[1];

            if (ExistData() == true)
            {
                ComFunc.MsgBoxEx(this,  "중복된 명칭이 존재합니다." + ComNum.VBLF + "하위그룹(자식)을 추가할 수 없습니다.");
                txtNodeName.Text = "";
                txtNodeName.Focus();
                return;
            }

            SaveNrMacro(ssData_Sheet1, "DATA");
            SaveNrMacro(ssAction_Sheet1, "ACTION");
            SaveNrMacro(ssResponse_Sheet1, "RESULT");
            SaveNrMacro(ssNarration_Sheet1, "NARA");
            //SaveNrMacroAll();
        }

        private void SaveNrMacroAll()
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            int i = 0;
            string strJindanno = txtGrpformNo.Text.Trim();
            if (strJindanno == "")
            {
                ComFunc.MsgBoxEx(this,  "간호진단을 선택해주십시요.");
                return;
            }

            string strUserGb = "";

            if (optUse.Checked == true)
            {
                strUserGb = clsType.User.Sabun;
            }
            else
            {
                strUserGb = cboWard.Text.Trim().Substring(cboWard.Text.Trim().Length - 6, 6).Trim();
            }

            string strUSECLS = "";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                for (i = 0; i < ssData_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(ssData_Sheet1.Cells[i, 0].Value) == true)
                    {
                        strUSECLS = "1";
                    }
                    else
                    {
                        strUSECLS = "0";
                    }

                    if (ssData_Sheet1.Cells[i, 3].Text.Trim() == "")
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "  INSERT INTO MHEMR.EMRNRMACRO ";
                        SQL = SQL + ComNum.VBLF + "      (MACRONO, JINDANNO, TYPE, CONTENT, DISPSEQ, USERGB, USEID, USECLS) ";
                        SQL = SQL + ComNum.VBLF + "  VALUES  ";
                        SQL = SQL + ComNum.VBLF + "      (MHEMR.EMRNRMACRO_MACRONO_SEQ.NEXTVAL,";
                        SQL = SQL + ComNum.VBLF + "      " + strJindanno + ",";
                        SQL = SQL + ComNum.VBLF + "      'D',";
                        SQL = SQL + ComNum.VBLF + "      '" + VB.Replace(ssData_Sheet1.Cells[i, 1].Text.Trim(), "'", "`") + "',";
                        SQL = SQL + ComNum.VBLF + "      '" + ssData_Sheet1.Cells[i, 2].Text.Trim() + "',";
                        SQL = SQL + ComNum.VBLF + "      '" + strUserGb + "',";
                        SQL = SQL + ComNum.VBLF + "      '" + clsType.User.Sabun + "',";
                        SQL = SQL + ComNum.VBLF + "      '1')";
                    }
                    else
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "  UPDATE MHEMR.EMRNRMACRO ";
                        SQL = SQL + ComNum.VBLF + "       SET JINDANNO = '" + strJindanno + "',";
                        SQL = SQL + ComNum.VBLF + "     CONTENT = '" + VB.Replace(ssData_Sheet1.Cells[i, 1].Text.Trim(), "'", "`") + "',";
                        SQL = SQL + ComNum.VBLF + "     USECLS = '" + strUSECLS + "',";
                        SQL = SQL + ComNum.VBLF + "     DISPSEQ = '" + ssData_Sheet1.Cells[i, 2].Text.Trim() + "'";
                        SQL = SQL + ComNum.VBLF + "  WHERE MACRONO = " + VB.Val(ssData_Sheet1.Cells[i, 3].Text.Trim());
                        SQL = SQL + ComNum.VBLF + "     AND USERGB = '" + strUserGb + "'";
                    }
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                //ssAction_Sheet1
                for (i = 0; i < ssAction_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(ssAction_Sheet1.Cells[i, 0].Value) == true)
                    {
                        strUSECLS = "1";
                    }
                    else
                    {
                        strUSECLS = "0";
                    }

                    if (ssAction_Sheet1.Cells[i, 3].Text.Trim() == "")
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "  INSERT INTO MHEMR.EMRNRMACRO ";
                        SQL = SQL + ComNum.VBLF + "      (MACRONO, JINDANNO, TYPE, CONTENT, DISPSEQ, USERGB, USEID, USECLS) ";
                        SQL = SQL + ComNum.VBLF + "  VALUES  ";
                        SQL = SQL + ComNum.VBLF + "      (MHEMR.EMRNRMACRO_MACRONO_SEQ.NEXTVAL,";
                        SQL = SQL + ComNum.VBLF + "      " + strJindanno + ",";
                        SQL = SQL + ComNum.VBLF + "      'A',";
                        SQL = SQL + ComNum.VBLF + "      '" + VB.Replace(ssAction_Sheet1.Cells[i, 1].Text.Trim(), "'", "`") + "',";
                        SQL = SQL + ComNum.VBLF + "      '" + ssAction_Sheet1.Cells[i, 2].Text.Trim() + "',";
                        SQL = SQL + ComNum.VBLF + "      '" + strUserGb + "',";
                        SQL = SQL + ComNum.VBLF + "      '" + clsType.User.Sabun + "',";
                        SQL = SQL + ComNum.VBLF + "      '1')";
                    }
                    else
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "  UPDATE MHEMR.EMRNRMACRO ";
                        SQL = SQL + ComNum.VBLF + "       SET JINDANNO = '" + strJindanno + "',";
                        SQL = SQL + ComNum.VBLF + "     CONTENT = '" + VB.Replace(ssAction_Sheet1.Cells[i, 1].Text.Trim(), "'", "`") + "',";
                        SQL = SQL + ComNum.VBLF + "     USECLS = '" + strUSECLS + "',";
                        SQL = SQL + ComNum.VBLF + "     DISPSEQ = '" + ssAction_Sheet1.Cells[i, 2].Text.Trim() + "'";
                        SQL = SQL + ComNum.VBLF + "  WHERE MACRONO = " + VB.Val(ssAction_Sheet1.Cells[i, 3].Text.Trim());
                        SQL = SQL + ComNum.VBLF + "     AND USERGB = '" + strUserGb + "'";
                    }
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                //ssResponse_Sheet1
                for (i = 0; i < ssResponse_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(ssResponse_Sheet1.Cells[i, 0].Value) == true)
                    {
                        strUSECLS = "1";
                    }
                    else
                    {
                        strUSECLS = "0";
                    }

                    if (ssResponse_Sheet1.Cells[i, 3].Text.Trim() == "")
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "  INSERT INTO MHEMR.EMRNRMACRO ";
                        SQL = SQL + ComNum.VBLF + "      (MACRONO, JINDANNO, TYPE, CONTENT, DISPSEQ, USERGB, USEID, USECLS) ";
                        SQL = SQL + ComNum.VBLF + "  VALUES  ";
                        SQL = SQL + ComNum.VBLF + "      (MHEMR.EMRNRMACRO_MACRONO_SEQ.NEXTVAL,";
                        SQL = SQL + ComNum.VBLF + "      " + strJindanno + ",";
                        SQL = SQL + ComNum.VBLF + "      'R',";
                        SQL = SQL + ComNum.VBLF + "      '" + VB.Replace(ssResponse_Sheet1.Cells[i, 1].Text.Trim(), "'", "`") + "',";
                        SQL = SQL + ComNum.VBLF + "      '" + ssResponse_Sheet1.Cells[i, 2].Text.Trim() + "',";
                        SQL = SQL + ComNum.VBLF + "      '" + strUserGb + "',";
                        SQL = SQL + ComNum.VBLF + "      '" + clsType.User.Sabun + "',";
                        SQL = SQL + ComNum.VBLF + "      '1')";
                    }
                    else
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "  UPDATE MHEMR.EMRNRMACRO ";
                        SQL = SQL + ComNum.VBLF + "       SET JINDANNO = '" + strJindanno + "',";
                        SQL = SQL + ComNum.VBLF + "     CONTENT = '" + VB.Replace(ssResponse_Sheet1.Cells[i, 1].Text.Trim(), "'", "`") + "',";
                        SQL = SQL + ComNum.VBLF + "     USECLS = '" + strUSECLS + "',";
                        SQL = SQL + ComNum.VBLF + "     DISPSEQ = '" + ssResponse_Sheet1.Cells[i, 2].Text.Trim() + "'";
                        SQL = SQL + ComNum.VBLF + "  WHERE MACRONO = " + VB.Val(ssResponse_Sheet1.Cells[i, 3].Text.Trim());
                        SQL = SQL + ComNum.VBLF + "     AND USERGB = '" + strUserGb + "'";
                    }
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this,  "저장 하였습니다.");

                GetJindanInfo(treeVal, treeName);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this,  ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void mbtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void optAll_CheckedChanged(object sender, EventArgs e)
        {
            if (optAll.Checked == true)
            {
                //mOption = "H";
                mMACROGB = "WARD";
                setWardTree("0", mMACROGB, trvJindan);
            }
        }

        private void optDept_CheckedChanged(object sender, EventArgs e)
        {
            if (optDept.Checked == true)
            {
                //mOption = "D";
                mMACROGB = clsType.User.BuseCode;
                setWardTree("0", mMACROGB, trvJindan);
            }
        }

        private void optUse_CheckedChanged(object sender, EventArgs e)
        {
            if (optUse.Checked == true)
            {
                //mOption = "U";
                switch (clsType.User.Sabun)
                {
                    case "14472":
                    case "16047":
                    case "22901":
                    case "28727":
                    case "28754":
                        mMACROGB = "21987";
                        break;
                    case "15317":
                    case "13662":
                        mMACROGB = "15317";
                        break;
                    default:
                        mMACROGB = clsType.User.IdNumber;
                        break;
                }
                setWardTree("0", mMACROGB, trvJindan);
            }
        }

        private void cboWard_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetOldMacro();
        }

        private void GetOldMacro()
        {
            //string strMACROGB = "";

            if (cboWard.SelectedIndex == 0)
            {
                mMACROGBOLD = clsType.User.DeptCode;
            }
            else
            {
                mMACROGBOLD = VB.Trim(VB.Right(cboWard.Text, 10));
            }

            setWardTree("0", mMACROGBOLD, trvJindanOld);
        }

        private void btnAddLast_Click(object sender, EventArgs e)
        {            
            if (AddLastSibling() == true)
            {
                txtNodeName.Text = "";
                if (optUse.Checked == true)
                {
                    cboWard.Enabled = false;

                    //mOption = "U";
                    switch (clsType.User.Sabun)
                    {
                        case "14472":
                        case "16047":
                        case "22901":
                        case "28727":
                        case "28754":
                            mMACROGB = "21987";
                            break;
                        case "15317":
                        case "13662":
                            mMACROGB = "15317";
                            break;
                        default:
                            mMACROGB = clsType.User.IdNumber;
                            break;
                    }

                    setWardTree("0", mMACROGB, trvJindan);
                }
                else
                {
                    cboWard.Enabled = true;
                    if (cboWard.Text.Trim() == "" || cboWard.Text.Trim().Length < 6)
                    {
                        return;
                    }

                    //mOption = "D";
                    mMACROGB = cboWard.Text.Trim().Substring(cboWard.Text.Trim().Length - 6, 6).Trim();
                    setWardTree("0", mMACROGB, trvJindan);
                }
            }
        }

        private void btnAddNext_Click(object sender, EventArgs e)
        {
            if (AddNextSibling() == true)
            {                
                txtNodeName.Text = "";
                if (optUse.Checked == true)
                {
                    cboWard.Enabled = false;

                    //mOption = "U";
                    switch (clsType.User.Sabun)
                    {
                        case "14472":
                        case "16047":
                        case "22901":
                        case "28727":
                        case "28754":
                            mMACROGB = "21987";
                            break;
                        case "15317":
                        case "13662":
                            mMACROGB = "15317";
                            break;
                        default:
                            mMACROGB = clsType.User.IdNumber;
                            break;
                    }

                    setWardTree("0", mMACROGB, trvJindan);
                }
                else
                {
                    cboWard.Enabled = true;
                    if (cboWard.Text.Trim() == "" || cboWard.Text.Trim().Length < 6)
                    {
                        return;
                    }

                    //mOption = "D";
                    mMACROGB = cboWard.Text.Trim().Substring(cboWard.Text.Trim().Length - 6, 6).Trim();
                    setWardTree("0", mMACROGB, trvJindan);
                }
            }
        }

        private void btnAddPrev_Click(object sender, EventArgs e)
        {
            if (AddPrevSibling() == true)
            {
                txtNodeName.Text = "";
                if (optUse.Checked == true)
                {
                    cboWard.Enabled = false;

                    //mOption = "U";
                    switch (clsType.User.Sabun)
                    {
                        case "14472":
                        case "16047":
                        case "22901":
                        case "28727":
                        case "28754":
                            mMACROGB = "21987";
                            break;
                        case "15317":
                        case "13662":
                            mMACROGB = "15317";
                            break;
                        default:
                            mMACROGB = clsType.User.IdNumber;
                            break;
                    }

                    setWardTree("0", mMACROGB, trvJindan);
                }
                else
                {
                    cboWard.Enabled = true;
                    if (cboWard.Text.Trim() == "" || cboWard.Text.Trim().Length < 6)
                    {
                        return;
                    }

                    //mOption = "D";
                    mMACROGB = cboWard.Text.Trim().Substring(cboWard.Text.Trim().Length - 6, 6).Trim();
                    setWardTree("0", mMACROGB, trvJindan);
                }
            }
        }
    }
}
