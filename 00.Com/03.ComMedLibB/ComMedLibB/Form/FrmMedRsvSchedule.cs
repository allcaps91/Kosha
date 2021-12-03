using ComBase;
using FarPoint.Win;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComMedLibB
{
    /// <summary>
    /// Description : 예약스케쥴조회
    /// Author : 이상훈
    /// Create Date : 2018.01.17
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref="Frm예약스케쥴조회OCS.frm"/>
    public partial class FrmMedRsvSchedule : Form
    {
        string strDrCode;

        string SQL;
        DataTable dt = null;
        DataTable dt1 = null;
        DataTable dt2 = null;
        string SqlErr = "";     //에러문 받는 변수
        int intRowAffected = 0; //변경된 Row 받는 변수

        string[] FstrRTime = new string[100];
        string[] FstrRDate = new string[200];
        int nResvTimeGbn;
        int nResvInwon;
        int nResvInwon2;
        int nSelRow;
        int nSelCol;
        string strRdate_old;
        int nMorningNo;

        clsSpread SP = new clsSpread();
        ComFunc CF = new ComFunc();

        public FrmMedRsvSchedule()
        {
            InitializeComponent();
        }

        public FrmMedRsvSchedule(string sDrCode)
        {
            InitializeComponent();

            strDrCode = sDrCode;
        }

        private void FrmMedRsvSchedule_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "N", "Y"); //폼 기본값 세팅 등

            fn_Yeyak_Inwon_Display_New(strDrCode);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            fn_Yeyak_Inwon_Display_New(strDrCode);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 의사별 예약,전화예약 인원 및 스케쥴을 표시
        /// </summary>
        /// <param name="sDrCode"></param>
        void fn_Yeyak_Inwon_Display_New(string sDrCode)
        {
            int nRow = 0;
            int nCol = 0;
            int nTime = 0;
            int kk;
            int nREAD = 0;
            int inx1 = 0;
            int inx2 = 0;
            string strRDate = "";
            string strRTime = "";
            string strETime = "";
            string strGDate = "";
            string strAmTime = "";
            string strAMTime2 = "";
            string strPmTime = "";
            string strPMTime2 = "";
            int nTimeCNT;
            int[,,] nCNT = new int[102, 102, 3];
            string strDeptCode = "";
            
            this.Text = "진료예약(" + clsVbfunc.GetBASDoctorName(clsDB.DbCon, sDrCode) + ")";
            strGDate = DateTime.Parse(clsPublic.GstrSysDate).AddDays(Int32.Parse(txtilsu.Text)).ToShortDateString();
            
            //예약시간단위 및 단위시간당 인원을 READ
            try
            {
                SQL = "";
                SQL += " SELECT DrDept1, DrCode, DrName, YTimeGbn   \r";
                SQL += "      , YInwon, AmTime, PmTime, YInwon2     \r";
                SQL += "      , AmTime2, PmTime2                    \r";
                SQL += "   FROM ADMIN.BAS_DOCTOR              \r";
                SQL += "  WHERE DRCODE = '" + sDrCode + "'          \r";
                clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                strAmTime = "";
                strPmTime = "";
                if (dt.Rows.Count > 0)
                {
                    nResvTimeGbn = Int32.Parse(dt.Rows[0]["YTimeGbn"].ToString());
                    nResvInwon = Int32.Parse(dt.Rows[0]["YInwon"].ToString());
                    nResvInwon2 = Int32.Parse(dt.Rows[0]["YInwon2"].ToString());
                    strAmTime = dt.Rows[0]["AmTime"].ToString();
                    strPmTime = dt.Rows[0]["PmTime"].ToString();
                    strAMTime2 = dt.Rows[0]["AmTime2"].ToString();
                    strPMTime2 = dt.Rows[0]["PmTime2"].ToString();
                    strDeptCode = dt.Rows[0]["DrDept1"].ToString();
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            lblDoctor.Text = strDeptCode + "." + clsVbfunc.GetBASDoctorName(clsDB.DbCon, sDrCode) + " 외래 예약 스케쥴 조회";

            if (strAMTime2 == "" || strPMTime2 == "")
            {
                ssSchedule_Sheet1.RowCount = 5;
                return;
            }

            strDrCode = sDrCode;
            //예약구분이 없으면 기본으로 30분단위 예약
            if (nResvTimeGbn == 0)
            {
                nResvTimeGbn = 4;
            }
            //예약인원이 없으면 인원제한 않함
            if (nResvInwon == 0)
            {
                nResvInwon = 999;
            }
            //예약 시작시간 설정
            if (strAmTime == "")
            {
                strAmTime = "09:30";
            }

            if (strPmTime == "")
            {
                strPmTime = "14:00";
            }

            for (int i = 0; i < 100; i++)
            {
                FstrRTime[i] = "";
            }

            for (int i = 0; i < 100; i++)
            {
                FstrRDate[i] = "";
            }

            nRow = 4;
            nMorningNo = 0;

            //ssDtl_Sheet1.RowCount = 0;  //예약자명단 Clear

            switch (nResvTimeGbn)
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
                default:
                    break;
            }

            kk = 0;

            for (int i = CF.TIME_MI(strAmTime); i <= CF.TIME_MI(strAMTime2); i += nTime)
            {
                FstrRTime[kk] = CF.TIME_MI_TIME(i);
                nRow += 1;
                if (nRow > ssSchedule_Sheet1.RowCount)
                {
                    ssSchedule_Sheet1.RowCount = nRow;
                }
                ssSchedule.ActiveSheet.Cells[nRow, 0].Text = FstrRTime[kk];
                nMorningNo = nRow;
                kk += 1;
            }

            for (int i = CF.TIME_MI(strPmTime); i <= CF.TIME_MI(strPMTime2); i += nTime)
            {
                FstrRTime[kk] = CF.TIME_MI_TIME(i);
                nRow += 1;
                if (nRow > ssSchedule_Sheet1.RowCount)
                {
                    ssSchedule_Sheet1.RowCount = nRow;
                }
                ssSchedule.ActiveSheet.Cells[nRow, 0].Text = FstrRTime[kk];
                kk += 1;
            }

            nTimeCNT = nRow - 4;
            FstrRTime[nTimeCNT] = "23:59";

            //예약 스케쥴을 읽어 SHEET의 상단에 Display
            try
            {
                SQL = "";
                SQL += " SELECT TO_CHAR(SchDate,'YYYY-MM-DD') SchDate,TO_CHAR(SchDate,'DY') Yoil,GbDay,GbJin,GbJin2     \r";
                SQL += "   FROM ADMIN.BAS_SCHEDULE                                                                \r";
                SQL += "  WHERE DrCode = '" + sDrCode + "'                                                              \r";
                SQL += "    AND SchDate > TRUNC(SYSDATE)                                                                \r";
                SQL += "    AND SchDate <= TO_DATE('" + strGDate + "','YYYY-MM-DD')                                     \r";
                SQL += "  ORDER BY SchDate                                                                              \r";

                clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssSchedule_Sheet1.RowCount = nRow + 1;

                if (dt.Rows.Count > 0)
                {
                    nREAD = dt.Rows.Count;
                    ssSchedule_Sheet1.ColumnCount = nREAD + 1;

                    ssSchedule.ActiveSheet.Cells[5, 1, ssSchedule.ActiveSheet.RowCount - 1, ssSchedule.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 255);
                    ssSchedule.ActiveSheet.Cells[5, 1, ssSchedule.ActiveSheet.RowCount - 1, ssSchedule.ActiveSheet.ColumnCount - 1].Text = "";

                    ssSchedule.ActiveSheet.Cells[0, 1, ssSchedule.ActiveSheet.RowCount - 1, ssSchedule.ActiveSheet.ColumnCount - 1].Border = new LineBorder(Color.Black, 0);

                    //스케쥴을 Sheet 상단에 표시함.
                    nCol = 0;
                    for (int i = 0; i < nREAD; i++)
                    {
                        //일요일은 표시 하지 않음.
                        if (dt.Rows[i]["YOIL"].ToString().Trim().ToUpper() != "일" && dt.Rows[i]["YOIL"].ToString().Trim().ToUpper() != "SUN")
                        {
                            nCol += 1;
                            FstrRDate[nCol - 1] = dt.Rows[i]["SCHDATE"].ToString().Trim();
                            strRDate = dt.Rows[i]["SCHDATE"].ToString().Trim();
                            ssSchedule.ActiveSheet.Columns.Get(nCol).Label = VB.Right(strRDate, 5) + "\r\n" + "+" + (i + 1);
                            ssSchedule.ActiveSheet.Cells[0, nCol].Text = strRDate;
                            ssSchedule.ActiveSheet.Cells[1, nCol].Text = dt.Rows[i]["GBJIN"].ToString().Trim();
                            switch (dt.Rows[i]["YOIL"].ToString().Trim().ToUpper())
                            {
                                case "SUN":
                                    ssSchedule.ActiveSheet.Cells[2, nCol].Text = "일";
                                    break;
                                case "일":
                                    ssSchedule.ActiveSheet.Cells[2, nCol].Text = "일";
                                    break;
                                case "MON":
                                    ssSchedule.ActiveSheet.Cells[2, nCol].Text = "월";
                                    break;
                                case "월":
                                    ssSchedule.ActiveSheet.Cells[2, nCol].Text = "월";
                                    break;
                                case "TUE":
                                    ssSchedule.ActiveSheet.Cells[2, nCol].Text = "화";
                                    break;
                                case "화":
                                    ssSchedule.ActiveSheet.Cells[2, nCol].Text = "화";
                                    break;
                                case "WED":
                                    ssSchedule.ActiveSheet.Cells[2, nCol].Text = "수";
                                    break;
                                case "수":
                                    ssSchedule.ActiveSheet.Cells[2, nCol].Text = "수";
                                    break;
                                case "THU":
                                    ssSchedule.ActiveSheet.Cells[2, nCol].Text = "목";
                                    break;
                                case "목":
                                    ssSchedule.ActiveSheet.Cells[2, nCol].Text = "목";
                                    break;
                                case "FRI":
                                    ssSchedule.ActiveSheet.Cells[2, nCol].Text = "금";
                                    break;
                                case "금":
                                    ssSchedule.ActiveSheet.Cells[2, nCol].Text = "금";
                                    break;
                                case "SAT":
                                    ssSchedule.ActiveSheet.Cells[2, nCol].Text = "토";
                                    break;
                                case "토":
                                    ssSchedule.ActiveSheet.Cells[2, nCol].Text = "토";
                                    break;
                                default:
                                    ssSchedule.ActiveSheet.Cells[2, nCol].Text = "";
                                    break;
                            }

                            switch (dt.Rows[i]["GBJIN"].ToString()) //오전
                            {
                                case "1":
                                    ssSchedule.ActiveSheet.Cells[3, nCol].Text = "진료";
                                    ssSchedule.ActiveSheet.Cells[5, nCol, nMorningNo, nCol].BackColor = Color.FromArgb(205, 250, 220);
                                    break;
                                case "2":
                                    ssSchedule.ActiveSheet.Cells[3, nCol].Text = "수술";
                                    ssSchedule.ActiveSheet.Cells[5, nCol, nMorningNo, nCol].BackColor = Color.FromArgb(255, 192, 128);
                                    break;
                                case "3":
                                    ssSchedule.ActiveSheet.Cells[3, nCol].Text = "특검";
                                    ssSchedule.ActiveSheet.Cells[5, nCol, nMorningNo, nCol].BackColor = Color.FromArgb(255, 128, 0);
                                    break;
                                case "9":
                                    ssSchedule.ActiveSheet.Cells[3, nCol].Text = "OFF";
                                    ssSchedule.ActiveSheet.Cells[5, nCol, nMorningNo, nCol].BackColor = Color.FromArgb(255, 192, 192);
                                    break;
                                default:
                                    ssSchedule.ActiveSheet.Cells[3, nCol].Text = "휴진";
                                    ssSchedule.ActiveSheet.Cells[5, nCol, nMorningNo, nCol].BackColor = Color.White;
                                    break;
                            }

                            switch (dt.Rows[i]["GBJIN2"].ToString())    //오후
                            {
                                case "1":
                                    ssSchedule.ActiveSheet.Cells[4, nCol].Text = "진료";
                                    ssSchedule.ActiveSheet.Cells[nMorningNo + 1, nCol, ssSchedule_Sheet1.RowCount - 1, nCol].BackColor = Color.FromArgb(205, 250, 220);
                                    break;
                                case "2":
                                    ssSchedule.ActiveSheet.Cells[4, nCol].Text = "수술";
                                    ssSchedule.ActiveSheet.Cells[nMorningNo + 1, nCol, ssSchedule_Sheet1.RowCount - 1, nCol].BackColor = Color.FromArgb(255, 192, 128);
                                    break;
                                case "3":
                                    ssSchedule.ActiveSheet.Cells[4, nCol].Text = "특검";
                                    ssSchedule.ActiveSheet.Cells[nMorningNo + 1, nCol, ssSchedule_Sheet1.RowCount - 1, nCol].BackColor = Color.FromArgb(255, 128, 0);
                                    break;
                                case "9":
                                    ssSchedule.ActiveSheet.Cells[4, nCol].Text = "OFF";
                                    ssSchedule.ActiveSheet.Cells[nMorningNo + 1, nCol, ssSchedule_Sheet1.RowCount - 1, nCol].BackColor = Color.FromArgb(255, 192, 192);
                                    break;
                                default:
                                    ssSchedule.ActiveSheet.Cells[4, nCol].Text = "휴진";
                                    ssSchedule.ActiveSheet.Cells[nMorningNo + 1, nCol, ssSchedule.ActiveSheet.RowCount - 1, nCol].BackColor = Color.White;
                                    break;
                            }
                        }
                    }
                    ssSchedule_Sheet1.ColumnCount = nCol + 1;
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            //ssSchedule_Sheet1.RowCount = nRow;
            //ssSchedule.ActiveSheet.Cells[5, 2, ssSchedule.ActiveSheet.RowCount - 1, ssSchedule.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 255);
            //ssSchedule.ActiveSheet.Cells[5, 2, ssSchedule.ActiveSheet.RowCount - 1, ssSchedule.ActiveSheet.ColumnCount - 1].Text = "";



            //ssSchedule.ActiveSheet.Cells[2, 1, ssSchedule.ActiveSheet.RowCount - 1, ssSchedule.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 255);
            ssSchedule.ActiveSheet.Rows.Get(1).Border = new LineBorder(Color.Black, 1, true, true, true, true);



            //의사별 기타 스케쥴을 읽어 Sheet에 표시함
            try
            {
                SQL = "";
                SQL += " SELECT TO_CHAR(SchDate,'YYYY-MM-DD') SchDate,STime,ETime       \r";
                SQL += "   FROM ADMIN.BAS_SCHEDULE_ETC                            \r";
                SQL += "  WHERE DrCode = '" + sDrCode + "'                              \r";
                SQL += "    AND SchDate > TRUNC(SYSDATE)                                \r";
                SQL += "    AND SchDate <= TO_DATE('" + strGDate + "','YYYY-MM-DD')     \r";
                SQL += "  ORDER BY SchDate,STime                                        \r";
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
                        strRDate = dt.Rows[i]["SCHDATE"].ToString();
                        strRTime = dt.Rows[i]["STIME"].ToString();
                        strETime = dt.Rows[i]["ETIME"].ToString();
                        //예약일자 Column 찾기
                        inx1 = 0;
                        for (int j = 0; j < 100; j++)
                        {
                            if (strRDate == FstrRDate[j])
                            {
                                inx1 = j;
                                break;
                            }
                        }


                        //임시막음 2018.02.08
                        if (inx1 > 0)
                        {
                            for (int j = 0; j < nTimeCNT; j++)
                            {
                                if (DateTime.Parse(FstrRTime[j]) >= DateTime.Parse(strRTime) && DateTime.Parse(FstrRTime[j]) <= DateTime.Parse(strETime))
                                {
                                    ssSchedule.ActiveSheet.Cells[j + 4, inx1 + 1].BackColor = Color.FromArgb(192, 192, 192);
                                }
                            }
                        }
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

            try
            {
                SQL = "";
                SQL += " SELECT TO_CHAR(Date3,'YYYY-MM-DD HH24:MI') RTime,COUNT(*) CNT          \r";
                SQL += "   FROM ADMIN.OPD_RESERVED_NEW                                    \r";
                SQL += "  WHERE Date3 > TRUNC(SYSDATE)                                          \r";
                SQL += "    AND Date3 <= TO_DATE('" + strGDate + " 23:59','YYYY-MM-DD HH24:MI') \r";
                SQL += "    AND DeptCode = '" + strDeptCode.Trim() + "'                         \r";
                SQL += "    AND DrCode = '" + sDrCode + "'                                      \r";
                SQL += "    AND TRANSDATE IS NULL                                               \r";
                SQL += "    AND RETDATE IS NULL                                                 \r";
                SQL += "  GROUP BY DATE3                                                        \r";
                SQL += "  ORDER BY DATE3                                                        \r";
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
                        strRDate = VB.Left(dt.Rows[i]["RTIME"].ToString(), 10);
                        strRTime = VB.Right(dt.Rows[i]["RTIME"].ToString(), 5);

                        //예약일자
                        inx1 = -1;
                        for (int j = 0; j < 100; j++)
                        {
                            if (strRDate == FstrRDate[j])
                            {
                                inx1 = j;
                                break;
                            }
                        }

                        //예약시간 Row
                        inx2 = -1;
                        for (int k = 0; k < nTimeCNT; k++)
                        {
                            if (Convert.ToDateTime(strRTime) < Convert.ToDateTime(FstrRTime[k + 1]))
                            //if (Convert.ToDateTime(strRTime) < Convert.ToDateTime(FstrRTime[k]))
                            {
                                inx2 = k;
                                break;
                            }
                        }

                        if (inx1 >= 0 && inx2 >= 0)
                        {
                            nCNT[inx1, inx2, 0] = nCNT[inx1, inx2, 0] + (int)VB.Val(dt.Rows[i]["CNT"].ToString());
                        }
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

            try
            {
                // 병동의 퇴원자예약 미수납자를 READ
                SQL = "";
                SQL += " SELECT TO_CHAR(RDate,'YYYY-MM-DD HH24:MI') RTime,COUNT(*) CNT          \r";
                SQL += "   FROM ADMIN.OCS_RESERVED                                         \r";
                SQL += "  WHERE BDate = TRUNC(SYSDATE)                                          \r";
                SQL += "    AND RDate > TRUNC(SYSDATE)                                          \r";
                SQL += "    AND RDate <= TO_DATE('" + strGDate + " 23:59','YYYY-MM-DD HH24:MI') \r";
                SQL += "    AND DrCode = '" + sDrCode + "'                                      \r";
                SQL += "    AND GBSUNAP = '0'                                                   \r"; //'允(2006-07-10) 추가
                SQL += "  GROUP BY RDate                                                        \r";
                SQL += "  ORDER BY RDate                                                        \r";
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
                        strRDate = VB.Left(dt.Rows[i]["RTIME"].ToString(), 10);
                        strRTime = VB.Right(dt.Rows[i]["RTIME"].ToString(), 5);

                        //예약일자
                        inx1 = -1;
                        for (int j = 0; j < 100; j++)
                        {
                            if (strRDate == FstrRDate[j])
                            {
                                inx1 = j;
                            }
                        }

                        //예약시간 Row
                        inx2 = -1;
                        for (int k = 0; k < nTimeCNT; k++)
                        {
                            if (Convert.ToDateTime(strRTime) < Convert.ToDateTime(FstrRTime[k + 1]))
                            {
                                inx2 = k;
                                break;
                            }
                        }

                        if (inx1 >= 0 && inx2 >= 0)
                        {
                            nCNT[inx1, inx2, 0] = nCNT[inx1, inx2, 0] + (int)VB.Val(dt.Rows[i]["CNT"].ToString());
                        }
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

            try
            {
                //전화접수 인원을 COUNT
                SQL = "";
                SQL += " SELECT TO_CHAR(RDate,'YYYY-MM-DD') RDate,RTime,COUNT(*) CNT    \r";
                SQL += "   FROM ADMIN.OPD_TELRESV                                 \r";
                SQL += "  WHERE RDate > TRUNC(SYSDATE)                                  \r";
                SQL += "    AND RDate <= TO_DATE('" + strGDate + "','YYYY-MM-DD')       \r";
                SQL += "    AND DrCode = '" + sDrCode + "'                              \r";
                SQL += "  GROUP BY RDate,RTime                                          \r";
                SQL += "  ORDER BY RDate,RTime                                          \r";
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
                        strRDate = dt.Rows[i]["RDate"].ToString();
                        strRTime = dt.Rows[i]["RTIME"].ToString();

                        //예약일자
                        inx1 = -1;
                        for (int j = 0; j < 100; j++)
                        {
                            if (strRDate == FstrRDate[j])
                            {
                                inx1 = j;
                            }
                        }

                        //예약시간 Row
                        inx2 = -1;
                        for (int k = 0; k < nTimeCNT; k++)
                        {
                            if (Convert.ToDateTime(strRTime) < Convert.ToDateTime(FstrRTime[k + 1]))
                            {
                                inx2 = k;
                                break;
                            }
                        }

                        if (inx1 >= 0 && inx2 >= 0)
                        {
                            nCNT[inx1, inx2, 1] = nCNT[inx1, inx2, 1] + (int)VB.Val(dt.Rows[i]["CNT"].ToString());
                        }
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

            //자료를 Sheet에 Display
            for (int i = 1; i < ssSchedule.ActiveSheet.ColumnCount - 1; i++)
            {
                for (int j = 0; j < nTimeCNT; j++)
                {
                    if (nCNT[i - 1, j, 0] != 0 || nCNT[i - 1, j, 1] != 0)
                    {
                        ssSchedule.ActiveSheet.Cells[j + 5, i].Text = string.Format("{0:##0}", nCNT[i - 1, j, 0] + nCNT[i - 1, j, 1]); //당일예약+전화예약
                        ssSchedule.ActiveSheet.Cells[j + 5, i].Text += "(" + string.Format("{0:##0}", nCNT[i - 1, j, 1]) + ")";
                    }
                }
            }

            nSelRow = 0;
            nSelCol = 0;
        }
    }
}
