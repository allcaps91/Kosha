using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmCertainSuCodeSearch
    /// File Name : frmCertainSuCodeSearch.cs
    /// Title or Description :     VRE 스크리닝 검사 현황 페이지
    /// Author : 박성완
    /// Create Date : 2017-06-05
    /// <history> 
    /// </history>
    /// </summary>
    public partial class frmCertainSuCodeSearch : Form
    {
        public frmCertainSuCodeSearch()
        {
            InitializeComponent();
        }

        private void frmCertainSuCodeSearch_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
            ss1_Sheet1.Columns[11].Visible = false;

            txtFDate.Text = DateTime.Parse(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).AddDays(-7).ToShortDateString();
            txtTDate.Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            cboSuCode.Items.Clear();
            cboSuCode.Items.Add("1.VRE screening culture (B4051D)");
            cboSuCode.SelectedIndex = 0;
        }

        /// <summary>
        /// 자료조회 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnView_Click(object sender, EventArgs e)
        {
            if (ViewData() == false) return;
        }

        private bool ViewData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return false; //권한 확인

            string strOrderCode = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            DataTable dt1 = null;

            string[] separators = { "(", ")" };
            string[] words = cboSuCode.Text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            strOrderCode = words[1].Trim().ToUpper();

            if (strOrderCode == "")
            {
                MessageBox.Show("검사코드 이상!!", "확인");
                return false;
            }

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT 'I' as IOGubun ,a.Ptno,b.SName,a.DeptCode,a.Staffid DrCode, ";
                SQL += ComNum.VBLF + " TO_CHAR(a.BDate,'YYYY-MM-DD') BDATE,TO_CHAR(a.EntDate,'YYYY-MM-DD HH24:MI') EntDATE, ";
                SQL += ComNum.VBLF + " b.Sex,a.SuCode,a.OrderCode,a.OrderNo,c.Age,c.SpecNo ";
                SQL += ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_IORDER a, " + ComNum.DB_PMPA + "BAS_PATIENT b,  " + ComNum.DB_MED + "EXAM_ORDER c ";
                SQL += ComNum.VBLF + "  WHERE a.PTNO=b.PANO ";
                SQL += ComNum.VBLF + "  AND a.Ptno=c.Pano(+)";
                SQL += ComNum.VBLF + "  AND a.BDate=c.BDate ";
                SQL += ComNum.VBLF + "  AND a.OrderNo=c.OrderNo ";
                SQL += ComNum.VBLF + "  AND a.BDATE >=TO_DATE('" + txtFDate.Text.Trim() + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND a.BDATE <=TO_DATE('" + txtTDate.Text.Trim() + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND TRIM(a.OrderCode) ='" + strOrderCode + "' ";
                SQL += ComNum.VBLF + "  AND GBPICKUP = '*' ";
                SQL += ComNum.VBLF + "  AND GbStatus  IN  (' ','D+','D','D-') ";
                SQL += ComNum.VBLF + "  AND ( GBIOE NOT IN ('E') OR GBIOE IS NULL) ";
                SQL += ComNum.VBLF + " GROUP BY a.Ptno,b.SName,a.DeptCode,a.Staffid , ";
                SQL += ComNum.VBLF + " TO_CHAR(a.BDate,'YYYY-MM-DD') ,TO_CHAR(a.EntDate,'YYYY-MM-DD HH24:MI') ,  b.Sex,a.SuCode,a.OrderCode,a.OrderNo,c.Age,c.SpecNo";

                SQL += ComNum.VBLF + " UNION ALL ";
                SQL += ComNum.VBLF + " SELECT 'E' as IOGubun ,a.Ptno,b.SName,a.DeptCode,a.Staffid DrCode, ";
                SQL += ComNum.VBLF + " TO_CHAR(a.BDate,'YYYY-MM-DD') BDATE,TO_CHAR(a.EntDate,'YYYY-MM-DD HH24:MI') EntDATE, ";
                SQL += ComNum.VBLF + " b.Sex,a.SuCode,a.OrderCode,a.OrderNo,c.Age,c.SpecNo ";
                SQL += ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_IORDER a, " + ComNum.DB_PMPA + "BAS_PATIENT b,  " + ComNum.DB_MED + "EXAM_ORDER c ";
                SQL += ComNum.VBLF + "  WHERE a.PTNO=b.PANO ";
                SQL += ComNum.VBLF + "  AND a.Ptno=c.Pano(+)";
                SQL += ComNum.VBLF + "  AND a.BDate=c.BDate ";
                SQL += ComNum.VBLF + "  AND a.OrderNo=c.OrderNo ";
                SQL += ComNum.VBLF + "  AND a.BDATE >=TO_DATE('" + txtFDate.Text.Trim() + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND a.BDATE <=TO_DATE('" + txtTDate.Text.Trim() + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND TRIM(a.OrderCode) ='" + strOrderCode + "' ";
                SQL += ComNum.VBLF + "  AND GbSend = ' ' ";
                SQL += ComNum.VBLF + "  AND GbStatus  IN  (' ','D+','D','D-') ";
                SQL += ComNum.VBLF + "  AND  GBIOE IN ('E') ";
                SQL += ComNum.VBLF + " GROUP BY a.Ptno,b.SName,a.DeptCode,a.Staffid , ";
                SQL += ComNum.VBLF + " TO_CHAR(a.BDate,'YYYY-MM-DD') ,TO_CHAR(a.EntDate,'YYYY-MM-DD HH24:MI') ,  b.Sex,a.SuCode,a.OrderCode,a.OrderNo,c.Age,c.SpecNo";

                SQL += ComNum.VBLF + " UNION ALL ";
                SQL += ComNum.VBLF + " SELECT 'O' as IOGubun ,a.Ptno,b.SName,a.DeptCode,a.DrCode, ";
                SQL += ComNum.VBLF + " TO_CHAR(a.BDate,'YYYY-MM-DD') BDATE,TO_CHAR(a.EntDate,'YYYY-MM-DD HH24:MI') EntDATE, ";
                SQL += ComNum.VBLF + " b.Sex,a.SuCode,a.OrderCode,a.OrderNo,c.Age,c.SpecNo ";
                SQL += ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_oORDER a, " + ComNum.DB_PMPA + "BAS_PATIENT b,  " + ComNum.DB_MED + "EXAM_ORDER c ";
                SQL += ComNum.VBLF + "  WHERE a.PTNO=b.PANO ";
                SQL += ComNum.VBLF + "  AND a.Ptno=c.Pano(+)";
                SQL += ComNum.VBLF + "  AND a.BDate=c.BDate ";
                SQL += ComNum.VBLF + "  AND a.OrderNo=c.OrderNo ";
                SQL += ComNum.VBLF + "  AND a.BDATE >=TO_DATE('" + txtFDate.Text.Trim() + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND a.BDATE <=TO_DATE('" + txtTDate.Text.Trim() + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND TRIM(a.OrderCode) ='" + strOrderCode + "' ";
                SQL += ComNum.VBLF + "  AND a.GbSunap ='1' ";
                SQL += ComNum.VBLF + " GROUP BY a.Ptno,b.SName,a.DeptCode,a.DrCode , ";
                SQL += ComNum.VBLF + " TO_CHAR(a.BDate,'YYYY-MM-DD') ,TO_CHAR(a.EntDate,'YYYY-MM-DD HH24:MI') ,  b.Sex,a.SuCode,a.OrderCode,a.OrderNo,c.Age,c.SpecNo";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return false;
                }
                ss1_Sheet1.Rows.Count = dt.Rows.Count;
                ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Ptno"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["BDate"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    //TODO: frm특정수가조회 에 있는 READ_BAS_Doctor 함수 임시로 만들어 사용
                    ss1_Sheet1.Cells[i, 4].Text = ReadBASDoctor(dt.Rows[i]["DrCode"].ToString().Trim());
                    ss1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["IOGubun"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Sex"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["Age"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 8].Text = "";

                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT a.MasterCode,a.SubCode,a.Status,b.Status stat,a.ReSult ";
                    SQL += ComNum.VBLF + " FROM KOSMOS_OCS.EXAM_RESULTC a, KOSMOS_OCS.EXAM_SPECMST b  ";
                    SQL += ComNum.VBLF + "  WHERE a.Pano=b.Pano ";
                    SQL += ComNum.VBLF + "   AND a.SpecNo=b.SpecNo ";
                    SQL += ComNum.VBLF + "   AND a.PANO ='" + dt.Rows[i]["PTNO"].ToString().Trim() + "' ";
                    SQL += ComNum.VBLF + "   AND a.SPECNO ='" + dt.Rows[i]["SpecNo"].ToString().Trim() + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return false;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        ComFunc.MsgBox("해당 DATA가 없습니다.");
                        return false;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        if (dt1.Rows[0]["STAT"].ToString().Trim() == "05")
                        {
                            ss1_Sheet1.Cells[i, 8].Text = dt1.Rows[0]["Result"].ToString().Trim();
                        }
                        else if (dt1.Rows[0]["STAT"].ToString().Trim() == "06")
                        {
                            ss1_Sheet1.Cells[i, 8].Text = "취소";
                        }
                        else if (dt1.Rows[0]["STAT"].ToString().Trim() == "00")
                        {
                            ss1_Sheet1.Cells[i, 8].Text = "미접수";
                        }
                        else
                        {
                            ss1_Sheet1.Cells[i, 8].Text = "진행중";
                        }
                    }
                    dt1.Dispose();
                    dt1 = null;

                    ss1_Sheet1.Cells[i, 9].Text = dt.Rows[i]["SuCode"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 10].Text = dt.Rows[i]["OrderCode"].ToString().Trim() + " " + ReadOrderName(dt.Rows[i]["OrderCode"].ToString().Trim());
                    //TODO:VB에서의 SQL 문으로 따오는 테이블 중 ROWID 컬럼은 없어 예외처리로 자꾸 넘어감 확인 필요
                    //ss1_Sheet1.Cells[i, 11].Text = dt.Rows[i]["ROWID"].ToString().Trim();                   
                }
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        //TODO: Read_OrderName 공용 함수 인지 확인 필요
        private string ReadOrderName(string argCode)
        {
            DataTable dt = null;
            string SQL = "";
            string SQLErr = ""; //에러문 받는 변수
            string rtnval = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT OrderName FROM KOSMOS_OCS.OCS_ORDERCODE ";
            SQL += ComNum.VBLF + " WHERE OrderCode = '" + argCode.Trim() + "'   ";
            SQL += ComNum.VBLF + "   AND (SendDept <> 'N' OR SendDept IS NULL) ";

            SQLErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SQLErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SQLErr, SQL, clsDB.DbCon); //에러로그 저장
                return rtnval;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                return rtnval;
            }

            rtnval = dt.Rows[0]["OrderName"].ToString();

            return rtnval;
        }

        //TODO: frm특정수가조회 에 있는 READ_BAS_Doctor 함수 임시로 만들어 사용
        private string ReadBASDoctor(string argDrCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";
            DataTable dt = null;

            if (argDrCode.Trim() == "")
            {
                rtnVal = "";
            }
            try
            {

                SQL = "";
                SQL += ComNum.VBLF + "SELECT DrName FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
                SQL += ComNum.VBLF + " WHERE DrCode='" + argDrCode + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["DrName"].ToString().Trim();
                }
                else
                {
                    rtnVal = "";
                }
            }
            catch (Exception ex)
            {
                rtnVal = "";
                MessageBox.Show(ex.Message);
            }
            finally
            {
                dt.Dispose();
                dt = null;
            }
            return rtnVal;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ss1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Column == 0)
            {
                clsPublic.GstrHelpCode = ss1_Sheet1.Cells[e.Row, 0].Text;
                //TODO:frmViewResult.Show 에 맞는 폼 필요
                //frmViewResult.Show

                return;
            }

            //TODO: 성명 컬럼 더블클릭시 EMR 셋팅 부분 구현 필요
        }

        /// <summary>
        /// 인쇄버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            btnPrint.Enabled = false;

            strFont1 = "/fn\"굴림\" /fz\"12\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont1 = "/fn\"굴림체\" /fz\"11\"";

            strHead1 = "/f1/c" + cboSuCode.Text.Trim() + " List " + "/n/n";
            strHead2 = "조회기간:" + txtFDate.Text.Trim() + "~" + txtTDate.Text.Trim();
            strHead2 += "        출력시간 : " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D") + " ";
            strHead2 += ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T") + VB.Space(5) + "Page : " + "/p";

            ss1_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ss1_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ss1_Sheet1.PrintInfo.Margin.Top = 10;
            ss1_Sheet1.PrintInfo.Margin.Bottom = 10;
            ss1_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ss1_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ss1_Sheet1.PrintInfo.ShowBorder = true;
            ss1_Sheet1.PrintInfo.ShowColor = false;
            ss1_Sheet1.PrintInfo.ShowGrid = true;
            ss1_Sheet1.PrintInfo.ShowShadows = false;
            ss1_Sheet1.PrintInfo.UseMax = false;
            ss1_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ss1_Sheet1.PrintInfo.Preview = true;
            ss1.PrintSheet(0);
        }
    }
}
