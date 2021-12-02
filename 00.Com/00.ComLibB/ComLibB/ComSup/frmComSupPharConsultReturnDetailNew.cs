using ComBase;
using ComDbB;
using ComEmrBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmComSupPharConsultReturnDetailNew : Form
    {
        //string GstrMode = "";
        string GstrPART = string.Empty;
        string GstrSEQNO = string.Empty;
        string GstrIPDNO = string.Empty;
        string GstrPROGRESS = string.Empty;
        string GstrPTNO = string.Empty;
        string GstrDRUGCODE = string.Empty;
        string GstrORDERCODE = string.Empty;
        string GstrWRITEDATE = string.Empty;
        string GstrWRITESABUN = string.Empty;
        string GstrDeptCode = string.Empty;
        string GstrBDATE = string.Empty;
        string GstrRTN_SPD = string.Empty;

        /// <summary>
        /// 2021-07-05 추가함.
        /// </summary>
        public string GstrTREATNO = string.Empty;

        FarPoint.Win.Spread.FpSpread Gspd = null;
        FarPoint.Win.Spread.SheetView GspdSheet = null;

        bool GbolNurse = false;

        public frmComSupPharConsultReturnDetailNew()
        {
            InitializeComponent();
        }

        public frmComSupPharConsultReturnDetailNew(string strPART, string strSEQNO, string strIPDNO = "", string strPTNO = "", string strPROGRESS = "", string strDRUGCODE = "")
        {
            InitializeComponent();

            GstrPART = strPART;
            GstrSEQNO = strSEQNO;
            GstrIPDNO = strIPDNO;
            GstrPROGRESS = strPROGRESS;
            GstrPTNO = strPTNO;
            GstrDRUGCODE = strDRUGCODE;
        }
        public frmComSupPharConsultReturnDetailNew(string strPART, string strSEQNO, string strPTNO, string strDEPTCODE, string strBDATE, string strPROGRESS = "", string strDRUGCODE = "")
        {
            InitializeComponent();

            GstrPART = strPART;
            GstrSEQNO = strSEQNO;
            GstrDeptCode = strDEPTCODE;
            GstrBDATE = strBDATE;
            GstrPROGRESS = strPROGRESS;
            GstrPTNO = strPTNO;
            GstrDRUGCODE = strDRUGCODE;
        }


        //public frmComSupPharConsultReturnDetailNew(string strMode, string strPART, string strSEQNO, string strIPDNO = "", string strPTNO = "", string strPROGRESS = "", string strDRUGCODE = "")
        //{
        //    InitializeComponent();

        //    GstrMode = strMode;
        //    GstrPART = strPART;
        //    GstrSEQNO = strSEQNO;
        //    GstrIPDNO = strIPDNO;
        //    GstrPROGRESS = strPROGRESS;
        //    GstrPTNO = strPTNO;
        //    GstrDRUGCODE = strDRUGCODE;
        //}

        private void frmComSupPharConsultReturnDetailNew_Load(object sender, EventArgs e)
        {
            
            InitPanel();
            
            switch (GstrPART)
            {
                case "01":
                    string SpdNumber = READ_USED_PCONSULT();
                    if (SpdNumber == "")
                    {
                        string strMsg = "초진으로 선택하시겠습니까?" + ComNum.VBLF + "예 : 초진 , 아니요 : 재진";
                        if (ComFunc.MsgBoxQEx(this, strMsg, "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                        {
                            Gspd = ssView01;
                            GspdSheet = ssView01.ActiveSheet;
                            GstrRTN_SPD = "01";
                            panPart01.Visible = true;
                        }
                        else
                        {
                            Gspd = ssView02;
                            GspdSheet = ssView02.ActiveSheet;
                            GstrRTN_SPD = "02";
                            panPart02.Visible = true;
                        }
                    }
                    else
                    {
                        if (SpdNumber == "01")
                        {
                            Gspd = ssView01;
                            GspdSheet = ssView01.ActiveSheet;
                            GstrRTN_SPD = "01";
                            panPart01.Visible = true;
                        }
                        else
                        {
                            Gspd = ssView02;
                            GspdSheet = ssView02.ActiveSheet;
                            GstrRTN_SPD = "02";
                            panPart02.Visible = true;
                        }                        
                    }
                    break;
                case "03":
                    panPart03.Visible = true;
                    break;
                case "04":
                    Gspd = ssView04;
                    GspdSheet = ssView04.ActiveSheet;
                    GstrRTN_SPD = "04";
                    panPart04.Visible = true;
                    break;
                case "05":
                    Gspd = ssView05;
                    GspdSheet = ssView05.ActiveSheet;
                    GstrRTN_SPD = "05";
                    panPart05.Visible = true;
                    break;
                case "06":
                    Gspd = ssView06;
                    GspdSheet = ssView06.ActiveSheet;
                    GstrRTN_SPD = "06";
                    panPart06.Visible = true;
                    break;
                case "07":
                    Gspd = ssView07;
                    GspdSheet = ssView07.ActiveSheet;
                    GstrRTN_SPD = "07";
                    panPart07.Visible = true;
                    break;
                case "08":
                    Gspd = ssView08;
                    GspdSheet = ssView08.ActiveSheet;
                    GstrRTN_SPD = "08";
                    panPart08.Visible = true;
                    break;
            }

            ChkNurse();

            if (ChkYakuk(clsType.User.Sabun) == false)
            {
                panSave.Visible = false;
                btnSaveC.Visible = false;
                panDelete.Visible = false;
                //panPrint.Visible = false;
            }

            if (GstrIPDNO != "")
            {
                setPatientInfo(GspdSheet, GstrIPDNO); 
            }
            else
            {
                setPatientInfoOpd(GspdSheet, GstrTREATNO);
            }

            if (GstrSEQNO != "" && GstrSEQNO != "DRUG")
            {
                READ_DATA(GspdSheet);
                readPConsult(GspdSheet, GstrSEQNO);
            }           

            //else if (GstrSEQNO == "DRUG")
            //{
            //    GstrIPDNO = clsVbfunc.readIPDNO(clsDB.DbCon, GstrPROGRESS, GstrPTNO);

            //    ssConsult0_Sheet1.Cells[13, 12].Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
            //    ssConsult1_Sheet1.Cells[13, 12].Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
            //}

            //if (GstrIPDNO != "")
            //{
            //    setPatientInfo(ssConsult0_Sheet1, GstrIPDNO);
            //    setPatientInfo(ssConsult1_Sheet1, GstrIPDNO);
            //}

            //btnSearch.Click += new EventHandler(btnSearch_Click);

            //if (GbolNurse == true)
            //{
            //    NurseDisplay();
            //}
            //else
            //{
            //    DefaultDisplay();
            //}

            //if (GstrSEQNO == "DRUG")
            //{
            //    ssConsult0_Sheet1.Cells[8, 3].Text = GstrDRUGCODE;
            //    ssConsult0_Sheet1.Cells[8, 4].Text = clsVbfunc.READ_SugaName(clsDB.DbCon, GstrDRUGCODE);
            //    ssConsult1_Sheet1.Cells[8, 3].Text = GstrDRUGCODE;
            //    ssConsult1_Sheet1.Cells[8, 4].Text = clsVbfunc.READ_SugaName(clsDB.DbCon, GstrDRUGCODE);
            //}
        }
            
        private void InitPanel()
        {
            panPart01.Dock = DockStyle.Fill;
            panPart02.Dock = DockStyle.Fill;
            panPart03.Dock = DockStyle.Fill;
            panPart04.Dock = DockStyle.Fill;
            panPart05.Dock = DockStyle.Fill;
            panPart06.Dock = DockStyle.Fill;
            panPart07.Dock = DockStyle.Fill;
            panPart08.Dock = DockStyle.Fill;

            panPart01.Visible = false;
            panPart02.Visible = false;
            panPart03.Visible = false;
            panPart04.Visible = false;
            panPart05.Visible = false;
            panPart06.Visible = false;
            panPart07.Visible = false;
            panPart08.Visible = false;
        }


        private void ChkNurse()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = "SELECT * FROM " + ComNum.DB_ERP + "INSA_MST";
                SQL = SQL + ComNum.VBLF + "     WHERE JIK IN ";
                SQL = SQL + ComNum.VBLF + "             (SELECT CODE FROM " + ComNum.DB_ERP + "INSA_CODE";
                SQL = SQL + ComNum.VBLF + "                 WHERE (NAME LIKE '%간호사%' OR NAME LIKE '%응급구조사%') AND GUBUN = '2')";
                SQL = SQL + ComNum.VBLF + "         AND SABUN = '" + clsType.User.Sabun + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    GbolNurse = true;
                }

                dt.Dispose();
                dt = null;

                GbolNurse = clsVbfunc.NurseSystemManagerChk(clsDB.DbCon, clsType.User.Sabun);
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private bool ChkYakuk(string strSabun)
        {
            if (VB.Left(clsType.User.JobGroup, 6) == "JOB011")
            {
                return true;
            }
            else
            {
                return false;
            }
            //string SQL = "";
            //DataTable dt = null;
            //string SqlErr = "";
            //bool rtnVal = false;

            //try
            //{
            //    SQL = "";
            //    SQL = "SELECT* FROM " + ComNum.DB_ERP + "INSA_MST A, " + ComNum.DB_ERP + "INSA_CODE B";
            //    SQL = SQL + ComNum.VBLF + "    WHERE A.SABUN = '" + strSabun + "'";
            //    SQL = SQL + ComNum.VBLF + "        AND B.GUBUN = '2'";
            //    SQL = SQL + ComNum.VBLF + "        AND B.CODE IN ('40', '41', '42', '43')";
            //    SQL = SQL + ComNum.VBLF + "        AND A.JIK = TRIM(B.CODE)";

            //    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            //    if (SqlErr != "")
            //    {
            //        ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
            //        return rtnVal;
            //    }
            //    if (dt.Rows.Count > 0)
            //    {
            //        rtnVal = true;
            //    }

            //    dt.Dispose();
            //    dt = null;

            //    return rtnVal;
            //}
            //catch (Exception ex)
            //{
            //    if (dt != null)
            //    {
            //        dt.Dispose();
            //        dt = null;
            //    }

            //    ComFunc.MsgBox(ex.Message);
            //    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            //    Cursor.Current = Cursors.Default;
            //    return rtnVal;
            //}
        }

        private void setPatientInfo(FarPoint.Win.Spread.SheetView spd, string strIPDNO)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     PANO, SNAME, SEX, AGE, ROOMCODE, DEPTCODE, TO_CHAR(INDATE,'YYYY-MM-DD') INDATE";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER";
                SQL = SQL + ComNum.VBLF + "WHERE IPDNO = " + strIPDNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    spd.Cells[1, 1].Text  = dt.Rows[0]["PANO"].ToString().Trim();
                    spd.Cells[1, 6].Text  = dt.Rows[0]["SNAME"].ToString().Trim();
                    spd.Cells[1, 12].Text = dt.Rows[0]["SEX"].ToString().Trim() + "/" + dt.Rows[0]["AGE"].ToString().Trim();
                    spd.Cells[1, 18].Text = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    spd.Cells[1, 22].Text = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    spd.Cells[1, 30].Text = dt.Rows[0]["INDATE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.ILLCODE, B.ILLNAMEK";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_IILLS A, " + ComNum.DB_PMPA + "BAS_ILLS B";
                SQL = SQL + ComNum.VBLF + "WHERE A.ILLCODE = B.ILLCODE";
                SQL = SQL + ComNum.VBLF + "  AND A.IPDNO = " + strIPDNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    spd.Cells[2, 1].Text = dt.Rows[0]["ILLCODE"].ToString().Trim();
                    spd.Cells[2, 12].Text = dt.Rows[0]["ILLNAMEK"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                //ssSpread_Sheet.Cells[12, 8].Text = GstrBIGO;

                //ssSpread_Sheet.Cells[13, 3].Text = GstrWRITEDATE;
                //ssSpread_Sheet.Cells[13, 6].Text = GstrWRITESABUN;
                //ssSpread_Sheet.Cells[13, 9].Text = GstrDeptCode;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void setPatientInfoOpd(FarPoint.Win.Spread.SheetView spd, string strIPDNO)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {

                #region 이전 쿼리
                //SQL = "";                
                //SQL = SQL + ComNum.VBLF + "SELECT PANO, SNAME, SEX, AGE, '' ROOMCODE, DEPTCODE, TO_CHAR(ACTDATE, 'YYYY-MM-DD') ACTDATE ";
                //SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_MASTER                                                               ";
                //SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + GstrPTNO + "'                                                            ";
                //SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + GstrDeptCode + "'                                                    ";
                //SQL = SQL + ComNum.VBLF + "   AND ACTDATE = TO_DATE('" + GstrBDATE + "', 'YYYY-MM-DD')                                 ";

                //SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                //if (SqlErr != "")
                //{
                //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                //    return;
                //}
                //if (dt.Rows.Count > 0)
                //{
                //    spd.Cells[1, 1].Text  = dt.Rows[0]["PANO"].ToString().Trim();
                //    spd.Cells[1, 6].Text  = dt.Rows[0]["SNAME"].ToString().Trim();
                //    spd.Cells[1, 12].Text = dt.Rows[0]["SEX"].ToString().Trim() + "/" + dt.Rows[0]["AGE"].ToString().Trim();
                //    spd.Cells[1, 18].Text = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                //    spd.Cells[1, 22].Text = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                //    spd.Cells[1, 30].Text = dt.Rows[0]["ACTDATE"].ToString().Trim();
                //}

                //dt.Dispose();
                //dt = null;

                #endregion


                #region 신규쿼리 21-07-05
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT   PATID                                           ";
                SQL = SQL + ComNum.VBLF + "     ,   O.SNAME                                         ";
                SQL = SQL + ComNum.VBLF + "     ,   O.AGE                                           ";
                SQL = SQL + ComNum.VBLF + "     ,   O.SEX                                           ";
                SQL = SQL + ComNum.VBLF + "     ,   O.DEPTCODE                                      ";
                SQL = SQL + ComNum.VBLF + "     ,   B.DRNAME                                        ";
                SQL = SQL + ComNum.VBLF + "     ,   TO_CHAR(O.ACTDATE, 'YYYY-MM-DD') ACTDATE        ";
                SQL = SQL + ComNum.VBLF + "     ,   TO_CHAR(O.BDATE, 'YYYY-MM-DD') BDATE            ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.EMR_TREATT A                           ";
                SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_OCS.OCS_DOCTOR B                      ";
                SQL = SQL + ComNum.VBLF + "    ON A.DOCCODE = TRIM(B.DOCCODE)                       ";
                SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_PMPA.OPD_MASTER O                     ";
                SQL = SQL + ComNum.VBLF + "    ON B.DRCODE = O.DRCODE                               ";
                SQL = SQL + ComNum.VBLF + "   AND A.PATID  = TRIM(O.PANO)                           ";
                SQL = SQL + ComNum.VBLF + "   AND O.BDATE  = TO_DATE(A.INDATE, 'YYYYMMDD')          ";
                SQL = SQL + ComNum.VBLF + "WHERE A.TREATNO = " + strIPDNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    spd.Cells[1, 1].Text  = dt.Rows[0]["PATID"].ToString().Trim();
                    spd.Cells[1, 6].Text  = dt.Rows[0]["SNAME"].ToString().Trim();
                    spd.Cells[1, 12].Text = dt.Rows[0]["SEX"].ToString().Trim() + "/" + dt.Rows[0]["AGE"].ToString().Trim();
                    spd.Cells[1, 18].Text = "";
                    spd.Cells[1, 22].Text = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    spd.Cells[1, 30].Text = dt.Rows[0]["ACTDATE"].ToString().Trim();

                    GstrBDATE = dt.Rows[0]["BDATE"].ToString().Trim();
                    GstrDeptCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                #endregion



                SQL = "";                                
                SQL = SQL + ComNum.VBLF + "SELECT A.ILLCODE, B.ILLNAMEK                                 ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_OILLS A, KOSMOS_PMPA.BAS_ILLS B        ";
                SQL = SQL + ComNum.VBLF + " WHERE A.ILLCODE = B.ILLCODE                                 ";
                SQL = SQL + ComNum.VBLF + "   AND A.PTNO = '"+ GstrPTNO + "'                            ";
                SQL = SQL + ComNum.VBLF + "   AND A.BDATE = TO_DATE('"+ GstrBDATE + "', 'YYYY-MM-DD')   ";
                SQL = SQL + ComNum.VBLF + "   AND A.DEPTCODE = '"+ GstrDeptCode + "'                    ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SEQNO                                               ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    spd.Cells[2, 1].Text  = dt.Rows[0]["ILLCODE"].ToString().Trim();
                    spd.Cells[2, 12].Text = dt.Rows[0]["ILLNAMEK"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                //ssSpread_Sheet.Cells[12, 8].Text = GstrBIGO;

                //ssSpread_Sheet.Cells[13, 3].Text = GstrWRITEDATE;
                //ssSpread_Sheet.Cells[13, 6].Text = GstrWRITESABUN;
                //ssSpread_Sheet.Cells[13, 9].Text = GstrDeptCode;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void readPConsult(FarPoint.Win.Spread.SheetView spd, string strSEQNO)
        {
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";

            try
            {
                
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT                                                      ";
                SQL = SQL + ComNum.VBLF + "      IPDNO, PANO, USED, BIGO, PROGRESS,                     ";
                SQL = SQL + ComNum.VBLF + "      A.ORDERCODE, B.COMMENTS ORDERNAME,                     ";
                SQL = SQL + ComNum.VBLF + "      A.CONSULTSAYU, A.CONSULTSAYU_ETC,                      ";
                SQL = SQL + ComNum.VBLF + "      WRITESABUN, C.DRNAME, C.DEPTCODE,                      ";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(WRITEDATE, 'YYYY-MM-DD') AS BDATE,             ";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(WRITEDATE, 'YYYY-MM-DD HH24:MI') AS WRITEDATE  ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_ADM.DRUG_PCONSULT A                             ";
                SQL = SQL + ComNum.VBLF + "     ,(SELECT PART, JEPCODE, COMMENTS                        ";
                SQL = SQL + ComNum.VBLF + "         FROM KOSMOS_ADM.DRUG_SETCODE                        ";
                SQL = SQL + ComNum.VBLF + "        WHERE GUBUN = '11'                                   ";
                SQL = SQL + ComNum.VBLF + "          AND JEPCODE <> '분류명칭'                            ";
                SQL = SQL + ComNum.VBLF + "          AND DELDATE IS NULL) B                             ";
                SQL = SQL + ComNum.VBLF + "     , KOSMOS_OCS.OCS_DOCTOR C                               ";
                SQL = SQL + ComNum.VBLF + " WHERE A.ORDERCODE = B.JEPCODE                               ";
                SQL = SQL + ComNum.VBLF + "   AND A.WRITESABUN = C.SABUN(+)                             ";
                SQL = SQL + ComNum.VBLF + "   AND A.SEQNO = " + strSEQNO;
                
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    GstrIPDNO = dt.Rows[0]["IPDNO"].ToString().Trim();
                    GstrORDERCODE = dt.Rows[0]["ORDERCODE"].ToString().Trim();
                    //GstrBIGO = dt.Rows[0]["BIGO"].ToString().Trim();
                    GstrWRITEDATE = dt.Rows[0]["WRITEDATE"].ToString().Trim();
                    GstrWRITESABUN = dt.Rows[0]["WRITESABUN"].ToString().Trim();
                    GstrDeptCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();

                    spd.Cells[4, 1].Text = GstrWRITEDATE;

                    if (dt.Rows[0]["BIGO"].ToString().Trim().IndexOf("자동발생") == -1)
                    {
                        spd.Cells[4, 9].Text = dt.Rows[0]["DRNAME"].ToString().Trim();
                        spd.Cells[4, 15].Text = GstrDeptCode;
                    }

                    spd.Cells[5, 1].Text = "[" + dt.Rows[0]["ORDERCODE"].ToString().Trim() + "] " + dt.Rows[0]["ORDERNAME"].ToString().Trim();

                    switch (dt.Rows[0]["CONSULTSAYU"].ToString().Trim())
                    {
                        case "0":
                            spd.Cells[4, 20].Value = true;
                            break;
                        case "1":
                            spd.Cells[4, 25].Value = true;
                            break;
                        case "2":
                            spd.Cells[5, 20].Value = true;
                            break;
                        case "3":
                            spd.Cells[5, 25].Value = true;
                            spd.Cells[5, 27].Text = dt.Rows[0]["CONSULTSAYU_ETC"].ToString().Trim();
                            break;
                    }
                    
                    //if (dt.Rows[0]["USED"].ToString().Trim() == "1")
                    //{
                    //    spd.Cells[5, 12].Value = true;
                    //    spd.Cells[5, 15].Value = false;
                    //}
                    //else
                    //{
                    //    spd.Cells[5, 12].Value = false;
                    //    spd.Cells[5, 15].Value = true;
                    //}

                    spd.Cells[6, 1].Text = dt.Rows[0]["BIGO"].ToString().Trim();


                    if (GstrPART == "01")
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT MAX(BDATE), DOSCODE FROM KOSMOS_OCS.OCS_IORDER ";
                        SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + dt.Rows[0]["PANO"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND BDATE <= TO_DATE('" + dt.Rows[0]["BDATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND ORDERCODE = '" + dt.Rows[0]["ORDERCODE"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + " GROUP BY DOSCODE ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            spd.Cells[5, 14].Text = READ_DOSNAME(clsDB.DbCon, dt1.Rows[0]["DOSCODE"].ToString().Trim());
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }
                }


                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT * FROM KOSMOS_ADM.DRUG_PCONSULT_ROW ";
                SQL = SQL + ComNum.VBLF + "WHERE SEQNO = " + GstrSEQNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    //spd.Cells[8, 24].Text = clsType.User.UserName;                    
                    spd.Cells[8, 30].Text = clsType.User.UserName;
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            SAVE_DATA(GspdSheet);            
            UPDATE_PCONSULT_PROGRESS(clsDB.DbCon, "2", GspdSheet.Cells[4, 1].Text);
            UPDATE_PCONSULT_RTNSPD(clsDB.DbCon);
        }

        private void btnSaveC_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            SAVE_DATA(GspdSheet);
            UPDATE_PCONSULT_PROGRESS(clsDB.DbCon, "C", GspdSheet.Cells[4, 1].Text);
            UPDATE_PCONSULT_RTNSPD(clsDB.DbCon);
            DataScan();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DELETE_DATA(GspdSheet);
            UPDATE_PCONSULT_PROGRESS(clsDB.DbCon, "3", GspdSheet.Cells[4, 1].Text);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";
            string SysDate = "";

            SysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            Cursor.Current = Cursors.WaitCursor;

            strFont1 = "/fn\"굴림체\" /fz\"16\" /fb0 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"10\" /fb0 /fi0 /fu0 /fk0 /fs2";
            
            GspdSheet.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;  //가로            
            GspdSheet.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            GspdSheet.PrintInfo.Margin.Left = 25;
            GspdSheet.PrintInfo.Margin.Right = 0;
            GspdSheet.PrintInfo.Margin.Top = GspdSheet.Equals(ssView01.ActiveSheet) ? 25 : 35;
            GspdSheet.PrintInfo.Margin.Bottom = 30;
            GspdSheet.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            GspdSheet.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            GspdSheet.PrintInfo.ShowBorder = true;
            GspdSheet.PrintInfo.ShowColor = false;
            GspdSheet.PrintInfo.ShowGrid = true;
            GspdSheet.PrintInfo.ShowShadows = false;
            GspdSheet.PrintInfo.UseMax = false;
            if (GspdSheet.Equals(ssView02.ActiveSheet))
            {
                GspdSheet.PrintInfo.ZoomFactor = 0.9f;
            }
            GspdSheet.PrintInfo.Preview = false;
            GspdSheet.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;            
            Gspd.PrintSheet(Gspd.ActiveSheetIndex);

            Cursor.Current = Cursors.Default;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void READ_DATA(FarPoint.Win.Spread.SheetView spd)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strITEMCD = "";
            string strITEMTYPE = "";
            string strITEMVALUE = "";

            Cursor.Current = Cursors.WaitCursor;
            try
            {                
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT * FROM KOSMOS_ADM.DRUG_PCONSULT_ROW ";
                SQL = SQL + ComNum.VBLF + "WHERE SEQNO = " + GstrSEQNO;
                
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
                        strITEMCD = dt.Rows[i]["ITEMCD"].ToString().Trim();
                        strITEMTYPE = dt.Rows[i]["ITEMTYPE"].ToString().Trim();
                        strITEMVALUE = dt.Rows[i]["ITEMVALUE"].ToString().Trim();

                        SET_SPREAD(spd, strITEMCD, strITEMTYPE, strITEMVALUE);                        
                    }
                }
                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void SET_SPREAD(FarPoint.Win.Spread.SheetView spd, string strITEMCD, string strITEMTYPE, string ITEMVALUE)
        {
            for (int iRow = 0; iRow < spd.RowCount; iRow++)
            {
                for (int iCol = 0; iCol < spd.ColumnCount; iCol++)
                {
                    if (spd.Cells[iRow, iCol].Tag == null)
                    {
                        continue;
                    }

                    if (spd.Cells[iRow, iCol].Tag.ToString().Equals(strITEMCD))
                    {
                        if (strITEMTYPE.Equals("CHECK"))
                        {
                            spd.Cells[iRow, iCol].Text = ITEMVALUE.Equals("1").ToString();
                        }
                        else
                        {
                            spd.Cells[iRow, iCol].Text = ITEMVALUE;
                        }

                        return;
                    }
                }
            }
        }

        private bool SAVE_DATA(FarPoint.Win.Spread.SheetView spd)
        {
            //FarPoint.Win.Spread.CellType.CheckBoxCellType TypeCheckBox = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.TextCellType TypeText = new FarPoint.Win.Spread.CellType.TextCellType();

            bool rtnVal = false;
            string strITEMCD = "";
            string strITEMINDEX = "0";
            string strITEMTYPE = "";
            string strITEMVALUE = "";
            string strITEMVALUE1 = "";
            string strDSPSEQ = "0";


            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                DELETE_PCONSULT_ROW(clsDB.DbCon, GstrSEQNO);


                for (int i = 0; i < spd.RowCount; i++)
                {
                    for (int j = 0; j < spd.ColumnCount; j++)
                    {
                        if (spd.Cells[i, j].Tag == null)
                        {
                            continue;
                        }

                        if (spd.Cells[i, j].Tag.ToString() != "")
                        {
                            strITEMCD = spd.Cells[i, j].Tag.ToString();
                            if (spd.Cells[i, j].CellType != null  && spd.Cells[i, j].CellType.ToString().Equals("CheckBoxCellType"))
                            {
                                strITEMTYPE = "CHECK";
                                if (spd.Cells[i, j].Text.Trim().Equals("True"))
                                {
                                    strITEMVALUE = "1";
                                }
                                else
                                {
                                    strITEMVALUE = "0";
                                }
                            }
                            else
                            {
                                strITEMTYPE = "TEXT";
                                strITEMVALUE = spd.Cells[i, j].Text;
                            }

                            if (SAVE_PCONSULT_ROW(clsDB.DbCon, GstrSEQNO, strITEMCD, strITEMINDEX, strITEMTYPE, strITEMVALUE, strITEMVALUE1, strDSPSEQ) == false)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }                            
                        }
                    }
                }


                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);                
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private bool SAVE_PCONSULT_ROW(PsmhDb pDbCon, string strSEQNO, string strITEMCD, string strITEMINDEX, string strITEMTYPE, string strITEMVALUE, string strITEMVALUE1, string strDSPSEQ)
        {
            bool rtnVal = false;            
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO KOSMOS_ADM.DRUG_PCONSULT_ROW (";
                SQL = SQL + ComNum.VBLF + "     SEQNO, ITEMCD, ITEMINDEX, ITEMTYPE, ITEMVALUE, ITEMVALUE1, DSPSEQ";
                SQL = SQL + ComNum.VBLF + ") VALUES (";
                SQL = SQL + ComNum.VBLF + "      " + strSEQNO + ",";
                SQL = SQL + ComNum.VBLF + "     '" + strITEMCD + "',";
                SQL = SQL + ComNum.VBLF + "     '" + strITEMINDEX + "',";
                SQL = SQL + ComNum.VBLF + "     '" + strITEMTYPE + "',";
                SQL = SQL + ComNum.VBLF + "     '" + strITEMVALUE + "',";
                SQL = SQL + ComNum.VBLF + "     '" + strITEMVALUE1 + "',";
                SQL = SQL + ComNum.VBLF + "     '" + strDSPSEQ + "'";
                SQL = SQL + ComNum.VBLF + ")";
                
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {                                        
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");                    
                    return rtnVal;
                }

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {                
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }            
        }

        private bool DELETE_DATA(FarPoint.Win.Spread.SheetView spd)
        {            
            bool rtnVal = false;
            
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                DELETE_PCONSULT_ROW(clsDB.DbCon, GstrSEQNO);
                
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private bool DELETE_PCONSULT_ROW(PsmhDb pDbCon, string strSEQNO)
        {
            bool rtnVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "DELETE FROM KOSMOS_ADM.DRUG_PCONSULT_ROW ";
                SQL = SQL + ComNum.VBLF + "WHERE SEQNO = " + strSEQNO;
                
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    return rtnVal;
                }

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            READ_DATA(GspdSheet);
        }

        private string READ_USED_PCONSULT()
        {            
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "0";
            
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "";                
                SQL = SQL + ComNum.VBLF + "SELECT RTN_SPD FROM KOSMOS_ADM.DRUG_PCONSULT ";
                SQL = SQL + ComNum.VBLF + "WHERE SEQNO = " + GstrSEQNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["RTN_SPD"].ToString().Trim();                    
                }
                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strValue">1: 접수, 2: 임시저장, 3: 취소(삭제), C: 완료</param>
        /// <param name="strDate"></param>
        /// <returns></returns>
        private bool UPDATE_PCONSULT_PROGRESS(PsmhDb pDbCon, string strValue, string strDate)
        {
            bool rtnVal = false;
            //bool bolAuto = false;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;

            try
            {
                if (strValue == "3")
                {
                    // 삭제
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT BIGO FROM KOSMOS_ADM.DRUG_PCONSULT ";
                    SQL = SQL + ComNum.VBLF + " WHERE SEQNO = '" + GstrSEQNO + "'        ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["BIGO"].ToString().Trim().IndexOf("자동발생") != -1)
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "DELETE FROM KOSMOS_ADM.DRUG_PCONSULT ";
                            SQL = SQL + ComNum.VBLF + " WHERE SEQNO = '" + GstrSEQNO + "'    ";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                return rtnVal;
                            }
                        }
                        else
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "UPDATE KOSMOS_ADM.DRUG_PCONSULT SET ";
                            SQL = SQL + ComNum.VBLF + "     WRITEDATE = TO_DATE('" + strDate + "','YYYY-MM-DD'), ";
                            SQL = SQL + ComNum.VBLF + "     PROGRESS = '" + strValue + "'";
                            SQL = SQL + ComNum.VBLF + "WHERE SEQNO = '" + GstrSEQNO + "'";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                return rtnVal;
                            }
                        }
                    }
                }
                else
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "UPDATE KOSMOS_ADM.DRUG_PCONSULT SET ";
                    SQL = SQL + ComNum.VBLF + "     WRITEDATE = TO_DATE('" + strDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "     PROGRESS = '" + strValue + "'";
                    SQL = SQL + ComNum.VBLF + "WHERE SEQNO = '" + GstrSEQNO + "'";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        return rtnVal;
                    }
                }

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private bool UPDATE_PCONSULT_RTNSPD(PsmhDb pDbCon)
        {
            bool rtnVal = false;            
            string SQL = "";            
            string SqlErr = "";
            int intRowAffected = 0;

            try
            {
                
                SQL = "";
                SQL = SQL + ComNum.VBLF + "UPDATE KOSMOS_ADM.DRUG_PCONSULT SET ";                
                SQL = SQL + ComNum.VBLF + "     RTN_SPD = '" + GstrRTN_SPD + "'";
                SQL = SQL + ComNum.VBLF + "WHERE SEQNO = '" + GstrSEQNO + "'";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    return rtnVal;
                }
                
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private string READ_DOSNAME(PsmhDb pDbCon, string strSuCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     IDOSNAME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_ODOSAGE";
                SQL = SQL + ComNum.VBLF + "     WHERE DOSCODE = '" + strSuCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["IDOSNAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

                return rtnVal;
            }
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            
        }

        private void DataScan()
        {
            string strPano = "";
            string strInOutCls = "";
            string strInDate = "";
            string strDeptCode = "";
            string strBdate = "";
            string strDrugName = "";

            if (GspdSheet.Cells[8, 1].Text.Trim() == "")
            {
                if (MessageBox.Show("회신일이 없습니다. 이미지변환을 하시겠습니까?", "회신일 없음", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
            }

            strPano = GspdSheet.Cells[1, 1].Text.Trim();
            strDeptCode = GspdSheet.Cells[1, 22].Text.Trim();
            strInDate = GspdSheet.Cells[1, 30].Text.Trim();
            strBdate = GspdSheet.Cells[4, 1].Text.Trim();   // 의뢰일
            strDrugName = GspdSheet.Cells[5, 1].Text.Trim();   // 대상의약품
            if (GstrIPDNO != "")
            {
                strInOutCls = "I";
            }
            else
            {
                strInOutCls = "O";
            }

            ImgCVTDrug(strPano, strInOutCls, strInDate, strDeptCode, strBdate, strDrugName);
        }


        private void ImgCVTDrug(string strPano, string strInOutCls, string strInDate, string strDeptCode, string strBdate, string strDrugName)
        {
            string strOutDate = string.Empty;
            string strTreatNo = string.Empty;
            clsImgcvt.NEW_PohangTreatInterface(clsDB.DbCon, this, strPano);
            clsImgcvt.GetPatIpdInfo(clsDB.DbCon, strPano, strInOutCls, strInDate, strDeptCode, ref strTreatNo, ref strOutDate);

            clsImgcvt.CreateSaveFolder();
            clsScan.DeleteFoldAll(@"C:\PSMHEXE\IMGCVT");

            if (clsImgcvt.IsDRUGCvt(clsDB.DbCon, strTreatNo, strBdate, strDrugName))
            {
                if (ComFunc.MsgBoxQEx(this, "해당 회신일자로 변환된 내역이 있습니다\r\n삭제후 다시 변환 하시겠습니까?") == DialogResult.No)
                {
                    return;
                }
                else
                {
                    clsImgcvt.DelDRUGCvt(clsDB.DbCon, strTreatNo, strBdate, strDrugName);
                }
            }

            long PageNum = 1;
            GspdSheet.PrintInfo.Centering = Centering.Both;
            GspdSheet.PrintInfo.ShowColumnHeader = PrintHeader.Hide;
            GspdSheet.PrintInfo.ShowRowHeader = PrintHeader.Hide;
            GspdSheet.PrintInfo.ShowBorder = true;
            GspdSheet.PrintInfo.ShowColor = true;
            GspdSheet.PrintInfo.ShowGrid = false;
            GspdSheet.PrintInfo.ShowShadows = true;
            GspdSheet.PrintInfo.UseMax = false;
            GspdSheet.PrintInfo.Preview = false;
            GspdSheet.PrintInfo.PrintType = PrintType.All;
            //GspdSheet.ActiveSheet.PrintInfo.ZoomFactor = 1.8f;
            GspdSheet.PrintInfo.ZoomFactor = 1.4f;
            clsImgcvt.SpreadToImg2(clsDB.DbCon, Gspd, strPano, strInDate, strDeptCode, "154", ref PageNum); 

            clsImgcvt.DRUGInfo.GBN = "복약";
            clsImgcvt.DRUGInfo.REQUESTDATE = strBdate;
            clsImgcvt.DRUGInfo.TREATNO = strTreatNo;
            clsImgcvt.DRUGInfo.DRUGNAME = strDrugName;

            string[] dirs = Directory.GetFiles(@"C:\PSMHEXE\IMGCVT", "*.tif");
            if (dirs.Length > 0 && strTreatNo.Equals("0") == false)
            {
                if (string.IsNullOrWhiteSpace(strOutDate))
                {
                    strOutDate = strInDate.Replace("-","");
                }

                if (clsImgcvt.ADO_LabUpload(clsDB.DbCon, clsImgcvt.gstrFormcode, strTreatNo, strOutDate, 99, dirs, false, true))
                {
                    ComFunc.MsgBoxEx(this, "복약상담회신서가 정상적으로 변환되었습니다. EMR 뷰어에서 반드시 재확인해주세요!!");
                }
                else
                {
                    ComFunc.MsgBoxEx(this, "복약상담회신서가 변환 도중 오류가 발생했습니다 재변환해주세요.");
                }
                clsScan.DeleteFoldAll(@"C:\PSMHEXE\IMGCVT");
            }
        }
    }
}
