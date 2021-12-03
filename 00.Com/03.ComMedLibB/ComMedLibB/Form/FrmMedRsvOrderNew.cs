using ComBase; //기본 클래스
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
    /// Author : 박창욱
    /// Create Date : 2018.02.12
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref="FrmReservedNew.frm"/>
    public partial class FrmMedRsvOrderNew : Form
    {
        string strPano;
        string strDeptCode;
        string strDrCode;

        string strBi;
        string strRowId;

        string strRDate_Old = string.Empty;

        string[] FstrRTime = new string[0];
        string[] FstrRDate = new string[0];
        int nResvTimeGbn;
        int nResvInwon;
        int nResvInwon2;
        int nSelRow = 0;
        int nSelCol = 0;
        int nMorningNo;

        string strSMS;      //sms 동의
        string strSMS_DRUG; //'sms 안부분자(약)

        string SQL;
        DataTable dt = null;
        //DataTable dt1 = null;
        //DataTable dt2 = null;
        string SqlErr = "";     //에러문 받는 변수
        int intRowAffected = 0; //변경된 Row 받는 변수

        clsSpread SP = new clsSpread();
        ComFunc CF = new ComFunc();

        FarPoint.Win.Spread.CellType.TextCellType txt = new FarPoint.Win.Spread.CellType.TextCellType();
        public FrmMedRsvOrderNew()
        {
            InitializeComponent();
        }

        public FrmMedRsvOrderNew(string sPano, string sDeptCode, string sDrCod)
        {
            InitializeComponent();

            strPano = sPano;
            strDeptCode = sDeptCode;
            strDrCode = sDrCod;
        }

        private void FrmMedRsvOrderNew_Load(object sender, EventArgs e)
        {
            string strData;
            int j = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }

            clsOrdFunction.GstrRsvCancelFlag = "";

            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            try
            {
                SQL = "";
                SQL += " SELECT BI                                                              \r";
                SQL += "   FROM ADMIN.OPD_MASTER                                          \r";
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


            FstrRTime = new string[txtRIlsu.Text.To<int>()];
            FstrRDate = new string[txtRIlsu.Text.To<int>()];

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
            btnSearch.Enabled = true;

            if (strDeptCode == "PD")
            {
                txtRIlsu.Text = "60";
            }

            lblInfo.Text = "";
            txtRemark.Text = "";

            try
            {
                SQL = "";
                SQL += " SELECT DrCode,DrName,YTimeGbn,YInwon           \r";
                SQL += "   FROM ADMIN.BAS_DOCTOR                  \r";
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
                SQL += "   FROM ADMIN.OCS_OORDER                                               \r";
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
                        btnCancel.Enabled = false;
                    }
                    else
                    {
                        lblStat.Text = "미수납";
                        btnRegist.Enabled = true;                        
                        btnCancel.Enabled = true;
                    }

                    btnRegist.Text = "예약변경(&O)";
                    
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
                SQL += "   FROM ADMIN.BAS_PATIENT     \r";
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
                SQL += "   FROM ADMIN.IPD_NEW_MASTER                  \r";
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

            ss10_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            ss10_Sheet1.Rows[0].Visible = false;
            ss10_Sheet1.Rows[1].Visible = false;

            ss20_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            ss20_Sheet1.Rows[0].Visible = false;
            ss20_Sheet1.Rows[1].Visible = false;
            
            btnViewR_Click(sender, e);
            SET_ComboDept();

            cboDoctor.SelectedIndex = j;
            btnUpdate.Enabled = false;
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
            if (txtPano.Text == "")
                return;
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
                    ssRsvStat.ActiveSheet.RowCount = ssRsvStat.ActiveSheet.RowCount + dt.Rows.Count;
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
            //int[,,] nCNT = new int[102, 102, 3];
            int[,,] nCNT = new int[200, 200, 3];
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

            FstrRTime = new string[txtRIlsu.Text.To<int>()];
            FstrRDate = new string[txtRIlsu.Text.To<int>()];

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
            for (int i = 1; i < ssSchedule.ActiveSheet.ColumnCount; i++)
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

        private void txtRIlsu_KeyDown(object sender, KeyEventArgs e)
        {
            string strDrCode;

            if (e.KeyCode == Keys.Enter)
            {
                strDrCode = ComFunc.LeftH(cboDoctor.Text, 4);
                fn_Yeyak_Inwon_Display(strDrCode);
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
                SQL += "   FROM ADMIN.IPD_reserved                                    \r";
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
                    SQL += " DELETE ADMIN.OCS_OORDER           \r";
                    SQL += "  WHERE ROWID   = '" + strRowId + "'    \r";
                    SQL += "    AND PTNO    = '" + strPano + "'     \r";
                    SQL += "    AND GBSUNAP = '0'                   \r";
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

        private void ssSchedule_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strRTime;
            string strTime1;
            string strTime2;
            double nYeyakInwon = 0;
            string strMsg = "";
            int nRow = 0;

            txtRemark.Text = "";

            if (e.Column < 1)
                return;

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
                SQL += "   FROM ADMIN.OPD_RESERVED_NEW                            \r";
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
                ssDtl_Sheet1.RowCount = 0;

                //해당 시각대 예약자를 SELECT
                SQL = "";
                SQL += " SELECT a.Pano,b.SName,b.Tel,a.DeptCode, A.DRCODE, c.DrName         \r";
                SQL += "      , TO_CHAR(a.Date3,'MM/DD HH24:MI') RDate                      \r";
                SQL += "      , TO_CHAR(a.Date3,'YYYY-MM-DD HH24:MI') RDate2, A.EXAM        \r";
                SQL += "      , b.Juso,b.Sex,b.ZipCode1,b.ZipCode2                          \r";
                SQL += "      , TO_CHAR(L.LASTDATE,'YYYY-MM-DD') LASTDATE, a.BI             \r";
                SQL += "      , ADMIN.FC_BI_NM(a.BI) BINAME                            \r";
                SQL += "      , ADMIN.FC_BAS_PATIENT_JUSO(b.PANO) ADDRESS              \r";
                SQL += "   FROM ADMIN.OPD_RESERVED_NEW a                              \r";
                SQL += "      , ADMIN.BAS_PATIENT      b                              \r";
                SQL += "      , ADMIN.BAS_DOCTOR       c                              \r";
                SQL += "      , ADMIN.BAS_LASTEXAM     L                              \r";
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
                            ///clsOrdFunction 에 직접 만들어 씀.
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
                SQL += "      , ADMIN.FC_BAS_PATIENT_JUSO(b.PANO) ADDRESS                  \r";
                SQL += "   FROM ADMIN.OPD_TELRESV a                                       \r";
                SQL += "      , ADMIN.BAS_PATIENT b                                       \r";
                SQL += "      , ADMIN.BAS_DOCTOR  c                                       \r";
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

                        ssDtl.ActiveSheet.Cells[nRow- 1, 0].Text = dt.Rows[i]["PANO"].ToString();
                        ssDtl.ActiveSheet.Cells[nRow- 1, 1].Text = dt.Rows[i]["SNAME"].ToString();
                        ssDtl.ActiveSheet.Cells[nRow- 1, 4].Text = dt.Rows[i]["DEPTCODE"].ToString();
                        ssDtl.ActiveSheet.Cells[nRow- 1, 5].Text = dt.Rows[i]["DRNAME"].ToString();
                        ssDtl.ActiveSheet.Cells[nRow- 1, 6].Text = dt.Rows[i]["RDATE"].ToString();
                        ssDtl.ActiveSheet.Cells[nRow- 1, 7].Text = "전화";
                        ssDtl.ActiveSheet.Cells[nRow- 1, 8].Text = dt.Rows[i]["TEL"].ToString();
                        ssDtl.ActiveSheet.Cells[nRow- 1, 9].Text = dt.Rows[i]["SEX"].ToString();
                        ssDtl.ActiveSheet.Cells[nRow- 1, 10].Text = dt.Rows[i]["ADDRESS"].ToString();
                        ssDtl.ActiveSheet.Cells[nRow- 1, 11].Text = dt.Rows[i]["RDATE"].ToString() + " " + dt.Rows[i]["RTIME"].ToString();
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

            //ssDtl_Sheet1.RowCount = nRow;

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
                        strMsg = cboDoctor.Text + " 과장님은 예약단위당(오전) " + nResvInwon + "명이 가능합니다" + "\r\n";
                        strMsg += strRTime + "은 예약 인원 초과입니다." + "\r\n";
                        strMsg += "다른 예약일시를 선택하십시오.";
                    }
                    else
                    {
                        strMsg = cboDoctor.Text + " 과장님은 예약단위당(오후) " + nResvInwon + "명이 가능합니다" + "\r\n";
                        strMsg += strRTime + "은 예약 인원 초과입니다." + "\r\n";
                        strMsg += "다른 예약일시를 선택하십시오.";
                    }

                    if ((clsPublic.GstrFM_Only == "Y" && ComFunc.LeftH(cboDoctor.Text, 4) == "1404") || (ComFunc.LeftH(cboDoctor.Text, 4) == "3214" ||
                          ComFunc.LeftH(cboDoctor.Text, 4) == "3218" || ComFunc.LeftH(cboDoctor.Text, 4) == "3219"))
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
                    //2021-02-05, MG 추가 ( 전산의뢰 < 2021-113 > ) 
                    else if (clsOrdFunction.Pat.DeptCode == "OS" || clsOrdFunction.Pat.DeptCode == "MG")
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
            btnUpdate.Enabled = false;
            btnCancel.Enabled = false;
            lblRTime.Text = strRTime;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (lblRTime.Text.Trim().Length != 16)
            {
                ComFunc.MsgBox("예약일시를 (YYYY-MM-DD HH:MI)형태로 입력하세요.");
                return;
            }

            if (cboDoctor.Text.Trim() == "")
            {
                ComFunc.MsgBox("예약의사가 공란입니다.");
                return;
            }

            if (strRDate_Old == "")
            {
                ComFunc.MsgBox("예약변경을 다시해주세요");
                return;
            }

            //int i = 0;
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
                SQL = SQL + ComNum.VBLF + "  AND DeptCode='" + strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "  AND DATE3 >= TO_DATE('" + strRDate_Old + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "  AND DATE3 <  TO_DATE('" + cf.DATE_ADD(clsDB.DbCon, strRDate_Old, 1) + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "  AND TRANSDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "  AND RETDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "";

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

                if (txtPano.Text == clsOrdFunction.Pat.PtNo)
                {
                    clsOrdFunction.Pat.RDATE = VB.Left(lblRTime.Text, 10).Replace("-", "");
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
                    Cursor.Current = Cursors.Default;
                    return;
                }
                else
                {
                    ComFunc.MsgBox(lblName.Text + "님 예약일자를 정상적으로 변경하였습니다.");

                    txtPano.Text = clsOrdFunction.Pat.PtNo;
                    lblName.Text = clsOrdFunction.Pat.sName;
                    lblRTime.Text = "";
                    lblStat.Text = "";

                    if (clsOrdFunction.Pat.RDATE != "")
                    {
                        if (clsOrdFunction.GstrReserved == "OK")
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
                        lblRTime.Text = VB.Left(clsOrdFunction.Pat.RDATE, 4) + "-" + VB.Mid(clsOrdFunction.Pat.RDATE, 5, 2) + "-" + VB.Right(clsOrdFunction.Pat.RDATE, 2) + " " + clsOrdFunction.Pat.RTime;
                    }
                    else
                    {
                        btnRegist.Enabled = true;
                        btnCancel.Enabled = true;
                        btnUpdate.Enabled = false;
                    }
                }

                clsOrdFunction.GstrRDate = clsOrdFunction.Pat.RDATE.Replace("-", "");
                clsOrdFunction.GstrRTime = clsOrdFunction.Pat.RTime;
                clsOrdFunction.GstrRDrCode = VB.Left(cboDoctor.Text, 4);
                clsOrdFunction.GstrResMemo = "";

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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            Yeyak_Inwon_Display_New3();
        }

        void Yeyak_Inwon_Display_New3()
        {
            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int j = 0;
            int K = 0;
            int nRow = 0;
            int nCol = 0;
            //int nHH = 0; //시k
            //int nMM = 0; //분
            //int nTime = 0;

            //int nREAD = 0;
            //int nREAD1 = 0;
            int inx1 = 0;
            int inx2 = 0;

            int nTimeCNT = 0;
            int[,,] nCNT = new int[101, 101, 3];

            //int nResvTimeGbn = 0;
            //int nResvInwon = 0;
            //int nResvInwon2 = 0;

            string StrRDate = "";
            string strRTime = "";
            string strETime = "";
            string strGDate = "";
            //string strAmTime = "";
            //string strAMTime2 = "";
            //string strPmTime = "";
            //string strPMTime2 = "";

            //string strDeptCode = "";

            string[] strRTimeT = new string[101];
            string[] strRDateT = new string[201];

            string strDrCodes = "";
            //string strDrCode = "";

            string strJin1 = "";
            string strJin2 = "";
            string strSDate = "";
            string strTemp = "";

            ComFunc cf = new ComFunc();

            if (cboDoctor.Text.Trim() != "")
            {
                strDrCodes = strDrCodes + "'" + ComFunc.LeftH(cboDoctor.Text, 4) + "',";
            }
            if (cboDoctS0.Text.Trim() != "")
            {
                strDrCodes = strDrCodes + "'" + ComFunc.LeftH(cboDoctS0.Text, 4) + "',";
            }
            if (cboDoctS1.Text.Trim() != "")
            {
                strDrCodes = strDrCodes + "'" + ComFunc.LeftH(cboDoctS1.Text, 4) + "',";
            }
            if (cboDoctS2.Text.Trim() != "")
            {
                strDrCodes = strDrCodes + "'" + ComFunc.LeftH(cboDoctS2.Text, 4) + "',";
            }

            if (strDrCodes == "")
            {
                return;
            }

            strDrCodes = VB.Mid(strDrCodes, 0, strDrCodes.Length - 1);

            strGDate = cf.DATE_ADD(clsDB.DbCon, ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"), (int)VB.Val(txtRIlsu.Text));

            for (i = 1; i < 101; i++)
            {
                strRTimeT[i] = "";
                strRDateT[i] = "";
            }

            nRow = 5;
            nMorningNo = 6;

            nTimeCNT = nRow - 4;
            strRTimeT[nTimeCNT] = "23:59";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //예약 스케쥴을 읽어 SHEET의 상단에 Display
                SQL = "";
                SQL = "SELECT TO_CHAR(SchDate,'YYYY-MM-DD') SchDate,TO_CHAR(SchDate,'DY') Yoil ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE ";
                SQL = SQL + ComNum.VBLF + "WHERE DrCode IN (" + strDrCodes + ") ";
                SQL = SQL + ComNum.VBLF + "  AND SchDate>TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + "  AND SchDate<=TO_DATE('" + strGDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " GROUP BY TO_CHAR(SchDate,'YYYY-MM-DD'),TO_CHAR(SchDate,'DY') ";
                SQL = SQL + ComNum.VBLF + " ORDER BY SchDate ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                ss20_Sheet1.ColumnCount = dt.Rows.Count + 1;
                ss20_Sheet1.RowCount = nRow;
                ss20_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                if (dt.Rows.Count > 0)
                {
                    ss20_Sheet1.Cells[0, 1, ss20_Sheet1.RowCount - 1, ss20_Sheet1.ColumnCount - 1].Text = "";
                    ss20_Sheet1.Cells[0, 1, ss20_Sheet1.RowCount - 1, ss20_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 255);
                }
                //스케쥴을 Sheet 상단에 표시함
                nCol = 1;
                strSDate = "";
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    //일요일은 표시 안 함
                    if (i == 0)
                    {
                        strSDate = dt.Rows[i]["SchDate"].ToString().Trim();
                    }
                    if (dt.Rows[i]["Yoil"].ToString().Trim().ToUpper() != "SUN" && dt.Rows[i]["Yoil"].ToString().Trim().ToUpper() != "일")
                    {
                        nCol += 1;
                        strRDateT[nCol - 1] = dt.Rows[i]["SchDate"].ToString().Trim();
                        StrRDate = dt.Rows[i]["SchDate"].ToString().Trim();
                        strTemp = Read_Resv_Col2(StrRDate, strDrCodes);

                        ss20_Sheet1.ColumnHeader.Cells[0, nCol - 1].Text = VB.Right(StrRDate, 5) + ComNum.VBLF + "+" + (i + 1);
                        ss20_Sheet1.Cells[0, nCol - 1].Text = StrRDate;
                        ss20_Sheet1.Cells[1, nCol - 1].Text = "";
                        switch (dt.Rows[i]["Yoil"].ToString().Trim().ToUpper())
                        {
                            case "SUN":
                            case "일":
                                ss20_Sheet1.Cells[2, nCol - 1].Text = "일";
                                break;
                            case "MON":
                            case "월":
                                ss20_Sheet1.Cells[2, nCol - 1].Text = "월";
                                break;
                            case "TUE":
                            case "화":
                                ss20_Sheet1.Cells[2, nCol - 1].Text = "화";
                                break;
                            case "WED":
                            case "수":
                                ss20_Sheet1.Cells[2, nCol - 1].Text = "수";
                                break;
                            case "THU":
                            case "목":
                                ss20_Sheet1.Cells[2, nCol - 1].Text = "목";
                                break;
                            case "FRI":
                            case "금":
                                ss20_Sheet1.Cells[2, nCol - 1].Text = "금";
                                break;
                            case "SAT":
                            case "토":
                                ss20_Sheet1.Cells[2, nCol - 1].Text = "토";
                                break;
                            default:
                                ss20_Sheet1.Cells[2, nCol - 1].Text = "";
                                break;
                        }
                        strJin1 = VB.Pstr(strTemp, "^^", 1).Trim();
                        strJin2 = VB.Pstr(strTemp, "^^", 2).Trim();

                        switch (strJin1)        //오전
                        {
                            case "1":
                                ss20_Sheet1.Cells[3, nCol - 1].Text = "진료";
                                ss20_Sheet1.Cells[3, nCol - 1].BackColor = Color.FromArgb(205, 250, 220);
                                break;
                            case "2":
                                ss20_Sheet1.Cells[3, nCol - 1].Text = "수술";
                                ss20_Sheet1.Cells[3, nCol - 1].BackColor = Color.FromArgb(255, 192, 128);
                                break;
                            case "3":
                                ss20_Sheet1.Cells[3, nCol - 1].Text = "특검";
                                ss20_Sheet1.Cells[3, nCol - 1].BackColor = Color.FromArgb(255, 128, 0);
                                break;
                            case "9":
                                ss20_Sheet1.Cells[3, nCol - 1].Text = "OFF";
                                ss20_Sheet1.Cells[3, nCol - 1].BackColor = Color.FromArgb(255, 192, 192);
                                break;
                            default:
                                ss20_Sheet1.Cells[3, nCol - 1].Text = "휴진";
                                ss20_Sheet1.Cells[3, nCol - 1].BackColor = Color.FromArgb(255, 255, 255);
                                break;
                        }

                        switch (strJin2)    //오후
                        {
                            case "1":
                                ss20_Sheet1.Cells[4, nCol - 1].Text = "진료";
                                ss20_Sheet1.Cells[4, nCol - 1].BackColor = Color.FromArgb(205, 250, 220);
                                break;
                            case "2":
                                ss20_Sheet1.Cells[4, nCol - 1].Text = "수술";
                                ss20_Sheet1.Cells[4, nCol - 1].BackColor = Color.FromArgb(255, 192, 128);
                                break;
                            case "3":
                                ss20_Sheet1.Cells[4, nCol - 1].Text = "특검";
                                ss20_Sheet1.Cells[4, nCol - 1].BackColor = Color.FromArgb(255, 128, 0);
                                break;
                            case "9":
                                ss20_Sheet1.Cells[4, nCol - 1].Text = "OFF";
                                ss20_Sheet1.Cells[4, nCol - 1].BackColor = Color.FromArgb(255, 192, 192);
                                break;
                            default:
                                ss20_Sheet1.Cells[4, nCol - 1].Text = "휴진";
                                ss20_Sheet1.Cells[4, nCol - 1].BackColor = Color.FromArgb(255, 255, 255);
                                break;
                        }
                    }
                }

                ss20_Sheet1.ColumnCount = nCol;

                //의사별 기타 스케쥴을 읽어 Sheet에 표시함
                SQL = "";
                SQL = "SELECT TO_CHAR(SchDate,'YYYY-MM-DD') SchDate,STime,ETime ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE_ETC ";
                SQL = SQL + ComNum.VBLF + "WHERE DrCode IN (" + strDrCodes + " ) ";
                SQL = SQL + ComNum.VBLF + "  AND SchDate>TRUNC(SYSDATE)";
                SQL = SQL + ComNum.VBLF + "  AND SchDate<=TO_DATE('" + strGDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SchDate,STime ";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                for (i = 0; i < dt1.Rows.Count; i++)
                {
                    StrRDate = dt1.Rows[i]["SchDate"].ToString().Trim();
                    strRTime = dt1.Rows[i]["STime"].ToString().Trim();
                    strETime = dt1.Rows[i]["ETime"].ToString().Trim();

                    //예약일자 Col 찾기
                    inx1 = 0;
                    for (j = 1; j < 101; j++)
                    {
                        if (StrRDate == strRDateT[j])
                        {
                            inx1 = j;
                            break;
                        }
                    }

                    if (inx1 > 0)
                    {
                        for (j = 1; j <= nTimeCNT; j++)
                        {
                            if (string.Compare(strRTimeT[j], strRTime) >= 0 && string.Compare(strRTimeT[j], strETime) <= 0)
                            {
                                ss20_Sheet1.Cells[j + 3, inx1].BackColor = Color.FromArgb(192, 192, 192);
                            }
                        }
                    }
                }
                dt1.Dispose();
                dt1 = null;

                //예약자 인원을 COUNT
                SQL = "";
                SQL = "SELECT TO_CHAR(Date3,'YYYY-MM-DD HH24:MI') RTime,COUNT(*) CNT ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
                SQL = SQL + ComNum.VBLF + "WHERE Date3> TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + "  AND Date3<=TO_DATE('" + strGDate + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + " AND DrCode IN (" + strDrCodes + " ) ";
                SQL = SQL + ComNum.VBLF + "  AND TRANSDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "  AND RETDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "GROUP BY DATE3 ";
                SQL = SQL + ComNum.VBLF + "ORDER BY DATE3 ";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                for (i = 0; i < dt1.Rows.Count; i++)
                {
                    StrRDate = VB.Left(dt1.Rows[i]["RTime"].ToString().Trim(), 10);
                    strRTime = VB.Right(dt1.Rows[i]["RTime"].ToString().Trim(), 5);

                    //예약일자
                    inx1 = 0;
                    for (j = 1; j < 101; j++)
                    {
                        if (StrRDate == strRDateT[j])
                        {
                            inx1 = j;
                            break;
                        }
                    }

                    //예약시간 ROW
                    inx2 = 0;
                    for (K = 1; K <= nTimeCNT; K++)
                    {
                        if (string.Compare(strRTime, strRTimeT[K + 1]) < 0)
                        {
                            inx2 = K;
                            break;
                        }
                    }

                    if (inx1 > 0 && inx2 > 0)
                    {
                        nCNT[inx1, inx2, 1] += (int)VB.Val(dt1.Rows[i]["CNT"].ToString().Trim());
                    }
                }
                dt1.Dispose();
                dt1 = null;

                //병동의 퇴원자예약 미수납자를 Read
                SQL = "";
                SQL = "SELECT TO_CHAR(RDate,'YYYY-MM-DD HH24:MI') RTime,COUNT(*) CNT ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_RESERVED ";
                SQL = SQL + ComNum.VBLF + "WHERE BDate=TRUNC(SYSDATE)";
                SQL = SQL + ComNum.VBLF + "  AND RDate> TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + "  AND RDate<=TO_DATE('" + strGDate + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + " AND DrCode IN (" + strDrCodes + " ) ";
                SQL = SQL + ComNum.VBLF + "  AND GBSUNAP ='0'";
                SQL = SQL + ComNum.VBLF + "GROUP BY RDate ";
                SQL = SQL + ComNum.VBLF + "ORDER BY RDate ";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                for (i = 0; i < dt1.Rows.Count; i++)
                {
                    StrRDate = VB.Left(dt1.Rows[i]["RTime"].ToString().Trim(), 10);
                    strRTime = VB.Right(dt1.Rows[i]["RTime"].ToString().Trim(), 5);

                    //예약일자
                    inx1 = 0;
                    for (j = 1; j < 101; j++)
                    {
                        if (StrRDate == strRDateT[j])
                        {
                            inx1 = j;
                            break;
                        }
                    }

                    //예약시간 ROW
                    inx2 = 0;
                    for (K = 1; K <= nTimeCNT; K++)
                    {
                        if (string.Compare(strRTime, strRTimeT[K + 1]) < 0)
                        {
                            inx2 = K;
                            break;
                        }
                    }

                    if (inx1 > 0 && inx2 > 0)
                    {
                        nCNT[inx1, inx2, 1] += (int)VB.Val(dt1.Rows[i]["CNT"].ToString().Trim());
                    }
                }
                dt1.Dispose();
                dt1 = null;

                //전화접수 인원을 COUNT
                SQL = "";
                SQL = "SELECT TO_CHAR(RDate,'YYYY-MM-DD') RDate,RTime,COUNT(*) CNT ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_TELRESV ";
                SQL = SQL + ComNum.VBLF + "WHERE RDate> TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + "  AND RDate<=TO_DATE('" + strGDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND DrCode IN (" + strDrCodes + " ) ";
                SQL = SQL + ComNum.VBLF + "GROUP BY RDate,RTime ";
                SQL = SQL + ComNum.VBLF + "ORDER BY RDate,RTime ";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                for (i = 0; i < dt1.Rows.Count; i++)
                {
                    StrRDate = dt1.Rows[i]["RDate"].ToString().Trim();
                    strRTime = dt1.Rows[i]["RTime"].ToString().Trim();

                    //예약일자
                    inx1 = 0;
                    for (j = 1; j < 101; j++)
                    {
                        if (StrRDate == strRTimeT[K + 1])
                        {
                            inx1 = j;
                            break;
                        }
                    }

                    //예약시간 ROW
                    inx2 = 0;
                    for (K = 1; K <= nTimeCNT; K++)
                    {
                        if (string.Compare(strRTime, strRTimeT[K + 1]) < 0)
                        {
                            inx2 = K;
                            break;
                        }
                    }

                    if (inx1 > 0 && inx2 > 0)
                    {
                        nCNT[inx1, inx2, 2] += (int)VB.Val(dt1.Rows[i]["CNT"].ToString().Trim());
                    }
                }

                dt1.Dispose();
                dt1 = null;

                //자료를 Sheet에 Display
                for (i = 2; i < 101; i++)
                {
                    for (j = 1; j <= nTimeCNT; j++)
                    {
                        if (nCNT[i - 1, j, 1] != 0 || nCNT[i - 1, j, 2] != 0)
                        {
                            ss20_Sheet1.Cells[j + 4, i - 1].Text = (nCNT[i - 1, j, 1] + nCNT[i - 1, j, 2]).ToString("##0"); //당일예약+전화예약
                            ss20_Sheet1.Cells[j + 4, i - 1].Text += "(" + nCNT[i - 1, j, 2].ToString("##0") + ")";  //전화예약
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                nSelRow = 0;
                nSelCol = 0;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        string Read_Resv_Col2(string argDate,string argDrCode)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strJin1 = "";
            string strJin2 = "";
            string rtnVar = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT TO_CHAR(SchDate,'YYYY-MM-DD') SchDate,TO_CHAR(SchDate,'DY') Yoil,GbDay,GbJin, GbJin2 ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE ";
                SQL = SQL + ComNum.VBLF + "WHERE DrCode IN (" + argDrCode + ") ";
                SQL = SQL + ComNum.VBLF + "  AND SchDate=TO_DATE('" + argDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " ORDER BY SchDate ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (strJin1 == "")
                        {
                            strJin1 = dt.Rows[i]["GbJin"].ToString().Trim();
                        }
                        else
                        {
                            if (strJin1 == "1")
                            {
                                strJin1 = dt.Rows[i]["GbJin"].ToString().Trim();
                            }
                        }

                        if (strJin2 == "")
                        {
                            strJin2 = dt.Rows[i]["GbJin2"].ToString().Trim();
                        }
                        else
                        {
                            if (strJin2 == "1")
                            {
                                strJin2 = dt.Rows[i]["GbJin2"].ToString().Trim();
                            }
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                rtnVar = strJin1 + "^^" + strJin2;

                Cursor.Current = Cursors.Default;

                return rtnVar;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVar;
            }
        }



        private void SET_ComboDept()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {                                
                cboDept0.Items.Clear();
                cboDeptS0.Items.Clear();
                cboDeptS1.Items.Clear();
                cboDeptS2.Items.Clear();

                cboDept0.Items.Add(" ");
                cboDeptS0.Items.Add(" ");
                cboDeptS1.Items.Add(" ");
                cboDeptS2.Items.Add(" ");

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT PrintRanking,DeptCode ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + " WHERE DeptCode NOT IN ('MD','HR','TO','RD','II','HC','R6','HD','PT','DM','DT','OC','ER','LM','AN','OM','PC','FM') ";
                SQL = SQL + ComNum.VBLF + "ORDER BY PrintRanking ";

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
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboDept0.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim());
                        cboDeptS0.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim());
                        cboDeptS1.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim());
                        cboDeptS2.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim());
                    }
                    cboDept0.SelectedIndex = 0;
                    cboDeptS0.SelectedIndex = 0;
                    cboDeptS1.SelectedIndex = 0;
                    cboDeptS2.SelectedIndex = 0;
                }                
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        private void cboDeptS0_SelectedIndexChanged(object sender, EventArgs e)
        {
            SET_ComboDoct(cboDeptS0,cboDoctS0);
        }

        private void cboDeptS1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SET_ComboDoct(cboDeptS1, cboDoctS1);
        }

        private void cboDeptS2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SET_ComboDoct(cboDeptS2, cboDoctS2);
        }

        private void cboDept0_SelectedIndexChanged(object sender, EventArgs e)
        {
            SET_ComboDoct(cboDept0, cboDoct10);
        }

        private void SET_ComboDoct(ComboBox cboDept, ComboBox cboDoct)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                cboDoct.Items.Clear();
                cboDoct.Items.Add(" ");
                                
                SQL = "SELECT DrName,DrCode FROM ADMIN.BAS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + " WHERE DrDept1='" + VB.Trim(cboDept.Text) + "' ";                
                SQL = SQL + ComNum.VBLF + " AND SUBSTR(DrCode,3,2) <>'99' ";
                SQL = SQL + ComNum.VBLF + " AND Tour <>'Y' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY DrName ";

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
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboDoct.Items.Add(dt.Rows[i]["DrCode"].ToString().Trim() + "." + dt.Rows[i]["DrName"].ToString().Trim());                        
                    }
                    cboDoct.SelectedIndex = 0;                    
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        private void cboDoct10_Click(object sender, EventArgs e)
        {                      
        }

        void Yeyak_Inwon_Display_NEW2(string sDrCode)
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
            int[,,] nCNT = new int[100, 100, 3];
            string strDeptCode = "";

            
            strGDate = DateTime.Parse(clsPublic.GstrSysDate).AddDays(Int32.Parse(txtRIlsu.Text)).ToShortDateString();

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
                if (nRow > ss10_Sheet1.RowCount)
                {
                    ss10_Sheet1.RowCount = nRow;
                }
                ss10.ActiveSheet.Cells[nRow, 0].Text = FstrRTime[kk];
                nMorningNo = nRow;
                kk += 1;
            }

            for (int i = CF.TIME_MI(strPmTime); i <= CF.TIME_MI(strPMTime2); i += nTime)
            {
                FstrRTime[kk] = CF.TIME_MI_TIME(i);
                nRow += 1;
                if (nRow > ss10_Sheet1.RowCount)
                {
                    ss10_Sheet1.RowCount = nRow;
                }
                ss10.ActiveSheet.Cells[nRow, 0].Text = FstrRTime[kk];
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

                ss10_Sheet1.RowCount = nRow + 1;

                if (dt.Rows.Count > 0)
                {
                    nREAD = dt.Rows.Count;
                    ss10_Sheet1.ColumnCount = nREAD + 1;

                    ss10.ActiveSheet.Cells[5, 1, ss10.ActiveSheet.RowCount - 1, ss10.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 255);
                    ss10.ActiveSheet.Cells[5, 1, ss10.ActiveSheet.RowCount - 1, ss10.ActiveSheet.ColumnCount - 1].Text = "";

                    ss10.ActiveSheet.Cells[0, 1, ss10.ActiveSheet.RowCount - 1, ss10.ActiveSheet.ColumnCount - 1].Border = new LineBorder(Color.Black, 0);

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
                            ss10.ActiveSheet.Columns.Get(nCol).Label = VB.Right(strRDate, 5) + "\r\n" + "+" + (i + 1);
                            ss10.ActiveSheet.Cells[0, nCol].Text = strRDate;
                            ss10.ActiveSheet.Cells[1, nCol].Text = dt.Rows[i]["GBJIN"].ToString().Trim();
                            switch (dt.Rows[i]["YOIL"].ToString().Trim().ToUpper())
                            {
                                case "SUN":
                                    ss10.ActiveSheet.Cells[2, nCol].Text = "일";
                                    break;
                                case "일":
                                    ss10.ActiveSheet.Cells[2, nCol].Text = "일";
                                    break;
                                case "MON":
                                    ss10.ActiveSheet.Cells[2, nCol].Text = "월";
                                    break;
                                case "월":
                                    ss10.ActiveSheet.Cells[2, nCol].Text = "월";
                                    break;
                                case "TUE":
                                    ss10.ActiveSheet.Cells[2, nCol].Text = "화";
                                    break;
                                case "화":
                                    ss10.ActiveSheet.Cells[2, nCol].Text = "화";
                                    break;
                                case "WED":
                                    ss10.ActiveSheet.Cells[2, nCol].Text = "수";
                                    break;
                                case "수":
                                    ss10.ActiveSheet.Cells[2, nCol].Text = "수";
                                    break;
                                case "THU":
                                    ss10.ActiveSheet.Cells[2, nCol].Text = "목";
                                    break;
                                case "목":
                                    ss10.ActiveSheet.Cells[2, nCol].Text = "목";
                                    break;
                                case "FRI":
                                    ss10.ActiveSheet.Cells[2, nCol].Text = "금";
                                    break;
                                case "금":
                                    ss10.ActiveSheet.Cells[2, nCol].Text = "금";
                                    break;
                                case "SAT":
                                    ss10.ActiveSheet.Cells[2, nCol].Text = "토";
                                    break;
                                case "토":
                                    ss10.ActiveSheet.Cells[2, nCol].Text = "토";
                                    break;
                                default:
                                    ss10.ActiveSheet.Cells[2, nCol].Text = "";
                                    break;
                            }

                            switch (dt.Rows[i]["GBJIN"].ToString()) //오전
                            {
                                case "1":
                                    ss10.ActiveSheet.Cells[3, nCol].Text = "진료";
                                    ss10.ActiveSheet.Cells[5, nCol, nMorningNo, nCol].BackColor = Color.FromArgb(205, 250, 220);
                                    break;
                                case "2":
                                    ss10.ActiveSheet.Cells[3, nCol].Text = "수술";
                                    ss10.ActiveSheet.Cells[5, nCol, nMorningNo, nCol].BackColor = Color.FromArgb(255, 192, 128);
                                    break;
                                case "3":
                                    ss10.ActiveSheet.Cells[3, nCol].Text = "특검";
                                    ss10.ActiveSheet.Cells[5, nCol, nMorningNo, nCol].BackColor = Color.FromArgb(255, 128, 0);
                                    break;
                                case "9":
                                    ss10.ActiveSheet.Cells[3, nCol].Text = "OFF";
                                    ss10.ActiveSheet.Cells[5, nCol, nMorningNo, nCol].BackColor = Color.FromArgb(255, 192, 192);
                                    break;
                                default:
                                    ss10.ActiveSheet.Cells[3, nCol].Text = "휴진";
                                    ss10.ActiveSheet.Cells[5, nCol, nMorningNo, nCol].BackColor = Color.White;
                                    break;
                            }

                            switch (dt.Rows[i]["GBJIN2"].ToString())    //오후
                            {
                                case "1":
                                    ss10.ActiveSheet.Cells[4, nCol].Text = "진료";
                                    ss10.ActiveSheet.Cells[nMorningNo + 1, nCol, ss10_Sheet1.RowCount - 1, nCol].BackColor = Color.FromArgb(205, 250, 220);
                                    break;
                                case "2":
                                    ss10.ActiveSheet.Cells[4, nCol].Text = "수술";
                                    ss10.ActiveSheet.Cells[nMorningNo + 1, nCol, ss10_Sheet1.RowCount - 1, nCol].BackColor = Color.FromArgb(255, 192, 128);
                                    break;
                                case "3":
                                    ss10.ActiveSheet.Cells[4, nCol].Text = "특검";
                                    ss10.ActiveSheet.Cells[nMorningNo + 1, nCol, ss10_Sheet1.RowCount - 1, nCol].BackColor = Color.FromArgb(255, 128, 0);
                                    break;
                                case "9":
                                    ss10.ActiveSheet.Cells[4, nCol].Text = "OFF";
                                    ss10.ActiveSheet.Cells[nMorningNo + 1, nCol, ss10_Sheet1.RowCount - 1, nCol].BackColor = Color.FromArgb(255, 192, 192);
                                    break;
                                default:
                                    ss10.ActiveSheet.Cells[4, nCol].Text = "휴진";
                                    ss10.ActiveSheet.Cells[nMorningNo + 1, nCol, ss10.ActiveSheet.RowCount - 1, nCol].BackColor = Color.White;
                                    break;
                            }
                        }
                    }
                    ss10_Sheet1.ColumnCount = nCol;
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            //ss10_Sheet1.RowCount = nRow;
            //ss10.ActiveSheet.Cells[5, 2, ss10.ActiveSheet.RowCount - 1, ss10.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 255);
            //ss10.ActiveSheet.Cells[5, 2, ss10.ActiveSheet.RowCount - 1, ss10.ActiveSheet.ColumnCount - 1].Text = "";



            //ss10.ActiveSheet.Cells[2, 1, ss10.ActiveSheet.RowCount - 1, ss10.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 255);
            ss10.ActiveSheet.Rows.Get(1).Border = new LineBorder(Color.Black, 1, true, true, true, true);



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
                                    ss10.ActiveSheet.Cells[j + 4, inx1 + 1].BackColor = Color.FromArgb(192, 192, 192);
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
                            //if (Convert.ToDateTime(strRTime) < Convert.ToDateTime(FstrRTime[k + 1]))
                            if (Convert.ToDateTime(strRTime) <= Convert.ToDateTime(FstrRTime[k]))
                            {
                                inx2 = k;
                                break;
                            }
                        }

                        if (inx1 >= 0 && inx2 >= 0)
                        {
                            nCNT[inx1, inx2, 1] = nCNT[inx1, inx2, 1] + Int32.Parse(dt.Rows[i]["CNT"].ToString());
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
                        inx1 = 0;
                        for (int j = 0; j < 100; j++)
                        {
                            if (strRDate == FstrRDate[j])
                            {
                                inx1 = j;
                            }
                        }

                        //예약시간 Row
                        inx2 = 0;
                        for (int k = 0; k < nTimeCNT; k++)
                        {
                            if (Convert.ToDateTime(strRTime) < Convert.ToDateTime(FstrRTime[k + 1]))
                            {
                                inx2 = k;
                                break;
                            }
                        }

                        if (inx1 > 0 && inx2 > 0)
                        {
                            nCNT[inx1, inx2, 1] = nCNT[inx1, inx2, 1] + Int32.Parse(dt.Rows[i]["CNT"].ToString());
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
                        strRDate = dt.Rows[i]["RTIME"].ToString();
                        strRTime = dt.Rows[i]["RTIME"].ToString();

                        //예약일자
                        inx1 = 0;
                        for (int j = 0; j < 100; j++)
                        {
                            if (strRDate == FstrRDate[j])
                            {
                                inx1 = j;
                            }
                        }

                        //예약시간 Row
                        inx2 = 0;
                        for (int k = 0; k < nTimeCNT; k++)
                        {
                            if (Convert.ToDateTime(strRTime) < Convert.ToDateTime(FstrRTime[k + 1]))
                            {
                                inx2 = k;
                                break;
                            }
                        }

                        if (inx1 > 0 && inx2 > 0)
                        {
                            nCNT[inx1, inx2, 1] = nCNT[inx1, inx2, 1] + Int32.Parse(dt.Rows[i]["CNT"].ToString());
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
            for (int i = 1; i < ss10.ActiveSheet.ColumnCount; i++)
            {
                for (int j = 0; j < nTimeCNT; j++)
                {
                    if (nCNT[i - 1, j, 0] != 0 || nCNT[i - 1, j, 1] != 0)
                    {
                        ss10.ActiveSheet.Cells[j + 5, i].Text = string.Format("{0:##0}", nCNT[i - 1, j, 1] + nCNT[i - 1, j, 2]); //당일예약+전화예약
                        ss10.ActiveSheet.Cells[j + 5, i].Text += "(" + string.Format("{0:##0}", nCNT[i - 1, j, 2]) + ")";
                    }
                }
            }

            nSelRow = 0;
            nSelCol = 0;
        }

        private void cboDoct10_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strDrCode;

            strDrCode = ComFunc.LeftH(cboDoctor.Text, 4);

            if (cboDoct10.Text != "")
            {
                Yeyak_Inwon_Display_NEW2(strDrCode);
            }
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
