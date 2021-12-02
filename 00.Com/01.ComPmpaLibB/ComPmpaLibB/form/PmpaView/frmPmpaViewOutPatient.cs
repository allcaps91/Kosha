using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    public partial class frmPmpaViewOutPatient : Form
    {

        /// <summary>
        /// Class Name      : ComPmpaLibB
        /// File Name       : frmPmpaViewOutPatient.cs
        /// Description     : 퇴원자 조회
        /// Author          : 김효성
        /// Create Date     : 2017-08-22
        /// Update History  : 
        /// </summary>
        /// <history>  
        /// 
        /// </history>
        /// <seealso cref= "psmh\IPD\iument\FrmOutPatient(IVEWA8.FRM) >> frmPmpaViewOutPatient.cs 폼이름 재정의" />	

        string strFlag = "";
        string nSELECT = "";
        string strActDate = "";
        string strOptSql = "";
        string SQL = "";
        string[] strBis = new string[100];
        string[] GstrSETBis = new string[56];
        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmPmpaViewOutPatient(string trFlag, string SELECT, string trActDat, string trOptSql, string QL, string[] trBis, string[] strSETBis)
        {
            strFlag = trFlag;
            nSELECT = SELECT;
            strActDate = trActDat;
            strOptSql = trOptSql;
            SQL = QL;
            strBis = trBis;
            GstrSETBis = strSETBis;

            InitializeComponent();
        }


        public frmPmpaViewOutPatient()
        {
            InitializeComponent();
        }

        private void frmPmpaViewOutPatient_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{ this.Close(); return; } //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            Load_Bi_IDs();
        }

        private void Load_Bi_IDs()
        {
            int i = 0;

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            for (i = 0; i < 100; i++)
            {
                strBis[i] = "";
            }

            for (i = 0; i < 56; i++)
            {
                GstrSETBis[i] = "";
            }


            strBis[11] = "공단";
            strBis[12] = "직장";
            strBis[13] = "지역";
            strBis[14] = "";
            strBis[15] = "";

            strBis[21] = "보호1";
            strBis[22] = "보호2";
            strBis[23] = "보호3";
            strBis[24] = "행려";
            strBis[25] = "";

            strBis[31] = "산재";
            strBis[32] = "공상";
            strBis[33] = "산재공상";
            strBis[34] = "";
            strBis[35] = "";

            strBis[41] = "공단180";
            strBis[42] = "직장180";
            strBis[43] = "지역180";
            strBis[44] = "가족계획";
            strBis[45] = "보험계약";

            strBis[51] = "일반";
            strBis[52] = "자보";
            strBis[53] = "계약";
            strBis[54] = "미확인";
            strBis[55] = "자보일반";

            GstrSETBis[11] = "11.공단";
            GstrSETBis[12] = "12.연합회";
            GstrSETBis[13] = "13.지역";
            GstrSETBis[21] = "21.보호1종";
            GstrSETBis[22] = "22.보호2종";
            GstrSETBis[23] = "23.의료부조";
            GstrSETBis[24] = "24.행려환자";
            GstrSETBis[31] = "31.산재";
            GstrSETBis[32] = "32.공무원공상";
            GstrSETBis[33] = "33.산재공상";
            GstrSETBis[41] = "41.공단100%";
            GstrSETBis[42] = "42.직장100%";
            GstrSETBis[43] = "43.지역100%";
            GstrSETBis[44] = "44.가족계획";
            GstrSETBis[45] = "45.보험계약";
            GstrSETBis[51] = "51.일반";
            GstrSETBis[52] = "52.TA보험";
            GstrSETBis[53] = "53.계약";
            GstrSETBis[54] = "54.미확인";
            GstrSETBis[55] = "55.TA일반";

            cbobi.Items.Add("00.전체");

            for (i = 11; i <= 55; i++)
            {
                if (GstrSETBis[i] != "")
                {
                    cbobi.Items.Add(GstrSETBis[i]);
                }
            }

            cbobi.SelectedIndex = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {


                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT DEPTCODE, DEPTNAMEK        ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY PRINTRANKING            ";

                SQL = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                cboDetp.Items.Add("**.전체");
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboDetp.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim() + "." + dt.Rows[i]["DEPTNAMEK"].ToString().Trim());
                }
                cboDetp.SelectedIndex = 0;

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;

                }

                dtpFTdate.Value = DateTime.Parse(strDTP).AddDays(-10);
                dtpTDate.Value = DateTime.Parse(strDTP);
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSearch01_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            int i = 0;
            int j = 0;
            int K = 0;
            string strBi = "";
            int nTotRow = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            ssView01_Sheet1.RowCount = 0;
            ssView01_Sheet1.RowCount = 20;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Pano,Bi,Sname,Pname,Sex,Age,                                             ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(InDate, 'YYYY-MM-DD') InDate,                                    ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(OutDate, 'YYYY-MM-DD') OutDate,DeptCode,DrName                   ";

                if ((DateTime.Parse(strDTP) == dtpFTdate.Value) && (DateTime.Parse(strDTP) == dtpTDate.Value))
                {
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER i," + ComNum.DB_PMPA + "BAS_DOCTOR k  ";
                    SQL = SQL + ComNum.VBLF + "  WHERE I.GBSTS IN ('0','2')                                                       ";
                    SQL = SQL + ComNum.VBLF + "    AND I.OUTDATE IS NULL";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "        , TO_CHAR(Actdate,'YYYY-MM-DD') ACTDATE FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER i," + ComNum.DB_PMPA + "BAS_DOCTOR k   ";
                    SQL = SQL + ComNum.VBLF + "  WHERE I.ActDate >= TO_DATE('" + dtpFTdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')             ";
                    SQL = SQL + ComNum.VBLF + "    AND I.ActDate <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')             ";
                }

                if (VB.Left(cboDetp.Text, 2) != "**")
                {
                    SQL = SQL + ComNum.VBLF + "    AND i.DEPTCODE  = '" + VB.Left(cboDetp.Text, 2) + "'                      ";
                }

                if (VB.Left(cbobi.Text, 2) != "00")
                {
                    SQL = SQL + ComNum.VBLF + "    AND i.Bi  = '" + VB.Left(cbobi.Text, 2) + "'                              ";
                }

                SQL = SQL + ComNum.VBLF + "    AND i.DrCode = k.DrCode(+)                                                   ";

                if ((DateTime.Parse(strDTP) == dtpFTdate.Value) && (DateTime.Parse(strDTP) == dtpTDate.Value))
                {
                    SQL = SQL + ComNum.VBLF + "  ORDER BY Sname                                                             ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  ORDER BY ActDate, Sname";
                }

                SQL = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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
                    ComFunc.MsgBox("해당일자 퇴원자 없음.", "확인");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //스프레드 출력문
                ssView01_Sheet1.RowCount = ssView01_Sheet1.RowCount + dt.Rows.Count - 20;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strBi = dt.Rows[i]["BI"].ToString().Trim();

                    if ((DateTime.Parse(strDTP) == dtpFTdate.Value) && (DateTime.Parse(strDTP) == dtpTDate.Value))
                    {
                        ssView01_Sheet1.Cells[i, 0].Text = strDTP;
                    }
                    else
                    {
                        ssView01_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ACTDATE"].ToString().Trim();
                    }
                    ssView01_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssView01_Sheet1.Cells[i, 2].Text = strBis[Convert.ToInt32(VB.Val(strBi))];
                    ssView01_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssView01_Sheet1.Cells[i, 4].Text = dt.Rows[i]["PNAME"].ToString().Trim();
                    ssView01_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SEX"].ToString().Trim();
                    ssView01_Sheet1.Cells[i, 6].Text = dt.Rows[i]["AGE"].ToString().Trim();
                    ssView01_Sheet1.Cells[i, 7].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                    ssView01_Sheet1.Cells[i, 8].Text = dt.Rows[i]["OUTDATE"].ToString().Trim();
                    ssView01_Sheet1.Cells[i, 9].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    ssView01_Sheet1.Cells[i, 10].Text = dt.Rows[i]["DRNAME"].ToString().Trim();

                }
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSearch02_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            int i = 0;
            int j = 0;
            int k = 0;
            int nTotRow = 0;
            string strBi = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            if (txtSname.Text == "")
            {
                return;
            }

            ssView02_Sheet1.RowCount = 0;
            ssView02_Sheet1.RowCount = 20;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Pano,Bi,Sname,Pname,Sex,Age,                                           ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(InDate, 'yy-mm-dd') InDate,                                    ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(OutDate, 'yy-mm-dd') OutDate,DeptCode,DrName,                  ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(ACTDATE, 'yy-mm-dd') ACTDATE                                    ";
                SQL = SQL + ComNum.VBLF + "   FROM IPD_NEW_MASTER i,BAS_DOCTOR k                                          ";
                SQL = SQL + ComNum.VBLF + "  WHERE OUTDATE IS NOT NULL                                                    ";
                SQL = SQL + ComNum.VBLF + "    AND Sname LIKE '" + txtSname.Text + "%'                                    ";
                SQL = SQL + ComNum.VBLF + "    AND i.DrCode = k.DrCode(+)                                                 ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY Sname                                                               ";


                SQL = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;

                }

                if (k == 1)
                {
                    ssView02_Sheet1.RowCount = ssView02_Sheet1.RowCount + dt.Rows.Count - 20;

                    ssView02_Sheet1.RowCount = dt.Rows.Count;
                    ssView02_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SPREAD_SET1(strBi, ref nTotRow, dt, i);
                    }
                }
                else
                {
                    ssView02_Sheet1.RowCount = ssView02_Sheet1.RowCount + dt.Rows.Count - 20;

                    ssView02_Sheet1.RowCount = dt.Rows.Count;
                    ssView02_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SPREAD_SET1(strBi, ref nTotRow, dt, i);
                    }
                }
                //스프레드 출력문


                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void SPREAD_SET1(string strBi, ref int nTotRow, DataTable dtFunc, int i)
        {
            strBi = dtFunc.Rows[i]["BI"].ToString().Trim();
            nTotRow = nTotRow + 1;

            ssView02_Sheet1.Cells[nTotRow - 1, 0].Text = dtFunc.Rows[i]["ACTDATE"].ToString().Trim();
            ssView02_Sheet1.Cells[nTotRow - 1, 1].Text = dtFunc.Rows[i]["PANO"].ToString().Trim();
            ssView02_Sheet1.Cells[nTotRow - 1, 2].Text = strBis[Convert.ToInt32(VB.Val(strBi))];
            ssView02_Sheet1.Cells[nTotRow - 1, 3].Text = dtFunc.Rows[i]["SNAME"].ToString().Trim();
            ssView02_Sheet1.Cells[nTotRow - 1, 4].Text = dtFunc.Rows[i]["PNAME"].ToString().Trim();
            ssView02_Sheet1.Cells[nTotRow - 1, 5].Text = dtFunc.Rows[i]["SEX"].ToString().Trim();
            ssView02_Sheet1.Cells[nTotRow - 1, 6].Text = dtFunc.Rows[i]["AGE"].ToString().Trim();
            ssView02_Sheet1.Cells[nTotRow - 1, 7].Text = dtFunc.Rows[i]["INDATE"].ToString().Trim();
            ssView02_Sheet1.Cells[nTotRow - 1, 8].Text = dtFunc.Rows[i]["OUTDATE"].ToString().Trim();
            ssView02_Sheet1.Cells[nTotRow - 1, 9].Text = dtFunc.Rows[i]["DEPTCODE"].ToString().Trim();
            ssView02_Sheet1.Cells[nTotRow - 1, 10].Text = dtFunc.Rows[i]["DRNAME"].ToString().Trim();

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSname.Text = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    txtSname.Focus();
                    break;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
