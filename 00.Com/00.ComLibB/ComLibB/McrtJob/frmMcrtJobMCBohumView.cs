using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmMcrtJobMCBohumView : Form
    {
        /// <summary>
        /// 닫기 이벤트
        /// </summary>
        public delegate void CloseEvent();
        public event CloseEvent rClosed;

        string fstrROWID = "";
        string FstrGUBUN = "";

        public frmMcrtJobMCBohumView()
        {
            InitializeComponent();
        }

        private void frmMcrtJobMCBohumView_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            ComFunc.SetIMEMODE(this, "K");
            lblKorE.Text = "한글";

            DateTime DT = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));

            dtpEDATE.Value = DT;
            dtpSDATE.Value = DT.AddDays(-10);

            txtPTNO.Text = "";
            txtDrCODE.Text = "";

            ss2.Visible = false;
            ss3.Visible = false;
            tab1.Visible = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            GetSearchData();
        }

        void GetSearchData()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            fstrROWID = "";
            FstrGUBUN = "";

            ss5_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT BDATE, PTNO, WRITESABUN, TITLE, '1' GUBUN, ROWID";
                SQL += ComNum.VBLF + "   FROM ADMIN.OCS_MCCERTIFI_BOHUM1";
                SQL += ComNum.VBLF + " WHERE BDATE >= TO_DATE('" + dtpSDATE.Text.Trim() + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "      AND BDATE <= TO_DATE('" + dtpEDATE.Text.Trim() + "','YYYY-MM-DD')";
                if (txtDrCODE.Text.Trim() != "") SQL += ComNum.VBLF + "      AND DRCODE IN (" + READ_DRCODE(txtDrCODE.Text) + ")";
                if (txtPTNO.Text.Trim() != "") SQL += ComNum.VBLF + "      AND PTNO = '" + txtPTNO.Text.Trim() + "'";
                SQL += ComNum.VBLF + " Union All";
                SQL += ComNum.VBLF + " SELECT BDATE, PTNO, WRITESABUN, TITLE, '2' GUBUN, ROWID";
                SQL += ComNum.VBLF + "   FROM ADMIN.OCS_MCCERTIFI_BOHUM2";
                SQL += ComNum.VBLF + " WHERE BDATE >= TO_DATE('" + dtpSDATE.Text.Trim() + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "      AND BDATE <= TO_DATE('" + dtpEDATE.Text.Trim() + "','YYYY-MM-DD')";
                if (txtDrCODE.Text.Trim() != "") SQL += ComNum.VBLF + "      AND DRCODE IN (" + READ_DRCODE(txtDrCODE.Text) + ")";
                if (txtPTNO.Text.Trim() != "") SQL += ComNum.VBLF + "      AND PTNO = '" + txtPTNO.Text.Trim() + "'";
                SQL += ComNum.VBLF + " Union All";
                SQL += ComNum.VBLF + " SELECT BDATE, PTNO, WRITESABUN, TITLE, '3' GUBUN, ROWID";
                SQL += ComNum.VBLF + "  From ADMIN.OCS_MCCERTIFI_BOHUM3";
                SQL += ComNum.VBLF + " WHERE BDATE >= TO_DATE('" + dtpSDATE.Text.Trim() + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "      AND BDATE <= TO_DATE('" + dtpEDATE.Text.Trim() + "','YYYY-MM-DD')";
                if (txtDrCODE.Text.Trim() != "") SQL += ComNum.VBLF + "      AND DRCODE IN (" + READ_DRCODE(txtDrCODE.Text) + ")";
                if (txtPTNO.Text.Trim() != "") SQL += ComNum.VBLF + "      AND PTNO = '" + txtPTNO.Text.Trim() + "'";
                SQL += ComNum.VBLF + " Union All";
                SQL += ComNum.VBLF + " SELECT BDATE, PTNO, WRITESABUN, '성인 ADHD 소견서', '4' GUBUN, ROWID";
                SQL += ComNum.VBLF + "  From ADMIN.OCS_MCCERTIFI_BOHUM4";
                SQL += ComNum.VBLF + " WHERE BDATE >= TO_DATE('" + dtpSDATE.Text.Trim() + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "      AND BDATE <= TO_DATE('" + dtpEDATE.Text.Trim() + "','YYYY-MM-DD')";
                if (txtDrCODE.Text.Trim() != "")  SQL += ComNum.VBLF + "      AND DRCODE IN (" + READ_DRCODE(txtDrCODE.Text) + ")";
                if (txtPTNO.Text.Trim() != "")  SQL += ComNum.VBLF + "      AND PTNO = '" + txtPTNO.Text.Trim() + "'";
                SQL += ComNum.VBLF + " ORDER BY BDATE DESC";
                SQL += ComNum.VBLF + "";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
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

                ss5_Sheet1.RowCount = dt.Rows.Count;
                ss5_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    ss5_Sheet1.Cells[i, 0].Text = ComFunc.FormatStrToDateTime(dt.Rows[i]["BDATE"].ToString().Trim() ,"D");
                    ss5_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                    ss5_Sheet1.Cells[i, 2].Text = dt.Rows[i]["TITLE"].ToString().Trim();
                    ss5_Sheet1.Cells[i, 3].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["WRITESABUN"].ToString().Trim());
                    ss5_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    ss5_Sheet1.Cells[i, 5].Text = dt.Rows[i]["GUBUN"].ToString().Trim();
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

        string READ_DRCODE(string arg)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string returnVal = "";

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT DRNAME ";
                SQL += ComNum.VBLF + " FROM ADMIN.OCS_DOCTOR";
                SQL += ComNum.VBLF + " WHERE DRCODE = '" + arg + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
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

                returnVal = dt.Rows[0]["DRNAME"].ToString().Trim();
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return returnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return returnVal;
            }
        }

        void READ_SS1(string strRowid)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            //string strPtno = "";

            CLEAR_SS1();
            string strPano = "";

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT * ";
                SQL += ComNum.VBLF + " FROM ADMIN.OCS_MCCERTIFI_BOHUM1 ";
                SQL += ComNum.VBLF + " WHERE ROWID = '" + strRowid + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
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

                strPano = dt.Rows[0]["PTNO"].ToString().Trim();

                SET_TEXT(ss1, 3, 6, clsVbfunc.GetPatientName(clsDB.DbCon, strPano));
                SET_TEXT(ss1, 3, 11, Read_PatientSex(strPano));
                SET_TEXT(ss1, 3, 14, clsVbfunc.READ_AGE_GESAN(clsDB.DbCon, strPano));
                SET_TEXT(ss1, 3, 22, ComFunc.FormatStrToDateTime(dt.Rows[0]["BDATE"].ToString().Trim(), "D"));
                SET_TEXT(ss1, 3, 32, dt.Rows[0]["JNO"].ToString().Trim());

                SET_TEXT(ss1, 4, 6, strPano);
                SET_TEXT(ss1, 4, 14, dt.Rows[0]["DEPTCODE"].ToString().Trim());
                SET_TEXT(ss1, 4, 22, dt.Rows[0]["BI"].ToString().Trim());
                SET_TEXT(ss1, 4, 32, dt.Rows[0]["SNO"].ToString().Trim());

                SET_TEXT(ss1, 5, 6, dt.Rows[0]["ILLNAMEK"].ToString().Trim());
                SET_TEXT(ss1, 6, 6, dt.Rows[0]["TITLE"].ToString().Trim());

                SET_TEXT(ss1, 9, 12, dt.Rows[0]["DATA_1_1"].ToString().Trim());

                SET_VALUE(ss1, 12, 3, dt.Rows[0]["DATA_2_1_1"].ToString().Trim());
                SET_TEXT(ss1, 12, 10, dt.Rows[0]["DATA_2_1_2"].ToString().Trim());

                SET_VALUE(ss1, 14, 3, dt.Rows[0]["DATA_2_2_1"].ToString().Trim());
                SET_TEXT(ss1, 14, 10, dt.Rows[0]["DATA_2_2_2"].ToString().Trim());
                //'SET_TEXT(ss1, 15, 23, dt.Rows[0]["DATA_2_2_3"].ToString().Trim();

                SET_VALUE(ss1, 16, 3, dt.Rows[0]["DATA_2_3_1"].ToString().Trim());
                SET_TEXT(ss1, 16, 7, dt.Rows[0]["DATA_2_3_2"].ToString().Trim());
                //'SET_TEXT(ss1, 17, 18, dt.Rows[0]["DATA_2_3_3"].ToString().Trim();
                //'추가
                SET_TEXT(ss1, 16, 21, dt.Rows[0]["DATA_2_2_3"].ToString().Trim());

                SET_VALUE(ss1, 18, 3, dt.Rows[0]["DATA_2_4_1"].ToString().Trim());
                SET_TEXT(ss1, 18, 9, dt.Rows[0]["DATA_2_4_2"].ToString().Trim());

                SET_VALUE(ss1, 21, 3, dt.Rows[0]["DATA_3_1_1"].ToString().Trim());
                SET_TEXT(ss1, 21, 8, dt.Rows[0]["DATA_3_1_2"].ToString().Trim());

                SET_VALUE(ss1, 23, 3, dt.Rows[0]["DATA_3_2_1"].ToString().Trim());

                SET_VALUE(ss1, 25, 3, dt.Rows[0]["DATA_3_3_1"].ToString().Trim());

                SET_VALUE(ss1, 27, 3, dt.Rows[0]["DATA_3_4_1"].ToString().Trim());
                SET_TEXT(ss1, 27, 13, dt.Rows[0]["DATA_3_4_2"].ToString().Trim());

                SET_VALUE(ss1, 29, 3, dt.Rows[0]["DATA_3_5_1"].ToString().Trim());
                SET_TEXT(ss1, 29, 8, dt.Rows[0]["DATA_3_5_2"].ToString().Trim());

                SET_TEXT(ss1, 34, 7, dt.Rows[0]["DATA_N_1"].ToString().Trim());
                SET_TEXT(ss1, 40, 2, dt.Rows[0]["DATA_N_2"].ToString().Trim());
                SET_TEXT(ss1, 40, 11, dt.Rows[0]["DATA_N_3"].ToString().Trim());

                SET_TEXT(ss1, 46, 6, dt.Rows[0]["DRBUNHO"].ToString().Trim());

                SET_TEXT(ss1, 46, 25, clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["WRITESABUN"].ToString().Trim()));


                SetDrSign(ss1, 44, 31, ComFunc.SetAutoZero(dt.Rows[0]["WRITESABUN"].ToString().Trim(), 5));


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

        void CLEAR_SS1()
        {
            SET_TEXT(ss1, 3, 6, "");
            SET_TEXT(ss1, 3, 11, "");
            SET_TEXT(ss1, 3, 14, "");
            SET_TEXT(ss1, 3, 22, "");
            SET_TEXT(ss1, 3, 32, "");

            SET_TEXT(ss1, 4, 6, "");
            SET_TEXT(ss1, 4, 14, "");
            SET_TEXT(ss1, 4, 22, "");
            SET_TEXT(ss1, 4, 32, "");

            SET_TEXT(ss1, 5, 6, "");
            //'SET_TEXT(ss1, 7, 7, "");

            SET_TEXT(ss1, 9, 12, "");

            SET_VALUE(ss1, 12, 4, "0");
            SET_TEXT(ss1, 12, 11, "");

            SET_VALUE(ss1, 14, 3, "0");
            SET_TEXT(ss1, 14, 10, "");
            //'SET_TEXT(ss1, 15, 23, "");


            SET_VALUE(ss1, 16, 3, "0");
            SET_TEXT(ss1, 16, 7, "");
            //'SET_TEXT(ss1, 17, 18, "");
            //'추가
            SET_TEXT(ss1, 16, 21, "");

            SET_VALUE(ss1, 18, 3, "0");
            SET_TEXT(ss1, 18, 9, "");

            SET_VALUE(ss1, 21, 3, "0");
            SET_TEXT(ss1, 21, 8, "");

            SET_VALUE(ss1, 23, 3, 0);

            SET_VALUE(ss1, 25, 3, 0);

            SET_VALUE(ss1, 27, 3, 0);
            SET_TEXT(ss1, 27, 13, "");

            SET_VALUE(ss1, 29, 3, 0);
            SET_TEXT(ss1, 29, 8, "");

            SET_TEXT(ss1, 34, 7, "");

            SET_TEXT(ss1, 40, 2, "");
            SET_TEXT(ss1, 40, 11, "");

            SET_TEXT(ss1, 46, 6, "");

            SET_TEXT(ss1, 46, 25, "");


            SetDrSign(ss1, 44, 31, ComFunc.SetAutoZero(clsType.User.Sabun, 5));

            SET_TEXT(ss1, 44, 25, "");

        }

        void READ_SS2(string argROWID)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strPtno = "";

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT * ";
                SQL += ComNum.VBLF + " FROM ADMIN.OCS_MCCERTIFI_BOHUM2 ";
                SQL += ComNum.VBLF + " WHERE ROWID = '" + argROWID + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
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

                strPtno = dt.Rows[0]["PTNO"].ToString().Trim();

                SET_TEXT(ss2, 3, 6, clsVbfunc.GetPatientName(clsDB.DbCon, strPtno));
                SET_TEXT(ss2, 3, 11, Read_PatientSex(strPtno));
                SET_TEXT(ss2, 3, 14, clsVbfunc.READ_AGE_GESAN(clsDB.DbCon, strPtno));
                SET_TEXT(ss2, 3, 22, ComFunc.FormatStrToDateTime(dt.Rows[0]["BDATE"].ToString().Trim(), "D"));
                SET_TEXT(ss2, 3, 32, dt.Rows[0]["JNO"].ToString().Trim());

                SET_TEXT(ss2, 4, 6, dt.Rows[0]["PTNO"].ToString().Trim());
                SET_TEXT(ss2, 4, 14, dt.Rows[0]["DEPTCODE"].ToString().Trim());
                SET_TEXT(ss2, 4, 22, dt.Rows[0]["BI"].ToString().Trim());
                SET_TEXT(ss2, 4, 32, dt.Rows[0]["SNO"].ToString().Trim());

                SET_TEXT(ss2, 5, 6, dt.Rows[0]["ILLNAMEK"].ToString().Trim());
                SET_TEXT(ss2, 6, 6, dt.Rows[0]["TITLE"].ToString().Trim());

                SET_VALUE(ss2, 15, 3, dt.Rows[0]["DATA_1_1_1"].ToString().Trim());
                SET_TEXT(ss2, 15, 8, dt.Rows[0]["DATA_1_1_2"].ToString().Trim());

                SET_VALUE(ss2, 17, 3, dt.Rows[0]["DATA_1_2_1"].ToString().Trim());
                SET_TEXT(ss2, 17, 8, dt.Rows[0]["DATA_1_2_2"].ToString().Trim());

                SET_VALUE(ss2, 19, 3, dt.Rows[0]["DATA_1_3_1"].ToString().Trim());
                SET_TEXT(ss2, 19, 8, dt.Rows[0]["DATA_1_3_2"].ToString().Trim());
                SET_TEXT(ss2, 19, 19, dt.Rows[0]["DATA_1_3_3"].ToString().Trim());
                SET_TEXT(ss2, 19, 27, dt.Rows[0]["DATA_1_3_4"].ToString().Trim());

                SET_VALUE(ss2, 21, 3, dt.Rows[0]["DATA_1_4_1"].ToString().Trim());
                SET_TEXT(ss2, 21, 9, dt.Rows[0]["DATA_1_4_2"].ToString().Trim());
                SET_TEXT(ss2, 21, 17, dt.Rows[0]["DATA_1_4_3"].ToString().Trim());
                SET_TEXT(ss2, 21, 30, dt.Rows[0]["DATA_1_4_4"].ToString().Trim());

                SET_VALUE(ss2, 25, 3, dt.Rows[0]["DATA_2_1"].ToString().Trim());
                SET_VALUE(ss2, 27, 3, dt.Rows[0]["DATA_2_2"].ToString().Trim());
                SET_VALUE(ss2, 28, 3, dt.Rows[0]["DATA_2_3"].ToString().Trim());
                SET_VALUE(ss2, 29, 3, dt.Rows[0]["DATA_2_4"].ToString().Trim());

                SET_TEXT(ss2, 32, 7, dt.Rows[0]["DATA_N_1"].ToString().Trim());
                SET_TEXT(ss2, 32, 28, dt.Rows[0]["DATA_N_2"].ToString().Trim());
                SET_TEXT(ss2, 33, 27, dt.Rows[0]["DATA_N_3"].ToString().Trim());

                SET_TEXT(ss2, 39, 6, dt.Rows[0]["DRBUNHO"].ToString().Trim());
                SET_TEXT(ss2, 39, 25, clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["WRITESABUN"].ToString().Trim()));
                SetDrSign(ss2, 37, 31, ComFunc.SetAutoZero(dt.Rows[0]["WRITESABUN"].ToString().Trim(), 5));

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

        void CLEAR_SS2()
        {
            SET_TEXT(ss2, 3, 6, "");
            SET_TEXT(ss2, 3, 11, "");
            SET_TEXT(ss2, 3, 14, "");
            SET_TEXT(ss2, 3, 22, "");
            SET_TEXT(ss2, 3, 32, "");

            SET_TEXT(ss2, 4, 6, "");
            SET_TEXT(ss2, 4, 14, "");
            SET_TEXT(ss2, 4, 22, "");
            SET_TEXT(ss2, 4, 32, "");

            SET_TEXT(ss2, 5, 6, "");
            //'SET_TEXT(ss2, 7, 7, "")

            SET_VALUE(ss2, 15, 3, 0);
            SET_TEXT(ss2, 15, 8, "");

            SET_VALUE(ss2, 17, 3, 0);
            SET_TEXT(ss2, 17, 8, "");

            SET_VALUE(ss2, 19, 3, 0);
            SET_TEXT(ss2, 19, 8, "");
            SET_TEXT(ss2, 19, 19, "");
            SET_TEXT(ss2, 19, 27, "");

            SET_VALUE(ss2, 21, 3, 0);
            SET_TEXT(ss2, 21, 9, "");
            SET_TEXT(ss2, 21, 17, "");
            SET_TEXT(ss2, 21, 30, "");

            SET_VALUE(ss2, 25, 3, 0);
            SET_VALUE(ss2, 27, 3, 0);
            SET_VALUE(ss2, 28, 3, 0);
            SET_VALUE(ss2, 29, 3, 0);

            SET_TEXT(ss2, 32, 7, "");
            SET_TEXT(ss2, 32, 28, "");
            SET_TEXT(ss2, 32, 27, "");

            SET_TEXT(ss2, 39, 6, "");

            SetDrSign(ss2, 37, 31, ComFunc.SetAutoZero(clsType.User.Sabun, 5));

            SET_TEXT(ss2, 39, 25, "");
        }

        void READ_SS3(string argROWID)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strPtno = "";

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT * ";
                SQL += ComNum.VBLF + " FROM ADMIN.OCS_MCCERTIFI_BOHUM3 ";
                SQL += ComNum.VBLF + " WHERE ROWID = '" + argROWID + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
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
                strPtno = dt.Rows[0]["PTNO"].ToString().Trim();

                SET_TEXT(ss3, 3, 6, clsVbfunc.GetPatientName(clsDB.DbCon, strPtno));
                SET_TEXT(ss3, 3, 11, Read_PatientSex(strPtno));
                SET_TEXT(ss3, 3, 14, clsVbfunc.READ_AGE_GESAN(clsDB.DbCon, strPtno));
                SET_TEXT(ss3, 3, 22, ComFunc.FormatStrToDateTime(dt.Rows[0]["BDATE"].ToString().Trim(), "D"));
                SET_TEXT(ss3, 3, 32, dt.Rows[0]["JNO"].ToString().Trim());

                SET_TEXT(ss3, 4, 6, strPtno);
                SET_TEXT(ss3, 4, 14, dt.Rows[0]["DEPTCODE"].ToString().Trim());
                SET_TEXT(ss3, 4, 22, dt.Rows[0]["BI"].ToString().Trim());
                SET_TEXT(ss3, 4, 32, dt.Rows[0]["SNO"].ToString().Trim());

                SET_TEXT(ss3, 5, 6, dt.Rows[0]["ILLNAMEK"].ToString().Trim());
                SET_TEXT(ss3, 6, 6, dt.Rows[0]["TITLE"].ToString().Trim());

                SET_TEXT(ss3, 8, 2, dt.Rows[0]["DATA_1"].ToString().Trim()
                    + ComNum.VBLF +
                    dt.Rows[0]["DATA_2"].ToString().Trim()
                    + ComNum.VBLF +
                    dt.Rows[0]["DATA_3"].ToString().Trim());

                SET_TEXT(ss3, 46, 6, dt.Rows[0]["DRBUNHO"].ToString().Trim());
                SET_TEXT(ss3, 46, 25, clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["WRITESABUN"].ToString().Trim()));
                SetDrSign(ss3, 46, 31, ComFunc.SetAutoZero(dt.Rows[0]["WRITESABUN"].ToString().Trim(), 5));

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

        void CLEAR_SS3()
        {
            SET_TEXT(ss3, 3, 6, "");
            SET_TEXT(ss3, 3, 11, "");
            SET_TEXT(ss3, 3, 14, "");
            SET_TEXT(ss3, 3, 22, "");
            SET_TEXT(ss3, 3, 32, "");

            SET_TEXT(ss3, 4, 6, "");
            SET_TEXT(ss3, 4, 14, "");
            SET_TEXT(ss3, 4, 22, "");
            SET_TEXT(ss3, 4, 32, "");

            SET_TEXT(ss3, 5, 6, "");
            SET_TEXT(ss3, 6, 6, "");

            SET_TEXT(ss3, 8, 2, "");

            SET_TEXT(ss3, 46, 6, "");

            SetDrSign(ss3, 44, 31, ComFunc.SetAutoZero(clsType.User.Sabun, 5));

            SET_TEXT(ss3, 46, 25, "");

        }

        void CLEAR_SS4()
        {
            SET_TEXT(ss8, 3, 1, "ㆍ환자명(등록번호) : " + "" + "(" + "" + ")" +
                             "     ㆍ성별/나이 : " + "" + "/" + "" + "     ㆍ진료일자 : " + "");

            SET_TEXT(ss8, 15, 1, "ㆍ처방의사면허번호 : " + "" + "     ㆍ처방의사명 : " + clsVbfunc.GetBASDoctorName(clsDB.DbCon, ""));

            SET_VALUE(ss8, 8, 5, 0);
            SET_VALUE(ss8, 9, 5, 0);
            SET_VALUE(ss8, 10, 5, 0);
            SET_VALUE(ss8, 11, 5, 0);
            SET_VALUE(ss8, 12, 5, 0);
            SET_VALUE(ss8, 13, 5, 0);


            SET_TEXT(ss9, 3, 7, "");

            SET_VALUE(ss9, 6, 7, 0);
            SET_VALUE(ss9, 6, 16, 0);
            SET_TEXT(ss9, 7, 6, "");
            SET_TEXT(ss9, 8, 6, "");
            SET_TEXT(ss9, 9, 6, "");
            SET_VALUE(ss9, 11, 10, 0);
            SET_VALUE(ss9, 11, 17, 0);
            SET_TEXT(ss9, 12, 10, "");
            SET_TEXT(ss9, 13, 10, "");
            SET_TEXT(ss9, 14, 10, "");
            SET_VALUE(ss9, 16, 12, 0);
            SET_VALUE(ss9, 16, 21, 0);
            SET_TEXT(ss9, 17, 8, "");
            SET_TEXT(ss9, 18, 8, "");
            SET_TEXT(ss9, 19, 8, "");
            SET_TEXT(ss9, 22, 5, "");
            SET_TEXT(ss9, 23, 5, "");
            SET_VALUE(ss9, 29, 9, 0);
            SET_VALUE(ss9, 29, 12, 0);
            SET_VALUE(ss9, 30, 6, 0);
            SET_VALUE(ss9, 30, 9, 0);
            SET_VALUE(ss9, 31, 6, 0);
            SET_VALUE(ss9, 31, 9, 0);
            SET_VALUE(ss9, 32, 7, 0);
            SET_VALUE(ss9, 32, 10, 0);
            SET_VALUE(ss9, 33, 6, 0);
            SET_VALUE(ss9, 33, 9, 0);
            SET_VALUE(ss9, 34, 7, 0);
            SET_VALUE(ss9, 34, 10, 0);
            SET_TEXT(ss9, 37, 2, "");
        }

        void READ_SS4(string argROWID)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strPtno = "";

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT * ";
                SQL += ComNum.VBLF + " FROM ADMIN.OCS_MCCERTIFI_BOHUM4 ";
                SQL += ComNum.VBLF + " WHERE ROWID = '" + argROWID + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
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

                strPtno = dt.Rows[0]["PTNO"].ToString().Trim();

                SET_TEXT(ss8, 3, 1, "ㆍ환자명(등록번호) : " + clsVbfunc.GetPatientName(clsDB.DbCon, strPtno) + "(" + strPtno + ")" +
                                 "     ㆍ성별/나이 : " + Read_PatientSex(strPtno) + "/" + "" + "     ㆍ진료일자 : " +  ComFunc.FormatStrToDateTime(dt.Rows[0]["BDATE"].ToString().Trim() , "D"));

                SET_TEXT(ss8, 15, 1, "ㆍ처방의사면허번호 : " + dt.Rows[0]["DRBUNHO"].ToString().Trim() +
                                             "     ㆍ처방의사명 : " + clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[0]["DRCODE"].ToString().Trim()));

                SET_VALUE(ss8, 8, 5, dt.Rows[0]["DATA_ILL_1"].ToString().Trim());
                SET_VALUE(ss8, 9, 5, dt.Rows[0]["DATA_ILL_2"].ToString().Trim());
                SET_VALUE(ss8, 10, 5, dt.Rows[0]["DATA_ILL_3"].ToString().Trim());
                SET_VALUE(ss8, 11, 5, dt.Rows[0]["DATA_ILL_4"].ToString().Trim());
                SET_VALUE(ss8, 12, 5, dt.Rows[0]["DATA_ILL_5"].ToString().Trim());
                SET_VALUE(ss8, 13, 5, dt.Rows[0]["DATA_ILL_6"].ToString().Trim());

                if (dt.Rows[0]["JINDATE"].ToString().Trim().Length > 6)
                {
                    SET_TEXT(ss9, 3, 7, ComFunc.FormatStrToDateTime(dt.Rows[0]["JINDATE"].ToString().Trim(), "D"));
                }

                SET_VALUE(ss9, 6, 7, dt.Rows[0]["DATA_1_1"].ToString().Trim());
                SET_VALUE(ss9, 6, 16, dt.Rows[0]["DATA_1_2"].ToString().Trim());
                SET_TEXT(ss9, 7, 6, dt.Rows[0]["DATA_1_A"].ToString().Trim());
                SET_TEXT(ss9, 8, 6, dt.Rows[0]["DATA_1_B"].ToString().Trim());
                SET_TEXT(ss9, 9, 6, dt.Rows[0]["DATA_1_C"].ToString().Trim());
                SET_VALUE(ss9, 11, 10, dt.Rows[0]["DATA_2_1"].ToString().Trim());
                SET_VALUE(ss9, 11, 17, dt.Rows[0]["DATA_2_2"].ToString().Trim());
                SET_TEXT(ss9, 12, 10, dt.Rows[0]["DATA_2_A"].ToString().Trim());
                SET_TEXT(ss9, 13, 10, dt.Rows[0]["DATA_2_B"].ToString().Trim());
                SET_TEXT(ss9, 14, 10, dt.Rows[0]["DATA_2_C"].ToString().Trim());
                SET_VALUE(ss9, 16, 12, dt.Rows[0]["DATA_3_1"].ToString().Trim());
                SET_VALUE(ss9, 16, 21, dt.Rows[0]["DATA_3_2"].ToString().Trim());
                SET_TEXT(ss9, 17, 8, dt.Rows[0]["DATA_3_A"].ToString().Trim());
                SET_TEXT(ss9, 18, 8, dt.Rows[0]["DATA_3_B"].ToString().Trim());
                SET_TEXT(ss9, 19, 8, dt.Rows[0]["DATA_3_C"].ToString().Trim());
                SET_TEXT(ss9, 22, 5, dt.Rows[0]["DATA_4_A"].ToString().Trim());
                SET_TEXT(ss9, 23, 5, dt.Rows[0]["DATA_4_B"].ToString().Trim());
                SET_VALUE(ss9, 29, 9, dt.Rows[0]["DATA_5_1_1"].ToString().Trim());
                SET_VALUE(ss9, 29, 12, dt.Rows[0]["DATA_5_1_2"].ToString().Trim());
                SET_VALUE(ss9, 30, 6, dt.Rows[0]["DATA_5_2_1"].ToString().Trim());
                SET_VALUE(ss9, 30, 9, dt.Rows[0]["DATA_5_2_2"].ToString().Trim());
                SET_VALUE(ss9, 31, 6, dt.Rows[0]["DATA_5_3_1"].ToString().Trim());
                SET_VALUE(ss9, 31, 9, dt.Rows[0]["DATA_5_3_2"].ToString().Trim());
                SET_VALUE(ss9, 32, 7, dt.Rows[0]["DATA_5_4_1"].ToString().Trim());
                SET_VALUE(ss9, 32, 10, dt.Rows[0]["DATA_5_4_2"].ToString().Trim());
                SET_VALUE(ss9, 33, 6, dt.Rows[0]["DATA_5_5_1"].ToString().Trim());
                SET_VALUE(ss9, 33, 9, dt.Rows[0]["DATA_5_5_2"].ToString().Trim());
                SET_VALUE(ss9, 34, 7, dt.Rows[0]["DATA_5_6_1"].ToString().Trim());
                SET_VALUE(ss9, 34, 10, dt.Rows[0]["DATA_5_6_2"].ToString().Trim());
                SET_TEXT(ss9, 37, 2, dt.Rows[0]["DATA_6_A"].ToString().Trim());

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

        string Read_PatientSex(string strPano)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            string returnVal = "";

            try
            {
                SQL = " SELECT SEX ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.BAS_PATIENT";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPano + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return returnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return returnVal;
                }

                returnVal = dt.Rows[0]["SEX"].ToString().Trim() == "F" ? "여" : "남";

                dt.Dispose();
                dt = null;
                return returnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return returnVal;
            }
        }

        /// <summary>
        /// VB SetSign
        /// </summary>
        /// <param name="spd">스프레드 이름</param>
        /// <param name="row">로우</param>
        /// <param name="Col">칼럼</param>
        /// <param name="sabun">의사 사번</param>

         void SetDrSign(FarPoint.Win.Spread.FpSpread spd, int row, int Col, string sabun)
        {
            Image ImageX = GetDrSign(sabun, "");
            FarPoint.Win.Spread.CellType.TextCellType cellType = new FarPoint.Win.Spread.CellType.TextCellType();
            cellType.BackgroundImage = new FarPoint.Win.Picture(ImageX, FarPoint.Win.RenderStyle.Stretch);
            spd.ActiveSheet.Cells[row, Col].CellType = cellType;

            ImageX = null;
            cellType = null;
        }


        Image GetDrSign( string strSabun, string strgubun)
        {
            Image rtnVAL = null;

            if (string.IsNullOrEmpty(strSabun)) return rtnVAL;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "SIGNATURE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR ";
                if (strgubun == "1")
                {
                    SQL = SQL + ComNum.VBLF + "WHERE TRIM(drcode) = '" + strSabun + "'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "WHERE TRIM(SABUN) = '" + strSabun + "'";
                }


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    return rtnVAL;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return rtnVAL;
                }

                if (dt.Rows[0]["SIGNATURE"] == DBNull.Value)
                {
                    ComFunc.MsgBox("현재 의사는 서명이 없습니다 확인해주세요.");
                    return rtnVAL;
                }

                using (MemoryStream memStream = new MemoryStream((byte[])dt.Rows[0]["SIGNATURE"]))
                {
                    rtnVAL = Image.FromStream(memStream);
                }

                return rtnVAL;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVAL;
            }
        }

        void SET_TEXT(FarPoint.Win.Spread.FpSpread spd, int argROW, int ArgCol, string ArgText)
        {
            spd.ActiveSheet.Cells[argROW, ArgCol].Text = ArgText;
        }

        void SET_VALUE(FarPoint.Win.Spread.FpSpread spd, int argROW, int ArgCol, object ArgText)
        {
            spd.ActiveSheet.Cells[argROW, ArgCol].Value = ArgText.ToString() == "1";
        }

        string GET_TEXT(FarPoint.Win.Spread.FpSpread spd, int argROW, int ArgCol)
        {
            return spd.ActiveSheet.Cells[argROW, ArgCol].Text.Trim();
        }

        string GET_VALUE(FarPoint.Win.Spread.FpSpread spd, int argROW, int ArgCol)
        {
            try
            {
                return spd.ActiveSheet.Cells[argROW, ArgCol].Text == "True" ? "1" : "0";
            }
            catch
            {
                return "0";
            }
        }



        private void ss5_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row < 0) return;

            fstrROWID = ss5_Sheet1.Cells[e.Row, 4].Text.Trim();
            FstrGUBUN = ss5_Sheet1.Cells[e.Row, 5].Text.Trim();
            ss1.Visible = false;
            ss2.Visible = false;
            ss3.Visible = false;
            tab1.Visible = false;

            switch(FstrGUBUN)
            {
                case "1":
                    ss1.Visible = true;
                    CLEAR_SS1();
                    READ_SS1(fstrROWID);
                    break;
                case "2":
                    ss2.Visible = true;
                    CLEAR_SS2();
                    READ_SS2(fstrROWID);
                    break;
                case "3":
                    ss3.Visible = true;
                    CLEAR_SS3();
                    READ_SS3(fstrROWID);
                    break;
                case "4":
                    tab1.Visible = true;
                    CLEAR_SS4();
                    READ_SS4(fstrROWID);
                    break;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            if(fstrROWID == "" || FstrGUBUN == "")
            {
                ComFunc.MsgBox("소견서를 선택하십시오.");
                return;
            }

            if (ComFunc.MsgBoxQ("출력 하시겠습니까?") == DialogResult.No) return;

            switch (FstrGUBUN)
            {
                case "1":
                    ss1.PrintSheet(0);
                    break;
                case "2":
                    ss2.PrintSheet(0);
                    break;
                case "3":
                    ss3.PrintSheet(0);
                    break;
                case "4":
                    ss8.PrintSheet(0);
                    ss9.PrintSheet(0);
                    break;
            }
        }

        private void lblKorE_Click(object sender, EventArgs e)
        {
            if (lblKorE.Text == "영어")
            {
                ComFunc.SetIMEMODE(this, "K");
                lblKorE.Text = "한글";
            }
            else
            {
                ComFunc.SetIMEMODE(this, "E");
                lblKorE.Text = "영어";
            }
        }
    }
}
