using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmOPDJuSaGumSaStats : Form
    {
        string mstrExamPart = "";
        string mstrExamCode = "";


        public frmOPDJuSaGumSaStats()
        {
            InitializeComponent();
        }

        private void frmOPDJuSaGumSaStats_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            lblUse.Visible = false;
            panAuto.Visible = false;

            if (clsPublic.GnJobSabun == 4349)
            {
                panAuto.Visible = true;
                lblUse.Visible = false;
            }

            //ss1_Sheet1.Columns.Get(7, 8).Visible = false;
            //ss2_Sheet1.Columns.Get(8).Visible = false;

            dtpFDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
            dtpTDate.Value = dtpFDate.Value;
            dtpBDate.Value = dtpTDate.Value;

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SQL = "SELECT PRINTRANKING, CODE     ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.NUR_CODE      ";
                SQL = SQL + ComNum.VBLF + "WHERE GUBUN ='Z' GROUP BY PRINTRANKING,CODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {

                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    cboDept.Items.Clear();
                    cboDept.Items.Add("전체");

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboDept.Items.Add(dt.Rows[i]["CODE"].ToString().Trim());
                    }
                    cboDept.SelectedIndex = 0;
                }

                dt.Dispose();
                dt = null;

                lblTot.Text = "";

                mstrExamPart = "";
                mstrExamCode = "";

                lblSTS.Text = "";
                btnChk.Enabled = false;
                btnSet.Enabled = false;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnCan_Click(object sender, EventArgs e)
        {
            READ_EXAM_CODE2_SUCODE();
        }

        private void READ_EXAM_CODE2_SUCODE()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ss2_Sheet1.RowCount = 0;
            ss2_Sheet1.RowCount = 10;

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SQL = " SELECT CODE,SUCODE,NAME,AUTO,USE,DEPT,ROWID  ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.NUR_CODE_EXAM";
                SQL = SQL + ComNum.VBLF + "  WHERE TRIM(PART) ='" + mstrExamPart + "' ";
                SQL = SQL + ComNum.VBLF + "   AND TRIM(CODE) ='" + mstrExamCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ( DELDATE IS NULL OR DELDATE ='' )";
                SQL = SQL + ComNum.VBLF + "  ORDER BY SUCODE,RANKING  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ss2_Sheet1.RowCount = dt.Rows.Count + 10;
                    ss2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ss2_Sheet1.Cells[i, 0].Text = "";
                        ss2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["NAME"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["AUTO"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 4].Text = dt.Rows[i]["USE"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DEPT"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 6].Text = "";
                        ss2_Sheet1.Cells[i, 7].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            lblTot.Text = "";
            btnChk.Enabled = false;
            btnSet.Enabled = false;
        }

        private void btnChk_Click(object sender, EventArgs e)
        {
            if (ss1_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data) < 0)
            {
                ComFunc.MsgBox("검사항목을 조회후 작업하십시오!!", "확인");
                return;
            }

            if (cboDept.Text == "전체")
            {
                ComFunc.MsgBox("검사 건수 조회는 해당부서를 선택후 조회하십시오!!", "확인");
                return;
            }

            READ_TONG_EXAM_CNT(cboDept.Text.Trim());

            if (cboDept.Text == "주사실")
            {
                Jusa_Tong_Temp(dtpBDate.Value.ToString("yyyy-MM-dd")); //'주사임시통계용
            }
            else if (cboDept.Text == "류마티스")
            {
                Jusa_Tong_RA(dtpBDate.Value.ToString("yyyy-MM-dd")); //'주사임시통계용
            }
            else if (cboDept.Text == "항암주사")
            {
                Jusa_Tong_CANCER(dtpBDate.Value.ToString("yyyy-MM-dd")); //'주사임시통계용
            }

            btnSet.Enabled = true;

            ComFunc.MsgBox("건수 자동계산 완료됨!!", "확인");
        }
        private void Jusa_Tong_Temp(string ArgDate)
        {
            int i = 0;
            int intInWon = 0;
            double dblQty = 0;
            int intInWon2 = 0;
            double dblQty2 = 0;

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인


                ssJusa_Sheet1.RowCount = 0;
                ssDetail_Sheet1.RowCount = 0;

                if (VB.IsDate(ArgDate) == false)
                {
                    return;
                }

                SQL = "";
                SQL = "SELECT DECODE(SUBSTR(DOSCODE, 2,1), '1', 'IM', '2', 'IV', '3', 'IV(M)','5', 'SC') DOSCODE,";
                SQL = SQL + ComNum.VBLF + "COUNT(PTNO) INWON, SUM(QTY * NAL) QTY";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.ETC_JUSASUB";
                SQL = SQL + ComNum.VBLF + "WHERE ACTDATE >= TO_DATE('" + ArgDate + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE < TO_DATE('" + Convert.ToDateTime(ArgDate).AddDays(1).ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND RTRIM(DOSCODE) IN ('910101', '910201', '920101', '920201', '950101', '950201', '930101','930201')";
                SQL = SQL + ComNum.VBLF + "   AND DOSCODE NOT IN (SELECT DOSCODE FROM ADMIN.OCS_ODOSAGE WHERE DOSNAME LIKE '%항암%')";
                SQL = SQL + ComNum.VBLF + "   AND GBACT = 'Y'";
                SQL = SQL + ComNum.VBLF + "GROUP BY DECODE(SUBSTR(DOSCODE, 2,1), '1', 'IM', '2', 'IV', '3', 'IV(M)','5', 'SC')";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                ssJusa_Sheet1.RowCount = dt.Rows.Count;
                ssJusa_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssJusa_Sheet1.Cells[i, 0].Text = "      " + dt.Rows[i]["DOSCODE"].ToString().Trim();
                        ssJusa_Sheet1.Cells[i, 1].Text = dt.Rows[i]["INWON"].ToString().Trim();
                        ssJusa_Sheet1.Cells[i, 2].Text = VB.Val(dt.Rows[i]["QTY"].ToString().Trim()).ToString("0.00");
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT       ";
                SQL = SQL + ComNum.VBLF + "CASE     ";
                SQL = SQL + ComNum.VBLF + "    WHEN X.DOSCODE IS NULL THEN (SELECT SUNAMEK  FROM ADMIN.BAS_SUN WHERE SUNEXT = X.SUCODE)       ";
                SQL = SQL + ComNum.VBLF + "        ELSE X.DOSCODE       ";
                SQL = SQL + ComNum.VBLF + "    END AS DOSCODE       ";
                SQL = SQL + ComNum.VBLF + "    , X.PTNO,X.SUCODE, X.DEPTCODE,       ";
                SQL = SQL + ComNum.VBLF + "    (SELECT SNAME FROM ADMIN.BAS_PATIENT WHERE PANO = X.PTNO) AS PTNAME,       ";
                SQL = SQL + ComNum.VBLF + "    (SELECT DRNAME FROM ADMIN.BAS_DOCTOR WHERE DRCODE = X.DRCODE) AS DRNAME       ";
                SQL = SQL + ComNum.VBLF + "FROM(SELECT DECODE(SUBSTR(DOSCODE, 2, 1), '1', 'IM', '2', 'IV', '3', 'IV(M)', '5', 'SC')  DOSCODE,       ";
                SQL = SQL + ComNum.VBLF + "            PTNO, BDATE, SUCODE, DRCODE, DEPTCODE, '1' GUBUN       ";
                SQL = SQL + ComNum.VBLF + "        FROM ADMIN.ETC_JUSASUB A       ";
                SQL = SQL + ComNum.VBLF + "        WHERE ACTDATE >= TO_DATE('" + ArgDate + "', 'YYYY-MM-DD')       ";
                SQL = SQL + ComNum.VBLF + "            AND ACTDATE < TO_DATE('" + Convert.ToDateTime(ArgDate).AddDays(1).ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')       ";
                SQL = SQL + ComNum.VBLF + "            AND RTRIM(DOSCODE) IN('910101', '910201', '920101', '920201', '950101', '950201', '930101', '930201')       ";
                SQL = SQL + ComNum.VBLF + "            AND GBACT = 'Y'       ";
                SQL = SQL + ComNum.VBLF + "        UNION ALL       ";
                SQL = SQL + ComNum.VBLF + "        SELECT '' DOSCODE, PTNO, BDATE, SUCODE, DRCODE, DEPTCODE, '2' GUBUN       ";
                SQL = SQL + ComNum.VBLF + "        FROM ADMIN.ETC_JUSASUB       ";
                SQL = SQL + ComNum.VBLF + "        WHERE ACTDATE >= TO_DATE('" + ArgDate + "', 'YYYY-MM-DD')       ";
                SQL = SQL + ComNum.VBLF + "            AND ACTDATE < TO_DATE('" + Convert.ToDateTime(ArgDate).AddDays(1).ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')       ";
                SQL = SQL + ComNum.VBLF + "            AND SUCODE IN(SELECT TCODE FROM ADMIN.OCS_OPDTONGCODE WHERE CANCEL = 'Y' AND GUBUN = '01')       ";
                SQL = SQL + ComNum.VBLF + "            AND GBACT = 'Y'       ";
                SQL = SQL + ComNum.VBLF + "        UNION ALL       ";
                SQL = SQL + ComNum.VBLF + "        SELECT '' DOSCODE, PTNO, BDATE, SUCODE, DRCODE, DEPTCODE, '3' GUBUN       ";
                SQL = SQL + ComNum.VBLF + "        FROM ADMIN.ETC_JUSASUB       ";
                SQL = SQL + ComNum.VBLF + "        WHERE ACTDATE >= TO_DATE('" + ArgDate + "', 'YYYY-MM-DD')       ";
                SQL = SQL + ComNum.VBLF + "            AND ACTDATE < TO_DATE('" + Convert.ToDateTime(ArgDate).AddDays(1).ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')       ";
                SQL = SQL + ComNum.VBLF + "            AND RTRIM(DOSCODE) NOT IN('910101', '910201', '920101', '920201', '950101', '950201', '930101', '930201')       ";
                SQL = SQL + ComNum.VBLF + "            AND GBACT = 'Y'       ";
                SQL = SQL + ComNum.VBLF + "            AND SUCODE NOT IN(SELECT TCODE FROM ADMIN.OCS_OPDTONGCODE WHERE CANCEL = 'Y' AND GUBUN = '01')       ";
                SQL = SQL + ComNum.VBLF + "        ORDER BY DOSCODE ASC, GUBUN ASC) X       ";


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                ssDetail_Sheet1.RowCount = dt.Rows.Count;
                ssDetail_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssDetail_Sheet1.Cells[i, 0].Text = dt.Rows[i]["DOSCODE"].ToString().Trim();
                        ssDetail_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                        ssDetail_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PTNAME"].ToString().Trim();
                        ssDetail_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ssDetail_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssDetail_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                    }
                }
                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT SUCODE,";
                SQL = SQL + ComNum.VBLF + "       COUNT(PTNO) INWON, SUM(QTY) QTY";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.ETC_JUSASUB";
                SQL = SQL + ComNum.VBLF + " WHERE ACTDATE >= TO_DATE('" + ArgDate + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE < TO_DATE('" + Convert.ToDateTime(ArgDate).AddDays(1).ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND SUCODE IN (SELECT TCODE FROM ADMIN.OCS_OPDTONGCODE WHERE CANCEL = 'Y' AND GUBUN = '01')";
                SQL = SQL + ComNum.VBLF + "   AND GBACT = 'Y'";
                SQL = SQL + ComNum.VBLF + " GROUP BY SUCODE";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssJusa_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    if (VB.Val(dt.Rows[0]["INWON"].ToString().Trim()) > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            ssJusa_Sheet1.RowCount = ssJusa_Sheet1.RowCount + 1;

                            ssJusa_Sheet1.Cells[ssJusa_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["SUCODE"].ToString().Trim() + "/" + READ_SuName2(dt.Rows[i]["SUCODE"].ToString().Trim()) + ")";
                            ssJusa_Sheet1.Cells[ssJusa_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["INWON"].ToString().Trim();
                            ssJusa_Sheet1.Cells[ssJusa_Sheet1.RowCount - 1, 2].Text = VB.Val(dt.Rows[i]["QTY"].ToString().Trim()).ToString("0.00");
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "    SELECT SUCODE, COUNT(PTNO) INWON, SUM(QTY * NAL) QTY ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.ETC_JUSASUB";
                SQL = SQL + ComNum.VBLF + " WHERE ACTDATE >= TO_DATE('" + ArgDate + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE < TO_DATE('" + Convert.ToDateTime(ArgDate).AddDays(1).ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND RTRIM(DOSCODE) NOT IN ('910101', '910201', '920101', '920201', '950101', '950201', '930101', '930201')";
                SQL = SQL + ComNum.VBLF + "   AND GBACT = 'Y'";
                SQL = SQL + ComNum.VBLF + "   AND SUCODE NOT IN (SELECT TCODE FROM ADMIN.OCS_OPDTONGCODE WHERE CANCEL = 'Y' AND GUBUN = '01')";
                SQL = SQL + ComNum.VBLF + " GROUP BY SUCODE";


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssJusa_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    if (VB.Val(dt.Rows[0]["INWON"].ToString().Trim()) > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            ssJusa_Sheet1.RowCount = ssJusa_Sheet1.RowCount + 1;

                            ssJusa_Sheet1.Cells[ssJusa_Sheet1.RowCount - 1, 0].Text = "기 타(수가코드:" + dt.Rows[i]["SUCODE"].ToString().Trim() + "//" + READ_SuName2(dt.Rows[i]["SUCODE"].ToString().Trim()) + ")";
                            ssJusa_Sheet1.Cells[ssJusa_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["INWON"].ToString().Trim();
                            ssJusa_Sheet1.Cells[ssJusa_Sheet1.RowCount - 1, 2].Text = VB.Val(dt.Rows[i]["QTY"].ToString().Trim()).ToString("0.00");
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                if (ss1_Sheet1.Cells[1, 1].Text.Trim() == "") return;


                intInWon = 0;
                dblQty = 0;

                for (i = 0; i < ssJusa_Sheet1.RowCount; i++)
                {
                    intInWon2 = Convert.ToInt32(VB.Val(ssJusa_Sheet1.Cells[i, 1].Text.Trim()));
                    dblQty2 = VB.Val(ssJusa_Sheet1.Cells[i, 2].Text.Trim());
                    intInWon = intInWon + intInWon2;
                    dblQty = dblQty + dblQty2;
                }

                ssJusa_Sheet1.RowCount = ssJusa_Sheet1.RowCount + 1;

                ssJusa_Sheet1.Cells[ssJusa_Sheet1.RowCount - 1, 0].Text = "     합         계  ";
                ssJusa_Sheet1.Cells[ssJusa_Sheet1.RowCount - 1, 1].Text = Convert.ToString(intInWon);
                ssJusa_Sheet1.Cells[ssJusa_Sheet1.RowCount - 1, 2].Text = dblQty.ToString("0.00");


            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void Jusa_Tong_CANCER(string ArgDate)
        {
            int i = 0;
            int intInWon = 0;
            double dblQty = 0;
            int intInWon2 = 0;
            double dblQty2 = 0;

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인


                ssJusa_Sheet1.RowCount = 0;
                ssDetail_Sheet1.RowCount = 0;

                if (VB.IsDate(ArgDate) == false)
                {
                    return;
                }

                SQL = "";
                SQL = "SELECT DECODE(SUBSTR(DOSCODE, 2,1), '1', 'IM', '2', 'IV', '3', 'IV(M)','5', 'SC') DOSCODE,";
                SQL = SQL + ComNum.VBLF + "COUNT(PTNO) INWON, SUM(QTY * NAL) QTY";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.ETC_JUSASUB";
                SQL = SQL + ComNum.VBLF + "WHERE ACTDATE >= TO_DATE('" + ArgDate + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE < TO_DATE('" + Convert.ToDateTime(ArgDate).AddDays(1).ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND DOSCODE IN (SELECT DOSCODE FROM ADMIN.OCS_ODOSAGE WHERE DOSNAME LIKE '%항암%')";
                SQL = SQL + ComNum.VBLF + "   AND GBACT = 'Y'";
                SQL = SQL + ComNum.VBLF + "GROUP BY DECODE(SUBSTR(DOSCODE, 2,1), '1', 'IM', '2', 'IV', '3', 'IV(M)','5', 'SC')";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                ssJusa_Sheet1.RowCount = dt.Rows.Count;
                ssJusa_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssJusa_Sheet1.Cells[i, 0].Text = "      " + dt.Rows[i]["DOSCODE"].ToString().Trim();
                        ssJusa_Sheet1.Cells[i, 1].Text = dt.Rows[i]["INWON"].ToString().Trim();
                        ssJusa_Sheet1.Cells[i, 2].Text = VB.Val(dt.Rows[i]["QTY"].ToString().Trim()).ToString("0.00");
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT       ";
                SQL = SQL + ComNum.VBLF + "CASE     ";
                SQL = SQL + ComNum.VBLF + "    WHEN X.DOSCODE IS NULL THEN (SELECT SUNAMEK  FROM ADMIN.BAS_SUN WHERE SUNEXT = X.SUCODE)       ";
                SQL = SQL + ComNum.VBLF + "        ELSE X.DOSCODE       ";
                SQL = SQL + ComNum.VBLF + "    END AS DOSCODE       ";
                SQL = SQL + ComNum.VBLF + "    , X.PTNO,X.SUCODE, X.DEPTCODE,       ";
                SQL = SQL + ComNum.VBLF + "    (SELECT SNAME FROM ADMIN.BAS_PATIENT WHERE PANO = X.PTNO) AS PTNAME,       ";
                SQL = SQL + ComNum.VBLF + "    (SELECT DRNAME FROM ADMIN.BAS_DOCTOR WHERE DRCODE = X.DRCODE) AS DRNAME       ";
                SQL = SQL + ComNum.VBLF + "FROM (SELECT DECODE(SUBSTR(DOSCODE, 2, 1), '1', 'IM', '2', 'IV', '3', 'IV(M)', '5', 'SC')  DOSCODE,       ";
                SQL = SQL + ComNum.VBLF + "            PTNO, BDATE, SUCODE, DRCODE, DEPTCODE, '1' GUBUN       ";
                SQL = SQL + ComNum.VBLF + "        FROM ADMIN.ETC_JUSASUB A       ";
                SQL = SQL + ComNum.VBLF + "        WHERE ACTDATE >= TO_DATE('" + ArgDate + "', 'YYYY-MM-DD')       ";
                SQL = SQL + ComNum.VBLF + "            AND ACTDATE < TO_DATE('" + Convert.ToDateTime(ArgDate).AddDays(1).ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')       ";
                SQL = SQL + ComNum.VBLF + "            AND DOSCODE IN (SELECT DOSCODE FROM ADMIN.OCS_ODOSAGE WHERE DOSNAME LIKE '%항암%')      ";
                SQL = SQL + ComNum.VBLF + "            AND GBACT = 'Y'       ";
                SQL = SQL + ComNum.VBLF + "        ORDER BY DOSCODE ASC, GUBUN ASC) X       ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                ssDetail_Sheet1.RowCount = dt.Rows.Count;
                ssDetail_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssDetail_Sheet1.Cells[i, 0].Text = dt.Rows[i]["DOSCODE"].ToString().Trim();
                        ssDetail_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                        ssDetail_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PTNAME"].ToString().Trim();
                        ssDetail_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ssDetail_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssDetail_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                    }
                }
                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT SUCODE,";
                SQL = SQL + ComNum.VBLF + "       COUNT(PTNO) INWON, SUM(QTY) QTY";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.ETC_JUSASUB";
                SQL = SQL + ComNum.VBLF + " WHERE ACTDATE >= TO_DATE('" + ArgDate + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE < TO_DATE('" + Convert.ToDateTime(ArgDate).AddDays(1).ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND DOSCODE IN (SELECT DOSCODE FROM ADMIN.OCS_ODOSAGE WHERE DOSNAME LIKE '%항암%')";
                SQL = SQL + ComNum.VBLF + "   AND GBACT = 'Y'";
                SQL = SQL + ComNum.VBLF + " GROUP BY SUCODE";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssJusa_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    if (VB.Val(dt.Rows[0]["INWON"].ToString().Trim()) > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            ssJusa_Sheet1.RowCount = ssJusa_Sheet1.RowCount + 1;

                            ssJusa_Sheet1.Cells[ssJusa_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["SUCODE"].ToString().Trim() + "/" + READ_SuName2(dt.Rows[i]["SUCODE"].ToString().Trim()) + ")";
                            ssJusa_Sheet1.Cells[ssJusa_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["INWON"].ToString().Trim();
                            ssJusa_Sheet1.Cells[ssJusa_Sheet1.RowCount - 1, 2].Text = VB.Val(dt.Rows[i]["QTY"].ToString().Trim()).ToString("0.00");
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                if (ss1_Sheet1.Cells[0, 0].Text.Trim() == "") return;


                intInWon = 0;
                dblQty = 0;

                for (i = 0; i < ssJusa_Sheet1.RowCount; i++)
                {
                    intInWon2 = Convert.ToInt32(VB.Val(ssJusa_Sheet1.Cells[i, 1].Text.Trim()));
                    dblQty2 = VB.Val(ssJusa_Sheet1.Cells[i, 2].Text.Trim());
                    intInWon = intInWon + intInWon2;
                    dblQty = dblQty + dblQty2;
                }

                ssJusa_Sheet1.RowCount = ssJusa_Sheet1.RowCount + 1;

                ssJusa_Sheet1.Cells[ssJusa_Sheet1.RowCount - 1, 0].Text = "     합         계  ";
                ssJusa_Sheet1.Cells[ssJusa_Sheet1.RowCount - 1, 1].Text = Convert.ToString(intInWon);
                ssJusa_Sheet1.Cells[ssJusa_Sheet1.RowCount - 1, 2].Text = dblQty.ToString("0.00");


            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private string READ_SuName2(string ArgCode)
        {
            string strVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (ArgCode == "") return strVal;

                if (ArgCode == "T")
                {
                    strVal = "조직 검사";
                    return strVal;
                }
                if (ArgCode == "A")
                {
                    strVal = "세포 검사";
                    return strVal;
                }

                SQL = "";
                SQL = "SELECT SUNAMEK  FROM ADMIN.BAS_SUN ";
                SQL = SQL + "WHERE SUNEXT = '" + ArgCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["SUNAMEK"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return strVal;
        }

        private void Jusa_Tong_RA(string ArgDate)
        {
            int i = 0;
            string strJCodes = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = "SELECT TCODE ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.OCS_OPDTONGCODE      ";
                SQL = SQL + ComNum.VBLF + "WHERE CANCEL = 'Y'       ";
                SQL = SQL + ComNum.VBLF + "    AND GUBUN = '01'         ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["TCODE"].ToString().Trim() != "")
                        {
                            strJCodes = strJCodes + "'" + dt.Rows[i]["TCODE"].ToString().Trim() + "',";
                        }
                    }

                    if (strJCodes != "")
                    {
                        strJCodes = VB.Mid(strJCodes.Trim(), 1, strJCodes.Trim().Length - 1);
                    }
                }
                dt.Dispose();
                dt = null;

                ssJusa_Sheet1.RowCount = 0;
                ssDetail_Sheet1.RowCount = 0;

                if (VB.IsDate(ArgDate) == false) return;

                if (cboDept.Text != "류마티스")
                {
                    return;
                }

                SQL = "";
                SQL = "SELECT DECODE(SUBSTR(DOSCODE, 2,1), '1', 'IM', '2', 'IV', '3', 'IV(M)','5', 'SC') DOSCODE,";
                SQL = SQL + ComNum.VBLF + "COUNT(PTNO) INWON, SUM(QTY * NAL) QTY";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.OCS_OORDER";
                SQL = SQL + ComNum.VBLF + "WHERE BDATE >= TO_DATE('" + ArgDate + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND BDATE < TO_DATE('" + Convert.ToDateTime(ArgDate).ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND SUCODE = 'E7190'";
                SQL = SQL + ComNum.VBLF + "   AND GBSUNAP = '1'";
                SQL = SQL + ComNum.VBLF + "GROUP BY DECODE(SUBSTR(DOSCODE, 2,1), '1', 'IM', '2', 'IV', '3', 'IV(M)','5', 'SC')";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssJusa_Sheet1.RowCount = dt.Rows.Count;
                    ssJusa_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssJusa_Sheet1.Cells[i, 0].Text = "      " + dt.Rows[i]["DOSCODE"].ToString().Trim();
                        ssJusa_Sheet1.Cells[i, 1].Text = dt.Rows[i]["INWON"].ToString().Trim();
                        ssJusa_Sheet1.Cells[i, 2].Text = VB.Val(dt.Rows[i]["QTY"].ToString().Trim()).ToString("0.00");
                    }
                }
                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT DECODE (SUBSTR (DOSCODE, 2, 1),  '1', 'IM',  '2', 'IV',  '3', 'IV(M)', '5', 'SC')  DOSCODE,";
                SQL = SQL + ComNum.VBLF + "   PTNO , BDATE, SUCODE, DRCODE, DEPTCODE, '1' GUBUN  ";
                SQL = SQL + ComNum.VBLF + "     FROM ADMIN.OCS_OORDER A";
                SQL = SQL + ComNum.VBLF + "    WHERE BDATE >= TO_DATE('" + ArgDate + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "      AND BDATE < TO_DATE('" + Convert.ToDateTime(ArgDate).ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "      AND SUCODE = 'E7190'";
                SQL = SQL + ComNum.VBLF + "      AND GBSUNAP = '1'";
                SQL = SQL + ComNum.VBLF + " ORDER BY DOSCODE ASC, GUBUN ASC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssDetail_Sheet1.RowCount = dt.Rows.Count;
                    ssDetail_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssDetail_Sheet1.Cells[i, 0].Text = dt.Rows[i]["DOSCODE"].ToString().Trim();

                        if (ssDetail_Sheet1.Cells[i, 0].Text.Trim() == "")
                        {
                            ssDetail_Sheet1.Cells[i, 0].Text = READ_SuName2(dt.Rows[i]["SUCODE"].ToString().Trim());
                        }

                        ssDetail_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                        ssDetail_Sheet1.Cells[i, 2].Text = clsVbfunc.GetPatientName(clsDB.DbCon, dt.Rows[i]["PTNO"].ToString().Trim());
                        ssDetail_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ssDetail_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssDetail_Sheet1.Cells[i, 5].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString().Trim());
                    }
                }
                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void READ_TONG_EXAM_CNT(string ArgPart)
        {
            int i = 0;
            int h = 0;

            string strCODE = "";
            string strSucode = "";

            string strDate = "";
            string strGubun = "";
            string strDept = "";

            int nCNT = 0;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            DataTable dt = null;

            strDate = dtpBDate.Value.ToString("yyyy-MM-dd"); //'기준일자

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                //'해당파트 검사코드 읽기
                for (i = 0; i < ss1_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data) + 1; i++)
                {

                    strCODE = ss1_Sheet1.Cells[i, 0].Text.Trim();

                    SQL = " SELECT SUCODE,AUTO,DEPT FROM ADMIN.NUR_CODE_EXAM ";
                    SQL = SQL + ComNum.VBLF + "  WHERE TRIM(PART) ='" + ArgPart + "'  ";
                    SQL = SQL + ComNum.VBLF + "  AND CODE ='" + strCODE + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND (DELDATE IS NULL OR DELDATE ='') ";
                    SQL = SQL + ComNum.VBLF + "  AND USE ='Y' ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY CODE,SUCODE ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    strSucode = "";
                    nCNT = 0;

                    if (dt.Rows.Count > 0)
                    {
                        for (h = 0; h < dt.Rows.Count; h++)
                        {
                            strGubun = dt.Rows[h]["AUTO"].ToString().Trim();
                            strSucode = dt.Rows[h]["SUCODE"].ToString().Trim();
                            strDept = dt.Rows[h]["DEPT"].ToString().Trim();

                            //'자동계산 수가 읽기
                            nCNT = nCNT + READ_TONG_CNT(strGubun, strDate, ArgPart, strCODE, strSucode, strDept);
                        }

                        ss1_Sheet1.Cells[i, 8].Text = Convert.ToString(nCNT);
                    }
                    dt.Dispose();
                    dt = null;

                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private int READ_TONG_CNT(string strGubun, string strBDate, string argPart, string strCODE, string strSucode, string strDept)
        {
            int intVal = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if (cboDept.Text == "류마티스")
            {
                ssJusa_Sheet1.RowCount = 0;
                ssDetail_Sheet1.RowCount = 0;
            }

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return intVal; //권한 확인

                switch (strGubun)
                {

                    case "00": //'오더(외래,입원);

                        SQL = " SELECT PTNO,ORDERNO,SUM(NAL*QTY) CNT ";
                        SQL = SQL + ComNum.VBLF + " FROM ADMIN.OCS_OORDER ";
                        SQL = SQL + ComNum.VBLF + "  WHERE BDATE =TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND SUCODE ='" + strSucode + "' ";

                        if (strDept != "") SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='" + strDept + "'";

                        SQL = SQL + ComNum.VBLF + " GROUP BY PTNO,ORDERNO  ";
                        SQL = SQL + ComNum.VBLF + "  HAVING SUM(NAL*QTY) > 0 ";
                        SQL = SQL + ComNum.VBLF + "   UNION ALL ";
                        SQL = SQL + ComNum.VBLF + "  SELECT PTNO,ORDERNO,SUM(NAL*QTY) CNT ";
                        SQL = SQL + ComNum.VBLF + " FROM ADMIN.OCS_IORDER ";
                        SQL = SQL + ComNum.VBLF + "  WHERE BDATE =TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND SUCODE ='" + strSucode + "' ";

                        if (strDept != "") SQL = SQL + "   AND DEPTCODE ='" + strDept + "' ";

                        SQL = SQL + " GROUP BY PTNO,ORDERNO  ";
                        SQL = SQL + "  HAVING SUM(NAL*QTY) > 0 ";
                        break;

                    case "01": //'오더(외래);


                        SQL = " SELECT PTNO,ORDERNO,SUM(NAL*QTY) CNT ";
                        SQL = SQL + ComNum.VBLF + " FROM ADMIN.OCS_OORDER ";
                        SQL = SQL + ComNum.VBLF + "  WHERE BDATE =TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND SUCODE ='" + strSucode + "' ";

                        if (strDept != "") SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='" + strDept + "' ";

                        SQL = SQL + ComNum.VBLF + " GROUP BY PTNO,ORDERNO  ";
                        SQL = SQL + ComNum.VBLF + "  HAVING SUM(NAL*QTY) > 0 ";
                        break;

                    case "02": // '오더(입원);

                        SQL = " SELECT PTNO,ORDERNO,SUM(NAL*QTY) CNT ";
                        SQL = SQL + ComNum.VBLF + " FROM ADMIN.OCS_IORDER ";
                        SQL = SQL + ComNum.VBLF + "  WHERE BDATE =TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND SUCODE ='" + strSucode + "' ";

                        if (strDept != "") SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='" + strDept + "' ";

                        SQL = SQL + ComNum.VBLF + " GROUP BY PTNO,ORDERNO  ";
                        SQL = SQL + ComNum.VBLF + "  HAVING SUM(NAL*QTY) > 0 ";
                        break;

                    case "03": //'ETC_JUPMST;

                        SQL = " SELECT PTNO,ORDERNO  ";
                        SQL = SQL + ComNum.VBLF + " FROM ADMIN.ETC_JUPMST  ";
                        //'조회기준 변경 2016-08-30 계장 김현욱;
                        SQL = SQL + ComNum.VBLF + "  WHERE STARTDATE >= TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND STARTDATE < TO_DATE('" + Convert.ToDateTime(strBDate).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND GBJOB ='3'  ";  //'완료건;
                        SQL = SQL + ComNum.VBLF + "   AND ORDERCODE ='" + strSucode + "' ";

                        if (strDept != "")
                        {
                            if (strDept != "**")
                            {
                                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='" + strDept + "' ";
                            }
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "   AND DEPTCODE NOT IN ('HR','TO') ";
                        }
                        SQL = SQL + ComNum.VBLF + " GROUP BY PTNO,ORDERNO  ";
                        break;

                    case "04": //'ENDO_JUPMST;
                        SQL = " SELECT PTNO,ORDERNO  ";
                        SQL = SQL + ComNum.VBLF + " FROM ADMIN.ENDO_JUPMST  ";
                        SQL = SQL + ComNum.VBLF + "  WHERE RDATE >=TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND RDATE <TO_DATE('" + Convert.ToDateTime(strBDate).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND GBSUNAP ='7'  ";  //'완료건;
                        SQL = SQL + ComNum.VBLF + "   AND ORDERCODE ='" + strSucode + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND RESULTDATE IS NOT NULL ";  //'결과일자 있는것;

                        if (strDept != "")
                        {
                            SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='" + strDept + "' ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "   AND BUSE ='056104' ";  //'내시경실 시행만;
                        }
                        SQL = SQL + ComNum.VBLF + " GROUP BY PTNO,ORDERNO  ";

                        break;

                    case "05": // 'ETC_JUSASUB - 용법,GBMASTER - 멀티처방;


                        SQL = "     SELECT PTNO,ORDERNO,GBMASTER  ";
                        SQL = SQL + ComNum.VBLF + "  FROM ADMIN.ETC_JUSASUB ";
                        SQL = SQL + ComNum.VBLF + " WHERE ACTDATE = TO_DATE('" + strBDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "   AND GBACT = 'Y'";
                        SQL = SQL + ComNum.VBLF + "   AND DOSCODE ='" + strSucode + "' ";
                        SQL = SQL + ComNum.VBLF + " GROUP BY PTNO,ORDERNO,GBMASTER  ";

                        break;

                    case "06": // 'ETC_JUSASUB - 수가;


                        SQL = "     SELECT PTNO,ORDERNO  ";
                        SQL = SQL + ComNum.VBLF + "  FROM ADMIN.ETC_JUSASUB ";
                        SQL = SQL + ComNum.VBLF + " WHERE ACTDATE = TO_DATE('" + strBDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "   AND GBACT = 'Y'";

                        switch (strSucode)
                        {
                            case "SKFLU4-A1":
                                SQL = SQL + ComNum.VBLF + "   AND SUCODE ='SKFLU4-A' ";
                                SQL = SQL + ComNum.VBLF + "   AND DRCODE ='0999' ";
                                break;
                            case "SKFLU4-A2":
                                SQL = SQL + ComNum.VBLF + "   AND SUCODE ='SKFLU4-A' ";
                                SQL = SQL + ComNum.VBLF + "   AND DRCODE <>'0999' ";
                                break;
                            default:
                                SQL = SQL + ComNum.VBLF + "   AND SUCODE ='" + strSucode + "' ";
                                break;
                        }
                        SQL = SQL + ComNum.VBLF + " GROUP BY PTNO,ORDERNO  ";
                        break;

                    case "07": //'XRAY_RESULTNEW 판독;

                        SQL = "SELECT PANO,WRTNO  ";
                        SQL = SQL + ComNum.VBLF + "  FROM ADMIN.XRAY_RESULTNEW  ";
                        SQL = SQL + ComNum.VBLF + " WHERE READDATE = TO_DATE('" + strBDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "   AND XCODE ='" + strSucode + "' ";
                        SQL = SQL + ComNum.VBLF + " GROUP BY PANO,WRTNO ";
                        break;

                    case "08": // 'XRAY_DETAIL;

                        SQL = "  SELECT PANO,ORDERNO ";
                        SQL = SQL + ComNum.VBLF + " FROM ADMIN.XRAY_DETAIL ";
                        SQL = SQL + ComNum.VBLF + " WHERE SEEKDATE>= TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND SEEKDATE < TO_DATE('" + Convert.ToDateTime(strBDate).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND XCODE ='" + strSucode + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND GBRESERVED IN ('6','7') "; //'접수된것;
                        SQL = SQL + ComNum.VBLF + "   AND GBSTS <> 'D' ";
                        //과에서 사용하는 초음파도 영상을 넣기때문에 조건 변경해줌
                        //SQL = SQL + ComNum.VBLF + "   AND (PACSSTUDYID IS NULL OR (EXINFO IS NULL OR EXINFO < 1000)) "; //'영상이 없고 판독하지 않은것;

                        if (strDept != "")
                        {
                            SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='" + strDept + "' ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "   AND DEPTCODE NOT IN ('HR','TO') ";
                        }
                        SQL = SQL + ComNum.VBLF + " GROUP BY PANO, ORDERNO ";
                        break;
                    default:
                        return intVal;
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {

                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return intVal;
                }

                if (dt.Rows.Count > 0)
                {
                    intVal = dt.Rows.Count;
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return intVal;
        }


        private int READ_TONG_CNT_DETAIL(FpSpread ArgSS, string strGubun, string strBDate, string argPart, string strCODE, string strSucode, string strDept)
        {
            int intVal = 0;
            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return intVal; //권한 확인

                switch (strGubun)
                {
                    case "00": //'오더(외래,입원);
                        SQL = "";
                        SQL = " SELECT A.PTNO,A.ORDERNO,A.SUCODE,A.ORDERCODE,B.SNAME  ";
                        SQL = SQL + ComNum.VBLF + " FROM ADMIN.OCS_OORDER A, ADMIN.BAS_PATIENT B ";
                        SQL = SQL + ComNum.VBLF + "  WHERE A.PTNO=B.PANO(+)";
                        SQL = SQL + ComNum.VBLF + "   AND A.BDATE =TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND A.SUCODE ='" + strSucode + "' ";

                        if (strDept != "") SQL = SQL + ComNum.VBLF + "   AND A.DEPTCODE ='" + strDept + "' ";
                        SQL = SQL + ComNum.VBLF + "   UNION ALL ";
                        SQL = SQL + ComNum.VBLF + "  SELECT A.PTNO,A.ORDERNO,A.SUCODE,A.ORDERCODE,B.SNAME ";
                        SQL = SQL + ComNum.VBLF + " FROM ADMIN.OCS_IORDER A, ADMIN.BAS_PATIENT B ";
                        SQL = SQL + ComNum.VBLF + "  WHERE A.PTNO=B.PANO(+)";
                        SQL = SQL + ComNum.VBLF + "   AND A.BDATE =TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND A.SUCODE ='" + strSucode + "' ";

                        if (strDept != "") SQL = SQL + ComNum.VBLF + "   AND A.DEPTCODE ='" + strDept + "' ";
                        SQL = SQL + ComNum.VBLF + " ORDER BY 1,5 ";
                        break;

                    case "01": //'오더(외래);

                        SQL = "";
                        SQL = " SELECT A.PTNO,A.ORDERNO,A.SUCODE,A.ORDERCODE,B.SNAME ";
                        SQL = SQL + ComNum.VBLF + " FROM ADMIN.OCS_OORDER A, ADMIN.BAS_PATIENT B ";
                        SQL = SQL + ComNum.VBLF + "  WHERE A.PTNO=B.PANO(+)";
                        SQL = SQL + ComNum.VBLF + "   AND A.BDATE =TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND A.SUCODE ='" + strSucode + "' ";

                        if (strDept != "") SQL = SQL + ComNum.VBLF + "   AND A.DEPTCODE ='" + strDept + "' ";

                        SQL = SQL + ComNum.VBLF + " ORDER BY A.PTNO,B.SNAME ";

                        break;

                    case "02": // '오더(입원);
                        SQL = "";
                        SQL = " SELECT A.PTNO,A.ORDERNO,A.SUCODE,A.ORDERCODE,B.SNAME ";
                        SQL = SQL + ComNum.VBLF + " FROM ADMIN.OCS_IORDER A, ADMIN.BAS_PATIENT B ";
                        SQL = SQL + ComNum.VBLF + "  WHERE A.PTNO=B.PANO(+)";
                        SQL = SQL + ComNum.VBLF + "   AND A.BDATE =TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND A.SUCODE ='" + strSucode + "' ";

                        if (strDept != "") SQL = SQL + ComNum.VBLF + "   AND A.DEPTCODE ='" + strDept + "' ";

                        SQL = SQL + " ORDER BY A.PTNO,B.SNAME ";

                        break;

                    case "03": //'ETC_JUPMST;
                        SQL = "";
                        SQL = "SELECT A.PTNO,A.ORDERNO,A.ORDERCODE SUCODE,A.ORDERCODE,B.SNAME  ";
                        SQL = SQL + ComNum.VBLF + " FROM ADMIN.ETC_JUPMST A, ADMIN.BAS_PATIENT B ";
                        SQL = SQL + ComNum.VBLF + "  WHERE A.PTNO=B.PANO(+)";
                        SQL = SQL + ComNum.VBLF + "   AND A.RDATE >=TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND A.RDATE <TO_DATE('" + Convert.ToDateTime(strBDate).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND A.GBJOB ='3'  ";  //'완료건;
                        SQL = SQL + ComNum.VBLF + "   AND A.ORDERCODE ='" + strSucode + "' ";

                        if (strDept != "")
                        {
                            if (strDept != "**")
                            {
                                SQL = SQL + "   AND A.DEPTCODE ='" + strDept + "' ";
                            }
                        }
                        else
                        {
                            SQL = SQL + "   AND A.DEPTCODE NOT IN ('HR','TO') ";
                        }

                        SQL = SQL + " ORDER BY A.PTNO,B.SNAME ";

                        break;

                    case "04": //'ENDO_JUPMST;
                        SQL = "";
                        SQL = " SELECT A.PTNO,A.ORDERNO,A.ORDERCODE SUCODE,A.ORDERCODE,B.SNAME  ";
                        SQL = SQL + " FROM ADMIN.ENDO_JUPMST A, ADMIN.BAS_PATIENT B ";
                        SQL = SQL + "  WHERE A.PTNO=B.PANO(+)";
                        SQL = SQL + "   AND A.RDATE >=TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                        SQL = SQL + "   AND A.RDATE <TO_DATE('" + Convert.ToDateTime(strBDate).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + "   AND A.GBSUNAP ='7'  ";  //'완료건;
                        SQL = SQL + "   AND A.ORDERCODE ='" + strSucode + "' ";
                        SQL = SQL + "   AND A.BUSE = '056104'     ";  //'내시경실 시행만; 윤계장님 내시경 선생님 요청 자동조회에만 있던 것 추가.
                        SQL = SQL + "   AND A.RESULTDATE IS NOT NULL ";

                        if (strDept != "")
                        {
                            SQL = SQL + "   AND A.DEPTCODE ='" + strDept + "' ";
                        }

                        SQL = SQL + " ORDER BY A.PTNO,B.SNAME ";

                        break;

                    case "05": // 'ETC_JUSASUB - 용법,GBMASTER - 멀티처방;

                        SQL = "";
                        SQL = "SELECT A.PTNO,A.ORDERNO,A.ORDERCODE SUCODE,A.ORDERCODE,B.SNAME   ";
                        SQL = SQL + "  FROM ADMIN.ETC_JUSASUB  A, ADMIN.BAS_PATIENT B ";
                        SQL = SQL + " WHERE A.PTNO=B.PANO(+)";
                        SQL = SQL + "   AND A.ACTDATE = TO_DATE('" + strBDate + "','YYYY-MM-DD')";
                        SQL = SQL + "   AND A.GBACT = 'Y'";
                        SQL = SQL + "   AND A.DOSCODE ='" + strSucode + "' ";
                        SQL = SQL + " ORDER BY A.PTNO,B.SNAME ";
                        break;

                    case "06": // 'ETC_JUSASUB - 수가;

                        SQL = "";
                        SQL = "SELECT A.PTNO,A.ORDERNO,A.SUCODE,A.ORDERCODE,B.SNAME   ";
                        SQL = SQL + "FROM ADMIN.ETC_JUSASUB  A, ADMIN.BAS_PATIENT B ";
                        SQL = SQL + "WHERE A.PTNO=B.PANO(+)";
                        SQL = SQL + "   AND A.ACTDATE = TO_DATE('" + strBDate + "','YYYY-MM-DD')";
                        SQL = SQL + "   AND A.GBACT = 'Y'";
                        SQL = SQL + "   AND A.SUCODE ='" + strSucode + "' ";
                        SQL = SQL + "ORDER BY A.PTNO,B.SNAME ";
                        break;

                    case "07": //'XRAY_RESULTNEW 판독;

                        SQL = "";
                        SQL = "SELECT A.PANO PTNO,A.WRTNO ORDERNO ,A.XCODE SUCODE ,B.SNAME  ";
                        SQL = SQL + "FROM ADMIN.XRAY_RESULTNEW  A, ADMIN.BAS_PATIENT B ";
                        SQL = SQL + "WHERE A.PANO=B.PANO(+)";
                        SQL = SQL + "   AND A.READDATE = TO_DATE('" + strBDate + "','YYYY-MM-DD')";
                        SQL = SQL + "   AND A.XCODE ='" + strSucode + "' ";
                        SQL = SQL + "ORDER BY A.PANO,B.SNAME ";

                        break;

                    case "08": // 'XRAY_DETAIL;
                        SQL = "";
                        SQL = "  SELECT A.PANO PTNO,A.ORDERNO ,A.XCODE SUCODE ,B.SNAME ";
                        SQL = SQL + " FROM ADMIN.XRAY_DETAIL  A, ADMIN.BAS_PATIENT B ";
                        SQL = SQL + " WHERE A.PANO=B.PANO(+)";
                        SQL = SQL + "   AND A.SEEKDATE >= TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                        SQL = SQL + "   AND A.SEEKDATE < TO_DATE('" + Convert.ToDateTime(strBDate).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + "   AND A.XCODE ='" + strSucode + "' ";
                        SQL = SQL + "   AND A.GBRESERVED IN ('6','7') "; //'접수된것;
                        SQL = SQL + "   AND A.GBSTS <> 'D' ";
                        SQL = SQL + "   AND (A.PACSSTUDYID IS NULL OR (A.EXINFO IS NULL OR A.EXINFO < 1000)) "; //'영상이 없고 판독하지 않은것;

                        if (strDept != "")
                        {
                            SQL = SQL + "   AND A.DEPTCODE ='" + strDept + "' ";
                        }
                        else
                        {
                            SQL = SQL + "   AND A.DEPTCODE NOT IN ('HR','TO') ";
                        }

                        SQL = SQL + " ORDER BY A.PANO,B.SNAME ";

                        break;

                    default:
                        return intVal;
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return intVal;
                }

                if (dt.Rows.Count > 0)
                {

                    ArgSS.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ArgSS.ActiveSheet.RowCount = ArgSS.ActiveSheet.RowCount + 1;

                        ArgSS.ActiveSheet.Cells[ArgSS.ActiveSheet.RowCount - 1, 0].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                        ArgSS.ActiveSheet.Cells[ArgSS.ActiveSheet.RowCount - 1, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ArgSS.ActiveSheet.Cells[ArgSS.ActiveSheet.RowCount - 1, 2].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ArgSS.ActiveSheet.Cells[ArgSS.ActiveSheet.RowCount - 1, 3].Text = dt.Rows[i]["ORDERNO"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return intVal;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int i = 0;

            string strFDate = "";
            string strTDate = "";

            string strCODE = "";
            string strGubun = "";
            double nQty = 0;
            string strROWID = "";

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            DataTable dt = null;

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인


            if (cboDept.Text.Trim() == "전체")
            {
                ComFunc.MsgBox("전체에서는 입력이 불가능합니다..", "확인");
                return;
            }

            //'주사
            strFDate = dtpFDate.Value.ToString("yyyy-MM-dd");
            strTDate = dtpTDate.Value.ToString("yyyy-MM-dd");

            if (strFDate != strTDate)
            {
                ComFunc.MsgBox("입력날짜를 같게 해주십시오", "확인");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                for (i = 0; i < ss1_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data) + 1; i++)
                {
                    strCODE = ss1_Sheet1.Cells[i, 0].Text.Trim();
                    nQty = VB.Val(ss1_Sheet1.Cells[i, 2].Text.Trim());
                    strGubun = VB.Pstr(ss1_Sheet1.Cells[i, 6].Text.Trim(), "@@", 2).Trim();

                    SQL = " SELECT ROWID FROM ADMIN.NUR_JUSASIL ";
                    SQL = SQL + "   WHERE ACTDATE =TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + "    AND CODE = '" + strCODE + "' ";
                    SQL = SQL + "    AND GUBUN ='" + strGubun + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    strROWID = "";

                    if (dt.Rows.Count > 0)
                    {
                        strROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;

                    if (strROWID != "")
                    {
                        SQL = "";
                        SQL = "UPDATE ADMIN.NUR_JUSASIL SET ";
                        SQL = SQL + " CODE = '" + strCODE + "',";
                        SQL = SQL + " QTY = '" + Convert.ToString(nQty) + "'";
                        SQL = SQL + " WHERE ROWID = '" + strROWID + "'";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                    }
                    else
                    {
                        if (nQty >= 0)
                        {
                            SQL = "";
                            SQL = "INSERT INTO ADMIN.NUR_JUSASIL (ACTDATE, GUBUN, CODE, QTY)";
                            SQL = SQL + " VALUES (TO_DATE('" + strTDate + "','YYYY-MM-DD'), ";
                            SQL = SQL + " '" + strGubun + "','" + strCODE + "', '" + nQty + "') ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                    }

                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("자료가 정상적으로 등록되었습니다.", "확인");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }

            btnChk.Enabled = false;
            btnSet.Enabled = false;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            ss1_Sheet1.RowCount = ss1_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data) + 1;


            ss1_Sheet1.RowCount = ss1_Sheet1.RowCount + 1;

            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 0].Text = "합계";
            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 2].Text = lblTot.Text.Trim();
            ss1_Sheet1.Rows.Get(ss1_Sheet1.RowCount - 1).BackColor = Color.White;
            ss1_Sheet1.Rows.Get(ss1_Sheet1.RowCount - 1).Border = new FarPoint.Win.LineBorder(Color.Black, 2,false,true,false,false);

            Cursor.Current = Cursors.WaitCursor;


            //'Print Head 지정
            strFont1 = "/fn\"굴림체\" /fz\"20\" /fb1 /fi0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"10\" /fb1 /fi0 /fk0 /fs2";
            strHead1 = "/c/f1" + "외래 특수검사,주사통계" + "/f1/n/n";
            strHead2 = "조회기간 :" + dtpFDate.Value.ToString("yyyy-MM-dd") + " ~ " + dtpTDate.Value.ToString("yyyy-MM-dd");
            strHead2 = strHead2 + "                 인쇄일자: " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A");


            ss1_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;

            ss1_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;// 가로 출력방향
            ss1_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;// 세로 출력방향

            ss1_Sheet1.PrintInfo.AbortMessage = "Printing - Click Cancel to quit";

            ss1_Sheet1.PrintInfo.Margin.Top = 40;
            ss1_Sheet1.PrintInfo.Margin.Bottom = 30;
            ss1_Sheet1.PrintInfo.Margin.Left = 60;
            ss1_Sheet1.PrintInfo.Margin.Right = 10;
            ss1_Sheet1.PrintInfo.HeaderHeight = 50; // 해드 높이
            ss1_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal; // 스프레드를 중앙으로 만듬.
            ss1_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2; //해드 넣는 부분
            ss1_Sheet1.PrintInfo.ShowColor = false;
            ss1_Sheet1.PrintInfo.ShowBorder = true; // 출력할 스프레드 전체 태두리
            ss1_Sheet1.PrintInfo.ShowGrid = true;
            ss1_Sheet1.PrintInfo.ShowShadows = false;
            ss1_Sheet1.PrintInfo.UseMax = true;

            ss1_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ss1_Sheet1.PrintInfo.UseSmartPrint = false;

            ss1_Sheet1.PrintInfo.ShowPrintDialog = false; // 프린트 설정 창
            ss1_Sheet1.PrintInfo.Preview = false; // 미리보기 창
            ss1.PrintSheet(0);

            Cursor.Current = Cursors.Default;
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            int i = 0;
            //string strRWOID = "";
            string strDel = "";

            string strSucode = "";
            string strSuName = "";
            string strAuto = "";
            string strUse = "";
            string strDEPT = "";
            string strChange = "";
            string strROWID = "";

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (mstrExamCode == "")
            {
                ComFunc.MsgBox("검사코드를 선택후 작업하십시오", "확인");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {

                for (i = 0; i < ss2_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data) + 1; i++)
                {
                    strDel = "";

                    if (Convert.ToBoolean(ss2_Sheet1.Cells[i, 0].Value) == true)
                    {
                        strDel = "Y";
                    }
                    strSucode = ss2_Sheet1.Cells[i, 1].Text.Trim();
                    strSuName = ss2_Sheet1.Cells[i, 2].Text.Trim();


                    //strAuto = ss2_Sheet1.Cells[i, 3].Text.Trim();
                    if (mstrExamPart.Trim() == "내시경")
                    {
                        strAuto = "04";
                    }
                    if (strAuto == "")
                    {
                        strAuto = "00";
                    }
                    strUse = ss2_Sheet1.Cells[i, 4].Text.Trim();
                    strDEPT = ss2_Sheet1.Cells[i, 5].Text.Trim();
                    strChange = ss2_Sheet1.Cells[i, 6].Text.Trim();
                    strROWID = ss2_Sheet1.Cells[i, 7].Text.Trim();

                    if (strROWID == "")
                    {
                        //'저장
                        if (strDel != "Y")
                        {
                            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

                            SQL = "";
                            SQL = " INSERT INTO ADMIN.NUR_CODE_EXAM ( PART,CODE,SUCODE,NAME,AUTO,USE,DEPT,RANKING,ENTDATE,ENTSABUN ) VALUES ( ";
                            SQL = SQL + ComNum.VBLF + " '" + mstrExamPart.Trim() + "','" + mstrExamCode.Trim() + "','" + strSucode + "','" + strSuName + "', ";

                            if (strAuto == "")
                            {
                                SQL = SQL + ComNum.VBLF + " '00','Y','" + strDEPT + "' ,1,SYSDATE," + clsPublic.GnJobSabun + " ) ";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + " '" + strAuto + "','Y','" + strDEPT + "' ,1,SYSDATE," + clsPublic.GnJobSabun + " ) ";
                            }


                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                    }
                    else if (strChange == "Y")
                    {
                        if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

                        //'갱신
                        SQL = "";
                        SQL = " UPDATE ADMIN.NUR_CODE_EXAM SET ";
                        SQL = SQL + ComNum.VBLF + " NAME ='" + strSuName + "', ";
                        SQL = SQL + ComNum.VBLF + " AUTO ='" + strAuto + "', ";
                        SQL = SQL + ComNum.VBLF + " USE ='" + strUse + "', ";
                        SQL = SQL + ComNum.VBLF + " DEPT ='" + strDEPT + "', ";
                        SQL = SQL + ComNum.VBLF + " ENTDATE =SYSDATE,  ";
                        SQL = SQL + ComNum.VBLF + " ENTSABUN =" + clsPublic.GnJobSabun + "  ";
                        SQL = SQL + ComNum.VBLF + "  WHERE ROWID ='" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }

            READ_EXAM_CODE2_SUCODE();
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            int i = 0;
            int nCNT = 0;
            //'해당파트 검사코드 읽기

            for (i = 0; i < ss1_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data) + 1; i++)
            {

                nCNT = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[i, 8].Text.Trim()));

                ss1_Sheet1.Cells[i, 2].Text = Convert.ToString(nCNT);

                if (nCNT == 0)
                {
                    ss1_Sheet1.Cells[i, 2].Text = "";
                }
            }

            ComFunc.MsgBox("자동건수적용후 등록하셔야 정상 저장됩니다.!!", "확인");
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            int i = 0;
            int h = 0;
            string strFDate = "";
            string strTDate = "";
            string strCodes = "";
            string strDEPT = "";
            string strCODE = "";

            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            mstrExamPart = "";
            mstrExamCode = "";

            ss1_Sheet1.RowCount = 0;
            ss10_Sheet1.RowCount = 0;
            ss2_Sheet1.RowCount = 0;
            ssJusa_Sheet1.RowCount = 0;
            ssDetail_Sheet1.RowCount = 0;

            strFDate = dtpFDate.Value.ToString("yyyy-MM-dd");
            strTDate = dtpTDate.Value.ToString("yyyy-MM-dd");

            this.Enabled = false;

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    this.Enabled = true;
                    return; //권한 확인
                }


                //'부서세팅
                SQL = "SELECT NAME     ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.NUR_CODE ";
                SQL = SQL + ComNum.VBLF + "  WHERE GUBUN ='Z' ";

                if (cboDept.Text.Trim() != "전체")
                {
                    SQL = SQL + ComNum.VBLF + "   AND CODE ='" + cboDept.Text.Trim() + "' ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                strDEPT = "";
                strCodes = "";

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    this.Enabled = true;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strDEPT = dt.Rows[0]["NAME"].ToString().Trim();

                    for (i = 0; i < VB.Split(strDEPT, ",").Length; i++)
                    {
                        strCodes = strCodes + "'" + VB.Split(strDEPT, ",")[i] + "',";
                    }

                    strCodes = VB.Mid(strCodes, 1, strCodes.Length - 1);
                }

                dt.Dispose();
                dt = null;

                //'기초코드 불러오기;
                SQL = "";
                SQL = " SELECT CODE, NAME, DECODE(GUBUN,'6','특수','3','주사','5','항암') GUBUN1,";
                SQL = SQL + ComNum.VBLF + " DECODE(GUBUN,'6','1','3','2','5','3') GUBUN, GBUSE  ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.NUR_CODE ";
                SQL = SQL + ComNum.VBLF + "  WHERE GUBUN IN ('6', '3','5') ";

                if (chkAll.Checked == false)
                {
                    SQL = SQL + ComNum.VBLF + "    AND ( GBUSE IS NULL OR GBUSE ='Y' ) ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND ( GBUSE IS NULL OR GBUSE ='Y' OR GBUSE ='N' )   ";
                }

                if (cboDept.Text.Trim() != "전체")
                {
                    SQL = SQL + ComNum.VBLF + "  AND CODE IN ( " + strCodes + " ) ";

                    if (cboDept.Text.Trim() == "주사실")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND GUBUN ='3' ";
                    }
                    else if (cboDept.Text.Trim() == "항암주사")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND GUBUN ='5' ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "  AND GUBUN ='6' ";
                    }
                }

                if (Convert.ToDateTime(strTDate) < Convert.ToDateTime("2012-07-01"))
                {
                    SQL = SQL + ComNum.VBLF + "  AND CODE NOT IN ('190') "; //'2012-07-03 외래 신은지 간호사 요청 강제 7월이전에서는 코드않보이게처리;
                }

                SQL = SQL + ComNum.VBLF + "   ORDER BY GUBUN ,PRINTRANKING ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    this.Enabled = true;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ss1_Sheet1.RowCount = dt.Rows.Count;
                    ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["CODE"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 1].Text = VB.Space(2) + dt.Rows[i]["NAME"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["GUBUN1"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["CODE"].ToString().Trim() + "@@" + dt.Rows[i]["GUBUN"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["GBUSE"].ToString().Trim();

                        if (dt.Rows[i]["GBUSE"].ToString().Trim() == "1")
                        {
                            ss1_Sheet1.Cells[i, 3].ForeColor = Color.FromArgb(0, 0, 0);
                            ss1_Sheet1.Cells[i, 3].BackColor = Color.FromArgb(202, 202, 255);
                        }

                        SQL = "";
                        SQL = " SELECT CODE FROM ADMIN.NUR_CODE ";
                        SQL = SQL + "  WHERE GUBUN ='Z' ";
                        SQL = SQL + "   AND JIK ='" + dt.Rows[i]["GUBUN"].ToString().Trim() + "' ";
                        SQL = SQL + "  AND NAME LIKE '%" + dt.Rows[i]["CODE"].ToString().Trim() + "%' ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            this.Enabled = true;
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            ss1_Sheet1.Cells[i, 5].Text = VB.Space(2) + dt1.Rows[0]["CODE"].ToString().Trim();
                        }

                        dt1.Dispose();
                        dt1 = null;

                    }
                }
                //'해당일자;
                dt.Dispose();
                dt = null;

                if (Convert.ToDateTime(strFDate) > dtpTDate.Value)
                {
                    ComFunc.MsgBox("작업일자를 다시 입력하세요..", "확인");
                    this.Enabled = true;
                    return;
                }
                if (strFDate == strTDate)
                {
                    SQL = "";
                    SQL = " SELECT GUBUN, CODE, QTY, ROWID";
                    SQL = SQL + " FROM ADMIN.NUR_JUSASIL";
                    SQL = SQL + " WHERE ACTDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + " AND ACTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";

                    if (cboDept.Text.Trim() != "전체")
                    {
                        SQL = SQL + "  AND CODE IN ( " + strCodes + " ) ";
                        if (cboDept.Text.Trim() == "주사실")
                        {
                            SQL = SQL + "  AND GUBUN ='2' ";
                        }
                        else if (cboDept.Text.Trim() == "항암주사")
                        {
                            SQL = SQL + "  AND GUBUN ='3' ";
                        }
                        else
                        {
                            SQL = SQL + "  AND GUBUN ='1' ";
                        }
                    }                    
                    SQL = SQL + "  ORDER BY GUBUN ,CODE ";
                }
                else
                {
                    SQL = "";
                    SQL = " SELECT GUBUN, CODE, SUM(QTY) QTY   ";
                    SQL = SQL + " FROM ADMIN.NUR_JUSASIL";
                    SQL = SQL + " WHERE ACTDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + " AND ACTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";

                    if (cboDept.Text.Trim() != "전체")
                    {
                        SQL = SQL + "  AND CODE IN ( " + strCodes + " ) ";
                        if (cboDept.Text.Trim() == "주사실")
                        {
                            SQL = SQL + "  AND GUBUN ='2' ";
                        }
                        else if (cboDept.Text.Trim() == "항암주사")
                        {
                            SQL = SQL + "  AND GUBUN ='3' ";
                        }
                        else
                        {
                            SQL = SQL + "  AND GUBUN ='1' ";
                        }
                    }
                    SQL = SQL + " GROUP BY GUBUN, CODE ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    this.Enabled = true;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strCODE = dt.Rows[i]["CODE"].ToString().Trim() + "@@" + dt.Rows[i]["GUBUN"].ToString().Trim();

                        for (h = 0; h < ss1_Sheet1.GetLastNonEmptyRow(NonEmptyItemFlag.Data) + 1; h++)
                        {
                            if (strCODE == ss1_Sheet1.Cells[h, 6].Text.Trim())
                            {
                                ss1_Sheet1.Cells[h, 2].Text = dt.Rows[i]["QTY"].ToString().Trim();
                                if (strFDate == strTDate)
                                {
                                    ss1_Sheet1.Cells[h, 7].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                                }
                                break;
                            }
                        }
                    }
                }
                dt.Dispose();
                dt = null;

                //'건수합계보이기
                Count_Sub_Tot();

                btnChk.Enabled = true;
                btnSet.Enabled = false;

                Application.DoEvents();

                if (cboDept.Text == "주사실")
                {
                    Jusa_Tong_Temp(dtpBDate.Value.ToString("yyyy-MM-dd")); //'주사임시통계용
                }
                else if (cboDept.Text == "류마티스")
                {
                    Jusa_Tong_RA(dtpBDate.Value.ToString("yyyy-MM-dd"));// '주사임시통계용
                }
                else if (cboDept.Text == "항암주사")
                {
                    Jusa_Tong_CANCER(dtpBDate.Value.ToString("yyyy-MM-dd"));// '주사임시통계용
                }

                this.Enabled = true;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                this.Enabled = true;
            }
        }

        private void Count_Sub_Tot()
        {
            int i = 0;
            double nSum = 0;

            lblTot.Text = "";

            for (i = 0; i < ss1_Sheet1.GetLastNonEmptyRow(NonEmptyItemFlag.Data) + 1; i++)
            {
                nSum = nSum + VB.Val(ss1_Sheet1.Cells[i, 2].Text.Trim());
            }

            lblTot.Text = Convert.ToString(nSum);
        }

        private void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            panAuto.Visible = false;
            switch (cboDept.Text.Trim())
            {
                case "전체":
                    panAuto.Visible = false;
                    break;

                case "내과":
                    panAuto.Visible = true;
                    break;

                case "내시경":
                    panAuto.Visible = true;
                    break;

                case "류마티스":
                    panAuto.Visible = true;
                    break;

                case "심전도":
                    panAuto.Visible = true;
                    break;

                case "안과":
                    panAuto.Visible = true;
                    break;

                case "정형외과":
                    panAuto.Visible = true;
                    break;

                case "산부인과":
                    panAuto.Visible = true;
                    break;

                case "비뇨기과":
                    panAuto.Visible = true;
                    break;

                case "주사실":
                case "항암주사":
                    panAuto.Visible = true;
                    break;
                case "치과":
                    panAuto.Visible = true;
                    break;

                case "재활의학":
                    panAuto.Visible = true;
                    break;

                case "이비인후":
                    panAuto.Visible = true;
                    break;

                case "일반외과":
                    panAuto.Visible = true;
                    break;

                case "흉부외과":
                    panAuto.Visible = true;
                    break;

                case "심장초음":
                    panAuto.Visible = true;
                    break;

                case "가정의학":
                    panAuto.Visible = true;
                    break;

                case "피부과":
                    panAuto.Visible = true;
                    break;

                //2019-02-01 안정수, 영상주사 추가
                case "영상주사":
                    panAuto.Visible = true;
                    break;
            }

            switch (cboDept.Text.Trim())
            {
                case "주사실":
                case "류마티스":
                case "항암주사":
                    panJtong.Visible = true;
                    panDetail.Visible = true;
                    this.Width = 990;
                    panel6.Visible = false;
                    panel10.Visible = true;
                    panel11.Visible = false;
                    panel12.Visible = true;
                    break;
                default:
                    panJtong.Visible = false;
                    panDetail.Visible = false;
                    this.Width = 640;
                    panel6.Visible = true;
                    panel10.Visible = false;
                    panel11.Visible = true;
                    panel12.Visible = false;
                    break;
            }

            if (clsPublic.GnJobSabun == 4349)
            {
                panAuto.Visible = true;
            }

            if (panAuto.Visible == true)
            {
                lblUse.Visible = true;
            }

        }

        private void btnCode_Click(object sender, EventArgs e)
        {

            panel6.Visible = false;
            panel10.Visible = false;
            panel11.Visible = false;
            panel12.Visible = false;

            if (cboDept.Text.Trim() == "주사실" || cboDept.Text.Trim() == "류마티스" || cboDept.Text.Trim() == "항암주사")
            {
                if (this.Width == 1275)
                {
                    this.Width = 990;
                    panel10.Visible = true;
                    panel12.Visible = true;
                    panel6.Visible = false;
                    panel11.Visible = false;
                }
                else
                {
                    this.Width = 1275;
                    panel6.Visible = true;
                    panel10.Visible = true;
                    panel11.Visible = true;
                    panel12.Visible = true;
                }
            }
            else
            {
                if (this.Width == 640)
                {
                    this.Width = 930;
                    panel6.Visible = true;
                    panel11.Visible = true;
                }
                else
                {
                    this.Width = 640;
                }
            }



        }

        private void ss1_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            //string strExamCode = "";

            int i = 0;
            string strCODE = "";
            string strGubun = "";
            string strSucode = "";
            string strDEPT = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            mstrExamPart = cboDept.Text.Trim();
            mstrExamCode = "";

            lblSTS.Text = "";

            if (e.RowHeader == true || e.ColumnHeader == true) return;
            if (ss1_Sheet1.RowCount == 0) return;

            if (e.Column == 5 && cboDept.Text.Trim() != "전체")
            {
                mstrExamCode = ss1_Sheet1.Cells[e.Row, 0].Text.Trim();

                lblSTS.Text = mstrExamPart + " " + mstrExamCode;

                if (mstrExamCode != "")
                {
                    READ_EXAM_CODE2_SUCODE();
                }
            }
            else if (e.Column == 8 && cboDept.Text.Trim() != "전체")
            {
                //'상세내역
                ss10_Sheet1.RowCount = 0;

                strCODE = ss1_Sheet1.Cells[e.Row, 0].Text.Trim();

                try
                {
                    if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                    SQL = "";
                    SQL = "SELECT SUCODE,AUTO,DEPT FROM ADMIN.NUR_CODE_EXAM ";
                    SQL = SQL + ComNum.VBLF + "  WHERE TRIM(PART) ='" + mstrExamPart + "'  ";
                    SQL = SQL + ComNum.VBLF + "  AND CODE ='" + strCODE + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND (DELDATE IS NULL OR DELDATE ='') ";
                    SQL = SQL + ComNum.VBLF + "  AND USE ='Y' ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY CODE,SUCODE ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strSucode = "";
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            strGubun = dt.Rows[i]["AUTO"].ToString().Trim();
                            strSucode = dt.Rows[i]["SUCODE"].ToString().Trim();
                            strDEPT = dt.Rows[i]["DEPT"].ToString().Trim();
                            //'상세내역;
                            READ_TONG_CNT_DETAIL(ss10, strGubun, dtpBDate.Value.ToString("yyyy-MM-dd"), mstrExamPart, strCODE, strSucode, strDEPT);
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }
                catch (Exception ex)
                {
                    if (dt != null)
                    {
                        dt.Dispose();
                        dt = null;
                    }
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(ex.Message);
                }
            }
        }

        private void ss2_EditChange(object sender, EditorNotifyEventArgs e)
        {
            ss2_Sheet1.Cells[e.Row, 6].Text = "Y";
        }

        private void dtpFDate_ValueChanged(object sender, EventArgs e)
        {
            dtpTDate.Value = dtpFDate.Value;
        }

        private void dtpTDate_ValueChanged(object sender, EventArgs e)
        {
            dtpBDate.Value = dtpTDate.Value;
        }

        private void btnDel2_Click(object sender, EventArgs e)
        {
            int i = 0;
            //string strRWOID = "";
            string strDel = "";

            //string strSucode = "";
            //string strSuName = "";
            //string strAuto = "";
            //string strUse = "";
            //string strDEPT = "";
            //string strChange = "";
            string strROWID = "";

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (mstrExamCode == "")
            {
                ComFunc.MsgBox("검사코드를 선택후 작업하십시오", "확인");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);


            try
            {

                for (i = 0; i < ss2_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data) + 1; i++)
                {
                    strDel = "";

                    if (Convert.ToBoolean(ss2_Sheet1.Cells[i, 0].Value) == true)
                    {
                        strDel = "Y";
                    }
                    strROWID = ss2_Sheet1.Cells[i, 7].Text.Trim();

                    if (strROWID != "")
                    {
                        //'저장
                        if (strDel == "Y")
                        {
                            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

                            //'갱신
                            SQL = "";
                            SQL = " UPDATE ADMIN.NUR_CODE_EXAM SET ";
                            SQL = SQL + ComNum.VBLF + " USE ='N',";
                            SQL = SQL + ComNum.VBLF + " DELDATE = TRUNC(SYSDATE) ";
                            SQL = SQL + ComNum.VBLF + "  WHERE ROWID ='" + strROWID + "' ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }

            READ_EXAM_CODE2_SUCODE();
        }
    }
}
