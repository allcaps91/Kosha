using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmQIVerbalCheck : Form
    {
        public frmQIVerbalCheck()
        {
            InitializeComponent();
        }

        private void frmQIVerbalCheck_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ssList_Sheet1.RowCount = 0;
            ssView2_Sheet1.RowCount = 0;
            ssView3_Sheet1.RowCount = 0;

            cboTeam.Items.Clear();
            cboTeam.Items.Add("전체");
            cboTeam.Items.Add("A");
            cboTeam.Items.Add("B");
            cboTeam.SelectedIndex = 0;

            cboTeam2.Items.Clear();
            cboTeam2.Items.Add("전체");
            cboTeam2.Items.Add("A");
            cboTeam2.Items.Add("B");
            cboTeam2.SelectedIndex = 0;

            Set_cboWard();

            clsVbfunc.SetComboDept(clsDB.DbCon, cboDept, "1", 1);
            clsVbfunc.SetComboDept(clsDB.DbCon, cboDept2, "1", 1);

            cboDrCode.Items.Clear();
            cboDrCode.Items.Add("****.전체");
            cboDrCode.SelectedIndex = 0;
            cboDrCode2.Items.Clear();
            cboDrCode2.Items.Add("****.전체");
            cboDrCode2.SelectedIndex = 0;

            dtpSDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).AddDays(-1);
            dtpEDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));

            dtpSDate2.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).AddDays(-1);
            dtpEDate2.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));

            Search_Data();
        }

        //void Read_Verbal_Order(string argPano, string argSDate, string argEDate, string argWard = "")
        //{
        //    int i = 0;
        //    DataTable dt = null;
        //    string SQL = "";    //Query문
        //    string SqlErr = ""; //에러문 받는 변수

        //    string strPtNo = "";
        //    string strBDATE = "";
        //    string strORDERCODE = "";
        //    string strORDERNO = "";

        //    string strDate1 = "";
        //    string strDate2 = "";

        //    //ssView_Sheet1.Columns[1].Visible = false;
        //    //ssView_Sheet1.Columns[2].Visible = false;

        //    if (argWard != "")
        //    {
        //        if (ssList_Sheet1.RowCount == 0)
        //        {
        //            return;
        //        }

        //        //ssView_Sheet1.Columns[1].Visible = true;
        //        //ssView_Sheet1.Columns[2].Visible = true;

        //        strPtNo = "";

        //        for (i = 0; i < ssList_Sheet1.RowCount; i++)
        //        {
        //            strPtNo += ssList_Sheet1.Cells[i, 1].Text.Trim() + "','";
        //        }

        //        if (strPtNo != "")
        //        {
        //            strPtNo = VB.Mid("'" + strPtNo, 1, strPtNo.Length - 1);
        //        }
        //    }

        //    //ssView_Sheet1.RowCount = 0;

        //    Cursor.Current = Cursors.WaitCursor;

        //    try
        //    {
        //        SQL = "";
        //        SQL = " SELECT PTNO, BDATE, BUN, ORDERCODE, DOSCODE, ";
        //        SQL = SQL + ComNum.VBLF + " CONTENTS, REALQTY, GBDIV, NAL, ";
        //        SQL = SQL + ComNum.VBLF + " TO_CHAR(ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE, NURSEID, V_ORDERNO, ORDERNO, ";
        //        SQL = SQL + ComNum.VBLF + " SLIPNO, SUCODE, GBVERB, ROWID, TO_CHAR(DRORDERVIEW,'YYYY-MM-DD HH24:MI') DRORDERVIEW";
        //        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_IORDER M";

        //        if (argWard != "")
        //        {
        //            SQL = SQL + ComNum.VBLF + " WHERE PTNO IN (" + strPtNo + ")";
        //        }
        //        else
        //        {
        //            SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + argPano + "'";
        //        }

        //        SQL = SQL + ComNum.VBLF + "   AND GbStatus IN (' ','D','D+')";
        //        SQL = SQL + ComNum.VBLF + "   AND BDate >= TO_DATE('" + argSDate + "','YYYY-MM-DD')";
        //        SQL = SQL + ComNum.VBLF + "   AND BDate <= TO_DATE('" + argEDate + "','YYYY-MM-DD')";
        //        SQL = SQL + ComNum.VBLF + "   AND NurseID <> ' '";
        //        SQL = SQL + ComNum.VBLF + "   AND GbVerb IN ('Y','C')";
        //        SQL = SQL + ComNum.VBLF + "   AND (GbSend  = ' ' OR GbSend IS NULL)";
        //        SQL = SQL + ComNum.VBLF + "   AND OrderSite Not Like 'DC%'";
        //        SQL = SQL + ComNum.VBLF + "   AND OrderSite <>  'CAN'";

        //        if (cboWard.Text == "ER")
        //        {
        //            SQL = SQL + ComNum.VBLF + "   AND GBIOE IN ('E','EI')";
        //        }
        //        else
        //        {
        //            SQL = SQL + ComNum.VBLF + "   AND (GBIOE NOT IN ('E','EI') OR GBIOE IS NULL)";
        //        }

        //        SQL = SQL + ComNum.VBLF + "   AND Bun IN ('11','12','20')";
        //        SQL = SQL + ComNum.VBLF + "   AND NOT EXISTS (";
        //        SQL = SQL + ComNum.VBLF + "     SELECT PTNO";
        //        SQL = SQL + ComNum.VBLF + "       FROM " + ComNum.DB_PMPA + "NUR_VERBAL_ORDER_CONFIRM S ";
        //        SQL = SQL + ComNum.VBLF + "      WHERE M.PTNO = S.PTNO";
        //        SQL = SQL + ComNum.VBLF + "        AND M.ORDERNO = S.ORDERNO";
        //        SQL = SQL + ComNum.VBLF + "        AND M.BDATE = S.BDATE";
        //        SQL = SQL + ComNum.VBLF + "        AND M.ENTDATE = S.ENTDATE) ";

        //        if (argWard != "")
        //        {
        //            SQL = SQL + ComNum.VBLF + "    ORDER BY M.PTNO, M.BDATE, M.BUN, M.ENTDATE";
        //        }
        //        else
        //        {
        //            SQL = SQL + ComNum.VBLF + "    ORDER BY M.BDATE, M.BUN, M.ENTDATE";
        //        }

        //        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

        //        if (SqlErr != "")
        //        {
        //            Cursor.Current = Cursors.Default;
        //            ComFunc.MsgBox("조회중 문제가 발생했습니다");
        //            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
        //            return;
        //        }
        //        if (dt.Rows.Count > 0)
        //        {
        //            ssView_Sheet1.RowCount = dt.Rows.Count;
        //            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

        //            for (i = 0; i < dt.Rows.Count; i++)
        //            {
        //                strDate1 = dt.Rows[i]["ENTDATE"].ToString().Trim();
        //                strDate2 = dt.Rows[i]["DRORDERVIEW"].ToString().Trim();
        //                strBDATE = dt.Rows[i]["BDATE"].ToString().Trim();

        //                ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
        //                ssView_Sheet1.Cells[i, 2].Text = clsVbfunc.GetPatientName(clsDB.DbCon, dt.Rows[i]["PTNO"].ToString().Trim());
        //                ssView_Sheet1.Cells[i, 6].Text = VB.Left(strBDATE, 10);
        //                ssView_Sheet1.Cells[i, 7].Text = Read_Bun(dt.Rows[i]["BUN"].ToString().Trim());
        //                strORDERCODE = dt.Rows[i]["ORDERCODE"].ToString().Trim();
        //                ssView_Sheet1.Cells[i, 8].Text = Read_OrderName(strORDERCODE, dt.Rows[i]["SLIPNO"].ToString().Trim());
        //                ssView_Sheet1.Cells[i, 9].Text = Read_DOSName(dt.Rows[i]["DOSCODE"].ToString().Trim());
        //                ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["CONTENTS"].ToString().Trim();

        //                if (ssView_Sheet1.Cells[i, 10].Text == "0")
        //                {
        //                    ssView_Sheet1.Cells[i, 10].Text = "";
        //                }

        //                ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["REALQTY"].ToString().Trim();
        //                ssView_Sheet1.Cells[i, 12].Text = dt.Rows[i]["GBDIV"].ToString().Trim();
        //                ssView_Sheet1.Cells[i, 13].Text = dt.Rows[i]["NAL"].ToString().Trim();
        //                ssView_Sheet1.Cells[i, 14].Text = dt.Rows[i]["ENTDATE"].ToString().Trim() + "(" + clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["NURSEID"].ToString().Trim()) + ")";
        //                strORDERNO = dt.Rows[i]["ORDERNO"].ToString().Trim();
        //                strPtNo = argPano;
        //                ssView_Sheet1.Cells[i, 15].Text = dt.Rows[i]["DRORDERVIEW"].ToString().Trim();
        //                ssView_Sheet1.Cells[i, 16].Text = "";
        //                ssView_Sheet1.Cells[i, 17].Text = "";
        //                ssView_Sheet1.Cells[i, 18].Text = strORDERNO;
        //                ssView_Sheet1.Cells[i, 19].Text = dt.Rows[i]["ROWID"].ToString().Trim();

        //                if (Read_Suga_Hang(dt.Rows[i]["SUCODE"].ToString().Trim()) == "OK")
        //                {
        //                    ssView_Sheet1.Cells[i, 8].Text = "★항혈전 " + ssView_Sheet1.Cells[i, 8].Text;
        //                    ssView_Sheet1.Cells[i, 8].ForeColor = Color.FromArgb(255, 0, 255);
        //                }

        //                if (Read_Suga_Component(dt.Rows[i]["SUCODE"].ToString().Trim()) == "OK")
        //                {
        //                    ssView_Sheet1.Cells[i, 8].Text = "<!> " + ssView_Sheet1.Cells[i, 8].Text;
        //                }

        //                switch (dt.Rows[i]["GBVERB"].ToString().Trim())
        //                {
        //                    case "Y":
        //                        ssView_Sheet1.Cells[i, 8].ForeColor = Color.FromArgb(240, 20, 20);
        //                        break;
        //                    case "C":
        //                        break;
        //                }

        //                ssView_Sheet1.Cells[i, 20].Text = Get_Time(strDate1, strDate2);
        //            }
        //        }

        //        dt.Dispose();
        //        dt = null;
        //        Cursor.Current = Cursors.Default;
        //    }
        //    catch (Exception ex)
        //    {
        //        Cursor.Current = Cursors.Default;
        //        ComFunc.MsgBox(ex.Message);
        //        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
        //        return;
        //    }
        //}

        string Read_DrConfirm_Gubun(string argPano, string argBDate, string argOrderCode, string argOrderNo, string argGbn = "")
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string rtnVar = "";

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE, DRCODE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_IORDER ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + argPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + argBDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND ORDERCODE = '" + argOrderCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND V_ORDERNO = " + argOrderNo;
                SQL = SQL + ComNum.VBLF + "   AND VERBC = 'Y' ";
                SQL = SQL + ComNum.VBLF + "   AND DRORDERVIEW IS NOT NULL ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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

                if (argGbn != "")
                {
                    rtnVar = dt.Rows[0]["ENTDATE"].ToString().Trim() + ComNum.VBLF + "(" + clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["DRCODE"].ToString().Trim()) + ")";
                }
                else
                {
                    rtnVar = dt.Rows[0]["ENTDATE"].ToString().Trim() + "(" + clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["DRCODE"].ToString().Trim()) + ")";
                }

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

        string Read_DOSName(string argCode)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string rtnVar = "";

            try
            {
                SQL = "";
                SQL = " SELECT DOSNAME";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_ODOSAGE ";
                SQL = SQL + ComNum.VBLF + " WHERE DOSCODE = '" + argCode + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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

                rtnVar = dt.Rows[0]["DOSNAME"].ToString().Trim();

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

        string Read_Bun(string argBun)
        {
            switch (argBun)
            {
                case "11":
                    return "경구";
                case "12":
                    return "외용";
                case "20":
                case "23":
                    return "주사";
            }

            return "";
        }

        string Read_OrderName(string argCode, string argNo)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string rtnVar = "";

            try
            {
                SQL = "";
                SQL = " SELECT ORDERNAMES,HNAME ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_ORDERCODE  A, " + ComNum.DB_MED + "OCS_DRUGINFO_NEW B";
                SQL = SQL + ComNum.VBLF + "  WHERE ORDERCODE = '" + argCode + "' ";
                SQL = SQL + ComNum.VBLF + "  AND SLIPNO = '" + argNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND A.OrderCode=B.SUNEXT(+) ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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

                rtnVar = dt.Rows[0]["HNAME"].ToString().Trim();

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
        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            Search_Data();
        }

        void Search_Data()
        {            
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strPano = "";
            string strRoom_Old = "";
            string strToDate = "";

            string strSysDate = "";

            ComFunc cf = new ComFunc();

            ssList_Sheet1.RowCount = 0;
            //ssView_Sheet1.RowCount = 0;
            ssView2_Sheet1.RowCount = 0;

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                //SQL = "SELECT M.ROOMCODE, M.PANO, M.SNAME, M.DEPTCODE, M.GBSTS ";
                //SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER  M ";

                SQL = SQL + ComNum.VBLF + "SELECT DISTINCT M.JDATE, TO_CHAR(M.INDATE, 'YYYY-MM-DD') INDATE, TO_CHAR(M.OUTDATE, 'YYYY-MM-DD') OUTDATE, ";
                SQL = SQL + ComNum.VBLF + "                M.ROOMCODE, M.PANO, M.SNAME, M.DEPTCODE,                                                   ";
                SQL = SQL + ComNum.VBLF + "                CASE WHEN M.GBSTS = 0 THEN '01.재원'                                                       ";
                SQL = SQL + ComNum.VBLF + "                     WHEN M.GBSTS = 9 THEN '03.취소'                                                       ";
                SQL = SQL + ComNum.VBLF + "                     ELSE '02.퇴원'                                                                        ";
                SQL = SQL + ComNum.VBLF + "                END AS GBSTS, IPDNO                                                                        ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_NEW_MASTER M                                                                         ";

                switch (cboWard.Text.Trim())
                {
                    case "전체":
                        SQL = SQL + ComNum.VBLF + " WHERE M.WardCode>' ' ";
                        break;
                    case "MICU":
                        SQL = SQL + ComNum.VBLF + " WHERE M.RoomCode='234' ";
                        break;
                    case "SICU":
                        SQL = SQL + ComNum.VBLF + " WHERE M.RoomCode='233' ";
                        break;
                    case "ND":
                    case "NR":
                        SQL = SQL + ComNum.VBLF + " WHERE M.WardCode IN ( 'ND','IQ' ,'NR') ";
                        break;
                    case "HD":
                        SQL = SQL + ComNum.VBLF + " WHERE M.PANO IN ( SELECT PANO FROM  TONG_HD_DAILY WHERE TDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD') )";
                        break;
                    case "OP":
                        SQL = SQL + ComNum.VBLF + " WHERE M.PANO IN ( SELECT PANO FROM KOSMOS_PMPA.ORAN_MASTER WHERE OPDATE =TO_DATE('" + strToDate + "','YYYY-MM-DD') ) ";
                        break;
                    case "ENDO":
                        SQL = SQL + ComNum.VBLF + " WHERE M.PANO IN ( SELECT PTNO FROM KOSMOS_OCS.ENDO_JUPMST WHERE TRUNC(RDATE) =TO_DATE('" + strToDate + "','YYYY-MM-DD') ) ";
                        break;
                    case "ER":
                        SQL = SQL + ComNum.VBLF + " WHERE M.PANO IN ( SELECT PANO FROM KOSMOS_PMPA.OPD_MASTER  WHERE BDATE =TO_DATE('" + strToDate + "','YYYY-MM-DD') AND DEPTCODE ='ER' ) ";
                        break;
                    case "RA":
                        SQL = SQL + ComNum.VBLF + " WHERE  M.PANO IN ( SELECT PTNO   FROM KOSMOS_OCS.OCS_ITRANSFER  WHERE TODRCODE ='1107' AND GBDEL <>'*'  AND TRUNC(EDATE) =TO_DATE('" + strToDate + "','YYYY-MM-DD' ))  ";
                        break;
                    default:
                        SQL = SQL + ComNum.VBLF + " WHERE M.WardCode='" + cboWard.Text.Trim() + "' ";
                        break;
                }

                if (VB.Left(cboDept.Text, 2) != "**")
                {
                    SQL = SQL + ComNum.VBLF + "  AND M.DEPTCODE = '" + VB.Left(cboDept.Text, 2) + "' ";
                }
                if (VB.Left(cboDrCode.Text, 4) != "****")
                {
                    SQL = SQL + ComNum.VBLF + "  AND M.DRCODE = '" + VB.Left(cboDrCode.Text, 4) + "' ";
                }

                SQL = SQL + ComNum.VBLF + " AND EXISTS (                                                            ";
                SQL = SQL + ComNum.VBLF + "             SELECT 1                                                    ";
                SQL = SQL + ComNum.VBLF + "                 FROM KOSMOS_PMPA.IPD_NEW_SLIP  SUB                      ";
                SQL = SQL + ComNum.VBLF + "             WHERE(PANO, BDATE, ORDERNO) IN                              ";
                SQL = SQL + ComNum.VBLF + "                 (                                                       ";
                SQL = SQL + ComNum.VBLF + "                 SELECT PTNO, BDATE, ORDERNO                             ";
                SQL = SQL + ComNum.VBLF + "                     FROM KOSMOS_OCS.OCS_IORDER                          ";
                SQL = SQL + ComNum.VBLF + "                 WHERE GbStatus IN(' ', 'D', 'D+')                       ";
                SQL = SQL + ComNum.VBLF + "                     AND BDate >= TO_DATE('" + dtpSDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "                     AND BDate <= TO_DATE('" + dtpEDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "                     AND NurseID <> ' '                                  ";
                SQL = SQL + ComNum.VBLF + "                     AND GbVerb IN('Y', 'C')                             ";
                SQL = SQL + ComNum.VBLF + "                     AND(GbSend = ' ' OR GbSend IS NULL)                 ";
                SQL = SQL + ComNum.VBLF + "                     AND OrderSite Not Like 'DC%'                        ";
                SQL = SQL + ComNum.VBLF + "                     AND OrderSite <> 'CAN'                              ";
                SQL = SQL + ComNum.VBLF + "                     AND(GBIOE NOT IN('E', 'EI') OR GBIOE IS NULL)       ";
                SQL = SQL + ComNum.VBLF + "                     AND Bun IN('11', '12', '20')                        ";
                SQL = SQL + ComNum.VBLF + "                 )                                                       ";
                SQL = SQL + ComNum.VBLF + "             AND M.IPDNO = SUB.IPDNO                                     ";
                SQL = SQL + ComNum.VBLF + "             )                                                           ";
                //SQL = SQL + ComNum.VBLF + " ORDER BY GBSTS, ROOMCODE, SNAME                                         ";
                SQL = SQL + ComNum.VBLF + " ORDER BY INDATE, OUTDATE, SNAME                                         ";

                //SQL = SQL + ComNum.VBLF + " AND ((JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND M.OutDate IS NULL) ";
                //SQL = SQL + ComNum.VBLF + " OR M.OutDate >= TO_DATE('" + strSysDate + "','YYYY-MM-DD')) ";
                //SQL = SQL + ComNum.VBLF + " AND M.IpwonTime < TO_DATE('" + cf.DATE_ADD(clsDB.DbCon, strSysDate, 1) + "','YYYY-MM-DD') ";
                //SQL = SQL + ComNum.VBLF + " AND M.Pano < '90000000' ";
                //SQL = SQL + ComNum.VBLF + " AND M.GbSTS <> '9' ";

                //if (cboTeam.Text != "전체")
                //{
                //    SQL = SQL + ComNum.VBLF + "  AND EXISTS ";
                //    SQL = SQL + ComNum.VBLF + " (SELECT * FROM " + ComNum.DB_PMPA + "NUR_TEAM_ROOMCODE T";
                //    SQL = SQL + ComNum.VBLF + "          WHERE M.WARDCODE = T.WARDCODE";
                //    SQL = SQL + ComNum.VBLF + "             AND M.ROOMCODE = T.ROOMCODE";
                //    SQL = SQL + ComNum.VBLF + "             AND T.TEAM = '" + cboTeam.Text.Trim() + "')";
                //}

                //SQL = SQL + ComNum.VBLF + " AND EXISTS (";
                //SQL = SQL + ComNum.VBLF + "  SELECT ORDERNO ";
                //SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_MED + "OCS_IORDER SUB";
                //SQL = SQL + ComNum.VBLF + " WHERE GbStatus IN (' ','D','D+')";
                //SQL = SQL + ComNum.VBLF + "   AND BDate >= TO_DATE('" + dtpSDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                //SQL = SQL + ComNum.VBLF + "   AND BDate <= TO_DATE('" + dtpEDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                //SQL = SQL + ComNum.VBLF + "   AND NurseID <> ' '";
                //SQL = SQL + ComNum.VBLF + "   AND GbVerb IN ('Y','C')";
                //SQL = SQL + ComNum.VBLF + "   AND (GbSend  = ' ' OR GbSend IS NULL)";
                //SQL = SQL + ComNum.VBLF + "   AND OrderSite Not Like 'DC%'";
                //SQL = SQL + ComNum.VBLF + "   AND OrderSite <>  'CAN'";
                //SQL = SQL + ComNum.VBLF + "   AND (GBIOE NOT IN ('E','EI') OR GBIOE IS NULL)";
                //SQL = SQL + ComNum.VBLF + "   AND Bun IN ('11','12','20')";
                //SQL = SQL + ComNum.VBLF + "   AND SUB.PTNO = M.PANO) ";
                //SQL = SQL + ComNum.VBLF + "ORDER BY M.RoomCode,M.SName, M.Indate DESC  ";

                //if (cboWard.Text.Trim() == "ER")
                //{
                //    SQL = "  SELECT 'ER' WardCode, '100' RoomCode, M.Pano,M.SName,M.Sex,M.Age, '0' GbSts, M.REP, ";
                //    SQL = SQL + ComNum.VBLF + " M.DeptCode,M.DrCode,D.DrName, M.ACTDATE INDATE, '' OUTDATE, E.ROWID, E.ACT_OK, '' GBDRG";
                //    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_MASTER  M, ";
                //    SQL = SQL + ComNum.VBLF + "       " + ComNum.DB_PMPA + "BAS_PATIENT P, ";
                //    SQL = SQL + ComNum.VBLF + "       " + ComNum.DB_PMPA + "BAS_DOCTOR  D, ";
                //    SQL = SQL + ComNum.VBLF + "       " + ComNum.DB_PMPA + "NUR_ER_PATIENT E";
                //    SQL = SQL + ComNum.VBLF + " WHERE M.BDate >= TO_DATE('" + dtpSDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                //    SQL = SQL + ComNum.VBLF + "   AND M.BDate <= TO_DATE('" + dtpEDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                //    SQL = SQL + ComNum.VBLF + "   AND M.DEPTCODE ='ER'  ";
                //    SQL = SQL + ComNum.VBLF + "   AND M.Pano=P.Pano(+) ";
                //    SQL = SQL + ComNum.VBLF + "   AND M.DrCode=D.DrCode(+) ";
                //    SQL = SQL + ComNum.VBLF + "   AND M.PANO = E.PANO ";
                //    SQL = SQL + ComNum.VBLF + "   AND M.BDATE = E.JDATE ";
                //    SQL = SQL + ComNum.VBLF + " AND EXISTS (";
                //    SQL = SQL + ComNum.VBLF + "  SELECT ORDERNO ";
                //    SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_MED + "OCS_IORDER SUB";
                //    SQL = SQL + ComNum.VBLF + " WHERE GbStatus IN (' ','D','D+')";
                //    SQL = SQL + ComNum.VBLF + "   AND BDate >= TO_DATE('" + dtpSDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                //    SQL = SQL + ComNum.VBLF + "   AND BDate <= TO_DATE('" + dtpEDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                //    SQL = SQL + ComNum.VBLF + "   AND NurseID <> ' '";
                //    SQL = SQL + ComNum.VBLF + "   AND GbVerb IN ('Y','C')";
                //    SQL = SQL + ComNum.VBLF + "   AND (GbSend  = ' ' OR GbSend IS NULL)";
                //    SQL = SQL + ComNum.VBLF + "   AND OrderSite Not Like 'DC%'";
                //    SQL = SQL + ComNum.VBLF + "   AND OrderSite <>  'CAN'";
                //    SQL = SQL + ComNum.VBLF + "   AND GBIOE IN ('E','EI')";
                //    SQL = SQL + ComNum.VBLF + "   AND Bun IN ('11','12','20')";
                //    SQL = SQL + ComNum.VBLF + "   AND SUB.PTNO = M.PANO)";
                //    SQL = SQL + ComNum.VBLF + "ORDER BY M.SName ";
                //}

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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

                ssList_Sheet1.RowCount = dt.Rows.Count;
                ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (strRoom_Old != dt.Rows[i]["ROOMCODE"].ToString().Trim())
                    {
                        
                        strRoom_Old = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                    }

                    strPano = dt.Rows[i]["PANO"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["GBSTS"].ToString().Trim().Split('.')[1];
                    ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["OUTDATE"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 4].Text = strPano;
                    ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();                    
                }

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

            //if (chkAll.Checked == true)
            //{
            //    Read_Verbal_Order("", dtpSDate.Value.ToString("yyyy-MM-dd"), dtpEDate.Value.ToString("yyyy-MM-dd"), cboWard.Text.Trim());
            //    Read_ConfirmedVerbal("", dtpSDate.Value.ToString("yyyy-MM-dd"), dtpEDate.Value.ToString("yyyy-MM-dd"), cboWard.Text.Trim());
            //}
        }

        bool Read_Verbal_NotConfirmed(string argPano, string argSDate, string argEDate)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            bool rtnVar = false;

            try
            {
                SQL = "";
                SQL = " SELECT A.PTNO";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_IORDER A     ";
                SQL = SQL + ComNum.VBLF + "WHERE Ptno     = '" + argPano + "' ";
                SQL = SQL + ComNum.VBLF + "  AND GbStatus IN (' ','D','D+')  ";
                SQL = SQL + ComNum.VBLF + "  AND BDate >= TO_DATE('" + argSDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND BDate <= TO_DATE('" + argEDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND NurseID <> ' ' ";   //간호사처방만
                SQL = SQL + ComNum.VBLF + "  AND GbVerb ='Y' ";   //구두처방대상건만
                SQL = SQL + ComNum.VBLF + "  AND (GbSend  = ' ' OR GbSend IS NULL) ";   //전송된것
                SQL = SQL + ComNum.VBLF + "  AND OrderSite Not Like 'DC%' ";
                SQL = SQL + ComNum.VBLF + "  AND OrderSite <>  'CAN' ";
                SQL = SQL + ComNum.VBLF + "  AND Bun IN ('11','12','20') ";
                SQL = SQL + ComNum.VBLF + "  AND DRORDERVIEW IS NULL ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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

                rtnVar = true;

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
        
        private void cboDept_Leave(object sender, EventArgs e)
        {
            if (VB.Left(cboDept.Text, 2) == "**")
            {
                cboDept.Items.Clear();
                cboDept.Items.Add("****.전체");
            }
            else
            {
                clsVbfunc.SetDrCodeCombo(clsDB.DbCon, cboDrCode, VB.Left(cboDept.Text, 2), "1", 1, "");
            }
        }

        void Set_cboWard()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = "SELECT WardCode, WardName  ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_WARD ";
                SQL = SQL + ComNum.VBLF + " WHERE WARDCODE NOT IN ('IU','NP','2W','IQ','ER') ";
                SQL = SQL + ComNum.VBLF + "     AND USED = 'Y'  ";
                SQL = SQL + ComNum.VBLF + " ORDER BY WardCode ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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
                    return;
                }

                cboWard.Items.Clear();
                cboWard.Items.Add("전체");
                cboWard2.Items.Clear();
                cboWard2.Items.Add("전체");

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboWard.Items.Add(dt.Rows[i]["WARDCODE"].ToString().Trim());
                    cboWard2.Items.Add(dt.Rows[i]["WARDCODE"].ToString().Trim());
                }

                cboWard.Items.Add("HD");
                cboWard.Items.Add("ER");
                cboWard2.Items.Add("HD");
                cboWard2.Items.Add("ER");

                dt.Dispose();
                dt = null;

                cboWard.SelectedIndex = 0;
                cboWard2.SelectedIndex = 0;

                //if (gsWard != "")
                //{
                //    cboWard.SelectedIndex = cboWard.Items.IndexOf(gsWard);
                //    cboWard.Enabled = false;
                //}
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssList_Sheet1.RowCount == 0)
            {
                return;
            }

            string strPano = "";

            strPano = ssList_Sheet1.Cells[e.Row, 4].Text.Trim();

            //Read_Verbal_Order(strPano, dtpSDate.Value.ToString("yyyy-MM-dd"), dtpEDate.Value.ToString("yyyy-MM-dd"));
            Read_ConfirmedVerbal(strPano, dtpSDate.Value.ToString("yyyy-MM-dd"), dtpEDate.Value.ToString("yyyy-MM-dd"));            
        }

        void Read_ConfirmedVerbal(string argPano, string argSDate, string argEDate, string argWard = "")
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strPano = "";
            string strBDate = "";
            string strOrderCode = "";
            string strOrderNo = "";

            string strDate1 = "";
            string strDate2 = "";

            ssView2_Sheet1.Columns[1].Visible = false;
            ssView2_Sheet1.Columns[2].Visible = false;

            if (argWard != "")
            {
                ssView2_Sheet1.Columns[1].Visible = true;
                ssView2_Sheet1.Columns[2].Visible = true;

                if (ssList_Sheet1.RowCount == 0)
                {
                    return;
                }

                strPano = "";

                for (i = 0; i < ssList_Sheet1.RowCount; i++)
                {
                    strPano += ssList_Sheet1.Cells[i, 1].Text.Trim() + "','";
                }

                if (strPano != "")
                {
                    strPano = VB.Mid("'" + strPano, 1, strPano.Length - 1);
                }
            }

            ssView2_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "  SELECT A.DRCODE, A.PTNO, A.BDATE, BUN, ORDERCODE, DOSCODE,";
                SQL = SQL + ComNum.VBLF + "  CONTENTS, REALQTY, GBDIV, NAL,";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(A.ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE, NURSEID, V_ORDERNO, A.ORDERNO,";
                SQL = SQL + ComNum.VBLF + "  SLIPNO, SUCODE, GBVERB, TO_CHAR(B.WRITEDATE,'YYYY-MM-DD HH24:MI') WDATE, B.WRITESABUN, ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(A.DRORDERVIEW,'YYYY-MM-DD HH24:MI') DRORDERVIEW";
                SQL = SQL + ComNum.VBLF + "    From " + ComNum.DB_MED + "OCS_IORDER A, " + ComNum.DB_PMPA + "NUR_VERBAL_ORDER_CONFIRM B";

                if (argWard != "")
                {
                    SQL = SQL + ComNum.VBLF + " WHERE A.PTNO IN (" + strPano + ")";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " WHERE A.PTNO = '" + argPano + "'";
                }

                SQL = SQL + ComNum.VBLF + "   AND A.GbStatus IN (' ','D','D+')";
                SQL = SQL + ComNum.VBLF + "    AND A.BDate >= TO_DATE('" + argSDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND A.BDate <= TO_DATE('" + argEDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND A.NurseID <> ' '";
                SQL = SQL + ComNum.VBLF + "   AND A.GbVerb IN ('Y','C')";
                SQL = SQL + ComNum.VBLF + "   AND (A.GbSend  = ' ' OR A.GbSend IS NULL)";
                SQL = SQL + ComNum.VBLF + "   AND A.OrderSite Not Like 'DC%'";
                SQL = SQL + ComNum.VBLF + "   AND A.OrderSite <>  'CAN'";
                SQL = SQL + ComNum.VBLF + "   AND A.Bun IN ('11','12','20')";
                SQL = SQL + ComNum.VBLF + "   AND A.PTNO = B.PTNO(+)";

                if (cboWard.Text == "ER")
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.GBIOE IN ('E','EI')";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND (A.GBIOE NOT IN ('E','EI') OR A.GBIOE IS NULL)";
                }

                SQL = SQL + ComNum.VBLF + "    AND A.ORDERNO = B.ORDERNO(+)";
                SQL = SQL + ComNum.VBLF + "    AND A.ENTDATE = B.ENTDATE(+)";

                if (argWard != "")
                {
                    SQL = SQL + ComNum.VBLF + "    ORDER BY A.PTNO, A.BDATE, A.BUN, A.ENTDATE";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    ORDER BY A.BDATE, A.BUN, A.ENTDATE";
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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

                ssView2_Sheet1.RowCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strDate1 = dt.Rows[i]["ENTDATE"].ToString().Trim();
                    strDate2 = dt.Rows[i]["DRORDERVIEW"].ToString().Trim();

                    ssView2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["WDATE"].ToString().Trim() + ComNum.VBLF + "(" + clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["WRITESABUN"].ToString().Trim()) + ")";
                    ssView2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                    ssView2_Sheet1.Cells[i, 2].Text = clsVbfunc.GetPatientName(clsDB.DbCon, dt.Rows[i]["PTNO"].ToString().Trim());
                    ssView2_Sheet1.Cells[i, 3].Text = clsVbfunc.GetOCSDoctorName(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString().Trim());
                    strBDate = dt.Rows[i]["BDATE"].ToString().Trim();
                    ssView2_Sheet1.Cells[i, 6].Text = VB.Left(strBDate, 10);
                    ssView2_Sheet1.Cells[i, 7].Text = Read_Bun(dt.Rows[i]["BUN"].ToString().Trim());
                    strOrderCode = dt.Rows[i]["ORDERCODE"].ToString().Trim();
                    ssView2_Sheet1.Cells[i, 8].Text = Read_OrderName(strOrderCode, dt.Rows[i]["SLIPNO"].ToString().Trim());
                    ssView2_Sheet1.Cells[i, 9].Text = Read_DOSName(dt.Rows[i]["DOSCODE"].ToString().Trim());
                    ssView2_Sheet1.Cells[i, 10].Text = dt.Rows[i]["CONTENTS"].ToString().Trim();

                    if (ssView2_Sheet1.Cells[i, 10].Text == "0")
                    {
                        ssView2_Sheet1.Cells[i, 10].Text = "";
                    }

                    ssView2_Sheet1.Cells[i, 11].Text = dt.Rows[i]["REALQTY"].ToString().Trim();
                    ssView2_Sheet1.Cells[i, 12].Text = dt.Rows[i]["GBDIV"].ToString().Trim();
                    ssView2_Sheet1.Cells[i, 13].Text = dt.Rows[i]["NAL"].ToString().Trim();
                    ssView2_Sheet1.Cells[i, 14].Text = dt.Rows[i]["ENTDATE"].ToString().Trim() + ComNum.VBLF + "(" + clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["NURSEID"].ToString().Trim()) + ")";
                    strOrderNo = dt.Rows[i]["ORDERNO"].ToString().Trim();
                    strPano = argPano;
                    ssView2_Sheet1.Cells[i, 15].Text = dt.Rows[i]["DRORDERVIEW"].ToString().Trim();
                    ssView2_Sheet1.Cells[i, 16].Text = "";
                    ssView2_Sheet1.Cells[i, 17].Text = "";
                    ssView2_Sheet1.Cells[i, 18].Text = strOrderNo;

                    if (Read_Suga_Hang(dt.Rows[i]["SUCODE"].ToString().Trim()) == "OK")
                    {
                        ssView2_Sheet1.Cells[i, 8].Text = "★항혈전 " + ssView2_Sheet1.Cells[i, 8].Text;
                        ssView2_Sheet1.Cells[i, 8].ForeColor = Color.FromArgb(255, 0, 255);
                    }

                    if (Read_Suga_Component(dt.Rows[i]["SUCODE"].ToString().Trim()) == "OK")
                    {
                        ssView2_Sheet1.Cells[i, 8].Text = "<!> " + ssView2_Sheet1.Cells[i, 8].Text;
                    }

                    switch (dt.Rows[i]["GBVERB"].ToString().Trim())
                    {
                        case "Y":
                            ssView2_Sheet1.Cells[i, 8].ForeColor = Color.FromArgb(240, 20, 20);
                            break;
                        case "C":
                            break;
                    }

                    ssView2_Sheet1.SetRowHeight(i, Convert.ToInt32(ssView2_Sheet1.GetPreferredRowHeight(i)) + 2);

                    ssView2_Sheet1.Cells[i, 20].Text = Get_Time(strDate1, strDate2);
                    if (VB.Val(ssView2_Sheet1.Cells[i, 20].Text) >= 24)
                    {
                        ssView2_Sheet1.Cells[i, 20].BackColor = Color.Red;
                    }
                }


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

        string Read_Suga_Hang(string argSuCode)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string rtnVar = "";

            try
            {
                SQL = "";
                SQL = " SELECT JEPCODE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_ERP + "DRUG_SETCODE ";
                SQL = SQL + ComNum.VBLF + "  WHERE GUBUN = '13'  ";
                SQL = SQL + ComNum.VBLF + "   AND (DELDATE IS NULL or DelDate ='')";
                SQL = SQL + ComNum.VBLF + "   AND JepCode ='" + argSuCode.Trim() + "'  ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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

                rtnVar = "OK";

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

        string Read_Suga_Component(string argSuCode)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string rtnVar = "";

            try
            {
                SQL = "";
                SQL = " SELECT COUNT(GRPNAME) CNT ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_DRUGINFO_COMPONENT ";
                SQL = SQL + ComNum.VBLF + "  WHERE TRIM(GRPNAME) in (SELECT TRIM(GRPNAME) FROM KOSMOS_OCS.OCS_DRUGINFO_COMPONENT WHERE TRIM(SUNEXT) ='" + argSuCode + "') ";
                SQL = SQL + ComNum.VBLF + " HAVING COUNT(GRPNAME) > 0";  //2건이상인것  - 일단풀어줌 2012-12-12
                SQL = SQL + ComNum.VBLF + "";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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

                rtnVar = "OK";

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

        string Get_Time(string argDate1, string argDate2)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strSysDateTime = "";
            string rtnVar = "";

            strSysDateTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A");

            try
            {
                SQL = "";
                SQL = "SELECT TRUNC((TO_DATE('" + argDate2 + "','YYYY-MM-DD HH24:MI') - TO_DATE('" + argDate1 + "','YYYY-MM-DD HH24:MI'))*24) result";
                SQL = SQL + ComNum.VBLF + "  FROM DUAL";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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

                rtnVar = dt.Rows[0]["RESULT"].ToString().Trim();

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

        private void btnExcel_Click(object sender, EventArgs e)
        {
            using (clsSpread CS = new clsSpread())
            {
                if (tabControl1.SelectedIndex == 0)
                {
                    if (ssView2_Sheet1.RowCount == 0) return;
                    CS.ExportToXLS(ssView2);
                }
                else
                {
                    if (ssView3_Sheet1.RowCount == 0) return;
                    CS.ExportToXLS(ssView3);
                }
            }
        }

        private void btnSearch2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            Search_Data2();
        }

        private void Search_Data2()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strBDate = "";
            string strOrderCode = "";
            string strOrderNo = "";

            string strToDate = "";

            string strDate1 = "";
            string strDate2 = "";
            
            ssView3_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT DISTINCT ";
                SQL = SQL + ComNum.VBLF + "        D.JDATE, TO_CHAR(D.INDATE, 'YYYY-MM-DD') INDATE, TO_CHAR(D.OUTDATE, 'YYYY-MM-DD') OUTDATE, ";
                SQL = SQL + ComNum.VBLF + "        D.ROOMCODE, D.PANO, D.SNAME, D.DEPTCODE,                                                   ";
                SQL = SQL + ComNum.VBLF + "        CASE WHEN D.GBSTS = 0 THEN '재원'                                                           ";
                SQL = SQL + ComNum.VBLF + "                WHEN D.GBSTS = 9 THEN '취소'                                                        ";
                SQL = SQL + ComNum.VBLF + "                ELSE '퇴원'                                                                         ";
                SQL = SQL + ComNum.VBLF + "        END AS GBSTS, D.IPDNO,                                                                     ";
                SQL = SQL + ComNum.VBLF + "        A.DRCODE,                                                                                  ";
                SQL = SQL + ComNum.VBLF + "        (SELECT DRNAME FROM KOSMOS_OCS.OCS_DOCTOR                                                  ";
                SQL = SQL + ComNum.VBLF + "            WHERE SABUN = A.DRCODE                                                                 ";
                SQL = SQL + ComNum.VBLF + "        ) AS DRNAME,                                                                               ";
                SQL = SQL + ComNum.VBLF + "        A.PTNO, A.BDATE, A.BUN, A.ORDERCODE, A.DOSCODE,                                            ";
                SQL = SQL + ComNum.VBLF + "        (SELECT DOSNAME                                                                            ";
                SQL = SQL + ComNum.VBLF + "            FROM KOSMOS_OCS.OCS_ODOSAGE                                                            ";
                SQL = SQL + ComNum.VBLF + "            WHERE DOSCODE = A.DOSCODE                                                              ";
                SQL = SQL + ComNum.VBLF + "        ) AS DOSNAME,                                                                              ";
                SQL = SQL + ComNum.VBLF + "        A.CONTENTS, A.REALQTY, A.GBDIV, A.NAL,                                                     ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(A.ENTDATE, 'YYYY-MM-DD HH24:MI') ENTDATE, A.NURSEID, A.V_ORDERNO, A.ORDERNO,       ";
                SQL = SQL + ComNum.VBLF + "        A.SLIPNO, A.SUCODE, A.GBVERB, TO_CHAR(B.WRITEDATE, 'YYYY-MM-DD HH24:MI') WDATE,            ";
                SQL = SQL + ComNum.VBLF + "        B.WRITESABUN,                                                                              ";
                SQL = SQL + ComNum.VBLF + "        (SELECT KORNAME FROM KOSMOS_ADM.INSA_MST                                                   ";
                SQL = SQL + ComNum.VBLF + "            WHERE SABUN = B.WRITESABUN                                                             ";
                SQL = SQL + ComNum.VBLF + "                AND(TOIDAY IS NULL OR TOIDAY < TRUNC(SYSDATE))                                     ";
                SQL = SQL + ComNum.VBLF + "        ) AS WRITENAME,                                                                            ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(A.DRORDERVIEW, 'YYYY-MM-DD HH24:MI') DRORDERVIEW                                   ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_IORDER A                                                                     ";                
                SQL = SQL + ComNum.VBLF + " LEFT OUTER JOIN KOSMOS_PMPA.NUR_VERBAL_ORDER_CONFIRM B                                              ";
                SQL = SQL + ComNum.VBLF + "    ON A.PTNO = B.PTNO                                                                               ";
                SQL = SQL + ComNum.VBLF + "   AND A.ORDERNO = B.ORDERNO                                                                         ";
                SQL = SQL + ComNum.VBLF + "   AND A.ENTDATE = B.ENTDATE                                                                         ";
     
                #region 입원 정보 조건
                SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_PMPA.IPD_NEW_MASTER D                                                             ";
                SQL = SQL + ComNum.VBLF + "    ON A.PTNO = D.PANO";
                SQL = SQL + ComNum.VBLF + "   AND EXISTS (                                                            ";
                SQL = SQL + ComNum.VBLF + "             SELECT 1                                                    ";
                SQL = SQL + ComNum.VBLF + "                 FROM KOSMOS_PMPA.IPD_NEW_SLIP  SUB                      ";
                SQL = SQL + ComNum.VBLF + "             WHERE SUB.BDATE >= A.BDATE                              ";
                SQL = SQL + ComNum.VBLF + "               AND SUB.BDATE <= A.BDATE                    ";
                SQL = SQL + ComNum.VBLF + "               AND D.IPDNO  = SUB.IPDNO                    ";
                SQL = SQL + ComNum.VBLF + "             )                                                           ";


                switch (cboWard2.Text.Trim())
                {
                    case "전체":
                        SQL = SQL + ComNum.VBLF + "   AND D.WardCode>' ' ";
                        break;
                    case "MICU":
                        SQL = SQL + ComNum.VBLF + "   AND D.RoomCode='234' ";
                        break;
                    case "SICU":
                        SQL = SQL + ComNum.VBLF + "   AND D.RoomCode='233' ";
                        break;
                    case "ND":
                    case "NR":
                        SQL = SQL + ComNum.VBLF + "   AND D.WardCode IN ( 'ND','IQ' ,'NR') ";
                        break;
                    case "HD":
                        SQL = SQL + ComNum.VBLF + "   AND D.PANO IN ( SELECT PANO FROM  TONG_HD_DAILY WHERE TDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD') )";
                        break;
                    case "OP":
                        SQL = SQL + ComNum.VBLF + "   AND D.PANO IN ( SELECT PANO FROM KOSMOS_PMPA.ORAN_MASTER WHERE OPDATE =TO_DATE('" + strToDate + "','YYYY-MM-DD') ) ";
                        break;
                    case "ENDO":
                        SQL = SQL + ComNum.VBLF + "   AND D.PANO IN ( SELECT PTNO FROM KOSMOS_OCS.ENDO_JUPMST WHERE TRUNC(RDATE) =TO_DATE('" + strToDate + "','YYYY-MM-DD') ) ";
                        break;
                    case "ER":
                        SQL = SQL + ComNum.VBLF + "   AND D.PANO IN ( SELECT PANO FROM KOSMOS_PMPA.OPD_MASTER  WHERE BDATE =TO_DATE('" + strToDate + "','YYYY-MM-DD') AND DEPTCODE ='ER' ) ";
                        break;
                    case "RA":
                        SQL = SQL + ComNum.VBLF + "   AND D.PANO IN ( SELECT PTNO   FROM KOSMOS_OCS.OCS_ITRANSFER  WHERE TODRCODE ='1107' AND GBDEL <>'*'  AND TRUNC(EDATE) =TO_DATE('" + strToDate + "','YYYY-MM-DD' ))  ";
                        break;
                    default:
                        SQL = SQL + ComNum.VBLF + "   AND D.WardCode='" + cboWard.Text.Trim() + "' ";
                        break;
                }

                if (VB.Left(cboDept.Text, 2) != "**")
                {
                    SQL = SQL + ComNum.VBLF + "   AND D.DEPTCODE = '" + VB.Left(cboDept.Text, 2) + "' ";
                }
                if (VB.Left(cboDrCode.Text, 4) != "****")
                {
                    SQL = SQL + ComNum.VBLF + "   AND D.DRCODE = '" + VB.Left(cboDrCode.Text, 4) + "' ";
                }
                #endregion

                SQL = SQL + ComNum.VBLF + " WHERE A.GbStatus IN (' ', 'D', 'D+')                                                                ";
                SQL = SQL + ComNum.VBLF + "   AND A.BDate >= TO_DATE('" + dtpSDate2.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND A.BDate <= TO_DATE('" + dtpEDate2.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND A.NurseID <> ' '                                                                              ";
                SQL = SQL + ComNum.VBLF + "   AND A.GbVerb IN ('Y', 'C')                                                                        ";
                SQL = SQL + ComNum.VBLF + "   AND(A.GbSend = ' ' OR A.GbSend IS NULL)                                                           ";
                SQL = SQL + ComNum.VBLF + "   AND A.OrderSite Not Like 'DC%'                                                                    ";
                SQL = SQL + ComNum.VBLF + "   AND A.OrderSite <> 'CAN'                                                                          ";
                SQL = SQL + ComNum.VBLF + "   AND A.Bun IN ('11', '12', '20')                                                                   ";
                if (cboWard2.Text == "ER")
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.GBIOE IN ('E','EI')";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND (A.GBIOE NOT IN ('E','EI') OR A.GBIOE IS NULL)";
                }

             


                SQL = SQL + ComNum.VBLF + " ORDER BY INDATE, OUTDATE, SNAME, BDATE                                                              ";
                
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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

                ssView3_Sheet1.RowCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView3_Sheet1.Cells[i, 0].Text = dt.Rows[i]["GBSTS"].ToString().Trim();
                    ssView3_Sheet1.Cells[i, 1].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                    ssView3_Sheet1.Cells[i, 2].Text = dt.Rows[i]["OUTDATE"].ToString().Trim();
                    ssView3_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                    ssView3_Sheet1.Cells[i, 4].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssView3_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssView3_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    
                    strDate1 = dt.Rows[i]["ENTDATE"].ToString().Trim();
                    strDate2 = dt.Rows[i]["DRORDERVIEW"].ToString().Trim();

                    ssView3_Sheet1.Cells[i, 7].Text = dt.Rows[i]["WDATE"].ToString().Trim() + ComNum.VBLF + "(" + clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["WRITESABUN"].ToString().Trim()) + ")";

                    //ssView3_Sheet1.Cells[i, 8].Text = clsVbfunc.GetOCSDoctorName(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString().Trim());
                    ssView3_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DRNAME"].ToString().Trim();

                    strBDate = dt.Rows[i]["BDATE"].ToString().Trim();
                    ssView3_Sheet1.Cells[i, 9].Text = VB.Left(strBDate, 10);
                    ssView3_Sheet1.Cells[i, 10].Text = Read_Bun(dt.Rows[i]["BUN"].ToString().Trim());
                    strOrderCode = dt.Rows[i]["ORDERCODE"].ToString().Trim();
                    ssView3_Sheet1.Cells[i, 11].Text = Read_OrderName(strOrderCode, dt.Rows[i]["SLIPNO"].ToString().Trim());
                    ssView3_Sheet1.Cells[i, 12].Text = Read_DOSName(dt.Rows[i]["DOSCODE"].ToString().Trim());
                    ssView3_Sheet1.Cells[i, 13].Text = dt.Rows[i]["CONTENTS"].ToString().Trim();

                    if (ssView3_Sheet1.Cells[i, 13].Text == "0")
                    {
                        ssView3_Sheet1.Cells[i, 13].Text = "";
                    }

                    ssView3_Sheet1.Cells[i, 14].Text = dt.Rows[i]["REALQTY"].ToString().Trim();
                    ssView3_Sheet1.Cells[i, 15].Text = dt.Rows[i]["GBDIV"].ToString().Trim();
                    ssView3_Sheet1.Cells[i, 16].Text = dt.Rows[i]["NAL"].ToString().Trim();
                    ssView3_Sheet1.Cells[i, 17].Text = dt.Rows[i]["ENTDATE"].ToString().Trim() + ComNum.VBLF + "(" + clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["NURSEID"].ToString().Trim()) + ")";
                    strOrderNo = dt.Rows[i]["ORDERNO"].ToString().Trim();                    
                    ssView3_Sheet1.Cells[i, 18].Text = dt.Rows[i]["DRORDERVIEW"].ToString().Trim();
                    
                    if (Read_Suga_Hang(dt.Rows[i]["SUCODE"].ToString().Trim()) == "OK")
                    {
                        ssView3_Sheet1.Cells[i, 11].Text = "★항혈전 " + ssView3_Sheet1.Cells[i, 8].Text;
                        ssView3_Sheet1.Cells[i, 11].ForeColor = Color.FromArgb(255, 0, 255);
                    }

                    if (Read_Suga_Component(dt.Rows[i]["SUCODE"].ToString().Trim()) == "OK")
                    {
                        ssView3_Sheet1.Cells[i, 11].Text = "<!> " + ssView3_Sheet1.Cells[i, 8].Text;
                    }

                    switch (dt.Rows[i]["GBVERB"].ToString().Trim())
                    {
                        case "Y":
                            ssView3_Sheet1.Cells[i, 11].ForeColor = Color.FromArgb(240, 20, 20);
                            break;
                        case "C":
                            break;
                    }

                    ssView3_Sheet1.SetRowHeight(i, Convert.ToInt32(ssView3_Sheet1.GetPreferredRowHeight(i)) + 2);

                    ssView3_Sheet1.Cells[i, 19].Text = Get_Time(strDate1, strDate2);
                    if (VB.Val(ssView3_Sheet1.Cells[i, 19].Text) >= 24)
                    {
                        ssView3_Sheet1.Cells[i, 19].BackColor = Color.Red;
                    }
                }


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
    }
}
