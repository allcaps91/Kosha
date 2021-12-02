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
using ComLibB;

namespace ComNurLibB
{
    public partial class frmNrCodeArbeit : Form
    {
        private int nRows = 0;
        private int nCols = 0;

        public frmNrCodeArbeit()
        {
            InitializeComponent();
        }

        private void frmNrCodeArbeit_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ssView_Sheet1.Columns[0].Visible = false;
            ssView_Sheet1.Columns[5].Visible = false;
            ssView_Sheet1.Columns[6].Visible = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
            btnSave.Enabled = false;
            btnSave.Focus();
        }

        private void SCREEN_CLEAR()
        {
            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인
            btnSaveClick();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            btnSearchClick();
            btnSave.Enabled = true;
        }

        private void btnSearchClick()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {                                
                SQL = "SELECT A.CODE, A.NAME, A.JIK, A.ROWID, B.NAME BNAME ";
                SQL = SQL + ComNum.VBLF + " FROM NUR_CODE A, NUR_CODE B";
                SQL = SQL + ComNum.VBLF + " WHERE A.GUBUN ='8'";
                SQL = SQL + ComNum.VBLF + "   AND A.JIK = B.CODE";
                SQL = SQL + ComNum.VBLF + "   AND B.GUBUN='1' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY A.CODE";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count + 20;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {                        
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Code"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Name"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Jik"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["BNAME"].ToString().Trim(); //'"학생간호사"
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();                        
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
                ComFunc.MsgBox(ex.Message);
            }
        }

        private bool btnSaveClick()
        {
            bool rtVal = false;
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strSabun = "";
            string strName  = "";
            string strJik   = "";
            string strRowid = "";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    strSabun = ssView_Sheet1.Cells[i, 1].Text;
                    strName = ssView_Sheet1.Cells[i, 2].Text;
                    strJik = ssView_Sheet1.Cells[i, 3].Text;
                    strRowid = ssView_Sheet1.Cells[i, 5].Text;

                    
                    if (ssView_Sheet1.Cells[i, 6].Text == "Y")   //'변경여부
                    {                        
                        if (Convert.ToBoolean(ssView_Sheet1.Cells[i, 0].Value) == true)
                        {
                            SQL = "DELETE NUR_CODE WHERE ROWID = '" + strRowid + "'";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                Cursor.Current = Cursors.Default;
                                return rtVal;
                            }
                        }
                        else
                        {                            
                            if (VB.Len(ssView_Sheet1.Cells[i, 1].Text) != 0)  //'사번 체크
                            {
                                if (VB.Len(ssView_Sheet1.Cells[i, 5].Text) != 0)  //'Rowid 체크
                                {
                                    SQL = "UPDATE NUR_CODE SET";
                                    SQL = SQL + " NAME = '" + strName + "',";
                                    SQL = SQL + " JIK = '" + strJik + "'";
                                    SQL = SQL + " WHERE ROWID = '" + strRowid + "'";

                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                        Cursor.Current = Cursors.Default;
                                        return rtVal;
                                    }
                                }
                                else
                                {
                                    SQL = "INSERT INTO NUR_CODE (GUBUN, CODE, NAME, JIK)";
                                    SQL = SQL + " VALUES('8','" + strSabun + "', '" + strName + "', '" + strJik + "')";

                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                        Cursor.Current = Cursors.Default;
                                        return rtVal;
                                    }
                                }
                            }                                
                        }                      
                    }                       
                }
                
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        private void ssView_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strSabun = "";
            FarPoint.Win.Spread.CellType.TextCellType TypeText = new FarPoint.Win.Spread.CellType.TextCellType();

            if (e.Column == 3)
            {
                ssView_Sheet1.Cells[e.Row, 3].CellType = TypeText;
                ssView_Sheet1.Cells[e.Row, 3].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;


                if (ssView_Sheet1.Cells[e.Row, 3].Text == "26")
                {
                    ssView_Sheet1.Cells[e.Row, 4].Text = "진료과장";
                }
                if (ssView_Sheet1.Cells[e.Row, 3].Text == "57")
                {
                    ssView_Sheet1.Cells[e.Row, 4].Text = "치위생사";
                }
                if (ssView_Sheet1.Cells[e.Row, 3].Text == "61")
                {
                    ssView_Sheet1.Cells[e.Row, 4].Text = "파트타임(RN)";
                }
                if (ssView_Sheet1.Cells[e.Row, 3].Text == "62")
                {
                    ssView_Sheet1.Cells[e.Row, 4].Text = "파트타임(NA)";
                }
                if (ssView_Sheet1.Cells[e.Row, 3].Text == "63")
                {
                    ssView_Sheet1.Cells[e.Row, 4].Text = "학생간호사";
                }
                if (ssView_Sheet1.Cells[e.Row, 3].Text == "64")
                {
                    ssView_Sheet1.Cells[e.Row, 4].Text = "미등록자(RN)";
                }
                if (ssView_Sheet1.Cells[e.Row, 3].Text == "65")
                {
                    ssView_Sheet1.Cells[e.Row, 4].Text = "미등록자(NA)";
                }
                if (ssView_Sheet1.Cells[e.Row, 3].Text == "66")
                {
                    ssView_Sheet1.Cells[e.Row, 4].Text = "영일오더리";
                }
            }

            if (e.Column == 1)
            {
                strSabun = ssView_Sheet1.Cells[e.Row, 1].Text;
                if (VB.Len(strSabun) != 0)
                {
                    try
                    {
                        SQL = "SELECT CODE FROM NUR_CODE WHERE CODE = '" + strSabun + "'";
                        SQL = SQL + ComNum.VBLF + " AND GUBUN ='8'";

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            dt.Dispose();
                            dt = null;
                            ssView_Sheet1.Cells[e.Row, 1].Text = "";
                            ComFunc.MsgBox("해당 사번이 존재합니다.");
                            return;
                        }

                        if (VB.Right(ssView_Sheet1.Cells[e.Row, 1].Text, 1) != "A")
                        {
                            dt.Dispose();
                            dt = null;
                            ssView_Sheet1.Cells[e.Row, 1].Text = "";
                            ComFunc.MsgBox("사번 ERROR:예재 (0001A)");
                            return;
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
            }
            

            if (nRows == e.Row)
            {
                ssView_Sheet1.Cells[e.Row, 6].Text = "Y";
            }
            
        }

        private void ssView_EditModeOn(object sender, EventArgs e)
        {
            int i = 0;
            string[] strValue = new string[] { "26", "57", "61", "62", "63", "64", "65", "66" };
            FarPoint.Win.Spread.CellType.ComboBoxCellType Type1 = new FarPoint.Win.Spread.CellType.ComboBoxCellType();

            try
            {
                Type1.Clear();
                ListBox list1 = new ListBox();
                for (i = 0; i < strValue.Length; i++)
                {
                    list1.Items.Add(strValue[i]);
                }
                Type1.ListControl = list1;
                Type1.Editable = false;
            }
            catch
            {
            }

            nRows = ssView_Sheet1.ActiveRowIndex;
            nCols = ssView_Sheet1.ActiveColumnIndex;
                        
            if (nCols == 3)
            {                
                ssView_Sheet1.Cells[nRows, nCols].CellType = Type1;
            }
        }
    }
}
