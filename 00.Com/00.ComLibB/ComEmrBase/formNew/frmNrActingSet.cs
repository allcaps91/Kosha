using System;
using System.Data;
using System.Windows.Forms;
using ComBase;

namespace ComEmrBase
{
    public partial class frmNrActingSet : Form
    {
        public EmrPatient p = null;
        string strFormName = "";

        public frmNrActingSet()
        {
            InitializeComponent();
        }

        public frmNrActingSet(EmrPatient po, string strFN)
        {
            InitializeComponent();
            p = po;
            strFormName = strFN;
        }

        private void frmNrActingSet_Load(object sender, EventArgs e)
        {
            dtpVitalDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
            cboWard.Items.Clear();
            cboWard.Items.Add("기본" + VB.Space(100) + "간호활동항목");

            SetDefaultData();
            NrActingSearch();

            int sIndex = 0;

            #region 병동 설정
            cboWard.Items.Add("전체");
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
                        sIndex = i;
                    }
                }
            }

            dt.Dispose();
            #endregion

            cboWard.Items.Add("HD");
            cboWard.Items.Add("ER");
            cboWard.Items.Add("OP");
            cboWard.Items.Add("ENDO");
            cboWard.Items.Add("외래수혈");
            cboWard.Items.Add("CT");
            cboWard.Items.Add("MRI");
            cboWard.Items.Add("RI");
            cboWard.Items.Add("SONO");

            cboWard.SelectedIndex = sIndex == -1 ? cboWard.SelectedIndex : sIndex + 1;

            if (NURSE_Manager_Check(clsType.User.IdNumber) == true || clsVbfunc.GetBCodeCODE(clsDB.DbCon, "NUR_간호부관리자사번IP", "").Equals(clsCompuInfo.gstrCOMIP))
            {
                cboWard.Enabled = true;
            }
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

        private void FormSearch(string strWard)
        {
            string SQL = "";
            int i = 0;
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            ssForm_Sheet1.RowCount = 0;

            SQL = "";
            SQL = "SELECT BASVAL, BASCD, BASNAME, BASEXNAME FROM " + ComNum.DB_EMR + "AEMRBASCD";
            SQL = SQL + ComNum.VBLF + "WHERE BSNSCLS = '기록지관리'  AND USECLS = '0' ";
            if (strWard == "간호활동항목")
            {
                SQL = SQL + ComNum.VBLF + "AND UNITCLS = '" + strWard + "'"; //간호활동항목
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "AND UNITCLS = '간호활동병동_" + strWard + "'"; //간호활동항목
            }
            SQL = SQL + ComNum.VBLF + "ORDER BY BASVAL";
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
                ssForm_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BASCD"].ToString().Trim();
                ssForm_Sheet1.Cells[i, 2].Text = dt.Rows[i]["BASNAME"].ToString().Trim();
                ssForm_Sheet1.Cells[i, 4].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        private void mbtnSelect_Click(object sender, EventArgs e)
        {
            ssForm_Sheet1.Cells[0, 1, ssForm_Sheet1.RowCount - 1, 1].Value = 1;
        }

        private void mbtnDeSelect_Click(object sender, EventArgs e)
        {
            ssForm_Sheet1.Cells[0, 1, ssForm_Sheet1.RowCount - 1, 1].Value = 0;
        }

        private void mbtnSave_Click(object sender, EventArgs e)
        {
            string strAcpNo = "";
            string strPtNo = "";
            string strItemCD = "";
            string strFreq = "";
            int i = 0;
            int k = 0;
            int intJongBok = 0;

            strAcpNo = p.acpNo;
            strPtNo = p.ptNo;


            for (i = 0; i < ssForm_Sheet1.RowCount; i++)
            {
                if (ssForm_Sheet1.Cells[i, 1].Text == "True")
                {
                    strItemCD = ssForm_Sheet1.Cells[i, 0].Text.Trim();
                    strFreq = ssForm_Sheet1.Cells[i, 3].Text.Trim();
                    intJongBok = 0;
                    for (k = 0; k < ssActForm_Sheet1.RowCount; k++)
                    {
                        if (ssForm_Sheet1.Cells[i, 0].Text.Trim() ==
                            ssActForm_Sheet1.Cells[k, 0].Text.Trim())
                        {
                            intJongBok = intJongBok + 1;
                        }
                    }

                    if (intJongBok == 0)
                    {
                        Insert(strAcpNo, strPtNo, strItemCD, strFreq);
                    }
                }
            }
            NrActingSearch();
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

            string strAcpNo = "";
            string strPtNo = "";
            string strItemCD = "";
            string strFreq = "";

            if (ssForm_Sheet1.RowCount == 0) return;
            if (e.ColumnHeader == true) return;

            strAcpNo = p.acpNo;
            strPtNo = p.ptNo;
            strFreq = ssForm_Sheet1.Cells[e.Row, 2].Text.Trim();
            strItemCD = ssForm_Sheet1.Cells[e.Row, 0].Text.Trim();

            Insert(strAcpNo, strPtNo, strItemCD, strFreq);
            NrActingSearch();

            //for (i = 0; i < ssActForm_Sheet1.RowCount; i++)
            //{
            //    if (ssActForm_Sheet1.Cells[i, 0].Text.Trim() == strItemCD.Trim())
            //    {
            //        break;
            //    }
            //}

            //ssActForm_Sheet1.SetActiveCell(i, 1);
        }

        private void Insert(string strAcpNo, string strPtNo, string strItemCD, string strFreq)
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
            if (VB.Val(strFreq) == 0)
            {
                strFreq = "1";
            }

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL = "SELECT ACPNO, PTNO, ITEMCD,CHARTDATE FROM " + ComNum.DB_EMR + "AEMRBNRACTSET";
            SQL = SQL + ComNum.VBLF + "WHERE ACPNO = '" + strAcpNo + "'";
            SQL = SQL + ComNum.VBLF + "AND ITEMCD = '" + strItemCD + "'";
            SQL = SQL + ComNum.VBLF + "AND CHARTDATE = '" + strDate + "'";

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
                if (VB.Val(strFreq) <= 0) strFreq = "1";
                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRBNRACTSET";
                SQL = SQL + ComNum.VBLF + "(ACPNO, PTNO, CHARTDATE, ITEMCD,ITEMCNT, WRITEDATE, WRITETIME, WRITEUSEID)";
                SQL = SQL + ComNum.VBLF + "VALUES (";
                SQL = SQL + ComNum.VBLF + strAcpNo + ","; //ACPNO
                SQL = SQL + ComNum.VBLF + "'" + strPtNo + "',"; //PTNO
                SQL = SQL + ComNum.VBLF + "'" + strDate + "',"; //CHARTDATE
                SQL = SQL + ComNum.VBLF + "'" + strItemCD + "',";
                SQL = SQL + ComNum.VBLF + "" + strFreq + ",";
                SQL = SQL + ComNum.VBLF + "'" + strWDate + "',";
                SQL = SQL + ComNum.VBLF + "'" + strWTime + "' ,";
                SQL = SQL + ComNum.VBLF + "'" + clsType.User.IdNumber + "'";
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
            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            int intRowAffected = 0;

            string strDate = dtpVitalDate.Value.ToString("yyyyMMdd");

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL = "SELECT B.BASVAL, B.BASCD, BASNAME, A.ITEMCNT ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBNRACTSET A";
            SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "    ON A.ITEMCD = B.BASCD";
            SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '간호활동항목'";
            SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = " + strDate;
            SQL = SQL + ComNum.VBLF + "ORDER BY B.BASVAL";

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

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                string strCurDataTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                string strWDate = VB.Left(strCurDataTime, 8);
                string strWTime = VB.Right(strCurDataTime, 6);

                SQL = "";
                SQL = "SELECT B.BASVAL, B.BASCD, BASNAME, A.ITEMCNT ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBNRACTSET A";
                SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRBASCD B";
                SQL = SQL + ComNum.VBLF + "    ON A.ITEMCD = B.BASCD";
                SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = '기록지관리'";
                SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '간호활동항목'";
                SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + p.acpNo;
                SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = (SELECT MAX(CHARTDATE) FROM " + ComNum.DB_EMR + "AEMRBNRACTSET AA ";
                SQL = SQL + ComNum.VBLF + "                                    WHERE AA.ACPNO = " + p.acpNo + ")";
                SQL = SQL + ComNum.VBLF + "ORDER BY B.BASVAL";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRBNRACTSET";
                        SQL = SQL + ComNum.VBLF + "(ACPNO, PTNO, CHARTDATE, ITEMCD,ITEMCNT, WRITEDATE, WRITETIME, WRITEUSEID)";
                        SQL = SQL + ComNum.VBLF + "VALUES (";
                        SQL = SQL + ComNum.VBLF + p.acpNo + ","; //ACPNO
                        SQL = SQL + ComNum.VBLF + "'" + p.ptNo + "',"; //PTNO
                        SQL = SQL + ComNum.VBLF + "'" + strDate + "',"; //CHARTDATE
                        SQL = SQL + ComNum.VBLF + "'" + dt.Rows[i]["BASCD"].ToString().Trim() + "',";
                        SQL = SQL + ComNum.VBLF + "" + dt.Rows[i]["ITEMCNT"].ToString().Trim() + ",";
                        SQL = SQL + ComNum.VBLF + "'" + strWDate + "',";
                        SQL = SQL + ComNum.VBLF + "'" + strWTime + "' ,";
                        SQL = SQL + ComNum.VBLF + "'" + clsType.User.IdNumber + "'";
                        SQL = SQL + ComNum.VBLF + ")";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBoxEx(this, "삭제중 오류가 발생하였습니다.");
                            Cursor.Current = Cursors.Default;
                            return;
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
                    SQL = SQL + ComNum.VBLF + "WHERE BSNSCLS = '기록지관리'";
                    SQL = SQL + ComNum.VBLF + "    AND UNITCLS = '간호활동병동'";
                    SQL = SQL + ComNum.VBLF + "ORDER BY BASVAL";
                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            SQL = "";
                            SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRBNRACTSET";
                            SQL = SQL + ComNum.VBLF + "(ACPNO, PTNO, CHARTDATE, ITEMCD,ITEMCNT, WRITEDATE, WRITETIME, WRITEUSEID)";
                            SQL = SQL + ComNum.VBLF + "VALUES (";
                            SQL = SQL + ComNum.VBLF + p.acpNo + ","; //ACPNO
                            SQL = SQL + ComNum.VBLF + "'" + p.ptNo + "',"; //PTNO
                            SQL = SQL + ComNum.VBLF + "'" + strDate + "',"; //CHARTDATE
                            SQL = SQL + ComNum.VBLF + "'" + dt.Rows[i]["BASCD"].ToString().Trim() + "',";
                            SQL = SQL + ComNum.VBLF + "" + "3" + ",";
                            SQL = SQL + ComNum.VBLF + "'" + strWDate + "',";
                            SQL = SQL + ComNum.VBLF + "'" + strWTime + "' ,";
                            SQL = SQL + ComNum.VBLF + "'" + clsType.User.IdNumber + "'";
                            SQL = SQL + ComNum.VBLF + ")";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBoxEx(this, "삭제중 오류가 발생하였습니다.");
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
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
                Cursor.Current = Cursors.Default;
            }
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
            SQL = "SELECT B.BASVAL, B.BASCD, BASNAME, A.ITEMCNT ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBNRACTSET A";
            SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "    ON A.ITEMCD = B.BASCD";
            SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '간호활동항목'";
            SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = " + strDate;
            SQL = SQL + ComNum.VBLF + "ORDER BY B.BASVAL";

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
            ssActForm_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssActForm_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BASCD"].ToString().Trim();
                ssActForm_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BASNAME"].ToString().Trim();
                ssActForm_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ITEMCNT"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;


        }

        private void mbtnDelete_Click(object sender, EventArgs e)
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

            if (Delete(e.Row) == true)
            {
                NrActingSearch();
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
            strItemCD = ssActForm_Sheet1.Cells[intRow, 0].Text.Trim(); ;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            DataTable dt = null;
            try
            {
                SQL = "";
                SQL = "SELECT ACPNO, PTNO, ITEMCD, CHARTDATE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST C";
                SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRNRACT R";
                SQL = SQL + ComNum.VBLF + "    ON R.EMRNO = C.EMRNO";
                SQL = SQL + ComNum.VBLF + "    AND R.ITEMCD = '" + strItemCD + "'";
                SQL = SQL + ComNum.VBLF + "    AND R.DCCLS = '0'";
                SQL = SQL + ComNum.VBLF + "WHERE C.ACPNO = '" + strAcpNo + "'";
                SQL = SQL + ComNum.VBLF + "    AND C.CHARTDATE = '" + VB.Format(dtpVitalDate.Value, "yyyyMMdd") + "'";
                SQL = SQL + ComNum.VBLF + "    AND C.FORMNO = 526";
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

        private void mbtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mbtnSet_Click(object sender, EventArgs e)
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
            string strFreq = "";

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
                    strFreq = ssActForm_Sheet1.Cells[i, 2].Text.Trim();

                    if (VB.Val(strFreq) == 0)
                    {
                        strFreq = "1";
                    }
                    SQL = "";
                    SQL = "UPDATE " + ComNum.DB_EMR + "AEMRBNRACTSET SET";
                    SQL = SQL + ComNum.VBLF + "    ITEMCNT = " + strFreq;
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

        private void mbtnSaveWard_Click(object sender, EventArgs e)
        {
            using (frmNrActingSetWard frmNrActingSetWardX = new frmNrActingSetWard())
            {
                frmNrActingSetWardX.TopMost = true;
                frmNrActingSetWardX.ShowDialog();
            }
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
    }
}
