using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmHira01
    /// Description     : 심평원자료-병용금기
    /// Author          : 이현종
    /// Create Date     : 2018-05-23
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= " \basic\busuga\busuga.vbp(FrmHira01) >> frmHira01.cs 폼이름 재정의" />
    public partial class frmHira01 : Form
    {
        public frmHira01()
        {
            InitializeComponent();
        }

        private void frmHira01_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");
            rdoGB1.Checked = true;

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            GetSpred2();
            GetSearchData();
        }

        void GetSpred2()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT SCODEA, SCODEB FROM ADMIN.HIRA_DURSCODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
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

                ss2_Sheet1.RowCount = dt.Rows.Count;
                ss2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ss2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SCODEA"].ToString().Trim();
                    ss2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SCODEB"].ToString().Trim();
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            GetSearchData();
        }

        void GetSearchData()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt2 = null;

            string strAnn = "";

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT B.DUR_CD_A,  B.ANNCE_DT, B.DUR_SD_EFT, C.SUNEXT CSUNEXT ,C.SUNAMEK  SSUNAMEK, B.DUR_CD_B,  E.SUNEXT ESUNEXT  , E.SUNAMEK  ESUNAMEK ";
                SQL += ComNum.VBLF + " FROM  ADMIN.EDI_SUGA A , ADMIN.HIRA_TBJBD43 B,";
                SQL += ComNum.VBLF + "   (   SELECT AA.SUNEXT , AA.SUNAMEK , AA.BCODE FROM   ADMIN.BAS_SUN AA, ADMIN.BAS_SUT BB  ";
                SQL += ComNum.VBLF + "   WHERE AA.SUNEXT = BB.SUNEXT ";

                if(rdoGB1.Checked == true)
                {
                    SQL += ComNum.VBLF + " AND BB.DELDATE IS NULL";
                }

                if(rdoGB2.Checked == true)
                {
                    SQL += ComNum.VBLF + " AND BB.DELDATE IS NOT NULL";
                }

                SQL += ComNum.VBLF + " ) C,   ";
                SQL += ComNum.VBLF + "      ADMIN.EDI_SUGA D ,";
                SQL += ComNum.VBLF + "   (";
                SQL += ComNum.VBLF + "    SELECT AA.SUNEXT , AA.SUNAMEK, AA.BCODE FROM   ADMIN.BAS_SUN AA, ADMIN.BAS_SUT BB  ";
                SQL += ComNum.VBLF + "   WHERE AA.SUNEXT = BB.SUNEXT ";


                if (rdoGB1.Checked == true)
                {
                    SQL += ComNum.VBLF + " AND BB.DELDATE IS NULL";
                }

                if (rdoGB2.Checked == true)
                {
                    SQL += ComNum.VBLF + " AND BB.DELDATE IS NOT NULL";
                }
    
                SQL += ComNum.VBLF + " ) E ";
                SQL += ComNum.VBLF + "   WHERE RTRIM(A.SCODE) = B.DUR_CD_A ";
                SQL += ComNum.VBLF + " AND A.CODE = C.BCODE ";
                SQL += ComNum.VBLF + " AND  RTRIM(D.SCODE) = B.DUR_CD_B";
                SQL += ComNum.VBLF + " AND D.CODE = E.BCODE";
                SQL += ComNum.VBLF + " ORDER BY B.ANNCE_DT DESC ,  C.SUNEXT ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
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

                ss1_Sheet1.RowCount = dt.Rows.Count;
                ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    if(strAnn != dt.Rows[i]["ANNCE_DT"].ToString().Trim())
                    {
                        clsSpread.gSpreadLineBoder(ss1, i, 0, i, ss1_Sheet1.ColumnCount - 1, Color.Blue, 1, false, false, false, true);
                        strAnn = dt.Rows[i]["ANNCE_DT"].ToString().Trim();
                    }


                    ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ANNCE_DT"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DUR_CD_A"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["CSUNEXT"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SSUNAMEK"].ToString().Trim();

                    ss1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DUR_CD_B"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ESUNEXT"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["ESUNAMEK"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DUR_SD_EFT"].ToString().Trim();

                    //성분코드 색깔표시
                    SQL = " SELECT ROWID FROM ADMIN.HIRA_DURSCODE ";
                    SQL += ComNum.VBLF + " WHERE ( SCODEA = '" +dt.Rows[i]["DUR_CD_A"].ToString().Trim() + "'  AND SCODEB = '" +
                        dt.Rows[i]["DUR_CD_B"].ToString().Trim() + "'  ) ";
                    SQL += ComNum.VBLF + "    OR ( SCODEA = '" + dt.Rows[i]["DUR_CD_B"].ToString().Trim() + "'  AND SCODEB = '" +
                        dt.Rows[i]["DUR_CD_A"].ToString().Trim() + "'  ) ";

                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    if(dt2.Rows.Count > 0)
                    {
                        ss1_Sheet1.Rows[i].ForeColor = Color.Red;
                    }

                    dt2.Dispose();
                    dt2 = null;
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            Set_Print();
        }

        void Set_Print()
        {
            string strTitle = "";
            string strTitle2 = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "심평원자료- 병용금기 약제 LIST";
            if (rdoGB0.Checked == true)
            {
                strTitle2 = "전체코드" + VB.Space(20);
            }
            if (rdoGB1.Checked == true)
            {
                strTitle2 = "삭제제외코드" + VB.Space(20);
            }
            if (rdoGB2.Checked == true)
            {
                strTitle2 = "삭제코드" + VB.Space(20);
            }

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String(strTitle2 + "출력 일자 : " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + VB.Space(20) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 30, 180, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, true, true, false, false);

            CS.setSpdPrint(ss1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            if( Set_Copy() == false) return;
            ComFunc.MsgBox("외래 제한사항( 병용금기) 으로 복사  완료 ", "완료");
        }

        bool Set_Copy()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;
            DataTable dt2 = null;

            string strSucodeA = "";
            string strSucodeB = "";
            string strScodeA  = "";
            string strScodeB  = "";
            string strGDate = "";

            int i = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                for(i = 0; i < ss1_Sheet1.NonEmptyRowCount; i++)
                {

                    strGDate = ss1_Sheet1.Cells[i, 0].Text;
                    strGDate = VB.Left(strGDate, 4) + "-" + VB.Mid(strGDate, 5, 2) + "-" + VB.Mid(strGDate, 7, 2);
                    strScodeA  = ss1_Sheet1.Cells[i, 1].Text;
                    strSucodeA = ss1_Sheet1.Cells[i, 2].Text;
                    strScodeB  = ss1_Sheet1.Cells[i, 5].Text;
                    strSucodeB = ss1_Sheet1.Cells[i, 6].Text;

                    if (strSucodeA != "" && strSucodeB != "")
                    {
                        SQL = " SELECT SUCODE, FIELDA ";
                        SQL += ComNum.VBLF + " From ADMIN.BAS_MSELF ";
                        SQL += ComNum.VBLF + " WHERE GUBUNA='0' ";
                        SQL += ComNum.VBLF + "   AND GUBUNB='9' ";
                        SQL += ComNum.VBLF + "   AND ( ";
                        SQL += ComNum.VBLF + "           (SUCODE = '" + strSucodeA + "'  AND FIELDA = '" + strSucodeB + "' ) ";
                        SQL += ComNum.VBLF + "       OR  (SUCODE = '" + strSucodeB + "'  AND FIELDA = '" + strSucodeA + "' ) ";
                        SQL += ComNum.VBLF + "       )";
                        SQL += ComNum.VBLF + " ORDER BY SUCODE ";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return false;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            //자료가 존재함
                        }
                        else
                        {
                            //자동 등록

                            //'제외성분읽기
                            SQL = " SELECT ROWID FROM ADMIN.HIRA_DURSCODE";
                            SQL += ComNum.VBLF + " WHERE ( SCODEA = '" + strScodeA + "' AND  SCODEB  = '" + strScodeB + "' )";
                            SQL += ComNum.VBLF + "    OR ( SCODEA = '" + strScodeB + "' AND  SCODEB  = '" + strScodeA + "' ) ";

                            SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return false;
                            }

                            if (dt2.Rows.Count == 0)
                            {
                                SQL = " INSERT INTO ADMIN.BAS_MSELF (SUCODE, GUBUNA, GUBUNB, FIELDA, FIELDB, ENTDATE ) ";
                                SQL = SQL + "  VALUES ( '" + strSucodeA + "' , '0','9', '" + strSucodeB + "' ,'', TO_DATE('" + strGDate + "' ,'YYYY-MM-DD') )";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }
                            }
                            dt2.Dispose();
                            dt2 = null;
                        }
                        dt.Dispose();
                        dt = null;

                        SQL = " SELECT SUCODE, FIELDA ";
                        SQL += ComNum.VBLF + " From ADMIN.BAS_MSELF ";
                        SQL += ComNum.VBLF + " WHERE GUBUNA='8' ";
                        SQL += ComNum.VBLF + "   AND GUBUNB='1' ";
                        SQL += ComNum.VBLF + "   AND ( ";
                        SQL += ComNum.VBLF + "           (SUCODE = '" + strSucodeA + "'  AND FIELDA = '" + strSucodeB + "' ) ";
                        SQL += ComNum.VBLF + "       OR  (SUCODE = '" + strSucodeB + "'  AND FIELDA = '" + strSucodeA + "' ) ";
                        SQL += ComNum.VBLF + "       )";
                        SQL += ComNum.VBLF + " ORDER BY SUCODE ";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return false;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            //자료가 존재함
                        }
                        else
                        {
                            ////자동 등록

                            //'제외성분읽기
                            SQL = " SELECT ROWID FROM ADMIN.HIRA_DURSCODE";
                            SQL += ComNum.VBLF + " WHERE ( SCODEA = '" + strScodeA + "' AND  SCODEB  = '" + strScodeB + "' )";
                            SQL += ComNum.VBLF + "    OR ( SCODEA = '" + strScodeB + "' AND  SCODEB  = '" + strScodeA + "' ) ";

                            SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return false;
                            }

                            if (dt2.Rows.Count == 0)
                            {
                                SQL = " INSERT INTO ADMIN.BAS_MSELF (SUCODE, GUBUNA, GUBUNB, FIELDA, FIELDB, ENTDATE ) ";
                                SQL = SQL + "  VALUES ( '" + strSucodeA + "' , '8','1', '" + strSucodeB + "' ,'',TRUNC(SYSDATE))";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }
                            }
                            dt2.Dispose();
                            dt2 = null;
                        }

                        dt.Dispose();
                        dt = null;
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
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            DUR_SCODE_SET_INSERT();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DUR_SCODE_SET_DELETE();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ss1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row < 0 || e.Column < 0) return;

            DUR_SCODE_SET_INSERT();
        }

        void DUR_SCODE_SET_INSERT()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            DataTable dt = null;

            string strCodeA = ss1_Sheet1.Cells[ss1_Sheet1.ActiveRowIndex, 1].Text;
            string strCodeB = ss1_Sheet1.Cells[ss1_Sheet1.ActiveRowIndex, 5].Text;

            if(strCodeA == "" || strCodeB == "")
            {
                ComFunc.MsgBox("선택을 다시해주세요");
                return;
            }

            string strCodeA2 = "";
            string strCodeB2 = "";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = " SELECT ROWID FROM ADMIN.HIRA_DURSCODE";
                SQL += ComNum.VBLF + " WHERE ( SCODEA = '" + strCodeA + "' AND  SCODEB  = '" + strCodeB + "' )";
                SQL += ComNum.VBLF + "    OR ( SCODEA = '" + strCodeB + "' AND  SCODEB  = '" + strCodeA + "' ) ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if(dt.Rows.Count > 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("성분이 등록 되었습니다.");
                    return;
                }

                dt.Dispose();
                dt = null;

                SQL = " INSERT INTO ADMIN.HIRA_DURSCODE ( SCODEA, SCODEB, ENTSABUN, ENTDATE) ";
                SQL = SQL + "  VALUES ( ";
                SQL = SQL + " '" + strCodeA + "', '" + strCodeB + "' , '" + clsType.User.Sabun + "', SYSDATE ) ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                ss2_Sheet1.RowCount = ss2_Sheet1.RowCount + 1;
                ss2_Sheet1.Cells[0, 0].Text = strCodeA;
                ss2_Sheet1.Cells[0, 1].Text = strCodeB;

                for(int i = 0; i < ss1_Sheet1.RowCount; i++)
                {
                    strCodeA2 = ss1_Sheet1.Cells[i, 1].Text;
                    strCodeB2 = ss1_Sheet1.Cells[i, 5].Text;

                    if( (strCodeA2 == strCodeA && strCodeB == strCodeB2) || (strCodeA2 == strCodeB && strCodeB2 == strCodeA) )
                    {
                        ss1_Sheet1.Rows[i].ForeColor = Color.Red;
                    }
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void ss2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row < 0 || e.Column < 0) return;

            DUR_SCODE_SET_DELETE();
        }

        void DUR_SCODE_SET_DELETE()
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strCodeA = ss2_Sheet1.Cells[ss2_Sheet1.ActiveRowIndex, 0].Text;
            string strCodeB = ss2_Sheet1.Cells[ss2_Sheet1.ActiveRowIndex, 1].Text;

            if (strCodeA == "" || strCodeB == "")
            {
                return;
            }

            string strCodeA2 = "";
            string strCodeB2 = "";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = " DELETE ADMIN.HIRA_DURSCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE ( SCODEA  = '" + strCodeA + "' AND  SCODEB = '" + strCodeB + "' )";
                SQL = SQL + ComNum.VBLF + "    OR ( SCODEA  = '" + strCodeB + "' AND  SCODEB = '" + strCodeA + "' ) ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                ss2_Sheet1.RowCount = ss2_Sheet1.RowCount - 1;

                for (int i = 0; i < ss1_Sheet1.RowCount; i++)
                {
                    strCodeA2 = ss1_Sheet1.Cells[i, 1].Text;
                    strCodeB2 = ss1_Sheet1.Cells[i, 5].Text;

                    if ((strCodeA2 == strCodeA && strCodeB == strCodeB2) || (strCodeA2 == strCodeB && strCodeB2 == strCodeA))
                    {
                        ss1_Sheet1.Rows[i].ForeColor = Color.Black;
                    }
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

        }
    }
}
