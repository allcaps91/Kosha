using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// Class Name      : ComLibB.dll
    /// File Name       : frmBuKiho05.cs
    /// Description     : 코드 찾기
    /// Author          : 김효성
    /// Create Date     : ?
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 2017-06-19 조회 부분 수정. - 박성완
    /// </history>
    /// <seealso> 
    /// 
    /// </seealso>
    /// <vbp>
    /// default : 
    /// seealso : 
    /// </vbp>
    public partial class frmBuKiho05 : Form
    {
        string fstrGubun = "";
        string fstrData = "";
        string GstrRetValue = "";
        int fnRow = 0;

        //이벤트를 전달할 경우
        public delegate void SetCodeName (string strRetValue);
        public event SetCodeName rSetCodeName;

        //폼이 Close될 경우
        public delegate void EventClosed ();
        public event EventClosed rEventClosed;

        public frmBuKiho05 ()
        {
            InitializeComponent ();
        }

        public frmBuKiho05 (string strRetValue)
        {
            InitializeComponent ();

            GstrRetValue = strRetValue;
        }

        private void frmBuKiho05_Load (object sender , EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회 EX
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            ssView_Sheet1.SetRowHeight (-1 , Convert.ToInt32 (ssView_Sheet1.GetPreferredRowHeight (-1)) + 10);

            fstrGubun = VB.Left (GstrRetValue , 2);
            fstrData = VB.Right (GstrRetValue , VB.Len (GstrRetValue) - 2);

            txtdata.Text = "";

            fnRow = 1;

            switch (fstrGubun)
            {
                case "01":
                    lblMenu.Text = "코드찾기【업종】";
                    break;
                case "02":
                    lblMenu.Text = "코드찾기【지방관서】";
                    break;
                case "03":
                    lblMenu.Text = "코드찾기【지도원】";
                    break;
                case "04":
                    lblMenu.Text = "코드찾기【제품코드】";
                    break;
                case "05":
                    lblMenu.Text = "코드찾기【직종】";
                    break;
                case "06":
                    lblMenu.Text = "코드찾기【작업공정】";
                    break;
                case "GJ":
                    lblMenu.Text = "코드찾기【건진종류】";
                    break;
                case "08":
                    lblMenu.Text = "코드찾기【유해물질동의어";
                    break;
                case "09":
                    lblMenu.Text = "코드찾기【유해인자】";
                    break;
                case "10":
                    lblMenu.Text = "코드찾기【질병분류";
                    break;
                case "11":
                    lblMenu.Text = "코드찾기【자타각증상】";
                    break;
                case "12":
                    lblMenu.Text = "코드찾기【사후관리내용】";
                    break;
                case "13":
                    lblMenu.Text = "코드찾기【업무적합성】";
                    break;
                case "14":
                    lblMenu.Text = "코드찾기【조치코드】";
                    break;
                case "15":
                    lblMenu.Text = "코드찾기【측정방법】";
                    break;
                case "16":
                    lblMenu.Text = "코드찾기【측정분석】";
                    break;
                case "17":
                    lblMenu.Text = "코드찾기【결과값코드】";
                    break;
                case "18":
                    lblMenu.Text = "코드찾기【사업장기호】";
                    break;
                case "19":
                    lblMenu.Text = "코드찾기【질병분류코드】";
                    break;
                case "21":
                    lblMenu.Text = "코드찾기【건강보험지사】";
                    break;
                case "22":
                    lblMenu.Text = "코드찾기【통계분류코드】";
                    break;
                case "23":
                    lblMenu.Text = "코드찾기【특정수가분류】";
                    break;
                case "24":
                    lblMenu.Text = "코드찾기【인원통계분류】";
                    break;
                case "25":
                    lblMenu.Text = "코드찾기【보건소(시군구)】";
                    break;
                case "26":
                    lblMenu.Text = "코드찾기【군병원】";
                    break;
                case "27":
                    lblMenu.Text = "코드찾기【일반소견,조치】";
                    break;
                case "31":
                    lblMenu.Text = "코드찾기【직업병코드】";
                    break;
                case "51":
                    lblMenu.Text = "코드찾기【취급물질명】";
                    break;
                case "53":
                    lblMenu.Text = "코드찾기【특수추가검사】";
                    break;
                case "54":
                    lblMenu.Text = "코드찾기【특수판정소견】";
                    break;
                case "IL":
                    lblMenu.Text = "코드찾기【상병코드ICD10】";
                    break;
                default:
                    lblMenu.Text = "코드찾기【오류】";
                    break;
            }
            Search();
        }

        private void btnExit_Click (object sender , EventArgs e)
        {
            rEventClosed ();
        }

        private void Search ()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                txtdata.Text = txtdata.Text.Trim();
                if (fstrGubun == "IL" && (txtdata.Text.Length < 6))
                {
                    MessageBox.Show("상병코드 찾기는 반드시 찾으실 명칭을 입력하세요", "오류");
                    return;
                }

                if (Convert.ToInt16(fstrGubun) < 50 || fstrGubun == "53")
                {
                    SQL = "SELECT Code,Name,GCODE FROM HIC_CODE ";
                    SQL = SQL + ComNum.VBLF + "WHERE Gubun='" + fstrGubun + "' ";
                    if (txtdata.Text != "") { SQL = SQL + ComNum.VBLF + " AND Name LIKE '%" + txtdata.Text + "%' "; }
                    if (fstrGubun == "10")
                    {
                        switch (VB.Right(GstrRetValue, 1))
                        {
                            case "1":
                                SQL = SQL + ComNum.VBLF + " AND GCODE ='A' "; break;
                            case "2":
                                SQL = SQL + ComNum.VBLF + " AND GCODE ='B' "; break;
                            case "3":
                                SQL = SQL + ComNum.VBLF + " AND GCODE ='C1' "; break;
                            case "4":
                                SQL = SQL + ComNum.VBLF + " AND GCODE ='C2' "; break;
                            case "5":
                                SQL = SQL + ComNum.VBLF + " AND GCODE ='D1' "; break;
                            case "6":
                                SQL = SQL + ComNum.VBLF + " AND GCODE ='D2' "; break;
                            case "7":
                                SQL = SQL + ComNum.VBLF + " AND GCODE ='R' "; break;
                            case "8":
                                SQL = SQL + ComNum.VBLF + " AND GCODE ='U' "; break;
                        }
                    }
                    SQL = SQL + ComNum.VBLF + " AND ROWNUM <= 500 ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY CODE ";
                }
                else if (fstrGubun == "54")
                {
                    SQL = "SELECT Code,Name FROM HIC_SPC_SCODE ";
                    SQL = SQL + ComNum.VBLF + "WHERE Panjeng='" + VB.Right(GstrRetValue, 1) + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND DelDate IS NULL ";

                    if (txtdata.Text != "")
                    {
                        SQL = SQL + ComNum.VBLF + " AND Name LIKE '%" + txtdata.Text + "%' ";
                        SQL = SQL + ComNum.VBLF + " AND ROWNUM <= 500 ";
                        SQL = SQL + ComNum.VBLF + "ORDER BY CODE ";
                    }
                }
                else if (fstrGubun == "51")
                {
                    SQL = "SELECT Code,Name FROM HIC_MCODE ";
                    if (txtdata.Text != "")
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE Name LIKE '%" + txtdata.Text + "%' ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE Name IS NOT NULL ";
                    }
                    SQL = SQL + ComNum.VBLF + " AND ROWNUM <= 500 ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY Code ";
                }
                else if (fstrGubun == "GJ")
                {
                    SQL = "SELECT Code,Name FROM HIC_EXJONG ";
                    if (txtdata.Text != "")
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE Name LIKE '%" + txtdata.Text + "%' ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE Name IS NOT NULL ";
                    }
                    SQL = SQL + ComNum.VBLF + " AND ROWNUM <= 500 ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY Code ";
                }

                else if (fstrGubun == "IL")
                {
                    SQL = "SELECT IllCode Code,IllNameK Name FROM BAS_ILLS ";
                    SQL = SQL + ComNum.VBLF + "WHERE IllNameK LIKE '%" + txtdata.Text + "%' ";
                    SQL = SQL + ComNum.VBLF + " AND ROWNUM <= 500 ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY IllCode ";
                }
                else if (fstrGubun == "SA")
                {
                    SQL = " SELECT SABUN CODE,KORNAME NAME FROM KOSMOS_ADM.INSA_MST  ";
                    if (txtdata.Text != "")
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE KORNAME LIKE '%" + txtdata.Text.Trim() + "%' ";
                        SQL = SQL + ComNum.VBLF + "  AND JAEGU ='0' ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE KORNAME IS NOT NULL ";
                        SQL = SQL + ComNum.VBLF + "  AND JAEGU ='0' ";
                    }
                    SQL = SQL + ComNum.VBLF + " AND ROWNUM <= 500 ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY KORNAME ";
                }
                else if (fstrGubun == "JO")
                {
                    SQL = " SELECT CODE,NAME FROM HIC_CODE ";
                    if (txtdata.Text != "")
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE NAME LIKE '%" + txtdata.Text.Trim() + "%' ";
                        SQL = SQL + ComNum.VBLF + " AND ROWNUM <= 30 ";
                    }
                    else
                    {
                        return;
                    }
                }

                else
                {
                    return;
                }

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

                ssView_Sheet1.RowCount = dt.Rows.Count;
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Code"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Name"].ToString().Trim();
                }

                ssView.Focus();

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }
        private void btnSearch_Click (object sender , EventArgs e)
        {
            Search ();
        }

        private void ssView_CellDoubleClick (object sender , FarPoint.Win.Spread.CellClickEventArgs e)
        {
            GstrRetValue = VB.Left (ssView_Sheet1.Cells [e.Row , 0].Text + VB.Space (10) , 10);
            GstrRetValue = GstrRetValue + ssView_Sheet1.Cells [e.Row , 1].Text;

            rSetCodeName (GstrRetValue);
            rEventClosed ();
        }

        private void ssView_KeyDown (object sender , KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && ssView_Sheet1.ActiveRowIndex != 0)
            {
                GstrRetValue = VB.Left (ssView_Sheet1.Cells [ssView_Sheet1.ActiveRowIndex , 0].Text + VB.Space (10) , 10);
                GstrRetValue = GstrRetValue + ssView_Sheet1.Cells [ssView_Sheet1.ActiveRowIndex , 1].Text;

                rSetCodeName (GstrRetValue);
                rEventClosed ();
            }
        }

        private void ssView_LeaveCell (object sender , FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            fnRow = e.Row;
        }

        private void txtdata_EnabledChanged (object sender , EventArgs e)
        {
            switch (fstrGubun)
            {
                case "02":
                case "03":
                case "07":
                case "09":
                case "10":
                case "11":
                    Search ();
                    break;
                case "12":
                case "13":
                case "14":
                case "15":
                case "16":
                case "17":
                case "19":
                    Search ();
                    break;
                case "22":
                case "23":
                case "26":
                case "31":
                case "51":
                case "53":
                case "54":
                    Search ();
                    break;
                default:
                    this.ImeMode = ImeMode.Hangul;
                    break;
            }
        }

        private void txtdata_KeyDown (object sender , KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send ("{Tab}");
            }
        }

        private void txtdata_Leave (object sender , EventArgs e)
        {
            this.ImeMode = ImeMode.Hangul;
        }
    }
}
