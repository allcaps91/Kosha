using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

/// <summary>
/// Description : 전원 사유서 작성
/// Author : 김형범
/// Create Date : 2017.06.27
/// </summary>
/// <history>
/// FormInfo_History 함수없음, FrmHOSPCODE form 없음
/// </history>
namespace ComLibB
{
    /// <summary> 전원 사유서 작성 </summary>
    public partial class frmJeonReason : Form
    {
        string GstrHospCode = ""; //병원코드
        string GstrHospName = ""; //병원이름
        string GstrHospGubn = ""; //병원구분

        /// <summary> 전원 사유서 작성 </summary>
        public frmJeonReason()
        {
            InitializeComponent();
        }

        public frmJeonReason(string strHospCode, string strHospName, string strHospGubn)
        {
            InitializeComponent();

            GstrHospCode = strHospCode;
            GstrHospName = strHospName;
            GstrHospGubn = strHospGubn;
        }

        //TODO: registry.bas/FormInfo_History
        void frmJeonReason_Load(object sender, EventArgs e)
        {

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
            //Call FormInfo_History(Me.Name, Me.Caption)  TODO: registry.bas/FormInfo_History

            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;


            ssView_Sheet1.RowCount = 0;
            ScreenClear();

            try
            {

                SQL = "";
                SQL = "SELECT WardCode, WardName  ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_WARD ";
                SQL = SQL + ComNum.VBLF + " WHERE WARDCODE NOT IN ('IU','NP','2W','IQ','ER') ";
                SQL = SQL + ComNum.VBLF + " ORDER BY WardCode ";

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

                cboWard.Items.Clear();
                cboWard.Items.Add("전체");

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboWard.Items.Add(dt.Rows[i]["WardCode"].ToString().Trim());
                }

                cboWard.Items.Add("SICU");
                cboWard.Items.Add("MICU");
                cboWard.Items.Add("HD");
                cboWard.Items.Add("ER");
                cboWard.Items.Add("RA");
                cboWard.Items.Add("TTE"); //심장초음파

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }

            cboWard.SelectedIndex = 0;

            cboGubun.Items.Clear();
            cboGubun.Items.Add("1.퇴원일 기준");
            cboGubun.Items.Add("2.입원일 기준");
            cboGubun.Items.Add("3.작성일 기준");
            cboGubun.Items.Add("4.전원일 기준");

            cboGubun.SelectedIndex = 0;

            Search();
        }

        struct fPT
        {
            public static string strPano = "";
            public static string strSName = "";
            public static string strSex = "";
            public static string strAge = "";
            public static string strDeptCode = "";
            public static string strROOMCODE = "";
            public static string strInDate = "";
            public static string strOutDate = "";
            public static string strWARDCODE = "";
            public static string strSDATE = "";
            public static string strHOSCODE = "";
            public static string strROWID = "";
        }

        void ScreenClear()
        {
            txtPano.Text = ""; //등록번호
            txtSName.Text = ""; //성명
            txtSex.Text = ""; //성별/나이
            txtDept.Text = ""; //진료과
            dtpIpwon.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")); //입원일자
            txtWard.Text = ""; //병동
            txtRoom.Text = ""; //병실
            txtJindan1.Text = ""; //진단명1
            txtJindan2.Text = ""; //진단명2
            txtJindan3.Text = ""; //진단명3
            dtpTDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")); //전원일자
            txtHospital.Text = ""; //전원병원
            txtHospital.Text = ""; //전원병원코드
            dtpDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));//작성일자
            txtRowid.Text = ""; //ROWID

            //전원사유(환자) 콤보박스
            chkSayuP0.Checked = false;
            chkSayuP1.Checked = false;
            chkSayuP2.Checked = false;
            chkSayuP3.Checked = false;
            chkSayuP4.Checked = false;
            chkSayuP5.Checked = false;
            txtSayuPt.Text = ""; //전원사유 기타 텍스트

            //전원사유(병원) 콤보박스
            chkSayuH0.Checked = false;
            chkSayuH1.Checked = false;
            chkSayuH2.Checked = false;
            chkSayuH3.Checked = false;
            chkSayuH4.Checked = false;
            chkSayuH4.Checked = false;
            chkSayuH5.Checked = false;
            chkSayuH6.Checked = false;
            chkSayuH7.Checked = false;
            chkSayuH8.Checked = false;
            txtSayuH.Text = ""; //전원사유(병원) 기타 텍스트

            //응급검사 /처치불가 콤보박스
            chkSayuHA0.Checked = false;
            chkSayuHA1.Checked = false;
            chkSayuHA2.Checked = false;
            chkSayuHA3.Checked = false;
            chkSayuHA4.Checked = false;
            chkSayuHA5.Checked = false;
            chkSayuHA6.Checked = false;
            chkSayuHA7.Checked = false;
            txtSayuHA.Text = ""; //응급검사/처치불가 기타 텍스트

            //전문응급의료 콤보박스
            chkSayuHB0.Checked = false;
            chkSayuHB1.Checked = false;
            chkSayuHB2.Checked = false;
            chkSayuHB3.Checked = false;
            txtSayuHB.Text = ""; //전문응급의료 기타 텍스트
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        void Search()
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}

            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ClearfPT();
            ScreenClear();

            ssView_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "    SELECT A.PANO, A.SNAME, A.SEX, A.AGE, ";
                SQL = SQL + ComNum.VBLF + " A.DEPTCODE, A.ROOMCODE, TO_CHAR(A.INDATE, 'YYYY-MM-DD') INDATE, ";
                SQL = SQL + ComNum.VBLF + " A.OUTDATE , A.WardCode, B.SDATE, B.THOSCODE, B.ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A, " + ComNum.DB_PMPA + "NUR_QI_TRANSFOR B ";
                SQL = SQL + ComNum.VBLF + " WHERE A.PANO = B.PANO(+) ";
                SQL = SQL + ComNum.VBLF + "      AND TRUNC(A.INDATE) = B.INDATE(+) ";

                if (txtPtno.Text.Trim() != "")
                {
                    SQL = SQL + ComNum.VBLF + "     AND a.PANO = '" + txtPtno.Text.Trim() + "' ";
                }

                SQL = SQL + ComNum.VBLF + "      AND A.GBSTS <> '9'";

                switch (VB.Left(cboGubun.Text, 1))
                {
                    case "1":
                        SQL = SQL + ComNum.VBLF + "      AND A.OUTDATE >= TO_DATE('" + dtpSDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "      AND A.OUTDATE <= TO_DATE('" + dtpEDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        break;
                    case "2":
                        SQL = SQL + ComNum.VBLF + "      AND A.INDATE >= TO_DATE('" + dtpSDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "      AND A.INDATE <= TO_DATE('" + dtpEDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        break;
                    case "3":
                        SQL = SQL + ComNum.VBLF + "      AND B.SDATE >= TO_DATE('" + dtpSDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "      AND B.SDATE <= TO_DATE('" + dtpEDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        break;
                    case "4":
                        SQL = SQL + ComNum.VBLF + "      AND B.TDATE >= TO_DATE('" + dtpSDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "      AND B.TDATE <= TO_DATE('" + dtpEDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        break;
                }

                SQL = SQL + ComNum.VBLF + "       AND DELDATE IS NULL ";

                if (rdoSort0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "      ORDER BY A.WARDCODE, A.ROOMCODE, A. SNAME ";
                }
                else if (rdoSort1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "      ORDER BY A.SNAME, A.WARDCODE, A.ROOMCODE ";
                }
                else if (rdoSort2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "      ORDER BY A.DEPTCODE, A.SNAME, A.PANO ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;
                prbView.Minimum = 0;

                if (dt.Rows.Count > 0)
                {
                    prbView.Maximum = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SEX"].ToString().Trim() + "/" + dt.Rows[i]["AGE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = VB.Format(Convert.ToDateTime(dt.Rows[i]["OUTDATE"].ToString().Trim()), "yyyy-MM-dd");
                        ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["SDATE"].ToString().Trim();

                        if (ssView_Sheet1.Cells[i, 8].Text != "")
                        {
                            ssView_Sheet1.ColumnHeader.Rows[0].BackColor = Color.FromArgb(255, 255, 176);
                        }
                        else
                        {
                            ssView_Sheet1.ColumnHeader.Rows[0].BackColor = Color.FromArgb(255, 255, 255);
                        }

                        ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["THOSCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                        prbView.Value = i + 1;
                    }
                }

                txtPtno.Text = "";
                lblSname.Text = "";

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                txtPtno.Text = "";
                lblSname.Text = "";

                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }


        }

        void ClearfPT()
        {
            fPT.strPano = "";
            fPT.strSName = "";
            fPT.strSex = "";
            fPT.strAge = "";
            fPT.strDeptCode = "";
            fPT.strROOMCODE = "";
            fPT.strInDate = "";
            fPT.strOutDate = "";
            fPT.strWARDCODE = "";
            fPT.strSDATE = "";
            fPT.strHOSCODE = "";
            fPT.strROWID = "";
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (CertVal() != "OK")
            {
                ComFunc.MsgBox(CertVal() + " 항목을 입력하십시요.", "확인");
                return;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string chkSP0 = "";
            string chkSP1 = "";
            string chkSP2 = "";
            string chkSP3 = "";
            string chkSP4 = "";

            string chkSHA0 = "";
            string chkSHA1 = "";
            string chkSHA2 = "";
            string chkSHA3 = "";
            string chkSHA4 = "";
            string chkSHA5 = "";
            string chkSHA6 = "";

            string chkSH0 = "";
            string chkSH2 = "";
            string chkSH3 = "";
            string chkSH5 = "";
            string chkSH6 = "";
            string chkSH7 = "";

            string chkSHB0 = "";
            string chkSHB1 = "";
            string chkSHB2 = "";

            chkSP0 = (chkSayuP0.Checked == true) ? "1" : "0";
            chkSP1 = (chkSayuP1.Checked == true) ? "1" : "0";
            chkSP2 = (chkSayuP2.Checked == true) ? "1" : "0";
            chkSP3 = (chkSayuP3.Checked == true) ? "1" : "0";
            chkSP4 = (chkSayuP4.Checked == true) ? "1" : "0";

            chkSHA0 = (chkSayuHA0.Checked == true) ? "1" : "0";
            chkSHA1 = (chkSayuHA1.Checked == true) ? "1" : "0";
            chkSHA2 = (chkSayuHA2.Checked == true) ? "1" : "0";
            chkSHA3 = (chkSayuHA3.Checked == true) ? "1" : "0";
            chkSHA4 = (chkSayuHA4.Checked == true) ? "1" : "0";
            chkSHA5 = (chkSayuHA5.Checked == true) ? "1" : "0";
            chkSHA6 = (chkSayuHA6.Checked == true) ? "1" : "0";

            chkSH0 = (chkSayuH0.Checked == true) ? "1" : "0";
            chkSH2 = (chkSayuH2.Checked == true) ? "1" : "0";
            chkSH3 = (chkSayuH3.Checked == true) ? "1" : "0";
            chkSH5 = (chkSayuH5.Checked == true) ? "1" : "0";
            chkSH6 = (chkSayuH6.Checked == true) ? "1" : "0";
            chkSH7 = (chkSayuH7.Checked == true) ? "1" : "0";

            chkSHB0 = (chkSayuHB0.Checked == true) ? "1" : "0";
            chkSHB1 = (chkSayuHB1.Checked == true) ? "1" : "0";
            chkSHB2 = (chkSayuHB2.Checked == true) ? "1" : "0";

            try
            {
                if (fPT.strROWID == "")
                {

                    SQL = "";
                    SQL = " INSERT INTO " + ComNum.DB_PMPA + "NUR_QI_TRANSFOR( ";
                    SQL = SQL + ComNum.VBLF + "  SDATE , PANO, SNAME, Sex, ";
                    SQL = SQL + ComNum.VBLF + "   Age , DEPTCODE, INDATE, WardCode, ";
                    SQL = SQL + ComNum.VBLF + "   ROOMCODE , DIAG1, DIAG2, DIAG3, ";
                    SQL = SQL + ComNum.VBLF + "   TDATE , THOSNAME, THOSCODE, SAYUP1, ";
                    SQL = SQL + ComNum.VBLF + "   SAYUP2 , SAYUP3, SAYUP4, SAYUP5, ";
                    SQL = SQL + ComNum.VBLF + "   SAYUPETC , SAYUH1, SAYUH2A, SAYUH2B, ";
                    SQL = SQL + ComNum.VBLF + "   SAYUH2C , SAYUH2D, SAYUH2E, SAYUH2F, ";
                    SQL = SQL + ComNum.VBLF + "   SAYUH2G , SAYUH2ETC, SAYUH3, SAYUH4, ";
                    SQL = SQL + ComNum.VBLF + "   SAYUH5A , SAYUH5B, SAYUH5C, SAYUH5ETC, ";
                    SQL = SQL + ComNum.VBLF + "   SAYUH6 , SAYUH7, SAYUH8, SAYUHETC) VALUES ( ";
                    SQL = SQL + ComNum.VBLF + "TO_DATE('" + VB.Left(Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).ToString(), 10) + "','YYYY-MM-DD'),'" + txtPano.Text.Trim() + "','" + txtSName.Text.Trim() + "','" + VB.Left(txtSex.Text.Trim(), 1) + "','" + VB.Mid(txtSex.Text.Trim(), 3, 6) + "'";
                    SQL = SQL + ComNum.VBLF + ",'" + txtDept.Text.Trim() + "',TO_DATE('" + dtpIpwon.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'),'" + txtWard.Text.Trim() + "','" + txtRoom.Text.Trim() + "'";
                    SQL = SQL + ComNum.VBLF + ",'" + txtJindan1.Text.Trim() + "','" + txtJindan2.Text.Trim() + "','" + txtJindan3.Text.Trim() + "',TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + ",'" + txtHospital.Text.Trim() + "','" + txtHospital1.Text.Trim() + "','" + chkSP0 + "','" + chkSP1 + "'";
                    SQL = SQL + ComNum.VBLF + ",'" + chkSP2 + "','" + chkSP3 + "','" + chkSP4 + "','" + txtSayuPt.Text.Trim() + "'";
                    SQL = SQL + ComNum.VBLF + ",'" + chkSH0 + "','" + chkSHA0 + "','" + chkSHA1 + "','" + chkSHA2 + "'";
                    SQL = SQL + ComNum.VBLF + ",'" + chkSHA3 + "','" + chkSHA4 + "','" + chkSHA5 + "','" + chkSHA6 + "'";
                    SQL = SQL + ComNum.VBLF + ",'" + txtSayuHA.Text.Trim() + "','" + chkSH2 + "','" + chkSH3 + "','" + chkSHB0 + "'";
                    SQL = SQL + ComNum.VBLF + ",'" + chkSHB1 + "','" + chkSHB2 + "','" + txtSayuHB.Text.Trim() + "','" + chkSH5 + "'";
                    SQL = SQL + ComNum.VBLF + ",'" + chkSH6 + "','" + chkSH7 + "','" + txtSayuH.Text.Trim() + "')";
                }
                else
                {

                    SQL = "";
                    SQL = " UPDATE " + ComNum.DB_PMPA + "NUR_QI_TRANSFOR SET ";
                    SQL = SQL + ComNum.VBLF + " DIAG1 = '" + txtJindan1.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "  DIAG2 = '" + txtJindan2.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "  DIAG3 = '" + txtJindan3.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "  TDATE = TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "  THOSNAME = '" + txtHospital.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "  THOSCODE = '" + txtHospital1.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "  SAYUP1 = '" + chkSP0 + "', ";
                    SQL = SQL + ComNum.VBLF + "  SAYUP2 = '" + chkSP1 + "', ";
                    SQL = SQL + ComNum.VBLF + "  SAYUP3 = '" + chkSP2 + "', ";
                    SQL = SQL + ComNum.VBLF + "  SAYUP4 = '" + chkSP3 + "', ";
                    SQL = SQL + ComNum.VBLF + "  SAYUP5 = '" + chkSP4 + "', ";
                    SQL = SQL + ComNum.VBLF + "  SAYUPETC = '" + txtSayuPt.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "  SAYUH1 = '" + chkSH0 + "', ";
                    SQL = SQL + ComNum.VBLF + "  SAYUH2A = '" + chkSHA0 + "', ";
                    SQL = SQL + ComNum.VBLF + "  SAYUH2B = '" + chkSHA1 + "', ";
                    SQL = SQL + ComNum.VBLF + "  SAYUH2C = '" + chkSHA2 + "', ";
                    SQL = SQL + ComNum.VBLF + "  SAYUH2D = '" + chkSHA3 + "', ";
                    SQL = SQL + ComNum.VBLF + "  SAYUH2E = '" + chkSHA4 + "', ";
                    SQL = SQL + ComNum.VBLF + "  SAYUH2F = '" + chkSHA5 + "', ";
                    SQL = SQL + ComNum.VBLF + "  SAYUH2G = '" + chkSHA6 + "', ";
                    SQL = SQL + ComNum.VBLF + "  SAYUH2ETC = '" + txtSayuHA.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "  SAYUH3 = '" + chkSH2 + "', ";
                    SQL = SQL + ComNum.VBLF + "  SAYUH4 = '" + chkSH3 + "', ";
                    SQL = SQL + ComNum.VBLF + "  SAYUH5A = '" + chkSHB0 + "', ";
                    SQL = SQL + ComNum.VBLF + "  SAYUH5B = '" + chkSHB1 + "', ";
                    SQL = SQL + ComNum.VBLF + "  SAYUH5C = '" + chkSHB2 + "', ";
                    SQL = SQL + ComNum.VBLF + "  SAYUH5ETC = '" + txtSayuHB.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "  SAYUH6 = '" + chkSH5 + "', ";
                    SQL = SQL + ComNum.VBLF + "  SAYUH7 = '" + chkSH6 + "', ";
                    SQL = SQL + ComNum.VBLF + "  SAYUH8 = '" + chkSH7 + "', ";
                    SQL = SQL + ComNum.VBLF + "  SAYUHETC = '" + txtSayuH.Text.Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "  WHERE ROWID = '" + txtRowid.Text.Trim() + "' ";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                else
                {
                    clsDB.setCommitTran(clsDB.DbCon);
                    ComFunc.MsgBox("저장 되었습니다.", "확인");
                    Search();
                }
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        string CertVal()
        {
            string strVal = "";

            if (txtPano.Text == "")
            {
                strVal = "'등록번호'";
                return strVal;
            }

            if (dtpTDate.Text == "")
            {
                strVal = "'전원일자'";
                return strVal;
            }

            if (txtHospital1.Text == "")
            {
                strVal = "'전원병원코드'";
                return strVal;
            }

            if (txtHospital.Text == "")
            {
                strVal = "'전원병원명'";
                return strVal;
            }

            strVal = "OK";
            return strVal;
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtRowid.Text == "")
            {
                return;
            }

            if (ComFunc.MsgBoxQ("해당 사유서를 삭제하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                SQL = "";
                SQL = " UPDATE " + ComNum.DB_PMPA + "NUR_QI_TRANSFOR SET DELDATE = TO_DATE('" + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + txtRowid.Text.Trim() + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                Search();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

        }

        void txtSayuHA_TextChanged(object sender, EventArgs e)
        {
            if (txtSayuHA.Text != "")
            {
                chkSayuHA7.Checked = true;
            }
            else
            {
                chkSayuHA7.Checked = false;
            }
        }

        void txtSayuHB_TextChanged(object sender, EventArgs e)
        {
            if (txtSayuHB.Text != "")
            {
                chkSayuHB3.Checked = true;
            }
            else
            {
                chkSayuHB3.Checked = false;
            }
        }

        void txtSayuPt_TextChanged(object sender, EventArgs e)
        {
            if (txtSayuPt.Text != "")
            {
                chkSayuP5.Checked = true;
            }
            else
            {
                chkSayuP5.Checked = false;
            }
        }

        void txtSayuH_TextChanged(object sender, EventArgs e)
        {
            if (txtSayuH.Text != "")
            {
                chkSayuH8.Checked = true;
            }
            else
            {
                chkSayuH8.Checked = false;
            }
        }

        //TODO: HOSPCODE form 생성X
        void btnHospital_Click(object sender, EventArgs e)
        {
            //FrmHOSPCODE frm = new FrmHOSPCODE(); TODO: HOSPCODE form 생성X
            //frm.Show();

            txtHospital1.Text = GstrHospCode;
            txtHospital.Text = GstrHospName;
            GstrHospCode = "";
            GstrHospName = "";
            GstrHospGubn = "";
        }

        int GetTrans(string strPano, string strSDATE, string strHOSCODE)
        {
            int intval = 0;
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL = " SELECT SDATE, PANO, SNAME, SEX,";
                SQL = SQL + ComNum.VBLF + "  AGE, DEPTCODE, INDATE, WARDCODE,";
                SQL = SQL + ComNum.VBLF + "  ROOMCODE, DIAG1, DIAG2, DIAG3,";
                SQL = SQL + ComNum.VBLF + "  TDATE, THOSNAME, THOSCODE, SAYUP1,";
                SQL = SQL + ComNum.VBLF + "  SAYUP2, SAYUP3, SAYUP4, SAYUP5,";
                SQL = SQL + ComNum.VBLF + "  SAYUPETC, SAYUH1, SAYUH2A, SAYUH2B,";
                SQL = SQL + ComNum.VBLF + "  SAYUH2C, SAYUH2D, SAYUH2E, SAYUH2F,";
                SQL = SQL + ComNum.VBLF + "  SAYUH2G, SAYUH2ETC, SAYUH3, SAYUH4,";
                SQL = SQL + ComNum.VBLF + "  SAYUH5A, SAYUH5B, SAYUH5C, SAYUH5ETC,";
                SQL = SQL + ComNum.VBLF + "  SAYUH6 , SAYUH7, SAYUH8, SAYUHETC,";
                SQL = SQL + ComNum.VBLF + "  SDATE, ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_QI_TRANSFOR";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND SDATE = TO_DATE('" + strSDATE + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND THOSCODE = '" + strHOSCODE + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return intval;
                }

                if (dt.Rows.Count > 0)
                {
                    txtPano.Text = dt.Rows[0]["PANO"].ToString().Trim();
                    txtSName.Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    txtSex.Text = dt.Rows[0]["SEX"].ToString().Trim() + "/" + dt.Rows[0]["AGE"].ToString().Trim();
                    txtDept.Text = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    dtpIpwon.Text = dt.Rows[0]["INDATE"].ToString().Trim();
                    txtWard.Text = dt.Rows[0]["WARDCODE"].ToString().Trim();
                    txtRoom.Text = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    txtJindan1.Text = dt.Rows[0]["DIAG1"].ToString().Trim();
                    txtJindan2.Text = dt.Rows[0]["DIAG2"].ToString().Trim();
                    txtJindan3.Text = dt.Rows[0]["DIAG3"].ToString().Trim();
                    dtpTDate.Text = dt.Rows[0]["TDATE"].ToString().Trim();
                    txtHospital.Text = dt.Rows[0]["THOSNAME"].ToString().Trim();
                    txtHospital1.Text = dt.Rows[0]["THOSCODE"].ToString().Trim();

                    //전원사유(환자)
                    chkSayuP0.Checked = (dt.Rows[0]["SAYUP1"].ToString().Trim() == "1" ? true : false);
                    chkSayuP1.Checked = (dt.Rows[0]["SAYUP2"].ToString().Trim() == "1" ? true : false);
                    chkSayuP2.Checked = (dt.Rows[0]["SAYUP3"].ToString().Trim() == "1" ? true : false);
                    chkSayuP3.Checked = (dt.Rows[0]["SAYUP4"].ToString().Trim() == "1" ? true : false);
                    chkSayuP4.Checked = (dt.Rows[0]["SAYUP5"].ToString().Trim() == "1" ? true : false);
                    txtSayuPt.Text = dt.Rows[0]["SAYUPETC"].ToString().Trim();
                    chkSayuP5.Checked = (dt.Rows[0]["SAYUPETC"].ToString().Trim() != null ? true : false);

                    //전원사유(병원
                    chkSayuH0.Checked = (dt.Rows[0]["SAYUH1"].ToString().Trim() == "1" ? true : false);
                    chkSayuHA0.Checked = (dt.Rows[0]["SAYUH2A"].ToString().Trim() == "1" ? true : false);
                    chkSayuHA1.Checked = (dt.Rows[0]["SAYUH2B"].ToString().Trim() == "1" ? true : false);
                    chkSayuHA2.Checked = (dt.Rows[0]["SAYUH2C"].ToString().Trim() == "1" ? true : false);
                    chkSayuHA3.Checked = (dt.Rows[0]["SAYUH2D"].ToString().Trim() == "1" ? true : false);
                    chkSayuHA4.Checked = (dt.Rows[0]["SAYUH2E"].ToString().Trim() == "1" ? true : false);
                    chkSayuHA5.Checked = (dt.Rows[0]["SAYUH2F"].ToString().Trim() == "1" ? true : false);
                    chkSayuHA6.Checked = (dt.Rows[0]["SAYUH2G"].ToString().Trim() == "1" ? true : false);
                    txtSayuHA.Text = dt.Rows[0]["SAYUH2ETC"].ToString().Trim();
                    chkSayuHA7.Checked = (dt.Rows[0]["SAYUH2ETC"].ToString().Trim() != null ? true : false);

                    for (i = 0; i < 8; i++)
                    {
                        if (chkSayuHA0.Checked == true)
                        {
                            chkSayuH1.Checked = true;
                            break;
                        }
                    }

                    chkSayuH2.Checked = (dt.Rows[0]["SAYUH3"].ToString().Trim() == "1" ? true : false);
                    chkSayuH3.Checked = (dt.Rows[0]["SAYUH4"].ToString().Trim() == "1" ? true : false);

                    chkSayuHB0.Checked = (dt.Rows[0]["SAYUH5A"].ToString().Trim() == "1" ? true : false);
                    chkSayuHB1.Checked = (dt.Rows[0]["SAYUH5B"].ToString().Trim() == "1" ? true : false);
                    chkSayuHB2.Checked = (dt.Rows[0]["SAYUH5C"].ToString().Trim() == "1" ? true : false);
                    txtSayuHB.Text = dt.Rows[0]["SAYUH5ETC"].ToString().Trim();
                    chkSayuHB3.Checked = (dt.Rows[0]["SAYUH5ETC"].ToString().Trim() != null ? true : false);

                    for (i = 0; i < 8; i++)
                    {
                        if (chkSayuHB0.Checked == true)
                        {
                            chkSayuH4.Checked = true;
                            break;
                        }
                    }

                    chkSayuH5.Checked = (dt.Rows[0]["SAYUH6"].ToString().Trim() == "1" ? true : false);
                    chkSayuH6.Checked = (dt.Rows[0]["SAYUH7"].ToString().Trim() == "1" ? true : false);
                    chkSayuH7.Checked = (dt.Rows[0]["SAYUH8"].ToString().Trim() == "1" ? true : false);
                    txtSayuH.Text = dt.Rows[0]["SAYUHETC"].ToString().Trim();
                    chkSayuH8.Checked = (dt.Rows[0]["SAYUHETC"].ToString().Trim() != null ? true : false);
                    dtpSDate.Text = dt.Rows[0]["SDATE"].ToString().Trim();
                    txtRowid.Text = dt.Rows[0]["ROWID"].ToString().Trim();

                    intval = 1;
                }
                else
                {
                    intval = 0;
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return intval;
            }


            return intval;
        }

        void txtPano_KeyDown(object sender, KeyEventArgs e)
        {

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (e.KeyCode == Keys.Enter)
            {
                txtPtno.Text = VB.Format(VB.Val(txtPtno.Text), "00000000");

                try
                {
                    SQL = "";
                    SQL = "SELECT SNAME FROM " + ComNum.DB_PMPA + "BAS_PATIENT WHERE PANO = '" + txtPtno.Text + "'";

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

                    lblSname.Text = dt.Rows[0]["SNAME"].ToString().Trim();

                    dt.Dispose();
                    dt = null;
                }
                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                }
            }
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0) { return; }

            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssView_Sheet1.Cells[e.Row, 0, e.Row, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            ClearfPT();

            fPT.strPano = ssView_Sheet1.Cells[e.Row, 0].Text.Trim();
            fPT.strSName = ssView_Sheet1.Cells[e.Row, 1].Text.Trim();
            fPT.strSex = ssView_Sheet1.Cells[e.Row, 2].Text.Trim();
            fPT.strAge = ssView_Sheet1.Cells[e.Row, 2].Text.Trim();
            fPT.strDeptCode = ssView_Sheet1.Cells[e.Row, 3].Text.Trim();
            fPT.strROOMCODE = ssView_Sheet1.Cells[e.Row, 4].Text.Trim();
            fPT.strInDate = ssView_Sheet1.Cells[e.Row, 5].Text.Trim();
            fPT.strOutDate = ssView_Sheet1.Cells[e.Row, 6].Text.Trim();
            fPT.strWARDCODE = ssView_Sheet1.Cells[e.Row, 7].Text.Trim();
            fPT.strSDATE = ssView_Sheet1.Cells[e.Row, 8].Text.Trim();
            fPT.strHOSCODE = ssView_Sheet1.Cells[e.Row, 9].Text.Trim();
            fPT.strROWID = ssView_Sheet1.Cells[e.Row, 10].Text.Trim();

            if (GetTrans(fPT.strPano, fPT.strSDATE, fPT.strHOSCODE) == 0)
            {
                ComFunc.MsgBox("신규등록입니다.", "확인");
                ScreenClear();
                txtPano.Text = fPT.strPano; //등록번호
                txtSName.Text = fPT.strSName; //성명
                txtSex.Text = fPT.strSex; //성별/나이
                txtDept.Text = fPT.strDeptCode; //진료과
                dtpIpwon.Text = fPT.strInDate; //입원일자
                txtWard.Text = fPT.strWARDCODE; //병동
                txtRoom.Text = fPT.strROOMCODE; //병실
                txtHospital.Text = fPT.strHOSCODE; //전원병원
                txtHospital1.Text = ""; //전원병원코드
                dtpSDate.Text = fPT.strSDATE; //작성일자
                txtRowid.Text = fPT.strROWID; //ROWID
            }
        }
    }
}
