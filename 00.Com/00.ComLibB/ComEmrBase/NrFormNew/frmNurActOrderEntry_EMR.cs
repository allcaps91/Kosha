using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase;
using ComBase.Controls;
using FarPoint.Win.Spread;
using Oracle.ManagedDataAccess.Client;

namespace ComEmrBase
{
    public partial class frmNurActOrderEntry_EMR : Form
    {
        #region 변수 선언
        /// <summary>
        /// 선택한 기록지 번호
        /// </summary>
        string SelectFormNo = string.Empty;
        /// <summary>
        /// 선택한 아이템 코드
        /// </summary>
        string SelectItemcd = string.Empty;
        /// <summary>
        /// 선택한 아이템 값
        /// </summary>
        string SelectItemVal = string.Empty;

        #endregion

        #region 생성자
        public frmNurActOrderEntry_EMR()
        {
            InitializeComponent();
        }

        #endregion

        private void frmNurActOrderEntry_EMR_Load(object sender, EventArgs e)
        {
            #region 클리어
            ssValue_Sheet1.RowCount = 0;
            ssItem_Sheet1.RowCount = 0;
            ss3_Sheet1.RowCount = 0;
            #endregion

            #region 쿼리
            string SQL = string.Empty;
            DataTable dt = null;
            int sIndex = -1;

            cboWard.Items.Add("전체");

            SQL = " SELECT NAME WARDCODE, MATCH_CODE";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_CODE";
            SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '2'";
            SQL = SQL + ComNum.VBLF + "   AND SUBUSE = '1'";
            SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING";

            string SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            string WardCodes = string.Empty;
            if (VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "") != "")
            {
                WardCodes = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "");
            }

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cboWard.Items.Add(dt.Rows[i]["WardCode"].ToString().Trim());
                    //if (dt.Rows[i]["MATCH_CODE"].ToString().Trim().Equals(clsType.User.BuseCode))
                    if (dt.Rows[i]["WardCode"].ToString().Trim().Equals(WardCodes))
                    {
                        sIndex = i;
                    }
                }
            }

            dt.Dispose();
            #endregion

            bool Manager = NURSE_Manager_Check(clsType.User.IdNumber);
            if (Manager == true || clsVbfunc.GetBCodeCODE(clsDB.DbCon, "NUR_간호부관리자사번IP", "").Equals(clsCompuInfo.gstrCOMIP))
            {
                cboWard.Enabled = true;
            }

            cboWard.SelectedIndex = sIndex == -1 ? cboWard.SelectedIndex : sIndex + 1;

            GetFormList();
            READ_BAT();

            if (SelectFormNo.NotEmpty())
            {
                GetItemList(SelectFormNo);
            }

            if (SelectItemcd.NotEmpty())
            {
                //GeItemBuseList(SelectItemcd);
                //btnBatView.PerformClick();
            }
        }

        private bool NURSE_Manager_Check(string strSABUN)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            SQL = "SELECT Code FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
            SQL = SQL + ComNum.VBLF + " WHERE Gubun='NUR_간호부관리자사번' ";
            SQL = SQL + ComNum.VBLF + "   AND Code= " + VB.Val(strSABUN) + " ";
            SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL    ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return false;
            }

            dt.Dispose();
            dt = null;
            return true;
        }


        #region 함수

        private void READ_BAT()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ss4_Sheet1.RowCount = 0;

            try
            {
                SQL = " SELECT SEQNAME, SEQNO  ";
                SQL = SQL + ComNum.VBLF + "     , CASE WHEN EXISTS(";
                SQL = SQL + ComNum.VBLF + "     SELECT 1";
                SQL = SQL + ComNum.VBLF + "       FROM KOSMOS_PMPA.BAS_SUT B";
                SQL = SQL + ComNum.VBLF + "         INNER JOIN KOSMOS_PMPA.NUR_CARECODE B2";
                SQL = SQL + ComNum.VBLF + "            ON B.SUNEXT = B2.SUCODE";
                SQL = SQL + ComNum.VBLF + "           AND B.DELDATE IS NOT NULL";
                SQL = SQL + ComNum.VBLF + "      WHERE A.SEQNO = B2.SEQNO";
                SQL = SQL + ComNum.VBLF + "      UNION ALL";
                SQL = SQL + ComNum.VBLF + "     SELECT 1";
                SQL = SQL + ComNum.VBLF + "       FROM KOSMOS_OCS.OCS_ORDERCODE B";
                SQL = SQL + ComNum.VBLF + "         INNER JOIN KOSMOS_PMPA.NUR_CARECODE B2";
                SQL = SQL + ComNum.VBLF + "            ON B.SUCODE = B2.SUCODE";
                SQL = SQL + ComNum.VBLF + "           AND B.ORDERCODE = B2.ORDERCODE";
                SQL = SQL + ComNum.VBLF + "      WHERE A.SEQNO = B2.SEQNO";
                SQL = SQL + ComNum.VBLF + "        AND B.SENDDEPT = 'Y'";
                SQL = SQL + ComNum.VBLF + "     ) ";
                SQL = SQL + ComNum.VBLF + "     THEN 'Y' END CHECK_DEL";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_CAREMST A";
                SQL = SQL + ComNum.VBLF + " WHERE WARDCODE = '" + cboWard.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY SEQNO ASC ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ss4_Sheet1.RowCount = dt.Rows.Count;
                    ss4_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ss4_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SEQNAME"].ToString().Trim();
                        ss4_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
                        if (dt.Rows[i]["CHECK_DEL"].ToString().Equals("Y"))
                        {
                            ss4_Sheet1.Cells[i, 1].Text = "[삭제코드포함]" + ss4_Sheet1.Cells[i, 1].Text;
                        }
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        private void SET_BAT(int ArgSeqno)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT SEQNO, SEQNAME ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_CAREMST ";
                SQL = SQL + ComNum.VBLF + " WHERE SEQNO = " + ArgSeqno;

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ss3_Sheet1.RowCount += dt.Rows.Count;
                    ss3_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    ss3_Sheet1.Cells[ss3_Sheet1.RowCount - 1, 1].Text = dt.Rows[0]["SEQNAME"].ToString().Trim();
                    ss3_Sheet1.Cells[ss3_Sheet1.RowCount - 1, 2].Text = dt.Rows[0]["SEQNO"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }


        //SelectMacroNm, SelectFormNo, SelectItemcd
        private void SET_BAT(string Ward, string FormNo, string Itemcd, string ItemVal)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수

            ss3_Sheet1.RowCount = 0;

            try
            {
                SQL = " SELECT ";
                SQL = SQL + ComNum.VBLF + "   A.SEQNO";
                SQL = SQL + ComNum.VBLF + " , SEQNAME";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRSUGAMAPPING A";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN KOSMOS_PMPA.NUR_CAREMST B";
                SQL = SQL + ComNum.VBLF + "       ON A.SEQNO = B.SEQNO";
                SQL = SQL + ComNum.VBLF + " WHERE A.FORMNO     = " + FormNo;
                SQL = SQL + ComNum.VBLF + "   AND A.ITEMCD     = '" + Itemcd + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.ITEMVALUE  = '" + ItemVal + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.WARD       = '" + Ward + "'";
                SQL = SQL + ComNum.VBLF + " ORDER BY A.SEQNO";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ss3_Sheet1.RowCount = dt.Rows.Count;
                    ss3_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ss3_Sheet1.Cells[i, 1].Text  = dt.Rows[i]["SEQNAME"].ToString().Trim();
                        ss3_Sheet1.Cells[i, 2].Text  = dt.Rows[i]["SEQNO"].ToString().Trim();
                    }

                    ss3_Sheet1.ColumnHeader.Cells[0, 0].Text = "■";

                    ss3_Sheet1.Cells[0, 0, ss3_Sheet1.RowCount - 1, 0].Value = true;
                    ss3_Sheet1.Rows[0, ss3_Sheet1.RowCount - 1].BackColor = Color.FromArgb(255, 128, 128);

                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        /// <summary>
        /// 기록지 리스트
        /// </summary>
        private void GetFormList()
        {
            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            #region 쿼리
            SQL = "SELECT FORMNO, FORMNAME";
            SQL += ComNum.VBLF + "FROM KOSMOS_EMR.AEMRFORM";
            SQL += ComNum.VBLF + "WHERE FORMNO IN (3150, 1575, 1573, 1725, 2638, 2240) -- 임상관찰, 기본간호활동, 욕창간호, 상처간호, 중심정맥관, 말초정맥관";
            SQL += ComNum.VBLF + "  AND UPDATENO > 0";
            SQL += ComNum.VBLF + "  AND USECHECK = 1";
            #endregion

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, SqlErr);
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                return;
            }

            ssForm_Sheet1.RowCount = dt.Rows.Count;
            ssForm_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ssForm_Sheet1.Cells[i, 0].Text = dt.Rows[i]["FORMNO"].ToString();
                ssForm_Sheet1.Cells[i, 1].Text = dt.Rows[i]["FORMNAME"].ToString();
            }

            dt.Dispose();
            SelectFormNo = ssForm_Sheet1.Cells[0, 0].Text;
            return;
        }

        /// <summary>
        /// 기록지 아이템 리스트
        /// </summary>
        private void GetItemList(string formNo)
        {
            ssValue_Sheet1.RowCount = 0;
            ssItem_Sheet1.RowCount = 0;

            if (formNo.Equals("1725") || formNo.Equals("1573"))
            {
                ssItem_Sheet1.RowCount += 1;
                ssItem_Sheet1.Cells[0, 1].Text = (formNo.Equals("1725") ? "상처간호" : "욕창간호");
                ssItem_Sheet1.Cells[0, 2].Text = (formNo.Equals("1725") ? "상처간호" : "욕창간호");
                ssItem_Sheet1.Cells[0, 3].Text = (formNo.Equals("1725") ? "상처간호" : "욕창간호");
                ssItem_Sheet1.Cells[0, 4].Text = (formNo.Equals("1725") ? "상처" : "욕창");
                return;
            }
            else if (formNo.Equals("2638"))
            {
                ssItem_Sheet1.RowCount += 1;
                ssItem_Sheet1.Cells[0, 1].Text = "중심정맥관";
                ssItem_Sheet1.Cells[0, 2].Text = "중심정맥관";
                ssItem_Sheet1.Cells[0, 3].Text = "중심정맥관";
                ssItem_Sheet1.Cells[0, 4].Text = "중심정맥관";
                return;
            }
            else if (formNo.Equals("2240"))
            {
                ssItem_Sheet1.RowCount += 1;
                ssItem_Sheet1.Cells[0, 1].Text = "말초정맥관";
                ssItem_Sheet1.Cells[0, 2].Text = "말초정맥관";
                ssItem_Sheet1.Cells[0, 3].Text = "말초정맥관";
                ssItem_Sheet1.Cells[0, 4].Text = "말초정맥관";
                return;
            }


            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            #region 쿼리
            if (rdoSelect1.Checked)
            {
                SQL = "SELECT   UNITCLS";
                SQL += ComNum.VBLF + "  , BASEXNAME";
                SQL += ComNum.VBLF + "  , BASNAME";
                SQL += ComNum.VBLF + "  , BASCD";

                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.AEMRBASCD A";
                SQL += ComNum.VBLF + "WHERE A.BSNSCLS = '기록지관리'";

                if (formNo.Equals("3150"))
                {
                    SQL += ComNum.VBLF + "  AND A.UNITCLS IN ('임상관찰', '특수치료', '섭취배설', '임상관찰') ";
                }
                else
                {
                    SQL += ComNum.VBLF + "  AND A.UNITCLS = '간호활동항목'";
                }

                SQL += ComNum.VBLF + "ORDER BY A.VFLAG1, A.NFLAG1";
            }
            else
            {
                SQL = "SELECT   UNITCLS";
                SQL += ComNum.VBLF + "  , BASEXNAME";
                SQL += ComNum.VBLF + "  , BASNAME";
                SQL += ComNum.VBLF + "  , BASCD";

                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.AEMRBASCD A";


                SQL += ComNum.VBLF + "WHERE A.BSNSCLS = '기록지관리'";

                if (formNo.Equals("3150"))
                {
                    SQL += ComNum.VBLF + "  AND A.UNITCLS IN ('임상관찰', '특수치료', '섭취배설', '임상관찰') ";
                }
                else
                {
                    SQL += ComNum.VBLF + "  AND A.UNITCLS = '간호활동항목'";
                }

                SQL += ComNum.VBLF + "   AND EXISTS";
                SQL += ComNum.VBLF + "   (";
                // JOIN KOSMOS_EMR.AEMRSUGAMAPPING B
                // JOIN KOSMOS_EMR.AEMRSUGAMAPPING B
                SQL += ComNum.VBLF + "      SELECT 1 ";
                SQL += ComNum.VBLF + "        FROM KOSMOS_EMR.AEMRSUGAMAPPING B";
                SQL += ComNum.VBLF + "       WHERE A.BASCD = B.ITEMCD";
                SQL += ComNum.VBLF + "         AND FORMNO = " + formNo;
                SQL += ComNum.VBLF + "         AND WARD = '" + cboWard.Text + "'";
                SQL += ComNum.VBLF + "   )";

                SQL += ComNum.VBLF + "ORDER BY A.VFLAG1, A.NFLAG1";
            }
            
            #endregion

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, SqlErr);
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                return;
            }

            ssItem_Sheet1.RowCount = dt.Rows.Count;
            ssItem_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //ssItem_Sheet1.Cells[i, 0].Text = (!string.IsNullOrWhiteSpace(dt.Rows[i]["ITEMCD"].ToString())).ToString();
                ssItem_Sheet1.Cells[i, 1].Text = dt.Rows[i]["UNITCLS"].ToString();
                ssItem_Sheet1.Cells[i, 2].Text = dt.Rows[i]["BASEXNAME"].ToString();
                ssItem_Sheet1.Cells[i, 3].Text = dt.Rows[i]["BASCD"].ToString();
                ssItem_Sheet1.Cells[i, 4].Text = dt.Rows[i]["BASNAME"].ToString();
                //ssItem_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ORDERGBN"].ToString();
                //ssItem_Sheet1.Cells[i, 6].Text = dt.Rows[i]["APPOINTMENT"].ToString();
            }

            dt.Dispose();
            return;
        }


        /// <summary>
        /// 기록지 아이템 - 값 리스트
        /// </summary>
        private void GetValueList()
        {
            ssValue_Sheet1.RowCount = 0;
            ss3_Sheet1.RowCount = 0;

            if (SelectFormNo.Equals("1573"))
            {
                ssValue_Sheet1.RowCount = 1;
                ssValue_Sheet1.Cells[ssValue_Sheet1.RowCount - 1, 0].Text = "욕창간호";
                return;
            }

            else if (SelectFormNo.Equals("1725"))
            {
                ssValue_Sheet1.RowCount = 1;
                ssValue_Sheet1.Cells[ssValue_Sheet1.RowCount - 1, 0].Text = "상처간호"; 
                return;
            }
             else if (SelectFormNo.Equals("2638"))
            {
                ssValue_Sheet1.RowCount = 3;
                ssValue_Sheet1.Cells[ssValue_Sheet1.RowCount - 3, 0].Text = "삽입";
                ssValue_Sheet1.Cells[ssValue_Sheet1.RowCount - 2, 0].Text = "유지";
                ssValue_Sheet1.Cells[ssValue_Sheet1.RowCount - 1, 0].Text = "제거";
                return;
            }
            else if (SelectFormNo.Equals("2240"))
            {
                ssValue_Sheet1.RowCount = 2;
                ssValue_Sheet1.Cells[ssValue_Sheet1.RowCount - 2, 0].Text = "삽입";
                ssValue_Sheet1.Cells[ssValue_Sheet1.RowCount - 1, 0].Text = "유지";
                return;
            }



            if (SelectFormNo.Equals("3150") == false &&
                SelectFormNo.Equals("1575") == false)
            {
                return;
            }

            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            #region 쿼리

            SQL = "SELECT ITEMVALUE, MAX(DSPSEQ) DSPSEQ ";
            SQL = SQL + ComNum.VBLF + "FROM";
            SQL = SQL + ComNum.VBLF + "(";
            SQL = SQL + ComNum.VBLF + "SELECT ITEMVALUE, 0 AS DSPSEQ ";
            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRSUGAMAPPING ";
            SQL = SQL + ComNum.VBLF + " WHERE FORMNO    = " + SelectFormNo;
            SQL = SQL + ComNum.VBLF + "   AND ITEMCD    = '" + SelectItemcd + "'";
            SQL = SQL + ComNum.VBLF + "   AND WARD      = '" + cboWard.Text + "'";
            SQL = SQL + ComNum.VBLF + " GROUP BY ITEMVALUE ";
            SQL = SQL + ComNum.VBLF + "UNION ALL";
            SQL = SQL + ComNum.VBLF + "SELECT ITEMVALUE, MAX(DSPSEQ) DSPSEQ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRITEMBASEVAL";
            if (SelectFormNo.Equals("3150"))
            {
                SQL = SQL + ComNum.VBLF + "WHERE JOBGB = 'IVT'";
            }
            else if (SelectFormNo.Equals("1575"))
            {
                SQL = SQL + ComNum.VBLF + "WHERE JOBGB = 'ACT'";
            }
            SQL = SQL + ComNum.VBLF + "    AND ITEMCD = '" + SelectItemcd + "'";
            SQL = SQL + ComNum.VBLF + "GROUP BY ITEMCD, ITEMVALUE";
            SQL = SQL + ComNum.VBLF + ")";
            SQL = SQL + ComNum.VBLF + "GROUP BY ITEMVALUE";
            SQL = SQL + ComNum.VBLF + "ORDER BY DSPSEQ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }

            ssValue_Sheet1.RowCount = dt.Rows.Count;
            ssValue_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ssValue_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ITEMVALUE"].ToString();
            }
          
            dt.Dispose();
            dt = null;

            ss3_Sheet1.RowCount = 0;

            SelectItemVal = ssValue_Sheet1.Cells[0, 0].Text.Trim();
            SET_BAT(cboWard.Text, SelectFormNo, SelectItemcd, SelectItemVal);
            #endregion
        }

        #endregion

        #region 스프레드 이벤트
        private void ssForm_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            SelectItemcd = string.Empty;
            SelectFormNo = string.Empty;
            SelectFormNo = ssForm_Sheet1.Cells[e.Row, 0].Text.Trim();

            if (e.ColumnHeader)
            {
                clsSpread.gSpdSortRow(ssForm, e.Column);
                return;
            }

            GetItemList(SelectFormNo);
        }

        private void ssItem_CellClick(object sender, CellClickEventArgs e)
        {
            SelectItemcd = string.Empty;
            SelectItemcd = ssItem_Sheet1.Cells[e.Row, 3].Text.Trim();
            SelectItemVal = string.Empty;

            if (e.ColumnHeader)
            {
                clsSpread.gSpdSortRow(ssItem, e.Column);
                return;
            }

            GetValueList();
        }
        private void ssDetail_CellClick(object sender, CellClickEventArgs e)
        {

        }


        private void ss3_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            //if (ss3_Sheet1.RowCount == 0)
            //    return;

            //if (VB.Left(ss3_Sheet1.Cells[e.Row, 1].Text, 3) == "[삭제")
            //{
            //    ComFunc.MsgBoxEx(this, "삭제된 코드가 포함되어 있습니다. 약속처방을 정리 후 다시 전송하시기 바랍니다.");
            //    return;
            //}

            //superTabControl1.SelectedTabIndex = 4;

            //SelectMacroNm = ss3_Sheet1.Cells[e.Row, 1].Text;
            //SelectFormNo  = ss3_Sheet1.Cells[e.Row, 2].Text;
            //SelectItemcd = ss3_Sheet1.Cells[e.Row, 3].Text;

            SET_BAT(cboWard.Text, SelectFormNo, SelectItemcd, SelectItemVal);
        }
        #endregion

        private void btnSaveOrder_Click(object sender, EventArgs e)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;
            OracleDataReader reader = null;
   

            int i;

            //if (strCNT.IsNullOrEmpty())
            //{
            //    ComFunc.MsgBoxEx(this, "약속처방에 등록할 코드를 선택하십시요.");
            //    return;
            //}

            if ((SelectFormNo.Equals("1725") || SelectFormNo.Equals("1573")) && SelectItemVal.IsNullOrEmpty())
            {
                SelectItemVal = SelectFormNo.Equals("1725") ? "상처간호" : "욕창간호";
            }

            if (SelectItemVal.IsNullOrEmpty())
            {
                ComFunc.MsgBoxEx(this, "기록 값을 선택해주세요!", "오류");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "     DELETE KOSMOS_EMR.AEMRSUGAMAPPING";
                SQL = SQL + ComNum.VBLF + "      WHERE FORMNO     = " + SelectFormNo;
                SQL = SQL + ComNum.VBLF + "        AND ITEMCD     = '" + SelectItemcd + "'";
                SQL = SQL + ComNum.VBLF + "        AND ITEMVALUE  = '" + SelectItemVal + "'";
                SQL = SQL + ComNum.VBLF + "        AND WARD       = '" + cboWard.Text + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                for (i = 0; i < ss3_Sheet1.Rows.Count; i++)
                {
                    if (ss3_Sheet1.Cells[i, 0].Text.Equals("True"))
                    {
                        string SeqNo = ss3_Sheet1.Cells[i, 2].Text.Trim();

                        #region 명칭 중복 점검
                        SQL = "SELECT 1";
                        SQL = SQL + ComNum.VBLF + "FROM DUAL";
                        SQL = SQL + ComNum.VBLF + "WHERE EXISTS";
                        SQL = SQL + ComNum.VBLF + "(";
                        SQL = SQL + ComNum.VBLF + "     SELECT 1";
                        SQL = SQL + ComNum.VBLF + "       FROM KOSMOS_EMR.AEMRSUGAMAPPING";
                        SQL = SQL + ComNum.VBLF + "      WHERE FORMNO     = " + SelectFormNo;
                        SQL = SQL + ComNum.VBLF + "        AND ITEMCD     = '" + SelectItemcd + "'";
                        SQL = SQL + ComNum.VBLF + "        AND ITEMVALUE  = '" + SelectItemVal + "'";
                        SQL = SQL + ComNum.VBLF + "        AND WARD       = '" + cboWard.Text + "'";
                        SQL = SQL + ComNum.VBLF + "        AND SEQNO      = " + SeqNo;
                        SQL = SQL + ComNum.VBLF + ")";

                        SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                        if (reader.HasRows)
                        {
                            reader.Dispose();
                            continue;
                        }

                        reader.Dispose();
                        #endregion

                        SQL = " INSERT INTO KOSMOS_EMR.AEMRSUGAMAPPING ";
                        SQL = SQL + ComNum.VBLF + " (FORMNO, WARD, ITEMCD, ITEMVALUE, SEQNO)";
                        SQL = SQL + ComNum.VBLF + "SELECT ";
                        SQL = SQL + ComNum.VBLF + SelectFormNo        + ", ";
                        SQL = SQL + ComNum.VBLF + "'" + cboWard.Text  + "', ";
                        SQL = SQL + ComNum.VBLF + "'" + SelectItemcd + "', ";
                        SQL = SQL + ComNum.VBLF + "'" + SelectItemVal + "', ";
                        SQL = SQL + ComNum.VBLF + SeqNo;

                        SQL = SQL + ComNum.VBLF + "FROM DUAL";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                SET_BAT(cboWard.Text, SelectFormNo, SelectItemcd, SelectItemVal);
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void ssDetail_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            string SQL = string.Empty;
            if (ComFunc.MsgBoxQEx(this, "삭제하시면 복구가 불가능합니다. 계속하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //SQL = " DELETE KOSMOS_EMR.AEMRSUGAMAPPING";
                //SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + ssDetail_Sheet1.Cells[e.Row, 1].Text + "'";

                int intRowAffected = 0;
                string SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                SET_BAT(cboWard.Text, SelectFormNo, SelectItemcd, SelectItemVal);
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void ssBuse_CellClick(object sender, CellClickEventArgs e)
        {
            if (ssValue_Sheet1.RowCount == 0)
                return;

            ss3_Sheet1.RowCount = 0;

            SelectItemVal = ssValue_Sheet1.Cells[e.Row, 0].Text.Trim();
            SET_BAT(cboWard.Text, SelectFormNo, SelectItemcd, SelectItemVal);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnValueAdd_Click(object sender, EventArgs e)
        {
            ssValue_Sheet1.RowCount += 1;
        }

        private void btnDelValue_Click(object sender, EventArgs e)
        {
            if (ssValue_Sheet1.ActiveRowIndex == -1)
                return;

            ssValue_Sheet1.Rows.Remove(ssValue_Sheet1.ActiveRowIndex, 1);
        }

        private void ss4_CellClick(object sender, CellClickEventArgs e)
        {
            int nSeqNo = (int)VB.Val(ss4_Sheet1.Cells[e.Row, 2].Text);
            if (nSeqNo < 1)
                return;
        }

        private void ss4_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (VB.Left(ss4_Sheet1.Cells[e.Row, 1].Text, 3) == "[삭제")
            {
                ComFunc.MsgBoxEx(this, "삭제된 코드가 포함되어 있습니다. 약속처방을 정리 후 다시 전송하시기 바랍니다.");
                return;
            }

            int nSeqNo = (int)VB.Val(ss4_Sheet1.Cells[e.Row, 2].Text);
            if (nSeqNo < 1)
                return;

            SET_BAT(nSeqNo);
        }

        private void btnBatView_Click(object sender, EventArgs e)
        {
            READ_BAT();
        }

        private void btnBatDel_Click(object sender, EventArgs e)
        {

        }

        private void cboWard_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void cboWard2_SelectedIndexChanged(object sender, EventArgs e)
        {
            READ_BAT();
        }

        private void cboWard_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            ss3_Sheet1.RowCount = 0;
            ss4_Sheet1.RowCount = 0;
            ssValue_Sheet1.RowCount = 0;
        }

        private void ss3_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader)
            {
                if (ss3_Sheet1.ColumnHeader.Cells[0, 0].Text.Equals("■"))
                {
                    ss3_Sheet1.ColumnHeader.Cells[0, 0].Text = "□";
                    ss3_Sheet1.Cells[0, 0, ss3_Sheet1.RowCount - 1, 0].Text = "False";
                    ss3_Sheet1.Rows[0, ss3_Sheet1.RowCount - 1].BackColor = Color.FromArgb(255, 220, 220);
                }
                else
                {
                    ss3_Sheet1.ColumnHeader.Cells[0, 0].Text = "■";
                    ss3_Sheet1.Cells[0, 0, ss3_Sheet1.RowCount - 1, 0].Text = "True";
                    ss3_Sheet1.Rows[0, ss3_Sheet1.RowCount - 1].BackColor = Color.FromArgb(255, 128, 128);
                }


            }
        }

        private void rdoSelect1_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                GetItemList(SelectFormNo);
            }
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            using (Ftpedt FtpedtX = new Ftpedt())
            {
                string strFile = @"C:\PSMHEXE\Manual_Emr_Suga.pdf";
                string strHost = "/psnfs/psmhexe/manual";
                string strHostFile = "Manual_Emr_Suga.pdf";

                if (FtpedtX.FtpDownload("192.168.100.35", "pcnfs", "pcnfs1", strFile, strHostFile, strHost) == true)
                {
                    System.Diagnostics.Process.Start(strFile);
                }
            }
        }
    }
}
