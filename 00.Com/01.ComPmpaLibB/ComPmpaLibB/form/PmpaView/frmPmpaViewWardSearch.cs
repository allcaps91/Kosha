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
    /// File Name       : frmPmpaViewWardSearch.cs
    /// Description     : 병실조회
    /// Author          : 박창욱
    /// Create Date     : 2017-11-09
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\OPD\wonmok\wonmok01.frm(FrmWonMok01.frm) >> frmPmpaViewWardSearch.cs 폼이름 재정의" />
    public partial class frmPmpaViewWardSearch : Form
    {
        string[] strWardCode = new string[51];
        string[] strBis = new string[56];

        public frmPmpaViewWardSearch()
        {
            InitializeComponent();
        }

        void Load_Bi_IDs()
        {
            strBis[11] = "공단";
            strBis[12] = "직장";
            strBis[13] = "지역";
            strBis[14] = "";
            strBis[15] = "";
            strBis[21] = "보호1";
            strBis[22] = "보호2";
            strBis[23] = "보호3";
            strBis[24] = "행려";
            strBis[25] = "";
            strBis[31] = "산재";
            strBis[32] = "공상";
            strBis[33] = "산재공상";
            strBis[34] = "";
            strBis[35] = "";
            strBis[41] = "공단100%";
            strBis[42] = "직장100%";
            strBis[43] = "지역100%";
            strBis[44] = "가족계획";
            strBis[45] = "보험계약";
            strBis[51] = "일반";
            strBis[52] = "TA 보험";
            strBis[53] = "계약";
            strBis[54] = "미확인";
            strBis[55] = "TA 일반";
        }

        void List_Event()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strBi = "";
            string strDcdate = "";
            string strSysDate = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT M.ROOMCODE, Pano, Bi,";
                SQL = SQL + ComNum.VBLF + "       Sname, Pname, Sex,";
                SQL = SQL + ComNum.VBLF + "       Age, TO_CHAR(InDate, 'yyyy-mm-dd') InDate, DeptCode,";
                SQL = SQL + ComNum.VBLF + "       DrName, Amset1, Sysdate,";
                SQL = SQL + ComNum.VBLF + "       Religion ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER M, " + ComNum.DB_PMPA + "BAS_DOCTOR D";
                SQL = SQL + ComNum.VBLF + " WHERE WardCode = '" + strWardCode[ssList1_Sheet1.ActiveRowIndex] + "'";
                SQL = SQL + ComNum.VBLF + "   AND M.RoomCode = " + VB.Mid(ssList2_Sheet1.ActiveCell.Text, 1, 3);
                SQL = SQL + ComNum.VBLF + "   AND M.OUTDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "   AND M.GBSTS  = '0' ";
                SQL = SQL + ComNum.VBLF + "   AND M.DrCode = D.DrCode";
                if (rdoBun1.Checked == true)  //천주교
                {
                    SQL = SQL + ComNum.VBLF + "   AND Religion = '1' ";
                }
                else if (rdoBun2.Checked == true) //불교
                {
                    SQL = SQL + ComNum.VBLF + "   AND Religion = '3' ";
                }
                else if (rdoBun3.Checked == true) //개신교
                {
                    SQL = SQL + ComNum.VBLF + "   AND Religion = '2' ";
                }
                else if (rdoBun4.Checked == true) //무교
                {
                    SQL = SQL + ComNum.VBLF + "   AND Religion NOT IN ('1','2','3') ";
                }
                SQL = SQL + ComNum.VBLF + " ORDER BY M.ROOMCODE ASC, Sname";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;

                if (dt.Rows.Count > 20)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strBi = dt.Rows[i]["Bi"].ToString().Trim();
                    strSysDate = dt.Rows[i]["Sysdate"].ToString().Trim();

                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Sname"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = strBis[Convert.ToInt32(strBi)];
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Sex"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Age"].ToString().Trim();

                    switch (dt.Rows[i]["Religion"].ToString().Trim())
                    {
                        case "1":
                            ssView_Sheet1.Cells[i, 6].Text = "1.천주교";
                            break;
                        case "2":
                            ssView_Sheet1.Cells[i, 6].Text = "2.개신교";
                            break;
                        case "3":
                            ssView_Sheet1.Cells[i, 6].Text = "3.불  교";
                            break;
                        case "4":
                            ssView_Sheet1.Cells[i, 6].Text = "4.천도교";
                            break;
                        case "5":
                            ssView_Sheet1.Cells[i, 6].Text = "5.유  교";
                            break;
                        default:
                            ssView_Sheet1.Cells[i, 6].Text = "9.기  타";
                            break;
                    }

                    ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["InDate"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["DrName"].ToString().Trim();
                    switch (dt.Rows[i]["Amset1"].ToString().Trim())
                    {
                        case "0":
                            ssView_Sheet1.Cells[i, 10].Text = " ";
                            break;
                        case "1":
                            ssView_Sheet1.Cells[i, 10].Text = "수납";
                            break;
                        case "2":
                            ssView_Sheet1.Cells[i, 10].Text = "계산중";
                            break;
                        case "3":
                            if (string.Compare(strDcdate, strSysDate) < 0)
                            {
                                ssView_Sheet1.Cells[i, 10].Text = "보관금퇴원";
                            }
                            else
                            {
                                ssView_Sheet1.Cells[i, 10].Text = "가퇴원";
                            }
                            break;
                    }
                }
                Cursor.Current = Cursors.Default;

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void List_Event1()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strBi = "";
            string strDcdate = "";
            string strSysDate = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT M.ROOMCODE, Pano, Bi,";
                SQL = SQL + ComNum.VBLF + "       Sname, Pname, Sex,";
                SQL = SQL + ComNum.VBLF + "       Age, TO_CHAR(InDate, 'yyyy-mm-dd') InDate, DeptCode,";
                SQL = SQL + ComNum.VBLF + "       DrName, Amset1, Sysdate,";
                SQL = SQL + ComNum.VBLF + "       Religion ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER M, " + ComNum.DB_PMPA + "BAS_DOCTOR D";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND WardCode = '" + strWardCode[ssList1_Sheet1.ActiveRowIndex] + "'";
                SQL = SQL + ComNum.VBLF + "   AND M.OUTDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "   AND Amset6 != '*' ";
                SQL = SQL + ComNum.VBLF + "   AND M.DrCode = D.DrCode";
                if (rdoBun1.Checked == true)  //천주교
                {
                    SQL = SQL + ComNum.VBLF + "   AND Religion = '1' ";
                }
                else if (rdoBun2.Checked == true) //불교
                {
                    SQL = SQL + ComNum.VBLF + "   AND Religion = '3' ";
                }
                else if (rdoBun3.Checked == true) //개신교
                {
                    SQL = SQL + ComNum.VBLF + "   AND Religion = '2' ";
                }
                else if (rdoBun4.Checked == true) //무교
                {
                    SQL = SQL + ComNum.VBLF + "   AND Religion NOT IN ('1','2','3') ";
                }
                SQL = SQL + ComNum.VBLF + " ORDER BY M.ROOMCODE ASC, Sname";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;

                if (dt.Rows.Count > 20)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strBi = dt.Rows[i]["Bi"].ToString().Trim();
                    strSysDate = dt.Rows[i]["Sysdate"].ToString().Trim();

                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Sname"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = strBis[Convert.ToInt32(strBi)];
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Sex"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Age"].ToString().Trim();

                    switch (dt.Rows[i]["Religion"].ToString().Trim())
                    {
                        case "1":
                            ssView_Sheet1.Cells[i, 6].Text = "1.천주교";
                            break;
                        case "2":
                            ssView_Sheet1.Cells[i, 6].Text = "2.개신교";
                            break;
                        case "3":
                            ssView_Sheet1.Cells[i, 6].Text = "3.불  교";
                            break;
                        case "4":
                            ssView_Sheet1.Cells[i, 6].Text = "4.천도교";
                            break;
                        case "5":
                            ssView_Sheet1.Cells[i, 6].Text = "5.유  교";
                            break;
                        default:
                            ssView_Sheet1.Cells[i, 6].Text = "9.기  타";
                            break;
                    }

                    ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["InDate"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["DrName"].ToString().Trim();
                    switch (dt.Rows[i]["Amset1"].ToString().Trim())
                    {
                        case "0":
                            ssView_Sheet1.Cells[i, 10].Text = " ";
                            break;
                        case "1":
                            ssView_Sheet1.Cells[i, 10].Text = "수납";
                            break;
                        case "2":
                            ssView_Sheet1.Cells[i, 10].Text = "계산중";
                            break;
                        case "3":
                            if (string.Compare(strDcdate, strSysDate) < 0)
                            {
                                ssView_Sheet1.Cells[i, 10].Text = "보관금퇴원";
                            }
                            else
                            {
                                ssView_Sheet1.Cells[i, 10].Text = "가퇴원";
                            }
                            break;
                    }
                }
                Cursor.Current = Cursors.Default;

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void Room_Set()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strRoom = "";
            string strClass = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT RoomCode, TBed";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_ROOM ";
                SQL = SQL + ComNum.VBLF + " WHERE WardCode = '" + strWardCode[ssList1_Sheet1.ActiveRowIndex] + "'";
                SQL = SQL + ComNum.VBLF + " ORDER BY ROOMCODE ASC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                ssList2_Sheet1.RowCount = 0;
                ssList2_Sheet1.RowCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strRoom = dt.Rows[i]["RoomCode"].ToString().Trim();
                    strClass = dt.Rows[i]["TBed"].ToString().Trim();
                    if (strClass != "S")
                    {
                        ssList2_Sheet1.Cells[i, 0].Text = strRoom + "( " + strClass + " 인실 )";
                    }
                    else
                    {
                        ssList2_Sheet1.Cells[i, 0].Text = strRoom + "( 특  실 )";
                    }
                }

                ssList2.Focus();

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void SS_Room_Clear()
        {
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 20;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "병실조회 LIST";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("조회구분: " + strWardCode[ssList1_Sheet1.ActiveRowIndex] + " 병동", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void frmPmpaViewWardSearch_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Load_Bi_IDs();

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT * ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_WARD";
                SQL = SQL + ComNum.VBLF + " WHERE WardCode > ' '";
                SQL = SQL + ComNum.VBLF + "   AND USED ='Y' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                ssList1_Sheet1.RowCount = 0;
                ssList1_Sheet1.RowCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssList1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["WardName"].ToString().Trim();
                    strWardCode[i] = dt.Rows[i]["WardCode"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

                pan1.Visible = true;
                ssList1.Enabled = true;
                ssList1.Visible = true;

                pan2.Visible = true;
                ssList2.Enabled = true;
                ssList2.Visible = true;

                ssList2_Sheet1.RowCount = 0;

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void ssList1_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (ssList1_Sheet1.RowCount == 0)
            {
                return;
            }

            ssList1_Sheet1.Cells[0, 0, ssList1_Sheet1.RowCount - 1, ssList1_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssList1_Sheet1.Cells[e.Row, 0, e.Row, ssList1_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            SS_Room_Clear();
            Room_Set();
            SS_Room_Clear();

            List_Event1();
        }

        private void ssList2_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (ssList2_Sheet1.RowCount == 0)
            {
                return;
            }

            ssList2_Sheet1.Cells[0, 0, ssList2_Sheet1.RowCount - 1, ssList2_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssList2_Sheet1.Cells[e.Row, 0, e.Row, ssList2_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            SS_Room_Clear();
            List_Event();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
