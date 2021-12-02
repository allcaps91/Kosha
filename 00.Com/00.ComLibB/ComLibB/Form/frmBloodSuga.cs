using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmBloodSuga.cs
    /// Description     : 혈액은행 수가매핑 조회하는 폼
    /// Author          : 안정수
    /// Create Date     : 2017-06
    /// Update History  : try-catch문 수정
    /// <history>       
    /// D:\타병원\PSMHH\basic\busuga\BuSuga23.frm(FrmBloodSuga) => frmBloodSuga.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\basic\busuga\BuSuga23.frm(FrmBloodSuga)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\basic\busuga\busuga.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    /// </summary>
    public partial class frmBloodSuga : Form
    {
        string FstrComponent = "";

        public frmBloodSuga()
        {
            InitializeComponent();            
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();               
        }

        void frmBloodSuga_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            ssBloodCC_Sheet1.Columns[4].Visible = false;
            Settting();
        }

        void Settting()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = "";    

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    CODE, NAME";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "WHERE GUBUN = 'EXAM_혈액종류' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssBlood_Sheet1.RowCount = dt.Rows.Count;
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssBlood_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Code"].ToString().Trim();
                    ssBlood_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Name"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }

            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            SaveData();
        }

        void SaveData()
        {
            int i = 0;

            string strFlag = "";
            string strSuDate = "";
            string strSucode_400 = "";
            string strSucode_320 = "";
            string strROWID = "";
            string strOK = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                for (i = 0; i < 5; i++)
                {
                    strOK = "OK";
                    ssBloodCC_Sheet1.RowCount += 1;
                    strFlag = ssBloodCC_Sheet1.Cells[i, 0].Text;
                    strSuDate = ssBloodCC_Sheet1.Cells[i, 1].Text;
                    strSucode_400 = ssBloodCC_Sheet1.Cells[i, 2].Text;
                    strSucode_320 = ssBloodCC_Sheet1.Cells[i, 3].Text;
                    strROWID = ssBloodCC_Sheet1.Cells[i, 4].Text;

                    if (strSuDate != "" && strSucode_400 != "" && strSucode_320 != "")
                    {
                        if (strSuDate == "")
                        {
                            ComFunc.MsgBox("적용일자가 공란입니다.");
                            strOK = "NO";
                        }
                        if (strSucode_400 == "")
                        {
                            ComFunc.MsgBox("400 수가코드가 공란입니다.");
                            strOK = "NO";
                        }
                        if (strSucode_320 == "")
                        {
                            ComFunc.MsgBox("320 수가코드가 공란입니다.");
                            strOK = "NO";
                        }

                        if (strOK == "OK")
                        {
                            if (strROWID == "")
                            {
                                SQL = "";
                                SQL = SQL + ComNum.VBLF + "INSERT INTO";
                                SQL = SQL + ComNum.VBLF + " " + ComNum.DB_PMPA + "BAS_SUGA_BLOOD( SUDATE,  COMPONENT,   SUCODE_400 ,  SUCODE_320 )";
                                SQL = SQL + ComNum.VBLF + " VALUES ( TO_DATE('" + strSuDate + "','YYYY-MM-DD') , '" + FstrComponent + "', '" + strSucode_400 + "', '" + strSucode_320 + "' ) ";
                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                }

                            }
                            else
                            {
                                if (strFlag == "True")
                                {
                                    SQL = "";
                                    SQL = SQL + ComNum.VBLF + "DELETE ";
                                    SQL = SQL + ComNum.VBLF + " " + ComNum.DB_PMPA + "BAS_SUGA_BLOOD";
                                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "'";
                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        ComFunc.MsgBox(SqlErr);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        Cursor.Current = Cursors.Default;
                                    }

                                }
                                else
                                {
                                    SQL = "";
                                    SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "BAS_SUGA_BLOOD SET";
                                    SQL = SQL + ComNum.VBLF + "    SUDATE = TO_DATE('" + strSuDate + "','YYYY-MM-DD') , ";
                                    SQL = SQL + ComNum.VBLF + "    SUCODE_400  = '" + strSucode_400 + "',";
                                    SQL = SQL + ComNum.VBLF + "    SUCODE_320  = '" + strSucode_320 + "' ";
                                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "'";
                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        ComFunc.MsgBox(SqlErr);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        Cursor.Current = Cursors.Default;
                                    }


                                }
                            }
                        }

                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
            }

            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        void ssBlood_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            FstrComponent = ssBlood_Sheet1.Cells[e.Row, 0].Text;

            txtBlood.Text = FstrComponent + " " + ssBlood_Sheet1.Cells[e.Row, 1].Text;

            DataTable dt = null;
            int i = 0;
            string SQL = string.Empty;
            string SqlErr = "";

            ssBloodCC_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    SUDATE , SUCODE_400, SUCODE_320, ROWID";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUGA_BLOOD";
                SQL = SQL + ComNum.VBLF + "WHERE COMPONENT = '" + FstrComponent + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssBloodCC_Sheet1.RowCount = dt.Rows.Count;
                ssBloodCC_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssBloodCC_Sheet1.Cells[i, 1].Text = VB.Left(dt.Rows[i]["SUDATE"].ToString().Trim(), 10);
                    ssBloodCC_Sheet1.Cells[i, 2].Text = dt.Rows[i]["sucode_400"].ToString().Trim();
                    ssBloodCC_Sheet1.Cells[i, 3].Text = dt.Rows[i]["sucode_320"].ToString().Trim();
                    ssBloodCC_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
        }

        void ssBloodCC_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            ssBloodCC_Sheet1.RowCount += 1;
        }
    }
}
