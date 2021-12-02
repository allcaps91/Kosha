using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{    /// <summary>
     /// Class Name      : ComPmpaLibB
     /// File Name       : frmPmPaVIEWJewonlistNew.cs
     /// Description     : 재원자명단
     /// Author          : 김효성
     /// Create Date     : 2017-09-15
     /// Update History  : 
     /// </summary>
     /// <history>  
     /// 
     /// </history>
     /// <seealso cref= "D:\psmh\IPD\iument\iument.vbp\frm재원자명단_NEW.frm  >> frmPmPaVIEWJewonlistNew.cs 폼이름 재정의" />	
     /// 
    public partial class frmPmPaVIEWJewonlistNew : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        string FstrPanoList = "";

        public frmPmPaVIEWJewonlistNew()
        {
            InitializeComponent();
        }

        public frmPmPaVIEWJewonlistNew(string strPanoList)
        {
            FstrPanoList = strPanoList;
            InitializeComponent();
        }

        private void frmPmPaVIEWJewonlistNew_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            int j = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            Cursor.Current = Cursors.WaitCursor;

            try
            {

                //'병동
                SQL = "";
                SQL = "SELECT WardCode, WardName  ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_WARD ";
                SQL = SQL + ComNum.VBLF + " WHERE WARDCODE NOT IN ('IU','NP','2W','ER') ";
                SQL = SQL + ComNum.VBLF + " ORDER BY WardCode ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;

                }

                cboWard.Items.Clear();
                cboWard.Items.Add("전체");

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboWard.Items.Add(dt.Rows[i]["WardCode"].ToString().Trim());
                }

                cboWard.Items.Add("SICU");
                cboWard.Items.Add("MICU");
                cboWard.SelectedIndex = 0;

                dt.Dispose();
                dt = null;

                //'진료과 Combo SET
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT DeptCode ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_ClinicDept ";
                SQL = SQL + ComNum.VBLF + " WHERE DeptCode NOT IN ('II','HR','TO','R6','HD','ER','PT','AN') ";
                SQL = SQL + ComNum.VBLF + " ORDER BY PrintRanking ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;

                }

                cbodept.Items.Clear();
                cbodept.Items.Add("전체");

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cbodept.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;
                cbodept.SelectedIndex = 0;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            bool bToday = false;
            int i = 0;
            int j = 0;
            int nLine = 0;
            int nRead = 0;
            int nRow = 0;
            int nIlsu = 0;
            int nAge = 0;
            int nDietIlsu = 0;
            string strWard = "";
            string strOK = "";
            string strPano = "";
            string strInDate = "";
            string strRoom = "";
            string strOpSche = "";
            string strSpecialExam = "";
            string strOldCode = "";
            string strRoutDate = "";
            string strAmset1 = "";
            string strAmSet3 = "";
            string strAmSet7 = "";
            string strAmsetB = "";
            string strDietName = "";
            string strDietDate = "";
            string strPriDate = "";
            string strToDate = "";
            string strNextDate = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            if (cboWard.Text == "")
            {
                ComFunc.MsgBox("병동이 공란입니다.", "오류");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            ssView_Sheet1.RowCount = 0;

            try
            {

                strPriDate = Convert.ToDateTime(strDTP).AddDays(-1).ToString("yyyy-MM-dd");
                strToDate = strDTP;
                strNextDate = Convert.ToDateTime(strDTP).AddDays(1).ToString("yyyy-MM-dd");



                //'대상 환자를 읽음
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT M.WardCode,M.RoomCode,M.Pano,M.SName,M.Sex,M.Age,M.Bi,M.PName,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(M.InDate,'YYYY-MM-DD') InDate,M.Ilsu,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(M.ROutDate,'YYYY-MM-DD') ROutDate,M.AmSet3,b.Juso,b.ZipCode1||b.ZipCode2 ZipCode,b.JiCode,M.Jiyuk, ";
                SQL = SQL + ComNum.VBLF + " M.DeptCode,D.DrName,M.AmSet1,M.AmSet6,M.AmSet7 ";
                SQL = SQL + ComNum.VBLF + " FROM   " + ComNum.DB_PMPA + "IPD_NEW_MASTER  M, " + ComNum.DB_PMPA + "BAS_PATIENT B, ";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_DOCTOR  D ";
                SQL = SQL + ComNum.VBLF + " WHERE 1=1";

                switch (cboWard.Text.Trim())
                {
                    case "전체":
                        SQL = SQL + ComNum.VBLF + "AND M.WardCode>' ' ";
                        break;
                    case "MICU":
                        SQL = SQL + ComNum.VBLF + "AND M.RoomCode='234' ";
                        break;
                    case "SICU":
                        SQL = SQL + ComNum.VBLF + "AND M.RoomCode='233' ";
                        break;
                    default:
                        SQL = SQL + ComNum.VBLF + "AND M.WardCode='" + cboWard.Text.Trim() + "' ";
                        break;
                }

                SQL = SQL + ComNum.VBLF + "  AND M.Pano<>'81000004' ";
                SQL = SQL + ComNum.VBLF + "  AND M.Amset4 <> '3' ";// '정상애기 제외

                //'진료과
                if (cbodept.Text != "전체")
                {
                    SQL = SQL + ComNum.VBLF + " AND M.DeptCode='" + cbodept.Text + "' ";
                }

                SQL = SQL + ComNum.VBLF + " AND (M.OutDate IS NULL OR M.OutDate>=TO_DATE('" + strNextDate + "','YYYY-MM-DD'))";
                SQL = SQL + ComNum.VBLF + " AND M.IpwonTime < TO_DATE('" + strNextDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + " AND M.Pano < '90000000'";
                SQL = SQL + ComNum.VBLF + " AND M.DrCode=D.DrCode(+) ";
                SQL = SQL + ComNum.VBLF + " AND M.PANO=B.PANO(+) ";


                //'SORT
                if (rdo0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY M.WardCode,M.RoomCode,M.SName ";
                }
                else if (rdo1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY M.SName,M.Pano ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY M.DeptCode,D.DrName,M.SName ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;

                }

                nRow = 0;
                nLine = 0;
                FstrPanoList = "";
                strOldCode = "";

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strOK = "OK";

                    strPano = dt.Rows[i]["Pano"].ToString().Trim();
                    strInDate = dt.Rows[i]["InDate"].ToString().Trim();
                    strWard = dt.Rows[i]["WardCode"].ToString().Trim();
                    strRoom = dt.Rows[i]["RoomCode"].ToString().Trim();
                    strRoutDate = dt.Rows[i]["RoutDate"].ToString().Trim();

                    if (string.Compare(strRoutDate, strDTP) < 0)
                    {
                        strRoutDate = "";
                    }
                    strAmset1 = dt.Rows[i]["AmSet1"].ToString().Trim();
                    strAmSet3 = dt.Rows[i]["AmSet3"].ToString().Trim();
                    strAmSet7 = dt.Rows[i]["AmSet7"].ToString().Trim();// '입원경로
                    nIlsu = Convert.ToInt32(dt.Rows[i]["Ilsu"].ToString().Trim());
                    nAge = Convert.ToInt32(dt.Rows[i]["Age"].ToString().Trim());


                    if (strOK == "OK")
                    {
                        nRow = nRow + 1;
                        if (nRow > ssView_Sheet1.RowCount)
                        {
                            ssView_Sheet1.RowCount = nRow;
                        }
                        ssView_Sheet1.Cells[nRow - 1, 1].Text = strWard;
                        ssView_Sheet1.Cells[nRow - 1, 2].Text = strRoom;
                        ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["Age"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 8].Text = dt.Rows[i]["DrName"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["InDate"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["Bi"].ToString().Trim();

                        ssView_Sheet1.Cells[nRow - 1, 11].Text = (VB.DateDiff("d", strInDate, strDTP)).ToString();
                        ssView_Sheet1.Cells[nRow - 1, 12].Text = dt.Rows[i]["ZipCode"].ToString().Trim() + dt.Rows[i]["Juso"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 13].Text = dt.Rows[i]["JiCode"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 14].Text = "";
                        switch (dt.Rows[i]["AmSet7"].ToString().Trim())
                        {
                            case "3":
                            case "4":
                            case "5":
                                ssView_Sheet1.Cells[nRow - 1, 14].Text = "E";
                                break;
                            default:
                                ssView_Sheet1.Cells[nRow - 1, 15].Text = dt.Rows[i]["ZipCode"].ToString().Trim();
                                break;
                        }
                    }
                }
                dt.Dispose();
                dt = null;
                ssView_Sheet1.RowCount = nRow;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }     //권한확인

            strTitle = "재  원   환  자   명  단";


            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("병동 : " + cboWard.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);

        }

        private void btnSelPrnt_Click(object sender, EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;
            int i = 0;
            int k = 0;
            int nRow = 0;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }     //권한확인

            strTitle = "재  원   환  자   명  단";

            for (i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                if (Convert.ToBoolean(ssView_Sheet1.Cells[i, 0].Value) == true)
                {
                    nRow = nRow + 1;
                    ssView_Sheet1.RowCount = nRow;
                }
            }

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("병동 : " + cboWard.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);


        }

        private void btncansel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
