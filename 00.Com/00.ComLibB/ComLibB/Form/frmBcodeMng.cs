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
    /// PSMHH\basic\bucode\Frm자료사전
    /// 2017.07.27
    /// Auther : 박웅규
    /// </summary>
    public partial class frmBcodeMng : Form
    {
        public frmBcodeMng()
        {
            InitializeComponent();
        }

        private void frmBcodeMng_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ssGichoCode_Sheet1.Rows.Count = 0; 
            ComboSet();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveData() == true)
            {
                GetData();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ssGichoCode_Sheet1.RowCount = ssGichoCode_Sheet1.RowCount + 1;
            ssGichoCode_Sheet1.SetRowHeight(ssGichoCode_Sheet1.RowCount - 1, ComNum.SPDROWHT);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ssGichoCode_Sheet1.RowCount == 0) return;
            if (ssGichoCode_Sheet1.Cells[ssGichoCode_Sheet1.RowCount -1, 1].Text.Trim() == "" )
            {
                ssGichoCode_Sheet1.RowCount = ssGichoCode_Sheet1.RowCount - 1;
            }
            else
            {
                ssGichoCode_Sheet1.Cells[ssGichoCode_Sheet1.RowCount - 1, 0].Value = true;
            }
        }

        private void ssGichoCode_EditChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column == 0) return;

            ssGichoCode_Sheet1.Cells[e.Row, 16].Text = "Y";
        }

        private void ComboSet()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            cboCode.Items.Clear();
            cboCode.Items.Add("자료사전목록");
            try
            {
                SQL = "";
                SQL = "SELECT Code,Name FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + "WHERE Gubun='자료사전목록' ";
                SQL = SQL + ComNum.VBLF + "  AND (DelDate IS NULL OR DelDate>TRUNC(SYSDATE)) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY Code ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    cboCode.SelectedIndex = 0;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    cboCode.SelectedIndex = 0;
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboCode.Items.Add(dt.Rows[i]["Code"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;
                cboCode.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void GetData()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strGubun = cboCode.Text.Trim();

            ssGichoCode_Sheet1.RowCount = 0;
            if (strGubun == "")
            {
                ComFunc.MsgBox("구분이 공란입니다.");
                return;
            }
            
            try
            {
                SQL = "";
                SQL = "SELECT Code,Name,TO_CHAR(JDate,'YYYY-MM-DD') JDate,  " ;
                SQL = SQL + ComNum.VBLF + " TO_CHAR(DelDate,'YYYY-MM-DD') DelDate,   " ;
                SQL = SQL + ComNum.VBLF + " GUBUN2, GUBUN3, GUBUN4, GUBUN5, GUNUM1, GUNUM2, GUNUM3, PART, CNT, SORT, ";
                SQL = SQL + ComNum.VBLF + " ROWID   ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_BCODE                    ";
                SQL = SQL + ComNum.VBLF + "WHERE Gubun='" + strGubun + "'                 " ;
                SQL = SQL + ComNum.VBLF + "ORDER BY Code                                  " ;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssGichoCode_Sheet1.RowCount = dt.Rows.Count;
                ssGichoCode_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssGichoCode_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Code"].ToString().Trim();
                    ssGichoCode_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Name"].ToString().Trim();
                    ssGichoCode_Sheet1.Cells[i, 3].Text = dt.Rows[i]["GUBUN2"].ToString().Trim();
                    ssGichoCode_Sheet1.Cells[i, 4].Text = dt.Rows[i]["GUBUN3"].ToString().Trim();
                    ssGichoCode_Sheet1.Cells[i, 5].Text = dt.Rows[i]["GUBUN4"].ToString().Trim();
                    ssGichoCode_Sheet1.Cells[i, 6].Text = dt.Rows[i]["GUBUN5"].ToString().Trim();
                    ssGichoCode_Sheet1.Cells[i, 7].Text = dt.Rows[i]["GUNUM1"].ToString().Trim();
                    ssGichoCode_Sheet1.Cells[i, 8].Text = dt.Rows[i]["GUNUM2"].ToString().Trim();
                    ssGichoCode_Sheet1.Cells[i, 9].Text = dt.Rows[i]["GUNUM3"].ToString().Trim();
                    ssGichoCode_Sheet1.Cells[i, 10].Text = dt.Rows[i]["PART"].ToString().Trim();
                    ssGichoCode_Sheet1.Cells[i, 11].Text = dt.Rows[i]["CNT"].ToString().Trim();
                    ssGichoCode_Sheet1.Cells[i, 12].Text = dt.Rows[i]["SORT"].ToString().Trim();
                    ssGichoCode_Sheet1.Cells[i, 13].Text = dt.Rows[i]["JDate"].ToString().Trim();
                    ssGichoCode_Sheet1.Cells[i, 14].Text = dt.Rows[i]["DelDate"].ToString().Trim();
                    ssGichoCode_Sheet1.Cells[i, 15].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }
        
        private bool SaveData()
        {
            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            string strGubun = cboCode.Text.Trim();

            if (strGubun == "")
            {
                ComFunc.MsgBox("구분이 공란입니다.");
                return false;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                for (i = 0; i < ssGichoCode_Sheet1.RowCount; i++)
                {
                    bool blnCheck = Convert.ToBoolean(ssGichoCode_Sheet1.Cells[i, 0].Value);
                    string strCODE = ssGichoCode_Sheet1.Cells[i, 1].Text.Trim();
                    string strNAME = ssGichoCode_Sheet1.Cells[i, 2].Text.Trim();
                    string strGUBUN2 = ssGichoCode_Sheet1.Cells[i, 3].Text.Trim();
                    string strGUBUN3 = ssGichoCode_Sheet1.Cells[i, 4].Text.Trim();
                    string strGUBUN4 = ssGichoCode_Sheet1.Cells[i, 5].Text.Trim();
                    string strGUBUN5 = ssGichoCode_Sheet1.Cells[i, 6].Text.Trim();
                    string strGUNUM1 = ssGichoCode_Sheet1.Cells[i, 7].Text.Trim();
                    string strGUNUM2 = ssGichoCode_Sheet1.Cells[i, 8].Text.Trim();
                    string strGUNUM3 = ssGichoCode_Sheet1.Cells[i, 9].Text.Trim();
                    string strPART = ssGichoCode_Sheet1.Cells[i, 10].Text.Trim();
                    string strCNT = ssGichoCode_Sheet1.Cells[i, 11].Text.Trim();
                    string strSORT = ssGichoCode_Sheet1.Cells[i, 12].Text.Trim();
                    string strJDATE = ssGichoCode_Sheet1.Cells[i, 13].Text.Trim();
                    string strDELDATE = ssGichoCode_Sheet1.Cells[i, 14].Text.Trim();
                    string strROWID = ssGichoCode_Sheet1.Cells[i, 15].Text.Trim();
                    string strEDIT = ssGichoCode_Sheet1.Cells[i, 16].Text.Trim();

                    if (blnCheck == true)
                    {
                        if (strROWID != "")
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "DELETE FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
                            SQL = SQL + ComNum.VBLF + "WHERE  ROWID = '" + strROWID + "'";
                        }
                    }
                    else
                    {
                        if (strEDIT == "")
                        {
                            continue; 
                        }
                        if (strROWID == "")
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "INSERT INTO  " + ComNum.DB_PMPA + "BAS_BCODE";
                            SQL = SQL + ComNum.VBLF + "(GUBUN, CODE, NAME, JDATE, DELDATE, ENTSABUN,  ";
                            SQL = SQL + ComNum.VBLF + "     ENTDATE, SORT, PART, CNT, GUBUN2, GUBUN3,  ";
                            SQL = SQL + ComNum.VBLF + "     GUBUN4, GUBUN5, GUNUM1, GUNUM2, GUNUM3 )";
                            SQL = SQL + ComNum.VBLF + "VALUES (" ; 
                            SQL = SQL + ComNum.VBLF + "     '" + strGubun + "',";     //GUBUN
                            SQL = SQL + ComNum.VBLF + "     '" + strCODE + "',";     //CODE
                            SQL = SQL + ComNum.VBLF + "     '" + strNAME + "',";     //NAME
                            if (strJDATE == "")
                            {
                                SQL = SQL + ComNum.VBLF + "     NULL,";     //JDATE
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "     TO_DATE(" + ComFunc.FormatStrToDate(strDELDATE.Replace("-", ""), "D") + ",'YYYY-MM-DD)";    //JDATE
                            }
                            if (strDELDATE == "")
                            {
                                SQL = SQL + ComNum.VBLF + "     NULL,";   //DELDATE
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "     TO_DATE(" + ComFunc.FormatStrToDate(strDELDATE.Replace("-", ""), "D") + ",'YYYY-MM-DD)";  //DELDATE
                            }
                            SQL = SQL + ComNum.VBLF + "     '" + clsType.User.Sabun + "',";     //ENTSABUN
                            SQL = SQL + ComNum.VBLF + "     SYSDATE,";     //ENTDATE
                            SQL = SQL + ComNum.VBLF + "     '" + "" + "',";     //SORT
                            SQL = SQL + ComNum.VBLF + "     '" + "" + "',";     //PART
                            SQL = SQL + ComNum.VBLF + "     '" + "" + "',";     //CNT
                            SQL = SQL + ComNum.VBLF + "     '" + strGUBUN2 + "',";     //GUBUN2
                            SQL = SQL + ComNum.VBLF + "     '" + strGUBUN3 + "',";     //GUBUN3
                            SQL = SQL + ComNum.VBLF + "     '" + strGUBUN4 + "',";     //GUBUN4
                            SQL = SQL + ComNum.VBLF + "     '" + strGUBUN5 + "',";     //GUBUN5
                            if (strGUNUM1 == "")
                            {
                                SQL = SQL + ComNum.VBLF + "     NULL,";     //GUNUM1
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "     " + VB.Val(strGUNUM1) + ",";
                            }
                            if (strGUNUM2 == "")
                            {
                                SQL = SQL + ComNum.VBLF + "     NULL,";     //GUNUM2
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "     " + VB.Val(strGUNUM2) + ",";
                            }
                            if (strGUNUM3 == "")
                            {
                                SQL = SQL + ComNum.VBLF + "     NULL";     //GUNUM3
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "     " + VB.Val(strGUNUM3) + "";
                            }
                            SQL = SQL + ComNum.VBLF + "     )";     
                        }
                        else
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "BAS_BCODE SET";
                            SQL = SQL + ComNum.VBLF + "     Code = '" + strCODE + "',";
                            SQL = SQL + ComNum.VBLF + "     Name = '" + strNAME + "',";
                            SQL = SQL + ComNum.VBLF + "     GUBUN2 = '" + strGUBUN2 + "',";
                            SQL = SQL + ComNum.VBLF + "     GUBUN3 = '" + strGUBUN3 + "',";
                            SQL = SQL + ComNum.VBLF + "     GUBUN4 = '" + strGUBUN4 + "',";
                            SQL = SQL + ComNum.VBLF + "     GUBUN5 = '" + strGUBUN5 + "',";
                            if (strGUNUM1 == "")
                            {
                                SQL = SQL + ComNum.VBLF + "     GUNUM1 = NULL,";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "     GUNUM1 = " + VB.Val(strGUNUM1) + ",";
                            }
                            if (strGUNUM2 == "")
                            {
                                SQL = SQL + ComNum.VBLF + "     GUNUM2 = NULL,";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "     GUNUM2 = " + VB.Val(strGUNUM2) + ",";
                            }
                            if (strGUNUM3 == "")
                            {
                                SQL = SQL + ComNum.VBLF + "     GUNUM3 = NULL,";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "     GUNUM3 = " + VB.Val(strGUNUM3) + ",";
                            }
                            SQL = SQL + ComNum.VBLF + "     PART = '" + strPART + "',";
                            if (strCNT == "")
                            {
                                SQL = SQL + ComNum.VBLF + "     CNT = NULL,";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "     CNT = " + VB.Val(strCNT) + ",";
                            }
                            if (strSORT == "")
                            {
                                SQL = SQL + ComNum.VBLF + "     SORT = NULL,";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "     SORT = " + VB.Val(strSORT) + ",";
                            }
                            if (strJDATE == "")
                            {
                                SQL = SQL + ComNum.VBLF + "     JDate = NULL,";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "     JDate = TO_DATE(" + ComFunc.FormatStrToDate(strJDATE.Replace("-",""),"D") + ",'YYYY-MM-DD) ,";
                            }
                            if (strDELDATE == "")
                            {
                                SQL = SQL + ComNum.VBLF + "     DelDate = NULL";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "     DelDate = TO_DATE(" + ComFunc.FormatStrToDate(strDELDATE.Replace("-", ""), "D") + ",'YYYY-MM-DD)";
                            }
                            SQL = SQL + ComNum.VBLF + "WHERE  ROWID = '" + strROWID + "'";
                        }
                    }
                    
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

    }
}
