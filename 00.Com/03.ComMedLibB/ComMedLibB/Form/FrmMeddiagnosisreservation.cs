using ComBase; //기본 클래스
using FarPoint.Win;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComMedLibB
{
    /// <summary>
    /// Class Name      : ComMedLibB
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-12-08
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= d:\psmh\Ocs\ipdocs\iorder\iorder23.frm(FrmReserved)" >> FrmMeddiagnosisreservation.cs 폼이름 재정의" />
    public partial class FrmMeddiagnosisreservation : Form
    {
        string SQL = "";
        DataTable dt = null;
        DataTable dt1 = null;
        string SqlErr = "";
        int i = 0;
        int intRowAffected = 0;
        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
        ComFunc CF = new ComFunc();


        string[] FstrRTime = new string[101];//   As String * 5
        string FstrDrCode = "";
        string[] FstrRDate = new string[200]; //   As String * 10
        string FstrReserved = "";  //'예약여부
        string FstrRDept = "";  //'예약과
        string FstrRDoct = "";  //'예약의사
        string FstrYDate = "";  //'예약일자
        string FstrROWID = "";  //'ROWID
        string FstrDate3 = "";
        int FnMorningNo = 0;
        int FnResvTimeGbn = 0;
        int FnResvInwon = 0;
        int FnResvInwon2 = 0;
        int FnSelRow = 0;
        int FnSelCol = 0;

        public FrmMeddiagnosisreservation(string Ptno, string Drcod, string Deptcod, string sName)
        {
            clsOrdFunction.Pat.PtNo = Ptno;
            clsOrdFunction.Pat.DrCode = Drcod;
            clsOrdFunction.Pat.DeptCode = Deptcod;
            clsOrdFunction.Pat.sName = sName;

            InitializeComponent();
        }

        public FrmMeddiagnosisreservation()
        {
            InitializeComponent();
        }

        private void FrmMeddiagnosisreservation_Load(object sender, EventArgs e)
        {
            int j = 0;
            string strData = "";

            //TEST
            //clsOrdFunction.Pat.PtNo = "09105255";
            //clsOrdFunction.Pat.DrCode = "3214";
            //clsOrdFunction.Pat.DeptCode = "PD";
            //clsOrdFunction.Pat.sName = "이다예";
            //----

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            btnRegist.Enabled = true;
            btnCancel.Enabled = true;
            btnChange.Enabled = false;

            SS1_Sheet1.Rows[0].Visible = false;
            SS1_Sheet1.Rows[1].Visible = false;

            SS2_Sheet1.RowCount = 0;
            SS2_Sheet1.Columns[9].Visible = false;

            lblRTime.Text = "";
            //TODO TEST


            btnYeyak0.Text = "";
            btnYeyak1.Text = "";
            btnYeyak2.Text = "";
            btnYeyak3.Text = "";
            btnYeyak4.Text = "";
            btnYeyak5.Text = "";
            btnYeyak6.Text = "";

            cboDept.Items.Clear();

            FstrReserved = "";
            FstrRDoct = (clsOrdFunction.Pat.DrCode);
            FstrYDate = "";
            FstrROWID = "";
            FstrDate3 = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT DeptCode ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + "WHERE DeptCode NOT IN ('AN','HR','TO','II','R6','PT','EM','HD','ER') ";//
                SQL = SQL + ComNum.VBLF + "ORDER BY PrintRanking ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboDept.Items.Add(dt.Rows[i]["deptcode"].ToString().Trim());
                    }
                }
                dt.Dispose();
                dt = null;

                //'병동에서 예약한 내역을 Display
                ////원본
                SQL = "";
                SQL = "SELECT DeptCode,DrCode,TO_CHAR(RDate,'YYYY-MM-DD HH24:MI') RDate ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.OCS_RESERVED ";
                SQL = SQL + ComNum.VBLF + "WHERE Pano='" + clsOrdFunction.Pat.PtNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND IpdOpd='I' ";
                SQL = SQL + ComNum.VBLF + "  AND BDate>=TRUNC(SYSDATE-5) ";
                SQL = SQL + ComNum.VBLF + "  AND GbSunap ='0' ";//  '2013-10-04

                //test
                //clsOrdFunction.Pat.PtNo = "10220483";
                //SQL = "";
                //SQL = "SELECT DeptCode,DrCode,TO_CHAR(RDate,'YYYY-MM-DD HH24:MI') RDate ";
                //SQL = SQL + ComNum.VBLF + " FROM ADMIN.OCS_RESERVED ";
                //SQL = SQL + ComNum.VBLF + "WHERE Pano='" + clsOrdFunction.Pat.PtNo + "' ";
                //// SQL = SQL + ComNum.VBLF + "  AND IpdOpd='I' ";
                //SQL = SQL + ComNum.VBLF + "  AND BDate>=TRUNC(SYSDATE-365) ";
                ////SQL = SQL + ComNum.VBLF + "  AND GbSunap ='0' ";//  '2013-10-04


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strData = dt.Rows[i]["RDate"].ToString().Trim() + " ";
                        strData = strData + dt.Rows[i]["DeptCode"].ToString().Trim();

                        if (i == 0)
                        {
                            btnYeyak0.Text = strData;
                        }
                        else if (i == 1)
                        {
                            btnYeyak1.Text = strData;
                        }
                        else if (i == 2)
                        {
                            btnYeyak2.Text = strData;
                        }
                        else if (i == 3)
                        {
                            btnYeyak3.Text = strData;
                        }
                        else if (i == 4)
                        {
                            btnYeyak4.Text = strData;
                        }
                        else if (i == 5)
                        {
                            btnYeyak5.Text = strData;
                        }
                        else if (i == 6)
                        {
                            btnYeyak6.Text = strData;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                if (btnYeyak0.Text != "")
                {
                    btnYeyak_Click(btnYeyak0);
                    return;
                }

                //'기존 예약이 1건도 없으면
                // '재원과를 표시함

                ComFunc.ComboFind(cboDept, "L", 10, clsOrdFunction.Pat.DeptCode);

                j = 0;
                cboDoct.Items.Clear();
                // '재원과의 의사를 Display
                SQL = "";
                SQL = "SELECT DrCode,DrName ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + "WHERE DrDept1 = '" + (clsOrdFunction.Pat.DeptCode).Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "  AND TOUR <> 'Y' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY PrintRanking ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strData = VB.Left(dt.Rows[i]["DrCode"].ToString().Trim() + ComNum.VBLF, 4) + ".";
                        strData = strData + dt.Rows[i]["DrName"].ToString().Trim();
                        cboDoct.Items.Add(strData);

                        if (clsOrdFunction.Pat.DrCode == dt.Rows[i]["DrCode"].ToString().Trim())
                        {
                            j = i;
                        }
                    }


                }
                dt.Dispose();
                dt = null;

                txtPtno.Text = clsOrdFunction.Pat.PtNo;
                lblName.Text = clsOrdFunction.Pat.sName;

                cboDoct.SelectedIndex = j;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        private void Yeyak_Inwon_Display_New(string ArgDrCode)
        {
            int I = 0;
            int j = 0;
            int K = 0;
            int nRow = 0;
            int nCol = 0;
            int nHH = 0;// '시
            int nMM = 0;// '분
            int nTime = 0;
            int nRead = 0;
            int nREAD1 = 0;
            int inx1 = 0;
            int inx2 = 0;
            string strRDate = "";
            string strRTime = "";
            string strETime = "";
            string strGDate = "";
            string strAmTime = "";
            string strAmTime2 = "";
            string strPmTime = "";
            string strPmTime2 = "";
            int nTimeCNT = 0;
            int[,,] nCnt = new int[100, 100, 3];

            Cursor.Current = Cursors.WaitCursor;

            strGDate = Convert.ToDateTime(strDTP).AddDays(59).ToString("yyyy-MM-dd");

            try
            {
                strAmTime = "";
                strPmTime = "";
                lblaYinwon.Text = "";
                //'예약시간단위 및 단위시간당 인원을 READ
                SQL = "";
                SQL = "SELECT DrCode,DrName,YTimeGbn,YInwon,AmTime,PmTime,YInwon2,AmTime2,PmTime2 ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + "WHERE DrCode = '" + ArgDrCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    FnResvTimeGbn = (int)VB.Val(dt.Rows[0]["YTimeGbn"].ToString().Trim());
                    FnResvInwon = (int)VB.Val(dt.Rows[0]["YInwon"].ToString().Trim());
                    strAmTime = dt.Rows[0]["AmTime"].ToString().Trim();
                    strPmTime = dt.Rows[0]["PmTime"].ToString().Trim();
                    FnResvInwon2 = (int)VB.Val(dt.Rows[0]["YInwon2"].ToString().Trim());
                    strAmTime2 = dt.Rows[0]["AmTime2"].ToString().Trim();
                    strPmTime2 = dt.Rows[0]["PmTime2"].ToString().Trim();
                    lblaYinwon.Text = "예약인원(오전:" + FnResvInwon + "명 오후:" + FnResvInwon2 + "명)";
                }
                dt.Dispose();
                dt = null;

                FstrDrCode = ArgDrCode;
                //'예약구분이 없으면 기본으로 30분단위 예약
                if (FnResvTimeGbn == 0)
                    FnResvTimeGbn = 4;
                //'예약인원이 없으면 인원제한 않함
                if (FnResvInwon == 0)
                    FnResvInwon = 999;
                //'예약 시작시간 설정
                if (strAmTime == "")
                    strAmTime = "09:30";
                if (strPmTime == "")
                    strPmTime = "14:00";

                for (i = 1; i < 100; i++)
                {
                    FstrRTime[i] = "";
                }
                for (i = 1; i < 100; i++)
                {
                    FstrRDate[i] = "";
                }
                nRow = 5;
                FnMorningNo = 0;

                SS2_Sheet1.RowCount = 0;    //'예약자 명단 Sheet를 Clear

                switch (FnResvTimeGbn)
                {
                    case 1:
                        nTime = 10;
                        break;
                    case 2:
                        nTime = 15;
                        break;
                    case 3:
                        nTime = 20;
                        break;
                    case 4:
                        nTime = 30;
                        break;
                }

                j = 1;

                if (strAmTime2 == "" || strPmTime2 == "")
                {
                    ComFunc.MsgBox("스케쥴 내역이 존재 하지 않습니다");
                    return;
                }


                for (i = CF.TIME_MI(strAmTime); i <= CF.TIME_MI(strAmTime2); i += nTime)
                {
                    FstrRTime[j] = CF.TIME_MI_TIME(i);
                    nRow = nRow + 1;
                    if (nRow > SS1_Sheet1.RowCount)
                    {
                        SS1_Sheet1.RowCount = nRow;
                    }
                    SS1_Sheet1.Cells[nRow - 1, 0].Text = FstrRTime[j];
                    FnMorningNo = nRow;
                    j = j + 1;
                }

                for (i = CF.TIME_MI(strPmTime); i <= CF.TIME_MI(strPmTime2); i += nTime)
                {
                    FstrRTime[j] = CF.TIME_MI_TIME(i);
                    nRow = nRow + 1;
                    if (nRow > SS1_Sheet1.RowCount)
                    {
                        SS1_Sheet1.RowCount = nRow;
                    }
                    SS1_Sheet1.Cells[nRow - 1, 0].Text = FstrRTime[j];
                    //FnMorningNo = nRow;
                    j = j + 1;
                }

                nTimeCNT = nRow - 4;
                FstrRTime[nTimeCNT] = "23:59";

                // '예약 스케쥴을 읽어 SHEET의 상단에 Display

                //원본
                SQL = "";
                SQL = "SELECT TO_CHAR(SchDate,'YYYY-MM-DD') SchDate,TO_CHAR(SchDate,'DY') Yoil,GbDay,GbJin, GbJin2 ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE ";
                SQL = SQL + ComNum.VBLF + "WHERE DrCode='" + ArgDrCode + "' ";
                SQL = SQL + ComNum.VBLF + "  AND SchDate>TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + "  AND SchDate<=TO_DATE('" + strGDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SchDate ";

                //TEST
                //SQL = "";
                //SQL = "SELECT TO_CHAR(SchDate,'YYYY-MM-DD') SchDate,TO_CHAR(SchDate,'DY') Yoil,GbDay,GbJin, GbJin2 ";
                //SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE ";
                //SQL = SQL + ComNum.VBLF + "WHERE DrCode='" + ArgDrCode + "' ";
                //SQL = SQL + ComNum.VBLF + "  AND SchDate>TRUNC(SYSDATE) - 365 ";
                //SQL = SQL + ComNum.VBLF + "  AND SchDate<=TO_DATE('2017-12-31','YYYY-MM-DD') ";
                //SQL = SQL + ComNum.VBLF + "ORDER BY SchDate ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                SS1_Sheet1.RowCount = nRow + 1;

                if (dt.Rows.Count > 0)
                {
                    nRead = dt.Rows.Count;
                    SS1_Sheet1.ColumnCount = nRead + 1;

                    //heet Clear & SET, 줄그리기, Cell Type을 설정

                    SS1_Sheet1.Rows.Count = nRow;
                    SS1_Sheet1.Cells[5, 1, SS1_Sheet1.RowCount - 1, SS1_Sheet1.ColumnCount - 1].Text = "";
                    SS1_Sheet1.Cells[5, 1, SS1_Sheet1.RowCount - 1, SS1_Sheet1.ColumnCount - 1].BackColor = System.Drawing.Color.FromArgb(255, 255, 255);

                    SS1_Sheet1.Cells[0, 1, SS1_Sheet1.RowCount - 1, SS1_Sheet1.ColumnCount - 1].Border = new LineBorder(Color.Black, 0);
                    //'스케쥴을 SHEET 상단에 표시함

                    nCol = 1;
                    if (nRead >= 61)
                        nRead = 61;// 'JJY(2004-05-04) 추가 변수최대가 60개

                    for (i = 0; i < nRead; i++)
                    {
                        //'일요일은 표시 않함
                        if ((dt.Rows[i]["YOIL"].ToString().Trim().ToUpper()) != "SUN")
                        {
                            nCol = nCol + 1;
                            FstrRDate[nCol - 1] = dt.Rows[i]["SchDate"].ToString().Trim();
                            strRDate = dt.Rows[i]["SchDate"].ToString().Trim();

                            // SS1.ColWidth(nCol) = 5.1

                            //SS1_Sheet1.RowHeader.Cells[0, nCol - 1].Text = VB.Right(strRDate, 5);
                            SS1_Sheet1.Columns.Get(nCol - 1).Label = VB.Right(strRDate, 5) + "\r\n" + "+" + (i + 1);
                            SS1_Sheet1.Cells[0, nCol - 1].Text = strRDate;
                            SS1_Sheet1.Cells[1, nCol - 1].Text = dt.Rows[i]["GbJin"].ToString().Trim();

                            switch ((dt.Rows[i]["YOIL"].ToString().Trim()).ToUpper())
                            {
                                case "SUN":
                                case "일":
                                    SS1_Sheet1.Cells[2, nCol - 1].Text = "일";
                                    break;
                                case "MON":
                                case "월":
                                    SS1_Sheet1.Cells[2, nCol - 1].Text = "월";
                                    break;
                                case "TUE":
                                case "화":
                                    SS1_Sheet1.Cells[2, nCol - 1].Text = "화";
                                    break;
                                case "WED":
                                case "수":
                                    SS1_Sheet1.Cells[2, nCol - 1].Text = "수";
                                    break;
                                case "THU":
                                case "목":
                                    SS1_Sheet1.Cells[2, nCol - 1].Text = "목";
                                    break;
                                case "FRI":
                                case "금":
                                    SS1_Sheet1.Cells[2, nCol - 1].Text = "금";
                                    break;
                                case "SAT":
                                case "토":
                                    SS1_Sheet1.Cells[2, nCol - 1].Text = "토";
                                    break;
                                default:
                                    SS1_Sheet1.Cells[2, nCol - 1].Text = "";
                                    break;
                            }
                            //오전
                            switch (dt.Rows[i]["GbJin"].ToString().Trim())
                            {
                                case "1":
                                    SS1_Sheet1.Cells[3, nCol - 1].Text = "진료";
                                    SS1_Sheet1.Cells[5, nCol - 1, FnMorningNo - 1, nCol - 1].BackColor = System.Drawing.Color.FromArgb(205, 250, 220);
                                    break;
                                case "2":
                                    SS1_Sheet1.Cells[3, nCol - 1].Text = "수술";
                                    SS1_Sheet1.Cells[5, nCol - 1, FnMorningNo - 1, nCol - 1].BackColor = System.Drawing.Color.FromArgb(255, 192, 128);
                                    break;
                                case "3":
                                    SS1_Sheet1.Cells[3, nCol - 1].Text = "특검";
                                    SS1_Sheet1.Cells[5, nCol - 1, FnMorningNo - 1, nCol - 1].BackColor = System.Drawing.Color.FromArgb(255, 128, 0);
                                    break;
                                case "9":
                                    SS1_Sheet1.Cells[3, nCol - 1].Text = "OFF";
                                    SS1_Sheet1.Cells[5, nCol - 1, FnMorningNo - 1, nCol - 1].BackColor = System.Drawing.Color.FromArgb(255, 192, 192);
                                    break;
                                default:
                                    SS1_Sheet1.Cells[3, nCol - 1].Text = "휴진";
                                    SS1_Sheet1.Cells[5, nCol - 1, FnMorningNo - 1, nCol - 1].BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                                    break;
                            }
                            //오후
                            switch (dt.Rows[i]["GbJin2"].ToString().Trim())
                            {
                                case "1":
                                    SS1_Sheet1.Cells[4, nCol - 1].Text = "진료";
                                    SS1_Sheet1.Cells[FnMorningNo, nCol - 1, SS1_Sheet1.RowCount - 1, nCol - 1].BackColor = System.Drawing.Color.FromArgb(205, 250, 220);
                                    break;
                                case "2":
                                    SS1_Sheet1.Cells[4, nCol - 1].Text = "수술";
                                    SS1_Sheet1.Cells[FnMorningNo, nCol - 1, SS1_Sheet1.RowCount - 1, nCol - 1].BackColor = System.Drawing.Color.FromArgb(255, 192, 128);
                                    break;
                                case "3":
                                    SS1_Sheet1.Cells[4, nCol - 1].Text = "특검";
                                    SS1_Sheet1.Cells[FnMorningNo, nCol - 1, SS1_Sheet1.RowCount - 1, nCol - 1].BackColor = System.Drawing.Color.FromArgb(255, 128, 0);
                                    break;
                                case "9":
                                    SS1_Sheet1.Cells[4, nCol - 1].Text = "OFF";
                                    SS1_Sheet1.Cells[FnMorningNo, nCol - 1, SS1_Sheet1.RowCount - 1, nCol - 1].BackColor = System.Drawing.Color.FromArgb(255, 192, 192);
                                    break;
                                default:
                                    SS1_Sheet1.Cells[4, nCol - 1].Text = "휴진";
                                    SS1_Sheet1.Cells[FnMorningNo, nCol - 1, SS1_Sheet1.RowCount - 1, nCol - 1].BackColor = Color.White;
                                    break;
                            }
                        }
                    }
                    SS1_Sheet1.ColumnCount = nCol;
                }
                dt.Dispose();
                dt = null;

                SS1_Sheet1.Rows.Get(1).Border = new LineBorder(Color.Black, 1, true, true, true, true);

                SQL = "";
                //'의사별 기타 스케쥴을 읽어 Sheet에 표시함
                //원본
                SQL = "SELECT TO_CHAR(SchDate,'YYYY-MM-DD') SchDate,STime,ETime ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE_ETC ";
                SQL = SQL + ComNum.VBLF + "WHERE DrCode='" + ArgDrCode + "' ";
                SQL = SQL + ComNum.VBLF + "  AND SchDate>TRUNC(SYSDATE)";
                SQL = SQL + ComNum.VBLF + "  AND SchDate<=TO_DATE('" + strGDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SchDate,STime ";

                //test
                //SQL = "SELECT TO_CHAR(SchDate,'YYYY-MM-DD') SchDate,STime,ETime ";
                //SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE_ETC ";
                //SQL = SQL + ComNum.VBLF + "WHERE DrCode='" + ArgDrCode + "' ";
                //SQL = SQL + ComNum.VBLF + "  AND SchDate>TRUNC(SYSDATE) - 365";
                //SQL = SQL + ComNum.VBLF + "  AND SchDate<=TO_DATE('2017-12-31','YYYY-MM-DD') ";
                //SQL = SQL + ComNum.VBLF + "ORDER BY SchDate,STime ";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt1.Rows.Count > 0)
                {
                    for (i = 0; i < dt1.Rows.Count; i++)
                    {
                        strRDate = dt1.Rows[i]["SchDate"].ToString().Trim();
                        strRTime = dt1.Rows[i]["STime"].ToString().Trim();
                        strETime = dt1.Rows[i]["ETime"].ToString().Trim();
                        // '예약일자 Col 찾기

                        inx1 = 0;
                        for (j = 1; j <= 100; j++)
                        {
                            if (strRDate == FstrRDate[j])
                            {
                                inx1 = j;
                                break;
                            }
                        }

                        if (inx1 > 0)
                        {
                            for (j = 1; j >= nTimeCNT; j++)
                            {
                                if (string.Compare(FstrRTime[j], strRTime) >= 0 && string.Compare(FstrRTime[j], strETime) <= 0)
                                {
                                    SS1_Sheet1.Cells[(j + 4) - 1, (inx1 + 1) - 1].BackColor = System.Drawing.Color.FromArgb(192, 192, 192);
                                }
                            }
                        }
                    }
                }
                dt1.Dispose();
                dt1 = null;

                //'예약자 인원을 COUNT
                SQL = "";
                SQL = "SELECT TO_CHAR(Date3,'YYYY-MM-DD HH24:MI') RTime,COUNT(*) CNT ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.OPD_RESERVED_NEW ";
                SQL = SQL + ComNum.VBLF + "WHERE Date3> TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + "  AND Date3<=TO_DATE('" + strGDate + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "  AND DrCode='" + ArgDrCode + "' ";
                SQL = SQL + ComNum.VBLF + "  AND TRANSDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "  AND RETDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "GROUP BY DATE3 ";
                SQL = SQL + ComNum.VBLF + "ORDER BY DATE3 ";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt1.Rows.Count > 0)
                {
                    for (i = 0; i < dt1.Rows.Count; i++)
                    {
                        strRDate = VB.Left(dt1.Rows[i]["RTIME"].ToString().Trim(), 10);
                        strRTime = VB.Right(dt1.Rows[i]["RTIME"].ToString().Trim(), 5);

                        // '예약일자
                        inx1 = 0;
                        for (j = 1; j <= 100; j++)
                        {
                            if (strRDate == FstrRDate[j])
                            {
                                inx1 = j;
                                //break;
                            }
                        }

                        //'예약시간 ROW
                        inx2 = 0;
                        for (K = 1; K <= nTimeCNT; K++)
                        {
                            if (string.Compare(strRTime, FstrRTime[K + 1]) < 0)
                            {
                                inx2 = K;
                                break;
                            }
                        }

                        if (inx1 > 0 && inx2 > 0)
                        {
                            nCnt[inx1, inx2, 1] = nCnt[inx1, inx2, 1] + Convert.ToInt32(dt1.Rows[i]["CNT"].ToString().Trim());
                        }
                    }
                }
                dt1.Dispose();
                dt1 = null;

                // '병동의 퇴원자예약 미수납자를 READ
                SQL = "";
                SQL = "SELECT TO_CHAR(RDate,'YYYY-MM-DD HH24:MI') RTime,COUNT(*) CNT ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_RESERVED ";
                SQL = SQL + ComNum.VBLF + "WHERE BDate=TRUNC(SYSDATE)";
                SQL = SQL + ComNum.VBLF + "  AND RDate> TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + "  AND RDate<=TO_DATE('" + strGDate + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "  AND DrCode='" + ArgDrCode + "' ";
                SQL = SQL + ComNum.VBLF + "GROUP BY RDate ";
                SQL = SQL + ComNum.VBLF + "ORDER BY RDate ";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt1.Rows.Count > 0)
                {
                    for (i = 0; i < dt1.Rows.Count; i++)
                    {
                        strRDate = VB.Left(dt1.Rows[i]["RTime"].ToString().Trim(), 10);
                        strRDate = VB.Right(dt1.Rows[i]["RTime"].ToString().Trim(), 5);

                        //  '예약일자
                        inx1 = 0;
                        for (j = 1; j <= 100; j++)
                        {
                            if (strRDate == FstrRDate[j])
                            {
                                inx1 = j;
                                //break;
                            }
                        }
                        //'예약시간 ROW
                        inx2 = 0;
                        for (K = 1; K <= nTimeCNT; K++)
                        {
                            if (string.Compare(strRTime, FstrRTime[K + 1]) < 0)
                            {
                                inx2 = K;
                                break;
                            }
                        }

                        if (inx1 > 0 && inx2 > 0)
                        {
                            nCnt[inx1, inx2, 1] = nCnt[inx1, inx2, 1] + Convert.ToInt32(dt1.Rows[i]["CNT"].ToString().Trim());
                        }
                    }
                }

                dt1.Dispose();
                dt1 = null;

                SQL = "";
                //'전화접수 인원을 COUNT
                SQL = "SELECT TO_CHAR(RDate,'YYYY-MM-DD') RDate,RTime,COUNT(*) CNT ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_TELRESV ";
                SQL = SQL + ComNum.VBLF + "WHERE RDate> TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + "  AND RDate<=TO_DATE('" + strGDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND DrCode='" + ArgDrCode + "' ";
                SQL = SQL + ComNum.VBLF + "GROUP BY RDate,RTime ";
                SQL = SQL + ComNum.VBLF + "ORDER BY RDate,RTime ";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt1.Rows.Count > 0)
                {
                    for (i = 0; i < dt1.Rows.Count; i++)
                    {
                        strRDate = dt1.Rows[i]["RDate"].ToString().Trim();
                        strRTime = dt1.Rows[i]["RTime"].ToString().Trim();
                        //'예약일자
                        inx1 = 0;

                        for (j = 1; j <= 100; j++)
                        {
                            if (strRDate == FstrRDate[j])
                            {
                                inx1 = j;
                                //break;
                            }
                        }

                        //'예약시간 ROW
                        inx2 = 0;
                        for (K = 1; K <= nTimeCNT; K++)
                        {
                            if (string.Compare(strRTime, FstrRTime[K + 1]) < 0)
                            {
                                inx2 = K;
                                break;
                            }
                        }

                        if (inx1 > 0 && inx2 > 0)
                        {
                            nCnt[inx1, inx2, 2] = Convert.ToInt32(nCnt[inx1, inx2, 2] + dt1.Rows[i]["CNT"].ToString().Trim());
                        }
                    }
                }
                dt1.Dispose();
                dt1 = null;

                // '자료를 SHEET에 Display
                for (i = 2; i < 100; i++)
                {
                    for (j = 1; j <= nTimeCNT; j++)
                    {
                        if (nCnt[i - 1, j, 1] != 0 || nCnt[i - 1, j, 2] != 0)
                        {
                            SS1_Sheet1.Cells[(j + 5) - 1, i - 1].Text = string.Format("{0:##0}", nCnt[(i - 1), j, 1] + nCnt[(i - 1), j, 2]);
                            SS1_Sheet1.Cells[(j + 5) - 1, i - 1].Text += "(" + string.Format("{0:##0}", nCnt[i - 1, j, 2]) + ")";
                        }
                    }
                }

                FnSelRow = 0;
                FnSelCol = 0;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "DELETE " + ComNum.DB_MED + "OCS_RESERVED ";
                SQL = SQL + ComNum.VBLF + "WHERE Pano='" + (clsOrdFunction.Pat.PtNo).Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "  AND DeptCode='" + (cboDept.Text).Trim() + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("과 예약을 취소 하였습니다.");
                Cursor.Current = Cursors.Default;

                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            int j = 0;
            string strRDate = "";
            string strRTime = "";

            if (VB.Len(lblRTime.Text) != 16)
            {
                ComFunc.MsgBox("예약일시를 (YYYY-MM-DD HH:MI) 형태로 입력하세요", "오류");
                return;
            }

            if (cboDoct.Text == "")
            {
                ComFunc.MsgBox("예약의사가 공란 입니다", "확인");
                return;
            }

            strRDate = VB.Left(lblRTime.Text, 10);
            strRDate = VB.Right(lblRTime.Text, 5);

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);


            try
            {
                if (FstrROWID == "")
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_RESERVED (BDate,Pano,SName,DeptCode,DrCode,IpdOpd,";
                    SQL = SQL + ComNum.VBLF + "RDate,GbSunap,EntSabun) VALUES (TRUNC(SYSDATE),'" + (txtPtno.Text).Trim() + "','";
                    SQL = SQL + ComNum.VBLF + (lblName.Text).Trim() + "','" + (cboDept.Text).Trim() + "','";
                    SQL = SQL + ComNum.VBLF + VB.Left(cboDoct.Text, 4) + "','I',";
                    SQL = SQL + ComNum.VBLF + "TO_DATE('" + strRDate + " " + strRTime + "','YYYY-MM-DD HH24:MI'),'0',";
                    SQL = SQL + ComNum.VBLF + clsOrdFunction.GstrDrCode + ") ";
                }
                else
                {
                    SQL = "UPDATE " + ComNum.DB_MED + "OCS_RESERVED SET DrCode='" + VB.Left(cboDoct.Text, 4) + "',";
                    SQL = SQL + ComNum.VBLF + " RDate=TO_DATE('" + strRDate + " " + strRTime + "','YYYY-MM-DD HH24:MI'),";
                    SQL = SQL + ComNum.VBLF + " EntSabun=" + clsOrdFunction.GstrDrCode + " ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + FstrROWID + "' ";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox(strRTime + "에 예약을 하였습니다", "확인");
                Cursor.Current = Cursors.Default;

                this.Close();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }


        }

        private void btnChange_Click(object sender, EventArgs e)
        {

            if (VB.Len(lblRTime.Text) != 16)
            {
                ComFunc.MsgBox("예약일시를 (YYYY-MM-DD HH:MI) 형태로 입력하세요", "오류");
                return;
            }

            if (cboDoct.Text == "")
            {
                ComFunc.MsgBox("예약의사가 공란 입니다", "확인");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "UPDATE ADMIN.OPD_RESERVED_NEW SET ";
                SQL = SQL + ComNum.VBLF + " Date3=TO_DATE('" + lblRTime.Text + "','YYYY-MM-DD HH24:MI'), ";
                SQL = SQL + ComNum.VBLF + " DrCode='" + VB.Left(cboDoct.Text, 4) + "' ";
                SQL = SQL + ComNum.VBLF + "WHERE Pano='" + txtPtno.Text + "' ";
                SQL = SQL + ComNum.VBLF + "  AND DeptCode='" + (clsOrdFunction.Pat.DeptCode) + "' ";
                SQL = SQL + ComNum.VBLF + "  AND DATE3 = TO_DATE('" + FstrDate3 + "','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "  AND TRANSDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "  AND RETDATE IS NULL ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr, "예약일자를 변경시 오류가 발생함");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("예약일자를 정상적으로 변경하였습니다.", "확인");
                Cursor.Current = Cursors.Default;

                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void cboDept_Click(object sender, EventArgs e)
        {

            int j = 0;
            string strDeptCode = "";
            string StrDrCode = "";
            string strData = "";

            Cursor.Current = Cursors.WaitCursor;

            btnRegist.Enabled = true;
            btnCancel.Enabled = true;
            btnChange.Enabled = false;

            strDeptCode = cboDept.Text;

            FstrReserved = "";
            FstrRDept = strDeptCode;
            FstrRDoct = (clsOrdFunction.Pat.DrCode).Trim();
            FstrYDate = "";
            FstrROWID = "";

            try
            {
                //'원무과 예약 Table에 자료가 있는지 Check
                SQL = "";
                SQL = "SELECT DeptCode,DrCode,TO_CHAR(DATE3,'YYYY-MM-DD HH24:MI') RTime,ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
                SQL = SQL + ComNum.VBLF + "WHERE Pano='" + clsOrdFunction.Pat.PtNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND DeptCode='" + strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "  AND Date3>=TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + "  AND TRANSDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "  AND RETDATE IS NULL ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    FstrReserved = "OK";
                    FstrRDoct = dt.Rows[0]["DrCode"].ToString().Trim();
                    FstrYDate = dt.Rows[0]["RTime"].ToString().Trim();
                    FstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                    FstrDate3 = FstrYDate;
                }

                dt.Dispose();
                dt = null;

                if (FstrReserved == "OK")//     '기수납한 예약자료 수정 못하게
                {
                    btnRegist.Enabled = false;
                    btnCancel.Enabled = false;
                    btnChange.Enabled = true;
                    lblStat.Text = "수납";
                }
                else
                {
                    //'입원예약이 있는지 Check
                    SQL = "SELECT TO_CHAR(RDate,'YYYY-MM-DD HH24:MI') RTime,DrCode,GbSunap,ROWID ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_RESERVED ";
                    SQL = SQL + ComNum.VBLF + "WHERE Pano='" + clsOrdFunction.Pat.PtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND DeptCode='" + strDeptCode + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND BDate>=TRUNC(SYSDATE-1) ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        FstrReserved = "YEYAK";
                        FstrRDoct = dt.Rows[0]["DrCode"].ToString().Trim();
                        FstrYDate = dt.Rows[0]["RTime"].ToString().Trim();
                        FstrDate3 = FstrYDate;
                        FstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                        lblStat.Text = "미수납";
                    }

                    dt.Dispose();
                    dt = null;
                }

                lblRTime.Text = FstrYDate;

                //'재원과의 의사를 Display
                SQL = "SELECT DrCode,DrName ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + "WHERE DrDept1 = '" + (strDeptCode).Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "  AND TOUR <> 'Y' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY PrintRanking ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    cboDoct.Items.Clear();

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strData = VB.Left(dt.Rows[i]["DrCode"].ToString().Trim() + ComNum.VBLF, 4) + ".";
                        strData = strData + dt.Rows[i]["DrName"].ToString().Trim();
                        cboDoct.Items.Add(strData);

                        if (FstrRDoct == dt.Rows[i]["DRCODE"].ToString().Trim())
                        {
                            //ComFunc.ComboFind(cboDept, "L", 10, dt.Rows[i]["DRCODE"].ToString().Trim());
                            j = i;
                        }
                    }
                }
                dt.Dispose();
                dt = null;

                cboDoct.SelectedIndex = j;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        private void cboDept_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cboDoct.Select();
            }
        }

        private void cboDoct_SelectedIndexChanged(object sender, EventArgs e)
        {
            string StrDrCode = "";

            StrDrCode = VB.Left(cboDoct.Text, 4);

            Yeyak_Inwon_Display_New(StrDrCode);
        }

        private void btnYeyak_Click(Button Send)
        {
            int j = 0;
            string strDeptCode = "";
            string StrDrCode = "";
            string strData = "";

            if (Send.Text == "")
            {
                return;
            }

            btnRegist.Enabled = true;
            btnCancel.Enabled = true;
            btnChange.Enabled = false;

            strDeptCode = VB.Right(Send.Text, 2);

            // ComFunc.ComboFind(cboDept, "L", 10, strDeptCode);

            cboDept.SelectedIndex = cboDept.Items.IndexOf(strDeptCode.Trim());

            Cursor.Current = Cursors.WaitCursor;

            FstrReserved = "";
            FstrRDept = strDeptCode;
            FstrRDoct = (clsOrdFunction.Pat.DrCode).Trim();
            FstrYDate = "";
            FstrROWID = "";

            try
            {
                SQL = "";
                //'원무과 예약 Table에 자료가 있는지 Check
                SQL = "SELECT DeptCode,DrCode,TO_CHAR(DATE3,'YYYY-MM-DD HH24:MI') RTime,ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
                SQL = SQL + ComNum.VBLF + "WHERE Pano='" + clsOrdFunction.Pat.PtNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND DeptCode='" + strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "  AND Date3>=TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + "  AND TRANSDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "  AND RETDATE IS NULL ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    FstrReserved = "OK";
                    FstrRDoct = dt.Rows[0]["DrCode"].ToString().Trim();
                    FstrYDate = dt.Rows[0]["RTime"].ToString().Trim();
                    FstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (FstrReserved == "OK")     //'기수납한 예약자료 수정 못하게
                {
                    btnRegist.Enabled = false;
                    btnCancel.Enabled = false;
                    btnChange.Enabled = true;
                    lblStat.Text = "수납";
                }
                else
                {
                    //'입원예약이 있는지 Check
                    SQL = "SELECT TO_CHAR(RDate,'YYYY-MM-DD HH24:MI') RTime,DrCode,GbSunap,ROWID ";
                    SQL = SQL + ComNum.VBLF + " FROM ADMIN.OCS_RESERVED ";
                    SQL = SQL + ComNum.VBLF + "WHERE Pano='" + clsOrdFunction.Pat.PtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND DeptCode='" + strDeptCode + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND Gbsunap ='0' ";// '2013-10-04;
                    SQL = SQL + ComNum.VBLF + "  AND BDate>=TRUNC(SYSDATE-5) ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        FstrReserved = "YEYAK";
                        FstrRDoct = dt.Rows[0]["DrCode"].ToString().Trim();
                        FstrYDate = dt.Rows[0]["RTime"].ToString().Trim();
                        FstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                        lblStat.Text = "미수납";
                    }

                    dt.Dispose();
                    dt = null;
                }


                lblRTime.Text = FstrYDate;
                FstrDate3 = FstrYDate;
                // '재원과의 의사를 Display
                SQL = "SELECT DrCode,DrName ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + "WHERE DrDept1 = '" + (strDeptCode).Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "  AND TOUR <> 'Y' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY PrintRanking ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    j = 0;

                    strData = VB.Left(dt.Rows[i]["DrCode"].ToString().Trim(), 4) + ComNum.VBLF + ".";
                    strData = strData + dt.Rows[i]["DrName"].ToString().Trim();
                    cboDoct.Items.Add(strData);

                    if (FstrRDoct == dt.Rows[i]["DrCode"].ToString().Trim())
                    {
                        //ComFunc.ComboFind(cboDept, "L", 10, dt.Rows[i]["DrCode"].ToString().Trim());
                        j = i;
                    }
                }
                dt.Dispose();
                dt = null;

                cboDoct.SelectedIndex = j;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        private void btnYeyak_Click(object sender, EventArgs e)
        {
            btnYeyak_Click(((Button)sender));
        }

        private void SS1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int nRow = 0;
            string strRTime = "";
            string strTime1 = "";
            string strTime2 = "";
            int nYeyakInwon = 0;
            string strTel = "";
            string strHTEL = "";
            string strAMPM = "";


            if (e.ColumnHeader == true || e.RowHeader == true)
                return;

            // '예약자 조회할 시각을 설정
            strRTime = SS1_Sheet1.Cells[0, e.Column].Text;

            if (e.Row < 6)
            {
                strTime1 = VB.Left(strRTime, 10) + " 00:00";
                strTime2 = VB.Left(strRTime, 10) + " 23:59";
            }
            else if (e.Row == 5 || e.Row == 4)
            {
                strTime1 = VB.Left(strRTime, 10) + " 00:00";
                strTime2 = VB.Left(strRTime, 10) + " " + FstrRTime[e.Row - 4];
            }
            else if (e.Row == SS1_Sheet1.RowCount)
            {
                strTime1 = VB.Left(strRTime, 10) + " " + FstrRTime[e.Row - 5];
                strTime2 = VB.Left(strRTime, 10) + " 23:59";
            }
            else
            {
                strTime1 = VB.Left(strRTime, 10) + " " + FstrRTime[e.Row - 5];
                strTime2 = VB.Left(strRTime, 10) + " " + FstrRTime[e.Row - 4];
            }

            //'2013-03-26
            if (VB.Left(cboDoct.Text, 4) == "0107" && CF.READ_YOIL(clsDB.DbCon, strRTime) == "화요일" && string.Compare(VB.Right(strTime1, 5), "10:30") <= 0)
            {
                ComFunc.MsgBox("박준모과장 화요일은 10시30전 예약 안됨!!", "내과문의");
                return;
            }

            SS2_Sheet1.RowCount = 0;

            //'해당 시각대 예약자를 SELECT

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                // '해당 시각대 예약자를 SELECT
                SQL = "";
                SQL = "SELECT a.Pano,b.SName,b.Tel,a.DeptCode,c.DrName,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(a.Date3,'MM/DD HH24:MI') RDate,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(a.Date3,'YYYY-MM-DD HH24:MI') RDate2,";
                SQL = SQL + ComNum.VBLF + "      b.Juso,b.Sex,b.ZipCode1,b.ZipCode2 ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW a," + ComNum.DB_PMPA + "BAS_PATIENT b,";
                SQL = SQL + ComNum.VBLF + "      " + ComNum.DB_PMPA + "BAS_DOCTOR c ";
                SQL = SQL + ComNum.VBLF + "WHERE a.Date3>=TO_DATE('" + strTime1 + "','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "  AND a.Date3< TO_DATE('" + strTime2 + "','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "  AND a.DrCode='" + VB.Left(cboDoct.Text, 4) + "' ";
                SQL = SQL + ComNum.VBLF + "  AND A.TRANSDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "  AND A.RETDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "  AND a.Pano=b.Pano(+) ";
                SQL = SQL + ComNum.VBLF + "  AND a.DrCode=c.DrCode(+) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY a.Date3,a.Pano ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                SS2_Sheet1.RowCount = dt.Rows.Count;

                if (dt.Rows.Count > 0)
                {
                    nRow = 0;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nRow = nRow + 1;
                        if (nRow > SS2_Sheet1.RowCount)
                        {
                            SS2_Sheet1.RowCount = nRow;
                        }
                        SS2_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["DrName"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["RDate"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 5].Text = "예약";
                        SS2_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["Tel"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["RDate2"].ToString().Trim();

                        //  '우편번호로 주소를 READ
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT MailJuso ";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_MAILNEW ";
                        SQL = SQL + ComNum.VBLF + "WHERE MailCode='" + dt.Rows[i]["ZipCode1"].ToString().Trim() + dt.Rows[i]["ZipCode2"].ToString().Trim() + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            SS2_Sheet1.Cells[nRow - 1, 8].Text = dt1.Rows[0]["MailJuso"].ToString().Trim() + " " + dt.Rows[i]["JUSO"].ToString().Trim();
                        }
                        else
                        {
                            SS2_Sheet1.Cells[nRow - 1, 8].Text = dt.Rows[i]["JUSO"].ToString().Trim();
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }
                }
                dt.Dispose();
                dt = null;

                //'해당 시각대 전화접수자를 SELECT
                SQL = "";
                SQL = "SELECT a.Pano,b.SName,b.Tel,a.DeptCode,c.DrName,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(a.RDate,'YYYY-MM-DD') RDate,RTime,";
                SQL = SQL + ComNum.VBLF + "      b.Juso,b.Sex,b.ZipCode1,b.ZipCode2 ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_TELRESV a," + ComNum.DB_PMPA + "BAS_PATIENT b,";
                SQL = SQL + ComNum.VBLF + "      " + ComNum.DB_PMPA + "BAS_DOCTOR c ";
                SQL = SQL + ComNum.VBLF + "WHERE a.RDate=TO_DATE('" + VB.Left(strTime1, 10) + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND a.RTime>='" + VB.Right(strTime1, 5) + "' ";
                SQL = SQL + ComNum.VBLF + "  AND a.RTime< '" + VB.Right(strTime2, 5) + "' ";
                SQL = SQL + ComNum.VBLF + "  AND a.DrCode='" + VB.Left(cboDoct.Text, 4) + "' ";
                SQL = SQL + ComNum.VBLF + "  AND a.Pano=b.Pano(+) ";
                SQL = SQL + ComNum.VBLF + "  AND a.DrCode=c.DrCode(+) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY a.RDate,a.RTime,a.Pano ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nRow = nRow + 1;
                        if (nRow > SS2_Sheet1.RowCount)
                        {
                            SS2_Sheet1.RowCount = nRow;
                        }

                        SS2_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["DrName"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 4].Text = VB.Right(dt.Rows[i]["RDate"].ToString().Trim(), 5) + " " + dt.Rows[i]["RTime"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 5].Text = "전화";
                        SS2_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["Tel"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["RDate"].ToString().Trim() + " " + dt.Rows[i]["RTime"].ToString().Trim();

                        //'우편번호로 주소를 READ
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT MailJuso ";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_MAILNEW ";
                        SQL = SQL + ComNum.VBLF + "WHERE MailCode='" + dt.Rows[i]["ZipCode1"].ToString().Trim() + dt.Rows[i]["ZipCode2"].ToString().Trim() + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            SS2_Sheet1.Cells[nRow - 1, 8].Text = dt1.Rows[0]["MailJuso"].ToString().Trim() + " " + dt.Rows[i]["JUSO"].ToString().Trim();
                        }
                        else
                        {
                            SS2_Sheet1.Cells[nRow - 1, 8].Text = dt.Rows[i]["JUSO"].ToString().Trim();
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }
                }
                dt.Dispose();
                dt = null;

                SS2_Sheet1.RowCount = nRow;

                // '예약이 가능한 시각인지 Check
                if (e.Row >= 5)
                {
                    strRTime = SS1_Sheet1.Cells[0, e.Column].Text;
                    strRTime = strRTime + " " + SS1_Sheet1.Cells[e.Row, 0].Text;
                    nYeyakInwon = (int)VB.Val(SS1_Sheet1.Cells[e.Row, e.Column].Text);

                    // '예약 불가능 시간대
                    if (SS1_Sheet1.Cells[e.Row, e.Column].BackColor == System.Drawing.Color.FromArgb(205, 250, 220))
                    {
                        clsPublic.GstrMsgList = strRTime + "은 예약이 불가능한 시간 입니다." + ComNum.VBLF;
                        clsPublic.GstrMsgList = clsPublic.GstrMsgList + "스케쥴에 오류가 있으면 심전도실(☏534)에" + ComNum.VBLF;
                        clsPublic.GstrMsgList = clsPublic.GstrMsgList + "통보하여 스케쥴을 수정 바랍니다." + ComNum.VBLF;

                        ComFunc.MsgBox(clsPublic.GstrMsgList, "오류");
                    }
                    else if (e.Row <= FnMorningNo && nYeyakInwon <= FnResvInwon)
                    {
                        lblRTime.Text = strRTime;
                        //'해당 예약일자의 Cell Backcolor를 변경

                        if (FnSelRow != 0)
                        {
                            SS1_Sheet1.Cells[FnSelRow - 1, FnSelCol - 1].BackColor = System.Drawing.Color.FromArgb(205, 250, 220);
                        }
                        SS1_Sheet1.Cells[e.Row, e.Column].BackColor = System.Drawing.Color.FromArgb(255, 250, 0);

                        FnSelRow = e.Row;
                        FnSelCol = e.Column;
                    }
                    else if (e.Row > FnMorningNo && nYeyakInwon <= FnResvInwon2)
                    {
                        lblRTime.Text = strRTime;
                        //'해당 예약일자의 Cell Backcolor를 변경
                        if (FnSelRow != 0)
                        {
                            SS1_Sheet1.Cells[FnSelRow - 1, FnSelCol - 1].BackColor = System.Drawing.Color.FromArgb(205, 250, 220);
                        }
                        SS1_Sheet1.Cells[e.Row, e.Column].BackColor = System.Drawing.Color.FromArgb(255, 250, 0);

                        FnSelRow = e.Row;
                        FnSelCol = e.Column;
                    }
                    else
                    {
                        if (e.Row <= FnMorningNo)
                        {
                            clsPublic.GstrMsgList = cboDoct.Text + " 과장님은 예약단위당(오전) " + FnResvInwon + "명이 가능합니다" + ComNum.VBLF;
                            clsPublic.GstrMsgList = clsPublic.GstrMsgList + strRTime + "은 예약인원 초과입니다." + ComNum.VBLF;
                            clsPublic.GstrMsgList = clsPublic.GstrMsgList + "다른 예약일시를 선택하십시오.";
                        }
                        else
                        {
                            clsPublic.GstrMsgList = cboDoct.Text + " 과장님은 예약단위당(오후) " + FnResvInwon2 + "명이 가능합니다" + ComNum.VBLF;
                            clsPublic.GstrMsgList = clsPublic.GstrMsgList + strRTime + "은 예약인원 초과입니다." + ComNum.VBLF;
                            clsPublic.GstrMsgList = clsPublic.GstrMsgList + "다른 예약일시를 선택하십시오.";
                        }

                        ComFunc.MsgBox(clsPublic.GstrMsgList);
                    }
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void SS2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strPano = "";
            string strSName = "";
            string strRTime = "";

            strPano = SS2_Sheet1.Cells[e.Row, 0].Text;
            strSName = SS2_Sheet1.Cells[e.Row, 1].Text;
            strRTime = SS2_Sheet1.Cells[e.Row, 9].Text;

            if (SS2_Sheet1.Cells[e.Row, 5].Text == "전화")
            {
                ComFunc.MsgBox("전화예약은 변경이 불가능 합니다", "오류");
                return;
            }

            clsPublic.GstrMsgList = strPano + " " + strSName + "님의 예약을 변경하겠습니까?";

            if (ComFunc.MsgBoxQ(clsPublic.GstrMsgList, "예약변경", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            FstrDate3 = "2003-" + VB.Left(SS2_Sheet1.Cells[e.Row, 4].Text, 2) + "-" + VB.Mid(SS2_Sheet1.Cells[e.Row, 4].Text, 4, 2);

            txtPtno.Text = strPano;
            lblName.Text = strSName;
            lblStat.Text = "수납";
            btnRegist.Enabled = false;
            btnCancel.Enabled = false;
            btnChange.Enabled = true;
            lblRTime.Text = strRTime;
        }
    }
}

