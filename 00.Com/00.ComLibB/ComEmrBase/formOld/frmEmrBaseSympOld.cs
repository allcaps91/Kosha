using ComBase;
using System;
using System.Data;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ComEmrBase
{
    /// <summary>
    /// VB : PSMH\mtsEmr\frmEmrSymp.frm
    /// </summary>
    /// <summary>
    /// Class Name      : ComEmrBase
    /// File Name       : frmEmrBaseSympOld
    /// Description     : 증상 DB
    /// Author          : 박웅규
    /// Create Date     : 2018-05-10
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// TODO : 폼 호출, 엑셀저장
    /// TODO : 전체적인 트리뷰 관련 함수 부분 확인 필요
    /// </history>
    /// <seealso cref= "PSMHVB\\Ocs\OpdOcs\Oorder\mtsoorder.vbp(PSMHVB\mtsEmr\frmEmrSymp.frm) >> frmEmrBaseSympOld.cs 폼이름 재정의" />
    public partial class frmEmrBaseSympOld : Form
    {
        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOZORDER = 0x0004;
        private const int TOPMOST_FLAGS = SWP_NOZORDER | SWP_NOSIZE;

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        // string mOption = "";
        int intImage = 0;
        int intSelectedImage = 1;
        int intImageSaved = 2;
        int intSelectedImageSaved = 2;

        public string mSYSMPGB = "";
        public string mDeptCd = "";
        public string mUseId = "";
        public string mConTrolId = "";
        public string mConTrolNm = "";

        bool blnFormLoad = false;

        //폼이 Close될 경우
        public delegate void EventMakeText(int intOption, string strMacro);
        public event EventMakeText rEventMakeText;

        //폼이 Close될 경우
        //public delegate void EventClosed();
        //public event EventClosed rEventClosed;

        //모니터 사이즈, 폼 위치
        //private int mintTop = 0;
        //private int mintLeft = 0;
        private int mintMonitor = 0;
        private int[] mintWidth = null;
        private int[] mintHeight = null;

        /// <summary>
        /// 모니터
        /// </summary>
        private void GetMonitorInfo()
        {
            Screen[] screens = Screen.AllScreens;

            mintMonitor = screens.Length;
            mintWidth = new int[mintMonitor];
            mintHeight = new int[mintMonitor];

            int i = 0;
            foreach (Screen screen in screens)
            {
                mintWidth[i] = screen.Bounds.Width;
                mintHeight[i] = screen.Bounds.Height;
                i = i + 1;
            }
        }

        /// <summary>
        /// 2번 모니터 띄우기
        /// </summary>
        private void viewFormMonitor2()
        {
            Screen[] screens = Screen.AllScreens;
            Screen secondary_screen = null;

            if (screens.Length == 1)    //모니터 하나
            {
                this.Show();
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                foreach (Screen screen in screens)
                {
                    if (screen.Primary == false)
                    {
                        secondary_screen = screen;
                        this.Bounds = secondary_screen.Bounds;
                        //this.Top = 0;
                        //this.Left = 0;
                        this.Show();
                        this.WindowState = FormWindowState.Maximized;
                        break;
                    }
                }
            }
        }

        public frmEmrBaseSympOld()
        {
            InitializeComponent();
        }

        public frmEmrBaseSympOld(string pSYSMPGB, string pDeptCd, string pUseId, string pConTrolId, string pConTrolNm)
        {
            InitializeComponent();
            mSYSMPGB = pSYSMPGB;
            mDeptCd = pDeptCd;
            mUseId = pUseId;
            mConTrolId = pConTrolId;
            mConTrolNm = pConTrolNm;
        }

        private void frmEmrBaseSympOld_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            GetMonitorInfo();

            string strEmrOption = "";
            //프로그래스 화면 위치(왼쪽,오른쪽)
            strEmrOption = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "OOORD", "PROGPOSIOTION");
            if (VB.Val(strEmrOption) == 1)
            {
                //VB에서 사용하는 탑모스트 구현
                SetWindowPos(this.Handle, HWND_TOPMOST, 660, 100, 0, 0, TOPMOST_FLAGS);
            }
            else
            {
                //VB에서 사용하는 탑모스트 구현
                SetWindowPos(this.Handle, HWND_TOPMOST, 430, 100, 0, 0, TOPMOST_FLAGS);
            }

            //this.Left = 300;
            //this.Top = 50;
            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            //권한 확인
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == true)
            {
                GetMeCroTitle();
            }

            this.Text = mConTrolNm + "-" + this.Text;
            blnFormLoad = true;

            intImage = 0;
            intSelectedImage = 1;
            intImageSaved = 2;
            intSelectedImageSaved = 2;

            treeJobGroup.ImageList = imageList2;
            treeTmpGrp.ImageList = imageList2;

            //>>옵션과별
            string strOptMcro = "";
            strOptMcro = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "EMRSYMP", "OPTDEPT");
            if (strOptMcro == "1")
            {
                rdoDept.Checked = true;
            }
            else if (strOptMcro == "2")
            {
                rdoAll.Checked = true;
            }
            else
            {
                rdoUse.Checked = true;
            }

            //>>옵션증상별
            strOptMcro = clsEmrQuery.EmrGetUserOption(clsDB.DbCon, clsType.User.IdNumber, "EMRSYMP", "OPTSYMP");
            if (strOptMcro == "1")
            {
                tabSymp.SelectedTab = tabM1;
            }
            else if (strOptMcro == "2")
            {
                tabSymp.SelectedTab = tabM1;
            }
            else
            {
                tabSymp.SelectedTab = tabM1;
            }

            blnFormLoad = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            GetMeCroTitle();
        }

        void GetMeCroTitle()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssGRPFORM_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT SYSMPGB, SYSMPINDEX, SYSMPKEY, SYSMPNAME";
                SQL += ComNum.VBLF + " FROM ADMIN.EMRSYSMP";
                SQL += ComNum.VBLF + "          WHERE SYSMPGB = '" + mSYSMPGB + "'";
                SQL += ComNum.VBLF + "          AND SYSMPRMK IS NOT NULL";
                SQL += ComNum.VBLF + "      ORDER BY SYSMPNAME";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, " EMRSYSMP 조회중 문제가 발생했습니다");
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

                ssGRPFORM_Sheet1.RowCount = dt.Rows.Count;
                ssGRPFORM_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    ssGRPFORM_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SYSMPNAME"].ToString().Trim();
                    ssGRPFORM_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SYSMPINDEX"].ToString().Trim();
                    ssGRPFORM_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SYSMPGB"].ToString().Trim();
                    ssGRPFORM_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SYSMPKEY"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int intOption = 0;

            if (chkAdd1.Checked == true)
            {
                intOption = 1;
            }

            rEventMakeText(intOption, txtMacro.Text.Trim());

            //string strText = "";
            //string strMacro = "";
            //string strCapsLock = "";
            //int i = 0;
            //if (chkAdd1.Checked == false)
            //{
            //    rEventMakeText(intOption, txtMacro.Text.Trim());
            //}
            //else
            //{
            //    if(IsKeyLocked(Keys.CapsLock) == true)
            //    {
            //        strCapsLock = "{CAPSLOCK}";
            //    } 
            //    else
            //    {
            //        strCapsLock = "";
            //    }

            //    for(i = 1; i < txtMacro.Text.Length; i++)
            //    {
            //        strText = VB.Mid(txtMacro.Text, i, 1);
            //        switch(strText)
            //        {
            //            case "(":
            //            case "+":
            //            case ")":
            //            case "%":
            //            case "^":
            //            case "~":
            //            case "{":
            //            case "}":
            //                strText = "{" + strText + "}";
            //                break;
            //        }
            //        strMacro = strMacro + strText;
            //    }

            //    strMacro = strMacro.Replace(ComNum.VBLF, "\r");
            //    SendKeys.Send(strCapsLock + strMacro);
            //}
        }

        private void btnAddFirst_Click(object sender, EventArgs e)
        {
            if(txtGrpName.Text.Trim() == "")
            {
                ComFunc.MsgBoxEx(this, "이름을 입력하십시오");
                txtGrpName.Focus();
                return;
            }
            RplChr();
            AddFirstSibling();
        }

        void RplChr()
        {
            txtTmpName.Text = txtTmpName.Text.Trim().Replace("'", "`");
            txtTmpName.Text = txtTmpName.Text.Trim().Replace("^", " ");
            txtTmpName.Text = txtTmpName.Text.Trim().Replace("&", " ");
        }

        void AddFirstSibling()
        {
            try
            {
                string sKey = GetNextKey() + GetSequencesNo("ADMIN.GETSYMPSEQ");
                treeJobGroup.Nodes.Insert(0, sKey, txtGrpName.Text.Trim(), 0, 1);
                SaveAddNode(sKey);
            }
            catch
            {
                ComFunc.MsgBoxEx(this, "추가할 위치의 노드를 선택하십시오.");
            }

            //TreeNode oNodex;
            //try
            //{
            //    string sPKey = "0_";

            //    string strRtnNo = GetSequencesNo("ADMIN.GETSYMPSEQ");
            //    string sKey = GetNextKey() + strRtnNo;

            //    oNodex = treeJobGroup.Nodes.Add(sKey, txtGrpName.Text.Trim(), 0, 1);
            //    SaveAddChildNode(sKey, txtGrpName.Text.Trim(), sPKey);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(new Form() { TopMost = true }, ex.Message);
            //}
        }


        void AddLastSibling()
        {
            string strKey = "";
            //try
            //{
                strKey = GetNextKey() + GetSequencesNo("ADMIN.GETSYMPSEQ");

                treeJobGroup.Nodes.Insert(treeJobGroup.Nodes.Count, strKey, txtGrpName.Text.Trim(), 0, 1);
                //TODO
                //trvSYMPGrp.Nodes.Add , tvwLast, sKey, Trim(txtGrpName.Text), 2, 3
                SaveAddNode(strKey);  
            //}
            //catch
            //{
            //    strKey = GetNextKey() + GetSequencesNo("ADMIN.GETSYMPSEQ");

            //    treeJobGroup.Nodes.Insert(treeJobGroup.Nodes.Count, strKey, txtGrpName.Text.Trim(), 0, 1);
            //    SaveAddNode(strKey);
            //}
        }

        string GetNextKey()
        {
            string rtnVal = "";
            int intHold = 0;

            try
            {

                //TODO
                //
                //
                //
                //iHold = Val(SptChar(trvSYMPGrp.Nodes(1).key, 0, "_"))
                //For i = 1 To trvSYMPGrp.Nodes.Count
                //    If Val(SptChar(trvSYMPGrp.Nodes(i).key, 0, "_")) > iHold Then
                //        iHold = Val(SptChar(trvSYMPGrp.Nodes(i).key, 0, "_"))
                //    End If
                //Next

                intHold = (int) VB.Val(SptChar(treeJobGroup.Nodes[1].Name, 0, "_"));
                for(int i = 0; i < treeJobGroup.Nodes.Count; i++)
                {
                    if(VB.Val(SptChar(treeJobGroup.Nodes[1].Name, 0, "_")) > intHold)
                    {
                        intHold =(int) VB.Val(SptChar(treeJobGroup.Nodes[1].Name, 0, "_"));
                    }
                }
                intHold = intHold + 1;
                rtnVal = intHold.ToString() + "_";
                return rtnVal;
            }
            catch
            {
                rtnVal = "1_";
                return rtnVal;
            }
        }

        string SptChar(string strInNm, int intSeq, string strDelimit)
        {
            string rtnVal = "";
            string[] strArrChr;

            try
            {
                strArrChr = VB.Split(strInNm, strDelimit);

                if(strArrChr.Length < intSeq)
                {
                    rtnVal = "";
                    return rtnVal;
                }
                rtnVal = strArrChr[intSeq];
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        void SaveAddNode(string strKey)
        {
            //'노드가 추가,삭제될 경우 테이블을 연동을 한다
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            //int intIndex = 0;

            //int intFist = 0;
            //int intLast = 0;
            //int intNode = 0;

            //int i = 0;


            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                TreeNode treeNode = GetSearchNode(treeJobGroup.Nodes, strKey);

                string strParentName = "0_";
                if (treeNode.Parent != null)
                {
                    strParentName = treeNode.Parent.Name;
                }

                SQL = "INSERT INTO ADMIN.EMRSYSMP ";
                SQL += ComNum.VBLF + "      (SYSMPGB, SYSMPINDEX,SYSMPKEY,";
                SQL += ComNum.VBLF + "      SYSMPPARENT,SYSMPNAME,SYSMPKEYV,SYSMPPARENTV)";
                SQL += ComNum.VBLF + "  VALUES (";
                SQL += ComNum.VBLF + "  '" + mSYSMPGB + "',";
                //TODO
                //Val(SptChar(trvSYMPGrp.Nodes(iIndex).key, 1, "_"))
                SQL += ComNum.VBLF + "  " + VB.Val(SptChar(treeNode.Name, 1, "_")) + ",";
                SQL += ComNum.VBLF + "  '" + treeNode.Name + "',";
                SQL += ComNum.VBLF + "  '" + strParentName + "',";
                SQL += ComNum.VBLF + "  '" + treeNode.Text + "',";
                //Val(SptChar(trvSYMPGrp.Nodes(iIndex).key, 0, "_"))
                SQL += ComNum.VBLF + "  " + VB.Val(SptChar(treeNode.Name, 0, "_")) + ",";
                SQL += ComNum.VBLF + "  " + VB.Val(SptChar(strParentName, 0, "_")) + ")";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //intFist = treeJobGroup.Nodes[].FirstNode.Index;
                //intNode = treeNode.FirstNode.Index;
                //intFist = treeNode.LastNode.Index;

                //while (intNode != treeNode.Nodes[intNode].LastNode.Index)
                //{
                //    SQL =  "UPDATE ADMIN.EMRSYSMP ";
                //    SQL += ComNum.VBLF + "  SET SYSDSPINDEX = " + i;
                //    //TODO
                //    //strSql = strSql & vbLf & "  WHERE SYSMPINDEX = " & Val(SptChar(trvSYMPGrp.Nodes(intNode).key, 1, "_"))
                //    SQL += ComNum.VBLF + "  WHERE SYSMPINDEX = " + VB.Val(SptChar(treeNode.Nodes[intNode].Name, 1, "_"));

                //    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                //    if (SqlErr != "")
                //    {
                //        clsDB.setRollbackTran(clsDB.DbCon);
                //        ComFunc.MsgBoxEx(this, SqlErr);
                //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                //        Cursor.Current = Cursors.Default;
                //        return;
                //    }

                //    i = i + 1;

                //    intNode = treeNode.Nodes[intNode].NextNode.Index;
                //}

                //SQL = "UPDATE ADMIN.EMRSYSMP ";
                //SQL += ComNum.VBLF + "  SET SYSDSPINDEX = " + i;
                ////TODO
                ////strSql = strSql & vbLf & "  WHERE SYSMPINDEX = " & Val(SptChar(trvSYMPGrp.Nodes(intLast).key, 1, "_"))
                //SQL += ComNum.VBLF + "  WHERE SYSMPINDEX = " + VB.Val(SptChar(treeNode.LastNode.Name, 1, "_"));

                //SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                //if (SqlErr != "")
                //{
                //    clsDB.setRollbackTran(clsDB.DbCon);
                //    ComFunc.MsgBoxEx(this, SqlErr);
                //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                //    Cursor.Current = Cursors.Default;
                //    return;
                //}

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        string GetParentKey(int intIndex)
        {
            string returnVal = "0_";

            try
            {
                //TODO
                //trvSYMPGrp.Nodes(intIndex).Parent.key
                returnVal = treeJobGroup.Nodes[intIndex].Parent.Name;
                return returnVal;
            }
            catch
            {
                returnVal = "0_";
                return returnVal;
            }
        }

        TreeNode GetSearchNode(TreeNodeCollection treeNode, string strKey)
        {
            foreach (TreeNode node in treeNode)
            {
                if (node.Name == strKey)
                {
                    return node;
                }
                TreeNode findNode = GetSearchNode(node.Nodes, strKey);
                if(findNode != null)
                {
                    return findNode;
                }
            }
            return null;
        }
        
        /// <summary>
        /// Sequences Function별 NextVal번호 가져오기
        /// GetSequencesNo("GETSEQPTNO")
        /// Function.NextVal 번호
        /// </summary>
        /// <returns></returns>
        //
        //
        string GetSequencesNo(string FunSeqName)
        {
            string returnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "SELECT " + FunSeqName + "() FunSeqNo FROM Dual";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return returnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return returnVal;
                }

                returnVal = dt.Rows[0]["FunSeqNo"].ToString().Trim();
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
            return returnVal;
        }

        void GetSysmpInfo(string strSYSMPINDEX)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "";
                SQL = " SELECT SYSMPRMK";
                SQL += ComNum.VBLF + " FROM ADMIN.EMRSYSMP";
                SQL += ComNum.VBLF + "      WHERE SYSMPINDEX = " + VB.Val(strSYSMPINDEX);

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, " BDISEASE 조회중 문제가 발생했습니다");
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

                txtSypmt.Text = dt.Rows[0]["SYSMPRMK"].ToString().Trim();

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnAddNext_Click(object sender, EventArgs e)
        {
            if (txtGrpName.Text.Trim() == "")
            {
                ComFunc.MsgBoxEx(this, "이름을 입력하십시오");
                txtGrpName.Focus();
                return;
            }
            RplChr();
            AddNextSibling();
        }

        void AddNextSibling()
        {
            //TODO
            try
            {
                string sKey = GetNextKey() + GetSequencesNo("ADMIN.GETSYMPSEQ");

                //trvSYMPGrp.Nodes.Add iIndex, tvwNext, sKey, Trim(txtGrpName.Text), 2, 3
                treeJobGroup.Nodes.Insert(treeJobGroup.SelectedNode.Index + 1, sKey, txtGrpName.Text.Trim(), 0, 1);
                SaveAddNode(sKey);
            }
            catch
            {
                ComFunc.MsgBoxEx(this, "추가할 위치의 노드를 선택하십시오.");
            }
        }

        private void btnAddChild_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인


            if (txtGrpName.Text.Trim() == "")
            {
                ComFunc.MsgBoxEx(this, "이름을 입력하십시오");
                txtGrpName.Focus();
                return;
            }
            RplChr();
            AddChildNode();
        }

        void AddChildNode()
        {
            try
            {
                if(ExistData() == true)
                {
                    ComFunc.MsgBoxEx(this, "등록된 증상이 존재합니다." + ComNum.VBLF + "하위그룹(자식)을 추가할 수 없습니다.");
                    return;
                }

                string sKey = GetNextKey() + GetSequencesNo("ADMIN.GETSYMPSEQ");

                TreeNode TN = treeJobGroup.SelectedNode.Nodes.Add(sKey, txtGrpName.Text.Trim(), 0, 1);
                //TODO Set oNodex = trvSYMPGrp.Nodes.Add(iIndex, tvwChild, sKey, Trim(txtGrpName.Text), 2, 3)
                SaveAddNode(sKey);
                TN.EnsureVisible();
            }
            catch
            {
                ComFunc.MsgBoxEx(this, "추가할 위치의 노드를 선택하십시오.");
            }
        }

        bool ExistData()
        {
            bool rtnVal = false;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "    SELECT * FROM ADMIN.EMRSYSMP";
                SQL += ComNum.VBLF + "          WHERE SYSMPGB = '" + mSYSMPGB + "'";
                SQL += ComNum.VBLF + "              AND SYSMPINDEX = " + VB.Val(SptChar(treeJobGroup.SelectedNode.Name, 1, "_"));
                //TODO
                //lngSYMPINDEX = Val(SptChar(trvSYMPGrp.Nodes.Item(iIndex).key, 1, "_"))

                SQL += ComNum.VBLF + "              AND SYSMPRMK IS NOT NULL";
                SQL += ComNum.VBLF + "";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                rtnVal = true;
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인


            if (txtGrpName.Text.Trim() == "")
            {
                ComFunc.MsgBoxEx(this, "이름을 입력하십시오");
                txtGrpName.Focus();
                return;
            }
            RplChr();

            if(UpdateNodeName(txtGrpName.Text.Trim()) == false)
            {
                ComFunc.MsgBoxEx(this, "명칭변경중 에러가 발생했습니다.");
                return;
            }

            treeJobGroup.SelectedNode.Text = txtGrpName.Text.Trim();

            GetMeCroTitle();
        }

        bool UpdateNodeName(string strSYMPNAME)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            long lngSYMPINDEX = (long) VB.Val(SptChar(treeJobGroup.SelectedNode.Name, 1, "_"));

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "    UPDATE ADMIN.EMRSYSMP SET SYSMPNAME = '" + strSYMPNAME + "'";
                SQL += ComNum.VBLF + "          WHERE SYSMPGB = '" + mSYSMPGB + "'";
                SQL += ComNum.VBLF + "              AND SYSMPINDEX = " + lngSYMPINDEX;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnAddLast_Click(object sender, EventArgs e)
        {
            if (txtGrpName.Text.Trim() == "")
            {
                ComFunc.MsgBoxEx(this, "이름을 입력하십시오");
                txtGrpName.Focus();
                return;
            }
            RplChr();
            AddLastSibling();
        }

        private void btnAddPrev_Click(object sender, EventArgs e)
        {
            if (txtGrpName.Text.Trim() == "")
            {
                ComFunc.MsgBoxEx(this, "이름을 입력하십시오");
                txtGrpName.Focus();
                return;
            }
            RplChr();
            AddPrevSibling();
        }

        void AddPrevSibling()
        {
            try
            {
                string sKey = GetNextKey() + GetSequencesNo("ADMIN.GETSYMPSEQ");
                //TODO
                //trvSYMPGrp.Nodes.Add iIndex, tvwPrevious, sKey, Trim(txtGrpName.Text), 2, 3
                treeJobGroup.Nodes.Insert(treeJobGroup.SelectedNode.Index - 1, sKey, txtGrpName.Text.Trim(), 0, 1);
                treeJobGroup.SelectedNode.PrevNode.Nodes.Add(sKey, txtGrpName.Text.Trim(), 0, 1);
                SaveAddNode(sKey);
            }
            catch
            {
                ComFunc.MsgBoxEx(this, "추가할 위치의 노드를 선택하십시오.");
            }
        }

        private void btnDelNode_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            DelNode();
        }

        void DelNode()
        {
            try
            {

                if(treeJobGroup.SelectedNode.GetNodeCount(true) > 0)
                {
                    ComFunc.MsgBoxEx(this, "자식이 존재합니다." + ComNum.VBLF + "자식을 삭제후 다시 삭제하십시오.");
                    return;
                }

                if(ExistData() == true)
                {
                    if(ComFunc.MsgBoxQ("등록된 증상이 존재합니다." + ComNum.VBLF + "삭제하시겠습니까?") == DialogResult.No)
                    {
                        return;
                    }
                }

                if(DelNodeData() == false)
                {
                    ComFunc.MsgBoxEx(this, "삭제중 에러가 발생했습니다.");
                    return;
                }

                TreeNode oNodex;
                oNodex = treeJobGroup.SelectedNode;

                oNodex.Remove();
                //treeJobGroup.Nodes.RemoveAt(treeJobGroup.SelectedNode.Index);
                txtGrpName.Text = "";
                GetMeCroTitle();
            }
            catch
            {
            }
        }

        bool DelNodeData()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;

            //TODO
            //  lngSYMPINDEX = Val(SptChar(trvSYMPGrp.Nodes.Item(iIndex).key, 1, "_"))
            long lngSYMPINDEX = (long)VB.Val(SptChar(treeJobGroup.SelectedNode.Name, 1, "_"));

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "    DELETE FROM ADMIN.EMRSYSMP";
                SQL += ComNum.VBLF + "          WHERE SYSMPGB = '" + mSYSMPGB + "'";
                SQL += ComNum.VBLF + "              AND SYSMPINDEX = " + lngSYMPINDEX;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnSaveRmk_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인


            if (txtSypmt.Text.Trim() == "")
            {
                ComFunc.MsgBoxEx(this, "증상을 입력하십시오");
                txtSypmt.Focus();
                return;
            }

            if(treeJobGroup.SelectedNode.GetNodeCount(true) > 0)
            {
                ComFunc.MsgBoxEx(this, "하위 그룹이 존재함으로 증상 상용구를 등록할 수 없습니다.");
                return;
            }

            if (UpdateRmk(txtSypmt.Text.Trim()) == false)
            {
                ComFunc.MsgBoxEx(this, "저장중 에러가 발생했습니다.");
                return;
            }

            treeJobGroup.SelectedNode.ImageIndex = intImageSaved;
            treeJobGroup.SelectedNode.SelectedImageIndex = intSelectedImageSaved;
            treeJobGroup.Refresh();

            GetMeCroTitle();
        }

        bool UpdateRmk(string strSYMPNAME)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            long lngSYMPINDEX = (long)VB.Val(SptChar(treeJobGroup.SelectedNode.Name, 1, "_"));

            strSYMPNAME = strSYMPNAME.Replace("'", "`");

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "    UPDATE ADMIN.EMRSYSMP SET SYSMPRMK = '" + strSYMPNAME + "'";
                SQL += ComNum.VBLF + "          WHERE SYSMPGB = '" + mSYSMPGB + "'";
                SQL += ComNum.VBLF + "              AND SYSMPINDEX = " + lngSYMPINDEX;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void ssGRPFORM_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssGRPFORM_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssGRPFORM, e.Column);
                return;
            }

            ssGRPFORM_Sheet1.Cells[0, 0, ssGRPFORM_Sheet1.RowCount - 1, ssGRPFORM_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssGRPFORM_Sheet1.Cells[e.Row, 0, e.Row, ssGRPFORM_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            GetMeCro(ssGRPFORM_Sheet1.Cells[e.Row, 1].Text.Trim());
        }


        private void ssGRPFORM_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssGRPFORM_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssGRPFORM, e.Column);
                return;
            }

            ssGRPFORM_Sheet1.Cells[0, 0, ssGRPFORM_Sheet1.RowCount - 1, ssGRPFORM_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssGRPFORM_Sheet1.Cells[e.Row, 0, e.Row, ssGRPFORM_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            GetMeCro(ssGRPFORM_Sheet1.Cells[e.Row, 1].Text.Trim());

            int intOption = 0;

            if (chkAdd1.Checked == true)
            {
                intOption = 1;
            }

            rEventMakeText(intOption, txtMacro.Text.Trim());
        }

        void GetMeCro(string strSYSMPINDEX)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "";
                SQL = " SELECT SYSMPRMK";
                SQL += ComNum.VBLF + " FROM ADMIN.EMRSYSMP";
                SQL += ComNum.VBLF + "      WHERE SYSMPINDEX = " + VB.Val(strSYSMPINDEX);

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, " BDISEASE 조회중 문제가 발생했습니다.");
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

                txtMacro.Text = dt.Rows[0]["SYSMPRMK"].ToString().Trim();

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            int intOption = 0;

            if (chkAdd2.Checked == true)
            {
                intOption = 1;
            }

            rEventMakeText(intOption, txtSypmt.Text.Trim());

            //string strText = "";
            //string strMacro = "";
            //string strCapsLock = "";
            //int i = 0;
            //if (chkAdd1.Checked == false)
            //{
            //    rEventMakeText(intOption, txtMacro.Text.Trim());
            //}
            //else
            //{
            //    if(IsKeyLocked(Keys.CapsLock) == true)
            //    {
            //        strCapsLock = "{CAPSLOCK}";
            //    } 
            //    else
            //    {
            //        strCapsLock = "";
            //    }

            //    for(i = 1; i < txtMacro.Text.Length; i++)
            //    {
            //        strText = VB.Mid(txtMacro.Text, i, 1);
            //        switch(strText)
            //        {
            //            case "(":
            //            case "+":
            //            case ")":
            //            case "%":
            //            case "^":
            //            case "~":
            //            case "{":
            //            case "}":
            //                strText = "{" + strText + "}";
            //                break;
            //        }
            //        strMacro = strMacro + strText;
            //    }

            //    strMacro = strMacro.Replace(ComNum.VBLF, "\r");
            //    SendKeys.Send(strCapsLock + strMacro);
            //}
        }

        private void btnTitle_Click(object sender, EventArgs e)
        {
            string strCaption = ((Button)sender).Text;
            int intOption = 0;

            if (chkAdd3.Checked == true)
            {
                intOption = 1;
            }

            rEventMakeText(intOption, strCaption);
        }

        private void button25_Click(object sender, EventArgs e)
        {
            if (ssTmp_Sheet1.ActiveRowIndex < 0) return;
            ssTmp_Sheet1.ActiveRow.Remove();
        }

        private void button27_Click(object sender, EventArgs e)
        {
            ssTmp_Sheet1.RowCount = ssTmp_Sheet1.RowCount + 1;
            ssTmp_Sheet1.SetRowHeight(ssTmp_Sheet1.RowCount - 1, ComNum.SPDROWHT);
            ssTmp_Sheet1.Cells[ssTmp_Sheet1.RowCount - 1, 2].Text = ssTmp_Sheet1.RowCount.ToString();
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            int intOption = 0;

            if (chkAdd3.Checked == true)
            {
                intOption = 1;
            }

            rEventMakeText(intOption, "\r\n");
        }

        private void btnSpace_Click(object sender, EventArgs e)
        {
            int intOption = 0;

            if (chkAdd3.Checked == true)
            {
                intOption = 1;
            }

            rEventMakeText(intOption, " ");
        }

        private void btnTmpSaveRmk_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            if (ssTmp_Sheet1.RowCount == 0)
            {
                ComFunc.MsgBoxEx(this, "증상을 입력하십시오");
                return;
            }

            int intIndex = treeTmpGrp.SelectedNode.Index;
            if(treeTmpGrp.SelectedNode.GetNodeCount(true) > 0)
            {
                ComFunc.MsgBoxEx(this, "하위 그룹이 존재함으로 증상 상용구를 등록할 수 없습니다.");
                return;
            }

            if(SaveSympT(intIndex) == false)
            {
                ComFunc.MsgBoxEx(this, "저장중 에러가 발생했습니다.");
                return;
            }

            //treeTmpGrp.SelectedNode.ImageIndex = intImageSaved;
            //treeTmpGrp.SelectedNode.SelectedImageIndex = intSelectedImageSaved;
        }

        bool SaveSympT(int intIndex)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            
            long lngSYMPINDEX = (long)VB.Val(SptChar(treeTmpGrp.SelectedNode.Name, 1, "_"));

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //'먼저 지우고 시작한다
                SQL = "";
                SQL = "    DELETE FROM ADMIN.EMRSYSMPTRMK";
                SQL += ComNum.VBLF + "          WHERE SYSMPGB = '" + mSYSMPGB + "'";
                SQL += ComNum.VBLF + "          AND SYSMPINDEX = " + lngSYMPINDEX;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for(int i = 0; i < ssTmp_Sheet1.NonEmptyRowCount; i++)
                {
                    if(ssTmp_Sheet1.Cells[i, 1].Text.Trim() != "")
                    {
                        SQL = "";
                        SQL = " INSERT INTO ADMIN.EMRSYSMPTRMK ";
                        SQL += ComNum.VBLF + "  (SYSMPGB,SYSMPINDEX,SYSMPKEY,SYSMPDSP,";
                        SQL += ComNum.VBLF + "  SYSMPRMK) VALUES (";
                        SQL += ComNum.VBLF + "  '" + mSYSMPGB + "',";
                        SQL += ComNum.VBLF + "  " + lngSYMPINDEX + ",";
                        SQL += ComNum.VBLF + "  " + i + ",";
                        SQL += ComNum.VBLF + "  " + VB.Val(ssTmp_Sheet1.Cells[i, 2].Text) + ",";
                        SQL += ComNum.VBLF + " '" + RplChrSp(ssTmp_Sheet1.Cells[i, 1].Text) + "')";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, "저장하였습니다.");
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

        }

        string RplChrSp(string strChr)
        {
            string rtnVal = strChr.Trim();
            rtnVal = strChr.Replace("'", "`");
            rtnVal = strChr.Replace("^", " ");
            rtnVal = strChr.Replace("&", " ");
            return rtnVal;
        }

        private void ssTmp_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (ssTmp_Sheet1.RowCount == 0) return;

            if (e.Row < 0) return;
            if (e.Column != 0) return;

            string strText = ssTmp_Sheet1.Cells[e.Row, 1].Text.Trim();
            int intOption = 0;

            if (chkAdd3.Checked == true)
            {
                intOption = 1;
            }

            rEventMakeText(intOption, strText);

            //string strMacro = "";
            //string strCapsLock = "";

            //if(ssTmp_Sheet1.Cells[e.Row, 1].Text.Trim() != "" )
            //{
            //    if (chkTmp.Checked == false)
            //    {
            //        //TODO
            //        //RaiseEvent MakeTextProgress(Val(chkTmp.Value), .Text)
            //    }
            //    else
            //    {
            //        if (IsKeyLocked(Keys.CapsLock) == true)
            //        {
            //            strCapsLock = "{CAPSLOCK}";
            //        }
            //        else
            //        {
            //            strCapsLock = "";
            //        }

            //        for (int i = 1; i < txtMacro.Text.Length; i++)
            //        {
            //            strText = VB.Mid(txtMacro.Text, i, 1);
            //            switch (strText)
            //            {
            //                case "(":
            //                case "+":
            //                case ")":
            //                case "%":
            //                case "^":
            //                case "~":
            //                case "{":
            //                case "}":
            //                    strText = "{" + strText + "}";
            //                    break;
            //            }
            //            strMacro = strMacro + strText;
            //        }


            //        strMacro = strMacro.Replace(ComNum.VBLF, "\r");
            //        SendKeys.Send(strCapsLock + strMacro);
            //        //TODO
            //        //Call cvtToEng(CallForm.txtProgress)
            //        //CallForm.txtProgress.SetFocus
            //        SendKeys.Send(strCapsLock + strMacro);
            //    }
            //}
        }

        private void rdoAll_CheckedChanged(object sender, EventArgs e)
        {
            this.Enabled = false;
            //mOption = "M";
            mSYSMPGB = "ALL";
            LoadTable();
            LoadTableTmp();
            GetMeCroTitle();
            this.Enabled = true;
        }

        private void rdoDept_CheckedChanged(object sender, EventArgs e)
        {
            this.Enabled = false;
            //mOption = "D";
            mSYSMPGB = clsType.User.DeptCode;
            LoadTable();
            LoadTableTmp();
            GetMeCroTitle();
            this.Enabled = true;
        }

        private void rdoUse_CheckedChanged(object sender, EventArgs e)
        {
            this.Enabled = false;
            //mOption = "U";
            mSYSMPGB = clsType.User.DrCode;
            LoadTable();
            LoadTableTmp();
            GetMeCroTitle();
            this.Enabled = true;
        }

        void LoadTable()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            treeJobGroup.Nodes.Clear();

            string strSYSMPRMK = "";
            string strSYSMPPARENT = "";

            string strSYSMPKEY = "";
            string strSYSMPNAME = "";

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "";
                SQL = "    SELECT A.SYSMPGB, A.SYSMPINDEX, A.SYSMPKEY, A.SYSMPPARENT, A.SYSMPNAME, A.SYSMPRMK";
                SQL += ComNum.VBLF + "           FROM ADMIN.EMRSYSMP A";
                SQL += ComNum.VBLF + "          WHERE A.SYSMPGB = '" + mSYSMPGB + "'";
                //'    strSql = strSql & vbLf & "          ORDER BY A.SYSMPPARENTV, A.SYSMPKEYV"
                SQL += ComNum.VBLF + "          ORDER BY A.SYSMPPARENTV, A.SYSDSPINDEX";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "EMRSYSMP 조회중 문제가 발생했습니다");
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

                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    strSYSMPPARENT = dt.Rows[i]["SYSMPPARENT"].ToString().Trim();
                    strSYSMPRMK = dt.Rows[i]["SYSMPRMK"].ToString().Trim();
                    strSYSMPKEY = dt.Rows[i]["SYSMPKEY"].ToString().Trim();
                    strSYSMPNAME = dt.Rows[i]["SYSMPNAME"].ToString().Trim();

                    if (strSYSMPPARENT == "0_")
                    {
                        if (strSYSMPRMK == "")
                        {
                            treeJobGroup.Nodes.Add(strSYSMPKEY, strSYSMPNAME, intImage, intSelectedImage);
                        }
                        else
                        {
                            treeJobGroup.Nodes.Add(strSYSMPKEY, strSYSMPNAME, intImageSaved, intSelectedImageSaved);
                        }
                    }
                    else
                    {
                        if (strSYSMPRMK == "")
                        {
                            if (treeJobGroup.Nodes.ContainsKey(strSYSMPPARENT) == true)
                            {
                                treeJobGroup.Nodes[strSYSMPPARENT]
                                .Nodes
                                .Add(strSYSMPKEY, strSYSMPNAME, intImage, intSelectedImage);
                            }
                            else
                            {
                                Set_Child_TreeView(strSYSMPPARENT, strSYSMPPARENT, strSYSMPKEY, strSYSMPNAME, treeJobGroup);
                            }
                        }
                        else
                        {
                            if (treeJobGroup.Nodes.ContainsKey(strSYSMPPARENT) == true)
                            {
                                treeJobGroup.Nodes[strSYSMPPARENT]
                                .Nodes
                                .Add(strSYSMPKEY, strSYSMPNAME, intImageSaved, intSelectedImageSaved);
                            }
                            else
                            {
                                Set_Child_TreeView(strSYSMPPARENT, strSYSMPPARENT, strSYSMPKEY, strSYSMPNAME, treeJobGroup);
                            }
                        }
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        void Set_Child_TreeView(string strSYSMPRMK, string strSYSMPPARENT, string strSYSMPKEY, string strSYSMPNAME, TreeView treeView)
        {
            TreeNode treeNode = GetSearchNode(treeView.Nodes, strSYSMPPARENT);
            if (treeNode == null) return;
            if (strSYSMPRMK == "")
            {
                treeNode.Nodes.Add(strSYSMPKEY, strSYSMPNAME, intImage, intSelectedImage);
            }
            else
            {
                treeNode.Nodes.Add(strSYSMPKEY, strSYSMPNAME, intImageSaved, intSelectedImageSaved);
            }
        }



         void LoadTableTmp()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            treeTmpGrp.Nodes.Clear();
            ssTmp_Sheet1.RowCount = 0;

            string strSYSMPPARENT = "";
            string strSYSMPRMK = "";
            string strSYSMPKEY = "";
            string strSYSMPNAME = "";

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "";
                SQL = "    SELECT A.SYSMPGB, A.SYSMPINDEX, A.SYSMPKEY, A.SYSMPPARENT, A.SYSMPNAME, A.SYSMPRMK";
                SQL += ComNum.VBLF + "           FROM ADMIN.EMRSYSMPT A";
                SQL += ComNum.VBLF + "          WHERE A.SYSMPGB = '" + mSYSMPGB + "'";
                //'    strSql = strSql & vbLf & "          ORDER BY A.SYSMPPARENTV, A.SYSMPKEYV"
                //'    strSql = strSql & vbLf & "          ORDER BY A.SYSMPPARENTV, A.SYSDSPINDEX"
                SQL += ComNum.VBLF + "          ORDER BY A.SYSMPPARENTV, A.SYSMPKEY,A.SYSDSPINDEX";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "  EMRSYSMP 조회중 문제가 발생했습니다");
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

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strSYSMPPARENT = dt.Rows[i]["SYSMPPARENT"].ToString().Trim();
                    strSYSMPRMK = dt.Rows[i]["SYSMPRMK"].ToString().Trim();
                    strSYSMPKEY = dt.Rows[i]["SYSMPKEY"].ToString().Trim();
                    strSYSMPNAME = dt.Rows[i]["SYSMPNAME"].ToString().Trim();

                    if (strSYSMPPARENT == "0_")
                    {
                        if (strSYSMPRMK == "")
                        {
                            treeTmpGrp.Nodes.Add(strSYSMPKEY, strSYSMPNAME, intImage, intSelectedImage);
                        }
                        else
                        {
                            treeTmpGrp.Nodes.Add(strSYSMPKEY, strSYSMPNAME, intImageSaved, intSelectedImageSaved);
                        }
                    }
                    else
                    {
                        if (strSYSMPRMK == "")
                        {
                            if (treeTmpGrp.Nodes.ContainsKey(strSYSMPPARENT) == true)
                            {
                                treeTmpGrp.Nodes[strSYSMPPARENT]
                                .Nodes
                                .Add(strSYSMPKEY, strSYSMPNAME, intImage, intSelectedImage);
                            }
                            else
                            {
                                Set_Child_TreeView(strSYSMPPARENT, strSYSMPPARENT, strSYSMPKEY, strSYSMPNAME, treeTmpGrp);
                            }
                        }
                        else
                        {
                            if (treeTmpGrp.Nodes.ContainsKey(strSYSMPPARENT) == true)
                            {
                                treeTmpGrp.Nodes[strSYSMPPARENT]
                                .Nodes
                                .Add(strSYSMPKEY, strSYSMPNAME, intImageSaved, intSelectedImageSaved);
                            }
                            else
                            {
                                Set_Child_TreeView(strSYSMPPARENT, strSYSMPPARENT, strSYSMPKEY, strSYSMPNAME, treeTmpGrp);
                            }
                        }
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void treeJobGroup_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                txtGrpName.Text = "";
                txtSypmt.Text = "";
                txtGrpName.Text = e.Node.Text;


                if (e.Node.GetNodeCount(true) > 0)
                {
                    return;
                }

                string strSYSMPINDEX = SptChar(e.Node.Name, 1, "_").Trim();

                GetSysmpInfo(strSYSMPINDEX);
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        void GetSysmpInfoTmp(string strSYSMPINDEX)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "";
                SQL = " SELECT SYSMPGB, SYSMPINDEX, SYSMPKEY, SYSMPDSP, SYSMPRMK";
                SQL += ComNum.VBLF + " FROM ADMIN.EMRSYSMPTRMK";
                SQL += ComNum.VBLF + "      WHERE SYSMPINDEX = " + VB.Val(strSYSMPINDEX);
                SQL += ComNum.VBLF + "      ORDER BY SYSMPDSP";

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

                ssTmp_Sheet1.RowCount = dt.Rows.Count;
                ssTmp_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    ssTmp_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SYSMPRMK"].ToString();
                    ssTmp_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SYSMPDSP"].ToString().Trim();
                    ssTmp_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SYSMPKEY"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void treeJobGroup_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {

            try
            {
                txtGrpName.Text = "";
                txtSypmt.Text = "";
                txtGrpName.Text = e.Node.Text;


                if (e.Node.GetNodeCount(true) > 0)
                {
                    return;
                }

                string strSYSMPINDEX = SptChar(e.Node.Name, 1, "_").Trim();

                GetSysmpInfo(strSYSMPINDEX);

                int intOption = 0;

                if (chkAdd2.Checked == true)
                {
                    intOption = 1;
                }

                rEventMakeText(intOption, txtSypmt.Text.Trim());
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }


            //string strText = "";
            //string strMacro = "";
            //string strCapsLock = "";

            //int i = 0;

            //if (chkAdd3.Checked == false)
            //{

            //}
            //else
            //{
            //    if (IsKeyLocked(Keys.CapsLock) == true)
            //    {
            //        strCapsLock = "{CAPSLOCK}";
            //    }
            //    else
            //    {
            //        strCapsLock = "";
            //    }

            //    for (i = 1; i < txtSypmt.Text.Length; i++)
            //    {
            //        strText = VB.Mid(txtSypmt.Text, i, 1);
            //        switch (strText)
            //        {
            //            case "(":
            //            case "+":
            //            case ")":
            //            case "%":
            //            case "^":
            //            case "~":
            //            case "{":
            //            case "}":
            //                strText = "{" + strText + "}";
            //                break;
            //        }
            //        strMacro = strMacro + strText;
            //    }

            //    //TODO
            //    //Call cvtToEng(CallForm.txtProgress)
            //    //CallForm.txtProgress.SetFocus
            //    strMacro = strMacro.Replace(ComNum.VBLF, "\r");
            //    SendKeys.Send(strCapsLock + strMacro);
            //}
        }

        private void treeTmpGrp_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

            try
            {
                txtTmpName.Text = "";
                ssTmp_Sheet1.RowCount = 0;
                txtTmpName.Text = e.Node.Text;


                if (e.Node.GetNodeCount(true) > 0)
                {
                    return;
                }

                //Trim(SptChar(trvTmpGrp.Nodes.Item(iIndex).key, 1, "_"))
                string strSYSMPINDEX = SptChar(e.Node.Name, 1, "_").Trim();

                GetSysmpInfoTmp(strSYSMPINDEX);
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        private void treeTmpGrp_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
            //if (rEventClosed != null)
            //{
            //    rEventClosed();
            //}
            //else
            //{
            //    Close();
            //}
        }

        private void btnTmpAddFirst_Click(object sender, EventArgs e)
        {
            if(txtTmpName.Text.Trim() == "")
            {
                ComFunc.MsgBoxEx(this, "이름을 입력하십시오");
                txtTmpName.Focus();
                return;
            }

            RplChr();
            AddFirstSiblingTmp();
        }

        void AddFirstSiblingTmp()
        {
            try
            {
                string sKey = GetNextKeyTmp() + GetSequencesNo("ADMIN.GETSYMPSEQ");

                treeTmpGrp.Nodes.Insert(0, sKey, txtTmpName.Text.Trim(), 0, 1);
                SaveAddNodeTmp(sKey);
                return;
            }
            catch
            {
                ComFunc.MsgBoxEx(this, "추가할 위치의 노드를 선택하십시오");
                return;
            }
        }

        void SaveAddNodeTmp(string strKey)
        {
            //'노드가 추가,삭제될 경우 테이블을 연동을 한다
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            //int intIndex = 0;

            //int intFist = 0;
            //int intLast = 0;
            //int intNode = 0;

            //int i = 0;


            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                TreeNode treeNode = GetSearchNode(treeTmpGrp.Nodes, strKey);

                string strParentName = "0_";
                if (treeNode.Parent != null)
                {
                    strParentName = treeNode.Parent.Name;
                }
                SQL = "INSERT INTO ADMIN.EMRSYSMPT ";
                SQL += ComNum.VBLF + "      (SYSMPGB, SYSMPINDEX,SYSMPKEY,";
                SQL += ComNum.VBLF + "      SYSMPPARENT,SYSMPNAME,SYSMPKEYV,SYSMPPARENTV)";
                SQL += ComNum.VBLF + "  VALUES (";
                SQL += ComNum.VBLF + "  '" + mSYSMPGB + "',";
                //TODO
                //Val(SptChar(trvSYMPGrp.Nodes(iIndex).key, 1, "_"))
                SQL += ComNum.VBLF + "  " + VB.Val(SptChar(treeNode.Name, 1, "_")) + ",";
                SQL += ComNum.VBLF + "  '" + treeNode.Name + "',";
                SQL += ComNum.VBLF + "  '" + strParentName + "',";
                SQL += ComNum.VBLF + "  '" + treeNode.Text + "',";
                //Val(SptChar(trvSYMPGrp.Nodes(iIndex).key, 0, "_"))
                SQL += ComNum.VBLF + "  " + VB.Val(SptChar(treeNode.Name, 0, "_")) + ",";
                SQL += ComNum.VBLF + "  " + VB.Val(SptChar(strParentName, 0, "_")) + ")";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //intFist = treeNode.FirstNode.Index;
                //intNode = intFist;
                //intFist = treeNode.LastNode.Index;

                //i = 1;

                //while (intNode != treeNode.LastNode.Index)
                //{
                //    SQL = "UPDATE ADMIN.EMRSYSMPT ";
                //    SQL += ComNum.VBLF + "  SET SYSDSPINDEX = " + i;
                //    SQL += ComNum.VBLF + "  WHERE SYSMPINDEX = " + VB.Val(SptChar(treeNode.Nodes[intNode].Name, 1, "_"));

                //    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                //    if (SqlErr != "")
                //    {
                //        clsDB.setRollbackTran(clsDB.DbCon);
                //        ComFunc.MsgBoxEx(this, SqlErr);
                //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                //        Cursor.Current = Cursors.Default;
                //        return;
                //    }

                //    i = i + 1;

                //    intNode = treeNode.Nodes[intNode].LastNode.Index;
                //}

                //SQL = "UPDATE ADMIN.EMRSYSMPT ";
                //SQL += ComNum.VBLF + "  SET SYSDSPINDEX = " + i;
                ////TODO
                ////strSql = strSql & vbLf & "  WHERE SYSMPINDEX = " & Val(SptChar(trvSYMPGrp.Nodes(intLast).key, 1, "_"))
                //SQL += ComNum.VBLF + "  WHERE SYSMPINDEX = " + VB.Val(SptChar(treeNode.LastNode.Name, 1, "_"));

                //SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                //if (SqlErr != "")
                //{
                //    clsDB.setRollbackTran(clsDB.DbCon);
                //    ComFunc.MsgBoxEx(this, SqlErr);
                //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                //    Cursor.Current = Cursors.Default;
                //    return;
                //}

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        string GetNextKeyTmp()
        {
            string rtnVal = "";

            try
            {

                int intHold = (int)VB.Val(SptChar(treeTmpGrp.Nodes[1].Name, 0, "_"));
                for(int i = 0; i < treeTmpGrp.Nodes.Count; i++)
                {
                    if(VB.Val(SptChar(treeTmpGrp.Nodes[i].Name, 0, "_")) > intHold) {
                        intHold = (int)VB.Val(SptChar(treeTmpGrp.Nodes[i].Name, 0, "_"));
                    }
                }
                intHold = intHold + 1;
                rtnVal = intHold + "_";
                return rtnVal;
            }
            catch
            {
                rtnVal = "1_";
            }

            return rtnVal;
        }

        private void btnTmpAddNext_Click(object sender, EventArgs e)
        {
            if (txtTmpName.Text.Trim() == "")
            {
                ComFunc.MsgBoxEx(this, "이름을 입력하십시오");
                txtTmpName.Focus();
                return;
            }

            RplChr();
            AddNextSiblingTmp();
        }

        void AddNextSiblingTmp()
        {
            try
            {

                string sKey = GetNextKeyTmp() + GetSequencesNo("ADMIN.GETSYMPSEQ");

                treeTmpGrp.Nodes.Insert(treeTmpGrp.SelectedNode.NextNode.Index, sKey, txtTmpName.Text.Trim(), 0, 1);
                SaveAddNodeTmp(sKey);
                return;
            }
            catch
            {
                ComFunc.MsgBoxEx(this, "추가할 위치의 노드를 선택하십시오");
                return;
            }
        }

        private void btnTmpAddChild_Click(object sender, EventArgs e)
        {
            if (txtTmpName.Text.Trim() == "")
            {
                ComFunc.MsgBoxEx(this, "이름을 입력하십시오");
                txtTmpName.Focus();
                return;
            }

            RplChr();
            AddChildNodeTmp();
        }

        void AddChildNodeTmp()
        {
            try
            {

                if(ExistDataTmp() == true)
                {
                    ComFunc.MsgBoxEx(this, "등록된 증상이 존재합니다" + ComNum.VBLF + "하위그룹(자식)을 추가 할 수 없습니다.");
                    return;
                }

                string sKey = GetNextKey() + GetSequencesNo("ADMIN.GETSYMPSEQ");

                treeTmpGrp.SelectedNode.Nodes.Add(sKey, txtTmpName.Text.Trim(), 0, 1);
                SaveAddNodeTmp(sKey);
                return;
            }
            catch
            {
                ComFunc.MsgBoxEx(this, "추가할 위치의 노드를 선택하십시오");
                return;
            }
        }

        bool ExistDataTmp()
        {
            bool rtnVal = false;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "    SELECT * FROM ADMIN.EMRSYSMPT";
                SQL += ComNum.VBLF + "          WHERE SYSMPGB = '" + mSYSMPGB + "'";
                SQL += ComNum.VBLF + "              AND SYSMPINDEX = " + VB.Val(SptChar(treeTmpGrp.SelectedNode.Name, 1, "_"));
                SQL += ComNum.VBLF + "              AND SYSMPRMK IS NOT NULL";
                SQL += ComNum.VBLF + "";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                rtnVal = true;
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }


        private void btnTmpRename_Click(object sender, EventArgs e)
        {
            if (txtTmpName.Text.Trim() == "")
            {
                ComFunc.MsgBoxEx(this, "이름을 입력하십시오");
                txtTmpName.Focus();
                return;
            }

            RplChr();

            if (UpdateNodeNameTmp(txtTmpName.Text.Trim()) == false)
            {
                ComFunc.MsgBoxEx(this, "명칭변경중 에러가 발생했습니다.");
                return;
            }

            treeTmpGrp.SelectedNode.Text = txtGrpName.Text.Trim();
            
        }

        bool UpdateNodeNameTmp(string strSYMPNAME)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            long lngSYMPINDEX = (long)VB.Val(SptChar(treeTmpGrp.SelectedNode.Name, 1, "_"));

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "    UPDATE ADMIN.EMRSYSMT SET SYSMPNAME = '" + strSYMPNAME + "'";
                SQL += ComNum.VBLF + "          WHERE SYSMPGB = '" + mSYSMPGB + "'";
                SQL += ComNum.VBLF + "              AND SYSMPINDEX = " + lngSYMPINDEX;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnTmpAddLast_Click(object sender, EventArgs e)
        {
            if (txtTmpName.Text.Trim() == "")
            {
                ComFunc.MsgBoxEx(this, "이름을 입력하십시오");
                txtTmpName.Focus();
                return;
            }

            RplChr();
            AddLastSiblingTmp();
        }

        void AddLastSiblingTmp()
        {
            string sKey = GetNextKey() + GetSequencesNo("ADMIN.GETSYMPSEQ");

            try
            {
                treeTmpGrp.Nodes.Insert(treeTmpGrp.Nodes.Count, sKey, txtTmpName.Text.Trim(), 0, 1);
                SaveAddNodeTmp(sKey);
                return;
            }
            catch
            {
                treeTmpGrp.Nodes.Insert(treeTmpGrp.Nodes.Count, sKey, txtTmpName.Text.Trim(), 0, 1);
                SaveAddNodeTmp(sKey);
                return;
            }
        }

        private void btnTmpAddPrev_Click(object sender, EventArgs e)
        {
            if (txtTmpName.Text.Trim() == "")
            {
                ComFunc.MsgBoxEx(this, "이름을 입력하십시오");
                txtTmpName.Focus();
                return;
            }

            RplChr();
            AddPrevSiblingTmp();
        }

        void AddPrevSiblingTmp()
        {
            try
            {

                string sKey = GetNextKeyTmp() + GetSequencesNo("ADMIN.GETSYMPSEQ");

                treeTmpGrp.Nodes.Insert(treeTmpGrp.SelectedNode.PrevNode.Index, sKey, txtTmpName.Text.Trim(), 0, 1);
                SaveAddNodeTmp(sKey);
                return;
            }
            catch
            {
                ComFunc.MsgBoxEx(this, "추가할 위치의 노드를 선택하십시오");
                return;
            }
        }

        private void btnTmpDelNode_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            DelNodeTmp();
        }

        void DelNodeTmp()
        {
            try
            {

                if (treeTmpGrp.SelectedNode.GetNodeCount(true) > 0)
                {
                    ComFunc.MsgBoxEx(this, "자식이 존재합니다." + ComNum.VBLF + "자식을 삭제후 다시 삭제하십시오.");
                    return;
                }

                if (ExistDataTmp() == true)
                {
                    if (ComFunc.MsgBoxQ("등록된 증상이 존재합니다." + ComNum.VBLF + "삭제하시겠습니까?") == DialogResult.No)
                    {
                        return;
                    }
                }

                if (DelNodeDataTmp() == false)
                {
                    ComFunc.MsgBoxEx(this, "삭제중 에러가 발생했습니다.");
                    return;
                }

                TreeNode oNodex;
                oNodex = treeTmpGrp.SelectedNode;

                oNodex.Remove();

                //treeTmpGrp.Nodes.RemoveAt(treeTmpGrp.SelectedNode.Index);
                txtTmpName.Text = "";
                ssTmp_Sheet1.RowCount = 0;

            }
            catch
            {
            }
        }

        bool DelNodeDataTmp()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;

            long lngSYMPINDEX = (long)VB.Val(SptChar(treeTmpGrp.SelectedNode.Name, 1, "_"));

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "    DELETE FROM ADMIN.EMRSYSMPT";
                SQL += ComNum.VBLF + "          WHERE SYSMPGB = '" + mSYSMPGB + "'";
                SQL += ComNum.VBLF + "              AND SYSMPINDEX = " + lngSYMPINDEX;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnSave3_Click(object sender, EventArgs e)
        {
            int intOption = 0;

            if (chkAdd3.Checked == true)
            {
                intOption = 1;
            }

            rEventMakeText(intOption, txtMacro.Text.Trim());

            //string strText = "";
            //string strMacro = "";
            //string strCapsLock = "";
            //int i = 0;
            //if (chkAdd1.Checked == false)
            //{
            //    rEventMakeText(intOption, txtMacro.Text.Trim());
            //}
            //else
            //{
            //    if(IsKeyLocked(Keys.CapsLock) == true)
            //    {
            //        strCapsLock = "{CAPSLOCK}";
            //    } 
            //    else
            //    {
            //        strCapsLock = "";
            //    }

            //    for(i = 1; i < txtMacro.Text.Length; i++)
            //    {
            //        strText = VB.Mid(txtMacro.Text, i, 1);
            //        switch(strText)
            //        {
            //            case "(":
            //            case "+":
            //            case ")":
            //            case "%":
            //            case "^":
            //            case "~":
            //            case "{":
            //            case "}":
            //                strText = "{" + strText + "}";
            //                break;
            //        }
            //        strMacro = strMacro + strText;
            //    }

            //    strMacro = strMacro.Replace(ComNum.VBLF, "\r");
            //    SendKeys.Send(strCapsLock + strMacro);
            //}
        }

        private void tabSymp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (blnFormLoad == true) return;

            string strOPTVALUE = "1";

            if (tabSymp.SelectedTab == tabM1)
            {
                strOPTVALUE = "1";
            }
            else if (tabSymp.SelectedTab == tabM2)
            {
                strOPTVALUE = "2";
            }
            else if (tabSymp.SelectedTab == tabM3)
            {
                strOPTVALUE = "0";
            }

            if (clsEmrQueryOld.EmrSaveUserOption(clsDB.DbCon, "EMRSYMP", "OPTSYMP", strOPTVALUE) == false)
            {

            }
        }

    }
}
