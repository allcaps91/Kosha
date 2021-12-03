using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : FrmAntiUSED2.cs
    /// Description     : 기간별 항생제 사용 내역 (W코드 및 향균제코드포함)
    /// Author          : 이정현
    /// Create Date     : 2018-04-02
    /// <history> 
    /// 기간별 항생제 사용 내역 (W코드 및 향균제코드포함)
    /// </history>
    /// <seealso>
    /// PSMH\Ocs\ipdocs\iorder\mtsiorder\FrmAntiUSED2.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\Ocs\ipdocs\iorder\mtsiorder.vbp
    /// </vbp>
    /// </summary>
    public partial class FrmAntiUSED2 : Form
    {
        string mPtno = "";
        string mSname = "";
            
        public FrmAntiUSED2()
        {
            InitializeComponent();
        }

        public FrmAntiUSED2(string pPtno, string pSname)
        {
            InitializeComponent();

            mPtno = pPtno;
            mSname = pSname;
        }

        private void FrmAntiUSED2_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.ColumnCount = 0;

            clsPublic.GnLogOutCNT = 0;

            if (mPtno == "")
            {
                ssPatientInfo_Sheet1.Cells[0, 0].Text = clsOrdFunction.Pat.PtNo;
                ssPatientInfo_Sheet1.Cells[0, 1].Text = clsOrdFunction.Pat.sName;
                ssPatientInfo_Sheet1.Cells[0, 2].Text = clsOrdFunction.Pat.Age + "/" + clsOrdFunction.Pat.Sex;
                ssPatientInfo_Sheet1.Cells[0, 3].Text = clsOrdFunction.Pat.INDATE;
                ssPatientInfo_Sheet1.Cells[0, 4].Text = clsOrdFunction.Pat.DeptCode;
                ssPatientInfo_Sheet1.Cells[0, 5].Text = clsOrdFunction.Pat.RoomCode;
                ssPatientInfo_Sheet1.Cells[0, 6].Text = clsOrdFunction.Pat.DrName;
            }
            else
            {
                string SQL = "";
                string SqlErr = "";
                DataTable dt = null;

                SQL = "SELECT PANO, SNAME, AGE, SEX, TO_CHAR(INDATE, 'YYYY-MM-DD') INDATE, DEPTCODE, DRCODE, ROOMCODE ";
                SQL += ComNum.VBLF + " FROM ADMIN.IPD_NEW_MASTER ";
                SQL += ComNum.VBLF + " WHERE  PANO = '" + mPtno + "' AND SNAME = '" + mSname + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    //ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    dt.Dispose();
                    dt = null;
                    return;
                }

                ssPatientInfo_Sheet1.Cells[0, 0].Text = dt.Rows[0]["PANO"].ToString().Trim();
                ssPatientInfo_Sheet1.Cells[0, 1].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                ssPatientInfo_Sheet1.Cells[0, 2].Text = dt.Rows[0]["AGE"].ToString().Trim() + "/" + dt.Rows[0]["SEX"].ToString().Trim(); 
                ssPatientInfo_Sheet1.Cells[0, 3].Text = dt.Rows[0]["INDATE"].ToString().Trim();
                ssPatientInfo_Sheet1.Cells[0, 4].Text = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                ssPatientInfo_Sheet1.Cells[0, 5].Text = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                ssPatientInfo_Sheet1.Cells[0, 6].Text = clsVbfunc.GetOCSDrCodeDrName(clsDB.DbCon, dt.Rows[0]["DRCODE"].ToString().Trim());
                dt.Dispose();
                dt = null;
            }


            ssView_Sheet1.RowCount = 2;

            dtpSDATE.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(-15);
            dtpEDATE.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            GetData();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            int i = 0;
            int k = 0;

            string strPano = "";
            string strInDate = "";
            string strMinDate = "";
            string strMaxDate = "";
            string strRows = "";

            int intDay = 0;

            clsPublic.GnLogOutCNT = 0;

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.ColumnCount = 0;

            strPano = ssPatientInfo_Sheet1.Cells[0, 0].Text.Trim();
            strInDate = ssPatientInfo_Sheet1.Cells[0, 3].Text.Trim();

            if (strPano.Trim() == "") { return; }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     /*+ index( ADMIN.ocs_iorder INXOCS_IORDER1) **/ Min(A.BDATE) AS MinDate, Max(A.BDATE) AS MaxDate, a.SuCode ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_IORDER A, " + ComNum.DB_PMPA + "BAS_SUN B";
                SQL = SQL + ComNum.VBLF + "     WHERE A.PTNO = '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "         AND A.BDATE >= TO_DATE('" + dtpSDATE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND A.BDATE <= TO_DATE('" + dtpEDATE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND A.BDATE >= TO_DATE('" + strInDate + "','YYYY-MM-DD') ";
                ///TODO : 이상훈 항생제 조건 통합 필요
                //SQL = SQL + ComNum.VBLF + "         AND TRIM(a.SuCode) IN (SELECT TRIM(CODE) FROM " + ComNum.DB_PMPA + "BAS_BCODE WHERE GUBUN = 'OCS_장기항생제코드' AND (DELDATE IS NULL OR DELDATE = '')) ";
                //SQL = SQL + ComNum.VBLF + "         AND TRIM(a.SuCode) IN ( SELECT TRIM(CODE) FROM " + ComNum.DB_PMPA + "BAS_BCODE WHERE GUBUN ='OCS_항생제코드' AND (DELDATE IS NULL OR DELDATE ='')  )  \r";
                //2021-06-24 조회 기준 약품마스터로 변경
                SQL = SQL + ComNum.VBLF + "         AND A.SUCODE IN ( SELECT JEPCODE FROM ADMIN.DRUG_MASTER2 WHERE SUB  IN (02, 07))  \r";
                SQL = SQL + ComNum.VBLF + "         AND A.SUCODE = B.SUNEXT ";
                SQL = SQL + ComNum.VBLF + "GROUP BY A.SUCODE ";
                SQL = SQL + ComNum.VBLF + "HAVING SUM(A.QTY * A.NAL) > 0";
                SQL = SQL + ComNum.VBLF + "ORDER BY MINDATE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strMinDate = Convert.ToDateTime(dt.Rows[0]["MINDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                    strMaxDate = dtpEDATE.Value.ToString("yyyy-MM-dd");

                    intDay = (int)VB.DateDiff("d", Convert.ToDateTime(strMinDate), Convert.ToDateTime(strMaxDate));

                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    
                    ssView_Sheet1.ColumnCount = intDay + 3;
                    ssView_Sheet1.SetColumnWidth(-1, 50);

                    ssView_Sheet1.Columns[0].Width = 80;
                    ssView_Sheet1.ColumnHeader.Cells[0, 0].Text = "약품코드";

                    ssView_Sheet1.Columns[1].Width = 220;
                    ssView_Sheet1.ColumnHeader.Cells[0, 1].Text = "약품명";

                    ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    ssView_Sheet1.Cells[0, 2, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;

                    strRows = "";

                    for (i = 0; i <= intDay; i++)
                    {
                        ssView_Sheet1.ColumnHeader.Cells[0, i + 2].Text = Convert.ToDateTime(strMinDate).AddDays(i).ToString("yyyy-MM-dd");

                        strRows = strRows + Convert.ToDateTime(strMinDate).AddDays(i).ToString("yyyy-MM-dd") + "," + (i + 3) + ";";
                    }

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     A.SUCODE, A.BDATE, A.DRCODE, SUM(A.QTY*A.NAL), A.GBIOE, B.SUNAMEK";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_IORDER A, " + ComNum.DB_PMPA + "BAS_SUN B";
                        SQL = SQL + ComNum.VBLF + "     WHERE A.BDATE >= TO_DATE('" + dtpSDATE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "         AND A.BDATE <= TO_DATE('" + dtpEDATE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "         AND A.BDATE >= TO_DATE('" + strInDate + "','YYYY-MM-DD') ";
                        //SQL = SQL + ComNum.VBLF + "         AND TRIM(a.SuCode) IN ( SELECT TRIM(CODE) FROM " + ComNum.DB_PMPA + "BAS_BCODE WHERE GUBUN = 'OCS_장기항생제코드' AND (DELDATE IS NULL OR DELDATE = '')) ";
                        //SQL = SQL + ComNum.VBLF + "         AND TRIM(a.SuCode) IN ( SELECT TRIM(CODE) FROM " + ComNum.DB_PMPA + "BAS_BCODE WHERE GUBUN = 'OCS_항생제코드' AND (DELDATE IS NULL OR DELDATE = '')) ";
                        //2021-06-24 조회 기준 약품마스터로 변경
                        SQL = SQL + ComNum.VBLF + "         AND A.SUCODE IN ( SELECT JEPCODE FROM ADMIN.DRUG_MASTER2 WHERE SUB  IN (02, 07))  \r";
                        SQL = SQL + ComNum.VBLF + "         AND A.PTNO = '" + strPano + "'";
                        SQL = SQL + ComNum.VBLF + "         AND A.SuCode = '" + dt.Rows[i]["SuCode"].ToString().Trim() + "'";
                        SQL = SQL + ComNum.VBLF + "         AND A.SUCODE = B.SUNEXT";
                        SQL = SQL + ComNum.VBLF + "GROUP BY A.SUCODE, A.BDATE, A.DRCODE, A.GBIOE, B.SUNAMEK";
                        SQL = SQL + ComNum.VBLF + "HAVING SUM(A.QTY * A.NAL) > 0";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            for (k = 0; k < dt1.Rows.Count; k++)
                            {
                                if (k == 0)
                                {
                                    ssView_Sheet1.Cells[i, 0].Text = dt1.Rows[k]["SUCODE"].ToString().Trim();
                                    ssView_Sheet1.Cells[i, 1].Text = dt1.Rows[k]["SUNAMEK"].ToString().Trim();
                                }

                                ssView_Sheet1.Cells[i, (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(strRows, Convert.ToDateTime(dt1.Rows[k]["BDATE"].ToString().Trim()).ToString("yyyy-MM-dd"), 2), ";", 1), ",", 2)) - 1].Text = "◎";
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
