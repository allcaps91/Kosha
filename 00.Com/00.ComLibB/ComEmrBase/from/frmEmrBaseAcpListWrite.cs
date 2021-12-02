using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// EMR 작성폼
    /// </summary>
    public partial class frmEmrBaseAcpListWrite : Form
    {
        #region 서식지 관련 이벤트 및 변수
        //선택된 폼을 전달한다.
        public delegate void SetWriteForm(EmrForm fWrite);
        public event SetWriteForm rSetWriteForm;

        //선택된 내원내역을 전달한다.
        public delegate void ViewPatInfo(EmrPatient tAcp);
        public event ViewPatInfo rViewPatInfo;


        //폼이 Close될 경우
        //public delegate void EventClosed();
        //public event EventClosed rEventClosed;

        private int nImage = 0;
        private int nSelectedImage = 1;
        private int nImageSaved = 2;
        private int nSelectedImageSaved = 3;

        private string mRvPTNO  = string.Empty;    //진료화면에서 받은 환자 정보
        private string mRvACPNO = string.Empty;    //진료화면에서 받은 환자 정보
        private string mstrPTNO = string.Empty;   //자체 화면에서 세팅한 환자 정보.

        private string sKeyHead = "^";
        private string mstrDeptCd = string.Empty;
        #endregion

        public frmEmrBaseAcpListWrite()
        {
            InitializeComponent();
        } 

        #region 서식지 관련
        private void MbtnSearch_Click(object sender, EventArgs e)
        {
            GetData("S");
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


        private void GetAllSheet(string strFlag = "")
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

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
                SQL = SQL + ComNum.VBLF + "  AND (";
                SQL = SQL + ComNum.VBLF + "  UPPER(REPLACE(F.FORMNAME, ' ', '')) LIKE '%" + txtFormName.Text.ToUpper().Trim() + "%'";
                SQL = SQL + ComNum.VBLF + "   OR UPPER(F.FORMNAME) LIKE '%" + txtFormName.Text.ToUpper().Trim() + "%'";
                SQL = SQL + ComNum.VBLF + "  )";
            }
            SQL = SQL + ComNum.VBLF + "WHERE  G.DEPTH = '0'";
            SQL = SQL + ComNum.VBLF + "  AND  G.DELYN = '0'";
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

            for (i = 0; i < dt.Rows.Count; i++)
            {
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
                SQL = SQL + ComNum.VBLF + "  AND (";
                SQL = SQL + ComNum.VBLF + "  UPPER(REPLACE(F.FORMNAME, ' ', '')) LIKE '%" + txtFormName.Text.ToUpper().Trim() + "%'";
                SQL = SQL + ComNum.VBLF + "   OR UPPER(F.FORMNAME) LIKE '%" + txtFormName.Text.ToUpper().Trim() + "%'";
                SQL = SQL + ComNum.VBLF + "  )";
            }
            SQL = SQL + ComNum.VBLF + "WHERE  G1.DEPTH = '1'";
            SQL = SQL + ComNum.VBLF + "  AND  G1.DELYN = '0'";
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
                SQL = SQL + ComNum.VBLF + "  AND (";
                SQL = SQL + ComNum.VBLF + "  UPPER(REPLACE(F.FORMNAME, ' ', '')) LIKE '%" + txtFormName.Text.ToUpper().Trim() + "%'";
                SQL = SQL + ComNum.VBLF + "   OR UPPER(F.FORMNAME) LIKE '%" + txtFormName.Text.ToUpper().Trim() + "%'";
                SQL = SQL + ComNum.VBLF + "  )";
            }
            SQL = SQL + ComNum.VBLF + "    AND F.USECHECK = '1'";
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
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            trvEmrForm.Nodes.Clear();

            string strSubSql = "";
            if (optUser.Checked == true)
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
                Cursor.Current = Cursors.Default;
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                trvEmrForm.Nodes.Add(dt.Rows[i]["GRPFORMNO"].ToString().Trim(), dt.Rows[i]["GRPFORMNAME"].ToString().Trim(), nImage, nSelectedImage);
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
                Cursor.Current = Cursors.Default;
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                trvEmrForm.Nodes.Find(dt.Rows[i]["GROUPPARENT"].ToString().Trim(), true)[0].Nodes.Add(dt.Rows[i]["GRPFORMNO"].ToString().Trim(), dt.Rows[i]["GRPFORMNAME"].ToString().Trim(), nImage, nSelectedImage);
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
                Cursor.Current = Cursors.Default;
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                trvEmrForm.Nodes.Find(dt.Rows[i]["GRPFORMNO"].ToString().Trim(), true)[0].Nodes.Add(dt.Rows[i]["FORMNO"].ToString().Trim() + sKeyHead + dt.Rows[i]["UPDATENO"].ToString().Trim(), dt.Rows[i]["FORMNAME"].ToString().Trim(), nImageSaved, nSelectedImageSaved);
            }
            dt.Dispose();
            trvEmrForm.ExpandAll();

            Cursor.Current = Cursors.Default;
        }

        private void MbtnExpandAll_Click(object sender, EventArgs e)
        {
            trvEmrForm.ExpandAll();
        }

        private void MbtnCollapseAll_Click(object sender, EventArgs e)
        {
            trvEmrForm.CollapseAll();
        }

        private void MbtnSave_Click(object sender, EventArgs e)
        {
            using (frmEmrUserFormReg frmEmrUserFormRegX = new frmEmrUserFormReg("U", ""))
            {
                frmEmrUserFormRegX.TopMost = true;
                frmEmrUserFormRegX.ShowDialog(this);
            }

            GetData();
        }

        private void CboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetUserForm();
        }

        private void txtFormName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                GetData("S");
            }
        }

        private void TrvEmrForm_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (AcpEmr == null)
            {
                ComFunc.MsgBoxEx(this, "내원내역을 선택해주세요.");
                return;
            }

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
                string[] strParams = VB.Split(VB.Trim(strIndex), sKeyHead);
                string strFormNo = strParams[0].ToString();
                string strUpdateNo = strParams.Length > 1 ? strParams[1].ToString() : "1";

                EmrForm fWrite = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, strFormNo, strUpdateNo);

                if(fWrite == null)
                {
                    return;
                }

                #region 기록지 입원, 외래 체크

                //폼에 등록한 과와 현재 
                if (fWrite.FmVISITSDEPT.Length > 0 && fWrite.FmVISITSDEPT.Equals(AcpEmr.medDeptCd) == false)
                {
                    ComFunc.MsgBoxEx(this, "현재 기록지는 " + fWrite.FmVISITSDEPT + "환자만 작성이 가능합니다.\r\n지금 챠트로 작성하시려고 하시는 환자는 '" + AcpEmr.medDeptCd + "'과입니다.\r\n해당 환자리스트에서 챠트버튼을 눌러서 작성하시거나,\r\n'EMR작성' 화면에서 맞는 내원내역을 선택후 작성해주세요.");
                    return;
                }

                //기록지 저장은 외래만 가능한데 환자 정보가 외래가 아닐경우
                if (fWrite.FmINOUTCLS == "1" && AcpEmr.inOutCls != "O")
                {
                    ComFunc.MsgBoxEx(this, "현재 기록지는 외래 혹은 응급실 환자만 작성이 가능합니다.\r\n해당 환자리스트에서 챠트버튼을 눌러서 작성하시거나,\r\n'EMR작성' 화면에서 맞는 내원내역을 선택후 작성해주세요.");

                    return;
                }

                //기록지 저장은 입원만 가능한데 환자 정보가 입원이 아닐경우
                if (fWrite.FmINOUTCLS == "2" && AcpEmr.inOutCls != "I")
                {
                    ComFunc.MsgBoxEx(this, "현재 기록지는 입원 환자만 작성이 가능합니다.\r\n입원 환자리스트에서 챠트버튼을 눌러서 작성하시거나,\r\n'EMR작성' 화면에서 맞는 내원내역을 선택후 작성해주세요.");
                    return;
                }
                #endregion

                rSetWriteForm(fWrite);
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }


        #endregion

        #region //폼에서 사용하는 변수

        EmrPatient AcpEmr = null; //외부에서 전달받은 환자 정보

        /// <summary>
        /// 등록번호
        /// </summary>
        string mPTNO = string.Empty;

        /// <summary>
        /// 정신과 열람 여부
        /// </summary>
        bool mViewNpChart = false;

        #endregion //폼에서 사용하는 변수

        #region //이벤트 전달


        public void ClearForm()
        {
            AcpEmr = null;
            mPTNO = "";

            ssViewEmrAcpDept_Sheet1.RowCount = 0;
        }

        public void GetJupHis(string pPTNO)
        {
            mPTNO = pPTNO;
            GetHisDept();
        }

        public void SetPatInfo(EmrPatient patient)
        {
            AcpEmr = patient;
        }

        #endregion

        #region //임시변수
        string GstrView01 = string.Empty;
        string gJinGubun  = string.Empty;
        string gJinState  = string.Empty;
        //bool gDateSET = false;
        private bool FindScanImageYn(string strInOutCls, string strMedFrDate, string strMedDeptCd, string strMedMedDrCd, string strMedEndDate)
        {
            return false;
        }
        #endregion //임시변수

        #region 외부폼 함수

        /// <summary>
        /// 진료 미비찾는용도
        /// </summary>
        /// <param name="strInDate"></param>
        /// <param name="strOutDate"></param>
        public void SetMibiCell(string strInDate, string strOutDate)
        {
            for (int i = 0; i < ssViewEmrAcpDept_Sheet1.RowCount; i++)
            {
                if (ssViewEmrAcpDept_Sheet1.Cells[i, 1].Text.Trim().Replace("-", "") == strInDate &&
                   ssViewEmrAcpDept_Sheet1.Cells[i, 2].Text.Trim().Replace("-", "") == strOutDate)
                {
                    ssViewEmrAcpDept.ShowRow(0, i, FarPoint.Win.Spread.VerticalPosition.Nearest);
                    ssViewEmrAcpDept_Sheet1.SetActiveCell(i, 0);
                    ssViewEmrAcpDeptCellClick(i);
                    return;
                }
            }
        }

        #endregion

        private void FrmEmrBaseAcpListWrite_Load(object sender, EventArgs e)
        {
            optUser.Checked = true;
            trvEmrForm.ImageList = ImageList2;

            mViewNpChart = clsEmrQueryOld.ViewNPChart(clsType.User.Sabun);
            //GbViewFMChart = clsEmrQueryOld.ViewFMChart(clsType.User.Sabun);
        }

        private void BtnSearchEmrDept_Click(object sender, EventArgs e)
        {
            GetHisDept();
        }


        private void GetHisDept()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssViewEmrAcpDept_Sheet1.RowCount = 0;

            if (mPTNO.Trim() == "") return;

            Cursor.Current = Cursors.WaitCursor;

            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            string strCurDate = VB.Left(strCurDateTime, 8);
            string strCurTime = VB.Right(strCurDateTime, 6);


            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "  XX.INOUTCLS, XX.PTNO, XX.PTNAME, XX.SEX, XX.AGE,";
            SQL = SQL + ComNum.VBLF + "  XX.MEDDEPTCD, XX.MEDDRCD, XX.MEDFRDATE, XX.MEDFRTIME, XX.MEDENDDATE, XX.MEDENDTIME, XX.DRNAME, XX.GBSPC, XX.GBSTS,  ";
            SQL = SQL + ComNum.VBLF + "  (SELECT DEPTKORNAME FROM KOSMOS_EMR.VIEWBMEDDEPT WHERE MEDDEPTCD = XX.MEDDEPTCD) AS DEPTKORNAME ";
            SQL = SQL + ComNum.VBLF + "  , CASE WHEN EXISTS (SELECT 1 FROM KOSMOS_OCS.OCS_MCCERTIFI_WONMU_REPRINT WHERE PANO = XX.PTNO AND BDATE = TO_DATE(XX.MEDFRDATE, 'YYYYMMDD') AND DEPTCODE = XX.MEDDEPTCD) THEN 1 END READ_DOCREPRINT";

            SQL = SQL + ComNum.VBLF + "FROM (";

            if (optEmrInOutDeptO.Checked == true || optEmrInOutDeptA.Checked == true)
            {
                //=================================================================
                //2011-06-15 HD외래의 경우 한달 이내의 내역만 조회 요청(의뢰서)
                //=================================================================
                SQL = SQL + ComNum.VBLF + " SELECT  /*+ INDEX(OPD_MASTER INDEX_OM5) */'O' AS INOUTCLS, Pano AS PTNO, SName AS PTNAME, Sex, Age, ";
                SQL = SQL + ComNum.VBLF + "    DeptCode AS MEDDEPTCD, A.DrCode AS MEDDRCD,";
                SQL = SQL + ComNum.VBLF + "    NVL(TO_CHAR(BDATE,'YYYYMMDD'),TO_CHAR(ACTDATE,'YYYYMMDD') )  AS MEDFRDATE, TO_CHAR(JTime,'HH24MI') || '00' AS MEDFRTIME,";
                SQL = SQL + ComNum.VBLF + "    '' AS MEDENDDATE, '' AS MEDENDTIME, B.DRNAME , A.GBSPC , '0' GBSTS ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER A, " + ComNum.DB_PMPA + "BAS_DOCTOR B ";
                SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + mPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "  AND DEPTCODE IN ('HD','RM') ";
                SQL = SQL + ComNum.VBLF + "  AND BDATE >= TO_DATE('" + (VB.DateAdd("D", -30, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                if (gJinGubun == "" || gJinGubun == "2")
                {
                    SQL = SQL + ComNum.VBLF + "    AND A.Jin    IN ('0','1','2','3','4','5','6','7','9','E','F','H','M','L','K','N','I','J','Q','R','S','A','B') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND A.JIN " + (gJinGubun == "1" ? " NOT IN " : " IN ") + "(" + gJinState + ")";
                }
                if (clsEmrPublic.gUserGrade != "SIMSA" || clsEmrPublic.gUserGrade == "")
                {
                    SQL = SQL + ComNum.VBLF + "    AND GBUSE = 'Y'";
                }
                SQL = SQL + ComNum.VBLF + "     AND A.DRCODE = B.DRCODE(+) ";

                SQL = SQL + ComNum.VBLF + " UNION ALL         ";
                SQL = SQL + ComNum.VBLF + " SELECT  /*+ INDEX(OPD_MASTER INDEX_OM5) */'O' AS INOUTCLS, Pano AS PTNO, SName AS PTNAME, Sex, Age, ";
                SQL = SQL + ComNum.VBLF + "    DeptCode AS MEDDEPTCD, A.DrCode AS MEDDRCD,";
                SQL = SQL + ComNum.VBLF + "    NVL(TO_CHAR(BDATE,'YYYYMMDD'),TO_CHAR(ACTDATE,'YYYYMMDD') )  AS MEDFRDATE, TO_CHAR(JTime,'HH24MI') || '00' AS MEDFRTIME,";
                SQL = SQL + ComNum.VBLF + "    '' AS MEDENDDATE, '' AS MEDENDTIME, B.DRNAME , A.GBSPC , '0' GBSTS ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER A, " + ComNum.DB_PMPA + "BAS_DOCTOR B ";
                SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + mPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "  AND DEPTCODE NOT IN ('HD','RM') ";
                if (chkGikan.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "     AND BDATE >= TO_DATE('" + dtpDateDeptS.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "     AND BDATE <= TO_DATE('" + dtpDateDeptE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                }
                SQL = SQL + ComNum.VBLF + "  AND BDATE >= TO_DATE('1900-01-01','YYYY-MM-DD') ";
                if (gJinGubun == "" || gJinGubun == "2")
                {
                    SQL = SQL + ComNum.VBLF + "    AND A.Jin    IN ('0','1','2','3','4','5','6','7','9','D','E','F','H','M','L','K','N','I','J','Q','R','S','A','B') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND A.JIN " + (gJinGubun == "1" ? " NOT IN " : " IN ") + "(" + gJinState + ")";
                }
                if (clsEmrPublic.gUserGrade != "SIMSA" || clsEmrPublic.gUserGrade == "")
                {
                    SQL = SQL + ComNum.VBLF + "    AND GBUSE = 'Y' ";
                }
                SQL = SQL + ComNum.VBLF + "  AND A.DRCODE =B.DRCODE(+)";
            }

            if (optEmrInOutDeptA.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "UNION ALL";
            }

            if (optEmrInOutDeptI.Checked == true || optEmrInOutDeptA.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + " SELECT 'I' AS INOUTCLS, A.Pano AS PTNO,  A.SName AS PTNAME, A.Sex, A.Age, ";
                SQL = SQL + ComNum.VBLF + "    A.DeptCode AS MEDDEPTCD, A.DrCode AS MEDDRCD,";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(A.InDate,'YYYYMMDD') AS MEDFRDATE, '120000' AS MEDFRTIME,";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(A.OutDate,'YYYYMMDD') AS MEDENDDATE, '120000' AS MEDENDTIME, B.DRNAME, A.GBSPC, A.GBSTS   ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A , " + ComNum.DB_PMPA + "BAS_DOCTOR B ";
                SQL = SQL + ComNum.VBLF + "WHERE A.PANO = '" + mPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "AND A.GBSTS <> '9'";
                SQL = SQL + ComNum.VBLF + "    AND A.DRCODE = B.DRCODE(+) ";
                SQL = SQL + ComNum.VBLF + "UNION ALL";
                if (GstrView01 == "1")
                {
                    SQL = SQL + ComNum.VBLF + "SELECT MAX(A.INOUTCLS) AS INOUTCLS, MAX(A.PTNO) AS PTNO, MAX(B.SName) AS PTNAME, MAX(B.SEX) AS SEX, MAX(B.AGE) AS AGE, ";
                    SQL = SQL + ComNum.VBLF + "    MAX(A.MEDDEPTCD) AS MEDDEPTCD, MAX(A.MEDDRCD) AS MEDDRCD, ";
                    SQL = SQL + ComNum.VBLF + "    MAX(A.MEDFRDATE) AS MEDFRDATE, MAX(A.MEDFRTIME) AS MEDFRTIME, ";
                    SQL = SQL + ComNum.VBLF + "    MAX(TO_CHAR(B.OUTDATE,'YYYYMMDD')) AS MEDENDDATE, '' AS MEDENDTIME, B.DRNAME , B.GBSPC, B.GBSTS   ";
                    SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.EMRXMLMST A, ";
                    SQL = SQL + ComNum.VBLF + "    " + ComNum.DB_PMPA + "IPD_NEW_MASTER B, " + ComNum.DB_PMPA + "BAS_DOCTOR C ";
                    SQL = SQL + ComNum.VBLF + "WHERE A.PTNO =  '" + mPTNO + "' ";
                    SQL = SQL + ComNum.VBLF + "AND A.INOUTCLS = 'I' ";
                    if (chkGikan.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     AND B.INDATE >= TO_DATE('" + dtpDateDeptS.Value.ToString("yyyy-MM-dd") + " 00:00','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "     AND B.INDATE <= TO_DATE('" + dtpDateDeptE.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD') ";
                    }
                    SQL = SQL + ComNum.VBLF + "AND B.GBSTS = '9' ";
                    SQL = SQL + ComNum.VBLF + "AND A.PTNO = B.PANO ";
                    SQL = SQL + ComNum.VBLF + "AND A.MEDFRDATE = TO_CHAR(B.INDATE,'YYYYMMDD') ";
                    SQL = SQL + ComNum.VBLF + "AND A.MEDDEPTCD = B.DeptCode ";
                    SQL = SQL + ComNum.VBLF + "UNION ALL ";
                    SQL = SQL + ComNum.VBLF + "SELECT MAX(A.INOUTCLS) AS INOUTCLS, MAX(A.PTNO) AS PTNO, MAX(B.SName) AS PTNAME, MAX(B.SEX) AS SEX, MAX(B.AGE) AS AGE, ";
                    SQL = SQL + ComNum.VBLF + "    MAX(A.MEDDEPTCD) AS MEDDEPTCD, MAX(A.MEDDRCD) AS MEDDRCD, ";
                    SQL = SQL + ComNum.VBLF + "    MAX(A.MEDFRDATE) AS MEDFRDATE, MAX(A.MEDFRTIME) AS MEDFRTIME, ";
                    SQL = SQL + ComNum.VBLF + "    MAX(TO_CHAR(B.OUTDATE,'YYYYMMDD')) AS MEDENDDATE, '' AS MEDENDTIME, B.DRNAME , B.GBSPC, B.GBSTS   ";
                    SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.AEMRCHARTMST A, ";
                    SQL = SQL + ComNum.VBLF + "    " + ComNum.DB_PMPA + "IPD_NEW_MASTER B, " + ComNum.DB_PMPA + "BAS_DOCTOR C ";
                    SQL = SQL + ComNum.VBLF + "WHERE A.PTNO =  '" + mPTNO + "' ";
                    SQL = SQL + ComNum.VBLF + "AND A.INOUTCLS = 'I' ";
                    if (chkGikan.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     AND B.INDATE >= TO_DATE('" + dtpDateDeptS.Value.ToString("yyyy-MM-dd") + " 00:00','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "     AND B.INDATE <= TO_DATE('" + dtpDateDeptE.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD') ";
                    }
                    SQL = SQL + ComNum.VBLF + "AND B.GBSTS = '9' ";
                    SQL = SQL + ComNum.VBLF + "AND A.PTNO = B.PANO ";
                    SQL = SQL + ComNum.VBLF + "AND A.MEDFRDATE = TO_CHAR(B.INDATE,'YYYYMMDD') ";
                    SQL = SQL + ComNum.VBLF + "AND A.MEDDEPTCD = B.DeptCode ";

                }
                else if (GstrView01 == "" || GstrView01 == "0")
                {
                    SQL = SQL + ComNum.VBLF + " SELECT INOUTCLS, PTNO, PTNAME, SEX, AGE, MEDDEPTCD, MEDDRCD, MEDFRDATE, MEDFRTIME, MEDENDDATE, MEDENDTIME, DRNAME, GBSPC, GBSTS";
                    SQL = SQL + ComNum.VBLF + "  FROM (";
                    SQL = SQL + ComNum.VBLF + "SELECT 'I' AS INOUTCLS, PANO AS PTNO, SName AS PTNAME, SEX, AGE,";
                    SQL = SQL + ComNum.VBLF + "    DEPTCODE AS MEDDEPTCD, A.DRCODE AS MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "    TO_CHAR(INDATE,'YYYYMMDD') AS MEDFRDATE, '1200' AS MEDFRTIME,";
                    SQL = SQL + ComNum.VBLF + "    TO_CHAR(OUTDATE,'YYYYMMDD') AS MEDENDDATE, '' AS MEDENDTIME, B.DRNAME, A.GBSPC, A.GBSTS   ";
                    SQL = SQL + ComNum.VBLF + "    From " + ComNum.DB_PMPA + "IPD_NEW_MASTER A, " + ComNum.DB_PMPA + "BAS_DOCTOR B ";
                    SQL = SQL + ComNum.VBLF + "WHERE PANO =  '" + mPTNO + "'";
                    SQL = SQL + ComNum.VBLF + "AND GBSTS = '9'";
                    if (chkGikan.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     AND INDATE >= TO_DATE('" + dtpDateDeptS.Value.ToString("yyyy-MM-dd") + " 00:00','YYYY-MM-DD HH24:MI') ";
                        SQL = SQL + ComNum.VBLF + "     AND INDATE <= TO_DATE('" + dtpDateDeptE.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI') ";
                    }
                    SQL = SQL + ComNum.VBLF + " AND A.DRCODE = B.DRCODE(+) ";
                    SQL = SQL + ComNum.VBLF + " GROUP BY 'I', PANO, SNAME, SEX, AGE, DEPTCODE, A.DRCODE, TO_CHAR(INDATE,'YYYYMMDD'), '1200', TO_CHAR(OUTDATE,'YYYYMMDD'), DRNAME, GBSPC, GBSTS )";
                }
            }
            SQL = SQL + ComNum.VBLF + "UNION ALL ";

            SQL = SQL + ComNum.VBLF + " SELECT A.CLASS AS INOUTCLS, A.PATID AS PTNO,  B.NAME AS PTNAME, B.Sex, 0 AS Age,  ";
            SQL = SQL + ComNum.VBLF + "    A.CLINCODE AS MEDDEPTCD, C.DRCODE AS MEDDRCD, ";
            SQL = SQL + ComNum.VBLF + "    A.INDATE AS MEDFRDATE, '120000' AS MEDFRTIME, ";
            SQL = SQL + ComNum.VBLF + "    A.OUTDATE AS MEDENDDATE, '120000' AS MEDENDTIME, C.DRNAME , '' GBSPC, '0' GBSTS   ";
            SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.EMR_TREATT A, KOSMOS_EMR.EMR_PATIENTT B, " + ComNum.DB_MED + "OCS_DOCTOR C ";
            SQL = SQL + ComNum.VBLF + "WHERE A.PATID = '" + mPTNO + "' ";
            SQL = SQL + ComNum.VBLF + "  AND A.DOCCODE = C.DOCCODE(+) ";
            SQL = SQL + ComNum.VBLF + "AND A.DELDATE IS NULL";
            if (chkGikan.Checked == true )
            {
                SQL = SQL + ComNum.VBLF + "     AND A.INDATE >= '" + dtpDateDeptS.Value.ToString("yyyyMMdd") + "' ";
                SQL = SQL + ComNum.VBLF + "     AND A.INDATE <= '" + dtpDateDeptE.Value.ToString("yyyyMMdd") + "' ";
            }
            SQL = SQL + ComNum.VBLF + "AND A.PATID = B.PATID ";
            SQL = SQL + ComNum.VBLF + "AND ((A.CLINCODE IN ('HD','RM') AND A.INDATE >= '" + (VB.DateAdd("D", -30, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd") + "') OR (A.CLINCODE NOT IN ('HD','RM') AND A.INDATE >= '19000101'))";

            if (optEmrInOutDeptO.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "AND A.CLASS = 'O'";
            }
            else if (optEmrInOutDeptI.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "AND A.CLASS = 'I'";
            }

            SQL = SQL + ComNum.VBLF + "AND (A.CLASS, A.INDATE, A.CLINCODE) NOT IN ( ";
            SQL = SQL + ComNum.VBLF + "            SELECT INOUTCLS, MEDFRDATE, MEDDEPTCD ";
            SQL = SQL + ComNum.VBLF + "            FROM ";
            SQL = SQL + ComNum.VBLF + "            (SELECT /*+ INDEX(OPD_MASTER INDEX_OM5) */ 'O' AS INOUTCLS, NVL(TO_CHAR(BDATE,'YYYYMMDD'),TO_CHAR(ACTDATE,'YYYYMMDD'))  AS MEDFRDATE, ";
            SQL = SQL + ComNum.VBLF + "            DECODE(DRCODE,'1107','RA','1125','RA',DeptCode) AS MEDDEPTCD";
            SQL = SQL + ComNum.VBLF + "            FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
            SQL = SQL + ComNum.VBLF + "            WHERE PANO = '" + mPTNO + "' ";
            SQL = SQL + ComNum.VBLF + "  AND DEPTCODE IN ('HD','RM') ";
            SQL = SQL + ComNum.VBLF + "  AND BDATE >= TO_DATE('" + (VB.DateAdd("D", -30, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

            if (gJinGubun == "" || gJinGubun == "2")
            {
                SQL = SQL + ComNum.VBLF + "    AND Jin    IN ('0','1','2','3','4','5','6','7','9','D','E','F','H','M','L','K','N','I','J','Q','R','S','A','B') ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "    AND Jin    IN ('0','1','2','3','4','5','6','7','8','D','9','E','F','H','M','L','K','N','I','J','Q','R','S','A','B') ";
            }
            if (clsEmrPublic.gUserGrade != "SIMSA" || clsEmrPublic.gUserGrade == "")
            {
                SQL = SQL + ComNum.VBLF + "                AND GBUSE = 'Y' ";
            }
            SQL = SQL + ComNum.VBLF + "        UNION ALL    ";
            SQL = SQL + ComNum.VBLF + "            SELECT /*+ INDEX(OPD_MASTER INDEX_OM5) */ 'O' AS INOUTCLS, NVL(TO_CHAR(BDATE,'YYYYMMDD'),TO_CHAR(ACTDATE,'YYYYMMDD'))  AS MEDFRDATE, ";
            SQL = SQL + ComNum.VBLF + "            DECODE(DRCODE,'1107','RA','1125','RA',DeptCode) AS MEDDEPTCD";
            SQL = SQL + ComNum.VBLF + "            FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
            SQL = SQL + ComNum.VBLF + "            WHERE PANO = '" + mPTNO + "' ";
            SQL = SQL + ComNum.VBLF + "  AND DEPTCODE NOT IN ('HD','RM') ";
            SQL = SQL + ComNum.VBLF + "  AND BDATE >= TO_DATE('1900-01-01','YYYY-MM-DD') ";
            if (chkGikan.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "     AND BDATE >= TO_DATE('" + dtpDateDeptS.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "     AND BDATE <= TO_DATE('" + dtpDateDeptE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            }
            if (gJinGubun == "" || gJinGubun == "2")
            {
                SQL = SQL + ComNum.VBLF + "    AND Jin    IN ('0','1','2','3','4','5','6','7','9','D','E','F','H','M','L','K','N','I','J','Q','R','S','A','B') ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "    AND Jin    IN ('0','1','2','3','4','5','6','7','8','D','9','E','F','H','M','L','K','N','I','J','Q','R','S','A','B') ";
            }
            if (clsEmrPublic.gUserGrade != "SIMSA" || clsEmrPublic.gUserGrade == "")
            {
                SQL = SQL + ComNum.VBLF + "                AND GBUSE = 'Y' ";
            }

            SQL = SQL + ComNum.VBLF + "            UNION ALL ";
            SQL = SQL + ComNum.VBLF + "             SELECT 'I' AS INOUTCLS, TO_CHAR(A2.InDate,'YYYYMMDD') AS MEDFRDATE, DECODE(A2.DRCODE,'1107','RA','1125','RA',A2.DeptCode) AS MEDDEPTCD ";
            SQL = SQL + ComNum.VBLF + "            FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A2  ";
            SQL = SQL + ComNum.VBLF + "            WHERE A2.PANO = '" + mPTNO + "' ";
            SQL = SQL + ComNum.VBLF + "            AND A2.GBSTS <> '9'";
            if (chkGikan.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "        AND A2.INDATE >= TO_DATE('" + dtpDateDeptS.Value.ToString("yyyy-MM-dd") + " 00:00','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "        AND A2.INDATE <= TO_DATE('" + dtpDateDeptE.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI') ";
            }
            SQL = SQL + ComNum.VBLF + "           UNION ALL";
            SQL = SQL + ComNum.VBLF + "            SELECT 'I' AS INOUTCLS, TO_CHAR(A2.InDate,'YYYYMMDD') AS MEDFRDATE, DECODE(A2.DRCODE,'1107','RA','1125','RA',B2.FRDEPT) AS MEDDEPTCD";
            SQL = SQL + ComNum.VBLF + "              FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A2,   " + ComNum.DB_PMPA + "IPD_TRANSFOR B2";
            SQL = SQL + ComNum.VBLF + "              Where A2.PANO = B2.PANO ";
            SQL = SQL + ComNum.VBLF + "                AND A2.PANO = '" + mPTNO + "' ";
            SQL = SQL + ComNum.VBLF + "                AND A2.IPDNO = B2.IPDNO";
            if (chkGikan.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "        AND A2.INDATE >= TO_DATE('" + dtpDateDeptS.Value.ToString("yyyy-MM-dd") + " 00:00','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "        AND A2.INDATE <= TO_DATE('" + dtpDateDeptE.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI') ";
            }
            SQL = SQL + ComNum.VBLF + "             AND A2.GBSTS <> '9')";
            SQL = SQL + ComNum.VBLF + "    )  ";
            SQL = SQL + ComNum.VBLF + ") XX";
            SQL = SQL + ComNum.VBLF + "  WHERE XX.INOUTCLS IS NOT NULL";
            SQL = SQL + ComNum.VBLF + "    ORDER BY XX.INOUTCLS ASC,  XX.MEDFRDATE DESC, XX.MEDDEPTCD";

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
                //ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                Cursor.Current = Cursors.Default;
                return;
            }

            string strMedFrDate   = string.Empty;
            string strMedEndDate  = string.Empty;
            string strMedDEPTCODE = string.Empty;
            int FnCheck = 0;
            string FstrDateCheck  = string.Empty;

            ssViewEmrAcpDept_Sheet1.RowCount = dt.Rows.Count;
            ssViewEmrAcpDept_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                strMedFrDate = ComFunc.FormatStrToDate(dt.Rows[i]["MEDFRDATE"].ToString().Trim(), "D");
                strMedEndDate = "";
                if (dt.Rows[i]["MEDENDDATE"].ToString().Trim() != "")
                {
                    strMedEndDate = ComFunc.FormatStrToDate(dt.Rows[i]["MEDENDDATE"].ToString().Trim(), "D");
                }
                strMedDEPTCODE = dt.Rows[i]["MEDDEPTCD"].ToString().Trim();
                ssViewEmrAcpDept_Sheet1.Cells[i, 0].Text = dt.Rows[i]["INOUTCLS"].ToString().Trim();
                ssViewEmrAcpDept_Sheet1.Cells[i, 1].Text = strMedFrDate;

                if (dt.Rows[i]["INOUTCLS"].ToString().Trim() == "I")
                {
                    if (dt.Rows[i]["GBSTS"].ToString().Trim() == "9" && dt.Rows[i]["MEDENDTIME"].ToString().Trim() == "")
                    {
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].Text = "입원취소";
                    }
                    else
                    {
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].Font = new Font("맑은 고딕", 10, FontStyle.Regular);
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].Text = strMedEndDate;
                    }
                }
                else
                {
                    if (dt.Rows[i]["READ_DOCREPRINT"].ToString().Equals("1"))
                    //if (clsEmrQueryPohangS.READ_DOCREPRINT(clsDB.DbCon, mPTNO, strMedFrDate, strMedDEPTCODE) == true)
                    {
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].Text = "서류재발급";
                    }
                    else
                    {
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                        ssViewEmrAcpDept_Sheet1.Cells[i, 2].Font = new Font("맑은 고딕", 10, FontStyle.Regular);
                    }
                }
                ssViewEmrAcpDept_Sheet1.Cells[i, 3].Text = strMedDEPTCODE;

                if ((dt.Rows[i]["MEDDRCD"].ToString().Trim() == "1107" || dt.Rows[i]["MEDDRCD"].ToString().Trim() == "1125") && dt.Rows[i]["MEDDEPTCD"].ToString().Trim() == "MD")
                {
                    ssViewEmrAcpDept_Sheet1.Cells[i, 4].Text = "류마티스내과";
                }
                else
                {
                    ssViewEmrAcpDept_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DEPTKORNAME"].ToString().Trim();
                }

                ssViewEmrAcpDept_Sheet1.Cells[i, 5].Text = dt.Rows[i]["MEDFRTIME"].ToString().Trim();
                if (dt.Rows[i]["INOUTCLS"].ToString().Trim() == "O")
                {
                    ssViewEmrAcpDept_Sheet1.Cells[i, 6].Text = "";
                }
                else
                {
                    ssViewEmrAcpDept_Sheet1.Cells[i, 6].Text = dt.Rows[i]["MEDFRTIME"].ToString().Trim();
                }
                ssViewEmrAcpDept_Sheet1.Cells[i, 7].Text = dt.Rows[i]["MEDDRCD"].ToString().Trim();
                ssViewEmrAcpDept_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                ssViewEmrAcpDept_Sheet1.Cells[i, 9].Text = strMedEndDate;
                ssViewEmrAcpDept_Sheet1.Cells[i, 10].Text = strMedDEPTCODE;

                if (dt.Rows[i]["GBSPC"].ToString().Trim() == "1")
                {
                    ssViewEmrAcpDept_Sheet1.Cells[i, 0].BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(236)))), ((int)(((byte)(162)))));
                }
                else if (dt.Rows[i]["GBSPC"].ToString().Trim() == "1")
                {
                    if (clsVbfunc.READ_SPECIAL_SERVICE(clsDB.DbCon, mPTNO, strMedFrDate, strMedDEPTCODE, dt.Rows[i]["INOUTCLS"].ToString().Trim()) == true)
                    {
                        ssViewEmrAcpDept_Sheet1.Cells[i, 0].BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(236)))), ((int)(((byte)(162)))));
                    }
                    else
                    {
                        ssViewEmrAcpDept_Sheet1.Cells[i, 0].BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
                    }
                }
                else
                {
                    ssViewEmrAcpDept_Sheet1.Cells[i, 0].BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
                }

                if (clsEmrPublic.gUserGrade == "SIMSA")
                {
                    if (FstrDateCheck != VB.Left(strMedFrDate, 4))
                    {
                        FstrDateCheck = VB.Left(strMedFrDate, 4);
                        FnCheck = FnCheck + 1;
                    }
                    if (FnCheck % 2 == 0)
                    {
                        ssViewEmrAcpDept_Sheet1.Cells[i, 1].BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(232)))), ((int)(((byte)(170)))));
                    }
                }
            }
            dt.Dispose();
            dt = null;

            if (optEmrInOutDeptO.Checked == true)
            {
                ssViewEmrAcpDept_Sheet1.Columns[2].Visible = false;
            }
            if (optEmrInOutDeptI.Checked == true)
            {
                ssViewEmrAcpDept_Sheet1.Columns[2].Visible = true;
            }

            Cursor.Current = Cursors.Default;

        }

        private void ssViewEmrAcpDept_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssViewEmrAcpDept_Sheet1.RowCount == 0) return;

            //퇴원일자가 아닐경우만 정렬
            //19-08-02 추가
            if (e.ColumnHeader == true && e.Column != 2)
            {
                clsSpread.gSpdSortRow(ssViewEmrAcpDept, e.Column);
                return;
            }

            ssViewEmrAcpDeptCellClick(e.Row);
        }

        private void ssViewEmrAcpDeptCellClick(int Row)
        {
            ssViewEmrAcpDept_Sheet1.Cells[0, 0, ssViewEmrAcpDept_Sheet1.RowCount - 1, ssViewEmrAcpDept_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssViewEmrAcpDept_Sheet1.Cells[Row, 0, Row, ssViewEmrAcpDept_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            string strInOutCls   = string.Empty;
            string strMedFrDate  = string.Empty;
            string strMedEndDate = string.Empty;
            string strMedDeptCd  = string.Empty;
            string strMedFrTime  = string.Empty;
            string strMedEndTime = string.Empty;
            string strMedMedDrCd = string.Empty;

            strInOutCls = ssViewEmrAcpDept_Sheet1.Cells[Row, 0].Text.Trim();
            strMedFrDate = ssViewEmrAcpDept_Sheet1.Cells[Row, 1].Text.Trim().Replace("-", "");
            strMedDeptCd = ssViewEmrAcpDept_Sheet1.Cells[Row, 3].Text.Trim();
            strMedEndDate = ssViewEmrAcpDept_Sheet1.Cells[Row, 2].Text.Trim().Replace("-", "");

            if (strMedEndDate == "입원취소")
            {
                strMedEndDate = strMedFrDate;
            }
            else if (strMedEndDate == "서류재발급")
            {
                strMedEndDate = "";
                clsEmrQueryPohangS.READ_DOCREPRINTHIS(clsDB.DbCon, this, mPTNO, strMedFrDate, strMedDeptCd);
            }

            strMedFrTime = ssViewEmrAcpDept_Sheet1.Cells[Row, 5].Text.Trim();
            strMedEndTime = ssViewEmrAcpDept_Sheet1.Cells[Row, 6].Text.Trim();
            strMedMedDrCd = ssViewEmrAcpDept_Sheet1.Cells[Row, 7].Text.Trim();

            if (strInOutCls == "O")
            {
                if (strMedDeptCd == "NP")
                {
                    if (mViewNpChart == false)
                    {
                        ComFunc.MsgBoxEx(this, "조회 권한이 없습니다.");
                        return;
                    }
                }
            }

            //EMR 내원 내역을 담는다
            //With gptEmrPt
            //    .PtPtNo = mPTNO
            //    .PtAcpNo = "0"
            //    .PtInOutCls = strInOutCls
            //    .PtMedFrDate = strMedFrDate
            //    .PtMedFrTime = strMedFrTime
            //    .PtMedEndDate = strMedEndDate
            //    .PtMedEndTime = strMedEndTime
            //    .PtMedDeptCd = strMedDeptCd
            //    .PtMedDrCd = strMedMedDrCd
            //End With

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            if (clsEmrPublic.gUserGrade == "SIMSA")
            {
                if (strInOutCls == "O")
                {
                    SQL = " SELECT SNAME, BI FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + mPTNO + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + strMedFrDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "  AND DEPTCODE = '" + strMedDeptCd + "' ";
                    
                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        //frmTextEmrMain.lblPtNm.Caption = Trim(AdoGetString(RS4, "SNAME", 0)) + "(" & READ_Bi_Name(Trim(AdoGetString(RS4, "BI", 0))) & ")"
                    }
                    dt.Dispose();
                    dt = null;
                }
                else if (strInOutCls == "I")
                {
                    SQL = " SELECT SNAME, BI FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + mPTNO + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND INDATE >= TO_DATE('" + strMedFrDate + " 00:00','YYYY-MM-DD HH24:MI')";
                    SQL = SQL + ComNum.VBLF + "   AND INDATE <= TO_DATE('" + strMedFrDate + " 23:59','YYYY-MM-DD HH24:MI')";
                    SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + strMedDeptCd + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND GBSTS NOT IN ('9')";
                    
                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        //frmTextEmrMain.lblPtNm.Caption = Trim(AdoGetString(RS4, "SNAME", 0)) + "(" & READ_Bi_Name(Trim(AdoGetString(RS4, "BI", 0))) & ")"
                    }
                    dt.Dispose();
                    dt = null;
                }
            }

            AcpEmr = clsEmrChart.ClearPatient();
            AcpEmr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, mPTNO, strInOutCls, strMedFrDate, strMedDeptCd);
            
            if (AcpEmr == null && strMedDeptCd.Equals("TO"))
            {
                clsImgcvt.NEW_PohangTreatInterface(clsDB.DbCon, this, mPTNO);
                AcpEmr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, mPTNO, strInOutCls, strMedFrDate, strMedDeptCd);
            }

            rViewPatInfo(AcpEmr);
        }

        private void optUser_CheckedChanged(object sender, EventArgs e)
        {
            if (optUser.Checked == true)
            {
                GetData();
            }

        }

        private void optAll_CheckedChanged(object sender, EventArgs e)
        {
            if (optAll.Checked == true)
            {
                GetData();
            }
        }

        private void optEmrInOutDeptA_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                if (optEmrInOutDeptO.Checked == true)
                {
                    ssViewEmrAcpDept_Sheet1.Columns[2].Visible = false;
                }

                if (optEmrInOutDeptA.Checked == true || optEmrInOutDeptI.Checked == true)
                {
                    ssViewEmrAcpDept_Sheet1.Columns[2].Visible = true;
                }

                GetHisDept();
            }
        }
    }
}
