using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmNeedInoculation : Form
    {
        /// Class Name      : ComLibB.dll
        /// File Name       : frmNeedInoculation.cs
        /// Description     : 필수 예방접종 수가코드 관리
        /// Author          : 김효성
        /// Create Date     : 2017-06-22
        /// Update History  : 
        /// </summary>
        /// <history>  
        /// VB\basic\busanid\Frm필수예방접종관리.frm => frmNeedInoculation.cs 으로 변경함
        /// </history>
        /// <seealso> 
        /// VB\basic\buppat\Frm필수예방접종관리.frm(Frm필수예방접종관리)
        /// </seealso>
        /// <vbp>
        /// default : VB\basic\buppat\busanid\buppat.vbp
        /// </vbp>

        string GstrROWID = "";
        string GSabun = "";

        public frmNeedInoculation ()
        {
            InitializeComponent ();
        }

        public frmNeedInoculation (string strSabun , string strROWID)
        {
            InitializeComponent ();

            GSabun = strSabun;
            GstrROWID = strROWID;
        }

        private void frmNeedInoculation_Load (object sender , EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            ComFunc.ReadSysDate(clsDB.DbCon);
            Clear ();
            ssJoin_Sheet1.Cells [0 , 8].Text = Convert.ToString (GSabun);
        }

        private void Clear ()
        {
            ssJoin_Sheet1.RowCount = 0;
            ssJoin_Sheet1.RowCount = 1;
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;
            ssJoin_Sheet1.Columns [0].Visible = false;
            ssView_Sheet1.Columns [0].Visible = false;
            ssView_Sheet1.Columns [8].Visible = false;
        }

        private void btnExit_Click (object sender , EventArgs e)
        {
            this.Close ();
        }

        private bool Save ()
        {
            bool rtnVal = false;
            int nMirAmt = 0;
            int nBonAmt = 0;
            int nGamAmt = 0;
            string strGubun = "";
            string strSucode = "";
            string strBDate = "";
            string strDelDate = "";
            string strEntSabun = "";
            string strES = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (ComQuery.IsJobAuth(this , "C", clsDB.DbCon) == false) return rtnVal; //rtnVal;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                strGubun = "1";
                strSucode = ssJoin_Sheet1.Cells [0 , 1].Text;
                strBDate = ssJoin_Sheet1.Cells [0 , 2].Text;
                nMirAmt = Convert.ToInt32 (ssJoin_Sheet1.Cells [0 , 4].Text.Trim ().Replace ("," , ""));
                nBonAmt = Convert.ToInt32 (ssJoin_Sheet1.Cells [0 , 5].Text.Trim ().Replace ("," , ""));
                nGamAmt = Convert.ToInt32 (ssJoin_Sheet1.Cells [0 , 6].Text.Trim ().Replace ("," , ""));
                strDelDate = ssJoin_Sheet1.Cells [0 , 7].Text;
                strEntSabun = ssJoin_Sheet1.Cells [0 , 8].Text;
                strES = ssJoin_Sheet1.Cells [0 , 9].Text;

                if (strEntSabun == "") strEntSabun = Convert.ToString (GSabun);
                if (GstrROWID != "")
                {
                    ComFunc.MsgBox ("기존 자료를 변경하시겠습니까?");
                }
                else
                {
                    ComFunc.MsgBox ("신규 자료를 변경하시겠습니까?");
                }

                if (GstrROWID != "")
                {
                    SQL = " ";
                    SQL = SQL + ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_VACC_MST SET ";
                    SQL = SQL + ComNum.VBLF + " GUBUN ='" + strGubun + "',  ";
                    SQL = SQL + ComNum.VBLF + " MIRAMT =" + nMirAmt + ", ";
                    SQL = SQL + ComNum.VBLF + " BONAMT =" + nBonAmt + ", ";
                    SQL = SQL + ComNum.VBLF + " GAMAMT =" + nGamAmt + ", ";
                    SQL = SQL + ComNum.VBLF + " DELDATE =TO_DATE('" + strDelDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + " ENTSABUN ='" + strEntSabun + "', ";
                    SQL = SQL + ComNum.VBLF + " GBES = '" + strES + "',";
                    SQL = SQL + ComNum.VBLF + " ENTDATE2 =SYSDATE ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + GstrROWID + "' ";
                }
                else
                {
                    SQL = " INSERT INTO " + ComNum.DB_PMPA + "BAS_VACC_MST ( GUBUN,SUCODE,MIRAMT,BONAMT,GAMAMT,";
                    SQL = SQL + ComNum.VBLF + " DELDATE,ENTDATE,ENTDATE2,ENTSABUN,SDATE,GBES ) VALUES ( ";
                    SQL = SQL + ComNum.VBLF + " '" + strGubun + "','" + strSucode + "'," + nMirAmt + "," + nBonAmt + "," + nGamAmt + ", ";
                    SQL = SQL + ComNum.VBLF + "  TO_DATE('" + strDelDate + "','YYYY-MM-DD') ,SYSDATE,SYSDATE,'" + strEntSabun + "',";
                    SQL = SQL + ComNum.VBLF + "  TO_DATE('" + strBDate + "','YYYY-MM-DD'),'" + strES + "' ) ";
                }

                SqlErr = clsDB.ExecuteNonQuery (SQL , ref intRowAffected , clsDB.DbCon);

                clsDB.setCommitTran (clsDB.DbCon);
                ComFunc.MsgBox ("저장하였습니다.");
                Cursor.Current = Cursors.Default;
                GstrROWID = "";

                ssJoin_Sheet1.RowCount = 0;
                ssJoin_Sheet1.RowCount = 1;
                ssJoin_Sheet1.Cells [0 , 8].Text = Convert.ToString (GSabun);
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran (clsDB.DbCon);
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnSave_Click (object sender , EventArgs e)
        {
            Save ();
        }

        private bool Del ()
        {
            bool rtnVal = false;
            string strEntSabun = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (ComQuery.IsJobAuth(this , "D", clsDB.DbCon) == false) return rtnVal; //rtnVal;

            if (ComFunc.MsgBoxQ ("수가를 삭제하시겠습니까?" , "확인" , MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No) { }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                if (GstrROWID != "")
                {
                    SQL = " UPDATE " + ComNum.DB_PMPA + "BAS_VACC_MST SET ";
                    SQL = SQL + ComNum.VBLF + " DELDATE =TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + " ENTSABUN ='" + strEntSabun + "', ";
                    SQL = SQL + ComNum.VBLF + " ENTDATE2 =SYSDATE ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + GstrROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery (SQL , ref intRowAffected , clsDB.DbCon);
                    ssJoin_Sheet1.Cells [0 , 8].Text = Convert.ToString (GSabun);
                }
                else
                {
                    clsDB.setRollbackTran (clsDB.DbCon);
                    ComFunc.MsgBox (SqlErr);
                    clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                }

                clsDB.setCommitTran (clsDB.DbCon);
                ComFunc.MsgBox ("저장하였습니다.");
                Cursor.Current = Cursors.Default;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran (clsDB.DbCon);
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void btnDelete_Click (object sender , EventArgs e)
        {
            if (Del () == true)
            {
                Search ();
            }
        }

        private bool Search ()
        {
            int j = 0;
            bool rtnVal = false;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if (ComQuery.IsJobAuth(this , "R", clsDB.DbCon) == false) return rtnVal;//권한 확인

            GstrROWID = "";

            try
            {
                SQL = "SELECT GUBUN,SUCODE,nvl(MIRAMT,0) MIRAMT,nvl(BONAMT,0) BONAMT ,nvl(GAMAMT,0) GAMAMT,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(SDATE,'YYYY-MM-DD') SDATE,TO_CHAR(EDATE,'YYYY-MM-DD') EDTAE,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(DELDATE,'YYYY-MM-DD') DELDATE,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE,TO_CHAR(ENTDATE2,'YYYY-MM-DD HH24:MI') ENTDATE2,  ";
                SQL = SQL + ComNum.VBLF + "  ENTSABUN, BIGO,GBES,ROWID ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_VACC_MST ";

                if (chkDel.Checked != true)
                {
                    SQL = SQL + ComNum.VBLF + "   WHERE DELDATE IS NULL ";
                }
                SQL = SQL + ComNum.VBLF + " ORDER By SUCODE,ENTDATE ";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                if (dt == null)
                {
                    ComFunc.MsgBox ("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose ();
                    dt = null;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight (-1 , ComNum.SPDROWHT);

                for (j = 0; j < dt.Rows.Count; j++)
                {

                    if (dt.Rows [j] ["DELDATE"].ToString ().Trim () != "") ssView_Sheet1.Rows [j].ForeColor = Color.Red;
                    //ssView.ActiveSheet.Cells [intRow2 , -1].ForeColor = Color.Red;
                    else ssView_Sheet1.Rows [j].ForeColor = Color.Black;

                    ssView_Sheet1.Cells [j , 1].Text = dt.Rows [j] ["SUCODE"].ToString ().Trim ();
                    ssView_Sheet1.Cells [j , 2].Text = dt.Rows [j] ["SDATE"].ToString ().Trim ();
                    ssView_Sheet1.Cells [j , 3].Text = VB.Val (dt.Rows [j] ["MIRAMT"].ToString ().Trim ()).ToString ("###,###,##0");
                    ssView_Sheet1.Cells [j , 4].Text = VB.Val (dt.Rows [j] ["BONAMT"].ToString ().Trim ()).ToString ("###,###,##0");
                    ssView_Sheet1.Cells [j , 5].Text = VB.Val (dt.Rows [j] ["GAMAMT"].ToString ().Trim ()).ToString ("###,###,##0");
                    ssView_Sheet1.Cells [j , 6].Text = dt.Rows [j] ["DELDATE"].ToString ().Trim ();
                    ssView_Sheet1.Cells [j , 7].Text = dt.Rows [j] ["ENTSABUN"].ToString ().Trim ();
                    ssView_Sheet1.Cells [j , 8].Text = dt.Rows [j] ["ROWID"].ToString ().Trim ();
                    ssView_Sheet1.Cells [j , 9].Text = dt.Rows [j] ["GBES"].ToString ().Trim ();
                }

                dt.Dispose ();
                dt = null;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private void btnSearch_Click (object sender , EventArgs e)
        {
            Search ();
        }

        private void ssView_CellDoubleClick (object sender , FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;
            int intCol = 0;

            if (e.RowHeader == false && e.ColumnHeader == false)
            {
                for (i = 0; i < 9; i++)
                {
                    intCol = i;
                    if (i > 2) intCol = i + 1;
                    if (i == 8)
                    {
                        intCol = i + 1;
                        i = i + 1;
                    }

                    ssJoin_Sheet1.Cells [0 , intCol].Text = ssView_Sheet1.Cells [e.Row , i].Text;
                }

                GstrROWID = ssView_Sheet1.Cells [e.Row , 8].Text;
            }
        }

        private void ssJoin_LeaveCell (object sender , FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            int nAMT = 0;
            int nCAMT = 0;
            int nBAMT = 0;
            string strSucode = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if (e.Row == 0 && e.Column == 1)
            {
                strSucode = VB.UCase (ssJoin_Sheet1.Cells [e.Row , 1].Text).Trim ();
                ssJoin_Sheet1.Cells [e.Row , 1].Text = strSucode;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Sucode,Bun,Nu,SugbA,SugbB,SugbC,                                        ";
                SQL = SQL + ComNum.VBLF + "       SugbD,SugbE,SugbF,SugbG,SugbH,SugbI,SugbL,                              ";
                SQL = SQL + ComNum.VBLF + "       SugbJ,SugbK,SugbM,SugbO, n.SugbQ, n.SugbR,n.SugbS,n.SugbW,n.SugbX,      ";
                SQL = SQL + ComNum.VBLF + "       Iamt, Tamt, Bamt,                                                       ";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(Sudate, 'yyyy-mm-dd') Suday,OldIamt,OldTamt,OldBamt,            ";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(Sudate3, 'yyyy-mm-dd') Suday3,Iamt3,Tamt3,Bamt3,                ";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(Sudate4, 'yyyy-mm-dd') Suday4,Iamt4,Tamt4,Bamt4,                ";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(Sudate5, 'yyyy-mm-dd') Suday5,Iamt5,Tamt5,Bamt5,                ";
                SQL = SQL + ComNum.VBLF + "       DayMax,TotMax, t.Sunext,n.GBBONE,n.GbNS,n.DtlBun,                       ";
                SQL = SQL + ComNum.VBLF + "       Sunamek,SuHam,Unit,Hcode,Bcode,TO_CHAR(DelDate,'YYYY-MM-DD') DelDate    ";
                SQL = SQL + ComNum.VBLF + "  FROM BAS_SUT t, BAS_SUN n                                                    ";
                SQL = SQL + ComNum.VBLF + " WHERE t.Sucode = '" + strSucode + "'                                          ";
                SQL = SQL + ComNum.VBLF + "   AND t.Sunext = n.Sunext                                                     ";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                nAMT = 0;

                if (SqlErr != "")
                {
                    ComFunc.MsgBox ("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose ();
                    dt = null;
                    ComFunc.MsgBox ("해당 DATA가 없습니다.");
                    return;
                }

                if (Convert.ToDateTime (dt.Rows [0] ["Suday"].ToString ().Trim ()) <= Convert.ToDateTime (clsPublic.GstrSysDate))
                {
                    nAMT = Convert.ToInt32 (dt.Rows [0] ["Iamt"].ToString ().Trim ());
                    ssJoin_Sheet1.Cells [0 , 3].Text = nAMT.ToString ();
                    ssJoin_Sheet1.Cells[0, 4].Text = "0";
                    ssJoin_Sheet1.Cells[0, 5].Text = "0";
                    ssJoin_Sheet1.Cells[0, 6].Text = "0";
                }
            }

            if (e.Row == 0 && e.Column == 4)
            {
                nAMT = Convert.ToInt32 (VB.Val (ssJoin_Sheet1.Cells [e.Row , 3].Text));
                nCAMT = Convert.ToInt32 (VB.Val (ssJoin_Sheet1.Cells [e.Row , 4].Text));
                nBAMT = Convert.ToInt32 (VB.Val (ssJoin_Sheet1.Cells [e.Row , 5].Text));
                ssJoin_Sheet1.Cells [e.Row , 6].Text = Convert.ToString (nAMT - nCAMT - nBAMT);
            }

        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);
            Clear();
            ssJoin_Sheet1.Cells[0, 8].Text = Convert.ToString(GSabun);
            GstrROWID = "";
        }
    }
}
