using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewIUSENT13.cs
    /// Description     : 퇴원예약자조회
    /// Author          : 김효성
    /// Create Date     : 2017-09-06
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// Flag를 사용해서 출력문 조회
    /// Screen_Display2() WORK_IPD_TRANS 존재 하지 않음
    /// 
    /// </history>
    /// <seealso cref= "\psmh\Ocs\ptocs\iupent.vbp(iupent08.frm) >> frmPmpaViewIUSENT13.cs 폼이름 재정의" />	
    /// <seealso cref= "psmh\IPD\ipdSim2\ipdsim2.vbp(DisChargeView.frm) >> frmPmpaViewIUSENT13.cs 폼이름 재정의" />	
    /// <seealso cref= "\psmh\IPD\iument\iusent.vbp(DisChargeView.frm) >> frmPmpaViewIUSENT13.cs 폼이름 재정의" />	
    /// <seealso cref= "\psmh\diet\dietorder\dietorder.vbp(dietorder70.frm) >> frmPmpaViewIUSENT13.cs 폼이름 재정의" />	
    public partial class frmPmpaViewIUSENT13 : Form
    {
        string strdtP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
        ComFunc cf = new ComFunc();

        int Flag = 0; //통합 폼 선택
        string strBi = "";
        string strBiName = "";
        string GstrJobName = "";

        public frmPmpaViewIUSENT13(string trBi, string trBiName, string strJobName)
        {
            GstrJobName = strJobName;
            strBi = trBi;
            strBiName = trBiName;

            InitializeComponent();
        }

        public frmPmpaViewIUSENT13()
        {
            InitializeComponent();
        }

        private void frmPmpaViewIUSENT13_Load(object sender, EventArgs e)
        {

            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{ this.Close(); return; } //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            panSub.Visible = false;

            switch (Flag)
            {
                case 1:
                    break;
                case 2:
                    Screen_Display2();    //로드 , 조회 버튼
                    break;
                case 3:
                    break;
                case 4:
                    panSub.Visible = true;
                    break;
            }

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            switch (Flag)
            {
                case 1:
                    Screen_Display1();
                    break;
                case 2:
                    Screen_Display2();    //로드 , 조회 버튼
                    break;
                case 3:
                    Screen_Display2();    //로드 , 조회 버튼
                    break;
                case 4:
                    panSub.Visible = true;
                    Screen_Display3();    //조회
                    break;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            switch (Flag)
            {
                case 1:
                    Print1();
                    break;
                case 2:
                    Print2();    //로드 , 조회 버튼
                    break;
                case 3:
                    Print2();
                    break;
                case 4:
                    Print3();
                    break;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            panSub.Visible = false;
        }

        #region 퇴원예약자조회 iupent.vbp
        private void Screen_Display1()
        {
            int i = 0;
            int nCount = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            ssView_Sheet1.Columns[11].Visible = false;
            ssView_Sheet1.Columns[14].Visible = false;
            ssView_Sheet1.Columns[15].Visible = false;
            ssView_Sheet1.Columns[16].Visible = false;
            ssView_Sheet1.Columns[17].Visible = false;
            ssView_Sheet1.Columns[18].Visible = false;
            ssView_Sheet1.Columns[19].Visible = false;
            ssView_Sheet1.Columns[10].Visible = false;

            //ssView_Sheet1 . Cells [ 0 , ssView_Sheet1 . ColumnCount - 1 , 0 , ssView_Sheet1 . ColumnCount - 1 ] . Text = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Pano, Sname, SEX, AGE, BI, TO_CHAR(InDate,'YYYY-MM-DD') InDate, ";
                SQL = SQL + ComNum.VBLF + "     ILSU, GBSTS, DEPTCODE, DRCODE, WARDCODE, RoomCode,  ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(RDate,'YYYY-MM-DD HH24:MI') RDate   ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE 1=1 ";
                SQL = SQL + ComNum.VBLF + "   AND OUTDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "   AND RDATE IS NOT NULL";
                SQL = SQL + ComNum.VBLF + "   AND GBSTS NOT IN ('0') ";
                SQL = SQL + ComNum.VBLF + " ORDER BY RDATE Desc, RoomCode ";

                SQL = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nCount = nCount + 1;
                    ssView_Sheet1.RowCount = nCount;
                    ssView_Sheet1.Cells[nCount - 1, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssView_Sheet1.Cells[nCount - 1, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[nCount - 1, 2].Text = dt.Rows[i]["SEX"].ToString().Trim();
                    ssView_Sheet1.Cells[nCount - 1, 3].Text = dt.Rows[i]["AGE"].ToString().Trim();
                    ssView_Sheet1.Cells[nCount - 1, 4].Text = dt.Rows[i]["BI"].ToString().Trim();
                    ssView_Sheet1.Cells[nCount - 1, 5].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[nCount - 1, 6].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[nCount - 1, 7].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[nCount - 1, 9].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[nCount - 1, 10].Text = dt.Rows[i]["RDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[nCount - 1, 12].Text = dt.Rows[i]["ILSU"].ToString().Trim();
                    ssView_Sheet1.Cells[nCount - 1, 13].Text = cf.Read_Bcode_Name(clsDB.DbCon, "IPD_입원상태", dt.Rows[i]["GBSTS"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void Print1()
        {
            Cursor.Current = Cursors.WaitCursor;

            string strHead1 = "";
            string strHead2 = "";
            string strFont1 = "";
            string strFont2 = "";
            string strFoot = "";

            strFont1 = "/c/fn\"굴림체\" /fz\"16\"  /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";

            strFont1 = "/fn\"굴림\" /fz\"18\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";

            strHead1 = "/c/f1" + this.Text + "/f1/n";   //제목 센터
            strHead2 = "/l/f2" + (dtpFdate.Value).ToString("yyyy-MM-dd") + "/f2/n/n";
            strFoot = "/l/f2" + "작성자 : " + GstrJobName + VB.Space(10) + "PAGE : " + "/p";

            ssView_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssView_Sheet1.PrintInfo.ZoomFactor = 0.85f;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2 + "/n";
            ssView_Sheet1.PrintInfo.Footer = strFont2 + strFoot;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 50;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 50;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = false;
            ssView_Sheet1.PrintInfo.ShowShadows = true;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.PrintType = 0;
            ssView.PrintSheet(0);

            btnPrint.Enabled = true;

            Cursor.Current = Cursors.Default;
        }


        #endregion iupent.vbp

        #region 퇴원예약자조회 ipdSim2.vbp , iusent.vbp
        private void Screen_Display2()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int nREAD = 0;
            int nRow = 0;
            string strGbSTS = "";
            string strAmSet3 = "";

            ssView_Sheet1.RowCount = 0;

            ssView_Sheet1.Columns[2].Visible = false;
            ssView_Sheet1.Columns[3].Visible = false;
            ssView_Sheet1.Columns[5].Visible = false;
            ssView_Sheet1.Columns[11].Visible = false;
            ssView_Sheet1.Columns[12].Visible = false;
            ssView_Sheet1.Columns[13].Visible = false;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT a.IPDNO, a.Pano,a.Sname,a.RoomCode,TO_CHAR(a.InDate,'YYYY-MM-DD') InDate, ";
                SQL = SQL + ComNum.VBLF + "        a.GbSTS,TO_CHAR(a.OutDate,'YYYY-MM-DD') OutDate,";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(a.GatewonTime,'YYYY-MM-DD HH24:Mi') GatewonTime,";
                SQL = SQL + ComNum.VBLF + "        b.DeptCode,b.DrCode,TO_CHAR(a.RoutDate,'YYYY-MM-DD HH24:MI') RoutDate,";
                SQL = SQL + ComNum.VBLF + "        a.GbSTS, b.TRSNO,b.Bi,b.GbIPD,b.AmSet3 ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "WORK_IPD_MASTER a," + ComNum.DB_PMPA + "WORK_IPD_TRANS b ";
                SQL = SQL + ComNum.VBLF + "  WHERE a.ActDate  IS NULL ";
                SQL = SQL + ComNum.VBLF + "    AND a.GbSTS IN ('2','3','4','5','6') ";
                SQL = SQL + ComNum.VBLF + "    AND a.IPDNO = b.IPDNO(+) ";
                SQL = SQL + ComNum.VBLF + "    AND b.GbIPD <> 'D' ";// '삭제
                SQL = SQL + ComNum.VBLF + "    AND b.ActDate IS NULL ";
                SQL = SQL + ComNum.VBLF + "    AND b.Amt50 <> 0 ";
                SQL = SQL + ComNum.VBLF + " ORDER BY a.RoutDate,a.RoomCode,a.IPDNO,b.TRSNO ";

                SQL = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["Sname"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["Bi"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["DrCode"].ToString().Trim());
                    ssView_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["InDate"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["OutDate"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 14].Text = "";
                    if (dt.Rows[i]["GbIPD"].ToString().Trim() == "9")
                    {
                        ssView_Sheet1.Cells[nRow - 1, 14].Text = "지병";
                    }
                    ssView_Sheet1.Cells[nRow - 1, 15].Text = dt.Rows[i]["ROutDate"].ToString().Trim();

                    strGbSTS = dt.Rows[i]["GbSTS"].ToString().Trim();
                    strAmSet3 = dt.Rows[i]["AmSet3"].ToString().Trim();

                    if (strGbSTS == "6")
                    {
                        ssView_Sheet1.Cells[nRow - 1, 0, nRow - 1, ssView_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(128, 255, 255);
                        ssView_Sheet1.Cells[nRow - 1, 16].Text = "계산중";
                    }
                    else if (dt.Rows[i]["GatewonTime"].ToString().Trim() == "") //가퇴원
                    {
                        if (string.Compare(strGbSTS, "5") >= 0)
                        {
                            ssView_Sheet1.Cells[nRow - 1, 0, nRow - 1, ssView_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 128);
                            ssView_Sheet1.Cells[nRow - 1, 16].Text = "가, 완료";
                        }
                        else
                        {
                            ssView_Sheet1.Cells[nRow - 1, 16].Text = "가, 심사중";
                        }
                    }
                    else if (strGbSTS == "5" && strAmSet3 == "9")
                    {
                        ssView_Sheet1.Cells[nRow - 1, 0, nRow - 1, ssView_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 128);
                        ssView_Sheet1.Cells[nRow - 1, 16].Text = "심사완료";
                    }
                    else if (strGbSTS == "2")
                    {
                        ssView_Sheet1.Cells[nRow - 1, 16].Text = "퇴원접수";
                    }
                    else if (strGbSTS == "3")
                    {
                        ssView_Sheet1.Cells[nRow - 1, 16].Text = "대조리스트";
                    }
                    else
                    {
                        ssView_Sheet1.Cells[nRow - 1, 16].Text = "심사중";
                    }
                    ssView_Sheet1.Cells[nRow - 1, 17].Text = dt.Rows[i]["IPDNO"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 18].Text = dt.Rows[i]["TRSNO"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 19].Text = dt.Rows[i]["GbSTS"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 20].Text = dt.Rows[i]["AmSet3"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void Print2()
        {
            Cursor.Current = Cursors.WaitCursor;

            string strHead1 = "";
            string strHead2 = "";
            string strFont1 = "";
            string strFont2 = "";
            string strFoot = "";

            strFont1 = "/c/fn\"굴림체\" /fz\"16\"  /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";

            strFont1 = "/fn\"굴림\" /fz\"18\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";

            strHead1 = "/c/f1" + "퇴 원 예 약 자  명 단" + "/f1/n";   //제목 센터
            strHead2 = "/l/f2" + (dtpFdate.Value).ToString("yyyy-MM-dd") + "/f2/n/n";
            strFoot = "/l/f2" + "작성자 : " + GstrJobName + VB.Space(10) + "PAGE : " + "/p";

            ssView_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssView_Sheet1.PrintInfo.ZoomFactor = 0.85f;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2 + "/n";
            ssView_Sheet1.PrintInfo.Footer = strFont2 + strFoot;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 50;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 50;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = false;
            ssView_Sheet1.PrintInfo.ShowShadows = true;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.PrintType = 0;
            ssView.PrintSheet(0);

            btnPrint.Enabled = true;

            Cursor.Current = Cursors.Default;
        }

        private bool Dblclick()
        {
            int nIPDNO = 0;
            int nTRSNO = 0;
            string strPano = "";
            string strBi = "";
            string strGbSTS = "";
            string strAmSet3 = "";
            int inRow = ssView_Sheet1.ActiveRowIndex;
            int inCol = ssView_Sheet1.ActiveColumnIndex;
            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            bool rtnVal = false;

            //TODO 
            //FrmSlipScreen . TxtPano . Text = SS1 . Text

            if (inCol != 11)
            {
                return rtnVal;
            }

            strPano = ssView_Sheet1.Cells[inRow, 0].Text;
            strBi = ssView_Sheet1.Cells[inRow, 17].Text;
            nIPDNO = Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[inRow, 18].Text));
            nTRSNO = Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[inRow, 19].Text));
            strGbSTS = ssView_Sheet1.Cells[inRow, 20].Text;

            if (strAmSet3 != "9")
            {
                return rtnVal;
            }

            if (strGbSTS != "4" && strGbSTS != "5")
            {
                return rtnVal;
            }

            if (clsVbfunc.ProgramExecuteSabun(clsDB.DbCon, "IPD_퇴원심사완료") == false)
            {
                ComFunc.MsgBox("해당하는 작업자는 퇴원완료 SET 을 풀수가 없습니다.", "확인");
                return rtnVal;
            }

            Cursor.Current = Cursors.WaitCursor;


            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (ComFunc.MsgBoxQ("심사완료 Setting을 취소 하시겠습니까?", "심사완료 SET 해제", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " UPDATE WORK_IPD_TRANS SET ";
                    SQL = SQL + ComNum.VBLF + "        AmSet3 = '0'  ";
                    SQL = SQL + ComNum.VBLF + "  WHERE TRSNO = " + nTRSNO + " ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr + "IPD_TRANS UPDATE 중 ERROR 발생!!!, 전산실로 연락 바람!!! ");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "UPDATE WORK_IPD_MASTER SET ";
                    SQL = SQL + ComNum.VBLF + " GbSTS='3',SimsaTime='', PrintTime='' ";
                    SQL = SQL + ComNum.VBLF + " WHERE IPDNO=" + nIPDNO + " ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr + "IPD_TRANS UPDATE 중 ERROR 발생!!!, 전산실로 연락 바람!!! ");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    ComFunc.MsgBox("저장하였습니다.");
                    Cursor.Current = Cursors.Default;
                    rtnVal = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
            return rtnVal;
        }
        #endregion

        #region 퇴원예약자조회 dietorder.vbp

        private string BiDefineProcess(string strBi)
        {
            string strVal = "";
            switch (strBi)
            {
                case "11":
                    strVal = "공단";
                    break;
                case "12":
                    strVal = "연합회";
                    break;
                case "13":
                    strVal = "지역";
                    break;
                case "21":
                    strVal = "보호1";
                    break;
                case "22":
                    strVal = "보호2";
                    break;
                case "23":
                    strVal = "보호2";
                    break;
                case "24":
                    strVal = "행려";
                    break;
                case "31":
                    strVal = "산재";
                    break;
                case "32":
                    strVal = "공상";
                    break;
                case "33":
                    strVal = "산재공상";
                    break;
                case "41":
                    strVal = "공단180";
                    break;
                case "42":
                    strVal = "직장180";
                    break;
                case "43":
                    strVal = "지역180";
                    break;
                case "44":
                    strVal = "가계부";
                    break;
                case "45":
                    strVal = "보험계약";
                    break;
                case "51":
                    strVal = "일반";
                    break;
                case "52":
                    strVal = "TA보험";
                    break;
                case "53":
                    strVal = "계약";
                    break;
                case "54":
                    strVal = "미확인";
                    break;
                case "55":
                    strVal = "TA일반";
                    break;
                default:
                    strVal = "";
                    break;
            }
            return strVal;
        }

        private void Screen_Display3()
        {
            int i = 0;
            int j = 0;
            DataTable dt = null;
            DataTable dtFucn = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string StrDate1 = "";
            string StrDate2 = "";
            string strDeptCode = "";
            string strDeptName = "";
            string nRowCnt = "";

            ssView_Sheet1.ColumnHeader.Cells[0, 16].Text = "상태";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                StrDate1 = dtpFdate.Text;
                StrDate2 = Convert.ToDateTime(StrDate1).AddDays(1).ToString("yyyy-MM-dd");

                SQL = "";
                SQL = SQL + ComNum.VBLF + " Select WardCode, RoomCode, Pano, Sname, DeptCode, Bi, ";
                SQL = SQL + ComNum.VBLF + "  To_Char(InDate,'YYYY-MM-DD') Idate, ";
                SQL = SQL + ComNum.VBLF + "  To_Char(RoutDate,'HH24:MI:SS') RTime, Amset1 ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";// '＃윗줄원본
                SQL = SQL + ComNum.VBLF + " WHERE ROutDate >= TO_DATE('" + StrDate1 + "' ,'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND ROutDate <= TO_DATE('" + StrDate2 + "' ,'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND GBSTS IN ('0', '2', '3', '4') ";// '＃추가했음;
                SQL = SQL + ComNum.VBLF + " ORDER BY WardCode, RoomCode, Pano ";

                SQL = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.RowCount = i + 1;
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Sname"].ToString().Trim();
                    ssView_Sheet1.Columns[2].Visible = false;
                    ssView_Sheet1.Columns[3].Visible = false;
                    ssView_Sheet1.Columns[4].Visible = false;
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                    strBi = dt.Rows[i]["BI"].ToString().Trim();

                    BiDefineProcess(ssView_Sheet1.Cells[i, 13].Text = strBiName);

                    strDeptCode = dt.Rows[i]["DeptCode"].ToString().Trim();

                    SQL = SQL + ComNum.VBLF + " Select DeptNameK ";
                    SQL = SQL + ComNum.VBLF + " From " + ComNum.DB_PMPA + "Bas_Clinicdept ";
                    SQL = SQL + ComNum.VBLF + " Where DeptCode = '" + strDeptCode + "' ";

                    SQL = clsDB.GetDataTable(ref dtFucn, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    strDeptName = "";
                    if (dtFucn.Rows.Count == 1)
                    {
                        strDeptName = dt.Rows[0]["DeptNameK"].ToString().Trim();
                    }
                    dtFucn.Dispose();
                    dtFucn = null;

                    ssView_Sheet1.Cells[i, 7].Text = strDeptName;
                    ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["IDate"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["RTime"].ToString().Trim();

                    switch (dt.Rows[i]["Amset1"].ToString().Trim())
                    {
                        case "0":
                            ssView_Sheet1.Cells[i, 16].Text = "재원중";
                            break;
                        case "1":
                            ssView_Sheet1.Cells[i, 16].Text = "퇴원";
                            break;
                        case "2":
                            ssView_Sheet1.Cells[i, 16].Text = "계산중";
                            break;
                        case "3":
                            ssView_Sheet1.Cells[i, 16].Text = "가퇴원";
                            break;
                    }
                }
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void Print3()
        {
            Cursor.Current = Cursors.WaitCursor;

            string sHead1 = "";
            string sHead2 = "";
            string sFont1 = "";
            string sFont2 = "";
            string sFont3 = "";
            string sFoot = "";

            sFont1 = "/c/fn\"굴림체\" /fz\"20\"  /fb1 /fi0 /fu0 /fk0 /fs1";
            sFont2 = "/fn\"굴림체\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";

            sFont1 = "/fn\"굴림\" /fz\"18\" /fb1 /fi0 /fu0 /fk0 /fs1";
            sFont2 = "/fn\"굴림\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";
            sFont3 = "/fn\"굴림\" /fz\"9\" /fb0 /fi0 /fu0 /fk0 /fs3";

            sHead1 = sHead1 + "/c/f1" + "퇴원예약자 조회" + "/f1/n";   //제목 센터
            sHead2 = "/n/l/f2" + "조회일자: " + (dtpFdate.Value).ToString("yyyy-MM-dd") + VB.Space(5) + "인쇄일자 : " + (dtpFdate.Value).ToString("yyyy-MM-dd") + "/f2";

            btnPrint.Enabled = false;

            ssView_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssView_Sheet1.PrintInfo.ZoomFactor = 0.85f;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView_Sheet1.PrintInfo.Header = sFont1 + sHead1 + "/n" + sFont2 + sHead2 + "/n";
            ssView_Sheet1.PrintInfo.Footer = sFont3 + sFoot;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 50;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 50;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = false;
            ssView_Sheet1.PrintInfo.ShowShadows = true;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.PrintType = 0;
            ssView.PrintSheet(0);

            btnPrint.Enabled = true;

            Cursor.Current = Cursors.Default;
        }
        #endregion


    }
}
