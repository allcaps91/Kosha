using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrBaseUserChoRegOld : Form
    {
        
        /// Class Name      : ComEmrBase
        /// File Name       : frmEmrBaseSympOld
        /// Description     : 상용기록지 등록(프로그래스 화면)
        /// Author          : 박웅규
        /// Create Date     : 2018-05-10
        /// Update History  : 
        /// </summary>
        /// <history>  
        /// TODO : 폼 호출, 엑셀저장
        /// </history>
        /// <seealso cref= "PSMHVB\\Ocs\OpdOcs\Oorder\mtsoorder.vbp(PSMHVB\mtsEmr\frmEmrUserChoReg.frm) >> frmEmrBaseUserChoRegOld.cs 폼이름 재정의" />
        public frmEmrBaseUserChoRegOld()
        {
            InitializeComponent();
        }

        private void frmEmrBaseUserChoRegOld_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ssGRPFORM_Sheet1.RowCount = 0;
            ssFORM_Sheet1.RowCount = 0;
            ssUSERFORM_Sheet1.RowCount = 0;

            pGetGrpForm();
            pGetUserForm();

        }

        private void pGetGrpForm()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssGRPFORM_Sheet1.RowCount = 0;

            SQL = "";
            SQL = "SELECT A.GRPFORMNO, A.GRPFORMNAME";
            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "EMRGRPFORM A ";
            SQL = SQL + ComNum.VBLF + "  WHERE (A.USECHECK IS NULL ";
            SQL = SQL + ComNum.VBLF + "      OR A.USECHECK = '0')";
            SQL = SQL + ComNum.VBLF + "      AND A.GRPFORMNO <> 999";
            SQL = SQL + ComNum.VBLF + "  ORDER BY A.DISPSEQ, A.GRPFORMNO";
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

            ssGRPFORM_Sheet1.RowCount = dt.Rows.Count;
            ssGRPFORM_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssGRPFORM_Sheet1.Cells[i, 0].Text = dt.Rows[i]["GRPFORMNAME"].ToString().Trim();
                ssGRPFORM_Sheet1.Cells[i, 1].Text = (dt.Rows[i]["GRPFORMNO"].ToString() + "").Trim();
            }

            dt.Dispose();
            dt = null;

        }

        private void pGetUserForm()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssUSERFORM_Sheet1.RowCount = 0;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "  SELECT B.GRPFORMNO, B.GRPFORMNAME, A.FORMNO, A.FORMNAME, C.DISPSEQ ";
            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "EMRFORM A ";
            SQL = SQL + ComNum.VBLF + "  INNER JOIN " + ComNum.DB_EMR + "EMRGRPFORM B";
            SQL = SQL + ComNum.VBLF + "        ON A.GRPFORMNO = B.GRPFORMNO";
            SQL = SQL + ComNum.VBLF + "  INNER JOIN " + ComNum.DB_EMR + "EMRUSERFORMCHO C";
            SQL = SQL + ComNum.VBLF + "        ON A.FORMNO = C.FORMNO";
            SQL = SQL + ComNum.VBLF + "  WHERE (B.USECHECK IS NULL ";
            SQL = SQL + ComNum.VBLF + "        OR B.USECHECK = '0')";
            SQL = SQL + ComNum.VBLF + "    AND C.USEID = '" + clsType.User.IdNumber + "'";
            SQL = SQL + ComNum.VBLF + "    ORDER BY C.DISPSEQ, A.FORMNO";
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

            ssUSERFORM_Sheet1.RowCount = dt.Rows.Count;
            ssUSERFORM_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssUSERFORM_Sheet1.Cells[i, 0].Text = (dt.Rows[i]["GRPFORMNAME"].ToString() + "").Trim();
                ssUSERFORM_Sheet1.Cells[i, 1].Text = (dt.Rows[i]["FORMNAME"].ToString() + "").Trim();
                ssUSERFORM_Sheet1.Cells[i, 2].Text = (dt.Rows[i]["DISPSEQ"].ToString() + "").Trim();
                ssUSERFORM_Sheet1.Cells[i, 3].Text = (dt.Rows[i]["FORMNO"].ToString() + "").Trim();
                ssUSERFORM_Sheet1.Cells[i, 4].Text = (dt.Rows[i]["GRPFORMNO"].ToString() + "").Trim();
            }

            dt.Dispose();
            dt = null;
        }

        private void ssGRPFORM_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                return;
            }
            if (ssGRPFORM_Sheet1.RowCount == 0)
            {
                return;
            }

            ssGRPFORM_Sheet1.Cells[0, 0, ssGRPFORM_Sheet1.RowCount - 1, ssGRPFORM_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssGRPFORM_Sheet1.Cells[e.Row, 0, e.Row, ssGRPFORM_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            string strGRPFORMNO = ssGRPFORM_Sheet1.Cells[e.Row, 1].Text.Trim();

            pGetForm(strGRPFORMNO);
        }

        private void pGetForm(string strGRPFORMNO)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssFORM_Sheet1.RowCount = 0;

            SQL = "";
            SQL = "SELECT A.GRPFORMNO, A.GRPFORMNAME, B.FORMNO, B.FORMNAME ";
            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "EMRGRPFORM A INNER JOIN " + ComNum.DB_EMR + "EMRFORM B";
            SQL = SQL + ComNum.VBLF + "      ON A.GRPFORMNO = B.GRPFORMNO";
            SQL = SQL + ComNum.VBLF + "  WHERE (B.USECHECK IS NULL ";
            SQL = SQL + ComNum.VBLF + "      OR B.USECHECK = '0')";
            SQL = SQL + ComNum.VBLF + "  AND B.GRPFORMNO = " + strGRPFORMNO;
            SQL = SQL + ComNum.VBLF + "  ORDER BY B.DISPSEQ, B.FORMNO";

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

            ssFORM_Sheet1.RowCount = dt.Rows.Count;
            ssFORM_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssFORM_Sheet1.Cells[i, 0].Text = (dt.Rows[i]["FORMNAME"].ToString() + "").Trim();
                ssFORM_Sheet1.Cells[i, 1].Text = (dt.Rows[i]["GRPFORMNAME"].ToString() + "").Trim();
                ssFORM_Sheet1.Cells[i, 2].Text = (dt.Rows[i]["FORMNO"].ToString() + "").Trim();
                ssFORM_Sheet1.Cells[i, 3].Text = (dt.Rows[i]["GRPFORMNO"].ToString() + "").Trim();
            }

            dt.Dispose();
            dt = null;
        }

        private void ssFORM_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                return;
            }
            if (ssFORM_Sheet1.RowCount == 0)
            {
                return;
            }

            string strGRPFORMNAME = "";
            string strFORMNAME = "";
            string strGRPFORMNO = "";
            string strFormNo = "";

            strFORMNAME = ssFORM_Sheet1.Cells[e.Row, 0].Text.Trim();
            strGRPFORMNAME = ssFORM_Sheet1.Cells[e.Row, 1].Text.Trim();
            strFormNo = ssFORM_Sheet1.Cells[e.Row, 2].Text.Trim();
            strGRPFORMNO = ssFORM_Sheet1.Cells[e.Row, 3].Text.Trim();

            pMoveUserChart(strGRPFORMNO, strFormNo, strGRPFORMNAME, strFORMNAME);
            
        }

        private void pMoveUserChart(string strGRPFORMNO, string strFormNo, string strGRPFORMNAME, string strFORMNAME)
        {
            int i = 0;
            string strFORMNO1 = "";
            string strGRPFORMNO1 = "";
            bool blnFind = false;

            for (i = 0; i < ssUSERFORM_Sheet1.RowCount; i++)
            {
                strFORMNO1 = ssUSERFORM_Sheet1.Cells[i, 3].Text.Trim();
                strGRPFORMNO1 = ssUSERFORM_Sheet1.Cells[i, 4].Text.Trim();
                if (strFormNo == strFORMNO1 && strGRPFORMNO == strGRPFORMNO1)
                {
                    blnFind = true;
                    break;
                }
            }

            if (blnFind == true)
            {
                ComFunc.MsgBoxEx(this, "이미저장된 코드입니다.");
                return;
            }
            ssUSERFORM_Sheet1.RowCount = ssUSERFORM_Sheet1.RowCount + 1;
            ssUSERFORM_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            ssUSERFORM_Sheet1.Cells[ssUSERFORM_Sheet1.RowCount - 1, 0].Text = strGRPFORMNAME;
            ssUSERFORM_Sheet1.Cells[ssUSERFORM_Sheet1.RowCount - 1, 1].Text = strFORMNAME;
            ssUSERFORM_Sheet1.Cells[ssUSERFORM_Sheet1.RowCount - 1, 2].Text = "999";
            ssUSERFORM_Sheet1.Cells[ssUSERFORM_Sheet1.RowCount - 1, 3].Text = strFormNo;
            ssUSERFORM_Sheet1.Cells[ssUSERFORM_Sheet1.RowCount - 1, 4].Text = strGRPFORMNO;
            ssUSERFORM_Sheet1.Cells[ssUSERFORM_Sheet1.RowCount - 1, 5].Text = "1";

            ssUSERFORM_Sheet1.Cells[0, 0, ssUSERFORM_Sheet1.RowCount - 1, ssUSERFORM_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssUSERFORM_Sheet1.Cells[ssUSERFORM_Sheet1.RowCount - 1, 0, ssUSERFORM_Sheet1.RowCount - 1, ssUSERFORM_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;
            ssUSERFORM_Sheet1.Cells[ssUSERFORM_Sheet1.RowCount - 1, 0, ssUSERFORM_Sheet1.RowCount - 1, ssUSERFORM_Sheet1.ColumnCount - 1].Font = new Font("맑은 고딕", 10, FontStyle.Bold); 
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) //권한 확인
            {
                return;
            }
            pGetUserForm();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) //권한 확인
            {
                return;
            }
            if (pSaveUserChart() == true)
            {
                pGetUserForm();
            }
        }

        private bool pSaveUserChart()
        {
            bool rtnVal = false;
            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");

                for (i = 0; i < ssUSERFORM_Sheet1.RowCount; i++)
                {
                    if (ssUSERFORM_Sheet1.Cells[i, 5].Text.Trim() == "1")
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_EMR + "EMRUSERFORMCHO ";
                        SQL = SQL + ComNum.VBLF + "      (USEID, FORMNO, DISPSEQ, WRITEDATE, WRITETIME)";
                        SQL = SQL + ComNum.VBLF + " VALUES (";
                        SQL = SQL + ComNum.VBLF + "  '" + clsType.User.IdNumber + "',";
                        SQL = SQL + ComNum.VBLF + "  '" + ssUSERFORM_Sheet1.Cells[i, 3].Text.Trim() + "',";
                        SQL = SQL + ComNum.VBLF + "  '" + ssUSERFORM_Sheet1.Cells[i, 2].Text.Trim() + "',";
                        SQL = SQL + ComNum.VBLF + "  '" + VB.Left(strCurDateTime, 8) + "',";
                        SQL = SQL + ComNum.VBLF + "  '" + VB.Right(strCurDateTime, 6) + "')";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, "저장하였습니다.");
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) //권한 확인
            {
                return;
            }

            if (ssUSERFORM_Sheet1.RowCount == 0)
            {
                return;
            }

            if (ComFunc.MsgBoxQ("선택된 기록지를 삭제하시겠습니까?", "삭제",MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }
            if (pDeleteUserChart() == true)
            {
                pGetUserForm();
            }
        }

        private bool pDeleteUserChart()
        {
            bool rtnVal = false;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strFormNo = "";

            if (ssUSERFORM_Sheet1.Cells[ssUSERFORM_Sheet1.ActiveRowIndex, 5].Text.Trim() == "1")
            {
                ssUSERFORM_Sheet1.Rows[ssUSERFORM_Sheet1.ActiveRowIndex].Remove();
                ssUSERFORM_Sheet1.RowCount = ssUSERFORM_Sheet1.RowCount - 1;
                return true;
            }

            strFormNo = ssUSERFORM_Sheet1.Cells[ssUSERFORM_Sheet1.ActiveRowIndex, 3].Text.Trim();

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE FROM " + ComNum.DB_EMR + "EMRUSERFORMCHO";
                SQL = SQL + ComNum.VBLF + " WHERE USEID = '" + clsType.User.IdNumber  + "'";
                SQL = SQL + ComNum.VBLF + "      AND FORMNO = " + strFormNo;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, "삭제하였습니다.");
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void ssFORM_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssFORM_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssFORM, e.Column);
                return;
            }
            ssFORM_Sheet1.Cells[0, 0, ssFORM_Sheet1.RowCount - 1, ssFORM_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssFORM_Sheet1.Cells[e.Row, 0, e.Row, ssFORM_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;
        }

        private void ssUSERFORM_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssUSERFORM_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssUSERFORM, e.Column);
                return;
            }
            ssUSERFORM_Sheet1.Cells[0, 0, ssUSERFORM_Sheet1.RowCount - 1, ssUSERFORM_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssUSERFORM_Sheet1.Cells[e.Row, 0, e.Row, ssUSERFORM_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
