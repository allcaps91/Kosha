using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComLibB;
using Oracle.DataAccess.Client;

namespace ComSupLibB
{
    /// <summary>
    /// Class Name      : ComSupLibB
    /// File Name       : frmNutritionSearchInput.cs
    /// Description     : 영양불량환자 관리
    /// Author          : 박창욱
    /// Create Date     : 2018-05-01
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\diet\dietorder\Frm영양검색Input.frm(Frm영양검색Input.frm) >> frmNutritionSearchInput.cs 폼이름 재정의" />
    public partial class frmNutritionSearchInput : Form
    {
        public frmNutritionSearchInput()
        {
            InitializeComponent();
        }

        public frmNutritionSearchInput(string strPano, string strInDate, string strSex, string strNew)
        {
            InitializeComponent();

            clsSupDiet.dst.Pano = strPano;
            clsSupDiet.dst.IpwonDay = strInDate;
            clsSupDiet.dst.New = strNew;
            clsSupDiet.dst.Sex = strSex;
        }


        private void btnBody_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strTemp = "";

            if (clsSupDiet.dst.New == "V")
            {
                return;
            }

            strTemp = ssView_Sheet1.Cells[33, 11].Text.Trim();

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " UPDATE " + ComNum.DB_PMPA + "DIET_S_SEARCH SET ";
                SQL = SQL + ComNum.VBLF + " HEIGHT = " + txtHeight.Text + ",";
                SQL = SQL + ComNum.VBLF + " WEIGHT = " + txtWeight.Text + ",";
                SQL = SQL + ComNum.VBLF + " HWEIGHT = " + ((VB.Val(txtHeight.Text) * 0.01) * (VB.Val(txtHeight.Text) * 0.01) * VB.Val((clsSupDiet.dst.Sex == "M" ? "22" : "21"))).ToString("##0.0");
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + clsSupDiet.dst.Pano + "'";
                SQL = SQL + ComNum.VBLF + "   AND INDATE = TO_DATE('" + clsSupDiet.dst.IpwonDay + "','YYYY-MM-DD HH24:MI:SS')";

                if (strTemp != "")
                {
                    SQL = SQL + ComNum.VBLF + "   AND UDATE = TO_DATE('" + strTemp + "','YYYY-MM-DD') ";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    panBody.Visible = false;
                    return;
                }


                clsDB.setCommitTran(clsDB.DbCon);

                Read_S_Search(clsSupDiet.dst.Pano, clsSupDiet.dst.IpwonDay, clsSupDiet.dst.UDATE);
                Read_S_Manager(clsSupDiet.dst.Pano, clsSupDiet.dst.IpwonDay, clsSupDiet.dst.UDATE);

                panBody.Visible = false;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void frmNutritionSearchInput_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Sheet_Clear();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //보기만 가능할 경우(처방등록 화면 및 병동환자 관리)
                if (clsSupDiet.dst.New == "V")
                {
                    SQL = "";
                    SQL = "SELECT IPDNO, PANO, to_char(INDATE, 'yyyy-mm-dd hh24:mi:ss') INDATE";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + clsSupDiet.dst.Pano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND TRUNC(INDATE) = TO_DATE('" + clsSupDiet.dst.IpwonDay + "','YYYY-MM-DD') ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("에러, 전산실 문의바람(351)");
                        return;
                    }

                    //clsSupDiet.dst.IpwonDay = ComFunc.FormatStrToDate(dt.Rows[0]["INDATE"].ToString().Trim(), "A");
                    clsSupDiet.dst.IpwonDay = dt.Rows[0]["INDATE"].ToString().Trim();

                    dt.Dispose();
                    dt = null;

                    btnRegist.Enabled = false;
                    btnDelete.Enabled = false;
                    btnPrint.Enabled = false;
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (clsSupDiet.dst.Pano == "" || clsSupDiet.dst.IpwonDay == "")
            {
                ComFunc.MsgBox("등록번호 또는 입원날짜가 없습니다.");
                this.Close();
                return;
            }
            else
            {
                if (clsSupDiet.dst.New == "Y")
                {
                    Read_S_Search_New();
                }
                else
                {
                    //구입력자일 경우
                    Read_S_Search(clsSupDiet.dst.Pano, clsSupDiet.dst.IpwonDay, clsSupDiet.dst.UDATE);
                    Read_S_Manager(clsSupDiet.dst.Pano, clsSupDiet.dst.IpwonDay, clsSupDiet.dst.UDATE);
                    Read_S_Manager_New(clsSupDiet.dst.Pano, clsSupDiet.dst.IpwonDay, clsSupDiet.dst.UDATE);
                }
            }
        }

        void Read_S_Search(string argPano, string argInDate, string argUDATE = "")
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strTemp = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT SDATE, to_char(INDATE, 'YYYY-MM-DD HH24:MI:SS') INDATE, PANO, SNAME, SEX, AGE, DIAGNOSIS, DIETFOOD, DEPTCODE, DRCODE ";
                SQL = SQL + ComNum.VBLF + " WARDCODE, ROOMCODE, HEIGHT, WEIGHT, HWEIGHT, IBW, WARNING, ALB, TLC, HB, TCHO, SCORE, ";
                SQL = SQL + ComNum.VBLF + " COMMENTS1, COMMENTS2, SABUN ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_S_SEARCH ";
                SQL = SQL + ComNum.VBLF + " WHERE INDATE = TO_DATE('" + argInDate + "', 'YYYY-MM-DD HH24:MI:SS')";
                SQL = SQL + ComNum.VBLF + "   AND PANO = '" + argPano + "'";
                if (argUDATE != "" && argUDATE != null)
                {
                    SQL = SQL + ComNum.VBLF + "   AND UDATE = TO_DATE('" + argUDATE + "','YYYY-MM-DD')";
                }
                SQL = SQL + ComNum.VBLF + " ORDER BY UDATE ASC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssView_Sheet1.Cells[1, 12].Text = argPano;
                ssView_Sheet1.Cells[2, 12].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                ssView_Sheet1.Cells[3, 12].Text = (dt.Rows[0]["SEX"].ToString().Trim() == "M" ? "남" : "여") + " / " + dt.Rows[0]["AGE"].ToString().Trim();
                ssView_Sheet1.Cells[4, 12].Text = dt.Rows[0]["DIAGNOSIS"].ToString().Trim();
                ssView_Sheet1.Cells[5, 12].Text = dt.Rows[0]["DIETFOOD"].ToString().Trim();
                ssView_Sheet1.Cells[6, 12].Text = dt.Rows[0]["DEPTCODE"].ToString().Trim() + " / " + dt.Rows[0]["ROOMCODE"].ToString().Trim();

                strTemp = dt.Rows[0]["WARNING"].ToString().Trim();

                ssView_Sheet1.Cells[9, 11].Text = strTemp == "H" ? "고위험군" : (strTemp == "M" ? "중위험군" : "저위험군");

                //ssView_Sheet1.Cells[11, 8].Text = ComFunc.FormatStrToDate(dt.Rows[0]["INDATE"].ToString().Trim(), "A");
                ssView_Sheet1.Cells[11, 8].Text = dt.Rows[0]["INDATE"].ToString().Trim();
                ssView_Sheet1.Cells[12, 8].Text = "▣ Ht : " + dt.Rows[0]["HEIGHT"].ToString().Trim() + " cm   ▣ Wt : " + dt.Rows[0]["WEIGHT"].ToString().Trim() + " kg   ▣ 표준체중 : " + dt.Rows[0]["HWEIGHT"].ToString().Trim() + " kg   ▣ %IBW : " + dt.Rows[0]["IBW"].ToString().Trim();

                ssView_Sheet1.Cells[17, 9].Text = dt.Rows[0]["IBW"].ToString().Trim();
                ssView_LabColor("IBW", VB.Val(ssView_Sheet1.Cells[17, 9].Text));

                ssView_Sheet1.Cells[19, 9].Text = dt.Rows[0]["ALB"].ToString().Trim();
                ssView_LabColor("ALB", VB.Val(ssView_Sheet1.Cells[19, 9].Text));

                ssView_Sheet1.Cells[20, 9].Text = dt.Rows[0]["TLC"].ToString().Trim();
                ssView_LabColor("TLC", VB.Val(ssView_Sheet1.Cells[20, 9].Text));

                ssView_Sheet1.Cells[21, 9].Text = dt.Rows[0]["HB"].ToString().Trim();
                ssView_LabColor("HB", VB.Val(ssView_Sheet1.Cells[21, 9].Text), dt.Rows[0]["SEX"].ToString().Trim());

                ssView_Sheet1.Cells[23, 9].Text = dt.Rows[0]["TCHO"].ToString().Trim();
                ssView_LabColor("TCHO", VB.Val(ssView_Sheet1.Cells[23, 9].Text));

                ssView_Sheet1.Cells[26, 10].Text = strTemp == "H" ? "고위험군" : (strTemp == "M" ? "중위험군" : "저위험군");

                ssView_Sheet1.Cells[29, 8].Text = dt.Rows[0]["COMMENTS1"].ToString().Trim();
                ssView_Sheet1.Cells[30, 8].Text = dt.Rows[0]["COMMENTS2"].ToString().Trim();

                ssView_Sheet1.Cells[33, 11].Text = dt.Rows[0]["SDATE"].ToString().Trim();
                ssView_Sheet1.Cells[33, 13].Text = dt.Rows[0]["SABUN"].ToString().Trim();



                ssView2_Sheet1.Cells[12, 7].Text = "▣ Ht : " + dt.Rows[0]["HEIGHT"].ToString().Trim() + " cm   ▣ Wt : " + dt.Rows[0]["WEIGHT"].ToString().Trim() + " kg   ▣ 표준체중 : " + dt.Rows[0]["HWEIGHT"].ToString().Trim() + " kg   ▣ %IBW : " + dt.Rows[0]["IBW"].ToString().Trim();

                ssView2_Sheet1.Cells[1, 11].Text = argPano;
                ssView2_Sheet1.Cells[2, 11].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                ssView2_Sheet1.Cells[3, 11].Text = (dt.Rows[0]["SEX"].ToString().Trim() == "M" ? "남" : "여") + " / " + dt.Rows[0]["AGE"].ToString().Trim();
                ssView2_Sheet1.Cells[4, 11].Text = dt.Rows[0]["DIAGNOSIS"].ToString().Trim();
                ssView2_Sheet1.Cells[5, 11].Text = dt.Rows[0]["DIETFOOD"].ToString().Trim();
                ssView2_Sheet1.Cells[6, 11].Text = dt.Rows[0]["DEPTCODE"].ToString().Trim() + " / " + dt.Rows[0]["ROOMCODE"].ToString().Trim();

                ssView2_Sheet1.Cells[17, 8].Text = dt.Rows[0]["IBW"].ToString().Trim();
                ssView2_LabColor("IBW", VB.Val(ssView_Sheet1.Cells[17, 8].Text));

                ssView2_Sheet1.Cells[22, 8].Text = dt.Rows[0]["ALB"].ToString().Trim();
                ssView2_LabColor("ALB", VB.Val(ssView_Sheet1.Cells[22, 8].Text));

                ssView2_Sheet1.Cells[23, 8].Text = dt.Rows[0]["TLC"].ToString().Trim();
                ssView2_LabColor("TLC", VB.Val(ssView_Sheet1.Cells[23, 8].Text));




                ssView3_Sheet1.Cells[12, 7].Text = "▣ Ht : " + dt.Rows[0]["HEIGHT"].ToString().Trim() + " cm   ▣ Wt : " + dt.Rows[0]["WEIGHT"].ToString().Trim() + " kg   ▣ 표준체중 : " + dt.Rows[0]["HWEIGHT"].ToString().Trim() + " kg   ▣ %IBW : " + dt.Rows[0]["IBW"].ToString().Trim();

                ssView3_Sheet1.Cells[1, 11].Text = argPano;
                ssView3_Sheet1.Cells[2, 11].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                ssView3_Sheet1.Cells[3, 11].Text = (dt.Rows[0]["SEX"].ToString().Trim() == "M" ? "남" : "여") + " / " + dt.Rows[0]["AGE"].ToString().Trim();
                ssView3_Sheet1.Cells[4, 11].Text = dt.Rows[0]["DIAGNOSIS"].ToString().Trim();
                ssView3_Sheet1.Cells[5, 11].Text = dt.Rows[0]["DIETFOOD"].ToString().Trim();
                ssView3_Sheet1.Cells[6, 11].Text = dt.Rows[0]["DEPTCODE"].ToString().Trim() + " / " + dt.Rows[0]["ROOMCODE"].ToString().Trim();

                ssView3_Sheet1.Cells[17, 8].Text = dt.Rows[0]["IBW"].ToString().Trim();
                ssView3_LabColor("IBW", VB.Val(ssView_Sheet1.Cells[17, 8].Text));

                ssView3_Sheet1.Cells[22, 8].Text = dt.Rows[0]["ALB"].ToString().Trim();
                ssView3_LabColor("ALB", VB.Val(ssView_Sheet1.Cells[22, 8].Text));

                ssView3_Sheet1.Cells[23, 8].Text = dt.Rows[0]["TLC"].ToString().Trim();
                ssView3_LabColor("TLC", VB.Val(ssView_Sheet1.Cells[23, 8].Text));

                ssView3_Sheet1.Cells[24, 8].Text = dt.Rows[0]["HB"].ToString().Trim();
                ssView3_LabColor("HB", VB.Val(ssView_Sheet1.Cells[24, 8].Text), dt.Rows[0]["SEX"].ToString().Trim());

                ssView3_Sheet1.Cells[26, 8].Text = dt.Rows[0]["TCHO"].ToString().Trim();
                ssView3_LabColor("TCHO", VB.Val(ssView_Sheet1.Cells[26, 8].Text));

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        void Read_S_Manager(string argPano, string argInDate, string argDate = "")
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT to_char(UDATE, 'yyyy-mm-dd') UDATE, SDATE, PANO, to_char(INDATE, 'yyyy-mm-dd hh24:mi:ss') INDATE, SABUN, STATUS, CWEIGHT1, CWEIGHT2, CWEIGHT3, ";
                SQL = SQL + ComNum.VBLF + " FAILA, FAILB, FAILC, FAILD, DIETA, DIETB, DIETC, DIETD, DIETE, DIETF, DIETG, ";
                SQL = SQL + ComNum.VBLF + " PLANA, PLANB1, PLANB2, PLANC, PLAND, PLANE, PLANF1, PLANF2, USE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_S_MANAGER ";
                SQL = SQL + ComNum.VBLF + " WHERE INDATE = TO_DATE('" + argInDate + "', 'YYYY-MM-DD HH24:MI:SS')";
                SQL = SQL + ComNum.VBLF + "   AND PANO = '" + argPano + "'";
                if (argDate != "" && argDate != null)
                {
                    SQL = SQL + ComNum.VBLF + "   AND UDATE = TO_DATE('" + argDate + "', 'YYYY-MM-DD HH24:MI:SS')";
                }
                SQL = SQL + ComNum.VBLF + " ORDER BY UDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssView2_Sheet1.Cells[11, 7].Text = clsSupDiet.DietSearch_Code(dt.Rows[0]["STATUS"].ToString().Trim(), "1");

                ssView2_Sheet1.Cells[19, Convert.ToInt32(VB.Val(dt.Rows[0]["cWeight1"].ToString().Trim())) + 9].Value = true;
                ssView2_Sheet1.Cells[20, Convert.ToInt32(VB.Val(dt.Rows[0]["cWeight2"].ToString().Trim())) + 9].Value = true;
                ssView2_Sheet1.Cells[21, Convert.ToInt32(VB.Val(dt.Rows[0]["cWeight3"].ToString().Trim())) + 9].Value = true;

                ssView2_Sheet1.Cells[24, Convert.ToInt32(VB.Val(dt.Rows[0]["FailA"].ToString().Trim())) + 9].BackColor = Color.FromArgb(219, 219, 219);
                ssView2_Sheet1.Cells[25, Convert.ToInt32(VB.Val(dt.Rows[0]["FailB"].ToString().Trim())) + 9].BackColor = Color.FromArgb(219, 219, 219);
                ssView2_Sheet1.Cells[26, Convert.ToInt32(VB.Val(dt.Rows[0]["FailC"].ToString().Trim())) + 9].BackColor = Color.FromArgb(219, 219, 219);
                ssView2_Sheet1.Cells[27, Convert.ToInt32(VB.Val(dt.Rows[0]["FailD"].ToString().Trim())) + 9].BackColor = Color.FromArgb(219, 219, 219);

                ssView2_Sheet1.Cells[30, 9].Text = dt.Rows[0]["DietA"].ToString().Trim();
                ssView2_Sheet1.Cells[30, 11].Text = dt.Rows[0]["DietB"].ToString().Trim();

                ssView2_Sheet1.Cells[31, 9].Text = dt.Rows[0]["DietF"].ToString().Trim();
                ssView2_Sheet1.Cells[31, 11].Text = dt.Rows[0]["DietG"].ToString().Trim();

                ssView2_Sheet1.Cells[33, 7].Value = dt.Rows[0]["PlanA"].ToString().Trim() == "1" ? true : false;

                ssView2_Sheet1.Cells[34, 9].Text = dt.Rows[0]["PlanB1"].ToString().Trim();
                ssView2_Sheet1.Cells[35, 9].Text = dt.Rows[0]["PlanB2"].ToString().Trim();

                ssView2_Sheet1.Cells[36, 9].Value = dt.Rows[0]["PlanC"].ToString().Trim() == "1" ? true : false;
                ssView2_Sheet1.Cells[36, 10].Value = dt.Rows[0]["PlanD"].ToString().Trim() == "1" ? true : false;
                ssView2_Sheet1.Cells[36, 11].Value = dt.Rows[0]["PlanE"].ToString().Trim() == "1" ? true : false;

                ssView2_Sheet1.Cells[37, 9].Text = dt.Rows[0]["PlanF1"].ToString().Trim();
                ssView2_Sheet1.Cells[38, 9].Text = dt.Rows[0]["PlanF2"].ToString().Trim();

                //ssView2_Sheet1.Cells[41, 10].Text = ComFunc.FormatStrToDate(dt.Rows[0]["UDATE"].ToString().Trim(), "D");
                ssView2_Sheet1.Cells[41, 10].Text = dt.Rows[0]["UDATE"].ToString().Trim();
                ssView2_Sheet1.Cells[41, 12].Text = dt.Rows[0]["SABUN"].ToString().Trim();

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        void Read_S_Manager_New(string argPano, string argInDate, string argDate = "")
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT to_char(UDATE, 'yyyy-MM-dd') UDATE, SDATE, PANO, INDATE, SABUN, STATUS, CWEIGHT1, CWEIGHT2, CWEIGHT3, ";
                SQL = SQL + ComNum.VBLF + " FAILA1, FAILA2, FAILA3, FAILB1, FAILB2, FAILB3, FAILB4, FAILB5, FAILB6, FAILC1, ";
                SQL = SQL + ComNum.VBLF + " FAILD1, FAILD2, DIETA, DIETB, DIETC, DIETD, DIETE, DIETF, DIETG, ";
                SQL = SQL + ComNum.VBLF + " PLANA, PLANB1, PLANB2, PLANC, PLAND, PLANE, PLANF, USE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_S_MANAGER_NEW ";
                SQL = SQL + ComNum.VBLF + " WHERE INDATE = TO_DATE('" + argInDate + "', 'YYYY-MM-DD HH24:MI:SS')";
                SQL = SQL + ComNum.VBLF + "   AND PANO = '" + argPano + "'";
                if (argDate != "" && argDate != null)
                {
                    SQL = SQL + ComNum.VBLF + "   AND UDATE = TO_DATE('" + argDate + "', 'YYYY-MM-DD HH24:MI:SS')";
                }
                SQL = SQL + ComNum.VBLF + " ORDER BY UDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssView3_Sheet1.Cells[11, 7].Text = clsSupDiet.DietSearch_Code(dt.Rows[0]["STATUS"].ToString().Trim(), "1");

                ssView3_Sheet1.Cells[19, Convert.ToInt32(VB.Val(dt.Rows[0]["cWeight1"].ToString().Trim())) + 9].Value = true;
                ssView3_Sheet1.Cells[20, Convert.ToInt32(VB.Val(dt.Rows[0]["cWeight2"].ToString().Trim())) + 9].Value = true;
                ssView3_Sheet1.Cells[21, Convert.ToInt32(VB.Val(dt.Rows[0]["cWeight3"].ToString().Trim())) + 9].Value = true;

                ssView3_Sheet1.Cells[27, Convert.ToInt32(VB.Val(dt.Rows[0]["FailA1"].ToString().Trim())) + 9].BackColor = Color.FromArgb(219, 219, 219);
                ssView3_Sheet1.Cells[28, Convert.ToInt32(VB.Val(dt.Rows[0]["FailA2"].ToString().Trim())) + 9].BackColor = Color.FromArgb(219, 219, 219);
                ssView3_Sheet1.Cells[29, Convert.ToInt32(VB.Val(dt.Rows[0]["FailA3"].ToString().Trim())) + 9].BackColor = Color.FromArgb(219, 219, 219);

                ssView3_Sheet1.Cells[30, Convert.ToInt32(VB.Val(dt.Rows[0]["FailB1"].ToString().Trim())) + 9].BackColor = Color.FromArgb(219, 219, 219);
                ssView3_Sheet1.Cells[31, Convert.ToInt32(VB.Val(dt.Rows[0]["FailB2"].ToString().Trim())) + 9].BackColor = Color.FromArgb(219, 219, 219);
                ssView3_Sheet1.Cells[32, Convert.ToInt32(VB.Val(dt.Rows[0]["FailB3"].ToString().Trim())) + 9].BackColor = Color.FromArgb(219, 219, 219);
                ssView3_Sheet1.Cells[33, Convert.ToInt32(VB.Val(dt.Rows[0]["FailB4"].ToString().Trim())) + 9].BackColor = Color.FromArgb(219, 219, 219);
                ssView3_Sheet1.Cells[34, Convert.ToInt32(VB.Val(dt.Rows[0]["FailB5"].ToString().Trim())) + 9].BackColor = Color.FromArgb(219, 219, 219);
                ssView3_Sheet1.Cells[35, Convert.ToInt32(VB.Val(dt.Rows[0]["FailB6"].ToString().Trim())) + 9].BackColor = Color.FromArgb(219, 219, 219);

                ssView3_Sheet1.Cells[36, Convert.ToInt32(VB.Val(dt.Rows[0]["FailC1"].ToString().Trim())) + 9].BackColor = Color.FromArgb(219, 219, 219);

                ssView3_Sheet1.Cells[37, Convert.ToInt32(VB.Val(dt.Rows[0]["FailD1"].ToString().Trim())) + 9].BackColor = Color.FromArgb(219, 219, 219);
                ssView3_Sheet1.Cells[38, Convert.ToInt32(VB.Val(dt.Rows[0]["FailD2"].ToString().Trim())) + 9].BackColor = Color.FromArgb(219, 219, 219);

                ssView3_Sheet1.Cells[41, 9].Text = dt.Rows[0]["DietA"].ToString().Trim();
                ssView3_Sheet1.Cells[41, 11].Text = dt.Rows[0]["DietB"].ToString().Trim();

                ssView3_Sheet1.Cells[42, 9].Text = dt.Rows[0]["DietF"].ToString().Trim();
                ssView3_Sheet1.Cells[42, 11].Text = dt.Rows[0]["DietG"].ToString().Trim();

                ssView3_Sheet1.Cells[44, 7].Value = dt.Rows[0]["PlanA"].ToString().Trim() == "1" ? true : false;

                ssView3_Sheet1.Cells[45, 9].Text = dt.Rows[0]["PlanB1"].ToString().Trim();
                ssView3_Sheet1.Cells[46, 9].Text = dt.Rows[0]["PlanB2"].ToString().Trim();

                ssView3_Sheet1.Cells[47, 9].Value = dt.Rows[0]["PlanC"].ToString().Trim() == "1" ? true : false;
                ssView3_Sheet1.Cells[47, 10].Value = dt.Rows[0]["PlanD"].ToString().Trim() == "1" ? true : false;
                ssView3_Sheet1.Cells[47, 11].Value = dt.Rows[0]["PlanE"].ToString().Trim() == "1" ? true : false;

                ssView3_Sheet1.Cells[48, 4].Text = dt.Rows[0]["PlanF"].ToString().Trim();

                //ssView3_Sheet1.Cells[52, 10].Text = ComFunc.FormatStrToDate(dt.Rows[0]["UDATE"].ToString().Trim(), "D");
                ssView3_Sheet1.Cells[52, 10].Text = dt.Rows[0]["UDATE"].ToString().Trim();
                ssView3_Sheet1.Cells[52, 12].Text = dt.Rows[0]["SABUN"].ToString().Trim();

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        void Read_S_Search_New()
        {
            //신규입력자를 위한

            string strTemp = "";

            ssView_Sheet1.Cells[1, 12].Text = clsSupDiet.dst.Pano;
            ssView_Sheet1.Cells[2, 12].Text = clsSupDiet.dst.sName;
            ssView_Sheet1.Cells[3, 12].Text = (clsSupDiet.dst.Sex == "M" ? "남" : "여") + " / " + clsSupDiet.dst.Age;
            ssView_Sheet1.Cells[4, 12].Text = clsSupDiet.dst.DIAGNOSIS;
            ssView_Sheet1.Cells[5, 12].Text = clsSupDiet.dst.DietName;
            ssView_Sheet1.Cells[6, 12].Text = clsSupDiet.dst.DeptCode + " / " + clsSupDiet.dst.RoomCode;

            strTemp = clsSupDiet.dst.Warning;
            ssView_Sheet1.Cells[9, 11].Text = strTemp == "H" ? "고위험군" : (strTemp == "M" ? "중위험군" : "저위험군");

            //ssView_Sheet1.Cells[11, 8].Text = ComFunc.FormatStrToDate(clsSupDiet.dst.IpwonDay, "A");
            ssView_Sheet1.Cells[11, 8].Text = clsSupDiet.dst.IpwonDay;
            ssView_Sheet1.Cells[12, 8].Text = "▣ Ht : " + clsSupDiet.dst.Height + " cm   ▣ Wt : " + clsSupDiet.dst.Weight + " kg   ▣ 표준체중 : " + clsSupDiet.dst.HWeight + " kg   ▣ %IBW : " + clsSupDiet.dst.IBW;

            ssView_Sheet1.Cells[17, 9].Text = clsSupDiet.dst.IBW;
            ssView_LabColor("IBW", VB.Val(ssView_Sheet1.Cells[17, 9].Text));

            ssView_Sheet1.Cells[19, 9].Text = clsSupDiet.dst.LabALB;
            ssView_LabColor("ALB", VB.Val(ssView_Sheet1.Cells[19, 9].Text));

            ssView_Sheet1.Cells[20, 9].Text = clsSupDiet.dst.LabTLC;
            ssView_LabColor("TLC", VB.Val(ssView_Sheet1.Cells[20, 9].Text));

            ssView_Sheet1.Cells[21, 9].Text = clsSupDiet.dst.LabHB;
            ssView_LabColor("HB", VB.Val(ssView_Sheet1.Cells[21, 9].Text), clsSupDiet.dst.Sex);

            ssView_Sheet1.Cells[23, 9].Text = clsSupDiet.dst.LabTcho;
            ssView_LabColor("TCHO", VB.Val(ssView_Sheet1.Cells[23, 9].Text));

            ssView_Sheet1.Cells[26, 10].Text = strTemp == "H" ? "고위험군" : (strTemp == "M" ? "중위험군" : "저위험군");

            ssView_Sheet1.Cells[33, 11].Text = clsSupDiet.dst.sDate;
            ssView_Sheet1.Cells[33, 13].Text = "";


            ssView3_Sheet1.Cells[12, 7].Text = "▣ Ht : " + clsSupDiet.dst.Height + " cm   ▣ Wt : " + clsSupDiet.dst.Weight + " kg   ▣ 표준체중 : " + clsSupDiet.dst.HWeight + " kg   ▣ %IBW : " + clsSupDiet.dst.IBW;

            ssView3_Sheet1.Cells[1, 11].Text = clsSupDiet.dst.Pano;
            ssView3_Sheet1.Cells[2, 11].Text = clsSupDiet.dst.sName;
            ssView3_Sheet1.Cells[3, 11].Text = (clsSupDiet.dst.Sex == "M" ? "남" : "여") + " / " + clsSupDiet.dst.Age;
            ssView3_Sheet1.Cells[4, 11].Text = clsSupDiet.dst.DIAGNOSIS;
            ssView3_Sheet1.Cells[5, 11].Text = clsSupDiet.dst.DietName;
            ssView3_Sheet1.Cells[6, 11].Text = clsSupDiet.dst.DeptCode + " / " + clsSupDiet.dst.RoomCode;

            ssView3_Sheet1.Cells[17, 8].Text = clsSupDiet.dst.IBW;
            ssView2_LabColor("IBW", VB.Val(ssView_Sheet1.Cells[17, 8].Text));

            ssView3_Sheet1.Cells[22, 8].Text = clsSupDiet.dst.LabALB;
            ssView2_LabColor("ALB", VB.Val(ssView_Sheet1.Cells[22, 8].Text));

            ssView3_Sheet1.Cells[23, 8].Text = clsSupDiet.dst.LabTLC;
            ssView2_LabColor("TLC", VB.Val(ssView_Sheet1.Cells[23, 8].Text));

            ssView3_Sheet1.Cells[24, 8].Text = clsSupDiet.dst.LabTcho;
            ssView2_LabColor("TCHO", VB.Val(ssView_Sheet1.Cells[24, 8].Text));
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            clsSupDiet.dst.Pano = "";
            clsSupDiet.dst.sDate = "";
            clsSupDiet.dst.IpwonDay = "";
            clsSupDiet.dst.New = "";
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strPANO = "";
            string strSDate = "";
            string strInDate = "";

            strPANO = ssView_Sheet1.Cells[1, 12].Text.Trim();
            strInDate = ssView_Sheet1.Cells[11, 8].Text.Trim();
            strSDate = ssView_Sheet1.Cells[33, 11].Text.Trim();

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " UPDATE " + ComNum.DB_PMPA + "DIET_SEARCH SET";
                SQL = SQL + " USE = 'N' ";
                SQL = SQL + " WHERE PANO = '" + strPANO + "' ";
                SQL = SQL + "   AND INDATE = TO_DATE('" + strInDate + "', 'YYYY-MM-DD HH24:MI:SS') ";
                SQL = SQL + ComNum.VBLF + "";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }


                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제하였습니다.");
                Cursor.Current = Cursors.Default;
                this.Close();
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                ssView.PrintSheet(0);
            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                ssView2.PrintSheet(0);
            }
            else if (tabControl1.SelectedTab == tabPage3)
            {
                ssView3.PrintSheet(0);
            }
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            int nIBW = 0;
            int nAlb = 0;
            int nTLC = 0;
            int nHB = 0;
            int nTCHO = 0;
            int nDietA = 0;
            int nDietB = 0;
            int nDietC = 0;
            int nDietD = 0;
            int nDietE = 0;
            int nDietF = 0;
            int nDietG = 0;
            string cPano = "";
            string cSDate = "";
            string cSabun = "";
            string cWarning = "";
            string cStatus = "";
            string cUDate = "";
            string cComment1 = "";
            string cComment2 = "";
            string cInDate = "";
            string cSSabun = "";

            string cWeight1 = "";
            string cWeight2 = "";
            string cWeight3 = "";
            string cFailA = "";
            string cFailB = "";
            string cFailC = "";
            string cFailD = "";
            string cPlanA = "";
            string cPlanB1 = "";
            string cPlanB2 = "";
            string cPlanC = "";
            string cPlanD = "";
            string cPlanE = "";
            string cPlanF = "";
            string cPlanF1 = "";
            string cPlanF2 = "";

            string cFailA1 = "";
            string cFailA2 = "";
            string cFailA3 = "";    //밥 string 죽 string 미음
            string cFailB1 = "";
            string cFailB2 = "";
            string cFailB3 = "";
            string cFailB4 = "";
            string cFailB5 = "";
            string cFailB6 = "";  //메스꺼움 string 구토 string 설사 string 변비 string 연하곤란 string 저작곤란
            string cFailC1 = "";    //피하지방손실 근육소모 - 사용안함(그래도 만들어 놓음)
            string cFailD1 = "";
            string cFailD2 = "";  //부종 string 복수

            #region Get_Data

            cWarning = VB.Left(ssView_Sheet1.Cells[9, 11].Text.Trim(), 1);

            if (cWarning == "고")
            {
                ssView_Sheet1.Cells[26, 10].Text = "고위험";
                cWarning = "H";
            }
            else if (cWarning == "중")
            {
                ssView_Sheet1.Cells[26, 10].Text = "중위험";
                cWarning = "M";
            }
            else if (cWarning == "저")
            {
                ssView_Sheet1.Cells[26, 10].Text = "저위험";
                cWarning = "L";
            }

            cPano = ssView_Sheet1.Cells[1, 12].Text.Trim();
            cInDate = ssView_Sheet1.Cells[11, 8].Text.Trim();
            cComment1 = ssView_Sheet1.Cells[29, 8].Text.Trim();
            cComment2 = ssView_Sheet1.Cells[30, 8].Text.Trim();

            cSDate = ssView_Sheet1.Cells[33, 11].Text.Trim();
            cSSabun = ssView_Sheet1.Cells[33, 13].Text.Trim();

            nIBW = Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[17, 9].Text));
            nAlb = Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[19, 9].Text));
            nTLC = Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[20, 9].Text));

            if (ssView_Sheet1.Cells[21, 9].Text.Trim() != "")
            {
                nHB = Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[21, 9].Text));
            }
            else
            {
                nHB = nIBW = Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[22, 9].Text));
            }

            nTCHO = nIBW = Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[23, 9].Text));


            cStatus = ssView3_Sheet1.Cells[11, 7].Text;

            switch (cStatus)
            {
                case "Adaquate":
                    cStatus = "0";
                    break;
                case "Marasmus-type":
                    cStatus = "1";
                    break;
                case "Kwashiorkor":
                    cStatus = "2";
                    break;
                case "Mild PCM":
                    cStatus = "3";
                    break;
                case "Moderate PCM":
                    cStatus = "4";
                    break;
                case "Severe PCM":
                    cStatus = "5";
                    break;
                case "Overweight":
                    cStatus = "6";
                    break;
                case "Obesity":
                    cStatus = "7";
                    break;
            }

            //1개월
            if (Convert.ToBoolean(ssView3_Sheet1.Cells[19, 9].Value) == true)
            {
                cWeight1 = "0";
            }
            if (Convert.ToBoolean(ssView3_Sheet1.Cells[19, 10].Value) == true)
            {
                cWeight1 = "1";
            }
            if (Convert.ToBoolean(ssView3_Sheet1.Cells[19, 11].Value) == true)
            {
                cWeight1 = "2";
            }
            if (Convert.ToBoolean(ssView3_Sheet1.Cells[19, 12].Value) == true)
            {
                cWeight1 = "3";
            }

            //2개월
            if (Convert.ToBoolean(ssView3_Sheet1.Cells[20, 9].Value) == true)
            {
                cWeight2 = "0";
            }
            if (Convert.ToBoolean(ssView3_Sheet1.Cells[20, 10].Value) == true)
            {
                cWeight2 = "1";
            }
            if (Convert.ToBoolean(ssView3_Sheet1.Cells[20, 11].Value) == true)
            {
                cWeight2 = "2";
            }
            if (Convert.ToBoolean(ssView3_Sheet1.Cells[20, 12].Value) == true)
            {
                cWeight2 = "3";
            }

            //3개월
            if (Convert.ToBoolean(ssView3_Sheet1.Cells[21, 9].Value) == true)
            {
                cWeight3 = "0";
            }
            if (Convert.ToBoolean(ssView3_Sheet1.Cells[21, 10].Value) == true)
            {
                cWeight3 = "1";
            }
            if (Convert.ToBoolean(ssView3_Sheet1.Cells[21, 11].Value) == true)
            {
                cWeight3 = "2";
            }
            if (Convert.ToBoolean(ssView3_Sheet1.Cells[21, 12].Value) == true)
            {
                cWeight3 = "3";
            }

            //식욕/식사 섭취상태 - 밥
            if (ssView3_Sheet1.Cells[27, 9].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailA1 = "0";
            }
            if (ssView3_Sheet1.Cells[27, 10].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailA1 = "1";
            }
            if (ssView3_Sheet1.Cells[27, 11].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailA1 = "2";
            }
            if (ssView3_Sheet1.Cells[27, 12].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailA1 = "3";
            }

            //식욕/식사 섭취상태 - 죽
            if (ssView3_Sheet1.Cells[28, 9].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailA2 = "0";
            }
            if (ssView3_Sheet1.Cells[28, 10].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailA2 = "1";
            }
            if (ssView3_Sheet1.Cells[28, 11].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailA2 = "2";
            }
            if (ssView3_Sheet1.Cells[28, 12].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailA2 = "3";
            }

            //식욕/식사 섭취상태 - 미음
            if (ssView3_Sheet1.Cells[29, 9].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailA3 = "0";
            }
            if (ssView3_Sheet1.Cells[29, 10].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailA3 = "1";
            }
            if (ssView3_Sheet1.Cells[29, 11].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailA3 = "2";
            }
            if (ssView3_Sheet1.Cells[29, 12].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailA3 = "3";
            }

            //식사시 문제 - 메스꺼움
            if (ssView3_Sheet1.Cells[30, 9].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailB1 = "0";
            }
            if (ssView3_Sheet1.Cells[30, 10].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailB1 = "1";
            }
            if (ssView3_Sheet1.Cells[30, 11].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailB1 = "2";
            }
            if (ssView3_Sheet1.Cells[30, 12].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailB1 = "3";
            }

            //식사시 문제 - 구토
            if (ssView3_Sheet1.Cells[31, 9].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailB2 = "0";
            }
            if (ssView3_Sheet1.Cells[31, 10].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailB2 = "1";
            }
            if (ssView3_Sheet1.Cells[31, 11].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailB2 = "2";
            }
            if (ssView3_Sheet1.Cells[31, 12].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailB2 = "3";
            }

            //식사시 문제 - 설사
            if (ssView3_Sheet1.Cells[32, 9].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailB3 = "0";
            }
            if (ssView3_Sheet1.Cells[32, 10].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailB3 = "1";
            }
            if (ssView3_Sheet1.Cells[32, 11].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailB3 = "2";
            }
            if (ssView3_Sheet1.Cells[32, 12].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailB3 = "3";
            }

            //식사시 문제 - 변비
            if (ssView3_Sheet1.Cells[33, 9].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailB4 = "0";
            }
            if (ssView3_Sheet1.Cells[33, 10].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailB4 = "1";
            }
            if (ssView3_Sheet1.Cells[33, 11].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailB4 = "2";
            }
            if (ssView3_Sheet1.Cells[33, 12].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailB4 = "3";
            }

            //식사시 문제 - 연하곤란
            if (ssView3_Sheet1.Cells[34, 9].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailB5 = "0";
            }
            if (ssView3_Sheet1.Cells[34, 10].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailB5 = "1";
            }
            if (ssView3_Sheet1.Cells[34, 11].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailB5 = "2";
            }
            if (ssView3_Sheet1.Cells[34, 12].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailB5 = "3";
            }

            //식사시 문제 - 저작곤란
            if (ssView3_Sheet1.Cells[35, 9].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailB6 = "0";
            }
            if (ssView3_Sheet1.Cells[35, 10].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailB6 = "1";
            }
            if (ssView3_Sheet1.Cells[35, 11].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailB6 = "2";
            }
            if (ssView3_Sheet1.Cells[35, 12].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailB6 = "3";
            }

            //피하지방 손실/근육소모
            if (ssView3_Sheet1.Cells[36, 9].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailC = "0";
            }
            if (ssView3_Sheet1.Cells[36, 10].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailC = "1";
            }
            if (ssView3_Sheet1.Cells[36, 11].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailC = "2";
            }
            if (ssView3_Sheet1.Cells[36, 12].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailC = "3";
            }

            //부종
            if (ssView3_Sheet1.Cells[37, 9].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailD1 = "0";
            }
            if (ssView3_Sheet1.Cells[37, 10].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailD1 = "1";
            }
            if (ssView3_Sheet1.Cells[37, 11].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailD1 = "2";
            }
            if (ssView3_Sheet1.Cells[37, 12].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailD1 = "3";
            }

            //복수
            if (ssView3_Sheet1.Cells[38, 9].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailD2 = "0";
            }
            if (ssView3_Sheet1.Cells[38, 10].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailD2 = "1";
            }
            if (ssView3_Sheet1.Cells[38, 11].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailD2 = "2";
            }
            if (ssView3_Sheet1.Cells[38, 12].BackColor == Color.FromArgb(219, 219, 219))
            {
                cFailD2 = "3";
            }


            nDietA = Convert.ToInt32(VB.Val(ssView3_Sheet1.Cells[41, 9].Text));
            nDietB = Convert.ToInt32(VB.Val(ssView3_Sheet1.Cells[41, 11].Text));

            nDietF = Convert.ToInt32(VB.Val(ssView3_Sheet1.Cells[42, 9].Text));
            nDietG = Convert.ToInt32(VB.Val(ssView3_Sheet1.Cells[42, 11].Text));

            cPlanA = Convert.ToBoolean(ssView3_Sheet1.Cells[44, 7].Value) == true ? "1" : "0";
            cPlanB1 = ssView3_Sheet1.Cells[45, 9].Text;
            cPlanB2 = ssView3_Sheet1.Cells[46, 9].Text;
            cPlanC = Convert.ToBoolean(ssView3_Sheet1.Cells[47, 9].Value) == true ? "1" : "0";
            cPlanD = Convert.ToBoolean(ssView3_Sheet1.Cells[47, 10].Value) == true ? "1" : "0";
            cPlanE = Convert.ToBoolean(ssView3_Sheet1.Cells[47, 11].Value) == true ? "1" : "0";
            cPlanF = ssView3_Sheet1.Cells[48, 4].Text;

            cUDate = ssView3_Sheet1.Cells[52, 10].Text;

            if (cUDate.Trim() == "")
            {
                cUDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            }

            cSabun = ssView3_Sheet1.Cells[52, 12].Text;

            #endregion

            if (cWarning.Trim() == "")
            {
                ComFunc.MsgBox("'위험도'는 필수 선택 항목입니다.");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (cUDate != "" && cSSabun != "")
                {
                    if (clsSupDiet.dst.New == "Y")
                    {
                        #region Insert_Data_S

                        SQL = "";
                        SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_S_SEARCH(SDATE, INDATE, PANO, SNAME, SEX, AGE, DIAGNOSIS, DIETFOOD, DEPTCODE, DRCODE, ";
                        SQL = SQL + ComNum.VBLF + " WARDCODE, ROOMCODE, HEIGHT, WEIGHT, HWEIGHT, IBW, WARNING, ALB, TLC, HB, TCHO, SCORE, IPDNO, UDATE,";
                        SQL = SQL + ComNum.VBLF + " COMMENTS1, COMMENTS2, STATUS, SABUN ) ";
                        SQL = SQL + ComNum.VBLF + " VALUES(TO_DATE('" + cSDate + "','YYYY-MM-DD'), TO_DATE('" + clsSupDiet.dst.IpwonDay + "', 'YYYY-MM-DD HH24:MI:SS') ";
                        SQL = SQL + ComNum.VBLF + ",'" + clsSupDiet.dst.Pano + "','" + clsSupDiet.dst.sName + "','" + clsSupDiet.dst.Sex + "'," + clsSupDiet.dst.Age + ",'" + clsSupDiet.dst.DIAGNOSIS + "'";
                        SQL = SQL + ComNum.VBLF + ",'" + clsSupDiet.dst.DietName + "','" + clsSupDiet.dst.DeptCode + "','" + clsSupDiet.dst.DrCode + "','" + clsSupDiet.dst.WardCode + "','" + clsSupDiet.dst.RoomCode + "'";
                        SQL = SQL + ComNum.VBLF + "," + clsSupDiet.dst.Height + "," + clsSupDiet.dst.Weight + "," + clsSupDiet.dst.HWeight + "," + VB.Val(clsSupDiet.dst.IBW).ToString("0.0") + ",'" + cWarning + "'";
                        SQL = SQL + ComNum.VBLF + "," + VB.Val(clsSupDiet.dst.LabALB) + "," + VB.Val(clsSupDiet.dst.LabTLC) + "," + VB.Val(clsSupDiet.dst.LabHB) + "," + VB.Val(clsSupDiet.dst.LabTcho) + "," + clsSupDiet.dst.Cnt + "," + clsSupDiet.dst.IPDNO + ",";
                        SQL = SQL + ComNum.VBLF + " TO_DATE('" + cUDate + "','YYYY-MM-DD'),'" + cComment1 + "','" + cComment2 + "','" + cWarning + "','" + cSSabun + "')";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        #endregion

                        if (cStatus.Trim() != "")
                        {
                            #region Insert_Data_M

                            SQL = "";
                            SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_S_MANAGER(UDATE, SDATE, PANO, INDATE, SABUN, STATUS, CWEIGHT1, CWEIGHT2, CWEIGHT3, ";
                            SQL = SQL + ComNum.VBLF + " FAILA, FAILB, FAILC, FAILD, DIETA, DIETB, DIETC, DIETD, DIETE, DIETF, DIETG, PLANA, PLANB1, PLANB2, ";
                            SQL = SQL + ComNum.VBLF + " PLANC, PLAND, PLANE, PLANF1, PLANF2, USE) VALUES (";
                            SQL = SQL + ComNum.VBLF + " TO_DATE('" + cUDate + "','YYYY-MM-DD'), TO_DATE('" + cSDate + "','YYYY-MM-DD'), '" + cPano + "', ";
                            SQL = SQL + ComNum.VBLF + " TO_DATE('" + cInDate + "','YYYY-MM-DD HH24:MI:SS'),'" + cSabun + "','" + cStatus + "',";
                            SQL = SQL + ComNum.VBLF + "'" + cWeight1 + "','" + cWeight2 + "','" + cWeight3 + "','" + cFailA + "','" + cFailB + "',";
                            SQL = SQL + ComNum.VBLF + "'" + cFailC + "','" + cFailD + "'," + nDietA + "," + nDietB + "," + nDietC + "," + nDietD + ",";
                            SQL = SQL + ComNum.VBLF + nDietE + "," + nDietF + "," + nDietG + ",'" + cPlanA + "','" + cPlanB1 + "','" + cPlanB2 + "',";
                            SQL = SQL + ComNum.VBLF + "'" + cPlanC + "','" + cPlanD + "','" + cPlanE + "','" + cPlanF1 + "','" + cPlanF2 + "','Y') ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox(SqlErr);
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                            #endregion
                        }

                        clsSupDiet.dst.New = "N";
                    }
                    else
                    {
                        #region Update_Data_S

                        SQL = "";
                        SQL = " UPDATE " + ComNum.DB_PMPA + "DIET_S_SEARCH SET ";
                        SQL = SQL + ComNum.VBLF + " WARNING = '" + cWarning + "', ";
                        SQL = SQL + ComNum.VBLF + " COMMENTS1 = '" + cComment1 + "', ";
                        SQL = SQL + ComNum.VBLF + " COMMENTS2 = '" + cComment2 + "', ";
                        SQL = SQL + ComNum.VBLF + " SABUN = '" + cSSabun + "', ";
                        SQL = SQL + ComNum.VBLF + " IBW = " + nIBW + ",";
                        SQL = SQL + ComNum.VBLF + " ALB = " + nAlb + ",";
                        SQL = SQL + ComNum.VBLF + " TLC = " + nTLC + ",";
                        SQL = SQL + ComNum.VBLF + " HB = " + nHB + ",";
                        SQL = SQL + ComNum.VBLF + " TCHO = " + nTCHO;
                        SQL = SQL + ComNum.VBLF + " WHERE INDATE = TO_DATE('" + cInDate + "','YYYY-MM-DD HH24:MI:SS') ";
                        SQL = SQL + ComNum.VBLF + "   AND PANO = '" + cPano + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND UDATE = TO_DATE('" + cUDate + "','YYYY-MM-DD') ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        #endregion

                        SQL = "";
                        SQL = "SELECT PANO";
                        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "DIET_S_MANAGER_NEW ";
                        SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + cPano + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND INDATE = TO_DATE('" + cInDate + "', 'YYYY-MM-DD HH24:MI:SS') ";
                        SQL = SQL + ComNum.VBLF + "   AND UDATE = TO_DATE('" + cUDate + "','YYYY-MM-DD')";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            if (cStatus.Trim() != "")
                            {
                                #region Update_Data_M_NEW

                                SQL = "";
                                SQL = " UPDATE " + ComNum.DB_PMPA + "DIET_S_MANAGER_NEW SET ";
                                SQL = SQL + ComNum.VBLF + " Status = '" + cStatus + "', ";
                                SQL = SQL + ComNum.VBLF + " CWeight1 = " + VB.Val(cWeight1) + ", ";
                                SQL = SQL + ComNum.VBLF + " CWeight2 = " + VB.Val(cWeight2) + ", ";
                                SQL = SQL + ComNum.VBLF + " CWeight3 = " + VB.Val(cWeight3) + ", ";
                                SQL = SQL + ComNum.VBLF + " FailA1 = '" + cFailA1 + "', ";
                                SQL = SQL + ComNum.VBLF + " FailA2 = '" + cFailA2 + "', ";
                                SQL = SQL + ComNum.VBLF + " FailA3 = '" + cFailA3 + "', ";
                                SQL = SQL + ComNum.VBLF + " FailB1 = '" + cFailB1 + "', ";
                                SQL = SQL + ComNum.VBLF + " FailB2 = '" + cFailB2 + "', ";
                                SQL = SQL + ComNum.VBLF + " FailB3 = '" + cFailB3 + "', ";
                                SQL = SQL + ComNum.VBLF + " FailB4 = '" + cFailB4 + "', ";
                                SQL = SQL + ComNum.VBLF + " FailB5 = '" + cFailB5 + "', ";
                                SQL = SQL + ComNum.VBLF + " FailB6 = '" + cFailB6 + "', ";
                                SQL = SQL + ComNum.VBLF + " FailC1 = '" + cFailC1 + "', ";
                                SQL = SQL + ComNum.VBLF + " FailD1 = '" + cFailD1 + "', ";
                                SQL = SQL + ComNum.VBLF + " FailD2 = '" + cFailD2 + "', ";
                                SQL = SQL + ComNum.VBLF + " DietA = " + nDietA + ", ";
                                SQL = SQL + ComNum.VBLF + " DietB = " + nDietB + ", ";
                                SQL = SQL + ComNum.VBLF + " DietC = " + nDietC + ", ";
                                SQL = SQL + ComNum.VBLF + " DietD = " + nDietD + ", ";
                                SQL = SQL + ComNum.VBLF + " DietE = " + nDietE + ", ";
                                SQL = SQL + ComNum.VBLF + " DietF = " + nDietF + ", ";
                                SQL = SQL + ComNum.VBLF + " DietG = " + nDietG + ", ";
                                SQL = SQL + ComNum.VBLF + " PlanA = '" + cPlanA + "', ";
                                SQL = SQL + ComNum.VBLF + " PlanB1 = '" + cPlanB1 + "', ";
                                SQL = SQL + ComNum.VBLF + " PlanB2 = '" + cPlanB2 + "', ";
                                SQL = SQL + ComNum.VBLF + " PlanC = '" + cPlanC + "', ";
                                SQL = SQL + ComNum.VBLF + " PlanD = '" + cPlanD + "', ";
                                SQL = SQL + ComNum.VBLF + " PlanE = '" + cPlanE + "', ";
                                SQL = SQL + ComNum.VBLF + " PlanF = '" + cPlanF + "', ";
                                SQL = SQL + ComNum.VBLF + " SABUN = '" + cSabun + "'";
                                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + cPano + "' ";
                                SQL = SQL + ComNum.VBLF + "   AND INDATE = TO_DATE('" + cInDate + "', 'YYYY-MM-DD HH24:MI:SS') ";
                                SQL = SQL + ComNum.VBLF + "   AND UDATE = TO_DATE('" + cUDate + "','YYYY-MM-DD')";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    if (dt != null)
                                    {
                                        dt.Dispose();
                                        dt = null;
                                    }
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox(SqlErr);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }

                                #endregion
                            }
                        }
                        else
                        {
                            if (cStatus.Trim() != "")
                            {
                                #region Insert_Data_M_NEW

                                SQL = "";
                                SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_S_MANAGER_NEW(UDATE, SDATE, PANO, ";
                                SQL = SQL + ComNum.VBLF + " INDATE, SABUN, STATUS, CWEIGHT1, ";
                                SQL = SQL + ComNum.VBLF + " CWEIGHT2, CWEIGHT3, ";
                                SQL = SQL + ComNum.VBLF + " FAILA1, FAILA2, FAILA3, ";
                                SQL = SQL + ComNum.VBLF + " FAILB1, FAILB2, FAILB3, FAILB4, FAILB5, FAILB6, ";
                                SQL = SQL + ComNum.VBLF + " FAILC1, FAILD1, FAILD2, ";
                                SQL = SQL + ComNum.VBLF + " DIETA , DIETB, DIETC, DIETD, ";
                                SQL = SQL + ComNum.VBLF + " DIETE, DIETF, DIETG, PLANA, ";
                                SQL = SQL + ComNum.VBLF + " PLANB1, PLANB2, PLANC, PLAND, ";
                                SQL = SQL + ComNum.VBLF + " PLANE, PLANF, USE) VALUES (";
                                SQL = SQL + ComNum.VBLF + " TO_DATE('" + cUDate + "','YYYY-MM-DD'), TO_DATE('" + cSDate + "','YYYY-MM-DD'), '" + cPano + "', ";
                                SQL = SQL + ComNum.VBLF + " TO_DATE('" + cInDate + "','YYYY-MM-DD HH24:MI:SS'),'" + cSabun + "','" + cStatus + "','" + cWeight1 + "',";
                                SQL = SQL + ComNum.VBLF + "'" + cWeight2 + "','" + cWeight3 + "',";
                                SQL = SQL + ComNum.VBLF + "'" + cFailA1 + "','" + cFailA2 + "','" + cFailA3 + "',";
                                SQL = SQL + ComNum.VBLF + "'" + cFailB1 + "','" + cFailB2 + "','" + cFailB3 + "','" + cFailB4 + "','" + cFailB5 + "','" + cFailB6 + "',";
                                SQL = SQL + ComNum.VBLF + "'" + cFailC + "','" + cFailD1 + "','" + cFailD2 + "',";
                                SQL = SQL + ComNum.VBLF + nDietA + "," + nDietB + "," + nDietC + "," + nDietD + ",";
                                SQL = SQL + ComNum.VBLF + nDietE + "," + nDietF + "," + nDietG + ",'" + cPlanA + "',";
                                SQL = SQL + ComNum.VBLF + "'" + cPlanB1 + "','" + cPlanB2 + "','" + cPlanC + "','" + cPlanD + "',";
                                SQL = SQL + ComNum.VBLF + "'" + cPlanE + "','" + cPlanF + "','Y') ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    if (dt != null)
                                    {
                                        dt.Dispose();
                                        dt = null;
                                    }
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox(SqlErr);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }

                                #endregion
                            }
                        }

                        dt.Dispose();
                        dt = null;
                    }

                    clsSupDiet.dst.UDATE = cUDate;
                    clsDB.setCommitTran(clsDB.DbCon);
                    ComFunc.MsgBox("저장하였습니다.");

                    SaveXML_2094(cPano, cInDate, cUDate);
                    SaveXML_2091(cPano, cInDate, cUDate);
                }
                else
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("등록일자 또는 영양사 서명이 없습니다.");
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            Sheet_Clear();

            if (clsSupDiet.dst.Pano == "" || clsSupDiet.dst.IpwonDay == "")
            {
                ComFunc.MsgBox("등록번호 또는 입원날짜가 없습니다.");
                this.Close();
                return;
            }
            else
            {
                //구입력자일 경우
                Read_S_Search(clsSupDiet.dst.Pano, clsSupDiet.dst.IpwonDay, clsSupDiet.dst.UDATE);
                Read_S_Manager(clsSupDiet.dst.Pano, clsSupDiet.dst.IpwonDay, clsSupDiet.dst.UDATE);
                Read_S_Manager_New(clsSupDiet.dst.Pano, clsSupDiet.dst.IpwonDay, clsSupDiet.dst.UDATE);
            }
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strTemp = "";

            if (clsSupDiet.dst.New == "V")
            {
                return;
            }

            if (e.Column == 11 && e.Row == 33)
            {
                frmCalendar frm = new frmCalendar();
                frm.ShowDialog();

                ssView_Sheet1.Cells[33, 11].Text = clsPublic.GstrCalDate;

                clsPublic.GstrCalDate = "";
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (e.Column >= 8 && e.Column <= 13 && e.Row == 12)
                {
                    strTemp = ssView_Sheet1.Cells[33, 11].Text.Trim();

                    SQL = "";
                    SQL = " SELECT HEIGHT, WEIGHT ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_S_SEARCH ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + clsSupDiet.dst.Pano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND INDATE = TO_DATE('" + clsSupDiet.dst.IpwonDay + "','YYYY-MM-DD HH24:MI:SS') ";
                    if (strTemp != "")
                    {
                        SQL = SQL + ComNum.VBLF + " AND UDATE = TO_DATE('" + strTemp + "','YYYY-MM-DD') ";
                    }

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        panBody.Visible = true; //키 몸무게 수정
                        txtHeight.Text = VB.Val(dt.Rows[0]["HEIGHT"].ToString().Trim()).ToString("##0.0");
                        txtWeight.Text = VB.Val(dt.Rows[0]["WEIGHT"].ToString().Trim()).ToString("##0.0");
                    }

                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private void ssView2_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            //각 항목별 중복 선택 불가능하게

            //1개월, 2개월, 3개월
            if (e.Row == 19 || e.Row == 20 || e.Row == 21)
            {
                if (e.Column == 9)
                {
                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[e.Row, 9].Value) == true)
                    {
                        ssView2_Sheet1.Cells[e.Row, 10].Value = false;
                        ssView2_Sheet1.Cells[e.Row, 11].Value = false;
                        ssView2_Sheet1.Cells[e.Row, 12].Value = false;
                    }
                }
                else if (e.Column == 10)
                {
                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[e.Row, 10].Value) == true)
                    {
                        ssView2_Sheet1.Cells[e.Row, 9].Value = false;
                        ssView2_Sheet1.Cells[e.Row, 11].Value = false;
                        ssView2_Sheet1.Cells[e.Row, 12].Value = false;
                    }
                }
                else if (e.Column == 11)
                {
                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[e.Row, 11].Value) == true)
                    {
                        ssView2_Sheet1.Cells[e.Row, 9].Value = false;
                        ssView2_Sheet1.Cells[e.Row, 10].Value = false;
                        ssView2_Sheet1.Cells[e.Row, 12].Value = false;
                    }
                }
                else if (e.Column == 12)
                {
                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[e.Row, 12].Value) == true)
                    {
                        ssView2_Sheet1.Cells[e.Row, 9].Value = false;
                        ssView2_Sheet1.Cells[e.Row, 10].Value = false;
                        ssView2_Sheet1.Cells[e.Row, 11].Value = false;
                    }
                }
            }
        }

        private void ssView2_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            //각 항목별 중복 선택 불가능하게

            //식욕/섭취상태, 식사시 문제, 피하지방 손실/근육소모, 부종/복수
            if (e.Row == 24 || e.Row == 25 || e.Row == 26 || e.Row == 27)
            {
                if (e.Column == 9 || e.Column == 10 || e.Column == 11 || e.Column == 12)
                {
                    ssView2_Sheet1.Cells[e.Row, e.Column].BackColor = Color.FromArgb(219, 219, 219);
                }

                if (e.Column == 9)
                {
                    if (ssView2_Sheet1.Cells[e.Row, e.Column].BackColor == Color.FromArgb(219, 219, 219))
                    {
                        ssView2_Sheet1.Cells[e.Row, 10].BackColor = Color.White;
                        ssView2_Sheet1.Cells[e.Row, 11].BackColor = Color.White;
                        ssView2_Sheet1.Cells[e.Row, 12].BackColor = Color.White;
                    }
                }
                else if (e.Column == 10)
                {
                    if (ssView2_Sheet1.Cells[e.Row, e.Column].BackColor == Color.FromArgb(219, 219, 219))
                    {
                        ssView2_Sheet1.Cells[e.Row, 9].BackColor = Color.White;
                        ssView2_Sheet1.Cells[e.Row, 11].BackColor = Color.White;
                        ssView2_Sheet1.Cells[e.Row, 12].BackColor = Color.White;
                    }
                }
                else if (e.Column == 11)
                {
                    if (ssView2_Sheet1.Cells[e.Row, e.Column].BackColor == Color.FromArgb(219, 219, 219))
                    {
                        ssView2_Sheet1.Cells[e.Row, 9].BackColor = Color.White;
                        ssView2_Sheet1.Cells[e.Row, 10].BackColor = Color.White;
                        ssView2_Sheet1.Cells[e.Row, 12].BackColor = Color.White;
                    }
                }
                else if (e.Column == 12)
                {
                    if (ssView2_Sheet1.Cells[e.Row, e.Column].BackColor == Color.FromArgb(219, 219, 219))
                    {
                        ssView2_Sheet1.Cells[e.Row, 9].BackColor = Color.White;
                        ssView2_Sheet1.Cells[e.Row, 10].BackColor = Color.White;
                        ssView2_Sheet1.Cells[e.Row, 11].BackColor = Color.White;
                    }
                }
            }
        }

        private void ssView2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (clsSupDiet.dst.New == "V")
            {
                return;
            }

            if (e.Column == 10 && e.Row == 41)
            {
                frmCalendar frm = new frmCalendar();
                frm.ShowDialog();

                ssView2_Sheet1.Cells[41, 10].Text = clsPublic.GstrCalDate;

                clsPublic.GstrCalDate = "";
            }
        }

        void Sheet_Clear()
        {
            int i = 0;
            int j = 0;

            //ssView
            ssView_Sheet1.Cells[1, 12, 6, 12].Text = "";

            ssView_Sheet1.Cells[9, 11].Text = "";

            ssView_Sheet1.Cells[11, 8, 12, 8].Text = "";

            ssView_Sheet1.Cells[17, 9].Text = "";
            ssView_Sheet1.Cells[19, 9, 23, 9].Text = "";

            ssView_Sheet1.Cells[26, 10].Text = "";

            ssView_Sheet1.Cells[29, 8, 30, 8].Text = "";

            ssView_Sheet1.Cells[33, 11].Text = "";
            ssView_Sheet1.Cells[33, 13].Text = "";

            for (i = 10; i <= 13; i++)
            {
                for (j = 17; j <= 23; j++)
                {
                    ssView_Sheet1.Cells[j, i].BackColor = Color.White;
                }
            }


            //ssView2
            ssView2_Sheet1.Cells[1, 11, 6, 11].Text = "";

            ssView2_Sheet1.Cells[11, 7, 12, 7].Text = "";

            ssView2_Sheet1.Cells[17, 8].Text = "";
            ssView2_Sheet1.Cells[22, 8, 23, 8].Text = "";

            for (i = 19; i <= 21; i++)
            {
                for (j = 9; j <= 12; j++)
                {
                    ssView2_Sheet1.Cells[i, j].Value = false;
                    ssView2_Sheet1.Cells[i, j].BackColor = Color.White;
                }
            }

            for (i = 24; i <= 27; i++)
            {
                for (j = 9; j <= 12; j++)
                {
                    ssView2_Sheet1.Cells[i, j].BackColor = Color.White;
                }
            }

            for (i = 17; i <= 18; i++)
            {
                for (j = 9; j <= 12; j++)
                {
                    ssView2_Sheet1.Cells[i, j].BackColor = Color.White;
                }
            }

            for (i = 22; i <= 23; i++)
            {
                for (j = 9; j <= 12; j++)
                {
                    ssView2_Sheet1.Cells[i, j].BackColor = Color.White;
                }
            }

            ssView2_Sheet1.Cells[30, 9].Text = "";
            ssView2_Sheet1.Cells[30, 11].Text = "";

            ssView2_Sheet1.Cells[31, 9].Text = "";
            ssView2_Sheet1.Cells[31, 11].Text = "";

            ssView2_Sheet1.Cells[33, 7].Value = false;

            ssView2_Sheet1.Cells[34, 9, 35, 9].Text = "";

            ssView2_Sheet1.Cells[36, 9, 36, 11].Value = false;

            ssView2_Sheet1.Cells[37, 9, 38, 9].Text = "";

            ssView2_Sheet1.Cells[41, 10].Text = "";
            ssView2_Sheet1.Cells[41, 12].Text = "";


            //ssView3
            ssView3_Sheet1.Cells[1, 11, 6, 11].Text = "";

            ssView3_Sheet1.Cells[11, 7, 12, 7].Text = "";

            ssView3_Sheet1.Cells[17, 8].Text = "";
            ssView3_Sheet1.Cells[22, 8, 23, 8].Text = "";

            for (i = 19; i <= 21; i++)
            {
                for (j = 9; j <= 12; j++)
                {
                    ssView3_Sheet1.Cells[i, j].Value = false;
                    ssView3_Sheet1.Cells[i, j].BackColor = Color.White;
                }
            }

            for (i = 27; i <= 35; i++)
            {
                for (j = 9; j <= 12; j++)
                {
                    ssView3_Sheet1.Cells[i, j].BackColor = Color.White;
                }
            }

            for (i = 17; i <= 18; i++)
            {
                for (j = 9; j <= 12; j++)
                {
                    ssView3_Sheet1.Cells[i, j].BackColor = Color.White;
                }
            }

            for (i = 22; i <= 23; i++)
            {
                for (j = 9; j <= 12; j++)
                {
                    ssView3_Sheet1.Cells[i, j].BackColor = Color.White;
                }
            }

            ssView3_Sheet1.Cells[41, 9].Text = "";
            ssView3_Sheet1.Cells[41, 11].Text = "";

            ssView3_Sheet1.Cells[42, 9].Text = "";
            ssView3_Sheet1.Cells[42, 11].Text = "";

            ssView3_Sheet1.Cells[44, 7].Value = false;

            ssView3_Sheet1.Cells[45, 9, 46, 9].Text = "";

            ssView3_Sheet1.Cells[47, 9, 47, 11].Value = false;

            ssView3_Sheet1.Cells[40, 9, 41, 9].Text = "";

            ssView3_Sheet1.Cells[48, 4].Text = "";
        }

        void ssView_LabColor(string argGubun, double argValue, string argSEX = "")
        {
            int nRow = 0;
            int nCol = 0;

            switch (argGubun)
            {
                case "IBW":
                    if (argValue >= 90 && argValue <= 110)
                    {
                        nRow = 17;
                        nCol = 10;
                    }
                    else if (argValue >= 80 && argValue <= 89)
                    {
                        nRow = 17;
                        nCol = 11;
                    }
                    else if (argValue >= 70 && argValue <= 79)
                    {
                        nRow = 17;
                        nCol = 12;
                    }
                    else if (argValue < 70)
                    {
                        nRow = 17;
                        nCol = 13;
                    }
                    else if (argValue >= 111 && argValue <= 120)
                    {
                        nRow = 18;
                        nCol = 11;
                    }
                    else if (argValue >= 121 && argValue <= 130)
                    {
                        nRow = 18;
                        nCol = 12;
                    }
                    else if (argValue > 130)
                    {
                        nRow = 18;
                        nCol = 13;
                    }
                    break;
                case "ALB":
                    nRow = 19;
                    if (argValue >= 3.1 && argValue <= 5.5)
                    {
                        nCol = 10;
                    }
                    else if (argValue >= 2.8 && argValue <= 3)
                    {
                        nCol = 11;
                    }
                    else if (argValue >= 2.3 && argValue <= 2.7)
                    {
                        nCol = 12;
                    }
                    else if (argValue < 2.3)
                    {
                        nCol = 13;
                    }
                    break;
                case "TLC":
                    nRow = 20;
                    if (argValue > 1.5)
                    {
                        nCol = 10;
                    }
                    else if (argValue >= 1.2 && argValue <= 1.5)
                    {
                        nCol = 11;
                    }
                    else if (argValue >= 0.8 && argValue <= 1.2)
                    {
                        nCol = 12;
                    }
                    else if (argValue < 0.8)
                    {
                        nCol = 13;
                    }
                    break;
                case "HB":
                    if (argSEX == "M")
                    {
                        nRow = 22;
                        if (argValue >= 14)
                        {
                            nCol = 10;
                        }
                        else if (argValue >= 12 && argValue <= 13.9)
                        {
                            nCol = 11;
                        }
                        else if (argValue >= 9 && argValue <= 11.9)
                        {
                            nCol = 12;
                        }
                        else if (argValue < 9)
                        {
                            nCol = 13;
                        }
                    }
                    else if (argSEX == "F")
                    {
                        nRow = 21;
                        if (argValue >= 12)
                        {
                            nCol = 10;
                        }
                        else if (argValue >= 10 && argValue <= 11.9)
                        {
                            nCol = 11;
                        }
                        else if (argValue >= 8 && argValue <= 9.9)
                        {
                            nCol = 12;
                        }
                        else if (argValue < 8)
                        {
                            nCol = 13;
                        }
                    }
                    break;
                case "TCHO":
                    nRow = 23;
                    if (argValue < 200)
                    {
                        nCol = 10;
                    }
                    else if (argValue >= 200 && argValue <= 239)
                    {
                        nCol = 11;
                    }
                    else if (argValue >= 240 && argValue <= 299)
                    {
                        nCol = 12;
                    }
                    else if (argValue > 300)
                    {
                        nCol = 13;
                    }
                    break;
            }

            ssView_Sheet1.Cells[nRow, nCol].BackColor = Color.FromArgb(219, 219, 219);
        }

        void ssView2_LabColor(string argGubun, double argValue, string argSEX = "")
        {
            int nRow = 0;
            int nCol = 0;

            switch (argGubun)
            {
                case "IBW":
                    if (argValue >= 90 && argValue <= 110)
                    {
                        nRow = 17;
                        nCol = 9;
                    }
                    else if (argValue >= 80 && argValue <= 89)
                    {
                        nRow = 17;
                        nCol = 10;
                    }
                    else if (argValue >= 70 && argValue <= 79)
                    {
                        nRow = 17;
                        nCol = 11;
                    }
                    else if (argValue < 70)
                    {
                        nRow = 17;
                        nCol = 12;
                    }
                    else if (argValue >= 111 && argValue <= 120)
                    {
                        nRow = 18;
                        nCol = 10;
                    }
                    else if (argValue >= 121 && argValue <= 130)
                    {
                        nRow = 18;
                        nCol = 11;
                    }
                    else if (argValue > 130)
                    {
                        nRow = 18;
                        nCol = 12;
                    }
                    break;
                case "ALB":
                    nRow = 22;
                    if (argValue >= 3.1 && argValue <= 5.5)
                    {
                        nCol = 9;
                    }
                    else if (argValue >= 2.8 && argValue <= 3)
                    {
                        nCol = 10;
                    }
                    else if (argValue >= 2.3 && argValue <= 2.7)
                    {
                        nCol = 11;
                    }
                    else if (argValue < 2.3)
                    {
                        nCol = 12;
                    }
                    break;
                case "TLC":
                    nRow = 23;
                    if (argValue > 1.5)
                    {
                        nCol = 9;
                    }
                    else if (argValue >= 1.2 && argValue <= 1.5)
                    {
                        nCol = 10;
                    }
                    else if (argValue >= 0.8 && argValue <= 1.2)
                    {
                        nCol = 11;
                    }
                    else if (argValue < 0.8)
                    {
                        nCol = 12;
                    }
                    break;
            }

            ssView2_Sheet1.Cells[nRow, nCol].BackColor = Color.FromArgb(219, 219, 219);
        }

        void ssView3_LabColor(string argGubun, double argValue, string argSEX = "")
        {
            int nRow = 0;
            int nCol = 0;

            switch (argGubun)
            {
                case "IBW":
                    if (argValue >= 90 && argValue <= 110)
                    {
                        nRow = 17;
                        nCol = 9;
                    }
                    else if (argValue >= 80 && argValue <= 89)
                    {
                        nRow = 17;
                        nCol = 10;
                    }
                    else if (argValue >= 70 && argValue <= 79)
                    {
                        nRow = 17;
                        nCol = 11;
                    }
                    else if (argValue < 70)
                    {
                        nRow = 17;
                        nCol = 12;
                    }
                    else if (argValue >= 111 && argValue <= 120)
                    {
                        nRow = 18;
                        nCol = 10;
                    }
                    else if (argValue >= 121 && argValue <= 130)
                    {
                        nRow = 18;
                        nCol = 11;
                    }
                    else if (argValue > 130)
                    {
                        nRow = 18;
                        nCol = 12;
                    }
                    break;
                case "ALB":
                    nRow = 22;
                    if (argValue >= 3.1 && argValue <= 5.5)
                    {
                        nCol = 9;
                    }
                    else if (argValue >= 2.8 && argValue <= 3)
                    {
                        nCol = 10;
                    }
                    else if (argValue >= 2.3 && argValue <= 2.7)
                    {
                        nCol = 11;
                    }
                    else if (argValue < 2.3)
                    {
                        nCol = 12;
                    }
                    break;
                case "TLC":
                    nRow = 23;
                    if (argValue > 1.5)
                    {
                        nCol = 9;
                    }
                    else if (argValue >= 1.2 && argValue <= 1.5)
                    {
                        nCol = 10;
                    }
                    else if (argValue >= 0.8 && argValue <= 1.2)
                    {
                        nCol = 11;
                    }
                    else if (argValue < 0.8)
                    {
                        nCol = 12;
                    }
                    break;
                case "HB":
                    if (argSEX == "M")
                    {
                        nRow = 24;
                        if (argValue >= 12)
                        {
                            nCol = 9;
                        }
                        else if (argValue >= 10 && argValue <= 11.9)
                        {
                            nCol = 10;
                        }
                        else if (argValue >= 8 && argValue <= 9.9)
                        {
                            nCol = 11;
                        }
                        else if (argValue < 8)
                        {
                            nCol = 12;
                        }
                    }
                    else if (argSEX == "F")
                    {
                        nRow = 25;
                        if (argValue >= 14)
                        {
                            nCol = 9;
                        }
                        else if (argValue >= 12 && argValue <= 13.9)
                        {
                            nCol = 10;
                        }
                        else if (argValue >= 9 && argValue <= 11.9)
                        {
                            nCol = 11;
                        }
                        else if (argValue < 9)
                        {
                            nCol = 12;
                        }
                    }
                    break;
                case "TCHO":
                    nRow = 26;
                    if (argValue < 200)
                    {
                        nCol = 9;
                    }
                    else if (argValue >= 200 && argValue <= 239)
                    {
                        nCol = 10;
                    }
                    else if (argValue >= 240 && argValue <= 299)
                    {
                        nCol = 11;
                    }
                    else if (argValue > 300)
                    {
                        nCol = 12;
                    }
                    break;
            }

            ssView3_Sheet1.Cells[nRow, nCol].BackColor = Color.FromArgb(219, 219, 219);
        }

        private void ssView3_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            //각 항목별 중복 선택 불가능하게

            //식욕/섭취상태, 식사시 문제, 피하지방 손실/근육소모, 부종/복수
            if (e.Row >= 27 || e.Row <= 38)
            {
                if (e.Column == 9 || e.Column == 10 || e.Column == 11 || e.Column == 12)
                {
                    ssView3_Sheet1.Cells[e.Row, e.Column].BackColor = Color.FromArgb(219, 219, 219);
                }

                if (e.Column == 9)
                {
                    if (ssView3_Sheet1.Cells[e.Row, e.Column].BackColor == Color.FromArgb(219, 219, 219))
                    {
                        ssView3_Sheet1.Cells[e.Row, 10].BackColor = Color.White;
                        ssView3_Sheet1.Cells[e.Row, 11].BackColor = Color.White;
                        ssView3_Sheet1.Cells[e.Row, 12].BackColor = Color.White;
                    }
                }
                else if (e.Column == 10)
                {
                    if (ssView3_Sheet1.Cells[e.Row, e.Column].BackColor == Color.FromArgb(219, 219, 219))
                    {
                        ssView3_Sheet1.Cells[e.Row, 9].BackColor = Color.White;
                        ssView3_Sheet1.Cells[e.Row, 11].BackColor = Color.White;
                        ssView3_Sheet1.Cells[e.Row, 12].BackColor = Color.White;
                    }
                }
                else if (e.Column == 11)
                {
                    if (ssView3_Sheet1.Cells[e.Row, e.Column].BackColor == Color.FromArgb(219, 219, 219))
                    {
                        ssView3_Sheet1.Cells[e.Row, 9].BackColor = Color.White;
                        ssView3_Sheet1.Cells[e.Row, 10].BackColor = Color.White;
                        ssView3_Sheet1.Cells[e.Row, 12].BackColor = Color.White;
                    }
                }
                else if (e.Column == 12)
                {
                    if (ssView3_Sheet1.Cells[e.Row, e.Column].BackColor == Color.FromArgb(219, 219, 219))
                    {
                        ssView3_Sheet1.Cells[e.Row, 9].BackColor = Color.White;
                        ssView3_Sheet1.Cells[e.Row, 10].BackColor = Color.White;
                        ssView3_Sheet1.Cells[e.Row, 11].BackColor = Color.White;
                    }
                }
            }
        }

        private void ssView3_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (clsSupDiet.dst.New == "V")
            {
                return;
            }

            if (e.Column == 10 && e.Row == 52)
            {
                frmCalendar frm = new frmCalendar();
                frm.ShowDialog();

                ssView3_Sheet1.Cells[e.Row, e.Column].Text = clsPublic.GstrCalDate;

                clsPublic.GstrCalDate = "";
            }
        }

        bool SaveXML_2094(string argPano, string argInDate, string argUDATE)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            double dblEmrNo = 0;
            double dblEmrHisNo = 0;
            string strHead = "";
            string strChartX1 = "";
            string strChartX2 = "";
            string strXML = "";
            string strXMLCert = "";

            string strSabun = "";
            string strPANO = "";
            string strUDate = "";
            string strInDate = "";
            string strInTime = "";
            string strDeptCode = "";
            string strDrCd = "";
            string strTemp = "";

            string[] arrIt = new string[21];
            string[] arrTa = new string[6];
            string[] arrDt = new string[6];

            string strROWID = "";

            string strChartDate = "";
            string strChartTime = "";

            string strSysDate = "";
            string strSysTime = "";

            bool rtnVar = false;

            for (i = 0; i < arrIt.Length; i++)
            {
                arrIt[i] = "";
            }

            for (i = 0; i < arrTa.Length; i++)
            {
                arrTa[i] = "";
            }

            for (i = 0; i < arrDt.Length; i++)
            {
                arrDt[i] = "";
            }

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " SELECT SDATE, INDATE, PANO, SNAME, SEX, AGE, DIAGNOSIS, DIETFOOD, DEPTCODE, DRCODE, ";
                SQL = SQL + ComNum.VBLF + "  WARDCODE, ROOMCODE, HEIGHT, WEIGHT, HWEIGHT, IBW, WARNING, ALB, TLC, HB, TCHO, SCORE, ";
                SQL = SQL + ComNum.VBLF + "  COMMENTS1, COMMENTS2, SABUN, EMRNO, ROWID ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "DIET_S_SEARCH ";
                SQL = SQL + ComNum.VBLF + " WHERE INDATE = TO_DATE('" + argInDate + "', 'YYYY-MM-DD HH24:MI:SS')";
                SQL = SQL + ComNum.VBLF + "   AND PANO = '" + argPano + "'";
                SQL = SQL + ComNum.VBLF + "   AND UDATE = TO_DATE('" + argUDATE + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND SABUN IS NOT NULL ";
                SQL = SQL + ComNum.VBLF + " ORDER BY UDATE ASC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }

                if (dt.Rows.Count > 0)
                {
                    strPANO = argPano;
                    strInDate = VB.Left(argInDate, 10);
                    strInTime = VB.Mid(argInDate, 11, argInDate.Length);
                    strUDate = argUDATE;
                    strDeptCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    strDrCd = dt.Rows[0]["DRCODE"].ToString().Trim();
                    strSabun = Read_Sabun(dt.Rows[0]["SABUN"].ToString().Trim().PadLeft(5, '0'));
                    strROWID = dt.Rows[0]["ROWID"].ToString().Trim();

                    arrIt[1] = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    arrIt[2] = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    arrIt[3] = dt.Rows[0]["SEX"].ToString().Trim() == "M" ? "남" : "여";
                    arrIt[4] = dt.Rows[0]["AGE"].ToString().Trim();
                    arrIt[5] = dt.Rows[0]["DIAGNOSIS"].ToString().Trim();

                    arrIt[5] = arrIt[5].Replace("&", " and ");
                    arrIt[5] = arrIt[5].Replace("<", "〈");
                    arrIt[5] = arrIt[5].Replace(">", "〉");

                    strTemp = dt.Rows[0]["WARNING"].ToString().Trim();
                    arrIt[6] = strTemp == "H" ? "고위험군" : (strTemp == "M" ? "중위험군" : "저위험군");
                    arrDt[3] = ComFunc.FormatStrToDate(dt.Rows[0]["INDATE"].ToString().Trim(), "A");
                    arrIt[8] = dt.Rows[0]["HEIGHT"].ToString().Trim() + "cm";
                    arrIt[9] = dt.Rows[0]["WEIGHT"].ToString().Trim() + "kg";
                    arrIt[10] = dt.Rows[0]["HWEIGHT"].ToString().Trim() + "kg";
                    arrIt[11] = dt.Rows[0]["IBW"].ToString().Trim();

                    arrIt[12] = dt.Rows[0]["IBW"].ToString().Trim();
                    arrIt[13] = dt.Rows[0]["ALB"].ToString().Trim();
                    arrIt[14] = dt.Rows[0]["TLC"].ToString().Trim();
                    arrIt[15] = dt.Rows[0]["HB"].ToString().Trim();
                    arrIt[16] = dt.Rows[0]["TCHO"].ToString().Trim();
                    arrIt[17] = strTemp == "H" ? "고위험군" : (strTemp == "M" ? "중위험군" : "저위험군");

                    arrTa[1] = dt.Rows[0]["COMMENTS1"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["COMMENTS2"].ToString().Trim();
                    arrDt[1] = dt.Rows[0]["SDATE"].ToString().Trim();

                    dblEmrNo = VB.Val(dt.Rows[0]["EMRNO"].ToString().Trim());
                }
                else
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    rtnVar = false;
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                dt.Dispose();
                dt = null;


                if (dblEmrNo != 0)
                {
                    //기존차트를 변경할 경우 : 백업 테이블로 백업을 하고 신규 data를 입력한다
                    //KOSMOS_EMR.EMRXMLHISTORY_HISTORYNO_SEQ

                    dblEmrHisNo = ComQuery.GetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "EMRXMLHISNO");

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_EMR + "EMRXMLHISTORY";
                    SQL = SQL + ComNum.VBLF + "      (HISTORYNO, EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                    SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,HISTORYWRITEDATE,HISTORYWRITETIME,DELUSEID,CERTNO)";
                    SQL = SQL + ComNum.VBLF + " SELECT  " + dblEmrHisNo + ",";
                    SQL = SQL + ComNum.VBLF + "      EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                    SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,";
                    SQL = SQL + ComNum.VBLF + "      '" + ComQuery.CurrentDateTime(clsDB.DbCon, "D").Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "      '" + ComQuery.CurrentDateTime(clsDB.DbCon, "T").Trim() + "', '" + clsType.User.Sabun + "',CERTNO";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRXML";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + dblEmrNo;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return rtnVar;
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " DELETE FROM " + ComNum.DB_EMR + "EMRXML";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + dblEmrNo;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return rtnVar;
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " DELETE FROM " + ComNum.DB_EMR + "EMRXMLMST";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + dblEmrNo;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return rtnVar;
                    }
                }

                strXML = "";
                strHead = "<?xml version=" + VB.Chr(34) + "1.0" + VB.Chr(34) + " encoding=" + VB.Chr(34) + "UTF-8" + VB.Chr(34) + "?>";
                strChartX1 = "<chart>";
                strChartX2 = "</chart>";
                strXML = strHead + strChartX1;
                strXML = strXML + MakeXML_Entity("IT", "it1", "진료과", arrIt[1]);
                strXML = strXML + MakeXML_Entity("IT", "it2", "병실", arrIt[2]);
                strXML = strXML + MakeXML_Entity("IT", "it3", "성별", arrIt[3]);
                strXML = strXML + MakeXML_Entity("IT", "it4", "나이", arrIt[4]);
                strXML = strXML + MakeXML_Entity("IT", "it5", "진단명", arrIt[5]);
                strXML = strXML + MakeXML_Entity("IT", "it6", "위험도", arrIt[6]);
                strXML = strXML + MakeXML_Entity("IT", "it8", "Ht", arrIt[8]);
                strXML = strXML + MakeXML_Entity("IT", "it9", "Wt", arrIt[9]);
                strXML = strXML + MakeXML_Entity("IT", "it10", "표준체중", arrIt[10]);
                strXML = strXML + MakeXML_Entity("IT", "it11", "%IBW", arrIt[11]);
                strXML = strXML + MakeXML_Entity("IT", "it12", "%IBW", arrIt[12]);
                strXML = strXML + MakeXML_Entity("IT", "it13", "ALB", arrIt[13]);
                strXML = strXML + MakeXML_Entity("IT", "it14", "TLC", arrIt[14]);
                strXML = strXML + MakeXML_Entity("IT", "it15", "HB", arrIt[15]);
                strXML = strXML + MakeXML_Entity("IT", "it16", "T-chol", arrIt[16]);
                strXML = strXML + MakeXML_Entity("IT", "it17", "위험군", arrIt[17]);
                strXML = strXML + MakeXML_Entity("dt", "dt1", "작성일자", arrDt[1]);
                strXML = strXML + MakeXML_Entity("dt", "dt3", "입원일자", arrDt[3]);
                strXML = strXML + MakeXML_Entity("ta", "ta1", "Comments", arrTa[1]);

                strXML = strXML + strChartX2;
                strXMLCert = strXML;


                SQL = "";
                SQL = "SELECT " + ComNum.DB_EMR + "GetEmrXmlNo() FunSeqNo FROM Dual";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }

                if (dt.Rows.Count > 0)
                {
                    dblEmrNo = VB.Val(dt.Rows[0]["FunSeqNo"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;


                SQL = "";
                SQL = " SELECT TO_CHAR(SYSDATE,'YYYYMMDD') AS CURRENTDATE, ";
                SQL = SQL + ComNum.VBLF + "             TO_CHAR(SYSDATE,'HH24MISS') AS CURRENTTIME ";
                SQL = SQL + ComNum.VBLF + "    FROM DUAL ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }

                if (dt.Rows.Count > 0)
                {
                    strChartDate = dt.Rows[0]["CURRENTDATE"].ToString().Trim();
                    strChartTime = dt.Rows[0]["CURRENTTIME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;


                OracleCommand cmd = new OracleCommand();

                PsmhDb pDbCon = clsDB.DbCon;

                cmd.Connection = pDbCon.Con;
                cmd.CommandText = "KOSMOS_EMR.XMLINSRT3";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("p_EMRNO", OracleDbType.Double, 0, dblEmrNo, ParameterDirection.Input);
                cmd.Parameters.Add("p_FORMNO", OracleDbType.Double, 0, 2094, ParameterDirection.Input);
                cmd.Parameters.Add("p_USEID", OracleDbType.Varchar2, 8, strSabun, ParameterDirection.Input);
                cmd.Parameters.Add("p_CHARTDATE", OracleDbType.Varchar2, 8, strUDate.Replace("-", ""), ParameterDirection.Input);
                cmd.Parameters.Add("p_CHARTTIME", OracleDbType.Varchar2, 6, "120000", ParameterDirection.Input);
                cmd.Parameters.Add("p_ACPNO", OracleDbType.Double, 0, 0, ParameterDirection.Input);
                cmd.Parameters.Add("p_PTNO", OracleDbType.Varchar2, 9, strPANO, ParameterDirection.Input);
                cmd.Parameters.Add("p_INOUTCLS", OracleDbType.Varchar2, 1, "I", ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDFRDATE", OracleDbType.Varchar2, 8, strInDate.Replace("-", ""), ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDFRTIME", OracleDbType.Varchar2, 6, strInTime.Replace(":", ""), ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDENDDATE", OracleDbType.Varchar2, 8, "", ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDENDTIME", OracleDbType.Varchar2, 6, "", ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDDEPTCD", OracleDbType.Varchar2, 4, strDeptCode, ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDDRCD", OracleDbType.Varchar2, 6, strDrCd, ParameterDirection.Input);
                cmd.Parameters.Add("p_MIBICHECK", OracleDbType.Varchar2, 1, "0", ParameterDirection.Input);
                cmd.Parameters.Add("p_WRITEDATE", OracleDbType.Varchar2, 8, strSysDate.Replace("-", ""), ParameterDirection.Input);
                cmd.Parameters.Add("p_WRITETIME", OracleDbType.Varchar2, 6, strSysTime.Replace(":", ""), ParameterDirection.Input);
                cmd.Parameters.Add("p_UPDATENO", OracleDbType.Int32, 0, 1, ParameterDirection.Input);
                cmd.Parameters.Add("p_CHARTXML", OracleDbType.Varchar2, strXML.Length, strXML, ParameterDirection.Input);
                cmd.ExecuteNonQuery();

                cmd.Dispose();
                cmd = null;


                SQL = "";
                SQL = " UPDATE " + ComNum.DB_PMPA + "DIET_S_SEARCH SET ";
                SQL = SQL + ComNum.VBLF + " EMRNO = " + dblEmrNo;
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return rtnVar;
                }


                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVar = true;
                return rtnVar;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVar;
            }
        }

        bool SaveXML_2091(string argPano, string argInDate, string argUDATE)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            double dblEmrNo = 0;
            double dblEmrHisNo = 0;
            string strHead = "";
            string strChartX1 = "";
            string strChartX2 = "";
            string strXML = "";
            string strXMLCert = "";

            string strSabun = "";
            string strPANO = "";
            string strUDate = "";
            string strInDate = "";
            string strInTime = "";
            string strDeptCode = "";
            string strDrCd = "";
            string strTemp = "";

            string[] arrIt = new string[71];
            string[] arrTa = new string[6];
            string[] arrDt = new string[6];
            string[] arrIk = new string[6];

            string strROWID = "";

            string strChartDate = "";
            string strChartTime = "";

            string strSysDate = "";
            string strSysTime = "";

            bool rtnVar = false;

            for (i = 0; i < arrIt.Length; i++)
            {
                arrIt[i] = "";
            }

            for (i = 0; i < arrTa.Length; i++)
            {
                arrTa[i] = "";
            }

            for (i = 0; i < arrDt.Length; i++)
            {
                arrDt[i] = "";
            }

            for (i = 0; i < arrIk.Length; i++)
            {
                arrIk[i] = "";
            }

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " SELECT B.SDATE, A.DEPTCODE, A.DRCODE, A.SABUN, A.ROOMCODE, ";
                SQL = SQL + ComNum.VBLF + " A.SEX, A.AGE, A.DIAGNOSIS, A.DEPTCODE,";
                SQL = SQL + ComNum.VBLF + " A.WARDCODE, B.STATUS, A.IBW, B.CWEIGHT1,";
                SQL = SQL + ComNum.VBLF + " B.CWEIGHT2, B.CWEIGHT3, A.ALB, A.TLC,";
                SQL = SQL + ComNum.VBLF + " A.HB, A.TCHO, B.FAILA1, B.FAILA2,";
                SQL = SQL + ComNum.VBLF + " B.FAILA3, B.FAILB1, B.FAILB2, B.FAILB3,";
                SQL = SQL + ComNum.VBLF + " B.FAILB4, B.FAILB5, B.FAILB6,";
                SQL = SQL + ComNum.VBLF + " B.FAILD1, B.FAILD2, B.DIETA, B.DIETB,";
                SQL = SQL + ComNum.VBLF + " B.DIETF, B.DIETG,";
                SQL = SQL + ComNum.VBLF + " B. PLANA, B.PLANB1, B.PLANB2,";
                SQL = SQL + ComNum.VBLF + " B.PLANC, B.PLAND, B.PLANE, B.PLANF,";
                SQL = SQL + ComNum.VBLF + " B.USE, B.WARDCODE, B.IPDNO, B.WRTNO,";
                SQL = SQL + ComNum.VBLF + " B.EMRNO, B.ROWID";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_S_SEARCH A, " + ComNum.DB_PMPA + "DIET_S_MANAGER_NEW B";
                SQL = SQL + ComNum.VBLF + " WHERE A.INDATE = B.INDATE";
                SQL = SQL + ComNum.VBLF + "   AND A.PANO = B.PANO";
                SQL = SQL + ComNum.VBLF + "   AND A.SABUN IS NOT NULL";
                SQL = SQL + ComNum.VBLF + "   AND B.SABUN IS NOT NULL";
                SQL = SQL + ComNum.VBLF + "   AND B.INDATE = TO_DATE('" + argInDate + "', 'YYYY-MM-DD HH24:MI:SS')";
                SQL = SQL + ComNum.VBLF + "   AND B.PANO = '" + argPano + "'";
                SQL = SQL + ComNum.VBLF + "   AND B.UDATE = TO_DATE('" + argUDATE + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + " ORDER BY B.UDATE ASC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }

                if (dt.Rows.Count > 0)
                {
                    strPANO = argPano;
                    strInDate = VB.Left(argInDate, 10);
                    strInTime = VB.Mid(argInDate, 11, argInDate.Length);
                    strUDate = argUDATE;
                    strDeptCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    strDrCd = dt.Rows[0]["DRCODE"].ToString().Trim();
                    strSabun = Read_Sabun(dt.Rows[0]["SABUN"].ToString().Trim().PadLeft(5, '0'));
                    strROWID = dt.Rows[0]["ROWID"].ToString().Trim();

                    arrIt[1] = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    arrIt[2] = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    arrIt[3] = dt.Rows[0]["SEX"].ToString().Trim() == "M" ? "남" : "여";
                    arrIt[4] = dt.Rows[0]["AGE"].ToString().Trim();
                    arrIt[5] = dt.Rows[0]["DIAGNOSIS"].ToString().Trim();

                    arrIt[5] = arrIt[5].Replace("&", " and ");
                    arrIt[5] = arrIt[5].Replace("<", "〈");
                    arrIt[5] = arrIt[5].Replace(">", "〉");

                    strTemp = dt.Rows[0]["STATUS"].ToString().Trim();

                    switch (strTemp)
                    {
                        case "0":
                            strTemp = "Adaquate";
                            break;
                        case "1-type":
                            strTemp = "Marasmus-type";
                            break;
                        case "2":
                            strTemp = "Kwashiorkor";
                            break;
                        case "3":
                            strTemp = "Mild PCM";
                            break;
                        case "4":
                            strTemp = "Moderate PCM";
                            break;
                        case "5":
                            strTemp = "Severe PCM";
                            break;
                        case "6":
                            strTemp = "Overweight";
                            break;
                        case "7":
                            strTemp = "Obesity";
                            break;
                    }

                    arrIt[6] = strTemp;
                    arrIt[7] = dt.Rows[0]["IBW"].ToString().Trim();
                    arrIt[8] = Set_Status("IBW", arrIt[7]);

                    arrIt[11] = dt.Rows[0]["ALB"].ToString().Trim();
                    arrIt[12] = Set_Status("ALB", arrIt[11]);

                    arrIt[15] = dt.Rows[0]["TLC"].ToString().Trim();
                    arrIt[16] = Set_Status("TLC", arrIt[15]);

                    arrIt[17] = Set_Status3("1", dt.Rows[0]["CWEIGHT1"].ToString().Trim());
                    arrIt[18] = Set_Status3("2", dt.Rows[0]["CWEIGHT2"].ToString().Trim());
                    arrIt[19] = Set_Status3("3", dt.Rows[0]["CWEIGHT3"].ToString().Trim());

                    arrIt[9] = dt.Rows[0]["HB"].ToString().Trim();
                    arrIt[10] = Set_Status("HB", arrIt[9], dt.Rows[0]["SEX"].ToString().Trim());

                    arrIt[13] = dt.Rows[0]["TCHO"].ToString().Trim();
                    arrIt[14] = Set_Status("TCHO", arrIt[13]);

                    arrIt[20] = Set_Status2("1", dt.Rows[0]["FAILA1"].ToString().Trim());
                    arrIt[21] = Set_Status2("1", dt.Rows[0]["FAILA2"].ToString().Trim());
                    arrIt[22] = Set_Status2("1", dt.Rows[0]["FAILA3"].ToString().Trim());
                    arrIt[23] = Set_Status2("2", dt.Rows[0]["FAILB1"].ToString().Trim());
                    arrIt[27] = Set_Status2("2", dt.Rows[0]["FAILB2"].ToString().Trim());
                    arrIt[29] = Set_Status2("2", dt.Rows[0]["FAILB3"].ToString().Trim());
                    arrIt[28] = Set_Status2("2", dt.Rows[0]["FAILB4"].ToString().Trim());
                    arrIt[26] = Set_Status2("2", dt.Rows[0]["FAILB5"].ToString().Trim());
                    arrIt[24] = Set_Status2("2", dt.Rows[0]["FAILB6"].ToString().Trim());

                    arrIt[25] = Set_Status2("3", dt.Rows[0]["FAILD1"].ToString().Trim());
                    arrIt[30] = Set_Status2("3", dt.Rows[0]["FAILD2"].ToString().Trim());

                    arrIt[31] = dt.Rows[0]["DIETA"].ToString().Trim();
                    arrIt[32] = dt.Rows[0]["DIETB"].ToString().Trim();
                    arrIt[66] = dt.Rows[0]["DIETF"].ToString().Trim();
                    arrIt[67] = dt.Rows[0]["DIETG"].ToString().Trim();
                    arrIk[1] = dt.Rows[0]["PLANA"].ToString().Trim() == "1" ? "true" : "false";
                    arrTa[1] = dt.Rows[0]["PLANB1"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["PLANB2"].ToString().Trim();
                    arrIk[2] = dt.Rows[0]["PLANC"].ToString().Trim() == "1" ? "true" : "false";
                    arrIk[3] = dt.Rows[0]["PLAND"].ToString().Trim() == "1" ? "true" : "false";
                    arrIk[4] = dt.Rows[0]["PLANE"].ToString().Trim() == "1" ? "true" : "false";
                    arrTa[2] = dt.Rows[0]["PLANF"].ToString().Trim();
                    arrTa[2] = arrTa[2].Replace("&", " 그리고 ");
                    arrDt[1] = dt.Rows[0]["SDATE"].ToString().Trim();

                    dblEmrNo = VB.Val(dt.Rows[0]["EMRNO"].ToString().Trim());
                }
                else
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    rtnVar = false;
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                dt.Dispose();
                dt = null;


                if (dblEmrNo != 0)
                {
                    //기존차트를 변경할 경우 : 백업 테이블로 백업을 하고 신규 data를 입력한다
                    //KOSMOS_EMR.EMRXMLHISTORY_HISTORYNO_SEQ

                    dblEmrHisNo = ComQuery.GetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "EMRXMLHISNO");

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_EMR + "EMRXMLHISTORY";
                    SQL = SQL + ComNum.VBLF + "      (HISTORYNO, EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                    SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,HISTORYWRITEDATE,HISTORYWRITETIME,DELUSEID,CERTNO)";
                    SQL = SQL + ComNum.VBLF + " SELECT  " + dblEmrHisNo + ",";
                    SQL = SQL + ComNum.VBLF + "      EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                    SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,";
                    SQL = SQL + ComNum.VBLF + "      '" + ComQuery.CurrentDateTime(clsDB.DbCon, "D").Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "      '" + ComQuery.CurrentDateTime(clsDB.DbCon, "T").Trim() + "', '" + clsType.User.Sabun + "',CERTNO";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRXML";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + dblEmrNo;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return rtnVar;
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " DELETE FROM " + ComNum.DB_EMR + "EMRXML";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + dblEmrNo;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return rtnVar;
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " DELETE FROM " + ComNum.DB_EMR + "EMRXMLMST";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + dblEmrNo;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return rtnVar;
                    }
                }

                strXML = "";

                strHead = "<?xml version=" + VB.Chr(34) + "1.0" + VB.Chr(34) + " encoding=" + VB.Chr(34) + "UTF-8" + VB.Chr(34) + "?>";
                strChartX1 = "<chart>";
                strChartX2 = "</chart>";

                strXML = strHead + strChartX1;

                strXML = strXML + MakeXML_Entity("IT", "it1", "진료과", arrIt[1]);
                strXML = strXML + MakeXML_Entity("IT", "it2", "병실", arrIt[2]);
                strXML = strXML + MakeXML_Entity("IT", "it3", "성별", arrIt[3]);
                strXML = strXML + MakeXML_Entity("IT", "it4", "나이", arrIt[4]);
                strXML = strXML + MakeXML_Entity("IT", "it5", "진단명", arrIt[5]);
                strXML = strXML + MakeXML_Entity("IT", "it6", "영양상태평가", arrIt[6]);
                strXML = strXML + MakeXML_Entity("IT", "it7", "%IBW", arrIt[7]);
                strXML = strXML + MakeXML_Entity("IT", "it8", "%IBW불량정도", arrIt[8]);
                strXML = strXML + MakeXML_Entity("IT", "it11", "ALB", arrIt[11]);
                strXML = strXML + MakeXML_Entity("IT", "it12", "ALB불량정도", arrIt[12]);
                strXML = strXML + MakeXML_Entity("IT", "it15", "TLC", arrIt[15]);
                strXML = strXML + MakeXML_Entity("IT", "it16", "TLC불량정도", arrIt[16]);

                strXML = strXML + MakeXML_Entity("IT", "it17", "체중감소_1개월", arrIt[17]);
                strXML = strXML + MakeXML_Entity("IT", "it18", "체중감소_3개월", arrIt[18]);
                strXML = strXML + MakeXML_Entity("IT", "it19", "체중감소_6개월", arrIt[19]);

                strXML = strXML + MakeXML_Entity("IT", "it9", "HB", arrIt[9]);
                strXML = strXML + MakeXML_Entity("IT", "it10", "HB불량정도", arrIt[10]);
                strXML = strXML + MakeXML_Entity("IT", "it13", "T-chol", arrIt[13]);
                strXML = strXML + MakeXML_Entity("IT", "it14", "T-chol불량정도", arrIt[14]);

                strXML = strXML + MakeXML_Entity("IT", "it20", "섭취상태_밥", arrIt[20]);
                strXML = strXML + MakeXML_Entity("IT", "it21", "섭취상태_죽", arrIt[21]);
                strXML = strXML + MakeXML_Entity("IT", "it22", "섭취상태_미음", arrIt[22]);

                strXML = strXML + MakeXML_Entity("IT", "it23", "식사문제_메스꺼움", arrIt[23]);
                strXML = strXML + MakeXML_Entity("IT", "it27", "식사문제_구토", arrIt[27]);
                strXML = strXML + MakeXML_Entity("IT", "it29", "식사문제_설사", arrIt[29]);
                strXML = strXML + MakeXML_Entity("IT", "it28", "식사문제_변비", arrIt[28]);
                strXML = strXML + MakeXML_Entity("IT", "it26", "식사문제_연하곤란", arrIt[26]);
                strXML = strXML + MakeXML_Entity("IT", "it24", "식사문제_저작곤란", arrIt[24]);

                strXML = strXML + MakeXML_Entity("IT", "it31", "열량_Kcal/일", arrIt[31]);
                strXML = strXML + MakeXML_Entity("IT", "it32", "열량_kcal/kg", arrIt[32]);
                strXML = strXML + MakeXML_Entity("IT", "it66", "단백질_g/일", arrIt[66]);
                strXML = strXML + MakeXML_Entity("IT", "it67", "단잭질_g/kg", arrIt[67]);

                strXML = strXML + MakeXML_Entity("IT", "it25", "부종", arrIt[25]);
                strXML = strXML + MakeXML_Entity("IT", "it30", "복수", arrIt[30]);

                strXML = strXML + MakeXML_Entity("IK", "ik1", "영양상담의뢰서요망", arrIk[1]);
                strXML = strXML + MakeXML_Entity("TA", "ta1", "식사변경요망", arrTa[1]);
                strXML = strXML + MakeXML_Entity("IK", "ik2", "Oral", arrIk[2]);
                strXML = strXML + MakeXML_Entity("IK", "ik3", "TF", arrIk[3]);
                strXML = strXML + MakeXML_Entity("IK", "ik4", "TPN", arrIk[4]);

                strXML = strXML + MakeXML_Entity("TA", "ta2", "Foolow-Up", arrTa[2]);
                strXML = strXML + MakeXML_Entity("DT", "dt1", "Date", arrDt[1]);


                //==========================================================
                //지표 설명

                strXML = strXML + MakeXML_Entity("IT", "it47", "지표", "%IBW");
                strXML = strXML + MakeXML_Entity("IT", "it51", "지표", "Alb(g/dl)");
                strXML = strXML + MakeXML_Entity("IT", "it50", "지표", "TLC(cell/mm3)");
                strXML = strXML + MakeXML_Entity("IT", "it49", "지표", "Hb(g/dl)");
                strXML = strXML + MakeXML_Entity("IT", "it48", "지표", "T-CHO(mg/dl)");

                strXML = strXML + MakeXML_Entity("IT", "it59", "지표", "Severe");
                strXML = strXML + MakeXML_Entity("IT", "it60", "지표", "〈70%");
                strXML = strXML + MakeXML_Entity("IT", "it61", "지표", "〈2.3");
                strXML = strXML + MakeXML_Entity("IT", "it62", "지표", "〈800");
                strXML = strXML + MakeXML_Entity("IT", "it63", "지표", "〈8");
                strXML = strXML + MakeXML_Entity("IT", "it64", "지표", "〈9");
                strXML = strXML + MakeXML_Entity("IT", "it65", "지표", "〉300");

                strXML = strXML + MakeXML_Entity("IT", "it40", "지표", "정상");
                strXML = strXML + MakeXML_Entity("IT", "it41", "지표", "90~110%");
                strXML = strXML + MakeXML_Entity("IT", "it42", "지표", "3.1~5.5");
                strXML = strXML + MakeXML_Entity("IT", "it43", "지표", "〉1500");
                strXML = strXML + MakeXML_Entity("IT", "it44", "지표", "12〉=");
                strXML = strXML + MakeXML_Entity("IT", "it45", "지표", "14〉=");
                strXML = strXML + MakeXML_Entity("IT", "it46", "지표", "〈200");

                strXML = strXML + MakeXML_Entity("IT", "it33", "지표", "Mild");
                strXML = strXML + MakeXML_Entity("IT", "it34", "지표", "80~90%");
                strXML = strXML + MakeXML_Entity("IT", "it35", "지표", "2.8~3.0");
                strXML = strXML + MakeXML_Entity("IT", "it36", "지표", "1200~1400");
                strXML = strXML + MakeXML_Entity("IT", "it37", "지표", "10~11.9");
                strXML = strXML + MakeXML_Entity("IT", "it38", "지표", "12~13.9");
                strXML = strXML + MakeXML_Entity("IT", "it39", "지표", "200~239");

                strXML = strXML + MakeXML_Entity("IT", "it52", "지표", "Moderate");
                strXML = strXML + MakeXML_Entity("IT", "it53", "지표", "70~79%");
                strXML = strXML + MakeXML_Entity("IT", "it54", "지표", "2.3~2.7");
                strXML = strXML + MakeXML_Entity("IT", "it55", "지표", "800~1200");
                strXML = strXML + MakeXML_Entity("IT", "it56", "지표", "8~9.9");
                strXML = strXML + MakeXML_Entity("IT", "it57", "지표", "9~11.9");
                strXML = strXML + MakeXML_Entity("IT", "it58", "지표", "240~299");

                strXML = strXML + strChartX2;
                strXMLCert = strXML;



                SQL = "";
                SQL = "SELECT " + ComNum.DB_EMR + "GetEmrXmlNo() FunSeqNo FROM Dual";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }

                if (dt.Rows.Count > 0)
                {
                    dblEmrNo = VB.Val(dt.Rows[0]["FunSeqNo"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;


                SQL = "";
                SQL = " SELECT TO_CHAR(SYSDATE,'YYYYMMDD') AS CURRENTDATE, ";
                SQL = SQL + ComNum.VBLF + "             TO_CHAR(SYSDATE,'HH24MISS') AS CURRENTTIME ";
                SQL = SQL + ComNum.VBLF + "    FROM DUAL ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }

                if (dt.Rows.Count > 0)
                {
                    strChartDate = dt.Rows[0]["CURRENTDATE"].ToString().Trim();
                    strChartTime = dt.Rows[0]["CURRENTTIME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;


                OracleCommand cmd = new OracleCommand();

                PsmhDb pDbCon = clsDB.DbCon;

                cmd.Connection = pDbCon.Con;
                cmd.CommandText = "KOSMOS_EMR.XMLINSRT3";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("p_EMRNO", OracleDbType.Double, 0, dblEmrNo, ParameterDirection.Input);
                cmd.Parameters.Add("p_FORMNO", OracleDbType.Double, 0, 2091, ParameterDirection.Input);
                cmd.Parameters.Add("p_USEID", OracleDbType.Varchar2, 8, strSabun, ParameterDirection.Input);
                cmd.Parameters.Add("p_CHARTDATE", OracleDbType.Varchar2, 8, strUDate.Replace("-", ""), ParameterDirection.Input);
                cmd.Parameters.Add("p_CHARTTIME", OracleDbType.Varchar2, 6, "120000", ParameterDirection.Input);
                cmd.Parameters.Add("p_ACPNO", OracleDbType.Double, 0, 0, ParameterDirection.Input);
                cmd.Parameters.Add("p_PTNO", OracleDbType.Varchar2, 9, strPANO, ParameterDirection.Input);
                cmd.Parameters.Add("p_INOUTCLS", OracleDbType.Varchar2, 1, "I", ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDFRDATE", OracleDbType.Varchar2, 8, strInDate.Replace("-", ""), ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDFRTIME", OracleDbType.Varchar2, 6, strInTime.Replace(":", ""), ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDENDDATE", OracleDbType.Varchar2, 8, "", ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDENDTIME", OracleDbType.Varchar2, 6, "", ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDDEPTCD", OracleDbType.Varchar2, 4, strDeptCode, ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDDRCD", OracleDbType.Varchar2, 6, strDrCd, ParameterDirection.Input);
                cmd.Parameters.Add("p_MIBICHECK", OracleDbType.Varchar2, 1, "0", ParameterDirection.Input);
                cmd.Parameters.Add("p_WRITEDATE", OracleDbType.Varchar2, 8, strSysDate.Replace("-", ""), ParameterDirection.Input);
                cmd.Parameters.Add("p_WRITETIME", OracleDbType.Varchar2, 6, strSysTime.Replace(":", ""), ParameterDirection.Input);
                cmd.Parameters.Add("p_UPDATENO", OracleDbType.Int32, 0, 1, ParameterDirection.Input);
                cmd.Parameters.Add("p_CHARTXML", OracleDbType.Varchar2, strXML.Length, strXML, ParameterDirection.Input);
                cmd.ExecuteNonQuery();

                cmd.Dispose();
                cmd = null;


                SQL = "";
                SQL = " UPDATE " + ComNum.DB_PMPA + "DIET_S_SEARCH SET ";
                SQL = SQL + ComNum.VBLF + " EMRNO = " + dblEmrNo;
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return rtnVar;
                }


                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVar = true;
                return rtnVar;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVar;
            }
        }

        string MakeXML_Entity(string argGubun, string argEntity, string argNamed, string argVal)
        {
            string strGUBUN2 = "";

            string strTagHead = "";
            string strTagVal = "";
            string strTagTail = "";

            string rtnVar = "";

            switch (argGubun.ToUpper())
            {
                case "IT":
                    strGUBUN2 = "inputText";
                    break;
                case "DT":
                    strGUBUN2 = "inputDate";
                    break;
                case "TA":
                    strGUBUN2 = "textArea";
                    break;
                case "IK":
                    strGUBUN2 = "inputCheck";
                    break;
                case "IR":
                    strGUBUN2 = "inputRadio";
                    break;
                default:
                    rtnVar = "";
                    return rtnVar;
            }

            strTagHead = "<" + argEntity + " type=" + VB.Chr(34) + strGUBUN2 + VB.Chr(34) + " label=" + VB.Chr(34) + argNamed + VB.Chr(34) + ">";
            strTagVal = argVal;
            strTagTail = "</" + argEntity + ">";
            rtnVar = strTagHead + strTagVal + strTagTail;

            return rtnVar;
        }

        string Read_Sabun(string argName)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string rtnVar = "";

            try
            {
                SQL = "";
                SQL = "SELECT SABUN ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_ADM.INSA_MST";
                SQL = SQL + ComNum.VBLF + " WHERE KORNAME = '" + argName + "' ";
                SQL = SQL + ComNum.VBLF + "   AND TOIDAY IS NULL ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                rtnVar = dt.Rows[0]["SABUN"].ToString().Trim();

                dt.Dispose();
                dt = null;

                return rtnVar;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVar;
            }
        }

        //LAB 결과 변환
        string Set_Status(string argGubun, string argValue, string argSEX = "")
        {
            string rtnVar = "";

            switch (argGubun)
            {
                case "IBW":
                    if (VB.Val(argValue) >= 90 && VB.Val(argValue) <= 110)
                    {
                        rtnVar = "정상";
                    }
                    else if (VB.Val(argValue) >= 80 && VB.Val(argValue) <= 89)
                    {
                        rtnVar = "Mild";
                    }
                    else if (VB.Val(argValue) >= 70 && VB.Val(argValue) <= 79)
                    {
                        rtnVar = "Moderate";
                    }
                    else if (VB.Val(argValue) < 70)
                    {
                        rtnVar = "Severe";
                    }
                    break;
                case "ALB":
                    if (VB.Val(argValue) >= 3.1 && VB.Val(argValue) <= 5.5)
                    {
                        rtnVar = "정상";
                    }
                    else if (VB.Val(argValue) >= 2.8 && VB.Val(argValue) <= 3)
                    {
                        rtnVar = "Mild";
                    }
                    else if (VB.Val(argValue) >= 2.3 && VB.Val(argValue) <= 2.7)
                    {
                        rtnVar = "Moderate";
                    }
                    else if (VB.Val(argValue) < 2.3)
                    {
                        rtnVar = "Severe";
                    }
                    break;
                case "TLC":
                    if (VB.Val(argValue) > 1.5)
                    {
                        rtnVar = "정상";
                    }
                    else if (VB.Val(argValue) >= 1.2 && VB.Val(argValue) <= 1.5)
                    {
                        rtnVar = "Mild";
                    }
                    else if (VB.Val(argValue) >= 0.8 && VB.Val(argValue) <= 1.2)
                    {
                        rtnVar = "Moderate";
                    }
                    else if (VB.Val(argValue) < 0.8)
                    {
                        rtnVar = "Severe";
                    }
                    break;
                case "HB":
                    if (argSEX == "F")
                    {
                        if (VB.Val(argValue) >= 14)
                        {
                            rtnVar = "정상";
                        }
                        else if (VB.Val(argValue) >= 12 && VB.Val(argValue) <= 13.9)
                        {
                            rtnVar = "Mild";
                        }
                        else if (VB.Val(argValue) >= 9 && VB.Val(argValue) <= 11.9)
                        {
                            rtnVar = "Moderate";
                        }
                        else if (VB.Val(argValue) < 9)
                        {
                            rtnVar = "Severe";
                        }
                    }
                    else if (argSEX == "M")
                    {
                        if (VB.Val(argValue) >= 12)
                        {
                            rtnVar = "정상";
                        }
                        else if (VB.Val(argValue) >= 10 && VB.Val(argValue) <= 11.9)
                        {
                            rtnVar = "Mild";
                        }
                        else if (VB.Val(argValue) >= 8 && VB.Val(argValue) <= 9.9)
                        {
                            rtnVar = "Moderate";
                        }
                        else if (VB.Val(argValue) < 8)
                        {
                            rtnVar = "Severe";
                        }
                    }
                    break;
                case "TCHO":
                    if (VB.Val(argValue) < 200)
                    {
                        rtnVar = "정상";
                    }
                    else if (VB.Val(argValue) >= 200 && VB.Val(argValue) <= 239)
                    {
                        rtnVar = "Mild";
                    }
                    else if (VB.Val(argValue) >= 240 && VB.Val(argValue) <= 299)
                    {
                        rtnVar = "Moderate";
                    }
                    else if (VB.Val(argValue) > 300)
                    {
                        rtnVar = "Severe";
                    }
                    break;
            }

            return rtnVar;
        }

        //식욕, 식사시 문제
        string Set_Status2(string argGubun, string argValue)
        {
            string rtnVar = "";

            switch (argGubun)
            {
                case "1":
                    switch (argValue)
                    {
                        case "":
                        case "0":
                            rtnVar = "양호,변화없음";
                            break;
                        case "1":
                            rtnVar = "약간감소(〈2주)";
                            break;
                        case "2":
                            rtnVar = "불량(〉2주)";
                            break;
                        case "3":
                            rtnVar = "불량,계속감소";
                            break;
                    }
                    break;
                case "2":
                    switch (argValue)
                    {
                        case "":
                        case "0":
                            rtnVar = "없음";
                            break;
                        case "1":
                            rtnVar = "간간히 약간";
                            break;
                        case "2":
                            rtnVar = "가끔(〈=2주)";
                            break;
                        case "3":
                            rtnVar = "자주,매일(〉2주)";
                            break;
                    }
                    break;
                case "3":
                    switch (argValue)
                    {
                        case "":
                        case "0":
                            rtnVar = "없음";
                            break;
                        case "1":
                            rtnVar = "약간";
                            break;
                        case "2":
                            rtnVar = "보통";
                            break;
                        case "3":
                            rtnVar = "심함";
                            break;
                    }
                    break;
            }

            return rtnVar;
        }

        //체중감소
        string Set_Status3(string argGubun, string argValue)
        {
            string rtnVar = "";

            switch (argGubun)
            {
                case "1":
                    switch (argValue)
                    {
                        case "":
                        case "0":
                            rtnVar = "양호,변화없음";
                            break;
                        case "1":
                            rtnVar = "약간감소(〈2주)";
                            break;
                        case "2":
                            rtnVar = "불량(〉2주)";
                            break;
                        case "3":
                            rtnVar = "불량,계속감소";
                            break;
                    }
                    break;
                case "2":
                    switch (argValue)
                    {
                        case "":
                        case "0":
                            rtnVar = "없음";
                            break;
                        case "1":
                            rtnVar = "간간히 약간";
                            break;
                        case "2":
                            rtnVar = "가끔(〈=2주)";
                            break;
                        case "3":
                            rtnVar = "자주,매일(〉2주)";
                            break;
                    }
                    break;
                case "3":
                    switch (argValue)
                    {
                        case "":
                        case "0":
                            rtnVar = "없음";
                            break;
                        case "1":
                            rtnVar = "약간";
                            break;
                        case "2":
                            rtnVar = "보통";
                            break;
                        case "3":
                            rtnVar = "심함";
                            break;
                    }
                    break;
            }

            return rtnVar;
        }
    }
}
