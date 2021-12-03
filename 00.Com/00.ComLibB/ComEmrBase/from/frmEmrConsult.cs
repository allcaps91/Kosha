using ComBase;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// EMR 작성시 사용하는 컨설트 폼
    /// </summary>
    public partial class frmEmrConsult : Form
    {

        public frmEmrConsult()
        {
            InitializeComponent();
        }

        private void FrmEmrConsult_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}

            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ComFunc.ReadSysDate(clsDB.DbCon);

            fn_DrCbo_Set();

            pnlIpd.Visible = true;
            pnlOpd.Visible = false;

            pnlIpd.Location = new Point(5, 10);

            dtpFrDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-30).ToShortDateString();
            dtpToDate.Text = DateTime.Parse(clsPublic.GstrSysDate).ToShortDateString();

            dtpOutFrDate.Text = "";
            dtpOutToDate.Text = "";

            fn_Read_Transfer();
        }

        void fn_spdCol_Hidden()
        {
            ssConsult1_Sheet1.Columns.Get(14).Visible = false;
            ssConsult1_Sheet1.Columns.Get(15).Visible = false;

            for (int i = 19; i <= 24; i++)
            {
                ssConsult1_Sheet1.Columns.Get(i).Visible = false;
            }

            //ssConsult1_Sheet1.Columns.Get(22).Visible = true;
        }

        void fn_DrCbo_Set()
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;


            try
            {
                SQL = "";
                SQL += " SELECT DRCODE, DRNAME                              \r";
                SQL += "   FROM ADMIN.BAS_DOCTOR                      \r";
                SQL += "  WHERE DRDEPT1 = '" + clsPublic.GstrDeptCode + "'  \r";
                SQL += "    AND TOUR = 'N'                                  \r";
                SQL += "  ORDER BY PRINTRANKING                             \r";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                cboDrCode.Items.Clear();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cboDrCode.Items.Add(dt.Rows[i]["DRCODE"].ToString().Trim() + "." + dt.Rows[i]["DRNAME"].ToString().Trim());
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void fn_Read_Transfer()
        {
            int nMin = 0;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL += " SELECT A.Ptno, A.GbConfirm, TO_CHAR(A.InpDate, 'YYYY-MM-DD') InpDate                               \r";
                SQL += "      , TO_CHAR(A.BDate, 'YYYY-MM-DD') BDate, nvl(A.ToDrCode,'000000') ToDrCode                     \r";
                SQL += "      , A.FrDeptCode Dept, C.DrName, B.RoomCode, B.SName, B.Age, B.Sex, B.DRCODE                    \r";
                SQL += "      , TO_CHAR(B.InDate,'YYYY-MM-DD') InDate, A.FrRemark, A.FrDrCode FrDrCode, A.BInpID, A.GBNST   \r";
                SQL += "      , B.GbSpc, B.Bi, B.DrCode, B.AmSet1, B.WardCode, DECODE(B.ROOMCODE,320,'SICU',321,'MICU',B.WARDCODE) WARdCODE2    \r";
                SQL += "      , TO_CHAR(B.INDATE, 'YYYY-MM-DD') EntDate, A.GbPrint, A.ROWID RID, a.GbSTS,a.Gubun            \r";
                SQL += "      , TO_CHAR(SDATE,'YYYY-MM-DD HH24:MI' ) SDATE , TO_CHAR(EDATE, 'YYYY-MM-DD HH24:MI') EDATE     \r";
                SQL += "      , B.SEX || '/' || B.AGE SEXAGE                                                                \r";
                SQL += "      , ADMIN.FC_BAS_DOCTOR_DRNAME(a.FRDRCODE) FRDRNAME                                        \r";
                SQL += "      , ADMIN.FC_BAS_DOCTOR_DRNAME(a.TODRCODE) TODRNAME                                        \r";
                SQL += "      , ADMIN.FC_OCS_DOCTOR_DRNAME(a.BInpID) BINPIDNAME                                        \r";
                SQL += "      , ADMIN.FC_BAS_DOCTOR_DRNAME(B.DRCODE) CURRDRNAME                                        \r";
                SQL += "      , (SELECT DISTINCT x.WARDCODE || ' ' || x.TEL WARDTEL                                                  \r";
                SQL += "           FROM ADMIN.NUR_TEAM          x                                                     \r";
                SQL += "              , ADMIN.NUR_TEAM_ROOMCODE y                                                     \r";
                SQL += "          WHERE X.WARDCODE = Y.WARDCODE                                                             \r";
                SQL += "            AND x.TEAM = y.TEAM                                                                     \r";
                SQL += "            AND y.ROOMCODE = B.ROOMCODE)  WARDTEL                                                   \r";
                SQL += "      , (SELECT CODE || ' ' || NAME CODENAME                                                        \r";
                SQL += "           FROM ADMIN.BAS_BCODE                                                               \r";
                SQL += "          WHERE GUBUN = 'ETC_병동전화'                                                              \r";
                SQL += "            AND TRIM(CODE) = DECODE(B.ROOMCODE,320,'SICU',321,'MICU',B.WARDCODE)) CODENAME          \r";
                SQL += "      , decode(ADMIN.FC_NUR_FALL_REPORT_CHK(a.PTNO, TRUNC(SYSDATE)), 'Y', '낙', '') FALLSCALE  \r";
                SQL += "      , b.IPDNO, B.WARDCODE, B.AGE                                                                  \r";
                SQL += "      , ADMIN.FC_EXAM_INFECT_MASTER_IMG(a.PTNO, a.BDATE) INFECTION                             \r";
                SQL += "      , a.TODEPTCODE, B.BEDNUM                                                                      \r";
                SQL += "   FROM ADMIN.OCS_ITRANSFER   A                                                                \r";
                SQL += "      , ADMIN.IPD_NEW_MASTER B                                                                \r";
                SQL += "      , ADMIN.BAS_DOCTOR     C                                                                \r";
                
                switch (clsPublic.GstrDeptCode)
                {
                    case "MG":
                    case "MC":
                    case "MP":
                    case "ME":
                    case "MN":
                    case "MR":
                    case "MI":
                        SQL += "  WHERE A.ToDeptCode IN ( '" + clsPublic.GstrDeptCode.Trim() + "', 'MD')                    \r";
                        break;
                    default:
                        if (clsType.User.Sabun.Trim() == "1367")
                        {
                            SQL += "  WHERE A.ToDeptCode IN ( '" + clsPublic.GstrDeptCode.Trim() + "', 'HU')                \r";
                        }
                        else
                        {
                            SQL += "  WHERE A.ToDeptCode = '" + clsPublic.GstrDeptCode.Trim() + "'                          \r";
                        }

                        break;
                }
                if (clsOrdFunction.GstrGbJob == "OPD")
                {
                    if (chkOpt.Checked == true)
                    {
                        if (clsPublic.GstrDeptCode.Trim() == "PC" || clsPublic.GstrDeptCode.Trim() == "EN" || clsPublic.GstrDeptCode.Trim() == "OT")
                        {
                            SQL += "    AND A.TODRCODE IN ( '" + clsOrdFunction.GstrDrCode + "','" + VB.Left(clsOrdFunction.GstrDrCode, 2) + "99" + "' ) \r";
                        }
                        else
                        {
                            SQL += "    AND A.TODRCODE = '" + clsOrdFunction.GstrDrCode + "'                                    \r";
                        }
                    }
                }
                else if (clsOrdFunction.GstrGbJob == "IPD")
                {
                    if (chkStaff.Checked == true && clsOrdFunction.GstrDrCode_N != "")
                    {
                        if (clsPublic.GstrDeptCode.Trim() == "PC" || clsPublic.GstrDeptCode.Trim() == "EN" || clsPublic.GstrDeptCode.Trim() == "OT")
                        {
                            SQL += "    AND A.TODRCODE IN ( '" + clsOrdFunction.GstrDrCode + "','" + VB.Left(clsOrdFunction.GstrDrCode, 2) + "99" + "' ) \r";
                        }
                        else
                        {
                            SQL += "    AND A.TODRCODE = '" + clsOrdFunction.GstrDrCode_N + "'                                    \r";
                        }

                    }
                    else
                    {
                        SQL += "    AND A.TODRCODE = '" + VB.Left(cboDrCode.Text, 4) + "'                                   \r";
                    }
                }
                SQL += "    AND A.GbFlag    = '1'                                                                           \r";
                SQL += "    AND ( A.GbDEL    <> '*'  OR A.GBDEL IS NULL)                                                    \r";
                if (rdoGb1.Checked == true)
                {
                    SQL += "    AND (A.GBCONFIRM IN ( ' ','','T') OR A.GBCONFIRM IS NULL )                                  \r"; //미완료
                }
                else if (rdoGb2.Checked == true)
                {
                    SQL += "    AND A.GBCONFIRM =  '*'                                                                      \r"; //완료
                }
                SQL += "    AND ( A.GBSEND IS NULL OR A.GBSEND =' ' )                                                       \r";
                SQL += "    AND A.Ptno      = B.Pano                                                                        \r";
                if (clsOrdFunction.GstrDrCode.Trim() == "ME" || clsOrdFunction.GstrDrCode.Trim() == "MG")
                {
                    SQL += "    AND a.IPDNO = b.IPDNO                                                                       \r";
                }
                if (chkOut.Checked == true)
                {
                    SQL += "    AND B.GBSTS IN ( '5','6','7')                                                               \r";
                    SQL += "    AND B.OutDate >= TO_DATE('" + dtpFrDate.Text + "','YYYY-MM-DD')                             \r";
                    SQL += "    AND B.OutDate <= TO_DATE('" + dtpToDate.Text + "','YYYY-MM-DD')                             \r";
                }
                else
                {
                    SQL += "    AND B.GBSTS IN ( '0','1','2','3','4')                                                       \r";
                    SQL += "    AND A.BDATE >= TO_DATE('" + dtpFrDate.Text + "','YYYY-MM-DD')                               \r";
                    SQL += "    AND A.BDATE <= TO_DATE('" + dtpToDate.Text + "','YYYY-MM-DD')                               \r";
                }
                //SQL += "    AND A.BDATE >= TO_DATE('2008-07-01','YYYY-MM-DD')                                               \r";
                SQL += "    AND A.FrDrCode  = C.DrCode(+)                                                                   \r";
                SQL += "  ORDER BY  SDATE DESC ,B.RoomCode, A.FrDeptCode                                                    \r";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssConsult1_Sheet1.RowCount = 0;
                ssConsult1_Sheet1.RowCount = dt.Rows.Count;

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["SDATE"].ToString() != "")
                        {
                            if (dt.Rows[i]["EDATE"].ToString() == "")
                            {
                                nMin = Convert.ToInt32(VB.DateDiff("n", Convert.ToDateTime(dt.Rows[i]["SDATE"].ToString()), Convert.ToDateTime(clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime)));
                            }
                            else
                            {
                                nMin = Convert.ToInt32(VB.DateDiff("n", Convert.ToDateTime(dt.Rows[i]["SDATE"].ToString()), Convert.ToDateTime(dt.Rows[i]["EDATE"].ToString())));
                            }
                            ssConsult1.ActiveSheet.Cells[i, 0].Text = clsVbfunc.DATE_ILSU_MIN(nMin);
                        }
                        if (dt.Rows[i]["WARDCODE"].ToString().Trim() == "32" || dt.Rows[i]["WARDCODE"].ToString().Trim() == "33" ||
                            dt.Rows[i]["WARDCODE"].ToString().Trim() == "35")
                        {
                            ssConsult1.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim() + "-" + READ_IPD_BED_NUMBER(dt.Rows[i]["WARDCODE"].ToString().Trim(), dt.Rows[i]["BEDNUM"].ToString().Trim());
                        }
                        else
                        {
                            ssConsult1.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        }

                        ssConsult1.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["DEPT"].ToString();
                        ssConsult1.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["FRDRNAME"].ToString();
                        ssConsult1.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["PTNO"].ToString();
                        ssConsult1.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["SNAME"].ToString();
                        ssConsult1.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["SEXAGE"].ToString();
                        ssConsult1.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["BDATE"].ToString();
                        ssConsult1.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["GBCONFIRM"].ToString();
                        ssConsult1.ActiveSheet.Cells[i, 9].Text = dt.Rows[i]["GBPRINT"].ToString();
                        ssConsult1.ActiveSheet.Cells[i, 10].Text = dt.Rows[i]["INDATE"].ToString();
                        ssConsult1.ActiveSheet.Cells[i, 11].Text = dt.Rows[i]["GBNST"].ToString().Trim() == "*" ? "●" : "";
                        if (dt.Rows[i]["GUBUN"].ToString() == "1")
                        {
                            ssConsult1.ActiveSheet.Cells[i, 12].Text = "○";
                        }
                        else
                        {
                            ssConsult1.ActiveSheet.Cells[i, 12].Text = "X";
                        }

                        ssConsult1.ActiveSheet.Cells[i, 13].Text = dt.Rows[i]["BI"].ToString().Trim();
                        ssConsult1.ActiveSheet.Cells[i, 14].Text = dt.Rows[i]["RID"].ToString().Trim();

                        ssConsult1.ActiveSheet.Cells[i, 15].Text = dt.Rows[i]["AGE"].ToString().Trim();
                        ssConsult1.ActiveSheet.Cells[i, 16].Text = dt.Rows[i]["GBSPC"].ToString().Trim();
                        ssConsult1.ActiveSheet.Cells[i, 17].Text = dt.Rows[i]["BI"].ToString().Trim();
                        ssConsult1.ActiveSheet.Cells[i, 18].Text = dt.Rows[i]["ENTDATE"].ToString().Trim();
                        ssConsult1.ActiveSheet.Cells[i, 19].Text = dt.Rows[i]["DRCODE"].ToString().Trim();

                        ssConsult1.ActiveSheet.Cells[i, 20].Text = dt.Rows[i]["AMSET1"].ToString().Trim();
                        ssConsult1.ActiveSheet.Cells[i, 21].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                        ssConsult1.ActiveSheet.Cells[i, 22].Text = "    ";
                        ssConsult1.ActiveSheet.Cells[i, 23].Text = dt.Rows[i]["TODRCODE"].ToString().Trim();
                        ssConsult1.ActiveSheet.Cells[i, 24].Text = dt.Rows[i]["IPDNO"].ToString().Trim();

                        if (dt.Rows[i]["GBSTS"].ToString() == "0")
                        {
                            ssConsult1.ActiveSheet.Cells[i, 25].Text = "○";
                        }
                        else
                        {
                            ssConsult1.ActiveSheet.Cells[i, 25].Text = "X";
                        }

                        ssConsult1.ActiveSheet.Cells[i, 26].Text = dt.Rows[i]["FALLSCALE"].ToString();

                        if (dt.Rows[i]["WARDTEL"].ToString() != "")
                        {
                            ssConsult1.ActiveSheet.Cells[i, 27].Text = dt.Rows[i]["WARDTEL"].ToString();
                        }
                        else
                        {
                            if (dt.Rows[i]["CODENAME"].ToString() != "")
                            {
                                ssConsult1.ActiveSheet.Cells[i, 27].Text = dt.Rows[i]["CODENAME"].ToString();
                            }
                            else
                            {
                                ssConsult1.ActiveSheet.Cells[i, 27].Text = dt.Rows[i]["WARDCODE"].ToString();
                            }
                        }

                        ssConsult1.ActiveSheet.Cells[i, 28].Text = dt.Rows[i]["ROOMCODE"].ToString();
                        ssConsult1.ActiveSheet.Cells[i, 29].Text = dt.Rows[i]["AGE"].ToString();
                        //ssConsult1.ActiveSheet.Cells[i, 30].Value = clsConPatInfo.InfactResource(dt.Rows[i]["INFECTION"].ToString().Trim());
                        ssConsult1.ActiveSheet.Cells[i, 31].Text = dt.Rows[i]["TODRCODE"].ToString();
                        ssConsult1.ActiveSheet.Cells[i, 32].Text = dt.Rows[i]["TODEPTCODE"].ToString();

                        if (nMin > 1440)
                        {
                            ssConsult1.ActiveSheet.Cells[i, 0, i, ssConsult1.ActiveSheet.ColumnCount - 1].ForeColor = Color.Red;
                        }
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            try
            {
                SQL = "";
                SQL += " SELECT A.Ptno, A.GbConfirm, TO_CHAR(A.InpDate, 'YYYY-MM-DD') InpDate, a.RETURN                     \r";
                SQL += "      , TO_CHAR(A.BDate, 'YYYY-MM-DD') BDate, nvl(A.ToDrCode,'000000') FRDrCode                     \r";
                SQL += "      , A.TODeptCode Dept, C.DrName, B.RoomCode, B.SName, B.Age, B.Sex, B.DRCODE                    \r";
                SQL += "      , TO_CHAR(B.InDate,'YYYY-MM-DD') InDate, A.FrRemark, A.TODrCode TODRCODE, A.BInpID, A.GBNST   \r";
                SQL += "      , B.GbSpc, B.Bi, B.DrCode, B.AmSet1, B.WardCode, B.WARDCODE                                   \r";
                SQL += "      , TO_CHAR(B.INDATE, 'YYYY-MM-DD') EntDate, A.GbPrint, A.ROWID RID,a.GbSTS,a.Gubun             \r";
                SQL += "      , TO_CHAR(SDATE,'YYYY-MM-DD HH24:MI' ) SDATE , TO_CHAR(EDATE, 'YYYY-MM-DD HH24:MI') EDATE     \r";
                SQL += "      , B.SEX || '/' || B.AGE SEXAGE                                                                \r";
                SQL += "      , ADMIN.FC_BAS_DOCTOR_DRNAME(a.FRDRCODE) FRDRNAME                                        \r";
                SQL += "      , ADMIN.FC_BAS_DOCTOR_DRNAME(a.TODRCODE) TODRNAME                                        \r";
                SQL += "      , ADMIN.FC_OCS_DOCTOR_DRNAME(a.BInpID) BINPIDNAME                                        \r";
                SQL += "      , ADMIN.FC_BAS_DOCTOR_DRNAME(B.DRCODE) CURRDRNAME                                        \r";
                SQL += "      , (SELECT DISTINCT x.WARDCODE || ' ' || x.TEL WARDTEL                                                  \r";
                SQL += "           FROM ADMIN.NUR_TEAM          x                                                     \r";
                SQL += "              , ADMIN.NUR_TEAM_ROOMCODE y                                                     \r";
                SQL += "          WHERE X.WARDCODE = Y.WARDCODE                                                             \r";
                SQL += "            AND x.TEAM = y.TEAM                                                                     \r";
                SQL += "            AND y.ROOMCODE = B.ROOMCODE)  WARDTEL                                                   \r";
                SQL += "      , (SELECT CODE || ' ' || NAME CODENAME                                                        \r";
                SQL += "           FROM ADMIN.BAS_BCODE                                                               \r";
                SQL += "          WHERE GUBUN = 'ETC_병동전화'                                                              \r";
                SQL += "            AND TRIM(CODE) = DECODE(B.ROOMCODE,320,'SICU',321,'MICU',B.WARDCODE)) CODENAME          \r";
                SQL += "   FROM ADMIN.OCS_ITRANSFER   A                                                                \r";
                SQL += "      , ADMIN.IPD_NEW_MASTER B                                                                \r";
                SQL += "      , ADMIN.BAS_DOCTOR     C                                                                \r";
                switch (clsPublic.GstrDeptCode)
                {
                    case "MG":
                    case "MC":
                    case "MP":
                    case "ME":
                    case "MN":
                    case "MR":
                    case "MI":
                        SQL += "  WHERE A.FrDeptCode IN ( '" + clsPublic.GstrDeptCode.Trim() + "', 'MD')                    \r";
                        break;
                    default:
                        SQL += "  WHERE A.FrDeptCode = '" + clsPublic.GstrDeptCode.Trim() + "'                              \r";
                        break;
                }
                if (clsOrdFunction.GstrGbJob == "OPD")
                {
                    if (chkOpt.Checked == true)
                    {
                        if (clsPublic.GstrDeptCode.Trim() == "PC")
                        {
                            SQL += "    AND A.FRDRCODE IN ( '" + clsOrdFunction.GstrDrCode + "','" + VB.Left(clsOrdFunction.GstrDrCode, 2) + "99" + "' ) \r";
                        }
                        else
                        {
                            SQL += "    AND A.FRDRCODE = '" + clsOrdFunction.GstrDrCode + "'                                    \r";
                        }
                    }
                }
                else if (clsOrdFunction.GstrGbJob == "IPD")
                {
                    if (chkStaff.Checked == true && clsOrdFunction.GstrDrCode_N != "")
                    {
                        SQL += "    AND A.FRDRCODE = '" + clsOrdFunction.GstrDrCode_N + "'                                  \r";
                    }
                    else
                    {
                        SQL += "    AND A.FRDRCODE = '" + VB.Left(cboDrCode.Text, 4) + "'                                   \r";
                    }
                }
                SQL += "    AND A.GbFlag    = '1'                                                                           \r";
                SQL += "    AND ( A.GbDEL    <> '*'  OR A.GBDEL IS NULL)                                                    \r";
                if (rdoGb1.Checked == true)
                {
                    SQL += "    AND (A.GBCONFIRM IN (' ','','T')   OR A.GBCONFIRM IS NULL )                                 \r"; //미완료
                }
                else if (rdoGb2.Checked == true)
                {
                    SQL += "    AND A.GBCONFIRM =  '*'                                                                      \r"; //완료
                }
                SQL += "    AND ( A.GBSEND IS NULL OR A.GBSEND =' ' )                                                       \r";
                SQL += "    AND A.Ptno      = B.Pano                                                                        \r";
                SQL += "    AND a.IPDNO = b.IPDNO                                                                       \r";
                SQL += "    AND B.GBSTS IN ( '0')                                                                           \r";
                SQL += "    AND A.BDATE >= TO_DATE('" + dtpFrDate.Text + "','YYYY-MM-DD')                                   \r";
                SQL += "    AND A.BDATE <= TO_DATE('" + dtpToDate.Text + "','YYYY-MM-DD')                                   \r";
                //SQL += "    AND A.BDATE >= TO_DATE('2008-07-01','YYYY-MM-DD')                                               \r";
                SQL += "    AND A.FrDrCode  = C.DrCode(+)                                                                   \r";
                //if (clsOrdFunction.GstrDrCode.Trim() == "ME" || clsOrdFunction.GstrDrCode.Trim() == "MG")
                //{
                //    SQL += "    AND a.IPDNO = b.IPDNO                                                                       \r";
                //}
                //19-01-29 수정
                SQL += "  ORDER BY  EDATE DESC, RETURN DESC, A.TODeptCode                                                   \r";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssConsult2_Sheet1.RowCount = 0;
                ssConsult2_Sheet1.RowCount = dt.Rows.Count;

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["SDATE"].ToString() != "")
                        {
                            if (dt.Rows[i]["EDATE"].ToString() == "")
                            {
                                nMin = Convert.ToInt32(VB.DateDiff("n", Convert.ToDateTime(dt.Rows[i]["SDATE"].ToString()), Convert.ToDateTime(clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime)));
                            }
                            else
                            {
                                nMin = Convert.ToInt32(VB.DateDiff("n", Convert.ToDateTime(dt.Rows[i]["SDATE"].ToString()), Convert.ToDateTime(dt.Rows[i]["EDATE"].ToString())));
                            }
                            ssConsult2.ActiveSheet.Cells[i, 0].Text = clsVbfunc.DATE_ILSU_MIN(nMin);
                        }

                        ssConsult2.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["ROOMCODE"].ToString();
                        ssConsult2.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["DEPT"].ToString();
                        ssConsult2.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["DRNAME"].ToString();
                        ssConsult2.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["PTNO"].ToString();
                        ssConsult2.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["SNAME"].ToString();
                        ssConsult2.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["SEXAGE"].ToString();
                        ssConsult2.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["BDATE"].ToString();
                        ssConsult2.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["GBCONFIRM"].ToString();
                        ssConsult2.ActiveSheet.Cells[i, 9].Text = dt.Rows[i]["GBPRINT"].ToString();
                        ssConsult2.ActiveSheet.Cells[i, 10].Text = dt.Rows[i]["RETURN"].ToString() == "*" ? "◎" : "";
                        ssConsult2.ActiveSheet.Cells[i, 11].Text = dt.Rows[i]["INDATE"].ToString();
                        ssConsult2.ActiveSheet.Cells[i, 12].Text = dt.Rows[i]["GBNST"].ToString();
                        if (dt.Rows[i]["GUBUN"].ToString() == "1")
                        {
                            ssConsult2.ActiveSheet.Cells[i, 13].Text = "○";
                        }
                        else
                        {
                            ssConsult2.ActiveSheet.Cells[i, 13].Text = "X";
                        }

                        ssConsult2.ActiveSheet.Cells[i, 14].Text = dt.Rows[i]["BI"].ToString();
                        ssConsult2.ActiveSheet.Cells[i, 15].Text = dt.Rows[i]["RID"].ToString();
                        ssConsult2.ActiveSheet.Cells[i, 16].Text = dt.Rows[i]["DRCODE"].ToString();
                        if (dt.Rows[i]["GBSTS"].ToString() == "0")
                        {
                            ssConsult2.ActiveSheet.Cells[i, 17].Text = "○";
                        }
                        else
                        {
                            ssConsult2.ActiveSheet.Cells[i, 17].Text = "X";
                        }
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private string READ_IPD_BED_NUMBER(string strWard, string strCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     Name";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'NUR_ICU_침상번호'";
                SQL = SQL + ComNum.VBLF + "         AND TRIM(CODE) = '" + strCode + "' ";
                SQL = SQL + ComNum.VBLF + "         AND (DELDATE IS NULL OR DELDATE = '') ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog("함수명 : " + "READ_IPD_BED_NUMBER" + ComNum.VBLF + SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["NAME"].ToString().Trim();
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

                ComFunc.MsgBoxEx(this, "함수명 : " + "READ_IPD_BED_NUMBER" + ComNum.VBLF + ex.Message);
                clsDB.SaveSqlErrLog("함수명 : " + "READ_IPD_BED_NUMBER" + ComNum.VBLF + ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }


        private void rdoGb1_Click(object sender, EventArgs e)
        {
            btnSearch_Click(btnSearch, new EventArgs());
        }

        private void rdoGb2_Click(object sender, EventArgs e)
        {
            btnSearch_Click(btnSearch, new EventArgs());
        }

        private void rdoGb3_Click(object sender, EventArgs e)
        {
            btnSearch_Click(btnSearch, new EventArgs());
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            fn_Read_Transfer();
        }

        private void ChkStaff_Click(object sender, EventArgs e)
        {
            clsOrdFunction.GstrSelPatient = "정상";

            if (chkStaff.Checked == true)
            {
                cboDrCode.Enabled = false;
            }
            else
            {
                cboDrCode.Enabled = true;
            }

            btnSearch_Click(btnSearch, new EventArgs());
        }

        private void SsConsult1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader == true)
            {
                clsSpread.gSpdSortRow(ssConsult1, e.Column);
                return;
            }

            if (clsOrdFunction.GstrGbJob == "IPD")
            {
                clsOrdFunction.GstrOrdersViewOrder = "FR";
            }
        }

        private void SsConsult1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true) { return; }

            clsOrdFunction.GstrOrdersViewOrder = "FR";
            clsPublic.GstrToRemark = "";

            string strRowId = ssConsult1.ActiveSheet.Cells[e.Row, 14].Text.Trim();
            ReadCounsult(strRowId);

            btnSearch_Click(btnSearch, new EventArgs());
        }

        private void SsConsult2_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader == true)
            {
                clsSpread.gSpdSortRow(ssConsult2, e.Column);
                return;
            }
        }

        private void SsConsult2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strRowId = ssConsult2.ActiveSheet.Cells[e.Row, 15].Text;

            ReadCounsult(strRowId);
        }

        void ReadCounsult(string strRowId)
        {

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;


            try
            {
                SQL = "";
                SQL += " SELECT A.Ptno, A.GbConfirm, TO_CHAR(A.InpDate, 'YYYY-MM-DD') InpDate, A.TODEPTCODE,  A.FRDeptCode          \r";
                SQL += "      , TO_CHAR(A.BDate, 'YYYY-MM-DD') BDate, nvl(A.ToDrCode,'000000') ToDrCode                             \r";
                SQL += "      , A.FrDeptCode Dept, C.DrName, B.RoomCode, B.SName, B.Age, B.Sex, B.GBPT, B.IPDNO,a.GbSTS             \r";
                SQL += "      , TO_CHAR(B.InDate,'YYYY-MM-DD') InDate, A.FrRemark, A.TOREMARK,  A.FrDrCode FrDrCode, A.INPID        \r";
                SQL += "      , A.BInpID,  a.PICXY, a.Gubun,a.Bohum,TO_CHAR(a.KDATE1, 'YYYY-MM-DD') KDate1                          \r";
                SQL += "      , TO_CHAR(a.KDATE2, 'YYYY-MM-DD') KDate2,TO_CHAR(a.KDATE3, 'YYYY-MM-DD') KDate3                       \r";
                SQL += "      , TO_CHAR(a.KDATE4, 'YYYY-MM-DD') KDate4                                                              \r";
                SQL += "      , B.GbSpc, B.Bi, B.DrCode, B.AmSet1, B.WardCode                                                       \r";
                SQL += "      , TO_CHAR(B.INDATE, 'YYYY-MM-DD') EntDate, A.GbPrint, A.ROWID                                         \r";
                SQL += "      , ADMIN.FC_BAS_DOCTOR_DRNAME(TRIM(A.FRDRCODE)) FRDRNAME                                          \r";
                SQL += "      , ADMIN.FC_BAS_DOCTOR_DRNAME(TRIM(A.TODRCODE)) TODRNAME                                          \r";
                SQL += "      , ADMIN.FC_BAS_USER_USERNAME(A.BINPID) BINPNAME                                                  \r";
                SQL += "      , ADMIN.FC_BAS_USER_USERNAME(A.INPID) INPNAME                                                    \r";
                SQL += "      , to_char(D.BIRTH, 'yyyy-mm-dd') BIRTH                                                                \r";
                SQL += "   FROM ADMIN.OCS_ITRANSFER   A                                                                        \r";
                SQL += "      , ADMIN.IPD_NEW_MASTER B                                                                        \r";
                SQL += "      , ADMIN.BAS_DOCTOR     C                                                                        \r";
                SQL += "      , ADMIN.BAS_PATIENT    D                                                                        \r";
                SQL += "  WHERE A.ROWID = '" + strRowId + "'                                                                        \r";
                SQL += "    AND A.Ptno     = B.Pano                                                                                 \r";
                SQL += "    AND A.IPDNO    = B.IPDNO                                                                                \r";
                SQL += "    AND A.Ptno     = D.Pano                                                                                 \r";
                SQL += "    AND B.GBSTS IN ( '0','1','2','3','4','5','6','7')                                                       \r";
                SQL += "    AND A.FrDrCode = C.DrCode(+)                                                                            \r";
                SQL += "  ORDER BY B.RoomCode, A.FrDeptCode                                                                         \r";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (clsOrdFunction.GstrOrdersViewOrder == "TO" && dt.Rows[0]["GbConfirm"].ToString().Trim() == "T")
                {
                    ComFunc.MsgBoxEx(this, "컨설트 임시저장 상태입니다.. 아직 확정내용이 아니니 참고하십시오!!", "확인");
                }

                if (dt.Rows.Count > 0)
                {

                    txtFrRemark.Text = dt.Rows[0]["FRREMARK"].ToString();
                    if (clsPublic.GstrToRemark == "")
                    {
                        txtToRemark.Text = dt.Rows[0]["TOREMARK"].ToString();
                    }

                    //거동여부
                    lblSts.Text = "";
                    lblSts.ForeColor = Color.Black;
                    lblSts.BackColor = Color.RoyalBlue;

                    if (dt.Rows[0]["GBSTS"].ToString() == "0")
                    {
                        lblSts.Text = "거동가능";
                        lblSts.ForeColor = Color.White;
                        lblSts.BackColor = Color.DeepSkyBlue;
                    }
                    else if (dt.Rows[0]["GBSTS"].ToString() == "1")
                    {
                        lblSts.Text = "거동불가능";
                        lblSts.ForeColor = Color.Red;
                        lblSts.BackColor = Color.Yellow;
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }
    }
}
