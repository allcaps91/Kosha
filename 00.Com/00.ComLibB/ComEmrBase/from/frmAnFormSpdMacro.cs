using ComBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmAnFormSpdMacro : Form
    {
        //이벤트를 전달할 경우
        public delegate void GetInfo(string Name, string Unit);
        public event GetInfo rGetInfo;

        //폼이 Close 될 경우
        //public delegate void EventClosed();
        //public event EventClosed rEventClosed;

        string strFlag = "";
        int mGB = 0;

        public frmAnFormSpdMacro()
        {
            InitializeComponent();
        }

        public frmAnFormSpdMacro(int nGB)
        {
            InitializeComponent();
            mGB = nGB;
        }

        private void frmAnFormSpdMacro_Load(object sender, EventArgs e)
        {
            panMng.Visible = false;
            btnMng.Visible = true;
            btnSave.Visible = false;

            // 상용구 콤보박스 세팅
            SetCombo();
            // 분류코드 조회
            GetGubunList();
            // 약속처방 조회
            GetDataList2();

            ssView.ActiveSheet.Columns[0].Visible = true;
            ssView.ActiveSheet.Columns[1].Visible = false;
            ssView.ActiveSheet.Columns[0].Locked = true;
            ssView.ActiveSheet.Columns[1].Locked = true;
        }

        private void SetCombo()
        {
            cboGubun.Items.Clear();
            cboGubun.Items.Add("01. 약    품");
            cboGubun.Items.Add("02. 물    품");
            cboGubun.Items.Add("03. P  C  A ");
            cboGubun.Items.Add("04. 회 복 실");
            cboGubun.Items.Add("05. Intake");
            cboGubun.SelectedIndex = mGB;
        }

        private void GetData(int nGb)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            string[] arrGB = new string[] { "Medication", "물품", "PCA", "회복실", "Intake" };

            ssView.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT BASCD, BASNAME, BASEXNAME, DISSEQNO   ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRBASCD        ";
                SQL = SQL + ComNum.VBLF + " WHERE BSNSCLS = '기록지관리'                ";
                SQL = SQL + ComNum.VBLF + "   AND UNITCLS = '마취기록지매크로'          ";
                SQL = SQL + ComNum.VBLF + "   AND REMARK1 = '" + arrGB[nGb] + "'        ";
                SQL = SQL + ComNum.VBLF + "ORDER BY DISSEQNO                            ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView.ActiveSheet.RowCount = dt.Rows.Count;
                    ssView.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["BASNAME"].ToString();
                        ssView.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["BASEXNAME"].ToString();
                    }
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void cboGubun_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetData(cboGubun.SelectedIndex);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ssView.ActiveSheet.RowCount = ssView.ActiveSheet.RowCount + 1;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {            
            int rowIndex = ssView.ActiveSheet.ActiveRowIndex;

            if (rowIndex < 0)
            {
                return;
            }

            ssView.ActiveSheet.RemoveRows(rowIndex, 1);            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveData(cboGubun.SelectedIndex);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool SaveData(int nGb)
        {
            int i = 0;
            //DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string[] arrGB = new string[] { "Medication", "물품", "PCA", "회복실", "Intake" };
            string strBasCd = "";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);


            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                //string strGRPCDB = "";


                SQL = "";                
                SQL = SQL + ComNum.VBLF + "DELETE FROM " + ComNum.DB_EMR + "AEMRBASCD   ";
                SQL = SQL + ComNum.VBLF + " WHERE BSNSCLS = '기록지관리'                ";
                SQL = SQL + ComNum.VBLF + "   AND UNITCLS = '마취기록지매크로'            ";
                SQL = SQL + ComNum.VBLF + "   AND REMARK1 = '" + arrGB[nGb] + "'        ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }


                for (i = 0; i < ssView.ActiveSheet.RowCount; i++)
                {
                    if (nGb == 0) strBasCd = "M" + ComFunc.LPAD(i.ToString(), 4, "0");
                    else if (nGb == 1) strBasCd = "G" + ComFunc.LPAD(i.ToString(), 4, "0");
                    else if (nGb == 2) strBasCd = "P" + ComFunc.LPAD(i.ToString(), 4, "0");
                    else if (nGb == 3) strBasCd = "R" + ComFunc.LPAD(i.ToString(), 4, "0");
                    else if (nGb == 4) strBasCd = "I" + ComFunc.LPAD(i.ToString(), 4, "0");


                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRBASCD  ";
                    SQL = SQL + ComNum.VBLF + " ( ";
                    SQL = SQL + ComNum.VBLF + "   BSNSCLS, UNITCLS, BASCD, APLFRDATE, APLENDDATE, ";
                    SQL = SQL + ComNum.VBLF + "   BASNAME, BASEXNAME, BASVAL, REMARK1, NFLAG1, ";                                        
                    SQL = SQL + ComNum.VBLF + "   USECLS, MNGCLS, DISSEQNO  ";
                    SQL = SQL + ComNum.VBLF + " )";
                    SQL = SQL + ComNum.VBLF + "VALUES ( ";
                    SQL = SQL + ComNum.VBLF + "     '기록지관리',                                       ";                 
                    SQL = SQL + ComNum.VBLF + "     '마취기록지매크로',                                 ";          
                    SQL = SQL + ComNum.VBLF + "     '" + strBasCd + "',   ";  //BASCD
                    SQL = SQL + ComNum.VBLF + "     '" + VB.Left(strCurDateTime, 8) + "',               ";
                    SQL = SQL + ComNum.VBLF + "     '29991231',                                         ";
                    SQL = SQL + ComNum.VBLF + "     '" + ssView.ActiveSheet.Cells[i, 0].Text.Trim() + "', ";    //BASNAME
                    SQL = SQL + ComNum.VBLF + "     '" + ssView.ActiveSheet.Cells[i, 1].Text.Trim() + "', ";    //BASEXNAME                    
                    SQL = SQL + ComNum.VBLF + "     " + 0 + ", ";                                           //BASVAL
                    SQL = SQL + ComNum.VBLF + "     '" + arrGB[nGb] + "', ";   //REMARK
                    SQL = SQL + ComNum.VBLF + "     " + 0 + ", ";                                           //NFLAG1
                    SQL = SQL + ComNum.VBLF + "     '0', ";
                    SQL = SQL + ComNum.VBLF + "     '0', ";
                    SQL = SQL + ComNum.VBLF + "     '" + i.ToString() + "' "; //DISPSEQ
                    SQL = SQL + ComNum.VBLF + " ) ";
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

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void btnMng_Click(object sender, EventArgs e)
        {
            strFlag = "MANAGER";
            panMng.Visible = true;
            btnSave.Visible = true;
            btnMng.Visible = false;
            
            ssView.ActiveSheet.Columns[0].Visible = true;
            ssView.ActiveSheet.Columns[1].Visible = true;
            ssView.ActiveSheet.Columns[0].Locked = false;
            ssView.ActiveSheet.Columns[1].Locked = false;
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true) return;

            if (strFlag != "") return;

            string strName = ssView.ActiveSheet.Cells[e.Row, 0].Text;
            string strUnit = ssView.ActiveSheet.Cells[e.Row, 1].Text;

            rGetInfo(strName, strUnit);
        }

        private void GetGubunList()
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            
            ssList1.ActiveSheet.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT * FROM OPR_CODE oc                ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'B'                       ";
                SQL = SQL + ComNum.VBLF + "   AND SORT<> 99                         ";
                SQL = SQL + ComNum.VBLF + "   AND (CODE <= '08' OR CODE = '94')     ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssList1.ActiveSheet.RowCount = dt.Rows.Count;
                    ssList1.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["NAME"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CODE"].ToString().Trim();
                    }
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void ssList1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strBun = "";
            
            ssViewHelp_Sheet1.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            try
            {                
                strBun = ssList1_Sheet1.Cells[e.Row, 1].Text.Trim();
                
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT                                       ";
                SQL = SQL + ComNum.VBLF + "       A.JEPCODE, A.CODEGBN, A.NAME          ";
                SQL = SQL + ComNum.VBLF + "     , C.UNIT, D.BUSE_UNIT                   ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPR_BUSEJEPUM A           ";
                SQL = SQL + ComNum.VBLF + "  LEFT OUTER JOIN KOSMOS_PMPA.BAS_SUT B      ";
                SQL = SQL + ComNum.VBLF + "               ON A.JEPCODE = B.SUNEXT       ";
                SQL = SQL + ComNum.VBLF + "  LEFT OUTER JOIN KOSMOS_PMPA.BAS_SUN C      ";
                SQL = SQL + ComNum.VBLF + "               ON B.SUNEXT = C.SUNEXT        ";
                SQL = SQL + ComNum.VBLF + "  LEFT OUTER JOIN KOSMOS_ADM.ORD_JEP D       ";
                SQL = SQL + ComNum.VBLF + "               ON A.JEPCODE = D.JEPCODE      ";                
                SQL = SQL + ComNum.VBLF + "WHERE A.BUCODE = '033103' ";
                SQL = SQL + ComNum.VBLF + "  AND A.BUN = '" + strBun + "' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SEQNO,NAME ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssViewHelp_Sheet1.RowCount = dt.Rows.Count;
                    ssViewHelp_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["CODEGBN"].ToString().Trim() == "1")
                        {
                            ssViewHelp_Sheet1.Cells[i, 0].Text = dt.Rows[i]["NAME"].ToString().Trim();
                            ssViewHelp_Sheet1.Cells[i, 1].Text = dt.Rows[i]["UNIT"].ToString().Trim();
                        }
                        else
                        {
                            ssViewHelp_Sheet1.Cells[i, 0].Text = dt.Rows[i]["NAME"].ToString().Trim();
                            ssViewHelp_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BUSE_UNIT"].ToString().Trim();
                        }
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void ssViewHelp_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true) return;

            if (strFlag != "") return;

            string strName = ssViewHelp.ActiveSheet.Cells[e.Row, 0].Text;
            string strUnit = ssViewHelp.ActiveSheet.Cells[e.Row, 1].Text;

            rGetInfo(strName, strUnit);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (strFlag == "MANAGER")
            {
                tabControl1.SelectedIndex = 0;
            }
        }

        public void GetDataList2()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = "SELECT WRTNO,NAME ";
                SQL = SQL + ComNum.VBLF + " FROM OPR_GROUPMST ";
                SQL = SQL + ComNum.VBLF + "WHERE BUCODE = '033103' ";
                //SQL = SQL + ComNum.VBLF + "  AND (DEPTCODE = '**' OR DEPTCODE='" + strDeptCode + "') ";

                //if (txtViewData.Text.Trim() != "")
                //{
                //    SQL = SQL + ComNum.VBLF + " AND NAME LIKE '%" + txtViewData.Text.Trim() + "%' ";
                //}
                SQL = SQL + ComNum.VBLF + "ORDER BY NAME ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                ssList2_Sheet1.RowCount = 0;

                if (dt.Rows.Count > 0)
                {
                    ssList2_Sheet1.RowCount = dt.Rows.Count;
                    ssList2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["NAME"].ToString().Trim();
                        ssList2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                    }
                }
                dt.Dispose();
                dt = null;

                //txtViewData.Text = "";

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void ssList2_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            int nWrtno = 0;
            //string strName = "";

            ssViewHelp2_Sheet1.RowCount = 0;

            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //    return; //권한 확인

            Cursor.Current = Cursors.WaitCursor;

            try
            {                
                nWrtno = Convert.ToInt32(VB.Val(ssList2_Sheet1.Cells[e.Row, 1].Text.Trim()));

                //'해당 약속코드를 읽음;
                SQL = "SELECT A.SEQNO,A.JEPCODE,B.NAME, C.UNIT ";
                SQL = SQL + ComNum.VBLF + " FROM OPR_GROUPDTL A, OPR_BUSEJEPUM B, KOSMOS_PMPA.BAS_SUN C ";
                SQL = SQL + ComNum.VBLF + "WHERE A.WRTNO = " + nWrtno + " ";
                SQL = SQL + ComNum.VBLF + "  AND A.CODEGBN = B.CODEGBN(+) ";
                SQL = SQL + ComNum.VBLF + "  AND A.JEPCODE = B.JEPCODE(+) ";
                SQL = SQL + ComNum.VBLF + "  AND A.JEPCODE = C.SUNEXT(+) ";
                SQL = SQL + ComNum.VBLF + "  AND B.BUCODE = '033103' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY B.BUN,A.JEPCODE ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssViewHelp2_Sheet1.RowCount = dt.Rows.Count;
                    ssViewHelp2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {                        
                        ssViewHelp2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["NAME"].ToString().Trim();
                        ssViewHelp2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["UNIT"].ToString().Trim();                        
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void ssViewHelp2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true) return;

            if (strFlag != "") return;

            string strName = ssViewHelp2.ActiveSheet.Cells[e.Row, 0].Text;
            string strUnit = ssViewHelp2.ActiveSheet.Cells[e.Row, 1].Text;

            rGetInfo(strName, strUnit);
        }
    }
}
