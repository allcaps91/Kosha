using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmBasXray.cs
    /// Description     : 방사선단순촬영변환하는 폼
    /// Author          : 안정수
    /// Create Date     : 2017-06
    /// Update History  : try-catch문 수정 및 권한 확인 부분 추가, GstrHelpCode를 받아오는 생성자 추가
    /// <history>       
    /// 전역변수인 GstrHelpCode에 HA464U값 사용
    /// D:\타병원\PSMHH\basic\busuga\FrmBasXray.frm(FrmBasXray) => frmBasXray.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\basic\busuga\FrmBasXray.frm(FrmBasXray)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\basic\busuga\busuga.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    public partial class frmBasXray : Form
    {
        // 테스트용 : HA464U
        //string mstrHelpCode = "HA464U";
        string mstrHelpCode = "";

        public frmBasXray()
        {
            InitializeComponent();
        }

        public frmBasXray(string strHelpCode)
        {
            InitializeComponent();
            mstrHelpCode = strHelpCode;
        }


        void btnPrint_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void frmBasXray_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            ssSSuga_Sheet1.Columns[5].Visible = false;
            ssCSuga_Sheet1.Columns[4].Visible = false;

            SetFormInit();
        }

        void SetFormInit()
        {
            int i = 0;
            string strXrayCode = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            ssSSuga_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    A.SUCODE, A.SUNEXT , B.SUNAMEK, B.XRAYQTY , B.XRAYCODE, B.ROWID";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUH A, "+ ComNum.DB_PMPA + "BAS_SUN B";
                SQL = SQL + ComNum.VBLF + "WHERE 1=1";
                SQL = SQL + ComNum.VBLF + "     AND A.SUCODE = '" + mstrHelpCode + "'";
                SQL = SQL + ComNum.VBLF + "     AND A.SUNEXT = B.SUNEXT  ";
                SQL = SQL + ComNum.VBLF + "     AND A.BAMT <> 0 ";
                SQL = SQL + ComNum.VBLF + "ORDER BY A.SUGBA, A.SUNEXT";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                ssSSuga_Sheet1.RowCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssSSuga_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SuCode"].ToString().Trim();
                    ssSSuga_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                    ssSSuga_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SuNameK"].ToString().Trim();
                    ssSSuga_Sheet1.Cells[i, 3].Text = dt.Rows[i]["XRAYQTY"].ToString().Trim();
                    ssSSuga_Sheet1.Cells[i, 4].Text = dt.Rows[i]["XRAYCODE"].ToString().Trim();
                    if (strXrayCode == "")
                    {
                        strXrayCode = dt.Rows[i]["XRAYCODE"].ToString().Trim();
                    }
                    ssSSuga_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (strXrayCode == "")
                {
                    return;
                }
                READ_BAS_SUN_XRAY(strXrayCode);
            }


            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            
        }

        void READ_BAS_SUN_XRAY(string ArgXrayCode)
        {
            if(ArgXrayCode.Trim() == "")
            {
                return;
            }

            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    A.XRAYCODE, A.XQTY , A.SUNEXT , B.SUNAMEK , A.ROWID";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUN_XRAY A, " + ComNum.DB_PMPA + "BAS_SUN B";
                SQL = SQL + ComNum.VBLF + "WHERE A.XRAYCODE = '" + ArgXrayCode + "' ";
                SQL = SQL + ComNum.VBLF + " AND A.SUNEXT = B.SUNEXT";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssCSuga_Sheet1.Cells[i, 0].Text = dt.Rows[i]["XRAYCODE"].ToString().Trim();
                    ssCSuga_Sheet1.Cells[i, 1].Text = dt.Rows[i]["XQTY"].ToString().Trim();
                    ssCSuga_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                    ssCSuga_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SuNameK"].ToString().Trim();
                    ssCSuga_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ROWID"].ToString().Trim();
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

        void btnReg_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            RegData();
        }

        void RegData()
        {
            int i = 0;
            string strXrayCode = "";
            string strXQty = "";
            string strROWID = "";
            string strOK = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            strOK = "OK";

            for(i = 0; i < ssSSuga_Sheet1.RowCount; i++)
            {
                strXQty = ssSSuga_Sheet1.Cells[i, 3].Text;
                strXrayCode = ssSSuga_Sheet1.Cells[i, 4].Text;
                strROWID = ssSSuga_Sheet1.Cells[i, 5].Text;

                try
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "UPDATE BAS_SUN SET";
                    SQL = SQL + ComNum.VBLF + "     XRAYCODE = '" + strXrayCode + "' ,";
                    SQL = SQL + ComNum.VBLF + "     XRAYQTY = '" + strXQty + "' ";
                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "'";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                    }

                    if (strOK == "OK")
                    {
                        clsDB.setCommitTran(clsDB.DbCon);
                        ComFunc.MsgBox("저장하였습니다.");
                        Cursor.Current = Cursors.Default;
                    }
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

        void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            SaveData();
        }

        void SaveData()
        {
            int i = 0;
            string strXrayCode = "";
            string strXQty = "";
            string strSuNext = "";
            string strROWID = "";
            string strOK = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            strOK = "OK";

            try
            {
                for (i = 0; i < ssCSuga_Sheet1.RowCount; i++)
                {
                    strXrayCode = ssCSuga_Sheet1.Cells[i, 0].Text;
                    strXQty = ssCSuga_Sheet1.Cells[i, 1].Text;
                    strSuNext = ssCSuga_Sheet1.Cells[i, 2].Text;
                    strROWID = ssCSuga_Sheet1.Cells[i, 4].Text;

                    if (strROWID == "")
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "INSERT INTO";
                        SQL = SQL + ComNum.VBLF + "    BAS_SUN_XRAY ( SUDATE, XRAYCODE, XQTY, SUNEXT )";
                        SQL = SQL + ComNum.VBLF + "VALUES(TRUNC(SYSDATE) , '" + strXrayCode + "', '" + strXQty + "' ,'" + strSuNext + "' )";
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
                        SQL = SQL + ComNum.VBLF + "UPDATE BAS_SUN_XRAY SET";
                        SQL = SQL + ComNum.VBLF + "     XRAYCODE = '" + strXrayCode + "' ,";
                        SQL = SQL + ComNum.VBLF + "     XQTY = '" + strXQty + "' ";
                        SQL = SQL + ComNum.VBLF + "     SUNEXT = '" + strSuNext + "' ";
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
                if (strOK == "OK")
                {
                    clsDB.setCommitTran(clsDB.DbCon);
                }
                else
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                }
            }

            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        void ssSSuga_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strXrayCode = "";
            int i = 0;

            strXrayCode = ssSSuga_Sheet1.Cells[e.Row, 4].Text;

            if(strXrayCode == "")
            {
                return;
            }

            READ_BAS_SUN_XRAY(strXrayCode);
        }
  
        void ssCSuga_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strROWID = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            strROWID = ssCSuga_Sheet1.Cells[e.Row, 4].Text;
           
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "DELETE BAS_SUN_XRAY ";
                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "'";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
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

        void ssCSuga_EditChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            string strSuNext = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            strSuNext = ssCSuga_Sheet1.Cells[e.Row, 2].Text;

            if(strSuNext == "")
            {
                return;
            }

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT SUNAMEK ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUN";
                SQL = SQL + ComNum.VBLF + "WHERE SUNEXT = '" + strSuNext + "'";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    ComFunc.MsgBox("해당하는 수가가 없습니다.");
                    ssCSuga_Sheet1.Cells[e.Row, 2].Text = "";
                }
                else
                {
                    ssCSuga_Sheet1.Cells[e.Row, 3].Text = dt.Rows[0]["SuNameK"].ToString().Trim() + "";
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
    }
}
