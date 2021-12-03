using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase;
using System.Text;

namespace ComLibB
{
    public partial class frmOcsCpTong_3 : Form, MainFormMessage

    {
        /// <summary>
        /// EMR 뷰어
        /// </summary>
        frmEmrViewer frmEmrViewerX = null;
        string fstrWard = "";

        #region //MainFormMessage
        string mPara1 = "";
        public MainFormMessage mCallForm = null;
        public void MsgActivedForm(Form frm)
        {

        }
        public void MsgUnloadForm(Form frm)
        {

        }
        public void MsgFormClear()
        {

        }
        public void MsgSendPara(string strPara)
        {

        }
        #endregion //MainFormMessage

        public frmOcsCpTong_3(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
        }

        public frmOcsCpTong_3()
        {
            InitializeComponent();
        }

        private void frmOcsCpTong_3_Load(object sender, EventArgs e)
        {
            GetDataCpCode();
        }

        /// <summary>
        /// CP기초코드 정보 가져오기
        /// </summary>
        private void GetDataCpCode()
        {
            ComFunc CF = new ComFunc();
           
            ComFunc.ReadSysDate(clsDB.DbCon);
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            clsVbfunc.SetComboDept(clsDB.DbCon, cboDeptcode, "1", 2);
            ComboWard_SET();

            if (fstrWard == "")
            {
                fstrWard = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "");
            }

            if (clsType.User.JobGroup != "JOB016002" && clsType.User.JobGroup != "JOB000001")
            {
               
                cboWard.Enabled = false;
                cboWard.Items.Clear();
                cboWard.Items.Add(fstrWard);
                cboWard.SelectedIndex = 0;

                cboWard2.Enabled = false;
                cboWard2.Items.Clear();
                cboWard2.Items.Add(fstrWard);
                cboWard2.SelectedIndex = 0;

                rbtnGubun1.Enabled = false;
                rbtnGubun2.Enabled = false;
                dtpFrDate2.Text = VB.Left(clsPublic.GstrSysDate, 7) + "-01";
                dtpToDate2.Text = CF.READ_LASTDAY(clsDB.DbCon, dtpFrDate2.Text.Trim());
            }


            cboMonth.Items.Clear();
            SQL = " SELECT ";
            SQL = SQL + ComNum.VBLF + "  TO_CHAR(ADD_MONTHS(SYSDATE, -1 * (LEVEL - 1)), 'YYYY-MM') YYMM ";
            SQL = SQL + ComNum.VBLF + "    FROM DUAL ";
            SQL = SQL + ComNum.VBLF + " CONNECT BY LEVEL< 24 ";
            SQL = SQL + ComNum.VBLF + "   ORDER BY YYMM DESC ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (string.IsNullOrEmpty(SqlErr) == false)
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboMonth.Items.Add(dt.Rows[i]["YYMM"].ToString().Trim());
                }
                cboMonth.SelectedIndex = 0;
            }
            dt.Dispose();
            dt = null;

            cboYear.Items.Clear();
            SQL = " SELECT ";
            SQL = SQL + ComNum.VBLF + "  TO_CHAR(ADD_MONTHS(SYSDATE, -12 * (LEVEL - 1)), 'YYYY') YYYY ";
            SQL = SQL + ComNum.VBLF + "    FROM DUAL ";
            SQL = SQL + ComNum.VBLF + " CONNECT BY LEVEL < 5 ";
            SQL = SQL + ComNum.VBLF + "   ORDER BY YYYY DESC ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (string.IsNullOrEmpty(SqlErr) == false)
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboYear.Items.Add(dt.Rows[i]["YYYY"].ToString().Trim());
                }
                cboYear.SelectedIndex = 0;
            }
            dt.Dispose();
            dt = null;
           
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnTongSearch_Click(object sender, EventArgs e)
        {
            GetCpStatistics();
        }

        private void ComboWard_SET()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT WARDCODE, WARDNAME  ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.BAS_WARD ";
                SQL = SQL + ComNum.VBLF + "WHERE WARDCODE NOT IN ('IU','NP','2W','IQ','ER') ";
                SQL = SQL + ComNum.VBLF + "     AND USED = 'Y'  ";
                SQL = SQL + ComNum.VBLF + "ORDER BY WARDCODE ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                cboWard2.Items.Clear();
                cboWard.Items.Clear();

                if (dt.Rows.Count > 0)
                {
                    cboWard2.Items.Add("**");
                    cboWard.Items.Add("**");

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboWard2.Items.Add(dt.Rows[i]["WARDCODE"].ToString().Trim());
                        cboWard.Items.Add(dt.Rows[i]["WARDCODE"].ToString().Trim());
                    }
                    cboWard2.SelectedIndex = 0;
                    cboWard.SelectedIndex = 0;
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        void GetCpStatistics()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            //DataTable dt2 = null;

            string strDate = dtpFrDate2.Value.ToShortDateString();
            string strDate2 = dtpToDate2.Value.ToShortDateString();

            ssStatistics_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT ";
                SQL = SQL + ComNum.VBLF + "  (SELECT BASNAME  FROM ADMIN.BAS_BASCD WHERE GRPCDB = 'CP관리' AND GRPCD = 'CP코드관리' AND BASCD = A.CPCODE_T AND ROWNUM = 1) CPNAME,  ";
                SQL = SQL + ComNum.VBLF + "  (SELECT vflag1 FROM ADMIN.BAS_BASCD WHERE GRPCDB = 'CP관리' AND GRPCD = 'CP코드관리' AND BASCD = A.CPCODE_T AND ROWNUM = 1) vflag1,  ";
                SQL = SQL + ComNum.VBLF + "  CPCODE_T, TONG1, TONG2, TONG3, TONG6,TONG4, TONG5,  ";
                SQL = SQL + ComNum.VBLF + "  case when  TONG2 > 0 then                               ";
                SQL = SQL + ComNum.VBLF + " ROUND(((TONG2 / (TONG2 + TONG6 )) * 100), 1)  ";
                SQL = SQL + ComNum.VBLF + " else 0  end  TONG7 , ";
                SQL = SQL + ComNum.VBLF + " case when  TONG5 > 0 then  ";
                SQL = SQL + ComNum.VBLF + " ROUND(((TONG5 / (TONG2)) * 100), 1) ";
                SQL = SQL + ComNum.VBLF + " else 0  end  TONG8 ";
                SQL = SQL + ComNum.VBLF + " FROM ( ";
                SQL = SQL + ComNum.VBLF + "    SELECT CPCODE_T, SUM(1) TONG1, "; //전체대상건수 
                SQL = SQL + ComNum.VBLF + "    SUM(CASE WHEN CP_STATUS IN('적용', '중단') THEN 1 ELSE 0 END) TONG2, "; //CP적용건수
                SQL = SQL + ComNum.VBLF + "    SUM(CASE WHEN(CP_STATUS IN('제외', '') OR CP_STATUS IS NULL) THEN 1 ELSE 0 END) TONG3, "; // CP제외건수
                SQL = SQL + ComNum.VBLF + "    SUM(CASE WHEN CP_STATUS IN('중단') THEN 1 ELSE 0 END) TONG4, ";  //--CP중단건수
                SQL = SQL + ComNum.VBLF + "    SUM(CASE WHEN CP_STATUS IN('적용') THEN 1 ELSE 0 END) TONG5, ";  // --CP완결건수
                SQL = SQL + ComNum.VBLF + "    SUM(CASE WHEN CP_STATUS IN('미적용') THEN 1 ELSE 0 END) TONG6 ";  // --CP완결건수
                SQL = SQL + ComNum.VBLF + "     FROM ADMIN.OCS_CP_IPD_LIST2 A, IPD_NEW_MASTER B";
                SQL = SQL + ComNum.VBLF + "    WHERE A.OUTDATE >= TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "      AND A.OUTDATE <= TO_DATE('" + strDate2 + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "      AND A.IPDNO=B.IPDNO ";
                if (cboWard2.Text.Trim() != "**")
                {
                    SQL += ComNum.VBLF + "       AND B.WARDCODE = '" + cboWard2.Text.Trim() + "' ";
                }
                SQL = SQL + ComNum.VBLF + "    GROUP BY CPCODE_T ";
                SQL = SQL + ComNum.VBLF + "    ) A ";
                SQL = SQL + ComNum.VBLF + " ORDER BY CPCODE_T ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
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

                ssStatistics_Sheet1.RowCount = dt.Rows.Count;
                ssStatistics_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssStatistics_Sheet1.Cells[i, 0].Text = dt.Rows[i]["vflag1"].ToString().Trim();
                    ssStatistics_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CPNAME"].ToString().Trim();
                    ssStatistics_Sheet1.Cells[i, 2].Text = dt.Rows[i]["TONG1"].ToString().Trim();
                    ssStatistics_Sheet1.Cells[i, 3].Text = dt.Rows[i]["TONG2"].ToString().Trim();
                    ssStatistics_Sheet1.Cells[i, 4].Text = dt.Rows[i]["TONG3"].ToString().Trim();
                    ssStatistics_Sheet1.Cells[i, 5].Text = dt.Rows[i]["TONG6"].ToString().Trim();
                    ssStatistics_Sheet1.Cells[i, 6].Text = dt.Rows[i]["TONG4"].ToString().Trim();
                    ssStatistics_Sheet1.Cells[i, 7].Text = dt.Rows[i]["TONG5"].ToString().Trim();
                    ssStatistics_Sheet1.Cells[i, 8].Text = dt.Rows[i]["TONG7"].ToString().Trim();
                    ssStatistics_Sheet1.Cells[i, 9].Text = dt.Rows[i]["TONG8"].ToString().Trim();
                    //ssStatistics_Sheet1.Cells[i, 6].Text = dt.Rows[i]["TONG6"].ToString().Trim();
                    //ssStatistics_Sheet1.Cells[i, 7].Text = dt.Rows[i]["TONG7"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }           

        string GetCodeList(string strCpCode, string strGubun)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            StringBuilder strVal = new StringBuilder();
            string strCompre = strGubun == "04" ? "B.ILLCODE IN(" : "C.SUCODE IN("; //진단일 경우 진단코드 아닐경우 수가코드(수술)

            try
            {
                SQL = "SELECT CODE";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_CP_SUB";
                SQL += ComNum.VBLF + "WHERE CPCODE = '" + strCpCode + "'";
                SQL += ComNum.VBLF + "  AND GUBUN  = '" + strGubun + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strVal.ToString().Trim();
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return strVal.ToString().Trim();
                }

                strVal.AppendLine("AND " + strCompre);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strVal.AppendLine( "	'" + dt.Rows[i]["Code"].ToString().Trim() + "' " + (i < dt.Rows.Count - 1 ? "," : ")"));
                }

                dt.Dispose();
                dt = null;


                return strVal.ToString().Trim();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return strVal.ToString().Trim();
            }
        }

        void strGetAge(string strCpCode, ref string strFrAge, ref string strToAge)
        {
            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty;
            DataTable dt = null;

            try
            {
                SQL.AppendLine("SELECT FRAGE, TOAGE");
                SQL.AppendLine("FROM " + ComNum.DB_MED + "OCS_CP_MAIN");
                SQL.AppendLine("WHERE CPCODE = '" + strCpCode + "'");

                SqlErr = clsDB.GetDataTable(ref dt, SQL.ToString().Trim(), clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                strFrAge = dt.Rows[0]["FRAGE"].ToString().Trim();
                strToAge = dt.Rows[0]["TOAGE"].ToString().Trim();

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
            }
        }
              
      
        private void ssStatistics_TextTipFetch(object sender, FarPoint.Win.Spread.TextTipFetchEventArgs e)
        {
            if(e.ColumnHeader == true || e.RowHeader == true)
            {
                return;
            }

            e.ShowTip = true;

            switch(e.Column)
            {
                case 3:
                    e.TipText = "해당 CP 전체 대상자";
                    break;
                case 4:
                    e.TipText = "QI실에서 적용한 대상자";
                    break;
                case 5:
                    e.TipText = "제외 한 수";
                    break;
                case 6:
                    e.TipText = "중단한 수";
                    break;
                case 7:
                    e.TipText = "CP가 적용 된 후 제외/중단 없이 퇴원한 환자 ";
                    break;
            }
        }

        private void frmOcsCpTong_3_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmEmrViewerX != null)
            {
                frmEmrViewerX.Close();
                frmEmrViewerX = null;
            }
            if (mCallForm != null)
            {
                mCallForm.MsgUnloadForm(this);
            }
        }

        private void btnView2_Click(object sender, EventArgs e)
        {
            GetTong01();
        }


        private void GetTong01()
        {

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            int i = 0;
            int j = 0;
            int k = 0;

            string strDate = "";
            string strDate2 = "";

            string strCPCODE = "";

            ComFunc cf = new ComFunc();

            if(rbtnYear.Checked == true)
            {
                strDate = cboYear.Text.Trim() + "01-01";
                strDate2 = cboYear.Text.Trim() + "12-31";

                SS1_Sheet1.ColumnCount = 15;
                SS1_Sheet1.SetColumnLabel(0, 14, "합계");

                for (i = 1; i <= 12; i++)
                {
                    SS1_Sheet1.SetColumnLabel(0, i + 1, i.ToString());
                }
            }
            else if(rbtnMonth.Checked == true)
            {
                strDate = cboMonth.Text.Trim() + "-01";
                strDate2 = cf.READ_LASTDAY(clsDB.DbCon, strDate);

                SS1_Sheet1.ColumnCount = 34;
                SS1_Sheet1.SetColumnLabel(0, 33, "합계");

                for (i = 1; i <= 31; i++)
                {
                    SS1_Sheet1.SetColumnLabel(0, i + 1, i.ToString());
                }
            }
            else
            {
                strDate = dtpSDATE.Text.Trim();
                strDate2 = dtpEDATE.Text.Trim();

                j = cf.DATE_ILSU(clsDB.DbCon, strDate2, strDate);

                SS1_Sheet1.ColumnCount = j + 4;
                SS1_Sheet1.SetColumnLabel(0, j + 3, "합계");

                for (i = 0; i <= j; i++)
                {
                    SS1_Sheet1.SetColumnLabel(0, i + 2, cf.DATE_ADD(clsDB.DbCon,strDate, i).Substring(5, 5).Replace("-",ComNum.VBLF));
                }

            }

            SS1_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {

                SQL = " SELECT CPCODE ";
                SQL += ComNum.VBLF + "   FROM ADMIN.OCS_CP_IPD_LIST2 A ";
                SQL += ComNum.VBLF + "  WHERE OUTDATE >= TO_DATE('" + strDate + "', 'YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND OUTDATE <= TO_DATE('" + strDate2 + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + " GROUP BY CPCODE ";
                SQL += ComNum.VBLF + " ORDER BY CPCODE ASC ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
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

                SS1_Sheet1.RowCount = dt.Rows.Count;
                SS1_Sheet1.SetRowHeight(-1, 28);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    SS1_Sheet1.Cells[i, 0].Text = ReadCPBASNAME("CP코드관리", dt.Rows[i]["CPCODE"].ToString().Trim());
                    SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CPCODE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                for (i = 0; i < SS1_Sheet1.RowCount; i++)
                {
                    strCPCODE = SS1_Sheet1.Cells[i, 1].Text.Trim();

                    if (rbtnYear.Checked == true)
                    {
                        SQL = "SELECT TO_CHAR(A.OUTDATE,'MM') DAY, SUM(1) CNT ";
                    }
                    else if (rbtnMonth.Checked == true)
                    {
                        SQL = "SELECT TO_CHAR(A.OUTDATE,'DD') DAY, SUM(1) CNT ";
                    }
                    else if(rbtnDate.Checked == true)
                    {
                        SQL = "SELECT TO_CHAR(A.OUTDATE,'MMDD') DAY, SUM(1) CNT ";
                    }

                    //SQL = "SELECT " + (rbtnYear.Checked == true ? "TO_CHAR(A.OUTDATE,'MM')" : "TO_CHAR(A.OUTDATE,'MMDD') ") + " DAY, SUM(1) CNT ";
                    SQL += ComNum.VBLF + "  FROM ADMIN.OCS_CP_IPD_LIST2 A, ADMIN.IPD_NEW_MASTER B ";
                    SQL += ComNum.VBLF + " WHERE A.OUTDATE >= TO_DATE('" + strDate + "', 'YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "   AND A.OUTDATE <= TO_DATE('" + strDate2 + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "   AND A.CPCODE_T = '" + strCPCODE + "' ";
                    SQL += ComNum.VBLF + "   AND A.IPDNO = B.IPDNO ";
                    if (rbtnGubun1.Checked == true)
                    {
                        //대상 전체
                    }
                    else if (rbtnGubun2.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   AND A.CP_STATUS IN ('적용','중단') ";
                    }
                    else if (rbtnGubun3.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   AND (A.CP_STATUS IN ('제외','') OR A.CP_STATUS IS NULL) ";
                    }
                    else if (rbtnGubun4.Checked == true)
                    {
                        SQL += ComNum.VBLF + "   AND (A.CP_STATUS IN ('중단')";
                    }


                    if (cboWard.Text != "**")
                    {
                        SQL += ComNum.VBLF + "       AND B.WARDCODE = '" + cboWard.Text.Trim() + "' ";
                    }
                    if (cboDeptcode.Text != "**")
                    {
                        SQL += ComNum.VBLF + "       AND B.DEPTCODE = '" + cboDeptcode.Text.Trim() + "' ";
                    }
                    if (rbtnYear.Checked == true)
                    {
                        SQL += ComNum.VBLF + "GROUP BY A.CPCODE, TO_CHAR(A.OUTDATE,'MM') ";
                    }
                    else if (rbtnMonth.Checked == true)
                    {
                        SQL += ComNum.VBLF + "GROUP BY A.CPCODE, TO_CHAR(A.OUTDATE,'DD') ";
                    }
                    else if (rbtnDate.Checked == true)
                    {
                        SQL += ComNum.VBLF + "GROUP BY A.CPCODE, TO_CHAR(A.OUTDATE,'MMDD') ";
                    }

                    //SQL += ComNum.VBLF + " GROUP BY A.CPCODE, " + (rbtnYear.Checked == true ? "TO_CHAR(A.OUTDATE, 'MM')" : "TO_CHAR(A.OUTDATE, 'MMDD') ");
                    SQL += ComNum.VBLF + " ORDER BY DAY ASC ";
                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                    if (string.IsNullOrEmpty(SqlErr) == false)
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
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

                    

                    for (j = 0; j < dt.Rows.Count; j++)
                    {
                        for (k = 2; k < SS1_Sheet1.ColumnCount; k++)
                        {
                            if (VB.Val(SS1_Sheet1.GetColumnLabel(0, k).ToString().Replace(ComNum.VBLF, "")) == VB.Val(dt.Rows[j]["DAY"].ToString()))
                            {
                                SS1_Sheet1.Cells[i, k].Text = dt.Rows[j]["CNT"].ToString().Trim();
                            }
                        }
                        
                    }

                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private string ReadCPBASNAME(string strGRPCD, string strBASCD)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strRTN = "";

            try
            {
                SQL = "  SELECT BASNAME ";
                SQL += ComNum.VBLF + "   FROM ADMIN.BAS_BASCD ";
                SQL += ComNum.VBLF + "  WHERE GRPCDB = 'CP관리' ";
                SQL += ComNum.VBLF + "    AND GRPCD = '" + strGRPCD + "' ";
                SQL += ComNum.VBLF + "    AND BASCD = '" + strBASCD + "' ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return "";
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return "";
                }

                strRTN = dt.Rows[0]["BASNAME"].ToString().Trim();

                dt.Dispose();
                dt = null;

                return strRTN;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return "";
            }
        }

        private void frmOcsCpTong_3_Activated(object sender, EventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgActivedForm(this);
            }
        }
    }
}
