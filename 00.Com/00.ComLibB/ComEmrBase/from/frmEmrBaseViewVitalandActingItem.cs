using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ComBase;

namespace ComEmrBase
{
    public partial class frmEmrBaseViewVitalandActingItem : Form
    {
        string mJOBGB = "IVT";

        public frmEmrBaseViewVitalandActingItem()
        {
            InitializeComponent();
        }

        public frmEmrBaseViewVitalandActingItem(string pJOBGB)
        {
            InitializeComponent();
            mJOBGB = pJOBGB;
        }

        private void frmEmrBaseViewVitalandActingItem_Load(object sender, EventArgs e)
        {
            ssForm_Sheet1.RowCount = 0;
            ssFormUser_Sheet1.RowCount = 0;

            if (clsType.User.IdNumber == "15273" || clsType.User.IdNumber == "13635")
            {
                panUseGb.Visible = true;
            }

            SaveDataDeptToUser("IVT");
            SaveDataDeptToUser("ACT");
            SaveDataDeptToUser("SP");
            SaveDataDeptToUser("SP2");
            SaveDataDeptToUser("WD");
            SaveDataDeptToUser("BD");
            SaveDataDeptToUser("CVC");

            if (mJOBGB == "ACT")
            {
                optAct.Checked = true;
            }
            else if (mJOBGB == "IVT")
            {
                optVital.Checked = true;
            }
            else if (mJOBGB.IndexOf("SP") != -1)
            {
                optSpecial.Checked = true;
            }
            else if (mJOBGB == "WD")
            {
                optWound.Checked = true;
            }
            else if (mJOBGB == "BD")
            {
                optBedsore.Checked = true;
            }
            else if (mJOBGB == "CVC")
            {
                optCvc.Checked = true;
            }
        }

        /// <summary>
        /// 기록지 아이템조회
        /// </summary>
        private void FormSearch()
        {
            ssForm_Sheet1.RowCount = 0;

            if (mJOBGB == "IVT")
            {
                FormSearchIVT(ssForm_Sheet1);
            }
            else if (mJOBGB == "ACT")
            {
                FormSearchAct(ssForm_Sheet1);
            }
            else if (mJOBGB.IndexOf("SP") != -1 )
            {
                FormSearchSp(ssForm_Sheet1, mJOBGB);
            }
            else if (mJOBGB == "WD")
            {
                FormSearchWd(ssForm_Sheet1);
            }
            else if (mJOBGB == "BD")
            {
                FormSearchBd(ssForm_Sheet1);
            }
            else if (mJOBGB == "CVC")
            {
                FormSearchCvc(ssForm_Sheet1);
            }

            FormSearchUser();
        }

        /// <summary>
        /// 사용자 아이템 조회
        /// </summary>
        private void FormSearchUser()
        {
            ssFormUser_Sheet1.RowCount = 0;

            if (mJOBGB == "IVT")
            {
                FormSearchIVT(ssFormUser_Sheet1);
            }
            else if (mJOBGB == "ACT")
            {
                FormSearchAct(ssFormUser_Sheet1);
            }
            else if (mJOBGB.IndexOf("SP") != -1)
            {
                FormSearchSp(ssFormUser_Sheet1, mJOBGB);
            }
            else if (mJOBGB == "WD")
            {
                FormSearchWd(ssFormUser_Sheet1);
            }
            else if (mJOBGB == "BD")
            {
                FormSearchBd(ssFormUser_Sheet1);
            }
            else if (mJOBGB == "CVC")
            {
                FormSearchCvc(ssFormUser_Sheet1);
            }
        }

        /// <summary>
        /// 중심정매관 항목조회
        /// </summary>
        private void FormSearchCvc(FarPoint.Win.Spread.SheetView Spd)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            Spd.RowCount = 0;

            SQL = "";
            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "     B.ITEMNO, B.ITEMNAME, BB.FORMNAME ";
            if (Spd == ssFormUser_Sheet1)
            {
                SQL = SQL + ComNum.VBLF + "     , (SELECT U1.ITEMCD ";
                SQL = SQL + ComNum.VBLF + "         FROM KOSMOS_EMR.AEMRUSERITEMVS U1 ";
                SQL = SQL + ComNum.VBLF + "         WHERE U1.JOBGB = '" + mJOBGB + "'";
                SQL = SQL + ComNum.VBLF + "            AND U1.USEGB = '" + clsType.User.BuseCode + "'";
                SQL = SQL + ComNum.VBLF + "            AND U1.ITEMCD = B.ITEMNO ";
                SQL = SQL + ComNum.VBLF + "     ) AS BUSEITEM  ";
            }
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRFLOWXML B";
            SQL = SQL + ComNum.VBLF + "   INNER JOIN " + ComNum.DB_EMR + "AEMRFORM BB";
            SQL = SQL + ComNum.VBLF + "      ON BB.FORMNO     = B.FORMNO ";
            SQL = SQL + ComNum.VBLF + "     AND BB.UPDATENO   = B.UPDATENO";
            SQL = SQL + ComNum.VBLF + "WHERE B.FORMNO     IN(2638)";
            SQL = SQL + ComNum.VBLF + "  AND B.UPDATENO   = 2";
            SQL = SQL + ComNum.VBLF + "  AND B.ITEMNUMBER >= 0 ";

            if (Spd == ssFormUser_Sheet1)
            {
                SQL = SQL + ComNum.VBLF + "     AND EXISTS (SELECT 1 FROM " + ComNum.DB_EMR + "AEMRUSERITEMVS U";
                SQL = SQL + ComNum.VBLF + "                 WHERE U.JOBGB = '" + mJOBGB + "'";
                if (optDept.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "                        AND U.USEGB = '" + clsType.User.BuseCode + "'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "                        AND U.USEGB = '" + clsType.User.IdNumber + "'";
                }
                SQL = SQL + ComNum.VBLF + "                        AND U.ITEMCD = B.ITEMNO";
                SQL = SQL + ComNum.VBLF + "                 )";
            }

            SQL = SQL + ComNum.VBLF + "ORDER BY B.FORMNO, B.ITEMNUMBER";
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

            string strBASEXNAME = "";
            int intS = 0;

            Spd.RowCount = dt.Rows.Count;
            Spd.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Spd.Cells[i, 0].Text = dt.Rows[i]["ITEMNO"].ToString().Trim();
                Spd.Cells[i, 1].Text = dt.Rows[i]["FORMNAME"].ToString().Trim();
                Spd.Cells[i, 1].Font = new Font("맑은 고딕", 9.7f, FontStyle.Bold);

                Spd.Cells[i, 2].Text = dt.Rows[i]["ITEMNAME"].ToString().Trim();
                if (Spd == ssFormUser_Sheet1)
                {
                    if (dt.Rows[i]["ITEMNAME"].ToString().Trim() != "")
                    {
                        Spd.Cells[i, 2].Font = new Font("맑은 고딕", 9.7f, FontStyle.Bold);
                    }
                }

                if (strBASEXNAME != dt.Rows[i]["FORMNAME"].ToString().Trim())
                {
                    Spd.Cells[i, 1].Text = dt.Rows[i]["FORMNAME"].ToString().Trim();
                    if (i != 0)
                    {
                        Spd.AddSpanCell(intS, 1, i - intS, 1);
                    }
                    intS = i;
                }
                strBASEXNAME = dt.Rows[i]["FORMNAME"].ToString().Trim();
            }
            Spd.AddSpanCell(intS, 1, i - intS, 1);

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// 욕창 항목조회
        /// </summary>
        private void FormSearchBd(FarPoint.Win.Spread.SheetView Spd)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            Spd.RowCount = 0;

            SQL = "";
            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "     B.ITEMNO, B.ITEMNAME, BB.FORMNAME ";
            if (Spd == ssFormUser_Sheet1)
            {
                SQL = SQL + ComNum.VBLF + "     , (SELECT U1.ITEMCD ";
                SQL = SQL + ComNum.VBLF + "         FROM KOSMOS_EMR.AEMRUSERITEMVS U1 ";
                SQL = SQL + ComNum.VBLF + "         WHERE U1.JOBGB = '" + mJOBGB + "'";
                SQL = SQL + ComNum.VBLF + "            AND U1.USEGB = '" + clsType.User.BuseCode + "'";
                SQL = SQL + ComNum.VBLF + "            AND U1.ITEMCD = B.ITEMNO ";
                SQL = SQL + ComNum.VBLF + "     ) AS BUSEITEM  ";
            }
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRFLOWXML B";
            SQL = SQL + ComNum.VBLF + "   INNER JOIN " + ComNum.DB_EMR + "AEMRFORM BB";
            SQL = SQL + ComNum.VBLF + "      ON BB.FORMNO     = B.FORMNO ";
            SQL = SQL + ComNum.VBLF + "     AND BB.UPDATENO   = B.UPDATENO";
            SQL = SQL + ComNum.VBLF + "     AND BB.USECHECK = 1";
            SQL = SQL + ComNum.VBLF + "WHERE B.FORMNO     = 1573 ";
            SQL = SQL + ComNum.VBLF + "  AND B.UPDATENO   > 0";
            SQL = SQL + ComNum.VBLF + "  AND B.ITEMNUMBER >= 0 ";

            if (Spd == ssFormUser_Sheet1)
            {
                SQL = SQL + ComNum.VBLF + "     AND EXISTS (SELECT 1 FROM " + ComNum.DB_EMR + "AEMRUSERITEMVS U";
                SQL = SQL + ComNum.VBLF + "                 WHERE U.JOBGB = '" + mJOBGB + "'";
                if (optDept.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "                        AND U.USEGB = '" + clsType.User.BuseCode + "'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "                        AND U.USEGB = '" + clsType.User.IdNumber + "'";
                }
                SQL = SQL + ComNum.VBLF + "                        AND U.ITEMCD = B.ITEMNO";
                SQL = SQL + ComNum.VBLF + "                 )";
            }

            SQL = SQL + ComNum.VBLF + "ORDER BY B.ITEMNUMBER";
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

            string strBASEXNAME = "";
            int intS = 0;

            Spd.RowCount = dt.Rows.Count;
            Spd.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Spd.Cells[i, 0].Text = dt.Rows[i]["ITEMNO"].ToString().Trim();
                Spd.Cells[i, 1].Text = dt.Rows[i]["FORMNAME"].ToString().Trim();
                Spd.Cells[i, 1].Font = new Font("맑은 고딕", 9.7f, FontStyle.Bold);

                Spd.Cells[i, 2].Text = dt.Rows[i]["ITEMNAME"].ToString().Trim();
                if (Spd == ssFormUser_Sheet1)
                {
                    if (dt.Rows[i]["ITEMNAME"].ToString().Trim() != "")
                    {
                        Spd.Cells[i, 2].Font = new Font("맑은 고딕", 9.7f, FontStyle.Bold);
                    }
                }

                if (strBASEXNAME != dt.Rows[i]["FORMNAME"].ToString().Trim())
                {
                    Spd.Cells[i, 1].Text = dt.Rows[i]["FORMNAME"].ToString().Trim();
                    if (i != 0)
                    {
                        Spd.AddSpanCell(intS, 1, i - intS, 1);
                    }
                    intS = i;
                }
                strBASEXNAME = dt.Rows[i]["FORMNAME"].ToString().Trim();
            }
            Spd.AddSpanCell(intS, 1, i - intS, 1);

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// 상처 항목조회
        /// </summary>
        private void FormSearchWd(FarPoint.Win.Spread.SheetView Spd)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            Spd.RowCount = 0;

            SQL = "";
            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "     B.ITEMNO, B.ITEMNAME, BB.FORMNAME ";
            if (Spd == ssFormUser_Sheet1)
            {
                SQL = SQL + ComNum.VBLF + "     , (SELECT U1.ITEMCD ";
                SQL = SQL + ComNum.VBLF + "         FROM KOSMOS_EMR.AEMRUSERITEMVS U1 ";
                SQL = SQL + ComNum.VBLF + "         WHERE U1.JOBGB = '" + mJOBGB + "'";
                SQL = SQL + ComNum.VBLF + "            AND U1.USEGB = '" + clsType.User.BuseCode + "'";
                SQL = SQL + ComNum.VBLF + "            AND U1.ITEMCD = B.ITEMNO ";
                SQL = SQL + ComNum.VBLF + "     ) AS BUSEITEM  ";
            }
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRFLOWXML B";
            SQL = SQL + ComNum.VBLF + "   INNER JOIN " + ComNum.DB_EMR + "AEMRFORM BB";
            SQL = SQL + ComNum.VBLF + "      ON BB.FORMNO     = B.FORMNO ";
            SQL = SQL + ComNum.VBLF + "     AND BB.UPDATENO   = B.UPDATENO";
            SQL = SQL + ComNum.VBLF + "     AND BB.USECHECK = 1";
            SQL = SQL + ComNum.VBLF + "WHERE B.FORMNO     = 1725 ";
            SQL = SQL + ComNum.VBLF + "  AND B.UPDATENO   > 0";
            SQL = SQL + ComNum.VBLF + "  AND B.UPDATENO   > 0";
            SQL = SQL + ComNum.VBLF + "  AND B.ITEMNUMBER >= 0 ";

            if (Spd == ssFormUser_Sheet1)
            {
                SQL = SQL + ComNum.VBLF + "     AND EXISTS (SELECT 1 FROM " + ComNum.DB_EMR + "AEMRUSERITEMVS U";
                SQL = SQL + ComNum.VBLF + "                 WHERE U.JOBGB = '" + mJOBGB + "'";
                if (optDept.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "                        AND U.USEGB = '" + clsType.User.BuseCode + "'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "                        AND U.USEGB = '" + clsType.User.IdNumber + "'";
                }
                SQL = SQL + ComNum.VBLF + "                        AND U.ITEMCD = B.ITEMNO";
                SQL = SQL + ComNum.VBLF + "                 )";
            }

            SQL = SQL + ComNum.VBLF + "ORDER BY B.ITEMNUMBER";
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

            string strBASEXNAME = "";
            int intS = 0;

            Spd.RowCount = dt.Rows.Count;
            Spd.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Spd.Cells[i, 0].Text = dt.Rows[i]["ITEMNO"].ToString().Trim();
                Spd.Cells[i, 1].Text = dt.Rows[i]["FORMNAME"].ToString().Trim();
                Spd.Cells[i, 1].Font = new Font("맑은 고딕", 9.7f, FontStyle.Bold);

                Spd.Cells[i, 2].Text = dt.Rows[i]["ITEMNAME"].ToString().Trim();
                if (Spd == ssFormUser_Sheet1)
                {
                    if (dt.Rows[i]["ITEMNAME"].ToString().Trim() != "")
                    {
                        Spd.Cells[i, 2].Font = new Font("맑은 고딕", 9.7f, FontStyle.Bold);
                    }
                }

                if (strBASEXNAME != dt.Rows[i]["FORMNAME"].ToString().Trim())
                {
                    Spd.Cells[i, 1].Text = dt.Rows[i]["FORMNAME"].ToString().Trim();
                    if (i != 0)
                    {
                        Spd.AddSpanCell(intS, 1, i - intS, 1);
                    }
                    intS = i;
                }
                strBASEXNAME = dt.Rows[i]["FORMNAME"].ToString().Trim();
            }
            Spd.AddSpanCell(intS, 1, i - intS, 1);

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// 특수치료 항목조회
        /// </summary>
        private void FormSearchSp(FarPoint.Win.Spread.SheetView Spd, string mJOBGB)
        {
            StringBuilder SQL = new StringBuilder();
            int i = 0;
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            Spd.RowCount = 0;

            SQL.AppendLine("SELECT ");
            SQL.AppendLine("     B.BASVAL, B.BASCD, B.BASNAME, B.BASEXNAME ");
            if (Spd == ssFormUser_Sheet1)
            {
                SQL.AppendLine("     , (SELECT U1.ITEMCD ");
                SQL.AppendLine("         FROM KOSMOS_EMR.AEMRUSERITEMVS U1 ");
                SQL.AppendLine("         WHERE U1.JOBGB = '" + mJOBGB + "'");
                SQL.AppendLine("            AND U1.USEGB = '" + clsType.User.BuseCode + "'");
                SQL.AppendLine("            AND U1.ITEMCD = B.BASCD ");
                SQL.AppendLine("       ) AS BUSEITEM  ");
            }
            SQL.AppendLine(" FROM " + ComNum.DB_EMR + "AEMRBASCD B");
            SQL.AppendLine("WHERE B.BSNSCLS = '기록지관리'  ");
            SQL.AppendLine("  AND B.UNITCLS > CHR(0)");
            SQL.AppendLine("  AND B.USECLS = '0' ");
            SQL.AppendLine("  AND B.APLFRDATE > CHR(0) ");
            if (mJOBGB.Equals("SP"))
            {
                SQL.AppendLine("  AND B.BASEXNAME IN('호흡간호', '인공기도') ");
            }
            else
            {
                SQL.AppendLine("  AND B.BASEXNAME IN('산소요법', '인공호흡기' , '흡인간호(Suction)') ");
            }
            SQL.AppendLine("  AND EXISTS                                                               ");
            SQL.AppendLine("  (                                                                        ");
            SQL.AppendLine("  SELECT 1                                                                 ");
            SQL.AppendLine("   FROM  KOSMOS_EMR.AEMRBASCD BB                                           ");
            SQL.AppendLine("   WHERE B.VFLAG1 = BB.BASCD                                               ");
            SQL.AppendLine("     AND BB.BSNSCLS = '기록지관리'                                            ");
            SQL.AppendLine("     AND BB.UNITCLS IN ('간호활동그룹', '기본간호그룹' ,'섭취배설그룹', '특수치료그룹')   ");
            SQL.AppendLine("     AND BB.BASCD = B.VFLAG1                                               ");
            SQL.AppendLine("     AND BB.USECLS = '0'                                                   ");
            SQL.AppendLine("  )                                                                        ");

            if (Spd == ssFormUser_Sheet1)
            {
                SQL.AppendLine( "     AND EXISTS (SELECT 1 FROM " + ComNum.DB_EMR + "AEMRUSERITEMVS U");
                SQL.AppendLine("                 WHERE U.JOBGB = '" + mJOBGB + "'");
                if (optDept.Checked == true)
                {
                    SQL.AppendLine("                        AND U.USEGB = '" + clsType.User.BuseCode + "'");
                }
                else
                {
                    SQL.AppendLine("                        AND U.USEGB = '" + clsType.User.IdNumber + "'");
                }
                SQL.AppendLine( "                        AND U.ITEMCD = B.BASCD");
                SQL.AppendLine("                 )");
            }

            SQL.AppendLine("ORDER BY BASEXNAME, B.NFLAG1, B.NFLAG2, B.NFLAG3");
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString(), clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString(), clsDB.DbCon); //에러로그 저장
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

            string strBASEXNAME = "";
            int intS = 0;

            Spd.RowCount = dt.Rows.Count;
            Spd.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Spd.Cells[i, 0].Text = dt.Rows[i]["BASCD"].ToString().Trim();
                Spd.Cells[i, 1].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                Spd.Cells[i, 1].Font = new Font("맑은 고딕", 9.7f, FontStyle.Bold);

                Spd.Cells[i, 2].Text = dt.Rows[i]["BASNAME"].ToString().Trim();
                if (Spd == ssFormUser_Sheet1)
                {
                    if (dt.Rows[i]["BASNAME"].ToString().Trim() != "")
                    {
                        Spd.Cells[i, 2].Font = new Font("맑은 고딕", 9.7f, FontStyle.Bold);
                    }
                }

                if (strBASEXNAME != dt.Rows[i]["BASEXNAME"].ToString().Trim())
                {
                    Spd.Cells[i, 1].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                    if (i != 0)
                    {
                        Spd.AddSpanCell(intS, 1, i - intS, 1);
                    }
                    intS = i;
                }
                strBASEXNAME = dt.Rows[i]["BASEXNAME"].ToString().Trim();
            }
            Spd.AddSpanCell(intS, 1, i - intS, 1);

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// 간호활동 항목조회
        /// </summary>
        private void FormSearchAct(FarPoint.Win.Spread.SheetView Spd)
        {
            string SQL = "";
            int i = 0;
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            Spd.RowCount = 0;

            SQL = "";
            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "     B.BASVAL, B.BASCD, B.BASNAME, B.BASEXNAME ";
            if (Spd == ssFormUser_Sheet1)
            {
                SQL = SQL + ComNum.VBLF + "     , (SELECT U1.ITEMCD ";
                SQL = SQL + ComNum.VBLF + "         FROM KOSMOS_EMR.AEMRUSERITEMVS U1 ";
                SQL = SQL + ComNum.VBLF + "         WHERE U1.JOBGB = '" + mJOBGB + "'";
                SQL = SQL + ComNum.VBLF + "            AND U1.USEGB = '" + clsType.User.BuseCode + "'";
                SQL = SQL + ComNum.VBLF + "            AND U1.ITEMCD = B.BASCD ";
                SQL = SQL + ComNum.VBLF + "     ) AS BUSEITEM  ";
            }
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB";
            SQL = SQL + ComNum.VBLF + "     ON B.VFLAG1 = BB.BASCD ";
            SQL = SQL + ComNum.VBLF + "     AND BB.BSNSCLS = '기록지관리'  ";
            SQL = SQL + ComNum.VBLF + "     AND BB.UNITCLS = '간호활동그룹'"; //간호활동항목
            SQL = SQL + ComNum.VBLF + "     AND BB.USECLS = '0' ";
            SQL = SQL + ComNum.VBLF + "WHERE B.BSNSCLS = '기록지관리'  ";
            SQL = SQL + ComNum.VBLF + "     AND B.UNITCLS = '간호활동항목'"; //간호활동항목
            SQL = SQL + ComNum.VBLF + "     AND B.USECLS = '0' ";

            if (Spd == ssFormUser_Sheet1)
            {
                SQL = SQL + ComNum.VBLF + "     AND EXISTS (SELECT 1 FROM " + ComNum.DB_EMR + "AEMRUSERITEMVS U";
                SQL = SQL + ComNum.VBLF + "                 WHERE U.JOBGB = '" + mJOBGB + "'";
                if (optDept.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "                        AND U.USEGB = '" + clsType.User.BuseCode + "'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "                        AND U.USEGB = '" + clsType.User.IdNumber + "'";
                }
                SQL = SQL + ComNum.VBLF + "                        AND U.ITEMCD = B.BASCD";
                SQL = SQL + ComNum.VBLF + "                 )";
            }

            SQL = SQL + ComNum.VBLF + "ORDER BY BB.DISSEQNO, B.NFLAG1, B.NFLAG2, B.NFLAG3";
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

            string strBASEXNAME = "";
            int intS = 0;

            Spd.RowCount = dt.Rows.Count;
            Spd.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Spd.Cells[i, 0].Text = dt.Rows[i]["BASCD"].ToString().Trim();
                Spd.Cells[i, 1].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                Spd.Cells[i, 1].Font = new Font("맑은 고딕", 9.7f, FontStyle.Bold);

                Spd.Cells[i, 2].Text = dt.Rows[i]["BASNAME"].ToString().Trim();
                if (Spd == ssFormUser_Sheet1)
                {
                    if (dt.Rows[i]["BASNAME"].ToString().Trim() != "")
                    {
                        Spd.Cells[i, 2].Font = new Font("맑은 고딕", 9.7f, FontStyle.Bold);
                    }
                }

                if (strBASEXNAME != dt.Rows[i]["BASEXNAME"].ToString().Trim())
                {
                    Spd.Cells[i, 1].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                    if (i != 0)
                    {
                        Spd.AddSpanCell(intS, 1, i - intS, 1);
                    }
                    intS = i;
                }
                strBASEXNAME = dt.Rows[i]["BASEXNAME"].ToString().Trim();
            }
            Spd.AddSpanCell(intS, 1, i - intS, 1);

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// 임상관찰 항목조회
        /// </summary>
        private void FormSearchIVT(FarPoint.Win.Spread.SheetView Spd)
        {
            string SQL = "";
            int i = 0;
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            Spd.RowCount = 0;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "     B.GRPSORT, B.GRPSEQ, B.BASCD, B.BASNAME, B.BASEXNAME, ";
            SQL = SQL + ComNum.VBLF + "     B.NFLAG1, B.BASVAL, B.NFLAG2, B.NFLAG3, B.DISSEQNO ";
            if (Spd == ssFormUser_Sheet1)
            {
                SQL = SQL + ComNum.VBLF + "     , (SELECT U1.ITEMCD ";
                SQL = SQL + ComNum.VBLF + "         FROM KOSMOS_EMR.AEMRUSERITEMVS U1 ";
                SQL = SQL + ComNum.VBLF + "         WHERE U1.JOBGB = '" + mJOBGB + "'";
                SQL = SQL + ComNum.VBLF + "            AND U1.USEGB = '" + clsType.User.BuseCode + "'";
                SQL = SQL + ComNum.VBLF + "            AND U1.ITEMCD = B.BASCD ";
                SQL = SQL + ComNum.VBLF + "     ) AS BUSEITEM  ";
            }
            SQL = SQL + ComNum.VBLF + "FROM (";
            SQL = SQL + ComNum.VBLF + "     SELECT ";
            SQL = SQL + ComNum.VBLF + "         CASE ";
            SQL = SQL + ComNum.VBLF + "             WHEN B.UNITCLS = '임상관찰' THEN 0 ";
            SQL = SQL + ComNum.VBLF + "             WHEN B.UNITCLS = '섭취배설' THEN 1 ";
            SQL = SQL + ComNum.VBLF + "             WHEN B.UNITCLS = '특수치료' THEN 2 ";
            SQL = SQL + ComNum.VBLF + "             ELSE 3 ";
            SQL = SQL + ComNum.VBLF + "         END AS GRPSORT, ";
            SQL = SQL + ComNum.VBLF + "         B.BASCD, B.BASNAME, B.BASEXNAME, ";
            SQL = SQL + ComNum.VBLF + "         B.NFLAG1, B.BASVAL, B.NFLAG2, B.NFLAG3, B.DISSEQNO, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT BB.DISSEQNO FROM " + ComNum.DB_EMR + "AEMRBASCD BB";
            SQL = SQL + ComNum.VBLF + "             WHERE BB.BASCD = B.VFLAG1 ";
            SQL = SQL + ComNum.VBLF + "             AND BB.BSNSCLS = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "             AND BB.UNITCLS = B.UNITCLS || '그룹') AS GRPSEQ";
            SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "     WHERE B.BSNSCLS = '기록지관리'  ";
            SQL = SQL + ComNum.VBLF + "          AND B.UNITCLS IN ('임상관찰', '섭취배설', '특수치료', '기본간호')";
            SQL = SQL + ComNum.VBLF + "          AND B.USECLS = '0' ";
            if (Spd == ssFormUser_Sheet1)
            {
                SQL = SQL + ComNum.VBLF + "         AND EXISTS (SELECT 1 FROM " + ComNum.DB_EMR + "AEMRUSERITEMVS U";
                SQL = SQL + ComNum.VBLF + "                     WHERE U.JOBGB = '" + mJOBGB + "'";
                if (optDept.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "                            AND U.USEGB = '" + clsType.User.BuseCode + "'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "                            AND U.USEGB = '" + clsType.User.IdNumber + "'";
                }
                SQL = SQL + ComNum.VBLF + "                            AND U.ITEMCD = B.BASCD";
                SQL = SQL + ComNum.VBLF + "                     )";
            }
            SQL = SQL + ComNum.VBLF + "     ) B";
            SQL = SQL + ComNum.VBLF + "ORDER BY B.GRPSORT, B.GRPSEQ, B.NFLAG1, B.BASVAL, B.NFLAG2, B.NFLAG3, B.DISSEQNO";
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

            string strBASEXNAME = "";
            int intS = 0;

            Spd.RowCount = dt.Rows.Count;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                Spd.Cells[i, 0].Text = dt.Rows[i]["BASCD"].ToString().Trim();
                Spd.Cells[i, 1].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                Spd.Cells[i, 1].Font = new Font("맑은 고딕", 9.7f, FontStyle.Bold);

                Spd.Cells[i, 2].Text = dt.Rows[i]["BASNAME"].ToString().Trim();
                if (Spd == ssFormUser_Sheet1)
                {
                    if (dt.Rows[i]["BASNAME"].ToString().Trim() != "")
                    {
                        Spd.Cells[i, 2].Font = new Font("맑은 고딕", 9.7f, FontStyle.Bold);
                    }
                }
                Spd.SetRowHeight(i, ComNum.SPDROWHT);
                if (strBASEXNAME != dt.Rows[i]["BASEXNAME"].ToString().Trim())
                {
                    Spd.Cells[i, 1].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                    if (i != 0)
                    {
                        Spd.AddSpanCell(intS, 1, i - intS, 1);
                    }
                    intS = i;
                }
                strBASEXNAME = dt.Rows[i]["BASEXNAME"].ToString().Trim();
            }
            Spd.AddSpanCell(intS, 1, i - intS, 1);

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// 부서별 아이템을 저장한다
        /// </summary>
        /// <param name="strItemCD"></param>
        /// <param name="strItemNm"></param>
        /// <returns></returns>
        private bool SaveData(string strItemCD, string strItemNm)
        {
            bool rtnVal = false;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            DataTable dt = null;
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "    ITEMCD ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRUSERITEMVS ";
                SQL = SQL + ComNum.VBLF + "WHERE JOBGB = '" + mJOBGB + "'";
                if (optDept.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND USEGB = '" + clsType.User.BuseCode + "'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND USEGB = '" + clsType.User.IdNumber + "'";
                }
                SQL = SQL + ComNum.VBLF + "    AND ITEMCD = '" + strItemCD + "'";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count != 0)
                {
                    dt.Dispose();
                    dt = null;
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return rtnVal;
                }
                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_EMR + "AEMRUSERITEMVS ";
                SQL = SQL + ComNum.VBLF + "            (USEGB,     JOBGB,    ITEMCD,  DISSEQ ) ";
                SQL = SQL + ComNum.VBLF + " VALUES( ";
                if (optDept.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "              '" + clsType.User.BuseCode + "', ";  //USEGB
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "              '" + clsType.User.IdNumber + "', ";  //USEGB
                }
                
                SQL = SQL + ComNum.VBLF + "              '" + mJOBGB + "',";  //JOBGB
                SQL = SQL + ComNum.VBLF + "              '" + strItemCD + "', ";  //ITEMCD
                SQL = SQL + ComNum.VBLF + "              0 "; //DISSEQ
                SQL = SQL + ComNum.VBLF + "    ) ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "INSERT중 오류가 발생하였습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 사용자별 아이템을 삭제한다
        /// </summary>
        /// <param name="strItemCD"></param>
        /// <returns></returns>
        private bool DeleteData(string strItemCD)
        {
            bool rtnVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                SQL = "";
                SQL = "DELETE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRUSERITEMVS ";
                SQL = SQL + ComNum.VBLF + "WHERE JOBGB = '" + mJOBGB + "'";
                if (optDept.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND USEGB = '" + clsType.User.BuseCode + "'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND USEGB = '" + clsType.User.IdNumber + "'";
                }
                SQL = SQL + ComNum.VBLF + "    AND ITEMCD = '" + strItemCD + "'";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "삭제중 오류가 발생하였습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 과별 디폴트 아이템을 사용자로 저장한다
        /// </summary>
        /// <returns></returns>
        private bool SaveDataDeptToUser(string strJOBGB)
        {
            bool rtnVal = false;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            //DataTable dt = null;
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRUSERITEMVS ";
                SQL = SQL + ComNum.VBLF + "    (USEGB, JOBGB, ITEMCD, DISSEQ)";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "    '" + clsType.User.IdNumber + "' AS USEGB, ";
                SQL = SQL + ComNum.VBLF + "    D.JOBGB, D.ITEMCD, D.DISSEQ ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRUSERITEMVS D";
                SQL = SQL + ComNum.VBLF + "WHERE D.JOBGB = '" + strJOBGB + "'";
                SQL = SQL + ComNum.VBLF + "    AND D.USEGB = '" + clsType.User.BuseCode + "'";
                SQL = SQL + ComNum.VBLF + "    AND NOT EXISTS ( SELECT 1 FROM " + ComNum.DB_EMR + "AEMRUSERITEMVS U";
                SQL = SQL + ComNum.VBLF + "                     WHERE U.JOBGB = '" + strJOBGB + "'";
                SQL = SQL + ComNum.VBLF + "                         AND U.USEGB = '" + clsType.User.IdNumber + "'";
                SQL = SQL + ComNum.VBLF + "                         AND D.ITEMCD = U.ITEMCD )";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "INSERT중 오류가 발생하였습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void mbtnExit_Click(object sender, EventArgs e)
        {
            SaveDataDeptToUser("IVT");
            SaveDataDeptToUser("ACT");
            SaveDataDeptToUser("SP");
            SaveDataDeptToUser("WD");
            SaveDataDeptToUser("BD");
            SaveDataDeptToUser("CVC");
            this.Close();
        }

        private void optVital_CheckedChanged(object sender, EventArgs e)
        {
            if (optVital.Checked == true)
            {
                mJOBGB = "IVT";
                SaveDataDeptToUser(mJOBGB);
                FormSearch();
            }
        }

        private void optAct_CheckedChanged(object sender, EventArgs e)
        {
            if (optAct.Checked == true)
            {
                mJOBGB = "ACT";
                SaveDataDeptToUser(mJOBGB);
                FormSearch();
            }
        }

        private void ssForm_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssForm_Sheet1.RowCount == 0)
            {
                return;
            }
            if (e.ColumnHeader == true)
            {
                return;
            }

            if (e.Column == 1)
            {
                string sItemNm = ssForm_Sheet1.Cells[e.Row, 1].Text.Trim();
                for (int i = e.Row; i < ssForm_Sheet1.RowCount; i++)
                {
                    if (!string.IsNullOrWhiteSpace(ssForm_Sheet1.Cells[i, 1].Text.Trim()) && !sItemNm.Equals(ssForm_Sheet1.Cells[i, 1].Text.Trim()))
                    {
                        break;
                    }

                    if (ssForm_Sheet1.Cells[i, 1].Text.Trim() == sItemNm || string.IsNullOrWhiteSpace(ssForm_Sheet1.Cells[i, 1].Text.Trim()))
                    {
                        if (SaveData(ssForm_Sheet1.Cells[i, 0].Text.Trim(), ssForm_Sheet1.Cells[i, 2].Text.Trim()) == true)
                        {

                        }
                    }
                }
                FormSearchUser();
                return;
            }

            string strItemCD = ssForm_Sheet1.Cells[e.Row, 0].Text.Trim();
            string strItemNm = ssForm_Sheet1.Cells[e.Row, 2].Text.Trim();
            if (SaveData(strItemCD, strItemNm) == true)
            {
                FormSearchUser();
            }
        }

        private void ssFormUser_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssFormUser_Sheet1.RowCount == 0)
            {
                return;
            }
            if (e.ColumnHeader == true)
            {
                return;
            }

            if (e.Column != 2)
            {
                return;
            }

            string strItemCD = ssFormUser_Sheet1.Cells[e.Row, 0].Text.Trim();

            if (DeleteData(strItemCD) == true)
            {
                FormSearchUser();
            }
        }

        private void optUser_CheckedChanged(object sender, EventArgs e)
        {
            FormSearchUser();
        }

        private void optSpecial_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked == true)
            {
                mJOBGB = sender.Equals(optSpecial) ?  "SP" : "SP2";
                SaveDataDeptToUser(mJOBGB);
                FormSearch();
            }
        }

        private void optWound_CheckedChanged(object sender, EventArgs e)
        {
            if (optWound.Checked == true)
            {
                mJOBGB = "WD";
                SaveDataDeptToUser(mJOBGB);
                FormSearch();
            }
        }

        private void optBedsore_CheckedChanged(object sender, EventArgs e)
        {
            if (optBedsore.Checked == true)
            {
                mJOBGB = "BD";
                SaveDataDeptToUser(mJOBGB);
                FormSearch();
            }
        }

        private void optCvc_CheckedChanged(object sender, EventArgs e)
        {
            if (optCvc.Checked == true)
            {
                mJOBGB = "CVC";
                SaveDataDeptToUser(mJOBGB);
                FormSearch();
            }
        }
    }
}
