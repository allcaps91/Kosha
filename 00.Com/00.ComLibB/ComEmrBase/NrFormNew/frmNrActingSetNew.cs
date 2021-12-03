using System;
using System.Data;
using System.Windows.Forms;
using ComBase;

namespace ComEmrBase
{
    public partial class frmNrActingSetNew : Form
    {
        public EmrPatient p = null;
        string strFormName = "";
        string[] mstrCode = null;
        string[] mstrCodeName = null;

        public frmNrActingSetNew()
        {
            InitializeComponent();
        }

        public frmNrActingSetNew(EmrPatient po, string strFN)
        {
            InitializeComponent();
            p = po;
            strFormName = strFN;
        }

        private void frmNrActingSetNew_Load(object sender, EventArgs e)
        {
            dtpVitalDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));

            cboWard.Items.Clear();

            GetActTimeSet();

            SetDefaultData();
            NrActingSearch();

            #region 병동 설정
            int sIndex = 0;
            cboWard.Items.Add("전체" + VB.Space(100) + "간호활동항목");
            cboWard.Enabled = false;

            DataTable dt = null;
            string SQL = " SELECT NAME WARDCODE, MATCH_CODE";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_CODE";
            SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '2'";
            SQL = SQL + ComNum.VBLF + "   AND SUBUSE = '1'";
            SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING";

            string SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            string WardCodes = string.Empty;
            if (VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "") != "")
            {
                WardCodes = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "");
            }

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cboWard.Items.Add(dt.Rows[i]["WardCode"].ToString().Trim());
                    if (dt.Rows[i]["WardCode"].ToString().Trim().Equals(WardCodes))
                    {
                        sIndex = i + 1;
                    }
                }
            }

            dt.Dispose();

            cboWard.Items.Add("HD");
            cboWard.Items.Add("ER");
            cboWard.Items.Add("OP");
            cboWard.Items.Add("ENDO");
            cboWard.Items.Add("외래수혈");
            cboWard.Items.Add("CT");
            cboWard.Items.Add("MRI");
            cboWard.Items.Add("RI");
            cboWard.Items.Add("SONO");

            cboWard.SelectedIndex = sIndex == -1 ? cboWard.SelectedIndex : sIndex;

            if (NURSE_Manager_Check(clsType.User.IdNumber) == true || clsVbfunc.GetBCodeCODE(clsDB.DbCon, "NUR_간호부관리자사번IP", "").Equals(clsCompuInfo.gstrCOMIP))
            {
                cboWard.Enabled = true;
            }

            cboWard.Enabled = true;
            #endregion
        }

        private void GetActTimeSet()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT ";
            SQL = SQL + ComNum.VBLF + "     VFLAG1";
            SQL = SQL + ComNum.VBLF + " FROM ADMIN.AEMRBASCD ";
            SQL = SQL + ComNum.VBLF + " WHERE BSNSCLS = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "     AND UNITCLS = '간호활동간격'";
            SQL = SQL + ComNum.VBLF + "     AND USECLS = '0'";
            SQL = SQL + ComNum.VBLF + " GROUP BY VFLAG1";
            SQL = SQL + ComNum.VBLF + " ORDER BY VFLAG1";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("수행시간 조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count > 0)
            {
                mstrCode = new string[dt.Rows.Count + 1];
                mstrCodeName = new string[dt.Rows.Count + 1];

                mstrCode[0] = "";
                mstrCodeName[0] = "";

                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    mstrCode[i] = dt.Rows[i - 1]["VFLAG1"].ToString().Trim();
                    mstrCodeName[i] = dt.Rows[i - 1]["VFLAG1"].ToString().Trim();
                }
            }

            dt.Dispose();
            dt = null;
        }

        private bool NURSE_Manager_Check(string strSABUN)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            SQL = "SELECT Code FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
            SQL = SQL + ComNum.VBLF + " WHERE Gubun='NUR_간호부관리자사번' ";
            SQL = SQL + ComNum.VBLF + "   AND Code= " + VB.Val(strSABUN) + " ";
            SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL    ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return false;
            }

            dt.Dispose();
            dt = null;
            return true;
        }

        private void dtpVitalDate_ValueChanged(object sender, EventArgs e)
        {
            NrActingSearch();
        }

        private void FormSearch(string strWard, string strSearch = "")
        {
            string SQL = "";
            int i = 0;
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            ssForm_Sheet1.RowCount = 0;

            if (strWard == "간호활동항목")
            {
                SQL = "";
                SQL = "SELECT B.BASVAL, B.BASCD, B.BASNAME, B.BASEXNAME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD B";
                SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BG";
                SQL = SQL + ComNum.VBLF + "     ON B.VFLAG1 = BG.BASCD ";
                SQL = SQL + ComNum.VBLF + "     AND BG.BSNSCLS = '기록지관리' ";
                SQL = SQL + ComNum.VBLF + "     AND BG.UNITCLS = '간호활동그룹'";
                SQL = SQL + ComNum.VBLF + "WHERE B.BSNSCLS = '기록지관리' ";
                SQL = SQL + ComNum.VBLF + "     AND B.UNITCLS = '" + strWard + "'";
                SQL = SQL + ComNum.VBLF + "     AND B.USECLS = '0' ";

                if (string.IsNullOrWhiteSpace(strSearch) == false)
                {
                    SQL = SQL + ComNum.VBLF + "     AND B.BASEXNAME LIKE '%" + strSearch.Replace("'", "`") + "%'  ";
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY BG.DISSEQNO, B.NFLAG1, B.NFLAG2, B.NFLAG3, B.DISSEQNO";
            }
            else
            {
                SQL = "";
                SQL = "SELECT B.BASVAL, B.BASCD, B.BASNAME, B.BASEXNAME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD BB";
                SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B";
                SQL = SQL + ComNum.VBLF + "     ON BB.BASCD = B.BASCD ";
                SQL = SQL + ComNum.VBLF + "     AND B.BSNSCLS = '기록지관리' ";
                SQL = SQL + ComNum.VBLF + "     AND B.UNITCLS = '간호활동항목'";
                SQL = SQL + ComNum.VBLF + "     AND B.USECLS = '0' ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BG";
                SQL = SQL + ComNum.VBLF + "     ON B.VFLAG1 = BG.BASCD ";
                SQL = SQL + ComNum.VBLF + "     AND BG.BSNSCLS = '기록지관리' ";
                SQL = SQL + ComNum.VBLF + "     AND BG.UNITCLS = '간호활동그룹'";
                SQL = SQL + ComNum.VBLF + "WHERE BB.BSNSCLS = '기록지관리' ";
                SQL = SQL + ComNum.VBLF + "     AND BB.UNITCLS = '간호활동병동_" + strWard + "'";
                SQL = SQL + ComNum.VBLF + "     AND BB.USECLS = '0' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY BG.DISSEQNO, B.NFLAG1, B.NFLAG2, B.NFLAG3, B.DISSEQNO";
            }

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

            string strBASEXNAME = "";
            int intS = 0;

            ssForm_Sheet1.RowCount = dt.Rows.Count;
            
            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssForm_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BASCD"].ToString().Trim();
                ssForm_Sheet1.Cells[i, 2].Text = dt.Rows[i]["BASNAME"].ToString().Trim();
                ssForm_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                ssForm_Sheet1.SetRowHeight(i, ComNum.SPDROWHT);

                if (strBASEXNAME != dt.Rows[i]["BASEXNAME"].ToString().Trim())
                {
                    if (i != 0)
                    {
                        ssForm_Sheet1.AddSpanCell(intS, 1, i - intS, 1);
                    }
                    intS = i;
                }
                strBASEXNAME = dt.Rows[i]["BASEXNAME"].ToString().Trim();
            }
            ssForm_Sheet1.AddSpanCell(intS, 1, i - intS, 1);

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            ssForm_Sheet1.Cells[0, 1, ssForm_Sheet1.RowCount - 1, 1].Value = 1;
        }

        private void btnDeSelect_Click(object sender, EventArgs e)
        {
            ssForm_Sheet1.Cells[0, 1, ssForm_Sheet1.RowCount - 1, 1].Value = 0;
        }

        private void ssForm_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssForm_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                return;
            }

            if (e.Column == 1)
            {
                string strItemGRP = ssForm_Sheet1.Cells[e.Row, 1].Text.Trim();
                InsertAll(strItemGRP);
                NrActingSearch();
            }
            else if (e.Column == 2)
            {
                string strAcpNo = "";
                string strPtNo = "";
                string strItemCD = "";

                strAcpNo = p.acpNo;
                strPtNo = p.ptNo;
                strItemCD = ssForm_Sheet1.Cells[e.Row, 0].Text.Trim();

                Insert(strAcpNo, strPtNo, strItemCD, "1");
                NrActingSearch();
            }
        }

        private void InsertAll(string strItemGRP)
        {
            int i = 0;

            for (i = 0; i < ssForm_Sheet1.RowCount; i++)
            {
                if (strItemGRP == ssForm_Sheet1.Cells[i, 1].Text.Trim())
                {
                    string strAcpNo = "";
                    string strPtNo = "";
                    string strItemCD = "";

                    strAcpNo = p.acpNo;
                    strPtNo = p.ptNo;
                    strItemCD = ssForm_Sheet1.Cells[i, 0].Text.Trim();

                    Insert(strAcpNo, strPtNo, strItemCD, "1");
                }
            }
        }

        private void Insert(string strAcpNo, string strPtNo, string strItemCD, string strACTINTERVAL)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            int intRowAffected = 0;

            string strDate = "";
            string strWDate = "";
            string strWTime = "";

            strDate = VB.Format(dtpVitalDate.Value, "yyyyMMdd");
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            strWDate = VB.Left(strCurDateTime, 8);
            strWTime = VB.Right(strCurDateTime, 6);
            if (VB.Val(strACTINTERVAL) == 0)
            {
                strACTINTERVAL = "3";
            }

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "     ACPNO, PTNO, ITEMCD, CHARTDATE ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBNRACTSET";
            SQL = SQL + ComNum.VBLF + "WHERE ACPNO = '" + strAcpNo + "'";
            SQL = SQL + ComNum.VBLF + "     AND ITEMCD = '" + strItemCD + "'";
            SQL = SQL + ComNum.VBLF + "     AND CHARTDATE = '" + strDate + "'";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dt.Rows.Count != 0)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBoxEx(this, "이미 저장된 DATA입니다.");
                return;
            }
            dt.Dispose();
            dt = null;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (VB.Val(strACTINTERVAL) <= 0) strACTINTERVAL = "3";

                string strVFLAG1 = "";
                string strACTINTERVALCD = "0001";
                string strACTCNT = "3";

                SQL = "";
                SQL = "SELECT ";
                SQL = SQL + ComNum.VBLF + "     BB.VFLAG1, BB.NFLAG1, BB.NFLAG2, BB.NFLAG3 ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD B";
                SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB";
                SQL = SQL + ComNum.VBLF + "     ON B.VFLAG3 = BB.BASCD";
                SQL = SQL + ComNum.VBLF + "     AND BB.BSNSCLS = '기록지관리'";
                SQL = SQL + ComNum.VBLF + "     AND BB.UNITCLS = '간호활동간격'";
                SQL = SQL + ComNum.VBLF + "WHERE B.BSNSCLS = '기록지관리'";
                SQL = SQL + ComNum.VBLF + "     AND B.UNITCLS = '간호활동항목'";
                SQL = SQL + ComNum.VBLF + "     AND B.BASCD = '" + strItemCD + "'";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count != 0)
                {
                    strVFLAG1 = dt.Rows[0]["VFLAG1"].ToString().Trim();

                    if (dt.Rows[0]["NFLAG3"].ToString().Trim() == "0")
                    {
                        strACTINTERVAL = dt.Rows[0]["NFLAG1"].ToString().Trim();
                        strACTINTERVALCD = dt.Rows[0]["VFLAG1"].ToString().Trim();
                        strACTCNT = dt.Rows[0]["NFLAG2"].ToString().Trim();
                    }
                    else
                    {
                        strACTINTERVAL = "0";
                        strACTINTERVALCD = dt.Rows[0]["VFLAG1"].ToString().Trim();
                        strACTCNT = "0";
                    }
                }
                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRBNRACTSET";
                SQL = SQL + ComNum.VBLF + "     (ACPNO, PTNO, CHARTDATE, ITEMCD, ACTINTERVAL, ACTINTERVALCD, ACTCNT, WRITEDATE, WRITETIME, WRITEUSEID)";
                SQL = SQL + ComNum.VBLF + "VALUES (";
                SQL = SQL + ComNum.VBLF + "      " + strAcpNo + ","; //ACPNO
                SQL = SQL + ComNum.VBLF + "     '" + strPtNo + "',"; //PTNO
                SQL = SQL + ComNum.VBLF + "     '" + strDate + "',"; //CHARTDATE
                SQL = SQL + ComNum.VBLF + "     '" + strItemCD + "',"; //ITEMCD
                SQL = SQL + ComNum.VBLF + "     " + strACTINTERVAL + ","; //ACTINTERVAL
                SQL = SQL + ComNum.VBLF + "     '" + strACTINTERVALCD + "',"; //ACTINTERVALCD
                SQL = SQL + ComNum.VBLF + "     " + strACTCNT + ","; //ACTCNT
                SQL = SQL + ComNum.VBLF + "     '" + strWDate + "',"; //WRITEDATE
                SQL = SQL + ComNum.VBLF + "     '" + strWTime + "' ,"; //WRITETIME
                SQL = SQL + ComNum.VBLF + "     '" + clsType.User.IdNumber + "'"; //WRITEUSEID
                SQL = SQL + ComNum.VBLF + ")";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "저장중 오류가 발생하였습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void SetDefaultData()
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            string strDate = dtpVitalDate.Value.ToString("yyyyMMdd");

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL = "SELECT A.ACPNO ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBNRACTSET A";
            SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "    ON A.ITEMCD = B.BASCD";
            SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '간호활동항목'";
            SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = " + strDate;
            SQL = SQL + ComNum.VBLF + "ORDER BY B.VFLAG1, B.NFLAG1, B.NFLAG2, B.NFLAG3";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }
            dt.Dispose();
            dt = null;

            #region //기본값 설정
            //clsDB.setBeginTran(clsDB.DbCon);
            //try
            //{
            //    string strCurDataTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            //    string strWDate = VB.Left(strCurDataTime, 8);
            //    string strWTime = VB.Right(strCurDataTime, 6);

            //    SQL = "";
            //    SQL = "SELECT B.BASVAL, B.BASCD, BASNAME, A.ITEMCNT ";
            //    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBNRACTSET A";
            //    SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRBASCD B";
            //    SQL = SQL + ComNum.VBLF + "    ON A.ITEMCD = B.BASCD";
            //    SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = '기록지관리'";
            //    SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '간호활동항목'";
            //    SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + p.acpNo;
            //    SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = (SELECT MAX(CHARTDATE) FROM " + ComNum.DB_EMR + "AEMRBNRACTSET AA ";
            //    SQL = SQL + ComNum.VBLF + "                                    WHERE AA.ACPNO = " + p.acpNo + ")";
            //    SQL = SQL + ComNum.VBLF + "ORDER BY B.VFLAG1, B.NFLAG1, B.NFLAG2, B.NFLAG3";

            //    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            //    if (SqlErr != "")
            //    {
            //        clsDB.setRollbackTran(clsDB.DbCon);
            //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
            //        ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
            //        Cursor.Current = Cursors.Default;
            //        return;
            //    }

            //    if (dt.Rows.Count > 0)
            //    {
            //        for (i = 0; i < dt.Rows.Count; i++)
            //        {
            //            SQL = "";
            //            SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRBNRACTSET";
            //            SQL = SQL + ComNum.VBLF + "(ACPNO, PTNO, CHARTDATE, ITEMCD,ITEMCNT, WRITEDATE, WRITETIME, WRITEUSEID)";
            //            SQL = SQL + ComNum.VBLF + "VALUES (";
            //            SQL = SQL + ComNum.VBLF + p.acpNo + ","; //ACPNO
            //            SQL = SQL + ComNum.VBLF + "'" + p.ptNo + "',"; //PTNO
            //            SQL = SQL + ComNum.VBLF + "'" + strDate + "',"; //CHARTDATE
            //            SQL = SQL + ComNum.VBLF + "'" + dt.Rows[i]["BASCD"].ToString().Trim() + "',";
            //            SQL = SQL + ComNum.VBLF + "" + dt.Rows[i]["ITEMCNT"].ToString().Trim() + ",";
            //            SQL = SQL + ComNum.VBLF + "'" + strWDate + "',";
            //            SQL = SQL + ComNum.VBLF + "'" + strWTime + "' ,";
            //            SQL = SQL + ComNum.VBLF + "'" + clsType.User.IdNumber + "'";
            //            SQL = SQL + ComNum.VBLF + ")";
            //            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            //            if (SqlErr != "")
            //            {
            //                clsDB.setRollbackTran(clsDB.DbCon);
            //                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
            //                ComFunc.MsgBoxEx(this, "삭제중 오류가 발생하였습니다.");
            //                Cursor.Current = Cursors.Default;
            //                return;
            //            }
            //        }
            //        dt.Dispose();
            //        dt = null;
            //    }
            //    else
            //    {
            //        dt.Dispose();
            //        dt = null;

            //        SQL = "";
            //        SQL = "SELECT BASVAL, BASCD, BASNAME ";
            //        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD";
            //        SQL = SQL + ComNum.VBLF + "WHERE BSNSCLS = '기록지관리'";
            //        SQL = SQL + ComNum.VBLF + "    AND UNITCLS = '간호활동병동'";
            //        SQL = SQL + ComNum.VBLF + "ORDER BY VFLAG1, NFLAG1, NFLAG2, NFLAG3";
            //        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            //        if (SqlErr != "")
            //        {
            //            clsDB.setRollbackTran(clsDB.DbCon);
            //            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
            //            ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
            //            Cursor.Current = Cursors.Default;
            //            return;
            //        }

            //        if (dt.Rows.Count > 0)
            //        {
            //            for (i = 0; i < dt.Rows.Count; i++)
            //            {
            //                SQL = "";
            //                SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRBNRACTSET";
            //                SQL = SQL + ComNum.VBLF + "(ACPNO, PTNO, CHARTDATE, ITEMCD,ITEMCNT, WRITEDATE, WRITETIME, WRITEUSEID)";
            //                SQL = SQL + ComNum.VBLF + "VALUES (";
            //                SQL = SQL + ComNum.VBLF + p.acpNo + ","; //ACPNO
            //                SQL = SQL + ComNum.VBLF + "'" + p.ptNo + "',"; //PTNO
            //                SQL = SQL + ComNum.VBLF + "'" + strDate + "',"; //CHARTDATE
            //                SQL = SQL + ComNum.VBLF + "'" + dt.Rows[i]["BASCD"].ToString().Trim() + "',";
            //                SQL = SQL + ComNum.VBLF + "" + "3" + ",";
            //                SQL = SQL + ComNum.VBLF + "'" + strWDate + "',";
            //                SQL = SQL + ComNum.VBLF + "'" + strWTime + "' ,";
            //                SQL = SQL + ComNum.VBLF + "'" + clsType.User.IdNumber + "'";
            //                SQL = SQL + ComNum.VBLF + ")";
            //                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            //                if (SqlErr != "")
            //                {
            //                    clsDB.setRollbackTran(clsDB.DbCon);
            //                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
            //                    ComFunc.MsgBoxEx(this, "삭제중 오류가 발생하였습니다.");
            //                    Cursor.Current = Cursors.Default;
            //                    return;
            //                }
            //            }
            //        }
            //        dt.Dispose();
            //        dt = null;
            //    }

            //    clsDB.setCommitTran(clsDB.DbCon);
            //    Cursor.Current = Cursors.Default;
            //}
            //catch (Exception ex)
            //{
            //    clsDB.setRollbackTran(clsDB.DbCon);
            //    MessageBox.Show(new Form() { TopMost = true }, ex.Message);
            //    Cursor.Current = Cursors.Default;
            //}
            #endregion //기본값 설정
        }

        private void NrActingSearch()
        {

            string SQL = "";
            string strDate = "";
            int i = 0;
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            strDate = VB.Format(dtpVitalDate.Value, "yyyyMMdd");

            Cursor.Current = Cursors.WaitCursor;
            ssActForm_Sheet1.RowCount = 0;

            SQL = "";
            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "     B.BASVAL, B.BASCD, B.BASNAME, B.BASEXNAME, ";
            SQL = SQL + ComNum.VBLF + "     A.ACTINTERVAL, A.ACTINTERVALCD, A.ACTCNT ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBNRACTSET A";
            SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "    ON A.ITEMCD = B.BASCD";
            SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '간호활동항목'";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BG";
            SQL = SQL + ComNum.VBLF + "     ON B.VFLAG1 = BG.BASCD ";
            SQL = SQL + ComNum.VBLF + "     AND BG.BSNSCLS = '기록지관리' ";
            SQL = SQL + ComNum.VBLF + "     AND BG.UNITCLS = '간호활동그룹'";
            SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = " + strDate;
            SQL = SQL + ComNum.VBLF + "ORDER BY BG.DISSEQNO, B.NFLAG1, B.NFLAG2, B.NFLAG3, B.DISSEQNO";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }
            ssActForm_Sheet1.RowCount = dt.Rows.Count;

            if (mstrCode != null)
            {
                clsSpread.gSpreadComboDataSetEx(ssActForm, 0, 4, ssActForm_Sheet1.RowCount - 1, 4, mstrCode);
            }

            string strBASEXNAME = "";
            int intS = 0;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssActForm_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BASCD"].ToString().Trim();
                //ssActForm_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                ssActForm_Sheet1.Cells[i, 2].Text = dt.Rows[i]["BASNAME"].ToString().Trim();
                ssActForm_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ACTINTERVAL"].ToString().Trim();
                ssActForm_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ACTINTERVALCD"].ToString().Trim();
                ssActForm_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ACTCNT"].ToString().Trim();
                ssActForm_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                ssActForm_Sheet1.SetRowHeight(i, ComNum.SPDROWHT);

                if (strBASEXNAME != dt.Rows[i]["BASEXNAME"].ToString().Trim())
                {
                    if (i != 0)
                    {
                        ssActForm_Sheet1.AddSpanCell(intS, 1, i - intS, 1);
                    }
                    intS = i;
                }
                strBASEXNAME = dt.Rows[i]["BASEXNAME"].ToString().Trim();
            }
            ssActForm_Sheet1.AddSpanCell(intS, 1, i - intS, 1);

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int intRow = 0;

            intRow = ssActForm_Sheet1.ActiveRowIndex;
            Delete(intRow);
        }

        private void ssActForm_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssActForm_Sheet1.RowCount == 0)
            {
                return;
            }
            if (e.ColumnHeader == true)
            {
                return;
            }

            if (e.Column == 1)
            {
                DeleteAll(e.Row);
                NrActingSearch();
            }
            else if (e.Column == 2)
            {
                if (Delete(e.Row) == true)
                {
                    NrActingSearch();
                }
            }
        }

        private void DeleteAll(int intRow)
        {
            int i = 0;
            string ItemGRP = ssActForm_Sheet1.Cells[intRow, 1].Text.Trim();

            for (i = 0; i < ssActForm_Sheet1.RowCount; i++)
            {
                if (ItemGRP == ssActForm_Sheet1.Cells[i, 1].Text.Trim())
                {
                    Delete(i);
                }
            }
        }

        private bool Delete(int intRow)
        {

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            string strAcpNo = "";
            string strPtNo = "";
            string strItemCD = "";

            strAcpNo = p.acpNo;
            strPtNo = p.ptNo;
            strItemCD = ssActForm_Sheet1.Cells[intRow, 0].Text.Trim();

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            DataTable dt = null;
            try
            {
                SQL = "";
                SQL = "SELECT C.ACPNO, C.PTNO, R.ITEMCD, C.CHARTDATE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST C";
                SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R";
                SQL = SQL + ComNum.VBLF + "    ON R.EMRNO    = C.EMRNO";
                SQL = SQL + ComNum.VBLF + "   AND R.EMRNOHIS = C.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "   AND R.ITEMCD = '" + strItemCD + "'";
                //SQL = SQL + ComNum.VBLF + "WHERE C.ACPNO = '" + strAcpNo + "'";          
                SQL = SQL + ComNum.VBLF + "WHERE C.PTNO = '" + strPtNo + "'";
                SQL = SQL + ComNum.VBLF + "  AND C.MEDFRDATE = '" + p.medFrDate + "'";
                SQL = SQL + ComNum.VBLF + "  AND C.CHARTDATE = '" + VB.Format(dtpVitalDate.Value, "yyyyMMdd") + "'";
                SQL = SQL + ComNum.VBLF + "  AND C.FORMNO = 1575";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                if (dt.Rows.Count != 0)
                {
                    dt.Dispose();
                    dt = null;
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, "이미 저장된 DATA가 있습니다." + ComNum.VBLF + "삭제할 수 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "DELETE FROM " + ComNum.DB_EMR + "AEMRBNRACTSET";
                SQL = SQL + ComNum.VBLF + "WHERE  ACPNO = '" + strAcpNo + "'";
                SQL = SQL + ComNum.VBLF + "    AND ITEMCD = '" + strItemCD + "'";
                SQL = SQL + ComNum.VBLF + "    AND CHARTDATE = '" + VB.Format(dtpVitalDate.Value, "yyyyMMdd") + "'";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "삭제중 오류가 발생하였습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSaveSet_Click(object sender, EventArgs e)
        {
            if (UpDate() == true)
            {
                NrActingSearch();
                this.Close();
            }
        }

        private bool UpDate()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strDate = "";
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            string strWDate = "";
            string strWTime = "";
            string strAcpNo = "";
            string strPtNo = "";
            string strItemCD = "";
            string strACTINTERVAL = "";
            string strACTINTERVALCD = "";
            string strACTCNT = "0";

            strDate = VB.Format(dtpVitalDate.Value, "yyyyMMdd");
            strWDate = VB.Left(strCurDateTime, 8);
            strWTime = VB.Right(strCurDateTime, 6);

            strAcpNo = p.acpNo;
            strPtNo = p.ptNo;

            int i = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssActForm_Sheet1.RowCount; i++)
                {
                    strItemCD = ssActForm_Sheet1.Cells[i, 0].Text.Trim();
                    strACTINTERVAL = ssActForm_Sheet1.Cells[i, 3].Text.Trim();
                    strACTINTERVALCD = ssActForm_Sheet1.Cells[i, 4].Text.Trim();
                    strACTCNT = VB.Val(ssActForm_Sheet1.Cells[i, 5].Text.Trim()).ToString();

                    if (VB.Val(strACTINTERVAL) == 0)
                    {
                        strACTINTERVAL = "1";
                    }
                    SQL = "";
                    SQL = "UPDATE " + ComNum.DB_EMR + "AEMRBNRACTSET SET";
                    SQL = SQL + ComNum.VBLF + "    ACTINTERVAL = " + strACTINTERVAL + ",";
                    SQL = SQL + ComNum.VBLF + "    ACTINTERVALCD = '" + strACTINTERVALCD + "',";
                    SQL = SQL + ComNum.VBLF + "    ACTCNT = " + strACTCNT;
                    SQL = SQL + ComNum.VBLF + "WHERE ACPNO='" + strAcpNo + "'";
                    SQL = SQL + ComNum.VBLF + "    AND  ITEMCD ='" + strItemCD + "'";
                    SQL = SQL + ComNum.VBLF + "    AND CHARTDATE = '" + strDate + "'";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBoxEx(this, "삭제중 오류가 발생하였습니다.");
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void cboWard_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormSearch((VB.Right(cboWard.Text, 20)).Trim());
        }

        private void ssForm_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssForm_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssForm, e.Column);
                return;
            }
        }

        private void ssActForm_ComboCloseUp(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            
        }

        #region 검색
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                FormSearch("간호활동항목");
                return;
            }
            FormSearch("간호활동항목", txtSearch.Text.Trim());
        }


        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                FormSearch("간호활동항목");
                return;
            }

            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                FormSearch("간호활동항목", txtSearch.Text.Trim());
            }
        }
        #endregion
    }
}
