using ComBase;
using Oracle.DataAccess.Client;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// Class Name      : ComEmrBase
    /// File Name       : FrmCompleteAfterModfiy
    /// Description     : 검수완료 퇴원차트 수정.삭제 리스트
    /// Author          : 이현종
    /// Create Date     : 2019-08-29
    /// Update History  : 2020-03-13 쿼리 및 화면 수정
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso cref= " PSMH\mtsEmr(FrmCompleteAfterModfiy.frm) >> FrmCompleteAfterModfiy.cs 폼이름 재정의" />
    /// 
    public partial class FrmCompleteAfterModfiy : Form
    {
        //미비 환자 정보 전달
        public delegate void EventCompleUser(string strPtNo, string strOutDate);
        public event EventCompleUser rEventCompleUserSend;

        //창종료 이벤트
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        public FrmCompleteAfterModfiy()
        {
            InitializeComponent();
        }

        private void FrmCompleteAfterModfiy_Load(object sender, EventArgs e)
        {
            SS1_Sheet1.RowCount = 0;

            dtpSDATE.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).AddDays(-1);
            dtpEDATE.Value = dtpSDATE.Value;

            txtPtno.Clear();
            txtSname.Clear();
        }


        private void TxtPtno_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtPtno.Text.Trim().Length == 0)
                    return;

                txtPtno.Text = ComFunc.SetAutoZero(txtPtno.Text, 8);

                txtSname.Text = GetPatientName(txtPtno.Text.Trim());
            }
        }

        /// <summary>
        /// 환자 이름 가져오기
        /// </summary>
        /// <param name="strPano"></param>
        /// <returns></returns>
        private string GetPatientName(string strPano)
        {
            string rtnVal = string.Empty;
            string strSql = string.Empty;
            OracleDataReader reader = null;

            strSql = " SELECT SNAME";
            strSql = strSql + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT";
            strSql = strSql + ComNum.VBLF + "  WHERE PANO = '" + strPano + "'";

            string sqlErr = clsDB.GetAdoRs(ref reader, strSql, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                clsDB.SaveSqlErrLog(sqlErr, sqlErr, clsDB.DbCon);
                ComFunc.MsgBox(sqlErr);
                return rtnVal;
            }

            if (reader.HasRows && reader.Read())
            {
                rtnVal = reader.GetValue(0).ToString().Trim();
            }

            reader.Dispose();

            return rtnVal;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
             GetSearhData();
        }

        void GetSearhData()
        {

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            OracleDataReader reader = null;

            SS1_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                #region 쿼리 수정
                SQL += ComNum.VBLF + "  SELECT (M.WRITEDATE || ' ' || SUBSTR(M.WRITETIME, 0, 4)) AS HISDATE , A.PTNO, I.SNAME, TO_CHAR(I.OUTDATE, 'YYYY-MM-DD') OUTDATE, I.DEPTCODE";
                SQL += ComNum.VBLF + ", (SELECT NAME FROM KOSMOS_EMR.EMR_USERT WHERE USERID = LTRIM(M.USEID, '0')) AS NAME";
                SQL += ComNum.VBLF + ", M.USEID, (B.CHARTDATE || ' ' || SUBSTR(B.CHARTTIME, 0, 4)) AS WRITEDATE";
                SQL += ComNum.VBLF + ", TO_CHAR(CDATE, 'YYYY-MM-DD HH24:MI') CDATE";
                SQL += ComNum.VBLF + ", (SELECT KORNAME FROM KOSMOS_ADM.INSA_MST WHERE SABUN3 = A.CSABUN) AS CNAME";
                SQL += ComNum.VBLF + ", A.EMRNO";
                SQL += ComNum.VBLF + "    FROM KOSMOS_EMR.EMRXML_COMPLETE A ";
                SQL += ComNum.VBLF + "      INNER JOIN KOSMOS_EMR.EMRXMLMST_HISTORY B ";
                SQL += ComNum.VBLF + "         ON A.EMRNO = B.EMRNO";
                SQL += ComNum.VBLF + "      INNER JOIN KOSMOS_PMPA.IPD_NEW_MASTER I ";
                SQL += ComNum.VBLF + "         ON I.PANO = A.PTNO";
                SQL += ComNum.VBLF + "        AND I.INDATE >= TO_DATE(A.MEDFRDATE || ' 00:00','YYYY-MM-DD HH24:MI') ";
                SQL += ComNum.VBLF + "        AND I.INDATE <= TO_DATE(A.MEDFRDATE || ' 23:59','YYYY-MM-DD HH24:MI') ";
                SQL += ComNum.VBLF + "        AND GBSTS NOT IN ('9') ";
                SQL += ComNum.VBLF + "      INNER JOIN KOSMOS_EMR.EMRXMLMST M ";
                SQL += ComNum.VBLF + "         ON A.PTNO = M.PTNO";
                SQL += ComNum.VBLF + "        AND A.MEDFRDATE = M.MEDFRDATE";
                SQL += ComNum.VBLF + "        AND M.FORMNO = 1647";
                SQL += ComNum.VBLF + "        AND M.WRITEDATE >= '" + dtpSDATE.Value.ToString("yyyyMMdd") + "'";
                SQL += ComNum.VBLF + "        AND M.WRITEDATE <= '" + dtpEDATE.Value.ToString("yyyyMMdd") + "'";
                SQL += ComNum.VBLF + "        AND NOT EXISTS";
                SQL += ComNum.VBLF + "        (";
                SQL += ComNum.VBLF + "          SELECT 1";
                SQL += ComNum.VBLF + "            FROM KOSMOS_EMR.EMRXML_COMPLETE";
                SQL += ComNum.VBLF + "           WHERE EMRNO = M.EMRNO";
                SQL += ComNum.VBLF + "        )";
                SQL += ComNum.VBLF + " WHERE A.CDATE >= TO_DATE('1900-01-01','YYYY-MM-DD') ";
                //SQL += ComNum.VBLF + " WHERE A.CDATE >= TO_DATE('" + dtpSDATE.Value.ToShortDateString() + "','YYYY-MM-DD') ";
                //SQL += ComNum.VBLF + "   AND A.CDATE <= TO_DATE('" + dtpEDATE.Value.ToShortDateString() + "','YYYY-MM-DD') ";
                if (txtSname.Text.Trim().Length > 0)
                {
                    SQL += ComNum.VBLF + "   AND A.PTNO = '" + txtPtno.Text.Trim() + "' ";
                }

                #region 신규
                SQL += ComNum.VBLF + " UNION ALL";
                SQL += ComNum.VBLF + "  SELECT (M.WRITEDATE || ' ' || SUBSTR(M.WRITETIME, 0, 4)) AS HISDATE , A.PTNO, I.SNAME, TO_CHAR(I.OUTDATE, 'YYYY-MM-DD') OUTDATE, I.DEPTCODE";
                SQL += ComNum.VBLF + ", (SELECT NAME FROM KOSMOS_EMR.EMR_USERT WHERE USERID = M.CHARTUSEID) AS NAME";
                SQL += ComNum.VBLF + ", M.CHARTUSEID, (B.CHARTDATE || ' ' || SUBSTR(B.CHARTTIME, 0, 4)) AS WRITEDATE";
                SQL += ComNum.VBLF + ", TO_CHAR(CDATE, 'YYYY-MM-DD HH24:MI') CDATE";
                SQL += ComNum.VBLF + ", (SELECT KORNAME FROM KOSMOS_ADM.INSA_MST WHERE SABUN3 = A.CSABUN) AS CNAME";
                SQL += ComNum.VBLF + ", A.EMRNO";
                SQL += ComNum.VBLF + "    FROM KOSMOS_EMR.EMRXML_COMPLETE A ";
                SQL += ComNum.VBLF + "      INNER JOIN KOSMOS_EMR.AEMRCHARTMSTHIS B ";
                SQL += ComNum.VBLF + "         ON A.EMRNOHIS = B.EMRNOHIS";
                SQL += ComNum.VBLF + "        AND B.DCDATE >= '" + dtpSDATE.Value.ToString("yyyyMMdd") + "'";
                SQL += ComNum.VBLF + "        AND B.DCDATE <= '" + dtpEDATE.Value.ToString("yyyyMMdd") + "'";
                SQL += ComNum.VBLF + "      INNER JOIN KOSMOS_EMR.AEMRCHARTMST M ";
                SQL += ComNum.VBLF + "         ON A.EMRNO = M.EMRNO";
                SQL += ComNum.VBLF + "        AND A.EMRNOHIS <> M.EMRNOHIS";
                SQL += ComNum.VBLF + "      INNER JOIN KOSMOS_PMPA.IPD_NEW_MASTER I ";
                SQL += ComNum.VBLF + "         ON I.PANO = A.PTNO";
                SQL += ComNum.VBLF + "        AND EXISTS ";
                SQL += ComNum.VBLF + "        (";
                SQL += ComNum.VBLF + "          SELECT 1";
                SQL += ComNum.VBLF + "            FROM KOSMOS_PMPA.IPD_NEW_MASTER";
                SQL += ComNum.VBLF + "           WHERE PANO  = M.PTNO ";
                SQL += ComNum.VBLF + "             AND I.INDATE >= TO_DATE(A.MEDFRDATE || ' 00:00','YYYY-MM-DD HH24:MI') ";
                SQL += ComNum.VBLF + "             AND I.INDATE <= TO_DATE(A.MEDFRDATE || ' 23:59','YYYY-MM-DD HH24:MI') ";
                SQL += ComNum.VBLF + "             AND GBSTS NOT IN ('9') ";
                SQL += ComNum.VBLF + "        )";
                SQL += ComNum.VBLF + " WHERE A.CDATE >= TO_DATE('2020-01-01','YYYY-MM-DD') ";
                if (txtSname.Text.Trim().Length > 0)
                {
                    SQL += ComNum.VBLF + "   AND A.PTNO = '" + txtPtno.Text.Trim() + "' ";
                }
                #endregion


                SQL += ComNum.VBLF + "  ORDER BY HISDATE";
                SqlErr = GetAdoRs(ref reader, SQL);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        SS1_Sheet1.RowCount += 1;

                        //HISTORYWRITEDATE, HISTORYWRITETIME
                        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 1].Text = DateTime.ParseExact(reader.GetValue(0).ToString().Trim(), "yyyyMMdd HHmm", null).ToString("yyyy-MM-dd HH:mm");
                        //PTNO
                        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 2].Text = reader.GetValue(1).ToString().Trim();
                        //SNAME
                        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 3].Text = reader.GetValue(2).ToString().Trim();
                        //OUTDATE
                        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 4].Text = reader.GetValue(3).ToString().Trim();
                        //DEPTCODE
                        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 5].Text = reader.GetValue(4).ToString().Trim();
                        //KORNAME
                        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 6].Text = reader.GetValue(5).ToString().Trim();
                        //USEID
                        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 7].Text = reader.GetValue(6).ToString().Trim();
                        //CHARTDATE +  CHARTTIME
                        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 8].Text = DateTime.ParseExact(reader.GetValue(7).ToString().Trim(), "yyyyMMdd HHmm", null).ToString("yyyy-MM-dd HH:mm");
                        //FORMNAME
                        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 9].Text = "입퇴원 요약지";
                        //검수 완료일(CDATE)
                        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 10].Text = reader.GetValue(8).ToString().Trim();
                        //검수자
                        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 11].Text = reader.GetValue(9).ToString().Trim();
                    }
                }
                else
                {
                    ComFunc.MsgBoxEx(this, "내역이 없습니다.");
                }

                reader.Dispose();
                #endregion

                #region 첫번째 읽음
                //SQL = " SELECT EMRNO, ROWID ";
                //SQL += ComNum.VBLF + "    FROM KOSMOS_EMR.EMRXML_COMPLETE A ";
                //SQL += ComNum.VBLF + " WHERE NOT EXISTS ";
                //SQL += ComNum.VBLF + " (SELECT * FROM KOSMOS_EMR.EMRXML B ";
                //SQL += ComNum.VBLF + " WHERE A.EMRNO = B.EMRNO) ";
                //SQL += ComNum.VBLF + "      AND A.CDATE >= TO_DATE('" + dtpSDATE.Value.ToShortDateString() + "','YYYY-MM-DD') ";
                //SQL += ComNum.VBLF + "      AND A.CDATE <= TO_DATE('" + dtpEDATE.Value.ToShortDateString() + "','YYYY-MM-DD') ";

                //if (txtSname.Text.Trim().Length > 0)
                //{
                //    SQL += ComNum.VBLF + "  AND A.PTNO = '" + txtPtno.Text.Trim() + "' ";
                //}
                //SQL += ComNum.VBLF + "  ORDER BY CDATE ASC ";

                //SqlErr = GetAdoRs(ref reader, SQL);
                //if (SqlErr != "")
                //{
                //    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                //    Cursor.Current = Cursors.Default;
                //    return;
                //}

                //if (reader.HasRows)
                //{

                //    while(reader.Read())
                //    {
                //        SS1_Sheet1.RowCount += 1;
                //        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 12].Text = reader.GetValue(1).ToString().Trim();

                //        #region EMRXMLHISTORY READ
                //        SQL = " SELECT A.HISTORYWRITEDATE, A.HISTORYWRITETIME, A.CHARTDATE, A.CHARTTIME, A.PTNO, F.FORMNAME, A.USEID,";
                //        SQL += ComNum.VBLF + " P.SNAME, TO_CHAR(D.OUTDATE, 'YYYY-MM-DD') OUTDATE, D.DEPTCODE,";
                //        SQL += ComNum.VBLF + " B.KORNAME, C.NAME";

                //        SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRXMLHISTORY A";
                //        SQL += ComNum.VBLF + "   INNER JOIN KOSMOS_PMPA.BAS_PATIENT P";
                //        SQL += ComNum.VBLF + "      ON P.PANO = A.PTNO";
                //        SQL += ComNum.VBLF + "   INNER JOIN KOSMOS_ADM.INSA_MST B";
                //        SQL += ComNum.VBLF + "      ON B.SABUN3 = A.USEID";
                //        SQL += ComNum.VBLF + "   INNER JOIN KOSMOS_PMPA.BAS_BUSE C";
                //        SQL += ComNum.VBLF + "      ON C.BUCODE = B.BUSE";
                //        SQL += ComNum.VBLF + "   INNER JOIN KOSMOS_EMR.AEMRFORM F";
                //        SQL += ComNum.VBLF + "      ON F.FORMNO = A.FORMNO";
                //        SQL += ComNum.VBLF + "      AND F.UPDATENO = 1";
                //        SQL += ComNum.VBLF + "   LEFT OUTER JOIN KOSMOS_PMPA.IPD_NEW_MASTER D";
                //        SQL += ComNum.VBLF + "      ON D.PANO = A.PTNO";
                //        SQL += ComNum.VBLF + "      AND INDATE >= TO_DATE(A.MEDFRDATE || ' 00:00','YYYY-MM-DD HH24:MI') ";
                //        SQL += ComNum.VBLF + "      AND INDATE <= TO_DATE(A.MEDFRDATE || ' 23:59','YYYY-MM-DD HH24:MI') ";
                //        SQL += ComNum.VBLF + "      AND GBSTS NOT IN ('9') ";
                //        SQL += ComNum.VBLF + " WHERE EMRNO = " + reader.GetValue(0).ToString().Trim();

                //        SqlErr = GetAdoRs(ref reader2, SQL);
                //        if (SqlErr != "")
                //        {
                //            ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                //            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                //            Cursor.Current = Cursors.Default;
                //            return;
                //        }

                //        if(reader2.HasRows)
                //        {
                //            //HISTORYWRITEDATE, HISTORYWRITETIME
                //            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 1].Text = reader2.GetValue(0).ToString().Trim() + " " +
                //                reader2.GetValue(1).ToString().Trim();
                //            //PTNO
                //            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 2].Text = reader2.GetValue(4).ToString().Trim();
                //            //SNAME
                //            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 3].Text = reader2.GetValue(7).ToString().Trim();
                //            //OUTDATE
                //            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 4].Text = reader2.GetValue(8).ToString().Trim();
                //            //DEPTCODE
                //            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 5].Text = reader2.GetValue(9).ToString().Trim();

                //            //BUSENAME
                //            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 6].Text = reader2.GetValue(11).ToString().Trim();
                //            //KORNAME
                //            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 7].Text = reader2.GetValue(10).ToString().Trim();

                //            //USEID
                //            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 8].Text = reader2.GetValue(6).ToString().Trim();
                //            //CHARTDATE +  CHARTTIME
                //            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 9].Text = reader2.GetValue(2).ToString().Trim() + " " +
                //                reader2.GetValue(3).ToString().Trim();
                //            //FORMNAME
                //            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 10].Text = reader2.GetValue(5).ToString().Trim();
                //        }

                //        reader2.Dispose();
                //        #endregion
                //    }
                //}

                //reader.Dispose();
                #endregion

                #region 두번째 읽음
                //SQL = "  SELECT B.WRITEDATE, B.WRITETIME, B.PTNO, P.SNAME, TO_CHAR(D.OUTDATE,'YYYY-MM-DD') OUTDATE, D.DEPTCODE,";
                //SQL += ComNum.VBLF + " C.NAME, I.KORNAME, B.USEID, B.CHARTDATE, B.CHARTTIME, F.FORMNAME";
                //SQL += ComNum.VBLF + "FROM KOSMOS_EMR.EMRXML_COMPLETE A";
                //SQL += ComNum.VBLF + "   INNER JOIN KOSMOS_EMR.EMRXML B";
                //SQL += ComNum.VBLF + "      ON B.EMRNO = A.EMRNO";
                //SQL += ComNum.VBLF + "   INNER JOIN KOSMOS_PMPA.BAS_PATIENT P";
                //SQL += ComNum.VBLF + "      ON P.PANO = A.PTNO";
                //SQL += ComNum.VBLF + "   INNER JOIN KOSMOS_ADM.INSA_MST I";
                //SQL += ComNum.VBLF + "      ON I.SABUN3 = B.USEID";
                //SQL += ComNum.VBLF + "   INNER JOIN KOSMOS_PMPA.BAS_BUSE C";
                //SQL += ComNum.VBLF + "      ON C.BUCODE = I.BUSE";
                //SQL += ComNum.VBLF + "   INNER JOIN KOSMOS_EMR.AEMRFORM F";
                //SQL += ComNum.VBLF + "      ON F.FORMNO = B.FORMNO";
                //SQL += ComNum.VBLF + "      AND F.UPDATENO = 1";
                //SQL += ComNum.VBLF + "   LEFT OUTER JOIN KOSMOS_PMPA.IPD_NEW_MASTER D";
                //SQL += ComNum.VBLF + "      ON D.PANO = A.PTNO";
                //SQL += ComNum.VBLF + "      AND D.INDATE >= TO_DATE(A.MEDFRDATE || ' 00:00','YYYY-MM-DD HH24:MI') ";
                //SQL += ComNum.VBLF + "      AND D.INDATE <= TO_DATE(A.MEDFRDATE || ' 23:59','YYYY-MM-DD HH24:MI') ";
                //SQL += ComNum.VBLF + "      AND GBSTS NOT IN ('9') ";

                //SQL += ComNum.VBLF + "WHERE A.CDATE <= TO_DATE(SUBSTR(B.WRITEDATE, 1, 8) || ' ' || SUBSTR(TRIM(B.WRITETIME), 1, 4),'YYYY-MM-DD HH24:MI')";
                //SQL += ComNum.VBLF + "  AND A.CDATE >= TO_DATE('2011-09-01','YYYY-MM-DD')";
                //SQL += ComNum.VBLF + "  AND B.CHARTDATE >= '" + dtpSDATE.Value.ToString("yyyyMMdd") + "' ";
                //SQL += ComNum.VBLF + "  AND B.CHARTDATE <= '" + dtpEDATE.Value.ToString("yyyyMMdd") + "' ";

                //if (txtSname.Text.Trim().Length > 0)
                //{
                //    SQL += ComNum.VBLF + "  AND A.PTNO = '" + txtPtno.Text.Trim() + "' ";
                //}


                //SqlErr = GetAdoRs(ref reader, SQL);
                //if (SqlErr != "")
                //{
                //    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                //    Cursor.Current = Cursors.Default;
                //    return;
                //}

                //if (reader.HasRows)
                //{

                //    while (reader.Read())
                //    {
                //        SS1_Sheet1.RowCount += 1;

                //        //WRITEDATE, WRITETIME
                //        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 1].Text = reader.GetValue(0).ToString().Trim() + " " +
                //            reader.GetValue(1).ToString().Trim();
                //        //PTNO
                //        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 2].Text = reader.GetValue(2).ToString().Trim();
                //        //SNAME
                //        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 3].Text = reader.GetValue(3).ToString().Trim();
                //        //OUTDATE
                //        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 4].Text = reader.GetValue(4).ToString().Trim();
                //        //DEPTCODE
                //        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 5].Text = reader.GetValue(5).ToString().Trim();

                //        //BUSENAME
                //        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 6].Text = reader.GetValue(6).ToString().Trim();
                //        //KORNAME
                //        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 7].Text = reader.GetValue(7).ToString().Trim();

                //        //USEID
                //        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 8].Text = reader.GetValue(8).ToString().Trim();
                //        //CHARTDATE +  CHARTTIME
                //        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 9].Text = reader.GetValue(9).ToString().Trim() + " " +
                //            reader.GetValue(10).ToString().Trim();
                //        //FORMNAME
                //        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 10].Text = reader.GetValue(11).ToString().Trim();

                //    }
                //}

                //reader.Dispose();
                #endregion

                SS1_Sheet1.SetRowHeight(-1, 25);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        string GetAdoRs(ref OracleDataReader reader, string SQL)
        {

            try
            {
                using (OracleCommand cmd = clsDB.DbCon.Con.CreateCommand())
                {
                    cmd.InitialLONGFetchSize = -1;
                    cmd.CommandText = SQL;
                    cmd.CommandTimeout = 120;

                    reader = cmd.ExecuteReader();
                }

                return string.Empty;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBoxEx(this, sqlExc.Message);
                return sqlExc.Message;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                return ex.Message;
            }
        }
               
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            string strHeader = string.Empty;
            using (clsSpread cls = new clsSpread())
            {
                using (Font font = new Font("바탕체", 20))
                {
                    strHeader+= cls.setSpdPrint_String("[ 검수완료 퇴원차트 수정 리스트 ]", font, clsSpread.enmSpdHAlign.Center, false, true);
                }

                using (Font font = new Font("바탕체", 12))
                {
                    strHeader += cls.setSpdPrint_String("\n" + VB.Space(12) +  string.Format("조회일자 : {0} ~ {1}", dtpSDATE.Value.ToString("yyyy-MM-dd"), dtpEDATE.Value.ToString("yyyy-MM-dd")), font, clsSpread.enmSpdHAlign.Left, false, false);
                    strHeader += cls.setSpdPrint_String("\n" + "출력일자 : " + Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).ToShortDateString() + VB.Space(12), font, clsSpread.enmSpdHAlign.Right, false, true);
                }
            }
         

            SS1_Sheet1.PrintInfo.AbortMessage = "검수완료 퇴원차트 수정 리스트를 출력중입니다.";
            SS1_Sheet1.PrintInfo.Header = strHeader;

            SS1_Sheet1.PrintInfo.Margin.Top = 40;
            SS1_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            SS1_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            SS1_Sheet1.PrintInfo.ShowBorder = true;
            SS1_Sheet1.PrintInfo.ShowColor = false;
            SS1_Sheet1.PrintInfo.ShowGrid = true;
            SS1_Sheet1.PrintInfo.ShowShadows = true;
            //SS1_Sheet1.PrintInfo.Preview = true;

            SS1_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            SS1.PrintSheet(0);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SS1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (SS1_Sheet1.RowCount == 0)
                return;

            if (e.Column == 2)
            {
                string strPtNo = SS1_Sheet1.Cells[e.Row, 2].Text.Trim();
                string strOutDate = SS1_Sheet1.Cells[e.Row, 4].Text.Trim();

                rEventCompleUserSend(strPtNo, strOutDate);
            }
            //clsemr
        }

        private void SS1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (SS1_Sheet1.RowCount == 0)
                return;

            if (e.ColumnHeader)
            {
                clsSpread.gSpdSortRow(SS1, e.Column);
            }
        }

        private void FrmCompleteAfterModfiy_FormClosed(object sender, FormClosedEventArgs e)
        {
            rEventClosed?.Invoke();
        }
    }
}
