using ComBase;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmOcrHis : Form
    {

        string strWard = "";
        
        public frmOcrHis()
        {
            InitializeComponent();
        }

        public frmOcrHis(string strWard)
        {
            InitializeComponent();
            this.strWard = strWard;
        }

        private void frmEmrOcrHis_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            ssHis_Sheet1.RowCount = 0;
            dtpChartDate1.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"),"D"));
            ComboWard_SET();

            if (clsType.User.BuseCode.Trim().Equals("033110"))
            {
                ComboBoxCellType cboCellType = (ComboBoxCellType) ssHis_Sheet1.Columns[4].CellType;
                List<string> lstItem = cboCellType.Items.ToList();
                lstItem.Add("위 내시경 검사ㆍ진정동의서");
                lstItem.Add("위 내시경적 소화관 인공도관 삽입술ㆍ진정동의서");
                lstItem.Add("위 내시경적 역행성 담췌관 조영술ㆍ진정동의서");
                lstItem.Add("위 내시경적 이물제거술ㆍ진정동의서");
                lstItem.Add("위 내시경적 점막 절제술 및 점막하박리술ㆍ진정동의서");
                lstItem.Add("위 내시경적 지혈술ㆍ진정동의서");
                lstItem.Add("대장 내시경 검사ㆍ진정동의서");
                lstItem.Add("대장 내시경적 소화관 인공도관 삽입술진정ㆍ진정동의서");
                lstItem.Add("대장 내시경적 점막 절제술 및 적막하 박리술ㆍ진정동의서");
                lstItem.Add("대장 내시경적 지혈술ㆍ진정동의서");
                lstItem.Add("기관지 내시경 검사ㆍ진정동의서");
                lstItem.Add("경피적 내시경적 위루술ㆍ진정동의서");

                cboCellType.Items = lstItem.ToArray();

                ssHis_Sheet1.Columns[4].CellType = cboCellType;
            }
        }

        void ComboWard_SET()
        {
            #region ComboWard_SET()
            cboWard.Items.Clear();
            cboWard.Items.Add("전체");

            OracleDataReader reader = null;

            int sIndex = -1;
            int sCount = 0;

            try
            {
                string SQL = " SELECT NAME WARDCODE, MATCH_CODE";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_CODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '2'";
                SQL = SQL + ComNum.VBLF + "   AND SUBUSE = '1'";
                SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING";

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        cboWard.Items.Add(reader.GetValue(0).ToString().Trim());
                        if (reader.GetValue(1).ToString().Trim().Equals(clsType.User.BuseCode))
                        {
                            sIndex = sCount;
                        }
                        sCount += 1;
                    }
                }

                cboWard.Items.Add("HD");
                cboWard.Items.Add("ER");
                cboWard.Items.Add("OP");
                cboWard.Items.Add("AG");
                cboWard.Items.Add("ENDO");
                cboWard.Items.Add("외래수혈");
                cboWard.Items.Add("CT");
                cboWard.Items.Add("MRI");
                cboWard.Items.Add("RI");
                cboWard.Items.Add("SONO");

                cboWard.SelectedIndex = sIndex == -1 ? cboWard.SelectedIndex : sIndex + 1;

                reader.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            #endregion
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            switch(cboWard.Text.Trim())
            {
                case "HD":
                    GetPatListHd();
                    break;
                case "ER":
                    GetPatListER();
                    break;
                case "OP":
                case "AG":
                    GetPatListOP();
                    break;
                case "ENDO":
                    GetPatListENDO();
                    break;
                default:
                    GetPatListIpd();
                    break;
            }
        }



        void GetPatListHd()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ComFunc.ReadSysDate(clsDB.DbCon);
            DateTime DT = Convert.ToDateTime(clsPublic.GstrSysDate);

            ssAipPatList_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                string strPriDate = DT.AddDays(-1).ToShortDateString();
                string strToDate   = DT.ToShortDateString();
                string strNextDate = DT.AddDays(+1).ToShortDateString();

                SQL = "SELECT BDATE AS TDATE, PANO, SNAME, SEX, AGE,  'O' AS IPDOPD";
                SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_MASTER";
                SQL += ComNum.VBLF + "  WHERE DEPTCODE = 'HD'";
                SQL += ComNum.VBLF + "      AND JIN IN ('0','1','2','3','4','5','6','7','9','E','F','H','M','L','K', 'N','I','J','Q','R','S','A','B')  ";
                SQL += ComNum.VBLF + "      AND BDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "UNION ALL    ";
                SQL += ComNum.VBLF + "SELECT TDATE AS TDATE, PANO, SNAME, SEX, AGE, IPDOPD ";
                SQL += ComNum.VBLF + "FROM  KOSMOS_PMPA.TONG_HD_DAILY ";
                SQL += ComNum.VBLF + "WHERE TDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "AND IPDOPD = 'I'";
                SQL += ComNum.VBLF + "ORDER BY IPDOPD , SNAME";

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

                ssAipPatList_Sheet1.RowCount = dt.Rows.Count;
                ssAipPatList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    ssAipPatList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["IPDOPD"].ToString().Trim();
                    ssAipPatList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssAipPatList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Sname"].ToString().Trim();
                    ssAipPatList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Age"].ToString().Trim();
                    ssAipPatList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Sex"].ToString().Trim();
                    ssAipPatList_Sheet1.Cells[i, 5].Text = "";
                    ssAipPatList_Sheet1.Cells[i, 6].Text = "";
                    ssAipPatList_Sheet1.Cells[i, 7].Text = "";
                    ssAipPatList_Sheet1.Cells[i, 8].Text = "";
                    ssAipPatList_Sheet1.Cells[i, 9].Text = "HD";
                    ssAipPatList_Sheet1.Cells[i, 10].Text = dt.Rows[i]["TDATE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }


        void GetPatListER()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ComFunc.ReadSysDate(clsDB.DbCon);
            DateTime DT = Convert.ToDateTime(clsPublic.GstrSysDate);

            ssAipPatList_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                string strPriDate = DT.AddDays(-1).ToShortDateString();
                string strToDate = DT.ToShortDateString();
                string strNextDate = DT.AddDays(+1).ToShortDateString();

                SQL = "SELECT 'O' AS IPDOPD, a.PANO,b.SName,A.Age,A.Sex,TO_CHAR(a.JDate,'YYYY-MM-DD') AS TDATE";
                SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_ER_PATIENT a,KOSMOS_PMPA.BAS_PATIENT b ";
                SQL += ComNum.VBLF + "WHERE a.JDate=TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND a.Pano=b.Pano(+) ";
                SQL += ComNum.VBLF + " AND OutTime IS NULL ";
                SQL += ComNum.VBLF + "ORDER BY a.InTime DESC,b.SName,a.Pano ";

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

                ssAipPatList_Sheet1.RowCount = dt.Rows.Count;
                ssAipPatList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssAipPatList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["IPDOPD"].ToString().Trim();
                    ssAipPatList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssAipPatList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Sname"].ToString().Trim();
                    ssAipPatList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Age"].ToString().Trim();
                    ssAipPatList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Sex"].ToString().Trim();
                    ssAipPatList_Sheet1.Cells[i, 5].Text = "";
                    ssAipPatList_Sheet1.Cells[i, 6].Text = "";
                    ssAipPatList_Sheet1.Cells[i, 7].Text = "";
                    ssAipPatList_Sheet1.Cells[i, 8].Text = "";
                    ssAipPatList_Sheet1.Cells[i, 9].Text = "ER";
                    ssAipPatList_Sheet1.Cells[i, 10].Text = dt.Rows[i]["TDATE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }


        void GetPatListOP()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ComFunc.ReadSysDate(clsDB.DbCon);
            DateTime DT = Convert.ToDateTime(clsPublic.GstrSysDate);

            ssAipPatList_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                string strPriDate = DT.AddDays(-1).ToShortDateString();
                string strToDate = DT.ToShortDateString();
                string strNextDate = DT.AddDays(+1).ToShortDateString();

                SQL = " SELECT M.WardCode,M.RoomCode,M.Pano,M.SName,M.Sex,M.Age,M.Bi,M.PName, ";
                SQL += ComNum.VBLF + " TO_CHAR(M.InDate,'YYYY-MM-DD') InDate,M.Ilsu,M.IpdNo,M.GbSts,  ";
                SQL += ComNum.VBLF + " TO_CHAR(M.OutDate,'YYYY-MM-DD') OutDate, M.DeptCode,M.DrCode,D.DrName,M.AmSet1, ";
                SQL += ComNum.VBLF + " M.AmSet4,M.AmSet6,M.AmSet7, M.EMR   ";
                SQL += ComNum.VBLF + " FROM   KOSMOS_PMPA.IPD_NEW_MASTER  M,          ";
                SQL += ComNum.VBLF + " KOSMOS_PMPA.BAS_PATIENT P,          ";
                SQL += ComNum.VBLF + " KOSMOS_PMPA.BAS_DOCTOR  D   ";
                SQL += ComNum.VBLF + " WHERE M.PANO IN (   ";
                SQL += ComNum.VBLF + "                                    SELECT PANO  ";
                SQL += ComNum.VBLF + "                                    FROM KOSMOS_PMPA.ORAN_MASTER  ";
                SQL += ComNum.VBLF + "                                    WHERE OPDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "                                )   ";
                SQL += ComNum.VBLF + "AND (M.OutDate IS NULL OR M.OutDate>= TO_DATE('" + strToDate + "','YYYY-MM-DD') ) ";
                SQL += ComNum.VBLF + "AND M.IpwonTime < TO_DATE('" + strToDate + "','YYYY-MM-DD')   ";
                SQL += ComNum.VBLF + "AND M.Pano < '90000000'   ";
                SQL += ComNum.VBLF + "AND M.GbSTS <> '9'    ";
                SQL += ComNum.VBLF + "AND M.Pano=P.Pano(+)    ";
                SQL += ComNum.VBLF + "AND M.DrCode=D.DrCode(+)  ";
                SQL += ComNum.VBLF + "ORDER BY M.RoomCode,M.SName, M.Indate DESC   ";

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

                ssAipPatList_Sheet1.RowCount = dt.Rows.Count;
                ssAipPatList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssAipPatList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                    ssAipPatList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssAipPatList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Sname"].ToString().Trim();
                    ssAipPatList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Age"].ToString().Trim();
                    ssAipPatList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Sex"].ToString().Trim();
                    ssAipPatList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DrName"].ToString().Trim();
                    ssAipPatList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["InDate"].ToString().Trim();
                    ssAipPatList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    ssAipPatList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DrCode"].ToString().Trim();

                    if (dt.Rows[i]["EMR"].ToString().Trim() == "1") //'TEXT EMR대상자
                    {
                        ssAipPatList_Sheet1.Rows[i].Font = new Font("굴림", 8, FontStyle.Bold);
                        ssAipPatList_Sheet1.Rows[i].ForeColor = Color.Blue;
                    }

                    if (dt.Rows[i]["GBSTS"].ToString().Trim() == "7")// '퇴원완료자
                    {
                        ssAipPatList_Sheet1.Rows[i].Font = new Font("굴림", 8, FontStyle.Bold);
                        ssAipPatList_Sheet1.Rows[i].ForeColor = Color.Red;
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        void GetPatListIpd()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ComFunc.ReadSysDate(clsDB.DbCon);
            DateTime DT = Convert.ToDateTime(clsPublic.GstrSysDate);

            ssAipPatList_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                string strPriDate = DT.AddDays(-1).ToShortDateString();
                string strToDate = DT.ToShortDateString();
                string strNextDate = DT.AddDays(+1).ToShortDateString();

                SQL = "SELECT M.WardCode,M.RoomCode,M.Pano,M.SName,M.Sex,M.Age,M.Bi,M.PName,";
                SQL += ComNum.VBLF + " TO_CHAR(M.InDate,'YYYY-MM-DD') InDate,M.Ilsu,M.IpdNo,M.GbSts,";
                SQL += ComNum.VBLF + " TO_CHAR(M.OutDate,'YYYY-MM-DD') OutDate,";
                SQL += ComNum.VBLF + " M.DeptCode,M.DrCode,D.DrName,M.AmSet1,M.AmSet4,M.AmSet6,M.AmSet7, M.EMR ";
                SQL += ComNum.VBLF + " FROM   KOSMOS_PMPA.IPD_NEW_MASTER  M, ";
                SQL += ComNum.VBLF + "        KOSMOS_PMPA.BAS_PATIENT P, ";
                SQL += ComNum.VBLF + "        KOSMOS_PMPA.BAS_DOCTOR  D ";
                switch(cboWard.Text.Trim())
                {
                    case "전체":
                        SQL += ComNum.VBLF + "WHERE M.WardCode>' ' ";
                        break;
                    case "MICU":
                        SQL += ComNum.VBLF + "WHERE M.RoomCode='234' ";
                        break;
                    case "SICU":
                        SQL += ComNum.VBLF + "WHERE M.RoomCode='233' ";
                        break;
                    case "ND":
                    case "NR":
                        SQL += ComNum.VBLF + "WHERE M.WardCode IN ('ND','IQ','NR') ";
                        break;
                    default:
                        SQL += ComNum.VBLF + "WHERE M.WardCode='" + cboWard.Text.Trim() + "' ";
                        break;
                }

                if (clsType.User.Sabun != "4349")
                {
                    SQL += ComNum.VBLF + "  AND M.Pano<>'81000004' ";
                }

                SQL += ComNum.VBLF + " AND (M.OutDate IS NULL OR M.OutDate>=TO_DATE('" + strToDate + "','YYYY-MM-DD')) ";
                SQL += ComNum.VBLF + " AND M.IpwonTime < TO_DATE('" + strNextDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + " AND M.Pano < '90000000' ";
                SQL += ComNum.VBLF + " AND M.GbSTS <> '9' ";
                SQL += ComNum.VBLF + " AND M.GbSts IN ('0','2')  ";
                SQL += ComNum.VBLF + "  AND M.Pano=P.Pano(+) ";
                SQL += ComNum.VBLF + "  AND M.DrCode=D.DrCode(+) ";
                SQL += ComNum.VBLF + "ORDER BY M.RoomCode,M.SName, M.Indate DESC  ";

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

                ssAipPatList_Sheet1.RowCount = dt.Rows.Count;
                ssAipPatList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssAipPatList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                    ssAipPatList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssAipPatList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Sname"].ToString().Trim();
                    ssAipPatList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Age"].ToString().Trim();
                    ssAipPatList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Sex"].ToString().Trim();
                    ssAipPatList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DrName"].ToString().Trim();
                    ssAipPatList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["InDate"].ToString().Trim();
                    ssAipPatList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    ssAipPatList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DrCode"].ToString().Trim();

                    if (dt.Rows[i]["EMR"].ToString().Trim() == "1") //'TEXT EMR대상자
                    {
                        ssAipPatList_Sheet1.Rows[i].Font = new Font("굴림", 8, FontStyle.Bold);
                        ssAipPatList_Sheet1.Rows[i].ForeColor = Color.Blue;
                    }

                    if (dt.Rows[i]["GBSTS"].ToString().Trim() == "7")// '퇴원완료자
                    {
                        ssAipPatList_Sheet1.Rows[i].Font = new Font("굴림", 8, FontStyle.Bold);
                        ssAipPatList_Sheet1.Rows[i].ForeColor = Color.Red;
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void GetPatListENDO()
        {
            int i = 0;
            int nRead = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            string strCurDate = VB.Left(strCurDateTime, 8);

            string strToDate = (VB.DateAdd("d", 0, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd");
            string strNextDate = (VB.DateAdd("d", 1, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT ROOMCODE, PANO, SNAME, AGE, SEX, GBDRG, B.DRNAME, TO_CHAR(INDATE,'YYYY-MM-DD') INDATE, DEPTCODE, A.DRCODE, 'I' GUBUN";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A, " + ComNum.DB_PMPA + "BAS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + "  WHERE a.DrCode = b.DrCode";
                SQL = SQL + ComNum.VBLF + "    AND (JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') OR OUTDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD'))";
                SQL = SQL + ComNum.VBLF + "    AND PANO IN (";
                SQL = SQL + ComNum.VBLF + "   SELECT A1.PTNO";
                SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_MED + "ENDO_JUPMST A1,";
                SQL = SQL + ComNum.VBLF + "          " + ComNum.DB_MED + "OCS_ORDERCODE b1";
                SQL = SQL + ComNum.VBLF + "    Where A1.OrderCode = b1.OrderCode";
                SQL = SQL + ComNum.VBLF + "          AND A1.BDATE < TRUNC (SYSDATE + 1)";
                SQL = SQL + ComNum.VBLF + "          AND (B1.SLIPNO = '0044' OR B1.SLIPNO = '0064' OR B1.SLIPNO = '0105')";
                SQL = SQL + ComNum.VBLF + "          AND (A1.BUSE IS NULL OR A1.BUSE = '056104')";
                SQL = SQL + ComNum.VBLF + "          AND A1.RDATE >= TO_DATE ('" + strToDate + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "          AND A1.RDATE < TO_DATE ('" + strNextDate + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "          AND A1.GBSUNAP IN ('1', '7')";
                SQL = SQL + ComNum.VBLF + "          AND A1.GBIO = 'I')";
                SQL = SQL + ComNum.VBLF + " Union All";
                SQL = SQL + ComNum.VBLF + " SELECT 0 ROOMCODE, PANO, SNAME, AGE, SEX, '' GBDRG, B.DRNAME, TO_CHAR(BDATE,'YYYY-MM-DD') INDATE, DEPTCODE, A.DRCODE, 'O' GUBUN";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_MASTER A, " + ComNum.DB_PMPA + "BAS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + "  Where a.DrCode = b.DrCode";
                SQL = SQL + ComNum.VBLF + "  AND A.BDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "  AND PANO IN (";
                SQL = SQL + ComNum.VBLF + "   SELECT A1.PTNO";
                SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_MED + "ENDO_JUPMST A1,";
                SQL = SQL + ComNum.VBLF + "          " + ComNum.DB_MED + "OCS_ORDERCODE b1";
                SQL = SQL + ComNum.VBLF + "    Where A1.OrderCode = b1.OrderCode";
                SQL = SQL + ComNum.VBLF + "          AND A1.BDATE < TRUNC (SYSDATE + 1)";
                SQL = SQL + ComNum.VBLF + "          AND (B1.SLIPNO = '0044' OR B1.SLIPNO = '0064' OR B1.SLIPNO = '0105')";
                SQL = SQL + ComNum.VBLF + "          AND (A1.BUSE IS NULL OR A1.BUSE = '056104')";
                SQL = SQL + ComNum.VBLF + "          AND A1.RDATE >= TO_DATE ('" + strToDate + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "          AND A1.RDATE < TO_DATE ('" + strNextDate + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "          AND A1.GBSUNAP IN ('1', '7')";
                SQL = SQL + ComNum.VBLF + "          AND A1.GBIO = 'O')";
                SQL = SQL + ComNum.VBLF + "  ORDER BY GUBUN, SNAME";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nRead = dt.Rows.Count;
                ssAipPatList_Sheet1.RowCount = nRead;

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssAipPatList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Sname"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Age"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DrName"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["InDate"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DrCode"].ToString().Trim();

                        //if (dt.Rows[i]["EMR"].ToString().Trim() == "1") //'TEXT EMR대상자
                        //{
                        //    ssAipPatList_Sheet1.Rows[i].Font = new Font("굴림", 8, FontStyle.Bold);
                        //    ssAipPatList_Sheet1.Rows[i].ForeColor = Color.Blue;
                        //}

                        //if (dt.Rows[i]["GBSTS"].ToString().Trim() == "7")// '퇴원완료자
                        //{
                        //    ssAipPatList_Sheet1.Rows[i].Font = new Font("굴림", 8, FontStyle.Bold);
                        //    ssAipPatList_Sheet1.Rows[i].ForeColor = Color.Red;
                        //}
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

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            PRT_SHEET();
        }

        void PRT_SHEET(string arg = "")
        {
            int lngRow = 0;
            int lngCol = 0;
            bool chk = false;

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strGUBUN = "";
            string sROWID = "";

            string strPRTDATE  = "";
            string strPRTTIME  = "";
            string strFormName = "";
            string strPtNo     = "";
            string strPTNAME   = "";
            string strMedFrDate= "";
            string strMedDeptCd= "";
            string strWARDCODE = "";
            string strPRTNAME  = "";
            string strRETNAME  = "";
            string strMoveTo = "";

            ssHisPrt_Sheet1.RowCount = 0;

            for (lngRow = 0; lngRow < ssHis_Sheet1.RowCount; lngRow++)
            {
                chk = Convert.ToBoolean(ssHis_Sheet1.Cells[lngRow, 0].Value);
                if(chk == false)
                {
                    ssHisPrt_Sheet1.RowCount += 1;
                    for(lngCol = 1; lngCol < ssHis_Sheet1.ColumnCount - 3; lngCol++)
                    {
                        if (lngCol == 2)
                            continue;

                        ssHisPrt_Sheet1.Cells[ssHisPrt_Sheet1.RowCount - 1, lngCol - 1].Text = ssHis_Sheet1.Cells[lngRow, lngCol].Text.Trim();
                    }

                    if(ssHisPrt_Sheet1.Cells[ssHisPrt_Sheet1.RowCount - 1, 3].Text.Trim().Length >= 23)
                    {
                        ssHisPrt_Sheet1.Rows[ssHisPrt_Sheet1.RowCount - 1].Height = ssHisPrt_Sheet1.Rows[ssHisPrt_Sheet1.RowCount - 1].GetPreferredHeight() + 5;
                    }
                    else
                    {
                        ssHisPrt_Sheet1.Rows[ssHisPrt_Sheet1.RowCount - 1].Height = 30;
                    }
                }
            }

            //ssHisPrt_Sheet1.SetRowHeight(-1, 30);

            if (arg == "1")
            {
                PreViewAndPrint("P");
                //return;
            }

            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            //Update

            try
            {
                for (lngRow = 0; lngRow < ssHis_Sheet1.RowCount; lngRow++)
                {
                    chk = Convert.ToBoolean(ssHis_Sheet1.Cells[lngRow, 0].Value);
                    if (chk == false)
                    {
                        strGUBUN = ssHis_Sheet1.Cells[lngRow, 14].Text;
                        sROWID = ssHis_Sheet1.Cells[lngRow, 13].Text;
                        if(strGUBUN == "1")
                        {
                            if(sROWID != "")
                            {
                                SQL = "";
                                SQL += ComNum.VBLF + "UPDATE KOSMOS_EMR.EMROCRPRTHIS";
                                SQL += ComNum.VBLF + "    SET PRTUSEID = '" + clsType.User.IdNumber + "',";
                                SQL += ComNum.VBLF + "    PRTDATE = '" + ComQuery.CurrentDateTime(clsDB.DbCon, "D") + "',";
                                SQL += ComNum.VBLF + "    PRTTIME = '" + ComQuery.CurrentDateTime(clsDB.DbCon, "T") + "'";
                                SQL += ComNum.VBLF + "    WHERE ROWID = '" + sROWID + "'";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBoxEx(this, SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                            }
                        }
                        else
                        {
                            strPRTDATE = ComFunc.FormatStrToDateTime(ssHis_Sheet1.Cells[lngRow, 1].Text, "D").Replace("-", "");
                            strPRTTIME = ComFunc.FormatStrToDateTime(ssHis_Sheet1.Cells[lngRow, 2].Text, "T").Replace(":", "");
                            strFormName = ssHis_Sheet1.Cells[lngRow, 4].Text;
                            strMoveTo = ssHis_Sheet1.Cells[lngRow, 5].Text;
                            strPtNo = ssHis_Sheet1.Cells[lngRow, 6].Text;
                            strPTNAME = ssHis_Sheet1.Cells[lngRow, 7].Text;
                            strMedFrDate = ssHis_Sheet1.Cells[lngRow, 8].Text;
                            strMedDeptCd = ssHis_Sheet1.Cells[lngRow, 9].Text;
                            strWARDCODE = ssHis_Sheet1.Cells[lngRow, 10].Text == "ENDO" ? "EO" : ssHis_Sheet1.Cells[lngRow, 10].Text;
                            strPRTNAME = ssHis_Sheet1.Cells[lngRow, 11].Text;
                            strRETNAME = clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun);
                            if(sROWID == "")
                            {
                                if (string.IsNullOrWhiteSpace(strPTNAME))
                                {
                                    continue;
                                }

                                SQL = " INSERT INTO KOSMOS_EMR.EMROCRPRTHIS_USER ";
                                SQL += ComNum.VBLF + " (PRTDATE, PRTTIME, FORMNAME, PTNO, ";
                                SQL += ComNum.VBLF + " PTNAME, MEDFRDATE, MEDDEPTCD, WARDCODE, ";
                                SQL += ComNum.VBLF + " PRTNAME, RETNAME, SABUN, MOVETO) VALUES (";
                                SQL += ComNum.VBLF + "'" + strPRTDATE + "','" + strPRTTIME + "','" + strFormName + "','" + strPtNo + "',";
                                SQL += ComNum.VBLF + "'" + strPTNAME + "','" + strMedFrDate.Replace("-", "") + "','" + strMedDeptCd + "','" + strWARDCODE + "',";
                                SQL += ComNum.VBLF + "'" + strPRTNAME + "','" + strRETNAME + "','" + clsType.User.Sabun + "','" + strMoveTo + "') ";
                            }
                            else
                            {
                                SQL = " UPDATE KOSMOS_EMR.EMROCRPRTHIS_USER SET ";
                                SQL += ComNum.VBLF + " PRTDATE = '" + strPRTDATE + "', ";
                                SQL += ComNum.VBLF + " PRTTIME = '" + strPRTTIME + "', ";
                                SQL += ComNum.VBLF + " FORMNAME = '" + strFormName + "', ";
                                SQL += ComNum.VBLF + " PTNO = '" + strPtNo + "', ";
                                SQL += ComNum.VBLF + " PTNAME = '" + strPTNAME + "', ";
                                SQL += ComNum.VBLF + " MEDFRDATE = '" + strMedFrDate.Replace("-", "") + "', ";
                                SQL += ComNum.VBLF + " MEDDEPTCD = '" + strMedDeptCd + "', ";
                                SQL += ComNum.VBLF + " WARDCODE = '" + strWARDCODE + "', ";
                                SQL += ComNum.VBLF + " PRTNAME = '" + strPRTNAME + "', ";
                                SQL += ComNum.VBLF + " RETNAME = '" + strRETNAME + "', ";
                                SQL += ComNum.VBLF + " SABUN = '" + clsType.User.Sabun + "', ";
                                SQL += ComNum.VBLF + " MOVETO = '" + strMoveTo + "' ";
                                SQL += ComNum.VBLF + " WHERE ROWID = '" + sROWID + "'";
                            }

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBoxEx(this, SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, "저장하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }


        }

        void PreViewAndPrint(string strPrintType = "")
        {
            btnPrint.Enabled = false;
            //'Print Head 지정
            string strFont1 = @"/fn""바탕체"" /fz""14"" /fb1 /fi0 /fu0 /fk0 /fs1";
            string strFont2 = @"/fn""바탕체"" /fz""12"" /fb0 /fi0 /fu0 /fk0 /fs2";
            string strHead1 = "/c/f1" + "OCR 출력현황조회" + " (" + cboWard.Text.Trim() + ") " + "/f1/n/n";
            string strHead2 = "/n/l/f2" + "    출력일자 : " + Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).ToString("yyyy년MM월dd일") + "     /n";
            strHead2 += "/n/l/f2" + "    출력자 : " + clsType.User.UserName + " /r/f2" + "기록사 : " + "        " + "     /n";

            ssHisPrt_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssHisPrt_Sheet1.PrintInfo.ShowRowHeader = PrintHeader.Hide;
            ssHisPrt_Sheet1.PrintInfo.ShowColumnHeader = PrintHeader.Show;
            ssHisPrt_Sheet1.PrintInfo.ShowBorder = true;
            ssHisPrt_Sheet1.PrintInfo.ShowColor = false;
            ssHisPrt_Sheet1.PrintInfo.ShowGrid = true;
            ssHisPrt_Sheet1.PrintInfo.ShowShadows = false;
            ssHisPrt_Sheet1.PrintInfo.UseMax = false;
            ssHisPrt_Sheet1.PrintInfo.Margin.Top = 30;
            ssHisPrt_Sheet1.PrintInfo.Centering = Centering.Horizontal;
            ssHisPrt_Sheet1.PrintInfo.PrintType = PrintType.All;
            ssHisPrt_Sheet1.PrintInfo.Orientation = PrintOrientation.Portrait;

            if (strPrintType == "V")
            {
                ssHisPrt_Sheet1.PrintInfo.Preview = true;
            }

            ssHisPrt.PrintSheet(0);
            Application.DoEvents();
         
            btnPrint.Enabled = true;
        }

        private void btnSearch2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            if(cboWard.Text.Trim() == "")
            {
                ComFunc.MsgBoxEx(this, "병동을 선택해주십시오");
                return;
            }

            GetSearchData();
        }

        private void ssAipPatList_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (e.Row < 0) return;

            if (ssHis_Sheet1.ActiveRowIndex < 0)
            {
                ComFunc.MsgBoxEx(this, "입력할 ROW를 선택해 주십시오.");
                return;
            }

            if (ssHis_Sheet1.Cells[ssHis_Sheet1.ActiveRowIndex, 6].Text.Trim() != "")
            {
                if (ComFunc.MsgBoxQ("이미 DATA가 존재합니다" + ComNum.VBLF + "변경하시겠습니까?") == DialogResult.No)
                {
                    return;
                }
            }

            ssHis_Sheet1.Cells[ssHis_Sheet1.ActiveRowIndex, 1].Text = ComFunc.FormatStrToDateTime(dtpChartDate1.Text, "D");

            ssHis_Sheet1.Cells[ssHis_Sheet1.ActiveRowIndex, 6].Text = ssAipPatList_Sheet1.Cells[e.Row, 1].Text.Trim();
            ssHis_Sheet1.Cells[ssHis_Sheet1.ActiveRowIndex, 7].Text = ssAipPatList_Sheet1.Cells[e.Row, 2].Text.Trim();
            ssHis_Sheet1.Cells[ssHis_Sheet1.ActiveRowIndex, 8].Text = ssAipPatList_Sheet1.Cells[e.Row, 6].Text.Trim();
            ssHis_Sheet1.Cells[ssHis_Sheet1.ActiveRowIndex, 9].Text = ssAipPatList_Sheet1.Cells[e.Row, 7].Text.Trim();
        }

        void GetSearchData()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            int i = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT A.OCRDATE, A.OCRTIME, A.PTNO, A.PTNAME, A.INOUTCLS,";
                SQL += ComNum.VBLF + " A.MEDFRDATE, A.MEDDEPTCD, A.WARDCODE,";
                SQL += ComNum.VBLF + " A.FORMNO, ";
                SQL += ComNum.VBLF + " (SELECT MAX(FORMNAME) AS FORMNAME FROM KOSMOS_EMR.EMRFORM WHERE FORMNO = A.FORMNO) AS FORMNAME,";
                SQL += ComNum.VBLF + " A.USEID, A.DEPTCD, A.DEPTCD1, A.PRTUSEID, A.PRTDATE, A.PRTTIME, A.ROWID,";
                SQL += ComNum.VBLF + " B.NAME, D.NAME AS PRTNAME";
                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMROCRPRTHIS A,";
                SQL += ComNum.VBLF + "      KOSMOS_PMPA.IPD_NEW_MASTER C,";
                SQL += ComNum.VBLF + "      KOSMOS_EMR.EMR_USERT B,";
                SQL += ComNum.VBLF + "      KOSMOS_EMR.EMR_USERT D";
                SQL += ComNum.VBLF + " WHERE A.USEID = B.USERID (+)";
                SQL += ComNum.VBLF + " AND A.PRTUSEID = D.USERID (+)";
                SQL += ComNum.VBLF + " AND A.OCRDATE = '" + VB.Format(dtpChartDate1.Value, "yyyyMMdd") + "'";
                //'''''    SQL = SQL & vbLf & " AND A.OCRDATE <= '" & Format(dtpChartDate2.Value, "YYYYMMDD") & "'"
                SQL += ComNum.VBLF + " AND A.PTNO = C.PANO";
                SQL += ComNum.VBLF + " AND TO_DATE(C.INDATE) <= TO_DATE('" + VB.Format(dtpChartDate1.Value, "yyyyMMdd") + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + " AND (C.OUTDATE IS NULL OR C.OUTDATE >= TO_DATE('" + VB.Format(dtpChartDate1.Value, "yyyyMMdd") + "','YYYY-MM-DD') )";
                SQL += ComNum.VBLF + " AND C.GBSTS <> '9'";

                if(cboWard.Text.Trim() != "전체")
                {
                    if (cboWard.Text.Trim() == "SICU")
                    {
                        SQL += ComNum.VBLF + " AND C.WARDCODE = 'IU' AND C.ROOMCODE = '233'";
                    }
                    else if (cboWard.Text.Trim() == "MICU")
                    {
                        SQL += ComNum.VBLF + " AND C.WARDCODE = 'IU' AND C.ROOMCODE = '234'";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + " AND C.WARDCODE = '" + cboWard.Text.Trim() + "'";
                    }
                }

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
                    SQL = " SELECT PRTDATE, PRTTIME, ";
                    SQL += ComNum.VBLF + " PTNO, PTNAME, MEDFRDATE,";
                    SQL += ComNum.VBLF + " MEDDEPTCD, WARDCODE, FORMNAME,";
                    SQL += ComNum.VBLF + " PRTNAME , RETNAME, MOVETO, MAX(ROWID) ROWIDS";
                    SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMROCRPRTHIS_USER";
                    SQL += ComNum.VBLF + " WHERE WARDCODE = '" + (cboWard.Text.Trim() == "ENDO" ? "EO" : cboWard.Text.Trim()) + "'";
                    SQL += ComNum.VBLF + " AND PRTDATE = '" + VB.Format(dtpChartDate1.Value, "yyyyMMdd") + "'";
                    SQL += ComNum.VBLF + " GROUP BY PRTDATE, PRTTIME, ";
                    SQL += ComNum.VBLF + " PTNO, PTNAME, MEDFRDATE,";
                    SQL += ComNum.VBLF + " MEDDEPTCD, WARDCODE, FORMNAME,";
                    SQL += ComNum.VBLF + " PRTNAME , RETNAME, MOVETO";
                    SQL += ComNum.VBLF + "ORDER BY PTNAME";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    if(dt.Rows.Count > 0)
                    {
                        ssHis_Sheet1.RowCount = dt.Rows.Count;
                        ssHis_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            ssHis_Sheet1.Cells[i, 0].Value = true;
                            ssHis_Sheet1.Cells[i, 1].Text = ComFunc.FormatStrToDate(dt.Rows[i]["PRTDATE"].ToString().Trim(), "D");
                            ssHis_Sheet1.Cells[i, 2].Text = ComFunc.FormatStrToDateTime(dt.Rows[i]["PRTTIME"].ToString().Trim(), "M");
                            ssHis_Sheet1.Cells[i, 4].Text = dt.Rows[i]["FORMNAME"].ToString().Trim();
                            ssHis_Sheet1.Cells[i, 5].Text = dt.Rows[i]["MOVETO"].ToString().Trim();
                            ssHis_Sheet1.Cells[i, 6].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                            ssHis_Sheet1.Cells[i, 7].Text = dt.Rows[i]["PTNAME"].ToString().Trim();
                            ssHis_Sheet1.Cells[i, 8].Text = dt.Rows[i]["MEDFRDATE"].ToString().Trim();
                            ssHis_Sheet1.Cells[i, 9].Text = dt.Rows[i]["MEDDEPTCD"].ToString().Trim();
                            ssHis_Sheet1.Cells[i, 10].Text = dt.Rows[i]["WARDCODE"].ToString().Trim() == "EO" ? "ENDO" : dt.Rows[i]["WARDCODE"].ToString().Trim();
                            ssHis_Sheet1.Cells[i, 11].Text = dt.Rows[i]["PRTNAME"].ToString().Trim();
                            ssHis_Sheet1.Cells[i, 12].Text = dt.Rows[i]["RETNAME"].ToString().Trim();
                            ssHis_Sheet1.Cells[i, 13].Text = dt.Rows[i]["ROWIDS"].ToString().Trim();
                            ssHis_Sheet1.Cells[i, 14].Text = "2";
                        }

                        dt.Dispose();
                        dt = null;
                    }

                    if (VB.Val(txt1.Text) >= 1)
                    {
                        ssHis_Sheet1.RowCount = (int)VB.Val(txt1.Text);
                        ssHis_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                        ssHis_Sheet1.Cells[0, 1, ssHis_Sheet1.RowCount - 1, 1].Text = VB.Format(dtpChartDate1.Value, "yyyy-MM-dd").ToString();
                        ssHis_Sheet1.Cells[0, 10, ssHis_Sheet1.RowCount - 1, 10].Text = cboWard.Text.Trim();
                        ssHis_Sheet1.Cells[0, 14, ssHis_Sheet1.RowCount - 1, 14].Text = "2";
                    }

                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssHis_Sheet1.RowCount = dt.Rows.Count;
                ssHis_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["PRTUSEID"].ToString().Trim() != "")
                    {
                        ssHis_Sheet1.Cells[i, 0].Value = true;
                    }

                    ssHis_Sheet1.Cells[i, 1].Text = ComFunc.FormatStrToDate(dt.Rows[i]["OCRDATE"].ToString().Trim(), "D");
                    ssHis_Sheet1.Cells[i, 2].Text = ComFunc.FormatStrToDateTime(VB.Left(dt.Rows[i]["OCRTIME"].ToString().Trim(), 4), "M");
                    ssHis_Sheet1.Cells[i, 3].Text = dt.Rows[i]["FORMNO"].ToString().Trim();
                    ssHis_Sheet1.Cells[i, 4].Text = dt.Rows[i]["FORMNAME"].ToString().Trim();
                    ssHis_Sheet1.Cells[i, 6].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                    ssHis_Sheet1.Cells[i, 7].Text = dt.Rows[i]["PTNAME"].ToString().Trim();
                    ssHis_Sheet1.Cells[i, 8].Text = ComFunc.FormatStrToDate(dt.Rows[i]["OCRDATE"].ToString().Trim(), "D");
                    ssHis_Sheet1.Cells[i, 9].Text = dt.Rows[i]["MEDDEPTCD"].ToString().Trim();
                    ssHis_Sheet1.Cells[i, 10].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                    ssHis_Sheet1.Cells[i, 11].Text = dt.Rows[i]["NAME"].ToString().Trim();
                    ssHis_Sheet1.Cells[i, 12].Text = dt.Rows[i]["PRTNAME"].ToString().Trim();
                    ssHis_Sheet1.Cells[i, 13].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    ssHis_Sheet1.Cells[i, 14].Text = "1";
                }

                dt.Dispose();
                dt = null;

                SQL = " SELECT PRTDATE, PRTTIME, ";
                SQL += ComNum.VBLF + " PTNO, PTNAME, MEDFRDATE,";
                SQL += ComNum.VBLF + " MEDDEPTCD, WARDCODE, FORMNAME,";
                SQL += ComNum.VBLF + " PRTNAME , RETNAME, ROWID, MOVETO";
                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMROCRPRTHIS_USER";
                SQL += ComNum.VBLF + " WHERE WARDCODE = '" + (cboWard.Text.Trim() == "ENDO" ? "EO" : cboWard.Text.Trim()) + "'";
                SQL += ComNum.VBLF + " AND PRTDATE = '" + VB.Format(dtpChartDate1.Value, "yyyyMMdd") + "'";
                SQL += ComNum.VBLF + "ORDER BY PTNAME";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                   
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssHis_Sheet1.RowCount += 1;
                        ssHis_Sheet1.Cells[ssHis_Sheet1.RowCount - 1, 0].Value = true;
                        ssHis_Sheet1.Cells[ssHis_Sheet1.RowCount - 1, 1].Text = ComFunc.FormatStrToDate(dt.Rows[i]["PRTDATE"].ToString().Trim(), "D");
                        ssHis_Sheet1.Cells[ssHis_Sheet1.RowCount - 1, 2].Text = ComFunc.FormatStrToDateTime(dt.Rows[i]["PRTTIME"].ToString().Trim(), "M");
                        ssHis_Sheet1.Cells[ssHis_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["FORMNAME"].ToString().Trim();
                        ssHis_Sheet1.Cells[ssHis_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["MOVETO"].ToString().Trim();
                        ssHis_Sheet1.Cells[ssHis_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                        ssHis_Sheet1.Cells[ssHis_Sheet1.RowCount - 1, 7].Text = dt.Rows[i]["PTNAME"].ToString().Trim();
                        ssHis_Sheet1.Cells[ssHis_Sheet1.RowCount - 1, 8].Text = dt.Rows[i]["MEDFRDATE"].ToString().Trim();
                        ssHis_Sheet1.Cells[ssHis_Sheet1.RowCount - 1, 9].Text = dt.Rows[i]["MEDDEPTCD"].ToString().Trim();
                        ssHis_Sheet1.Cells[ssHis_Sheet1.RowCount - 1, 10].Text = dt.Rows[i]["WARDCODE"].ToString().Trim() == "EO" ? "ENDO" : dt.Rows[i]["WARDCODE"].ToString().Trim();
                        ssHis_Sheet1.Cells[ssHis_Sheet1.RowCount - 1, 11].Text = dt.Rows[i]["PRTNAME"].ToString().Trim();
                        ssHis_Sheet1.Cells[ssHis_Sheet1.RowCount - 1, 12].Text = dt.Rows[i]["RETNAME"].ToString().Trim();
                        ssHis_Sheet1.Cells[ssHis_Sheet1.RowCount - 1, 13].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssHis_Sheet1.Cells[ssHis_Sheet1.RowCount - 1, 14].Text = "2";
                    }

                    dt.Dispose();
                    dt = null;      
                }

                if (VB.Val(txt1.Text) >= 1)
                {
                    for (i = 0; i < VB.Val(txt1.Text); i++)
                    {
                        ssHis_Sheet1.RowCount += 1;
                        ssHis_Sheet1.Cells[ssHis_Sheet1.RowCount - 1, 1].Text = VB.Format(dtpChartDate1.Value, "yyyy-MM-dd").ToString();
                        ssHis_Sheet1.Cells[ssHis_Sheet1.RowCount - 1, 10].Text = cboWard.Text.Trim();
                        ssHis_Sheet1.Cells[ssHis_Sheet1.RowCount - 1, 14].Text = "2";
                    }
                    ssHis_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            PRT_SHEET("1");
        }
          
        private void btnPrintPreView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            PreViewAndPrint("V");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
