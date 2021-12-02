using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

/// <summary>
/// Description : 전원 통계 조회
/// Author : 김형범
/// Create Date : 2017.06.30
/// </summary>
/// <history>
/// 완료, form 3개 미생성
/// </history>
namespace ComLibB
{
    /// <summary> 전원 통계 조회 </summary>
    public partial class frmSearchJunwonTong : Form
    {
        /// <summary> 전원 통계 조회 </summary>
        public frmSearchJunwonTong()
        {
            InitializeComponent();
        }

        //TODO: FormInfo_History
        void frnSearchJunwonTong_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            //Call FormInfo_History(Me.Name, Me.Caption)

            cboGubun1.Items.Clear();
            cboGubun1.Items.Add("전체");
            cboGubun1.Items.Add("1.연고지");
            cboGubun1.Items.Add("2.3차 병원 진료 원해");
            cboGubun1.Items.Add("3.기존 진료받던 병원");
            cboGubun1.Items.Add("4.지정병원");
            cboGubun1.Items.Add("5.지인(병원)");
            cboGubun1.Items.Add("6.기타");
            cboGubun1.SelectedIndex = 0;

            cboGubun2.Items.Clear();
            cboGubun2.Items.Add("전체");
            cboGubun2.Items.Add("1.진료과 부재");
            cboGubun2.Items.Add("2.응급검사/처치불가");
            cboGubun2.Items.Add("3.고위험환자로 3차병원 권유");
            cboGubun2.Items.Add("4.응급수술 지연");
            cboGubun2.Items.Add("5.전문응급의료를 요하여");
            cboGubun2.Items.Add("6.병동, 병실부족");
            cboGubun2.Items.Add("7.중환자실 병동 부족");
            cboGubun2.Items.Add("8.연고지 관계");
            cboGubun2.Items.Add("9.기타");
            cboGubun2.SelectedIndex = 0;
        }

        //TODO Frm전원통계환자목록 미생성
        void btnTranList_Click(object sender, EventArgs e)
        {
            //Frm전원통계환자목록 frm = new Frm전원통계환자목록();
            //frm.Show();
        }

        //TODO: Frm전원통계비교 미생성
        void btnTranscompare_Click(object sender, EventArgs e)
        {
            //Frm전원통계비교 frm = new Frm전원통계비교();
            //frm.Show();
        }

        //TODO: Frm전원사유별통계_입원 미생성
        void btnSayuTong_Click(object sender, EventArgs e)
        {
            //Frm전원사유별통계_입원 frm = new Frm전원사유별통계_입원();
            //frm.Show();
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            string strSDATE = "";
            string strEDATE = "";

            strSDATE = dtpDate.Value.ToString("yyyy") + "-01-01 00:00";
            strEDATE = dtpDate.Value.ToString("yyyy") + "-12-31 23:59";

            SumTrans(strSDATE, strEDATE);
            SumHospital(strSDATE, strEDATE);
            SumDept(strSDATE, strEDATE);
        }

        void SumTrans(string strSDATE, string strEDATE)
        {

            int i = 0;
            int j = 0;
            int intTSUM = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                for (i = 1; i < ssView1_Sheet1.Columns.Count; i++)
                {
                    for (j = 0; j < 2; j++)
                    {
                        ssView1_Sheet1.Cells[j, i].Text = "";
                    }
                }

                //전원 환자 합계
                SQL = "";
                SQL = "SELECT TO_CHAR(TDATE, 'MM') TDATE, SUM(1) TSUM, 'I' GUBUN";
                SQL = SQL + ComNum.VBLF + "  From " + ComNum.DB_PMPA + "NUR_QI_TRANSFOR";
                SQL = SQL + ComNum.VBLF + " WHERE TDATE >= TO_DATE('" + strSDATE + "','YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "      AND TDATE <= TO_DATE('" + strEDATE + "','YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "      AND DELDATE IS NULL";

                if (cboGubun1.Text != "전체")
                {
                    switch (VB.Left(cboGubun1.Text, 1).Trim())
                    {
                        case "1":
                            SQL = SQL + ComNum.VBLF + " AND SAYUP1 = '1' ";
                            break;
                        case "2":
                            SQL = SQL + ComNum.VBLF + " AND SAYUP2 = '1' ";
                            break;
                        case "3":
                            SQL = SQL + ComNum.VBLF + " AND SAYUP3 = '1' ";
                            break;
                        case "4":
                            SQL = SQL + ComNum.VBLF + " AND SAYUP4 = '1' ";
                            break;
                        case "5":
                            SQL = SQL + ComNum.VBLF + " AND SAYUP5 = '1' ";
                            break;
                        case "6":
                            SQL = SQL + ComNum.VBLF + " AND SAYUPETC IS NOT NULL ";
                            break;
                    }
                }

                if (cboGubun2.Text.Trim() != "전체")
                {
                    switch (VB.Left(cboGubun2.Text, 1).Trim())
                    {
                        case "1":
                            SQL = SQL + ComNum.VBLF + " AND SAYUH1 = '1' ";
                            break;
                        case "2":
                            SQL = SQL + ComNum.VBLF + " AND (SAYUH2A = '1' OR SAYUH2B = '1' OR SAYUH2C = '1' OR SAYUH2D = '1' OR SAYUH2E = '1' OR SAYUH2F = '1' OR SAYUH2G = '1' OR SAYUH2ETC IS NOT NULL) ";
                            break;
                        case "3":
                            SQL = SQL + ComNum.VBLF + " AND SAYUH3 = '1' ";
                            break;
                        case "4":
                            SQL = SQL + ComNum.VBLF + " AND SAYUH4 = '1' ";
                            break;
                        case "5":
                            SQL = SQL + ComNum.VBLF + " AND (SAYUH5A = '1' OR SAYUH5B = '1' OR SAYUH5C = '1' OR SAYUH5ETC IS NOT NULL)";
                            break;
                        case "6":
                            SQL = SQL + ComNum.VBLF + " AND SAYUH6 = '1' ";
                            break;
                        case "7":
                            SQL = SQL + ComNum.VBLF + " AND SAYUH7 = '1' ";
                            break;
                        case "8":
                            SQL = SQL + ComNum.VBLF + " AND SAYUH8 = '1' ";
                            break;
                        case "9":
                            SQL = SQL + ComNum.VBLF + " AND SAYUHETC IS NOT NULL '1' ";
                            break;
                    }
                }

                SQL = SQL + ComNum.VBLF + "      GROUP BY TO_CHAR(TDATE, 'MM'), 'I'";
                SQL = SQL + ComNum.VBLF + " Union All";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(OUTTIME, 'MM') TDATE, SUM(1) TSUM, 'E' GUBUN";
                SQL = SQL + ComNum.VBLF + "  From NUR_ER_PATIENT";
                SQL = SQL + ComNum.VBLF + " WHERE OUTTIME >= TO_DATE('" + strSDATE + "','YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "           AND OUTTIME <= TO_DATE('" + strEDATE + "','YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "           AND OUTGBN = '6'";
                SQL = SQL + ComNum.VBLF + " GROUP BY TO_CHAR(OUTTIME, 'MM'), 'E' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView1_Sheet1.RowCount = 3;

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        switch (dt.Rows[i]["GUBUN"].ToString().Trim())
                        {
                            case "E":
                                ssView1_Sheet1.Cells[0, Convert.ToInt32(dt.Rows[i]["TDATE"].ToString().Trim())].Text = dt.Rows[i]["TSUM"].ToString().Trim();
                                ssView1_Sheet1.Cells[0, Convert.ToInt32(dt.Rows[i]["TDATE"].ToString().Trim())].Text = ssView1_Sheet1.Cells[0, Convert.ToInt32(dt.Rows[i]["TDATE"].ToString().Trim())].Text == "0" ? "" : ssView1_Sheet1.Cells[0, Convert.ToInt32(dt.Rows[i]["TDATE"].ToString().Trim())].Text;
                                break;
                            case "I":
                                ssView1_Sheet1.Cells[1, Convert.ToInt32(dt.Rows[i]["TDATE"].ToString().Trim())].Text = dt.Rows[i]["TSUM"].ToString().Trim();
                                ssView1_Sheet1.Cells[1, Convert.ToInt32(dt.Rows[i]["TDATE"].ToString().Trim())].Text = (ssView1_Sheet1.Cells[1, Convert.ToInt32(dt.Rows[i]["TDATE"].ToString().Trim())].Text == "0" ? "" : ssView1_Sheet1.Cells[1, Convert.ToInt32(dt.Rows[i]["TDATE"].ToString().Trim())].Text);
                                break;
                        }
                    }
                }


                dt.Dispose();
                dt = null;

                //가로
                for (i = 1; i < 13; i++)
                {
                    intTSUM = 0;
                    for (j = 0; j < 2; j++)
                    {
                        intTSUM += Convert.ToInt32(VB.Val(ssView1_Sheet1.Cells[j, i].Text.Trim()));
                    }
                    ssView1_Sheet1.Cells[2, i].Text = intTSUM.ToString();
                }

                //합계
                for (j = 0; j < 3; j++)
                {
                    intTSUM = 0;
                    for (i = 1; i < 13; i++)
                    {
                        intTSUM += Convert.ToInt32(VB.Val(ssView1_Sheet1.Cells[j, i].Text.Trim()));
                    }
                    ssView1_Sheet1.Cells[j, 13].Text = intTSUM.ToString();
                    ssView1_Sheet1.Cells[j, 14].Text = VB.Format(intTSUM / (double)12, "0.0");
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        void SumHospital(string strSDATE, string strEDATE)
        {

            int i = 0;
            int j = 0;
            int intTSUM = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                //병원별 합계-입원
                ssView2_Sheet1.RowCount = 0;

                SQL = "";
                SQL = " SELECT THOSNAME FROM " + ComNum.DB_PMPA + "NUR_QI_TRANSFOR";
                SQL = SQL + ComNum.VBLF + "WHERE TDATE >= TO_DATE('" + strSDATE + "','YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "     AND TDATE <= TO_DATE('" + strEDATE + "','YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "     AND DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "  GROUP BY THOSNAME ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView2_Sheet1.RowCount = dt.Rows.Count;

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["THOSNAME"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "     SELECT TO_CHAR(TDATE, 'MM') TDATE, THOSNAME, SUM(1) TSUM, 'I' GUBUN";
                SQL = SQL + ComNum.VBLF + "   From " + ComNum.DB_PMPA + "NUR_QI_TRANSFOR";
                SQL = SQL + ComNum.VBLF + "WHERE TDATE >= TO_DATE('" + strSDATE + "','YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "     AND TDATE <= TO_DATE('" + strEDATE + "','YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "     AND DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "     GROUP BY TO_CHAR(TDATE, 'MM'), THOSNAME, 'I'";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        for (j = 0; j < ssView2_Sheet1.RowCount; j++)
                        {
                            if (ssView2_Sheet1.Cells[j, 0].Text == dt.Rows[i]["THOSNAME"].ToString().Trim())
                            {
                                break;
                            }
                        }

                        ssView2_Sheet1.Cells[j, Convert.ToInt32(VB.Val(dt.Rows[i]["TDATE"].ToString().Trim()))].Text = dt.Rows[i]["TSUM"].ToString().Trim();
                        ssView2_Sheet1.Cells[j, Convert.ToInt32(VB.Val(dt.Rows[i]["TDATE"].ToString().Trim()))].Text = (ssView2_Sheet1.Cells[j, Convert.ToInt32(VB.Val(dt.Rows[i]["TDATE"].ToString().Trim()))].Text == "0" ? "" : ssView2_Sheet1.Cells[j, Convert.ToInt32(VB.Val(dt.Rows[i]["TDATE"].ToString().Trim()))].Text);
                    }
                }

                dt.Dispose();
                dt = null;

                ssView2_Sheet1.RowCount = ssView2_Sheet1.RowCount + 1;

                ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 0].Text = "합계";

                for (i = 1; i < 13; i++)
                {
                    intTSUM = 0;

                    for (j = 0; j < ssView2_Sheet1.RowCount; j++)
                    {
                        intTSUM += Convert.ToInt32(VB.Val(ssView2_Sheet1.Cells[j, i].Text.Trim()));
                    }

                    ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, i].Text = intTSUM.ToString();
                }

                //합계
                for (j = 0; j < ssView2_Sheet1.RowCount; j++)
                {
                    intTSUM = 0;

                    for (i = 1; i < 13; i++)
                    {
                        intTSUM += Convert.ToInt32(VB.Val(ssView2_Sheet1.Cells[j, i].Text.Trim()));
                    }

                    ssView2_Sheet1.Cells[j, 13].Text = intTSUM.ToString();
                    ssView2_Sheet1.Cells[j, 14].Text = VB.Format(intTSUM / 12, "0.0");
                }

                //병원별 합계-응급실
                ssView3_Sheet1.RowCount = 0;

                SQL = "";
                SQL = "     SELECT HUSONGNAME From NUR_ER_PATIENT";
                SQL = SQL + ComNum.VBLF + "WHERE OUTTIME >= TO_DATE('" + strSDATE + "','YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "     AND OUTTIME <= TO_DATE('" + strEDATE + "','YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "     AND OUTGBN = '6'";
                SQL = SQL + ComNum.VBLF + "     GROUP BY HUSONGNAME ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView3_Sheet1.RowCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView3_Sheet1.Cells[i, 0].Text = dt.Rows[i]["HUSONGNAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "     SELECT TO_CHAR(OUTTIME, 'MM') TDATE, HUSONGNAME, SUM(1) TSUM, 'E' GUBUN";
                SQL = SQL + ComNum.VBLF + "From NUR_ER_PATIENT";
                SQL = SQL + ComNum.VBLF + "WHERE OUTTIME >= TO_DATE('" + strSDATE + "','YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "     AND OUTTIME <= TO_DATE('" + strEDATE + "','YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "     AND OUTGBN = '6'";
                SQL = SQL + ComNum.VBLF + "     GROUP BY TO_CHAR(OUTTIME, 'MM'), HUSONGNAME, 'E'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    for (j = 0; j < ssView3_Sheet1.RowCount; j++)
                    {
                        if (ssView3_Sheet1.Cells[j, 0].Text.Trim() == dt.Rows[i]["HUSONGNAME"].ToString().Trim())
                        {
                            break;
                        }
                    }

                    ssView3_Sheet1.Cells[j, Convert.ToInt32(VB.Val(dt.Rows[i]["TDATE"].ToString().Trim()))].Text = dt.Rows[i]["TSUM"].ToString().Trim();
                    ssView3_Sheet1.Cells[j, Convert.ToInt32(VB.Val(dt.Rows[i]["TDATE"].ToString().Trim()))].Text = (ssView3_Sheet1.Cells[j, Convert.ToInt32(VB.Val(dt.Rows[i]["TDATE"].ToString().Trim()))].Text == "0" ? "" : ssView3_Sheet1.Cells[j, Convert.ToInt32(VB.Val(dt.Rows[i]["TDATE"].ToString().Trim()))].Text);
                }

                dt.Dispose();
                dt = null;

                ssView3_Sheet1.RowCount = ssView3_Sheet1.RowCount + 1;

                ssView3_Sheet1.Cells[ssView3_Sheet1.RowCount - 1, 0].Text = "합계";

                for (i = 1; i < 13; i++)
                {
                    intTSUM = 0;

                    for (j = 0; j < ssView3_Sheet1.RowCount; j++)
                    {
                        intTSUM += Convert.ToInt32(VB.Val(ssView3_Sheet1.Cells[j, i].Text.Trim()));
                    }

                    ssView3_Sheet1.Cells[ssView3_Sheet1.RowCount - 1, i].Text = intTSUM.ToString();
                }


                //합계
                for (j = 0; j < ssView3_Sheet1.RowCount; j++)
                {
                    intTSUM = 0;

                    for (i = 1; i < 13; i++)
                    {
                        intTSUM += Convert.ToInt32(VB.Val(ssView3_Sheet1.Cells[j, i].Text.Trim()));
                    }

                    ssView3_Sheet1.Cells[j, 13].Text = intTSUM.ToString();
                    ssView3_Sheet1.Cells[j, 14].Text = VB.Format(intTSUM / 12, "0.0");
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        void SumDept(string strSDATE, string strEDATE)
        {

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            int i = 0;
            int j = 0;
            int intTSUM = 0;

            try
            {
                //과별 합계-입원
                ssView4_Sheet1.RowCount = 0;

                SQL = "";
                SQL = " SELECT DEPTCODE FROM " + ComNum.DB_PMPA + "NUR_QI_TRANSFOR";
                SQL = SQL + ComNum.VBLF + "WHERE TDATE >= TO_DATE('" + strSDATE + "','YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "     AND TDATE <= TO_DATE('" + strEDATE + "','YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "  GROUP BY DEPTCODE ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView4_Sheet1.RowCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView4_Sheet1.Cells[i, 0].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "     SELECT TO_CHAR(TDATE, 'MM') TDATE, DEPTCODE, SUM(1) TSUM, 'I' GUBUN";
                SQL = SQL + ComNum.VBLF + "From " + ComNum.DB_PMPA + "NUR_QI_TRANSFOR";
                SQL = SQL + ComNum.VBLF + "WHERE TDATE >= TO_DATE('" + strSDATE + "','YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "     AND TDATE <= TO_DATE('" + strEDATE + "','YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "     GROUP BY TO_CHAR(TDATE, 'MM'), DEPTCODE, 'I'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    for (j = 0; j < ssView4_Sheet1.RowCount; j++)
                    {
                        if (ssView4_Sheet1.Cells[j, 0].Text.Trim() == dt.Rows[i]["DEPTCODE"].ToString().Trim())
                        {
                            break;
                        }
                    }

                    ssView4_Sheet1.Cells[j, Convert.ToInt32(VB.Val(dt.Rows[i]["TDATE"].ToString().Trim()))].Text = dt.Rows[i]["TSUM"].ToString().Trim();
                    ssView4_Sheet1.Cells[j, Convert.ToInt32(VB.Val(dt.Rows[i]["TDATE"].ToString().Trim()))].Text = (ssView4_Sheet1.Cells[j, Convert.ToInt32(VB.Val(dt.Rows[i]["TDATE"].ToString().Trim()))].Text == ")" ? "" : ssView4_Sheet1.Cells[j, Convert.ToInt32(VB.Val(dt.Rows[i]["TDATE"].ToString().Trim()))].Text);
                }

                dt.Dispose();
                dt = null;

                ssView4_Sheet1.RowCount = ssView4_Sheet1.RowCount + 1;

                ssView4_Sheet1.Cells[ssView4_Sheet1.RowCount - 1, 0].Text = "합계";

                for (i = 1; i < 13; i++)
                {
                    intTSUM = 0;

                    for (j = 0; j < ssView4_Sheet1.RowCount; j++)
                    {
                        intTSUM += Convert.ToInt32(VB.Val(ssView4_Sheet1.Cells[j, i].Text.Trim()));
                    }

                    ssView4_Sheet1.Cells[ssView4_Sheet1.RowCount - 1, i].Text = intTSUM.ToString();
                }

                //합계
                for (j = 0; j < ssView4_Sheet1.RowCount; j++)
                {
                    intTSUM = 0;
                    for (i = 1; i < 13; i++)
                    {
                        intTSUM += Convert.ToInt32(VB.Val(ssView4_Sheet1.Cells[j, i].Text.Trim()));
                    }

                    ssView4_Sheet1.Cells[j, 13].Text = intTSUM.ToString();
                    ssView4_Sheet1.Cells[j, 14].Text = VB.Format(intTSUM / 12, "0.0");
                }

                //과별 합계-퇴원
                ssView5_Sheet1.RowCount = 0;

                SQL = "";
                SQL = "     SELECT DEPTCODE From NUR_ER_PATIENT";
                SQL = SQL + ComNum.VBLF + "WHERE OUTTIME >= TO_DATE('" + strSDATE + "','YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "     AND OUTTIME <= TO_DATE('" + strEDATE + "','YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "     AND OUTGBN = '6'";
                SQL = SQL + ComNum.VBLF + "     GROUP BY DEPTCODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView5_Sheet1.RowCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView5_Sheet1.Cells[i, 0].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "     SELECT TO_CHAR(OUTTIME, 'MM') TDATE, DEPTCODE, SUM(1) TSUM, 'E' GUBUN";
                SQL = SQL + ComNum.VBLF + "From NUR_ER_PATIENT";
                SQL = SQL + ComNum.VBLF + "WHERE OUTTIME >= TO_DATE('" + strSDATE + "','YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "     AND OUTTIME <= TO_DATE('" + strEDATE + "','YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "     AND OUTGBN = '6'";
                SQL = SQL + ComNum.VBLF + "     GROUP BY TO_CHAR(OUTTIME, 'MM'), DEPTCODE, 'E'";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    for (j = 0; j < ssView5_Sheet1.RowCount; j++)
                    {
                        if (ssView5_Sheet1.Cells[j, 0].Text.Trim() == dt.Rows[i]["DEPTCODE"].ToString().Trim())
                        {
                            break;
                        }
                    }

                    ssView5_Sheet1.Cells[j, Convert.ToInt32(VB.Val(dt.Rows[i]["TDATE"].ToString().Trim()))].Text = dt.Rows[i]["TSUM"].ToString().Trim();
                    ssView5_Sheet1.Cells[j, Convert.ToInt32(VB.Val(dt.Rows[i]["TDATE"].ToString().Trim()))].Text = (ssView5_Sheet1.Cells[j, Convert.ToInt32(VB.Val(dt.Rows[i]["TDATE"].ToString().Trim()))].Text == "0" ? "" : ssView5_Sheet1.Cells[j, Convert.ToInt32(VB.Val(dt.Rows[i]["TDATE"].ToString().Trim()))].Text);
                }

                dt.Dispose();
                dt = null;

                ssView5_Sheet1.RowCount = ssView5_Sheet1.RowCount + 1;

                ssView5_Sheet1.Cells[ssView5_Sheet1.RowCount - 1, 0].Text = "합계";

                for (i = 1; i < 13; i++)
                {
                    intTSUM = 0;

                    for (j = 0; j < ssView5_Sheet1.RowCount; j++)
                    {
                        intTSUM += Convert.ToInt32(VB.Val(ssView5_Sheet1.Cells[j, i].Text.Trim()));
                    }

                    ssView5_Sheet1.Cells[ssView5_Sheet1.RowCount - 1, i].Text = intTSUM.ToString();
                }

                //합계
                for (j = 0; j < ssView5_Sheet1.RowCount; j++)
                {
                    intTSUM = 0;

                    for (i = 1; i < 13; i++)
                    {
                        intTSUM += Convert.ToInt32(VB.Val(ssView5_Sheet1.Cells[j, i].Text.Trim()));
                    }

                    ssView5_Sheet1.Cells[j, 13].Text = intTSUM.ToString();
                    ssView5_Sheet1.Cells[j, 14].Text = VB.Format(intTSUM / 12, "0.0");
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }
    }
}