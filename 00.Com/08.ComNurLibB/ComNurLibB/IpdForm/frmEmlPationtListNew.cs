using ComBase; //기본 클래스
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;


namespace ComNurLibB
{

    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : 
    /// Description     : 
    /// Author          : 김현욱
    /// Create Date     : 2018-12-13
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= d:\psmh\Ocs\ipdocs\nurview\Nview16.frm" >> frmChulgo.cs 폼이름 재정의" />
    /// 

    public partial class frmEmlPationtListNew : Form
    {
        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
        //string FstrREAD = "";

        public frmEmlPationtListNew()
        {
            InitializeComponent();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int i = 0;
            string[] colHeader = null;


            colHeader = new string[SS1_Sheet1.ColumnCount];

            for (i = 0; i < SS1_Sheet1.ColumnCount; i++)
            {
                colHeader[i] = SS1_Sheet1.ColumnHeader.Cells[0, i].Text.Trim();
            }

            SS1_Sheet1.AddRows(0, 1);

            for (i = 0; i < SS1_Sheet1.ColumnCount; i++)
            {
                SS1_Sheet1.Cells[0, i].Text = colHeader[i];
            }

            clsSpread CS = new clsSpread();
            CS.ExportToXLS(SS1);
            CS = null;
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string READ_경로(string ArgPano, string ArgInDate, string ArgOutDate)
        {
            string strRtn = "";
            //int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";

                SQL = "SELECT  EXTRACTVALUE(CHARTXML, '//it7')   CHARTXML ";
                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML ";
                SQL += ComNum.VBLF + "WHERE Ptno='" + ArgPano + "' ";
                SQL += ComNum.VBLF + "  AND CHARTDATE =replace('" + ArgInDate + "','-','') ";
                SQL += ComNum.VBLF + "  AND FORMNO = '2506' ";
                SQL += ComNum.VBLF + "   ORDER BY CHARTDATE DESC, TRIM(CHARTTIME) DESC ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strRtn;
                }
                if (dt.Rows.Count > 0)
                {
                    strRtn = dt.Rows[0]["CHARTXML"].ToString().Trim();
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            return strRtn;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //{
            //    return;//권한 확인
            //}

            READ_DATA();
        }

        private void FrmEmlPationtListNew_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
            SS1_Sheet1.RowCount = 2;
            TxtSDATE.Text = Convert.ToDateTime(strDTP).AddDays(-20).ToString("yyyy-MM-dd");
            TxtEDATE.Text = strDTP;
            //ComboWard_SET();
            SS1_Sheet1.RowCount = 2;
            if (clsType.User.Sabun == "21403")
            {
            }
            else
            {
                SS1_Sheet1.Columns[26].Visible = false;
                SS1_Sheet1.Columns[27].Visible = false;
                SS1_Sheet1.Columns[28].Visible = false;
                //SS1_Sheet1.Columns[29].Visible = false;
                SS1_Sheet1.Columns[30].Visible = false;
                SS1_Sheet1.Columns[31].Visible = false;
                SS1_Sheet1.ColumnHeader.Visible = false;
            }
        }

        private void ComboWard_SET()
        {
            int i = 0;
            //int j = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  WardCode, WardName";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_WARD";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND WARDCODE NOT IN ('IU','NP','2W','NR','DR','IQ') ";
            SQL += ComNum.VBLF + "      AND WARDCODE  IN ('65','33','83')";
            SQL = SQL + ComNum.VBLF + "     AND USED = 'Y'  ";
            SQL += ComNum.VBLF + "ORDER BY WardCode";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            cboWARD.Items.Clear();

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboWARD.Items.Add(dt.Rows[i]["WardCode"].ToString().Trim());
                }
            }

            dt.Dispose();
            dt = null;

            cboWARD.SelectedIndex = 0;
        }

        private string Read_Bed_History(string arg, string argWARD, string argSort = "")
        {
            string strRtn = "";
            string StrTemp = "";
            //int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "SELECT a.Roomcode,b.NAME, b.CODE ";
                SQL += ComNum.VBLF + " from NUR_BED_TRANSFER a,BAS_BCODE b  ";

                if (argWARD == "33")
                {
                    SQL += ComNum.VBLF + " where GUBUN = 'NUR_ICU_침상번호' AND DELDATE IS NULL ";
                }
                else if (argWARD == "65")
                {
                    SQL += ComNum.VBLF + " where GUBUN = 'NUR_EWARD_침상번호' AND DELDATE IS NULL ";
                }
                else
                {
                    SQL += ComNum.VBLF + " where GUBUN = 'NUR_EWARD_침상번호_83' AND DELDATE IS NULL ";
                }
                SQL += ComNum.VBLF + "   AND ipdno = '" + arg + "' ";
                SQL += ComNum.VBLF + "   AND wardcode = '" + argWARD + "' ";
                SQL += ComNum.VBLF + "   and a.bednum=b.code ";
                SQL += ComNum.VBLF + " ORDER BY trsdate  " + argSort + " ";


                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strRtn;
                }
                if (dt.Rows.Count > 0)
                {
                    StrTemp = dt.Rows[0]["Roomcode"].ToString().Trim() + "-" + dt.Rows[0]["name"].ToString().Trim();
                    if (StrTemp.IndexOf("격리") != -1)
                    {
                        StrTemp = StrTemp.Replace("격리", "격");
                    }
                    strRtn = StrTemp;
                }
                dt.Dispose();
                dt = null;
                return strRtn;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            return strRtn;
        }
        
        private string READ_SEND_TIME(string ArgPano, string ArgInDate, string ArgWardCode, string ArgGubun)
        {

            //int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strRtn = "";
            string strTime1 = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "SELECT EXTRACTVALUE(a.CHARTXML, '//dt1') date1, EXTRACTVALUE(a.CHARTXML, '//it26') time1, ";
                SQL += ComNum.VBLF + " EXTRACTVALUE(a.CHARTXML, '//it19') path, A.CHARTTIME ";
                SQL += ComNum.VBLF + " from kosmos_emr.emrxml a, kosmos_emr.emrxmlmst b ";
                SQL += ComNum.VBLF + " where b.chartdate = '" + ArgInDate.Replace("-","") + "'";
                SQL += ComNum.VBLF + "   AND B.PTNO = '" + ArgPano + "'";
                if (ArgGubun == "SEND")
                {
                    SQL += ComNum.VBLF + " and b.formno in ('2279')";
                    SQL += ComNum.VBLF + " and trim(EXTRACTVALUE(a.CHARTXML, '//it19')) = '" + ArgWardCode + "'";
                }
                else if (ArgGubun == "RECV")
                {
                    SQL += ComNum.VBLF + " and b.formno in ('2280')";
                    SQL += ComNum.VBLF + " and trim(EXTRACTVALUE(a.CHARTXML, '//it22')) = '" + ArgWardCode + "'";
                }
                
                SQL += ComNum.VBLF + " and a.emrno = b.emrno";

                #region 신규
                SQL = SQL + ComNum.VBLF + "UNION ALL";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "   A.CHARTDATE AS date1";
                SQL = SQL + ComNum.VBLF + " , A.CHARTTIME AS Time1";
                SQL = SQL + ComNum.VBLF + " , (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND ITEMNO = 'I0000033506') path ";
                SQL = SQL + ComNum.VBLF + " , A.CHARTTIME";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW B";
                SQL = SQL + ComNum.VBLF + "       ON A.EMRNO = B.EMRNO";
                SQL = SQL + ComNum.VBLF + "      AND A.EMRNOHIS = B.EMRNOHIS";
                if (ArgGubun == "SEND")
                {
                    SQL += ComNum.VBLF + "      AND B.ITEMNO = 'I0000033506'";
                    SQL += ComNum.VBLF + "      AND B.ITEMVALUE = '" + ArgWardCode + "'";
                }
                else if (ArgGubun == "RECV")
                {
                    SQL += ComNum.VBLF + "      AND B.ITEMNO = 'I0000033507'";
                    SQL += ComNum.VBLF + "      AND B.ITEMVALUE = '" + ArgWardCode + "'";
                }

                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + ArgPano + "'";                
                SQL = SQL + ComNum.VBLF + "   AND CHARTDATE = '" + ArgInDate.Replace("-", "") + "'";

                if (ArgGubun == "SEND")
                {
                    SQL += ComNum.VBLF + "   AND FORMNO in (2279)";
                }
                else if (ArgGubun == "RECV")
                {
                    SQL += ComNum.VBLF + "   AND FORMNO in (2280)";
                }

                #endregion


                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strRtn;
                }
                if (dt.Rows.Count > 0)
                {
                    strTime1 = dt.Rows[0]["time1"].ToString().Trim();
                    if (strTime1 == "")
                    {
                        strTime1 = dt.Rows[0]["CHARTTIME"].ToString().Trim();
                        strTime1 = VB.Left(strTime1, 4);
                    }
                    if (ArgGubun == "SEND")
                    {
                        strRtn = strTime1 + "XXXXX";
                    }
                    else if (ArgGubun == "RECV")
                    {
                        strRtn = strTime1 + dt.Rows[0]["PATH"].ToString().Trim();
                    }
                }
                dt.Dispose();
                dt = null;
                return strRtn;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            return strRtn;
        }
  
        private void READ_DATA()
        {
            int i = 0;
            //int j = 0;
            int k = 0;
            int l = 0;
            int m = 0;

            string SQL = "";
            DataTable dt = null;
            DataTable dt2 = null;
            string SqlErr = "";

            string strPTMIIDNO = "";
            string strPTMIINDT = "";
            string strPTMIINTM = "";

            string strINDT1 = "";       //응급실 내원일자 FROM
            string strINDT2 = "";       //응급실 내원일자 TO

            string strINDATE = "";      //입원일자 IPD_NEW_MASTER
            string strOUTDATE = "";     //퇴원일자 IPD_NEW_MASTER
            string strIPDNO = "";       //입원번호
            string strIpdGubun = "";    //재원기간 중 구분(입원/재원/퇴원/전실)
            string strIpdGubun2 = "";      //재원기간 중 구분이 전실일 경우 입실 = + , 퇴실 = -

            string strCboWard = "";     //콤보박스 wardcode

            string strWardCode = "";    //대상병동
            string strWardIn = "";      //해당병동 입실일자
            string strWardOut = "";     //해당병동 퇴실일자

            string strTime = "";        //입실시간
            string strPath = "";        //입실경로
            string strPathICU = "";     //중환자실 경유 환자일 경우
            string strGUBUN = "";       //저장된 데이터 여부 

            string strHSDT = "";
            string strHSTM = "";

            string strBedASC = "";
            string strBedDESC = "";

            string strCheckIndate = "";
            string strCheckIntime = "";


            //string str33 = "";      //33병동 경유 여부
            //string str35 = "";      //35병동 경유 여부
            //string str65 = "";      //65병동 경유 여부

            SS1_Sheet1.RowCount = 2;

            strINDT1 = TxtSDATE.Text.Replace("-", "");
            strINDT2 = TxtEDATE.Text.Replace("-", "");

            strCboWard = cboWARD.Text.Trim();

            if (strCboWard == "")
            {
                MessageBox.Show("병동을 선택하시기 바랍니다.");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "   SELECT PTMIIDNO, PTMIINDT, PTMIINTM, PTMIOTDT, ";
                SQL += ComNum.VBLF + "          PTMIOTTM, '' PTMIDCDT, '' PTMIDCTM, '' PTMIHSDT,  ";
                SQL += ComNum.VBLF + "          '' PTMIHSTM, IPDNO, '' PANO, '' INDATE,  ";
                SQL += ComNum.VBLF + "          '' OUTDATE, KTASLEVL, BEDNUM, SNAME,  ";
                SQL += ComNum.VBLF + "          HOSPDEPT DEPTCODE, '2' GUBUN, ";
                SQL += ComNum.VBLF + "          EWRDINDT, EWRDINTM, EWRDOTDT, ";
                SQL += ComNum.VBLF + "          EWRDOTTM, EWRDPATH, EICUINDT, EICUINTM, ";
                SQL += ComNum.VBLF + "          EICUOTDT, EICUOTTM, EICUPATH, NEDSDEPT, ROWID ROWID1, INPATH ";
                SQL += ComNum.VBLF + "     FROM KOSMOS_PMPA.NUR_ER_SPBOOK ";
                SQL += ComNum.VBLF + "    WHERE ( (PTMIINDT >= '" + strINDT1 + "' AND PTMIINDT <= '" + strINDT2 + "') ";
                if (strCboWard == "65")
                {
                    SQL += ComNum.VBLF + "    OR ( PTMIINDT IS NULL AND EWRDINDT >= '" + strINDT1 + "' AND EWRDINDT <= '" + strINDT2 + "') )";
                    SQL += ComNum.VBLF + "    AND WARDCODE = '65' ";
                }
                else if (strCboWard == "83")
                {
                    SQL += ComNum.VBLF + "    OR ( PTMIINDT IS NULL AND EWRDINDT >= '" + strINDT1 + "' AND EWRDINDT <= '" + strINDT2 + "') )";
                    SQL += ComNum.VBLF + "    AND WARDCODE = '83' ";
                }
                else
                {
                    SQL += ComNum.VBLF + "    OR ( PTMIINDT IS NULL AND EICUINDT >= '" + strINDT1 + "' AND EICUINDT <= '" + strINDT2 + "') )";
                    SQL += ComNum.VBLF + "    AND WARDCODE = '33' ";
                }
                
                SQL += ComNum.VBLF + "      AND DELDATE IS NULL ";

                SQL += ComNum.VBLF + " UNION ALL ";
                SQL += ComNum.VBLF + " SELECT A.PTMIIDNO, A.PTMIINDT, A.PTMIINTM, A.PTMIOTDT, A.PTMIOTTM, A.PTMIDCDT, A.PTMIDCTM, A.PTMIHSDT, A.PTMIHSTM, ";
                SQL += ComNum.VBLF + "  B.IPDNO, B.PANO, TO_CHAR(B.INDATE,'YYYY-MM-DD') INDATE, ";
                SQL += ComNum.VBLF + " TO_CHAR(B.OUTDATE,'YYYY-MM-DD') OUTDATE, B.KTASLEVL, B.BEDNUM, B.SNAME, B.DEPTCODE, ";
                SQL += ComNum.VBLF + " '1' GUBUN,";     //여기까지 공용 부분
                SQL += ComNum.VBLF + " '' EWRDINDT, '' EWRDINTM, '' EWRDOTDT, ";
                SQL += ComNum.VBLF + " '' EWRDOTTM, '' EWRDPATH, '' EICUINDT, '' EICUINTM, ";
                SQL += ComNum.VBLF + " '' EICUOTDT, '' EICUOTTM, '' EICUPATH, '' NEDSDEPT,  B.ROWID ROWID1, '' INPATH ";
                SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.VIEW_ER_EMIHPTMI A, KOSMOS_PMPA.IPD_NEW_MASTER B";
                SQL += ComNum.VBLF + " WHERE PTMIINDT >= '" + strINDT1 + "'";
                SQL += ComNum.VBLF + "   AND PTMIINDT <= '" + strINDT2 + "'";
                SQL += ComNum.VBLF + "   AND TO_DATE(A.PTMIINDT, 'YYYY-MM-DD') = TRUNC(B.INDATE(+))";
                SQL += ComNum.VBLF + "   AND A.PTMIIDNO = B.PANO(+)";
                SQL += ComNum.VBLF + "   AND B.AMSET7(+) IN ('3', '4', '5')";
                //SQL += ComNum.VBLF + "   AND PTMIDGKD IN ('1', '2')";
                //SQL += ComNum.VBLF + "   AND PTMIDCRT >= '1'";
                //SQL += ComNum.VBLF + "   AND A.PTMIIDNO = '10786544'";
                SQL += ComNum.VBLF + "   AND EXISTS ( ";
                SQL += ComNum.VBLF + "                SELECT * ";
                SQL += ComNum.VBLF + "                  FROM KOSMOS_PMPA.NUR_NURSEGRADE_TONG ";
                if (strCboWard == "65")
                {
                    SQL += ComNum.VBLF + "                 WHERE IPDNO = B.IPDNO AND WARDCODE = '65')";
                }
                else if (strCboWard == "83")
                {
                    SQL += ComNum.VBLF + "                 WHERE IPDNO = B.IPDNO AND WARDCODE = '83')";
                }

                else
                {
                    SQL += ComNum.VBLF + "                 WHERE IPDNO = B.IPDNO AND WARDCODE = '33')";
                }

                SQL += ComNum.VBLF + "   AND NOT EXISTS ";
                SQL += ComNum.VBLF + "   ( SELECT *  ";
                SQL += ComNum.VBLF + "       FROM KOSMOS_PMPA.NUR_ER_SPBOOK ";
                SQL += ComNum.VBLF + "      WHERE A.PTMIIDNO = PTMIIDNO ";
                SQL += ComNum.VBLF + "        AND A.PTMIINDT = PTMIINDT ";
                SQL += ComNum.VBLF + "        AND A.PTMIINTM = PTMIINTM ";
                if (strCboWard == "65")
                {
                    SQL += ComNum.VBLF + "        AND WARDCODE = '65') ";
                }
                else if (strCboWard == "83")
                {
                    SQL += ComNum.VBLF + "        AND WARDCODE = '83') ";
                }
                else
                {
                    SQL += ComNum.VBLF + "        AND WARDCODE = '33') ";
                }
                SQL += ComNum.VBLF + " ORDER BY PTMIINDT, PTMIINTM, PTMIIDNO ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {

                    SS1_Sheet1.Rows.Count = dt.Rows.Count + 2;
                    SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        k = i + 2;
                        strPathICU = "";
                        strINDATE = dt.Rows[i]["INDATE"].ToString().Trim();
                        strOUTDATE = dt.Rows[i]["OUTDATE"].ToString().Trim();
                        strIPDNO = dt.Rows[i]["IPDNO"].ToString().Trim();
                        strPTMIIDNO = dt.Rows[i]["PTMIIDNO"].ToString().Trim();
                        strPTMIINDT = dt.Rows[i]["PTMIINDT"].ToString().Trim();
                        strPTMIINTM = dt.Rows[i]["PTMIINTM"].ToString().Trim();
                        strGUBUN = dt.Rows[i]["GUBUN"].ToString().Trim();

                        if (strPTMIIDNO == "10674047")
                        {
                            //strPTMIIDNO = strPTMIIDNO;
                        }

                        SS1_Sheet1.Cells[k, 1].Text = (i + 1).ToString();
                        SS1_Sheet1.Cells[k, 2].Text = Read_Bed_History(strIPDNO, cboWARD.Text, "DESC");
                        SS1_Sheet1.Cells[k, 3].Text = "Y";
                        SS1_Sheet1.Cells[k, 5].Text = "";
                        SS1_Sheet1.Cells[k, 6].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        SS1_Sheet1.Cells[k, 7].Text = strPTMIIDNO;
                        SS1_Sheet1.Cells[k, 8].Text = SetDateFormat(strPTMIINDT);
                        SS1_Sheet1.Cells[k, 9].Text = SetTimeFormat(strPTMIINTM);
                        SS1_Sheet1.Cells[k, 30].Text = strPTMIINDT;
                        SS1_Sheet1.Cells[k, 31].Text = ComFunc.SetAutoZero(strPTMIINTM, 4);
                        SS1_Sheet1.Cells[k, 10].Text = SetDateFormat(dt.Rows[i]["PTMIOTDT"].ToString().Trim());
                        SS1_Sheet1.Cells[k, 11].Text = SetTimeFormat(dt.Rows[i]["PTMIOTTM"].ToString().Trim());
                        SS1_Sheet1.Cells[k, 21].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        SS1_Sheet1.Cells[k, 22].Text = CHANGE_DEPTCODE(dt.Rows[i]["DEPTCODE"].ToString().Trim());
                        SS1_Sheet1.Cells[k, 25].Text = READ_KTAS(strPTMIIDNO, strPTMIINDT, strPTMIINTM);
                        //SS1_Sheet1.Cells[k, 25].Text = dt.Rows[i]["KTASLEVL"].ToString().Trim();
                        SS1_Sheet1.Cells[k, 26].Text = strIPDNO;
                        SS1_Sheet1.Cells[k, 27].Text = dt.Rows[i]["ROWID1"].ToString().Trim();
                        SS1_Sheet1.Cells[k, 28].Text = strGUBUN;

                        strBedASC = Read_Bed_History(strIPDNO, cboWARD.Text, "ASC");
                        strBedDESC = Read_Bed_History(strIPDNO, cboWARD.Text, "DESC");

                        if (strBedASC == strBedDESC)
                        {
                            SS1_Sheet1.Cells[k, 29].Text = strBedDESC;
                        }
                        else
                        {
                            SS1_Sheet1.Cells[k, 29].Text = strBedASC + "->" + strBedDESC;
                        }

                        if (strGUBUN == "2")
                        {
                            SS1_Sheet1.Cells[k, 2].Text = dt.Rows[i]["BEDNUM"].ToString().Trim();
                            SS1_Sheet1.Cells[k, 12].Text = SetDateFormat(dt.Rows[i]["EWRDINDT"].ToString().Trim());
                            SS1_Sheet1.Cells[k, 13].Text = SetTimeFormat(dt.Rows[i]["EWRDINTM"].ToString().Trim());
                            SS1_Sheet1.Cells[k, 14].Text = SetDateFormat(dt.Rows[i]["EWRDOTDT"].ToString().Trim());
                            SS1_Sheet1.Cells[k, 15].Text = SetTimeFormat(dt.Rows[i]["EWRDOTTM"].ToString().Trim());
                            SS1_Sheet1.Cells[k, 16].Text = dt.Rows[i]["EWRDPATH"].ToString().Trim();

                            SS1_Sheet1.Cells[k, 17].Text = SetDateFormat(dt.Rows[i]["EICUINDT"].ToString().Trim());
                            SS1_Sheet1.Cells[k, 19].Text = SetTimeFormat(dt.Rows[i]["EICUINTM"].ToString().Trim());
                            SS1_Sheet1.Cells[k, 20].Text = dt.Rows[i]["EICUPATH"].ToString().Trim();
                            SS1_Sheet1.Cells[k, 23].Text = SetDateFormat(dt.Rows[i]["EICUOTDT"].ToString().Trim());
                            SS1_Sheet1.Cells[k, 24].Text = SetTimeFormat(dt.Rows[i]["EICUOTTM"].ToString().Trim());

                            SS1_Sheet1.Cells[k, 29].Text = dt.Rows[i]["INPATH"].ToString().Trim();

                        }
                        else
                        {
                            strPathICU = "";

                            SQL = " SELECT TO_CHAR(JOBDATE,'YYYY-MM-DD') JOBDATE, WARDCODE, GUBUN, GUBUN2,";
                            SQL += ComNum.VBLF + " TO_CHAR(INDATE,'YYYY-MM-DD') WARDIN, TO_CHAR(OUTDATE, 'YYYY-MM-DD') WARDOUT, BI ";
                            SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_NURSEGRADE_TONG";
                            SQL += ComNum.VBLF + " WHERE IPDNO = " + strIPDNO;
                            if (strCboWard == "65")
                            {
                                SQL += ComNum.VBLF + " AND WARDCODE = '65'";
                                SQL += ComNum.VBLF + " ORDER BY JOBDATE ASC, ";
                            }
                            else if (strCboWard == "83")
                            {
                                SQL += ComNum.VBLF + " AND WARDCODE = '83'";
                                SQL += ComNum.VBLF + " ORDER BY JOBDATE ASC, ";
                            }
                            else
                            {
                                SQL += ComNum.VBLF + " AND WARDCODE IN ('83','65','33')";
                                SQL += ComNum.VBLF + " ORDER BY DECODE(WARDCODE, '33','1','65','2','83','3') ASC, JOBDATE ASC, ";
                            }

                            SQL += ComNum.VBLF + " CASE WHEN GUBUN = '입원' THEN 0 ";
                            SQL += ComNum.VBLF + "      WHEN GUBUN = '전실' AND GUBUN2 = '-' THEN 1 ";
                            SQL += ComNum.VBLF + "      WHEN GUBUN = '전실' AND GUBUN2 = '+' THEN 2 ";
                            SQL += ComNum.VBLF + "      WHEN GUBUN = '재원' THEN 3 ";
                            SQL += ComNum.VBLF + "      WHEN GUBUN = '퇴원' THEN 4 ELSE 5 END";
                            SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            if (dt2.Rows.Count > 0)
                            {

                                strIpdGubun = "";
                                strIpdGubun2 = "";
                                strWardIn = "";
                                strWardOut = "";

                                for (l = 0; l < dt2.Rows.Count; l++)
                                {

                                    strWardCode = dt2.Rows[l]["WARDCODE"].ToString().Trim();

                                    strIpdGubun = dt2.Rows[l]["GUBUN"].ToString().Trim();
                                    strIpdGubun2 = dt2.Rows[l]["GUBUN2"].ToString().Trim();

                                    strWardIn = dt2.Rows[l]["JOBDATE"].ToString().Trim();
                                    strWardOut = dt2.Rows[l]["JOBDATE"].ToString().Trim();

                                    strHSDT = "";
                                    strHSTM = "";

                                    if (strCboWard == "65" || strCboWard == "83")
                                    {
                                        //if (strWardCode == "65")
                                        //{

                                        if (strIpdGubun == "입원")
                                        {

                                            strHSDT = dt.Rows[i]["PTMIHSDT"].ToString().Trim();
                                            strHSTM = dt.Rows[i]["PTMIHSTM"].ToString().Trim();
                                            if (strHSDT != "" && strHSTM != "")
                                            {
                                                SS1_Sheet1.Cells[k, 17].Text = strHSDT;
                                                SS1_Sheet1.Cells[k, 19].Text = strHSTM;
                                            }
                                            else
                                            {
                                                strHSTM = READ_IPWON_TIME(strPTMIIDNO, strWardIn);
                                                SS1_Sheet1.Cells[k, 17].Text = strWardIn;
                                                SS1_Sheet1.Cells[k, 19].Text = strHSTM;
                                            }

                                            
                                            SS1_Sheet1.Cells[k, 20].Text = "ER";

                                        }
                                        else if (strIpdGubun == "전실" && strIpdGubun2 == "+")     //해당 병동에 들어온 날짜
                                        {
                                            SS1_Sheet1.Cells[k, 17].Text = strWardIn;
                                            strPath = READ_SEND_TIME(strPTMIIDNO, strWardIn, strWardCode, "RECV");
                                            strTime = READ_INDATE_ICU(strPTMIIDNO, strINDATE, strWardCode);
                                            SS1_Sheet1.Cells[k, 19].Text = strTime;
                                            SS1_Sheet1.Cells[k, 20].Text = READ_PATH(VB.Mid(strPath, 6, VB.Len(strPath)));

                                        }
                                        else if (strIpdGubun == "퇴원")
                                        {
                                            SS1_Sheet1.Cells[k, 23].Text = dt.Rows[i]["PTMIDCDT"].ToString().Trim();
                                            SS1_Sheet1.Cells[k, 24].Text = dt.Rows[i]["PTMIDCTM"].ToString().Trim();

                                        }
                                        else if (strIpdGubun == "전실" && strIpdGubun2 == "-")
                                        {

                                            SS1_Sheet1.Cells[k, 23].Text = strWardIn;
                                            strPath = READ_SEND_TIME(strPTMIIDNO, strWardIn, strWardCode, "SEND");
                                            strTime = READ_OUTDATE_ICU(strPTMIIDNO, strINDATE, strWardCode, strWardOut);
                                            //SS1_Sheet1.Cells[k, 24].Text = VB.Left(strPath, 5).Replace("X", "");
                                            SS1_Sheet1.Cells[k, 24].Text = strTime;

                                        }
                                        //}
                                    }
                                    else
                                    {
                                        if ((strWardCode == "65" || strWardCode == "83") && strPathICU == "EWARD")
                                        {

                                            if (strIpdGubun == "입원")
                                            {


                                                strHSDT = dt.Rows[i]["PTMIHSDT"].ToString().Trim();
                                                strHSTM = dt.Rows[i]["PTMIHSTM"].ToString().Trim();
                                                if (strHSDT != "" && strHSTM != "")
                                                {
                                                    SS1_Sheet1.Cells[k, 12].Text = strHSDT;
                                                    SS1_Sheet1.Cells[k, 13].Text = strHSTM;
                                                }
                                                else
                                                {
                                                    strHSTM = READ_IPWON_TIME(strPTMIIDNO, strWardIn);
                                                    SS1_Sheet1.Cells[k, 12].Text = strWardIn;
                                                    SS1_Sheet1.Cells[k, 13].Text = strHSTM;
                                                }
                                                SS1_Sheet1.Cells[k, 16].Text = "ER";

                                            }
                                            else if (strIpdGubun == "전실" && strIpdGubun2 == "+")     //해당 병동에 들어온 날짜
                                            {
                                                SS1_Sheet1.Cells[k, 12].Text = strWardIn;
                                                strPath = READ_SEND_TIME(strPTMIIDNO, strWardIn, strWardCode, "RECV");
                                                strTime = READ_INDATE_ICU(strPTMIIDNO, strINDATE, strWardCode);
                                                SS1_Sheet1.Cells[k, 13].Text = strTime;
                                                SS1_Sheet1.Cells[k, 16].Text = READ_PATH(VB.Mid(strPath, 6, VB.Len(strPath)));
                                            }
                                            else if (strIpdGubun == "퇴원")
                                            {
                                                SS1_Sheet1.Cells[k, 14].Text = dt.Rows[i]["PTMIDCDT"].ToString().Trim();
                                                SS1_Sheet1.Cells[k, 15].Text = dt.Rows[i]["PTMIDCTM"].ToString().Trim();

                                            }
                                            else if (strIpdGubun == "전실" && strIpdGubun2 == "-")
                                            {
                                                SS1_Sheet1.Cells[k, 14].Text = strWardIn;
                                                strPath = READ_SEND_TIME(strPTMIIDNO, strWardIn, strWardCode, "SEND");
                                                strTime = READ_OUTDATE_ICU(strPTMIIDNO, strINDATE, strWardCode, strWardOut);
                                                //SS1_Sheet1.Cells[k, 15].Text = VB.Left(strPath, 5).Replace("X", "");
                                                SS1_Sheet1.Cells[k, 15].Text = strTime;
                                            }
                                        }
                                        if (strWardCode == "33")
                                        {
                                            if (strIpdGubun == "입원")
                                            {

                                                strHSDT = dt.Rows[i]["PTMIHSDT"].ToString().Trim();
                                                strHSTM = dt.Rows[i]["PTMIHSTM"].ToString().Trim();
                                                if (strHSDT != "" && strHSTM != "")
                                                {
                                                    SS1_Sheet1.Cells[k, 17].Text = strHSDT;
                                                    SS1_Sheet1.Cells[k, 19].Text = strHSTM;
                                                }
                                                else
                                                {
                                                    strHSTM = READ_IPWON_TIME(strPTMIIDNO, strWardIn);
                                                    SS1_Sheet1.Cells[k, 17].Text = strWardIn;
                                                    SS1_Sheet1.Cells[k, 19].Text = strHSTM;
                                                }
                                                SS1_Sheet1.Cells[k, 20].Text = "ER";
                                            }
                                            else if (strIpdGubun == "전실" && strIpdGubun2 == "+")     //해당 병동에 들어온 날짜
                                            {
                                                SS1_Sheet1.Cells[k, 17].Text = strWardIn;
                                                strPath = READ_SEND_TIME(strPTMIIDNO, strWardIn, strWardCode, "RECV");
                                                strTime = READ_INDATE_ICU(strPTMIIDNO, strINDATE, strWardCode);
                                                //SS1_Sheet1.Cells[k, 19].Text = VB.Left(strPath, 5);
                                                SS1_Sheet1.Cells[k, 19].Text = strTime;
                                                SS1_Sheet1.Cells[k, 20].Text = READ_PATH(VB.Mid(strPath, 6, VB.Len(strPath)));
                                                if (strPathICU == "" && SS1_Sheet1.Cells[k, 20].Text.Trim() == "EWARD")
                                                {
                                                    strPathICU = "EWARD";
                                                }

                                            }
                                            else if (strIpdGubun == "퇴원")
                                            {
                                                SS1_Sheet1.Cells[k, 23].Text = dt.Rows[i]["PTMIDCDT"].ToString().Trim();
                                                SS1_Sheet1.Cells[k, 24].Text = dt.Rows[i]["PTMIDCTM"].ToString().Trim();

                                            }
                                            else if (strIpdGubun == "전실" && strIpdGubun2 == "-")
                                            {

                                                SS1_Sheet1.Cells[k, 23].Text = strWardIn;
                                                strPath = READ_SEND_TIME(strPTMIIDNO, strWardIn, strWardCode, "SEND");
                                                strTime = READ_OUTDATE_ICU(strPTMIIDNO, strINDATE, strWardCode, strWardOut);
                                                //SS1_Sheet1.Cells[k, 24].Text = VB.Left(strPath, 5).Replace("X", "");
                                                SS1_Sheet1.Cells[k, 24].Text = strTime;


                                            }
                                        }
                                    }

                                }



                                SS1_Sheet1.Cells[k, 12].Text = SetDateFormat(SS1_Sheet1.Cells[k, 12].Text.Replace("-", ""));
                                strCheckIndate = SS1_Sheet1.Cells[k, 12].Text;
                                SS1_Sheet1.Cells[k, 13].Text = SetTimeFormat(SS1_Sheet1.Cells[k, 13].Text.Replace(":", ""));
                                strCheckIntime = SS1_Sheet1.Cells[k, 13].Text;

                                if ((strCheckIndate == "" || strCheckIntime == "") && strWardCode == "35" )
                                {
                                    strCheckIndate = READ_INDATE_ICU2(strPTMIIDNO, strINDATE, strWardCode);

                                    strCheckIntime = VB.Right(strCheckIndate, 5);
                                    strCheckIndate = VB.Left(strCheckIndate, 10);

                                    SS1_Sheet1.Cells[k, 12].Text = SetDateFormat(strCheckIndate);
                                    SS1_Sheet1.Cells[k, 13].Text = SetTimeFormat(strCheckIntime);

                                    SS1_Sheet1.Cells[k, 16].Text = "ER";

                                    strCheckIndate = "";
                                    strCheckIntime = "";
                                }

                                SS1_Sheet1.Cells[k, 14].Text = SetDateFormat(SS1_Sheet1.Cells[k, 14].Text.Replace("-", ""));
                                SS1_Sheet1.Cells[k, 15].Text = SetTimeFormat(SS1_Sheet1.Cells[k, 15].Text.Replace(":", ""));

                                SS1_Sheet1.Cells[k, 17].Text = SetDateFormat(SS1_Sheet1.Cells[k, 17].Text.Replace("-", ""));
                                strCheckIndate = SS1_Sheet1.Cells[k, 17].Text;
                                SS1_Sheet1.Cells[k, 19].Text = SetTimeFormat(SS1_Sheet1.Cells[k, 19].Text.Replace(":", ""));
                                strCheckIntime = SS1_Sheet1.Cells[k, 19].Text;

                                if (strCheckIndate == "" || strCheckIntime == "")
                                {
                                    strCheckIndate = READ_INDATE_ICU2(strPTMIIDNO, strINDATE, strWardCode);

                                    strCheckIntime = VB.Right(strCheckIndate, 5);
                                    strCheckIndate = VB.Left(strCheckIndate, 10);

                                    SS1_Sheet1.Cells[k, 17].Text = SetDateFormat(strCheckIndate);
                                    SS1_Sheet1.Cells[k, 19].Text = SetTimeFormat(strCheckIntime);

                                    SS1_Sheet1.Cells[k, 20].Text = "ER";
                                    strCheckIndate = "";
                                    strCheckIntime = "";
                                }

                                SS1_Sheet1.Cells[k, 23].Text = SetDateFormat(SS1_Sheet1.Cells[k, 23].Text.Replace("-", ""));
                                SS1_Sheet1.Cells[k, 24].Text = SetTimeFormat(SS1_Sheet1.Cells[k, 24].Text.Replace(":", ""));



                            }

                            dt2.Dispose();
                            dt2 = null;
                        }



                        if (strGUBUN == "2")
                        {
                            for (m = 0; m < SS1_Sheet1.ColumnCount; m++)
                            {
                                SS1_Sheet1.Cells[k, m].ForeColor = Color.Blue;
                            }
                        }

                    }

                    dt.Dispose();
                    dt = null;
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        private string SetDateFormat(string arg)
        {

            string rtnVal = "";

            if(arg == "" )
            { return ""; }

            rtnVal = VB.Left(arg, 4) + "-" + VB.Mid(arg, 5, 2) + "-" + VB.Right(arg, 2);

            return rtnVal;
        }

        private string SetTimeFormat(string arg)
        {

            string rtnVal = "";

            if (arg == "")
            { return ""; }

            rtnVal = VB.Left(arg, 2) + ":" +  VB.Right(arg, 2);

            return rtnVal;
        }

        private string READ_IPWON_TIME(string argPTNO, string argINDATE)
        {

            string SQL = "";
            DataTable dt = null;
            
            string SqlErr = "";
            string strTime = "";

            ComFunc cf = new ComFunc();

            #region 신규 기록지
            SQL = "";
            SQL = "SELECT A.FORMNO, B.ITEMCD, B.ITEMVALUE";
            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW B";
            SQL = SQL + ComNum.VBLF + "       ON A.EMRNO = B.EMRNO";
            SQL = SQL + ComNum.VBLF + "      AND A.EMRNOHIS = B.EMRNOHIS";
            SQL = SQL + ComNum.VBLF + "      AND B.ITEMCD IN ('I0000033484', 'I0000015591', 'I0000028905') --입원시간,  출생시간";
            SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + argPTNO + "'";
            SQL = SQL + ComNum.VBLF + "   AND A.FORMNO IN(2311, 2285, 2356, 2294, 2295, 2305, 1561)";
            SQL = SQL + ComNum.VBLF + "    AND CHARTDATE >= '" + argINDATE.Replace("-", "") + "'";
            SQL = SQL + ComNum.VBLF + "    AND CHARTDATE <= '" + cf.DATE_ADD(clsDB.DbCon, argINDATE, 1).Replace("-", "") + "'";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return strTime;
            }

            if (dt.Rows.Count > 0)
            {
                strTime = dt.Rows[0]["ITEMVALUE"].ToString().Trim();
                dt.Dispose();
                return strTime;
            }
            dt.Dispose();

            #endregion


            SQL = " SELECT extractValue(chartxml, '//dt1') indate, extractValue(chartxml, '//it4') time";
            SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML ";
            SQL += ComNum.VBLF + "    WHERE PTNO = '" + argPTNO + "'";
            SQL += ComNum.VBLF + "    AND FORMNO = '2311'";
            SQL += ComNum.VBLF + "    AND CHARTDATE >= '" + argINDATE.Replace("-","") + "'";
            SQL += ComNum.VBLF + "    AND CHARTDATE <= '" +  cf.DATE_ADD(clsDB.DbCon, argINDATE, 1).Replace("-", "") + "'";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return "0000";
            }

            if (dt.Rows.Count > 0)
            {
                strTime = dt.Rows[0]["time"].ToString().Trim();
                dt.Dispose();
                dt = null;
                return strTime;
            }
            else
            {
                dt.Dispose();
                dt = null;
            }

            SQL = " SELECT DECODE(FORMNO, '2285', extractValue(chartxml, '//dt1'), extractValue(chartxml, '//dt2')) indate, ";
            SQL += ComNum.VBLF + " extractValue(chartxml, '//it3') time";
            SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML ";
            SQL += ComNum.VBLF + "    WHERE PTNO = '" + argPTNO + "'";
            SQL += ComNum.VBLF + "    AND FORMNO IN ('2285','2356')";
            SQL += ComNum.VBLF + "    AND CHARTDATE = '" + argINDATE.Replace("-", "") + "'";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return "0000";
            }

            if (dt.Rows.Count > 0)
            {
                strTime = dt.Rows[0]["time"].ToString().Trim();
                dt.Dispose();
                dt = null;
                return strTime;
            }
            else
            {
                dt.Dispose();
                dt = null;

            }

            SQL = " SELECT DECODE(FORMNO, '2295', extractValue(chartxml, '//dt3'), extractValue(chartxml, '//dt2')) indate, ";
            SQL += ComNum.VBLF + " extractValue(chartxml, '//it4') time";
            SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML ";
            SQL += ComNum.VBLF + "   WHERE PTNO = '" + argPTNO + "'";
            SQL += ComNum.VBLF + "   AND FORMNO IN ('2294','2295','2305')";
            SQL += ComNum.VBLF + "   AND CHARTDATE = '" + argINDATE.Replace("-", "") + "'";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return "0000";
            }

            if (dt.Rows.Count > 0)
            {
                strTime = dt.Rows[0]["time"].ToString().Trim();
                dt.Dispose();
                dt = null;
                return strTime;
            }
            else
            {
                dt.Dispose();
                dt = null;

            }

            SQL = "   SELECT DECODE(FORMNO, '1556', extractValue(chartxml, '//it8'), ";
            SQL += ComNum.VBLF + " DECODE(FORMNO, '1558', extractValue(chartxml, '//it99'), extractValue(chartxml, '//it3'))) time";
            SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML";
            SQL += ComNum.VBLF + "   WHERE PTNO = '" + argPTNO + "'";
            SQL += ComNum.VBLF + "   AND FORMNO IN ('1545','1553','1554','1556','1558','1561')";
            SQL += ComNum.VBLF + "   AND CHARTDATE = '" + argINDATE.Replace("-", "") + "'";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return "0000";
            }

            if (dt.Rows.Count > 0)
            {
                strTime = dt.Rows[0]["time"].ToString().Trim();
                dt.Dispose();
                dt = null;
                return strTime;
            }
            else
            {
                dt.Dispose();
                dt = null;

            }

            return "0000";

        }


        private string READ_KTAS(string argIDNO, string argINDT, string argINTM)
        {

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";


            SQL = "";

            SQL = " SELECT PTMIIDNO, PTMIINDT, PTMIINTM, MIN(PTMIKTS) KTAS ";
            SQL += ComNum.VBLF + "    FROM KOSMOS_PMPA.NUR_ER_KTAS ";
            SQL += ComNum.VBLF + "   WHERE PTMIIDNO = '" + argIDNO + "' ";
            SQL += ComNum.VBLF + "     AND PTMIINDT = '" + argINDT.Replace("-", "") + "' ";
            SQL += ComNum.VBLF + "     AND PTMIINTM = '" + argINTM.Replace(":", "") + "' ";
            SQL += ComNum.VBLF + "   GROUP BY PTMIIDNO, PTMIINDT, PTMIINTM ";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return "";
            }

            if (dt.Rows.Count > 0)
            {
                rtnVal = dt.Rows[0]["KTAS"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;

            return rtnVal;

        }


        private string CHANGE_DEPTCODE(string argDEPTCODE)
        {

            string strDEPT = "";

            switch (argDEPTCODE)
            {

                case "MI":
                    strDEPT = "AF";
                    break;
                case "MO":
                    strDEPT = "AG";
                    break;
                case "MG":
                    strDEPT = "AC";
                    break;
                case "MC":
                    strDEPT = "AA";
                    break;
                case "MP":
                    strDEPT = "AB";
                    break;
                case "ME":
                    strDEPT = "AE";
                    break;
                case "MN":
                    strDEPT = "AD";
                    break;
                case "MR":
                    strDEPT = "AX";
                    break;
                case "GS":
                    strDEPT = "BA";
                    break;
                case "NS":
                    strDEPT = "BB";
                    break;
                case "OS":
                    strDEPT = "BD";
                    break;
                case "OG":
                    strDEPT = "CA";
                    break;
                case "CS":
                    strDEPT = "BC";
                    break;
                case "PD":
                    strDEPT = "DA";
                    break;
                case "NP":
                    strDEPT = "EA";
                    break;
                case "OT":
                    strDEPT = "GA";
                    break;
                case "EN":
                    strDEPT = "HA";
                    break;
                case "UR":
                    strDEPT = "IA";
                    break;
                case "ER":
                    strDEPT = "JA";
                    break;
                case "DM":
                    strDEPT = "LA";
                    break;
                case "DT":
                    strDEPT = "NA";
                    break;
                case "NE":
                    strDEPT = "FA";
                    break;
                default:
                    strDEPT = "XX";
                    break;

            }

            return strDEPT;

        }

        private string READ_PATH(string ArgPath)
        {
            string rtnVal = "";

            switch (ArgPath)
            {
                case "ER":
                    rtnVal = "ER";
                    break;
                case "33":
                    rtnVal = "EICU";
                    break;
                case "65":
                case "83":
                    rtnVal = "EWARD";
                    break;
                default:
                    rtnVal = "ETC";
                    break;
            }

            return rtnVal;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            //string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            { }
                //strTitle = "";

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, false, false, true, true, false, false, true);

            CS.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void cboWARD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboWARD.Text == "33")
            {
                SS1_Sheet1.Columns[12].Visible = true;
                SS1_Sheet1.Columns[13].Visible = true;
                SS1_Sheet1.Columns[14].Visible = true;
                SS1_Sheet1.Columns[15].Visible = true;
                SS1_Sheet1.Columns[16].Visible = true;

                SS1_Sheet1.Cells[0, 17].Text = "중환자실";
                SS1_Sheet1.Cells[0, 23].Text = "중환자실";
            }
            else
            {
                SS1_Sheet1.Columns[12].Visible = false;
                SS1_Sheet1.Columns[13].Visible = false;
                SS1_Sheet1.Columns[14].Visible = false;
                SS1_Sheet1.Columns[15].Visible = false;
                SS1_Sheet1.Columns[16].Visible = false;
                SS1_Sheet1.Cells[0, 17].Text = "입원실";
                SS1_Sheet1.Cells[0, 23].Text = "입원실";


            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            int j = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strBEDNUM = "";      //병상번호
            string strERGBN = "";       //응급전용여부
            string strDEPTGBN = "";     //진료과지정여부
            string strSNAME = "";       //이름
            string strPTMIIDNO = "";    //등록번호
            string strPTMIINDT = "";    //응급실-내원일자
            string strPTMIINTM = "";    //응급실-내원시간
            string strPTMIOTDT = "";    //응급실-퇴실일자
            string strPTMIOTTM = "";    //응급실-퇴실시간
            string strEWRDINDT = "";    //응급전용입원실-입원일자
            string strEWRDINTM = "";    //응급전용입원실-입원시간
            string strEWRDOTDT = "";    //응급전용입원실-퇴실일자
            string strEWRDOTTM = "";    //응급전용입원실-퇴실시간
            string strEWRDPATH = "";    //응급전용입원실-입실경로
            string strEICUINDT = "";    //중환자실,입원실-입실일자
            string strEICUINTM = "";    //중환자실,입원실-입실시간
            string strEICUOTDT = "";    //중환자실,입원실-입실경로
            string strEICUOTTM = "";    //중환자실,입원실-퇴실일자
            string strEICUPATH = "";    //중환자실,입원실-퇴실시간
            string strHOSPDEPT = "";    //주진료과(병원코드)
            string strNEDSDEPT = "";    //주진료과(NEDIS코드)
            string strWARDCODE = "";    //저장하는 부서

            string strJedate = "";      //재원일자

            string strKTASLEVL = "";

            string strIPDNO = "";
            string strGUBUN = "";
            string strINPATH = "";
            string strROWID = "";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            strWARDCODE = cboWARD.Text.Trim();

            if (strWARDCODE == "")
            {
                MessageBox.Show("병동을 선택 후 저장버튼을 클릭하시기 바랍니다.");
                return;
            }


            try
            {
                for (i = 0; i < SS1_Sheet1.RowCount-2; i++)
                {
                    j = i + 2;

                    strBEDNUM = SS1_Sheet1.Cells[j, 2].Text.Trim();
                    strERGBN = SS1_Sheet1.Cells[j, 3].Text.Trim();
                    strDEPTGBN = SS1_Sheet1.Cells[j, 5].Text.Trim();
                    strSNAME = SS1_Sheet1.Cells[j, 6].Text.Trim();
                    strPTMIIDNO = SS1_Sheet1.Cells[j, 7].Text.Trim();
                    
                    strPTMIINDT = SS1_Sheet1.Cells[j, 30].Text.Trim();
                    if (strPTMIINDT == "")
                    {
                        strPTMIINDT = SS1_Sheet1.Cells[j, 8].Text.Trim();
                    }
                    strPTMIINTM = ComFunc.SetAutoZero(SS1_Sheet1.Cells[j, 31].Text.Trim(), 4);
                    if (strPTMIINTM == "")
                    {
                        strPTMIINTM = SS1_Sheet1.Cells[j, 9].Text.Trim();
                    }

                    strPTMIOTDT = SS1_Sheet1.Cells[j, 10].Text.Trim();
                    strPTMIOTTM = SS1_Sheet1.Cells[j, 11].Text.Trim();
                    strEWRDINDT = SS1_Sheet1.Cells[j, 12].Text.Trim();
                    if (strJedate == "" )
                    {
                        strJedate = strEWRDINDT;
                    }
                    strEWRDINTM = SS1_Sheet1.Cells[j, 13].Text.Trim();
                    strEWRDOTDT = SS1_Sheet1.Cells[j, 14].Text.Trim();
                    strEWRDOTTM = SS1_Sheet1.Cells[j, 15].Text.Trim();
                    strEWRDPATH = SS1_Sheet1.Cells[j, 16].Text.Trim();
                    strEICUINDT = SS1_Sheet1.Cells[j, 17].Text.Trim();
                    if (strJedate == "")
                    {
                        strJedate = strEICUINDT;
                    }
                    strEICUINTM = SS1_Sheet1.Cells[j, 19].Text.Trim();
                    strEICUOTDT = SS1_Sheet1.Cells[j, 23].Text.Trim();
                    strEICUOTTM = SS1_Sheet1.Cells[j, 24].Text.Trim();
                    strEICUPATH = SS1_Sheet1.Cells[j, 20].Text.Trim();
                    strHOSPDEPT = SS1_Sheet1.Cells[j, 21].Text.Trim();
                    strNEDSDEPT = SS1_Sheet1.Cells[j, 22].Text.Trim();
                    strKTASLEVL = SS1_Sheet1.Cells[j, 25].Text.Trim();
                    strIPDNO = SS1_Sheet1.Cells[j, 26].Text.Trim();
                    if (strIPDNO == "" && (strPTMIINDT != "" || strJedate != ""))
                    {
                        strIPDNO = GetIPDNO(strPTMIIDNO, strPTMIINDT, strJedate);
                    }
                  
                    strROWID = SS1_Sheet1.Cells[j, 27].Text.Trim();
                    strGUBUN = SS1_Sheet1.Cells[j, 28].Text.Trim();
                    strINPATH = SS1_Sheet1.Cells[j, 29].Text.Trim();


                    strPTMIINDT = strPTMIINDT.Replace("-", "");
                    strPTMIINTM = strPTMIINTM.Replace(":", "");
                    strPTMIOTDT = strPTMIOTDT.Replace("-", "");
                    strPTMIOTTM = strPTMIOTTM.Replace(":", "");
                    strEWRDINDT = strEWRDINDT.Replace("-", "");
                    strEWRDINTM = strEWRDINTM.Replace(":", "");
                    strEWRDOTDT = strEWRDOTDT.Replace("-", "");
                    strEWRDOTTM = strEWRDOTTM.Replace(":", "");
                    strEICUINDT = strEICUINDT.Replace("-", "");
                    strEICUINTM = strEICUINTM.Replace(":", "");
                    strEICUOTDT = strEICUOTDT.Replace("-", "");
                    strEICUOTTM = strEICUOTTM.Replace(":", "");

                    if (strROWID != "" && strGUBUN == "2")
                    {
                        SQL = " UPDATE KOSMOS_PMPA.NUR_ER_SPBOOK SET ";
                        SQL += ComNum.VBLF + " BEDNUM = '" + strBEDNUM + "',";
                        SQL += ComNum.VBLF + " ERGBN = '" + strERGBN + "',";
                        SQL += ComNum.VBLF + " DEPTGBN = '" + strDEPTGBN + "',";
                        SQL += ComNum.VBLF + " SNAME = '" + strSNAME + "',";
                        SQL += ComNum.VBLF + " PTMIIDNO = '" + strPTMIIDNO + "',";
                        SQL += ComNum.VBLF + " PTMIINDT = '" + strPTMIINDT + "',";
                        SQL += ComNum.VBLF + " PTMIINTM = '" + strPTMIINTM + "',";
                        SQL += ComNum.VBLF + " PTMIOTDT = '" + strPTMIOTDT + "',";
                        SQL += ComNum.VBLF + " PTMIOTTM = '" + strPTMIOTTM + "',";
                        SQL += ComNum.VBLF + " EWRDINDT = '" + strEWRDINDT + "',";
                        SQL += ComNum.VBLF + " EWRDINTM = '" + strEWRDINTM + "',";
                        SQL += ComNum.VBLF + " EWRDOTDT = '" + strEWRDOTDT + "',";
                        SQL += ComNum.VBLF + " EWRDOTTM = '" + strEWRDOTTM + "',";
                        SQL += ComNum.VBLF + " EWRDPATH = '" + strEWRDPATH + "',";
                        SQL += ComNum.VBLF + " EICUINDT = '" + strEICUINDT + "',";
                        SQL += ComNum.VBLF + " EICUINTM = '" + strEICUINTM + "',";
                        SQL += ComNum.VBLF + " EICUOTDT = '" + strEICUOTDT + "',";
                        SQL += ComNum.VBLF + " EICUOTTM = '" + strEICUOTTM + "',";
                        SQL += ComNum.VBLF + " EICUPATH = '" + strEICUPATH + "',";
                        SQL += ComNum.VBLF + " HOSPDEPT = '" + strHOSPDEPT + "',";
                        SQL += ComNum.VBLF + " NEDSDEPT = '" + strNEDSDEPT + "',";
                        SQL += ComNum.VBLF + " KTASLEVL = '" + strKTASLEVL + "',";
                        SQL += ComNum.VBLF + " IPDNO = '" + strIPDNO + "',";
                        SQL += ComNum.VBLF + " WRITEDATE = SYSDATE,";
                        SQL += ComNum.VBLF + " WRITESABUN = " + clsType.User.Sabun + ",";
                        SQL += ComNum.VBLF + " WARDCODE = '" + strWARDCODE + "',";
                        SQL += ComNum.VBLF + " INPATH = '" + strINPATH + "' ";
                        SQL += ComNum.VBLF + " WHERE ROWID = '" + strROWID + "'";
                    }
                    else if (strGUBUN == "1")
                    {
                        SQL = " INSERT INTO KOSMOS_PMPA.NUR_ER_SPBOOK( ";
                        SQL += ComNum.VBLF + " BEDNUM, ERGBN, DEPTGBN, SNAME, PTMIIDNO,";
                        SQL += ComNum.VBLF + " PTMIINDT, PTMIINTM, PTMIOTDT, PTMIOTTM, EWRDINDT,";
                        SQL += ComNum.VBLF + " EWRDINTM, EWRDOTDT, EWRDOTTM, EWRDPATH, EICUINDT,";
                        SQL += ComNum.VBLF + " EICUINTM, EICUOTDT, EICUOTTM, EICUPATH, HOSPDEPT,";
                        SQL += ComNum.VBLF + " NEDSDEPT, WARDCODE, WRITEDATE, WRITESABUN, IPDNO, KTASLEVL, INPATH) VALUES(";
                        SQL += ComNum.VBLF + "'" + strBEDNUM + "','" + strERGBN + "','" + strDEPTGBN + "','" + strSNAME + "','" + strPTMIIDNO + "',";
                        SQL += ComNum.VBLF + "'" + strPTMIINDT + "','" + strPTMIINTM + "','" + strPTMIOTDT + "','" + strPTMIOTTM + "','" + strEWRDINDT + "',";
                        SQL += ComNum.VBLF + "'" + strEWRDINTM + "','" + strEWRDOTDT + "','" + strEWRDOTTM + "','" + strEWRDPATH + "','" + strEICUINDT + "',";
                        SQL += ComNum.VBLF + "'" + strEICUINTM + "','" + strEICUOTDT + "','" + strEICUOTTM + "','" + strEICUPATH + "','" + strHOSPDEPT + "',";
                        SQL += ComNum.VBLF + "'" + strNEDSDEPT + "','" + strWARDCODE + "', SYSDATE, '" + clsType.User.Sabun + "'," + strIPDNO + ",'" + strKTASLEVL + "','" + strINPATH + "')";

                    }

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }


                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                READ_DATA();
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
        private string GetIPDNO(string argPtno, string argIndate, string argJedate)
        {

            string rtnVal = "0";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {

                SQL = " SELECT IPDNO FROM KOSMOS_PMPA.IPD_NEW_MASTER  ";
                SQL += ComNum.VBLF + " WHERE PANO = '" + argPtno + "'";
                if (argIndate != "")
                { 
                    SQL += ComNum.VBLF + "   AND INDATE >= TO_DATE('" + argIndate + " 00:00','YYYY-MM-DD HH24:MI') ";
                    SQL += ComNum.VBLF + "   AND INDATE <= TO_DATE('" + argIndate + " 23:59','YYYY-MM-DD HH24:MI') ";
                }
                else if(argJedate != "")
                {
                    SQL += ComNum.VBLF + "   AND INDATE <= TO_DATE('" + argJedate + " 23:59','YYYY-MM-DD HH24:MI') ";
                    SQL += ComNum.VBLF + "   AND (OUTDATE >= TO_DATE('" + argJedate + "','YYYY-MM-DD') OR OUTDATE IS NULL)";
                }
                else
                {
                    SQL += ComNum.VBLF + "  AND INDATE >= TRUNC(SYSDATE-200)";
                }
                SQL += ComNum.VBLF + " ORDER BY INDATE DESC ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return "0";
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["IPDNO"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                //clsDB.setRollbackTran(clsDB.DbCon);
                //ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return "0";
            }
        }

        private void btnAddRow_Click(object sender, EventArgs e)
        {
            SS1.ActiveSheet.AddRows(2, 1);
            SS1_Sheet1.Cells[2, 28].Text = "1";
        }


        private string READ_INDATE_ICU(string ArgPano, string ArgInDate, string ArgWardCode)
        {

            //int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strRtn = "";
            //string strWard = "";


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //SQL = " SELECT EXTRACTVALUE(b.CHARTXML, '//ta3') WardCode , to_char(to_date(min(a.CHARTDATE||a.CHARTTIME),'YYYY-MM-DD HH24:MI'),'YYYY-MM-DD HH24:MI')  CHARTDATE ";
                SQL = " SELECT EXTRACTVALUE(b.CHARTXML, '//ta3') WardCode , SUBSTR(MIN(A.CHARTTIME), 1, 4) CHARTDATE ";
                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRXMLMST A, KOSMOS_EMR.EMRXML B  ";
                SQL += ComNum.VBLF + " WHERE a.PTNO = '" + ArgPano + "' ";
                SQL += ComNum.VBLF + " AND a.FORMNO = '965'  AND a.emrno = b.emrno ";
                SQL += ComNum.VBLF + " AND EXTRACTVALUE(b.CHARTXML, '//ta3')  = '" + ArgWardCode + "' ";
                SQL += ComNum.VBLF + " AND a.MEDFRDATE = replace('" + ArgInDate + "','-','') ";
                SQL += ComNum.VBLF + "  group by EXTRACTVALUE(b.CHARTXML, '//ta3') ";

                SQL += ComNum.VBLF + " UNION ALL ";
                SQL += ComNum.VBLF + " SELECT B.WARDCODE, SUBSTR(MIN(A.CHARTTIME), 1, 4) CHARTDATE ";
                SQL += ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
                SQL += ComNum.VBLF + "   INNER JOIN " + ComNum.DB_EMR + "AEMRNURSRECORD B";
                SQL += ComNum.VBLF + "      ON A.EMRNO    = B.EMRNO";
                SQL += ComNum.VBLF + "     AND A.EMRNOHIS = B.EMRNOHIS";
                SQL += ComNum.VBLF + "     AND B.WARDCODE = '" + ArgWardCode + "' ";
                SQL += ComNum.VBLF + " WHERE a.PTNO = '" + ArgPano + "' ";
                SQL += ComNum.VBLF + "   AND a.FORMNO = 965";
                SQL += ComNum.VBLF + "   AND a.MEDFRDATE = '" + ArgInDate.Replace("-", "") + "'";
                SQL += ComNum.VBLF + " GROUP BY B.WARDCODE";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strRtn;
                }
                if (dt.Rows.Count > 0)
                {
                    strRtn = dt.Rows[0]["CHARTDATE"].ToString().Trim();

                }
                dt.Dispose();
                dt = null;
                return strRtn;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            return strRtn;
        }

        private string READ_INDATE_ICU2(string ArgPano, string ArgInDate, string ArgWardCode)
        {

            //int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strRtn = "";
            //string strWard = "";


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = " SELECT EXTRACTVALUE(b.CHARTXML, '//ta3') WardCode , to_char(to_date(min(a.CHARTDATE||a.CHARTTIME),'YYYY-MM-DD HH24:MI'),'YYYY-MM-DD HH24:MI')  CHARTDATE ";
                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRXMLMST A, KOSMOS_EMR.EMRXML B  ";
                SQL += ComNum.VBLF + " WHERE a.PTNO = '" + ArgPano + "' ";
                SQL += ComNum.VBLF + " AND a.FORMNO = '965'  AND a.emrno = b.emrno ";
                SQL += ComNum.VBLF + " AND EXTRACTVALUE(b.CHARTXML, '//ta3')  = '" + ArgWardCode + "' ";
                SQL += ComNum.VBLF + " AND a.MEDFRDATE =replace('" + ArgInDate + "','-','') ";
                SQL += ComNum.VBLF + "  group by EXTRACTVALUE(b.CHARTXML, '//ta3') ";

                SQL = SQL + ComNum.VBLF + " UNION ALL ";
                SQL = SQL + ComNum.VBLF + " SELECT B.WARDCODE, to_char(to_date(min(a.CHARTDATE|| SUBSTR(a.CHARTTIME, 0, 4)),'YYYY-MM-DD HH24:MI'),'YYYY-MM-DD HH24:MI')  CHARTDATE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
                SQL = SQL + ComNum.VBLF + "   INNER JOIN " + ComNum.DB_EMR + "AEMRNURSRECORD B";
                SQL = SQL + ComNum.VBLF + "      ON A.EMRNO    = B.EMRNO";
                SQL = SQL + ComNum.VBLF + "     AND A.EMRNOHIS = B.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "     AND B.WARDCODE = '" + ArgWardCode + "' ";
                SQL = SQL + ComNum.VBLF + " WHERE a.PTNO = '" + ArgPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.FORMNO = 965";
                SQL = SQL + ComNum.VBLF + "   AND a.MEDFRDATE = '" + ArgInDate.Replace("-", "") + "'";
                SQL = SQL + ComNum.VBLF + " GROUP BY B.WARDCODE";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strRtn;
                }
                if (dt.Rows.Count > 0)
                {
                    strRtn = dt.Rows[0]["CHARTDATE"].ToString().Trim();

                }
                dt.Dispose();
                dt = null;
                return strRtn;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            return strRtn;
        }

        private string READ_OUTDATE_ICU(string ArgPano, string ArgInDate, string ArgWardCode, string ArgOutDate)
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strRtn = "";
            //string strWard = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //SQL = " SELECT EXTRACTVALUE(b.CHARTXML, '//ta3') WardCode , to_char(to_date(max(a.CHARTDATE||a.CHARTTIME),'YYYY-MM-DD HH24:MI'),'YYYY-MM-DD HH24:MI')  CHARTDATE ";
                //SQL = " SELECT EXTRACTVALUE(b.CHARTXML, '//ta3') WardCode , SUBSTR(MAX(A.CHARTTIME), 1, 4) CHARTDATE ";
                SQL = " SELECT EXTRACTVALUE(b.CHARTXML, '//ta3') WardCode , SUBSTR(MAX(A.CHARTDATE || A.CHARTTIME), 1, 12) CHARTDATE ";
                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRXMLMST A, KOSMOS_EMR.EMRXML B  ";
                SQL += ComNum.VBLF + " WHERE a.PTNO = '" + ArgPano + "' ";
                SQL += ComNum.VBLF + " AND a.FORMNO = '965'  AND a.emrno = b.emrno ";
                SQL += ComNum.VBLF + " AND a.MEDFRDATE = replace('" + ArgInDate + "','-','') ";
                SQL += ComNum.VBLF + "   group by EXTRACTVALUE(b.CHARTXML, '//ta3') ";

                SQL = SQL + ComNum.VBLF + " UNION ALL ";
                SQL = SQL + ComNum.VBLF + " SELECT B.WARDCODE, SUBSTR(MAX(A.CHARTDATE || A.CHARTTIME), 1, 12) CHARTDATE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
                SQL = SQL + ComNum.VBLF + "   INNER JOIN " + ComNum.DB_EMR + "AEMRNURSRECORD B";
                SQL = SQL + ComNum.VBLF + "      ON A.EMRNO    = B.EMRNO";
                SQL = SQL + ComNum.VBLF + "     AND A.EMRNOHIS = B.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "     AND B.WARDCODE = '" + ArgWardCode + "' ";
                SQL = SQL + ComNum.VBLF + " WHERE a.PTNO = '" + ArgPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.FORMNO = 965";
                SQL = SQL + ComNum.VBLF + "   AND a.MEDFRDATE = '" + ArgInDate.Replace("-", "") + "'";
                SQL = SQL + ComNum.VBLF + " GROUP BY B.WARDCODE";
                SQL = SQL + ComNum.VBLF + " ORDER BY 2 DESC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strRtn;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows.Count == 1 && dt.Rows[i]["WardCode"].ToString().Trim() == ArgWardCode && ArgOutDate != "")
                        {
                            strRtn = VB.Right(dt.Rows[i]["CHARTDATE"].ToString().Trim(), 4);
                        }
                        else if (dt.Rows.Count > 1 && dt.Rows[i]["WardCode"].ToString().Trim() == ArgWardCode && i != 0)
                        {
                            strRtn = VB.Right(dt.Rows[i]["CHARTDATE"].ToString().Trim(), 4);
                        }
                        else if (dt.Rows.Count > 1 && dt.Rows[i]["WardCode"].ToString().Trim() == ArgWardCode && i == 0 && ArgOutDate != "")
                        {
                            strRtn = VB.Right(dt.Rows[i]["CHARTDATE"].ToString().Trim(), 4);
                        }
                    }
                }
                dt.Dispose();
                dt = null;
                return strRtn;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return strRtn;
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            int j = 0;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strBEDNUM = "";      //병상번호
            string strERGBN = "";       //응급전용여부
            string strDEPTGBN = "";     //진료과지정여부
            string strSNAME = "";       //이름
            string strPTMIIDNO = "";    //등록번호
            string strPTMIINDT = "";    //응급실-내원일자
            string strPTMIINTM = "";    //응급실-내원시간
            string strPTMIOTDT = "";    //응급실-퇴실일자
            string strPTMIOTTM = "";    //응급실-퇴실시간
            string strEWRDINDT = "";    //응급전용입원실-입원일자
            string strEWRDINTM = "";    //응급전용입원실-입원시간
            string strEWRDOTDT = "";    //응급전용입원실-퇴실일자
            string strEWRDOTTM = "";    //응급전용입원실-퇴실시간
            string strEWRDPATH = "";    //응급전용입원실-입실경로
            string strEICUINDT = "";    //중환자실,입원실-입실일자
            string strEICUINTM = "";    //중환자실,입원실-입실시간
            string strEICUOTDT = "";    //중환자실,입원실-입실경로
            string strEICUOTTM = "";    //중환자실,입원실-퇴실일자
            string strEICUPATH = "";    //중환자실,입원실-퇴실시간
            string strHOSPDEPT = "";    //주진료과(병원코드)
            string strNEDSDEPT = "";    //주진료과(NEDIS코드)
            string strWARDCODE = "";    //저장하는 부서
            string strIPDNO = "";
            string strGUBUN = "";

            string strKTASLEVL = "";

            string strDel = "";

            string strROWID = "";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            strWARDCODE = cboWARD.Text.Trim();

            if (strWARDCODE == "")
            {
                MessageBox.Show("병동을 선택 후 삭제하시기 바랍니다.");
                return;
            }


            try
            {
                for (i = 0; i < SS1_Sheet1.RowCount-2; i++)
                {
                    j = i + 2;
                    strDel = SS1_Sheet1.Cells[j, 0].Text.Trim();

                    if (strDel == "True")
                    {
                        strBEDNUM = SS1_Sheet1.Cells[j, 2].Text.Trim();
                        strERGBN = SS1_Sheet1.Cells[j, 3].Text.Trim();
                        strDEPTGBN = SS1_Sheet1.Cells[j, 5].Text.Trim();
                        strSNAME = SS1_Sheet1.Cells[j, 6].Text.Trim();
                        strPTMIIDNO = SS1_Sheet1.Cells[j, 7].Text.Trim();
                        strPTMIINDT = SS1_Sheet1.Cells[j, 8].Text.Trim();
                        strPTMIINTM = SS1_Sheet1.Cells[j, 9].Text.Trim();
                        strPTMIOTDT = SS1_Sheet1.Cells[j, 10].Text.Trim();
                        strPTMIOTTM = SS1_Sheet1.Cells[j, 11].Text.Trim();
                        strEWRDINDT = SS1_Sheet1.Cells[j, 12].Text.Trim();
                        strEWRDINTM = SS1_Sheet1.Cells[j, 13].Text.Trim();
                        strEWRDOTDT = SS1_Sheet1.Cells[j, 14].Text.Trim();
                        strEWRDOTTM = SS1_Sheet1.Cells[j, 15].Text.Trim();
                        strEWRDPATH = SS1_Sheet1.Cells[j, 16].Text.Trim();
                        strEICUINDT = SS1_Sheet1.Cells[j, 17].Text.Trim();
                        strEICUINTM = SS1_Sheet1.Cells[j, 19].Text.Trim();
                        strEICUOTDT = SS1_Sheet1.Cells[j, 23].Text.Trim();
                        strEICUOTTM = SS1_Sheet1.Cells[j, 24].Text.Trim();
                        strEICUPATH = SS1_Sheet1.Cells[j, 20].Text.Trim();
                        strHOSPDEPT = SS1_Sheet1.Cells[j, 21].Text.Trim();
                        strNEDSDEPT = SS1_Sheet1.Cells[j, 22].Text.Trim();
                        strKTASLEVL = SS1_Sheet1.Cells[j, 25].Text.Trim();
                        strIPDNO = SS1_Sheet1.Cells[j, 26].Text.Trim();
                        strROWID = SS1_Sheet1.Cells[j, 27].Text.Trim();
                        strGUBUN = SS1_Sheet1.Cells[j, 28].Text.Trim();

                        if (strGUBUN == "2" && strROWID != "")
                        {
                            SQL = " UPDATE KOSMOS_PMPA.NUR_ER_SPBOOK SET ";
                            SQL += ComNum.VBLF + " DELDATE = SYSDATE,";
                            SQL += ComNum.VBLF + " DELSABUN = " + clsType.User.Sabun + ",";
                            SQL += ComNum.VBLF + " WARDCODE = '" + strWARDCODE + "'";
                            SQL += ComNum.VBLF + " WHERE ROWID = '" + strROWID + "'";
                        }
                        else
                        {
                            SQL = " INSERT INTO KOSMOS_PMPA.NUR_ER_SPBOOK( ";
                            SQL += ComNum.VBLF + " BEDNUM, ERGBN, DEPTGBN, SNAME, PTMIIDNO,";
                            SQL += ComNum.VBLF + " PTMIINDT, PTMIINTM, PTMIOTDT, PTMIOTTM, EWRDINDT,";
                            SQL += ComNum.VBLF + " EWRDINTM, EWRDOTDT, EWRDOTTM, EWRDPATH, EICUINDT,";
                            SQL += ComNum.VBLF + " EICUINTM, EICUOTDT, EICUOTTM, EICUPATH, HOSPDEPT,";
                            SQL += ComNum.VBLF + " NEDSDEPT, WARDCODE, DELDATE, DELSABUN, KTASLEVL) VALUES(";
                            SQL += ComNum.VBLF + "'" + strBEDNUM + "','" + strERGBN + "','" + strDEPTGBN + "','" + strSNAME + "','" + strPTMIIDNO + "',";
                            SQL += ComNum.VBLF + "'" + strPTMIINDT + "','" + strPTMIINTM + "','" + strPTMIOTDT + "','" + strPTMIOTTM + "','" + strEWRDINDT + "',";
                            SQL += ComNum.VBLF + "'" + strEWRDINTM + "','" + strEWRDOTDT + "','" + strEWRDOTTM + "','" + strEWRDPATH + "','" + strEICUINDT + "',";
                            SQL += ComNum.VBLF + "'" + strEICUINTM + "','" + strEICUOTDT + "','" + strEICUOTTM + "','" + strEICUPATH + "','" + strHOSPDEPT + "',";
                            SQL += ComNum.VBLF + "'" + strNEDSDEPT + "','" + strWARDCODE + "', SYSDATE, " + clsType.User.Sabun + ",'" + strKTASLEVL + "')";

                        }
                    
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제하였습니다.");
                Cursor.Current = Cursors.Default;

                READ_DATA();
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

    }
}

