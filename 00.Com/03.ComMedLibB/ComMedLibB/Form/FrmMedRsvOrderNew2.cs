using ComBase; //기본 클래스
using FarPoint.Win;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComMedLibB
{
    /// <summary>
    /// Class Name      : MedIpdNr
    /// File Name       : FrmMedRsvOrderNew2.cs
    /// Description     : 진료예약(New)
    /// Author          : 박창욱
    /// Create Date     : 2018-02-14
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\Ocs\OpdOcs\Oorder\Oorder83_new2.frm(FrmReservedNew2.frm) >> FrmMedRsvOrderNew2.cs 폼이름 재정의" />	
    public partial class FrmMedRsvOrderNew2 : Form
    {
        string SQL = "";
        DataTable dt = null;
        DataTable dt1 = null;
        string SqlErr = "";
        int i = 0;
        int intRowAffected = 0;
        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
        ComFunc CF = new ComFunc();

        int FnResvTimeGbn = 0;
        int FnResvInwon = 0;
        int FnResvInwon2 = 0;
        int FnSelRow = 0;
        int FnSelCol = 0;
        int FnMorningNo = 0;
        string FstrDrCode = "";
        string FstrSMS = "";  //sms 동의
        string FstrSMS_DRUG = "";  //sms 안부분자(약)
        string FstrRdate_old = "";
        string[] FstrRTime = new string[101];
        string[] FstrRDate = new string[201];

        public FrmMedRsvOrderNew2()
        {
            InitializeComponent();
        }

        //의사별 예약, 전화예약 인원 및 스케쥴을 표시
        private void Yeyak_Inwon_Display_New(string ArgDrCode)
        {
            int j = 0;
            int K = 0;
            int nRow = 0;
            int nCol = 0;
            int nTime = 0;
            int nRead = 0;
            int inx1 = 0;
            int inx2 = 0;
            int nTimeCNT = 0;
            int nDay = 0;
            int nROW1 = 0;  //소계1
            int nROW2 = 0;  //소계2
            double nSum1 = 0;
            double nSum2 = 0;
            double nSum3 = 0;
            double nSum4 = 0;
            string strRDate = "";
            string strRTime = "";
            string strETime = "";
            string strGDate = "";
            string strAmTime = "";
            string strAmTime2 = "";
            string strPmTime = "";
            string strPmTime2 = "";
            string strYoil = "";
            string strDeptCode = "";
            int[,,] nCnt = new int[201, 201, 6];

            Cursor.Current = Cursors.WaitCursor;

            this.Text = "진료예약(" + cboDoctor.Text + ")";

            for (i = 0; i < 201; i++)
            {
                for (j = 0; j < 201; j++)
                {
                    for (K = 0; K < 6; K++)
                    {
                        nCnt[i, j, K] = 0;
                    }
                }
            }

            strGDate = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).AddDays(VB.Val(txtRIlsu.Text)).ToString("yyyy-MM-dd");

            nDay = (int)VB.Val(txtRIlsu.Text);

            try
            {
                strAmTime = "";
                strPmTime = "";
                lblPaYinwon.Text = "";
                //'예약시간단위 및 단위시간당 인원을 READ
                SQL = "";
                SQL = "SELECT DrDept1,DrCode,DrName,YTimeGbn,YInwon,AmTime,PmTime,YInwon2,AmTime2,PmTime2 ";
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
                    lblPaYinwon.Text = "예약인원(오전:" + FnResvInwon + "명 오후:" + FnResvInwon2 + "명)";
                    strDeptCode = dt.Rows[0]["DrDept1"].ToString().Trim();
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

                for (i = 1; i < 101; i++)
                {
                    FstrRTime[i] = "";
                }
                for (i = 1; i < 101; i++)
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

                //if (strAmTime2 == "" || strPmTime2 == "")
                //{
                //    ComFunc.MsgBox("스케쥴 내역이 존재 하지 않습니다");
                //    return;
                //}

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

                nRow += 1;
                nROW1 = nRow;

                SS1_Sheet1.Cells[nRow - 1, 0].Text = "소계";

                for (i = CF.TIME_MI(strPmTime); i <= CF.TIME_MI(strPmTime2); i += nTime)
                {
                    FstrRTime[j] = CF.TIME_MI_TIME(i);
                    nRow = nRow + 1;
                    if (nRow > SS1_Sheet1.RowCount)
                    {
                        SS1_Sheet1.RowCount = nRow;
                    }
                    SS1_Sheet1.Cells[nRow - 1, 0].Text = FstrRTime[j];
                    j = j + 1;
                }

                nRow += 1;
                nROW2 = nRow;

                SS1_Sheet1.Cells[nRow - 1, 0].Text = "소계";

                nTimeCNT = nRow - 6;
                FstrRTime[nTimeCNT] = "23:59";

                //예약 스케쥴을 읽어 SHEET의 상단에 Display
                SQL = "";
                SQL = "SELECT TO_CHAR(SchDate,'YYYY-MM-DD') SchDate,TO_CHAR(SchDate,'DY') Yoil,GbDay,GbJin, GbJin2 ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE ";
                SQL = SQL + ComNum.VBLF + "WHERE DrCode='" + ArgDrCode + "' ";
                SQL = SQL + ComNum.VBLF + "  AND SchDate>TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + "  AND SchDate<=TO_DATE('" + strGDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SchDate ";

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

                    //Sheet Clear & SET, 줄그리기, Cell Type을 설정

                    SS1_Sheet1.Rows.Count = nRow;
                    SS1_Sheet1.Cells[5, 1, SS1_Sheet1.RowCount - 1, SS1_Sheet1.ColumnCount - 1].Text = "";
                    SS1_Sheet1.Cells[5, 1, SS1_Sheet1.RowCount - 1, SS1_Sheet1.ColumnCount - 1].BackColor = System.Drawing.Color.FromArgb(255, 255, 255);

                    SS1_Sheet1.Cells[0, 1, SS1_Sheet1.RowCount - 1, SS1_Sheet1.ColumnCount - 1].Border = new LineBorder(Color.Black, 0);
                   
                    //스케쥴을 SHEET 상단에 표시함
                    nCol = 1;

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

                //의사별 기타 스케쥴을 읽어 Sheet에 표시함
                SQL = "";
                SQL = "SELECT TO_CHAR(SchDate,'YYYY-MM-DD') SchDate,STime,ETime ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE_ETC ";
                SQL = SQL + ComNum.VBLF + "WHERE DrCode='" + ArgDrCode + "' ";
                SQL = SQL + ComNum.VBLF + "  AND SchDate>TRUNC(SYSDATE)";
                SQL = SQL + ComNum.VBLF + "  AND SchDate<=TO_DATE('" + strGDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SchDate,STime ";

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
                        for (j = 1; j <= 200; j++)
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
                SQL = "SELECT TO_CHAR(Date3,'YYYY-MM-DD HH24:MI') RTime,GWACHOJAE,COUNT(*) CNT ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
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

                        //예약일자
                        inx1 = 0;
                        for (j = 1; j <= 200; j++)
                        {
                            if (strRDate == FstrRDate[j])
                            {
                                inx1 = j;
                                break;
                            }
                        }

                        //예약시간 ROW
                        inx2 = 0;
                        for (K = 1; K <= nTimeCNT; K++)
                        {
                            if (string.Compare(strRTime, FstrRTime[K + 1]) < 0)
                            {
                                inx2 = K;
                                if (string.Compare(strPmTime, strRTime) <= 0)
                                {
                                    inx2 = K + 1;
                                }
                                break;
                            }
                        }

                        if (inx1 > 0 && inx2 > 0)
                        {
                            nCnt[inx1, inx2, 1] = nCnt[inx1, inx2, 1] + Convert.ToInt32(dt1.Rows[i]["CNT"].ToString().Trim());

                            if (dt1.Rows[i]["GWACHOJAE"].ToString().Trim() == "C")
                            {
                                nCnt[inx1, inx2, 3] += Convert.ToInt32(dt1.Rows[i]["Cnt"].ToString().Trim());
                            }

                            if (chkYoil.Checked == true)
                            {
                                clsOcsRsv.READ_FM_CHOJAE_INWON_Time_Yoil(clsDB.DbCon, strGDate, clsPublic.GstrDeptCode, ArgDrCode, strRTime, strYoil);
                            }
                            else
                            {
                                clsOcsRsv.READ_FM_CHOJAE_INWON_Time(clsDB.DbCon, strGDate, clsPublic.GstrDeptCode, ArgDrCode, strRTime);
                            }

                            nCnt[inx1, inx2, 4] = clsPublic.GnRInWon_Cho;
                            nCnt[inx1, inx2, 5] = clsPublic.GnRInWon_Jae;
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

                        //예약일자
                        inx1 = 0;
                        for (j = 1; j <= 100; j++)
                        {
                            if (strRDate == FstrRDate[j])
                            {
                                inx1 = j;
                                break;
                            }
                        }
                        //예약시간 ROW
                        inx2 = 0;
                        for (K = 1; K <= nTimeCNT; K++)
                        {
                            if (string.Compare(strRTime, FstrRTime[K + 1]) < 0)
                            {
                                inx2 = K;
                                if (string.Compare(strPmTime, strRTime) <= 0)
                                {
                                    inx2 = K + 1;
                                }
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

                //전화접수 인원을 COUNT
                SQL = "";
                SQL = "SELECT TO_CHAR(RDate,'YYYY-MM-DD') RDate,RTime,GWACHOJAE,COUNT(*) CNT ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_TELRESV ";
                SQL = SQL + ComNum.VBLF + "WHERE RDate> TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + "  AND RDate<=TO_DATE('" + strGDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND DrCode='" + ArgDrCode + "' ";
                SQL = SQL + ComNum.VBLF + "GROUP BY RDate,RTime,GWACHOJAE ";
                SQL = SQL + ComNum.VBLF + "ORDER BY RDate,RTime,GWACHOJAE ";

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

                        for (j = 1; j <= 200; j++)
                        {
                            if (strRDate == FstrRDate[j])
                            {
                                inx1 = j;
                                break;
                            }
                        }

                        //'예약시간 ROW
                        inx2 = 0;
                        for (K = 1; K <= nTimeCNT; K++)
                        {
                            if (string.Compare(strRTime, FstrRTime[K + 1]) < 0)
                            {
                                inx2 = K;

                                if (string.Compare(strPmTime, strRTime) <= 0)
                                {
                                    inx2 = K + 1;
                                }

                                break;
                            }
                        }

                        if (inx1 > 0 && inx2 > 0)
                        {
                            nCnt[inx1, inx2, 2] = Convert.ToInt32(nCnt[inx1, inx2, 2] + dt1.Rows[i]["CNT"].ToString().Trim());

                            if (dt1.Rows[i]["GWACHOJAE"].ToString().Trim() == "C")
                            {
                                nCnt[inx1, inx2, 3] += Convert.ToInt32(dt1.Rows[i]["Cnt"].ToString().Trim());
                            }

                            if (chkYoil.Checked == true)
                            {
                                clsOcsRsv.READ_FM_CHOJAE_INWON_Time_Yoil(clsDB.DbCon, strGDate, clsPublic.GstrDeptCode, ArgDrCode, strRTime, strYoil);
                            }
                            else
                            {
                                clsOcsRsv.READ_FM_CHOJAE_INWON_Time(clsDB.DbCon, strGDate, clsPublic.GstrDeptCode, ArgDrCode, strRTime);
                            }

                            nCnt[inx1, inx2, 4] = clsPublic.GnRInWon_Cho;
                            nCnt[inx1, inx2, 5] = clsPublic.GnRInWon_Jae;
                        }
                    }
                }
                dt1.Dispose();
                dt1 = null;

                //자료를 SHEET에 Display
                for (i = 2; i <= 200; i++)
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

                for (i = 2; i <= SS1_Sheet1.GetLastNonEmptyColumn(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
                {
                    nSum1 = 0;
                    nSum2 = 0;
                    nSum3 = 0;
                    nSum4 = 0;

                    for (j = 6; j <= SS1_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); j++)
                    {
                        if (SS1_Sheet1.Cells[j - 1, i - 1].Text.Trim() != "")
                        {
                            if (nROW1 > j)
                            {
                                //소계1
                                nSum1 += VB.Val(VB.Pstr(SS1_Sheet1.Cells[i - 1, j - 1].Text.Trim(), "(", 1));
                                nSum2 += VB.Val(VB.Pstr(VB.Pstr(SS1_Sheet1.Cells[i - 1, j - 1].Text.Trim(), "(", 2), ")", 1));
                            }
                            else
                            {
                                //소계2
                                nSum3 += VB.Val(VB.Pstr(SS1_Sheet1.Cells[i - 1, j - 1].Text.Trim(), "(", 1));
                                nSum4 += VB.Val(VB.Pstr(VB.Pstr(SS1_Sheet1.Cells[i - 1, j - 1].Text.Trim(), "(", 2), ")", 1));
                            }
                        }
                    }

                    SS1_Sheet1.Cells[nROW1 - 1, i - 1].Text = nSum1 + "(" + nSum2 + ")";
                    SS1_Sheet1.Cells[nROW2 - 1, i - 1].Text = nSum3 + "(" + nSum4 + ")";
                }

                SS1_Sheet1.Rows[nROW1 - 1].BackColor = Color.FromArgb(255, 255, 98);
                SS1_Sheet1.Rows[nROW2 - 1].BackColor = Color.FromArgb(255, 255, 98);

                FnSelRow = 0;
                FnSelCol = 0;

                Cursor.Current = Cursors.Default;
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

        private void chkSMS_CheckedChanged(object sender, EventArgs e)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                if (chkSMS.Checked == true)
                {
                    SQL = "UPDATE " + ComNum.DB_PMPA + "BAS_PATIENT SET GBSMS = 'Y' WHERE PANO = '" + txtPano.Text + "'";
                }
                else
                {
                    SQL = "UPDATE " + ComNum.DB_PMPA + "BAS_PATIENT SET GBSMS = 'N' WHERE PANO = '" + txtPano.Text + "'";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }


                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
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

        private void chkSMSDrug_CheckedChanged(object sender, EventArgs e)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                if (chkSMSDrug.Checked == true)
                {
                    SQL = "UPDATE " + ComNum.DB_PMPA + "BAS_PATIENT SET GBSMS_DRUG = '*' WHERE PANO = '" + txtPano.Text + "'";
                }
                else
                {
                    SQL = "UPDATE " + ComNum.DB_PMPA + "BAS_PATIENT SET GBSMS_DRUG = '' WHERE PANO = '" + txtPano.Text + "'";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }


                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            clsOrdFunction.Pat.RDATE = "";
            clsOrdFunction.Pat.RTime = "";
            clsOrdFunction.Pat.RDrCode = "";
            clsOrdFunction.Pat.Exam = "";
            clsOrdFunction.Pat.ResMemo = "";
            clsOrdFunction.Pat.ResSMSNot = "";

            clsOrdFunction.GstrRsvCancelFlag = "Y";

            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            clsOrdFunction.GstrRsvCancelFlag = "";
            this.Close();
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            string strRDate = "";
            string strRTime = "";

            if (VB.Len(lblRTime.Text) != 16)
            {
                ComFunc.MsgBox("예약일시를 (YYYY-MM-DD HH:MI) 형태로 입력하세요", "오류");
                return;
            }

            if (cboDoctor.Text == "")
            {
                ComFunc.MsgBox("예약의사가 공란 입니다", "확인");
                return;
            }

            strRDate = VB.Left(lblRTime.Text, 10);
            strRDate = VB.Right(lblRTime.Text, 5);

            clsOrdFunction.Pat.RDATE = Convert.ToDateTime(strRDate).ToString("yyyyMMdd");
            clsOrdFunction.Pat.RTime = strRTime;
            clsOrdFunction.Pat.RDrCode = VB.Left(cboDoctor.Text, 4);
            clsOrdFunction.Pat.ResMemo = txtRemark.Text.Trim();

            if (chkExam.Checked == true)
            {
                clsOrdFunction.Pat.Exam = "Y";
            }
            else
            {
                clsOrdFunction.Pat.Exam = "N";
            }

            clsOrdFunction.Pat.GbChojae = "3";  //재진

            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (VB.Len(lblRTime.Text.Trim()) != 16)
            {
                ComFunc.MsgBox("예약일시를 (YYYY-MM-DD HH:MI)형태로 입력하세요");
                return;
            }

            if (cboDoctor.Text.Trim() == "")
            {
                ComFunc.MsgBox("예약의사가 공란입니다.");
                return;
            }

            if (FstrRdate_old == "")
            {
                ComFunc.MsgBox("예약변경을 다시해주세요.");
                return;
            }

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            ComFunc cf = new ComFunc();

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "UPDATE " + ComNum.DB_PMPA + "OPD_RESERVED_NEW SET ";
                SQL = SQL + ComNum.VBLF + " Date3=TO_DATE('" + lblRTime.Text + "','YYYY-MM-DD HH24:MI'), ";
                SQL = SQL + ComNum.VBLF + " DrCode='" + ComFunc.LeftH(cboDoctor.Text, 4) + "', ";
                if (chkExam.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " EXAM = 'Y'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " EXAM = '' ";
                }
                SQL = SQL + ComNum.VBLF + "WHERE Pano='" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + "  AND DeptCode='" + clsOrdFunction.Pat.DeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "  AND DATE3 >= TO_DATE('" + FstrRdate_old + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "  AND DATE3 <  TO_DATE('" + cf.DATE_ADD(clsDB.DbCon, FstrRdate_old, 1) + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "  AND TRANSDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "  AND RETDATE IS NULL ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    lblRTime.Text = VB.Left(clsOrdFunction.Pat.RDATE, 4) + "-" + VB.Mid(clsOrdFunction.Pat.RDATE, 5, 2) + "-"
                                  + VB.Right(clsOrdFunction.Pat.RDATE, 2) + " " + clsOrdFunction.Pat.RTime;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                if (txtPano.Text == clsOrdFunction.Pat.PtNo)
                {
                    clsOrdFunction.Pat.RDATE = Convert.ToDateTime(VB.Left(lblRTime.Text, 10)).ToString("yyyyMMdd");
                    clsOrdFunction.Pat.RTime = VB.Right(lblRTime.Text, 5);
                    if (chkExam.Checked == true)
                    {
                        clsOrdFunction.Pat.Exam = "Y";
                    }
                    else
                    {
                        clsOrdFunction.Pat.Exam = "N";
                    }
                    ComFunc.MsgBox("예약일자를 정상적으로 변경하였습니다.");
                    this.Close();
                    return;
                }
                else
                {
                    ComFunc.MsgBox(lblName.Text + "님 예약일자를 정상적으로 변경하였습니다.");

                    txtPano.Text = clsOrdFunction.Pat.PtNo;
                    lblName.Text = clsOrdFunction.Pat.sName;
                    lblRTime.Text = "";
                    lblStat.Text = "";

                    if (clsOrdFunction.Pat.RDATE.Trim() != "")
                    {
                        if (clsOrdFunction.GstrReserved == "OK")    //기수납한 예약자료 수정 못하게
                        {
                            btnRegist.Enabled = false;
                            btnCancel.Enabled = false;
                            btnUpdate.Enabled = true;
                            lblStat.Text = "수납";
                        }
                        else if (clsOrdFunction.GstrReserved == "YEYAK")
                        {
                            lblStat.Text = "미수납";
                            btnRegist.Enabled = true;
                            btnCancel.Enabled = true;
                            btnUpdate.Enabled = false;
                        }
                        lblRTime.Text = VB.Left(clsOrdFunction.Pat.RDATE, 4) + "-" + VB.Mid(clsOrdFunction.Pat.RDATE, 5, 2) + "-"
                                      + VB.Right(clsOrdFunction.Pat.RDATE, 2) + " " + clsOrdFunction.Pat.RTime;
                    }
                    else
                    {
                        btnRegist.Enabled = true;
                        btnCancel.Enabled = true;
                        btnUpdate.Enabled = false;
                    }
                }
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

        private void btnViewR_Click(object sender, EventArgs e)
        {
            SearchR_Data();
        }

        void SearchR_Data()
        {
            SS3_Sheet1.RowCount = 0;

            if (txtPanoR.Text == "")
            {
                return;
            }

            txtPanoR.Text = string.Format("{0:00000000}", txtPanoR.Text);

            try
            {
                SQL = "";
                SQL += " SELECT b.SName,b.Pano,a.DeptCode,a.DrCode,a.Ordercode RDate,a.SuCode RTime     \r";
                SQL += "      , ADMIN.FC_BAS_DOCTOR_DRNAME(a.DrCode) DRNAME                        \r";
                SQL += "   FROM ADMIN.OCS_OORDER   a                                               \r";
                SQL += "      , ADMIN.BAS_PATIENT b                                               \r";
                SQL += "  WHERE a.Ptno    = b.Pano(+)                                                   \r";
                SQL += "    AND a.Ptno    = '" + txtPanoR.Text + "'                                     \r";
                SQL += "    AND a.BDate   = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD')       \r";
                SQL += "    AND a.Gbsunap = '0'                                                         \r"; //미수납 - 가예약상태
                SQL += "    AND a.Slipno  = '0000'                                                      \r"; //예약구분
                SQL += "    AND a.GbInfo  = 'N'                                                         \r"; //예약구분
                SQL += "  ORDER BY BDate,SuCode                                                         \r";
                clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SS3.ActiveSheet.Cells[i, 0].Text = "가예약";
                        SS3.ActiveSheet.Cells[i, 1].Text = string.Format("{0:yyyy-MM-dd}", dt.Rows[i]["RDATE"].ToString());
                        SS3.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["RTIME"].ToString();
                        SS3.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["SNAME"].ToString();
                        SS3.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["DEPTCODE"].ToString();
                        SS3.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["DRNAME"].ToString();
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL += " SELECT SName,Pano,DeptCode,DrCode,TO_CHAR(Date3,'YYYY-MM-DD') RDate,TO_CHAR(Date3,'HH24:MI') RTime \r";
                SQL += "      , ADMIN.FC_BAS_DOCTOR_DRNAME(DrCode) DRNAME                                              \r";
                SQL += "   FROM ADMIN.OPD_RESERVED_NEW                                                                \r";
                SQL += "  WHERE Pano = '" + txtPanoR.Text + "'                                                              \r";
                SQL += "    AND Date3 >= TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD')                              \r";
                SQL += "    AND TRANSDATE IS NULL                                                                           \r";
                SQL += "    AND RETDATE IS NULL                                                                             \r";
                SQL += "  ORDER BY Date3,DeptCode                                                                           \r";
                clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SS3.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["RDATE"].ToString();
                        SS3.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["RTIME"].ToString();
                        SS3.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["SNAME"].ToString();
                        SS3.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["DEPTCODE"].ToString();
                        SS3.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["DRNAME"].ToString();
                    }
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

        private void cboDoctor_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strDrCode = "";

            strDrCode = ComFunc.LeftH(cboDoctor.Text, 4);

            Yeyak_Inwon_Display_New(strDrCode);
        }

        private void FrmMedRsvOrderNew2_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
            
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            clsOrdFunction.GstrRsvCancelFlag = "";

            int i = 0;
            int j = 0;
            string strData = "";

            SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            SS1_Sheet1.Rows[0].Visible = false;
            SS1_Sheet1.Rows[1].Visible = false;

            SS2_Sheet1.RowCount = 0;
            SS2_Sheet1.Columns[11].Visible = false; //예약일시

            txtPanoR.Text = clsOrdFunction.Pat.PtNo;

            SearchR_Data();

            btnRegist.Enabled = true;
            btnCancel.Enabled = true;
            btnUpdate.Enabled = false;

            if (clsPublic.GstrDeptCode == "PD")
            {
                txtRIlsu.Text = "60";
            }

            lblInfo.Text = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //cboDoct SET
                SQL = "";
                SQL = "SELECT DrCode,DrName,YTimeGbn,YInwon ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + "WHERE DrDept1 = '" + clsPublic.GstrDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "  AND TOUR <> 'Y' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY PrintRanking ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                cboDoctor.Items.Clear();

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strData = ComFunc.LeftH(dt.Rows[i]["DrCode"].ToString().Trim() + VB.Space(4), 4) + ".";
                        strData += dt.Rows[i]["DrName"].ToString().Trim();
                        cboDoctor.Items.Add(strData);
                        if (clsOrdFunction.GstrDrCode == dt.Rows[i]["Drcode"].ToString().Trim())
                        {
                            j = i;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                cboDoctor.SelectedIndex = j;

                txtPano.Text = clsOrdFunction.Pat.PtNo;
                lblName.Text = clsOrdFunction.Pat.sName;
                lblRTime.Text = "";
                lblStat.Text = "";
                FstrRdate_old = VB.Left(clsOrdFunction.Pat.RDATE, 4) + "-" + VB.Mid(clsOrdFunction.Pat.RDATE, 5, 2) + "-" + VB.Right(clsOrdFunction.Pat.RDATE, 2);

                if (clsOrdFunction.Pat.Exam == "Y")
                {
                    chkExam.Checked = true;
                }
                else
                {
                    chkExam.Checked = false;
                }

                if (clsOrdFunction.Pat.RDATE.Trim() != "")
                {
                    if (clsOrdFunction.GstrReserved == "OK")    //기수납한 예약자료 수정 못하게
                    {
                        btnRegist.Enabled = false;
                        btnCancel.Enabled = false;
                        btnUpdate.Enabled = true;
                        lblStat.Text = "수납";
                    }
                    else if (clsOrdFunction.GstrReserved == "YEYAK")
                    {
                        lblStat.Text = "미수납";
                    }
                    lblRTime.Text = VB.Left(clsOrdFunction.Pat.RDATE, 4) + "-" + VB.Mid(clsOrdFunction.Pat.RDATE, 5, 2) + "-" + VB.Right(clsOrdFunction.Pat.RDATE, 2)
                                  + " " + clsOrdFunction.Pat.RTime;
                }

                //SMS 동의 여부 표시
                FstrSMS = "";
                FstrSMS_DRUG = "";

                SQL = "";
                SQL = "SELECT GBSMS, GBSMS_DRUG";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + txtPano.Text + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    FstrSMS = dt.Rows[0]["gbsms"].ToString().Trim();
                    FstrSMS_DRUG = dt.Rows[0]["gbsms_drug"].ToString().Trim();
                    chkSMS.Checked = FstrSMS == "Y" ? true : false;
                    chkSMSDrug.Checked = FstrSMS_DRUG == "*" ? true : false;
                }

                dt.Dispose();
                dt = null;


                SQL = "";
                SQL = " SELECT TO_CHAR(INDATE,'YYYY-MM-DD') INDATE,GBSTS";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + txtPano.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "  AND ACTDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "   ORDER BY INDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    lblInfo.Text = "입원중 => 입원일자 : " + dt.Rows[0]["INDATE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;


                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void SS1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int nRow = 0;
            string strRTime = "";
            string strTime1 = "";
            string strTime2 = "";
            int nYeyakInwon = 0;

            string strYoil = "";

            int nTemp1 = 0; //초진
            int nTemp2 = 0; //재진

            if (e.ColumnHeader == true || e.RowHeader == true)
            {
                return;
            }

            if (e.Column < 1)
            {
                return;
            }

            txtRemark.Text = "";

            // '예약자 조회할 시각을 설정
            strRTime = SS1_Sheet1.Cells[0, e.Column].Text;

            strYoil = clsVbfunc.GetYoIl(VB.Left(strRTime, 10));

            if (e.Row < 5)
            {
                strTime1 = VB.Left(strRTime, 10) + " 00:00";
                strTime2 = VB.Left(strRTime, 10) + " 23:59";
            }
            else if (e.Row == 4 || e.Row == 3)
            {
                strTime1 = VB.Left(strRTime, 10) + " 00:00";
                strTime2 = VB.Left(strRTime, 10) + " " + FstrRTime[e.Row - 3];
            }
            else if (e.Row == SS1_Sheet1.RowCount - 1)
            {
                strTime1 = VB.Left(strRTime, 10) + " " + FstrRTime[e.Row - 4];
                strTime2 = VB.Left(strRTime, 10) + " 23:59";
            }
            else
            {
                if (e.Row > 12)
                {
                    strTime1 = VB.Left(strRTime, 10) + " " + FstrRTime[e.Row - 5];
                    strTime2 = VB.Left(strRTime, 10) + " " + FstrRTime[e.Row - 4];
                }
                else
                {
                    strTime1 = VB.Left(strRTime, 10) + " " + FstrRTime[e.Row - 4];
                    strTime2 = VB.Left(strRTime, 10) + " " + FstrRTime[e.Row - 3];
                }
            }

            //예약체크
            SQL = "";
            SQL = "SELECT * FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW";
            SQL = SQL + ComNum.VBLF + "  WHERE PANO ='" + txtPano.Text.Trim() + "' ";
            SQL = SQL + ComNum.VBLF + "  AND DEPTCODE ='" + clsPublic.GstrDeptCode + "' ";
            SQL = SQL + ComNum.VBLF + "  AND TRUNC(DATE3)=TO_DATE('" + strRTime + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "  AND (TRANSDATE IS NULL OR TRANSDATE ='')";
            SQL = SQL + ComNum.VBLF + "  AND (RETDATE IS NULL OR RETDATE ='')";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                ComFunc.MsgBox("이미 해당일자에 예약이 있습니다.");
                dt.Dispose();
                dt = null;
                return;
            }

            dt.Dispose();
            dt = null;

            SS2_Sheet1.RowCount = 0;

            //'해당 시각대 예약자를 SELECT

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                // '해당 시각대 예약자를 SELECT
                SQL = "";
                SQL = "SELECT a.Pano,b.SName,b.Tel,a.DeptCode, A.DRCODE, c.DrName,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(a.Date3,'MM/DD HH24:MI') RDate,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(a.Date3,'YYYY-MM-DD HH24:MI') RDate2, A.EXAM, ";
                SQL = SQL + ComNum.VBLF + "      b.Juso,b.Sex,b.ZipCode1,b.ZipCode2, ";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(L.LASTDATE,'YYYY-MM-DD') LASTDATE, a.BI ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW a,";
                SQL = SQL + ComNum.VBLF + "      " + ComNum.DB_PMPA + "BAS_PATIENT b,";
                SQL = SQL + ComNum.VBLF + "      " + ComNum.DB_PMPA + "BAS_DOCTOR c, ";
                SQL = SQL + ComNum.VBLF + "      " + ComNum.DB_PMPA + "BAS_LASTEXAM L ";
                SQL = SQL + ComNum.VBLF + " WHERE a.Date3>=TO_DATE('" + strTime1 + "','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "   AND a.Date3< TO_DATE('" + strTime2 + "','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "   AND a.DrCode='" + ComFunc.LeftH(cboDoctor.Text, 4) + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.TransDate IS NULL ";
                SQL = SQL + ComNum.VBLF + "   AND a.RetDate IS NULL ";
                SQL = SQL + ComNum.VBLF + "   AND a.Pano=b.Pano(+) ";
                SQL = SQL + ComNum.VBLF + "   AND a.DrCode=c.DrCode(+) ";
                SQL = SQL + ComNum.VBLF + "   AND a.PANO = L.PANO(+) ";
                if (clsPublic.GstrDeptCode == "MD")
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.DRCODE = L.DRCODE(+) ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.DEPTCODE = L.DEPTCODE(+) ";
                }
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

                        if (dt.Rows[i]["LastDate"].ToString().Trim() == "")
                        {
                            SS2_Sheet1.Cells[nRow - 1, 2].Text = "과초진";
                            SS2_Sheet1.Cells[nRow - 1, 2].Text = clsOrdFunction.Read_Dept_Chojae(clsDB.DbCon, dt.Rows[i]["Pano"].ToString().Trim(), dt.Rows[i]["DeptCode"].ToString().Trim(), dt.Rows[i]["DRCode"].ToString().Trim());
                        }

                        SS2_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["EXAM"].ToString().Trim() == "Y" ? "◎" : "";
                        SS2_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["DrName"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["RDate"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 7].Text = "예약";
                        SS2_Sheet1.Cells[nRow - 1, 8].Text = dt.Rows[i]["Tel"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["SEX"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 11].Text = dt.Rows[i]["RDate2"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 12].Text = dt.Rows[i]["Exam"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 13].Text = clsVbfunc.GetBiName(dt.Rows[i]["Bi"].ToString().Trim());

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
                            SS2_Sheet1.Cells[nRow - 1, 10].Text = dt1.Rows[0]["MailJuso"].ToString().Trim() + " " + dt.Rows[i]["JUSO"].ToString().Trim();
                        }
                        else
                        {
                            SS2_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["JUSO"].ToString().Trim();
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }
                }

                dt.Dispose();
                dt = null;

                //'해당 시각대 전화접수자를 SELECT
                SQL = "";
                SQL = "SELECT a.Pano, b.SName, b.Tel, a.DeptCode, c.DrName, a.DrCode,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(a.RDate,'YYYY-MM-DD') RDate, RTime,";
                SQL = SQL + ComNum.VBLF + "      b.Juso, b.Sex, b.ZipCode1, b.ZipCode2 ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_TELRESV a, " + ComNum.DB_PMPA + "BAS_PATIENT b,";
                SQL = SQL + ComNum.VBLF + "      " + ComNum.DB_PMPA + "BAS_DOCTOR c ";
                SQL = SQL + ComNum.VBLF + "WHERE a.RDate=TO_DATE('" + VB.Left(strTime1, 10) + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND a.RTime>='" + VB.Right(strTime1, 5) + "' ";
                SQL = SQL + ComNum.VBLF + "  AND a.RTime< '" + VB.Right(strTime2, 5) + "' ";
                SQL = SQL + ComNum.VBLF + "  AND a.DrCode='" + ComFunc.LeftH(cboDoctor.Text, 4) + "' ";
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
                        SS2_Sheet1.Cells[nRow - 1, 2].Text = clsOrdFunction.Read_Dept_Chojae(clsDB.DbCon, dt.Rows[i]["Pano"].ToString().Trim(), dt.Rows[i]["DeptCode"].ToString().Trim(), dt.Rows[i]["DRCode"].ToString().Trim());
                        SS2_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["DrName"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 6].Text = VB.Right(dt.Rows[i]["RDate"].ToString().Trim(), 5) + " " + dt.Rows[i]["DRCode"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 7].Text = "전화";
                        SS2_Sheet1.Cells[nRow - 1, 8].Text = dt.Rows[i]["Tel"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 11].Text = dt.Rows[i]["RDate"].ToString().Trim() + " " + dt.Rows[i]["RTime"].ToString().Trim();

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
                            SS2_Sheet1.Cells[nRow - 1, 10].Text = dt1.Rows[0]["MailJuso"].ToString().Trim() + " " + dt.Rows[i]["JUSO"].ToString().Trim();
                        }
                        else
                        {
                            SS2_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["JUSO"].ToString().Trim();
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }
                }
                dt.Dispose();
                dt = null;

                SS2_Sheet1.RowCount = nRow;


                if (chkYoil.Checked == true)
                {
                    clsOcsRsv.READ_FM_CHOJAE_INWON_Time_Yoil(clsDB.DbCon, strRTime, clsPublic.GstrDeptCode, clsOrdFunction.GstrDrCode, VB.Right(strTime1, 5), strYoil);
                }
                else
                {
                    clsOcsRsv.READ_FM_CHOJAE_INWON_Time(clsDB.DbCon, strRTime, clsPublic.GstrDeptCode, clsOrdFunction.GstrDrCode, VB.Right(strTime1, 5));
                }

                //시간대별 초재진인원
                nTemp1 = clsPublic.GnRInWon_Cho;
                nTemp2 = clsPublic.GnRInWon_Jae;

                // '예약이 가능한 시각인지 Check
                if (e.Row >= 5)
                {
                    strRTime = SS1_Sheet1.Cells[0, e.Column].Text;
                    strRTime += " " + SS1_Sheet1.Cells[e.Row, 0].Text;
                    nYeyakInwon = (int)VB.Val(SS1_Sheet1.Cells[e.Row, e.Column].Text) + 1;

                    // '예약 불가능 시간대
                    if (SS1_Sheet1.Cells[e.Row, e.Column].BackColor != System.Drawing.Color.FromArgb(205, 250, 220) && SS1_Sheet1.Cells[e.Row, e.Column].BackColor != System.Drawing.Color.FromArgb(255, 119, 187))
                    {
                        clsPublic.GstrMsgList = strRTime + "은 예약이 불가능한 시간 입니다." + ComNum.VBLF;
                        clsPublic.GstrMsgList = clsPublic.GstrMsgList + "스케쥴에 오류가 있으면 심전도실(☏534)에" + ComNum.VBLF;
                        clsPublic.GstrMsgList = clsPublic.GstrMsgList + "통보하여 스케쥴을 수정 바랍니다." + ComNum.VBLF;

                        ComFunc.MsgBox(clsPublic.GstrMsgList, "오류");
                    }
                    else if (e.Row <= FnMorningNo - 1 && nYeyakInwon <= nTemp2)
                    {
                        lblRTime.Text = strRTime;
                        //'해당 예약일자의 Cell Backcolor를 변경

                        if (FnSelRow != 0)
                        {
                            SS1_Sheet1.Cells[FnSelRow, FnSelCol].BackColor = System.Drawing.Color.FromArgb(205, 250, 220);
                        }
                        SS1_Sheet1.Cells[e.Row, e.Column].BackColor = System.Drawing.Color.FromArgb(255, 255, 0);

                        FnSelRow = e.Row;
                        FnSelCol = e.Column;
                    }
                    else if (e.Row > FnMorningNo - 1 && nYeyakInwon <= nTemp2)
                    {
                        lblRTime.Text = strRTime;
                        //'해당 예약일자의 Cell Backcolor를 변경
                        if (FnSelRow != 0)
                        {
                            SS1_Sheet1.Cells[FnSelRow, FnSelCol].BackColor = System.Drawing.Color.FromArgb(205, 250, 220);
                        }
                        SS1_Sheet1.Cells[e.Row, e.Column].BackColor = System.Drawing.Color.FromArgb(255, 255, 0);

                        FnSelRow = e.Row;
                        FnSelCol = e.Column;
                    }
                    else
                    {
                        if (e.Row <= FnMorningNo - 1)
                        {
                            clsPublic.GstrMsgList = cboDoctor.Text + " 과장님은 예약단위당(오전) 초진" + nTemp1 + " 재진" + nTemp2 + "명이 가능합니다" + ComNum.VBLF;
                            clsPublic.GstrMsgList = clsPublic.GstrMsgList + strRTime + "은 예약인원 초과입니다." + ComNum.VBLF;
                            clsPublic.GstrMsgList = clsPublic.GstrMsgList + "다른 예약일시를 선택하십시오.";
                        }
                        else
                        {
                            clsPublic.GstrMsgList = cboDoctor.Text + " 과장님은 예약단위당(오후) 초진" + nTemp1 + " 재진" + nTemp2 + "명이 가능합니다" + ComNum.VBLF;
                            clsPublic.GstrMsgList = clsPublic.GstrMsgList + strRTime + "은 예약인원 초과입니다." + ComNum.VBLF;
                            clsPublic.GstrMsgList = clsPublic.GstrMsgList + "다른 예약일시를 선택하십시오.";
                        }

                        if (clsPublic.GstrFM_Only == "Y" && ComFunc.LeftH(cboDoctor.Text, 4) == "1404")
                        {
                            if (ComFunc.MsgBoxQ(clsPublic.GstrMsgList + ComNum.VBLF + "예약인원을 무시하고 예약하시겠습니까?") == DialogResult.Yes)
                            {
                                lblRTime.Text = strRTime;

                                if (FnSelRow != 0)
                                {
                                    SS1_Sheet1.Cells[FnSelRow, FnSelCol].BackColor = Color.FromArgb(205, 250, 220);
                                }
                                SS1_Sheet1.Cells[e.Row, e.Column].BackColor = Color.FromArgb(255, 255, 0);
                                FnSelRow = e.Row;
                                FnSelCol = e.Column;
                            }
                        }
                        else
                        {
                            ComFunc.MsgBox(clsPublic.GstrMsgList);
                        }
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
            string strExam = "";

            strPano = SS2_Sheet1.Cells[e.Row, 0].Text.Trim();
            strSName = SS2_Sheet1.Cells[e.Row, 1].Text.Trim();
            strRTime = SS2_Sheet1.Cells[e.Row, 11].Text.Trim();
            strExam = SS2_Sheet1.Cells[e.Row, 12].Text.Trim();
            if (SS2_Sheet1.Cells[e.Row, 7].Text == "전화")
            {
                ComFunc.MsgBox("전화예약은 변경이 불가능합니다.");
                return;
            }

            clsPublic.GstrMsgList = strPano + " " + strSName + "님의 예약을 변경하시겠습니까?";
            if (ComFunc.MsgBoxQ(clsPublic.GstrMsgList) == DialogResult.No)
            {
                return;
            }

            FstrRdate_old = "2007-" + VB.Left(SS2_Sheet1.Cells[e.Row, 6].Text, 2) + "-" + VB.Mid(SS2_Sheet1.Cells[e.Row, 6].Text, 4, 2);

            txtPano.Text = strPano;
            lblName.Text = strSName;
            lblStat.Text = "수납";
            if (strExam == "Y")
            {
                chkExam.Checked = true;
            }
            btnRegist.Enabled = false;
            btnCancel.Enabled = false;
            btnUpdate.Enabled = true;
            lblRTime.Text = strRTime;
        }

        private void txtPanoR_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnViewR.Focus();
            }
        }

        private void txtRIlsu_KeyDown(object sender, KeyEventArgs e)
        {
            string strDrCode = "";

            if (e.KeyCode == Keys.Enter)
            {
                strDrCode = ComFunc.LeftH(cboDoctor.Text, 4);
                Yeyak_Inwon_Display_New(strDrCode);
            }
        }
    }
}
