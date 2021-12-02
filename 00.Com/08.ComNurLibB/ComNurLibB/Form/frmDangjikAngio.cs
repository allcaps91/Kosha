using ComBase;
using ComDbB;
using System;
using System.Data;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using ComLibB;

namespace ComNurLibB
{
    public partial class frmDangjikAngio : Form
    {
        #region //MainFormMessage

        //string fstrWard = "";

        public MainFormMessage mCallForm = null;

        public void MsgActivedForm(Form frm)
        {
        }

        public void MsgUnloadForm(Form frm)
        {
        }

        public void MsgFormClear()
        {
        }

        public void MsgSendPara(string strPara)
        {
        }

        #endregion //MainFormMessage

        clsPublic cpublic = new clsPublic(); //공용함수

        public frmDangjikAngio(MainFormMessage pform)
        {
            InitializeComponent();
            setEvent();

        }

        public frmDangjikAngio()
        {
            InitializeComponent();
            setEvent();

        }

        private void setCtrlData()
        {
            ComFunc cf = new ComFunc();
            clsIpdNr sup = new clsIpdNr ();

            READ_SYSDATE();
            sup.setYYYYMM(cboYYMM, cpublic.strSysDate, 24, 12, 12, "-");

            cboGubun.Items.Clear();
            cboGubun.Items.Add("1.혈관조영실");
            cboGubun.Items.Add("2.운수실");
            cboGubun.Items.Add("3.전산정보팀");
            cboGubun.SelectedIndex = 0;

            cf = null;
            sup = null;

        }

        private void setCtrlInit()
        {
        }

        private void eFormLoad(object sender, EventArgs e)
        {

        }

        private void eFormActivated(object sender, EventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgActivedForm(this);
            }
        }

        private void eFormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }

        private void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch2.Click += new EventHandler(eBtnClick);
            this.btnSearch3.Click += new EventHandler(eBtnClick);

            this.cboYYMM.SelectedIndexChanged += new EventHandler(eBtnClick);
            this.cboGubun.SelectedIndexChanged += new EventHandler(eBtnClick);
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == this.btnSearch2)
            {
                setMonth(false);
                SetCalendar();
            }
            else if (sender == this.btnSearch3)
            {
                setMonth(true);
                SetCalendar();
            }
            else if (sender == this.cboYYMM || sender == this.cboGubun)
            {
                SetCalendar();
            }

        }

        private string GetDayData(string argRDate, string argDept)
        {
            string SQL = "";
            string SqlErr = "";
            string strTEMP = "";

            int i = 0;

            DataTable dt = null;

            if (VB.Left(cboGubun.Text, 1) == "1" || VB.Left(cboGubun.Text, 1) == "3")
            {
                if (VB.Left(cboGubun.Text, 1) == "1")
                {
                    SQL = " SELECT DNAME1 || '/' || DNAME2 DNAME ";
                    SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.ETC_DANGJIK ";
                    SQL += ComNum.VBLF + "  WHERE TDATE = TO_DATE('" + argRDate + "', 'YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND GUBUN = '95' ";
                }
                else if (VB.Left(cboGubun.Text, 1) == "3")
                {
                    SQL = " SELECT DNAME1 DNAME ";
                    SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.ETC_DANGJIK ";
                    SQL += ComNum.VBLF + "  WHERE TDATE = TO_DATE('" + argRDate + "', 'YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND GUBUN = '99' ";
                }
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    dt.Dispose();
                    dt = null;
                    return "";
                }

                if (dt.Rows.Count > 0)
                {
                    strTEMP = " " + dt.Rows[i]["DNAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
            else if (VB.Left(cboGubun.Text, 1) == "2")
            {
                strTEMP = GetDangJikBus(VB.Replace(VB.Left(argRDate, 7), "-", ""), VB.Right(argRDate, 2));
            }

            return strTEMP;
        }


        void setMonth(bool bADD)
        {
            string strMonth = cboYYMM.Text.Trim().Replace("년", "").Replace("월", "").Replace(" ", "").Trim();
            string strYYYY = VB.Left(strMonth, 4);
            string strMM = "";
            strMM = ComFunc.SetAutoZero((bADD == true ? Convert.ToInt16(VB.Right(strMonth, 2)) + 1 : Convert.ToInt16(VB.Right(strMonth, 2)) - 1).ToString(), 2);
            if (strMM == "13")
            {
                strYYYY = (Convert.ToInt16(strYYYY) + 1).ToString();
                strMM = "01";

            }
            else if (strMM == "00")
            {
                strYYYY = (Convert.ToInt16(strYYYY) - 1).ToString();
                strMM = "12";
            }

            cboYYMM.Text = strYYYY + "년 " + strMM + "월";
        }

        void READ_SYSDATE()
        {
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void SetCalendar()
        {

            txtMemo.Text = "";
            if (VB.Left(cboGubun.Text, 1) == "1")
            {
                txtMemo.Text = "   방사선사 ";
                txtMemo.Text += ComNum.VBLF + "";
                txtMemo.Text += ComNum.VBLF + "김도영    010 - 4543 - 5902";
                txtMemo.Text += ComNum.VBLF + "남궁동훈  010 - 3585 - 2785   ";
                txtMemo.Text += ComNum.VBLF + "박세용    010 - 3796 - 5558   ";
                txtMemo.Text += ComNum.VBLF + "박효근    010 - 7236 - 7297";
                txtMemo.Text += ComNum.VBLF + "";
                txtMemo.Text += ComNum.VBLF + "   간호사";
                txtMemo.Text += ComNum.VBLF + "김상화    010 - 8708 - 9559";
                txtMemo.Text += ComNum.VBLF + "김종백    010 - 9367 - 4587";
                txtMemo.Text += ComNum.VBLF + "심다혜    010 - 2412 - 5828";
            }
            else if (VB.Left(cboGubun.Text, 1) == "2")
            {
                txtMemo.Text = "   운수실 메모사항   ";
                txtMemo.Text += ComNum.VBLF + "";
                txtMemo.Text += ComNum.VBLF + " ◈ 조재훈: 010 - 3816 - 2685";
                txtMemo.Text += ComNum.VBLF + " ◈ 허공명: 010 - 2825 - 9854";
                txtMemo.Text += ComNum.VBLF + " ◈ 황보창민: 010 - 9907 - 9511";
                txtMemo.Text += ComNum.VBLF + "			";
                txtMemo.Text += ComNum.VBLF + " ※ D1: 07:30~16:30";
                txtMemo.Text += ComNum.VBLF + " ※ D1: 07:30~11:30(토요일)";
                txtMemo.Text += ComNum.VBLF + " ※ D2: 08:30~17:30";
                txtMemo.Text += ComNum.VBLF + " ※ D2: 08:30~13:30(토요일)";
            }

            else if (VB.Left(cboGubun.Text, 1) == "3")
            {

                txtMemo.Text = "  1.시간          "; 
                txtMemo.Text += ComNum.VBLF + "    ※ 평 일 : 당일 17:30 ~익일 06:00";
                txtMemo.Text += ComNum.VBLF + "    ※ 토요일: 당일 13:30 ~익일 06:00";
                txtMemo.Text += ComNum.VBLF + "    ※ 휴 일 : 당일 06:00 ~익일 06:00";
                txtMemo.Text += ComNum.VBLF + "";
                txtMemo.Text += ComNum.VBLF + "  2.연락처";
                txtMemo.Text += ComNum.VBLF + "    김현욱 010 - 7159 - 4679";
                txtMemo.Text += ComNum.VBLF + "    김민철 010 - 8842 - 0253";
                txtMemo.Text += ComNum.VBLF + "    김경동 010 - 9328 - 4620";
                txtMemo.Text += ComNum.VBLF + "    한기호 010 - 8566 - 9765";
                txtMemo.Text += ComNum.VBLF + "    안정수 010 - 2729 - 1026";
                txtMemo.Text += ComNum.VBLF + "    김해수 010 - 6666 - 7472";
                txtMemo.Text += ComNum.VBLF + "    배재민 010 - 3191 - 3439";
                txtMemo.Text += ComNum.VBLF + "    김욱동 010 - 6886 - 0171";
            }

            if (cboYYMM.Text.Trim() == "")
            {
                return;
            }

            clsIpdNr sup = new clsIpdNr();

            string[] strDay = null;

            string strTemp = (cboYYMM.Text.Trim().Replace("년", "").Replace("월", "")).Replace(" ", "").Trim();
            string strSDate = VB.Left(strTemp, 4) + "-" + VB.Mid(strTemp, 5, 2) + "-01";
            string strTDate = Convert.ToString(Convert.ToDateTime(strSDate).AddMonths(1).AddDays(-1).ToShortDateString());

            READ_SYSDATE();

            int nRow = 0;
            int nCol = 0;

            int start = Convert.ToInt32(Convert.ToDateTime(strSDate).DayOfWeek);

            strTemp = "";
            //DataTable dt = null;

            //휴일체크
            strDay = sup.read_huil(clsDB.DbCon, strSDate, strTDate);

            //clear
            SS1.ActiveSheet.ClearRange(0, 0, (int)SS1.ActiveSheet.RowCount, (int)SS1.ActiveSheet.ColumnCount, true);

            nCol = start;

            for (int i = 0; i < Convert.ToInt16(VB.Right(strTDate, 2)); i++)
            {

                SS1.ActiveSheet.Cells[nRow, nCol].Text = (i + 1).ToString();
                SS1.ActiveSheet.Cells[nRow, nCol].ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
                SS1.ActiveSheet.Cells[nRow, nCol].BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                if (strDay[i] == "토")
                {
                    SS1.ActiveSheet.Cells[nRow, nCol].ForeColor = System.Drawing.Color.FromArgb(40, 30, 230);
                }
                if (strDay[i] == "일")
                {
                    SS1.ActiveSheet.Cells[nRow, nCol].ForeColor = System.Drawing.Color.FromArgb(255, 0, 0);
                }
                if (strDay[i] == "*")
                {
                    SS1.ActiveSheet.Cells[nRow, nCol].ForeColor = System.Drawing.Color.FromArgb(255, 0, 0);
                }

                SS1.ActiveSheet.Cells[nRow, nCol].Text = (i + 1).ToString() + ComNum.VBLF + GetDayData(VB.Left(strSDate, 8) + ComFunc.SetAutoZero((i + 1).ToString(), 2), cboGubun.Text.Trim());

                //dt = null;
                strTemp = "";

                nCol++;
                if (nCol > 6)
                {
                    nRow++;
                    nCol = 0;
                }
                if (nRow > 4) nRow = 0;


            }

            sup = null;

        }


        private string GetDangJikBus(string argYYMM, string argDay)
        {

            string SQL = "";
            string SqlErr = "";
            string strRtnD1 = "";
            string strRtnD2 = "";
            string strRtnOFF = "";
            //string strRtn = "";

            string strTDate = "";

            int i = 0;
            //int j = 0;

            DataTable dt = null;

            string strKorName1 = "";
            string strKorName2 = "";
            string strKorName3 = "";
            string strKorName4 = "";
            string strKorName1OFF = "";
            string strKorName3OFF = "";
            string strKorNameTOFF = "";
            string strKorNamePOFF = "";

            string strWeek = "";
            string strOil = "";

            string strDay = "";

            strTDate = argYYMM + ComFunc.SetAutoZero(argDay, 2);

            strDay = VB.Val(argDay).ToString();

            SQL = "Select TO_CHAR(TO_DATE('" + strTDate + "', 'YYYY-MM-DD'),'DY') cWeek ";
            SQL = SQL + " FROM DUAL ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                dt.Dispose();
                dt = null;
                return "";
            }

            strWeek = dt.Rows[i]["CWEEK"].ToString().Trim();

            dt.Dispose();
            dt = null;

            switch (strWeek)
            {
                case "MON":
                    strOil = "월";
                    break;
                case "TUE":
                    strOil = "월";
                    break;
                case "WED":
                    strOil = "월";
                    break;
                case "THU":
                    strOil = "월";
                    break;
                case "FRI":
                    strOil = "월";
                    break;
                case "SAT":
                    strOil = "월";
                    break;
                case "SUN":
                    strOil = "월";
                    break;
                default:
                    strOil = strWeek;
                    break;

            }


            //D1 근무자 표시
            SQL = " SELECT KORNAME FROM KOSMOS_ADM.INSA_MST a, KOSMOS_ADM.INSA_CHULTIME b ";
            SQL = SQL + " WHERE b.SABUN = a.SABUN ";
            SQL = SQL + "    AND b.YYMM = '" + argYYMM + "' ";
            SQL = SQL + "    AND BUNPYO" + strDay + " = 'DD73' ";
            SQL = SQL + "    and (TOIDAY IS NULL OR TOIDAY >= TO_DATE('" + strTDate + "','YYYY-MM-DD')) ";
            SQL = SQL + "    and b.SABUN <> '18640' ";
            SQL = SQL + "    AND a.BUSE = '066107'";
                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return "";
            }

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strKorName1 = strKorName1 + dt.Rows[i]["KORNAME"].ToString().Trim() + ",";
                }
                strKorName1 = VB.Left(strKorName1, VB.Len(strKorName1) - 1);
            }

            dt.Dispose();
            dt = null;


            //DOFF 평일에는 D1반휴, 토요일에는 D1근무, 일요일에는 D2근무
            SQL = " SELECT KORNAME FROM KOSMOS_ADM.INSA_MST a, KOSMOS_ADM.INSA_CHULTIME b ";
            SQL = SQL + " WHERE b.SABUN = a.SABUN ";
            SQL = SQL + "    AND b.YYMM = '" + argYYMM + "' ";
            SQL = SQL + "    AND BUNPYO" + strDay + " = 'D113' ";
            SQL = SQL + "    AND a.BUSE = '066107'";
            SQL = SQL + "    and (TOIDAY IS NULL OR TOIDAY >= TO_DATE('" + strTDate + "','YYYY-MM-DD')) ";
                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                dt.Dispose();
                dt = null;
                return "";
            }

            if (dt.Rows.Count > 0)
            {
                switch (strWeek)
                {
                    case "월": case "화": case "수": case "목": case "금":
                    case "MON": case "TUE": case "WED": case "THU": case "FRI":
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            strKorName1OFF = strKorName1OFF + dt.Rows[i]["KORNAME"].ToString().Trim() + ",";
                        }
                        strKorName1OFF = VB.Left(strKorName1OFF, VB.Len(strKorName1OFF) - 1);
                        break;
                    case "토": case "SAT":
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            strKorName1 = strKorName1 + dt.Rows[i]["KORNAME"].ToString().Trim() + ",";
                        }
                        strKorName1 = VB.Left(strKorName1, VB.Len(strKorName1) - 1);
                        break;
                    case "일": case "SUN":
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            strKorName2 = strKorName2 + dt.Rows[i]["KORNAME"].ToString().Trim() + ",";
                        }
                        strKorName2 = VB.Left(strKorName2, VB.Len(strKorName2) - 1);
                        break;
                }
                                    
            }

            dt.Dispose();
            dt = null;


            //D2 근무자 표시
            SQL = " SELECT KORNAME FROM KOSMOS_ADM.INSA_MST a, KOSMOS_ADM.INSA_CHULTIME b ";
            SQL = SQL + " WHERE b.SABUN = a.SABUN ";
            SQL = SQL + "    AND b.YYMM = '" + argYYMM + "' ";
            SQL = SQL + "    AND BUNPYO" + strDay + " IN ('DD90','D083', 'D133') ";
            SQL = SQL + "    AND a.BUSE = '066107'";
            SQL = SQL + "    and (TOIDAY IS NULL OR TOIDAY >= TO_DATE('" + strTDate + "','YYYY-MM-DD')) ";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                dt.Dispose();
                dt = null;
                return "";
            }

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strKorName2 = strKorName2 + dt.Rows[i]["KORNAME"].ToString().Trim() + ",";
                }
                strKorName2 = VB.Left(strKorName2, VB.Len(strKorName2) - 1);
            }

            dt.Dispose();
            dt = null;


            //D3 근무자 표시
            SQL = " SELECT KORNAME FROM KOSMOS_ADM.INSA_MST a, KOSMOS_ADM.INSA_CHULTIME b ";
            SQL = SQL + " WHERE b.SABUN = a.SABUN ";
            SQL = SQL + "    AND b.YYMM = '" + argYYMM + "' ";
            SQL = SQL + "    AND BUNPYO" + strDay + " IN ('DE90','D091') ";
            SQL = SQL + "    AND a.BUSE = '066107'";
            SQL = SQL + "    and (TOIDAY IS NULL OR TOIDAY >= TO_DATE('" + strTDate + "','YYYY-MM-DD')) ";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                dt.Dispose();
                dt = null;
                return "";
            }

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strKorName3 = strKorName3 + dt.Rows[i]["KORNAME"].ToString().Trim() + ",";
                }
                strKorName3 = VB.Left(strKorName3, VB.Len(strKorName3) - 1);
            }

            dt.Dispose();
            dt = null;


            //D3 반휴 표시
            SQL = " SELECT KORNAME FROM KOSMOS_ADM.INSA_MST a, KOSMOS_ADM.INSA_CHULTIME b ";
            SQL = SQL + " WHERE b.SABUN = a.SABUN ";
            SQL = SQL + "    AND b.YYMM = '" + argYYMM + "' ";
            SQL = SQL + "    AND BUNPYO" + strDay + " = 'EHFF' ";
            SQL = SQL + "    AND a.BUSE = '066107'";
            SQL = SQL + "    and (TOIDAY IS NULL OR TOIDAY >= TO_DATE('" + strTDate + "','YYYY-MM-DD')) ";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                dt.Dispose();
                dt = null;
                return "";
            }

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strKorName3OFF = strKorName3OFF + dt.Rows[i]["KORNAME"].ToString().Trim() + ",";
                }
                strKorName3OFF = VB.Left(strKorName3OFF, VB.Len(strKorName3OFF) - 1);
            }

            dt.Dispose();
            dt = null;

            //휴무자 표시
            SQL = " SELECT KORNAME FROM KOSMOS_ADM.INSA_MST a, KOSMOS_ADM.INSA_CHULTIME b ";
            SQL = SQL + " WHERE b.SABUN = a.SABUN ";
            SQL = SQL + "    AND b.YYMM = '" + argYYMM + "' ";
            SQL = SQL + "    AND BUNPYO" + strDay + " = 'TOFF' ";
            SQL = SQL + "    AND a.BUSE = '066107'";
            SQL = SQL + "    and (TOIDAY IS NULL OR TOIDAY >= TO_DATE('" + strTDate + "','YYYY-MM-DD')) ";
                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                dt.Dispose();
                dt = null;
                return "";
            }

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strKorNameTOFF = strKorNameTOFF + dt.Rows[i]["KORNAME"].ToString().Trim() + ",";
                }
                strKorNameTOFF = VB.Left(strKorNameTOFF, VB.Len(strKorNameTOFF) - 1);
            }

            dt.Dispose();
            dt = null;


            //D2 반휴 표시
            SQL = " SELECT KORNAME FROM KOSMOS_ADM.INSA_MST a, KOSMOS_ADM.INSA_CHULTIME b ";
            SQL = SQL + " WHERE b.SABUN = a.SABUN ";
            SQL = SQL + "    AND b.YYMM = '" + argYYMM + "' ";
            SQL = SQL + "    AND BUNPYO" + strDay + " IN ('POFF','EH14','')  ";
            SQL = SQL + "    AND a.BUSE = '066107'";
            SQL = SQL + "    and (TOIDAY IS NULL OR TOIDAY >= TO_DATE('" + strTDate + "','YYYY-MM-DD')) ";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                dt.Dispose();
                dt = null;
                return "";
            }

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strKorNamePOFF = strKorNamePOFF + dt.Rows[i]["KORNAME"].ToString().Trim() + ",";
                }
                strKorNamePOFF = VB.Left(strKorNamePOFF, VB.Len(strKorNamePOFF) - 1);
            }

            dt.Dispose();
            dt = null;

            
            strKorName1 = VB.Replace(strKorName1, " ", "");
            strKorName2 = VB.Replace(strKorName2, " ", "");
            strKorName3 = VB.Replace(strKorName3, " ", "");
            strKorName4 = VB.Replace(strKorName4, " ", "");
            strKorName1OFF = VB.Replace(strKorName1OFF, " ", "");
            strKorName3OFF = VB.Replace(strKorName3OFF, " ", "");
            strKorNameTOFF = VB.Replace(strKorNameTOFF, " ", "");
            strKorNamePOFF = VB.Replace(strKorNamePOFF, " ", "");

            if (strKorName1 == null) { strKorName1 = ""; }
            if (strKorName2 == null) { strKorName2 = ""; }
            if (strKorName3 == null) { strKorName3 = ""; }
            if (strKorName4 == null) { strKorName4 = ""; }
            if (strKorName1OFF == null) { strKorName1OFF = ""; }
            if (strKorName3OFF == null) { strKorName3OFF = ""; }
            if (strKorNameTOFF == null) { strKorNameTOFF = ""; }
            if (strKorNamePOFF == null) { strKorNamePOFF = ""; }



            strRtnD1 = strKorName1 + " " + (strKorName1OFF != "" ? strKorName1OFF + "D1" : "");
            strRtnD2 = strKorName2 + " " + strKorName4 + (strKorNamePOFF != "" ? strKorNamePOFF + "D2" : "");
            strRtnOFF = strKorNameTOFF + (strKorNamePOFF != "" ? " " + strKorNamePOFF + "(반휴)" : "") +
                                                  (strKorName1OFF != "" ? strKorName1OFF + "(반휴)" : "") +
                                                  (strKorName3OFF != "" ? strKorName3OFF + "(반휴)" : "");

            if (strRtnD1 != "")
            {
                strRtnD1 = "(D1) " + ComNum.VBLF +  strRtnD1;
            }
            if (strRtnD2 != "")
            {
                strRtnD2 = "(D2) " + ComNum.VBLF + strRtnD2;
            }
            if (strRtnOFF != "")
            {
                strRtnOFF = "(OFF) " + ComNum.VBLF + strRtnOFF;
            }

            return strRtnD1 + ComNum.VBLF + strRtnD2 + ComNum.VBLF + strRtnOFF;

        }

       
    }



}
