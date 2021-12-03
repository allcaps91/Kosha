using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase;
 
namespace ComEmrBase
{
    /// <summary>
    /// VB : PSMH\mtsEmr\frmSingularRemark.frm
    /// 박웅규 : 2018-05-17
    /// </summary>
    public partial class frmEmrBaseSingularRemark : Form
    {
        string mPTNO = "";

        public frmEmrBaseSingularRemark()
        {
            InitializeComponent();
        }

        public frmEmrBaseSingularRemark(string pPTNO)
        {
            InitializeComponent();
            mPTNO = pPTNO;
        }

        private void frmEmrBaseSingularRemark_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ssSingularView_Sheet1.RowCount = 0;

            if (mPTNO != "")
            {
                GetPatientInfo(mPTNO);
            }
        }

        private void GetPatientInfo(string pPTNO)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssSingularView_Sheet1.RowCount = 0;
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "     A.PTNO, A.PTNAME, A.SSNO1, A.SSNO2 ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "VIEWBPT A ";
                SQL = SQL + ComNum.VBLF + "WHERE A.PTNO = '" + pPTNO + "'";

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
                txtPtNo.Text = dt.Rows[0]["PTNO"].ToString().Trim();
                txtPtName.Text = dt.Rows[0]["PTNAME"].ToString().Trim();
                txtSexAge.Text = ComFunc.SexCheck(dt.Rows[0]["SSNO1"].ToString().Trim() + dt.Rows[0]["SSNO1"].ToString().Trim(), "1") + "/" + ComFunc.AgeCalc(clsDB.DbCon, dt.Rows[0]["SSNO1"].ToString().Trim() + dt.Rows[0]["SSNO1"].ToString().Trim());
                dt.Dispose();
                dt = null;

                if (optSingularType0.Checked == true)
                {
                    GetSingularRemark(pPTNO, "0");
                }
                else if (optSingularType1.Checked == true)
                {
                    GetSingularRemark(pPTNO, "1");
                }
                else if (optSingularType2.Checked == true)
                {
                    GetSingularRemark(pPTNO, "2");
                }

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void GetSingularRemark(string strPTNO, string strGUBUN)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) //권한 확인
            {
                return;
            }
            ssSingularView_Sheet1.RowCount = 0;
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT A.INPDATE, A.REMARK, A.GUBUN, A.USEID, B.USENAME ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "SGLRMK A INNER JOIN " + ComNum.DB_EMR + "VIEWBUSER B ON A.USEID = B.USEID";
                SQL = SQL + ComNum.VBLF + " WHERE A.PTNO = '" + strPTNO + "'";
                if (strGUBUN != "2" )
                {
                    SQL = SQL + ComNum.VBLF + "      AND A.GUBUN = '" + strGUBUN + "'";
                }

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
                    ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssSingularView_Sheet1.RowCount = dt.Rows.Count;
                ssSingularView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssSingularView_Sheet1.Cells[i, 0].Text = ComFunc.FormatStrToDate(dt.Rows[i]["INPDATE"].ToString().Trim(), "D");
                    ssSingularView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["USENAME"].ToString().Trim();
                    ssSingularView_Sheet1.Cells[i, 2].Text = (dt.Rows[i]["GUBUN"].ToString().Trim() == "0" ? "좋음":"나쁨");
                    ssSingularView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                    ssSingularView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["USEID"].ToString().Trim();
                    ssSingularView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["GUBUN"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            string strCurDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"),"D");
            dtpInpDate.Value = Convert.ToDateTime(strCurDate);
            txtSingularRemark.Text = "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) //권한 확인
            {
                return;
            }
            if (SaveData() == true)
            {
                if (optSingularType0.Checked == true)
                {
                    GetSingularRemark(mPTNO, "0");
                }
                else if (optSingularType1.Checked == true)
                {
                    GetSingularRemark(mPTNO, "1");
                }
                else if (optSingularType2.Checked == true)
                {
                    GetSingularRemark(mPTNO, "2");
                }
            }
        }

        private bool SaveData()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strPtNo = "";
            string strInpDate = "";
            string strGUBUN = "";
            string strRemark = "";

            strPtNo = txtPtNo.Text.Trim();
            strInpDate = dtpInpDate.Value.ToString("yyyyMMdd");  

            if (optGood.Checked == true)
            {
                strGUBUN = "0";
            }
            else
            {
                strGUBUN = "1";
            }
            strRemark = txtSingularRemark.Text.Trim().Replace("'", "`");

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT *";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "SGLRMK ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPtNo + "'";
                SQL = SQL + ComNum.VBLF + "      AND INPDATE = '" + strInpDate + "'";
                SQL = SQL + ComNum.VBLF + "      AND GUBUN = '" + strGUBUN + "'";
                SQL = SQL + ComNum.VBLF + "      AND USEID = '" + clsType.User.IdNumber + "'";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }

                if (dt.Rows.Count > 0 )
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_EMR + "SGLRMK SET";
                    SQL = SQL + ComNum.VBLF + "      USEID = '" + clsType.User.IdNumber + "',";
                    SQL = SQL + ComNum.VBLF + "      INPDATE = '" + strInpDate + "',";
                    SQL = SQL + ComNum.VBLF + "      GUBUN = '" + strGUBUN + "',";
                    SQL = SQL + ComNum.VBLF + "      REMARK = '" + strRemark + "'";
                    SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "      AND INPDATE = '" + strInpDate + "'";
                    SQL = SQL + ComNum.VBLF + "      AND GUBUN = '" + strGUBUN + "'";
                    SQL = SQL + ComNum.VBLF + "      AND USEID = '" + clsType.User.IdNumber   + "'";
                }
                else
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "SGLRMK ";
                    SQL = SQL + ComNum.VBLF + "            (PTNO, USEID, INPDATE, GUBUN, REMARK)";
                    SQL = SQL + ComNum.VBLF + "VALUES('" + strPtNo + "', ";
                    SQL = SQL + ComNum.VBLF + "             '" + clsType.User.IdNumber + "', ";
                    SQL = SQL + ComNum.VBLF + "             '" + strInpDate + "', ";
                    SQL = SQL + ComNum.VBLF + "             '" + strGUBUN + "', ";
                    SQL = SQL + ComNum.VBLF + "             '" + strRemark + "')";
                }
                dt.Dispose();
                dt = null;

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
                ComFunc.MsgBoxEx(this, "저장하였습니다.");
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) //권한 확인
            {
                return;
            }
            if (ssSingularView_Sheet1.Rows.Count ==0 )
            {
                ComFunc.MsgBoxEx(this, "삭제할 데이타를 선택해 주십시요.");
                return;
            }

            if (DeleteData() == true)
            {
                if (optSingularType0.Checked == true)
                {
                    GetSingularRemark(mPTNO, "0");
                }
                else if (optSingularType1.Checked == true)
                {
                    GetSingularRemark(mPTNO, "1");
                }
                else if (optSingularType2.Checked == true)
                {
                    GetSingularRemark(mPTNO, "2");
                }
            }
        }

        private bool DeleteData()
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strPtNo = "";
            string strInpDate = "";
            string strGUBUN = "";
            string strRemark = "";
            string strUseId = "";

            strPtNo = txtPtNo.Text.Trim();
            strInpDate = dtpInpDate.Value.ToString("yyyyMMdd");

            if (optGood.Checked == true)
            {
                strGUBUN = "0";
            }
            else
            {
                strGUBUN = "1";
            }
            strRemark = txtSingularRemark.Text.Trim().Replace("'", "`");
            strUseId = ssSingularView_Sheet1.Cells[ssSingularView_Sheet1.ActiveRowIndex, 4].Text.Trim();

            if (strUseId != clsType.User.IdNumber)
            {
                ComFunc.MsgBoxEx(this, "입력자가 틀립니다. 확인해주세요!");
                return false;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "DELETE FROM " + ComNum.DB_EMR + "SGLRMK";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "      AND INPDATE = '" + strInpDate + "'";
                SQL = SQL + ComNum.VBLF + "      AND GUBUN = '" + strGUBUN + "'";
                SQL = SQL + ComNum.VBLF + "      AND USEID = '" + clsType.User.IdNumber + "'";

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
                ComFunc.MsgBoxEx(this, "저장하였습니다.");
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void ssSingularView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssSingularView_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssSingularView, e.Column);
                return;
            }

            ssSingularView_Sheet1.Cells[0, 0, ssSingularView_Sheet1.RowCount - 1, ssSingularView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssSingularView_Sheet1.Cells[e.Row, 0, e.Row, ssSingularView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            string strInpDate = ssSingularView_Sheet1.Cells[e.Row, 0].Text;
            string strGUBUN = ssSingularView_Sheet1.Cells[e.Row, 5].Text;
            string strRemark = ssSingularView_Sheet1.Cells[e.Row, 3].Text;
            string strUseId = ssSingularView_Sheet1.Cells[e.Row, 4].Text.Trim();

            txtSingularRemark.Text = "";
            dtpInpDate.Value = Convert.ToDateTime(strInpDate);
            
            if (strGUBUN == "0")
            {
                optGood.Checked = true;
            }
            else
            {
                optBed.Checked = true;
            }

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT REMARK ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "SGLRMK";
            SQL = SQL + ComNum.VBLF + "WHERE PTNO = '" + mPTNO + "' ";
            SQL = SQL + ComNum.VBLF + "      AND INPDATE = '" + strInpDate.Replace("-","") + "'";
            SQL = SQL + ComNum.VBLF + "      AND GUBUN = '" + strGUBUN + "'";
            SQL = SQL + ComNum.VBLF + "      AND USEID = '" + strUseId + "'";

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

            txtSingularRemark.Text = dt.Rows[0]["REMARK"].ToString().Trim();
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

        }
    }
}
