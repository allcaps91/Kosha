using ComBase;
using ComBase.Controls;
using FarPoint.Win;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComMedLibB
{
    /// <summary>
    /// Description : 진료 예약(New)
    /// Author : 이상훈
    /// Create Date : 2017.10.16
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref="Frm예약오더입력1.frm"/>
    public partial class FrmMedRsvOrder : Form 
    {
        string strPano;
        string strDeptCode;
        string strDrCode;

        string strBi;
        string strRowId;

        string strRDate_Old;

        string[] FstrRTime = new string[0];
        string[] FstrRDate = new string[0];
        int nResvTimeGbn;
        int nResvInwon;
        int nResvInwon2;
        int nSelRow;
        int nSelCol;
        string strRdate_old;
        int nMorningNo;

        string strSMS;      //sms 동의
        string strSMS_DRUG; //'sms 안부분자(약)

        string SQL;
        DataTable dt = null;
        DataTable dt1 = null;
        DataTable dt2 = null;
        string SqlErr = "";     //에러문 받는 변수
        int intRowAffected = 0; //변경된 Row 받는 변수

        clsSpread SP = new clsSpread();
        ComFunc CF = new ComFunc();

        FarPoint.Win.Spread.CellType.TextCellType txt = new FarPoint.Win.Spread.CellType.TextCellType();

        public FrmMedRsvOrder()
        {
            InitializeComponent();
        }

        public FrmMedRsvOrder(string sPano, string sDeptCode, string sDrCod)
        {
            InitializeComponent();

            strPano = sPano;
            strDeptCode = sDeptCode;
            strDrCode = sDrCod;
        }

        private void FrmMedRsvOrder_Load(object sender, EventArgs e)
        {
            string strData;
            int j = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsOrdFunction.GstrRsvCancelFlag = "";

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            try
            {
                SQL = "";
                SQL += " SELECT BI                                                              \r";
                SQL += "   FROM KOSMOS_PMPA.OPD_MASTER                                          \r";
                SQL += "  WHERE PANO = '" + strPano + "'                                        \r";
                SQL += "    AND BDATE = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD')   \r";
                SQL += "    AND DEPTCODE = '" + strDeptCode + "'                                \r";
                clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strBi = dt.Rows[0]["BI"].ToString();
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            strRowId = "";
            txtRemark.Text = "";
            btnRegist.Text = "예약저장(&O)";

            ssSchedule_Sheet1.Rows.Get(0).Visible = false;
            ssSchedule_Sheet1.Rows.Get(1).Visible = false;

            ssDtl_Sheet1.RowCount = 0;
            ssDtl_Sheet1.Columns.Get(11).Visible = false;

            this.Location = new Point(30, 30);

            txtPanoR.Text = strPano;
            btnRegist.Enabled = true;

            if (strDeptCode == "PD")
            {
                txtRIlsu.Text = "60";
            }

            FstrRTime = new string[txtRIlsu.Text.To<int>()];
            FstrRDate = new string[txtRIlsu.Text.To<int>()];

            lblInfo.Text = "";
            txtRemark.Text = "";

            try
            {
                SQL = "";
                SQL += " SELECT DrCode,DrName,YTimeGbn,YInwon           \r";
                SQL += "   FROM KOSMOS_PMPA.BAS_DOCTOR                  \r";
                SQL += "  WHERE DrDept1 = '" + strDeptCode + "'         \r";
                SQL += "    AND TOUR <> 'Y'                             \r";
                SQL += "    AND SUBSTR(DrCode,3,2) <> '99'              \r";
                SQL += "  ORDER BY PrintRanking                         \r";
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
                        strData = VB.Left(dt.Rows[i]["DRCODE"].ToString() + VB.Space(4), 4) + ".";
                        strData += dt.Rows[i]["DRNAME"].ToString().Trim();
                        cboDoctor.Items.Add(strData);
                        if (strDrCode == dt.Rows[i]["DRCODE"].ToString())
                        {
                            j = i;
                        }
                    }                    
                    txtPano.Text = strPano;
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
                //예약체크
                SQL = "";
                SQL += " SELECT Sucode, OrderCode,DrCode,GBINFO,Remark,GbSunap,ROWID                \r";
                SQL += "   FROM KOSMOS_OCS.OCS_OORDER                                               \r";
                SQL += "  WHERE Ptno     = '" + strPano + "'                                        \r";
                SQL += "    AND DeptCode = '" + strDeptCode + "'                                    \r";
                SQL += "    AND BDate    =  TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD')   \r";
                SQL += "    AND Seqno    =  0                                                       \r";
                SQL += "    AND OrderCode Is Not Null                                               \r";
                clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                chkExam.Checked = false;
                lblRTime.Text = "";
                lblStat.Text = "";

                if (dt.Rows.Count > 0)
                {
                    strRowId = dt.Rows[0]["ROWID"].ToString().Trim();
                    txtRemark.Text = dt.Rows[0]["REMARK"].ToString().Trim();
                    if (dt.Rows[0]["GBINFO"].ToString().Trim() == "Y")
                    {
                        chkExam.Checked = true;
                    }

                    strRDate_Old = VB.Left(dt.Rows[0]["ORDERCODE"].ToString().Trim(), 4) + "-" + VB.Mid(dt.Rows[0]["ORDERCODE"].ToString().Trim(), 5, 2) + "-" + VB.Right(dt.Rows[0]["ORDERCODE"].ToString().Trim(), 2);
                    lblRTime.Text = VB.Left(dt.Rows[0]["ORDERCODE"].ToString().Trim(), 4) + "-" + VB.Mid(dt.Rows[0]["ORDERCODE"].ToString().Trim(), 5, 2) + "-" + VB.Right(dt.Rows[0]["ORDERCODE"].ToString().Trim(), 2) + " " + dt.Rows[0]["SUCODE"].ToString().Trim();

                    if (dt.Rows[0]["GBSUNAP"].ToString() == "1")
                    {
                        lblStat.Text = "수납";
                        btnRegist.Enabled = false;
                    }
                    else
                    {
                        lblStat.Text = "미수납";
                        btnRegist.Enabled = true;
                    }

                    btnRegist.Text = "예약변경(&O)";
                    btnCancel.Enabled = true;
                    MessageBox.Show(lblStat.Text.Trim() + " 된 당일 예약정보가 있습니다.. 참고하십시오!!!" + "\r\n" + "\r\n" + "예약일시 : " + lblRTime.Text, "예약정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            chkResNoSend.Checked = false;
            if (clsBagage.CHECK_RES_SMSNOTSEND2(clsDB.DbCon, strPano, clsPublic.GstrSysDate, strDeptCode) == "Y")
            {
                chkResNoSend.Checked = true;
            }

            //SMS 동의 여부 표시
            strSMS = "";
            strSMS_DRUG = "";

            try
            {
                SQL = "";
                SQL += " SELECT GBSMS, GBSMS_DRUG,SName     \r";
                SQL += "   FROM KOSMOS_PMPA.BAS_PATIENT     \r";
                SQL += "  WHERE PANO = '" + strPano + "'    \r";
                clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    lblName.Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    strSMS = dt.Rows[0]["GBSMS"].ToString();
                    strSMS_DRUG = dt.Rows[0]["GBSMS_DRUG"].ToString();
                    chkSMS.Checked = strSMS == "Y" ? true : false;
                    chkSMSDrug.Checked = strSMS_DRUG == "*" ? true : false;
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
                SQL += " SELECT TO_CHAR(INDATE,'YYYY-MM-DD') INDATE,GBSTS   \r";
                SQL += "   FROM KOSMOS_PMPA.IPD_NEW_MASTER                  \r";
                SQL += "  WHERE PANO = '" + strPano + "'                    \r";
                SQL += "    AND ACTDATE IS NULL                             \r";
                SQL += "  ORDER BY INDATE DESC                              \r";
                clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    lblInfo.Text = "입원중 => 입원일자 : " + dt.Rows[0]["INDATE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            btnViewR_Click(sender, e);
            cboDoctor.SelectedIndex = j;
            //cboDoctor_Click(cboDoctor, e);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            clsOrdFunction.GstrRsvCancelFlag = "";
            this.Close();
        }

        private void btnViewR_Click(object sender, EventArgs e)
        {
            SP.Spread_All_Clear(ssRsvStat);
            if (txtPano.Text == "") return;
            txtPanoR.Text = string.Format("{0:00000000}", txtPanoR.Text);

            try
            {
                SQL = "";
                SQL += " SELECT b.SName,b.Pano,a.DeptCode,a.DrCode,a.Ordercode RDate,a.SuCode RTime     \r";
                SQL += "      , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(a.DrCode) DRNAME                        \r";
                SQL += "   FROM KOSMOS_OCS.OCS_OORDER   a                                               \r";
                SQL += "      , KOSMOS_PMPA.BAS_PATIENT b                                               \r";
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
                    ssRsvStat.ActiveSheet.RowCount = dt.Rows.Count;
                    ssRsvStat.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ssRsvStat.ActiveSheet.Cells[i, 0].Text = "가예약";
                        ssRsvStat.ActiveSheet.Cells[i, 1].Text = ComFunc.FormatStrToDateEx(dt.Rows[i]["RDATE"].ToString().Replace("-", ""), "D", "-");
                        //ssRsvStat.ActiveSheet.Cells[i, 1].Text = string.Format("{0:yyyy-MM-dd}", dt.Rows[i]["RDATE"].ToString());
                        ssRsvStat.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["RTIME"].ToString();
                        ssRsvStat.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["SNAME"].ToString();
                        ssRsvStat.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["DEPTCODE"].ToString();
                        ssRsvStat.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["DRNAME"].ToString();
                    }
                }

                dt.Dispose();
                dt = null;
            
                SQL = "";
                SQL += " SELECT SName,Pano,DeptCode,DrCode,TO_CHAR(Date3,'YYYY-MM-DD') RDate,TO_CHAR(Date3,'HH24:MI') RTime \r";
                SQL += "      , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(DrCode) DRNAME                                              \r";
                SQL += "   FROM KOSMOS_PMPA.OPD_RESERVED_NEW                                                                \r";
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
                    ssRsvStat.ActiveSheet.RowCount = ssRsvStat.ActiveSheet.RowCount + dt.Rows.Count;
                    ssRsvStat.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ssRsvStat.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["RDATE"].ToString();
                        ssRsvStat.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["RTIME"].ToString();
                        ssRsvStat.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["SNAME"].ToString();
                        ssRsvStat.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["DEPTCODE"].ToString();
                        ssRsvStat.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["DRNAME"].ToString();
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

        private void cboDoctor_Click(object sender, EventArgs e)
        {
            //string strDrCode;

            //strDrCode = ComFunc.LeftH(cboDoctor.Text, 4);

            //fn_Yeyak_Inwon_Display(strDrCode);
        }

        /// <summary>
        /// 의사별 예약,전화예약 인원 및 스케쥴을 표시
        /// </summary>
        /// <param name="sDrCode"></param>

        /// <summary>
        /// 의사별 예약,전화예약 인원 및 스케쥴을 표시
        /// </summary>
        /// <param name="sDrCode"></param>
        void fn_Yeyak_Inwon_Display(string sDrCode)
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
            string strGDate;
            string strAmTime = "";
            string strAMTime2 = "";
            string strPmTime = "";
            string strPMTime2 = "";
            int nTimeCNT;

            FstrRTime = new string[txtRIlsu.Text.To<int>()];
            FstrRDate = new string[txtRIlsu.Text.To<int>()];

            int[,,] nCNT = new int[FstrRDate.Length, FstrRDate.Length, 3];
            string strDeptCode = "";

            this.Text = "진료예약(" + cboDoctor.Text + ")";
            strGDate = DateTime.Parse(clsPublic.GstrSysDate).AddDays(Int32.Parse(txtRIlsu.Text)).ToShortDateString();

            //예약시간단위 및 단위시간당 인원을 READ
            try
            {
                SQL = "";
                SQL += " SELECT DrDept1, DrCode, DrName, YTimeGbn   \r";
                SQL += "      , YInwon, AmTime, PmTime, YInwon2     \r";
                SQL += "      , AmTime2, PmTime2                    \r";
                SQL += "   FROM KOSMOS_PMPA.BAS_DOCTOR              \r";
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
                    nResvTimeGbn = (int)VB.Val(dt.Rows[0]["YTimeGbn"].ToString());
                    nResvInwon = (int)VB.Val(dt.Rows[0]["YInwon"].ToString());
                    nResvInwon2 = (int)VB.Val(dt.Rows[0]["YInwon2"].ToString());
                    strAmTime = dt.Rows[0]["AmTime"].ToString();
                    strPmTime = dt.Rows[0]["PmTime"].ToString();
                    strAMTime2 = dt.Rows[0]["AmTime2"].ToString();
                    strPMTime2 = dt.Rows[0]["PmTime2"].ToString();
                    lblPaYinwon.Text = "예약인원(오전:" + nResvInwon + "명 오후:" + nResvInwon2 + "명)";
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

            for (int i = 0; i < FstrRTime.Length; i++)
            {
                FstrRTime[i] = "";
            }

            for (int i = 0; i < FstrRDate.Length; i++)
            {
                FstrRDate[i] = "";
            }

            nRow = 4;
            nMorningNo = 0;

            ssDtl_Sheet1.RowCount = 0;  //예약자명단 Clear

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
                SQL += "   FROM KOSMOS_PMPA.BAS_SCHEDULE                                                                \r";
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
                SQL += "   FROM KOSMOS_PMPA.BAS_SCHEDULE_ETC                            \r";
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
                        for (int j = 0; j < FstrRDate.Length; j++)
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
                SQL += "   FROM KOSMOS_PMPA.OPD_RESERVED_NEW                                    \r";
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
                        for (int j = 0; j < FstrRDate.Length; j++)
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
                            //if (Convert.ToDateTime(strRTime) <= Convert.ToDateTime(FstrRTime[k]))
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
                SQL += "   FROM KOSMOS_OCS.OCS_RESERVED                                         \r";
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
                        for (int j = 0; j < FstrRDate.Length; j++)
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
                SQL += "   FROM KOSMOS_PMPA.OPD_TELRESV                                 \r";
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
                        for (int j = 0; j < FstrRDate.Length; j++)
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
            //for (int i = 1; i < ssSchedule.ActiveSheet.ColumnCount - 1; i++)
            for (int i = 1; i < FstrRDate.Length; i++)
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
        
        private void txtRIlsu_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strDrCode;

            if (e.KeyChar == (char)13)
            {
                strDrCode = ComFunc.LeftH(cboDoctor.Text, 4);
                fn_Yeyak_Inwon_Display(strDrCode);
            }
        }

        private void ssDtl_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strPano;
            string strSName;
            string strRTime;
            string strExam;
            string strMsg = "";

            strPano = ssDtl.ActiveSheet.Cells[e.Row, 0].Text;
            strSName = ssDtl.ActiveSheet.Cells[e.Row, 1].Text;
            strRTime = ssDtl.ActiveSheet.Cells[e.Row, 11].Text;
            strExam = ssDtl.ActiveSheet.Cells[e.Row, 12].Text;

            if (ssDtl.ActiveSheet.Cells[e.Row, 7].Text == "전화")
            {
                MessageBox.Show("전화예약은 변경이 불가능 합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            strMsg = strPano + " " + strSName + "님의 예약을 변경하시겠습니까?";
            
            if (MessageBox.Show(strMsg, "예약변경", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            strRDate_Old = "2007-" + VB.Left(ssDtl.ActiveSheet.Cells[e.Row, 6].Text, 2) + "-" + VB.Mid(ssDtl.ActiveSheet.Cells[e.Row, 6].Text, 4, 2);

            txtPano.Text = strPano;
            lblName.Text = strSName;
            lblStat.Text = "수납";
            if (strExam == "Y")
            {
                chkExam.Checked = true;
            }
            btnRegist.Enabled = false;
            btnCancel.Enabled = false;
            lblRTime.Text = strRTime;
        }

        private void ssSchedule_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strRTime;
            string strTime1;
            string strTime2;
            double nYeyakInwon = 0;
            string strMsg = "";
            int nRow = 0;

            txtRemark.Text = "";

            if (e.Column < 1) return;

            //예약자 조회할 시각을 설정
            strRTime = ssSchedule.ActiveSheet.Cells[0, e.Column].Text;
            if (e.Row < 5)
            {
                strTime1 = VB.Left(strRTime, 10) + " 00:00";
                strTime2 = VB.Left(strRTime, 10) + " 23:59";
            }
            else if (e.Row == 4 || e.Row == 3)
            {
                strTime1 = VB.Left(strRTime, 10) + " 00:00";
                strTime2 = VB.Left(strRTime, 10) + " " + FstrRTime[e.Row - 4];
            }
            else if (e.Row == ssSchedule.ActiveSheet.NonEmptyRowCount)
            {
                strTime1 = VB.Left(strRTime, 10) + " " + FstrRTime[e.Row - 5];
                strTime2 = VB.Left(strRTime, 10) + " 23:59";
            }
            else
            {
                strTime1 = VB.Left(strRTime, 10) + " " + FstrRTime[e.Row - 5];
                strTime2 = VB.Left(strRTime, 10) + " " + FstrRTime[e.Row - 4];
            }

            if (ComFunc.LeftH(cboDoctor.Text, 4) == "0107" && CF.READ_YOIL(clsDB.DbCon, strRTime) == "화요일" && Convert.ToDateTime(VB.Right(strTime1, 5)) <= Convert.ToDateTime("10:30"))
            {
                MessageBox.Show("박준모 과장 화요일은 10시30분 전 예약 안됨!!!", "내과문의", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                //예약체크
                SQL = "";
                SQL += " SELECT *                                                       \r";
                SQL += "   FROM KOSMOS_PMPA.OPD_RESERVED_NEW                            \r";
                SQL += "  WHERE PANO = '" + strPano + "'                                \r";
                SQL += "    AND DEPTCODE = '" + strDeptCode + "'                        \r";
                SQL += "    AND TRUNC(DATE3) = TO_DATE('" + strRTime + "','YYYY-MM-DD') \r";
                SQL += "    AND (TRANSDATE IS NULL OR TRANSDATE = '')                   \r";
                SQL += "    AND (RETDATE IS NULL OR RETDATE = '')                       \r";
                clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("이미 해당일자에 예약이 있습니다...", "예약확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dt.Dispose();
                    dt = null;
                    return;
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
                //해당 시각대 예약자를 SELECT
                SQL = "";
                SQL += " SELECT a.Pano,b.SName,b.Tel,a.DeptCode, A.DRCODE, c.DrName         \r";
                SQL += "      , TO_CHAR(a.Date3,'MM/DD HH24:MI') RDate                      \r";
                SQL += "      , TO_CHAR(a.Date3,'YYYY-MM-DD HH24:MI') RDate2, A.EXAM        \r";
                SQL += "      , b.Juso,b.Sex,b.ZipCode1,b.ZipCode2                          \r";
                SQL += "      , TO_CHAR(L.LASTDATE,'YYYY-MM-DD') LASTDATE, a.BI             \r";
                SQL += "      , KOSMOS_OCS.FC_BI_NM(a.BI) BINAME                            \r";
                SQL += "      , KOSMOS_OCS.FC_BAS_PATIENT_JUSO(b.PANO) ADDRESS              \r";
                SQL += "   FROM KOSMOS_PMPA.OPD_RESERVED_NEW a                              \r";
                SQL += "      , KOSMOS_PMPA.BAS_PATIENT      b                              \r";
                SQL += "      , KOSMOS_PMPA.BAS_DOCTOR       c                              \r";
                SQL += "      , KOSMOS_PMPA.BAS_LASTEXAM     L                              \r";
                SQL += "  WHERE a.Date3 >= TO_DATE('" + strTime1 + "','YYYY-MM-DD HH24:MI') \r";
                SQL += "    AND a.Date3 < TO_DATE('" + strTime2 + "','YYYY-MM-DD HH24:MI')  \r";
                SQL += "    AND a.DrCode = '" + ComFunc.LeftH(cboDoctor.Text, 4) + "'       \r";
                SQL += "    AND a.TransDate IS NULL                                         \r";
                SQL += "    AND a.RetDate IS NULL                                           \r";
                SQL += "    AND a.Pano=b.Pano(+)                                            \r";
                SQL += "    AND a.DrCode=c.DrCode(+)                                        \r";
                SQL += "    AND a.PANO = L.PANO(+)                                          \r";
                if (clsPublic.GstrDeptCode == "MD")
                {
                    SQL += "    AND A.DRCODE = L.DRCODE(+)                                  \r";
                }
                else
                {
                    SQL += "    AND A.DEPTCODE = L.DEPTCODE(+)                              \r";
                }
                SQL += "  ORDER BY a.Date3,a.Pano                                           \r";
                clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssDtl_Sheet1.RowCount = dt.Rows.Count;

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        nRow += 1;
                        if (nRow > ssDtl_Sheet1.RowCount)
                        {
                            ssDtl_Sheet1.RowCount = nRow;
                        }

                        ssDtl.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString();
                        ssDtl.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString();
                        if (dt.Rows[i]["LASTDATE"].ToString() == "")
                        {
                            ssDtl.ActiveSheet.Cells[i, 2].Text = "과초진";
                        }
                        
                        if (dt.Rows[i]["LASTDATE"].ToString() == "")
                        {
                            //이상훈 Root\VbOcs_Res.bas/Read_Dept_Chojae2() 컨버전 완료 후 진행(담당자 : 박웅규)
                            //clsOrdFunction 에 직접 만들어 씀.
                            ssDtl.ActiveSheet.Cells[i, 2].Text = clsOrdFunction.Read_Dept_Chojae(clsDB.DbCon, dt.Rows[i]["PANO"].ToString(), dt.Rows[i]["DeptCode"].ToString(), dt.Rows[i]["DRCode"].ToString());
                        }
                        ssDtl.ActiveSheet.Cells[i, 3].Text = (dt.Rows[i]["EXAM"].ToString() == "Y" ? "◎" : "");
                        ssDtl.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["DEPTCODE"].ToString();
                        ssDtl.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["DRNAME"].ToString();
                        ssDtl.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["RDATE"].ToString();
                        ssDtl.ActiveSheet.Cells[i, 7].Text = "예약";
                        ssDtl.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["TEL"].ToString();
                        ssDtl.ActiveSheet.Cells[i, 9].Text = dt.Rows[i]["SEX"].ToString();
                        ssDtl.ActiveSheet.Cells[i, 10].Text = dt.Rows[i]["ADDRESS"].ToString();
                        ssDtl.ActiveSheet.Cells[i, 11].Text = dt.Rows[i]["RDATE2"].ToString();
                        ssDtl.ActiveSheet.Cells[i, 12].Text = dt.Rows[i]["EXAM"].ToString();
                        ssDtl.ActiveSheet.Cells[i, 13].Text = dt.Rows[i]["BINAME"].ToString();
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
                //해당 시각대 전화접수자를 SELECT
                SQL = "";
                SQL += " SELECT a.Pano,b.SName,b.Tel,a.DeptCode,c.DrName                        \r";
                SQL += "      , TO_CHAR(a.RDate,'YYYY-MM-DD') RDate,RTime                       \r";
                SQL += "      , b.Juso,b.Sex,b.ZipCode1,b.ZipCode2                              \r";
                SQL += "      , KOSMOS_OCS.FC_BAS_PATIENT_JUSO(b.PANO) ADDRESS                  \r";
                SQL += "   FROM KOSMOS_PMPA.OPD_TELRESV a                                       \r";
                SQL += "      , KOSMOS_PMPA.BAS_PATIENT b                                       \r";
                SQL += "      , KOSMOS_PMPA.BAS_DOCTOR  c                                       \r";
                SQL += "  WHERE a.RDate = TO_DATE('" + VB.Left(strTime1, 10) + "','YYYY-MM-DD') \r";
                SQL += "    AND a.RTime >= '" + VB.Right(strTime1, 5) + "'                      \r";
                SQL += "    AND a.RTime < '" + VB.Right(strTime2, 5) + "'                       \r";
                SQL += "    AND a.DrCode = '" + ComFunc.LeftH(cboDoctor.Text, 4) + "'           \r";
                SQL += "    AND a.Pano = b.Pano(+)                                              \r";
                SQL += "    AND a.DrCode = c.DrCode(+)                                          \r";
                SQL += "  ORDER BY a.RDate,a.RTime,a.Pano                                       \r";
                clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                //ssDtl_Sheet1.RowCount = dt.Rows.Count;

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        nRow += 1;
                        if (nRow > ssDtl_Sheet1.RowCount)
                        {
                            ssDtl_Sheet1.RowCount = nRow;
                        }

                        ssDtl.ActiveSheet.Cells[nRow - 1, 0].Text = dt.Rows[i]["PANO"].ToString();
                        ssDtl.ActiveSheet.Cells[nRow - 1, 1].Text = dt.Rows[i]["SNAME"].ToString();
                        ssDtl.ActiveSheet.Cells[nRow - 1, 4].Text = dt.Rows[i]["DEPTCODE"].ToString();
                        ssDtl.ActiveSheet.Cells[nRow - 1, 5].Text = dt.Rows[i]["DRNAME"].ToString();
                        ssDtl.ActiveSheet.Cells[nRow - 1, 6].Text = dt.Rows[i]["RDATE"].ToString();
                        ssDtl.ActiveSheet.Cells[nRow - 1, 7].Text = "전화";
                        ssDtl.ActiveSheet.Cells[nRow - 1, 8].Text = dt.Rows[i]["TEL"].ToString();
                        ssDtl.ActiveSheet.Cells[nRow - 1, 9].Text = dt.Rows[i]["SEX"].ToString();
                        ssDtl.ActiveSheet.Cells[nRow - 1, 10].Text = dt.Rows[i]["ADDRESS"].ToString();
                        ssDtl.ActiveSheet.Cells[nRow - 1, 11].Text = dt.Rows[i]["RDATE"].ToString() + " " + dt.Rows[i]["RTIME"].ToString();
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

            ssDtl_Sheet1.RowCount = nRow;

            //예약이 가능한 시각인지 Check
            if (e.Row >= 5)
            {
                strRTime = ssSchedule.ActiveSheet.Cells[0, e.Column].Text;
                strRTime = strRTime + " " + ssSchedule.ActiveSheet.Cells[e.Row, 0].Text;
                nYeyakInwon = VB.Val(ssSchedule.ActiveSheet.Cells[e.Row, e.Column].Text) + 1;

                //예약 불가능 시간대
                if (ssSchedule.ActiveSheet.Cells[e.Row, e.Column].BackColor != Color.FromArgb(205, 250, 220))
                {
                    strMsg = strRTime + "은 예약이 불가능한 시간입니다" + "\r\n";
                    strMsg += "스케쥴에 오류가 있으면 심전도실(☎534)에" + "\r\n";
                    strMsg += "통보하여 스케쥴을 수정 바랍니다.";
                    MessageBox.Show(strMsg, "오류", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (e.Row <= nMorningNo && nYeyakInwon <= nResvInwon)
                {
                    lblRTime.Text = strRTime;
                    //해당 예약일자의 Cell BackColor를 변경
                    if (nSelRow != 0)
                    {
                        ssSchedule.ActiveSheet.Cells[nSelRow, nSelCol].BackColor = Color.FromArgb(205, 250, 220);
                    }
                    ssSchedule.ActiveSheet.Cells[e.Row, e.Column].BackColor = Color.FromArgb(255, 255, 0);
                    nSelRow = e.Row;
                    nSelCol = e.Column;
                }
                else if (e.Row > nMorningNo && nYeyakInwon <= nResvInwon2)
                {
                    lblRTime.Text = strRTime;
                    //해당 예약일자의 Cell Backcolor를 변경
                    if (nSelRow != 0)
                    {
                        ssSchedule.ActiveSheet.Cells[nSelRow, nSelCol].BackColor = Color.FromArgb(205, 250, 220);
                    }
                    ssSchedule.ActiveSheet.Cells[e.Row, e.Column].BackColor = Color.FromArgb(255, 255, 0);
                    nSelRow = e.Row;
                    nSelCol = e.Column;
                }
                else
                {
                    if (e.Row <= nMorningNo)
                    {
                        strMsg  = cboDoctor.Text + " 과장님은 예약단위당(오전) " + nResvInwon + "명이 가능합니다" + "\r\n";
                        strMsg += strRTime + "은 예약 인원 초과입니다." + "\r\n";
                        strMsg += "다른 예약일시를 선택하십시오.";
                    }
                    else
                    {
                        strMsg = cboDoctor.Text + " 과장님은 예약단위당(오후) " + nResvInwon + "명이 가능합니다" + "\r\n";
                        strMsg += strRTime + "은 예약 인원 초과입니다." + "\r\n";
                        strMsg += "다른 예약일시를 선택하십시오.";
                    }

                    if ( (clsPublic.GstrFM_Only == "Y" && ComFunc.LeftH(cboDoctor.Text, 4) == "1404") || (ComFunc.LeftH(cboDoctor.Text, 4) == "3214" ||
                          ComFunc.LeftH(cboDoctor.Text, 4) == "3218" || ComFunc.LeftH(cboDoctor.Text, 4) == "3219") )
                    {
                        if (MessageBox.Show(strMsg + "\r\n" + "예약인원을 무시하고 예약을 하시겠습니까?", "추가예약허용", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            lblRTime.Text = strRTime;
                            //해당 예약일자의 Cell Backcolor를 변경
                            if (nSelRow != 0)
                            {
                                ssSchedule.ActiveSheet.Cells[nSelRow, nSelCol].BackColor = Color.FromArgb(205, 250, 220);
                            }
                            ssSchedule.ActiveSheet.Cells[e.Row, e.Column].BackColor = Color.FromArgb(255, 255, 0);
                            nSelRow = e.Row;
                            nSelCol = e.Column;
                        }
                    }
                    else
                    {
                        MessageBox.Show(strMsg, "오류", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            string strRDate = "";
            string strRTime = "";
            string strDrCode = "";
            string strRemark = "";
            string strExam = "";
            string strResSMS = "";

            if (lblRTime.Text.Trim().Length != 16)
            {
                MessageBox.Show("예약일시가 형식에 맞지 않습니다!" + "\r\n" + "(YYYY-MM-DD HH:MI) 형식이 되어야 합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cboDoctor.Text.Trim() == "")
            {
                MessageBox.Show("예약일시를 (YYYY-MM-DD HH:MI) 형태로 입력하세요!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            strRDate = VB.Left(lblRTime.Text, 10);
            strRDate = DateTime.Parse(strRDate).ToShortDateString();

            strRTime = VB.Right(lblRTime.Text, 5);
            strDrCode = VB.Left(cboDoctor.Text, 4);
            strRemark = txtRemark.Text.Trim();

            clsOrdFunction.Pat.RDATE = strRDate.Replace("-", "");
            clsOrdFunction.Pat.RTime = strRTime;
            clsOrdFunction.Pat.RDrCode = VB.Left(cboDoctor.Text, 4);
            clsOrdFunction.Pat.ResMemo = strRemark;

            clsOrdFunction.GstrRDate = strRDate.Replace("-", "");
            clsOrdFunction.GstrRTime = strRTime;
            clsOrdFunction.GstrRDrCode = VB.Left(cboDoctor.Text, 4);
            clsOrdFunction.GstrResMemo = strRemark;

            if (chkResNoSend.Checked == true)
            {
                strResSMS = "Y";
            }
            else
            {
                strResSMS = "";
            }

            if (chkExam.Checked == true)
            {
                strExam = "Y";
            }
            else
            {
                strExam = "N";
            }

            //DRG 예약자 체크
            try
            {
                //해당 시각대 전화접수자를 SELECT
                SQL = "";
                SQL += " SELECT pano                                                        \r";
                SQL += "   FROM KOSMOS_PMPA.IPD_reserved                                    \r";
                SQL += "  WHERE ReDate = to_date('" + strRDate.Trim() + "','yyyy-mm-dd')    \r";
                SQL += "    AND Pano = '" + strPano.Trim() + "'                             \r";
                SQL += "    AND (GBCHK IS NULL OR GBCHK <> '1' )                            \r";
                SQL += "    AND GbDRG = 'Y'                                                 \r";
                clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                
                if (dt.Rows.Count >= 1)
                {
                    MessageBox.Show("예약일자에 DRG예약자 입니다", "예약불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dt.Dispose();
                    dt = null;
                    return;
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            clsOrdFunction.GstrRsvCancelFlag = "";

            //예약오더 생성
            if (clsOrdFunction.Res_Order_Insert(clsDB.DbCon, strPano, strDeptCode, strDrCode, strRDate.Replace("-",""), strRTime, strBi, strExam, strRemark, strRowId, strResSMS) == "OK")
            {
                ComFunc.MsgBox(lblName.Text + "(" + strPano + ") " + strDeptCode + " " + strRDate + " " + strRTime + " 예약 되었습니다.");                
                this.Close();
            }            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (strRowId != "")
            {
                clsDB.setBeginTran(clsDB.DbCon);

                try
                {
                    SQL = "";
                    SQL += " DELETE KOSMOS_OCS.OCS_OORDER           \r";
                    SQL += "  WHERE ROWID   = '" + strRowId + "'    \r";
                    SQL += "    AND PTNO    = '" + strPano + "'     \r";
                    SQL += "    AND GBSUNAP = '0'                    \r";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                                        
                    clsDB.setCommitTran(clsDB.DbCon);
                    MessageBox.Show("예약오더 정상 취소 되었습니다!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clsOrdFunction.GstrRsvCancelFlag = "Y";
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                }
            }

            this.Close();
        }

        private void cboDoctor_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strDrCode;

            strDrCode = ComFunc.LeftH(cboDoctor.Text, 4);

            ssSchedule_Sheet1.RowCount = 5;
            ssSchedule_Sheet1.RowCount = 54;
            fn_Yeyak_Inwon_Display(strDrCode);
        }
    }
}
