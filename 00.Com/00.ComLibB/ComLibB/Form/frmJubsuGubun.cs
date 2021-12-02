using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{

    /// Class Name      : ComLibB.dll
    /// File Name       : frmJubsuGubun.cs
    /// Description     : 접수 수납구분 등록
    /// Author          : 김효성
    /// Create Date     : 2017-06-20
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// VB\basic\bucode\Frm접수수납구분.frm => frmJubsuGubun.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\basic\bucode\Frm접수수납구분.frm(Frm접수수납구분)
    /// </seealso>
    /// <vbp>
    /// default : VB\basic\bucode\bucode.vbp
    /// </vbp>

    /// <summary>
    /// 2017.05.29 TODO : Test 해야함
    /// 프린트버튼은 있지만 프린트 코드가 없으므로 프린트 버튼 삭제
    /// </summary>
    public partial class frmJubsuGubun : Form
    {

        string GSabun = "";
        public frmJubsuGubun ()
        {
            InitializeComponent ();
        }

        public frmJubsuGubun (string strSabun)
        {
            InitializeComponent ();

            GSabun = strSabun;
        }

        private void frmJubsuGubun_Load (object sender , EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회 EX
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            ssView_Sheet1.Columns [12].Visible = false;
            ssView_Sheet1.Columns [13].Visible = false;

            Search ();
        }

        private void Search()
        {
            if (ComQuery.IsJobAuth(this , "R", clsDB.DbCon) == false) return;//권한 확인

            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            
            SQL = "SELECT Code,Name,DeptCode,HDept,GbJinAmt,JinBonAmt,GbOpdWork,GbOCS,";
            SQL = SQL + ComNum.VBLF + " GbDeptJepsu,GbTong,TO_CHAR(DelDate,'YYYY-MM-DD') DelDate,ROWID ";
            SQL = SQL + ComNum.VBLF + " FROM BAS_OPDJIN ";
            SQL = SQL + ComNum.VBLF + "ORDER BY Code ";

            SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

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

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = dt.Rows.Count + 20;
            ssView_Sheet1.SetRowHeight (-1 , ComNum.SPDROWHT);

            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssView_Sheet1.Cells [i , 0].Text = "";
                ssView_Sheet1.Cells [i , 1].Text = dt.Rows [i] ["code"].ToString ().Trim ();
                ssView_Sheet1.Cells [i , 2].Text = dt.Rows [i] ["Name"].ToString ().Trim ();
                ssView_Sheet1.Cells [i , 3].Text = dt.Rows [i] ["DeptCode"].ToString ().Trim ();

                ssView_Sheet1.Cells [i , 4].Text = dt.Rows [i] ["HDept"].ToString ().Trim ();
                ssView_Sheet1.Cells [i , 5].Text = dt.Rows [i] ["GbJinAmt"].ToString ().Trim ();
                ssView_Sheet1.Cells [i , 6].Text = dt.Rows [i] ["JinBonAmt"].ToString ().Trim ();

                ssView_Sheet1.Cells [i , 7].Text = dt.Rows [i] ["GbOpdWork"].ToString ().Trim ();
                ssView_Sheet1.Cells [i , 8].Text = dt.Rows [i] ["GbOCS"].ToString ().Trim ();
                ssView_Sheet1.Cells [i , 9].Text = dt.Rows [i] ["GbDeptJepsu"].ToString ().Trim ();

                ssView_Sheet1.Cells [i , 10].Text = dt.Rows [i] ["GbTong"].ToString ().Trim ();
                ssView_Sheet1.Cells [i , 11].Text = dt.Rows [i] ["DelDate"].ToString ().Trim ();
                ssView_Sheet1.Cells [i , 12].Text = dt.Rows [i] ["ROWID"].ToString ().Trim ();
            }

            dt.Dispose ();
            dt = null;
        }

        private void btnSave_Click (object sender , EventArgs e)
        {
            if (ComQuery.IsJobAuth(this , "C", clsDB.DbCon) == false) return;//권한 확인

            if (SaveData() == true)
            {
                Search ();
            }
        }

        private bool SaveData()
        {
            int i = 0;
            int intRowAffected = 0; //변경된 Row 받는 변수
            int nJinbonAmt = 0;
            string SqlErr = ""; //에러문 받는 변수
            string strDel = "";
            string strCode = "";
            string strName = "";
            string strDeptCode = "";
            string strHDept = "";
            string strGbJinAmt = "";
            string strGbOpdWork = "";
            string strGbOcs = "";
            string strGbDeptJepsu = "";
            string strGbTong = "";
            string strROWID = "";
            string strChange = "";
            string strDeldate = "";
            string SQL = "";
            bool rtnVal = false;

            for (i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                strDel = ssView_Sheet1.Cells [i , 0].Text.ToString ().Trim ();
                strCode = ssView_Sheet1.Cells [i , 1].Text.ToString ().Trim ();
                strName = ssView_Sheet1.Cells [i , 2].Text.ToString ().Trim ();
                strDeptCode = ssView_Sheet1.Cells [i , 3].Text.ToString ().Trim ();
                strHDept = ssView_Sheet1.Cells [i , 4].Text.ToString ().Trim ();
                strGbJinAmt = ssView_Sheet1.Cells [i , 5].Text.ToString ().Trim ();
                nJinbonAmt = Convert.ToInt32 (VB.Val (ssView_Sheet1.Cells [i , 6].Text.ToString ().Trim ()));
                strGbOpdWork = ssView_Sheet1.Cells [i , 7].Text.ToString ().Trim ();
                strGbOcs = ssView_Sheet1.Cells [i , 8].Text.ToString ().Trim ();
                strGbDeptJepsu = ssView_Sheet1.Cells [i , 9].Text.ToString ().Trim ();
                strGbTong = ssView_Sheet1.Cells [i , 10].Text.ToString ().Trim ();
                strDeldate = ssView_Sheet1.Cells [i , 11].Text.ToString ().Trim ();
                strROWID = ssView_Sheet1.Cells [i , 12].Text.ToString ().Trim ();
                strChange = ssView_Sheet1.Cells [i , 13].Text.ToString ().Trim ();

                if (strDel != "1")
                {
                    if (strCode != "")
                    {
                        if (strName == "") { ComFunc.MsgBox (i + 1 + "번줄 코드명칭이 누락됨" , "오류"); }
                        if (strDeptCode == "") { ComFunc.MsgBox (i + 1 + "번줄 적용과가 누락됨" , "오류"); }
                        if (strGbJinAmt == "") { ComFunc.MsgBox (i + 1 + "번줄 진찰료 구분이 누락됨" , "오류"); }
                        if (strGbOpdWork != "Y") { ComFunc.MsgBox (i + 1 + "번줄 접수증 구분이 오류" , "오류"); }
                        if (strGbOcs != "Y") { ComFunc.MsgBox (i + 1 + "번줄 OCS 구분이 오류" , "오류"); }
                        if (strGbDeptJepsu != "Y") { ComFunc.MsgBox (i + 1 + "번줄 대기순번 구분이 오류" , "오류"); }
                        if (strGbTong != "Y") { ComFunc.MsgBox (i + 1 + "번줄 인원통계 구분이 오류" , "오류"); }
                    }
                    else
                    {
                        if (strName != "") { ComFunc.MsgBox (i + 1 + "번줄 코드가 누락됨" , "오류"); }
                        if (strDeptCode != "") { ComFunc.MsgBox (i + 1 + "번줄 코드가 누락됨" , "오류"); }
                    }
                }

            }
            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    strDel = ssView_Sheet1.Cells [i , 0].Text.ToString ().Trim ();
                    strCode = ssView_Sheet1.Cells [i , 1].Text.ToString ().Trim ();
                    strName = ssView_Sheet1.Cells [i , 2].Text.ToString ().Trim ();
                    strDeptCode = ssView_Sheet1.Cells [i , 3].Text.ToString ().Trim ();
                    strHDept = ssView_Sheet1.Cells [i , 4].Text.ToString ().Trim ();
                    strGbJinAmt = ssView_Sheet1.Cells [i , 5].Text.ToString ().Trim ();
                    nJinbonAmt = Convert.ToInt32 (VB.Val (ssView_Sheet1.Cells [i , 6].Text.ToString ().Trim ()));
                    strGbOpdWork = ssView_Sheet1.Cells [i , 7].Text.ToString ().Trim ();
                    strGbOcs = ssView_Sheet1.Cells [i , 8].Text.ToString ().Trim ();
                    strGbDeptJepsu = ssView_Sheet1.Cells [i , 9].Text.ToString ().Trim ();
                    strGbTong = ssView_Sheet1.Cells [i , 10].Text.ToString ().Trim ();
                    strDeldate = ssView_Sheet1.Cells [i , 11].Text.ToString ().Trim ();
                    strROWID = ssView_Sheet1.Cells [i , 12].Text.ToString ().Trim ();
                    strChange = ssView_Sheet1.Cells [i , 13].Text.ToString ().Trim ();

                    SQL = "";
                    if (strDel == "1")
                    {
                        if (strROWID != "") { SQL = "DELETE BAS_OPDJIN WHERE ROWID='" + strROWID + "' "; }
                    }
                    else
                    {
                        if (strROWID == "")
                        {
                            if (strCode != "")
                            {
                                SQL = "INSERT INTO BAS_OPDJIN (Code,Name,DeptCode,HDept,GbJinAmt,JinBonAmt,";
                                SQL = SQL + ComNum.VBLF + "GbOpdWork,GbOCS,GbDeptJepsu,GbTong,DelDate,";
                                SQL = SQL + ComNum.VBLF + "EntDate,EntSabun) VALUES ('";
                                SQL = SQL + ComNum.VBLF + strCode + "','" + strName + "','" + strDeptCode + "','";
                                SQL = SQL + ComNum.VBLF + strHDept + "','" + strGbJinAmt + "'," + nJinbonAmt + ",'";
                                SQL = SQL + ComNum.VBLF + strGbOpdWork + "','" + strGbOcs + "','" + strGbDeptJepsu + "','";
                                SQL = SQL + ComNum.VBLF + strGbTong + "',";
                                SQL = SQL + ComNum.VBLF + "TO_DATE('" + strDeldate + "','YYYY-MM-DD'),";
                                SQL = SQL + ComNum.VBLF + "SYSDATE," + GSabun + ") ";
                            }
                        }
                        else
                        {
                            if (strChange == "Y")
                            {
                                SQL = "UPDATE BAS_OPDJIN SET ";
                                SQL = SQL + ComNum.VBLF + " Name='" + strName + "',";
                                SQL = SQL + ComNum.VBLF + " DeptCode='" + strDeptCode + "',";
                                SQL = SQL + ComNum.VBLF + " HDept='" + strHDept + "',";
                                SQL = SQL + ComNum.VBLF + " GbJinAmt='" + strGbJinAmt + "',";
                                SQL = SQL + ComNum.VBLF + " JinBonAmt=" + nJinbonAmt + ",";
                                SQL = SQL + ComNum.VBLF + " GbOpdWork='" + strGbOpdWork + "',";
                                SQL = SQL + ComNum.VBLF + " GbOCS='" + strGbOcs + "',";
                                SQL = SQL + ComNum.VBLF + " GbDeptJepsu='" + strGbDeptJepsu + "',";
                                SQL = SQL + ComNum.VBLF + " GbTong='" + strGbTong + "',";
                                SQL = SQL + ComNum.VBLF + " DelDate=TO_DATE('" + strDeldate + "','YYYY-MM-DD'),";
                                SQL = SQL + ComNum.VBLF + " EntDate=SYSDATE,EntSabun=" + GSabun + " ";
                                SQL = SQL + ComNum.VBLF + " WHERE Code='" + strCode + "' ";
                            }
                        }
                    }
                    SqlErr = clsDB.ExecuteNonQuery (SQL , ref intRowAffected , clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran (clsDB.DbCon);
                        ComFunc.MsgBox (SqlErr);
                        clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
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
                return rtnVal;
            }
        }

        private void btnExit_Click (object sender , EventArgs e)
        {
            this.Close ();
        }

        private void ssView_EditModeOff (object sender , EventArgs e)
        {
            int intRow = ssView_Sheet1.ActiveRowIndex;

            ssView_Sheet1.Cells [intRow , 13].Text = "Y";
        }

        private void ssView_LeaveCell (object sender , FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            switch (e.Column)
            {
                case 0:
                    lblmemo.Text = "삭제를 하실려면 버튼을 선택을 하세요..";
                    break;
                case 1:
                    lblmemo.Text = "코드를 입력하세요..";
                    break;
                case 2:
                    lblmemo.Text = "코드명칭을 입력하세요.";
                    break;
                case 3:
                    lblmemo.Text = "적용할 진료과를 입력하세요 (**.전체과)";
                    break;
                case 4:
                    lblmemo.Text = "협진과를 선택하세요..";
                    break;
                case 5:
                    lblmemo.Text = "1.정상산정 2.진찰료없음 3.본인정율 4.본인정액 5.본인정액,나머지 감액 6.본인정율 나머지 감액";
                    break;
                case 6:
                    lblmemo.Text = "정율(0-100), 정액은 금액을 입력하세요..";
                    break;
                case 7:
                    lblmemo.Text = "접수증 및 차트대출 여부(Y/N)";
                    break;
                case 8:
                    lblmemo.Text = "외래OCS 환자명단 표시 여부(Y/N)";
                    break;
                case 9:
                    lblmemo.Text = "대기순번에 환자명단 표시 여부(Y/N)";
                    break;
                case 10:
                    lblmemo.Text = "인원통계 포함 여부(Y/N)";
                    break;
                case 11:
                    lblmemo.Text = "적용취소(삭제)일자는?";
                    break;
                default:
                    lblmemo.Text = "";
                    break;
            }
        }
    }
}
