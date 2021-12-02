using System;

//namespace 선언
using System.Data;
using System.Windows.Forms;
using ComBase;
using ComDbB;
using ComLibB;


/// <summary>
/// Description : BAS모듈
/// Author : 박병규
/// Create Date : 
/// </summary>
/// <history>
/// </history>
/// <seealso cref="basAcct.bas"/> 

namespace ComPmpaLibB
{

    public class clsBasAcct : MTSDisposable
    {
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        //ComFunc CF = new ComFunc();

        #region 외래본인부담율
        public void Bas_Opd_Bon()
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";

            string strDate1 = "";
            string strDate2 = "";
            string strAcctDate = "";
            int i, j, k;

            if (clsPmpaPb.GstrMirFlag == "OK")
                strAcctDate = clsPmpaPb.GstrMirDate;
            else
                strAcctDate = clsPublic.GstrSysDate;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(STARTDATE, 'YYYY-MM-DD') SDATE";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ACCOUNT";
                SQL += ComNum.VBLF + "  WHERE 1 = 1";
                SQL += ComNum.VBLF + "    AND IDNAME    = 'OPD_BON'";
                SQL += ComNum.VBLF + "    AND STARTDATE <= TO_DATE('" + strAcctDate + "', 'YYYY-MM-DD')";
                SQL += ComNum.VBLF + "  GROUP BY STARTDATE";
                SQL += ComNum.VBLF + "  ORDER BY STARTDATE DESC";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                strDate1 = Dt.Rows[0]["SDATE"].ToString();

                if (Dt.Rows.Count > 1)
                {
                    strDate2 = Dt.Rows[1]["SDATE"].ToString();
                }

                clsPmpaPb.OBON_DATE = strDate1;

                Dt.Dispose();
                Dt = null;

                SQL = "";
                SQL += ComNum.VBLF + " SELECT ARRAYCLASS, RATEVALUE";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ACCOUNT";
                SQL += ComNum.VBLF + "  WHERE 1 = 1";
                SQL += ComNum.VBLF + "    AND IDNAME    = 'OPD_BON'";
                SQL += ComNum.VBLF + "    AND STARTDATE = TO_DATE('" + strDate1 + "', 'YYYY-MM-DD')";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        j = Convert.ToInt32(Dt.Rows[i]["ARRAYCLASS"].ToString());
                        k = Convert.ToInt32(Dt.Rows[i]["RATEVALUE"].ToString());
                        clsPmpaPb.OBON[j] = k;
                    }
                }

                Dt.Dispose();
                Dt = null;

                if (strDate2 != "")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT ARRAYCLASS, RATEVALUE";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ACCOUNT";
                    SQL += ComNum.VBLF + "  WHERE 1 = 1";
                    SQL += ComNum.VBLF + "    AND IDNAME    = 'OPD_BON'";
                    SQL += ComNum.VBLF + "    AND STARTDATE = TO_DATE('" + strDate2 + "', 'YYYY-MM-DD')";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                    if (Dt.Rows.Count > 0)
                    {
                        for (i = 0; i < Dt.Rows.Count; i++)
                        {
                            j = Convert.ToInt32(Dt.Rows[i]["ARRAYCLASS"].ToString());
                            k = Convert.ToInt32(Dt.Rows[i]["RATEVALUE"].ToString());

                            clsPmpaPb.OLD_OBON[j] = k;
                        }
                    }

                    Dt.Dispose();
                    Dt = null;
                }

                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;

            }
        }
        #endregion

        #region 입원본인부담율     
        public void Bas_Ipd_Bon()
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";

            string strDate = "";
            string strAcctDate = "";
            int i, j, k;

            if (clsPmpaPb.GstrMirFlag == "OK")
                strAcctDate = clsPmpaPb.GstrMirDate;
            else
                strAcctDate = clsPublic.GstrSysDate;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(STARTDATE, 'YYYY-MM-DD') SDATE";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ACCOUNT";
                SQL += ComNum.VBLF + "  WHERE 1 = 1   ";
                SQL += ComNum.VBLF + "    AND IDNAME    = 'IPD_BON'";
                SQL += ComNum.VBLF + "    AND STARTDATE <= TO_DATE('" + strAcctDate + "', 'YYYY-MM-DD')";
                SQL += ComNum.VBLF + "  GROUP BY STARTDATE ";
                SQL += ComNum.VBLF + "  ORDER BY STARTDATE DESC ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                strDate = Dt.Rows[0]["SDATE"].ToString();

                Dt.Dispose();
                Dt = null;

                SQL = "";
                SQL += ComNum.VBLF + " SELECT ARRAYCLASS, RATEVALUE ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_ACCOUNT ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1  ";
                SQL += ComNum.VBLF + "    AND IDNAME    = 'IPD_BON' ";
                SQL += ComNum.VBLF + "    AND STARTDATE = TO_DATE('" + strDate + "', 'YYYY-MM-DD')   ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                for (i = 0; i < Dt.Rows.Count; i++)
                {
                    j = Convert.ToInt32(Dt.Rows[i]["ARRAYCLASS"].ToString());
                    k = Convert.ToInt32(Dt.Rows[i]["RATEVALUE"].ToString());

                    clsPmpaPb.IBON[j] = k;
                }

                Dt.Dispose();
                Dt = null;

                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }
        }
        #endregion

        #region 중증질환 본인부담율  //READ_Cancer_BonRate
        public int Read_Cancer_BonRate(string ArgBi, string ArgDate, string ArgVCode)
        {
            int rtnVal = 0;

            DataTable DtFunc = new DataTable();
            string SQL = "";
            string SqlErr = "";

            if (string.Compare(ArgBi, "25") > 0)
                return -1;

            //DB에 저장된 값을 읽음
            SQL = "";
            SQL += ComNum.VBLF + " SELECT RateValue,TO_CHAR(StartDate,'YYYY-MM-DD') StartDate ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ACCOUNT ";
            SQL += ComNum.VBLF + "  WHERE IdName = 'JUNG_BON' ";
            SQL += ComNum.VBLF + "    AND StartDate <= TO_DATE('" + ArgDate.Trim() + "','YYYY-MM-DD') ";
            switch (ArgVCode)
            {
                case "V191":
                    SQL += ComNum.VBLF + " AND ArrayClass=1 "; //개두술
                    break;
                case "V192":
                    SQL += ComNum.VBLF + " AND ArrayClass=2 "; //개심술
                    break;
                case "V193":
                    SQL += ComNum.VBLF + " AND ArrayClass=3 ";  //등록암
                    break;
                case "V194":
                    SQL += ComNum.VBLF + " AND ArrayClass=4 ";  //등록암 가정간호
                    break;
                case "F003":
                    SQL += ComNum.VBLF + " AND ArrayClass=5 ";  //의약분업 환자 약값 30%
                    break;
                case "V268":
                    SQL += ComNum.VBLF + " AND ArrayClass=6 ";  //뇌출혈 2015-04-06
                    break;
                case "V275":
                    SQL += ComNum.VBLF + " AND ArrayClass=7 ";  //뇌경색 2016-07-28
                    break;
                default:
                    rtnVal = -1;
                    break;
            }
            SQL += ComNum.VBLF + " ORDER BY StartDate DESC ";
            SqlErr = clsDB.GetDataTable(ref DtFunc, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                return -1;
            }

            if (DtFunc.Rows.Count == 0)
            {
                DtFunc.Dispose();
                DtFunc = null;
                return -1;
            }

            rtnVal = Convert.ToUInt16(DtFunc.Rows[0]["RateValue"].ToString());
            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }
        #endregion

        #region 내복약조제료 일수
        public void Bas_Joje()
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";

            string strDate1 = "";
            string strDate2 = "";
            string strAcctDate = "";
            int i, j, k;

            if (clsPmpaPb.GstrMirFlag == "OK")
                strAcctDate = clsPmpaPb.GstrMirDate;
            else
                strAcctDate = clsPublic.GstrSysDate;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(STARTDATE, 'YYYY-MM-DD') SDATE ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_ACCOUNT ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1   ";
                SQL += ComNum.VBLF + "    AND IDNAME    = 'JOJE' ";
                SQL += ComNum.VBLF + "    AND STARTDATE <= TO_DATE('" + strAcctDate + "', 'YYYY-MM-DD')";
                SQL += ComNum.VBLF + "  GROUP BY STARTDATE   ";
                SQL += ComNum.VBLF + "  ORDER BY STARTDATE DESC ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                strDate1 = Dt.Rows[0]["SDATE"].ToString();

                if (Dt.Rows.Count > 1)
                {
                    strDate2 = Dt.Rows[1]["SDATE"].ToString();
                }

                clsPmpaPb.JOJE_DATE = strDate1;

                Dt.Dispose();
                Dt = null;

                SQL = "";
                SQL += ComNum.VBLF + " SELECT ARRAYCLASS, RATEVALUE  ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ACCOUNT";
                SQL += ComNum.VBLF + "  WHERE 1 = 1                ";
                SQL += ComNum.VBLF + "    AND IDNAME    = 'JOJE'   ";
                SQL += ComNum.VBLF + "    AND STARTDATE = TO_DATE('" + strDate1 + "', 'YYYY-MM-DD')";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                for (i = 0; i < Dt.Rows.Count; i++)
                {
                    j = Convert.ToInt32(Dt.Rows[i]["ARRAYCLASS"].ToString());
                    k = Convert.ToInt32(Dt.Rows[i]["RATEVALUE"].ToString());

                    clsPmpaPb.JOJE[j] = k;
                }

                Dt.Dispose();
                Dt = null;

                if (strDate2 != "")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT ARRAYCLASS, RATEVALUE";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ACCOUNT";
                    SQL += ComNum.VBLF + "  WHERE 1 = 1";
                    SQL += ComNum.VBLF + "    AND IDNAME    = 'JOJE'";
                    SQL += ComNum.VBLF + "    AND STARTDATE = TO_DATE('" + strDate2 + "', 'YYYY-MM-DD')";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        j = Convert.ToInt32(Dt.Rows[i]["ARRAYCLASS"].ToString());
                        k = Convert.ToInt32(Dt.Rows[i]["RATEVALUE"].ToString());

                        clsPmpaPb.OLD_JOJE[j] = k;
                    }

                    Dt.Dispose();
                    Dt = null;
                }

                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }
        }
        #endregion

        #region 기술료가산율
        public void Bas_Gisul()
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";

            string strDate1 = "";
            string strDate2 = "";
            string strAcctDate = "";
            int i, j, k;

            if (clsPmpaPb.GstrMirFlag == "OK")
                strAcctDate = clsPmpaPb.GstrMirDate;
            else
                strAcctDate = clsPublic.GstrSysDate;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(STARTDATE, 'YYYY-MM-DD') SDATE";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ACCOUNT";
                SQL += ComNum.VBLF + "  WHERE 1 = 1";
                SQL += ComNum.VBLF + "    AND IDNAME    = 'GISUL'";
                SQL += ComNum.VBLF + "    AND STARTDATE <= TO_DATE('" + strAcctDate + "', 'YYYY-MM-DD')";
                SQL += ComNum.VBLF + "  GROUP BY STARTDATE ";
                SQL += ComNum.VBLF + "  ORDER BY STARTDATE DESC";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                strDate1 = Dt.Rows[0]["SDATE"].ToString();

                if (Dt.Rows.Count > 1)
                {
                    strDate2 = Dt.Rows[1]["SDATE"].ToString();
                }

                clsPmpaPb.GISUL_DATE = strDate1;

                Dt.Dispose();
                Dt = null;

                SQL = "";
                SQL += ComNum.VBLF + " SELECT ARRAYCLASS, RATEVALUE ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_ACCOUNT ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND IDNAME    = 'GISUL'";
                SQL += ComNum.VBLF + "    AND STARTDATE = TO_DATE('" + strDate1 + "', 'YYYY-MM-DD')";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                for (i = 0; i < Dt.Rows.Count; i++)
                {
                    j = Convert.ToInt32(Dt.Rows[i]["ARRAYCLASS"].ToString());
                    k = Convert.ToInt32(Dt.Rows[i]["RATEVALUE"].ToString());

                    clsPmpaPb.GISUL[j] = k;
                }

                Dt.Dispose();
                Dt = null;

                if (strDate2 != "")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT ARRAYCLASS, RATEVALUE ";
                    SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_ACCOUNT ";
                    SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                    SQL += ComNum.VBLF + "    AND IDNAME    = 'GISUL' ";
                    SQL += ComNum.VBLF + "    AND STARTDATE = TO_DATE('" + strDate2 + "', 'YYYY-MM-DD') ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        j = Convert.ToInt32(Dt.Rows[i]["ARRAYCLASS"].ToString());
                        k = Convert.ToInt32(Dt.Rows[i]["RATEVALUE"].ToString());

                        clsPmpaPb.OLD_GISUL[j] = k;
                    }

                    Dt.Dispose();
                    Dt = null;
                }

                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }
        }
        #endregion

        #region 심야가산율
        public void Bas_Night()
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";

            string strDate1 = "";
            string strDate2 = "";
            string strAcctDate = "";
            int i, j, k;

            if (clsPmpaPb.GstrMirFlag == "OK")
                strAcctDate = clsPmpaPb.GstrMirDate;
            else
                strAcctDate = clsPublic.GstrSysDate;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(STARTDATE, 'YYYY-MM-DD') SDATE";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ACCOUNT";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND IDNAME    = 'NIGHT' ";
                SQL += ComNum.VBLF + "    AND STARTDATE <= TO_DATE('" + strAcctDate + "', 'YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  GROUP BY STARTDATE  ";
                SQL += ComNum.VBLF + " ORDER BY STARTDATE DESC ";
                //  SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, clsDB.DbCon);

                strDate1 = Dt.Rows[0]["SDATE"].ToString();

                if (Dt.Rows.Count > 1)
                {
                    strDate2 = Dt.Rows[1]["SDATE"].ToString();
                }

                clsPmpaPb.NIGHT_DATE = strDate1;

                Dt.Dispose();
                Dt = null;

                SQL = "";
                SQL += ComNum.VBLF + " SELECT ARRAYCLASS, RATEVALUE";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ACCOUNT";
                SQL += ComNum.VBLF + "  WHERE 1= 1 ";
                SQL += ComNum.VBLF + "    AND IDNAME    = 'NIGHT'";
                SQL += ComNum.VBLF + "    AND STARTDATE = TO_DATE('" + strDate1 + "', 'YYYY-MM-DD')";
               // SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, clsDB.DbCon);

                for (i = 0; i < Dt.Rows.Count; i++)
                {
                    j = Convert.ToInt32(Dt.Rows[i]["ARRAYCLASS"].ToString());
                    k = Convert.ToInt32(Dt.Rows[i]["RATEVALUE"].ToString());

                    clsPmpaPb.NIGHT[j] = k;
                }

                Dt.Dispose();
                Dt = null;

                if (strDate2 != "")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT ARRAYCLASS, RATEVALUE ";
                    SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_ACCOUNT ";
                    SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                    SQL += ComNum.VBLF + "    AND IDNAME    = 'NIGHT' ";
                    SQL += ComNum.VBLF + "    AND STARTDATE = TO_DATE('" + strDate2 + "', 'YYYY-MM-DD')";
                    //SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                    SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, clsDB.DbCon);

                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        j = Convert.ToInt32(Dt.Rows[i]["ARRAYCLASS"].ToString());
                        k = Convert.ToInt32(Dt.Rows[i]["RATEVALUE"].ToString());

                        clsPmpaPb.OLD_NIGHT[j] = k;
                    }

                    Dt.Dispose();
                    Dt = null;
                }

                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }
        }
        #endregion

        #region 심야중복가산율(신생아, 소아, 노인)
        public void Bas_Night_22()
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";

            string strDate1 = "";
            string strDate2 = "";
            string strAcctDate = "";
            int i, j, k;

            if (clsPmpaPb.GstrMirFlag == "OK")
                strAcctDate = clsPmpaPb.GstrMirDate;
            else
                strAcctDate = clsPublic.GstrSysDate;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(STARTDATE, 'YYYY-MM-DD') SDATE";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_ACCOUNT ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND IDNAME    = 'NIGHT_22' ";
                SQL += ComNum.VBLF + "    AND STARTDATE <= TO_DATE('" + strAcctDate + "', 'YYYY-MM-DD')";
                SQL += ComNum.VBLF + "  GROUP BY STARTDATE  ";
                SQL += ComNum.VBLF + " ORDER BY STARTDATE DESC ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                strDate1 = Dt.Rows[0]["SDATE"].ToString();

                if (Dt.Rows.Count > 1)
                {
                    strDate2 = Dt.Rows[1]["SDATE"].ToString();
                }

                clsPmpaPb.NGT22_DATE = strDate1;

                Dt.Dispose();
                Dt = null;

                SQL = "";
                SQL += ComNum.VBLF + " SELECT ARRAYCLASS, RATEVALUE ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_ACCOUNT ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND IDNAME    = 'NIGHT_22' ";
                SQL += ComNum.VBLF + "    AND STARTDATE = TO_DATE('" + strDate1 + "', 'YYYY-MM-DD')";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                for (i = 0; i < Dt.Rows.Count; i++)
                {
                    j = Convert.ToInt32(Dt.Rows[i]["ARRAYCLASS"].ToString());
                    k = Convert.ToInt32(Dt.Rows[i]["RATEVALUE"].ToString());

                    clsPmpaPb.NIGHT_22[j] = k;
                }

                Dt.Dispose();
                Dt = null;

                if (strDate2 != "")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT ARRAYCLASS, RATEVALUE ";
                    SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_ACCOUNT ";
                    SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                    SQL += ComNum.VBLF + "    AND IDNAME    = 'NIGHT_22'";
                    SQL += ComNum.VBLF + "    AND STARTDATE = TO_DATE('" + strDate2 + "', 'YYYY-MM-DD') ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        j = Convert.ToInt32(Dt.Rows[i]["ARRAYCLASS"].ToString());
                        k = Convert.ToInt32(Dt.Rows[i]["RATEVALUE"].ToString());

                        clsPmpaPb.OLD_NIGHT_22[j] = k;
                    }

                    Dt.Dispose();
                    Dt = null;
                }

                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }
        }
        #endregion

        #region 감액율
        public void Bas_Gamek()
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";

            string strDate = "";
            string strAcctDate = "";
            int i, j, k;

            if (clsPmpaPb.GstrMirFlag == "OK")
                strAcctDate = clsPmpaPb.GstrMirDate;
            else
                strAcctDate = clsPublic.GstrSysDate;

            try
            {
                //진찰료감액
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(STARTDATE, 'YYYY-MM-DD') SDATE";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_ACCOUNT  ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1  ";
                SQL += ComNum.VBLF + "    AND IDNAME    = 'GAMEK_JIN' ";
                SQL += ComNum.VBLF + "    AND STARTDATE <= TO_DATE('" + strAcctDate + "', 'YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  GROUP BY STARTDATE ";
                SQL += ComNum.VBLF + " ORDER BY STARTDATE DESC ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (Dt.Rows.Count > 1)
                    strDate = Dt.Rows[0]["SDATE"].ToString();

                Dt.Dispose();
                Dt = null;

                SQL = "";
                SQL += ComNum.VBLF + " SELECT ARRAYCLASS, RATEVALUE ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_ACCOUNT ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND IDNAME    = 'GAMEK_JIN' ";
                SQL += ComNum.VBLF + "    AND STARTDATE = TO_DATE('" + strDate + "', 'YYYY-MM-DD')";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (Dt.Rows.Count > 1)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        j = Convert.ToInt32(Dt.Rows[i]["ARRAYCLASS"].ToString());
                        k = Convert.ToInt32(Dt.Rows[i]["RATEVALUE"].ToString());

                        clsPmpaPb.GAMEK_JIN[j] = k;
                    }
                }

                Dt.Dispose();
                Dt = null;

                //보험 감액율
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(STARTDATE, 'YYYY-MM-DD') SDATE";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_ACCOUNT ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND IDNAME    = 'GAM_BOHUM' ";
                SQL += ComNum.VBLF + "    AND STARTDATE <= TO_DATE('" + strAcctDate + "', 'YYYY-MM-DD')";
                SQL += ComNum.VBLF + "  GROUP BY STARTDATE ";
                SQL += ComNum.VBLF + " ORDER BY STARTDATE DESC ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (Dt.Rows.Count > 0)
                    strDate = Dt.Rows[0]["SDATE"].ToString();

                Dt.Dispose();
                Dt = null;

                SQL = "";
                SQL += ComNum.VBLF + " SELECT ARRAYCLASS, RATEVALUE ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_ACCOUNT ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND IDNAME    = 'GAM_BOHUM' ";
                SQL += ComNum.VBLF + "    AND STARTDATE = TO_DATE('" + strDate + "', 'YYYY-MM-DD') ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        j = Convert.ToInt32(Dt.Rows[i]["ARRAYCLASS"].ToString());
                        k = Convert.ToInt32(Dt.Rows[i]["RATEVALUE"].ToString());

                        clsPmpaPb.BGAMEK[j] = k;
                    }
                }

                Dt.Dispose();
                Dt = null;

                //일반 감액율
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(STARTDATE, 'YYYY-MM-DD') SDATE ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_ACCOUNT ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND IDNAME    = 'GAM_ILBAN' ";
                SQL += ComNum.VBLF + "    AND STARTDATE <= TO_DATE('" + strAcctDate + "', 'YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  GROUP BY STARTDATE  ";
                SQL += ComNum.VBLF + " ORDER BY STARTDATE DESC ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (Dt.Rows.Count > 0)
                    strDate = Dt.Rows[0]["SDATE"].ToString();

                Dt.Dispose();
                Dt = null;

                SQL = "";
                SQL += ComNum.VBLF + " SELECT ARRAYCLASS, RATEVALUE ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_ACCOUNT ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND IDNAME    = 'GAM_ILBAN' ";
                SQL += ComNum.VBLF + "    AND STARTDATE = TO_DATE('" + strDate + "', 'YYYY-MM-DD')";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        j = Convert.ToInt32(Dt.Rows[i]["ARRAYCLASS"].ToString());
                        k = Convert.ToInt32(Dt.Rows[i]["RATEVALUE"].ToString());

                        clsPmpaPb.IGAMEK[j] = k;
                    }
                }

                Dt.Dispose();
                Dt = null;

                //보험100% 감액율
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(STARTDATE, 'YYYY-MM-DD') SDATE";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_ACCOUNT ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND IDNAME    = 'GAM_OPD'";
                SQL += ComNum.VBLF + "    AND STARTDATE <= TO_DATE('" + strAcctDate + "', 'YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  GROUP BY STARTDATE ";
                SQL += ComNum.VBLF + " ORDER BY STARTDATE DESC ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (Dt.Rows.Count > 0)
                    strDate = Dt.Rows[0]["SDATE"].ToString();

                Dt.Dispose();
                Dt = null;

                SQL = "";
                SQL += ComNum.VBLF + " SELECT ARRAYCLASS, RATEVALUE";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_ACCOUNT ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1";
                SQL += ComNum.VBLF + "    AND IDNAME    = 'GAM_OPD'";
                SQL += ComNum.VBLF + "    AND STARTDATE = TO_DATE('" + strDate + "', 'YYYY-MM-DD')";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        j = Convert.ToInt32(Dt.Rows[i]["ARRAYCLASS"].ToString());
                        k = Convert.ToInt32(Dt.Rows[i]["RATEVALUE"].ToString());

                        clsPmpaPb.OGAMEK[j] = k;
                    }
                }

                Dt.Dispose();
                Dt = null;

                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        #endregion

        #region 소아가산율
        public void Bas_PedAdd()
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";

            string strDate1 = "";
            string strDate2 = "";
            string strAcctDate = "";
            string strArrClass = "";
            int i;

            if (clsPmpaPb.GstrMirFlag == "OK")
                strAcctDate = clsPmpaPb.GstrMirDate;
            else
                strAcctDate = clsPublic.GstrSysDate;

            clsPmpaPb.PedAdd_Date = "";
            clsPmpaPb.PedAdd1 = 0;
            clsPmpaPb.PedAdd2 = 0;
            clsPmpaPb.PedAdd3 = 0;
            clsPmpaPb.PedAdd4 = 0;
            clsPmpaPb.PedAdd5 = 0;
            clsPmpaPb.PedAdd6 = 0;
            clsPmpaPb.Old_PedAdd1 = 0;
            clsPmpaPb.Old_PedAdd2 = 0;


            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(STARTDATE, 'YYYY-MM-DD') SDATE ";
            SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_ACCOUNT ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND IDNAME    = 'PEDADD' ";
            SQL += ComNum.VBLF + "    AND STARTDATE <= TO_DATE('" + strAcctDate + "', 'YYYY-MM-DD')";
            SQL += ComNum.VBLF + "  GROUP BY STARTDATE ";
            SQL += ComNum.VBLF + " ORDER BY STARTDATE DESC ";
            // SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
            SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, clsDB.DbCon);

            strDate1 = Dt.Rows[0]["SDATE"].ToString();
            clsPmpaPb.PedAdd_Date = strDate1;

            if (Dt.Rows.Count > 1)
                strDate2 = Dt.Rows[1]["SDATE"].ToString();

            Dt.Dispose();
            Dt = null;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT ARRAYCLASS, RATEVALUE  ";
            SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_ACCOUNT  ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1  ";
            SQL += ComNum.VBLF + "    AND IDNAME    = 'PEDADD' ";
            SQL += ComNum.VBLF + "    AND STARTDATE = TO_DATE('" + strDate1 + "', 'YYYY-MM-DD') ";
            //  SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
            SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, clsDB.DbCon);
            for (i = 0; i < Dt.Rows.Count; i++)
            {
                strArrClass = Dt.Rows[i]["ARRAYCLASS"].ToString();

                switch (strArrClass.Trim())
                {
                    case "1":
                        clsPmpaPb.PedAdd1 = Convert.ToInt32(Dt.Rows[i]["RATEVALUE"].ToString()); break;
                    case "2":
                        clsPmpaPb.PedAdd2 = Convert.ToInt32(Dt.Rows[i]["RATEVALUE"].ToString()); break;
                    case "3":
                        clsPmpaPb.PedAdd3 = Convert.ToInt32(Dt.Rows[i]["RATEVALUE"].ToString()); break;
                    case "4":
                        clsPmpaPb.PedAdd4 = Convert.ToInt32(Dt.Rows[i]["RATEVALUE"].ToString()); break;
                    case "5":
                        clsPmpaPb.PedAdd5 = Convert.ToInt32(Dt.Rows[i]["RATEVALUE"].ToString()); break;
                    case "6":
                        clsPmpaPb.PedAdd6 = Convert.ToInt32(Dt.Rows[i]["RATEVALUE"].ToString()); break;
                }
            }

            Dt.Dispose();
            Dt = null;

            if (strDate2 != "")
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT ARRAYCLASS, RATEVALUE ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_ACCOUNT ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1  ";
                SQL += ComNum.VBLF + "    AND IDNAME    = 'PEDADD' ";
                SQL += ComNum.VBLF + "    AND STARTDATE = TO_DATE('" + strDate2 + "', 'YYYY-MM-DD')";
                //SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, clsDB.DbCon);
                for (i = 0; i < Dt.Rows.Count; i++)
                {
                    if (Convert.ToInt32(Dt.Rows[i]["ARRAYCLASS"].ToString()) == 1)
                        clsPmpaPb.Old_PedAdd1 = Convert.ToInt32(Dt.Rows[i]["RATEVALUE"].ToString());
                    else
                        clsPmpaPb.Old_PedAdd2 = Convert.ToInt32(Dt.Rows[i]["RATEVALUE"].ToString());
                }

                Dt.Dispose();
                Dt = null;
            }

            Bas_PedAddYg(); //0세 소아가산률

            return;
        }

        /// <summary>
        /// 0세 소아가산률
        /// </summary>
        private void Bas_PedAddYg()
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";

            string strDate1 = "";
            string strDate2 = "";
            string strAcctDate = "";
            string strArrClass = "";
            int i;

            if (clsPmpaPb.GstrMirFlag == "OK")
                strAcctDate = clsPmpaPb.GstrMirDate;
            else
                strAcctDate = clsPublic.GstrSysDate;

            clsPmpaPb.PedAddYg_Date = "";
            clsPmpaPb.PedAddYg1 = 0;
            clsPmpaPb.PedAddYg2 = 0;
            clsPmpaPb.PedAddYg3 = 0;
            clsPmpaPb.PedAddYg4 = 0;
            clsPmpaPb.PedAddYg5 = 0;
            clsPmpaPb.PedAddYg6 = 0;
            clsPmpaPb.Old_PedAddYg1 = 0;
            clsPmpaPb.Old_PedAddYg2 = 0;


            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(STARTDATE, 'YYYY-MM-DD') SDATE ";
            SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_ACCOUNT ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND IDNAME    = 'PEDADDYG' ";
            SQL += ComNum.VBLF + "    AND STARTDATE <= TO_DATE('" + strAcctDate + "', 'YYYY-MM-DD')";
            SQL += ComNum.VBLF + "  GROUP BY STARTDATE ";
            SQL += ComNum.VBLF + " ORDER BY STARTDATE DESC ";
            //  SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
            SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, clsDB.DbCon);

            strDate1 = Dt.Rows[0]["SDATE"].ToString();
            clsPmpaPb.PedAddYg_Date = strDate1;

            if (Dt.Rows.Count > 1)
                strDate2 = Dt.Rows[1]["SDATE"].ToString();

            Dt.Dispose();
            Dt = null;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT ARRAYCLASS, RATEVALUE  ";
            SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_ACCOUNT  ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1  ";
            SQL += ComNum.VBLF + "    AND IDNAME    = 'PEDADDYG' ";
            SQL += ComNum.VBLF + "    AND STARTDATE = TO_DATE('" + strDate1 + "', 'YYYY-MM-DD') ";
           // SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
            SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, clsDB.DbCon);
            for (i = 0; i < Dt.Rows.Count; i++)
            {
                strArrClass = Dt.Rows[i]["ARRAYCLASS"].ToString();

                switch (strArrClass.Trim())
                {
                    case "1":
                        clsPmpaPb.PedAddYg1 = Convert.ToInt32(Dt.Rows[i]["RATEVALUE"].ToString()); break;
                    case "2":
                        clsPmpaPb.PedAddYg2 = Convert.ToInt32(Dt.Rows[i]["RATEVALUE"].ToString()); break;
                    case "3":
                        clsPmpaPb.PedAddYg3 = Convert.ToInt32(Dt.Rows[i]["RATEVALUE"].ToString()); break;
                    case "4":
                        clsPmpaPb.PedAddYg4 = Convert.ToInt32(Dt.Rows[i]["RATEVALUE"].ToString()); break;
                    case "5":
                        clsPmpaPb.PedAddYg5 = Convert.ToInt32(Dt.Rows[i]["RATEVALUE"].ToString()); break;
                    case "6":
                        clsPmpaPb.PedAddYg6 = Convert.ToInt32(Dt.Rows[i]["RATEVALUE"].ToString()); break;
                }
            }

            Dt.Dispose();
            Dt = null;

            if (strDate2 != "")
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT ARRAYCLASS, RATEVALUE ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_ACCOUNT ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1  ";
                SQL += ComNum.VBLF + "    AND IDNAME    = 'PEDADDYG' ";
                SQL += ComNum.VBLF + "    AND STARTDATE = TO_DATE('" + strDate2 + "', 'YYYY-MM-DD')";
               // SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon); //GetDataTableEx
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, clsDB.DbCon);
                for (i = 0; i < Dt.Rows.Count; i++)
                {
                    if (Convert.ToInt32(Dt.Rows[i]["ARRAYCLASS"].ToString()) == 1)
                        clsPmpaPb.Old_PedAddYg1 = Convert.ToInt32(Dt.Rows[i]["RATEVALUE"].ToString());
                    else
                        clsPmpaPb.Old_PedAddYg2 = Convert.ToInt32(Dt.Rows[i]["RATEVALUE"].ToString());
                }

                Dt.Dispose();
                Dt = null;
            }

            return;
        }

        #endregion

        #region 본인부가세율
        public void Bas_Bon_Tax()
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";

            string strDate1 = "";
            string strDate2 = "";
            string strAcctDate = "";
            int i, j, k;

            if (clsPmpaPb.GstrMirFlag == "OK")
                strAcctDate = clsPmpaPb.GstrMirDate;
            else
                strAcctDate = clsPublic.GstrSysDate;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(STARTDATE, 'YYYY-MM-DD') SDATE";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_ACCOUNT ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND IDNAME    = 'BON_TAX' ";
                SQL += ComNum.VBLF + "    AND STARTDATE <= TO_DATE('" + strAcctDate + "', 'YYYY-MM-DD')";
                SQL += ComNum.VBLF + "  GROUP BY STARTDATE  ";
                SQL += ComNum.VBLF + " ORDER BY STARTDATE DESC ";
                // SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, clsDB.DbCon);

                strDate1 = Dt.Rows[0]["SDATE"].ToString();

                if (Dt.Rows.Count > 1)
                {
                    strDate2 = Dt.Rows[1]["SDATE"].ToString();
                }

                clsPmpaPb.OBON_TAX_DATE = strDate1;

                Dt.Dispose();
                Dt = null;

                SQL = "";
                SQL += ComNum.VBLF + " SELECT ARRAYCLASS, RATEVALUE ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_ACCOUNT ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND IDNAME    = 'BON_TAX' ";
                SQL += ComNum.VBLF + "    AND STARTDATE = TO_DATE('" + strDate1 + "', 'YYYY-MM-DD')";
             //   SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, clsDB.DbCon);
                for (i = 0; i < Dt.Rows.Count; i++)
                {
                    j = Convert.ToInt32(Dt.Rows[i]["ARRAYCLASS"].ToString());
                    k = Convert.ToInt32(Dt.Rows[i]["RATEVALUE"].ToString());

                    clsPmpaPb.BON_TAX[j] = k;
                }

                Dt.Dispose();
                Dt = null;

                if (strDate2 != "")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT ARRAYCLASS, RATEVALUE ";
                    SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_ACCOUNT ";
                    SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                    SQL += ComNum.VBLF + "    AND IDNAME    = 'BON_TAX'";
                    SQL += ComNum.VBLF + "    AND STARTDATE = TO_DATE('" + strDate2 + "', 'YYYY-MM-DD')";
                    // SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                    SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, clsDB.DbCon);
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        j = Convert.ToInt32(Dt.Rows[i]["ARRAYCLASS"].ToString());
                        k = Convert.ToInt32(Dt.Rows[i]["RATEVALUE"].ToString());

                        clsPmpaPb.OLD_BON_TAX[j] = k;
                    }

                    Dt.Dispose();
                    Dt = null;
                }
                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }
        }
        #endregion

        #region Bas_Add 나이구분
        public string Bas_Add_Age_Set(int nAge, double Ageilsu, string strCurDate, bool bXray, bool bOG, string strHang)
        {
            string rtnVal = "0";

            // 나이구분                       // strHang  
            // 0.성인                         // 01.진찰료
            // 1.신생아                       // 02.약제조제료
            // 2.만1세미만                    // 03.주사수기료
            // 3.만1세이상 - 만6세미만        // 04.마취료
            // 4.만6세미만                    // 05.처치수술료
            // 5.만70세이상                   // 06.검사료
            // 6.만35세이상(분만수가)         // 07.영상진단료
            // 7.신생아0세
            if (nAge == 0)
            {
                //개월수 체크 1달 미만 신생아 여부 체크
                if (nAge == 0)
                {
                    if (Ageilsu <= 28)        //생후 28일
                    {
                        rtnVal = "1";     //신생아
                    }
                    else
                    {
                        rtnVal = "2";     //1세미만
                    }
                }
                else
                {
                    rtnVal = "2";     //1세미만
                }
            }
            else if (nAge >= 1 && nAge < 6)
            {
                if (bXray)
                {
                    rtnVal = "4";   //6세미만
                }
                else
                {
                    rtnVal = "3";   //1세이상 -6세미만
                }
            }
            else if (nAge >= 70)
            {
                rtnVal = "5";   //70세이상
            }
            else
            {
                if (bOG)        //분만수가의 경우 나이구분 35세이상 구분 추가됨
                {
                    if (nAge >= 35)
                    {
                        rtnVal = "6";
                    }
                    else
                    {
                        rtnVal = "0";
                    }
                }
                else
                {
                    rtnVal = "0";
                }

            }

            //마취구분이 아니면 70세 이상 노인은 성인으로 분류
            //마취구분에서만 70세 이상 노인 구분 사용함.
            if (strHang != "04")
            {
                if (rtnVal == "5")
                {
                    rtnVal = "0";
                }
            }

            return rtnVal;
        }
        #endregion

        #region Bas_Add 가산코드(PCODE) 읽기
        public string Bas_Add_PCodeDtl(PsmhDb pDbCon, clsPmpaType.cBas_Add_Arg cBArg, string strHang, string strChild)
        {
            int i = 0;
            string rtnVal = "000";
            string strSDate = string.Empty;
            string strEDate = string.Empty;

            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "  SELECT GUBUN,PCODE,GBCHILD,NIGHT,GBER,HOLIDAY,ADD1,ADD2,ADD3,ADD4,ADD5,ADD6, ";
                SQL += ComNum.VBLF + "         SDATE,EDATE ";
                SQL += ComNum.VBLF + "    From " + ComNum.DB_PMPA + "BAS_ADD ";
                SQL += ComNum.VBLF + "   Where GUBUN = '" + strHang + "' ";
                if (cBArg.NIGHT == "2")
                {
                    SQL += ComNum.VBLF + "       AND NIGHT = 'Y' ";
                }
                else if (cBArg.NIGHT == "1")
                {
                    SQL += ComNum.VBLF + "       AND HOLIDAY = 'Y' ";
                }
                else
                {
                    SQL += ComNum.VBLF + "       AND NIGHT IS NULL ";
                    SQL += ComNum.VBLF + "       AND HOLIDAY IS NULL  ";
                }

                if (cBArg.MIDNIGHT != "")
                {
                    SQL += ComNum.VBLF + "       AND MIDNIGHT = 'Y' ";
                }
                else
                {
                    SQL += ComNum.VBLF + "       AND MIDNIGHT IS NULL ";
                }
                
                if (cBArg.GBER == "1" || cBArg.GBER == "2" || cBArg.GBER == "3" || cBArg.GBER == "A")
                {
                    SQL += ComNum.VBLF + "       AND GBER = 'Y' ";
                }
                else
                {
                    SQL += ComNum.VBLF + "       AND GBER IS NULL ";
                }
                if (cBArg.ADD1 != "") { SQL += ComNum.VBLF + "       AND ADD1 = 'Y' "; }
                else { SQL += ComNum.VBLF + "       AND ADD1 IS NULL "; }
                if (cBArg.ADD2 != "") { SQL += ComNum.VBLF + "       AND ADD2 = 'Y' "; }
                else { SQL += ComNum.VBLF + "       AND ADD2 IS NULL "; }
                if (cBArg.ADD3 != "") { SQL += ComNum.VBLF + "       AND ADD3 = 'Y' "; }
                else { SQL += ComNum.VBLF + "       AND ADD3 IS NULL "; }
                if (cBArg.ADD4 != "") { SQL += ComNum.VBLF + "       AND ADD4 = 'Y' "; }
                else { SQL += ComNum.VBLF + "       AND ADD4 IS NULL "; }
                if (cBArg.ADD5 != "") { SQL += ComNum.VBLF + "       AND ADD5 = 'Y' "; }
                else { SQL += ComNum.VBLF + "       AND ADD5 IS NULL "; }
                if (cBArg.ADD6 != "") { SQL += ComNum.VBLF + "       AND ADD6 = 'Y' "; }
                else { SQL += ComNum.VBLF + "       AND ADD6 IS NULL "; }
                if (cBArg.ADD7 != "") { SQL += ComNum.VBLF + "       AND ADD7 = 'Y' "; }
                else { SQL += ComNum.VBLF + "       AND ADD7 IS NULL "; }
                if (cBArg.ADD8 != "0") { SQL += ComNum.VBLF + "       AND ADD8 = 'Y' "; }
                else { SQL += ComNum.VBLF + "       AND ADD8 IS NULL "; }
                if (cBArg.ADD9 != "") { SQL += ComNum.VBLF + "       AND ADD9 = 'Y' "; }
                else { SQL += ComNum.VBLF + "       AND ADD9 IS NULL "; }
                SQL += ComNum.VBLF + "       AND GBCHILD = '" + strChild + "' ";
                SQL += ComNum.VBLF + "       AND (DELDATE IS NULL OR DELDATE = '' ) ";
                SQL += ComNum.VBLF + "     ORDER By SDATE DESC";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        strSDate = Dt.Rows[i]["SDATE"].ToString().Trim();
                        strEDate = Dt.Rows[i]["EDATE"].ToString().Trim();

                        if (strEDate.Equals(""))
                        {
                            if (string.Compare(cBArg.BDATE, strSDate) >= 0)
                            {
                                rtnVal = Dt.Rows[i]["PCODE"].ToString().Trim();
                                break;
                            }
                        }
                        else
                        {
                            if (string.Compare(cBArg.BDATE, strSDate) >= 0 && string.Compare(cBArg.BDATE, strEDate) <= 0)
                            {
                                rtnVal = Dt.Rows[i]["PCODE"].ToString().Trim();
                                break;
                            }
                        }
                    }

                    if (rtnVal == "")
                    {
                        rtnVal = "000";
                    }
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }
        #endregion

        #region 분류별 EDI 항목 매칭
        public string Bas_Acct_Hang_Set(string strBun)
        {
            string rtnVal = "00";

            if (string.Compare(strBun, "01") >= 0 && string.Compare(strBun, "10") <= 0)      //진찰료, 입원료
            {
                rtnVal = "01";
            }
            else if (string.Compare(strBun, "11") >= 0 && string.Compare(strBun, "15") <= 0)    //투약및처방전료
            {
                rtnVal = "02";
            }
            else if (string.Compare(strBun, "16") >= 0 && string.Compare(strBun, "21") <= 0)    //주사료
            {
                rtnVal = "03";
            }
            else if (string.Compare(strBun, "22") >= 0 && string.Compare(strBun, "23") <= 0)    //마취료
            {
                rtnVal = "04";
            }
            else if (string.Compare(strBun, "28") >= 0 && string.Compare(strBun, "40") <= 0)    //처치및수술료
            {
                rtnVal = "05";
            }
            else if (string.Compare(strBun, "41") >= 0 && string.Compare(strBun, "64") <= 0)    //검사료
            {
                rtnVal = "06";
            }
            else if (string.Compare(strBun, "65") >= 0 && string.Compare(strBun, "70") <= 0)    //방사선
            {
                rtnVal = "07";
            }
            else
            {
                rtnVal = "00";              //오류
            }

            return rtnVal;
        }
        #endregion

        #region 분류별 가산 항목 세팅
        public void Bas_Add_Set(ref clsPmpaType.cBas_Add_Arg cBArg, string strHang)
        {
            cBArg.ADD1 = "";
            cBArg.ADD2 = "";
            cBArg.ADD3 = "";
            cBArg.ADD4 = "";
            cBArg.ADD5 = "";
            cBArg.ADD6 = "";
            cBArg.ADD7 = "";
            cBArg.ADD9 = "";

            //병원에서 사용하는 가산만 세팅함.
            if (strHang == "04")
            {
                if (cBArg.AN1 == "1") { cBArg.ADD1 = "Y"; }     //개두마취
                if (cBArg.AN1 == "2") { cBArg.ADD2 = "Y"; }     //일측폐환기
                if (cBArg.AN1 == "3") { cBArg.ADD3 = "Y"; }     //개흉적심장마취
                if (cBArg.AN2 == "1") { cBArg.ADD9 = "Y"; }     //ASA
            }
            else if (strHang == "05")
            {
                if (cBArg.OP1 == "1") { cBArg.ADD1 = "Y"; }     //외과가산
                if (cBArg.OP1 == "2") { cBArg.ADD3 = "Y"; }     //흉부외과가산
                if (cBArg.OP2 == "1") { cBArg.ADD2 = "Y"; }     //화상가산
                if (cBArg.OP3 == "1") { cBArg.ADD4 = "Y"; }     //부수술
                if (cBArg.OP3 == "2") { cBArg.ADD5 = "Y"; }     //제2수술
                if (cBArg.OP4 == "1") { cBArg.ADD6 = "Y"; }     //산모가산
                if (cBArg.ADD8 == "1") { cBArg.ADD8 = "Y"; }    //신경외과가산
                if (cBArg.ADD8 == "2") { cBArg.ADD8 = "Y"; }    //신경외과가산
                if (cBArg.ADD8 == "3") { cBArg.ADD8 = "Y"; }    //신경외과가산
            }
            else if (strHang == "07")
            {
                if (cBArg.XRAY1 == "1") { cBArg.ADD1 = "Y"; }   //판독가산
            }

            if (cBArg.BUN == "35") { cBArg.ADD7 = "Y"; }        //분만수가

        }
        #endregion

        #region 본인부담 상한액 
        public void IPD_BON_SANG()
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";

            int i;

            for (i = 0; i < 11; i++)
            {
                clsPmpaPb.GstrSangBdate[i] = "";
                clsPmpaPb.GnSangAmt[i] = 0;
            }

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT RateValue, TO_CHAR(StartDate, 'YYYY-MM-DD') STARTDATE     ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ACCOUNT                         ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1                                                     ";
                SQL += ComNum.VBLF + "    AND IDNAME = 'IPD_SANG'                                       ";
                SQL += ComNum.VBLF + "  ORDER BY STARTDATE DESC                                         ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        clsPmpaPb.GstrSangBdate[i] = Dt.Rows[i]["STARTDATE"].ToString().Trim();
                        clsPmpaPb.GnSangAmt[i] = Convert.ToInt64(VB.Val(Dt.Rows[i]["RateValue"].ToString()) * 10000);
                    }
                }

                Dt.Dispose();
                Dt = null;

                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }
        }
        #endregion

        /// <summary>
        /// 최종 가산 산정코드(3자리)를 산정하여 BCODE+000 EDI수가를 Return        
        /// <param name="pDbCon"></param>
        /// <param name="strPCode"></param>
        /// <param name="cBArg"></param>
        /// <param name="strHang"></param>
        /// <param name="bOG"></param>
        /// <returns>string</returns>
        /// </summary>
        public string PCodeDtl_Process(PsmhDb pDbCon, string strPCode, clsPmpaType.cBas_Add_Arg cBArg, string strHang, bool bOG)
        {
            string rtnVal = "XX";
            string strGbChild = "0";
            string strPCodeDtl = string.Empty;
            bool bXray = false;

            bXray = strHang == "07" ? true : false; //영상의학가산은 나이구분이 다름

            if (clsPmpaType.ISG.GBNS != "Y")
            {
                strGbChild = Bas_Add_Age_Set(cBArg.AGE, cBArg.AGEILSU, cBArg.BDATE, bXray, bOG, strHang);     //나이구분
            }
            if (cBArg.SUGBB  == "T" && (strGbChild =="1" || strGbChild == "2") )
            {
                strGbChild = "7";     //나이구분
            }


            if (string.Compare(cBArg.BDATE, "2021-02-01") >= 0 && cBArg.ADD7 == "Y"  )
            { 
                if (strGbChild != "6")
                {
                    cBArg.ADD7 = "";         //분만수가
                }
                
            }



            //마취가산 구분 조정
            if (strHang == "04")
            {
                Chk_Mach_Add_Confirm(cBArg.SUNEXT, cBArg, ref strGbChild);
            }

            strPCodeDtl = Bas_Add_PCodeDtl(pDbCon, cBArg, strHang, strGbChild);                   //산정코드

            if (strPCodeDtl.Equals("000"))
            {
                strPCodeDtl = "";
            }

            rtnVal = strPCode + strPCodeDtl;   //ex) AA156010

            return rtnVal;
        }

        #region 입원료 산정기준
        public string PCode_02_Process(string strSunext, string strBun, string strChild, string strGisul, string strNgt, double nQty, int nNal, int nAge, string strSugaAA, int nAgeIlsu, string strBDate)
        {
            string rtnVal = "XX";

            return rtnVal;
        }
        #endregion

        #region 투약료 산정기준
        public string PCode_03_Process(string strSunext, string strBun, string strChild, string strGisul, string strNgt, double nQty, int nNal, int nAge, string strSugaAA, int nAgeIlsu, string strBDate)
        {
            string rtnVal = "XX";

            return rtnVal;
        }
        #endregion

        #region 주사수기료 산정기준
        public string PCode_04_Process(string strSunext, string strBun, string strChild, string strGisul, string strNgt, double nQty, int nNal, int nAge, string strSugaAA, int nAgeIlsu, string strBDate)
        {
            string rtnVal = "XX";

            return rtnVal;
        }
        #endregion

        #region 마취료 산정기준
        public string PCode_05_Process(string strSunext, string strBun, string strChild, string strGisul, string strNgt, double nQty, int nNal, int nAge, string strSugaAA, int nAgeIlsu, string strBDate)
        {
            string rtnVal = "XX";

            return rtnVal;
        }
        #endregion

        #region 물리치료료 산정기준
        public string PCode_06_Process(string strSunext, string strBun, string strChild, string strGisul, string strNgt, double nQty, int nNal, int nAge, string strSugaAA, int nAgeIlsu, string strBDate)
        {
            string rtnVal = "XX";

            return rtnVal;
        }
        #endregion

        #region 신경정신요법료 산정기준
        public string PCode_07_Process(string strSunext, string strBun, string strChild, string strGisul, string strNgt, double nQty, int nNal, int nAge, string strSugaAA, int nAgeIlsu, string strBDate)
        {
            string rtnVal = "XX";

            return rtnVal;
        }
        #endregion

        #region 처치,수술료 산정기준
        public string PCode_08_Process(string strSunext, string strBun, string strChild, string strGisul, string strNgt, double nQty, int nNal, int nAge, string strSugaAA, int nAgeIlsu, string strBDate)
        {
            string rtnVal = "XX";

            return rtnVal;
        }
        #endregion

        #region 검사료 산정기준
        public string PCode_09_Process(string strSunext, string strBun, string strChild, string strGisul, string strNgt, double nQty, int nNal, int nAge, string strSugaAA, int nAgeIlsu, string strBDate)
        {
            string rtnVal = "XX";

            return rtnVal;
        }
        #endregion

        #region 방사선료 산정기준
        public string PCode_10_Process(string strSunext, string strBun, string strChild, string strGisul, string strNgt, double nQty, int nNal, int nAge, string strSugaAA, int nAgeIlsu, string strBDate)
        {
            string rtnVal = "XX";

            return rtnVal;
        }
        #endregion

        #region CT/MRI 산정기준
        public string PCode_11_Process(string strSunext, string strBun, string strChild, string strGisul, string strNgt, double nQty, int nNal, int nAge, string strSugaAA, int nAgeIlsu, string strBDate)
        {
            string rtnVal = "XX";

            return rtnVal;
        }
        #endregion

        #region EDI_SUGA 테이블에서 산정코드 기준 금액 읽어오기
        public long Read_EDI_SUGA_PCode(string strPCode, string strJDate)
        {
            long rtnVal = 0;
            int i = 0, nRead = 0;
            string strDate1 = string.Empty;
            string strDate2 = string.Empty;
            string strDate3 = string.Empty;
            string strDate4 = string.Empty;
            string strDate5 = string.Empty;
            string strDate6 = string.Empty;

            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "  SELECT  CODE,JONG,PNAME,BUN,DANWI1,DANWI2,SPEC,COMPNY,EFFECT,GUBUN,DANGN, ";
                SQL = SQL + ComNum.VBLF + "          PRICE1,PRICE2,PRICE3,PRICE4,PRICE5,PRICE6, ";
                SQL = SQL + ComNum.VBLF + "          TO_CHAR(JDATE1,'YYYY-MM-DD') JDATE1,TO_CHAR(JDATE2,'YYYY-MM-DD') JDATE2, ";
                SQL = SQL + ComNum.VBLF + "          TO_CHAR(JDATE1,'YYYY-MM-DD') JDATE3,TO_CHAR(JDATE2,'YYYY-MM-DD') JDATE4, ";
                SQL = SQL + ComNum.VBLF + "          TO_CHAR(JDATE1,'YYYY-MM-DD') JDATE5,TO_CHAR(JDATE2,'YYYY-MM-DD') JDATE6, ";
                SQL = SQL + ComNum.VBLF + "          SUSUL,SCORE,SCODE,KASAN,CODE_OLD,SILNO,SUGAGBN, ";
                SQL = SQL + ComNum.VBLF + "          JANG1,SA1,JANG2,SA2,JANG3,SA3,JANG4,SA4,JANG5,SA5,JANG6,SA6 ";
                SQL = SQL + ComNum.VBLF + "      From " + ComNum.DB_PMPA + "EDI_SUGA ";
                SQL = SQL + ComNum.VBLF + "     Where CODE = '" + strPCode + "' ";
                //SQL = SQL + ComNum.VBLF + "       AND JONG <> '8'               ";      //재료대 제외
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return 0;
                }

                nRead = Dt.Rows.Count;

                if (nRead > 0)
                {
                    strDate1 = Dt.Rows[i]["JDATE1"].ToString().Trim();
                    strDate2 = Dt.Rows[i]["JDATE2"].ToString().Trim();
                    strDate3 = Dt.Rows[i]["JDATE3"].ToString().Trim();
                    strDate4 = Dt.Rows[i]["JDATE4"].ToString().Trim();
                    strDate5 = Dt.Rows[i]["JDATE5"].ToString().Trim();
                    strDate6 = Dt.Rows[i]["JDATE6"].ToString().Trim();

                    if (string.Compare(strJDate, strDate1) >= 0)
                    {
                        rtnVal = Convert.ToInt32(Dt.Rows[0]["PRICE1"].ToString());
                    }
                    else if (string.Compare(strJDate, strDate2) >= 0)
                    {
                        rtnVal = Convert.ToInt32(Dt.Rows[0]["PRICE2"].ToString());
                    }
                    else if (string.Compare(strJDate, strDate3) >= 0)
                    {
                        rtnVal = Convert.ToInt32(Dt.Rows[0]["PRICE3"].ToString());
                    }
                    else if (string.Compare(strJDate, strDate4) >= 0)
                    {
                        rtnVal = Convert.ToInt32(Dt.Rows[0]["PRICE4"].ToString());
                    }
                    else if (string.Compare(strJDate, strDate5) >= 0)
                    {
                        rtnVal = Convert.ToInt32(Dt.Rows[0]["PRICE5"].ToString());
                    }
                    else
                    {
                        rtnVal = Convert.ToInt32(Dt.Rows[0]["PRICE6"].ToString());
                    }
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return 0;
            }
        }
        #endregion

        #region BAS_ADD 수가가산 테이블 작업 (INSERT, UPDATE, DELETE)
        public string ins_BasAdd(PsmhDb pDbCon, clsPmpaType.cBas_Add cBAdd, ref int intRowAffected)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "  INSERT INTO " + ComNum.DB_PMPA + "BAS_ADD                                 \r\n";
            SQL += "   ( GUBUN,PCODE,GBCHILD,NIGHT,GBER,HOLIDAY,ADD1,ADD2,ADD3,ADD4,ADD5,ADD6,  \r\n";
            SQL += "     ADD7,ADD8,ADD9,SDATE,EDATE,DELDATE,ENTDATE,ENTSABUN,MIDNIGHT )                   \r\n";
            SQL += "   VALUES (                                                                 \r\n";
            SQL += "      '" + cBAdd.GUBUN + "'                                                 \r\n";
            SQL += "     ,'" + cBAdd.PCODE + "'                                                 \r\n";
            SQL += "     ,'" + cBAdd.GBCHILD + "'                                               \r\n";
            SQL += "     ,'" + cBAdd.NIGHT + "'                                                 \r\n";
            SQL += "     ,'" + cBAdd.GBER + "'                                                  \r\n";
            SQL += "     ,'" + cBAdd.HOLIDAY + "'                                               \r\n";
            SQL += "     ,'" + cBAdd.ADD1 + "'                                                  \r\n";
            SQL += "     ,'" + cBAdd.ADD2 + "'                                                  \r\n";
            SQL += "     ,'" + cBAdd.ADD3 + "'                                                  \r\n";
            SQL += "     ,'" + cBAdd.ADD4 + "'                                                  \r\n";
            SQL += "     ,'" + cBAdd.ADD5 + "'                                                  \r\n";
            SQL += "     ,'" + cBAdd.ADD6 + "'                                                  \r\n";
            SQL += "     ,'" + cBAdd.ADD7 + "'                                                  \r\n";
            SQL += "     ,'" + cBAdd.ADD8 + "'                                                  \r\n";
            SQL += "     ,'" + cBAdd.ADD9 + "'                                                  \r\n";
            SQL += "     ,'" + cBAdd.SDATE + "'                                                 \r\n";
            SQL += "     ,'" + cBAdd.EDATE + "'                                                 \r\n";
            SQL += "     ,'" + cBAdd.DELDATE + "'                                               \r\n";
            SQL += "     ,SYSDATE                                                               \r\n";
            SQL += "     ,'" + cBAdd.ENTSABUN + "'                                              \r\n";
            SQL += "     ,'" + cBAdd.MIDNIGHT + "'                                              \r\n";
            SQL += "         )                                                                  \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string up_BasAdd(PsmhDb pDbCon, clsPmpaType.cBas_Add cBAdd, ref int intRowAffected)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "  UPDATE " + ComNum.DB_PMPA + "BAS_ADD                          \r\n";
            SQL += "     SET EDATE = '" + cBAdd.EDATE + "'                          \r\n";
            SQL += "   WHERE ROWID = '" + cBAdd.ROWID + "'                          \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string del_BasAdd(PsmhDb pDbCon, string strRowid, string strTable, ref int intRowAffected)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "  UPDATE " + ComNum.DB_PMPA + "BAS_ADD                  \r\n";
            SQL += "     SET DELDATE = TRUNC(SYSDATE)                       \r\n";
            SQL += "   WHERE ROWID = '" + strRowid + "'                     \r\n";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }
        #endregion

        /// <summary>
        /// 누적코드 치환(급여 > 비급여)
        /// </summary>
        /// <param name="ArgNu"></param>
        /// <returns></returns>
        public string Rtn_Nu_ToBiGub(string ArgNu)
        {
            string rtnVal = ArgNu;

            switch (ArgNu.Trim())
            {
                case "01":
                case "02":
                case "03":
                    rtnVal = "21";
                    break;

                case "04":
                case "05":
                case "06":
                case "07":
                case "08":
                case "09":
                case "10":
                case "11":
                case "12":
                case "13":
                case "14":
                case "15":
                    rtnVal = string.Format("{0:D2}", Convert.ToInt32(ArgNu) + 18);
                    break;

                case "16":
                    rtnVal = "34";
                    break;

                case "17":
                    rtnVal = "42";
                    break;

                case "18":
                    rtnVal = "47";
                    break;

                case "19":
                    rtnVal = "37";
                    break;

                case "20":
                    rtnVal = "27";
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// 누적코드 치환(비급여 > 급여)
        /// </summary>
        /// <param name="ArgNu"></param>
        /// <returns></returns>
        public string Rtn_Nu_ToGub(string ArgNu)
        {
            string rtnVal = ArgNu;

            switch (ArgNu.Trim())
            {
                case "21":
                case "22":
                case "23":
                case "24":
                case "25":
                case "26":
                case "27":
                case "28":
                case "29":
                case "30":
                case "31":
                case "32":
                case "33":
                case "34":
                case "35":
                case "36":
                case "37":
                case "38":
                case "39":
                case "40":
                    rtnVal = "19";

                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// Slip_Wrt_AmtAdd Bun 치환
        /// </summary>
        /// <param name="ArgBun"></param>
        /// <returns></returns>
        public string Rtn_Bun_AmtAdd(string ArgBun)
        {
            string rtnVal = ArgBun;

            switch (ArgBun.Trim())
            {
                case "01":
                case "02":
                case "03":
                case "04":
                    rtnVal = "01"; //진찰료
                    break;

                case "11":
                case "12":
                case "13":
                case "14":
                case "15":
                    rtnVal = "02"; //약
                    break;

                case "16":
                case "17":
                case "18":
                case "19":
                case "20":
                    rtnVal = "03"; //주사
                    break;

                case "22":
                case "23":
                case "28":
                case "34":
                case "35":
                case "39":
                    rtnVal = "06"; //처치수술
                    break;

                case "41":
                case "42":
                case "43":
                case "44":
                case "45":
                case "46":
                case "47":
                case "48":
                case "49":
                case "50":
                case "51":
                case "52":
                case "53":
                case "54":
                case "55":
                case "56":
                case "57":
                case "58":
                case "59":
                case "60":
                case "61":
                case "62":
                case "63":
                    rtnVal = "04"; //검사
                    break;

                case "65":
                case "66":
                    rtnVal = "05"; //방사선
                    break;

                case "71":
                case "72":
                case "73":
                    rtnVal = "07"; //CT,MRI,SONO
                    break;

                default:
                    rtnVal = "08"; //기타
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// 응급실 가산 기초모듈
        /// </summary>
        /// <param name="ArgJtime">접수시각</param>
        /// <param name="ArgOrderTime">처방시각</param>
        /// <param name="ArgSugbAA">응급가산항목</param>
        /// <param name="ArgDept">진료과</param>
        /// <param name="ArgKTASLev">KTAS레벨</param>
        /// <param name="ArgIO">입원/외래</param>
        /// <param name="ArgGbKTAS">강제입력여부 (OK)강제</param>
        /// <seealso cref="ErAcct.bas:BAS_ER_RATE"/>
        public void Bas_ER_Rate(string ArgJtime, string ArgOrderTime, string ArgSugbAA, string ArgDept, string ArgKTASLev, string ArgIO, string ArgGbKTAS = "")
        {
            clsPmpaPb.GnErRate = 0;
            clsPmpaPb.GnErRateK = 0;

            if (VB.Val(ArgSugbAA) < 1) { return; } //가산항목이 아님

            if (ArgSugbAA != "3")
                if (ArgIO == "O" && ArgDept != "ER") { return; } //응급실만 적용

            if (ArgSugbAA == "1") //응급가산 50%
            {
                if (ArgIO == "I")
                {
                    if (ArgKTASLev == "A" || (VB.Val(ArgKTASLev) > 0 && VB.Val(ArgKTASLev) <= 3))
                        clsPmpaPb.GnErRate = 50;
                }
                else
                    clsPmpaPb.GnErRate = 50;
            }
            else if (ArgSugbAA == "2") //중증응급가산 50%
            {
                if (VB.Val(ArgKTASLev) == 0 || VB.Val(ArgKTASLev) > 3) { return; }

                clsPmpaPb.GnErRate = 50;
            }
            else if (ArgSugbAA == "3") //권역응급가산
            {
                if (VB.Val(ArgKTASLev) == 0 || VB.Val(ArgKTASLev) > 3) { return; }

                if (ArgGbKTAS != "OK")
                    if (ArgOrderTime == "0" || ArgOrderTime == "") { return; }

                clsPmpaPb.GnErRate = 50;
            }
        }

        /// <summary>
        /// 신생아 일수계산을 위해 계산로직
        /// </summary>
        /// <param name="ArgAge"></param>
        /// <param name="ArgSugbB"></param>
        /// <param name="ArgSugbE"></param>
        /// <param name="ArgBun"></param>
        /// <param name="ArgDate"></param>
        /// <param name="ArgAgeiLsu"></param>
        /// <param name="ArgIO">Option</param>
        /// <param name="ArgDept">Option</param>
        /// <param name="ArgGbZ">Option</param>
        /// <seealso cref="ErAcct.bas:Bas_PED_Rate"/>
        public void Bas_PED_Rate(int ArgAge, string ArgSugbB, string ArgSugbE, string ArgBun, string ArgDate, int ArgAgeiLsu = 0, string ArgIO = "", string ArgDept = "", string ArgGbZ = "")
        {
            clsPmpaPb.GnPedRate = 0;
            if ((ArgDept == "ER") && ( clsPmpaPb.GstrKTASLev == "1" || clsPmpaPb.GstrKTASLev == "2" || clsPmpaPb.GstrKTASLev == "3")) { ArgIO = "I"; }
            //내복약,외용,주사,주사재료 소아가산 없음
            if (ArgBun == "11" || ArgBun == "12" || ArgBun == "20" || ArgBun == "21") { return; }

            if (ArgBun == "22") //마취료 소아가산은 0-6세(만 6세미만)만 됨(마취료는 70세이상 가산됨)
            {
                if (ArgAge > 5 && ArgAge < 70) { return; }
            }
            else if (ArgBun == "45") //뇌파검사 소아가산은 6세미만
            {
                if (ArgAge > 5) { return; }
            }
            else if (ArgBun == "47") //기타기능검사 소아가산은 6세미만
            {
                if (ArgAge > 5) { return; }
            }
            else
            {
                if (ArgSugbB == "Z" && (ArgBun == "35" || ArgBun == "28"))
                {

                }
                else if (ArgSugbB == "Y" && ArgBun == "48")
                {
                    if (ArgAge > 5 && ArgAge < 70) { return; }
                }
                else
                {
                    if (ArgAge > 5 && ArgDept != "DT") { return; }
                }
            }

            //소아가산은 행위료만 가산
            if (ArgSugbE != "1") { return; }

            //소아가산 구분이 0이면 가산율 0%
            if (string.Compare(ArgSugbB, "A") < 0) { return; }

            //0에 신생아 분류를 사용안하고 0세신생아분류 추가로 가산 산정 방식 2021-02-01
            if (ArgSugbB.Trim() =="T" )
            {
                if (  ArgAge == 0 )
                {
                    clsPmpaPb.GnPedRate = 200;
                    return;
                }
                else
                {
                    ArgSugbB = "E";
                }
                
            }
          

            //가산율 SET
            switch (ArgSugbB.Trim())
            {
                case "A": clsPmpaPb.GnPedRate = 15; break;
                case "B": clsPmpaPb.GnPedRate = 20; break;
                case "C":
                    clsPmpaPb.GnPedRate = 30;
                    if (ArgAge == 0) { clsPmpaPb.GnPedRate = 50; }
                    break;
                case "D": clsPmpaPb.GnPedRate = 25; break;
                case "E":
                    clsPmpaPb.GnPedRate = 30;

                    if (ArgAge == 0) { clsPmpaPb.GnPedRate = 50; }

                    if ( ArgBun == "28" || ArgBun == "34"   ) //처치수술
                    {
                        if (ArgDate.CompareTo("2018-07-01") > 0)
                        {
                            if (ArgAge > 0 && ArgAge < 6) //소아(만29일이후 - 6세 미만) 30% 가산
                            {
                                clsPmpaPb.GnPedRate = ArgIO == "I" ? 50 : 0;

                                //외래로 응급중증인 경우 가산
                                if (clsPmpaPb.GnErRate > 0) { clsPmpaPb.GnPedRate = 50; }
                            }
                            else if (ArgAge > 0 && ArgAge < 8 && ArgDept == "DT") //소아(만29일이후 - 8세 미만) 30% 가산 치과
                            {
                                clsPmpaPb.GnPedRate = ArgIO == "I" ? 50 : 0;

                                if (ArgIO != "I" && ArgDept == "DT") { clsPmpaPb.GnPedRate = 30; }
                                //외래로 응급중증인 경우 가산
                                if (clsPmpaPb.GnErRate > 0) { clsPmpaPb.GnPedRate = 50; }
                            }
                            else if (ArgAgeiLsu > 28 && ArgAge == 0) //소아(만29일이후 - 0세 ) 50% 가산
                            {
                                clsPmpaPb.GnPedRate = ArgIO == "I" ? 100 : 0;

                                if (ArgIO != "I" && ArgDept == "DT") { clsPmpaPb.GnPedRate = 30; }

                                //외래로 응급중증인 경우 가산
                                if (clsPmpaPb.GnErRate > 0) { clsPmpaPb.GnPedRate = 100; }
                            }
                            else if (ArgAgeiLsu <= 28 && ArgAge == 0) //신생아(만28일이전) 60% 가산
                            {
                                clsPmpaPb.GnPedRate = ArgIO == "I" ? 120 : 0;

                                if (ArgIO != "I" && ArgDept == "DT") { clsPmpaPb.GnPedRate = 30; }

                                //외래로 응급중증인 경우 가산
                                if (clsPmpaPb.GnErRate > 0) { clsPmpaPb.GnPedRate = 120; }
                            }
                            else
                            {
                                clsPmpaPb.GnPedRate = 0;
                                return;
                            }
                        }
                        else
                        {
                            if (ArgAge > 0 && ArgAge < 6) //소아(만29일이후 - 8세 미만) 30% 가산
                            {
                                clsPmpaPb.GnPedRate = ArgIO == "I" ? 30 : 0;

                                //외래로 응급중증인 경우 가산
                                if (clsPmpaPb.GnErRate > 0) { clsPmpaPb.GnPedRate = 30; }
                            }
                            else if (ArgAgeiLsu > 28 && ArgAge == 0) //소아(만29일이후 - 0세 ) 50% 가산
                            {
                                clsPmpaPb.GnPedRate = ArgIO == "I" ? 50 : 0;

                                if (ArgIO != "I" && ArgDept == "DT") { clsPmpaPb.GnPedRate = 30; }

                                //외래로 응급중증인 경우 가산
                                if (clsPmpaPb.GnErRate > 0) { clsPmpaPb.GnPedRate = 50; }
                            }
                            else if (ArgAgeiLsu <= 28 && ArgAge == 0) //신생아(만28일이전) 60% 가산
                            {
                                clsPmpaPb.GnPedRate = ArgIO == "I" ? 100 : 0;

                                if (ArgIO != "I" && ArgDept == "DT") { clsPmpaPb.GnPedRate = 30; }

                                //외래로 응급중증인 경우 가산
                                if (clsPmpaPb.GnErRate > 0) { clsPmpaPb.GnPedRate = 100; }
                            }
                            else
                            {
                                clsPmpaPb.GnPedRate = 0;
                                return;
                            }

                        }

                    } 

                    break;

                case "F": clsPmpaPb.GnPedRate = 35; break;
                case "G": clsPmpaPb.GnPedRate = 40; break;
                case "H": clsPmpaPb.GnPedRate = 45; break;
                case "I": clsPmpaPb.GnPedRate = 50; break;
                case "J": clsPmpaPb.GnPedRate = 60; break;
                case "Y":
                    clsPmpaPb.GnPedRate = 30;

                    if (ArgAgeiLsu > 28 || (ArgAge > 0 && ArgAge < 6))
                        clsPmpaPb.GnPedRate = 30;
                    else if (ArgAgeiLsu > 28 && ArgAge == 0)
                        clsPmpaPb.GnPedRate = 50;
                    else if (ArgAgeiLsu <= 28 && ArgAge == 0)
                        clsPmpaPb.GnPedRate = 100;

                    break;
                case "Z":
                    //고위험 산모가산
                    if (ArgAge >= 35) { clsPmpaPb.GnPedRate = 30; }
                    if (ArgGbZ == "Z") { clsPmpaPb.GnPedRate = 30; }
                    break;

                default:

                    clsPmpaPb.GnPedRate = 20;

                    if ((ArgBun == "28" || ArgBun == "34") && ArgIO == "I") { clsPmpaPb.GnPedRate = 30; }
                    break;
            }

            //분만료
            if (ArgBun == "35") { clsPmpaPb.GnPedRate += 50; }

            //내복약 조제료는 6세미만 가산
            if (ArgBun == "13" && ArgAge > 6) { clsPmpaPb.GnPedRate = 0; }

            
            if (ArgDate.CompareTo("2018-07-01") > 0 )
            {  //마취료 0세미만은 60%를 가산
                if (ArgBun == "22" && ArgAge == 0)
                    clsPmpaPb.GnPedRate = 100;
                else if (ArgBun == "22" && ArgAge < 6 && ArgAge >= 70)
                    clsPmpaPb.GnPedRate = 30;


                if (ArgBun == "22" && ArgAge == 0)
                {
                    if (ArgAgeiLsu <= 28)
                        clsPmpaPb.GnPedRate = 100;
                    else
                        clsPmpaPb.GnPedRate = 50;
                }
                else if (ArgBun == "22" && ArgAge >= 70)
                    clsPmpaPb.GnPedRate = 30;
                else if (ArgBun == "22")
                    clsPmpaPb.GnPedRate = 30;
            }
            else
            { 
            //마취료 0세미만은 60%를 가산
            if (ArgBun == "22" && ArgAge == 0)
                clsPmpaPb.GnPedRate = 100;
            else if (ArgBun == "22" && ArgAge < 6 && ArgAge >= 70)
                clsPmpaPb.GnPedRate = 30;


            if (ArgBun == "22" && ArgAge == 0)
            {
                if (ArgAgeiLsu <= 28)
                    clsPmpaPb.GnPedRate = 100;
                else
                    clsPmpaPb.GnPedRate = 50;
            }
            else if (ArgBun == "22" && ArgAge >= 70)
                clsPmpaPb.GnPedRate = 30;
            else if (ArgBun == "22")
                clsPmpaPb.GnPedRate = 30;
            }
        }    

        /// <summary>
        /// 소아 GstatPED 정의
        /// </summary>
        /// <param name="ArgJtime">접수시각</param>
        /// <param name="ArgOrderTime">처방시각</param>
        /// <param name="ArgSugbAA">응급가산항목</param>
        /// <param name="ArgDept">진료과</param>
        /// <param name="ArgKTASLev">KTAS레벨</param>
        /// <param name="ArgIO">입원/외래</param>
        /// <param name="ArgGbKTAS">강제입력여부 (OK)강제</param>
        /// <seealso cref="BasAcct.bas:BAS_PEDSLIP"/>
        public string Bas_PED_Slip(string ArgDate, int ArgAge, int ArgIlsu)
        {
            string rtnVal = "";

            if (ArgAge == 0)
            {
                if (ArgIlsu <= 28 && ArgIlsu != 0)
                    rtnVal = "A";
                else
                    rtnVal = "B";
            }
            else if (ArgAge < 6)
                rtnVal = "C";
            else
                rtnVal = "1";

            return rtnVal;
        }

        public long BAS_MACH_AMT(int argGb, string argSuNext, long argBaseAmt, double argQty, int argNal)
        {
            long nAmt = 0;
            int argMinA = 0;
            int argMinB = 0;
            int argMinC = 0;
            int argMinD = 0;
            int argMinE = 0;
            int argMinF = 0;    //2015-08-31 30분경과 15분 단수

            //2001년1월1일부로 전신마취(L1210)은 15분 단수에서 1시간경과후 15분단수로 변경
            argMinA = (int)((argQty * 60) + Math.Abs(argNal));  //총환산분 시간*60 + 분
            argMinC = (argMinA + 89) / 90;                      //90분단수
            argMinB = (argMinA + 14) / 15;                      //15분 단수
            argMinD = (argMinA + 14 - 60) / 15;                 //1시간경과후 15분 단수
            argMinE = (argMinA + 29 - 30) / 30;                 //30분경과후 30분단수
            argMinF = (argMinA + 14 - 30) / 15;                 //30분경과후 15분단수

            if (argMinD < 0) { argMinD = 0; }
            if (argMinE < 0) { argMinE = 0; }
            if (argMinF < 0) { argMinF = 0; }   //2015-08-31

            //2015-08-31 회복관리료는 강제로 행위료로 계산 당분간 이방식으로 산정함 - 보험심사과장 통화
            if (argSuNext.Trim() == "AP601")
            {
                argGb = 1;
            }

            //일반 마취 재료대 계산
            if (argGb == 2)
            {
                switch (VB.Trim(argSuNext))
                {
                    case "L6010HA":
                    case "L7010HA":
                    case "L1210HA":
                        nAmt = argBaseAmt * argMinA / 60; //할로텐
                        break;
                    case "L6010EF":
                    case "L7010EF":
                    case "L1210EF":
                        nAmt = argBaseAmt * argMinA / 60; //엔후란
                        break;
                    case "L6010IF":
                    case "L7010IF":
                    case "L7010IFG":
                    case "L7010IFA":
                    case "L7010EI":
                    case "L1210IF":
                    case "L1210EI":
                    case "L7010IS":
                    case "L7010ISG":
                    case "L7010SVG":
                        nAmt = argBaseAmt;            //이소푸르란
                        break;
                    case "L6010IF1":
                    case "L7010EI1":
                    case "L1210IF1":
                    case "L1210EI1":
                        nAmt = argBaseAmt * argMinB; //15분단수"
                        break;
                    case "L7010IF1":
                    case "L7010IF2":
                    case "L7010IS1":
                    case "L7010SV1":
                        nAmt = argBaseAmt * (argMinB - 1);    //15분단수
                        break;
                    case "L6010HA5":
                    case "L7010HA5":
                    case "L1210HA5":
                        nAmt = argBaseAmt * argMinA / 60; //할로텐 50%
                        break;
                    case "L6010EF5":
                    case "L7010EF5":
                    case "L1210EF5":
                        nAmt = argBaseAmt * argMinA / 60; //엔후란 50%
                        break;
                    case "L6010IF5":
                    case "L7010IF5":
                    case "L1210IF5":
                        nAmt = argBaseAmt;       //이소푸르란 50%
                        break;
                    case "L6010IFA":
                    //case "L7010IFA": 두개 있음
                    case "L1210IFA":
                        nAmt = argBaseAmt * argMinB; //15분단수 50%
                        break;
                    case "L6010K1":
                    case "L7010K1":
                    case "L1210K1":
                    case "L1211K1":
                    case "L12119K1":
                    case "L1212K1":
                    case "L12116K1":
                        nAmt = argBaseAmt * argMinB; //아산화질소"
                        break;
                    case "L6010K2":
                    case "L7010K2":
                    case "L1210K2":
                    case "L1211K2":
                    case "L12119K2":
                    case "L1212K2":
                    case "L12116K2":
                    case "L12118K2":
                        nAmt = argBaseAmt * argMinB; //산소         //2017 - 05 - 02
                        break;
                    case "L6010K3":
                    case "L7010K3":
                    case "L1210K3":
                    case "L1211K3":
                    case "L12119K3":
                    case "L1212K3":
                    case "L12116K3":
                    case "L12118K3":
                        nAmt = argBaseAmt * argMinC; //소다라임     //2017 - 05 - 02
                        break;
                    case "L6010K4":
                    case "L7010K4":
                    case "L1210K4":
                    case "L1211K4":
                    case "L12119K4":
                    case "L1212K4":
                    case "L12116K4":
                        nAmt = argBaseAmt;               //치오펜탈"
                        break;
                    case "L6010K5":
                    case "L7010K5":
                    case "L1210K5":
                    case "L1211K5":
                    case "L1211K6":
                    case "L12119K5":
                    case "L12119K6":
                    case "L1212K5":
                    case "L1212K6":
                    case "L12116K6":
                        nAmt = argBaseAmt;  //썩시니콜린
                        break;
                    case "L6010K6":
                    case "L7010K6":
                    case "L1210K6":
                        nAmt = argBaseAmt + (argMinE * (argBaseAmt / 2)); //판크로니움
                        break;
                    default:
                        nAmt = (long)(argBaseAmt * argQty * Math.Abs(argNal));
                        break;
                }
            }
            //일반 마취 행위료 계산
            else if (argGb == 1)
            {
                switch (VB.Trim(argSuNext))
                {
                    case "L2010K": nAmt = argBaseAmt; break;              //경막외마취기본
                    case "L3010K": nAmt = argBaseAmt; break;              //척추마취기본
                    case "L6010K": nAmt = argBaseAmt; break;              //전신Mask기본
                    case "L7010K": nAmt = argBaseAmt; break;              //전신Intu기본

                    //2015-08-31
                    case "AP601   ": nAmt = argBaseAmt; break;              //회복관리료

                    //case "L1210K  ": case "L1211K  ": case "L12119K ": case "L1212K  ": case "L1213K  ": case "L1214K  ": case "L1215K  ": case "L12116K ":
                    case "L1210K":
                    case "L12119K":
                    case "L1212K":
                    case "L1213K":
                    case "L1215K":
                    case "L1216K":
                    case "L12116K":
                    case "L12118K":
                    case "L1211K":
                    case "L1214K":   //2017-05-02 L1216K add 2017-07-01
                        nAmt = argBaseAmt; break;            //전신Intu기본
                    case "L12101K": nAmt = argBaseAmt; break;              //척수Intu기본
                    case "L2010K0": nAmt = argBaseAmt * argMinD; break; //경막외마취15단수
                    case "L3010K0": nAmt = argBaseAmt * argMinD; break; //척추마취15단수
                    case "L6010K0": nAmt = argBaseAmt * argMinB; break; //전신Mask15단수
                    case "L7010K0": nAmt = argBaseAmt * argMinB; break; //전신Intu15단수
                    case "L1210K0":
                    case "L12119K0":
                    case "L1212K0":
                    case "L1213K0":
                    case "L1215K0":
                    case "L1216K0":
                    case "L12116K0":
                    case "L12118K0":
                    case "L1211K0":
                    case "L1214K0":  //2017-05-02 L1216K0 add 2017-07-01
                        nAmt = argBaseAmt * argMinD; break;//전신Intu15단수
                    case "L12101K0": nAmt = argBaseAmt * argMinD; break;//척수Intu15단수
                                                                        //2015-08-31
                    case "L0103K": nAmt = argBaseAmt; break;              //감시하 전신마취 기본
                    case "L0104K": nAmt = argBaseAmt * argMinF; break; //감시하 전신마취 15단수

                    default: nAmt = (long)(argBaseAmt * argQty * Math.Abs(argNal)); break;
                }
            }
            else
            {
                nAmt = 0;
            }

            return nAmt;
        }

        /// <summary>
        /// Slip_Retn_AmtAdd Bun 치환
        /// </summary>
        /// <param name="ArgBun"></param>
        /// <returns></returns>
        public string Rtn_Retn_Bun_AmtAdd(string ArgBun)
        {
            string rtnVal = "";

            switch (ArgBun.Trim())
            {
                case "01":
                case "02":
                case "03":
                case "04":
                    rtnVal = "01"; //진찰료
                    break;

                case "11":
                case "12":
                case "13":
                case "14":
                case "15":
                    rtnVal = "02"; //약
                    break;

                case "16":
                case "17":
                case "18":
                case "19":
                case "20":
                    rtnVal = "03"; //주사
                    break;

                case "22":
                case "23":
                case "28":
                case "34":
                case "35":
                case "39":
                    rtnVal = "06"; //처치수술
                    break;

                case "41":
                case "42":
                case "43":
                case "44":
                case "45":
                case "46":
                case "47":
                case "48":
                case "49":
                case "50":
                case "51":
                case "52":
                case "53":
                case "54":
                case "55":
                case "56":
                case "57":
                case "58":
                case "59":
                case "60":
                case "61":
                case "62":
                case "63":
                    rtnVal = "04"; //검사
                    break;

                case "65":
                case "66":
                    rtnVal = "05"; //방사선
                    break;

                case "71":
                case "72":
                case "73":
                    rtnVal = "07"; //CT,MRI,SONO
                    break;

                case "85":
                case "86":
                case "87":
                case "88":
                case "89":
                case "90":
                case "91":
                case "92":
                case "93":
                case "94":
                case "95":
                case "96":
                case "97":
                case "98":
                case "99":
                    rtnVal = "09"; //현금계정
                    break;

                default:
                    rtnVal = "08"; //기타
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// Description : 수가가산 계산 및 EDI 수가 읽기 메인 로직
        /// Author      : 김민철
        /// Create Date : 2018-02-24
        /// <param name="pDbCon">pDbCon</param>
        /// <param name="cBArg">가산항목 Class</param>
        /// <returns>clsPmpaType.Bas_Acc_Rtn</returns>
        /// </summary>
        /// 
         # region 수가 표준계수 조회 // RTN_BAS_SUN_표준계수
        public Double Rtn_Bas_Sun_Standard(PsmhDb pDbCon, string strCode)
        {
            double num = 1;

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT SUHAM                            ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUN    ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                            ";
                SQL = SQL + ComNum.VBLF + "    AND SUNEXT = '" + strCode + "'       ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return 1;
                }
                if (Dt.Rows.Count == 0)
                {
                    Dt.Dispose();
                    Dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return 1;
                }

                num = Convert.ToDouble(Dt.Rows[0]["SUHAM"].ToString());

                Dt.Dispose();
                Dt = null;

                return num;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return 1;
            }

        }
        #endregion
        public clsPmpaType.Bas_Acc_Rtn Rtn_BasAdd_EdiSuga_Amt(PsmhDb pDbCon, clsPmpaType.cBas_Add_Arg cBArg)
        {
            bool bOG = false;
            int nBi = cBArg.Bi;
            string strPCode = string.Empty;
            string strPCodeDtl = string.Empty;
            string strHang = string.Empty;
            Double suham = 1;
            clsIuSentChk cISentChk = new clsIuSentChk();
            clsPmpaType.Bas_Acc_Rtn cBAR = new clsPmpaType.Bas_Acc_Rtn();

            try
            {
                strPCode = cISentChk.Rtn_Bas_Sun_BCode(pDbCon, cBArg.SUNEXT, cBArg.BDATE);  //보험코드
                cBArg.ADD8 = cISentChk.Rtn_Bas_Sun_SUGBAE(pDbCon, cBArg.SUNEXT, cBArg.BDATE);  //신경외과 항목가산
                cBArg.SUGBB = cISentChk.Rtn_Bas_Sun_SUGBB(pDbCon, cBArg.SUNEXT, cBArg.BDATE);  //신경외과 항목가산
                strHang = Bas_Acct_Hang_Set(cBArg.BUN);
                bOG = Rtn_OG_Suga(pDbCon, cBArg.SUNEXT, cBArg.BDATE);  //분만수가여부
              
               
                Bas_Add_Set(ref cBArg, strHang);     //가산항목 세팅

                strPCodeDtl = PCodeDtl_Process(pDbCon, strPCode, cBArg, strHang, bOG);      //최종가산코드를 붙인 EDI코드
                cBAR.BAMT = Convert.ToInt64(Read_EDI_SUGA_PCode(strPCode, cBArg.BDATE));     //원단가

                //보험코드가 이미 가산된코드는 그냥 조회
                if (strPCode.Length > 7)
                {
                    cBAR.BASEAMT = Convert.ToInt64(Read_EDI_SUGA_PCode(strPCode, cBArg.BDATE));  //가산후 단가
                    cBAR.PCODE = strPCode;                                                     //표준코드
                }
                else
                {
                    cBAR.BASEAMT = Convert.ToInt64(Read_EDI_SUGA_PCode(strPCodeDtl, cBArg.BDATE));   //가산후 단가
                    cBAR.PCODE = strPCodeDtl;                                                      //표준코드                                                                        
                }
                //표준계수 확인 2018-11-20

                suham = Rtn_Bas_Sun_Standard( pDbCon, cBArg.SUNEXT);
                if (suham != 1)
                { cBAR.BASEAMT = (long)Math.Truncate(cBAR.BASEAMT * suham)  ; }
                
                // 기술료가산c
                if (cBArg.SUGBE == "1")
                {
                    if (string.Compare(cBArg.BDATE, clsPmpaPb.GISUL_DATE) < 0 && clsPmpaPb.OLD_GISUL[nBi] > 0)
                    {
                        cBAR.AMT = (long)Math.Truncate(cBAR.BASEAMT * (clsPmpaPb.OLD_GISUL[nBi] / 100.0));       //발생금액
                    }
                    else
                    {
                        cBAR.AMT = (long)Math.Truncate(cBAR.BASEAMT * (clsPmpaPb.GISUL[nBi] / 100.0));       //발생금액
                    }
                }
                else
                {
                    cBAR.AMT = cBAR.BASEAMT;
                }

                return cBAR;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 마취가산 중 특정가산만 되도록 세팅
        /// </summary>
        /// <param name="ArgSuNext"></param>
        /// <param name="cBArg"></param>
        /// <param name="strGbChild"></param>
        /// <returns></returns>
        void Chk_Mach_Add_Confirm(string ArgSuNext, clsPmpaType.cBas_Add_Arg cBArg, ref string strGbChild)
        {
            try
            {
                switch (ArgSuNext)
                {
                    case "L1310":
                        strGbChild = "0";
                        cBArg.ADD1 = "";
                        cBArg.ADD2 = "";
                        cBArg.ADD3 = "";
                        cBArg.NIGHT = "";
                        cBArg.MIDNIGHT = "";
                        break;
                    default:
                        break;
                }

                if (cBArg.AN1 == "1") { strGbChild = "0"; }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgSuCode"></param>
        public void Suga_Read(PsmhDb pDbCon, string ArgSuCode)
        {
            DataTable DtAcct = new DataTable();
            string SQL = "";
            string SqlErr = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT Sucode, Bun, Nu,";
            SQL += ComNum.VBLF + "        SugbA, SugbB, SugbC, ";
            SQL += ComNum.VBLF + "        SugbD, SugbE, SugbF, ";
            SQL += ComNum.VBLF + "        SugbG, SugbH, SugbI, ";
            SQL += ComNum.VBLF + "        SugbJ, SugbK, b.SugbM, ";
            SQL += ComNum.VBLF + "        b.SugbO, b.SugbQ, b.SugbR, ";
            SQL += ComNum.VBLF + "        b.SugbW, b.SugbX, Iamt, ";
            SQL += ComNum.VBLF + "        Tamt, Bamt, ";
            SQL += ComNum.VBLF + "        TO_CHAR(a.Sudate, 'yyyy-mm-dd') Suday, ";
            SQL += ComNum.VBLF + "        OldIamt, OldTamt, OldBamt, ";
            SQL += ComNum.VBLF + "        TO_CHAR(a.Sudate3, 'yyyy-mm-dd') Suday3,";
            SQL += ComNum.VBLF + "        Iamt3, Tamt3, Bamt3, ";
            SQL += ComNum.VBLF + "        TO_CHAR(a.Sudate4, 'yyyy-mm-dd') Suday4, ";
            SQL += ComNum.VBLF + "        Iamt4, Tamt4, Bamt4, ";
            SQL += ComNum.VBLF + "        TO_CHAR(a.Sudate5, 'yyyy-mm-dd') Suday5, ";
            SQL += ComNum.VBLF + "        Iamt5, Tamt5, Bamt5, ";
            SQL += ComNum.VBLF + "        a.Sunext, TotMax, b.SugbY, ";
            SQL += ComNum.VBLF + "        b.SugbZ, b.SugbAA, b.SugbAB, ";
            SQL += ComNum.VBLF + "        b.SugbAC, b.SugbAD, b.SugbS ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUT a, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Sucode    = '" + ArgSuCode + "' ";
            SQL += ComNum.VBLF + "    AND a.SuNext  = b.SuNext(+) ";
            SqlErr = clsDB.GetDataTableEx(ref DtAcct, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                DtAcct.Dispose();
                DtAcct = null;
                return;
            }

            if (DtAcct.Rows.Count > 0)
            {
                clsPmpaType.SG.SuCode = DtAcct.Rows[0]["SUCODE"].ToString();
                clsPmpaType.SG.SuNext = DtAcct.Rows[0]["Sunext"].ToString();
                clsPmpaType.SG.Bun = DtAcct.Rows[0]["Bun"].ToString();
                clsPmpaType.SG.Nu = DtAcct.Rows[0]["Nu"].ToString();
                clsPmpaType.SG.SugbA = DtAcct.Rows[0]["SugbA"].ToString();
                clsPmpaType.SG.SugbB = DtAcct.Rows[0]["SugbB"].ToString();
                clsPmpaType.SG.SugbC = DtAcct.Rows[0]["SugbC"].ToString();
                clsPmpaType.SG.SugbD = DtAcct.Rows[0]["SugbD"].ToString();
                clsPmpaType.SG.SugbE = DtAcct.Rows[0]["SugbE"].ToString();
                clsPmpaType.SG.SugbF = DtAcct.Rows[0]["SugbF"].ToString();
                clsPmpaType.SG.SugbG = DtAcct.Rows[0]["SugbG"].ToString();
                clsPmpaType.SG.SugbH = DtAcct.Rows[0]["SugbH"].ToString();
                clsPmpaType.SG.SugbI = DtAcct.Rows[0]["SugbI"].ToString();
                clsPmpaType.SG.SugbJ = DtAcct.Rows[0]["SugbJ"].ToString();
                clsPmpaType.SG.SugbK = DtAcct.Rows[0]["SugbK"].ToString();
                clsPmpaType.SG.SugbL = "";
                clsPmpaType.SG.SugbO = DtAcct.Rows[0]["SugbO"].ToString();
                clsPmpaType.SG.SugbQ = DtAcct.Rows[0]["SugbQ"].ToString();
                clsPmpaType.SG.SugbR = DtAcct.Rows[0]["SugbR"].ToString();
                clsPmpaType.SG.SugbW = DtAcct.Rows[0]["SugbW"].ToString();
                clsPmpaType.SG.SugbX = DtAcct.Rows[0]["SugbX"].ToString();
                clsPmpaType.SG.SugbY = DtAcct.Rows[0]["SugbY"].ToString();
                clsPmpaType.SG.SugbZ = DtAcct.Rows[0]["SugbZ"].ToString();
                clsPmpaType.SG.SugbAA = DtAcct.Rows[0]["SugbAA"].ToString();
                clsPmpaType.SG.SugbAB = DtAcct.Rows[0]["SugbAB"].ToString();
                clsPmpaType.SG.SugbAC = DtAcct.Rows[0]["SugbAC"].ToString();
                clsPmpaType.SG.SugbAD = DtAcct.Rows[0]["SugbAD"].ToString();
                clsPmpaType.SG.SugbS = DtAcct.Rows[0]["SugbS"].ToString();
                clsPmpaType.SG.TotMax = Convert.ToInt32(VB.Val(DtAcct.Rows[0]["TotMax"].ToString()));
                clsPmpaType.SG.OrderNo = 0;
                clsPmpaType.SG.DrCode = "";
                clsPmpaType.SG.Iamt = Convert.ToInt64(VB.Val(DtAcct.Rows[0]["Iamt"].ToString()));
                clsPmpaType.SG.Tamt = Convert.ToInt64(VB.Val(DtAcct.Rows[0]["Tamt"].ToString()));
                clsPmpaType.SG.Bamt = Convert.ToInt64(VB.Val(DtAcct.Rows[0]["Bamt"].ToString()));
                clsPmpaType.SG.Sudate = DtAcct.Rows[0]["Suday"].ToString();
                clsPmpaType.SG.OldIamt = Convert.ToInt64(VB.Val(DtAcct.Rows[0]["OldIamt"].ToString()));
                clsPmpaType.SG.OldTamt = Convert.ToInt64(VB.Val(DtAcct.Rows[0]["OldTamt"].ToString()));
                clsPmpaType.SG.OldBamt = Convert.ToInt64(VB.Val(DtAcct.Rows[0]["OldBamt"].ToString()));
                clsPmpaType.SG.Sudate3 = DtAcct.Rows[0]["SuDay3"].ToString();
                clsPmpaType.SG.Iamt3 = Convert.ToInt64(VB.Val(DtAcct.Rows[0]["Iamt3"].ToString()));
                clsPmpaType.SG.Tamt3 = Convert.ToInt64(VB.Val(DtAcct.Rows[0]["Tamt3"].ToString()));
                clsPmpaType.SG.Bamt3 = Convert.ToInt64(VB.Val(DtAcct.Rows[0]["Bamt3"].ToString()));
                clsPmpaType.SG.Sudate4 = DtAcct.Rows[0]["SuDay4"].ToString();
                clsPmpaType.SG.Iamt4 = Convert.ToInt64(VB.Val(DtAcct.Rows[0]["Iamt4"].ToString()));
                clsPmpaType.SG.Tamt4 = Convert.ToInt64(VB.Val(DtAcct.Rows[0]["Tamt4"].ToString()));
                clsPmpaType.SG.Bamt4 = Convert.ToInt64(VB.Val(DtAcct.Rows[0]["Bamt4"].ToString()));
                clsPmpaType.SG.Sudate5 = DtAcct.Rows[0]["Suday5"].ToString();
                clsPmpaType.SG.Iamt5 = Convert.ToInt64(VB.Val(DtAcct.Rows[0]["Iamt5"].ToString()));
                clsPmpaType.SG.Tamt5 = Convert.ToInt64(VB.Val(DtAcct.Rows[0]["Tamt5"].ToString()));
                clsPmpaType.SG.Bamt5 = Convert.ToInt64(VB.Val(DtAcct.Rows[0]["Bamt5"].ToString()));

                if (string.Compare(clsPmpaType.SG.Sudate, clsPmpaType.a.Date) <= 0)
                {
                    clsPmpaType.SG.Iamt = clsPmpaType.SG.Iamt;
                    clsPmpaType.SG.Tamt = clsPmpaType.SG.Tamt;
                    clsPmpaType.SG.Bamt = clsPmpaType.SG.Bamt;
                }
                else if (string.Compare(clsPmpaType.SG.Sudate3, clsPmpaType.a.Date) <= 0)
                {
                    clsPmpaType.SG.Iamt = clsPmpaType.SG.OldIamt;
                    clsPmpaType.SG.Tamt = clsPmpaType.SG.OldTamt;
                    clsPmpaType.SG.Bamt = clsPmpaType.SG.OldBamt;
                }
                else if (string.Compare(clsPmpaType.SG.Sudate4, clsPmpaType.a.Date) <= 0)
                {
                    clsPmpaType.SG.Iamt = clsPmpaType.SG.Iamt3;
                    clsPmpaType.SG.Tamt = clsPmpaType.SG.Tamt3;
                    clsPmpaType.SG.Bamt = clsPmpaType.SG.Bamt3;
                }
                else if (string.Compare(clsPmpaType.SG.Sudate5, clsPmpaType.a.Date) <= 0)
                {
                    clsPmpaType.SG.Iamt = clsPmpaType.SG.Iamt4;
                    clsPmpaType.SG.Tamt = clsPmpaType.SG.Tamt4;
                    clsPmpaType.SG.Bamt = clsPmpaType.SG.Bamt4;
                }
                else
                {
                    clsPmpaType.SG.Iamt = clsPmpaType.SG.Iamt5;
                    clsPmpaType.SG.Tamt = clsPmpaType.SG.Tamt5;
                    clsPmpaType.SG.Bamt = clsPmpaType.SG.Bamt5;
                }

                clsPmpaType.SG.BaseAmt = clsPmpaType.SG.Bamt;

                if (clsPmpaType.a.Bi > 50) { clsPmpaType.SG.BaseAmt = clsPmpaType.SG.Iamt; }
                if (clsPmpaType.a.Bi == 52 || clsPmpaType.a.Bi == 55) { clsPmpaType.SG.BaseAmt = clsPmpaType.SG.Tamt; }

                //산재환자중 초음파수가는 자보수가를 읽음
                if ((clsPmpaType.a.Bi == 31 || clsPmpaType.a.Bi == 33 || clsPmpaType.a.Bi == 52 || clsPmpaType.a.Bi == 55) && clsPmpaType.SG.SugbX == "1" && clsPmpaType.SG.GbSelf == "0" && clsPmpaType.SG.Bun == "71")
                    clsPmpaType.SG.BaseAmt = clsPmpaType.SG.Tamt;
            }
            else
            {
                clsPmpaType.SG.SuCode = ArgSuCode;
                clsPmpaType.SG.SuNext = ArgSuCode;
                clsPmpaType.SG.SugbA = "1";
                clsPmpaType.SG.SugbB = "0";
                clsPmpaType.SG.SugbC = "0";
                clsPmpaType.SG.SugbD = "0";
                clsPmpaType.SG.SugbE = "0";
                clsPmpaType.SG.SugbF = "1";
                clsPmpaType.SG.SugbG = "1";
                clsPmpaType.SG.SugbH = "0";
                clsPmpaType.SG.SugbI = "0";
                clsPmpaType.SG.SugbJ = "0";
                clsPmpaType.SG.SugbK = "0";
                clsPmpaType.SG.SugbL = "0";
                clsPmpaType.SG.SugbO = "0";
                clsPmpaType.SG.SugbQ = "0";
                clsPmpaType.SG.SugbR = "0";
                clsPmpaType.SG.SugbW = "0";
                clsPmpaType.SG.SugbX = "0";
                clsPmpaType.SG.SugbY = "0";
                clsPmpaType.SG.SugbZ = "0";
                clsPmpaType.SG.DtlBun = "";
                clsPmpaType.SG.SugbS = "0";
                clsPmpaType.SG.Iamt = 0;
                clsPmpaType.SG.Tamt = 0;
                clsPmpaType.SG.Bamt = 0;
                clsPmpaType.SG.Selamt = 0;
                clsPmpaType.SG.GbSpc_No = "0";
                clsPmpaType.SG.Sudate = "";
                clsPmpaType.SG.OldIamt = 0;
                clsPmpaType.SG.OldTamt = 0;
                clsPmpaType.SG.OldBamt = 0;
                clsPmpaType.SG.BaseAmt = 0;
                clsPmpaType.SG.TotMax = 0;
                clsPmpaType.SG.DelDate = "";
                clsPmpaType.SG.OrderNo = 0;
                clsPmpaType.SG.DrCode = "";
                clsPmpaType.SG.Multi = "";
                clsPmpaType.SG.MultiRemark = "";
                clsPmpaType.SG.ScodeSayu = "";
                clsPmpaType.SG.ScodeRemark = "";
                clsPmpaType.SG.Dur = "";
                clsPmpaType.SG.Div = 0;
            }

            DtAcct.Dispose();
            DtAcct = null;
        }

        /// <summary>
        /// 영수증 Bun 치환
        /// </summary>
        /// <param name="ArgBun"></param>
        /// <returns></returns>
        public int Rtn_Reception_Report(string ArgBun)
        {
            int rtnVal = 17;

            switch (ArgBun.Trim())
            {
                case "01":
                case "02":
                case "03":
                case "04":
                    rtnVal = 0; //진찰료
                    break;

                case "11":
                case "12":
                case "13":
                case "14":
                case "15":
                    rtnVal = 2; //투약료및조제료
                    break;

                case "16":
                case "17":
                case "18":
                case "19":
                case "20":
                case "21":
                    rtnVal = 3; //주사료
                    break;

                case "22":
                case "23":
                    rtnVal = 4; //마취료
                    break;

                case "24":
                case "25":
                    rtnVal = 5; //물리치료
                    break;

                case "26":
                case "27":
                    rtnVal = 6; //정신요법료
                    break;

                case "28":
                case "29":
                case "30":
                case "31":
                case "32":
                case "33":
                case "34":
                case "35":
                case "36":
                case "38":
                case "39":
                    rtnVal = 7; //처치수술
                    break;

                case "37":
                    rtnVal = 26; //처치수술
                    break;

                case "41":
                case "42":
                case "43":
                case "44":
                case "45":
                case "46":
                case "47":
                case "48":
                case "49":
                case "50":
                case "51":
                case "52":
                case "53":
                case "54":
                case "55":
                case "56":
                case "57":
                case "58":
                case "59":
                case "60":
                case "61":
                case "62":
                case "63":
                case "64":
                    rtnVal = 8; //검사료
                    break;

                case "65":
                case "66":
                case "67":
                case "68":
                case "69":
                case "70":
                    rtnVal = 9; //방사선료
                    break;

                case "72":
                    rtnVal = 10; //CT
                    break;

                case "73":
                    rtnVal = 11; //MRI
                    break;

                case "71":
                    rtnVal = 12; //SONO
                    break;

                case "40":
                    rtnVal = 13; //보철료
                    break;

                case "75":
                    rtnVal = 14; //증명료
                    break;

                case "99":
                    rtnVal = 24; //영수액
                    break;

                case "98":
                    rtnVal = 19; //조합부담액
                    break;

                case "92":
                    rtnVal = 22; //감액
                    break;

                case "96":
                    rtnVal = 23; //미수액
                    break;

                default:
                    rtnVal = 16; //기타
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// 분만수가 여부 확인
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgSuCode"></param>
        /// <param name="ArgBDate"></param>
        /// <seealso cref="OrderETC.bas : READ_분만수가"/>
        /// <returns></returns>
        public bool Rtn_OG_Suga(PsmhDb pDbCon, string ArgSuCode, string ArgBDate)
        {
            bool rtnVal = false;
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";
            string strCODE = string.Empty;

            if (string.Compare(ArgBDate, "2016-11-01") < 0)
            {
                return rtnVal;
            }

           

            try
            {
                SQL = "";
                SQL += " SELECT CODE FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
                SQL += "  WHERE GUBUN = 'OCS_분만수가코드' ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (Dt.Rows.Count > 0)
                {
                    for (int i = 0; i < Dt.Rows.Count; i++)
                    {
                        strCODE = strCODE + "'" + Dt.Rows[i]["CODE"].ToString().Trim() + "',";
                    }

                    if (strCODE.Trim() != "" && VB.Right(strCODE, 1) == ",")
                    {
                        strCODE = VB.Mid(strCODE, 1, VB.Len(strCODE) - 1);
                    }


                    
                }

                Dt.Dispose();
                Dt = null;

                if (strCODE.Trim() != "")
                {
                    SQL = "";
                    SQL += " SELECT ORDERCODE FROM " + ComNum.DB_MED + "OCS_ORDERCODE   ";
                    SQL += "  WHERE SUCODE IN (" + strCODE + ")                         ";
                    SQL += "    AND ORDERCODE = '" + ArgSuCode + "'                     ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (Dt.Rows.Count > 0)
                        rtnVal = true;

                    Dt.Dispose();
                    Dt = null;

                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        public void Convert_Rate_Argument(clsPmpaType.BonRate cBON)
        {
            //신종 코로나 보건소 지원 분류코드는 본인부담에서 사용안함
            if (cBON.FCODE == "MT04" )
            {
                cBON.FCODE = "";
            }

            //건강보험은 11종으로 통합설정됨
            if (VB.Left(cBON.BI, 1) == "1")
            {
                cBON.BI = "11";
            }
            //건강보험100%은 41종, 0.성인으로 통합설정됨
            else if (VB.Left(cBON.BI, 1) == "4")
            {
                cBON.BI = "41";
            }
                
            if (cBON.DEPT != "NP" && cBON.DEPT != "DT") { cBON.DEPT = "**"; }

            //건강보험 NP진료과는 무시 
            if (cBON.BI == "11" && cBON.DEPT == "NP") { cBON.DEPT = "**"; }

            //2018.05.10. 박병규 : 건강보험 DT진료과는 무시 
            if (cBON.BI == "11" && cBON.DEPT == "DT")
            {
                if (cBON.CHILD != "4")
                    cBON.DEPT = "**";
            }
            if (cBON.BI == "22" && cBON.DEPT == "NP")
            {
                cBON.VCODE = "";
            }

            //2018.05.09. 박병규 : 건강보험 자격코드 (E000,F000)일경우 진료과무시
            if (cBON.BI == "11" && (cBON.MCODE == "E000" || cBON.MCODE == "F000") && cBON.DEPT != "DT") { cBON.DEPT = "**"; }

            //2018.05.10. 박병규 : 건강보험 자격코드(H000)이며, 질환코드없을경우 강제로 VCODE 설정
            if (cBON.BI == "11" && (cBON.MCODE == "H000" && cBON.VCODE == "")) { cBON.VCODE = "V001"; }
            //2018.06.10. 박병규 : 건강보험 자격코드(H000)이며, 질환코드있을경우 강제로 성인으로 설정
            if (cBON.BI == "11" && (cBON.MCODE == "H000" && cBON.VCODE != "")) { cBON.CHILD = "0"; }

            //급여 1 종은 나이구분없음 모두 0.성인 으로 통일함 외래/입원 동일

            if (VB.Left(cBON.BI, 1) == "2")
            {
                //입원부담율중 소아구분인경우는 제외하고 나머지 처리
                if (cBON.OGPDBUN != "P" && cBON.OGPDBUN != "S" && cBON.OGPDBUN != "Y")
                {
                    if (cBON.BI == "22" && cBON.IO == "I")
                    { }
                    else if (cBON.BI == "22" && cBON.CHILD == "5")
                    { }
                    else if (cBON.DEPT == "DT" && cBON.CHILD == "4")
                    { cBON.MCODE = ""; }
                    else
                    { cBON.CHILD = "0"; }

                    if (cBON.DEPT == "DT" &&  cBON.CHILD != "4") { cBON.DEPT = "**"; }

                    //21종 NP는 진료과 무시
                    if (cBON.BI == "21" && cBON.DEPT != "DT" && cBON.CHILD != "4") { cBON.DEPT = "**"; }
                }
            }

            //결핵환자는 진료과 무시
            if (cBON.VCODE == "V000" || cBON.VCODE == "V010") { cBON.DEPT = "**"; }     //잠복결핵 추가(2021-06-28)

            //65세이상 본인부담율은 DT의 임플란트 틀니 부담율 때문에 구분하였으므로 DT인 경우 65세이상은 0.성인으로 분류함
            if (cBON.DEPT != "DT")
            {
                if (cBON.CHILD == "4") { cBON.CHILD = "0"; }
            }

            //산재, 자보, 일반은 아무런 자격조건이 없으므로 Default 세팅
            if (cBON.BI == "31" || cBON.BI == "33" || cBON.BI == "41" || cBON.BI == "51" || cBON.BI == "52" || cBON.BI == "55")
            {
                cBON.CHILD = "0";
                cBON.MCODE = "";
                cBON.VCODE = "";
                cBON.OGPDBUN = "";
                cBON.FCODE = "";
                cBON.DEPT = "**";
                return;
            }

            if (cBON.DEPT == "DT")
            {
                if (cBON.CHILD == "4") { return; }
            }

            //1.외래는 3.6세이상 15세미만 >> 0.성인으로 치환
            if (cBON.IO == "O" && cBON.CHILD == "3") { cBON.CHILD = "0"; }

            //2018.05.29. 박병규 : 건강보험 입원 6세이상 15세미만 >> 0.성인으로 치환
            //2018.06.08  김민철 : 입원 나이구분 무시되어서 아래 코드 주석처리
            //if (cBON.BI == "11" && cBON.IO == "I" && cBON.CHILD == "3") { cBON.CHILD = "0"; }

            //차상위 C000은 본인부담 0% 이므로 나이와 상관없음
            //2018.05.26 박병규 : 차상위 본인부담없음으로 질환코드 구분과 상관없음
            if (cBON.MCODE == "C000")
            {
                cBON.CHILD = "0";
                cBON.VCODE = "";
            }

            //V000 산정특례 나이와 상관없음
            if (cBON.MCODE == "V000") { cBON.CHILD = "0"; }

            //2017.06.14 박병규 : B099 산전지원금 대상자 본인부담은 기본자격의 본인부담을 따라감
            if (cBON.MCODE == "B099") { cBON.MCODE = ""; }
            //EV00 자격은 MCODE가 E000 이거나 F000 인 사람이 중증기호를 달고 있는 경우이며,
            //이때 중증기호는 오더(처방)에서 @V00x 형태로 넘어오므로 받아서 처리함
            //clsPmpaFunc.Rtn_Input_Vcode 함수에서 처리되므로 별도의 작업이 필요하지 않음.
            if (cBON.VCODE == "EV00" && cBON.IO == "O") { cBON.VCODE = ""; }

            //2018.05.14. 박병규 : 의약분업약값30%는 수납시 본인부담 별도산정 하므로 별도의 작업이 필요없음.
            if (cBON.VCODE == "F003") { cBON.VCODE = ""; }

            //2018.05.23. 박병규 :4대중증 의심환자 초음파보험은 본인부담 별도산정 하므로 별도의 작업이 필요없음.
            if (cBON.VCODE == "V999") { cBON.VCODE = ""; }

            //입원이면서 급여 1종, 2종은 별도의 MCODE(M000, M001...) 구분이 없음    (M코드를 적용하지 않는다. 단, H000 제외)
            //입원이면서 급여 1종, 2종은 별도의 MCODE(C000, E000, F000) 구분이 없음 (M코드를 적용하지 않는다.)
            if (cBON.IO == "I" && VB.Left(cBON.BI, 1) == "2")
            {
                if (cBON.MCODE != "H000") { cBON.MCODE = ""; }
            }

            //2018.05.08. 박병규 : 건강보험 임산부외래(F015) 30%, 저체중조산아(F016) 10%
            if (cBON.BI == "11")
            {
                if (cBON.JINDTL == "25" || cBON.VCODE == "F015" || cBON.FCODE == "02")//임산부외래
                {
                    cBON.IO = "O";
                    cBON.VCODE = "";
                    cBON.MCODE = "";
                    cBON.FCODE = "02";
                }     
                if (cBON.JINDTL == "22" || cBON.VCODE == "F016" || cBON.FCODE == "03")//저체중조산아
                {
                    if (cBON.CHILD != "5") { cBON.CHILD = "2"; }
                    cBON.IO = "O";
                    cBON.VCODE = "";
                    cBON.MCODE = "";
                    cBON.FCODE = "03";
                }     
            }

            //2018.05.10. 박병규 : 잠복결핵치료비 지원대상자
            if (cBON.BI == "11")
            {
                if (cBON.JIN == "L") //결핵쿠폰접수시
                {
                    cBON.VCODE = "";
                    cBON.MCODE = "";
                    cBON.FCODE = "";
                    cBON.OGPDBUN = "";
                }

                if (cBON.VCODE == "F009")//검진비지원대상
                {
                    cBON.IO = "O";
                    cBON.VCODE = "";
                    cBON.MCODE = "";
                    cBON.FCODE = "04";
                }

                if (cBON.VCODE == "F010")//치료비지원대상
                {
                    cBON.IO = "O";
                    cBON.VCODE = "";
                    cBON.MCODE = "";
                    cBON.FCODE = "05";
                }
            }

            //2018.05.10. 박병규 : 의료급여 임산부외래(F015) 는 기본본인부담율로 설정
            if (cBON.BI == "21" || cBON.BI == "22")
            {
                if (cBON.JINDTL == "25" || cBON.VCODE == "F015" || cBON.FCODE == "02")//임산부외래
                {
                    cBON.VCODE = "";
                    cBON.MCODE = "";
                    cBON.FCODE = "";
                }
            }

            //2018.06.01. 박병규 : 의료급여 조건부연장승인자 1종
            //2018.06.05. 박병규 : MCODE = 'M000' 추가
            //2018.06.08. 박병규 : MCODE = 'M016' 추가
            if (cBON.BI == "21" && (cBON.MCODE == "M000" ||  cBON.MCODE == "M001" || cBON.MCODE == "M002" || cBON.MCODE == "M003" || cBON.MCODE == "M015" || cBON.MCODE == "M016" || cBON.MCODE == "M018" || cBON.MCODE == "M019"))
            {
                cBON.IO = "O";
                cBON.CHILD = "0";
                cBON.VCODE = "";
                cBON.OGPDBUN = "";
                cBON.FCODE = "";
                cBON.DEPT = "**";
                return;
            }

        
            //2018.06.05. 박병규 : 의료급여 V193
            if (cBON.BI == "21" && cBON.VCODE == "V193")
            {
                //cBON.IO = "O";
                if (cBON.CHILD != "4") { cBON.CHILD = "0"; }
                cBON.MCODE = "";
                cBON.OGPDBUN = "";
                cBON.FCODE = "";
                cBON.DEPT = "**";
                return;
            }

            //TODO : 65세이상 치과중 틀니, 임플란트 아닌 일반진료의 경우 성인자격으로 처리해야함. 
            //JINDTL == "02", JINDTL == "07" 이용

            if (cBON.FCODE == "F013" && cBON.IO == "I")
            {

            }

         

            //잠복결핵관련 세팅
            //잠복결핵 진료비 대상자는 건강보험에 한하므로 타 자격환자들은 잠복결핵 구분 Clear
            if (cBON.FCODE == "04" || cBON.FCODE == "05")
            {
                if (cBON.BI != "11")
                {
                    cBON.FCODE = "";
                }
            }

            if (cBON.FCODE == "F014" && cBON.IO == "I")
            {
                cBON.FCODE = "";
            }

            if (cBON.FCODE == "F013" && cBON.IO == "I")
            {
                cBON.FCODE = "06";
            }

            if (cBON.FCODE == "F010" && cBON.IO == "I") //F010 을 뺀 자기 본인부담을 찾아 급여부분을 지원금으로 돌림
            {
                cBON.FCODE = "";
            }

            //치과는 인공신장당일 본인부담률 타지 않음
            if (cBON.DEPT == "DT" && cBON.CHILD == "4" && (cBON.JINDTL == "02" || cBON.JINDTL == "07"))
            {
                clsPmpaPb.GstrOtherHD = "";
            }   

        }

    }
}

