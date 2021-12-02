using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{

    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-10-23
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= D:\psmh\IPD\ipdSim2\ipdsim2.vbp\IUSENT95.frm" >> frmPmpaViewAllRoomDeptSearch.cs 폼이름 재정의" />
    /// <seealso cref= D:\psmh\diet\dietorder\dietorder.vbp\dietorder65.frm" >> frmPmpaViewAllRoomDeptSearch.cs 폼이름 재정의" />
    /// <seealso cref= D:\psmh\IPD\iument\iument.vbp\Frm일자별전실전과내역.frm" >> frmPmpaViewAllRoomDeptSearch.cs 폼이름 재정의" />


    public partial class frmPmpaViewAllRoomDeptSearch : Form
    {
        /// <summary>
        /// Flag == 1 (ipdsim2.vbp)
        /// Flag == 2 (dietorder.vbp)
        /// Flag == 3 (iument.vbp)
        /// </summary>
        string Flag = "3";

        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmPmpaViewAllRoomDeptSearch()
        {
            InitializeComponent();
        }

        private void frmPmpaViewAllRoomDeptSearch_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            if (Flag == "1")
            {
                ssView_Sheet1.Columns[0].Visible = true;
                ssView_Sheet1.Columns[1].Visible = true;
                ssView_Sheet1.Columns[2].Visible = true;
                ssView_Sheet1.Columns[3].Visible = true;
                ssView_Sheet1.Columns[4].Visible = true;
                ssView_Sheet1.Columns[5].Visible = true;
                ssView_Sheet1.Columns[6].Visible = true;
                ssView_Sheet1.Columns[7].Visible = true;
                ssView_Sheet1.Columns[8].Visible = true;
                ssView_Sheet1.Columns[9].Visible = true;

                panTransfor.Visible = true;
                panTransfor.Dock = DockStyle.Left;
                panDateTrans.Visible = false;
                panDateTrans.Dock = DockStyle.None;

                dtpFDate.Value = Convert.ToDateTime(strDTP);
                dtpTDate.Value = Convert.ToDateTime(strDTP);
            }
            else if (Flag == "2")
            {
                ssView.Sheets[0].ColumnHeader.Cells[0, 0].Text = "등록번호";
                ssView.Sheets[0].Columns[0].Width = 100;
                ssView.Sheets[0].ColumnHeader.Cells[0, 1].Text = "성명";
                ssView.Sheets[0].Columns[1].Width = 80;
                ssView.Sheets[0].ColumnHeader.Cells[0, 2].Text = "종류";
                ssView.Sheets[0].Columns[2].Width = 60;
                ssView.Sheets[0].ColumnHeader.Cells[0, 3].Text = "입원일자";
                ssView.Sheets[0].Columns[3].Width = 100;
                ssView.Sheets[0].ColumnHeader.Cells[0, 4].Text = "이실시간";
                ssView.Sheets[0].Columns[4].Width = 100;
                ssView.Sheets[0].ColumnHeader.Cells[0, 5].Text = "이실전";
                ssView.Sheets[0].Columns[5].Width = 100;
                ssView.Sheets[0].ColumnHeader.Cells[0, 6].Text = "이실후";
                ssView.Sheets[0].Columns[6].Width = 100;

                CS.setColAlign(ssView, 5, clsSpread.HAlign_L, clsSpread.VAlign_C);
                CS.setColAlign(ssView, 6, clsSpread.HAlign_L, clsSpread.VAlign_C);

                ssView_Sheet1.Columns[7].Visible = false;
                ssView_Sheet1.Columns[8].Visible = false;
                ssView_Sheet1.Columns[9].Visible = false;
                panTransfor.Visible = false;
                panTransfor.Dock = DockStyle.None;
                panDateTrans.Visible = true;
                panDateTrans.Dock = DockStyle.Left;

                dtpJobFDate.Value = Convert.ToDateTime(strDTP);
                dtpJobTDate.Value = Convert.ToDateTime(strDTP);
            }

            else if (Flag == "3")
            {
                ssView.Sheets[0].ColumnHeader.Cells[0, 0].Text = "등록번호";
                ssView.Sheets[0].Columns[0].Width = 100;
                ssView.Sheets[0].ColumnHeader.Cells[0, 1].Text = "성명";
                ssView.Sheets[0].Columns[1].Width = 80;
                ssView.Sheets[0].ColumnHeader.Cells[0, 2].Text = "종류";
                ssView.Sheets[0].Columns[2].Width = 60;
                ssView.Sheets[0].ColumnHeader.Cells[0, 3].Text = "입원일자";
                ssView.Sheets[0].Columns[3].Width = 100;
                ssView.Sheets[0].ColumnHeader.Cells[0, 4].Text = "이실전";
                ssView.Sheets[0].Columns[4].Width = 100;
                ssView.Sheets[0].ColumnHeader.Cells[0, 5].Text = "이실후";
                ssView.Sheets[0].Columns[5].Width = 100;
                ssView.Sheets[0].ColumnHeader.Cells[0, 6].Text = "비고";
                ssView.Sheets[0].Columns[6].Width = 0;
                ssView.Sheets[0].ColumnHeader.Cells[0, 7].Text = "";
                ssView.Sheets[0].Columns[7].Width = 0;

                ssView_Sheet1.Columns[7].Visible = false;
                ssView_Sheet1.Columns[8].Visible = false;
                ssView_Sheet1.Columns[9].Visible = false;

                CS.setColAlign(ssView, 4, clsSpread.HAlign_L, clsSpread.VAlign_C);
                CS.setColAlign(ssView, 5, clsSpread.HAlign_L, clsSpread.VAlign_C);
                
                panTransfor.Visible = false;
                panTransfor.Dock = DockStyle.None;
                panDateTrans.Visible = true;
                panDateTrans.Dock = DockStyle.Left;
                panpanDateTrans1.Visible = false;
                panpanDateTrans2.Visible = true;
                panpanDateTrans2.Dock = DockStyle.Left;

                dtpJobFDate.Value = Convert.ToDateTime(strDTP);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string SQL = "";    //Query문

            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;
            try
            {

                if (Flag == "1")
                {
                    Flag1_btnSearch();
                }

                if (Flag == "2")
                {
                    Flag2_btnSearch();
                }

                if (Flag == "3")
                {
                    Flag3_btnSearch();
                }

                ssView.ActiveSheet.Columns[0].Width = ssView.ActiveSheet.GetPreferredColumnWidth(0) + 8;
                ssView.ActiveSheet.Columns[1].Width = ssView.ActiveSheet.GetPreferredColumnWidth(1) + 8;
                ssView.ActiveSheet.Columns[2].Width = ssView.ActiveSheet.GetPreferredColumnWidth(2) + 14;
                ssView.ActiveSheet.Columns[3].Width = ssView.ActiveSheet.GetPreferredColumnWidth(3) + 8;
                ssView.ActiveSheet.Columns[4].Width = ssView.ActiveSheet.GetPreferredColumnWidth(4) + 8;
                ssView.ActiveSheet.Columns[5].Width = ssView.ActiveSheet.GetPreferredColumnWidth(5) + 8;

                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;
            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void Flag1_btnSearch()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssView_Sheet1.RowCount = 0;


            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(TRSDATE,'YYYY-MM-DD') TRSDATE, PANO, SNAME, ";
            SQL = SQL + ComNum.VBLF + " FRWARD, FRROOM, TOWARD, TOROOM, FRDEPT, TODEPT ";
            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_TRANSFOR ";
            SQL = SQL + ComNum.VBLF + "  WHERE 1=1";
            SQL = SQL + ComNum.VBLF + "    AND TRSDATE >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "    AND TRSDATE < TO_DATE('" + dtpTDate.Value.AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";//'TRSDATE 시간분표시

            if (rdoGB1.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "   AND (FRWARD <> TOWARD OR FRROOM <> TOROOM )";
            }
            if (rdoGB2.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "   AND (FRDEPT <> TODEPT)";
            }

            SQL = SQL + ComNum.VBLF + " ORDER BY TRSDATE , PANO ";

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

            //스프레드 출력문
            ssView_Sheet1.RowCount = dt.Rows.Count;
            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssView_Sheet1.Cells[i, 1 - 1].Text = dt.Rows[i]["TRSDATE"].ToString().Trim();
                ssView_Sheet1.Cells[i, 2 - 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                ssView_Sheet1.Cells[i, 3 - 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                ssView_Sheet1.Cells[i, 4 - 1].Text = dt.Rows[i]["FRWARD"].ToString().Trim();
                ssView_Sheet1.Cells[i, 5 - 1].Text = dt.Rows[i]["FRROOM"].ToString().Trim();
                ssView_Sheet1.Cells[i, 6 - 1].Text = dt.Rows[i]["TOWARD"].ToString().Trim();
                ssView_Sheet1.Cells[i, 7 - 1].Text = dt.Rows[i]["TOROOM"].ToString().Trim();
                ssView_Sheet1.Cells[i, 8 - 1].Text = dt.Rows[i]["FRDEPT"].ToString().Trim();
                ssView_Sheet1.Cells[i, 9 - 1].Text = dt.Rows[i]["TODEPT"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;

        }

        private void Flag2_btnSearch()
        {
            int i = 0;
            string strPANO = "";
            string strBi = "";
            string strSNAME = "";
            string strInDate = "";
            string strTrstime = "";
            string strFrRoom = "";
            string strToRoom = "";
            string strList = "";
            DataTable dt = null;
            DataTable dtFc = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT Pano,FrWard,FrRoom,FrDept,ToWard,ToRoom,ToDept,TO_CHAR(TRSDATE,'HH24:MI:SS') trstime  ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_TRANSFOR  ";
            SQL = SQL + ComNum.VBLF + "WHERE 1=1";
            SQL = SQL + ComNum.VBLF + "  AND TrsDate >= TO_DATE('" + dtpJobFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')  ";
            SQL = SQL + ComNum.VBLF + "  AND TrsDate < TO_DATE('" + dtpJobTDate.Value.AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')  ";

            if (clsPublic.GstrWardCodes == "TOP" || clsType.User.PassId == "04349" || clsType.User.PassId == "20433" || clsType.User.PassId == "20442" || clsType.User.PassId == "20193" || clsType.User.PassId == "04444")
            {
            }
            else
            {
                if (clsPublic.GstrWardCodes == "SICU" || clsPublic.GstrWardCodes == "MICU")
                {
                    SQL = SQL + ComNum.VBLF + "  AND (FrWard = 'IU' ";
                    SQL = SQL + ComNum.VBLF + "   OR  ToWard = 'IU' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND (FrWard = '" + clsPublic.GstrWardCodes + "' ";
                    SQL = SQL + ComNum.VBLF + "   OR  ToWard = '" + clsPublic.GstrWardCodes + "' ";
                }

                if (clsPublic.GstrWardCodes == "NR")
                {
                    SQL = SQL + ComNum.VBLF + "OR  FrWard = 'IQ'  ";
                    SQL = SQL + ComNum.VBLF + "OR  ToWard = 'IQ'  ";
                }
                SQL = SQL + ComNum.VBLF + ")  ";
            }

            SQL = SQL + ComNum.VBLF + " UNION ALL";
            SQL = SQL + ComNum.VBLF + " SELECT PANO, 'HD' FRWARD , 500 FRROOM, 'HD' FRDEPT, WARDCODE TOWARD, ROOMCODE TORROM, DEPTCODE TODEPT, TO_CHAR(INDATE, 'HH24:MI:SS') TRSTIME";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER";
            SQL = SQL + ComNum.VBLF + " WHERE INDATE >= TO_DATE('" + dtpJobFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')  ";
            SQL = SQL + ComNum.VBLF + " AND  INDATE <= TO_DATE('" + dtpJobTDate.Value.AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')  ";
            SQL = SQL + ComNum.VBLF + " AND PANO IN (";
            SQL = SQL + ComNum.VBLF + " SELECT PANO FROM KOSMOS_PMPA.DIET_NEWORDER";
            SQL = SQL + ComNum.VBLF + " WHERE ACTDATE = TO_DATE('" + dtpJobFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')  ";
            SQL = SQL + ComNum.VBLF + " AND WARDCODE = 'HD')";

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
                ComFunc.MsgBox("전실,전과 자료가 1건도 없습니다.");
                return;
            }

            //스프레드 출력문
            ssView_Sheet1.RowCount = dt.Rows.Count;
            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (i = 0; i < dt.Rows.Count; i++)
            {
                strPANO = Convert.ToInt32(dt.Rows[i]["Pano"].ToString().Trim()).ToString("00000000");
                SQL = " SELECT Sname,Bi,TO_CHAR(InDate,'YYyy-mm-dd') InDate  ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER  ";// '＃원본윗줄
                SQL = SQL + ComNum.VBLF + "WHERE Pano = '" + strPANO + "' ";
                SQL = SQL + ComNum.VBLF + "  AND GBSTS = '0' ";

                SqlErr = clsDB.GetDataTable(ref dtFc, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dtFc.Rows.Count == 0)
                {
                    strSNAME = dtFc.Rows[0]["Sname"].ToString().Trim();
                    strBi = dtFc.Rows[0]["Bi"].ToString().Trim();
                    strInDate = dtFc.Rows[0]["InDate"].ToString().Trim();
                }
                else
                {
                    strSNAME = "<퇴원자>";
                    strBi = "";
                    strInDate = "";
                }

                dtFc.Dispose();
                dtFc = null;

                strPANO = ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1 - 1].Text;
                strSNAME = ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2 - 1].Text;
                strBi = ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3 - 1].Text;
                strInDate = ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4 - 1].Text;

                strFrRoom = dt.Rows[i]["FrRoom"].ToString().Trim();
                strTrstime = dt.Rows[i]["trstime"].ToString().Trim();

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5 - 1].Text = strTrstime;
                strList = dt.Rows[i]["FrWard"].ToString().Trim() + ",";
                strList = strList + strFrRoom + ",";
                strList = strList + dt.Rows[i]["FrDept"].ToString().Trim() + ",";
                strList = strList + RoomClassSet(strFrRoom);
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6 - 1].Text = strList;

                strToRoom = dt.Rows[i]["ToRoom"].ToString().Trim();
                strList = dt.Rows[i]["ToWard"].ToString().Trim() + ",";
                strList = strList + strToRoom + ",";
                strList = strList + dt.Rows[i]["ToDept"].ToString().Trim() + ",";
                strList = strList + RoomClassSet(strToRoom);
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7 - 1].Text = strList;

            }
            dt.Dispose();
            dt = null;

        }

        private string RoomClassSet(string RoomCode)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strRoomClass = "";
            string strOverAmt = "";
            string strRtn = "";

            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;
            try
            {
                SQL = "";
                SQL = " SELECT RoomClass,OverAmt ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_ROOM   ";
                SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                SQL = SQL + ComNum.VBLF + "   AND RoomCOde = '" + RoomCode + "'  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return strRtn;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("<등급오류>");
                    Cursor.Current = Cursors.Default;
                    return strRtn;
                }

                strRoomClass = dt.Rows[0]["RoomClass"].ToString().Trim();
                strOverAmt = Convert.ToDouble(dt.Rows[0]["OverAmt"].ToString().Trim()).ToString("###,###,###");

                switch (strRoomClass)
                {
                    case "A":
                        strRtn = "특AA";
                        break;
                    case "B":
                        strRtn = "특AB";
                        break;
                    case "C":
                        strRtn = "특BA";
                        break;
                    case "D":
                        strRtn = "특BB";
                        break;
                    case "E":
                        strRtn = "특CA";
                        break;
                    case "F":
                        strRtn = "특CB";
                        break;
                    case "G":
                        strRtn = "2인실A";
                        break;
                    case "H":
                        strRtn = "2인실B";
                        break;
                    case "I":
                        strRtn = "3인실A";
                        break;
                    case "J":
                        strRtn = "3인실B";
                        break;
                    case "K":
                    case "L":
                    case "M":
                    case "N":
                    case "O":
                    case "P":
                    case "Q":
                    case "R":
                        strRtn = "일반병실";
                        break;
                    case "T":
                        strRtn = "격리실";
                        break;
                    case "U":
                        strRtn = "중환자실";
                        break;
                    case "V":
                        strRtn = "분만실";
                        break;
                    case "W":
                        strRtn = "신생아실";
                        break;
                    case "X":
                        strRtn = "인큐베타";
                        break;
                    case "Y":
                        strRtn = "화상치료실";
                        break;
                    case "Z":
                        strRtn = "임신중독실";
                        break;
                    default:
                        strRtn = "기타";
                        break;
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;
                return strRtn;

            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return strRtn;
            }
        }

        private void Flag3_btnSearch()
        {
            int i = 0;
            string strPano = "";
            string strBi = "";
            string strSname = "";
            string strInDate = "";
            string strFrRoom = "";
            string strToRoom = "";
            string strList = "";
            double nIPDNO = 0;
            DataTable dt = null;
            DataTable dtFc = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT Pano,FrWard,FrRoom,FrDept,ToWard,ToRoom,ToDept,IPDNO                      ";
            SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_TRANSFOR                                        ";
            SQL = SQL + ComNum.VBLF + "  WHERE 1=1";
            SQL = SQL + ComNum.VBLF + "    AND TrsDate >= TO_DATE('" + dtpJobFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')                 ";
            SQL = SQL + ComNum.VBLF + "    AND TrsDate < TO_DATE('" + dtpJobFDate.Value.AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')   ";

            if (rdoSort0.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "  ORDER BY Pano,ToRoom                                                          ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "  ORDER BY ToWard,ToRoom                                                        ";
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
                ComFunc.MsgBox("전실,전과 자료가 1건도 없습니다.");
                return;
            }

            //스프레드 출력문
            ssView_Sheet1.RowCount = dt.Rows.Count;
            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (i = 0; i < dt.Rows.Count; i++)
            {
                strPano = Convert.ToInt32(dt.Rows[i]["Pano"].ToString().Trim()).ToString("00000000");
                nIPDNO = Convert.ToDouble(dt.Rows[i]["IPDNO"].ToString().Trim());

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Sname,Bi,TO_CHAR(InDate,'YYYY-MM-DD') InDate   ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER           ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1=1                                             ";
                SQL = SQL + ComNum.VBLF + "    AND Pano = '" + strPano + "'                       ";
                SQL = SQL + ComNum.VBLF + "    AND IPDNO = " + nIPDNO + "                         ";
                SQL = SQL + ComNum.VBLF + "    AND AmSet6 != '*'                                  ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY INDATE DESC";

                SqlErr = clsDB.GetDataTable(ref dtFc, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                }
                if (dt.Rows.Count > 0)
                {
                    strSname = dtFc.Rows[0]["SNAME"].ToString().Trim();
                    strBi = dtFc.Rows[0]["Bi"].ToString().Trim();
                    strInDate = dtFc.Rows[0]["Indate"].ToString().Trim();
                }
                else
                {
                    strSname = "";
                    strBi = "";
                    strInDate = "";
                }
                dtFc.Dispose();
                dtFc = null;

                ssView_Sheet1.Cells[i, 1 - 1].Text = strPano.Trim();
                ssView_Sheet1.Cells[i, 2 - 1].Text = (strSname).Trim();
                ssView_Sheet1.Cells[i, 3 - 1].Text = (strBi).Trim();
                ssView_Sheet1.Cells[i, 4 - 1].Text = (strInDate).Trim();

                strFrRoom = dt.Rows[i]["FrRoom"].ToString().Trim();
                if (strFrRoom == "510")
                {
                    strFrRoom = strFrRoom;
                }

                strList = dt.Rows[i]["FrWard"].ToString().Trim() + ",";
                strList = strList + strFrRoom + ",";
                strList = strList + dt.Rows[i]["FrDept"].ToString().Trim();
                strList = strList + RoomClassSetNew(strFrRoom);
                ssView_Sheet1.Cells[i, 5 - 1].Text = " " + strList;
                strToRoom = dt.Rows[i]["ToRoom"].ToString().Trim();
                strList = dt.Rows[i]["ToWard"].ToString().Trim() + ",";
                strList = strList + strToRoom + ",";
                strList = strList + dt.Rows[i]["ToDept"].ToString().Trim();
                strList = strList + RoomClassSetNew(strToRoom);
                ssView_Sheet1.Cells[i, 6 - 1].Text = " " + strList;
                ssView_Sheet1.Cells[i, 7 - 1].Text = " ";
            }

            dt.Dispose();
            dt = null;
            
        }

        private string RoomClassSetNew(string RoomCode)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strRoomClass = "";
            string strOverAmt = "";
            string strRtn = "";
            
            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;
            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(TRANSDATE1,'YYYY-MM-DD') TRDATE1,OVERAMT,TBED,        ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(TRANSDATE2,'YYYY-MM-DD') TRDATE2,OVERAMT1,TBED1,      ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(TRANSDATE3,'YYYY-MM-DD') TRDATE3,OVERAMT2,TBED2,      ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(TRANSDATE4,'YYYY-MM-DD') TRDATE4,OVERAMT3,TBED3,      ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(TRANSDATE5,'YYYY-MM-DD') TRDATE5,OVERAMT4,TBED4,      ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(TRANSDATE6,'YYYY-MM-DD') TRDATE6,OVERAMT5,TBED5,      ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(TRANSDATE7,'YYYY-MM-DD') TRDATE7,OVERAMT6,TBED6,      ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(TRANSDATE8,'YYYY-MM-DD') TRDATE8,OVERAMT7,TBED7,      ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(TRANSDATE9,'YYYY-MM-DD') TRDATE9,OVERAMT8,TBED8,      ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(TRANSDATE10,'YYYY-MM-DD') TRDATE10,OVERAMT8,TBED9     ";
                SQL = SQL + ComNum.VBLF + " FROM BAS_ROOM                                                  ";
                SQL = SQL + ComNum.VBLF + " WHERE ROOMCODE = '" + RoomCode + "'                           ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return strRtn;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("<등급오류>");
                    Cursor.Current = Cursors.Default;
                    return strRtn;
                }
                double nSSOverAmt = 0;
                if ((VB.Val(dt.Rows[0]["OVERAMT"].ToString().Trim()) != 0) && string.Compare(dtpJobFDate.Value.ToString("yyyy-MM-dd"), "2019-07-01") >= 0)
                {
                    nSSOverAmt = 60000;
                }

                if (string.Compare(dt.Rows[0]["TRDATE1"].ToString().Trim(), dtpJobFDate.Value.ToString("yyyy-MM-dd")) <= 0)
                {
                    if (VB.Val(dt.Rows[0]["OVERAMT"].ToString().Trim()) != 0 || VB.Val(dt.Rows[0]["TBED"].ToString().Trim()) == 2 ) 
                    {
                        nSSOverAmt = nSSOverAmt + VB.Val(dt.Rows[0]["OVERAMT"].ToString().Trim()) ;
                        strRtn = ",(" + nSSOverAmt.ToString("###,###,###") + " [" +VB.Val(dt.Rows[0]["TBED"].ToString().Trim()) + "인실])" ;
                    }
                }

                else if (string.Compare(dt.Rows[0]["TRDATE2"].ToString().Trim(), dtpJobFDate.Value.ToString("yyyy-MM-dd")) <= 0)
                {
                    if (VB.Val(dt.Rows[0]["OVERAMT1"].ToString().Trim()) != 0)
                    {
                        strRtn = ",(" + VB.Val(dt.Rows[0]["OVERAMT1"].ToString().Trim()).ToString("###,###,###") + ")";
                    }
                }
                else if (string.Compare(dt.Rows[0]["TRDATE3"].ToString().Trim(), dtpJobFDate.Value.ToString("yyyy-MM-dd")) <= 0)
                {
                    if (VB.Val(dt.Rows[0]["OVERAMT2"].ToString().Trim()) != 0)
                    {
                        strRtn = ",(" + VB.Val(dt.Rows[0]["OVERAMT2"].ToString().Trim()).ToString("###,###,###") + ")";
                    }
                }
                else if (string.Compare(dt.Rows[0]["TRDATE4"].ToString().Trim(), dtpJobFDate.Value.ToString("yyyy-MM-dd")) <= 0)
                {
                    if (VB.Val(dt.Rows[0]["OVERAMT3"].ToString().Trim()) != 0)
                    {
                        strRtn = ",(" + VB.Val(dt.Rows[0]["OVERAMT3"].ToString().Trim()).ToString("###,###,###") + ")";
                    }
                }
                else if (string.Compare(dt.Rows[0]["TRDATE5"].ToString().Trim(), dtpJobFDate.Value.ToString("yyyy-MM-dd")) <= 0)
                {
                    if (VB.Val(dt.Rows[0]["OVERAMT4"].ToString().Trim()) != 0)
                    {
                        strRtn = ",(" + VB.Val(dt.Rows[0]["OVERAMT4"].ToString().Trim()).ToString("###,###,###") + ")";
                    }
                }
                else if (string.Compare(dt.Rows[0]["TRDATE6"].ToString().Trim(), dtpJobFDate.Value.ToString("yyyy-MM-dd")) <= 0)
                {
                    if (VB.Val(dt.Rows[0]["OVERAMT5"].ToString().Trim()) != 0)
                    {
                        strRtn = ",(" + VB.Val(dt.Rows[0]["OVERAMT5"].ToString().Trim()).ToString("###,###,###") + ")";
                    }
                }
                else if (string.Compare(dt.Rows[0]["TRDATE7"].ToString().Trim(), dtpJobFDate.Value.ToString("yyyy-MM-dd")) <= 0)
                {
                    if (VB.Val(dt.Rows[0]["OVERAMT6"].ToString().Trim()) != 0)
                    {
                        strRtn = ",(" + VB.Val(dt.Rows[0]["OVERAMT6"].ToString().Trim()).ToString("###,###,###") + ")";
                    }
                }
                else if (string.Compare(dt.Rows[0]["TRDATE8"].ToString().Trim(), dtpJobFDate.Value.ToString("yyyy-MM-dd")) <= 0)
                {
                    if (VB.Val(dt.Rows[0]["OVERAMT7"].ToString().Trim()) != 0)
                    {
                        strRtn = ",(" + VB.Val(dt.Rows[0]["OVERAMT7"].ToString().Trim()).ToString("###,###,###") + ")";
                    }
                }
                else if (string.Compare(dt.Rows[0]["TRDATE9"].ToString().Trim(), dtpJobFDate.Value.ToString("yyyy-MM-dd")) <= 0)
                {
                    if (VB.Val(dt.Rows[0]["OVERAMT8"].ToString().Trim()) != 0)
                    {
                        strRtn = ",(" + VB.Val(dt.Rows[0]["OVERAMT8"].ToString().Trim()).ToString("###,###,###") + ")";
                    }
                }
                else if (string.Compare(dt.Rows[0]["TRDATE10"].ToString().Trim(), dtpJobFDate.Value.ToString("yyyy-MM-dd")) <= 0)
                {
                    if (VB.Val(dt.Rows[0]["OVERAMT9"].ToString().Trim()) != 0)
                    {
                        strRtn = ",(" + VB.Val(dt.Rows[0]["OVERAMT9"].ToString().Trim()).ToString("###,###,###") + ")";
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;
                return strRtn;

            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return strRtn;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (Flag == "1")
            {
                Flag1_btnPrint();
            }

            if (Flag == "2")
            {
                Flag2_btnPrint();
            }

            if (Flag == "3")
            {
                Flag3_btnPrint();
            }
        }


        private void Flag1_btnPrint()
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }     //권한확인

            strTitle = "일 자 별  전실 전과 내역";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("조회일자 : " + dtpFDate.Value.ToString("yyyy-MM-dd") + " ~ " + dtpTDate.Value.ToString("yyyy-MM-dd"), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            if (rdoGB0.Checked == true)
            {
                strHeader += CS.setSpdPrint_String("전실, 전과", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }
            if (rdoGB1.Checked == true)
            {
                strHeader += CS.setSpdPrint_String("전실 ", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }
            if (rdoGB2.Checked == true)
            {
                strHeader += CS.setSpdPrint_String("전과 ", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }


            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }
        private void Flag2_btnPrint()
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }     //권한확인

            strTitle = dtpJobFDate.Value.ToString("yyyy-MM-dd") + ")전실,전과 명부";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }
        private void Flag3_btnPrint()
        {
            string strTitle = "";
            string strTitle2 = "";
            string strHeader = "";
            string strHeader2 = "";
            string strHeader3 = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }     //권한확인

            strTitle = "전실,전과 명부";

            strTitle2 = "";
            strTitle2 += "┌--┬--------┬--------┬--------┐" + ComNum.VBLF;
            strTitle2 += "│결│  담 당 │원무행정│ 팀  장 │" + ComNum.VBLF;
            strTitle2 += "│  ├--------┼--------┼--------┤" + ComNum.VBLF;
            strTitle2 += "│  │        │        │        │" + ComNum.VBLF;
            strTitle2 += "│  │        │        │        │" + ComNum.VBLF;
            strTitle2 += "│재│        │        │        │" + ComNum.VBLF;
            strTitle2 += "└--┴--------┴--------┴--------┘" + ComNum.VBLF; 

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader2 = CS.setSpdPrint_String(strTitle2, new Font("굴림체", 10, FontStyle.Regular), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader3 = CS.setSpdPrint_String("발생일자:" + dtpJobFDate.Text , new Font("굴림체", 10, FontStyle.Regular), clsSpread.enmSpdHAlign.Left, false, true);
            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader + strHeader2 + strHeader3, strFooter);
        }

        private void ssView_CellClick(object sender, CellClickEventArgs e)
        {
            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssView_Sheet1.Cells[e.Row, 0, e.Row, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
