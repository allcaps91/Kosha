using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmVitalSet : Form
    {
        #region //변수 선언
        public EmrPatient p = null;
        public string mstrFormName = "";
        public string mstrFormNameGb = "기록지관리";
        public string mstrFormNameWard = "임상관찰";
        public string mstrFormNo = "";
        public string mstrUpdateNo = "1";
        public string mJOBGB = "VT";
        #endregion //변수 선언

        #region //생성자
        public frmVitalSet()
        {
            InitializeComponent();
        }

        public frmVitalSet(EmrPatient po, string strFN )
        {
            InitializeComponent();
            p = po;
            mstrFormName = strFN;
        }

        public frmVitalSet(EmrPatient po, string strFN, string strFormNo, string strUpdateNo)
        {
            InitializeComponent();
            p = po;
            mstrFormName = strFN;
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
        }

        private void frmVitalSet_Load(object sender, EventArgs e)
        {
            TitleChange();
            FormSearch();
            SetDefaultData();
            VitalFormSearch();
        }
        #endregion //변수 선언

        #region //함수 선언

        /// <summary>
        /// 타이틀을 바꾼다
        /// </summary>
        private void TitleChange()
        {
            if (mstrFormName == "임상관찰")
            {
                lblTitle.Text = mstrFormName + " 설정";
                mJOBGB = "IVT";
                mstrFormNameWard = "임상관찰";
            }
            else if (mstrFormName == "섭취배설")
            {
                lblTitle.Text = mstrFormName + " 설정";
                mJOBGB = "IIO";
                mstrFormNameWard = "섭취배설";
            }
            else if (mstrFormName == "특수치료")
            {
                lblTitle.Text = mstrFormName + " 설정";
                mJOBGB = "IST";
                mstrFormNameWard = "특수치료";
            }
            else if (mstrFormName == "기본간호")
            {
                lblTitle.Text = mstrFormName + " 설정";
                mJOBGB = "IBN";
                mstrFormNameWard = "기본간호";
            }
            else
            {
                lblTitle.Text = "설정 화면";
            }
        }

        /// <summary>
        /// 아이템 조회 전체
        /// </summary>
        private void FormSearch(string strSearch = "")
        {
            string SQL = "";
            int i = 0;
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            ssForm_Sheet1.RowCount = 0;

            SQL = "";
            SQL = "SELECT B.BASCD, B.BASNAME, B.BASEXNAME ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB";
            SQL = SQL + ComNum.VBLF + "    ON B.VFLAG1 = BB.BASCD";
            SQL = SQL + ComNum.VBLF + "    AND BB.BSNSCLS = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "    AND BB.UNITCLS = '" + VB.Left(mstrFormNameWard, 4) + "그룹" + "'";
            SQL = SQL + ComNum.VBLF + "WHERE B.BSNSCLS = '" + mstrFormNameGb + "'";
            SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '" + mstrFormNameWard + "'";
            SQL = SQL + ComNum.VBLF + "    AND B.USECLS = '0'";
            if (mJOBGB == "IIO")
            {
                SQL = SQL + ComNum.VBLF + "    AND B.REMARK2 IS NULL";
            }

            if (string.IsNullOrWhiteSpace(strSearch) == false)
            {
                SQL = SQL + ComNum.VBLF + "     AND B.BASEXNAME LIKE '%" + strSearch.Replace("'", "`") + "%'  ";
            }

            //SQL = SQL + ComNum.VBLF + "ORDER BY B.NFLAG1, B.BASVAL, B.NFLAG2, B.NFLAG3, B.DISSEQNO";
            SQL = SQL + ComNum.VBLF + "ORDER BY BB.DISSEQNO, B.NFLAG1, B.NFLAG2, B.NFLAG3, B.DISSEQNO";

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
            ssForm_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssForm_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BASCD"].ToString().Trim();
                ssForm_Sheet1.Cells[i, 3].Text = dt.Rows[i]["BASNAME"].ToString().Trim();
                ssForm_Sheet1.Cells[i, 2].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                if (strBASEXNAME != dt.Rows[i]["BASEXNAME"].ToString().Trim())
                {
                    ssForm_Sheet1.Cells[i, 2].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                    if (i != 0)
                    {
                        ssForm_Sheet1.AddSpanCell(intS, 2, i - intS, 1);
                    }
                    intS = i;
                }
                
                strBASEXNAME = dt.Rows[i]["BASEXNAME"].ToString().Trim();
            }
            ssForm_Sheet1.AddSpanCell(intS, 2, i - intS, 1);

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// 아이템 그룹 전체를 저장한다
        /// </summary>
        /// <param name="ItemGRP"></param>
        private void InsertAll(string ItemGRP)
        {
            int i = 0;
            
            for (i = 0; i < ssForm_Sheet1.RowCount; i++)
            {
                if (ItemGRP == ssForm_Sheet1.Cells[i, 2].Text.Trim())
                {
                    string strAcpNo = "";
                    string strPtNo = "";
                    string ItemCD = "";

                    strAcpNo = p.acpNo;
                    strPtNo = p.ptNo;
                    ItemCD = ssForm_Sheet1.Cells[i, 0].Text.Trim();
                    Insert(ItemCD);
                }
            }
        }

        /// <summary>
        /// 하나씩 저정을 한다
        /// </summary>
        /// <param name="ItemCD"></param>
        private void Insert(string ItemCD)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strDate = "";
            string strWDate = "";
            string strWTime = "";

            strDate = VB.Format(dtpVitalDate.Value, "yyyyMMdd");

            string strCurDataTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            strWDate = VB.Left(strCurDataTime,8);
            strWTime = VB.Right(strCurDataTime, 6);

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "    ACPNO, PTNO, ITEMCD,CHARTDATE ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET";
            SQL = SQL + ComNum.VBLF + "WHERE FORMNO = " + mstrFormNo ;
            SQL = SQL + ComNum.VBLF + "    AND ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND CHARTDATE = '" + strDate + "'";
            SQL = SQL + ComNum.VBLF + "    AND JOBGB = '" + mJOBGB + "'";
            SQL = SQL + ComNum.VBLF + "    AND ITEMCD = '" + ItemCD + "'";
            
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
                ComFunc.MsgBoxEx(this, "이미 등록된 DATA입니다.");
                return;
            }
            dt.Dispose();
            dt = null;
            
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
            
                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRBVITALSET";
                SQL = SQL + ComNum.VBLF + "(FORMNO, ACPNO, PTNO, CHARTDATE, JOBGB, ITEMCD, WRITEDATE, WRITETIME, WRITEUSEID)";
                SQL = SQL + ComNum.VBLF + "VALUES ( ";
                SQL = SQL + ComNum.VBLF + "     " + mstrFormNo + ", ";
                SQL = SQL + ComNum.VBLF + "     " + p.acpNo + ", ";
                SQL = SQL + ComNum.VBLF + "     '" + p.ptNo + "', ";
                SQL = SQL + ComNum.VBLF + "     '" + strDate + "',";
                SQL = SQL + ComNum.VBLF + "     '" + mJOBGB + "', ";
                SQL = SQL + ComNum.VBLF + "     '" + ItemCD + "', ";
                SQL = SQL + ComNum.VBLF + "     '" + strWDate + "', ";
                SQL = SQL + ComNum.VBLF + "     '" + strWTime + "', ";
                SQL = SQL + ComNum.VBLF + "     '" + clsType.User.IdNumber + "'";
                SQL = SQL + ComNum.VBLF + "     )";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                 clsDB.setCommitTran(clsDB.DbCon);
                 Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        /// <summary>
        /// 환자별 저장된 데이타를 조회한다
        /// </summary>
        private void SetDefaultData()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "    B.BASCD, BASNAME  ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET A";
            SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "    ON A.ITEMCD = B.BASCD";
            SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = '" + mstrFormNameGb + "'";
            SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '" + mstrFormNameWard + "'";
            SQL = SQL + ComNum.VBLF + "WHERE A.FORMNO = " + mstrFormNo;
            SQL = SQL + ComNum.VBLF + "    AND A.ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND A.JOBGB = '" + mJOBGB + "'";
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = '" + dtpVitalDate.Value.ToString("yyyyMMdd") + "'";

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
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                string strCurDataTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                string strWDate = VB.Left(strCurDataTime, 8);
                string strWTime = VB.Right(strCurDataTime, 6);

                SQL = "";
                SQL = "SELECT B.BASVAL, B.BASCD, BASNAME  FROM " + ComNum.DB_EMR + "AEMRBVITALSET A";
                SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRBASCD B";
                SQL = SQL + ComNum.VBLF + "    ON A.ITEMCD = B.BASCD";
                SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = '" + mstrFormNameGb + "'";
                SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '" + mstrFormNameWard + "'";
                SQL = SQL + ComNum.VBLF + "WHERE A.FORMNO = " + mstrFormNo;
                SQL = SQL + ComNum.VBLF + "    AND A.ACPNO = " + p.acpNo;
                SQL = SQL + ComNum.VBLF + "    AND JOBGB = '" + mJOBGB + "'";
                SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = (SELECT MAX(CHARTDATE) FROM " + ComNum.DB_EMR + "AEMRBVITALSET AA ";
                SQL = SQL + ComNum.VBLF + "                                    WHERE AA.FORMNO = " + mstrFormNo;
                SQL = SQL + ComNum.VBLF + "                                        AND AA.ACPNO = " + p.acpNo;
                SQL = SQL + ComNum.VBLF + "                                        AND AA.JOBGB = '" + mJOBGB + "')";
                SQL = SQL + ComNum.VBLF + "ORDER BY B.BASEXNAME, B.DISSEQNO";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return ;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRBVITALSET ";
                        SQL = SQL + ComNum.VBLF + "(FORMNO, ACPNO, PTNO, CHARTDATE, JOBGB, ITEMCD, WRITEDATE, WRITETIME, WRITEUSEID)";
                        SQL = SQL + ComNum.VBLF + "VALUES (" + mstrFormNo + ", " + p.acpNo + ",'" + p.ptNo + "','" + dtpVitalDate.Value.ToString("yyyyMMdd") + "',";
                        SQL = SQL + ComNum.VBLF + "'" + mJOBGB + "','" + dt.Rows[i]["BASCD"].ToString().Trim() + "','" + strWDate + "','" + strWTime + "' ,'" + clsType.User.IdNumber + "')";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return ;
                        }
                    }
                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    dt.Dispose();
                    dt = null;

                    SQL = "";
                    SQL = "SELECT BASVAL, BASCD, BASNAME ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD";
                    SQL = SQL + ComNum.VBLF + "WHERE BSNSCLS = '" + mstrFormNameGb + "'";
                    SQL = SQL + ComNum.VBLF + "    AND UNITCLS = '" + mstrFormNameWard + "'";
                    SQL = SQL + ComNum.VBLF + "    AND BASVAL = 1";
                    SQL = SQL + ComNum.VBLF + "    AND USECLS = '0'";
                    SQL = SQL + ComNum.VBLF + "ORDER BY BASEXNAME, DISSEQNO";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            SQL = "";
                            SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRBVITALSET ";
                            SQL = SQL + ComNum.VBLF + "(FORMNO, ACPNO, PTNO, CHARTDATE, JOBGB, ITEMCD, WRITEDATE, WRITETIME, WRITEUSEID)";
                            SQL = SQL + ComNum.VBLF + "VALUES (" + mstrFormNo + ", " + p.acpNo + ",'" + p.ptNo + "','" + dtpVitalDate.Value.ToString("yyyyMMdd") + "',";
                            SQL = SQL + ComNum.VBLF + "'" + mJOBGB + "','" + dt.Rows[i]["BASCD"].ToString().Trim() + "','" + strWDate + "','" + strWTime + "' ,'" + clsType.User.IdNumber + "')";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBoxEx(this, SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                    }
                    dt.Dispose();
                    dt = null;
                }
                
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        /// <summary>
        /// 임상관찰 항목 조회
        /// </summary>
        private void VitalFormSearch()
        {
            string strDate = "";
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            strDate = VB.Format(dtpVitalDate.Value, "yyyyMMdd");
            
            Cursor.Current = Cursors.WaitCursor;
            ssVitalForm_Sheet1.RowCount = 0;
            
            
            SQL = "";
            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "    B.BASVAL, B.BASCD, B.BASNAME, B.BASEXNAME  ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET A";
            SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "    ON A.ITEMCD = B.BASCD";
            SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = '" + mstrFormNameGb + "'";
            SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '" + mstrFormNameWard + "'";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB";
            SQL = SQL + ComNum.VBLF + "    ON B.VFLAG1 = BB.BASCD";
            SQL = SQL + ComNum.VBLF + "    AND BB.BSNSCLS = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "    AND BB.UNITCLS = '" + VB.Left(mstrFormNameWard, 4) + "그룹" + "'";
            SQL = SQL + ComNum.VBLF + "WHERE A.FORMNO = " + mstrFormNo;
            SQL = SQL + ComNum.VBLF + "    AND A.ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND A.JOBGB = '" + mJOBGB + "'";
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = '" + dtpVitalDate.Value.ToString("yyyyMMdd") + "'";
            SQL = SQL + ComNum.VBLF + "ORDER BY BB.DISSEQNO, B.NFLAG1, B.NFLAG2, B.NFLAG3, B.DISSEQNO";

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
                string strBASEXNAME = "";
                int intS = 0;

                ssVitalForm_Sheet1.RowCount = dt.Rows.Count;
                ssVitalForm_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssVitalForm_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BASCD"].ToString().Trim();
                    ssVitalForm_Sheet1.Cells[i, 2].Text = dt.Rows[i]["BASNAME"].ToString().Trim();
                    ssVitalForm_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                    if (strBASEXNAME != dt.Rows[i]["BASEXNAME"].ToString().Trim())
                    {
                        if (i != 0)
                        {
                            ssVitalForm_Sheet1.AddSpanCell(intS, 1, i - intS, 1);
                        }
                        intS = i;
                    }
                    strBASEXNAME = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                }
                ssVitalForm_Sheet1.AddSpanCell(intS, 1, i - intS, 1);

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }
            dt.Dispose();
            dt = null;
        }

        /// <summary>
        /// 아이템 그룹 전체를 삭제 한다
        /// </summary>
        /// <param name="intRow"></param>
        private void DeleteAll(int intRow)
        {
            int i = 0;
            string ItemGRP = ssVitalForm_Sheet1.Cells[intRow, 1].Text.Trim();

            for (i = 0; i < ssVitalForm_Sheet1.RowCount; i++)
            {
                if (ItemGRP == ssVitalForm_Sheet1.Cells[i, 1].Text.Trim())
                {
                    Delete(i);
                }
            }
        }

        /// <summary>
        /// 하나씩 삭제를 한다
        /// </summary>
        /// <param name="intRow"></param>
        private void Delete(int intRow)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strAcpNo = "";
            string strPtNo = "";
            string strItemCD = "";
            
            strAcpNo = p.acpNo;
            strPtNo = p.ptNo;
            strItemCD = ssVitalForm_Sheet1.Cells[intRow, 0].Text.Trim(); ;

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT A.EMRNO, A.CHARTDATE, A.CHARTTIME, A.CHARTUSEID, A.PRNTYN, ";
            SQL = SQL + ComNum.VBLF + "                B. ITEMCD, B.ITEMNO, B.ITEMINDEX, B.ITEMTYPE, B.ITEMVALUE";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B";
            SQL = SQL + ComNum.VBLF + "    ON  B.EMRNO = A.EMRNO";
            SQL = SQL + ComNum.VBLF + "    AND B. ITEMCD = '" + strItemCD + "'";
            SQL = SQL + ComNum.VBLF + "    AND (B.ITEMVALUE <> '' OR B.ITEMVALUE IS NOT NULL)";
            SQL = SQL + ComNum.VBLF + "WHERE A.FORMNO = " + mstrFormNo;
            SQL = SQL + ComNum.VBLF + "    AND A.ACPNO = " + p.acpNo ;
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = '" + dtpVitalDate.Value.ToString("yyyyMMdd") + "'";
            SQL = SQL + ComNum.VBLF + "    AND A.FORMNO = " + mstrFormNo;
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
                dt.Dispose();
                dt = null;
                ComFunc.MsgBoxEx(this, "당일 차트가 입력된 아이템입니다."+ ComNum.VBLF +"삭제할 수 없습니다.");
                return;
            }
            dt.Dispose();
            dt = null;

            //총섭취량, 총배설량 : 세부 아이템이 존재하면 삭제 못하도록
            if (strItemCD == "I0000030622" || strItemCD == "I0000030623")
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT A.EMRNO, A.CHARTDATE, A.CHARTTIME, A.CHARTUSEID, A.PRNTYN, ";
                SQL = SQL + ComNum.VBLF + "                B. ITEMCD, B.ITEMNO, B.ITEMINDEX, B.ITEMTYPE, B.ITEMVALUE";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B";
                SQL = SQL + ComNum.VBLF + "    ON  B.EMRNO = A.EMRNO";
                SQL = SQL + ComNum.VBLF + "    AND (B.ITEMVALUE <> '' OR B.ITEMVALUE IS NOT NULL)";
                SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRBASCD I";
                SQL = SQL + ComNum.VBLF + "    ON  B.ITEMCD = I.BASCD";
                SQL = SQL + ComNum.VBLF + "    AND I.BSNSCLS = '기록지관리' ";
                SQL = SQL + ComNum.VBLF + "    AND I.UNITCLS = '섭취배설' ";
                if (strItemCD == "I0000030622")
                {
                    SQL = SQL + ComNum.VBLF + "    AND I.VFLAG3 = '01.섭취' ";
                }
                else if (strItemCD == "I0000030623")
                {
                    SQL = SQL + ComNum.VBLF + "    AND I.VFLAG3 = '11.배설' ";
                }
                SQL = SQL + ComNum.VBLF + "WHERE A.FORMNO = " + mstrFormNo;
                SQL = SQL + ComNum.VBLF + "    AND A.ACPNO = " + p.acpNo;
                SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = '" + dtpVitalDate.Value.ToString("yyyyMMdd") + "'";
                SQL = SQL + ComNum.VBLF + "    AND A.FORMNO = " + mstrFormNo;
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
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBoxEx(this, "당일 섭취/배설 아이템존재합니다" + ComNum.VBLF + "삭제할 수 없습니다.");
                    return;
                }
                dt.Dispose();
                dt = null;
            }

            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "DELETE FROM " + ComNum.DB_EMR + "AEMRBVITALSET";
                SQL = SQL + ComNum.VBLF + "WHERE  FORMNO = " + mstrFormNo;
                SQL = SQL + ComNum.VBLF + "    AND ACPNO = " + p.acpNo;
                SQL = SQL + ComNum.VBLF + "    AND JOBGB = '" + mJOBGB + "'";
                SQL = SQL + ComNum.VBLF + "    AND ITEMCD = '" + strItemCD + "'";
                SQL = SQL + ComNum.VBLF + "    AND CHARTDATE = " + VB.Format(dtpVitalDate.Value, "yyyyMMdd");

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return ;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return ;
            }
        }

        #endregion //함수 선언

        #region //기타 컨트롤 이벤트
        private void mbtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void dtpVitalDate_ValueChanged(object sender, EventArgs e)
        {
            SetDefaultData();
            VitalFormSearch();
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

            if (e.Column == 2)
            {
                string ItemGRP = ssForm_Sheet1.Cells[e.Row, 2].Text.Trim();

                InsertAll(ItemGRP);
                VitalFormSearch();
            }
            else if (e.Column == 3)
            {
                string strAcpNo = "";
                string strPtNo = "";
                string ItemCD = "";

                strAcpNo = p.acpNo;
                strPtNo = p.ptNo;
                ItemCD = ssForm_Sheet1.Cells[e.Row, 0].Text.Trim();

                Insert(ItemCD);
                VitalFormSearch();
            }
        }

        private void ssVitalForm_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                return;
            }
            if (ssVitalForm_Sheet1.RowCount == 0)
            {
                return;
            }
            if (e.Column == 1)
            {
                DeleteAll(e.Row);
                VitalFormSearch();
            }
            else if (e.Column == 2)
            {
                Delete(e.Row);
                VitalFormSearch();
            }
        }

        #endregion //기타 컨트롤 이벤트

        #region 검색
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                FormSearch();
                return;
            }
            FormSearch(txtSearch.Text.Trim());
        }


        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                FormSearch();
                return;
            }

            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            { 
                FormSearch(txtSearch.Text.Trim());
            }
        }
        #endregion
    }
}
