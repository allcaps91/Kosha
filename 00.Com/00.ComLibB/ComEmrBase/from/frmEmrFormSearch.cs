using System;
using System.Data;
using System.Windows.Forms;
using ComBase;

namespace ComEmrBase
{
    public partial class frmEmrFormSearch : Form
    {
        //선택된 폼을 전달한다.
        public delegate void SetWriteForm(EmrForm fWrite);
        public event SetWriteForm rSetWriteForm;

        //폼이 Close될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        private int nImage = 0;
        private int nSelectedImage = 1;
        private int nImageSaved = 2;
        private int nSelectedImageSaved = 3;

        //private string mRvPTNO = "";    //진료화면에서 받은 환자 정보
        //private string mRvACPNO = "";    //진료화면에서 받은 환자 정보
        //private string mstrPTNO = "";   //자체 화면에서 세팅한 환자 정보.

        private string sKeyHead = "^";
        private string mstrDeptCd = "";

        const string NewEmrStartDate = "2020-04-22 07:00";

        public frmEmrFormSearch()
        {
            InitializeComponent();
        }

        private void frmEmrFormSearch_Load(object sender, EventArgs e)
        {
            trvEmrForm.ImageList = this.ImageList2;

            SetDeptCd();

            if (clsType.User.IsNurse.Equals("OK"))
            {
                optAll.Checked = true;
            }
            else
            {
                optUser.Checked = true;
            }

        }

        private void mbtnExit_Click(object sender, EventArgs e)
        {
            rEventClosed();
        }

        private void mbtnExpandAll_Click(object sender, EventArgs e)
        {
            trvEmrForm.ExpandAll(); 
        }

        private void mbtnCollapseAll_Click(object sender, EventArgs e)
        {
            trvEmrForm.CollapseAll();
        }

        private void GetData(string strFlag = "")
        {
            //2016-02-03 권한관리
            if (optAll.Checked == true)
            {
                GetAllSheet(strFlag);
            }
            else
            {
                GetUserForm();
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

            int intX = 0;
            bool blnDept = false;

            for (i = 0; i < cboDept.Items.Count; i++)
            {
                if (cboDept.Items[i].ToString().Trim() == clsType.User.DeptCode)
                {
                    blnDept = true;
                    intX = i;
                }
            }

            if (blnDept == true)
            {
                cboDept.SelectedIndex = intX;
            }
            else
            {
                cboDept.SelectedIndex = 0;
            }

        }

        private void GetAllSheet(string strFlag = "")
        {
            string WardCodes = string.Empty;
            if (VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "") != "")
            {
                WardCodes = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "");
            }

            WardCodes = "85";

            DataTable dt = null;
            string SQL = string.Empty;    //Query문
            string SqlErr = string.Empty; //에러문 받는 변수

            trvEmrForm.Nodes.Clear();

            Cursor.Current = Cursors.WaitCursor;

            //그룹 1 조회
            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "     DISTINCT G.GRPFORMNO, G.GRPFORMNAME, G.GROUPPARENT, G.DEPTH, G.DISPSEQ ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRGRPFORM G";
            if (strFlag == "S" && txtFormName.Text.Trim() != "")
            {
                SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRGRPFORM G1";
                SQL = SQL + ComNum.VBLF + "   ON G1.GROUPPARENT = G.GRPFORMNO";
                SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRFORM F";
                SQL = SQL + ComNum.VBLF + "   ON F.GRPFORMNO = G1.GRPFORMNO";
                SQL = SQL + ComNum.VBLF + "   AND F.FORMNAME LIKE '%" + txtFormName.Text.Trim() + "%'";
                SQL = SQL + ComNum.VBLF + "   AND F.USECHECK  = '1'";
                if (clsCompuInfo.gstrCOMIP.Equals("192.168.28.55") ||
                    clsCompuInfo.gstrCOMIP.Equals("192.168.0.115") ||
                    WardCodes.Equals("85")
                    )
                {
                    //SQL = SQL + ComNum.VBLF + "   AND F.GRPFORMNO <> 1078 -- 전자동의서 제외";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND F.GRPFORMNO NOT IN(1078, 1083) -- 전자동의서, 사회사업실 기록지 제외";
                }
            }
            SQL = SQL + ComNum.VBLF + "WHERE  G.DEPTH = '0'";
            SQL = SQL + ComNum.VBLF + "  AND  G.DELYN = '0'";
            if ((!clsType.User.BuseCode.Equals("077900") && !clsType.User.BuseCode.Equals("077901")))
            {
                SQL = SQL + ComNum.VBLF + "  AND G.GRPFORMNO NOT IN (1082, 1083) -- 사회사업실 기록지 제외";
            }
            SQL = SQL + ComNum.VBLF + "ORDER BY G.DISPSEQ";
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
                Cursor.Current = Cursors.Default;
                return;
            }

            //TreeNode oNodex = null;
            //TreeNode oNodex = new TreeNode();

            int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                //oNodex = trvEmrForm.Nodes.Add(dt.Rows[i]["GRPFORMNO"].ToString().Trim(), dt.Rows[i]["GRPFORMNAME"].ToString().Trim(), nImage, nSelectedImage);
                trvEmrForm.Nodes.Add(dt.Rows[i]["GRPFORMNO"].ToString().Trim(), dt.Rows[i]["GRPFORMNAME"].ToString().Trim(), nImage, nSelectedImage);
            }
            dt.Dispose();
            dt = null;

            //그룹 2 조회
            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "     DISTINCT G1.GRPFORMNO, G1.GRPFORMNAME, G1.GROUPPARENT, G1.DEPTH, G1.DISPSEQ ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRGRPFORM G1";
            if (strFlag == "S" && txtFormName.Text.Trim() != "")
            {
                SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRFORM F";
                SQL = SQL + ComNum.VBLF + "   ON F.GRPFORMNO = G1.GRPFORMNO";
                SQL = SQL + ComNum.VBLF + "   AND F.FORMNAME LIKE '%" + txtFormName.Text.Trim() + "%'";
                SQL = SQL + ComNum.VBLF + "   AND F.USECHECK = '1'";
                if (clsCompuInfo.gstrCOMIP.Equals("192.168.28.55") ||
                    clsCompuInfo.gstrCOMIP.Equals("192.168.0.115") ||
                    WardCodes.Equals("85")
                    )
                {

                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND F.GRPFORMNO NOT IN(1078, 1083) -- 전자동의서, 사회사업실 기록지 제외";
                }
                //SQL = SQL + ComNum.VBLF + "   AND F.GRPFORMNO <> 1078 -- 전자동의서 제외";
            }
            SQL = SQL + ComNum.VBLF + "WHERE  G1.DEPTH = '1'";
            SQL = SQL + ComNum.VBLF + "  AND  G1.DELYN = '0'";
            if ((!clsType.User.BuseCode.Equals("077900") && !clsType.User.BuseCode.Equals("077901")))
            {
                SQL = SQL + ComNum.VBLF + "  AND G1.GRPFORMNO NOT IN (1082, 1083) -- 사회사업실 기록지 제외";
            }
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
                Cursor.Current = Cursors.Default;
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                //oNodex = trvEmrForm.Nodes.Find(dt.Rows[i]["GROUPPARENT"].ToString().Trim(), true)[0].Nodes.Add(dt.Rows[i]["GRPFORMNO"].ToString().Trim(), dt.Rows[i]["GRPFORMNAME"].ToString().Trim(), nImage, nSelectedImage);
                trvEmrForm.Nodes.Find(dt.Rows[i]["GROUPPARENT"].ToString().Trim(), true)[0].Nodes.Add(dt.Rows[i]["GRPFORMNO"].ToString().Trim(), dt.Rows[i]["GRPFORMNAME"].ToString().Trim(), nImage, nSelectedImage);
            }
            dt.Dispose();
            dt = null;

            // 기록지 조회
            SQL = " SELECT ";
            SQL = SQL + ComNum.VBLF + "     DISTINCT F.GRPFORMNO, F.FORMNO, F.UPDATENO, F.FORMNAME, F.FORMNAMEPRINT, F.USECHECK, F.DISPSEQ ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRFORM F";
            SQL = SQL + ComNum.VBLF + "WHERE F.UPDATENO = (SELECT MAX(B.UPDATENO) FROM " + ComNum.DB_EMR + "AEMRFORM B";
            SQL = SQL + ComNum.VBLF + "                                    WHERE B.FORMNO = F.FORMNO AND B.USECHECK = '1')";
            if (strFlag == "S" && txtFormName.Text.Trim() != "")
            {
                SQL = SQL + ComNum.VBLF + "    AND F.FORMNAME LIKE '%" + txtFormName.Text.Trim() + "%'";
            }
            SQL = SQL + ComNum.VBLF + "  AND F.USECHECK = '1'";
            if (clsCompuInfo.gstrCOMIP.Equals("192.168.28.55") ||
                clsCompuInfo.gstrCOMIP.Equals("192.168.0.115") ||
                WardCodes.Equals("85")
                )
            {

            }
            else
            {
                SQL = SQL + ComNum.VBLF + "   AND F.GRPFORMNO NOT IN(1078, 1083) -- 전자동의서, 사회사업실 기록지 제외";
            }
            //SQL = SQL + ComNum.VBLF + "  AND F.GRPFORMNO <> 1078 -- 전자동의서 제외";

            SQL = SQL + ComNum.VBLF + "ORDER BY F.FORMNAME, F.DISPSEQ";
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
                Cursor.Current = Cursors.Default;
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                //oNodex = trvEmrForm.Nodes.Find(dt.Rows[i]["GRPFORMNO"].ToString().Trim(), true)[0].Nodes.Add(dt.Rows[i]["FORMNO"].ToString().Trim() + sKeyHead + dt.Rows[i]["UPDATENO"].ToString().Trim(), dt.Rows[i]["FORMNAME"].ToString().Trim(), nImageSaved, nSelectedImageSaved);
                if (trvEmrForm.Nodes.Find(dt.Rows[i]["GRPFORMNO"].ToString().Trim(), true).Length == 0)
                    continue;

                trvEmrForm.Nodes.Find(dt.Rows[i]["GRPFORMNO"].ToString().Trim(), true)[0].Nodes.Add(dt.Rows[i]["FORMNO"].ToString().Trim() + sKeyHead + dt.Rows[i]["UPDATENO"].ToString().Trim(), dt.Rows[i]["FORMNAME"].ToString().Trim(), nImageSaved, nSelectedImageSaved);
            }
            dt.Dispose();

            Cursor.Current = Cursors.Default;
            if (strFlag == "S" && txtFormName.Text.Trim() != "")
            {
                trvEmrForm.ExpandAll(); 
            }
        }

        private void GetUserForm()
        {
            TreeNode oNodex = new TreeNode();

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            trvEmrForm.Nodes.Clear();

            string strSubSql = "";
            if (optDept.Checked == true)
            {
                if (cboDept.Text.Trim() == "") return;
                mstrDeptCd = cboDept.Text.Trim();
                strSubSql = strSubSql + ComNum.VBLF + "                                                        AND U.GRPGB = 'D' ";
                strSubSql = strSubSql + ComNum.VBLF + "                                                        AND U.USEGB = '" + mstrDeptCd + "' ";
            }
            else if (optUser.Checked == true)
            {
                strSubSql = strSubSql + ComNum.VBLF + "                                                        AND U.GRPGB = 'U' ";
                strSubSql = strSubSql + ComNum.VBLF + "                                                        AND U.USEGB = '" + clsType.User.IdNumber + "' ";
            }

            Cursor.Current = Cursors.WaitCursor;

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
            SQL = SQL + strSubSql;
            SQL = SQL + ComNum.VBLF + "                                                WHERE A.UPDATENO = (SELECT MAX(B.UPDATENO) FROM " + ComNum.DB_EMR + "AEMRFORM B ";
            SQL = SQL + ComNum.VBLF + "                                                                WHERE B.FORMNO = A.FORMNO AND B.USECHECK = '1') ";
            SQL = SQL + ComNum.VBLF + "                                                    AND A.USECHECK = '1'";
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
                oNodex = trvEmrForm.Nodes.Add(dt.Rows[i]["GRPFORMNO"].ToString().Trim(), dt.Rows[i]["GRPFORMNAME"].ToString().Trim(), nImage, nSelectedImage);
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
            SQL = SQL + strSubSql;
            SQL = SQL + ComNum.VBLF + "                        WHERE A.UPDATENO = (SELECT MAX(B.UPDATENO) FROM " + ComNum.DB_EMR + "AEMRFORM B ";
            SQL = SQL + ComNum.VBLF + "                                        WHERE B.FORMNO = A.FORMNO AND B.USECHECK = '1') ";
            SQL = SQL + ComNum.VBLF + "                            AND A.USECHECK = '1'";
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
                oNodex = trvEmrForm.Nodes.Find(dt.Rows[i]["GROUPPARENT"].ToString().Trim(), true)[0].Nodes.Add(dt.Rows[i]["GRPFORMNO"].ToString().Trim(), dt.Rows[i]["GRPFORMNAME"].ToString().Trim(), nImage, nSelectedImage);
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
            SQL = SQL + strSubSql;
            SQL = SQL + ComNum.VBLF + "WHERE A.UPDATENO = (SELECT MAX(B.UPDATENO) FROM " + ComNum.DB_EMR + "AEMRFORM B ";
            SQL = SQL + ComNum.VBLF + "                                        WHERE B.FORMNO = A.FORMNO AND B.USECHECK = '1') ";
            SQL = SQL + ComNum.VBLF + "        AND A.USECHECK = '1'";
            SQL = SQL + ComNum.VBLF + "ORDER BY U.DISPSEQ ";
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
                if (trvEmrForm.Nodes.Find(dt.Rows[i]["GRPFORMNO"].ToString().Trim(), true).Length > 0)
                {
                    trvEmrForm.Nodes.Find(dt.Rows[i]["GRPFORMNO"].ToString().Trim(), true)[0].Nodes.Add(dt.Rows[i]["FORMNO"].ToString().Trim() + sKeyHead + dt.Rows[i]["UPDATENO"].ToString().Trim(), dt.Rows[i]["FORMNAME"].ToString().Trim(), nImageSaved, nSelectedImageSaved);
                }

            }
            dt.Dispose();
            dt = null;
            trvEmrForm.ExpandAll();

            Cursor.Current = Cursors.Default;
        }

        private void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (optDept.Checked == false) return;
            if (cboDept.Text.Trim() == "") return;
            mstrDeptCd = cboDept.Text.Trim();
            GetUserForm();
        }

        private void trvEmrForm_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode Node;

            try
            {
                Node = e.Node;
                if (Node.GetNodeCount(false) > 0)
                {
                    return;
                }

                string strIndex;

                strIndex = Node.Name.ToString();
                if (strIndex.IndexOf(sKeyHead) == -1)
                    return;

                string[] strParams = VB.Split(VB.Trim(strIndex), sKeyHead);
                string strFormNo = strParams[0].ToString();
                string strUpdateNo = strParams[1].ToString();
                
                // 경과기록지, 경과이미지
                if (strFormNo.Equals("963") || strFormNo.Equals("1232"))
                {
                    if (Convert.ToDateTime(clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime) >= Convert.ToDateTime(NewEmrStartDate))
                    {
                        #region // 외래 진료과별 부분오픈
                        //if (clsOrdFunction.GstrGbJob.Equals("OPD") &&
                        //    (!clsType.User.DeptCode.Equals("NP") && !clsType.User.DeptCode.Equals("DM") && !clsType.User.DeptCode.Equals("OG") &&
                        //     !clsType.User.DeptCode.Equals("OS") && !clsType.User.DeptCode.Equals("NS") && !clsType.User.DeptCode.Equals("CS") &&
                        //     !clsType.User.DeptCode.Equals("UR") && !clsType.User.DeptCode.Equals("RM") && !clsType.User.DeptCode.Equals("NE") && !clsType.User.DeptCode.Equals("MI"))
                        //    )
                        //{
                        //    // 구EMR
                        //    strUpdateNo = "1";
                        //}
                        //else
                        //{
                        //    // 신규EMR
                        //    strUpdateNo = "2";
                        //}
                        #endregion
                        
                        // 신규EMR
                        strUpdateNo = "2";
                    }
                    else
                    {
                        // 구EMR
                        strUpdateNo = "1";
                    }
                }

                EmrForm fWrite = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, strFormNo, strUpdateNo);

                rSetWriteForm(fWrite);
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        private void optAll_CheckedChanged(object sender, EventArgs e)
        {
            if (optAll.Checked == true)
            {
                cboDept.Visible = false;
                GetData();
            }
        }

        private void optDept_CheckedChanged(object sender, EventArgs e)
        {
            if (optDept.Checked == true)
            {
                cboDept.Visible = true;
                if (cboDept.Text.Trim() == "") return;
                mstrDeptCd = cboDept.Text.Trim();
                GetData();
            }
        }

        private void optUser_CheckedChanged(object sender, EventArgs e)
        {
            if (optUser.Checked == true)
            {
                cboDept.Visible = false;
                GetData();
            }

        }

        private void mbtnSave_Click(object sender, EventArgs e)
        {
            using (frmEmrUserFormReg frmEmrUserFormRegX = new frmEmrUserFormReg("U", ""))
            {
                frmEmrUserFormRegX.TopMost = true;
                frmEmrUserFormRegX.ShowDialog(this);
            }
            GetData();
        }

        private void mbtnSearch_Click(object sender, EventArgs e)
        {
            GetData("S");
        }

        private void txtFormName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                GetData("S");
            }
        }

    }
}
