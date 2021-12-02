using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : FrmMedicationUse.cs
    /// Description     : 기간별 처방 사용 내역 
    /// Author          : 이상훈 
    /// Create Date     : 2018-10-01
    /// </summary>
    public partial class FrmMedicationUse : Form
    {
        string FstrPtno;
        string FstrInDate;
        string FstrOrderCode;
        string FstrOrderName;

        public FrmMedicationUse()
        {
            InitializeComponent();
        }

        public FrmMedicationUse(string strPtno, string strInDate, string strOrderCode, string strOrderName)
        {
            InitializeComponent();

            FstrPtno = strPtno;
            FstrInDate = strInDate;
            FstrOrderCode = strOrderCode;
            FstrOrderName = strOrderName;
        }

        private void FrmMedicationUse_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            if (clsOrdFunction.Pat.INDATE == null)
            {
                Close();
                return;
            }

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.ColumnCount = 0;

            lblOrdName.Text = FstrOrderName;
             
            clsPublic.GnLogOutCNT = 0;

            ssPatientInfo_Sheet1.Cells[0, 0].Text = clsOrdFunction.Pat.PtNo;
            ssPatientInfo_Sheet1.Cells[0, 1].Text = clsOrdFunction.Pat.sName;
            ssPatientInfo_Sheet1.Cells[0, 2].Text = clsOrdFunction.Pat.Age + "/" + clsOrdFunction.Pat.Sex;
            ssPatientInfo_Sheet1.Cells[0, 3].Text = clsOrdFunction.Pat.INDATE;
            ssPatientInfo_Sheet1.Cells[0, 4].Text = clsOrdFunction.Pat.DeptCode;
            ssPatientInfo_Sheet1.Cells[0, 5].Text = clsOrdFunction.Pat.RoomCode;
            ssPatientInfo_Sheet1.Cells[0, 6].Text = clsOrdFunction.Pat.DrName;

            ssView_Sheet1.RowCount = 2;
            
            dtpSDATE.Text = DateTime.Parse(FstrInDate).ToShortDateString();
            dtpEDATE.Text = DateTime.Parse(clsPublic.GstrSysDate).ToShortDateString();

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
            
            string strMinDate = "";
            string strMaxDate = "";
            string strRows = "";

            int intDay = 0;

            clsPublic.GnLogOutCNT = 0;

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.ColumnCount = 0;
            if (FstrPtno.Trim() == "") { return; }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     /*+ index( kosmos_ocs.ocs_iorder INXOCS_IORDER1) **/ Min(A.BDATE) AS MinDate, Max(A.BDATE) AS MaxDate, a.SuCode ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_IORDER A, " + ComNum.DB_PMPA + "BAS_SUN B";
                SQL = SQL + ComNum.VBLF + "     WHERE A.PTNO = '" + FstrPtno + "' ";
                SQL = SQL + ComNum.VBLF + "         AND A.BDATE >= TO_DATE('" + dtpSDATE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND A.BDATE <= TO_DATE('" + dtpEDATE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND TRIM(a.ORDERCODE) = '" + FstrOrderCode.Trim() + "'  ";
                SQL = SQL + ComNum.VBLF + "         AND A.SUCODE = B.SUNEXT ";
                SQL = SQL + ComNum.VBLF + "         AND A.GBPRN <> 'P' ";
                SQL = SQL + ComNum.VBLF + "         AND A.GBSTATUS NOT IN('D', 'D-') ";
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
                    //strMaxDate = dtpEDATE.Value.ToString("yyyy-MM-dd");
                    strMaxDate = Convert.ToDateTime(dt.Rows[0]["MAXDATE"].ToString().Trim()).ToString("yyyy-MM-dd");

                    intDay = (int)VB.DateDiff("d", Convert.ToDateTime(strMinDate), Convert.ToDateTime(strMaxDate));

                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    ssView_Sheet1.ColumnCount = intDay + 3;
                    ssView_Sheet1.SetColumnWidth(-1, 50);

                    ssView_Sheet1.Columns[0].Width = 80;
                    ssView_Sheet1.ColumnHeader.Cells[0, 0].Text = "처방코드";

                    ssView_Sheet1.Columns[1].Width = 220;
                    ssView_Sheet1.ColumnHeader.Cells[0, 1].Text = "처방명";

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
                        SQL = SQL + ComNum.VBLF + "       AND A.BDATE <= TO_DATE('" + dtpEDATE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "       AND A.PTNO = '" + FstrPtno + "'";
                        SQL = SQL + ComNum.VBLF + "       AND A.SuCode = '" + dt.Rows[i]["SuCode"].ToString().Trim() + "'";
                        SQL = SQL + ComNum.VBLF + "       AND A.SUCODE = B.SUNEXT";
                        SQL = SQL + ComNum.VBLF + "       AND A.GBPRN <> 'P' ";
                        SQL = SQL + ComNum.VBLF + "       AND A.GBSTATUS NOT IN('D', 'D-') ";
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
