using ComLibB;
using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스


namespace ComPmpaLibB
{

    /// <summary>
    /// Class Name : frmPmpaSimsaTarget
    /// File Name : frmPmpaSimsaTarget.cs
    /// Title or Description : 재원심사시 심사자별 대상 세팅작업 폼
    /// Author : 김민철
    /// Create Date : 2017-06-14
    /// Update History : 
    /// <seealso cref="d:\psmh\IPD\ipdSim2\Frm심사대상세팅.frm"/>
    /// </summary>

    public partial class frmPmpaSimsaTarget : Form
    {
        private string FstrSabun = "";
        private string FstrSName = "";
        private string FstrSeq = "";

        public frmPmpaSimsaTarget()
        {
            InitializeComponent();
        }

        private void frmPmpaSimsaTarget_Load(object sender, EventArgs e)
        {
            Screen_Clear_All();
        }

        private void Screen_Clear_All()
        {
            SSSeq.ActiveSheet.ClearRange(0, 0, SSSeq_Sheet1.Rows.Count, SSSeq_Sheet1.ColumnCount, false);
            SSList.ActiveSheet.ClearRange(0, 0, SSList_Sheet1.Rows.Count, SSList_Sheet1.ColumnCount, false);
            SSBi.ActiveSheet.ClearRange(0, 0, SSBi_Sheet1.Rows.Count, SSBi_Sheet1.ColumnCount, false);
            SSWard.ActiveSheet.ClearRange(0, 0, SSWard_Sheet1.Rows.Count, SSWard_Sheet1.ColumnCount, false);
            SSDept.ActiveSheet.ClearRange(0, 0, SSDept_Sheet1.Rows.Count, SSDept_Sheet1.ColumnCount, false);

            Display_Job_Sabun_Set();
            Display_Bi_Set();
            Display_Ward_Set();
            Display_DeptCode_Set();

            SSSeq_Sheet1.Rows.Count = 0;

            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            groupBox4.Enabled = false;

            lblName.Text = "";
            FstrSabun = "";
            FstrSName = "";
            FstrSeq = "";
        }

        private void Screen_Clear()
        {
            SSBi.ActiveSheet.ClearRange(0, 0, SSBi_Sheet1.Rows.Count, SSBi_Sheet1.ColumnCount, false);
            SSWard.ActiveSheet.ClearRange(0, 0, SSWard_Sheet1.Rows.Count, SSWard_Sheet1.ColumnCount, false);
            SSDept.ActiveSheet.ClearRange(0, 0, SSDept_Sheet1.Rows.Count, SSDept_Sheet1.ColumnCount, false);

            Display_Bi_Set();
            Display_Ward_Set();
            Display_DeptCode_Set();
        }

        private void Display_Job_Sabun_Set()
        {
            int i = 0;
            int nRow = 0;
            int nREAD = 0;

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SSList_Sheet1.Rows.Count = 0;

            try
            {
                //보험심사과 직원 세팅
                SQL = "";
                SQL += ComNum.VBLF + " SELECT a.SABUN, b.KORNAME ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ETC_JSIM_SET a, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_ERP + "INSA_MST b ";
                SQL += ComNum.VBLF + "  WHERE b.BUSE = '078201' ";
                SQL += ComNum.VBLF + "    AND b.TOIDAY IS NULL ";
                SQL += ComNum.VBLF + "    AND a.SABUN = b.SABUN(+) ";
                SQL += ComNum.VBLF + "  GROUP By a.SABUN, b.KORNAME ";
                SQL += ComNum.VBLF + "  ORDER By a.SABUN ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nREAD = Dt.Rows.Count;

                for (i = 0; i < nREAD; i++)
                {
                    nRow += 1;
                    if (SSList_Sheet1.Rows.Count < nRow) { SSList_Sheet1.Rows.Count = nRow; }

                    SSList_Sheet1.Cells[nRow - 1, 0].Text = Dt.Rows[i]["SABUN"].ToString().Trim();
                    SSList_Sheet1.Cells[nRow - 1, 1].Text = Dt.Rows[i]["KORNAME"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }
        }

        private void Display_Bi_Set()
        {
            int i = 0;
            int nRow = 0;
            int nREAD = 0;

            DataTable Dt = null;

            string SQL = "";    //Query문
            
            SSBi_Sheet1.Rows.Count = 0;

            try
            {
                //환자종류
                Dt = ComQuery.Set_BaseCode_Foundation(clsDB.DbCon, "BAS_환자종류", "");

                nREAD = Dt.Rows.Count;

                for (i = 0; i < nREAD; i++)
                {
                    nRow += 1;
                    if (SSBi_Sheet1.Rows.Count < nRow) { SSBi_Sheet1.Rows.Count = nRow; }

                    SSBi_Sheet1.Cells[nRow - 1, 0].Text = Dt.Rows[i]["CODE"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }
        }

        private void Display_Ward_Set()
        {
            int i = 0;
            int nRow = 0;
            int nREAD = 0;

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SSWard_Sheet1.Rows.Count = 0;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT WARDCODE,ROOMCODE ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ROOM ";
                SQL += ComNum.VBLF + "  WHERE TBED > 0 ";
                SQL += ComNum.VBLF + "  ORDER By WARDCODE,ROOMCODE ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nREAD = Dt.Rows.Count;

                for (i = 0; i < nREAD; i++)
                {
                    nRow += 1;
                    if (SSWard_Sheet1.Rows.Count < nRow) { SSWard_Sheet1.Rows.Count = nRow; }

                    SSWard_Sheet1.Cells[nRow - 1, 0].Text = Dt.Rows[i]["WARDCODE"].ToString().Trim();
                    SSWard_Sheet1.Cells[nRow - 1, 1].Text = Dt.Rows[i]["ROOMCODE"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }
        }

        private void Display_DeptCode_Set()
        {
            int i = 0;
            int nRow = 0;
            int nREAD = 0;

            DataTable Dt = new DataTable();

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            SSDept_Sheet1.Rows.Count = 0;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT DEPTCODE,DEPTNAMEK ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT ";
                SQL += ComNum.VBLF + "  WHERE DEPTCODE NOT IN ('PT','LM','II','R6','HR','TO') ";
                SQL += ComNum.VBLF + "  ORDER By DEPTCODE ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nREAD = Dt.Rows.Count;

                for (i = 0; i < nREAD; i++)
                {
                    nRow += 1;
                    if (SSDept_Sheet1.Rows.Count < nRow) { SSDept_Sheet1.Rows.Count = nRow; }

                    SSDept_Sheet1.Cells[nRow - 1, 0].Text = Dt.Rows[i]["DEPTCODE"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SSList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;            
            int nRow = 0;
            int nREAD = 0;
            
            DataTable Dt = new DataTable();

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (e.Row < 0 || e.Column < 0) { return; }

            Screen_Clear_All();

            SSSeq_Sheet1.Rows.Count = 0;

            groupBox4.Enabled = true;

            FstrSabun = SSList_Sheet1.Cells[e.Row, 0].Text;
            FstrSName = SSList_Sheet1.Cells[e.Row, 1].Text;

            lblName.Text = "대상자 : " + FstrSName;

            try
            {
                //보험심사과 직원 세팅
                SQL = "";
                SQL += ComNum.VBLF + " SELECT SEQNO ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ETC_JSIM_SET ";
                SQL += ComNum.VBLF + "  WHERE SABUN = '" + FstrSabun + "' ";
                SQL += ComNum.VBLF + "  GROUP By SEQNO ";                
                SQL += ComNum.VBLF + "  ORDER By SEQNO ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nREAD = Dt.Rows.Count;

                if (nREAD == 0)
                {
                    SSSeq_Sheet1.Rows.Count += 1;
                    SSSeq_Sheet1.Cells[i, 0].Text = "0";
                    lblName.Text += "  신규세팅";
                }

                else
                {
                    for (i = 0; i < nREAD; i++)
                    {
                        nRow += 1;
                        if (SSSeq_Sheet1.Rows.Count < nRow) { SSSeq_Sheet1.Rows.Count = nRow; }

                        SSSeq_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["SEQNO"].ToString().Trim();

                    }
                }
                
                Dt.Dispose();
                Dt = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }
        }

        private string Get_Seq_No(string strSabun)
        {
            string rtnSeq = "";

            DataTable Dt = new DataTable();

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT MAX(SEQNO) MSEQ ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ETC_JSIM_SET ";
                SQL += ComNum.VBLF + "  WHERE SABUN = '" + FstrSabun + "' ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return "";
                }
                if (Dt.Rows.Count == 0)
                {
                    rtnSeq = "";
                }
                else
                {
                    rtnSeq = Dt.Rows[0]["MSEQ"].ToString().Trim();
                }

                rtnSeq = (Convert.ToInt16(rtnSeq) + 1).ToString();

                Dt.Dispose();
                Dt = null;

                return rtnSeq;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                return "";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int i = 0;
            string strTemp = "";
            string strSeq = "";

            if (FstrSabun == "") { return; }

            strSeq = Get_Seq_No(FstrSabun);

            if (strSeq == "")
            {
                FstrSeq = "0";
            }
            else
            {
                Delete_Jsim_Set(FstrSabun, FstrSeq);
            }

            //환자구분
            for (i = 0; i < SSBi_Sheet1.Rows.Count; i++)
            {
                if (Convert.ToBoolean(SSBi_Sheet1.Cells[i, 1].Value))
                {
                    strTemp = SSBi_Sheet1.Cells[i, 0].Text;
                    Insert_Jsim_Set("1", strTemp, FstrSeq);
                }
            }

            //병동병실 구분
            for (i = 0; i < SSWard_Sheet1.Rows.Count; i++)
            {
                if (Convert.ToBoolean(SSWard_Sheet1.Cells[i, 2].Value))
                {
                    strTemp = SSWard_Sheet1.Cells[i, 0].Text;
                    strTemp += "/" + SSWard_Sheet1.Cells[i, 1].Text;
                    Insert_Jsim_Set("2", strTemp, FstrSeq);
                }
            }

            //진료과 구분
            for (i = 0; i < SSDept_Sheet1.Rows.Count; i++)
            {
                if (Convert.ToBoolean(SSDept_Sheet1.Cells[i, 1].Value))
                {
                    strTemp = SSDept_Sheet1.Cells[i, 0].Text;
                    Insert_Jsim_Set("3", strTemp, FstrSeq);
                }
            }

            Screen_Clear_All();

        }

        private void Insert_Jsim_Set(string strGbn, string strRemark, string strSeq)
        {
            DataTable Dt = new DataTable();

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            int intRowAffected = 0; //변경된 Row 받는 변수
            
             

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "ETC_JSIM_SET (";
                SQL += ComNum.VBLF + " SABUN,GUBUN,REMARK,ENTDATE,Seqno) VALUES (";
                SQL += ComNum.VBLF + " " + FstrSabun + ",'" + strGbn + "','" + strRemark + "',SYSDATE," + strSeq + ") ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                
                clsDB.setCommitTran(clsDB.DbCon);

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }
            
        }

        private void Delete_Jsim_Set(string strSabun, string strSeq)
        {
            DataTable Dt = new DataTable();

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            int intRowAffected = 0; //변경된 Row 받는 변수

             

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " DELETE FROM " + ComNum.DB_PMPA + "ETC_JSIM_SET ";
                SQL += ComNum.VBLF + "  WHERE SABUN = '" + strSabun + "' ";
                SQL += ComNum.VBLF + "    AND SEQNO = '" + strSeq + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }
        }

        private void SSSeq_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;
            int j = 0;
            int nREAD = 0;

            DataTable Dt = new DataTable();

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (e.Row < 0 || e.Column < 0) { return; }

            FstrSeq = SSSeq_Sheet1.Cells[e.Row, 0].Text;
            
            lblName.Text = lblName.Text = "대상자 : " + FstrSName + "  " + FstrSeq + "번 세팅";

            Screen_Clear();

            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
            groupBox3.Enabled = true;

            try
            {
                //보험심사과 직원 세팅
                SQL = "";
                SQL += ComNum.VBLF + " SELECT SABUN,GUBUN,REMARK ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ETC_JSIM_SET ";
                SQL += ComNum.VBLF + "  WHERE SABUN = '" + FstrSabun + "' ";
                SQL += ComNum.VBLF + "    AND SEQNO = '" + FstrSeq + "' ";
                SQL += ComNum.VBLF + "  ORDER By REMARK ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nREAD = Dt.Rows.Count;

                if (nREAD == 0)
                {
                    lblName.Text = "대상자 : " + FstrSName + "  신규세팅";
                    Dt.Dispose();
                    Dt = null;
                    return;
                }

                for (i = 0; i < nREAD; i++)
                {
                    if (Dt.Rows[i]["GUBUN"].ToString().Trim() == "1")           //환자자격
                    {
                        for (j = 0; j < SSBi_Sheet1.Rows.Count; j++)
                        {
                            if (Dt.Rows[i]["REMARK"].ToString().Trim() == SSBi_Sheet1.Cells[j, 0].Text)
                            {
                                SSBi_Sheet1.Cells[j, 1].Value = true;
                            }
                        }
                    }
                    else if (Dt.Rows[i]["GUBUN"].ToString().Trim() == "2")      //병동병실
                    {
                        for (j = 0; j < SSWard_Sheet1.Rows.Count; j++)
                        {
                            if (VB.SinglePiece(Dt.Rows[i]["REMARK"].ToString().Trim(), "/", 1) == SSWard_Sheet1.Cells[j, 0].Text)
                            {
                                if (VB.SinglePiece(Dt.Rows[i]["REMARK"].ToString().Trim(), "/", 2) == SSWard_Sheet1.Cells[j, 1].Text)
                                {
                                    SSWard_Sheet1.Cells[j, 2].Value = true;
                                }
                            }
                        }
                    }
                    else if (Dt.Rows[i]["GUBUN"].ToString().Trim() == "3")      //진료과
                    {
                        for (j = 0; j < SSDept_Sheet1.Rows.Count; j++)
                        {
                            if (Dt.Rows[i]["REMARK"].ToString().Trim() == SSDept_Sheet1.Cells[j, 0].Text)
                            {
                                SSDept_Sheet1.Cells[j, 1].Value = true;
                            }
                        }
                    }
                }

                Dt.Dispose();
                Dt = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SSSeq_Sheet1.Rows.Count += 1;
            SSSeq_Sheet1.Cells[SSSeq_Sheet1.Rows.Count - 1, 0].Text = (SSSeq_Sheet1.Rows.Count - 1).ToString();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Delete_Jsim_Set(FstrSabun, FstrSeq);
            Screen_Clear_All();
        }
    }
}
