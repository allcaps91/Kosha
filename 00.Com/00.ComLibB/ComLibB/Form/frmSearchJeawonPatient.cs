using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

/// <summary>
/// Description : 재원환자 명부
/// Author : 김형범
/// Create Date : 2017.06.20
/// </summary>
/// <history>
/// 실서버 테스트 필요
/// </history>
namespace ComLibB
{
    /// <summary> 재원환자 명부 </summary>
    public partial class frmSearchJeawonPatient : Form
    {
        string FstrPanoList = "";
        string FstrJob = "";
        string FstrCaption = "";
        int FnMinIlsu = 0;
        int FnMaxIlsu = 0;

        string GstrJewon = "";

        public delegate void EventExit();
        public event EventExit rEventExit;

        public delegate void SendJewon(string strJewon);
        public event SendJewon rSendJewon;

        /// <summary> 재원환자 명부 </summary>
        public frmSearchJeawonPatient()
        {
            InitializeComponent();
        }

        public frmSearchJeawonPatient(string strJewon)
        {
            InitializeComponent();

            GstrJewon = strJewon;
        }

        void frmSearchJeawonPatient_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (GstrJewon != "")
            {
                FstrJob = VB.Pstr(GstrJewon, "{@}", 1).Trim();
                FnMinIlsu = Convert.ToInt32(VB.Val(VB.Pstr(GstrJewon, "{@}", 2)));
                FnMaxIlsu = Convert.ToInt32(VB.Val(VB.Pstr(GstrJewon, "{@}", 3)));
                FstrCaption = VB.Pstr(GstrJewon, "{@}", 4);
                dtpFDate.Text = (VB.Pstr(GstrJewon, "{@}", 5).Trim() != "" ? VB.Pstr(GstrJewon, "{@}", 5).Trim() : Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).ToString());
                dtpTDate.Text = (VB.Pstr(GstrJewon, "{@}", 6).Trim() != "" ? VB.Pstr(GstrJewon, "{@}", 6).Trim() : Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).ToString());
                this.Text = FstrCaption;
                rSendJewon("");
            }

            this.Show();
            this.Refresh();

            try
            {

                //병동
                SQL = "";
                SQL = "SELECT WardCode, WardName  ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_WARD ";
                SQL = SQL + ComNum.VBLF + " WHERE WARDCODE NOT IN ('IU','NP','2W','ER') ";
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

                dt.Dispose();
                dt = null;

                cboWard.SelectedIndex = 0;

                //진료과 Combo SET
                SQL = "";
                SQL = "SELECT DeptCode ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_ClinicDept ";
                SQL = SQL + ComNum.VBLF + " WHERE DeptCode NOT IN ('II','HR','TO','R6','HD','ER','PT','AN') ";
                SQL = SQL + ComNum.VBLF + " ORDER BY PrintRanking ";

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

                cboDept.Items.Clear();
                cboDept.Items.Add("전체");

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboDept.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;

                cboDept.SelectedIndex = 0;

                //TODO : 폼 로드시 걸리는 시간이 너무 길어짐
                //Search();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            rEventExit();
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        void Search()
        {

            int i = 0;
            int j = 0;
            int intREAD = 0;
            int intRow = 0;
            int intIlsu = 0;
            int intAge = 0;
            string strOK = "";
            string strPANO = "";
            string strInDate = "";
            string strRoom = "";
            string strOldCode = "";
            string strRoutDate = "";
            string strAmSet1 = "";
            string strAmSet3 = "";
            string strAmSet7 = "";
            string strDietName = "";
            int intDietIlsu = 0;
            string strDietDate = "";
            string strSugaList = "";
            string strPriDate = "";
            string strToDate = "";
            string strNextDate = "";
            string strGDate = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;

            if (cboWard.Text == "")
            {
                ComFunc.MsgBox("병동이 공란입니다.", "오류");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            ssView1_Sheet1.RowCount = 0;
            ssView1_Sheet1.RowCount = 30;

            txtCount.Text = "0 (명)";
            strPriDate = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(-1).ToString();
            strToDate = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).ToString();
            strNextDate = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(1).ToString();

            //오늘기준 2일전 입원환자
            if (FstrJob == "11" || FstrJob == "12")
            {
                strGDate = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(-2).ToString();
                FnMinIlsu = 3;
                FnMaxIlsu = 0;
            }
            else if (FstrJob == "102")
            {
                dtpFDate.Text = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(-1).ToString();
                dtpTDate.Text = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(-1).ToString();
            }

            strSugaList = "";

            //2.9.8 조사당일 복약지도 약물 투여환자 명단

            try
            {

                if (FstrJob == "28")
                {
                    SQL = "";
                    SQL = "SELECT Code  ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                    SQL = SQL + ComNum.VBLF + " WHERE Gubun='DRUG_복약지도필요약품' ";

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
                            strSugaList = strSugaList + "'" + dt.Rows[i]["Code"].ToString().Trim() + "',";
                        }
                        strSugaList = VB.Left(strSugaList, VB.Len(strSugaList) - 1);
                    }
                    dt.Dispose();
                    dt = null;
                }

                //대상 환자를 읽음
                SQL = "";
                SQL = "SELECT M.WardCode,M.RoomCode,M.Pano,M.SName,M.Sex,M.Age,M.Bi,M.PName,M.IPDNO,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(M.InDate,'YYYY-MM-DD') InDate,M.Ilsu,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(M.ROutDate,'YYYY-MM-DD') ROutDate,M.AmSet3,";
                SQL = SQL + ComNum.VBLF + " M.DeptCode,D.DrName,M.AmSet1,M.AmSet6,M.AmSet7 ";
                SQL = SQL + ComNum.VBLF + " FROM   " + ComNum.DB_PMPA + "IPD_NEW_MASTER  M, ";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_DOCTOR  D ";
                  
                if (VB.Left(this.Text, 5) == "2.1.6")
                {
                    switch (cboWard.Text.Trim())
                    {
                        case "전체":
                            SQL = SQL + ComNum.VBLF + "WHERE M.WardCode>' ' ";
                            break;
                        case "MICU":
                            SQL = SQL + ComNum.VBLF + "WHERE M.RoomCode ='234' ";
                            break;
                        case "SICU":
                            SQL = SQL + ComNum.VBLF + "WHERE M.RoomCode ='233' ";
                            break;
                        default:
                            SQL = SQL + ComNum.VBLF + "WHERE M.WardCode = '" + cboWard.Text.Trim() + "' ";
                            break;
                    }
                }
                else
                {
                    if (FstrJob == "103")
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE M.RoomCode IN ('233','234') ";
                    }
                    else
                    {
                        switch (cboWard.Text.Trim())
                        {
                            case "전체":
                                SQL = SQL + ComNum.VBLF + "WHERE M.WardCode > ' ' ";
                                break;
                            case "MICU":
                                SQL = SQL + ComNum.VBLF + "WHERE M.RoomCode = '234' ";
                                break;
                            case "SICU":
                                SQL = SQL + ComNum.VBLF + "WHERE M.RoomCode = '233' ";
                                break;
                            default:
                                SQL = SQL + ComNum.VBLF + "WHERE M.WardCode = '" + cboWard.Text.Trim() + "' ";
                                break;
                        }
                    }
                }

                SQL = SQL + ComNum.VBLF + "  AND M.Pano <> '81000004' ";
                SQL = SQL + ComNum.VBLF + "  AND M.Amset4 <> '3' ";   //정상애기 제외

                //진료과
                if (cboDept.Text != "전체")
                {
                    SQL = SQL + ComNum.VBLF + " AND M.DeptCode = '" + cboDept.Text + "' ";
                }

                //작업분류
                if (FstrJob == "01") //재원자
                {
                    SQL = SQL + ComNum.VBLF + " AND ((M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + " AND M.OutDate IS NULL) OR M.OutDate >= TO_DATE('" + strNextDate + "','YYYY-MM-DD'))";
                    SQL = SQL + ComNum.VBLF + " AND M.IpwonTime < TO_DATE('" + strNextDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + " AND M.Pano < '90000000'";
                }
                else if (FstrJob == "02") //당일입원자
                {
                    SQL = SQL + ComNum.VBLF + "  AND M.InDate >= TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND M.IpwonTime >= TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND M.IpwonTime < TO_DATE('" + strNextDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND M.Pano < '90000000' ";
                }
                else if (FstrJob == "03") //당일퇴원예정자
                {
                    SQL = SQL + ComNum.VBLF + " AND (M.ActDate IS NULL OR M.ActDate = TRUNC(SYSDATE)) ";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSts NOT IN ('7','9')  ";
                    SQL = SQL + ComNum.VBLF + " AND M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND M.OutDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.ROutDate >= TRUNC(SYSDATE) ";
                }
                else if (FstrJob == "05") //치료식 3일이상 18세~64세 입원환자
                {
                    SQL = SQL + ComNum.VBLF + " AND (M.ActDate IS NULL OR M.ActDate = TRUNC(SYSDATE)) ";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSts IN ('0','2') ";
                    SQL = SQL + ComNum.VBLF + " AND M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND M.OutDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.Age >= 18 ";
                    SQL = SQL + ComNum.VBLF + " AND M.Age <= 64 ";
                }
                else if (FstrJob == "08") //재원 7일이상 14일미만 기준병실 입원환자
                {
                    SQL = SQL + ComNum.VBLF + " AND (M.ActDate IS NULL OR M.ActDate=TRUNC(SYSDATE)) ";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSts IN ('0','2') ";
                    SQL = SQL + ComNum.VBLF + " AND M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND M.OutDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.WardCode NOT IN ('HU','IU','IQ') ";
                    SQL = SQL + ComNum.VBLF + " AND M.RoomCode IN (SELECT RoomCode FROM  " + ComNum.DB_PMPA + "BAS_ROOM ";
                    SQL = SQL + ComNum.VBLF + "     WHERE TBed >= 4) ";
                }
                else if (FstrJob == "09") //재원 3일이상 7일미만 입원환자
                {
                    SQL = SQL + ComNum.VBLF + " AND M.ActDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSTS = '0' ";
                    SQL = SQL + ComNum.VBLF + " AND M.Ilsu >= 3 AND M.Ilsu <= 6 ";
                }
                else if (FstrJob == "10") //10.ER응급 수술후 재원31일이하 환자 명단
                {
                    SQL = SQL + ComNum.VBLF + " AND M.ActDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSTS = '0' ";
                    SQL = SQL + ComNum.VBLF + " AND M.Ilsu <= 31";
                    SQL = SQL + ComNum.VBLF + " AND M.Pano IN (SELECT Pano FROM " + ComNum.DB_PMPA + "ORAN_MASTER ";
                    SQL = SQL + ComNum.VBLF + "     WHERE OpDate >= TRUNC(SYSDATE-31) ";
                    SQL = SQL + ComNum.VBLF + "       AND OpBun = '3') "; //응급ER수술
                }
                else if (FstrJob == "11") //재원자 (재원기간 xx ~ xx일)
                {
                    SQL = SQL + ComNum.VBLF + " AND M.ActDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSTS = '0' ";
                    SQL = SQL + ComNum.VBLF + " AND M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + " AND M.OutDate IS NULL ";
                }
                else if (FstrJob == "12") //1.1.6 재원 3일 이상 기준병실 입원환자
                {
                    SQL = SQL + ComNum.VBLF + " AND M.ActDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSts = '0' ";
                    SQL = SQL + ComNum.VBLF + " AND M.WardCode NOT IN ('HU','IU','IQ') ";
                    SQL = SQL + ComNum.VBLF + " AND M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND M.OutDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.RoomCode IN (SELECT RoomCode FROM " + ComNum.DB_PMPA + "BAS_ROOM ";
                    SQL = SQL + ComNum.VBLF + "     WHERE TBed >= 4) ";
                }
                else if (FstrJob == "13") //1.1.8 수술 후 7일 이내인 환자
                {
                    SQL = SQL + ComNum.VBLF + " AND M.ActDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSts = '0' ";
                    SQL = SQL + ComNum.VBLF + " AND M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND M.OutDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.Pano IN (SELECT Pano FROM " + ComNum.DB_PMPA + "ORAN_MASTER ";
                    SQL = SQL + ComNum.VBLF + "     WHERE OpDate >= TRUNC(SYSDATE-6) ";
                    SQL = SQL + ComNum.VBLF + "       AND Ipdopd = 'I' ";
                    SQL = SQL + ComNum.VBLF + "       AND AnGbn <> 'L' "; //Local은 제외
                    SQL = SQL + ComNum.VBLF + "     GROUP BY Pano) ";
                }
                else if (FstrJob == "15") //2.1.10 재원일이 7~13일 외과계 입원환자"
                {
                    SQL = SQL + ComNum.VBLF + " AND M.ActDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSts = '0' ";
                    SQL = SQL + ComNum.VBLF + " AND M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND M.OutDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.DeptCode NOT IN ('MD','PD','DM','NP','RM','NE') ";
                }
                else if (FstrJob == "16") //2.4.2 일반상식을 3일 이상 섭취(18~64세 입원환자)
                {
                    SQL = SQL + ComNum.VBLF + " AND (M.ActDate IS NULL OR M.ActDate = TRUNC(SYSDATE)) ";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSts IN ('0','2') ";
                    SQL = SQL + ComNum.VBLF + " AND M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + " AND M.OutDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.Age >= 18 ";
                    SQL = SQL + ComNum.VBLF + " AND M.Age <= 64 ";
                }
                else if (FstrJob == "21") //2.1.3 외래경유 재원3~7일 환자(내과계)
                {
                    SQL = SQL + ComNum.VBLF + " AND M.ActDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSts = '0' ";
                    SQL = SQL + ComNum.VBLF + " AND M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + " AND M.OutDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.AmSet7 IN ('0','1','2') ";
                    SQL = SQL + ComNum.VBLF + " AND M.Ilsu <= 6 ";
                    SQL = SQL + ComNum.VBLF + " AND M.DeptCode IN ('MD','PD','DM','NP','RM','NE') ";
                }
                else if (FstrJob == "22") //2.1.3 외래경유 재원3~7일 환자(외과계)
                {
                    SQL = SQL + ComNum.VBLF + " AND M.ActDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSts = '0' ";
                    SQL = SQL + ComNum.VBLF + " AND M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND M.OutDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.AmSet7 IN ('0','1','2') ";
                    SQL = SQL + ComNum.VBLF + " AND M.Ilsu <= 6 ";
                    SQL = SQL + ComNum.VBLF + " AND M.DeptCode NOT IN ('MD','PD','DM','NP','RM','NE') ";
                }
                else if (FstrJob == "23") //1.6.6 육체적 구속을 시행한 재원환자
                {
                    SQL = SQL + ComNum.VBLF + " AND M.ActDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSts = '0' ";
                    SQL = SQL + ComNum.VBLF + " AND M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND M.OutDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.Pano IN (SELECT Pano FROM " + ComNum.DB_PMPA + "NUR_MASTER ";
                    SQL = SQL + ComNum.VBLF + "                 WHERE OutDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + "                   AND GbNurinfo2 = 'Y') ";
                }
                else if (FstrJob == "24") //2.1.6 무의식 또는 사지마비 환자 명단
                {
                    SQL = SQL + ComNum.VBLF + " AND M.ActDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSts = '0' ";
                    SQL = SQL + ComNum.VBLF + " AND M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND M.OutDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.Pano IN (SELECT Pano FROM " + ComNum.DB_PMPA + "NUR_MASTER ";
                    SQL = SQL + ComNum.VBLF + "                 WHERE OutDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + "                   AND (GbNurinfo3 = 'Y' OR GbNurinfo4='Y')) ";
                }
                else if (FstrJob == "25") //2.1.6 기관지절개술 환자 명단
                {
                    SQL = SQL + ComNum.VBLF + " AND M.ActDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSts = '0' ";
                    SQL = SQL + ComNum.VBLF + " AND M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND M.OutDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.Pano IN (SELECT Pano FROM " + ComNum.DB_PMPA + "NUR_MASTER ";
                    SQL = SQL + ComNum.VBLF + "                 WHERE OutDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + "                   AND GbNurinfo5 = 'Y') ";
                }
                else if (FstrJob == "26" || FstrJob == "27") //2.5.5 조사시행 3일전부터 조사전일까지 응급실경유 입원환자
                {
                    SQL = SQL + ComNum.VBLF + " AND M.ActDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSts = '0' ";
                    SQL = SQL + ComNum.VBLF + " AND M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + " AND M.OutDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.AmSet7 IN ('3','4','5') ";

                    if (FstrJob == "26")
                    {
                        SQL = SQL + ComNum.VBLF + " AND M.DeptCode IN ('MD','PD','DM','NP','RM','NE') ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " AND M.DeptCode NOT IN ('MD','PD','DM','NP','RM','NE') ";
                    }
                }
                else if (FstrJob == "28") //2.9.8 조사당일 복약지도 약물 투여환자 명단"             --2005
                {
                    SQL = SQL + ComNum.VBLF + " AND ((M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND M.OutDate IS NULL) OR M.OutDate = TRUNC(SYSDATE)) ";
                    SQL = SQL + ComNum.VBLF + " AND M.Pano IN (SELECT Pano FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                    SQL = SQL + ComNum.VBLF + "                 WHERE ActDate = TRUNC(SYSDATE) ";
                    SQL = SQL + ComNum.VBLF + "                   AND SuNext IN (" + strSugaList + ") ";
                    SQL = SQL + ComNum.VBLF + "                 GROUP BY Pano) ";
                    SQL = SQL + ComNum.VBLF + " AND M.ILSU >= '5'";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSts = '0' ";
                }
                else if (FstrJob == "29") //2.1.3 재원일수 3일 이상 7일 이하 외래 통한 입원한 내과계및 외과계환자
                {
                    SQL = SQL + ComNum.VBLF + " AND M.ActDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSts = '0' ";
                    SQL = SQL + ComNum.VBLF + " AND M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND M.OutDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.AmSet7 IN ('0','1','2') ";
                    SQL = SQL + ComNum.VBLF + " AND M.Ilsu <= 6 ";
                    SQL = SQL + ComNum.VBLF + " AND M.Ilsu >= 3 ";
                    SQL = SQL + ComNum.VBLF + " AND M.Ilsu <= 6 ";
                }
                else if (FstrJob == "30") //2.1.6 조사시행 전일까지 가장최근에 응급실을 통해 입원한 환자
                {
                    SQL = SQL + ComNum.VBLF + " AND M.ActDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSts = '0' ";
                    SQL = SQL + ComNum.VBLF + " AND M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND M.OutDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.AmSet7 IN ('3','4','5') ";
                    SQL = SQL + ComNum.VBLF + " AND M.INDATE >= TRUNC(SYSDATE -4)";
                }
                else if (FstrJob == "31") //2.1.6 중환자실 재원일수가 3일이상인 중환자
                {
                    SQL = SQL + ComNum.VBLF + " AND M.ActDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSts = '0' ";
                    SQL = SQL + ComNum.VBLF + " AND M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND M.OutDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.WARDCODE ='IU' ";
                }
                else if (FstrJob == "32" || FstrJob == "44") //2.1.12 내과계및 외과계환자 재원자
                {
                    SQL = SQL + ComNum.VBLF + " AND M.ActDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSts = '0' ";
                    SQL = SQL + ComNum.VBLF + " AND M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND M.OutDate IS NULL ";

                    if (FstrJob == "32")
                    {
                        SQL = SQL + ComNum.VBLF + " AND M.DeptCode IN ('MD','PD','DM','NP','RM','NE') ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " AND M.DeptCode NOT IN ('MD','PD','DM','NP','RM','NE') ";
                    }
                }
                else if (FstrJob == "33") //2.1.15 통증관리 재원일수 3일이상 14일미만 마약성 진동체를 3일이상 투여받은 환자
                {
                    SQL = SQL + ComNum.VBLF + " AND M.ActDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSts = '0' ";
                    SQL = SQL + ComNum.VBLF + " AND M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND M.OutDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.Ilsu >= 3 ";
                    SQL = SQL + ComNum.VBLF + " AND M.Ilsu < 14 ";
                    SQL = SQL + ComNum.VBLF + "  AND M.IPDNO IN ( SELECT B.IPDNO";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_MAYAK A , " + ComNum.DB_PMPA + "IPD_NEW_MASTER B";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.PTNO = B.PANO ";
                    SQL = SQL + ComNum.VBLF + "   AND B.ACTDATE IS NULL";
                    SQL = SQL + ComNum.VBLF + "   AND A.BDATE >= B.INDATE ";
                    SQL = SQL + ComNum.VBLF + "   GROUP BY B.IPDNO ) ";
                }
                else if (FstrJob == "34") //2.1.15 전심마취 하에 근골격제 수술을 받은후 48시간 이내의 외과계환자
                {
                    SQL = SQL + ComNum.VBLF + " AND M.ActDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSts = '0' ";
                    SQL = SQL + ComNum.VBLF + " AND M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND M.OutDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.Pano IN (SELECT Pano FROM " + ComNum.DB_PMPA + "ORAN_MASTER ";
                    SQL = SQL + ComNum.VBLF + "     WHERE OpDate >= TRUNC(SYSDATE-3) ";
                    SQL = SQL + ComNum.VBLF + "       AND DeptCode  IN ('NS','OS') ";
                    SQL = SQL + ComNum.VBLF + "       AND Ipdopd = 'I' ";
                    SQL = SQL + ComNum.VBLF + "       AND AnGbn IN ( 'G', 'E','LV-A','LV-K','MASK','S') "; //전신마취 및 척추마취
                    SQL = SQL + ComNum.VBLF + "       AND OPBUN = '1' "; // 정규수술
                    SQL = SQL + ComNum.VBLF + "     GROUP BY Pano) ";
                }
                else if (FstrJob == "35") //당뇨(경구)
                {
                    SQL = SQL + ComNum.VBLF + " AND ((M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "AND M.OutDate IS NULL) OR M.OutDate = TRUNC(SYSDATE)) ";
                    SQL = SQL + ComNum.VBLF + " AND M.Pano IN (SELECT Pano FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                    SQL = SQL + ComNum.VBLF + "                 WHERE ActDate >= TO_DATE('2008-10-04','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "                   AND ActDate <= TO_DATE('2008-10-08','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "                   AND SuNext IN ( ";
                    SQL = SQL + ComNum.VBLF + "                SELECT A.SUNEXT FROM " + ComNum.DB_PMPA + "BAS_SUN A, " + ComNum.DB_PMPA + "BAS_SUT B";
                    SQL = SQL + ComNum.VBLF + "                 WHERE B.SUNEXT = A.SUNEXT ";
                    SQL = SQL + ComNum.VBLF + "                   AND A.DAICODE = '396'"; // 당뇨
                    SQL = SQL + ComNum.VBLF + "                   AND B.DELDATE IS NULL";
                    SQL = SQL + ComNum.VBLF + "                   AND B.BUN = '11')";
                    SQL = SQL + ComNum.VBLF + "                 GROUP BY Pano) ";
                    SQL = SQL + ComNum.VBLF + " AND M.ILSU >='5'";
                    SQL = SQL + ComNum.VBLF + " AND M.ILSU <='30'";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSts ='0' ";
                }
                else if (FstrJob == "36") //당뇨(주사)
                {
                    SQL = SQL + ComNum.VBLF + " AND ((M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND M.OutDate IS NULL) OR M.OutDate=TRUNC(SYSDATE)) ";
                    SQL = SQL + ComNum.VBLF + " AND M.Pano IN (SELECT Pano FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                    SQL = SQL + ComNum.VBLF + "                 WHERE ActDate >= TO_DATE('2008-10-04','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "                   AND ActDate <= TO_DATE('2008-10-08','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "                   AND SuNext IN ( ";
                    SQL = SQL + ComNum.VBLF + "               SELECT A.SUNEXT FROM " + ComNum.DB_PMPA + "BAS_SUN A, " + ComNum.DB_PMPA + "BAS_SUT B";
                    SQL = SQL + ComNum.VBLF + "                WHERE B.SUNEXT = A.SUNEXT ";
                    SQL = SQL + ComNum.VBLF + "                  AND A.DAICODE = '396'"; //당뇨
                    SQL = SQL + ComNum.VBLF + "                  AND B.DELDATE IS NULL";
                    SQL = SQL + ComNum.VBLF + "                  AND B.BUN = '20')";
                    SQL = SQL + ComNum.VBLF + "                 GROUP BY Pano) ";
                    SQL = SQL + ComNum.VBLF + " AND M.ILSU >='5'";
                    SQL = SQL + ComNum.VBLF + " AND M.ILSU <='30'";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSts ='0' ";
                }
                else if (FstrJob == "37") //혈압강화제(경구)
                {
                    SQL = SQL + ComNum.VBLF + " AND ((M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND M.OutDate IS NULL) OR M.OutDate=TRUNC(SYSDATE)) ";
                    SQL = SQL + ComNum.VBLF + " AND M.Pano IN (SELECT Pano FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                    SQL = SQL + ComNum.VBLF + "                 WHERE ActDate >= TO_DATE('2008-10-04','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "                   AND ActDate <= TO_DATE('2008-10-08','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "                   AND SuNext IN ( ";
                    SQL = SQL + ComNum.VBLF + "               SELECT A.SUNEXT FROM " + ComNum.DB_PMPA + "BAS_SUN A, " + ComNum.DB_PMPA + "BAS_SUT B";
                    SQL = SQL + ComNum.VBLF + "                WHERE B.SUNEXT = A.SUNEXT ";
                    SQL = SQL + ComNum.VBLF + "                  AND A.DAICODE = '214'"; //혈압강화제
                    SQL = SQL + ComNum.VBLF + "                  AND B.DELDATE IS NULL";
                    SQL = SQL + ComNum.VBLF + "                  AND B.BUN = '11')";
                    SQL = SQL + ComNum.VBLF + "                 GROUP BY Pano) ";
                    SQL = SQL + ComNum.VBLF + " AND M.ILSU >='5'";
                    SQL = SQL + ComNum.VBLF + " AND M.ILSU <='30'";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSts ='0' ";
                }
                else if (FstrJob == "38") //혈압강화제(주사)
                {
                    SQL = SQL + ComNum.VBLF + " AND ((M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND M.OutDate IS NULL) OR M.OutDate=TRUNC(SYSDATE)) ";
                    SQL = SQL + ComNum.VBLF + " AND M.Pano IN (SELECT Pano FROM IPD_NEW_SLIP ";
                    SQL = SQL + ComNum.VBLF + "                 WHERE ActDate >= TO_DATE('2008-10-04','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "                   AND ActDate <= TO_DATE('2008-10-08','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "                   AND SuNext IN ( ";
                    SQL = SQL + ComNum.VBLF + "               SELECT A.SUNEXT FROM " + ComNum.DB_PMPA + "BAS_SUN A, " + ComNum.DB_PMPA + "BAS_SUT B";
                    SQL = SQL + ComNum.VBLF + "                WHERE B.SUNEXT = A.SUNEXT ";
                    SQL = SQL + ComNum.VBLF + "                  AND A.DAICODE = '214'"; //혈압강화제
                    SQL = SQL + ComNum.VBLF + "                  AND B.DELDATE IS NULL";
                    SQL = SQL + ComNum.VBLF + "                  AND B.BUN = '20')";
                    SQL = SQL + ComNum.VBLF + "                 GROUP BY Pano) ";
                    SQL = SQL + ComNum.VBLF + " AND M.ILSU >='5'";
                    SQL = SQL + ComNum.VBLF + " AND M.ILSU <='30'";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSts ='0' ";
                }
                else if (FstrJob == "39") //제산제
                {
                    SQL = SQL + ComNum.VBLF + " AND ((M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND M.OutDate IS NULL) OR M.OutDate=TRUNC(SYSDATE)) ";
                    SQL = SQL + ComNum.VBLF + " AND M.Pano IN (SELECT Pano FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                    SQL = SQL + ComNum.VBLF + "                 WHERE ActDate >= TO_DATE('2008-10-04','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "                   AND ActDate <= TO_DATE('2008-10-08','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "                   AND SuNext IN ( ";
                    SQL = SQL + ComNum.VBLF + "               SELECT A.SUNEXT FROM " + ComNum.DB_PMPA + "BAS_SUN A, " + ComNum.DB_PMPA + "BAS_SUT B";
                    SQL = SQL + ComNum.VBLF + "                WHERE B.SUNEXT = A.SUNEXT ";
                    SQL = SQL + ComNum.VBLF + "                  AND A.DAICODE = '234'"; //제산제
                    SQL = SQL + ComNum.VBLF + "                  AND B.DELDATE IS NULL )";
                    SQL = SQL + ComNum.VBLF + "                 GROUP BY Pano) ";
                    SQL = SQL + ComNum.VBLF + " AND M.ILSU >='5'";
                    SQL = SQL + ComNum.VBLF + " AND M.ILSU <='30'";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSts ='0' ";
                }
                else if (FstrJob == "40") //궤양제
                {
                    SQL = SQL + ComNum.VBLF + " AND ((M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND M.OutDate IS NULL) OR M.OutDate=TRUNC(SYSDATE)) ";
                    SQL = SQL + ComNum.VBLF + " AND M.Pano IN (SELECT Pano FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                    SQL = SQL + ComNum.VBLF + "                 WHERE ActDate >= TO_DATE('2008-10-04','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "                   AND ActDate <= TO_DATE('2008-10-08','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "                   AND SuNext IN ( ";
                    SQL = SQL + ComNum.VBLF + "               SELECT A.SUNEXT FROM " + ComNum.DB_PMPA + "BAS_SUN A, " + ComNum.DB_PMPA + "BAS_SUT B";
                    SQL = SQL + ComNum.VBLF + "                WHERE B.SUNEXT = A.SUNEXT ";
                    SQL = SQL + ComNum.VBLF + "                  AND A.DAICODE = '232'"; //궤양제
                    SQL = SQL + ComNum.VBLF + "                  AND B.DELDATE IS NULL)";
                    SQL = SQL + ComNum.VBLF + "                 GROUP BY Pano) ";
                    SQL = SQL + ComNum.VBLF + " AND M.ILSU >='5'";
                    SQL = SQL + ComNum.VBLF + " AND M.ILSU <='30'";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSts ='0' ";
                }
                else if (FstrJob == "41") //항생제
                {
                    SQL = SQL + ComNum.VBLF + " AND ((M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND M.OutDate IS NULL) OR M.OutDate=TRUNC(SYSDATE)) ";
                    SQL = SQL + ComNum.VBLF + " AND M.Pano IN (SELECT Pano FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                    SQL = SQL + ComNum.VBLF + "                 WHERE ActDate >= TO_DATE('2008-10-04','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "                   AND ActDate <= TO_DATE('2008-10-08','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "                   AND SuNext IN ( ";
                    SQL = SQL + ComNum.VBLF + "               SELECT A.SUNEXT FROM " + ComNum.DB_PMPA + "BAS_SUN A, " + ComNum.DB_PMPA + "BAS_SUT B";
                    SQL = SQL + ComNum.VBLF + "                WHERE B.SUNEXT = A.SUNEXT ";
                    SQL = SQL + ComNum.VBLF + "                  AND A.DAICODE IN ( '618','613','619')"; //항생제
                    SQL = SQL + ComNum.VBLF + "                  AND B.DELDATE IS NULL";
                    SQL = SQL + ComNum.VBLF + "                  AND B.BUN = '20')";
                    SQL = SQL + ComNum.VBLF + "                 GROUP BY Pano) ";
                    SQL = SQL + ComNum.VBLF + " AND M.ILSU >= '5'";
                    SQL = SQL + ComNum.VBLF + " AND M.ILSU <= '30'";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSts = '0' ";
                }
                else if (FstrJob == "42") //재원일수 2일이하 재원환자(최초입원자)
                {
                    SQL = SQL + ComNum.VBLF + " AND M.ActDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSts = '0' ";
                    SQL = SQL + ComNum.VBLF + " AND M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND M.OutDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.Ilsu <= 2 ";
                    SQL = SQL + ComNum.VBLF + " AND M.PANO NOT IN ( SELECT PANO FROM " + ComNum.DB_PMPA + "MID_SUMMARY GROUP BY PANO ) ";
                }
                else if (FstrJob == "101" || FstrJob == "102" || FstrJob == "103")
                {
                    SQL = SQL + ComNum.VBLF + " AND ((M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND M.OutDate IS NULL) OR M.OutDate>=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'))";
                    SQL = SQL + ComNum.VBLF + " AND M.IpwonTime < TO_DATE('" + Convert.ToDateTime(dtpTDate.Value.ToString("yyyy-MM-dd")).AddDays(1) + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + " AND M.Pano < '90000000'";
                }

                SQL = SQL + ComNum.VBLF + "  AND M.DrCode = D.DrCode(+) ";

                //SORT
                if (rdoSort0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY M.WardCode DESC,M.RoomCode,M.SName ";
                }
                else if (rdoSort1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY M.SName,M.Pano ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY M.DeptCode,D.DrName,M.SName ";
                }

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

                ssView1_Sheet1.RowCount = dt.Rows.Count;

                intREAD = dt.Rows.Count;
                intRow = 0;
                FstrPanoList = "";
                strOldCode = "";

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strOK = "OK";
                    strPANO = dt.Rows[i]["Pano"].ToString().Trim();
                    strInDate = dt.Rows[i]["InDate"].ToString().Trim();
                    strRoom = dt.Rows[i]["RoomCode"].ToString().Trim();
                    strRoutDate = dt.Rows[i]["RoutDate"].ToString().Trim();

                    if (string.Compare(strRoutDate, ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")) < 0)
                    {
                        strRoutDate = "";
                    }

                    //if (strRoutDate == "")
                    //{
                    //    strRoutDate = "9998-12-31";
                    //}
                    //if (Convert.ToDateTime(strRoutDate) < Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")))
                    //    {
                    //        strRoutDate = "";
                    //    }

                    strAmSet1 = dt.Rows[i]["AmSet1"].ToString().Trim();
                    strAmSet3 = dt.Rows[i]["AmSet3"].ToString().Trim();
                    strAmSet7 = dt.Rows[i]["AmSet7"].ToString().Trim(); //입원경로
                    intIlsu = Convert.ToInt32(VB.DateDiff("d", Convert.ToDateTime(strInDate), Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")))) + 1;
                    intAge = Convert.ToInt32(dt.Rows[i]["Age"].ToString().Trim());
                    strDietName = "";

                    //시작일자
                    if (dtpFDate.Text.Trim() != "" && Convert.ToDateTime(strInDate) < dtpFDate.Value)
                    {
                        strOK = "NO";
                    }

                    if (dtpTDate.Text.Trim() != "" && Convert.ToDateTime(strInDate) > dtpTDate.Value)
                    {
                        strOK = "NO";
                    }

                    if (dtpFDate.Text.Trim() == "" && dtpTDate.Text.Trim() == "")
                    {
                        if (FnMinIlsu > 0 && intIlsu < FnMinIlsu)
                        {
                            strOK = "NO";
                        }

                        if (FnMaxIlsu > 0 && intIlsu > FnMaxIlsu)
                        {
                            strOK = "NO";
                        }
                    }

                    if (FstrJob == "04") //재원 3일이상 18세~60세 입원환자
                    {
                        if (Convert.ToInt32(VB.Val(dt.Rows[i]["Ilsu"].ToString().Trim())) < 3)
                        {
                            strOK = "NO";
                        }

                        if (intAge < 18 || intAge > 60)
                        {
                            strOK = "NO";
                        }
                    }
                    else if (FstrJob == "05") //치료식 3일이상 18세~60세 입원환자
                    {
                        if (Convert.ToInt32(VB.Val(dt.Rows[i][""].ToString().Trim())) < 3)
                        {
                            strOK = "NO";
                        }

                        if (intAge < 18 || intAge > 64)
                        {
                            strOK = "NO";
                        }

                        //치료식 3일이상 Check
                        if (strOK == "OK")
                        {
                            SQL = "";
                            SQL = "SELECT TO_CHAR(ActDate,'YYYY-MM-DD') ActDate,DietName,COUNT(*) CNT ";
                            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NEWORDER ";
                            SQL = SQL + ComNum.VBLF + "WHERE Pano = '" + strPANO + "' ";
                            SQL = SQL + ComNum.VBLF + "  AND ActDate >= TRUNC(SYSDATE-2) ";
                            SQL = SQL + ComNum.VBLF + "  AND Bun IN ('02','03') ";
                            SQL = SQL + ComNum.VBLF + "GROUP BY ActDate,DietName ";
                            SQL = SQL + ComNum.VBLF + "ORDER BY ActDate,DietName ";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            strDietDate = "";
                            intDietIlsu = 0;

                            for (j = 0; j < dt1.Rows.Count; j++)
                            {
                                if (dt1.Rows[j]["ActDate"].ToString().Trim() != strDietDate)
                                {
                                    intDietIlsu = intDietIlsu + 1;
                                    strDietDate = dt1.Rows[j]["ActDate"].ToString().Trim();
                                }

                                //오늘 식이를 저장
                                if (Convert.ToDateTime(strDietDate) == Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")))
                                {
                                    strDietName = strDietName + dt1.Rows[j]["DietName"].ToString().Trim() + ",";
                                }
                            }

                            dt1.Dispose();
                            dt1 = null;

                            if (intDietIlsu < 3)
                            {
                                strOK = "NO";
                            }
                        }
                    }
                    else if (FstrJob == "06") //재원3일이하 응급실 경유 입원환자
                    {
                        //if (intIlsu > 3)
                        //{
                        //    strOK = "NO";
                        //}

                        if (VB.Val(strAmSet7) < VB.Val("3") || VB.Val(strAmSet7) > VB.Val("5"))
                        {
                            strOK = "NO";
                        }
                    }
                    else if (FstrJob == "07") //재원3~7일 외래 경유 입원환자
                    {
                        if (VB.Val(strAmSet7) < VB.Val("0") || VB.Val(strAmSet7) > VB.Val("2"))
                        {
                            strOK = "NO";
                        }
                    }
                    else if (FstrJob == "08") //재원 7일이상 14일미만 기준병실 입원환자
                    {

                    }
                    else if (FstrJob == "16") //일반상식 3일이상 18세~64세 입원환자
                    {
                        if (intAge < 18 || intAge > 64)
                        {
                            strOK = "NO";
                        }

                        //일반상식 3일이상 Check
                        if (strOK == "OK")
                        {
                            SQL = "";
                            SQL = "SELECT TO_CHAR(ActDate,'YYYY-MM-DD') ActDate,DietName,COUNT(*) CNT ";
                            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NEWORDER ";
                            SQL = SQL + ComNum.VBLF + "WHERE Pano = '" + strPANO + "' ";
                            SQL = SQL + ComNum.VBLF + "  AND ActDate >= TRUNC(SYSDATE-2) ";
                            SQL = SQL + ComNum.VBLF + "  AND Bun = '01' ";
                            SQL = SQL + ComNum.VBLF + "  AND DietCode IN ('10','25') "; //일반상식,선택식(죽식은 제외)
                            SQL = SQL + ComNum.VBLF + "GROUP BY ActDate,DietName ";
                            SQL = SQL + ComNum.VBLF + "ORDER BY ActDate,DietName ";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            strDietDate = "";
                            intDietIlsu = 0;

                            for (j = 0; j < dt1.Rows.Count; j++)
                            {
                                if (dt1.Rows[j]["ActDate"].ToString().Trim() != strDietDate)
                                {
                                    intDietIlsu = intDietIlsu + 1;
                                    strDietDate = dt1.Rows[j]["ActDate"].ToString().Trim();
                                }

                                //오늘 식이를 저장
                                if (Convert.ToDateTime(strDietDate) == Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")))
                                {
                                    strDietName = strDietName + dt1.Rows[j]["DietName"].ToString().Trim() + ",";
                                }
                            }

                            dt1.Dispose();
                            dt1 = null;

                            if (intDietIlsu < 3)
                            {
                                strOK = "NO";
                            }
                        }
                    }
                    else
                    {
                        strOK = "OK";
                    }

                    if (strOK == "OK")
                    {
                        FstrPanoList = FstrPanoList + "'" + strPANO + "',";

                        if (intRow > ssView1_Sheet1.RowCount)
                        {
                            ssView1_Sheet1.RowCount = intRow;
                        }

                        //호실별로 조회시 같은 병실명은 생략
                        if (rdoSort0.Checked == true)
                        {
                            if (strOldCode != strRoom)
                            {
                                ssView1_Sheet1.Cells[intRow, 1].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                                ssView1_Sheet1.Cells[intRow, 2].Text = strRoom;
                                strOldCode = strRoom;
                            }

                        }
                    }
                    else
                    {
                        ssView1_Sheet1.Cells[intRow, 1].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                        ssView1_Sheet1.Cells[intRow, 2].Text = strRoom;
                    }

                    ssView1_Sheet1.Cells[intRow, 3].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssView1_Sheet1.Cells[intRow, 4].Text = dt.Rows[i]["SName"].ToString().Trim();
                    ssView1_Sheet1.Cells[intRow, 5].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["Sex"].ToString().Trim();
                    ssView1_Sheet1.Cells[intRow, 6].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    ssView1_Sheet1.Cells[intRow, 7].Text = dt.Rows[i]["DrName"].ToString().Trim();
                    ssView1_Sheet1.Cells[intRow, 8].Text = dt.Rows[i]["InDate"].ToString().Trim();
                    ssView1_Sheet1.Cells[intRow, 9].Text = intIlsu.ToString();

                    if (strAmSet1 == "1")
                    {
                        ssView1_Sheet1.Cells[intRow, 12].Text = "퇴원완료";
                    }
                    else if (strAmSet1 == "2")
                    {
                        ssView1_Sheet1.Cells[intRow, 12].Text = "계산발부";
                    }
                    else if (strRoutDate == ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"))
                    {
                        if (strAmSet3 == "9")
                        {
                            ssView1_Sheet1.Cells[intRow, 12].Text = "심사완료";
                        }
                        else
                        {
                            ssView1_Sheet1.Cells[intRow, 12].Text = "심 사 중";
                        }
                    }
                    else if (string.Compare(strRoutDate, ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")) > 0)
                    {
                        ssView1_Sheet1.Cells[intRow, 12].Text = VB.Right(strRoutDate, 5);
                    }

                    ssView1_Sheet1.Cells[intRow, 13].Text = dt.Rows[i]["Bi"].ToString().Trim();
                    ssView1_Sheet1.Cells[intRow, 14].Text = "";

                    switch (dt.Rows[i]["AmSet7"].ToString().Trim())
                    {
                        case "3":
                        case "4":
                        case "5":
                            ssView1_Sheet1.Cells[intRow, 14].Text = "E"; //응급실경유 입원
                            break;
                        default:
                            ssView1_Sheet1.Cells[intRow, 14].Text = "";
                            break;
                    }

                    ssView1_Sheet1.Cells[intRow, 15].Text = strDietName;
                    ssView1_Sheet1.Cells[intRow, 16].Text = VB.Val(dt.Rows[i]["IPDNO"].ToString().Trim()).ToString();

                    //병동 환자마스타를 읽음
                    SQL = "";
                    SQL = "SELECT TO_CHAR(ROutDate,'YYYY-MM-DD') ROutDate,ROutGbPrt,Grade,Diagnosis,Remark,";
                    SQL = SQL + ComNum.VBLF + " GbICU,Bun7,TO_CHAR(ROutEntTime,'MM/DD HH24:MI') ROutEntTime ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_MASTER ";
                    SQL = SQL + ComNum.VBLF + "WHERE Pano = '" + strPANO + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND InDate = TO_DATE('" + strInDate + "','YYYY-MM-DD') ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        ssView1_Sheet1.Cells[intRow, 10].Text = dt1.Rows[0]["Diagnosis"].ToString().Trim();
                        ssView1_Sheet1.Cells[intRow, 11].Text = dt1.Rows[0]["Grade"].ToString().Trim();

                        //퇴원예고
                        if (string.Compare(dt1.Rows[0]["ROutDate"].ToString().Trim(), ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")) >= 0)
                        {
                            if (ssView1_Sheet1.Cells[intRow, 12].Text == "")
                            {
                                ssView1_Sheet1.Cells[intRow, 12].Text = "☞" + VB.Right(dt1.Rows[0]["ROutDate"].ToString().Trim(), 5);
                            }

                            if (FstrJob == "03") //퇴원예고
                            {
                                ssView1_Sheet1.Cells[intRow, 12].Text = dt1.Rows[0]["ROutTime"].ToString().Trim();
                            }
                        }

                        ssView1_Sheet1.Cells[intRow, 15].Text = dt1.Rows[0]["Remark"].ToString().Trim();

                        //의사소통 가능 여부를 표시함
                        if (dt1.Rows[0]["GbICU"].ToString().Trim() != "Y" && VB.Val(dt1.Rows[0]["Bun7"].ToString().Trim()) > VB.Val("0"))
                        {
                            ssView1_Sheet1.Cells[intRow, 11].Text = ssView1_Sheet1.Cells[intRow, 11].Text + "/" + dt1.Rows[0]["Bun7"].ToString().Trim();
                        }
                    }

                    dt1.Dispose();
                    dt1 = null;

                    if (FstrJob == "33") //2.1.15
                    {
                        string strTemp = "";

                        strTemp = "";
                        SQL = "";
                        SQL = " SELECT A.SUCODE, C.SUNAMEK ";
                        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_MAYAK A , " + ComNum.DB_PMPA + "IPD_NEW_MASTER B, " + ComNum.DB_PMPA + "BAS_SUN C";
                        SQL = SQL + ComNum.VBLF + "  WHERE A.PTNO = B.PANO ";
                        SQL = SQL + ComNum.VBLF + "   AND B.ACTDATE IS NULL";
                        SQL = SQL + ComNum.VBLF + "   AND A.BDATE >= B.INDATE ";
                        SQL = SQL + ComNum.VBLF + "    AND B.IPDNO = '" + dt.Rows[i]["IPDNO"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND A.SUCODE = C.SUNEXT ";
                        SQL = SQL + ComNum.VBLF + "   GROUP BY A.SUCODE, C.SUNAMEK";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        for (i = 0; i < dt1.Rows.Count; i++)
                        {
                            if (i == 0)
                            {
                                strTemp = dt1.Rows[i]["SUNAMEK"].ToString().Trim();
                            }
                            else
                            {
                                strTemp = strTemp + "," + dt1.Rows[i]["SUNAMEK"].ToString().Trim();
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;

                        ssView1_Sheet1.Cells[intRow, 15].Text = strTemp;
                    }

                    //수술
                    SQL = "";
                    SQL = "SELECT TO_CHAR(OPDate,'YYYY-MM-DD') OPDate,OPTITLE ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "ORAN_MASTER ";
                    SQL = SQL + ComNum.VBLF + " WHERE Pano= '" + strPANO + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND OPDate >= TO_DATE('" + strInDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND (OPBUN IN ('1','2','3','4') OR OPBUN IS NULL)";
                    SQL = SQL + ComNum.VBLF + "   AND OpCancel IS NULL ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        ssView1_Sheet1.Cells[intRow, 17].Text = "◎";
                    }

                    dt1.Dispose();
                    dt1 = null;

                    //전과여부
                    SQL = "";
                    SQL = " SELECT ROWID ";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_TRANSFOR  ";
                    SQL = SQL + ComNum.VBLF + "  WHERE Pano       = '" + strPANO + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND TRUNC(TrsDate) >= TO_DATE('" + strInDate + "','YYYY-MM-DD')  ";
                    SQL = SQL + ComNum.VBLF + "    AND FrDept <> ToDept ";
                    SQL = SQL + ComNum.VBLF + "  ORDER BY TrsDate ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        ssView1_Sheet1.Cells[intRow, 18].Text = "◎";
                    }

                    dt1.Dispose();
                    dt1 = null;

                    intRow = intRow + 1;
                    //Application.DoEvents();
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            ssView1_Sheet1.RowCount = intRow;

            Cursor.Current = Cursors.Default;
        }

        void btnPrint_Click(object sender, EventArgs e)
        {

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            btnPrint.Enabled = false;

            strFont1 = "/fn\"굴림\" /fz\"20\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont1 = "/fn\"굴림\" /fz\"11\" /fb1 /fi0 /fu0 /fk0 /fs2";

            strHead1 = "/f1" + "재  원   환  자   명  단" + "/n";
            strHead2 = "/n/l/f2" + "병동: " + cboWard.Text;
            strHead2 += "작업방법: " + FstrCaption;
            strHead2 += "출력시간 : " + Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")) + " " + Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T", ":")) + "PAGE:" + "/p";

            ssView1_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2 + "/n";
            ssView1_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView1_Sheet1.PrintInfo.Margin.Top = 50;
            ssView1_Sheet1.PrintInfo.Margin.Bottom = 50;
            ssView1_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView1_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView1_Sheet1.PrintInfo.ShowBorder = true;
            ssView1_Sheet1.PrintInfo.ShowColor = false;
            ssView1_Sheet1.PrintInfo.ShowGrid = false;
            ssView1_Sheet1.PrintInfo.ShowShadows = false;
            ssView1_Sheet1.PrintInfo.UseMax = false;
            ssView1_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView1_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView1.PrintSheet(0);

            btnPrint.Enabled = true;
        }

        void btnPrintSel_Click(object sender, EventArgs e)
        {

            int i = 0;
            int j = 0;
            int intRow = 0;
            string strChk = "";
            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            //SS1 => SS2로 자료를 옮김
            ssView2_Sheet1.RowCount = 0;
            ssView2_Sheet1.RowCount = ssView1_Sheet1.RowCount;
            intRow = 0;

            for (i = 0; i < ssView1_Sheet1.RowCount; i++)
            {
                strChk = Convert.ToBoolean(ssView1_Sheet1.Cells[i, 0].Value) == true ? "1" : "0";

                if (strChk == "1")
                {
                    for (j = 1; j < ssView1_Sheet1.ColumnCount; j++)
                    {
                        ssView2_Sheet1.Cells[intRow, j - 1].Text = ssView1_Sheet1.Cells[i, j-1].Text;
                    }
                    intRow += 1;
                }
            }

            ssView2_Sheet1.RowCount = intRow;

            strFont1 = "/fn\"굴림\" /fz\"20\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림\" /fz\"11\" /fb1 /fi0 /fu0 /fk0 /fs2";
            strHead1 = "/f1" + "재  원   환  자   명  단" + "/n";
            strHead2 = "/n/l/f2" + "병동: " + cboWard.Text + "  작업방법: " + FstrCaption;
            strHead2 += "출력시간 : " + Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")) + " " + Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T", ":")) + "PAGE:" + "/p";

            ssView2_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2 + "/n";
            ssView2_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView2_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssView2_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView2_Sheet1.PrintInfo.Margin.Top = 40;
            ssView2_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssView2_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView2_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView2_Sheet1.PrintInfo.ShowColor = false;
            ssView2_Sheet1.PrintInfo.ShowBorder = true;
            ssView2_Sheet1.PrintInfo.ShowGrid = true;
            ssView2_Sheet1.PrintInfo.ShowShadows = false;
            ssView2_Sheet1.PrintInfo.UseMax = false;
            ssView2_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView2_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView2.PrintSheet(0);
        }

        void cboDept_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtCount.Focus();
            }
        }

        void cboWard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cboDept.Focus();
            }
        }

        void ssView1_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            int i = 0;
            int intCnt = 0;

            if (e.Column != 0)
            {
                return;
            }

            if (ssView1_Sheet1.Cells[e.Row, e.Column].Text == "1")
            {

            }
            else
            {
                //TODO : 셀 색상 지정
                // ssView1_Sheet1.ColumnHeader.Rows[e.Row].BackColor = Color.FromArgb(255, 255, 255);
            }

            intCnt = 0;

            for (i = 0; i < ssView1_Sheet1.RowCount; i++)
            {
                if (Convert.ToBoolean(ssView1_Sheet1.Cells[i, 0].Value) == true)
                {
                    intCnt += 1;
                }
            }

            txtCount.Text = intCnt.ToString() + " (명)";
        }

        void dtpFDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                dtpTDate.Focus();
            }
        }

        void dtpTDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.Focus();
            }
        }

        private void txtCount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnPrintSel.Focus();
            }
        }
    }
}
