using ComBase;
using FarPoint.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrBaseRingerIOViewer : Form
    {
        #region //변수선언
        EmrPatient p = null;
        string mstrChartDate = "";
        //ContextMenu PopupMenu = null;
        //int mPopRow = -1;
        //string mstrFormNameGb = "기록지관리";
        string mstrFormNo = "";
        string mstrUpdateNo = "0";
        #endregion //변수선언

        public frmEmrBaseRingerIOViewer()
        {
            InitializeComponent();
        }

        public frmEmrBaseRingerIOViewer(EmrPatient po, string strChartDate, string strFormNo, string strUpdateNo)
        {
            InitializeComponent();
            p = po;
            mstrChartDate = strChartDate;
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
        }

        private void frmEmrBaseRingerIOViewer_Load(object sender, EventArgs e)
        {
            ssView_Sheet1.RowCount = 0;

            if (p != null)
            {
                if (p.medEndDate == "" || p.medEndDate == "99981231" || p.medEndDate == "99991231")
                {
                    string strCurDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
                    dtpOrderDate2.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(strCurDate, "D"));
                    dtpOrderDate1.Value = VB.DateAdd("D", -1, dtpOrderDate1.Value.ToString("yyyy-MM-dd"));
                }
                else
                {
                    dtpOrderDate1.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(p.medFrDate, "D"));
                    dtpOrderDate2.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(p.medEndDate, "D"));
                }
            }
            if (mstrChartDate != "")
            {
                dtpOrderDate2.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(mstrChartDate, "D"));
                dtpOrderDate1.Value = VB.DateAdd("D", -1, dtpOrderDate1.Value.ToString("yyyy-MM-dd"));
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetOrderData();
        }

        /// <summary>
        /// 처방조회
        /// </summary>
        private void GetOrderData()
        {
            ssView_Sheet1.RowCount = 0;

            if (p == null) return;
            if (VB.Val(p.acpNo) == 0) return;

            int i = 0;
            DataTable dt = null;
            string SQL = string.Empty;    //Query문
            string SqlErr = string.Empty; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            string strBun = "'20','23' ";

            #region Query

            #region //입원
            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT ";
            SQL = SQL + ComNum.VBLF + "     'IPD' AS SITEGB, ";
            SQL = SQL + ComNum.VBLF + "      O.ORDERNO,";
            SQL = SQL + ComNum.VBLF + "      O.ORDERCODE,";
            SQL = SQL + ComNum.VBLF + "      TO_CHAR(O.BDATE,'YYYY-MM-DD') AS BDATE,";
            SQL = SQL + ComNum.VBLF + "      S.SUNAMEK,";
            SQL = SQL + ComNum.VBLF + "      O.GBDIV,";
            SQL = SQL + ComNum.VBLF + "      O.GBSTATUS,";
            SQL = SQL + ComNum.VBLF + "      O.GBGROUP, ";
            SQL = SQL + ComNum.VBLF + "      O.ENTDATE, ";
            SQL = SQL + ComNum.VBLF + "      O.ROWID AS ORDROWID ";
            SQL = SQL + ComNum.VBLF + " FROM   ADMIN.OCS_IORDER O, ";
            SQL = SQL + ComNum.VBLF + " (SELECT ACTDATE, INDATE, OUTDATE, GBSTS, WARDCODE, ROOMCODE, PANO, SNAME, SEX, AGE, DEPTCODE, IPDNO ";
            SQL = SQL + ComNum.VBLF + "  FROM ADMIN.IPD_NEW_MASTER ";
            SQL = SQL + ComNum.VBLF + "  WHERE IPDNO = (SELECT ";
            SQL = SQL + ComNum.VBLF + "                     MAX(IPDNO) ";
            SQL = SQL + ComNum.VBLF + "                  FROM ADMIN.IPD_NEW_MASTER ";
            SQL = SQL + ComNum.VBLF + "                  WHERE(ACTDATE IS NULL OR OUTDATE = TRUNC(SYSDATE))   ";
            SQL = SQL + ComNum.VBLF + " 		         AND PANO = '" + p.ptNo + "')) M,                 ";
            SQL = SQL + ComNum.VBLF + "        ADMIN.BAS_PATIENT P,                           ";
            SQL = SQL + ComNum.VBLF + "        ADMIN.OCS_ORDERCODE           C,                           ";
            SQL = SQL + ComNum.VBLF + "        ADMIN.OCS_ODOSAGE             D,                           ";
            SQL = SQL + ComNum.VBLF + "        ADMIN.OCS_DOCTOR              N,                            ";
            SQL = SQL + ComNum.VBLF + "        ADMIN.BAS_SUN     S   ,";
            SQL = SQL + ComNum.VBLF + "        ADMIN.DRUG_MASTER2 F   ";
            SQL = SQL + ComNum.VBLF + "  WHERE PTNO = '" + p.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "   AND O.BUN IN ( " + strBun + " ) ";
            SQL = SQL + ComNum.VBLF + "   AND O.BDATE >= TO_DATE('" + dtpOrderDate1.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') - 1";
            SQL = SQL + ComNum.VBLF + "   AND O.BDATE <= TO_DATE('" + dtpOrderDate2.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
            SQL = SQL + ComNum.VBLF + "   AND  (O.GBPRN IN  NULL OR O.GBPRN <> 'P') ";
            if (p.ward == "ER")
            {
                SQL = SQL + ComNum.VBLF + " AND  (  O.ORDERSITE  IN ('ERO')   OR  O.GBIOE IN ('E','EI') )";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + " AND  (O.ORDERSITE  NOT IN ( 'CAN','DC0','DC1','DC2','OPDX') OR O.ORDERSITE IS NULL )";
            }
            SQL = SQL + ComNum.VBLF + "  AND    O.GBPRN <>'S' "; //'JJY 추가(2000/05/22 'S는 선수납(선불);
            SQL = SQL + ComNum.VBLF + "  AND   ( O.GBSTATUS NOT IN ('D-','D+' )  OR  (  O.GBSTATUS = 'D' AND   ACTDIV >'0' ) )  ";
            SQL = SQL + ComNum.VBLF + "  AND    O.PTNO       =  M.PANO           ";
            SQL = SQL + ComNum.VBLF + "  AND  O.GBPICKUP = '*' ";
            SQL = SQL + ComNum.VBLF + "  AND  ( O.VERBC IS NULL OR O.VERBC <>'Y' )";
            if (p.ward == "HD")
            {
                SQL = SQL + ComNum.VBLF + " AND ( O.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL)";
            }
            else if (p.ward == "ENDO")
            {
                SQL = SQL + ComNum.VBLF + " AND O.DOSCODE IN ( SELECT DOSCODE FROM ADMIN.OCS_ODOSAGE WHERE WARDCODE IN ( 'EN') ) ";
                SQL = SQL + ComNum.VBLF + " AND ( O.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL)";
            }
            else if (p.ward == "CT/MRI")
            {
                SQL = SQL + ComNum.VBLF + " AND O.DOSCODE IN ( SELECT DOSCODE FROM ADMIN.OCS_ODOSAGE WHERE WARDCODE IN ( 'RD') ) ";
                SQL = SQL + ComNum.VBLF + " AND ( O.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL)";
            }
            else if (p.ward == "OP" || p.ward == "AG")
            {
                //    '수술방은 모든 오더 보이도록 처리 추후 보완 예정;
            }
            else if (p.ward == "RA")
            {
                SQL = SQL + ComNum.VBLF + " AND ( O.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL)";
            }
            else if (p.ward == "MICU")
            {
                SQL = SQL + ComNum.VBLF + " AND M.WARDCODE ='IU'";
                SQL = SQL + ComNum.VBLF + " AND M.ROOMCODE ='234'";
                SQL = SQL + ComNum.VBLF + " AND ( O.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL)";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + " AND ( O.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL)";
                if (p.ward == "SICU")
                {
                    SQL = SQL + ComNum.VBLF + " AND M.WARDCODE ='IU'   ";
                    SQL = SQL + ComNum.VBLF + " AND M.ROOMCODE ='233'";
                }
                else if (p.ward != "ER")
                {
                    if (p.ward == "IQ" || p.ward == "ND" || p.ward == "NR")
                    {
                        SQL = SQL + ComNum.VBLF + " AND  M.WARDCODE IN ('IQ','ND','NR')";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " AND  M.WARDCODE = '" + p.ward.Trim() + "' ";
                    }
                }
            }
            SQL = SQL + ComNum.VBLF + "  AND   O.QTY  <>  '0'    ";
            SQL = SQL + ComNum.VBLF + "  AND    M.GBSTS NOT IN  ('9') "; //" '입원취소 제외;
            SQL = SQL + ComNum.VBLF + "  AND    O.PTNO       =  P.PANO(+)        ";
            SQL = SQL + ComNum.VBLF + "  AND    O.SLIPNO     =  C.SLIPNO(+)      ";
            SQL = SQL + ComNum.VBLF + "  AND    O.ORDERCODE  =  C.ORDERCODE(+)   ";
            SQL = SQL + ComNum.VBLF + "  AND    (C.SENDDEPT  !=  'N' OR C.SENDDEPT IS NULL)   ";
            SQL = SQL + ComNum.VBLF + "  AND    O.DOSCODE    =  D.DOSCODE(+)     ";
            SQL = SQL + ComNum.VBLF + "  AND    O.DRCODE      =  N.SABUN(+)      ";
            SQL = SQL + ComNum.VBLF + "  AND    O.SUCODE = S.SUNEXT(+) ";
            SQL = SQL + ComNum.VBLF + "  AND O.ORDERCODE = F.JEPCODE ";
            SQL = SQL + ComNum.VBLF + "  AND F.SUGABUN = '20'  ";
            SQL = SQL + ComNum.VBLF + "  AND F.JEHYENGBUN = '02' ";
            if (chkAll.Checked == false)
            {
                SQL = SQL + ComNum.VBLF + "  AND EXISTS ( SELECT 1 FROM ADMIN.AEMRBIOFLUID FF ";
                SQL = SQL + ComNum.VBLF + "                WHERE FF.ACPNO = " + p.acpNo;
                SQL = SQL + ComNum.VBLF + "                 AND FF.ORDDATE = TO_CHAR(O.BDATE, 'YYYYMMDD') ";
                SQL = SQL + ComNum.VBLF + "                 AND FF.ORDERCODE = TRIM(O.ORDERCODE) ";
                SQL = SQL + ComNum.VBLF + "                 AND FF.ORDNO = O.ORDERNO ";
                SQL = SQL + ComNum.VBLF + "                 AND FF.DCCLS = '0')  ";
            }
            #endregion //입원

            SQL = SQL + ComNum.VBLF + " UNION ALL ";

            #region //응급실
            SQL = SQL + ComNum.VBLF + " SELECT ";
            SQL = SQL + ComNum.VBLF + "     'ERD' AS SITEGB, ";
            SQL = SQL + ComNum.VBLF + "      O.ORDERNO,";
            SQL = SQL + ComNum.VBLF + "      O.ORDERCODE,";
            SQL = SQL + ComNum.VBLF + "      TO_CHAR(O.BDATE,'YYYY-MM-DD') AS BDATE,";
            SQL = SQL + ComNum.VBLF + "      S.SUNAMEK,";
            SQL = SQL + ComNum.VBLF + "      O.GBDIV,";
            SQL = SQL + ComNum.VBLF + "      O.GBSTATUS,";
            SQL = SQL + ComNum.VBLF + "      O.GBGROUP, ";
            SQL = SQL + ComNum.VBLF + "      O.ENTDATE, ";
            SQL = SQL + ComNum.VBLF + "      O.ROWID AS ORDROWID ";
            SQL = SQL + ComNum.VBLF + " FROM   ADMIN.OCS_IORDER O, ";
            SQL = SQL + ComNum.VBLF + "        ADMIN.OPD_MASTER  M,                           ";
            SQL = SQL + ComNum.VBLF + "        ADMIN.BAS_PATIENT P,                           ";
            SQL = SQL + ComNum.VBLF + "        ADMIN.OCS_ORDERCODE           C,                           ";
            SQL = SQL + ComNum.VBLF + "        ADMIN.OCS_ODOSAGE             D,                           ";
            SQL = SQL + ComNum.VBLF + "        ADMIN.OCS_DOCTOR              N,                            ";
            SQL = SQL + ComNum.VBLF + "        ADMIN.BAS_SUN     S  ,";
            SQL = SQL + ComNum.VBLF + "         ADMIN.DRUG_MASTER2 F   ";
            SQL = SQL + ComNum.VBLF + " WHERE  O.PTNO = '" + p.ptNo + "' ";
            SQL = SQL + ComNum.VBLF + "    AND  O.BUN IN ( " + strBun + " ) ";
            SQL = SQL + ComNum.VBLF + "    AND  (O.GBPRN IN  NULL OR O.GBPRN <> 'P') ";
            SQL = SQL + ComNum.VBLF + "    AND  O.ORDERSITE  NOT IN ( 'CAN','DC0','DC1','DC2','OPDX') ";
            SQL = SQL + ComNum.VBLF + "    AND  (  O.ORDERSITE  IN ('ERO')   OR  O.GBIOE IN ('E','EI') )";
            SQL = SQL + ComNum.VBLF + "    AND   O.BDATE >= TO_DATE('" + dtpOrderDate1.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') - 1 ";
            SQL = SQL + ComNum.VBLF + "    AND   O.BDATE <= TO_DATE('" + dtpOrderDate2.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "    AND    O.GBPRN <>'S' ";  //'JJY 추가(2000/05/22 'S는 선수납(선불);
            SQL = SQL + ComNum.VBLF + "    AND   ( O.GBSTATUS NOT IN ('D-','D+' )  OR  (  O.GBSTATUS = 'D' AND   ACTDIV >'0' ) )  ";
            SQL = SQL + ComNum.VBLF + "    AND    O.PTNO       =  M.PANO           ";
            SQL = SQL + ComNum.VBLF + "    AND   O.QTY  <>  '0'    ";
            SQL = SQL + ComNum.VBLF + "    AND  M.ACTDATE = TO_DATE('" + p.medFrDate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "    AND  M.DEPTCODE = 'ER'";
            SQL = SQL + ComNum.VBLF + "    AND   O.GBTFLAG <> 'T'";        //'2010-04-27     양수령수간호사 퇴원약 제외해달라고 함;
            SQL = SQL + ComNum.VBLF + "    AND    O.PTNO       =  P.PANO(+)        ";
            SQL = SQL + ComNum.VBLF + "    AND    O.SLIPNO     =  C.SLIPNO(+)      ";
            SQL = SQL + ComNum.VBLF + "    AND    O.ORDERCODE  =  C.ORDERCODE(+)   ";
            SQL = SQL + ComNum.VBLF + "    AND    (C.SENDDEPT  !=  'N' OR C.SENDDEPT IS NULL)   ";
            SQL = SQL + ComNum.VBLF + "    AND    O.DOSCODE    =  D.DOSCODE(+)     ";
            SQL = SQL + ComNum.VBLF + "    AND    O.DRCODE      =  N.SABUN(+)      ";
            SQL = SQL + ComNum.VBLF + "    AND    O.SUCODE = S.SUNEXT(+) ";
            SQL = SQL + ComNum.VBLF + "    AND O.ORDERCODE = F.JEPCODE ";
            SQL = SQL + ComNum.VBLF + "    AND F.SUGABUN = '20'  ";
            SQL = SQL + ComNum.VBLF + "    AND F.JEHYENGBUN = '02' ";
            if (chkAll.Checked == false)
            {
                SQL = SQL + ComNum.VBLF + "  AND EXISTS ( SELECT 1 FROM ADMIN.AEMRBIOFLUID FF ";
                SQL = SQL + ComNum.VBLF + "                WHERE FF.ACPNO = " + p.acpNo;
                SQL = SQL + ComNum.VBLF + "                 AND FF.ORDDATE = TO_CHAR(O.BDATE, 'YYYYMMDD') ";
                SQL = SQL + ComNum.VBLF + "                 AND FF.ORDERCODE = TRIM(O.ORDERCODE) ";
                SQL = SQL + ComNum.VBLF + "                 AND FF.ORDNO = O.ORDERNO ";
                SQL = SQL + ComNum.VBLF + "                 AND FF.DCCLS = '0')  ";
            }
            
            #endregion //응급실

            //SQL = SQL + ComNum.VBLF + " UNION ALL ";

            #region //수술실
            //SQL = SQL + ComNum.VBLF + "SELECT ";
            //SQL = SQL + ComNum.VBLF + "    'OPR' AS SITEGB, ";
            //SQL = SQL + ComNum.VBLF + "    0 AS ORDERNO, ";
            //SQL = SQL + ComNum.VBLF + "    O.JEPCODE AS ORDERCODE, ";
            //SQL = SQL + ComNum.VBLF + "    TO_CHAR(O.OPDATE,'YYYY-MM-DD') AS BDATE , ";
            //SQL = SQL + ComNum.VBLF + "    J.NAME AS SUNAMEK,  ";
            //SQL = SQL + ComNum.VBLF + "    O.QTY AS GBDIV, ";
            //SQL = SQL + ComNum.VBLF + "    '' AS GBSTATUS, ";
            //SQL = SQL + ComNum.VBLF + "    '' AS GBGROUP, ";
            //SQL = SQL + ComNum.VBLF + "    O.ENTDATE, ";
            //SQL = SQL + ComNum.VBLF + "    O.ROWID AS ORDROWID ";
            //SQL = SQL + ComNum.VBLF + "    --C.UNIT,   ";
            //SQL = SQL + ComNum.VBLF + "FROM ADMIN.ORAN_SLIP O ";
            //SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.OPR_BUSEJEPUM J ";
            //SQL = SQL + ComNum.VBLF + "    ON O.JEPCODE = J.JEPCODE ";
            //SQL = SQL + ComNum.VBLF + "    AND J.BUCODE = '033103'  ";
            //SQL = SQL + ComNum.VBLF + "    AND J.BUN = '04' ";
            //SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN ADMIN.BAS_SUT B    ";
            //SQL = SQL + ComNum.VBLF + "   ON J.JEPCODE = B.SUCODE    ";
            //SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN ADMIN.BAS_SUN C    ";
            //SQL = SQL + ComNum.VBLF + "   ON B.SUNEXT = C.SUNEXT     ";
            //SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN ADMIN.ORD_JEP D    ";
            //SQL = SQL + ComNum.VBLF + "   ON J.JEPCODE = D.JEPCODE  ";
            //SQL = SQL + ComNum.VBLF + "WHERE O.PANO = '" + p.ptNo + "' ";
            //SQL = SQL + ComNum.VBLF + "    AND   O.OPDATE >= TO_DATE('" + dtpOrderDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') - 1 ";
            //SQL = SQL + ComNum.VBLF + "    AND   O.OPDATE <= TO_DATE('" + dtpOrderDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            #endregion //수술실

            if (chkDesc.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + " ORDER BY BDATE DESC, SITEGB, GBGROUP, GBDIV, ENTDATE     ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + " ORDER BY BDATE, SITEGB, GBGROUP, GBDIV, ENTDATE     ";
            }

            #endregion Query

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

            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssView_Sheet1.RowCount += 1;
                ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = ""; //ACTSEQ
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["SITEGB"].ToString().Trim();
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["ORDROWID"].ToString().Trim();
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["ORDERNO"].ToString().Trim();
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim();
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                if (dt.Rows[i]["SITEGB"].ToString().Trim() == "OPR")
                {
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = "[" + "수술" + "] " + dt.Rows[i]["SUNAMEK"].ToString().Trim();
                }
                else if (dt.Rows[i]["SITEGB"].ToString().Trim() == "ERD")
                {
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = "[" + "응급실" + "] " + dt.Rows[i]["SUNAMEK"].ToString().Trim();
                }
                else
                {
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = "[" + "병동" + "] " + dt.Rows[i]["SUNAMEK"].ToString().Trim();
                }

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = dt.Rows[i]["GBDIV"].ToString().Trim();


                if (dt.Rows[i]["GBSTATUS"].ToString().Trim() == "D" || dt.Rows[i]["GBSTATUS"].ToString().Trim() == "D-") //DC표기
                {
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = "(D/C)" + ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text.Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.Columns.Count - 1].ForeColor = Color.Red;
                }
                
                string pstrOrderDate = dt.Rows[i]["BDATE"].ToString().Replace("-", "").Trim();
                double pOrderNo = VB.Val(dt.Rows[i]["ORDERNO"].ToString().Trim());
                string pstrOrderCode = dt.Rows[i]["ORDERCODE"].ToString().Trim();
                string pstrGbGroup = dt.Rows[i]["GBGROUP"].ToString().Trim();

                if (dt.Rows[i]["SITEGB"].ToString().Trim() != "OPR" && dt.Rows[i]["GBGROUP"].ToString().Trim().Length > 0)
                {
                    GetDateMixInfoEx(ssView_Sheet1.RowCount - 1, pstrOrderDate, pOrderNo, pstrOrderCode, pstrGbGroup);
                }

                GetDateActInfo(ssView_Sheet1.RowCount - 1, pstrOrderDate, pOrderNo, pstrOrderCode);

                ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, 44);
            }

            dt.Dispose();
            dt = null;

            GetOrderDataSpen();

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// 수액 믹스 정보 표시 : 처방정보와 사용자가 믹스 한것을 표시한다.
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="pstrOrderDate"></param>
        /// <param name="pOrderNo"></param>
        /// <param name="pstrOrderCode"></param>
        /// <param name="pstrGbGroup"></param>
        private void GetDateMixInfoEx(int Row, string pstrOrderDate, double pOrderNo, string pstrOrderCode, string pstrGbGroup)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "    O.ORDERCODE,  ";
            SQL = SQL + ComNum.VBLF + "    C.ORDERNAME, C.ORDERNAMES, ";
            SQL = SQL + ComNum.VBLF + "    S.SUNAMEK  ";
            SQL = SQL + ComNum.VBLF + "  FROM ADMIN.OCS_IORDER O ";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.OCS_ORDERCODE C ";
            SQL = SQL + ComNum.VBLF + "       ON O.ORDERCODE = C.ORDERCODE ";
            SQL = SQL + ComNum.VBLF + "      AND O.SLIPNO     =  C.SLIPNO   ";
            SQL = SQL + ComNum.VBLF + "      AND O.ORDERCODE  =  C.ORDERCODE  ";
            SQL = SQL + ComNum.VBLF + "      AND (C.SENDDEPT  !=  'N' OR C.SENDDEPT IS NULL)   ";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.BAS_SUN S ";
            SQL = SQL + ComNum.VBLF + "       ON O.SUCODE = S.SUNEXT ";
            SQL = SQL + ComNum.VBLF + " WHERE O.PTNO = '" + p.ptNo + "' ";
            SQL = SQL + ComNum.VBLF + "   AND O.BUN IN ( '20','23'  )  ";
            SQL = SQL + ComNum.VBLF + "   AND O.BDATE = TO_DATE('" + pstrOrderDate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "   AND (O.GBPRN IN  NULL OR O.GBPRN <> 'P')  ";
            SQL = SQL + ComNum.VBLF + "   AND (O.ORDERSITE  NOT IN ( 'CAN','DC0','DC1','DC2','OPDX') OR O.ORDERSITE IS NULL ) ";
            SQL = SQL + ComNum.VBLF + "   AND O.GBPRN <>'S'  ";
            SQL = SQL + ComNum.VBLF + "   AND ( O.GBSTATUS NOT IN ('D-','D+' )  OR  (  O.GBSTATUS = 'D' AND O.ACTDIV >'0' ) )   ";
            SQL = SQL + ComNum.VBLF + "   AND O.GBPICKUP = '*'  ";
            SQL = SQL + ComNum.VBLF + "   AND ( O.VERBC IS NULL OR O.VERBC <>'Y' ) ";
            SQL = SQL + ComNum.VBLF + "   AND ( O.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL) ";
            SQL = SQL + ComNum.VBLF + "   AND O.QTY  <>  '0'   ";
            SQL = SQL + ComNum.VBLF + "   AND O.ORDERNO <> " + pOrderNo;
            SQL = SQL + ComNum.VBLF + "   AND O.GBGROUP IS NOT NULL ";
            SQL = SQL + ComNum.VBLF + "   AND O.GBGROUP = '" + pstrGbGroup + "'";
            //SQL = SQL + ComNum.VBLF + "	   AND O.GBGROUP = (SELECT MAX(O1.GBGROUP)  ";
            //SQL = SQL + ComNum.VBLF + "                        FROM ADMIN.OCS_IORDER O1 ";
            //SQL = SQL + ComNum.VBLF + "                        WHERE O1.PTNO = '" + p.ptNo + "' ";
            //SQL = SQL + ComNum.VBLF + "                            AND O1.BDATE = TO_DATE('" + pstrOrderDate + "','YYYY-MM-DD') ";
            //SQL = SQL + ComNum.VBLF + "                            AND O1.ORDERNO = " + pOrderNo;
            //SQL = SQL + ComNum.VBLF + "    					       AND (O1.GBPRN IN  NULL OR O1.GBPRN <> 'P')  ";
            //SQL = SQL + ComNum.VBLF + "    					       AND (O1.ORDERSITE  NOT IN ( 'CAN','DC0','DC1','DC2','OPDX') OR O1.ORDERSITE IS NULL ) ";
            //SQL = SQL + ComNum.VBLF + "    					       AND O1.GBPRN <>'S'  ";
            //SQL = SQL + ComNum.VBLF + "    					       AND ( O1.GBSTATUS NOT IN ('D-','D+' )  OR  (  O1.GBSTATUS = 'D' AND   O1.ACTDIV >'0' ) )   ";
            //SQL = SQL + ComNum.VBLF + "    					       AND O1.GBPICKUP = '*'  ";
            //SQL = SQL + ComNum.VBLF + "    					       AND ( O1.VERBC IS NULL OR O1.VERBC <>'Y' ) ";
            //SQL = SQL + ComNum.VBLF + "    					       AND ( O1.GBIOE NOT IN ('E','EI') OR O1.GBIOE IS NULL) ";
            //SQL = SQL + ComNum.VBLF + "    					       AND O1.QTY  <>  '0'  ) ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }

            string strOrderName = ssView_Sheet1.Cells[Row, 6].Text.Trim();
            Font font = new Font("굴림", 10);
            Size TxtSize = TextRenderer.MeasureText(strOrderName, font);
            List<int> lstWidth = new List<int>();
            lstWidth.Add(TxtSize.Width);

            StringBuilder strNote = new StringBuilder();
            strNote.AppendLine(strOrderName);

            for (i = 0; i < dt.Rows.Count; i++)
            {
                strNote.AppendLine(dt.Rows[i]["SUNAMEK"].ToString().Trim());
                //텍스트길이 계산
                TxtSize = TextRenderer.MeasureText(dt.Rows[i]["SUNAMEK"].ToString().Trim(), font);
                //List에 넣기
                lstWidth.Add(TxtSize.Width);

                //ssView_Sheet1.Cells[Row, 4].Text = strOrderName + ComNum.VBLF + dt.Rows[i]["SUNAMEK"].ToString().Trim();
                //strOrderName = ssView_Sheet1.Cells[Row, 4].Text.Trim();
            }

            ssView_Sheet1.Cells[Row, 6].NoteStyle = FarPoint.Win.Spread.NoteStyle.PopupStickyNote;
            ssView_Sheet1.Cells[Row, 6].NoteIndicatorPosition = FarPoint.Win.Spread.NoteIndicatorPosition.TopRight;
            ssView_Sheet1.Cells[Row, 6].NoteIndicatorColor = Color.Pink;
            ssView_Sheet1.Cells[Row, 6].NoteIndicatorSize = new Size(20, 20);
            ssView_Sheet1.Cells[Row, 6].Note = strNote.ToString().Trim();
            FarPoint.Win.Spread.DrawingSpace.StickyNoteStyleInfo nsinfo = new FarPoint.Win.Spread.DrawingSpace.StickyNoteStyleInfo();
            nsinfo = ssView_Sheet1.GetStickyNoteStyleInfo(Row, 6);
            //nsinfo.BackColor = Color.Red;
            nsinfo.Font = font;
            nsinfo.ForeColor = Color.Black;
            nsinfo.Width = lstWidth.Max(); //가장 긴 텍스트 길이에 맞춰서 너비 설정
            nsinfo.ShapeOutlineColor = Color.Red;
            nsinfo.ShapeOutlineThickness = 1;
            nsinfo.ShadowOffsetX = 3;
            nsinfo.ShadowOffsetY = 3;
            ssView_Sheet1.SetStickyNoteStyleInfo(Row, 6, nsinfo);

            dt.Dispose();
            dt = null;

            ssView_Sheet1.SetRowHeight(Row, 44);

        }

        /// <summary>
        /// 수액 액팅정보 표시
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="pstrOrderDate"></param>
        /// <param name="pOrderNo"></param>
        /// <param name="pstrOrderCode"></param>
        private void GetDateActInfo(int Row, string pstrOrderDate, double pOrderNo, string pstrOrderCode)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = " ";
            SQL = SQL + ComNum.VBLF + "SELECT";
            SQL = SQL + ComNum.VBLF + "    F.ACTSEQ, F.ACTGB, F.ACTQTY, F.ACTRMK, ";
            SQL = SQL + ComNum.VBLF + "    F.ACTDATE, F.ACTTIME, F.ACTUSEID, ";
            SQL = SQL + ComNum.VBLF + "    U.USENAME  || '(' || D.NAME || ')' AS USENAME";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBIOFLUID F";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_EMR + "AVIEWEMRUSER U";
            SQL = SQL + ComNum.VBLF + "     ON F.ACTUSEID = U.USEID";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_PMPA + "BAS_BUSE D";
            SQL = SQL + ComNum.VBLF + "     ON TRIM(U.BUSECODE) = TRIM(D.BUCODE)";
            SQL = SQL + ComNum.VBLF + "WHERE F.ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "     AND F.ORDDATE = '" + pstrOrderDate + "'";
            SQL = SQL + ComNum.VBLF + "     AND F.ORDERCODE = '" + pstrOrderCode + "'";
            SQL = SQL + ComNum.VBLF + "     AND F.ORDNO = " + pOrderNo;
            SQL = SQL + ComNum.VBLF + "     AND F.DCCLS = '0' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY F.ACTGB, (F.ACTDATE || F.ACTTIME) "; ;

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (i > 0)
                {
                    ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                    ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 1].Text;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 2].Text;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 3].Text;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 4].Text;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 5].Text;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 6].Text;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 7].Text;
                }

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["ACTSEQ"].ToString().Trim();

                if (dt.Rows[i]["ACTGB"].ToString().Trim() == "00")
                {
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = "시작";
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].BackColor = System.Drawing.Color.LightGreen;
                }
                else if (dt.Rows[i]["ACTGB"].ToString().Trim() == "01")
                {
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = "유지";
                }
                else if (dt.Rows[i]["ACTGB"].ToString().Trim() == "02")
                {
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = "종료";
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].BackColor = System.Drawing.Color.LightPink;
                }
                else if (dt.Rows[i]["ACTGB"].ToString().Trim() == "03")
                {
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = "원타임";
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].BackColor = System.Drawing.Color.LightPink;
                }

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = ComFunc.FormatStrToDate(dt.Rows[i]["ACTDATE"].ToString().Trim(), "D") + " " + ComFunc.FormatStrToDate(dt.Rows[i]["ACTTIME"].ToString().Trim(), "M");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = dt.Rows[i]["ACTQTY"].ToString().Trim();
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = dt.Rows[i]["USENAME"].ToString().Trim();
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12].Text = dt.Rows[i]["ACTRMK"].ToString().Trim();
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13].Text = dt.Rows[i]["ACTUSEID"].ToString().Trim();
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 14].Text = dt.Rows[i]["ACTDATE"].ToString().Trim();
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 15].Text = dt.Rows[i]["ACTTIME"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;
        }

        /// <summary>
        /// 스프래드를 병합한다
        /// </summary>
        private void GetOrderDataSpen()
        {
            //string ppSITEGB = "";
            string pOrderNo = "";
            string pstrOrderDate = "";

            int intOrderNo = 0;
            int intOrderDate = 0;

            if (ssView_Sheet1.RowCount < 0) return;

            LineBorder lineBorder = new LineBorder(System.Drawing.Color.Black, 2, false, false, false, true);


            for (int i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                if (pstrOrderDate != ssView_Sheet1.Cells[i, 5].Text.Trim())
                {
                    pstrOrderDate = ssView_Sheet1.Cells[i, 5].Text.Trim();

                    if (intOrderDate != 0)
                    {
                        if (i - intOrderDate - 1 < 0)
                        {
                            ssView_Sheet1.Cells[0, 5].RowSpan = intOrderDate;
                            ssView_Sheet1.Cells[0, 5].Border = lineBorder;
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i - intOrderDate - 1, 5].RowSpan = intOrderDate + 1;
                            ssView_Sheet1.Cells[i - intOrderDate - 1, 5].Border = lineBorder;
                            ssView_Sheet1.Rows[intOrderDate].Border = lineBorder;
                        }
                    }

                    pOrderNo = ssView_Sheet1.Cells[i, 3].Text.Trim();

                    if (intOrderNo != 0)
                    {
                        if (i - intOrderNo - 1 < 0)
                        {
                            ssView_Sheet1.Cells[0, 3].RowSpan = intOrderNo;
                            ssView_Sheet1.Cells[0, 4].RowSpan = intOrderNo;
                            ssView_Sheet1.Cells[0, 6].RowSpan = intOrderNo;
                            ssView_Sheet1.Cells[0, 7].RowSpan = intOrderNo;
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i - intOrderNo - 1, 3].RowSpan = intOrderNo + 1;
                            ssView_Sheet1.Cells[i - intOrderNo - 1, 4].RowSpan = intOrderNo + 1;
                            ssView_Sheet1.Cells[i - intOrderNo - 1, 6].RowSpan = intOrderNo + 1;
                            ssView_Sheet1.Cells[i - intOrderNo - 1, 7].RowSpan = intOrderNo + 1;

                        }
                    }

                    intOrderNo = 0;
                    intOrderDate = 0;
                }
                else
                {
                    if (i == ssView_Sheet1.RowCount - 1)
                    {
                        ssView_Sheet1.Cells[i - intOrderDate - 1, 5].RowSpan = intOrderDate + 2;
                    }

                    intOrderDate = intOrderDate + 1;

                    if (pOrderNo != ssView_Sheet1.Cells[i, 3].Text.Trim())
                    {
                        pOrderNo = ssView_Sheet1.Cells[i, 3].Text.Trim();

                        if (intOrderNo != 0)
                        {
                            if (i - intOrderNo - 1 < 0)
                            {
                                ssView_Sheet1.Cells[0, 3].RowSpan = intOrderNo;
                                ssView_Sheet1.Cells[0, 4].RowSpan = intOrderNo;
                                ssView_Sheet1.Cells[0, 6].RowSpan = intOrderNo;
                                ssView_Sheet1.Cells[0, 7].RowSpan = intOrderNo;
                            }
                            else
                            {
                                ssView_Sheet1.Cells[i - intOrderNo - 1, 3].RowSpan = intOrderNo + 1;
                                ssView_Sheet1.Cells[i - intOrderNo - 1, 4].RowSpan = intOrderNo + 1;
                                ssView_Sheet1.Cells[i - intOrderNo - 1, 6].RowSpan = intOrderNo + 1;
                                ssView_Sheet1.Cells[i - intOrderNo - 1, 7].RowSpan = intOrderNo + 1;
                            }
                        }

                        intOrderNo = 0;
                    }
                    else
                    {
                        if (i == ssView_Sheet1.RowCount - 1)
                        {
                            ssView_Sheet1.Cells[i - intOrderNo - 1, 3].RowSpan = intOrderNo + 2;
                            ssView_Sheet1.Cells[i - intOrderNo - 1, 4].RowSpan = intOrderNo + 2;
                            ssView_Sheet1.Cells[i - intOrderNo - 1, 6].RowSpan = intOrderNo + 2;
                            ssView_Sheet1.Cells[i - intOrderNo - 1, 7].RowSpan = intOrderNo + 2;
                        }
                        intOrderNo = intOrderNo + 1;
                    }
                }
            }
        }

    }
}
