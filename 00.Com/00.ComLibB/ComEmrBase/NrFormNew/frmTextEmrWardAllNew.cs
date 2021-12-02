using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using ComBase;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using Oracle.DataAccess.Client;

namespace ComEmrBase
{
    public partial class frmTextEmrWardAllNew : Form
    {
        #region // 기록지 관련 변수(기록지번호, 명칭 등 수정요함)
        //Form mCallForm = null;
        //FormEmrMessage mEmrCallForm = null;
        //EmrPatient pAcp = null;
        EmrForm fWrite = null;

        public string mstrFormNo = "";
        public string mstrFORMNAME = "";
        public string mstrUpdateNo = "0";
        public string mstrEmrNo = "0";  //961 131641  //963 735603
        public string mstrMode = "W";

        const int mlngStartCol = 13;
        const int mlngPtStartCol = 2;

        int intCol  = 0;
        int intCol2 = 0;
        int intCol3 = 0;
        #endregion

        #region TOP 메뉴 시간 관련
        usTimeSet usTimeSetEvent = null;
        #endregion

        #region //생성자
        public frmTextEmrWardAllNew()
        {
            InitializeComponent();
        }

        private void frmTextEmrWardAllNew_Load(object sender, EventArgs e)
        {
            dtpChartDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            txtMedFrTime.Text = dtpChartDate.Value.ToString("HH:mm");

            ssUserChart_Sheet1.ColumnHeader.Visible = true;
            ssUserChart_Sheet1.ColumnCount = mlngStartCol;
            ssUserChart_Sheet1.RowCount = 0;

            ssFORM_Sheet1.ColumnCount = mlngStartCol;
            ssFORM_Sheet1.RowCount = 0;

            lblFORMNAME.Text = "";

            SetCombo();
            GetUserChart();
            GetPatList();
        }

        #endregion //생성자

        #region //Private Function

        /// <summary>
        /// 콤보박스 설정
        /// </summary>
        void SetCombo()
        {
            OracleDataReader reader = null;
            string SQL = string.Empty;

            cboTeam.Items.Clear();
            cboTeam.Items.Add("전체");
            cboTeam.Items.Add("A");
            cboTeam.Items.Add("B");
            cboTeam.SelectedIndex = 0;

            cboJob.Items.Clear();
            cboJob.Items.Add("1.재원자명단");
            cboJob.Items.Add("2.당일입원자");
            cboJob.Items.Add("3.퇴원예고자");
            cboJob.Items.Add("4.당일퇴원자");
            cboJob.Items.Add("5.중증도미분류");
            cboJob.Items.Add("6.수술예정자");
            cboJob.Items.Add("7.진단명 누락자");
            cboJob.Items.Add("A.응급실경유입원(1-3일전)");
            cboJob.Items.Add("B.재원기간 7-14일 환자");
            cboJob.Items.Add("C.재원기간 3-7일 환자");
            cboJob.Items.Add("D.어제퇴원자");

            #region ComboWard_SET()
            cboWard.Items.Clear();
            cboWard.Items.Add("전체");

            int sIndex = -1;
            int sCount = 0;

            try
            {
                SQL = " SELECT NAME WARDCODE, MATCH_CODE";
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
                        if (reader.GetValue(0).ToString().Trim() != "ER" && reader.GetValue(0).ToString().Trim() != "HD")
                        {
                            cboWard.Items.Add(reader.GetValue(0).ToString().Trim());
                            if (reader.GetValue(1).ToString().Trim().Equals(clsType.User.BuseCode))
                            {
                                sIndex = sCount;
                            }
                            sCount += 1;
                        }
                    }
                }

                //cboWard.Items.Add("HD");
                //cboWard.Items.Add("ER");
                //cboWard.Items.Add("OP");
                //cboWard.Items.Add("ENDO");
                //cboWard.Items.Add("외래수혈");

                cboWard.SelectedIndex = sIndex == -1 ? cboWard.SelectedIndex : sIndex + 1;

                reader.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
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
            #endregion

            #region ComboVRoom
            cboVRoom.Items.Clear();
            cboVRoom.Items.Add("전체");
            SQL = string.Empty;

            try
            {
                SQL = " SELECT ROOMCODE FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " Where ACTDATE Is Null ";
                SQL = SQL + ComNum.VBLF + "  AND PANO NOT IN ('81000004') ";
                SQL = SQL + ComNum.VBLF + "  AND GBSTS <> '9' ";

                switch (cboWard.Text.Trim())
                {
                    case "전체":
                        SQL = SQL + ComNum.VBLF + "  AND  WardCode>' ' ";
                        break;
                    case "MICU":
                        SQL = SQL + ComNum.VBLF + "  AND  RoomCode='234' ";
                        break;
                    case "SICU":
                        SQL = SQL + ComNum.VBLF + "  AND  RoomCode='233' ";
                        break;
                    case "ND":
                    case "NR":
                        SQL = SQL + ComNum.VBLF + "  AND  WardCode IN ('ND','IQ','NR') ";
                        break;
                    default:
                        SQL = SQL + ComNum.VBLF + "  AND  WardCode = '" + cboWard.Text.Trim() + "' ";
                        break;
                }

                SQL = SQL + ComNum.VBLF + " GROUP BY ROOMCODE ";
                SQL = SQL + ComNum.VBLF + " ORDER BY ROOMCODE ";

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
                        cboVRoom.Items.Add(reader.GetValue(0).ToString().Trim());
                    }
                }

                cboVRoom.SelectedIndex = 0;

                reader.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
            #endregion

            #region ComboVDr
            cboVDr.Items.Clear();
            cboVDr.Items.Add("전체");
            SQL = string.Empty;

            try
            {
                SQL = SQL + ComNum.VBLF + "SELECT DRNAME, A.DRCODE";
                SQL = SQL + ComNum.VBLF + "FROM (";
                SQL = SQL + ComNum.VBLF + " SELECT DRCODE FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " Where ACTDATE Is Null ";
                SQL = SQL + ComNum.VBLF + "  AND PANO NOT IN ('81000004') ";
                SQL = SQL + ComNum.VBLF + "  AND GBSTS <> '9' ";

                switch (cboWard.Text.Trim())
                {
                    case "전체":
                        SQL = SQL + ComNum.VBLF + "  AND  WardCode>' ' ";
                        break;
                    case "MICU":
                        SQL = SQL + ComNum.VBLF + "  AND  RoomCode = '234' ";
                        break;
                    case "SICU":
                        SQL = SQL + ComNum.VBLF + "  AND  RoomCode = '233' ";
                        break;
                    case "ND":
                    case "NR":
                        SQL = SQL + ComNum.VBLF + "  AND  WardCode IN ('ND','IQ','NR') ";
                        break;
                    default:
                        SQL = SQL + ComNum.VBLF + "  AND  WardCode = '" + cboWard.Text.Trim() + "' ";
                        break;
                }

                SQL = SQL + ComNum.VBLF + " GROUP BY DRCODE ";
                SQL = SQL + ComNum.VBLF + ") A";
                SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_OCS.OCS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + "   ON A.DRCODE = B.DRCODE";
                SQL = SQL + ComNum.VBLF + " ORDER BY A.DRCODE ";

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
                        cboVDr.Items.Add(reader.GetValue(0).ToString().Trim() + "." + reader.GetValue(1).ToString().Trim());
                    }
                }

                cboVDr.SelectedIndex = 0;

                reader.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
            #endregion
        }

        /// <summary>
        /// 일괄작업 폼 리스트를 가지고 온다
        /// </summary>
        void GetUserChart()
        {
            OracleDataReader reader = null;
            string SQL = string.Empty;

            try
            {
                SQL = "SELECT  A.GRPFORMNAME, B.FORMNAME, B.DISPSEQ, B.FORMNO,  A.GRPFORMNO, B.UPDATENO  ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRGRPFORM A ";
                SQL = SQL + ComNum.VBLF + "      INNER JOIN " + ComNum.DB_EMR + "AEMRFORM B";
                SQL = SQL + ComNum.VBLF + "          ON A.GRPFORMNO = B.GRPFORMNO";
                SQL = SQL + ComNum.VBLF + "          AND B.FORMNO IN (1572, 3150, 2201)";
                SQL = SQL + ComNum.VBLF + "  WHERE B.OLDGB = '0'";
                //SQL = SQL + ComNum.VBLF + "  WHERE (B.USECHECK IS NULL ";
                //SQL = SQL + ComNum.VBLF + "      OR B.USECHECK = '0')";
                //SQL = SQL + ComNum.VBLF + "  ORDER BY A.GRPFORMNAME, B.FORMNAME";
                SQL = SQL + ComNum.VBLF + "  ORDER BY DECODE(FORMNO, 3150, 0, 1572, 1, 2201, 2)";

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (reader.HasRows)
                {
                    ssFORM_Sheet1.RowCount = 0;
                    while (reader.Read())
                    {
                        if (reader.GetValue(3).ToString().Trim().Equals("2201") && clsType.User.BuseCode.Equals("033108") == false)
                        {
                            continue;
                        }

                        ssFORM_Sheet1.RowCount += 1;
                        ssFORM_Sheet1.Cells[ssFORM_Sheet1.RowCount - 1, 0].Text = reader.GetValue(0).ToString().Trim();
                        ssFORM_Sheet1.Cells[ssFORM_Sheet1.RowCount - 1, 1].Text = reader.GetValue(1).ToString().Trim();
                        ssFORM_Sheet1.Cells[ssFORM_Sheet1.RowCount - 1, 2].Text = reader.GetValue(2).ToString().Trim();
                        ssFORM_Sheet1.Cells[ssFORM_Sheet1.RowCount - 1, 3].Text = reader.GetValue(3).ToString().Trim();
                        ssFORM_Sheet1.Cells[ssFORM_Sheet1.RowCount - 1, 4].Text = reader.GetValue(4).ToString().Trim();


                    }
                }

                cboVRoom.SelectedIndex = 0;

                reader.Dispose();


                if (ssFORM_Sheet1.RowCount > 0)
                {
                    ssFORMCellClick(0);
                }

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        /// <summary>
        /// 환자리스트를 가지고 온다
        /// </summary>
        void GetPatList()
        {
            DataTable dt = null;
            string SQL = string.Empty;

            try
            {
                ssUserChart_Sheet1.RowCount = 0;

                string strJob = VB.Left(cboJob.Text, 1);

                string strPriDate = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).AddDays(-1).ToShortDateString();
                string strToDate = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).AddDays(0).ToShortDateString();
                string strNextDate = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).AddDays(1).ToShortDateString();

                if (cboWard.Text.Trim() != "HD")
                {
                    SQL = "SELECT M.WardCode,M.RoomCode,M.Pano,M.SName,M.Sex,M.Age,M.Bi,M.PName,";
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(M.InDate,'YYYY-MM-DD') InDate,M.Ilsu,M.IpdNo,M.GbSts,";
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(M.OutDate,'YYYY-MM-DD') OutDate,";
                    SQL = SQL + ComNum.VBLF + " M.DeptCode,M.DrCode,D.DrName,M.AmSet1,M.AmSet4,M.AmSet6,M.AmSet7 ";
                    SQL = SQL + ComNum.VBLF + " FROM   KOSMOS_PMPA.IPD_NEW_MASTER  M, ";
                    SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.BAS_PATIENT P, ";
                    SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.BAS_DOCTOR  D ";

                    switch (cboWard.Text.Trim())
                    {
                        case "전체": SQL = SQL + ComNum.VBLF + "WHERE M.WardCode>' ' "; break;
                        case "MICU": SQL = SQL + ComNum.VBLF + "WHERE M.RoomCode='234' "; break;
                        case "SICU": SQL = SQL + ComNum.VBLF + "WHERE M.RoomCode='233' "; break;
                        case "ND":
                        case "NR":
                            SQL = SQL + ComNum.VBLF + "WHERE M.WardCode IN ('ND','IQ','NR') "; break;
                        //'Case "3B":   SQL = SQL + ComNum.VBLF + "WHERE M.WardCode IN ('3B','DR') "; //'COMBOBOX 처리
                        default: SQL = SQL + ComNum.VBLF + "WHERE M.WardCode='" + cboWard.Text.Trim() + "' "; break;
                    }



                    //if (clsType.User.IdNumber != "4349") SQL = SQL + ComNum.VBLF + "  AND M.Pano<>'81000004' ";

                    //'작업분류
                    if (strJob == "1") //'재원자
                    {
                        SQL = SQL + ComNum.VBLF + " AND (M.OutDate IS NULL OR M.OutDate>=TO_DATE('" + strToDate + "','YYYY-MM-DD')) ";
                        SQL = SQL + ComNum.VBLF + " AND M.IpwonTime < TO_DATE('" + strNextDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + " AND M.Pano < '90000000' ";
                        SQL = SQL + ComNum.VBLF + " AND M.GbSTS <> '9' ";
                    }
                    else if (strJob == "2") //'당일입원자
                    {
                        SQL = SQL + ComNum.VBLF + "  AND M.InDate >= TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "  AND M.IpwonTime >= TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "  AND M.IpwonTime < TO_DATE('" + strNextDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "  AND M.Pano < '90000000' ";
                        SQL = SQL + ComNum.VBLF + "  AND M.Pano <> '81000004' ";
                        SQL = SQL + ComNum.VBLF + "  AND M.GbSTS <> '9' ";
                    }
                    else if (strJob == "3") //'퇴원예고
                    {
                        SQL = SQL + ComNum.VBLF + " AND (M.ActDate IS NULL OR M.ActDate>=TRUNC(SYSDATE)) ";
                        SQL = SQL + ComNum.VBLF + " AND M.GbSts NOT IN ('7','9') ";
                        SQL = SQL + ComNum.VBLF + " AND M.OutDate = TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + " AND (M.ROutDate IS NULL OR M.ROutDate>=TRUNC(SYSDATE) ) ";
                    }
                    else if (strJob == "4") //'당일퇴원
                    {
                        SQL = SQL + ComNum.VBLF + " AND M.OutDate=TRUNC(SYSDATE) ";
                        SQL = SQL + ComNum.VBLF + " AND M.GbSTS = '7' ";// '퇴원수납완료
                    }
                    else if (strJob == "6") //'수술예정자
                    {
                        SQL = SQL + ComNum.VBLF + " AND M.ActDate IS NULL ";
                        SQL = SQL + ComNum.VBLF + " AND M.GbSts IN ('0','2')  ";
                        SQL = SQL + ComNum.VBLF + " AND M.OutDate IS NULL ";
                    }
                    else if (strJob == "A") //'응급실경유입원 1-3일전
                    {
                        SQL = SQL + ComNum.VBLF + " AND (M.ActDate IS NULL OR M.ActDate=TRUNC(SYSDATE)) ";
                        SQL = SQL + ComNum.VBLF + " AND (M.Ilsu >= 1 AND M.Ilsu<=3) ";
                        SQL = SQL + ComNum.VBLF + " AND M.AmSet7 IN ('3','4','5') ";
                        SQL = SQL + ComNum.VBLF + " AND M.OutDate IS NULL ";
                        SQL = SQL + ComNum.VBLF + " AND M.ROutDate>=TRUNC(SYSDATE) ";
                        SQL = SQL + ComNum.VBLF + " AND M.GbSTS <> '9' ";
                    }
                    else if (strJob == "B") //'재원기간 7-14일 환자
                    {
                        SQL = SQL + ComNum.VBLF + " AND (M.ActDate IS NULL OR M.ActDate=TRUNC(SYSDATE)) ";
                        SQL = SQL + ComNum.VBLF + " AND (M.Ilsu>=7 AND M.Ilsu<=14) ";
                        SQL = SQL + ComNum.VBLF + " AND M.GbSTS <> '9' ";
                    }
                    else if (strJob == "C") //'재원기간 3-7일 환자
                    {
                        SQL = SQL + ComNum.VBLF + " AND (M.ActDate IS NULL OR M.ActDate=TRUNC(SYSDATE)) ";
                        SQL = SQL + ComNum.VBLF + " AND (M.Ilsu>=3 AND M.Ilsu<=7) ";
                        SQL = SQL + ComNum.VBLF + " AND M.GbSTS <> '9' ";
                    }

                    else if (strJob == "D") //'어제퇴원자
                    {
                        SQL = SQL + ComNum.VBLF + " AND M.OutDate=TRUNC(SYSDATE-1) ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " AND M.GbSts IN ('0','2')  ";
                    }


                    SQL = SQL + ComNum.VBLF + "  AND M.Pano=P.Pano(+) ";
                    SQL = SQL + ComNum.VBLF + "  AND M.DrCode=D.DrCode(+) ";

                    //'스탭
                    if (cboVDr.Text != "전체")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND M.DRCODE = '" + VB.Right(cboVDr.Text.Trim(), 4) + "' ";
                    }
                    //'병실
                    if (cboVRoom.Text != "전체")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND M.ROOMCODE = '" + cboVRoom.Text.Trim() + "' ";
                    }

                    if (mstrFormNo.Equals("1572"))
                    {
                        SQL = SQL + ComNum.VBLF + "  AND EXISTS ( SELECT SUB1.PTNO";
                        SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_IORDER SUB1";
                        SQL = SQL + ComNum.VBLF + "  WHERE SUB1.BDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "      AND SUB1.SUCODE = 'C3710'";
                        SQL = SQL + ComNum.VBLF + "      AND SUB1.PTNO = M.PANO)        ";
                    }


                    if (cboTeam.Text.Trim() != "전체")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND EXISTS ";
                        SQL = SQL + ComNum.VBLF + " (SELECT * FROM KOSMOS_PMPA.NUR_TEAM_ROOMCODE T";
                        SQL = SQL + ComNum.VBLF + "          WHERE M.WARDCODE = T.WARDCODE";
                        SQL = SQL + ComNum.VBLF + "             AND M.ROOMCODE = T.ROOMCODE";
                        SQL = SQL + ComNum.VBLF + "             AND T.TEAM = '" + cboTeam.Text.Trim() + "')";
                    }
                }
                else
                {
                    SQL = " SELECT '' AS WardCode, '' AS RoomCode, Pano, SName, Sex, Age, '' AS Bi,  '' AS PName,";
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(BDATE,'YYYY-MM-DD') AS InDate, 0 as Ilsu, 0 as IpdNo,  '' AS GBSTS,";
                    SQL = SQL + ComNum.VBLF + " '' AS OutDate, ";
                    SQL = SQL + ComNum.VBLF + "  DeptCode, DrCode,  '' AS DrName,  '' AS AmSet1, '' AS AmSet4,  '' AS AmSet6,  '' AS AmSet7 ";
                    SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_PMPA.OPD_MASTER M";
                    SQL = SQL + ComNum.VBLF + " WHERE DEPTCODE = 'HD'";
                    SQL = SQL + ComNum.VBLF + "      AND JIN IN ('0','1','2','3','4','5','6','7','9','C','E','F','H','M','L','K', 'N','I','J','Q','R','S','A','B')  ";
                    SQL = SQL + ComNum.VBLF + "      AND BDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "UNION ALL    ";
                    SQL = SQL + ComNum.VBLF + "SELECT B.WARDCODE AS WardCode, TO_CHAR(B.ROOMCODE) AS RoomCode, A.Pano, A.SName, A.Sex, A.Age, B.BI,  B.PNAME,";
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(B.INDATE,'YYYY-MM-DD') AS InDate, B.ILSU, IpdNo,  GBSTS,";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(B.OutDate,'YYYY-MM-DD') OUTDATE,";
                    SQL = SQL + ComNum.VBLF + "     b.DeptCode , b.DrCode, c.DrName, AmSet1, AmSet4, AmSet6, AmSet7";
                    SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.TONG_HD_DAILY A, KOSMOS_PMPA.IPD_NEW_MASTER B, KOSMOS_PMPA.BAS_DOCTOR C";
                    SQL = SQL + ComNum.VBLF + "WHERE TDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "    AND IPDOPD = 'I'";
                    SQL = SQL + ComNum.VBLF + "    AND A.PANO = B.PANO";
                    SQL = SQL + ComNum.VBLF + "    AND TRUNC(B.INDATE) <= A.TDATE";
                    SQL = SQL + ComNum.VBLF + "    AND (B.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') OR B.OUTDATE >= A.TDATE)";
                    SQL = SQL + ComNum.VBLF + "    AND B.DRCODE = C.DRCODE";
                }

                SQL = SQL + ComNum.VBLF + "   ORDER BY RoomCode, SName, Indate DESC  ";

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (dt == null)
                    return;

                if (dt.Rows.Count > 0)
                {
                    ssUserChart_Sheet1.RowCount = dt.Rows.Count;
                    ssUserChart_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < ssUserChart_Sheet1.ColumnCount; i++)
                    {
                        if (ssUserChart_Sheet1.Columns[i].Tag != null)
                        {
                            ssUserChart_Sheet1.Cells[0, i, ssUserChart_Sheet1.RowCount - 1, i].Text = ssUserChart_Sheet1.Columns[i].Tag.ToString();
                        }
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ssUserChart_Sheet1.Cells[i, mlngPtStartCol].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, mlngPtStartCol + 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, mlngPtStartCol + 2].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, mlngPtStartCol + 3].Text = dt.Rows[i]["Age"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, mlngPtStartCol + 4].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, mlngPtStartCol + 5].Text = "0";
                        ssUserChart_Sheet1.Cells[i, mlngPtStartCol + 6].Text = "0";
                        ssUserChart_Sheet1.Cells[i, mlngPtStartCol + 7].Text = dt.Rows[i]["InDate"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, mlngPtStartCol + 8].Text = "120000";
                        ssUserChart_Sheet1.Cells[i, mlngPtStartCol + 9].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, mlngPtStartCol + 10].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                    }
                }
                dt.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
            DefaultValue();
            //MakeWardChart();
        }

        /// <summary>
        /// 폼을 클리어 한다
        /// </summary>
        void ClearForm()
        {
            mstrEmrNo = "0";

            for (int i = 3; i < ssUserChart_Sheet1.RowCount; i++)
            {
                for (int j = 3; j < ssUserChart_Sheet1.ColumnCount; j++)
                {
                    ssUserChart_Sheet1.Cells[i, j].Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 서식지를 클리어 한다
        /// </summary>
        void ClearChart()
        {
            ssUserChart_Sheet1.ColumnCount = 0;
            ssUserChart_Sheet1.ColumnCount = 1;
            ssUserChart_Sheet1.Columns[0].Locked = false;
        }

        /// <summary>
        /// 생성후 기본값 설정
        /// </summary>
        void DefaultValue()
        {
            if (mstrFormNo.Equals("3150") == false)
                return;

            Cursor.Current = Cursors.WaitCursor;

            string SQL = string.Empty;
            DataTable dt = null;
            OracleDataReader reader = null;

            string strVal1 = string.Empty;
            string strVal2 = string.Empty;

            SQL = " SELECT VAL1, VAL2 FROM " + ComNum.DB_EMR + "EMR_SETUP_01 ";
            SQL = SQL + ComNum.VBLF + " WHERE BUSE = '" + clsType.User.BuseCode + "'";

            string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                return;
            }

            try
            {
                if (dt.Rows.Count > 0)
                {
                    strVal1 = dt.Rows[0]["VAL1"].ToString().Trim();
                    strVal2 = dt.Rows[0]["VAL2"].ToString().Trim();
                }

                dt.Dispose();


                List<string> strTEMP1 = new List<string>();


                for (int i = 0; i < ssUserChart_Sheet1.RowCount; i++)
                {
                    string strPano = ssUserChart_Sheet1.Cells[i, 3].Text.Trim();
                    int index = -1;

                    switch (mstrFormNo)
                    {
                        case "3150":
                            ssUserChart_Sheet1.Cells[i, intCol3].Text = "정규";
                            break;
                        case "2201":
                            //intCol = (int) VB.Val(ssItem_Sheet1.Cells[0, 2].Text);
                            break;
                    }

                    #region 쿼리

                    SQL = " SELECT RT_A, LT_A, RT_L, LT_L ";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_VITAL_REGION ";
                    SQL = SQL + ComNum.VBLF + "    WHERE PANO = '" + strPano + "'";

                    string SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                    if (SqlErr.Length > 0)
                    {
                        ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }


                    ListBox strTEMP1_T = new ListBox();
                    strTEMP1.Clear();

                    if (reader.HasRows && reader.Read())
                    {
                        if (reader.GetValue(0).ToString().Trim().Equals("0"))
                        {
                            strTEMP1.Add("Rt Arm");
                        }
                        if (reader.GetValue(1).ToString().Trim().Equals("0"))
                        {
                            strTEMP1.Add("Lt Arm");
                        }
                        if (reader.GetValue(2).ToString().Trim().Equals("0"))
                        {
                            strTEMP1.Add("Rt Leg");
                        }
                        if (reader.GetValue(3).ToString().Trim().Equals("0"))
                        {
                            strTEMP1.Add("Lt Leg");
                        }

                        strTEMP1_T.Items.Clear();
                        strTEMP1_T.Items.AddRange(strTEMP1.ToArray());

                        if (strVal1.Equals("Rt Arm") && strTEMP1.IndexOf("Rt Arm") == -1)
                        {
                        }
                        else if (strVal1.Equals("Lt Arm") && strTEMP1.IndexOf("Lt Arm") == -1)
                        {
                        }
                        else if (strVal1.Equals("Rt Leg") && strTEMP1.IndexOf("Rt Leg") == -1)
                        {
                        }
                        else if (strVal1.Equals("Lt Leg") && strTEMP1.IndexOf("Lt Leg") == -1)
                        {
                        }
                        else
                        {
                            index = strTEMP1_T.Items.IndexOf(strVal1);
                            if (strVal1.Length > 0 && index != -1)
                            {
                                strTEMP1_T.Items.RemoveAt(index);
                                strTEMP1_T.Items.Insert(0, strVal1);
                            }
                        }

                        ssUserChart_Sheet1.Cells[i, intCol].CellType = null;
                        ComboBoxCellType TypeCombo = new ComboBoxCellType();
                        TypeCombo.ListControl = strTEMP1_T;
                        TypeCombo.Editable = true;
                        ssUserChart_Sheet1.Cells[i, intCol].CellType = TypeCombo;
                        ssUserChart_Sheet1.Cells[i, intCol].Text = strTEMP1_T.Items[0].ToString();
                    }
                    else
                    {
                        if (strVal1.Length > 0)
                        {
                            ListBox strTEMP2_T = new ListBox();
                            strTEMP2_T.Items.Add("Rt Arm");
                            strTEMP2_T.Items.Add("Lt Arm");
                            strTEMP2_T.Items.Add("Rt Leg");
                            strTEMP2_T.Items.Add("Lt Leg");

                            index = strTEMP2_T.Items.IndexOf(strVal1);
                            if (index != -1)
                            {
                                strTEMP2_T.Items.RemoveAt(index);
                                strTEMP2_T.Items.Insert(0, strVal1);
                            }

                            ComboBoxCellType TypeCombo3 = new ComboBoxCellType();
                            ssUserChart_Sheet1.Cells[i, intCol].CellType = null;
                            TypeCombo3.ListControl = strTEMP2_T;
                            TypeCombo3.Editable = true;
                            ssUserChart_Sheet1.Cells[i, intCol].CellType = TypeCombo3;
                            ssUserChart_Sheet1.Cells[i, intCol].Text = strTEMP2_T.Items[0].ToString();
                        }
                    }

                    reader.Dispose();


                    //switch (mstrFormNo)
                    //{
                    //    case "1562":
                    //        intCol = (int)VB.Val(ssItem_Sheet1.Cells[i, 2].Text);
                    //        break;
                    //    case "2201":
                    //        intCol = (int)VB.Val(ssItem_Sheet1.Cells[i, 2].Text);
                    //        break;
                    //}

                    ListBox strTEMP3_T = new ListBox();
                    strTEMP3_T.Items.Clear();
                    strTEMP3_T.Items.Add("고막");
                    strTEMP3_T.Items.Add("Axilla");
                    strTEMP3_T.Items.Add("Oral");
                    strTEMP3_T.Items.Add("Rectal");

                    index = strTEMP3_T.Items.IndexOf(strVal2);
                    if (strVal2.Length > 0 && index != -1)
                    {
                        strTEMP3_T.Items.RemoveAt(index);
                        strTEMP3_T.Items.Insert(0, strVal2);
                    }

                    ComboBoxCellType TypeCombo4 = new ComboBoxCellType();
                    ssUserChart_Sheet1.Cells[i, intCol2].CellType = null;
                    TypeCombo4.ListControl = strTEMP3_T;
                    TypeCombo4.Editable = true;
                    ssUserChart_Sheet1.Cells[i, intCol2].CellType = TypeCombo4;
                    ssUserChart_Sheet1.Cells[i, intCol2].Text = strTEMP3_T.Items[0].ToString();
                    #endregion
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                return;
            }
        }

        /// <summary>
        /// Print Data
        /// </summary>
        /// <param name="PrintType"></param>
        private void PreViewAndPrint(string PrintType)
        {
            btnPrint.Enabled = false;

            SmartPrintRulesCollection prules = new SmartPrintRulesCollection();

            //'Print Head 지정
            string strFont1 = @"/fn""바탕체"" /fz""14"" /fb1 /fi0 /fu0 /fk0 /fs1";
            string strFont2 = @"/fn""바탕체"" /fz""12"" /fb0 /fi0 /fu0 /fk0 /fs2";
            string strHead1 = "/c/f1" + mstrFORMNAME + "/f1/n/n";
            string strHead2 = "/n/l/f2" + "차트일자 : " + dtpChartDate.Value.ToString("yyyy년 mm월 dd일") + " /r/f2" + "출력자 : " + clsType.User.UserName + "     /n";

            ssUserChart_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssUserChart_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssUserChart_Sheet1.PrintInfo.HeaderHeight = 100;
            ssUserChart_Sheet1.PrintInfo.Margin.Left = 10;
            ssUserChart_Sheet1.PrintInfo.Margin.Right = 10;
            ssUserChart_Sheet1.PrintInfo.Margin.Top = 20;
            ssUserChart_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssUserChart_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssUserChart_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssUserChart_Sheet1.PrintInfo.ShowBorder = true;
            ssUserChart_Sheet1.PrintInfo.ShowColor = false;
            ssUserChart_Sheet1.PrintInfo.ShowGrid = true;
            ssUserChart_Sheet1.PrintInfo.ShowShadows = false;

            ssUserChart_Sheet1.PrintInfo.UseMax = false;
            //ssUserChart_Sheet1.PrintInfo.BestFitCols = true;
            //ssUserChart_Sheet1.PrintInfo.BestFitRows = true;

            //ssUserChart_Sheet1.PrintInfo.SmartPrintPagesTall = 1;
            //ssUserChart_Sheet1.PrintInfo.SmartPrintPagesWide = 1;

            //prules.Add(new BestFitColumnRule(ResetOption.None));
            //prules.Add(new LandscapeRule(ResetOption.None));
            //prules.Add(new ScaleRule(ResetOption.None, 1, (float)0.1, (float)0.1));

            //ssUserChart_Sheet1.PrintInfo.SmartPrintRules = prules;
            //ssUserChart_Sheet1.PrintInfo.UseSmartPrint = false;

            ssUserChart_Sheet1.PrintInfo.Centering = Centering.Horizontal;
            ssUserChart_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            if (mstrFormNo == "1572")
            {
                ssUserChart_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            }
            ssUserChart_Sheet1.PrintInfo.Preview = PrintType.Equals("V");
            ssUserChart.PrintSheet(0);

            Application.DoEvents();

            btnPrint.Enabled = true;
        }

        /// <summary>
        /// 서식지를 만든다
        /// </summary>
        /// <param name="FormNo"></param>
        private void MakeFowChart()
        {
            
            ssUserChart_Sheet1.ColumnCount = mlngStartCol;

            Cursor.Current = Cursors.WaitCursor;

            string SQL = string.Empty;
            DataTable dt = null;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "    A.ITEMNO, A.ITEMNAME, A.CELLTYPE, A.HALIGN, A.VALIGN, A.SIZEWIDTH, A.SIZEHEIGHT, ";
            SQL = SQL + ComNum.VBLF + "    A.MULTILINE, A.CHECKALIGN, A.USERMCRO, A.USERFUNC, A.USERFUNCNM ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRFLOWXML A ";
            if (mstrFormNo == "3150")
            {
                SQL = SQL + ComNum.VBLF + "WHERE A.FORMNO = 3510";
            }
            else if (mstrFormNo == "2201")
            {
                SQL = SQL + ComNum.VBLF + "WHERE A.FORMNO = 3517";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "WHERE A.FORMNO = " + mstrFormNo;
            }
            SQL = SQL + ComNum.VBLF + "    AND A.UPDATENO = (SELECT  ";
            SQL = SQL + ComNum.VBLF + "                            MAX(A1.UPDATENO) ";
            SQL = SQL + ComNum.VBLF + "                        FROM " + ComNum.DB_EMR + "AEMRFLOWXML A1 ";
            SQL = SQL + ComNum.VBLF + "                        WHERE A1.FORMNO = A.FORMNO) ";
            SQL = SQL + ComNum.VBLF + "ORDER BY A.ITEMNUMBER ";

            string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                return;
            }

            int TotItemCnt = dt.Rows.Count;
            string pOption = "W";

            ssUserChart_Sheet1.ColumnCount = mlngStartCol + TotItemCnt;
            ssFORMXML_Sheet1.ColumnCount = mlngStartCol + TotItemCnt;
            ssFORMXML_Sheet1.RowCount = 2;
            ssItem_Sheet1.ColumnCount = mlngStartCol + TotItemCnt;

            FarPoint.Win.ComplexBorder complexBorder2 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            ssUserChart_Sheet1.DefaultStyle.Border = complexBorder2;
            ssUserChart_Sheet1.SheetCornerStyle.Border = complexBorder2;
            ssUserChart_Sheet1.ColumnHeader.DefaultStyle.Border = complexBorder2;
            ssUserChart_Sheet1.RowHeader.DefaultStyle.Border = complexBorder2;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ssUserChart_Sheet1.Columns[mlngStartCol + i].Label = dt.Rows[i]["ITEMNAME"].ToString().Trim();
                ssUserChart_Sheet1.Columns[mlngStartCol + i].Width = (int)VB.Val(dt.Rows[i]["SIZEWIDTH"].ToString().Trim());

                if (dt.Rows[i]["CELLTYPE"].ToString().Trim() == "ComboBoxCellType")
                {
                    if (dt.Rows[i]["USERMCRO"].ToString().Trim() != "")
                    {
                        ComboBoxCellType TypeCombo = new ComboBoxCellType();
                        ListBox listBox = new ListBox();

                        listBox.Items.AddRange(dt.Rows[i]["USERMCRO"].ToString().Trim().Split('^'));
                        TypeCombo.ListControl = listBox;
                        TypeCombo.Editable = true;
                        ssUserChart_Sheet1.Columns[mlngStartCol + i].CellType = TypeCombo;
                    }
                }
                else
                {
                    ssUserChart_Sheet1.Columns[mlngStartCol + i].CellType = DesignFunc.CellType(dt.Rows[i]["CELLTYPE"].ToString().Trim(), dt.Rows[i]["MULTILINE"].ToString().Trim(), dt.Rows[i]["CHECKALIGN"].ToString().Trim(), dt.Rows[i]["USERMCRO"].ToString().Trim(), pOption);
                }

                ssUserChart_Sheet1.Columns[mlngStartCol + i].HorizontalAlignment = DesignFunc.HorizontalAlignment(dt.Rows[i]["HALIGN"].ToString().Trim());
                ssUserChart_Sheet1.Columns[mlngStartCol + i].VerticalAlignment = DesignFunc.VerticalAlignment(dt.Rows[i]["VALIGN"].ToString().Trim());
                ssUserChart_Sheet1.Columns[mlngStartCol + i].Locked = false;

                if (dt.Rows[i]["ITEMNO"].ToString().Trim() == "I0000035481")
                {
                    ssUserChart_Sheet1.Columns[mlngStartCol + i].Tag = "Unit";
                }
                else if (dt.Rows[i]["ITEMNO"].ToString().Trim() == "I0000035482")
                {
                    ssUserChart_Sheet1.Columns[mlngStartCol + i].Tag = "SC";
                }


                //구분
                if (dt.Rows[i]["ITEMNO"].ToString().Trim() == "I0000024733")
                {
                    intCol3 = mlngStartCol + i;
                }
                //혈압측정위치
                if (dt.Rows[i]["ITEMNO"].ToString().Trim() == "I0000037575")
                {
                    intCol = mlngStartCol + i;
                }

                //체온 측정위치
                if (dt.Rows[i]["ITEMNO"].ToString().Trim() == "I0000035464")
                {
                    intCol2 = mlngStartCol + i;
                }

                ssItem_Sheet1.Columns[mlngStartCol + i].Label = dt.Rows[i]["ITEMNAME"].ToString().Trim();
                ssItem_Sheet1.Cells[0, mlngStartCol + i].Text = dt.Rows[i]["ITEMNO"].ToString().Trim();
                ssItem_Sheet1.Cells[1, mlngStartCol + i].Text = dt.Rows[i]["CELLTYPE"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;

            //환자 조회를 한 상태에서 기록지를 선택한 경우
            if (ssUserChart_Sheet1.Rows.Count > 0)
            {
                for (int i = 0; i < ssUserChart_Sheet1.ColumnCount; i++)
                {
                    if (ssUserChart_Sheet1.Columns[i].Tag != null)
                    {
                        ssUserChart_Sheet1.Cells[0, i, ssUserChart_Sheet1.RowCount - 1, i].Text = ssUserChart_Sheet1.Columns[i].Tag.ToString();
                    }
                }
                DefaultValue();
            }
        }

        /// <summary>
        /// 임상관찰기록지 서식지를 만든다
        /// 이기록지는 가짜로 만들어서 사용하도록 한다 : 하드 코딩 안할려구
        /// </summary>
        private void MakeFowChart_3150()
        {
            //#region //Clucose
            //intCol = 0;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].Label = "Clucose";
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].Width = 100;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].Locked = false;

            //TypeText.Multiline = false;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].CellType = TypeText;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

            //ssItem_Sheet1.Columns[mlngStartCol + intCol].Label = "Clucose";
            //ssItem_Sheet1.Cells[0, mlngStartCol + intCol].Text = "I0000009122";
            //#endregion //Clucose

            //#region //주사 부위
            //intCol = intCol + 1;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].Label = "주사 부위";
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].Width = 41;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].Locked = false;

            //TypeText.Multiline = false;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].CellType = TypeText;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

            //ssItem_Sheet1.Columns[mlngStartCol + intCol].Label = "주사 부위";
            //ssItem_Sheet1.Cells[0, mlngStartCol + intCol].Text = "I0000035479";
            //#endregion //주사 부위

            //#region //Insulin
            //intCol = intCol + 1;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].Label = "Insulin";
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].Width = 100;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].Locked = false;

            //TypeCombo = new ComboBoxCellType();
            //listBox = new ListBox();
            //listBox.Items.AddRange("SC^IV".Split('^'));
            //TypeCombo.ListControl = listBox;
            //TypeCombo.Editable = true;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].CellType = TypeCombo;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

            //ssItem_Sheet1.Columns[mlngStartCol + intCol].Label = "Insulin";
            //ssItem_Sheet1.Cells[0, mlngStartCol + intCol].Text = "I0000004686";
            //#endregion //Insulin

            //#region //용량
            //intCol = intCol + 1;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].Label = "용량";
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].Width = 80;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].Locked = false;

            //TypeText.Multiline = false;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].CellType = TypeText;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

            //ssItem_Sheet1.Columns[mlngStartCol + intCol].Label = "용량";
            //ssItem_Sheet1.Cells[0, mlngStartCol + intCol].Text = "I0000035480";
            //#endregion //용량

            //#region //Unit
            //intCol = intCol + 1;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].Label = "Unit";
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].Width = 40;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].Locked = false;

            //TypeText.Multiline = false;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].CellType = TypeText;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

            //ssItem_Sheet1.Columns[mlngStartCol + intCol].Label = "Unit";
            //ssItem_Sheet1.Cells[0, mlngStartCol + intCol].Text = "I0000035481";
            //#endregion //Unit

            //#region //주사 방법
            //intCol = intCol + 1;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].Label = "주사 방법";
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].Width = 40;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].Locked = false;

            //TypeCombo = new ComboBoxCellType();
            //listBox = new ListBox();
            //listBox.Items.AddRange("SC^IV".Split('^'));
            //TypeCombo.ListControl = listBox;
            //TypeCombo.Editable = true;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].CellType = TypeCombo;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

            //ssItem_Sheet1.Columns[mlngStartCol + intCol].Label = "주사 방법";
            //ssItem_Sheet1.Cells[0, mlngStartCol + intCol].Text = "I0000035479";
            //#endregion //주사 방법

            //#region //비고
            //intCol = intCol + 1;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].Label = "비고";
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].Width = 140;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].Locked = false;

            //TypeText.Multiline = false;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].CellType = TypeText;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            //ssUserChart_Sheet1.Columns[mlngStartCol + intCol].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

            //ssItem_Sheet1.Columns[mlngStartCol + intCol].Label = "비고";
            //ssItem_Sheet1.Cells[0, mlngStartCol + intCol].Text = "I0000001311";
            //#endregion //비고
        }

        /// <summary>
        /// 조회 함수
        /// </summary>
        void GetUchartHis(string strPtno, string strFormNo, FarPoint.Win.Spread.FpSpread objSpread, int lngRow)
        {
            if (mstrFormNo == "")
                return;

            //if (pAcp.ptNo == "")
            //    return;

            string strFormNoFlw = strFormNo;

            if (strFormNo == "2201" || strFormNo == "3150")
            {
                strFormNoFlw = "3510";
            }

            string SQL = string.Empty;
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "    C.ACPNO, C.CHARTDATE, C.CHARTTIME, ";
                SQL = SQL + ComNum.VBLF + "    R.ITEMCD, R.ITEMVALUE || R.ITEMVALUE1 AS ITEMVALUE";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST C  ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRFORM F  ";
                SQL = SQL + ComNum.VBLF + "    ON C.FORMNO = F.FORMNO ";
                SQL = SQL + ComNum.VBLF + "    AND F.UPDATENO = (SELECT MAX(X.UPDATENO) FROM " + ComNum.DB_EMR + "AEMRFORM X ";
                SQL = SQL + ComNum.VBLF + "                      WHERE X.FORMNO = F.FORMNO) ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R ";
                SQL = SQL + ComNum.VBLF + "    ON C.EMRNO = R.EMRNO ";
                SQL = SQL + ComNum.VBLF + "    AND R.ITEMCD IN ( SELECT FL.ITEMNO FROM " + ComNum.DB_EMR + "AEMRFLOWXML FL ";
                SQL = SQL + ComNum.VBLF + "                      WHERE FL.FORMNO = " + strFormNoFlw;
                SQL = SQL + ComNum.VBLF + "                          AND FL.UPDATENO = (SELECT   ";
                SQL = SQL + ComNum.VBLF + "                                                    MAX(FL1.UPDATENO)  ";
                SQL = SQL + ComNum.VBLF + "                                                FROM " + ComNum.DB_EMR + "AEMRFLOWXML FL1  ";
                SQL = SQL + ComNum.VBLF + "                                                WHERE FL1.FORMNO = FL.FORMNO)  )";
                SQL = SQL + ComNum.VBLF + "WHERE C.PTNO = '" + strPtno + "' ";
                SQL = SQL + ComNum.VBLF + "    AND C.FORMNO = " + strFormNo ;
                SQL = SQL + ComNum.VBLF + "    AND C.EMRNO = (SELECT MAX(R1.EMRNO) FROM " + ComNum.DB_EMR + "AEMRCHARTROW R1 ";
                SQL = SQL + ComNum.VBLF + "                        WHERE R1.EMRNO = C.EMRNO ";
                SQL = SQL + ComNum.VBLF + "                        AND R1.ITEMVALUE IS NOT NULL ";
                SQL = SQL + ComNum.VBLF + "                        AND R1.ITEMCD IN ( SELECT FL.ITEMNO FROM " + ComNum.DB_EMR + "AEMRFLOWXML FL ";
                SQL = SQL + ComNum.VBLF + "                                          WHERE FL.FORMNO = " + strFormNoFlw;
                SQL = SQL + ComNum.VBLF + "                                              AND FL.UPDATENO = (SELECT ";
                SQL = SQL + ComNum.VBLF + "                                                                        MAX(FL1.UPDATENO) ";
                SQL = SQL + ComNum.VBLF + "                                                                    FROM " + ComNum.DB_EMR + "AEMRFLOWXML FL1  ";
                SQL = SQL + ComNum.VBLF + "                                                                    WHERE FL1.FORMNO = FL.FORMNO) ) ) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY R.DSPSEQ ";

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = mlngStartCol; j < ssUserChart_Sheet1.ColumnCount ; j++)
                    {
                        //ssItem_Sheet1.Cells[0, mlngStartCol + i].Text
                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == ssItem_Sheet1.Cells[0, j].Text.Trim())
                        {
                            ssUserChart_Sheet1.Cells[lngRow, j].Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                            break;
                        }
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        /// <summary>
        /// Data 저장
        /// </summary>
        /// <returns></returns>
        private bool SaveData()
        {
            bool rtnVal = false;

            if (txtMedFrTime.Text.Replace(":", "").Trim() == "")
            {
                ComFunc.MsgBoxEx(this, "사간 형식이 맞지 않습니다");
                txtMedFrTime.Focus();
                return rtnVal;
            }

            if (VB.IsDate("2020-01-01 " + ComFunc.FormatStrToDate(txtMedFrTime.Text.Replace(":", "").Trim(), "M")) == false)
            {
                ComFunc.MsgBoxEx(this, "사간 형식이 맞지 않습니다");
                txtMedFrTime.Focus();
                return rtnVal;
            }

            if (mstrFormNo == "2201" || mstrFormNo == "3150")
            {
                rtnVal = SaveDataVital();
            }
            else if (mstrFormNo == "1572")
            {
                rtnVal = SaveDataNomal();
            }
            return rtnVal;
        }

        /// <summary>
        /// 임상관찰을 제외한 일괄작업
        /// </summary>
        /// <returns></returns>
        private bool SaveDataNomal()
        {
            bool rtnVal = false;

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            if (string.IsNullOrWhiteSpace(txtMedFrTime.Text))
            {
                ComFunc.MsgBoxEx(this, "시간을 입력해 주십시오");
                return rtnVal;
            }

            string strChartDate = dtpChartDate.Value.ToString("yyyyMMdd");
            string strChartTime = txtMedFrTime.Text.Trim().Replace(":", "").Trim();
            string strInOutCls = "I";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            string strCurDate = VB.Left(strCurDateTime, 8);
            string strCurTime = VB.Right(strCurDateTime, 6);

            string pFLOWGB = "";
            int intFLOWITEMCNT = 0;
            int intFLOWHEADCNT = 0;
            int intFLOWINPUTSIZE = 0;
            FormFlowSheet[] pFormFlowSheet = null;
            FormFlowSheetHead[,] pFormFlowSheetHead = null;
            FormDesignQuery.GetSetDate_AEMRFLOWXML(mstrFormNo, mstrUpdateNo, ref pFLOWGB, ref intFLOWITEMCNT, ref intFLOWHEADCNT, ref intFLOWINPUTSIZE, ref pFormFlowSheet, ref pFormFlowSheetHead);

            try
            {
                for (int lngRow = 0; lngRow < ssUserChart_Sheet1.RowCount; lngRow++)
                {
                    if (ssUserChart_Sheet1.Cells[lngRow, 0].Text.Trim().Equals("True") == false)
                    {
                        continue;
                    }

                    string strPtNo = ssUserChart_Sheet1.Cells[lngRow, 3].Text.Trim();
                    string strAcpNo = ssUserChart_Sheet1.Cells[lngRow, 7].Text.Trim();
                    string strMedFrDate = ssUserChart_Sheet1.Cells[lngRow, 9].Text.Trim().Replace("-", "");
                    string strMedFrTime = ssUserChart_Sheet1.Cells[lngRow, 10].Text.Trim();
                    string strMedDeptCd = ssUserChart_Sheet1.Cells[lngRow, 11].Text.Trim();
                    string strMedDrCd = ssUserChart_Sheet1.Cells[lngRow, 12].Text.Trim();
                    strInOutCls = strMedDeptCd.Equals("HD") ? "O" : "I";

                    EmrPatient AcpEmr = clsEmrChart.ClearPatient();
                    AcpEmr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, strPtNo, strInOutCls, strMedFrDate, strMedDeptCd);
                    if (AcpEmr == null)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("접수내역을 찾을 수 없습니다.");
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    double dblEmrNo = 0;

                    SQL = " ";
                    SQL = SQL + ComNum.VBLF + "SELECT ";
                    SQL = SQL + ComNum.VBLF + "    EMRNO ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST ";
                    SQL = SQL + ComNum.VBLF + "WHERE PTNO = '" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND CHARTDATE = '" + strChartDate + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND SUBSTR(CHARTTIME, 1, 4) = '" + strChartTime + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND FORMNO = " + mstrFormNo;

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        dblEmrNo = VB.Val(dt.Rows[0]["EMRNO"].ToString().Trim());
                    }
                    dt.Dispose();
                    dt = null;

                    if (dblEmrNo > 0)
                    {
                        //TODO
                        //기존 데이타가 있으면 처리하는 루틴이 필요함
                        continue;
                    }

                    double dblEmrNoNew = 0;

                    dblEmrNoNew = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "GETEMRXMLNO")); //임상관찰의 경우는
                    double dblEmrHisNo = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "EMRXMLHISNO"));

                    #region SaveRutine

                    #region //저장 CHRATMAST
                    string strSAVEGB = "1";
                    string strSAVECERT = "1";
                    string strSaveFlag = "1"; //인증저장
                    string strSaveId = clsType.User.IdNumber;
                    string strFORMGB = "0";

                    if (clsEmrQuery.SaveChartMstOnly(clsDB.DbCon, dblEmrNoNew, dblEmrHisNo, AcpEmr, mstrFormNo, mstrUpdateNo,
                                        strChartDate, strChartTime,
                                        strSaveId, strSaveId, strSAVEGB, strSAVECERT, strFORMGB,
                                        strCurDate, strCurTime, strSaveFlag) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("AEMRCHARTMST저장 중 에러가 발생했습니다.");
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    #endregion //저장 CHRATMAST

                    #region //저장 CHRATROW

                    string[] arryEMRNO = null;
                    string[] arryITEMCD = null;
                    string[] arryITEMNO = null;
                    string[] arryITEMINDEX = null;
                    string[] arryITEMTYPE = null;
                    string[] arryITEMVALUE = null;
                    string[] arryITEMVALUE1 = null;
                    string[] arryDSPSEQ = null;

                    int i = 0;

                    for (int lngCol2 = mlngStartCol; lngCol2 < ssUserChart_Sheet1.ColumnCount; lngCol2++)
                    {
                        if (lngCol2 == ssUserChart_Sheet1.ColumnCount)
                            break;

                        string strITEMCD = ssItem_Sheet1.Cells[0, lngCol2].Text.Trim();
                        string strITEMNO = "";
                        string strITEMINDEX = "";
                        string strITEMTYPE = "";
                        string strITEMVALUE = "";
                        string strITEMVALUE1 = "";
                        string strDSPSEQ = i.ToString();

                        if (VB.InStr(strITEMCD, "_") > 0)
                        {
                            strITEMNO = VB.Split(strITEMCD, "_")[0];
                            strITEMINDEX = VB.Split(strITEMCD, "_")[1];
                        }
                        else
                        {
                            strITEMNO = strITEMCD;
                            strITEMINDEX = "0";
                        }
                        strITEMTYPE = ssItem_Sheet1.Cells[1, lngCol2].Text.Trim(); //pFormFlowSheet[i].CellType;
                        strITEMVALUE = ssUserChart_Sheet1.Cells[lngRow, lngCol2].Text.Trim();

                        if (arryEMRNO == null)
                        {
                            arryEMRNO = new string[0];
                            arryITEMCD = new string[0];
                            arryITEMNO = new string[0];
                            arryITEMINDEX = new string[0];
                            arryITEMTYPE = new string[0];
                            arryITEMVALUE = new string[0];
                            arryITEMVALUE1 = new string[0];
                            arryDSPSEQ = new string[0];
                        }

                        Array.Resize<string>(ref arryEMRNO, arryEMRNO.Length + 1);
                        Array.Resize<string>(ref arryITEMCD, arryITEMCD.Length + 1);
                        Array.Resize<string>(ref arryITEMNO, arryITEMNO.Length + 1);
                        Array.Resize<string>(ref arryITEMINDEX, arryITEMINDEX.Length + 1);
                        Array.Resize<string>(ref arryITEMTYPE, arryITEMTYPE.Length + 1);
                        Array.Resize<string>(ref arryITEMVALUE, arryITEMVALUE.Length + 1);
                        Array.Resize<string>(ref arryITEMVALUE1, arryITEMVALUE1.Length + 1);
                        Array.Resize<string>(ref arryDSPSEQ, arryDSPSEQ.Length + 1);

                        arryEMRNO[arryEMRNO.Length - 1] = dblEmrNoNew.ToString();
                        arryITEMCD[arryEMRNO.Length - 1] = strITEMCD;
                        arryITEMNO[arryEMRNO.Length - 1] = strITEMNO;
                        arryITEMINDEX[arryEMRNO.Length - 1] = strITEMINDEX;
                        arryITEMTYPE[arryEMRNO.Length - 1] = strITEMTYPE;
                        arryITEMVALUE[arryEMRNO.Length - 1] = strITEMVALUE;
                        arryITEMVALUE1[arryEMRNO.Length - 1] = strITEMVALUE1;
                        arryDSPSEQ[arryEMRNO.Length - 1] = strDSPSEQ;

                        i = i + 1;
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTROW ";
                    SQL = SQL + ComNum.VBLF + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUSEID, INPDATE, INPTIME )";
                    SQL = SQL + ComNum.VBLF + "VALUES (";
                    SQL = SQL + ComNum.VBLF +  dblEmrNoNew.ToString() + ",";    //EMRNO
                    SQL = SQL + ComNum.VBLF +  dblEmrHisNo.ToString() + ",";    //EMRNOHIS
                    SQL = SQL + ComNum.VBLF + " :ITEMCD,";   //ITEMCD
                    SQL = SQL + ComNum.VBLF + " :ITEMNO,"; //ITEMNO
                    SQL = SQL + ComNum.VBLF + " :ITEMINDEX,"; //ITEMINDEX
                    SQL = SQL + ComNum.VBLF + " :ITEMTYPE,";   //ITEMTYPE
                    SQL = SQL + ComNum.VBLF + " :ITEMVALUE,";   //ITEMVALUE
                    SQL = SQL + ComNum.VBLF + " :DSPSEQ,";   //DSPSEQ
                    SQL = SQL + ComNum.VBLF + " :ITEMVALUE1,";   //ITEMVALUE
                    SQL = SQL + ComNum.VBLF + "'" + clsType.User.IdNumber + "',";   //INPUSEID
                    SQL = SQL + ComNum.VBLF + "'" + strCurDate + "', ";   //INPDATE
                    SQL = SQL + ComNum.VBLF + "'" + strCurTime + "' ";   //INPTIME
                    SQL = SQL + ComNum.VBLF + ")";

                    SqlErr = clsDB.ExecuteChartRow(clsDB.DbCon, SQL, dblEmrNoNew, dblEmrHisNo, arryITEMCD, arryITEMNO, arryITEMINDEX, arryITEMTYPE, arryITEMVALUE, arryDSPSEQ, arryITEMVALUE1);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    #endregion //저장 CHRATROW

                    clsEmrQuery.SaveEmrCert(clsDB.DbCon, dblEmrNoNew, false);

                    #endregion

                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                rtnVal = true;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 임상관찰 기록을 저장한다
        /// </summary>
        /// <returns></returns>
        private bool SaveDataVital()
        {
            bool rtnVal = false;

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (string.IsNullOrWhiteSpace(txtMedFrTime.Text))
            {
                ComFunc.MsgBoxEx(this, "시간을 입력해 주십시오");
                return rtnVal;
            }

            string strChartDate = dtpChartDate.Value.ToString("yyyyMMdd");
            string strChartTime = txtMedFrTime.Text.Trim().Replace(":", "").Trim();
            strChartTime = strChartTime + "00";

            string strInOutCls = "I";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            string strCurDate = VB.Left(strCurDateTime, 8);
            string strCurTime = VB.Right(strCurDateTime, 6);

            try
            {
                for (int lngRow = 0; lngRow < ssUserChart_Sheet1.RowCount; lngRow++)
                {
                    if (ssUserChart_Sheet1.Cells[lngRow, 0].Text.Trim().Equals("True") == false)
                    {
                        continue;
                    }

                    string strPtNo = ssUserChart_Sheet1.Cells[lngRow, 3].Text.Trim();
                    string strAcpNo = ssUserChart_Sheet1.Cells[lngRow, 7].Text.Trim();
                    string strMedFrDate = ssUserChart_Sheet1.Cells[lngRow, 9].Text.Trim().Replace("-", "");
                    string strMedFrTime = ssUserChart_Sheet1.Cells[lngRow, 10].Text.Trim();
                    string strMedDeptCd = ssUserChart_Sheet1.Cells[lngRow, 11].Text.Trim();
                    string strMedDrCd = ssUserChart_Sheet1.Cells[lngRow, 12].Text.Trim();
                    strInOutCls = strMedDeptCd.Equals("HD") ? "O" : "I";

                    EmrPatient AcpEmr = clsEmrChart.ClearPatient();
                    AcpEmr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, strPtNo, strInOutCls, strMedFrDate, strMedDeptCd);
                    if (AcpEmr == null)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("접수내역을 찾을 수 없습니다.");
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    #region //작성된 데이타가 있는지 확인한다

                    int intCahrtCnt = 0;
                    string strChartItems = "";
                    for (int lngCol2 = mlngStartCol; lngCol2 < ssUserChart_Sheet1.ColumnCount; lngCol2++)
                    {
                        if (lngCol2 == ssUserChart_Sheet1.ColumnCount)
                            break;

                        string strITEMCD = ssItem_Sheet1.Cells[0, lngCol2].Text.Trim();
                        string strITEMVALUE = ssUserChart_Sheet1.Cells[lngRow, lngCol2].Text.Trim();

                        if (strITEMCD != "I0000024733" && strITEMCD != "I0000037575" && strITEMCD != "I0000035464")
                        {
                            if (strITEMVALUE != "")
                            {
                                intCahrtCnt = intCahrtCnt + 1;
                                strChartItems = strChartItems + "'" + strITEMCD + "',";
                            }
                        }
                    }

                    if (intCahrtCnt == 0) continue;

                    if (mstrFormNo != "2201")
                    {
                        strChartItems = strChartItems + "'I0000024733', 'I0000037575', 'I0000035464'";
                    }
                    else
                    {
                        strChartItems = VB.Left(strChartItems, strChartItems.Length - 1);
                    }
                    #endregion //작성된 데이타가 있는지 확인한다

                    #region 기존 데이타가 있으면 처리하는 루틴이 필요함

                    double dblEmrNo = 0;
                    SQL = " ";
                    SQL = SQL + ComNum.VBLF + "SELECT ";
                    SQL = SQL + ComNum.VBLF + "    C.EMRNO ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST C";
                    SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R";
                    SQL = SQL + ComNum.VBLF + "    ON C.EMRNO = R.EMRNO";
                    SQL = SQL + ComNum.VBLF + "    AND C.EMRNOHIS = R.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "    AND R.ITEMCD IN (" + strChartItems + ") ";
                    SQL = SQL + ComNum.VBLF + "    AND R.ITEMVALUE IS NOT NULL ";
                    SQL = SQL + ComNum.VBLF + "    AND R.INPUSEID IS NOT NULL ";
                    SQL = SQL + ComNum.VBLF + "    AND R.INPUSEID <> '" + clsType.User.IdNumber + "'";
                    SQL = SQL + ComNum.VBLF + "WHERE C.PTNO = '" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND C.CHARTDATE = '" + strChartDate + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND SUBSTR(C.CHARTTIME, 1, 4) = '" + VB.Left(strChartTime, 4) + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND C.FORMNO = " + mstrFormNo;

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        dblEmrNo = VB.Val(dt.Rows[0]["EMRNO"].ToString().Trim());
                    }
                    dt.Dispose();
                    dt = null;

                    if (dblEmrNo > 0)
                    {
                        //clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, AcpEmr.ptName + "[" + AcpEmr.ptNo + "] 님의" + ComNum.VBLF + "해당 시간의 차트가 저장되어 있습니다");
                        //Cursor.Current = Cursors.Default;
                        //return rtnVal;
                        continue;
                    }

                    #endregion 기존 데이타가 있으면 처리하는 루틴이 필요함

                    #region 재원환자의 경우 당일 기본 시간을 만들어 줘야 한다
                    if (clsEmrQueryEtc.SetSaveDefaultVitalTime(clsDB.DbCon, AcpEmr.acpNo, cboWard.Text.Trim(), strChartDate, mstrFormNo) == false)
                    {
                        return false;
                    }

                    if (strMedDeptCd == "HD" || mstrFormNo == "2201")
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT 1";
                        SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRBVITALTIME";
                        SQL = SQL + ComNum.VBLF + "  WHERE FORMNO = " + mstrFormNo;
                        SQL = SQL + ComNum.VBLF + "   AND ACPNO = " + AcpEmr.acpNo;
                        SQL = SQL + ComNum.VBLF + "   AND CHARTDATE = '" + strChartDate + "'";
                        SQL = SQL + ComNum.VBLF + "   AND TIMEVALUE = '" + VB.Left(strChartTime, 4) + "'";
                        SQL = SQL + ComNum.VBLF + "   AND JOBGB = '" + "IVT" + "'";

                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return false;
                        }

                        if (dt.Rows.Count == 0)
                        {
                            SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRBVITALTIME ";
                            SQL = SQL + ComNum.VBLF + "(FORMNO, ACPNO, JOBGB , CHARTDATE, TIMEVALUE, SUBGB, WRITEDATE, WRITETIME, WRITEUSEID)";
                            SQL = SQL + ComNum.VBLF + "VALUES (";
                            SQL = SQL + ComNum.VBLF + "" + mstrFormNo + ",";  //FORMNO
                            SQL = SQL + ComNum.VBLF + "" + AcpEmr.acpNo + ",";  //ACPNO
                            SQL = SQL + ComNum.VBLF + "'" + "IVT" + "',";    //JOBGB
                            SQL = SQL + ComNum.VBLF + "'" + strChartDate + "',";  //CHARTDATE
                            SQL = SQL + ComNum.VBLF + "'" + VB.Left(strChartTime, 4) + "',"; //TIMEVALUE
                            SQL = SQL + ComNum.VBLF + "'0',";   //SUBGB
                            SQL = SQL + ComNum.VBLF + "'" + strCurDate + "',";  //WRITEDATE
                            SQL = SQL + ComNum.VBLF + "'" + strCurTime + "',";  //WRITETIME
                            SQL = SQL + ComNum.VBLF + "'" + clsType.User.IdNumber + "'";    //WRITEUSEID
                            SQL = SQL + ComNum.VBLF + ")";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return false;
                            }
                        }
                        dt.Dispose();
                        dt = null;
                    }
                    #endregion

                    #region //해당일자에 아이템이 있는지 확인한다
                    if (strMedDeptCd == "HD" || mstrFormNo == "2201")
                    {
                        #region 작성일에 아이템 여부
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT ";
                        SQL = SQL + ComNum.VBLF + "    A.ITEMCD";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET A";
                        SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + AcpEmr.acpNo;
                        SQL = SQL + ComNum.VBLF + "    AND A.FORMNO = " + mstrFormNo;
                        SQL = SQL + ComNum.VBLF + "    AND A.JOBGB IN ('IVT', 'IIO', 'IST', 'IBN')";
                        SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = '" + strChartDate + "'";

                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return false;
                        }

                        if (dt.Rows.Count == 0)
                        {
                            SQL = "";
                            SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRBVITALSET ";
                            SQL = SQL + ComNum.VBLF + "(FORMNO, ACPNO, PTNO, CHARTDATE, JOBGB, ITEMCD, WRITEDATE, WRITETIME, WRITEUSEID)";
                            SQL = SQL + ComNum.VBLF + "SELECT " ;
                            SQL = SQL + ComNum.VBLF + "    " + mstrFormNo + ","; //FORMNO
                            SQL = SQL + ComNum.VBLF + "    " + AcpEmr.acpNo + ","; //ACPNO
                            SQL = SQL + ComNum.VBLF + "    '" + AcpEmr.ptNo + "',"; //PTNO
                            SQL = SQL + ComNum.VBLF + "    '" + strChartDate + "',"; //CHARTDATE
                            SQL = SQL + ComNum.VBLF + "    '" + "IVT" + "',"; //JOBGB
                            SQL = SQL + ComNum.VBLF + "    ITEMNO,"; //ITEMCD
                            SQL = SQL + ComNum.VBLF + "    '" + strCurDate + "',"; //WRITEDATE
                            SQL = SQL + ComNum.VBLF + "    '" + strCurTime + "',"; //WRITETIME
                            SQL = SQL + ComNum.VBLF + "    '" + clsType.User.IdNumber + "'"; //WRITEUSEID
                            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRFLOWXML";
                            SQL = SQL + ComNum.VBLF + " WHERE FORMNO = 3517";
                            SQL = SQL + ComNum.VBLF + "   AND ITEMNUMBER < 6";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("아이템 저장중 오류가 발생하였습니다.");
                                return rtnVal;
                            }
                        }
                        else
                        {
                            dt.Dispose();

                            #region 있을때 빈 아이템만.
                            string[] arryITEMCD_T = VB.Split(strChartItems, ",");

                            for (int j = 0; j < arryITEMCD_T.Length; j++)
                            {
                                SQL = "";
                                SQL = SQL + ComNum.VBLF + "SELECT ";
                                SQL = SQL + ComNum.VBLF + "    A.ITEMCD";
                                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET A";
                                SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + AcpEmr.acpNo;
                                SQL = SQL + ComNum.VBLF + "    AND A.FORMNO = " + mstrFormNo;
                                SQL = SQL + ComNum.VBLF + "    AND A.JOBGB IN ('IVT', 'IIO', 'IST', 'IBN')";
                                SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = '" + strChartDate + "'";
                                SQL = SQL + ComNum.VBLF + "    AND A.ITEMCD = '" + arryITEMCD_T[j].Trim().Replace("'", "") + "'";

                                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    return false;
                                }

                                if (dt.Rows.Count == 0)
                                {
                                    SQL = "";
                                    SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRBVITALSET ";
                                    SQL = SQL + ComNum.VBLF + "(FORMNO, ACPNO, PTNO, CHARTDATE, JOBGB, ITEMCD, WRITEDATE, WRITETIME, WRITEUSEID)";
                                    SQL = SQL + ComNum.VBLF + "VALUES";
                                    SQL = SQL + ComNum.VBLF + "    (";
                                    SQL = SQL + ComNum.VBLF + "    " + mstrFormNo + ","; //FORMNO
                                    SQL = SQL + ComNum.VBLF + "    " + AcpEmr.acpNo + ","; //ACPNO
                                    SQL = SQL + ComNum.VBLF + "    '" + AcpEmr.ptNo + "',"; //PTNO
                                    SQL = SQL + ComNum.VBLF + "    '" + strChartDate + "',"; //CHARTDATE
                                    SQL = SQL + ComNum.VBLF + "    '" + "IVT" + "',"; //JOBGB
                                    SQL = SQL + ComNum.VBLF + "    '" + arryITEMCD_T[j].Trim().Replace("'", "") + "',"; //ITEMCD
                                    SQL = SQL + ComNum.VBLF + "    '" + strCurDate + "',"; //WRITEDATE
                                    SQL = SQL + ComNum.VBLF + "    '" + strCurTime + "',"; //WRITETIME
                                    SQL = SQL + ComNum.VBLF + "    '" + clsType.User.IdNumber + "'"; //WRITEUSEID
                                    SQL = SQL + ComNum.VBLF + "    )";

                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        ComFunc.MsgBox("아이템 저장중 오류가 발생하였습니다.");
                                        return rtnVal;
                                    }
                                }
                                dt.Dispose();
                                dt = null;
                            }
                            #endregion
                        }

                        #endregion
                    }
                    else
                    {
                        if (clsEmrQueryEtc.GetSetTodayItem(clsDB.DbCon, AcpEmr, strChartDate, mstrFormNo) == false)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                    #endregion //해당일자에 아이템이 있는지 확인한다

                    //해당일자에 차팅된 아이템이 있는지 확인한다
                    if (clsEmrQueryEtc.GetSetTodayChartedItem(clsDB.DbCon, AcpEmr, strChartItems, strChartDate, mstrFormNo, strCurDate, strCurTime, "IVT") == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    //데이타 저장하기
                    if (SaveDataVitalSub(lngRow, AcpEmr, strChartDate, strChartTime, strCurDate, strCurTime) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                rtnVal = true;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }
        
        /// <summary>
        /// 임상관찰 기록지를 실제 저장하는 부분
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="AcpEmr"></param>
        /// <param name="strChartDate"></param>
        /// <param name="strChartTime"></param>
        /// <param name="strCurDate"></param>
        /// <param name="strCurTime"></param>
        /// <returns></returns>
        private bool SaveDataVitalSub(int Row, EmrPatient AcpEmr, string strChartDate, string strChartTime, string strCurDate, string strCurTime)
        {
            bool rtnVal = false;

            try
            {
                //0. AEMRBVITALTIME 저장을 한다
                if (clsEmrQueryEtc.SaveAEMRBVITALTIME(clsDB.DbCon, AcpEmr, strChartDate, strChartTime, mstrFormNo, strCurDate, strCurTime) == false)
                {
                    return false;
                }

                //0. AEMRCHRATMST, AEMRCHARTROW 저장을 한다
                if (SaveAEMRCHRATMSTandAEMRCHARTROW(Row, AcpEmr, strChartDate, strChartTime, strCurDate, strCurTime) == 0)
                {
                    //전자인증을 한다

                    return false;
                }

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        /// <summary>
        /// 임상관찰 기록지를 실제 저장하는 부분 : AEMRCHRATMST, AEMRCHARTROW
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="AcpEmr"></param>
        /// <param name="strChartDate"></param>
        /// <param name="strChartTime"></param>
        /// <param name="strCurDate"></param>
        /// <param name="strCurTime"></param>
        /// <returns></returns>
        private double SaveAEMRCHRATMSTandAEMRCHARTROW(int Row, EmrPatient AcpEmr, string strChartDate, string strChartTime, string strCurDate, string strCurTime)
        {
            double rtnVal = 0;

            DataTable dt = null;
            int i = 0;
            int j = 0;
            string SQL = "";
            string SqlErr = "";

            double dblEmrHisNo = 0;
            double dblEmrNoNew = 0;

            string strCHARTUSEID = clsType.User.IdNumber;
            string strCOMPUSEID = clsType.User.IdNumber;
            string strSaveFlag = "SAVE";
            string strSAVEGB = "1";
            string strSAVECERT = "1"; // 0:임시저장, 1:인증저장
            string strFORMGB = "0";

            try
            {

                #region //차팅이 된 아이템을 변수에 담는다.
                string[] arryEMRNO_T = null;
                string[] arryITEMCD_T = null;
                string[] arryITEMNO_T = null;
                string[] arryITEMINDEX_T = null;
                string[] arryITEMTYPE_T = null;
                string[] arryITEMVALUE_T = null;
                string[] arryITEMVALUE1_T = null;
                string[] arryDSPSEQ_T = null;

                string strChartItems = "";

                for (int lngCol2 = mlngStartCol; lngCol2 < ssUserChart_Sheet1.ColumnCount; lngCol2++)
                {
                    if (lngCol2 == ssUserChart_Sheet1.ColumnCount)
                        break;

                    string strITEMCD = ssItem_Sheet1.Cells[0, lngCol2].Text.Trim();
                    string strITEMNO = "";
                    string strITEMINDEX = "";
                    string strITEMTYPE = "";
                    string strITEMVALUE = "";
                    string strITEMVALUE1 = "";
                    string strDSPSEQ = i.ToString();

                    if (VB.InStr(strITEMCD, "_") > 0)
                    {
                        strITEMNO = VB.Split(strITEMCD, "_")[0];
                        strITEMINDEX = VB.Split(strITEMCD, "_")[1];
                    }
                    else
                    {
                        strITEMNO = strITEMCD;
                        strITEMINDEX = "-1";
                    }

                    strITEMTYPE = "TEXT"; //pFormFlowSheet[i].CellType;
                    strITEMVALUE = ssUserChart_Sheet1.Cells[Row, lngCol2].Text.Trim();

                    if (strITEMVALUE != "")
                    {
                        strChartItems = strChartItems + "'" + strITEMCD + "',";

                        if (arryEMRNO_T == null)
                        {
                            arryEMRNO_T = new string[0];
                            arryITEMCD_T = new string[0];
                            arryITEMNO_T = new string[0];
                            arryITEMINDEX_T = new string[0];
                            arryITEMTYPE_T = new string[0];
                            arryITEMVALUE_T = new string[0];
                            arryITEMVALUE1_T = new string[0];
                            arryDSPSEQ_T = new string[0];
                        }

                        Array.Resize<string>(ref arryEMRNO_T, arryEMRNO_T.Length + 1);
                        Array.Resize<string>(ref arryITEMCD_T, arryITEMCD_T.Length + 1);
                        Array.Resize<string>(ref arryITEMNO_T, arryITEMNO_T.Length + 1);
                        Array.Resize<string>(ref arryITEMINDEX_T, arryITEMINDEX_T.Length + 1);
                        Array.Resize<string>(ref arryITEMTYPE_T, arryITEMTYPE_T.Length + 1);
                        Array.Resize<string>(ref arryITEMVALUE_T, arryITEMVALUE_T.Length + 1);
                        Array.Resize<string>(ref arryITEMVALUE1_T, arryITEMVALUE1_T.Length + 1);
                        Array.Resize<string>(ref arryDSPSEQ_T, arryDSPSEQ_T.Length + 1);

                        arryEMRNO_T[arryEMRNO_T.Length - 1] = dblEmrNoNew.ToString();
                        arryITEMCD_T[arryEMRNO_T.Length - 1] = strITEMCD;
                        arryITEMNO_T[arryEMRNO_T.Length - 1] = strITEMNO;
                        arryITEMINDEX_T[arryEMRNO_T.Length - 1] = strITEMINDEX;
                        arryITEMTYPE_T[arryEMRNO_T.Length - 1] = strITEMTYPE;
                        arryITEMVALUE_T[arryEMRNO_T.Length - 1] = strITEMVALUE.Replace("'", "`");
                        arryITEMVALUE1_T[arryEMRNO_T.Length - 1] = strITEMVALUE1.Replace("'", "`");
                        arryDSPSEQ_T[arryEMRNO_T.Length - 1] = strDSPSEQ;
                    }
                }

                strChartItems = VB.Left(strChartItems, strChartItems.Length - 1);

                #endregion //차팅이 된 아이템을 변수에 담는다.

                //동일 시간에 저장된 데이타가 있는지 조회 한다
                double dblEmrNo = 0;
                SQL = " ";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "    C.EMRNO ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST C";
                SQL = SQL + ComNum.VBLF + "WHERE C.PTNO = '" + AcpEmr.ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "    AND C.CHARTDATE = '" + strChartDate + "' ";
                SQL = SQL + ComNum.VBLF + "    AND SUBSTR(C.CHARTTIME, 1, 4) = '" + VB.Left(strChartTime, 4) + "' ";
                SQL = SQL + ComNum.VBLF + "    AND C.FORMNO = " + mstrFormNo;

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    dblEmrNo = VB.Val(dt.Rows[0]["EMRNO"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;

                //당일 아이템을 조회해서 다시 정리 한다
                #region //Query

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "    A.CHARTDATE, B.ITEMCD, B.BASNAME, B.BASEXNAME, B.ITEMGROUP, B.ITEMGROUPNM, ";
                SQL = SQL + ComNum.VBLF + "    R.ITEMVALUE, R.ITEMVALUE1, R.INPUSEID, R.INPDATE, R.INPTIME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET A";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ( ";
                SQL = SQL + ComNum.VBLF + "    SELECT ";
                SQL = SQL + ComNum.VBLF + "        B.BASCD AS ITEMCD, B.BASNAME, B.BASEXNAME, B.NFLAG1  AS SORT3, B.NFLAG2  AS SORT4, B.NFLAG3  AS SORT5, B.DISSEQNO  AS SORT6, ";
                SQL = SQL + ComNum.VBLF + "        BB.BASCD AS ITEMGROUP, BB.BASNAME AS ITEMGROUPNM, BB.VFLAG1 AS SORT1, BB.DISSEQNO AS SORT2 ";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRBASCD B ";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB ";
                SQL = SQL + ComNum.VBLF + "        ON B.VFLAG1 = BB.BASCD ";
                SQL = SQL + ComNum.VBLF + "        AND BB.BSNSCLS = '기록지관리' ";
                SQL = SQL + ComNum.VBLF + "        AND BB.UNITCLS IN ('임상관찰그룹')  ";
                SQL = SQL + ComNum.VBLF + "    WHERE B.BSNSCLS = '기록지관리' ";
                SQL = SQL + ComNum.VBLF + "        AND B.UNITCLS IN ('임상관찰')  ";
                SQL = SQL + ComNum.VBLF + "    UNION ALL ";
                SQL = SQL + ComNum.VBLF + "    SELECT ";
                SQL = SQL + ComNum.VBLF + "        B.BASCD AS ITEMCD, B.BASNAME, B.BASEXNAME, B.NFLAG1  AS SORT3, B.NFLAG2  AS SORT4, B.NFLAG3  AS SORT5, B.DISSEQNO  AS SORT6, ";
                SQL = SQL + ComNum.VBLF + "        BB.BASCD AS ITEMGROUP, BB.BASNAME AS ITEMGROUPNM, BB.VFLAG1 AS SORT1, BB.DISSEQNO AS SORT2 ";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRBASCD B ";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB ";
                SQL = SQL + ComNum.VBLF + "        ON B.VFLAG1 = BB.BASCD ";
                SQL = SQL + ComNum.VBLF + "        AND BB.BSNSCLS = '기록지관리' ";
                SQL = SQL + ComNum.VBLF + "        AND BB.UNITCLS IN ('섭취배설그룹')  ";
                SQL = SQL + ComNum.VBLF + "    WHERE B.BSNSCLS = '기록지관리' ";
                SQL = SQL + ComNum.VBLF + "        AND B.UNITCLS IN ('섭취배설') ";
                SQL = SQL + ComNum.VBLF + "    UNION ALL ";
                SQL = SQL + ComNum.VBLF + "    SELECT ";
                SQL = SQL + ComNum.VBLF + "        B.BASCD AS ITEMCD, B.BASNAME, B.BASEXNAME, B.NFLAG1  AS SORT3, B.NFLAG2  AS SORT4, B.NFLAG3  AS SORT5, B.DISSEQNO  AS SORT6, ";
                SQL = SQL + ComNum.VBLF + "        BB.BASCD AS ITEMGROUP, BB.BASNAME AS ITEMGROUPNM, BB.VFLAG1 AS SORT1, BB.DISSEQNO AS SORT2 ";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRBASCD B ";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB ";
                SQL = SQL + ComNum.VBLF + "        ON B.VFLAG1 = BB.BASCD ";
                SQL = SQL + ComNum.VBLF + "        AND BB.BSNSCLS = '기록지관리' ";
                SQL = SQL + ComNum.VBLF + "        AND BB.UNITCLS IN ('특수치료그룹')  ";
                SQL = SQL + ComNum.VBLF + "    WHERE B.BSNSCLS = '기록지관리' ";
                SQL = SQL + ComNum.VBLF + "        AND B.UNITCLS IN ('특수치료') ";
                SQL = SQL + ComNum.VBLF + "    UNION ALL ";
                SQL = SQL + ComNum.VBLF + "    SELECT ";
                SQL = SQL + ComNum.VBLF + "        B.BASCD AS ITEMCD, B.BASNAME, B.BASEXNAME, B.NFLAG1  AS SORT3, B.NFLAG2  AS SORT4, B.NFLAG3  AS SORT5, B.DISSEQNO  AS SORT6, ";
                SQL = SQL + ComNum.VBLF + "        BB.BASCD AS ITEMGROUP, BB.BASNAME AS ITEMGROUPNM, BB.VFLAG1 AS SORT1, BB.DISSEQNO AS SORT2 ";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRBASCD B ";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB ";
                SQL = SQL + ComNum.VBLF + "        ON B.VFLAG1 = BB.BASCD ";
                SQL = SQL + ComNum.VBLF + "        AND BB.BSNSCLS = '기록지관리' ";
                SQL = SQL + ComNum.VBLF + "        AND BB.UNITCLS IN ('기본간호그룹')  ";
                SQL = SQL + ComNum.VBLF + "    WHERE B.BSNSCLS = '기록지관리' ";
                SQL = SQL + ComNum.VBLF + "        AND B.UNITCLS IN ('기본간호') ";
                SQL = SQL + ComNum.VBLF + "    ) B ";
                SQL = SQL + ComNum.VBLF + "    ON A.ITEMCD = B.ITEMCD ";
                SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_EMR + "AEMRCHARTMST C ";
                SQL = SQL + ComNum.VBLF + "    ON A.ACPNO = C.ACPNO ";
                SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = C.CHARTDATE ";
                SQL = SQL + ComNum.VBLF + "    AND A.FORMNO = C.FORMNO ";
                SQL = SQL + ComNum.VBLF + "    AND SUBSTR(C.CHARTTIME, 1, 4) = '" + VB.Left(strChartTime, 4) + "' ";
                SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R ";
                SQL = SQL + ComNum.VBLF + "    ON C.EMRNO = R.EMRNO ";
                SQL = SQL + ComNum.VBLF + "    AND A.ITEMCD = R.ITEMCD ";
                SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + AcpEmr.acpNo;
                SQL = SQL + ComNum.VBLF + "    AND A.FORMNO = " + mstrFormNo;
                SQL = SQL + ComNum.VBLF + "    AND A.JOBGB IN ('IVT', 'IIO', 'IST', 'IBN')";
                SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = '" + strChartDate + "'";
                SQL = SQL + ComNum.VBLF + "ORDER BY B.SORT1, B.SORT2, B.SORT3, B.SORT4, B.SORT5, B.SORT6 ";

                #endregion //Query

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBoxEx(this, AcpEmr.ptName + "[" + AcpEmr.ptNo + "] 님의" + ComNum.VBLF + "당일 임상관찰 아이템을 찾을 수 없습니다.");
                    return rtnVal;
                }

                #region //저장을 위하여 배열에 다시 담는다

                string[] arryEMRNO = new string[dt.Rows.Count];
                string[] arryITEMCD = new string[dt.Rows.Count];
                string[] arryITEMNO = new string[dt.Rows.Count];
                string[] arryITEMINDEX = new string[dt.Rows.Count];
                string[] arryITEMTYPE = new string[dt.Rows.Count];
                string[] arryITEMVALUE = new string[dt.Rows.Count];
                string[] arryITEMVALUE1 = new string[dt.Rows.Count];
                string[] arryDSPSEQ = new string[dt.Rows.Count];
                string[] arryINPUSEID = new string[dt.Rows.Count];
                string[] arryINPDATE = new string[dt.Rows.Count];
                string[] arryINPTIME = new string[dt.Rows.Count];

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    bool IsExistsItem = false;
                    for (j = 0; j < arryITEMCD_T.Length; j++)
                    {
                        if (arryITEMCD_T[j].Trim() == dt.Rows[i]["ITEMCD"].ToString().Trim())
                        {
                            IsExistsItem = true;
                            break;
                        }
                    }

                    arryEMRNO[i] = dblEmrNoNew.ToString();
                    arryITEMCD[i] = dt.Rows[i]["ITEMCD"].ToString().Trim();

                    if (VB.InStr(arryITEMCD[i], "_") > 0)
                    {
                        arryITEMNO[i] = VB.Split(arryITEMCD[i], "_")[0];
                        arryITEMINDEX[i] = VB.Split(arryITEMCD[i], "_")[1];
                    }
                    else
                    {
                        arryITEMNO[i] = arryITEMCD[i];
                        arryITEMINDEX[i] = "-1";
                    }

                    arryITEMTYPE[i] = "TEXT";
                    arryITEMVALUE[i] = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                    arryITEMVALUE1[i] = dt.Rows[i]["ITEMVALUE1"].ToString().Trim();
                    arryDSPSEQ[i] = i.ToString();
                    arryINPUSEID[i] = dt.Rows[i]["INPUSEID"].ToString().Trim();
                    arryINPDATE[i] = dt.Rows[i]["INPDATE"].ToString().Trim();
                    arryINPTIME[i] = dt.Rows[i]["INPTIME"].ToString().Trim();

                    if (IsExistsItem == true)
                    {
                        arryITEMVALUE[i] = arryITEMVALUE_T[j];
                        arryINPUSEID[i] = clsType.User.IdNumber;
                        arryINPDATE[i] = strCurDate;
                        arryINPTIME[i] = strCurTime;
                    }
                }
                dt.Dispose();
                dt = null;

                #endregion //저장을 위하여 배열에 다시 담는다

                dblEmrHisNo = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "EMRXMLHISNO"));

                if (dblEmrNo > 0)
                {
                    dblEmrNoNew = dblEmrNo;

                    #region //과거기록 백업
                    SqlErr = clsEmrQuery.SaveChartMastHis(clsDB.DbCon, dblEmrNo.ToString(), dblEmrHisNo, strCurDate, strCurTime, "C", strSaveFlag, strCHARTUSEID);
                    if (SqlErr != "OK")
                    {
                        ComFunc.MsgBox("SaveAEMRCHRATMSTandAEMRCHARTROW 오류가 발생했습니다.");
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    #endregion
                }
                else
                {
                    dblEmrNoNew = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "GETEMRXMLNO"));
                }

                if (clsEmrQuery.SaveChartMstOnly(clsDB.DbCon, dblEmrNoNew, dblEmrHisNo, AcpEmr, mstrFormNo, mstrUpdateNo,
                            strChartDate, strChartTime,
                            strCHARTUSEID, strCOMPUSEID, strSAVEGB, strSAVECERT, strFORMGB,
                            strCurDate, strCurTime, strSaveFlag) == false)
                {
                    ComFunc.MsgBox("SaveAEMRCHRATMSTandAEMRCHARTROW 오류가 발생했습니다.");
                    return rtnVal;
                }
                
                #region //CHARTROW
                SQL = "";
                SQL = SQL + "\r\n" + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTROW ";
                SQL = SQL + "\r\n" + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUSEID, INPDATE, INPTIME )";
                SQL = SQL + "\r\n" + "VALUES (";
                SQL = SQL + "\r\n" + dblEmrNoNew.ToString() + ", ";    //EMRNO
                SQL = SQL + "\r\n" + dblEmrHisNo.ToString() + ", ";    //EMRNOHIS
                SQL = SQL + "\r\n" + " :ITEMCD, ";   //ITEMCD
                SQL = SQL + "\r\n" + " :ITEMNO, "; //ITEMNO
                SQL = SQL + "\r\n" + " :ITEMINDEX, "; //ITEMINDEX
                SQL = SQL + "\r\n" + " :ITEMTYPE, ";   //ITEMTYPE
                SQL = SQL + "\r\n" + " :ITEMVALUE, ";   //ITEMVALUE
                SQL = SQL + "\r\n" + " :DSPSEQ, ";   //DSPSEQ
                SQL = SQL + "\r\n" + " :ITEMVALUE1, ";   //ITEMVALUE
                SQL = SQL + ComNum.VBLF + " :INPUSEID,";   //INPUSEID
                SQL = SQL + ComNum.VBLF + " :INPDATE, ";   //INPDATE
                SQL = SQL + ComNum.VBLF + " :INPTIME ";   //INPTIME
                SQL = SQL + ComNum.VBLF + ")";

                SqlErr = clsDB.ExecuteChartRowEx(clsDB.DbCon, SQL, dblEmrNoNew, dblEmrHisNo, arryITEMCD, arryITEMNO, arryITEMINDEX, arryITEMTYPE,
                    arryITEMVALUE, arryDSPSEQ, arryITEMVALUE1, arryINPUSEID, arryINPDATE, arryINPTIME);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return 0;
                }

                #endregion //CHARTROW

                rtnVal = dblEmrNoNew;

                clsEmrQuery.SaveEmrCert(clsDB.DbCon, dblEmrNoNew, false);
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        #endregion //Private Function

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetPatList();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            for (int lngRow = 0; lngRow < ssUserChart_Sheet1.RowCount; lngRow++)
            {
                if (ssUserChart_Sheet1.Cells[lngRow, 0].Text.Trim().Equals("True"))
                {
                    for (int lngCol = 13; lngCol < ssUserChart_Sheet1.ColumnCount; lngCol++)
                    {
                        ssUserChart_Sheet1.Cells[lngRow, lngCol].Text = "";
                    }
                }
            }
        }

        private void btnSearchPreHis_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(mstrFormNo))
                return;

            if (ssUserChart_Sheet1.RowCount == 0)
                return;

            Cursor.Current = Cursors.WaitCursor;

            for (int lngRow = 0; lngRow < ssUserChart_Sheet1.RowCount; lngRow++)
            {
                GetUchartHis(ssUserChart_Sheet1.Cells[lngRow, 3].Text.Trim(), mstrFormNo, ssUserChart, lngRow);
            }

            Cursor.Current = Cursors.Default;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveData() == true)
            {
                ComFunc.MsgBoxEx(this, "작업을 완료했습니다.");
                GetPatList();
                chkAll.Checked = false;
            }
        }

        private void btnPrintPreView_Click(object sender, EventArgs e)
        {
            PreViewAndPrint("V");
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PreViewAndPrint("P");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mbtnTime_Click(object sender, EventArgs e)
        {
            if (usTimeSetEvent != null)
            {
                usTimeSetEvent.Dispose();
                usTimeSetEvent = null;
            }

            usTimeSetEvent = new usTimeSet();
            usTimeSetEvent.rSetTime += new usTimeSet.SetTime(usTimeSetEvent_SetTime);
            usTimeSetEvent.rEventClosed += new usTimeSet.EventClosed(usTimeSetEvent_EventClosed);
            this.Controls.Add(usTimeSetEvent);
            usTimeSetEvent.Left = 179;
            usTimeSetEvent.Top = 75;
            usTimeSetEvent.BringToFront();
        }

        private void usTimeSetEvent_SetTime(string strText)
        {
            if (usTimeSetEvent != null)
            {
                usTimeSetEvent.Dispose();
                usTimeSetEvent = null;
            }
            txtMedFrTime.Text = strText;
        }

        private void usTimeSetEvent_EventClosed()
        {
            if (usTimeSetEvent != null)
            {
                usTimeSetEvent.Dispose();
                usTimeSetEvent = null;
            }
        }

        private void btnDuty_Click(object sender, EventArgs e)
        {
            string BtnText = (sender as Button).Text.Trim();
            int lngFcol = -1;

            for (int lngCol = 0; lngCol < ssUserChart_Sheet1.ColumnCount; lngCol++)
            {
                if (ssUserChart_Sheet1.Columns[lngCol].Label.ToUpper().Equals("DUTY"))
                {
                    lngFcol = lngCol;
                    break;
                }
            }

            if (lngFcol == -1)
                return;

            for (int lngRow = 0; lngRow < ssUserChart_Sheet1.RowCount; lngRow++)
            {
                if (ssUserChart_Sheet1.Cells[lngRow, 0].Text.Trim().Equals("True"))
                {
                    ssUserChart_Sheet1.Cells[lngRow, lngFcol].Text = BtnText;
                }
            }
        }

        private void dtpChartDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void ssFORM_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssFORM_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                return;
            }

            ssFORMCellClick(e.Row);
        }

        private void ssFORMCellClick(int Row)
        {
            mstrFormNo = ssFORM_Sheet1.Cells[Row, 3].Text.Trim();
            mstrFORMNAME = ssFORM_Sheet1.Cells[Row, 1].Text.Trim();
            mstrUpdateNo = clsEmrQuery.GetNewFormMaxUpdateNo(clsDB.DbCon, VB.Val(mstrFormNo)).ToString();

            fWrite = null;
            fWrite = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, mstrFormNo, mstrUpdateNo);

            lblFORMNAME.Text = mstrFORMNAME;

            MakeFowChart();
        }

        private void ssUserChart_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            if (ssUserChart_Sheet1.RowCount == 0)
                return;

            if (e.Column < 1)
                return;

            if (ChkAutoWrite.Checked == false)
                return;

            switch (e.Column)
            {
                case 13:
                case 14:
                    if (mstrFormNo.Equals("1562") == false)
                    {
                        return;
                    }
                    break;
                default:
                    return;
            }

            int nSelectCol = e.Column;
            string strData = ssUserChart_Sheet1.Cells[e.Row, e.Column].Text.Trim();
            if (ssUserChart_Sheet1.Cells[e.Row, 0].Text.Trim().Equals("True"))
            {
                for (int i = 0; i < ssUserChart_Sheet1.RowCount; i++)
                {
                    if (ssUserChart_Sheet1.Cells[e.Row, 0].Text.Trim().Equals("True"))
                    {
                        ssUserChart_Sheet1.Cells[i, nSelectCol].Text = strData;
                    }
                }
            }
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            ssUserChart_Sheet1.Cells[0, 0, ssUserChart_Sheet1.RowCount - 1, 0].Text = chkAll.Checked ? "True" : "False";
        }


        private void ssUserChart_EditChange(object sender, EditorNotifyEventArgs e)
        {
          

        }

        private void GeneralEditor_KeyDown(object sender, KeyEventArgs e)
        {
      
        }

        private void ssUserChart_EditorFocused(object sender, EditorNotifyEventArgs e)
        {
            if (e.EditingControl.GetType().Equals(typeof(FarPoint.Win.Spread.CellType.GeneralEditor)))
            {
                FarPoint.Win.Spread.CellType.GeneralEditor ge = (FarPoint.Win.Spread.CellType.GeneralEditor) e.EditingControl;
                ge.Tag = e.Row;
                ge.PreviewKeyDown += Ge_PreviewKeyDown;
            }
        }

        private void Ge_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (ssUserChart_Sheet1.ActiveRowIndex + 1 > ssUserChart_Sheet1.RowCount - 1)
                    return;

                ssUserChart_Sheet1.ActiveRowIndex += 1;
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (ssUserChart_Sheet1.ActiveRowIndex <= 0)
                    return;

                ssUserChart_Sheet1.ActiveRowIndex -= 1;
            }
            else if (e.KeyCode == Keys.Left)
            {
                if (ssUserChart_Sheet1.ActiveColumnIndex <= 0)
                    return;

                ssUserChart_Sheet1.ActiveColumnIndex -= 1;
            }
            else if (e.KeyCode == Keys.Right)
            {
                if (ssUserChart_Sheet1.ActiveColumnIndex + 1 < ssUserChart_Sheet1.ColumnCount - 1)
                    return;

                ssUserChart_Sheet1.ActiveColumnIndex += 1;
            }
        }
    }
}
